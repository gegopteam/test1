using System;
using System.Collections.Generic;
using Google.Protobuf;
using UnityEngine;
using System.Text;

namespace AssemblyCSharp
{
	public class FishingCommonMsgHandle:IMsgHandle
	{
		
		public FishingCommonMsgHandle ()
		{

		}

		public void SendGetRobotRequest ()
		{
//			UnityEngine.Debug.LogError ("召唤机器人");
			DataControl.GetInstance ().PushSocketSndByte (FiEventType.SEND_GET_ROBOT_REQUEST, null);
		}

		public void SendReturnRobotRequest (int nUserId)
		{
			ReturnRobotRequest nRequest = new ReturnRobotRequest ();
			nRequest.UserId = nUserId;
			DataControl.GetInstance ().PushSocketSndByte (FiEventType.SEND_RETURN_ROBOT_REQUEST, nRequest.ToByteArray ());
		}

		/// <summary>
		/// 发送改变炮台样式请求
		/// </summary>
		/// <param name="nCannonStyle">N cannon style.</param>
		/// <param name="userId">User identifier.</param>
		public void SendChangeCannonStyleRequest (int nCannonStyle, int userId)
		{
			FiChangeCannonStyleRequest nRequest = new FiChangeCannonStyleRequest ();
			nRequest.cannonStyle = nCannonStyle;
			nRequest.userId = userId;

			//Debug.Log(FiEventType.SEND_CHANGE_CANNON_STYLE_REQUEST +" : "+ nCannonStyle + " : "+ userId);
			DataControl.GetInstance ().PushSocketSnd (FiEventType.SEND_CHANGE_CANNON_STYLE_REQUEST, nRequest);
		}

		/// <summary>
		/// 发送改变炮座请求
		/// </summary>
		/// <param name="equipmentType">Equipment type.</param>
		/// <param name="propID">Property I.</param>
		public void SendChangeBarbetteStyleRequest (int equipmentType, int propID, int barrbetteStyle = 0)
		{
			FiChangeBarbetteStyle nRequest = new FiChangeBarbetteStyle ();
			nRequest.EquipmentType = equipmentType;
			nRequest.propID = propID;
			nRequest.removeChangeBarbetteStyle = barrbetteStyle;
//			UnityEngine.Debug.LogError ("nRequest.EquipmentType = " + nRequest.EquipmentType);
//			UnityEngine.Debug.LogError ("nRequest.propID = " + nRequest.propID);

			DataControl.GetInstance ().PushSocketSnd (FiEventType.SEND_XL_EQUIPMENTBARBETTE_REQUEST, nRequest);
		}
		//机器人发送改变分身请求
		public  void SendRobotReplicationRequest (int robotFishID, List<FiProperty> property)
		{
			FiChangeRobotReplicationFishID nRequest = new FiChangeRobotReplicationFishID ();
			nRequest.robotFishID = robotFishID;
			nRequest.target = property;
			DataControl.GetInstance ().PushSocketSnd (FiEventType.SEND_XL_ROBOTREPLICATION_REQUEST, nRequest);
		}

		public void SendManmonsettingRequest ()
		{
			DataControl.GetInstance ().PushSocketSnd (FiEventType.SEND_XL_MANMONSETTING_REQUEST, null);
		}

		public void SendFireBullet (int bulletId, float x, float y, int groupId = -1, int fishId = -1)
		{//自己发送子弹
			//Debug.Log ("发送子弹ID:"+bulletId);
			MyInfo myInfo = (MyInfo)Facade.GetFacade ().data.Get (FacadeConfig.USERINFO_MODULE_ID);
			FiFireBulletRequest fireBullet = new FiFireBulletRequest ();
			fireBullet.userId = myInfo.userID;
			fireBullet.cannonMultiple = myInfo.cannonMultipleNow;
			fireBullet.bulletId = bulletId;
			fireBullet.position = new Cordinate ();
			fireBullet.position.x = x;
			fireBullet.position.y = y;
			fireBullet.groupId = groupId;
			fireBullet.fishId = fishId;
			DataControl.GetInstance ().PushSocketSnd (FiEventType.SEND_FIRE_BULLET_REQUEST, fireBullet);
		}

		public void SendHitFish (int userId, int groupId, int fishId, int bulletId, float x, float y, long gunWaterGold)
		{//发送子弹打到鱼请求
			MyInfo myInfo = (MyInfo)Facade.GetFacade ().data.Get (FacadeConfig.USERINFO_MODULE_ID);
			if (userId != myInfo.userID)
				return;
			FiHitFishRequest hitFish = new FiHitFishRequest ();
			hitFish.groupId = groupId;
			hitFish.fishId = fishId;
			hitFish.userId = userId;
			hitFish.bulletId = bulletId;
			hitFish.cannonMultiple = myInfo.cannonMultipleNow;
			hitFish.position = new Cordinate ();
			hitFish.position.x = x;
			hitFish.position.y = y;
//            hitFish.longLiuShuiGold = gunWaterGold;
//            UnityEngine.Debug.LogError ("hitFish.amount_water_gold" + hitFish.longLiuShuiGold);
			DataControl.GetInstance ().PushSocketSnd (FiEventType.SEND_HITTED_FISH_REQUEST, hitFish);
			//Tool.OutLogWithToFile ("发送子弹打到鱼请求 userId:"+myInfo.userID+" bulletId:"+bulletId);
		}

		public void SendChangeCannonMultiple (int cannonMultiple)
		{//发送改变炮倍数
			Debug.Log(" 修改砲倍數 傳送給後台 "+ cannonMultiple);
			FiChangeCannonMultipleRequest changeCannon = new FiChangeCannonMultipleRequest ();
			changeCannon.cannonMultiple = cannonMultiple;
			DataControl.GetInstance ().PushSocketSnd (FiEventType.SEND_CHANGE_CANNON_REQUEST, changeCannon);
		}

		public void SendEffectRequest (int effectId)
		{//发送特效请求
			MyInfo myInfo = (MyInfo)Facade.GetFacade ().data.Get (FacadeConfig.USERINFO_MODULE_ID);
			FiEffectRequest effectInfo = new FiEffectRequest ();
			effectInfo.userId = myInfo.userID;
			effectInfo.effect = new FiEffectInfo ();
			effectInfo.effect.type = effectId;
			DataControl.GetInstance ().PushSocketSnd (FiEventType.SEND_USE_EFFECT_REQUEST, effectInfo);
			Tool.OutLogWithToFile("發送特效請求 effectId:" + effectId);
		}

		public void SendFishOut (int groupId, int fishId = 0)
		{//发送鱼群游出屏幕
			List<EnemyBase> listFish = UIFishingObjects.GetInstance ().GetGroupFish (groupId);
			if (null != listFish) {
				if (listFish.Count > 1) {//如果组里还有除这条鱼外的其它鱼，不发送游出屏幕外的消息
					return;
				}
			}
			FiFishsOutRequest fishOut = new FiFishsOutRequest ();
			fishOut.groupId = groupId;
			DataControl.GetInstance ().PushSocketSnd (FiEventType.SEND_FISH_OUT_REQUEST, fishOut);
		}


		public void SendTorpedoExplodeRequest (int torpedoID, int torpedoType, List<FiFish> fishs)
		{//发送鱼雷爆炸请求
			FiTorpedoExplodeRequest torpedoExplode = new FiTorpedoExplodeRequest ();
			torpedoExplode.torpedoId = torpedoID;
			torpedoExplode.torpedoType = torpedoType;
			torpedoExplode.fishes = new List<FiFish> ();
			foreach (FiFish fish in fishs) {
				if (null != fish) {
					torpedoExplode.fishes.Add (fish);
				}
			}
			DataControl.GetInstance ().PushSocketSnd (FiEventType.SEND_TORPEDO_EXPLODE_REQUEST, torpedoExplode);
		}

		public void SendManmonBettingType (int type)
		{
			FiFishGoldShow changeCannon = new FiFishGoldShow ();
			changeCannon.chipIndex = type;
			DataControl.GetInstance ().PushSocketSnd (FiEventType.SEND_XL_MANMONETTING_REQUEST, changeCannon);
		}

		public void SendManmonYaoQianShuType (int type)
		{
			FiFishYGoldShow changeCannon = new FiFishYGoldShow ();
			changeCannon.chipIndex = type;
			DataControl.GetInstance ().PushSocketSnd (FiEventType.SEND_XL_MANMONYAOQIANSHU_REQUEST, changeCannon);
		}

  
		//发送消息
		public void SendLongPoolRewardRequest (int type)
		{
			FiFishGetCoinPool sendtype = new FiFishGetCoinPool ();
			sendtype.type = type;
			DataControl.GetInstance ().PushSocketSnd (FiEventType.SEND_XL_TURNTABLEGETPOOL_REQUST, sendtype);
//			UnityEngine.Debug.LogError ("sendtype.type = " + sendtype.type);
		}

		public void SendManMonRankReward ()
		{
			DataControl.GetInstance ().PushSocketSnd (FiEventType.SEND_XL_MANMONRANKREWARD_REQUST, null);
		}
		//转盘抽奖
		public void SendShenLongLuckDrawRequest (int type = 0)
		{
			FishTurnTableLuckyDraw send = new FishTurnTableLuckyDraw ();
			send.type = type;
			DataControl.GetInstance ().PushSocketSnd (FiEventType.SEND_XL_TURNTABLELUCKDRAW_REQUST, null);
//			UnityEngine.Debug.LogError ("send.type+" + send.type);
		}
		//重置时间
		public void SendChangeLiuShuiTimeRequest ()
		{
			DataControl.GetInstance ().PushSocketSnd (FiEventType.SEND_XL_CHANGELIUSHUITIME_REQUST, null);
//			UnityEngine.Debug.LogError ("send.nulllllllllllllllllllllllllllllll");
		}
		//发送取消技能
		public void SendOtherCancelSkill (int skillType, int userId)
		{
			InformOtherCancelSKill send = new InformOtherCancelSKill ();
			//send.lUserID = userId;
			send.skillID = skillType;
			send.lUserID = userId;
			DataControl.GetInstance ().PushSocketSnd (FiEventType.SEND_XL_CANCELOTHERSKILL_REQUST, send);
//			UnityEngine.Debug.LogError ("send.skillType" + skillType);
		}

		//发送获取排名
		public void SendGetFishingRankInfo ()
		{
			DataControl.GetInstance ().PushSocketSnd (FiEventType.SEND_XL_UPDATERANKINFO_REQUST, null);
//			UnityEngine.Debug.LogError ("send.Rank");
		}

		//发送退出财神游戏协议
		public void SendManmonExitGame ()
		{
			DataControl.GetInstance ().PushSocketSnd (FiEventType.SEND_XL_WINTIMECOUNTFO_RESQUSET, null);
//			UnityEngine.Debug.LogError ("send.Rank");
		}

		//发送请求最高连胜的协议
		public void SendManmonWinriemcout ()
		{
			DataControl.GetInstance ().PushSocketSnd (FiEventType.SEND_XL_WINTIMECOUN_RESQUSET, null);
//			UnityEngine.Debug.LogError ("send.Rank");
		}
		//发送获取boss猎杀排名
		public void SendGetFishingBossKill ()
		{
			DataControl.GetInstance ().PushSocketSnd (FiEventType.SEND_XL_BOSSKILLRANK_RESQUSET, null);
//            Debug.LogError("sendBoss猎杀");
		}

		public void OnInit ()
		{
			EventControl nControl = EventControl.instance ();

			nControl.addEventHandler (FiEventType.RECV_CHANGE_CANNON_STYLE_RESPONSE, RecvChangeCannonStyleResponse);
			nControl.addEventHandler (FiEventType.RECV_OTHER_CHANGE_CANNON_STYLE_INFORM, RecvOtherChangeCannonStyleResponse);

			nControl.addEventHandler (FiEventType.RECV_OTHER_UNLOCK_CANNON_MUTIPLE_INFORM, RecvOtherUnlockCannonInform);

			nControl.addEventHandler (FiEventType.RECV_GET_ROBOT_RESPONSE, OnRecvRobotResponse);
			nControl.addEventHandler (FiEventType.RECV_XL_EQUIPMENTBARBETTE_RESPONSE, RecvChangeBarbetteStyleResponse);
			nControl.addEventHandler (FiEventType.RECV_XL_MANMONSTATRT_RESPONSE, RcvManmonStartShowResponse);
			nControl.addEventHandler (FiEventType.RECV_XL_MANMONSETTING_RESPONSE, RcvManmonSettingshowResponse);
			nControl.addEventHandler (FiEventType.RECV_XL_MANMONBETTING_RESPONSE, RcvManmonBettingshowResponse);
			nControl.addEventHandler (FiEventType.RECV_XL_MANMONYAOQIANSHU_RESPONSE, RcvManmonYaoQainShushowResponse);
			nControl.addEventHandler (FiEventType.RECV_XL_TURNTABLEGETPOOL_RESPONSE, RecvRotatingTurnTableRequst);
			nControl.addEventHandler (FiEventType.RECV_XL_MANMONRANKREWARD_REQUST, RecvManmonRankReward);
			nControl.addEventHandler (FiEventType.RECV_XL_TURNTABLELUCKDRAW_RESPOSE, RcvTurnTableLuckDrawResponse);
			nControl.addEventHandler (FiEventType.RECV_XL_TURNTABLELIUSHUI_RESPONSE, RcvTurntableLiuShuiResponse);
			nControl.addEventHandler (FiEventType.RECV_XL_CHANGELIUSHUITIME_RESPOSE, RcvTurnTableChangeLiuShuiTime);
			nControl.addEventHandler (FiEventType.RECV_XL_CANCELOTHERSKILL_REQUST, RecvOtherCancelSkill);
			nControl.addEventHandler (FiEventType.RECV_XL_UPDATERANKINFO_RESPOSE, RectFishingUserRank);
			nControl.addEventHandler (FiEventType.RECV_XL_UPDATEMYRANKINFO_RESPOSE, RecvMyFishingUserRank);
			nControl.addEventHandler (FiEventType.RECV_XL_WINTIMECOUNTFO_RESPOSE, RecvMyManmonWincout);
			nControl.addEventHandler (FiEventType.RECV_XL_BOSSROOMMATCH_RESPOSE, RecvBossMatchInfo);
			nControl.addEventHandler (FiEventType.RECV_XL_NOTIFYSIGNUP_RESPOSE, RecvNotifySignUp);
			nControl.addEventHandler (FiEventType.RECV_XL_INTOBOSSROMM_RESPOSE, RecvIntoBossRoom);
			nControl.addEventHandler (FiEventType.RECV_XL_BOSSKILLRANK_RESPOSE, RecvBossKillRank);
			nControl.addEventHandler (FiEventType.RECV_XL_BOSSKILLMYRANK_RESPOSE, RecvBossKillMyRank);
			/*nControl.addEventHandler ( FiEventType.RECV_FISHS_CREATED_INFORM , RecvFishCreateInform );
			nControl.addEventHandler ( FiEventType.RECV_OTHER_FIRE_BULLET_INFORM , RecvOtherFireBullet );
			nControl.addEventHandler ( FiEventType.RECV_HITTED_FISH_RESPONSE , RecvHitFishResponse );

			nControl.addEventHandler ( FiEventType.RECV_FISH_OUT_RESPONSE ,      RecvFishOutResponse );
			nControl.addEventHandler ( FiEventType.RECV_CHANGE_CANNON_RESPONSE , RecvChangeCannonResponse );
			nControl.addEventHandler ( FiEventType.RECV_OTHER_CHANGE_CANNON_INFORM , RecvOtherChangeCannonMultiple );

			nControl.addEventHandler ( FiEventType.RECV_USE_EFFECT_RESPONSE , RecvUserEffectResponse );
			nControl.addEventHandler ( FiEventType.RECV_OTHER_EFFECT_INFORM , RecvOtherEffectResponse );
			nControl.addEventHandler ( FiEventType.RECV_FISH_FREEZE_TIMEOUT_INFORM , RecvFishFreezeTimeOutResponse );
	
			nControl.addEventHandler ( FiEventType.RECV_FISH_TIDE_COMING_INFORM ,      RcvTideComingInform );
			nControl.addEventHandler ( FiEventType.RECV_FISH_TIDE_CLEAN_INFORM ,       RcvTideCleanInform );*/
		}


		void OnRecvRobotResponse (object data) //先触发OnUserJoin再触发这里，触发OnUserJoin的时候视为普通路人跑台，再触发这里，开启炮台的AI模式
		{
			GetRobotResponse nData = (GetRobotResponse)data;
//			UnityEngine.Debug.LogError ("OnRecvRobotResponse nData.Result = " + nData.Result);
			if (nData.Result == 0) {
				int robotId = nData.User.UserId;
				GunControl robotGun = PrefabManager._instance.GetGunByUserID (robotId);
				if (robotGun != null)
					robotGun.ActiveRobotMode ();
			} else {
				Tool.LogError ("Error! RecvRobotResponse result=" + nData.Result);
			}

		}

		private void RecvOtherUnlockCannonInform (object data)
		{
			FiOtherUnlockCannonMutipleInform nInform = (FiOtherUnlockCannonMutipleInform)data;
//			UnityEngine.Debug.LogError ("nInform.userId = " + nInform.userId);
			GunControl otherGun = PrefabManager._instance.GetGunByUserID (nInform.userId);
			if (otherGun != null) {
				otherGun.maxCannonMultiple = nInform.maxCannonMultiple;
				otherGun.gunUI.AddValue (0, nInform.rewardGold, -nInform.needDiamond);
			}
		}

		/// <summary>
		/// 接受自己切换炮台样式
		/// </summary>
		/// <param name="data">Data.</param>
		private void RecvChangeCannonStyleResponse (object data)
		{
			FiChangeCannonStyleResponse nResponse = (FiChangeCannonStyleResponse)data;
			if (nResponse.result == 0) {
				MyInfo nInfo = (MyInfo)Facade.GetFacade ().data.Get (FacadeConfig.USERINFO_MODULE_ID);
				nInfo.cannonStyle = nResponse.currentCannonStyle;
				Debug.Log("  取得目前裝備的砲台 "+ nInfo.cannonStyle);
				IUiMediator nMediator = Facade.GetFacade ().ui.Get (FacadeConfig.UI_BACKPACK_MODULE_ID);
				if (nMediator != null) {
					nMediator.OnRecvData (UIBackPack.UPDATE_STATE, nResponse);
				}
				if (PrefabManager._instance != null && PrefabManager._instance.gameObject != null) {
					//Tool.LogError ("接受更换炮台样式:" + nResponse.currentCannonStyle);
					PrefabManager._instance.GetLocalGun ().SetGunSpine (nResponse.currentCannonStyle - 3000);
				}
			} else {
				Debug.Log("切换炮台失败，result = " + nResponse.result);
			}
		}

		/// <summary>
		/// 接受其他玩家切换炮台
		/// </summary>
		/// <param name="data">Data.</param>
		private void RecvOtherChangeCannonStyleResponse (object data)
		{
			FiOtherChangeCannonStyleInform nInform = (FiOtherChangeCannonStyleInform)data;
			//Tool.LogError ("收到他人更换炮台");
			if (PrefabManager._instance != null && PrefabManager._instance.gameObject != null) {
				
				PrefabManager._instance.GetGunByUserID (nInform.userId).SetGunSpine (nInform.currentCannonStyle - 3000);
			}
		}

		/// <summary>
		/// 接受自己切换炮座样式
		/// </summary>
		/// <param name="data">Data.</param>
		private void RecvChangeBarbetteStyleResponse (object data)
		{
			FiChangeBarbetteStyle nResponse = (FiChangeBarbetteStyle)data;
//			UnityEngine.Debug.LogError ("RecvChangeBarbetteStyleResponse nResponse.result = " + nResponse.result);
			if (nResponse.result == 0) {
				MyInfo nInfo = (MyInfo)Facade.GetFacade ().data.Get (FacadeConfig.USERINFO_MODULE_ID);
				nInfo.cannonBabetteStyle = nResponse.EquipmentType % 10;
//				UnityEngine.Debug.LogError ("nResponse.removeChangeBarbetteStyle  = " + nResponse.removeChangeBarbetteStyle);
				if (nResponse.propID == 0 && nResponse.removeChangeBarbetteStyle != 0) {
					//卸下
					UIBackPack_Brief.isDischarge = true;
					nInfo.cannonBabetteStyle = 0;
				} else {
					//装备
					UIBackPack_Brief.isDischarge = false;
				}
//				UnityEngine.Debug.LogError ("nResponse.EquipmentType = " + nResponse.EquipmentType);
//				UnityEngine.Debug.LogError ("nInfo.cannonBabetteStyle = " + nInfo.cannonBabetteStyle);
				IUiMediator nMediator = Facade.GetFacade ().ui.Get (FacadeConfig.UI_BACKPACK_MODULE_ID);
				if (nMediator != null) {
					nMediator.OnRecvData (UIBackPack.UPDATE_STATE, nResponse);
				}
				if (PrefabManager._instance != null && PrefabManager._instance.gameObject != null) {
//					UnityEngine.Debug.LogError ("nResponse.EquipmentType = " + nResponse.EquipmentType);
					PrefabManager._instance.GetLocalGun ().SetBarbette (nResponse.EquipmentType);
				}
			} else {
				UnityEngine.Debug.LogError("切換砲台失敗，result=" + nResponse.result);
			}
		}

		/// <summary>
		/// 接受其他玩家切换炮座
		/// </summary>
		/// <param name="data">Data.</param>
		private void RecvOtherChangeBarbetteStyleResponse (object data)
		{
//			FiOtherChangeCannonStyleInform nInform = (FiOtherChangeCannonStyleInform)data;
//			//Tool.LogError ("收到他人更换炮座");
//			if (PrefabManager._instance != null && PrefabManager._instance.gameObject != null) {
			//				PrefabManager._instance.GetGunByUserID (nInform.userId).SetGunBarbette (nInform.currentCannonStyle - 3000);
//			}
		}

		public void RecvFishFreezeTimeOutResponse (object data)
		{//接收冰冻时间超时消息
			//Debug.LogError("接收冰冻超时信息");
			FiFreezeTimeOutInform freezeTimeOut = (FiFreezeTimeOutInform)data;
			UIFishingObjects.GetInstance ().FreezeTimeOut (freezeTimeOut);
		}

		public void RecvOtherEffectResponse (object data)
		{//接收其他玩家特效
			FiOtherEffectInform otherEffectInfo = (FiOtherEffectInform)data;
			FiEffectInfo info = otherEffectInfo.info;
			if (null == info)
				return;
			UIFishingObjects.GetInstance ().ToEffect (info, otherEffectInfo.userId);
		}

		private void RecvUserEffectResponse (object data)
		{//接收自己特效
			FiEffectResponse effectInfo = (FiEffectResponse)data;
			FiEffectInfo info = effectInfo.info; 
			if (null == info)
				return;
			if (0 == effectInfo.result) {
				BackpackInfo backpackInfo = (BackpackInfo)Facade.GetFacade ().data.Get (FacadeConfig.BACKPACK_MODULE_ID);
				backpackInfo.Delete (info.type, 1);
				UIFishingObjects.GetInstance ().ToEffect (info);
			} else {
				if (effectInfo.result == 40004) {
					HintTextPanel._instance.SetTextShow("魚箱中的魚已經滿了，請稍後使用召喚", 2f);
				} else {
					//HintText._instance.ShowHint ("道具使用失败，道具编号:" + info.type + " 错误结果:" + effectInfo.result);
				}
				
			}
		}

		public void RecvOtherChangeCannonMultiple (object data)
		{//接收其他玩家改变炮倍数
			FiOtherChangeCannonMultipleInform otherChangeCannon = (FiOtherChangeCannonMultipleInform)data;
			UIFishingObjects.GetInstance ().ChangeCannonMultiple (otherChangeCannon.userId, otherChangeCannon.cannonMultiple);
		}

		private void RecvChangeCannonResponse (object data)
		{//接收改变炮倍数
			FiChangeCannonMultipleResponse changeCannon = (FiChangeCannonMultipleResponse)data;
			if (0 == changeCannon.result) {//改变炮倍数成功
				int multiple = changeCannon.cannonMultiple;
				MyInfo myInfo = (MyInfo)Facade.GetFacade ().data.Get (FacadeConfig.USERINFO_MODULE_ID);
				myInfo.cannonMultipleNow = multiple;
				UIFishingObjects.GetInstance ().ChangeCannonMultiple (myInfo.userID, multiple);
			} 
		}

		public void RecvFishCreateInform (object data)
		{
			FiFishsCreatedInform info = (FiFishsCreatedInform)data;
			UIFishingObjects.GetInstance ().CreateFish (info);
		}

		public void RecvFishOutResponse (object data)
		{//接收鱼群游出屏幕
			FiFishsOutResponse fishOut = (FiFishsOutResponse)data;
			UIFishingObjects.GetInstance ().ToFishOut (fishOut.groupId);
		}

		public void RecvOtherFireBullet (object data)
		{//接收别人发送的子弹信息
			FiFireBulletRequest info = (FiFireBulletRequest)data;
			//如果子弹的userId是自己，不进行处理
			MyInfo myInfo = (MyInfo)Facade.GetFacade ().data.Get (FacadeConfig.USERINFO_MODULE_ID);
			if (info.userId == myInfo.userID)
				return;
			UIFishingObjects.GetInstance ().ToFireBullet (info);
		}

		public void RecvHitFishResponse (object data)
		{//子弹打到鱼 存储掉落的道具等
			FiHitFishResponse info = (FiHitFishResponse)data;
			if (info.propertyArray.Count > 0) {
				MyInfo myInfo = (MyInfo)Facade.GetFacade ().data.Get (FacadeConfig.USERINFO_MODULE_ID);
				BackpackInfo backpackInfo = (BackpackInfo)Facade.GetFacade ().data.Get (FacadeConfig.BACKPACK_MODULE_ID);
				foreach (FiProperty one in info.propertyArray) {
					//GOLD = 1, DIAMOND = 2, EXP(经验) = 3, FREEZE = 4, AIM(锁定) = 5, CALL(召唤) = 6,
					switch (one.type) {
					case FiPropertyType.GOLD:
						myInfo.gold += one.value;
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
							FiBackpackProperty property = new FiBackpackProperty ();
							backpackInfo.Add (one.type, one.value);
//							UnityEngine.Debug.Log (one.type + " = 获得道具的类型");
							break;
						}
					default:
						int mGiveUnitId = one.type;
						if (mGiveUnitId >= FiPropertyType.TORPEDO_MINI && mGiveUnitId <= FiPropertyType.TORPEDO_NUCLEAR) {
							UIFishingObjects.GetInstance ().ToHitFish (info);
						}
						break;
					}
				}
			}
		}


		private void RcvManmonStartShowResponse (object data)
		{
			FiFishRoomShow nResponse = (FiFishRoomShow)data;
//			MyInfo myinfo = DataControl.GetInstance ().GetMyInfo ();
//			myinfo.nManmon = nResponse.Startnum;
			DataControl.GetInstance ().GetMyInfo ().nManmon = nResponse.Startnum;
//			UnityEngine.Debug.LogError ("RcvManmonStartShowResponse" + DataControl.GetInstance ().GetMyInfo ().nManmon);
			UIManmmonShow.instance.Refreshstatrnum (nResponse.Startnum);
			if (nResponse.Startnum == 1) {
				Facade.GetFacade ().message.fishCommom.SendManmonsettingRequest ();
			}
		}

		private void RcvManmonSettingshowResponse (object data)
		{
			GetManmonChipGolds nResponese = (GetManmonChipGolds)data;
			for (int i = 0; i < nResponese.shipGoldDataArray.Count; i++) {
//				UnityEngine.Debug.LogError ("mansetting.nManmonStartNum" + nResponese.shipGoldDataArray [i]);
			}
//			UnityEngine.Debug.LogError ("sssssssss" + nResponese.result + nResponese.userid + "currendcould" + nResponese.curGold + "currednstartnum" + nResponese.nManmonCount + "shhipcount" + nResponese.shipGoldDataArray.Count);
			for (int i = 0; i < nResponese.shipGoldDataArray.Count; i++) {
//				UnityEngine.Debug.LogError ("ww" + nResponese.shipGoldDataArray [i]);
			}
			if (nResponese.result == 0) {
                if (UIManmmonShow.instance.IsFirstShow && nResponese.nManmonCount == 1)
                {
                    return;
                }
				string path = "Window/WindowManmonSetting";
				UnityEngine.GameObject WindowClone = AppControl.OpenWindow (path);
				WindowClone.gameObject.SetActive (true);
				UIManmonBetting mansetting = WindowClone.gameObject.GetComponent<UIManmonBetting> ();
				mansetting.gold = nResponese.curGold;
				mansetting.BettingGoldShowArray = nResponese.shipGoldDataArray;
				mansetting.nManmonStartNum = nResponese.nManmonCount;
//				UnityEngine.Debug.LogError ("mansetting.nManmonStartNum" + mansetting.nManmonStartNum + "mansetting.gold" + mansetting.gold);
			} else if (nResponese.result == -1) {
				UnityEngine.GameObject Window = UnityEngine.Resources.Load ("Window/WindowTips")as UnityEngine.GameObject;
				UnityEngine.GameObject WindowClone = UnityEngine.GameObject.Instantiate (Window);
				UITipClickHideManager ClickTips = WindowClone.GetComponent<UITipClickHideManager> ();
				ClickTips.text.text = "當前財神元寶數量不足!";
			} else {

				string path = "Window/WindowManmonSetting";
				UnityEngine.GameObject WindowClone = AppControl.OpenWindow (path);
				WindowClone.gameObject.SetActive (true);
				UIManmonBetting mansetting = WindowClone.gameObject.GetComponent<UIManmonBetting> ();
				mansetting.gold = nResponese.curGold;
			}

		}

		private void RcvManmonBettingshowResponse (object data)
		{
			FiFishGoldShow nResponese = (FiFishGoldShow)data;
//			for (int i = 0; i < nResponese.shipGoldDataArray.Count; i++) {
//				UnityEngine.Debug.LogError ("mansetting.nManmonStartNum" + nResponese.shipGoldDataArray [i]);
//			}
//			UnityEngine.Debug.LogError ("RcvManmonBettingshowResponse" + nResponese.result + nResponese.nWinGold + nResponese.chipIndex + "nresponecounty" + nResponese.nManmoncount + "currold" + nResponese.selfGold + "showtime" + nResponese.showTime);
			if (nResponese.result == 0) {
//				string path = "Window/ManmonEffect";
//				UnityEngine.GameObject WindowClone = AppControl.OpenWindow (path);
//				WindowClone.gameObject.SetActive (true);
				UIManmmonShow.instance.ManmonEffectObj.SetActive (true);
				AudioManager._instance.PlayEffectClip (AudioManager.effect_manmon);
				ManmonEffect effect = UIManmmonShow.instance.ManmonEffectObj.GetComponent<ManmonEffect> ();
//				ManmonEffect effect = WindowClone.gameObject.GetComponent<ManmonEffect> ();
				effect.ShowNum = nResponese.nWinGold;
				effect.PlayAnim ("godofwealth2");
//				UnityEngine.Debug.LogError ("UIManmmonShow.instance.manmonlist [0].Startnum" + UIManmmonShow.instance.nManmonNum);
				UIManmmonShow.instance.nManmonNum -= 1;
				UIManmmonShow.instance.Refreshstatrnum (UIManmmonShow.instance.nManmonNum);
				DataControl.GetInstance ().GetMyInfo ().nManmon -= 1;

				DragonCardInfo nDragoninfo = (DragonCardInfo)Facade.GetFacade ().data.Get (FacadeConfig.UI_DRAGONCARD);
				nDragoninfo.CurrendGold = nResponese.selfGold;
				nDragoninfo.CheckGold = nResponese.nWinGold;
				nDragoninfo.Curcheckgold = nResponese.nWinGold;
//				UIManmonGameShow.instance.currendgold = nResponese.selfGold;
				PrefabManager._instance.GetLocalGun ().gunUI.RefrshCoin (0, nResponese.selfGold);
			} else {
				
			}

		}

		private void RcvManmonYaoQainShushowResponse (object data)
		{
			FiFishYGoldShow nResponese = (FiFishYGoldShow)data;
			//			for (int i = 0; i < nResponese.shipGoldDataArray.Count; i++) {
			//				UnityEngine.Debug.LogError ("mansetting.nManmonStartNum" + nResponese.shipGoldDataArray [i]);
			//			}
//			UnityEngine.Debug.LogError ("RcvManmonBettingshowResponse" + nResponese.result + nResponese.nWinGold + "nsssss" + nResponese.chipIndex + "showtime" + nResponese.showTime + "selfgold" + nResponese.selfGold);
			if (nResponese.result == 0) {
				UIManmonGameShow.instance.RefreshMoney (nResponese.nWinGold);
				UIManmonGameShow.instance.ShowType (nResponese.chipIndex, nResponese.nWinGold, nResponese.showTime, nResponese.mbeishu);
				UIManmonGameShow.instance.RototionMove ();
				DragonCardInfo nDragoninfo = (DragonCardInfo)Facade.GetFacade ().data.Get (FacadeConfig.UI_DRAGONCARD);
				if (nResponese.nWinGold == 0) {
					UIManmonGameShow.instance.DealtyShow ();
//					UnityEngine.Debug.LogError ("11111111111111111+" + nDragoninfo.Curcheckgold + "=" + nDragoninfo.CheckGold);
					nDragoninfo.Curcheckgold = nDragoninfo.CheckGold;

				} else {
					UIManmonGameShow.instance.ShowEffect (ShwoEffect.JINBIDIAOLUO);

					nDragoninfo.CheckGold = nResponese.nWinGold;
					nDragoninfo.Curcheckgold = nResponese.nWinGold;
//					UnityEngine.Debug.LogError ("222222222222222222+" + nDragoninfo.Curcheckgold + "=" + nDragoninfo.CheckGold);
				}

//				UIManmonGameShow.instance.currendgold = nResponese.selfGold;
				nDragoninfo.CurrendGold = nResponese.selfGold;
				PrefabManager._instance.GetLocalGun ().gunUI.RefrshCoin (0, nResponese.selfGold);
			} else {

			}

		}

		private void RecvRotatingTurnTableRequst (object data)
		{
            
			FiFishGetCoinPool nResp = (FiFishGetCoinPool)data;
			DragonCardInfo nTurnTableInfo = (DragonCardInfo)Facade.GetFacade ().data.Get (FacadeConfig.UI_DRAGONCARD);
			nTurnTableInfo.GoldPool = nResp.saveLongRewardPoolGold;
			//UnityEngine.Debug.LogError ("SendRotatingTurnTableRequst" + nResp.result + "ID" + nResp.userID + "Type" + nResp.type + "jc");
			if (nResp.result == 0) {
				//接收新的数据时就更新UI
				if (Facade.GetFacade ().ui.Get (FacadeConfig.UI_COINNUM) != null) {
					Facade.GetFacade ().ui.Get (FacadeConfig.UI_COINNUM).OnRecvData (FiEventType.RECV_XL_TURNTABLEGETPOOL_RESPONSE, data);
				}
				//for (int i = 0; i < nTurnTableInfo.GoldPool.Count; i++) {
				//	UnityEngine.Debug.LogError ("nTurnTableInfo.GoldPool" + nTurnTableInfo.GoldPool [i] + "iiiii" + i);
				//}
			}
		}

		private void RecvManmonRankReward (object data)
		{
			FishGetRankReward nResp = (FishGetRankReward)data;
//			UnityEngine.Debug.LogError ("SendRotatingTurnTableRequst" + nResp.result + "count" + nResp.rankcout.Count);
			if (nResp.result == 0) {
//				for (int i = 0; i < nResp.rankcout.Count; i++) {
//					UnityEngine.Debug.LogError ("type" + nResp.rankcout [i].type + "value" + nResp.rankcout [i].value);
//				}
				UIManmonGameShow.instance.rankcout = nResp.rankcout;
			}
		}


		GameObject Window_1;
		GameObject obj_1;
		GameObject Window_2 = null;
		GameObject obj_2 = null;
		GameObject Window_3 = null;
		GameObject obj_3 = null;
		GunControl tempGun = null;

		private void RcvTurntableLiuShuiResponse (object data)
		{
			FishGetGoldLiuShui nResp = (FishGetGoldLiuShui)data;
			//Debug.Log ("新用户在新手场内累计发炮流水达到10000时====" + nResp.nIsUserTopUpState);
			//Debug.Log("当用户在3倍房和BOSS场内击杀200倍以上BOSS====" + nResp.nTwoSelectOneTopUpState);
			//Debug.Log("当天发炮流水达到500万以上的用户====" + nResp.nThreeSelectOneTopUpdate);
			MyInfo myInfo = DataControl.GetInstance ().GetMyInfo ();

//			Debug.Log(" ~~~~~~~~~~ myInfo.showTreasure = "+ myInfo.showTreasure + " ~~~~~~~~~~ myInfo.showDouble = "+ myInfo.showDouble);
			RoomInfo myRoomInfo = DataControl.GetInstance ().GetRoomInfo ();
			if (myInfo.showAwesome == 0 && nResp.nIsUserTopUpState == 1 && myRoomInfo.roomMultiple == 0) {
				myInfo.showAwesome = 1;
				if (Window_1 == null) {
					obj_1 = UnityEngine.Resources.Load ("Window/Preferential/AwesomeCanvas") as UnityEngine.GameObject;
					Window_1 = UnityEngine.Object.Instantiate (obj_1);
					Window_1.SetActive (true);
					Window_1.gameObject.GetComponent<AwesomeCommand> ().ShowDubbble ();
					if (myInfo.showAwesome == 1) {
						GiftBagManager.Instance.awesome.gameObject.SetActive (true);
					}
				} else if (Window_1.gameObject.activeSelf == false) {
					Window_1.SetActive (true);
				}
			}
			if (myInfo.showDouble == 0 && nResp.nTwoSelectOneTopUpState == 1) {
				myInfo.showDouble = 1;
                PlayerPrefs.SetString(myInfo.userID + "isShowDouble", "1");
                if (Window_2 == null && AppInfo.trenchNum > 51000000) {
					obj_2 = UnityEngine.Resources.Load ("Window/Preferential/Double") as UnityEngine.GameObject;
					Window_2 = UnityEngine.Object.Instantiate (obj_2);
					Window_2.SetActive (true);
                    //需求更改不需要再渔场显示
					//if (myInfo.showDouble == 1) {
					//	GiftBagManager.Instance.doubleGiftBag.gameObject.SetActive (true);
					//}
				} else if (Window_2.gameObject.activeSelf == false) {
					Window_2.SetActive (true);
				}
			}
			//Debug.Log("用户id===="+myInfo.userID);
			//Debug.Log (PlayerPrefs.GetString (myInfo.userID + "isBankruptcy", "-1"));
			if (myInfo.showTreasure == 0 && nResp.nThreeSelectOneTopUpdate == 1 && IsGlod (myInfo.userID)) {
				myInfo.showTreasure = 1;
				PlayerPrefs.SetString (myInfo.userID + "isShowTreasure", "1");
				if (Window_3 == null && AppInfo.trenchNum > 51000000) {
					obj_3 = UnityEngine.Resources.Load ("Window/Preferential/Treasure") as UnityEngine.GameObject;
					Window_3 = UnityEngine.Object.Instantiate (obj_3);
					Window_3.SetActive (true);
					//UIHallCore.Instance.treasureButton.gameObject.SetActive(true);
				} else if (Window_3.gameObject.activeSelf == false) {
					Window_3.SetActive (true);
				}
			}

			//UnityEngine.Debug.LogError ("RcvTurntableLiuShuiResponse" + nResp.lUserID + "lTimeData" + nResp.lTimeData + "lLongLiuShui" + nResp.lLongLiuShui);
			//UnityEngine.Debug.LogError ("下发流水和重置时间++++++++++++++++++++++++++++++++++++++++++++");
			DragonCardInfo nTurnTableInfo = (DragonCardInfo)Facade.GetFacade ().data.Get (FacadeConfig.UI_DRAGONCARD);
			nTurnTableInfo.LongLiuShui = nResp.lLongLiuShui;
			nTurnTableInfo.userId = nResp.lUserID;
			if (FishingUpTurnTable._instance != null) {
				FishingUpTurnTable._instance.SetSpeedOfPro (nResp.lLongLiuShui);
			} else if (nResp.lTimeData != 0) {
				nTurnTableInfo.LongTime = nResp.lTimeData * 60;
			}
			GameObject obj = GameObject.FindGameObjectWithTag ("Level");
			if (nResp.ShengJiDuanWei != 0) {
				if (obj != null) {
					UnityEngine.GameObject.Destroy (GameObject.FindGameObjectWithTag ("Level").gameObject);
				}
				UnityEngine.GameObject Window = UnityEngine.Resources.Load ("Window/HoroRewardLevelShow")as UnityEngine.GameObject;
				GameObject WindowClone = UnityEngine.GameObject.Instantiate (Window);
				UIHoroLevelShow reward = WindowClone.GetComponent<UIHoroLevelShow> ();
				reward.SetDuanWeiShow (nResp.ShengJiDuanWei.ToString ());
			}
			//新增升降段位
			Debug.LogError ("段位的升降" + nResp.nChangeCurDuanWei);
			Debug.LogError ("段位的升降nCurRank" + nResp.nCurRank);
			Debug.LogError ("段位的升降nCurMax" + nResp.nCurMax);
			if (nResp.nChangeCurDuanWei != 0) {
				GunControl gun = PrefabManager._instance.GetLocalGun ();
				if (nResp.nChangeCurDuanWei > 0) {
					gun.gunUI.GetBigwinRank (nResp.nChangeCurDuanWei, nResp.nCurRank, true);
				} else {
					gun.gunUI.GetBigwinRank (Math.Abs (nResp.nChangeCurDuanWei), nResp.nCurRank, false);
				}
		

			}


		}

		/// <summary>
		/// 判读某个用户金币是否不足
		/// </summary>
		private bool IsGlod (int userId)
		{
			if (tempGun == null) {
				tempGun = PrefabManager._instance.GetGunByUserID (userId);//得到某个用户的GunCtroll
			}
			return !tempGun.CheckCanFire ();
		}

		private void RcvTurnTableLuckDrawResponse (object data)
		{
			FishTurnTableLuckyDraw nResp = (FishTurnTableLuckyDraw)data;
//			UnityEngine.Debug.LogError ("nResp.prpo2,VALUE" + nResp.prpo2 [0].value);
//			UnityEngine.Debug.LogError ("nResp.prpo2,TYPE" + nResp.prpo2 [0].type);
//			UnityEngine.Debug.LogError ("nResp.type" + nResp.type);
			FishingUpTurnTable._instance.prizeType = nResp.prpo2 [0].type;
			FishingUpTurnTable._instance.prizeValue = nResp.prpo2 [0].value;
			FishingUpTurnTable._instance.prizeIndex = nResp.type;
//			UnityEngine.Debug.LogError ("nResp.prpo2.count" + nResp.prpo2.Count);
		}

		private void RcvTurnTableChangeLiuShuiTime (object data)
		{
            
			FishChangeLiuShuiTime nResp = (FishChangeLiuShuiTime)data;
			//UnityEngine.Debug.LogError ("nResp.lDiamond+++++++++++++" + nResp.lDiamond);
			//UnityEngine.Debug.LogError ("nResp.nTimeData++++++++++++" + nResp.nTimeData);
			DragonCardInfo nTurnTableInfo = (DragonCardInfo)Facade.GetFacade ().data.Get (FacadeConfig.UI_DRAGONCARD);
			nTurnTableInfo.liuShuiDiamond = nResp.lDiamond;
			nTurnTableInfo.LongTime += (nResp.nTimeData * 60);
		}

		private void RecvOtherCancelSkill (object data)
		{
			InformOtherCancelSKill nResp = (InformOtherCancelSKill)data;
			//UnityEngine.Debug.LogError ("nResp.UeerId+++++++++++++" + nResp.lUserID);
			//UnityEngine.Debug.LogError ("nResp.SkillId++++++++++++" + nResp.skillID);
			if (PrefabManager._instance == null)
				return;
			GunControl gun = PrefabManager._instance.GetGunByUserID ((int)nResp.lUserID);
			SkillType skillType = 0;
			if (nResp.skillID == 3) {
				skillType = SkillType.Berserk;

			} else if (nResp.skillID == 8) {
				skillType = SkillType.Replication;
			}
			gun.UserCanCelSkill (skillType, Skill.Instance.skillDuration, false);

		}

		private void RectFishingUserRank (object data)
		{
			FishingUserRank nResp = (FishingUserRank)data;
			//UnityEngine.Debug.LogError ("nResp.UeerId+++++++++++++" + nResp.lUserID);
			//UnityEngine.Debug.LogError ("nResp.nRank++++++++++++" + nResp.nRank);
			if (PrefabManager._instance == null) {
				return;
			}
			GunControl gun = PrefabManager._instance.GetGunByUserID ((int)nResp.lUserID);
			//GunInUI gun = PrefabManager._instance.get
			gun.gunUI.GetFishingUserRank ((int)nResp.lUserID, nResp.nRank);
                                                                    
		}

		private void RecvMyFishingUserRank (object data)
		{
			FishingUserRank nResp = (FishingUserRank)data;
			if (PrefabManager._instance == null) {
				return;
			}

			GunControl gun = PrefabManager._instance.GetLocalGun ();
			//UnityEngine.Debug.LogError ("nResp.MyUeerId+++++++++++++" + nResp.lUserID);
			//UnityEngine.Debug.LogError ("nResp.MynRank++++++++++++" + nResp.nRank);
			gun.gunUI.GetFishingUserRank ((int)nResp.lUserID, nResp.nRank);

		}

		private void RecvBossKillRank (object data)
		{
			FishingBossKillRank nResp = (FishingBossKillRank)data;
			if (PrefabManager._instance == null) {
				return;
			}
			// UnityEngine.Debug.LogError("nResp.OtherUeerIdBoss猎杀+++++++++++++" + nResp.lUserID);
			// UnityEngine.Debug.LogError("nResp.OthernRankBoss猎杀++++++++++++" + nResp.nRank);
			GunControl gun = PrefabManager._instance.GetGunByUserID ((int)nResp.lUserID);
			gun.gunUI.GetFishingUserRank ((int)nResp.lUserID, nResp.nRank, true);
		}

		private void RecvBossKillMyRank (object data)
		{
			FishingBossKillRank nResp = (FishingBossKillRank)data;
			if (PrefabManager._instance == null) {
				return;
			}
			GunControl gun = PrefabManager._instance.GetLocalGun ();
			// UnityEngine.Debug.LogError ("nResp.MyUeerIdBoss猎杀+++++++++++++" + nResp.lUserID);
			// UnityEngine.Debug.LogError ("nResp.MynRankBoss猎杀++++++++++++" + nResp.nRank);
			gun.gunUI.GetFishingUserRank ((int)nResp.lUserID, nResp.nRank, true);
		}

		private void RecvMyManmonWincout (object data)
		{
			MamonMaxWinCounts nResp = (MamonMaxWinCounts)data;

			//UnityEngine.Debug.LogError ("nResp.MyUeerId+++++++++++++11111111111" + nResp.useid);
			//UnityEngine.Debug.LogError ("nResp.MynRank++++++++++++" + nResp.wintime);
			DragonCardInfo nDragoninfo = (DragonCardInfo)Facade.GetFacade ().data.Get (FacadeConfig.UI_DRAGONCARD);
			nDragoninfo.ManmonWinTime = nResp.wintime;

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

        //private void NotifyToUi(int nType, object data)
        //{
        //    //Debug.LogError ( "--------NotifyToUi-------" );
        //    IUiMediator nMediator = Facade.GetFacade().ui.Get(FacadeConfig.UI_BACKPACK_MODULE_ID);
        //    if (nMediator != null)
        //        nMediator.OnRecvData(nType, data);
        //}
        //		case FiEventType.RECV_XL_BOSSROOMMATCH_RESPOSE:
        //		fishingMsg.RcvBossMatchInfo (data);
        //		break;
        //		case FiEventType.RECV_XL_NOTIFYSIGNUP_RESPOSE:
        //		fishingMsg.RcvNotifySignUp (data);
        //		break;
        /// <summary>
        /// Boss场匹配
        /// </summary>
        /// <param name="data">Data.</param>
        public void RecvBossMatchInfo (object data)
		{
			FiBossRoomMatchInfo matchInfo = (FiBossRoomMatchInfo)data;
			MyInfo myInfo = (MyInfo)Facade.GetFacade ().data.Get (FacadeConfig.USERINFO_MODULE_ID);
//			Debug.LogError ("RecvBossMatchInfo matchInfo.type = " + matchInfo.type);
			if (AppInfo.isInHall) {
				if (matchInfo.type == 1) {
					if ((int)myInfo.gold < 5000000 && !myInfo.misHaveDraCard) {
						return;
					} 
					string path = string.Empty;
					path = "Game/BossMatchCanvas";
					AppControl.OpenWindow (path);	
					SwitchType (matchInfo.type, matchInfo.content);
				}
			} else {
				bool isLocal = PrefabManager._instance.GetLocalGun ().isLocal;
//				Debug.LogError ("ToBossMatchInfo matchInfo.type = " + matchInfo.type);

				if (isLocal) {
					if (PrefabManager._instance != null) {
						SwitchType (matchInfo.type, matchInfo.content);
					}
				}
			}
		}

		/// <summary>
		/// 接受Boss场
		/// </summary>
		/// <param name="data">Data.</param>
		public void RecvNotifySignUp (object data)
		{
			FiNotifySignUp _signUp = (FiNotifySignUp)data;
			switch (_signUp.type) {
			case 1:
				if (!AppInfo.isInHall) {
					LeaveRoomTool.LeaveRoom ();	
				} else {
					//直接报名
					UIHallObjects.GetInstance ().PlayBossMatch ();
				}
				break;
			case 2:
				if (!AppInfo.isInHall) {
					LeaveRoomTool.LeaveRoom ();	
				} else {
					
				}
				break;
			default:
				break;
			}
		}

		void SwitchType (int type, ByteString content = null)
		{
			MyInfo myInfo = (MyInfo)Facade.GetFacade ().data.Get (FacadeConfig.USERINFO_MODULE_ID);
			switch (type) {
			//开始
			case 1:
				if ((int)myInfo.gold < 5000000 && !myInfo.misHaveDraCard) {
					return;
				} 
				if (PrefabManager._instance != null) {
					PrefabManager._instance.CreateBossMatchPrefab ();
				}
                //BossMatchEndAndStart(true);//打开漩涡
				byte[] byteTemp = content.ToByteArray ();
				string des = Encoding.UTF8.GetString (byteTemp);
				//					des = System.Text.Encoding.
//				Debug.LogError ("ToBossMatchInfo des = " + des);
				SplitDescripe (des);
				break;
			//结束
			case 2:
//				BossMatchScript.beginType = 2;
				if (GameController._instance.isBossMode) {
					if (PrefabManager._instance != null) {
						PrefabManager._instance.CreateBossMatchPrefab ();
						PrefabManager._instance.GetLocalGun ().CompleteAllSkill ();
						PrefabManager._instance.GetSkillUIByType (SkillType.Berserk).CompleteAllUseSkill (SkillType.Berserk);
						PrefabManager._instance.GetSkillUIByType (SkillType.Lock).CompleteAllUseSkill (SkillType.Lock);
						PrefabManager._instance.GetSkillUIByType (SkillType.Replication).CompleteAllUseSkill (SkillType.Replication);
					}
						//BossMatchEndAndStart(false);
						BossMatchScript.Instance.SetTextDescription(false, "立即返回", "繼續遊戲");
					}
				//					string des = matchInfo.content.ToString ();
				//					Debug.LogError ("ToBossMatchInfo des = " + des);
//				UIHallObjects.GetInstance ().SndNotifySignUpRequest (2);
				break;
			default:
				break;
			}
		}

		/// <summary>
		/// 文字截取
		/// </summary>
		/// <param name="content">Content.</param>
		void SplitDescripe (string content)
		{
			string[] strArr = content.Split (new string[]{ "&" }, StringSplitOptions.RemoveEmptyEntries);
			foreach (string s in strArr) {
//				Debug.LogError ("字符串 s = " + s);
			}
//			string title = "限时猎杀赛马上开始";
			string time = strArr [0] + "\n" + strArr [1];
			string reward = strArr [2];
			string rule = strArr [3];
			//		string ruleTemp = rule.Replace ("*", "<color=#f6fc29ff>");
			//, title
			BossMatchScript.Instance.SetTextDescription(true, "立即參與", "暫不參加", time, reward, rule);
		}

		private void RecvIntoBossRoom (object data)
		{
			IntoBossRoomMessage nResp = (IntoBossRoomMessage)data;
			//Debug.LogError("nResp.nRoomIndex"+nResp.nRoomIndex);
			//Debug.LogError("nResp.nType"+nResp.nType);
			//UIColseManage.instance.intoBossRommType = nResp.nRoomIndex;
			byte[] byteTemp = nResp.modifyNick.ToByteArray ();
			string des = Encoding.UTF8.GetString (byteTemp);
			PrefabManager._instance.CreateIntoBossRoomPrefab (des);
			//UnityEngine.GameObject Window = UnityEngine.Resources.Load("Game/IntoBossRoom") as UnityEngine.GameObject;
			//Window.transform.Find("Text").GetComponent<UnityEngine.UI.Text>().text = des;
			//UnityEngine.GameObject.Instantiate(Window);

		}

		/// <summary>
		/// 大厅的boss猎杀赛漩涡入口
		/// </summary>
		void BossMatchEndAndStart (bool isStart)
		{
			if (UIHallCore.Instance == null) {
				return;
			}
			UIHallCore.Instance.bossMatchVortex.SetActive (isStart);
		}

		public  void OnDestroy ()
		{
			EventControl nControl = EventControl.instance ();
			nControl.removeEventHandler (FiEventType.RECV_GET_ROBOT_RESPONSE, OnRecvRobotResponse);
			nControl.removeEventHandler (FiEventType.RECV_CHANGE_CANNON_STYLE_RESPONSE, RecvChangeCannonStyleResponse);
			nControl.removeEventHandler (FiEventType.RECV_OTHER_CHANGE_CANNON_STYLE_INFORM, RecvOtherChangeCannonStyleResponse);
			nControl.removeEventHandler (FiEventType.RECV_OTHER_UNLOCK_CANNON_MUTIPLE_INFORM, RecvOtherUnlockCannonInform);

			nControl.removeEventHandler (FiEventType.RECV_FISHS_CREATED_INFORM, RecvFishCreateInform);
			nControl.removeEventHandler (FiEventType.RECV_OTHER_FIRE_BULLET_INFORM, RecvOtherFireBullet);
			nControl.removeEventHandler (FiEventType.RECV_HITTED_FISH_RESPONSE, RecvHitFishResponse);

			nControl.removeEventHandler (FiEventType.RECV_FISH_OUT_RESPONSE, RecvFishOutResponse);
			nControl.removeEventHandler (FiEventType.RECV_CHANGE_CANNON_RESPONSE, RecvChangeCannonResponse);
			nControl.removeEventHandler (FiEventType.RECV_OTHER_CHANGE_CANNON_INFORM, RecvOtherChangeCannonMultiple);

			nControl.removeEventHandler (FiEventType.RECV_USE_EFFECT_RESPONSE, RecvUserEffectResponse);
			nControl.removeEventHandler (FiEventType.RECV_OTHER_EFFECT_INFORM, RecvOtherEffectResponse);
			nControl.removeEventHandler (FiEventType.RECV_FISH_FREEZE_TIMEOUT_INFORM, RecvFishFreezeTimeOutResponse);
			nControl.removeEventHandler (FiEventType.RECV_FISH_TIDE_COMING_INFORM, RcvTideComingInform);
			nControl.removeEventHandler (FiEventType.RECV_FISH_TIDE_CLEAN_INFORM, RcvTideCleanInform);
			nControl.removeEventHandler (FiEventType.RECV_XL_MANMONSTATRT_RESPONSE, RcvManmonStartShowResponse);
			nControl.removeEventHandler (FiEventType.RECV_XL_MANMONSETTING_RESPONSE, RcvManmonSettingshowResponse);
			nControl.removeEventHandler (FiEventType.RECV_XL_MANMONBETTING_RESPONSE, RcvManmonBettingshowResponse);
			nControl.removeEventHandler (FiEventType.RECV_XL_MANMONYAOQIANSHU_RESPONSE, RcvManmonYaoQainShushowResponse);
			nControl.removeEventHandler (FiEventType.RECV_XL_TURNTABLEGETPOOL_RESPONSE, RecvRotatingTurnTableRequst);
			nControl.removeEventHandler (FiEventType.RECV_XL_MANMONRANKREWARD_REQUST, RecvManmonRankReward);
			nControl.removeEventHandler (FiEventType.RECV_XL_TURNTABLELUCKDRAW_RESPOSE, RcvTurnTableLuckDrawResponse);
			nControl.removeEventHandler (FiEventType.RECV_XL_TURNTABLELIUSHUI_RESPONSE, RcvTurntableLiuShuiResponse);
			nControl.removeEventHandler (FiEventType.RECV_XL_CHANGELIUSHUITIME_RESPOSE, RcvTurnTableChangeLiuShuiTime);
			nControl.removeEventHandler (FiEventType.RECV_XL_CANCELOTHERSKILL_REQUST, RecvOtherCancelSkill);
			nControl.removeEventHandler (FiEventType.RECV_XL_UPDATERANKINFO_RESPOSE, RectFishingUserRank);
			nControl.removeEventHandler (FiEventType.RECV_XL_UPDATEMYRANKINFO_RESPOSE, RecvMyFishingUserRank);
			//接受更换炮座
			nControl.removeEventHandler (FiEventType.RECV_XL_EQUIPMENTBARBETTE_RESPONSE, RecvChangeBarbetteStyleResponse);
			nControl.removeEventHandler (FiEventType.RECV_XL_WINTIMECOUNTFO_RESPOSE, RecvMyManmonWincout);
			//移除boss匹配
			nControl.removeEventHandler (FiEventType.RECV_XL_BOSSROOMMATCH_RESPOSE, RecvBossMatchInfo);
			//移除boss接受
			nControl.removeEventHandler (FiEventType.RECV_XL_NOTIFYSIGNUP_RESPOSE, RecvNotifySignUp);

			nControl.removeEventHandler (FiEventType.RECV_XL_INTOBOSSROMM_RESPOSE, RecvIntoBossRoom);

			nControl.removeEventHandler (FiEventType.RECV_XL_BOSSKILLRANK_RESPOSE, RecvBossKillRank);

			nControl.removeEventHandler (FiEventType.RECV_XL_BOSSKILLMYRANK_RESPOSE, RecvBossKillMyRank);
		}
	}
}

