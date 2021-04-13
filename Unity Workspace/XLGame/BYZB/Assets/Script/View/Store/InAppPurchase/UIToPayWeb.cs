using UnityEngine;
using System.Collections;
using AssemblyCSharp;

public class UIToPayWeb : MonoBehaviour
{
	WebViewObject webViewObject = null;
	string url;
	public static UIToPayWeb webPayInstance = null;

	// Use this for initialization
	void Start ()
	{
		webPayInstance = this;
	}
	
	// Update is called once per frame
	void Update ()
	{
	
	}

	public WebViewObject OpenWebView (string n_url)
	{
		Debug.LogError ("OpenWebView___________");
		url = n_url;
		if (null == webViewObject) {
			webViewObject = (new GameObject ("WebViewObject")).AddComponent<WebViewObject> ();
			webViewObject.Init (
				cb: (msg) => {

					//msg = "0&trade_no:2018030817571263228732";

					Debug.LogError ("----log for webpay callback---- " + msg);
					try {
						int nIndex = msg.IndexOf ("&");
						if (nIndex == 0)
							nIndex = 1;
						string nPayResult = msg.Substring (0, nIndex);
						int nResult = int.Parse (nPayResult);
						//解析出来是数字，那么说明是 支付宝支付的返回了
						//Debug.LogError ("----log for webpay nResult---- " + nResult);
						if (nResult != 0) {
							//ClickTips1.tipText.text = "支付失败";
							//CloseWebView();   
//                        GameObject PayFileWindow = UnityEngine.Resources.Load("Window/WindowTipPayFail") as UnityEngine.GameObject;
//                        GameObject PayFileWindow1 = UnityEngine.GameObject.Instantiate(PayFileWindow);
//                            Debug.Log("支付失败");
							UIToPay.DarGonCardType = 0;

						} else {
							CloseWebView ();
							GameObject Window1 = UnityEngine.Resources.Load ("Window/WindowTipsPaySuccess")as UnityEngine.GameObject;
							GameObject WindowClone1 = UnityEngine.GameObject.Instantiate (Window1);
							UITipAutoNoMask ClickTips1 = WindowClone1.GetComponent<UITipAutoNoMask> ();
							ClickTips1.tipText.text = "支付成功";
						}

						MyInfo nUserInfo = (MyInfo)Facade.GetFacade ().data.Get (FacadeConfig.USERINFO_MODULE_ID);
						int nIndexTrade = msg.IndexOf ("trade_no:") + 9;
						int nRemainLen = msg.Length - nIndexTrade;
						if (nRemainLen > 0) {
							string nTradeNum = msg.Substring (nIndexTrade, nRemainLen);
							nUserInfo.bAfterCharge = true;
							Debug.LogError ("----nTradeNum----" + nTradeNum);
							if (nUserInfo.loginInfo.preferencePackBought == 0) {
								Facade.GetFacade ().message.toolPruchase.SendPayStateRequest (nTradeNum);
							}
						}

					} catch {
						
					}
					Invoke ("ClosePage", 1.5f);
				}
			);
//            // webViewObject.SetMargins (5, 10, 5, Screen.height / 5);
			webViewObject.SetMargins (0, 100, 0, 0);
		}
	
		webViewObject.SetVisibility (true);
		webViewObject.LoadURL (url);
		// Tool.LogError("OpenWebView");
		return webViewObject;
	}

	void ClosePage ()
	{
		Back (gameObject);
	}

	public void Back (GameObject obj)
	{
		UIToPay.DarGonCardType = 0;
		CloseWebView ();
		DestroyObject (obj);
		webPayInstance = null;
		Facade.GetFacade ().message.backpack.SendReloadRequest ();
		Facade.GetFacade ().message.toolPruchase.ShowTopUpWinodw ();
	}

	public void CloseWebView ()
	{
		if (webViewObject != null) {
			Destroy (webViewObject.gameObject);
		}
		webViewObject = null;
	}
}
