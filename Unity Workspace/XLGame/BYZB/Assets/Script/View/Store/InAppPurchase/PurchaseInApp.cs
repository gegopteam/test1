using UnityEngine;
using System.Collections;
using System.Security.Cryptography;

using System;
using System.Text;

using LitJson;
using AssemblyCSharp;




//该类负责购买的web流程
//需要被内购UI脚本继承
/*
 * 2020/02/29 Joey 修改 GetPayUrl() 付款網址根據 app 啟動取得配置時 server 給的付款 url 
 */

public class PurchaseOrderForWeb
{
	public string code;
	public string msg;
	public OrderForWeb data;
}

//{"code":0,"msg":"获取成功","data":{"orderid":"2018030215164690049482","rmb":98,"sign":"","coin":980000,"ip":"183.131.213.2","bill_time":"20180302151646","retun_url":""}}

public class WebOrderResult
{
	public int code;
	public string msg;
	public WebOrderData data;
}

public class ProductsItem
{
	public int goodsid;
	public string iosItemid;
	public int price;
	public string priceView;
	public int coin;
	public string coinView;
	public int freeCoin;
	public string freeView;
}

public class WebOrderData
{
	public string orderid;
	public int rmb;
	public string sign;
	public int coin;
	public string ip;
	public string bill_time;
	public string retun_url;
}

public class OrderForWeb
{
	public int orderId;
}

public class PurchaseInApp : MonoBehaviour
{
	//private string IosOrderUrl = "http://api.cl0579.com/api/MobilePayConfig.aspx";
    //推廣版付款 url
	//private string IosOrderUrl = UIUpdate.WebUrlDic[WebUrlEnum.PublishPayWeb];
	private string IosOrderUrl="";
	private string orderIDForWeb = "";
	private string receiptData = "";
	protected string buyProductId = "";

	protected PurchaseControl purchaseControl = null;

	public static PurchaseInApp instance;

	private static int isRequestcout = 0;
	GameObject mWaitingView;

	public PurchaseInApp ()
	{

	}

	~PurchaseInApp ()
	{
		//UnInitPurchaseInApp ();
	}

	void Awake ()
	{
		try {
			InitPurchaseInApp ();
		} catch {
			Debug.LogError ("\r\n [ prucase in app ] awake error!!! \r\n ");
		}
//		RequestProductsDetail ();
		instance = this;
	}

	void Start ()
	{
		ShowWaitingView(false);
		ShowWaitingView(true);

		StartCoroutine(DoSomething());
	}

	private IEnumerator DoSomething()
	{
		// doing something

		// waits 5 seconds
		yield return new WaitForSeconds(5.0f);

		// do something else
		try {
			RequestProductsDetail();
		}
		catch (Exception ex) {
			Debug.Log(ex.ToString());
			StartCoroutine(DoSomething());
		}
		ShowWaitingView(false);
	}

	protected void InitPurchaseInApp ()
	{//初始化内购
		//Debug.Log("!!!!!!@@@@@@@########$$$$$$$$$$$%%%%%%%%%%^^^^^^^^^^^  InitPurchaseInApp");
		purchaseControl = PurchaseControl.GetInstance ();
		purchaseControl.SetRcv (PurchaseMsg.IOS_INIT_BACK, this.RcvIOSPurchaseInit);
		purchaseControl.SetRcv (PurchaseMsg.IOS_BUY_BACK, this.RcvIOSPurchasePay);
		purchaseControl.SetPurchaseInAppObject (this);
		//SndUnfinishedReceipt ();
	}

	protected void UnInitPurchaseInApp ()
	{//反初始化
		purchaseControl.SetRcv (PurchaseMsg.IOS_INIT_BACK, null);
		purchaseControl.SetRcv (PurchaseMsg.IOS_BUY_BACK, null);
	}

	protected GameObject OpenWebView (string url)
	{
		string path = "Window/ToPayWebWindow";
		GameObject payWindow = AppControl.OpenWindow (path);
		payWindow.SetActive (true);
		UIToPayWeb payWeb = payWindow.GetComponent<UIToPayWeb> ();
		if (null == payWeb)
			return null;
		
		payWeb.OpenWebView (url);
		return payWindow;
	}

	protected void CloseWebView (GameObject payWindow)
	{
		if (null != payWindow) {
			UIToPayWeb payWeb = payWindow.GetComponent<UIToPayWeb> ();
			if (null == payWeb)
				return;
			payWeb.CloseWebView ();
		}
		return;
	}

	#if UNITY_IPHONE && !UNITY_APP_WEB
	public void BuyProductForIOS (int userIdx, string productID)
	{//IOS购买
		this.buyProductId = productID;
		if (nProductDetail != null) {
			RequestCreateOrder ();
		}
	}
	











#elif UNITY_ANDROID || UNITY_APP_WEB
	public void BuyForAlipay (int userIdx, int nPrice)
	{//支付宝购买
		MyInfo userInfo = (MyInfo)Facade.GetFacade ().data.Get (FacadeConfig.USERINFO_MODULE_ID);
		OpenWebView ((GetPayUrl (nPrice, 1, userInfo.loginInfo.gameId)));// "http://192.168.185.128:186/sample.html");
		PurchaseControl.GetInstance ().buyGoldCoin = PurchaseControl.GetInstance ().GetOtherPurchasePrice (nPrice);
	}

	public void BuyForWX (int userIdx, int nPrice)
	{//微信购买
		MyInfo userInfo = (MyInfo)Facade.GetFacade ().data.Get (FacadeConfig.USERINFO_MODULE_ID);
		OpenWebView (GetPayUrl (nPrice, 2, userInfo.loginInfo.gameId));
		PurchaseControl.GetInstance ().buyGoldCoin = PurchaseControl.GetInstance ().GetOtherPurchasePrice (nPrice);
	}
	#endif

	string GetMJId ()
	{
		string mjid = "";
#if UNITY_IPHONE && UNITY_APP_WEB
		mjid = "wx9258b8a8a4fbc9d4";
#elif UNITY_ANDROID
		mjid = "com.tencent.tmgp.fishingxinlong";
#endif
		return mjid;
	}

	/*paytype 1 支付宝,2 微信
	devictype  1 Android ,3 IOS
	from  预留活动参数
    2020/02/29 Joey 修改付款網址根據 app 啟動取得配置時 server 給的付款 url 
         */
	string GetPayUrl (float nPrice, int nPlatform, long gameId)
	{
		Debug.LogError ("UIToPay.DarGonCardType = " + UIToPay.DarGonCardType);
		int formtype = 0;
		if (UIToPay.DarGonCardType != 0) {
			formtype = UIToPay.DarGonCardType;
		}
		string urlTmp = "";

		#if UNITY_IPHONE && UNITY_APP_WEB
		urlTmp = UIUpdate.WebUrlDic [WebUrlEnum.PayWeb]+ "?gameid=" + gameId + "&mjid=" + GetMJId () + "&paymoney=" + nPrice + "&paytype=" + nPlatform + "&from=" + formtype + "&devictype=3";
		//urlTmp = "http://api.cl0579.com/Weixin/PayH5/NewL_Pay3D/pay.aspx?gameid=" + gameId + "&mjid=" + GetMJId () + "&paymoney=" + nPrice + "&paytype=" + nPlatform + "&from=" + formtype + "&devictype=3";
        Debug.LogError ("-----------------" + urlTmp);
		#endif

		#if UNITY_ANDROID
        urlTmp = UIUpdate.WebUrlDic[WebUrlEnum.PayWeb] + "?gameid=" + gameId + "&mjid=" + GetMJId () + "&paymoney=" + nPrice + "&paytype=" + nPlatform + "&from=" + formtype + "&devictype=1";
		//urlTmp = "http://api.cl0579.com/Weixin/PayH5/NewL_Pay3D/pay.aspx?gameid=" + gameId + "&mjid=" + GetMJId () + "&paymoney=" + nPrice + "&paytype=" + nPlatform + "&from=" + formtype + "&devictype=1";
        Debug.LogError ("-----------------" + urlTmp);
		#endif

		//Debug.LogError ("-----------------" + urlTmp);
		return urlTmp;
	}

	public int GetInitState ()
	{//获取ios初始化状态
		return purchaseControl.iosInitState;
	}

	public void RcvIOSPurchaseInit (object data, int type = 0)
	{//IOS内购初始化完成 //回调
		string content = (string)data;
		Debug.LogError ("--------RcvIOSPurchaseInit--------:" + content);
		if (0 == type) {//初始化完成
			purchaseControl.iosInitState = 0;
		} else {//初始化失败
			purchaseControl.iosInitState = 1;
		}
	}

	int getAmount ()
	{
		switch (buyProductId) {
		case ProductID.CL_GOLD_6:
			return 6;
		case ProductID.CL_GOLD_18:
			return 18;
		case ProductID.CL_GOLD_50:
			return 50;
		case ProductID.CL_GOLD_98:
			return 98;
		case ProductID.CL_GOLD_198:
			return 198;
		case ProductID.CL_GOLD_488:
			return 488;
		}
		return 0;
	}

	public void RcvIOSPurchasePay (object data, int type = 0)
	{
		//Debug.Log("!!@!@!@!@!@!@!@!@!@!@!@!@#@#@#@#@#@#@#@#@#    RcvIOSPurchasePay");
		//IOS内购支付完成 //回调
		string content = (string)data;
		if (0 == type) {//购买成功
			PurchaseReceipt jsonDataInfo = JsonMapper.ToObject<PurchaseReceipt> (content);
			if (null == jsonDataInfo)
				return;
//			SetPurchasStep (PurchaseStep.BuyProduct_Response);
			VerifyReceipt (DataControl.GetInstance ().GetMyInfo ().userID, orderIDForWeb, jsonDataInfo.orderId, jsonDataInfo.receipt, DataControl.GetInstance ().GetMyInfo ().password);
			Debug.LogError ("[ purchage in app ]购买回调 购买成功");
		} else if (1 == type) {   //购买取消
			//SetPurchasStep (PurchaseStep.Ready);
		} else if (-1 == type) {   //购买失败
			//SetPurchasStep (PurchaseStep.Ready);
		}
	}

	IEnumerator PostGetOrder (string url, WWWForm data, PurchaseEvent handle)
	{
		WWW www = new WWW (url, data);
		yield return www;
		if (null != www.error) {
			Debug.LogError ("PostGetOrder error:" + www.error);
			if (null != handle) {
				//handle (www.error, -1);
			}
		} else {
			Debug.LogError ("----------order   return  ]  :" + www.text);
			try {
				JsonData nData = JsonMapper.ToObject (www.text);
//				Debug.LogError ("-----JsonMapper-----order   return  ]  :"+nData[ "code" ]);
				if (!nData ["code"].ToString ().Equals ("0")) {
					GameObject Window1 = UnityEngine.Resources.Load ("Window/WindowTipsThree")as UnityEngine.GameObject;
					GameObject WindowClone1 = UnityEngine.GameObject.Instantiate (Window1);
					UITipAutoNoMask ClickTips1 = WindowClone1.GetComponent<UITipAutoNoMask> ();
					ClickTips1.tipText.text = nData ["msg"].ToString ();
					PurchaseControl.GetInstance ().CloseWaitingView ();
				}
//				Debug.LogError ("-----JsonMapper-----order   return  ]  :"+nData[ "msg" ]);
				WebOrderResult jsonInfo = JsonMapper.ToObject<WebOrderResult> (www.text);
//				Debug.LogError ("-----JsonMapper-----order   return 2 ]  :"+nData[ "msg" ]);
				//Debug.LogError ("----------order   return 1 ]  :"+jsonInfo.msg + " / " + jsonInfo.data.orderid );
				if (jsonInfo.code == 0) {
					orderIDForWeb = jsonInfo.data.orderid;
//					Debug.LogError ("-----JsonMapper-----order   return 3 ]  :"+nData[ "msg" ]);
					#if UNITY_IPHONE && !UNITY_APP_WEB && !UNITY_EDITOR
				    PurchaseControl.BuyProductIOS ( buyProductId );
					#endif
				}
			} catch {
				Debug.LogError ("----------order json error,  return  ]  :" + www.text);
			}
		}
	}

	/**
    189 -- 98
    190-- 198 
    191-- 298
	*/

	#if UNITY_IPHONE && !UNITY_APP_WEB
	int psid = 1; //Ios
	#elif UNITY_ANDROID || UNITY_APP_WEB
	int psid = 0;
	//H5
	#endif

	int gameId = 510;
	int payType = 3;
	string nMjId = "default";
	string nVersion = "0.0.0";
	string goodsid = "189";
	string verSdk = "0.0.0";
	string deviceType = "3";
	string key = "E39,D-=SM39S10-ATU85NC,53\\kg9";

	IEnumerator  PostIOSVerify (string url, WWWForm data)
	{
		WWW www = new WWW (url, data);
		yield return www;
		if (null != www.error) {
			Debug.LogError ("[ verify ------ ] error:" + www.error);
		} else {
			Debug.LogError ("[ verify ------ ] return:" + www.text);
			Facade.GetFacade ().message.toolPruchase.SendPayStateRequest (orderIDForWeb);
			MyInfo myInfo = DataControl.GetInstance ().GetMyInfo ();
			myInfo.bAfterCharge = true;
			Facade.GetFacade ().message.backpack.SendReloadRequest ();
		}
	}

	protected void VerifyReceipt (int nUserId, string webOrderId, string iosOrderId, string receipt, string passwd)
	{//发送虚拟商品购买凭证进行验证

		Debug.Log("!!@!@!@!@!@!@!@!@!@!@!@!@#@#@#@#@#@#@#@#@#       VerifyReceipt");
		string nVerifyUrl = "https://api.cl0579.com/pay/iOsPay_v4.aspx"; //"http://api.cl0579.com/pay/iOsPay_v3.aspx";
		WWWForm nSendData = new WWWForm ();

		nSendData.AddField ("amount", getAmount ());
		nSendData.AddField ("mode", 0);
		nSendData.AddField ("receiptdata", receipt);
		nSendData.AddField ("userid", nUserId);
		nSendData.AddField ("majianame", nMjId);
		nSendData.AddField ("orderid", webOrderId);
		nSendData.AddField ("psid", psid);  
		nSendData.AddField ("gameid", gameId);  
		nSendData.AddField ("version", nVersion);
		nSendData.AddField ("iosordernum", iosOrderId);
		nSendData.AddField ("pwd", passwd);
		string nSign = key + nMjId + psid.ToString () + gameId.ToString () + nVersion;
		nSendData.AddField ("sign", LoginUtil.GetMD5 (nSign).ToLower ());

		string vskey = nUserId.ToString () + passwd + webOrderId.ToString () + key;
		nSendData.AddField ("key", LoginUtil.GetMD5 (vskey).ToLower ());

		// Debug.LogError("---VerifyReceipt amount:"+getAmount()+" majianame:"+nMjId);

//		Debug.LogError ( 
//
//			"amount=" + getAmount() + "\r\n"+
//			"mode="  + "0" + "\r\n"+
//			"receiptdata="   + receipt + "\r\n"+
//			"userid="   + nUserId + "\r\n"+
//			"majianame="   +  nMjId  + "\r\n"+
//			"orderid="   + webOrderId + "\r\n"+
//			"psid="   + psid + "\r\n"+
//			"gameid="   + gameId + "\r\n"+
//			"version="   + nVersion + "\r\n"+
//			"iosordernum="   + iosOrderId + "\r\n"+
//			"pwd="   + passwd + "\r\n"+
//			"sign="   + LoginUtil.GetMD5(nSign).ToLower() + "\r\n"+
//			"key="   + LoginUtil.GetMD5(vskey).ToLower() + "\r\n"
//		);

		PurchaseControl.GetInstance ().buyGoldCoin = PurchaseControl.GetInstance ().GetPurchasePrice (buyProductId);
		StartCoroutine (PostIOSVerify (nVerifyUrl, nSendData));
	}

	JsonData nProductDetail = null;

	IEnumerator RequestProductPost (string url, WWWForm data, PurchaseEvent handle)
	{
		WWW www = new WWW (url, data);
		yield return www;
		if (null != www.error) {
			isRequestcout++;
			//Debug.LogError ("isweqqww : " + isRequestcout);
			if (3 > isRequestcout) {
				StartCoroutine (RequestProductPost (url, data, handle));
				if (null != handle) {
					//handle (www.error, -1);
				}
			}
		} else {
			//Debug.LogError ("----------PostReturn: "+www.text);
			//string nResultStr = "{\"code\":0,\"msg\":\"获取成功\",\"data\":[{\"goodsid\":75,\"iosItemid\":\"\",\"price\":5,\"priceView\":\"￥5\",\"coin\":80000,\"coinView\":\"80000金币\",\"freeCoin\":0,\"freeView\":\"\"},{\"goodsid\":76,\"iosItemid\":\"\",\"price\":10,\"priceView\":\"￥10\",\"coin\":160000,\"coinView\":\"184000金币\",\"freeCoin\":24000,\"freeView\":\"加送15%\"},{\"goodsid\":77,\"iosItemid\":\"\",\"price\":50,\"priceView\":\"￥50\",\"coin\":800000,\"coinView\":\"960000金币\",\"freeCoin\":160000,\"freeView\":\"加送20%\"},{\"goodsid\":91,\"iosItemid\":\"renminbi98a_3da\",\"price\":98,\"priceView\":\"￥98\",\"coin\":980000,\"coinView\":\"980000金币\",\"freeCoin\":0,\"freeView\":\"\"},{\"goodsid\":78,\"iosItemid\":\"\",\"price\":100,\"priceView\":\"￥100\",\"coin\":1600000,\"coinView\":\"1952000金币\",\"freeCoin\":352000,\"freeView\":\"加送22%\"},{\"goodsid\":101,\"iosItemid\":\"renminbi198a_3da\",\"price\":198,\"priceView\":\"￥198\",\"coin\":1980000,\"coinView\":\"1980000金币\",\"freeCoin\":0,\"freeView\":\"\"},{\"goodsid\":79,\"iosItemid\":\"\",\"price\":200,\"priceView\":\"￥200\",\"coin\":3200000,\"coinView\":\"3968000金币\",\"freeCoin\":768000,\"freeView\":\"加送24%\"},{\"goodsid\":92,\"iosItemid\":\"renminbi298a_3da\",\"price\":298,\"priceView\":\"￥298\",\"coin\":2980000,\"coinView\":\"2980000金币\",\"freeCoin\":0,\"freeView\":\"\"},{\"goodsid\":80,\"iosItemid\":\"\",\"price\":500,\"priceView\":\"￥500\",\"coin\":8000000,\"coinView\":\"10080000金币\",\"freeCoin\":2080000,\"freeView\":\"加送26%\"}],\"status\":-1,\"remark\":\"\"}";
			try {
				//Debug.LogError ("-----------------------------2" + www.text);
				//Debug.Log ("www.text = " + www.text);
				nProductDetail = JsonMapper.ToObject (www.text);
			} catch {
			
			}
			//
		}
	}

	IEnumerator RequestProductGet(string url, string getStr, WWWForm data, PurchaseEvent handle)
	{
		WWW www = new WWW(url+getStr);
		yield return www;
		if (null != www.error)
		{
			isRequestcout++;
			Debug.LogError("isweqqww : " + isRequestcout);
			if (3 > isRequestcout)
			{
				StartCoroutine(RequestProductPost(url, data, handle));
				if (null != handle)
				{
					//handle (www.error, -1);
				}
			}
		}
		else
		{
			//RequestProductsDetailDebug.LogError("----------PostReturn: " + www.text);
			//string nResultStr = "{\"code\":0,\"msg\":\"获取成功\",\"data\":[{\"goodsid\":75,\"iosItemid\":\"\",\"price\":5,\"priceView\":\"￥5\",\"coin\":80000,\"coinView\":\"80000金币\",\"freeCoin\":0,\"freeView\":\"\"},{\"goodsid\":76,\"iosItemid\":\"\",\"price\":10,\"priceView\":\"￥10\",\"coin\":160000,\"coinView\":\"184000金币\",\"freeCoin\":24000,\"freeView\":\"加送15%\"},{\"goodsid\":77,\"iosItemid\":\"\",\"price\":50,\"priceView\":\"￥50\",\"coin\":800000,\"coinView\":\"960000金币\",\"freeCoin\":160000,\"freeView\":\"加送20%\"},{\"goodsid\":91,\"iosItemid\":\"renminbi98a_3da\",\"price\":98,\"priceView\":\"￥98\",\"coin\":980000,\"coinView\":\"980000金币\",\"freeCoin\":0,\"freeView\":\"\"},{\"goodsid\":78,\"iosItemid\":\"\",\"price\":100,\"priceView\":\"￥100\",\"coin\":1600000,\"coinView\":\"1952000金币\",\"freeCoin\":352000,\"freeView\":\"加送22%\"},{\"goodsid\":101,\"iosItemid\":\"renminbi198a_3da\",\"price\":198,\"priceView\":\"￥198\",\"coin\":1980000,\"coinView\":\"1980000金币\",\"freeCoin\":0,\"freeView\":\"\"},{\"goodsid\":79,\"iosItemid\":\"\",\"price\":200,\"priceView\":\"￥200\",\"coin\":3200000,\"coinView\":\"3968000金币\",\"freeCoin\":768000,\"freeView\":\"加送24%\"},{\"goodsid\":92,\"iosItemid\":\"renminbi298a_3da\",\"price\":298,\"priceView\":\"￥298\",\"coin\":2980000,\"coinView\":\"2980000金币\",\"freeCoin\":0,\"freeView\":\"\"},{\"goodsid\":80,\"iosItemid\":\"\",\"price\":500,\"priceView\":\"￥500\",\"coin\":8000000,\"coinView\":\"10080000金币\",\"freeCoin\":2080000,\"freeView\":\"加送26%\"}],\"status\":-1,\"remark\":\"\"}";
			try
			{
				//Debug.LogError ("-----------------------------2" + www.text);
				//Debug.Log ("www.text = " + www.text);
				nProductDetail = JsonMapper.ToObject(www.text);
			}
			catch
			{

			}
			//
		}
	}

	public JsonData GetProducts ()
	{
		return nProductDetail;
	}

	void RequestProductsDetail ()
	{//获取产品信息
		string nSignStr = "E39,D-=SM39S10-ATU85NC,53\\kg9" + nMjId + "" + psid + "" + gameId + "";
		//Debug.LogError ( "[-----nSignStr------]" + nSignStr );
		WWWForm form = new WWWForm ();  
		form.AddField ("act", "configs");  
		form.AddField ("mjid", nMjId);  
		// form.AddField("gameid",508); 
		form.AddField ("gameid", gameId);
		form.AddField ("psid", psid);  
		form.AddField ("sign", LoginUtil.GetMD5 (nSignStr).ToLower ());
		Debug.LogError ("-----------------------------1");
		//Debug.LogError ("RequestProductsDetail RequestProductsDetail nMjId =" + nMjId + " ;psid = " + psid + " ;gameId = " + gameId + " ;sign = " + LoginUtil.GetMD5(nSignStr).ToLower());//default 0 510
        String getStr = "?act=configs&mjid=" + nMjId + "&psid=" + psid + "&gameId=" + gameId + "&sign=" + LoginUtil.GetMD5(nSignStr).ToLower();
		IosOrderUrl = UIUpdate.WebUrlDic[WebUrlEnum.PromotePayWeb];
		//Debug.LogError("IosOrderUrl = " + IosOrderUrl);
		StartCoroutine (RequestProductGet(IosOrderUrl, getStr, form, null));
	}

	//生产订单，然后调用iOS支付
	void RequestCreateOrder ()
	{
		if (buyProductId.Equals (ProductID.CL_GOLD_6)) {
			goodsid = "327";
		} else if (buyProductId.Equals (ProductID.CL_GOLD_18)) {
			// goodsid = "189";
			goodsid = "331";
		} else if (buyProductId.Equals (ProductID.CL_GOLD_50)) {
			// goodsid = "189";
			goodsid = "334";
		} else if (buyProductId.Equals (ProductID.CL_GOLD_98)) {
			// goodsid = "189";
			goodsid = "338";
		} else if (buyProductId.Equals (ProductID.CL_GOLD_198)) {
			// goodsid = "190";
			goodsid = "342";
		} else if (buyProductId.Equals (ProductID.CL_GOLD_488)) {
			// goodsid = "190";
			goodsid = "343";
		} else {
			goodsid = "191";
		}

		string nSignStr = "E39,D-=SM39S10-ATU85NC,53\\kg9" + nMjId + "" + psid + "" + nVersion + "" + goodsid + "" + payType + "" + verSdk + "" + gameId + "" + DataControl.GetInstance ().GetMyInfo ().userID + "" + deviceType;
		Debug.LogError ("---RequestCreateOrder---" + nSignStr);
		WWWForm form = new WWWForm ();  
		form.AddField ("act", "order");

		form.AddField ("mjid", nMjId);  
		form.AddField ("psid", psid);  
		form.AddField ("sign", LoginUtil.GetMD5 (nSignStr).ToLower ());  

		form.AddField ("gameid", gameId);  
		form.AddField ("version", nVersion);
		form.AddField ("versdk", verSdk);

		form.AddField ("goodsid", goodsid);
		form.AddField ("payType", payType);
		form.AddField ("deviceType", deviceType);
		form.AddField ("usrid", DataControl.GetInstance ().GetMyInfo ().userID);

		// Debug.LogError("---sign:"+LoginUtil.GetMD5( nSignStr ).ToLower());
		// Debug.LogError("---mjid:"+nMjId+" psid:"+psid+" gameid:"+gameId+" version:"+nVersion+" versdk:"+verSdk+" goodsid:"+goodsid+" payType:"+payType+" deviceType:"+deviceType+" usrid:"+DataControl.GetInstance().GetMyInfo().userID);
		IosOrderUrl = UIUpdate.WebUrlDic[WebUrlEnum.PromotePayWeb];
		StartCoroutine (PostGetOrder (IosOrderUrl, form, null));
	}

	void OnGUI ()
	{

	}

	public void ShowWaitingView(bool nValue)
	{
		if (AppControl.miniGameState)
		{

		}
		else
		{
			if (nValue)
			{
				GameObject Window1 = UnityEngine.Resources.Load("MainHall/Common/WaitingView") as UnityEngine.GameObject;
				mWaitingView = Instantiate(Window1);
				UIWaiting nData = mWaitingView.GetComponentInChildren<UIWaiting>();
				nData.HideBackGround();
				nData.SetInfo(20f, "獲取配置信息異常");
			}
			else
			{
				Destroy(mWaitingView);
				mWaitingView = null;
			}
		}
	}
}
