using System;

namespace AssemblyCSharp
{
	public class RedPacketMsgHandle : IMsgHandle
	{
		public RedPacketMsgHandle ()
		{
			
		}

		public void OnInit()
		{
			EventControl mEventCtrl = EventControl.instance ();

			mEventCtrl.addEventHandler ( FiEventType.RECV_ENTER_RED_PACKET_ROOM_RESPONSE ,  RcvEnterRedPacketRoomResponse );
			mEventCtrl.addEventHandler ( FiEventType.RECV_OTHER_ENTER_RED_PACKET_ROOM_INFORM ,RcvOtherEnterRedPacketRoom );

			mEventCtrl.addEventHandler ( FiEventType.RECV_LEAVE_RED_PACKET_ROOM_RESPONSE , RcvLeaveRedPacketRoomResponse );
			mEventCtrl.addEventHandler ( FiEventType.RECV_OTHER_LEAVE_RED_PACKET_ROOM_INFORM , RcvOtherLeaveRedPacketRoom);

			mEventCtrl.addEventHandler ( FiEventType.RECV_REDPACKET_INFORM               , RcvGetRedPacktInform );
			mEventCtrl.addEventHandler ( FiEventType.RECV_OPEN_REDPACKET_RESPONSE        , RcvOpenRedPacketResponse );
			mEventCtrl.addEventHandler ( FiEventType.RECV_OTHER_OPEN_RED_PACKET_INFORM   , RcvOtherOpenRedPacketInform );

			mEventCtrl.addEventHandler (FiEventType.RECV_RED_PACKET_COUNTDOWN_INFORM     , RcvRedPacketCountDown );
			mEventCtrl.addEventHandler ( FiEventType.RECV_GET_RED_PACKET_LIST_RESPONSE   , RcvGetRedPacketListResponse );
		}

		public void OnDestroy()
		{
			EventControl mEventCtrl = EventControl.instance ();
			mEventCtrl.removeEventHandler( FiEventType.RECV_ENTER_RED_PACKET_ROOM_RESPONSE ,  RcvEnterRedPacketRoomResponse );
			mEventCtrl.removeEventHandler ( FiEventType.RECV_OTHER_ENTER_RED_PACKET_ROOM_INFORM ,RcvOtherEnterRedPacketRoom );

			mEventCtrl.removeEventHandler ( FiEventType.RECV_LEAVE_RED_PACKET_ROOM_RESPONSE , RcvLeaveRedPacketRoomResponse );
			mEventCtrl.removeEventHandler ( FiEventType.RECV_OTHER_LEAVE_RED_PACKET_ROOM_INFORM , RcvOtherLeaveRedPacketRoom);

			mEventCtrl.removeEventHandler ( FiEventType.RECV_REDPACKET_INFORM               , RcvGetRedPacktInform );
			mEventCtrl.removeEventHandler ( FiEventType.RECV_OPEN_REDPACKET_RESPONSE        , RcvOpenRedPacketResponse );
			mEventCtrl.removeEventHandler ( FiEventType.RECV_OTHER_OPEN_RED_PACKET_INFORM   , RcvOtherOpenRedPacketInform );

			mEventCtrl.removeEventHandler (FiEventType.RECV_RED_PACKET_COUNTDOWN_INFORM     , RcvRedPacketCountDown );
			mEventCtrl.removeEventHandler ( FiEventType.RECV_GET_RED_PACKET_LIST_RESPONSE   , RcvGetRedPacketListResponse );
		}


//		public void AddMsgEventHandle()
//		{
//			
//		}

		public void SendGetRedPacketList()
		{
			DataControl.GetInstance ().PushSocketSnd ( FiEventType.SEND_GET_RED_PACKET_LIST_REQUEST , null );
		}


		public void SendOpenRedPacketRequest( int nPacketId )
		{
			FiOpenRedPacketRequest nRequest = new FiOpenRedPacketRequest ();
			nRequest.packetId = nPacketId;
			DataControl.GetInstance ().PushSocketSnd ( FiEventType.SEND_OPEN_REDPACKET_REQUEST , nRequest );
		}

		public void SendLeaveRedPacketRoomRequest( int nRoomIndex , int nRoomType )
		{
			FiLeaveRedPacketRoomRequest nRequest = new FiLeaveRedPacketRoomRequest ();
			nRequest.roomIndex = nRoomIndex;
			nRequest.leaveType = nRoomType;
			DataControl.GetInstance ().PushSocketSnd ( FiEventType.SEND_LEAVE_RED_PACKET_ROOM_REQUEST , nRequest );
		}

		private void RcvRedPacketCountDown( object data )
		{
			FiRedPacketDistributionCountdown nInform = (FiRedPacketDistributionCountdown)data;
			if (RedPacket_TopInfo._instance!= null)
				RedPacket_TopInfo._instance.SetTime (nInform.countdown);
		}

		//红包场其他玩家打开红包通知
		private void RcvOtherOpenRedPacketInform( object data )
		{
			FiOtherOpenRedPacketInform nInform = (FiOtherOpenRedPacketInform)data;
			if (PrefabManager._instance != null) {
					
				//UIFishingObjects.GetInstance ().cannonManage.GetInfo(nInform.userId).cannon.gunUI.ShowRedPacketEffect ((float)nInform.redPacketTicket);
				PrefabManager._instance.GetGunByUserID(nInform.userId).gunUI.ShowRedPacketEffect((float)nInform.redPacketTicket);
			}
		}

		//获取红包数组通知
		private void RcvGetRedPacketListResponse( object data )
		{
			FiGetRedPacketListResponse nInform = (FiGetRedPacketListResponse)data;

		}

		//收到服务下发的有红包通知
		private void RcvGetRedPacktInform( object data )
		{
			FiRedPacketInform nInform = (FiRedPacketInform)data;
			//RedPacket_TopInfo._instance.ResetProgress ();
			if (nInform.packetId!=0) {
				if(RedPacket_TopInfo._instance.gameObject!=null){
					RedPacket_TopInfo._instance.ShowMovedRedPacket ((int)nInform.packetId);
					RedPacket_TopInfo._instance.ResetProgress ();
				}
			} else {//id=0代表没有红包
				if(HintTextPanel._instance!=null){
					HintTextPanel._instance.SetTextShow ("本輪未獲得紅包獎勵！ \n（當前進度可累計至下一輪） ");
				}
			}
		}

		//打开红包后的反馈
		private void RcvOpenRedPacketResponse( object data )
		{
			FiOpenRedPacketResponse nInform = (FiOpenRedPacketResponse)data;

			if (nInform.result == 0) {
				//RedPacket_Unpack._instance.GetOpenedRedPacketGold ((float)nInform.redPacketTicket,(int)nInform.packetId);
				if(SelectRedpacketPanel._instance.gameObject!=null){
					SelectRedpacketPanel._instance.RecvOpenRedpacket (nInform.redPacketTicket); 
				}
			} else {
				//Tool.LogError ("OpenRedpacketFailed:" + nInform.result);
			}
			Tool.LogError ("RecvOpenRedpacket:" + nInform.redPacketTicket + "," + nInform.result);
		}


		//进入红包场的处理反馈
		private void RcvEnterRedPacketRoomResponse( object data )
		{
			/*FiEnterRedPacketRoomResponse nRedRoomInfo = (FiEnterRedPacketRoomResponse)data;
			RoomInfo roomInfo = DataControl.GetInstance ().GetRoomInfo();
			MyInfo myInfo = DataControl.GetInstance ().GetMyInfo ();
			//UnityEngine.Debug.LogError ("-----------------RcvEnterRedPacketRoomResponse----------------" + myInfo.cannonStyle );

			if (nRedRoomInfo.result == 0) {
				roomInfo.Clear ();

				myInfo.lastGame.type = nRedRoomInfo.roomType;
				UIFishingMsg.GetInstance ().SetFishing (nRedRoomInfo.roomType);

				//DataControl.GetInstance ().GetMyInfo ().SetState ( MyInfo.STATE_ENTER_FISHINGROOM );

				roomInfo.roomIndex = nRedRoomInfo.roomIndex;
				roomInfo.roomType  = nRedRoomInfo.roomType;
				myInfo.seatIndex   = nRedRoomInfo.seatIndex;

				RedPacket_TopInfo.lastGoldCostRest =(int) nRedRoomInfo.roomConsumedGold;

				Tool.LogError ("IMRcvPKEnterRoomResponse my userID:"+myInfo.userID+" seatIndex:"+myInfo.seatIndex);
				foreach(FiUserInfo user in nRedRoomInfo.others)
				{
					Tool.LogError ("IMRcvPKEnterRoomResponse userID:"+user.userId+" seatIndex:"+user.seatIndex);
					roomInfo.AddUser (new FiUserInfo(user));
				}

				UIFishingObjects.GetInstance ().InitCannonInfo ();

				/*FiUserInfo myUserInfo = new FiUserInfo ();
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

				foreach(FiUserInfo user in nRedRoomInfo.others )
				{
					UIFishingObjects.GetInstance ().AddCannonInfo (new FiUserInfo (user));
					//roomInfo.AddUser (new FiUserInfo (user));
				}
				//UIFishingObjects.GetInstance ().CreateCannons ();*/

				//MyInfo myInfo = (MyInfo)Facade.GetFacade ().data.Get ( FacadeConfig.USERINFO_MODULE_ID );
				/*myInfo.TargetView = AppView.FISHING;
				AppControl.ToView (AppView.LOADING);
				//AppControl.ToView (AppView.FISHING);
			} else {
				UnityEngine.Debug.Log ("进入红包场失败");
			}*/
		}

		private void RcvOtherEnterRedPacketRoom( object data )
		{
			FiOtherEnterRedPacketRoomInform nOtherInform = (FiOtherEnterRedPacketRoomInform)data;
			DataControl.GetInstance ().GetRoomInfo ().AddUser ( new FiUserInfo( nOtherInform.other ) );
			UIFishingObjects.GetInstance ().CreateCannon( nOtherInform.other );
		}

		//离开红包场的处理反馈
		private void RcvLeaveRedPacketRoomResponse( object data )
		{
			FiLeaveRedPacketRoomResponse nLeaveResponse = (FiLeaveRedPacketRoomResponse)data;
			//DataControl.GetInstance ().GetMyInfo ().SetState ( MyInfo.STATE_IN_HALL );
			DataControl.GetInstance ().GetRoomInfo ().Clear ();
			UIFishingObjects.GetInstance ().Clear ();
		}


		private void RcvOtherLeaveRedPacketRoom( object data )
		{
			FiOtherLeaveRedPacketRoomInform nInform = (FiOtherLeaveRedPacketRoomInform)data;
			DataControl.GetInstance ().GetRoomInfo ().RemoveUser ( nInform.userId  );
			UIFishingObjects.GetInstance ().RemoveCannon ( nInform.userId );
		}

	}
}

