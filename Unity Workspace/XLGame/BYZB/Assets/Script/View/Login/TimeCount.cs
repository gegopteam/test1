using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using AssemblyCSharp;
public class TimeCount : MonoBehaviour 
{
    public static TimeCount Instanse;

    float time = 0;
    bool isruning = false;
    MyInfo myInfo;
    public static bool isShow_1 = false;
    public static bool isShow_2 = false;
    public static bool isShow_3 = false;


    public static bool isGiftBag_1 = false;//礼包1面板 是否弹过 默认flase 没有谈过
    public static bool isGiftBag_2 = false;
    public static bool isGiftBag_3 = false;
    void Start () 
    {
        myInfo = (MyInfo)Facade.GetFacade().data.Get(FacadeConfig.USERINFO_MODULE_ID);
        Instanse = this;

	}
	
	// Update is called once per frame
	void Update () 
    {
        if(isruning == true)
        {
            time += Time.deltaTime;
            if (time >= myInfo.Timer)
            {
                time = 0;
                isruning = false;
            }
        }

	}

    public void StartTime()
    {
        time = 0;
        isruning = true;
    }

    public bool EndTime()
    {
        if(isruning)
        {
            return true;
        }
        else
        {
            return false;
        }
    }
}
