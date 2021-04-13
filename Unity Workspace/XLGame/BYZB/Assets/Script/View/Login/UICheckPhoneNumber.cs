using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using AssemblyCSharp;

public class UICheckPhoneNumber : MonoBehaviour, IMsgHandle
{
	string key = "72FAWaNZdkl4t65EdvMVCZApcy1UlmlL";

	public InputField txtPhoneNumber;
	public InputField txtMMSAuth;
	public Button BtnCode;
	public Text TimeCode;
    
	MyInfo myInfo;
	GameObject Window1;
	GameObject mWaitingView;
	float nRemainDuration = 60.0f;
	public GameObject myself;

	public static UICheckPhoneNumber Instance;

	void Awake()
	{
		if (GameController._instance == null) //渔场和大厅里赋值的摄像机不同
			gameObject.GetComponent<Canvas>().worldCamera = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<Camera>();
		else
			gameObject.GetComponent<Canvas>().worldCamera = ScreenManager.uiCamera;

		Instance = this;
		UIColseManage.instance.ShowUI(this.gameObject);
		Window1 = UnityEngine.Resources.Load("Window/WindowTipsThree") as UnityEngine.GameObject;
		myInfo = (MyInfo)Facade.GetFacade().data.Get(FacadeConfig.USERINFO_MODULE_ID);
		myInfo.isPhoneNumberLogin = true;
		DataControl.GetInstance().ConnectSvr(myInfo.ServerUrl, AppInfo.portNumber);
	}

	void Start()
	{
		//userid = myInfo.mLoginData.username;
		OnInit();
		myInfo.Associate_Type = new ArrayList();
		myInfo.Associate_Userid = new ArrayList();
		myInfo.Associate_Name = new ArrayList();
		myInfo.Associate_Token = new ArrayList();
		myInfo.Associate_Nickname = new ArrayList();
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

	public void OnInit()
	{
		Debug.Log(" UICheckPhoneNumber OnInit ");
		EventControl nControl = EventControl.instance();
		nControl.addEventHandler(FiProtoType.XL_GET_PHONE_LOGIN, RecvBackPhoneLogin);
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

	/// <summary>
	/// 获取验证码发送
	/// </summary>
	public void OnSendVerifyNumber()
	{
		AudioManager._instance.PlayEffectClip(AudioManager.ui_click);

		if (string.IsNullOrEmpty(txtPhoneNumber.text) || txtPhoneNumber.text.Length != 11)
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
			string nCode = www.text.Substring(0,1);
			if (nCode.Equals("1"))
			{
				string msg = www.text.Substring(2);
				ShowAuthMsg(msg);
				Debug.LogError(" [ get msg ] " + msg);
			}
			else
			{
				string msg = www.text.Substring(2);
				Debug.LogError(" [ get msg ] " + msg);
				if (nCode.Equals("1"))
				{
					ShowAuthMsg("提交失敗");
				}
				else if (nCode.Equals("2"))
				{
					ShowAuthMsg("請在60秒之後再獲取");
				}
				else if (nCode.Equals("3"))
				{
					ShowAuthMsg("當前帳戶當日短信已達到上限");
				}
			}
		}
	}

	string GetAskString()
	{
		/*
         * 参数名	必选	类型	说明
         * type     是类型 1 根据账号名或密码 发送验证码 2 注册发送验证码
         * uid      是用户id
         * account  是用戶帳號
         * phone    是手机号
         * md5      是由(phone+key)进行md5后的字符串
         * key      是 72FAWaNZdkl4t65EdvMVCZApcy1UlmlL
         */
		string nSign = Tool.GetMD532("" + txtPhoneNumber.text + key).ToLower();
		string head = UIUpdate.WebUrlDic[WebUrlEnum.MMSAuth] + "?account=&uid=&type=2&";

		//return "http://mobile.cl0579.com/api/api_YKBind.aspx?type=3&uid=" + userid + "&phone=" + TxtTelNum.text + "&md5=" + nSign;
		//return UIUpdate.WebUrlDic[WebUrlEnum.RegisterWithPhone] + "?type=3&uid=" + "&phone=" + txtPhoneNumber.text + "&md5=" + nSign;
		string mmsurl = head + "phone=" + txtPhoneNumber.text + "&md5=" + nSign;
		Debug.Log("       mms     "+ mmsurl);
		
		return mmsurl;
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

	private void RecvBackPhoneLogin(object data) {
		ShowWaitingView(false);
		//Debug.Log(" ========== RecvBackPhoneLogin ========== ");
		PhoneNumberLoginArray info = (PhoneNumberLoginArray)data;
		Debug.Log(" ========== RecvBackPhoneLogin ========== result = "+info.result);
		Debug.Log(" ========== RecvBackPhoneLogin ========== count = " + info.PhoneNumberInfo.Count);

#if UNITY_EDITOR
		if (info.PhoneNumberInfo.Count > 0)
		{
			for (int detial = 0; detial < info.PhoneNumberInfo.Count; detial++)
			{

				PhoneNumberLoginInfo info_detial = (PhoneNumberLoginInfo)info.PhoneNumberInfo[detial];
				Debug.Log("-----UICheckPhoneNumber-----RecvBackPhoneLogin-----accountType = " + info_detial.accountType);
				Debug.Log("-----UICheckPhoneNumber-----RecvBackPhoneLogin-----user_id = " + info_detial.user_id);
				Debug.Log("-----UICheckPhoneNumber-----RecvBackPhoneLogin-----accountName = " + info_detial.accountName);
				Debug.Log("-----UICheckPhoneNumber-----RecvBackPhoneLogin-----strToken = " + info_detial.strToken);
				Debug.Log("-----UICheckPhoneNumber-----RecvBackPhoneLogin-----nickname = " + info_detial.nickname);

			}
			//gameObject.SetActive(false);
			//transform.parent.Find("Choise_Associate_Account").gameObject.SetActive(true);
		}
#endif
		//登陆过程添加了新的验证码判断条件，return的值-1 code不对/ -2 验证码错误次数太多，需要重新获取/ -3 验证码失效
		if (info.result == 0)
		{
			if (info.PhoneNumberInfo.Count > 0)
			{
				for (int detial = 0; detial < info.PhoneNumberInfo.Count; detial++)
				{
					PhoneNumberLoginInfo info_detial = (PhoneNumberLoginInfo)info.PhoneNumberInfo[detial];
					myInfo.Associate_Type.Add(info_detial.accountType);
					myInfo.Associate_Userid.Add(info_detial.user_id);
					myInfo.Associate_Name.Add(info_detial.accountName);
					myInfo.Associate_Token.Add(info_detial.strToken);
					myInfo.Associate_Nickname.Add(info_detial.nickname);
				}
				//gameObject.SetActive(false);
				//transform.parent.Find("Choise_Associate_Account").gameObject.SetActive(true);
				OnExit();
                GameObject window = Resources.Load("Window/Choise_Associate_Account") as GameObject;
                //GameObject window = Resources.Load("Window/SetupPhonePassword") as GameObject;
                GameObject.Instantiate(window);
			}
		}
		else if (info.result == 1) //註冊新號碼
		{
			myInfo.phone = txtPhoneNumber.text.ToString();
			//gameObject.SetActive(false);
			//transform.parent.Find("SetupPhonePassword").gameObject.SetActive(true);
			OnExit();
			GameObject window = Resources.Load("Window/SetupPhonePassword") as GameObject;
			GameObject.Instantiate(window);
		}
		else if (info.result == -1)
		{
			ShowAuthMsg("驗證碼錯誤");
		}
		else if (info.result == -2) {
			ShowAuthMsg("驗證碼錯誤次數太多，需要重新獲取");
		}
		else if (info.result == -3)
		{
			ShowAuthMsg("驗證碼失效");
		}
		else
		{
			ShowAuthMsg(info.result.ToString());
		}
    }

	public void OnSendMMS()
	{
		if (string.IsNullOrEmpty(txtPhoneNumber.text) || LoginUtil.GetIntance() == null)
		{
			return;
		}
	}

	public void OnBind()
	{
		AudioManager._instance.PlayEffectClip(AudioManager.ui_click);

		if (string.IsNullOrEmpty(txtPhoneNumber.text))
		{
			GameObject WindowClone1 = Instantiate(Window1);
			UITipAutoNoMask ClickTips1 = WindowClone1.GetComponent<UITipAutoNoMask>();
			ClickTips1.tipText.text = "請輸入手機號";
			return;
		}
		if (string.IsNullOrEmpty(txtMMSAuth.text))
		{
			GameObject WindowClone1 = Instantiate(Window1);
			UITipAutoNoMask ClickTips1 = WindowClone1.GetComponent<UITipAutoNoMask>();
			ClickTips1.tipText.text = "請輸入驗證碼";
			return;
		}

		//MD5登錄密碼
		//MD5Pasw = Tool.GetMD532(TxtPasW.text).ToLower();

		Debug.Log("正在發送！");
		//Bind(TxtTelNum.text, TxtPasW.text, int.Parse(userid), int.Parse(TxtCode.text), token);
		Debug.Log("電話號: " + txtPhoneNumber.text);
		Debug.Log("CODE: " + txtMMSAuth.text);
		Bind(txtPhoneNumber.text, txtMMSAuth.text);
		Debug.Log("已發送！");
	}

	/// <summary>
	/// 向服务器发送数据
	/// </summary>
	/// <returns>The bind.</returns>
	/// <param name="moble">Moble.</param>
	/// <param name="code">Code.</param>
	void Bind(string moble, string code)
	{
		ShowWaitingView(true);  //转圈等待

        ConvertFormalPhoneNumber nRequest = new ConvertFormalPhoneNumber();
        nRequest.phone = moble;
        nRequest.code = code;

        Debug.Log("@@@@ 驗證碼 = " + nRequest.code);
        DataControl.GetInstance().PushSocketSnd(FiEventType.XL_GET_PHONE_NUMBER_REQUEST, nRequest);
        Debug.Log("手機號碼已傳送檢查是否有關連帳號！");
    }

	/// <summary>
	/// 顯示等待
	/// </summary>
	public void ShowWaitingView(bool nValue)
	{
		if (nValue)
		{
			mWaitingView = Instantiate(Resources.Load<GameObject>("MainHall/Common/WaitingView"));
			UIWaiting nData = mWaitingView.GetComponentInChildren<UIWaiting>();
			nData.HideBackGround();
			nData.SetInfo(10.0f, "請重新嘗試");
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

	public void onSure()
    {
		if (string.IsNullOrEmpty(txtPhoneNumber.text) || LoginUtil.GetIntance() == null || string.IsNullOrEmpty(txtMMSAuth.text))
		{
			return;
		}
	}

	public void OnExit()
	{
		//gameObject.SetActive(false);
		OnDestroy();
		UIColseManage.instance.CloseUI();
	}

	public void OnDisplay()
	{
		gameObject.SetActive(true);
	}

	public void OnDestroy()
	{
        EventControl nControl = EventControl.instance();
		nControl.removeEventHandler(FiProtoType.XL_GET_PHONE_LOGIN, RecvBackPhoneLogin);
    }
}
