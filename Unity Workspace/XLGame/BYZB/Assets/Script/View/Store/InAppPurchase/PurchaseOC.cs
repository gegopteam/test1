using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//该类负责IOS内购信息的回调，PurchaseOC为IOS回调的接收者
//必须挂在场景固定的对象上，对象不能是动态创建的，比如：相机

public class PurchaseOC : MonoBehaviour
{
	public PurchaseOC ()
	{

	}

	~PurchaseOC ()
	{

	}

	// Use this for initialization
	void Awake ()
	{
		
	}

	void Start ()
	{
		Invoke ("InitPurchase", 0.5f);
		//InitPurchase ();
	}

	// Update is called once per frame
	void Update ()
	{

	}

	private void InitPurchase ()
	{
		PurchaseControl.GetInstance ().UpdatePurchaseState ();
		#if UNITY_IPHONE && !UNITY_APP_WEB
		//Tool.OutLogWithToFile("InitPurchase");
		try {
			PurchaseControl.InitPurchaseIOS ();
			PurchaseControl.SetPurchaseBackIOS (gameObject.name, "PurchaseBack");
		} catch {
		
		}
		#endif
	}

	public void PurchaseBack (string content)
	{
		//Tool.OutLogWithToFile ("PurchaseBack："+content);
		#if UNITY_IPHONE && !UNITY_APP_WEB
		PurchaseControl.GetInstance ().IOSPurchaseBack (content);
		UIStore.instance.ShowStoreWaitingView (false);
		#endif
	}


}
