using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using AssemblyCSharp;
using System;
using System.Security.Cryptography;
using System.Text;

public class UISetPhonePassword : MonoBehaviour, IMsgHandle
{
	public InputField txtPhoneNumber;
	public InputField txtPassword1;
	public InputField txtPassword2;
	public Text textNickname;

	MyInfo myInfo;
	GameObject Window1;
	GameObject mWaitingView;
	public GameObject mainWindow;

	private List<string> nicknames = new List<string>();

	public static UISetPhonePassword Instance;

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

	}

	void Start()
	{
		OnInit();

	}

	public void OnInit()
	{
		Debug.Log(" UICheckPhoneNumber OnInit ");
		EventControl nControl = EventControl.instance();
		nControl.addEventHandler(FiProtoType.XL_GET_PHONE_PASSWORD, RecvBackPassword); // 118 驗證資訊
		nControl.addEventHandler(FiProtoType.XL_CHOISE_ACCOUNT_LOGIN_RESPON, RecvBackAssociateToken); // 119 登入
		nControl.addEventHandler(FiProtoType.XL_GET_NEW_NICK_RESPON, RecvBackNickname); // 121 獲取隨機暱稱

		mainWindow.gameObject.SetActive(false); // 關閉視窗至接收到暱稱為止
		SendNickRequest();
	}

	private void RecvBackPassword(object data)
	{
		ShowWaitingView(false);
		PhoneNumberPass info = (PhoneNumberPass)data;
		Debug.Log(" ========== RecvBackPhoneLogin ========== result = " + info.result);
		Debug.Log(" ========== RecvBackPhoneLogin ========== Count = " + info.PhoneNumberPassInfo.Count);

		if (info.result == 0)
		{
            //註冊成功 開始登入
			//gameObject.SetActive(false);
			ShowWaitingView(true);
			if (info.PhoneNumberPassInfo.Count > 0)
			{
				for (int PWA = 0; PWA < info.PhoneNumberPassInfo.Count; PWA++)
				{
                    Debug.Log("-----RecvBackPassword-----PWA = " + PWA);
					PhoneNumberLoginInfo info_detial = (PhoneNumberLoginInfo)info.PhoneNumberPassInfo[PWA];
					Debug.Log("-----RecvBackPassword-----accountType = " + info_detial.accountType);
					Debug.Log("-----RecvBackPassword-----user_id = " + info_detial.user_id);
					Debug.Log("-----RecvBackPassword-----accountName = " + info_detial.accountName);
					Debug.Log("-----RecvBackPassword-----strToken = " + info_detial.strToken);
					Debug.Log("-----RecvBackPassword-----nickName = " + info_detial.nickname);

					myInfo.Associate_Type.Add(info_detial.accountType);
					myInfo.Associate_Userid.Add(info_detial.user_id);
					myInfo.Associate_Name.Add(info_detial.accountName);
					myInfo.Associate_Token.Add(info_detial.strToken);
					//myInfo.Associate_Nickname.Add(info_detial.nickname);
                    //myInfo.nickname = info_detial.accountName;

                    //if (!string.IsNullOrEmpty(info_detial.strToken))
                    //{
                    //	myInfo.acessToken = info_detial.strToken;
                    //	myInfo.openId = info_detial.user_id.ToString();
                    //	myInfo.userID = info_detial.user_id;
                    //	myInfo.nickname = info_detial.accountName;
                    //	myInfo.platformType = 22;
                    //	myInfo.isPhoneNumberLogin = true;
                    //	Facade.GetFacade().message.login.SendLoginRequest();
                    //}
                }
				GetResponseToLogin();
				//gameObject.SetActive(false);
				ShowWaitingView(false);
				//transform.parent.Find("Choise_Associate_Account").gameObject.SetActive(true);
				ShowWaitingView(true);
			}
		}
		else if (info.result == 4)
		{
			ShowAuthMsg("此帳號名已被註冊，請換另一帳號名字嘗試再次註冊！ ！此暱稱已存在，請換另一暱稱嘗試再次註冊！");
			ShowWaitingView(false);
		}
		else if(info.result == 6)
		{
			ShowAuthMsg("抱歉地通知您，系統禁止了您的機器的註冊功能，請聯繫客戶服務中心了解詳細情況！");
			ShowWaitingView(false);
		}
		else if (info.result == 7)
		{
			ShowAuthMsg("此帳號名已被註冊，請換另一帳號名字嘗試再次註冊！");
			ShowWaitingView(false);
		}
		else if (info.result == 9)
		{
			ShowAuthMsg("註冊超限，請稍後再試！");
			ShowWaitingView(false);
		}
		else if (info.result == -4)
		{
			ShowAuthMsg("手機號長度不對");
			ShowWaitingView(false);
		}
		else if (info.result == -5)
		{
			ShowAuthMsg("手機號碼已註冊過");
			ShowWaitingView(false);
		}
		else if (info.result == -6)
		{
			ShowAuthMsg("有手機關聯帳號");
			gameObject.SetActive(false);
			transform.parent.Find("InputPhoneNubmer").gameObject.SetActive(true);
			ShowWaitingView(false);
		}
		else if (info.result == 30)
		{
			ShowAuthMsg("名稱重複");
			ShowWaitingView(false);
		}
		else
		{
			ShowAuthMsg("註冊失敗");
			ShowWaitingView(false);
		}

		
		//#if UNITY_EDITOR
		//		if (info.PhoneNumberPassInfo.Count>0) {
		//			for (int PWA=0; PWA< info.PhoneNumberPassInfo.Count; PWA++)
		//            {
		//				PhoneNumberLoginInfo info_detial = (PhoneNumberLoginInfo)info.PhoneNumberPassInfo[PWA];
		//				Debug.Log("-----RecvBackPassword-----accountType = " + info_detial.accountType);
		//				Debug.Log("-----RecvBackPassword-----user_id = " + info_detial.user_id);
		//				Debug.Log("-----RecvBackPassword-----accountName = " + info_detial.accountName);
		//				Debug.Log("-----RecvBackPassword-----strToken = " + info_detial.strToken);
		//			}
		//        }
		//#endif
	}

	public void GetResponseToLogin() {
		LoginAccountAssociateChoise nRequest = new LoginAccountAssociateChoise();
		nRequest.result = 0;
		nRequest.accountType = (int)myInfo.Associate_Type[0];
		nRequest.user_id = (int)myInfo.Associate_Userid[0];
		nRequest.accountName = "" + myInfo.Associate_Name[0];
		nRequest.strToken = "" + myInfo.Associate_Token[0];
		//nRequest.nickname = "" + myInfo.Associate_Nickname[0];
		//Debug.Log(nRequest.nickname);
		DataControl.GetInstance().PushSocketSnd(FiEventType.XL_GET_ASSOCIATE_ACC_LOGIN_REQUEST, nRequest);
		Debug.Log("已發送登入！");
	}

	private void RecvBackAssociateToken(object data)
	{
		ShowWaitingView(false);
		LoginAccountAssociateChoise info = (LoginAccountAssociateChoise)data;
#if UNITY_EDITOR
		Debug.Log(" ========== RecvBackAssociateToken ========== result = " + info.result);
		Debug.Log(" ========== RecvBackAssociateToken ========== accountType = " + info.accountType);
		Debug.Log(" ========== RecvBackAssociateToken ========== user_id = " + info.user_id);
		Debug.Log(" ========== RecvBackAssociateToken ========== accountName = " + info.accountName);
		Debug.Log(" ========== RecvBackAssociateToken ========== strToken = " + info.strToken);
		Debug.Log(" ========== RecvBackAssociateToken ========== nickname = " + info.nickname);
#endif

		if (info.result == 0)
		{
			//ShowAuthMsg("token取得成功");
			if (!string.IsNullOrEmpty(info.strToken))
			{
				myInfo.acessToken = info.strToken;
				myInfo.openId = info.user_id.ToString();
				myInfo.userID = info.user_id;
                if (info.nickname != null)
                {
                    myInfo.nickname = info.nickname;
                }
				myInfo.isPhoneNumberLogin = true;
				
#if UNITY_EDITOR
				Debug.Log(" ========== RecvBackAssociateToken ========== platformType = " + myInfo.platformType);
				Debug.Log(" ========== RecvBackAssociateToken ========== isPhoneNumberLogin = " + myInfo.isPhoneNumberLogin);
				Debug.Log(" ========== RecvBackAssociateToken ========== user_id = " + myInfo.openId);
				Debug.Log(" ========== RecvBackAssociateToken ========== nickname = " + myInfo.nickname);
				Debug.Log(" ========== RecvBackAssociateToken ========== strToken = " + myInfo.acessToken);
#endif
				Facade.GetFacade().message.login.SendLoginRequest();

			}
		}
		else
		{
			ShowAuthMsg("登入失敗");
			ShowWaitingView(false);
		}
	}

	public void OnSureLogin()
	{
		AudioManager._instance.PlayEffectClip(AudioManager.ui_click);
		ShowWaitingView(true);
		if (string.IsNullOrEmpty(txtPhoneNumber.text))
		{
			GameObject WindowClone1 = Instantiate(Window1);
			UITipAutoNoMask ClickTips1 = WindowClone1.GetComponent<UITipAutoNoMask>();
			ClickTips1.tipText.text = "請輸入手機號";
			ShowWaitingView(false);
			return;
		}
        if (!myInfo.phone.Equals(txtPhoneNumber.text))
        {
            GameObject WindowClone1 = Instantiate(Window1);
            UITipAutoNoMask ClickTips1 = WindowClone1.GetComponent<UITipAutoNoMask>();
            ClickTips1.tipText.text = "手機號碼不一致";
			ShowWaitingView(false);
			return;
        }
        if (string.IsNullOrEmpty(txtPassword1.text) || string.IsNullOrEmpty(txtPassword2.text))
		{
			GameObject WindowClone1 = Instantiate(Window1);
			UITipAutoNoMask ClickTips1 = WindowClone1.GetComponent<UITipAutoNoMask>();
			ClickTips1.tipText.text = "請輸入密碼";
			ShowWaitingView(false);
			return;
		}
		if (!txtPassword1.text.Equals(txtPassword2.text))
		{
			GameObject WindowClone1 = Instantiate(Window1);
			UITipAutoNoMask ClickTips1 = WindowClone1.GetComponent<UITipAutoNoMask>();
			ClickTips1.tipText.text = "密碼不一致";
			ShowWaitingView(false);
			return;
		}

		Debug.Log("正在發送！");
#if UNITY_EDITOR
		//Bind(TxtTelNum.text, TxtPasW.text, int.Parse(userid), int.Parse(TxtCode.text), token);
		Debug.Log("電話號: " + txtPhoneNumber.text);
		Debug.Log("密碼: " + txtPassword1.text);
#endif
		SendProto(txtPhoneNumber.text, txtPassword1.text, textNickname.text);
		Debug.Log("已發送！");
	}

	/// <summary>
	/// 向服务器发送数据
	/// </summary>
	/// <returns>The bind.</returns>
	/// <param name="moble">Moble.</param>
	/// <param name="code">Code.</param>
	void SendProto(string moble, string password,string nickcame)
	{
		ShowWaitingView(false);  //转圈等待
		ShowWaitingView(true);  //转圈等待

		SetPhoneLoginPass nRequest = new SetPhoneLoginPass();
		nRequest.phone = moble;
		nRequest.pass = GetMD5(password).ToLower();
		nRequest.nickname = nickcame;
        if (myInfo.Associate_Nickname.Count > 0) // 1697版暫時方案
        {
            myInfo.Associate_Nickname[0] = nickcame;
        }
        myInfo.nickname = nickcame;

		Debug.Log("密碼: " + nRequest.pass);
		DataControl.GetInstance().PushSocketSnd(FiEventType.XL_SET_PHONE_LOGIN_PASS_REQUEST, nRequest);
		Debug.Log("手機密碼已傳送！");
	}

	public static string GetMD5(string myString)
	{
		byte[] result = Encoding.Default.GetBytes(myString);    //tbPass为输入密码的文本框
		MD5 md5 = new MD5CryptoServiceProvider();
		byte[] output = md5.ComputeHash(result);
		string str = BitConverter.ToString(output).Replace("-", "");
		if (str.Length != 32)
			Debug.LogError("--------------開始調試--------------");
		return str;
	}

    #region 暱稱
    private void RecvBackNickname(object data)
	{
		ShowWaitingView(false);
		LoginAccountNickChoice info = (LoginAccountNickChoice)data;
#if UNITY_EDITOR
		Debug.Log(" ========== RecvBackNickname ========== languageType = " + info.languageType);
		string logText = " ========== RecvBackNickname ========== nickArray = ";
		for(int i = 0; i < info.nickArray.Count; i++)
		{
			logText += info.nickArray[i] + ",";
			//Debug.Log(" ========== RecvBackNickname ========== nickArray[" + i + "] =" + info.nickArray[i]);
		}
		Debug.Log(logText);
#endif
        if (!mainWindow.gameObject.activeSelf) // 開啟視窗
        {
			mainWindow.SetActive(true);
        }

		nicknames = info.nickArray;
		textNickname.text = nicknames[0];
	}

	private int nicknameIndex;
	public void ChangeNickname()
	{
		if (nicknameIndex < nicknames.Count)
		{
			nicknameIndex += 1;
			textNickname.text = nicknames[nicknameIndex];
		}
		else
		{
			nicknameIndex = 0;
			//SendNickRequest();
		}
	}
	/// <summary>
	/// 送出暱稱請求
	/// </summary>
	private void SendNickRequest()
	{
		ShowWaitingView(false);  //转圈等待
		ShowWaitingView(true);  //转圈等待

		LoginAccountNickChoice nRequest = new LoginAccountNickChoice();
		DataControl.GetInstance().PushSocketSnd(FiEventType.XL_GET_USER_NICK_REQUEST, null); // 
		Debug.Log("已請求暱稱！");
	}
    #endregion

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

	/// <summary>
	/// 顯示提示
	/// </summary>
	/// <param name="msg">顯示訊息</param>
	public void ShowAuthMsg(string msg)
	{
		GameObject WindowClone1 = Instantiate(Window1);
		UITipAutoNoMask ClickTips1 = WindowClone1.GetComponent<UITipAutoNoMask>();
		ClickTips1.tipText.text = msg;
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

	public void OnDestroy()
	{
		EventControl nControl = EventControl.instance();
		nControl.removeEventHandler(FiProtoType.XL_GET_PHONE_PASSWORD, RecvBackPassword);
		nControl.removeEventHandler(FiProtoType.XL_CHOISE_ACCOUNT_LOGIN_RESPON, RecvBackAssociateToken);
		nControl.removeEventHandler(FiProtoType.XL_GET_NEW_NICK_RESPON, RecvBackNickname);
	}
}
