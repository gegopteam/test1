using UnityEngine;
using System.Collections;
using AssemblyCSharp;

public class HuaweiLogin : MonoBehaviour
{
    private static HuaweiLogin instance;
    private MyInfo huaweiUserInfo;
    private int huaweiLoginType = 8;
    private string key = "E39,D-=SM39S10-ATU85NC,53\\kg9";
    private string huaweiURL = "http://api.cl0579.com/api/MobileFishSms.aspx";

    public static HuaweiLogin Instance
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
        huaweiUserInfo = DataControl.GetInstance().GetMyInfo();
        huaweiUserInfo.platformType = huaweiLoginType;
    }

    private string ServerUrl = "";

    public void SetHuaweiServerUrl(string serverUrl)
    {
        this.ServerUrl = serverUrl;
    }
    //登录
    public void OnHuaweiLoginClick()
    {
        AudioManager._instance.PlayEffectClip(AudioManager.ui_click);
        UILogin.Instance.OnLoginPreparing();
        huaweiUserInfo.ServerUrl = UILogin.Instance.mServerUrl;
        huaweiUserInfo.isLogining = true;
        huaweiUserInfo.isGuestLogin = false;
        Debug.Log("点击了QQ登录1");
        AndroidJavaClass jc = new AndroidJavaClass("com.unity3d.player.UnityPlayer");
        AndroidJavaObject jo = jc.GetStatic<AndroidJavaObject>("currentActivity");
        jo.Call("HuaweiLogin");
        Debug.Log("点击了QQ登录2");  
    }

    //获取用户名
    public void ToUnity_HuaweiName(string userName)
    {
        Debug.Log("ToUnity_HuaweiName=====" + userName);
        huaweiUserInfo.nickname = userName;
    }
    //获取openid
    public void ToUnity_HuaweiOpenId(string OpenId)
    {
        Debug.Log("ToUnity_HuaweiOpenId=====" + OpenId);
        huaweiUserInfo.openId = OpenId;
    }
    //获取Token
    public void ToUnity_HuaweiToken(string Token)
    {
        Debug.Log("ToUnity_HuaweiToken=====" + Token);
        huaweiUserInfo.acessToken = Token;
        huaweiUserInfo.huaweiAcessToken = Token;
    }

    //获取登录信息
    public void GetHuaweiUserLoginInfo(string info)
    {
        Debug.Log("=====GetVivoUserLoginInfo:" + info);
        if (info == "登录成功")
        {
            Debug.Log("====GetVivoUserLoginInfo=====2");
            SendHuaweiUserInfo();
            OpenTipsWindow(info);
            Debug.Log("====GetVivoUserLoginInfo=====3");
        }
        else
        {
            Debug.Log("获取登录信息3");
            OpenTipsWindow("登錄失敗");
        }
    }
    //发送用户信息
    private void SendHuaweiUserInfo()
    {
        Debug.Log("====SendHuaweiUserInfo=====1");
        StartCoroutine(GetHuaweiLoginnResponse(huaweiURL, GetHuaweiPosData()));
        Debug.Log("====SendHuaweiUserInfo=====2");
    }

    //请求数据数据
    private WWWForm GetHuaweiPosData()
    {
        Debug.Log("ssoid====:" + huaweiUserInfo.openId);
        Debug.Log("token_1====:" + huaweiUserInfo.acessToken);
        Debug.Log("type:" + huaweiLoginType);
        Debug.Log("oauth:" + 37);
        Debug.Log("mid:" + LoginMsgHandle.getChannelNumber().ToString());
        Debug.Log("mac:" + SystemInfo.deviceUniqueIdentifier);

        WWWForm nData = new WWWForm();
        nData.AddField("type", huaweiLoginType);
        nData.AddField("oauth", 37);
        nData.AddField("openid", huaweiUserInfo.openId);
        nData.AddField("token", huaweiUserInfo.acessToken);
        nData.AddField("sex", "1");
        nData.AddField("mid", LoginMsgHandle.getChannelNumber().ToString());//渠道号
        nData.AddField("sid", "0");
        nData.AddField("mac", SystemInfo.deviceUniqueIdentifier);//设备唯一标识符
        string getMD5 = LoginUtil.GetMD5(37 + huaweiUserInfo.openId + huaweiUserInfo.acessToken + "1" + LoginMsgHandle.getChannelNumber().ToString() + "0" + SystemInfo.deviceUniqueIdentifier + key).ToLower();
        nData.AddField("md5", getMD5);
        Debug.Log("md5:" + getMD5);
        Debug.Log("GetOppoPosData:" + nData);
        return nData;
    }
    private string test1;
    private string test2;
    private string test3;
    private string test4;
    private string test5;
    private IEnumerator GetHuaweiLoginnResponse(string url, WWWForm data)
    {
        Debug.Log(" GetHuaweiLoginnResponse DoConnectAndSendLogin");
        WWW www = new WWW(url, data);
        Debug.Log("GetHuaweiLoginnResponse1");
        yield return www;
        Debug.Log("[ --GetHuaweiLoginnResponse- ]" + www.text);
        if (string.IsNullOrEmpty(www.error))
        {
            Debug.Log("GetHuaweiLoginnResponse2");
            Debug.Log("[ --GetVivoLoginnResponse- ]" + www.text);
            LoginUnit nUnit = new LoginUnit();
            nUnit.DoConvert(www.text);
            huaweiUserInfo.mLoginData = nUnit;
            if (nUnit.result != 0)
            {
                Debug.Log("GetHuaweiLoginnResponse3");
                string data_1 = www.text;
                string[] data_2 = data_1.Split('|');
                huaweiUserInfo.acessToken = data_2[3];
                Debug.Log("data_2[0]===" + data_2[0]);
                Debug.Log("data_2[1]===" + data_2[1]);
                Debug.Log("data_2[2]===" + data_2[2]);
                Debug.Log("data_2[3]===" + data_2[3]);
                Debug.Log("data_2===" + data_2);
                if (huaweiUserInfo.isGuestLogin)
                {
                    huaweiUserInfo.nickname = nUnit.username;
                }
                DoConnectAndSendLogin();
            }
            else
            {
                Debug.Log("GetHuaweiLoginnResponse4");
                ShowWaitingView(false);
                OpenTipsWindow("錯誤的登錄數據");
                OpenTipsWindow("nUnit.result:" + nUnit.result.ToString());
            }
        }
    }
    private void DoConnectAndSendLogin()
    {
        Debug.Log("连接和发送登录1");
        if (string.IsNullOrEmpty(huaweiUserInfo.openId))
        {
            Debug.Log("连接和发送登录2");
            OpenTipsWindow("獲取用戶信息失敗！請重試");
            return;
        }
        //oppoInfo.TargetView = AppView.HALL;
        //AppControl.ToView(AppView.LOADING);
        //oppoInfo.ServerUrl = ServerUrl;
        huaweiUserInfo.isLogining = true;
        //正式服务器
        DataControl.GetInstance().ConnectSvr(huaweiUserInfo.ServerUrl, AppInfo.portNumber);
        Invoke("ResetLoginState", 3.0f);
        //测试服务器
        //DataControl.GetInstance().ConnectSvr("183.131.69.235", 50666);
    }
    void ResetLoginState()
    {
        huaweiUserInfo.isLogining = false;
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
    //退出
    public void OnVivoQuitClick()
    {
        Debug.Log("OnVivoQuitClick");
        AndroidJavaClass jc = new AndroidJavaClass("com.unity3d.player.UnityPlayer");
        AndroidJavaObject jo = jc.GetStatic<AndroidJavaObject>("currentActivity");
        jo.Call("onBackPressed");
    }

    //打开提示窗口
    private void OpenTipsWindow(string info)
    {
        GameObject window = UnityEngine.Resources.Load("Window/WindowTipsThree") as UnityEngine.GameObject;
        GameObject WindowTips = GameObject.Instantiate(window);
        UITipAutoNoMask TipsInfo = WindowTips.GetComponent<UITipAutoNoMask>();
        TipsInfo.tipText.text = info;
    }
}
