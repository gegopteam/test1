using System;
using System.Collections.Generic;
using UnityEngine;

namespace AssemblyCSharp
{
	public class FishingPkRoomMsgHandle:IMsgHandle
	{
		public FishingPkRoomMsgHandle ()
		{
			
		}

		public void SendPKTorpedoExplodeRequest (int torpedoID, int torpedoType, List<FiFish> fishs)
		{//发送PK场鱼雷爆炸请求
			FiTorpedoExplodeRequest torpedoExplode = new FiTorpedoExplodeRequest ();
			torpedoExplode.torpedoId = torpedoID;
			torpedoExplode.torpedoType = torpedoType;
			torpedoExplode.fishes = new List<FiFish> ();
			foreach (FiFish fish in fishs) {
				if (null != fish) {
					torpedoExplode.fishes.Add (fish);
				}
			}
			DataControl.GetInstance ().PushSocketSnd (FiEventType.SEND_PK_TORPEDO_EXPLODE_REQUEST, torpedoExplode);
		}

		public void SendPKEffectRequest (int effectId)
		{//发送特效请求
			MyInfo myInfo = (MyInfo)Facade.GetFacade ().data.Get (FacadeConfig.USERINFO_MODULE_ID);
			FiEffectRequest effectInfo = new FiEffectRequest ();
			effectInfo.userId = myInfo.userID;
			effectInfo.effect = new FiEffectInfo ();
			effectInfo.effect.type = effectId;
			DataControl.GetInstance ().PushSocketSnd (FiEventType.SEND_PK_USE_EFFECT_REQUEST, effectInfo);
		}

		public void SendPKLaunchTorpedoRequest (int torpedoID, int torpedoType, int x, int y)
		{//发送鱼雷请求
			FiLaunchTorpedoRequest launchTorpedo = new FiLaunchTorpedoRequest ();
			launchTorpedo.torpedoId = torpedoID;
			launchTorpedo.torpedoType = torpedoType;
			launchTorpedo.position = new Cordinate ();
			launchTorpedo.position.x = x;
			launchTorpedo.position.y = y;
			DataControl.GetInstance ().PushSocketSnd (FiEventType.SEND_PK_LAUNCH_TORPEDO_REQUEST, launchTorpedo);
		}

		public void SendPKEnterRoomRequest (int roomType, int goldType)
		{
			FiEnterPKRoomRequest enterRoom = new FiEnterPKRoomRequest ();
			enterRoom.roomType = roomType;
			enterRoom.goldType = goldType;
			MyInfo myInfo = (MyInfo)Facade.GetFacade ().data.Get (FacadeConfig.USERINFO_MODULE_ID);
			DataControl.GetInstance ().PushSocketSnd (FiEventType.SEND_ENTER_PK_ROOM_REQUEST, enterRoom);
			myInfo.lastGame.level = goldType;

		}

		public void SendPKLeaveRoomRequest (int roomType, int roomIndex, int goldType)
		{
			FiLeavePKRoomRequest leaveRoom = new FiLeavePKRoomRequest ();
			leaveRoom.roomType = roomType;
			leaveRoom.roomIndex = roomIndex;
			leaveRoom.goldType = goldType;
			DataControl.GetInstance ().PushSocketSnd (FiEventType.SEND_LEAVE_PK_ROOM_REQUEST, leaveRoom);
			Debug.LogError ("SndPKLeaveRoomRequest:" + leaveRoom.ToString ());
		}

		public void OnInit ()
		{
			/*
			EventControl nControl = EventControl.instance ();
			nControl.addEventHandler ( FiEventType.RECV_ENTER_PK_ROOM_RESPONSE ,     RecvPKEnterRoomResponse );
			nControl.addEventHandler ( FiEventType.RECV_OTHER_ENTER_PK_ROOM_INFORM , RecvPKOtherEnterRoomInform );
			nControl.addEventHandler ( FiEventType.RECV_LEAVE_PK_ROOM_RESPONSE ,     RecvPKLeaveRoomResponse );
			nControl.addEventHandler ( FiEventType.RECV_OTHER_LEAVE_PK_ROOM_INFORM , RecvPKOtherLeaveRoomInform );

			//pk场开始后玩家收到游戏开始通知
			nControl.addEventHandler ( FiEventType.RECV_START_PK_GAME_INFORM   ,     RecvPKStartGameInform );


			nControl.addEventHandler ( FiEventType.RECV_PK_LAUNCH_TORPEDO_RESPONSE ,      RecvPKLaunchTorpedoResponse  );
			nControl.addEventHandler ( FiEventType.RECV_PK_OTHER_LAUNCH_TORPEDO_INFORM ,  RecvPKOtherLaunchTorpedoInform );
			nControl.addEventHandler ( FiEventType.RECV_PK_TORPEDO_EXPLODE_RESPONSE ,     RecvPKTorpedoExplodeResponse );
			nControl.addEventHandler ( FiEventType.RECV_PK_OTHER_TORPEDO_EXPLODE_INFORM , RecvPKOtherTorpedoExplodeInform );
			nControl.addEventHandler ( FiEventType.RECV_OTHER_LEAVE_PK_ROOM_INFORM ,      RecvPKOtherLeaveRoom );
			nControl.addEventHandler ( FiEventType.RECV_PK_GOLD_GAME_RESULT_INFORM ,      RecvPKGoldGameResultInform);
			nControl.addEventHandler ( FiEventType.RECV_PRE_PKGAME_COUNTDOWN_INFORM ,     RecvPKPreGameCountdownInform);
			nControl.addEventHandler ( FiEventType.RECV_PKGAME_COUNTDOWN_INFORM ,         RecvPKGameCountdownInform);

			nControl.addEventHandler ( FiEventType.RECV_PK_USE_EFFECT_RESPONSE ,          RecvPKUseEffectResponse);
			nControl.addEventHandler ( FiEventType.RECV_PK_OTHER_EFFECT_INFORM ,          RecvPKOtherEffectInform);

			nControl.addEventHandler ( FiEventType.RECV_PK_DISTRIBUTE_PROPERTY_INFORM ,   RecvPKDistributePropertyInform);
			nControl.addEventHandler ( FiEventType.RECV_PK_POINT_GAME_RESULT_INFORM ,     RecvPKPointResultInform);
			nControl.addEventHandler ( FiEventType.RECV_PK_POINT_GAME_ROUND_RESULT_INFORM ,     RecvPKPointRoundResultInform);*/

			EventControl nControl = EventControl.instance ();
			//其他玩家断线后，重连进入渔场
			nControl.addEventHandler (FiEventType.RECV_OTHER_RECONNECT_PKGAME_INFORM, RecvOtherReconnectPkRoom);
			//登陆后，服务器下发还在PK场消息
			nControl.addEventHandler (FiEventType.RECV_HAVE_DISCONNECTED_ROOM_INFORM, RcvPKHaveDisconnectedRoomInform);
			//获取PK场重连消息
			nControl.addEventHandler (FiEventType.RECV_RECONNECT_GAME_RESPONSE, RcvPKRoomReconnectResponse);
		}

		//收到服务器重连房间的数据回馈
		private void RcvPKRoomReconnectResponse (object data)
		{
			/*FiReconnectResponse nReconnect = (FiReconnectResponse)data;
		
			if (nReconnect.result != 0) {
				Debug.LogError ( "[ pkroom fishing ] recv reconnect pk room error response!!!" );
				return;
			}
			//if( nReconnect )
			RoomInfo roomInfo =(RoomInfo) Facade.GetFacade ().data.Get ( FacadeConfig.ROOMINFO_MODULE_ID );
			MyInfo myInfo = (MyInfo)Facade.GetFacade ().data.Get ( FacadeConfig.USERINFO_MODULE_ID ); 
			roomInfo.Clear ();

			UIFishingMsg.GetInstance ().SetFishing ( nReconnect.roomType );
			myInfo.lastGame.type = nReconnect.roomType;
			myInfo.lastGame.level = nReconnect.goldType;
			roomInfo.roomType = nReconnect.roomType;
			roomInfo.goldType = nReconnect.goldType;
			roomInfo.roomIndex = nReconnect.roomIndex;
			myInfo.seatIndex = nReconnect.seatIndex;
			myInfo.SetPKReconnectProperties ( nReconnect.properties );
			//指示重连进PK场
			myInfo.isReconnectInPKRoom = true;

			if(null!=nReconnect.others)
			{
				Debug.LogError ( "[------------]" +  nReconnect.others.Count );

				foreach(FiOtherGameInfo nGameInfo in nReconnect.others)
				{
					
					FiUserInfo nReconnetUser = new FiUserInfo ();
					nReconnetUser.avatar = nGameInfo.avatar;
					nReconnetUser.gender = nGameInfo.gender;
					nReconnetUser.nickName = nGameInfo.nickname;
					nReconnetUser.seatIndex = nGameInfo.seatIndex;
					nReconnetUser.userId = nGameInfo.userId;
					nReconnetUser.vipLevel = nGameInfo.vipLevel;
					nReconnetUser.cannonStyle = nGameInfo.cannonStyle == 0 ? 3000: nGameInfo.cannonStyle;
					nReconnetUser.properties = nGameInfo.properties;
					roomInfo.AddUser ( nReconnetUser );
				}
			}
			UIFishingObjects.GetInstance ().InitCannonInfo ();
			//锁定屏幕，不允许操作
			myInfo.lockScene = true;
			myInfo.TargetView = AppView.FISHING;
			AppControl.ToView (AppView.LOADING);*/
		}

		//只有在pk场才会有这种操作
		void RecvOtherReconnectPkRoom (object data)
		{
			FiOtherReconnectPKGameInform nInform = (FiOtherReconnectPKGameInform)data;
		}

		private void RecvPKPointResultInform (object data)
		{
			
		}

		private void RecvPKPointRoundResultInform (object data)
		{

		}

		private void RecvPKDistributePropertyInform (object data)
		{//收到PK场分配技能统计广播
			FiDistributePKProperty distribute = (FiDistributePKProperty)data;
			UIFishingObjects.GetInstance ().SetPKDistributePropertyInfo (distribute);
			return;
		}

		private void RecvPKOtherEffectInform (object data)
		{
			FiOtherEffectInform otherEffectInfo = (FiOtherEffectInform)data;
			//otherEffectInfo.userId
			FiEffectInfo info = otherEffectInfo.info;
			if (null == info)
				return;
			UIFishingObjects.GetInstance ().ToEffect (info, otherEffectInfo.userId);
		}

		private void RecvPKUseEffectResponse (object data)
		{
			FiEffectResponse effectInfo = (FiEffectResponse)data;
			FiEffectInfo info = effectInfo.info; 
			if (null == info)
				return;
			if (0 == effectInfo.result) {
				UIFishingObjects.GetInstance ().ToEffect (info);
			} else {
				if (effectInfo.result == 40004) {
					HintTextPanel._instance.SetTextShow ("魚箱中的魚已經滿了，請稍後使用召喚", 2f);
				} else {
					//HintText._instance.ShowHint("道具使用失败，道具编号:" + info.type + " 错误结果:" + effectInfo.result);
				}
			}
		}

		private void RecvPKGameCountdownInform (object data)
		{//游戏倒计时
			FiPkGameCountDownInform countdown = (FiPkGameCountDownInform)data;
			UIFishingObjects.GetInstance ().ToPKGameCountdown (countdown);
		}

		private void RecvPKPreGameCountdownInform (object data)
		{//游戏开始前的倒计时
			FiPkGameCountDownInform countdown = (FiPkGameCountDownInform)data;
			UIFishingObjects.GetInstance ().ToPKPreGameCountdown (countdown);
		}

		private void RecvPKGoldGameResultInform (object data)
		{
			FiGoldGameResult goldGame = (FiGoldGameResult)data;
//			Debug.LogError("PointEnd:"+ goldGame.ToString ());
			GameController._instance.GameEnd (goldGame);
		}

		private void RecvPKOtherLeaveRoom (object data)
		{
			FiOtherLeavePKRoomInform nInform = (FiOtherLeavePKRoomInform)data;
			if (GameController._instance != null) {
				UIFishingObjects.GetInstance ().cannonManage.GetInfo (nInform.leaveUserId).cannon.gunUI.SetLeaveTextShow (true);
			} else {
				UnityEngine.Debug.LogError ("User leave: inGame=false,id=" + nInform.leaveUserId);
			}
		}


		private void RecvPKOtherTorpedoExplodeInform (object data)
		{//接收其他玩家鱼雷爆炸通知
			FiOtherTorpedoExplodeInform otherTorpedoExplode = (FiOtherTorpedoExplodeInform)data;
			UIFishingObjects.GetInstance ().ToPKOtherTorpedoExplode (otherTorpedoExplode);
		}

		private void RecvPKLaunchTorpedoResponse (object data)
		{//发送PK场鱼雷回复
			FiLaunchTorpedoResponse launchTorpedo = (FiLaunchTorpedoResponse)data;

		}

		private void RecvPKOtherLaunchTorpedoInform (object data)
		{//发送PK场鱼雷回复
			FiOtherLaunchTorpedoInform otherLaunchTorpedo = (FiOtherLaunchTorpedoInform)data;
			UIFishingObjects.GetInstance ().ToPKOtherLaunchTorpedo (otherLaunchTorpedo);
		}

		private void RecvPKTorpedoExplodeResponse (object data)
		{//发送PK场鱼雷回复
			FiTorpedoExplodeResponse torpedoExplode = (FiTorpedoExplodeResponse)data;
			UIFishingObjects.GetInstance ().ToPKTorpedoExplode (torpedoExplode);
		}

		private void NotifyPkRoomMediator (int nType, object data)
		{
			IUiMediator nUiMediator = Facade.GetFacade ().ui.Get (FacadeConfig.FISHING_PKROOM_MODULE_ID);
			if (nUiMediator != null)
				nUiMediator.OnRecvData (nType, data);
		}

		private void RecvPKStartGameInform (object data)
		{
			/*FiStartPKGameInform startGame = (FiStartPKGameInform)data;
			RoomInfo roomInfo =(RoomInfo) Facade.GetFacade ().data.Get ( FacadeConfig.ROOMINFO_MODULE_ID );
			roomInfo.roomType = startGame.roomType;
			MyInfo myInfo =(MyInfo) Facade.GetFacade ().data.Get ( FacadeConfig.USERINFO_MODULE_ID );
			myInfo.lastGame.type = startGame.roomType;*/

//			UIFishingObjects.GetInstance ()
//			UIFishingObjects.GetInstance ().InitCannon ();
//			NotifyPkRoomMediator ( FiEventType.RECV_START_PK_GAME_INFORM , data );
		}

		private void RecvPKOtherLeaveRoomInform (object data)
		{
			FiOtherLeavePKRoomInform leaveRoom = (FiOtherLeavePKRoomInform)data;
			RoomInfo roomInfo = (RoomInfo)Facade.GetFacade ().data.Get (FacadeConfig.ROOMINFO_MODULE_ID);
			roomInfo.RemoveUser (leaveRoom.leaveUserId);
			NotifyPkRoomMediator (FiEventType.RECV_OTHER_LEAVE_PK_ROOM_INFORM, data);
		}

		private void RecvPKLeaveRoomResponse (object data)
		{
			FiLeavePKRoomResponse leaveRoom = (FiLeavePKRoomResponse)data;
			if (leaveRoom.result != 0) {
				UnityEngine.GameObject Window1 = UnityEngine.Resources.Load ("Window/WindowTipsThree")as UnityEngine.GameObject;
				UnityEngine.GameObject WindowClone1 = UnityEngine.GameObject.Instantiate (Window1);
				UITipAutoNoMask ClickTips1 = WindowClone1.GetComponent<UITipAutoNoMask> ();
				ClickTips1.tipText.text = "離開房間失敗，錯誤碼:" + leaveRoom.result;
			}
			RoomInfo roomInfo = (RoomInfo)Facade.GetFacade ().data.Get (FacadeConfig.ROOMINFO_MODULE_ID);
			roomInfo.Clear ();
			NotifyPkRoomMediator (FiEventType.RECV_LEAVE_PK_ROOM_RESPONSE, data);
		}

		//进入PK场消息反馈通知
		private void RecvPKEnterRoomResponse (object data)
		{
			FiEnterPKRoomResponse enterRoom = (FiEnterPKRoomResponse)data;
			if (0 == enterRoom.result) {
				RoomInfo roomInfo = (RoomInfo)Facade.GetFacade ().data.Get (FacadeConfig.ROOMINFO_MODULE_ID);
				roomInfo.roomIndex = enterRoom.roomIndex;
				roomInfo.roomType = enterRoom.roomType;
				foreach (FiUserInfo user in enterRoom.others) {
					roomInfo.AddUser (new FiUserInfo (user));
				}
				MyInfo myInfo = (MyInfo)Facade.GetFacade ().data.Get (FacadeConfig.USERINFO_MODULE_ID);
				myInfo.seatIndex = enterRoom.seatindex;

				NotifyPkRoomMediator (FiEventType.RECV_ENTER_PK_ROOM_RESPONSE, data);
			}
		}

		//其他玩家进入PK打鱼场通知
		private void RecvPKOtherEnterRoomInform (object data)
		{
			FiOtherEnterPKRoomInform enterRoom = (FiOtherEnterPKRoomInform)data;
			RoomInfo roomInfo = (RoomInfo)Facade.GetFacade ().data.Get (FacadeConfig.ROOMINFO_MODULE_ID);
			roomInfo.AddUser (new FiUserInfo (enterRoom.other));

			NotifyPkRoomMediator (FiEventType.RECV_OTHER_ENTER_PK_ROOM_INFORM, data);
		}

		//登陆后发现还在PK场渔场，那么我们再次进入PK场
		private void RcvPKHaveDisconnectedRoomInform (object data)
		{
			//DataControl.GetInstance().PushSocketSnd (FiEventType.SEND_RECONNECT_GAME_REQUEST, null);
			UnityEngine.Debug.LogError ("------------------RcvPKHaveDisconnectedRoomInform------------------");
		}

		public void OnDestroy ()
		{
			EventControl nControl = EventControl.instance ();
			nControl.removeEventHandler (FiEventType.RECV_OTHER_RECONNECT_PKGAME_INFORM, RecvOtherReconnectPkRoom);
			nControl.removeEventHandler (FiEventType.RECV_HAVE_DISCONNECTED_ROOM_INFORM, RcvPKHaveDisconnectedRoomInform);
			nControl.removeEventHandler (FiEventType.RECV_RECONNECT_GAME_RESPONSE, RcvPKRoomReconnectResponse);

			/*EventControl nControl = EventControl.instance ();
			nControl.removeEventHandler ( FiEventType.RECV_ENTER_PK_ROOM_RESPONSE ,     RecvPKEnterRoomResponse );
			nControl.removeEventHandler ( FiEventType.RECV_OTHER_ENTER_PK_ROOM_INFORM , RecvPKOtherEnterRoomInform );
			nControl.removeEventHandler ( FiEventType.RECV_LEAVE_PK_ROOM_RESPONSE ,     RecvPKLeaveRoomResponse );
			nControl.removeEventHandler ( FiEventType.RECV_OTHER_LEAVE_PK_ROOM_INFORM , RecvPKOtherLeaveRoomInform );
			//pk场开始后玩家收到游戏开始通知
			nControl.removeEventHandler ( FiEventType.RECV_START_PK_GAME_INFORM   ,     RecvPKStartGameInform );


			nControl.removeEventHandler ( FiEventType.RECV_PK_LAUNCH_TORPEDO_RESPONSE ,      RecvPKLaunchTorpedoResponse  );
			nControl.removeEventHandler ( FiEventType.RECV_PK_OTHER_LAUNCH_TORPEDO_INFORM ,  RecvPKOtherLaunchTorpedoInform );
			nControl.removeEventHandler ( FiEventType.RECV_PK_TORPEDO_EXPLODE_RESPONSE ,     RecvPKTorpedoExplodeResponse );
			nControl.removeEventHandler ( FiEventType.RECV_PK_OTHER_TORPEDO_EXPLODE_INFORM , RecvPKOtherTorpedoExplodeInform );
			nControl.removeEventHandler ( FiEventType.RECV_OTHER_LEAVE_PK_ROOM_INFORM ,      RecvPKOtherLeaveRoom );
			nControl.removeEventHandler ( FiEventType.RECV_PK_GOLD_GAME_RESULT_INFORM ,      RecvPKGoldGameResultInform);
		
			nControl.removeEventHandler ( FiEventType.RECV_PRE_PKGAME_COUNTDOWN_INFORM ,     RecvPKPreGameCountdownInform);
			nControl.removeEventHandler ( FiEventType.RECV_PKGAME_COUNTDOWN_INFORM ,         RecvPKGameCountdownInform);
			nControl.removeEventHandler ( FiEventType.RECV_PK_USE_EFFECT_RESPONSE ,          RecvPKUseEffectResponse);
			nControl.removeEventHandler ( FiEventType.RECV_PK_OTHER_EFFECT_INFORM ,          RecvPKOtherEffectInform);
			nControl.removeEventHandler ( FiEventType.RECV_PK_DISTRIBUTE_PROPERTY_INFORM ,   RecvPKDistributePropertyInform);*/
		}

	}
}

