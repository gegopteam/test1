
using System;
using UnityEngine;
using UnityEngine.UI;
using AssemblyCSharp;
using System.Collections.Generic;

public class UIBackPack_Brief : MonoBehaviour
{
	//赠送和售卖，鱼雷属性
	public GameObject GiveSell;
	//装备，炮台属性
	public GameObject Equipment;
	//赠送使用，道具(赠送)和礼包（使用）
	public GameObject GiveUse;

	public Image EquipmentImage;


	public Image IconTarget;

	public Text TxtName;

	public Text TxtCount;

	public Text TxtDetailInfo;

	public static UIBackPack_Brief instance;

	public static bool isDischarge = false;
	int nCurrentUnitId = -1;
	int nSeletType = -1;

	void Awake ()
	{
		instance = this;
	}


	public void Refresh ()
	{
		UpdateInfo (nCurrentUnitId, null, nSeletType);
	}

	public void UpdateInfo (int nUnitId, Sprite nImage, int bagType)
	{
		nCurrentUnitId = nUnitId;
		nSeletType = bagType;
		if (nImage != null)
			IconTarget.sprite = nImage;
		if (nUnitId >= FiPropertyType.TIMELIMTPROTYPE_1 && nUnitId <= FiPropertyType.TIMELIMTPROTYPE_3) {
			//UnitId根据服务器下发的数据的名称
			TxtName.text = FiPropertyType.GetTimeSpriteName (nUnitId);
			//UnitId根据服务器下发的数据的描述
			TxtDetailInfo.text = FiPropertyType.GetTimeSpriteDescripe (nUnitId);
		} else {
			TxtName.text = FiPropertyType.GetToolName (nUnitId);
			TxtDetailInfo.text = FiPropertyType.GetDescribtion (nUnitId);
		}
		BackpackInfo nInfo = (BackpackInfo)Facade.GetFacade ().data.Get (FacadeConfig.BACKPACK_MODULE_ID);
		TxtCount.text = nInfo.Get (nUnitId).count.ToString ();

		MyInfo nUserInfo = (MyInfo)Facade.GetFacade ().data.Get (FacadeConfig.USERINFO_MODULE_ID);
		if (nUnitId == nUserInfo.cannonStyle) {
			EquipmentImage.gameObject.SetActive (true);
			transform.Find ("Equipment").Find ("EquipButton").gameObject.SetActive (false);
			transform.Find ("Equipment").Find ("IsEquipted").gameObject.SetActive (true);
			transform.Find ("Equipment").Find ("DischargeButton").gameObject.SetActive (false);
		} else {
			EquipmentImage.gameObject.SetActive (false);
			transform.Find ("Equipment").Find ("EquipButton").gameObject.SetActive (true);
			transform.Find ("Equipment").Find ("IsEquipted").gameObject.SetActive (false);
			transform.Find ("Equipment").Find ("DischargeButton").gameObject.SetActive (false);
		}

		Equipment.SetActive (false);
		GiveSell.SetActive (false);
		GiveUse.SetActive (false);

//		Debug.LogError ("---------------------------" + nUnitId);

		//炮台
		if (nUnitId >= FiPropertyType.CANNON_VIP0 && nUnitId <= FiPropertyType.CANNON_VIP9) {
			Equipment.SetActive (true);
			//鱼雷
		} else if (nUnitId >= FiPropertyType.TORPEDO_MINI && nUnitId <= FiPropertyType.TORPEDO_NUCLEAR) {
			GiveSell.SetActive (true);
			//礼包类型的
		} else if (nUnitId == FiPropertyType.GIFT_TORPEDO || (nUnitId >= FiPropertyType.GIFT_VIP1 && nUnitId <= FiPropertyType.GIFT_VIP9)) {
			GiveUse.SetActive (true);
			GiveUse.transform.Find ("UseButton").gameObject.SetActive (true);
			GiveUse.transform.Find ("GiveButton").gameObject.SetActive (false);
		} else if (nUnitId >= FiPropertyType.FISHING_EFFECT_FREEZE && nUnitId <= FiPropertyType.FISHING_EFFECT_SUMMON) {
			GiveUse.SetActive (true);
			GiveUse.transform.Find ("GiveButton").gameObject.SetActive (true);
			GiveUse.transform.Find ("UseButton").gameObject.SetActive (false);
		}
		//限时道具
		else if (nUnitId >= FiPropertyType.TIMELIMTPROTYPE_1 && nUnitId <= FiPropertyType.TIMELIMTPROTYPE_3) {
//			GiveUse.SetActive (true);
//			GiveUse.transform.Find ("UseButton").gameObject.SetActive (true);
//			GiveUse.transform.Find ("GiveButton").gameObject.SetActive (false);
			//	Debug.Log ("Equipment UsePropTool.isFirstUsePro = " + UsePropTool.isFirstUsePro);
//			if (UsePropTool.isFirstUsePro == true && !UsePropTool.isUsePro) {
//				
			//			} 
			//登录下发
			if (UsePropTool.Instance.toolpropArr.allProp.Count > 0) {
				Equipment.SetActive (true);
//				Debug.Log ("bagType = " + bagType);
				ActivityEquipt (bagType);
			}
			if (UsePropTool.Instance.proTimeRequest.Count > 0) {
				for (int i = 0; i < UsePropTool.Instance.proTimeRequest.Count; i++) {
					if (nUnitId == UsePropTool.Instance.proTimeRequest [i].propID) {
						Equipment.SetActive (true);
//						Debug.Log ("UsePropTool.Instance.proTimeRequest [i].propType = " + UsePropTool.Instance.proTimeRequest [i].propType);
						ActivityEquipt (UsePropTool.Instance.proTimeRequest [i].propType);
					}
				}
			}

		}
	}

	public void OnEquipment ()
	{
//		Debug.LogError ( "OnEquipment" );
		MyInfo nInfo = (MyInfo)Facade.GetFacade ().data.Get (FacadeConfig.USERINFO_MODULE_ID);
		Debug.Log("OnEquipment : " + nInfo.cannonStyle + " : " + nCurrentUnitId + " : "+ nSeletType);
		if (nInfo.cannonStyle == nCurrentUnitId) {
//			Debug.LogError ( "-----------is cannon equipmented--------------" + nCurrentUnitId );
			return;
		}
		bool isValid = nCurrentUnitId >= 3000 && nCurrentUnitId < 3010;
        bool isValid3 = nSeletType >= 3000 && nSeletType < 3010;
		bool isValid2 = nCurrentUnitId >= FiPropertyType.TIMELIMTPROTYPE_1 && nCurrentUnitId < FiPropertyType.TIMELIMTPROTYPE_3;



		//	Debug.LogError ("-------------do EquipmentCannon-------------" + isValid2);
		if (isValid) {
			Debug.Log("是否发送 isValid");
			Facade.GetFacade ().message.fishCommom.SendChangeCannonStyleRequest (nCurrentUnitId+3000, nInfo.userID);
		}

        if (isValid3)
        {
            Debug.Log("是否发送 isValid3");
            Facade.GetFacade().message.fishCommom.SendChangeCannonStyleRequest(nSeletType + 3000, nInfo.userID);
        }

        if (isValid2 && !isValid3) {
			Debug.Log("是否发送 isValid2");
			if (UsePropTool.Instance.proTimeRequest.Count > 0) {
				for (int i = 0; i < UsePropTool.Instance.proTimeRequest.Count; i++) {
//					Debug.LogError ("是否发送装备炮座11");
//					Debug.LogError ("UsePropTool.Instance.proTimeRequest [i].type = " + UsePropTool.Instance.proTimeRequest [i].propType);
//					Debug.Log ("nCurrentUnitId = " + nCurrentUnitId);
//					Debug.Log ("UsePropTool.Instance.proTimeRequest [i].propID = " + UsePropTool.Instance.proTimeRequest [i].propID);
					if (nCurrentUnitId == UsePropTool.Instance.proTimeRequest [i].propID) {
						if (nCurrentUnitId > 8010 && nCurrentUnitId < 8999) {
//							Debug.LogError ("是否发送装备炮座3333");
							Facade.GetFacade ().message.fishCommom.SendChangeBarbetteStyleRequest (UsePropTool.Instance.proTimeRequest [i].propType, UsePropTool.Instance.proTimeRequest [i].propID);
						} else {
							Facade.GetFacade ().message.fishCommom.SendChangeCannonStyleRequest (UsePropTool.Instance.proTimeRequest [i].propType, nInfo.userID);
						}
					}
				}
			}
			//登录下发
			if (UsePropTool.Instance.toolpropArr.allProp.Count > 0) {
				if (nCurrentUnitId > 8010 && nCurrentUnitId < 8999) {
//					Debug.LogError ("是否发送装备炮座2222");
//					Debug.LogError ("nSeletType = " + nSeletType);
//					Debug.Log ("nCurrentUnitId = " + nCurrentUnitId);
					Facade.GetFacade ().message.fishCommom.SendChangeBarbetteStyleRequest (nSeletType, nCurrentUnitId);
				} else {
					Facade.GetFacade ().message.fishCommom.SendChangeCannonStyleRequest (nSeletType, nInfo.userID);
				}
			}
		}
	}

	public void OnSell ()
	{
//		Debug.LogError ( "OnSell" );
		GameObject nSellObject = Resources.Load ("MainHall/BackPack/BagSellWindow") as GameObject;
		if (nSellObject != null) {
			GameObject nSellWin = Instantiate (nSellObject) as GameObject;
			UISellWindow nSellData = nSellWin.GetComponentInChildren<UISellWindow> ();
			nSellData.DoInit (nCurrentUnitId);
			nSellData.SetIcon (IconTarget.sprite);
		}
	}

	public void OnDischarge ()
	{
//		Debug.LogError ("是否发送卸下备炮座11");

		if (UsePropTool.Instance.proTimeRequest.Count > 0) {
			for (int i = 0; i < UsePropTool.Instance.proTimeRequest.Count; i++) {
//				Debug.LogError ("是否发送卸下备炮座11");
//				Debug.Log ("nCurrentUnitId = " + nCurrentUnitId);
				if (nCurrentUnitId == UsePropTool.Instance.proTimeRequest [i].propID) {
					if (nCurrentUnitId > 8010 && nCurrentUnitId < 8999) {
//						Debug.LogError ("是否发送卸下备炮座333");
						Facade.GetFacade ().message.fishCommom.SendChangeBarbetteStyleRequest (0, 0, UsePropTool.Instance.proTimeRequest [i].propID);
					} 
				}
			}
		}
		//登录下发
		if (UsePropTool.Instance.toolpropArr.allProp.Count > 0) {
			if (nCurrentUnitId > 8010 && nCurrentUnitId < 8999) {
//				Debug.LogError ("是否发送卸下备炮座333");
//				Debug.LogError ("nSeletType = " + nSeletType);
//				Debug.Log ("nCurrentUnitId = " + nCurrentUnitId);
				Facade.GetFacade ().message.fishCommom.SendChangeBarbetteStyleRequest (0, 0, nCurrentUnitId);

			}
		}
	}

	public void OnGive ()
	{
//		Debug.LogError ( "OnGive" );
//		Debug.Log ("赠送道具");
		//点击赠送按钮，首先判断该用户有没有达到相应的等级,则不能赠送
		MyInfo myInfo = (MyInfo)Facade.GetFacade ().data.Get (FacadeConfig.USERINFO_MODULE_ID);
		MyInfo Info = DataControl.GetInstance ().GetMyInfo ();
		try {
			if (Info.isGuestLogin == true)
			{
				GameObject Window = Resources.Load("Window/WindowTips") as GameObject;
				GameObject nWindowClone = Instantiate(Window);
				UITipClickHideManager ClickTip = nWindowClone.GetComponent<UITipClickHideManager>();
				ClickTip.text.text = "遊客模式無法贈送!";
				return;
			}
		}
		catch {
			Debug.Log("OnGive if 1");
		}

		Debug.Log("OnGive nCurrentUnitId = "+ nCurrentUnitId);

		try
		{
			if (myInfo.levelVip < 1)
			{
				GameObject Window = Resources.Load("Window/WindowTips") as GameObject;
				GameObject nWindowClone = Instantiate(Window);
				UITipClickHideManager ClickTip = nWindowClone.GetComponent<UITipClickHideManager>();
				ClickTip.text.text = "升級到VIP1，可以贈送道具!";
				//弹出等级不足的串口提示窗口
				return;
			}
			else
			{
				//弹出赠送窗口
				//发送获取好友的请求,获取好友列表信息
				BackpackInfo nBackInfo = (BackpackInfo)Facade.GetFacade().data.Get(FacadeConfig.BACKPACK_MODULE_ID);
				if (nBackInfo != null)
				{//道具数量为0时，无法赠送
					FiBackpackProperty nProp = nBackInfo.Get(nCurrentUnitId);
					Debug.Log("OnGive nProp.count = " + nProp.count);
					if (nProp == null || nProp.count == 0)
						return;
				}

				GameObject nWindow = Resources.Load("MainHall/BackPack/BagGiveWindow") as GameObject;
				GameObject nWindowClone = Instantiate(nWindow);

				UIGive nGive = nWindowClone.GetComponent<UIGive>();
				if (nGive != null)
				{
					Debug.Log("OnGive nGive != null");
					nGive.SetToolInfo(nCurrentUnitId, IconTarget.sprite);
				}

				UIGive_Friends nFriendPanel = nWindowClone.GetComponentInChildren<UIGive_Friends>();
				int nSelectUserId = UIBackPack.Instance.DefaultUserId;
				if (nSelectUserId != 0)
				{
					nFriendPanel.SetSelectedUserId(nSelectUserId);
				}
			}
		}
		catch {
			Debug.Log("OnGive if 2");
		}
		
	}

	public void OnUse ()
	{
		MyInfo myInfo = (MyInfo)Facade.GetFacade ().data.Get (FacadeConfig.USERINFO_MODULE_ID);
//		Debug.LogError ( "OnUse" );
		FiUserGetAllPropTimeResponse allPro = new FiUserGetAllPropTimeResponse ();
				 
		//Debug.Log ("nCurrentUnitId1 = " + nCurrentUnitId);
		#region 限时道具请求 暂时废弃掉
		//		if (nCurrentUnitId >= FiPropertyType.TIMELIMTPROTYPE_1 && nCurrentUnitId <= FiPropertyType.TIMELIMTPROTYPE_3) {
		//			Debug.Log ("nCurrentUnitId1 = " + nCurrentUnitId);
		//			allPro.propID = nCurrentUnitId;
		//			allPro.propType = nSeletType;
		//			Debug.Log ("我所选中发的Type = " + allPro.propType);
		//
		//			allPro.remainTime = 0;
		//			allPro.useTime = 0;
		//			//发送使用炮台道具
		//			Facade.GetFacade ().message.backpack.SendBackpackProTimeRequest (myInfo.userID, allPro);
		//		} else {
		//			}
		#endregion

		Facade.GetFacade ().message.backpack.SendOpenPackRequest (nCurrentUnitId);

	}

	/// <summary>
	/// 激活 装备按钮
	/// </summary>
	void ActivityEquipt (int propType)
	{
		MyInfo nUserInfo = (MyInfo)Facade.GetFacade ().data.Get (FacadeConfig.USERINFO_MODULE_ID);
		//EquipmentImage.gameObject.SetActive (true);
		//		Debug.Log ("ActivityEquipt nUserInfo.cannonStyle = " + nUserInfo.cannonStyle);
//		Debug.Log ("ActivityEquipt propType = " + propType);
//		Debug.Log ("ActivityEquipt nUserInfo.cannonStyle = " + nUserInfo.cannonBabetteStyle);
		GameObject equipButton = transform.Find ("Equipment").Find ("EquipButton").gameObject;
		GameObject dischargeButton = transform.Find ("Equipment").Find ("DischargeButton").gameObject;
		GameObject isEquiptedButton = transform.Find ("Equipment").Find ("IsEquipted").gameObject;
		if (isDischarge) {
			if (nCurrentUnitId > 8010 && nCurrentUnitId < 8999) {
//				Debug.LogError ("是否发送卸下备炮座11");
				EquipmentImage.gameObject.SetActive (false);
				equipButton.SetActive (true);
				dischargeButton.SetActive (false);
				isEquiptedButton.SetActive (false);
			} else {
//				Debug.LogError ("是否发送卸下备炮座22");
				EquipmentImage.gameObject.SetActive (true);
				equipButton.SetActive (true);
				dischargeButton.SetActive (false);
				isEquiptedButton.SetActive (false);
			}
		} else {
			if (propType == nUserInfo.cannonStyle) {
				EquipmentImage.gameObject.SetActive (true);
				equipButton.SetActive (false);
				dischargeButton.SetActive (false);
				isEquiptedButton.SetActive (true);
			} else if (nCurrentUnitId > 8010 && nCurrentUnitId < 8999) {
				if ((propType % 10) == nUserInfo.cannonBabetteStyle) {
//				Debug.LogError ("是否走到这里 炮座");
					EquipmentImage.gameObject.SetActive (true);
					equipButton.SetActive (false);
					dischargeButton.SetActive (true);
					isEquiptedButton.SetActive (false);
				}
			} else {
//			Debug.LogError ("是否走到这里了?");
				EquipmentImage.gameObject.SetActive (false);
				equipButton.SetActive (true);
				dischargeButton.SetActive (false);
				isEquiptedButton.SetActive (false);
			}
		}
		Equipment.SetActive (true);
		//		GiveUse.SetActive (false);
		//		GiveUse.transform.Find ("GiveButton").gameObject.SetActive (false);
		//		GiveUse.transform.Find ("UseButton").gameObject.SetActive (false);
	}

	void OnDestroy ()
	{
		instance = null;
	}

}

