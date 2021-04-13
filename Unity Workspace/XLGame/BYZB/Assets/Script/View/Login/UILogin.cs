using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.IO;
using System.Text;
using AssemblyCSharp;
using System.Runtime.InteropServices;
using LitJson;

/*
 * Joey 2020/02/27 推廣版修改，只有微信登入 -> SetLoginState { else {QQ登入、帳號登入 顯示改為false} }
 */

public class ServerAppData
{
	public int version;

	public int forceUpdate;

	//登陆验证的情况
	public int loginAuthor;
	//支付情况
	public int payAuthor;
}

public class LoginInfo
{

	public static bool isWechatLogin = false;
	public static bool isAccoutLogin = false;
	public static bool isQQLogin = false;
	public static bool isAPPToAPPLogin = false;
	public static bool c = false;
	public static string accountStr = "";
	public static string passwordStr = "";

	#if UNITY_ANDROID  || UNITY_IPHONE
	public static string pathUrl = Application.persistentDataPath + "/Login/";
	#elif UNITY_STANDALONE_WIN || UNITY_EDITOR
	public static string pathUrl = "file://" + Application.persistentDataPath + "/StreamingAssets/";   
	#endif
}

public class UILogin : MonoBehaviour
{
	private Facade mFacade = null;
	public GameObject BtnWechatLogin;
	public GameObject BtnGuestLogin;
    public GameObject BtnQQLogin;
    public GameObject nAccoutLogin;
	public GameObject BtnOtherLogin;
	public GameObject OtherLogin;
	public GameObject InputPhoneNumber;
	public GameObject SetupPassword;
	public GameObject ChoiseAssociateAcc;
	//	public GameObject Testtxtobj;//测试用的

	public string AppToApp;

	//测试
	//public string mServerUrl = "183.131.69.72";
	public Transform severChangeparent;
	
	//勋哥本机服务器
	//public string mServerUrl = "192.168.2.22";

	//台湾服务器
	//public string mServerUrl = "219.84.197.67";


	bool isUpdateTurnOn = false;
	bool bIpVerifySuccess = false;

	//舊正式1.6.5.0
	//public	string mServerUrl = "183.131.69.234";

	//新正式1.6.6.0
	public string mServerUrl;
	//public string mServerUrl = "122.226.186.30";
	//public string mServerUrl = "122.226.186.80";
	public string mServerUrlPromote = "183.131.69.230";
	//183.131.69.235

	//推廣版
	//public string mServerUrl = "183.131.69.72";
	//public string mServerPort = "50668";
	//public string mServerPort = "50669";



	public static UILogin Instance;

	
	public bool IschangeSever = false;

	ServerAppData mSvrData = null;
	public Text noticeText;
	public Text versionText;
	public Text trenchNum;

	void Awake ()
	{
		if (Facade.GetFacade ().config.isIphoneX2 ()) {
			noticeText.fontSize = 26;
		}
		versionText.text = "v" + AppInfo.appVersion.ToString ();
		trenchNum.text = ""+AppInfo.trenchNum;
		Tool.Init ();
		mFacade = Facade.GetFacade ();
		mFacade.DestroyAll ();
		mFacade.InitAll ();
		BtnGuestLogin.SetActive (false);
		UIFishingMsg.GetInstance ();
		UIHallMsg.GetInstance ();
		DataControl.GetInstance ().GetMyInfo ().loginInfo = null;
		//string ServerVersion_URL = UIUpdate.WebUrlDic[WebUrlEnum.LoginAddrIp];
		//StartCoroutine(VerifyServerVersion("http://183.131.69.227:186/FishingAppVersion.php"));
		//		Debug.Log ("LoginInfo.pathUrl = " + LoginInfo.pathUrl);
#if UNITY_DEBUG
		ShowChangeSever ();
#endif
		StartCoroutine (CheckVersion ());
	}

	public void CheckVersionEx()
	{
		//Debug.LogError ("ffffffffffffffffffffCheckVersionEx!= " + (int)WebUrlEnum.LoginAddrIp + "  value=" + UIUpdate.WebUrlDic.Count);
		if (UIUpdate.WebUrlDic.ContainsKey(WebUrlEnum.LoginAddrIp))
		{
            mServerUrl = UIUpdate.WebUrlDic[WebUrlEnum.LoginAddrIp];
            //測試用，24hr正式要改成server下發配置
            //mServerUrl = "183.131.69.230";
            //mServerUrl = "xlplg1.272gb.com";
            //mServerUrl = "183.146.209.245";
            //mServerUrl = "60.191.186.252";
            //Debug.LogError ("aaaaaaaaaaaaaaaa CheckVersionEx!= " + mServerUrl + "  value=" + UIUpdate.WebUrlDic.Count);
        }
		else
		{
			mServerUrl = "xllogin010.272gb.com";
			//Debug.LogError ("bbbbbbbbbbbbbbbbbb CheckVersionEx!= " + mServerUrl);
		}

		//Debug.LogError("domain = "+ mServerUrl);
		StartCoroutine(CheckVersion());
	}

	private void ShowChangeSever ()
	{
		GameObject window = Resources.Load ("Window/CheshiChange") as GameObject;
		GameObject windowClone = Instantiate (window);
		windowClone.transform.SetParent (severChangeparent);
		windowClone.transform.localScale = new Vector3 (1.9f, 1.9f, 1.9f);
		windowClone.transform.localPosition = new Vector3 (400f, 200f, 0f);
	
	}

	public void OnLoginState (bool isInstallWeChat, bool bGuestLogin = true)
	{
		BtnGuestLogin.SetActive (bGuestLogin);
		BtnWechatLogin.SetActive (isInstallWeChat);
		//游客登陆和微信登陆都开始了
		if (BtnGuestLogin.activeSelf && BtnWechatLogin.activeSelf) {
            BtnGuestLogin.transform.localPosition = new Vector3(-286, BtnGuestLogin.transform.localPosition.y);
            BtnWechatLogin.transform.localPosition = new Vector3(690, BtnGuestLogin.transform.localPosition.y);
        } else if (BtnGuestLogin.activeSelf && !BtnWechatLogin.activeSelf) { //只显示游客登陆，只有在微信没有安装的时候才存在
            BtnGuestLogin.transform.localPosition = new Vector3(0, BtnGuestLogin.transform.localPosition.y);
        } else if (!BtnGuestLogin.activeSelf && BtnWechatLogin.activeSelf) {
            BtnWechatLogin.transform.localPosition = new Vector3(690, BtnGuestLogin.transform.localPosition.y);
        }
	}


	IEnumerator VerifyWechatAvalible ()
	{
		while (true) {
			yield return new WaitForSeconds (1.0f);
			if (LoginUtil.GetIntance () != null) {
				if (mSvrData != null) {
					//开启游客登陆的情况
					if (mSvrData.loginAuthor == 1) {
						OnLoginState (LoginUtil.GetIntance ().isWeChatValid (), true);
					} else {//游客登陆的情况下，微信登陆必须开始
						OnLoginState (true, false);
					}
				}

				/*else {
					OnLoginState (LoginUtil.GetIntance ().isWeChatValid (), true);
				}*/
			}
		}
	}

	void Start ()
	{
		Screen.sleepTimeout = SleepTimeout.NeverSleep;
		Instance = this;

		//#if UNITY_ANDROID  && !UNITY_EDITOR
		//BtnGuestLogin.SetActive (true);
		//BtnWechatLogin.SetActive (true);
		//BtnGuestLogin.transform.localPosition = new Vector3 (-286, BtnGuestLogin.transform.localPosition.y);
		//BtnWechatLogin.transform.localPosition = new Vector3 (481, BtnGuestLogin.transform.localPosition.y);
		//#elif UNITY_EDITOR
		//BtnGuestLogin.SetActive (true);
		//BtnGuestLogin.transform.localPosition = new Vector3(0, BtnGuestLogin.transform.localPosition.y);
		//#endif


		//后台配置功能
		//		UIUpdate.Instance.BackstageSend ();

		/* 2020/03/01 Joey 推廣版禁止遊客登入
		StartCoroutine (GuestLoginIOSPromptly ());
        */

		//if (AppInfo.trenchNum == 51000000) {
		//	StartCoroutine(GuestLoginIOSPromptly());
		//}

		//GetAppInfostr();

		Screen.autorotateToLandscapeLeft = true;
		Screen.autorotateToLandscapeRight = true;
		Screen.autorotateToPortrait = false;
		Screen.autorotateToPortraitUpsideDown = false;
		//if (FileUtilManager.IsExistsFile (LoginInfo.pathUrl, "Login.txt")) {
		//          InitAutoLogin(LoginInfo.pathUrl, "Login.txt");
		//          關閉自動登入

		//          StartCoroutine(AutoLoginEnu());
		//      }
	}

	/// <summary>
	/// 自动登录,Android的sharesdk调用会比ios慢,新需求说自动登录也不显示按钮
	/// </summary>
	/// <returns>The login enu.</returns>
    /// 
	//IEnumerator AutoLoginEnu ()
	//{
	//	SetLoginState (0);
	//	#if UNITY_IPHONE
	//	yield return new WaitForSeconds (1.7f);
	//	#elif UNITY_ANDROID
	//	yield return new WaitForSeconds (2.8f);
	//	#endif

	//	if (LoginInfo.isWechatLogin) {
	//		ToWeChatLogin ();
	//	}
	//	if (LoginInfo.isQQLogin) {
	//		OnQQLogin ();
	//	}
	//	if (LoginInfo.isAccoutLogin) {
	//		AutoSetAccountState (LoginInfo.accountStr, LoginInfo.passwordStr);
	//	}
	//	yield return new WaitForSeconds (2f);
	//	SetLoginState (1);
	//}

	/// <summary>
	/// 初始化解析
	/// </summary>
	/// <param name="path">Path.</param>
	/// <param name="filename">Filename.</param>
	void  InitAutoLogin (string path, string filename)
	{
		string data = FileUtilManager.LoadFile (path, filename);
		if (data == null || data == "") {
			return;
		}
		LitJson.JsonData jd = LitJson.JsonMapper.ToObject (data);

		//int isQQLogin = (int)jd ["login"] ["isQQLogin"];
		int isWechatLogin = (int)jd ["login"] ["isWechatLogin"];
		int isAccountLogin = (int)jd ["login"] ["isAccoutLogin"];
		string account = (string)jd ["login"] ["Account"];
		string pwd = (string)jd ["login"] ["password"];
		//LoginInfo.isQQLogin = isQQLogin == 0 ? false : true;
		LoginInfo.isWechatLogin = isWechatLogin == 0 ? false : true;
		//LoginInfo.isAccoutLogin = isAccountLogin == 0 ? false : true;
		LoginInfo.accountStr = account;
		LoginInfo.passwordStr = pwd;
	}

	/// <summary>
	/// 设置账号自动登录的状态
	/// </summary>
	/// <param name="account">Account.</param>
	/// <param name="pwd">Pwd.</param>
	void AutoSetAccountState (string account, string pwd)
	{
		if (string.IsNullOrEmpty (account) || LoginUtil.GetIntance () == null) {
			return;
		}
		MyInfo myInfo = DataControl.GetInstance ().GetMyInfo ();
		myInfo.isGuestLogin = false;
		myInfo.platformType = 0;
		LoginUtil.GetIntance ().AutoLoginWitnAccount (account, pwd);
		LoginInfo.isAccoutLogin = true;
		AutoAccountLoginFile (LoginInfo.pathUrl, account, pwd);
	}

	/// <summary>
	/// 账号自动登录
	/// </summary>
	/// <param name="pathUrl">Path URL.</param>
	/// <param name="Account">Account.</param>
	/// <param name="password">Password.</param>
	void AutoAccountLoginFile (string pathUrl, string Account, string password)
	{
		string loginStr = "{\"login\":{" + "\"Account\":\"" + Account +
		                  "\"," + "\"password\":\"" + password +
		                  "\"," + "\"isQQLogin\":" + (LoginInfo.isQQLogin ? 1 : 0) +
		                  "," + "\"isWechatLogin\":" + (LoginInfo.isWechatLogin ? 1 : 0) +
		                  "," + "\"isAccoutLogin\":" + (LoginInfo.isAccoutLogin ? 1 : 0) +
		                  "}}";
		Debug.Log("AutoAccountLoginFile = " + loginStr);
		FileUtilManager.CreateFile (pathUrl, "Login.txt", loginStr);
	}

	/// <summary>
	/// QQ自动登录
	/// </summary>
	/// <param name="pathUrl">Path URL.</param>
	void AutoQQLoginFile (string pathUrl)
	{
		string loginStr = "{\"login\":{" + "\"Account\":\"" + LoginInfo.accountStr +
		                  "\"," + "\"password\":\"" + LoginInfo.passwordStr +
		                  "\"," + "\"isQQLogin\":" + (LoginInfo.isQQLogin ? 1 : 0) +
		                  "," + "\"isWechatLogin\":" + (LoginInfo.isWechatLogin ? 1 : 0) +
		                  "," + "\"isAccoutLogin\":" + (LoginInfo.isAccoutLogin ? 1 : 0) +
		                  "}}";
		Debug.Log("AutoQQLoginFile = " + loginStr);
		FileUtilManager.CreateFile (pathUrl, "Login.txt", loginStr);
	}

	/// <summary>
	/// 微信自动登录
	/// </summary>
	/// <param name="pathUrl">Path URL.</param>
	void AutoWeChatLogin (string pathUrl)
	{
		string loginStr = "{\"login\":{" + "\"Account\":\"" + LoginInfo.accountStr +
		                  "\"," + "\"password\":\"" + LoginInfo.passwordStr +
		                  "\"," + "\"isQQLogin\":" + (LoginInfo.isQQLogin ? 1 : 0) +
		                  "," + "\"isWechatLogin\":" + (LoginInfo.isWechatLogin ? 1 : 0) +
		                  "," + "\"isAccoutLogin\":" + (LoginInfo.isAccoutLogin ? 1 : 0) +
		                  "}}";
		Debug.Log("AutoWeChatLogin = " + loginStr);
		FileUtilManager.CreateFile (pathUrl, "Login.txt", loginStr);
	}

	IEnumerator CheckVersion ()
	{
		Debug.Log("  CheckVersion  CheckVersion  CheckVersion : "+ UIUpdate.Instance.isUpdateVersion+" : "+ UIUpdate.Instance.isLaterShow);
		yield return new WaitForSeconds (.5f);
		if (UIUpdate.Instance.isUpdateVersion && UIUpdate.Instance.isLaterShow) {
			Debug.Log("  CheckVersion  1");
			string path = "Prefabs/UpdateCanvas";
			GameObject WindowClone = AppControl.OpenWindow(path);
			WindowClone.gameObject.SetActive(true);
			Instantiate (UIUpdate.Instance.UpdateWindow);
			Invoke ("OpenConnection", 1f);
		} 
		if (UIUpdate.Instance.isUpdateVersion && UIUpdate.Instance.isLaterShow == false) {
			Debug.Log("  CheckVersion  2");
			string path = "Prefabs/UpdateCanvas";
			GameObject WindowClone = AppControl.OpenWindow(path);
			WindowClone.gameObject.SetActive(true);
			Instantiate (UIUpdate.Instance.UpdateWindow);
		} else {
			Debug.Log("  CheckVersion  3");
			Invoke ("OpenConnection", 1f);
		}

	}

     //* 2020/03/01 Joey 推廣版禁止遊客登入
	//public void GuestLoginToAndroid ()
	//{
	//	StartCoroutine (GuestLoginAndPromptly ());
	//}

	/// <summary>
	/// 其他APP发送来的消息url
	/// </summary>
	/// <param name="str">String.</param>
	public void ReciveAppMsg (string str)
	{
		Debug.Log ("APP is comeing!!!!" + str);
		AppToApp = str;

		if (AppToApp == "" || AppToApp == "0" || AppToApp == null) {
			if (FileUtilManager.IsExistsFile (LoginInfo.pathUrl, "Login.txt")) {
				InitAutoLogin (LoginInfo.pathUrl, "Login.txt");
                //關閉自動登入
				//StartCoroutine (AutoLoginEnu ());
			}
		} else {
			//string[] strs = str.Split('&');
			//if(strs.Length<4)
			//{
			//    Debug.LogError("传来的参数不正确,请查明~~~~~~");
			//    return;
			//}
			//string uid = strs[0];
			//string token = strs[1];
			//string nickname = strs[2];
			//string avatar = strs[3];
			//LoginUtil.GetIntance().SetUserInfo(int.Parse(uid), token, nickname, avatar);
			//LoginUtil.GetIntance().SendAPPToAPPData();
			StartCoroutine (APPLogin (str));
		}
	}


	IEnumerator APPLogin (string str)
	{
		SetLoginState (0);
		string[] strs = str.Split ('&');
		if (strs.Length < 4) {
			Debug.LogError ("传来的参数不正确,请查明~~~~~~");
			yield return 0;
		}
		Debug.LogError (string.Format ("参数正确~~~~UID:{0},Token:{1},NickName:{2},Avatar:{3}", strs [0], strs [1], strs [2], strs [3]));
		string uid = strs [0];
		string token = strs [1];
		string nickname = strs [2];
		string avatar = strs [3];

#if UNITY_IPHONE
		yield return new WaitForSeconds (1.7f);
#elif UNITY_ANDROID
		yield return new WaitForSeconds (2.8f);
#endif
		LoginUtil.GetIntance ().ShowWaitingView (true);

		LoginUtil.GetIntance ().SetUserInfo (uid, token, nickname, avatar);

		LoginUtil.GetIntance ().SendAPPToAPPData ();
		yield return new WaitForSeconds (2f);
		SetLoginState (1);
	}

	public static void AccountLoginToAndroid ()
	{
		AndroidJavaClass jc = new AndroidJavaClass ("com.unity3d.player.UnityPlayer");
		AndroidJavaObject jo = jc.GetStatic<AndroidJavaObject> ("currentActivity");
		jo.Call ("setAccountLogin");
	}

	/// <summary>
	/// 调用安卓/IOS发送消息给unity的方法
	/// </summary>
	public  void GetAppInfostr ()
	{
		#if UNITY_DEBUG

		#elif UNITY_ANDROID
		AndroidJavaClass jc = new AndroidJavaClass ("com.unity3d.player.UnityPlayer");
		AndroidJavaObject jo = jc.GetStatic<AndroidJavaObject> ("currentActivity");
		jo.Call ("GetTestInfostr");
		#elif UNITY_IPHONE
		GetIosAppInfo ();
		#endif
	}

	//	public void test_button ()
	//	{
	//		GetIosAppInfo ();
	//	}

	/// <summary>
	/// 游客第一次登录按钮不显示
	/// </summary>
	void SetLoginState (int state)
	{
		if (AppInfo.trenchNum == 51000000 || AppInfo.trenchNum == 51000002)
		{
			if (state == 0)
			{
				BtnWechatLogin.SetActive(false);
				BtnQQLogin.SetActive(false);
				nAccoutLogin.SetActive(false);
				BtnOtherLogin.SetActive(false);
			}
			else
			{
				// Joey 2020/02/27 推廣版修改，只有微信登入。
				BtnWechatLogin.SetActive(true);
				//BtnQQLogin.SetActive(true);
				// 泰山 2020/11/25 取消其他登入按鈕
				//BtnOtherLogin.SetActive(true);
				//測試把帳號打開
				nAccoutLogin.SetActive(true);
			}
		}
		else {
			if (state == 0)
			{
				BtnWechatLogin.SetActive(false);
				BtnQQLogin.SetActive(false);
				nAccoutLogin.SetActive(false);
                BtnOtherLogin.SetActive(false);
			}
			else
			{
				// Joey 2020/02/27 推廣版修改，只有微信登入。
				BtnWechatLogin.SetActive(true);
				BtnQQLogin.SetActive(false);
				//測試把帳號打開
				nAccoutLogin.SetActive(false);
				BtnOtherLogin.SetActive(false);
			}
		}
		
	}

	//	/// <summary>
	//	/// 自动登录按钮不点击
	//	/// </summary>
	//	void SetAutoBtnLoginState (int state)
	//	{
	//		if (state == 0) {
	//			BtnWechatLogin.GetComponent <Button> ().interactable = false;
	//			BtnQQLogin.GetComponent <Button> ().interactable = false;
	//			nAccoutLogin.GetComponent <Button> ().interactable = false;
	//		} else {
	//			BtnWechatLogin.GetComponent <Button> ().interactable = true;
	//			BtnQQLogin.GetComponent <Button> ().interactable = true;
	//			nAccoutLogin.GetComponent <Button> ().interactable = true;
	//		}
	//	}

	/// <summary>
	/// 安卓游客登录
	/// </summary>
	/// <returns>The login and promptly.</returns>
	//IEnumerator GuestLoginAndPromptly ()
	//{
	//	if (AppInfo.trenchNum == 51000000)
	//	{
	//		SetLoginState(0);
	//		yield return new WaitForSeconds(2f);
	//		ToGuestLogin();
	//		SetLoginState(1);
	//	}
	//}

	/// <summary>
	/// ios游客登录
	/// </summary>
	//IEnumerator GuestLoginIOSPromptly ()
	//{
	//	#if UNITY_IPHONE && !UNITY_EDITOR
	//	if (IsHasAccountLogin () == false && IsHasGuestLogin () == false) {
	//		SetLoginState(0);
	//	}
	//	#endif
	//	yield return new WaitForSeconds (2f);
	//	#if UNITY_IPHONE && !UNITY_EDITOR
	//	AppInfo.phoneUUID = GetLoginUUID ();
	//	StartCoroutine (VerifyWechatAvalible ());
	//	if (IsHasAccountLogin () == false && IsHasGuestLogin () == false) {
	//		ToGuestLogin ();
	//	}
	//	#endif
	//}

	IEnumerator VerifyServerVersion (string url)
	{
		WWW www = new WWW (url);
		yield return www;
		if (null != www.error) {
			//Debug.LogError("VerifyServerVersion1====>:"+www.error);
		} else {
//			Debug.LogError ("VerifyServerVersion23===>:" + www.text);
			try {
				ServerAppData nData = LitJson.JsonMapper.ToObject<ServerAppData> (www.text);
				MyInfo myInfo = DataControl.GetInstance ().GetMyInfo ();
				myInfo.mSvrData = mSvrData = nData;
				//Debug.LogError( "UIAccountLogin.instance.gameObject.SetActive( true );" + nData.payAuthor );
				//Debug.LogError("VerifyServerVersion1====>:" + nData.version);
#if UNITY_IPHONE //正在ios验证阶段
				if (nData.payAuthor == 1) {
					nAccoutLogin.gameObject.SetActive (true);
				}
#else
				myInfo.mSvrData.payAuthor = 0;
#endif
				if (nData.forceUpdate == 1) {
                    if (AppInfo.version < nData.version)
                    {
#if UNITY_IPHONE
                        GameObject Window = Resources.Load ("Window/WindowTips") as GameObject;
						GameObject mCloseTipWin = GameObject.Instantiate (Window);
						UITipClickHideManager ClickTip = mCloseTipWin.GetComponent<UITipClickHideManager> ();
						ClickTip.text.text = "檢測到最新版本，請更新";
						ClickTip.SetClickCallback (() => {
							try {
								GoToAppStore ();
								isUpdateTurnOn = true;
							} catch {
							}
						});
#endif
                    }
                }
			} catch {

			}
		}
	}


	void OpenConnection ()
	{
		SocketToUI socketToUI = SocketToUI.GetInstance ();
		if (null != socketToUI) {
			socketToUI.OpenDispatch ();
		}
		OnLoginPreparing ();
	}

	public void OnLoginPreparing ()
	{
		if (IschangeSever) {
			bIpVerifySuccess = false;
		}
		if (bIpVerifySuccess == false) {
			try {
				//string nOutHostStr = FiSocketSession.CheckNetWorkVersion (mServerUrl);
				bIpVerifySuccess = true;
			} catch {
				bIpVerifySuccess = false;
				Debug.LogError ("[ login ] ip version vering fail!!!");
			}
		}

		//UIHallCore.isFirstComeing = true;
	}

	public void OnLoginAccount ()
	{
		MyInfo myInfo = DataControl.GetInstance().GetMyInfo();
		myInfo.loginAccountType = 0;
		LoginInfo.isWechatLogin = false;
		LoginInfo.isAccoutLogin = true;
		LoginInfo.isQQLogin = false;
		OnLoginPreparing ();
	}

	public void OpenPhoneNumberWindows()
	{
		GameObject window = Resources.Load("Window/OtherLoginFrame") as GameObject;
		GameObject.Instantiate(window);
	}
	public void OnOpenInputPhone()
	{
		GameObject window = Resources.Load("Window/InputPhoneNubmer") as GameObject;
		GameObject.Instantiate(window);
	}

	public void ToGuestLogin ()
	{
		OnLoginPreparing ();
		AudioManager._instance.PlayEffectClip (AudioManager.ui_click);
		if (isUpdateTurnOn)
			return;
		if (LoginUtil.GetIntance () == null)
			return;

		LoginUtil.GetIntance ().SendGuestData ();
		MyInfo myInfo = DataControl.GetInstance ().GetMyInfo ();
		myInfo.isGuestLogin = true;
		myInfo.isLogining = true;
		myInfo.platformType = 10;
		myInfo.loginAccountType = 0;
		//SetAutoLoginState (0);
	}

	/// <summary>
	/// 设置登录状态
	/// </summary>
	/// <param name="loginState">Login state.</param>
	void  SetAutoLoginState (int loginState)
	{
		LoginInfo.isQQLogin = false;
		LoginInfo.isWechatLogin = false;
		LoginInfo.isAccoutLogin = false;
		MyInfo myInfo = DataControl.GetInstance().GetMyInfo();
		switch (loginState) {
		case 0:
			break;
		case 1:
			LoginInfo.isQQLogin = true;
			myInfo.loginAccountType = 2;
			break;
		case 2:
			LoginInfo.isWechatLogin = true;
			myInfo.loginAccountType = 1;
			break;
		case 3:
			LoginInfo.isAccoutLogin = true;
			myInfo.loginAccountType = 0;
			break;
		default:
			break;
		}
		Debug.Log ("LoginInfo.isQQLogin = " + LoginInfo.isQQLogin +
		"LoginInfo.isWechatLogin = " + LoginInfo.isWechatLogin +
		"LoginInfo.isAccoutLogin = " + LoginInfo.isAccoutLogin);
	}

	public void OnQQLogin ()
	{
		OnLoginPreparing ();
		MyInfo myInfo = DataControl.GetInstance ().GetMyInfo ();
		AudioManager._instance.PlayEffectClip (AudioManager.ui_click);
		//FiSocketSession.CheckNetWorkVersion ( ipText.text );
		if (LoginUtil.GetIntance () == null)
			return;
		if (isUpdateTurnOn)
			return;
		//SetAutoLoginState (1);
		myInfo.loginAccountType = 1;
		LoginInfo.isWechatLogin = false;
		LoginInfo.isAccoutLogin = false;
		LoginInfo.isQQLogin = true;
		//AutoQQLoginFile (LoginInfo.pathUrl);

		myInfo.ServerUrl = mServerUrl;
		/*if ( myInfo.isLogining ) 
		{
			GameObject Window1 = UnityEngine.Resources.Load ("Window/WindowTipsThree")as UnityEngine.GameObject;
			GameObject WindowClone1 = UnityEngine.GameObject.Instantiate ( Window1 );
			UITipAutoNoMask ClickTips1 = WindowClone1.GetComponent<UITipAutoNoMask> ();
			ClickTips1.tipText.text = "正在登陆中，请耐心等待...";
			return;
		}Invoke ("ResetLoginState", 3.0f);
		*/
		myInfo.isLogining = true;

		//测试账号激活情况下，使用测试账号激活
		myInfo.isGuestLogin = false;

		LoginUtil.GetIntance ().LoginFromQQ ();
	}

	//wechat login
	public void ToWeChatLogin ()
	{
		
		MyInfo myInfo = DataControl.GetInstance ().GetMyInfo ();
		AudioManager._instance.PlayEffectClip (AudioManager.ui_click);
		//FiSocketSession.CheckNetWorkVersion ( ipText.text );
		if (isUpdateTurnOn)
			return;
		if (LoginUtil.GetIntance () == null)
			return;
		//SetAutoLoginState (2);
		myInfo.loginAccountType = 2;
		LoginInfo.isWechatLogin = true;
		LoginInfo.isAccoutLogin = false;
		LoginInfo.isQQLogin = false;
		//AutoWeChatLogin (LoginInfo.pathUrl);

		myInfo.ServerUrl = mServerUrl;
		/*if ( myInfo.isLogining ) {
			GameObject Window1 = UnityEngine.Resources.Load ("Window/WindowTipsThree")as UnityEngine.GameObject;
			GameObject WindowClone1 = UnityEngine.GameObject.Instantiate ( Window1 );
			UITipAutoNoMask ClickTips1 = WindowClone1.GetComponent<UITipAutoNoMask> ();
			ClickTips1.tipText.text = "正在登陆中，请耐心等待...";
			return;
		}
		Invoke ("ResetLoginState", 3.0f);
		*/
		myInfo.isLogining = true;
		//测试账号激活情况下，使用测试账号激活
		myInfo.isGuestLogin = false;
		LoginUtil.GetIntance ().LoginFromWeChat ();

		UIHallCore.isFirstComeing = true;
	}

	public void ChangeOtherLoginVisable(string v)
    {
		if (v.Equals("1"))
		{
			// 泰山 2020/11/25 取消其他登入按鈕
			//BtnOtherLogin.SetActive(true);
		}
		else {
			BtnOtherLogin.SetActive(false);
        }
    }



	#if UNITY_ANDROID
	public void AndroidGetUUID (string uid)
	{
		AppInfo.phoneUUID = uid;
		Debug.Log ("AppInfo.phoneUUID = " + AppInfo.phoneUUID);
	}
	#endif

	void ResetLoginState ()
	{
		MyInfo myInfo = DataControl.GetInstance ().GetMyInfo ();
		myInfo.isLogining = false;
	}

	public void OnOpenServiceWindow ()
	{
		AudioManager._instance.PlayEffectClip (AudioManager.ui_click);
		string path = "Window/ServerWindow";
		GameObject WindowClone = AppControl.OpenWindow (path);
		WindowClone.SetActive (true);
	}

	void OnGUI ()
	{
		//GUI.Label (new Rect (Screen.width - 80, Screen.height - 40, 100, 200), "v" + AppInfo.appVersion.ToString ());
	}

	void OnDestroy ()
	{
		Instance = null;
		AppToApp = null;
	}


	#if UNITY_IPHONE
	[DllImport ("__Internal")]
	public static extern string GetLoginUUID ();

	[DllImport ("__Internal")]
	public static extern void GoToAppStore ();

	[DllImport ("__Internal")]
	public static extern bool IsHasAccountLogin ();

	[DllImport ("__Internal")]
	public static extern bool ToSetAccountLogin ();

	[DllImport ("__Internal")]
	public static extern bool IsHasGuestLogin ();

	[DllImport ("__Internal")]
	public static extern bool ToSetGuestLogin ();

	[DllImport ("__Internal")]
	public static extern void GetIosAppInfo ();
	#endif
	#if UNITY_ANDROID
	//oppo登录
	public void OnOPPOLoginClick ()
	{
		Debug.Log ("OnOPPOLoginClick=========");
		UIOppoLogin uIOppoLogin = UIOppoLogin.Instance; 

		if (null != uIOppoLogin) {
			Debug.Log ("OnOPPOLoginClick：" + "opop登录了");
			uIOppoLogin.SetServerUrl (mServerUrl);
			uIOppoLogin.OnOppoLogin ();
		}
	}

	#endif
}
