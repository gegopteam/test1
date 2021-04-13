using UnityEngine;
using System.Collections;

using AssemblyCSharp;

public class UIHallMsg : IDispatch
{
	//cj17/8/18
	//public delegate void ToolDelegate();
	//public event ToolDelegate ToolEvent;
	//private GameObject giftWindow;
	//cj

	//public int currentDay = 0;

	private static UIHallMsg instance = null;

	public static UIHallMsg GetInstance ()
	{
		if (null == instance) {
			instance = new UIHallMsg ();
		}

		return instance;
	}

	public static void DestroyInstance ()
	{
		if (null != instance) {
			instance = null;
		}
	}

	private DispatchControl dispatchControl = null;
	private DataControl dataControl = null;
	private RoomInfo roomInfo = null;
	private MyInfo myInfo = null;

	private BackpackInfo backpackInfo = null;

	private UIHallObjects hallObjects = null;

	private EventControl mEventCtrl;

	/*private FriendMsgHandle      mFriendHandle = new FriendMsgHandle();
	private RedPacketMsgHandle   mRoomMsgHandle = new RedPacketMsgHandle();
//	private TaskMsgHandle        mTaskMsgHandle = new TaskMsgHandle();
	private PurchaseMsgHandle    mPurchageHandle = new  PurchaseMsgHandle();
	private StartGiftMsgHandle   mStartHandle = new StartGiftMsgHandle();

	public FriendMsgHandle friendMsgHandle 
	{
		get { return mFriendHandle; }
	}

	public RedPacketMsgHandle redPacketRoomMsgHandle 
	{
		get { return mRoomMsgHandle; }
	}

//	public TaskMsgHandle taskMsgHandle 
//	{
//		get { return mTaskMsgHandle; }
//	}

	public PurchaseMsgHandle purchageHandle 
	{
		get { return mPurchageHandle; }
	}

	public StartGiftMsgHandle startGiftMsgHandle
	{
		get{return mStartHandle;}
	}*/

	private UIHallMsg ()
	{
		dispatchControl = DispatchControl.GetInstance ();
		dispatchControl.AddRcv (AppFun.HALL, this);
		dataControl = DataControl.GetInstance ();
		roomInfo = dataControl.GetRoomInfo ();
		myInfo = dataControl.GetMyInfo ();
		backpackInfo = dataControl.GetBackpackInfo ();

		hallObjects = UIHallObjects.GetInstance ();

		addEventHandler ();
	}



	private void addEventHandler ()
	{
		//mFriendHandle.AddMsgEventHandle ();
		//mRoomMsgHandle.AddMsgEventHandle ();
//		mTaskMsgHandle.AddMsgEventHandle ();//
		//mPurchageHandle.AddMsgEventHandle ();

		mEventCtrl = EventControl.instance ();
	

		mEventCtrl.addEventHandler (FiEventType.RECV_LEVEL_UP_INFORM, RecvLevelUpInform);
	}

	private void RecvLevelUpInform (object data)
	{
		FiLevelUpInfrom nResponse = (FiLevelUpInfrom)data;

		if (nResponse.userId != myInfo.userID) {
			if (PrefabManager._instance == null) {
				UnityEngine.Debug.LogError ("Error! PrefabManager=null");
				return;
			}
                
			GunControl otherGun = PrefabManager._instance.GetGunByUserID (nResponse.userId);
			if (otherGun != null) {
				otherGun.level = nResponse.level;
			}
			return;
		}
            
		myInfo.experience = nResponse.experience;
		myInfo.level = nResponse.level;
		PrefabManager._instance.GetLocalGun ().level = nResponse.level;
		myInfo.nextLevelExp = nResponse.nextLevelExp;

		int diamondReturn = 0;
		int lockReturn = 0;
		int freezeReturn = 0;

		foreach (FiProperty nProperty in nResponse.properties) {
			switch (nProperty.type) {
			case FiPropertyType.GOLD:
				myInfo.gold += nProperty.value;
				Debug.LogError ("如果这条log出现了，说明升级奖励了错误的东西");
				break;
			case FiPropertyType.DIAMOND:
				myInfo.diamond += nProperty.value;
				diamondReturn = nProperty.value;
				break;
			default:				
				backpackInfo.Add (nProperty.type, nProperty.value);
				if (nProperty.type == FiPropertyType.FISHING_EFFECT_FREEZE) {
					freezeReturn = nProperty.value;
				} else if (nProperty.type == FiPropertyType.FISHING_EFFECT_AIM) {
					lockReturn = nProperty.value;
				}

				break;
			}
		}
		Debug.LogError ("--------------recv level up-----------------" + myInfo.level);
		if (PrefabManager._instance != null) {
			PrefabManager._instance.ShowLevelUpPanel (myInfo.level, diamondReturn, lockReturn, freezeReturn);
		}
	}



	//启航礼包通知
	/*public void RcvStartGameInform( int nDayOffset )
	{
		if (myInfo.bDisplayedStartGift) {
			return;
		}
		myInfo.bDisplayedStartGift = true;

		GameObject window = Resources.Load ("Window/SetSailGift")as GameObject;
		GameObject.Instantiate (window);
	}*/

	/*public void RcvGetStartGiftResponse( object data )
	{
		FiGetStartGiftResponse nResponse = (FiGetStartGiftResponse)data;
		UnityEngine.Debug.Log ("nResponse"+nResponse.result);

		if (nResponse.result == 0) {
			//if (ToolEvent != null) {
			//	ToolEvent ();
			//}
		}
		/*if (nResponse.result == 0) {//表示成功,道具飞向背包，则将当前窗口删除，所以关闭窗口成功发送一个事件
			
		} else {
			Debug.Log ("关闭领取窗口失败");
		}
	}*/



	~UIHallMsg ()
	{
		
	}

	public void OnRcv (int type, object data)
	{
		switch (type) {
		//	case FiEventType.RECV_ROOM_MATCH_RESPONSE:
		//RcvRoomMatchResponse(data);
		//		break;
		//	case FiEventType.RECV_BACKPACK_RESPONSE:
		//		RcvBackpackResponse(data);
		//		break;

		//HallPK
		//case FiEventType.RECV_HAVE_DISCONNECTED_ROOM_INFORM: //有断开连接的Fishing游戏通知
		//	RcvHaveDisconnectedRoomInform (data);
		//	break;

		case FiEventType.RECV_ENTER_PK_ROOM_RESPONSE: //进入PK准备房间消息回复
		case FiEventType.RECV_OTHER_ENTER_PK_ROOM_INFORM: //其他人进入PK准备房间通知
		case FiEventType.RECV_LEAVE_PK_ROOM_RESPONSE: //离开PK准备房间消息回复
		case FiEventType.RECV_OTHER_LEAVE_PK_ROOM_INFORM: //其他人离开PK准备房间消息通知

		///case FiEventType.RECV_OTHER_PREPARE_PKGAME_INFORM: //接收其他玩家准备通知
		//case FiEventType.RECV_OTHER_CANCEL_PREPARE_PKGAME_INFORM: //接收其他玩家取消准备通知

		//case FiEventType.RECV_CREATE_FRIEND_ROOM_RESPONSE: //接收创建好友约战房间回复
		//case FiEventType.RECV_ENTER_FRIEND_ROOM_RESPONSE: //接收进入好友约战房间回复
		//case FiEventType.RECV_OTHER_ENTER_FRIEND_ROOM_INFORM: //接收其他玩家进入好友约战房间通知
		//case FiEventType.RECV_LEAVE_FRIEND_ROOM_RESPONSE: //接收离开好友约战房间回复
		//case FiEventType.RECV_OTHER_LEAVE_FRIEND_ROOM_INFORM: //接收其他玩家来开好友约战房间通知
		//case FiEventType.RECV_DISBAND_FRIEND_ROOM_RESPONSE: //房间收到解散房间反馈
		//case FiEventType.RECV_DISBAND_FRIEND_ROOM_INFORM: //其他玩家收到房间解散的通知

		//case FiEventType.RECV_START_PK_GAME_RESPONSE: //接收房主开始PK游戏消息
		//case FiEventType.RECV_START_PK_GAME_INFORM: //接收开始PK游戏通知
			RcvToPKRoom (type, data);
			break;

		}
	}

	/*private void RcvRoomMatchResponse(object data)
	{
		FiRoomMatchResponse roomMatchReply = (FiRoomMatchResponse) data;
		if(0==roomMatchReply.result)
		{
			//Debug.Log ("房间匹配成功");
			Tool.OutLogWithToFile ("Hall 房间匹配成功");
		}
		else
		{//非零房间匹配失败
			//先提示房间匹配失败
			//Debug.Log ("房间匹配失败");
			Tool.OutLogWithToFile ("Hall 房间匹配失败");
			return;
		}
	}*/

	/*public void SndGiveOtherPropertyRequest( int nUserId , int nPropType, int nPropValue )
	{
		FiGiveOtherPropertyRequest nRequest = new FiGiveOtherPropertyRequest ();
		nRequest.userId = nUserId;
		nRequest.property.type = nPropType;
		nRequest.property.value = nPropValue;
		dataControl.PushSocketSndByte ( FiEventType.SEND_GIVE_OTHER_PROPERTY_REQUEST , nRequest.serialize() );
	}*/

	//	public void SndEveryDayTaskRequest( int nTaskId )
	//	{
	//		FiEveryDayActivityRequest nRequest = new FiEveryDayActivityRequest ();
	//		nRequest.taskId = nTaskId;
	//		dataControl.PushSocketSndByte ( FiEventType.SEND_EVERYDAY_TASK_REQUEST , nRequest.serialize() );
	//	}

	public void SndStartGiftRequest (int nDayOffset)
	{
		Debug.LogError ("SndStartGiftRequest---------------->");
		//giftWindow = window;
		FiGetStartGiftRequest nRequest = new FiGetStartGiftRequest ();
		nRequest.dayOffset = nDayOffset;
//		dataControl.PushSocketSnd (FiEventType.SEND_GET_START_GIFT_REQUEST, nRequest );
		dataControl.PushSocketSndByte (FiProtoType.FISHING_GET_START_GIFT_REQUEST, nRequest.serialize ());
	}

	public void SndEnterRedPacketRoomRequest (int nType)
	{
		FiEnterRedPacketRoomRequest nRequest = new FiEnterRedPacketRoomRequest ();
		nRequest.roomType = nType;
		dataControl.PushSocketSnd (FiEventType.SEND_ENTER_RED_PACKET_ROOM_REQUEST, nRequest);
	}

	//	public void SndBackpackRequest()
	//	{//发送背包数据请求
	//		dataControl.PushSocketSnd (FiEventType.SEND_BACKPACK_REQUEST, null);
	//	}

	/*	private void RcvBackpackResponse(object data)
	{
		FiBackpackResponse info = (FiBackpackResponse) data;
		if(0==info.result)
		{
//			object tmp;
//			backpackInfo.SetRcv ();
//			backpackInfo.OpenRcvInfo ();
		}
	}*/
		
	public void SndCreatePKRoomRequest (int roomType, int goldType, int bulletType, int timeType, int playerNumType, string roomName, string roomPassword = "")
	{
		FiCreatePKRoomRequest createPKRoom = new FiCreatePKRoomRequest ();
		createPKRoom.roomType = roomType;
		createPKRoom.goldType = goldType;
		createPKRoom.bulletType = bulletType;
		createPKRoom.timeType = timeType;
		createPKRoom.playerNumType = playerNumType;
		createPKRoom.roomName = roomName;
		createPKRoom.roomPassword = roomPassword;

		Debug.Log ("CreatePK:" + roomPassword);

		dataControl.PushSocketSnd (FiEventType.SEND_CREATE_PK_ROOM_REQUEST, createPKRoom);
	}

	private void RcvCreatePKRoomResponse (object data)
	{
		FiCreatePKRoomResponse info = (FiCreatePKRoomResponse)data;
		Tool.Log ("RcvCreatePKRoomResponse 000");
		if (0 == info.result) {
			hallObjects.CreatePKRoom (info);
		} else {
			
		}
	}

	/*public void SndGetPKRoomsRequest(int roomType, int pageNum)
	{
		Tool.OutLogWithToFile ("发送房间列表请求 roomType:"+roomType+" pageNum:"+pageNum);
		FiGetPkRoomListRequest pkRoomList = new FiGetPkRoomListRequest ();
		pkRoomList.roomType = roomType;
		pkRoomList.pageNum = pageNum;
		dataControl.PushSocketSnd (FiEventType.SEND_GET_PK_ROOMS_REQUEST, pkRoomList);
	}*/

	/*private void RcvGetPKRoomsResponse(object data)
	{
		FiGetPkRoomListResponse pkRoomList = (FiGetPkRoomListResponse) data;

		if(0==pkRoomList.result)
		{
			//pkRoomList.roomType
		}
		else
		{
			//获取房间失败
		}
	}*/

	private void RcvToPKRoom (int type, object data)
	{
		if (null == hallObjects)
			return;

//		if(FiEventType.RECV_START_PK_GAME_RESPONSE==type)
//		{
//			FiStartPKGameResponse info = (FiStartPKGameResponse)data;
//			Debug.LogError ("RECV_START_PK_GAME_RESPONSE");
//			Debug.LogError (info.ToString ());
//		}
//		else if (FiEventType.RECV_START_PK_GAME_INFORM==type)
//		{
//			FiStartPKGameInform info = (FiStartPKGameInform)data;
//			Debug.LogError ("RECV_START_PK_GAME_INFORM");
//			Debug.LogError (info.ToString ());
//		}

		
		hallObjects.ToPKRoom (type, data);
	}

	//发送进入渔场的操作，那么场景10s内  禁止场景切换功能（之后优化）
	public void SndRoomMatchRequest (int roomType, int roomMultiple)
	{
        Debug.Log("   修改砲倍數傳送給後端 roomType = " + roomType);
        Debug.Log("   修改砲倍數傳送給後端 roomMultiple = " + roomMultiple);
        //		GameObject Window1 = UnityEngine.Resources.Load ("MainHall/Common/WaitingView")as UnityEngine.GameObject;
        //		GameObject mWaitingView = UnityEngine.GameObject.Instantiate (Window1);
        //		mWaitingView.GetComponentInChildren<UIWaiting> ().HideBackGround ();
        LoginUtil.GetIntance ().ShowWaitingView (true);
		MyInfo nInfo =	(MyInfo)Facade.GetFacade ().data.Get (FacadeConfig.USERINFO_MODULE_ID);
		nInfo.lockScene = true;
		FiRoomMatchRequest roomMatchRequest = new FiRoomMatchRequest ();
		roomMatchRequest.enterType = roomType;
		roomMatchRequest.roomMultiple = roomMultiple;
		
		if (null != roomInfo) {
			roomInfo.roomType = roomType;
			roomInfo.roomMultiple = roomMultiple;
		}
		dataControl.PushSocketSnd (FiEventType.SEND_ROOM_MATCH_REQUEST, roomMatchRequest);
	}

	
	public void SndPKStartGameRequest (int roomType, int roomIndex)
	{
		FiStartPKGameRequest startGame = new FiStartPKGameRequest ();
		startGame.roomType = roomType;
		startGame.roomIndex = roomIndex;

		dataControl.PushSocketSnd (FiEventType.SEND_START_PK_GAME_REQUEST, startGame);
	}

	//	public const int RECV_START_PK_GAME_RESPONSE         = 200012;
	//	public class  FiStartPKGameResponse
	//	{
	//		public int  result;
	//		public int  roomType;
	//		public int  roomIndex;
	//	}
	//
	private void RcvPKStartGameResponse (object data)
	{
		FiStartPKGameResponse startGame = (FiStartPKGameResponse)data;

	}

	//	public const int RECV_START_PK_GAME_INFORM     = 300012;

	//	public class  FiStartPKGameInform
	//	{
	//		public int  roomType;
	//		public int  roomIndex;
	//	}
	//
	//
	private void RcvPKStartGameInform (object data)
	{
		FiStartPKGameInform startGame = (FiStartPKGameInform)data;
	
	}

	//	public const int SEND_ENTER_PK_ROOM_REQUEST          = 100013;
	//	public class  FiEnterPKRoomRequest
	//	{
	//		public int     roomType;
	//		public int     roomIndex;
	//		public string  roomPassword;
	//	}
	//
	/*public void SndPKEnterRoomRequest(int roomType, int goldType)
	{//
		FiEnterPKRoomRequest enterRoom = new FiEnterPKRoomRequest ();
		enterRoom.roomType = roomType;
		enterRoom.goldType = goldType;
//		enterRoom.roomIndex = roomIndex;
//		enterRoom.roomPassword = password;
//		enterRoom.createTime = createTime;

		dataControl.PushSocketSnd (FiEventType.SEND_ENTER_PK_ROOM_REQUEST, enterRoom);

		myInfo.lastGame.level = goldType;
	}*/

	//	public const int RECV_ENTER_PK_ROOM_RESPONSE         = 200013;
	//	public class   FiEnterPKRoomResponse
	//	{
	//		public int result;
	//		public int roomIndex;
	//		public int seatindex;
	//		public int roomOwnerId;
	//		public List<FiUserInfo> others;
	//	}
	//
	public void RcvPKEnterRoomResponse (object data)
	{
		FiEnterPKRoomResponse enterRoom = (FiEnterPKRoomResponse)data;
	}

	//	public const int RECV_OTHER_ENTER_PK_ROOM_INFORM     = 300013;
	//	public class   FiOtherEnterPKRoomInform
	//	{
	//		public int  enterType;
	//		public long roomIndex;
	//		public FiUserInfo other;
	//	}
	//
	public void RcvPKOtherEnterRoomInform (object data)
	{
		FiOtherEnterPKRoomInform enterRoom = (FiOtherEnterPKRoomInform)data;
	}

	// SEND_LEAVE_PK_ROOM_REQUEST
	//	public class   FiLeavePKRoomRequest
	//	{
	//		public int roomType;
	//		public int roomIndex;
	//	}
	

	/*public void SndPKLeaveRoomRequest(int roomType, int roomIndex, int goldType)
	{
		FiLeavePKRoomRequest leaveRoom = new FiLeavePKRoomRequest ();
		leaveRoom.roomType = roomType;
		leaveRoom.roomIndex = roomIndex;
		leaveRoom.goldType = goldType;
		dataControl.PushSocketSnd (FiEventType.SEND_LEAVE_PK_ROOM_REQUEST, leaveRoom);
		Debug.LogError ("SndPKLeaveRoomRequest:"+leaveRoom.ToString());
	}*/
		
	public void RcvPKLeaveRoomResponse (object data)
	{
		FiLeavePKRoomResponse leaveRoom = (FiLeavePKRoomResponse)data;
	}

	public void RcvPKOtherLeaveRoomInform (object data)
	{
		FiOtherLeavePKRoomInform leaveRoom = (FiOtherLeavePKRoomInform)data;
	}

	public void SndPKPrepareGameRequest (int userID, int roomType, int roomIndex)
	{
		FiPreparePKGame prepareGame = new FiPreparePKGame ();
		prepareGame.userId = userID;
		prepareGame.roomType = roomType;
		prepareGame.roomIndex = roomIndex;
		dataControl.PushSocketSnd (FiEventType.SEND_PREPARE_PKGAME_REQUEST, prepareGame);
		Debug.LogError ("SndPKPrepareGameRequest:" + prepareGame.ToString ());
	}

	public void SndPKCancelPrepareGame (int userID, int roomType, int roomIndex)
	{
		FiCancelPreparePKGame cancelPrepareGame = new FiCancelPreparePKGame ();
		cancelPrepareGame.userId = userID;
		cancelPrepareGame.roomType = roomType;
		cancelPrepareGame.roomIndex = roomIndex;
		dataControl.PushSocketSnd (FiEventType.SEND_CANCEL_PREPARE_PKGAME, cancelPrepareGame);
		Debug.LogError ("SndPKCancelPrepareGame:" + cancelPrepareGame.ToString ());
	}




	public void SndPKCreateFriendRoomRequest (int roomType, int goldType, int timeType, int roundType)
	{
		Debug.LogError ("SndPKCreateFriendRoomRequest roomType:" + roomType + "goldType:" + goldType + "timeType:" + timeType + "roundType:" + roundType);
		FiCreateFriendRoomRequest createFriendRoom = new FiCreateFriendRoomRequest ();
		createFriendRoom.roomType = roomType;
		createFriendRoom.goldType = goldType;
		createFriendRoom.timeType = timeType;
		createFriendRoom.roundType = roundType;
		myInfo.lastGame.level = goldType;
		myInfo.lastGame.type = roomType;
		dataControl.PushSocketSnd (FiEventType.SEND_CREATE_FRIEND_ROOM_REQUEST, createFriendRoom);
	}

	public void SndPKEnterFriendRoomRequest (int roomIndex)
	{
		Debug.LogError ("SndPKEnterFriendRoomRequest roomIndex:" + roomIndex);
		FiEnterFriendRoomRequest enterFriendRoom = new FiEnterFriendRoomRequest ();
		enterFriendRoom.roomIndex = roomIndex;

		dataControl.PushSocketSnd (FiEventType.SEND_ENTER_FRIEND_ROOM_REQUEST, enterFriendRoom);
	}

	public void SndPKLeaveFriendRoomRequest (int roomType, int roomIndex)
	{
		Debug.LogError ("SndPKLeaveFriendRoomRequest roomIndex:" + roomIndex + "roomType:" + roomType);
		FiLeaveFriendRoomRequest leaveFriendRoom = new FiLeaveFriendRoomRequest ();
		leaveFriendRoom.roomType = roomType;
		leaveFriendRoom.roomIndex = roomIndex;

		dataControl.PushSocketSnd (FiEventType.SEND_LEAVE_FRIEND_ROOM_REQUEST, leaveFriendRoom);
	}

	//---
	//	case FiEventType.RECV_CREATE_FRIEND_ROOM_RESPONSE: //接收创建好友约战房间回复
	//	RcvPKCreateFriendRoomResponse (data);
	//	break;
	//	case FiEventType.RECV_ENTER_FRIEND_ROOM_RESPONSE: //接收进入好友约战房间回复
	//	RcvPKEnterFriendRoomResponse (data);
	//	break;
	//	case FiEventType.RECV_OTHER_ENTER_FRIEND_ROOM_INFORM: //接收其他玩家进入好友约战房间通知
	//	RcvPKOtherEnterFriendRoomInform (data);
	//	break;
	//	case FiEventType.RECV_LEAVE_FRIEND_ROOM_RESPONSE: //接收离开好友约战房间回复
	//	RcvPKLeaveFriendRoomResponse (data);
	//	break;
	//	case FiEventType.RECV_OTHER_LEAVE_FRIEND_ROOM_INFORM: //接收其他玩家来开好友约战房间通知
	//	RcvPKOtherLeaveFriendRoomInform (data);
	//	break;
	private void RcvPKCreateFriendRoomResponse (object data)
	{
		FiCreateFriendRoomResponse createFriendRoom = (FiCreateFriendRoomResponse)data;
		if (0 == createFriendRoom.result) {

		} else {

		}

	}

	private void RcvPKEnterFriendRoomResponse (object data)
	{
		FiEnterFriendRoomResponse enterFriendRoom = (FiEnterFriendRoomResponse)data;
		if (0 == enterFriendRoom.result) {

		} else {

		}
	}

	private void RcvPKOtherEnterFriendRoomInform (object data)
	{
		FiOtherEnterFriendRoomInform otherEnter = (FiOtherEnterFriendRoomInform)data;
	

	}

	private void RcvPKLeaveFriendRoomResponse (object data)
	{
		FiLeaveFriendRoomResponse leaveFriendRoom = (FiLeaveFriendRoomResponse)data;
		if (0 == leaveFriendRoom.result) {

		} else {

		}
	}

	private void RcvPKOtherLeaveFriendRoomInform (object data)
	{
		FiOtherLeaveFriendRoomInform leaveFriendRoom = (FiOtherLeaveFriendRoomInform)data;

	}

	//	//拥有断线链接的房间消息
	//	public const int RECV_HAVE_DISCONNECTED_ROOM_INFORM	     = 300038;

	private void RcvHaveDisconnectedRoomInform (object data)
	{

	}

	//SEND_RECONNECT_GAME_REQUEST
	public void SndReconnectGameRequest ()
	{
		dataControl.PushSocketSnd (FiEventType.SEND_RECONNECT_GAME_REQUEST, null);
	}

	//	public class     FiDisbandFriendRoomRequest
	//	{
	//		public int    roomType;
	//		public int    roomIndex;
	//	}
	//	//房主发送解散房间申请
	//	public const int SEND_DISBAND_FRIEND_ROOM_REQUEST	     = 100039;
	public void SndPKDisbandFriendRoomRequest (int roomType, int roomIndex)
	{
		Debug.LogError ("");
		FiDisbandFriendRoomRequest disbandRoom = new FiDisbandFriendRoomRequest ();
		disbandRoom.roomType = roomType;
		disbandRoom.roomIndex = roomIndex;

		dataControl.PushSocketSnd (FiEventType.SEND_DISBAND_FRIEND_ROOM_REQUEST, disbandRoom);
	}



	//	case FiEventType.RECV_DISBAND_FRIEND_ROOM_RESPONSE: //房间收到解散房间反馈
	//	case FiEventType.RECV_DISBAND_FRIEND_ROOM_INFORM: //其他玩家收到房间解散的通知
	private void RcvPKDisbandFriendRoomResponse (object data)
	{
		FiDisbandFriendRoomResponse disbandFriendRoom = (FiDisbandFriendRoomResponse)data;
		
	}

	private void RcvPKDisbandFriendRoomInform (object data)
	{
		FiDisbandFriendRoomInform disbandFriendRoom = (FiDisbandFriendRoomInform)data;

	}

}