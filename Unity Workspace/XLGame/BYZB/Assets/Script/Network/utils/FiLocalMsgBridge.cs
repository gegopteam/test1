using UnityEngine;
using System.Collections;
using System.Runtime.InteropServices;
using System.Net;
using System.Net.Sockets;
using System;
using System.Threading;
using System.IO;
using System.Collections.Generic;
using Google.Protobuf;

namespace AssemblyCSharp
{

	public class MsgBaseAgent
	{
		protected NetControl mNetCtrl;

		public MsgBaseAgent ()
		{
			
		}

		public void SetDispatch (NetControl nValue)
		{
			mNetCtrl = nValue;
		}

		public void SendByteArray (int nMsgId, byte[] nByteSend)
		{
			mNetCtrl.SendBytes (nMsgId, nByteSend);
		}

		public void Dispatch (int nType, object data)
		{
			mNetCtrl.dispatchEvent (nType, data);
		}
	}

		
	public class FiLocalMsgBridge
	{

		private delegate void MessageConvert (byte[] data);

		private Dictionary< int , MessageConvert > mConvertRecvMap = new Dictionary<int, MessageConvert> ();

		public Boolean isContainMessage (int nMessageId)
		{
			return mConvertRecvMap.ContainsKey (nMessageId);
		}

		FiFishingMsgAgent mFishingAgent;

		FiHallMsgAgent mHallAgent;

		FiLoginMsgAgent mLoginAgent;
	
		NetControl mNetCtrl;

		public FiLocalMsgBridge (NetControl nValue)
		{
			mNetCtrl = nValue;
			mFishingAgent = new FiFishingMsgAgent (nValue);
			mHallAgent = new FiHallMsgAgent (nValue);
			mLoginAgent = new FiLoginMsgAgent (nValue);
			RegisterRecv ();
		}


		public void  processRecv (int nType, byte[] data)
		{
			if (mConvertRecvMap.ContainsKey (nType)) {
				mConvertRecvMap [nType].Invoke (data);
			} else {
				Debug.LogError ("[ bridge ] type" + nType + " missing !");
			}
		}

		public void processSend ()
		{
			
		}

		private void RegisterSend ()
		{

		}

		private void RegisterRecv ()
		{
			//RecvOtherUnlockMutipleInform
			mConvertRecvMap.Add (FiProtoType.FISHING_OTHER_UNLOCK_CANNON_MULTIPLE, mFishingAgent.RecvOtherUnlockMutipleInform);
			mConvertRecvMap.Add (FiProtoType.FISHING_NOTIFY_FISH_OUT_OF_SCENE, mFishingAgent.RecvOutOfSence);
			mConvertRecvMap.Add (FiProtoType.FISHING_LEAVE_CLASSICAL_ROOM_RES, mFishingAgent.RecvClassicLeaveRoom);
			mConvertRecvMap.Add (FiProtoType.FISHING_NOTIFY_LEAVE_CLASSICAL_ROOM, mFishingAgent.RecvClassicOtherLeaveRoom);
			mConvertRecvMap.Add (FiProtoType.FISHING_NOTIFY_ENTER_CLASSICAL_ROOM, mFishingAgent.RecvClassicOtherEnterRoom);
			mConvertRecvMap.Add (FiProtoType.FISHING_ENTER_CLASSICAL_ROOM_RES, mFishingAgent.RecvClassicEnterRoom);
			mConvertRecvMap.Add (FiProtoType.FISHING_NOTIFY_FIRE, mFishingAgent.RecvOtherFire);
			mConvertRecvMap.Add (FiProtoType.FISHING_NOTIFY_ON_FISH_HIT, mFishingAgent.RecvOnFishHit);
            mConvertRecvMap.Add (FiProtoType.XL_GET_HONG_BAO_GOLD, mFishingAgent.RecvOnRedpack);
			mConvertRecvMap.Add (FiProtoType.FISHING_NOTIFY_FISH_GROUP, mFishingAgent.RecvOnCreateFishGroup);
			mConvertRecvMap.Add (FiProtoType.FISHING_EFFECT_RESPONSE, mFishingAgent.RecvEffectResponse);
			mConvertRecvMap.Add (FiProtoType.FISHING_OTHER_EFFECT, mFishingAgent.RecvOtherEcffectInform);
			mConvertRecvMap.Add (FiProtoType.FISHING_FREEZE_TIMEOUT, mFishingAgent.RecvFreezeTimeOut);
			mConvertRecvMap.Add (FiProtoType.FISHING_OTHER_CHANGE_CANNON_MULTIPLE, mFishingAgent.RecvOtherChangeConnonMultiple);
			mConvertRecvMap.Add (FiProtoType.FISHING_CHANGE_CANNON_MULTIPLE_RESPONSE, mFishingAgent.RecvChangeConnonMultiple);
			mConvertRecvMap.Add (FiProtoType.FISHING_START_PKGAME_RESPONSE, mFishingAgent.RecvPKStartGAMEResponse);
			mConvertRecvMap.Add (FiProtoType.FISHING_NOTIFY_PKGAME_START, mFishingAgent.RecvPKGameStartInform);
			mConvertRecvMap.Add (FiProtoType.FISHING_ENTER_PKROOM_RESPONSE, mFishingAgent.RecvPKEnterRoomResponse);
			mConvertRecvMap.Add (FiProtoType.FISHING_NOTIFY_ENTER_PKROOM, mFishingAgent.RecvPKOtherEnterRoomInform);
			mConvertRecvMap.Add (FiProtoType.FISHING_LEAVE_PKROOM_RESPONSE, mFishingAgent.RecvPKLeaveRoomResponse);
			mConvertRecvMap.Add (FiProtoType.FISHING_NOTIFY_LEAVE_PKROOM, mFishingAgent.RecvPKOtherLeaveRoomInform);
			mConvertRecvMap.Add (FiProtoType.FISHING_PREPARE_PKGAME, mFishingAgent.RecvPKPrepareGame);
			mConvertRecvMap.Add (FiProtoType.FISHING_CANCEL_PREPARE_PKGAME, mFishingAgent.RecvPkCancelPrepareGame);
			mConvertRecvMap.Add (FiProtoType.FISHING_PK_GAME_COUNTDOWN, mFishingAgent.RecvPKGameCountDown);
			mConvertRecvMap.Add (FiProtoType.FISHING_PRE_START_COUNTDOWN, mFishingAgent.RecvPKPreStartCountDown);
			mConvertRecvMap.Add (FiProtoType.FISHING_LAUNCH_TORPEDO_RESPONSE, mFishingAgent.RecvClassicLaunchTorpedoResponse);
			mConvertRecvMap.Add (FiProtoType.FISHING_NOTIFY_LAUNCH_TORPEDO, mFishingAgent.RecvClassicOtherLaunchTorpedo);
			mConvertRecvMap.Add (FiProtoType.FISHING_PK_LAUNCH_TORPEDO_RESPONSE, mFishingAgent.RecvPKLaunchTorpedoResponse);
			mConvertRecvMap.Add (FiProtoType.FISHING_NOTIFY_PK_LAUNCH_TORPEDO, mFishingAgent.RecvPKOtherOtherLaunchTorpedo);
			mConvertRecvMap.Add (FiProtoType.FISHING_TORPEDO_EXPLODE_RESPONSE, mFishingAgent.RecvClassicTorpedoExplode);
			mConvertRecvMap.Add (FiProtoType.FISHING_NOTIFY_TORPEDO_EXPLODE, mFishingAgent.RecvClassicOtherTorpedoExplode);
			mConvertRecvMap.Add (FiProtoType.FISHING_NOTIFY_PK_TORPEDO_EXPLODE, mFishingAgent.RecvPKOtherTorpedoExplode);
			mConvertRecvMap.Add (FiProtoType.FISHING_PK_TORPEDO_EXPLODE_RESPONSE, mFishingAgent.RecvPKTorpedoExplodeResponse);
			mConvertRecvMap.Add (FiProtoType.FISHING_PK_DISTRIBUTE_PROPERTY, mFishingAgent.RecvPKDistributeProperty);
			mConvertRecvMap.Add (FiProtoType.FISHING_GOLD_GAME_RESULT, mFishingAgent.RecvGoldGameResult);
			mConvertRecvMap.Add (FiProtoType.FISHING_POINT_GAME_RESULT, mFishingAgent.RecvPointGameResult);
			mConvertRecvMap.Add (FiProtoType.FISHING_POINT_GAME_ROUND_RESULT, mFishingAgent.RecvPointGameRoundResult);
			mConvertRecvMap.Add (FiProtoType.FISHING_PK_EFFECT_RESPONSE, mFishingAgent.RecvPKEffectResponse);
			mConvertRecvMap.Add (FiProtoType.FISHING_OTHER_PK_EFFECT, mFishingAgent.RecvPKOtherEffect);
			mConvertRecvMap.Add (FiProtoType.FISHING_CREATE_FRIEND_ROOM_RESPONSE, mFishingAgent.RecvPKCreateFriendRoomResponse);
			mConvertRecvMap.Add (FiProtoType.FISHING_ENTER_FRIEND_ROOM_RESPONSE, mFishingAgent.RecvPKEnterFriendRoomResponse);
			mConvertRecvMap.Add (FiProtoType.FISHING_NOTIFY_ENTER_FRIEND_ROOM, mFishingAgent.RecvPKOtherEnterFriendRoom);
			mConvertRecvMap.Add (FiProtoType.FISHING_FRIEND_ROOM_GAME_RESULT, mFishingAgent.RecvPKFriendRoomGameResult);
			mConvertRecvMap.Add (FiProtoType.FISHING_LEAVE_FRIEND_ROOM_RESPONSE, mFishingAgent.RecvPKLeaveFriendRoom);
			mConvertRecvMap.Add (FiProtoType.FISHING_NOTIFY_LEAVE_FRIEND_ROOM, mFishingAgent.RecvPKOtherLeaveFriendRoom);
			mConvertRecvMap.Add (FiProtoType.FISHING_DISBAND_FRIEND_ROOM_RESPONSE, mFishingAgent.RecvPKDisbandFriendRoomResponse);
			mConvertRecvMap.Add (FiProtoType.FISHING_NOTIFY_DISBAND_FRIEND_ROOM, mFishingAgent.RecvPKDisbandFriendRoomInform);
			mConvertRecvMap.Add (FiProtoType.FISHING_OPEN_RED_PACKET_RESPONSE, mFishingAgent.RecvPKOpenRedPacketResponse);
			mConvertRecvMap.Add (FiProtoType.FISHING_RED_PACKET_COUNTDOWN, mFishingAgent.RecvPKRedPacketCountDown);
			mConvertRecvMap.Add (FiProtoType.FISHING_NOTIFY_RED_PACKET, mFishingAgent.RecvPKNotifyRedPacket);
			mConvertRecvMap.Add (FiProtoType.FISHING_NOTIFY_OTHER_OPEN_RED_PACKET, mFishingAgent.RecvPKOtherOpenRedPacket);
			mConvertRecvMap.Add (FiProtoType.FISHING_ENTER_RED_PACKET_ROOM_RESPONSE, mFishingAgent.RecvPKEnterRedPacketRoom);
			mConvertRecvMap.Add (FiProtoType.FISHING_NOTIFY_ENTER_RED_PACKET_ROOM, mFishingAgent.RecvPKOtherEnterRedPacketRoom);
			mConvertRecvMap.Add (FiProtoType.FISHING_LEAVE_RED_PACKET_ROOM_RESPONSE, mFishingAgent.RecvPKLeaveRedPacketRoom);
			mConvertRecvMap.Add (FiProtoType.FISHING_NOTIFY_LEAVE_RED_PACKET_ROOM, mFishingAgent.RecvPKOtherLeaveRedPacketRoom);
			mConvertRecvMap.Add (FiProtoType.FISHING_NOTIFY_FISH_TIDE_COMING, mFishingAgent.RecvFishTideComing);
			mConvertRecvMap.Add (FiProtoType.FISHING_NOTIFY_FISH_TIDE_CLEAN_FISH, mFishingAgent.RecvFishTideCleanFish);
			mConvertRecvMap.Add (FiProtoType.FISHING_UNLOCK_CANNON_MULTIPLE_RESPONSE, mFishingAgent.RecvUnlockCannonMultiple);
			mConvertRecvMap.Add (FiProtoType.FISHING_PURCHASE_PROPERTY_RESPONSE, mFishingAgent.RecvPurchaseResponse);
			mConvertRecvMap.Add (FiProtoType.FISHING_CHANGE_CANNON_STYLE_RESPONSE, mFishingAgent.RecvChangeCannonStyleResponse);
			mConvertRecvMap.Add (FiProtoType.FISHING_NOTIFY_CHANGE_CANNON_STYLE, mFishingAgent.RecvOtherChangeCannonStyleInform);
			mConvertRecvMap.Add (FiProtoType.FISHING_GET_RED_PACKET_LIST_RESPONSE, mFishingAgent.RecvGetRedPacketListResponse);
			mConvertRecvMap.Add (FiProtoType.FISHING_NOTIFY_HAVE_DISCONNECTED_ROOM, mFishingAgent.RecvPKHaveDisconnectRoomInform);
			mConvertRecvMap.Add (FiProtoType.FISHING_EVERYDAY_TASK_RESPONSE, mHallAgent.RecvEveryDayTaskResponse);
			mConvertRecvMap.Add (FiProtoType.FISHING_EVERYDAY_TASK_PROGRESS_RESPONSE, mHallAgent.RecvEveryDayTaskProcessResponse);
			mConvertRecvMap.Add (FiProtoType.FISHING_GET_RANK_RESPONSE, mHallAgent.RecvGameRankResponse);
			mConvertRecvMap.Add (FiProtoType.FISHING_RECONNECT_GAME_RESPONSE, mHallAgent.RecvReconnectGameResponse);
			mConvertRecvMap.Add (FiProtoType.FISHING_TOP_UP_RESPONSE, mHallAgent.RecvTopupResponse);
			mConvertRecvMap.Add (FiProtoType.FISHING_GET_BACKPACK_PROPERTY_RESPONSE, mHallAgent.RecvGetBackPackResponse);
			mConvertRecvMap.Add (FiProtoType.FISHING_GIVE_OTHER_PROPERTY_RESPONSE, mHallAgent.RecvGiveOtherPropertyResponse);
			mConvertRecvMap.Add (FiProtoType.FISHING_GET_ADD_FRIEND_LIST_RESPONSE, mHallAgent.RecvGetFriendApplyListResponse);
			mConvertRecvMap.Add (FiProtoType.FISHING_GET_FRIEND_LIST_RESPONSE, mHallAgent.RecvGetFriendListResponse);
			mConvertRecvMap.Add (FiProtoType.FISHING_ADD_FRIEND_RESPONSE, mHallAgent.RecvAddFriendResponse);
			mConvertRecvMap.Add (FiProtoType.FISHING_REJECT_FRIEND_RESPONSE, mHallAgent.RecvRejectFriendApplyResponse);
			mConvertRecvMap.Add (FiProtoType.FISHING_DELETE_FRIEND_RESPONSE, mHallAgent.RecvDeleteFriendResponse);
			mConvertRecvMap.Add (FiProtoType.FISHING_ACCEPT_FRIEND_RESPONSE, mHallAgent.RecvAcceptFriendResponse);
			mConvertRecvMap.Add (FiProtoType.FISHING_GET_START_GIFT_RESPONSE, mHallAgent.RecvStartGiftResponse);
			mConvertRecvMap.Add (FiProtoType.FISHING_NOTIFY_START_GIFT, mHallAgent.RecvStartGiftInform);
			mConvertRecvMap.Add (FiProtoType.FISHING_GET_MAIL_RESPONSE, mHallAgent.RecvGetSystemMailResponse);
			mConvertRecvMap.Add (FiProtoType.FISHING_DEL_MAIL_GET_AWARD_RESPONSE, mHallAgent.RecvProcessSystemMailResponse);
			mConvertRecvMap.Add (FiProtoType.FISHING_GET_GIVE_RECORD_RESPONSE, mHallAgent.RecvGetGiveRecordResponse);
			mConvertRecvMap.Add (FiProtoType.FISHING_GET_GIVE_RESPONSE, mHallAgent.RecvAcceptGiveResponse);
			mConvertRecvMap.Add (FiProtoType.FISHING_IOS_PAY_PROPERTY_RESPONSE, mHallAgent.RecvIosPaymentResponse);
			mConvertRecvMap.Add (FiProtoType.FISHING_SIGN_IN_AWARD_RESPONSE, mHallAgent.RecvSignInAwardResponse);
			mConvertRecvMap.Add (FiProtoType.FISHING_NOTIFY_LEVEL_UP, mHallAgent.RecvLevelUpResponse);
			mConvertRecvMap.Add (FiProtoType.FISHING_GET_USER_INFO_RESPONSE, mHallAgent.RecvGetUserInfoResponse);
			mConvertRecvMap.Add (FiProtoType.FISHING_NOTIFY_DECREASE_CONSUMED_GOLD, mHallAgent.RecvDecreaseConsumeInform);
			mConvertRecvMap.Add (FiProtoType.FISHING_EVERYDAY_ACTIVITY_AWARD_RESPONSE, mHallAgent.RecvEveryDayActivityAwardResponse);
			mConvertRecvMap.Add (FiProtoType.FISHING_SELL_PROPERTY_RESPONSE, mHallAgent.RecvSellPropertyResponse);
			mConvertRecvMap.Add (FiProtoType.FISHING_NOTIFY_TOP_UP, mHallAgent.RecvTopUpInform);
			mConvertRecvMap.Add (FiEventType.RECV_BEGINNER_TASK_REWARD_RESPONSE, mHallAgent.RecvBeginnerTaskRewardResponse);
			mConvertRecvMap.Add (FiEventType.RECV_NOTIFY_OTHER_BEGINNER_REWARD, mHallAgent.RecvOtherBeginnerTaskRewardInform);
			mConvertRecvMap.Add (FiEventType.RECV_NOTIFY_BEGINNER_TASK_PROGRESS, mHallAgent.RecvBeginnerTaskProgress);
			mConvertRecvMap.Add (FiEventType.RECV_OPEN_PACK_RESPONSE, mHallAgent.RecvOpenPackResponse);
			mConvertRecvMap.Add (FiEventType.RECV_ROOM_CHAT_MESSAGE, mHallAgent.RecvUserChatInform);
			mConvertRecvMap.Add (FiEventType.RECV_GET_MONTHLY_PACK_RESPONSE, mHallAgent.RecvMonthlyPackResponse);
			mConvertRecvMap.Add (FiEventType.RECV_OTHER_RECONNECT_PKGAME_INFORM, mHallAgent.RecvOtherReconnectInform);
			mConvertRecvMap.Add (FiEventType.RECV_BROADCAST_USER_MESSAGE_RESPONSE, mHallAgent.RecvBroadCastMsgResponse);
			mConvertRecvMap.Add (FiEventType.RECV_NOTIFY_BROADCAST_GAME_INFO, mHallAgent.RecvBroadCastGameInfo);
			mConvertRecvMap.Add (FiEventType.RECV_FISHING_GET_SEVEN_REWARD_NOTIFY, mHallAgent.RecvBroadCastSevenInfo);
			mConvertRecvMap.Add (FiEventType.RECV_NEW_USER_LEVELUP_NOTIFY, mHallAgent.RecvBroadCastNewUserLevelup);
			mConvertRecvMap.Add (FiEventType.RECV_NOTIFY_BROADCAST_USER_MESSAGE, mHallAgent.RecvBroadCastMsgInform);
			//mConvertRecvMap.Add (FiEventType.RECV_FISH_LUCKY_DRAW_RESPONSE, mHallAgent.RecvLuckyDrawResponse);
			mConvertRecvMap.Add (FiEventType.RECV_NOTIFY_FISH_LUCKY_DRAW, mHallAgent.RecvLuckyDrawInform);
			mConvertRecvMap.Add (FiEventType.RECV_BANK_ACCESS_RESPONSE, mHallAgent.RecvBankAccessResponse);
			mConvertRecvMap.Add (FiEventType.RECV_SET_PSWD_RESPONSE, mHallAgent.RecvSetBankPswdResponse);
			//mConvertRecvMap.Add ( FiEventType.RECV_NOTIFY_BANK_MESSGAE_INFORM , mHallAgent.RecvNotifyBankMessageInform );
			mConvertRecvMap.Add (FiEventType.RECV_GET_BANKMSG_RESPONSE, mHallAgent.RecvBankMessageResponse);
			mConvertRecvMap.Add (FiEventType.RECV_EXCHANGE_CHARM_RESPONSE, mHallAgent.RecvExchangeCharmResponse);
			mConvertRecvMap.Add (FiEventType.RECV_CL_GIVE_RESPONSE, mHallAgent.RecvGiveCharmResponse);
			mConvertRecvMap.Add (FiEventType.RECV_CL_RELOAD_ASSET_INFO_RESPONSE, mHallAgent.RecvReloadAssetResponse);
			mConvertRecvMap.Add (FiEventType.RECV_NOTIFY_GIVE_CHARM, mHallAgent.RecvGiveCharmInform);
			mConvertRecvMap.Add (FiEventType.RECV_NOTIFY_SCROLLING_UPDATE, mHallAgent.RecvScollNotice);
			mConvertRecvMap.Add (FiEventType.RECV_EXCHANGE_DIAMOND_RESPONSE, mHallAgent.RecvExchageDiamondResponse);
			mConvertRecvMap.Add (FiEventType.RECV_NOTIFY_EXCHANGE_DIAMOND, mHallAgent.RecvNotifyOtherDiamondChangeResponse);
			mConvertRecvMap.Add (FiProtoType.FISHING_LOGIN_RES, mLoginAgent.RecvLoginResponse);
			mConvertRecvMap.Add (FiProtoType.YK_CONVERSION_RESPONSE, mHallAgent.RecvConvertFormalAccountResponse);
			mConvertRecvMap.Add (FiProtoType.CL_MODIFY_NICK_RESPONSE, mHallAgent.RecvModifyNickResponse);
			mConvertRecvMap.Add (FiProtoType.CL_HELP_GOLD_TASK, mHallAgent.RecvHelpGoldTaskData);
			mConvertRecvMap.Add (FiProtoType.CL_GET_HELP_TASK_REWARD_RESPONSE, mHallAgent.RecvHelpGoldTaskDataReward);
			mConvertRecvMap.Add (FiProtoType.CL_SIGNRETROACTIVE_REWARD_RESPONSE, mHallAgent.RecvSignRetroactiveResponse);
			mConvertRecvMap.Add (FiProtoType.CL_BIND_PHONE_STATE_RESPONSE, mHallAgent.RecvBindPhoneStartResponse);    
			//限时道具
			mConvertRecvMap.Add (FiProtoType.CL_USE_PROP_TIME_RESPONSE, mHallAgent.RecvUsePropTimeResponse);
			mConvertRecvMap.Add (FiProtoType.CL_DEL_PROP_TIME_RESPONSE, mHallAgent.RecvDelPropTimeResponse);
			//获取所有显示道具
			mConvertRecvMap.Add (FiProtoType.CL_GET_ALL_PROP_TIME_RESPONSE, mHallAgent.RecvGetAllPropTimeResponse);
			//接收奖励信息
			mConvertRecvMap.Add (FiProtoType.XL_GET_REWARD_INFO, mHallAgent.RecvGetRewardInfoResponse);
			//接收龙卡信息
			mConvertRecvMap.Add (FiProtoType.CL_DARGON_CARD_RESPONSE, mHallAgent.RecvGetDraGonCardInfoResponse);
			//接收龙卡购买后的信息
			mConvertRecvMap.Add (FiProtoType.CL_PURCHASE_DARGON_CARD_RESPONSE, mHallAgent.RecvPurchaseGetDraGonCardInfoResponse);
			//接收特惠数据
			mConvertRecvMap.Add (FiProtoType.XL_MONTHLY_GET_GIFT_BAG_INFO, mHallAgent.RecvGetPreferentialInfoResponse);
			//购买成功后接收特惠数据
			mConvertRecvMap.Add (FiProtoType.FISHING_NOTIFY_BUY_SALE_GIFTBAG, mHallAgent.RecvGetPurPreferentialInfoResponse);
			//购买炮座接收数据
			mConvertRecvMap.Add (FiProtoType.XL_GET_EXCHANGEBARBETTE_RESPONSE, mHallAgent.RecvExchangeBarbetteResponse);
			//更换炮座接收数据
			mConvertRecvMap.Add (FiProtoType.XL_CHANGEBARBETTESTYLE_RESPONSE, mHallAgent.RecvChangeBarbetteStyleResponse);
			//接受财神元宝点亮信息
			mConvertRecvMap.Add (FiProtoType.CL_MANMONSTARTSHOW_RESPONSE, mHallAgent.RecvManmonStartResponse);
			//接受财神赌注数据信息
			mConvertRecvMap.Add (FiProtoType.CL_MANMONBETTINGSHOW_RESPONSE, mHallAgent.RecvManmonSettingResponse);
			//下注请求数据信息
			mConvertRecvMap.Add (FiProtoType.CL_MANMONBETTING_RESPONSE, mHallAgent.RecvManmonBettingResponse);
			//摇钱树请求数据信息
			mConvertRecvMap.Add (FiProtoType.CL_MANMONYAOQIANSHU_RESPONSE, mHallAgent.RecvManmoneYaoQianShuResponse);
			//转盘奖池请求信息
			mConvertRecvMap.Add (FiProtoType.XL_GET_LONG_POOL_REWARD_INFO, mHallAgent.RecvShenlongTuanTableResponse);
			//财神排名奖励请求信息
			mConvertRecvMap.Add (FiProtoType.XL_SENMANMON_RANKREWARD_RESPONSE, mHallAgent.RecvManmomRankRewardResponse);
			//
			mConvertRecvMap.Add (FiProtoType.XL_GET_LONG_LIU_SHUI_GOLD, mHallAgent.RecvShenLongTurnTableLiuShuiResponse);
			//
			mConvertRecvMap.Add (FiEventType.RECV_FISH_LUCKY_DRAW_RESPONSE, mHallAgent.RecvShenlongTurnTableLuckDrawResponse);
			mConvertRecvMap.Add (FiProtoType.XL_CHANGE_LIU_SHUI_TIME, mHallAgent.RecvShenLongChangeLiuShuiTime);
			mConvertRecvMap.Add (FiProtoType.FISHING_CANCEL_SKILLS, mHallAgent.RecvCancelOtherSkill);
			mConvertRecvMap.Add (FiProtoType.FISHING_UPDATE_WINER_RANK_INFO, mHallAgent.RecvFishingUserRank);
			mConvertRecvMap.Add (FiProtoType.XL_SELF_UPDATE_RANK_INFO, mHallAgent.RecvMyFishingUserRank);
			mConvertRecvMap.Add (FiProtoType.XL_SELF_WINTIME_COUNT_INFO, mHallAgent.RecvWinTimeResult);
			//赋值金币操作
			mConvertRecvMap.Add (FiProtoType.XL_CHANGEUSERGOLD_RESPONSE, mFishingAgent.RecvChangeUserGold);
			//boss场匹配消息
			mConvertRecvMap.Add (FiProtoType.XL_BOSSROOMATCH_RESPONSE, mHallAgent.RecvBossRoomMatch);
			//boss匹配接受消息
			mConvertRecvMap.Add (FiProtoType.XL_BOSSROOMSIGNUP_RESPONSE, mHallAgent.RecvNotifySignUp);
			//接收引导进入boss场消息
			mConvertRecvMap.Add (FiProtoType.XL_ENTER_BOSS_ROOM_MESSAGE_RESPONSE, mHallAgent.RecvIntoBossRoom);
			//接收boss猎杀排名
			mConvertRecvMap.Add (FiProtoType.FISHING_GET_BOSS_MATCH_RANK_INFO, mHallAgent.RecvGetBossKillRank);
			//接收自己boss猎杀
			mConvertRecvMap.Add (FiProtoType.XL_GET_BOSS_MATCH_RANK_RESPONSE, mHallAgent.RecvGetBossKillMyRank);
			//接收Boss时间
			mConvertRecvMap.Add (FiProtoType.XL_UPDATEBOSSMATCHTIME_RESPONSE, mFishingAgent.RecvUpdateBossMatchTime);
			//接受Boss比赛结束排行榜
			mConvertRecvMap.Add (FiProtoType.XL_BOSS_MATCH_LEAVE_MESSAGE_RESPONSE, mHallAgent.RecvGetBossMatchResultRank);
			//接受荣耀排位数据
			mConvertRecvMap.Add (FiProtoType.XL_API_WEI_SAI_RANK_RESPONSE, mHallAgent.RecvHoroDatasResultRank);
			//接受荣誉殿堂排名
			mConvertRecvMap.Add (FiProtoType.XL_RONGYUDIANTANG_RANK_RESPONSE, mHallAgent.RecvRongYuRankInfo);
			//接收奖励信息
			mConvertRecvMap.Add (FiProtoType.XL_GET_PAI_WEI_REWARD_INFO_RESPONSE, mHallAgent.RecvPaiWeiPrize);
			//接收奖励状态
			mConvertRecvMap.Add (FiProtoType.XL_GET_PAI_WEI_REWARD_RESPONSE, mHallAgent.RecvPaiWeiPrizeInfo);
			//接收荣耀奖励
			mConvertRecvMap.Add (FiProtoType.XL_RECV_RONGYAO_PRIZE_RESPONSE, mHallAgent.RecvRongYaoPrize);
			//接收购买给力 双喜 宝藏 状态协议
			mConvertRecvMap.Add (FiProtoType.XL_GET_TOP_UP_GIFT_BAG_STATE_INFO_NEW, mHallAgent.RecvGiftBagResultRank);
			//接收七日礼包初始协议
			mConvertRecvMap.Add (FiProtoType.XL_GET_SEVEN_DAY_INIT_INFO, mHallAgent.RecvSevenGiftBagResultRank);
			//接收七日礼包奖励领取协议
			mConvertRecvMap.Add (FiProtoType.XL_GET_SEVENDAY_BAG_STATE_INFO_NEW, mHallAgent.RecvSevenGiftRewardBagResult);
			//接收获取活动 升级任务进度信息
			mConvertRecvMap.Add(FiProtoType.XL_GET_UP_LEVEL_ACTIVY_INFO, mHallAgent.RecvUserLevelUpInfo);
			//接收领取活动 升级活动 奖励
			mConvertRecvMap.Add(FiProtoType.XL_GET_UP_LEVEL_BAG_STATE, mHallAgent.RecvUserLevelUpReward);
			//接收付款、金幣資訊
			mConvertRecvMap.Add(FiProtoType.XL_GET_PAY_INFO, mHallAgent.RecvPayInfo);
			//接收手機綁定
			mConvertRecvMap.Add(FiEventType.RECV_XL_GET_BIND_PHONE_RESPONSE, mHallAgent.RecvConvertFormalBindPhoneResponse);
			//接收按钮状态协议
			mConvertRecvMap.Add(FiProtoType.XL_GET_BUTTON_HIDE_STATE, mHallAgent.RecvButtonState);
			mConvertRecvMap.Add (FiEventType.RECV_GET_PAY_STATE_RESPONSE, ( byte[] data) => {
				try {
					GetPayStateByNoResponse nResponse = GetPayStateByNoResponse.Parser.ParseFrom (data);
					//Debug.LogError( "----RECV_GET_PAY_STATE_RESPONSE----" + nResponse );
					mNetCtrl.dispatchEvent (FiEventType.RECV_GET_PAY_STATE_RESPONSE, nResponse);
				} catch {
					Debug.LogError ("RECV_GET_PAY_STATE_RESPONSE error");
				}
			});



			mConvertRecvMap.Add (FiEventType.RECV_GET_FIRST_PAY_REWARD_RESPONSE, ( byte[] data) => {
				try {
					GetFirstPayRewardResponse nResponse = GetFirstPayRewardResponse.Parser.ParseFrom (data);
					//Debug.LogError( "----RECV_GET_FIRST_PAY_REWARD_RESPONSE----" + nResponse );
					mNetCtrl.dispatchEvent (FiEventType.RECV_GET_FIRST_PAY_REWARD_RESPONSE, nResponse);
				} catch {
					Debug.LogError ("RECV_GET_FIRST_PAY_REWARD_RESPONSE error");
				}
			});

			mConvertRecvMap.Add (FiEventType.RECV_GET_ROBOT_RESPONSE, ( byte[] data) => {
				try {
					GetRobotResponse nResponse = GetRobotResponse.Parser.ParseFrom (data);
					//Debug.LogError( "----RECV_GET_FIRST_PAY_REWARD_RESPONSE----" + nResponse );
					mNetCtrl.dispatchEvent (FiEventType.RECV_GET_ROBOT_RESPONSE, nResponse);
				} catch {
					Debug.LogError ("RECV_GET_ROBOT_RESPONSE error");
				}
			});


			mConvertRecvMap.Add (FiEventType.RECV_NOTIFY_PURCHASE_PROPERTY, ( byte[] data) => {
				try {
					NotifyOtherPurchaseProperty nResponse = NotifyOtherPurchaseProperty.Parser.ParseFrom (data);
					mNetCtrl.dispatchEvent (FiEventType.RECV_NOTIFY_PURCHASE_PROPERTY, nResponse);
				} catch {
					Debug.LogError ("RECV_NOTIFY_PURCHASE_PROPERTY error");
				}
			});

            //手機登入客戶端接收協議
            mConvertRecvMap.Add(FiProtoType.XL_GET_PHONE_LOGIN, mHallAgent.RecvPhoneLoginAccount);
            //手機號碼設定密碼 客戶端接收協議
            mConvertRecvMap.Add(FiProtoType.XL_GET_PHONE_PASSWORD, mHallAgent.RecvPhoneNumberPass);
            //根據關聯帳號選擇要登入的帳號回傳token
            mConvertRecvMap.Add(FiProtoType.XL_CHOISE_ACCOUNT_LOGIN_RESPON, mHallAgent.RecvChoseAccountAssociate);
			//回傳接收到的暱稱選項
			mConvertRecvMap.Add(FiProtoType.XL_GET_NEW_NICK_RESPON, mHallAgent.RecvNickname);

			//mConvertRecvMap.Add(FiEventType.RECV_FISH_LUCKY_DRAW_RESPONSE, (byte[] data) =>
			//{
			//    try
			//    {
			//        GetFishLuckyDrawResponse nReps = GetFishLuckyDrawResponse.Parser.ParseFrom(data);
			//        mNetCtrl.dispatchEvent(FiEventType.RECV_FISH_LUCKY_DRAW_RESPONSE, nReps);
			//    }
			//    catch
			//    {
			//        Debug.LogError("RECV_FISH_LUCKY_DRAW_RESPONSE");
			//    }
			//});

		}

	}
}

