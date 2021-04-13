using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using AssemblyCSharp;

public class MyInfo : IDataProxy
{
	/*public const int STATE_IN_LOGIN  = 0;
	public const int STATE_IN_HALL   = 0;
	//当前用户状态索引标识常量，正在进入经典房间
	public const int STATE_ENTER_CLASSICROOM = 1;

	public const int STATE_IN_CLASSICROOM = 2;

	public const int STATE_IN_FISHINGROOM = 4;

	public const int STATE_ENTER_FISHINGROOM = 5;*/

	public int loginType;
    //0:帳號登入 1:Wechat登入 2:QQ登入
	public int loginAccountType;
	//normal,wechat,qq,sina
	public int systemType;
	//ios,android,mac,windows
	public int deviceType;
	//iphone,huawei,meizu,mi

	//是否有升級獎勵可以領取
	public bool isLevelupCanGet = false;
    //是否領取完升級獎勵
	public bool isUserGetAllUpLevel = false;
	//特惠底包買完了沒
	public bool isFinishTH = false;
	//等級大於20?
	//public bool isHigher20 = false;
	//七日新手
	public bool isNewSevenHand = true;

	//pk场选择的房间类型，有子弹赛，限时赛，对抗赛
	public int PK_EnterRoomType = 0;
	//public int PK_SelectLevel = 0;
	//public int EnterPkRoomType = 0;

	public bool isLogining = false;

	//public string loginName= ""; //登录名
	//public string password= ""; //密码

	public string nickname = "";
	//昵称
	public string avatar = "";
	//头像
	public int userID;
	//ID编号
	public int sex;
	//性别：0 girl, 1 boy  值为1时是男性，值为2时是女性，值为0时是未知

	public Texture2D headTexture = null;

	public int level;
	//普通等级
	public int experience;
	//经验

	public int nextLevelExp;

	public int levelVip;
	//VIP等级
	public int cannonMultipleMax;
	//炮倍数
	public int cannonMultipleNow;
	//当前炮倍数

    //選擇砲台購買暫存
	public int ChooseGunTemp;
/*
     * xlplg1.272gb.com 183.146.209.245
       xlplg3.272gb.com 183.146.209.246
       xlplg13.272gb.com  115.238.196.42
       xlplg7.272gb.com 183.146.209.245
       xlplg9.272gb.com 183.146.209.246
     */
	public string ServerUrl = "";
    public bool isServerMsgUseDefault = false;
	//public string[] ServerDefaultMsg = new string[11] {"http://down.xinlongbuyu.com/buyu/index.html", "http://down.xinlongbuyu.com/buyu/index.html", "https://down.lhzdbg.cn/3d/ldy/xinlongbuyu/index.html",
	//													"http://gamelobby.cxtqgypc.cn/LobbyLogin.aspx", "https://xlconfig.lhzdbg.cn/GameAPI/API.aspx", "https://xlapi.lhzdbg.cn/Weixin/PayH5/NewL_Pay3D/pay.aspx",
	//													"http://183.131.69.227:8004/GameConfig/GetNoticeAndActivity", "http://mobile.cl0579.com/api/api_YKBind.aspx", "xlplg13.272gb.com",
	//													"http://api.cl0579.com:8081/api/MobilePayConfig.aspx", "13588633259"};
	// public string[] ServerURLArray1 = new string[6] { "183.131.69.230", "183.146.209.248", "183.146.209.248", "183.146.209.248", "183.146.209.248", "183.146.209.248" };
	// "183.131.69.230", "183.131.69.230", "183.131.69.230", "183.131.69.230", "183.131.69.230", "183.131.69.230"

    //多個登入域名
	public string[] ServerURLArray1 = new string[6] { "xlplg1.272gb.com", "xlplg3.272gb.com", "xlplg5.272gb.com", "xlplg7.272gb.com", "xlplg9.272gb.com", "xlplg11.272gb.com" };
	public string[] ServerURLArray2 = new string[6] { "xlplg1.272gb.com", "xlplg3.272gb.com", "xlplg5.272gb.com", "xlplg7.272gb.com", "xlplg9.272gb.com", "xlplg11.272gb.com" };
	public string[] RandomServerURL = new string[6];
	public bool connectServerAlr = false;
	public bool isconnecting = false;
	public bool isReconnectInPKRoom = false;
	public bool isPhoneNumberLogin = false;

	public int cannonStyle;
	/// <summary>
	/// 炮座
	/// </summary>
	public int cannonBabetteStyle;

	public double redPacketTicket;

	public int topupSum;
	//充值数
	public long gold;
	//金币
	public long _diamond;
	//钻石

	public long diamond {
		set { 
			_diamond = value;
//			Debug.LogError ( "--------------------" + _diamond );
		}
		get { return _diamond; }
	}

	public string openId = "";
	public string acessToken = "";
	public int user_id;
	//手機號碼登入儲存號碼
	public string phone = "";
	public string oppoAcessToken = "";
	public string vivoAcessToken = "";
	public string huaweiAcessToken = "";
	//根據手機號碼關聯帳號
	public int associateIndex;
    public ArrayList Associate_Type = new ArrayList();
	public ArrayList Associate_Userid = new ArrayList();
	public ArrayList Associate_Name = new ArrayList();
	public ArrayList Associate_Token = new ArrayList();
	public ArrayList Associate_Nickname = new ArrayList();
	//oppo手机号
	public string email;
	//电子邮件
	public string channel;
	//oppo渠道号
	public string adld;
	public int platformType = 0;

	public string account = "";
	public string password = "";

	public bool isGuestLogin = false;
	public int teihui = 0;

	public LoginUnit mLoginData;

	public bool misHaveDraCard = false;
   
	public int beginnerCurTask;
	public int beginnerProgress;
	public long roomCard;
	public bool isShowTreasure = false;
	//是否显示三选一礼包
	public bool isShowAwesome = false;
	//是否显示一元礼包
	//public bool isShowDouble = false;

	//推廣版 显示一元礼包
	public bool isShowDouble = true;

	//是否显示二选一礼包
	public int isShowNumner;
	//记录是否显示一元礼包图标
	public bool bAfterCharge = true;
	public int showAwesome;
	public int showDouble;
	public int showTreasure;

    //動態下發付款金額、金幣
	//龍卡付款、金幣資訊
	public ArrayList Pay_Drang_RMB = new ArrayList();
	public ArrayList Pay_Drang_AddGold = new ArrayList();
	public ArrayList Pay_Drang_id = new ArrayList();
	//特惠付款、金幣資訊
	public ArrayList Pay_Preferential_RMB = new ArrayList();
	public ArrayList Pay_Preferential_AddGold = new ArrayList();
	public ArrayList Pay_Preferential_id = new ArrayList();
	//三選一 發現寶藏 付款、金幣資訊
	public ArrayList Pay_Three_RMB = new ArrayList();
	public ArrayList Pay_Three_AddGold = new ArrayList();
	public ArrayList Pay_Three_id = new ArrayList();
	//二選一 雙喜臨門 付款、金幣資訊
	public ArrayList Pay_Two_RMB = new ArrayList();
	public ArrayList Pay_Two_AddGold = new ArrayList();
	public ArrayList Pay_Two_id = new ArrayList();
	//新手七日付款、金幣資訊
	public ArrayList Pay_NewSeven_RMB = new ArrayList();
	public ArrayList Pay_NewSeven_AddGold = new ArrayList();
	public ArrayList Pay_NewSeven_id = new ArrayList();
	//商城付款資訊
	public ArrayList Pay_Store_RMB = new ArrayList();
	public ArrayList Pay_Store_AddGold = new ArrayList();
    public ArrayList Pay_Store_id = new ArrayList();

	public FiLoginResponse loginInfo = null;

	public bool oneMoreGame;
	//是否再来一局游戏
	public GameInfo lastGame = new GameInfo ();
	//最近一次游戏的模式

	public ServerAppData mSvrData = null;
	public int isBindPhone;
	//是否绑定手机
	public string strPhoneNum;
	//手机号码


	public int sailDay = 0;
	//是否已经显示过起航礼包提示

	private int signstatue = 0;
	public bool bDisplayedStartGift = false;

	public List<FiDailySignIn> signInArray;

	public int seatIndex;
	private CannonInfo cannonInfo = null;
	private List<FiProperty> properties = null;

	//	public bool bWaitingGetCode = false;

	public int SmallGame = 1;
	//小游戏开关
	public int RankingList = 1;
	//排行榜开关
	public int Charm = 1;
	//魅力兑换开关
	public int Gift = 1;
	//礼物赠送开关
	public int Consume = 200;
	//用户发公告消耗
	public int Timer = 1;
	//发公告间隔
	public int NoticeWindow = 1;
	//公告控制面板

	//体验场携带金币
	public long testCoin;
	//是否是体验场
	public int isTestRoom;

	//支付是否打开支付宝支付
	public int isOpenAlipay = 1;
	//商场是否打开微信支付
	public int isOpenWechat = 1;

	public int nManmon;

	public int isNewUser;

	public int IsNewSevenUser = 1;
	/// <summary>
	/// Boss状态 (登录下发)
	/// </summary>
	public int myBossMatchState;

	public bool isCLAccountLogin {
		get {
			if (platformType == 0 || platformType == 9) {
				return true;
			}
			return false;
		}
	}

	public AppView TargetView;

	public int GetTokenType ()
	{
		if (platformType == 22) {
			return 3;
		} else if (platformType == 24) {
			return 4;
		} else if (platformType == 0) {
			return 5;
		}
		return 10;
	}

	//private int mUserState = -1;

	//	public void SetState( int nStateNum ){
	//		mUserState = nStateNum;
	//	}
	public void SetVip (int topup, int level)
	{
		topupSum = topup + topupSum;
		levelVip = level;
	}

	bool isSceneLocked = false;
	System.Timers.Timer nTimer = new System.Timers.Timer (3000);

	public bool lockScene {
		get{ return isSceneLocked; } 
		set {  
			isSceneLocked = value;

			if (nTimer != null) {
				nTimer.Stop ();
			}
			if (isSceneLocked) {
				nTimer = new System.Timers.Timer (3000);
				nTimer.Elapsed += OnTimerEnd;//new System.Timers.ElapsedEventHandler (OnTimerEnd);
				nTimer.AutoReset = false;//设置是执行一次（false）还是一直执行(true)；
				nTimer.Enabled = true;
				nTimer.Start ();
			}
		}
	}

	void OnTimerEnd (object source, System.Timers.ElapsedEventArgs e)
	{
		nTimer.Elapsed -= OnTimerEnd;
		isSceneLocked = false;
		//Debug.LogError ("[ lock scene ] timeout -------------------------------");
	}

	//	public int GetState()
	//	{
	//		return mUserState;
	//	}
	//
	//	public bool isEnterClassicRoom()
	//	{
	//		return (mUserState == STATE_ENTER_CLASSICROOM || mUserState == STATE_IN_CLASSICROOM );
	//	}

	public MyInfo ()
	{
		ClearMyInfo ();
		lastGame = new GameInfo ();
		properties = new List<FiProperty> ();
	}

	public void OnAddData (int nType, object nData)
	{

	}

	public void OnInit ()
	{

	}


	//标记签到日期，标示今天已经签到了，之后进入大厅页面不跳出签到的显示窗口
	public void markSignInDay ()
	{
		if (signInArray != null) {
			List<int > nListSign = new List<int> ();
			foreach (FiDailySignIn nSign in signInArray) {
				if (nSign.status == 1 && nSign.day > 0) {
					nSign.status = 2;//标识已经领取了签到
				}
			}
		}
	}

	public void Redaystatue (int day)
	{
		if (signInArray != null) {
			for (int i = 0; i < signInArray.Count; i++) {
				if (signInArray [i].day == day) {
					signInArray [i].status = 2;
					Debug.LogError ("zzzzzzzzzzzzzz" + signInArray [i].day);
				}
			}
		}
		for (int i = 0; i < signInArray.Count; i++) {
			Debug.LogError ("zkkzkzkzkkzkzkz" + signInArray [i].status);
		}
	}

	public List<FiDailySignIn> GetsSignArray ()
	{
		return signInArray;
	}

	public void  SetignStatue (int statue)
	{
		signstatue = statue;
	}

	public int GetSignStatue ()
	{
		return signstatue;
	}

	public int getSignInDay ()
	{
		if (signInArray != null) {
			List<int > nListSign = new List<int> ();
			foreach (FiDailySignIn nSign in signInArray) {
				if (nSign.status == 1 && nSign.day > 0) {
					int nDay = nSign.day;
					return nDay;
				}
			}
		}
		return 0;
	}

	public void OnDestroy ()
	{
		sailDay = 0;
		bDisplayedStartGift = false;
		signInArray = null;
		ClearMyInfo ();
	}

	~MyInfo ()
	{
		Clear ();
	}

	void ClearMyInfo ()
	{
		//loginName = "";
		//password = "";
		isReconnectInPKRoom = false;
		//nickname = "";
		avatar = "";
		userID = 0;
		//sex = 1;

		level = 0;
		experience = 0;
		levelVip = 0;
		cannonMultipleMax = 0;
		cannonMultipleNow = 0;

		topupSum = 0;
		gold = 0;
		diamond = 0;

		//openId = "";
		//acessToken = "";

		seatIndex = -1;

		oneMoreGame = false;
		lastGame = new GameInfo ();

		cannonInfo = new CannonInfo ();
	}

	public void SetInfo (string nickname, string avatar)
	{
		this.nickname = nickname;
		this.avatar = avatar;
	}

	public void SetCannonInfo (CannonInfo info)
	{
		cannonInfo = info;
	}

	public CannonInfo GetCannonInfo ()
	{
		return cannonInfo;
	}

	public void ClearFishingInfo ()
	{
		seatIndex = 0;
		SetCannonInfo (null);
		ClearProperties ();
	}

	public void SetPKReconnectProperties (List<FiProperty> info)
	{
		properties = info;
	}

	public List<FiProperty> GetPkReconnectProperties ()
	{
		return properties;
	}

	public void ClearProperties ()
	{
		properties.Clear ();
	}

	public void Clear ()
	{
		ClearMyInfo ();
		ClearFishingInfo ();
	}
}
