using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UITipPayFail : MonoBehaviour
{


	private void Start ()
	{
		Debug.Log (UIStore.instance.itemBuyCoin.ToString () + "充值的价格");
	}
	//取消充值
	public void ClosePayFailUI ()
	{

		// UIColseManage.instance.CloseUI();
		Destroy (this.gameObject);
		UIStore.instance.itemBuyCoin = -1;
		UIStore.itemBuyCard = 0;
	}
	//按照之前点击的钱继续充值
	public void ContinuePay ()
	{
      
		Destroy (this.gameObject, 1f);
		if (UIStore.instance.itemBuyCoin != -1) {
			Debug.Log ("再次点击了金币充值" + UIStore.instance.itemBuyCoin);
			UIStore.instance.BuyCoin (UIStore.instance.itemBuyCoin);
			// 
		} else if (UIStore.itemBuyCard != 0) {
			Debug.Log ("再次点击了龙卡充值" + UIStore.itemBuyCard);
			UIMouthCard uIMouth = new UIMouthCard ();
			uIMouth.OnBuyMonthlyGift (UIStore.itemBuyCard);
			// 
		}

	}
}
