using System;
using Google.Protobuf;
using System.Collections.Generic;

namespace AssemblyCSharp
{
	public class FiProtoHelper
	{
		public FiProtoHelper ()
		{
			
		}

		/**登陆协议转化*/
		public static PB_LoginRequest  toProto_LoginRequest (FiLoginRequest nRequest)
		{
			PB_LoginRequest nLogin = new PB_LoginRequest ();
			nLogin.OpenId = nRequest.openId;
			nLogin.AccessToken = nRequest.accessToken;
			nLogin.Nickname = nRequest.nickname;
			nLogin.AvatarUrl = nRequest.avatarUrl;
			nLogin.Gender = nRequest.gender;
			nLogin.Platform = nRequest.platfrom;
			return nLogin;
		}

		public static FiLoginResponse toLocal_LoginReply (PB_LoginResponse nValue)
		{
			FiLoginResponse nReply = new FiLoginResponse ();
			nReply.avatar = nValue.Avatar;
			nReply.diamond = nValue.Diamond;
			nReply.gold = nValue.Gold;
			nReply.experience = nValue.Experience;
			nReply.level = nValue.Level;
			nReply.maxCanonMultiple = nValue.MaxCannonMultiple;

			nReply.nickname = nValue.Nickname;
			nReply.reuslt = nValue.Result;
			nReply.topupSum = nValue.TopupSum;
			nReply.userId = (int)nValue.UserId;
			nReply.vipLevel = nValue.VipLevel;
			nReply.redPacketTicket = nValue.RedPacketTicket;

			nReply.cannonStyle = nValue.CannonStyle;
			nReply.sailDay = nValue.SailDay;
			nReply.redPacketTicket = nValue.RedPacketTicket;

			nReply.nextLevelExp = nValue.NextLevelExp;

			nReply.beginnerCurTask = nValue.BeginnerCurTask;
			nReply.beginnerProgress = nValue.BeginnerProgress;
			nReply.roomCard = nValue.RoomCard;

			nReply.monthlyCardDurationDays = nValue.MonthlyCardExpiryDay;
			nReply.monthlyPackGot = nValue.MonthlyPackGotToday;
			nReply.preferencePackBought = nValue.PreferencePackBought;

			nReply.loginGold = nValue.LoginGold;
			nReply.luckyGold = nValue.LuckyGold;
			nReply.luckyFishNum = nValue.LuckyFishNum;

			nReply.charm = nValue.Charm;
			nReply.charmExchangeTimes = nValue.CharmExchangeTimes;
			nReply.bankGold = nValue.BankGold;
			nReply.hasBankPswd = nValue.HasBankPswd;
			nReply.gameId = nValue.GameId;

			nReply.errorMsg = nValue.ErrorMsg;
			nReply.gender = nValue.Gender;
			nReply.testCoin = nValue.TestCoin;
//			UnityEngine.Debug.LogError ("nValue.TestCoin = " + nValue.TestCoin);
			nReply.isTestRoom = nValue.IsTestRoom;
//			UnityEngine.Debug.LogError ("nValue.nmanmonnum = " + nValue.NManmon);

			nReply.cannonBabetteStyle = nValue.CannonBottomStly;
			nReply.nmanmonnum = nValue.NManmon;
			nReply.bossMatchState = nValue.NBossMatchState;
			nReply.isNewUser = nValue.IsNewUser;
			nReply.isPaiWeiTopUpJiaCeng = nValue.IsPaiWeiTopUpJiaCeng;
			nReply.isRongYuDianTangUser = nValue.IsRongYuDianTangUser;
			nReply.IsResterUserSteate = nValue.IsResterUserSteate;
			UnityEngine.Debug.LogError ("nValue.IsNewUser===" + nValue.IsResterUserSteate);
//			UnityEngine.Debug.LogError ("toLocal_LoginReply nValue.NBossMatchState = " + nValue.NBossMatchState);

			if (nValue.SignIn != null && nValue.SignIn.Count > 0) {
				IEnumerator<PB_SignIn> nEum = nValue.SignIn.GetEnumerator ();
				while (nEum.MoveNext ()) {
					FiDailySignIn nSignIn = new FiDailySignIn ();
					nSignIn.day = nEum.Current.Day;
					nSignIn.status = nEum.Current.Status;
					nReply.signInArray.Add (nSignIn);
				}
			}
				
			if (nValue.FirstPayProducts != null) {
				IEnumerator<int> nEumPays = nValue.FirstPayProducts.GetEnumerator ();
				while (nEumPays.MoveNext ()) {
					nReply.firstPayProducts.Add (nEumPays.Current);
				}
			}
			return nReply;
		}

		/**房间匹配消息反馈*/
		public static PB_EnterRoomRequest toProto_MatchRequest (FiRoomMatchRequest nValue)
		{
			PB_EnterRoomRequest nRequst = new PB_EnterRoomRequest ();
			nRequst.EnterType = nValue.enterType;
			nRequst.RoomRatio = nValue.roomMultiple;
			return nRequst;
		}

		public static FiRoomMatchResponse  toLocal_MatchReply (PB_EnterRoomResponse nValue)
		{
			FiRoomMatchResponse nReply = new FiRoomMatchResponse ();
			nReply.result = nValue.Result;
			nReply.roomIndex = nValue.RoomIndex;
			nReply.seatIndex = nValue.SeatIndex;
			nReply.gold = nValue.Gold;
			nReply.userArray = new List< FiUserInfo> ();
			IEnumerator<OtherUserInfo> nEum = nValue.Others.GetEnumerator ();
			while (nEum.MoveNext ()) {
				FiUserInfo nInfo = new  FiUserInfo ();
				nInfo.avatar = nEum.Current.Avatar;
				nInfo.cannonMultiple = nEum.Current.CurrentCannonRatio;
				nInfo.diamond = nEum.Current.Diamond;
				nInfo.gender = nEum.Current.Gender;
				nInfo.gold = nEum.Current.Gold;
				nInfo.nickName = nEum.Current.Nickname;
				nInfo.seatIndex = nEum.Current.SeatIndex;
				nInfo.userId = nEum.Current.UserId;
				nInfo.cannonStyle = nEum.Current.CannonStyle;
				nInfo.level = nEum.Current.Level;
				nInfo.maxCannonMultiple = nEum.Current.MaxCannonMultiple;
				nInfo.vipLevel = nEum.Current.VipLevel;
				nInfo.experience = nEum.Current.Experience;
				nInfo.cannonStyle = nEum.Current.CannonStyle;
				nInfo.gameId = nEum.Current.GameId;
				nInfo.monthlyCardType = nEum.Current.MonthlyCardType;
				nInfo.cannonBabetteStyle = nEum.Current.CannonBottomType;
				nInfo.testCoin = nEum.Current.TestCoin;
				nInfo.isRoomTest = nEum.Current.IsRoomTest;
				nInfo.userChampionsRank = nEum.Current.UserChampionsRank;
				nReply.userArray.Add (nInfo);
			}
			return nReply;
		}

		/**其他用户进入房间提示*/
		public static  FiOtherEnterRoom   toLocal_OtherEnter (PB_OtherEnterRoomInfrom nValue)
		{
			FiOtherEnterRoom nOtherEnter = new  FiOtherEnterRoom ();
			nOtherEnter.enterType = nValue.EnterType;
			nOtherEnter.roomIndex = nValue.RoomIndex;
			nOtherEnter.roomMultiple = nValue.RoomRatio;

			nOtherEnter.user = new  FiUserInfo ();
			nOtherEnter.user.avatar = nValue.Other.Avatar;
//			UnityEngine.Debug.LogError ("FiOtherEnterRoom = nValue.Other.CurrentCannonRatio = " + nValue.Other.CurrentCannonRatio);	

			nOtherEnter.user.cannonMultiple = nValue.Other.CurrentCannonRatio;
			nOtherEnter.user.diamond = nValue.Other.Diamond;
			nOtherEnter.user.gender = nValue.Other.Gender;
			nOtherEnter.user.gold = nValue.Other.Gold;
			nOtherEnter.user.nickName = nValue.Other.Nickname;
			nOtherEnter.user.seatIndex = nValue.Other.SeatIndex;
			nOtherEnter.user.userId = nValue.Other.UserId;
			nOtherEnter.user.gameId = nValue.Other.GameId;
			nOtherEnter.user.level = nValue.Other.Level;
			nOtherEnter.user.maxCannonMultiple = nValue.Other.MaxCannonMultiple;
			nOtherEnter.user.vipLevel = nValue.Other.VipLevel;
			nOtherEnter.user.experience = nValue.Other.Experience;
			nOtherEnter.user.cannonStyle = nValue.Other.CannonStyle;
			nOtherEnter.user.monthlyCardType = nValue.Other.MonthlyCardType;
			nOtherEnter.user.cannonBabetteStyle = nValue.Other.CannonBottomType;
			nOtherEnter.user.testCoin = nValue.Other.TestCoin;
			nOtherEnter.user.isRoomTest = nValue.Other.IsRoomTest;
			nOtherEnter.user.userChampionsRank = nValue.Other.UserChampionsRank;
			return nOtherEnter;
		}

		/**其他用户离开房间提示*/
		public static  FiOtherLeaveRoom toLocal_OtherLeave (PB_OtherLeaveRoomInfrom  nValue)
		{
			FiOtherLeaveRoom nOtherLeave = new  FiOtherLeaveRoom ();

			//nOtherLeave.leaveType = nValue.LeaveType;
			//nOtherLeave.roomIndex  = nValue.RoomIndex;
			//nOtherLeave.roomRatio = nValue.RoomRatio;
			nOtherLeave.seatIndex = nValue.SeatIndex;
			nOtherLeave.userId = nValue.UserId;

			return nOtherLeave;
		}






		public static PB_LeaveRoomRequest  toProto_UserLeaveReq (FiLeaveRoomRequest nValue)
		{
			PB_LeaveRoomRequest nRequest = new PB_LeaveRoomRequest ();
			nRequest.LeaveType = nValue.leaveType;
			nRequest.RoomIndex = nValue.roomIndex;
			nRequest.RoomRatio = nValue.roomMultiple;
			return nRequest;
		}








		public static  FiFishsOutResponse toLocal_FishOutReply (PB_FishOutScene nValue)
		{
			FiFishsOutResponse nReply = new  FiFishsOutResponse ();
			nReply.groupId = nValue.GroupId;
			return nReply;
		}








		public static  FiLeaveRoomResponse toLocal_LeaveReply (PB_LeaveRoomResponse nValue)
		{
			FiLeaveRoomResponse nReply = new  FiLeaveRoomResponse ();
			nReply.result = nValue.Result;
			nReply.gold = nValue.Gold;
			return nReply;
		}








		public static PB_UserFireRequest toProto_UserFireBullet (FiFireBulletRequest nValue)
		{
			PB_UserFireRequest nReq = new PB_UserFireRequest ();
			nReq.BulletId = nValue.bulletId;
			nReq.CannonRatio = nValue.cannonMultiple;
			nReq.Position = new BulletPosition ();
			nReq.Position.X = nValue.position.x;
			nReq.Position.Y = nValue.position.y;

			nReq.UserId = nValue.userId;

			nReq.FishId = nValue.fishId;
			nReq.GroupId = nValue.groupId;
			nReq.Violent = nValue.violent;


//
//			UnityEngine.Debug.LogError ("PB_UserFireRequest nValue.bulletId = " + nValue.bulletId);
//			UnityEngine.Debug.LogError ("PB_UserFireRequest nValue.cannonMultiple = " + nValue.cannonMultiple);
//			UnityEngine.Debug.LogError ("PB_UserFireRequest nValue.position.x = " + nValue.position.x);
//			UnityEngine.Debug.LogError ("PB_UserFireRequest nValue.position.y = " + nValue.position.y);
//			UnityEngine.Debug.LogError ("PB_UserFireRequest nValue.userId = " + nValue.userId);
//			UnityEngine.Debug.LogError ("PB_UserFireRequest nValue.fishId = " + nValue.fishId);
//			UnityEngine.Debug.LogError ("PB_UserFireRequest nValue.groupId = " + nValue.groupId);
//			UnityEngine.Debug.LogError ("PB_UserFireRequest nValue.violent = " + nValue.violent);

			return nReq;
		}









		public static FiFishsCreatedInform toLocal_FishsCreatedInform (PB_FishGroupInfrom nValue)
		{
			FiFishsCreatedInform nInform = new FiFishsCreatedInform ();
			nInform.fishNum = nValue.FishNum;
			nInform.fishType = nValue.FishType;
			nInform.groupId = nValue.GroupId;
			nInform.trackId = nValue.TrackId;
			nInform.trackType = nValue.TrackType;
			nInform.tideType = nValue.TideType;
			return nInform;
		}










		public static  FiHitFishResponse toLocal_OtherHitFishInform (PB_FishHitInfrom nValue)
		{
			FiHitFishResponse nInfrom = new  FiHitFishResponse ();
			nInfrom.bulletId = nValue.BulletId;
			nInfrom.fishId = nValue.FishId;
			nInfrom.groupId = nValue.GroupId;
			nInfrom.userId = nValue.UserId;
			nInfrom.cannonMultiple = nValue.CannonMultiple;
			nInfrom.violent = nValue.Violent;
			//	if (nValue.Position != null) {
			//		nInfrom.position = new Cordinate ();
			//		nInfrom.position.x = nValue.Position.X;
			//		nInfrom.position.y = nValue.Position.Y;
			//	}

			nInfrom.propertyArray = new List<FiProperty> ();

			if (nValue.Properties != null) {
				IEnumerator<PB_Property> nEum = nValue.Properties.GetEnumerator ();
				while (nEum.MoveNext ()) {
					FiProperty nInfo = new  FiProperty ();
					nInfo.type = nEum.Current.PropertyType;
					nInfo.value = nEum.Current.Sum;
					nInfrom.propertyArray.Add (nInfo);
				}
			}
//			UnityEngine.Debug.LogError ("FiHitFishResponse nValue.bulletId = " + nValue.BulletId);
//			UnityEngine.Debug.LogError ("FiHitFishResponse nValue.FishId = " + nValue.FishId);
//			UnityEngine.Debug.LogError ("FiHitFishResponse nValue.GroupId = " + nValue.GroupId);
//			UnityEngine.Debug.LogError ("FiHitFishResponse nValue.UserId = " + nValue.UserId);
//			UnityEngine.Debug.LogError ("FiHitFishResponse nValue.CannonMultiple = " + nValue.CannonMultiple);
//			UnityEngine.Debug.LogError ("FiHitFishResponse nValue.Violent = " + nValue.Violent);

			return nInfrom;
		}









		public static  FiOtherFireBulletInform  toLocal_OtherFireInform (PB_UserFireRequest  nValue)
		{
			FiOtherFireBulletInform nFireInform = new  FiOtherFireBulletInform ();
			nFireInform.bulletId = nValue.BulletId;
			nFireInform.cannonMultiple = nValue.CannonRatio;

			nFireInform.position = new  Cordinate ();
			nFireInform.position.x = nValue.Position.X;
			nFireInform.position.y = nValue.Position.Y;

			nFireInform.userId = nValue.UserId;

			nFireInform.fishId = nValue.FishId;
			nFireInform.groupId = nValue.GroupId;
//
//			UnityEngine.Debug.LogError ("nFireInform.bulletId = " + nFireInform.bulletId);
//			UnityEngine.Debug.LogError ("nFireInform.cannonMultiple = " + nFireInform.cannonMultiple);
//			UnityEngine.Debug.LogError ("nFireInform.userId = " + nFireInform.userId);
//			UnityEngine.Debug.LogError ("nFireInform.fishId = " + nFireInform.fishId);
//			UnityEngine.Debug.LogError ("nFireInform.groupId = " + nFireInform.groupId);
//			UnityEngine.Debug.LogError ("nFireInform.cannonMultiple = " + nFireInform.cannonMultiple);

			return nFireInform;
		}








		public static FiOtherChangeCannonMultipleInform toLocal_CannonChangeInform (PB_OtherChangeCannon nValue)
		{
			FiOtherChangeCannonMultipleInform nInform = new FiOtherChangeCannonMultipleInform ();
			nInform.cannonMultiple = nValue.CannonMultiple;
			nInform.userId = nValue.UserId;
			return nInform;
		}





		public static FiChangeCannonMultipleResponse toLocal_CannonChangeResponse (PB_ChangeCannonResponse nValue)
		{
			FiChangeCannonMultipleResponse nInform = new FiChangeCannonMultipleResponse ();
			nInform.result = nValue.Result;
			nInform.cannonMultiple = nValue.CannonMultiple;
			return nInform;
		}



		public static PB_ChangeCannonRequest toProto_CannonChangeRequest (FiChangeCannonMultipleRequest nValue)
		{
			PB_ChangeCannonRequest nInform = new PB_ChangeCannonRequest ();
			nInform.CannonMultiple = nValue.cannonMultiple;

			return nInform;
		}






		public static PB_FishHitInfrom toProto_HitFishRequest (FiHitFishRequest nHitReq)
		{

			PB_FishHitInfrom nHitInform = new PB_FishHitInfrom ();
			nHitInform.BulletId = nHitReq.bulletId;
			nHitInform.CannonMultiple = nHitReq.cannonMultiple;
			nHitInform.FishId = nHitReq.fishId;
			nHitInform.GroupId = nHitReq.groupId;
			nHitInform.UserId = nHitReq.userId;
			if (nHitReq.position != null) {
				nHitInform.Position = new BulletPosition ();
				nHitInform.Position.X = nHitReq.position.x;
				nHitInform.Position.Y = nHitReq.position.y;
			}
			nHitInform.Violent = nHitReq.violent;
			nHitInform.LongLiuShuiGold = nHitReq.longLiuShuiGold;
			nHitInform.BeiyongFishID = nHitReq.beiyongFishID;
//			UnityEngine.Debug.LogError ("[-----hit fish-----nHitReq.beiyongFishID]" + nHitInform.BeiyongFishID);

//			UnityEngine.Debug.LogError ("[-----hit fish-----]" + nHitInform);
			return nHitInform;
		}





		public static PB_FishOutScene toProto_FishOutScene (FiFishsOutRequest nValue)
		{
			PB_FishOutScene nInform = new PB_FishOutScene ();
			nInform.GroupId = nValue.groupId;
			return nInform;
		}


	




		//发送特效请求
		public static PB_EffectRequest toProto_EffectRequest (FiEffectRequest nValue)
		{
			PB_EffectRequest nReq = new PB_EffectRequest ();
			if (nValue.effect != null) {
				nReq.Effect = new PB_Effect ();
				nReq.Effect.Id = nValue.effect.type;
			}
			nReq.UserId = nValue.userId;
			return nReq;
		}






		public static FiEffectResponse toLocal_EffectResponse (PB_EffectResponse nValue)
		{
			FiEffectResponse nRes = new FiEffectResponse ();
			nRes.result = nValue.Result;

			nRes.info = new FiEffectInfo ();

			if (nValue.Effect != null) {
			
				nRes.info.type = nValue.Effect.Id;
				nRes.info.value = new List<int> ();

				IEnumerator<int> nEum = nValue.Effect.Target.GetEnumerator ();
				while (nEum.MoveNext ()) {
					nRes.info.value.Add (nEum.Current);
				}

			}
			return nRes;
		}




		public static FiFreezeTimeOutInform toLocal_FreezeInfrom (FreezeTimeout nValue)
		{
			FiFreezeTimeOutInform nRes = new FiFreezeTimeOutInform ();
			nRes.value = new List<int> ();

			if (nValue.Target != null) {
				IEnumerator<int> nEum = nValue.Target.GetEnumerator ();
				while (nEum.MoveNext ()) {
					nRes.value.Add (nEum.Current);
				}

			}
			return nRes;
		}





		public static FiOtherEffectInform toLocal_EffectInfrom (PB_OtherEffect nValue)
		{
			FiOtherEffectInform nRes = new FiOtherEffectInform ();
			nRes.userId = nValue.UserId;

			nRes.info = new FiEffectInfo ();
			if (nValue.Effect != null) {

				nRes.info.type = nValue.Effect.Id;
				nRes.info.value = new List<int> ();

				IEnumerator<int> nEum = nValue.Effect.Target.GetEnumerator ();
				while (nEum.MoveNext ()) {
					nRes.info.value.Add (nEum.Current);
				}

			}
			return nRes;
		}






		public static TopUpRequest  toProto_TopupRequest (FiTopUpRequest nValue)
		{
			TopUpRequest nRequest = new TopUpRequest ();
			nRequest.Type = nValue.type;
			nRequest.RMB = nValue.RMB;
			return nRequest;
		}





		public static FiTopUpResponse toLocal_TopUpResponse (TopUpResponse nValue)
		{
			FiTopUpResponse nRes = new FiTopUpResponse ();
			nRes.result = nValue.Result;
			nRes.sum = nValue.Sum;
			nRes.type = nValue.Type;
			return nRes;
		}






		public static FiBackpackResponse toLocal_BackpackResponse (GetBackpackPropertyResponse nValue)
		{
			FiBackpackResponse nRes = new  FiBackpackResponse ();
			nRes.result = nValue.Result;

			nRes.properties = new List<FiBackpackProperty> ();
			if (nValue.Properties != null) {
				IEnumerator<BackpackProperty> nEum = nValue.Properties.GetEnumerator ();
				while (nEum.MoveNext ()) {
					FiBackpackProperty nSingle = new FiBackpackProperty ();
					nSingle.canGiveAway = nEum.Current.CanGiveAway;
					nSingle.description = nEum.Current.Description;
					nSingle.diamondCost = nEum.Current.DiamondCost;
					nSingle.id = nEum.Current.Id;

					nSingle.name = nEum.Current.Name;
					nSingle.type = nEum.Current.Type;
					nSingle.useable = nEum.Current.Useable;
					nSingle.count = nEum.Current.Count;
					nSingle.propTime = nEum.Current.PropTime;

					nRes.properties.Add (nSingle);
				}
			}
			return nRes;
		}

		public static FiSystemReward toLocal_SystemReward (PB_SystemReward nValue)
		{
			FiSystemReward info = new FiSystemReward ();
			info.resultCode = nValue.ResultCode;
			info.propID = nValue.PropID;
			info.propCount = nValue.PropCount;
			info.msg = nValue.Msg;
			return info;
		}

        //綁定手機
		public static FiConvertFormalBindAccount toLocal_BindPhoneReward(PB_ConvertFormalBindAccount nValue)
		{
			FiConvertFormalBindAccount info = new FiConvertFormalBindAccount();
			info.result = nValue.Result;
			info.userID = nValue.UserID;
			info.phone = nValue.StrPhoneNum;
			info.code = nValue.StrCode;

			UnityEngine.Debug.Log("@@@ info.result = "+ info.result);
			return info;
		}

		public static FiHelpGoldTaskData toLocal_HelpGoldTaskData (PB_HelpGoldTaskData nValue)
		{
			FiHelpGoldTaskData info = new FiHelpGoldTaskData ();
			info.resultCode = nValue.ResultCode;
			info.taskID = nValue.TaskID;
			info.nValue = nValue.NValue;
			info.propID = nValue.PropID;
			info.count = nValue.Count;
			info.dec = nValue.Dec;
			return info;
		}

		/// <summary>
		/// 兑换炮座回复
		/// </summary>
		/// <returns>The local fi exchange barbette data.</returns>
		/// <param name="nValue">N value.</param>
		public static FiExchangeBarbette toLocal_FiExchangeBarbetteData (useBuyCannonBottomInfo nValue)
		{
			FiExchangeBarbette barbetteInfo = new FiExchangeBarbette ();
			barbetteInfo.buyType = nValue.BuyType;
			barbetteInfo.goldCost = nValue.Gold;
			barbetteInfo.result = nValue.Result;
			return barbetteInfo;
		}
		//是否绑定手机号
		public static FiIsBindPhoneResponse toLocal_FiIsBindPhoneResponse (GetBindPhoneState nResult)
		{
			FiIsBindPhoneResponse result = new FiIsBindPhoneResponse ();
			result.isBindPhone = nResult.IsBindPhone;
			result.strPhoneNum = nResult.StrPhoneNum;
			return result;
		}

		//获取所有已经使用的 限时道具信息 Arr信息
		public static FiUseProTimeArr toLocal_UseProTimeArr (UsePropTimeArray nValue)
		{
			FiUseProTimeArr nResponseOut = new FiUseProTimeArr ();
			if (nValue.UseProp != null) {
				IEnumerator<usePropTime> nEumPropty = nValue.UseProp.GetEnumerator ();
				while (nEumPropty.MoveNext ()) {
					FiUserGetAllPropTimeResponse nSignle = new FiUserGetAllPropTimeResponse ();
					nSignle.propID = nEumPropty.Current.PropID;
					nSignle.propType = nEumPropty.Current.PropType;
					nSignle.remainTime = nEumPropty.Current.RemainTime;
					nSignle.useTime = nEumPropty.Current.UseTime;
					nSignle.propTime = nEumPropty.Current.NPropTime;
//					UnityEngine.Debug.Log ("toLocal_UseProTimeArr nEumPropty.Current.PropType= " + nEumPropty.Current.PropType);
//					UnityEngine.Debug.Log ("toLocal_UseProTimeArr nEumPropty.Current.RemainTime= " + nEumPropty.Current.RemainTime);
//					UnityEngine.Debug.Log ("toLocal_UseProTimeArr nEumPropty.Current.PropID= " + nEumPropty.Current.PropID);
//					UnityEngine.Debug.Log ("toLocal_UseProTimeArr nEumPropty.Current.UseTime= " + nEumPropty.Current.UseTime);
					nResponseOut.allProp.Add (nSignle);
				}
			}
			return nResponseOut;

		}

		//获取所有已经使用的 限时道具信息 单独的usePropTime
		public static FiUserGetAllPropTimeResponse toLocal_FiUserGetAllPropTimeResponse (usePropTime nResult)
		{
			FiUserGetAllPropTimeResponse result = new FiUserGetAllPropTimeResponse ();
			result.propID = nResult.PropID;
			result.propType = nResult.PropType;
			result.remainTime = nResult.RemainTime;
			result.useTime = nResult.UseTime;
			result.propTime = nResult.NPropTime;
			return result;
		}
		//使用限时道具请求
		public static FiUsePropTimeResponse toLocal_FiUsePropTimeResponse (UsePropTimeExResponse nResult)
		{
			FiUsePropTimeResponse result = new FiUsePropTimeResponse ();
			result.userID = nResult.UserID;
			result.resultCode = nResult.ResultCode;
			result.useProp = new FiUserGetAllPropTimeResponse ();
			if (result.useProp != null) {
				result.useProp.propID = nResult.UseProp.PropID;
				result.useProp.propType = nResult.UseProp.PropType;
				result.useProp.useTime = nResult.UseProp.UseTime;
				result.useProp.remainTime = nResult.UseProp.RemainTime;
				result.useProp.propTime = nResult.UseProp.NPropTime;
			}
			return result;
		}
		//删除道具请求
		public static FiDelPropTimeResponse toLocal_FiDelPropTimeResponse (DelUsePropTimeEx nReslut)
		{
			FiDelPropTimeResponse result = new FiDelPropTimeResponse ();
			result.userID = nReslut.UserID;
			result.delPropID = nReslut.DelPropID;
			result.resultCode = nReslut.ResultCode;
			return result;
		}

		public static FiRetroactiveReward toLocal_SignRetroactive (PB_SignRetroactiveResponse nValue)
		{
			
			FiRetroactiveReward nResponseOut = new FiRetroactiveReward ();
			nResponseOut.result = nValue.Result;
			if (nValue.SignIn != null) {
				nResponseOut.singIn.day = nValue.SignIn.Day;
				nResponseOut.singIn.status = nValue.SignIn.Status;
			}
				

			if (nValue.Property != null) {
				IEnumerator< PB_Property > nEumPropty = nValue.Property.GetEnumerator ();
				while (nEumPropty.MoveNext ()) {
					FiProperty nSingle = new FiProperty ();
					nSingle.type = nEumPropty.Current.PropertyType;
					nSingle.value = nEumPropty.Current.Sum;
					nResponseOut.properties.Add (nSingle);
				}
			}
			return nResponseOut;
		}




		/*public static PB_GetPKRoomListRequest toProto_GetPKRoomRequest( FiGetPkRoomListRequest nValue )
		{
			PB_GetPKRoomListRequest nReq = new PB_GetPKRoomListRequest ();
			nReq.PageNum = nValue.pageNum;
			nReq.RoomType = nValue.roomType;
			return nReq;
		}*/


		public static PB_CreatePKRoomRequest toProto_CreateRoomRequest (FiCreatePKRoomRequest nValue)
		{
			PB_CreatePKRoomRequest nReq = new PB_CreatePKRoomRequest ();
			nReq.BulletType = nValue.bulletType;
			nReq.GoldType = nValue.goldType;
			nReq.PlayerNumType = nValue.playerNumType;
			nReq.RoomName = nValue.roomName;
			nReq.RoomPassword = nValue.roomPassword;
			nReq.RoomType = nValue.roomType;
			nReq.TimeType = nValue.timeType;
			nReq.PointType = nValue.pointType;
			return nReq;
		}



		public static FiCreatePKRoomResponse toLocal_CreateRoomResponse (PB_CreatePKRoomResponse nValue)
		{
			FiCreatePKRoomResponse nRes = new FiCreatePKRoomResponse ();
			nRes.result = nValue.Result;
			nRes.roomType = nValue.RoomType;
			nRes.seatIndex = nValue.SeatIndex;

			nRes.info = new FiPkRoomInfo ();
			if (nValue.Room != null) {
				nRes.info.roomType = nValue.Room.RoomType;
				nRes.info.roundType = nValue.Room.RoundType;
				//			nRes.info.bulletType = nValue.Room.BulletType;
				nRes.info.goldType = nValue.Room.GoldType;
				//			nRes.info.playerNumType = nValue.Room.PlayerNumType;
				nRes.info.roomIndex = nValue.Room.RoomIndex;
				//			nRes.info.roomName = nValue.Room.RoomName;
				//			nRes.info.hasPassword = nValue.Room.RoomPassword;
				nRes.info.timeType = nValue.Room.TimeType;
				//		nRes.info.pointType = nValue.Room.PointType;
				nRes.info.begun = nValue.Room.Begun;
				//		nRes.info.createTime = nValue.Room.CreateTime;
				nRes.info.currentPlayerCount = nValue.Room.CurrentPlayerNum;
			}
			return nRes;
		}



		/*public static FiGetPkRoomListResponse toLocal_GetRoomListResponse( PB_GetPKRoomListResponse nValue )
		{
			FiGetPkRoomListResponse nRes = new FiGetPkRoomListResponse ();
			nRes.roomType = nValue.RoomType;
			nRes.pageNum =  nValue.PageNum;
			nRes.infoArray = new List<FiPkRoomInfo> ();
			nRes.result = nValue.Result;
			IEnumerator<PB_PKRoomInfo> nEum = nValue.Rooms.GetEnumerator ();
			while( nEum.MoveNext() )
			{
				FiPkRoomInfo nSingle = new FiPkRoomInfo ();
				nSingle.roomType = nEum.Current.RoomType;
				nSingle.roundType = nEum.Current.RoundType;
		//		nSingle.bulletType = nEum.Current.BulletType;
				nSingle.goldType = nEum.Current.GoldType;
		//		nSingle.playerNumType = nEum.Current.PlayerNumType;

				nSingle.roomIndex    = nEum.Current.RoomIndex;
		//		nSingle.roomName     =  nEum.Current.RoomName;
		//		nSingle.hasPassword =  nEum.Current.RoomPassword;
				nSingle.timeType     =  nEum.Current.TimeType;
		//		nSingle.pointType = nEum.Current.PointType;
				nSingle.begun = nEum.Current.Begun;
				nSingle.currentPlayerCount = nEum.Current.CurrentPlayerNum;
			//	nSingle.createTime = nEum.Current.CreateTime;
				nRes.infoArray.Add ( nSingle );
			}

			return nRes;
		}*/






		public static PB_StartPKGameRequest toProto_StartPkGame (FiStartPKGameRequest nValue)
		{
			PB_StartPKGameRequest nPkStart = new PB_StartPKGameRequest ();
			nPkStart.RoomIndex = nValue.roomIndex;
			nPkStart.RoomType = nValue.roomType;
			return nPkStart;
		}

		public static FiStartPKGameResponse toLocal_StartPkResponse (PB_StartPKGameResponse  nValue)
		{
			FiStartPKGameResponse nResponse = new FiStartPKGameResponse ();
			nResponse.result = nValue.Result;
			nResponse.roomIndex = nValue.RoomIndex;
			nResponse.roomType = nValue.RoomType;
			return nResponse;
		}



		public static FiStartPKGameInform toLocal_OwnerStartPkResponse (PB_NotifyPKGameStart  nValue)
		{
			FiStartPKGameInform nResponse = new FiStartPKGameInform ();
			nResponse.roomIndex = nValue.RoomIndex;
			nResponse.roomType = nValue.RoomType;
			return nResponse;
		}


		public static PB_EnterPKRoomRequest toProto_EnterPkGame (FiEnterPKRoomRequest nValue)
		{
			PB_EnterPKRoomRequest nPkStart = new PB_EnterPKRoomRequest ();
			nPkStart.GoldType = nValue.goldType;
			nPkStart.RoomType = nValue.roomType;
			return nPkStart;
		}


		public static FiEnterPKRoomResponse toLocal_EnterPkRoomResponse (PB_EnterPKRoomResponse nValue)
		{
			FiEnterPKRoomResponse nResponse = new FiEnterPKRoomResponse ();
			nResponse.seatindex = nValue.SeatIndex;
			nResponse.goldType = nValue.GoldType;
			nResponse.roomIndex = nValue.RoomIndex;
			nResponse.roomType = nValue.RoomType;
			nResponse.result = nValue.Result;
			nResponse.others = new List<FiUserInfo> ();

			IEnumerator<OtherUserInfo> nEum = nValue.Others.GetEnumerator ();
			while (nEum.MoveNext ()) {
				FiUserInfo nInfo = new  FiUserInfo ();
				nInfo.avatar = nEum.Current.Avatar;
				nInfo.cannonMultiple = nEum.Current.CurrentCannonRatio;
				nInfo.diamond = nEum.Current.Diamond;
				nInfo.gender = nEum.Current.Gender;
				nInfo.gold = nEum.Current.Gold;
				nInfo.nickName = nEum.Current.Nickname;
				nInfo.seatIndex = nEum.Current.SeatIndex;
				nInfo.userId = nEum.Current.UserId;

				nInfo.level = nEum.Current.Level;
				nInfo.maxCannonMultiple = nEum.Current.MaxCannonMultiple;
				nInfo.vipLevel = nEum.Current.VipLevel;
				nInfo.experience = nEum.Current.Experience;
				nInfo.cannonStyle = nEum.Current.CannonStyle;
				nInfo.gameId = nEum.Current.GameId;
				nInfo.monthlyCardType = nEum.Current.MonthlyCardType;
				nInfo.cannonBabetteStyle = nEum.Current.CannonBottomType;
				nInfo.testCoin = nEum.Current.TestCoin;
				nInfo.isRoomTest = nEum.Current.IsRoomTest;
				nInfo.userChampionsRank = nEum.Current.UserChampionsRank;
				nResponse.others.Add (nInfo);
			}
			return nResponse;
		}

		public static PB_LeavePKRoomRequest toProto_LeavePkGame (FiLeavePKRoomRequest nValue)
		{
			PB_LeavePKRoomRequest nPkStart = new PB_LeavePKRoomRequest ();
			nPkStart.RoomIndex = nValue.roomIndex;
			nPkStart.RoomType = nValue.roomType;
			nPkStart.GoldType = nValue.goldType;
			return nPkStart;
		}


		public static FiLeavePKRoomResponse toLocal_LeavePkRoomResponse (PB_LeavePKRoomResponse nValue)
		{
			FiLeavePKRoomResponse nLeaveRoom = new FiLeavePKRoomResponse ();
			nLeaveRoom.result = nValue.Result;
			nLeaveRoom.roomIndex = nValue.RoomIndex;
			nLeaveRoom.roomType = nValue.RoomType;
			return nLeaveRoom;
		}

		public static FiOtherLeavePKRoomInform toLocal_OtherLeavePkRoomResponse (PB_NotifyOtherLeavePKRoom nValue)
		{
			FiOtherLeavePKRoomInform nLeaveRoom = new FiOtherLeavePKRoomInform ();
			nLeaveRoom.leaveUserId = nValue.LeaveUserId;
			//		nLeaveRoom.roomOwnerUserId = nValue.RoomOwnerUserId;
			nLeaveRoom.seatIndex = nValue.SeatIndex;
			return nLeaveRoom;
		}


		public static FiOtherEnterPKRoomInform toLocal_OtherEnterPkRoom (PB_NotifyOtherEnterPKRoom nValue)
		{
			FiOtherEnterPKRoomInform nEnterRoom = new FiOtherEnterPKRoomInform ();
			nEnterRoom.enterType = nValue.EnterType;
			nEnterRoom.roomIndex = (int)nValue.RoomIndex;
			nEnterRoom.other = new FiUserInfo ();
			if (nValue.Other != null) {
			
				nEnterRoom.other.avatar = nValue.Other.Avatar;
				nEnterRoom.other.cannonMultiple = nValue.Other.CurrentCannonRatio;
				nEnterRoom.other.diamond = nValue.Other.Diamond;
				nEnterRoom.other.experience = nValue.Other.Experience;
				nEnterRoom.other.gender = nValue.Other.Gender;
				nEnterRoom.other.gold = nValue.Other.Gold;
				nEnterRoom.other.level = nValue.Other.Level;
				nEnterRoom.other.maxCannonMultiple = nValue.Other.MaxCannonMultiple;
				nEnterRoom.other.nickName = nValue.Other.Nickname;
				nEnterRoom.other.seatIndex = nValue.Other.SeatIndex;
				nEnterRoom.other.userId = nValue.Other.UserId;
				nEnterRoom.other.vipLevel = nValue.Other.VipLevel;
				nEnterRoom.other.cannonStyle = nValue.Other.CannonStyle;
				nEnterRoom.other.gameId = nValue.Other.GameId;
				nEnterRoom.other.monthlyCardType = nValue.Other.MonthlyCardType;
				nEnterRoom.other.cannonBabetteStyle = nValue.Other.CannonBottomType;
				nEnterRoom.other.testCoin = nValue.Other.TestCoin;
				nEnterRoom.other.isRoomTest = nValue.Other.IsRoomTest;
				nEnterRoom.other.userChampionsRank = nValue.Other.UserChampionsRank;
			}
			return nEnterRoom;
		}

		public static FiDraGonRewardData toLocal_DraGonData (DBGetMonthlyCardReward nValue)
		{
//			UnityEngine.Debug.LogError ("toLocal_DraGonData" + nValue.DraGonreward.Count);

			FiDraGonRewardData dragonDate = new FiDraGonRewardData ();
			dragonDate.cannonmultiplemax = nValue.Cannonmultiplemax;
			IEnumerator<int> nEum = nValue.DraGonreward.GetEnumerator ();
			while (nEum.MoveNext ()) {
				dragonDate.DarGonCardDataArray.Add (nEum.Current);	
			}
			return dragonDate;
		}

		public static FiPurChaseDraGonRewradData toLocal_PurchaseDraGonData (DBTopUpMonthlyCardResponse nValue)
		{
			FiPurChaseDraGonRewradData purdragonDate = new FiPurChaseDraGonRewradData ();
			purdragonDate.cannonmultiplemax = nValue.Cannonmultiplemax;
			purdragonDate.result = nValue.Result;
			purdragonDate.cardType = nValue.CardType;
			purdragonDate.current_vip = nValue.CurrentVip;
			purdragonDate.total_recharge = nValue.TotalRecharge;
			purdragonDate.userid = nValue.Userid;

			if (nValue.Property != null) {
				IEnumerator< PB_Property > nEumPropty = nValue.Property.GetEnumerator ();
				while (nEumPropty.MoveNext ()) {
					FiProperty DragonData = new FiProperty ();
					DragonData.type = nEumPropty.Current.PropertyType;
					DragonData.value = nEumPropty.Current.Sum;
					purdragonDate.prop.Add (DragonData);
				}
			}
			return purdragonDate;

		}

		public static FiRewardAllData toLocal_RewardAllData (PB_RewardAllData nValue)
		{
			FiRewardAllData rewardAllData = new FiRewardAllData ();
			rewardAllData.rewardAll = new List<FiRewardStructure> ();
			if (null != nValue.Reward) {
				IEnumerator<PB_RewardStructure> nEum = nValue.Reward.GetEnumerator ();
				while (nEum.MoveNext ()) {
					FiRewardStructure nSingle = new FiRewardStructure ();
					nSingle.RewardType = nEum.Current.RewardType;
					nSingle.TaskID = nEum.Current.TaskID;
					nSingle.TaskValue = nEum.Current.TaskValue;
					nSingle.rewardPro = new List<FiProperty> ();
					if (null != nEum.Current.RewardPro) {
						IEnumerator<PB_Property> nEumRewardPro = nEum.Current.RewardPro.GetEnumerator ();
						while (nEumRewardPro.MoveNext ()) {
							FiProperty property = new FiProperty ();
							property.type = nEumRewardPro.Current.PropertyType;
							property.value = nEumRewardPro.Current.Sum;
							nSingle.rewardPro.Add (property);	
						}
					}

					rewardAllData.rewardAll.Add (nSingle);
				}
			
			}
			return rewardAllData;
		}
		//特惠 数据解析
		public static FiPreferentialData toLocal_PreferentialData (DBGetPreferentialReward value)
		{
//			UnityEngine.Debug.LogError ("tehui___________________");
			FiPreferentialData preferentialData = new FiPreferentialData ();
			preferentialData.cannonmultiplemax = value.Cannonmultiplemax;
			IEnumerator<int> enumerator = value.DraGonreward.GetEnumerator ();
			while (enumerator.MoveNext ()) {
				preferentialData.preferentialDataArray.Add (enumerator.Current);
			}
			return preferentialData;
		}

		//特惠 购买成功数据解析
		public static FiPurChaseTehuiRewradData toLocal_PreferentialData1 (DBGetPreferentialResponse nValue)
		{
			FiPurChaseTehuiRewradData purdragonDate = new FiPurChaseTehuiRewradData ();
			purdragonDate.result = nValue.Result;
			purdragonDate.cardType = nValue.CardType;
			purdragonDate.current_vip = nValue.CurrentVip;
			purdragonDate.total_recharge = nValue.TotalRecharge;
			purdragonDate.userid = nValue.Userid;
			purdragonDate.cannonmultiplemax = nValue.Cannonmultiplemax;


//			UnityEngine.Debug.LogError ("purdragonDate.result" + purdragonDate.result);
//			UnityEngine.Debug.LogError ("purdragonDate.cardType" + purdragonDate.cardType);
//			UnityEngine.Debug.LogError ("purdragonDate.current_vip" + purdragonDate.current_vip);
//			UnityEngine.Debug.LogError ("purdragonDate.total_recharge" + purdragonDate.total_recharge);
//			UnityEngine.Debug.LogError ("purdragonDate.total_recharge" + purdragonDate.userid);

			if (nValue.Property != null) {
				IEnumerator< PB_Property > nEumPropty = nValue.Property.GetEnumerator ();
				while (nEumPropty.MoveNext ()) {
					FiProperty DragonData = new FiProperty ();
					DragonData.type = nEumPropty.Current.PropertyType;
					DragonData.value = nEumPropty.Current.Sum;
					purdragonDate.prop.Add (DragonData);
				}
			}
			return purdragonDate;
		}

		public static FiChangeBarbetteStyle toLocal_ChangeBarbetteStyleData (EquipmentCannonBottom cannonBottom)
		{
			FiChangeBarbetteStyle barbetteStyle = new FiChangeBarbetteStyle ();
			barbetteStyle.EquipmentType = cannonBottom.EquipmentType;
			barbetteStyle.propID = cannonBottom.PropID;
			barbetteStyle.result = cannonBottom.Result;
			barbetteStyle.removeChangeBarbetteStyle = cannonBottom.RemoveCannonBottom;
			return barbetteStyle;
		}

		public static FiFishRoomShow toLocal_ManmonStartData (ManmonCount cannonBottom)
		{
			FiFishRoomShow fifishrommshow = new FiFishRoomShow ();
			fifishrommshow.userId = cannonBottom.LUserID;
			fifishrommshow.Startnum = cannonBottom.NManmonCount;
//			UnityEngine.Debug.LogError ("nValue.nmanmonnum = " + cannonBottom.NManmonCount);
			return fifishrommshow;
		}

		public static GetManmonChipGolds toLocal_ManmonSettingData (GetManmonChipGold cannonBottom)
		{
			
			GetManmonChipGolds manmonshipgolds = new GetManmonChipGolds ();
//			UnityEngine.Debug.LogError ("nValue.nmanmonnum = " + manmonshipgolds.nManmonCount);
			manmonshipgolds.curGold = cannonBottom.CurGold;
			manmonshipgolds.result = cannonBottom.NResult;
			manmonshipgolds.nManmonCount = cannonBottom.NManmonCount;
			manmonshipgolds.userid = cannonBottom.LUserID;
			IEnumerator<long> nEum = cannonBottom.ShipGold.GetEnumerator ();
			while (nEum.MoveNext ()) {
				manmonshipgolds.shipGoldDataArray.Add (nEum.Current);	
			}
			return manmonshipgolds;
		}

		public static FiFishGoldShow toLocal_ManmonBettingData (ChipJettorGold cannonBottom)
		{
			FiFishGoldShow manmonshipgolds = new FiFishGoldShow ();
			manmonshipgolds.userid = cannonBottom.LUserID;
			manmonshipgolds.result = cannonBottom.NResult;
			manmonshipgolds.chipIndex = cannonBottom.ChipIndex;
			manmonshipgolds.nChaValue = cannonBottom.NChaValue;
			manmonshipgolds.selfGold = cannonBottom.SelfGold;
			manmonshipgolds.nWinGold = cannonBottom.NWinGold;
			manmonshipgolds.nTax = cannonBottom.NTax;
			manmonshipgolds.showTime = cannonBottom.Showtime;
			manmonshipgolds.nManmoncount = cannonBottom.NManmonCount;
			return manmonshipgolds;
		}

		public static FiFishYGoldShow toLocal_ManmonYaoQianShuData (ChipJettorGold cannonBottom)
		{
			FiFishYGoldShow manmonshipgolds = new FiFishYGoldShow ();
			manmonshipgolds.userid = cannonBottom.LUserID;
			manmonshipgolds.result = cannonBottom.NResult;
			manmonshipgolds.chipIndex = cannonBottom.ChipIndex;
			manmonshipgolds.nChaValue = cannonBottom.NChaValue;
			manmonshipgolds.selfGold = cannonBottom.SelfGold;
			manmonshipgolds.nWinGold = cannonBottom.NWinGold;
			manmonshipgolds.nTax = cannonBottom.NTax;
			manmonshipgolds.showTime = cannonBottom.Showtime;
			manmonshipgolds.nManmoncount = cannonBottom.NManmonCount;
			manmonshipgolds.mbeishu = cannonBottom.NBeiShu;
			UnityEngine.Debug.LogError ("yaoqianshu__________::" + manmonshipgolds.mbeishu + cannonBottom.NBeiShu);
			return manmonshipgolds;
		}

		public static FiFishGetCoinPool toLocal_GetCoinTurnTablePool (GetLongRewardPoolCount getLongRewardPoolCount)
		{
			FiFishGetCoinPool fishGetCoinPool = new FiFishGetCoinPool ();
			fishGetCoinPool.userID = getLongRewardPoolCount.LUserID;
			fishGetCoinPool.result = getLongRewardPoolCount.NResult;
			fishGetCoinPool.type = getLongRewardPoolCount.NType;
			IEnumerator<long> EnmSavePool = getLongRewardPoolCount.SaveLongRewardPoolGold.GetEnumerator ();
			while (EnmSavePool.MoveNext ()) {
				fishGetCoinPool.saveLongRewardPoolGold.Add (EnmSavePool.Current);
			}
			return fishGetCoinPool;
		}

		public static FishGetRankReward toLocal_ManmonRankReward (GetManmonRewardInfo getLongRewardPoolCount)
		{
			FishGetRankReward fishGetCoinPool = new FishGetRankReward ();
			fishGetCoinPool.result = getLongRewardPoolCount.NResult;
			if (getLongRewardPoolCount.SaveManmonRewardInfo != null) {
				IEnumerator< PB_Property > nEumPropty = getLongRewardPoolCount.SaveManmonRewardInfo.GetEnumerator ();
				while (nEumPropty.MoveNext ()) {
					FiProperty DragonData = new FiProperty ();
					DragonData.type = nEumPropty.Current.PropertyType;
					DragonData.value = nEumPropty.Current.Sum;
					fishGetCoinPool.rankcout.Add (DragonData);
				}
			}
			return fishGetCoinPool;
		}

		public static FishGetGoldLiuShui toLocal_TurnTableLiuShui (LongLiuShuiGold longLiuShuiGold)
		{
			FishGetGoldLiuShui fishGetGoldLiu = new FishGetGoldLiuShui ();
			fishGetGoldLiu.lUserID = longLiuShuiGold.LUserID;
			fishGetGoldLiu.lLongLiuShui = longLiuShuiGold.LLongLiuShui;
			fishGetGoldLiu.lTimeData = longLiuShuiGold.LTimeData;
			fishGetGoldLiu.lManmonExp = longLiuShuiGold.LManmonExp;
			fishGetGoldLiu.ShengJiDuanWei = longLiuShuiGold.NShengJiDuanWei;
			//新加的礼包判断
			fishGetGoldLiu.nIsUserTopUpState = longLiuShuiGold.NIsUserTopUpState;
			fishGetGoldLiu.nTwoSelectOneTopUpState = longLiuShuiGold.NTwoSelectOneTopUpState;
			fishGetGoldLiu.nThreeSelectOneTopUpdate = longLiuShuiGold.NThreeSelectOneTopUpdate;
			//新增升降段位
			fishGetGoldLiu.nChangeCurDuanWei = longLiuShuiGold.NChangeCurDuanWei;
			fishGetGoldLiu.nCurMax = longLiuShuiGold.NCurMax;
			fishGetGoldLiu.nCurRank = longLiuShuiGold.NCurRank;
			//新加无用的
			fishGetGoldLiu.nSevenTaskID = longLiuShuiGold.NSevenTaskID;
			fishGetGoldLiu.nSevenTaskValue = longLiuShuiGold.NSevenTaskValue;

			UnityEngine.Debug.Log(" ~~~~~~~~~~ nIsUserTopUpState = "+ fishGetGoldLiu.nIsUserTopUpState);
			UnityEngine.Debug.Log(" ~~~~~~~~~~ nTwoSelectOneTopUpState = " + fishGetGoldLiu.nTwoSelectOneTopUpState);
			UnityEngine.Debug.Log(" ~~~~~~~~~~ nThreeSelectOneTopUpdate = " + fishGetGoldLiu.nThreeSelectOneTopUpdate);
			return fishGetGoldLiu;
		}

		public static FishTurnTableLuckyDraw toLocal_TurnTableLuckDraw (GetFishLuckyDrawResponse getFishLuckyDrawResponse)
		{
			FishTurnTableLuckyDraw fishTurnTableLuckyDraw = new FishTurnTableLuckyDraw ();
			fishTurnTableLuckyDraw.result = getFishLuckyDrawResponse.Result;
			fishTurnTableLuckyDraw.type = getFishLuckyDrawResponse.Type;
           
			if (getFishLuckyDrawResponse.Prop != null) {
				IEnumerator<PB_PropertyEx> nEumPropty = (System.Collections.Generic.IEnumerator<PB_PropertyEx>)getFishLuckyDrawResponse.Prop.GetEnumerator ();
				while (nEumPropty.MoveNext ()) {
					FiPropertyEx DragonData = new FiPropertyEx ();
					DragonData.type = nEumPropty.Current.PropertyType;
					DragonData.value = nEumPropty.Current.Sum;
					fishTurnTableLuckyDraw.prpo2.Add (DragonData);
				}

			}
			return fishTurnTableLuckyDraw;
		}

		public static FishChangeLiuShuiTime toLocal_ChangeLiuShuiTime (LongChangeLiuShuiTime longChangeLiuShuiTime)
		{
			FishChangeLiuShuiTime fishChangeLiuShuiTime = new FishChangeLiuShuiTime ();
			fishChangeLiuShuiTime.nReuslt = longChangeLiuShuiTime.NReuslt;
			fishChangeLiuShuiTime.lDiamond = longChangeLiuShuiTime.LDiamond;
			fishChangeLiuShuiTime.lUserID = longChangeLiuShuiTime.LUserID;
			fishChangeLiuShuiTime.nTimeData = longChangeLiuShuiTime.NTimeData;
			return fishChangeLiuShuiTime;
		}

		public static InformOtherCancelSKill toLocal_OtherCancelSkill (CancelSkill cancelSkill)
		{
			InformOtherCancelSKill otherCancelSKill = new InformOtherCancelSKill ();
			otherCancelSKill.lUserID = cancelSkill.LUserID;
			otherCancelSKill.skillID = cancelSkill.SkillID;
			otherCancelSKill.nState = cancelSkill.NState;
			return otherCancelSKill;
		}

		public static FishingUserRank toLocal_FishingUserRank (UserRankInfo userRankInfo)
		{
			FishingUserRank fishingUserRank = new FishingUserRank ();
			fishingUserRank.lUserID = userRankInfo.LUserID;
			fishingUserRank.nRank = userRankInfo.NRank;
			return fishingUserRank;
		}

		public static FishingUserRank toLocal_FishingMyUserRank (UserRankInfo userRankInfo)
		{
			FishingUserRank fishingUser = new FishingUserRank ();
			fishingUser.lUserID = userRankInfo.LUserID;
			fishingUser.nRank = userRankInfo.NRank;
			return fishingUser;
		}

		public static MamonMaxWinCounts toLocal_Wincoutime (MamonMaxWinCount userRankInfo)
		{
			MamonMaxWinCounts fishingUser = new MamonMaxWinCounts ();
			fishingUser.useid = userRankInfo.LuserID;
			fishingUser.wintime = userRankInfo.MaxwinCount;
			return fishingUser;
		}

		public static FiChangeUserGold toLocal_ChangeUserGold (ChangeUserGold changeUserGold)
		{
			FiChangeUserGold fiChange = new FiChangeUserGold ();
			fiChange.userID = changeUserGold.LUserID;
//			if (changeUserGold.Property != null) {
//				IEnumerator<PB_Property> nEumPropty = (System.Collections.Generic.IEnumerator<PB_Property>)changeUserGold.Property.GetEnumerator ();
//				while (nEumPropty.MoveNext ()) {
//					FiProperty prop = new FiProperty ();
//					prop.type = nEumPropty.Current.PropertyType;
//					prop.value = nEumPropty.Current.Sum;
//					fiChange.property.Add (prop);
//				}
//
//			}
//			UnityEngine.Debug.LogError ("fiChange.gold = !!!!!!!!!!!!!!" + fiChange.gold);
//			UnityEngine.Debug.LogError ("fiChange.userID = !!!!!!!!!!!!!!!!!!!1" + fiChange.userID);
			fiChange.propertyID = changeUserGold.PropertyID;
			fiChange.count = changeUserGold.LCount;

			return fiChange;
		}

		public static FiBossRoomMatchInfo toLocal_BossRoomMatchInfo (NotifyBossRoomMatchInfo bossRoomMatch)
		{
			FiBossRoomMatchInfo roomMatch = new FiBossRoomMatchInfo ();
			roomMatch.type = bossRoomMatch.NType;
			roomMatch.content = bossRoomMatch.Content;
//			roomMatch.selfGold = bossRoomMatch.SelfGold;
//			if (bossRoomMatch.RoomArrayID != null) {
//				IEnumerator<int> nEumPropty = (IEnumerator<int>)bossRoomMatch.RoomArrayID.GetEnumerator ();
//				while (nEumPropty.MoveNext ()) {
//					roomMatch.roomArrayID.Add (nEumPropty.Current);
//				}
//			
//			}
//			UnityEngine.Debug.LogError ("roomMatch.type = !!!!!!!!!!!!!!" + roomMatch.type);
//			UnityEngine.Debug.LogError ("roomMatch.content = !!!!!!!!!!!!!!!!!!!1" + roomMatch.content);

			return roomMatch;
		}

		public static FiNotifySignUp toLocal_NotifySignUp (NotifySignUp _signUp)
		{
			FiNotifySignUp signUp = new FiNotifySignUp ();
			signUp.type = _signUp.NType;
//			signUp.roomIndex = _signUp.NRoomIndex;
//			signUp.signUpGold = _signUp.SignUpGold;
//			signUp.gameType = _signUp.NGameType;
			//			roomMatch.selfGold = bossRoomMatch.SelfGold;
			//			if (bossRoomMatch.RoomArrayID != null) {
			//				IEnumerator<int> nEumPropty = (IEnumerator<int>)bossRoomMatch.RoomArrayID.GetEnumerator ();
			//				while (nEumPropty.MoveNext ()) {
			//					roomMatch.roomArrayID.Add (nEumPropty.Current);
			//				}
			//			
			//			}
//			UnityEngine.Debug.LogError ("roomMatch.type = !!!!!!!!!!!!!!" + signUp.type);
//			UnityEngine.Debug.LogError ("roomMatch.content = !!!!!!!!!!!!!!!!!!!1" + roomMatch.content);

			return signUp;
		}

		public static IntoBossRoomMessage toLocal_IntoBossRoom (NotifyEnterBossRoomMessage intoMessage)
		{
			IntoBossRoomMessage _intoMessage = new IntoBossRoomMessage ();
			_intoMessage.nType = intoMessage.NType;
			_intoMessage.nRoomIndex = intoMessage.NRoomIndex;
			_intoMessage.modifyNick = intoMessage.ModifyNick;
			return _intoMessage;
		}

		public static FishingBossKillRank toLocal_BossKillRank (UserBossKillRankInfo bossRankInfo)
		{
			FishingBossKillRank _bossKillRank = new FishingBossKillRank ();
			_bossKillRank.lUserID = bossRankInfo.LUserID;
			_bossKillRank.nRank = bossRankInfo.NRank;
			return _bossKillRank;
		}

		public static FishingBossKillRank toLocal_BossKillMyRank (UserBossKillRankInfo bossMyRankInfo)
		{
			FishingBossKillRank _bossKillMyRank = new FishingBossKillRank ();
			_bossKillMyRank.lUserID = bossMyRankInfo.LUserID;
			_bossKillMyRank.nRank = bossMyRankInfo.NRank;
			return _bossKillMyRank;
		}

		public static FiUpdateBossMatchTime toLocal_UpdateBossMatchTime (UpdateBossMatchTime _matchTime)
		{
			FiUpdateBossMatchTime matchTime = new FiUpdateBossMatchTime ();
			matchTime.chaTime = _matchTime.ChaTime;
//			UnityEngine.Debug.LogError ("toLocal_UpdateBossMatchTime matchTime.chaTime = " + matchTime.chaTime);
			matchTime.startTime = _matchTime.StartTime;
			matchTime.endTime = _matchTime.EndTime;
			matchTime.roomIndex = _matchTime.RoomIndex;
			return matchTime;
		}

		/// <summary>
		/// 比赛rank解析
		/// </summary>
		/// <returns>The local get boss match result rank.</returns>
		/// <param name="_array">Array.</param>
		public static FiUserRankArray toLocal_GetBossMatchResultRank (UserRankInfoArray _array)
		{
			FiUserRankArray array = new FiUserRankArray ();
			array.content = _array.Content;
//			UnityEngine.Debug.LogError ("toLocal_GetBossMatchResultRank array.content  = " + array.content);
			array.rankArray = new List<FishingUserRank> ();
			if (_array.RankInfoArray != null) {
				IEnumerator<UserRankInfo> nEum = _array.RankInfoArray.GetEnumerator ();
				while (nEum.MoveNext ()) {
					FishingUserRank userRank = new FishingUserRank ();
					userRank.lUserID = nEum.Current.LUserID;
					userRank.nRank = nEum.Current.NRank;
					userRank.vip = nEum.Current.Vip;
					userRank.longCard = nEum.Current.LongCard;
					userRank.nGold = nEum.Current.NGold;
					userRank.nRewardGold = nEum.Current.RewardGold;
					userRank.nickname = nEum.Current.Nickname;
//					UnityEngine.Debug.LogError ("toLocal_GetBossMatchResultRank userRank.lUserID  = " + userRank.lUserID);
//					UnityEngine.Debug.LogError ("toLocal_GetBossMatchResultRank userRank.nRank  = " + userRank.nRank);
//					UnityEngine.Debug.LogError ("toLocal_GetBossMatchResultRank userRank.vip  = " + userRank.vip);
//					UnityEngine.Debug.LogError ("toLocal_GetBossMatchResultRank userRank.longCard  = " + userRank.longCard);
//					UnityEngine.Debug.LogError ("toLocal_GetBossMatchResultRank userRank.nGold  = " + userRank.nGold);
//					UnityEngine.Debug.LogError ("toLocal_GetBossMatchResultRank userRank.nRewardGold  = " + userRank.nRewardGold);
//					UnityEngine.Debug.LogError ("toLocal_GetBossMatchResultRank userRank.nickname  = " + userRank.nickname);
//					byte[] byteString = userRank.nickname.ToByteArray ();
//					string name = System.Text.Encoding.UTF8.GetString (byteString);
//					UnityEngine.Debug.LogError ("toLocal_GetBossMatchResultRank name = " + name);
					array.rankArray.Add (userRank);
				}
			}
			return array;
		}



		//		FiGetRankResponse
		//接受荣耀排位数据
		public static PaiWeiSaiRankInfos toLocal_HoroDataInfo (PaiWeiSaiRankInfo _horoinfo)
		{
			PaiWeiSaiRankInfos horodata = new PaiWeiSaiRankInfos ();
			horodata.result = _horoinfo.Result;
			horodata.bossmatchdouble = _horoinfo.Bossmatchdouble;
			horodata.duanwei = _horoinfo.Duanwei;
			horodata.isTopUp = _horoinfo.IsTopUp;
			horodata.monthCardtype = _horoinfo.MonthCardtype;
			horodata.nrank = _horoinfo.Nrank;
			horodata.shangqipaiming = _horoinfo.ShangPaiMing;
			horodata.lishizuigao = _horoinfo.Lishizuigao;
			horodata.qishu = _horoinfo.QiShu;
			horodata.shenyutime = _horoinfo.Shenyutime;
			horodata.beiqizuigao = _horoinfo.Beiqizuigao;
			UnityEngine.Debug.Log ("_horoinfo.Shenyutime::" + _horoinfo.Shenyutime);

			IEnumerator<PB_GameRank> EnmSavePool = _horoinfo.Rank.GetEnumerator ();
			while (EnmSavePool.MoveNext ()) {
				FiRankInfo hororank = new FiRankInfo ();
				hororank.avatarUrl = EnmSavePool.Current.AvatarUrl;
				hororank.gameId = EnmSavePool.Current.GameId;
				hororank.gender = EnmSavePool.Current.Gender;
				hororank.gold = EnmSavePool.Current.Gold;
				hororank.level = EnmSavePool.Current.Level;
				hororank.maxMultiple = EnmSavePool.Current.MaxMultiple;
				hororank.nickname = EnmSavePool.Current.Nickname;
				hororank.userId = EnmSavePool.Current.UserId;
				hororank.vipLevel = EnmSavePool.Current.Vip;
				horodata.rankList.Add (hororank);
			}
			return horodata;
		}

		public static RongYuDianTangRanInfo toLocal_RongYuRankInfo (RongYuDianTangkInfo _info)
		{
            
			RongYuDianTangRanInfo tangRanInfo = new RongYuDianTangRanInfo ();
			tangRanInfo.result = _info.Result;
			tangRanInfo.hotPrizePool = _info.Hotprizepool;
			IEnumerator<PB_GameRank> enumerator = _info.Rank.GetEnumerator ();
			while (enumerator.MoveNext ()) {
				FiRankInfo fiRankInfo = new FiRankInfo ();
				fiRankInfo.avatarUrl = enumerator.Current.AvatarUrl;
				fiRankInfo.nickname = enumerator.Current.Nickname;
				fiRankInfo.userId = enumerator.Current.UserId;
				fiRankInfo.shangLiuShui = enumerator.Current.ShangLiuShui;
				fiRankInfo.gold = enumerator.Current.Gold;
				fiRankInfo.xingxing = enumerator.Current.Xingxing;
				tangRanInfo.rongYuRankList.Add (fiRankInfo);
			}
			return tangRanInfo;
		}

		public static PaiWeiPrizeInfo to_Local_PaiWeiPrizeInfo (GetPaiWeiSaiReward _info)
		{
			PaiWeiPrizeInfo paiWeiPrizeInfo = new PaiWeiPrizeInfo ();
			paiWeiPrizeInfo.rewardIndex = _info.RewardIndex;
			paiWeiPrizeInfo.rewardState = _info.RewardState;
			IEnumerator<PB_Property> enumerator = _info.RewardData.GetEnumerator ();
			while (enumerator.MoveNext ()) {
				FiProperty fiProperty = new FiProperty ();
				fiProperty.type = enumerator.Current.PropertyType;
				fiProperty.value = enumerator.Current.Sum;
				paiWeiPrizeInfo.rewardData.Add (fiProperty);
			}
			return paiWeiPrizeInfo;
		}

		public static PaiWeiPrizeState to_Local_PawiWeiPrize (PlayerPaiWeiSaiRewardInfo _info)
		{
			PaiWeiPrizeState paiWeiPrizeState = new PaiWeiPrizeState ();
			IEnumerator<int> enumerator = _info.RewardList.GetEnumerator ();
			while (enumerator.MoveNext ()) {
				paiWeiPrizeState.rewardList.Add (enumerator.Current);
			}
			return paiWeiPrizeState;
		}

		public static PaiWeiPrizeInfo toLocal_RongYaoPrize (GetPaiWeiSaiReward _info)
		{
			//UnityEngine.Debug.LogError("接收消息");
			PaiWeiPrizeInfo honorPrizeInfo = new PaiWeiPrizeInfo ();
			honorPrizeInfo.rewardIndex = _info.RewardIndex;
			honorPrizeInfo.rewardState = _info.RewardState;
			//荣誉殿堂领取奖励的任务数量
			honorPrizeInfo.curCatchFishNum = _info.CurCatchFishNum;
			honorPrizeInfo.maxCatchFishNum = _info.MaxCatchFishNum;
			IEnumerator<PB_Property> enumerator = _info.RewardData.GetEnumerator ();
			while (enumerator.MoveNext ()) {
				FiProperty fiProperty = new FiProperty ();
				fiProperty.type = enumerator.Current.PropertyType;
				fiProperty.value = enumerator.Current.Sum;
				honorPrizeInfo.rewardData.Add (fiProperty);
			}
			return honorPrizeInfo;
		}

		public static GetTopUpGiftBagState toLocal_GetGiftBagState (GetTopUpGiftBagStateNew stateNew)
		{
			//UnityEngine.Debug.LogError("2222222222222222");
			GetTopUpGiftBagState getTopUpGiftBagState = new GetTopUpGiftBagState ();
			getTopUpGiftBagState.one_giftBagState = stateNew.OneGiftBagState;
			getTopUpGiftBagState.two_select_oneBagState = stateNew.TwoSelectOneBagState;
			getTopUpGiftBagState.three_select_oneBagState = stateNew.ThreeSelectOneBagState;

			//UnityEngine.Debug.LogError("one_giftBagState====" + stateNew.OneGiftBagState);
			//UnityEngine.Debug.LogError("two_select_oneBagState====" + stateNew.TwoSelectOneBagState);
			//UnityEngine.Debug.LogError("three_select_oneBagState====" + stateNew.ThreeSelectOneBagState);
			return getTopUpGiftBagState;
		}

		public static InitSevenDayInfos toLocal_SevendayGetGiftBagState (InitSevenDayInfo stateNew)
		{
			//UnityEngine.Debug.LogError("2222222222222222");
			InitSevenDayInfos getTopUpGiftBagState = new InitSevenDayInfos ();
			getTopUpGiftBagState.result = stateNew.Result;
			getTopUpGiftBagState.curDay = stateNew.CurDay;
			getTopUpGiftBagState.userDay = stateNew.UserDay;
			getTopUpGiftBagState.userDayState = stateNew.UserDayState;
			getTopUpGiftBagState.taskDay = stateNew.TaskDay;
			getTopUpGiftBagState.taskValue = stateNew.TaskValue;
			getTopUpGiftBagState.taskDayState = stateNew.TaskDayState;
			getTopUpGiftBagState.userGiftDay = stateNew.UserGiftDay;
			getTopUpGiftBagState.userGiftDyaState = stateNew.UserGiftDyaState;

			UnityEngine.Debug.LogError ("getTopUpGiftBagState.result" + stateNew.Result);
			UnityEngine.Debug.LogError ("stateNew.CurDay:" + stateNew.CurDay);
			UnityEngine.Debug.LogError ("stateNew.UserDay:" + stateNew.UserDay);
			UnityEngine.Debug.LogError ("stateNew.UserDayState:" + stateNew.UserDayState);
			UnityEngine.Debug.LogError ("stateNew.TaskDay:" + stateNew.TaskDay);
			UnityEngine.Debug.LogError ("stateNew.taskvalue:" + stateNew.TaskValue);
			UnityEngine.Debug.LogError ("stateNew.TaskDayState:" + stateNew.TaskDayState);
			UnityEngine.Debug.LogError ("stateNew.UserGiftDay:" + stateNew.UserGiftDay);
			UnityEngine.Debug.LogError ("stateNew.UserGiftDyaState:" + stateNew.UserGiftDyaState);
			return getTopUpGiftBagState;
		}

		public static DbGetUpLevelActivityInfos toLocal_UpLevelState(DBGetUpLevelActivityInfo stateNew)
        {
			DbGetUpLevelActivityInfos upLevelList = new DbGetUpLevelActivityInfos();
            IEnumerator<InitUpLevelInfo> enumerator2 = stateNew.Rewarduplevleinfo.GetEnumerator();
            while (enumerator2.MoveNext())
            {
				UpLevelTaskInfos upLevelInfo = new UpLevelTaskInfos();
				upLevelInfo.taskID = enumerator2.Current.TaskID;
				upLevelInfo.taskCurValue = enumerator2.Current.TaskCurValue;
				upLevelInfo.taskMaxValue = enumerator2.Current.TaskMaxValue;
				upLevelInfo.rewardState = enumerator2.Current.RewardState;
				upLevelInfo.showInfoMaxValue = enumerator2.Current.ShowInfoMaxValue;
				upLevelList.levelList.Add(upLevelInfo);

				//UnityEngine.Debug.LogError("upLevelInfo.taskID = " + upLevelInfo.taskID);
				//UnityEngine.Debug.LogError("upLevelInfo.taskCurValue = " + upLevelInfo.taskCurValue);
				//UnityEngine.Debug.LogError("upLevelInfo.taskMaxValue = " + upLevelInfo.taskMaxValue);
				//UnityEngine.Debug.LogError("upLevelInfo.rewardState = " + upLevelInfo.rewardState);
				//UnityEngine.Debug.LogError("upLevelInfo.showInfoMaxValue = " + upLevelInfo.showInfoMaxValue);
				//if ((upLevelInfo.taskID == 3) && (upLevelInfo.rewardState==3) )
    //            {
				//	MyInfo.isUserGetAllUpLevel = true;
				//}
			}
            return upLevelList;
        }

        public static UpLevelRewards toLocal_UpLevelSendReward(SendUpLevelReward stateNew)
		{
			UpLevelRewards sendUpLevelstate = new UpLevelRewards();
			sendUpLevelstate.taskID = stateNew.TaskID;
			
			UnityEngine.Debug.LogError("getUpLevelstate.taskID = " + stateNew.TaskID);
			return sendUpLevelstate;
		}

		public static UpLevelRewardGets toLocal_UpLevelGetReward(GetUpLevelReward stateNew)
		{
			UpLevelRewardGets getUpLevelstate = new UpLevelRewardGets();
			getUpLevelstate.result = stateNew.Result;
			getUpLevelstate.taskID = stateNew.TaskID;
			getUpLevelstate.taskLevel = stateNew.TaskLevel;
			getUpLevelstate.gold = stateNew.Gold;

			//UnityEngine.Debug.LogError("getUpLevelstate.result = " + stateNew.Result);
			//UnityEngine.Debug.LogError("getUpLevelstate.taskID = " + stateNew.TaskID);
			//UnityEngine.Debug.LogError("getUpLevelstate.taskLevel = " + stateNew.TaskLevel);
			//UnityEngine.Debug.LogError("getUpLevelstate.Gold = " + stateNew.Gold);
			return getUpLevelstate;
		}

		public static FiSevenDaysPage toLocal_SevendayRewardGetGiftBagState (GetSevenDayReward stateNew)
		{
			//UnityEngine.Debug.LogError("2222222222222222");
			FiSevenDaysPage getTopUpGiftBagState = new FiSevenDaysPage ();
			getTopUpGiftBagState.SendIndex = stateNew.SelectIndex;
			getTopUpGiftBagState.UserDay = stateNew.UserDay;
			IEnumerator<PB_Property> enumerator = stateNew.RewardPro.GetEnumerator ();
			while (enumerator.MoveNext ()) {
				FiProperty fiProperty = new FiProperty ();
				fiProperty.type = enumerator.Current.PropertyType;
				fiProperty.value = enumerator.Current.Sum;
				UnityEngine.Debug.LogError ("fiProperty.type" + enumerator.Current.PropertyType);
				UnityEngine.Debug.LogError ("fiProperty.value" + enumerator.Current.Sum);
				getTopUpGiftBagState.target.Add (fiProperty);
			}
		

			UnityEngine.Debug.LogError ("getTopUpGiftBagState.index" + stateNew.SelectIndex + "userday" + stateNew.UserDay);

			return getTopUpGiftBagState;
		}

        public static GetButtonState toLocal_ButtonState(GetHideButtonState stateNew)
        {
            GetButtonState getbuttonstate = new GetButtonState();
            getbuttonstate.Count = stateNew.Count;
            IEnumerator<int> enumerator = stateNew.NButtonStateArray.GetEnumerator();
            while (enumerator.MoveNext())
            {
                getbuttonstate.nButtonStateArray.Add(enumerator.Current);
            }
            Facade.GetFacade().data.Add(FacadeConfig.UI_AND_BUTTON_CLOSE_OR_OPEN, getbuttonstate);
            return getbuttonstate;
        }

		public static PropPayInfoArray toLocal_PayInfo (PB_PayInfo data)
		{
			UnityEngine.Debug.Log(" ~~~~~ ~~~~~ toLocal_PayInfo ");
			PropPayInfoArray getPayInfos = new PropPayInfoArray();
			IEnumerator<InitPayInfo> enumerator = data.Payinfo.GetEnumerator();
			while (enumerator.MoveNext())
			{
				PropPayInfo payDetial = new PropPayInfo();
				payDetial.changeNum = enumerator.Current.ChangeNum;
				payDetial.payType = enumerator.Current.PayType;
				payDetial.id = enumerator.Current.ID;
				payDetial.rmb = enumerator.Current.RMB;
				payDetial.addGold = enumerator.Current.AddGold;

				getPayInfos.payInfoArray.Add(payDetial);

                #if UNITY_EDITOR
				UnityEngine.Debug.Log(" ~~~~~ ~~~~~ PropPayInfo id = " + payDetial.id);
                UnityEngine.Debug.Log(" ~~~~~ ~~~~~ PropPayInfo changeNum = " + payDetial.changeNum);
                UnityEngine.Debug.Log(" ~~~~~ ~~~~~ PropPayInfo payType = " + payDetial.payType);
                UnityEngine.Debug.Log(" ~~~~~ ~~~~~ PropPayInfo rmb = " + payDetial.rmb);
                UnityEngine.Debug.Log(" ~~~~~ ~~~~~ PropPayInfo addGold = " + payDetial.addGold);
                UnityEngine.Debug.Log(" ~~~~~ ~~~~~ ~~~~~ ~~~~~ ~~~~~ ~~~~~ ~~~~~ ~~~~~ ~~~~~ ~~~~~");
                #endif
			}
			//UnityEngine.Debug.Log(" ~~~~~ PropPayInfoArray getPayInfos size = " + getPayInfos.payInfoArray.Count);
			return getPayInfos;
		}

		public static PhoneNumberLoginArray toLocal_PhoneNumberInfo(PB_PhoneLogin data)
		{
			UnityEngine.Debug.Log(" ~~~~~ ~~~~~ toLocal_PhoneNumberInfo ");
			PhoneNumberLoginArray getPhoneInfos = new PhoneNumberLoginArray();
			getPhoneInfos.result = data.Result;
			IEnumerator<InitPhoneAccountInfo> enumerator = data.AccountInfoArr.GetEnumerator();
			while (enumerator.MoveNext())
			{
				PhoneNumberLoginInfo accountInfo = new PhoneNumberLoginInfo();
				accountInfo.resutl = enumerator.Current.Result;
				accountInfo.accountType = enumerator.Current.AccountType;
				accountInfo.user_id = enumerator.Current.UserID;
				accountInfo.accountName = enumerator.Current.AccountName;
				accountInfo.strToken = enumerator.Current.StrToken;
				accountInfo.nickname = enumerator.Current.Nickname;

				getPhoneInfos.PhoneNumberInfo.Add(accountInfo);

                #if UNITY_EDITOR
				//UnityEngine.Debug.Log(" ~~~~~ ~~~~~ PhoneNumberLoginArray resutl = " + accountInfo.resutl);
				//UnityEngine.Debug.Log(" ~~~~~ ~~~~~ PhoneNumberLoginArray accountType = " + accountInfo.accountType);
				//UnityEngine.Debug.Log(" ~~~~~ ~~~~~ PhoneNumberLoginArray user_id = " + accountInfo.user_id);
				//UnityEngine.Debug.Log(" ~~~~~ ~~~~~ PhoneNumberLoginArray accountName = " + accountInfo.accountName);
				//UnityEngine.Debug.Log(" ~~~~~ ~~~~~ PhoneNumberLoginArray strToken = " + accountInfo.strToken);
				UnityEngine.Debug.Log(" ~~~~~ ~~~~~ ~~~~~ ~~~~~ ~~~~~ ~~~~~ ~~~~~ ~~~~~ ~~~~~ ~~~~~");
                #endif
			}
			//UnityEngine.Debug.Log(" ~~~~~ PropPayInfoArray getPayInfos size = " + getPayInfos.payInfoArray.Count);
			return getPhoneInfos;
		}

		public static PhoneNumberPass toLocal_PhoneNumberPass(PB_GetIphonePassResp data)
		{
			UnityEngine.Debug.Log(" ~~~~~ ~~~~~ toLocal_PhoneNumberPass ");
			PhoneNumberPass getPhonePass = new PhoneNumberPass();
			getPhonePass.result = data.Result;
			IEnumerator<InitPhoneAccountInfo> enumerator = data.AccountInfoArr.GetEnumerator();
			while (enumerator.MoveNext())
			{
				PhoneNumberLoginInfo accountInfo = new PhoneNumberLoginInfo();
				accountInfo.resutl = enumerator.Current.Result;
				accountInfo.accountType = enumerator.Current.AccountType;
				accountInfo.user_id = enumerator.Current.UserID;
				accountInfo.accountName = enumerator.Current.AccountName;
				accountInfo.strToken = enumerator.Current.StrToken;

				getPhonePass.PhoneNumberPassInfo.Add(accountInfo);

#if UNITY_EDITOR
				//UnityEngine.Debug.Log(" ~~~~~ ~~~~~ PhoneNumberPass resutl = " + accountInfo.resutl);
				//UnityEngine.Debug.Log(" ~~~~~ ~~~~~ PhoneNumberPass accountType = " + accountInfo.accountType);
				//UnityEngine.Debug.Log(" ~~~~~ ~~~~~ PhoneNumberPass user_id = " + accountInfo.user_id);
				//UnityEngine.Debug.Log(" ~~~~~ ~~~~~ PhoneNumberPass accountName = " + accountInfo.accountName);
				//UnityEngine.Debug.Log(" ~~~~~ ~~~~~ PhoneNumberPass strToken = " + accountInfo.strToken);
				UnityEngine.Debug.Log(" ~~~~~ ~~~~~ ~~~~~ ~~~~~ ~~~~~ ~~~~~ ~~~~~ ~~~~~ ~~~~~ ~~~~~");
#endif
			}
			//UnityEngine.Debug.Log(" ~~~~~ PropPayInfoArray getPayInfos size = " + getPayInfos.payInfoArray.Count);
			return getPhonePass;
		}


			public static LoginAccountAssociateChoise toLocal_LoginAccountAssociate(PB_AssociateAccountLogin data)
			{
				UnityEngine.Debug.Log(" ~~~~~ ~~~~~ toLocal_LoginAccountAssociate ");
				LoginAccountAssociateChoise getPhoneAss = new LoginAccountAssociateChoise();
				getPhoneAss.result = data.Result;
				getPhoneAss.accountType = data.AccountType;
				getPhoneAss.user_id = data.UserID;
				getPhoneAss.accountName = data.AccountName;
				getPhoneAss.strToken = data.StrToken;
				//getPhoneAss.nickname = data.Nickname;

#if UNITY_EDITOR
				UnityEngine.Debug.Log(" ~~~~~ ~~~~~ LoginAccountAssociateChoise resutl = " + getPhoneAss.result);
				UnityEngine.Debug.Log(" ~~~~~ ~~~~~ LoginAccountAssociateChoise accountType = " + getPhoneAss.accountType);
				UnityEngine.Debug.Log(" ~~~~~ ~~~~~ LoginAccountAssociateChoise user_id = " + getPhoneAss.user_id);
				UnityEngine.Debug.Log(" ~~~~~ ~~~~~ LoginAccountAssociateChoise accountName = " + getPhoneAss.accountName);
				UnityEngine.Debug.Log(" ~~~~~ ~~~~~ LoginAccountAssociateChoise strToken = " + getPhoneAss.strToken);
				UnityEngine.Debug.Log(" ~~~~~ ~~~~~ LoginAccountAssociateChoise nickname = " + getPhoneAss.nickname);
				UnityEngine.Debug.Log(" ~~~~~ ~~~~~ ~~~~~ ~~~~~ ~~~~~ ~~~~~ ~~~~~ ~~~~~ ~~~~~ ~~~~~");
#endif

				//UnityEngine.Debug.Log(" ~~~~~ PropPayInfoArray getPayInfos size = " + getPayInfos.payInfoArray.Count);
				return getPhoneAss;
			}
		

			//DLL
			public static LoginAccountAssociateChoise toLocal_LoginAccountAssociate(PB_PhoneLoginAccount data)
			{
				UnityEngine.Debug.Log(" ~~~~~ ~~~~~ toLocal_LoginAccountAssociate ");
				LoginAccountAssociateChoise getPhoneAss = new LoginAccountAssociateChoise();
				getPhoneAss.result = data.result;
				getPhoneAss.accountType = data.accountType;
				getPhoneAss.user_id = data.userId;
				getPhoneAss.accountName = data.accountName;
				getPhoneAss.strToken = data.strToken;
				getPhoneAss.nickname = data.nickname;

#if UNITY_EDITOR
				UnityEngine.Debug.Log(" ~~~~~ ~~~~~ LoginAccountAssociateChoise resutl = " + getPhoneAss.result);
				UnityEngine.Debug.Log(" ~~~~~ ~~~~~ LoginAccountAssociateChoise accountType = " + getPhoneAss.accountType);
				UnityEngine.Debug.Log(" ~~~~~ ~~~~~ LoginAccountAssociateChoise user_id = " + getPhoneAss.user_id);
				UnityEngine.Debug.Log(" ~~~~~ ~~~~~ LoginAccountAssociateChoise accountName = " + getPhoneAss.accountName);
				UnityEngine.Debug.Log(" ~~~~~ ~~~~~ LoginAccountAssociateChoise strToken = " + getPhoneAss.strToken);
				UnityEngine.Debug.Log(" ~~~~~ ~~~~~ LoginAccountAssociateChoise nickname = " + getPhoneAss.nickname);
				UnityEngine.Debug.Log(" ~~~~~ ~~~~~ ~~~~~ ~~~~~ ~~~~~ ~~~~~ ~~~~~ ~~~~~ ~~~~~ ~~~~~");
#endif

				//UnityEngine.Debug.Log(" ~~~~~ PropPayInfoArray getPayInfos size = " + getPayInfos.payInfoArray.Count);
				return getPhoneAss;
			}

		public static LoginAccountNickChoice toLocal_LoginAccountNickChoice(PBMsg_GetUserNicknameRequest data)
		{
			LoginAccountNickChoice getNicknames = new LoginAccountNickChoice();
			getNicknames.languageType = data.languageType;
			getNicknames.nickArray = data.nicknames;

#if UNITY_EDITOR
			UnityEngine.Debug.Log(" ~~~~~ ~~~~~ LoginAccountNickChoice languageType = " + getNicknames.languageType);
			UnityEngine.Debug.Log(" ~~~~~ ~~~~~ LoginAccountNickChoice nickArray = " + getNicknames.nickArray);
#endif

			return getNicknames;
		}
		public static LoginAccountNickChoice toLocal_LoginAccountNickChoice(PB_LoginAccountNickChoice data)
        {
			LoginAccountNickChoice getNicknames = new LoginAccountNickChoice();
			getNicknames.languageType = data.LanguageType;
			IEnumerator<string> enumerator = data.NickArray.GetEnumerator();
			while (enumerator.MoveNext())
			{
				getNicknames.nickArray.Add(enumerator.Current);
			}

#if UNITY_EDITOR
			UnityEngine.Debug.Log(" ~~~~~ ~~~~~ LoginAccountNickChoice languageType = " + getNicknames.languageType);
			UnityEngine.Debug.Log(" ~~~~~ ~~~~~ LoginAccountNickChoice nickArray = " + getNicknames.nickArray);
#endif

			return getNicknames;
		}
	}
}
	