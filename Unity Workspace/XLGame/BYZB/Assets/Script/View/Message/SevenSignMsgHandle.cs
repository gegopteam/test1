using System;
using AssemblyCSharp;
using System.Collections.Generic;
using UnityEngine;

namespace AssemblyCSharp
{
	public class SevenSignMsgHandle:IMsgHandle
	{
		private List< int > mRequestTypes = new List<int> ();


		public SevenSignMsgHandle ()
		{

		}

		public void OnInit ()
		{
			EventControl mEventControl = EventControl.instance ();
			mEventControl.addEventHandler (FiEventType.RECV_XL_SEVENDAY_BAG_STATE_INFO_NEW_RESPOSE, RcvSevenSignInfo);
			mEventControl.addEventHandler (FiEventType.RECV_XL_SEVENDAY_START_BAG_STATE_INFO_NEW_RESPOSE, RecvSevenDayGiftBagResponseHandle);

		}

	
		public void SendNewSignMessage (int index, int day)
		{
			FiSevenDaysPage nRequest = new FiSevenDaysPage ();
			nRequest.SendIndex = index;
			nRequest.UserDay = day;
			Debug.LogError ("nsenindex" + nRequest.SendIndex + "nRequest.UserDay" + nRequest.UserDay);
			DataControl.GetInstance ().PushSocketSnd (FiEventType.SEND_XL_SEVENDAY_BAG_STATE_INFO_NEW_RESPOSE, nRequest);
		}

		

		public void RcvSevenSignInfo (object data)
		{

			NewSevenDayInfo nInfo = (NewSevenDayInfo)Facade.GetFacade ().data.Get (FacadeConfig.UI_SEVENPART);

			FiSevenDaysPage nRequest = (FiSevenDaysPage)data;
			Debug.LogError ("nresquest" + nRequest.SendIndex);
			if (nRequest.SendIndex < 0) {
				if (nRequest.SendIndex == -111)
				{
					GameObject Window1 = UnityEngine.Resources.Load("Window/WindowTipsThree") as UnityEngine.GameObject;
					GameObject WindowClone1 = UnityEngine.GameObject.Instantiate(Window1);
					UITipAutoNoMask ClickTips1 = WindowClone1.GetComponent<UITipAutoNoMask>();
					ClickTips1.tipText.text = " 領取異常，如有疑問請聯繫客服";
					return;
				}
				else
				{
					GameObject Window1 = UnityEngine.Resources.Load("Window/WindowTipsThree") as UnityEngine.GameObject;
					GameObject WindowClone1 = UnityEngine.GameObject.Instantiate(Window1);
					UITipAutoNoMask ClickTips1 = WindowClone1.GetComponent<UITipAutoNoMask>();
					ClickTips1.tipText.text = "領取失敗";
					return;
				}

			}

			IUiMediator nMediator = Facade.GetFacade ().ui.Get (FacadeConfig.UI_SEVENPART);
			if (nMediator != null) {
				if (nRequest.SendIndex == 1) {
					nMediator.OnRecvData (1000, data);
				} else {
					nMediator.OnRecvData (1001, data);
				}

			}
		}

		//七日礼包初始话协议
		public void RecvSevenDayGiftBagResponseHandle (object data)
		{
			NewSevenDayInfo nSevenDayinfo = (NewSevenDayInfo)Facade.GetFacade ().data.Get (FacadeConfig.UI_SEVENPART);
			InitSevenDayInfos sevenifno = (InitSevenDayInfos)data;
			nSevenDayinfo.nresult = sevenifno.result;
			nSevenDayinfo.ncurDay = sevenifno.curDay;
			nSevenDayinfo.nuserDay = sevenifno.userDay;
			nSevenDayinfo.nuserDayState = sevenifno.userDayState;
			nSevenDayinfo.ntaskDay = sevenifno.taskDay;
			nSevenDayinfo.ntaskDayState = sevenifno.taskDayState;
			nSevenDayinfo.ntaskValue = sevenifno.taskValue;
			nSevenDayinfo.nuserGiftDay = sevenifno.userGiftDay;
			nSevenDayinfo.nuserGiftDyaState = sevenifno.userGiftDyaState;
//			if (sevenifno.result == -100) {
//				nSevenDayinfo.nIsOpenseven = false;
//			} else {
//				nSevenDayinfo.nIsOpenseven = true;
//			}

			if (nSevenDayinfo.nuserDay > 7) {
				nSevenDayinfo.nuserDayState = -1;
			}
			if (nSevenDayinfo.ntaskDay > 7) {
				nSevenDayinfo.ntaskDayState = -1;
			}
			if (nSevenDayinfo.nuserGiftDay > 7) {
				nSevenDayinfo.nuserGiftDyaState = -1;
			}

			if (sevenifno.result == -100) {
				UIHallCore nHallCore = (UIHallCore)Facade.GetFacade ().ui.Get (FacadeConfig.UI_HALL_MODULE_ID);
				if (nHallCore != null && nHallCore.gameObject.activeSelf) {
					//nHallCore.HindSevnDay ();
				}
			} else {
				if (!nSevenDayinfo.nIsOpenseven && AppInfo.trenchNum > 51000000) {
					Debug.Log("新手七日 NewSevenDaySign");
					//string path = "Window/NewSevenDaySign";
					//GameObject jumpObj = AppControl.OpenWindow (path);
					//jumpObj.SetActive (true);
				}
			}
		}

		public void OnDestroy ()
		{
			EventControl mEventControl = EventControl.instance ();
			mEventControl.removeEventHandler (FiEventType.RECV_XL_SEVENDAY_BAG_STATE_INFO_NEW_RESPOSE, RcvSevenSignInfo);
			mEventControl.removeEventHandler (FiEventType.RECV_XL_SEVENDAY_START_BAG_STATE_INFO_NEW_RESPOSE, RecvSevenDayGiftBagResponseHandle);

		}

	}
}



