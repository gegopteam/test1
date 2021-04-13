using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using System.IO;
using System.Text;

using LitJson;

using System.Runtime.InteropServices;

//该类负责主要的内购流程

public class PurchasePrice //苹果支付
{
	public string productID; //产品id
	public string goldCoin; //金币
}

public class OtherPurchasePrice //三方支付，alipay,wx
{
	public int price; //产品价格
	public string goldCoin; //金币
}

public class PurchaseControl
{
	private static PurchaseControl instance = null;
//	private static object objGet = new object();
	public static PurchaseControl GetInstance()
	{
//		lock(objGet)
//		{
			if(null == instance)
			{
				instance = new PurchaseControl ();
			}
//		}

		return instance;
	}

	public static void DestroyInstance()
	{
		if(null!=instance)
		{
			instance = null;
		}
	}

	#if UNITY_IPHONE && !UNITY_APP_WEB
	//iOS内购支付
	[DllImport("__Internal")]
	public static extern void InitPurchaseIOS ();
	[DllImport("__Internal")]
	public static extern void BuyProductIOS (string productID);
	[DllImport("__Internal")]
	public static extern void SetPurchaseBackIOS (string obj, string method);
	#endif

	public int iosInitState = -1; //-1未初始化; 0初始化完成; 1初始化失败;
	public int iosPurchasStep = PurchaseStep.Ready; //0未进行购买; >0购买步骤,即购买ing;

	public string productID = "";
	public float money = 0.01f;
	public string buyGoldCoin = "";
	public PurchasePrice [] purchasePrice = new PurchasePrice[6]; 
	public OtherPurchasePrice[] otherPurchasePrice = new OtherPurchasePrice[6];

	private PurchaseEvent funIOSInitBack = null;
	private PurchaseEvent funIOSBuyBack = null;

//	private PurchaseEvent funBuyForIOS = null; 
//	private PurchaseEvent funBuyForAlipay = null;
//	private PurchaseEvent funBuyForWeiXin = null;
//	private PurchaseEvent funForBuyBack = null;

	public object receiptIO = new object ();
	private string receiptPathFile = Application.persistentDataPath + "/PurchaseReceipt.txt";

	private PurchaseInApp purchaseInApp = null;
	private UIStore uiStore = null;

#if UNITY_IPHONE && !UNITY_APP_WEB
    public bool openAllPay = false;
#else
	public bool openAllPay = true;
#endif

    private PurchaseControl ()
	{
	
	}

	~PurchaseControl ()
	{

	}

	//设置购买的产品的币值
	public void SetPurchasePrice(int num, string goldCoin)
	{
		if (null==purchasePrice)
		{
			purchasePrice = new PurchasePrice[6];
		}

		if (num > 5 || num < 0)
			return;
		string pID = "";
		switch(num)
		{
			case 0:
				pID = ProductID.CL_GOLD_6;
				break;
			case 1:
				pID = ProductID.CL_GOLD_18;
				break;
			case 2:
				pID = ProductID.CL_GOLD_50;
				break;
			case 3:
				pID = ProductID.CL_GOLD_98;
				break;
			case 4:
				pID = ProductID.CL_GOLD_198;
				break;
			case 5:
				pID = ProductID.CL_GOLD_488;
				break;
		}
		Debug.LogError("---SetPurchasePrice num:"+num);
		purchasePrice[num] = new PurchasePrice();
		purchasePrice[num].productID = pID;
		purchasePrice[num].goldCoin = goldCoin;
	}

	public string GetPurchasePrice(string pID)
	{
		string goldCoin = "";
		int num = -1;
		switch (pID) 
		{
		case ProductID.CL_GOLD_6:
			num = 0;
			break;
		case ProductID.CL_GOLD_18:
			num = 1;
			break;
		case ProductID.CL_GOLD_50:
			num = 2;
			break;
		case ProductID.CL_GOLD_98:
			num = 3;
			break;
		case ProductID.CL_GOLD_198:
			num = 4;
			break;
		case ProductID.CL_GOLD_488:
			num = 5;
			break;
		}
		if(-1==num || null == purchasePrice)
			return goldCoin;
		goldCoin = purchasePrice[num].goldCoin;
		return goldCoin;
	}

	//设置购买的产品的币值
	public void SetOtherPurchasePrice(int num, string goldCoin)
	{
		if (null==otherPurchasePrice)
		{
			otherPurchasePrice = new OtherPurchasePrice[6];
		}

		if (num > 5 || num < 0)
			return;
		int price = 0;
		switch(num)
		{
			case 0:
				price = 5;
				break;
			case 1:
				price = 10;
				break;
			case 2:
				price = 50;
				break;
			case 3:
				price = 100;
				break;
			case 4:
				price = 200;
				break;
			case 5:
				price = 500;
				break;
		}
		Debug.LogError("---SetPurchasePrice num:"+num);
		otherPurchasePrice[num] = new OtherPurchasePrice();
		otherPurchasePrice[num].price = price;
		otherPurchasePrice[num].goldCoin = goldCoin;
	}

	public string GetOtherPurchasePrice(int price)
	{
		string goldCoin = "";
		int num = -1;
		switch (price) 
		{
		case 5:
			num = 0;
			break;
		case 10:
			num = 1;
			break;
		case 50:
			num = 2;
			break;
		case 100:
			num = 3;
			break;
		case 200:
			num = 4;
			break;
		case 500:
			num = 5;
			break;
		}
		if(-1==num || null == otherPurchasePrice)
			return goldCoin;
		goldCoin = otherPurchasePrice[num].goldCoin;
		return goldCoin;
	}

	public void SetRcv(int type, PurchaseEvent rcv)
	{//设置IOSPurchaseInApp回调
		switch(type)
		{
		case PurchaseMsg.IOS_INIT_BACK:
			{
				if(null!=rcv)
				{
//					Tool.OutLogWithToFile ("PurchaseControl SetRcv IOS_INIT is not null");
				}
				else
				{
//					Tool.OutLogWithToFile ("PurchaseControl SetRcv IOS_INIT is null");
				}
			}
			funIOSInitBack = rcv;
			break;
		case PurchaseMsg.IOS_BUY_BACK:
			{
				if(null!=rcv)
				{
//					Tool.OutLogWithToFile ("PurchaseControl SetRcv IOS_PAY is not null");
				}
				else
				{
//					Tool.OutLogWithToFile ("PurchaseControl SetRcv IOS_PAY is null");
				}
			}
			funIOSBuyBack = rcv;
			break;
//		case PurchaseMsg.BUY_FOR_IOS:
//			funBuyForIOS = rcv;
//			break;
//		case PurchaseMsg.BUY_FOR_IOS:
//			funBuyForAlipay = rcv;
//			break;
//		case PurchaseMsg.BUY_FOR_IOS:
//			funBuyForWeiXin = rcv;
//			break;
//		case PurchaseMsg.FOR_BUY_BACK:
//			funForBuyBack = rcv;
//			break;
		}
	}

	public void CloseWaitingView()
	{
		if (mWaitingView != null) {
			if (mWaitingView.activeSelf) {
				UnityEngine.GameObject.Destroy ( mWaitingView );
			}
		}
		mWaitingView = null;
	}

	public void SetPurchaseInAppObject(PurchaseInApp obj)
	{
		purchaseInApp = obj;
	}

	public void SetUIStore(UIStore obj)
	{
		uiStore = obj;
	}

	public void UpdatePurchaseState()
	{//只有IOS平台需要检测
	#if UNITY_IPHONE && !UNITY_APP_WEB
        if(null!=purchaseInApp)
		{
			//purchaseInApp.UpdatePayState();
		}
 	#endif
		return;
	}

	public void UpdateCheckAppVersion(PurchaseEvent handle)
	{
		if(null!=purchaseInApp)
		{
			//purchaseInApp.RequestCheckAppVersion(handle);
		}
	}

	public GameObject mWaitingView = null;
	#if UNITY_IPHONE && !UNITY_APP_WEB
	public void BuyProductForIOS(int userIdx, string productID)
	{//IOS支付
//		/*#if UNITY_EDITOR
//		iosInitState = 0;
//		#endif
//
//		if (iosInitState != 0) {
//			return;
//		}*/

		if(null!=purchaseInApp)
		{
			Debug.LogError ( "productID:" + productID + " / " + userIdx );
			GameObject Window1 = UnityEngine.Resources.Load ("MainHall/Common/WaitingView")as UnityEngine.GameObject;
			mWaitingView = UnityEngine.GameObject.Instantiate ( Window1 );
			purchaseInApp.BuyProductForIOS (userIdx, productID);
		}
		return;
	}

	public void IOSPurchaseBack(string content)
	{//Json解析消息内容, Json:"{'msg':msg, 'data':'{'result':result, 'data':'data'}'}"
		PurchaseInfo jsonInfo = JsonMapper.ToObject<PurchaseInfo> (content);
		if (null == jsonInfo)
			return;
		PurchaseData jsonData = JsonMapper.ToObject<PurchaseData> (jsonInfo.data);
		if (null == jsonData)
			return;
		switch(jsonInfo.msg)
		{
		case PurchaseMsg.IOS_INIT_BACK:
			{
				if(null!=funIOSInitBack)
				{
					funIOSInitBack.Invoke (jsonData.data, jsonData.result);
				}
				if(0==jsonData.result)
				{//初始化完成
					iosInitState = 0;
				}
				else
				{//初始化失败
					iosInitState = 1;
				}
			}
			break;
		case PurchaseMsg.IOS_BUY_BACK:
			{//做购买记录的保存，如果请求成功，这个删除这条记录，如果失败则提示用户找人工服务进行充值。无法进行订单处理。重新启动客户端进行二次提交

				Debug.LogError ( "[ IOSPurchaseBack ]-------call back-------" + content );

				if(null!=funIOSBuyBack)
				{
					funIOSBuyBack.Invoke (jsonData.data, jsonData.result);
				}
				if (mWaitingView != null) {
					if (mWaitingView.activeSelf) {
						UnityEngine.GameObject.Destroy ( mWaitingView );
					}
				}
				mWaitingView = null;
			}
			break;
		}
	}

//	public void ForIOSBuyBack()
//	{
//		if(null!=uiStore)
//		{
//			uiStore.ForIOSBuyBack ();
//		}
//	}

	#elif UNITY_ANDROID || UNITY_APP_WEB
	public void BuyForAlipay( int nUserId , int nPrice )
	{
		if(null!=purchaseInApp)
		{
			purchaseInApp.BuyForAlipay(nUserId, nPrice);
		}
	}

	public void BuyForWX( int nUserId , int nPrice )
	{
		if(null!=purchaseInApp)
		{
			purchaseInApp.BuyForWX(nUserId, nPrice);
		}
	}

//	public void BuyProductForAlipay(int userIdx, string productID)
//	{//支付宝支付
//		if(null!=purchaseInApp)
//		{
//			purchaseInApp.BuyProductForAlipay (userIdx, productID);
//		}
//		return;
//	}
//
//	public void BuyProductForWX(int userIdx, string productID)
//	{//微信支付
//		if(null!=purchaseInApp)
//		{
//			purchaseInApp.BuyProductForWX (userIdx, productID);
//		}
//		return;
//	}

//	public void ForAlipayBuyBack()
//	{
//		if(null!=uiStore)
//		{
//			uiStore.ForAlipayBuyBack ();
//		}
//	}
//
//	public void ForWXBuyBack()
//	{
//		if(null!=uiStore)
//		{
//			uiStore.ForWXBuyBack ();
//		}
//	}
	#endif

	public void FileWrite(string pathFile, string content, bool removeBefore)
	{//removeBefore:如果文件存在，写内容前是否删除之前的内容，默认是追加内容
		if(removeBefore)
		{
			FileRemove (pathFile);
		}
		FileWrite (pathFile, content);
	}

	public void FileWrite(string pathFile, string content)
	{
		lock(receiptIO)
		{
			//创建文件
			//FileStream fileStream = File.Create (pathFile);
			//FileInfo fileInfo = new FileInfo (pathFile);
			//fileInfo.Create ();
			StreamWriter writer = new StreamWriter (pathFile, true, Encoding.UTF8);
			writer.WriteLine (content);
			writer.Close ();
			writer.Dispose ();
		}
	}

	public string FileRead(string pathFile)
	{//读取文件内容
		string content = "";
		lock(receiptIO)
		{
			if(System.IO.File.Exists(pathFile))
			{
				StreamReader reader = new StreamReader (pathFile);
				//Txt = reader.ReadLine ();
				content = reader.ReadToEnd();
			}
			else
			{
				Tool.Log (pathFile + "不存在");
			}
		}
		return content;
	}

	public void FileRemove(string pathFile)
	{//删除文件
		lock(receiptIO)
		{
			if(System.IO.File.Exists(receiptPathFile))
			{
				File.Delete (receiptPathFile);
			}
		}
	}

	public void SaveReceiptInfo(int userId, int webOrderId, string data)
	{
		string content = "{" + 
			"\"userId\":"+userId+"," +
			"\"webOrderId\":"+webOrderId+"," +
			"\"receiptData\":\""+data+"\"" +
			"}";
//		Tool.OutLogWithToFile ("SaveReceiptInfo:"+content);
		FileWrite (receiptPathFile, content);
	}

	public string ReadReceiptInfo()
	{
		string result = "";
		string content = FileRead(receiptPathFile);
		ReceiptInfo jsonDataInfo = JsonMapper.ToObject<ReceiptInfo> (content);
		if (null == jsonDataInfo)
			return result;
//		Tool.OutLogWithToFile ("ReadReceiptInfo userId:"+jsonDataInfo.userId+
//			" webOrderId:"+jsonDataInfo.webOrderId+
//			" receiptData:"+jsonDataInfo.receiptData);
		
		result = jsonDataInfo.receiptData;
		return result;
	}

	public void RemoveReceiptInfo()
	{
		FileRemove (receiptPathFile);
	}
}
