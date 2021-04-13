using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using AssemblyCSharp;

public class MyDuan : MonoBehaviour
{

	/// <summary>
	/// 礼盒打开和关闭
	/// </summary>
	public Sprite[] giftBox;
	/// <summary>
	/// 段位图标
	/// </summary>
	public Sprite[] duanSpr;
	/// <summary>
	/// 加速开启和关闭
	/// </summary>
	public Sprite[] speedUp;

	List<FiRewardStructure> rewardinfo;
	NobelInfo nobelInfo;
	InfinityGridListMyDuan infinityGridListMyDuan;

	//奖励的长度
	public int amount;
	public Font[] duanFonts;

	Button closeWindowBtn;
	Text lastTimeRank;
	Text historyRank;



    GameObject lastTimeNoRankObj;
    GameObject historyNoRankObj;

    GameObject lastTimeNoRankNewObj;
    GameObject historyNoRankNewObj;

    GameObject lastTimeRankNewObj;
    GameObject historyRankNewObj;

    MyInfo myInfo;

    Image head;
    Button longCardBtn;
    Button bossBtn;
    Button moneyBtn;
    GameObject longCardObj;
    GameObject bossBtnObj;
    GameObject moneyBtnObj;
    Transform currentGetBox;
    //等级转换
    int ChangeRank(int rank)
    {
        if (rank >= 5 && rank < 10)
        {
            return 0;
        }else if (rank >= 10 && rank < 15)
        {
            return 1; 
        } else if(rank>=15)
        {
            return rank - 15 + 2;
        }else
        {
            return -1;//没有段位
        }
    }
	void Start ()
	{

        myInfo = (MyInfo)Facade.GetFacade().data.Get(FacadeConfig.USERINFO_MODULE_ID);
        transform.Find("HeadImg/nikeName").GetComponent<Text>().text = Tool.GetName(myInfo.nickname,5);
        transform.Find("HeadImg/MyId").GetComponent<Text>().text = "ID: "+myInfo.loginInfo.gameId;
        transform.Find("HeadImg/Head").GetComponent<Image>();
        head = transform.Find("HeadImg/Head").GetComponent<Image>();
        longCardBtn = transform.Find("longCard").GetComponent<Button>();
        bossBtn = transform.Find("Boss").GetComponent<Button>();
        moneyBtn = transform.Find("Money").GetComponent<Button>();
        historyRank = transform.Find("HistoryHighest/HistoryTxt").GetComponent<Text>();
        lastTimeRank = transform.Find("LastTime/LatsTimeTxt").GetComponent<Text>();
        lastTimeNoRankObj = transform.Find("LastTime/NoRank").gameObject;
        historyNoRankObj = transform.Find("HistoryHighest/NoRank").gameObject;
        lastTimeNoRankNewObj = transform.Find("LastTimeNo").gameObject;
        historyNoRankNewObj = transform.Find("HistoryHighestNo").gameObject;

        lastTimeRankNewObj = transform.Find("LastTime").gameObject;
        historyRankNewObj = transform.Find("HistoryHighest").gameObject;

        longCardBtn.onClick.RemoveAllListeners();
        bossBtn.onClick.RemoveAllListeners();
        moneyBtn.onClick.RemoveAllListeners();

        longCardBtn.onClick.AddListener(delegate (){
            OpenTipsBubble(1);
        });
        moneyBtn.onClick.AddListener(delegate () {
            OpenTipsBubble(2);
        });
        bossBtn.onClick.AddListener(delegate () {
            OpenTipsBubble(3);
        });
        longCardObj= transform.Find("longCard/longCardBubble").gameObject;
        moneyBtnObj= transform.Find("Money/moneyBubble").gameObject;
        bossBtnObj = transform.Find("Boss/bossBubble").gameObject;
        longCardObj.GetComponent<Button>().onClick.RemoveAllListeners();
        moneyBtnObj.GetComponent<Button>().onClick.RemoveAllListeners();
        bossBtnObj.GetComponent<Button>().onClick.RemoveAllListeners();
        longCardObj.GetComponent<Button>().onClick.AddListener(delegate() {
            OpenTipsBubble(4);
        });
        moneyBtnObj.GetComponent<Button>().onClick.AddListener(delegate () {
            OpenTipsBubble(4);
        });
        bossBtnObj.GetComponent<Button>().onClick.AddListener(delegate () {
            OpenTipsBubble(4);
        });

        //添加头像
        if (!string.IsNullOrEmpty(myInfo.avatar))
        {
            AvatarInfo nAvaInfo = (AvatarInfo)Facade.GetFacade().data.Get(FacadeConfig.AVARTAR_MODULE_ID);
            nAvaInfo.Load(myInfo.userID, myInfo.avatar, (int nResult, Texture2D nTexture) => {
                if (nResult == 0 && head != null)
                {
                    if (nTexture.width == 8 && nTexture.height == 8)
                    {
                        return;
                    }
                    nTexture.filterMode = FilterMode.Bilinear;
                    nTexture.Compress(true);
                    head.sprite = Sprite.Create(nTexture, new Rect(0, 0, nTexture.width, nTexture.height), new Vector2(0, 0));
                }
            }
            );
        }

        if (GameController._instance == null) //渔场和大厅里赋值的摄像机不同
            gameObject.GetComponent<Canvas>().worldCamera = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<Camera>();
        else
            gameObject.GetComponent<Canvas>().worldCamera = ScreenManager.uiCamera;
        UIColseManage.instance.ShowUI(this.gameObject);

		closeWindowBtn = transform.Find ("CloseBtn").GetComponent<Button> ();
		closeWindowBtn.onClick.RemoveAllListeners ();
		closeWindowBtn.onClick.AddListener (CloseWindow);
		//初始化数据列表;
		rewardinfo = Facade.GetFacade ().message.reward.GetHoroduanweiArray ();
        //amount = rewardinfo.Count;
        amount = 25;//改回去
        nobelInfo = (NobelInfo)Facade.GetFacade().data.Get(FacadeConfig.UI_NOBEL_ID);
		infinityGridListMyDuan = transform.Find ("Panel_Scroll/Panel_Grid").GetComponent<InfinityGridListMyDuan> ();
		infinityGridListMyDuan.SetAmount (amount);
		infinityGridListMyDuan.updateChildrenCallMyDuan = UpdateChildMyDuan;
        //Debug.LogError("我的最高段位是"+nobelInfo.Lishizuigao);
        //Debug.LogError("我的上期段位是"+nobelInfo.Shangqipaiming);
        //Debug.LogError("我的当前段位是" + nobelInfo.CurrentDuanwei);
        //Debug.LogError("我的本期最高段位是"+nobelInfo.Beiqizuigao);
        if (nobelInfo.Lishizuigao==0)
        {
            //historyRank.gameObject.SetActive(false);
            // historyNoRankObj.SetActive(true);
            historyNoRankNewObj.SetActive(true);
            historyRankNewObj.SetActive(false);
        }
        else
        {
            historyNoRankNewObj.SetActive(false);
            historyRankNewObj.SetActive(true);
            historyRank.text = nobelInfo.Lishizuigao + "D";
        }
        if (nobelInfo.Shangqipaiming==0)
        {
            //lastTimeRank.gameObject.SetActive(false);
            //lastTimeNoRankObj.SetActive(true);
            lastTimeNoRankNewObj.SetActive(true);
            lastTimeRankNewObj.SetActive(false);
        }
        else
        {
            lastTimeNoRankNewObj.SetActive(false);
            lastTimeRankNewObj.SetActive(true);
            lastTimeRank.text = nobelInfo.Shangqipaiming + "D";
        }

		
		if (nobelInfo.IsToUp != 0) {
            transform.Find ("Money/openBtn").GetComponent<Image> ().sprite = speedUp [1];
		}
		if (nobelInfo.IsMonthType !=0 ) {
            transform.Find ("longCard/openBtn").GetComponent<Image> ().sprite = speedUp [1];
		}
		if (nobelInfo.ISbossmatchdouble != 0) {
            transform.Find ("Boss/openBtn").GetComponent<Image> ().sprite = speedUp [1];
		}
	}
    /// <summary>
    /// 滑动时替换UI
    /// </summary>
    /// <param name="index">Index.</param>
    /// <param name="trans">Trans.</param>
	void UpdateChildMyDuan (int index, Transform trans)
	{

        if (index+1 >= 0 && index+1 <= 4)
        {
            SetDuanIcon(trans,index);
        }
        else if (index+1 == 5)
        {
            SetDuanIcon(trans,index,0);
        }
        else if (index+1 > 5 && index+1 <= 9)
        {

            SetDuanIcon(trans,index);
            //trans.Find("DuamData/image").GetComponent<Image>().sprite = duanSpr[1];
            //trans.Find("DuamData/image").GetComponent<Image>().SetNativeSize();
            ////trans.Find("DuamData/DuanFrame").GetComponent<Image>().sprite = duanSpr[3];
            //trans.Find("DuamData/DuanFrame").GetComponent<Image>().SetNativeSize();
            //trans.Find("DuamData/coinImg").gameObject.SetActive(false);
            //trans.Find("DuamData/coinTxt").gameObject.SetActive(false);
            //trans.Find("DuamData/diamond").gameObject.SetActive(false);
            //trans.Find("DuamData/Text").gameObject.SetActive(false);
            //trans.Find("DuamData/image/Text").GetComponent<Text>().font = duanFonts[1];
            //trans.Find("DuamData/image/Text").GetComponent<Text>().text = index + 1 + "D";
            //trans.Find("DuamData/box").gameObject.SetActive(false);
        }
        else if (index+1 == 10)
        {
            SetDuanIcon(trans,index,1);
        }
        else if (index+1 > 10 && index+1 < 15)
        {
            SetDuanIcon(trans,index);
        }
        else if (index+1 >= 15) 
        {
            SetDuanIcon(trans,index,index-12);
        }


        if (nobelInfo.CurrentDuanwei==index+1)
        {
            trans.Find("DuamData/DuanFrame").gameObject.SetActive(true);
        }
        else
        {
            trans.Find("DuamData/DuanFrame").gameObject.SetActive(false);
        }


        //反复改了几次的结果
        //13个
        //trans.Find("DuamData/Text").GetComponent<Text>().text = rewardinfo[index].rewardPro[1].value.ToString();
        //trans.Find("DuamData/coinTxt").GetComponent<Text>().text = rewardinfo[index].rewardPro[0].value.ToString();
        //trans.Find("DuamData/image").GetComponent<Image>().sprite = duanSpr[0];
        //trans.Find("DuamData/image").GetComponent<Image>().SetNativeSize();
        //trans.Find("DuamData/DuanFrame").GetComponent<Image>().sprite = duanSpr[2];
        //trans.Find("DuamData/DuanFrame").GetComponent<Image>().SetNativeSize();
        //trans.Find("DuamData/image/Text").GetComponent<Text>().font = duanFonts[0];
        //if (index >= 2)
        //{
        //    //trans.Find("DuamData/image").GetComponent<Image>().sprite = duanSpr[0];
        //    //trans.Find("DuamData/image").GetComponent<Image>().SetNativeSize();
        //    //trans.Find("DuamData/DuanFrame").GetComponent<Image>().sprite = duanSpr[2];
        //    //trans.Find("DuamData/DuanFrame").GetComponent<Image>().SetNativeSize();
        //    //trans.Find("DuamData/image/Text").GetComponent<Text>().font = duanFonts[0];
        //    trans.Find("DuamData/image/Text").GetComponent<Text>().text = index + 13 + "D";
        //}
        //else if (index == 0)
        //{
        //    //trans.Find("DuamData/image").GetComponent<Image>().sprite = duanSpr[1];
        //    //trans.Find("DuamData/image").GetComponent<Image>().SetNativeSize();
        //    //trans.Find("DuamData/DuanFrame").GetComponent<Image>().sprite = duanSpr[3];
        //    //trans.Find("DuamData/DuanFrame").GetComponent<Image>().SetNativeSize();
        //    //trans.Find("DuamData/image/Text").GetComponent<Text>().font = duanFonts[1];
        //    trans.Find("DuamData/image/Text").GetComponent<Text>().text = index + 5 + "D";
        //}
        //else if (index == 1)
        //{
        //    //trans.Find("DuamData/image").GetComponent<Image>().sprite = duanSpr[1];
        //    //trans.Find("DuamData/image").GetComponent<Image>().SetNativeSize();
        //    //trans.Find("DuamData/DuanFrame").GetComponent<Image>().sprite = duanSpr[3];
        //    //trans.Find("DuamData/DuanFrame").GetComponent<Image>().SetNativeSize();
        //    //trans.Find("DuamData/image/Text").GetComponent<Text>().font = duanFonts[1];
        //    trans.Find("DuamData/image/Text").GetComponent<Text>().text = index + 9 + "D";
        //}
        //13个
        //if (ChangeRank(nobelInfo.CurrentDuanwei) != -1)
        //{
        //    if (ChangeRank(nobelInfo.CurrentDuanwei) == index)
        //    {
        //        trans.Find("DuamData/DuanFrame").gameObject.SetActive(true);
        //    }
        //    else
        //    {
        //        trans.Find("DuamData/DuanFrame").gameObject.SetActive(false);
        //    }
        //}
        //if (ChangeRank(nobelInfo.Beiqizuigao) >= index && nobelInfo.prizeState[index] == 0)
        //{
        //    GetDuanPrizeBtnAddListener(index, trans, true);
        //    trans.Find("DuamData/box").GetComponent<Image>().sprite = giftBox[0];
        //    trans.Find("DuamData/box/effect_lihelizihuanrao").gameObject.SetActive(false);
        //    trans.Find("DuamData/box/_Effect_RewardWindow").gameObject.SetActive(false);
        //}
        //else if (ChangeRank(nobelInfo.Beiqizuigao) >= index && nobelInfo.prizeState[index] == 3)
        //{
        //    GetDuanPrizeBtnAddListener(index, trans, false);
        //    trans.Find("DuamData/box").GetComponent<Image>().sprite = giftBox[1];
        //    trans.Find("DuamData/box/effect_lihelizihuanrao").gameObject.SetActive(false);
        //    trans.Find("DuamData/box/_Effect_RewardWindow").gameObject.SetActive(false);
        //}  
        //else
        //{
        //    GetDuanPrizeBtnAddListener(index, trans, false);
        //    trans.Find("DuamData/box").GetComponent<Image>().sprite = giftBox[0];
        //    trans.Find("DuamData/box/effect_lihelizihuanrao").gameObject.SetActive(false);
        //    trans.Find("DuamData/box/_Effect_RewardWindow").gameObject.SetActive(false);
        //}   

    }

	void CloseWindow ()
	{
		UIColseManage.instance.CloseUI (); 
	}

    void OpenTipsBubble(int index)
    {
        switch (index)
        {
            case 1:
                longCardObj.SetActive(true);
                bossBtnObj.SetActive(false);
                moneyBtnObj.SetActive(false);
                break;
            case 2:
                longCardObj.SetActive(false);
                bossBtnObj.SetActive(false);
                moneyBtnObj.SetActive(true);
                break;
            case 3:
                longCardObj.SetActive(false);
                bossBtnObj.SetActive(true);
                moneyBtnObj.SetActive(false);
                break;
            case 4:
                longCardObj.SetActive(false);
                bossBtnObj.SetActive(false);
                moneyBtnObj.SetActive(false);
                break;
            default:
                break;
        }
    }
    /// <summary>
    /// 领取奖励的按钮绑定方法
    /// </summary>
    /// <param name="index">Index.</param>
    /// <param name="nTrans">N trans.</param>
    /// <param name="canGet">If set to <c>true</c> can get.</param>
    void GetDuanPrizeBtnAddListener(int index,Transform nTrans,bool canGet)
    {
        if (canGet)
        {
            nTrans.Find("DuamData/box").GetComponent<Button>().onClick.RemoveAllListeners();
            nTrans.Find("DuamData/box").GetComponent<Button>().onClick.AddListener(delegate () {
                GetDuanPrize(index,nTrans);
            });
          
        }
        else
        {
            nTrans.Find("DuamData/box").GetComponent<Button>().onClick.RemoveAllListeners();
        }

    }
    /// <summary>
    /// 发送领取奖励的消息
    /// </summary>
    /// <param name="index">Index.</param>
    /// <param name="trans">Trans.</param>
    void GetDuanPrize(int index,Transform trans)
    {
        Debug.LogError("index"+index);
        Facade.GetFacade().message.nobel.SendGetPaiWeiPrizeState(index+1);
        trans.Find("DuamData/box").GetComponent<Image>().sprite = giftBox[1];
        nobelInfo.prizeState[index] = 3;
        //trans.Find("DuamData/box/effect_lihelizihuanrao").gameObject.SetActive(false);
        //trans.Find("DuamData/box/_Effect_RewardWindow").gameObject.SetActive(false);
        trans.Find("DuamData/box").GetComponent<Button>().onClick.RemoveAllListeners();
    }


    /// <summary>
    /// 设置段位和图标信息,有奖品,废弃的代码,需求更改,现在又用这个了
    /// </summary>
    /// <param name="ntrans">Ntrans.</param>
    /// <param name="nIndex">当前图标的索引.</param>
    /// <param name="i">奖品的索引.</param>
    void SetDuanIcon(Transform ntrans,int nIndex,int i)
    {
        ntrans.Find("DuamData/coinImg").gameObject.SetActive(true);
        ntrans.Find("DuamData/coinTxt").gameObject.SetActive(true);
        ntrans.Find("DuamData/diamond").gameObject.SetActive(true);
        ntrans.Find("DuamData/Text").gameObject.SetActive(true);
        if (nIndex<14)
        {
            ntrans.Find("DuamData/image").GetComponent<Image>().sprite = duanSpr[1];
            ntrans.Find("DuamData/image").GetComponent<Image>().SetNativeSize();
            //ntrans.Find("DuamData/DuanFrame").GetComponent<Image>().sprite = duanSpr[3];
            //ntrans.Find("DuamData/DuanFrame").GetComponent<Image>().SetNativeSize();
            ntrans.Find("DuamData/image/Text").GetComponent<Text>().font = duanFonts[1];
        }
        else
        {
            ntrans.Find("DuamData/image").GetComponent<Image>().sprite = duanSpr[0];
            ntrans.Find("DuamData/image").GetComponent<Image>().SetNativeSize();
            //ntrans.Find("DuamData/DuanFrame").GetComponent<Image>().sprite = duanSpr[2];
            //ntrans.Find("DuamData/DuanFrame").GetComponent<Image>().SetNativeSize();
            ntrans.Find("DuamData/image/Text").GetComponent<Text>().font = duanFonts[0];
        }
        ntrans.Find("DuamData/image/Text").GetComponent<Text>().text = nIndex + 1 + "D";
        ntrans.Find("DuamData/box").gameObject.SetActive(true);

   
        ntrans.Find("DuamData/Text").GetComponent<Text>().text = rewardinfo[i].rewardPro[1].value.ToString();
        ntrans.Find("DuamData/coinTxt").GetComponent<Text>().text = rewardinfo[i].rewardPro[0].value.ToString();

        Debug.LogError("nobelInfo.prizeState"+nobelInfo.prizeState.Count);
        if (nobelInfo.Beiqizuigao > nIndex && nobelInfo.prizeState[i] == 0)
        {
            GetDuanPrizeBtnAddListener(i, ntrans, true);
            ntrans.Find("DuamData/box").GetComponent<Image>().sprite = giftBox[0];
            ntrans.Find("DuamData/box/lock").gameObject.SetActive(false);
            //ntrans.Find("DuamData/box/effect_lihelizihuanrao").gameObject.SetActive(true);
            //ntrans.Find("DuamData/box/_Effect_RewardWindow").gameObject.SetActive(true);

        }
        else if (nobelInfo.Beiqizuigao > nIndex && nobelInfo.prizeState[i] == 3)
        {
            GetDuanPrizeBtnAddListener(i, ntrans, false);
            ntrans.Find("DuamData/box").GetComponent<Image>().sprite = giftBox[1];
            ntrans.Find("DuamData/box/lock").gameObject.SetActive(false);
            //ntrans.Find("DuamData/box/effect_lihelizihuanrao").gameObject.SetActive(false);
            //ntrans.Find("DuamData/box/_Effect_ RewardWindow").gameObject.SetActive(false);
        }
        else
        {
            GetDuanPrizeBtnAddListener(i, ntrans, false);
            ntrans.Find("DuamData/box").GetComponent<Image>().sprite = giftBox[0];
            ntrans.Find("DuamData/box/lock").gameObject.SetActive(true);
            //ntrans.Find("DuamData/box/effect_lihelizihuanrao").gameObject.SetActive(false);
            //ntrans.Find("DuamData/box/_Effect_RewardWindow").gameObject.SetActive(false);
        }

    }
    /// <summary>
    /// 设置段位和图标信息,无奖品,又说不要了???,老板又说要这个
    /// </summary>
    /// <param name="ntrans">Ntrans.</param>
    /// <param name="nIndex">N index.</param>
    void SetDuanIcon(Transform ntrans, int nIndex)
    {


        ntrans.Find("DuamData/coinImg").gameObject.SetActive(false);
        ntrans.Find("DuamData/coinTxt").gameObject.SetActive(false);
        ntrans.Find("DuamData/diamond").gameObject.SetActive(false);
        ntrans.Find("DuamData/Text").gameObject.SetActive(false);
        ntrans.Find("DuamData/box").GetComponent<Image>().sprite = giftBox[2];
        if (nIndex < 14)
        {
            ntrans.Find("DuamData/image").GetComponent<Image>().sprite = duanSpr[1];
            ntrans.Find("DuamData/image").GetComponent<Image>().SetNativeSize();
            //ntrans.Find("DuamData/DuanFrame").GetComponent<Image>().sprite = duanSpr[3];
            //ntrans.Find("DuamData/DuanFrame").GetComponent<Image>().SetNativeSize();
            ntrans.Find("DuamData/image/Text").GetComponent<Text>().font = duanFonts[1];
        }
        else
        {
            ntrans.Find("DuamData/image").GetComponent<Image>().sprite = duanSpr[0];
            ntrans.Find("DuamData/image").GetComponent<Image>().SetNativeSize();
            //ntrans.Find("DuamData/DuanFrame").GetComponent<Image>().sprite = duanSpr[2];
            //ntrans.Find("DuamData/DuanFrame").GetComponent<Image>().SetNativeSize();
            ntrans.Find("DuamData/image/Text").GetComponent<Text>().font = duanFonts[0];
        }
        ntrans.Find("DuamData/image/Text").GetComponent<Text>().text = nIndex + 1 + "D";
  
        if (nobelInfo.Beiqizuigao > nIndex)
        {
            ntrans.Find("DuamData/box/lock").gameObject.SetActive(false);
            //ntrans.Find("DuamData/box/effect_lihelizihuanrao").gameObject.SetActive(true);
            //ntrans.Find("DuamData/box/_Effect_RewardWindow").gameObject.SetActive(true);
        }
        else if (nobelInfo.Beiqizuigao <= nIndex)
        {
            ntrans.Find("DuamData/box/lock").gameObject.SetActive(true);
        }
        else
        {
            ntrans.Find("DuamData/box/lock").gameObject.SetActive(false);
        }

        //ntrans.Find("DuamData/box").gameObject.SetActive(false);
    }
 
}
