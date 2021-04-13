using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using LitJson;
using AssemblyCSharp;
using System.Text.RegularExpressions;
using System;

public class UIUpdate : MonoBehaviour
{

	//string url = "http://admin.xinlongbuyu.com/GameAPI/API.aspx?ajaxaction=CheckVersion";
	//string url = "http://admin.xinlongbuyu.com/GameAPI/API.aspx?ajaxaction=CheckVersion_New";
	string url;
	//string[] cd_array = new string[3] {"https://xlconfig.hnxyylh.com/", "https://xlconfig.g272b.com", "https://xlconfig.i272g.com" };
	string[] cd_array = new string[3] { "https://xlconfig.ewppkkq.cn", "https://xlconfig.ewppkkq.cn", "https://xlconfig.ewppkkq.cn" };
	string configuration_domain;
	string configuration_method;
	string Configuration;
	public static ArrayList configuration_backupUrl = new ArrayList();
	public static ArrayList load_config = new ArrayList();
	int url_index = -1;
	//string url = UIUpdate.WebUrlDic[WebUrlEnum.Setting] + "?ajaxaction=CheckVersion_New";
	public static string PayWebUrl = "";

	string url2;
	public bool OpenLogcat;

	public static UIUpdate Instance;
	public GameObject UpdateWindow;
	[HideInInspector]
	public bool isUpdateVersion = false;
	[HideInInspector]
	public bool isLaterShow = false;
	[HideInInspector]
	public string contentText = "";
	[HideInInspector]
	public string urlStr = "";
	MyInfo myInfo;
	public static string userid;

	[HideInInspector]
	public static string geTuiUserID = null;
	public static bool isSaveGeTuiData = true;
	public static Dictionary<WebUrlEnum,string> WebUrlDic = null;
	[HideInInspector]
	public string addrIp = "";

	GameObject mWaitingView;

	void Awake()
	{
		Debug.unityLogger.logEnabled = OpenLogcat;
		Instance = this;
		myInfo = DataControl.GetInstance().GetMyInfo();
		

		//不同渠道 用不同的配置訊息取得下發配置
		if (AppInfo.trenchNum == 51000011)
			configuration_method = "/GameAPI/API.aspx?ajaxaction=CheckVersion_New";
		else if (AppInfo.trenchNum == 51000021)
			configuration_method = "/GameAPI/API.aspx?ajaxaction=checkversion_new1";
		else if (AppInfo.trenchNum == 51000031)
			configuration_method = "/GameAPI/API.aspx?ajaxaction=checkversion_new2";
		else if (AppInfo.trenchNum == 51000041)
			configuration_method = "/GameAPI/API.aspx?ajaxaction=checkversion_new3";
		else if (AppInfo.trenchNum == 51000051)
			configuration_method = "/GameAPI/API.aspx?ajaxaction=checkversion_new4";
		else if (AppInfo.trenchNum == 51000061)
			configuration_method = "/GameAPI/API.aspx?ajaxaction=checkversion_new5";
		else if (AppInfo.trenchNum == 51000071)
			configuration_method = "/GameAPI/API.aspx?ajaxaction=checkversion_new6";
		else if (AppInfo.trenchNum == 51000081)
			configuration_method = "/GameAPI/API.aspx?ajaxaction=checkversion_new7";
		else if (AppInfo.trenchNum == 51000091)
			configuration_method = "/GameAPI/API.aspx?ajaxaction=checkversion_new8";
		else if (AppInfo.trenchNum == 51000101)
			configuration_method = "/GameAPI/API.aspx?ajaxaction=checkversion_new9";
		else if (AppInfo.trenchNum == 51000002) { 
		    configuration_method = "/GameAPI/API.aspx?ajaxaction=CheckVersion1";
		    configuration_method = "/GameAPI/API.aspx?ajaxaction=checkversion_test";
	    }
		else
			configuration_method = "/GameAPI/API.aspx?ajaxaction=CheckVersion";
		url = "https://xlconfig.hhkj2.com" + configuration_method;
		url = "https://xlconfig.ewppkkq.cn" + configuration_method;

		//url = "http://xlconfigt.lhzdbg.cn" + configuration_method;
		Debug.Log(url);
		//url = "https://xlconfig.tsysdsm.cn" + configuration_method;

		WebUrlDic = new Dictionary<WebUrlEnum, string> ();

//		SaveHttpUrl ();
		IsShow ();
	}

	public void IsShow ()
	{
		TimeCount.isShow_1 = false;
		TimeCount.isShow_2 = false;
		TimeCount.isShow_3 = false;
		TimeCount.isGiftBag_1 = false;
		TimeCount.isGiftBag_2 = false;
		TimeCount.isGiftBag_3 = false;
	}

	private void Start ()
	{
		Debug.Log("isQQLogin = "+ LoginInfo.isQQLogin + " ;isWechatLogin = "+ LoginInfo.isWechatLogin + " ;isAccoutLogin = "+ LoginInfo.isAccoutLogin);
		try {
            #if UNITY_ANDROID || UNITY_IPHONE
			    if (FileUtilManager.IsExistsFile(LoginInfo.pathUrl, "Configuration.txt"))
			    {
				    Debug.Log("~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~IsExistsFile  "+ FileUtilManager.IsExistsFile(LoginInfo.pathUrl, "Configuration.txt"));
				    InitConfig(LoginInfo.pathUrl, "Configuration.txt");
			    }
            #else
                Debug.Log("~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~IsExistsFile  "+ FileUtilManager.IsExistsFile(LoginInfo.pathUrl, "Configuration.txt"));
            #endif
		}
		catch (Exception e) {
			Debug.Log(e.ToString());
		}

		Debug.Log("WebUrlDic = "+ WebUrlDic.Count);
		if (WebUrlDic.Count == 0) {
			HttpSend();
		}

		string[] tempURL = myInfo.ServerURLArray1;
		System.Random rnd = new System.Random();
		for (int i = 0; i < tempURL.Length; i++)
		{
			string temp = tempURL[i];
			int randomIndex = rnd.Next(0, tempURL.Length);
			tempURL[i] = tempURL[randomIndex];
			tempURL[randomIndex] = temp;
		}
		myInfo.RandomServerURL = tempURL;
		myInfo.ServerUrl = myInfo.RandomServerURL[0];
		string whosip = FiSocketSession.CheckNetWorkVersion(myInfo.ServerUrl);
		Debug.Log ("游戏的ID=========" + myInfo.userID);
	}

	/// <summary>
	/// 初始化解析
	/// </summary>
	/// <param name="path">Path.</param>
	/// <param name="filename">Filename.</param>
	void InitConfig(string path, string filename)
	{
		string data = FileUtilManager.LoadFile(path, filename);
		if (data == null || data == "")
		{
			return;
		}
		LitJson.JsonData jd = LitJson.JsonMapper.ToObject(data);

		string url = (string)jd["configuration"]["url"];

		string[] url_spilt = url.Split(char.Parse(" "));

		for (int c = 0; c < url_spilt.Length; c++) {
			if (!url_spilt[c].Equals("")) {
				load_config.Add(url_spilt[c]);
				//Debug.Log(url_spilt[c]);
			}
		}
	}

	public string GetIP ()
	{
		using (var webClient = new System.Net.WebClient ()) {
			try {
				webClient.Credentials = System.Net.CredentialCache.DefaultCredentials;
				byte[] pageDate = webClient.DownloadData ("http://pv.sohu.com/cityjson?ie=utf-8");
				string ip = System.Text.Encoding.UTF8.GetString (pageDate);
				webClient.Dispose ();
				Match rebool = Regex.Match (ip, @"\d{2,3}\.\d{1,3}\.\d{1,3}\.\d{1,3}");
				return rebool.Value;
			} catch (Exception e) {
				//Debug.Log(e);
				return null;
			}
		}
	}

	/// <summary>
	/// Post请求
	/// </summary>
	/// <returns>The post.</returns>
	/// <param name="_url">URL.</param>
	/// <param name="_wwwForm">Www form.</param>/
	IEnumerator SendPost (string _url, WWWForm _wwwForm)
	{
		//Debug.LogError("url = "+_url);
		ShowWaitingView(false);
		ShowWaitingView (true);
		yield return StartCoroutine (SaveHttpUrl ());
        StartCoroutine(Backstage());

        //获取到_url 中的数据有 Status   Url   LowVersion      HighVersion    Content 
        WWW postData = new WWW(_url, _wwwForm);
        yield return postData;

        ShowWaitingView(false);

        string content = postData.text;
        //        //Debug.Log("content = " + content);
        JsonData jd = JsonMapper.ToObject(content);

        string low = (string)jd["LowVersion"];
        string cur = AppInfo.appVersion;
        string max = (string)jd["HighVersion"];

		Debug.Log(" 強更比較 ：" + low.CompareTo(cur));
		Debug.Log(" 強更比較 ：" + cur.CompareTo(max));
		//1.等于最大版本,不用弹框提示
		// 2020 / 08 / 23 繁體版取消強更
		if (cur.CompareTo(max) >= 0)
        {
			Debug.Log(" 強更比較結果if");
			isUpdateVersion = false;
            isLaterShow = false;
        }
        //3.状态为2并且小于客户端的版本小于服务器的最低版本,强制更新
        else if ((int)jd["Status"] == 2 && (low.CompareTo(cur) <= 1 && low.CompareTo(cur) > 0))
        {
			Debug.Log(" 強更比較結果else if");
			isUpdateVersion = true;
            isLaterShow = false;
            contentText = (string)jd["Content"];
            urlStr = (string)jd["Url"];
        }
        //2.在最小最大区间内,提示更新,有以后再说
        else
        {
			Debug.Log(" 強更比較結果else");
			isUpdateVersion = true;
            contentText = (string)jd["Content"];
            urlStr = (string)jd["Url"];
            isLaterShow = true;
        }
		StartCoroutine(CheckVersion());
		Debug.Log(" 強更訊息 = "+ contentText);
		//		废弃掉的 
		//		low 比 cur 大 就返回1,相等返回0,小 就返回-1 依次类推
		//		通过Status的0,1,2 进行判断,2为强制更新,1为可更新可不更新,为0时,需要根据LowVersion 进行判断..
		//		当前版本小于LowVersion 版本,那么必须强更
		//		if ((int)jd ["Status"] == 2 || (low.CompareTo (cur) <= 1 && low.CompareTo (cur) > 0) || ((cur.CompareTo (max) < 0) && low.CompareTo (cur) > 0 && (int)jd ["Status"] == 0)) {
		//			isUpdateVersion = true;
		//			contentText = (string)jd ["Content"];
		//			urlStr = (string)jd ["Url"];
		//		} else {
		//			isUpdateVersion = false;
		//		}
	}

	IEnumerator AfterGetHttpUrl()
	{
		WWWForm wwwForm = new WWWForm();
		//1 为 ios  2为 android
        #if UNITY_IPHONE
		    wwwForm.AddField("platformid", 1);
		    StartCoroutine(SendPost(url, wwwForm));
		    //UILogin.CheckVersionEx();
        #elif UNITY_ANDROID
		    wwwForm.AddField ("platformid", 2);
		    StartCoroutine (SendPost (url, wwwForm));
		    //UILogin.CheckVersionEx();
        #endif
		StartCoroutine(Backstage());

		//获取到_url 中的数据有 Status   Url   LowVersion      HighVersion    Content 
		WWW postData = new WWW(url, wwwForm);
		yield return postData;

		ShowWaitingView(false);

		string content = postData.text;
		//        //Debug.Log("content = " + content);
		JsonData jd = JsonMapper.ToObject(content);

		string low = (string)jd["LowVersion"];
		string cur = AppInfo.appVersion;
		string max = (string)jd["HighVersion"];

		Debug.Log("Version :" + low);
		Debug.Log("Version :" + max);
		Debug.Log("Version :" + cur);

		//1.等于最大版本,不用弹框提示
		if (cur.CompareTo(max) >= 0)
		{
			isUpdateVersion = false;
			isLaterShow = false;
		}
		//3.状态为2并且小于客户端的版本小于服务器的最低版本,强制更新
		else if ((int)jd["Status"] == 2 && (low.CompareTo(cur) <= 1 && low.CompareTo(cur) > 0))
		{
			isUpdateVersion = true;
			isLaterShow = false;
			contentText = (string)jd["Content"];
			urlStr = (string)jd["Url"];
		}
		//2.在最小最大区间内,提示更新,有以后再说
		else
		{
			isUpdateVersion = true;
			contentText = (string)jd["Content"];
			urlStr = (string)jd["Url"];
			isLaterShow = true;
		}
	}

		/// <summary>
		/// 获取httpurl
		/// </summary>
		/// <param name="_url">URL.</param>
		/// <param name="_wwwForm">Www form.</param>
		IEnumerator GetHttpUrl (string _url, WWWForm _wwwForm)
	{
		//Debug.Log("GetHttpUrl      " + _url);
		WWW postData1 = new WWW (_url, _wwwForm);
		yield return postData1;

		if (postData1 != null && WebUrlDic != null)
        {
            string content1 = postData1.text;
            Debug.Log(content1);
            JsonData jd1 = JsonMapper.ToObject(content1);
			
			try
            {
                for (int i = 0; i < jd1.Count; i++)
                {
                    //Debug.Log((int)jd1[i]["Status"]);
                    WebUrlEnum temp = (WebUrlEnum)((int)jd1[i]["Status"]);
					//Debug.Log(" ========== ========== ==========1");
					//Debug.Log("temp = " + temp);
					String str = (string)jd1[i]["Url"];

					if ((int)jd1[i]["Status"] == 14)
					{
                        if(!configuration_backupUrl.Contains(str))
						    configuration_backupUrl.Add(str);
					}
					else {
						if (!WebUrlDic.ContainsKey(temp))
						{
							Debug.Log("temp = " + temp);
							Debug.Log("URL = " + str);
							Debug.Log(" ========== ========== ==========");
                            WebUrlDic.Add(temp, str);
						}
						else
						{
							WebUrlDic[temp] = str;
						}
					}
                }
				UILogin.Instance.ChangeOtherLoginVisable(WebUrlDic[WebUrlEnum.OtherLogin]);

				for (int con=0; con< cd_array.Length; con++) {
					configuration_backupUrl.Add(cd_array[con]+ configuration_method);
				}

				string url_temp = "";
                for (int con = configuration_backupUrl.Count-1; con>=0 ; con--)
                {
					url_temp = url_temp + " " +configuration_backupUrl[con];
					//Debug.Log(configuration_backupUrl[con]);
					//Debug.Log(url_temp);
				}
				
				string domainStr = "{\"configuration\":{" + "\"url\":\"" + url_temp + "\"" + "}}";
                
				//Debug.Log(domainStr);
				if (FileUtilManager.IsExistsFile(LoginInfo.pathUrl, "Configuration.txt"))
				{
					FileUtilManager.DeleteFile(LoginInfo.pathUrl, "Configuration.txt");
				}
				FileUtilManager.CreateFile(LoginInfo.pathUrl, "Configuration.txt", domainStr);
				
			}
            catch (Exception e)
            {
                Debug.Log("GetHttpUrl catch");
                if (url_index < load_config.Count)
                {
                    url_index++;
                    url = (string)load_config[url_index];
                    Debug.Log(url);
                    HttpSend();
                }
            }
        }
        else
        {
            Debug.Log("GetHttpUrl else");
            if (url_index < load_config.Count)
            {
                url_index++;
                url = (string)load_config[url_index];
                HttpSend();
            }
        }

		UILogin.Instance.CheckVersionEx();
		ShowWaitingView(false);
		//  測試推廣版
		//UILogin.Instance.mServerUrl = UILogin.Instance.mServerUrlPromote;
	}

	void HttpSend ()
	{
		//if (FileUtilManager.IsExistsFile(LoginInfo.pathUrl, "Login.txt")) {

  //      }
		
		WWWForm wwwForm = new WWWForm ();
		//1 为 ios  2为 android
#if UNITY_IPHONE
		    wwwForm.AddField ("platformid", 1);
		    StartCoroutine (SendPost (url, wwwForm));
            //UILogin.CheckVersionEx();
#elif UNITY_ANDROID
		    wwwForm.AddField ("platformid", 2);
		    StartCoroutine (SendPost (url, wwwForm));
		    //UILogin.CheckVersionEx();
#endif
	}

	/// <summary>
	/// 保存http链接
	/// </summary>
	IEnumerator SaveHttpUrl ()
	{
		WWWForm wwwForm = new WWWForm ();
		wwwForm.AddField ("platformid", 0);
		yield return StartCoroutine (GetHttpUrl (url, wwwForm));
	}

	/// <summary>
	/// 后台配置请求
	/// </summary>
	public void BackstageSend ()
	{
		StartCoroutine (Backstage ());
	}
	//后台配置管理
	IEnumerator Backstage ()
	{

		MyInfo myInfo = (MyInfo)Facade.GetFacade ().data.Get (FacadeConfig.USERINFO_MODULE_ID);

		//Debug.Log("82300000======");
		//说明：Status: 1 表示请求成功  2 表示请求失败。
		//     Data：为请求的最终数据

		if (UIUpdate.WebUrlDic.ContainsKey (WebUrlEnum.Setting)) {
			if (WebUrlDic [WebUrlEnum.Setting] != null || WebUrlDic [WebUrlEnum.Setting] != "") {
				url2 = WebUrlDic [WebUrlEnum.Setting] + "?ajaxaction=GetModuleFunctionSwitchConfigJsonData";
			}
		}
		WWWForm wWWForm2 = new WWWForm ();
		wWWForm2.AddField ("ip", GetIP ());
		wWWForm2.AddField ("userid", myInfo.userID);
		WWW www = new WWW (url2, wWWForm2);

		yield return www;
		//Debug.LogError("www.text   = " + www.text);

		string wwwtext = www.text;
		//Debug.Log(wwwtext);
		JsonData jsonData = JsonMapper.ToObject (wwwtext);

		string status = jsonData ["Status"].ToString ();
		//Debug.Log("status =+++++++++++++++++++++++++++++ " + status);

		if (status == "1") {
			myInfo.SmallGame = (int)jsonData ["Data"] [0] ["1"];
//			Debug.Log ("status =+++++++++++++++++++++++++++++ " + myInfo.SmallGame);

			//Debug.Log(myInfo.SmallGame + "5716e89318e790");
			myInfo.RankingList = (int)jsonData ["Data"] [0] ["2"];
			myInfo.Charm = (int)jsonData ["Data"] [0] ["3"];
			myInfo.Gift = (int)jsonData ["Data"] [0] ["4"];
			myInfo.Consume = (int)jsonData ["Data"] [0] ["5"];//用户发公告消耗
			myInfo.Timer = (int)jsonData ["Data"] [0] ["6"] * 60; //发公告间隔
			myInfo.NoticeWindow = (int)jsonData ["Data"] [0] ["7"];   //公告控制面板

		}
	}

	IEnumerator CheckVersion()
	{
		Debug.Log("  CheckVersion  CheckVersion  CheckVersion : " + UIUpdate.Instance.isUpdateVersion + " : " + UIUpdate.Instance.isLaterShow);
		yield return new WaitForSeconds(.5f);
		if (UIUpdate.Instance.isUpdateVersion && UIUpdate.Instance.isLaterShow)
		{
			Debug.Log("  CheckVersion  1");
			string path = "Prefabs/UpdateCanvas";
			GameObject WindowClone = AppControl.OpenWindow(path);
			WindowClone.gameObject.SetActive(true);
			Instantiate(UIUpdate.Instance.UpdateWindow);
			Invoke("OpenConnection", 1f);
		}
		if (UIUpdate.Instance.isUpdateVersion && UIUpdate.Instance.isLaterShow == false)
		{
			Debug.Log("  CheckVersion  2");
			string path = "Prefabs/UpdateCanvas";
			GameObject WindowClone = AppControl.OpenWindow(path);
			WindowClone.gameObject.SetActive(true);
			Instantiate(UIUpdate.Instance.UpdateWindow);
		}
		else
		{
			Debug.Log("  CheckVersion  3");
			Invoke("OpenConnection", 1f);
		}

	}

	public void ShowWaitingView (bool nValue)
	{
		if (AppControl.miniGameState) {

		} else {
			if (nValue) {
				GameObject Window1 = UnityEngine.Resources.Load ("MainHall/Common/WaitingView") as UnityEngine.GameObject;
				mWaitingView = Instantiate (Window1);
				UIWaiting nData = mWaitingView.GetComponentInChildren<UIWaiting> ();
				nData.HideBackGround ();
				nData.SetInfo (20f, "獲取配置信息異常");
			} else {
				Destroy (mWaitingView);
				mWaitingView = null;
			}
		}
	}




#if UNITY_ANDROID
	//测试  个推清空个推日志
	public void OnClearClick ()
	{
		//Debug.Log("清空个推日志1");
		AndroidJavaClass jc = new AndroidJavaClass ("com.unity3d.player.UnityPlayer");
		AndroidJavaObject jo = jc.GetStatic<AndroidJavaObject> ("currentActivity");
		jo.Call ("GeTuiClear");
		//Debug.Log("清空个推日志2");
	}
	//测试  个推服务是否开启
	public void OnServiceClick ()
	{
		//Debug.Log("个推服务是否开启1");
		AndroidJavaClass jc = new AndroidJavaClass ("com.unity3d.player.UnityPlayer");
		AndroidJavaObject jo = jc.GetStatic<AndroidJavaObject> ("currentActivity");
		jo.Call ("GeTuiService");
		//Debug.Log("个推服务是否开启2");
	}
	//测试  个推绑定别名
	public void OnBindAliasClick ()
	{
		//Debug.Log("绑定别名1");
		AndroidJavaClass jc = new AndroidJavaClass ("com.unity3d.player.UnityPlayer");
		AndroidJavaObject jo = jc.GetStatic<AndroidJavaObject> ("currentActivity");
		jo.Call ("GeTuiBindAlias");
		//Debug.Log("个推服务是否开启2");
	}
	//测试  个推解绑别名
	public void OnUnBindAliasClick ()
	{
		//Debug.Log("解绑别名1");
		AndroidJavaClass jc = new AndroidJavaClass ("com.unity3d.player.UnityPlayer");
		AndroidJavaObject jo = jc.GetStatic<AndroidJavaObject> ("currentActivity");
		jo.Call ("GeTuiUnBindAlias");
		//Debug.Log("解绑别名2");
	}
	//测试  个推透传测试
	public void OnShowTransmissionClick ()
	{
		//Debug.Log("透传测试1");
		AndroidJavaClass jc = new AndroidJavaClass ("com.unity3d.player.UnityPlayer");
		AndroidJavaObject jo = jc.GetStatic<AndroidJavaObject> ("currentActivity");
		jo.Call ("GetTuiShowTransmission");
		//Debug.Log("透传测试2");
	}
	//测试  个推通知栏测试
	public void OnShowNotificationClick ()
	{
		//Debug.Log("通知栏测试1");
		AndroidJavaClass jc = new AndroidJavaClass ("com.unity3d.player.UnityPlayer");
		AndroidJavaObject jo = jc.GetStatic<AndroidJavaObject> ("currentActivity");
		jo.Call ("GeTuiShowNotification");
		//Debug.Log("通知栏测试2");
	}
	//测试  个推添加标签
	public void OnAddTagClick ()
	{
		//Debug.Log("添加标签1");
		AndroidJavaClass jc = new AndroidJavaClass ("com.unity3d.player.UnityPlayer");
		AndroidJavaObject jo = jc.GetStatic<AndroidJavaObject> ("currentActivity");
		jo.Call ("GeTuiAddTag");
		//Debug.Log("添加标签2");
	}
	//测试  个推获取CID
	public void OnGetCidClick ()
	{
		//Debug.Log("获取CID1");
		AndroidJavaClass jc = new AndroidJavaClass ("com.unity3d.player.UnityPlayer");
		AndroidJavaObject jo = jc.GetStatic<AndroidJavaObject> ("currentActivity");
		jo.Call ("GeTuiGetCid");
		//Debug.Log("获取CID2");
	}
	//测试  个推设置静默时间
	public void OnSetSilentimeClick ()
	{
		//Debug.Log("设置静默时间1");
		AndroidJavaClass jc = new AndroidJavaClass ("com.unity3d.player.UnityPlayer");
		AndroidJavaObject jo = jc.GetStatic<AndroidJavaObject> ("currentActivity");
		jo.Call ("GeTuiSetSilentime");
		//Debug.Log("设置静默时间2");
	}
	//测试  个推得到当前版本号
	public void OnGetVersionClick ()
	{
		//Debug.Log("得到当前版本号1");
		AndroidJavaClass jc = new AndroidJavaClass ("com.unity3d.player.UnityPlayer");
		AndroidJavaObject jo = jc.GetStatic<AndroidJavaObject> ("currentActivity");
		jo.Call ("GeTuiGetVersion");
		//Debug.Log("得到当前版本号2");
	}

	//获取用户cid
	public void GetCilentID (string cid)
	{
		Debug.Log ("获取用户cid====" + cid);
		if (cid != null) {
			if (isSaveGeTuiData == true) {
				geTuiUserID = cid;
			}
		}
		Debug.Log ("个推用户ID==" + geTuiUserID);
	}
#endif
}