using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using AssemblyCSharp;
using DG.Tweening;
public class MainExplain : MonoBehaviour {

    Button closeWindowBtn;
    Button openMatchWindowBtn;
    Button openDuanWindowBtn;
    Button openHallOfHonorWindowBtn;
    Button opeHonor;

    GameObject matchWindowObj;
    GameObject duanWindowObj;
    GameObject hallOfHonorObj;

    RankInfo info;

    Text goldCoinText;
    Text dimaioText;
    Text torpTxt;

    private List<FiRewardStructure> rewardinfo;
	void Start () {

        if (GameController._instance == null) //渔场和大厅里赋值的摄像机不同
            gameObject.GetComponent<Canvas>().worldCamera = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<Camera>();
        else
            gameObject.GetComponent<Canvas>().worldCamera = ScreenManager.uiCamera;

        UIColseManage.instance.ShowUI(this.gameObject);

        matchWindowObj = transform.Find("MatchExplain").gameObject;
        duanWindowObj = transform.Find("DuanExplain").gameObject;
        hallOfHonorObj = transform.Find("HallOfHonriExplain").gameObject;

        closeWindowBtn = transform.Find("CloseBtn").GetComponent<Button>();
        openMatchWindowBtn = transform.Find("MatchBtn").GetComponent<Button>();
        openDuanWindowBtn = transform.Find("DuanBtn").GetComponent<Button>();
        openHallOfHonorWindowBtn = transform.Find("HallOfHonorBtn").GetComponent<Button>();
        opeHonor = transform.Find("HallOfHonriExplain/HonorBtn").GetComponent<Button>();

        closeWindowBtn.onClick.RemoveAllListeners();
        openDuanWindowBtn.onClick.RemoveAllListeners();
        openMatchWindowBtn.onClick.RemoveAllListeners();
        openHallOfHonorWindowBtn.onClick.RemoveAllListeners();
        opeHonor.onClick.RemoveAllListeners();

        closeWindowBtn.onClick.AddListener(CloseWindow);
        openDuanWindowBtn.onClick.AddListener(OpenDuan);
        openMatchWindowBtn.onClick.AddListener(OpenMatch);
        openHallOfHonorWindowBtn.onClick.AddListener(OpenHallOfHonor);
        opeHonor.onClick.AddListener(OpenHonor);

        rewardinfo = Facade.GetFacade().message.reward.GetHoroRewardInfoArray();
        //info = (RankInfo)Facade.GetFacade().data.Get(FacadeConfig.RANK_MODULE_ID);
        Debug.LogError("长度是"+rewardinfo.Count);

        if (rewardinfo.Count== transform.Find("DuanExplain/DuanPrize/GridList").childCount)
        {
            for (int i = 0; i < rewardinfo.Count; i++)
            {
                transform.Find("DuanExplain/DuanPrize/GridList").GetChild(i).Find("CoinTxt").GetComponent<Text>().text = "X" + rewardinfo[rewardinfo.Count-1-i].rewardPro[0].value.ToString();
                transform.Find("DuanExplain/DuanPrize/GridList").GetChild(i).Find("TorpedTxt").GetComponent<Text>().text = "X" + rewardinfo[rewardinfo.Count - 1 - i].rewardPro[2].value.ToString();
                transform.Find("DuanExplain/DuanPrize/GridList").GetChild(i).Find("DiamondTxt").GetComponent<Text>().text = "X" + rewardinfo[rewardinfo.Count - 1 - i].rewardPro[1].value.ToString();
                transform.Find("DuanExplain/DuanPrize/GridList").GetChild(i).Find("DuanImg/Text").GetComponent<Text>().text = rewardinfo.Count - i + "D";
                Debug.LogError(rewardinfo[i].rewardPro[2].type);
                transform.Find("DuanExplain/DuanPrize/GridList").GetChild(i).Find("skillImg").GetComponent<Image>().sprite=FiPropertyType.GetSprite(rewardinfo[rewardinfo.Count - 1 - i].rewardPro[2].type);
            }
        }
        else if (rewardinfo.Count >= transform.Find("DuanExplain/DuanPrize/GridList").childCount)
        {
            int differ = rewardinfo.Count - transform.Find("DuanExplain/DuanPrize/GridList").childCount;
            for (int i = 0; i < differ; i++)
            {
                GameObject gameObject = Instantiate(transform.Find("DuanExplain/DuanPrize/GridList/DuanPrizeData").gameObject,transform.Find("DuanExplain/DuanPrize/GridList"));
            }
        }else if (rewardinfo.Count <= transform.Find("DuanExplain/DuanPrize/GridList").childCount)
        {
            int differ= transform.Find("DuanExplain/DuanPrize/GridList").childCount - rewardinfo.Count;
            for (int i = 0; i < differ; i++)
            {
                Destroy(transform.Find("DuanExplain/DuanPrize/GridList").GetChild(transform.Find("DuanExplain/DuanPrize/GridList").childCount-i));
            }
        }

       
    }
	
    void SetPrizeData()
    {
        
    }

    void CloseWindow()
    {
        UIColseManage.instance.CloseUI (); 
    }
    /// <summary>
    /// 打开荣耀
    /// </summary>
    void OpenHallOfHonor()
    {
        matchWindowObj.SetActive(false);
        duanWindowObj.SetActive(false);
        hallOfHonorObj.SetActive(true);
    }
    /// <summary>
    /// 打开段位
    /// </summary>
    void OpenDuan()
    {
        matchWindowObj.SetActive(false);
        duanWindowObj.SetActive(true);
        hallOfHonorObj.SetActive(false); 
    }
    /// <summary>
    /// 打开比赛说明
    /// </summary>
    void OpenMatch()
    {
        matchWindowObj.SetActive(true);
        duanWindowObj.SetActive(false);
        hallOfHonorObj.SetActive(false);
    }
    /// <summary>
    /// 打开荣誉殿堂
    /// </summary>
    void OpenHonor()
    {
        CloseWindow();
        DataControl.GetInstance().PushSocketSnd(FiEventType.SEND_XL_RONGYURANK_RESQUSET, null);
    }
}
