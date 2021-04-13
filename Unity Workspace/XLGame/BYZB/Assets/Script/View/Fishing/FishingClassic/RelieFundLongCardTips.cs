using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RelieFundLongCardTips : MonoBehaviour {

    public GameObject backConfirmPanel;

    private void Start()
    {
        if (GameController._instance != null)
        {
            if (this.GetComponent<Canvas>().worldCamera == null)
            {
                this.GetComponent<Canvas>().worldCamera = GameObject.FindWithTag(TagManager.uiCamera).GetComponent<Camera>();
            }
        }
        else
        {
            if (this.GetComponent<Canvas>().worldCamera == null)
            {
                this.GetComponent<Canvas>().worldCamera = GameObject.FindWithTag("MainCamera").GetComponent<Camera>();
            }
        }
    }
    public void BuyLongCrad()
    {
        if (AppInfo.isInHall)
        {
            //
            UIHallCore.Instance.MouthCard();
            Destroy(this.gameObject);
        }
        else
        {
            //返回大厅购买龙卡
            GameObject temp = GameObject.Instantiate(backConfirmPanel);
            temp.GetComponent<Canvas>().enabled = false;
            temp.SetActive(false);
            temp.GetComponent<AskBackPanel>().Show("");
            UIColseManage.instance.isFishingBuyLongCard = true;
            temp.GetComponent<AskBackPanel>().Btn_Confirm();  
        }
      
    }

    public void NoBuyLongCard()
    {
        Destroy(this.gameObject);
    }
}
