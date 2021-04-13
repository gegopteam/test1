using UnityEngine;
using System.Collections;
using UnityEngine.UI;
/// <summary>
/// Room list button.无效代码
/// </summary>

public class RoomListButton : MonoBehaviour {
	private Button[] roomButton;
	private GameObject WindowClone;
	public static Transform parent;
	public static int roomNumber;

	public static long buildTime;

	// Use this for initialization
	void Start () {
		roomButton = GetComponentsInChildren<Button> ();
		for (int i = 0; i < roomButton.Length; i++) {
			EventTriggerListener.Get (roomButton [i].gameObject).onClick = ClickCallBack;
		}
	}

	void ClickCallBack(GameObject go)
	{
		parent = go.transform.parent;
		Debug.Log (go.transform.parent.name);
		string Check = parent.Find ("Lock").Find ("PassWord").GetComponent<Text> ().text;
		if (Check == "有密码") {
			string path = "Window/TipsWindow";
			WindowClone = AppControl.OpenWindow (path);
			WindowClone.SetActive (true);
			roomNumber = int.Parse(parent.Find ("NumberLabel").Find ("Number").GetComponent<Text>().text);
			buildTime = long.Parse(parent.Find ("BuildTime").GetComponent<Text>().text);
			//parent.transform.FindChild ("Lock").Find ("PassWord").GetComponent<Text> ().text;
		} else {
			//向服务器发送请求，点击进入房间
			int roomtype = 0;
			switch (UIRoom.name) {
			case "Bullet":
				roomtype = 4;
				break;
			case "Time":
				roomtype = 5;
				break;
			case "Integral":
				roomtype = 6;
				break;
			}
			roomNumber = int.Parse(parent.Find ("NumberLabel").Find ("Number").GetComponent<Text>().text);
			buildTime = long.Parse(parent.Find ("BuildTime").GetComponent<Text>().text);
			Debug.Log ("buildTime"+buildTime);
			//UIHallMsg.GetInstance ().SndPKEnterRoomRequest (roomtype,roomNumber,buildTime,"");
			string path = "Window/ReadyWindowInRoom";
			WindowClone = AppControl.OpenWindow (path);
			WindowClone.SetActive (true);
			//进房间，判断房间最多人数和当前人数匹配。
//			string CurrentPerson = parent.transform.FindChild("PersonLabel").transform.FindChild("CurrentMuch").GetComponent<Text>().text;
//			string MaxPerson = parent.transform.FindChild("PersonLabel").transform.FindChild("PersonMuch").GetComponent<Text>().text;
//			if (CurrentPerson == MaxPerson) {
//				Debug.Log ("人数已满不能进入");
//			} else {
//				Debug.Log ("进房间");
//			}
		}
	}
	// Update is called once per frame
	void Update () {
	
	}
}
