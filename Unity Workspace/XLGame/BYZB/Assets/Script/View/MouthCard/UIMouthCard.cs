using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;
using AssemblyCSharp;

/// <summary>
/// User interface mouth card.月卡礼包
/// </summary>
/// 
public class DragonArray
{
	//	private List<FiDgonCard> arrays = new List<FiDgonCard> ();
	//
	//	public  List<FiDgonCard> GetArray ()
	//	{
	//
	//		FiDgonCard s = new FiDgonCard ();
	//		s.day = 0;
	//		s.status = 0;
	//		FiDgonCard s1 = new FiDgonCard ();
	//		s1.day = 20;
	//		s1.status = 1;
	//		FiDgonCard s2 = new FiDgonCard ();
	//		s2.day = 9;
	//		s2.status = 1;
	//
	//		arrays.Add (s);
	//		arrays.Add (s1);
	//		arrays.Add (s2);
	//		return arrays;
	//	}
}

public enum BOOLISSTORE
{
	NONE,
	SURE
}

public class UIMouthCard : MonoBehaviour , IUiMediator
{
	
	[SerializeField]
	private GameObject mouthcard;
	[SerializeField]
	private Camera mouthcardCamera;
	[SerializeField]
	private Canvas mainCanvas;

	//未购买龙卡对象 一下都是银龙 到神龙的序列排的数组
	public GameObject[] NopurchaseCardArray;
	//已购买龙卡对象
	public GameObject[] PurchaseCardArray;
	//剩余天数集合
	public Text[] CardDaysCost;
	//龙卡立即获得奖励的金币数
	public Text[] RewardGoldNumArray;
	//这是邮箱的龙卡奖励金币数
	public Text[] MailRewardGoldNumArray;
	//顯示下發付款金額
	public Text[] Button_RMB_Text;
	//顯示繁體版付款金幣
	public Text[] Button_Gold_Text;
	//根據繁體版、推廣版 顯示不同按鈕 繁體版＝金幣購買，推廣版＝ＲＭＢ購買
	public GameObject[] Buy_Button;

	public DragonCardInfo mDragonCardInfo;
	public BOOLISSTORE boolisstore;
	long buycostTemp;
	private int[] DefaultRMB = new int[] {30, 30, 100};
	private long[] DefaultGold = new long[] { 100000, 50000, 200000 };
	private long[] DefaultGoldTC = new long[] {450000, 600000, 1200000 };
	private long[] DefaultDailyGold = new long[] { 20000 , 10000 , 15000 };
	private MyInfo myInfo;
	Transform dragonControl;

	void Awake ()
	{
		dragonControl = transform.Find ("DragonControl");
		Debug.Log ("dragonControl . name = " + dragonControl.name);
		if (Facade.GetFacade ().config.isIphoneX2 ()) {
			dragonControl.localScale = new Vector3 (.9f, .9f, .9f);
		}
		if (GameController._instance == null) {
			mouthcard = GameObject.FindGameObjectWithTag ("MainCamera");
		} else {
			mouthcard = GameObject.FindGameObjectWithTag (TagManager.uiCamera);
		}

		Debug.Log (mouthcard.name);
		mouthcardCamera = mouthcard.GetComponent<Camera> ();
		mainCanvas = transform.GetComponentInChildren<Canvas> ();
		mainCanvas.worldCamera = mouthcardCamera;

		Facade.GetFacade ().ui.Add (FacadeConfig.UI_DRAGONCARD, this);
		myInfo = (MyInfo)Facade.GetFacade ().data.Get (FacadeConfig.USERINFO_MODULE_ID);
		mDragonCardInfo = (DragonCardInfo)Facade.GetFacade ().data.Get (FacadeConfig.UI_DRAGONCARD);
		if (AppInfo.trenchNum == 51000002 || AppInfo.trenchNum == 51000000 )
		{
			Buy_Button[3].SetActive(true);
			Buy_Button[0].SetActive(false);
			transform.Find("DragonControl/Canvas/SilverCard/Buy_Btn_S_TC").GetComponent<Button>().onClick.AddListener(delegate {
				newOnBuyMonthGift(1);
				//OnBuyMonthlyGift(1);
			});
			Buy_Button[4].SetActive(true);
			Buy_Button[1].SetActive(false);
			transform.Find("DragonControl/Canvas/GlodCard/Buy_Btn_G_TC").GetComponent<Button>().onClick.AddListener(delegate {
				newOnBuyMonthGift(2);
				//OnBuyMonthlyGift(2);
			});
			Buy_Button[5].SetActive(true);
			Buy_Button[2].SetActive(false);
			transform.Find("DragonControl/Canvas/ShenlongCard/Buy_Btn_SL_TC").GetComponent<Button>().onClick.AddListener(delegate {
				newOnBuyMonthGift(3);
				//OnBuyMonthlyGift(3);
			});
		}
		//else
  //      {
		//	Buy_Button[0].SetActive(true);
		//	Buy_Button[3].SetActive(false);
		//	transform.Find("DragonControl/Canvas/SilverCard/Buy_Btn_S").GetComponent<Button>().onClick.AddListener(delegate {
		//		newOnBuyMonthGift(1);
		//		//OnBuyMonthlyGift(1);
		//	});
		//	Buy_Button[1].SetActive(true);
		//	Buy_Button[4].SetActive(false);
		//	transform.Find("DragonControl/Canvas/GlodCard/Buy_Btn_G").GetComponent<Button>().onClick.AddListener(delegate {
		//		newOnBuyMonthGift(2);
		//		//OnBuyMonthlyGift(2);
		//	});
		//	Buy_Button[2].SetActive(true);
		//	Buy_Button[5].SetActive(false);
		//	transform.Find("DragonControl/Canvas/ShenlongCard/Buy_Btn_SL").GetComponent<Button>().onClick.AddListener(delegate {
		//		newOnBuyMonthGift(3);
		//		//OnBuyMonthlyGift(3);
		//	});
		//}
//		mDragonCardInfo.AppendDgArrayData (myInfo.loginInfo.dgoncardArray);//这个准备放在登录或者大厅的时候获取数据

	}
		
	// Use this for initialization
	void Start ()
	{
		if (boolisstore == BOOLISSTORE.NONE) {
			UIColseManage.instance.ShowUI (this.gameObject);
		}

        //顯示下發的金額、金幣
		if (myInfo.Pay_Drang_RMB.Count > 2)
		{
			//myInfo.Pay_Drang_RMB.Sort();
			//myInfo.Pay_Drang_AddGold.Sort();
			for (int btn = 0; btn < myInfo.Pay_Drang_RMB.Count; btn++)
			{
				Button_RMB_Text[btn].text = "" + myInfo.Pay_Drang_RMB[btn];
				//Button_Gold_Text[btn].text = "" + ((int)myInfo.Pay_Drang_RMB[btn] * 16000);
				Button_Gold_Text[btn].text = DefaultGoldTC[btn].ToString();
				RewardGoldNumArray[btn].text = "" + myInfo.Pay_Drang_AddGold[btn];
			}
		}
		else {
			for (int btn = 0; btn < DefaultGold.Length; btn++)
			{
				Button_RMB_Text[btn].text = "" + DefaultRMB[btn];
				//Button_Gold_Text[btn].text = "" + ((int)DefaultRMB[btn] * 16000);
				Button_Gold_Text[btn].text = DefaultGoldTC[btn].ToString();
				RewardGoldNumArray[btn].text = "" + DefaultGold[btn];
			}
		}

		FreshDragonDate ();
	}


	public void OnInit ()
	{
		Debug.LogError ("OnInit______________________");

	}

	//購買龍卡成功后的处理
	public void OnRecvData (int nType, object nData)
	{
		Debug.Log(" 購買龍卡接收 OnRecvData " + nType);
		FiPurChaseDraGonRewradData nResult = (FiPurChaseDraGonRewradData)nData;
		Debug.LogError ("OnRecvData______________________ "+ nResult.cardType);
		Debug.LogError("OnRecvData______________________ " + nResult.cardType);
		UIToPay.DarGonCardType = 0;
		if (nType == FiEventType.RECV_PURCHASE_DARGON_CARD_RESPONSE) {
			if (nResult.cardType <= 0) {
				return;
			}
			myInfo.gold = myInfo.gold - buycostTemp;
			ShowReward (nResult.prop, nResult.cardType);

			switch (nResult.cardType) {
			case 1: 
				mDragonCardInfo.SetCardStatue (nResult.cardType);
				FreshDragonDate ();
				break;
			case 2:
				mDragonCardInfo.SetCardStatue (nResult.cardType);
				FreshDragonDate ();
				break;
			case 3:
				mDragonCardInfo.SetCardStatue (nResult.cardType);
				FreshDragonDate ();
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
	//刷新龙卡数据
	public void FreshDragonDate ()
	{
		
		//显示龙卡数据
		for (int i = 0; i < mDragonCardInfo.mFiDragonCardData.DarGonCardDataArray.Count; i++) {
			if (mDragonCardInfo.mFiDragonCardData.DarGonCardDataArray [i] <= 0) {
				Debug.LogError ("11111111111111111SSSSSS" + mDragonCardInfo.mFiDragonCardData.DarGonCardDataArray [i]);
				NopurchaseCardArray [i].gameObject.SetActive (true);
				PurchaseCardArray [i].gameObject.SetActive (false);

			} else {
				PurchaseCardArray [i].gameObject.SetActive (true);
				NopurchaseCardArray [i].gameObject.SetActive (false);
				Debug.LogError (mDragonCardInfo.mFiDragonCardData.DarGonCardDataArray [i]);
				CardDaysCost [i].text = mDragonCardInfo.mFiDragonCardData.DarGonCardDataArray [i].ToString ();
			}
		}
	}

	public void OnRelease ()
	{

	}
	//设置是否已经领取
	public void SetHaveAccpeted ()
	{
		
	}
	//
	public void  SetRemianDays (int nDay, bool bRecvd)
	{

	}

	// Update is called once per frame
	void Update ()
	{

	}

	public void OnExit ()
	{
		AudioManager._instance.PlayEffectClip (AudioManager.effect_closePanel);
		UIColseManage.instance.CloseUI ();
	}


	public void OnRecvMonthlyGift ()
	{
		//Debug.LogError ( "--------OnRecvGift---------" );
		//获取月卡礼包的每日奖励
		Facade.GetFacade ().message.toolPruchase.SendGetMonthlyPackReqeust ();
		Destroy (gameObject);
	}

	public void newOnBuyMonthGift(int type) {
		if (myInfo.gold >= DefaultGoldTC[type - 1]) {
			buycostTemp = DefaultGoldTC[type - 1];
			Facade.GetFacade().message.toolPruchase.SendGoldBuyDragonGiftRequest(type);
		}
		else
		{
			GameObject Window = Resources.Load("Window/WindowTips") as GameObject;
			GameObject nTip = Instantiate(Window);
			UITipClickHideManager ClickTips = nTip.GetComponent<UITipClickHideManager>();
			ClickTips.text.text = "金幣餘額不足";


			//GameObject WindowClone1 = UnityEngine.Resources.Load("Window/WindowTipsThree") as UnityEngine.GameObject;
			//UITipAutoNoMask ClickTips1 = WindowClone1.GetComponent<UITipAutoNoMask>();
			//ClickTips1.tipText.text = "金幣餘額不足";
		}
	}

	public void OnBuyMonthlyGift (int type)
	{
		Debug.LogError ("--------OnBuyMonthlyGift---------");
		UIStore.itemBuyCard = type;

		int pay_gold = 30;
		if (myInfo.Pay_Drang_RMB.Count > 2)
			pay_gold = (int)myInfo.Pay_Drang_RMB[type-1];
		else
			pay_gold = DefaultRMB[type-1];

		switch (type) {
		case 1:
			UIToPay.DarGonCardType = 5101;
			UIToPay.OpenThirdPartPay (pay_gold);
//			ShowTest (1);	
			break;
		case 2:
			UIToPay.DarGonCardType = 5102;
			UIToPay.OpenThirdPartPay (pay_gold);
//			ShowTest (2);
			break;
		case 3:
			UIToPay.DarGonCardType = 5103;
			UIToPay.OpenThirdPartPay (pay_gold);
//			ShowTest (3);
			break;
		default:
			break;
		}
	}

//	void ShowTest (int  type)
//	{
//		string Urltest;
//		Urltest = "http://183.131.69.227:8004/pay/notifygameserverfortest" + "?UserID=" + myInfo.userID + "&cardtype=" + type;
//		Debug.LogError ("Urltest" + Urltest);
////		OpenWebScript.Instance.SetActivityWebUrl (Urltest);
//		Application.OpenURL (Urltest);

//	}
}
