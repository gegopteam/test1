using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using AssemblyCSharp;

public class OpenStore : MonoBehaviour , IUiMediator
{
	//	public delegate void storeDelegate(string name);
	//	public static event storeDelegate storeEvent;

	private Button[] hallButtons;

	private GameObject WindowClone;

	public static string name;

	MyInfo myInfo;

	void Awake ()
	{
		myInfo = (MyInfo)Facade.GetFacade ().data.Get (FacadeConfig.USERINFO_MODULE_ID);
	}

	public void OnRecvData (int nType, object nData)
	{
		if (nType == FiPropertyType.GOLD) {
			SetGold ((long)nData);
		} else if (nType == FiPropertyType.DIAMOND) {
			SetDiamond ((long)nData);
		} else if (nType == FiPropertyType.ROOM_CARD) {
			SetRoomCard ((long)nData);
		}
	}

	public void OnInit ()
	{
		
	}

	public void OnRelease ()
	{
		
	}

	private void SetGold (long nCount)
	{
		Text[] nText = transform.GetComponentsInChildren <Text> ();
		for (int i = 0; i < nText.Length; i++) {

			if (nText [i].transform.GetComponentInParent<Image> () != null && nText [i].transform.GetComponentInParent<Image> ().name.Equals ("Coin")) {
				if (nCount >= 1000000) {
					nText [i].text = "" + (int)(nCount / 10000) + "萬";
				} else {
					nText [i].text = "" + nCount;
				}
				break;
			}
			//	Debug.LogError ( nText[i].text );
		}
	}

	public void SetRoomCard (long nCount)
	{
		Text[] nText = transform.GetComponentsInChildren <Text> ();
		for (int i = 0; i < nText.Length; i++) {
			if (nText [i].transform.GetComponentInParent<Image> () != null && nText [i].transform.GetComponentInParent<Image> ().name.Equals ("Lottery")) {
				if (nCount >= 1000000) {
					nText [i].text = "" + (int)(nCount / 10000) + "萬";
				} else {
					nText [i].text = "" + nCount;
				}
				break;
			}
		}
	}

	public void SetDiamond (long nCount)
	{
		Text[] nText = transform.GetComponentsInChildren <Text> ();
		for (int i = 0; i < nText.Length; i++) {
			
			if (nText [i].transform.GetComponentInParent<Image> () != null && nText [i].transform.GetComponentInParent<Image> ().name.Equals ("Brilliant")) {
				if (nCount >= 1000000) {
					nText [i].text = "" + (int)(nCount / 10000) + "萬";
				} else {
					nText [i].text = "" + nCount;
				}
				break;
			}
			//	Debug.LogError ( nText[i].text );
		}
	}

	// Use this for initialization
	void Start ()
	{
		hallButtons = transform.GetComponentsInChildren<Button> ();
		for (int i = 0; i < hallButtons.Length; i++) {
			EventTriggerListener.Get (hallButtons [i].gameObject).onClick = OnButton;
		}
		Facade.GetFacade ().ui.Add (FacadeConfig.UI_STORE_MODULE_ID, this);

		MyInfo myInfo = (MyInfo)Facade.GetFacade ().data.Get (FacadeConfig.USERINFO_MODULE_ID);
		SetDiamond (myInfo.diamond);
		SetGold (myInfo.gold);
		SetRoomCard (myInfo.roomCard);
	}

	public void OpenNotice ()
	{
		string path = "Window/NoticeWindow";
		GameObject WindowClone = AppControl.OpenWindow (path);
		WindowClone.SetActive (true);
	}

	public void OnOpenMall ()
	{
		AudioManager._instance.PlayEffectClip (AudioManager.ui_click);
		if (myInfo.isGuestLogin) {
			StartGiftManager.GuestToStoreManager ();
			//return;
		}
		name = "Mall";
		UIStore.openWindow = true;
        //string path = "Window/StoreWindow";//商店预制体名称更换
        string path = "Window/NewStoreCanvas";
		WindowClone = AppControl.OpenWindow (path);
		WindowClone.SetActive (true);
		WindowClone.GetComponent<UIStore> ().CoinButton ();
		//Reset ();
	}

	void OnButton (GameObject go)
	{
		if (go.name.Equals ("CoinButton") || go.name.Equals ("BrilliantButton")) {
			AudioManager._instance.PlayEffectClip (AudioManager.ui_click);
			name = go.name;
			UIStore.openWindow = true;
			Debug.Log (go.name);
			Reset (go.name);
		} else if (go.name.Equals ("UserIcon")) {
			AudioManager._instance.PlayEffectClip (AudioManager.ui_click);
			string path = "Window/PersonalInfo";
			GameObject WindowClone = AppControl.OpenWindow (path);
			WindowClone.SetActive (true);
		} else if (go.name.Equals ("LotteryButton")) {
			string path = "Window/RoomCardTip";
			GameObject WindowClone = AppControl.OpenWindow (path);
			WindowClone.SetActive (true);
		}
	}

	void Reset (string name)
	{
		if (myInfo.isGuestLogin) {
			StartGiftManager.GuestToStoreManager ();
			//return;
		}
        //string path = "Window/StoreWindow";//商店预制体名称更换
        string path = "Window/NewStoreCanvas";
		WindowClone = AppControl.OpenWindow (path);
		WindowClone.SetActive (true);
		if (name == "CoinButton")
			WindowClone.GetComponent<UIStore> ().CoinButton ();
		else
			WindowClone.GetComponent<UIStore> ().DiamondButton ();
	}
	// Update is called once per frame
	void Update ()
	{
	
	}

	void OnDestroy ()
	{
		Facade.GetFacade ().ui.Remove (FacadeConfig.UI_STORE_MODULE_ID);
	}
}
