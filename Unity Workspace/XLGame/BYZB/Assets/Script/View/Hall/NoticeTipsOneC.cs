using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using AssemblyCSharp;

public class NoticeTipsOneC : MonoBehaviour
{

	public Button store;
	public Button refuse;
	public Text content;
	// Use this for initialization
	MyInfo myInfo;

	void Awake ()
	{
		store.onClick.AddListener (GoToStore);
		refuse.onClick.AddListener (Refuse);
		myInfo = (MyInfo)Facade.GetFacade ().data.Get (FacadeConfig.USERINFO_MODULE_ID);
		UIColseManage.instance.ShowUI (this.gameObject);
	}

	void GoToStore ()
	{
		AudioManager._instance.PlayEffectClip (AudioManager.ui_click);
		if (myInfo.isGuestLogin) {
			StartGiftManager.GuestToStoreManager ();
			//return;
		}
        //string path = "Window/StoreWindow";//商店预制体名称更换
        string path = "Window/NewStoreCanvas";
		GameObject WindowClone = AppControl.OpenWindow (path);
		WindowClone.SetActive (true);
		WindowClone.GetComponent<UIStore> ().DiamondButton ();
		Destroy (this.gameObject);
		Destroy (NoticeManager.instans.gameObject);
	}

	void Refuse ()
	{
		AudioManager._instance.PlayEffectClip (AudioManager.ui_click);
		DestroyImmediate (this.gameObject);
	}

	public void RemoveListener ()
	{
		store.onClick.RemoveAllListeners ();
		refuse.onClick.RemoveAllListeners ();
	}
}
