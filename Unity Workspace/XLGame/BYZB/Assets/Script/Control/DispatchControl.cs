/* author:KinSen
 * Date:2017.05.26
 */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using AssemblyCSharp;

//负责数据的分发
public class DispatchControl
{
	private static DispatchControl instance = null;

	public static DispatchControl GetInstance ()
	{
		if (null == instance) {
			instance = new DispatchControl ();
		}

		return instance;
	}

	public static void DestroyInstance ()
	{
		if (null != instance) {
			instance = null;
		}
	}

	private IDispatch infoMsg = null;
	private IDispatch uiLoginMsg = null;
	private IDispatch uiHallMsg = null;
	private IDispatch uiFishingMsg = null;
	private DataControl dataControl = null;
	private EventControl mEventCtrl;
	private MyInfo myInfo;
	//	private MyInfo myInfo = null;
	//	private RoomInfo roomInfo = null;
	private int connectCount = 0;

	private DispatchControl ()
	{
		dataControl = DataControl.GetInstance ();
		mEventCtrl = EventControl.instance ();
		myInfo = DataControl.GetInstance().GetMyInfo();
//		myInfo = dataControl.GetMyInfo ();
//		roomInfo = dataControl.GetRoomInfo ();
	}

	~DispatchControl ()
	{
		infoMsg = null;
		uiLoginMsg = null;
		uiHallMsg = null;
		uiFishingMsg = null;
	}

	//设置数据接收者
	public void AddRcv (AppFun type, IDispatch obj)
	{
		SetRcv (type, obj);
	}

	public void DeleteRcv (AppFun type)
	{
		SetRcv (type, null);
	}

	//消息接收按照功能模块设置分发，具体数据该给哪个对象由功能模块内部自行决定
	void SetRcv (AppFun type, IDispatch obj)
	{
		switch (type) {
		case AppFun.DATA:
			infoMsg = obj;
			break;
		case AppFun.LOGIN:
			uiLoginMsg = obj;
			break;
		case AppFun.HALL:
			uiHallMsg = obj;
			break;
		case AppFun.FISHING:
			uiFishingMsg = obj;
			break;
		}
	}

	public void Dispatch ()
	{
		DispatchData data = dataControl.GetSocketRcv ();
		if (null == data)
			return;
		OnData (data.type, data.data);
		mEventCtrl.dispatch (data);
		//Debug.Log ("Dispatch data ...");
	}

	void OnData (int type, object data)
	{
		if (null == data)
			return;
		ToData (type, data);
		ToUI (type, data);
	}

	void ToData (int type, object data)
	{
		if (null != infoMsg)
			infoMsg.OnRcv (type, data);
	}

	//数据分发
	void ToUI (int type, object data)
	{
		switch (type) {
		//Connect
		case FiEventType.CONNECT_START:
			{
				/*if(0!=connectCount)
				{
					string tip = "开始重新第 "+connectCount+"/7 连接网络。。。";
					Tool.LogError (tip);
					UIOtherObjects.GetInstance ().ShowLineTip (tip);
				}*/
				//	connectCount++;
			}
			break;
		case FiEventType.CONNECT_SUCCESS:
			Tool.LogError ("網絡連接成功。。。");
			connectCount = 1;
			break;
		case FiEventType.CONNECT_SUCCESS_WITHLOGIN:
			Tool.LogError("網絡連接成功但是不登入。。。");
			connectCount = 1;
			break;
		case FiEventType.CONNECT_FAIL:
		    Tool.LogError ("網絡連接失敗。。。");
		    UIOtherObjects.GetInstance ().ShowLineTip ("網絡連接失敗。。。");
		break;
		case FiEventType.CONNECTIONT_CLOSED:
			myInfo.isconnecting = false;
			Tool.LogError ("網絡連接已關閉。。。");
			break;
		case FiEventType.CONNECT_COUNT_OUT:
			Tool.LogError ("超出網絡重連次數，請檢查您的網絡！");
			UIOtherObjects.GetInstance ().ShowLineTip ("");
			UIOtherObjects.GetInstance ().ShowNowNoWifi ("超出網絡重連次數，請檢查您的網絡！");
			break;
		case FiEventType.CONNECT_TIMEROUT:
			FiNetworkInfo nNetInfo = (FiNetworkInfo)data;
			connectCount = nNetInfo.nConnectCount;
			Tool.LogError ("網絡連接超時。。。" + connectCount);
			UIOtherObjects.GetInstance ().ShowLineTip ("網絡連接超時。。。");
			break;

//		case FiEventType.RECV_MESSAGE:
//			break;
//		case FiEventType.SEND_MESSAGE:
//			break;


		//Login
		case FiEventType.RECV_LOGIN_RESPONSE:
			ToUILogin (type, data);
			break;


		//Hall
		case FiEventType.RECV_BACKPACK_RESPONSE:
		case FiEventType.RECV_HAVE_DISCONNECTED_ROOM_INFORM: //有断开连接的Fishing游戏通知
		//HallPK
		//注释的消息作废
//		case FiEventType.RECV_GET_PK_ROOMS_RESPONSE: //接收获取PK房间列表信息
//		case FiEventType.RECV_CREATE_PK_ROOM_RESPONSE: //接收创建PK房消息
		case FiEventType.RECV_ENTER_PK_ROOM_RESPONSE: //进入PK准备房间消息回复
		case FiEventType.RECV_OTHER_ENTER_PK_ROOM_INFORM: //其他人进入PK准备房间通知
//		case FiEventType.RECV_LEAVE_PK_ROOM_RESPONSE: //离开PK准备房间消息回复
		case FiEventType.RECV_OTHER_LEAVE_PK_ROOM_INFORM: //其他人离开PK准备房间消息通知

		//case FiEventType.RECV_OTHER_PREPARE_PKGAME_INFORM: //接收其他玩家准备通知
		//case FiEventType.RECV_OTHER_CANCEL_PREPARE_PKGAME_INFORM: //接收其他玩家取消准备通知

		//case FiEventType.RECV_CREATE_FRIEND_ROOM_RESPONSE: //接收创建好友约战房间回复
		//case FiEventType.RECV_ENTER_FRIEND_ROOM_RESPONSE: //接收进入好友约战房间回复
		//case FiEventType.RECV_OTHER_ENTER_FRIEND_ROOM_INFORM: //接收其他玩家进入好友约战房间通知
		//case FiEventType.RECV_LEAVE_FRIEND_ROOM_RESPONSE: //接收离开好友约战房间回复
		//case FiEventType.RECV_OTHER_LEAVE_FRIEND_ROOM_INFORM: //接收其他玩家来开好友约战房间通知
		//case FiEventType.RECV_DISBAND_FRIEND_ROOM_RESPONSE: //房间收到解散房间反馈
		//case FiEventType.RECV_DISBAND_FRIEND_ROOM_INFORM: //其他玩家收到房间解散的通知
			ToUIHall (type, data);
			break;


		//Hall & Fishing
		//case FiEventType.RECV_ROOM_MATCH_RESPONSE: //用户自己进(打鱼)房间
		//	ToUIFishing (type, data);
		//	ToUIHall (type, data);
		//	break;
		//Hall & FishingPK
		//case FiEventType.RECV_START_PK_GAME_RESPONSE: //接收房主开始PK游戏消息
		//case FiEventType.RECV_START_PK_GAME_INFORM: //接收开始PK游戏通知
			ToUIHall (type, data);
			ToUIFishing (type, data);
			break;
		case FiEventType.RECV_LEAVE_PK_ROOM_RESPONSE:
			ToUIHall (type, data);
			ToUIFishing (type, data);
			break;


		//Fishing
		case FiEventType.RECV_FISHS_CREATED_INFORM: //收到生成鱼群消息
		case FiEventType.RECV_OTHER_FIRE_BULLET_INFORM: //收到其他玩家发送子弹通知
		case FiEventType.RECV_HITTED_FISH_RESPONSE: //玩家击中鱼反馈
		case FiEventType.RECV_XL_GET_HONG_BAO_GOLD: //天将横财
		case FiEventType.RECV_OTHER_ENTER_ROOM_INFORM: //收到其他玩家进入房间消息通知
		case FiEventType.RECV_OTHER_LEAVE_ROOM_INFORM: //收到其他玩家离开房间消息通知
		case FiEventType.RECV_USER_LEAVE_RESPONSE: //玩家自己离开房间
		case FiEventType.RECV_FISH_OUT_RESPONSE: //鱼游出屏幕
		case FiEventType.RECV_CHANGE_CANNON_RESPONSE: //改变炮等级
		case FiEventType.RECV_OTHER_CHANGE_CANNON_INFORM: //其他人改变炮等级
		case FiEventType.RECV_USE_EFFECT_RESPONSE: //接收自己的特效信息
		case FiEventType.RECV_OTHER_EFFECT_INFORM: //接收其他玩家的特效信息
		case FiEventType.RECV_FISH_FREEZE_TIMEOUT_INFORM: //接收冰冻时间结束
		//case FiEventType.RECV_LAUNCH_TORPEDO_RESPONSE: //接收发鱼雷回复
		//case FiEventType.RECV_OTHER_LAUNCH_TORPEDO_INFORM: //接收其他玩家发鱼雷回复
		//case FiEventType.RECV_TORPEDO_EXPLODE_RESPONSE: //接收鱼雷爆炸
		//case FiEventType.RECV_OTHER_TORPEDO_EXPLODE_INFORM: //接收其他玩家鱼雷爆炸通知

		//FishingPK
//		case FiEventType.RECV_LEAVE_PK_ROOM_RESPONSE:
		case FiEventType.RECV_PRE_PKGAME_COUNTDOWN_INFORM: //游戏开始前的倒计时
		case FiEventType.RECV_PKGAME_COUNTDOWN_INFORM: //游戏倒计时
		case FiEventType.RECV_PK_USE_EFFECT_RESPONSE: //接收自己发的道具
		case FiEventType.RECV_PK_OTHER_EFFECT_INFORM: //接收其他玩家发的道具通知
		case FiEventType.RECV_PK_DISTRIBUTE_PROPERTY_INFORM: //接收初次进PK房下发的道具
		//case FiEventType.RECV_PK_LAUNCH_TORPEDO_RESPONSE: //接收自己发的鱼雷
		//case FiEventType.RECV_PK_OTHER_LAUNCH_TORPEDO_INFORM: //接收其他玩家发的鱼雷
		//case FiEventType.RECV_PK_TORPEDO_EXPLODE_RESPONSE: //接收自己发的鱼雷爆炸
		//case FiEventType.RECV_PK_OTHER_TORPEDO_EXPLODE_INFORM: //接收其他玩家发的鱼雷爆炸
			
		case FiEventType.RECV_PK_GOLD_GAME_RESULT_INFORM: //FishingPoint,FishingButtle比赛结果
		case FiEventType.RECV_PK_POINT_GAME_RESULT_INFORM: 
		case FiEventType.RECV_PK_POINT_GAME_ROUND_RESULT_INFORM:
		case FiEventType.RECV_XL_CHANGEUSERGOLD_RESPOSE:
		case FiEventType.RECV_XL_BOSSMATCHTIME_RESPOSE:
//		case FiEventType.RECV_XL_BOSSROOMMATCH_RESPOSE:
//		case FiEventType.RECV_XL_NOTIFYSIGNUP_RESPOSE:
		//FishingPKFriend
		//case FiEventType.RECV_FRIEND_ROOM_RESULT_INFORM: //朋友约战比赛结果
			ToUIFishing (type, data);
			break;

		}

	}

	void ToUILogin (int type, object data)
	{
		if (null != uiLoginMsg) {
			Debug.Log ("null!=uiLogin");
			uiLoginMsg.OnRcv (type, data);
		} else {
			Debug.Log ("null==uiLogin");
		}
			
	}

	void ToUIHall (int type, object data)
	{
		if (null != uiHallMsg)
			uiHallMsg.OnRcv (type, data);
	}

	void ToUIFishing (int type, object data)
	{
		if (null != uiFishingMsg)
			uiFishingMsg.OnRcv (type, data);
	}
}
