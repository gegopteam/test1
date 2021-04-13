using System;
using UnityEngine;
using System.Collections;
using System.Runtime.InteropServices;
using System.Net;
using System.Net.Sockets;
using System.Threading;
using System.IO;
using System.Collections.Generic;
using Google.Protobuf;
using ProtoBuf;

/*
 * 2020/03/04 Joey 新增 RecvBroadCastNewUserLevelup() 取得新手升級獎勵公告
 */
namespace AssemblyCSharp
{
	public class FiHallMsgAgent: MsgBaseAgent
	{
		const bool isDebugOutOn = false;

		public FiHallMsgAgent (NetControl nValue) : base ()
		{
			SetDispatch (nValue);
		}

		public void RecvGiveCharmInform (byte[] data)
		{
			try {
				PB_NotifyGiveCharm nResponse = PB_NotifyGiveCharm.Parser.ParseFrom (data);
				FiGiveCharmInform nResponseOut = new FiGiveCharmInform ();
				if (nResponse.Msg != null) {
					nResponseOut.data.bankChanged = nResponse.Msg.BankChanged;
					nResponseOut.data.charmChanged = nResponse.Msg.CharmChanged;
					nResponseOut.data.dateTime = nResponse.Msg.DateTime;
					nResponseOut.data.giftCount = nResponse.Msg.GiftCount;
					nResponseOut.data.giftGold = nResponse.Msg.GiftGold;
					nResponseOut.data.nickname = nResponse.Msg.Nickname;
					nResponseOut.data.type = nResponse.Msg.Type;
					nResponseOut.data.userId = nResponse.Msg.UserId;
				}
				Dispatch (FiEventType.RECV_NOTIFY_GIVE_CHARM, nResponseOut);
			} catch {

			}
		}


		public void RecvSetBankPswdResponse (byte[] data)
		{
			try {

				PB_SetBankPswdResponse nResponse = PB_SetBankPswdResponse.Parser.ParseFrom (data);
				FiSetBankPswdResponse nResponseOut = new FiSetBankPswdResponse ();

				nResponseOut.result = nResponse.Result;
				Dispatch (FiEventType.RECV_SET_PSWD_RESPONSE, nResponseOut);
			} catch {

			}
		}

		public void RecvExchageDiamondResponse (byte[] data)
		{
			try {
				ExchangeDiamondResponse nOutput = ExchangeDiamondResponse.Parser.ParseFrom (data);
				//Debug.LogError ( "-=---2---------" + nOutput );
				Dispatch (FiEventType.RECV_EXCHANGE_DIAMOND_RESPONSE, nOutput);
			} catch {
				Debug.LogError ("RecvExchageDiamondResponse error");
			}
		}

		public void RecvNotifyOtherDiamondChangeResponse (byte[] data)
		{
			try {
				NotifyExchangeDiamond nOutput = NotifyExchangeDiamond.Parser.ParseFrom (data);
				//Debug.LogError ( "-=---2---------" + nOutput );
				Dispatch (FiEventType.RECV_NOTIFY_EXCHANGE_DIAMOND, nOutput);
			} catch {
				Debug.LogError ("RecvExchageDiamondResponse error");
			}
		}

		public void RecvScollNotice (byte[] data)
		{
			try {
				PB_NotifyScrollingNoticesUpdate nResponse = PB_NotifyScrollingNoticesUpdate.Parser.ParseFrom (data);
				//Debug.LogError( "PB_NotifyScrollingNoticesUpdate------" + nResponse );
				FiNotifyScrollingNoticesUpdate nResponseOut = new FiNotifyScrollingNoticesUpdate ();

				IEnumerator<PB_ScrollingNotice> nNoticeMsg = nResponse.Notices.GetEnumerator ();
				while (nNoticeMsg.MoveNext ()) {
					FiScrollingNotice nUnit = new FiScrollingNotice ();
					nUnit.content = nNoticeMsg.Current.Content;
					nUnit.cycleInterval = nNoticeMsg.Current.CycleInterval;
					nResponseOut.noticeArray.Add (nUnit);
				}
				Dispatch (FiEventType.RECV_NOTIFY_SCROLLING_UPDATE, nResponseOut);
			} catch {

			}
		}

		public void RecvReloadAssetResponse (byte[] data)
		{
			try {
				PB_ReloadAssetInfoResponse nResponse = PB_ReloadAssetInfoResponse.Parser.ParseFrom (data);
				Dispatch (FiEventType.RECV_CL_RELOAD_ASSET_INFO_RESPONSE, nResponse);
			} catch {

			}
		}

		public void RecvGiveCharmResponse (byte[] data)
		{
			try {
				PB_CLGiveGiftResponse nResponse = PB_CLGiveGiftResponse.Parser.ParseFrom (data);
				FiCLGiveGiftResponse nResponseOut = new FiCLGiveGiftResponse ();
				//Debug.LogError( "start ----error--" + nResponse );
				nResponseOut.currentCount = nResponse.CurrentCount;
				nResponseOut.errorMsg = nResponse.ErrorMsg;
				if (nResponse.Gift != null) {
					nResponseOut.gift = new FiProperty ();
					nResponseOut.gift.type = nResponse.Gift.PropertyType;
					nResponseOut.gift.value = nResponse.Gift.Sum;
				}
				nResponseOut.giveType = nResponse.GiveType;
				nResponseOut.result = nResponse.Result;
				Dispatch (FiEventType.RECV_CL_GIVE_RESPONSE, nResponseOut);
			} catch {
				Debug.LogError ("start ----error--");
			}
		}


		public void RecvBankAccessResponse (byte[] data)
		{
			try {

				PB_BankAccessResponse nResponse = PB_BankAccessResponse.Parser.ParseFrom (data);
				FiBankAccessResponse nResponseOut = new FiBankAccessResponse ();

				nResponseOut.gold = nResponse.Gold;
				nResponseOut.result = nResponse.Result;
				nResponseOut.errorMsg = nResponse.ErrMsg;
				Dispatch (FiEventType.RECV_BANK_ACCESS_RESPONSE, nResponseOut);
			} catch {

			}
		}


		public void RecvExchangeCharmResponse (byte[] data)
		{
			try {

				PB_ExchangeCharmResponse nResponse = PB_ExchangeCharmResponse.Parser.ParseFrom (data);
				//Debug.LogError( "---PB_ExchangeCharmResponse-----" + nResponse );
				FiExchangeCharmResponse nResponseOut = new FiExchangeCharmResponse ();

				nResponseOut.bankGold = nResponse.BankGold;
				nResponseOut.charm = nResponse.Charm;
				nResponseOut.result = nResponse.Result;
				nResponseOut.errorMsg = nResponse.ErrMsg;
				Dispatch (FiEventType.RECV_EXCHANGE_CHARM_RESPONSE, nResponseOut);
			} catch {

			}
		}


		/*public void RecvNotifyBankMessageInform( byte[] data )
		{
			try{

				PB_NotifyBankMessage nResponse = PB_NotifyBankMessage.Parser.ParseFrom( data );
				FiBankMessageInform nResponseOut = new FiBankMessageInform();
				nResponseOut.data.bankChanged = nResponse.Msg.BankChanged;
				nResponseOut.data.charmChanged = nResponse.Msg.CharmChanged;
				nResponseOut.data.dateTime = nResponse.Msg.DateTime;
				nResponseOut.data.giftCount = nResponse.Msg.GiftCount;
				nResponseOut.data.giftGold = nResponse.Msg.GiftGold;
				nResponseOut.data.nickname = nResponse.Msg.Nickname;
				nResponseOut.data.type     = nResponse.Msg.Type;
				nResponseOut.data.userId   = nResponse.Msg.UserId;

				//Dispatch( FiEventType.RECV_NOTIFY_BANK_MESSGAE_INFORM , nResponseOut );
			}catch{

			}
		}*/

			

		public void RecvBankMessageResponse (byte[] data)
		{
			try {
			
				PB_GetBankMessageResponse nResponse = PB_GetBankMessageResponse.Parser.ParseFrom (data);
				FiGetBankMessageResponse nResponseOut = new FiGetBankMessageResponse ();
				nResponseOut.reuslt = nResponse.Result;
				if (nResponse.Messages != null) {
					IEnumerator<PB_BankMessage> nBankMsg = nResponse.Messages.GetEnumerator ();
					while (nBankMsg.MoveNext ()) {
						FiBankMessageInfo nInfo = new FiBankMessageInfo ();
						nInfo.bankChanged = nBankMsg.Current.BankChanged;
						nInfo.charmChanged = nBankMsg.Current.CharmChanged;
						nInfo.dateTime = nBankMsg.Current.DateTime;
						nInfo.giftCount = nBankMsg.Current.GiftCount;
						nInfo.giftGold = nBankMsg.Current.GiftGold;
						nInfo.nickname = nBankMsg.Current.Nickname;
						nInfo.type = nBankMsg.Current.Type;
						nInfo.userId = nBankMsg.Current.UserId;
						nResponseOut.messages.Add (nInfo);
					}
				}
				Dispatch (FiEventType.RECV_GET_BANKMSG_RESPONSE, nResponseOut);
			} catch {
			
			}
		}



		public void RecvLuckyDrawInform (byte[] data)
		{
			try {
				PB_NotifyFishLuckyDraw nInfoIn = PB_NotifyFishLuckyDraw.Parser.ParseFrom (data);
				FiFishLuckyDrawInform nInfoOut = new FiFishLuckyDrawInform ();
				nInfoOut.type = nInfoIn.Type;
				nInfoOut.userId = nInfoIn.UserId;
				if (nInfoIn.Prop != null) {
					nInfoOut.property.type = nInfoIn.Prop.PropertyType;
					nInfoOut.property.value = nInfoIn.Prop.Sum;
				}
				Dispatch (FiEventType.RECV_NOTIFY_FISH_LUCKY_DRAW, nInfoOut);
			} catch {

			}
		}

		public void RecvLuckyDrawResponse (byte[] data)
		{
			try {
				PB_GetFishLuckyDrawResponse nInfoIn = PB_GetFishLuckyDrawResponse.Parser.ParseFrom (data);
				FiGetFishLuckyDrawResponse nInfoOut = new FiGetFishLuckyDrawResponse ();
				nInfoOut.type = nInfoIn.Type;
				nInfoOut.result = nInfoIn.Result;
				if (nInfoIn.Prop != null) {
					nInfoOut.property.type = nInfoIn.Prop.PropertyType;
					nInfoOut.property.value = nInfoIn.Prop.Sum;
				}
				Dispatch (FiEventType.RECV_FISH_LUCKY_DRAW_RESPONSE, nInfoOut);
			} catch {

			}
		}

		//收到公告通知
		public void RecvBroadCastMsgInform (byte[] data)
		{
			try {
				PB_NotifyBroadcastUserMessage nInfoIn = PB_NotifyBroadcastUserMessage.Parser.ParseFrom (data);
				FiBroadCastUserMsgInfrom nInfoOut = new FiBroadCastUserMsgInfrom ();
				nInfoOut.content = nInfoIn.Content;
				nInfoOut.nickname = nInfoIn.Nickname;
				Dispatch (FiEventType.RECV_NOTIFY_BROADCAST_USER_MESSAGE, nInfoOut);
			} catch {

			}
		}


		//玩家发起公告回复
		public void RecvBroadCastMsgResponse (byte[] data)
		{
			try {
				PB_BroadcastUserMessageResponse nInfoIn = PB_BroadcastUserMessageResponse.Parser.ParseFrom (data);
				FiBroadCastUserMsgResponse nInfoOut = new FiBroadCastUserMsgResponse ();
				nInfoOut.result = nInfoIn.Result;
				Dispatch (FiEventType.RECV_BROADCAST_USER_MESSAGE_RESPONSE, nInfoOut);
			} catch {

			}
		}

		//收到游戏公告
		public void RecvBroadCastGameInfo (byte[] data)
		{
			try {
				PB_NotifyBroadcastGameInfo nInfoIn = PB_NotifyBroadcastGameInfo.Parser.ParseFrom (data);
				FiBroadCastGameInfo nInfoOut = new FiBroadCastGameInfo ();
				nInfoOut.content = nInfoIn.Content;
				nInfoOut.type = nInfoIn.Type;
//				UnityEngine.Debug.LogError ("intytype" + nInfoIn.Type);
				Dispatch (FiEventType.RECV_NOTIFY_BROADCAST_GAME_INFO, nInfoOut);
			} catch {
				
			}
		}

		//收到七日公告
		public void RecvBroadCastSevenInfo (byte[] data)
		{
			try {
				PB_NotifyBroadcastSevenDayinfo nInfoIn = PB_NotifyBroadcastSevenDayinfo.Parser.ParseFrom (data);
				FiBroadCastSevenInfo nInfoOut = new FiBroadCastSevenInfo ();
				nInfoOut.content = nInfoIn.Content;
				nInfoOut.type = nInfoIn.Type;
				Debug.LogError ("七日訊息公告 intytype = " + nInfoIn.Type);
				Debug.LogError("七日訊息公告 intytype = " + nInfoIn.Content);
				Dispatch (FiEventType.RECV_FISHING_GET_SEVEN_REWARD_NOTIFY, nInfoOut);
			} catch {

			}
		}

		//收到新手升級獎勵公告
		public void RecvBroadCastNewUserLevelup(byte[] data)
		{
			try
			{
				PB_NotifyBroadcastSevenDayinfo nInfoIn = PB_NotifyBroadcastSevenDayinfo.Parser.ParseFrom(data);
				FiBroadCastUpgradeInfo nInfoOut = new FiBroadCastUpgradeInfo();
				nInfoOut.content = nInfoIn.Content;
				nInfoOut.type = nInfoIn.Type;
				Debug.LogError("新手升級獎勵公告 = " + nInfoIn.Type);
				Debug.LogError("新手升級獎勵公告 = " + nInfoIn.Content);
				Dispatch(FiEventType.RECV_NEW_USER_LEVELUP_NOTIFY, nInfoOut);
			}
			catch
			{

			}
		}

		//新手任务奖励反馈
		public void RecvBeginnerTaskRewardResponse (byte[] data)
		{
			try {
				PB_BeginnerTaskRewardResponse nResponseIn = PB_BeginnerTaskRewardResponse.Parser.ParseFrom (data);
				FiBeginnerTaskRewardResponse nResponseOut = new FiBeginnerTaskRewardResponse ();
				nResponseOut.beginnerCurTask = nResponseIn.BeginnerCurTask;
				if (nResponseIn.Properties != null) {
					nResponseOut.properties.type = nResponseIn.Properties.PropertyType;
					nResponseOut.properties.value = nResponseIn.Properties.Sum;
				}
				nResponseOut.result = nResponseIn.Result;
				Dispatch (FiEventType.RECV_BEGINNER_TASK_REWARD_RESPONSE, nResponseOut);
			} catch (Exception e) {
				Debug.LogError ("[ message parse ] RecvBeginnerTaskRewardResponse error !!! ");
			}
		}

		//其他玩家获取新手任务奖励反馈
		public void RecvOtherBeginnerTaskRewardInform (byte[] data)
		{
			try {
				PB_NotifyOtherBeginnerReward nResponseIn = PB_NotifyOtherBeginnerReward.Parser.ParseFrom (data);
				FiNotifyOtherBeginnerReward nResponseOut = new FiNotifyOtherBeginnerReward ();
				nResponseOut.beginnerCurTask = nResponseIn.BeginnerCurTask;
				if (nResponseIn.Property != null) {
					nResponseOut.property.type = nResponseIn.Property.PropertyType;
					nResponseOut.property.value = nResponseIn.Property.Sum;
				}
				nResponseOut.userId = nResponseIn.UserId;
				Dispatch (FiEventType.RECV_NOTIFY_OTHER_BEGINNER_REWARD, nResponseOut);
			} catch (Exception e) {
				Debug.LogError ("[ message parse ] RecvOtherBeginnerTaskRewardInform error !!! ");
			}
		}

		//新手任务进度通知
		public void RecvBeginnerTaskProgress (byte[] data)
		{
			try {
				PB_NotifyBeginnerTaskProgress nResponseIn = PB_NotifyBeginnerTaskProgress.Parser.ParseFrom (data);
				FiNotifyBeginnerTaskProgress nResponseOut = new FiNotifyBeginnerTaskProgress ();
				nResponseOut.beginnerCurTask = nResponseIn.BeginnerCurTask;
				nResponseOut.beginnerTaskProgress = nResponseIn.BeginnerTaskProgress;
				Dispatch (FiEventType.RECV_NOTIFY_BEGINNER_TASK_PROGRESS, nResponseOut);
			} catch (Exception e) {
				Debug.LogError ("[ message parse ] RecvBeginnerTaskProgress error !!! ");
			}
		}

		/*public void RecvBeginnerTaskInform( byte[] data )
		{
			try
			{
				PB_BeginnerTaskResponse nInformIn = PB_BeginnerTaskResponse.Parser.ParseFrom( data );
				FiBeginTaskInform nInformOut = new FiBeginTaskInform();
				nInformOut.currentTask = nInformIn.BeginnerCurTask;
				nInformOut.diamond     = nInformIn.BeginnerTaskDiamond;
				Dispatch( FiEventType.RECV_BEGINNER_TASK_REWARD_RESPONSE , nInformOut );
			}catch( Exception e )
			{
				
			}
		}*/

		public void RecvOtherReconnectInform (byte[] data)
		{
			try {
				PB_OtherReconnectGame nInform = PB_OtherReconnectGame.Parser.ParseFrom (data);
				FiOtherReconnectPKGameInform nInformOut = new FiOtherReconnectPKGameInform ();
				nInformOut.goldType = nInform.GoldType;
				nInformOut.roomIndex = nInform.RoomIndex;
				nInformOut.roomType = nInform.RoomType;
				if (nInform.Other != null) {
					nInformOut.other.avatar = nInform.Other.Avatar;
					nInformOut.other.gender = nInform.Other.Gender;
					nInformOut.other.nickname = nInform.Other.Nickname;
					nInformOut.other.seatIndex = nInform.Other.SeatIndex;
					nInformOut.other.userId = nInform.Other.UserId;
					nInformOut.other.vipLevel = nInform.Other.VipLevel;
					if (nInform.Other.Properties != null) {
						IEnumerator< PB_Property > nEumPropty = nInform.Other.Properties.GetEnumerator ();
						while (nEumPropty.MoveNext ()) {
							FiProperty nProp = new FiProperty ();
							nProp.type = nEumPropty.Current.PropertyType;
							nProp.value = nEumPropty.Current.Sum;
							nInformOut.other.properties.Add (nProp);
						}
					}
				}
				Dispatch (FiEventType.RECV_OTHER_RECONNECT_PKGAME_INFORM, nInformOut);
			} catch {
				Debug.LogError ("[ network ] recv message== RECV_OTHER_RECONNECT_PKGAME_INFORM error");
			}
		}


		public void RecvMonthlyPackResponse (byte[] data)
		{
			try {
				PB_GetMonthlyPackResponse nResponseIn = PB_GetMonthlyPackResponse.Parser.ParseFrom (data);
				FiGetMonthlyPackResponse nResponseOut = new FiGetMonthlyPackResponse ();
				nResponseOut.result = nResponseIn.Result;

				if (nResponseIn.Properties != null) {
					IEnumerator< PB_Property > nEumPropty = nResponseIn.Properties.GetEnumerator ();
					while (nEumPropty.MoveNext ()) {
						FiProperty nProperty = new FiProperty ();
						nProperty.type = nEumPropty.Current.PropertyType;
						nProperty.value = nEumPropty.Current.Sum;
						nResponseOut.properties.Add (nProperty);
					}
				}
				Dispatch (FiEventType.RECV_GET_MONTHLY_PACK_RESPONSE, nResponseOut);
			} catch {
				Debug.LogError ("[ network ] recv message== RECV_GET_MONTHLY_PACK_RESPONSE error");
			}
		}

		public void RecvUserChatInform (byte[] data)
		{
			try {
				PB_NotifyRoomChatMessage nResponseIn = PB_NotifyRoomChatMessage.Parser.ParseFrom (data);
				FiChatMessage nResponseOut = new FiChatMessage ();
				nResponseOut.message = nResponseIn.Message;
				nResponseOut.userId = nResponseIn.UserId;
				//Debug.LogError( "----------RecvUserChatInform-----------" + nResponseIn );
				Dispatch (FiEventType.RECV_ROOM_CHAT_MESSAGE, nResponseOut);
			} catch {
				Debug.LogError ("[ network ] recv message== RECV_OPEN_PACK_RESPONSE error");
			}
		}

		public void RecvOpenPackResponse (byte[] data)
		{
			try {
				PB_OpenPackResponse nResponseIn = PB_OpenPackResponse.Parser.ParseFrom (data);
				FiOpenPackResponse nResponseOut = new FiOpenPackResponse ();
				nResponseOut.packId = nResponseIn.PackId;
				nResponseOut.result = nResponseIn.Result;
				if (nResponseIn.Properties != null) {
					IEnumerator< PB_Property > nEumPropty = nResponseIn.Properties.GetEnumerator ();
					while (nEumPropty.MoveNext ()) {
						FiProperty nProperty = new FiProperty ();
						nProperty.type = nEumPropty.Current.PropertyType;
						nProperty.value = nEumPropty.Current.Sum;
						nResponseOut.properties.Add (nProperty);
					}
				}
				Dispatch (FiEventType.RECV_OPEN_PACK_RESPONSE, nResponseOut);
			} catch {
				Debug.LogError ("[ network ] recv message== RECV_OPEN_PACK_RESPONSE error");
			}
		}

		public void RecvTopUpInform (byte[] data)
		{
			try {
				PB_NotifyTopUp nInformIn = PB_NotifyTopUp.Parser.ParseFrom (data);
				FiTopUpInform nInformOut = new FiTopUpInform ();
				nInformOut.property.type = nInformIn.Property.PropertyType;
				nInformOut.property.value = nInformIn.Property.Sum;
				nInformOut.userId = nInformIn.UserId;
				nInformOut.currentVip = nInformIn.CurrentVip;
				nInformOut.money = nInformIn.Money;

				//Debug.LogError( "--------------RECV_TOP_UP_INFORM----------------" + nInformIn );

				Dispatch (FiEventType.RECV_TOP_UP_INFORM, nInformOut);
			} catch (Exception e) {
				Tool.OutLogToFile ("[ network ] recv message== RECV_GET_USER_INFO_RESPONSE error" + e.Message);
			}
		}

		public void RecvSellPropertyResponse (byte[] data)
		{
			try {
				PB_SellPropertyResponse nInform = PB_SellPropertyResponse.Parser.ParseFrom (data);
				FiSellPropertyResponse nInformOut = new FiSellPropertyResponse ();
				nInformOut.gold = nInform.Gold;
				nInformOut.result = nInform.Result;

//				Debug.LogError( "------PB_SellPropertyResponse------" + nInform );

				IEnumerator< PB_Property > nEumPropty = nInform.Properties.GetEnumerator ();
				while (nEumPropty.MoveNext ()) {
					FiProperty nEntity = new FiProperty ();
					nEntity.type = nEumPropty.Current.PropertyType;
					nEntity.value = nEumPropty.Current.Sum;
					nInformOut.properties.Add (nEntity);
				}
				Dispatch (FiEventType.RECV_SELL_PROPERTY_RESPONSE, nInformOut);
			} catch (Exception e) {
				Tool.OutLogToFile ("[ network ] recv message== RECV_GET_USER_INFO_RESPONSE error" + e.Message);
			}
		}

		public void  RecvDecreaseConsumeInform (byte[] data)
		{
			try {
				PB_NotifyDecreaseConsumedGold nInform = PB_NotifyDecreaseConsumedGold.Parser.ParseFrom (data);
				FiDecreaseConsumedGoldInform nInformOut = new FiDecreaseConsumedGoldInform ();
				nInformOut.decreasedGold = nInform.DecreasedGold;
				Dispatch (FiEventType.RECV_DECREASE_CONSUMED_GOLD_INFORM, nInformOut);
			} catch (Exception e) {
				Tool.OutLogToFile ("[ network ] recv message== RECV_GET_USER_INFO_RESPONSE error" + e.Message);
			}
		}


		public void RecvGetUserInfoResponse (byte[] data)
		{
			try {
				PB_GetUserInfoResponse nResponseIn = PB_GetUserInfoResponse.Parser.ParseFrom (data);
				FiGetUserInfoResponse nResponseOut = new FiGetUserInfoResponse ();
				nResponseOut.reuslt = nResponseIn.Result;
				//Debug.LogError( "---------RecvGetUserInfoResponse---------" + nResponseIn );
				if (nResponseIn.User != null) {
					nResponseOut.nUserInfo = new FiUserInfo ();
					nResponseOut.nUserInfo.avatar = nResponseIn.User.Avatar;
					nResponseOut.nUserInfo.cannonStyle = nResponseIn.User.CannonStyle;
					nResponseOut.nUserInfo.cannonMultiple = nResponseIn.User.CurrentCannonRatio;
					nResponseOut.nUserInfo.diamond = nResponseIn.User.Diamond;
					nResponseOut.nUserInfo.experience = nResponseIn.User.Experience;
					nResponseOut.nUserInfo.gender = nResponseIn.User.Gender;
					nResponseOut.nUserInfo.gold = nResponseIn.User.Gold;
					nResponseOut.nUserInfo.level = nResponseIn.User.Level;
					nResponseOut.nUserInfo.maxCannonMultiple = nResponseIn.User.MaxCannonMultiple;
					nResponseOut.nUserInfo.nickName = nResponseIn.User.Nickname;
					nResponseOut.nUserInfo.prepared = nResponseIn.User.Prepared;
					nResponseOut.nUserInfo.seatIndex = nResponseIn.User.SeatIndex;
					nResponseOut.nUserInfo.userId = nResponseIn.User.UserId;
					nResponseOut.nUserInfo.vipLevel = nResponseIn.User.VipLevel;
					nResponseOut.nUserInfo.gameId = nResponseIn.User.GameId;
					nResponseOut.nUserInfo.userChampionsRank = nResponseIn.User.UserChampionsRank;
				}

				Dispatch (FiEventType.RECV_GET_USER_INFO_RESPONSE, nResponseOut);
			} catch (Exception e) {
				Tool.OutLogToFile ("[ network ] recv message== RECV_GET_USER_INFO_RESPONSE error" + e.Message);
			}
		}


		public void RecvSignInAwardResponse (byte[] data)
		{
			try {
				PB_SignInAwardResponse nResponseIn = PB_SignInAwardResponse.Parser.ParseFrom (data);
				FiSignInAwardResponse nResponseOut = new FiSignInAwardResponse ();
				nResponseOut.result = nResponseIn.Result;
				if (nResponseIn.SignIn != null) {
					nResponseOut.singIn.day = nResponseIn.SignIn.Day;
					nResponseOut.singIn.status = nResponseIn.SignIn.Status;
				}

				if (isDebugOutOn) {
					//Debug.LogError( "---------PB_SignInAwardResponse---------" + nResponseIn );
				}

				if (nResponseIn.Property != null) {
					IEnumerator< PB_Property > nEumPropty = nResponseIn.Property.GetEnumerator ();
					while (nEumPropty.MoveNext ()) {
						FiProperty nSingle = new FiProperty ();
						nSingle.type = nEumPropty.Current.PropertyType;
						nSingle.value = nEumPropty.Current.Sum;
						nResponseOut.properties.Add (nSingle);
					}
				}
				Dispatch (FiEventType.RECV_SIGN_IN_AWARD_RESPONSE, nResponseOut);
			} catch (Exception e) {
				Tool.OutLogToFile ("[ network ] recv message== RECV_SIGN_IN_AWARD_RESPONSE error" + e.Message);
			}
		}

		public void RecvLevelUpResponse (byte[] data)
		{
			try {
				PB_NotifyLevelUp nResponseIn = PB_NotifyLevelUp.Parser.ParseFrom (data);
				FiLevelUpInfrom nResponseOut = new FiLevelUpInfrom ();
				nResponseOut.experience = nResponseIn.Experience;
				nResponseOut.level = nResponseIn.Level;
				nResponseOut.userId = nResponseIn.UserId;
				nResponseOut.nextLevelExp = nResponseIn.NextLevelExp;
				IEnumerator<PB_Property> nEumPtr = nResponseIn.Properties.GetEnumerator ();
				while (nEumPtr.MoveNext ()) {
					FiProperty nProp = new FiProperty ();
					nProp.type = nEumPtr.Current.PropertyType;
					nProp.value = nEumPtr.Current.Sum;
					nResponseOut.properties.Add (nProp);
				}
				mNetCtrl.dispatchEvent (FiEventType.RECV_LEVEL_UP_INFORM, nResponseOut);
			} catch (Exception e) {
				Tool.OutLogToFile ("[ network ] recv message== RECV_ACCEPT_GIVE_RESPONSE error" + e.Message);
			}
		}

		public void RecvIosPaymentResponse (byte[] data)
		{
			try {
				PB_IosPayPropertyResponse nResponseIn = PB_IosPayPropertyResponse.Parser.ParseFrom (data);
				FiIosPayPropertyResponse nResponseOut = new FiIosPayPropertyResponse ();
				nResponseOut.result = nResponseIn.Result;
				nResponseOut.firstAward = nResponseIn.FirstAward;
				if (nResponseIn.Property != null) {
					nResponseOut.property.type = nResponseIn.Property.PropertyType;
					nResponseOut.property.value = nResponseIn.Property.Sum;
				}
				mNetCtrl.dispatchEvent (FiEventType.RECV_IOS_PAY_PROPERTY_RESPONSE, nResponseOut);
			} catch (Exception e) {
				Tool.OutLogToFile ("[ network ] recv message== RECV_ACCEPT_GIVE_RESPONSE error" + e.Message);
			}
		}


		public void RecvAcceptGiveResponse (byte[] data)
		{
			try {
				PB_GetGiveResponse nResponseIn = PB_GetGiveResponse.Parser.ParseFrom (data);
				FiAcceptPresentResponse nResponseOut = new FiAcceptPresentResponse ();
				nResponseOut.result = nResponseIn.Result;

				IEnumerator<PB_Property> nEum = nResponseIn.Property.GetEnumerator ();
				while (nEum.MoveNext ()) {
					FiProperty nProperty = new FiProperty ();
					nProperty.type = nEum.Current.PropertyType;
					nProperty.value = nEum.Current.Sum;

					nResponseOut.properties.Add (nProperty); 
				}
				mNetCtrl.dispatchEvent (FiEventType.RECV_ACCEPT_GIVE_RESPONSE, nResponseOut);
			} catch (Exception e) {
				Tool.OutLogToFile ("[ network ] recv message== RECV_ACCEPT_GIVE_RESPONSE error" + e.Message);
			}
		}

		public void RecvGetGiveRecordResponse (byte[] data)
		{
			try {
				PB_GiveRecordResponse nResponseIn = PB_GiveRecordResponse.Parser.ParseFrom (data);
				FiGetPresentRecordResponse nResponseOut = new FiGetPresentRecordResponse ();
				nResponseOut.result = nResponseIn.Result;
				IEnumerator<PB_GiveRecord> nEum = nResponseIn.Record.GetEnumerator ();
				while (nEum.MoveNext ()) {
					FiPresentRecord nRecord = new FiPresentRecord ();

					nRecord.avatar_url = nEum.Current.AvatarUrl;
					nRecord.id = nEum.Current.GiveId;
					nRecord.nickname = nEum.Current.Nickname;
					if (nEum.Current.Property != null) {
						nRecord.property.type = nEum.Current.Property.PropertyType;
						nRecord.property.value = nEum.Current.Property.Sum;
					}
					nRecord.timestamp = nEum.Current.Timestamp;
					nRecord.userid = nEum.Current.Userid;
					nResponseOut.records.Add (nRecord);
				}

				mNetCtrl.dispatchEvent (FiEventType.RECV_GET_GIVE_RECORD_RESPONSE, nResponseOut);
			} catch (Exception e) {
				Tool.OutLogToFile ("[ network ] recv message== RECV_GET_GIVE_RECORD_RESPONSE error" + e.Message);
			}
		}


		public void RecvProcessSystemMailResponse (byte[] data)
		{
			try {
				PB_DelMailGetAwardResponse nResponseIn = PB_DelMailGetAwardResponse.Parser.ParseFrom (data);
				FiGetMailAwardsAndDeleteResponse nResponseOut = new FiGetMailAwardsAndDeleteResponse ();
				nResponseOut.result = nResponseIn.Result;

				//Debug.LogError( "---------PB_DelMailGetAwardResponse---------" + nResponseIn );

				IEnumerator<long> nMailEum = nResponseIn.MailId.GetEnumerator ();
				while (nMailEum.MoveNext ()) {
					nResponseOut.mailId.Add (nMailEum.Current);
				}
				IEnumerator<PB_Property> nEum = nResponseIn.Property.GetEnumerator ();
				while (nEum.MoveNext ()) {
					FiProperty nProperty = new FiProperty ();
					nProperty.type = nEum.Current.PropertyType;
					nProperty.value = nEum.Current.Sum;
					nResponseOut.property.Add (nProperty); 
				}
				mNetCtrl.dispatchEvent (FiEventType.RECV_PROCESS_SYSTEM_MAIL_RESPONSE, nResponseOut);
			} catch (Exception e) {
				Debug.LogError ("---------PB_DelMailGetAwardResponse--------- error " + e.Message);
			}
		}

		public void RecvGetSystemMailResponse (byte[] data)
		{
			try {
				PB_GetMailResponse nResponseIn = PB_GetMailResponse.Parser.ParseFrom (data);
				FiGetSystemMailResponse nResponseOut = new FiGetSystemMailResponse ();
				nResponseOut.result = nResponseIn.Result;
			
				//Debug.LogError( "---------RecvGetSystemMailResponse---------" + nResponseIn );

				IEnumerator<PB_Mail> nEum = nResponseIn.Mail.GetEnumerator ();
				while (nEum.MoveNext ()) {
					FiSystemMail nMail = new FiSystemMail ();
					nMail.content = nEum.Current.Content;
					nMail.mailId = nEum.Current.MailId;
					if (nEum.Current.Property != null) {
						IEnumerator<PB_Property> nEumProp = nEum.Current.Property.GetEnumerator ();
						while (nEumProp.MoveNext ()) {
							FiProperty nProp = new FiProperty ();
							nProp.type = nEumProp.Current.PropertyType;
							nProp.value = nEumProp.Current.Sum;
							nMail.property.Add (nProp);
						}
					}
					nMail.sendTime = nEum.Current.SendTime;
					nMail.title = nEum.Current.Title;
					nResponseOut.mails.Add (nMail);
				}
				mNetCtrl.dispatchEvent (FiEventType.RECV_GET_SYSTEM_MAIL_RESPONSE, nResponseOut);
			} catch (Exception e) {
				Tool.OutLogToFile ("[ network ] recv message== RECV_GET_SYSTEM_MAIL_RESPONSE error" + e.Message);
			}
		}


		public void RecvStartGiftResponse (byte[] data)
		{
			try {
				PB_GetStartGiftResponse nResponseIn = PB_GetStartGiftResponse.Parser.ParseFrom (data);
				FiGetStartGiftResponse nResponseOut = new FiGetStartGiftResponse ();

				nResponseOut.result = nResponseIn.Result;
				nResponseOut.dayOffset = nResponseIn.DayOffset;

				IEnumerator<PB_Property> nEum = nResponseIn.Properties.GetEnumerator ();
				while (nEum.MoveNext ()) {
					FiProperty nProp = new FiProperty ();
					nProp.type = nEum.Current.PropertyType;
					nProp.value = nEum.Current.Sum;
					nResponseOut.data.Add (nProp);
				}
			
				mNetCtrl.dispatchEvent (FiEventType.RECV_GET_START_GIFT_RESPONSE, nResponseOut);
			} catch (Exception e) {
				Tool.OutLogToFile ("[ network ] recv message== RECV_GET_START_GIFT_RESPONSE error" + e.Message);
			}
		}

		public void RecvStartGiftInform (byte[] data)
		{
//			try
//			{
//				PB_NotifyStartGift  nResponseIn = PB_NotifyStartGift.Parser.ParseFrom ( data );
//				FiStartGiftInform nResponseOut = new FiStartGiftInform();
//
//				//Debug.LogError( "RecvStartGiftInform nResponseIn ===== > " + nResponseIn );
//				nResponseOut.dayOffset = nResponseIn.DayOffset;
//
//				IEnumerator<PB_Property> nEum = nResponseIn.Properties.GetEnumerator();
//				while( nEum.MoveNext() )
//				{
//					FiProperty nProp = new FiProperty();
//					nProp.type  = nEum.Current.PropertyType;
//					nProp.value = nEum.Current.Sum;
//					nResponseOut.data.Add( nProp );
//				}
//				mNetCtrl.dispatchEvent( FiEventType.RECV_START_GIFT_INFORM , nResponseOut );
//			}catch( Exception e )
//			{
//				Tool.OutLogToFile ("[ network ] recv message== RECV_START_GIFT_INFORM error" + e.Message);
//			}
		}

		public void RecvGetFriendListResponse (byte[] data)
		{
			try {
				PB_GetFriendListResponse nResponseIn = PB_GetFriendListResponse.Parser.ParseFrom (data);
				FiGetFriendListResponse nResponseOut = new FiGetFriendListResponse ();

				nResponseOut.friendLimit = nResponseIn.FriendLimit;
				nResponseOut.result = nResponseIn.Result;

				IEnumerator<PB_Friend> nEum = nResponseIn.Friends.GetEnumerator ();
				while (nEum.MoveNext ()) {
					FiFriendInfo nInfo = new FiFriendInfo ();
					nInfo.avatar = nEum.Current.Avatar;
					nInfo.gender = nEum.Current.Gender;
					nInfo.nickname = nEum.Current.Nickname;
					nInfo.userId = nEum.Current.UserId;
					nInfo.vipLevel = nEum.Current.VipLevel;
					nInfo.hasGivenGold = nEum.Current.HasGivenGold;
					nInfo.status = nEum.Current.Status;
					nInfo.level = nEum.Current.Level;
					nInfo.gameId = nEum.Current.GameId;
					nResponseOut.friends.Add (nInfo);
				}
				mNetCtrl.dispatchEvent (FiEventType.RECV_GET_FRIEND_LIST_RESPONSE, nResponseOut);
			} catch (Exception e) {
				Tool.OutLogToFile ("[ network ] recv message== RECV_GET_FRIEND_LIST_RESPONSE error" + e.Message);
			}
		}

		public void RecvGetFriendApplyListResponse (byte[] data)
		{
			try {
				PB_GetAddFriendListResponse nResponseIn = PB_GetAddFriendListResponse.Parser.ParseFrom (data);
				FiGetFriendApplyResponse nResponseOut = new FiGetFriendApplyResponse ();

//				Debug.LogError( "------------" + nResponseIn );

				nResponseOut.result = nResponseIn.Result;

				IEnumerator<PB_Friend> nEum = nResponseIn.Friends.GetEnumerator ();
				while (nEum.MoveNext ()) {
					FiFriendInfo nInfo = new FiFriendInfo ();
					nInfo.avatar = nEum.Current.Avatar;
					nInfo.gender = nEum.Current.Gender;
					nInfo.nickname = nEum.Current.Nickname;
					nInfo.userId = nEum.Current.UserId;
					nInfo.vipLevel = nEum.Current.VipLevel;
					nInfo.hasGivenGold = nEum.Current.HasGivenGold;
					nInfo.status = nEum.Current.Status;
					nInfo.level = nEum.Current.Level;
					nResponseOut.friends.Add (nInfo);
				}
				mNetCtrl.dispatchEvent (FiEventType.RECV_GET_FRIEND_APPLY_LIST_RESPONSE, nResponseOut);
			} catch (Exception e) {
				Tool.OutLogToFile ("[ network ] recv message== RECV_GET_FRIEND_APPLY_LIST_RESPONSE error" + e.Message);
			}
		}

		public void RecvRejectFriendApplyResponse (byte[] data)
		{
			try {
				PB_RejectFriendResponse nResponseIn = PB_RejectFriendResponse.Parser.ParseFrom (data);

				FiRejectFriendResponse nResponseOut = new FiRejectFriendResponse ();

				nResponseOut.userId = nResponseIn.UserId;
				mNetCtrl.dispatchEvent (FiEventType.RECV_REJECT_FRIEND_RESPONSE, nResponseOut);
			} catch (Exception e) {
				Tool.OutLogToFile ("[ network ] recv message== RECV_REJECT_FRIEND_RESPONSE error" + e.Message);
			}
		}

		public void RecvAcceptFriendResponse (byte[] data)
		{
			try {
				PB_AcceptFriendResponse nResponseIn = PB_AcceptFriendResponse.Parser.ParseFrom (data);
				FiAcceptFriendResponse nResponseOut = new FiAcceptFriendResponse ();
				nResponseOut.result = nResponseIn.Result;
				nResponseOut.userId = nResponseIn.UserId;
				mNetCtrl.dispatchEvent (FiEventType.RECV_ACCEPT_FRIEND_RESPONSE, nResponseOut);
			} catch (Exception e) {
				Tool.OutLogToFile ("[ network ] recv message== RECV_ACCEPT_FRIEND_RESPONSE error" + e.Message);
			}
		}

		public void RecvAddFriendResponse (byte[] data)
		{
			try {
				PB_AddFriendResponse nResponseIn = PB_AddFriendResponse.Parser.ParseFrom (data);
				FiAddFriendResponse nResponseOut = new FiAddFriendResponse ();
			
				nResponseOut.result = nResponseIn.Result;
				nResponseOut.userId = nResponseIn.UserId;
				mNetCtrl.dispatchEvent (FiEventType.RECV_ADD_FRIEND_RESPONSE, nResponseOut);
			} catch (Exception e) {
				Tool.OutLogToFile ("[ network ] recv message== RECV_ACCEPT_FRIEND_RESPONSE error" + e.Message);
			}
		}

		public void RecvDeleteFriendResponse (byte[] data)
		{
			try {
				PB_DeleteFriendResponse nResponseIn = PB_DeleteFriendResponse.Parser.ParseFrom (data);
				FiDeleteFriendResponse nResponseOut = new FiDeleteFriendResponse ();
		
				nResponseOut.result = nResponseIn.Result;
				nResponseOut.userId = nResponseIn.UserId;
				mNetCtrl.dispatchEvent (FiEventType.RECV_DELETE_FRIEND_RESPONSE, nResponseOut);
			} catch (Exception e) {
				Tool.OutLogToFile ("[ network ] recv message== RECV_ACCEPT_FRIEND_RESPONSE error" + e.Message);
			}
		}

		public void RecvEveryDayTaskProcessResponse (byte[] data)
		{
			try {
				PB_EverydayTaskProgressResponse nResponseIn = PB_EverydayTaskProgressResponse.Parser.ParseFrom (data);

				FiEverydayTaskProgressInform nResponseOut = new FiEverydayTaskProgressInform ();
				nResponseOut.result = nResponseIn.Result;
				nResponseOut.activity = nResponseIn.Activity;

				IEnumerator<int> nEumState = nResponseIn.State.GetEnumerator ();
				while (nEumState.MoveNext ()) {
					nResponseOut.states.Add (nEumState.Current);
				}

				IEnumerator<PB_EverydayTaskProgress> nEumTask = nResponseIn.Task.GetEnumerator ();
				while (nEumTask.MoveNext ()) {
					FiEverydayTaskDetial nDetail = new FiEverydayTaskDetial ();
					nDetail.progress = nEumTask.Current.Progress;
					nDetail.taskId = nEumTask.Current.TaskId;
					nResponseOut.tasks.Add (nDetail);
				}
					
				mNetCtrl.dispatchEvent (FiEventType.RECV_EVERYDAY_TASK_PROCESS_INFORM, nResponseOut);
			} catch (Exception e) {
				Tool.OutLogToFile ("[ network ] recv message== RECV_EVERYDAY_TASK_RESPONSE error" + e.Message);
			}
		}

		public void RecvEveryDayActivityAwardResponse (byte[] data)
		{
			try {
				PB_EverydayActivityAwardResponse nResponseIn = PB_EverydayActivityAwardResponse.Parser.ParseFrom (data);
				FiEverydayActivityAwardResponse nResponseOut = new FiEverydayActivityAwardResponse ();

				nResponseOut.activity = nResponseIn.Activity;
				nResponseOut.result = nResponseIn.Result;

				IEnumerator<PB_Property> nEumTask = nResponseIn.Property.GetEnumerator ();
				while (nEumTask.MoveNext ()) {
					FiProperty nProp = new FiProperty ();
					nProp.type = nEumTask.Current.PropertyType;
					nProp.value = nEumTask.Current.Sum;
					nResponseOut.property.Add (nProp);
				}

				mNetCtrl.dispatchEvent (FiEventType.RECV_EVERYDAY_ACTIVITY_AWARD_RESPONSE, nResponseOut);
			} catch (Exception e) {
				Tool.OutLogToFile ("[ network ] recv message== RECV_EVERYDAY_ACTIVITY_AWARD_RESPONSE error" + e.Message);
			}
		}

		public void RecvEveryDayTaskResponse (byte[] data)
		{
			try {
				PB_EverydayActivityResponse nResponseIn = PB_EverydayActivityResponse.Parser.ParseFrom (data);
				FiEveryDayActivityResponse nResponseOut = new FiEveryDayActivityResponse ();

				nResponseOut.activity = nResponseIn.Activity;
				nResponseOut.result = nResponseIn.Result;

				IEnumerator<int> nEumTask = nResponseIn.TaskId.GetEnumerator ();
				while (nEumTask.MoveNext ()) {
					nResponseOut.taskId.Add (nEumTask.Current);
				}

				mNetCtrl.dispatchEvent (FiEventType.RECV_EVERYDAY_TASK_RESPONSE, nResponseOut);
			} catch (Exception e) {
				Tool.OutLogToFile ("[ network ] recv message== RECV_EVERYDAY_TASK_RESPONSE error" + e.Message);
			}
		}

		public void RecvGiveOtherPropertyResponse (byte[] data)
		{
			try {
				PB_GiveOtherPropertyResponse nResponseIn = PB_GiveOtherPropertyResponse.Parser.ParseFrom (data);
				FiGiveOtherPropertyResponse nResponseOut = new FiGiveOtherPropertyResponse ();

				//Debug.LogError( "---------PB_GiveOtherPropertyResponse------------" + nResponseIn );

				nResponseOut.userId = nResponseIn.UserId;
				nResponseOut.result = nResponseIn.Result;
				if (nResponseIn.Property != null) {
					nResponseOut.property.type = nResponseIn.Property.PropertyType;
					nResponseOut.property.value = nResponseIn.Property.Sum;
				}
				mNetCtrl.dispatchEvent (FiEventType.RECV_GIVE_OTHER_PROPERTY_RESPONSE, nResponseOut);
			} catch (Exception e) {
				//Debug.LogError( "RECV_GIVE_OTHER_PROPERTY_RESPONSE error ");
				Tool.OutLogToFile ("[ network ] recv message== RECV_GIVE_OTHER_PROPERTY_RESPONSE error" + e.Message);
			}
		}

		public void RecvGameRankResponse (byte[] data)
		{
			try {
				PB_GameRankResponse nResponseIn = PB_GameRankResponse.Parser.ParseFrom (data);
				FiGetRankResponse nResponseOut = new FiGetRankResponse ();

				//Debug.LogError( "-----------nResponseIn-------------" + nResponseIn );
				nResponseOut.result = nResponseIn.Result;
				IEnumerator<PB_GameRank> nEumOther = nResponseIn.Rank.GetEnumerator ();
				while (nEumOther.MoveNext ()) {
					FiRankInfo nInfo = new FiRankInfo ();
					nInfo.avatarUrl = nEumOther.Current.AvatarUrl;
					nInfo.gold = nEumOther.Current.Gold;
					nInfo.nickname = nEumOther.Current.Nickname;
					nInfo.userId = nEumOther.Current.UserId;
					nInfo.vipLevel = nEumOther.Current.Vip;
					nInfo.gender = nEumOther.Current.Gender;
					nInfo.level = nEumOther.Current.Level;
					nInfo.maxMultiple = nEumOther.Current.MaxMultiple;
					nInfo.gameId = nEumOther.Current.GameId;
					nResponseOut.rankList.Add (nInfo);
				}

				mNetCtrl.dispatchEvent (FiEventType.RECV_GET_RANK_RESPONSE, nResponseOut);
			} catch (Exception e) {
				Tool.OutLogToFile ("[ network ] recv message== FISHING_UNLOCK_CANNON_MULTIPLE_RESPONSE error" + e.Message);
			}
		}

		public void RecvTopupResponse (byte[] data)
		{
			try {
				TopUpResponse nTopupRes = TopUpResponse.Parser.ParseFrom (data);
				mNetCtrl.dispatchEvent (FiEventType.RECV_TOPUP_RESPONSE, FiProtoHelper.toLocal_TopUpResponse (nTopupRes));
			} catch (Exception e) {
				Debug.Log ("[ network ] recv message== FISHING_TOP_UP_RESPONSE error" + e.Message); 
			}
		}

		public void RecvGetBackPackResponse (byte[] data)
		{
			try {
				GetBackpackPropertyResponse nPropRes = GetBackpackPropertyResponse.Parser.ParseFrom (data);
				//Debug.LogError( "--------RecvGetBackPackResponse----------" + nPropRes );
				mNetCtrl.dispatchEvent (FiEventType.RECV_BACKPACK_RESPONSE, FiProtoHelper.toLocal_BackpackResponse (nPropRes));
			} catch (Exception e) {
				Debug.Log ("[ network ] recv message== FISHING_GET_BACKPACK_PROPERTY_RESPONSE error" + e.Message); 
			}
		}

		public void RecvReconnectGameResponse (byte[] data)
		{
			try {
				PB_ReconnectGameResponse nRecnnect = PB_ReconnectGameResponse.Parser.ParseFrom (data);
				FiReconnectResponse nRecResponse = new FiReconnectResponse ();
				nRecResponse.goldType = nRecnnect.GoldType;
				nRecResponse.roomIndex = nRecnnect.RoomIndex;
				nRecResponse.roomType = nRecnnect.RoomType;
				nRecResponse.properties = new List<FiProperty> ();
				nRecResponse.seatIndex = nRecnnect.SeatIndex;
				nRecResponse.result = nRecnnect.Result;

				IEnumerator<PB_Property> nEumProp = nRecnnect.Properties.GetEnumerator ();
				while (nEumProp.MoveNext ()) {
					FiProperty nProp = new FiProperty ();
					nProp.type = nEumProp.Current.PropertyType;
					nProp.value = nEumProp.Current.Sum;
					nRecResponse.properties.Add (nProp);
				}


				nRecResponse.others = new List<FiOtherGameInfo> ();
				IEnumerator<PB_OtherGameInfo> nEumOther = nRecnnect.Others.GetEnumerator ();
				while (nEumOther.MoveNext ()) {
					FiOtherGameInfo nInfoGame = new FiOtherGameInfo ();
					nInfoGame.avatar = nEumOther.Current.Avatar;
					nInfoGame.gender = nEumOther.Current.Gender;
					nInfoGame.nickname = nEumOther.Current.Nickname;
					nInfoGame.seatIndex = nEumOther.Current.SeatIndex;
					nInfoGame.userId = nEumOther.Current.UserId;
					nInfoGame.vipLevel = nEumOther.Current.VipLevel;
					nInfoGame.cannonStyle = nEumOther.Current.CannonStyle;
					nInfoGame.gameId = nEumOther.Current.GameId;
					IEnumerator<PB_Property> nEumOtherProp = nEumOther.Current.Properties.GetEnumerator ();
					while (nEumOtherProp.MoveNext ()) {
						FiProperty nPropIn = new FiProperty ();
						nPropIn.type = nEumOtherProp.Current.PropertyType;
						nPropIn.value = nEumOtherProp.Current.Sum;
						nInfoGame.properties.Add (nPropIn);
					}
					nRecResponse.others.Add (nInfoGame);
				}
				mNetCtrl.dispatchEvent (FiEventType.RECV_RECONNECT_GAME_RESPONSE, nRecResponse);
				//Debug.LogError( " reconnect info ====> " + nRecnnect );
			} catch (Exception e) {
				Tool.OutLogToFile ("[ network ] recv message== FISHING_RECONNECT_GAME_RESPONSE error" + e.Message);
			}
		}

		public void RecvConvertFormalAccountResponse (byte[] data)
		{
			try {
				PB_SystemReward info = PB_SystemReward.Parser.ParseFrom (data);
				//Debug.LogError( "--------RecvGetBackPackResponse----------" + nPropRes );
				mNetCtrl.dispatchEvent (FiEventType.RECV_CONVERSION_REQUEST, FiProtoHelper.toLocal_SystemReward (info));
			} catch (Exception e) {
				Debug.Log ("RecvSystemRewardResponse error" + e.Message); 
			}
		}

		public void RecvModifyNickResponse (byte[] data)
		{
			try {
				PB_SystemReward info = PB_SystemReward.Parser.ParseFrom (data);
				//Debug.LogError( "--------RecvGetBackPackResponse----------" + nPropRes );
				mNetCtrl.dispatchEvent (FiEventType.RECV_CL_MODIFY_NICK_RESPONSE, FiProtoHelper.toLocal_SystemReward (info));
			} catch (Exception e) {
				Debug.Log ("RecvSystemRewardResponse error" + e.Message); 
			}
		}

		public void RecvHelpGoldTaskData (byte[] data)
		{
			try {
				PB_HelpGoldTaskData info = PB_HelpGoldTaskData.Parser.ParseFrom (data);
				mNetCtrl.dispatchEvent (FiEventType.RECV_CL_HELP_GOLD_TASK, FiProtoHelper.toLocal_HelpGoldTaskData (info));
			} catch (Exception e) {
				Debug.Log ("RecvHelpGoldTaskData error" + e.Message); 
			}
		}

		public void RecvHelpGoldTaskDataReward (byte[] data)
		{
			try {
				PB_HelpGoldTaskData info = PB_HelpGoldTaskData.Parser.ParseFrom (data);
				mNetCtrl.dispatchEvent (FiEventType.RECV_CL_GET_HELP_TASK_REWARD_RESPONSE, FiProtoHelper.toLocal_HelpGoldTaskData (info));
			} catch (Exception e) {
				Debug.Log ("RecvHelpGoldTaskDataReward error" + e.Message); 
			}
		}

		public void RecvSignRetroactiveResponse (byte[] data)
		{
			try {
				PB_SignRetroactiveResponse nResponseIn = PB_SignRetroactiveResponse.Parser.ParseFrom (data);
				mNetCtrl.dispatchEvent (FiEventType.RECV_CL_SIGNRETROACTIVE_RESPONSE, FiProtoHelper.toLocal_SignRetroactive (nResponseIn));

			} catch (Exception e) {
				Tool.OutLogToFile ("[ network ] recv message== RECV_SIGN_IN_AWARD_RESPONSE error" + e.Message);
			}
		}
		//接收是否绑定手机号响应
		public void RecvBindPhoneStartResponse (byte[] data)
		{
			Debug.LogError ("--------RecvBindPhoneStartResponse---------------");
			try {
				GetBindPhoneState isBindPhone = GetBindPhoneState.Parser.ParseFrom (data);
				mNetCtrl.dispatchEvent (FiEventType.RECV_BIND_PHONE_STATE_RESPONSE, FiProtoHelper.toLocal_FiIsBindPhoneResponse (isBindPhone));
			} catch (Exception e) {
				Debug.Log ("RecvBindPhoneStartResponse" + e.Message);
			}
		}

		//接受使用道具请求
		public void RecvUsePropTimeResponse (byte[] data)
		{
			try {
				UsePropTimeExResponse useProp = UsePropTimeExResponse.Parser.ParseFrom (data);
				mNetCtrl.dispatchEvent (FiEventType.RECV_USE_PROP_TIME_RESPONSE, FiProtoHelper.toLocal_FiUsePropTimeResponse (useProp));
				
			} catch (Exception e) {
				Debug.Log ("RecvUsePropTimeResponse" + e.Message);
			}
		}
		//删除显示道具
		public void RecvDelPropTimeResponse (byte[] data)
		{
			try {
				DelUsePropTimeEx delProp = DelUsePropTimeEx.Parser.ParseFrom (data);
				mNetCtrl.dispatchEvent (FiEventType.RECV_DEL_PROP_TIME_RESPONSE, FiProtoHelper.toLocal_FiDelPropTimeResponse (delProp));
				
			} catch (Exception e) {
				Debug.Log ("RecvDelPropTimeResponse" + e.Message);
			}
		}

		//获取所有已经使用的 限时道具信息
		public void RecvGetAllPropTimeResponse (byte[] data)
		{
			try {
				UsePropTimeArray useProp = UsePropTimeArray.Parser.ParseFrom (data);
				mNetCtrl.dispatchEvent (FiEventType.RECV_GET_ALL_PROP_TIME_RESPONSE, FiProtoHelper.toLocal_UseProTimeArr (useProp));

			} catch (Exception e) {
				Debug.Log ("RecvGetAllPropTimeResponse" + e.Message);
			}
		}

		public void RecvGetRewardInfoResponse (byte[] data)
		{
			//Debug.Log("10078 10078 10078 10078 10078 10078 10078 10078 10078 10078");
			try {
				PB_RewardAllData useProp = PB_RewardAllData.Parser.ParseFrom (data);
				mNetCtrl.dispatchEvent (FiEventType.RECV_XL_GET_REWARD_INFO, FiProtoHelper.toLocal_RewardAllData (useProp));

			} catch (Exception e) {
				Debug.Log ("RecvGetRewardInfoResponse" + e.Message);
			}
		}

		public void RecvGetDraGonCardInfoResponse (byte[] data)
		{
			try {
				DBGetMonthlyCardReward dracardpro = DBGetMonthlyCardReward.Parser.ParseFrom (data);

				mNetCtrl.dispatchEvent (FiEventType.RECV_CL_USE_DARGON_CARD_RESPONSE, FiProtoHelper.toLocal_DraGonData (dracardpro));

			} catch (Exception e) {
				Debug.Log ("RecvGetRewardInfoResponse" + e.Message);
			}
		}

		public void RecvPurchaseGetDraGonCardInfoResponse (byte[] data)
		{
			try {
                Debug.LogError("RecvPurchaseGetDraGonCardInfoResponse");
                DBTopUpMonthlyCardResponse purchasedracardpro = DBTopUpMonthlyCardResponse.Parser.ParseFrom (data);

				mNetCtrl.dispatchEvent (FiEventType.RECV_PURCHASE_DARGON_CARD_RESPONSE, FiProtoHelper.toLocal_PurchaseDraGonData (purchasedracardpro));

			} catch (Exception e) {
				Debug.Log ("RecvGetRewardInfoResponse" + e.Message);
			}
		}
		//特惠 接收数据
		public void RecvGetPreferentialInfoResponse (byte[] data)
		{
			try {
//				Debug.Log ("RecvGetPreferentialInfoResponse" + "特惠接收消息");
				DBGetPreferentialReward dBGetPreferential = DBGetPreferentialReward.Parser.ParseFrom (data);
				mNetCtrl.dispatchEvent (FiEventType.RECV_CL_USER_PREFERENTIAL_REQUEST, FiProtoHelper.toLocal_PreferentialData (dBGetPreferential));
			} catch (Exception ex) {
				Debug.Log ("RecvGetPreferentialInfoResponse" + ex.Message);
			}
		}
		//特惠成功 接收数据
		public void RecvGetPurPreferentialInfoResponse (byte[] data)
		{
			try {
//				Debug.Log ("RecvGetPreferentialInfoResponse" + "特惠成功后接收消息");
				DBGetPreferentialResponse dBGetPreferential1 = DBGetPreferentialResponse.Parser.ParseFrom (data);
				mNetCtrl.dispatchEvent (FiEventType.RECV_PURCHASE_TEHUI_CARD_RESPONSE, FiProtoHelper.toLocal_PreferentialData1 (dBGetPreferential1));
			} catch (Exception ex) {
				Debug.Log ("RecvGetPreferentialInfoResponse" + ex.Message);
			}
		}

		/// <summary>
		/// 接受兑换炮座回复
		/// </summary>
		/// <param name="data">Data.</param>
		public void RecvExchangeBarbetteResponse (byte[] data)
		{
			try {
				Debug.Log("  接收炮座   RecvExchangeBarbetteResponse  ");
				useBuyCannonBottomInfo nResponseIn = useBuyCannonBottomInfo.Parser.ParseFrom (data);
				mNetCtrl.dispatchEvent (FiEventType.RECV_XL_EXCHANGEBARBETTE_RESPONSE, FiProtoHelper.toLocal_FiExchangeBarbetteData (nResponseIn));

			} catch (Exception ex) {
				Debug.Log ("RecvExchangeBarbetteResponse error" + ex.Message); 
			}
		}

		/// <summary>
		/// 跟换炮座样式回复
		/// </summary>
		/// <param name="data">Data.</param>
		public void RecvChangeBarbetteStyleResponse (byte[] data)
		{
			try {
				EquipmentCannonBottom cannonBottom = EquipmentCannonBottom.Parser.ParseFrom (data);
				mNetCtrl.dispatchEvent (FiEventType.RECV_XL_EQUIPMENTBARBETTE_RESPONSE, FiProtoHelper.toLocal_ChangeBarbetteStyleData (cannonBottom));
			} catch (Exception ex) {
				Debug.Log ("RecvChangeBarbetteStyleResponse error" + ex.Message); 
			}
		}

		public void RecvManmonStartResponse (byte[] data)
		{
			try {
				ManmonCount manmoncout = ManmonCount.Parser.ParseFrom (data);
				mNetCtrl.dispatchEvent (FiEventType.RECV_XL_MANMONSTATRT_RESPONSE, FiProtoHelper.toLocal_ManmonStartData (manmoncout));
			} catch (Exception ex) {
				Debug.Log ("RecvManmonStartResponse error" + ex.Message); 
			}
		}

		public void RecvManmonSettingResponse (byte[] data)
		{
			try {
				GetManmonChipGold manmoncout = GetManmonChipGold.Parser.ParseFrom (data);
				mNetCtrl.dispatchEvent (FiEventType.RECV_XL_MANMONSETTING_RESPONSE, FiProtoHelper.toLocal_ManmonSettingData (manmoncout));
			} catch (Exception ex) {
				Debug.Log ("RecvManmonStartResponse error" + ex.Message); 
			}
		}

		public void RecvManmonBettingResponse (byte[] data)
		{
			try {
				ChipJettorGold chipjettorgold = ChipJettorGold.Parser.ParseFrom (data);
				mNetCtrl.dispatchEvent (FiEventType.RECV_XL_MANMONBETTING_RESPONSE, FiProtoHelper.toLocal_ManmonBettingData (chipjettorgold));
			} catch (Exception ex) {
				Debug.Log ("RecvManmonStartResponse error" + ex.Message); 
			}
		}

		public void RecvManmoneYaoQianShuResponse (byte[] data)
		{
			try {
				ChipJettorGold chipjettorgold = ChipJettorGold.Parser.ParseFrom (data);
				mNetCtrl.dispatchEvent (FiEventType.RECV_XL_MANMONYAOQIANSHU_RESPONSE, FiProtoHelper.toLocal_ManmonYaoQianShuData (chipjettorgold));
			} catch (Exception ex) {
				Debug.Log ("RecvManmonStartResponse error" + ex.Message); 
			}
		}

		public void RecvShenlongTuanTableResponse (byte[] data)
		{
			try {
				GetLongRewardPoolCount getLongRewardPoolCount = GetLongRewardPoolCount.Parser.ParseFrom (data);
				mNetCtrl.dispatchEvent (FiEventType.RECV_XL_TURNTABLEGETPOOL_RESPONSE, FiProtoHelper.toLocal_GetCoinTurnTablePool (getLongRewardPoolCount));
				//这里需要写入proto数据，
			} catch (Exception ex) {
				Debug.Log ("RecvShenlongTuanTableResponse erro" + ex.Message);
			}
		}

		public void RecvManmomRankRewardResponse (byte[] data)
		{
			try {
				GetManmonRewardInfo getLongRewardPoolCount = GetManmonRewardInfo.Parser.ParseFrom (data);
				mNetCtrl.dispatchEvent (FiEventType.RECV_XL_MANMONRANKREWARD_REQUST, FiProtoHelper.toLocal_ManmonRankReward (getLongRewardPoolCount));
				//这里需要写入proto数据，
			} catch (Exception ex) {
				Debug.Log ("RecvShenlongTuanTableResponse erro" + ex.Message);
			}
		}

		public void RecvShenLongTurnTableLiuShuiResponse (byte[] data)
		{
			try {
				LongLiuShuiGold longLiuShuiGold = LongLiuShuiGold.Parser.ParseFrom (data);
				mNetCtrl.dispatchEvent (FiEventType.RECV_XL_TURNTABLELIUSHUI_RESPONSE, FiProtoHelper.toLocal_TurnTableLiuShui (longLiuShuiGold));
			} catch (Exception ex) {
				Debug.Log ("RecvShenLongTurnTableLiuShuiResponse" + ex.Message);
			}
		}

		public void RecvShenlongTurnTableLuckDrawResponse (byte[] data)
		{
			try {
				GetFishLuckyDrawResponse getFishLuckyDrawResponse = GetFishLuckyDrawResponse.Parser.ParseFrom (data);
				mNetCtrl.dispatchEvent (FiEventType.RECV_XL_TURNTABLELUCKDRAW_RESPOSE, FiProtoHelper.toLocal_TurnTableLuckDraw (getFishLuckyDrawResponse));

			} catch (Exception ex) {
				Debug.Log ("RecvShenlongTurnTableLuckDrawResponse" + ex.Message);
			}
		}

		public void RecvShenLongChangeLiuShuiTime (byte[] data)
		{
			try {
				LongChangeLiuShuiTime longChangeLiuShuiTime = LongChangeLiuShuiTime.Parser.ParseFrom (data);
				mNetCtrl.dispatchEvent (FiEventType.RECV_XL_CHANGELIUSHUITIME_RESPOSE, FiProtoHelper.toLocal_ChangeLiuShuiTime (longChangeLiuShuiTime));
			} catch (Exception ex) {
				Debug.Log ("RecvShenLongChangeLiuShuiTime" + ex.Message);
			}
		}

		public void RecvCancelOtherSkill (byte[] data)
		{
			try {
				CancelSkill cancelSkill = CancelSkill.Parser.ParseFrom (data);
				mNetCtrl.dispatchEvent (FiEventType.RECV_XL_CANCELOTHERSKILL_REQUST, FiProtoHelper.toLocal_OtherCancelSkill (cancelSkill));
			} catch (Exception ex) {
				Debug.Log ("RecvCancelOtherSkill" + ex.Message);
			}
		}

		public void RecvFishingUserRank (byte[] data)
		{
			try {
				UserRankInfo userRankInfo = UserRankInfo.Parser.ParseFrom (data);
				mNetCtrl.dispatchEvent (FiEventType.RECV_XL_UPDATERANKINFO_RESPOSE, FiProtoHelper.toLocal_FishingUserRank (userRankInfo));
			} catch (Exception ex) {
				Debug.Log ("RecvFishingUserRank" + ex.Message);
			}
		}

		public void RecvMyFishingUserRank (byte[] data)
		{
			try {
				UserRankInfo userRankInfo = UserRankInfo.Parser.ParseFrom (data);
				mNetCtrl.dispatchEvent (FiEventType.RECV_XL_UPDATEMYRANKINFO_RESPOSE, FiProtoHelper.toLocal_FishingMyUserRank (userRankInfo));
			} catch (Exception ex) {
				Debug.Log ("RecvMyFishingUserRank" + ex.Message);
			}
		}

		public void RecvWinTimeResult (byte[] data)
		{
			try {
				MamonMaxWinCount userRankInfo = MamonMaxWinCount.Parser.ParseFrom (data);
				mNetCtrl.dispatchEvent (FiEventType.RECV_XL_WINTIMECOUNTFO_RESPOSE, FiProtoHelper.toLocal_Wincoutime (userRankInfo));
			} catch (Exception ex) {
				Debug.Log ("RecvMyFishingUserRank" + ex.Message);
			}
		}

		public void RecvBossRoomMatch (byte[] data)
		{
			try {
				NotifyBossRoomMatchInfo matchInfo = NotifyBossRoomMatchInfo.Parser.ParseFrom (data);
				//Debug.Log ("[ network ]!!!!!!!!!!!! recv create fishs == num" + nGroupInfrom.FishNum + "/ type = " + nGroupInfrom.FishType + " / GroupId =" + nGroupInfrom.GroupId + "/ TrackId =" + nGroupInfrom.TrackId + "/TrackType =" + nGroupInfrom.TrackType);
				mNetCtrl.dispatchEvent (FiEventType.RECV_XL_BOSSROOMMATCH_RESPOSE, FiProtoHelper.toLocal_BossRoomMatchInfo (matchInfo));
			} catch (Exception e) {
				Debug.Log ("[ network ] recv message== RecvBossRoomMatch error" + e.Message); 
			}
		}

		public void RecvNotifySignUp (byte[] data)
		{
			try {
				NotifySignUp signUp = NotifySignUp.Parser.ParseFrom (data);
				//Debug.Log ("[ network ]!!!!!!!!!!!! recv create fishs == num" + nGroupInfrom.FishNum + "/ type = " + nGroupInfrom.FishType + " / GroupId =" + nGroupInfrom.GroupId + "/ TrackId =" + nGroupInfrom.TrackId + "/TrackType =" + nGroupInfrom.TrackType);
				mNetCtrl.dispatchEvent (FiEventType.RECV_XL_NOTIFYSIGNUP_RESPOSE, FiProtoHelper.toLocal_NotifySignUp (signUp));
			} catch (Exception e) {
				Debug.Log ("[ network ] recv message== RecvNotifySignUp error" + e.Message); 
			}
		}

		public void RecvIntoBossRoom (byte[] data)
		{
			try {
				NotifyEnterBossRoomMessage intoMessage = NotifyEnterBossRoomMessage.Parser.ParseFrom (data);
				mNetCtrl.dispatchEvent (FiEventType.RECV_XL_INTOBOSSROMM_RESPOSE, FiProtoHelper.toLocal_IntoBossRoom (intoMessage));
			} catch (Exception ex) {
				Debug.Log ("[ network ] recv message== RecvIntoBossRoom error" + ex.Message);
			}
		}

		public void RecvGetBossKillRank (byte[] data)
		{
			try {
				UserBossKillRankInfo userBossKillRank = UserBossKillRankInfo.Parser.ParseFrom (data);
				mNetCtrl.dispatchEvent (FiEventType.RECV_XL_BOSSKILLRANK_RESPOSE, FiProtoHelper.toLocal_BossKillRank (userBossKillRank));
			} catch (Exception ex) {
				Debug.Log ("[ network ] recv message== RecvGetBossKillRank error" + ex.Message);
			}
		}

		public void RecvGetBossKillMyRank (byte[] data)
		{
			try {
				UserBossKillRankInfo userBossKillMyRank = UserBossKillRankInfo.Parser.ParseFrom (data);
				mNetCtrl.dispatchEvent (FiEventType.RECV_XL_BOSSKILLMYRANK_RESPOSE, FiProtoHelper.toLocal_BossKillMyRank (userBossKillMyRank));
			} catch (Exception ex) {
				Debug.Log ("[ network ] recv message== RecvGetBossKillMyRank error" + ex.Message);
			}
		}

		public void RecvGetBossMatchResultRank (byte[] data)
		{
			try {
				UserRankInfoArray userRankInfoArray = UserRankInfoArray.Parser.ParseFrom (data);
				mNetCtrl.dispatchEvent (FiEventType.RECV_XL_BOSSMATCHRESULT_RESPOSE, FiProtoHelper.toLocal_GetBossMatchResultRank (userRankInfoArray));
			} catch (Exception e) {
				Tool.OutLogToFile ("[ network ] recv message== RecvGetBossMatchResultRank error" + e.Message);
			}
		}

		//接受荣耀排位数据
		public void RecvHoroDatasResultRank (byte[] data)
		{
			try {
				PaiWeiSaiRankInfo horodataInfoArray = PaiWeiSaiRankInfo.Parser.ParseFrom (data);
				mNetCtrl.dispatchEvent (FiEventType.RECV_XL_HORODATA_RESPOSE, FiProtoHelper.toLocal_HoroDataInfo (horodataInfoArray));
			} catch (Exception e) {
				Tool.OutLogToFile ("[ network ] recv message== RecvHoroDatasResultRank error" + e.Message);
			}
		}

		public void RecvRongYuRankInfo (byte[] data)
		{
			try {
				RongYuDianTangkInfo rongYuDianTangkInfo = RongYuDianTangkInfo.Parser.ParseFrom (data);
				mNetCtrl.dispatchEvent (FiEventType.RECV_XL_RONGYURANK_RESPOSE, FiProtoHelper.toLocal_RongYuRankInfo (rongYuDianTangkInfo));
			} catch (Exception ex) {
				Tool.OutLogToFile ("[ network ] recv message== RecvRongYuRankInfo error" + ex.Message);
			}
		}

		public void RecvPaiWeiPrizeInfo (byte[] data)
		{
			try {
				GetPaiWeiSaiReward reward = GetPaiWeiSaiReward.Parser.ParseFrom (data);
				mNetCtrl.dispatchEvent (FiEventType.RECV_XL_PAIWEIPRIZE_RESPOSE, FiProtoHelper.to_Local_PaiWeiPrizeInfo (reward));
			} catch (Exception ex) {
				Tool.OutLogToFile ("[ network ] recv message== GetPaiWeiSaiReward error" + ex.Message);
			}
		}

		public void RecvPaiWeiPrize (byte[] data)
		{
			try {
                
				PlayerPaiWeiSaiRewardInfo reward = PlayerPaiWeiSaiRewardInfo.Parser.ParseFrom (data);
				mNetCtrl.dispatchEvent (FiEventType.RECV_XL_PAIWEIPRIZEINFO_RESPOSE, FiProtoHelper.to_Local_PawiWeiPrize (reward));
			} catch (Exception ex) {
				Tool.OutLogToFile ("[ network ] recv message== PlayerPaiWeiSaiRewardInfo error" + ex.Message);
			}
		}

		public void RecvRongYaoPrize (byte[] data)
		{
			try {
				Debug.LogError ("rrongyao ling qu ");
				GetPaiWeiSaiReward reward = GetPaiWeiSaiReward.Parser.ParseFrom (data);
				mNetCtrl.dispatchEvent (FiEventType.RECV_XL_RONGYAOPRIZE_RESQUSET, FiProtoHelper.toLocal_RongYaoPrize (reward));
			} catch (Exception ex) {
				Tool.OutLogToFile ("[ network ] recv message== RecvRongYaoPrize error" + ex.Message);
			}
		}


		public void RecvGiftBagResultRank (byte[] data)
		{
			try {
				Debug.LogError ("data===1234567898997r386r816r891=========" + data.Length);
				GetTopUpGiftBagStateNew getTopUpGiftBagStateNew = GetTopUpGiftBagStateNew.Parser.ParseFrom (data);
				mNetCtrl.dispatchEvent (FiEventType.RECV_XL_TOP_UP_GIFT_BAG_STATE_INFO_NEW_RESPOSE, FiProtoHelper.toLocal_GetGiftBagState (getTopUpGiftBagStateNew));
			} catch (Exception ex) {
				Debug.Log ("[ network ] recv message== RecvGiftBagResultRank error" + ex.Message);
			}
		}

		//接收七日礼包奖励领取协议
		public void RecvSevenGiftRewardBagResult (byte[] data)
		{
			try {
				Debug.LogError ("data===1234567898997r386r816r891=========" + data.Length);
				GetSevenDayReward getSevendayGiftBagStateNew = GetSevenDayReward.Parser.ParseFrom (data);
				mNetCtrl.dispatchEvent (FiEventType.RECV_XL_SEVENDAY_BAG_STATE_INFO_NEW_RESPOSE, FiProtoHelper.toLocal_SevendayRewardGetGiftBagState (getSevendayGiftBagStateNew));
			} catch (Exception ex) {
				Debug.Log ("[ network ] recv message== RecvGiftBagResultRank error" + ex.Message);
			}
		}


        public void RecvButtonState(byte[] data)
        {
            try
            {
                Debug.LogError("data===buttonstate=========" + data.Length);
                GetHideButtonState getHideButton = GetHideButtonState.Parser.ParseFrom(data);
                mNetCtrl.dispatchEvent(FiEventType.RECV_XL_BUTTON_HIDE_STATE, FiProtoHelper.toLocal_ButtonState(getHideButton));
            }
            catch (Exception ex)
            {
                Debug.Log("[ network ] recv message== RecvButtonState error" + ex.Message);
            }
        }

		public void RecvSevenGiftBagResultRank(byte[] data)
		{
			try
			{
				Debug.LogError("data===1234567898997r386r816r891=========" + data.Length);
				InitSevenDayInfo getSevendayGiftBagStateNew = InitSevenDayInfo.Parser.ParseFrom(data);
				mNetCtrl.dispatchEvent(FiEventType.RECV_XL_SEVENDAY_START_BAG_STATE_INFO_NEW_RESPOSE, FiProtoHelper.toLocal_SevendayGetGiftBagState(getSevendayGiftBagStateNew));
			}
			catch (Exception ex)
			{
				Debug.Log("[ network ] recv message== RecvGiftBagResultRank error" + ex.Message);
			}
		}

        public void RecvUserLevelUpInfo(byte[] data)
        {
            try
            {
                Debug.LogError("data===接收新手升級獎勵資訊=========" + data.Length);
                DBGetUpLevelActivityInfo getUplevleActivityInfo = DBGetUpLevelActivityInfo.Parser.ParseFrom(data);
                mNetCtrl.dispatchEvent(FiProtoType.XL_GET_UP_LEVEL_ACTIVY_INFO, FiProtoHelper.toLocal_UpLevelState(getUplevleActivityInfo));
            }
            catch (Exception ex)
            {
                Debug.Log("[ network ] recv message== RecvGiftBagResultRank error" + ex.Message);
            }
        }

        // 2020/03/05 Joey 接收領取新手升級獎勵
        public void RecvUserLevelUpReward(byte[] data)
		{
			try
			{
				Debug.Log("Joey Look 接收領取新手升級獎勵 data = " + data.Length);
				//for (int d=0; d< data.Length; d++) {

				//	Debug.Log("Joey Look 接收領取新手升級獎勵 data = " + BitConverter.ToInt32(data[d], 0);
    //            }
				//Parse(data);
				GetUpLevelReward getUpLevelRewardNew = GetUpLevelReward.Parser.ParseFrom(data);
                mNetCtrl.dispatchEvent(FiProtoType.XL_GET_UP_LEVEL_BAG_STATE, FiProtoHelper.toLocal_UpLevelGetReward(getUpLevelRewardNew));
            }
			catch (Exception ex)
			{
				Debug.Log("[ network ] recv message== RecvGiftBagResultRank error" + ex.Message);
			}
		}

        //綁定手機
		public void RecvConvertFormalBindPhoneResponse(byte[] data)
		{
			try
			{
				PB_ConvertFormalBindAccount info = PB_ConvertFormalBindAccount.Parser.ParseFrom(data);
				//Debug.LogError( "--------RecvGetBackPackResponse----------" + nPropRes );
				mNetCtrl.dispatchEvent(FiEventType.RECV_XL_GET_BIND_PHONE_RESPONSE, FiProtoHelper.toLocal_BindPhoneReward(info));
			}
			catch (Exception e)
			{
				Debug.Log("RecvSystemRewardResponse error" + e.Message);
			}
		}

        //付款、金幣配置
		public void RecvPayInfo(byte[] data)
		{
			try
			{
				Debug.Log("RecvPayInfo!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!");
				PB_PayInfo infoArray = PB_PayInfo.Parser.ParseFrom(data);
				mNetCtrl.dispatchEvent(FiProtoType.XL_GET_PAY_INFO, FiProtoHelper.toLocal_PayInfo(infoArray));
                //Dispatch(FiProtoType.XL_GET_PAY_INFO, null);
            }
			catch (Exception ex)
			{
				Debug.Log("RecvPayInfo error" + ex.Message);
			}
        }

        //接收手機號碼登入回傳資訊
		public void RecvPhoneLoginAccount(byte[] data)
		{
			try {
				Debug.Log("RecvPhoneLoginAccount!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!" + data.Length);
				PB_PhoneLogin infoArray = PB_PhoneLogin.Parser.ParseFrom(data);
				mNetCtrl.dispatchEvent(FiProtoType.XL_GET_PHONE_LOGIN, FiProtoHelper.toLocal_PhoneNumberInfo(infoArray));
			}
			catch (Exception ex)
			{
				Debug.Log("RecvPhoneLoginAccount error" + ex.Message);
			}
		}

		//接收設定手機密碼回傳
		public void RecvPhoneNumberPass(byte[] data)
		{
			try
			{
				Debug.Log("RecvPhoneNumberPass!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!" + data.Length);
				PB_GetIphonePassResp infoArray = PB_GetIphonePassResp.Parser.ParseFrom(data);
				mNetCtrl.dispatchEvent(FiProtoType.XL_GET_PHONE_PASSWORD, FiProtoHelper.toLocal_PhoneNumberPass(infoArray));
			}
			catch (Exception ex)
			{
				Debug.Log("RecvPhoneLoginAccount error" + ex.Message);
			}
		}

		//接收選擇要登入的帳號回傳
		public void RecvChoseAccountAssociate(byte[] data)
		{
			try
			{
				Debug.Log("RecvChoseAccountAssociate!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!! "+data.Length);
				//PB_PhoneLoginAccount infoArray = ProtoMsgDeserializer<PB_PhoneLoginAccount>(data);
				PB_AssociateAccountLogin infoArray = PB_AssociateAccountLogin.Parser.ParseFrom(data);
				Debug.Log("RecvChoseAccountAssociate's result :" + infoArray.Result);
				Debug.Log("RecvChoseAccountAssociate's userId :" + infoArray.UserID);
				Debug.Log("RecvChoseAccountAssociate's accountType :" + infoArray.AccountType);
				Debug.Log("RecvChoseAccountAssociate's accountName :" + infoArray.AccountName);
				Debug.Log("RecvChoseAccountAssociate's strToken :" + infoArray.StrToken);
				//Debug.Log("RecvChoseAccountAssociate's nickname :" + infoArray.Nickname);

				mNetCtrl.dispatchEvent(FiProtoType.XL_CHOISE_ACCOUNT_LOGIN_RESPON, FiProtoHelper.toLocal_LoginAccountAssociate(infoArray));
			}
			catch (Exception ex)
			{
				Debug.Log("RecvChoseAccountAssociate error" + ex.Message);
			}
		}

		/// <summary>
		/// 接收暱稱回傳
		/// </summary>
		public void RecvNickname(byte[] data)
		{
			try
			{
				Debug.Log("RecvNickname!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!! " + data.Length);
				PB_LoginAccountNickChoice outData = PB_LoginAccountNickChoice.Parser.ParseFrom(data);
				//PBMsg_GetUserNicknameRequest outData = ProtoMsgDeserializer<PBMsg_GetUserNicknameRequest>(data);
				mNetCtrl.dispatchEvent(FiProtoType.XL_GET_NEW_NICK_RESPON, FiProtoHelper.toLocal_LoginAccountNickChoice(outData));
			}
			catch (Exception ex)
			{
				Debug.Log("RecvChoseAccountAssociate error" + ex.Message);
			}
		}
		/// <summary>
		/// 將傳入的Protocol Buffer訊息進行反序列化
		/// </summary>
		public T ProtoMsgDeserializer<T>(byte[] data)
		{
			try
			{
				using (MemoryStream ms = new MemoryStream())
				{
					ms.Write(data, 0, data.Length);
					ms.Position = 0;

					T deserizedData = Serializer.Deserialize<T>(ms);
					return deserizedData;
				}
			}
			catch (Exception ex)
			{
				Debug.Log("Deserialize failed: " + ex.ToString());
				return default(T);
			}
		}
	}
}

