using System;
using UnityEngine;
using System.Collections.Generic;
using Google.Protobuf;
using System.Collections;

namespace AssemblyCSharp
{
	public class PurchaseMsgHandle : IMsgHandle
	{
		
		List<FiTopUpInform> mTopUpList = new List<FiTopUpInform> ();

		public PurchaseMsgHandle ()
		{
		}

		public void OnInit ()
		{
			//GameObject Window = Resources.Load ("Window/WindowTips")as GameObject;
			//GameObject WindowClone = GameObject.Instantiate (Window);
			//UITipClickHideManager ClickTip = WindowClone.GetComponent<UITipClickHideManager> ();
			mTopUpList = new List<FiTopUpInform> ();
			//string nStringName = nInform.property.type == FiPropertyType.GOLD ? "金币" : "钻石";
			//ClickTip.text.text = "充值成功\n获得" ;//+ nInform.property.value + " " + nStringName;
			EventControl mControl = EventControl.instance ();
			mControl.addEventHandler (FiEventType.RECV_PURCHASE_PROPERTY_RESPONSE, RecvPurchaseResponse);

			mControl.addEventHandler (FiEventType.RECV_TOPUP_RESPONSE, RecvTopUpResponse);
			mControl.addEventHandler (FiEventType.RECV_TOP_UP_INFORM, RecvTopUpInform);
			mControl.addEventHandler (FiEventType.RECV_GET_MONTHLY_PACK_RESPONSE, RecvMothlyPackResponse);
			mControl.addEventHandler (FiEventType.RECV_EXCHANGE_DIAMOND_RESPONSE, RecvDiamondkResponse);
			mControl.addEventHandler (FiEventType.RECV_NOTIFY_EXCHANGE_DIAMOND, RecvOtherDiamondChange);

			mControl.addEventHandler (FiEventType.RECV_GET_PAY_STATE_RESPONSE, RecvPayStateResponse);
			mControl.addEventHandler (FiEventType.RECV_GET_FIRST_PAY_REWARD_RESPONSE, RecvPayRewardResponse);

			mControl.addEventHandler (FiEventType.RECV_NOTIFY_PURCHASE_PROPERTY, RecvPurchasePropertyResponse);
			mControl.addEventHandler (FiEventType.RECV_XL_EXCHANGEBARBETTE_RESPONSE, RecvBarbetteResponse);

		}

		void RecvPurchasePropertyResponse (object data)
		{
			NotifyOtherPurchaseProperty nData = (NotifyOtherPurchaseProperty)data;
			if (PrefabManager._instance == null)
				return;
			GunControl tempGun = PrefabManager._instance.GetGunByUserID (nData.UserId);
			if (tempGun != null) {
				tempGun.gunUI.AddValue (0, 0, -(int)nData.DiamondCost);
			} else {
				Debug.LogError ("Error! Purchase failed,userId=" + nData.UserId);
			}
		}


		void RecvPayStateResponse (object data)
		{
			GetPayStateByNoResponse nResult = (GetPayStateByNoResponse)data;
			if (nResult.Result == 0) {
				MyInfo nUserInfo = (MyInfo)Facade.GetFacade ().data.Get (FacadeConfig.USERINFO_MODULE_ID);
				if (nUserInfo.loginInfo.preferencePackBought == 0) {
					nUserInfo.loginInfo.preferencePackBought = 1;
				}
				//0未支付 ， 1 已经支付
				if (nResult.State == 2) {
					//显示领取按钮
					UIFirstRecharge.SetState = 1;
					//隐藏锁
				
				} else {
					UIFirstRecharge.SetState = 0;
				}
			}
		}

		void RecvPayRewardResponse (object data)
		{
			MyInfo nUserInfo = (MyInfo)Facade.GetFacade ().data.Get (FacadeConfig.USERINFO_MODULE_ID);
			GetFirstPayRewardResponse nResult = (GetFirstPayRewardResponse)data;
			if (nResult.Result == 0) {

				//-----------如果是游客状态,更改领取奖励脚本的状态,在true状态下,点击确定按钮会执行本脚本中的显示弹窗方法
				if (nUserInfo.isGuestLogin) {
					UIReward.isClose = true;
				}

				UnityEngine.GameObject Window = UnityEngine.Resources.Load ("Window/RewardWindow")as UnityEngine.GameObject;
				GameObject WindowClone = UnityEngine.GameObject.Instantiate (Window);

				UIReward reward = WindowClone.GetComponent<UIReward> ();
				IEnumerator<PB_Property> nPropList = nResult.Props.GetEnumerator ();
				
				List<FiProperty> nArray = new List<FiProperty> ();
				while (nPropList.MoveNext ()) {
					if (nPropList.Current.PropertyType != 1009) {
						FiProperty nSingle = new FiProperty ();
						nSingle.type = nPropList.Current.PropertyType;
						nSingle.value = nPropList.Current.Sum;
						nArray.Add (nSingle);
					} else {
						
						nUserInfo.cannonMultipleMax = nPropList.Current.Sum;
					}
				}
				reward.SetRewardData (nArray);
				UIHallCore nHallCore = (UIHallCore)Facade.GetFacade ().ui.Get (FacadeConfig.UI_HALL_MODULE_ID);
				if (nHallCore != null && nHallCore.gameObject.activeSelf) {
					nHallCore.BtnPreference.SetActive (false);
					nHallCore.OnProcessStartGift ();
				}

				nUserInfo.loginInfo.preferencePackBought = 2;
				if (AppInfo.isInHall) {
					if (UIClassic.Instance != null) {
						UIClassic.Instance.NewLock.SetActive (false);
						UIClassic.Instance.DeepLock.SetActive (false);
						UIClassic.Instance.GodLock.SetActive (false);
					}
				}

			} else {
				GameObject Window1 = UnityEngine.Resources.Load ("Window/WindowTipsThree")as UnityEngine.GameObject;
				GameObject WindowClone1 = UnityEngine.GameObject.Instantiate (Window1);
				UITipAutoNoMask ClickTips1 = WindowClone1.GetComponent<UITipAutoNoMask> ();
				ClickTips1.tipText.text = "領取失敗";
			}
		}

		public void SendPayStateRequest (string nTradeNum)
		{
			GetPayStateByNoRequest nRequest = new GetPayStateByNoRequest ();
			nRequest.TradeNumber = nTradeNum;
			DataControl.GetInstance ().PushSocketSndByte (FiEventType.SEND_CL_GET_PAY_STATE_REQUEST, nRequest.ToByteArray ());
		}

		public void SendGetPayRewardRequest ()
		{
			DataControl.GetInstance ().PushSocketSndByte (FiEventType.SEND_GET_FIRST_PAY_REWARD_REQUEST, null);
		}



		public void RecvOtherDiamondChange (object data)
		{
			NotifyExchangeDiamond nResult = (NotifyExchangeDiamond)data;
			if (PrefabManager._instance != null) {
				PrefabManager._instance.GetGunByUserID ((int)nResult.UserId).gunUI.AddValue (0, -(int)nResult.Gold, (int)nResult.Diamond);
				;//.GetGunByuseId（ nResult ）.gunUI.addValue	
			}
		}

		public void RecvDiamondkResponse (object data)
		{
			ExchangeDiamondResponse nResult = (ExchangeDiamondResponse)data;
			if (nResult.Result == 0) {

				MyInfo nUserInfo = (MyInfo)Facade.GetFacade ().data.Get (FacadeConfig.USERINFO_MODULE_ID);
				nUserInfo.gold -= nResult.Gold;
				nUserInfo.diamond += nResult.Diamond;
				Debug.Log (nUserInfo.diamond);
				GameObject Window = Resources.Load ("Window/WindowTips")as GameObject;
				GameObject WindowClone = GameObject.Instantiate (Window);
				UITipClickHideManager ClickTip = WindowClone.GetComponent<UITipClickHideManager> ();
				ClickTip.text.text = "兌換成功\n獲得" + (nResult.Diamond) + "鑽石";

				if (PrefabManager._instance != null) {
					PrefabManager._instance.GetLocalGun ().gunUI.AddValue (0, -(int)nResult.Gold, (int)nResult.Diamond);//GunUI
					//PrefabManager._instance.GetLocalGun().currentGold = nUserInfo.gold;
					//PrefabManager._instance.GetLocalGun ().curretnDiamond = (int)nUserInfo.diamond;
				}

				if (Facade.GetFacade ().ui.Get (FacadeConfig.UI_STORE_MODULE_ID) != null) {
					Facade.GetFacade ().ui.Get (FacadeConfig.UI_STORE_MODULE_ID).OnRecvData (FiPropertyType.GOLD, nUserInfo.gold);
					Facade.GetFacade ().ui.Get (FacadeConfig.UI_STORE_MODULE_ID).OnRecvData (FiPropertyType.DIAMOND, nUserInfo.diamond);
					Facade.GetFacade ().ui.Get (FacadeConfig.UI_STORE_MODULE_ID).OnRecvData (FiPropertyType.ROOM_CARD, nUserInfo.roomCard);
				}
				if (Facade.GetFacade ().ui.Get (FacadeConfig.UI_NEWSTORE_SHOW) != null) {
					Facade.GetFacade ().ui.Get (FacadeConfig.UI_NEWSTORE_SHOW).OnRecvData (FiPropertyType.GOLD, nUserInfo.gold);
					Facade.GetFacade ().ui.Get (FacadeConfig.UI_NEWSTORE_SHOW).OnRecvData (FiPropertyType.DIAMOND, nUserInfo.diamond);
				}

			} else {
				GameObject Window = Resources.Load ("Window/WindowTips")as GameObject;
				GameObject WindowClone = GameObject.Instantiate (Window);
				UITipClickHideManager ClickTip = WindowClone.GetComponent<UITipClickHideManager> ();
				ClickTip.text.text = "鑽石兌換失敗!";
			}
		}

		/// <summary>
		/// 发送兑换炮座请求
		/// </summary>
		/// <param name="nDiamondCount">N diamond count.</param>
		public void SendExchangeBarbetteRequest (int _buyType)
		{
			FiExchangeBarbette nRequest = new FiExchangeBarbette ();
			nRequest.buyType = _buyType;
			Debug.LogError ("SendExchangeBarbetteRequest nRequest.buyType ------" + nRequest.buyType);
			DataControl.GetInstance ().PushSocketSnd (FiEventType.SEND_XL_EXCHANGEBARBETTE_REQUEST, nRequest);
		}

		/// <summary>
		/// 發送金幣購買龍卡请求
		/// </summary>
		/// <param name="nDiamondCount">N diamond count.</param>
		public void SendGoldBuyDragonGiftRequest(int _buyType)
		{
			FiExchangeBarbette nRequest = new FiExchangeBarbette();
			nRequest.buyType = _buyType;
			Debug.LogError("SendGoldBuyDragonGiftRequest nRequest.buyType ------" + nRequest.buyType);
			DataControl.GetInstance().PushSocketSnd(FiEventType.XL_BUY_DRAGONCARD_REQUEST, nRequest);
		}

		/// <summary>
		/// 接受炮座
		/// </summary>
		/// <param name="data">Data.</param>
		private void RecvBarbetteResponse (object data)
		{
			FiExchangeBarbette nResult = (FiExchangeBarbette)data;
			Debug.LogError("nResult.result = " + nResult.result);
			Debug.LogError("nResult.goldCost = " + nResult.goldCost);
			Debug.LogError("nResult.buyType = " + nResult.buyType);
			//小于0就兑换失败
			if (nResult.result == 0) {

				MyInfo nUserInfo = (MyInfo)Facade.GetFacade ().data.Get (FacadeConfig.USERINFO_MODULE_ID);

				nUserInfo.gold -= nResult.goldCost;

				//这里弹出新的奖励面板
				string path = "Window/BarbetteBuyCanvas";
				if (nResult.buyType > 6000 && nResult.buyType < 6010)
				{
					UIBatteryBuy._Instance.OnExit();
				}
				else {
					GameObject obj = AppControl.OpenWindow(path);
					obj.SetActive(true);
				}

				if (UIBarbette.Instance != null ) {
					UIBarbette.Instance.SetBarbetteImage (nResult.buyType);
				}

				if (PrefabManager._instance != null) {
					PrefabManager._instance.GetLocalGun ().gunUI.AddValue (0, -(int)nResult.goldCost, 0);
				}

				if (nResult.buyType >= 6000) {
					Debug.LogError("  新增購買砲座 "+nResult.buyType);
					BackpackInfo nBackInfo = (BackpackInfo)Facade.GetFacade().data.Get(FacadeConfig.BACKPACK_MODULE_ID);
					FiBackpackProperty nCannonGun = new FiBackpackProperty();
					nCannonGun.canGiveAway = false;
					nCannonGun.count = 1;
					nCannonGun.type = UIBackPack_Grids.CANNON;
					nCannonGun.id = 3000 + (nResult.buyType % 6000);  //炮台样式索引
					nCannonGun.description = FiPropertyType.GetCannonName(nCannonGun.id);
					nCannonGun.useable = false;
					nCannonGun.name = FiPropertyType.GetCannonName(nCannonGun.id);
					nBackInfo.Add(nCannonGun);
				}

				if (Facade.GetFacade ().ui.Get (FacadeConfig.UI_STORE_MODULE_ID) != null) {
					Facade.GetFacade ().ui.Get (FacadeConfig.UI_STORE_MODULE_ID).OnRecvData (FiPropertyType.GOLD, nUserInfo.gold);
					Facade.GetFacade ().ui.Get (FacadeConfig.UI_STORE_MODULE_ID).OnRecvData (FiPropertyType.DIAMOND, nUserInfo.diamond);
					Facade.GetFacade ().ui.Get (FacadeConfig.UI_STORE_MODULE_ID).OnRecvData (FiPropertyType.ROOM_CARD, nUserInfo.roomCard);
				}
				if (Facade.GetFacade ().ui.Get (FacadeConfig.UI_NEWSTORE_SHOW) != null) {
					Facade.GetFacade ().ui.Get (FacadeConfig.UI_NEWSTORE_SHOW).OnRecvData (FiPropertyType.GOLD, nUserInfo.gold);
					Facade.GetFacade ().ui.Get (FacadeConfig.UI_NEWSTORE_SHOW).OnRecvData (FiPropertyType.DIAMOND, nUserInfo.diamond);
					Facade.GetFacade ().ui.Get (FacadeConfig.UI_NEWSTORE_SHOW).OnRecvData (FiPropertyType.ROOM_CARD, nUserInfo.roomCard);
				}
				IUiMediator nMediator = Facade.GetFacade ().ui.Get (FacadeConfig.UI_NEWSTORE_SHOW);

				if (nMediator != null) {
					Debug.LogError ("RecvBarbetteResponse OnRecvData");
					nMediator.OnRecvData (FiEventType.RECV_XL_EXCHANGEBARBETTE_RESPONSE, data);
				}
			} else {
				GameObject Window = Resources.Load ("Window/WindowTips")as GameObject;
				GameObject WindowClone = GameObject.Instantiate (Window);
				UITipClickHideManager ClickTip = WindowClone.GetComponent<UITipClickHideManager> ();
				ClickTip.text.text = "炮座購買失敗!";
			}
		}

		public void SendExchangeDiamondRequest (int nDiamondCount)
		{
			ExchangeDiamondRequest nRequest = new ExchangeDiamondRequest ();
			nRequest.Diamond = nDiamondCount;
			//Debug.LogError ( "-=------------" + nRequest );
			DataControl.GetInstance ().PushSocketSndByte (FiEventType.SEND_EXCHANGE_DIAMOND_REQUEST, nRequest.ToByteArray ());
		}


		public void OnDestroy ()
		{
			mTopUpList.Clear ();
			EventControl mControl = EventControl.instance ();
			mControl.removeEventHandler (FiEventType.RECV_PURCHASE_PROPERTY_RESPONSE, RecvPurchaseResponse);
			mControl.removeEventHandler (FiEventType.RECV_EXCHANGE_DIAMOND_RESPONSE, RecvDiamondkResponse);
			mControl.removeEventHandler (FiEventType.RECV_NOTIFY_EXCHANGE_DIAMOND, RecvOtherDiamondChange);
			mControl.removeEventHandler (FiEventType.RECV_TOPUP_RESPONSE, RecvTopUpResponse);
			mControl.removeEventHandler (FiEventType.RECV_TOP_UP_INFORM, RecvTopUpInform);
			mControl.removeEventHandler (FiEventType.RECV_GET_MONTHLY_PACK_RESPONSE, RecvMothlyPackResponse);
			mControl.removeEventHandler (FiEventType.RECV_GET_PAY_STATE_RESPONSE, RecvPayStateResponse);
			mControl.removeEventHandler (FiEventType.RECV_GET_FIRST_PAY_REWARD_RESPONSE, RecvPayRewardResponse);
			mControl.removeEventHandler (FiEventType.RECV_NOTIFY_PURCHASE_PROPERTY, RecvPurchasePropertyResponse);
			mControl.removeEventHandler (FiEventType.RECV_XL_EXCHANGEBARBETTE_RESPONSE, RecvBarbetteResponse);
		}

		public void RecvMothlyPackResponse (object data)
		{
			FiGetMonthlyPackResponse nResponse = (FiGetMonthlyPackResponse)data;
			if (nResponse.result == 0) {
				MyInfo nUserInfo = (MyInfo)Facade.GetFacade ().data.Get (FacadeConfig.USERINFO_MODULE_ID);
				//nUserInfo.loginInfo.monthlyPackGot = true;
				UnityEngine.GameObject Window = UnityEngine.Resources.Load ("Window/RewardWindow")as UnityEngine.GameObject;
				GameObject rewardInstance = UnityEngine.GameObject.Instantiate (Window);
				UIReward reward = rewardInstance.GetComponent<UIReward> ();
				reward.SetRewardData (nResponse.properties);
			}
		}

		/*public void AddMsgEventHandle()
		{
			EventControl mControl = EventControl.instance ();
			mControl.addEventHandler ( FiEventType.RECV_PURCHASE_PROPERTY_RESPONSE ,      RecvPurchaseResponse );

			mControl.addEventHandler ( FiEventType.RECV_TOPUP_RESPONSE , RecvTopUpResponse );
			mControl.addEventHandler ( FiEventType.RECV_TOP_UP_INFORM  , RecvTopUpInform );
		}*/

		//获取特惠礼包奖励物品
		List<FiProperty> GetPrefrenceGift ()
		{
			List<FiProperty> nGift = new List<FiProperty> ();

			/*FiProperty nProp = new FiProperty ();
			nProp.type = FiPropertyType.TORPEDO_BRONZE;
			nProp.value = 3;
			nGift.Add (nProp);*/

			FiProperty nPropGold = new FiProperty ();
			nPropGold.type = FiPropertyType.GOLD;
			nPropGold.value = 100000;
			nGift.Add (nPropGold);

			FiProperty nPropDiamond = new FiProperty ();
			nPropDiamond.type = FiPropertyType.DIAMOND;
			nPropDiamond.value = 100;
			nGift.Add (nPropDiamond);

			FiProperty nPropAim = new FiProperty ();
			nPropAim.type = FiPropertyType.FISHING_EFFECT_AIM;
			nPropAim.value = 20;
			nGift.Add (nPropAim);

			return nGift;
		}

		int GetGoldPayId (int nMoney)
		{
			switch (nMoney) {
			case 6:
				return ProductPayId.GLOD_6;
			case 30:
				return ProductPayId.GLOD_30;
			case 68:
				return ProductPayId.GLOD_68;
			case 118:
				return ProductPayId.GLOD_118;
			case 198:
				return ProductPayId.GLOD_198;
			case 348:
				return ProductPayId.GLOD_348;
			}
			return 0;
		}

		void HandleSingleInform (FiTopUpInform nInform)
		{
			MyInfo nUserInfo = (MyInfo)Facade.GetFacade ().data.Get (FacadeConfig.USERINFO_MODULE_ID);
			string nStringName = null;
			if (nInform.property.type == FiPropertyType.GOLD) {
				nStringName = "金幣";

				//更新首充奖励提示
				int nGoldPayId = GetGoldPayId (nInform.money);
				bool bFindPayId = false;
				for (int i = 0; i < nUserInfo.loginInfo.firstPayProducts.Count; i++) {
					if (nUserInfo.loginInfo.firstPayProducts [i] == nGoldPayId) {
						bFindPayId = true;
						break;
					}
				}
				if (!bFindPayId) {
					nUserInfo.loginInfo.firstPayProducts.Add (nGoldPayId);
//					if (UIStore.instance != null) {
//						UIStore.instance.DoUpdateState ();
//					}
				}

				nUserInfo.gold += nInform.property.value;
				DragonCardInfo nDragoninfo = (DragonCardInfo)Facade.GetFacade ().data.Get (FacadeConfig.UI_DRAGONCARD);
				nDragoninfo.CurrendGold = nUserInfo.gold;
				if (PrefabManager._instance != null) {
					PrefabManager._instance.GetLocalGun ().gunUI.AddValue (0, nInform.property.value, 0);
				}
			} else if (nInform.property.type == FiPropertyType.DIAMOND) {
				nStringName = "鑽石";
				nUserInfo.diamond += nInform.property.value;
				if (PrefabManager._instance != null) {
					PrefabManager._instance.GetLocalGun ().gunUI.AddValue (0, 0, nInform.property.value);
				}

			} else if (nInform.property.type == FiPropertyType.ROOM_CARD) {
				nStringName = "房卡";
				nUserInfo.roomCard += nInform.property.value;

				BackpackInfo nBackInfo = (BackpackInfo)Facade.GetFacade ().data.Get (FacadeConfig.BACKPACK_MODULE_ID);
				FiBackpackProperty nRoomCard = new FiBackpackProperty ();
				nRoomCard.canGiveAway = true;
				nRoomCard.count = (int)nInform.property.value;
				nRoomCard.type = UIBackPack_Grids.TOOL;
				nRoomCard.id = FiPropertyType.ROOM_CARD;
				nRoomCard.useable = false;
				nRoomCard.name = FiPropertyType.GetToolName (nRoomCard.id);
				nBackInfo.Add (nRoomCard);

			}


			//购买的反馈是月卡礼包或者是特惠礼包
			if (nStringName == null) {
			
				//购买的是特惠礼包，那么直接谈特惠礼包的领取道具,并且大厅显示商城按钮
				if (nInform.property.type == FiPropertyType.PACK_PREFERENCE) {



					UnityEngine.GameObject Window = UnityEngine.Resources.Load ("Window/RewardWindow")as UnityEngine.GameObject;
					GameObject rewardInstance = UnityEngine.GameObject.Instantiate (Window);
					UIReward reward = rewardInstance.GetComponent<UIReward> ();
					reward.SetRewardData (GetPrefrenceGift ());

					//-----------如果是游客状态,更改领取奖励脚本的状态,在true状态下,点击确定按钮会执行本脚本中的显示弹窗方法-----------
					if (nUserInfo.isGuestLogin) {
						UIReward.isClose = true;
					} else {
						UIReward.isClose = false;
					}

					IUiMediator nMediator = Facade.GetFacade ().ui.Get (FacadeConfig.UI_HALL_MODULE_ID);
					//nUserInfo.loginInfo.preferencePackBought = true;
					nUserInfo.cannonMultipleMax = 5000;
					if (nMediator != null) {
						((UIHallCore)nMediator).OnShowMailButton (); //显示商城按钮，去除特惠礼包按钮
					}
                   

					if (PrefabManager._instance != null) { //渔场内加道具金币钻石
						PrefabManager._instance.GetLocalGun ().gunUI.AddValue (0, 100000, 100);
						PrefabManager._instance.GetSkillUIByType (SkillType.Lock).AddRestNum (20);
						PrefabManager._instance.GetSkillUIByType (SkillType.Torpedo, 2).AddRestNum (3);
             
						//PrefabManager._instance.GetSkillUIByType(SkillType.Lock).r += num;
					}


				} else if (nInform.property.type == FiPropertyType.PACK_MONTH) {
					//购买的是月卡礼包,打开月卡礼包领取界面，用户可以领取月卡礼包
					string path = "Window/MouthCardWindow";
					GameObject WindowClone = AppControl.OpenWindow (path);
					UIMouthCard nMonthData = WindowClone.GetComponent<UIMouthCard> ();

					if (nUserInfo.loginInfo.monthlyCardDurationDays <= 0) {
						nUserInfo.loginInfo.monthlyCardDurationDays = 0;
						//nUserInfo.loginInfo.monthlyPackGot = false;
					}

					nUserInfo.loginInfo.monthlyCardDurationDays += 30;
					//nMonthData.SetRemianDays (  (int)nUserInfo.loginInfo.monthlyCardDurationDays  , nUserInfo.loginInfo.monthlyPackGot );
				}

			} else {
				


				GameObject Window = Resources.Load ("Window/WindowTips")as GameObject;
				GameObject WindowClone = GameObject.Instantiate (Window);
				UITipClickHideManager ClickTip = WindowClone.GetComponent<UITipClickHideManager> ();
				ClickTip.text.text = "充值成功\n獲得" + nInform.property.value + " " + nStringName;

				//-----------如果是游客状态,更改领取奖励脚本的状态,在true状态下,点击确定按钮会执行本脚本中的显示弹窗方法-----------
				if (nUserInfo.isGuestLogin) {
					UITipClickHideManager.isClose = true;
				} else {
					UITipClickHideManager.isClose = false;
				}

				if (Facade.GetFacade ().ui.Get (FacadeConfig.UI_STORE_MODULE_ID) != null) {
					Facade.GetFacade ().ui.Get (FacadeConfig.UI_STORE_MODULE_ID).OnRecvData (FiPropertyType.GOLD, nUserInfo.gold);
					Facade.GetFacade ().ui.Get (FacadeConfig.UI_STORE_MODULE_ID).OnRecvData (FiPropertyType.DIAMOND, nUserInfo.diamond);
					Facade.GetFacade ().ui.Get (FacadeConfig.UI_STORE_MODULE_ID).OnRecvData (FiPropertyType.ROOM_CARD, nUserInfo.roomCard);
				}
				if (Facade.GetFacade ().ui.Get (FacadeConfig.UI_NEWSTORE_SHOW) != null) {
					Facade.GetFacade ().ui.Get (FacadeConfig.UI_NEWSTORE_SHOW).OnRecvData (FiPropertyType.GOLD, nUserInfo.gold);
					Facade.GetFacade ().ui.Get (FacadeConfig.UI_NEWSTORE_SHOW).OnRecvData (FiPropertyType.DIAMOND, nUserInfo.diamond);
				}
				//更新背包的道具信息
				IUiMediator nMediator = Facade.GetFacade ().ui.Get (FacadeConfig.UI_BACKPACK_MODULE_ID);
				if (nMediator != null) {
					nMediator.OnRecvData (UIBackPack.UPDATE_ALL, nInform);
				}

			}
		}

		public void ShowTopUpWinodw ()
		{
			MyInfo nUserInfo = (MyInfo)Facade.GetFacade ().data.Get (FacadeConfig.USERINFO_MODULE_ID);
            if(Facade.GetFacade().data.Get(FacadeConfig.USERINFO_MODULE_ID)==null)
            {
                Debug.LogError("走向充值空");
            }
            FiTopUpInform nInform = new FiTopUpInform ();
			for (int i = 0; i < mTopUpList.Count; i++) {

				nInform = mTopUpList [i];
				HandleSingleInform (nInform);

				Debug.LogError ("--------gold--------" + nUserInfo.gold + " / " + nUserInfo.gold);
			}
			mTopUpList.Clear ();
		}

		void UpdateBagCannon ()
		{
			BackpackInfo nBackInfo = (BackpackInfo)Facade.GetFacade ().data.Get (FacadeConfig.BACKPACK_MODULE_ID);
			MyInfo nUserInfo = (MyInfo)Facade.GetFacade ().data.Get (FacadeConfig.USERINFO_MODULE_ID);
			int nVipLevel = nUserInfo.levelVip;
			for (int i = 0; i <= nVipLevel; i++) {
				FiBackpackProperty nCannonStyle = new FiBackpackProperty ();
				nCannonStyle.canGiveAway = false;
				nCannonStyle.count = 1;
				nCannonStyle.type = UIBackPack_Grids.CANNON;
				nCannonStyle.id = 3000 + i;  //炮台样式索引
				nCannonStyle.description = "VIP" + i + "級專屬砲台";
				if (i == 0) {
					nCannonStyle.description = "新手炮台";
				}
				nCannonStyle.useable = false;
				nCannonStyle.name = FiPropertyType.GetCannonName (nCannonStyle.id);
				nBackInfo.Add (nCannonStyle);
			}
		}

		private void RecvTopUpInform (object data)
		{
			FiTopUpInform nInform = (FiTopUpInform)data;
			MyInfo nUserInfo = (MyInfo)Facade.GetFacade ().data.Get (FacadeConfig.USERINFO_MODULE_ID);
			NobelInfo nobelInfo = (NobelInfo)Facade.GetFacade ().data.Get (FacadeConfig.UI_NOBEL_ID);

//			Debug.Log ("PurchaseMsgHandle RecvTopUpInform ---Start---");
//			Debug.Log ("PurchaseMsgHandle RecvTopUpInform nInform.money1" + nInform.money);
//			Debug.Log ("PurchaseMsgHandle RecvTopUpInform nInform.currentVip1" + nInform.currentVip);
//			Debug.Log ("PurchaseMsgHandle RecvTopUpInform nInform.property1" + nInform.property);
//			Debug.Log ("PurchaseMsgHandle RecvTopUpInform nInform.userId1" + nInform.userId);
//			Debug.Log ("PurchaseMsgHandle RecvTopUpInform nUserInfo.userID1" + nUserInfo.userID);

			//	if (nInform.result == 0) {
			//充值对象是本人
			if (nInform.userId == nUserInfo.userID) {
				mTopUpList.Add (nInform);

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
                //充值界面已经关闭了
                if (UIToPayWeb.webPayInstance == null) {
					ShowTopUpWinodw ();
				}

				Debug.LogError ("[ vip level ]" + nUserInfo.levelVip + " / " + nInform.currentVip);

				nobelInfo.IsToUps ();
				if (nUserInfo.levelVip < nInform.currentVip) {
					string path = "Window/UpgradeWindow";
					GameObject WindowClone = AppControl.OpenWindow (path);
					WindowClone.SetActive (true);
					WindowClone.GetComponent<UIUpgrade> ().SetVipInfo (nInform.currentVip);
					if (UIUserInfo.instance != null) {
						UIUserInfo.instance.SetVipLevel (nInform.currentVip);
					}
				}
				nUserInfo.SetVip (nInform.money, nInform.currentVip);
                    
				if (StoreMiddle.instans != null)
					StoreMiddle.instans.Refresh ();
				//在大厅再显示
				if (UIUserDetail.instace != null) {
					UIUserDetail.instace.FreshVip (UIHallTexturers.instans.VipFrame [nInform.currentVip]);
				} else {
					
				}
				if (nUserInfo.levelVip >= 2) {
                    
					Debug.LogError ("nUserInfo.levelVip = " + nUserInfo.levelVip);
					nUserInfo.cannonMultipleMax = 9900;
					Debug.LogError ("nUserInfo.levelVip = " + nUserInfo.levelVip);

				}
//				Debug.Log ("PurchaseMsgHandle RecvTopUpInform nInform.money2" + nInform.money);
//				Debug.Log ("PurchaseMsgHandle RecvTopUpInform nInform.currentVip2" + nInform.currentVip);
//				Debug.Log ("PurchaseMsgHandle RecvTopUpInform nInform.property2" + nInform.property);
//				Debug.Log ("PurchaseMsgHandle RecvTopUpInform nInform.userId2" + nInform.userId);
//				Debug.Log ("PurchaseMsgHandle RecvTopUpInform nUserInfo.userID2" + nUserInfo.userID);
				UpdateBagCannon ();
				if (PrefabManager._instance != null) {
					//开启自动
					if (nUserInfo.levelVip > 0) {
						Skill.isReceiveServerLock = false;
						if (Skill.Instance != null) {
							PrefabManager._instance.GetSkillUIByType (SkillType.AutoFire).SetLockIconShow (Skill.isReceiveServerLock);
						}
					}
					//开启分身
					if (nUserInfo.levelVip >= 2) {
						Skill.isReceiveServerLock = false;
						if (Skill.Instance != null) {
							PrefabManager._instance.GetSkillUIByType (SkillType.Replication).SetLockIconShow (Skill.isReceiveServerLock);
						}
					}
				}
            } else {
				Debug.LogError ("-----------收到其他玩家充值消息通知，userid：" + nInform.userId);
			}
        }

		private void RecvTopUpResponse (object data)
		{
			FiTopUpResponse nResponse = (FiTopUpResponse)data;

			if (nResponse.result == 0) {

				MyInfo myInfo = (MyInfo)Facade.GetFacade ().data.Get (FacadeConfig.USERINFO_MODULE_ID);
				if (nResponse.type == FiPropertyType.GOLD) {
					myInfo.gold += nResponse.sum;
//					DragonCardInfo nDragoninfo = (DragonCardInfo)Facade.GetFacade ().data.Get (FacadeConfig.UI_DRAGONCARD);
//					nDragoninfo.CurrendGold = myInfo.gold;

					if (Facade.GetFacade ().ui.Get (FacadeConfig.UI_STORE_MODULE_ID) != null) {
						Facade.GetFacade ().ui.Get (FacadeConfig.UI_STORE_MODULE_ID).OnRecvData (FiPropertyType.GOLD, myInfo.gold);
					}
				} else if (nResponse.type == FiPropertyType.DIAMOND) {
					myInfo.diamond += nResponse.sum;
					if (Facade.GetFacade ().ui.Get (FacadeConfig.UI_STORE_MODULE_ID) != null) {
						Facade.GetFacade ().ui.Get (FacadeConfig.UI_STORE_MODULE_ID).OnRecvData (FiPropertyType.DIAMOND, myInfo.diamond);
					}
				}
			}
		}

		private void RecvPurchaseResponse (object data)
		{
			FiPurchasePropertyResponse nData = (FiPurchasePropertyResponse)data;
			//	nData.diamondCost + nData.info.type + nData.info.value + nData.result;
			if (nData.result == 0) {
				if (PrefabManager._instance != null) {
					PrefabManager._instance.GetSkillUIByServerId (nData.info.type).BuySucess ((int)nData.diamondCost, nData.info.value);
				}
			} else {
				Tool.LogError ("道具購買失敗");
			}
		}

		public void SendGetMonthlyPackReqeust ()
		{
			DataControl.GetInstance ().PushSocketSndByte (FiEventType.SEND_GET_MONTHLY_PACK_REQUEST, null);
		}

		public void SendTopupRequest (int nType, int nRMBCount)
		{
			FiTopUpRequest nRequest = new FiTopUpRequest ();
			nRequest.RMB = nRMBCount;
			nRequest.type = nType;
			DataControl.GetInstance ().PushSocketSnd (FiEventType.SEND_TOPUP_REQUEST, nRequest);
		}

		public void SendPurchaseRequest (int nType, int nValue)
		{
			FiPurchasePropertyRequest nRequest = new FiPurchasePropertyRequest ();
			nRequest.info.type = nType;
			nRequest.info.value = nValue;
			DataControl.GetInstance ().PushSocketSndByte (FiEventType.SEND_PURCHASE_PROPERTY_REQUEST, nRequest.serialize ());
		}

		public static void CreatWarningWindow ()
		{
			BindWindowCtrl.Instense.WarningWindow ();
		}
        
	}
}

