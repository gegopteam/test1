using UnityEngine;
using System.Collections;
using AssemblyCSharp;
using System.Collections.Generic;
using System;
using UnityEngine.UI;

public class PreferentialPay : MonoBehaviour,IUiMediator
{
	/*
     * id-1	RMB-12	一号特惠礼包		金币额度	215040
     * id-2	RMB-38	二号特惠礼包		金币额度	687040
     * id-3	RMB-68	三号特惠礼包		金币额度	1140320
     * id-4	RMB-98	四号特惠礼包		金币额度	1303200
     * id-5	RMB-188	五号特惠礼包		金币额度	2489280
    */
	public DragonCardInfo dragonCardInfo;

	private int ShowType = 0;
	private int[] DefaultRMB = new int[] { 20, 30, 50, 100, 100};
	private long[] DefaultGold = new long[] { 358400, 542400, 812000, 1340000, 13560000 };

	public GameObject[] TehuiShowArray;
	public Text[] GoldArray;
	public Text[] RMBArray;

	[SerializeField]
	private GameObject mouthcard;
	[SerializeField]
	private Camera mouthcardCamera;
	[SerializeField]
	private Canvas mainCanvas;

	private MyInfo myInfo;

	private void Awake ()
	{
		if (GameController._instance == null) {
			mouthcard = GameObject.FindGameObjectWithTag ("MainCamera");
		} else {
			mouthcard = GameObject.FindGameObjectWithTag (TagManager.uiCamera);
		}

		Debug.Log (mouthcard.name);
		mouthcardCamera = mouthcard.GetComponent<Camera> ();
		mainCanvas = transform.GetComponentInChildren<Canvas> ();
		mainCanvas.worldCamera = mouthcardCamera;
		myInfo = (MyInfo)Facade.GetFacade ().data.Get (FacadeConfig.USERINFO_MODULE_ID);
		transform.Find ("Preferential1/Purchare").GetComponent<Button> ().onClick.AddListener (delegate {
			OnPreferentialBayClick (0);
		});
		transform.Find ("Preferential2/Purchare").GetComponent<Button> ().onClick.AddListener (delegate {
			OnPreferentialBayClick (1);
		});
		transform.Find ("Preferential3/Purchare").GetComponent<Button> ().onClick.AddListener (delegate {
			OnPreferentialBayClick (2);
		});
		transform.Find ("Preferential4/Purchare").GetComponent<Button> ().onClick.AddListener (delegate {
			OnPreferentialBayClick (3);
		});
		transform.Find ("Preferential5/Purchare").GetComponent<Button> ().onClick.AddListener (delegate {
			OnPreferentialBayClick (4);
		});
	}

	private void Start ()
	{
		dragonCardInfo = (DragonCardInfo)Facade.GetFacade ().data.Get (FacadeConfig.UI_DRAGONCARD);
		PreferentialData ();
		Facade.GetFacade ().ui.Add (FacadeConfig.UI_TEIHUI, this);

		//Debug.Log("myInfo.Pay_Preferential_RMB Count = " + myInfo.Pay_Preferential_RMB.Count);
		try {
			if (myInfo.Pay_Preferential_RMB.Count > 4)
			{
				myInfo.Pay_Preferential_RMB.Sort();
				myInfo.Pay_Preferential_AddGold.Sort();
				Debug.Log(" 用下發金額 ");
				for (int rmb = 0; rmb < myInfo.Pay_Preferential_RMB.Count; rmb++)
				{
					//Debug.Log("rmb = " + rmb);
					//Debug.Log("myInfo.Pay_Preferential_RMB[rmb] = " + myInfo.Pay_Preferential_RMB[rmb]);
					//Debug.Log("myInfo.Pay_Preferential_AddGold[rmb] = " + myInfo.Pay_Preferential_AddGold[rmb]);
					RMBArray[rmb].text = "" + myInfo.Pay_Preferential_RMB[rmb];
					GoldArray[rmb].text = "" + myInfo.Pay_Preferential_AddGold[rmb];
				}
			}
			else
			{
				Debug.Log(" 用預設金額 ");
				for (int rmb = 0; rmb < myInfo.Pay_Preferential_RMB.Count; rmb++)
				{
					RMBArray[rmb].text = "" + DefaultRMB[rmb];
					GoldArray[rmb].text = "" + DefaultGold[rmb];
				}
			}
		}
		catch (Exception e) {
			Debug.Log(e.ToString());
		}
	}

	public void OnPreferentialBayClick (int name)
	{
		Debug.LogError ("name" + name);
		AudioManager._instance.PlayEffectClip (AudioManager.ui_click);
		int pay_gold = 20;
		if (myInfo.Pay_Preferential_RMB.Count > 4)
			pay_gold = (int)myInfo.Pay_Preferential_RMB[name];
        else
			pay_gold = DefaultRMB[name];

		switch (name) {
		    case 0:
			    UIToPay.DarGonCardType = 5104;
			    UIToPay.OpenThirdPartPay(pay_gold);
			    //ShowTest (4);
				    break;
		    case 1:
			    UIToPay.DarGonCardType = 5105;
			    UIToPay.OpenThirdPartPay(pay_gold);
				    //			ShowTest (5);

				    break;
		    case 2:
			    UIToPay.DarGonCardType = 5106;
			    UIToPay.OpenThirdPartPay(pay_gold);
				    //			ShowTest (6);
				    break;
		    case 3:
			    UIToPay.DarGonCardType = 5107;
			    if (myInfo.Pay_Preferential_RMB.Count > 3)
				    UIToPay.OpenThirdPartPay(pay_gold);
				    //			ShowTest (7);
				    break;
		    case 4:
			    UIToPay.DarGonCardType = 5108;
			    UIToPay.OpenThirdPartPay(pay_gold);
				    //			ShowTest (8);
				    break;
		    default:
			    break;
		}
	}

	public void OnRecvData (int nType, object nData)
	{
		Debug.LogError ("++++++++++++++++OnRecvData+++++++++++++++++++++");

		FiPurChaseTehuiRewradData nResult = (FiPurChaseTehuiRewradData)nData;
		Debug.LogError ("OnRecvData______________________");
		UIToPay.DarGonCardType = 0;
		if (nType == FiEventType.RECV_PURCHASE_TEHUI_CARD_RESPONSE) {
			if (nResult.cardType <= 0) {
				return;
			}
			Debug.LogError ("nResult.cardType" + nResult.cardType);
			ShowReward (nResult.prop, 0);

			switch (nResult.cardType) {
			case 1: 
				dragonCardInfo.fiPreferentialData.preferentialDataArray [0] = 1;
				PreferentialData ();
				break;
			case 2:
				dragonCardInfo.fiPreferentialData.preferentialDataArray [0] = 2;
				PreferentialData ();
				break;
			case 3:
				dragonCardInfo.fiPreferentialData.preferentialDataArray [0] = 3;
				PreferentialData ();
				break;
			case 4:
				dragonCardInfo.fiPreferentialData.preferentialDataArray [0] = 4;
				PreferentialData ();
				break;
			case 5:
				dragonCardInfo.fiPreferentialData.preferentialDataArray [0] = -1;
				myInfo.teihui = -1;
				UIHallCore nHallCore = (UIHallCore)Facade.GetFacade ().ui.Get (FacadeConfig.UI_HALL_MODULE_ID);
				if (nHallCore != null && nHallCore.gameObject.activeSelf) {
					nHallCore.BtnPreference.SetActive (false);
				}
				Destroy (this.gameObject);

				break;
			}
		}
	}
	//显示奖励
	public void ShowReward (List<FiProperty> nPorpList, int type)
	{
		UnityEngine.GameObject Window = UnityEngine.Resources.Load ("Window/RewardWindow")as UnityEngine.GameObject;
		GameObject rewardInstance = UnityEngine.GameObject.Instantiate (Window);
		UIReward reward = rewardInstance.GetComponent<UIReward> ();
		reward.ShowRewardType (type);
		reward.SetRewardData (nPorpList);
	}

	public void OnInit ()
	{
		
	}

	public void OnRelease ()
	{
		
	}
	//关闭
	public void OnCloseClick ()
	{
		AudioManager._instance.PlayEffectClip (AudioManager.ui_click);
		if (ReSignDay.isFristPrefapay) {
			ReSignDay.isFristPrefapay = false;
			// string path = "MainHall/RankList/BigwinRankWindowCanvas"; //"Window/BagWindow";


			//if (UIHallCore.PopWindowCase.Equals("C"))
			//{
			//	string path = "MainHall/Activity/ActivityCanvas"; //"Window/Activity";
			//	GameObject WindowClone = AppControl.OpenWindow(path);
			//	WindowClone.SetActive(true);
			//}
			
		}

		if (myInfo.isNewSevenHand)
		{
			if (myInfo.isLevelupCanGet)
			{
				if (UIHallCore.isFristPreferential && AppInfo.trenchNum > 51000000)
				{
					UIHallCore.isFristPreferential = false;
					Debug.Log(" 新手升級任務 NewLevelupGrade ");
					//DataControl.GetInstance().PushSocketSnd(FiEventType.SEND_XL_LEVELUP_INFO_NEW_RESPOSE, null);
					//string path = "Window/NewLevelupGrade";
					//GameObject WindowClone = AppControl.OpenWindow(path);
					//WindowClone.SetActive(true);

				}
			}
		}
		else {
			if (UIHallCore.isFristPreferential)
			{
				UIHallCore.isFristPreferential = false;
				string path = "MainHall/RankList/BigwinRankWindowCanvas";
				GameObject WindowClone = AppControl.OpenWindow(path);
				WindowClone.SetActive(true);
			}
		}

		Destroy (this.gameObject); 

	}

	public void IncludeAllType ()
	{
		for (int i = 0; i < TehuiShowArray.Length; i++) {
			TehuiShowArray [i].gameObject.SetActive (false);
		}
	}

	public void PreferentialData ()
	{
		Debug.LogError ("112233:" + dragonCardInfo.fiPreferentialData.preferentialDataArray.Count);
		Debug.LogError ("2222222222222:" + dragonCardInfo.fiPreferentialData.preferentialDataArray [0]);
		ShowType = dragonCardInfo.fiPreferentialData.preferentialDataArray [0];
		if (ShowType < 0) {
			return;
		}
		switch (ShowType) {
		case 0:
			for (int i = 0; i < TehuiShowArray.Length; i++) {
				if (i == 0) {
					TehuiShowArray [i].gameObject.SetActive (true);
				} else {
					TehuiShowArray [i].gameObject.SetActive (false);
				}
			}
			break;
		case 1:
			for (int i = 0; i < TehuiShowArray.Length; i++) {
				if (i == 1) {
					TehuiShowArray [i].gameObject.SetActive (true);
				} else {
					TehuiShowArray [i].gameObject.SetActive (false);
				}
			}
			break;
		case 2:
			for (int i = 0; i < TehuiShowArray.Length; i++) {
				if (i == 2) {
					TehuiShowArray [i].gameObject.SetActive (true);
				} else {
					TehuiShowArray [i].gameObject.SetActive (false);
				}
			}
			break;
		case 3:
			for (int i = 0; i < TehuiShowArray.Length; i++) {
				if (i == 3) {
					TehuiShowArray [i].gameObject.SetActive (true);
				} else {
					TehuiShowArray [i].gameObject.SetActive (false);
				}
			}
			break;
		case 4:
			for (int i = 0; i < TehuiShowArray.Length; i++) {
				if (i == 4) {
					TehuiShowArray [i].gameObject.SetActive (true);
				} else {
					TehuiShowArray [i].gameObject.SetActive (false);
				}
			}
			break;
		}

	}

	void ShowTest (int  type)
	{
		string Urltest;
		Urltest = "http://183.131.69.227:8004/pay/notifygameserverfortest" + "?UserID=" + myInfo.userID + "&cardtype=" + type;
		Debug.LogError ("Urltest" + Urltest);
		//		OpenWebScript.Instance.SetActivityWebUrl (Urltest);
		Application.OpenURL (Urltest);

	}

	private void OnDestroy ()
	{
		Facade.GetFacade ().ui.Remove (FacadeConfig.UI_TEIHUI);
	}
}
