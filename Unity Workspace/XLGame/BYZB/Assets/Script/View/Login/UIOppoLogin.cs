using System.Collections;
using System.Collections.Generic;
using AssemblyCSharp;
using LitJson;
using UnityEngine;
using UnityEngine.UI;

public class UIOppoLogin : MonoBehaviour
{
#if UNITY_ANDROID
    MyInfo userInfo;
    private static UIOppoLogin instance;
    int oppoLoginType = 8;
    string key = "E39,D-=SM39S10-ATU85NC,53\\kg9";
    string oppoURL = "http://api.cl0579.com/api/MobileFishSms.aspx";
    public static UIOppoLogin Instance
    {
        get
        {
            return instance;
        }
    }
    private void Awake()
    {
        instance = this;
    }

    private void Start()
    {
        userInfo = DataControl.GetInstance().GetMyInfo();
        userInfo.platformType = oppoLoginType;
    }
    private string ServerUrl = "";

    public void SetServerUrl(string serverUrl)
    {
        this.ServerUrl = serverUrl;
    }

#if UNITY_ANDROID
    public void OnOppoLogin()
    {
        OppoLogin();
    }

    public void OppoLogin()
    {
        AudioManager._instance.PlayEffectClip(AudioManager.ui_click);
        UILogin.Instance.OnLoginPreparing();
        userInfo.ServerUrl = UILogin.Instance.mServerUrl;
        userInfo.isLogining = true;
        userInfo.isGuestLogin = false;
        AndroidJavaClass jc = new AndroidJavaClass("com.unity3d.player.UnityPlayer");
        AndroidJavaObject jo = jc.GetStatic<AndroidJavaObject>("currentActivity");
        jo.Call("OppoLogin");
    }
    public void ToOppoOut()
    {
        Debug.Log("点击了oppo登录1");
        AndroidJavaClass jc = new AndroidJavaClass("com.unity3d.player.UnityPlayer");
        AndroidJavaObject jo = jc.GetStatic<AndroidJavaObject>("currentActivity");
        jo.Call("OnExitGmae");
        Debug.Log("点击了OPPO登录2");
    }
    public void ToUnity_OppoSsoid(string info)
    {
        Debug.Log("ToUnity_OppoSsoid:" + info);
        userInfo.openId = info;
        userInfo.nickname = info;
    }
    //获取oppotoken
    public void ToUnity_OppoToken(string token)
    {
        Debug.Log("获取OppoToken1" + token);
        userInfo.acessToken = token;
        userInfo.oppoAcessToken = token;
    }
#endif 

    //获取登录信息
    public void GetOppoUserLoginInfo(string info)
    {
        Debug.Log("=====GetOppoUserLoginInfo:" + info);
        if (info == "登录成功")
        {
            Debug.Log("====GetOppoUserLoginInfo=====2");
            SendOPPOUserInfo();
            OpenTipsWindow(info);
            Debug.Log("====GetOppoUserLoginInfo=====3");
        }
        else
        {
            Debug.Log("获取登录信息3");
            OpenTipsWindow("登录失败");
        }
    }

    //发送用户信息
    private void SendOPPOUserInfo()
    {
        Debug.Log("====SendOPPOUserInfo=====1");
        StartCoroutine(GetOppoLoginResponse(oppoURL, GetOppoPosData()));
        Debug.Log("====SendOPPOUserInfo=====2");
    }

    private WWWForm GetOppoPosData()
    {
        Debug.Log("ssoid====:" + userInfo.openId);
        Debug.Log("token_1====:" + userInfo.acessToken);
        Debug.Log("type:" + oppoLoginType);
        Debug.Log("oauth:" + 33);
        Debug.Log("mid:" + LoginMsgHandle.getChannelNumber().ToString());
        Debug.Log("mac:" + SystemInfo.deviceUniqueIdentifier);

        WWWForm nData = new WWWForm();
        nData.AddField("type", oppoLoginType);
        nData.AddField("oauth", 33);
        nData.AddField("openid", userInfo.openId);
        nData.AddField("token", userInfo.acessToken);
        nData.AddField("sex", "1");
        nData.AddField("mid", LoginMsgHandle.getChannelNumber().ToString());//渠道号
        nData.AddField("sid", "0");
        nData.AddField("mac", SystemInfo.deviceUniqueIdentifier);//设备唯一标识符
        string getMD5 = LoginUtil.GetMD5(33 + userInfo.openId + userInfo.acessToken + "1" + LoginMsgHandle.getChannelNumber().ToString() + "0" + SystemInfo.deviceUniqueIdentifier + key).ToLower();
        nData.AddField("md5", getMD5);
        Debug.Log("md5:" + getMD5);
        Debug.Log("GetOppoPosData:" + nData);
        return nData;
    }

    private IEnumerator GetOppoLoginResponse(string url, WWWForm data)
    {
        WWW www = new WWW(url, data);
        Debug.Log("GetOppoLoginResponse1");
        yield return www;
        if (string.IsNullOrEmpty(www.error))
        {
            Debug.Log("GetOppoLoginResponse2");
            Debug.Log("[ --GetOppoLoginResponse- ]" + www.text);
            //JsonData data_1 = JsonMapper.ToObject(www.text);
            ////if (data_1!=null)
            ////{
            ////    string userieid = data_1["userieid"].ToString();
            ////    string token = data_1["token"].ToString();
            ////    Debug.Log("userieid:" + userieid);
            //Debug.Log("token:" + data_1);
            ////}
            LoginUnit nUnit = new LoginUnit();
            nUnit.DoConvert(www.text);
            userInfo.mLoginData = nUnit;
            if (nUnit.result != 0)
            {
                Debug.Log("GetOppoLoginResponse3");
                Debug.Log("nUnit===" + userInfo.mLoginData);

                string data_1 = www.text;
                string[] data_2 = data_1.Split('|');
                userInfo.acessToken = data_2[3];
                Debug.Log("data_2[0]===" + data_2[0]);
                Debug.Log("data_2[1]===" + data_2[1]);
                Debug.Log("data_2[2]===" + data_2[2]);
                Debug.Log("data_2[3]===" + data_2[3]);
                Debug.Log("data_2===" + data_2);

                if (userInfo.isGuestLogin)
                {
                    Debug.Log("GetOppoLoginResponse4");
                    userInfo.nickname = nUnit.username;
                    userInfo.acessToken = nUnit.token;
                    userInfo.password = nUnit.passwd;
                    Debug.Log("nUnit.username==" + nUnit.username);
                    Debug.Log("nUnit.token==" + nUnit.token);
                    Debug.Log("nUnit.passwd==" + nUnit.passwd);
                    Debug.Log("nUnit.userid==" + nUnit.userid);
                    Debug.Log("result==" + nUnit.result);
                    Debug.Log("GetOppoLoginResponse5");
                }
                DoConnectAndSendLogin();
            }
            else
            {
                Debug.Log("GetOppoLoginResponse4");
                ShowWaitingView(false);
                OpenTipsWindow("错误的登录数据");
                OpenTipsWindow("nUnit.result:" + nUnit.result.ToString());
            }
            //OpenTipsWindow("123456");
            //DoConnectAndSendLogin();
        }
    }


    //连接和发送登录
    private void DoConnectAndSendLogin()
    {
        Debug.Log("连接和发送登录1");
        if (string.IsNullOrEmpty(userInfo.openId))
        {
            Debug.Log("连接和发送登录2");
            OpenTipsWindow("獲取用戶信息失敗！請重試");
            return;
        }
        //oppoInfo.TargetView = AppView.HALL;
        //AppControl.ToView(AppView.LOADING);
        //oppoInfo.ServerUrl = ServerUrl;
        userInfo.isLogining = true;

        //正式服务器
        DataControl.GetInstance().ConnectSvr(userInfo.ServerUrl, AppInfo.portNumber);
        Invoke("ResetLoginState", 3.0f);
        //测试服务器
        //DataControl.GetInstance().ConnectSvr("183.131.69.235", 50666);
    }
    void ResetLoginState()
    {
        userInfo.isLogining = false;
    }
    GameObject mWaitingView;
    public void ShowWaitingView(bool nValue)
    {
        if (AppControl.miniGameState)
        {

        }
        else
        {
            if (nValue)
            {
                GameObject Window1 = UnityEngine.Resources.Load("MainHall/Common/WaitingView") as UnityEngine.GameObject;
                mWaitingView = UnityEngine.GameObject.Instantiate(Window1);
                UIWaiting nData = mWaitingView.GetComponentInChildren<UIWaiting>();
                nData.HideBackGround();
                nData.SetInfo(10f, "獲取登錄信息異常，請重新嘗試");
            }
            else
            {
                Destroy(mWaitingView);
                mWaitingView = null;
            }
        }
    }

    //打开提示窗口
    private void OpenTipsWindow(string info)
    {
        GameObject window = UnityEngine.Resources.Load("Window/WindowTipsThree") as UnityEngine.GameObject;
        GameObject WindowTips = GameObject.Instantiate(window);
        UITipAutoNoMask TipsInfo = WindowTips.GetComponent<UITipAutoNoMask>();
        TipsInfo.tipText.text = info;
    }
#endif
}
