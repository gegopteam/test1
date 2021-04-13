using UnityEngine;
using System.Collections;

using AssemblyCSharp;

//负责：Hall部分UI对象的管理
public class UIHallObjects
{
	private static UIHallObjects instance = null;

	public static UIHallObjects GetInstance ()
	{
		if (null == instance) {
			instance = new UIHallObjects ();
		}

		return instance;
	}

	public static void DestroyInstance ()
	{
		if (null != instance) {
			instance = null;
		}
	}

	IData rcvUIHallPKRoom = null;
	IDispatch rcvUIHallInPKRoom = null;
	IDispatch rcvUIHallEnterRoom = null;
	IDispatch rcvUIHallCreateFriendRoom = null;

	DataControl dataControl = null;
	MyInfo myInfo = null;
	RoomInfo roomInfo = null;

	private UIHallObjects ()
	{
		dataControl = DataControl.GetInstance ();
		myInfo = dataControl.GetMyInfo ();
		roomInfo = dataControl.GetRoomInfo ();
	}

	~UIHallObjects ()
	{

	}

	public void SetRcv (AppFun type, object obj)
	{
		switch (type) {
		case AppFun.UIHALL_PKROOM:
			rcvUIHallPKRoom = (IData)obj;
			break;
		case AppFun.UIHALL_INPKROOM:
			rcvUIHallInPKRoom = (IDispatch)obj;
			break;
		case AppFun.UIHALL_ENTER_FRIENDROOM:
			rcvUIHallEnterRoom = (IDispatch)obj;
			break;
		case AppFun.UIHALL_CREATE_FRIENDROOM:
			{
				if (null == obj) {
					Tool.Log ("SetRcv rcvUIHallCreateFriendRoom 000");
				} else {
					Tool.Log ("SetRcv rcvUIHallCreateFriendRoom 111");
				}
			}
			rcvUIHallCreateFriendRoom = (IDispatch)obj;

			break;
		}
	}

	public void CreatePKRoom (FiCreatePKRoomResponse info)
	{
		Tool.Log ("CreatePKRoom 000");
		if (null == rcvUIHallPKRoom)
			return;
		Tool.Log ("CreatePKRoom 111");
		rcvUIHallPKRoom.RcvInfo (info);
	}

	public void ToPKRoom (int type, object data)
	{
		switch (type) {
		case FiEventType.RECV_ENTER_PK_ROOM_RESPONSE: //进入PK准备房间消息回复
		case FiEventType.RECV_OTHER_ENTER_PK_ROOM_INFORM: //其他人进入PK准备房间通知
			
		case FiEventType.RECV_LEAVE_PK_ROOM_RESPONSE: //离开PK准备房间消息回复
		case FiEventType.RECV_OTHER_LEAVE_PK_ROOM_INFORM: //其他人离开PK准备房间消息通知

		//case FiEventType.RECV_OTHER_ENTER_FRIEND_ROOM_INFORM: //接收其他玩家进入好友约战房间通知
		//case FiEventType.RECV_LEAVE_FRIEND_ROOM_RESPONSE: //接收离开好友约战房间回复
		//case FiEventType.RECV_OTHER_LEAVE_FRIEND_ROOM_INFORM: //接收其他玩家来开好友约战房间通知

		//case FiEventType.RECV_OTHER_PREPARE_PKGAME_INFORM: //接收其他玩家准备通知
		//case FiEventType.RECV_OTHER_CANCEL_PREPARE_PKGAME_INFORM: //接收其他玩家取消准备通知

		//case FiEventType.RECV_DISBAND_FRIEND_ROOM_RESPONSE: //房间收到解散房间反馈 
		//case FiEventType.RECV_DISBAND_FRIEND_ROOM_INFORM: //其他玩家收到房间解散的通知 

		//case FiEventType.RECV_START_PK_GAME_RESPONSE: //接收房主开始PK游戏消息
		//case FiEventType.RECV_START_PK_GAME_INFORM: //接收开始PK游戏通知
			{
				if (null != rcvUIHallInPKRoom) {
					rcvUIHallInPKRoom.OnRcv (type, data);
				}
			}
			break;

//		case FiEventType.RECV_ENTER_FRIEND_ROOM_RESPONSE: //接收进入好友约战房间回复
//			{
//				if(null!=rcvUIHallEnterRoom)
//				{
//					rcvUIHallEnterRoom.OnRcv (type,data);
//				}
//			}
//			break;

		/*case FiEventType.RECV_CREATE_FRIEND_ROOM_RESPONSE: //接收创建好友约战房间回复
			{
				if (null != rcvUIHallCreateFriendRoom) {
					Tool.LogError ("RECV_CREATE_FRIEND_ROOM_RESPONSE !null");
					rcvUIHallCreateFriendRoom.OnRcv (type, data);
				} else {
					Tool.LogError ("RECV_CREATE_FRIEND_ROOM_RESPONSE null");
				}
			}

			break;*/
		}


	}

	public void ToPlay (int nCannonIndex)
	{
		if (myInfo.lockScene) {
			return;
		}
		Debug.LogError ("nCannonIndex = " + nCannonIndex);
		UIHallMsg.GetInstance ().SndRoomMatchRequest (0, nCannonIndex);
//		Tool.OutLogWithToFile ("snd RoomMatchRequest");
	}

	/// <summary>
	/// 自动进入废弃的,因为当前炮倍数直接为9900,所以这里用不到
	/// </summary>
	private void AutoEnterClassicRoom1 ()
	{
		return;
		Debug.LogError ("myInfo.cannonMultipleMax" + myInfo.cannonMultipleMax);
		int nMaxCannonMutiple = myInfo.cannonMultipleMax;
		if (nMaxCannonMutiple < 1) {
			Debug.LogError ("[ classic room ] error cannon mutiple " + nMaxCannonMutiple);
			Tool.AddLogMsg ("[ classic room ] error cannon mutiple " + nMaxCannonMutiple);
			return;
		}
		if (nMaxCannonMutiple <= 100) {
			ToPlay (5);
		} else if (nMaxCannonMutiple < 1000) {
			ToPlay (1);
		} else {
			ToPlay (2);
		}
	}

	/// <summary>
	/// 自动进入房间,根据身上携带金币数判断
	/// 2019.3.7 需求 10万以下 新手房  10万~50万中级房 50万~500万3倍房 500万以上BOSS房
	/// </summary>
	private void AutoEnterClassicRoom ()
	{
		//十万
		long OneHundredThousand = 100000;
		//五十万
		long FiveHundredThousand = 500000;
		//五百万
		long FiveMillion = 5000000;
//		Debug.LogError ("myInfo.cannonMultipleMax" + myInfo.cannonMultipleMax);
		long maxGoldNum = myInfo.gold;
		if (maxGoldNum < 0) {
			Debug.LogError ("[ classic room ] error myInfo.gold " + maxGoldNum);
			return;
		}
		//10万以下 海盜港灣
		if (maxGoldNum < OneHundredThousand) {
			ToPlay (6);
		}
		//10万~50万中级房
		else if (maxGoldNum >= OneHundredThousand && maxGoldNum < FiveHundredThousand) {
			ToPlay (1);
		}
		//50万~500万3倍房
		else if (maxGoldNum >= FiveHundredThousand && maxGoldNum < FiveMillion) {
			ToPlay (2);
		}
		//500万以上BOSS房
		else {
			ToPlay (3);
		}
	}

	public void StartGame ()
	{//一键开始
		if (null != myInfo) {
			//if (myInfo.isEnterClassicRoom ())
			//	return;
			AutoEnterClassicRoom ();
		}

	}

	public void PlayFieldGrade_5()
	{//海盜海湾
		int multiple = myInfo.cannonMultipleMax;
		Debug.LogError("--------multiple-------" + multiple + " / " + CannonMultiple.NEWBIE);
		ToPlay(6);
	}

	public void PlayFieldGrade_1 ()
	{//新手海湾
		int multiple = myInfo.cannonMultipleMax;
		Debug.LogError ("--------multiple-------" + multiple + " / " + CannonMultiple.NEWBIE);
		ToPlay (0);
	}

	public void PlayFieldGrade_2 ()
	{//深海遗址
		int multiple = myInfo.cannonMultipleMax;

		Debug.LogError ("--------multiple-------" + multiple + " / " + CannonMultiple.DEEPSEA);

        if (multiple >= CannonMultiple.DEEPSEA || myInfo.misHaveDraCard) {// || myInfo.levelVip >= 2

			ToPlay (1);
		}
        else
        {
            //炮倍数不满足开启龙卡界面
            UIHallCore.Instance.MouthCard();
        }
	}

	public void PlayFieldGrade_3 ()
	{//海神宝藏
		int multiple = myInfo.cannonMultipleMax;

		Debug.LogError ("--------multiple-------" + multiple + " / " + CannonMultiple.POSEIDON);

        if (multiple >= CannonMultiple.POSEIDON || myInfo.misHaveDraCard) {// || myInfo.levelVip >= 2
			ToPlay (2);
		}
        else
        {
            //炮倍数不满足开启龙卡界面
            UIHallCore.Instance.MouthCard();
        }
	}

	public void PlayFieldGrade_4 ()
	{//夺金岛？
		int multiple = myInfo.cannonMultipleMax;

//		Debug.LogError ("--------multiple-------" + multiple + " / " + CannonMultiple.POSEIDON);

        if (multiple >= CannonMultiple.GOLDMEDAL || myInfo.misHaveDraCard) {// || myInfo.levelVip >= 2
		//--todo打开二级界面的位置
		//现在直接跳转没有二级界面
		    ToPlay (3);
		//AppControl.ToView (AppView.HALLNEWPLAY);
		//Debug.Log ("打开夺金岛");
		//Debug.LogError ("_____________________scendinsce____________");
		}
        else
        {
            //炮倍数不满足开启龙卡界面
            UIHallCore.Instance.MouthCard();
        }
	}

	public void PlayHallnewFieldGrade_0 ()
	{//boss场
		int multiple = myInfo.cannonMultipleMax;

//		Debug.LogError ("--------multiple-------" + multiple + " / " + CannonMultiple.DEEPSEA + myInfo.misHaveDraCard + myInfo.gold);

//		if (multiple >= CannonMultiple.POSEIDON && (int)myInfo.gold >= 5000000 || myInfo.misHaveDraCard) {
		ToPlay (3);
//		} else {
//			GameObject Window1 = UnityEngine.Resources.Load ("Window/WindowTipsThree")as UnityEngine.GameObject;
//			GameObject WindowClone1 = UnityEngine.GameObject.Instantiate (Window1);
//			UITipAutoNoMask ClickTips1 = WindowClone1.GetComponent<UITipAutoNoMask> ();
//			ClickTips1.tipText.text = "你的炮的倍数小于1000或者金币不足500万";
//		}
	}

	public void PlayExperience ()
	{//体验场
		int multiple = myInfo.cannonMultipleMax;

//		Debug.LogError ("--------multiple-------" + multiple + " / " + CannonMultiple.DEEPSEA + myInfo.misHaveDraCard + myInfo.gold);

		if (multiple >= 1) {
			ToPlay (4);
		} else {
			GameObject Window1 = UnityEngine.Resources.Load ("Window/WindowTipsThree")as UnityEngine.GameObject;
			GameObject WindowClone1 = UnityEngine.GameObject.Instantiate (Window1);
			UITipAutoNoMask ClickTips1 = WindowClone1.GetComponent<UITipAutoNoMask> ();
			ClickTips1.tipText.text = "不能進體驗場";
		}
	}

	public void PlayBossMatch ()
	{//夺金岛？
		int multiple = myInfo.cannonMultipleMax;

//		Debug.LogError ("--------multiple-------" + multiple + " / " + CannonMultiple.POSEIDON);

		//if (multiple >= CannonMultiple.POSEIDON || myInfo.misHaveDraCard || myInfo.levelVip >= 2) {
		//--todo打开二级界面的位置
		//现在直接跳转没有二级界面
		ToPlay (5);
		BossMatchScript.beginType = 0;

		//AppControl.ToView (AppView.HALLNEWPLAY);
		/*Debug.Log ("打开夺金岛");
			Debug.LogError ("_____________________scendinsce____________");*/
		//		}/
	}
}
