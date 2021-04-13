using System;
using UnityEngine;
using System.Collections.Generic;

namespace AssemblyCSharp
{
	public class BackPackMsgHandle:IMsgHandle
	{
		public BackPackMsgHandle ()
		{
		}

		public void SendReloadRequest ()
		{
			DataControl.GetInstance ().PushSocketSndByte (FiEventType.SEND_CL_RELOAD_ASSET_INFO_REQUEST, null);
			MyInfo nUserInfo = (MyInfo)Facade.GetFacade ().data.Get (FacadeConfig.USERINFO_MODULE_ID);
//			Debug.Log ("BackPackMsgHandle SendReloadRequest nUserInfo.gold = " + nUserInfo.gold);
		}

		public void OnInit ()
		{
			EventControl nControl = EventControl.instance ();
			nControl.addEventHandler (FiEventType.RECV_BACKPACK_RESPONSE, RcvBackpackResponse);
			nControl.addEventHandler (FiEventType.RECV_GIVE_OTHER_PROPERTY_RESPONSE, RcvGiveOtherPropertyResponse);
			nControl.addEventHandler (FiEventType.RECV_SELL_PROPERTY_RESPONSE, RcvSellResponse);
			nControl.addEventHandler (FiEventType.RECV_OPEN_PACK_RESPONSE, RcvOpenPackResponse);
			nControl.addEventHandler (FiEventType.RECV_CL_RELOAD_ASSET_INFO_RESPONSE, RecvReloadAssetInfo);

			//使用限时道具的回复
			nControl.addEventHandler (FiEventType.RECV_USE_PROP_TIME_RESPONSE, RecvUseProTimeResponse);
			//获取所有限时道具的回复
			nControl.addEventHandler (FiEventType.RECV_GET_ALL_PROP_TIME_RESPONSE, RecvGetAllPropTimeResponse);
			//删除限时道具回复
			nControl.addEventHandler (FiEventType.RECV_DEL_PROP_TIME_RESPONSE, RecvDelProTimeResponse);
		}

		public void RecvReloadAssetInfo (object data)
		{
			Debug.LogError ("qqqqqqqqqqqqqqqqqqqqq");
			PB_ReloadAssetInfoResponse nResponse = (PB_ReloadAssetInfoResponse)data;
			//Debug.LogError ( "-----------------" + nResponse );
			MyInfo nUserInfo = (MyInfo)Facade.GetFacade ().data.Get (FacadeConfig.USERINFO_MODULE_ID);
			//			UnityEngine.Debug.Log ("BackPackMsgHandel RecvReloadAssetInfo1111Start = ");
			//			UnityEngine.Debug.Log ("BackPackMsgHandel RecvReloadAssetInfonResponse.Result = " + nResponse.Result);
			Debug.Log(" ~~~~~!!!!! RecvReloadAssetInfo !!!!!~~~~~ " + nResponse.Result);
			if (nResponse.Result == 0) {
				IEnumerator<PB_PropertyEs> nArray = nResponse.Props.GetEnumerator ();
				while (nArray.MoveNext ()) {
					//金币
					switch (nArray.Current.PropertyType) {
					case FiPropertyType.GOLD:
						{
							//	nUserInfo.gold = nArray.Current.Sum;
//							string buyGoldCoin = PurchaseControl.GetInstance ().buyGoldCoin;
//							PurchaseControl.GetInstance ().buyGoldCoin = "";

							//if (nArray.Current.Sum != nUserInfo.gold) {
							/*if (nUserInfo.bAfterCharge) {
//									long nChangeValue = nArray.Current.Sum - nUserInfo.gold;
//									if (nChangeValue > 0 && "" != buyGoldCoin) {
//										// Debug.LogError("---提示获得的金币---购买返回信息--弹框---");
//										GameObject Window = Resources.Load ("Window/WindowTips")as GameObject;
//										GameObject WindowClone = GameObject.Instantiate (Window);
//										UITipClickHideManager ClickTip = WindowClone.GetComponent<UITipClickHideManager> ();
//										// ClickTip.text.text = "获得" + (nArray.Current.Sum - nUserInfo.gold) + "金币";
//										ClickTip.text.text = "获得" + buyGoldCoin;
//									}
									nUserInfo.bAfterCharge = false;
								} else {
									buyGoldCoin = ""; //如果没有充值则清空
								}*/
							//如果在渔场，那么更新渔场的金币数量
							//if (PrefabManager._instance != null) {
//									if ("" != buyGoldCoin) {
//										buyGoldCoin = buyGoldCoin.Replace ("金币", "");
//										int goldCoin = int.Parse (buyGoldCoin);
//										UnityEngine.Debug.Log ("BackPackMsgHandel goldCoin111111 = " + goldCoin);
//									} else {
							//UnityEngine.Debug.Log ("BackPackMsgHandel (int)(nArray.Current.Sum - nUserInfo.gold)1111 = " + (int)(nArray.Current.Sum - nUserInfo.gold));

							//	PrefabManager._instance.GetLocalGun ().gunUI.AddValue (0, (int)(nArray.Current.Sum - nUserInfo.gold));
//									}
							//PrefabManager._instance.GetLocalGun ().gunUI.AddValue (0, goldCoin);

							//}
							//}
							//	UnityEngine.Debug.Log ("BackPackMsgHandel nUserInfo.gold1111 = " + nUserInfo.gold);

							//Debug.LogError ( "---------nUserInfo.gold----------" + nUserInfo.gold );
						}
						break;
					case FiPropertyType.BANK_GOLD:
						{
							
							nUserInfo.loginInfo.bankGold = nArray.Current.Sum;
						}
						break;
					case FiPropertyType.BANK_CHARM:
						{
							nUserInfo.loginInfo.charm = nArray.Current.Sum;
						}
						break;
					default:
						{
							Debug.Log(" ~~~~~!!!!! nArray.Current.PropertyType !!!!!~~~~~ " + nArray.Current.PropertyType);
							Debug.Log(" ~~~~~!!!!! nArray.Current.PropertyType !!!!!~~~~~ " + nArray.Current.Sum);
							BackpackInfo nBack = (BackpackInfo)Facade.GetFacade ().data.Get (FacadeConfig.BACKPACK_MODULE_ID);
							nBack.Replace (nArray.Current.PropertyType, nArray.Current.Sum);
						}
						break;
					}

					if (Facade.GetFacade ().ui.Get (FacadeConfig.UI_STORE_MODULE_ID) != null) {
                        Debug.Log(" ~~~~~!!!!! RecvReloadAssetInfo  Facade.GetFacade !!!!!~~~~~ " + nUserInfo.gold);
                        Debug.Log(" ~~~~~!!!!! RecvReloadAssetInfo  Facade.GetFacade !!!!!~~~~~ " + nUserInfo.diamond);
                        Debug.Log(" ~~~~~!!!!! RecvReloadAssetInfo  Facade.GetFacade !!!!!~~~~~ " + nUserInfo.roomCard);
                        Facade.GetFacade ().ui.Get (FacadeConfig.UI_STORE_MODULE_ID).OnRecvData (FiPropertyType.GOLD, nUserInfo.gold);
						Facade.GetFacade ().ui.Get (FacadeConfig.UI_STORE_MODULE_ID).OnRecvData (FiPropertyType.DIAMOND, nUserInfo.diamond);
						Facade.GetFacade ().ui.Get (FacadeConfig.UI_STORE_MODULE_ID).OnRecvData (FiPropertyType.ROOM_CARD, nUserInfo.roomCard);
					}
					//更新背包的道具信息
					IUiMediator nMediator = Facade.GetFacade ().ui.Get (FacadeConfig.UI_BACKPACK_MODULE_ID);
					if (nMediator != null) {
						Debug.Log(" ~~~~~!!!!! RecvReloadAssetInfo  UIBackPack.UPDATE_ALL !!!!!~~~~~ ");
						nMediator.OnRecvData (UIBackPack.UPDATE_ALL, null);
					}
				}

			}
		}

		public void OnDestroy ()
		{
			EventControl nControl = EventControl.instance ();
			nControl.removeEventHandler (FiEventType.RECV_BACKPACK_RESPONSE, RcvBackpackResponse);
			nControl.removeEventHandler (FiEventType.RECV_GIVE_OTHER_PROPERTY_RESPONSE, RcvGiveOtherPropertyResponse);
			nControl.removeEventHandler (FiEventType.RECV_SELL_PROPERTY_RESPONSE, RcvSellResponse);
			nControl.removeEventHandler (FiEventType.RECV_OPEN_PACK_RESPONSE, RcvOpenPackResponse);
			nControl.removeEventHandler (FiEventType.RECV_CL_RELOAD_ASSET_INFO_RESPONSE, RecvReloadAssetInfo);
		
			//使用限时道具的回复
			nControl.removeEventHandler (FiEventType.RECV_USE_PROP_TIME_RESPONSE, RecvUseProTimeResponse);
			//获取所有限时道具的回复
			nControl.removeEventHandler (FiEventType.RECV_GET_ALL_PROP_TIME_RESPONSE, RecvGetAllPropTimeResponse);
			//删除限时道具回复
			nControl.removeEventHandler (FiEventType.RECV_DEL_PROP_TIME_RESPONSE, RecvDelProTimeResponse);
		}

		public void SendOpenPackRequest (int nPropId)
		{
			FiOpenPackRequest nRequest = new FiOpenPackRequest ();
			nRequest.packId = nPropId;
			DataControl.GetInstance ().PushSocketSndByte (FiEventType.SEND_OPEN_PACK_REQUEST, nRequest.serialize ());
		}

		public void SendGiveOtherPropertyRequest (int nUserId, int nPropType, int nPropValue)
		{
			FiGiveOtherPropertyRequest nRequest = new FiGiveOtherPropertyRequest ();
			nRequest.userId = nUserId;
			nRequest.property.type = nPropType;
			nRequest.property.value = nPropValue;
			DataControl.GetInstance ().PushSocketSndByte (FiEventType.SEND_GIVE_OTHER_PROPERTY_REQUEST, nRequest.serialize ());
		}

		//		public void SendGiveOtherTprpedoRequest( int nUserId , int nPropType, int nPropValue )
		//		{
		//
		//
		//			FiGiveOtherPropertyRequest nRequest = new FiGiveOtherPropertyRequest ();
		//			nRequest.userId = nUserId;
		//			nRequest.property.type = nPropType;
		//			nRequest.property.value = nPropValue;
		//			DataControl.GetInstance ().PushSocketSndByte ( FiEventType.SEND_CL_GIVE_REQUEST , nRequest.serialize() );
		//		}

		public void SendSellRequest (FiProperty nSellData)
		{
			FiSellPropertyRequest nRequest = new FiSellPropertyRequest ();
			nRequest.mSellArray.Add (nSellData);
			DataControl.GetInstance ().PushSocketSndByte (FiEventType.SEND_SELL_PROPERTY_REQUEST, nRequest.serialize ());
		}

		public void SendBackPackRequest ()
		{
			DataControl.GetInstance ().PushSocketSnd (FiEventType.SEND_BACKPACK_REQUEST, null);
		}

		/// <summary>
		/// 发送开启限时道具
		/// </summary>
		public void SendBackpackProTimeRequest (int userID, FiUserGetAllPropTimeResponse allPro)
		{

			Debug.Log(" 发送开启限时道具 SendBackpackProTimeRequest ");
			FiUseProTimeRequest nRequest = new FiUseProTimeRequest ();

			nRequest.userID = userID;
			//	Debug.Log ("BackPackMsgHandle  SendBackpackProTimeRequest nRequest.userID= " + nRequest.userID);
			nRequest.useProp = allPro;
			if (nRequest.useProp != null) {
				nRequest.useProp.propID = allPro.propID;
				nRequest.useProp.propType = allPro.propType;
				nRequest.useProp.useTime = allPro.useTime;
				nRequest.useProp.remainTime = allPro.remainTime;
				nRequest.useProp.propTime = allPro.propTime;
//				Debug.Log ("BackPackMsgHandle  SendBackpackProTimeRequest nRequest.useProp.propID = " + nRequest.useProp.propID);
//				Debug.Log ("BackPackMsgHandle  SendBackpackProTimeRequest nRequest.useProp.propType = " + nRequest.useProp.propType);
//				Debug.Log ("BackPackMsgHandle  SendBackpackProTimeRequest nRequest.useProp.useTime = " + nRequest.useProp.useTime);
//				Debug.Log ("BackPackMsgHandle  SendBackpackProTimeRequest nRequest.useProp.remainTime = " + nRequest.useProp.remainTime);

			}
			DataControl.GetInstance ().PushSocketSnd (FiEventType.SEND_CL_USE_PROP_TIMEEX_REQUEST, nRequest);
		}


		/// <summary>
		/// 开启限时道具回复
		/// </summary>
		/// <param name="data">Data.</param>
		private void RecvUseProTimeResponse (object data)
		{
			Debug.Log(" 开启限时道具回复 RecvUseProTimeResponse ");
			//收到回复后要开始计时并且不将该道具移除,等时间到了,就将该道具移除,并且将 使用按钮 变为 可装备
			FiUsePropTimeResponse nResponse = (FiUsePropTimeResponse)data;
//			MyInfo nUserInfo = (MyInfo)Facade.GetFacade ().data.Get (FacadeConfig.USERINFO_MODULE_ID);
			BackpackInfo nBackInfo = (BackpackInfo)Facade.GetFacade ().data.Get (FacadeConfig.BACKPACK_MODULE_ID);

//			Debug.Log ("BackPackMsgHandle  RecvUseProTimeResponse resultCode = " + nResponse.resultCode);

			if (nResponse.resultCode == 0) {
//				Debug.Log ("BackPackMsgHandle  RecvUseProTimeResponse userID = " + nResponse.userID);
//				Debug.Log ("BackPackMsgHandle  RecvUseProTimeResponse useProp.propID1 = " + nResponse.useProp.propID);
//				Debug.Log ("BackPackMsgHandle  RecvUseProTimeResponse useProp.propType1 = " + nResponse.useProp.propType);
//				Debug.Log ("BackPackMsgHandle  RecvUseProTimeResponse useProp.remainTime1 = " + nResponse.useProp.remainTime);
//				Debug.Log ("BackPackMsgHandle  RecvUseProTimeResponse useProp.useTime1 = " + nResponse.useProp.useTime);
//				Debug.Log ("BackPackMsgHandle  RecvUseProTimeResponse useProp.propTime1 = " + nResponse.useProp.propTime);
//				UsePropTool.Instance.propID = nResponse.useProp.propID;
//				UsePropTool.Instance.propType = nResponse.useProp.propType;
//				UsePropTool.Instance.remainTime = nResponse.useProp.remainTime;
//				UsePropTool.Instance.useTime = nResponse.useProp.useTime;
//				UsePropTool.Instance.propTime = nResponse.useProp.propTime;
//				UsePropTool.Instance.proTimeRequest = nResponse.;
				UsePropTool.Instance.proTimeRequest.Add (nResponse.useProp);
				Debug.LogError("--------BackPackMsgHandle RecvUseProTimeResponse id--------" + nResponse.useProp.propID + " / "+ nResponse.useProp.propType);
				//只有炮座才会走主动添加背包,炮台显示道具会走领取添加背包
				if (nResponse.useProp.propID > 8010 && nResponse.useProp.propID < 8999) {
					switch (nResponse.useProp.propID) {
						case FiPropertyType.GOLD:
						case FiPropertyType.DIAMOND:
							break;
						default:
							Debug.LogError(nResponse.useProp.propID);
							Debug.LogError("--------BackPackMsgHandle RecvUseProTimeResponse type--------" + nResponse.useProp.propType + " / ");
							nBackInfo.Add(nResponse.useProp.propID, 1);
							nBackInfo.SetProperty(nResponse.useProp.propID, nResponse.useProp.propType, 1);
							Facade.GetFacade().message.fishCommom.SendChangeBarbetteStyleRequest(nResponse.useProp.propType, nResponse.useProp.propID);
							break;
					}
				} else if (nResponse.useProp.propType >3000 && nResponse.useProp.propType<3010) {
					Debug.LogError(nResponse.useProp.propID);
					Debug.LogError("--------BackPackMsgHandle RecvUseProTimeResponse type--------" + nResponse.useProp.propType + " / ");
					nBackInfo.Add(nResponse.useProp.propID, nResponse.useProp.propType);
					nBackInfo.SetProperty(nResponse.useProp.propID, nResponse.useProp.propType, 1);
					Facade.GetFacade().message.fishCommom.SendChangeBarbetteStyleRequest(nResponse.useProp.propType, nResponse.useProp.propID);
				}
				//UsePropTool.isUsePro = true; 
				UsePropTool.isFirstUsePro = true;
				//更新背包信息
				NotifyToUi (UIBackPack.UPDATE_ALL, data);
			} else {
				Debug.LogError ("resultCode 不為 0");
			}

		}

		/// <summary>
		/// 获取所有限时礼包
		/// </summary>
		/// <param name="data">Data.</param>
		private void RecvGetAllPropTimeResponse (object data)
		{
			Debug.Log(" 获取所有限时礼包 RecvGetAllPropTimeResponse ");
			FiUseProTimeArr propArr = (FiUseProTimeArr)data;
			UsePropTool.Instance.toolpropArr = propArr;
			//UsePropTool.isFirstUsePro = false;
			if (propArr != null) {
				for (int i = 0; i < propArr.allProp.Count; i++) {
					if (propArr.allProp [i].propID >= FiPropertyType.TIMELIMTPROTYPE_1 && propArr.allProp [i].propID <= FiPropertyType.TIMELIMTPROTYPE_3) {
						UsePropTool.isUsePro = propArr.allProp [i].useTime > 0 ? true : false;
					}
//					Debug.Log ("BackPackMsgHandle  RecvGetAllPropTimeResponse  allPropTime [i].propType1 " + " = " + i + " = " + UsePropTool.Instance.toolpropArr.allProp [i].propType);
//					Debug.Log ("BackPackMsgHandle  RecvGetAllPropTimeResponse  allPropTime [i].useTime " + " = " + i + " = " + UsePropTool.Instance.toolpropArr.allProp [i].useTime);
//					Debug.Log ("BackPackMsgHandle  RecvGetAllPropTimeResponse  UsePropTool isUsePro = " + UsePropTool.isUsePro);
//					Debug.Log ("BackPackMsgHandle  RecvGetAllPropTimeResponse  allPropTime [i].remainTime2222222222 " + " = " + i + " = " + UsePropTool.Instance.toolpropArr.allProp [i].remainTime);
//					Debug.Log ("BackPackMsgHandle  RecvGetAllPropTimeResponse  allPropTime [i].propID " + " = " + i + " = " + UsePropTool.Instance.toolpropArr.allProp [i].propID);
				}
			}
		}

		/// <summary>
		/// 删除限时道具回复
		/// </summary>
		/// <param name="data">Data.</param>
		private void RecvDelProTimeResponse (object data)
		{
			Debug.Log(" 删除限时道具回复 RecvDelProTimeResponse ");
			//收到回复后要将该道具移除
			FiDelPropTimeResponse nResponse = (FiDelPropTimeResponse)data;
//			Debug.Log ("BackPackMsgHandle  RecvDelProTimeResponse resultCode1 = " + nResponse.resultCode);
//			Debug.Log ("BackPackMsgHandle  RecvDelProTimeResponse userID1 = " + nResponse.userID);
//			Debug.Log ("BackPackMsgHandle  RecvDelProTimeResponse delPropID1 = " + nResponse.delPropID);
			MyInfo myInfo = (MyInfo)Facade.GetFacade ().data.Get (FacadeConfig.USERINFO_MODULE_ID);
			if (nResponse.resultCode == 0) {
				BackpackInfo nBackInfo = (BackpackInfo)Facade.GetFacade ().data.Get (FacadeConfig.BACKPACK_MODULE_ID);
				UsePropTool.isUsePro = false;
				UsePropTool.isDelUsePro = true;
				if (UsePropTool.Instance != null) {
//					Debug.Log ("发送切换炮的为初始炮 第一次");
					FishingCommonMsgHandle temp = new FishingCommonMsgHandle ();
					if (nResponse.delPropID > 8010 && nResponse.delPropID < 8999)
						temp.SendChangeBarbetteStyleRequest (0, 0, nResponse.delPropID);
					else
						temp.SendChangeCannonStyleRequest (3000, myInfo.userID);
				}
				FiUserGetAllPropTimeResponse prop = new FiUserGetAllPropTimeResponse ();

				for (int i = 0; i < UsePropTool.Instance.proTimeRequest.Count; i++) {
					if (UsePropTool.Instance.proTimeRequest [i].propID == nResponse.delPropID) {
						prop = UsePropTool.Instance.proTimeRequest [i];
						UsePropTool.Instance.proTimeRequest.Remove (prop);
						break;
					}
				}

				for (int i = 0; i < UsePropTool.Instance.toolpropArr.allProp.Count; i++) {
					if (UsePropTool.Instance.toolpropArr.allProp [i].propID == nResponse.delPropID) {
						prop = UsePropTool.Instance.toolpropArr.allProp [i];
						UsePropTool.Instance.toolpropArr.allProp.Remove (prop);
						break;
					}
				}
//				Debug.LogError ("222222222RecvDelProTimeResponse UsePropTool.Instance.toolpropArr.allProp.Count = " + UsePropTool.Instance.toolpropArr.allProp.Count);
//				Debug.Log ("BackPackMsgHandle  RecvDelProTimeResponse resultCode2 = " + nResponse.resultCode);
//				Debug.Log ("BackPackMsgHandle  RecvDelProTimeResponse userID2 = " + nResponse.userID);
//				Debug.Log ("BackPackMsgHandle  RecvDelProTimeResponse delPropID2 = " + nResponse.delPropID);
				nBackInfo.Delete (nResponse.delPropID, 1);
				NotifyToUi (UIBackPack.UPDATE_ALL, data);
			} else {
				Debug.LogError("刪除限時道具失敗");
			}

		}
		//打开礼包后的结果反馈
		void RcvOpenPackResponse (object data)
		{
			FiOpenPackResponse nResponse = (FiOpenPackResponse)data;
			if (nResponse.result == 0) {
//				MyInfo          myInfo    = (MyInfo)Facade.GetFacade ().data.Get ( FacadeConfig.USERINFO_MODULE_ID );
				BackpackInfo nBackInfo = (BackpackInfo)Facade.GetFacade ().data.Get (FacadeConfig.BACKPACK_MODULE_ID);
				nBackInfo.Delete (nResponse.packId, 1);
				UnityEngine.GameObject Window = UnityEngine.Resources.Load ("Window/RewardWindow")as UnityEngine.GameObject;
				GameObject rewardInstance = UnityEngine.GameObject.Instantiate (Window);
				UIReward reward = rewardInstance.GetComponent<UIReward> ();
				reward.SetRewardData (nResponse.properties);
			} else {
				GameObject Window1 = UnityEngine.Resources.Load ("Window/WindowTipsThree")as UnityEngine.GameObject;
				GameObject WindowClone1 = UnityEngine.GameObject.Instantiate (Window1);
				UITipAutoNoMask ClickTips1 = WindowClone1.GetComponent<UITipAutoNoMask> ();
				ClickTips1.tipText.text = "開啟禮包失敗!";
			}
		}

		void RcvSellResponse (object data)
		{
			FiSellPropertyResponse nResponse = (FiSellPropertyResponse)data;
			if (nResponse.result == 0) {
				GameObject Window = Resources.Load ("Window/WindowTips")as GameObject;
				GameObject WindowClone = GameObject.Instantiate (Window);
				UITipClickHideManager ClickTip = WindowClone.GetComponent<UITipClickHideManager> ();
				if (nResponse.gold > 10000) {
					ClickTip.text.text = "出售成功\n獲得" + (nResponse.gold / 10000) + "萬金幣";
				} else {
					ClickTip.text.text = "出售成功\n獲得" + nResponse.gold + "金幣";
				}
				MyInfo nUserInfo = (MyInfo)Facade.GetFacade ().data.Get (FacadeConfig.USERINFO_MODULE_ID);
				nUserInfo.gold += nResponse.gold;
				Debug.LogError ("--------gold--------" + nUserInfo.gold + " / " + nUserInfo.gold);
				if (Facade.GetFacade ().ui.Get (FacadeConfig.UI_STORE_MODULE_ID) != null) {
					Facade.GetFacade ().ui.Get (FacadeConfig.UI_STORE_MODULE_ID).OnRecvData (FiPropertyType.GOLD, nUserInfo.gold);
				}

				BackpackInfo backpackInfo = (BackpackInfo)Facade.GetFacade ().data.Get (FacadeConfig.BACKPACK_MODULE_ID);
				backpackInfo.Delete (nResponse.properties [0].type, nResponse.properties [0].value);
				NotifyToUi (UIBackPack.UPDATE_ALL, data);
			} else {
				GameObject Window1 = UnityEngine.Resources.Load ("Window/WindowTipsThree")as UnityEngine.GameObject;
				GameObject WindowClone1 = UnityEngine.GameObject.Instantiate (Window1);
				UITipAutoNoMask ClickTips1 = WindowClone1.GetComponent<UITipAutoNoMask> ();
				ClickTips1.tipText.text = "道具出售失敗!";
			}
//			Debug.LogError ( "---------------------------" );
		}

		//开启礼包的操作
		private void RcvBackpackResponse (object data)
		{
			FiBackpackResponse info = (FiBackpackResponse)data;
			BackpackInfo nBackInfo = (BackpackInfo)Facade.GetFacade ().data.Get (FacadeConfig.BACKPACK_MODULE_ID);

			Debug.Log("  接收背包訊息  "+ info.properties.Count);
			Debug.Log("  接收背包訊息  " + info.ToString());
			if (0 == info.result) {
				MyInfo nUserInfo = (MyInfo)Facade.GetFacade().data.Get(FacadeConfig.USERINFO_MODULE_ID);
				if (null != info.properties && info.properties.Count > 0) {
					nBackInfo.SetArrayData (info.properties);
					//foreach (FiBackpackProperty one in info.properties) 
					//{
					//	nBackInfo.Add ( one );
					//}
//					for (int i = 0; i < info.properties.Count; i++) {
//						Debug.Log ("RcvBackpackResponse info.properties id= " + i + " = id = " + info.properties [i].id);
//						Debug.Log ("RcvBackpackResponse info.properties name= " + i + " = name = " + info.properties [i].name);
//						Debug.Log ("RcvBackpackResponse info.properties type= " + i + " = type = " + info.properties [i].type);
//
//					}

				}

				//2020 07 21 砲台改為下發限時道具
				for (int bag = 0; bag < info.properties.Count; bag++)
				{
					FiBackpackProperty nCannonStyle = (FiBackpackProperty)info.properties[bag];
					if (nCannonStyle.id >= 6000 && nCannonStyle.id == nCannonStyle.type && nCannonStyle.id <= 60010)
					{
						Debug.Log("  接收背包砲台訊息  " + nCannonStyle.id);
						FiBackpackProperty nCannonGun = new FiBackpackProperty();
						nCannonGun.canGiveAway = false;
						nCannonGun.count = 1;
						nCannonGun.type = UIBackPack_Grids.CANNON;
						nCannonGun.id = 3000 + (nCannonStyle.id % 6000);  //炮台样式索引
						nCannonGun.description = FiPropertyType.GetCannonName(nCannonGun.id);
						nCannonGun.useable = false;
						nCannonGun.name = FiPropertyType.GetCannonName(nCannonGun.id);
						nBackInfo.Add(nCannonGun);
					}

				}
				FiBackpackProperty nCannonGunDefault = new FiBackpackProperty();
				nCannonGunDefault.canGiveAway = false;
				nCannonGunDefault.count = 1;
				nCannonGunDefault.type = UIBackPack_Grids.CANNON;
				nCannonGunDefault.id = 3000;  //炮台样式索引
				nCannonGunDefault.description = "新手炮台";
				nCannonGunDefault.useable = false;
				nCannonGunDefault.name = FiPropertyType.GetCannonName(nCannonGunDefault.id);
				nBackInfo.Add(nCannonGunDefault);

				//int nVipLevel = nUserInfo.levelVip;

				//Joey
				//if (nUserInfo.roomCard > 0)
				//{
				//    FiBackpackProperty nRoomCard = new FiBackpackProperty();
				//    nRoomCard.canGiveAway = true;
				//    nRoomCard.count = (int)nUserInfo.roomCard;
				//    nRoomCard.type = UIBackPack_Grids.TOOL;
				//    nRoomCard.id = FiPropertyType.ROOM_CARD;
				//    nRoomCard.useable = false;
				//    nRoomCard.name = FiPropertyType.GetToolName(nRoomCard.id);
				//    nBackInfo.Add(nRoomCard);
				//}

				/*FiBackpackProperty nGiftVip1 = new FiBackpackProperty ();
				nGiftVip1.id = FiPropertyType.GIFT_VIP1; // FiPropertyType.GIFT_VIP1 + (nVipLevel - 1);
				nGiftVip1.count = 1;
				nGiftVip1.canGiveAway = false;
				nGiftVip1.useable = true;
				nGiftVip1.name = "鱼雷礼包";
				nGiftVip1.type = GridManager.TOOL;
				nGiftVip1.description = FiPropertyType.GetDescribtion (nGiftVip1.id);
				nBackInfo.Add ( nGiftVip1 );

				FiBackpackProperty nGiftVip = new FiBackpackProperty ();
				nGiftVip.id = FiPropertyType.GIFT_TORPEDO; // FiPropertyType.GIFT_VIP1 + (nVipLevel - 1);
				nGiftVip.count = 1;
				nGiftVip.canGiveAway = false;
				nGiftVip.useable = true;
				nGiftVip.name = "鱼雷礼包";
				nGiftVip.type = GridManager.TOOL;
				nGiftVip.description = FiPropertyType.GetDescribtion (nGiftVip.id);
				nBackInfo.Add ( nGiftVip );*/

				/*if (nVipLevel > 0) {
					FiBackpackProperty nGiftVip = new FiBackpackProperty ();
					nGiftVip.id = FiPropertyType.GIFT_TORPEDO; // FiPropertyType.GIFT_VIP1 + (nVipLevel - 1);
					nGiftVip.count = 1;
					nGiftVip.canGiveAway = false;
					nGiftVip.useable = true;
					nGiftVip.name = "鱼雷礼包";
					nGiftVip.type = GridManager.TOOL;
					nGiftVip.description = FiPropertyType.GetDescribtion (nGiftVip.id);
					nBackInfo.Add ( nGiftVip );
				}*/

				//Debug.LogError ( "-------user vip level--------" + nVipLevel );
				//FiBackpackProperty nCannonStyle = (FiBackpackProperty)info.properties[bag];
				//每個vip等级都对应一个炮台样式
				//for (int i = 0; i <= nVipLevel; i++)
				//{
				//    FiBackpackProperty nCannonStyle = new FiBackpackProperty();
				//    nCannonStyle.canGiveAway = false;
				//    nCannonStyle.count = 1;
				//    nCannonStyle.type = UIBackPack_Grids.CANNON;
				//    nCannonStyle.id = 3000 + i;  //炮台样式索引
				//    nCannonStyle.description = "VIP" + i + "級專屬砲台";
				//    if (i == 0)
				//    {
				//        nCannonStyle.description = "新手炮台";
				//    }
				//    nCannonStyle.useable = false;
				//    nCannonStyle.name = FiPropertyType.GetCannonName(nCannonStyle.id);
				//    nBackInfo.Add(nCannonStyle);
				//}
			}
			NotifyToUi (UIBackPack.UPDATE_ALL, data);
		}

		private void RcvGiveOtherPropertyResponse (object data)
		{
			GameObject WindowClone;
			FiGiveOtherPropertyResponse nResponse = (FiGiveOtherPropertyResponse)data;
			Debug.LogError ( "--------------------RcvGiveOtherPropertyResponse----------------------" );
			if (nResponse.result == 0) {
				//弹出赠送道具成功界面
				UnityEngine.Debug.Log("贈送道具成功");
				GameObject Window = UnityEngine.Resources.Load ("Window/WindowTipsThree")as UnityEngine.GameObject;
				WindowClone = UnityEngine.GameObject.Instantiate (Window);
				UITipAutoNoMask ClickTips1 = WindowClone.GetComponent<UITipAutoNoMask> ();
				if (nResponse.property.type == 1000) {
					ClickTips1.tipText.text = "成功贈送金幣!";
				} else {
					ClickTips1.tipText.text = "成功贈送道具!";
				}
				BackpackInfo backpackInfo = (BackpackInfo)Facade.GetFacade ().data.Get (FacadeConfig.BACKPACK_MODULE_ID);
				backpackInfo.Delete (nResponse.property.type, nResponse.property.value);
				NotifyToUi (UIBackPack.UPDATE_ALL, data);
			} else {
				UnityEngine.Debug.Log("贈送道具失敗");
				GameObject Window = Resources.Load ("Window/WindowTips")as GameObject;
				WindowClone = GameObject.Instantiate (Window);
				UITipClickHideManager ClickTip = WindowClone.GetComponent<UITipClickHideManager> ();
				ClickTip.text.text = "贈送道具失敗!";
			}
		}

		private void NotifyToUi (int nType, object data)
		{
			//Debug.LogError ( "--------NotifyToUi-------" );
			IUiMediator nMediator = Facade.GetFacade ().ui.Get (FacadeConfig.UI_BACKPACK_MODULE_ID);
			if (nMediator != null)
				nMediator.OnRecvData (nType, data);
		}

	}
}

