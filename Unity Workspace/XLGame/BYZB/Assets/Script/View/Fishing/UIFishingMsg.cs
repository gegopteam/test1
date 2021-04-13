/* author:KinSen
 * Date:2017.06.30
 */

using UnityEngine;
using System.Collections;

using System.Collections.Generic;

using AssemblyCSharp;

/*新增场次*/
public class UIFishingMsg : IDispatch, 
ISndFishingMsg, ISndFishingMsgClassic, 
ISndFishingMsgPK, ISndFishingMsgPKBullet, ISndFishingMsgPKPoint, ISndFishingMsgPKTime, 
IFishingOther
{
	private static UIFishingMsg instance = null;

	public static UIFishingMsg GetInstance ()
	{
		if (null == instance) {
			instance = new UIFishingMsg ();
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
	private MyInfo myInfo = null;
	private RoomInfo roomInfo = null;
	private EventControl mEventCtrl = null;
	private int typeFishing = 0;
	private RcvFishingMsg fishingMsg = null;

	private RcvFishingMsgClassic mClassicMsgHandle = new RcvFishingMsgClassic ();
	private RcvFishingMsgPK mPkMsgHandle = new RcvFishingMsgPK ();
	//	private RcvFishingMsgBoss mBossMsgHandle = new RcvFishingMsgBoss ();

	private UIFishingMsg ()
	{
		Init ();
	}

	~UIFishingMsg ()
	{
		UnInit ();
	}

	void Init ()
	{
		dispatchControl = DispatchControl.GetInstance ();
		dispatchControl.AddRcv (AppFun.FISHING, this);
		dataControl = DataControl.GetInstance ();
		myInfo = dataControl.GetMyInfo ();
		roomInfo = dataControl.GetRoomInfo ();
		InitEventHandler ();
	}


	private void InitEventHandler ()
	{
		mEventCtrl = EventControl.instance ();
		//mEventCtrl.addEventHandler ( FiEventType.RECV_FRIEND_ROOM_RESULT_INFORM , new RcvFishingMsgPKFriendGold().RcvPKFriendGameResultInform );

		mEventCtrl.addEventHandler (FiEventType.RECV_FISH_TIDE_COMING_INFORM, RcvTideComingInform);
		mEventCtrl.addEventHandler (FiEventType.RECV_FISH_TIDE_CLEAN_INFORM, RcvTideCleanInform);


		mEventCtrl.addEventHandler (FiEventType.RECV_ROOM_MATCH_RESPONSE, mClassicMsgHandle.RcvRoomMatchResponse);
		mEventCtrl.addEventHandler (FiEventType.RECV_LAUNCH_TORPEDO_RESPONSE, mClassicMsgHandle.RcvLaunchTorpedoResponse);
		mEventCtrl.addEventHandler (FiEventType.RECV_OTHER_LAUNCH_TORPEDO_INFORM, mClassicMsgHandle.RcvOtherLaunchTorpedoInform);
		mEventCtrl.addEventHandler (FiEventType.RECV_TORPEDO_EXPLODE_RESPONSE, mClassicMsgHandle.RcvTorpedoExplodeResponse);
		mEventCtrl.addEventHandler (FiEventType.RECV_OTHER_TORPEDO_EXPLODE_INFORM, mClassicMsgHandle.RcvOtherTorpedoExplodeInform);
		mEventCtrl.addEventHandler (FiEventType.RECV_UNLOCK_CANNON_MULTIPLE_RESPONSE, mClassicMsgHandle.RcvUnlockCannonResponse);


		mEventCtrl.addEventHandler (FiEventType.RECV_PK_LAUNCH_TORPEDO_RESPONSE, mPkMsgHandle.RcvPKLaunchTorpedoResponse);
		mEventCtrl.addEventHandler (FiEventType.RECV_PK_OTHER_LAUNCH_TORPEDO_INFORM, mPkMsgHandle.RcvPKOtherLaunchTorpedoInform);
		mEventCtrl.addEventHandler (FiEventType.RECV_PK_TORPEDO_EXPLODE_RESPONSE, mPkMsgHandle.RcvPKTorpedoExplodeResponse);
		mEventCtrl.addEventHandler (FiEventType.RECV_PK_OTHER_TORPEDO_EXPLODE_INFORM, mPkMsgHandle.RcvPKOtherTorpedoExplodeInform);
		mEventCtrl.addEventHandler (FiEventType.RECV_OTHER_LEAVE_PK_ROOM_INFORM, mPkMsgHandle.RcvPKOtherLeaveRoom);

		mEventCtrl.addEventHandler (FiEventType.RECV_DECREASE_CONSUMED_GOLD_INFORM, RcvDecreaseConsumedGoldInform);

		// ----------------------Boss场部分
//		mEventCtrl.addEventHandler (FiEventType.RECV_BOSS_LAUNCH_TORPEDO_RESPONSE, mBossMsgHandle.RcvBossLaunchTorpedoResponse);
//		mEventCtrl.addEventHandler (FiEventType.RECV_BOSS_OTHER_LAUNCH_TORPEDO_INFORM, mBossMsgHandle.RcvBossOtherLaunchTorpedoInform);
//		mEventCtrl.addEventHandler (FiEventType.RECV_BOSS_TORPEDO_EXPLODE_RESPONSE, mBossMsgHandle.RcvBossTorpedoExplodeResponse);
//		mEventCtrl.addEventHandler (FiEventType.RECV_BOSS_OTHER_TORPEDO_EXPLODE_INFORM, mBossMsgHandle.RcvBossOtherTorpedoExplodeInform);
//		mEventCtrl.addEventHandler (FiEventType.RECV_OTHER_LEAVE_PK_ROOM_INFORM, mBossMsgHandle.RcvBossOtherLeaveRoom);

	}

	private void RcvDecreaseConsumedGoldInform (object data)  //挂机15秒后发，值为减少的值
	{
		FiDecreaseConsumedGoldInform nInform = (FiDecreaseConsumedGoldInform)data;
		Debug.LogError ("红包场挂机减少的值:" + nInform.decreasedGold);
		if (RedPacket_TopInfo._instance.gameObject != null) {
			RedPacket_TopInfo._instance.DecreaseProgress ((int)nInform.decreasedGold);
		}
		//	nInform.decreasedGold
	}

	private void RcvTideComingInform (object data)
	{
		if (GameController._instance != null)
			GameController._instance.FishTideComing1 ();
	}

	private void RcvTideCleanInform (object data)
	{
		if (GameController._instance != null)
			GameController._instance.FishTideComing2 ();
	}



	void UnInit ()
	{
		Clear ();
		if (null != dispatchControl) {
			dispatchControl.AddRcv (AppFun.FISHING, null);
		}
		dataControl = null;
		myInfo = null;
		roomInfo = null;
	}

	void Clear ()
	{
		if (null != myInfo) {
			myInfo.SetCannonInfo (null);
		}

	}

	public void SetFishing (int type)
	{
		if (typeFishing != type) {
			fishingMsg = null;
		}

		switch (type) {
		case TypeFishing.CLASSIC:
			fishingMsg = new RcvFishingMsgClassic ();
			break;
		case TypeFishing.PKPoint:
			fishingMsg = new RcvFishingMsgPKPoint ();
			break;
		case TypeFishing.PKTime:
			fishingMsg = new RcvFishingMsgPKTime ();
			break;
		case TypeFishing.PKBullet:
			fishingMsg = new RcvFishingMsgPKBullet ();
			break;
		case TypeFishing.PKFriendGold:
			fishingMsg = new RcvFishingMsgPKFriendGold ();
			break;
		case TypeFishing.PKFriendCard:
			fishingMsg = new RcvFishingMsgPKFriendCard ();
			break;
		case TypeFishing.REDPACKET:
			fishingMsg = new RcvFishingMsgClassic ();
			break;
//		case TypeFishing.BOSS:
//			fishingMsg = new RcvFishingMsgBoss ();
//			break;
		}

		typeFishing = type;
	}

	public void OnRcv (int type, object data)
	{
		if (null == fishingMsg)
			return;
		
		switch (type) {
		case FiEventType.RECV_FISHS_CREATED_INFORM: //收到生成鱼群消息
			fishingMsg.RcvFishCreateInfo (data);
			break;

		case FiEventType.RECV_OTHER_FIRE_BULLET_INFORM: //收到其他玩家发送子弹通知
			fishingMsg.RcvOtherFireBullet (data);
			break;
		case FiEventType.RECV_HITTED_FISH_RESPONSE: //击中鱼反馈
			fishingMsg.RcvHitFishResponse (data);
			break;
		case FiEventType.RECV_XL_GET_HONG_BAO_GOLD://红包
			fishingMsg.RveHieFish_RedPack_Response (data);
			break;
		case FiEventType.RECV_OTHER_ENTER_ROOM_INFORM: //收到其他玩家进入房间消息通知
			fishingMsg.RcvOtherEnterRoom (data);
			break;
		case FiEventType.RECV_OTHER_LEAVE_ROOM_INFORM: //收到其他玩家离开房间消息通知
			fishingMsg.RcvOtherLeaveRoom (data);
			break;
		case FiEventType.RECV_USER_LEAVE_RESPONSE: //玩家自己离开房间
			fishingMsg.RcvUserLeaveRoom (data);
			break;

		case FiEventType.RECV_FISH_OUT_RESPONSE: //鱼游出屏幕
			fishingMsg.RcvFishOutResponse (data);
			break;

		case FiEventType.RECV_CHANGE_CANNON_RESPONSE: //改变炮等级
			fishingMsg.RcvChangeCannonMultiple (data);
			break;
		case FiEventType.RECV_OTHER_CHANGE_CANNON_INFORM: //其他人改变炮等级
			fishingMsg.RcvOtherChangeCannonMultiple (data);
			break;
		case FiEventType.RECV_USE_EFFECT_RESPONSE: //接收自己的特效信息
			fishingMsg.RcvEffectResponse (data);
			break;
		case FiEventType.RECV_OTHER_EFFECT_INFORM: //接收其他玩家的特效信息
			fishingMsg.RcvOtherEffectResponse (data);
			break;
		case FiEventType.RECV_FISH_FREEZE_TIMEOUT_INFORM: //接收冰冻时间结束
			fishingMsg.RcvFishFreezeTimeOutResponse (data);
			break;
		//赋值金币
		case FiEventType.RECV_XL_CHANGEUSERGOLD_RESPOSE:
			fishingMsg.RcvChangeUserGold (data);
			break;
		case FiEventType.RECV_XL_BOSSMATCHTIME_RESPOSE:
			fishingMsg.RcvUpdateBossMatchTime (data);
			break;
		}

		if (TypeFishing.CLASSIC == typeFishing) {
//			RcvFishingMsgClassic fishingClassic = (RcvFishingMsgClassic)fishingMsg;
			switch (type) {
			//	case FiEventType.RECV_ROOM_MATCH_RESPONSE: //房间匹配成功，进(打鱼)房间
			//		fishingClassic.RcvRoomMatchResponse (data);
			//		break;

			case FiEventType.RECV_LAUNCH_TORPEDO_RESPONSE: //接收发鱼雷回复
				//fishingClassic.RcvLaunchTorpedoResponse (data);
				break;

			case FiEventType.RECV_OTHER_LAUNCH_TORPEDO_INFORM: //接收其他玩家发鱼雷回复
				//fishingClassic.RcvOtherLaunchTorpedoInform (data);
				break;

			case FiEventType.RECV_TORPEDO_EXPLODE_RESPONSE: //接收鱼雷爆炸
				//fishingClassic.RcvTorpedoExplodeResponse (data);
				break;
			case FiEventType.RECV_OTHER_TORPEDO_EXPLODE_INFORM: //接收其他玩家鱼雷爆炸通知
				//fishingClassic.RcvOtherTorpedoExplodeInform (data);
				break;

			}
			return;
		} else if (TypeFishing.PKPoint == typeFishing ||
		           TypeFishing.PKTime == typeFishing ||
		           TypeFishing.PKBullet == typeFishing ||
		           TypeFishing.PKFriendGold == typeFishing ||
		           TypeFishing.PKFriendCard == typeFishing) {
			OnRcvFishingPK (type, data);
		}
//		} else if (TypeFishing.BOSS == typeFishing) {
//			OnRcvFishingBOSS (type, data);
//		}

	}

	//	void  OnRcvFishingBOSS (int type, object data)
	//	{
	//		RcvFishingMsgBoss fishingBoss = (RcvFishingMsgBoss)fishingMsg;
	//		switch (type) {
	//		case 1:
	//			break;
	//		default:
	//			break;
	//		}
	//	}

	void OnRcvFishingPK (int type, object data)
	{
		RcvFishingMsgPK fishingPK = (RcvFishingMsgPK)fishingMsg;
		switch (type) {
		case FiEventType.RECV_LEAVE_PK_ROOM_RESPONSE:
//			fishingMsg.RcvUserLeaveRoom (data);
			fishingPK.RcvPKLeaveRoom (data);
			break;

		//case FiEventType.RECV_START_PK_GAME_INFORM: //接收开始PK游戏通知
		//	fishingPK.RcvPKStartGameInform (data);
		//	break;
		case FiEventType.RECV_PRE_PKGAME_COUNTDOWN_INFORM: //游戏开始前的倒计时
			fishingPK.RcvPKPreGameCountdownInform (data);
			break;
		case FiEventType.RECV_PKGAME_COUNTDOWN_INFORM: //游戏倒计时
			fishingPK.RcvPKGameCountdownInform (data);
			break;
		case FiEventType.RECV_PK_USE_EFFECT_RESPONSE: //接收自己发的道具
			fishingPK.RcvPKUseEffectResponse (data);
			break;
		case FiEventType.RECV_PK_OTHER_EFFECT_INFORM: //接收其他玩家发的道具通知
			fishingPK.RcvPKOtherEffectInform (data);
			break;
		case FiEventType.RECV_PK_DISTRIBUTE_PROPERTY_INFORM: //接收初次进PK房下发的道具
			fishingPK.RcvPKDistributePropertyInform (data);
			break;
		case FiEventType.RECV_PK_LAUNCH_TORPEDO_RESPONSE: //接收自己发的鱼雷
			fishingPK.RcvPKLaunchTorpedoResponse (data);
			break;
		case FiEventType.RECV_PK_OTHER_LAUNCH_TORPEDO_INFORM: //接收其他玩家发的鱼雷
			fishingPK.RcvPKOtherLaunchTorpedoInform (data);
			break;
		case FiEventType.RECV_PK_TORPEDO_EXPLODE_RESPONSE: //接收自己发的鱼雷爆炸
			fishingPK.RcvPKTorpedoExplodeResponse (data);
			break;
		case FiEventType.RECV_PK_OTHER_TORPEDO_EXPLODE_INFORM: //接收其他玩家发的鱼雷爆炸
			fishingPK.RcvPKOtherTorpedoExplodeInform (data);
			break;
		}

		if (TypeFishing.PKPoint == typeFishing) {
			RcvFishingMsgPKPoint fishingPKPoint = (RcvFishingMsgPKPoint)fishingMsg;
			switch (type) {
			case FiEventType.RECV_PK_GOLD_GAME_RESULT_INFORM:
				fishingPKPoint.RcvPKGoldGameResultInform (data);
				break;

			}
		}

		if (TypeFishing.PKTime == typeFishing) {
			RcvFishingMsgPKTime fishingPKTime = (RcvFishingMsgPKTime)fishingMsg;
			switch (type) {
//			case FiEventType.RECV_START_PK_GAME_RESPONSE: //接收房主开始PK游戏消息
//				fishingPKTime.RcvPKStartGameResponse (data);
//				break;
			case FiEventType.RECV_PK_GOLD_GAME_RESULT_INFORM:
				fishingPKTime.RcvPKGoldGameResultInform (data);
				break;

//			case FiEventType.RECV_PK_POINT_GAME_ROUND_RESULT_INFORM:
//				fishingPKTime.RcvPKPointGameRoundResultInform (data);
//				break;
//			case FiEventType.RECV_PK_POINT_GAME_RESULT_INFORM:
//				fishingPKTime.RcvPKPointGameResultInform (data);
//				break;
			}
		}

		if (TypeFishing.PKBullet == typeFishing) {
			RcvFishingMsgPKBullet fishingPKBullet = (RcvFishingMsgPKBullet)fishingMsg;
			switch (type) {
			case FiEventType.RECV_PK_GOLD_GAME_RESULT_INFORM:
				fishingPKBullet.RcvPKGoldGameResultInform (data);
				break;
			}
		}

		if (TypeFishing.PKFriendGold == typeFishing ||
		    TypeFishing.PKFriendCard == typeFishing) {
			RcvFishingMsgPKFriend fishingMsgPKFriend = (RcvFishingMsgPKFriend)fishingMsg;



			switch (type) {
			//case FiEventType.RECV_START_PK_GAME_RESPONSE: //接收房主开始PK游戏消息
			//	fishingMsgPKFriend.RcvPKStartGameResponse (data);
			//	break;
			//case FiEventType.RECV_FRIEND_ROOM_RESULT_INFORM: //接收开始PK游戏通知
			//fishingMsgPKFriend.RcvPKFriendRoomResultInform (data);
			//	break;
			//case FiEventType.RECV_LEAVE_FRIEND_ROOM_RESPONSE:
			//	fishingMsgPKFriend.RcvPKFriendLeaveRoom ( data );
			//	break;
			}
		}
		return;
	}
}
