using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;
using System.Runtime.InteropServices;
using AssemblyCSharp;
using System;
using LitJson;

public class UIStore : MonoBehaviour,IUiMediator
{
	public delegate void HideDelegate ();

	public static event HideDelegate HideEvent;

	public static bool openWindow;

	private PurchaseControl purchaseControl = null;

	public static UIStore instance = null;

	GameObject tipsGameObj;
	GameObject coinNotEnoughObj;

	//商城中新加入的展示金币和钻石数量
	public Text GoldTxtInStore;
	public Text DiamondTxtInStore;

	public GameObject[] mTargetArray;

	public GameObject[] WebProducts;
	public GameObject[] iOSProducts;
	public GameObject[] barbetteObj;

	private ProductsItem[] productsData = null;

	MyInfo mInfo;

	Button sureBtn;
	Button cancleBtn;
	Button sureCoinNotEnough;
	/// <summary>
	/// 加载面板
	/// </summary>
	GameObject mWaitingView;
	GameObject AssistantView;
	bool isBuyDiamond;
	//上次点击的金币购买
	public int itemBuyCoin = -1;
	//上次点击的龙卡购买
	public static int itemBuyCard = 0;

	//这里是将camaer转换
	private GameObject store;
	private Camera storeCamera;
	private Canvas mainCanvas;

	GameObject dimondParent;
	GameObject coinParent;

	public UIStore ()
	{
		
	}

	~UIStore ()
	{
		purchaseControl.SetUIStore (null);
	}

	void Awake ()
	{
		if (AppInfo.isInHall) {
			store = GameObject.FindGameObjectWithTag ("MainCamera");
			Debug.Log (store.name);
			storeCamera = store.GetComponent<Camera> ();
			mainCanvas = transform.GetComponentInChildren<Canvas> ();
			mainCanvas.worldCamera = storeCamera;
		} else {
			store = GameObject.FindGameObjectWithTag ("UICamera");
			Debug.Log (store.name);
			storeCamera = store.GetComponent<Camera> ();
			mainCanvas = transform.GetComponentInChildren<Canvas> ();
			mainCanvas.worldCamera = storeCamera;
		}
			
		mInfo = (MyInfo)Facade.GetFacade ().data.Get (FacadeConfig.USERINFO_MODULE_ID);
		Init();
		if (GameController._instance == null) //渔场和大厅里赋值的摄像机不同
            gameObject.GetComponent<Canvas> ().worldCamera = GameObject.FindGameObjectWithTag ("MainCamera").GetComponent<Camera> ();
		else
			gameObject.GetComponent<Canvas> ().worldCamera = ScreenManager.uiCamera;
		instance = this;

		purchaseControl = PurchaseControl.GetInstance ();
		purchaseControl.SetUIStore (this);
		Facade.GetFacade ().ui.Add (FacadeConfig.UI_NEWSTORE_SHOW, this);
	
		
	}

	void HideThirdPayView ()
	{
		mTargetArray [3].SetActive (false);
		mTargetArray [2].SetActive (false);

		mTargetArray [4].SetActive (false);
		mTargetArray [6].SetActive (true);
	}

	//void ShowTest ()
	//{
	//	string Urltest;
	//	Urltest = "http://183.131.69.227:8004/pay/notifygameserverfortest" + "?UserID=" + mInfo.userID;
	//	Debug.LogError ("Urltest" + Urltest);
	//	//		OpenWebScript.Instance.SetActivityWebUrl (Urltest);
	//	Application.OpenURL (Urltest);

	//}

	void SetProductDetail (GameObject nEntity, string priceView, string nCoins, string freePersent)
	{
		nEntity.SetActive(true);
		nEntity.transform.Find ("TxtPrice").GetComponent<Text> ().text = priceView;
		nEntity.transform.Find ("TxtCount").GetComponent<Text> ().text = nCoins;
		if (!string.IsNullOrEmpty (freePersent)) {
			nEntity.transform.Find ("CoinImage (1)").gameObject.SetActive (true);
			nEntity.transform.Find ("CoinImage (1)").GetComponentInChildren<Text> ().text = freePersent;
		} else {
			nEntity.transform.Find ("CoinImage (1)").gameObject.SetActive (false);
		}
	}

	// Use this for initialization
	void Start ()
	{
//#if UNITY_IPHONE && !UNITY_APP_WEB //&& ! UNITY_EDITOR
//		// mTargetArray [3].gameObject.SetActive ( true );
//		//mTargetArray [6].gameObject.SetActive (true);
//		mTargetArray [4].gameObject.SetActive (false);
//#elif UNITY_ANDROID || UNITY_APP_WEB
//        //mTargetArray[4].gameObject.SetActive (true);
//        //mTargetArray [6].gameObject.SetActive (false);
//#endif


        UIColseManage.instance.ShowUI (this.gameObject);

		//try {
			
		//}
		//catch (Exception e) {
		//	Debug.Log(e.ToString());
        //}

		JsonData nProducts = PurchaseInApp.instance.GetProducts();
		Debug.Log(nProducts.ToString());
		Debug.Log(nProducts["data"].ToString());
		ProductsItem[] nData = JsonMapper.ToObject<ProductsItem[]>(JsonMapper.ToJson(nProducts["data"]));
		productsData = nData;
		//Debug.LogError ( nData.Length + " / " + nData[ 0 ].goodsid );
		Debug.Log("!!!!!!!!!!!!!!!!!!!mInfo.Pay_Store_RMB size = "+ mInfo.Pay_Store_RMB.Count);
		if (mInfo.Pay_Store_RMB.Count <= 6) {
			mInfo.Pay_Store_RMB.Sort();
			mInfo.Pay_Store_AddGold.Sort();
		}
		
		int nIndex = 0;
		Debug.Log("nData.Length = "+nData.Length);
		for (int i = 0; i < nData.Length; i++)
		{
			if (!string.IsNullOrEmpty(nData[i].iosItemid))
			{
				continue;
			}
			if (nIndex < 6)
			{
                #if UNITY_IPHONE && !UNITY_APP_WEB
				    //SetProductDetail (iOSProducts [nIndex], nData [i].priceView, nData [i].coinView, nData [i].freeView);
				    //PurchaseControl.GetInstance ().SetPurchasePrice (i, nData [i].coinView);
                    try
				        {
                        SetProductDetail(iOSProducts[nIndex], "¥"+mInfo.Pay_Store_RMB[i], mInfo.Pay_Store_AddGold[i]+"金幣", nData[i].freeView);
				        PurchaseControl.GetInstance().SetOtherPurchasePrice(i, ""+mInfo.Pay_Store_RMB[i]);
                    }
				        catch { }
                #elif UNITY_ANDROID || UNITY_APP_WEB
				    //SetProductDetail (WebProducts [nIndex], nData [i].priceView, nData [i].coinView, nData [i].freeView);
				    //PurchaseControl.GetInstance().SetOtherPurchasePrice(i, nData[i].coinView);
				    Debug.Log("IsNullOrEmpty mInfo.Pay_Store_RMB = "+ mInfo.Pay_Store_RMB[i]);
				    Debug.Log("IsNullOrEmpty mInfo.Pay_Store_AddGold = " + mInfo.Pay_Store_AddGold[i]);
				    Debug.Log("==================================================");
				    try
				    {
					    SetProductDetail(WebProducts[nIndex], "¥" + mInfo.Pay_Store_RMB[i], mInfo.Pay_Store_AddGold[i] + "金幣", nData[i].freeView);
					    PurchaseControl.GetInstance().SetOtherPurchasePrice(i, "" + mInfo.Pay_Store_RMB[i]);
				    }
				    catch { }
				    
                #endif
				    // Debug.LogError("----设置价格---:"+nData[i].coinView);
				    nIndex++;
			}
		}


		//Init ();

		ResetStore scroll = GetComponentInChildren<ResetStore>();
		scroll.SetAmount();

		try
		{
			GetButtonState buttonState = (GetButtonState)Facade.GetFacade().data.Get(FacadeConfig.UI_AND_BUTTON_CLOSE_OR_OPEN);

			if (buttonState.nButtonStateArray.Count > 2 && buttonState.nButtonStateArray[2] == 0)
			{
				WebProducts[5].SetActive(false);
				iOSProducts[5].SetActive(false);

			}
			else if (buttonState.nButtonStateArray.Count > 2 && buttonState.nButtonStateArray[2] == 1)
			{
				WebProducts[5].SetActive(true);
				iOSProducts[5].SetActive(true);
			}
		}
		catch {
			Debug.LogError("InvalidCastException: Cannot cast from source type to destination type.");
        }
	}

	void Init ()
	{
		tipsGameObj = transform.Find ("StoreWindow/Tips").gameObject;
		coinNotEnoughObj = transform.Find ("StoreWindow/CoinNotEnoughTips").gameObject;
		sureBtn = transform.Find ("StoreWindow/Tips/sure").GetComponent <Button> ();
		cancleBtn = transform.Find ("StoreWindow/Tips/cancel").GetComponent <Button> ();
		sureCoinNotEnough = transform.Find ("StoreWindow/CoinNotEnoughTips/sure").GetComponent <Button> ();
		dimondParent = transform.Find ("StoreWindow/Top/DiamondMoudle").gameObject;
		coinParent = transform.Find ("StoreWindow/Top/GoldMoudle").gameObject;

		tipsGameObj.SetActive (false);
		coinNotEnoughObj.SetActive (false);

		cancleBtn.onClick.RemoveAllListeners ();
		sureBtn.onClick.RemoveAllListeners ();
		sureCoinNotEnough.onClick.RemoveAllListeners ();

		cancleBtn.onClick.AddListener (ClickCancleBtn);
		sureBtn.onClick.AddListener (ClickSureBtn);
//		Debug.LogError ("sureBtn.name = " + sureBtn.name);
		sureCoinNotEnough.onClick.AddListener (ClickSureCoinNotEnoughBtn);

		if (AppInfo.isInHall) {
			SetDiamond (mInfo.diamond);
			SetGold (mInfo.gold); 
		} else {
			dimondParent.SetActive (false);
			coinParent.SetActive (false);
			mTargetArray [7].SetActive (false);
			mTargetArray [8].SetActive (false);
			mTargetArray [11].SetActive (false);
		}
	}

	void ClearContent ()
	{
		for (int i = 0; i < mTargetArray.Length; i++) {
			if (mTargetArray [i].transform.Find ("on") != null) {
				mTargetArray [i].transform.Find ("on").gameObject.SetActive (false);
				mTargetArray [i].transform.Find ("off").gameObject.SetActive (true);
			} else {
				mTargetArray [i].SetActive (false);
			}
		}
	}

	public void SelectApplePay ()
	{
		if (mTargetArray [5].activeSelf) {
			CoinButton ();
		}

		// mTargetArray [2].transform.Find ( "on" ).gameObject.SetActive( false );
		// mTargetArray [2].transform.Find ( "off" ).gameObject.SetActive( true );

		// mTargetArray [3].transform.Find ( "on" ).gameObject.SetActive( true );
		// mTargetArray [3].transform.Find ( "off" ).gameObject.SetActive( false );

		mTargetArray [4].SetActive (false);
		mTargetArray [6].SetActive (true);
	}

	public void SelectThirdPay ()
	{
		if (mTargetArray [5].activeSelf) {
			CoinButton ();
		}
		// mTargetArray [3].transform.Find ( "on" ).gameObject.SetActive( false );
		// mTargetArray [3].transform.Find ( "off" ).gameObject.SetActive( true );
		// mTargetArray [2].transform.Find ( "on" ).gameObject.SetActive( true );
		// mTargetArray [2].transform.Find ( "off" ).gameObject.SetActive( false );

		mTargetArray [4].SetActive (true);
		mTargetArray [6].SetActive (false);
	}

	int count = 0;

	public void BuyDiamond (int nCount)
	{
		/**GameObject Window1 = UnityEngine.Resources.Load ("Window/WindowTipsThree")as UnityEngine.GameObject;
		GameObject WindowClone1 = UnityEngine.GameObject.Instantiate ( Window1 );
		UITipAutoNoMask ClickTips1 = WindowClone1.GetComponent<UITipAutoNoMask> ();
		ClickTips1.tipText.text = "钻石暂未开放购买";*/
		switch (nCount) {
		case 50:
			TipsForCoinNotEnough (80000, nCount);
			break;
		case 100:
			TipsForCoinNotEnough (160000, nCount);
			break;
		case 500:
			TipsForCoinNotEnough (800000, nCount);
			break;
		case 1000:
			TipsForCoinNotEnough (1600000, nCount);
			break;
		case 2000:
			TipsForCoinNotEnough (3200000, nCount);
			break;
		case 5000:
			TipsForCoinNotEnough (8000000, nCount);
			break;
		default:
			break;
		}
		//Facade.GetFacade ().message.toolPruchase.SendExchangeDiamondRequest (nCount);
	}

	void TipsForCoinNotEnough (int coinNum, int nCount)
	{
		Text textTemp = tipsGameObj.transform.Find ("Label").GetComponent <Text> ();
		Debug.LogError ("textTemp . name = " + textTemp.name);
		long isenoughgold;

		if (AppInfo.isInHall) {
			isenoughgold = mInfo.gold;
		} else {
			isenoughgold = PrefabManager._instance.GetLocalGun ().currentGold;
		}

		if (isenoughgold < coinNum) {
			coinNotEnoughObj.SetActive (true);
			Debug.LogError ("coinNotEnoughObj.name  = " + coinNotEnoughObj.name);
		} else {
			if (isBuyDiamond) {
				textTemp.text = "確定兌換鑽石嗎?";
				tipsGameObj.SetActive (true);
				count = nCount;
			} else {
				//这里需要弹出是否购买炮座的提示
				textTemp.text = "確定購買砲座嗎?";
				tipsGameObj.SetActive (true);
				count = nCount;
			}
		}
	}

	void ClickSureBtn ()
	{
		if (isBuyDiamond) {
			Facade.GetFacade ().message.toolPruchase.SendExchangeDiamondRequest (count);
			tipsGameObj.SetActive (false);
		} else {
			//TODO,发送购买炮座协议
			Facade.GetFacade ().message.toolPruchase.SendExchangeBarbetteRequest (count);
			tipsGameObj.SetActive (false);
		}
	}


	void ClickCancleBtn ()
	{
		tipsGameObj.SetActive (false);
	}

	void ClickSureCoinNotEnoughBtn ()
	{
		Debug.LogError ("點擊");
		coinNotEnoughObj.SetActive (false);
	}

	//新版本只有金币才能才需要购买，钻石用金币兑换
	public void BuyCoin (int item)
	{
		#if UNITY_ANDROID || UNITY_APP_WEB
		if (null == productsData)
			return;
		if (item >= productsData.Length || item < 0)
			return;

		//		Debug.LogError("---BuyCoin roductsData.Length:" + productsData.Length);
		//		Debug.LogError("---BuyCoin item:" + item);
		//		Debug.LogError("---BuyCoin productsData[item].price:" + productsData[item].price);
		try {
			int price = productsData[item].price;
			int RMB = (int)mInfo.Pay_Store_RMB[item];
			//Debug.Log(" !!!!! !!!!!OpenThirdPartPay price = " + price);
			UIToPay.OpenThirdPartPay(RMB);
		}
		catch (Exception e) {
			Debug.LogError(e);
        }
		
//		ShowTest ();


        #endif

		UIStore.instance.itemBuyCoin = item;
	}

	public void AppleBuyClick (int nPrice)
	{
		Debug.LogError ("-----------------" + nPrice);
		ShowStoreWaitingView (true);
		#if UNITY_IPHONE && !UNITY_APP_WEB
		switch (nPrice) {
		case 6:
			UIToPay.OpenApplePay (ProductID.CL_GOLD_6);
			break;
		case 18:
			UIToPay.OpenApplePay (ProductID.CL_GOLD_18);
			break;
		case 50:
			UIToPay.OpenApplePay (ProductID.CL_GOLD_50);
			break;
		case 98:
			UIToPay.OpenApplePay (ProductID.CL_GOLD_98);
			break;
		case 198:
			UIToPay.OpenApplePay (ProductID.CL_GOLD_198);
			break;
		case 488:
			UIToPay.OpenApplePay (ProductID.CL_GOLD_488);
			break;
		// case 298:
		// 	UIToPay.OpenApplePay ( ProductID.CL_GOLD_298 );
		// 	break;
		}
		#endif
	}

	// Update is called once per frame
	void OnDestroy ()
	{
		instance = null;
	}

	public void OnClose ()
	{
		Destroy (gameObject);
		UIColseManage.instance.CloseUI ();
		//transform.gameObject.SetActive (false);
	}


	public void ToVip ()
	{
		//发送事件，将VIP窗口显示出来
		GameObject Window = Resources.Load ("Window/VIP")as GameObject;
		Instantiate (Window);
		if (transform.Find ("VIP") != null) {
			
		}
		if (HideEvent != null) {
			HideEvent ();
		}
		OnClose ();
	}

	//0 coin 1 diamond 2 inland 3 apple
	public void CoinButton ()
	{
		transform.Find ("StoreWindow/Board").gameObject.SetActive (true);

		if (mTargetArray [6].activeSelf)
			return;
		ClearContent ();

		//打开支付验证
		// if (DataControl.GetInstance ().GetMyInfo ().mSvrData.payAuthor == 1) 
		if (false) {
			HideThirdPayView ();
		} else {
            #if UNITY_IPHONE && !UNITY_APP_WEB//&& ! UNITY_EDITOR
                if (AppInfo.trenchNum > 51000000)
			    {
				    mTargetArray [6].SetActive (true);
			    }
			    else {
				    mTargetArray[13].SetActive(true);
			    }
			
            #elif UNITY_ANDROID || UNITY_APP_WEB
			    if (AppInfo.trenchNum == 51000000)
			    {
				    mTargetArray[4].SetActive(true);
			    }
			    else {
				    mTargetArray[13].SetActive(true);
			    }   
			#endif
			mTargetArray [9].SetActive (false);
			mTargetArray [10].SetActive (false);
//			mTargetArray [12].SetActive (false);
			mTargetArray [1].transform.Find ("on").gameObject.SetActive (false);
			mTargetArray [1].transform.Find ("off").gameObject.SetActive (true);

			mTargetArray [2].transform.Find ("on").gameObject.SetActive (true);
			mTargetArray [2].transform.Find ("off").gameObject.SetActive (false);
			mTargetArray [3].transform.Find ("on").gameObject.SetActive (false);
			mTargetArray [3].transform.Find ("off").gameObject.SetActive (true);
		}
		mTargetArray [0].transform.Find ("on").gameObject.SetActive (true);
		mTargetArray [0].transform.Find ("off").gameObject.SetActive (false);

		mTargetArray [7].transform.Find ("on").gameObject.SetActive (false);
		mTargetArray [7].transform.Find ("off").gameObject.SetActive (true);

		mTargetArray [8].transform.Find ("on").gameObject.SetActive (false);
		mTargetArray [8].transform.Find ("off").gameObject.SetActive (true);

//		mTargetArray [11].transform.Find ("on").gameObject.SetActive (false);
//		mTargetArray [11].transform.Find ("off").gameObject.SetActive (true);
	}

	public void DiamondButton ()
	{
		transform.Find ("StoreWindow/Board").gameObject.SetActive (true);
		isBuyDiamond = true;
        #if UNITY_IPHONE && !UNITY_APP_WEB //&& ! UNITY_EDITOR
		    mTargetArray [5].SetActive (true);
		    mTargetArray [6].SetActive (false);
            mTargetArray[13].SetActive(false);
        #elif UNITY_ANDROID || UNITY_APP_WEB
		    mTargetArray[5].SetActive (true);
		    mTargetArray [4].SetActive (false);
		    mTargetArray[13].SetActive(false);
		    Debug.Log (mTargetArray [4].activeSelf + "============");
		#endif

		mTargetArray [9].SetActive (false);
		mTargetArray [10].SetActive (false);
//		mTargetArray [12].SetActive (false);
		mTargetArray [1].transform.Find ("on").gameObject.SetActive (true);
		mTargetArray [1].transform.Find ("off").gameObject.SetActive (false);
		mTargetArray [0].transform.Find ("on").gameObject.SetActive (false);
		mTargetArray [0].transform.Find ("off").gameObject.SetActive (true);

		mTargetArray [7].transform.Find ("on").gameObject.SetActive (false);
		mTargetArray [7].transform.Find ("off").gameObject.SetActive (true);

		mTargetArray [8].transform.Find ("on").gameObject.SetActive (false);
		mTargetArray [8].transform.Find ("off").gameObject.SetActive (true);

//		mTargetArray [11].transform.Find ("on").gameObject.SetActive (false);
//		mTargetArray [11].transform.Find ("off").gameObject.SetActive (true);

		if (mTargetArray [5].activeSelf) {
			return;
		}
		ClearContent ();
		if (DataControl.GetInstance ().GetMyInfo ().mSvrData.payAuthor == 1) {
			HideThirdPayView ();
		}

	}

	public void OpenNobel (int type)
	{
		transform.Find ("StoreWindow/Board").gameObject.SetActive (false);
        #if UNITY_IPHONE && !UNITY_APP_WEB //&& ! UNITY_EDITOR
		    mTargetArray [5].SetActive (false);
		    mTargetArray [6].SetActive (false);
            mTargetArray[13].SetActive(false);
        #elif UNITY_ANDROID || UNITY_APP_WEB
		    mTargetArray[5].SetActive (false);
		    mTargetArray [4].SetActive (false);
		    mTargetArray[13].SetActive(false);
		    Debug.Log (mTargetArray [4].activeSelf + "============");
		#endif

		mTargetArray [9].SetActive (false);
		mTargetArray [10].SetActive (false);
//		mTargetArray [12].SetActive (true);
//		UINobel mnobel = mTargetArray [12].GetComponent<UINobel> ();
		mTargetArray [1].transform.Find ("on").gameObject.SetActive (false);
		mTargetArray [1].transform.Find ("off").gameObject.SetActive (true);
		mTargetArray [0].transform.Find ("on").gameObject.SetActive (false);
		mTargetArray [0].transform.Find ("off").gameObject.SetActive (true);

		mTargetArray [7].transform.Find ("on").gameObject.SetActive (false);
		mTargetArray [7].transform.Find ("off").gameObject.SetActive (true);

		mTargetArray [8].transform.Find ("on").gameObject.SetActive (false);
		mTargetArray [8].transform.Find ("off").gameObject.SetActive (true);
//		mTargetArray [11].transform.Find ("on").gameObject.SetActive (true);
//		mTargetArray [11].transform.Find ("off").gameObject.SetActive (false);

		//添加后需要取消下面的注释
		if (mTargetArray [12].activeSelf) {
			return;
		}
		ClearContent ();
		if (DataControl.GetInstance ().GetMyInfo ().mSvrData.payAuthor == 1) {
			HideThirdPayView ();
		}

	}

	/// <summary>
	/// 显示转动条
	/// </summary>
	/// <param name="nValue">If set to <c>true</c> n value.</param>
	public void ShowStoreWaitingView (bool nValue)
	{
		if (nValue) {
			GameObject Window1 = UnityEngine.Resources.Load ("MainHall/Common/WaitingView")as UnityEngine.GameObject;
			mWaitingView = UnityEngine.GameObject.Instantiate (Window1);
			UIWaiting nData = mWaitingView.GetComponentInChildren<UIWaiting> ();
			nData.HideBackGround ();
			nData.SetInfo (5.0f, "請求超時,請再試一次");
		} else {
			if (mWaitingView.activeSelf) {
				Destroy (mWaitingView);
			}
			mWaitingView = null;
		}
	}

	/// <summary>
	/// 点击商城龙卡
	/// </summary>
	public void DragonButton ()
	{
		//TODO : 这里需要自己添加商城龙卡预制体  Insepector面板的 mTargetArray 新加一个元素就ok
		mTargetArray [10].SetActive (true);
		mTargetArray [9].SetActive (false);
		//		mTargetArray [12].SetActive (false);
        #if UNITY_IPHONE && !UNITY_APP_WEB //&& ! UNITY_EDITOR
		    mTargetArray [5].SetActive (false);
		    mTargetArray [6].SetActive (false);
            mTargetArray[13].SetActive(false);
        #elif UNITY_ANDROID || UNITY_APP_WEB
		    mTargetArray[5].SetActive (false);
		    mTargetArray [4].SetActive (false);
		    mTargetArray[13].SetActive(false);
        #endif
		mTargetArray [8].transform.Find ("on").gameObject.SetActive (true);
		mTargetArray [8].transform.Find ("off").gameObject.SetActive (false);
		mTargetArray [1].transform.Find ("on").gameObject.SetActive (false);
		mTargetArray [1].transform.Find ("off").gameObject.SetActive (true);
		mTargetArray [0].transform.Find ("on").gameObject.SetActive (false);
		mTargetArray [0].transform.Find ("off").gameObject.SetActive (true);

		mTargetArray [7].transform.Find ("on").gameObject.SetActive (false);
		mTargetArray [7].transform.Find ("off").gameObject.SetActive (true);

//		mTargetArray [11].transform.Find ("on").gameObject.SetActive (false);
//		mTargetArray [11].transform.Find ("off").gameObject.SetActive (true);

		//龙卡界面没有板子
		transform.Find ("StoreWindow/Board").gameObject.SetActive (false);


		//添加后需要取消下面的注释
		if (mTargetArray [10].activeSelf) {
			return;
		}
		ClearContent ();
		if (DataControl.GetInstance ().GetMyInfo ().mSvrData.payAuthor == 1) {
			HideThirdPayView ();
		}
	}

	//---------------炮座功能

	/// <summary>
	/// 点击左侧炮座按钮
	/// </summary>
	public void BarbetteButton ()
	{

		transform.Find ("StoreWindow/Board").gameObject.SetActive (true);
		mTargetArray [9].SetActive (true);
		mTargetArray [10].SetActive (false);
		//		mTargetArray [12].SetActive (false);
        #if UNITY_IPHONE && !UNITY_APP_WEB //&& ! UNITY_EDITOR
		    mTargetArray [5].SetActive (false);
		    mTargetArray [6].SetActive (false);
            mTargetArray[13].SetActive(false);
        #elif UNITY_ANDROID || UNITY_APP_WEB
		    mTargetArray[5].SetActive (false);
		    mTargetArray [4].SetActive (false);
		    mTargetArray[13].SetActive(false);
        #endif
		mTargetArray [1].transform.Find ("on").gameObject.SetActive (false);
		mTargetArray [1].transform.Find ("off").gameObject.SetActive (true);
		mTargetArray [0].transform.Find ("on").gameObject.SetActive (false);
		mTargetArray [0].transform.Find ("off").gameObject.SetActive (true);

		mTargetArray [7].transform.Find ("on").gameObject.SetActive (true);
		mTargetArray [7].transform.Find ("off").gameObject.SetActive (false);

		mTargetArray [8].transform.Find ("on").gameObject.SetActive (false);
		mTargetArray [8].transform.Find ("off").gameObject.SetActive (true);

//		mTargetArray [11].transform.Find ("on").gameObject.SetActive (false);
//		mTargetArray [11].transform.Find ("off").gameObject.SetActive (true);

		BackpackInfo nBackInfo = (BackpackInfo)Facade.GetFacade ().data.Get (FacadeConfig.BACKPACK_MODULE_ID);
		List<FiBackpackProperty> tempBackPack = nBackInfo.getInfoArray ();
		if (mTargetArray [9].activeSelf) {
			return;
		}
		ClearContent ();
		if (DataControl.GetInstance ().GetMyInfo ().mSvrData.payAuthor == 1) {
			HideThirdPayView ();
		}
	
		//FreshBarBetteState ();
	}

	/// <summary>
	/// 点击购买Button事件
	/// </summary>
	/// <param name="nCount">N count.</param>
	public void BuyBarbette (int nCount)
	{
		isBuyDiamond = false;
		switch (nCount) {
		case 1:
			TipsForCoinNotEnough (300000, nCount);
			break;
		case 2:
			TipsForCoinNotEnough (500000, nCount);
			break;
		case 3:
			TipsForCoinNotEnough (1000000, nCount);
			break;
		default:
			break;
		}
		Debug.LogError ("BuyBarbette count = " + nCount);
	}

	public void OnInit ()
	{
		Debug.LogError ("UIStroe_________OnInit______________________");
		BackpackInfo nBackInfo = (BackpackInfo)Facade.GetFacade ().data.Get (FacadeConfig.BACKPACK_MODULE_ID);
		List<FiBackpackProperty> tempBackPack = nBackInfo.getInfoArray ();
		//获取背包是否有炮座id
		foreach (var item in tempBackPack) {
			if (item.id > 8010 && item.id < 8999) {
				FreshBarBetteState (item.id);
			}
		}
	}

	//加入了之前的接口更新用户当前的金币和钻石
	public void OnRecvData (int nType, object nData)
	{
		if (nType == FiPropertyType.GOLD) {
			this.SetGold ((long)nData);
			Debug.Log (nData);
		} else if (nType == FiPropertyType.DIAMOND) {
			this.SetDiamond ((long)nData);
			Debug.Log (nData);
		}
		Debug.LogError ("是否走到111");
		//通过PurchaseMsgHandle中去调用数据
		if (nType == FiEventType.RECV_XL_EXCHANGEBARBETTE_RESPONSE) {
			FiExchangeBarbette nResult = (FiExchangeBarbette)nData;
			Debug.LogError ("是否走到  RECV_XL_EXCHANGEBARBETTE_RESPONSE ");
			Debug.LogError ("nResult.buyType = " + nResult.buyType);
			Debug.LogError ("nResult.result = " + nResult.result);

			if (nResult.result == 0) {
				if (nResult.buyType <= 0) {
					return;
				} 
				//这里是购买完成之后刷新的状态,再一次获取,就要通过背包数剧是否有炮座信息了..
				switch (nResult.buyType) {
				case 1:
					FreshBarBetteState (nResult.buyType);
					break;
				case 2:
					FreshBarBetteState (nResult.buyType);
					break;
				case 3:
					FreshBarBetteState (nResult.buyType);
					break;
				default:
					break;
				}
			}	
		}
	}

	public void OnRelease ()
	{

	}

	/// <summary>
	/// 刷新商城炮座状态
	/// </summary>
	void FreshBarBetteState (int id)
	{
//		return;
		for (int i = 1; i <= barbetteObj.Length; i++) {
			//如果购买的话..就让他不显示
			if (id % 10 == i) {
				Debug.LogError ("FreshBarBetteState i = " + i);
				barbetteObj [i - 1].transform.Find ("BuyBtn").GetComponent <Button> ().interactable = false;
				barbetteObj [i - 1].transform.Find ("TxtPrice").GetComponent <Text> ().text = "已購買";
			} 
		}
	}

	public void SetGold (long nCount)
	{
		if (nCount > 100000000) {
			GoldTxtInStore.text = "" + (float)(nCount / 1000000) / 100 + "億";

		} else if (nCount >= 1000000) {
			GoldTxtInStore.text = "" + (int)(nCount / 10000) + "萬";
		} else {
			GoldTxtInStore.text = "" + nCount;
			Debug.Log (nCount);
		}
	}

	public void SetDiamond (long nCount)
	{
		if (nCount > 100000000) {
			DiamondTxtInStore.text = "" + (float)(nCount / 1000000) / 100 + "億";
		} else if (nCount >= 1000000) {
			DiamondTxtInStore.text = "" + (int)(nCount / 10000) + "萬";
		} else {
			DiamondTxtInStore.text = "" + nCount;
			Debug.Log (nCount);
		}
	}

	///// <summary>
	///// 2020/07/26 Joey 打開小助手下載
	///// </summary>
	public void OpenAssistant()
	{
		string path = "Window/OpenUrlCanvas";
		AssistantView = AppControl.OpenWindow(path);
		AssistantView.SetActive(true);
		//先创建控件,在进行浏览
		if (OpenWebScript.Instance != null)
		{
			//打开webview控件
			//OpenWebScript.Instance.SetActivityWebUrl(UIUpdate.WebUrlDic[WebUrlEnum.LittleHelper]);
			Application.OpenURL(UIUpdate.WebUrlDic[WebUrlEnum.LittleHelper]);
		}
		//AppControl.miniGameState = true;
		//产品需求说需要关闭音效
		AudioManager._instance.StopBgm();
	}
}
