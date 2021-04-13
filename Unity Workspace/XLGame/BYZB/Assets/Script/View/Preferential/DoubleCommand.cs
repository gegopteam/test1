using UnityEngine;
using System.Collections;
using AssemblyCSharp;
using UnityEngine.UI;
using System.Collections.Generic;

public class DoubleCommand : MonoBehaviour,IUiMediator
{
   
    RoomInfo myRoomInfo = null;
    public static DoubleCommand Instance;
    MyInfo myInfo;
    [SerializeField]
    private GameObject mouthcard;
    [SerializeField]
    private Camera mouthcardCamera;
    [SerializeField]
    private Canvas mainCanvas;
    public Button show_128;
    public Button show_388;

    public Text[] showGold;
    public Text[] ShowRmb;

    private int[] DefaultRMB = new int[] { 50, 100 };
    private long[] DefaultGold = new long[] { 896000, 1840000 };

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
        mainCanvas.sortingOrder = 45;
        myRoomInfo = DataControl.GetInstance().GetRoomInfo();
        myInfo = (MyInfo)Facade.GetFacade().data.Get(FacadeConfig.USERINFO_MODULE_ID);
        giftBagType = GiftBagType.Double;
    }

    private void Start()
    {
        if (myInfo.Pay_Two_RMB.Count > 1)
        {
            //myInfo.Pay_Two_RMB.Sort();
            //myInfo.Pay_Two_AddGold.Sort();
            for (int btn = 0; btn < myInfo.Pay_Two_RMB.Count; btn++)
            {
                showGold[btn].text = "" + myInfo.Pay_Two_AddGold[btn];
                ShowRmb[btn].text = "" + myInfo.Pay_Two_RMB[btn];
            }
        }
        else {
            for (int btn = 0; btn < DefaultGold.Length; btn++)
            {
                showGold[btn].text = "" + DefaultGold[btn];
                ShowRmb[btn].text = "" + DefaultRMB[btn];
            }
        }
    }

    public void OnPay_128Click()
    {
        int pay_gold = 30;
        if (myInfo.Pay_Two_RMB.Count > 1)
            pay_gold = (int)myInfo.Pay_Two_RMB[0];
        else
            pay_gold = DefaultRMB[0];

        UIToPay.DarGonCardType = 51013;
        UIToPay.OpenThirdPartPay(pay_gold);
        //ShowTest(13);
    }
    public void OnPay_388Click()
    {
        int pay_gold = 30;
        if (myInfo.Pay_Two_RMB.Count > 1)
            pay_gold = (int)myInfo.Pay_Two_RMB[1];
        else
            pay_gold = DefaultRMB[1];

        UIToPay.DarGonCardType = 51014;
        UIToPay.OpenThirdPartPay(pay_gold);
        //ShowTest(14);
    }

    public void OnDeleteClick()
    {
        Destroy(this.gameObject);
    }

    //void ShowTest(int type)
    //{
    //    string Urltest;
    //    Urltest = "http://183.131.69.227:8004/pay/notifygameserverfortest" + "?UserID=" + myInfo.userID + "&cardtype=" + type;
    //    Debug.LogError("Urltest" + Urltest);
    //    //      OpenWebScript.Instance.SetActivityWebUrl (Urltest);
    //    Application.OpenURL(Urltest);

    //}

    IEnumerator ShowImage()
    {
        yield return new WaitForSeconds(1f);

    }

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
        Destroy(this.gameObject);
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
