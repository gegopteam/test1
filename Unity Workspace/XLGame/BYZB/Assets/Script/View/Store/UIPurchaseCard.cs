using System;
using AssemblyCSharp;
using UnityEngine;

public class UIPurchaseCard : MonoBehaviour
{
	public UIPurchaseCard ()
	{
		
	}

	public void OnConfirm()
	{
		Destroy ( gameObject );
		
		//购买房卡
		#if UNITY_IPHONE && !UNITY_APP_WEB
			UIToPay.OpenApplePay ( ProductID.Card_Room_CNY_6 );
		#elif UNITY_ANDROID || UNITY_APP_WEB
			UIToPay.OpenThirdPartPay( 6 );// OpenUIToPay(ProductID.Card_Room_CNY_6);
		#endif
	}

	public void OnCancel()
	{
		Destroy ( gameObject );
	}
}