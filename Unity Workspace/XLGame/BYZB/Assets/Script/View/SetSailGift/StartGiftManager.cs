using System;
using UnityEngine;
using UnityEngine.UI;
using AssemblyCSharp;
using DG.Tweening;

namespace AssemblyCSharp
{
	public class StartGiftManager
	{
		public StartGiftManager ()
		{
		}

		public static GameObject OpenCountDownBoxTip (Transform nParant, bool isInHall, bool playAnimation = false)
		{
			DailySignInfo nInfo = (DailySignInfo)Facade.GetFacade ().data.Get (FacadeConfig.SIGN_IN_MODULE_ID);
			nInfo.Setistowwor (true);
			GameObject nLoadObject = Resources.Load ("Window/TomorrowTips") as GameObject;
			UnityEngine.Debug.LogError ("________________!!!!!!!!!____________");
			if (nLoadObject != null) {
				GameObject nInstance = GameObject.Instantiate (nLoadObject);

				nInstance.SetActive (true);
				if (nParant != null)
					nInstance.transform.SetParent (nParant);
				Debug.LogError ("PlayAnim=" + playAnimation);
				if (playAnimation) {
					nInstance.transform.localPosition = new Vector3 (0, 0, 0);
					if (isInHall)
						nInstance.transform.DOLocalMove (new Vector3 (-350, 180, 0), 1);
					else {
						nInstance.transform.SetParent (nParant.transform.Find ("LeftOption2/StartGiftPos"));
//						nInstance.transform.DOMove (nInstance.transform.parent.position, 1f);

					}
				} else {
					if (isInHall) {
						nInstance.transform.localPosition = new Vector3 (-363.7f, -10f, 0);
						nInstance.transform.Find ("TomorrowTip").localScale = new Vector3 (0.8f, 0.8f, 0.8f);
					} else {//取消了在渔场的弹出
						// nInstance.transform.localPosition = new Vector3 (-350, 180, 0);
						//nInstance.transform.SetParent (GameObject.FindGameObjectWithTag (TagManager.uiCanvas).
      //                                                transform.Find ("LeftOption2/StartGiftPos"));
						//nInstance.transform.localPosition = nInstance.transform.parent.position;
						//nInstance.transform.SetParent(nParant.transform.FindChild("LeftOption2/StartGiftPos"));
						//nInstance.transform.localPosition = Vector3.zero;
//						nInstance.transform.localScale = new Vector3 (1f, 1f, 1f);
					}
				}
				nInstance.transform.localScale = new Vector3 (1f, 1f, 1f);
				return nInstance;
			}
			return null;
		}

		public static GameObject OpenCountDownWindow (int nDayIndex)
		{
			GameObject Window = Resources.Load ("Window/TomorrowGift")as GameObject;
			GameObject nWindowClone = GameObject.Instantiate (Window);
			if (nWindowClone != null) {
				UITomorrow nTomorrow = nWindowClone.GetComponentInChildren<UITomorrow> ();
				nTomorrow.SetDayIndex (nDayIndex);
				//nTomorrow.transform.SetParent ();
			}
			return nWindowClone;
		}

		//打开起航礼包窗口
		public static GameObject OpenStartGiftWindow ()
		{
			MyInfo myInfo = DataControl.GetInstance ().GetMyInfo ();
			if (myInfo.isGuestLogin)    //游客不下发启航礼包
                return null;
			GameObject window = Resources.Load ("Window/SetSailGift")as GameObject;
			return GameObject.Instantiate (window);
		}

		/// <summary>
		/// 开启救济金
		/// </summary>
		/// <returns>The task reward window.</returns>
		/// <param name="nParant">N parant.</param>
		/// <param name="isInHall">If set to <c>true</c> is in hall.</param>
		public static GameObject HelpTaskRewardWindow (bool isInHall)
		{
			DailySignInfo nInfo = (DailySignInfo)Facade.GetFacade ().data.Get (FacadeConfig.SIGN_IN_MODULE_ID);
			nInfo.Setisalms (true);
			GameObject helpRewardWindow = Resources.Load ("MainHall/Task/AlmsCountdown") as GameObject;
			if (helpRewardWindow != null) {
				GameObject temp = GameObject.Instantiate (helpRewardWindow);
				temp.SetActive (true);
				Transform parent;
				if (isInHall) {
					//parent = GameObject.FindGameObjectWithTag (TagManager.uiCanvas).transform;
					parent = GameObject.FindWithTag (TagManager.leftButton).transform;
					temp.transform.SetParent (parent);
					temp.transform.localPosition = new Vector3 (-309.9f, -90.6f, 0);
					temp.transform.localScale = new Vector3 (1f, 1f, 1f);
//					temp.transform.localScale = new Vector3 (0.6f, 0.6f, 0.6f);
				} else {//取消了在渔场的弹出
					//temp.transform.SetParent (GameObject.FindGameObjectWithTag (TagManager.uiCanvas).
					//	transform.Find ("LeftOption2/StartGiftPos"));
					//temp.transform.localPosition = new Vector3 (14f, -96.3f, 0);
					//temp.transform.localScale = new Vector3 (.6f, .6f, .6f);
				}
				return temp;
			}
			return null;
		}

		public static void GuestToStoreManager ()
		{
			return;
			string path = "Window/WindowTips";
			GameObject Window = Resources.Load (path)as GameObject;
			GameObject WindowClone = GameObject.Instantiate (Window);
			UITipClickHideManager ClickTips = WindowClone.GetComponent<UITipClickHideManager> ();
			ClickTips.text.text = "游客模式无法打开商城哦!";
			ClickTips.time.text = "3";
		}
	}
}

