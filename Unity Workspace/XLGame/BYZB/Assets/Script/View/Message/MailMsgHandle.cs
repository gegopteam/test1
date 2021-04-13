using System;
using System.Collections.Generic;
using UnityEngine;

namespace AssemblyCSharp
{
	public class MailMsgHandle:IMsgHandle
	{
		public MailMsgHandle ()
		{
		}
		//删除系统邮件的操作
		public void SendDeleteSystemMailRequest (List<long>  nMailIds)
		{
			/*FiAcceptPresentResponse nres = new FiAcceptPresentResponse ();
			FiProperty nss = new FiProperty ();
			nss.type = FiPropertyType.FISHING_EFFECT_VIOLENT;
			nss.value = 1000;
			nres.properties.Add ( nss );
			RecvAcceptGiveResponse ( nres );
			return;*/
			FiGetMailAwardsAndDeleteRequest nRequest = new FiGetMailAwardsAndDeleteRequest ();
			nRequest.mailId = nMailIds;
			MailDataInfo nMailInfo = (MailDataInfo)Facade.GetFacade ().data.Get (FacadeConfig.MAIL_MODULE_ID);
			if (nMailInfo != null) {
				foreach (long nMailId in nMailIds) {
//					UnityEngine.Debug.LogError ("-------delete mail--------" + nMailId);
					nMailInfo.RemoveMail ((int)nMailId);
				}
				if (UIMail.instans != null)
					UIMail.instans.RefreshSystemPoint ();
			}
			DataControl.GetInstance ().PushSocketSndByte (FiEventType.SEND_PROCESS_SYSTEM_MAIL_REQUEST, nRequest.serialize ());
		}
		//获得系统邮件
		public void SendGetSystemMailRequest ()
		{
			DataControl.GetInstance ().PushSocketSndByte (FiEventType.SEND_GET_SYSTEM_MAIL_REQUEST, null);
		}

		//获得好友赠送
		public void SendGetPersentRecordRequest ()
		{
			DataControl.GetInstance ().PushSocketSndByte (FiEventType.SEND_GET_GIVE_RECORD_REQUEST, null);
		}

		//接收赠送的请求
		public void SendAcceptPersentRequest (List<long> nPersentList)
		{
//			FiAcceptPresentResponse nres = new FiAcceptPresentResponse ();
//			FiProperty nss = new FiProperty ();
//			nss.type = FiPropertyType.GOLD;
//			nss.value = 1000;
//			nres.properties.Add ( nss );
//			RecvAcceptGiveResponse ( nres );
//			return;

			FiAcceptPresentRequest nAcceptRequest = new FiAcceptPresentRequest ();
			nAcceptRequest.giveId = nPersentList;
			MailDataInfo nMailInfo = (MailDataInfo)Facade.GetFacade ().data.Get (FacadeConfig.MAIL_MODULE_ID);
			if (nMailInfo != null) {
				foreach (long nPersentId in nPersentList) {
					UnityEngine.Debug.LogError ("-------delete record--------" + nPersentId);
					nMailInfo.RemoveRecord ((int)nPersentId);
				}
			}
			if (UIMail.instans != null)
				UIMail.instans.RefreshGivePoint ();
			DataControl.GetInstance ().PushSocketSndByte (FiEventType.SEND_ACCEPT_GIVE_REQUEST, nAcceptRequest.serialize ());
		}


		public void OnInit ()
		{
			EventControl nControl = EventControl.instance ();

			nControl.addEventHandler (FiEventType.RECV_GET_SYSTEM_MAIL_RESPONSE, RecvSystemMailResponse);
			nControl.addEventHandler (FiEventType.RECV_PROCESS_SYSTEM_MAIL_RESPONSE, RecvProcessSystemMailResponse);
			nControl.addEventHandler (FiEventType.RECV_GET_GIVE_RECORD_RESPONSE, RecvGiveRecordResponse);
			nControl.addEventHandler (FiEventType.RECV_ACCEPT_GIVE_RESPONSE, RecvAcceptGiveResponse);
		}

		public void OnDestroy ()
		{
			EventControl nControl = EventControl.instance ();
			nControl.removeEventHandler (FiEventType.RECV_GET_SYSTEM_MAIL_RESPONSE, RecvSystemMailResponse);
			nControl.removeEventHandler (FiEventType.RECV_PROCESS_SYSTEM_MAIL_RESPONSE, RecvProcessSystemMailResponse);
			nControl.removeEventHandler (FiEventType.RECV_GET_GIVE_RECORD_RESPONSE, RecvGiveRecordResponse);
			nControl.removeEventHandler (FiEventType.RECV_ACCEPT_GIVE_RESPONSE, RecvAcceptGiveResponse);
		}

		private void RecvAcceptGiveResponse (object data)
		{
			FiAcceptPresentResponse nResponse = (FiAcceptPresentResponse)data;
			if (nResponse.result == 0) {
				//把奖励数据添加到BackpackInfo
				if (nResponse.properties.Count > 0) {
					UnityEngine.GameObject Window = UnityEngine.Resources.Load ("Window/RewardWindow")as UnityEngine.GameObject;
					GameObject rewardInstance = UnityEngine.GameObject.Instantiate (Window);
					UIReward reward = rewardInstance.GetComponent<UIReward> ();
					reward.SetRewardData (nResponse.properties);
				}
				if (UIMail.instans != null)
					UIMail.instans.RefreshGivePoint ();
			}
		}

		private void RecvProcessSystemMailResponse (object data)
		{
			FiGetMailAwardsAndDeleteResponse nResponse = (FiGetMailAwardsAndDeleteResponse)data;
			if (nResponse.result == 0) {
				if (nResponse.property.Count > 0) {
					UnityEngine.GameObject Window = UnityEngine.Resources.Load ("Window/RewardWindow")as UnityEngine.GameObject;
					GameObject rewardInstance = UnityEngine.GameObject.Instantiate (Window);
					UIReward reward = rewardInstance.GetComponent<UIReward> ();
					reward.SetRewardData (nResponse.property);
				}
				if (UIMail.instans != null)
					UIMail.instans.RefreshSystemPoint ();
			}
		}

		//获取赠送数据信息
		private void RecvGiveRecordResponse (object data)
		{
			FiGetPresentRecordResponse nResponse = (FiGetPresentRecordResponse)data;
			if (nResponse.result == 0) {
				IDataProxy nMailInfo = Facade.GetFacade ().data.Get (FacadeConfig.MAIL_MODULE_ID);
				if (nMailInfo != null)
					nMailInfo.OnAddData (FiEventType.RECV_GET_GIVE_RECORD_RESPONSE, nResponse.records);

				IUiMediator nMediator = Facade.GetFacade ().ui.Get (FacadeConfig.UI_MAIL_MODULE_ID);
				if (nMediator != null) {
					nMediator.OnRecvData (FiEventType.RECV_GET_GIVE_RECORD_RESPONSE, null);
				}
				if (UIMail.instans != null)
					UIMail.instans.RefreshGivePoint ();
			} else {
				UnityEngine.Debug.LogError ("获取好友赠送邮件失败 nResponse.result = " + nResponse.result);
			}
//			UnityEngine.Debug.LogError ("接收到的赠送数据长度为：" + nResponse.records.Count);
		}

		//获取邮件的数据信息
		private void RecvSystemMailResponse (object data)
		{
			FiGetSystemMailResponse nResponse = (FiGetSystemMailResponse)data;
			/*FiSystemMail nCopy = new FiSystemMail ();
			nCopy.mailId = 1000;
			nCopy.content = "test1";
			nCopy.title = "来快活呀";

			nCopy.property.type = FiPropertyType.DIAMOND;
			nCopy.property.value = 99;

			nResponse.mails.Add ( nCopy );*/

			if (nResponse.result == 0) {
				IDataProxy nMailInfo = Facade.GetFacade ().data.Get (FacadeConfig.MAIL_MODULE_ID);
				if (nMailInfo != null)
					nMailInfo.OnAddData (FiEventType.RECV_GET_SYSTEM_MAIL_RESPONSE, nResponse.mails);
				IUiMediator nMediator = Facade.GetFacade ().ui.Get (FacadeConfig.UI_MAIL_MODULE_ID);
				if (nMediator != null) {
					nMediator.OnRecvData (FiEventType.RECV_GET_SYSTEM_MAIL_RESPONSE, null);
				}
			}
			if (UIMail.instans != null)
				UIMail.instans.RefreshSystemPoint ();
		}

	}
}

