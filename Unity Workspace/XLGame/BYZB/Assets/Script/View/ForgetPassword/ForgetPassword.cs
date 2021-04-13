using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ForgetPassword : MonoBehaviour {


    #if UNITY_IPHONE
    [System.Runtime.InteropServices.DllImport("__Internal")]
    private static extern void SetCopyCode_(string str);
    [System.Runtime.InteropServices.DllImport("__Internal")]
    private static extern void GetCopyCode_();
    [System.Runtime.InteropServices.DllImport("__Internal")]
    private static extern void WeixinOpen();
    #endif

    //private static WebViewObject _webViewObject;
	private void Awake()
	{
        //_webViewObject = gameObject.AddComponent<WebViewObject>();
	}

    /// <summary>
    /// 打开微信
    /// </summary>
    public void OpneWechatClick()
    {
        #if UNITY_ANDROID
        AndroidJavaClass jc = new AndroidJavaClass("com.unity3d.player.UnityPlayer");
        AndroidJavaObject jo = jc.GetStatic<AndroidJavaObject>("currentActivity");
        if (Application.platform == RuntimePlatform.Android)
        {
            jo.Call("SnedWeChatAuth");
            Debug.Log("GetCopyCode:" + "获取绑定码");
            UISetting.Instance.LogOff();
        }
#endif
#if UNITY_IPHONE
            if(Application.platform == RuntimePlatform.IPhonePlayer)
            {
                WeixinOpen();
                UISetting.Instance.LogOff();
            }
#endif
    }
    public void Close()
    {
        Destroy(gameObject);
    }
}
