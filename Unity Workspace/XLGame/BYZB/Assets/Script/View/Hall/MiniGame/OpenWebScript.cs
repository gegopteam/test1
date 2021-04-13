using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using AssemblyCSharp;
using System;

public class OpenWebScript : MonoBehaviour
{
	/// <summary>
	/// web调用窗口
	/// </summary>
	private static WebViewObject webViewObject;
	public static OpenWebScript Instance;
	public static string web_temp = "";
    private bool IsIphoneXDevice = false;

	void Awake ()
	{
		//webViewObject.SetVisibility (0, 0, 0, 0);
		Instance = this;
		webViewObject = gameObject.AddComponent<WebViewObject> ();
		#if UNITY_ANDROID
		webViewObject.SetMargins (0, 0, 0, 0);
#elif UNITY_IPHONE
        //IsIphoneXDevice = modelStr.Equals("iPhone11,8"));
        //if (IsIphoneXDevice)
        //{
        //    webViewObject.SetMargins (-322, -148, -322, -148);
        //}else
        //{
        //    webViewObject.SetMargins (0, 0, 0, 0);
        //}
        webViewObject.SetMargins (0, 0, 0, 0);
#elif UNITY_STANDALONE_WIN || UNITY_EDITOR
		webViewObject.SetMargins  (0, 0, 0, 0);
#else
		webViewObject.SetMargins  (0, 0, 0, 0);
#endif
	}

	/// <summary>
	/// 设置web网页
	/// </summary>
	/// <param name="url">URL.</param>
	public void SetActivityWebUrl (string url)
	{
		Debug.Log("SetActivityWebURL = url = " + url);
		if (url.Equals(UIUpdate.WebUrlDic[WebUrlEnum.OnlineCostomerService])) {
			web_temp = "OnlineCostomerService";
		}
		//web的回调
		LoginUtil.ShowWaitingMask (true);
		webViewObject.Init (cb: (msg) => {
			AppControl.miniGameState = true;
			Debug.Log("SetActivityWebURL = msg = " + msg);
			if (msg != null && msg == "AppType=WebMiniGame&msg=out") {
				if (UIHallCore.Instance != null)
				{
					if (web_temp.Equals("OnlineCostomerService"))
					{
						Screen.orientation = ScreenOrientation.LandscapeLeft;
						//Screen.autorotateToLandscapeLeft = true;
						//Screen.autorotateToLandscapeRight = true;
						//Screen.autorotateToPortrait = false;
						//Screen.autorotateToPortraitUpsideDown = false;
						UIHallCore.Instance.DestoryWebObj(this.gameObject);
					}
					else {
						UIHallCore.Instance.DestoryWebObj(this.gameObject);
					}
                    
				}
			}
			
		}, ld: (msg) => {
			{
				//Debug.Log (string.Format ("CallOnLoaded[{0}]", msg));
				#if UNITY_EDITOR_OSX || !UNITY_ANDROID
				// NOTE: depending on the situation, you might prefer
				// the 'iframe' approach.
				// cf. https://github.com/gree/unity-webview/issues/189
				#if true
				webViewObject.EvaluateJS (@"
		if (window && window.webkit && window.webkit.messageHandlers && window.webkit.messageHandlers.unityControl) {
		window.Unity = {
		call: function(msg) {
		window.webkit.messageHandlers.unityControl.postMessage(msg);
		}
		}
		} else {
		window.Unity = {
		call: function(msg) {
		window.location = 'unity:' + msg;
		}
		}
		}
		");
				#else
		webViewObject.EvaluateJS(@"
		if (window && window.webkit && window.webkit.messageHandlers && window.webkit.messageHandlers.unityControl) {
		window.Unity = {
		call: function(msg) {
		window.webkit.messageHandlers.unityControl.postMessage(msg);
		}
		}
		} else {
		window.Unity = {
		call: function(msg) {
		var iframe = document.createElement('IFRAME');
		iframe.setAttribute('src', 'unity:' + msg);
		document.documentElement.appendChild(iframe);
		iframe.parentNode.removeChild(iframe);
		iframe = null;
		}
		}
		}
		");
				#endif
				#endif
				webViewObject.EvaluateJS (@"Unity.call('ua=' + navigator.userAgent)");
			}
		}, enableWKWebView: true);
		if (Facade.GetFacade ().config.isIphoneX2 ()) {
			webViewObject.SetCenterPositionWithScale (new Vector2 (0, 0), new Vector2 (2308, 1125));
			webViewObject.SetCenterPositionWithScale(new Vector2(0, 0), new Vector2(100, 100));
		}
		webViewObject.SetVisibility (true);
		webViewObject.LoadURL (url);
	}

	public void ClickCloseBtn ()
	{
		AudioManager._instance.PlayEffectClip (AudioManager.ui_click);
		Destroy (this.gameObject); 
	}

}
