using System;

namespace AssemblyCSharp
{
	public class FishingClassicMsgHandle: IMsgHandle
	{
		public FishingClassicMsgHandle ()
		{
		}


		public void OnInit()
		{
			/*EventControl nControl = EventControl.instance ();
			nControl.addEventHandler ( FiEventType.RECV_ROOM_MATCH_RESPONSE ,  RecvRoomMatchResponse );
			nControl.addEventHandler ( FiEventType.RECV_USER_LEAVE_RESPONSE ,  RecvLeaveClassicRoom );
			nControl.addEventHandler ( FiEventType.RECV_OTHER_ENTER_ROOM_INFORM ,  RecvOtherEnterRoom );
			nControl.addEventHandler ( FiEventType.RECV_OTHER_LEAVE_ROOM_INFORM ,  RecvOtherLeaveRoom );

			nControl.addEventHandler ( FiEventType.RECV_LAUNCH_TORPEDO_RESPONSE ,          RecvLaunchTorpedoResponse );
			nControl.addEventHandler ( FiEventType.RECV_OTHER_LAUNCH_TORPEDO_INFORM ,      RecvOtherLaunchTorpedoInform );
			nControl.addEventHandler ( FiEventType.RECV_TORPEDO_EXPLODE_RESPONSE ,         RecvTorpedoExplodeResponse );
			nControl.addEventHandler ( FiEventType.RECV_OTHER_TORPEDO_EXPLODE_INFORM ,     RecvOtherTorpedoExplodeInform );
			nControl.addEventHandler ( FiEventType.RECV_UNLOCK_CANNON_MULTIPLE_RESPONSE ,  RecvUnlockCannonResponse );*/
		}


		public void OnDestroy()
		{
			/*EventControl nControl = EventControl.instance ();
			nControl.removeEventHandler ( FiEventType.RECV_ROOM_MATCH_RESPONSE ,  RecvRoomMatchResponse );
			nControl.removeEventHandler ( FiEventType.RECV_USER_LEAVE_RESPONSE ,  RecvLeaveClassicRoom );
			nControl.removeEventHandler ( FiEventType.RECV_OTHER_ENTER_ROOM_INFORM ,  RecvOtherEnterRoom );
			nControl.removeEventHandler ( FiEventType.RECV_OTHER_LEAVE_ROOM_INFORM ,  RecvOtherLeaveRoom );

			nControl.removeEventHandler ( FiEventType.RECV_LAUNCH_TORPEDO_RESPONSE ,          RecvLaunchTorpedoResponse );
			nControl.removeEventHandler ( FiEventType.RECV_OTHER_LAUNCH_TORPEDO_INFORM ,      RecvOtherLaunchTorpedoInform );
			nControl.removeEventHandler ( FiEventType.RECV_TORPEDO_EXPLODE_RESPONSE ,         RecvTorpedoExplodeResponse );
			nControl.removeEventHandler ( FiEventType.RECV_OTHER_TORPEDO_EXPLODE_INFORM ,     RecvOtherTorpedoExplodeInform );
			nControl.removeEventHandler ( FiEventType.RECV_UNLOCK_CANNON_MULTIPLE_RESPONSE ,  RecvUnlockCannonResponse );*/
		}

		public void SendUnlockCannon( int nMultiple )
		{
			FiUnlockCannonRequest nRequest = new FiUnlockCannonRequest ();
			nRequest.targetMultiple = nMultiple;
			DataControl.GetInstance().PushSocketSnd(FiEventType.SEND_UNLOCK_CANNON_MULTIPLE_REQUEST, nRequest);
		}

		public void SendLaunchTorpedoRequest(int torpedoId, int torpedoType, int x, int y)
		{//发送鱼雷请求

			FiLaunchTorpedoRequest launchTorpedo = new FiLaunchTorpedoRequest ();
			launchTorpedo.position = new Cordinate ();
			launchTorpedo.torpedoId = torpedoId;
			launchTorpedo.torpedoType = torpedoType;
			launchTorpedo.position.x = x;
			launchTorpedo.position.y = y;
			DataControl.GetInstance().PushSocketSnd (FiEventType.SEND_LAUNCH_TORPEDO_REQUEST, launchTorpedo);
		}

		public void SendRoomMatchRequest(int roomType, int roomMultiple)
		{
			FiRoomMatchRequest roomMatchRequest = new FiRoomMatchRequest ();
			roomMatchRequest.enterType = roomType;
			roomMatchRequest.roomMultiple = roomMultiple;

			RoomInfo roomInfo = (RoomInfo)Facade.GetFacade ().data.Get ( FacadeConfig.ROOMINFO_MODULE_ID );
			if( null!=roomInfo )
			{
				roomInfo.roomType = roomType;
				roomInfo.roomMultiple = roomMultiple;
			}
			DataControl.GetInstance().PushSocketSnd (FiEventType.SEND_ROOM_MATCH_REQUEST, roomMatchRequest);
		}

		public void SendLeaveClassicalRoom()
		{//发送自己离开房间
			RoomInfo roomInfo = (RoomInfo)Facade.GetFacade ().data.Get ( FacadeConfig.ROOMINFO_MODULE_ID );
			FiLeaveRoomRequest leaveRoom = new FiLeaveRoomRequest();
			leaveRoom.leaveType = roomInfo.roomType;
			leaveRoom.roomIndex = roomInfo.roomIndex;
			leaveRoom.roomMultiple = roomInfo.roomMultiple;
			DataControl.GetInstance().PushSocketSnd (FiEventType.SEND_USER_LEAVE_REQUEST, leaveRoom);
		}

		public void RecvOtherEnterRoom(object data)
		{//其他用户进房间
			FiOtherEnterRoom info = (FiOtherEnterRoom) data;
			FiUserInfo user = info.user;
			RoomInfo roomInfo =(RoomInfo) Facade.GetFacade ().data.Get ( FacadeConfig.ROOMINFO_MODULE_ID );
			roomInfo.AddUser ( user );
			if(null!=user)
			{
				UIFishingObjects.GetInstance ().CreateCannon (user);
			}
		}

		public void RecvOtherLaunchTorpedoInform(object data)
		{//接收其他玩家发送的鱼雷
			FiOtherLaunchTorpedoInform otherLaunchTorpedo = (FiOtherLaunchTorpedoInform)data;
			UIFishingObjects.GetInstance ().ToOtherLaunchTorpedo (otherLaunchTorpedo);
		}

		public void RecvUnlockCannonResponse( object data )
		{
            return; //废弃
			FiUnlockCannonResponse nResponse = (FiUnlockCannonResponse)data;
			if (nResponse.result == 0) {
				Panel_UnlockMultiples._instance.RcvUnlockInfo (nResponse.currentMaxMultiple, nResponse.rewardGold,nResponse.needDiamond);
            }else{
                UnityEngine.Debug.LogError("UnlockError!result=" + nResponse.result);
            }

		}

		public void RecvTorpedoExplodeResponse(object data)
		{//接收鱼雷爆炸回复
			FiTorpedoExplodeResponse torpedoExplode = (FiTorpedoExplodeResponse)data;
			UIFishingObjects.GetInstance ().ToTorpedoExplode (torpedoExplode);
		}	

		public void RecvOtherTorpedoExplodeInform(object data)
		{//接收其他玩家鱼雷爆炸通知
			FiOtherTorpedoExplodeInform otherTorpedoExplode = (FiOtherTorpedoExplodeInform)data;
			UIFishingObjects.GetInstance ().ToOtherTorpedoExplode (otherTorpedoExplode);
		}

		public void RecvLaunchTorpedoResponse(object data)
		{//接收发送的鱼雷回复 , 没有被处理
			FiLaunchTorpedoResponse launchTorpedo = (FiLaunchTorpedoResponse)data;
//			launchTorpedo.result == 0
//			UIFishingObjects.GetInstance().ToLaunchTorpedo (launchTorpedo);
		}

		public void RecvOtherLeaveRoom(object data)
		{//其他用户离开房间
			FiOtherLeaveRoom info = (FiOtherLeaveRoom) data;
			RoomInfo roomInfo =(RoomInfo) Facade.GetFacade ().data.Get ( FacadeConfig.ROOMINFO_MODULE_ID );
			roomInfo.RemoveUser ( info.userId );
			UIFishingObjects.GetInstance ().RemoveCannon (info.userId);
		}

		public void RecvRoomMatchResponse(object data)
		{//进捕鱼房间

			/*FiRoomMatchResponse roomMatchReply = (FiRoomMatchResponse) data;

			RoomInfo roomInfo =(RoomInfo) Facade.GetFacade ().data.Get ( FacadeConfig.ROOMINFO_MODULE_ID );
			MyInfo   myInfo   =(MyInfo) Facade.GetFacade ().data.Get ( FacadeConfig.USERINFO_MODULE_ID );
			roomInfo.roomIndex = roomMatchReply.roomIndex;

			if(0==roomMatchReply.result)
			{//房间匹配成功
				//AppControl.ToView (AppView.FISHING);
				myInfo.seatIndex = roomMatchReply.seatIndex;
				roomInfo.InitUser (roomMatchReply.userArray);

				UIFishingMsg.GetInstance ().SetFishing (TypeFishing.CLASSIC);
				myInfo.lastGame.type = TypeFishing.CLASSIC;
				myInfo.SetState( MyInfo.STATE_IN_CLASSICROOM );

				FiUserInfo myUserInfo = new FiUserInfo ();
				myUserInfo.userId = myInfo.userID;
				myUserInfo.seatIndex = myInfo.seatIndex;
				myUserInfo.gender = myInfo.sex;
				myUserInfo.nickName = myInfo.nickname;
				myUserInfo.gold = myInfo.gold;
				myUserInfo.diamond = myInfo.diamond;
				myUserInfo.cannonMultiple = myInfo.cannonMultipleMax;
				myUserInfo.avatar = myInfo.avatar;
				myUserInfo.cannonStyle = myInfo.cannonStyle;
				myUserInfo.level = myInfo.level;
				myUserInfo.experience = myInfo.experience;
				myUserInfo.vipLevel = myInfo.levelVip;
				myUserInfo.maxCannonMultiple = myInfo.cannonMultipleMax;

				UIFishingObjects.GetInstance ().AddCannonInfo ( myUserInfo );

				foreach(FiUserInfo user in roomMatchReply.userArray)
				{
					UIFishingObjects.GetInstance ().AddCannonInfo (new FiUserInfo (user));
					//roomInfo.AddUser (new FiUserInfo (user));
				}
				UIFishingObjects.GetInstance ().CreateCannons ();

				//MyInfo myInfo = (MyInfo)Facade.GetFacade ().data.Get ( FacadeConfig.USERINFO_MODULE_ID );
				myInfo.TargetView = AppView.FISHING;
				AppControl.ToView (AppView.LOADING);
			}
			else
			{
				myInfo.SetState( MyInfo.STATE_IN_HALL );
				//然后跳转到大厅界面
				AppControl.ToView (AppView.HALL);
			}*/
		}

		public void RecvLeaveClassicRoom(object data)
		{//接收自己离开房间
			FiLeaveRoomResponse fiLeave = (FiLeaveRoomResponse) data;
			MyInfo   myInfo   =(MyInfo) Facade.GetFacade ().data.Get ( FacadeConfig.USERINFO_MODULE_ID );
			myInfo.SetCannonInfo (null);
		}


	}
}

