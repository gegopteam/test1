using System;
using AssemblyCSharp;
using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

public class UIBackPack : MonoBehaviour, IUiMediator
{
	private const int PAGE_FRIEND = 0;

	private const int PAGE_APPLY = 1;

	private const int PAGE_ADD = 2;

	public Image WholeImage;

	public Image BatteryImage;

	public Image PropImage;

	private List<Image> nImageList = new List<Image> ();

	public Image BtnToolOn;

	public Image BtnCannonOn;

	public Image BtnTotalOn;

	public Text TxtGold;

	public Text TxtDiamond;

	//更新金币
	public const int UPDATE_GOLD = 1001;
	//更新砖石
	public const int UPDATE_DIAMOND = 1002;
	//更新
	public const int UPDATE_ALL = 1003;
	//更新
	public const int UPDATE_STATE = 1004;

	public int DefaultUserId = 0;

	public static UIBackPack Instance;
	private bool Display = true;

	MyInfo myInfo;

	public UIBackPack ()
	{

	}

	void Awake ()
	{
		myInfo = (MyInfo)Facade.GetFacade ().data.Get (FacadeConfig.USERINFO_MODULE_ID);
	}


	void Start ()
	{
		Facade.GetFacade ().ui.Add (FacadeConfig.UI_BACKPACK_MODULE_ID, this);
		DoSyncGold ();
		DoSyncDiamond ();
		Instance = this;
		if (this.GetComponent<Canvas> ().worldCamera == null) {
			//Debug.LogError ( "222222" );
			this.GetComponent<Canvas> ().worldCamera = GameObject.FindWithTag ("MainCamera").GetComponent<Camera> ();
		}
		nImageList.Add (WholeImage);
		nImageList.Add (BatteryImage);
		nImageList.Add (PropImage);
		UIColseManage.instance.ShowUI (this.gameObject);
	}

	void SwitchToPage (int nPageType)
	{
       
		Image toSelectImage = null;
		switch (nPageType) {
		case PAGE_ADD:
              
			toSelectImage = WholeImage;
			break;
		case PAGE_APPLY:
               
			toSelectImage = BatteryImage;
			break;
		case PAGE_FRIEND:
              
			toSelectImage = PropImage;
			break;
		}
		for (int i = 0; i < nImageList.Count; i++) {
			if (nImageList [i].Equals (toSelectImage)) {
				toSelectImage.gameObject.SetActive (true);
				nImageList [i].color = Color.white;
			} else {
				nImageList [i].gameObject.SetActive (false);
				nImageList [i].color = Color.clear;
			}
		}

	}

	void SelectPage (Image nTargetBtn)
	{
		BtnToolOn.gameObject.SetActive (false);
		BtnCannonOn.gameObject.SetActive (false);
		BtnTotalOn.gameObject.SetActive (false);
		nTargetBtn.gameObject.SetActive (true);
	}

	public void OnSelectTotal ()
	{
		SelectPage (BtnTotalOn);
		UIBackPack_Grids.Instance.ResetSelected ();
		UIBackPack_Grids.Instance.UpdateUnits (UIBackPack_Grids.TOTAL);
		SwitchToPage (PAGE_ADD);
	}

	public void OnSelectCannon ()
	{
		SelectPage (BtnCannonOn);
		UIBackPack_Grids.Instance.ResetSelected ();
		UIBackPack_Grids.Instance.UpdateUnits (UIBackPack_Grids.CANNON);
		SwitchToPage (PAGE_FRIEND);

	}

	public void OnSelectTool ()
	{
		SelectPage (BtnToolOn);
		UIBackPack_Grids.Instance.ResetSelected ();
		UIBackPack_Grids.Instance.UpdateUnits (UIBackPack_Grids.TOOL);
		SwitchToPage (PAGE_APPLY);
	}

	public void OnBtnClose ()
	{
		UIColseManage.instance.CloseUI ();
	}


	public void OnOpenGold ()
	{
		if (myInfo.isGuestLogin) {
			StartGiftManager.GuestToStoreManager ();
			//return;
		}
		UIStore.openWindow = true;
        //string path = "Window/StoreWindow";//商店预制体名称更换
        string path = "Window/NewStoreCanvas";
		GameObject WindowClone = AppControl.OpenWindow (path);
		WindowClone.SetActive (true);
		WindowClone.GetComponent<UIStore> ().CoinButton ();
	}

	public void OnOpenDiamond ()
	{
		if (myInfo.isGuestLogin) {
			StartGiftManager.GuestToStoreManager ();
			//return;
		}
		UIStore.openWindow = true;
        //string path = "Window/StoreWindow";//商店预制体名称更换
        string path = "Window/NewStoreCanvas";
		GameObject WindowClone = AppControl.OpenWindow (path);
		WindowClone.SetActive (true);
		WindowClone.GetComponent<UIStore> ().DiamondButton ();
	}


	public void DoSyncGold ()
	{
		MyInfo mInfo = (MyInfo)Facade.GetFacade ().data.Get (FacadeConfig.USERINFO_MODULE_ID);
		if (mInfo.gold >= 1000000) {
			TxtGold.text = "" + (int)(mInfo.gold / 10000) + "萬";
		} else {
			TxtGold.text = "" + mInfo.gold;
		}
	}

	public void DoSyncDiamond ()
	{
		MyInfo mInfo = (MyInfo)Facade.GetFacade ().data.Get (FacadeConfig.USERINFO_MODULE_ID);
		if (mInfo.diamond >= 1000000) {
			TxtDiamond.text = "" + (int)(mInfo.diamond) / 10000 + "萬";
		} else {
			TxtDiamond.text = "" + mInfo.diamond;
		}
	}


	public void OnRecvData (int nType, object nData)
	{
		Debug.LogError ( "--------OnRecvData-------" + nType);

		if (nType == UPDATE_GOLD) { //充值消息反馈，更新金币
			DoSyncGold ();
		} else if (nType == UPDATE_DIAMOND) {
			DoSyncDiamond ();
		} else if (nType == UPDATE_ALL) {
			UIBackPack_Grids.Instance.Refresh ();
			UIBackPack_Brief.instance.Refresh ();
			DoSyncGold ();
			DoSyncDiamond ();
		} else if (nType == UPDATE_STATE) {
			UIBackPack_Grids.Instance.Refresh ();
			UIBackPack_Brief.instance.Refresh ();
		}
	}

	public void OnInit ()
	{

	}

	public void OnRelease ()
	{

	}


	void OnDestroy ()
	{
		Instance = null;
		Facade.GetFacade ().ui.Remove (FacadeConfig.UI_BACKPACK_MODULE_ID);
	}
}

