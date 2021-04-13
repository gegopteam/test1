using UnityEngine;
using System.Collections;
using AssemblyCSharp;
using UnityEngine.UI;
using System.Collections.Generic;

public class TreasureCommand : MonoBehaviour,IUiMediator
{

    MyInfo myInfo;
    private GameObject mouthcard;
    [SerializeField]
    private Camera mouthcardCamera;
    [SerializeField]
    private Canvas mainCanvas;
    public Text[] showGold;
    public Text[] ShowRmb;

    private int[] DefaultRMB = new int[] { 30, 50, 100 };
    private long[] DefaultGold = new long[] { 480000, 800000, 1600000 };

    public static TreasureCommand Instance;
    public Button[] treasure;
    private GiftBagType giftBagType;

    private void Awake()
    {
        Facade.GetFacade().ui.Add(FacadeConfig.UI_TEIHUI, this);
        Instance = this;
        if (GameController._instance == null)
        {
            mouthcard = GameObject.FindGameObjectWithTag("MainCamera");
        }
        else
        {
            mouthcard = GameObject.FindGameObjectWithTag(TagManager.uiCamera);
        }
        Debug.Log(mouthcard.name);
        mouthcardCamera = mouthcard.GetComponent<Camera>();
        mainCanvas = transform.GetComponentInChildren<Canvas>();
        mainCanvas.worldCamera = mouthcardCamera;
        mainCanvas.planeDistance = 30;
        mainCanvas.sortingOrder = 10;
        myInfo = (MyInfo)Facade.GetFacade().data.Get(FacadeConfig.USERINFO_MODULE_ID);
        giftBagType = GiftBagType.Treasure;
    }

    private void Start()
    {
        if (myInfo.Pay_Three_RMB.Count > 2)
        {
            //myInfo.Pay_Three_RMB.Sort();
            //myInfo.Pay_Three_AddGold.Sort();
            for (int btn = 0; btn < myInfo.Pay_Three_RMB.Count; btn++)
            {
                showGold[btn].text = "" + myInfo.Pay_Three_AddGold[btn];
                ShowRmb[btn].text = "" + myInfo.Pay_Three_RMB[btn];
            }
        }
        else {
            for (int btn = 0; btn < myInfo.Pay_Three_RMB.Count; btn++)
            {
                showGold[btn].text = "" + DefaultGold[btn];
                ShowRmb[btn].text = "" + DefaultRMB[btn];
            }
        }
    }

    public void OnDeleteClick()
    {
        myInfo.isShowTreasure = true;
        Destroy(this.gameObject);
    }

    public void OnPay_40Click()
    {
        UIToPay.DarGonCardType = 51010;
        if (myInfo.Pay_Three_RMB.Count > 2)
            UIToPay.OpenThirdPartPay((int)myInfo.Pay_Three_RMB[0]);
        else
            UIToPay.OpenThirdPartPay(DefaultRMB[0]);
        //UIToPay.OpenThirdPartPay(40);
        //ShowTest(10);
    }

    public void OnPay_68Click()
    {

        UIToPay.DarGonCardType = 51011;
        if (myInfo.Pay_Three_RMB.Count > 2)
            UIToPay.OpenThirdPartPay((int)myInfo.Pay_Three_RMB[1]);
        else
            UIToPay.OpenThirdPartPay(DefaultRMB[1]);
        //UIToPay.OpenThirdPartPay(68);
        //ShowTest(11);
    }

    public void OnPay_128Click()
    {
        UIToPay.DarGonCardType = 51012;
        if (myInfo.Pay_Three_RMB.Count > 2)
            UIToPay.OpenThirdPartPay((int)myInfo.Pay_Three_RMB[2]);
        else
            UIToPay.OpenThirdPartPay(DefaultRMB[2]);
        //UIToPay.OpenThirdPartPay(128);
        //ShowTest(12);
    }

    //void ShowTest(int type)
    //{
    //    string Urltest;
    //    Urltest = "http://183.131.69.227:8004/pay/notifygameserverfortest" + "?UserID=" + myInfo.userID + "&cardtype=" + type;
    //    Debug.LogError("Urltest" + Urltest);
    //    //      OpenWebScript.Instance.SetActivityWebUrl (Urltest);
    //    Application.OpenURL(Urltest);

    //}

    public void OnRecvData(int nType, object nData)
    {
        Debug.LogError("接收特惠购买后的信息OnRecvData+++++++++++++++++++++");

        FiPurChaseTehuiRewradData nResult = (FiPurChaseTehuiRewradData)nData;

        Debug.LogError("接收特惠购买后的信息OnRecvData______________________" + nResult.cardType);
        if (nType == FiEventType.RECV_PURCHASE_TEHUI_CARD_RESPONSE)
        {
            Debug.LogError("接收特惠购买后的信息nResult.cardType" + nResult.cardType);
            ShowReward(nResult.prop, 0);

        }
    }
    //显示奖励
    public void ShowReward(List<FiProperty> nPorpList, int type)
    {
        Debug.LogError("接收特惠购买后的信息" + type + "长度" + nPorpList.Count);
        UnityEngine.GameObject Window = UnityEngine.Resources.Load("Window/RewardWindow") as UnityEngine.GameObject;
        GameObject rewardInstance = UnityEngine.GameObject.Instantiate(Window);
        Debug.LogError("接收特惠购买后的信息创建成功" + type + "长度" + nPorpList.Count);
        UIReward reward = rewardInstance.GetComponent<UIReward>();
        reward.ShowRewardType(type);
        reward.SetRewardData(nPorpList);
    }

    public void OnInit()
    {
       
    }

    public void OnRelease()
    {
        
    }

    private void OnDestroy()
    {
        Facade.GetFacade().ui.Remove(FacadeConfig.UI_TEIHUI);
    }
}
