using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using AssemblyCSharp;
using UnityEngine.UI;

public class UILevelUpgradeControl : MonoBehaviour, IUiMediator
{
    private Button Close;
    private Button Rule;
    private Button Sign10;
    private Button Sign15;
    private Button Sign20;

    public Image[] HindReward;
    public Image[] HindRewardAlready;
    public Text myNickName;
    public Text GID;
    public Text myCoin;
    public Text myLevel;
    

    public int TaskID;
    public int TaskCurValue;
    public int TaskMaxValue;
    public int RewardState;
    public int ShowInfoMaxValue;

    public static UILevelUpgradeControl instans;
    
    public bool isRoll = false;
    WaitForSeconds waitTime;
    Tweener tween;
    Tweener tweenerInHall;
    Vector3 originPos;
    public Sprite[] ShowNoticeRol;
    public static int[] levelRewardState = {-1, -1, -1};
    int[] checkRewardState = new int[3];
    long gold_temp;
    private MyInfo userInfo;
    UpLevelInfo upLevelinfo;
    //UpLevelReward upLevelreward;

    private List<UpLevelTaskInfos> mLevelInfo = new List<UpLevelTaskInfos>();

    private void Awake()
    {

        if (GameController._instance == null) //渔场和大厅里赋值的摄像机不同
            gameObject.GetComponent<Canvas>().worldCamera = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<Camera>();
        else
            gameObject.GetComponent<Canvas>().worldCamera = ScreenManager.uiCamera;

        userInfo = (MyInfo)Facade.GetFacade().data.Get(FacadeConfig.USERINFO_MODULE_ID);
        upLevelinfo = (UpLevelInfo)Facade.GetFacade().data.Get(FacadeConfig.UI_UPLEVEL);
        //upLevelreward = (UpLevelReward)Facade.GetFacade().data.Get(FacadeConfig.UI_UPLEVEL_REWARD);

        UIColseManage.instance.ShowUI(this.gameObject);
        Close = transform.Find("btclose").GetComponent<Button>();
        Rule = transform.Find("rule").GetComponent<Button>();
        //Sign10 = transform.Find("SevenList/10L/SignReward10").GetComponent<Button>();
        //Sign15 = transform.Find("SevenList/15L/SignReward15").GetComponent<Button>();
        //Sign20 = transform.Find("SevenList/20L/SignReward20").GetComponent<Button>();
        

        Close.onClick.AddListener(BtnloseUI);
        Rule.onClick.AddListener(OpenRuleUI);

        DecideShowButton();
        //Sign10.onClick.AddListener(Reward10);
        //Sign15.onClick.AddListener(Reward15);
        //Sign20.onClick.AddListener(Reward20);
    }
    // Use this for initialization
    IEnumerator Start()
    {
        yield return new WaitForSeconds(0.2f);
        myNickName.text = userInfo.nickname;
        GID.text = userInfo.loginInfo.gameId.ToString();
        
        myCoin.text = userInfo.gold.ToString();
        myLevel.text = userInfo.level.ToString();

        gold_temp = long.Parse(userInfo.gold.ToString());

        Facade.GetFacade().ui.Add(FacadeConfig.UI_UPLEVEL, this);

        if (GameController._instance != null)
        { //如果在渔场，并且是pk场，直接摧毁
            if (GameController._instance.myGameType != GameType.Classical)
            {
                //transform.position = Vector3.up * 1000f;
                Destroy(this.gameObject);
            }
            else
            { //如果是在普通场
                originPos = transform.position;
                SetShowInFishingScene(false);
            }
        }
    }

    public void DecideShowButton() {
        if (-1 == levelRewardState[0])
            return;

        for (int level=0; level< levelRewardState.Length; level++) {
            switch (level) {
                case 0:
                    if (levelRewardState[level] == 1)
                    {
                        HindReward[level].gameObject.SetActive(false);
                        HindRewardAlready[level].gameObject.SetActive(false);
                    }
                    else if (levelRewardState[level] == 3)
                    {
                        HindReward[level].gameObject.SetActive(false);
                        HindRewardAlready[level].gameObject.SetActive(true);
                    }
                    else {
                        HindReward[level].gameObject.SetActive(true);
                        HindRewardAlready[level].gameObject.SetActive(false);
                    }
                    break;
                case 1:
                    if (levelRewardState[level] == 1)
                    {
                        HindReward[level].gameObject.SetActive(false);
                        HindRewardAlready[level].gameObject.SetActive(false);
                    }
                    else if (levelRewardState[level] == 3)
                    {
                        HindReward[level].gameObject.SetActive(false);
                        HindRewardAlready[level].gameObject.SetActive(true);
                    }
                    else
                    {
                        HindReward[level].gameObject.SetActive(true);
                        HindRewardAlready[level].gameObject.SetActive(false);
                    }
                    break;
                case 2:
                    if (levelRewardState[level] == 1)
                    {
                        HindReward[level].gameObject.SetActive(false);
                        HindRewardAlready[level].gameObject.SetActive(false);
                    }
                    else if (levelRewardState[level] == 3)
                    {
                        HindReward[level].gameObject.SetActive(false);
                        HindRewardAlready[level].gameObject.SetActive(true);
                    }
                    else
                    {
                        HindReward[level].gameObject.SetActive(true);
                        HindRewardAlready[level].gameObject.SetActive(false);
                    }
                    break;
            }
        }
    }
    
    // Update is called once per frame
    void Update()
    {

    }

    public void SetLevelUpInfoData(List<UpLevelTaskInfos> nArray)
    {
        mLevelInfo = nArray;
    }

    public void OnRecvData(int nType, object nData)
    {
        Debug.Log("-----UILevelUpgradeControl-----OnRecvData-----" + nType);

        if (nType == 1000) {
            DbGetUpLevelActivityInfos LevelInfoS = (DbGetUpLevelActivityInfos)nData;
            //Debug.Log("-----UILevelUpgradeControl-----OnRecvData-----Count = " + LevelInfoS.levelList.Count);

            if (LevelInfoS.levelList.Count > 0)
            {
                for (int levelActivity = 0; levelActivity < LevelInfoS.levelList.Count; levelActivity++)
                {
                    UpLevelTaskInfos levelInfo = (UpLevelTaskInfos)LevelInfoS.levelList[levelActivity];
                    Debug.Log("-----UILevelUpgradeControl-----OnRecvData-----taskID = " + levelInfo.taskID);
                    Debug.Log("-----UILevelUpgradeControl-----OnRecvData-----rewardState = " + levelInfo.rewardState);
                    Debug.Log("-----UILevelUpgradeControl-----OnRecvData-----taskMaxValue = " + levelInfo.taskMaxValue);
                    switch (levelInfo.taskID)
                    {
                        case 1:
                            if (levelInfo.rewardState == 1 & (userInfo.level >= levelInfo.taskMaxValue))
                            {
                                HindReward[levelInfo.taskID-1].gameObject.SetActive(false);
                                HindRewardAlready[levelInfo.taskID-1].gameObject.SetActive(false);
                                checkRewardState[0] = levelInfo.rewardState;
                            }
                            else if (levelInfo.rewardState == 0)
                            {
                                HindReward[levelInfo.taskID-1].gameObject.SetActive(true);
                                HindRewardAlready[levelInfo.taskID-1].gameObject.SetActive(false);
                                checkRewardState[0] = levelInfo.rewardState;
                            }
                            else if (levelInfo.rewardState == 3)
                            {
                                HindRewardAlready[levelInfo.taskID - 1].gameObject.SetActive(true);
                                checkRewardState[0] = levelInfo.rewardState;
                            }
                            break;

                        case 2:
                            if (levelInfo.rewardState == 1 & (userInfo.level >= levelInfo.taskMaxValue))
                            {
                                HindReward[levelInfo.taskID-1].gameObject.SetActive(false);
                                HindRewardAlready[levelInfo.taskID - 1].gameObject.SetActive(false);
                                checkRewardState[1] = levelInfo.rewardState;
                            }
                            else if (levelInfo.rewardState == 0)
                            {
                                HindReward[levelInfo.taskID - 1].gameObject.SetActive(true);
                                HindRewardAlready[levelInfo.taskID - 1].gameObject.SetActive(false);
                                checkRewardState[1] = levelInfo.rewardState;
                            }
                            else if (levelInfo.rewardState == 3)
                            {
                                HindRewardAlready[levelInfo.taskID - 1].gameObject.SetActive(true);
                                checkRewardState[1] = levelInfo.rewardState;
                            }
                            break;

                        case 3:
                            if (levelInfo.rewardState == 1 & (userInfo.level >= levelInfo.taskMaxValue))
                            {
                                HindReward[levelInfo.taskID - 1].gameObject.SetActive(false);
                                HindRewardAlready[levelInfo.taskID - 1].gameObject.SetActive(false);
                                checkRewardState[2] = levelInfo.rewardState;
                            }
                            else if (levelInfo.rewardState == 0)
                            {
                                HindReward[levelInfo.taskID - 1].gameObject.SetActive(true);
                                HindRewardAlready[levelInfo.taskID - 1].gameObject.SetActive(false);
                                checkRewardState[2] = levelInfo.rewardState;
                            }
                            else if (levelInfo.rewardState == 3)
                            {
                                HindRewardAlready[levelInfo.taskID - 1].gameObject.SetActive(true);
                                checkRewardState[2] = levelInfo.rewardState;
                            }
                            break;

                    }
                    Debug.Log("------------------------------");
                }

                if ( (checkRewardState[0] == 3) && (checkRewardState[1] == 3) && (checkRewardState[2] == 3) )
                {
                    userInfo.isUserGetAllUpLevel = true;
                    UIHallCore.isNeedToUpdate = true;
                    Debug.Log("UIHallCore.isNeedToUpdate = "+ UIHallCore.isNeedToUpdate);
                }
                else
                    userInfo.isUserGetAllUpLevel = false;
            }
            else
            {
                userInfo.isUserGetAllUpLevel = true;
            }
            Debug.Log("-----OnRecvData @ UILevelUpgradeControl-----isUserGetAllUpLevel = "+ userInfo.isUserGetAllUpLevel);
        }
        else if (nType ==1001)
        {
            Debug.Log("~~~~~OnRecvData @ UILevelUpgradeControl");
            UpLevelRewardGets nResponse = (UpLevelRewardGets)nData;
            Debug.Log("~~~~~OnRecvData @ UILevelUpgradeControl~~~~~nResponse.result = "+ nResponse.result);
            if (nResponse.result == 0)
            {
                UIUserDetail.instace.UpdateGold(nResponse.gold);
                ShowReward(nResponse.taskLevel, nResponse.gold);
            }
            else if (nResponse.result == -1)
            {
                GameObject Window1 = UnityEngine.Resources.Load("Window/WindowTipsThree") as UnityEngine.GameObject;
                GameObject WindowClone1 = UnityEngine.GameObject.Instantiate(Window1);
                UITipAutoNoMask ClickTips1 = WindowClone1.GetComponent<UITipAutoNoMask>();
                ClickTips1.tipText.text = "領取失敗";
            }
            else if (nResponse.result == -2)
            {
                GameObject Window1 = UnityEngine.Resources.Load("Window/WindowTipsThree") as UnityEngine.GameObject;
                GameObject WindowClone1 = UnityEngine.GameObject.Instantiate(Window1);
                UITipAutoNoMask ClickTips1 = WindowClone1.GetComponent<UITipAutoNoMask>();
                ClickTips1.tipText.text = "已領取過";
            }
            else if (nResponse.result == -3)
            {
                GameObject Window1 = UnityEngine.Resources.Load("Window/WindowTipsThree") as UnityEngine.GameObject;
                GameObject WindowClone1 = UnityEngine.GameObject.Instantiate(Window1);
                UITipAutoNoMask ClickTips1 = WindowClone1.GetComponent<UITipAutoNoMask>();
                ClickTips1.tipText.text = "未達到條件";
            }
            else if (nResponse.result == -4)
            {
                GameObject Window1 = UnityEngine.Resources.Load("Window/WindowTipsThree") as UnityEngine.GameObject;
                GameObject WindowClone1 = UnityEngine.GameObject.Instantiate(Window1);
                UITipAutoNoMask ClickTips1 = WindowClone1.GetComponent<UITipAutoNoMask>();
                ClickTips1.tipText.text = "領取異常請聯繫客服";
            }
        }      
    }

    public void OnInit()
    {

    }

    public void OnRelease()
    {

    }

    void OnDestroy()
    {
        Facade.GetFacade().ui.Remove(FacadeConfig.UI_UPLEVEL);
        //Facade.GetFacade().ui.Remove(FacadeConfig.UI_UPLEVEL_REWARD);
        MyInfo nUserInfo = (MyInfo)Facade.GetFacade().data.Get(FacadeConfig.USERINFO_MODULE_ID);
    }

    public void OnRecvData(object nData)
    {
        //这应该做三种接受类型判断
        //		ShowReward()
    }

    public void ShowReward(int level, long gold)
    {
        upLevelinfo = (UpLevelInfo)Facade.GetFacade().data.Get(FacadeConfig.UI_UPLEVEL);
        MyInfo myInfo = (MyInfo)Facade.GetFacade().data.Get(FacadeConfig.USERINFO_MODULE_ID);
        
        myInfo.gold = gold_temp + gold;
        myCoin.text = myInfo.gold.ToString();
        gold_temp = myInfo.gold;

        UnityEngine.GameObject Window = UnityEngine.Resources.Load("Window/LevelUpgradeGet") as UnityEngine.GameObject;
        GameObject rewardInstance = UnityEngine.GameObject.Instantiate(Window);
        UILevelupReward reward = rewardInstance.GetComponent<UILevelupReward>();
        reward.SetRewardData(level, gold);

        UIHallCore.isNeedToUpdate = true;
        //levelRewardState[level - 1] = 3;
        //checkRewardState[level - 1] = 3;
    }
    


    /// <summary>
    /// 用戶領取升級獎勵
    /// </summary>
    /// <param name="taskID">領取獎勵編號</param>
    public void Reward(int taskID)
    {
        if (userInfo.isGuestLogin)
        {
            string path = "Window/BindPhoneNumber";
            GameObject jumpObj = AppControl.OpenWindow(path);
            jumpObj.SetActive(true);
            return;
        }

        //發送用戶領取獎勵
        Facade.GetFacade().message.upLevel.SendNewUpLevelMessage(taskID);

        switch (taskID) {
            case 1:
                HindRewardAlready[taskID - 1].gameObject.SetActive(true);
                break;
            case 2:
                HindRewardAlready[taskID - 1].gameObject.SetActive(true);
                break;
            case 3:
                HindRewardAlready[taskID - 1].gameObject.SetActive(true);
                userInfo.isUserGetAllUpLevel = true;
                break;
        }

        //UIUserDetail.instace.UpdateGold();
    }

    

    void SetShowInFishingScene(bool toShow)
    {
        if (GameController._instance == null)
            return;
        if (toShow)
        {
            transform.position = originPos;
        }
        else
        {
            transform.position = Vector3.up * 1000f;
        }
    }

    //public void RefeshHindReward(int curreLevel)
    //{
    //    Debug.Log("目前等級 = "+ curreLevel);
    //    //當前等級20級以上
    //    if (curreLevel >= 20)
    //    {
    //        HindReward20.gameObject.SetActive(false);
    //    }
    //    else
    //    {
    //        HindReward20.gameObject.SetActive(true);
    //    }

    //    //當前等級15級以上未滿20
    //    if (curreLevel >= 15 && curreLevel < 20)
    //    {
    //        HindReward15.gameObject.SetActive(false);
    //    }
    //    else
    //    {
    //        HindReward15.gameObject.SetActive(true);
    //    }

    //    //當前等級10級以上未滿15
    //    if (curreLevel >= 10 && curreLevel < 15)
    //    {
    //        HindReward10.gameObject.SetActive(false);
    //    }
    //    else
    //    {
    //        HindReward10.gameObject.SetActive(true);
    //    }
    //}

    void OpenRuleUI()
    {

        Debug.LogError("OpenRule");
        string path = "Window/LevelUpgradeRule";
        GameObject jumpObj = AppControl.OpenWindow(path);
        jumpObj.SetActive(true);
    }

    void BtnloseUI()
    {

        Debug.LogError("Btnlose");
        UIColseManage.instance.CloseUI();

        //if (UIHallCore.Instance != null)
        //{
        //    UIHallCore.Instance.ToActivity();
        //}
        
        MyInfo myInfo = (MyInfo)Facade.GetFacade().data.Get(FacadeConfig.USERINFO_MODULE_ID);
        
    }
}
