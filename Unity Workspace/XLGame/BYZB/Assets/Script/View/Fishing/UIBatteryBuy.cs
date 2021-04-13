using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System;
using UnityEngine.UI;
using AssemblyCSharp;

public class UIBatteryBuy : MonoBehaviour
{

	public Image unlockBatterySprite;
	public Button btnBuy;
	public Button btnExit;
	public Text txtCost;

	public Sprite[] GunList;
	//public int[] GunID = { 6001, 6002, 6003, 6004, 6005, 6006, 6007, 6008, 6009, 6010};
	public static int[] GunID = { 6006, 6007, 6000, 6001, 6002, 6003, 6004, 6005, 6008, 6009 };
	public static int[] GoldCost = {800000, 800000, 0, 320000, 320000, 320000, 320000, 320000, 320000, 320000 };

	MyInfo myInfo;
	GameObject Window1;
	public static UIBatteryBuy _Instance;

	void Awake()
	{
		if (null == _Instance)
			_Instance = this;
		Window1 = UnityEngine.Resources.Load("Window/WindowTipsThree") as UnityEngine.GameObject;
		myInfo = (MyInfo)Facade.GetFacade().data.Get(FacadeConfig.USERINFO_MODULE_ID);
	}

	// Use this for initialization
	void Start () {
		unlockBatterySprite.sprite = GunList[myInfo.ChooseGunTemp];
		txtCost.text = GoldCost[myInfo.ChooseGunTemp].ToString();
		//OnInit();
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	public void ToBuyGun()
    {
		if (myInfo.gold >= GoldCost[myInfo.ChooseGunTemp])
			Facade.GetFacade().message.toolPruchase.SendExchangeBarbetteRequest(GunID[myInfo.ChooseGunTemp]);
		else {
			GameObject WindowClone1 = Instantiate(Window1);
			UITipAutoNoMask ClickTips1 = WindowClone1.GetComponent<UITipAutoNoMask>();
			ClickTips1.tipText.text = "金幣餘額不足";
		}
	}

	//public void OnInit()
	//{
	//	Debug.Log(" UIBatteryBuy OnInit ");
	//	EventControl nControl = EventControl.instance();
	//    nControl.addEventHandler(FiEventType.RECV_XL_EXCHANGEBARBETTE_RESPONSE, RecvBackUnlockBattery);
	//}

	//private void RecvBackUnlockBattery(object data)
 //   {
	//	FiExchangeBarbette nResult = (FiExchangeBarbette)data;
	//	Debug.LogError("nResult.result = " + nResult.result);
	//	Debug.LogError("nResult.goldCost = " + nResult.goldCost);
	//	Debug.LogError("nResult.buyType = " + nResult.buyType);
	//	//小于0就兑换失败
	//	if (nResult.result == 0)
	//	{

	//		MyInfo nUserInfo = (MyInfo)Facade.GetFacade().data.Get(FacadeConfig.USERINFO_MODULE_ID);

	//		nUserInfo.gold -= nResult.goldCost;

	//		//这里弹出新的奖励面板
	//		string path = "Window/BarbetteBuyCanvas";
	//		GameObject obj = AppControl.OpenWindow(path);
	//		obj.SetActive(true);
 //           if (UIBarbette.Instance != null)
 //           {
 //               UIBarbette.Instance.SetBarbetteImage(nResult.buyType);
 //           }

 //           if (PrefabManager._instance != null)
	//		{
	//			PrefabManager._instance.GetLocalGun().gunUI.AddValue(0, -(int)nResult.goldCost, 0);
	//		}
	//		if (Facade.GetFacade().ui.Get(FacadeConfig.UI_STORE_MODULE_ID) != null)
	//		{
	//			Facade.GetFacade().ui.Get(FacadeConfig.UI_STORE_MODULE_ID).OnRecvData(FiPropertyType.GOLD, nUserInfo.gold);
	//			Facade.GetFacade().ui.Get(FacadeConfig.UI_STORE_MODULE_ID).OnRecvData(FiPropertyType.DIAMOND, nUserInfo.diamond);
	//			Facade.GetFacade().ui.Get(FacadeConfig.UI_STORE_MODULE_ID).OnRecvData(FiPropertyType.ROOM_CARD, nUserInfo.roomCard);
	//		}
	//		if (Facade.GetFacade().ui.Get(FacadeConfig.UI_NEWSTORE_SHOW) != null)
	//		{
	//			Facade.GetFacade().ui.Get(FacadeConfig.UI_NEWSTORE_SHOW).OnRecvData(FiPropertyType.GOLD, nUserInfo.gold);
	//			Facade.GetFacade().ui.Get(FacadeConfig.UI_NEWSTORE_SHOW).OnRecvData(FiPropertyType.DIAMOND, nUserInfo.diamond);
	//			Facade.GetFacade().ui.Get(FacadeConfig.UI_NEWSTORE_SHOW).OnRecvData(FiPropertyType.ROOM_CARD, nUserInfo.roomCard);
	//		}
	//		IUiMediator nMediator = Facade.GetFacade().ui.Get(FacadeConfig.UI_NEWSTORE_SHOW);

	//		if (nMediator != null)
	//		{
	//			Debug.LogError("RecvBarbetteResponse OnRecvData");
	//			nMediator.OnRecvData(FiEventType.RECV_XL_EXCHANGEBARBETTE_RESPONSE, data);
	//		}
	//	}
	//	else
	//	{
	//		GameObject Window = Resources.Load("Window/WindowTips") as GameObject;
	//		GameObject WindowClone = GameObject.Instantiate(Window);
	//		UITipClickHideManager ClickTip = WindowClone.GetComponent<UITipClickHideManager>();
	//		ClickTip.text.text = "炮座購買失敗!";
	//	}
	//}

	public void OnExit()
	{
		_Instance = null;
		Destroy(this.gameObject);
	}

	//public void OnDestroy()
	//{
	//	EventControl nControl = EventControl.instance();
	//	nControl.removeEventHandler(FiEventType.RECV_XL_EXCHANGEBARBETTE_RESPONSE, RecvBackUnlockBattery);
	//}
}
