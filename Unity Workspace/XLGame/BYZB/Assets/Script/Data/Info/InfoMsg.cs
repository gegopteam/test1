/* author:KinSen
 * Date:2017.07.07
 */

using UnityEngine;
using System.Collections;
using AssemblyCSharp;
using System.Runtime.CompilerServices;
using UnityEngine.Internal;
using UnityEngine.Scripting;

//负责：数据层处理 , 断线处理
//比如：MyInfo，RoomInfo，BackpackInfo等
//还有比如：炮，子弹，鱼信息的话就由渔场自行管理就好。
public class InfoMsg : IDispatch
{
	private static InfoMsg instance = null;

	public static InfoMsg GetInstance ()
	{
		if (null == instance) {
			instance = new InfoMsg ();
		}

		return instance;
	}

	public static void DestroyInstance ()
	{
		if (null != instance) {
			instance = null;
		}
	}
	//    "登录异样 请重新登录"
	private DataControl dataControl = null;
	private DispatchControl dispatchControl = null;

	//data info
	private MyInfo myInfo = null;
	private BackpackInfo backpackInfo = null;
	private RoomInfo roomInfo = null;
	private FriendInfo mFriendInfo = null;

	private bool mIsReConnected = false;

	private InfoMsg ()
	{
		InitInfo ();
	}

	~InfoMsg ()
	{
		UnInit ();
	}



	private void InitInfo ()
	{
		dispatchControl = DispatchControl.GetInstance ();
		dataControl = DataControl.GetInstance ();    
		dispatchControl.AddRcv (AppFun.DATA, this);

		myInfo = dataControl.GetMyInfo ();
		backpackInfo = dataControl.GetBackpackInfo ();
		roomInfo = dataControl.GetRoomInfo ();


		EventControl nControl = EventControl.instance ();
		nControl.addEventHandler (FiEventType.CONNECT_SUCCESS, RcvConnectSuccess);
		//nControl.addEventHandler ( FiEventType.CONNECT_TIMEROUT, RcvConnectTimeOut );
		nControl.addEventHandler (FiEventType.RECV_LOGIN_RESPONSE, RcvLoginResponse);
//        nControl.addEventHandler (  FiEventType.RECV_BACKPACK_RESPONSE , RcvBackpackResponse );
		//nControl.addEventHandler (  FiEventType.RECV_HAVE_DISCONNECTED_ROOM_INFORM , RcvPKHaveDisconnectedRoomInform );
		//    nControl.addEventHandler (  FiEventType.RECV_RECONNECT_GAME_RESPONSE , RcvPKRoomReconnectResponse );

		//nControl.addEventHandler (FiEventType.CONNECTIONT_CLOSED, RcvConnectionClose);//只有在登陆成功后，才会
	}

	GameObject mCloseTipWin = null;

	//只有在连接成功后，网络异常的情况下才能收到这条消息
	public void RcvConnectionClose (object data)
	{
		//ShowReplacement (true);
		Debug.LogError ("------------RcvConnectionClose-----------!!!!!!!!!!!" + mCloseTipWin);
		DataControl.GetInstance ().ShutDown ();
		//Debug.Log ("RcvConnectionClose = 111" + mCloseTipWin.activeSelf);-->错误的代码:对象为空不能获取对象状态
		if (mCloseTipWin == null || !mCloseTipWin.activeSelf) {
			//Debug.Log ("RcvConnectionClose = 222" + mCloseTipWin.activeSelf);-->错误的代码:对象为空不能获取对象状态

			//SystemManger.instans.StartIEnumerator (Replacement ());//开启转圈等待

//            Debug.Log ("SystemManger.instans.StartIEnumerator (Replacement ()) = 3333 ");
			GameObject Window = Resources.Load ("Window/WindowTips") as GameObject;
			mCloseTipWin = GameObject.Instantiate (Window);
			UITipClickHideManager ClickTip = mCloseTipWin.GetComponent<UITipClickHideManager> ();
			ClickTip.text.text = "網絡連接中斷!\n請點擊確定重新登錄遊戲。";
			ClickTip.SetClickCallback (() => {
				AppControl.ToView (AppView.LOGIN);
			});
			AppInfo.isFritsInGame = true;
		}

//        GameObject Window1 = UnityEngine.Resources.Load ("Window/WindowTipsThree")as UnityEngine.GameObject;
//        GameObject WindowClone1 = UnityEngine.GameObject.Instantiate (Window1);
//        UITipAutoNoMask ClickTips1 = WindowClone1.GetComponent<UITipAutoNoMask> ();
//        ClickTips1.tipText.text = "网络连接中断!";
		//连接断开啦，那么跳到登陆页面哦
		//    myInfo.SetState (MyInfo.STATE_IN_LOGIN);
		myInfo.isLogining = false;
	}

	IEnumerator Replacement ()
	{
		yield return new WaitForSeconds (2f);
		ShowReplacement (false);
	}

	GameObject mWaitingView;

	void ShowReplacement (bool nValue)
	{

		if (nValue) {
			GameObject Window1 = Resources.Load ("MainHall/Common/WaitingView") as GameObject;
			mWaitingView = GameObject.Instantiate (Window1);
			UIWaiting nData = mWaitingView.GetComponentInChildren<UIWaiting> ();
			nData.HideBackGround ();
			nData.SetInfo (15.0f, "獲取信息異常，請再試一次!");
		} else {
			if (mWaitingView.activeSelf) {
				SystemManger.instans.DestroyGameobject (mWaitingView);
			}
			mWaitingView = null;
		}
	}

	bool isSendLoginMessage = false;

	//连接池
	private void RcvConnectSuccess (object nValue)
	{
		//如果是重连后的成功，重新发送登陆消息
		//Debug.LogError("------------RcvConnectSuccess----------- need send login message : " + mIsReConnected );
		/*if ( mIsReConnected ) {
            DispatchData data = new DispatchData ();
            data.type = FiEventType.SEND_LOGIN_REQUEST;
            FiLoginRequest loginInfo = new FiLoginRequest ();
            data.data = (object)loginInfo;
            loginInfo.accessToken = "";
            loginInfo.openId   = myInfo.openId; //每个人机子上对应自己的编号，金进胜1，蓝天2，杨水清3，夏昕4
            loginInfo.nickname = myInfo.nickname;
            loginInfo.avatarUrl = myInfo.avatar;
            dataControl.PushSocketSnd (data);
        }
        mIsReConnected = true;*/
		/*GameObject Window1 = UnityEngine.Resources.Load ("Window/WindowTipsThree")as UnityEngine.GameObject;
        GameObject WindowClone1 = UnityEngine.GameObject.Instantiate ( Window1 );
        UITipAutoNoMask ClickTips1 = WindowClone1.GetComponent<UITipAutoNoMask> ();
        //ClickTips1.tipText.text = "连接建立成功，发送登陆请求";*/
		if (!isSendLoginMessage) {
			isSendLoginMessage = true;
			Debug.LogError ("連接建立成功，發送登陸請求");
			Facade.GetFacade().message.login.SendLoginRequest();
		}
	}

	void AutoAccountLoginFile (string pathUrl, string Account, string password)
	{
		string loginStr = "{\"login\":{" + "\"Account\":\"" + myInfo.account +
		                  "\"," + "\"password\":\"" + myInfo.password +
		                  "\"," + "\"isQQLogin\":" + (LoginInfo.isQQLogin ? 1 : 0) +
		                  "," + "\"isWechatLogin\":" + (LoginInfo.isWechatLogin ? 1 : 0) +
		                  "," + "\"isAccoutLogin\":" + (LoginInfo.isAccoutLogin ? 1 : 0) +
		                  "}}";
#if UNITY_EDITOR
        Debug.LogError("loginStr = " + loginStr);
#endif
        FileUtilManager.CreateFile (pathUrl, "Login.txt", loginStr);
		LoginInfo.accountStr = Account;
		MyInfo myinfo = DataControl.GetInstance ().GetMyInfo ();
		LoginInfo.passwordStr = myinfo.password;
	}

	private void RcvLoginResponse (object data)
	{
        //Debug.Log ("$$$$$$$$$$$RcvLoginResponse===");
		isSendLoginMessage = false;
		FiLoginResponse loginResponse = (FiLoginResponse)data;
		if (LoginUtil.GetIntance () != null) {
			LoginUtil.GetIntance ().ShowWaitingView (false);
		}
		Debug.Log ("loginResponse.reuslt = " + loginResponse.reuslt);//登录返回
		if (loginResponse.reuslt == 0) {
			AutoAccountLoginFile (LoginInfo.pathUrl, myInfo.account, myInfo.password);

			//登陆成功过，重连后发送登陆的消息
			mIsReConnected = true;
			if (myInfo.isGuestLogin == false) {
				#if UNITY_IPHONE && !UNITY_EDITOR
                //Debug.Log ("InfoMsg   IsHasAccountLogin ()11 == " + UILogin.IsHasAccountLogin ());
                //Debug.Log ("InfoMsg   IsHasGuestLogin ()11 == " + UILogin.IsHasGuestLogin ());
                UILogin.ToSetAccountLogin ();
                //Debug.Log ("InfoMsg   IsHasAccountLogin ()22 == " + UILogin.IsHasAccountLogin ());
                //Debug.Log ("InfoMsg   IsHasGuestLogin ()22 == " + UILogin.IsHasGuestLogin ());
				#elif UNITY_ANDROID && !UNITY_EDITOR
                //因渠道登录 需要关闭账号登录功能
                //UILogin.AccountLoginToAndroid ();
				#endif
			}        
			if (null != myInfo) {
				myInfo.userID = (int)loginResponse.userId;
				//清除名称中的空格
				string nicTrim = loginResponse.nickname.Trim (new char[]{ ' ' });
				myInfo.nickname = nicTrim;
				myInfo.avatar = loginResponse.avatar;
				myInfo.levelVip = loginResponse.vipLevel;
				myInfo.level = loginResponse.level;
				myInfo.experience = loginResponse.experience;
				myInfo.gold = loginResponse.gold;
				myInfo.diamond = loginResponse.diamond;
				myInfo.topupSum = loginResponse.topupSum;
				myInfo.cannonMultipleMax = loginResponse.maxCanonMultiple;
				myInfo.cannonMultipleNow = myInfo.cannonMultipleMax;
				myInfo.redPacketTicket = loginResponse.redPacketTicket; //服务器下发的红包券单位为分，显示时要转化为元
				myInfo.cannonStyle = loginResponse.cannonStyle;
//                Debug.LogError ("myInfo.cannonStyle = " + myInfo.cannonStyle);
				myInfo.sailDay = loginResponse.sailDay;
				myInfo.signInArray = loginResponse.signInArray;
				myInfo.nextLevelExp = loginResponse.nextLevelExp;
				myInfo.beginnerCurTask = loginResponse.beginnerCurTask;
				myInfo.beginnerProgress = loginResponse.beginnerProgress;
        
				//myInfo.sex = loginResponse.gender;
				myInfo.roomCard = loginResponse.roomCard;
				myInfo.testCoin = loginResponse.testCoin;
				myInfo.isTestRoom = loginResponse.isTestRoom;
				myInfo.cannonBabetteStyle = loginResponse.cannonBabetteStyle;
				myInfo.nManmon = loginResponse.nmanmonnum;
				myInfo.myBossMatchState = loginResponse.bossMatchState;
				myInfo.isNewUser = loginResponse.isNewUser;
				myInfo.IsNewSevenUser = loginResponse.IsResterUserSteate;
				Debug.LogError ("是否是新用户==" + loginResponse.isNewUser);
//                Debug.LogError ("__________________________loginResponse.cannonMultipleMax" + loginResponse.maxCanonMultiple);
//                Debug.LogError ("__________________________myInfo.cannonMultipleMax" + myInfo.cannonMultipleMax);

//                Debug.LogError ("myInfo.cannonBabetteStyle = " + myInfo.cannonBabetteStyle);
//                if (myInfo.isGuestLogin) {
//                    myInfo.nickname = "guest"+ loginResponse.userId;
//                }
				//myInfo.loginInfo = loginResponse;
				//myInfo.account = null;
			}
			LoginUtil.ShowWaitingMask (false);
			dataControl.PushSocketSnd (FiEventType.SEND_BACKPACK_REQUEST, null);
			dataControl.PushSocketSnd (FiEventType.START_HEART_BEAT, null);
			dataControl.PushSocketSnd (FiEventType.SEND_CL_USE_DARGON_CARD_REQUEST, null);
			dataControl.PushSocketSnd (FiEventType.SEND_CL_PREFERENTIAL_MSG_REQUEST, null);//发送特惠消息
			//请求一次大赢家的数据,因为用户如果不再大厅打开大赢家直接去渔场,加成状态就获取不到,限制就发一次
			dataControl.PushSocketSnd (FiEventType.SEND_XL_HORODATA_RESQUSET, null);
			//发送七日初始协议
			dataControl.PushSocketSnd (FiEventType.SEND_XL_SEVENDAY_STARTINFO_OPEN_NEW_RESPOSE, null);
			//發送获取活动 升级任务进度信息
			if (AppInfo.trenchNum > 51000002)
				dataControl.PushSocketSnd(FiEventType.SEND_XL_LEVELUP_INFO_NEW_RESPOSE, null);
			//发送购买给力 双喜 宝藏 状态协议
			dataControl.PushSocketSnd (FiEventType.SEND_XL_TOP_UP_GIFT_BAG_STATE_INFO_NEW_RESPOSE, null);
			//发送获取按钮状态请求
			dataControl.PushSocketSnd (FiEventType.SEND_XL_BUTTON_HIDE_STATE, null);
			if (AppControl.miniGameState) {
                
			} else {
				LoginUtil.GetIntance ().JumpLoadingView ();
			}
			AppControl.miniGameState = false;
			if (UIUserDetail.instace != null) {
				UIUserDetail.instace.SetGold (myInfo.gold);
				UIUserDetail.instace.SetDiamond (myInfo.diamond);
			}
			Debug.Log("FangChenMiFangChenMiFangChenMiFangChenMiFangChenMiFangChenMiFangChenMiFangChenMi");
			FangChenMi._instance.StartTimer();
			//DataControl.GetInstance ().GetMyInfo ().SetState (MyInfo.STATE_IN_HALL);
			//AppControl.ToView (AppView.HALL);
			/*if (myInfo.avatar != null) {
                IUiMediator nMediator = Facade.GetFacade ().ui.Get (FacadeConfig.UI_HALL_MODULE_ID);
                if( nMediator != null )
                    nMediator.OnRecvData (FiEventType.RECV_LOGIN_RESPONSE, null);
            }*/
		} else if (loginResponse.reuslt == 1100 && AppControl.miniGameState == false) {
//            Debug.LogError ("------------------loginResponse.reuslt111 = ");
			//myInfo.bWaitingGetCode = true;
			DataControl.GetInstance ().ShutDown ();
			GameObject Window1 = UnityEngine.Resources.Load ("Window/AuthCodeWindow")as UnityEngine.GameObject;
			UnityEngine.GameObject.Instantiate (Window1);
			myInfo.mLoginData = new LoginUnit ();


			myInfo.connectServerAlr = true;
			myInfo.isconnecting = true;
			LoginUtil.GetIntance().ForStopIE();
		} else if (loginResponse.reuslt == -510999) {
//            Debug.LogError ("------------------loginResponse.reuslt222 = ");
			DataControl.GetInstance ().ShutDown ();
			GameObject Window1 = UnityEngine.Resources.Load ("Window/WindowTipsThree")as UnityEngine.GameObject;
			GameObject WindowClone1 = UnityEngine.GameObject.Instantiate (Window1);
			UITipAutoNoMask ClickTips1 = WindowClone1.GetComponent<UITipAutoNoMask> ();
			ClickTips1.tipText.text = "該帳號正在遊戲中" + loginResponse.errorMsg;
//            DeleteUtil ();
			myInfo.account = null;
			LoginUtil.GetIntance ().ShowWaitingView (false);
			myInfo.connectServerAlr = true;
			myInfo.isconnecting = true;
			LoginUtil.GetIntance().ForStopIE();
		} else if(loginResponse.reuslt == 7)
		{
			LoginUtil.GetIntance().ShowWaitingView(false);
			myInfo.connectServerAlr = true;
			myInfo.isconnecting = true;
			LoginUtil.GetIntance().ForStopIE();
			Debug.LogError("------------------loginResponse.reuslt333 = "+ loginResponse.reuslt);
            GameObject Window1 = UnityEngine.Resources.Load ("Window/WindowTipsThree")as UnityEngine.GameObject;
			GameObject WindowClone1 = UnityEngine.GameObject.Instantiate (Window1);
			UITipAutoNoMask ClickTips1 = WindowClone1.GetComponent<UITipAutoNoMask> ();
			ClickTips1.tipText.text = "您的帳號不存在或是密碼輸入有誤，請重新登入 ";
			//            DeleteUtil ();
			DataControl.GetInstance ().ShutDown ();
			myInfo.account = null;
		}
		else
		{
			Debug.LogError("------------------loginResponse.reuslt333 = " + loginResponse.reuslt);
			GameObject Window1 = UnityEngine.Resources.Load("Window/WindowTipsThree") as UnityEngine.GameObject;
			GameObject WindowClone1 = UnityEngine.GameObject.Instantiate(Window1);
			UITipAutoNoMask ClickTips1 = WindowClone1.GetComponent<UITipAutoNoMask>();
			ClickTips1.tipText.text = "登入失敗";
			//            DeleteUtil ();
			DataControl.GetInstance().ShutDown();
			myInfo.account = null;
			LoginUtil.GetIntance().ShowWaitingView(false);

			myInfo.connectServerAlr = true;
			myInfo.isconnecting = true;
			LoginUtil.GetIntance().ForStopIE();
		}
		//        Debug.LogError ("------------------loginResponse.reuslt444 = ");

		myInfo.loginInfo = loginResponse;
		myInfo.isLogining = false;
	}

	//    void DeleteUtil ()
	//    {
	//        if (FileUtilManager.IsExistsFile (LoginInfo.pathUrl, "Login.txt")) {
	//            FileUtilManager.DeleteFile (LoginInfo.pathUrl, "Login.txt");
	//            LoginInfo.isQQLogin = false;
	//            LoginInfo.isWechatLogin = false;
	//            LoginInfo.isAccoutLogin = false;
	//            LoginInfo.accountStr = "";
	//            LoginInfo.passwordStr = "";
	//        }
	//    }

	private void UnInit ()
	{
		dispatchControl = null;
		dataControl = null;
		myInfo = null;
		backpackInfo = null;
		roomInfo = null;
	}

	public void OnRcv (int type, object data)
	{//注:数据部分不关心的消息数据不接
		switch (type) {
		case FiEventType.RECV_ENTER_PK_ROOM_RESPONSE: //进入PK准备房间消息回复
			IMRcvPKEnterRoomResponse (data);
			break;
		case FiEventType.RECV_OTHER_ENTER_PK_ROOM_INFORM: //其他人进入PK准备房间通知
			IMRcvPKOtherEnterRoomInform (data);
			break;
		case FiEventType.RECV_LEAVE_PK_ROOM_RESPONSE: //离开PK准备房间消息回复
			IMRcvPKLeaveRoomResponse (data);
			break;
		case FiEventType.RECV_OTHER_LEAVE_PK_ROOM_INFORM: //其他人离开PK准备房间消息通知
			IMRcvPKOtherLeaveRoomInform (data);
			break;
		//case FiEventType.RECV_OTHER_PREPARE_PKGAME_INFORM: //接收其他玩家准备通知
		//    IMRcvPKOtherPrepareGameInform (data);
		//    break;
		//case FiEventType.RECV_OTHER_CANCEL_PREPARE_PKGAME_INFORM: //接收其他玩家取消准备通知
		//    IMRcvPKOtherCancelPrepareGameInform (data);
		//    break;

		//
		//case FiEventType.RECV_CREATE_FRIEND_ROOM_RESPONSE: //接收创建好友约战房间回复
		//    IMRcvPKCreateFriendRoomResponse (data);
		//    break;
		//case FiEventType.RECV_ENTER_FRIEND_ROOM_RESPONSE: //接收进入好友约战房间回复
		//    IMRcvPKEnterFriendRoomResponse (data);
		//    break;
		//case FiEventType.RECV_OTHER_ENTER_FRIEND_ROOM_INFORM: //接收其他玩家进入好友约战房间通知
		//    IMRcvPKOtherEnterFriendRoomInform (data);
		//    break;
		//case FiEventType.RECV_LEAVE_FRIEND_ROOM_RESPONSE: //接收离开好友约战房间回复
		//    IMRcvPKLeaveFriendRoomResponse (data);
		//    break;
		//case FiEventType.RECV_OTHER_LEAVE_FRIEND_ROOM_INFORM: //接收其他玩家来开好友约战房间通知
		//    IMRcvPKOtherLeaveFriendRoomInform (data);
		//    break;
		//case FiEventType.RECV_DISBAND_FRIEND_ROOM_RESPONSE: //房间收到解散房间反馈
		//    IMRcvPKDisbandFriendRoomResponse (data);
		//    break;
		//case FiEventType.RECV_DISBAND_FRIEND_ROOM_INFORM: //其他玩家收到房间解散的通知
		//    IMRcvPKDisbandFriendRoomInform (data);
		//    break;


		//case FiEventType.RECV_START_PK_GAME_RESPONSE: //接收房主开始PK游戏消息
		//    IMRcvPKStartGameResponse (data);
		//    break;
		//case FiEventType.RECV_START_PK_GAME_INFORM: //接收开始PK游戏通知
		//    IMRcvPKStartGameInform (data);
		//    break;


		//Fishing
		case FiEventType.RECV_FISHS_CREATED_INFORM: //收到生成鱼群消息
			IMRcvFishsCreatedInform (data);
			break;
		case FiEventType.RECV_OTHER_FIRE_BULLET_INFORM: //收到其他玩家发送子弹通知
			IMRcvOtherFireBulletInform (data);
			break;
		case FiEventType.RECV_HITTED_FISH_RESPONSE: //击中鱼反馈
			IMRcvHitFishResponse (data);
			break;
		case FiEventType.RECV_XL_GET_HONG_BAO_GOLD://红包
			break;
		case FiEventType.RECV_OTHER_ENTER_ROOM_INFORM: //收到其他玩家进入房间消息通知
			IMRcvOtherEnterRoomInform (data);
			break;
		case FiEventType.RECV_OTHER_LEAVE_ROOM_INFORM: //收到其他玩家离开房间消息通知
			IMRcvOtherLeaveRoomInform (data);
			break;
		//case FiEventType.RECV_USER_LEAVE_RESPONSE: //玩家自己离开房间
		//    IMRcvUserLeaveResponse (data);
		//    break;
		case FiEventType.RECV_FISH_OUT_RESPONSE: //鱼游出屏幕
			IMRcvFishOutResponse (data);
			break;
		case FiEventType.RECV_CHANGE_CANNON_RESPONSE: //改变炮等级
			IMRcvChangeCannonResponse (data);
			break;
		case FiEventType.RECV_OTHER_CHANGE_CANNON_INFORM: //其他人改变炮等级
			break;
		case FiEventType.RECV_USE_EFFECT_RESPONSE: //接收自己的特效信息
			IMRcvUserEffectResponse (data);
			break;
		}
	}

	/*private void RcvBackpackResponse(object data)
    {
        Tool.Log ("接收背包信息");
        FiBackpackResponse info = (FiBackpackResponse) data;
        if(0==info.result)
        {
            if (null != info.properties) {
                if (info.properties.Count > 0) {
                    foreach (FiBackpackProperty one in info.properties) {
                        Tool.Log ("id:" + one.id +
                        " name:" + one.name +
                        " type:" + one.type +
                        " description:" + one.description +
                        " useable:" + one.useable +
                        " canGiveAway:" + one.canGiveAway +
                        " diamondCost:" + one.diamondCost +
                        " count:" + one.count);
                        
                        backpackInfo.Add ( one );
                    }
                } else {
                    Tool.Log ("包裹为空");
                }

            } else {
                Tool.Log ("包裹 指针为空");
            }
        }

    //    UIHallMsg.GetInstance ().taskMsgHandle.SendTaskProcessRequest ();

    //    BackpackDataProvider nProvider = new BackpackDataProvider ();
    
        //Debug.LogError ("---------------Count ===== " + nProvider.GetProperty ().Count);

    }*/


	private void IMRcvPKCreateRoomResponse (object data)
	{
		//        public int  roomIndex;
		//        public int  goldType;
		//        public int  bulletType;
		//        public int  timeType;
		//        public int pointType;
		//        public int  playerNumType;
		//        public string roomName;
		//        public bool hasPassword;
		//        public bool begun;
		//        public int currentPlayerCount;
		FiCreatePKRoomResponse info = (FiCreatePKRoomResponse)data;


		Debug.LogError ("===================IMRcvPKCreateRoomResponse=====================>" + info.roomType);

		if (0 == info.result) {
			myInfo.seatIndex = info.seatIndex;

			roomInfo.roomIndex = info.info.roomIndex;
			roomInfo.roomType = info.roomType;
//            roomInfo.roomName = info.info.roomName;
//            roomInfo.hasPassword = info.info.hasPassword;
			roomInfo.currentPlayerCount = info.info.currentPlayerCount;
		} else {

		}
	}

	private void IMRcvPKEnterRoomResponse (object data)
	{
		FiEnterPKRoomResponse enterRoom = (FiEnterPKRoomResponse)data;
		Debug.LogError ("===================RcvPKEnterRoomResponse=====================>" + enterRoom.roomType + " / " + enterRoom.result);
		if (0 == enterRoom.result) {
			UIFishingMsg.GetInstance ().SetFishing (enterRoom.roomType);
			roomInfo.goldType = enterRoom.goldType;
			roomInfo.roomIndex = enterRoom.roomIndex;
			roomInfo.roomType = enterRoom.roomType;
			//roomInfo.roomOwnerID = enterRoom.roomOwnerId;
			myInfo.seatIndex = enterRoom.seatindex;

			//Tool.LogError ("IMRcvPKEnterRoomResponse my userID:"+myInfo.userID+" seatIndex:"+myInfo.seatIndex);
			foreach (FiUserInfo user in enterRoom.others) {
				//Tool.LogError ("IMRcvPKEnterRoomResponse userID:"+user.userId+" seatIndex:"+user.seatIndex);
				roomInfo.AddUser (new FiUserInfo (user));
			}

			GameObject Window = Resources.Load ("PkHall/PKWaitingWindow") as GameObject;
			GameObject nInstance = GameObject.Instantiate (Window);
			nInstance.GetComponentInChildren<UIPkWaiting> ().SetRoomInfo (enterRoom.roomType, enterRoom.goldType);
			nInstance.GetComponentInChildren<UIPkWaiting> ().RcvEnterRoomResponse (data);

//            if (enterRoom.roomType == 6) {
//                GameObject Window = Resources.Load ("Window/CombatWindow") as GameObject;
//                GameObject WindowClone = GameObject.Instantiate (Window);
//                WindowClone.GetComponentInChildren<UICombatSignUp> ().RcvEnterRoomResponse (data);
//            } else {
//                GameObject Window = Resources.Load ("Window/ReadyWindow") as GameObject;
//                GameObject nEntity = GameObject.Instantiate (Window);
//                nEntity.GetComponentInChildren<UISignUp> ().RcvEnterRoomResponse( data );
//            }
		} else {

		}
	}

	private void IMRcvPKOtherEnterRoomInform (object data)
	{
		FiOtherEnterPKRoomInform enterRoom = (FiOtherEnterPKRoomInform)data;
		Tool.LogError ("IMRcvPKOtherEnterRoomInform :" + enterRoom.ToString ());
		roomInfo.AddUser (new FiUserInfo (enterRoom.other));
	}

	private void IMRcvPKLeaveRoomResponse (object data)
	{
		FiLeavePKRoomResponse leaveRoom = (FiLeavePKRoomResponse)data;
		roomInfo.ClearUser ();
	}

	private void IMRcvPKOtherLeaveRoomInform (object data)
	{
		FiOtherLeavePKRoomInform leaveRoom = (FiOtherLeavePKRoomInform)data;
		Tool.Log ("IMRcvPKOtherLeaveRoomInform userID:" + leaveRoom.leaveUserId);
		//roomInfo.roomOwnerID = leaveRoom.roomOwnerUserId;
		roomInfo.RemoveUser (leaveRoom.leaveUserId);
	}

	private void IMRcvPKOtherPrepareGameInform (object data)
	{
		FiPreparePKGame prepareGame = (FiPreparePKGame)data;
	}

	private void IMRcvPKOtherCancelPrepareGameInform (object data)
	{
		FiCancelPreparePKGame cancelPrepareGame = (FiCancelPreparePKGame)data;
	}
        
	/*private void IMRcvPKEnterFriendRoomResponse(object data)
    {
        FiEnterFriendRoomResponse enterFriendRoom = (FiEnterFriendRoomResponse)data;
        if(0==enterFriendRoom.result)
        {
            myInfo.seatIndex = enterFriendRoom.seatIndex;
            roomInfo.SetRoomInfo (enterFriendRoom.room);
            foreach(FiUserInfo user in enterFriendRoom.others)
            {
                roomInfo.AddUser (user);
            }
        }
        else
        {

        }
    }*/

	private void IMRcvPKOtherEnterFriendRoomInform (object data)
	{
		FiOtherEnterFriendRoomInform otherEnter = (FiOtherEnterFriendRoomInform)data;
		roomInfo.AddUser (otherEnter.other);

	}



	private void IMRcvPKLeaveFriendRoomResponse (object data)
	{
		FiLeaveFriendRoomResponse leaveFriendRoom = (FiLeaveFriendRoomResponse)data;
		if (0 == leaveFriendRoom.result) {
			roomInfo.Clear ();
		} else {

		}
	}

	private void IMRcvPKOtherLeaveFriendRoomInform (object data)
	{
		FiOtherLeaveFriendRoomInform leaveFriendRoom = (FiOtherLeaveFriendRoomInform)data;
		roomInfo.RemoveUser (leaveFriendRoom.leaveUserId);

	}

	private void IMRcvPKDisbandFriendRoomResponse (object data)
	{
		FiDisbandFriendRoomResponse disbandFriendRoom = (FiDisbandFriendRoomResponse)data;
		roomInfo.Clear ();

	}

	private void IMRcvPKDisbandFriendRoomInform (object data)
	{
		FiDisbandFriendRoomInform disbandFriendRoom = (FiDisbandFriendRoomInform)data;
		roomInfo.Clear ();
	}

	/*private void IMRcvRoomMatchResponse(object data)
    {
        FiRoomMatchResponse roomMatchReply = (FiRoomMatchResponse) data;
        if(0==roomMatchReply.result)
        {
            Debug.Log ("我的座位号："+roomMatchReply.seatIndex);
            myInfo.seatIndex = roomMatchReply.seatIndex;
            roomInfo.InitUser (roomMatchReply.userArray);
            UIFishingMsg.GetInstance ().SetFishing (TypeFishing.CLASSIC);
            myInfo.lastGame.type = TypeFishing.CLASSIC;


        }
    }*/

	private void IMRcvPKStartGameResponse (object data)
	{
		FiStartPKGameResponse startGame = (FiStartPKGameResponse)data;

		//    Debug.LogError ("==========IMRcvPKStartGameResponse============>" + startGame.roomType );

		if (0 == startGame.result) {
            
			roomInfo.roomIndex = startGame.roomIndex;
			roomInfo.roomType = startGame.roomType;
			UIFishingMsg.GetInstance ().SetFishing (startGame.roomType);
			myInfo.lastGame.type = startGame.roomType;


        
		} else {

		}
	}

	private void IMRcvPKStartGameInform (object data)
	{
		FiStartPKGameInform startGame = (FiStartPKGameInform)data;
		roomInfo.roomType = startGame.roomType;

		//    Debug.LogError ("==========IMRcvPKStartGameInform============>" + startGame.roomType );

		UIFishingMsg.GetInstance ().SetFishing (startGame.roomType);
		myInfo.lastGame.type = startGame.roomType;

	}

	private void IMRcvOtherEnterRoomInform (object data)
	{
		FiOtherEnterRoom info = (FiOtherEnterRoom)data;
		if (null != info.user)
			roomInfo.AddUser (info.user);
	}

	private void IMRcvOtherLeaveRoomInform (object data)
	{
		FiOtherLeaveRoom info = (FiOtherLeaveRoom)data;
		roomInfo.RemoveUser (info.userId);
	}

	private void IMRcvFishsCreatedInform (object data)
	{
		FiFishsCreatedInform fishs = (FiFishsCreatedInform)data;
	}

	private void IMRcvOtherFireBulletInform (object data)
	{
		FiOtherFireBulletInform bullet = (FiOtherFireBulletInform)data;
	}

	private void IMRcvHitFishResponse (object data)
	{
		FiHitFishResponse info = (FiHitFishResponse)data;

		//Tool.OutLogWithToFile ("接收别人打到鱼 userId:"+info.userId+" bulletId:"+info.bulletId);

		if (myInfo.userID != info.userId) {
			//Debug.LogError ( "other hit fishing-------" );
			return;
		}

		if (null != info.propertyArray) {
			if (0 != info.propertyArray.Count) {
				//获取掉落
				foreach (FiProperty one in info.propertyArray) {
					//GOLD = 1, DIAMOND = 2, EXP(经验) = 3, FREEZE = 4, AIM(锁定) = 5, CALL(召唤) = 6,
					switch (one.type) {
					case FiPropertyType.GOLD:
						myInfo.gold += one.value;
                        //Debug.LogError ( "--------------------------------------------" );
						break;
					case FiPropertyType.DIAMOND:
						myInfo.diamond += one.value;
						break;
					case FiPropertyType.EXP:
						myInfo.experience += one.value;
						break;
					case FiPropertyType.FISHING_EFFECT_FREEZE: //冰冻
					case FiPropertyType.FISHING_EFFECT_AIM: //瞄准
					case FiPropertyType.FISHING_EFFECT_SUMMON: //召唤                        
						{
							if (null != backpackInfo) {
								FiBackpackProperty property = new FiBackpackProperty ();
								//        Debug.LogError ( "--------------recv property-----------------id:" + one.type + "/" +one.value );
								backpackInfo.Add (one.type, one.value);
							}
						}
						break;
					case FiPropertyType.TEST_GOLD:
//                        Debug.LogError ("TEST_GOLD = " + one.value);
						myInfo.testCoin += one.value;
						break;
					default:
						Debug.LogError("--------------------------------------------");
						int mGiveUnitId = one.type;
						if (mGiveUnitId >= FiPropertyType.TORPEDO_MINI && mGiveUnitId <= FiPropertyType.TORPEDO_NUCLEAR) {
							backpackInfo.Add (one.type, one.value);
						}
						break;
					}
				}
			}
		}
	}

	private void IMRcvUserLeaveResponse (object data)
	{
        
	}

	private void IMRcvFishOutResponse (object data)
	{
        
	}

	private void IMRcvChangeCannonResponse (object data)
	{//接收改变炮倍数
		FiChangeCannonMultipleResponse changeCannon = (FiChangeCannonMultipleResponse)data;
		int multiple = changeCannon.cannonMultiple;
		if (0 == changeCannon.result) {//改变炮倍数成功
			myInfo.cannonMultipleNow = multiple;
			if (multiple > myInfo.cannonMultipleMax)
				myInfo.cannonMultipleMax = multiple;
		} else {//改变炮倍数失败
		}
        
	}

	private void IMRcvUserEffectResponse (object data)
	{//接收自己特效
		FiEffectResponse effectInfo = (FiEffectResponse)data;
		FiEffectInfo info = effectInfo.info; 
		if (null == info)
			return;

		if (0 == effectInfo.result) {

//            Debug.LogError ("-------------id " + info.type + "--------------");
			BackpackInfo backpackInfo = (BackpackInfo)Facade.GetFacade ().data.Get (FacadeConfig.BACKPACK_MODULE_ID);
			backpackInfo.Delete (info.type, 1);

		} else {
			Tool.OutLogWithToFile ("接收自己特效 特效失敗 effectType:" + info.type);

		}
        
	}

}

