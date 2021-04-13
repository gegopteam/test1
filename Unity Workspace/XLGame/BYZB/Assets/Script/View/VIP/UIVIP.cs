using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using AssemblyCSharp;

public class UIVIP : MonoBehaviour
{
	public delegate void Seedelegate ();

	public static event Seedelegate SeeEvent;

	public static bool isopen = false;
	public GameObject leftButton = null;
	public GameObject rightButton = null;
	public GameObject hallWindow;

	private GameObject WindowClone;
	private VipSliderContrl vipSlider;
	public GameObject VIP;

	private Camera VIPCamera;
    public static GameObject VIPLevel;
	
	//public Canvas [] mainCanvas;
	MyInfo myInfo;

	void Awake ()
	{
        VIPLevel = transform.Find("VIP_Canvas/Buttom/Tips_Image").gameObject;
        Debug.Log(VIPLevel.name);
		QuitBack.SeeEvent += ExitButton;
		myInfo = (MyInfo)Facade.GetFacade ().data.Get (FacadeConfig.USERINFO_MODULE_ID);
		//VIP = GameObject.FindGameObjectWithTag ("UICamera");
		//if(VIP==null)
		//VIP = GameObject.FindGameObjectWithTag ("MainCamera");
		// Debug.Log (VIP.name); //
		// VIPCamera = VIP.GetComponent<Camera> (); //
		//mainCanvas = transform.GetComponentsInChildren<Canvas> ();
		//mainCanvas[0].worldCamera = VIPCamera;
		//mainCanvas [1].worldCamera = VIPCamera;
		//mainCanvas [2].worldCamera = VIPCamera;
		// vipSlider = transform.GetComponentInChildren<VipSliderContrl> (); //
	}

	// Use this for initialization
	void Start ()
	{
		//DontDestroyOnLoad (transform.gameObject);
		UIColseManage.instance.ShowUI (this.gameObject);
	}

	void OnDestroy ()
	{
		QuitBack.SeeEvent -= ExitButton;
		UIColseManage.instance.CloseUI ();
	}

	// Update is called once per frame
	void Update ()
	{
		if (null == leftButton || null == leftButton)
			return;
		if (StoreRectHelper.currentIndex == 0 || StoreRectHelper.currentIndex == 8) {
			switch (StoreRectHelper.currentIndex) {
			case 0:
				leftButton.SetActive (false);
				rightButton.SetActive (true);
				break;
			case 8:
				leftButton.SetActive (true);
				rightButton.SetActive (false);
				break;
			}
		} else {
			leftButton.SetActive (true);
			rightButton.SetActive (true);
		}
	}

	public void ExitButton ()
	{
		AudioManager._instance.PlayEffectClip (AudioManager.effect_closePanel);
		Destroy (this.gameObject);

		if (SeeEvent != null) {

			SeeEvent ();
		}

	}

	public void AddMoney ()
	{
		AudioManager._instance.PlayEffectClip (AudioManager.ui_click);
		if (SeeEvent != null) {
			SeeEvent ();
		}

		DestroyImmediate (this.gameObject);
		if (myInfo.isGuestLogin) {
			StartGiftManager.GuestToStoreManager ();
			//return;
		}
        //string path = "Window/StoreWindow";//商店预制体名称更换
        string path = "Window/NewStoreCanvas";
		GameObject WindowClone = AppControl.OpenWindow (path);
		WindowClone.SetActive (true);
		WindowClone.GetComponent<UIStore> ().CoinButton ();
		UIColseManage.instance.ShowUI (WindowClone);

		UIStore.openWindow = true;

		OpenStore.name = "";

	}
}
