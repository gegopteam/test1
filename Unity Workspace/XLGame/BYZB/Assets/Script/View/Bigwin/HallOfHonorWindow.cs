using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System.Collections.Generic;
using DG.Tweening;
using AssemblyCSharp;
public class HallOfHonorWindow : MonoBehaviour
{

	InfinityGridListHallOfHonor infinityGridLayoutGroup;
    List<Transform> currentTransList;

	//人数
	public int amount;

    public Sprite[] honorPrizeBtnImg;
    public Sprite defaultHead;

	Button closeBtn;
	Button closePoolExplainBtn;
	Button openPoolExplainBtn;
	Button openMainExplainBtn;
    Button getHonorPrizeBtn;
	GameObject poolExplainWindow;
    NobelInfo nobelInfo;
    MyInfo myInfo;
    Text hotPool;
    Text nikeName;
    Text totalFlow;
    Text currentFlow;
    Text currentHot;
    Image headImg;
    GameObject playerDataObj;
    /// <summary>
    /// 所有玩家的总流水,用作计算红利百分比
    /// </summary>
    long totalPlayerFlow = 0;
    //红利
    long poolNum = 0;

    bool myCanGetProze = false;
    private void Awake()
    {
    }
    void Start ()
	{
        currentTransList = new List<Transform>();
        closeBtn = transform.Find("closeButton").GetComponent<Button>();
        closeBtn.onClick.RemoveAllListeners();
        closeBtn.onClick.AddListener(CloseWindow);
        playerDataObj = transform.Find("Panel_Scroll/Panel_Grid/PlayerData").gameObject;
        //StartCoroutine(RefreshGoldPoolLocal());//停止了奖池跳动的协程
        if (GameController._instance == null) //渔场和大厅里赋值的摄像机不同
            gameObject.GetComponent<Canvas>().worldCamera = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<Camera>();
        else
            gameObject.GetComponent<Canvas>().worldCamera = ScreenManager.uiCamera;

        nobelInfo = (NobelInfo)Facade.GetFacade().data.Get(FacadeConfig.UI_NOBEL_ID);
        myInfo = (MyInfo)Facade.GetFacade().data.Get(FacadeConfig.USERINFO_MODULE_ID);
        amount = nobelInfo.rongYuRankInfoList.Count;
		////初始化数据列表;
		UIColseManage.instance.ShowUI (this.gameObject);
		infinityGridLayoutGroup = transform.Find ("Panel_Scroll/Panel_Grid").GetComponent<InfinityGridListHallOfHonor> ();
		infinityGridLayoutGroup.SetAmount (amount);
		infinityGridLayoutGroup.updateChildrenCallbackNew = UpdateChildrenCallback;
         
		
		poolExplainWindow = transform.Find ("PoolExplainWindow").gameObject;
		poolExplainWindow.SetActive (false);
		closePoolExplainBtn = transform.Find ("PoolExplainWindow/closeBtn").GetComponent<Button> ();
		closePoolExplainBtn.onClick.RemoveAllListeners ();
		closePoolExplainBtn.onClick.AddListener (ClosePoolExplain);
		openPoolExplainBtn = transform.Find ("PoolExplainBtn").GetComponent<Button> ();
		openPoolExplainBtn.onClick.RemoveAllListeners ();
		openPoolExplainBtn.onClick.AddListener (OpenPoolExplain);

		openMainExplainBtn = transform.Find ("MainExplainBtn").GetComponent<Button> ();
		openMainExplainBtn.onClick.RemoveAllListeners ();
		openMainExplainBtn.onClick.AddListener (OpenMainExplain);

        getHonorPrizeBtn = transform.Find("GetHonorPrizeBtn").GetComponent<Button>();
        getHonorPrizeBtn.onClick.RemoveAllListeners();
        getHonorPrizeBtn.onClick.AddListener(GetPrize);

        hotPool = transform.Find("GoldPoolNum").GetComponent<Text>();
        poolNum = nobelInfo.hotPrizePool;
        hotPool.text = poolNum.ToString();
        Debug.LogError(""+nobelInfo.rongYuRankInfoList.Count);
        isGetHonorPrize(false);
        if(nobelInfo.rongYuRankInfoList.Count==0)
        {
            isGetHonorPrize(false);
            transform.Find("Panel_Scroll/Panel_Grid/PlayerData/PlayerData").gameObject.SetActive(false);
            Debug.LogError("daozheli" + nobelInfo.rongYuRankInfoList.Count);
        }
        else
        {
            //根据自己的排名 滑动到自己所在的位置
            for (int i = 0; i < nobelInfo.rongYuRankInfoList.Count; i++)
            {
               // Debug.LogError(i+nobelInfo.rongYuRankInfoList[i].nickname+"userId" + nobelInfo.rongYuRankInfoList[i].userId+"流水gold:"+nobelInfo.rongYuRankInfoList[i].gold+"流水shangliu"+nobelInfo.rongYuRankInfoList[i].shangLiuShui);
                if (myInfo.userID == nobelInfo.rongYuRankInfoList[i].userId)
                {
                    if (nobelInfo.honorPrizeState == 0)
                    {
                        isGetHonorPrize(true);
                    }
                    switch (i)
                    {
                        case 0:
                        case 1:
                        case 2:
                        case 3:
                        case 4:
                            break;
                        case 5:
                        case 6:
                        case 7:
                        case 8:
                        case 9:
                            transform.Find("Panel_Scroll/Panel_Grid").GetComponent<RectTransform>().DOLocalMoveY(200f,1f);
                            //transform.Find("Panel_Scroll/Panel_Grid").GetComponent<RectTransform>().offsetMin = new Vector2(transform.Find("Panel_Scroll/Panel_Grid").GetComponent<RectTransform>().offsetMin.x, -212f);//移动到自己的位置
                            //transform.Find("Panel_Scroll/Panel_Grid").GetComponent<RectTransform>().offsetMax = new Vector2(transform.Find("Panel_Scroll/Panel_Grid").GetComponent<RectTransform>().offsetMax.x, -228f);
                            break;
                        case 10:
                        case 11:
                        case 12:
                        case 13:
                        case 14:
                            transform.Find("Panel_Scroll/Panel_Grid").GetComponent<RectTransform>().DOLocalMoveY(440f, 1f);
                            break;
                        case 15:
                        case 16:
                        case 17:
                        case 18:
                        case 19:
                            transform.Find("Panel_Scroll/Panel_Grid").GetComponent<RectTransform>().DOLocalMoveY(680f, 1f);
                            break;
                        case 20:
                        case 21:
                        case 22:
                        case 23:
                        case 24:
                            transform.Find("Panel_Scroll/Panel_Grid").GetComponent<RectTransform>().DOLocalMoveY(920f, 1f);
                            break;
                        case 25:
                        case 26:
                        case 27:
                        case 28:
                        case 29:
                            transform.Find("Panel_Scroll/Panel_Grid").GetComponent<RectTransform>().DOLocalMoveY(1160f, 1f);
                            break;
                        default:
                            break;
                    }
                    Debug.LogError("dakai打开领取nobelInfo.rongYuRankInfoList[i].userId"+nobelInfo.rongYuRankInfoList[i].userId);
                }
                //else
                //{
                    
                //    Debug.LogError("nobelInfo.rongYuRankInfoList[i].userId" + nobelInfo.rongYuRankInfoList[i].userId);
                //}
                totalPlayerFlow += nobelInfo.rongYuRankInfoList[i].gold;
                GameObject newPlayer = Instantiate(playerDataObj, transform.Find("Panel_Scroll/Panel_Grid"));
                newPlayer.SetActive(true);

            }
            Destroy(playerDataObj);
        }


    }
    /// <summary>
    /// 每次滑动界面时就会调用，用来替换UI的显示
    /// </summary>
    /// <param name="index">Index.</param>
    /// <param name="trans">Trans.</param>
	void UpdateChildrenCallback (int index, Transform trans)
	{
		Debug.Log ("UpdateChildrenCallback: index=" + index + " name:" + trans.name);

        if (nobelInfo.rongYuRankInfoList.Count!=0)
        {
            trans.Find("PlayerData/NikeName").GetComponent<Text>().text = Tool.GetName(nobelInfo.rongYuRankInfoList[index].nickname,5);
            trans.Find("PlayerData/TotalFlow").GetComponent<Text>().text = SetGold(nobelInfo.rongYuRankInfoList[index].shangLiuShui);
            trans.Find("PlayerData/CurrentFlow").GetComponent<Text>().text =SetGold(nobelInfo.rongYuRankInfoList[index].gold);
        }
        else
        {
            return;
        }

        if (totalPlayerFlow == 0)
        {
            trans.Find("PlayerData/CurrentHot").GetComponent<Text>().text = 0 + "%";
        }
        else if (nobelInfo.rongYuRankInfoList[index].gold == 0)
        {
            trans.Find("PlayerData/CurrentHot").GetComponent<Text>().text = 0 + "%";
        }
        else if ((float)nobelInfo.rongYuRankInfoList[index].gold / (float)totalPlayerFlow < 0.01 && (float)nobelInfo.rongYuRankInfoList[index].gold / (float)totalPlayerFlow > 0)
        {
            trans.Find("PlayerData/CurrentHot").GetComponent<Text>().text = 0 + "%";
        }
        else if ((float)nobelInfo.rongYuRankInfoList[index].gold / (float)totalPlayerFlow >= 0.01) 
        {
            trans.Find("PlayerData/CurrentHot").GetComponent<Text>().text = ((float)nobelInfo.rongYuRankInfoList[index].gold / (float)totalPlayerFlow).ToString("P0");
        }

        if (nobelInfo.rongYuRankInfoList[index].xingxing!=0)
        {
            //星星最多只有六个
            if (nobelInfo.rongYuRankInfoList[index].xingxing>=6)
            {
                nobelInfo.rongYuRankInfoList[index].xingxing = 6;
            }
            for (int j = 0; j < nobelInfo.rongYuRankInfoList[index].xingxing; j++)
            {
                trans.Find("PlayerData/stars").GetChild(j).gameObject.SetActive(true);
            }
            for (int i = 0; i < 6- nobelInfo.rongYuRankInfoList[index].xingxing; i++)
            {
                trans.Find("PlayerData/stars").GetChild(6-i-1).gameObject.SetActive(false);
            }
        }
        else
        {
            for (int k = 0; k < trans.Find("PlayerData/stars").childCount; k++)
            {
                trans.Find("PlayerData/stars").GetChild(k).gameObject.SetActive(false);
                //Debug.LogError(nobelInfo.rongYuRankInfoList[index].nickname + index + "nobelInfo.rongYuRankInfoList[index].xingxing" + nobelInfo.rongYuRankInfoList[index].xingxing);
            }
        }
       
        //currentTransList.Add(trans);
        //SetPlayerHeadImg(currentTransList[index],index);
        if (!string.IsNullOrEmpty(nobelInfo.rongYuRankInfoList[index].avatarUrl))
        {
            //headImg = trans.Find("PlayerData/HeadImg").GetComponent<Image>();
            AvatarInfo nAvaInfo = (AvatarInfo)Facade.GetFacade().data.Get(FacadeConfig.AVARTAR_MODULE_ID);
            nAvaInfo.Load((int)nobelInfo.rongYuRankInfoList[index].userId, nobelInfo.rongYuRankInfoList[index].avatarUrl, (int nResult, Texture2D nTexture) => {
                if (nResult == 0 && trans.Find("PlayerData/HeadImg").GetComponent<Image>() != null)
                {
                    if (nTexture.width == 8 && nTexture.height == 8)
                    {
                        return;
                    }
                    nTexture.filterMode = FilterMode.Bilinear;
                    nTexture.Compress(true);
                    trans.Find("PlayerData/HeadImg").GetComponent<Image>().sprite = Sprite.Create(nTexture, new Rect(0, 0, nTexture.width, nTexture.height), new Vector2(0, 0));
                }
            }
            );
        }
        else
        {
            //headImg = trans.Find("PlayerData/HeadImg").GetComponent<Image>();
            trans.Find("PlayerData/HeadImg").GetComponent<Image>().sprite = defaultHead;
        }
       
        //}
	}
    //暂时没用到
    void SetPlayerHeadImg(Transform trans,int index)
    {
        Debug.LogError("设置头像啊"+index);
        if (!string.IsNullOrEmpty(nobelInfo.rongYuRankInfoList[index].avatarUrl))
        {
            headImg = trans.Find("PlayerData/HeadImg").GetComponent<Image>();
            AvatarInfo nAvaInfo = (AvatarInfo)Facade.GetFacade().data.Get(FacadeConfig.AVARTAR_MODULE_ID);
            nAvaInfo.Load((int)nobelInfo.rongYuRankInfoList[index].userId, nobelInfo.rongYuRankInfoList[index].avatarUrl, (int nResult, Texture2D nTexture) => {
                if (nResult == 0 && trans.Find("PlayerData/HeadImg").GetComponent<Image>() != null)
                {
                    if (nTexture.width == 8 && nTexture.height == 8)
                    {
                        return;
                    }
                    nTexture.filterMode = FilterMode.Bilinear;
                    nTexture.Compress(true);
                    trans.Find("PlayerData/HeadImg").GetComponent<Image>().sprite = Sprite.Create(nTexture, new Rect(0, 0, nTexture.width, nTexture.height), new Vector2(0, 0));
                }
            }
            );
        }
        else
        {
            //trans.Find("PlayerData/HeadImg").GetComponent<Image>() = trans.Find("PlayerData/HeadImg").GetComponent<Image>();
            trans.Find("PlayerData/HeadImg").GetComponent<Image>().sprite = defaultHead;
        }
    }


	void CloseWindow ()
	{
		UIColseManage.instance.CloseUI ();
	}

	void OpenPoolExplain ()
	{
		poolExplainWindow.SetActive (true);
	}

	void ClosePoolExplain ()
	{
		poolExplainWindow.SetActive (false);
	}
    //打开说明界面
	void OpenMainExplain ()
	{
		string path = "Window/MainExplain";
		GameObject windowClone = AppControl.OpenWindow (path);
        windowClone.SetActive(true);
		windowClone.transform.Find ("HallOfHonriExplain").gameObject.SetActive (true);
		windowClone.transform.Find ("MatchExplain").gameObject.SetActive (false);
		windowClone.transform.Find ("DuanExplain").gameObject.SetActive (false);
	}
    //领取奖励
    void GetPrize()
    {
        DataControl.GetInstance().PushSocketSnd(FiEventType.SEND_XL_RONGYAOPRIZE_RESQUSET,null);
        getHonorPrizeBtn.enabled = false;
        getHonorPrizeBtn.transform.GetComponent<Image>().sprite = honorPrizeBtnImg[1];
    }

    long PrizeHotPoolNum(long coinNum)
    {
        poolNum += coinNum;
        return poolNum;
    }
    //假的奖池跳动，已取消
    IEnumerator RefreshGoldPoolLocal()
    {
        yield return new WaitForSeconds(0.5f);
        long num = 0;//Random.Range(1, 100);
        hotPool.text = PrizeHotPoolNum(num).ToString();
        //Debug.LogError(PrizeHotPoolNum(num));
        StartCoroutine(RefreshGoldPoolLocal());
    }
    /// <summary>
    /// 自己是否可领荣耀奖励，不可领按钮置灰
    /// </summary>
    /// <param name="bo">If set to <c>true</c> bo.</param>
    void isGetHonorPrize(bool bo)
    {
        if (bo && nobelInfo.honorPrizeState == 0)
        {
            getHonorPrizeBtn.enabled = true;
            getHonorPrizeBtn.transform.GetComponent<Image>().sprite = honorPrizeBtnImg[0];
        }
        else
        {
            getHonorPrizeBtn.enabled = false;
            getHonorPrizeBtn.transform.GetComponent<Image>().sprite = honorPrizeBtnImg[1];
        }
    }

    string SetGold( long nCount)
    {
        if (nCount > 100000000)
        {

            return (float)(nCount / 1000000) / 100 + "亿";

        }
        else
        {
            return nCount.ToString();
        }
    }
}
