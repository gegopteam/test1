using UnityEngine;
using System.Collections;
using LitJson;
using AssemblyCSharp;

public class OppoPay : MonoBehaviour
{
#if UNITY_ANDROID
    //oppo请求订单地址
    //paytype 314
    private string OppoOrderUrl = "http://api.cl0579.com/api/MobilePayConfig.aspx";
    private string oppoPayUrl = "http://notify.clgame.cc/OPPO_M_XL.aspx";
    private JsonData ProductData = null;
    private string signWeb;
    protected string buyProductId = "";
    private string orderIDForWeb = "";
    MyInfo userInfo;
    private string nMjId = "8";
    private int psid = 0;
    private string nVersion = "0.0.0";
    private string goodsid = "189";
    private int payType = 314;
    private string verSdk = "0.0.0";
    private int gameId = 510;
    private string deviceType = "3";
    string key = "E39,D-=SM39S10-ATU85NC,53\\kg9";
    private int money;
    private void Start()
    {
        userInfo = DataControl.GetInstance().GetMyInfo();
        MyInfo nInfo = (MyInfo)Facade.GetFacade().data.Get(FacadeConfig.USERINFO_MODULE_ID);
    }

    public void RequestOppoProductsData_5()
    {
        StartCoroutine(GetOppoOrderUrl(OppoOrderUrl, GetFormData("398")));
    }
    public void RequestOppoProductsData_10()
    {
        StartCoroutine(GetOppoOrderUrl(OppoOrderUrl, GetFormData("399")));
    }
    public void RequestOppoProductsData_50()
    {
        StartCoroutine(GetOppoOrderUrl(OppoOrderUrl, GetFormData("400")));
    }
    public void RequestOppoProductsData_100()
    {
        StartCoroutine(GetOppoOrderUrl(OppoOrderUrl, GetFormData("401")));
    }
    public void RequestOppoProductsData_200()
    {
        StartCoroutine(GetOppoOrderUrl(OppoOrderUrl, GetFormData("402")));
    }
    public void RequestOppoProductsData_500()
    {
        StartCoroutine(GetOppoOrderUrl(OppoOrderUrl, GetFormData("403")));
    }
    //银龙
    public void RequestOppoProductsData_Yinlong()
    {
        StartCoroutine(GetOppoOrderUrl(OppoOrderUrl, GetFormData("404")));
    }
    //金龙
    public void RequestOppoProductsData_Jinlong()
    {
        StartCoroutine(GetOppoOrderUrl(OppoOrderUrl, GetFormData("405")));
    }
    //神龙
    public void RequestOppoProductsData_Shenlong()
    {
        StartCoroutine(GetOppoOrderUrl(OppoOrderUrl, GetFormData("406")));
    }
    //特惠12
    public void RequestOppoProductsData_Preferential_12()
    {
        StartCoroutine(GetOppoOrderUrl(OppoOrderUrl, GetFormData("407")));
    }
    //特惠38
    public void RequestOppoProductsData_Preferential_38()
    {
        StartCoroutine(GetOppoOrderUrl(OppoOrderUrl, GetFormData("408")));
    }
    //特惠68
    public void RequestOppoProductsData_Preferential_68()
    {
        StartCoroutine(GetOppoOrderUrl(OppoOrderUrl, GetFormData("409")));
    }
    //特惠98
    public void RequestOppoProductsData_Preferential_98()
    {
        StartCoroutine(GetOppoOrderUrl(OppoOrderUrl, GetFormData("410")));
    }
    //特惠188
    public void RequestOppoProductsData_Preferential_188()
    {
        StartCoroutine(GetOppoOrderUrl(OppoOrderUrl, GetFormData("411")));
    }
    //给力
    public void RequestOppoProductsData_Awesome_1()
    {
        StartCoroutine(GetOppoOrderUrl(OppoOrderUrl, GetFormData("412")));
    }
    //宝藏40
    public void RequestOppoProductsData_Treasure_40()
    {
        StartCoroutine(GetOppoOrderUrl(OppoOrderUrl, GetFormData("413")));
    }
    //宝藏68
    public void RequestOppoProductsData_Treasure_68()
    {
        StartCoroutine(GetOppoOrderUrl(OppoOrderUrl, GetFormData("414")));
    }
    //宝藏128
    public void RequestOppoProductsData_Treasure_128()
    {
        StartCoroutine(GetOppoOrderUrl(OppoOrderUrl, GetFormData("415")));
    }
    //双喜128
    public void RequestOppoProductsData_Double_128()
    {
        StartCoroutine(GetOppoOrderUrl(OppoOrderUrl, GetFormData("416")));
    }
    //双喜388
    public void RequestOppoProductsData_Double_388()
    {
        StartCoroutine(GetOppoOrderUrl(OppoOrderUrl, GetFormData("417")));
    }
    private WWWForm GetFormData(string goodID)
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

    private string test1;
    private string test2;
    private string test3;
    private string test4;
    private string test5;
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
                    test2 = jsonInfo.data.coin.ToString();
                    test3 = jsonInfo.data.ip;
                    test4 = jsonInfo.data.bill_time;
                    test5 = jsonInfo.data.retun_url;
                    Debug.Log("orderIDForWeb====" + orderIDForWeb);
                    Debug.Log("signWeb====" + signWeb);
                    Debug.Log("test2===" + test2);
                    Debug.Log("test3===" + test3);
                    Debug.Log("test4===" + test4);
                    Debug.Log("test5===" + test5);
                    money = (money * 100);
                    Debug.Log("money+====" + money);
                    OnOppoPayClick(money, oppoPayUrl, orderIDForWeb);
                }
            }
            catch (System.Exception ex)
            {
                Debug.Log("oppo支付" + ex);
            }
        }
    }
    //oppo验证收据
    protected void OppoVerifyReceipt(int nUserId, string webOrderId, string oppoOrderId, string receipt, string passwd)
    {
        string nVerifyUrl = "http://notify.clgame.cc/OPPO_M_XL.aspx";
        WWWForm sendData = new WWWForm();
        sendData.AddField("amount", getAmount());
        sendData.AddField("mode", 0);
        sendData.AddField("receiptdata", receipt);
        sendData.AddField("userid", nUserId);
        sendData.AddField("majianame", nMjId);
        sendData.AddField("orderid", webOrderId);
        sendData.AddField("psid", psid);
        sendData.AddField("gameid", gameId);
        sendData.AddField("version", nVersion);
        sendData.AddField("oppoordernum", oppoOrderId);
        sendData.AddField("pwd", passwd);
        string nSign = key + nMjId + psid.ToString() + gameId.ToString() + nVersion;
        sendData.AddField("sign", LoginUtil.GetMD5(nSign).ToLower());

        string vskey = nUserId.ToString() + passwd + webOrderId + key;
        sendData.AddField("key", LoginUtil.GetMD5(vskey).ToLower());
        PurchaseControl.GetInstance().buyGoldCoin = PurchaseControl.GetInstance().GetPurchasePrice(buyProductId);
        StartCoroutine(PostOppoVerify(nVerifyUrl, sendData));
    }
    //支付回调验证
    IEnumerator PostOppoVerify(string url, WWWForm data)
    {
        WWW www = new WWW(url, data);
        yield return www;
        if (www.error != null)
        {
            Debug.Log("oppo支付回调验证错误" + www.error);
        }
        else
        {
            Debug.Log("oppo支付回调验证返回结果" + www.text);
            Facade.GetFacade().message.toolPruchase.SendPayStateRequest(orderIDForWeb);
            MyInfo myInfo = DataControl.GetInstance().GetMyInfo();
            myInfo.bAfterCharge = true;
            Facade.GetFacade().message.backpack.SendReloadRequest();
        }
    }

    public void PayCallback(string info)
    {
        Debug.Log("支付是否成功:" + info);
        if (info == "成功")
        {
            Debug.Log("支付是否成功1:" + info);
            Facade.GetFacade().message.toolPruchase.SendPayStateRequest(orderIDForWeb);
            MyInfo myInfo = DataControl.GetInstance().GetMyInfo();
            myInfo.bAfterCharge = true;
            Facade.GetFacade().message.backpack.SendReloadRequest();
            Debug.Log("支付是否成功1:" + info);
            Debug.Log("金币:" + userInfo.gold);
        }
    }
    public JsonData GetProducts()
    {
        return ProductData;
    }
    int getAmount()
    {
        switch (buyProductId)
        {
            case ProductID.CL_GOLD_6:
                return 6;
            case ProductID.CL_GOLD_18:
                return 18;
            case ProductID.CL_GOLD_50:
                return 50;
            case ProductID.CL_GOLD_98:
                return 98;
            case ProductID.CL_GOLD_198:
                return 198;
            case ProductID.CL_GOLD_488:
                return 488;
        }
        return 0;
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
    //获取android吐司
    public void GetAndroidMakeText(string infoText)
    {
        AndroidJavaClass jc = new AndroidJavaClass("com.unity3d.player.UnityPlayer");
        AndroidJavaObject jo = jc.GetStatic<AndroidJavaObject>("currentActivity");
        jo.Call("GetMakeText", infoText);
    }
#endif
}
