using System;
using UnityEngine;
using Google.Protobuf;
namespace AssemblyCSharp
{
	public class BankMsgHandle:IMsgHandle
	{
		public BankMsgHandle ()
		{
		}

		public void SendGetBankMessageRequest()
		{
//			 nRequest = new FiBroadCastUserMsgRequest ();
//			nRequest.content = nContent;
			DataControl.GetInstance ().PushSocketSndByte ( FiEventType.SNED_GET_BANKMSG_REQUEST , null );
		}

		public void SendSetPswdRequest( string nPswd )
		{
			MyInfo myInfo = (MyInfo)Facade.GetFacade ().data.Get (FacadeConfig.USERINFO_MODULE_ID);
			CLModifySecPasswdRequest nRequest = new CLModifySecPasswdRequest ();
			nRequest.NewPasswd = nPswd;
			nRequest.UserId = myInfo.userID;
			nRequest.Token = myInfo.mLoginData.token;
			nRequest.TokenType = myInfo.GetTokenType();

			//Debug.LogError ( "----nRequest---" +  nRequest );
			DataControl.GetInstance ().PushSocketSndByte ( FiEventType.SEND_CL_MODIFY_SECOND_PSWD_REQUEST , nRequest.ToByteArray() );
		}

		public void SendBankAccessRequest( long nGold , string nPswd )
		{
			FiBankAccessRequest nRequest = new FiBankAccessRequest ();
			nRequest.gold = nGold;
			nRequest.pswd = nPswd;
			DataControl.GetInstance ().PushSocketSndByte ( FiEventType.SEND_BANK_ACCESS_REQUEST , nRequest.serialize() );
		}

		public void SendExchangeCharmRequest( long nCharm , string nPswd )
		{
			FiExchangeCharmRequest nRequest = new FiExchangeCharmRequest ();
			nRequest.charm = nCharm;
			nRequest.password = nPswd;
			DataControl.GetInstance ().PushSocketSndByte ( FiEventType.SNED_EXCHANGE_CHARM_REQUEST , nRequest.serialize() );
		}

//		public void SendGiveCharmRequest( int nGiftCount , int nGiftGold , string pswd , int userId )
//		{
//			FiGiveCharmRequest nRequest = new FiGiveCharmRequest ();
//			nRequest.giftCount = nGiftCount;
//			nRequest.giftGold = nGiftGold;
//			nRequest.pswd = pswd;
//			nRequest.userId = userId;
//			DataControl.GetInstance ().PushSocketSndByte ( FiEventType.SEND_GIVE_CHARM_REQUEST , nRequest.serialize() );
//		}

		//送鱼雷和魅力用这个函数 nGiveType = 1 鱼雷 2 魅力
		public void SendCLGiveRequest( int nGiveType , ByteString nPswd , int nToGameId , FiProperty nProp )
		{
			FiCLGiveGiftRequest nRequest = new FiCLGiveGiftRequest ();
			nRequest.giveType = nGiveType;
			nRequest.secondPasswd = nPswd;
			nRequest.toGameId = nToGameId;
			nRequest.gift = nProp;
			//Debug.LogError ( "------------PushSocketSndByte------------");
			DataControl.GetInstance ().PushSocketSndByte ( FiEventType.SEND_CL_GIVE_REQUEST , nRequest.serialize() );
		}

		public void OnInit()
		{
			EventControl nControl = EventControl.instance ();

			nControl.addEventHandler ( FiEventType.RECV_BANK_ACCESS_RESPONSE , RecvBankAccessResponse );
			nControl.addEventHandler ( FiEventType.RECV_SET_PSWD_RESPONSE ,    RecvSetBankPswdResponse );
			//nControl.addEventHandler ( FiEventType.RECV_NOTIFY_BANK_MESSGAE_INFORM , RecvNotifyBankMessageInform );
			nControl.addEventHandler ( FiEventType.RECV_GET_BANKMSG_RESPONSE ,  RecvBankMessageResponse );
			nControl.addEventHandler ( FiEventType.RECV_EXCHANGE_CHARM_RESPONSE , RecvExchangeCharmResponse );
			nControl.addEventHandler ( FiEventType.RECV_CL_GIVE_RESPONSE ,  RecvGiveCharmResponse );
			nControl.addEventHandler ( FiEventType.RECV_NOTIFY_GIVE_CHARM , RecvNotifyGiveCharmInform );
		}

		void RecvNotifyGiveCharmInform( object data )
		{
			FiGiveCharmInform nData = (FiGiveCharmInform)data;
			BankInfo nDataInfo =(BankInfo) Facade.GetFacade ().data.Get ( FacadeConfig.UI_BANk_MOUDLE_ID );
			if (nDataInfo != null) {
				nDataInfo.AddMessage (nData.data);
			}
			MyInfo myInfo=(MyInfo)Facade.GetFacade ().data.Get ( FacadeConfig.USERINFO_MODULE_ID );
			myInfo.loginInfo.charm += nData.data.charmChanged;
			if (BankManager.instance != null) {
				BankManager.instance.RedPoint.SetActive (true);
				if(BankManager.instance.CharmGameObject.activeInHierarchy)
					BankManager.instance.CharmGameObject.GetComponent<IBankItem> ().Refresh ();
			}
		}

		void RecvBankAccessResponse( object data )
		{
			FiBankAccessResponse nMessage = (FiBankAccessResponse)data;
			if (nMessage.result == 0)
			{
				Debug.LogError("存取成功，gold：" + nMessage.gold);
				MyInfo myInfo = (MyInfo)Facade.GetFacade().data.Get(FacadeConfig.USERINFO_MODULE_ID);
				myInfo.loginInfo.bankGold += nMessage.gold;
				myInfo.gold -= nMessage.gold;
				if (BankManager.instance != null)
				{
					BankManager.instance.BankGameObject.GetComponent<IBankItem>().halfRefresh();
					if (nMessage.gold > 0)
						BankManager.instance.CallTips("存款成功！");
					else
						BankManager.instance.CallTips("取款成功！");
				}
				if (Facade.GetFacade().ui.Get(FacadeConfig.UI_STORE_MODULE_ID) != null)
				{
					Facade.GetFacade().ui.Get(FacadeConfig.UI_STORE_MODULE_ID).OnRecvData(FiPropertyType.GOLD, myInfo.gold);
				}
			}
			else if (nMessage.result == 1)
				BankManager.instance.CallTips("取出失敗，請輸入正確的銀行密碼！", true);
			else
			{
				Debug.LogError("存取失敗，result：" + nMessage.result);
				BankManager.instance.CallTips("存取失敗，result：" + nMessage.result, true);
			}
		}

		void RecvSetBankPswdResponse( object data )
		{
			FiSetBankPswdResponse nMessage = (FiSetBankPswdResponse)data;
			if (nMessage.result == 0)
			{
				if (SetBankPassword.instanse != null)
				{
					MyInfo myInfo = (MyInfo)Facade.GetFacade().data.Get(FacadeConfig.USERINFO_MODULE_ID);
					myInfo.loginInfo.hasBankPswd = true;
					SetBankPassword.instanse.SetPasswordSucess();
					if (BankManager.instance != null)
						BankManager.instance.CallTips("設置密碼成功！");
				}
			}
			else
			{
				Debug.LogError("設置密碼失敗，result：" + nMessage.result);
				if (BankManager.instance != null)
				{
					BankManager.instance.CallTips("設置密碼失敗，result：" + nMessage.result, true);
				}
				else
				{
					GameObject Window1 = UnityEngine.Resources.Load("Window/WindowTipsThree") as UnityEngine.GameObject;
					GameObject WindowClone1 = UnityEngine.GameObject.Instantiate(Window1);
					UITipAutoNoMask ClickTips1 = WindowClone1.GetComponent<UITipAutoNoMask>();
					ClickTips1.tipText.text = "設置密碼失敗";
				}
			}
		}

		void RecvNotifyBankMessageInform( object data )
		{
			FiBankMessageInform nMessage = (FiBankMessageInform)data;

			BankInfo nDataInfo =(BankInfo) Facade.GetFacade ().data.Get ( FacadeConfig.UI_BANk_MOUDLE_ID );
			if (nDataInfo != null) {
				nDataInfo.AddMessage (nMessage.data);
			}
			Debug.LogError ("getmsginfo~");

		}

		void RecvBankMessageResponse( object data )
		{
			FiGetBankMessageResponse nMessage = (FiGetBankMessageResponse)data;
			if (nMessage.reuslt == 0) {
				BankInfo nDataInfo = (BankInfo)Facade.GetFacade ().data.Get (FacadeConfig.UI_BANk_MOUDLE_ID);
				if (nDataInfo != null) {
					nDataInfo.OnAddData (0, nMessage.messages);
					nDataInfo.SetMsgList (nMessage.messages);
				}
			} else {
				Debug.LogError("獲取消息列表失敗，result：" + nMessage.reuslt);
				BankManager.instance.CallTips("獲取消息列表失敗，result：" + nMessage.reuslt, true);
			}
			//Debug.LogError ("getmsglist~:"+nMessage.messages.Count);
		}

		void RecvExchangeCharmResponse( object data )
		{
			FiExchangeCharmResponse nMessage = (FiExchangeCharmResponse)data;
			if (nMessage.result == 0) {
				Debug.LogError("兌換成功，charm：" + nMessage.charm);
				Debug.LogError("兌換成功，bankgold:" + nMessage.bankGold);
				MyInfo myInfo = (MyInfo)Facade.GetFacade().data.Get(FacadeConfig.USERINFO_MODULE_ID);
				myInfo.loginInfo.charm -= nMessage.charm;
				myInfo.loginInfo.bankGold += nMessage.bankGold;
				myInfo.loginInfo.charmExchangeTimes -= 1;
				string path = "Window/WindowTips";
				GameObject WindowClone = AppControl.OpenWindow(path);
				var temp = WindowClone.GetComponent<UITipClickHideManager>();
				temp.text.text = "成功兌換" + nMessage.charm + "魅力！銀行內金幣增加" + nMessage.bankGold / 10000 + "萬！";
				temp.time.text = "5";
				WindowClone.SetActive (true);
			}else if(nMessage.result==6)
				BankManager.instance.CallTips("兌換失敗，請輸入正確的銀行密碼！", true);
			else {
				Debug.LogError("兌換魅力值失敗，result：" + nMessage.result);
				BankManager.instance.CallTips("兌換魅力值失敗，result：" + nMessage.result, true);
			}
			if (BankManager.instance != null) {
				BankManager.instance.CharmGameObject.GetComponent<IBankItem> ().halfRefresh ();
				BankManager.instance.CharmGameObject.GetComponent<Bank_CharmManager> ().changeButton.interactable = true;
			}
		}

		void RecvGiveCharmResponse( object data )
		{
			//Debug.LogError ( "----------------------1 ");
			FiCLGiveGiftResponse nResponse = ( FiCLGiveGiftResponse )data;
			//赠送失败了
			if (nResponse.result != 0) {
				if (nResponse.result == 1001) {
					MyInfo myInfo = (MyInfo)Facade.GetFacade ().data.Get (FacadeConfig.USERINFO_MODULE_ID);
					myInfo.loginInfo.bankGold = nResponse.currentCount;
					if (BankManager.instance != null)
						BankManager.instance.PresentGameObject.GetComponent<IBankItem> ().halfRefresh ();
					
				} else if (nResponse.result == 6) {
					BankManager.instance.CallTips(nResponse.errorMsg,true);
					if (BankGiveTips.instanse != null)
						BankGiveTips.instanse.GiveSucess ();
					return;
				}

				if (BankGiveTips.instanse != null)
					BankGiveTips.instanse.GiveSucess ();
				if (BankManager.instance != null) {
					BankManager.instance.CallTips (nResponse.errorMsg , true);
					return;
				}
				GameObject Window = Resources.Load ("Window/WindowTips")as GameObject;
				GameObject mCloseTipWin =GameObject.Instantiate(Window);
				UITipClickHideManager ClickTip = mCloseTipWin.GetComponent<UITipClickHideManager> ();
				ClickTip.text.text = nResponse.errorMsg;
				return;
			}

			//鱼雷赠送
			if (nResponse.giveType == 1) {

				BackpackInfo nBack =( BackpackInfo ) Facade.GetFacade ().data.Get ( FacadeConfig.BACKPACK_MODULE_ID );
				nBack.Replace ( (int)nResponse.gift.type , (int)nResponse.currentCount );
				IUiMediator nMediator = Facade.GetFacade ().ui.Get (FacadeConfig.UI_BACKPACK_MODULE_ID);
				if (nMediator != null) {
					nMediator.OnRecvData ( UIBackPack.UPDATE_ALL , null );
				}

			} else if (nResponse.giveType == 2) { //魅力赠送返回，扣除金币
				MyInfo myInfo=(MyInfo)Facade.GetFacade ().data.Get ( FacadeConfig.USERINFO_MODULE_ID );
				if (nResponse.gift.type == 2) {
					myInfo.loginInfo.bankGold -= 100000 * nResponse.gift.value;
				} else if (nResponse.gift.type == 3) {
					myInfo.loginInfo.bankGold -= 500000 * nResponse.gift.value;
				} else if (nResponse.gift.type == 4) {
					myInfo.loginInfo.bankGold -= 1000000 * nResponse.gift.value;
				}
				if (BankGiveTips.instanse != null)
					BankGiveTips.instanse.GiveSucess ();
				if (BankManager.instance != null)
					BankManager.instance.PresentGameObject.GetComponent<IBankItem> ().halfRefresh ();
				if (BankManager.instance != null) 
					BankManager.instance.CallTips ("贈送禮物成功！");
			}
			//FiGiveCharmResponse nMessage = ( FiGiveCharmResponse )data;
			/*if (nMessage.result == 0) {
				MyInfo myInfo = (MyInfo)Facade.GetFacade ().data.Get (FacadeConfig.USERINFO_MODULE_ID);
				myInfo.loginInfo.bankGold -= nMessage.goldDec;
				if (BankGiveTips.instanse != null)
					BankGiveTips.instanse.GiveSucess ();
				if (BankManager.instance != null)
					BankManager.instance.PresentGameObject.GetComponent<IBankItem> ().halfRefresh ();
				if (BankManager.instance != null) 
					BankManager.instance.CallTips ("赠送礼物成功！");
			} else if (nMessage.result == 2) {
				BankManager.instance.CallTips ("赠送失败，请输入正确的银行密码！", true);
				if (BankGiveTips.instanse != null)
					BankGiveTips.instanse.GiveSucess ();
			}
			else {
				Debug.LogError ("赠送礼物失败，result：" + nMessage.result);
				BankManager.instance.CallTips ("赠送礼物失败，result：" + nMessage.result, true);
			}*/
		}

		public void OnDestroy()
		{
			EventControl nControl = EventControl.instance ();
			nControl.removeEventHandler ( FiEventType.RECV_NOTIFY_GIVE_CHARM , RecvNotifyGiveCharmInform );
			nControl.removeEventHandler ( FiEventType.RECV_BANK_ACCESS_RESPONSE , RecvBankAccessResponse );
			nControl.removeEventHandler ( FiEventType.RECV_SET_PSWD_RESPONSE ,    RecvSetBankPswdResponse );
			//nControl.removeEventHandler ( FiEventType.RECV_NOTIFY_BANK_MESSGAE_INFORM , RecvNotifyBankMessageInform );
			nControl.removeEventHandler ( FiEventType.RECV_GET_BANKMSG_RESPONSE ,  RecvBankMessageResponse );
			nControl.removeEventHandler ( FiEventType.RECV_EXCHANGE_CHARM_RESPONSE , RecvExchangeCharmResponse );
			nControl.removeEventHandler ( FiEventType.RECV_CL_GIVE_RESPONSE ,  RecvGiveCharmResponse );
		}

	}
}

