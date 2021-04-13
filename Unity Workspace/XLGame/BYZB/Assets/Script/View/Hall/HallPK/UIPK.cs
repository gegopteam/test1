//using UnityEngine;
//using System.Collections;
//using UnityEngine.UI;
///// <summary>
///// UIP.进入场次的选择逻辑
///// </summary>
//
//public class UIPK : MonoBehaviour{
//	private Button[] itemButtons;
//
//	private GameObject WindowClone;
//
//	public Text nickName;
//
//	// Use this for initialization
//	void Start ()
//	{
//		UIStore.HideEvent += Hide;
//		UIVIP.SeeEvent += See;
//		itemButtons = GetComponentsInChildren<Button> ();
//		Debug.Log (itemButtons.Length);
//		for (int i = 0; i < itemButtons.Length; i++) 
//		{
//			EventTriggerListener.Get(itemButtons [i].gameObject).onClick = OnButton;
//		}
//		nickName.text = UIHall.nickNamestr;
//
//		OneMoreGame ();
//	}
//
//	void OneMoreGame()
//	{
//		MyInfo myInfo = DataControl.GetInstance ().GetMyInfo ();
//		if (myInfo.oneMoreGame) {
//			string path;
//			int type = myInfo.lastGame.type;
//			Debug.LogError (type+"++++");
//			switch (type) {
//			case 4:
//				UIGame.roomName = "Bullet";
//				UICombatGame.roomName = "Bullet";
//				GameObject BulletWindow = Resources.Load ("Window/BulletGameWIndow") as GameObject;
//				WindowClone = Instantiate (BulletWindow);
//				break;
//			case 5:
//				UIGame.roomName = "Time";
//				UICombatGame.roomName = "Time";
//				GameObject TimeWindow = Resources.Load ("Window/TimeGameWIndow") as GameObject;
//				WindowClone = Instantiate (TimeWindow);
//				break;
//			case 6:
//				UIGame.roomName = "Integral";
//				UICombatGame.roomName = "Integral";
//				GameObject IntegralWindow = Resources.Load ("Window/CombatGameWIndow") as GameObject;
//				WindowClone = Instantiate (IntegralWindow);
//				break;
//			}
//
//		}
//	}
//
//	void Hide()
//	{
//		transform.gameObject.SetActive (false);
//	}
//
//	void See()
//	{
//		transform.gameObject.SetActive (true);
//	}
//
//	void OnButton(GameObject go)
//	{
//		string path;
//		//go.name.Equals ("CoinButton") || go.name.Equals ("BrilliantButton") || go.name.Equals ("Mall")
//		UIGame.roomName = go.name;
//		UICombatGame.roomName = go.name;
//	    switch (go.name) {
//		case "Bullet":
//			GameObject BulletWindow = Resources.Load ("Window/BulletGameWIndow") as GameObject;
//			WindowClone = Instantiate (BulletWindow);
//			break;
//		case "Time":
//			GameObject TimeWindow = Resources.Load ("Window/TimeGameWIndow") as GameObject;
//			WindowClone = Instantiate (TimeWindow);
//			break;
//		case "Integral":
//			GameObject IntegralWindow = Resources.Load ("Window/CombatGameWIndow") as GameObject;
//			WindowClone = Instantiate (IntegralWindow);
//			break;
//		case "ExitButton":
//			AppControl.ToView (AppView.PKHALLMAIN);
//			break;
//		}
//	}
//
////	void OnGUI()
////	{
////		Tool.ShowInGUI ();
////	}
//
//	void StoreOpen()
//	{
//		string path = "Window/StoreWindow";
//		WindowClone = AppControl.OpenWindow (path);
//		WindowClone.SetActive (true);
//	}
//	// Update is called once per frame
//	void Update () {
//	   
//	}
//
//	void OnDestroy()
//	{
//		UIStore.HideEvent -= Hide;
//		UIVIP.SeeEvent -= See;
//	}
//}
