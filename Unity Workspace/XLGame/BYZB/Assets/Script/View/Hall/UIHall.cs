using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;
using AssemblyCSharp;

public class UIHall : MonoBehaviour , IUiMediator {

	//private AppControl appControl = null;
	private DataControl dataControl = null;
	private BackpackInfo backpackInfo = null;
	//private RankInfo rankInfo = null;

	private MyInfo myInfo = null;


	public static string nickNamestr;
	//public static bool tomrrow = true;

	public GameObject hallWindow;
	//存储背包数据的字典，为了根据物品的名字实例化这张图片
	//public static Dictionary <string ,FiBackpackProperty> arrayDic = new Dictionary<string, FiBackpackProperty> ();
	public Image bagJiaobiao;
	public Image friendJiaobiao;
	public Image taskJiaobiao;
	public Image mailJiaobiao;
	public Image bankJiaobiao;
	public GameObject newHandEffect;
	//public GameObject redSceneEffect;
	//public GameObject redGiftLock;
	public GameObject container;

	//起航礼包倒计时提示宝箱
	private GameObject mCountDownBoxTip;

	private InfoUpdateHelper mInfoUpdater = new InfoUpdateHelper ();

	//商城图标
	public GameObject mailButton;
	//特惠图标
	public GameObject preferenceButton;

	public void OnRecvData( int nType , object nData )
	{
		
	}

	public void OnInit()
	{
		
	}

	public void OnRelease()
	{
		
	}

	void Awake()
	{
		//UISetSailGift.TomrrowEvent += TomorrowGift;
		//背包数据清空
		//判断当前有没有好友申请，以及有没有邮件
//		appControl = AppControl.GetInstance ();

		dataControl = DataControl.GetInstance ();
		if(null!=dataControl)
		{
			myInfo = dataControl.GetMyInfo ();
//			roomInfo = dataControl.GetRoomInfo ();
		}

		UIHallMsg.GetInstance ();

//		msgBox = new MsgBox ();

		backpackInfo = dataControl.GetBackpackInfo ();
		//rankInfo = dataControl.GetRankInfo ();
		//nickName.text = myInfo.nickname;
		nickNamestr = myInfo.nickname;

		/*if(myInfo.cannonMultipleMax >= 300)
		{
			//redSceneEffect.SetActive (false);
			//redGiftLock.SetActive (false);
		}*/
	}

	//打开签到界面
	void OpenSignInWindow()
	{
		GameObject window = Resources.Load ("Window/SignWIndow")as GameObject;
		GameObject.Instantiate (window);
	}

	//Use this for initialization
	void Start () {
		//newHandEffect.SetActive (false);
		UIStore.HideEvent += HideSelf;
		UIVIP.SeeEvent += SeeSelf;
		UIUpgrade.HideEvent += HideSelf;
		//UISetSailGift.SeeHandEvent += NewHandEffect;


		bagJiaobiao.gameObject.SetActive (false);
		friendJiaobiao.gameObject.SetActive (false);
		taskJiaobiao.gameObject.SetActive (false);
		mailJiaobiao.gameObject.SetActive (false);
		bankJiaobiao.gameObject.SetActive (false);
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

		//签到系统,为了启航礼包没有一开始弹出签到系统，在这里判断是否有弹出该窗口
		//Debug.LogError( myInfo.sailDay +"/"+ myInfo.bDisplayedStartGift + "/" + myInfo.signInArray.Count );
		if ( (myInfo.sailDay == 0 || myInfo.bDisplayedStartGift) && myInfo.signInArray!= null && myInfo.signInArray.Count > 0 && myInfo.getSignInDay() != 0 ) {
			OpenSignInWindow ();
		}

		Facade.GetFacade ().ui.Add ( FacadeConfig.UI_HALL_MODULE_ID , this );
		//myInfo.avatar = "https://gss0.baidu.com/94o3dSag_xI4khGko9WTAnF6hhy/zhidao/pic/item/03087bf40ad162d97a2bc4711bdfa9ec8b13cd92.jpg";

		//添加头像
		if ( !string.IsNullOrEmpty( myInfo.avatar) )
		{
			AvatarInfo nAvaInfo = ( AvatarInfo )Facade.GetFacade ().data.Get ( FacadeConfig.AVARTAR_MODULE_ID );
			nAvaInfo.Load ( myInfo.userID , myInfo.avatar , OnRecvAvatarResponse );
		}

		Facade.GetFacade ().message.friend.SendGetFriendList ();
		StartCoroutine ( "UpdateClientInfo" );

		//myInfo.sailDay = 1;
		OnProcessStartGift ();

		//已经领取了起航礼包奖励
		if ( myInfo.sailDay <= 0 &&  myInfo.level <= 5) {
			ShowHandEffect ();
		}

		Debug.LogError ( "---------------------" +  myInfo.loginInfo.preferencePackBought  );
		//如果特惠礼包已经购买了，那么显示商城图标
//		if ( myInfo.loginInfo.preferencePackBought ) {
//			OnShowMailButton ();
//		}
	}

	public void OnShowMailButton()
	{
		mailButton.SetActive ( true );
		preferenceButton.SetActive ( false );
	}

	//处理起航礼包的消息
	void OnProcessStartGift()
	{
		//Debug.LogError ( "------------------" + myInfo.bDisplayedStartGift + " / " +  myInfo.sailDay );
		//如果没有显示过起航礼包,并且有起航礼包的消息，显示起航礼包
		if ( !myInfo.bDisplayedStartGift && myInfo.sailDay > 0) {
			myInfo.bDisplayedStartGift = true;
			Debug.Log("~~~~~~~~~~~~~~~~~~~~~~~~~~~~~UIHall~~~~~~~~~~~~~~~~~~~~~~~~~~~~~OnProcessStartGift ");
			StartGiftManager.OpenStartGiftWindow ();
			UIHallCore.PopSevenDaySignForFrist = true;
		}

		/*myInfo.sailDay = 1;*/
		//如果是领取起航礼包的第二天，那么显示倒计时图标,并且炮倍数已经解锁到5级了，那么显示倒计时
		//myInfo.sailDay = -2;
		//如果已经领取过起航礼包了,并且最大炮倍数解锁到5倍了，那么显示倒计时提醒宝箱

		//StartGiftManager.OpenCountDownWindow ( 2 );

		if( (myInfo.sailDay == -1 || myInfo.sailDay == -2 ) && myInfo.cannonMultipleMax >= 5 ){
			ShowStartGiftBoxTip ();
		}
	}

	//appid  wxbbbd13c64debe77a
	//appsecret 9207abf11315eae171d7a87a167e799a

	void OnRecvAvatarResponse( int nResult , Texture2D nImage )
	{
		//Debug.LogError ("-------------------/" + nResult);
		/*if (nResult == 0 && MyHead != null) {
			nImage.filterMode = FilterMode.Bilinear;
			nImage.Compress (true);

			MyHead.sprite = Sprite.Create (nImage, new Rect (0, 0, nImage.width, nImage.height), new Vector2 (0, 0));
		}*/
	}

	//每隔5s更新信息，如果有相关更新数据，那么显示角标
	IEnumerator UpdateClientInfo()
	{
		FriendInfo    nInfoFriend = (FriendInfo)  Facade.GetFacade ().data.Get ( FacadeConfig.FRIEND_MODULE_ID );
		MailDataInfo  nMailInfo   = (MailDataInfo)Facade.GetFacade ().data.Get ( FacadeConfig.MAIL_MODULE_ID );
		BackpackInfo  nBackInfo   = (BackpackInfo)Facade.GetFacade ().data.Get ( FacadeConfig.BACKPACK_MODULE_ID );
		TaskInfo      mTaskInfo   = (TaskInfo)Facade.GetFacade ().data.Get ( FacadeConfig.TASK_MODULE_ID );
		BankInfo      mBankInfo   = (BankInfo)Facade.GetFacade ().data.Get (FacadeConfig.UI_BANk_MOUDLE_ID);
		RankInfo      nRankInfo=(RankInfo)Facade.GetFacade ().data.Get (FacadeConfig.RANK_MODULE_ID);
		int  nRequestFrequent = 0;
		long rankRequstTime = 0;
		while( true )
		{
			if(  nRequestFrequent == 0 )
			    mInfoUpdater.DoUpdateInfoRequest ();
			nRequestFrequent++;
			if (nRequestFrequent >= 10) {
				rankRequstTime += 10;
				nRequestFrequent = 0;
			}
			if (rankRequstTime >= 600) {
				Facade.GetFacade().message.rank.SendGetRankInfoRequest( 1 );
				Facade.GetFacade().message.rank.SendGetRankInfoRequest( 0 );
				rankRequstTime = 0;
			}
		    yield return new WaitForSeconds( 5.0f );
			//背包有更新的数据，那么设置背包的角标
			BagNewTool ( nBackInfo.isUpdated () );
			//好友有更新的数据，设置好友角标
			FriendNewInfo ( nInfoFriend.isUpdated() );
			//邮件有更新的数据，设置邮件角标
			MailNew ( nMailInfo.isUpdated () );
			//任务模块角标设置
			TaskNew ( mTaskInfo.isUpdated () );
			//银行消息角标
			BankNewInfo (mBankInfo.isUpdate);

		}
	}


//	//获取头像
//	public void GetMyHead(Texture2D image)
//	{
//		image.filterMode = FilterMode.Bilinear;
//		image.Compress (true);
//		MyHead.sprite = Sprite.Create(image, new Rect (0, 0, image.width,image.height),new Vector2(0,0));
//	}
	public void BankNewInfo(bool newInfo)
	{
		if( bankJiaobiao.gameObject.activeSelf != newInfo )
			bankJiaobiao.gameObject.SetActive (newInfo);
		if (BankManager.instance != null) {
			BankManager.instance.RedPoint.SetActive (newInfo);
		}
	}

	public void BagNewTool(bool newTool)
	{
		if( bagJiaobiao.gameObject.activeSelf != newTool )
			bagJiaobiao.gameObject.SetActive (newTool);
	}

	public void FriendNewInfo(bool newInfo)
	{
		if( friendJiaobiao.gameObject.activeSelf != newInfo )
			friendJiaobiao.gameObject.SetActive (newInfo);
	}

	public void TaskNew(bool newTask)
	{
		if( taskJiaobiao.gameObject.activeSelf != newTask )
			taskJiaobiao.gameObject.SetActive (newTask);
	}

	public void MailNew(bool newMail)
	{
		if( mailJiaobiao.gameObject.activeSelf!= newMail )
	    	mailJiaobiao.gameObject.SetActive (newMail);
	}
		
	void HideSelf()
	{
		hallWindow.SetActive (false);
	}
		
	void SeeSelf()
	{
		hallWindow.SetActive (true);
	}


	public void TestAddGold()
	{
		Facade.GetFacade ().message.toolPruchase.SendTopupRequest( FiPropertyType.GOLD , 6 );
	}

	public void TestAddDiamond()
	{
		Facade.GetFacade ().message.toolPruchase.SendTopupRequest( FiPropertyType.DIAMOND , 348 );
	}

	public void ShowHandEffect()
	{
		newHandEffect.SetActive (true);
	}

//	void OnGUI()
//	{
//		Tool.ShowInGUI ();
//	}

	public void ToCash()
	{
		AudioManager._instance.PlayEffectClip (AudioManager.ui_click);
		string path = "Window/CashPrize";
		GameObject WindowClone = AppControl.OpenWindow (path);
		WindowClone.SetActive (true);
	}

	public void ToBag()
	{
		AudioManager._instance.PlayEffectClip (AudioManager.ui_click);
		//backpackInfo.SetUpdate ( false );
		string path = "MainHall/BackPack/BagMainWindow";//"Window/BagWindow";
		GameObject WindowClone = AppControl.OpenWindow (path);
		WindowClone.SetActive (true);
	}
	public void ToBank()
	{
		AudioManager._instance.PlayEffectClip (AudioManager.ui_click);
		string path;
		GameObject WindowClone;
		myInfo.isGuestLogin = false;
		//myInfo.loginInfo.hasBankPswd = true;
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
			temp.content.text = "為了賬號安全，請設置銀行密碼！";
			Image no = temp.store.GetComponent<Image> ();
			Image yes = temp.refuse.GetComponent<Image> ();
			no.sprite = UIHallTexturers.instans.Bank [10];
			no.SetNativeSize ();
			no.rectTransform.localScale = Vector3.one * 1.2f;
			yes.sprite = UIHallTexturers.instans.Bank [11];
			yes.SetNativeSize ();
			yes.rectTransform.localScale = Vector3.one * 1.2f;
			temp.store.onClick.AddListener(()=>Destroy(temp.gameObject));
			temp.refuse.onClick.AddListener (() => {
				string mpath = "MainHall/Common/BankSetPassword";
				GameObject mWindowClone = AppControl.OpenWindow (mpath);
				mWindowClone.SetActive (true);
				Destroy(temp.gameObject);
			});
			WindowClone.SetActive (true);
			return;
		}
		path = "MainHall/Bank/BankWindow";
		WindowClone = AppControl.OpenWindow (path);
		WindowClone.SetActive (true);
	}

	//public delegate void GoodFriendDelegate();
	//public static event GoodFriendDelegate GoodFriendEvent;

	public void ToNotice(){
		BroadCastManager.instans.broadCastObj.GetComponent<Button> ().interactable = false;
		AudioManager._instance.PlayEffectClip(AudioManager.ui_click);
		string path = "Window/NoticeWindow";
		GameObject WindowClone = AppControl.OpenWindow(path);
		WindowClone.SetActive(true);
	}


	public void ToGoodFriend()
	{
		AudioManager._instance.PlayEffectClip (AudioManager.ui_click);
		//发送获取当前好友数据请求
		string path = "Window/GoodFriends";
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

	public void ToRank()
	{
		AudioManager._instance.PlayEffectClip (AudioManager.ui_click);
		string path = "Window/RankingList";
		GameObject WindowClone = AppControl.OpenWindow (path);
		WindowClone.SetActive (true);
		//同上注册获取排行的数据，记录时间，30分钟要刷新一次
	}
		
	public void ToStore()
	{
		Debug.LogError ( "----------to store -------" );
		AudioManager._instance.PlayEffectClip (AudioManager.ui_click);
		//先判断是否有没有首充过，如果首充过，则先弹出首充特惠的窗口，首充过则直接弹出商城的界面
        //string path = "Window/StoreWindow";//商店预制体名称更换
        string path = "Window/NewStoreCanvas";
		GameObject WindowClone = AppControl.OpenWindow (path);
		WindowClone.SetActive (true);
	}

	//public delegate void InitMailDelegate();
	//public static event InitMailDelegate InitMailEvent;

	//打开邮件模块
	public void ToMail()
	{
		/*//剩余的月卡天数 > 0 就是月卡用户
		myInfo.loginInfo.monthlyCardDurationDays;
		//是否购买了特惠礼包，是为true
		myInfo.loginInfo.preferencePackBought;*/

		string path = "Window/MailWindow";
		GameObject nMailWindow = AppControl.OpenWindow (path);
		nMailWindow.SetActive (true);
		MailDataInfo nInfo = (MailDataInfo)Facade.GetFacade ().data.Get ( FacadeConfig.MAIL_MODULE_ID );
		if (nInfo.getMailList ().Count == 0)
			AudioManager._instance.PlayEffectClip (AudioManager.ui_click);
		

		/*Debug.Log ("初始化系统邮件");
		if (InitMailEvent != null) {
			InitMailEvent ();
		}*/
	}

	public delegate void ResetScrollDelegate();
	public static event ResetScrollDelegate ResetScrollEvent;

	public void ToTask()
	{
		//获取任务的回复之后，获取当前改变值任务的index，根据当前index-1获取当前的slider根据内容改变其中的值
		Facade.GetFacade ().message.task.SendTaskProcessRequest ();
		AudioManager._instance.PlayEffectClip (AudioManager.ui_click);
		string path = "Window/TaskWindow";
		GameObject WindowClone = AppControl.OpenWindow (path);
		WindowClone.SetActive (true);

		if (ResetScrollEvent != null) {
			ResetScrollEvent ();
		}
	}



	public void ToAction()
	{
		AudioManager._instance.PlayEffectClip (AudioManager.ui_click);
		string path = "Window/ActionWindow";
		GameObject WindowClone = AppControl.OpenWindow (path);
		WindowClone.SetActive (true);
	}

	public void OnSetting()
	{
		AudioManager._instance.PlayEffectClip (AudioManager.ui_click);
		string path = "Window/SettingWindow";
		GameObject WindowClone = AppControl.OpenWindow (path);
		WindowClone.SetActive (true);

	}

	public void OnServer()
	{
		AudioManager._instance.PlayEffectClip (AudioManager.ui_click);
		string path = "Window/ServerWindow";
		GameObject WindowClone = AppControl.OpenWindow (path);
		WindowClone.SetActive (true);
	}

	public void TomorrowButton()
	{
		//GameObject Window = Resources.Load ("Window/TomorrowGift")as GameObject;
		//WindowClone =Instantiate(Window);
		//OpenNextDayGiftTip ( 3 );
	}


	public void ToClassic()
	{
		if (myInfo.lockScene) {
			Debug.LogError ( "----------locked return--------------" )  ;
			UnityEngine.GameObject nTipWindow = UnityEngine.Resources.Load ("Window/WindowTipsThree")as UnityEngine.GameObject;
			UnityEngine.GameObject nTipWindowEntity = UnityEngine.GameObject.Instantiate ( nTipWindow );
			UITipAutoNoMask ClickTips1 = nTipWindowEntity.GetComponent<UITipAutoNoMask> ();
			ClickTips1.tipText.text = "操作太過頻繁！！！";
			return;
		}
		AppControl.ToView (AppView.CLASSICHALL);
	}

	public void ToPk()
	{
		if (myInfo.lockScene) {
			Debug.LogError ( "----------locked return--------------" )  ;
			UnityEngine.GameObject nTipWindow = UnityEngine.Resources.Load ("Window/WindowTipsThree")as UnityEngine.GameObject;
			UnityEngine.GameObject nTipWindowEntity = UnityEngine.GameObject.Instantiate ( nTipWindow );
			UITipAutoNoMask ClickTips1 = nTipWindowEntity.GetComponent<UITipAutoNoMask> ();
			ClickTips1.tipText.text = "操作太過頻繁！！！";
			return;
		}
		AppControl.ToView (AppView.PKHALLMAIN);
	}

	public void OnOpenTinyGame()
	{
		Debug.LogError ( "open tiny game start!!!" );
		GameObject Window = Resources.Load ("Window/WindowTips")as GameObject;
		GameObject WindowClone = Instantiate (Window);
		UITipClickHideManager ClickTips = WindowClone.GetComponent<UITipClickHideManager> ();
		ClickTips.text.text = "暫未開放，敬請期待！";
		ClickTips.time.text = "3";
	}

	public void ShowStartGiftBoxTip()
	{
		if (mCountDownBoxTip == null) {
			mCountDownBoxTip = StartGiftManager.OpenCountDownBoxTip ( container.transform , true );
		}
	}

	public void Frist()
	{
		AudioManager._instance.PlayEffectClip (AudioManager.ui_click);
		string path = "Window/FirstWindow";
		GameObject WindowClone = AppControl.OpenWindow (path);
		WindowClone.SetActive (true);

	}
		
	public void  MouthCard()
	{
		//关闭签到界面弹出月卡领取界面，如果有用户在弹出月卡弹窗直接强退，再次进入的时候点击大厅的月卡还是可以再次领取
		//打开月卡确认是否有购买月卡记录以及购买后是否当天有领取
		//如果是购买则显示领取，如果当天已经领取过显示获取的按键
		//所以获取按键必须向服务器发送今天已经领取的消息
		AudioManager._instance.PlayEffectClip (AudioManager.ui_click);
		string path = "Window/MouthCardWindow";
		GameObject WindowClone = AppControl.OpenWindow (path);
		WindowClone.SetActive (true);

		UIMouthCard nMonthData = WindowClone.GetComponent<UIMouthCard> ();
		//如果已经领取了，那么显示获取按钮，可以继续购买月卡礼包，增加持续时间
		//nMonthData.SetRemianDays ( (int)myInfo.loginInfo.monthlyCardDurationDays , myInfo.loginInfo.monthlyPackGot );
	}

	public void ToVip()
	{
		AudioManager._instance.PlayEffectClip (AudioManager.ui_click);
		GameObject Window = Resources.Load ("Window/VIP")as GameObject;
		Instantiate (Window);
		HideSelf ();
	}
		
	public void GiftScence()
	{
		Debug.Log ("myInfo.cannonMultipleNow"+myInfo.cannonMultipleNow);
		//判断炮倍数，满足300倍炮则进入渔场，弹出提示窗口,打开窗口要求传炮倍数CannonWindowTips
		GameObject Window1 = UnityEngine.Resources.Load ("Window/WindowTipsThree")as UnityEngine.GameObject;
		GameObject WindowClone1 = UnityEngine.GameObject.Instantiate ( Window1 );
		UITipAutoNoMask ClickTips1 = WindowClone1.GetComponent<UITipAutoNoMask> ();
		ClickTips1.tipText.text = "此功能暫未開放";
		return;
		if (myInfo.cannonMultipleMax >= 300) {
			
			//向服务器发送请求进入红包场
			//UIHallMsg.GetInstance().SndEnterRedPacketRoomRequest(1);
		}
		else {
			GameObject Window = Resources.Load ("Window/WindowTips")as GameObject;
			GameObject WindowClone = Instantiate (Window);
			UITipClickHideManager ClickTips = WindowClone.GetComponent<UITipClickHideManager> ();
			ClickTips.text.text = "解鎖300倍倍炮開啟紅包場，獲得紅包券，兌換微信紅包!";
		}
	}

	public void StartGame()
	{
		if (myInfo.lockScene) {
			Debug.LogError ( "----------locked return--------------" )  ;
			UnityEngine.GameObject nTipWindow = UnityEngine.Resources.Load ("Window/WindowTipsThree")as UnityEngine.GameObject;
			UnityEngine.GameObject nTipWindowEntity = UnityEngine.GameObject.Instantiate ( nTipWindow );
			UITipAutoNoMask ClickTips1 = nTipWindowEntity.GetComponent<UITipAutoNoMask> ();
			ClickTips1.tipText.text = "操作太過頻繁！！！";
			return;
		}
		AudioManager._instance.PlayEffectClip (AudioManager.ui_click);
		UIHallObjects.GetInstance().StartGame ();
		newHandEffect.SetActive (false);
	}

	void OnRcvButton(MsgBoxType.Result result)
	{
		if(result == MsgBoxType.Result.Confirm)
		{
			print ("OnRcvButton Confirm");
		}

		if(result == MsgBoxType.Result.Cancel)
		{
			print ("OnRcvButton Cancel");
		}
	}

	void OnDestroy()
	{
		StopCoroutine ( "UpdateClientInfo" );
		Facade.GetFacade ().ui.Remove( FacadeConfig.UI_HALL_MODULE_ID );
		UIStore.HideEvent -= HideSelf;
		UIVIP.SeeEvent -= SeeSelf;
		UIUpgrade.HideEvent -= HideSelf;
		//UISetSailGift.SeeHandEvent -= NewHandEffect;
	}
}
