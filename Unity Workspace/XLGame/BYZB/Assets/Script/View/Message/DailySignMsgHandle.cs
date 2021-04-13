using System;
using System.Collections.Generic;
using UnityEngine;

namespace AssemblyCSharp
{
	
	public class DailySignMsgHandle : IMsgHandle
	{
		
		public DailySignMsgHandle ()
		{
		}

		public void OnInit ()
		{
			EventControl mControl = EventControl.instance ();
			mControl.addEventHandler (FiEventType.RECV_SIGN_IN_AWARD_RESPONSE, RecvDailySignInAwardResponse);
			mControl.addEventHandler (FiEventType.RECV_CL_SIGNRETROACTIVE_RESPONSE, RecvSignInAwardResponse);

		}

		public void OnDestroy ()
		{
			EventControl mControl = EventControl.instance ();
			mControl.removeEventHandler (FiEventType.RECV_SIGN_IN_AWARD_RESPONSE, RecvDailySignInAwardResponse);
			mControl.removeEventHandler (FiEventType.RECV_CL_SIGNRETROACTIVE_RESPONSE, RecvSignInAwardResponse);
		}

		public void SendGetSignAwardRequest (int nDays)
		{
			FiSignInAwardRequest nRequest = new FiSignInAwardRequest ();
			nRequest.day = nDays;
			DataControl.GetInstance ().PushSocketSndByte (FiEventType.SEND_SIGN_IN_AWARD_REQUEST, nRequest.serialize ());
		}

		public void SendSignRetroactiveRequest (int userid, int type, int proid, int count, int reday)
		{
			//Debug.LogError (type + "s" + userid + "s" + proid + "ssss" + reday);
			FiRetroactive nRequest = new FiRetroactive ();
			nRequest.userID = userid;
			nRequest.retRoactivetype = type;
			nRequest.propID = proid;
			nRequest.count = count;
			nRequest.reday = reday;
			DataControl.GetInstance ().PushSocketSnd (FiEventType.SEND_CL_SIGNRETROACTIVE_REQUEST, nRequest);
		}

		private void RecvDailySignInAwardResponse (object data)
		{
			FiSignInAwardResponse nResponse = (FiSignInAwardResponse)data;
			//Debug.LogError ("signstatue" + nResponse.singIn.status + "ssssss" + nResponse.singIn.day + "kkaks" + nResponse.result);

			MyInfo myInfo = (MyInfo)Facade.GetFacade ().data.Get (FacadeConfig.USERINFO_MODULE_ID);
			if (nResponse.result == 0) {
				ReSignDay.isSignComplet = true;
				DailySignInfo nInfo = (DailySignInfo)Facade.GetFacade ().data.Get (FacadeConfig.SIGN_IN_MODULE_ID);
				nInfo.Setissign (false);
				//Debug.LogError ("RecvDailySignInAwardResponse");

				if (LoginUtil.GetIntance ().onshowsignchange != null) {
					LoginUtil.GetIntance ().onshowsignchange ();
				}
				//BackpackInfo backInfo = (BackpackInfo)Facade.GetFacade ().data.Get (FacadeConfig.BACKPACK_MODULE_ID);
				//MyInfo myInfo = (MyInfo)Facade.GetFacade ().data.Get (FacadeConfig.USERINFO_MODULE_ID);

				//签到奖励
				/*foreach (FiProperty one in nResponse.properties) 
				{
					switch (one.type) {
					case FiPropertyType.GOLD:
						name = "金币";
						much = one.value;
						myInfo.gold += one.value;
						if (Facade.GetFacade ().ui.Get (FacadeConfig.UI_STORE_MODULE_ID) != null) {
							Facade.GetFacade ().ui.Get (FacadeConfig.UI_STORE_MODULE_ID).OnRecvData (FiPropertyType.GOLD, myInfo.gold);
						}
						break;
					case FiPropertyType.DIAMOND:
						myInfo.diamond += one.value;
						if (Facade.GetFacade ().ui.Get (FacadeConfig.UI_STORE_MODULE_ID) != null) {
							Facade.GetFacade ().ui.Get (FacadeConfig.UI_STORE_MODULE_ID).OnRecvData (FiPropertyType.DIAMOND, myInfo.diamond);
						}
						break;
					default:
						if (one.type == FiPropertyType.FISHING_EFFECT_AIM) {
							name = "锁定";
						} else if (one.type == FiPropertyType.FISHING_EFFECT_FREEZE) {
							name = "冰冻";
						}
						backInfo.Add (one.type, one.value);
						break;
					}
				}*/


//				reward.bAddToBackPack = false;


				/*if (nResponse.singIn.status == 0 && nResponse.singIn.day < 0) {
					UnityEngine.GameObject Window = UnityEngine.Resources.Load ("Window/RewardWindow")as UnityEngine.GameObject;
					GameObject WindowClone = UnityEngine.GameObject.Instantiate (Window);
					UIReward reward = WindowClone.GetComponent<UIReward> ();
					switch (nResponse.singIn.day.ToString ()) {
					case "-3":
						reward.toolName = "钻石";
						reward.textObj.text = "10";
						break;
					case "-7":
						reward.toolName = "钻石";
						reward.textObj.text = "30";
						break;
					case "-15":
						reward.toolName = "鱼雷";
						break;
					}
				} else {
					UnityEngine.GameObject Window = UnityEngine.Resources.Load ("Window/RewardWindow")as UnityEngine.GameObject;
					GameObject WindowClone = UnityEngine.GameObject.Instantiate (Window);
					UIReward reward = WindowClone.GetComponent<UIReward> ();
					reward.toolName = name;
					reward.textObj.text = much.ToString ();
				}*/
				IUiMediator nMediator = Facade.GetFacade ().ui.Get (FacadeConfig.UI_SIGN_IN_MODULE_ID);
				if (nMediator != null) {
					nMediator.OnRecvData (FiEventType.RECV_SIGN_IN_AWARD_RESPONSE, data);
				}

			} else if (nResponse.result == -15) {
				UnityEngine.Debug.Log("當前帳號異常 領取失敗！");
				UnityEngine.GameObject
				Window = UnityEngine.Resources.Load("Window/WindowTips") as UnityEngine.GameObject;
				GameObject WindowClone = UnityEngine.GameObject.Instantiate(Window);
				UITipClickHideManager ClickTips = WindowClone.GetComponent<UITipClickHideManager>();
				ClickTips.text.text = "當前帳號異常 領取失敗!";
			}
		}

		private void RecvSignInAwardResponse (object data)
		{
			FiRetroactiveReward nResponse = (FiRetroactiveReward)data;
			Debug.LogError ("~~~~~RecvDailySignInAwardResponse nResponse.result = "+ nResponse.result);
			if (nResponse.result == 0) {
						    
			
//				BackpackInfo backInfo = (BackpackInfo)Facade.GetFacade ().data.Get (FacadeConfig.BACKPACK_MODULE_ID);
				//Debug.LogError ("resignday" + (ReSignDay.reday));
				MyInfo myInfo = (MyInfo)Facade.GetFacade ().data.Get (FacadeConfig.USERINFO_MODULE_ID);
				myInfo.Redaystatue (ReSignDay.reday);
				ReSignDay.redayback = nResponse.singIn.day;
				//Debug.LogError ("redayback" + ReSignDay.redayback);
				//这判断是因为是最后一天会下标数组越界
				for (int i = 0; i < ReSignDay.suppleday.Count; i++) {
//					//Debug.LogError("suppleday.Count"+ReSignDay.suppleday.Count+"ssssskkk"+)
					if (nResponse.singIn.day == ReSignDay.suppleday [i]) {
						if (i == ReSignDay.suppleday.Count - 1) {
							ReSignDay.reday = ReSignDay.suppleday [i] + 1;
						} else {
							ReSignDay.reday = ReSignDay.suppleday [i + 1];

						}

					}
				}

				if (LoginUtil.GetIntance ().onshowsignchange != null) {
					LoginUtil.GetIntance ().onshowsignchange ();
				}

				//Debug.LogError ("ReSignDay.reday" + ReSignDay.reday);
				//Debug.LogError ("myInfo.gold" + myInfo.gold);
				//Debug.LogError ("nResponse.properties.Count" + nResponse.properties.Count);
				//Debug.LogError ("resignday" + (ReSignDay.reday));

				if (DaySupplement.isMoneySign) {
					myInfo.gold -= 5000;
					//Debug.LogError ("myInfo.gold" + myInfo.gold);
					if (Facade.GetFacade ().ui.Get (FacadeConfig.UI_STORE_MODULE_ID) != null) {
						Facade.GetFacade ().ui.Get (FacadeConfig.UI_STORE_MODULE_ID).OnRecvData (FiPropertyType.GOLD, myInfo.gold);
					}
				}

				if (DaySupplement.isDiamondSign)
				{
					myInfo.diamond -= 5;
					//Debug.LogError ("myInfo.gold" + myInfo.gold);
					if (Facade.GetFacade().ui.Get(FacadeConfig.UI_STORE_MODULE_ID) != null)
					{
						Facade.GetFacade().ui.Get(FacadeConfig.UI_STORE_MODULE_ID).OnRecvData(FiPropertyType.DIAMOND, myInfo.diamond);
					}
				}

				//签到奖励
				/*foreach (FiProperty one in nResponse.properties) 
//				{
//					switch (one.type) {
//					case FiPropertyType.GOLD:
//						name = "金币";
//						much = one.value;
//						myInfo.gold += one.value;
//						if (Facade.GetFacade ().ui.Get (FacadeConfig.UI_STORE_MODULE_ID) != null) {
//							Facade.GetFacade ().ui.Get (FacadeConfig.UI_STORE_MODULE_ID).OnRecvData (FiPropertyType.GOLD, myInfo.gold);
//						}
//						break;
//					case FiPropertyType.DIAMOND:
//						myInfo.diamond += one.value;
//						if (Facade.GetFacade ().ui.Get (FacadeConfig.UI_STORE_MODULE_ID) != null) {
//							Facade.GetFacade ().ui.Get (FacadeConfig.UI_STORE_MODULE_ID).OnRecvData (FiPropertyType.DIAMOND, myInfo.diamond);
//						}
//						break;
//					default:
//						if (one.type == FiPropertyType.FISHING_EFFECT_AIM) {
//							name = "锁定";
//						} else if (one.type == FiPropertyType.FISHING_EFFECT_FREEZE) {
//							name = "冰冻";
//						}
//						backInfo.Add (one.type, one.value);
//						break;
//					}
//				}*/
				//

				//				reward.bAddToBackPack = false;









				/*if (nResponse.singIn.status == 0 && nResponse.singIn.day < 0) {
					UnityEngine.GameObject Window = UnityEngine.Resources.Load ("Window/RewardWindow")as UnityEngine.GameObject;
					GameObject WindowClone = UnityEngine.GameObject.Instantiate (Window);
					UIReward reward = WindowClone.GetComponent<UIReward> ();
					switch (nResponse.singIn.day.ToString ()) {
					case "-3":
						reward.toolName = "钻石";
						reward.textObj.text = "10";
						break;
					case "-7":
						reward.toolName = "钻石";
						reward.textObj.text = "30";
						break;
					case "-15":
						reward.toolName = "鱼雷";
						break;
					}
				} else {
					UnityEngine.GameObject Window = UnityEngine.Resources.Load ("Window/RewardWindow")as UnityEngine.GameObject;
					GameObject WindowClone = UnityEngine.GameObject.Instantiate (Window);
					UIReward reward = WindowClone.GetComponent<UIReward> ();
					reward.toolName = name;
					reward.textObj.text = much.ToString ();
				}*/

				IUiMediator nMediator = Facade.GetFacade ().ui.Get (FacadeConfig.UI_SIGN_IN_MODULE_ID);
				if (nMediator != null) {
					nMediator.OnRecvData (FiEventType.RECV_CL_SIGNRETROACTIVE_RESPONSE, data);
				}
			} else if (nResponse.result == -15) {
				UnityEngine.Debug.Log("當前賬號異常 領取失敗！");
				UnityEngine.GameObject
				Window = UnityEngine.Resources.Load("Window/WindowTips") as UnityEngine.GameObject;
				GameObject WindowClone = UnityEngine.GameObject.Instantiate(Window);
				UITipClickHideManager ClickTips = WindowClone.GetComponent<UITipClickHideManager>();
				ClickTips.text.text = "當前賬號異常 領取失敗!";
			} 
		}

	}

}

