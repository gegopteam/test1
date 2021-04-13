using System;
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
	public class FiFishingMsgAgent: MsgBaseAgent
	{
		public FiFishingMsgAgent (NetControl nValue) : base ()
		{
			SetDispatch (nValue);
		}

		public void RecvOtherUnlockMutipleInform (byte[] data)
		{
			try {
				PB_OtherUnlockCannonMultiple nResponseIn = PB_OtherUnlockCannonMultiple.Parser.ParseFrom (data);
				FiOtherUnlockCannonMutipleInform nResponseOut = new FiOtherUnlockCannonMutipleInform ();
				nResponseOut.maxCannonMultiple = nResponseIn.MaxCannonMultiple;
				nResponseOut.userId = nResponseIn.UserId;
				nResponseOut.needDiamond = nResponseIn.NeedDiamond;
				nResponseOut.rewardGold = nResponseIn.RewardGold;
				Dispatch (FiEventType.RECV_OTHER_UNLOCK_CANNON_MUTIPLE_INFORM, nResponseOut);
			} catch (Exception e) {
				Debug.Log ("[ network ] recv message== RECV_OTHER_CHANGE_CANNON_STYLE_INFORM error" + e.Message);
			}
		}

		public void RecvOtherChangeCannonStyleInform (byte[] data)
		{
			try {
				PB_OtherChangeCannonStyle nResponseIn = PB_OtherChangeCannonStyle.Parser.ParseFrom (data);
				FiOtherChangeCannonStyleInform nResponseOut = new FiOtherChangeCannonStyleInform ();
				nResponseOut.currentCannonStyle = nResponseIn.CurrentCannonStyle;
				nResponseOut.userId = nResponseIn.UserId;

				Dispatch (FiEventType.RECV_OTHER_CHANGE_CANNON_STYLE_INFORM, nResponseOut);
			} catch (Exception e) {
				Debug.Log ("[ network ] recv message== RECV_OTHER_CHANGE_CANNON_STYLE_INFORM error" + e.Message);
			}
		}


		public void RecvChangeCannonStyleResponse (byte[] data)
		{
			try {
				PB_ChangeCannonStyleResponse nResponseIn = PB_ChangeCannonStyleResponse.Parser.ParseFrom (data);
				FiChangeCannonStyleResponse nResponseOut = new FiChangeCannonStyleResponse ();
				nResponseOut.currentCannonStyle = nResponseIn.CurrentCannonStyle;
				nResponseOut.result = nResponseIn.Result;

				Dispatch (FiEventType.RECV_CHANGE_CANNON_STYLE_RESPONSE, nResponseOut);
			} catch (Exception e) {
				Debug.Log ("[ network ] recv message== RECV_CHANGE_CANNON_STYLE_RESPONSE error" + e.Message);
			}
		}

		public void RecvPurchaseResponse (byte[] data)
		{
			try {
				PB_PurchasePropertyResponse nResponse = PB_PurchasePropertyResponse.Parser.ParseFrom (data);

				FiPurchasePropertyResponse nResponseOut = new FiPurchasePropertyResponse ();
				nResponseOut.diamondCost = nResponse.DiamondCost;
				if (nResponse.Property != null) {
					nResponseOut.info.type = nResponse.Property.PropertyType;
					nResponseOut.info.value = nResponse.Property.Sum;
				} else {
					nResponseOut.info = null;
				}
				nResponseOut.result = nResponse.Result;
				Dispatch (FiEventType.RECV_PURCHASE_PROPERTY_RESPONSE, nResponseOut);
			} catch (Exception e) {
				Debug.Log ("[ network ] recv message== FISHING_NOTIFY_FISH_OUT_OF_SCENE error" + e.Message);
			}
		}

		//移出渔场的数据处理
		public void RecvOutOfSence (byte[] data)
		{
			try {
				PB_FishOutScene nOutScene = PB_FishOutScene.Parser.ParseFrom (data);
				Dispatch (FiEventType.RECV_FISH_OUT_RESPONSE, FiProtoHelper.toLocal_FishOutReply (nOutScene));
			} catch (Exception e) {
				Debug.Log ("[ network ] recv message== FISHING_NOTIFY_FISH_OUT_OF_SCENE error" + e.Message);
			}
		}

		public void RecvClassicLeaveRoom (byte[] data)
		{
			try {
				PB_LeaveRoomResponse nLeaveRes = PB_LeaveRoomResponse.Parser.ParseFrom (data);
				Dispatch (FiEventType.RECV_USER_LEAVE_RESPONSE, FiProtoHelper.toLocal_LeaveReply (nLeaveRes));
			} catch (Exception e) {
				Debug.Log ("[ network ] recv message== FISHING_LEAVE_CLASSICAL_ROOM_RES error" + e.Message);
			}
		}

		public void RecvClassicOtherLeaveRoom (byte[] data)
		{
			try {
				PB_OtherLeaveRoomInfrom nOtherLeave = PB_OtherLeaveRoomInfrom.Parser.ParseFrom (data);   
				Dispatch (FiEventType.RECV_OTHER_LEAVE_ROOM_INFORM, FiProtoHelper.toLocal_OtherLeave (nOtherLeave));
			} catch (Exception e) {
				Debug.Log ("[ network ] recv message== FISHING_OTHER_LEAVE_ROOM_ error" + e.Message);
			}
		}

		public void RecvClassicOtherEnterRoom (byte[] data)
		{
			try {
				PB_OtherEnterRoomInfrom nOtherEnter = PB_OtherEnterRoomInfrom.Parser.ParseFrom (data);   
				mNetCtrl.dispatchEvent (FiEventType.RECV_OTHER_ENTER_ROOM_INFORM, FiProtoHelper.toLocal_OtherEnter (nOtherEnter));
			} catch (Exception e) {
				Debug.Log ("[ network ] recv message== FISHING_OTHER_ENTER_ROOM_INFORM error" + e.Message);
			}
		}

		public void RecvClassicEnterRoom (byte[] data)
		{
			try {
				PB_EnterRoomResponse nMatchResult = PB_EnterRoomResponse.Parser.ParseFrom (data);
				//Debug.Log ("[ network ] recv message== Others.Count" + nMatchResult.Others.Count + "---SeatIndex" + nMatchResult.SeatIndex);
				mNetCtrl.dispatchEvent (FiEventType.RECV_ROOM_MATCH_RESPONSE, FiProtoHelper.toLocal_MatchReply (nMatchResult));
			} catch (Exception e) {
				Debug.Log ("[ network ] recv message== FISHING_ENTER_CLASSICAL_ROOM_RES error" + e.Message);
			}
		}

		public void RecvOtherFire (byte[] data)
		{
			try {
				PB_UserFireRequest nRecvFire = PB_UserFireRequest.Parser.ParseFrom (data);
				//Debug.Log ("[ network ] recv user fire message== " + nRecvFire.BulletId + "/" + nRecvFire.UserId + "/" + nRecvFire.CannonRatio + "/" + nRecvFire.Position.X);
				mNetCtrl.dispatchEvent (FiEventType.RECV_OTHER_FIRE_BULLET_INFORM, FiProtoHelper.toLocal_OtherFireInform (nRecvFire));
			} catch (Exception e) {
				Debug.Log ("[ network ] recv message== FISHING_RECV_OTHER_FIRE_INFORM error" + e.Message);
			}
		}


		public void RecvOnFishHit (byte[] data)
		{
			try {
				PB_FishHitInfrom nHitInfom = PB_FishHitInfrom.Parser.ParseFrom (data);
				//Debug.Log ("[ network ] recv user FISHING_NOTIFY_ON_FISH_HIT== " + nHitInfom.BulletId + "/" + nHitInfom.FishId + "/" + nHitInfom.GroupId + "/" + nHitInfom.UserId);
				mNetCtrl.dispatchEvent (FiEventType.RECV_HITTED_FISH_RESPONSE, FiProtoHelper.toLocal_OtherHitFishInform (nHitInfom));
			} catch (Exception e) {
				Debug.Log ("[ network ] ========= FISHING_NOTIFY_ON_FISH_HIT error!!!!!!!!!! decoder error!!!" + e.Message);
			}
		}

        public void RecvOnRedpack(byte[] data)
        {
            Debug.Log("收到红包了~~~~~~~~~~~~~~");
            try
            {
                PB_FishHitInfrom nHitInfom = PB_FishHitInfrom.Parser.ParseFrom(data);
                //Debug.Log ("[ network ] recv user FISHING_NOTIFY_ON_FISH_HIT== " + nHitInfom.BulletId + "/" + nHitInfom.FishId + "/" + nHitInfom.GroupId + "/" + nHitInfom.UserId);
                mNetCtrl.dispatchEvent(FiEventType.RECV_XL_GET_HONG_BAO_GOLD, FiProtoHelper.toLocal_OtherHitFishInform(nHitInfom));
            }
            catch (Exception e)
            {
                Debug.Log("[ network ] ========= RECV_XL_GET_HONG_BAO_GOLD error!!!!!!!!!! decoder error!!!" + e.Message);
            }
        }

		public void RecvOnCreateFishGroup (byte[] data)
		{
			try {
				PB_FishGroupInfrom nGroupInfrom = PB_FishGroupInfrom.Parser.ParseFrom (data);
				//Debug.Log ("[ network ]!!!!!!!!!!!! recv create fishs == num" + nGroupInfrom.FishNum + "/ type = " + nGroupInfrom.FishType + " / GroupId =" + nGroupInfrom.GroupId + "/ TrackId =" + nGroupInfrom.TrackId + "/TrackType =" + nGroupInfrom.TrackType);
				mNetCtrl.dispatchEvent (FiEventType.RECV_FISHS_CREATED_INFORM, FiProtoHelper.toLocal_FishsCreatedInform (nGroupInfrom));
			} catch (Exception e) {
				Debug.Log ("[ network ] recv message== FISHING_NOTIFY_FISH_GROUP error" + e.Message); 
			}
		}

		public void RecvChangeConnonMultiple (byte[] data)
		{
			try {
				PB_ChangeCannonResponse nCannonRes = PB_ChangeCannonResponse.Parser.ParseFrom (data);
				mNetCtrl.dispatchEvent (FiEventType.RECV_CHANGE_CANNON_RESPONSE, FiProtoHelper.toLocal_CannonChangeResponse (nCannonRes));
			} catch (Exception e) {
				Debug.Log ("[ network ] recv message== FISHING_CHANGE_CANNON_MULTIPLE_RESPONSE error" + e.Message); 
			}
		}

		public void RecvOtherChangeConnonMultiple (byte[] data)
		{
			try {
				PB_OtherChangeCannon nOthCannonRes = PB_OtherChangeCannon.Parser.ParseFrom (data);
				mNetCtrl.dispatchEvent (FiEventType.RECV_OTHER_CHANGE_CANNON_INFORM, FiProtoHelper.toLocal_CannonChangeInform (nOthCannonRes));
			} catch (Exception e) {
				Debug.Log ("[ network ] recv message== FISHING_OTHER_CHANGE_CANNON_MULTIPLE error" + e.Message); 
			}
		}

		public void RecvEffectResponse (byte[] data)
		{
			try {
				PB_EffectResponse nEffectRes = PB_EffectResponse.Parser.ParseFrom (data);
				mNetCtrl.dispatchEvent (FiEventType.RECV_USE_EFFECT_RESPONSE, FiProtoHelper.toLocal_EffectResponse (nEffectRes));
			} catch (Exception e) {
				Debug.Log ("[ network ] recv message== FISHING_EFFECT_RESPONSE error" + e.Message); 
			}
		}

		public void RecvOtherEcffectInform (byte[] data)
		{
			try {
				PB_OtherEffect nOthEffectRes = PB_OtherEffect.Parser.ParseFrom (data);
				mNetCtrl.dispatchEvent (FiEventType.RECV_OTHER_EFFECT_INFORM, FiProtoHelper.toLocal_EffectInfrom (nOthEffectRes));
			} catch (Exception e) {
				Debug.Log ("[ network ] recv message== FISHING_OTHER_EFFECT error" + e.Message); 
			}
		}

		public void RecvFreezeTimeOut (byte[] data)
		{
			try {
				FreezeTimeout nFreezeInform = FreezeTimeout.Parser.ParseFrom (data);
				Dispatch (FiEventType.RECV_FISH_FREEZE_TIMEOUT_INFORM, FiProtoHelper.toLocal_FreezeInfrom (nFreezeInform));
			} catch (Exception e) {
				Debug.Log ("[ network ] recv message== FISHING_FREEZE_TIMEOUT error" + e.Message); 
			}
		}

		public void RecvPKStartGAMEResponse (byte[] data)
		{

			//Debug.LogError ( "---------------RecvPKStartGAMEResponse-----------------" );

			try {
				PB_StartPKGameResponse nPkRes = PB_StartPKGameResponse.Parser.ParseFrom (data);
				Dispatch (FiEventType.RECV_START_PK_GAME_RESPONSE, FiProtoHelper.toLocal_StartPkResponse (nPkRes));
			} catch (Exception e) {
				Debug.Log ("[ network ] recv message== FISHING_START_PKGAME_RESPONSE error" + e.Message); 
			}
		}

		public void RecvPKGameStartInform (byte[] data)
		{
			//Debug.LogError ( "---------------RecvPKGameStartInform-----------------" );

			try {
				PB_NotifyPKGameStart nPkStart = PB_NotifyPKGameStart.Parser.ParseFrom (data);
				mNetCtrl.dispatchEvent (FiEventType.RECV_START_PK_GAME_INFORM, FiProtoHelper.toLocal_OwnerStartPkResponse (nPkStart));
			} catch (Exception e) {
				Debug.Log ("[ network ] recv message== FISHING_START_PKGAME_RESPONSE error" + e.Message); 
			}
		}

		public void RecvPKEnterRoomResponse (byte[] data)
		{
			try {
				PB_EnterPKRoomResponse nEnterPkRes = PB_EnterPKRoomResponse.Parser.ParseFrom (data);
				mNetCtrl.dispatchEvent (FiEventType.RECV_ENTER_PK_ROOM_RESPONSE, FiProtoHelper.toLocal_EnterPkRoomResponse (nEnterPkRes));
			} catch (Exception e) {
				Debug.Log ("[ network ] recv message== FISHING_ENTER_PKROOM_RESPONSE error" + e.Message); 
			}
		}

		public void RecvPKOtherEnterRoomInform (byte[] data)
		{
			try {
				PB_NotifyOtherEnterPKRoom nOtherLeavePkRes = PB_NotifyOtherEnterPKRoom.Parser.ParseFrom (data);
				mNetCtrl.dispatchEvent (FiEventType.RECV_OTHER_ENTER_PK_ROOM_INFORM, FiProtoHelper.toLocal_OtherEnterPkRoom (nOtherLeavePkRes));
			} catch (Exception e) {
				Debug.Log ("[ network ] recv message== FISHING_NOTIFY_ENTER_PKROOM error" + e.Message); 
			}
		}

		public void RecvPKLeaveRoomResponse (byte[] data)
		{
			try {
				PB_LeavePKRoomResponse nLeavePkRes = PB_LeavePKRoomResponse.Parser.ParseFrom (data);
				mNetCtrl.dispatchEvent (FiEventType.RECV_LEAVE_PK_ROOM_RESPONSE, FiProtoHelper.toLocal_LeavePkRoomResponse (nLeavePkRes));
			} catch (Exception e) {
				Debug.Log ("[ network ] recv message== FISHING_LEAVE_PKROOM_RESPONSE error" + e.Message); 
			}
		}

		public void RecvPKOtherLeaveRoomInform (byte[] data)
		{
			try {
				PB_NotifyOtherLeavePKRoom nOtherLeavePkRes = PB_NotifyOtherLeavePKRoom.Parser.ParseFrom (data);
				mNetCtrl.dispatchEvent (FiEventType.RECV_OTHER_LEAVE_PK_ROOM_INFORM, FiProtoHelper.toLocal_OtherLeavePkRoomResponse (nOtherLeavePkRes));
			} catch (Exception e) {
				Debug.Log ("[ network ] recv message== FISHING_NOTIFY_LEAVE_PKROOM error" + e.Message); 
			}
		}

		public void RecvPKPrepareGame (byte[] data)
		{
			try {
				PB_PreparePKGame nPrepare = PB_PreparePKGame.Parser.ParseFrom (data);
				FiPreparePKGame nRecvPrepare = new FiPreparePKGame ();
				nRecvPrepare.roomIndex = nPrepare.RoomIndex;
				nRecvPrepare.roomType = nPrepare.RoomType;
				nRecvPrepare.userId = nPrepare.UserId;
				mNetCtrl.dispatchEvent (FiEventType.RECV_OTHER_PREPARE_PKGAME_INFORM, nRecvPrepare);
			} catch (Exception e) {
				Debug.Log ("[ network ] recv message== FISHING_PREPARE_PKGAME error" + e.Message); 
			}
		}


		public void RecvPKGameCountDown (byte[] data)
		{
			try {
				PB_PreStartCountdown nPreStart = PB_PreStartCountdown.Parser.ParseFrom (data);
				FiPkGameCountDownInform nCountInform = new FiPkGameCountDownInform ();
				nCountInform.countdown = nPreStart.Countdown;
				mNetCtrl.dispatchEvent (FiEventType.RECV_PKGAME_COUNTDOWN_INFORM, nCountInform);
			} catch (Exception e) {
				Tool.OutLogToFile ("[ network ] recv message== FISHING_PK_GAME_COUNTDOWN error" + e.Message);
			}
		}


		public void RecvPkCancelPrepareGame (byte[] data)
		{
			try {
				PB_CancelPreparePKGame nPrepareCancel = PB_CancelPreparePKGame.Parser.ParseFrom (data);
				FiCancelPreparePKGame nRecvCancel = new FiCancelPreparePKGame ();
				nRecvCancel.roomIndex = nPrepareCancel.RoomIndex;
				nRecvCancel.roomType = nPrepareCancel.RoomType;
				nRecvCancel.userId = nPrepareCancel.UserId;
				mNetCtrl.dispatchEvent (FiEventType.RECV_OTHER_CANCEL_PREPARE_PKGAME_INFORM, nRecvCancel);
			} catch (Exception e) {
				Debug.Log ("[ network ] recv message== FISHING_CANCEL_PREPARE_PKGAME error" + e.Message); 
			}
		}


		public void RecvPKPreStartCountDown (byte[] data)
		{
			try {

				PB_PreStartCountdown nCountDown = PB_PreStartCountdown.Parser.ParseFrom (data);
				FiPkGameCountDownInform nCountInform = new FiPkGameCountDownInform ();
				nCountInform.countdown = nCountDown.Countdown;
				mNetCtrl.dispatchEvent (FiEventType.RECV_PRE_PKGAME_COUNTDOWN_INFORM, nCountInform);

			} catch (Exception e) {
				Tool.OutLogToFile ("[ network ] recv message== FISHING_PRE_START_COUNTDOWN error" + e.Message);
			}
		}



		public void RecvClassicLaunchTorpedoResponse (byte[] data)
		{
			try {

				PB_LaunchTorpedoResponse nLauchRes = PB_LaunchTorpedoResponse.Parser.ParseFrom (data);
				FiLaunchTorpedoResponse nFiTorRes = new FiLaunchTorpedoResponse ();

				if (nLauchRes.Position != null) {
					nFiTorRes.position = new Cordinate ();
					nFiTorRes.position.x = nLauchRes.Position.X;
					nFiTorRes.position.y = nLauchRes.Position.Y;
				}
				nFiTorRes.result = nLauchRes.Result;
				nFiTorRes.torpedoId = nLauchRes.TorpedoId;
				nFiTorRes.torpedoType = nLauchRes.TorpedoType;

				//		Tool.addExMessage ("FISHING_LAUNCH_TORPEDO_RESPONSE" + nFiTorRes.result );
				mNetCtrl.dispatchEvent (FiEventType.RECV_LAUNCH_TORPEDO_RESPONSE, nFiTorRes);

			} catch (Exception e) {
				Tool.OutLogToFile ("[ network ] recv message== FISHING_LAUNCH_TORPEDO_RESPONSE error" + e.Message);
			}
		}

		public void RecvClassicOtherLaunchTorpedo (byte[] data)
		{
			try {

				PB_NotifyOtherLaunchTorpedo nOthLauchRes = PB_NotifyOtherLaunchTorpedo.Parser.ParseFrom (data);
				FiOtherLaunchTorpedoInform nFiTorInform = new FiOtherLaunchTorpedoInform ();

				if (nOthLauchRes.Position != null) {
					nFiTorInform.position = new Cordinate ();
					nFiTorInform.position.x = nOthLauchRes.Position.X;
					nFiTorInform.position.y = nOthLauchRes.Position.Y;
				}
				nFiTorInform.userId = nOthLauchRes.UserId;
				nFiTorInform.torpedoId = nOthLauchRes.TorpedoId;
				nFiTorInform.torpedoType = nOthLauchRes.TorpedoType;

				mNetCtrl.dispatchEvent (FiEventType.RECV_OTHER_LAUNCH_TORPEDO_INFORM, nFiTorInform);
			} catch (Exception e) {
				Tool.OutLogToFile ("[ network ] recv message== FISHING_NOTIFY_LAUNCH_TORPEDO error" + e.Message);
			}
		}

		public void RecvPKLaunchTorpedoResponse (byte[] data)
		{
			try {

				PB_LaunchTorpedoResponse nLauchRes = PB_LaunchTorpedoResponse.Parser.ParseFrom (data);
				FiLaunchTorpedoResponse nFiTorRes = new FiLaunchTorpedoResponse ();

				if (nLauchRes.Position != null) {
					nFiTorRes.position = new Cordinate ();
					nFiTorRes.position.x = nLauchRes.Position.X;
					nFiTorRes.position.y = nLauchRes.Position.Y;

					nFiTorRes.result = nLauchRes.Result;
					nFiTorRes.torpedoId = nLauchRes.TorpedoId;
					nFiTorRes.torpedoType = nLauchRes.TorpedoType;

					mNetCtrl.dispatchEvent (FiEventType.RECV_PK_LAUNCH_TORPEDO_RESPONSE, nFiTorRes);

				}
			} catch (Exception e) {
				Tool.OutLogToFile ("[ network ] recv message== FISHING_PK_LAUNCH_TORPEDO_RESPONSE error" + e.Message);
			}
		}

		public void RecvPKOtherOtherLaunchTorpedo (byte[] data)
		{
			try {

				PB_NotifyOtherLaunchTorpedo nOthLauchRes = PB_NotifyOtherLaunchTorpedo.Parser.ParseFrom (data);
				FiOtherLaunchTorpedoInform nFiTorInform = new FiOtherLaunchTorpedoInform ();

				if (nOthLauchRes.Position != null) {
					nFiTorInform.position = new Cordinate ();
					nFiTorInform.position.x = nOthLauchRes.Position.X;
					nFiTorInform.position.y = nOthLauchRes.Position.Y;
				}
				nFiTorInform.userId = nOthLauchRes.UserId;
				nFiTorInform.torpedoId = nOthLauchRes.TorpedoId;
				nFiTorInform.torpedoType = nOthLauchRes.TorpedoType;

				mNetCtrl.dispatchEvent (FiEventType.RECV_PK_OTHER_LAUNCH_TORPEDO_INFORM, nFiTorInform);
			} catch (Exception e) {
				Tool.OutLogToFile ("[ network ] recv message== FISHING_NOTIFY_PK_LAUNCH_TORPEDO error" + e.Message);
			}
		}

		public void RecvClassicTorpedoExplode (byte[] data)
		{
			try {
				PB_TorpedoExplodeResponse nExpRes = PB_TorpedoExplodeResponse.Parser.ParseFrom (data);
				FiTorpedoExplodeResponse nFiExp = new FiTorpedoExplodeResponse ();

				nFiExp.torpedoId = nExpRes.TorpedoId;
				nFiExp.torpedoType = nExpRes.TorpedoType;

				nFiExp.result = nExpRes.Result;
				nFiExp.rewards = new List<FiFishReward> ();

				if (nExpRes.Rewards != null) {
					IEnumerator<PB_FishReward> nEum = nExpRes.Rewards.GetEnumerator ();
					while (nEum.MoveNext ()) {
						FiFishReward nReward = new FiFishReward ();
						nReward.fishId = nEum.Current.FishId;
						nReward.groupId = nEum.Current.GroupId;
						nReward.properties = new List<FiProperty> ();

						if (nEum.Current.Properties != null) {
							IEnumerator<PB_Property> nEumProperty = nEum.Current.Properties.GetEnumerator ();

							while (nEumProperty.MoveNext ()) {
								FiProperty nSingP = new FiProperty ();
								nSingP.type = nEumProperty.Current.PropertyType;
								nSingP.value = nEumProperty.Current.Sum;
								nReward.properties.Add (nSingP);
							}
						}
						nFiExp.rewards.Add (nReward);
					}
				}
	
				mNetCtrl.dispatchEvent (FiEventType.RECV_TORPEDO_EXPLODE_RESPONSE, nFiExp);


			} catch (Exception e) {

				Tool.OutLogToFile ("[ network ] recv message== FISHING_TORPEDO_EXPLODE_RESPONSE error" + e.Message);
			}
		}

		public void RecvClassicOtherTorpedoExplode (byte[] data)
		{
			try {
				PB_NotifyOtherTorpedoExplode nExp = PB_NotifyOtherTorpedoExplode.Parser.ParseFrom (data);
				FiOtherTorpedoExplodeInform nOtherExp = new FiOtherTorpedoExplodeInform ();

				nOtherExp.rewards = new List<FiFishReward> ();
				nOtherExp.torpedoId = nExp.TorpedoId;
				nOtherExp.torpedoType = nExp.TorpedoType;
				nOtherExp.userId = nExp.UserId;

				if (nExp.Rewards != null) {
					IEnumerator<PB_FishReward> nEum = nExp.Rewards.GetEnumerator ();
					while (nEum.MoveNext ()) {
						FiFishReward nReward = new FiFishReward ();
						nReward.fishId = nEum.Current.FishId;
						nReward.groupId = nEum.Current.GroupId;
						nReward.properties = new List<FiProperty> ();

						IEnumerator<PB_Property> nEumProp = nEum.Current.Properties.GetEnumerator ();
						while (nEumProp.MoveNext ()) {
							FiProperty nSingP = new FiProperty ();
							nSingP.type = nEumProp.Current.PropertyType;
							nSingP.value = nEumProp.Current.Sum;
							nReward.properties.Add (nSingP);
						}
						nOtherExp.rewards.Add (nReward);
					}
				}
				mNetCtrl.dispatchEvent (FiEventType.RECV_OTHER_TORPEDO_EXPLODE_INFORM, nOtherExp);
			} catch (Exception e) {
				//		Tool.addExMessage ("FISHING_NOTIFY_TORPEDO_EXPLODE error");
				Tool.OutLogToFile ("[ network ] recv message== FISHING_NOTIFY_TORPEDO_EXPLODE error" + e.Message);
			}
		}

		public void RecvPKOtherTorpedoExplode (byte[] data)
		{
			try {
				PB_NotifyOtherTorpedoExplode nExp = PB_NotifyOtherTorpedoExplode.Parser.ParseFrom (data);
				FiOtherTorpedoExplodeInform nOtherExp = new FiOtherTorpedoExplodeInform ();
				nOtherExp.rewards = new List<FiFishReward> ();
				nOtherExp.torpedoId = nExp.TorpedoId;
				nOtherExp.torpedoType = nExp.TorpedoType;
				nOtherExp.userId = nExp.UserId;

				if (nExp.Rewards != null) {
					IEnumerator<PB_FishReward> nEum = nExp.Rewards.GetEnumerator ();
					while (nEum.MoveNext ()) {
						FiFishReward nReward = new FiFishReward ();
						nReward.fishId = nEum.Current.FishId;
						nReward.groupId = nEum.Current.GroupId;
						nReward.properties = new List<FiProperty> ();

						IEnumerator<PB_Property> nEumProp = nEum.Current.Properties.GetEnumerator ();
						while (nEumProp.MoveNext ()) {
							FiProperty nSingP = new FiProperty ();
							nSingP.type = nEumProp.Current.PropertyType;
							nSingP.value = nEumProp.Current.Sum;
							nReward.properties.Add (nSingP);
						}
						nOtherExp.rewards.Add (nReward);
					}
				}
				mNetCtrl.dispatchEvent (FiEventType.RECV_PK_OTHER_TORPEDO_EXPLODE_INFORM, nOtherExp);
			} catch (Exception e) {
				Tool.OutLogToFile ("[ network ] recv message== FISHING_NOTIFY_TORPEDO_EXPLODE error" + e.Message);
			}
		}

		public void RecvPKTorpedoExplodeResponse (byte[] data)
		{
			try {
				PB_TorpedoExplodeResponse nExpRes = PB_TorpedoExplodeResponse.Parser.ParseFrom (data);
				FiTorpedoExplodeResponse nFiExp = new FiTorpedoExplodeResponse ();
				nFiExp.result = nExpRes.Result;
				nFiExp.rewards = new System.Collections.Generic.List<FiFishReward> ();
				nFiExp.torpedoType = nExpRes.TorpedoType;
				nFiExp.torpedoId = nExpRes.TorpedoId;

				if (nExpRes.Rewards != null) {
					IEnumerator<PB_FishReward> nEum = nExpRes.Rewards.GetEnumerator ();
					while (nEum.MoveNext ()) {
						FiFishReward nReward = new FiFishReward ();
						nReward.fishId = nEum.Current.FishId;
						nReward.groupId = nEum.Current.GroupId;
						nReward.properties = new List<FiProperty> ();

						IEnumerator<PB_Property> nEumProp = nEum.Current.Properties.GetEnumerator ();
						while (nEumProp.MoveNext ()) {
							FiProperty nSingP = new FiProperty ();
							nSingP.type = nEumProp.Current.PropertyType;
							nSingP.value = nEumProp.Current.Sum;
							nReward.properties.Add (nSingP);
						}
						nFiExp.rewards.Add (nReward);
					}
				}
				mNetCtrl.dispatchEvent (FiEventType.RECV_PK_TORPEDO_EXPLODE_RESPONSE, nFiExp);


			} catch (Exception e) {
				Tool.OutLogToFile ("[ network ] recv message== FISHING_PK_TORPEDO_EXPLODE_RESPONSE error" + e.Message);
			}
		}

		public void RecvPKDistributeProperty (byte[] data)
		{
			try {
				PB_DistributePKProperty nDistribute = PB_DistributePKProperty.Parser.ParseFrom (data);
				FiDistributePKProperty nFiDis = new FiDistributePKProperty ();
				nFiDis.roomIndex = nDistribute.RoomIndex;
				nFiDis.properties = new List<FiProperty> ();

				IEnumerator<PB_Property> nEum = nDistribute.Properties.GetEnumerator ();
				while (nEum.MoveNext ()) {
					FiProperty nProp = new FiProperty ();
					nProp.type = nEum.Current.PropertyType;
					nProp.value = nEum.Current.Sum;
					nFiDis.properties.Add (nProp);
				}
				mNetCtrl.dispatchEvent (FiEventType.RECV_PK_DISTRIBUTE_PROPERTY_INFORM, nFiDis);
			} catch (Exception e) {
				Tool.OutLogToFile ("[ network ] recv message== FISHING_PK_DISTRIBUTE_PROPERTY error" + e.Message);
			}
		}

		public void RecvGoldGameResult (byte[] data)
		{
			try {

				PB_GoldGameResult nGoldRes = PB_GoldGameResult.Parser.ParseFrom (data);
				FiGoldGameResult nResult = new FiGoldGameResult ();
				nResult.info = new List<FiPlayerInfo> ();
				IEnumerator<PB_PlayerInfo> nEum = nGoldRes.Info.GetEnumerator ();
				while (nEum.MoveNext ()) {
					FiPlayerInfo nProp = new FiPlayerInfo ();
					nProp.gold = nEum.Current.Gold;
					nProp.point = nEum.Current.Point;
					nProp.userId = nEum.Current.UserId;
					nResult.info.Add (nProp);
				}
				mNetCtrl.dispatchEvent (FiEventType.RECV_PK_GOLD_GAME_RESULT_INFORM, nResult);

			} catch (Exception e) {
				Tool.OutLogToFile ("[ network ] recv message== FISHING_GOLD_GAME_RESULT error" + e.Message);
			}
		}

		public void RecvPointGameResult (byte[] data)
		{
			try {
				PB_PointGameResult nPoint = PB_PointGameResult.Parser.ParseFrom (data);
				FiPointGameResult nPoResult = new FiPointGameResult ();
				nPoResult.winnerUserId = new List<int> ();
				IEnumerator<int> nEum = nPoint.WinnerUserId.GetEnumerator ();
				while (nEum.MoveNext ()) {
					nPoResult.winnerUserId.Add (nEum.Current);
				}
				mNetCtrl.dispatchEvent (FiEventType.RECV_PK_POINT_GAME_RESULT_INFORM, nPoResult);
			} catch (Exception e) {
				Tool.OutLogToFile ("[ network ] recv message== FISHING_POINT_GAME_RESULT error" + e.Message);
			}
		}

		public void RecvPointGameRoundResult (byte[] data)
		{
			try {
				PB_PointGameRoundResult nPoint = PB_PointGameRoundResult.Parser.ParseFrom (data);
				FiPointGameRoundResult nPoResult = new FiPointGameRoundResult ();
				nPoResult.round = nPoint.Round;
				nPoResult.winnerUserId = nPoint.WinnerUserId;

				mNetCtrl.dispatchEvent (FiEventType.RECV_PK_POINT_GAME_ROUND_RESULT_INFORM, nPoResult);
			} catch (Exception e) {
				Tool.OutLogToFile ("[ network ] recv message== FISHING_POINT_GAME_ROUND_RESULT error" + e.Message);
			}
		}

		public void RecvPKEffectResponse (byte[] data)
		{
			try {
				PB_EffectResponse nEffectRes = PB_EffectResponse.Parser.ParseFrom (data);
				mNetCtrl.dispatchEvent (FiEventType.RECV_PK_USE_EFFECT_RESPONSE, FiProtoHelper.toLocal_EffectResponse (nEffectRes));
			} catch (Exception e) {
				Tool.OutLogToFile ("[ network ] recv message== FISHING_PK_EFFECT_RESPONSE error" + e.Message);
			}
		}

		public void RecvPKOtherEffect (byte[] data)
		{
			try {
				PB_OtherEffect nOthEffectRes = PB_OtherEffect.Parser.ParseFrom (data);
				mNetCtrl.dispatchEvent (FiEventType.RECV_PK_OTHER_EFFECT_INFORM, FiProtoHelper.toLocal_EffectInfrom (nOthEffectRes));
			} catch (Exception e) {
				Tool.OutLogToFile ("[ network ] recv message== FISHING_OTHER_PK_EFFECT error" + e.Message);
			}
		}

		public void RecvPKCreateFriendRoomResponse (byte[] data)
		{
			try {

				PB_CreateFriendRoomResponse nFriendIn = PB_CreateFriendRoomResponse.Parser.ParseFrom (data);
				FiCreateFriendRoomResponse nValueRes = new FiCreateFriendRoomResponse ();
				nValueRes.result = nFriendIn.Result;
				nValueRes.seatIndex = nFriendIn.SeatIndex;
				if (nFriendIn.Room != null) {
					nValueRes.room = new FiPkRoomInfo ();
					nValueRes.room.begun = nFriendIn.Room.Begun;
					nValueRes.room.currentPlayerCount = nFriendIn.Room.CurrentPlayerNum;
					nValueRes.room.goldType = nFriendIn.Room.GoldType;
					nValueRes.room.roomIndex = nFriendIn.Room.RoomIndex;
					nValueRes.room.roomType = nFriendIn.Room.RoomType;
					;
					nValueRes.room.roundType = nFriendIn.Room.RoundType;
					nValueRes.room.timeType = nFriendIn.Room.TimeType;
				}
				mNetCtrl.dispatchEvent (FiEventType.RECV_CREATE_FRIEND_ROOM_RESPONSE, nValueRes);

			} catch (Exception e) {
				Tool.OutLogToFile ("[ network ] recv message== FISHING_CREATE_FRIEND_ROOM_RESPONSE error" + e.Message);
			}
		}


		public void RecvPKEnterFriendRoomResponse (byte[] data)
		{
			try {
				PB_EnterFriendRoomResponse nFriendIn = PB_EnterFriendRoomResponse.Parser.ParseFrom (data);
				FiEnterFriendRoomResponse nValueRes = new FiEnterFriendRoomResponse ();

				nValueRes.others = new List<FiUserInfo> ();

				nValueRes.result = nFriendIn.Result;
				nValueRes.seatIndex = nFriendIn.SeatIndex;
				nValueRes.roomOwnerId = nFriendIn.RoomOwnerUserId;


				nValueRes.room = new FiPkRoomInfo ();
				if (nFriendIn.Room != null) {
					nValueRes.room.begun = nFriendIn.Room.Begun;
					nValueRes.room.currentPlayerCount = nFriendIn.Room.CurrentPlayerNum;
					nValueRes.room.goldType = nFriendIn.Room.GoldType;
					nValueRes.room.roomIndex = nFriendIn.Room.RoomIndex;
					nValueRes.room.roomType = nFriendIn.Room.RoomType;
					;
					nValueRes.room.roundType = nFriendIn.Room.RoundType;
					nValueRes.room.timeType = nFriendIn.Room.TimeType;
				}


				IEnumerator<OtherUserInfo> nEumOther = nFriendIn.Others.GetEnumerator ();
				while (nEumOther.MoveNext ()) {
					FiUserInfo nUserInfo = new FiUserInfo ();
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
					nUserInfo.cannonStyle = nEumOther.Current.CannonStyle;
					nUserInfo.gameId = nEumOther.Current.GameId;
					nValueRes.others.Add (nUserInfo);
				}

				mNetCtrl.dispatchEvent (FiEventType.RECV_ENTER_FRIEND_ROOM_RESPONSE, nValueRes);

			} catch (Exception e) {
				Tool.OutLogToFile ("[ network ] recv message== FISHING_ENTER_FRIEND_ROOM_RESPONSE error" + e.Message);
			}
		}

		public void RecvPKOtherEnterFriendRoom (byte[] data)
		{
			try {

				PB_NotifyOtherEnterFriendRoom nFriendIn = PB_NotifyOtherEnterFriendRoom.Parser.ParseFrom (data);
				FiOtherEnterFriendRoomInform nValueRes = new FiOtherEnterFriendRoomInform ();

				FiUserInfo nUserInfo = new FiUserInfo ();
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
				nUserInfo.cannonStyle = nFriendIn.Other.CannonStyle;

				nValueRes.roomIndex = nFriendIn.RoomIndex;
				nValueRes.roomType = nFriendIn.RoomType;

				mNetCtrl.dispatchEvent (FiEventType.RECV_OTHER_ENTER_FRIEND_ROOM_INFORM, nValueRes);
			} catch (Exception e) {
				Tool.OutLogToFile ("[ network ] recv message== RECV_OTHER_ENTER_FRIEND_ROOM_INFORM error" + e.Message);
			}
		}

		public void RecvPKHaveDisconnectRoomInform (byte[] data)
		{
			mNetCtrl.dispatchEvent (FiEventType.RECV_HAVE_DISCONNECTED_ROOM_INFORM, new object ());
		}

		public void RecvPKFriendRoomGameResult (byte[] data)
		{
			PB_FriendRoomGameResult nResultIn = PB_FriendRoomGameResult.Parser.ParseFrom (data);
			FiFriendRoomGameResult nResultOut = new FiFriendRoomGameResult ();

			nResultOut.roomType = nResultIn.ResultType;
			nResultOut.users = new List<FiUserRoundResult> ();

			if (nResultIn.Users != null) {

				IEnumerator<PB_UserRoundResult> nEumOther = nResultIn.Users.GetEnumerator ();
				while (nEumOther.MoveNext ()) {
					FiUserRoundResult nRoundResult = new FiUserRoundResult ();
					nRoundResult.sum = nEumOther.Current.Sum;
					nRoundResult.userId = nEumOther.Current.UserId;
					nRoundResult.roundNum = new List<int> ();
					IEnumerator<int> nEumInt = nEumOther.Current.RoundNums.GetEnumerator ();
					while (nEumInt.MoveNext ()) {
						nRoundResult.roundNum.Add (nEumInt.Current);
					}
					nResultOut.users.Add (nRoundResult);
				}
			}
			mNetCtrl.dispatchEvent (FiEventType.RECV_FRIEND_ROOM_RESULT_INFORM, nResultOut);

		}

		public void RecvPKLeaveFriendRoom (byte[] data)
		{
			try {
				PB_LeaveFriendRoomResponse nLeaveRes = PB_LeaveFriendRoomResponse.Parser.ParseFrom (data);
				FiLeaveFriendRoomResponse nLeaveFriendOut = new FiLeaveFriendRoomResponse ();
				nLeaveFriendOut.result = nLeaveRes.Result;
				nLeaveFriendOut.roomIndex = nLeaveRes.RoomIndex;
				nLeaveFriendOut.roomType = nLeaveRes.RoomType;
				mNetCtrl.dispatchEvent (FiEventType.RECV_LEAVE_FRIEND_ROOM_RESPONSE, nLeaveFriendOut);

			} catch (Exception e) {
				Tool.OutLogToFile ("[ network ] recv message== RECV_OTHER_ENTER_FRIEND_ROOM_INFORM error" + e.Message);
			}
		}

		public void RecvPKOtherLeaveFriendRoom (byte[] data)
		{
			try {
				PB_NotifyOtherLeaveFriendRoom nLeaveRes = PB_NotifyOtherLeaveFriendRoom.Parser.ParseFrom (data);
				FiOtherLeaveFriendRoomInform nLeaveFriendOut = new FiOtherLeaveFriendRoomInform ();
				nLeaveFriendOut.leaveUserId = nLeaveRes.LeaveUserId;
				nLeaveFriendOut.seatIndex = nLeaveRes.SeatIndex;

				mNetCtrl.dispatchEvent (FiEventType.RECV_OTHER_LEAVE_FRIEND_ROOM_INFORM, nLeaveFriendOut);

			} catch (Exception e) {
				Tool.OutLogToFile ("[ network ] recv message== RECV_OTHER_ENTER_FRIEND_ROOM_INFORM error" + e.Message);
			}
		}

		public void RecvPKDisbandFriendRoomResponse (byte[] data)
		{
			try {
				PB_DisbandFriendRoomResponse nDisbandRes = PB_DisbandFriendRoomResponse.Parser.ParseFrom (data);
				FiDisbandFriendRoomResponse nDisbandOut = new FiDisbandFriendRoomResponse ();
				nDisbandOut.result = nDisbandRes.Result;
				nDisbandOut.roomIndex = nDisbandRes.RoomIndex;
				nDisbandOut.roomType = nDisbandRes.RoomType;

				mNetCtrl.dispatchEvent (FiEventType.RECV_DISBAND_FRIEND_ROOM_RESPONSE, nDisbandOut);
			} catch (Exception e) {
				Tool.OutLogToFile ("[ network ] recv message== FISHING_DISBAND_FRIEND_ROOM_RESPONSE error" + e.Message);
			}
		}

		public void RecvPKDisbandFriendRoomInform (byte[] data)
		{
			try {
				PB_NotifyDisbandFriendRoom nDisbandRes = PB_NotifyDisbandFriendRoom.Parser.ParseFrom (data);
				FiDisbandFriendRoomInform nDisbandOut = new FiDisbandFriendRoomInform ();

				nDisbandOut.roomIndex = nDisbandRes.RoomIndex;
				nDisbandOut.roomType = nDisbandRes.RoomType;
				mNetCtrl.dispatchEvent (FiEventType.RECV_DISBAND_FRIEND_ROOM_INFORM, nDisbandOut);
			} catch (Exception e) {
				Tool.OutLogToFile ("[ network ] recv message== FISHING_DISBAND_FRIEND_ROOM_RESPONSE error" + e.Message);
			}
		}

		public void RecvGetRedPacketListResponse (byte[] data)
		{
			try {
				PB_GetRedPacketListResponse nResponseIn = PB_GetRedPacketListResponse.Parser.ParseFrom (data);
				FiGetRedPacketListResponse nResponseOut = new FiGetRedPacketListResponse ();
				nResponseOut.result = nResponseIn.Result;

				IEnumerator<long> nEum = nResponseIn.Packets.GetEnumerator ();
				while (nEum.MoveNext ()) {
					nResponseOut.packets.Add (nEum.Current);	
				}
				Dispatch (FiEventType.RECV_GET_RED_PACKET_LIST_RESPONSE, nResponseOut);
			} catch (Exception e) {
				Debug.Log ("[ network ] recv message== RECV_OTHER_CHANGE_CANNON_STYLE_INFORM error" + e.Message);
			}
		}

		public void RecvPKRedPacketCountDown (byte[] data)
		{
			try {
				PB_RedPacketDistributionCountdown nRedPacket = PB_RedPacketDistributionCountdown.Parser.ParseFrom (data);
				FiRedPacketDistributionCountdown nRedpacketOut = new FiRedPacketDistributionCountdown ();

				nRedpacketOut.countdown = nRedPacket.Countdown;


				mNetCtrl.dispatchEvent (FiEventType.RECV_RED_PACKET_COUNTDOWN_INFORM, nRedpacketOut);
			} catch (Exception e) {
				Tool.OutLogToFile ("[ network ] recv message== RECV_RED_PACKET_COUNTDOWN_INFORM error" + e.Message);
			}
		}

		public void RecvPKOpenRedPacketResponse (byte[] data)
		{
			try {
				PB_OpenRedPacketResponse nRedPacket = PB_OpenRedPacketResponse.Parser.ParseFrom (data);
				FiOpenRedPacketResponse nRedpacketOut = new FiOpenRedPacketResponse ();
				nRedpacketOut.redPacketTicket = nRedPacket.RedPacketTicket;
				nRedpacketOut.result = nRedPacket.Result;

				nRedpacketOut.packetId = nRedPacket.PacketId;
				mNetCtrl.dispatchEvent (FiEventType.RECV_OPEN_REDPACKET_RESPONSE, nRedpacketOut);
			} catch (Exception e) {
				Tool.OutLogToFile ("[ network ] recv message== FISHING_OPEN_RED_PACKET_RESPONSE error" + e.Message);
			}
		}

		public void RecvPKNotifyRedPacket (byte[] data)
		{
			try {
				PB_NotifyRedPacket nRedPacket = PB_NotifyRedPacket.Parser.ParseFrom (data);
				FiRedPacketInform nRedpacketOut = new FiRedPacketInform ();
				nRedpacketOut.packetId = nRedPacket.PacketId;
				nRedpacketOut.consumedGold = nRedPacket.ConsumedGold;
				mNetCtrl.dispatchEvent (FiEventType.RECV_REDPACKET_INFORM, nRedpacketOut);
			} catch (Exception e) {
				Tool.OutLogToFile ("[ network ] recv message== FISHING_OPEN_RED_PACKET_RESPONSE error" + e.Message);
			}
		}


		public void RecvPKOtherOpenRedPacket (byte[] data)
		{
			try {
				PB_NotifyOtherOpenRedPacket nOtherOpen = PB_NotifyOtherOpenRedPacket.Parser.ParseFrom (data);
				FiOtherOpenRedPacketInform nOtherInform = new FiOtherOpenRedPacketInform ();
				nOtherInform.redPacketTicket = nOtherOpen.RedPacketTicket;
				nOtherInform.userId = nOtherOpen.UserId;
				mNetCtrl.dispatchEvent (FiEventType.RECV_OTHER_OPEN_RED_PACKET_INFORM, nOtherInform);
			} catch (Exception e) {
				Tool.OutLogToFile ("[ network ] recv message== FISHING_NOTIFY_OTHER_OPEN_RED_PACKET error" + e.Message);
			}
		}

		public void RecvPKEnterRedPacketRoom (byte[] data)
		{
			try {
				PB_EnterRedPacketRoomResponse nResponseIn = PB_EnterRedPacketRoomResponse.Parser.ParseFrom (data);
				FiEnterRedPacketRoomResponse nResponseOut = new FiEnterRedPacketRoomResponse ();

				nResponseOut.gold = nResponseIn.Gold;
				nResponseOut.result = nResponseIn.Result;
				nResponseOut.roomConsumedGold = nResponseIn.RoomConsumedGold;
				nResponseOut.roomIndex = nResponseIn.RoomIndex;
				nResponseOut.roomType = nResponseIn.RoomType;
				nResponseOut.seatIndex = nResponseIn.SeatIndex;
				nResponseOut.others = new List<FiUserInfo> ();


				IEnumerator<OtherUserInfo> nEumOther = nResponseIn.Others.GetEnumerator ();
				while (nEumOther.MoveNext ()) {
					FiUserInfo nUserInfo = new FiUserInfo ();
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
					nUserInfo.cannonStyle = nEumOther.Current.CannonStyle;
					nResponseOut.others.Add (nUserInfo);
				}

				mNetCtrl.dispatchEvent (FiEventType.RECV_ENTER_RED_PACKET_ROOM_RESPONSE, nResponseOut);
			} catch (Exception e) {
				Tool.OutLogToFile ("[ network ] recv message== FISHING_NOTIFY_OTHER_OPEN_RED_PACKET error" + e.Message);
			}
		}

		public void RecvPKOtherEnterRedPacketRoom (byte[] data)
		{
			try {
				PB_NotifyOtherEnterRedPacketRoom nInformIn = PB_NotifyOtherEnterRedPacketRoom.Parser.ParseFrom (data);
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
					nUserInfo.cannonStyle = nInformIn.Other.CannonStyle;
				}
				mNetCtrl.dispatchEvent (FiEventType.RECV_OTHER_ENTER_RED_PACKET_ROOM_INFORM, nInformOut);
			} catch (Exception e) {
				Tool.OutLogToFile ("[ network ] recv message== FISHING_NOTIFY_ENTER_RED_PACKET_ROOM error" + e.Message);
			}
		}


		public void RecvPKLeaveRedPacketRoom (byte[] data)
		{
			try {
				PB_LeaveRedPacketRoomResponse nResponseIn = PB_LeaveRedPacketRoomResponse.Parser.ParseFrom (data);
				FiLeaveRedPacketRoomResponse nResponseOut = new FiLeaveRedPacketRoomResponse ();

				nResponseOut.gold = nResponseIn.Gold;
				nResponseOut.result = nResponseIn.Result;
				nResponseOut.roomIndex = nResponseIn.RoomIndex;

				mNetCtrl.dispatchEvent (FiEventType.RECV_LEAVE_RED_PACKET_ROOM_RESPONSE, nResponseOut);
			} catch (Exception e) {
				Tool.OutLogToFile ("[ network ] recv message== FISHING_LEAVE_RED_PACKET_ROOM_RESPONSE error" + e.Message);
			}
		}


		public void RecvPKOtherLeaveRedPacketRoom (byte[] data)
		{
			try {
				PB_NotifyOtherLeaveRedPacketRoom nResponseIn = PB_NotifyOtherLeaveRedPacketRoom.Parser.ParseFrom (data);
				FiOtherLeaveRedPacketRoomInform nResponseOut = new FiOtherLeaveRedPacketRoomInform ();

				nResponseOut.seatIndex = nResponseIn.SeatIndex;
				nResponseOut.userId = nResponseIn.UserId;
				mNetCtrl.dispatchEvent (FiEventType.RECV_OTHER_LEAVE_RED_PACKET_ROOM_INFORM, nResponseOut);

			} catch (Exception e) {
				Tool.OutLogToFile ("[ network ] recv message== FISHING_LEAVE_RED_PACKET_ROOM_RESPONSE error" + e.Message);
			}
		}

		public void RecvFishTideComing (byte[] data)
		{
			mNetCtrl.dispatchEvent (FiEventType.RECV_FISH_TIDE_COMING_INFORM, new object ());
		}

		public void RecvFishTideCleanFish (byte[] data)
		{
			mNetCtrl.dispatchEvent (FiEventType.RECV_FISH_TIDE_CLEAN_INFORM, new object ());
		}

		public void RecvUnlockCannonMultiple (byte[] data)
		{
			try {
				PB_UnlockCannonMultipleResponse nResponseIn = PB_UnlockCannonMultipleResponse.Parser.ParseFrom (data);
				FiUnlockCannonResponse nResponseOut = new FiUnlockCannonResponse ();

				nResponseOut.currentMaxMultiple = nResponseIn.CurrentMaxMultiple;
				nResponseOut.result = nResponseIn.Result;
				nResponseOut.rewardGold = nResponseIn.RewardGold;
				nResponseOut.needDiamond = nResponseIn.NeedDiamond;

				mNetCtrl.dispatchEvent (FiEventType.RECV_UNLOCK_CANNON_MULTIPLE_RESPONSE, nResponseOut);

			} catch (Exception e) {
				Tool.OutLogToFile ("[ network ] recv message== FISHING_UNLOCK_CANNON_MULTIPLE_RESPONSE error" + e.Message);
			}
		}

		public void RecvChangeUserGold (byte[] data)
		{
			try {
				ChangeUserGold userGold = ChangeUserGold.Parser.ParseFrom (data);
				//Debug.Log ("[ network ]!!!!!!!!!!!! recv create fishs == num" + nGroupInfrom.FishNum + "/ type = " + nGroupInfrom.FishType + " / GroupId =" + nGroupInfrom.GroupId + "/ TrackId =" + nGroupInfrom.TrackId + "/TrackType =" + nGroupInfrom.TrackType);
				mNetCtrl.dispatchEvent (FiEventType.RECV_XL_CHANGEUSERGOLD_RESPOSE, FiProtoHelper.toLocal_ChangeUserGold (userGold));
			} catch (Exception e) {
				Debug.Log ("[ network ] recv message== RecvChangeUserGold error" + e.Message); 
			}
		}


		public void RecvUpdateBossMatchTime (byte[] data)
		{
			try {
				UpdateBossMatchTime updateBossMatchTime = UpdateBossMatchTime.Parser.ParseFrom (data);
				mNetCtrl.dispatchEvent (FiEventType.RECV_XL_BOSSMATCHTIME_RESPOSE, FiProtoHelper.toLocal_UpdateBossMatchTime (updateBossMatchTime));
			} catch (Exception ex) {
				Debug.Log ("[ network ] recv message== RecvGetBossMatchTime error" + ex.Message);
			}
		}
	}
}

