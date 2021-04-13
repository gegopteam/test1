using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using AssemblyCSharp;
using System.Collections.Generic;

public class AwesomeCommand : MonoBehaviour,IUiMediator
{

    public Image showImage;
    public static AwesomeCommand Instance;
    MyInfo myInfo;
    private GameObject mouthcard;
    [SerializeField]
    private Camera mouthcardCamera;
    [SerializeField]
    private Canvas mainCanvas;
    public Button showButton;
    private GiftBagType giftBagType;
    private void Awake()
    {

        Facade.GetFacade().ui.Add(FacadeConfig.UI_TEIHUI, this);

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
        Instance = this;
        showImage.gameObject.SetActive(false);
        myInfo = (MyInfo)Facade.GetFacade().data.Get(FacadeConfig.USERINFO_MODULE_ID);
        giftBagType = GiftBagType.Awesome;
    }
    /// <summary>
    /// 关闭窗口
    /// </summary>
    public void OnDeleteClick()
    {
        //myInfo.isShowAwesome = true;
        Destroy(this.gameObject);
    }

    /// <summary>
    /// 点击支付
    /// </summary>
    public void OnPayClick()
    {
        UIToPay.DarGonCardType = 5109;
        UIToPay.OpenThirdPartPay(1);
        //ShowTest(9);
    }

    /// <summary>
    /// 显示购买气泡
    /// </summary>
    public void ShowDubbble()
    {
        showImage.gameObject.SetActive(true);
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

    void OnDestroy()
    {
        Facade.GetFacade().ui.Remove(FacadeConfig.UI_TEIHUI);
    }
}
