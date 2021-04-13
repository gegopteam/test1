using System;
using UnityEngine;
using UnityEngine.UI;

namespace AssemblyCSharp
{
	public class StartGiftMsgHandle:IMsgHandle
	{
		public StartGiftMsgHandle ()
		{
			
		}

		public void OnInit ()
		{
			EventControl nControl = EventControl.instance ();

//			nControl.addEventHandler ( FiEventType.RECV_START_GIFT_INFORM ,       RecvStartGameInform );
			nControl.addEventHandler (FiEventType.RECV_GET_START_GIFT_RESPONSE, RecvGetStartGiftResponse);
		}

		private bool bRecvdStartGift = false;

		public void SendGetStartGiftRequest (int nDayOffset)
		{
			if (bRecvdStartGift)
				return;
			FiGetStartGiftRequest nRequest = new FiGetStartGiftRequest ();
			nRequest.dayOffset = nDayOffset;
			DataControl.GetInstance ().PushSocketSndByte (FiEventType.SEND_GET_START_GIFT_REQUEST, nRequest.serialize ());
		}

		private void NotifyToUi (int nType, object data)
		{
			/*IUiMediator nMediator = Facade.GetFacade ().ui.Get ( FacadeConfig.START_GIFT_MODULE_ID );
			if (nMediator != null)
				nMediator.OnRecvData ( nType , data );*/
		}

		private void RecvGetStartGiftResponse (object data)
		{
			//NotifyToUi ( FiEventType.RECV_GET_START_GIFT_RESPONSE , data );
			//用户金币以及道具信息增加
			FiGetStartGiftResponse nInform = (FiGetStartGiftResponse)data;
			UnityEngine.Debug.LogError ("--------------------------" + nInform.data.Count + "/" + nInform.result);
			BackpackInfo backpackInfo = (BackpackInfo)Facade.GetFacade ().data.Get (FacadeConfig.BACKPACK_MODULE_ID);
			MyInfo myInfo = (MyInfo)Facade.GetFacade ().data.Get (FacadeConfig.USERINFO_MODULE_ID);
			if (nInform.result == 0) {
				DailySignInfo nInfo = (DailySignInfo)Facade.GetFacade ().data.Get (FacadeConfig.SIGN_IN_MODULE_ID);
				nInfo.Setistowwor (false);
				bRecvdStartGift = true;
				for (int i = 0; i < nInform.data.Count; i++) {
					switch (nInform.data [i].type) {
					case FiPropertyType.GOLD:
						myInfo.gold += nInform.data [i].value;
						if (Facade.GetFacade ().ui.Get (FacadeConfig.UI_STORE_MODULE_ID) != null) {
							Facade.GetFacade ().ui.Get (FacadeConfig.UI_STORE_MODULE_ID).OnRecvData (FiPropertyType.GOLD, myInfo.gold);
						}
						break;
					case FiPropertyType.FISHING_EFFECT_FREEZE:
						backpackInfo.Add (nInform.data [i].type, nInform.data [i].value);
						break;
					case FiPropertyType.FISHING_EFFECT_AIM:
						backpackInfo.Add (nInform.data [i].type, nInform.data [i].value);
						break;
					}
				}
			} else {
				GameObject Window1 = UnityEngine.Resources.Load ("Window/WindowTipsThree")as UnityEngine.GameObject;
				GameObject WindowClone1 = UnityEngine.GameObject.Instantiate (Window1);
				UITipAutoNoMask ClickTips1 = WindowClone1.GetComponent<UITipAutoNoMask> ();
				ClickTips1.tipText.text = "起航禮包領取失敗，錯誤碼 " + nInform.result;
			}

			IUiMediator nMediator = Facade.GetFacade ().ui.Get (FacadeConfig.UI_START_GIFT_MODULE_ID);
			if (nMediator != null)
				nMediator.OnRecvData (FiEventType.RECV_GET_START_GIFT_RESPONSE, data);
			
			//如果领取第一天，显示意见进入渔场的手势提示
			if (myInfo.sailDay == 1) {
				IUiMediator nMediatorHall = Facade.GetFacade ().ui.Get (FacadeConfig.UI_HALL_MODULE_ID);
				if (nMediatorHall != null) {
					//((UIHall)nMediatorHall).ShowHandEffect ();
				}
			}

			//当sailDay  为负数的时候， 表示已经领取过了s
			if (myInfo.sailDay > 0) {
				myInfo.sailDay = -myInfo.sailDay;
			}
		}

		public void OnDestroy ()
		{
			EventControl nControl = EventControl.instance ();
			nControl.removeEventHandler (FiEventType.RECV_GET_START_GIFT_RESPONSE, RecvGetStartGiftResponse);
		}

	}
}

