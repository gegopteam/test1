using System;
using UnityEngine;

namespace AssemblyCSharp
{
	public class FriendMsgHandle: IMsgHandle
	{
		public FriendMsgHandle ()
		{
			
		}

		public void OnInit ()
		{
			EventControl mControl = EventControl.instance ();
			mControl.addEventHandler (FiEventType.RECV_ADD_FRIEND_RESPONSE, RecvAddFirendResponse);
			mControl.addEventHandler (FiEventType.RECV_GET_FRIEND_LIST_RESPONSE, RecvGetFirendListResponse);
			mControl.addEventHandler (FiEventType.RECV_GET_FRIEND_APPLY_LIST_RESPONSE, RecvGetFriendApplyResponse);
			mControl.addEventHandler (FiEventType.RECV_DELETE_FRIEND_RESPONSE, RecvDeleteFriendResponse);
			mControl.addEventHandler (FiEventType.RECV_ACCEPT_FRIEND_RESPONSE, RecvAcceptFriendResponse);
			mControl.addEventHandler (FiEventType.RECV_GET_USER_INFO_RESPONSE, RecvGetUserInfoResponse);
			mControl.addEventHandler (FiEventType.RECV_ROOM_CHAT_MESSAGE, RecvChatMessageInform);
		}

		public void OnDestroy ()
		{
			EventControl mControl = EventControl.instance ();
			mControl.removeEventHandler (FiEventType.RECV_ADD_FRIEND_RESPONSE, RecvAddFirendResponse);
			mControl.removeEventHandler (FiEventType.RECV_GET_FRIEND_LIST_RESPONSE, RecvGetFirendListResponse);
			mControl.removeEventHandler (FiEventType.RECV_GET_FRIEND_APPLY_LIST_RESPONSE, RecvGetFriendApplyResponse);
			mControl.removeEventHandler (FiEventType.RECV_DELETE_FRIEND_RESPONSE, RecvDeleteFriendResponse);
			mControl.removeEventHandler (FiEventType.RECV_ACCEPT_FRIEND_RESPONSE, RecvAcceptFriendResponse);
			mControl.removeEventHandler (FiEventType.RECV_GET_USER_INFO_RESPONSE, RecvGetUserInfoResponse);
			mControl.removeEventHandler (FiEventType.RECV_ROOM_CHAT_MESSAGE, RecvChatMessageInform);
		}

		public void SendChatMessage (string nContent)
		{
			if (!string.IsNullOrEmpty (nContent)) {
				FiChatMessage nChatMessage = new FiChatMessage ();
				nChatMessage.userId = DataControl.GetInstance ().GetMyInfo ().userID;
				nChatMessage.message = nContent;
				DataControl.GetInstance ().PushSocketSndByte (FiEventType.SEND_ROOM_CHAT_MESSAGE, nChatMessage.serialize ());
			}
		}

		public void RecvChatMessageInform (object data)
		{
			FiChatMessage nChatMessage = (FiChatMessage)data;
			Tool.LogError ("RecvChat:" + nChatMessage.userId + "   " + nChatMessage.message);
			GunControl tempGun = PrefabManager._instance.GetGunByUserID (nChatMessage.userId);
			if (tempGun != null) {
				tempGun.gunUI.ShowChatBubbleBox (nChatMessage.message, 4f, PlayVoice (nChatMessage.message));
			}
			ChatDataInfo nInfo = (ChatDataInfo)Facade.GetFacade ().data.Get (FacadeConfig.CHAT_MODULE_ID);
			if (nInfo != null) {
				nInfo.addChatMsg (nChatMessage);
				if (ChatPanelManager.instans != null && ChatPanelManager.instans.gameObject.activeInHierarchy) {
					ChatPanelManager.instans.scroll.cellNumber = nInfo.getChatList ().Count;
					ChatPanelManager.instans.RefreshList ();
				}
			}
		}

		int PlayVoice (string msg)
		{
			int num = 0;
			switch (msg)
			{
				case "大家好，很高興見到各位！":
					num = 1;
					break;
				case "抱歉！":
					num = 2;
					break;
				case "打打打，看你能得意多久。":
					num = 3;
					break;
				case "技不如人，甘拜下風！":
					num = 4;
					break;
				case "不好意思，又贏了！":
					num = 5;
					break;
			}
			return num;
		}
		//		public void AddMsgEventHandle()
		//		{
		//
		//			//mControl.addEventHandler ( FiEventType.SEND_REJECT_FRIEND_REQUEST , RecvAcceptFriendResponse );
		//		}

		private void RecvGetUserInfoResponse (object data)
		{
			FiGetUserInfoResponse nResponse = (FiGetUserInfoResponse)data;
			if (nResponse.reuslt == 0 && nResponse.nUserInfo.userId != 0 && nResponse.nUserInfo.nickName != null) {
				IUiMediator nMediator = Facade.GetFacade ().ui.Get (FacadeConfig.UI_GIVE_MODULE_ID);
				if (nMediator != null)
					nMediator.OnRecvData (FiEventType.RECV_GET_USER_INFO_RESPONSE, nResponse);
				if (BankManager.instance != null) {
					MyInfo myInfo = (MyInfo)Facade.GetFacade ().data.Get (FacadeConfig.USERINFO_MODULE_ID);
					if (nResponse.nUserInfo.userId == myInfo.userID) {
						BankManager.instance.CallTips("無法贈送自己禮物！");
						return;
					}
					string path = "MainHall/Common/BankGiveTips";
					GameObject WindowClone = AppControl.OpenWindow (path);
					WindowClone.GetComponent<BankGiveTips> ().SetUser (nResponse.nUserInfo);
					WindowClone.SetActive (true);
				}
			} else {
				string path = "Window/WindowTips";
				GameObject WindowClone = AppControl.OpenWindow (path);
				WindowClone.SetActive (true);
				UITipClickHideManager ClickTips = WindowClone.GetComponent<UITipClickHideManager> ();
				ClickTips.time.text = "2";
				ClickTips.text.text = "請輸入正確的id!";
			}
		}

		//添加好友反馈
		private void RecvAddFirendResponse (object data)
		{
            
			if (GameController._instance != null)//如果是在渔场里，不用显示信息
                return;
            
			FiAddFriendResponse nResponse = (FiAddFriendResponse)data;
			UITipClickHideManager ClickTips;

			if (nResponse.result == 0) {
				GameObject Window = UnityEngine.Resources.Load("Window/WindowTipsThree") as UnityEngine.GameObject;
				GameObject WindowClone = UnityEngine.GameObject.Instantiate(Window);
				if (GameController._instance == null)
				{
					UITipAutoNoMask ClickTips1 = WindowClone.GetComponent<UITipAutoNoMask>();
					ClickTips1.tipText.text = "好友請求已發送!";
				}
				else
				{
					HintTextPanel._instance.SetTextShow("好友請求已發送!");
				}

			} else {
              
				if (nResponse.result == -1002) {
					GameObject Window = UnityEngine.Resources.Load ("Window/WindowTips")as UnityEngine.GameObject;
					GameObject WindowClone = UnityEngine.GameObject.Instantiate (Window);
					ClickTips = WindowClone.GetComponent<UITipClickHideManager> ();
					if (PrefabManager._instance != null) {
						GunControl tempGun = PrefabManager._instance.GetGunByUserID (nResponse.userId);
						if (tempGun != null)
						{
							if (tempGun.gameID == 0)
							{
								ClickTips.time.text = "2";
								ClickTips.text.text = "請求已發送!";
							}
						}
					}
					else
					{
						ClickTips.time.text = "2";
						ClickTips.text.text = "當前ID不存在!";
					}

				} else if (nResponse.result == -1006) { 
					GameObject Window = UnityEngine.Resources.Load ("Window/WindowTipsThree")as UnityEngine.GameObject;
					GameObject WindowClone = UnityEngine.GameObject.Instantiate (Window);
					UITipAutoNoMask ClickTips1 = WindowClone.GetComponent<UITipAutoNoMask> ();
					ClickTips1.tipText.text = "請求已發送!";
				} else {
					GameObject Window = UnityEngine.Resources.Load ("Window/WindowTipsThree")as UnityEngine.GameObject;
					GameObject WindowClone = UnityEngine.GameObject.Instantiate (Window);
					UITipAutoNoMask ClickTips1 = WindowClone.GetComponent<UITipAutoNoMask> ();
					ClickTips1.tipText.text = "好友請求失敗!";
				}
				UnityEngine.Debug.Log ("發送申請失敗" + nResponse.result);
			}
		}

		//获取好友列表反馈
		private void RecvGetFirendListResponse (object data)
		{
			FiGetFriendListResponse nResponse = (FiGetFriendListResponse)data;
//			UnityEngine.Debug.Log ("nResponse.friends.count"+nResponse.friends.Count);
			if (nResponse.result == 0) {
				FriendInfo nInfo = (FriendInfo)Facade.GetFacade ().data.Get (FacadeConfig.FRIEND_MODULE_ID);
				nInfo.SetFriendData (nResponse.friends);
				nInfo.countLimits = nResponse.friendLimit;
				if (GameManager.friendBackage != null) {
					GameManager.friendBackage.cellNumber = nResponse.friends.Count;
					GameManager.friendBackage.Refresh ();
					GameManager.instans.UpdateInfo ();
				}
			}
		}

		//获取好友申请列表
		private void RecvGetFriendApplyResponse (object data)
		{
			FiGetFriendApplyResponse nResponse = (FiGetFriendApplyResponse)data;
//			UnityEngine.Debug.LogError ( "----------" + nResponse );
			if (nResponse.result == 0) {
				FriendInfo nInfo = (FriendInfo)Facade.GetFacade ().data.Get (FacadeConfig.FRIEND_MODULE_ID);
				nInfo.SetApplyFriend (nResponse.friends);
				nInfo.AddApplyInfo (nResponse);
				if (ApplyManager.applyBackage != null && ApplyManager.applyBackage.gameObject.activeInHierarchy) {
					ApplyManager.applyBackage.cellNumber = nResponse.friends.Count;
					ApplyManager.applyBackage.Refresh ();
					if (nInfo.getApplyFriends ().Count > 0)
						UIGoodFriends.instans.redPoint.SetActive (true);
					else
						UIGoodFriends.instans.redPoint.SetActive (false);
						
				}
				if (UIGoodFriends.instans != null)
					UIGoodFriends.instans.RefreshApply ();
			} else {
				UnityEngine.Debug.Log ("獲取好友申請列表失敗");
			}
		}

		private void RecvDeleteFriendResponse (object data)
		{
			FiDeleteFriendResponse nResponse = (FiDeleteFriendResponse)data;
			if (nResponse.result == 0) {
				FriendInfo nInfo = (FriendInfo)Facade.GetFacade ().data.Get (FacadeConfig.FRIEND_MODULE_ID);
				nInfo.DeleteUser (nResponse.userId);
				IUiMediator nMediator = Facade.GetFacade ().ui.Get (FacadeConfig.UI_FRIEND_MODULE_ID);
				if (nMediator != null)
					nMediator.OnRecvData (FiEventType.RECV_DELETE_FRIEND_RESPONSE, data);
				GameObject Window = UnityEngine.Resources.Load ("Window/WindowTips")as UnityEngine.GameObject;
				GameObject WindowClone = UnityEngine.GameObject.Instantiate (Window);
				UITipClickHideManager ClickTips = WindowClone.GetComponent<UITipClickHideManager> ();
				ClickTips.time.text = "2";
				ClickTips.text.text = "好友刪除成功!";

				GameManager.friendBackage.cellNumber = nInfo.GetFriendList ().Count;
				GameManager.friendBackage.Refresh ();
			} else {
				UnityEngine.Debug.Log ("刪除失敗");
			}
		}

		private void RecvAcceptFriendResponse (object data)
		{
			FiAcceptFriendResponse nResponse = (FiAcceptFriendResponse)data;
			if (nResponse.result == 0) {
				GameObject Window = UnityEngine.Resources.Load ("Window/WindowTips")as UnityEngine.GameObject;
				GameObject WindowClone = UnityEngine.GameObject.Instantiate (Window);
				UITipClickHideManager ClickTips = WindowClone.GetComponent<UITipClickHideManager> ();
				ClickTips.time.text = "2";
				ClickTips.text.text = "好友添加成功!";
				UnityEngine.Debug.Log("好友添加成功");

				//SendGetFriendList ();
				FriendInfo nInfo = (FriendInfo)Facade.GetFacade ().data.Get (FacadeConfig.FRIEND_MODULE_ID);
				nInfo.DeleteApply (nResponse.userId);
				ApplyManager.applyBackage.cellNumber = nInfo.getApplyFriends ().Count;
				ApplyManager.applyBackage.Refresh ();

				if (nInfo.getApplyFriends ().Count > 0)
					UIGoodFriends.instans.redPoint.SetActive (true);
				else
					UIGoodFriends.instans.redPoint.SetActive (false);
			} else {
				UnityEngine.Debug.Log("好友添加失敗");
			}
		}


		//获取好友列表
		public void SendGetFriendList ()
		{
			DataControl.GetInstance ().PushSocketSndByte (FiEventType.SEND_GET_FRIEND_LIST_REQUEST, null);
		}

		//获取好友申请列表
		public void SendGetFriendApplyList ()
		{
			DataControl.GetInstance ().PushSocketSndByte (FiEventType.SEND_GET_FRIEND_APPLY_LIST_REQUEST, null);
		}

		//删除好友
		public void SendDeleteFriend (int nUserId)
		{
			FiDeleteFriendRequest nRequest = new FiDeleteFriendRequest ();
			nRequest.userId = nUserId;
			DataControl.GetInstance ().PushSocketSndByte (FiEventType.SEND_DELETE_FRIEND_REQUEST, nRequest.serialize ());
		}

		public void SendAddFriend (int nUserId)
		{
			FiAddFriendRequest nRequest = new FiAddFriendRequest ();
			nRequest.userId = nUserId;
			DataControl.GetInstance ().PushSocketSndByte (FiEventType.SEND_ADD_FRIEND_REQUEST, nRequest.serialize ());
		}

		public void SendAcceptFriend (int nUserId)
		{
			FiAcceptFriendRequest nAccept = new FiAcceptFriendRequest ();
			nAccept.userId = nUserId;
			DataControl.GetInstance ().PushSocketSndByte (FiEventType.SEND_ACCEPT_FRIEND_REQUEST, nAccept.serialize ());
		}

		//拒绝好友申请
		public void SendRefuseFriendApply (int nUserId)
		{
			FiRejectFriendRequest nRequest = new FiRejectFriendRequest ();
			nRequest.userId = nUserId;
			DataControl.GetInstance ().PushSocketSndByte (FiEventType.SEND_REJECT_FRIEND_REQUEST, nRequest.serialize ());
		}

		public void SendGetUserInfoRequest (int nUserId)
		{
			FiGetUserInfoRequest nRequest = new FiGetUserInfoRequest ();
			nRequest.userId = nUserId;
			DataControl.GetInstance ().PushSocketSndByte (FiEventType.SEND_GET_USER_INFO_REQUEST, nRequest.serialize ());
		}

	}
}

