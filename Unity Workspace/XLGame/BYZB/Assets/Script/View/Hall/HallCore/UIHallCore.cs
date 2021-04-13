using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;
using AssemblyCSharp;
using LitJson;
using DG.Tweening;

public class UIHallCore : MonoBehaviour, IUiMediator
{
	public string testresult;
	public string teststatus;
	//private AppControl appControl = null;
	private DataControl dataControl = null;
	private BackpackInfo backpackInfo = null;
	//private RankInfo rankInfo = null;

	private MyInfo myInfo = null;

	//	public static string nickNamestr;
	//public static bool tomrrow = true;

	//	public GameObject hallWindow;
	//存储背包数据的字典，为了根据物品的名字实例化这张图片
	//public static Dictionary <string ,FiBackpackProperty> arrayDic = new Dictionary<string, FiBackpackProperty> ();
	public Image bagJiaobiao;
	public Image friendJiaobiao;
	public Image taskJiaobiao;
	public Image mailJiaobiao;
	public Image bankJiaobiao;
	public Image activityJiaobiao;
	public Image welfarejiaobiao;
	public Image Settingjiaobiao;
	public Image SignTitle;
	//	public GameObject newHandEffect;
	//public GameObject redSceneEffect;
	//public GameObject redGiftLock;
	public GameObject container;
	public GameObject WelFare;
	public Image welfarebg;
	public Image SevenDay;
	public GameObject treasureButton;
	public GameObject doubleButton;
	public GameObject bossMatchVortex;
	public GameObject sevenDay;
	public GameObject upgradeActivity;
	public GameObject openSign;

	//大厅加入boss猎杀赛的漩涡

	private bool IsSHowWelFare = false;

	//起航礼包倒计时提示宝箱
	private GameObject mCountDownBoxTip;

	private InfoUpdateHelper mInfoUpdater = new InfoUpdateHelper ();

	NewSevenDayInfo sevenDayinfo;
	UpLevelInfo upLevelinfo;
	GameObject mWaitingView;
	//商城图标
	//public GameObject mailButton;
	//特惠图标
	public GameObject BtnPreference;
	//排行榜按钮
	public GameObject RankButton;

	public GameObject BtnVip;

	public GameObject BtnBindPhone;
	public static bool setBtnBinActivity = false;
	public static bool isShowBtnBind = false;
	public static bool isNeedToUpdate = false;
	public static bool isFirstComeing;
	public static bool isShowLevelUpgrade = true;
	public static bool PopSevenDaySignForFrist = false;
	public static bool FristTimeLeaveGame = false;

	
	public static bool isFristNewSevenDay = true;
	public static bool isFristActivity = true;
	public static bool isFristPreferential = true;
	public static bool isFristBigWineer = true;
	public static bool isFristSign = true;

	public static bool GameComeOutFromHall = false;

	//	string miniGameUrl = "https://web.clgame.cc/Activitys/SmallGameForXl/index.aspx?";

	string miniGameUrl = "";

	static string mServerUrl = "183.131.69.234";
	static string CLLoginUrl = "";
	static string MobileRegisterKey = "E39,D-=SM39S10-ATU85NC,53\\kg9";
	public static UIHallCore Instance;
	string key = "E39,D-=SM39S10-ATU85NC,53\\kg9";
	string URL = "http://api.cl0579.com/api/MobileFishSms.aspx";
	/// <summary>
	/// 活动红点是否显示
	/// </summary>
	public static bool isShow = true;
	/// 年
	/// </summary>
	string NowYear;
	/// <summary>
	/// 月
	/// </summary>
	string NowMouth;
	/// <summary>
	/// 日
	/// </summary>
	string NowDay;
	/// <summary>
	/// 当前时间
	/// </summary>
	public static string NowTime;
	/// <summary>
	/// 比对时间
	/// </summary>
	public static string timeStr;

	

	public void OnRecvData (int nType, object nData)
	{

	}

	public void OnInit ()
	{
		
	}

	public void OnRelease ()
	{

	}

	void Awake ()
	{
		
//		if (Facade.GetFacade ().config.isIphoneX ()) {
//			GetComponent<CanvasScaler> ().matchWidthOrHeight = 0.25f;
//		}
		//UISetSailGift.TomrrowEvent += TomorrowGift;
		//背包数据清空
		//判断当前有没有好友申请，以及有没有邮件
		//		appControl = AppControl.GetInstance ();
		Instance = this;
		Debug.Log("WebUrlDic = " + UIUpdate.WebUrlDic.Count);
		CLLoginUrl = UIUpdate.WebUrlDic[WebUrlEnum.ThirdToken];


		dataControl = DataControl.GetInstance ();
		if (null != dataControl) {
			myInfo = dataControl.GetMyInfo ();
			//			roomInfo = dataControl.GetRoomInfo ();
		}
		myInfo.connectServerAlr = true;
		myInfo.isconnecting = true;
        LoginUtil.GetIntance().ForStopIE();
		//新手七日签到界面資料更新
		DataControl.GetInstance().PushSocketSnd(FiEventType.SEND_XL_SEVENDAY_STARTINFO_OPEN_NEW_RESPOSE, null);
		//發送获取活动 升级任务进度信息
        if(AppInfo.trenchNum > 51000002)
		    DataControl.GetInstance().PushSocketSnd(FiEventType.SEND_XL_LEVELUP_INFO_NEW_RESPOSE, null);

		UIHallMsg.GetInstance ();

		//		msgBox = new MsgBox ();

		backpackInfo = dataControl.GetBackpackInfo ();
		//rankInfo = dataControl.GetRankInfo ();
		//nickName.text = myInfo.nickname;
		//nickNamestr = myInfo.nickname;

		/*if(myInfo.cannonMultipleMax >= 300)
		{
			//redSceneEffect.SetActive (false);
			//redGiftLock.SetActive (false);
		}*/
        
		AppInfo.isInHall = true;

		sevenDayinfo = (NewSevenDayInfo)Facade.GetFacade ().data.Get (FacadeConfig.UI_SEVENPART);
		if (AppInfo.trenchNum > 51000000)
			upLevelinfo = (UpLevelInfo)Facade.GetFacade().data.Get(FacadeConfig.UI_UPLEVEL);
		//Facade.GetFacade().ui.Add(FacadeConfig.UI_AND_BUTTON_CLOSE_OR_OPEN, this);

		if (FileUtilManager.IsExistsFile (LoginInfo.pathUrl, "GuestLogin.txt")) {
			InitGuestToFishing (LoginInfo.pathUrl, "GuestLogin.txt");
		}
		TimeControl ();

		isFristNewSevenDay = true;
	    isFristActivity = true;
	    isFristPreferential = true;
	    isFristBigWineer = true;
	    isFristSign = true;

	    InitLoadActivity (LoginInfo.pathUrl, "RedStatue.txt");
        Debug.Log("UIHallCore");
}

	void TimeControl ()
	{
		NowYear	= DateTime.Now.ToString ("yyyy");
		NowMouth = DateTime.Now.ToString ("MM");
		NowDay	= DateTime.Now.ToString ("dd");
		if (NowMouth.Length == 1) {
			NowMouth = "0" + NowMouth;
		}
		if (NowDay.Length == 1) {
			NowDay = "0" + NowDay;
		}
		NowTime = NowYear + NowMouth + NowDay;
	}

	/// <summary>
	/// 打开新的七日签到界面
	/// </summary>
	public void OpenNewSevenSign ()
	{
		//dataControl.PushSocketSnd(FiEventType.SEND_XL_SEVENDAY_STARTINFO_OPEN_NEW_RESPOSE, null);
		Debug.Log("新手七日 NewSevenDaySign");
		//DataControl.GetInstance().PushSocketSnd(FiEventType.SEND_XL_SEVENDAY_STARTINFO_OPEN_NEW_RESPOSE, null);
		//GameObject window = Resources.Load("Window/NewSevenDaySign") as GameObject;
		//GameObject.Instantiate(window);
		isFirstComeing = false;
	}

	/// <summary>
	/// 打开签到界面
	/// </summary>
	public void OpenSignInWindow ()
	{
		if (myInfo.isGuestLogin) {
			string path = "Window/BindPhoneNumber";
			GameObject jumpObj = AppControl.OpenWindow (path);
			jumpObj.SetActive (true);
			return;
		}
//		Debug.LogError ("11111111111111111111111111111");
		if (AppControl.GetInstance ().getActiveView () == AppView.HALL || AppControl.GetInstance ().getActiveView () == AppView.HALLNEWPLAY) {
			GameObject window = Resources.Load ("MainHall/Gift/SignWindow")as GameObject;
			GameObject.Instantiate (window);
		}
		isFirstComeing = false;
	}

	/// <summary>
	/// 2020/03/01 Joey 推廣版新增新手升級活動
	/// </summary>
	public void NewhandUpgrade()
	{
		//發送获取活动 升级任务进度信息
		DataControl.GetInstance().PushSocketSnd(FiEventType.SEND_XL_LEVELUP_INFO_NEW_RESPOSE, null);
		if (myInfo.isGuestLogin)
		{
            string path = "Window/BindAccount";
            GameObject jumpObj = AppControl.OpenWindow(path);
            jumpObj.SetActive(true);
            return;
		}
		//		Debug.LogError ("11111111111111111111111111111");
		if (AppControl.GetInstance().getActiveView() == AppView.HALL || AppControl.GetInstance().getActiveView() == AppView.HALLNEWPLAY)
		{
			Debug.Log(" 新手升級任務 NewLevelupGrade ");
			//GameObject window = Resources.Load("Window/NewLevelupGrade") as GameObject;
			//GameObject.Instantiate(window);
		}
	}

	/// <summary>
	/// 2020/03/20 Joey 綁定手機視窗
	/// </summary>
	public void OpenBindPhoneWindows()
	{
		//GameObject window = Resources.Load("Window/BindPhoneNumber") as GameObject;
		GameObject window = Resources.Load("Window/BindAccount") as GameObject;
		GameObject.Instantiate(window);
	}

	///// <summary>
	///// 2020/06/12 Joey 打開在線客服
	///// </summary>
	public void OpenCostomerService()
    {
#if UNITY_IPHONE
        OpenCostomerServiceForIos();
#else
		StartCoroutine(ChangeScreen());
#endif
	}

    private IEnumerator ChangeScreen()
	{
		string path = "Window/OpenUrlCanvas";
		miniGameObj = AppControl.OpenWindow(path);
		miniGameObj.SetActive(true);
		//先创建控件,在进行浏览
		if (OpenWebScript.Instance != null)
		{
			//打开webview控件
			OpenWebScript.Instance.SetActivityWebUrl(UIUpdate.WebUrlDic[WebUrlEnum.OnlineCostomerService]);
			yield return Screen.orientation = ScreenOrientation.Portrait;

		}
		//AppControl.miniGameState = true;
		//产品需求说需要关闭音效
		AudioManager._instance.StopBgm();
    }

    /// <summary>
    /// 2020/06/12 Joey 打開在線客服
    /// </summary>
    public void OpenCostomerServiceForIos()
    {
        string path = "Window/OpenUrlCanvas";
        miniGameObj = AppControl.OpenWindow(path);
        miniGameObj.SetActive(true);
        //先创建控件,在进行浏览
        if (OpenWebScript.Instance != null)
        {
			//打开webview控件
			Application.OpenURL(UIUpdate.WebUrlDic[WebUrlEnum.OnlineCostomerService]);
		}
        //AppControl.miniGameState = true;
        //产品需求说需要关闭音效
        AudioManager._instance.StopBgm();
    }

    public void SetupTopControl()
    {
		NewSevenDayInfo nSevenDayinfo = (NewSevenDayInfo)Facade.GetFacade().data.Get(FacadeConfig.UI_SEVENPART);
		////Debug.LogError ( "----------open sign window---------" );
		Debug.LogError("myinfoSeven = " + myInfo.IsNewSevenUser + " ;nSevenDayinfo.nresult = " + nSevenDayinfo.nresult);
		
		// myInfo.IsNewSevenUser 1 表示 新注册的七日 用户， 2 或者其他的表示 老用户
		// nresult -100 不是新戶了
		testresult = nSevenDayinfo.nresult.ToString();
		teststatus = myInfo.IsNewSevenUser.ToString();
		if (myInfo.IsNewSevenUser == 1 || nSevenDayinfo.nresult == -100)
		{
			if (nSevenDayinfo.nresult == -100)
			{
				//2020/03/03 Joey 判斷用戶是否為新用戶，不是則顯示一般簽到
				openSign.SetActive(true);       //簽到     顯示
				try
				{
					sevenDay.SetActive(false);      //七日禮包 不顯示
				}
				catch { }
				
				myInfo.isNewSevenHand = false;
				//if(isFirstComeing)
				//  OpenSignInWindow();
			}
			    //} else if(nSevenDayinfo.ncurDay != 0)
			else
			{
				//2020/03/03 Joey 判斷用戶是否為新用戶，是則顯示新手七日福利
				try
				{
					sevenDay.SetActive(true);       //七日禮包 顯示
				}
				catch { }
				
				openSign.SetActive(false);      //簽到    不顯示
				myInfo.isNewSevenHand = true;

			//OpenNewSevenSign ();
			//DataControl.GetInstance ().PushSocketSnd (FiEventType.SEND_XL_SEVENDAY_STARTINFO_OPEN_NEW_RESPOSE, null);
			if (isFirstComeing) { }
					//OpenNewSevenSign();
				//string path = "Window/NewSevenDaySign";
				//GameObject jumpObj = AppControl.OpenWindow(path);
				//jumpObj.SetActive(true);
			}
		}
		else
		{
			////2020/03/03 Joey 判斷用戶是否為新用戶，不是則顯示一般簽到
			//sevenDay.SetActive(false);
			//openSign.SetActive(true);
			myInfo.isNewSevenHand = false;
			//if (isFirstComeing)
			//	OpenSignInWindow();
		}

        //2020/03/06 Joey 判斷用戶是否領取完畢所有新手升級獎勵
        Debug.LogError("UIHallCore isUserGetAllUpLevel = " + myInfo.isUserGetAllUpLevel);
        //2020/05/22 Joey 先判斷是否為推廣渠道，在判斷是否顯示新手升級獎勵
		if (AppInfo.trenchNum > 51000000)
		{
			if (!myInfo.isUserGetAllUpLevel)
			{
				try
				{
					upgradeActivity.SetActive(true);
				}
				catch { }
				
				isShowLevelUpgrade = true;
			}
			else
			{
				try
				{
					upgradeActivity.SetActive(false);
				}
				catch { }

				isShowLevelUpgrade = false;
			}
		}
	}

    /// <summary>
    /// 判斷左欄顯示
    /// </summary>
	public void SetupLeftControl() {
		Debug.Log(" 手機綁定 " + myInfo.isBindPhone);
		if (myInfo.isBindPhone == 1)
		{
            //用戶已綁定手機
			BtnBindPhone.SetActive(false);
		}
        else{
			//用戶未綁定手機
			BtnBindPhone.SetActive(true);
		}
	}

 //   /// <summary>
 //   /// 判斷是否顯示手機綁定
 //   /// </summary>
 //   /// <param name="isVisuable">顯示或不顯示</param>
	//public void SetupBindBtn(bool isVisuable) {
	//	BtnBindPhone.SetActive(isVisuable);
	//}

    /// <summary>
    /// 顯示彈框
    /// </summary>
	public void ShowPopWindows()
    {
		
		if (myInfo.isNewSevenHand && myInfo.IsNewSevenUser == 1 && AppInfo.trenchNum > 51000000)    //7日新用户
		{
			Debug.Log("新手七日 NewSevenDaySign");
			//OpenNewSevenSign(); 
		}
		else    //非7日新用户
		{
			OpenSignInWindow();
		}

		if (AppInfo.isFritsInGame)
		{
			AppInfo.isFritsInGame = false;
			//Debug.LogError("isfristgame" + AppInfo.isFritsInGame);
		}
	}


	//Use this for initialization
	void Start ()
	{
		//newHandEffect.SetActive (false);
		UIStore.HideEvent += HideSelf;
		UIVIP.SeeEvent += SeeSelf;
		UIUpgrade.HideEvent += HideSelf;

		//Joey 如果是從漁場出來
		if (GameComeOutFromHall) {
			isFristNewSevenDay = false;
	        isFristActivity = false;
	        isFristPreferential = false;
	        isFristBigWineer = false;
	        isFristSign = false;
			GameComeOutFromHall = false;
		}
		//UISetSailGift.SeeHandEvent += NewHandEffect;

		//当用户第一次解锁30倍炮台的时候出现手指引
		//启航礼包，判断是否新用户，新用户首次登录，必弹出UIHallMag中RcvStartGameInform
		//第二天炮倍数达到五级则解锁炮，则可以领取第二天的礼包,否则不可以领取，第三天达到炮倍数，直接领取第三天的礼包
		//有礼包的时候不会自动弹出签到系统，离开大厅再次进入大厅的时候弹出
		//如果不是3天的新用户，直接弹出签到系统
		//如果是3天用户则先弹出启航礼包然后再次回到大厅的时候弹出签到系统，或者当日再次登录时弹出
		//签到系统，每次弹出签到系统的时候向服务器发送当前系统时间，服务器匹配当前日期，如果是当前时间可用
		//服务器记录当前用户的登录信息，登录几天
		//每次登录的时候需要读取是否有新的邮件，接收服务器的消息如果有则显示角标。
		//每次登录的时候需要读取是否有新的消息，每次进入大厅有消息则需要在好友列表出显示角标
		//每次登录的时候需要读取任务，每日登录必领取一个完成任务，每次进入大厅完成一个任务也要显示角标；
		//背包在启航礼包的时候领取后显示角标，如果获取新的道具也要显示角标
		//		string path = "Window/EveryDayReward";
		//		WindowClone = AppControl.OpenWindow (path);
		//		WindowClone.SetActive (true);
		//		myInfo.sailDay = 1;

		NewSevenDayInfo nSevenDayinfo = (NewSevenDayInfo)Facade.GetFacade ().data.Get (FacadeConfig.UI_SEVENPART);
		

		//签到系统,为了启航礼包没有一开始弹出签到系统，在这里判断是否有弹出该窗口
		//Debug.LogError( myInfo.sailDay +"/"+ myInfo.bDisplayedStartGift + "/" + myInfo.signInArray.Count );
		//Debug.LogError ("myInfo.sailDay = " + myInfo.sailDay + " ;myInfo.bDisplayedStartGift = " + myInfo.bDisplayedStartGift + " ;myInfo.GetSignStatue() = " + myInfo.GetSignStatue());
		//Debug.LogError("myInfo.isGuestLogin = " + myInfo.isGuestLogin + " ;myinfoisguestlogin = " + myInfo.isGuestLogin + " ;myInfo.signInArray.Count = " + myInfo.signInArray.Count + " ;myInfo.getSignInDay() = " + myInfo.getSignInDay());
		if ((myInfo.sailDay == 0 || myInfo.bDisplayedStartGift) && myInfo.signInArray != null && myInfo.signInArray.Count > 0 && myInfo.getSignInDay() != 0 && myInfo.isGuestLogin == false && myInfo.GetSignStatue() == 0)
        {

        }
        else
        {
            if (myInfo.isGuestLogin == true)
            {

            }
            else
            {
				//sevenDay.SetActive(false);
				//openSign.SetActive(true);
            }

        }
        
		JudegeBossMatchState ();
//		myInfo.isTestRoom = 0;
		if (myInfo.isGuestLogin) {
			Settingjiaobiao.gameObject.SetActive (true);
		}
		if (myInfo.isTestRoom == 0 && myInfo.isGuestLogin) {
			LoginUtil.ShowWaitingMask (true);
		}
		for (int i = 0; i < myInfo.signInArray.Count; i++) {
			if (myInfo.signInArray [i].status == 1 && myInfo.signInArray [i].day > 0) {
				DailySignInfo nInfo = (DailySignInfo)Facade.GetFacade ().data.Get (FacadeConfig.SIGN_IN_MODULE_ID);
				nInfo.Setissign (true);
			}
		}

		Facade.GetFacade ().ui.Add (FacadeConfig.UI_HALL_MODULE_ID, this);
		//myInfo.avatar = "https://gss0.baidu.com/94o3dSag_xI4khGko9WTAnF6hhy/zhidao/pic/item/03087bf40ad162d97a2bc4711bdfa9ec8b13cd92.jpg";

		//添加头像
		/*if ( !string.IsNullOrEmpty( myInfo.avatar) )
		{
			AvatarInfo nAvaInfo = ( AvatarInfo )Facade.GetFacade ().data.Get ( FacadeConfig.AVARTAR_MODULE_ID );
			nAvaInfo.Load ( myInfo.userID , myInfo.avatar , OnRecvAvatarResponse );
		}*/

		Facade.GetFacade ().message.friend.SendGetFriendList ();
		StartCoroutine ("UpdateClientInfo");

		//myInfo.sailDay = 1;
		// myInfo.bDisplayedStartGift
		OnProcessStartGift();

		//在渔场内转盘结束后点击购买龙卡就跳转去大厅打开龙卡购买
		if (UIColseManage.instance.isFishingBuyLongCard) {
			UIColseManage.instance.isFishingBuyLongCard = false;
			MouthCard ();
		}

		if (myInfo.IsNewSevenUser != 1 || nSevenDayinfo.nresult == -100) {
			SevenDay.gameObject.SetActive (false);
			SignTitle.gameObject.SetActive (true);
			SignTitle.GetComponent<Button> ().enabled = true;
		} else {
			Debug.LogError ("222222222222222");
			SevenDay.gameObject.SetActive (true);
			SignTitle.gameObject.SetActive (false);
			SignTitle.GetComponent<Button> ().enabled = false;
			DailySignInfo nInfo = (DailySignInfo)Facade.GetFacade ().data.Get (FacadeConfig.SIGN_IN_MODULE_ID);
			nInfo.Setissign (false);
		}

		//已经领取了起航礼包奖励
		if (myInfo.sailDay <= 0 && myInfo.level <= 5) {
			ShowHandEffect ();
		}
			
		//Debug.LogError ("---------------------" + myInfo.loginInfo.preferencePackBought);
		if (BtnPreference != null) {
			//如果特惠礼包没有购买了，那么显示商城图标
			//			if (myInfo.loginInfo.preferencePackBought == 0 || myInfo.loginInfo.preferencePackBought == 1) {
			//			
			//				BtnPreference.SetActive (true);
			//			} else {
			//				BtnPreference.SetActive (false);
			//			}
			if (AppInfo.trenchNum == 51000000 || AppInfo.trenchNum == 51000002)
			{
				BtnPreference.SetActive(false);
			}
			else
            {
				if (myInfo.teihui < 0 || myInfo.teihui >= 5)
				{
					BtnPreference.SetActive(false);
				}
				else
				{
					BtnPreference.SetActive(true);
				}
			}
		}

		if (AppInfo.isReciveHelpMsg && AppInfo.isInHall) {
			OpenHelpTaskReward ();
		}

	
#if UNITY_IPHONE && !UNITY_EDITOR
		if ((AppInfo.phoneUUID != null || AppInfo.phoneUUID != "") && myInfo.isGuestLogin == true) {
			StartCoroutine (GuestToFishing ());
		}
#elif UNITY_ANDROID && !UNITY_EDITOR
		if ((AppInfo.phoneUUID != null || AppInfo.phoneUUID != "") && myInfo.isGuestLogin == true) {
			StartCoroutine (GuestToFishing ());
		}
#endif

		if ((AppInfo.phoneUUID != null || AppInfo.phoneUUID != "") && myInfo.isGuestLogin == true) {
			StartCoroutine (GuestToFishing ());
		}
		if (Facade.GetFacade ().config.isIphoneX2 ()) {
			container.transform.localScale = new Vector3 (0.75f, 0.75f, 0.75f);
		} else {
			container.transform.localScale = new Vector3 (1f, 1f, 1f);
		}
		if (timeStr != NowTime && isShow == false) {
			//ActivityNew (true);
			FileUtilManager.DeleteFile (LoginInfo.pathUrl, "RedStatue.txt");
			StartCoroutine (ReloadActivity ());
		}
//		Debug.LogError ("BossMatchScript.beginType = " + BossMatchScript.beginType);
//		Debug.LogError ("UIColseManage.instance.intoBossRommType = " + UIColseManage.instance.intoBossRommType);
		if (BossMatchScript.beginType == 1) {
			StartCoroutine (BossMatchToFish ());
		}  
		if (UIColseManage.instance.intoBossRommType == 0) {
			StartCoroutine (BossToFish ());
			UIColseManage.instance.intoBossRommType = -1;
		}
		//		ActivityNew (isShow);


		SetupTopControl();
		SetupLeftControl();

        //繁體版不顯示、推廣板才顯示
		if (AppInfo.trenchNum > 51000000)
		{
			IsShowTreasure();
			IsShowDouble();
		}

        //是否領啟航禮包
		if (!myInfo.bDisplayedStartGift) {
            //第一次進入大廳
			if (AppInfo.isFritsInGame)
			{
				ShowPopWindows();
			}
		}

		Debug.Log("myInfo.isNewSevenHand = " + myInfo.isNewSevenHand);
		Debug.Log("myInfo.IsNewSevenUser = " + myInfo.IsNewSevenUser);
		//用來判斷剛領完啟航禮包第一次離開遊戲
		if (FristTimeLeaveGame && myInfo.IsNewSevenUser == 1 && AppInfo.trenchNum > 51000000) {
			Debug.Log("新手七日 NewSevenDaySign");
			//OpenNewSevenSign();
			FristTimeLeaveGame = false;
		}
			

		isNeedToUpdate = false;

		//Debug.LogError("-------UIHallCored-------start-------loginType = " + myInfo.loginType);
		//Debug.LogError("-------UIHallCored-------start-------nickname = " + myInfo.nickname);
		//Debug.LogError("-------UIHallCored-------start-------openId = " + myInfo.openId);
		//Debug.LogError("-------UIHallCored-------start-------acessToken = " + myInfo.acessToken);
		//Debug.LogError("-------UIHallCored-------start-------account = " + myInfo.account);
		//Debug.LogError("-------UIHallCored-------start-------username = " + myInfo.mLoginData.username);
		try {
			UiAndButtonCloseOrOpen();
		}
		catch (Exception e) {
			Debug.Log(e.ToString());
        }
	}

	public void HindSevnDay ()
	{
		SevenDay.gameObject.SetActive (false);
		SignTitle.gameObject.SetActive (true);
		SignTitle.GetComponent<Button> ().enabled = true;
		OpenSignInWindow ();
	}

	// Update is called once per frame
	void Update()
	{
		if (setBtnBinActivity) {
			Debug.LogError("-------UIHallCored-------Update-------");
			BtnBindPhone.SetActive(isShowBtnBind);
			setBtnBinActivity = false;
		}

		if (isNeedToUpdate)
		{
            //2020/03/06 Joey 判斷用戶是否領取完畢所有新手升級獎勵
            Debug.LogError("-----UIHallCore isUserGetAllUpLevel = " + myInfo.isUserGetAllUpLevel);
			isNeedToUpdate = false;
			//新手七日签到界面資料更新
			DataControl.GetInstance().PushSocketSnd(FiEventType.SEND_XL_SEVENDAY_STARTINFO_OPEN_NEW_RESPOSE, null);
			//發送获取活动 升级任务进度信息
            if (AppInfo.trenchNum > 51000000)
			    DataControl.GetInstance().PushSocketSnd(FiEventType.SEND_XL_LEVELUP_INFO_NEW_RESPOSE, null);

			if (!myInfo.isUserGetAllUpLevel)
            {
				try
				{
					upgradeActivity.SetActive(true);
				}
				catch { }
				
				isShowLevelUpgrade = true;
			}
            else
            {
				try
				{
					upgradeActivity.SetActive(false);
				}
				catch { }
				
				isShowLevelUpgrade = false;
			}

            isNeedToUpdate = false;
		}
    }

	/// <summary>
	/// 是否显示发现宝藏礼包的图标
	/// </summary>
	private void IsShowTreasure ()
	{
		if (AppInfo.trenchNum > 51000000) {
			string flag = PlayerPrefs.GetString(myInfo.userID + "isShowTreasure", "-1");
			Debug.Log(" !!!!!!!!!! IsShowTreasure flag = " + flag + " !!!!!!!!!! myInfo.showTreasure = " + myInfo.showTreasure);
			try
			{
				if (flag == "1" && myInfo.showTreasure == 1)
					treasureButton.gameObject.SetActive(true);
				else
					treasureButton.gameObject.SetActive(false);
			}
			catch { }
		}

	}

	/// <summary>
	/// 是否显示双喜临门礼包的图标
	/// </summary>
	private void IsShowDouble ()
	{
		if (AppInfo.trenchNum > 51000000)
		{
			string flag = PlayerPrefs.GetString(myInfo.userID + "isShowDouble", "-1");
			Debug.Log(" !!!!!!!!!! IsShowDouble flag = " + flag + " !!!!!!!!!! myInfo.showDouble = " + myInfo.showDouble);
			try
			{
				if (flag == "1" && myInfo.showDouble == 1)
					doubleButton.gameObject.SetActive(true);
				else
					doubleButton.gameObject.SetActive(false);
			}
			catch { }
			
		}
	}

	IEnumerator ReloadActivity ()
	{
		yield return new WaitForSeconds (.3f);
		string nolongerStr = "{\"time\":{" + "\"isShow\":" + (isShow ? 1 : 0) + ","
		                     + "\"newtime\":" + NowTime + "}}";
		FileUtilManager.CreateFile (LoginInfo.pathUrl, "RedStatue.txt", nolongerStr);
		//Debug.LogError ("ReloadActivity isShow = " + isShow);
		//Debug.LogError ("NowTime =  " + NowTime);
	}

	IEnumerator GuestToFishing ()
	{
		yield return new WaitForSeconds (.2f);
		AppInfo.isFrist = false;
		if (myInfo.isTestRoom == 0) {
			UIHallObjects.GetInstance ().PlayExperience ();
			GuestToFishingIsLoad (LoginInfo.pathUrl);
			LoginUtil.ShowWaitingMask (false);
		}
	}

	IEnumerator BossMatchToFish ()
	{
		yield return new WaitForSeconds (.3f);
		UIHallObjects.GetInstance ().PlayBossMatch ();
	}

	IEnumerator BossToFish ()
	{
		yield return new WaitForSeconds (.3f);
		UIHallObjects.GetInstance ().PlayFieldGrade_4 ();
	}

	/// <summary>
	/// 游客进入渔场
	/// </summary>
	/// <param name="pathUrl">Path URL.</param>
	void GuestToFishingIsLoad (string pathUrl)
	{
		string loginStr = "{\"login\":{"
		                  + "\"isiFristLogin\":" + (AppInfo.isFrist ? 1 : 0) + "}}";
		FileUtilManager.CreateFile (pathUrl, "GuestLogin.txt", loginStr);
	}

	/// <summary>
	/// 初始化游客登录的状态
	/// </summary>
	/// <param name="path">Path.</param>
	/// <param name="filename">Filename.</param>
	void  InitGuestToFishing (string path, string filename)
	{
		string data = FileUtilManager.LoadFile (path, filename);
		if (data == null || data == "") {
			return;
		}
		LitJson.JsonData jd = LitJson.JsonMapper.ToObject (data);

		int isFrist = (int)jd ["login"] ["isiFristLogin"];
		AppInfo.isFrist = isFrist == 0 ? false : true;
		//Debug.Log ("AppInfo.isFrist = " + AppInfo.isFrist);
	}

	public void OnShowMailButton ()
	{
//		BtnPreference.SetActive ( true );
		//BtnVip.transform.localPosition = new Vector3 ( BtnPreference.transform.localPosition.x , BtnPreference.transform.localPosition.y , 0 );
//		mailButton.SetActive ( true );
//		preferenceButton.SetActive ( false );
	}

	//处理起航礼包的消息
	public void OnProcessStartGift ()
	{
		////Debug.LogError ( "------------------" + myInfo.bDisplayedStartGift + " / " +  myInfo.sailDay );
		//如果没有显示过起航礼包,并且有起航礼包的消息，显示起航礼包
		if (!myInfo.bDisplayedStartGift && myInfo.sailDay > 0) {

			myInfo.bDisplayedStartGift = true;
			if (myInfo.platformType == 22 || myInfo.platformType == 24) {
				Debug.Log("~~~~~~~~~~~~~~~~~~~~~~~~~~~~~UIHallCore~~~~~~~~~~~~~~~~~~~~~~~~~~~~~OnProcessStartGift 1");
				StartGiftManager.OpenStartGiftWindow ();
				PopSevenDaySignForFrist = true;
				//				if( (myInfo.sailDay == -1 || myInfo.sailDay == -2 ) && myInfo.cannonMultipleMax >= 5 ){
				//					ShowStartGiftBoxTip ();
				//				}
			} else {
				if (myInfo.sailDay == 1) {
					Debug.Log("~~~~~~~~~~~~~~~~~~~~~~~~~~~~~UIHallCore~~~~~~~~~~~~~~~~~~~~~~~~~~~~~OnProcessStartGift 2");
					StartGiftManager.OpenStartGiftWindow ();
					PopSevenDaySignForFrist = true;
				}
			}
		}
		if (myInfo.platformType == 22 || myInfo.platformType == 24) {
			if ((myInfo.sailDay == -1 || myInfo.sailDay == -2) && myInfo.cannonMultipleMax >= 5) {
				ShowStartGiftBoxTip ();
			}
		}

		/*myInfo.sailDay = 1;*/
		//如果是领取起航礼包的第二天，那么显示倒计时图标,并且炮倍数已经解锁到5级了，那么显示倒计时
		//myInfo.sailDay = -2;
		//如果已经领取过起航礼包了,并且最大炮倍数解锁到5倍了，那么显示倒计时提醒宝箱
		//StartGiftManager.OpenCountDownWindow ( 2 );
	}

	void OpenHelpTaskReward ()
	{
		StartGiftManager.HelpTaskRewardWindow (AppInfo.isInHall);
	}

	//appid  wxbbbd13c64debe77a
	//appsecret 9207abf11315eae171d7a87a167e799a

	//	void OnRecvAvatarResponse( int nResult , Texture2D nImage )
	//	{
	//		////Debug.LogError ("-------------------/" + nResult);
	//		/*if (nResult == 0 && MyHead != null) {
	//			nImage.filterMode = FilterMode.Bilinear;
	//			nImage.Compress (true);
	//
	//			MyHead.sprite = Sprite.Create (nImage, new Rect (0, 0, nImage.width, nImage.height), new Vector2 (0, 0));
	//		}*/
	//	}

	//每隔5s更新信息，如果有相关更新数据，那么显示角标
	IEnumerator UpdateClientInfo ()
	{
		FriendInfo nInfoFriend = (FriendInfo)Facade.GetFacade ().data.Get (FacadeConfig.FRIEND_MODULE_ID);
		MailDataInfo nMailInfo = (MailDataInfo)Facade.GetFacade ().data.Get (FacadeConfig.MAIL_MODULE_ID);
		BackpackInfo nBackInfo = (BackpackInfo)Facade.GetFacade ().data.Get (FacadeConfig.BACKPACK_MODULE_ID);
		TaskInfo mTaskInfo = (TaskInfo)Facade.GetFacade ().data.Get (FacadeConfig.TASK_MODULE_ID);
		BankInfo mBankInfo = (BankInfo)Facade.GetFacade ().data.Get (FacadeConfig.UI_BANk_MOUDLE_ID);
		DailySignInfo nInfo = (DailySignInfo)Facade.GetFacade ().data.Get (FacadeConfig.SIGN_IN_MODULE_ID);
		int nRequestFrequent = 0;
		long rankRequstTime = 0;
		while (true) {
			if (nRequestFrequent == 0)
				mInfoUpdater.DoUpdateInfoRequest ();
			nRequestFrequent++;
			if (nRequestFrequent >= 10) {
				rankRequstTime += 10;
				nRequestFrequent = 0;
			}
			if (rankRequstTime >= 600) {
				Facade.GetFacade ().message.rank.SendGetRankInfoRequest (1);
				Facade.GetFacade ().message.rank.SendGetRankInfoRequest (0);
				rankRequstTime = 0;
			}
			yield return new WaitForSeconds (1.0f);

			//背包有更新的数据，那么设置背包的角标
			//BagNewTool (nBackInfo.isUpdated ());
			////Debug.LogError ("Hallcore!!!!bag"+nBackInfo.isUpdated ());


			/*
             * 2020/03/04 Joey 推廣包不需要顯示
			//好友有更新的数据，设置好友角标
			FriendNewInfo (nInfoFriend.isUpdated ());
			////Debug.LogError ("Hallcore!!!!friend"+nInfoFriend.isUpdated ());
			//邮件有更新的数据，设置邮件角标
			MailNew (nMailInfo.isUpdated ());
			////Debug.LogError ("Hallcore!!!!nMailInfo"+nMailInfo.isUpdated ());
			//任务模块角标设置
			TaskNew (mTaskInfo.isUpdated ());
			////Debug.LogError ("Hallcore!!!!mTaskInfo"+mTaskInfo.isUpdated ());
			BankNewInfo (mBankInfo.isUpdate);
			WelFafeNew (nInfo.IsUpdated ());
            */
		}
	}


	//	//获取头像
	//	public void GetMyHead(Texture2D image)
	//	{
	//		image.filterMode = FilterMode.Bilinear;
	//		image.Compress (true);
	//		MyHead.sprite = Sprite.Create(image, new Rect (0, 0, image.width,image.height),new Vector2(0,0));
	//	}


	/* 2020/03/04 Joey 推廣包不需要紅點
	//public void BankNewInfo (bool newInfo)
	//{
	//	if (bagJiaobiao != null) {
	//		if (bankJiaobiao.gameObject.activeSelf != newInfo)
	//			bankJiaobiao.gameObject.SetActive (newInfo);
	//		if (BankManager.instance != null) {
	//			BankManager.instance.RedPoint.SetActive (newInfo);
	//		}
	//	}
	//}

	//2020/03/04 Joey 推廣包不需要紅點
	//public void BagNewTool (bool newTool)
	//{
	//	if (bagJiaobiao != null) {
	//		if (bagJiaobiao.gameObject.activeSelf != newTool)
	//			bagJiaobiao.gameObject.SetActive (newTool);
	//	}
	//}

	//public void FriendNewInfo (bool newInfo)
	//{
	//	if (bagJiaobiao != null) {
	//		if (friendJiaobiao.gameObject.activeSelf != newInfo)
	//			friendJiaobiao.gameObject.SetActive (newInfo);
	//	}
	//}

	//public void TaskNew (bool newTask)
	//{
	//	if (bagJiaobiao != null) {
	//		if (taskJiaobiao.gameObject.activeSelf != newTask)
	//			taskJiaobiao.gameObject.SetActive (newTask);
	//	}
	//}

	//public void MailNew (bool newMail)
	//{
	//	if (bagJiaobiao != null) {
	//		if (mailJiaobiao.gameObject.activeSelf != newMail)
	//			mailJiaobiao.gameObject.SetActive (newMail);
	//	}
	//}

	//public void ActivityNew (bool isShowRed)
	//{
	//	if (bagJiaobiao != null) {
	//		if (activityJiaobiao.gameObject.activeSelf != isShowRed)
	//			activityJiaobiao.gameObject.SetActive (isShowRed);
	//	}
	//}

	//public void WelFafeNew (bool isShowRed)
	//{
	//	if (welfarejiaobiao != null) {
	//		if (welfarejiaobiao.gameObject.activeSelf != isShowRed) {
	//			welfarejiaobiao.gameObject.SetActive (isShowRed);
	//		}
	//	}
	//}
    */

	void HideSelf ()
	{
		BtnVip.GetComponent<Button> ().interactable = false;
		gameObject.SetActive (false);
	}

	void AllowVipClick ()
	{
		BtnVip.GetComponent<Button> ().interactable = true;
	}

	void SeeSelf ()
	{
		
		Invoke ("AllowVipClick", 1f);
		gameObject.SetActive (true);
	}


	public void TestAddGold ()
	{
		Facade.GetFacade ().message.toolPruchase.SendTopupRequest (FiPropertyType.GOLD, 6);
	}

	public void TestAddDiamond ()
	{
		Facade.GetFacade ().message.toolPruchase.SendTopupRequest (FiPropertyType.DIAMOND, 348);
	}

	public void ShowHandEffect ()
	{
		//newHandEffect.SetActive (true);
	}

	//	void OnGUI()
	//	{
	//		Tool.ShowInGUI ();
	//	}

	public void ToCash ()
	{
		AudioManager._instance.PlayEffectClip (AudioManager.ui_click);
		string path = "Window/CashPrize";
		GameObject WindowClone = AppControl.OpenWindow (path);
		WindowClone.SetActive (true);
	}

	public void ToBag ()
	{
		AudioManager._instance.PlayEffectClip (AudioManager.ui_click);
		//backpackInfo.SetUpdate ( false );
		string path = "MainHall/BackPack/BagMainWindow";//"Window/BagWindow";
		GameObject WindowClone = AppControl.OpenWindow (path);
		WindowClone.SetActive (true);
		Facade.GetFacade ().message.backpack.SendReloadRequest ();
	}

	public void Toopenrank ()
	{
		dataControl.PushSocketSnd (FiEventType.SEND_XL_HORODATA_RESQUSET, null);
		AudioManager._instance.PlayEffectClip (AudioManager.ui_click);
		//backpackInfo.SetUpdate ( false );
		string path = "MainHall/RankList/BigwinRankWindowCanvas";//"Window/BagWindow";
		GameObject WindowClone = AppControl.OpenWindow (path);
		WindowClone.SetActive (true);
		//		Facade.GetFacade ().message.backpack.SendReloadRequest ();
	}

	public void ToBank ()
	{
		Facade.GetFacade ().message.backpack.SendReloadRequest ();
		AudioManager._instance.PlayEffectClip (AudioManager.ui_click);
		string path;
		GameObject WindowClone;
		//myInfo.isGuestLogin = false;
		myInfo.loginInfo.hasBankPswd = myInfo.loginInfo.hasBankPswd ? true : myInfo.isCLAccountLogin;
		if (myInfo.isGuestLogin) {
			path = "Window/WindowTips";
			WindowClone = AppControl.OpenWindow (path);
			var temp = WindowClone.GetComponent<UITipClickHideManager> ();
			temp.text.text = "遊客模式無法使用該功能";
			temp.time.text = "9";
			WindowClone.SetActive (true);
			return;
		} else if (!myInfo.loginInfo.hasBankPswd) {
			path = "MainHall/Common/NoticeTipsOne";
			WindowClone = AppControl.OpenWindow (path);
			var temp = WindowClone.GetComponent<NoticeTipsOneC> ();
			temp.RemoveListener ();
			temp.content.text = "為了帳號安全，請設置銀行密碼！";
			Image no = temp.store.GetComponent<Image> ();
			Image yes = temp.refuse.GetComponent<Image> ();
			no.sprite = UIHallTexturers.instans.Bank [10];
			no.SetNativeSize ();
			no.rectTransform.localScale = Vector3.one * 0.78f;
			yes.sprite = UIHallTexturers.instans.Bank [11];
			yes.SetNativeSize ();
			yes.rectTransform.localScale = Vector3.one * 0.78f;
			temp.store.onClick.AddListener (() => Destroy (temp.gameObject));
			temp.refuse.onClick.AddListener (() => {
				string mpath = "MainHall/Common/BankSetPassword";
				GameObject mWindowClone = AppControl.OpenWindow (mpath);
				mWindowClone.SetActive (true);
				Destroy (temp.gameObject);
			});
			WindowClone.SetActive (true);
			return;
		}
		path = "MainHall/Bank/BankWindowNew";
		WindowClone = AppControl.OpenWindow (path);
		WindowClone.SetActive (true);
	}

	public void ToNotice ()
	{
		BroadCastManager.instans.broadCastObj.GetComponent<Button> ().interactable = false;
		AudioManager._instance.PlayEffectClip (AudioManager.ui_click);
		string path = "Window/NoticeWindow";
		GameObject WindowClone = AppControl.OpenWindow (path);
		WindowClone.SetActive (true);
	}
	//public delegate void GoodFriendDelegate();
	//public static event GoodFriendDelegate GoodFriendEvent;

	public void ToGoodFriend ()
	{
		AudioManager._instance.PlayEffectClip (AudioManager.ui_click);
		//发送获取当前好友数据请求
		string path = "MainHall/Friend/FriendWindowNew";
		GameObject WindowClone = AppControl.OpenWindow (path);
		WindowClone.SetActive (true);
		//if (GoodFriendEvent != null) {
		//	GoodFriendEvent ();
		//}
		//		GameObject Window = Resources.Load ("Window/GoodFriends")as GameObject;
		//		WindowClone = Instantiate (Window);
		//在回复里面获取数据FriendMsgHandle类里面
		//同上注册获取申请好友的数据
		//同上注册获取游戏好友的数据
	}

	public void ToRank ()
	{
		AudioManager._instance.PlayEffectClip (AudioManager.ui_click);
		string path = "MainHall/RankList/RankWindowCanvas";
		GameObject WindowClone = AppControl.OpenWindow (path);
		WindowClone.SetActive (true);
		//同上注册获取排行的数据，记录时间，30分钟要刷新一次
	}

	public void ToStore ()
	{
		//Debug.LogError ("----------to store -------");
		AudioManager._instance.PlayEffectClip (AudioManager.ui_click);
		if (myInfo.isGuestLogin) {
			StartGiftManager.GuestToStoreManager ();
			//return;
		}
		//先判断是否有没有首充过，如果首充过，则先弹出首充特惠的窗口，首充过则直接弹出商城的界面
		string path = "Window/NewStoreCanvas";
		GameObject WindowClone = AppControl.OpenWindow (path);
		WindowClone.SetActive (true);
		WindowClone.GetComponent<UIStore> ().CoinButton ();
	}

	//public delegate void InitMailDelegate();
	//public static event InitMailDelegate InitMailEvent;

	//打开邮件模块
	public void ToMail ()
	{
		/*//剩余的月卡天数 > 0 就是月卡用户
		myInfo.loginInfo.monthlyCardDurationDays;
		//是否购买了特惠礼包，是为true
		myInfo.loginInfo.preferencePackBought;*/

		string path = "MainHall/Mail/MailWindowNew";
		GameObject nMailWindow = AppControl.OpenWindow (path);
		nMailWindow.SetActive (true);
		MailDataInfo nInfo = (MailDataInfo)Facade.GetFacade ().data.Get (FacadeConfig.MAIL_MODULE_ID);
		if (nInfo.getMailList ().Count == 0)
			AudioManager._instance.PlayEffectClip (AudioManager.ui_click);


		/*//Debug.Log ("初始化系统邮件");
		if (InitMailEvent != null) {
			InitMailEvent ();
		}*/
	}

	public void ToPreferential()
	{
		//string path = "MainHall/Activity/ActivityCanvas";
		string path = "Window/Preferential/Preferential_Canvas12";
		GameObject activityWindow = AppControl.OpenWindow (path);
		activityWindow.SetActive (true);
		//ActivityNew (false);
		isShow = false;
		string nolongerStr = "{\"time\":{" + "\"isShow\":" + (isShow ? 1 : 0) + ","
		                     + "\"newtime\":" + NowTime + "}}";
		FileUtilManager.CreateFile (LoginInfo.pathUrl, "RedStatue.txt", nolongerStr);
	}

	public void ToActivity()
	{
		//string path = "MainHall/Activity/ActivityCanvas";
		string path = "MainHall/ActivityCanvas";
		GameObject activityWindow = AppControl.OpenWindow(path);
		activityWindow.SetActive(true);
		//ActivityNew (false);
		isShow = false;
		string nolongerStr = "{\"time\":{" + "\"isShow\":" + (isShow ? 1 : 0) + ","
							 + "\"newtime\":" + NowTime + "}}";
		FileUtilManager.CreateFile(LoginInfo.pathUrl, "RedStatue.txt", nolongerStr);
	}

	void InitLoadActivity (string path, string filename)
	{
		//Debug.LogError ("-------------------------------------fjakshfsfhhsjfsjkhfskjfhskksjkhsfshfk");
		string data = FileUtilManager.LoadFile (path, filename);
		//Debug.LogError ("------------------------------------- data = " + data);
		if (data == null || data == "") {
			isShow = true;
			//ActivityNew (isShow);
			return;
		}
		LitJson.JsonData jd = LitJson.JsonMapper.ToObject (data);
		int show = (int)jd ["time"] ["isShow"];
		int timeInt = (int)jd ["time"] ["newtime"];
		isShow = show == 0 ? false : true;
		//Debug.LogError ("InitLoadActivity isShow ========= " + isShow);
		timeStr = timeInt.ToString ();
		//Debug.LogError ("InitLoadActivity timeStr========== " + timeStr);
	}

	public delegate void ResetScrollDelegate ();

	public static event ResetScrollDelegate ResetScrollEvent;

	public void ToTask ()
	{
		//获取任务的回复之后，获取当前改变值任务的index，根据当前index-1获取当前的slider根据内容改变其中的值
		Facade.GetFacade ().message.task.SendTaskProcessRequest ();
		AudioManager._instance.PlayEffectClip (AudioManager.ui_click);
		//string path = "Window/TaskWindow";
		string path = "MainHall/Task/TaskWindowNew";
		GameObject WindowClone = AppControl.OpenWindow (path);
		WindowClone.SetActive (true);

		if (ResetScrollEvent != null) {
			ResetScrollEvent ();
		}
	}

	public void ToAction ()
	{
		AudioManager._instance.PlayEffectClip (AudioManager.ui_click);
		string path = "Window/ActionWindow";
		GameObject WindowClone = AppControl.OpenWindow (path);
		WindowClone.SetActive (true);
	}

	public void OnSetting ()
	{
		AudioManager._instance.PlayEffectClip (AudioManager.ui_click);
		string path = "MainHall/Setting/SettingWindowNew";
		GameObject WindowClone = AppControl.OpenWindow (path);
		WindowClone.SetActive (true);

	}

	public void OnServer ()
	{
//		AudioManager._instance.PlayEffectClip (AudioManager.ui_click);
//		string path = "MainHall/Setting/ServerWindowNew";
//		GameObject WindowClone = AppControl.OpenWindow (path);
//		WindowClone.SetActive (true);
		SetShowWelFare ();
			

	}

	void SetShowWelFare ()
	{
		float scale = 0.4f;
		//if (Facade.GetFacade ().config.isIphoneX2 ()) {
		//	if (UIClassic.Instance.IsNewPlay) {
		//              //WelFare.gameObject.GetComponent<RectTransform> ().anchoredPosition3D = new Vector3 (1932f, 380f, 1080f);
		//              //welfarebg.gameObject.GetComponent<RectTransform> ().anchoredPosition3D = new Vector3 (1932f, 380f, 1080f);
		//              scale = 0.4f;
		//	} else {
		//		//WelFare.gameObject.GetComponent<RectTransform> ().anchoredPosition3D = new Vector3 (930f, 380f, 1080f);
		//		//welfarebg.gameObject.GetComponent<RectTransform> ().anchoredPosition3D = new Vector3 (930f, 380f, 1080f);
		//              //scale = 1f;
		//	}

		//} else {
		//	if (UIClassic.Instance.IsNewPlay) {
		//		//WelFare.gameObject.GetComponent<RectTransform> ().anchoredPosition3D = new Vector3 (1595f, 380f, 1080f);
		//		//welfarebg.gameObject.GetComponent<RectTransform> ().anchoredPosition3D = new Vector3 (1595f, 380f, 1080f);
		//	} else {
		//		//WelFare.gameObject.GetComponent<RectTransform> ().anchoredPosition3D = new Vector3 (781f, 390f, 1080f);
		//		//welfarebg.gameObject.GetComponent<RectTransform> ().anchoredPosition3D = new Vector3 (781f, 390f, 1080f);
		//	}

		//}

		if (WelFare.transform.childCount <= 2) {
			WelFare.gameObject.GetComponent<RectTransform> ().sizeDelta = new Vector2 (300f, 108f);
			welfarebg.gameObject.GetComponent<RectTransform> ().sizeDelta = new Vector2 (300f, 108f);
		} else {
			welfarebg.SetNativeSize ();
			WelFare.gameObject.GetComponent<Image> ().SetNativeSize ();
		}
		//boss场入场签到适配
		//if (UIClassic.Instance.IsNewPlay)
		//{
		//    scale = 0.4f;
		//}
		//else
		//{
		//    scale = 1f;
		//}

		if (!IsSHowWelFare) {
			IsSHowWelFare = true;
			welfarebg.gameObject.SetActive (IsSHowWelFare);
			WelFare.transform.DOScale (scale, 0.01f);

		} else {
			IsSHowWelFare = false;
			welfarebg.gameObject.SetActive (IsSHowWelFare);
			WelFare.transform.DOScale (0, 0.01f);
		}
	}

	public void TomorrowButton ()
	{
		//GameObject Window = Resources.Load ("Window/TomorrowGift")as GameObject;
		//WindowClone =Instantiate(Window);
		//OpenNextDayGiftTip ( 3 );
	}


	public void ToClassic ()
	{
		if (myInfo.lockScene) {
			//Debug.LogError ("----------locked return--------------");
			UnityEngine.GameObject nTipWindow = UnityEngine.Resources.Load ("Window/WindowTipsThree")as UnityEngine.GameObject;
			UnityEngine.GameObject nTipWindowEntity = UnityEngine.GameObject.Instantiate (nTipWindow);
			UITipAutoNoMask ClickTips1 = nTipWindowEntity.GetComponent<UITipAutoNoMask> ();
			ClickTips1.tipText.text = "操作太过频繁！！！";
			return;
		}
		AppControl.ToView (AppView.CLASSICHALL);
	}

	public void ToPk ()
	{
		if (myInfo.lockScene) {
			//Debug.LogError ("----------locked return--------------");
			UnityEngine.GameObject nTipWindow = UnityEngine.Resources.Load ("Window/WindowTipsThree")as UnityEngine.GameObject;
			UnityEngine.GameObject nTipWindowEntity = UnityEngine.GameObject.Instantiate (nTipWindow);
			UITipAutoNoMask ClickTips1 = nTipWindowEntity.GetComponent<UITipAutoNoMask> ();
			ClickTips1.tipText.text = "操作太過頻繁！！！";
			return;
		}
		AppControl.ToView (AppView.PKHALLMAIN);
	}

	GameObject miniGameObj;

	public void OnOpenAreaystical ()
	{
		//Debug.LogError ("open tiny game start!!!");
		GameObject Window = Resources.Load ("Window/WindowTips")as GameObject;
		GameObject WindowClone = Instantiate (Window);
		UITipClickHideManager ClickTips = WindowClone.GetComponent<UITipClickHideManager> ();
		ClickTips.text.text = "暫未開放，敬請期待！";
		ClickTips.time.text = "3";
		return;
	}

	public void OnOpenTinyGame ()
	{
		if (myInfo.isGuestLogin) {
			BindWindowCtrl.Instense.WarningWindow ();
			//OnOpenAreaystical_Tow ();
		} else {
			string urlTemp;
			Debug.Log("platformType platformType platformType = "+ myInfo.platformType);
			if (UIUpdate.WebUrlDic [WebUrlEnum.SmallGame] != null || UIUpdate.WebUrlDic [WebUrlEnum.SmallGame] != "") {
				miniGameUrl = UIUpdate.WebUrlDic [WebUrlEnum.SmallGame] + "?pid=509&";
			}

			if (myInfo.platformType == 22 || myInfo.platformType == 24)
			{
				if (myInfo.isPhoneNumberLogin)
				{
					//这里是由于微信,qq没有密码,所以拿到服务器下发的token进行小游戏的登录
					urlTemp = miniGameUrl + "UserID=" + myInfo.userID + "&pwd=" + myInfo.acessToken + "&type=" + myInfo.GetTokenType() + "&uname=" + WWW.EscapeURL(myInfo.nickname);
				}
				else {
					//这里是由于微信,qq没有密码,所以拿到服务器下发的token进行小游戏的登录
					urlTemp = miniGameUrl + "UserID=" + myInfo.userID + "&pwd=" + myInfo.mLoginData.token + "&type=" + myInfo.GetTokenType() + "&uname=" + WWW.EscapeURL(myInfo.nickname);
				}
				
			}
            else
            { 
				if (myInfo.isPhoneNumberLogin)
				{
					urlTemp = miniGameUrl + "UserID=" + myInfo.userID + "&pwd=" + myInfo.acessToken + "&type=" + 5 + "&uname=" + WWW.EscapeURL(myInfo.nickname);
				}
				else {
					//用 WWW.EscapeURL 是因为拼接的时候,如果昵称含有中文,这个时候会打不开web
					urlTemp = miniGameUrl + "UserID=" + myInfo.userID + "&pwd=" + myInfo.password + "&type=" + 0 + "&uname=" + WWW.EscapeURL(myInfo.nickname);
					//urlTemp = miniGameUrl + "UserID=" + myInfo.userID + "&pwd=" + myInfo.acessToken + "&type=" + myInfo.GetTokenType () + "&uname=" + WWW.EscapeURL (myInfo.nickname);
				}
			}

			Debug.Log ("!!!!!!!:" + urlTemp);
#if UNITY_Huawei
                    //这里由于是渠道登录所以不能用 账号和微信QQ的pwd
            urlTemp = miniGameUrl + "UserID=" + myInfo.userID + "&pwd=" + myInfo.acessToken + "&type=" + myInfo.GetTokenType() + "&uname=" + WWW.EscapeURL(myInfo.nickname);
#elif UNITY_OPPO
                    //这里由于是渠道登录所以不能用 账号和微信QQ的pwd
            urlTemp = miniGameUrl + "UserID=" + myInfo.userID + "&pwd=" + myInfo.acessToken + "&type=" + myInfo.GetTokenType() + "&uname=" + WWW.EscapeURL(myInfo.nickname);
#elif UNITY_VIVO
                    //这里由于是渠道登录所以不能用 账号和微信QQ的pwd
            urlTemp = miniGameUrl + "UserID=" + myInfo.userID + "&pwd=" + myInfo.acessToken + "&type=" + myInfo.GetTokenType() + "&uname=" + WWW.EscapeURL(myInfo.nickname);
#endif
			string path = "Window/OpenUrlCanvas";
			miniGameObj = AppControl.OpenWindow (path);
			//urlTemp = "http://gamelobby.cxtqgypc.cn/LobbyLogin.aspx?pid=509&UserID=20338704&pwd=e10adc3949ba59abbe56e057f20f883e&type=0&uname=test_05274";
			Debug.Log ("urlTemp = " + urlTemp);
			miniGameObj.SetActive (true);
			//先创建控件,在进行浏览
			if (OpenWebScript.Instance != null) {
				//打开webview控件
				OpenWebScript.Instance.SetActivityWebUrl (urlTemp);
			}
			//AppControl.miniGameState = true;
			//产品需求说需要关闭音效
			AudioManager._instance.StopBgm ();

			//注销登录
			LogOff();
			//渠道宏定
#if UNITY_Huawei
                    LogOffTow(); //因为渠道不能使用第三方登录平台所以这里重新断开连接
#elif UNITY_OPPO
                    LogOffTow(); //因为渠道不能使用第三方登录平台所以这里重新断开连接
#elif UNITY_VIVO
                    LogOffTow(); //因为渠道不能使用第三方登录平台所以这里重新断开连接
#endif
		}
	}

	/// <summary>
	/// Android里面OnIntent调用,当点击web返回的时候,销毁
	/// ios里面 
	/// -(BOOL)application:(UIApplication *)app openURL:(NSURL *)url options:(NSDictionary<NSString *,id> *)options 
	/// 中调用
	/// </summary>
	public void DestoryWebObj (GameObject webObj)
	{
		Debug.Log("  退出小遊戲 活動  ");
		if (webObj != null) {
			if (webObj.activeSelf) {
				Destroy (webObj);
			}
		}
		//自动链接
		AutoConnect ();
		//获取本地背景音乐播放数据,这里是因为需求上说去到其他游戏,自己的声音需要关闭,读取本地数据,如果设置中,音乐开了,那就让他播放,否则不播放
		if (PlayerPrefs.HasKey ("backmusic")) {
			if (PlayerPrefs.GetInt ("backmusic") == 0) {
			} else {
				AudioManager._instance.useBgm = true;
				AudioManager._instance.PlayBgm (AudioManager.bgm_hall);
			}
		}
		LoginUtil.ShowWaitingMask (false);
	}

	public void ShowStartGiftBoxTip ()
	{
		//只有qq登陆和微信登陆用户才有明日礼包倒计时
		if (myInfo.platformType == 22 || myInfo.platformType == 24) {
			if (mCountDownBoxTip == null && AppControl.GetInstance ().getActiveView () == AppView.HALL) {
				mCountDownBoxTip = StartGiftManager.OpenCountDownBoxTip (WelFare.transform, true);
			}
		}
	}

	public void Frist ()
	{
		AudioManager._instance.PlayEffectClip (AudioManager.ui_click);
		string path = "Window/Preferential/Preferential_Canvas12";
		GameObject WindowClone = AppControl.OpenWindow (path);
		WindowClone.SetActive (true);
		UIFirstRecharge.SetState = myInfo.loginInfo.preferencePackBought;
	}

	public void  MouthCard ()
	{
		//关闭签到界面弹出月卡领取界面，如果有用户在弹出月卡弹窗直接强退，再次进入的时候点击大厅的月卡还是可以再次领取
		//打开月卡确认是否有购买月卡记录以及购买后是否当天有领取
		//如果是购买则显示领取，如果当天已经领取过显示获取的按键
		//所以获取按键必须向服务器发送今天已经领取的消息
		AudioManager._instance.PlayEffectClip (AudioManager.ui_click);
		string path = "MainHall/MothlyCard/DragonCardWindow";
//		string path = "DragonCardWindow";
		GameObject WindowClone = AppControl.OpenWindow (path);
		WindowClone.SetActive (true);

		UIMouthCard nMonthData = WindowClone.GetComponent<UIMouthCard> ();
		//如果已经领取了，那么显示获取按钮，可以继续购买月卡礼包，增加持续时间
		//nMonthData.SetRemianDays ( (int)myInfo.loginInfo.monthlyCardDurationDays , myInfo.loginInfo.monthlyPackGot );
	}

	public void ToVip ()
	{
		AudioManager._instance.PlayEffectClip (AudioManager.ui_click);
		GameObject Window = Resources.Load ("Window/VIP")as GameObject;
		Instantiate (Window);
		UIVIP.isopen = true;
		HideSelf ();
	}

	public void GiftScence ()
	{
		//Debug.Log ("myInfo.cannonMultipleNow" + myInfo.cannonMultipleNow);
		//判断炮倍数，满足300倍炮则进入渔场，弹出提示窗口,打开窗口要求传炮倍数CannonWindowTips
		GameObject Window1 = UnityEngine.Resources.Load ("Window/WindowTipsThree")as UnityEngine.GameObject;
		GameObject WindowClone1 = UnityEngine.GameObject.Instantiate (Window1);
		UITipAutoNoMask ClickTips1 = WindowClone1.GetComponent<UITipAutoNoMask> ();
		ClickTips1.tipText.text = "此功能暫未開放";
		return;
		if (myInfo.cannonMultipleMax >= 300) {

			//向服务器发送请求进入红包场
			//UIHallMsg.GetInstance().SndEnterRedPacketRoomRequest(1);
		} else {
			GameObject Window = Resources.Load ("Window/WindowTips")as GameObject;
			GameObject WindowClone = Instantiate (Window);
			UITipClickHideManager ClickTips = WindowClone.GetComponent<UITipClickHideManager> ();
			ClickTips.text.text = "解鎖300倍倍炮開啟紅包場，獲得紅包券，兌換紅包!";
		}
	}

	public void StartGame ()
	{
		if (myInfo.lockScene) {
			//Debug.LogError ("----------locked return--------------");
			UnityEngine.GameObject nTipWindow = UnityEngine.Resources.Load ("Window/WindowTipsThree")as UnityEngine.GameObject;
			UnityEngine.GameObject nTipWindowEntity = UnityEngine.GameObject.Instantiate (nTipWindow);
			UITipAutoNoMask ClickTips1 = nTipWindowEntity.GetComponent<UITipAutoNoMask> ();
			ClickTips1.tipText.text = "操作太過頻繁！！！";
			return;
		}
		AudioManager._instance.PlayEffectClip (AudioManager.ui_click);
		UIHallObjects.GetInstance ().StartGame ();
//		newHandEffect.SetActive (false);
	}

	void OnRcvButton (MsgBoxType.Result result)
	{
		if (result == MsgBoxType.Result.Confirm) {
			print ("OnRcvButton Confirm");
		}

		if (result == MsgBoxType.Result.Cancel) {
			print ("OnRcvButton Cancel");
		}
	}

	/// <summary>
	/// 注销登录
	/// </summary>
	public void LogOff ()
	{
		LoginUtil.GetIntance ().CancelAuthorize ();
		myInfo.SetignStatue (0);
//		myInfo.account = null;
		LoginUtil.GetIntance ().Ishare = false;
		LoginUtil.GetIntance ().IsNoteQqorWet = true;
	}

	/// <summary>
	/// 因为渠道不能使用第三方登录平台所以这里重新断开连接
	/// </summary>
	public void LogOffTow ()
	{
		DataControl.GetInstance ().ShutDown ();
		myInfo.SetignStatue (0);
		LoginUtil.GetIntance ().Ishare = false;
		LoginUtil.GetIntance ().IsNoteQqorWet = true;
	}

	/// <summary>
	/// 自动连接
	/// </summary>
	void AutoConnect ()
	{
		//获取本地数据,进行微信自动授权登录
		ShowWaitingView(true);
		
		if (LoginInfo.isWechatLogin || myInfo.platformType == 22) {
			//ToWeChatLogin();
			LoginUtil.GetIntance().DoConnectAndSendLoginForRelogin();
			Facade.GetFacade().message.login.SendLoginRequest();
		}
		//获取本地数据,进行QQ自动授权登录
		else if (LoginInfo.isQQLogin || myInfo.platformType == 24) {
			//OnQQLogin();
			LoginUtil.GetIntance().DoConnectAndSendLoginForRelogin();
			Facade.GetFacade().message.login.SendLoginRequest();
		}
		//获取本地数据,进行账号授权登录
		else if (myInfo.platformType == 0 && myInfo.isPhoneNumberLogin) {
			//OnTokenLogin();
			LoginUtil.GetIntance().DoConnectAndSendLoginForRelogin();
			Facade.GetFacade().message.login.SendLoginRequest();
		}
		else if (LoginInfo.isAccoutLogin) {
			AutoSetAccountState(LoginInfo.accountStr, LoginInfo.passwordStr);
		} else {
			AutoSetAccountState(myInfo.account, myInfo.password);
		}

#if UNITY_OPPO
        // OPPO自动连接
        OnOPPOLogin();
#endif

#if UNITY_VIVO
         // VIVO自动连接
        OnVIVOLogin();
#endif

#if UNITY_Huawei
		// 华为自动连接
		OnHuaweiLogin ();
#endif
	}

	/// <summary>
	/// 微信登录
	/// </summary>
	void ToWeChatLogin ()
	{
		Debug.Log(" ToWeChatLogin ");
		SendWechatData ();
	}

	/// <summary>
	/// QQ登录
	/// </summary>
	void OnQQLogin ()
	{
		Debug.Log(" OnQQLogin ");
		SendQQData ();
	}

	void OnTokenLogin() {
		Debug.Log(" OnTokenLogin ");
		SendTokenData();
	}

	/// <summary>
	/// oppo登录
	/// </summary>
	void OnOPPOLogin ()
	{
		SendOPPOData ();
	}

	/// <summary>
	/// vivo登录
	/// </summary>
	void OnVIVOLogin ()
	{
		SendVIVOData ();
	}

	/// <summary>
	/// 华为登录
	/// </summary>
	void OnHuaweiLogin ()
	{
		SendHuaweiData ();
	}

	/// <summary>
	/// 设置账号自动登录的状态
	/// </summary>
	/// <param name="account">Account.</param>
	/// <param name="pwd">Pwd.</param>
	void AutoSetAccountState (string account, string pwd)
	{
		//Debug.LogError ("account= " + account);
		//Debug.LogError ("pwd= " + pwd);
		Debug.LogError(" 222  222 AutoSetAccountState");
		if (string.IsNullOrEmpty (account) || LoginUtil.GetIntance () == null) {
			return;
		}
		MyInfo myInfo = DataControl.GetInstance ().GetMyInfo ();
		myInfo.isGuestLogin = false;
		myInfo.platformType = 0;
		LoginUtil.GetIntance ().AutoLoginWitnAccountForWeb (account, pwd);
		LoginInfo.isAccoutLogin = true;
	}

	IEnumerator GetLoginResponse (string url, WWWForm data)
	{
		Debug.Log(" GetLoginResponse "+url);
		WWW www = new WWW (url, data);
		yield return www;
		if (string.IsNullOrEmpty (www.error)) {
			////Debug.Log ("IEnumerator 11123");
			Debug.LogError ("[ -----------GetLoginResponse22222------------ ]" + www.text);
			LoginUnit nUnit = new LoginUnit ();
			nUnit.DoConvert (www.text);
			myInfo.mLoginData = nUnit;
			//userInfo.openId = "thirdpart";
			if (nUnit.result != 0) {
				if (myInfo.isGuestLogin) {
					myInfo.nickname = nUnit.username;
				}
				DoConnectAndSendLogin ();
			} else {
				GameObject Window1 = UnityEngine.Resources.Load ("Window/WindowTipsThree")as UnityEngine.GameObject;
				GameObject WindowClone1 = UnityEngine.GameObject.Instantiate (Window1);
				UITipAutoNoMask ClickTips1 = WindowClone1.GetComponent<UITipAutoNoMask> ();
				ClickTips1.tipText.text = "錯誤的登錄數據";
			}
		} else {
			Debug.LogError ("[ -----------GetLoginResponse error------------ ]" + www.error);
		}
		
	}

	void SendWechatData ()
	{
		Debug.Log(" SendWechatData ");
		//StartCoroutine (GetLoginResponse (CLLoginUrl, GetWechatPostData (myInfo.openId, myInfo.acessToken, myInfo.sex)));
		Facade.GetFacade().message.login.SendLoginRequest();
	}

	void SendQQData ()
	{
		//userInfo.openId = nQQId;
		//userInfo.acessToken = nQQToken;
		// //Debug.LogError("---SendQQData url:"+CLLoginUrl);
		// //Debug.LogError("---SendQQData openId:"+userInfo.openId);
		// //Debug.LogError("---SendQQData token:"+userInfo.acessToken);
		// //Debug.LogError("---SendQQData sex:"+userInfo.sex);
		Debug.Log(" SendQQData ");
		StartCoroutine (GetLoginResponse (CLLoginUrl, GetQQPostData (myInfo.openId, myInfo.acessToken, myInfo.sex)));
	}

	void SendTokenData()
	{
		Debug.Log(" SendTokenData ");
		StartCoroutine(GetLoginResponse(CLLoginUrl, GetWechatPostData(myInfo.openId, myInfo.acessToken, myInfo.sex)));
	}

	WWWForm GetQQPostData (string nOpenId, string nToken, int sex)
	{
		string nMacId = SystemInfo.deviceUniqueIdentifier;
#if UNITY_IPHONE && !UNITY_EDITOR
		nMacId = UILogin.GetLoginUUID();
#endif
		WWWForm nData = new WWWForm ();
		nData.AddField ("openid", nOpenId);

		string nOauth = "25";
		// string nOauth = "27";
#if UNITY_IPHONE
		nOauth = "24";
		// nOauth = "26";
#endif
		nData.AddField ("oauth", nOauth);
		nData.AddField ("token", nToken);
		nData.AddField ("nick", myInfo.nickname);
		nData.AddField ("sex", sex);
		nData.AddField ("mid", LoginMsgHandle.getChannelNumber ().ToString ());
		nData.AddField ("sid", "0");

		nData.AddField ("mac", nMacId);
		string nSignResult = GetMd5SignData (nOauth, nOpenId, nToken, sex.ToString (), nMacId);//"1" + nOpenId + "" + nToken + "1"+ "00" + nMacId + "" + MobileRegisterKey;
		nData.AddField ("md5", nSignResult);
		nData.AddField ("type", 8);

		Debug.Log("GetQQPostData nOpenId = " + nOpenId);
		Debug.Log("GetQQPostData nToken = " + nToken);
		Debug.Log("GetQQPostData nickname = " + myInfo.nickname);
		Debug.Log("GetQQPostData mid = " + LoginMsgHandle.getChannelNumber().ToString());
		Debug.Log("GetQQPostData nMacId = " + nMacId);
		Debug.Log("GetQQPostData nSignResult = " + nSignResult);
		//		Tool.FileWrite ( Application.persistentDataPath + "/test.txt" , "[-- "+nOauth + " \r\n--/-- " + nToken + " \r\n--/-- " + nOpenId + " \r\n--/-- "  +  nMacId + "--]" ); 
		return nData;
	}

	WWWForm GetWechatPostData (string nOpenId, string nToken, int sex)
	{
		WWWForm nData = new WWWForm ();
		nData.AddField ("openid", nOpenId);
		nData.AddField ("oauth", 3);
		nData.AddField ("token", nToken);
		nData.AddField ("nick", myInfo.nickname);
		nData.AddField ("sex", sex);
		nData.AddField ("mid", LoginMsgHandle.getChannelNumber ().ToString ());
		nData.AddField ("sid", "0");

		string nMacId = SystemInfo.deviceUniqueIdentifier;
#if UNITY_IPHONE && !UNITY_EDITOR
		nMacId = UILogin.GetLoginUUID();
#endif
		nData.AddField ("mac", nMacId);

		string nSignResult = GetMd5SignData ("3", nOpenId, nToken, sex.ToString (), nMacId); //"3" + nOpenId + "" + nToken + "1"+ "00" + nMacId + "" + MobileRegisterKey;
		nData.AddField ("md5", nSignResult);
		nData.AddField ("type", 8);

		Debug.Log("GetWechatPostData nOpenId = " + nOpenId);
		Debug.Log("GetWechatPostData nToken = " + nToken);
		Debug.Log("GetWechatPostData nickname = " + myInfo.nickname);
		Debug.Log("GetWechatPostData mid = " + LoginMsgHandle.getChannelNumber().ToString());
		Debug.Log("GetWechatPostData nMacId = " + nMacId);
		Debug.Log("GetWechatPostData nSignResult = " + nSignResult);
		return nData;
	}

	/// <summary>
	/// 发送oppo断线重连
	/// </summary>
	void SendOPPOData ()
	{
		StartCoroutine (GetOppoLoginResponse (URL, GetOppoPosData (myInfo.openId, myInfo.oppoAcessToken)));
	}

	/// <summary>
	/// 获取oppoPOS数据
	/// </summary>
	private WWWForm GetOppoPosData (string nOpenId, string nToken)
	{
		WWWForm nData = new WWWForm ();
		nData.AddField ("type", 8);
		nData.AddField ("oauth", 33);
		nData.AddField ("openid", nOpenId);
		nData.AddField ("token", nToken);
		nData.AddField ("sex", "1");
		nData.AddField ("mid", LoginMsgHandle.getChannelNumber ().ToString ());//渠道号
		nData.AddField ("sid", "0");
		nData.AddField ("mac", SystemInfo.deviceUniqueIdentifier);//设备唯一标识符
		string getMD5 = LoginUtil.GetMD5 (33 + myInfo.openId + myInfo.oppoAcessToken + "1" + LoginMsgHandle.getChannelNumber ().ToString () + "0" + SystemInfo.deviceUniqueIdentifier + key).ToLower ();
		nData.AddField ("md5", getMD5);
		Debug.Log ("md5:" + getMD5);
		Debug.Log ("GetOppoPosData:" + nData);
		return nData;
	}

	/// <summary>
	/// 请求oppo登录返回数据
	/// </summary>
	private IEnumerator GetOppoLoginResponse (string url, WWWForm data)
	{
		Debug.Log(" GetOppoLoginResponse DoConnectAndSendLogin");
		WWW www = new WWW (url, data);
		yield return www;
		if (string.IsNullOrEmpty (www.error)) {
			LoginUnit nUnit = new LoginUnit ();
			nUnit.DoConvert (www.text);
			myInfo.mLoginData = nUnit;
			if (nUnit.result != 0) {
				string data_1 = www.text;
				string[] data_2 = data_1.Split ('|');
				myInfo.acessToken = data_2 [3];
				if (myInfo.isGuestLogin) {
					myInfo.nickname = nUnit.username;
					myInfo.acessToken = nUnit.token;
					myInfo.password = nUnit.passwd;
				}
				DoConnectAndSendLogin ();
			} else {
				Debug.Log ("GetOppoLoginResponse4");
			}
		}
	}

	/// <summary>
	/// 发送VIVO断线重连
	/// </summary>
	void SendVIVOData ()
	{
		StartCoroutine (GetVivoLoginnResponse (URL, GetVivoPosData (myInfo.openId, myInfo.vivoAcessToken)));
	}

	/// <summary>
	/// 获取vivoPOS数据
	/// </summary>
	private WWWForm GetVivoPosData (string nOpenId, string nToken)
	{
		WWWForm nData = new WWWForm ();
		nData.AddField ("type", 8);
		nData.AddField ("oauth", 36);
		nData.AddField ("openid", nOpenId);
		nData.AddField ("token", nToken);
		nData.AddField ("sex", "1");
		nData.AddField ("mid", LoginMsgHandle.getChannelNumber ().ToString ());//渠道号
		nData.AddField ("sid", "0");
		nData.AddField ("mac", SystemInfo.deviceUniqueIdentifier);//设备唯一标识符
		string getMD5 = LoginUtil.GetMD5 (36 + myInfo.openId + myInfo.vivoAcessToken + "1" + LoginMsgHandle.getChannelNumber ().ToString () + "0" + SystemInfo.deviceUniqueIdentifier + key).ToLower ();
		nData.AddField ("md5", getMD5);
		Debug.Log ("md5:" + getMD5);
		Debug.Log ("GetOppoPosData:" + nData);
		return nData;
	}

	/// <summary>
	/// 请求vivo登录返回数据
	/// </summary>
	private IEnumerator GetVivoLoginnResponse (string url, WWWForm data)
	{
		WWW www = new WWW (url, data);
		yield return www;
		if (string.IsNullOrEmpty (www.error)) {
			Debug.Log ("[ --GetVivoLoginnResponse- ]" + www.text);
			LoginUnit nUnit = new LoginUnit ();
			nUnit.DoConvert (www.text);
			myInfo.mLoginData = nUnit;
			if (nUnit.result != 0) {
				string data_1 = www.text;
				string[] data_2 = data_1.Split ('|');
				myInfo.acessToken = data_2 [3];
				if (myInfo.isGuestLogin) {
					myInfo.nickname = nUnit.username;
				}
				DoConnectAndSendLogin ();
			} else {
				Debug.Log ("錯誤的登錄數據");
			}
		}
	}

	/// <summary>
	/// 发送huawei断线重连
	/// </summary>
	void SendHuaweiData ()
	{
		StartCoroutine (GetHuaweiLoginnResponse (URL, GetHuaweiPosData (myInfo.openId, myInfo.huaweiAcessToken)));
	}

	/// <summary>
	/// 获取huaweiPOS数据
	/// </summary>
	private WWWForm GetHuaweiPosData (string nOpenId, string nToken)
	{
		WWWForm nData = new WWWForm ();
		nData.AddField ("type", 8);
		nData.AddField ("oauth", 37);
		nData.AddField ("openid", nOpenId);
		nData.AddField ("token", nToken);
		nData.AddField ("sex", "1");
		nData.AddField ("mid", LoginMsgHandle.getChannelNumber ().ToString ());//渠道号
		nData.AddField ("sid", "0");
		nData.AddField ("mac", SystemInfo.deviceUniqueIdentifier);//设备唯一标识符
		string getMD5 = LoginUtil.GetMD5 (37 + myInfo.openId + myInfo.huaweiAcessToken + "1" + LoginMsgHandle.getChannelNumber ().ToString () + "0" + SystemInfo.deviceUniqueIdentifier + key).ToLower ();
		nData.AddField ("md5", getMD5);
		Debug.Log ("md5:" + getMD5);
		Debug.Log ("GetOppoPosData:" + nData);
		return nData;
	}

	/// <summary>
	/// 请求huawei登录返回数据
	/// </summary>
	private IEnumerator GetHuaweiLoginnResponse (string url, WWWForm data)
	{
		WWW www = new WWW (url, data);
		yield return www;
		Debug.Log ("[ --GetHuaweiLoginnResponse- ]" + www.text);
		if (string.IsNullOrEmpty (www.error)) {
			Debug.Log ("[ --GetVivoLoginnResponse- ]" + www.text);
			LoginUnit nUnit = new LoginUnit ();
			nUnit.DoConvert (www.text);
			myInfo.mLoginData = nUnit;
			if (nUnit.result != 0) {
				string data_1 = www.text;
				string[] data_2 = data_1.Split ('|');
				myInfo.acessToken = data_2 [3];
				if (myInfo.isGuestLogin) {
					myInfo.nickname = nUnit.username;
				}
				DoConnectAndSendLogin ();
			} else {
				Debug.Log ("錯誤的登錄數據");
			}
		}
	}

	public void ShowWaitingView(bool nValue)
	{
		if (nValue)
		{
			GameObject Window1 = UnityEngine.Resources.Load("MainHall/Common/WaitingView") as UnityEngine.GameObject;
			mWaitingView = UnityEngine.GameObject.Instantiate(Window1);
			UIWaiting nData = mWaitingView.GetComponentInChildren<UIWaiting>();
			nData.HideBackGround();
			nData.SetInfo(10.0f, "連線完成");
		}
		else
		{
			Destroy(mWaitingView);
			mWaitingView = null;
		}
	}

	string GetMd5SignData (string oauth, string openid, string token, string sex, string mac)
	{
		return LoginUtil.GetMD5 (oauth + openid + token + sex + "" + LoginMsgHandle.getChannelNumber ().ToString () + "" + "0" + mac + MobileRegisterKey).ToLower ();
	}

	void DoConnectAndSendLogin ()
	{
		Debug.Log(" DoConnectAndSendLogin ");
		if (string.IsNullOrEmpty (myInfo.openId)) {
			GameObject Window = UnityEngine.Resources.Load ("Window/WindowTipsThree")as UnityEngine.GameObject;
			GameObject WindowClone = UnityEngine.GameObject.Instantiate (Window);
			UITipAutoNoMask ClickTips1 = WindowClone.GetComponent<UITipAutoNoMask> ();
			ClickTips1.tipText.text = "無法獲取用戶信息，請重試";
			LoginUtil.GetIntance ().CancelAuthorize ();
			return;
		}

		myInfo.isLogining = true;
		//JumpLoadingView ();
		DataControl.GetInstance ().ConnectSvr (myInfo.ServerUrl, AppInfo.portNumber);
		StartCoroutine (ResetLoginState ());
		//Debug.Log ("=======DoConnectAndSendLogin=====");
	}

	IEnumerator ResetLoginState ()
	{
		yield return new WaitForSeconds (1f);
		myInfo.isLogining = false;
	}

	void JudegeBossMatchState ()
	{
//		Debug.LogError ("myInfo.myBossMatchState = " + myInfo.myBossMatchState);
		if (!myInfo.isGuestLogin && myInfo.loginInfo.reuslt == 0) {
			if ((int)myInfo.gold < 5000000 && !myInfo.misHaveDraCard) {
				return;
			} 
			//&& myInfo.myBossMatchState < 5
			if (AppInfo.isFritsInGame && myInfo.myBossMatchState > 0) {
				string path = string.Empty;
				path = "Game/BossMatchCanvas";
				AppControl.OpenWindow (path);
				if (myInfo.myBossMatchState > 0 && myInfo.myBossMatchState < 5) {
					//这个时候弹框 正常离开返回
					BossMatchScript.Instance.SetTextDescription (false, "立即返回", "暫不參加");
					AppInfo.isFritsInGame = false;
				} else if (myInfo.myBossMatchState == 5) {
					//这个时候还没进游戏
					BossMatchScript.Instance.SetTextDescription (false, "立即返回", "暫不參加");
					AppInfo.isFritsInGame = false;
				} else {
//					if (AppInfo.isFritsInGame) {
//						AppInfo.isFritsInGame = false;
//						ToActivity ();
//					}
				}
			}
		}
	}

	public void OnOpenTreasureClick ()
	{
		if (AppInfo.trenchNum > 51000000) {
			AudioManager._instance.PlayEffectClip(AudioManager.ui_click);
			string path = "Window/Preferential/Treasure";
			GameObject WindowClone = AppControl.OpenWindow(path);
			WindowClone.SetActive(true);
		}
		
	}

	/// <summary>
	/// 打开双喜临门图标
	/// </summary>
	public void OnOpendoubleClick ()
	{
		if (AppInfo.trenchNum > 51000000) {
			AudioManager._instance.PlayEffectClip(AudioManager.ui_click);
			string path = "Window/Preferential/Double";
			GameObject WindowClone = AppControl.OpenWindow(path);
			WindowClone.SetActive(true);
		}
	}

	/// <summary>
	/// 根据请求的关闭结果来控制页面按钮的打开
	/// </summary>
	void UiAndButtonCloseOrOpen ()
	{
		FangChenMi._instance.StartTimer();
		GetButtonState buttonState = (GetButtonState)Facade.GetFacade ().data.Get (FacadeConfig.UI_AND_BUTTON_CLOSE_OR_OPEN);
		if (RankButton != null && buttonState.nButtonStateArray.Count > 1 && buttonState.nButtonStateArray [1] == 1) {//当值为1的时候,排行榜按钮打开
			RankButton.SetActive (true);
		}
		if (buttonState.nButtonStateArray.Count > 0 && buttonState.nButtonStateArray [0] != 1) {//当值为1的时候,不运行防沉迷携程函数
			FangChenMi._instance.StartTimer ();
		}
	}


	void OnDestroy ()
	{
		StopCoroutine ("UpdateClientInfo");
		Facade.GetFacade ().ui.Remove (FacadeConfig.UI_HALL_MODULE_ID);
		UIStore.HideEvent -= HideSelf;
		UIVIP.SeeEvent -= SeeSelf;
		UIUpgrade.HideEvent -= HideSelf;
		//UISetSailGift.SeeHandEvent -= NewHandEffect;
	}

}

