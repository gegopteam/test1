using UnityEngine;
using System.Collections.Generic;
using UnityEngine.UI;
using AssemblyCSharp;
using System.Collections;
using UnityEngine;
using DG.Tweening;

public class UIDailySign : MonoBehaviour, IUiMediator
{

	public Image Arrow;

	public Image LightSelected;

	//public Image BtnStart;

	public Text ContinueLoginDay;

	public Text TxtVipReward;

	public Text TxtVipLevel;

	//public Text TxtVipInfo;

	const int DeltaAngle = 30;

	public Sprite[] IconVipLevel;

	List< FiProperty > mTotalRewards = null;

	public GameObject FinishAnimate;

	public Image ImgVipLevel;

	public GameObject LightContent;

	public Button startBtn;

	void Test ()
	{
		mTotalRewards = new List<FiProperty> ();
		mCurrentDay = 1;
		mContinuedDays = 3;

		FiProperty npp1 = new FiProperty ();
		npp1.type = FiPropertyType.GOLD;
		npp1.value = 2000;

		FiProperty Vip = new FiProperty ();
		Vip.type = FiPropertyType.GIFT_VIP9;
		Vip.value = 1;

		FiProperty continue1 = new FiProperty ();
		continue1.type = FiPropertyType.DIAMOND;
		continue1.value = 1000;

		mTotalRewards.Add (npp1);
		mTotalRewards.Add (Vip);
		mTotalRewards.Add (continue1);
	}

	public Text TxtContentTip;

	public Text TxtVip0Info;
	public Text TxtVip9Info;
	public Text TxtVip1_8Info;

	// Use this for initialization
	void Start ()
	{

		GameObject sign = GameObject.FindGameObjectWithTag ("MainCamera");
		//Debug.Log (sign.name);
		Camera signCamera = sign.GetComponent<Camera> ();
		Canvas mainCanvas = transform.GetComponentInChildren<Canvas> ();
		mainCanvas.worldCamera = signCamera;
		//Test ();

		MyInfo nUserInfo = (MyInfo)Facade.GetFacade ().data.Get (FacadeConfig.USERINFO_MODULE_ID);
		//nUserInfo.levelVip = 1;

		if (nUserInfo.levelVip == 0) {
			TxtVip1_8Info.gameObject.SetActive (false);
			TxtVipLevel.gameObject.SetActive (false);
			TxtVipReward.gameObject.SetActive (false);
			TxtVip0Info.gameObject.SetActive (true);
			TxtContentTip.text = "當前             暫無每日獎勵領取";
			TxtVip0Info.transform.Find ("TxtCharge").GetComponent<Text> ().text = (10 - nUserInfo.topupSum).ToString ();
		} else if (nUserInfo.levelVip == 9) {

			TxtVip1_8Info.gameObject.SetActive (false);
			TxtVip9Info.gameObject.SetActive (true);

		} else {
			TxtVip1_8Info.gameObject.SetActive (true);
			TxtVipLevel.text = nUserInfo.levelVip.ToString ();
			TxtVipReward.text = FiPropertyType.GetVipSignReward (nUserInfo.levelVip);
			TxtVip1_8Info.transform.Find ("InfoVipLevel").GetComponent<Image> ().sprite = IconVipLevel [nUserInfo.levelVip + 1];
			TxtVip1_8Info.transform.Find ("TxtCost").GetComponent<Text> ().text = (VipSliderContrl.GetMaxValue ((byte)nUserInfo.levelVip) - nUserInfo.topupSum).ToString ();
		}
		ImgVipLevel.sprite = IconVipLevel [nUserInfo.levelVip];

		Facade.GetFacade ().ui.Add (FacadeConfig.UI_SIGN_IN_MODULE_ID, this);
	}

	int mContinuedDays = 0;
	int mCurrentDay = 1;
	//float nRotateAngle   = 0;

	public void OnExit ()
	{
		Destroy (gameObject);
	}

	int GetRotateAngle (int nDayIndex)
	{
		switch (nDayIndex) {
		case 1:
		case 3:
		case 5:
		case 6:
			return 15;
		case 7:
			return 135;
		case 2:
		case 4:
			return 75;
		}
		return 0;
	}


	bool isStart = false;

	public void StartRotation ()
	{
		if (mTotalRewards == null || isStart)
			return;
		isStart = true;
		LightSelected.gameObject.SetActive (true);
		FinishAnimate.SetActive (false);
		MyInfo myInfo = (MyInfo)Facade.GetFacade ().data.Get (FacadeConfig.USERINFO_MODULE_ID);
		myInfo.markSignInDay ();

		float nRotateAngle = GetRotateAngle (mCurrentDay);
		float nDuration = 4.0f;
		int nRepeatCount = 4;
		Tweener nReturn = Arrow.transform.DORotate (new Vector3 (0, 0, -(nRotateAngle + 360 * nRepeatCount)), nDuration, RotateMode.FastBeyond360);
		//LightSelected.transform.DORotate( new Vector3( 0 , 0 , -(nRotateAngle + 360*nRepeatCount) + 15 ) , nDuration , RotateMode.FastBeyond360 );
		Invoke ("OnRotateComplete", nDuration);
		//DoSlowStart();
		StartCoroutine (SetBtnActive ());
	}

	IEnumerator SetBtnActive ()
	{
		yield return new WaitForSeconds (3.8f);
		startBtn.enabled = false;
	}

	float mLastAngle = 0;

	void Update ()
	{
		if (!isStart)
			return;
		float nAngle = Arrow.transform.localEulerAngles.z;
		float nRotateAngle = (int)(nAngle / 30 + 1) * 30;

		Transform nCurLight = LightContent.transform.Find ("Light" + nRotateAngle);

		if (nCurLight) {
			nCurLight.gameObject.SetActive (true);
			UILightAnimate nLightScript = nCurLight.gameObject.GetComponent<UILightAnimate> ();
			if (nLightScript == null) {
				nLightScript = nCurLight.gameObject.AddComponent<UILightAnimate> ();
			}
			nLightScript.StartAnimate ();
		}
		mLastAngle = nRotateAngle;
		LightSelected.transform.eulerAngles = new Vector3 (0, 0, nRotateAngle);
	}

	void OnRotateComplete ()
	{
		isStart = false;
		float nRotateAngle = GetRotateAngle (mCurrentDay);
		FinishAnimate.transform.localEulerAngles = new Vector3 (0, 0, -nRotateAngle);
		FinishAnimate.SetActive (true);
		Invoke ("DisplayReward", 2.0f);
	}

	void DisplayReward ()
	{
		ShowReward (mTotalRewards);
	}

	void CloseWindow ()
	{
		Destroy (this.gameObject);
	}

	//奖励领取完毕了，3s后关闭签到窗口
	void OnRewardCompelete ()
	{
		Debug.LogError ("---------------OnRewardCompelete--------------");
		CloseWindow ();
	}

	void ShowReward (List<FiProperty> nPorpList)
	{
		UnityEngine.GameObject Window = UnityEngine.Resources.Load ("MainHall/Gift/SignRewardWindow")as UnityEngine.GameObject;
		GameObject WindowClone = UnityEngine.GameObject.Instantiate (Window);
		UISignReward reward = WindowClone.GetComponent<UISignReward> ();
		if (mContinuedDays == 3 || mContinuedDays == 7 || mContinuedDays == 15) {
			reward.SetRewardData (nPorpList, true);
		} else {
			reward.SetRewardData (nPorpList);
		}
		reward.OnRewardCompelete = OnRewardCompelete;
	}

	public void OnRecvData (int nType, object nData)
	{
		if (nType == FiEventType.RECV_SIGN_IN_AWARD_RESPONSE) {
			FiSignInAwardResponse nResponse = (FiSignInAwardResponse)nData;
			//领取失败，关闭界面
			if (nResponse.result != 0) {
				//Debug.LogError ( "-----------FiEventType.RECV_SIGN_IN_AWARD_RESPONSE--------------" );
				Invoke ("CloseWindow", 1.0f);
				return;
			}

			//当signIn字段都为0时，说明是领取连续签到礼包的消息
			if (mTotalRewards != null) {
				for (int i = 0; i < nResponse.properties.Count; i++) {
					mTotalRewards.Add (nResponse.properties [i]);
				}
				return;
			}

			//领取当天礼包的结果 设置领取进度，弹出领取奖励的界面
			mContinuedDays++;
			mTotalRewards = nResponse.properties;
			if (nResponse.singIn.status == 0 || mContinuedDays == 3 || mContinuedDays == 7 || mContinuedDays == 15) {
				Debug.LogError ("---------SendGetSignAwardRequest1--------");
				//如果条件触发了连续签的礼包
				if (nResponse.singIn.day == -3 || nResponse.singIn.day == -7 || nResponse.singIn.day == -15) {
					Debug.LogError ("---------SendGetSignAwardRequest2--------" + nResponse.singIn.day);
					Facade.GetFacade ().message.signIn.SendGetSignAwardRequest (nResponse.singIn.day);
					return;
				}
			}
		}
	}

	public void OnOpenVip ()
	{
		UIHallCore nHall = (UIHallCore)Facade.GetFacade ().ui.Get (FacadeConfig.UI_HALL_MODULE_ID);
		nHall.ToVip ();
	}

	public void OnRelease ()
	{
		
	}

	public void OnInit ()
	{
		/**签到数据处理*/
		//此处相当于Start  //0:连续天数 1:未领取 2:已领取 3:错过
		MyInfo myInfo = (MyInfo)Facade.GetFacade ().data.Get (FacadeConfig.USERINFO_MODULE_ID);
		List<FiDailySignIn> signInArray = myInfo.signInArray;

		//Debug.LogError ( signInArray + "-----------------------" +  myInfo.levelVip );

		TxtVipLevel.text = myInfo.levelVip.ToString ();

		if (signInArray == null)
			return;
		for (int i = 0; i < signInArray.Count; i++) {
			//statu == 0 的时候，表明有连续签到的数据，更新连续签到的进度
			if (signInArray [i].status == 0) {
				int day = System.Math.Abs (signInArray [i].day);
				mContinuedDays = day;
				ContinueLoginDay.text = day.ToString ();
				//Debug.LogError ( "-----------mContinuedDays------------ / " + mContinuedDays );
				continue;
			}

			//设置1-7每天的签到情况和展示
			int nWeekDay = signInArray [i].day;
			//Debug.LogError ( nWeekDay +  "signInArray [i].status======>" + signInArray [i].status );
			if (nWeekDay > 0 && nWeekDay <= 7) {
				
				if (signInArray [i].status == 1 && signInArray [i].status > 0) { 
					//还没有领取的，那么就是当天的， 服务器会下发{ -3, 1  } 这个消息，表示有没有领取的 连续签到，所以我们判断status > 0 ，来区分是连续签到还是平时奖励数据
					mCurrentDay = nWeekDay;
					Debug.LogError ("my current day======>" + mCurrentDay);
				}
			}
		}

		if (mCurrentDay > 0) {
			Facade.GetFacade ().message.signIn.SendGetSignAwardRequest (mCurrentDay);
		}
	}

	void OnDestroy ()
	{
		Facade.GetFacade ().ui.Remove (FacadeConfig.UI_SIGN_IN_MODULE_ID);
	}


}
