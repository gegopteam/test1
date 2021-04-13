using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.IO;
using System.Text;
using AssemblyCSharp;
using System.Runtime.InteropServices;
using LitJson;
using cn.sharesdk.unity3d;
using System;
using System.Security.Cryptography;
using System.Threading;
using AssemblyCSharp;

public class UnionidInfo
{
	public string client_id;
	public string openid;
	public string unionid;
}

public class LoginUnit
{
	public int result;

	public int userid;

	public string username;

	public string passwd;

	public string token;

	public void DoConvert (string nInputData)
	{
		try {
			result = int.Parse (nInputData.Substring (0, 1));//
			if (result != 0) {
				Debug.LogError ("[ -----------GetLoginResponse------------ ] obtain success ");

				int nIndexStart = 2;
				int nIndexEnd = nInputData.IndexOf ("|", 1);
				string nUserId = nInputData.Substring (nIndexStart, nIndexEnd - nIndexStart);

				userid = int.Parse (nUserId);
				Debug.LogError ("[ -----------GetLoginResponse------------ ] nUserId " + nUserId);

				nIndexStart = nIndexEnd + 1;
				nIndexEnd = nInputData.IndexOf ("|", nIndexStart);
				username = nInputData.Substring (nIndexStart, nIndexEnd - nIndexStart);
				Debug.LogError ("[ -----------GetLoginResponse------------ ] nUserName " + username);

				nIndexStart = nIndexEnd + 1;
				nIndexEnd = nInputData.IndexOf ("|", nIndexStart);
				passwd = nInputData.Substring (nIndexStart, nIndexEnd - nIndexStart);

				nIndexStart = nIndexEnd + 1;
				nIndexEnd = nInputData.IndexOf ("|", nIndexStart);
				token = nInputData.Substring (nIndexStart, nIndexEnd - nIndexStart);
				Debug.LogError ("[ -----------GetLoginResponse------------ ] passwd " + passwd + " - token " + token);
			}
		} catch {
		
		}
	}
}


public class LoginUtil : MonoBehaviour
{

	private static LoginUtil mInstance = null;

	public ShareSDK ssdk;

	public MyInfo userInfo;
   
	public MyInfo APPToAPPUserInfo = new MyInfo ();

	public bool Ishare = false;

	public  bool IsNoteQqorWet = true;
	public  bool IsWXAPPDown = true;

	public delegate void  ShareSucceed ();

	public delegate void  OnShowSignChange ();

	public OnShowSignChange onshowsignchange;
	public ShareSucceed sharesd;
	string CLLoginUrl;
	

	string MobileRegisterKey = "E39,D-=SM39S10-ATU85NC,53\\kg9";

	void Start ()
	{
		LoginInfo.isQQLogin = false;
		LoginInfo.isWechatLogin = false;
		LoginInfo.isAccoutLogin = false;
		StartCoroutine(delayStart());
		//CLLoginUrl = UIUpdate.WebUrlDic[WebUrlEnum.ThirdToken];
		//InitShareSdk ();
		//mInstance = this;
	}

	IEnumerator delayStart()
	{
		yield return new WaitForSeconds(5.0f);
		try {
			CLLoginUrl = UIUpdate.WebUrlDic[WebUrlEnum.ThirdToken];
			InitShareSdk();
			mInstance = this;
		}
		catch (Exception ex) {
			Debug.Log(ex.ToString());
			StartCoroutine(delayStart());
		}
		
	}

	public bool isWeChatValid ()
	{
		if (ssdk == null) {
			return false;
		}
		try {
			if (ssdk.IsClientValid (PlatformType.WeChat)) {
				return true;
			}
		} catch {
		
		}
		return false;
	}

	public static string GetMD5 (string myString)
	{
		byte[] result = Encoding.Default.GetBytes (myString);    //tbPass为输入密码的文本框
		MD5 md5 = new MD5CryptoServiceProvider ();
		byte[] output = md5.ComputeHash (result);
		string str = BitConverter.ToString (output).Replace ("-", "");
		if (str.Length != 32)
			Debug.LogError ("--------------開始調試--------------");
		return str;   
	}
		


	/*wechat login param*/
	//string nTestToken = "7_TFTInwiTzK5gufyaaxl9R1ZsFkXjaG_lL_3DBDGXhRiqIKZc8wWP1G7P7HA0x4E1B2wbwa_QYTBd93Yp_s1ZJCMNGsXHylqDySB14MEtjvY";
	//string nUnionId   = "o8zcZ1DMGXYunpc09z7Aw4skMVdA";//"olLyauEj0griyrJ6U06lJUAm0Icg";//"o8zcZ1DMGXYunpc09z7Aw4skMVdA";

	//string nQQToken = "EAB83138C053274F8B6AABA34E910A60";
	//string nQQId   = "C5F0247CC3EE98D82D50854164F376B6";//"olLyauEj0griyrJ6U06lJUAm0Icg";//"o8zcZ1DMGXYunpc09z7Aw4skMVdA";

	string GetMd5SignData (string oauth, string openid, string token, string sex, string mac)
	{
		return GetMD5 (oauth + openid + token + sex + "" + LoginMsgHandle.getChannelNumber ().ToString () + "" + "0" + mac + MobileRegisterKey).ToLower ();
	}

	WWWForm GetWechatPostData (string nOpenId, string nToken, int sex)
	{
		WWWForm nData = new WWWForm ();
		nData.AddField ("openid", nOpenId);
		nData.AddField ("oauth", 3);
		nData.AddField ("token", nToken);
		nData.AddField ("nick", userInfo.nickname);
		nData.AddField ("sex", sex);
		nData.AddField ("mid", LoginMsgHandle.getChannelNumber ().ToString ());
		nData.AddField ("sid", "0");

		string nMacId = SystemInfo.deviceUniqueIdentifier;
		
		#if UNITY_IPHONE && !UNITY_EDITOR
		nMacId = UILogin.GetLoginUUID();
		#endif
		nData.AddField ("mac", nMacId);
		string nSignResult = GetMd5SignData ("3", nOpenId, nToken, sex.ToString (), nMacId); //"3" + nOpenId + "" + nToken + "1"+ "00" + nMacId + "" + MobileRegisterKey;
		nData.AddField ("md5", nSignResult);
		nData.AddField ("type", 8);
		return nData;
	}

	//Random ran = new System.Random ();

	#region 新游客预留接口

	//	int ranNum = 1;
	//
	//
	//	public void GuestSaveDeviceID (string deviceID)
	//	{
	//		if (!string.IsNullOrEmpty (deviceID)) {
	//			AppInfo.GuestDeviceID = deviceID;
	//			Debug.Log ("AppInfo.GuestDeviceID = " + AppInfo.GuestDeviceID);
	//		}
	//	}

	//	WWWForm GetGuestPostData ()
	//	{
	//		//int i = UnityEngine.Random.Range (0, ranNum);
	//		string nMacId = "";
	//		//string nMacId = SystemInfo.deviceUniqueIdentifier;
	//		if (AppInfo.GuestDeviceID == null) {
	//			nMacId = SystemInfo.deviceUniqueIdentifier + 2;
	//		} else {
	//			nMacId = AppInfo.GuestDeviceID;
	//		}
	//		#if UNITY_ANDROID&&!UNITY_EDITOR
	//		AndroidJavaClass jc = new AndroidJavaClass ("com.unity3d.player.UnityPlayer");
	//		AndroidJavaObject jo = jc.GetStatic<AndroidJavaObject> ("currentActivity");
	//		jo.Call ("GetGuestLoginSave", nMacId);
	//		#endif
	//
	//
	//		Debug.Log ("nMacID SystemInfo.deviceUniqueIdentifier = " + nMacId);
	//		#if UNITY_IPHONE && !UNITY_EDITOR
	//		//nMacId = UILogin.GetLoginUUID();
	//		#endif
	//		WWWForm nData = new WWWForm ();
	//		nData.AddField ("openid", nMacId);
	//		nData.AddField ("oauth", 0);
	//		nData.AddField ("token", nMacId);
	//		nData.AddField ("nick", "guest");
	//		nData.AddField ("sex", "1");
	//		nData.AddField ("mid", LoginMsgHandle.getChannelNumber ().ToString ());
	//		nData.AddField ("sid", "0");
	//
	//		nData.AddField ("mac", nMacId);
	//		//GetMd5SignData ( "0" , nMacId , nMacId , "1" , nMacId );
	//		string nSignResult = GetMd5SignData ("0", nMacId, nMacId, "1", nMacId);// "0" + nMacId + "" + nMacId + "00" + nMacId + "" + MobileRegisterKey;
	//		nData.AddField ("md5", nSignResult);
	//		nData.AddField ("type", 8);
	//		return nData;
	//	}

	#endregion

	//新游客添加
	WWWForm GetGuestPostData ()
	{
		string nMacId = SystemInfo.deviceUniqueIdentifier;
		#if UNITY_IPHONE && !UNITY_EDITOR
		//nMacId = UILogin.GetLoginUUID();
		#endif
		WWWForm nData = new WWWForm ();
		nData.AddField ("openid", nMacId);
		nData.AddField ("oauth", 0);
		nData.AddField ("token", nMacId);
		nData.AddField ("nick", "guest");
		nData.AddField ("sex", "1");
		nData.AddField ("mid", LoginMsgHandle.getChannelNumber ().ToString ());
		nData.AddField ("sid", "0");

		nData.AddField ("mac", nMacId);
		//GetMd5SignData ( "0" , nMacId , nMacId , "1" , nMacId );
		string nSignResult = GetMd5SignData ("0", nMacId, nMacId, "1", nMacId);// "0" + nMacId + "" + nMacId + "00" + nMacId + "" + MobileRegisterKey;
		nData.AddField ("md5", nSignResult);
		nData.AddField ("type", 8);
		return nData;
	}

	//app跳转用户添加
	WWWForm GetAPPToAPPPostData (int userId, string nToken, string nickname, string avtor)
	{
		string nMacId = SystemInfo.deviceUniqueIdentifier;
		WWWForm nData = new WWWForm ();
		//nData.AddField("openid", nOpenId);
		nData.AddField ("oauth", 0);
		nData.AddField ("token", nToken);
		nData.AddField ("nick", "guest");
		nData.AddField ("sex", "1");
		nData.AddField ("mid", LoginMsgHandle.getChannelNumber ().ToString ());
		nData.AddField ("sid", "0");

		nData.AddField ("mac", nMacId);
		//GetMd5SignData ( "0" , nMacId , nMacId , "1" , nMacId );
		//string nSignResult = GetMd5SignData("0", nOpenId, nToken, "1", nMacId);// "0" + nMacId + "" + nMacId + "00" + nMacId + "" + MobileRegisterKey;
		//nData.AddField("md5", nSignResult);
		nData.AddField ("type", 8);
		return nData;
	}

	WWWForm GetQQPostData (string nOpenId, string nToken, int sex)
	{
		string nMacId = SystemInfo.deviceUniqueIdentifier;
		#if UNITY_IPHONE && !UNITY_EDITOR
		nMacId = UILogin.GetLoginUUID();
		#endif
		WWWForm nData = new WWWForm ();
		nData.AddField ("openid", nOpenId);

		string nOauth = "25";
		// string nOauth = "27";
		#if UNITY_IPHONE
		nOauth = "24";
		// nOauth = "26";
		#endif
		nData.AddField ("oauth", nOauth);
		nData.AddField ("token", nToken);
		nData.AddField ("nick", userInfo.nickname);
		nData.AddField ("sex", sex);
		nData.AddField ("mid", LoginMsgHandle.getChannelNumber ().ToString ());
		nData.AddField ("sid", "0");

		nData.AddField ("mac", nMacId);
		string nSignResult = GetMd5SignData (nOauth, nOpenId, nToken, sex.ToString (), nMacId);//"1" + nOpenId + "" + nToken + "1"+ "00" + nMacId + "" + MobileRegisterKey;
		nData.AddField ("md5", nSignResult);
		nData.AddField ("type", 8);


//		Tool.FileWrite ( Application.persistentDataPath + "/test.txt" , "[-- "+nOauth + " \r\n--/-- " + nToken + " \r\n--/-- " + nOpenId + " \r\n--/-- "  +  nMacId + "--]" ); 
		return nData;
	}

	public void SendWechatData ()
	{
		StartCoroutine (GetLoginResponse (CLLoginUrl, GetWechatPostData (userInfo.openId, userInfo.acessToken, userInfo.sex)));
	}

	public void SendQQData ()
	{
		//userInfo.openId = nQQId;
		//userInfo.acessToken = nQQToken;
		// Debug.LogError("---SendQQData url:"+CLLoginUrl);
		// Debug.LogError("---SendQQData openId:"+userInfo.openId);
		// Debug.LogError("---SendQQData token:"+userInfo.acessToken);
		// Debug.LogError("---SendQQData sex:"+userInfo.sex);
		StartCoroutine (GetLoginResponse (CLLoginUrl, GetQQPostData (userInfo.openId, userInfo.acessToken, userInfo.sex)));
	}

	/// <summary>
	/// APP跳转请求wwwdata(Facebook)
	/// </summary>
	public void SendAPPToAPPData ()
	{
		//StartCoroutine(GetLoginResponse(CLLoginUrl, GetAPPToAPPPostData(userInfo.userID, userInfo.acessToken,userInfo.nickname,userInfo.avatar)));

		DoConnectAndSendLoginForAPP ();
	}

	/// <summary>
	/// 仅APP跳转登录调用
	/// </summary>
	/// <param name="ID">Identifier.</param>
	/// <param name="token">Token.</param>
	/// <param name="nickname">Nickname.</param>
	/// <param name="avatar">Avatar.</param>
	public void SetUserInfo (string ID, string token, string nickname, string avatar)
	{
		int a = 0;
		APPToAPPUserInfo.userID = int.TryParse (ID, out a) ? (int.Parse (ID)) : 0;
		APPToAPPUserInfo.acessToken = token;
		APPToAPPUserInfo.nickname = nickname;
		APPToAPPUserInfo.avatar = avatar;
		APPToAPPUserInfo.openId = "APPToAPP";
		Debug.LogError ("賦值完成~~~~~~~~");
	}

	void ToGetUnionid (string acessToken)
	{
		Debug.LogError ("---ToGetUnionid token:" + acessToken);
		if (!string.IsNullOrEmpty (acessToken)) {
			string url = "https://graph.qq.com/oauth2.0/me?access_token=" + userInfo.acessToken + "&unionid=1";
			StartCoroutine (GetUnionid (url, userInfo));
		}
	}

	IEnumerator GetUnionid (string url, MyInfo userInfo)
	{
		WWW www = new WWW (url);
		yield return www;
		if (null != www.error) {
			Debug.LogError ("GetUnionid====> error:" + www.error);
		} else {
			Debug.LogError ("GetUnionid====>:" + www.text);
			string strTmp = www.text;
			string[] strArray = strTmp.Split (new string[] { "callback(", ");" }, StringSplitOptions.RemoveEmptyEntries);
			Debug.LogError ("GetUnionid====>result:" + strArray [0]);
			UnionidInfo dataInfo = LitJson.JsonMapper.ToObject<UnionidInfo> (strArray [0]);
			// userInfo.openId = dataInfo.openid;
			// userInfo.unionid = dataInfo.unionid;
			userInfo.openId = dataInfo.unionid;
			Debug.LogError ("GetUnionid====> openId:" + dataInfo.unionid);
			SendQQData ();
		}
	}

	public void SendGuestData ()
	{
		ShowWaitingView (true);
		userInfo.openId = "guest";
		StartCoroutine (GetLoginResponse (CLLoginUrl, GetGuestPostData ()));
	}

	IEnumerator GetLoginResponse (string url, WWWForm data)
	{
		WWW www = new WWW (url, data);
		yield return www;

		if (string.IsNullOrEmpty (www.error)) {
			//Debug.Log ("IEnumerator 11123");
			Debug.LogError ("[ -----------GetLoginResponse22222------------ ]" + www.text);
			LoginUnit nUnit = new LoginUnit ();
			nUnit.DoConvert (www.text);
			userInfo.mLoginData = nUnit;
			//userInfo.openId = "thirdpart";
			if (nUnit.result != 0) {
				if (userInfo.isGuestLogin) {
					userInfo.nickname = nUnit.username;
					//Debug.LogError("[ -----------GetLoginResponse22222------------ ]" + userInfo.nickname);
				}
				userInfo.connectServerAlr = false;
				userInfo.isconnecting = false;
				DoConnectAndSendLogin ();
			} else {
				ShowWaitingView (false);
				DeleteUtil ();
				GameObject Window1 = UnityEngine.Resources.Load ("Window/WindowTipsThree")as UnityEngine.GameObject;
				GameObject WindowClone1 = UnityEngine.GameObject.Instantiate (Window1);
				UITipAutoNoMask ClickTips1 = WindowClone1.GetComponent<UITipAutoNoMask> ();
				ClickTips1.tipText.text = "錯誤的登錄數據";
			}
		} else {
			Debug.LogError ("[ -----------GetLoginResponse error------------ ]" + www.error);
		}
	}




	private void InitShareSdk ()
	{
		ssdk = gameObject.GetComponent<ShareSDK> ();
		ssdk.authHandler = OnAuthResultHandler;
		ssdk.showUserHandler = OnGetUserInfoResultHandler;
		ssdk.shareHandler = OnShareResultHandler;
		userInfo = DataControl.GetInstance ().GetMyInfo ();
//		if (UILogin.Instance != null) {
//			UILogin.Instance.OnLoginState ( isWeChatValid() );
//		}
	}

	public static LoginUtil GetIntance ()
	{
		return mInstance;
	}

	void OnDestroy ()
	{
		mInstance = null;
	}

	public void JumpLoadingView ()
	{
		userInfo.TargetView = AppView.HALL;
		AppControl.ToView (AppView.LOADING);
	}

	GameObject mWaitingView;

	public void ShowWaitingView (bool nValue)
	{
		if (AppControl.miniGameState) {
		} else {
			if (nValue) {
				GameObject Window1 = UnityEngine.Resources.Load ("MainHall/Common/WaitingView")as UnityEngine.GameObject;
				mWaitingView = UnityEngine.GameObject.Instantiate (Window1);
				UIWaiting nData = mWaitingView.GetComponentInChildren<UIWaiting> ();
				nData.HideBackGround ();
				//nData.SetInfo (15.0f, "获取登录信息异常，请重新尝试");
			} else {
				Destroy (mWaitingView);
				mWaitingView = null;
			}
		}
	}

	static GameObject maskObj;

	/// <summary>
	/// 设置遮罩
	/// </summary>
	/// <param name="isShow">If set to <c>true</c> is show.</param>
	public static void ShowWaitingMask (bool isShow)
	{
		string path = "MainHall/Common/WaitingMask";
		if (isShow) {
			maskObj = AppControl.OpenWindow (path);
			maskObj.SetActive (true);
		} else {
			if (maskObj != null && maskObj.activeSelf) {
				Destroy (maskObj);
			}
			maskObj = null;
		}
	}

	public void LoginFromWeChat ()
	{
		try {
			ShowWaitingView (true);
			if (ssdk.IsAuthorized (PlatformType.WeChat)) {
				loadUserInfo (PlatformType.WeChat);
			} else {
				ssdk.Authorize (PlatformType.WeChat);
			}
		} catch (Exception e) {
			DeleteUtil ();
			GameObject Window1 = UnityEngine.Resources.Load ("Window/WindowTipsThree")as UnityEngine.GameObject;
			GameObject WindowClone1 = UnityEngine.GameObject.Instantiate (Window1);
			UITipAutoNoMask ClickTips1 = WindowClone1.GetComponent<UITipAutoNoMask> ();
			ClickTips1.tipText.text = "授權失敗";
			ShowWaitingView (false);
			Debug.Log (e);
		}
	}

	public void LoginWithAccount (string nAccount, string nPassword)
	{
        Debug.Log("bbbbb  LoginWithAccount");
        ShowWaitingView (true);
		userInfo.account = nAccount;
//		Debug.Log ("LoginWithAccount Account = " + userInfo.account);
		userInfo.password = GetMD5 (nPassword).ToLower ();
		userInfo.openId = "account";
		userInfo.connectServerAlr = false;
		userInfo.isconnecting = false;
		DoConnectAndSendLogin ();
	}

	public void AutoLoginWitnAccount (string nAccount, string nPassword)
	{
        Debug.Log("bbbbb  AutoLoginWitnAccount");
        ShowWaitingView (true);
		userInfo.account = nAccount;
//		Debug.Log ("LoginWithAccount Account = " + userInfo.account);
		userInfo.password = nPassword;
		userInfo.openId = "account";
		userInfo.connectServerAlr = false;
		userInfo.isconnecting = false;
		DoConnectAndSendLogin ();
	}

	public void AutoLoginWitnAccountForWeb(string nAccount, string nPassword)
	{
		Debug.Log("bbbbb");
		ShowWaitingView(true);
		userInfo.account = nAccount;
		//		Debug.Log ("LoginWithAccount Account = " + userInfo.account);
		userInfo.password = nPassword;
		userInfo.openId = "account";
		//DoConnectAndSendLogin();
		DoConnectAndSendLoginForRelogin();
	}

	public void LoginFromQQ ()
	{
		try {
			ShowWaitingView (true);
			if (ssdk.IsAuthorized (PlatformType.QQ)) {
				loadUserInfo (PlatformType.QQ);
			} else {
				ssdk.Authorize (PlatformType.QQ);
			}
		} catch (Exception e) {
			DeleteUtil ();
			GameObject Window1 = UnityEngine.Resources.Load ("Window/WindowTipsThree")as UnityEngine.GameObject;
			GameObject WindowClone1 = UnityEngine.GameObject.Instantiate (Window1);
			UITipAutoNoMask ClickTips1 = WindowClone1.GetComponent<UITipAutoNoMask> ();
			ClickTips1.tipText.text = "QQ授權失敗";
			ShowWaitingView (false);
			Debug.Log (e);
		}
	}

	public void CancelAuthorize ()
	{
		DataControl.GetInstance ().ShutDown ();
		try {
			if (ssdk.IsAuthorized (PlatformType.WeChat)) {
				ssdk.CancelAuthorize (PlatformType.WeChat);
			} else if (ssdk.IsAuthorized (PlatformType.QQ)) {
				ssdk.CancelAuthorize (PlatformType.QQ);
			}
		
		} catch (Exception e) {
			Debug.LogError ("[ login util ] cacnel login error " + e);
		}
	}
	//判断是否下载微信app
	public  void IsDown ()
	{
		#if UNITY_ANDROID
		AndroidJavaClass jc = new AndroidJavaClass ("com.unity3d.player.UnityPlayer");
		AndroidJavaObject jo = jc.GetStatic<AndroidJavaObject> ("currentActivity");
		IsWXAPPDown = jo.Call<bool> ("IsDownWX");
		#elif UNITY_IPHONE
		IsWXAPPDown = IsDownWX ();
		#endif

//		Debug.LogError ("ishow____" + IsWXAPPDown);
	}

	private void loadUserInfo (PlatformType type)
	{
		if (type == PlatformType.QQ || type == PlatformType.WeChat) {
			IsNoteQqorWet = false;
			Debug.LogError ("_____________________________sdk____________________________");
		}

		ssdk.GetUserInfo (type);
		// 有效字段 (string)token , (string)uid , (bool)available
		Hashtable result = ssdk.GetAuthInfo (type);
		userInfo.acessToken = Convert.ToString (result ["token"]);
		userInfo.openId = Convert.ToString (result ["uid"]);
		if (string.IsNullOrEmpty (userInfo.openId)) {
			userInfo.openId = Convert.ToString (result ["userID"]);
		}



//		Tool.FileWrite ( Application.persistentDataPath + "/test1.txt" , "[-- " +  MiniJSON.jsonEncode (result) + "--]" ); 
		print ("\r\n\r\n*********************loadUserInfo1*********************" + MiniJSON.jsonEncode (result) + " \r\n\r\n ");
		//myInfo.state        = MyInfo.StateInfo.SUCCESS;
	}

	public void OnShowService ()
	{
		string path = "Window/ServerWindow";
		GameObject WindowClone = AppControl.OpenWindow (path);
		WindowClone.SetActive (true);
	}

	//不会给出nickname ，access_token ， openid , 统一到 geiuserInfo 回调后处理
	void OnAuthResultHandler (int reqID, ResponseState state, PlatformType type, Hashtable result)
	{
		switch (state) {
		case ResponseState.Success:
			print ("\r\n\r\n*********************OnAuthResultHandler*********************" + MiniJSON.jsonEncode (result) + " \r\n\r\n ");
			loadUserInfo (type);
			break;
		case ResponseState.Fail:
			{//	myInfo.state        = MyInfo.StateInfo.FAIL;
				DeleteUtil ();
				ShowWaitingView (false);
				GameObject Window1 = UnityEngine.Resources.Load ("Window/WindowTipsThree")as UnityEngine.GameObject;
				GameObject WindowClone1 = UnityEngine.GameObject.Instantiate (Window1);
				UITipAutoNoMask ClickTips1 = WindowClone1.GetComponent<UITipAutoNoMask> ();
				ClickTips1.tipText.text = "授權失敗";
			}
			break;
		case ResponseState.Cancel:
			{//	myInfo.state        = MyInfo.StateInfo.CANCEL;
				DeleteUtil ();
				ShowWaitingView (false);
				GameObject Window1 = UnityEngine.Resources.Load ("Window/WindowTipsThree")as UnityEngine.GameObject;
				GameObject WindowClone1 = UnityEngine.GameObject.Instantiate (Window1);
				UITipAutoNoMask ClickTips1 = WindowClone1.GetComponent<UITipAutoNoMask> ();
				ClickTips1.tipText.text = "授權取消";
			}
			break;
		}
		/**if (state == ResponseState.Success)
		{
			print ("authorize success !" + "Platform :" + type);
		}
		else if (state == ResponseState.Fail)
		{
			#if UNITY_ANDROID
			print ("fail! throwable stack = " + result["stack"] + "; error msg = " + result["msg"]);
			#elif UNITY_IPHONE
			print ("fail! error code = " + result["error_code"] + "; error msg = " + result["error_msg"]);
			#endif
		}
		else if (state == ResponseState.Cancel) 
		{
			print ("cancel !");
		}*/
	}

	public bool isEmoji (char hs, char ls)
	{
		if (0xd800 <= hs && hs <= 0xdbff) {
			int uc = ((hs - 0xd800) * 0x400) + (ls - 0xdc00) + 0x10000;
			if (0x1d000 <= uc && uc <= 0x1f77f) {
				return true;
			}
		} else {
			if (0x2100 <= hs && hs <= 0x27ff && hs != 0x263b) {
				return true;
			} else if (0x2B05 <= hs && hs <= 0x2b07) {
				return true;
			} else if (0x2934 <= hs && hs <= 0x2935) {
				return true;
			} else if (0x3297 <= hs && hs <= 0x3299) {
				return true;
			} else if (hs == 0xa9 || hs == 0xae || hs == 0x303d || hs == 0x3030 || hs == 0x2b55 || hs == 0x2b1c || hs == 0x2b1b || hs == 0x2b50 || hs == 0x231a) {
				return true;
			}
			if (ls == 0x20e3) {
				return true;
			}
		}
		return  false;
	}

	void DoConnectAndSendLoginForAPP_ ()
	{
		Debug.Log("----------DoConnectAndSendLoginForAPP----------");
		userInfo.isLogining = true;
		//JumpLoadingView ();
		DataControl.GetInstance ().ConnectSvr (userInfo.ServerUrl, AppInfo.portNumber);
		Invoke ("ResetLoginState", 3.0f);
	}

	void DoConnectAndSendLoginForAPP()
	{
		Debug.Log("----------DoConnectAndSendLoginForAPP----------");
		userInfo.isLogining = true;
		//JumpLoadingView ();
		DataControl.GetInstance().ConnectSvr(userInfo.ServerUrl, AppInfo.portNumber);
		Invoke("ResetLoginState", 3.0f);
	}

	public void DoConnectAndSendLoginForRelogin ()
	{
		if (string.IsNullOrEmpty (userInfo.openId)) {
			DeleteUtil ();
			GameObject Window = UnityEngine.Resources.Load ("Window/WindowTipsThree")as UnityEngine.GameObject;
			GameObject WindowClone = UnityEngine.GameObject.Instantiate (Window);
			UITipAutoNoMask ClickTips1 = WindowClone.GetComponent<UITipAutoNoMask> ();
			ClickTips1.tipText.text = "無法獲取用戶信息，請重試";
			CancelAuthorize ();
			return;
		}

		userInfo.isLogining = true;
		//JumpLoadingView ();
		DataControl.GetInstance ().ConnectSvr (userInfo.ServerUrl, AppInfo.portNumber);
		Invoke ("ResetLoginState", 3.0f);
		Debug.Log ("=======DoConnectAndSendLoginForRelogin=====");
	}
	
	void DoConnectAndSendLogin () 
	{
		Debug.Log("----------DoConnectAndSendLogin----------");
		if (string.IsNullOrEmpty(userInfo.openId))
		{
			DeleteUtil();
			GameObject Window = UnityEngine.Resources.Load("Window/WindowTipsThree") as UnityEngine.GameObject;
			GameObject WindowClone = UnityEngine.GameObject.Instantiate(Window);
			UITipAutoNoMask ClickTips1 = WindowClone.GetComponent<UITipAutoNoMask>();
			ClickTips1.tipText.text = "無法獲取用戶信息，請重試";
			CancelAuthorize();
			return;
		}
		userInfo.connectServerAlr = false;
		userInfo.isconnecting = false;
		//string[] tempURL = userInfo.ServerURLArray1;
		//System.Random rnd = new System.Random();
		//for (int i = 0; i < tempURL.Length; i++)
		//{
		//	string temp = tempURL[i];
		//	int randomIndex = rnd.Next(0, tempURL.Length);
		//	tempURL[i] = tempURL[randomIndex];
		//	tempURL[randomIndex] = temp;
		//}

		StartCoroutine(ConnectRandomServerUrlLoop(userInfo.RandomServerURL));
	}

	public void ForStopIE() {
		userInfo.connectServerAlr = true;
		userInfo.isconnecting = true;
		StopCoroutine(ConnectRandomServerUrlLoop(userInfo.RandomServerURL));
    }

	IEnumerator ConnectRandomServerUrlLoop(string[] Loginurl_Array)
	{
		int index1 = Loginurl_Array.Length;
		int index2 = 0;
		Debug.Log("----------DoConnectAndSendLogin---------- " + index1 + " =======connectServerAlr===== " + userInfo.connectServerAlr + " =======isconnecting===== " + userInfo.isconnecting);
		
		while (!userInfo.connectServerAlr)
		{
			Debug.Log("=======1connectServerAlr===== " + userInfo.connectServerAlr + " =======1isconnecting===== " + userInfo.isconnecting);
			if (index2 < index1 && !userInfo.isconnecting)
			{
                ShowWaitingView(false);
				ShowWaitingView(true);
				userInfo.ServerUrl = Loginurl_Array[index2];
				
				string whosip = FiSocketSession.CheckNetWorkVersion(userInfo.ServerUrl);

				//Debug.Log("[ ConnectRandomServerUrlLoop ] Server URL :" + userInfo.ServerUrl);
				//try
				//{
				//	whosip = FiSocketSession.CheckNetWorkVersion(userInfo.ServerUrl);
				//}
				//catch (Exception e)
				//{
				//	userInfo.isconnecting = false;
				//}
				//Debug.Log("=======ServerUrl=====" + userInfo.ServerUrl);
				DataControl.GetInstance().ConnectSvr(userInfo.ServerUrl, AppInfo.portNumber);
				index2++;
			}
            
			yield return new WaitForSeconds(10.0f);

			if (userInfo.connectServerAlr || index2 >= index1) {
				userInfo.connectServerAlr = true;
				userInfo.isconnecting = true;
				break;
			}
				
			//try
			//{
			//if (!DataControl.GetInstance().check_Connect_State())
			//	userInfo.isconnecting = false;
			//else
			//{
			//	userInfo.isconnecting = true;
			//	userInfo.connectServerAlr = true;
			//}
			//}
			//catch (Exception e)
			//{
			//	Debug.Log(e.ToString());
			//	userInfo.isconnecting = false;
			//}
		}


        ShowWaitingView(false);
		Invoke("ResetLoginState", 3.0f);
		Debug.Log("=======DoConnectAndSendLogin=====");
		yield break;
	}

	public void CancelConnectRandomServerUrl()
	{
		Debug.Log("=======CancelConnectRandomServerUrl=====");
		//StopCoroutine(ConnectRandomServerUrlLoop());
		CancelInvoke("ConnectRandomServerUrl");
	}

	void ResetLoginState ()
	{
		userInfo.isLogining = false;
	}

	public void Capture ()
	{
		ShareContent content = new ShareContent ();
		content.SetText("好基友,好閨蜜，人太多了幫我一起捕魚吧，晚上請你吃魚");
		content.SetTitle("求幫忙！這裡隨時可以捕魚,重點是能換真金白銀，問題是人太多");
		// content.SetTitleUrl ("https://timgsa.baidu.com/timg?image&quality=80&size=b9999_10000&sec=1532582910690&di=0d10c41eec7ad581988d012f5ee52eee&imgtype=0&src=http%3A%2F%2Fa.h​​iphotos.baidu.com%2Fimage%2Fpic%2Fitem %2Fb64543a98226cffceee78e5eb5014a90f703ea09.jpg");
		content.SetUrl("http://down.xinlongbuyu.com/buyu/index.html");
		content.SetComment("分享comment");
		content.SetImageUrl("http://qb.cl0579.com/img/xinlongbuyu.jpg");
		content.SetShareType(ContentType.Webpage);
		//		content.SetShareContentCustomize (null, content);
		ssdk.ShareContent(PlatformType.WeChatMoments, content);
		//		//截屏
		//		Application.CaptureScreenshot ("Shot4Share.png");
		//		//设置图片路径
		//		content.SetImagePath (Application.persistentDataPath + "/Shot4Share.png");
		//		PlatformType[] type=new PlatformType[2];
		//		type [1] = PlatformType.WeChat;
		//		type [2] = PlatformType.WeChatMoments;
		//		ssdk.ShowPlatformList (null , content, 100, 100);
	}


	//以下为回调的定义:
	void OnShareResultHandler (int reqID, ResponseState state, PlatformType type, Hashtable result)
	{
		if (state == ResponseState.Success) {
			Ishare = true;
			GameObject Window1 = UnityEngine.Resources.Load ("Window/WindowTipsThree")as UnityEngine.GameObject;
			GameObject WindowClone1 = UnityEngine.GameObject.Instantiate (Window1);
			UITipAutoNoMask ClickTips1 = WindowClone1.GetComponent<UITipAutoNoMask> ();
			ClickTips1.tipText.text = "分享成功";
			MyInfo myInfo = (MyInfo)Facade.GetFacade ().data.Get (FacadeConfig.USERINFO_MODULE_ID);
			Debug.LogError ("ssssssskkkkkkzzzzzz____________");
			Facade.GetFacade ().message.signIn.SendSignRetroactiveRequest (myInfo.userID, 1, 0, 0, ReSignDay.reday);
			if (sharesd != null) {
				sharesd ();
			}
		} else if (state == ResponseState.Fail) {
			Debug.LogError ("zzzzzzzzzss");
			GameObject Window1 = UnityEngine.Resources.Load ("Window/WindowTipsThree")as UnityEngine.GameObject;
			GameObject WindowClone1 = UnityEngine.GameObject.Instantiate (Window1);
			UITipAutoNoMask ClickTips1 = WindowClone1.GetComponent<UITipAutoNoMask> ();
			ClickTips1.tipText.text = "分享失敗";
		} else if (state == ResponseState.Cancel) {
			Debug.LogError ("kkkkkkkkkkkkkkss");
			GameObject Window1 = UnityEngine.Resources.Load ("Window/WindowTipsThree")as UnityEngine.GameObject;
			GameObject WindowClone1 = UnityEngine.GameObject.Instantiate (Window1);
			UITipAutoNoMask ClickTips1 = WindowClone1.GetComponent<UITipAutoNoMask> ();
			ClickTips1.tipText.text = "分享取消";
		}
	}
	//获取nickname 的唯一入口，包括头像地址  headimgurl ， nickname ， openid ， 没有token
	void OnGetUserInfoResultHandler (int reqID, ResponseState state, PlatformType type, Hashtable result)
	{	
		print ("\r\n\r\n*********************OnGetUserInfoResultHandler*********************" + MiniJSON.jsonEncode (result) + " \r\n\r\n ");
//		Tool.FileWrite ( Application.persistentDataPath + "/test2.txt" , "[-- " +  MiniJSON.jsonEncode (result) + "--]" ); 
		switch (state) {
		case ResponseState.Success:
			string nOrginalName = Convert.ToString (result ["nickname"]);

			if (type == PlatformType.QQ) {
				userInfo.avatar = Convert.ToString (result ["figureurl_qq_2"]);
				string nGender = Convert.ToString (result ["gender"]);
				userInfo.sex = 0;
				if (nGender == "男") {
					userInfo.sex = 1;
				}
			} else {
				userInfo.avatar = Convert.ToString (result ["headimgurl"]);
				userInfo.sex = Convert.ToInt32 (result ["sex"]);
			}

			if (string.IsNullOrEmpty (userInfo.openId)) {
				userInfo.openId = Convert.ToString (result ["openid"]);
			}
			userInfo.loginType = type == PlatformType.WeChat ? 0 : 1;//22 wechat  24 qq

			char[] nDetail = nOrginalName.ToCharArray ();
			for (int i = 0; i < nDetail.Length; i++) {
				//如果便利到末尾了，不判断emoji
				if (i >= nDetail.Length - 1) {
					break;
				}
				bool bEmoji = isEmoji (nDetail [i], nDetail [i + 1]);
				if (bEmoji) {
					nDetail [i] = '?';
					nDetail [i + 1] = '?';
					i++;
				}
			}
			nOrginalName = new string (nDetail);
			userInfo.nickname = nOrginalName;


			if (type == PlatformType.QQ) {
				userInfo.platformType = 24;
				SendQQData ();
				// ToGetUnionid(userInfo.acessToken);
			} else {
				userInfo.platformType = 22;
				SendWechatData ();
			}
			break;
		case ResponseState.Fail:
			{
				DeleteUtil ();
				ShowWaitingView (false);
				GameObject Window1 = UnityEngine.Resources.Load ("Window/WindowTipsThree")as UnityEngine.GameObject;
				GameObject WindowClone1 = UnityEngine.GameObject.Instantiate (Window1);
				UITipAutoNoMask ClickTips1 = WindowClone1.GetComponent<UITipAutoNoMask> ();
				ClickTips1.tipText.text = "授權失敗";
			}
			break;
		case ResponseState.Cancel:
			{
				DeleteUtil ();
				ShowWaitingView (false);
				GameObject Window1 = UnityEngine.Resources.Load ("Window/WindowTipsThree")as UnityEngine.GameObject;
				GameObject WindowClone1 = UnityEngine.GameObject.Instantiate (Window1);
				UITipAutoNoMask ClickTips1 = WindowClone1.GetComponent<UITipAutoNoMask> ();
				ClickTips1.tipText.text = "授權失敗";
			}
			break;
		}
	}

	void DeleteUtil ()
	{
		if (FileUtilManager.IsExistsFile (LoginInfo.pathUrl, "Login.txt")) {
			FileUtilManager.DeleteFile (LoginInfo.pathUrl, "Login.txt");
			SetLoginState ();
			Debug.Log ("DeleteUtil");
		}
	}

	void  SetLoginState ()
	{
		LoginInfo.isQQLogin = false;
		LoginInfo.isWechatLogin = false;
		LoginInfo.isAccoutLogin = false;
		LoginInfo.isAPPToAPPLogin = false;
		LoginInfo.accountStr = "";
		LoginInfo.passwordStr = "";
	}

	#if  UNITY_IPHONE
	[DllImport ("__Internal")]
	public static extern bool IsDownWX ();
	#endif
}
