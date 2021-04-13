using UnityEngine;
using System.Collections;
using LitJson;
using AssemblyCSharp;

public class HuaweiPay : MonoBehaviour
{

    private string huaweiOrderUrl = "http://api.cl0579.com/api/MobilePayConfig.aspx";
    private string huaweiPayUrl = "http://notify.clgame.cc/OPPO_M_XL.aspx";
    private JsonData ProductData = null;
    protected string buyProductId = "";
    private string orderIDForWeb = "";
    private string signWeb;
    private MyInfo huaweiUserInfo;
    private string nMjId = "8";
    private int psid = 0;
    private string nVersion = "0.0.0";
    private string goodsid = "189";
    private int payType = 314;
#if UNITY_OPPO
        private int payType = 314;
#endif

#if UNITY_VIVO
    private int payType = 219;
#endif

#if UNITY_Huawei
    private int payType = 116;
#endif
    private string verSdk = "0.0.0";
    private int gameId = 510;
    private string deviceType = "3";
    string key = "E39,D-=SM39S10-ATU85NC,53\\kg9";
    private static HuaweiPay instance;
    private int money;

    private string coin;
    private string ip;
    private string time;
    private string url;
    public static HuaweiPay Instance
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
    }

    public void RequestHuaweiProductsData_5()
    {
#if UNITY_OPPO
                StartCoroutine(GetOppoOrderUrl(huaweiOrderUrl, GetOPPOFormData("398")));
#endif

#if UNITY_VIVO
        StartCoroutine(GetVivoOrderUrl(huaweiOrderUrl, GetVivoFormData("398")));
#endif

#if UNITY_Huawei
        StartCoroutine(GetHuaweiOrderUrl(huaweiOrderUrl, GetHuaweiFormData("398")));

#endif

    }
    public void RequestHuaweiProductsData_10()
    {
#if UNITY_OPPO
                StartCoroutine(GetOppoOrderUrl(huaweiOrderUrl, GetOPPOFormData("399")));
#endif

#if UNITY_VIVO
         StartCoroutine(GetVivoOrderUrl(huaweiOrderUrl, GetVivoFormData("399")));
#endif

#if UNITY_Huawei
        StartCoroutine(GetHuaweiOrderUrl(huaweiOrderUrl, GetHuaweiFormData("399")));
        #endif

    }
    public void RequestHuaweiProductsData_50()
    {
#if UNITY_OPPO
                StartCoroutine(GetOppoOrderUrl(huaweiOrderUrl, GetOPPOFormData("400")));
#endif

#if UNITY_VIVO
        StartCoroutine(GetVivoOrderUrl(huaweiOrderUrl, GetVivoFormData("400")));
#endif

#if UNITY_Huawei
        StartCoroutine(GetHuaweiOrderUrl(huaweiOrderUrl, GetHuaweiFormData("400")));
        #endif
    }
    public void RequestHuaweiProductsData_100()
    {
#if UNITY_OPPO
                StartCoroutine(GetOppoOrderUrl(huaweiOrderUrl, GetOPPOFormData("401")));
#endif

#if UNITY_VIVO
        StartCoroutine(GetVivoOrderUrl(huaweiOrderUrl, GetVivoFormData("401")));
#endif

#if UNITY_Huawei
        StartCoroutine(GetHuaweiOrderUrl(huaweiOrderUrl, GetHuaweiFormData("401")));
        #endif
    }
    public void RequestHuaweiProductsData_200()
    {
#if UNITY_OPPO
                StartCoroutine(GetOppoOrderUrl(huaweiOrderUrl, GetOPPOFormData("402")));
#endif

#if UNITY_VIVO
        StartCoroutine(GetVivoOrderUrl(huaweiOrderUrl, GetVivoFormData("402")));
#endif

#if UNITY_Huawei
        StartCoroutine(GetHuaweiOrderUrl(huaweiOrderUrl, GetHuaweiFormData("402")));
        #endif
    }
    public void RequestHuaweiProductsData_500()
    {
#if UNITY_OPPO
                StartCoroutine(GetOppoOrderUrl(huaweiOrderUrl, GetOPPOFormData("403")));
#endif

#if UNITY_VIVO
        StartCoroutine(GetVivoOrderUrl(huaweiOrderUrl, GetVivoFormData("403")));
#endif

#if UNITY_Huawei
        StartCoroutine(GetHuaweiOrderUrl(huaweiOrderUrl, GetHuaweiFormData("403")));
        #endif
    }
    //银龙
    public void RequestHuaweiProductsData_Yinlong()
    {
#if UNITY_OPPO
                StartCoroutine(GetOppoOrderUrl(huaweiOrderUrl, GetOPPOFormData("404")));
#endif

#if UNITY_VIVO
        StartCoroutine(GetVivoOrderUrl(huaweiOrderUrl, GetVivoFormData("404")));
#endif

#if UNITY_Huawei
        StartCoroutine(GetHuaweiOrderUrl(huaweiOrderUrl, GetHuaweiFormData("404")));
        #endif
    }
    //金龙
    public void RequestHuaweiProductsData_Jinlong()
    {
#if UNITY_OPPO
                StartCoroutine(GetOppoOrderUrl(huaweiOrderUrl, GetOPPOFormData("405")));
#endif

#if UNITY_VIVO
        StartCoroutine(GetVivoOrderUrl(huaweiOrderUrl, GetVivoFormData("405")));
#endif

#if UNITY_Huawei
        StartCoroutine(GetHuaweiOrderUrl(huaweiOrderUrl, GetHuaweiFormData("405")));
        #endif

    }
    //神龙
    public void RequestHuaweiProductsData_Shenlong()
    {
#if UNITY_OPPO
                StartCoroutine(GetOppoOrderUrl(huaweiOrderUrl, GetOPPOFormData("406")));
#endif

#if UNITY_VIVO
        StartCoroutine(GetVivoOrderUrl(huaweiOrderUrl, GetVivoFormData("406")));
#endif

#if UNITY_Huawei
        StartCoroutine(GetHuaweiOrderUrl(huaweiOrderUrl, GetHuaweiFormData("406")));
        #endif
    }
    //特惠12
    public void RequestHuaweiProductsData_Preferential_12()
    {
#if UNITY_OPPO
                StartCoroutine(GetOppoOrderUrl(huaweiOrderUrl, GetOPPOFormData("407")));
#endif

#if UNITY_VIVO
        StartCoroutine(GetVivoOrderUrl(huaweiOrderUrl, GetVivoFormData("407")));
#endif

#if UNITY_Huawei
        StartCoroutine(GetHuaweiOrderUrl(huaweiOrderUrl, GetHuaweiFormData("407")));
        #endif
    }
    //特惠38
    public void RequestHuaweiProductsData_Preferential_38()
    {
#if UNITY_OPPO
                StartCoroutine(GetOppoOrderUrl(huaweiOrderUrl, GetOPPOFormData("408")));
#endif

#if UNITY_VIVO
        StartCoroutine(GetVivoOrderUrl(huaweiOrderUrl, GetVivoFormData("408")));
#endif

#if UNITY_Huawei
        StartCoroutine(GetHuaweiOrderUrl(huaweiOrderUrl, GetHuaweiFormData("408")));
        #endif
    }
    //特惠68
    public void RequestHuaweiProductsData_Preferential_68()
    {
#if UNITY_OPPO
                StartCoroutine(GetOppoOrderUrl(huaweiOrderUrl, GetOPPOFormData("409")));
#endif

#if UNITY_VIVO
        StartCoroutine(GetVivoOrderUrl(huaweiOrderUrl, GetVivoFormData("409")));
#endif

#if UNITY_Huawei
        StartCoroutine(GetHuaweiOrderUrl(huaweiOrderUrl, GetHuaweiFormData("409")));
        #endif
    }
    //特惠98
    public void RequestHuaweiProductsData_Preferential_98()
    {
#if UNITY_OPPO
                StartCoroutine(GetOppoOrderUrl(huaweiOrderUrl, GetOPPOFormData("410")));
#endif

#if UNITY_VIVO
        StartCoroutine(GetVivoOrderUrl(huaweiOrderUrl, GetVivoFormData("410")));
#endif

#if UNITY_Huawei
        StartCoroutine(GetHuaweiOrderUrl(huaweiOrderUrl, GetHuaweiFormData("410")));
        #endif
    }
    //特惠188
    public void RequestHuaweiProductsData_Preferential_188()
    {
#if UNITY_OPPO
                StartCoroutine(GetOppoOrderUrl(huaweiOrderUrl, GetOPPOFormData("411")));
#endif

#if UNITY_VIVO
        StartCoroutine(GetVivoOrderUrl(huaweiOrderUrl, GetVivoFormData("411")));
#endif

#if UNITY_Huawei
        StartCoroutine(GetHuaweiOrderUrl(huaweiOrderUrl, GetHuaweiFormData("411")));
        #endif
    }
    //给力
    public void RequestHuaweiProductsData_Awesome_1()
    {
#if UNITY_OPPO
                StartCoroutine(GetOppoOrderUrl(huaweiOrderUrl, GetOPPOFormData("412")));
#endif

#if UNITY_VIVO
        StartCoroutine(GetVivoOrderUrl(huaweiOrderUrl, GetVivoFormData("412")));
#endif

#if UNITY_Huawei
        StartCoroutine(GetHuaweiOrderUrl(huaweiOrderUrl, GetHuaweiFormData("412")));
        #endif
    }
    //宝藏40
    public void RequestHuaweiProductsData_Treasure_40()
    {
#if UNITY_OPPO
                StartCoroutine(GetOppoOrderUrl(huaweiOrderUrl, GetOPPOFormData("413")));
#endif

#if UNITY_VIVO
        StartCoroutine(GetVivoOrderUrl(huaweiOrderUrl, GetVivoFormData("413")));
#endif

#if UNITY_Huawei
        StartCoroutine(GetHuaweiOrderUrl(huaweiOrderUrl, GetHuaweiFormData("413")));
        #endif
    }
    //宝藏68
    public void RequestHuaweiProductsData_Treasure_68()
    {
#if UNITY_OPPO
                StartCoroutine(GetOppoOrderUrl(huaweiOrderUrl, GetOPPOFormData("414")));
#endif

#if UNITY_VIVO
        StartCoroutine(GetVivoOrderUrl(huaweiOrderUrl, GetVivoFormData("414")));
#endif

#if UNITY_Huawei
        StartCoroutine(GetHuaweiOrderUrl(huaweiOrderUrl, GetHuaweiFormData("414")));
        #endif

    }
    //宝藏128
    public void RequestHuaweiProductsData_Treasure_128()
    {
#if UNITY_OPPO
                StartCoroutine(GetOppoOrderUrl(huaweiOrderUrl, GetOPPOFormData("415")));
#endif

#if UNITY_VIVO
        StartCoroutine(GetVivoOrderUrl(huaweiOrderUrl, GetVivoFormData("415")));
#endif

#if UNITY_Huawei
        StartCoroutine(GetHuaweiOrderUrl(huaweiOrderUrl, GetHuaweiFormData("415")));
        #endif
    }
    //双喜128
    public void RequestHuaweiProductsData_Double_128()
    {
#if UNITY_OPPO
                StartCoroutine(GetOppoOrderUrl(huaweiOrderUrl, GetOPPOFormData("416")));
#endif

#if UNITY_VIVO
        StartCoroutine(GetVivoOrderUrl(huaweiOrderUrl, GetVivoFormData("416")));
#endif

#if UNITY_Huawei
        StartCoroutine(GetHuaweiOrderUrl(huaweiOrderUrl, GetHuaweiFormData("416")));
        #endif

    }
    //双喜388
    public void RequestHuaweiProductsData_Double_388()
    {

#if UNITY_OPPO
        StartCoroutine(GetOppoOrderUrl(huaweiOrderUrl, GetOPPOFormData("417")));
#endif

#if UNITY_VIVO
        StartCoroutine(GetVivoOrderUrl(huaweiOrderUrl, GetVivoFormData("417")));
#endif

#if UNITY_Huawei
        StartCoroutine(GetHuaweiOrderUrl(huaweiOrderUrl, GetHuaweiFormData("417")));
#endif
    }

    //得到form数据
    private WWWForm GetHuaweiFormData(string goodID)
    {
        string MD5Sign = LoginUtil.GetMD5(key + nMjId + psid + nVersion + goodID + payType + verSdk + gameId + DataControl.GetInstance().GetMyInfo().userID + deviceType).ToLower();
        WWWForm sendData = new WWWForm();
        sendData.AddField("act", "order");
        sendData.AddField("mjid", nMjId);
        sendData.AddField("psid", psid);//0
        sendData.AddField("sign", MD5Sign);
        sendData.AddField("gameid", gameId);// 510
        sendData.AddField("version", nVersion);//  "0.0.0"
        sendData.AddField("versdk", verSdk);// "0.0.0"
        sendData.AddField("goodsid", goodID);// 189
        sendData.AddField("payType", payType);// 
        sendData.AddField("deviceType", deviceType);// "3"
        sendData.AddField("usrid", DataControl.GetInstance().GetMyInfo().userID);
        Debug.Log("act=" + "order" + "&mjid=" + nMjId + "&sign=" + MD5Sign + "&gameid=" + gameId + "&version=" + nVersion + "&versdk=" + verSdk + "&goodsid=" + goodID + "&payType=" + payType + "&deviceType=" + deviceType + "&usrid=" + DataControl.GetInstance().GetMyInfo().userID);
        return sendData;
    }

    //得到订单
    private IEnumerator GetHuaweiOrderUrl(string url, WWWForm data)
    {
        WWW www = new WWW(url, data);
        yield return www;
        Debug.Log("得到订单:" + www.text);
        if (www.error != null)
        {
            Debug.LogError("GetOppoOrderUrl error:" + www.error);
        }
        else
        {
            try
            {
                Debug.Log("www.text=:" + www.text);
                ProductData = JsonMapper.ToObject(www.text);
                if (!ProductData["code"].ToString().Equals("0"))
                {
                    GetAndroidMakeText(ProductData["msg"].ToString());
                }
                WebOrderResult jsonInfo = JsonMapper.ToObject<WebOrderResult>(www.text);
                Debug.Log("jsonInfo.code:" + jsonInfo.code);
                if (jsonInfo.code == 0)
                {
                    Debug.Log("jsonInfo.code:" + jsonInfo.code);
                    orderIDForWeb = jsonInfo.data.orderid;
                    signWeb = jsonInfo.data.sign;
                    Debug.Log("orderIDForWeb:" + orderIDForWeb);
                    Debug.Log("signWeb:" + signWeb);

                    money = jsonInfo.data.rmb;
                    coin = jsonInfo.data.coin.ToString();
                    ip = jsonInfo.data.ip;
                    time = jsonInfo.data.bill_time;
                    url = jsonInfo.data.retun_url;
                    //string[] strArray = signWeb.Split('|');
                    //Debug.Log("0:" + strArray[0]);
                    //Debug.Log("1:" + strArray[1]);
                    //Debug.Log("2:" + strArray[2]);
                    Debug.Log("money==" + money);
                    Debug.Log("test2==" + coin);
                    Debug.Log("test3==" + ip);
                    Debug.Log("test4==" + time);
                    Debug.Log("test5==" + url);
                    OnHuaweiPayClick(money, orderIDForWeb);
                    //TODO
                    //OnHuaweiPayClick(strArray[0], strArray[1], strArray[2]);
                }
            }
            catch (System.Exception ex)
            {
                Debug.Log("oppo支付" + ex);
            }
        }
    }
    //支付是否成功回调
    public void HuaweiPayCallback(string info)
    {
        Debug.Log("支付是否成功:" + info);
        if (info == "支付成功")
        {
            Debug.Log("支付是否成功1:" + info);
            Facade.GetFacade().message.toolPruchase.SendPayStateRequest(orderIDForWeb);
            MyInfo myInfo = DataControl.GetInstance().GetMyInfo();
            myInfo.bAfterCharge = true;
            Facade.GetFacade().message.backpack.SendReloadRequest();
            Debug.Log("支付是否成功1:" + info);
            Debug.Log("金币:" + huaweiUserInfo.gold);
        }
    }

    //华为支付
    public void OnHuaweiPayClick(int money,string orderID)
    {
        Debug.Log("点击了QQ登录1");
        AndroidJavaClass jc = new AndroidJavaClass("com.unity3d.player.UnityPlayer");
        AndroidJavaObject jo = jc.GetStatic<AndroidJavaObject>("currentActivity");
        jo.Call("PayCallbark",money,orderID);
        Debug.Log("点击了QQ登录2");
    }

    private WWWForm GetOPPOFormData(string goodID)
    {
        string MD5Sign = LoginUtil.GetMD5(key + nMjId + psid + nVersion + goodID + payType + verSdk + gameId + DataControl.GetInstance().GetMyInfo().userID + deviceType).ToLower();
        WWWForm sendData = new WWWForm();
        sendData.AddField("act", "order");
        sendData.AddField("mjid", nMjId);
        sendData.AddField("psid", psid);//0
        sendData.AddField("sign", MD5Sign);
        sendData.AddField("gameid", gameId);// 510
        sendData.AddField("version", nVersion);//  "0.0.0"
        sendData.AddField("versdk", verSdk);// "0.0.0"
        sendData.AddField("goodsid", goodID);// 189
        sendData.AddField("payType", payType);// 314
        sendData.AddField("deviceType", deviceType);// "3"
        sendData.AddField("usrid", DataControl.GetInstance().GetMyInfo().userID);
        Debug.Log("act=" + "order" + "&mjid=" + nMjId + "&sign=" + MD5Sign + "&gameid=" + gameId + "&version=" + nVersion + "&versdk=" + verSdk + "&goodsid=" + goodID + "&payType=" + payType + "&deviceType=" + deviceType + "&usrid=" + DataControl.GetInstance().GetMyInfo().userID);
        return sendData;
    }

    //得到订单
    IEnumerator GetOppoOrderUrl(string url, WWWForm data)
    {
        WWW www = new WWW(url, data);
        yield return www;
        Debug.Log("得到订单:" + www.text);
        if (www.error != null)
        {
            Debug.LogError("GetOppoOrderUrl error:" + www.error);
        }
        else
        {
            try
            {
                Debug.Log("www.text=:" + www.text);
                ProductData = JsonMapper.ToObject(www.text);
                if (!ProductData["code"].ToString().Equals("0"))
                {
                    GetAndroidMakeText(ProductData["msg"].ToString());
                }
                WebOrderResult jsonInfo = JsonMapper.ToObject<WebOrderResult>(www.text);
                Debug.Log("jsonInfo.code:" + jsonInfo.code);
                if (jsonInfo.code == 0)
                {
                    Debug.Log("jsonInfo.code:" + jsonInfo.code);
                    orderIDForWeb = jsonInfo.data.orderid;
                    money = jsonInfo.data.rmb;
                    signWeb = jsonInfo.data.sign;
                    coin = jsonInfo.data.coin.ToString();
                    ip = jsonInfo.data.ip;
                    time = jsonInfo.data.bill_time;
                    url = jsonInfo.data.retun_url;
                    Debug.Log("orderIDForWeb====" + orderIDForWeb);
                    Debug.Log("signWeb====" + signWeb);
                    Debug.Log("test2===" + coin);
                    Debug.Log("test3===" + ip);
                    Debug.Log("test4===" + time);
                    Debug.Log("test5===" + url);
                    money = (money * 100);
                    Debug.Log("money+====" + money);
                    OnOppoPayClick(money, huaweiPayUrl, orderIDForWeb);
                }
            }
            catch (System.Exception ex)
            {
                Debug.Log("oppo支付" + ex);
            }
        }
    }

    /// <param name="number">钱</param>
    /// <param name="url">支付回调地址</param>
    /// <param name="code">订单号</param>
    public void OnOppoPayClick(int number, string url, string code)
    {
        Debug.Log("OnOppoPayClick==1==" + number);
        AndroidJavaClass jc = new AndroidJavaClass("com.unity3d.player.UnityPlayer");
        AndroidJavaObject jo = jc.GetStatic<AndroidJavaObject>("currentActivity");
        jo.Call("OppoPayCallback", number, url, code);
        Debug.Log("OnOppoPayClick==2==" + number);
    }

    //得到form数据
    private WWWForm GetVivoFormData(string goodID)
    {
        string MD5Sign = LoginUtil.GetMD5(key + nMjId + psid + nVersion + goodID + payType + verSdk + gameId + DataControl.GetInstance().GetMyInfo().userID + deviceType).ToLower();
        WWWForm sendData = new WWWForm();
        sendData.AddField("act", "order");
        sendData.AddField("mjid", nMjId);
        sendData.AddField("psid", psid);//0
        sendData.AddField("sign", MD5Sign);
        sendData.AddField("gameid", gameId);// 510
        sendData.AddField("version", nVersion);//  "0.0.0"
        sendData.AddField("versdk", verSdk);// "0.0.0"
        sendData.AddField("goodsid", goodID);// 189
        sendData.AddField("payType", payType);// 314
        sendData.AddField("deviceType", deviceType);// "3"
        sendData.AddField("usrid", DataControl.GetInstance().GetMyInfo().userID);
        Debug.Log("act=" + "order" + "&mjid=" + nMjId + "&sign=" + MD5Sign + "&gameid=" + gameId + "&version=" + nVersion + "&versdk=" + verSdk + "&goodsid=" + goodID + "&payType=" + payType + "&deviceType=" + deviceType + "&usrid=" + DataControl.GetInstance().GetMyInfo().userID);
        return sendData;
    }

    //得到订单
    private IEnumerator GetVivoOrderUrl(string url, WWWForm data)
    {
        WWW www = new WWW(url, data);
        yield return www;
        Debug.Log("得到订单:" + www.text);
        if (www.error != null)
        {
            Debug.LogError("GetOppoOrderUrl error:" + www.error);
        }
        else
        {
            try
            {
                Debug.Log("www.text=:" + www.text);
                ProductData = JsonMapper.ToObject(www.text);
                if (!ProductData["code"].ToString().Equals("0"))
                {
                    GetAndroidMakeText(ProductData["msg"].ToString());
                }
                WebOrderResult jsonInfo = JsonMapper.ToObject<WebOrderResult>(www.text);
                Debug.Log("jsonInfo.code:" + jsonInfo.code);
                if (jsonInfo.code == 0)
                {
                    Debug.Log("jsonInfo.code:" + jsonInfo.code);
                    orderIDForWeb = jsonInfo.data.orderid;
                    signWeb = jsonInfo.data.sign;
                    Debug.Log("orderIDForWeb:" + orderIDForWeb);
                    Debug.Log("signWeb:" + signWeb);
                    string[] strArray = signWeb.Split('|');
                    Debug.Log("strArray[0]====" + strArray[0]);
                    Debug.Log("strArray[1]====" + strArray[1]);
                    Debug.Log("strArray[2]====:" + strArray[2]);
                    OnVivoPayClick(strArray[0], strArray[1], strArray[2]);
                }
            }
            catch (System.Exception ex)
            {
                Debug.Log("oppo支付" + ex);
            }
        }
    }
    /// <param name="number">钱</param>
    /// <param name="code">订单号</param>
    public void OnVivoPayClick(string sign, string code, string number)
    {
        Debug.Log("OnOppoPayClick==1==" + number);
        AndroidJavaClass jc = new AndroidJavaClass("com.unity3d.player.UnityPlayer");
        AndroidJavaObject jo = jc.GetStatic<AndroidJavaObject>("currentActivity");
        jo.Call("VivoPayInterface", sign, code, number);
        Debug.Log("OnOppoPayClick==2==" + number);
    }
    //支付是否成功回调
    public void VivoPayCallback(string info)
    {
        Debug.Log("支付是否成功:" + info);
        if (info == "支付成功")
        {
            Debug.Log("支付是否成功1:" + info);
            Facade.GetFacade().message.toolPruchase.SendPayStateRequest(orderIDForWeb);
            MyInfo myInfo = DataControl.GetInstance().GetMyInfo();
            myInfo.bAfterCharge = true;
            Facade.GetFacade().message.backpack.SendReloadRequest();
            Debug.Log("支付是否成功1:" + info);
            Debug.Log("金币:" + huaweiUserInfo.gold);
        }
    }
    /// <summary>
    /// 打开论坛
    /// </summary>
    public void OnLuntanClick()
    {
        AndroidJavaClass jc = new AndroidJavaClass("com.unity3d.player.UnityPlayer");
        AndroidJavaObject jo = jc.GetStatic<AndroidJavaObject>("currentActivity");
        jo.Call("OpenBBS");
    }
    //获取android吐司
    public void GetAndroidMakeText(string infoText)
    {
        AndroidJavaClass jc = new AndroidJavaClass("com.unity3d.player.UnityPlayer");
        AndroidJavaObject jo = jc.GetStatic<AndroidJavaObject>("currentActivity");
        jo.Call("showToastTips", infoText);
    }
}
