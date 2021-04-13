using UnityEngine;
using System.Collections;
using System.Runtime.InteropServices;
using System.Net;
using System.Net.Sockets;
using System;
using System.Threading;
using System.IO;
using System.Collections.Generic;

namespace AssemblyCSharp
{

	public class NetStateListener: OnNetworkListener
	{
		private NetControl mNetCtrl;

		private FiProtoDecoder mDecoder = new FiProtoDecoder ();

		private const  int RECV_BUFFER_SIZE = 8192;

		//private const  int SINGLE_MESSAGE_SIZE_MAX = 8192;

		private byte[] mCacheBuffer = new byte[ RECV_BUFFER_SIZE ];

		private int mBufferLen = 0;
	
		private int nLastRecvLen = 0;

		private FiLocalMsgBridge mMsgBridge;

		private CollectNetInfo mInfoCollect;

		public void setCollect (CollectNetInfo nInfo)
		{
			mInfoCollect = nInfo;
		}

		/*public void setState( bool isRunning )
		{

		}*/

		public NetStateListener (NetControl nCtrl)
		{
			//Debug.LogError ("NetStateListener--------------");
			mNetCtrl = nCtrl;
			mMsgBridge = new FiLocalMsgBridge (nCtrl);
		}

		/**server response callback*/
		public int onRecvMessage (int type, byte[] data, int len)
		{
			if (type == FiEventType.RECV_MESSAGE) {
				nLastRecvLen = len;

				//数据长度添满啦，只能当单条协议长度 > 8192的时候，会出现
				if (mBufferLen + len > mCacheBuffer.Length) {
					byte[] nNewCacheBuffer = new byte[ mCacheBuffer.Length * 2 ];
					Buffer.BlockCopy (mCacheBuffer, 0, nNewCacheBuffer, 0, mBufferLen);
					mCacheBuffer = nNewCacheBuffer;
				}

				Buffer.BlockCopy (data, 0, mCacheBuffer, mBufferLen, len);
				mBufferLen += len;
				
				//返回消耗掉掉字节数，
				int nUsedLen = handleRecvData (mCacheBuffer, mBufferLen);
				//buffer中有数据消耗
				if (nUsedLen > 0) {
					//查看消耗了多少数据，不存在mBufferLen < nUsedLen 的情况
					if (mBufferLen >= nUsedLen) {
						int nCacheLen = mBufferLen - nUsedLen;
						//存在粘包数据
						if (nCacheLen != 0) {
							byte[] nReaminData = new byte[ nCacheLen ];
							Buffer.BlockCopy (mCacheBuffer, nUsedLen, nReaminData, 0, nCacheLen);
							Buffer.BlockCopy (nReaminData, 0, mCacheBuffer, 0, nCacheLen);
						}
						mBufferLen = nCacheLen;
						return nUsedLen;
					}
				}

				//错误的消息，找不到messageid，或者服务器下发消息出问题
				if (nUsedLen == FiEventType.ERROR_MESSAGE) {
					mBufferLen = 0;
					mNetCtrl.dispatchEvent (FiEventType.ERROR_MESSAGE, new FiNetworkInfo ());
				}
			}
			mNetCtrl.dispatchEvent (type, new FiNetworkInfo ());
			return 0;
		}




		private int handleRecvData (byte[] dataIn, int dataLen)
		{
			//没有头部
			int nHeadLen = Marshal.SizeOf (new FiProtoType.Head ());
			if (dataLen < nHeadLen) {
				Debug.Log ("[ decoder message  error] dataLen < nHeadLen" + dataLen + "error################################----------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------");
				return 0;
			}
			int nCount = 0;
			// 至少==16 ，包含头部消息
			int nConsumedSize = 0;

			byte[] nRefData = new byte[  dataLen ];
			byte[] nHeadByte = new byte[ nHeadLen ];

			//解包处理
			while (nConsumedSize < dataLen) {
				nCount++;
				Buffer.BlockCopy (dataIn, nConsumedSize, nHeadByte, 0, nHeadLen);
				FiProtoType.Head nRecvHead = mDecoder.ByteToStruct (nHeadByte).GetValueOrDefault ();

				//解包后的数据小于头部16个字节
				if (nRecvHead.data_length < nHeadLen) {
					Debug.LogError ("---------------recv error message------------------" + nRecvHead.data_length + "/" + nLastRecvLen);
					return FiEventType.ERROR_MESSAGE;
				}

				//找不到消息id，错误的数据
				if (!mMsgBridge.isContainMessage (nRecvHead.message_id) && nRecvHead.message_id != 0) {
					Debug.LogError ("---------------recv error message------------------ :" + nRecvHead.message_id + "/" + nRecvHead.data_length);
					Tool.OutLogToFile ("---------------recv error message------------------ :" + nRecvHead.message_id + "/" + nRecvHead.data_length);
					return FiEventType.ERROR_MESSAGE;
				}

				//还有没能消费的数据,存在粘包数据，需要的数据长度大于总长度
				if (nRecvHead.data_length + nConsumedSize > dataLen) {
					Debug.LogError ("---------------hast remain message------------------" + nRecvHead.data_length + "/ nConsumedSize" + nConsumedSize + "/ dataLen" + dataLen + " / " + nRecvHead.message_id);
					Tool.OutLogToFile ("---------------hast remain message------------------" + nRecvHead.data_length + "/ nConsumedSize" + nConsumedSize);
					return nConsumedSize;
				}
				Buffer.BlockCopy (dataIn, nConsumedSize, nRefData, 0, nRecvHead.data_length);
				nConsumedSize += nRecvHead.data_length;

				byte[] content = mDecoder.toByteContent (nRecvHead, nRefData);
				doDispatch (nRecvHead, content);



				if (nCount >= 10) {
//					Debug.LogError ("---------------nCount >=10------------------" + nCount + "/ nConsumedSize" + nConsumedSize + " / dataLen == " + dataLen + "/" + nRecvHead.message_id + "/" + nRecvHead.data_length);
				}
			}

			if (nConsumedSize != dataLen) {
				Debug.LogError ("---------------nConsumedSize != dataLen------------------" + nConsumedSize + "/ nConsumedSize" + dataLen);
				Tool.OutLogToFile ("---------------nConsumedSize != dataLen------------------" + nConsumedSize + "/ nConsumedSize" + dataLen);
			} else {
				//Debug.LogError ( "---------------nConsumedSize = dataLen------------------" + nConsumedSize + "/ nConsumedSize" + dataLen );
			}
			//只能当消耗当数据 == 收到当数据时候
			return dataLen;
		}

		//private int nHeartBeatCount = 0;

		private void  doDispatch (FiProtoType.Head nRecvHead, byte[] content)
		{
			if (mNetCtrl == null)
				return;
			mInfoCollect.OnRecvData ();
			if (nRecvHead.message_id == 0) {
				//		Debug.Log ( "[ data dispatch ]!!!收到请求心跳包 " );
				//mInfoCollect.OnRecvResponse ();
				//		Debug.LogError ("【 recv 】-------nHeartBeatCount--------" + (nHeartBeatCount++) );
				return;
			}
			//Debug.LogError ("【 recv 】-------message_id--------" +  nRecvHead.message_id );
			mMsgBridge.processRecv (nRecvHead.message_id, content);

			/*switch (nRecvHead.message_id)
			{



			//鱼游出场景设置
			case FiProtoType.FISHING_NOTIFY_FISH_OUT_OF_SCENE:
				try {
					PB_FishOutScene nOutScene = PB_FishOutScene.Parser.ParseFrom (content);
					mNetCtrl.dispatchEvent (FiEventType.RECV_FISH_OUT_RESPONSE, FiProtoHelper.toLocal_FishOutReply (nOutScene));
				} catch (Exception e) {
					Debug.Log ("[ network ] recv message== FISHING_NOTIFY_FISH_OUT_OF_SCENE error" + e.Message);
				}
				break;



			//玩家离开渔场反馈
			case FiProtoType.FISHING_LEAVE_CLASSICAL_ROOM_RES:

				Debug.Log ("[ network ] recv message == FISHING_LEAVE_CLASSICAL_ROOM_RES" + content.Length);
				try {
					PB_LeaveRoomResponse nLeaveRes = PB_LeaveRoomResponse.Parser.ParseFrom (content);
					mNetCtrl.dispatchEvent (FiEventType.RECV_USER_LEAVE_RESPONSE, FiProtoHelper.toLocal_LeaveReply (nLeaveRes));
				} catch (Exception e) {
					Debug.Log ("[ network ] recv message== FISHING_LEAVE_CLASSICAL_ROOM_RES error" + e.Message);
				}
				break;




			
			//其他玩家离开房间消息
			case FiProtoType.FISHING_NOTIFY_LEAVE_CLASSICAL_ROOM:
				Debug.Log ("[ network ] recv message == FISHING_OTHER_LEAVE_ROOM_");
				try {
					PB_OtherLeaveRoomInfrom nOtherLeave = PB_OtherLeaveRoomInfrom.Parser.ParseFrom (content);   
					mNetCtrl.dispatchEvent (FiEventType.RECV_OTHER_LEAVE_ROOM_INFORM, FiProtoHelper.toLocal_OtherLeave (nOtherLeave));
				} catch (Exception e) {
					Debug.Log ("[ network ] recv message== FISHING_OTHER_LEAVE_ROOM_ error" + e.Message);
				}

				break;



			//其他玩家登陆
			case FiProtoType.FISHING_NOTIFY_ENTER_CLASSICAL_ROOM:
				Debug.Log ("[ network ] recv message == FISHING_OTHER_ENTER_ROOM_INFORM");
				try {
					PB_OtherEnterRoomInfrom nOtherEnter = PB_OtherEnterRoomInfrom.Parser.ParseFrom (content);   
					mNetCtrl.dispatchEvent (FiEventType.RECV_OTHER_ENTER_ROOM_INFORM, FiProtoHelper.toLocal_OtherEnter (nOtherEnter));
				} catch (Exception e) {
					Debug.Log ("[ network ] recv message== FISHING_OTHER_ENTER_ROOM_INFORM error" + e.Message);
				}
				break;





			//登陆反馈
			case FiProtoType.FISHING_LOGIN_RES:
				
				Debug.Log ("[ network ] recv message == FISHING_LOGIN_RES");
				try {
					PB_LoginResponse nLoginResult = PB_LoginResponse.Parser.ParseFrom (content);   
					FiLoginResponse nLoginReply = FiProtoHelper.toLocal_LoginReply (nLoginResult);
					Debug.Log ("[ network ] recv message== " + nLoginReply.ToString ());
					mNetCtrl.setUserId (nLoginResult.UserId);
					mNetCtrl.dispatchEvent (FiEventType.RECV_LOGIN_RESPONSE, nLoginReply);
				} catch (Exception e) {
					Debug.Log ("[ network ] recv message== FISHING_LOGIN_RES error" + e.Message);
				}
				break;




			//房间匹配反馈
			case FiProtoType.FISHING_ENTER_CLASSICAL_ROOM_RES:
				//Debug.Log ("[ network ] recv message== FISHING_ENTER_CLASSICAL_ROOM_RES");
				try {
					PB_EnterRoomResponse nMatchResult = PB_EnterRoomResponse.Parser.ParseFrom (content);
					Debug.Log ("[ network ] recv message== Others.Count" + nMatchResult.Others.Count + "---SeatIndex" + nMatchResult.SeatIndex);
					mNetCtrl.dispatchEvent (FiEventType.RECV_ROOM_MATCH_RESPONSE, FiProtoHelper.toLocal_MatchReply (nMatchResult));
				} catch (Exception e) {
					Debug.Log ("[ network ] recv message== FISHING_ENTER_CLASSICAL_ROOM_RES error" + e.Message);
				}
				break;




			//收到其他玩家发射子弹消息
			case FiProtoType.FISHING_NOTIFY_FIRE:
				try {
					PB_UserFireRequest nRecvFire = PB_UserFireRequest.Parser.ParseFrom (content);
					Debug.Log ("[ network ] recv user fire message== " + nRecvFire.BulletId + "/" + nRecvFire.UserId + "/" + nRecvFire.CannonRatio + "/" + nRecvFire.Position.X);
					mNetCtrl.dispatchEvent (FiEventType.RECV_OTHER_FIRE_BULLET_INFORM, FiProtoHelper.toLocal_OtherFireInform (nRecvFire));
				} catch (Exception e) {
					Debug.Log ("[ network ] recv message== FISHING_RECV_OTHER_FIRE_INFORM error" + e.Message);
				}
				break;





			//子弹打中鱼消息
			case FiProtoType.FISHING_NOTIFY_ON_FISH_HIT:
				try {
					PB_FishHitInfrom nHitInfom = PB_FishHitInfrom.Parser.ParseFrom (content);
					Debug.Log ("[ network ] recv user FISHING_NOTIFY_ON_FISH_HIT== " + nHitInfom.BulletId + "/" + nHitInfom.FishId + "/" + nHitInfom.GroupId + "/" + nHitInfom.UserId);
					mNetCtrl.dispatchEvent (FiEventType.RECV_HITTED_FISH_RESPONSE, FiProtoHelper.toLocal_OtherHitFishInform (nHitInfom));
				} catch (Exception e) {
					Debug.Log ("[ network ] ========= FISHING_NOTIFY_ON_FISH_HIT error!!!!!!!!!! decoder error!!!" + e.Message);
				}
				break;





			//创建鱼群消息
			case FiProtoType.FISHING_NOTIFY_FISH_GROUP:
				try {
					PB_FishGroupInfrom nGroupInfrom = PB_FishGroupInfrom.Parser.ParseFrom (content);
					Debug.Log ("[ network ]!!!!!!!!!!!! recv create fishs == num" + nGroupInfrom.FishNum + "/ type = " + nGroupInfrom.FishType + " / GroupId =" + nGroupInfrom.GroupId + "/ TrackId =" + nGroupInfrom.TrackId + "/TrackType =" + nGroupInfrom.TrackType);
					mNetCtrl.dispatchEvent (FiEventType.RECV_FISHS_CREATED_INFORM, FiProtoHelper.toLocal_FishsCreatedInform (nGroupInfrom));
				} catch (Exception e) {
					Debug.Log ("[ network ] recv message== FISHING_NOTIFY_FISH_GROUP error" + e.Message); 
				}
				break;






				//用户改变炮倍数反馈
			case FiProtoType.FISHING_CHANGE_CANNON_MULTIPLE_RESPONSE:
				try {
					PB_ChangeCannonResponse nCannonRes = PB_ChangeCannonResponse.Parser.ParseFrom (content);
					mNetCtrl.dispatchEvent (FiEventType.RECV_CHANGE_CANNON_RESPONSE, FiProtoHelper.toLocal_CannonChangeResponse (nCannonRes));
				} catch (Exception e) {
					Debug.Log ("[ network ] recv message== FISHING_CHANGE_CANNON_MULTIPLE_RESPONSE error" + e.Message); 
				}
				break;








				//其他用户改变炮倍数通知
			case FiProtoType.FISHING_OTHER_CHANGE_CANNON_MULTIPLE:
				try {
					PB_OtherChangeCannon nOthCannonRes = PB_OtherChangeCannon.Parser.ParseFrom (content);
					mNetCtrl.dispatchEvent (FiEventType.RECV_OTHER_CHANGE_CANNON_INFORM, FiProtoHelper.toLocal_CannonChangeInform (nOthCannonRes));
				} catch (Exception e) {
					Debug.Log ("[ network ] recv message== FISHING_OTHER_CHANGE_CANNON_MULTIPLE error" + e.Message); 
				}
				break;








			//收到自己发起到特效反馈
			case FiProtoType.FISHING_EFFECT_RESPONSE:
				try {
					PB_EffectResponse nEffectRes = PB_EffectResponse.Parser.ParseFrom (content);
					mNetCtrl.dispatchEvent (FiEventType.RECV_USE_EFFECT_RESPONSE, FiProtoHelper.toLocal_EffectResponse (nEffectRes));
				} catch (Exception e) {
					Debug.Log ("[ network ] recv message== FISHING_EFFECT_RESPONSE error" + e.Message); 
				}
				break;








				//其他用户的特效处理
			case FiProtoType.FISHING_OTHER_EFFECT:
				try {
					PB_OtherEffect nOthEffectRes = PB_OtherEffect.Parser.ParseFrom (content);
					mNetCtrl.dispatchEvent (FiEventType.RECV_OTHER_EFFECT_INFORM, FiProtoHelper.toLocal_EffectInfrom (nOthEffectRes));
				} catch (Exception e) {
					Debug.Log ("[ network ] recv message== FISHING_OTHER_EFFECT error" + e.Message); 
				}
				break;







				//其他玩家的冰冻超时
			case FiProtoType.FISHING_FREEZE_TIMEOUT:
				try {
					FreezeTimeout nFreezeInform = FreezeTimeout.Parser.ParseFrom (content);
					mNetCtrl.dispatchEvent (FiEventType.RECV_FISH_FREEZE_TIMEOUT_INFORM, FiProtoHelper.toLocal_FreezeInfrom (nFreezeInform));
				} catch (Exception e) {
					Debug.Log ("[ network ] recv message== FISHING_FREEZE_TIMEOUT error" + e.Message); 
				}
				break;




				//充值反馈
			case FiProtoType.FISHING_TOP_UP_RESPONSE:
				try {
					TopUpResponse nTopupRes = TopUpResponse.Parser.ParseFrom (content);
					mNetCtrl.dispatchEvent (FiEventType.RECV_TOPUP_RESPONSE, FiProtoHelper.toLocal_TopUpResponse  (nTopupRes));
				} catch (Exception e) {
					Debug.Log ("[ network ] recv message== FISHING_TOP_UP_RESPONSE error" + e.Message); 
				}
				break;





				//获取背包反馈
			case FiProtoType.FISHING_GET_BACKPACK_PROPERTY_RESPONSE:
				try {
					GetBackpackPropertyResponse nPropRes = GetBackpackPropertyResponse.Parser.ParseFrom (content);
					mNetCtrl.dispatchEvent (FiEventType.RECV_BACKPACK_RESPONSE, FiProtoHelper.toLocal_BackpackResponse  (nPropRes));
				} catch (Exception e) {
					Debug.Log ("[ network ] recv message== FISHING_GET_BACKPACK_PROPERTY_RESPONSE error" + e.Message); 
				}
				break;





				//获取PK房间列表反馈
			case FiProtoType.FISHING_GET_PKROOM_LIST_RESPONSE:
				try {
					PB_GetPKRoomListResponse nPKGetRes = PB_GetPKRoomListResponse.Parser.ParseFrom (content);
					mNetCtrl.dispatchEvent (FiEventType.RECV_GET_PK_ROOMS_RESPONSE, FiProtoHelper.toLocal_GetRoomListResponse   (nPKGetRes));
				} catch (Exception e) {
					Debug.Log ("[ network ] recv message== FISHING_GET_PKROOM_LIST_RESPONSE error" + e.Message); 
				}
				break;




//				//创建PK场反馈
//			case FiProtoType.FISHING_CREATE_PKROOM_RESPONSE:
//				try {
//					PB_CreatePKRoomResponse nPKCreateRes = PB_CreatePKRoomResponse.Parser.ParseFrom (content);
//					mNetCtrl.dispatchEvent (FiEventType.RECV_CREATE_PK_ROOM_RESPONSE, FiProtoHelper.toLocal_CreateRoomResponse (nPKCreateRes) );
//				} catch (Exception e) {
//					Debug.Log ("[ network ] recv message== FISHING_CREATE_PKROOM_RESPONSE error" + e.Message); 
//				}
//				break;







				//开始PK赛的反馈
			case FiProtoType.FISHING_START_PKGAME_RESPONSE :
				try{
					PB_StartPKGameResponse nPkRes = PB_StartPKGameResponse.Parser.ParseFrom( content );
					mNetCtrl.dispatchEvent (FiEventType.RECV_START_PK_GAME_RESPONSE, FiProtoHelper.toLocal_StartPkResponse( nPkRes ) );
				}catch( Exception e )
				{
					Debug.Log ("[ network ] recv message== FISHING_START_PKGAME_RESPONSE error" + e.Message); 
				}
				break;




				//通知其他玩家PK赛开始啦
			case FiProtoType.FISHING_NOTIFY_PKGAME_START:
				try{
					PB_NotifyPKGameStart nPkStart = PB_NotifyPKGameStart.Parser.ParseFrom( content );
					mNetCtrl.dispatchEvent (FiEventType.RECV_START_PK_GAME_INFORM, FiProtoHelper.toLocal_OwnerStartPkResponse ( nPkStart ) );
				}catch( Exception e )
				{
					Debug.Log ("[ network ] recv message== FISHING_START_PKGAME_RESPONSE error" + e.Message); 
				}
				break;



				//进入PK场的反馈
			case FiProtoType.FISHING_ENTER_PKROOM_RESPONSE:
				try{
					PB_EnterPKRoomResponse nEnterPkRes = PB_EnterPKRoomResponse.Parser.ParseFrom( content );
					mNetCtrl.dispatchEvent (FiEventType.RECV_ENTER_PK_ROOM_RESPONSE, FiProtoHelper.toLocal_EnterPkRoomResponse( nEnterPkRes ) );
				}catch( Exception e )
				{
					Debug.Log ("[ network ] recv message== FISHING_ENTER_PKROOM_RESPONSE error" + e.Message); 
				}
				break;




				//通知其他玩家进入PK场
			case FiProtoType.FISHING_NOTIFY_ENTER_PKROOM:
				try{
					PB_NotifyOtherEnterPKRoom nOtherLeavePkRes = PB_NotifyOtherEnterPKRoom.Parser.ParseFrom( content );
				
					mNetCtrl.dispatchEvent (FiEventType.RECV_OTHER_ENTER_PK_ROOM_INFORM, FiProtoHelper.toLocal_OtherEnterPkRoom ( nOtherLeavePkRes ) );
				}catch( Exception e )
				{
					Debug.Log ("[ network ] recv message== FISHING_NOTIFY_ENTER_PKROOM error" + e.Message); 
				}
				break;




				//玩家离开PK场反馈
			case FiProtoType.FISHING_LEAVE_PKROOM_RESPONSE :
				try{
					PB_LeavePKRoomResponse nLeavePkRes = PB_LeavePKRoomResponse.Parser.ParseFrom( content );
					mNetCtrl.dispatchEvent (FiEventType.RECV_LEAVE_PK_ROOM_RESPONSE, FiProtoHelper.toLocal_LeavePkRoomResponse( nLeavePkRes ) );
				}catch( Exception e )
				{
					Debug.Log ("[ network ] recv message== FISHING_LEAVE_PKROOM_RESPONSE error" + e.Message); 
				}
				break;




				//其他玩家离开PK场房间通知
			case FiProtoType.FISHING_NOTIFY_LEAVE_PKROOM:
				try{
					PB_NotifyOtherLeavePKRoom nOtherLeavePkRes = PB_NotifyOtherLeavePKRoom.Parser.ParseFrom( content );
					mNetCtrl.dispatchEvent (FiEventType.RECV_OTHER_LEAVE_PK_ROOM_INFORM, FiProtoHelper.toLocal_OtherLeavePkRoomResponse( nOtherLeavePkRes ) );
				}catch( Exception e )
				{
					Debug.Log ("[ network ] recv message== FISHING_NOTIFY_LEAVE_PKROOM error" + e.Message); 
				}
				break;




				//其他玩家准备PK赛通知
			case FiProtoType.FISHING_PREPARE_PKGAME:

				try{
					PB_PreparePKGame nPrepare = PB_PreparePKGame.Parser.ParseFrom( content );
					FiPreparePKGame  nRecvPrepare = new FiPreparePKGame();
					nRecvPrepare.roomIndex = nPrepare.RoomIndex;
					nRecvPrepare.roomType = nPrepare.RoomType;
					nRecvPrepare.userId = nPrepare.UserId;
					mNetCtrl.dispatchEvent (FiEventType.RECV_OTHER_PREPARE_PKGAME_INFORM, nRecvPrepare );
				}catch( Exception e )
				{
					Debug.Log ("[ network ] recv message== FISHING_PREPARE_PKGAME error" + e.Message); 
				}

				break;




				//其他玩家取消准备通知
			case FiProtoType.FISHING_CANCEL_PREPARE_PKGAME:

				try{
					PB_CancelPreparePKGame nPrepareCancel = PB_CancelPreparePKGame.Parser.ParseFrom( content );
					FiCancelPreparePKGame  nRecvCancel = new FiCancelPreparePKGame();
					nRecvCancel.roomIndex = nPrepareCancel.RoomIndex;
					nRecvCancel.roomType = nPrepareCancel.RoomType;
					nRecvCancel.userId = nPrepareCancel.UserId;
					mNetCtrl.dispatchEvent (FiEventType.RECV_OTHER_CANCEL_PREPARE_PKGAME_INFORM, nRecvCancel );
				}catch( Exception e )
				{
					Debug.Log ("[ network ] recv message== FISHING_CANCEL_PREPARE_PKGAME error" + e.Message); 
				}

				break;






				//PK赛倒计时结束同步
			case FiProtoType.FISHING_PK_GAME_COUNTDOWN:
				try
				{

					PB_PreStartCountdown nPreStart = PB_PreStartCountdown.Parser.ParseFrom(content);
					FiPkGameCountDownInform nCountInform = new FiPkGameCountDownInform();
					nCountInform.countdown = nPreStart.Countdown;
					mNetCtrl.dispatchEvent (FiEventType.RECV_PKGAME_COUNTDOWN_INFORM, nCountInform );

				}catch( Exception e ) {
					Tool.OutLogToFile ("[ network ] recv message== FISHING_PK_GAME_COUNTDOWN error" + e.Message);
				}

				break;
			





				//PK赛倒计时同步
			case FiProtoType.FISHING_PRE_START_COUNTDOWN:
				try
				{

					PB_PreStartCountdown nCountDown = PB_PreStartCountdown.Parser.ParseFrom(content);
					FiPkGameCountDownInform nCountInform = new FiPkGameCountDownInform();
					nCountInform.countdown = nCountDown.Countdown;
					mNetCtrl.dispatchEvent (FiEventType.RECV_PRE_PKGAME_COUNTDOWN_INFORM, nCountInform );

				}catch( Exception e ) {
					Tool.OutLogToFile ("[ network ] recv message== FISHING_PRE_START_COUNTDOWN error" + e.Message);
				}
				break;







			case FiProtoType.FISHING_LAUNCH_TORPEDO_RESPONSE:
				try
				{

					PB_LaunchTorpedoResponse nLauchRes = PB_LaunchTorpedoResponse.Parser.ParseFrom( content );
					FiLaunchTorpedoResponse nFiTorRes = new FiLaunchTorpedoResponse();

					if( nLauchRes.Position != null){
					    nFiTorRes.position = new Cordinate();
						nFiTorRes.position.x = nLauchRes.Position.X;
						nFiTorRes.position.y = nLauchRes.Position.Y;
					}
						nFiTorRes.result = nLauchRes.Result;
						nFiTorRes.torpedoId = nLauchRes.TorpedoId;
						nFiTorRes.torpedoType = nLauchRes.TorpedoType;

			//		Tool.addExMessage ("FISHING_LAUNCH_TORPEDO_RESPONSE" + nFiTorRes.result );
						mNetCtrl.dispatchEvent (FiEventType.RECV_LAUNCH_TORPEDO_RESPONSE, nFiTorRes );

				}catch( Exception e )
				{
					Tool.OutLogToFile ("[ network ] recv message== FISHING_LAUNCH_TORPEDO_RESPONSE error" + e.Message);
				}
				break;




			case FiProtoType.FISHING_NOTIFY_LAUNCH_TORPEDO:
		//		Tool.addExMessage ("FISHING_NOTIFY_LAUNCH_TORPEDO" );
				try
				{

					PB_NotifyOtherLaunchTorpedo nOthLauchRes = PB_NotifyOtherLaunchTorpedo.Parser.ParseFrom( content );
					FiOtherLaunchTorpedoInform nFiTorInform = new FiOtherLaunchTorpedoInform();

					if( nOthLauchRes.Position != null)
					{
						nFiTorInform.position = new Cordinate();
						nFiTorInform.position.x = nOthLauchRes.Position.X;
						nFiTorInform.position.y = nOthLauchRes.Position.Y;
					}
					nFiTorInform.userId = nOthLauchRes.UserId;
					nFiTorInform.torpedoId = nOthLauchRes.TorpedoId;
					nFiTorInform.torpedoType = nOthLauchRes.TorpedoType;

					mNetCtrl.dispatchEvent (FiEventType.RECV_OTHER_LAUNCH_TORPEDO_INFORM, nFiTorInform );
				}catch( Exception e )
				{
					Tool.OutLogToFile ("[ network ] recv message== FISHING_NOTIFY_LAUNCH_TORPEDO error" + e.Message);
				}
				break;



			case FiProtoType.FISHING_PK_LAUNCH_TORPEDO_RESPONSE:
				try
				{

					PB_LaunchTorpedoResponse nLauchRes = PB_LaunchTorpedoResponse.Parser.ParseFrom( content );
					FiLaunchTorpedoResponse nFiTorRes = new FiLaunchTorpedoResponse();

					if( nLauchRes.Position != null){
						nFiTorRes.position = new Cordinate();
						nFiTorRes.position.x = nLauchRes.Position.X;
						nFiTorRes.position.y = nLauchRes.Position.Y;

						nFiTorRes.result = nLauchRes.Result;
						nFiTorRes.torpedoId = nLauchRes.TorpedoId;
						nFiTorRes.torpedoType = nLauchRes.TorpedoType;

						mNetCtrl.dispatchEvent (FiEventType.RECV_PK_LAUNCH_TORPEDO_RESPONSE, nFiTorRes );

					}
				}catch( Exception e )
				{
					Tool.OutLogToFile ("[ network ] recv message== FISHING_PK_LAUNCH_TORPEDO_RESPONSE error" + e.Message);
				}
				break;



			case FiProtoType.FISHING_NOTIFY_PK_LAUNCH_TORPEDO:
				try
				{

					PB_NotifyOtherLaunchTorpedo nOthLauchRes = PB_NotifyOtherLaunchTorpedo.Parser.ParseFrom( content );
					FiOtherLaunchTorpedoInform nFiTorInform = new FiOtherLaunchTorpedoInform();

					if( nOthLauchRes.Position != null)
					{
						nFiTorInform.position = new Cordinate();
						nFiTorInform.position.x = nOthLauchRes.Position.X;
						nFiTorInform.position.y = nOthLauchRes.Position.Y;
					}
					nFiTorInform.userId = nOthLauchRes.UserId;
					nFiTorInform.torpedoId = nOthLauchRes.TorpedoId;
					nFiTorInform.torpedoType = nOthLauchRes.TorpedoType;

					mNetCtrl.dispatchEvent (FiEventType.RECV_PK_OTHER_LAUNCH_TORPEDO_INFORM, nFiTorInform );
				}catch( Exception e )
				{
					Tool.OutLogToFile ("[ network ] recv message== FISHING_NOTIFY_PK_LAUNCH_TORPEDO error" + e.Message);
				}
				break;






			case FiProtoType.FISHING_TORPEDO_EXPLODE_RESPONSE:
		
				try
				{
					PB_TorpedoExplodeResponse nExpRes = PB_TorpedoExplodeResponse.Parser.ParseFrom( content );
					FiTorpedoExplodeResponse  nFiExp = new FiTorpedoExplodeResponse();

					Debug.LogError("**********EXPLODE_RESPONSE*********" );

					nFiExp.result = nExpRes.Result;
					nFiExp.rewards = new List<FiFishReward>();

					if( nExpRes.Rewards != null )
					{
						IEnumerator<PB_FishReward> nEum = nExpRes.Rewards.GetEnumerator ();
						while( nEum.MoveNext() )
						{
							FiFishReward nReward = new FiFishReward();
							nReward.fishId = nEum.Current.FishId;
							nReward.groupId = nEum.Current.GroupId;
							nReward.properties = new List<FiProperty>();

							if( nEum.Current.Properties != null )
							{
								IEnumerator<PB_Property> nEumProperty = nEum.Current.Properties.GetEnumerator ();
							//	Debug.LogError ("------------------------success!!!" + nEum.Current.Properties.Count );
								while( nEumProperty.MoveNext() )
								{
									FiProperty nSingP = new FiProperty();
									nSingP.type  = nEumProperty.Current.PropertyType;
									nSingP.value = nEumProperty.Current.Sum;
									nReward.properties.Add( nSingP );
								}
							}
							nFiExp.rewards.Add(nReward);
						}
					}
		//			Debug.LogError ("------------------------success!!!");
					mNetCtrl.dispatchEvent (FiEventType.RECV_TORPEDO_EXPLODE_RESPONSE, nFiExp );


				}catch( Exception e )
				{

					Debug.LogError ("------------------------error!!!");
					Tool.OutLogToFile ("[ network ] recv message== FISHING_TORPEDO_EXPLODE_RESPONSE error" + e.Message);
				}
				break;







			case FiProtoType.FISHING_NOTIFY_TORPEDO_EXPLODE:
			//	Tool.addExMessage ("FISHING_NOTIFY_TORPEDO_EXPLODE start");
				try
				{
					PB_NotifyOtherTorpedoExplode nExp = PB_NotifyOtherTorpedoExplode.Parser.ParseFrom( content );
					FiOtherTorpedoExplodeInform  nOtherExp = new FiOtherTorpedoExplodeInform();

				//	Debug.LogError("---------NOTIFY_TORPEDO-----------" );

					nOtherExp.rewards = new List<FiFishReward>();
					nOtherExp.torpedoId = nExp.TorpedoId;
					nOtherExp.torpedoType = nExp.TorpedoType;
					nOtherExp.userId = nExp.UserId;

					if( nExp.Rewards != null )
					{
						IEnumerator<PB_FishReward> nEum = nExp.Rewards.GetEnumerator ();
						while( nEum.MoveNext() )
						{
							FiFishReward nReward = new FiFishReward();
							nReward.fishId = nEum.Current.FishId;
							nReward.groupId = nEum.Current.GroupId;
							nReward.properties = new List<FiProperty>();

							IEnumerator<PB_Property> nEumProp = nEum.Current.Properties.GetEnumerator ();
							while( nEumProp.MoveNext() )
							{
								FiProperty nSingP = new FiProperty();
								nSingP.type = nEumProp.Current.PropertyType;
								nSingP.value = nEumProp.Current.Sum;
								nReward.properties.Add( nSingP );
							}
							nOtherExp.rewards.Add(nReward);
						}
					}
					mNetCtrl.dispatchEvent (FiEventType.RECV_OTHER_TORPEDO_EXPLODE_INFORM, nOtherExp );
				}catch( Exception e )
				{
			//		Tool.addExMessage ("FISHING_NOTIFY_TORPEDO_EXPLODE error");
					Tool.OutLogToFile ("[ network ] recv message== FISHING_NOTIFY_TORPEDO_EXPLODE error" + e.Message);
				}
				break;





			


			case FiProtoType.FISHING_NOTIFY_PK_TORPEDO_EXPLODE:
				try
				{
					PB_NotifyOtherTorpedoExplode nExp = PB_NotifyOtherTorpedoExplode.Parser.ParseFrom( content );
					FiOtherTorpedoExplodeInform  nOtherExp = new FiOtherTorpedoExplodeInform();
					nOtherExp.rewards = new List<FiFishReward>();
					nOtherExp.torpedoId = nExp.TorpedoId;
					nOtherExp.torpedoType = nExp.TorpedoType;
					nOtherExp.userId = nExp.UserId;

					if( nExp.Rewards != null )
					{
						IEnumerator<PB_FishReward> nEum = nExp.Rewards.GetEnumerator ();
						while( nEum.MoveNext() )
						{
							FiFishReward nReward = new FiFishReward();
							nReward.fishId = nEum.Current.FishId;
							nReward.groupId = nEum.Current.GroupId;
							nReward.properties = new List<FiProperty>();

							IEnumerator<PB_Property> nEumProp = nEum.Current.Properties.GetEnumerator ();
							while( nEumProp.MoveNext() )
							{
								FiProperty nSingP = new FiProperty();
								nSingP.type = nEumProp.Current.PropertyType;
								nSingP.value = nEumProp.Current.Sum;
								nReward.properties.Add( nSingP );
							}
							nOtherExp.rewards.Add(nReward);
						}
					}
					mNetCtrl.dispatchEvent (FiEventType.RECV_PK_OTHER_TORPEDO_EXPLODE_INFORM, nOtherExp );
				}catch( Exception e )
				{
					Tool.OutLogToFile ("[ network ] recv message== FISHING_NOTIFY_TORPEDO_EXPLODE error" + e.Message);
				}
				break;









			case FiProtoType.FISHING_PK_TORPEDO_EXPLODE_RESPONSE:
				try
				{
					PB_TorpedoExplodeResponse nExpRes = PB_TorpedoExplodeResponse.Parser.ParseFrom( content );
					FiTorpedoExplodeResponse  nFiExp = new FiTorpedoExplodeResponse();
					nFiExp.result = nExpRes.Result;
					nFiExp.rewards = new System.Collections.Generic.List<FiFishReward>();

					if( nExpRes.Rewards != null )
					{
						IEnumerator<PB_FishReward> nEum = nExpRes.Rewards.GetEnumerator ();
						while( nEum.MoveNext() )
						{
							FiFishReward nReward = new FiFishReward();
							nReward.fishId = nEum.Current.FishId;
							nReward.groupId = nEum.Current.GroupId;
							nReward.properties = new List<FiProperty>();

							IEnumerator<PB_Property> nEumProp = nEum.Current.Properties.GetEnumerator ();
							while( nEumProp.MoveNext() )
							{
								FiProperty nSingP = new FiProperty();
								nSingP.type = nEumProp.Current.PropertyType;
								nSingP.value = nEumProp.Current.Sum;
								nReward.properties.Add( nSingP );
							}
							nFiExp.rewards.Add(nReward);
						}
					}
					mNetCtrl.dispatchEvent (FiEventType.RECV_PK_TORPEDO_EXPLODE_RESPONSE, nFiExp );


				}catch( Exception e )
				{
					Tool.OutLogToFile ("[ network ] recv message== FISHING_PK_TORPEDO_EXPLODE_RESPONSE error" + e.Message);
				}
				break;





			case FiProtoType.FISHING_PK_DISTRIBUTE_PROPERTY:
				try
				{
					PB_DistributePKProperty nDistribute = PB_DistributePKProperty.Parser.ParseFrom( content );
					FiDistributePKProperty nFiDis = new FiDistributePKProperty();
					nFiDis.roomIndex = nDistribute.RoomIndex;
					nFiDis.properties = new List<FiProperty>();

					IEnumerator<PB_Property> nEum = nDistribute.Properties.GetEnumerator ();
					while( nEum.MoveNext() )
					{
						FiProperty nProp = new FiProperty();
						nProp.type = nEum.Current.PropertyType;
						nProp.value = nEum.Current.Sum;
						nFiDis.properties.Add(nProp);
					}
					mNetCtrl.dispatchEvent (FiEventType.RECV_PK_DISTRIBUTE_PROPERTY_INFORM, nFiDis );
				}catch( Exception e )
				{
					Tool.OutLogToFile ("[ network ] recv message== FISHING_PK_DISTRIBUTE_PROPERTY error" + e.Message);
				}
				break;





			case FiProtoType.FISHING_GOLD_GAME_RESULT:
				try
				{

					PB_GoldGameResult nGoldRes = PB_GoldGameResult.Parser.ParseFrom( content );
					FiGoldGameResult  nResult = new FiGoldGameResult();
					nResult.info = new List<FiPlayerInfo>();
					IEnumerator<PB_PlayerInfo> nEum = nGoldRes.Info.GetEnumerator ();
					while( nEum.MoveNext() )
					{
						FiPlayerInfo nProp = new FiPlayerInfo();
						nProp.gold = nEum.Current.Gold;
						nProp.point = nEum.Current.Point;
						nProp.userId = nEum.Current.UserId;
						nResult.info.Add( nProp );
					}
					mNetCtrl.dispatchEvent (FiEventType.RECV_PK_GOLD_GAME_RESULT_INFORM, nResult );

				}catch( Exception e )
				{
					Tool.OutLogToFile ("[ network ] recv message== FISHING_GOLD_GAME_RESULT error" + e.Message);
				}
				break;




			case FiProtoType.FISHING_POINT_GAME_RESULT:
				try
				{
					PB_PointGameResult  nPoint = PB_PointGameResult.Parser.ParseFrom( content );
					FiPointGameResult  nPoResult = new FiPointGameResult();
					nPoResult.winnerUserId = new List<int>();
					IEnumerator<int> nEum = nPoint.WinnerUserId.GetEnumerator ();
					while( nEum.MoveNext() )
					{
						nPoResult.winnerUserId.Add( nEum.Current );
					}
					mNetCtrl.dispatchEvent (FiEventType.RECV_PK_POINT_GAME_RESULT_INFORM, nPoResult );
				}catch( Exception e )
				{
					Tool.OutLogToFile ("[ network ] recv message== FISHING_POINT_GAME_RESULT error" + e.Message);
				}
				break;



			case FiProtoType.FISHING_POINT_GAME_ROUND_RESULT:
				try
				{
					PB_PointGameRoundResult  nPoint = PB_PointGameRoundResult.Parser.ParseFrom( content );
					FiPointGameRoundResult  nPoResult = new FiPointGameRoundResult();
					nPoResult.round = nPoint.Round;
					nPoResult.winnerUserId = nPoint.WinnerUserId;
	
					mNetCtrl.dispatchEvent (FiEventType.RECV_PK_POINT_GAME_ROUND_RESULT_INFORM, nPoResult );
				}catch( Exception e )
				{
					Tool.OutLogToFile ("[ network ] recv message== FISHING_POINT_GAME_ROUND_RESULT error" + e.Message);
				}
				break;





			case FiProtoType.FISHING_PK_EFFECT_RESPONSE:
				try
				{
					PB_EffectResponse nEffectRes = PB_EffectResponse.Parser.ParseFrom (content);
					mNetCtrl.dispatchEvent (FiEventType.RECV_PK_USE_EFFECT_RESPONSE, FiProtoHelper.toLocal_EffectResponse (nEffectRes));
				}catch( Exception e )
				{
					Tool.OutLogToFile ("[ network ] recv message== FISHING_PK_EFFECT_RESPONSE error" + e.Message);
				}
				break;





			case FiProtoType.FISHING_OTHER_PK_EFFECT:
				try
				{
					PB_OtherEffect nOthEffectRes = PB_OtherEffect.Parser.ParseFrom (content);
					mNetCtrl.dispatchEvent (FiEventType.RECV_PK_OTHER_EFFECT_INFORM, FiProtoHelper.toLocal_EffectInfrom (nOthEffectRes));
				}catch( Exception e )
				{
					Tool.OutLogToFile ("[ network ] recv message== FISHING_OTHER_PK_EFFECT error" + e.Message);
				}
				break;



			case FiProtoType.FISHING_RECONNECT_GAME_RESPONSE:
				try
				{
					
					PB_ReconnectGameResponse nRecnnect = PB_ReconnectGameResponse.Parser.ParseFrom( content );
					FiReconnectResponse nRecResponse = new FiReconnectResponse();
					nRecResponse.goldType = nRecnnect.GoldType;
					nRecResponse.roomIndex = nRecnnect.RoomIndex;
					nRecResponse.roomType  = nRecnnect.RoomType;
					nRecResponse.properties = new List<FiProperty>();

					IEnumerator<PB_Property> nEum = nRecnnect.Properties.GetEnumerator ();
					while( nEum.MoveNext() )
					{
						FiProperty nProp = new FiProperty();
						nProp.type = nEum.Current.PropertyType;
						nProp.value = nEum.Current.Sum;
						nRecResponse.properties.Add( nProp );
					}


					nRecResponse.others = new List<FiOtherGameInfo>();
					IEnumerator<PB_OtherGameInfo> nEumOther = nRecnnect.Others.GetEnumerator ();
					while( nEum.MoveNext() )
					{
						FiOtherGameInfo nInfoGame = new FiOtherGameInfo();
						nInfoGame.avatar = nEumOther.Current.Avatar;
						nInfoGame.gender = nEumOther.Current.Gender;
						nInfoGame.nickname = nEumOther.Current.Nickname;
						nInfoGame.seatIndex = nEumOther.Current.SeatIndex;
						nInfoGame.userId = nEumOther.Current.UserId;
						nInfoGame.vipLevel =nEumOther.Current.VipLevel;

						IEnumerator<PB_Property> nEumIn = nEumOther.Current.Properties.GetEnumerator ();
						while( nEum.MoveNext() )
						{
							FiProperty nPropIn = new FiProperty();
							nPropIn.type = nEumIn.Current.PropertyType;
							nPropIn.value = nEumIn.Current.Sum;
							nInfoGame.properties.Add( nPropIn );
						}
						nRecResponse.others.Add( nInfoGame );
					}
					mNetCtrl.dispatchEvent (FiEventType.RECV_RECONNECT_GAME_RESPONSE, nRecResponse);

				}catch( Exception e )
				{
					Tool.OutLogToFile ("[ network ] recv message== FISHING_RECONNECT_GAME_RESPONSE error" + e.Message);
				}
				break;


				//-------------
			case FiProtoType.FISHING_CREATE_FRIEND_ROOM_RESPONSE:
				{
					try{

					PB_CreateFriendRoomResponse nFriendIn = PB_CreateFriendRoomResponse.Parser.ParseFrom ( content );
					FiCreateFriendRoomResponse nValueRes = new FiCreateFriendRoomResponse ();
					nValueRes.result = nFriendIn.Result;
					nValueRes.seatIndex = nFriendIn.SeatIndex;
					if (nFriendIn.Room != null) {
						nValueRes.room = new FiPkRoomInfo ();
						nValueRes.room.begun = nFriendIn.Room.Begun;
						nValueRes.room.currentPlayerCount = nFriendIn.Room.CurrentPlayerNum;
						nValueRes.room.goldType = nFriendIn.Room.GoldType;
						nValueRes.room.roomIndex = nFriendIn.Room.RoomIndex;
						nValueRes.room.roomType = nFriendIn.Room.RoomType;;
						nValueRes.room.roundType = nFriendIn.Room.RoundType;
						nValueRes.room.timeType = nFriendIn.Room.TimeType;
					}
						mNetCtrl.dispatchEvent (FiEventType.RECV_CREATE_FRIEND_ROOM_RESPONSE, nValueRes);

					}catch( Exception e )
					{
						Tool.OutLogToFile ("[ network ] recv message== FISHING_CREATE_FRIEND_ROOM_RESPONSE error" + e.Message);
					}
				}
				break;



			case FiProtoType.FISHING_ENTER_FRIEND_ROOM_RESPONSE:
				{
					try{
						PB_EnterFriendRoomResponse nFriendIn = PB_EnterFriendRoomResponse.Parser.ParseFrom ( content );
						FiEnterFriendRoomResponse nValueRes = new FiEnterFriendRoomResponse ();

						nValueRes.others = new List<FiUserInfo>();

						nValueRes.result = nFriendIn.Result;
						nValueRes.seatIndex = nFriendIn.SeatIndex;
						nValueRes.roomOwnerId = nFriendIn.RoomOwnerUserId;


						nValueRes.room = new FiPkRoomInfo ();
						if (nFriendIn.Room != null) {
							nValueRes.room.begun = nFriendIn.Room.Begun;
							nValueRes.room.currentPlayerCount = nFriendIn.Room.CurrentPlayerNum;
							nValueRes.room.goldType = nFriendIn.Room.GoldType;
							nValueRes.room.roomIndex = nFriendIn.Room.RoomIndex;
							nValueRes.room.roomType = nFriendIn.Room.RoomType;;
							nValueRes.room.roundType = nFriendIn.Room.RoundType;
							nValueRes.room.timeType = nFriendIn.Room.TimeType;
						}


						IEnumerator<OtherUserInfo> nEumOther = nFriendIn.Others.GetEnumerator ();
						while( nEumOther.MoveNext() )
						{
							FiUserInfo nUserInfo = new FiUserInfo();
							nUserInfo.avatar = nEumOther.Current.Avatar;
							nUserInfo.cannonMultiple = nEumOther.Current.CurrentCannonRatio;
							nUserInfo.diamond = nEumOther.Current.Diamond;
							nUserInfo.experience = nEumOther.Current.Experience;
							nUserInfo.gender = nEumOther.Current.Gender;
							nUserInfo.gold = nEumOther.Current.Gold;
							nUserInfo.level = nEumOther.Current.Level;
							nUserInfo.maxCannonMultiple = nEumOther.Current.MaxCannonMultiple;
							nUserInfo.nickName = nEumOther.Current.Nickname;
							nUserInfo.prepared = nEumOther.Current.Prepared;
							nUserInfo.seatIndex = nEumOther.Current.SeatIndex;
							nUserInfo.userId = nEumOther.Current.UserId;
							nUserInfo.vipLevel = nEumOther.Current.VipLevel;
							nValueRes.others.Add( nUserInfo );
						}

						mNetCtrl.dispatchEvent (FiEventType.RECV_ENTER_FRIEND_ROOM_RESPONSE, nValueRes);

					}catch( Exception e )
					{
						Tool.OutLogToFile ("[ network ] recv message== FISHING_ENTER_FRIEND_ROOM_RESPONSE error" + e.Message);
					}
				}
				break;



			case FiProtoType.FISHING_NOTIFY_ENTER_FRIEND_ROOM:
				try{

					PB_NotifyOtherEnterFriendRoom nFriendIn = PB_NotifyOtherEnterFriendRoom.Parser.ParseFrom ( content );
					FiOtherEnterFriendRoomInform nValueRes = new FiOtherEnterFriendRoomInform ();

					FiUserInfo nUserInfo = new FiUserInfo();
					nValueRes.other = nUserInfo;
					nUserInfo.avatar = nFriendIn.Other.Avatar;
					nUserInfo.cannonMultiple = nFriendIn.Other.CurrentCannonRatio;
					nUserInfo.diamond = nFriendIn.Other.Diamond;
					nUserInfo.experience = nFriendIn.Other.Experience;
					nUserInfo.gender = nFriendIn.Other.Gender;
					nUserInfo.gold = nFriendIn.Other.Gold;
					nUserInfo.level = nFriendIn.Other.Level;
					nUserInfo.maxCannonMultiple = nFriendIn.Other.MaxCannonMultiple;
					nUserInfo.nickName = nFriendIn.Other.Nickname;
					nUserInfo.prepared = nFriendIn.Other.Prepared;
					nUserInfo.seatIndex = nFriendIn.Other.SeatIndex;
					nUserInfo.userId = nFriendIn.Other.UserId;
					nUserInfo.vipLevel = nFriendIn.Other.VipLevel;

					nValueRes.roomIndex = nFriendIn.RoomIndex;
					nValueRes.roomType = nFriendIn.RoomType;

					mNetCtrl.dispatchEvent (FiEventType.RECV_OTHER_ENTER_FRIEND_ROOM_INFORM, nValueRes);
				}catch( Exception e )
				{
					Tool.OutLogToFile ("[ network ] recv message== RECV_OTHER_ENTER_FRIEND_ROOM_INFORM error" + e.Message);
				}
				break;





			case FiProtoType.FISHING_NOTIFY_HAVE_DISCONNECTED_ROOM:
				mNetCtrl.dispatchEvent (FiEventType.RECV_HAVE_DISCONNECTED_ROOM_INFORM, new object());
				break;




			case FiProtoType.FISHING_FRIEND_ROOM_GAME_RESULT:
				{
					PB_FriendRoomGameResult nResultIn = PB_FriendRoomGameResult.Parser.ParseFrom ( content );
					FiFriendRoomGameResult nResultOut = new FiFriendRoomGameResult ();

					nResultOut.roomType = nResultIn.ResultType;
					nResultOut.users = new List<FiUserRoundResult> ();

					if (nResultIn.Users != null) 
					{
					
						IEnumerator<PB_UserRoundResult> nEumOther = nResultIn.Users.GetEnumerator ();
						while (nEumOther.MoveNext ()) 
						{
							FiUserRoundResult nRoundResult = new FiUserRoundResult ();
							nRoundResult.sum = nEumOther.Current.Sum;
							nRoundResult.userId = nEumOther.Current.UserId;
							nRoundResult.roundNum = new List<int> ();
							IEnumerator<int> nEumInt = nEumOther.Current.RoundNums.GetEnumerator ();
							while( nEumInt.MoveNext() )
							{
								nRoundResult.roundNum.Add (nEumInt.Current);
							}
							nResultOut.users.Add (nRoundResult);
						}
					}
					mNetCtrl.dispatchEvent ( FiEventType.RECV_FRIEND_ROOM_RESULT_INFORM , nResultOut );
				}
				break;




			case FiProtoType.FISHING_LEAVE_FRIEND_ROOM_RESPONSE:
				{
					try
					{
						PB_LeaveFriendRoomResponse nLeaveRes = PB_LeaveFriendRoomResponse.Parser.ParseFrom ( content );
						FiLeaveFriendRoomResponse nLeaveFriendOut = new FiLeaveFriendRoomResponse ();
						nLeaveFriendOut.result = nLeaveRes.Result;
						nLeaveFriendOut.roomIndex = nLeaveRes.RoomIndex;
						nLeaveFriendOut.roomType = nLeaveRes.RoomType;
						mNetCtrl.dispatchEvent (FiEventType.RECV_LEAVE_FRIEND_ROOM_RESPONSE, nLeaveFriendOut);

					}catch( Exception e )
					{
						Tool.OutLogToFile ("[ network ] recv message== RECV_OTHER_ENTER_FRIEND_ROOM_INFORM error" + e.Message);
					}
				}
				break;




			case FiProtoType.FISHING_NOTIFY_LEAVE_FRIEND_ROOM:
				{
					try
					{
						PB_NotifyOtherLeaveFriendRoom nLeaveRes = PB_NotifyOtherLeaveFriendRoom.Parser.ParseFrom ( content );
						FiOtherLeaveFriendRoomInform nLeaveFriendOut = new FiOtherLeaveFriendRoomInform ();
						nLeaveFriendOut.leaveUserId = nLeaveRes.LeaveUserId;
						nLeaveFriendOut.seatIndex   = nLeaveRes.SeatIndex;

						mNetCtrl.dispatchEvent (FiEventType.RECV_OTHER_LEAVE_FRIEND_ROOM_INFORM, nLeaveFriendOut);

					}catch( Exception e )
					{
						Tool.OutLogToFile ("[ network ] recv message== RECV_OTHER_ENTER_FRIEND_ROOM_INFORM error" + e.Message);
					}
				}
				break;





			case FiProtoType.FISHING_DISBAND_FRIEND_ROOM_RESPONSE:
				{
					try
					{
						PB_DisbandFriendRoomResponse nDisbandRes = PB_DisbandFriendRoomResponse.Parser.ParseFrom ( content );
						FiDisbandFriendRoomResponse  nDisbandOut = new FiDisbandFriendRoomResponse ();
						nDisbandOut.result = nDisbandRes.Result;
						nDisbandOut.roomIndex = nDisbandRes.RoomIndex;
						nDisbandOut.roomType = nDisbandRes.RoomType;



						mNetCtrl.dispatchEvent (FiEventType.RECV_DISBAND_FRIEND_ROOM_RESPONSE, nDisbandOut);

					}catch( Exception e )
					{
						Tool.OutLogToFile ("[ network ] recv message== FISHING_DISBAND_FRIEND_ROOM_RESPONSE error" + e.Message);
					}
				}
				break;



			case FiProtoType.FISHING_NOTIFY_DISBAND_FRIEND_ROOM:
				{
					try
					{
						PB_NotifyDisbandFriendRoom nDisbandRes = PB_NotifyDisbandFriendRoom.Parser.ParseFrom ( content );
						FiDisbandFriendRoomInform  nDisbandOut = new FiDisbandFriendRoomInform ();
	
						 
						nDisbandOut.roomIndex = nDisbandRes.RoomIndex;
						nDisbandOut.roomType = nDisbandRes.RoomType;

						mNetCtrl.dispatchEvent (FiEventType.RECV_DISBAND_FRIEND_ROOM_INFORM, nDisbandOut);

					}catch( Exception e )
					{
						Tool.OutLogToFile ("[ network ] recv message== FISHING_DISBAND_FRIEND_ROOM_RESPONSE error" + e.Message);
					}
				}
				break;


			case FiProtoType.FISHING_OPEN_RED_PACKET_RESPONSE:
				{
					try
					{
						PB_OpenRedPacketResponse nRedPacket = PB_OpenRedPacketResponse.Parser.ParseFrom( content );
						FiOpenRedPacketResponse  nRedpacketOut = new FiOpenRedPacketResponse();
						nRedpacketOut.gold = nRedPacket.Gold;
						nRedpacketOut.result = nRedPacket.Result;
						nRedpacketOut.roomIndex = nRedPacket.RoomIndex;
						nRedpacketOut.roomType = nRedPacket.RoomType;

						mNetCtrl.dispatchEvent( FiEventType.RECV_OPEN_REDPACKET_RESPONSE , nRedpacketOut );
					}catch( Exception e ) {
						Tool.OutLogToFile ("[ network ] recv message== FISHING_OPEN_RED_PACKET_RESPONSE error" + e.Message);
					}
				}
				break;





			case FiProtoType.FISHING_NOTIFY_RED_PACKET:
				{
					try
					{
						PB_NotifyRedPacket nRedPacket = PB_NotifyRedPacket.Parser.ParseFrom( content );
						FiRedPacketInform  nRedpacketOut = new FiRedPacketInform();

						nRedpacketOut.haveReward = nRedPacket.HaveReward;
						nRedpacketOut.roomIndex  = nRedpacketOut.roomIndex;
						nRedpacketOut.roomType   = nRedPacket.RoomType;

						mNetCtrl.dispatchEvent( FiEventType.RECV_REDPACKET_INFORM , nRedpacketOut );
					}catch( Exception e ) {
						Tool.OutLogToFile ("[ network ] recv message== FISHING_OPEN_RED_PACKET_RESPONSE error" + e.Message);
					}
				}
				break;






			case  FiProtoType.FISHING_NOTIFY_OTHER_OPEN_RED_PACKET:
				{
					try
					{
						PB_NotifyOtherOpenRedPacket nOtherOpen = PB_NotifyOtherOpenRedPacket.Parser.ParseFrom(content );
						FiOtherOpenRedPacketInform nOtherInform = new FiOtherOpenRedPacketInform();
						nOtherInform.gold = nOtherOpen.Gold;
						nOtherInform.userId = nOtherOpen.UserId;
						mNetCtrl.dispatchEvent( FiEventType.RECV_OTHER_OPEN_RED_PACKET_INFORM , nOtherInform );
					}catch( Exception e ) {
						Tool.OutLogToFile ("[ network ] recv message== FISHING_NOTIFY_OTHER_OPEN_RED_PACKET error" + e.Message);
					}
				}
				break;

		



			case FiProtoType.FISHING_ENTER_RED_PACKET_ROOM_RESPONSE:
				{
					try
					{
						PB_EnterRedPacketRoomResponse nResponseIn = PB_EnterRedPacketRoomResponse.Parser.ParseFrom(content );
						FiEnterRedPacketRoomResponse nResponseOut = new FiEnterRedPacketRoomResponse();
						nResponseOut.gold   = nResponseIn.Gold;
						nResponseOut.result = nResponseIn.Result;
						nResponseOut.roomConsumedGold = nResponseIn.RoomConsumedGold;
						nResponseOut.roomIndex      = nResponseIn.RoomIndex;
						nResponseOut.roomType       = nResponseIn.RoomType;
						nResponseOut.seatIndex      = nResponseIn.SeatIndex;
						nResponseOut.others = new List<FiUserInfo>();


						IEnumerator<OtherUserInfo> nEumOther = nResponseIn.Others.GetEnumerator();
						while( nEumOther.MoveNext() )
						{
							FiUserInfo nUserInfo = new FiUserInfo();
							nUserInfo.avatar = nEumOther.Current.Avatar;
							nUserInfo.cannonMultiple = nEumOther.Current.CurrentCannonRatio;
							nUserInfo.diamond = nEumOther.Current.Diamond;
							nUserInfo.experience = nEumOther.Current.Experience;
							nUserInfo.gender = nEumOther.Current.Gender;
							nUserInfo.gold = nEumOther.Current.Gold;
							nUserInfo.level = nEumOther.Current.Level;
							nUserInfo.maxCannonMultiple = nEumOther.Current.MaxCannonMultiple;
							nUserInfo.nickName = nEumOther.Current.Nickname;
							nUserInfo.prepared = nEumOther.Current.Prepared;
							nUserInfo.seatIndex = nEumOther.Current.SeatIndex;
							nUserInfo.userId = nEumOther.Current.UserId;
							nUserInfo.vipLevel = nEumOther.Current.VipLevel;
							nResponseOut.others.Add( nUserInfo );
						}

						mNetCtrl.dispatchEvent( FiEventType.RECV_ENTER_RED_PACKET_ROOM_RESPONSE , nResponseOut );
					}catch( Exception e ) {
						Tool.OutLogToFile ("[ network ] recv message== FISHING_NOTIFY_OTHER_OPEN_RED_PACKET error" + e.Message);
					}
				}
				break;




			case FiProtoType.FISHING_NOTIFY_ENTER_RED_PACKET_ROOM:
				{
					try
					{
						PB_NotifyOtherEnterRedPacketRoom nInformIn = PB_NotifyOtherEnterRedPacketRoom.Parser.ParseFrom ( content );
						FiOtherEnterRedPacketRoomInform nInformOut = new FiOtherEnterRedPacketRoomInform ();
						nInformOut.roomIndex = nInformIn.RoomIndex;
						nInformOut.roomType = nInformIn.RoomType;
						nInformOut.other = new FiUserInfo ();
						if (nInformIn.Other != null) {
							FiUserInfo nUserInfo = nInformOut.other;
							nUserInfo.avatar = nInformIn.Other.Avatar;
							nUserInfo.cannonMultiple = nInformIn.Other.CurrentCannonRatio;
							nUserInfo.diamond = nInformIn.Other.Diamond;
							nUserInfo.experience = nInformIn.Other.Experience;
							nUserInfo.gender = nInformIn.Other.Gender;
							nUserInfo.gold = nInformIn.Other.Gold;
							nUserInfo.level = nInformIn.Other.Level;
							nUserInfo.maxCannonMultiple = nInformIn.Other.MaxCannonMultiple;
							nUserInfo.nickName = nInformIn.Other.Nickname;
							nUserInfo.prepared = nInformIn.Other.Prepared;
							nUserInfo.seatIndex = nInformIn.Other.SeatIndex;
							nUserInfo.userId = nInformIn.Other.UserId;
							nUserInfo.vipLevel = nInformIn.Other.VipLevel;
						}
						mNetCtrl.dispatchEvent ( FiEventType.RECV_OTHER_ENTER_RED_PACKET_ROOM_INFORM , nInformOut );
					}
					catch( Exception e ) {
						Tool.OutLogToFile ("[ network ] recv message== FISHING_NOTIFY_ENTER_RED_PACKET_ROOM error" + e.Message);
					}
				}
				break;



			case FiProtoType.FISHING_LEAVE_RED_PACKET_ROOM_RESPONSE:
				{
					try
					{
						PB_LeaveRedPacketRoomResponse  nResponseIn = PB_LeaveRedPacketRoomResponse.Parser.ParseFrom( content );
						FiLeaveRedPacketRoomResponse   nResponseOut = new FiLeaveRedPacketRoomResponse();

						nResponseOut.gold = nResponseIn.Gold;
						nResponseOut.result = nResponseIn.Result;
						nResponseOut.roomIndex = nResponseIn.RoomIndex;

						mNetCtrl.dispatchEvent( FiEventType.RECV_LEAVE_RED_PACKET_ROOM_RESPONSE , nResponseOut );
					}catch( Exception e )
					{
						Tool.OutLogToFile ("[ network ] recv message== FISHING_LEAVE_RED_PACKET_ROOM_RESPONSE error" + e.Message);
					}
				}
				break;


			case FiProtoType.FISHING_NOTIFY_LEAVE_RED_PACKET_ROOM:
				{
					try
					{
						PB_NotifyOtherLeaveRedPacketRoom  nResponseIn = PB_NotifyOtherLeaveRedPacketRoom.Parser.ParseFrom( content );
						FiOtherLeaveRedPacketRoomInform   nResponseOut = new FiOtherLeaveRedPacketRoomInform();

						nResponseOut.seatIndex = nResponseIn.SeatIndex;
						nResponseOut.userId    = nResponseIn.UserId;
						mNetCtrl.dispatchEvent( FiEventType.RECV_OTHER_LEAVE_RED_PACKET_ROOM_INFORM , nResponseOut );

					}catch( Exception e )
					{
						Tool.OutLogToFile ("[ network ] recv message== FISHING_LEAVE_RED_PACKET_ROOM_RESPONSE error" + e.Message);
					}
				}
				break;


			case FiProtoType.FISHING_NOTIFY_FISH_TIDE_COMING:
				{
					mNetCtrl.dispatchEvent( FiEventType.RECV_FISH_TIDE_COMING_INFORM , new object() );
				}
				break;



			case FiProtoType.FISHING_NOTIFY_FISH_TIDE_CLEAN_FISH:
				{
					mNetCtrl.dispatchEvent( FiEventType.RECV_FISH_TIDE_CLEAN_INFORM , new object() );
				}
				break;


			case FiProtoType.FISHING_UNLOCK_CANNON_MULTIPLE_RESPONSE:
				{
					try
					{
						PB_UnlockCannonMultipleResponse nResponseIn = PB_UnlockCannonMultipleResponse.Parser.ParseFrom( content );
						FiUnlockCannonResponse nResponseOut = new FiUnlockCannonResponse();

						nResponseOut.currentMaxMultiple = nResponseIn.CurrentMaxMultiple;
						nResponseOut.result = nResponseIn.Result;
						nResponseOut.rewardGold = nResponseIn.RewardGold;

						mNetCtrl.dispatchEvent( FiEventType.RECV_UNLOCK_CANNON_MULTIPLE_RESPONSE , nResponseOut );

					}catch( Exception e )
					{
						Tool.OutLogToFile ("[ network ] recv message== FISHING_UNLOCK_CANNON_MULTIPLE_RESPONSE error" + e.Message);
					}
				}
				break;


			case FiProtoType.FISHING_GET_RANK_RESPONSE:
				{
					try
					{
						PB_GameRankResponse nResponseIn = PB_GameRankResponse.Parser.ParseFrom( content );
						FiGetRankResponse nResponseOut = new FiGetRankResponse();

						IEnumerator<PB_GameRank> nEumOther = nResponseIn.Rank.GetEnumerator();
						while( nEumOther.MoveNext() )
						{
							FiRankInfo nInfo = new FiRankInfo();
							nInfo.avatarUrl = nEumOther.Current.AvatarUrl;
							nInfo.gold      = nEumOther.Current.Gold;
							nInfo.nickname  = nEumOther.Current.Nickname;
							nInfo.userId    = nEumOther.Current.UserId;
							nInfo.vipLevel  = nEumOther.Current.Vip;

							nResponseOut.rankList.Add( nInfo );
						}

						mNetCtrl.dispatchEvent( FiEventType.RECV_GET_RANK_RESPONSE , nResponseOut );
					}catch( Exception e )
					{
						Tool.OutLogToFile ("[ network ] recv message== FISHING_UNLOCK_CANNON_MULTIPLE_RESPONSE error" + e.Message);
					}
				}
				break;


			}*/
		}




	}
}

