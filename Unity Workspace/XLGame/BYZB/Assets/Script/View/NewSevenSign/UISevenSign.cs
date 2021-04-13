using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using AssemblyCSharp;
using UnityEngine.UI;

public class UISevenSign : MonoBehaviour,IUiMediator
{

	private Button Close;
	private Button Rule;
	private Button SevesignBtn;
	private Button TastBtn;
	private Button preBtn;


	public Image[] Islock;
	public Image[] IstommorwDay;
	public Sprite[] ShowMoneyText;

	public Image SignMoneyText;
	/// <summary>
	/// 任务
	/// </summary>
	public  Text Tasktext;
	public  Image[] Tasktype;
	public  Text[] TaskTextnum;
	public  Text ProgressText;
	public Slider Taskslider;

	public Image HindReward;
	private long MaxValue;
	private long CurrValue;

	public Text orgPrice;
	public Text nowPrice;
	public Text ShowGold;
	private int[] DefaultRMB = new int[] { 20, 20, 30, 30, 50, 50, 100 };
	private long[] DefaultGold = new long[] { 368000, 368000, 537600, 537600, 896000, 896000, 1792000 };
	/// <summary>
	/// 礼包
	/// </summary>
	public Text[] SevenMoney;
	public Image[] Seventype;
	public Text[] Sevennum;
	public Image SevenMoneyNum;

	public Sprite[] moneyArray;

	MyInfo nUserInfo;

	NewSevenDayInfo sevenDayinfo;

	public static bool have2minus = false;
	//	public DaySignInfo[] weekImage = new DaySignInfo[7];

	void Awake ()
	{
		if (GameController._instance == null) //渔场和大厅里赋值的摄像机不同
			gameObject.GetComponent<Canvas> ().worldCamera = GameObject.FindGameObjectWithTag ("MainCamera").GetComponent<Camera> ();
		else
			gameObject.GetComponent<Canvas> ().worldCamera = ScreenManager.uiCamera;
		nUserInfo = (MyInfo)Facade.GetFacade ().data.Get (FacadeConfig.USERINFO_MODULE_ID);
		sevenDayinfo = (NewSevenDayInfo)Facade.GetFacade ().data.Get (FacadeConfig.UI_SEVENPART);
		UIColseManage.instance.ShowUI (this.gameObject);
		Close = transform.Find ("btclose").GetComponent<Button> ();
		Rule = transform.Find ("rule").GetComponent<Button> ();
        //今日簽到福利
		SevesignBtn = transform.Find ("SevenList/Signbg/SignReward").GetComponent<Button> ();
		SevesignBtn.gameObject.SetActive(true);

		//今日任務福利
		TastBtn = transform.Find ("SevenList/Taskbbg/SignReward1").GetComponent<Button> ();
		TastBtn.gameObject.SetActive(true);
		
		//今日特惠福利
		preBtn = transform.Find ("SevenList/prepay/SignReward2").GetComponent<Button> ();
		
		Close.onClick.AddListener (Btnlose);
		Rule.onClick.AddListener (OpenRule);
		SevesignBtn.onClick.AddListener (SendSignMessage);
		TastBtn.onClick.AddListener (AccepTasktReward);
		preBtn.onClick.AddListener (delegate {
			PurchaseLibao (sevenDayinfo.nuserGiftDay);
		});

//		StartCoroutine (Start ());
	}
	// Use this for initialization
	IEnumerator Start ()
	{
		yield return new WaitForSeconds (0.3f);

		Debug.Log ("自己的金币+++" + nUserInfo.gold);
		sevenDayinfo.SevenDayState = new List<int> ();
		Facade.GetFacade ().ui.Add (FacadeConfig.UI_SEVENPART, this);

		CurrValue = sevenDayinfo.ntaskValue;
		InitSevenSignMoney (sevenDayinfo.nuserDay);
		InitSevenItem (sevenDayinfo.nuserGiftDay);
		InitTaskItem (sevenDayinfo.ntaskDay);
	

		sevenDayinfo.SevenDayState.Add (sevenDayinfo.nuserDayState);
		//这里改用NTASKVALUE是因为服务器不用那做State
		sevenDayinfo.SevenDayState.Add (sevenDayinfo.ntaskDayState);
		sevenDayinfo.SevenDayState.Add (sevenDayinfo.nuserGiftDyaState);
		SetLockInfo (sevenDayinfo.SevenDayState, Islock);
		Debug.Log("sevenDayinfo.nuserGiftDay = "+ sevenDayinfo.nuserGiftDay);
		if (nUserInfo.Pay_NewSeven_RMB.Count > 6)
		{
			nUserInfo.Pay_NewSeven_RMB.Sort();
			nUserInfo.Pay_NewSeven_AddGold.Sort();
			orgPrice.text = "" + ((int)nUserInfo.Pay_NewSeven_RMB[sevenDayinfo.nuserGiftDay-1] + 10);
			nowPrice.text = "" + nUserInfo.Pay_NewSeven_RMB[sevenDayinfo.nuserGiftDay-1];
			ShowGold.text = "" + nUserInfo.Pay_NewSeven_AddGold[sevenDayinfo.nuserGiftDay-1];
		}
		else {
			orgPrice.text = "" + (DefaultRMB[sevenDayinfo.nuserGiftDay-1] + 10);
			nowPrice.text = "" + DefaultRMB[sevenDayinfo.nuserGiftDay-1];
			ShowGold.text = "" + DefaultGold[sevenDayinfo.nuserGiftDay-1];
		}
		    
	}
	//判断是否是购买锁上状态
	void SetLockInfo (List<int> SignState, Image[] locks)
	{
		for (int i = 0; i < SignState.Count; i++) {
			Debug.Log (SignState [i] + "signtaste" + locks.Length);
			if (i == 0)
			{
				if (SignState[i] != 0)
				{
					locks[i].gameObject.SetActive(true);
					IstommorwDay[i].gameObject.SetActive(true);
                    have2minus = true;
					//Debug.LogError("Joey Test Lock = " + have2minus);
				}
				else
				{
					locks[i].gameObject.SetActive(false);
					IstommorwDay[i].gameObject.SetActive(false);
					have2minus = false;
					//Debug.LogError("Joey Test Lock = " + have2minus);
				}
			}
			else
            {
				if (SignState[i] != 0)
				{
					locks[i].gameObject.SetActive(true);
					IstommorwDay[i].gameObject.SetActive(true);
				}
				else
				{
					locks[i].gameObject.SetActive(false);
					IstommorwDay[i].gameObject.SetActive(false);
				}
			}
			
		}
	}



	// Update is called once per frame
	void Update ()
	{
		
	}

	public void OnRecvData (int nType, object nData)
	{
		//这应该做三种接受类型判断
//		ShowReward()
		if (nType == 1000) {
			FiSevenDaysPage nResponse = (FiSevenDaysPage)nData;
			ShowReward (nResponse.target);
			Islock [0].gameObject.SetActive (true);
			have2minus = true;
			// Debug.LogError("Joey Test Lock = "+ have2minus);
			IstommorwDay [0].gameObject.SetActive (true);
			CurrValue = 0;
			InitSevenSignMoney (nResponse.UserDay);

		} else if (nType == 1001) {
			FiSevenDaysPage nResponse = (FiSevenDaysPage)nData;
			ShowReward (nResponse.target);
			Islock [1].gameObject.SetActive (true);
			IstommorwDay [1].gameObject.SetActive (true);
			CurrValue = 0;
			InitTaskItem (nResponse.UserDay);

		} else if (nType == 1002) {
			FiPurChaseTehuiRewradData nResponse = (FiPurChaseTehuiRewradData)nData;
			ShowReward (nResponse.prop);
			Islock [2].gameObject.SetActive (true);
			IstommorwDay [2].gameObject.SetActive (true);
			CurrValue = 0;

			InitSevenItem (sevenDayinfo.nuserGiftDay + 1);
		
		}
	}

	public void ShowReward (List<FiProperty> nPorpList)
	{
		UnityEngine.GameObject Window = UnityEngine.Resources.Load ("Window/RewardWindow")as UnityEngine.GameObject;
		GameObject rewardInstance = UnityEngine.GameObject.Instantiate (Window);
		UIReward reward = rewardInstance.GetComponent<UIReward> ();
		reward.ShowRewardType (0);
		reward.SetRewardData (nPorpList);
	}

	public void OnInit ()
	{

	}

	public void OnRelease ()
	{
		
	}
	//打开规则
	void OpenRule ()
	{
		string path = "Window/NewSevenSignRule";
		GameObject jumpObj = AppControl.OpenWindow (path);
		jumpObj.SetActive (true);
	}
	//签到发送协议
	void SendSignMessage ()
	{
		//2020/03/03 Joey 防止用戶連續點擊
		//SevesignBtn.gameObject.SetActive(false);

		if (nUserInfo.isGuestLogin) {
			string path = "Window/BindPhoneNumber";
			GameObject jumpObj = AppControl.OpenWindow (path);
			jumpObj.SetActive (true);
			return;
		}
		Islock[0].gameObject.SetActive(true);
		Facade.GetFacade ().message.sevensign.SendNewSignMessage (1, sevenDayinfo.nuserDay);
	}

	//领取任务奖励
	void AccepTasktReward ()
	{
		if (nUserInfo.isGuestLogin) {
			string path = "Window/BindPhoneNumber";
			GameObject jumpObj = AppControl.OpenWindow (path);
			jumpObj.SetActive (true);
			return;
		}
        //測試領取獎勵資訊更新
		InfoMsg.GetInstance();
		//2020/03/03 Joey 防止用戶連續點擊
		Islock[1].gameObject.SetActive(true);
		TastBtn.gameObject.SetActive(false);
		Facade.GetFacade ().message.sevensign.SendNewSignMessage (2, sevenDayinfo.ntaskDay);
		HindReward.gameObject.SetActive(true);
	}
	//购买礼包
	void PurchaseLibao (int day)
	{
		Debug.Log("-----UISevenSign-----PurchaseLibao-----day = "+day);
		if (nUserInfo.isGuestLogin) {
			string path = "Window/BindPhoneNumber";
			GameObject jumpObj = AppControl.OpenWindow (path);
			jumpObj.SetActive (true);
			return;
		}

		int pay_gold = 30;
		if (nUserInfo.Pay_NewSeven_RMB.Count > 2)
			pay_gold = (int)nUserInfo.Pay_NewSeven_RMB[day-1];
		else
			pay_gold = DefaultRMB[day-1];

		switch (day) {
		case 1:
			UIToPay.DarGonCardType = 51016;
			UIToPay.OpenThirdPartPay (pay_gold);
//			ShowTest (16);
			break;
		case 2:
			UIToPay.DarGonCardType = 51017;
			UIToPay.OpenThirdPartPay (pay_gold);
//			ShowTest (17);
			break;
		case 3:
			UIToPay.DarGonCardType = 51018;
			UIToPay.OpenThirdPartPay (pay_gold);
//			ShowTest (18);
			break;
		case 4:
			UIToPay.DarGonCardType = 51019;
			UIToPay.OpenThirdPartPay (pay_gold);
//			ShowTest (19);
			break;
		case 5:
			UIToPay.DarGonCardType = 51020;
			UIToPay.OpenThirdPartPay (pay_gold);
//			ShowTest (20);
			break;
		case 6:
			UIToPay.DarGonCardType = 51021;
			UIToPay.OpenThirdPartPay (pay_gold);
//			ShowTest (21);
			break;
		case 7:
			UIToPay.DarGonCardType = 51022;
			UIToPay.OpenThirdPartPay (pay_gold);
//			ShowTest (22);
			break;
		}
	}


	void InitSevenSignMoney (int Day)
	{
		if (Day > 7) {
			Day = 7;
		}
		SignMoneyText.sprite = ShowMoneyText [Day - 1];
	}

	void InitSevenItem (int Day)
	{
		if (Day > 7) {
			Day = 7;
		}
		Sprite[] itemicon = new Sprite[2];
		switch (Day) {
		case 1:
			SevenMoney [0].text = "3元";
			SevenMoney [1].text = "20元";
			SevenMoneyNum.sprite = moneyArray [0];
			Seventype [0].sprite = FiPropertyType.GetDaiKuangSprite (FiPropertyType.DIAMOND);
			Seventype [1].sprite = FiPropertyType.GetDaiKuangSprite (FiPropertyType.FISHING_EFFECT_FREEZE);
			Seventype [2].sprite = FiPropertyType.GetDaiKuangSprite (FiPropertyType.FISHING_EFFECT_AIM);
			Seventype [3].sprite = FiPropertyType.GetDaiKuangSprite (FiPropertyType.FISHING_EFFECT_REPLICATE);

			Sevennum [0].text = "30";
			Sevennum [1].text = "3";
			Sevennum [2].text = "3";
			Sevennum [3].text = "3";

			break;
		case 2:
			SevenMoney [0].text = "3元";
			SevenMoney [1].text = "20元";
			SevenMoneyNum.sprite = moneyArray [0];
			Seventype [0].sprite = FiPropertyType.GetDaiKuangSprite (FiPropertyType.DIAMOND);
			itemicon = FiPropertyType.GetTimeSpriteShow (9073);

			Seventype [1].sprite = itemicon [0];
			Seventype [1].transform.Find ("Day").gameObject.SetActive (true);
			Seventype [1].transform.Find ("Day").GetComponent<Image> ().sprite = itemicon [1];
			Seventype [2].sprite = FiPropertyType.GetDaiKuangSprite (FiPropertyType.FISHING_EFFECT_REPLICATE);
			Seventype [3].sprite = FiPropertyType.GetDaiKuangSprite (FiPropertyType.FISHING_EFFECT_FREEZE);

			Sevennum [0].text = "30";
			Sevennum [1].text = "1";
			Sevennum [2].text = "3";
			Sevennum [3].text = "3";
			break;
		case 3:
			SevenMoney [0].text = "10元";
			SevenMoney [1].text = "80元";
			SevenMoneyNum.sprite = moneyArray [1];
			Seventype [0].sprite = FiPropertyType.GetDaiKuangSprite (FiPropertyType.DIAMOND);
			itemicon = FiPropertyType.GetTimeSpriteShow (9075);

			Seventype [1].sprite = itemicon [0];
			Seventype [1].transform.Find ("Day").gameObject.SetActive (true);
			Seventype [1].transform.Find ("Day").GetComponent<Image> ().sprite = itemicon [1];
			Seventype [2].sprite = FiPropertyType.GetDaiKuangSprite (FiPropertyType.FISHING_EFFECT_REPLICATE);
			Seventype [3].sprite = FiPropertyType.GetDaiKuangSprite (FiPropertyType.FISHING_EFFECT_AIM);

			Sevennum [0].text = "200";
			Sevennum [1].text = "1";
			Sevennum [2].text = "15";
			Sevennum [3].text = "15";
			break;
		case 4:
			SevenMoney [0].text = "10元";
			SevenMoney [1].text = "80元";
			SevenMoneyNum.sprite = moneyArray [1];
			Seventype [0].sprite = FiPropertyType.GetDaiKuangSprite (FiPropertyType.DIAMOND);
			Seventype [1].sprite = FiPropertyType.GetDaiKuangSprite (FiPropertyType.FISHING_EFFECT_VIOLENT);
			Seventype [2].sprite = FiPropertyType.GetDaiKuangSprite (FiPropertyType.FISHING_EFFECT_REPLICATE);
			Seventype [3].sprite = FiPropertyType.GetDaiKuangSprite (FiPropertyType.FISHING_EFFECT_AIM);

			Sevennum [0].text = "200";
			Sevennum [1].text = "15";
			Sevennum [2].text = "15";
			Sevennum [3].text = "15";
			break;
		case 5:
			SevenMoney [0].text = "30元";
			SevenMoney [1].text = "200元";
			SevenMoneyNum.sprite = moneyArray [2];
			Seventype [0].sprite = FiPropertyType.GetDaiKuangSprite (FiPropertyType.DIAMOND);
			itemicon = FiPropertyType.GetTimeSpriteShow (9076);

			Seventype [1].sprite = itemicon [0];
			Seventype [1].transform.Find ("Day").gameObject.SetActive (true);
			Seventype [1].transform.Find ("Day").GetComponent<Image> ().sprite = itemicon [1];
			Seventype [2].sprite = FiPropertyType.GetDaiKuangSprite (FiPropertyType.FISHING_EFFECT_AIM);
			Seventype [3].sprite = FiPropertyType.GetDaiKuangSprite (FiPropertyType.FISHING_EFFECT_VIOLENT);

			Sevennum [0].text = "260";
			Sevennum [1].text = "1";
			Sevennum [2].text = "20";
			Sevennum [3].text = "20";
			break;
		case 6:
			SevenMoney [0].text = "30元";
			SevenMoney [1].text = "200元";
			SevenMoneyNum.sprite = moneyArray [2];
			Seventype [0].sprite = FiPropertyType.GetDaiKuangSprite (FiPropertyType.DIAMOND);
			itemicon = FiPropertyType.GetTimeSpriteShow (7071);

			Seventype [1].sprite = itemicon [0];
			Seventype [1].transform.Find ("Day").gameObject.SetActive (true);
			Seventype [1].transform.Find ("Day").GetComponent<Image> ().sprite = itemicon [1];
//			Seventype [2].sprite = FiPropertyType.GetDaiKuangSprite (FiPropertyType.DIAMOND);
			itemicon = FiPropertyType.GetTimeSpriteShow (9079);

			Seventype [2].sprite = itemicon [0];
			Seventype [2].transform.Find ("Day").gameObject.SetActive (true);
			Seventype [2].transform.Find ("Day").GetComponent<Image> ().sprite = itemicon [1];
			Seventype [3].sprite = FiPropertyType.GetDaiKuangSprite (FiPropertyType.FISHING_EFFECT_REPLICATE);

			Sevennum [0].text = "260";
			Sevennum [1].text = "1";
			Sevennum [2].text = "1";
			Sevennum [3].text = "20";
			break;
		case 7:
			SevenMoney [0].text = "66元";
			SevenMoney [1].text = "500元";
			SevenMoneyNum.sprite = moneyArray [3];
			Seventype [0].sprite = FiPropertyType.GetDaiKuangSprite (FiPropertyType.DIAMOND);
			itemicon = FiPropertyType.GetTimeSpriteShow (9077);

			Seventype [1].sprite = itemicon [0];
			Seventype [1].transform.Find ("Day").gameObject.SetActive (true);
			Seventype [1].transform.Find ("Day").GetComponent<Image> ().sprite = itemicon [1];
			Seventype [2].sprite = FiPropertyType.GetDaiKuangSprite (FiPropertyType.FISHING_EFFECT_REPLICATE);
			Seventype [3].sprite = FiPropertyType.GetDaiKuangSprite (FiPropertyType.FISHING_EFFECT_VIOLENT);

			Sevennum [0].text = "600";
			Sevennum [1].text = "1";
			Sevennum [2].text = "40";
			Sevennum [3].text = "40";
			break;
		}
	}

	public void Refesh (long currevalue, long maxvalue)
	{
		if ((currevalue <= 0)) {
			currevalue = 0;
		}
		if (currevalue >= maxvalue) {
			HindReward.gameObject.SetActive (false);
			currevalue = maxvalue;
		}

		Taskslider.value = currevalue / (float)maxvalue;
		ProgressText.text = currevalue + "/" + maxvalue;
	}

	void InitTaskItem (int Day)
	{
		if (Day > 7) {
			Day = 7;
		}
		Sprite[] itemicon = new Sprite[2];
		switch (Day) {
		case 1:
			Tasktext.text = "擊殺任意魚10條";
			Tasktype [0].sprite = FiPropertyType.GetDaiKuangSprite (FiPropertyType.GOLD);
			Tasktype [1].sprite = FiPropertyType.GetDaiKuangSprite (FiPropertyType.FISHING_EFFECT_AIM);
			Tasktype [2].sprite = FiPropertyType.GetDaiKuangSprite (FiPropertyType.DIAMOND);

			TaskTextnum [0].text = "2000";
			TaskTextnum [1].text = "5";
			TaskTextnum [2].text = "10";
			Refesh (CurrValue, 10);

			break;
		case 2:
			Tasktext.text = "擊殺任意魚30條";
			Tasktype [0].sprite = FiPropertyType.GetDaiKuangSprite (FiPropertyType.GOLD);

			itemicon = FiPropertyType.GetTimeSpriteShow (9036);

			Tasktype [1].sprite = itemicon [0];
			Tasktype [1].transform.Find ("Day").gameObject.SetActive (true);
			Tasktype [1].transform.Find ("Day").GetComponent<Image> ().sprite = itemicon [1];
			Tasktype [2].sprite = FiPropertyType.GetDaiKuangSprite (FiPropertyType.DIAMOND);

			TaskTextnum [0].text = "5000";
			TaskTextnum [1].text = "1";
			TaskTextnum [2].text = "20";
			Refesh (CurrValue, 30);
			break;
		case 3:
			Tasktext.text = "累計發炮流水10萬";
			Tasktype [0].sprite = FiPropertyType.GetDaiKuangSprite (FiPropertyType.GOLD);

			itemicon = FiPropertyType.GetTimeSpriteShow (7072);

			Tasktype [1].sprite = itemicon [0];
			Tasktype [1].transform.Find ("Day").gameObject.SetActive (true);
			Tasktype [1].transform.Find ("Day").GetComponent<Image> ().sprite = itemicon [1];
			Tasktype [2].sprite = FiPropertyType.GetDaiKuangSprite (FiPropertyType.DIAMOND);

			TaskTextnum [0].text = "8000";
			TaskTextnum [1].text = "1";
			TaskTextnum [2].text = "30";
			Refesh (CurrValue, 100000);
			break;
		case 4:
			Tasktext.text = "累計發炮流水20萬";
			Tasktype [0].sprite = FiPropertyType.GetDaiKuangSprite (FiPropertyType.GOLD);
			Tasktype [1].sprite = FiPropertyType.GetDaiKuangSprite (FiPropertyType.FISHING_EFFECT_REPLICATE);
			Tasktype [2].sprite = FiPropertyType.GetDaiKuangSprite (FiPropertyType.DIAMOND);

			TaskTextnum [0].text = "10000";
			TaskTextnum [1].text = "5";
			TaskTextnum [2].text = "50";
			Refesh (CurrValue, 200000);
			break;
		case 5:
			Tasktext.text = "累计击杀鱼价值30万";
			Tasktype [0].sprite = FiPropertyType.GetDaiKuangSprite (FiPropertyType.GOLD);
			Tasktype [1].sprite = FiPropertyType.GetDaiKuangSprite (FiPropertyType.FISHING_EFFECT_FREEZE);
			Tasktype [2].sprite = FiPropertyType.GetDaiKuangSprite (FiPropertyType.DIAMOND);

			TaskTextnum [0].text = "15000";
			TaskTextnum [1].text = "10";
			TaskTextnum [2].text = "60";
			Refesh (CurrValue, 300000);
			break;
		case 6:
			Tasktext.text = "累計擊殺魚價值50萬";
			Tasktype [0].sprite = FiPropertyType.GetDaiKuangSprite (FiPropertyType.GOLD);
			Tasktype [1].sprite = FiPropertyType.GetDaiKuangSprite (FiPropertyType.FISHING_EFFECT_SUMMON);
			Tasktype [2].sprite = FiPropertyType.GetDaiKuangSprite (FiPropertyType.DIAMOND);

			TaskTextnum [0].text = "20000";
			TaskTextnum [1].text = "10";
			TaskTextnum [2].text = "80";
			Refesh (CurrValue, 500000);
			break;
		case 7:
			Tasktext.text = "累計發炮流水100萬";
			Tasktype [0].sprite = FiPropertyType.GetDaiKuangSprite (FiPropertyType.GOLD);
			itemicon = FiPropertyType.GetTimeSpriteShow (9077);

			Tasktype [1].sprite = itemicon [0];
			Tasktype [1].transform.Find ("Day").gameObject.SetActive (true);
			Tasktype [1].transform.Find ("Day").GetComponent<Image> ().sprite = itemicon [1];
			Tasktype [2].sprite = FiPropertyType.GetDaiKuangSprite (FiPropertyType.DIAMOND);

			TaskTextnum [0].text = "30000";
			TaskTextnum [1].text = "1";
			TaskTextnum [2].text = "100";
			Refesh (CurrValue, 1000000);
			break;
		}
	}

	void OnDestroy ()
	{
		Facade.GetFacade ().ui.Remove (FacadeConfig.UI_SEVENPART);
		sevenDayinfo.SevenDayState.Clear ();
		MyInfo nUserInfo = (MyInfo)Facade.GetFacade ().data.Get (FacadeConfig.USERINFO_MODULE_ID);
		nUserInfo.SetignStatue (1);

	}

	//void ShowTest (int  type)
	//{
	//	string Urltest;
	//	//16-=22明日礼包
	//	Urltest = "http://183.131.69.227:8004/pay/notifygameserverfortest" + "?UserID=" + nUserInfo.userID + "&cardtype=" + type;
	//	Debug.LogError ("Urltest" + Urltest);
	//	//		OpenWebScript.Instance.SetActivityWebUrl (Urltest);
	//	Application.OpenURL (Urltest);

	//}

	void Btnlose ()
	{
		UIColseManage.instance.CloseUI ();

		if (UIHallCore.PopSevenDaySignForFrist)
			UIHallCore.PopSevenDaySignForFrist = false;
		else {
			if (nUserInfo.isFinishTH)
			{
				if (UIHallCore.isFristNewSevenDay)
				{
					if (UIHallCore.Instance != null)
					{
						UIHallCore.isFristNewSevenDay = false;
						UIHallCore.Instance.ToActivity();
					}
				}
			}
			else
			{
				if (UIHallCore.isFristNewSevenDay && AppInfo.trenchNum > 51000000)
				{
					if (UIHallCore.Instance != null)
					{
						UIHallCore.isFristNewSevenDay = false;
						UIHallCore.Instance.ToPreferential();
					}
				}
			}
		}
        
		MyInfo myInfo = (MyInfo)Facade.GetFacade ().data.Get (FacadeConfig.USERINFO_MODULE_ID);
		if (ReSignDay.isFristActive) {
			for (int i = 0; i < myInfo.signInArray.Count; i++) {
				if (myInfo.signInArray [i].status == 1 && myInfo.signInArray [i].day > 0) {
					ReSignDay.isFristActive = true;
					break;
				} else {
					ReSignDay.isFristActive = false;
				}
			}
		}
	}
}
