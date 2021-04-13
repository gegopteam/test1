using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using AssemblyCSharp;
using System.Runtime.InteropServices;
using System;

public class UIBindAccount : MonoBehaviour, IMsgHandle
{

	string key = "72FAWaNZdkl4t65EdvMVCZApcy1UlmlL";
	//string key = "72FAWaNZdkl4t65EdvMVCZApcy1UlmlL";

	string userid = "";

	string iUserid = "";

	string accountName = "";

	public InputField TxtTelNum;

	public InputField TxtCode;

	//public GameObject BtnGetCode;

	public Button BtnSure;

	public Button BtnCode;

	public Button BtnExit;

	public Text TimeCode;

	MyInfo myInfo;
	GameObject Window1;
	GameObject mWaitingView;
	float nRemainDuration = 60.0f;
	public GameObject myself;

	public static UIBindAccount Instance;



	void Awake()
	{
		Instance = this;
		UIColseManage.instance.ShowUI(this.gameObject);
	}

	// Use this for initialization
	void Start()
	{
		Window1 = UnityEngine.Resources.Load("Window/WindowTipsThree") as UnityEngine.GameObject;
		myInfo = (MyInfo)Facade.GetFacade().data.Get(FacadeConfig.USERINFO_MODULE_ID);

		//userid = myInfo.loginInfo.userId.ToString();
		//userid = myInfo.account;

		iUserid = myInfo.loginInfo.userId.ToString();
		//userid = myInfo.mLoginData.username;
		accountName = myInfo.account;



		Debug.Log("获取到的 accountName 为:" + accountName);
		Debug.Log("获取到的 iUserid 为:" + iUserid);
		OnInit();

		//BtnGetCode.GetComponent<Button>().onClick.AddListener(OnSendVerifyNumber);
		//BtnCode.onClick.AddListener(OnSendVerifyNumber);
		//BtnSure.onClick.AddListener(OnBind);
		//BtnExit.onClick.AddListener(OnClose);
	}

	public void OnInit()
	{
		EventControl nControl = EventControl.instance();
		nControl.addEventHandler(FiEventType.RECV_XL_GET_BIND_PHONE_RESPONSE, RecvBackBindPhone);
	}

	// Update is called once per frame
	void Update()
	{
		if (TimeCode.gameObject.activeSelf)
		{
			nRemainDuration -= Time.deltaTime;
			if (nRemainDuration <= 0)
			{
				nRemainDuration = 60.0f;
				ShowBtnCode(true);
			}
			else
			{
				TimeCode.text = "<color=red>" + ((int)nRemainDuration).ToString() + "</color>" + "秒後可以重新接收短信驗證碼";
			}
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

	string GetAskString()
	{
		//string nSign = Tool.GetMD532(userid + TxtTelNum.text + key).ToLower();
		string nSign = Tool.GetMD532(TxtTelNum.text + key).ToLower();

		//return "http://mobile.cl0579.com/api/api_YKBind.aspx?type=3&uid=" + userid + "&phone=" + TxtTelNum.text + "&md5=" + nSign;

		//return UIUpdate.WebUrlDic[WebUrlEnum.RegisterWithPhone] + "?type=3&uid=" + iUserid + "&phone=" + TxtTelNum.text + "&md5=" + nSign;
		return UIUpdate.WebUrlDic[WebUrlEnum.MMSAuth] + "?type=1&account=&uid=" + iUserid + "&phone=" + TxtTelNum.text + "&md5=" + nSign;
		//string nSign2 = FormsAuthentication.HashPasswordForStoringInConfigFile(userid + TxtTelNum.text + key, "md5").ToLower();
		//      http://mobile.cl0579.com/api/api_YKBind.aspx?type=3&uid=100158&phone=13817203292&type=3&md5=e6c7b240117d9572d5d88134617119e2
		//      http://mobile.cl0579.com/api/api_YKBind.aspx?type=3&uid=14754410&phone=18837922972&type=3&md5=d46f92fcd8ca1be86b3b49eec254f2c7
		//      http://mobile.cl0579.com/api/api_YKBind.aspx?type=3&uid=14754410&phone=18837922973&type=3&md5=f969cb8743201f355e377dde5efe8bde
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

			GameObject WindowClone1 = Instantiate(Window1);
			UITipAutoNoMask ClickTips1 = WindowClone1.GetComponent<UITipAutoNoMask>();
			ClickTips1.tipText.text = "驗證碼獲取失敗！";
			Debug.Log("驗證碼獲取失敗！");
		}
		else
		{
			Debug.LogError(" [ get code success ] " + www.text);
			Debug.LogError(" [ get code success ] " + www.text.Length);
			string nCode = www.text.Substring(9, 1);
			if (nCode.Equals("1"))
			{
				nCode = www.text.Substring(0, www.text.Length - 2);
				Debug.LogError(" [ get msg ] " + nCode);
			}
			else
			{
				nCode = www.text.Substring(10, 1);
				Debug.LogError(" [ get msg ] " + nCode);
				if (nCode.Equals("1"))
				{
					ShowAuthMsg("提交失敗");
					//GameObject WindowClone1 = Instantiate(Window1);
					//UITipAutoNoMask ClickTips1 = WindowClone1.GetComponent<UITipAutoNoMask>();
					//ClickTips1.tipText.text = "提交失败";
				}
				else if (nCode.Equals("2"))
				{
					ShowAuthMsg("請在90秒之後再獲取");
					//GameObject WindowClone1 = Instantiate(Window1);
					//UITipAutoNoMask ClickTips1 = WindowClone1.GetComponent<UITipAutoNoMask>();
					//ClickTips1.tipText.text = "请在五分钟之后再获取";
				}
				else if (nCode.Equals("3"))
				{
					ShowAuthMsg("當前帳戶當日短信已達到上限");
					//GameObject WindowClone1 = Instantiate(Window1);
					//UITipAutoNoMask ClickTips1 = WindowClone1.GetComponent<UITipAutoNoMask>();
					//ClickTips1.tipText.text = "当前账户当日短信已达到上限";
				}


			}

			//         string nResult = www.text.Substring(0, 1);
			//         Debug.Log("nResult = " + nResult);
			//         if (nResult.Equals("1"))
			//         {
			//             string nToken = nResult.Substring(2, nResult.Length - 2);
			//	GameObject WindowClone1 = Instantiate(Window1);
			//	UITipAutoNoMask ClickTips1 = WindowClone1.GetComponent<UITipAutoNoMask>();
			//	ClickTips1.tipText.text = nToken;
			//	Debug.Log("nToken = " + nToken);
			//}
			//         else
			//         {
			//	//ShowBtnCode(true);
			//	Debug.Log("验证码获取错误！");
			//	string nToken = nResult.Substring(2, nResult.Length - 2);
			//	GameObject WindowClone1 = Instantiate(Window1);
			//	UITipAutoNoMask ClickTips1 = WindowClone1.GetComponent<UITipAutoNoMask>();
			//	ClickTips1.tipText.text = nToken;
			//	Debug.Log("nToken = " + nToken);
			//         }
		}
	}

	/// <summary>
	/// 显示验证码或按钮
	/// </summary>
	/// <param name="value">验证码按钮二选一</param>
	void ShowBtnCode(bool value)
	{
		BtnCode.gameObject.SetActive(value);
		TimeCode.gameObject.SetActive(!value);
		//ShowText(value);
		//nRemainDuration = 90.0f;
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

		//MD5登录密码
		//MD5Pasw = Tool.GetMD532(TxtPasW.text).ToLower();

		Debug.Log("正在发送！");
		//Bind(TxtTelNum.text, TxtPasW.text, int.Parse(userid), int.Parse(TxtCode.text), token);
		Debug.Log("电话号: " + TxtTelNum.text);
		Debug.Log("UID: " + int.Parse(iUserid));
		Debug.Log("CODE: " + TxtCode.text);
		Bind(TxtTelNum.text, int.Parse(iUserid), TxtCode.text);
		Debug.Log("已发送！");
	}

	/// <summary>
	/// 向服务器发送数据
	/// </summary>
	/// <returns>The bind.</returns>
	/// <param name="moble">Moble.</param>
	/// <param name="Userid">Userid.</param>
	/// <param name="code">Code.</param>
	void Bind(string moble, int Userid, string code)
	{
		ShowWaitingView(true);  //转圈等待

		//Debug.Log("电话号: " + moble);
		//Debug.Log("UID: " + Userid);
		//Debug.Log("CODE: " + code);

		FiConvertFormalBindAccount nRequest = new FiConvertFormalBindAccount();
		nRequest.result = 0;
		nRequest.userID = Userid;
		nRequest.phone = moble;
		nRequest.code = code;

		Debug.Log("@@@@ 驗證碼 = " + nRequest.code);

		//DispatchData nData = new DispatchData();
		//nData.type = FiEventType.SEND_CONVERSION_REQUEST;
		//nData.data = nRequest;
		//DataControl.GetInstance().PushSocketSnd(nData.type,nRequest);
		DataControl.GetInstance().PushSocketSnd(FiEventType.XL_GET_BIND_PHONE_REQUEST, nRequest);

		Debug.Log("正在进行绑定！");

	}

	public void OnClose()
	{
		AudioManager._instance.PlayEffectClip(AudioManager.effect_closePanel);
		//DataControl.GetInstance ().GetMyInfo ().account = null;
		//Destroy(gameObject);
		Destroy(myself);
	}

	/// <summary>
	/// 绑定时等待
	/// </summary>
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

	/// <summary>
	/// 回调
	/// </summary>
	/// <param name="data">Data.</param>
	private void RecvBackBindPhone(object data)
	{
		Debug.Log("接收回调");

		//FiSystemReward info = (FiSystemReward)data;
		FiConvertFormalBindAccount info = (FiConvertFormalBindAccount)data;
		//UIBindSuccess.nResponse = info;


		//Debug.Log("---回调------resultCode: " + info.resultCode + ", propID：" + info.propID + ", propCount: " + info.propCount + ", msg: " + info.msg);

		if (info.result == 1 || info.result == 2)
		{    //手机绑定成功
			myInfo.isGuestLogin = false;
			ShowWaitingView(false); //关闭转圈等待

			//显示成功信息
			GameObject WindowClone1 = Instantiate(Window1);
			UITipAutoNoMask ClickTips1 = WindowClone1.GetComponent<UITipAutoNoMask>();
			ClickTips1.tipText.text = "綁定成功！";
			Debug.Log("綁定成功！");

			myInfo.isGuestLogin = false;    //关闭游客登录
			UIBindSuccess.TelNum = TxtTelNum.text;
			UIHallCore.setBtnBinActivity = true;
			UIHallCore.isShowBtnBind = false;

			//弹出绑定成功弹窗
			try
			{
				GameObject window = BindWindowCtrl.Instense.BindSuccessWindow();
			}
			catch {

            }

			#region 清除游客
#if UNITY_IPHONE && !UNITY_EDITOR
			    ToSetGuestLogin ();
			    //CleanGuestLoginCache();
#elif UNITY_ANDROID && !UNITY_EDITOR
			    UpdateGuestLoginState ();
#endif
			#endregion

			
			//关闭当前弹窗
			BindWindowCtrl.Instense.GenerateWindow_Close();
			UIColseManage.instance.CloseUI();
			OnClose();
		}
		else if (info.result == -10)
		{   //绑定失败
			ShowWaitingView(false); //关闭转圈等待

			////显示失败信息
			GameObject WindowClone1 = Instantiate(Window1);
			UITipAutoNoMask ClickTips1 = WindowClone1.GetComponent<UITipAutoNoMask>();
			ClickTips1.tipText.text = "驗證碼錯誤";
			Debug.Log("驗證碼錯誤！");
			return;
		}
		else if (info.result == -5)
		{
			ShowWaitingView(false); //关闭转圈等待

			////显示失败信息
			GameObject WindowClone1 = Instantiate(Window1);
			UITipAutoNoMask ClickTips1 = WindowClone1.GetComponent<UITipAutoNoMask>();
			ClickTips1.tipText.text = "您的手機號碼已達綁定上限";
			Debug.Log("您的手機號碼已達綁定上限！");
			return;
		}
		else
		{
			ShowWaitingView(false);
			////显示失败信息
			GameObject WindowClone1 = Instantiate(Window1);
			UITipAutoNoMask ClickTips1 = WindowClone1.GetComponent<UITipAutoNoMask>();
			ClickTips1.tipText.text = "綁定失敗，請檢查輸入信息是否有誤";
			Debug.Log("绑定失败！");
			return;
		}
	}

	/// <summary>
	/// 顯示發送手機驗證回傳訊息
	/// </summary>
	/// <param name="msg">顯示訊息</param>
	public void ShowAuthMsg(string msg)
	{
		GameObject WindowClone1 = Instantiate(Window1);
		UITipAutoNoMask ClickTips1 = WindowClone1.GetComponent<UITipAutoNoMask>();
		ClickTips1.tipText.text = msg;
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
	//void ShowText(bool value)
	//{
	//	if (value)
	//		text.text = "账号升级后绑定手机号为登录账号，可以通过账号登录按钮进入游戏，账号信息永久保留。";
	//	else
	//		text.text = "验证码已发送，请在90秒内输入";
	//}

	public void OnDestroy()
	{
		EventControl nControl = EventControl.instance();
		nControl.removeEventHandler(FiEventType.XL_GET_BIND_PHONE_REQUEST, RecvBackBindPhone);
	}
}
