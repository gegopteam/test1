using UnityEngine;
using System.Collections;
using System.Runtime.InteropServices;
using AssemblyCSharp;
using System.Net;
using System.Net.Sockets;
using System;
using System.Threading;
using System.Text;
using System.IO;
using System.Collections.Generic;
using Google.Protobuf;
using ProtoBuf;


namespace AssemblyCSharp
{

	public class CollectNetInfo
	{
		//private int mRecvHeartBeatCount = 0;
		//private int mSendHeartBeatCount = 0;

		//private int mStartTick = 0;

		bool bRecvedData = false;

		public void OnRecvData ()
		{
			bRecvedData = true;
		}

		public bool isRecvDataInNextClock ()
		{
			bool isRecved = bRecvedData;
			bRecvedData = false;
			return isRecved;
		}

		/*public  void OnRecvResponse()
		{
			mSendHeartBeatCount++;
			ClearState ();//已经收到回复了，网络正常
//			Debug.LogError ( "[ Collect ] OnRecvResponse " + mRecvTick );
		}

		public void SignSendTick()
		{
			mSendHeartBeatCount++;
//			Debug.LogError ( "[ Collect ] SignSendTick " + mSendTick );
		}

		//如果心跳数量不匹配，那么我们立马发送心跳去检测网络情况
		public bool isOutCount()
		{
			if (mRecvHeartBeatCount != mSendHeartBeatCount)
				return true;
			return false;
		}

		private bool bVerifyState = false;

		public void SignNetWorkState()
		{
			bVerifyState = true;
			mStartTick = DateTime.Now.Hour * 60 + DateTime.Now.Second;
		}

		public int GetDuration()
		{
			if( mStartTick != 0 )
				return DateTime.Now.Hour * 60 + DateTime.Now.Second - mStartTick;
			return 0;
		}

		public bool isVerifingNetWork()
		{
			return bVerifyState;
		}

		public void ClearState()
		{
			mSendHeartBeatCount = mSendHeartBeatCount = 0;
			mStartTick = 0;
			bVerifyState = false;
		}*/
	}





	public class FiSocketThread
	{

		private int nHeartBeatCount = 0;

		private ConnectVars mConVars = null;
		//private bool              bConnectClosed    = false;
		private INetSession mSession = null;
		private bool bStoped = false;
		private Thread mThread = null;
		//private OnNetworkListener mNetListner       = null;

		private AutoResetEvent mEventHandler = new AutoResetEvent (false);

		private object mDataMutex = new object ();
		private FiProtoEncoder mEncoder = new FiProtoEncoder ();

		private int mTickCount = 0;
		//private int mLinkState = 0;

		private int mSysTcikCount = 0;

		//	private int mHeartBeatTick = 0;

		private CollectNetInfo mStateCollect = new CollectNetInfo ();

		public  int mUserId = 0;
		private List<DispatchData> mDataArray;
		private NetControl mRefCtrl;

		private bool isLoginSuccess = false;

		public FiSocketThread (NetControl nNetCtrl)
		{
			mRefCtrl = nNetCtrl;	
			mVerifyTimer.AutoReset = true;
			mVerifyTimer.Elapsed += OnTimerEnd;
		}

		public bool isConnected ()
		{
			if (mSession != null) {
				return mSession.isConnected ();
			}
			return false;
		}

		public void Setup ()
		{

		}

		public void setConfig (ConnectVars nValue, OnNetworkListener nListener)
		{
			Debug.Log("--------------setConfig---------------");
			mConVars = nValue;
			mDataArray = new List<DispatchData> ();

			//注：线程中会使用到 mSession 所以先初始化 mSession 然后启动线程
			mSession = SessionFactory.createSession (SessionFactory.PROTO_TCP);
			mSession.setStateListener (nListener);
			nListener.setCollect (mStateCollect);

			mThread = new Thread (new ThreadStart (OnRunning));
			mThread.Start ();
			bStoped = false;

		}

		System.Timers.Timer mVerifyTimer = new System.Timers.Timer (15000);

		void OnTimerEnd (object source, System.Timers.ElapsedEventArgs e)
		{
			bool bRecvedData = mStateCollect.isRecvDataInNextClock ();
			//10-19s 内没能收到服务器数据，那么我们认为socket连接已经断开了，心跳包5s一个，所以不可能出现这种情况
			if (!bRecvedData) {
				//Debug.LogError ("[ socket verify ] do not recv message!!! -------------------------------");
				mVerifyTimer.Stop ();
				mVerifyTimer = null;

				NetControl nNetEvent = mRefCtrl;
				if (nNetEvent != null) {
					nNetEvent.dispatchEvent (FiEventType.CONNECTIONT_CLOSED, new FiNetworkInfo ());
				}
			} else {
				//Debug.LogError ("[ socket verify ] have recv message!!! -------------------------------");
			}
		}


		private void connect ()
		{
			Debug.LogError("------------connect---------------");
			mSession.connect (mConVars);
			mSysTcikCount = DateTime.Now.Hour * 60 + DateTime.Now.Second;
		}

		public void sendMessage (DispatchData nData)
		{
			if (bStoped) {
				Debug.LogError ("------------send message error--------------- socket stoped!!!");
				return;
			}

			//Debug.LogError (  "------------send message id---------------" + nData.type );
			lock (mDataMutex) {
				mDataArray.Add (nData);
			}
			//Debug.LogError (  "------------send message end---------------" + nData.type );
			mEventHandler.Set ();
		}


		private void OnConnect ()
		{
			Debug.Log("----------OnConnect----------");
			isLoginSuccess = false;
			int nCurTick = DateTime.Now.Second + DateTime.Now.Minute * 60;
			int nConnectCount = 0;
			while (!bStoped && null != mSession && !mSession.isConnected ()) {
				//已经连接成功了，那么处理数据线程
				int nNewTick = DateTime.Now.Second + DateTime.Now.Minute * 60;
				if (!mSession.isConnecting ()) {
					connect ();
					Thread.Sleep (100);
				}

				if (mSession.isConnected ())
					return;
				//10s 后链接还没能成功
				if (nNewTick - nCurTick >= 10) {
					NetControl nNetEvent = mRefCtrl;
					if (nNetEvent != null) {
						mRefCtrl.dispatchEvent (FiEventType.CONNECTIONT_CLOSED, new FiNetworkInfo ());
						mRefCtrl.dispatchEvent (FiEventType.CONNECT_TIMEROUT, new FiNetworkInfo (0, "", nConnectCount));
					}
					mSession.close ();
					nCurTick = nNewTick;
				}
			}
		}

		private void OnRunning ()
		{
			Debug.LogError ("--------------socket event OnRunning--------------- 1");

			while (!bStoped) {
				OnConnect ();

				while (!bStoped && null != mSession && mSession.isConnected ()) {
					int nCurrentTick = DateTime.Now.Hour * 60 + DateTime.Now.Second;
					if (nCurrentTick < mSysTcikCount) {
						mSysTcikCount = nCurrentTick;
					}

					if (nCurrentTick - mSysTcikCount >= 5) {
						mSysTcikCount = nCurrentTick;
						//			Debug.Log ( "[ socket thread ] 客户端发送心跳包!!!");
						if (isLoginSuccess)
							doSendHeatBeat ();
					}

					//如果登陆成功，并且发出心跳包和收到心跳包的数据不一样 那么，立马发送心跳，判断网络连接情况
					/*if ( !mStateCollect.isVerifingNetWork () && isLoginSuccess && mStateCollect.isOutCount ()) {
				//		Debug.LogError ( mDataArray.Count  + "===============>" + mStateCollect.isOutCount() );
						doSendHeatBeat ();
						mStateCollect.SignNetWorkState ();
					}

					//正在检测网络状况 ,并且15后还没有收到心跳包，那么认为网络意见中断 (2个心跳周期)
					if ( mStateCollect.isVerifingNetWork () && mStateCollect.GetDuration() >= 10 ) {
						//Debug.LogError ( mDataArray.Count  +"------------------>" + mStateCollect.GetDuration() );
						lock (mDataMutex) {
							mDataArray.Clear ();
						}
						mStateCollect.ClearState ();
						if( mNetListner!= null )
					    	mNetListner.onRecvMessage ( FiEventType.CONNECTIONT_CLOSED , null , 0 ) ;
						Debug.LogError ( "-------------------------net work error ,send connection down message!!!!!"+ mStateCollect.GetDuration() );
						mSession.close ();
						break;
					}*/


//					if (mDataArray.Count == 0) {
//						mEventHandler.WaitOne (1000, false);
//					}
					//Debug.LogError ( "--------------socket event OnRunning2---------------" + mDataArray.Count );
//					Debug.LogError("---Runing---Snd---data---begin---");

					if (mDataArray.Count > 0) {//发送数据
						if (mSession.SendIsOK ()) {
							//Debug.LogError ( "--------------socket event OnRunning locked in---------------" + mDataArray.Count );
							DispatchData nData = null;
							lock (mDataMutex) {
								//Debug.LogError ( "--------------socket event OnRunning locked---------------" + mDataArray.Count );
								nData = mDataArray [0];
								mDataArray.RemoveAt (0);
							}
							//						Debug.LogError ( "--------------socket event OnRunning locked2---------------" + mDataArray.Count );
							processData (nData);
						}
					}
//					Debug.LogError("---Runing---Snd---data---end---");
//					Debug.LogError("---Runing---Rcv---data---begin---");
					mSession.ToRcv ();//接收数据
//					Debug.LogError("---Runing---Rcv---data---begin---");
				}
			}

			//		Debug.LogError ( "--------------connect end---------------" + bConnectCount );
			mSession.close ();
		}

		public void stop ()
		{
			if (null != mThread) { //先关闭线程
				mThread.Abort ();
				mThread = null;
			}
	
			if (null != mSession) {
				mSession.close ();
				mSession = null;
			}

			bStoped = true;
			//mNetListner = null;
			mRefCtrl = null;
		}

		private void processData (DispatchData nDataIn)
		{
			mTickCount++;
			//Debug.LogError( FiEventType.SEND_LOGIN_REQUEST + "/" +  "-------------------------nDataIn.type****************" + nDataIn.type + " / " + nDataIn.data );

			switch (nDataIn.type) {

			case FiEventType.START_HEART_BEAT:
				isLoginSuccess = true;
				mVerifyTimer.Start ();
				break;


			//send HeartBeat
			case -1:
				{
					//mStateCollect.SignSendTick ();
					SendByteArray (0, null);
					//		Debug.LogError ("【 send 】-------nHeartBeatCount--------" + (nHeartBeatCount ++));
				}
				break;

			case FiEventType.SEND_LOGIN_REQUEST:
				{
					byte[] nByteBody = FiProtoHelper.toProto_LoginRequest ((FiLoginRequest)nDataIn.data).ToByteArray ();
					//Debug.LogError ( "------------------" + FiProtoHelper.toProto_LoginRequest ( (  FiLoginRequest) nDataIn.data ) );
					SendByteArray (FiProtoType.FISHING_LOGIN_REQ, nByteBody);
				}
				break;

			case FiEventType.SEND_ROOM_MATCH_REQUEST:
				{
					//			Debug.Log ("****************SEND_ROOM_MATCH_REQUEST***********************");
					byte[] nByteBody = FiProtoHelper.toProto_MatchRequest ((FiRoomMatchRequest)nDataIn.data).ToByteArray ();
					SendByteArray (FiProtoType.FISHING_ENTER_CLASSICAL_ROOM_REQ, nByteBody);
				}
				break;





			case FiEventType.SEND_USER_LEAVE_REQUEST:
				{
					byte[] nByteBody = FiProtoHelper.toProto_UserLeaveReq ((FiLeaveRoomRequest)nDataIn.data).ToByteArray ();
					SendByteArray (FiProtoType.FISHING_LEAVE_CLASSICAL_ROOM_REQ, nByteBody, mUserId);
				}
				break;





			case FiEventType.SEND_FIRE_BULLET_REQUEST:
				{
					FiFireBulletRequest nRequest = (FiFireBulletRequest)nDataIn.data;
					byte[] nByteBody = FiProtoHelper.toProto_UserFireBullet (nRequest).ToByteArray ();
					SendByteArray (FiProtoType.FISHING_NOTIFY_FIRE, nByteBody, nRequest.userId);
                        //Debug.Log(string.Format("子弹id:{0}", nRequest.bulletId));
				}
				break;





			case FiEventType.SEND_HITTED_FISH_REQUEST:
				{
					FiHitFishRequest nRequest = (FiHitFishRequest)nDataIn.data;
					byte[] nByteBody = FiProtoHelper.toProto_HitFishRequest (nRequest).ToByteArray ();
					SendByteArray (FiProtoType.FISHING_NOTIFY_ON_FISH_HIT, nByteBody, nRequest.userId);
				}
				break;




			case FiEventType.SEND_CHANGE_CANNON_REQUEST:
				{
					//Debug.LogError ( "SEND_CHANGE_CANNON_REQUEST" );
					FiChangeCannonMultipleRequest nRequest = (FiChangeCannonMultipleRequest)nDataIn.data;
					byte[] nByteBody = FiProtoHelper.toProto_CannonChangeRequest (nRequest).ToByteArray ();
					SendByteArray (FiProtoType.FISHING_CHANGE_CANNON_MULTIPLE_REQUEST, nByteBody, nRequest.userId);
				}
				break;

			//发送改变炮样式请求
			case FiEventType.SEND_CHANGE_CANNON_STYLE_REQUEST:
				{
					//Debug.LogError ( "SEND_CHANGE_CANNON_REQUEST" );
					FiChangeCannonStyleRequest nRequest = (FiChangeCannonStyleRequest)nDataIn.data;
					//FiChangeCannonMultipleRequest nRequest = 
					byte[] nByteBody = nRequest.serialize ();
					SendByteArray (FiEventType.SEND_CHANGE_CANNON_STYLE_REQUEST, nByteBody, nRequest.userId);
				}
				break;
			case FiEventType.SEND_FISH_OUT_REQUEST:
				{
					byte[] nByteBody = FiProtoHelper.toProto_FishOutScene ((FiFishsOutRequest)nDataIn.data).ToByteArray ();
					SendByteArray (FiProtoType.FISHING_NOTIFY_FISH_OUT_OF_SCENE, nByteBody);
				}
				break;





			case FiEventType.SEND_USE_EFFECT_REQUEST:
				{
					
					FiEffectRequest nReuqest = (FiEffectRequest)nDataIn.data;
					byte[] nByteBody = FiProtoHelper.toProto_EffectRequest (nReuqest).ToByteArray ();
					Debug.Log("Joey Test 發送冰凍訊息 = " + FiProtoType.FISHING_EFFECT_REQUEST + " ;nByteBody = " + nByteBody + " ;nReuqest.userId = " + nReuqest.userId);
					SendByteArray (FiProtoType.FISHING_EFFECT_REQUEST, nByteBody, nReuqest.userId);
				}
				break;





			case FiEventType.SEND_TOPUP_REQUEST:
				{
					byte[] nByteBody = FiProtoHelper.toProto_TopupRequest ((FiTopUpRequest)nDataIn.data).ToByteArray ();
					SendByteArray (FiProtoType.FISHING_TOP_UP_REQUEST, nByteBody);
				}
				break;





			case FiEventType.SEND_BACKPACK_REQUEST:
				{
					SendByteArray (FiProtoType.FISHING_GET_BACKPACK_PROPERTY_REQUEST, null);
				}
				break;



			case FiEventType.SEND_START_PK_GAME_REQUEST:
				{
					byte[] nByteBody = FiProtoHelper.toProto_StartPkGame ((FiStartPKGameRequest)nDataIn.data).ToByteArray ();
					SendByteArray (FiProtoType.FISHING_START_PKGAME_REQUEST, nByteBody);
				}
				break;





			case FiEventType.SEND_LEAVE_PK_ROOM_REQUEST:
				{
					byte[] nByteBody = FiProtoHelper.toProto_LeavePkGame ((FiLeavePKRoomRequest)nDataIn.data).ToByteArray ();
					SendByteArray (FiProtoType.FISHING_LEAVE_PKROOM_REQUEST, nByteBody);
				}
				break;




			case FiEventType.SEND_ENTER_PK_ROOM_REQUEST:
				{
					byte[] nByteBody = FiProtoHelper.toProto_EnterPkGame ((FiEnterPKRoomRequest)nDataIn.data).ToByteArray ();
					SendByteArray (FiProtoType.FISHING_ENTER_PKROOM_REQUEST, nByteBody);
				}
				break;





			case FiEventType.SEND_PREPARE_PKGAME_REQUEST:
				{
					FiPreparePKGame nValue = (FiPreparePKGame)nDataIn.data;
					PB_PreparePKGame nRequest = new PB_PreparePKGame ();
					nRequest.RoomIndex = nValue.roomIndex;
					nRequest.RoomType = nValue.roomType;
					nRequest.UserId = nValue.userId;
					byte[] nByteBody = nRequest.ToByteArray ();
					SendByteArray (FiProtoType.FISHING_PREPARE_PKGAME, nByteBody);
				}
				break;





			case FiEventType.SEND_CANCEL_PREPARE_PKGAME:
				{
					FiCancelPreparePKGame nValue = (FiCancelPreparePKGame)nDataIn.data;

					PB_CancelPreparePKGame nRequest = new PB_CancelPreparePKGame ();
					nRequest.RoomIndex = nValue.roomIndex;
					nRequest.RoomType = nValue.roomType;
					nRequest.UserId = nValue.userId;
					byte[] nByteBody = nRequest.ToByteArray ();

					SendByteArray (FiProtoType.FISHING_CANCEL_PREPARE_PKGAME, nByteBody);
				}
				break;




			//发送PK场的特效请求
			case FiEventType.SEND_PK_USE_EFFECT_REQUEST:
				{
					FiEffectRequest nPkRequest = (FiEffectRequest)nDataIn.data;

					PB_EffectRequest nPbPkRequest = new PB_EffectRequest ();
					nPbPkRequest.UserId = nPkRequest.userId;
					nPbPkRequest.Effect = new PB_Effect ();

					if (nPkRequest.effect != null)
						nPbPkRequest.Effect.Id = nPkRequest.effect.type;
					byte[] nByteBody = nPbPkRequest.ToByteArray ();
					SendByteArray (FiProtoType.FISHING_PK_EFFECT_REQUEST, nByteBody);
				}
				break;




			//普通场发射鱼雷消息
			case FiEventType.SEND_LAUNCH_TORPEDO_REQUEST:
				{

					//		Tool.addExMessage ("SEND_LAUNCH_TORPEDO_REQUEST start");

					FiLaunchTorpedoRequest nTorRequest = (FiLaunchTorpedoRequest)nDataIn.data;

					//Debug.LogError ("--------------------------- SEND_LAUNCH_TORPEDO_REQUEST ");

					PB_LaunchTorpedoRequest nPBTorRequest = new PB_LaunchTorpedoRequest ();
					nPBTorRequest.TorpedoId = nTorRequest.torpedoId;
					nPBTorRequest.TorpedoType = nTorRequest.torpedoType;
					nPBTorRequest.Position = new BulletPosition ();
					nPBTorRequest.Position.X = nTorRequest.position.x;
					nPBTorRequest.Position.Y = nTorRequest.position.y;

					byte[] nByteBody = nPBTorRequest.ToByteArray ();
					SendByteArray (FiProtoType.FISHING_LAUNCH_TORPEDO_REQUEST, nByteBody);

					//				Tool.addExMessage ("SEND_LAUNCH_TORPEDO_REQUEST end");
				}
				break;



			//PK场发射鱼雷消息
			case FiEventType.SEND_PK_LAUNCH_TORPEDO_REQUEST:
				{
					FiLaunchTorpedoRequest nTorRequest = (FiLaunchTorpedoRequest)nDataIn.data;

					PB_LaunchTorpedoRequest nPBTorRequest = new PB_LaunchTorpedoRequest ();
					nPBTorRequest.TorpedoId = nTorRequest.torpedoId;
					nPBTorRequest.TorpedoType = nTorRequest.torpedoType;
					nPBTorRequest.Position = new BulletPosition ();
					nPBTorRequest.Position.X = nTorRequest.position.x;
					nPBTorRequest.Position.Y = nTorRequest.position.y;

					byte[] nByteBody = nPBTorRequest.ToByteArray ();
					SendByteArray (FiProtoType.FISHING_PK_LAUNCH_TORPEDO_REQUEST, nByteBody);

				}
				break;



			//鱼雷爆炸消息
			case FiEventType.SEND_TORPEDO_EXPLODE_REQUEST:
				{
					//			Tool.addExMessage ("SEND_TORPEDO_EXPLODE_REQUEST start!");


					FiTorpedoExplodeRequest nExpRequest = (FiTorpedoExplodeRequest)nDataIn.data;

					PB_TorpedoExplodeRequest nTorExpReq = new PB_TorpedoExplodeRequest ();
					nTorExpReq.TorpedoId = nExpRequest.torpedoId;
					nTorExpReq.TorpedoType = nExpRequest.torpedoType;

					for (int i = 0; i < nExpRequest.fishes.Count; i++) {
						PB_Fish nFiValue = new PB_Fish ();
						nFiValue.FishId = nExpRequest.fishes [i].fishId;
						nFiValue.GroupId = nExpRequest.fishes [i].groupId;
						nTorExpReq.TargetFishes.Add (nFiValue);
					}

					//	Debug.Log ( "==" + nTorExpReq );
					byte[] nByteBody = nTorExpReq.ToByteArray ();
					SendByteArray (FiProtoType.FISHING_TORPEDO_EXPLODE_REQUEST, nByteBody);

					//			Tool.addExMessage ("SEND_TORPEDO_EXPLODE_REQUEST end!");
				}
				break;






			//PK场鱼雷爆炸消息
			case FiEventType.SEND_PK_TORPEDO_EXPLODE_REQUEST:
				{
					FiTorpedoExplodeRequest nExpRequest = (FiTorpedoExplodeRequest)nDataIn.data;

					PB_TorpedoExplodeRequest nTorExpReq = new PB_TorpedoExplodeRequest ();
					nTorExpReq.TorpedoId = nExpRequest.torpedoId;
					nTorExpReq.TorpedoType = nExpRequest.torpedoType;
					//		nTorExpReq.TargetFishes = new Google.Protobuf.Collections.RepeatedField<PB_Fish> ();

					for (int i = 0; i < nExpRequest.fishes.Count; i++) {
						PB_Fish nFiValue = new PB_Fish ();
						nFiValue.FishId = nExpRequest.fishes [i].fishId;
						nFiValue.GroupId = nExpRequest.fishes [i].groupId;
						nTorExpReq.TargetFishes.Add (nFiValue);
					}

					byte[] nByteBody = nTorExpReq.ToByteArray ();
					SendByteArray (FiProtoType.FISHING_PK_TORPEDO_EXPLODE_REQUEST, nByteBody);
				}
				break;




			case  FiEventType.SEND_RECONNECT_GAME_REQUEST:
				{
					SendByteArray (FiProtoType.FISHING_RECONNECT_GAME_REQUEST, null);
				}
				break;




			case FiEventType.SEND_CREATE_FRIEND_ROOM_REQUEST:
				{
					FiCreateFriendRoomRequest nFriendRoom = (FiCreateFriendRoomRequest)nDataIn.data;
					PB_CreateFriendRoomRequest nRoomRequest = new PB_CreateFriendRoomRequest ();
					nRoomRequest.GoldType = nFriendRoom.goldType;
					nRoomRequest.RoomType = nFriendRoom.roomType;
					nRoomRequest.RoundType = nFriendRoom.roundType;
					nRoomRequest.TimeType = nFriendRoom.timeType;


					byte[] nByteBody = nRoomRequest.ToByteArray ();
					SendByteArray (FiProtoType.FISHING_CREATE_FRIEND_ROOM_REQUEST, nByteBody);
				}
				break;



			case FiEventType.SEND_ENTER_FRIEND_ROOM_REQUEST:
				{
					FiEnterFriendRoomRequest nValue = (FiEnterFriendRoomRequest)nDataIn.data;
					PB_EnterFriendRoomRequest nPbValue = new PB_EnterFriendRoomRequest ();
					nPbValue.RoomIndex = nValue.roomIndex;

					byte[] nByteBody = nPbValue.ToByteArray ();
					SendByteArray (FiProtoType.FISHING_ENTER_FRIEND_ROOM_REQUEST, nByteBody);
				}
				break;


			case FiEventType.SEND_LEAVE_FRIEND_ROOM_REQUEST:
				{
					FiLeaveFriendRoomRequest nLeaveIn = (FiLeaveFriendRoomRequest)nDataIn.data;
					PB_LeaveFriendRoomRequest nLeavePb = new PB_LeaveFriendRoomRequest ();
					nLeavePb.RoomIndex = nLeaveIn.roomIndex;
					nLeavePb.RoomType = nLeaveIn.roomType;

					byte[] nByteBody = nLeavePb.ToByteArray ();
					SendByteArray (FiProtoType.FISHING_LEAVE_FRIEND_ROOM_REQUEST, nByteBody);
				}
				break;



			case FiEventType.SEND_DISBAND_FRIEND_ROOM_REQUEST:
				{
					FiDisbandFriendRoomRequest nDisbandIn = (FiDisbandFriendRoomRequest)nDataIn.data;
					PB_DisbandFriendRoomRequest nDisbandPB = new PB_DisbandFriendRoomRequest ();
					nDisbandPB.RoomIndex = nDisbandIn.roomIndex;
					nDisbandPB.RoomType = nDisbandIn.roomType;

					byte[] nByteBody = nDisbandPB.ToByteArray ();
					SendByteArray (FiProtoType.FISHING_DISBAND_FRIEND_ROOM_REQUEST, nByteBody);
				}
				break;





			case FiEventType.SEND_OPEN_REDPACKET_REQUEST:
				{
					FiOpenRedPacketRequest nRequest = (FiOpenRedPacketRequest)nDataIn.data;
					PB_OpenRedPacketRequest nPbRequest = new PB_OpenRedPacketRequest ();

					nPbRequest.PacketId = nRequest.packetId;

					byte[] nByteBody = nPbRequest.ToByteArray ();
					SendByteArray (FiProtoType.FISHING_OPEN_RED_PACKET_REQUEST, nByteBody);
				}
				break;





			case FiEventType.SEND_ENTER_RED_PACKET_ROOM_REQUEST:
				{
					FiEnterRedPacketRoomRequest nRequestIn = (FiEnterRedPacketRoomRequest)nDataIn.data;
					PB_EnterRedPacketRoomRequest nRequestOut = new PB_EnterRedPacketRoomRequest ();
					nRequestOut.RoomType = nRequestIn.roomType;

					byte[] nByteBody = nRequestOut.ToByteArray ();
					SendByteArray (FiProtoType.FISHING_ENTER_RED_PACKET_ROOM_REQUEST, nByteBody);
				}
				break;






			case FiEventType.SEND_LEAVE_RED_PACKET_ROOM_REQUEST:
				{
					FiLeaveRedPacketRoomRequest nRequestIn = (FiLeaveRedPacketRoomRequest)nDataIn.data;
					PB_LeaveRedPacketRoomRequest nRequestOut = new PB_LeaveRedPacketRoomRequest ();
					nRequestOut.LeaveType = nRequestIn.leaveType;
					nRequestOut.RoomIndex = nRequestIn.roomIndex;

					byte[] nByteBody = nRequestOut.ToByteArray ();
					SendByteArray (FiProtoType.FISHING_LEAVE_RED_PACKET_ROOM_REQUEST, nByteBody);
				}
				break;
			
			


			
			case FiEventType.SEND_UNLOCK_CANNON_MULTIPLE_REQUEST:
				{
					FiUnlockCannonRequest nValue = (FiUnlockCannonRequest)nDataIn.data;
					PB_UnlockCannonMultipleRequest nRequestOut = new PB_UnlockCannonMultipleRequest ();
					nRequestOut.TargetMultiple = nValue.targetMultiple;

					byte[] nByteBody = nRequestOut.ToByteArray ();
					SendByteArray (FiProtoType.FISHING_UNLOCK_CANNON_MULTIPLE_REQUEST, nByteBody);
				}
				break;


			case FiEventType.SEND_GET_RANK_REQUEST:
				{
					FiGetRankRequest nRequestIn = (FiGetRankRequest)nDataIn.data;
					PB_GameRankRequest nRequestOut = new PB_GameRankRequest ();
					nRequestOut.Type = nRequestIn.type;

					byte[] nByteBody = nRequestOut.ToByteArray ();
					SendByteArray (FiProtoType.FISHING_GET_RANK_REQUEST, nByteBody);
				}
				break;

			case FiEventType.SEND_CONVERSION_REQUEST:
				{
					FiConvertFormalAccount nRequestIn = (FiConvertFormalAccount)nDataIn.data;
					PB_ConvertFormalAccount nRequestOut = new PB_ConvertFormalAccount ();
					nRequestOut.UserID = nRequestIn.userID;
					nRequestOut.Code = nRequestIn.code;
					nRequestOut.Mobile = nRequestIn.mobile;
					nRequestOut.Pwd = nRequestIn.pwd;
					nRequestOut.Token = nRequestIn.token;
					nRequestOut.PropID = nRequestIn.propID;
					nRequestOut.PropCount = nRequestIn.propCount;
					byte[] nByteBody = nRequestOut.ToByteArray ();
					SendByteArray (FiProtoType.YK_CONVERSION_REQUEST, nByteBody);
				}
				break;

            //綁定手機
			case FiEventType.XL_GET_BIND_PHONE_REQUEST:
				{
					FiConvertFormalBindAccount nRequestIn = (FiConvertFormalBindAccount)nDataIn.data;
					PB_ConvertFormalBindAccount nRequestOut = new PB_ConvertFormalBindAccount();
					nRequestOut.Result = nRequestIn.result;
					nRequestOut.UserID = nRequestIn.userID;
					nRequestOut.StrPhoneNum = nRequestIn.phone;
					nRequestOut.StrCode = nRequestIn.code;
					Debug.Log("@@@ nRequestOut.StrCode = "+ nRequestOut.StrCode);
					Debug.Log("@@@ nRequestIn.code = " + nRequestIn.code);
					byte[] nByteBody = nRequestOut.ToByteArray();
					SendByteArray(FiProtoType.XL_SEND_BIND_PHONE, nByteBody);
				}
				break;

			case FiEventType.SEND_CL_MODIFY_NICK_REQUEST:
				{
					FiModifyNick nRequestIn = (FiModifyNick)nDataIn.data;
					PB_ModifyNick nRequestOut = new PB_ModifyNick ();
					nRequestOut.UserID = nRequestIn.userID;
					nRequestOut.LoginType = nRequestIn.loginType;
					nRequestOut.PropID = nRequestIn.propID;
					nRequestOut.PropCount = nRequestIn.propCount;
					nRequestOut.ModifyNick = nRequestIn.modifyNick;
					byte[] nByteBody = nRequestOut.ToByteArray ();
					SendByteArray (FiProtoType.CL_MODIFY_NICK_REQUEST, nByteBody);
				}
				break;

			case FiEventType.SEND_CL_GET_HELP_TASK_REWARD_REQUEST:
				{
					FiGetHelpGodlReward nRequestIn = (FiGetHelpGodlReward)nDataIn.data;
					PB_GetHelpGodlReward nRequestOut = new PB_GetHelpGodlReward ();
					nRequestOut.UserID = nRequestIn.userID;
					nRequestOut.TaskID = nRequestIn.taskID;
					nRequestOut.PropID = nRequestIn.propID;
					nRequestOut.Count = nRequestIn.count;
					byte[] nByteBody = nRequestOut.ToByteArray ();
					SendByteArray (FiProtoType.CL_GET_HELP_TASK_REWARD_REQUEST, nByteBody);
				}
				break;
			case FiEventType.SEND_CL_SIGNRETROACTIVE_REQUEST:
				{
					FiRetroactive nRequestIn = (FiRetroactive)nDataIn.data;
					PB_RetroactiveReques nRequestOut = new PB_RetroactiveReques ();
					nRequestOut.UserID = nRequestIn.userID;
					nRequestOut.PropID = nRequestIn.propID;
					nRequestOut.ReDay = nRequestIn.reday;
					nRequestOut.PropCount = nRequestIn.count;
					nRequestOut.RetroactiveType = nRequestIn.retRoactivetype;
					byte[] nByteBody = nRequestOut.ToByteArray ();
					SendByteArray (FiProtoType.CL_SIGNRETROACTIVE_REQUEST, nByteBody);
				}
				break;
			//使用道具请求
			case FiEventType.SEND_CL_USE_PROP_TIMEEX_REQUEST:
				{
					FiUseProTimeRequest nRequestIn = (FiUseProTimeRequest)nDataIn.data;
					UsePropTimeEx nRequestOut = new UsePropTimeEx ();
					nRequestOut.UserID = nRequestIn.userID;
					nRequestOut.ResultCode = nRequestIn.resultCode;
					nRequestOut.UseProp = new usePropTime ();
					//nRequestOut.UseProp = nRequestIn.useProp;
					if (nRequestIn.useProp != null) {
						nRequestOut.UseProp.PropID = nRequestIn.useProp.propID;
						nRequestOut.UseProp.PropType = nRequestIn.useProp.propType;
						nRequestOut.UseProp.UseTime = nRequestIn.useProp.useTime;
						nRequestOut.UseProp.RemainTime = nRequestIn.useProp.remainTime;
						nRequestOut.UseProp.NPropTime = nRequestIn.useProp.propTime;
//						Debug.Log ("SEND_CL_USE_PROP_TIMEEX_REQUEST nRequestOut.UseProp.propType = " + nRequestOut.UseProp.PropType);
//						Debug.Log ("SEND_CL_USE_PROP_TIMEEX_REQUEST nRequestOut.UseProp.PropID = " + nRequestOut.UseProp.PropID);
//						Debug.Log ("SEND_CL_USE_PROP_TIMEEX_REQUEST nRequestOut.UseProp.useTime = " + nRequestOut.UseProp.UseTime);
//						Debug.Log ("SEND_CL_USE_PROP_TIMEEX_REQUEST nRequestOut.UseProp.remainTime = " + nRequestOut.UseProp.RemainTime);
					}
					byte[] nByteBody = nRequestOut.ToByteArray ();
					SendByteArray (FiProtoType.CL_USE_PROP_TIMEEX_REQUEST, nByteBody);
				}
				break;

			case FiEventType.SEND_CL_USE_DARGON_CARD_REQUEST:
				{
					SendByteArray (FiProtoType.CL_DARGON_CARD_REQUEST, null);
				}
				break;
			case FiEventType.SEND_CL_PREFERENTIAL_MSG_REQUEST:
				{
					SendByteArray (FiProtoType.CL_GET_PREFERENTIAL_REQUEST, null);
				}
				break;
			//发送兑换炮座请求
			case FiEventType.SEND_XL_EXCHANGEBARBETTE_REQUEST:
				{
					FiExchangeBarbette nRequestIn = (FiExchangeBarbette)nDataIn.data;
					useBuyCannonBottomInfo nRequestOut = new useBuyCannonBottomInfo ();
					nRequestOut.BuyType = nRequestIn.buyType;
					byte[] nByteBody = nRequestOut.ToByteArray ();
					SendByteArray (FiProtoType.XL_GET_EXCHANGEBARBETTE_REQUEST, nByteBody);
				}
				break;
			//发送改变炮座请求
			case FiEventType.SEND_XL_EQUIPMENTBARBETTE_REQUEST:
				{
					FiChangeBarbetteStyle nRequestIn = (FiChangeBarbetteStyle)nDataIn.data;
					EquipmentCannonBottom nRequestOut = new EquipmentCannonBottom ();
					nRequestOut.EquipmentType = nRequestIn.EquipmentType;
					nRequestOut.PropID = nRequestIn.propID;
					nRequestOut.RemoveCannonBottom = nRequestIn.removeChangeBarbetteStyle;
					byte[] nByteBody = nRequestOut.ToByteArray ();
					SendByteArray (FiProtoType.XL_CHANGEBARBETTESTYLE_REQUEST, nByteBody);
				}
				break;
			case FiEventType.SEND_XL_ROBOTREPLICATION_REQUEST:
				{
					FiChangeRobotReplicationFishID requestIn = (FiChangeRobotReplicationFishID)nDataIn.data;
					Change_FenShenFish_ID requestOut = new Change_FenShenFish_ID ();
					requestOut.Id = requestIn.robotFishID;
//					Debug.LogError ("requestOut.Id = " + requestOut.Id);
					for (int i = 0; i < requestIn.target.Count; i++) {
						PB_Property property = new PB_Property ();
						property.PropertyType = requestIn.target [i].type;
//						Debug.LogError ("requestOut.property.PropertyType = " + property.PropertyType);

						property.Sum = requestIn.target [i].value;
//						Debug.LogError ("requestOut.property.Sum = " + property.Sum);

						requestOut.Target.Add (property);
					}
					byte[] nByteBody = requestOut.ToByteArray ();
					SendByteArray (FiProtoType.XL_ROBOTREPLICATION_REQUEST, nByteBody);
				}
				break;
			case  FiEventType.SEND_XL_MANMONSETTING_REQUEST:
				{
					SendByteArray (FiProtoType.XL_MANMONSETTING_REQUEST, null);
				}
				break;
			case FiEventType.SEND_XL_MANMONETTING_REQUEST:
				{
					//Debug.LogError ( "SEND_CHANGE_CANNON_REQUEST" );
					FiFishGoldShow nRequest = (FiFishGoldShow)nDataIn.data;
					ChipJettorGold nRequestOut = new ChipJettorGold ();
					nRequestOut.ChipIndex = nRequest.chipIndex;
					byte[] nByteBody = nRequestOut.ToByteArray ();
					SendByteArray (FiProtoType.XL_MANMONBETTING_REQUEST, nByteBody);
				}
				break;
			case FiEventType.SEND_XL_MANMONYAOQIANSHU_REQUEST:
				{
					//Debug.LogError ( "SEND_CHANGE_CANNON_REQUEST" );
					FiFishYGoldShow nRequest = (FiFishYGoldShow)nDataIn.data;
					ChipJettorGold nRequestOut = new ChipJettorGold ();
					nRequestOut.ChipIndex = nRequest.chipIndex;
					byte[] nByteBody = nRequestOut.ToByteArray ();
					SendByteArray (FiProtoType.XL_MANMONYAOQIANSHU_REQUEST, nByteBody);
				}
				break;
			case FiEventType.SEND_XL_MANMONRANKREWARD_REQUST:
				{
//					FiFishGetCoinPool nRequest = (FiFishGetCoinPool)nDataIn.data;
//					GetLongRewardPoolCount nRequsetout = new GetLongRewardPoolCount ();
//					byte[] nByteBody = nRequsetout.ToByteArray ();
//					SendByteArray (FiProtoType.XL_SENDLONG_POOL_REWARD_REQUEST, nByteBody);
					SendByteArray (FiProtoType.XL_SENMANMON_RANKREWARD_REQUEST, null);
				}
				break;
			//发送获取奖池请求
			case FiEventType.SEND_XL_TURNTABLEGETPOOL_REQUST:
				{
					FiFishGetCoinPool requestIn = (FiFishGetCoinPool)nDataIn.data;
					GetLongRewardPoolCount requestOut = new GetLongRewardPoolCount ();
					requestOut.NType = requestIn.type;
					byte[] nByteBody = requestOut.ToByteArray ();
					SendByteArray (FiProtoType.XL_SENDLONG_POOL_REWARD_REQUEST, nByteBody);
				}
				break;
			case FiEventType.SEND_XL_TURNTABLELUCKDRAW_REQUST:
				{
					//FishTurnTableLuckyDraw requesIn = (FishTurnTableLuckyDraw)nDataIn.data;
					//GetFishLuckyDrawResponse requseOut = new GetFishLuckyDrawResponse();
					//requseOut.Type = requesIn.type;
					//byte[] nByteBody = requseOut.ToByteArray();
					SendByteArray (FiEventType.SEND_FISH_LUCKY_DRAW_REQUEST, null);
				}
				break;
			case FiEventType.SEND_XL_CHANGELIUSHUITIME_REQUST:
				{
					SendByteArray (FiProtoType.XL_SENDCHANGE_LIU_SHUI_TIME, null);
				}
				break;
			case FiEventType.SEND_XL_CANCELOTHERSKILL_REQUST:
				{
					InformOtherCancelSKill otherCancelSKillIn = (InformOtherCancelSKill)nDataIn.data;
					CancelSkill cancelSkillOut = new CancelSkill ();
					cancelSkillOut.SkillID = otherCancelSKillIn.skillID;
					cancelSkillOut.LUserID = otherCancelSKillIn.lUserID;
					byte[] nByteBody = cancelSkillOut.ToByteArray ();
					SendByteArray (FiProtoType.XL_SENDCANCEL_OTHER_SKILL, nByteBody);
					//Debug.LogError("发送取消技能"+otherCancelSKillIn.skillID);
				}
				break;
			case FiEventType.SEND_XL_UPDATERANKINFO_REQUST:
				{
					SendByteArray (FiProtoType.SEND_FISHING_UPDATE_WINER_RANK_INFO, null);
				}
				break;
			case FiEventType.SEND_XL_WINTIMECOUNTFO_RESQUSET:
				{
					SendByteArray (FiProtoType.XL_SEND_MANMONEXIT, null);
				}
				break;
			case FiEventType.SEND_XL_WINTIMECOUN_RESQUSET:
				{
					SendByteArray (FiProtoType.XL_SEND_MANMONWINTIMECOUT, null);
				}
				break;
			case FiEventType.SEND_XL_NOTIFYSIGNUP_RESQUSET:
				{
					FiNotifySignUp signUp = (FiNotifySignUp)nDataIn.data;
					NotifySignUp _signUp = new NotifySignUp ();
					_signUp.NType = signUp.type;
//					_signUp.NGameType = signUp.gameType;
//					_signUp.NRoomIndex = signUp.roomIndex;
//					_signUp.SignUpGold = signUp.signUpGold;
					byte[] nByte = _signUp.ToByteArray ();
//					Debug.LogError ("_signUp.NType = " + _signUp.NType);
					SendByteArray (FiProtoType.XL_BOSSROOMSIGNUP_REQUEST, nByte);
				}
				break;
			case FiEventType.SEND_XL_BOSSKILLRANK_RESQUSET:
				{
					SendByteArray (FiProtoType.XL_SEND_BOSS_KILL_RANK, null);
					break;
				}
			case FiEventType.SEND_XL_BOSSMATCHTIME_RESQUSET:
				{
					SendByteArray (FiProtoType.XL_UPDATEBOSSMATCHTIME_REQUEST, null);
					break;
				}
			case FiEventType.SEND_XL_HORODATA_RESQUSET:
				{
					SendByteArray (FiProtoType.XL_API_WEI_SAI_RANK_REQUEST, null);
					break;
				}
			case FiEventType.SEND_XL_RONGYURANK_RESQUSET:
				{
					SendByteArray (FiProtoType.XL_RONGYUDIANTANG_RANK_REQUEST, null);
				}
				break;
			case FiEventType.SEND_XL_PAIWEIPRIZEINFO_RESQUSET:
				{
					Debug.LogError ("发送" + FiProtoType.XL_SEND_PAI_WEI_REWARD_INFO_REQUEST);
					//PaiWeiPrizeInfo paiWeiPrizeInfo = (PaiWeiPrizeInfo)nDataIn.data;
					//GetPaiWeiSaiReward reward = new GetPaiWeiSaiReward();
					//reward.RewardIndex = paiWeiPrizeInfo.rewardIndex;
					//byte[] nByteBody = reward.ToByteArray();
					SendByteArray (FiProtoType.XL_SEND_PAI_WEI_REWARD_INFO_REQUEST, null);
					break;
				}
			case FiEventType.SEND_XL_PAIWEIPRIZE_RESQUSET:
				{
					Debug.LogError ("发送" + FiProtoType.XL_SEND_PAI_WEI_REWAED_REQUEST);

					PaiWeiPrizeInfo paiWeiPrizeInfo = (PaiWeiPrizeInfo)nDataIn.data;

					GetPaiWeiSaiReward reward = new GetPaiWeiSaiReward ();
					reward.RewardIndex = paiWeiPrizeInfo.rewardIndex;
					byte[] nByteBody = reward.ToByteArray ();
					SendByteArray (FiProtoType.XL_SEND_PAI_WEI_REWAED_REQUEST, nByteBody);
					break;
				}
			case FiEventType.SEND_XL_RONGYAOPRIZE_RESQUSET:
				{
					Debug.LogError ("发送领取荣耀奖励"); 
					SendByteArray (FiProtoType.XL_SEND_RONGYAO_PRIZE_REQUEST, null);
					break;
				}
			case FiEventType.SEND_XL_TOP_UP_GIFT_BAG_STATE_INFO_NEW_RESPOSE:
				{
					Debug.LogError ("发送礼包");
					SendByteArray (FiProtoType.XL_SEND_TOP_UP_GIFT_BAG_STATE_INFO_NEW, null);
					break;
				}
			case FiEventType.SEND_XL_SEVENDAY_BAG_STATE_INFO_NEW_RESPOSE:
				{
					Debug.LogError ("发送七日签到礼包");

					FiSevenDaysPage SevenDayInfo = (FiSevenDaysPage)nDataIn.data;
					GetSevenDayReward reward = new GetSevenDayReward ();
					reward.SelectIndex = SevenDayInfo.SendIndex;
					reward.UserDay = SevenDayInfo.UserDay;
					Debug.LogError ("nsenindex1" + reward.SelectIndex + "nRequest.UserDay1" + reward.UserDay);
					byte[] nByteBody = reward.ToByteArray ();
					SendByteArray (FiProtoType.XL_SEND_SEVENDAY_BAG_STATE_INFO_NEW, nByteBody);
					break;
				}
			case FiEventType.SEND_XL_SEVENDAY_STARTINFO_OPEN_NEW_RESPOSE:
				{
					Debug.LogError ("发送七日初始"); 
					SendByteArray (FiProtoType.XL_SEND_SEVENDAY_STARTINFO_STATE_INFO_NEW, null);
					break;
				}
			case FiEventType.SEND_XL_LEVELUP_INFO_NEW_RESPOSE:
				{
					Debug.LogError("發送新手升級資訊");
					SendByteArray(FiProtoType.XL_GET_LEVEL_UPGRADE_INFO, null);
					break;
				}

			case FiEventType.SEND_XL_LEVELUP_GET_NEW_RESPOSE:
				{
					Debug.LogError("發送新手升級領取獎勵");
					UpLevelRewards nRequest = (UpLevelRewards)nDataIn.data;
					SendUpLevelReward sReward = new SendUpLevelReward();
					sReward.TaskID = nRequest.taskID;
					byte[] nByteBody = sReward.ToByteArray();
					SendByteArray(FiProtoType.XL_SEND_LEVEL_UPGRADE_OPEN, nByteBody);
					break;
				}
			case FiEventType.SEND_XL_BUTTON_HIDE_STATE:
                {
                    Debug.LogError("发送按钮状态请求");
                    SendByteArray(FiProtoType.XL_SEND_BUTTON_HIDE_STATE, null);
                    break;
                }

				//發送金幣購買龍卡请求
				case FiEventType.XL_BUY_DRAGONCARD_REQUEST:
				{
					FiExchangeBarbette nRequestIn = (FiExchangeBarbette)nDataIn.data;
					useBuyCannonBottomInfo nRequestOut = new useBuyCannonBottomInfo();
					nRequestOut.BuyType = nRequestIn.buyType;
					byte[] nByteBody = nRequestOut.ToByteArray();
					SendByteArray(FiProtoType.XL_BUY_DRAGON_CARD, nByteBody);
				}
				break;


				//接收手機登入協定回覆
				case FiEventType.XL_GET_PHONE_NUMBER_REQUEST:
					{
                        Debug.Log("@@@ XL_GET_PHONE_NUMBER_REQUEST");
                        ConvertFormalPhoneNumber nRequestIn = (ConvertFormalPhoneNumber)nDataIn.data;
                        PB_ConvertFormalPhoneNumber nRequestOut = new PB_ConvertFormalPhoneNumber();
                        nRequestOut.StrPhoneNum = nRequestIn.phone;
                        nRequestOut.StrCode = nRequestIn.code;
#if UNITY_EDITOR
                        //Debug.Log("@@@ XL_GET_PHONE_NUMBER_REQUEST nRequestOut.StrPhoneNum = " + nRequestOut.StrPhoneNum);
                        //Debug.Log("@@@ XL_GET_PHONE_NUMBER_REQUEST nRequestOut.Pass = " + nRequestOut.StrCode);
#endif
                        byte[] nByteBody = nRequestOut.ToByteArray();
					SendByteArray(FiProtoType.XL_SEND_PHONE_LOGIN, nByteBody);
				}
				break;

			//接收手機號碼設定密碼協定回覆
			case FiEventType.XL_GET_PHONE_NUMBER_PASSWORD_REQUEST:
				{
					SetPhoneNumberPassword nRequestIn = (SetPhoneNumberPassword)nDataIn.data;
					PB_SetPhoneNumberPassword nRequestOut = new PB_SetPhoneNumberPassword();
					nRequestOut.StrPhoneNum = nRequestIn.phone;
					nRequestOut.Pass = nRequestIn.pass;
#if UNITY_EDITOR
                        Debug.Log("@@@ XL_GET_PHONE_NUMBER_PASSWORD_REQUEST nRequestOut.StrPhoneNum = " + nRequestOut.StrPhoneNum);
                        Debug.Log("@@@ XL_GET_PHONE_NUMBER_PASSWORD_REQUEST nRequestOut.Pass = " + nRequestOut.Pass);
#endif
                    byte[] nByteBody = nRequestOut.ToByteArray();
					//Debug.Log("@@@ XL_GET_PHONE_NUMBER_PASSWORD_REQUEST nByteBody length = " + nByteBody.Length);
					SendByteArray(FiProtoType.XL_SET_PHONE_PASSWORD, nByteBody);
				}
				break;

			//接收關聯帳號登入協定回覆
			case FiEventType.XL_GET_ASSOCIATE_ACC_LOGIN_REQUEST:
                    {
                        Debug.Log("@@@ XL_GET_ASSOCIATE_ACC_LOGIN_REQUEST 我是 20120 ");
                        LoginAccountAssociateChoise nRequestIn = (LoginAccountAssociateChoise)nDataIn.data;
                        PB_AssociateAccountLogin nRequestOut = new PB_AssociateAccountLogin();
                        nRequestOut.Result = nRequestIn.result;
                        nRequestOut.AccountType = nRequestIn.accountType;
                        nRequestOut.UserID = nRequestIn.user_id;
                        nRequestOut.AccountName = nRequestIn.accountName;
                        nRequestOut.StrToken = nRequestIn.strToken;
                        //nRequestOut.Nickname = nRequestIn.nickname;
                        Debug.Log("@@@ XL_GET_ASSOCIATE_ACC_LOGIN_REQUEST 我是 20120    20120 ");
#if UNITY_EDITOR
                        Debug.Log("@@@ XL_GET_ASSOCIATE_ACC_LOGIN_REQUEST nRequestOut.Result = " + nRequestOut.Result);
                        Debug.Log("@@@ XL_GET_ASSOCIATE_ACC_LOGIN_REQUEST nRequestOut.AccountType = " + nRequestOut.AccountType);
                        Debug.Log("@@@ XL_GET_ASSOCIATE_ACC_LOGIN_REQUEST nRequestOut.UserID = " + nRequestOut.UserID);
                        Debug.Log("@@@ XL_GET_ASSOCIATE_ACC_LOGIN_REQUEST nRequestOut.AccountName = " + nRequestOut.AccountName);
                        Debug.Log("@@@ XL_GET_ASSOCIATE_ACC_LOGIN_REQUEST nRequestOut.StrToken = " + nRequestOut.StrToken);
                        //Debug.Log("@@@ XL_GET_ASSOCIATE_ACC_LOGIN_REQUEST nRequestOut.nickname = " + nRequestOut.Nickname);
#endif
                        Debug.Log("@@@ XL_GET_ASSOCIATE_ACC_LOGIN_REQUEST 我是 20120    20120    20120");
                        byte[] nByteBody = nRequestOut.ToByteArray();
                        //                        #region DLL
                        //                      PB_PhoneLoginAccount nRequestOut = new PB_PhoneLoginAccount();
                        //                        nRequestOut.result = nRequestIn.result;
                        //                        nRequestOut.accountType = nRequestIn.accountType;
                        //                        nRequestOut.userId = nRequestIn.user_id;
                        //                        nRequestOut.accountName = nRequestIn.accountName;
                        //#if UNITY_EDITOR
                        //                        Debug.Log("@@@ XL_GET_ASSOCIATE_ACC_LOGIN_REQUEST nRequestOut.Result = " + nRequestOut.result);
                        //                        Debug.Log("@@@ XL_GET_ASSOCIATE_ACC_LOGIN_REQUEST nRequestOut.AccountType = " + nRequestOut.accountType);
                        //                        Debug.Log("@@@ XL_GET_ASSOCIATE_ACC_LOGIN_REQUEST nRequestOut.UserID = " + nRequestOut.userId);
                        //                        Debug.Log("@@@ XL_GET_ASSOCIATE_ACC_LOGIN_REQUEST nRequestOut.AccountName = " + nRequestOut.accountName);
                        //#endif
                        //                        byte[] nByteBody = MsgSerializer(nRequestOut);
                        //                        #endregion
                        Debug.Log("@@@ XL_GET_ASSOCIATE_ACC_LOGIN_REQUEST 我是 20120    20120    20120   20120 ");
                        SendByteArray(122, nByteBody);
                        Debug.Log("@@@ XL_GET_ASSOCIATE_ACC_LOGIN_REQUEST 我是 20120    20120    20120   20120 20120");
                    }
				break;
			case FiEventType.XL_SET_PHONE_LOGIN_PASS_REQUEST:// 20120
					{
						SetPhoneLoginPass nRequestIn = (SetPhoneLoginPass)nDataIn.data;

                        PB_SetPhoneLoginPass nRequestOut = new PB_SetPhoneLoginPass();
                        nRequestOut.PhoneNumber = nRequestIn.phone;
                        nRequestOut.Password = nRequestIn.pass;
						//nRequestOut.Nickname = Google.Protobuf.ByteString.CopyFromUtf8(nRequestIn.nickname);
						nRequestOut.Nickname = nRequestIn.nickname;
						byte[] nByteBody = nRequestOut.ToByteArray();
						#region DLL
						//PBMsg_SetPhoneLoginPass nRequestOut = new PBMsg_SetPhoneLoginPass();
						//nRequestOut.phoneNumber = nRequestIn.phone;
						//nRequestOut.password = nRequestIn.pass;
						//nRequestOut.nickname = nRequestIn.nickname;
						//byte[] nByteBody = MsgSerializer(nRequestOut);
						#endregion

						SendByteArray(FiProtoType.XL_SET_PHONE_LOGINPASS, nByteBody);
					}
				break;
			case FiEventType.XL_GET_USER_NICK_REQUEST:// 10121
					{
						LoginAccountNickChoice nRquestIn = (LoginAccountNickChoice)nDataIn.data;

					SendByteArray(FiProtoType.XL_GET_NEW_NICK, null);
					}
				break;

				default:
				{
					Debug.LogError (  "-----------start---------------" + nDataIn.type + " / " + nDataIn.data);
					if (nDataIn.data != null) {
						try {
							byte[] nSendData = (byte[])nDataIn.data;
							SendByteArray (nDataIn.type, nSendData);
						} catch {
							Debug.LogError ("------------send message error---------------");
						}
					} else {
						SendByteArray (nDataIn.type, null);
					}
					/*if ( nDataIn.data.GetType() == typeof( byte[] ))
					{
						//byte[] nSendData = (byte[])nDataIn.data;
						//SendByteArray (nDataIn.type, (byte[])nDataIn.data);
						Debug.LogError (  "------------send in threads---------------");
					} else {
						Debug.LogError (  "------------send error---------------" + nDataIn.type );
					}*/
				}
				break;
			
			}
		}



		public void SendByteArray (int nMsgId, byte[] nByteSend, int nUserId = -1)
		{
			//Debug.LogError ("send byte array nMsgId " + nMsgId);
			FiProtoType.Head nRequestHead = new FiProtoType.Head ();
			nRequestHead.message_id = (short)nMsgId;
			nRequestHead.timetick = mTickCount;
			nRequestHead.userid = mUserId;
			if (nUserId != -1) {
				nRequestHead.userid = nUserId;
			}
			byte[] nByteBody = nByteSend;
			byte[] nResultByte = mEncoder.encode (nRequestHead, nByteBody);
			int byteLength;
			if (mSession != null) {
				
				if (nByteBody != null) {
					byteLength = nResultByte.Length - nByteBody.Length;
					//Debug.LogError ("[---Error---- nResultByte.Length - nByteBody.Length---]  111==== " + (nResultByte.Length - nByteBody.Length) + "  ");
					//Debug.LogError ("SendByteArray[----Error ------- nByteSend.Length ---]  " + nByteSend.Length);

					if (byteLength < 16 || nResultByte.Length > 1024) {
//						Debug.LogError ("[---Error---- nResultByte.Length - nByteBody.Length---]  22==== " + (nResultByte.Length - nByteBody.Length));
//						Debug.LogError ("[---Error---- The ByteArray.Length---] = " + nResultByte.Length);
//						Debug.LogError ("---Error---- nMsgId---  === " + nMsgId);
						return;
					}
				}
				if (nMsgId == 6) {
					//Debug.LogError ("SendByteArray-----Fishing Out Scene nResultByte.Length --- " + nResultByte.Length);
					if (nByteBody != null) {
						//Debug.LogError ("SendByteArray -------  nByteBody.Length---] = " + nByteBody.Length);
					}

				}
				//		Debug.Log ("send byte array" + nResultByte.Length );
//				Debug.LogError ("======包头=======");
//				Debug.LogError ("SendByteArray[------- nRequestHead.message_id ---]    " + nRequestHead.message_id);
//				Debug.LogError ("SendByteArray[------- nRequestHead.timetick ---]  " + nRequestHead.timetick);
//				Debug.LogError ("SendByteArray------- nRequestHead.userid ---  " + nRequestHead.userid);
//				Debug.LogError ("SendByteArray------- nMsgId --- " + nMsgId);
//				Debug.LogError ("SendByteArray[------- nResultByte.Length ---]  " + nResultByte.Length);
//				Debug.LogError ("======包底=======");
				mSession.sendBytes (nResultByte);
			}
		}


		private void doSendHeatBeat ()
		{
			DispatchData nData = new DispatchData ();
			nData.type = -1;
			sendMessage (nData);
		}

        public byte[] MsgSerializer<T>(T msg)
        {
            try
            {
                using (MemoryStream ms = new MemoryStream())
                {
                    Serializer.Serialize(ms, msg);
                    byte[] result = new byte[ms.Length];
                    ms.Position = 0;
                    ms.Read(result, 0, result.Length);
                    return result;
                }
            }
            catch (Exception ex)
            {
                Debug.Log("序列化失败: " + ex.ToString());
                return null;
            }
            using (MemoryStream ms = new MemoryStream())
            {
                ms.Position = 0;

                Serializer.Serialize<T>(ms, msg);

                ms.Flush();
                return ms.GetBuffer();
            }
        }
    }
}

