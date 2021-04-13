using UnityEngine;
using System.Collections.Generic;
using UnityEngine.UI;
using AssemblyCSharp;
using System.Collections;
using UnityEngine;

/// <summary>
/// User interface sign.每日登录
/// </summary>


public class ReSignDay
{
	public static bool isFristActive = true;
	//是否第一次打开活动
	public static bool isFristPrefapay = true;
	public static bool isfrist = true;
	public static bool isfristhow = true;
	public static bool isSignComplet = false;
	public static bool isContinueSign = false;
	//是否签到完成
	public static int reday = 0;
	public static int redayback = 0;
	public static int continuedday = 0;
	//签到后的连续签到天数累计
	//这个天数是为了显示接收服务器给的天数不做任何改变
	public static int signrewarday;
	public static List<int> suppleday = new List<int> ();
	//	public static int proid = 1000;
	//	public static int count =10;
}

public class UISign : MonoBehaviour , IUiMediator
{
	public Animator threeAni;
	public Animator sixAni;
	public Animator eightAni;
	public Animator fiftyAni;

	private const int RECV_SIGN_REWARD = 0;
	private const int RECV_CONTINUE_REWARD = 1;
	private const int RECV_REWARD_OVER = 2;

	int mSignState = RECV_SIGN_REWARD;

	public DaySignInfo[] weekImage;
	public Button[] weekbutton;
	public Slider signSlider;
	public static string toolName;
	public static int toolMuch;

	private GameObject sign;
	private Camera signCamera;
	private Canvas mainCanvas;

	private int mCurrentDay = 0;
	//连续签到的日期
	private int mContinuedDays = 0;

	public GameObject m3DayGiftBox;
	public GameObject m8DayGiftBox;
	public GameObject m7DayGiftBox;
	public GameObject m15DayGiftBox;

	public GameObject mOpened8DayTip;
	public GameObject mOpened3DayTip;
	public GameObject mOpened7DayTip;
	public GameObject mOpened15DayTip;

	public Image ImgVipLevel;
	public Text TxtVipLevel;
	public Text txtViprewardlevel;
	public Text TxtVipReward;
	public Sprite[] IconVipLevel;
	public Image Vipgift;
	public Image SignOnclick;

	public Text TxtContentTip;

	public Text TxtVip0Info;
	public Text TxtVip9Info;



	public Image[] contincueicon;
	public Text[] conticuevalue;
	public Text mcontinueday;
	public int SignDayReward;
	//	private List<int> suppleday;
	//	public Text TxtVip1_8Info;

	public List<SignCofing> signList = new List<SignCofing> ();

	public int SignSupplement = 1;
	//补签天数

	public static UISign instance;

	private List<FiRewardStructure> rewardinfo;

	private List<FiRewardStructure> signrewardinfo = new List<FiRewardStructure> ();
	private List<FiRewardStructure> conyticueinfo = new List<FiRewardStructure> ();

	public DaySignInfo currentsign = null;

	void Awake ()
	{

		sign = GameObject.FindGameObjectWithTag ("MainCamera");
		signCamera = sign.GetComponent<Camera> ();
		mainCanvas = transform.GetComponentInChildren<Canvas> ();
		mainCanvas.worldCamera = signCamera;
		mainCanvas.planeDistance = 80;
		mainCanvas.sortingOrder = 10;	
		instance = this;
		LoginUtil.GetIntance ().onshowsignchange += ShowSignOnclick;
	}

	// Use this for initialization
	void Start ()
	{

		rewardinfo = Facade.GetFacade ().message.reward.GetRewardInfo (RewardMsgHandle.SIGN_IN_REWARD_TYPE);

		threeAni.enabled = false;
		sixAni.enabled = false;
		fiftyAni.enabled = false;
		eightAni.enabled = false;
		Facade.GetFacade ().ui.Add (FacadeConfig.UI_SIGN_IN_MODULE_ID, this);
		UIColseManage.instance.ShowUI (this.gameObject);
	}

	public void GetSignrewardinfo ()
	{
		for (int i = 0; i < rewardinfo.Count; i++) {
			if (rewardinfo [i].TaskID >= 0) {
				signrewardinfo.Add (rewardinfo [i]);
			} else {
				conyticueinfo.Add (rewardinfo [i]);
			}
		}
	}

	void OnRecv3DayGift ()
	{
		mOpened3DayTip.SetActive (true);
		if (threeAni != null) {
			threeAni.enabled = false;
		}

	}

	void OnRecv7DayGift ()
	{
		mOpened7DayTip.SetActive (true);
		mOpened3DayTip.SetActive (true);
		if (sixAni != null) {
			sixAni.enabled = false;
		}
	}

	void OnRecv8DayGift ()
	{
		if (eightAni != null) {
			eightAni.enabled = false;
		}
		mOpened8DayTip.SetActive (true);
		mOpened7DayTip.SetActive (true);
		mOpened3DayTip.SetActive (true);
	}


	void OnRecv15DayGift ()
	{
		if (fiftyAni != null) {
			fiftyAni.enabled = false;
		}
		mOpened15DayTip.SetActive (true);
		mOpened8DayTip.SetActive (true);
		mOpened7DayTip.SetActive (true);
		mOpened3DayTip.SetActive (true);

	}


	public void OnInit ()
	{
		/**签到数据处理*/
		OnInitData ();
		//此处相当于Start  //0:连续天数 1:未领取 2:已领取 3:错过
		MyInfo myInfo = (MyInfo)Facade.GetFacade ().data.Get (FacadeConfig.USERINFO_MODULE_ID);
		//		List<FiDailySignIn> signInArray = new List<FiDailySignIn> ();
		//		TestAddSignDay (signInArray);
		List<FiDailySignIn> signInArray = myInfo.signInArray;

		for (int i = 0; i < signInArray.Count; i++) {
//			//Debug.LogError ("signArraystatue" + signInArray [i].status + "signarrayday" + signInArray [i].day);
		}

		if (signInArray == null)
			return;


		ReSignDay.isfrist = true;
//		ReSignDay.isContinueSign = false;
		ReSignDay.isfristhow = true;
		ReSignDay.isSignComplet = true;
		ReSignDay.suppleday.Clear ();
		ReSignDay.reday = 0;
//		//Debug.LogError ("____________________________________ReSignDay.reday_____________________" + ReSignDay.reday + ReSignDay.suppleday.Count);
		SignOnclick.sprite = UIHallTexturers.instans.SignRewardType [5];
		SignOnclick.gameObject.GetComponent<Button> ().enabled = true;

		for (int i = 0; i < signInArray.Count; i++) {
			//statu == 0 的时候，表明有连续签到的数据，更新连续签到的进度
			if (signInArray [i].day < 0) {
				int day = System.Math.Abs (signInArray [i].day);
				mContinuedDays = day;
	
				if (signInArray [i].status == 2) {
					InitRecvDayStatus (mContinuedDays);
					SetSignProgress (mContinuedDays, true);
				} else {
					//InitRecvDayStatus (mContinuedDays);//这里等以后服务器修改确定状态为1的话玩家连续签到可自己控制
					SetSignProgress (mContinuedDays);
					if (mContinuedDays != 3 || mContinuedDays != 6 || mContinuedDays != 8 || mContinuedDays != 16) {
						InitRecvDayStatus (mContinuedDays);
					}
				}




				continue;
			}

			int nWeekDay = signInArray [i].day;
			if (nWeekDay > 0 && nWeekDay <= 7) {
				if (signInArray [i].status == 2) { //已经领取啦
					weekImage [nWeekDay - 1].Mask.gameObject.SetActive (true);
					weekImage [nWeekDay - 1].Sign.gameObject.SetActive (true);
					weekImage [nWeekDay - 1].NoSign.gameObject.SetActive (false);
					//					weekImage [nWeekDay - 1].signbutton.enabled = false;
				} else if (signInArray [i].status == 3) { // 错过的
					if (ReSignDay.isfrist) {
						ReSignDay.reday = nWeekDay;
						ReSignDay.isfrist = false;
					}
					ReSignDay.suppleday.Add (nWeekDay);
					weekImage [nWeekDay - 1].Mask.gameObject.SetActive (true);
					weekImage [nWeekDay - 1].Sign.gameObject.SetActive (false);
					weekImage [nWeekDay - 1].NoSign.gameObject.SetActive (true);
				} else if (signInArray [i].status == 1 && signInArray [i].status > 0) { 
					//还没有领取的，那么就是当天的， 服务器会下发{ -3, 1  } 这个消息，表示有没有领取的 连续签到，所以我们判断status > 0 ，来区分是连续签到还是平时奖励数据
					mCurrentDay = nWeekDay;
					weekImage [nWeekDay - 1].onclick.gameObject.SetActive (true);
					SignDayReward = mCurrentDay;
					ReSignDay.isSignComplet = false;
				}
			}
		}
		//记录已签连续签到的值
		if (ReSignDay.continuedday != 0) {
//			//Debug.LogError ("ReSignDay.continuedday" + ReSignDay.continuedday);
			mContinuedDays = mContinuedDays + ReSignDay.continuedday;
			mcontinueday.text = mContinuedDays.ToString ();
		} else {
			mcontinueday.text = mContinuedDays.ToString ();
		}
		if (int.Parse (mcontinueday.text) >= 1) {
			SetSignProgress (int.Parse (mcontinueday.text));
		}
		if (ReSignDay.isContinueSign) {
			InitRecvDayStatus (mContinuedDays);
		}


		//		//Debug.LogError ("mCurrentDay" + mCurrentDay);
		//		weekImage [mCurrentDay].onclick.gameObject.SetActive (true);

		//初始化签到显示
		for (int i = 0; i < signList.Count; i++) {
			weekImage [i].SignShowInit (signList [i]);
			if (i <= signInArray.Count - 1) {
				weekImage [i].statue = signInArray [i].status;
			}
		}
//		//Debug.LogError ("____________________________________ReSignDay.reday11111111_____________________" + ReSignDay.reday + ReSignDay.suppleday.Count);
		ShowSignOnclick ();
		//初始化签到按钮
		//		foreach (DaySignInfo s in weekImage) {
		//			s.signbutton.onClick.AddListener (  delegate() {
		//				SignButtonOnClick(s);
		//			});
		//		}
		//用foreach就没问题用了循环就不行 思考下
		//		for (int i = 0; i < weekbutton.Length; i++) {
		//			weekbutton[i].onClick.AddListener (  delegate() {
		//				SignButtonOnClick(weekbutton[i].gameObject);
		//			});
		//		}

		//
		//		for (int i = mCurrentDay; i < signList.Count; i++) {
		//			
		//			//Debug.LogError ("ssssssssssssss" + mCurrentDay);
		//			weekImage [i].signbutton.enabled = false;
		//		}
		//


		if (myInfo.levelVip == 0) {
			//			TxtVip1_8Info.gameObject.SetActive (false);
			//			TxtVipLevel.gameObject.SetActive (false);
			TxtVipLevel.text = myInfo.levelVip.ToString ();
			TxtVipReward.gameObject.SetActive (false);
			TxtVip0Info.gameObject.SetActive (true);
			TxtVip0Info.text = "无礼包";
			Vipgift.gameObject.SetActive (false);

			//		}else if (myInfo.levelVip == 9) {
			////			TxtVip1_8Info.gameObject.SetActive (false);
			//			TxtVip9Info.gameObject.SetActive (true);
		} else {
			//			TxtVip1_8Info.gameObject.SetActive (true);
			TxtVipLevel.text = myInfo.levelVip.ToString ();
			txtViprewardlevel.text = myInfo.levelVip.ToString ();
			TxtVipReward.text = FiPropertyType.GetVipSignReward (myInfo.levelVip);
			//			TxtVip1_8Info.transform.Find ("InfoVipLevel").GetComponent<Image> ().sprite = IconVipLevel [myInfo.levelVip + 1];
			//			TxtVip1_8Info.transform.Find ("TxtCost").GetComponent<Text> ().text = (VipSliderContrl.GetMaxValue ((byte)myInfo.levelVip) - myInfo.topupSum).ToString ();
		}
		ImgVipLevel.sprite = IconVipLevel [myInfo.levelVip];


	}

	public void OnOpenDetail ()
	{
		GameObject Window = Resources.Load ("Window/VIP")as GameObject;
		Instantiate (Window);
	}

	// Update is called once per frame
	void Update ()
	{

	}

	void test_1send ()
	{
		FiSignInAwardResponse nRes = new FiSignInAwardResponse ();
		nRes.result = 0;
		nRes.singIn.day = -3;
		nRes.singIn.status = 0;
		FiProperty npp = new FiProperty ();
		npp.type = FiPropertyType.FISHING_EFFECT_VIOLENT;
		npp.value = 99;
		nRes.properties.Add (npp);
		NetControl.instance ().dispatchEvent (FiEventType.RECV_SIGN_IN_AWARD_RESPONSE, nRes);
	}

	/**/

	void CloseWindow ()
	{
		Destroy (this.gameObject);
	}

	int GetContinueData (int nInput)
	{
		if (nInput == 3) {
			return -3;
		} else if (nInput == 6) {
			return -6;
		} else if (nInput == 8) {
			return -8;
		} else if (nInput == 15) {
			return -15;
		}
		return 0;
	}

	//签到按钮
	public void RecevieButton ()
	{


		MyInfo nUserInfo = (MyInfo)Facade.GetFacade ().data.Get (FacadeConfig.USERINFO_MODULE_ID);

		//if (nUserInfo.isBindPhone==0)
		//{
		//	GetColdManager.OnOpenGetColdClick();

		//}else if (nUserInfo.isBindPhone == 1)
		//{
		mSignState = RECV_SIGN_REWARD;
		Debug.Log(" ~~~~~ !!!!! UISign !!!!!~~~~~ currentsign.statue = "+ currentsign.statue);
		if (currentsign.statue == 3) {
			SignDayReward = ReSignDay.reday;
			string path = "Window/WindowSign";
			GameObject WindowClone = AppControl.OpenWindow (path);
			DaySupplement daysupplement = WindowClone.GetComponent<DaySupplement> ();
			daysupplement.issupplementDay = true;
			WindowClone.SetActive (true);
		} else if (currentsign.statue == 1) {
			Debug.LogError ("signday" + currentsign.SignCurrentDay);
			if (currentsign.SignCurrentDay > 0 && mSignState == RECV_SIGN_REWARD) {
				Facade.GetFacade ().message.signIn.SendGetSignAwardRequest (currentsign.SignCurrentDay);
//					nUserInfo.markSignInDay();
				mSignState = RECV_CONTINUE_REWARD;
			}
		} else {
			GameObject Window1 = UnityEngine.Resources.Load ("Window/WindowTipsThree") as UnityEngine.GameObject;
			GameObject WindowClone1 = UnityEngine.GameObject.Instantiate (Window1);
			UITipAutoNoMask ClickTips1 = WindowClone1.GetComponent<UITipAutoNoMask> ();
			ClickTips1.tipText.text = "簽到異樣";
			return;
		}


		//            if (SignDayReward > mCurrentDay)
		//            {
		//                Debug.Log("sssssss" + mCurrentDay);
		//                GameObject Window1 = UnityEngine.Resources.Load("Window/WindowTipsThree") as UnityEngine.GameObject;
		//                GameObject WindowClone1 = UnityEngine.GameObject.Instantiate(Window1);
		//                UITipAutoNoMask ClickTips1 = WindowClone1.GetComponent<UITipAutoNoMask>();
		//                ClickTips1.tipText.text = "当前签到天数不符合";
		//                return;
		//            }
		//			for (int i = 0; i < weekImage.Length; i++)
		//            {
		//				
		//                if (SignDayReward == weekImage[i].SignCurrentDay)
		//                {
		//                    sign = weekImage[i];
		////					if (nUserInfo.GetsSignArray () [i].day == sign.SignCurrentDay) {
		////						sign.statue = nUserInfo.GetsSignArray () [i].status;
		////					}
		//                }
		//            }

		//            if (sign.statue == 3)
		//            {
		//                if (LoginUtil.GetIntance().Ishare)
		//                {
		//                    GameObject Window1 = UnityEngine.Resources.Load("Window/WindowTipsThree") as UnityEngine.GameObject;
		//                    GameObject WindowClone1 = UnityEngine.GameObject.Instantiate(Window1);
		//                    UITipAutoNoMask ClickTips1 = WindowClone1.GetComponent<UITipAutoNoMask>();
		//                    ClickTips1.tipText.text = "微信朋友圈已经分享成功，不能再次分享";
		//                    return;
		//                }
		//				//Debug.LogError("sign.SignCurrentDay"+sign.SignCurrentDay+"")
		//                if (sign.SignCurrentDay != ReSignDay.reday)
		//                {
		//                    string path = "Window/WindowSign";
		//                    GameObject WindowClone = AppControl.OpenWindow(path);
		//                    DaySupplement daysupplement = WindowClone.GetComponent<DaySupplement>();
		//                    daysupplement.tips.text = "你不能分享这天的奖励请从头领取";
		//                    daysupplement.issupplementDay = false;
		//                    WindowClone.SetActive(true);
		//                }
		//                else
		//                {
		//                    string path = "Window/WindowSign";
		//                    GameObject WindowClone = AppControl.OpenWindow(path);
		//                    DaySupplement daysupplement = WindowClone.GetComponent<DaySupplement>();
		//                    daysupplement.issupplementDay = true;
		//                    WindowClone.SetActive(true);

		//                }
		//            }
		//            else if (sign.statue == 1)
		//            {
		//                //Debug.LogError("signday" + sign.SignCurrentDay);
		//                if (sign.SignCurrentDay > 0 && mSignState == RECV_SIGN_REWARD)
		//                {
		//					//Debug.LogError ("kkkkkkkkkksssssssss");
		////                    Facade.GetFacade().message.signIn.SendGetSignAwardRequest(sign.SignCurrentDay);
		////                    nUserInfo.markSignInDay();
		////                    mSignState = RECV_CONTINUE_REWARD;
		//                }
		//            }
		//            else
		//            {
		//                return;
		//            }
	}
	//}

	/// <summary>
	/// 以后再说领取
	/// </summary>
	//public   void Later()
	//{
	//	MyInfo nUserInfo = (MyInfo)Facade.GetFacade().data.Get(FacadeConfig.USERINFO_MODULE_ID);
	//	mSignState = RECV_SIGN_REWARD;

	//	mSignState = RECV_SIGN_REWARD;

	//	if (currentsign.statue == 3) {
	//		SignDayReward = ReSignDay.reday;
	//		string path = "Window/WindowSign";
	//		GameObject WindowClone = AppControl.OpenWindow (path);
	//		DaySupplement daysupplement = WindowClone.GetComponent<DaySupplement> ();
	//		daysupplement.issupplementDay = true;
	//		WindowClone.SetActive (true);

	//	} else if (currentsign.statue == 1) {
	//		//Debug.LogError ("signday" + currentsign.SignCurrentDay);
	//		if (currentsign.SignCurrentDay > 0 && mSignState == RECV_SIGN_REWARD) {
	//			Facade.GetFacade().message.signIn.SendGetSignAwardRequest(currentsign.SignCurrentDay);
	//			nUserInfo.markSignInDay();
	//			mSignState = RECV_CONTINUE_REWARD;
	//		}
	//	} else {
	//		GameObject Window1 = UnityEngine.Resources.Load("Window/WindowTipsThree") as UnityEngine.GameObject;
	//		GameObject WindowClone1 = UnityEngine.GameObject.Instantiate(Window1);
	//		UITipAutoNoMask ClickTips1 = WindowClone1.GetComponent<UITipAutoNoMask>();
	//		ClickTips1.tipText.text = "签到异样";
	//		return;
	//	}
	//}

	//图标上的每日奖励点击
	//	public void PanelButton()
	//	{
	//		if ( mCurrentDay > 0 && mSignState == RECV_SIGN_REWARD ) {
	//			MyInfo myInfo = (MyInfo)Facade.GetFacade ().data.Get ( FacadeConfig.USERINFO_MODULE_ID );
	//			Facade.GetFacade ().message.signIn.SendGetSignAwardRequest ( mCurrentDay );
	//			myInfo.markSignInDay();
	//			mSignState = RECV_CONTINUE_REWARD;
	//		}
	//	}


	public void  SignButtonOnClick (DaySignInfo sign)
	{

		for (int i = 0; i < weekImage.Length; i++) {
			weekImage [i].onclick.gameObject.SetActive (false);
		}
		sign.onclick.gameObject.SetActive (true);
		//		SignDayReward = sign.SignCurrentDay;
		//Debug.LogError ("SignDayReward" + SignDayReward + "kkkkkkkkzzzz" + mCurrentDay);
	}

	public void ShowSignOnclick ()
	{
		//Debug.LogError ("ReSignDay.isSignComplet" + ReSignDay.isSignComplet);
		if (ReSignDay.isSignComplet && ReSignDay.reday != 0) {
			SignDayReward = ReSignDay.reday;

			//Debug.LogError ("ShowSignOnclickSignDayReward " + SignDayReward + "mCurrentDay" + ReSignDay.reday + "signwindays" + ReSignDay.suppleday [ReSignDay.suppleday.Count - 1]);
			if (SignDayReward <= ReSignDay.suppleday [ReSignDay.suppleday.Count - 1]) {
				SignOnclick.sprite = UIHallTexturers.instans.SignRewardType [3];
			} else {
				//Debug.LogError ("ssssssssssssssssssss");
				SignOnclick.sprite = UIHallTexturers.instans.SignRewardType [4];
				SignOnclick.gameObject.GetComponent<Button> ().enabled = false;
			}
		} else if (ReSignDay.isSignComplet && ReSignDay.reday == 0) {
			//Debug.LogError ("zzzzzzzzzzzzzzzzzzz");
			SignOnclick.sprite = UIHallTexturers.instans.SignRewardType [4];
			SignOnclick.gameObject.GetComponent<Button> ().enabled = false;
			SignDayReward = 1;
		}



		for (int i = 0; i < weekImage.Length; i++) {

			if (SignDayReward == weekImage [i].SignCurrentDay) {
				currentsign = weekImage [i];
			}
		}
		//Debug.LogError ("currentsign" + currentsign.SignCurrentDay);
		SignButtonOnClick (currentsign);




	}

	void SendContinueRewardRequest (int ConticueDay)
	{
		//Debug.LogError ("_________" + mContinuedDays);
		if (ConticueDay != mContinuedDays) {
			return;
		}
		if (mSignState == RECV_CONTINUE_REWARD) {
			int nResult = GetContinueData (ConticueDay);
			if (nResult != 0) {
				//Debug.LogError ("@@@@@@@@@@@@@@@" + nResult);
				Facade.GetFacade ().message.signIn.SendGetSignAwardRequest (nResult);
				CancelInvoke ();
				//5s 后如果服务器不返回数据，强制关闭
				Invoke ("CloseWindow", 5.0f);
			}
		}
	}

	public void ThreeButton ()
	{
		if (threeAni != null && threeAni.enabled) {
			Destroy (threeAni);
			threeAni = null;
			mSignState = RECV_CONTINUE_REWARD;
		}
		//如果是连续签到奖励领取
		SendContinueRewardRequest (3);
	}


	public void SixButton ()
	{
		if (sixAni != null && sixAni.enabled) {
			Destroy (sixAni);
			sixAni = null;
			mSignState = RECV_CONTINUE_REWARD;
		}
		SendContinueRewardRequest (6);
	}

	public void EightButton ()
	{
		if (eightAni != null && eightAni.enabled) {
			Destroy (eightAni);
			eightAni = null;
			mSignState = RECV_CONTINUE_REWARD;
		}
		SendContinueRewardRequest (8);
	}

	public void FiftyButton ()
	{
		if (fiftyAni != null && fiftyAni.enabled) {
			Destroy (fiftyAni);
			fiftyAni = null;
			mSignState = RECV_CONTINUE_REWARD;
		}
		SendContinueRewardRequest (15);
	}

	private void StartContinueAnimation (int nContinueDay)
	{
		if (nContinueDay == -3) {
			//可以播放3天的动画
			if (threeAni != null) {
				threeAni.enabled = true;
			}
		}
		if (nContinueDay == -6) {

			if (sixAni != null) {
				sixAni.enabled = true;
			}
		}
		if (nContinueDay == -8) {
			if (eightAni != null) {
				eightAni.enabled = true;
			}
		}
		if (nContinueDay == -15) {
			if (fiftyAni != null) {
				fiftyAni.enabled = true;
			}
		}
	}

	//奖励领取完毕了，3s后关闭签到窗口
	//	void OnRewardCompelete()
	//	{
	//		//Debug.LogError( "---------------OnRewardCompelete--------------" );
	//		//如果有连续签到的奖励能领取，那么我们不关闭签到窗口
	//		if (mSignState == RECV_CONTINUE_REWARD && GetContinueData (mContinuedDays) != 0) {
	//			////Debug.LogError ( "-----------return--------------" + GetContinueData (mCurrentDay) );
	//			return;
	//		}
	//		CloseWindow ();
	////		Invoke ( "CloseWindow" , 1.0f );
	//	}

	void ShowReward (List<FiProperty> nPorpList)
	{
		UnityEngine.GameObject Window = UnityEngine.Resources.Load ("MainHall/Gift/SignRewardWindow")as UnityEngine.GameObject;
		GameObject WindowClone = UnityEngine.GameObject.Instantiate (Window);
		UISignReward reward = WindowClone.GetComponent<UISignReward> ();
		if (mContinuedDays == 3 || mContinuedDays == 6 || mContinuedDays == 8 || mContinuedDays == 15) {
			reward.SetRewardData (nPorpList, true);
		} else {
			reward.SetRewardData (nPorpList);
		}
	}

	public void OnRecvData (int nType, object nData)
	{
		//收到服务器数据了，那么销毁签到页面
		CancelInvoke ();
		if (nType == FiEventType.RECV_SIGN_IN_AWARD_RESPONSE) {
			//			//Debug.LogError ("ssssssssss");
			FiSignInAwardResponse nResponse = (FiSignInAwardResponse)nData;
			//Debug.LogError ("nResponse.properties" + nResponse.properties.Count);

			//当signIn字段都为0时，说明是领取连续签到礼包的消息
			if (nResponse.singIn.day == 0 && nResponse.singIn.status == 0) {
				//Debug.LogError ("___________是否是补签成功后的入口——————————————" + nResponse.singIn.day + nResponse.singIn.status);
				InitRecvDayStatus (mContinuedDays);
				ShowReward (nResponse.properties);
				ReSignDay.isContinueSign = true;
				return;
			}
			MyInfo myInfo = (MyInfo)Facade.GetFacade ().data.Get (FacadeConfig.USERINFO_MODULE_ID);
			//Debug.LogError ("______________________________signday" + SignDayReward);
			for (int i = 0; i < myInfo.signInArray.Count; i++) {
				if (mCurrentDay == myInfo.signInArray [i].day) {
					myInfo.signInArray [i].status = 2;
				}

			}
			//领取当天礼包的结果 设置领取进度，弹出领取奖励的界面
			mContinuedDays++;
			ReSignDay.continuedday++;
			//Debug.LogError ("_________ReSignDay.continuedday_____" + ReSignDay.continuedday);
			mcontinueday.text = mContinuedDays.ToString ();
			SetSignProgress (mContinuedDays);
//			if (mContinuedDays == 3 || mContinuedDays == 6 || mContinuedDays == 8 || mContinuedDays == 15)
//			{
//				int nResult = GetContinueData(mContinuedDays);
//				Facade.GetFacade().message.signIn.SendGetSignAwardRequest(nResult);
//			}
			weekImage [mCurrentDay - 1].transform.Find ("Mask").gameObject.SetActive (true);
			weekImage [mCurrentDay - 1].transform.Find ("Mask").Find ("Right").gameObject.SetActive (true);

			if (nResponse.singIn.status == 0) {
				//如果条件触发了连续签的礼包
				if (nResponse.singIn.day == -3 || nResponse.singIn.day == -6 || nResponse.singIn.day == -8 || nResponse.singIn.day == -15) {
					//Debug.LogError ("nResponse.singIn.day" + nResponse.singIn.day);
					StartContinueAnimation (nResponse.singIn.day);
					return;
				}
			}
			ShowReward (nResponse.properties);
		}

		if (nType == FiEventType.RECV_CL_SIGNRETROACTIVE_RESPONSE) {
			FiRetroactiveReward nResponse = (FiRetroactiveReward)nData;

			//Debug.LogError ("SignDayReward" + SignDayReward);
			//领取当天礼包的结果 设置领取进度，弹出领取奖励的界面
			weekImage [ReSignDay.redayback - 1].transform.Find ("Mask").gameObject.SetActive (true);
			weekImage [ReSignDay.redayback - 1].transform.Find ("Mask").Find ("Right").gameObject.SetActive (true);
			weekImage [ReSignDay.redayback - 1].transform.Find ("Mask").Find ("Right1").gameObject.SetActive (false);
			ShowReward (nResponse.properties);
		}
	}

	void InitRecevDaysignStatue (int signday)
	{
		weekImage [signday].Mask.gameObject.SetActive (true);

	}
	//设置连续签到的领取状态
	void InitRecvDayStatus (int nContinueDay)
	{
		if (nContinueDay >= 3) {
			OnRecv3DayGift ();
		}
		if (nContinueDay >= 6) {
			OnRecv7DayGift ();
		}
		if (nContinueDay >= 8) {
			OnRecv8DayGift ();
		}
		if (nContinueDay >= 15) {
			OnRecv15DayGift ();
		}
	}



	void SetSignProgress (int nContinueDay, bool IsNcontinueReward = false)
	{
		if (nContinueDay <= 3) {
			signSlider.value = nContinueDay * 0.05f;
		}
		if (nContinueDay > 3 && nContinueDay <= 6) {
			signSlider.value = (nContinueDay - 3) * 0.066f + 0.18f;
		}
		if (nContinueDay >= 7) {
			signSlider.value = (nContinueDay - 6) * 0.075f + 0.4f;
		}
		if (IsNcontinueReward || ReSignDay.isContinueSign) {
			return;
		}
		//显示领取动画
		if (nContinueDay == 3 && !mOpened3DayTip.activeSelf) {
			StartContinueAnimation (-3);
		} else if (nContinueDay == 6 && !mOpened7DayTip.activeSelf) {
			StartContinueAnimation (-6);
		} else if (nContinueDay == 8 && !mOpened8DayTip.activeSelf) {
			StartContinueAnimation (-8);
		} else if (nContinueDay == 15 && !mOpened15DayTip.activeSelf) {
			StartContinueAnimation (-15);
		}
	}

	public void OnRelease ()
	{

	}

	void OnDestroy ()
	{
		Facade.GetFacade ().ui.Remove (FacadeConfig.UI_SIGN_IN_MODULE_ID);
		LoginUtil.GetIntance ().onshowsignchange -= ShowSignOnclick;
		MyInfo nUserInfo = (MyInfo)Facade.GetFacade ().data.Get (FacadeConfig.USERINFO_MODULE_ID);
		nUserInfo.SetignStatue (1);
	}

	public void OnBtnClose ()
	{
		
		UIColseManage.instance.CloseUI ();

		MyInfo myInfo = (MyInfo)Facade.GetFacade().data.Get(FacadeConfig.USERINFO_MODULE_ID);
		if (UIHallCore.Instance != null)
        {
			if (ReSignDay.isFristActive)
			{
				UIHallCore.Instance.ToActivity();
				ReSignDay.isFristActive = false;
			}
			
		}

        if (ReSignDay.isFristActive)
        {
            for (int i = 0; i < myInfo.signInArray.Count; i++)
            {
                if (myInfo.signInArray[i].status == 1 && myInfo.signInArray[i].day > 0)
                {
                    ReSignDay.isFristActive = true;
                    break;
                }
                else
                {
                    ReSignDay.isFristActive = false;
                }
            }
        }
    }
	//控制活动窗弹起
	public void TestActivity ()
	{
		if (BindWindowCtrl.isStartActivity) {
			GameObject activity = Resources.Load<GameObject> ("Window/Canvas_1");
			GameObject go = Instantiate (activity);
			go.SetActive (true);
		}
	}

	void OnInitData ()
	{
		GetSignrewardinfo ();
//		//Debug.LogError ("conyticueinfo.Count" + conyticueinfo.Count);
		for (int i = 0; i < conyticueinfo.Count; i++) {
			for (int j = 0; j < conyticueinfo [i].rewardPro.Count; j++) {
				contincueicon [i].sprite = FiPropertyType.GetSigncontiuenSprite (conyticueinfo [i].rewardPro [j].type);
				conticuevalue [i].text = conyticueinfo [i].rewardPro [j].value.ToString ();

			}
		}
//		//Debug.LogError ("signrewardinfo.Count" + signrewardinfo.Count);
		for (int i = 0; i < signrewardinfo.Count; i++) {
			SignCofing temp = new SignCofing ();
			temp.SignDay = signrewardinfo [i].TaskID;
			//Debug.Log(" ----- TaskID = "+ signrewardinfo[i].TaskID);
			//Debug.Log(" ----- ----- Rewardtcount = " + signrewardinfo[i].rewardPro.Count);
			//Debug.Log(" ----- ----- Rewardtype = " + signrewardinfo[i].rewardPro);
			switch (signrewardinfo [i].TaskID) {
			case 1:
				temp.Rewardtcount = signrewardinfo [i].rewardPro.Count;
				temp.Rewardtype = signrewardinfo [i].rewardPro;
				break;
			case 2:
				temp.Rewardtcount = signrewardinfo [i].rewardPro.Count;
				temp.Rewardtype = signrewardinfo [i].rewardPro;

				break;
			case 3:
				temp.Rewardtcount = signrewardinfo [i].rewardPro.Count;
				temp.Rewardtype = signrewardinfo [i].rewardPro;

				break;
			case 4:
				temp.Rewardtcount = signrewardinfo [i].rewardPro.Count;
				temp.Rewardtype = signrewardinfo [i].rewardPro;

				break;
			case 5:
				temp.Rewardtcount = signrewardinfo [i].rewardPro.Count;
				temp.Rewardtype = signrewardinfo [i].rewardPro;

				break;
			case 6:
				temp.Rewardtcount = signrewardinfo [i].rewardPro.Count;
				temp.Rewardtype = signrewardinfo [i].rewardPro;

				break;
			case 7:
				temp.Rewardtcount = signrewardinfo [i].rewardPro.Count;
				temp.Rewardtype = signrewardinfo [i].rewardPro;

				break;
			default:
//				//Debug.LogError ("no task");
				break;
			}
			signList.Add (temp);
		}			
	}

}