using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public delegate void LaterClickEvent();



public  class GetColdManager : MonoBehaviour 
{
    public event LaterClickEvent laterClickEvent;
    
    /// <summary>
    /// 是否是游客登录
    /// </summary>
    private void isGuestLogin()
    {
        MyInfo myInfo = DataControl.GetInstance().GetMyInfo();
        //myInfo.isGuestLogin = true;
        //myInfo.isLogining = true;
        //myInfo.platformType = 10;
        if (myInfo.isGuestLogin==true||myInfo.platformType==10)
        {
            gameObject.SetActive(false);
        }

    }
	void Start()
	{
		UIColseManage.instance.ShowUI (this.gameObject);
	}
    /// <summary>
    /// 打开金币限制窗口
    /// </summary>
    public static void OnOpenGetColdClick()
    {
        UnityEngine.GameObject Window = UnityEngine.Resources.Load("Window/GetGold") as UnityEngine.GameObject;
        GameObject WindowClone = UnityEngine.GameObject.Instantiate(Window);

    }
    public void OnColseButtonClick()
    {
        Debug.Log("OnColseButtonClick");
		UIColseManage.instance.CloseUI ();
    }

    /// <summary>
    /// 以后再说领取
    /// </summary>
    public void OnLaterClick()
    {
        Debug.Log("OnLaterClick----1234455566777--------------");
        Destroy(gameObject);
        //UISign.instance.Later();
		UIColseManage.instance.CloseUI ();
        //StartCoroutine(Close());
    }


}
