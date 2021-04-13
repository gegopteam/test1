using System.Collections;
using System.Collections.Generic;
using AssemblyCSharp;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// 复制码
/// </summary>
public class CopyCode:MonoBehaviour,IMsgHandle
{
	#if UNITY_IPHONE
    [System.Runtime.InteropServices.DllImport("__Internal")]
    private static extern void SetCopyCode_(string str);
    [System.Runtime.InteropServices.DllImport("__Internal")]
    private static extern void GetCopyCode_();
    [System.Runtime.InteropServices.DllImport("__Internal")]
    private static extern void WeixinOpen();
#endif

	//private  AndroidJavaClass jc;
	//private  AndroidJavaObject jo;

	private static CopyCode intance;

	private void Awake ()
	{
		intance = this;
	}

	public Text textCode;

	private void Start ()
	{
		//AndroidJavaClass jc = new AndroidJavaClass("com.unity3d.player.UnityPlay");
		//AndroidJavaObject jo = jc.GetStatic<AndroidJavaObject>("currentActivity");
		//ReceiveIsBindPhoneMassage(new FiIsBindPhoneResponse());
	}

	/// <summary>
	/// 初始化
	/// </summary>
	public void OnInit ()
	{
		EventControl eventControl = EventControl.instance ();
		eventControl.addEventHandler (FiEventType.RECV_BIND_PHONE_STATE_RESPONSE, ReceiveIsBindPhoneMassage);

	}

	/// <summary>
	/// 是否绑定手机号
	/// </summary>
	/// <param name="data">数据.</param>
	public void ReceiveIsBindPhoneMassage (object data)
	{
		//Debug.Log("1234556777=======================");
		FiIsBindPhoneResponse result = (FiIsBindPhoneResponse)data;
		//Debug.Log("ReceiveIsBindPhoneMassage---------" + result.isBindPhone);

		//BackpackInfo nBackInfo = (BackpackInfo)Facade.GetFacade().data.Get(FacadeConfig.BACKPACK_MODULE_ID);
		MyInfo nUserInfo = (MyInfo)Facade.GetFacade ().data.Get (FacadeConfig.USERINFO_MODULE_ID);
		nUserInfo.isBindPhone = result.isBindPhone;
		nUserInfo.strPhoneNum = result.strPhoneNum;
		if (nUserInfo.isBindPhone == 1) {
			//Debug.Log("nUserInfo.isBindPhone==1-------" + nUserInfo.isBindPhone);
		}
	}

	/// <summary>
	/// 获取绑定码
	/// </summary>
	public  void GetCopyCode (Text textNumber)
	{
#if UNITY_ANDROID
		AndroidJavaClass jc = new AndroidJavaClass ("com.unity3d.player.UnityPlayer");
		AndroidJavaObject jo = jc.GetStatic<AndroidJavaObject> ("currentActivity");
		if (Application.platform == RuntimePlatform.Android) {
			//Debug.Log("GetCopyCode:" + "获取绑定码");
			jo.Call ("GetCopyCode", textCode.text);
			OffLogin ();
		}
#endif
#if UNITY_IPHONE
        if(Application.platform == RuntimePlatform.IPhonePlayer)
            {
                SetCopyCode_(textCode.text);
                WeixinOpen();
                OffLogin();
            }
		#endif          
	}

	/// <summary>
	/// 删除
	/// </summary>
	public void OnDestroy ()
	{
		EventControl eventControl = EventControl.instance ();
		eventControl.removeEventHandler (FiEventType.RECV_BIND_PHONE_STATE_RESPONSE, ReceiveIsBindPhoneMassage);
	}


	public void OffLogin ()
	{
		//Debug.Log("1232345456767---------------");
		AudioManager._instance.PlayEffectClip (AudioManager.ui_click);
		TaskMsgHandle.DestoryList ();
		LoginUtil.GetIntance ().CancelAuthorize ();
		AppControl.ToView (AppView.LOGIN);
	}
}
