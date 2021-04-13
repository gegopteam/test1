using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using AssemblyCSharp;
public class ShenlongTurnTable : MonoBehaviour
{
    public Button turnBtn;//旋转
    public Button retBtn;//关闭
    public Slider pace;//这个是进度条//进度条三种进度
    public Text paceTxt;//这是数字显示的进度
    public GameObject smallTable;
    bool isMove = false;
    public GameObject getPrizeTips;
    public GameObject getPrizeTipsLongCard;//新增需求龙卡要显示出加成
    public Sprite[] longCard;
    public Sprite[] prizeAddition;
    public Sprite[] smallSurnTable;
    public Sprite[] diamioAndCoin;
    public Image longCardImg;
    public Image prizeAdditionImg;
    public Image smallSurnTableImg;
    public Image diamioAndCoinImg;
    //三种转盘的素材
    public Sprite[] titleImg;
    public Sprite[] numBaseImg;
    public Sprite[] turnTableImg;
    //钻石数量四种
    public Sprite[] numDiamioImg;

    //转盘改变的部分
    public GameObject title;
    public GameObject turnTable;
    public GameObject numBase;

    //钻石改变的部分
    public Image[] numDiamio;
    //奖池改变的部分
    public Image[] numGold;

    //这个是奖品的索引
    int prizeIndex = 5;
  
    //判断场次
    int roomNum = 0;
    //倒计时
    public Text timeDown;
  
    int hour = 0;
    int minute = 0;
    int second = 0;
    float timeSpend = 1;

    //获奖的提示部分
    public GameObject isLongCardBtn;//龙卡用户获奖

    public GameObject noLongCardBtn;//非龙卡得奖
    public GameObject buyLongCardBtn;//购买龙卡
    public GameObject longCard20Tips;//加成提示
    public Image prizeImg;
    public Sprite[] prizeSprite;
    public Image imgTxt;
    public Sprite[] imgTxtSprite;
    //这里购买延期部分
    public GameObject buyDayTipsObj;
    public GameObject buyDaySuccessTipsObj;
    public GameObject buyDayFailTipsObj;

    public GameObject backConfirmPanel;
    int turnTimeNum;
    MyInfo myInfo;
    //对应奖品所需旋转的角度
    int Prize(int index)
    {
        switch (index)
        {
            case 1:
                return 18;
            case 2:
                return 54;
            case 3:
                return 90;
            case 4:
                return 126;
            case 5:
                return 162;
            case 6:
                return 198;
            case 7:
                return 234;
            case 8:
                return 270;
            case 9:
                return 306;
            case 10:
                return 342;
            default:
                return 18;
        }
    }
    void Awake()
    {
        gameObject.GetComponent<Canvas>().worldCamera = ScreenManager.uiCamera;
    }

    void Start()
    {
        roomNum = FishingUpTurnTable._instance.roomType;
        turnBtn.onClick.RemoveAllListeners();
        turnBtn.onClick.AddListener(LuckDraw);
        retBtn.onClick.RemoveAllListeners();
        retBtn.onClick.AddListener(onClose);
        SetNotTurnInFinsing(roomNum);
        //转盘的进度等于渔场上部的进度
        pace.value = FishingUpTurnTable._instance.speedOfProInFishing.value;
        paceTxt.text = FishingUpTurnTable._instance.speedOfProInFishingTxt.text;
        transform.Find("bgImg/numBase/Text").GetComponent<Text>().text = FishingUpTurnTable._instance.GoldCoinPoolNum.ToString();
        myInfo = (MyInfo)Facade.GetFacade().data.Get(FacadeConfig.USERINFO_MODULE_ID);
    }

    void Update()
    {
        //进度倒计时
        timeSpend += Time.deltaTime;
        if (timeSpend>=1)
        {
            hour = (int)FishingUpTurnTable._instance.liuShuiTime / 3600;
            minute =((int)FishingUpTurnTable._instance.liuShuiTime - hour * 3600) / 60;
            second =(int)FishingUpTurnTable._instance.liuShuiTime - hour * 3600 - minute * 60;
            timeDown.text = "進度重置  " + hour.ToString("D2") + ":" + minute.ToString("D2") + ":" + second.ToString("D2");

            timeSpend = 0;
        }
       

    }
    /// <summary>
    /// 设置不同的转盘在不同的渔场中，进度条也加在这里
    /// </summary>
    void SetNotTurnInFinsing(int index)
    {
        title.GetComponent<Image>().sprite = titleImg[index - 1];
        turnTable.GetComponent<Image>().sprite = turnTableImg[index - 1];
        numBase.GetComponent<Image>().sprite = numBaseImg[index - 1];

        switch (index)
        {
            case 1:
                {
                    numDiamio[0].sprite = numDiamioImg[0];
                    numDiamio[1].sprite = numDiamioImg[1];
                    break;
                }
            case 2:
                {
                    numDiamio[0].sprite = numDiamioImg[1];
                    numDiamio[1].sprite = numDiamioImg[2];
                    break;
                }
            case 3:
                {
                    numDiamio[0].sprite = numDiamioImg[2];
                    numDiamio[1].sprite = numDiamioImg[3];
                    break;
                }
            default:
                break;
        }
        for (int i = 0; i < numGold.Length; i++)
        {
            numGold[i].SetNativeSize();
        }
    }
    //旋转奖盘
    public void LuckDraw()
    {
        Debug.Log("myInfo.gold"+myInfo.gold);
        if ((int)pace.value >= 1)
        //if(true)
        {
            Facade.GetFacade().message.fishCommom.SendShenLongLuckDrawRequest();
            Facade.GetFacade().message.fishCommom.SendLongPoolRewardRequest(0);
            StartCoroutine(WaiteLuckDraw());
        }
        else
        {
            //进度不够
            //transform.Find("Tips").gameObject.SetActive(true);
            string path = "Window/WindowTipsThree";
            GameObject WindowClone = AppControl.OpenWindow(path);
            WindowClone.SetActive(true);
            WindowClone.GetComponent<UITipAutoNoMask>().tipText.text = "進度不足";
        }
    }
    //转盘停止后打开按钮
    void openBtn()
    {
        Debug.Log("FishingUpTurnTable._instance.prizeType" + FishingUpTurnTable._instance.prizeType);
        Debug.Log("FishingUpTurnTable._instance.prizeValue" + FishingUpTurnTable._instance.prizeValue);
        GetPrizeTips(FishingUpTurnTable._instance.prizeIndex);
        turnBtn.enabled = true;
        retBtn.enabled = true;
    }

    public void smallTurnTable(bool isShow)
    {
        isMove = isShow;
        Debug.Log("遭到點擊" + isMove);
        if (isShow)
        {
            Debug.Log("111" + isShow);
            isMove = true;
            Tweener nMove = smallTable.GetComponent<Image>().transform.DOLocalMoveX(184f, 1f);
            smallTable.GetComponent<Image>().transform.DOScaleX(1f, 1f);
            // nMove.OnComplete(moveStop);
        }
        else
        {
            Debug.Log("222" + isShow);

            Tweener nMove = smallTable.GetComponent<Image>().transform.DOLocalMoveX(50f, 1f);
            smallTable.GetComponent<Image>().transform.DOScaleX(0f, 1f);
        }
    }

    public void moveStop()
    {
        Debug.Log("移動點擊1");
        smallTurnTable(!isMove);
        Debug.Log("移動點擊2");
    }
    public void onCloseTips()
    {
        transform.Find("Tips").gameObject.SetActive(false);
    }
    public void onCloseTipsLongCard()
    {
        getPrizeTipsLongCard.SetActive(false);
    }
    public void onClose()
    {
        Destroy(this.gameObject);
        UIColseManage.instance.CloseUI();
    }
    //龙卡加成提示
    public void OpenLongCardTips()
    {
        transform.Find("longCardTips").gameObject.SetActive(true);
    }
    //随意点击关闭提示
    public void OnCloseLongCardTips()
    {
        transform.Find("longCardTips").gameObject.SetActive(false);
    }
    //得奖提示
    public void GetPrizeTips(int index)
    {
        
        //transform.Find("Tips/Bg/GetGoldTxt").GetComponent<Text>().text =FishingUpTurnTable._instance.prizeValue.ToString();
		if (!myInfo.misHaveDraCard)
        {
            //非龙卡
            getPrizeTips.SetActive(true);
            SetPrizeGold(FishingUpTurnTable._instance.prizeValue);
            isLongCardBtn.SetActive(false);
            noLongCardBtn.SetActive(true);
            buyLongCardBtn.SetActive(true);
            longCard20Tips.SetActive(true);
            switch (index)
            {
                case 1:
                case 2:
                case 4:
                case 5:
                case 6:
                case 7:
                case 9:
                case 10:
                    prizeImg.sprite = prizeSprite[0];
                    imgTxt.sprite = imgTxtSprite[0];
                    break;
                case 3:
                case 8:
                    prizeImg.sprite = prizeSprite[1];
                    imgTxt.sprite = imgTxtSprite[1];
                    break;
                default:
                    break;
            }
        }
        else
        {
            getPrizeTipsLongCard.SetActive(true);
            SetPrizeGold(FishingUpTurnTable._instance.prizeValue,IsLongCardGetPrize());
            //isLongCardBtn.SetActive(true);
            //noLongCardBtn.SetActive(false);
            //buyLongCardBtn.SetActive(false);
            //longCard20Tips.SetActive(false);
            switch (index)
            {
                case 1:
                case 2:
                case 4:
                case 5:
                case 6:
                case 7:
                case 9:
                case 10:
                    diamioAndCoinImg.sprite = diamioAndCoin[0];
                    diamioAndCoinImg.SetNativeSize();
                    break;
                case 3:
                case 8:
                    diamioAndCoinImg.sprite = diamioAndCoin[1];
                    diamioAndCoinImg.SetNativeSize();
                    break;
                default:
                    break;
            }
        }
       
        prizeImg.SetNativeSize();
    }
    //购买延期提示
    public void BuyDay()
    {
        buyDayTipsObj.SetActive(true);
    }
    public void OncloseBuyDay()
    {
        buyDayTipsObj.SetActive(false);
        buyDayFailTipsObj.SetActive(false);
    }
    //确认购买
    public void BuyDaySuccess()
    {

        if (PrefabManager._instance.GetLocalGun().curretnDiamond>=100)
        {
            Facade.GetFacade().message.fishCommom.SendChangeLiuShuiTimeRequest();
            buyDaySuccessTipsObj.SetActive(true);
            buyDayTipsObj.SetActive(false);
        }
        else
        {
            buyDayFailTipsObj.SetActive(true);
            buyDayTipsObj.SetActive(false);
        }

    }
    //前往商店充值
    public void OpenStore()
    {
        string path = "Window/NewStoreCanvas";
        GameObject WindowClone = AppControl.OpenWindow(path);
        WindowClone.SetActive(true);
    }
    //购买延长
    public void OncloseDaySuccess()
    {
        FishingUpTurnTable._instance.BuyLiuShuiTime();
        PrefabManager._instance.GetLocalGun().gunUI.AddValue(0, 0, -FishingUpTurnTable._instance.shenLongData.liuShuiDiamond);
        buyDaySuccessTipsObj.SetActive(false);
    }
    //确定领取奖励
    public void SureGetPrize()
    {
        if (FishingUpTurnTable._instance.prizeType == FiPropertyType.DIAMOND)
        {
            PrefabManager._instance.GetLocalGun().gunUI.AddValue(0, 0,(int)FishingUpTurnTable._instance.prizeValue);
        }
        else if (FishingUpTurnTable._instance.prizeType == FiPropertyType.GOLD)
        {
            PrefabManager._instance.GetLocalGun().gunUI.AddValue(0, FishingUpTurnTable._instance.prizeValue, 0);
            //myInfo.gold += FishingUpTurnTable._instance.prizeValue;
        }
        FishingUpTurnTable._instance.prizeType = 0;
        FishingUpTurnTable._instance.prizeValue = 0;
        FishingUpTurnTable._instance.prizeIndex = 0;
        transform.Find("Tips").gameObject.SetActive(false);
        getPrizeTipsLongCard.SetActive(false);
    }
    public void Btn_Confirm()
    {
        //返回大厅购买龙卡
        GameObject temp = GameObject.Instantiate(backConfirmPanel);
        temp.GetComponent<Canvas>().enabled = false;
        temp.SetActive(false);
        temp.GetComponent<AskBackPanel>().Show("");
        UIColseManage.instance.isFishingBuyLongCard = true;
        temp.GetComponent<AskBackPanel>().Btn_Confirm(); 
    }
    bool ReturnType()
    {
        return FishingUpTurnTable._instance.prizeType == 0;
    }

    IEnumerator WaiteLuckDraw()
    {
        yield return new WaitWhile(ReturnType) ;
        if (FishingUpTurnTable._instance.prizeIndex==0)
        {
            string path = "Window/WindowTipsThree";
            GameObject WindowClone = AppControl.OpenWindow(path);
            WindowClone.SetActive(true);
            WindowClone.GetComponent<UITipAutoNoMask>().tipText.text = "抽獎異常";
            StopCoroutine("WaiteLuckDraw");
        }
        Facade.GetFacade().message.fishCommom.SendLongPoolRewardRequest(0);
        FishingUpTurnTable._instance.speedOfProFishingGoldNum = 0;
        FishingUpTurnTable._instance.shenLongData.LongLiuShui = 0;
        FishingUpTurnTable._instance.SetSpeedOfPro(0);
        pace.value = 0;
        paceTxt.text = 0 + "%";
        FishingUpTurnTable._instance.effectZhuanPanGod.SetActive(false);
        FishingUpTurnTable._instance.effectZhuanPanGold.SetActive(false);
        FishingUpTurnTable._instance.effectZhuanPanSilver.SetActive(false);

        turnBtn.enabled = false;
        retBtn.enabled = false;
        Debug.Log(FishingUpTurnTable._instance.prizeType+"FishingUpTurnTable._instance.prizeType");
        DragonCardInfo shenLongData = (DragonCardInfo)Facade.GetFacade().data.Get(FacadeConfig.UI_DRAGONCARD);
        float nAngle = Prize(FishingUpTurnTable._instance.prizeIndex);
        Tweener nReturn = turnTable.transform.DORotate(new Vector3(0, 0, 360 * 4 + nAngle), 4f, RotateMode.FastBeyond360);
        turnBtn.enabled = false;
        retBtn.enabled = false;
        Invoke("openBtn", 4f);
    }
    //IEnumerator GetRefreshGoldPoo()
    //{
    //    yield return new WaitForSeconds(60);
    //}

    void SetPrizeGold(long nCount)
    {
        if (nCount >= 100000)
        {
            transform.Find("Tips/Bg/GetGoldTxt").GetComponent<Text>().text = "" + (int)(nCount / 10000) + "萬";
        }
        else
        {
            transform.Find("Tips/Bg/GetGoldTxt").GetComponent<Text>().text = "" + nCount;
            Debug.Log(nCount);
        }
    }
    /// <summary>
    /// 有龙卡的用户计算加成的金币并显示
    /// </summary>
    /// <param name="nCount">N count.</param>
    /// <param name="longCardType">Long card type.</param>
    void SetPrizeGold(long nCount,int longCardType)
    {
        long additionCoin = 0;
        long realityCoin = 0;
        switch (longCardType)
        {
            case 1:
                realityCoin = (long)(nCount/1.1f);
                additionCoin = nCount - realityCoin;
                longCardImg.sprite = longCard[0];
                prizeAdditionImg.sprite = prizeAddition[0];
                getPrizeTipsLongCard.transform.Find("Bg/LongCaedName").GetComponent<Text>().text = "銀龍卡";
                //smallSurnTableImg.sprite = smallSurnTable[0];
                break;
            case 2:
                realityCoin = (long)(nCount / 1.15f);
                additionCoin = nCount - realityCoin;
                longCardImg.sprite = longCard[1];
                prizeAdditionImg.sprite = prizeAddition[1];
                getPrizeTipsLongCard.transform.Find("Bg/LongCaedName").GetComponent<Text>().text = "金龍卡";
                //smallSurnTableImg.sprite = smallSurnTable[1];
                break;
            case 3:
                realityCoin = (long)(nCount / 1.2f);
                additionCoin = nCount - realityCoin;
                longCardImg.sprite = longCard[2];
                prizeAdditionImg.sprite = prizeAddition[2];
                getPrizeTipsLongCard.transform.Find("Bg/LongCaedName").GetComponent<Text>().text = "神龍卡";
                //smallSurnTableImg.sprite = smallSurnTable[2];
                break;
        }
        if (roomNum>=3)
        {
            roomNum = 3;
        }
        smallSurnTableImg.sprite = smallSurnTable[roomNum-1];

        if (realityCoin >= 1000000)
        {
            getPrizeTipsLongCard.transform.Find("Bg/turnTableDi/zhuanpanCoin").GetComponent<Text>().text = (realityCoin / 10000) + "萬";
        }
        else
        {
            getPrizeTipsLongCard.transform.Find("Bg/turnTableDi/zhuanpanCoin").GetComponent<Text>().text =  realityCoin.ToString();
            Debug.Log(realityCoin+ "正常金幣");
        }
        if (additionCoin >= 1000000)
        {
            getPrizeTipsLongCard.transform.Find("Bg/longTextDi/longcardCoin").GetComponent<Text>().text =  (additionCoin / 10000) + "萬";
        }
        else
        {
            getPrizeTipsLongCard.transform.Find("Bg/longTextDi/longcardCoin").GetComponent<Text>().text =  additionCoin.ToString();
            Debug.Log(additionCoin+ "加成金幣");
        }
        if (nCount >= 10000000)
        {
            getPrizeTipsLongCard.transform.Find("Bg/zongDi/Text").GetComponent<Text>().text = (nCount / 10000) + "萬";
        }
        else
        {
            getPrizeTipsLongCard.transform.Find("Bg/zongDi/Text").GetComponent<Text>().text = nCount.ToString();
            Debug.Log(nCount + "最終金幣");
        }

    }
    /// <summary>
    /// 判断用户是那种龙卡，0无，1银，2金，3神
    /// </summary>
    /// <returns>The long card get prize.</returns>
    int IsLongCardGetPrize()
    {
        DragonCardInfo shenLongData = (DragonCardInfo)Facade.GetFacade().data.Get(FacadeConfig.UI_DRAGONCARD);
        if (shenLongData.IsPurDragonTypeArray[2]>=1)
        {
            return 3;
        }
        else if (shenLongData.IsPurDragonTypeArray[1] >= 1)
        {
            return 2;
        }
        else if (shenLongData.IsPurDragonTypeArray[0] >= 1)
        {
            return 1;
        }
        else 
        {
            return 0;
        }
    }
}
