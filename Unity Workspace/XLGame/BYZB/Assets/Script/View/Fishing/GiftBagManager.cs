using UnityEngine;
using System.Collections;
using AssemblyCSharp;

public class GiftBagManager : MonoBehaviour
{
    MyInfo myInfo;
    public static bool isShowAwesome = true;
    public  GameObject awesome;//一元礼包
    [HideInInspector]
    public GameObject doubleGiftBag;
    public static GiftBagManager Instance =null;
    GetTopUpGiftBagState getTopUpGiftBagState;
    private void Awake()
    {
        Instance = this;
        myInfo = (MyInfo)Facade.GetFacade().data.Get(FacadeConfig.USERINFO_MODULE_ID);
        //doubleGiftBag.gameObject.SetActive(false);
        //awesome.gameObject.SetActive(false);
        getTopUpGiftBagState = new GetTopUpGiftBagState();
    }
    private void Start()
    {
        //IsShowGiftBag();
        //IsShow();
        //Show();
        IsShowGiftBag();
    }


    public void Show()
    {
        if (myInfo.showAwesome==1)
        {
            awesome.gameObject.SetActive(true);
        }
        else
        {
            awesome.gameObject.SetActive(false);
        }

        //if (myInfo.showAwesome == 1)
        //{
        //    doubleGiftBag.gameObject.SetActive(true);
        //}
        //else
        //{
        //    doubleGiftBag.gameObject.SetActive(false);
        //}
    }
    /// <summary>
    /// 是否显示超值礼包
    /// </summary>
    public void IsShowGiftBag()
    {
        if(myInfo.showAwesome==1&& myInfo.isShowNumner==1)//1元礼包界面,打开过,但是没有充值,打开礼包图标
        {
            awesome.SetActive(true);
        }
        else if(myInfo.showAwesome==2)//充值过,隐藏礼包图标
        {
            awesome.SetActive(false); 
        }

        //需求更改不需要再渔场显示图标要在大厅显示;
        ////二选一礼包界面,打开过,但是没有充值,打开礼包图标
        //if (myInfo.showDouble == 1)  
        //{
        //    if (myInfo.isShowNumner == 3 || myInfo.isShowNumner == 4)
        //    {
        //        doubleGiftBag.SetActive(true);
        //    }
        //}
        //else if (myInfo.showDouble == 2)//充值过,隐藏礼包图标
        //{
        //    doubleGiftBag.SetActive(false);
        //}

        //如果服务器下发的状态是1  或者点击进入新手礼包就显示一元礼包
        //if (myInfo.isShowAwesome==true&&myInfo.isShowNumner==1)
        //{
        //    Debug.Log("111223");
        //    awesome.gameObject.SetActive(true);
        //}
        //else
        //{
        //    awesome.gameObject.SetActive(false);
        //}

        //如果等于
        //if (myInfo.isShowDouble==true)
        //{
        //    //并且进入3倍场或者进入boos场就显示
        //    if (myInfo.isShowNumner == 3 || myInfo.isShowNumner == 4)
        //    {
        //        doubleGiftBag.gameObject.SetActive(true);
        //    }
        //}
        //else
        //{
        //    doubleGiftBag.gameObject.SetActive(false);
        //}
    }
    /// <summary>
    /// 打开一元礼包
    /// </summary>
    public void OpenAwesomeWindowClick()
    {
        string path = "Window/Preferential/AwesomeCanvas";
        GameObject WindowClone = AppControl.OpenWindow(path);
        WindowClone.SetActive(true);
    }   

    /// <summary>
    /// 打开双喜
    /// </summary>
    public void OpenDoubleWindowClick()
    {
        string path = "Window/Preferential/Double";
        GameObject WindowClone = AppControl.OpenWindow(path);
        WindowClone.SetActive(true);
    }

    /// <summary>
    /// 显示双喜临门图标
    /// </summary>
    public void ShowDoubleImage()
    {
        Debug.LogError("1111111111=======");
        StartCoroutine(ShowImage());
    }
    IEnumerator ShowImage()
    {
        yield return new WaitForSeconds(1f);
        //doubleGiftBag.gameObject.SetActive(true);
    }

    public void IsShow()
    {
        Debug.Log("是否=====" + myInfo.showAwesome);
        if (myInfo.isNewUser==0)
        {
            if ( myInfo.isShowNumner == 1&&PlayerPrefs.GetString(myInfo.userID+"1111","-1")=="-1")
            {
                Debug.Log("9999999");
                PlayerPrefs.SetString(myInfo.userID + "1111", "1");
                //awesome.gameObject.SetActive(true);
            }
            else
            {
                //awesome.gameObject.SetActive(false);
            }
        }

        if (myInfo.showDouble == 1)
        {
            //if (myInfo.isShowNumner == 3 || myInfo.isShowNumner == 4)
            //{
            //    doubleGiftBag.gameObject.SetActive(true);
            //}
            //else
            //{
            //    doubleGiftBag.gameObject.SetActive(false);
            //}
        }
    }
}
