using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using AssemblyCSharp;
public class WinDowTipGoshop : MonoBehaviour {

	public Text tipText;
	private Button sure;
	private Button cancel;
	// Use this for initialization
	void Awake()
	{
		tipText = transform.Find ("Label").gameObject.GetComponent<Text>();
		sure = transform.Find ("sure").gameObject.GetComponent<Button> ();
		cancel = transform.Find ("cancel").gameObject.GetComponent<Button> ();
	}
	void Start () {
		UIColseManage.instance.ShowUI (this.gameObject);
		sure.onClick.AddListener (SureButton);
		cancel.onClick.AddListener (CancelButton);
	}

	// Update is called once per frame
	public void SureButton()
	{
		MyInfo myInfo = (MyInfo)Facade.GetFacade ().data.Get (FacadeConfig.USERINFO_MODULE_ID);

		if (myInfo.isGuestLogin) {
			StartGiftManager.GuestToStoreManager ();
			//return;
		}
		//先判断是否有没有首充过，如果首充过，则先弹出首充特惠的窗口，首充过则直接弹出商城的界面
        //string path = "Window/StoreWindow";//商店预制体名称更换
        string path = "Window/NewStoreCanvas";
		GameObject WindowClone = AppControl.OpenWindow (path);
		WindowClone.SetActive (true);
		WindowClone.GetComponent<UIStore> ().CoinButton ();

		UIColseManage.instance.CloseUI ();
	}

	public void CancelButton()
	{
		UIColseManage.instance.CloseUI ();
	}
}
