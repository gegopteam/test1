using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AssemblyCSharp
{
	public enum GiftBagType
	{
		Awesome,
		Double,
		Treasure
	}

	public class DarGonMsgHandle : IMsgHandle
	{
		public DarGonMsgHandle ()
		{

		}

		public void OnInit ()
		{
			EventControl mEventControl = EventControl.instance ();
			mEventControl.addEventHandler (FiEventType.RECV_CL_USE_DARGON_CARD_RESPONSE, RcvDarGonCardResponseHandle);
			mEventControl.addEventHandler (FiEventType.RECV_PURCHASE_DARGON_CARD_RESPONSE, RcvPurchaseDarGonCardResponseHandle);

			mEventControl.addEventHandler (FiEventType.RECV_CL_USER_PREFERENTIAL_REQUEST, RcvePreferentialResponseHandle);
			mEventControl.addEventHandler (FiEventType.RECV_PURCHASE_TEHUI_CARD_RESPONSE, RcvePurPreferentialResponseHandle);
			mEventControl.addEventHandler (FiEventType.RECV_XL_TOP_UP_GIFT_BAG_STATE_INFO_NEW_RESPOSE, RecvGiftBagResponseHandle);
			//mEventControl.addEventHandler (FiEventType.RECV_CL_USE_DARGON_CARD_RESPONSE, RcvDarGonCardResponseHandle);
		}

		

		//接收龙卡显示信息
		private void RcvDarGonCardResponseHandle (object data)
		{
//			////Debug.Log ("接收龙卡显示信息");
			FiDraGonRewardData nResult = (FiDraGonRewardData)data;
            Debug.Log(nResult + "接收龙卡显示信息");
            DragonCardInfo nDragoninfo = (DragonCardInfo)Facade.GetFacade ().data.Get (FacadeConfig.UI_DRAGONCARD);
			MyInfo nuserinfo = (MyInfo)Facade.GetFacade ().data.Get (FacadeConfig.USERINFO_MODULE_ID);
			nDragoninfo.AppendDgData (nResult);
			nDragoninfo.IsPurDragonTypeArray = nResult.DarGonCardDataArray;
//			////Debug.Log ("nResult.cannonmultiplemax" + nResult.cannonmultiplemax);
//			////Debug.Log ("nResult.DarGonCardDataArray" + nResult.DarGonCardDataArray.Count);
			for (int i = 0; i < nDragoninfo.IsPurDragonTypeArray.Count; i++) {
//				////Debug.Log ("nDragoninfo.IsPurDragonTypeArray" + nDragoninfo.IsPurDragonTypeArray [i]);
				if (nDragoninfo.IsPurDragonTypeArray [i] == 0) {
					if (nResult.cannonmultiplemax >= 9900) {
						nuserinfo.cannonMultipleMax = nResult.cannonmultiplemax;
					}
				}
			}

			for (int i = 0; i < nResult.DarGonCardDataArray.Count; i++) {
				if (nResult.DarGonCardDataArray [i] > 0) {
					nuserinfo.misHaveDraCard = true;
					return;
				} 
			}
		}

		GiftBagType giftBagType;
		//接收特惠购买后的信息
		public void RcvePurPreferentialResponseHandle (object data)
		{
			FiPurChaseTehuiRewradData nResult = (FiPurChaseTehuiRewradData)data;
			DragonCardInfo nDragoninfo = (DragonCardInfo)Facade.GetFacade ().data.Get (FacadeConfig.UI_DRAGONCARD);
			MyInfo nUserInfo = (MyInfo)Facade.GetFacade ().data.Get (FacadeConfig.USERINFO_MODULE_ID);
//			////Debug.Log ("RcvePurPreferentialResponseHandle" + nResult.result);
//			////Debug.Log ("/" + nResult.cardType + "/" + nResult.result + "/" + nResult.prop.Count + "/" + nResult.current_vip + "/" + nResult.total_recharge);
			for (int i = 0; i < nResult.prop.Count; i++) {
				Debug.LogError ("nresult proey" + nResult.prop [i].type + "nResult.prop [i].value" + nResult.prop [i].value);
			}
			//Debug.LogError ("接收特惠购买后的信息===nResult.result===" + nResult.result);
			//Debug.LogError ("接收特惠购买后的信息===nResult.nResult.total_recharge===" + nResult.total_recharge);
			//Debug.LogError ("接收特惠购买后的信息===nResult.current_vip===" + nResult.current_vip);
			//Debug.LogError ("接收特惠购买后的信息=== nResult.cardType===" + nResult.cardType);
			//Debug.LogError ("接收特惠购买后的信息===nResult.userid===" + nResult.userid);
			//Debug.LogError ("接收特惠购买后的信息===nResult.cannonmultiplemax===" + nResult.cannonmultiplemax);

			if (nResult.result == 0) {

				//渠道宏定义
#if UNITY_Huawei
                                Facade.GetFacade().message.backpack.SendReloadRequest();
                                Facade.GetFacade().message.toolPruchase.ShowTopUpWinodw();
#elif UNITY_OPPO
                Facade.GetFacade().message.backpack.SendReloadRequest();
                Facade.GetFacade().message.toolPruchase.ShowTopUpWinodw();
#elif UNITY_VIVO
                                Facade.GetFacade().message.backpack.SendReloadRequest();
                                Facade.GetFacade().message.toolPruchase.ShowTopUpWinodw();
#endif

				Debug.LogError ("接收特惠购买后的信息礼品进入刷新0000");
				//              ////Debug.Log ("[ vip level ]" + nUserInfo.levelVip + " / " + nResult.current_vip);
				if (nUserInfo.levelVip < nResult.current_vip) {
					string path = "Window/UpgradeWindow";
					GameObject WindowClone = AppControl.OpenWindow (path);
					WindowClone.SetActive (true);
					WindowClone.GetComponent<UIUpgrade> ().SetVipInfo (nResult.current_vip);
					if (UIUserInfo.instance != null) {
						UIUserInfo.instance.SetVipLevel (nResult.current_vip);
					}
				}
				//////Debug.Log ("111111111111111111111111111" + nResult.current_vip);
				nUserInfo.SetVip ((int)nResult.total_recharge, nResult.current_vip);
				if (StoreMiddle.instans != null)
					StoreMiddle.instans.Refresh ();
				//在大厅再显示    
				if (UIUserDetail.instace != null) {
					UIUserDetail.instace.FreshVip (UIHallTexturers.instans.VipFrame [nResult.current_vip]);
				} else {
				}
				IUiMediator nMediator = Facade.GetFacade ().ui.Get (FacadeConfig.UI_TEIHUI);
				if (nMediator != null) {
					nMediator.OnRecvData (FiEventType.RECV_PURCHASE_TEHUI_CARD_RESPONSE, data);
				}

               
			}
			if (nResult.cardType == 13 || nResult.cardType == 14 || nResult.cardType == 15 || nResult.cardType == 16 || nResult.cardType == 17 || nResult.cardType == 18 || nResult.cardType == 19) {
				IUiMediator nMediator = Facade.GetFacade ().ui.Get (FacadeConfig.UI_SEVENPART);
				if (nMediator != null) {
					nMediator.OnRecvData (1002, data);
				}
			}
			//购买超给力礼包成功操作
			if (nResult.cardType == 6) {
				if (AwesomeCommand.Instance != null && AwesomeCommand.Instance.gameObject != null)
					UnityEngine.Object.Destroy (AwesomeCommand.Instance.gameObject);
				nUserInfo.showAwesome = 2;

				Debug.Log ("接收特惠购买后的信息购买超給力礼包成功操作00000" + nUserInfo.showAwesome);
				//for (int i = 0; i < nResult.prop.Count; i++)
				//{
				//    Debug.LogError("接收特惠购买后的信息购买超给力礼包成功操作for" + nResult.prop.Count + "skill" + FiPropertyType.GetGiftSkill(nResult.prop[i].type) + "数量" + nResult.prop[i].value);
				//    if (PrefabManager._instance != null)
				//    {
				//        if (FiPropertyType.GetGiftSkill(nResult.prop[i].type) != SkillType.Error)
				//        {
				//            PrefabManager._instance.GetSkillUIByType(FiPropertyType.GetGiftSkill(nResult.prop[i].type)).AddRestNum(nResult.prop[i].value);
				//        }
				//        else if (nResult.prop[i].type == FiPropertyType.GOLD)
				//        {
				//            PrefabManager._instance.GetLocalGun().gunUI.AddValue(0, nResult.prop[i].value, 0);
				//        }
				//        else if (nResult.prop[i].type == FiPropertyType.DIAMOND)
				//        {
				//            PrefabManager._instance.GetLocalGun().gunUI.AddValue(0, 0, nResult.prop[i].value);
				//        }
				//    }
				//}
				FishingPrizeAdd (nResult);
				Debug.LogError ("接收特惠购买后的信息购买超給力礼包成功操作11111" + nUserInfo.showAwesome);
				//如果购买成功之后就隐藏一元礼包
				if (GiftBagManager.Instance != null && GiftBagManager.Instance.awesome != null)
					GiftBagManager.Instance.awesome.SetActive (false);
				Debug.LogError ("接收特惠购买后的信息购买超給力礼包成功操作2222" + nUserInfo.showAwesome);
			}

			//够买双喜临门成功之后操作
			if (nResult.cardType == 10 || nResult.cardType == 11) {
				if (DoubleCommand.Instance != null && DoubleCommand.Instance.gameObject != null)
					UnityEngine.Object.Destroy (DoubleCommand.Instance.gameObject);
				nUserInfo.showDouble = 2;
				FishingPrizeAdd (nResult);
                if (nUserInfo.showDouble == 2 && UIHallCore.Instance != null && UIHallCore.Instance.transform != null)
                    UIHallCore.Instance.doubleButton.SetActive(false);
            }
			//购买发现宝藏礼包成功操作
			if (nResult.cardType == 7 || nResult.cardType == 8 || nResult.cardType == 9) {
				//销毁发现宝藏礼包界面
				if (TreasureCommand.Instance != null && TreasureCommand.Instance.gameObject != null)
					UnityEngine.Object.Destroy (TreasureCommand.Instance.gameObject);
				nUserInfo.showTreasure = 2;
				FishingPrizeAdd (nResult);
				//销毁发现宝藏大厅礼包图标
				if (nUserInfo.showTreasure == 2 && UIHallCore.Instance != null && UIHallCore.Instance.transform != null)
					UIHallCore.Instance.treasureButton.SetActive (false);
			}
		}
		//接收龙卡购买后的信息
		private void RcvPurchaseDarGonCardResponseHandle (object data)
		{
			Debug.Log(" 購買龍卡接收 RcvPurchaseDarGonCardResponseHandle ");
			FiPurChaseDraGonRewradData nResult = (FiPurChaseDraGonRewradData)data;
			DragonCardInfo nDragoninfo = (DragonCardInfo)Facade.GetFacade ().data.Get (FacadeConfig.UI_DRAGONCARD);
			MyInfo nUserInfo = (MyInfo)Facade.GetFacade ().data.Get (FacadeConfig.USERINFO_MODULE_ID);
//			////Debug.Log ("RcvPurchaseDarGonCardResponseHandle" + nResult.result);
			////Debug.Log ("/" + nResult.cardType + "/" + nResult.result + "/" + nResult.prop.Count);
			for (int i = 0; i < nResult.prop.Count; i++) {
				Debug.Log ("nresult proey" + nResult.prop [i].type + "nResult.prop [i].value" + nResult.prop [i].value);
			}
			if (nResult.result == 0) {

				//渠道宏定义
#if UNITY_Huawei
                                Facade.GetFacade().message.backpack.SendReloadRequest();
                                Facade.GetFacade().message.toolPruchase.ShowTopUpWinodw();
#elif UNITY_OPPO
                Facade.GetFacade().message.backpack.SendReloadRequest();
                Facade.GetFacade().message.toolPruchase.ShowTopUpWinodw();
#elif UNITY_VIVO
                                Facade.GetFacade().message.backpack.SendReloadRequest();
                                Facade.GetFacade().message.toolPruchase.ShowTopUpWinodw();
#endif
				////Debug.Log ("nResult.cannonmultiplemax;" + nResult.cannonmultiplemax + "nResult.cardType" + nResult.cardType);
				nUserInfo.misHaveDraCard = true;
				if (nUserInfo.cannonMultipleMax >= 15000 && nResult.cardType == 3) {
					nUserInfo.cannonMultipleMax = 30000;
				} else {
					////Debug.Log ("22221111222" + nUserInfo.cannonMultipleMax);
					if (nResult.cardType != 3 && nUserInfo.cannonMultipleMax <= 15000) {
						nUserInfo.cannonMultipleMax = 20000;
					} else {
						nUserInfo.cannonMultipleMax = 30000;
					}
				}
				UIClassic.Instance.InitFieldGrade ();
				////Debug.Log ("viplevel" + nUserInfo.levelVip + " / " + nResult.current_vip + "/" + nResult.total_recharge);
				for (int i = 0; i < nDragoninfo.IsPurDragonTypeArray.Count; i++) {
					if (nResult.cardType == i + 1) {
						nDragoninfo.IsPurDragonTypeArray [i] = 1;
					}
				}

				/*s	if (nUserInfo.levelVip < nResult.current_vip) {
					string path = "Window/UpgradeWindow";
					GameObject WindowClone = AppControl.OpenWindow (path);
					WindowClone.SetActive (true);
					WindowClone.GetComponent<UIUpgrade> ().SetVipInfo (nResult.current_vip);
					if (UIUserInfo.instance != null) {
						UIUserInfo.instance.SetVipLevel (nResult.current_vip);
					}
				}*/
				nUserInfo.SetVip ((int)nResult.total_recharge, nResult.current_vip);

				if (StoreMiddle.instans != null)
					StoreMiddle.instans.Refresh ();
				//在大厅再显示
				if (UIUserDetail.instace != null) {
					UIUserDetail.instace.FreshVip (UIHallTexturers.instans.VipFrame [nResult.current_vip]);
				} else {

				}
				IUiMediator nMediator = Facade.GetFacade ().ui.Get (FacadeConfig.UI_DRAGONCARD);
				if (nMediator != null) {
					nMediator.OnRecvData (FiEventType.RECV_PURCHASE_DARGON_CARD_RESPONSE, data);
				}
			}
		}
		//接收特惠显示
		private void RcvePreferentialResponseHandle (object data)
		{
//			//Debug.Log ("特惠显示数据" + data);
			FiPreferentialData result = (FiPreferentialData)data;
			MyInfo nUserInfo = (MyInfo)Facade.GetFacade ().data.Get (FacadeConfig.USERINFO_MODULE_ID);
			DragonCardInfo dragonCardInfo = (DragonCardInfo)Facade.GetFacade ().data.Get (FacadeConfig.UI_DRAGONCARD);
			dragonCardInfo.LoginShowPreferentialData (result);
			Debug.Log (result.preferentialDataArray.Count + "特惠1");
			Debug.Log (result.preferentialDataArray [0] + "特惠");
			nUserInfo.teihui = result.preferentialDataArray [0];
			if (result.preferentialDataArray[0] < 5)
				nUserInfo.isFinishTH = false;
            else
				nUserInfo.isFinishTH = true;
		}

		GameObject Window_1;
		GameObject obj_1;
		GameObject Window_2 = null;
		GameObject obj_2 = null;
		GameObject Window_3 = null;
		GameObject obj_3 = null;
		static bool isGiftBag_1 = false;
		//礼包1面板 是否弹过 默认flase 没有谈过
		static bool isGiftBag_2 = false;
		static bool isGiftBag_3 = false;
		GunControl tempGun = null;

		/// <summary>
		/// 接收 给力,宝藏,双喜 是否显示 
		/// </summary>
		/// <param name="data">Data.</param>
		private void RecvGiftBagResponseHandle (object data)
		{
			//0 没有显示过礼包界面
			//1 显示过没有购买
			//2 是已经购买过
			GetTopUpGiftBagState result = (GetTopUpGiftBagState)data;
			MyInfo nUserInfo = (MyInfo)Facade.GetFacade ().data.Get (FacadeConfig.USERINFO_MODULE_ID);
			Debug.LogError ("接收 給力 是否显示==1111==" + result.one_giftBagState);
			Debug.LogError ("接收 双喜 是否显示==2222==" + result.two_select_oneBagState);
			Debug.LogError ("接收 宝藏 是否显示==3333==" + result.three_select_oneBagState);
			nUserInfo.showAwesome = result.one_giftBagState;
			nUserInfo.showDouble = result.two_select_oneBagState;
			nUserInfo.showTreasure = result.three_select_oneBagState;
			RoomInfo myRoomInfo = DataControl.GetInstance ().GetRoomInfo ();
			MyInfo myInfo = DataControl.GetInstance ().GetMyInfo ();
		}

		/// <summary>
		/// 判读某个用户金币是否不足
		/// </summary>
		private bool IsGlod (int userId)
		{
			if (tempGun == null) {
				tempGun = PrefabManager._instance.GetGunByUserID (userId);//得到某个用户的GunCtroll
			}
			return !tempGun.CheckCanFire ();
		}

		/// <summary>
		/// 渔场内奖励刷新
		/// </summary>
		/// <param name="nResult">N result.</param>
		void FishingPrizeAdd (FiPurChaseTehuiRewradData nResult)
		{
			if (PrefabManager._instance != null) {
				for (int i = 0; i < nResult.prop.Count; i++) {
					Debug.LogError ("接收特惠购买后的信息购买超給力礼包成功操作for" + nResult.prop.Count + "skill" + FiPropertyType.GetGiftSkill (nResult.prop [i].type) + "数量" + nResult.prop [i].value);

					if (FiPropertyType.GetGiftSkill (nResult.prop [i].type) != SkillType.Error) {
						PrefabManager._instance.GetSkillUIByType (FiPropertyType.GetGiftSkill (nResult.prop [i].type)).AddRestNum (nResult.prop [i].value);
					} else if (nResult.prop [i].type == FiPropertyType.GOLD) {
						PrefabManager._instance.GetLocalGun ().gunUI.AddValue (0, nResult.prop [i].value, 0);
					} else if (nResult.prop [i].type == FiPropertyType.DIAMOND) {
						PrefabManager._instance.GetLocalGun ().gunUI.AddValue (0, 0, nResult.prop [i].value);
					}
				}
			}
		}

		public void OnDestroy ()
		{
			EventControl mEventControl = EventControl.instance ();
			mEventControl.removeEventHandler (FiEventType.RECV_CL_USE_DARGON_CARD_RESPONSE, RcvDarGonCardResponseHandle);
			mEventControl.removeEventHandler (FiEventType.RECV_PURCHASE_DARGON_CARD_RESPONSE, RcvPurchaseDarGonCardResponseHandle);

			mEventControl.removeEventHandler (FiEventType.RECV_CL_USER_PREFERENTIAL_REQUEST, RcvePreferentialResponseHandle);
			mEventControl.removeEventHandler (FiEventType.RECV_PURCHASE_TEHUI_CARD_RESPONSE, RcvePurPreferentialResponseHandle);
			mEventControl.removeEventHandler (FiEventType.RECV_XL_TOP_UP_GIFT_BAG_STATE_INFO_NEW_RESPOSE, RecvGiftBagResponseHandle);
		}
	}
}