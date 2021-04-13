using System;
using UnityEngine;
using System.Threading;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using Google.Protobuf;

/*
 *  2020/03/04 Joey 增加新手等級提升獎勵跑馬燈消息 @OnInit, @OnDestroy
 */

namespace AssemblyCSharp
{
	public class BroadCastMsgHandle :IMsgHandle
	{
		public string Newname;

		public BroadCastMsgHandle ()
		{
		}
		//发送公告
		public void SendBroadCastMessage (string nContent)
		{
			if (string.IsNullOrEmpty (nContent))
				return;
			FiBroadCastUserMsgRequest nRequest = new FiBroadCastUserMsgRequest ();
			nRequest.content = nContent;
			DataControl.GetInstance ().PushSocketSndByte (FiEventType.SEND_BROADCAST_USER_MESSAGE_REQUEST, nRequest.serialize ());
		}
		//		public void SendChangeNameMessage( string nContent )
		//		{
		//
		//			FiBroadCastUserMsgRequest nRequest = new FiBroadCastUserMsgRequest ();
		//			nRequest.content = nContent;
		//			DataControl.GetInstance ().PushSocketSndByte ( FiEventType.SEND_BROADCAST_USER_MESSAGE_REQUEST , nRequest.serialize() );
		//		}
		public void SendChangeNameRequset (string name, int userid, int logtype)
		{
			FiModifyNick nRequest = new FiModifyNick ();
			nRequest.loginType = logtype;
			nRequest.userID = userid;
			nRequest.modifyNick = name;
			//ByteString modifyTmp = "哈哈";
			//byte[] byteArray = System.Text.Encoding.Default.GetBytes (name);
			//nRequest.modifyNick = Google.Protobuf.ByteString.CopyFrom (byteArray);
			//Google.Protobuf.ByteString.CopyFromUtf8
			nRequest.propCount = 1;
			nRequest.propID = 2;
			Newname = name;
			//Debug.LogError (logtype + "s" + userid + "s" + name);
			DataControl.GetInstance ().PushSocketSnd (FiEventType.SEND_CL_MODIFY_NICK_REQUEST, nRequest);
			//Debug.LogError ("SEND_CL_MODIFY_NICK_REQUEST");
		}

		public void OnInit ()
		{
			EventControl nControl = EventControl.instance ();
			nControl.addEventHandler (FiEventType.RECV_BROADCAST_USER_MESSAGE_RESPONSE, RecvBroadCastResponse);
			nControl.addEventHandler (FiEventType.RECV_NOTIFY_BROADCAST_GAME_INFO, RecvBroadCastGameInfo);
			nControl.addEventHandler (FiEventType.RECV_NOTIFY_BROADCAST_USER_MESSAGE, RecvBroadCastUserMessage);
			nControl.addEventHandler (FiEventType.RECV_CL_MODIFY_NICK_RESPONSE, RecevChangeName);
			nControl.addEventHandler (FiEventType.RECV_NOTIFY_SCROLLING_UPDATE, RecvScrollingUpdate);
			nControl.addEventHandler (FiEventType.RECV_FISHING_GET_SEVEN_REWARD_NOTIFY, RecvBroadCastSevensInfo);
            // 2020/03/04 Joey 增加新手等級提升獎勵跑馬燈消息
			nControl.addEventHandler(FiEventType.RECV_NEW_USER_LEVELUP_NOTIFY, RecvBroadCastUpgradeInfo);
            //接收付款、金幣配置
			//nControl.addEventHandler(FiProtoType.XL_GET_PAY_INFO, RecvPayUpdateInfo);

			//nControl.addEventHandler (FiEventType.RECV_BIND_PHONE_STATE_RESPONSE, ReceiveIsBindPhoneMassage);
		}

		public void RecvScrollingUpdate (object data)
		{
			FiNotifyScrollingNoticesUpdate nResult = (FiNotifyScrollingNoticesUpdate)data;
			BroadCastInfo nDataInfo = (BroadCastInfo)Facade.GetFacade ().data.Get (FacadeConfig.BROADCAST_MODULE_ID);
			nDataInfo.SetRollMessageInfo (nResult.noticeArray);
//			//Debug.LogError ("-----RecvScrollingUpdate" + nResult.noticeArray.Count);
		}



		//玩家发送公告反馈，成功或者失败
		void RecvBroadCastResponse (object data)
		{
            
			FiBroadCastUserMsgResponse nResponse = (FiBroadCastUserMsgResponse)data;
			if (nResponse.result == -12) {
				//发送字符中存在非法字符
				string path = "Window/WindowTipsThree";
				GameObject WindowClone = AppControl.OpenWindow (path);
				WindowClone.SetActive (true);
				UITipAutoNoMask ClickTips1 = WindowClone.GetComponent<UITipAutoNoMask> ();
				ClickTips1.tipText.text = "發送公告中存在非法字符,請重新輸入";
				return;

			}
			//发送广播成功
			if (nResponse.result == 0) {
				TimeCount.Instanse.StartTime (); //开始计时;

				MyInfo myInfo = (MyInfo)Facade.GetFacade ().data.Get (FacadeConfig.USERINFO_MODULE_ID);

				myInfo.diamond -= myInfo.Consume;   //消耗钻石数量通过获取服务器下发的数量决定

				if (Facade.GetFacade ().ui.Get (FacadeConfig.UI_STORE_MODULE_ID) != null) {
					Facade.GetFacade ().ui.Get (FacadeConfig.UI_STORE_MODULE_ID).OnRecvData (FiPropertyType.DIAMOND, myInfo.diamond);
				}

				if (PrefabManager._instance != null && PrefabManager._instance.GetLocalGun () != null) {
					PrefabManager._instance.GetLocalGun ().gunUI.AddValue (0, 0, -myInfo.Consume);
				}
			} else {
				//Debug.LogError ("发送失败，错误码：" + nResponse.result);
			}
		}

		//玩家收到游戏公告
		void RecvBroadCastGameInfo (object data)
		{
			FiBroadCastGameInfo nInfo = (FiBroadCastGameInfo)data;
			BroadCastInfo nDataInfo = (BroadCastInfo)Facade.GetFacade ().data.Get (FacadeConfig.BROADCAST_MODULE_ID);
			nDataInfo.AddGameInfo (nInfo.content, nInfo.type);
//			nDataInfo.AddSevenDayMessage (nInfo.content, nInfo.type);
			if (NoticeManager.instans != null) {
				NoticeManager.instans.RefreshNotice ();
			}
			if (BroadCastManager.instans != null && !BroadCastManager.instans.isRoll) {

				BroadCastManager.instans.RollComplete ();
			}
		}

		//玩家收到七日公告
		void RecvBroadCastSevensInfo (object data)
		{
			FiBroadCastSevenInfo nInfo = (FiBroadCastSevenInfo)data;
			BroadCastInfo nDataInfo = (BroadCastInfo)Facade.GetFacade ().data.Get (FacadeConfig.BROADCAST_MODULE_ID);
			//			nDataInfo.AddGameInfo (nInfo.content, nInfo.type);
			nDataInfo.AddSevenDayMessage (nInfo.content, nInfo.type);
			if (NoticeManager.instans != null) {
				NoticeManager.instans.RefreshNotice ();
			}
			if (SevenBroad.instans != null && !SevenBroad.instans.isRoll) {
				SevenBroad.instans.RollComplete ();
			}
		}

		//收到玩家发出的公告消息
		void RecvBroadCastUserMessage (object data)
		{
			FiBroadCastUserMsgInfrom nInfrom = (FiBroadCastUserMsgInfrom)data;
			BroadCastInfo nDataInfo = (BroadCastInfo)Facade.GetFacade ().data.Get (FacadeConfig.BROADCAST_MODULE_ID);
			nDataInfo.AddUserMessage (nInfrom.nickname, nInfrom.content);
			if (NoticeManager.instans != null) {
				NoticeManager.instans.RefreshNotice ();
			}
			if (BroadCastManager.instans != null && !BroadCastManager.instans.isRoll) {
				
				BroadCastManager.instans.RollComplete ();
			}

		}

		//玩家收到七日公告
		void RecvBroadCastUpgradeInfo(object data)
		{
			Debug.Log(" ~~~~~BroadCastMsgHandle~~~~~RecvBroadCastUpgradeInfo~~~~~");
			FiBroadCastUpgradeInfo nInfo = (FiBroadCastUpgradeInfo)data;
			BroadCastInfo nDataInfo = (BroadCastInfo)Facade.GetFacade().data.Get(FacadeConfig.BROADCAST_MODULE_ID);
			//			nDataInfo.AddGameInfo (nInfo.content, nInfo.type);
			nDataInfo.AddUpgradeDayMessage(nInfo.content, nInfo.type);
			if (NoticeManager.instans != null)
			{
				NoticeManager.instans.RefreshNotice();
			}
			if (UpgradeBroad.instans != null && !UpgradeBroad.instans.isRoll)
			{
				UpgradeBroad.instans.RollComplete();
			}
		}

		//付款、金幣配置
		void RecvPayUpdateInfo(object data)
		{
			Debug.Log("RecvPayUpdateInfo!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!");
		}

		public void OnDestroy ()
		{
			EventControl nControl = EventControl.instance ();
			nControl.removeEventHandler (FiEventType.RECV_BROADCAST_USER_MESSAGE_RESPONSE, RecvBroadCastResponse);
			nControl.removeEventHandler (FiEventType.RECV_NOTIFY_BROADCAST_GAME_INFO, RecvBroadCastGameInfo);
			nControl.removeEventHandler (FiEventType.RECV_NOTIFY_BROADCAST_USER_MESSAGE, RecvBroadCastUserMessage);
			nControl.removeEventHandler (FiEventType.RECV_NOTIFY_SCROLLING_UPDATE, RecvScrollingUpdate);
			nControl.removeEventHandler (FiEventType.RECV_CL_MODIFY_NICK_RESPONSE, RecevChangeName);
			nControl.removeEventHandler (FiEventType.RECV_FISHING_GET_SEVEN_REWARD_NOTIFY, RecvBroadCastSevensInfo);
			// 2020/03/04 Joey 增加新手等級提升獎勵跑馬燈消息
			nControl.removeEventHandler(FiEventType.RECV_NEW_USER_LEVELUP_NOTIFY, RecvBroadCastUpgradeInfo);
            //付款、金幣配置
			//nControl.removeEventHandler(FiProtoType.XL_GET_PAY_INFO, RecvPayUpdateInfo);
			//nControl.removeEventHandler(FiEventType.RECV_BIND_PHONE_STATE_RESPONSE, ReceiveIsBindPhoneMassage);
		}

		public void RecevChangeName (object data)
		{
			//Debug.LogError ("ssssss");
			BackpackInfo nBackInfo = (BackpackInfo)Facade.GetFacade ().data.Get (FacadeConfig.BACKPACK_MODULE_ID);
			MyInfo nUserInfo = (MyInfo)Facade.GetFacade ().data.Get (FacadeConfig.USERINFO_MODULE_ID);
			FiSystemReward nResponse = (FiSystemReward)data;
			//Debug.LogError (nResponse.resultCode);
			if (nResponse.resultCode == 0) {
				switch (nResponse.propID) {
				case FiPropertyType.GOLD:
					nUserInfo.gold += nResponse.propCount;
					//Debug.LogError ("--------ReciveButton gold--------" + nResponse.propCount + " / " + nUserInfo.gold);
					if (Facade.GetFacade ().ui.Get (FacadeConfig.UI_STORE_MODULE_ID) != null) {
						Facade.GetFacade ().ui.Get (FacadeConfig.UI_STORE_MODULE_ID).OnRecvData (FiPropertyType.GOLD, nUserInfo.gold);
					}
					break;
				case FiPropertyType.DIAMOND:
					nUserInfo.diamond += nResponse.propCount;
					//Debug.LogError ("--------ReciveButton diamond--------" + nResponse.propCount + " / " + nUserInfo.diamond);
					if (Facade.GetFacade ().ui.Get (FacadeConfig.UI_STORE_MODULE_ID) != null) {
						Facade.GetFacade ().ui.Get (FacadeConfig.UI_STORE_MODULE_ID).OnRecvData (FiPropertyType.DIAMOND, nUserInfo.diamond);
					}
					break;
				default:
					//Debug.LogError (nResponse.propID);
					Debug.LogError ("--------ReciveButton type--------" + nResponse.propID + " / " + nResponse.propCount);
					nBackInfo.Add (nResponse.propID, nResponse.propCount);
					break;

				}

				UIUserDetail.instace.OnChangeName (Newname);
				MiddleInfo.instance.OnChangeName (Newname);

				nUserInfo.nickname = (string)Newname;
				GameObject Window1 = UnityEngine.Resources.Load ("Window/WindowTipsThree")as UnityEngine.GameObject;
				GameObject WindowClone1 = UnityEngine.GameObject.Instantiate (Window1);
				UITipAutoNoMask ClickTips1 = WindowClone1.GetComponent<UITipAutoNoMask> ();
				ClickTips1.tipText.text = "修改暱稱成功";

			} else {
				//Debug.LogError ("----------修改昵称失败----------");
				GameObject Window1 = UnityEngine.Resources.Load ("Window/WindowTipsThree")as UnityEngine.GameObject;
				GameObject WindowClone1 = UnityEngine.GameObject.Instantiate (Window1);
				UITipAutoNoMask ClickTips1 = WindowClone1.GetComponent<UITipAutoNoMask> ();
				ClickTips1.tipText.text = "您輸入的內容含有非法字符，請重新輸入";
			}


		}

		///// <summary>
		///// 是否绑定手机号
		///// </summary>
		///// <param name="data">数据.</param>
		//public void ReceiveIsBindPhoneMassage(object data)
		//{
		//    //Debug.Log("1234556777=======================");
		//    FiIsBindPhoneResponse result = (FiIsBindPhoneResponse)data;
		//    //Debug.Log("ReceiveIsBindPhoneMassage---------" + result.isBindPhone);

		//    //BackpackInfo nBackInfo = (BackpackInfo)Facade.GetFacade().data.Get(FacadeConfig.BACKPACK_MODULE_ID);
		//    MyInfo nUserInfo = (MyInfo)Facade.GetFacade().data.Get(FacadeConfig.USERINFO_MODULE_ID);
		//    nUserInfo.isBindPhone = result.isBindPhone;
		//    nUserInfo.strPhoneNum = result.strPhoneNum;
		//    if (nUserInfo.isBindPhone == 1)
		//    {
		//        //Debug.Log("nUserInfo.isBindPhone==1-------" + nUserInfo.isBindPhone);
		//    }
		//}
	}
}

