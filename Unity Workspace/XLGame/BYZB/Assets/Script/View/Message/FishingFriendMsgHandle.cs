using System;

namespace AssemblyCSharp
{
	public class FishingFriendMsgHandle : IMsgHandle
	{
		public FishingFriendMsgHandle ()
		{
			
		}

		public void SendEnterRedPacketRoomRequest( int nType )
		{
			FiEnterRedPacketRoomRequest nRequest = new FiEnterRedPacketRoomRequest ();
			nRequest.roomType = nType;
			DataControl.GetInstance().PushSocketSnd (FiEventType.SEND_ENTER_RED_PACKET_ROOM_REQUEST, nRequest);
		}

		public void SendPKDisbandFriendRoomRequest(int roomType, int roomIndex)
		{
			FiDisbandFriendRoomRequest disbandRoom = new FiDisbandFriendRoomRequest ();
			disbandRoom.roomType = roomType;
			disbandRoom.roomIndex = roomIndex;
			DataControl.GetInstance().PushSocketSnd (FiEventType.SEND_DISBAND_FRIEND_ROOM_REQUEST, disbandRoom);
		}

		public void SendPKLeaveFriendRoomRequest(int roomType, int roomIndex)
		{
			FiLeaveFriendRoomRequest leaveFriendRoom = new FiLeaveFriendRoomRequest ();
			leaveFriendRoom.roomType = roomType;
			leaveFriendRoom.roomIndex = roomIndex;
			DataControl.GetInstance().PushSocketSnd(FiEventType.SEND_LEAVE_FRIEND_ROOM_REQUEST, leaveFriendRoom);
		}

		public void SendPKEnterFriendRoomRequest(int roomIndex)
		{
			FiEnterFriendRoomRequest enterFriendRoom = new FiEnterFriendRoomRequest ();
			enterFriendRoom.roomIndex = roomIndex;
			DataControl.GetInstance().PushSocketSnd(FiEventType.SEND_ENTER_FRIEND_ROOM_REQUEST, enterFriendRoom);
		}

		public void SendPKCreateFriendRoomRequest(int roomType, int goldType, int timeType, int roundType)
		{
			FiCreateFriendRoomRequest createFriendRoom = new FiCreateFriendRoomRequest ();
			createFriendRoom.roomType = roomType;
			createFriendRoom.goldType = goldType;
			createFriendRoom.timeType = timeType;
			createFriendRoom.roundType = roundType;
			MyInfo myInfo =(MyInfo) Facade.GetFacade ().data.Get ( FacadeConfig.USERINFO_MODULE_ID );
			myInfo.lastGame.level = goldType;
			myInfo.lastGame.type = roomType;
			//dataControl.PushSocketSnd(FiEventType.SEND_CREATE_FRIEND_ROOM_REQUEST, createFriendRoom);

			DataControl.GetInstance().PushSocketSnd(FiEventType.SEND_CREATE_FRIEND_ROOM_REQUEST, createFriendRoom);
		}

		public void SendPKPrepareGameRequest(int userID, int roomType, int roomIndex)
		{
			FiPreparePKGame prepareGame = new FiPreparePKGame ();
			prepareGame.userId = userID;
			prepareGame.roomType = roomType;
			prepareGame.roomIndex = roomIndex;
			DataControl.GetInstance().PushSocketSnd (FiEventType.SEND_PREPARE_PKGAME_REQUEST, prepareGame);
		}

		public void SendPKCancelPrepareGame(int userID, int roomType, int roomIndex)
		{
			FiCancelPreparePKGame cancelPrepareGame = new FiCancelPreparePKGame ();
			cancelPrepareGame.userId = userID;
			cancelPrepareGame.roomType = roomType;
			cancelPrepareGame.roomIndex = roomIndex;
			DataControl.GetInstance().PushSocketSnd (FiEventType.SEND_CANCEL_PREPARE_PKGAME, cancelPrepareGame);
		}

		public void OnDestroy()
		{
			EventControl nControl = EventControl.instance ();
			//nControl.removeEventHandler ( FiEventType.RECV_CREATE_FRIEND_ROOM_RESPONSE ,       RecvPKCreateFriendRoomResponse );
			nControl.removeEventHandler ( FiEventType.RECV_CREATE_FRIEND_ROOM_RESPONSE ,       RecvPKCreateFriendRoomResponse );
			nControl.removeEventHandler ( FiEventType.RECV_DISBAND_FRIEND_ROOM_RESPONSE ,      RecvPKDisbandFriendRoomResponse );

			nControl.removeEventHandler ( FiEventType.RECV_ENTER_FRIEND_ROOM_RESPONSE ,        RecvPKEnterFriendRoomResponse );
			nControl.removeEventHandler ( FiEventType.RECV_LEAVE_FRIEND_ROOM_RESPONSE ,        RecvPKLeaveFriendRoomResponse );

			nControl.removeEventHandler ( FiEventType.RECV_OTHER_ENTER_FRIEND_ROOM_INFORM   ,  RecvPKOtherEnterFriendRoomInform );
			nControl.removeEventHandler ( FiEventType.RECV_OTHER_LEAVE_FRIEND_ROOM_INFORM ,    RecvPKOtherLeaveFriendRoomInform );
			nControl.removeEventHandler ( FiEventType.RECV_OTHER_PREPARE_PKGAME_INFORM ,       RecvPKOtherPrepareGameInform );
			nControl.removeEventHandler ( FiEventType.RECV_OTHER_CANCEL_PREPARE_PKGAME_INFORM ,RecvPKOtherCancelPrepareGameInform );

			nControl.removeEventHandler ( FiEventType.RECV_DISBAND_FRIEND_ROOM_INFORM ,        RecvPKDisbandFriendRoomInform );
			nControl.removeEventHandler ( FiEventType.RECV_START_PK_GAME_RESPONSE ,            RecvPKStartGameResponse );

			//pk 场和 好友约战共有的协议
			nControl.removeEventHandler ( FiEventType.RECV_START_PK_GAME_INFORM   ,            RecvPKStartGameInform );

			nControl.removeEventHandler ( FiEventType.RECV_FRIEND_ROOM_RESULT_INFORM   ,       RecvPKFriendGameResultInform );/**/		}

		public void OnInit()
		{
			EventControl nControl = EventControl.instance ();
			nControl.addEventHandler ( FiEventType.RECV_CREATE_FRIEND_ROOM_RESPONSE ,       RecvPKCreateFriendRoomResponse );
			nControl.addEventHandler ( FiEventType.RECV_DISBAND_FRIEND_ROOM_RESPONSE ,      RecvPKDisbandFriendRoomResponse );

			nControl.addEventHandler ( FiEventType.RECV_ENTER_FRIEND_ROOM_RESPONSE ,        RecvPKEnterFriendRoomResponse );
			nControl.addEventHandler ( FiEventType.RECV_LEAVE_FRIEND_ROOM_RESPONSE ,        RecvPKLeaveFriendRoomResponse );

			nControl.addEventHandler ( FiEventType.RECV_OTHER_ENTER_FRIEND_ROOM_INFORM   ,  RecvPKOtherEnterFriendRoomInform );
			nControl.addEventHandler ( FiEventType.RECV_OTHER_LEAVE_FRIEND_ROOM_INFORM ,    RecvPKOtherLeaveFriendRoomInform );
			nControl.addEventHandler ( FiEventType.RECV_OTHER_PREPARE_PKGAME_INFORM ,       RecvPKOtherPrepareGameInform );
			nControl.addEventHandler ( FiEventType.RECV_OTHER_CANCEL_PREPARE_PKGAME_INFORM ,RecvPKOtherCancelPrepareGameInform );

			nControl.addEventHandler ( FiEventType.RECV_DISBAND_FRIEND_ROOM_INFORM ,        RecvPKDisbandFriendRoomInform );
			nControl.addEventHandler ( FiEventType.RECV_START_PK_GAME_RESPONSE ,            RecvPKStartGameResponse );

			//pk 场和 好友约战共有的协议
			nControl.addEventHandler ( FiEventType.RECV_START_PK_GAME_INFORM   ,            RecvPKStartGameInform );

			nControl.addEventHandler ( FiEventType.RECV_FRIEND_ROOM_RESULT_INFORM   ,       RecvPKFriendGameResultInform );/**/

		}

		public void RecvPKFriendGameResultInform( object data )
		{
			//FiFriendRoomGameResult nResult = (FiFriendRoomGameResult) data;
			if( GameController._instance != null )
				GameController._instance.GameEnd ( data );
		} 

		private void RecvPKOtherEnterFriendRoomInform(object data)
		{
			FiOtherEnterFriendRoomInform otherEnter = (FiOtherEnterFriendRoomInform)data;
			RoomInfo roomInfo = (RoomInfo)Facade.GetFacade ().data.Get (FacadeConfig.ROOMINFO_MODULE_ID);
			roomInfo.AddUser (otherEnter.other);
			NotifyToUi ( FiEventType.RECV_OTHER_ENTER_FRIEND_ROOM_INFORM , data );
		}

		void UpdateGoldInfo()
		{
			MyInfo myInfo = (MyInfo)Facade.GetFacade ().data.Get ( FacadeConfig.USERINFO_MODULE_ID );
			int nLevel = myInfo.lastGame.level;
			switch( nLevel )
			{
			case PKRoomRuleType.GOLD_TYPE_LOW:
				myInfo.gold -= PKRoomRuleType.GOLD_COUNT_LOW;
				break;
			case PKRoomRuleType.GOLD_TYPE_MIDDLE:
				myInfo.gold -= PKRoomRuleType.GOLD_COUNT_MIDDLE;
				break;
			case PKRoomRuleType.GOLD_TYPE_HIGH:
				myInfo.gold -= PKRoomRuleType.GOLD_COUNT_HIGH;
				break;
			case PKRoomRuleType.GOLD_TYPE_MASTER:
				myInfo.gold -= PKRoomRuleType.GOLD_COUNT_MASTER;
				break;
			}
		}

		//pk赛开始通知
		private void RecvPKStartGameInform(object data)
		{
			FiStartPKGameInform startGame = (FiStartPKGameInform)data;
			UnityEngine.Debug.LogError ( "[ pk room message ]-----------RecvPKStartGameInform--------------" + startGame.roomIndex + "/" + startGame.roomType );
			//指示初级场，中级场，高级场等
			//如果不是房卡模式，那么都要减金币
			if ( startGame.roomType != PKRoomRuleType.FRIEND_ROOM_CARD ) 
			{
				UpdateGoldInfo ();
			}

			UIFishingObjects.GetInstance ().EnterPkFishingRoom ( startGame.roomIndex , startGame.roomType );
		}

		//好友场房主开始游戏操作
		private void RecvPKStartGameResponse(object data)
		{
			FiStartPKGameResponse startGame = (FiStartPKGameResponse)data;

			if(0==startGame.result)
			{
				//RoomInfo roomInfo =(RoomInfo)Facade.GetFacade ().data.Get ( FacadeConfig.ROOMINFO_MODULE_ID );
				MyInfo myInfo =(MyInfo)Facade.GetFacade ().data.Get ( FacadeConfig.USERINFO_MODULE_ID );
				if ( startGame.roomType == PKRoomRuleType.FRIEND_ROOM_CARD) {
					myInfo.roomCard -= 1;
					UnityEngine.Debug.LogError ("[  ] room card mode" + myInfo.roomCard);
					BackpackInfo nBackInfo = (BackpackInfo)Facade.GetFacade ().data.Get (FacadeConfig.BACKPACK_MODULE_ID);
					nBackInfo.Delete (FiPropertyType.ROOM_CARD, 1);
				} else {
					
					//好友约战金币模式，扣除房费，然后根据场次，扣除手续费
					if (myInfo.lastGame.level == 1) {
						myInfo.gold -= 10000;
					}else if( myInfo.lastGame.level == 2 ){
						myInfo.gold -= 50000;
					}else if( myInfo.lastGame.level == 3 ){
						myInfo.gold -= 100000;
					}

					if ( startGame.roomType != PKRoomRuleType.FRIEND_ROOM_CARD ) {
						UpdateGoldInfo ();
					}
				}


				NotifyToUi ( FiEventType.RECV_START_PK_GAME_RESPONSE , data );
				//进入渔场
				UIFishingObjects.GetInstance ().EnterPkFishingRoom ( startGame.roomIndex , startGame.roomType );

				UnityEngine.Debug.LogError ( "[ message ]-----------RecvPKStartGameResponse--------------" );
			}
		}

		private void RecvPKDisbandFriendRoomResponse(object data)
		{
//			FiDisbandFriendRoomResponse disbandFriendRoom = (FiDisbandFriendRoomResponse)data;
			RoomInfo roomInfo =(RoomInfo)Facade.GetFacade ().data.Get ( FacadeConfig.ROOMINFO_MODULE_ID );
			roomInfo.Clear ();
			NotifyToUi ( FiEventType.RECV_DISBAND_FRIEND_ROOM_RESPONSE , data );
		}

		private void RecvPKDisbandFriendRoomInform(object data)
		{
			FiDisbandFriendRoomInform disbandFriendRoom = (FiDisbandFriendRoomInform)data;
			RoomInfo nRoomInfo =(RoomInfo)Facade.GetFacade ().data.Get ( FacadeConfig.ROOMINFO_MODULE_ID );
			nRoomInfo.Clear ();
			NotifyToUi ( FiEventType.RECV_DISBAND_FRIEND_ROOM_INFORM , data );
		}

		private void RecvPKOtherCancelPrepareGameInform (object data)
		{
			NotifyToUi ( FiEventType.RECV_OTHER_CANCEL_PREPARE_PKGAME_INFORM , data );
		}

		private void RecvPKOtherPrepareGameInform (object data)
		{
			NotifyToUi ( FiEventType.RECV_OTHER_PREPARE_PKGAME_INFORM , data );
		}

		private void RecvPKOtherLeaveFriendRoomInform(object data)
		{
			FiOtherLeaveFriendRoomInform leaveFriendRoom = (FiOtherLeaveFriendRoomInform) data;

			RoomInfo roomInfo =(RoomInfo)Facade.GetFacade ().data.Get ( FacadeConfig.ROOMINFO_MODULE_ID );
			roomInfo.RemoveUser (leaveFriendRoom.leaveUserId);
			NotifyToUi ( FiEventType.RECV_OTHER_LEAVE_FRIEND_ROOM_INFORM , data );
		}

		private void RecvPKLeaveFriendRoomResponse(object data)
		{
			FiLeaveFriendRoomResponse leaveFriendRoom = (FiLeaveFriendRoomResponse) data;
			RoomInfo roomInfo = (RoomInfo)Facade.GetFacade ().data.Get (FacadeConfig.ROOMINFO_MODULE_ID);
			roomInfo.Clear ();
			if ( leaveFriendRoom.result != 0 ) {
				UnityEngine.GameObject Window1 = UnityEngine.Resources.Load ("Window/WindowTipsThree")as UnityEngine.GameObject;
				UnityEngine.GameObject WindowClone1 = UnityEngine.GameObject.Instantiate ( Window1 );
				UITipAutoNoMask ClickTips1 = WindowClone1.GetComponent<UITipAutoNoMask> ();
				ClickTips1.tipText.text = "離開房間失敗，錯誤碼:" + leaveFriendRoom.result;
			}
			NotifyToUi ( FiEventType.RECV_LEAVE_FRIEND_ROOM_RESPONSE , data );
		}

		private void RecvPKEnterFriendRoomResponse(object data)
		{
			FiEnterFriendRoomResponse enterFriendRoom = (FiEnterFriendRoomResponse)data;
			if (0 == enterFriendRoom.result) {
				MyInfo myInfo = (MyInfo)Facade.GetFacade ().data.Get (FacadeConfig.USERINFO_MODULE_ID);
				RoomInfo roomInfo = (RoomInfo)Facade.GetFacade ().data.Get (FacadeConfig.ROOMINFO_MODULE_ID);
				myInfo.seatIndex = enterFriendRoom.seatIndex;
				roomInfo.SetRoomInfo (enterFriendRoom.room);
				foreach (FiUserInfo user in enterFriendRoom.others) {
					roomInfo.AddUser (user);
				}
				if (enterFriendRoom.room != null) {
					myInfo.lastGame.level = enterFriendRoom.room.goldType;
				}
			} else {
				UnityEngine.GameObject nTipWindow = UnityEngine.Resources.Load ("Window/WindowTipsThree")as UnityEngine.GameObject;
				UnityEngine.GameObject nTipWindowEntity = UnityEngine.GameObject.Instantiate ( nTipWindow );
				UITipAutoNoMask ClickTips1 = nTipWindowEntity.GetComponent<UITipAutoNoMask> ();
				if (enterFriendRoom.result == 3)
				{
					ClickTips1.tipText.text = "房間號不存在";
				}
				else
				{
					ClickTips1.tipText.text = "房間已滿";
				}
			}
			NotifyToUi ( FiEventType.RECV_ENTER_FRIEND_ROOM_RESPONSE , data );
		}

		private void NotifyToUi( int nType , object data )
		{
			IUiMediator nMediator = Facade.GetFacade ().ui.Get ( FacadeConfig.UI_FISHING_FRIEND_ID );
			if (nMediator != null)
				nMediator.OnRecvData ( nType , data );
		}

		private void RecvPKCreateFriendRoomResponse(object data)
		{
			FiCreateFriendRoomResponse createFriendRoom = (FiCreateFriendRoomResponse)data;
			//UnityEngine.Debug.LogError ( "-------------RecvPKCreateFriendRoomResponse--------------- / " +   createFriendRoom.result );
			if (0 == createFriendRoom.result) {
				MyInfo myInfo = (MyInfo)Facade.GetFacade ().data.Get (FacadeConfig.USERINFO_MODULE_ID);
				myInfo.seatIndex = createFriendRoom.seatIndex;
				if (null != createFriendRoom.room) {
					RoomInfo roomInfo = (RoomInfo)Facade.GetFacade ().data.Get (FacadeConfig.ROOMINFO_MODULE_ID);
					roomInfo.SetRoomInfo (createFriendRoom.room);
				}
			} else {
				UnityEngine.GameObject nTipWindow = UnityEngine.Resources.Load ("Window/WindowTipsThree")as UnityEngine.GameObject;
				UnityEngine.GameObject nTipWindowEntity = UnityEngine.GameObject.Instantiate ( nTipWindow );
				UITipAutoNoMask ClickTips1 = nTipWindowEntity.GetComponent<UITipAutoNoMask> ();
				if (createFriendRoom.result == 8)
				{
					ClickTips1.tipText.text = "創建房間失敗，金幣不足";
				}
				else
				{
					ClickTips1.tipText.text = "創建房間失敗，錯誤碼 :" + createFriendRoom.result;
				}
			}
			NotifyToUi ( FiEventType.RECV_CREATE_FRIEND_ROOM_RESPONSE , data );
		}

	}
}

