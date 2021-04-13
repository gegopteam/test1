using UnityEngine;
using System.Collections;

using UnityEngine.EventSystems;
using AssemblyCSharp;

public class UIToPay : MonoBehaviour
{

	private PurchaseControl purchaseControl = null;
	private DataControl dataControl = null;
	private int userID = 0;
	//这个是活动的类型吧 5011是银龙,5012 金龙 5013 神龙暂时这么写
	public static int DarGonCardType = 0;

	// Use this for initialization

	private string productID = "";
	// private int nProductID = 0;
	//private float productMoney = 0.0f;

	public GameObject IosPayItem;

	public GameObject aliPay;

	public GameObject wechatPay;

	void Start ()
	{
		purchaseControl = PurchaseControl.GetInstance ();
		dataControl = DataControl.GetInstance ();
		userID = dataControl.GetMyInfo ().userID;

		#if UNITY_ANDROID || UNITY_APP_WEB
		IosPayItem.SetActive (false);
		#endif
	}

	// Update is called once per frame
	void Update ()
	{

	}

	#if UNITY_IPHONE && !UNITY_APP_WEB
	public static void OpenApplePay( string  productID )
	{
		MyInfo myInfo = DataControl.GetInstance().GetMyInfo();
		if(null!=myInfo)
		{
			PurchaseControl.GetInstance().BuyProductForIOS(myInfo.userID, productID);
		}
	}













#elif UNITY_ANDROID || UNITY_APP_WEB
	public static void OpenThirdPartPay (int nPrice)
	{
		string path = "Window/ToPayWindow";
		GameObject WindowClone = AppControl.OpenWindow (path);
		WindowClone.SetActive (true);
		UIToPay uiToPay = WindowClone.GetComponent<UIToPay> ();
		MyInfo myInfo = DataControl.GetInstance ().GetMyInfo ();
		if (null != uiToPay) {
			uiToPay.SetPayPrice (nPrice);
			uiToPay.aliPay.SetActive (myInfo.isOpenAlipay == 1);
			uiToPay.wechatPay.SetActive (myInfo.isOpenWechat == 1);
		}
		GetButtonState buttonState = (GetButtonState)Facade.GetFacade().data.Get(FacadeConfig.UI_AND_BUTTON_CLOSE_OR_OPEN);
		if (buttonState.nButtonStateArray.Count >= 4)
		{
			myInfo.isOpenAlipay = buttonState.nButtonStateArray[3];
		}
		if (buttonState.nButtonStateArray.Count >= 5)
		{
			myInfo.isOpenWechat = buttonState.nButtonStateArray[4];
		}
		foreach (var item in buttonState.nButtonStateArray)
		{
			Debug.LogError(item);
		}
	}

	public static void OpenUIToPay (string productID)
	{
		//先判断是否使用三方支付，如果不使用则直接调用ios支付

//		 if(!PurchaseControl.GetInstance().openAllPay)
//		 {
//		 	MyInfo myInfo = DataControl.GetInstance().GetMyInfo();
//		 	if(null!=myInfo)
//		 	{
//		 		PurchaseControl.GetInstance().BuyProductForIOS(myInfo.userID, productID);
//		 	}
//		 	return;
//		 }

		string path = "Window/ToPayWindow";
		GameObject WindowClone = AppControl.OpenWindow (path);
		WindowClone.SetActive (true);
		UIToPay uiToPay = WindowClone.GetComponent<UIToPay> ();
		if (null != uiToPay) {
			// Debug.LogError("OpenUIToPay null != uiToPay 111111111");
			uiToPay.SetProduct (productID);
		} else {
			// Debug.LogError("OpenUIToPay null == uiToPay 22222222");
		}

	}

	private float SumProductMoney (string productID)
	{
		float money = 0.0f;
		switch (productID) {
		//特惠礼包
		case ProductID.Pack_Preference_CNY_6:
		// nProductID = ProductID.NPack_Preference_CNY_6;
			money = 6;
			break;

		//房卡
		case ProductID.Card_Room_CNY_6:
		// nProductID = ProductID.NCard_Room_CNY_6;
			money = 6;
			break;

		//金币钻石
		case ProductID.Gold_CNY_6:
		// nProductID = ProductID.NGold_CNY_6;
			money = 6;
			break;
		case ProductID.Diamond_CNY_6:
		// nProductID = ProductID.NDiamond_CNY_6;
			money = 6;
			break;

		case ProductID.Gold_CNY_30:
		// nProductID = ProductID.NGold_CNY_30;
			money = 30;
			break;
		case ProductID.Diamond_CNY_30:
		// nProductID = ProductID.NDiamond_CNY_30;
			money = 30;
			break;

		case ProductID.Gold_CNY_68:
        // nProductID = ProductID.NGold_CNY_68;
			money = 68;
			break;
		case ProductID.Diamond_CNY_68:
		// nProductID = ProductID.NDiamond_CNY_68;
			money = 68;
			break;

		case ProductID.Gold_CNY_118:
		// nProductID = ProductID.NGold_CNY_118;
			money = 118;
			break;
		case ProductID.Diamond_CNY_118:
		// nProductID = ProductID.NDiamond_CNY_118;
			money = 118;
			break;

		case ProductID.Gold_CNY_198:
		// nProductID = ProductID.NGold_CNY_198;
			money = 198;
			break;
		case ProductID.Diamond_CNY_198:
		// nProductID = ProductID.NDiamond_CNY_198;
			money = 198;
			break;

		case ProductID.Gold_CNY_348:
		// nProductID = ProductID.NGold_CNY_348;
			money = 348;
			break;
		case ProductID.Diamond_CNY_348:
		// nProductID = ProductID.NDiamond_CNY_348;
			money = 348;
			break;

		//月卡
		case ProductID.Dragon_SilverCard:
		// nProductID = ProductID.NCard_Month_CNY_28;
			money = 28;
			break;
		case ProductID.Dragon_GlodCard:
			// nProductID = ProductID.NCard_Month_CNY_28;
			money = 28;
			break;
		case ProductID.Dragon_ShenlongCard:
			// nProductID = ProductID.NCard_Month_CNY_28;
			money = 68;
			break;

		}
		money = 0.01f; //测试用，上线前去掉
		return money;
	}



	int nSelectPrice = 0;

	public void SetPayPrice (int nPrice)
	{
		nSelectPrice = nPrice;
	}

	public void SetProduct (string id)
	{
		productID = id;
		//productMoney = SumProductMoney(productID);
	}

	public void ToApple (GameObject obj)
	{
//		if(0==userID || ""==productID)
//			return;
//        //string productId = ProductID.Gold_CNY_6;
//        purchaseControl.BuyProductForIOS(userID, productID);
//		DestroyObject(obj);
//		ClearBuy();
	}

	public void ToAlipay (GameObject obj)
	{
		if (0 == userID)
			return;
		//Debug.LogError("ToAlipay userID:"+userID+" productID:"+productID+" productMoney:"+productMoney);
		purchaseControl.BuyForAlipay (userID, nSelectPrice);// BuyProductForAlipay(userID, productID);
		DestroyObject (obj);
		ClearBuy ();
	}

	public void ToWXPay (GameObject obj)
	{
		LoginUtil.GetIntance ().IsDown ();
		if (0 == userID)
			return;
		if (LoginUtil.GetIntance ().IsWXAPPDown) {
			purchaseControl.BuyForWX (userID, nSelectPrice);
			DestroyObject (obj);
			ClearBuy ();
		} else {
			GameObject Window1 = UnityEngine.Resources.Load ("Window/WindowTipsThree")as UnityEngine.GameObject;
			GameObject WindowClone1 = UnityEngine.GameObject.Instantiate (Window1);
			UITipAutoNoMask ClickTips1 = WindowClone1.GetComponent<UITipAutoNoMask> ();
			ClickTips1.tipText.text = "暫未安裝微信應用";
		}

	}
	#endif

	public void ToCancel (GameObject obj)
	{
		DestroyObject (obj);
	}

	private void ClearBuy ()
	{
		productID = "";
		// nProductID = 0;
		// productMoney = 0;
	}
}
