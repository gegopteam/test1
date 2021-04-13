using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using AssemblyCSharp;
using UnityEngine.UI;
using System;
using LitJson;

public class BindWindowCtrl : MonoBehaviour
{
	public static BindWindowCtrl Instense;
	MyInfo myInfo ;
	public Transform parent;
	GameObject obj;
	GameObject Obj;
	public static bool isStartActivity = true;

	public GameObject BtnTinyGame;
	public GameObject RankingList;
	public GameObject notice;
    //public GameObject go;

    private MyInfo myInfo_1=null;
    //测试接口地址
    //private string geTuiURL = "http://183.131.69.227:8004/game/collectionclientid";
    //正式接口地址
    private string geTuiURL = "http://183.131.69.227:8003/game/collectionclientid";
    private string key = "9B8E2637248C4E83BD33334E24BDBB3F";
    private void Awake ()
	{
        myInfo = DataControl.GetInstance().GetMyInfo();

        //IsGuesttLogin ();

    }

	void Start ()
	{
		//控制活动窗弹起 
		Instense = this;
		GetInfo ();
        myInfo_1 = (MyInfo)Facade.GetFacade ().data.Get (FacadeConfig.USERINFO_MODULE_ID);
       
   
        Debug.Log("自己的金币+++" + myInfo_1.gold);
        if (myInfo_1.SmallGame == 0) {
			BtnTinyGame.SetActive (false);
            
        }
		if (myInfo_1.RankingList == 0) {
			RankingList.transform.GetComponent<Button> ().enabled = false;
			RankingList.transform.GetChild (0).GetComponent<Button> ().enabled = false;
			Debug.Log ("排行关闭");
		}
		if (myInfo_1.NoticeWindow == 0) {
			notice.transform.GetComponent<Button> ().enabled = false;
		}
#if UNITY_OPPO
        //上传oppo玩家信息
        GetUploadGamePlayerInfo(myInfo_1.loginInfo.gameId.ToString (), myInfo_1.nickname, myInfo_1.level);
#endif
#if UNITY_VIVO
        //上传vivo玩家信息
        UploadUserInfo(myInfo_1.loginInfo.gameId.ToString(), myInfo_1.level.ToString(), myInfo_1.nickname);
#endif
        //发送个推数据
        StartCoroutine(DelaySendGeTuiData());

    }

    /// <summary>
    /// 上传oppo玩家信息
    /// </summary>
    /// <param name="id">用户ID.</param>
    /// <param name="name">用户名.</param>
    /// <param name="level">用户等级.</param>
    public void GetUploadGamePlayerInfo (string id, string name, int level)
	{
#if UNITY_ANDROID
		AndroidJavaClass jc = new AndroidJavaClass ("com.unity3d.player.UnityPlayer");
		AndroidJavaObject jo = jc.GetStatic<AndroidJavaObject> ("currentActivity");
		jo.Call ("UploadGamePlayerInfo", id, name, level);
#endif
	}

    /// <summary>
    /// 上传vivo玩家信息
    /// </summary>
    /// <param name="userID">用户ID.</param>
    /// <param name="userLevel">用户等级.</param>
    /// <param name="userName">用户名.</param>
    public void UploadUserInfo(string userID,string userLevel,string userName)
    {
#if UNITY_ANDROID
        AndroidJavaClass jc = new AndroidJavaClass("com.unity3d.player.UnityPlayer");
        AndroidJavaObject jo = jc.GetStatic<AndroidJavaObject>("currentActivity");
        jo.Call("UploadUserInfo", userID, userLevel, userName);
#endif
    }

    void GetInfo ()
	{
		if (myInfo.isGuestLogin && AppInfo.isFrist == false && myInfo.isTestRoom == 1) {
			WarningWindow ();
		}
	}

	////如果是游客模式,,不显示小游戏
	//private void IsGuesttLogin ()
	//{
	//	#if UNITY_ANDROID
	//	Debug.Log ("是否是游客登录" + myInfo.isGuestLogin);
	//	if (myInfo.isGuestLogin == true) {
	//		go.gameObject.SetActive (false);
	//	}
	//	#endif
	//}

	/// <summary>
	/// 绑定警告窗口
	/// </summary>
	public void WarningWindow ()
	{
		obj = Resources.Load<GameObject> ("Window/BindPhoneNumber");
		Debug.Log ("obj111" + obj.name);
		Obj = Instantiate (obj);
		Debug.Log ("obj222" + obj.name);
	}

	/// <summary>
	/// 绑定窗口
	/// </summary>
	public void GenerateWindow ()
	{
		obj = Resources.Load<GameObject> ("Window/BindPhoneNumber");
		Obj = Instantiate (obj);
	}

	/// <summary>
	/// 绑定成功窗口
	/// </summary>
	public GameObject BindSuccessWindow ()
	{
		obj = Resources.Load<GameObject> ("Window/BindSuccess");
		Obj = Instantiate (obj);
		return Obj;
	}

    IEnumerator DelaySendGeTuiData()
    {
        yield return new WaitForSeconds(3f);
        SendGeTuiData();
    }

    /// <summary>
    /// 发送个推用户数据
    /// </summary>
    private void SendGeTuiData()
    {
        StartCoroutine(SendUseridAndClientid(geTuiURL, SendGeTuiUserData()));
        UIUpdate.isSaveGeTuiData = false;
    }

    //获取当前时间戳
    public string GetDataTime()
    {
        string dataTime = DateTime.Now.Year.ToString() + DateTime.Now.Month.ToString() + DateTime.Now.Day.ToString() + DateTime.Now.Hour.ToString() + DateTime.Now.Minute.ToString() + DateTime.Now.Second.ToString();
        return dataTime;
    }

    private WWWForm SendGeTuiUserData()
    {
        string time = GetDataTime();
        WWWForm data = new WWWForm();
        data.AddField("userid", myInfo_1.loginInfo.gameId.ToString());
        Debug.Log("userid=======" + myInfo_1.loginInfo.gameId.ToString());
        data.AddField("clientid", UIUpdate.geTuiUserID);
        Debug.Log("clientid=====" + UIUpdate.geTuiUserID);
        data.AddField("timestamp", time);
        Debug.Log("当前时间戳=====" + time);
        string getMD5 = LoginUtil.GetMD5(myInfo_1.loginInfo.gameId.ToString() + UIUpdate.geTuiUserID + time + key).ToLower();
        data.AddField("sign", getMD5);
        return data;
    }
    private IEnumerator SendUseridAndClientid(string url,WWWForm nData)
    {

        WWW www = new WWW(url, nData);
        yield return www;
        Debug.Log("www.text===" + www.text);
        string wwwtext = www.text;
        JsonData json = JsonMapper.ToObject(wwwtext);
        string status = json["code"].ToString();
        int number = int.Parse(status);
        switch (number)
        {
            case 0:
                Debug.Log("=====发送成功=====");
                break;
            case 1:
                Debug.Log("=====数据验证失败=====");
                break;
            case -1:
                Debug.Log("=====传递参数不全=====");
                break;
            case -2:
                Debug.Log("=====服务器错误=========");
                break;
        }
    }

    public void GenerateWindow_Close ()
	{
		Destroy (Obj);
	}
}
