using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using AssemblyCSharp;
using System.Runtime.InteropServices;

/// <summary>
/// 手机绑定功能
/// </summary>
public class UIPhoneBind : MonoBehaviour, IMsgHandle
{
	string key = "72FAWaNZdkl4t65EdvMVCZApcy1UlmlL";

	string userid;

	string token;

	public InputField TxtTelNum;

	public InputField TxtCode;

	public InputField TxtPasW;

	public GameObject BtnGetCode;

	public Button BtnSure;

	public Button BtnEsc;

	public Button BtnExit;

	public Text TimeCode;

	public Text text;
	string MD5Pasw;
	MyInfo myInfo;
	//获取提示弹窗
	GameObject Window1;
	public static UIPhoneBind Instance;

	string GetAskString()
	{
		string nSign = Tool.GetMD532(userid + TxtTelNum.text + key).ToLower();

		//		return "http://mobile.cl0579.com/api/api_YKBind.aspx?type=3&uid=" + userid + "&phone=" + TxtTelNum.text + "&md5=" + nSign;

		return UIUpdate.WebUrlDic[WebUrlEnum.RegisterWithPhone] + "?type=3&uid=" + userid + "&phone=" + TxtTelNum.text + "&md5=" + nSign;

		//string nSign2 = FormsAuthentication.HashPasswordForStoringInConfigFile(userid + TxtTelNum.text + key, "md5").ToLower();
		//      http://mobile.cl0579.com/api/api_YKBind.aspx?type=3&uid=100158&phone=13817203292&type=3&md5=e6c7b240117d9572d5d88134617119e2
		//      http://mobile.cl0579.com/api/api_YKBind.aspx?type=3&uid=14754410&phone=18837922972&type=3&md5=d46f92fcd8ca1be86b3b49eec254f2c7
		//      http://mobile.cl0579.com/api/api_YKBind.aspx?type=3&uid=14754410&phone=18837922973&type=3&md5=f969cb8743201f355e377dde5efe8bde
	}

	void Awake()
	{
		Instance = this;
	}

	// Use this for initialization
	void Start()
	{
		Window1 = UnityEngine.Resources.Load("Window/WindowTipsThree") as UnityEngine.GameObject;
		//userid = DataControl.GetInstance ().GetMyInfo ().account;
		//MyInfo myInfo = DataControl.GetInstance().GetMyInfo();

		myInfo = (MyInfo)Facade.GetFacade().data.Get(FacadeConfig.USERINFO_MODULE_ID);

		userid = myInfo.loginInfo.userId.ToString();
		token = myInfo.mLoginData.token;
		//token = "aaaaa";
		//TxtTelNum.text = "18837922972";

		Debug.Log("获取到的UserId为:" + userid);
		Debug.Log("获取到的token为:" + token);

		ShowBtnCode(true);

		OnInit();
		//button事件
		BtnGetCode.GetComponent<Button>().onClick.AddListener(OnSendVerifyNumber);
		BtnSure.onClick.AddListener(OnBind);
		BtnEsc.onClick.AddListener(OnClose);
		BtnExit.onClick.AddListener(OnClose);
	}

	public void OnInit()
	{
		EventControl nControl = EventControl.instance();
		nControl.addEventHandler(FiEventType.RECV_CONVERSION_REQUEST, RecvBackBindPhone);
	}

	/// <summary>
	/// 获取验证码
	/// </summary>
	/// <returns>The code.</returns>
	/// <param name="url">URL.</param>
	IEnumerator GetCode(string url)
	{
		Debug.LogError("url : " + url);
		WWW www = new WWW(url);
		yield return www;
		if (www.error != null)
		{
			Debug.LogError("VerifyServerVersion1====>:" + www.error);
			ShowBtnCode(true);
			Debug.Log("驗證碼獲取失敗！");
		}
		else
		{
			Debug.LogError(" [ get code success ] " + www.text);
			//			string nResult = www.text.Substring ( 0 , 1 );
			//            Debug.Log(nResult);
			//			if (nResult.Equals ("1")) 
			//            {
			////				string nToken = nResult.Substring (2, nResult.Length - 2);
			//}
			//         else
			//         {
			//	ShowBtnCode ( true );
			//             Debug.Log("验证码获取错误！");
			//}
		}
	}

	/// <summary>
	/// 获取验证码发送
	/// </summary>
	public void OnSendVerifyNumber()
	{
		AudioManager._instance.PlayEffectClip(AudioManager.ui_click);

		if (string.IsNullOrEmpty(TxtTelNum.text) || TxtTelNum.text.Length != 11)
		{
			GameObject WindowClone1 = Instantiate(Window1);
			UITipAutoNoMask ClickTips1 = WindowClone1.GetComponent<UITipAutoNoMask>();
			ClickTips1.tipText.text = "請輸入正確的手機號";
			return;
		}
		ShowBtnCode(false);
		StartCoroutine(GetCode(GetAskString()));
	}


	/// <summary>
	/// 显示验证码或按钮
	/// </summary>
	/// <param name="value">验证码按钮二选一</param>
	void ShowBtnCode(bool value)
	{
		BtnGetCode.SetActive(value);
		TimeCode.gameObject.SetActive(!value);
		ShowText(value);
		nRemainDuration = 90.0f;
	}


	public void OnBind()
	{
		AudioManager._instance.PlayEffectClip(AudioManager.ui_click);

		if (string.IsNullOrEmpty(TxtTelNum.text))
		{
			GameObject WindowClone1 = Instantiate(Window1);
			UITipAutoNoMask ClickTips1 = WindowClone1.GetComponent<UITipAutoNoMask>();
			ClickTips1.tipText.text = "請輸入手機號";
			return;
		}
		if (string.IsNullOrEmpty(TxtCode.text))
		{
			GameObject WindowClone1 = Instantiate(Window1);
			UITipAutoNoMask ClickTips1 = WindowClone1.GetComponent<UITipAutoNoMask>();
			ClickTips1.tipText.text = "請輸入驗證碼";
			return;
		}
		if (string.IsNullOrEmpty(TxtPasW.text))
		{
			GameObject WindowClone1 = Instantiate(Window1);
			UITipAutoNoMask ClickTips1 = WindowClone1.GetComponent<UITipAutoNoMask>();
			ClickTips1.tipText.text = "請輸入密碼";
			return;
		}

		//MD5登录密码
		MD5Pasw = Tool.GetMD532(TxtPasW.text).ToLower();

		Debug.Log("加密密码=" + MD5Pasw);
		Debug.Log("正在发送！");
		//Bind(TxtTelNum.text,TxtPasW.text,int.Parse(userid),int.Parse(TxtCode.text),token);
		Bind(TxtTelNum.text, MD5Pasw, int.Parse(userid), TxtCode.text, token);
		Debug.Log("已发送！");

	}

	/// <summary>
	/// 向服务器发送数据
	/// </summary>
	/// <returns>The bind.</returns>
	/// <param name="moble">Moble.</param>
	/// <param name="pasw">Pasw.</param>
	/// <param name="Userid">Userid.</param>
	/// <param name="code">Code.</param>
	/// <param name="Token">Token.</param>
	void Bind(string moble, string pasw, int Userid, string code, string Token)
	{
		ShowWaitingView(true);  //转圈等待

		//		Debug.Log ("电话号: " + TxtTelNum.text);
		//		Debug.Log ("密码: " + TxtPasW.text);
		//		Debug.Log ("UID: " + userid);
		//		Debug.Log ("CODE: " + TxtCode.text);
		//		Debug.Log ("TOKEN: " + token);

		FiConvertFormalAccount nRequest = new FiConvertFormalAccount();
		nRequest.code = code;
		nRequest.mobile = moble;
		nRequest.pwd = pasw;
		nRequest.token = Token;
		nRequest.userID = Userid;

		DispatchData nData = new DispatchData();
		//nData.type = FiEventType.SEND_CONVERSION_REQUEST;
		//nData.data = nRequest;
		//DataControl.GetInstance().PushSocketSnd(nData.type,nRequest);
		DataControl.GetInstance().PushSocketSnd(FiEventType.SEND_CONVERSION_REQUEST, nRequest);

		Debug.Log("正在进行绑定！");

	}

	/// <summary>
	/// 回调
	/// </summary>
	/// <param name="data">Data.</param>
	private void RecvBackBindPhone(object data)
	{
		Debug.Log("接收回调");

		FiSystemReward info = (FiSystemReward)data;
		UIBindSuccess.nResponse = info;


		Debug.Log("---回调------resultCode: " + info.resultCode + ", propID：" + info.propID + ", propCount: " + info.propCount + ", msg: " + info.msg);

		if (info.resultCode == 0)
		{    //手机绑定成功
			
			ShowWaitingView(false); //关闭转圈等待

			//显示成功信息
			GameObject WindowClone1 = Instantiate(Window1);
			UITipAutoNoMask ClickTips1 = WindowClone1.GetComponent<UITipAutoNoMask>();
			if (info.msg != "")
			{
				ClickTips1.tipText.text = info.msg;
			}
			else
			{
				ClickTips1.tipText.text = "綁定成功！";
			}

			myInfo.isBindPhone = 1;
			UIHallCore.Instance.SetupLeftControl(); 
			Debug.Log("綁定成功！");

			//关闭当前弹窗
			BindWindowCtrl.Instense.GenerateWindow_Close();
			//弹出绑定成功弹窗
			GameObject window = BindWindowCtrl.Instense.BindSuccessWindow();

			myInfo.isGuestLogin = false;
			#region 清除游客
#if UNITY_IPHONE && !UNITY_EDITOR
			ToSetGuestLogin ();
			//CleanGuestLoginCache();
#elif UNITY_ANDROID && !UNITY_EDITOR
			UpdateGuestLoginState ();
#endif
			#endregion
			UIBindSuccess.propID = info.propID;
			UIBindSuccess.propCount = info.propCount;
			UIBindSuccess.TelNum = TxtTelNum.text;
			UIBindSuccess.PasW = TxtPasW.text;
			myInfo.password = MD5Pasw;
			Debug.LogError("TxtTelNum.text = " + TxtTelNum.text);
			myInfo.account = TxtTelNum.text;
			Debug.LogError("myInfo.account = " + myInfo.account);


		}
		else if (info.resultCode == -2)
		{   //绑定失败
			ShowWaitingView(false); //关闭转圈等待

			////显示失败信息
			GameObject WindowClone1 = Instantiate(Window1);
			UITipAutoNoMask ClickTips1 = WindowClone1.GetComponent<UITipAutoNoMask>();
			if (info.msg != "")
			{
				ClickTips1.tipText.text = info.msg;
			}
			else
			{
				ClickTips1.tipText.text = "該手機號已被使用";
			}
			Debug.Log("該手機號已被使用！");
			return;
		}
		else if (info.resultCode == -3)
		{   //绑定失败
			ShowWaitingView(false); //关闭转圈等待

			////显示失败信息
			GameObject WindowClone1 = Instantiate(Window1);
			UITipAutoNoMask ClickTips1 = WindowClone1.GetComponent<UITipAutoNoMask>();
			if (info.msg != "")
			{
				ClickTips1.tipText.text = info.msg;
			}
			else
			{
				ClickTips1.tipText.text = "驗證碼錯誤";
			}
			Debug.Log("驗證碼錯誤！");
			return;
		}
		else if (info.resultCode == -16)
		{
			ShowWaitingView(false); //关闭转圈等待

			////显示失败信息
			GameObject WindowClone1 = Instantiate(Window1);
			UITipAutoNoMask ClickTips1 = WindowClone1.GetComponent<UITipAutoNoMask>();
			if (info.msg != "")
			{
				ClickTips1.tipText.text = info.msg;
			}
			else
			{
				ClickTips1.tipText.text = "您的手機號碼已達綁定上限";
			}
			Debug.Log("您的手機號碼已達綁定上限！");
			return;
		}
		else
		{
			ShowWaitingView(false);
			////显示失败信息
			GameObject WindowClone1 = Instantiate(Window1);
			UITipAutoNoMask ClickTips1 = WindowClone1.GetComponent<UITipAutoNoMask>();
			if (info.msg != "")
			{
				ClickTips1.tipText.text = info.msg;
			}
			else
			{
				ClickTips1.tipText.text = "綁定失敗，請檢查輸入信息是否有誤";
			}
			Debug.Log("綁定失敗，請檢查輸入信息是否有誤！");
			return;
		}
	}

	/// <summary>
	/// 绑定时等待
	/// </summary>
	GameObject mWaitingView;

	public void ShowWaitingView(bool nValue)
	{
		if (nValue)
		{
			mWaitingView = Instantiate(Resources.Load<GameObject>("MainHall/Common/WaitingView"));
			UIWaiting nData = mWaitingView.GetComponentInChildren<UIWaiting>();
			nData.HideBackGround();
			nData.SetInfo(15.0f, "綁定失敗，請重新嘗試");
		}
		else
		{
			if (mWaitingView != null && mWaitingView.activeSelf)
			{
				Destroy(mWaitingView);
			}
			mWaitingView = null;
		}
	}

	void ShowText(bool value)
	{
		if (value)
			text.text = "帳號升級後綁定手機號為登錄帳號，可以通過帳號登錄按鈕進入遊戲，帳號信息永久保留。";
		else
			text.text = "驗證碼已發送，請在90秒內輸入";
	}

	public void OnClose()
	{
		AudioManager._instance.PlayEffectClip(AudioManager.effect_closePanel);
		//DataControl.GetInstance ().GetMyInfo ().account = null;
		Destroy(gameObject);
	}



	float nRemainDuration = 90.0f;

	void Update()
	{
		if (TimeCode.gameObject.activeSelf)
		{
			nRemainDuration -= Time.deltaTime;
			if (nRemainDuration <= 0)
			{
				nRemainDuration = 90.0f;
				ShowBtnCode(true);
			}
			else
			{
				TimeCode.text = "<color=red>" + ((int)nRemainDuration).ToString() + "</color>" + "秒後可以重新接收短信驗證碼";
			}
		}
	}

	public void OnDestroy()
	{
		EventControl nControl = EventControl.instance();
		nControl.removeEventHandler(FiEventType.RECV_CONVERSION_REQUEST, RecvBackBindPhone);
	}

	/// <summary>
	/// android更改游客状态
	/// </summary>
	void UpdateGuestLoginState()
	{
		AndroidJavaClass jc = new AndroidJavaClass("com.unity3d.player.UnityPlayer");
		AndroidJavaObject jo = jc.GetStatic<AndroidJavaObject>("currentActivity");
		jo.Call("setGuestLogin");
	}

#if UNITY_IOS
	/// <summary>
	/// ios更改游客状态
	/// </summary>
	/// <returns><c>true</c>, if set guest login was toed, <c>false</c> otherwise.</returns>
	[DllImport("__Internal")]
	public static extern bool ToSetGuestLogin();
#endif
}

