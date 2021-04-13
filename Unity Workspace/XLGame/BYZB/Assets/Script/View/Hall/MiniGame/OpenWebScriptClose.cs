using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using AssemblyCSharp;
using System;

public class OpenWebScriptClose : MonoBehaviour
{
	/// <summary>
	/// web调用窗口
	/// </summary>
	private static WebViewObject webViewObject;
	public static OpenWebScriptClose Instance;

	void Awake ()
	{
		//webViewObject.SetVisibility (0, 0, 0, 0);
		Instance = this;
		webViewObject = gameObject.AddComponent<WebViewObject> ();
		#if UNITY_ANDROID
		webViewObject.SetMargins (0, 0, 0, 0);
		#elif UNITY_IPHONE 
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
		
		//web的回调
		LoginUtil.ShowWaitingMask(true);
		webViewObject.Init(cb: (msg) => {
			AppControl.miniGameState = true;
			Debug.Log("SetActivityWebURL = msg = " + msg);
			if (msg != null && msg == "AppType=WebMiniGame&msg=out")
			{
				if (UIHallCore.Instance != null)
					UIHallCore.Instance.DestoryWebObj(this.gameObject);
			}
		}, ld: (msg) => {
			{
				//Debug.Log (string.Format ("CallOnLoaded[{0}]", msg));
#if UNITY_EDITOR_OSX || !UNITY_ANDROID
				// NOTE: depending on the situation, you might prefer
				// the 'iframe' approach.
				// cf. https://github.com/gree/unity-webview/issues/189
#if true
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
				webViewObject.EvaluateJS(@"Unity.call('ua=' + navigator.userAgent)");
			}
		}, enableWKWebView: true);
		if (Facade.GetFacade().config.isIphoneX2())
		{
			webViewObject.SetCenterPositionWithScale(new Vector2(0, 0), new Vector2(2308, 1125));
		}
		webViewObject.SetVisibility(true);
		webViewObject.LoadURL(url);
	}

	public void ClickCloseBtn ()
	{
		AudioManager._instance.PlayEffectClip (AudioManager.ui_click);
		Destroy (this.gameObject); 
	}

}
