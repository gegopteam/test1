using UnityEngine;
using System.Collections;
using UnityEngine.UI;
/// <summary>
/// Ready room manager.获取Panelmanager中infoList中的数据将其显示在准备房间中
/// </summary>
public class ReadyRoomManager : MonoBehaviour {
	
	private Transform roomNumber;
	private Transform roomName;
	private Transform coinMuch;
	private Transform timeMuch;
	private Transform bulletMuch;
	private Transform reward;

	public int RoomNumber;

	// Use this for initialization
	void Start () {
		
	}

	void Init()
	{
        switch (UIRoom.name) {
		case "Bullet":
			coinMuch = transform.Find ("Coin");
			bulletMuch = transform.Find ("Bullet");
			break;
		case "Time":
			coinMuch = transform.Find ("Coin");
			timeMuch = transform.Find ("Time");
			break;
		case "Integral":
			timeMuch = transform.Find ("Time");
			break;
		}
		roomNumber = transform.Find ("Room");
		roomName = transform.Find ("RoomName");
		reward = transform.Find ("Reward");
			
		InitInfo ();
	}
	void InitInfo()
	{
		switch (UIRoom.name) {
		case "Bullet":
			coinMuch.Find("CoinMuch").GetComponent<Text>().text = PanelManager.infoList[1].ToString();
			bulletMuch.Find("ButtleMuch").GetComponent<Text>().text = PanelManager.infoList[2].ToString();
			break;
		case "Time":
			coinMuch.Find("CoinMuch").GetComponent<Text>().text = PanelManager.infoList[1].ToString();
			timeMuch.Find("TimeMuch").GetComponent<Text>().text = PanelManager.infoList [5].ToString();
			break;
		case "Integral":
			timeMuch.Find ("TimeMuch").GetComponent<Text> ().text = PanelManager.infoList [5].ToString ();
			break;
		}
		roomNumber.Find ("Number").GetComponent<Text> ().text = PanelManager.infoList[7].ToString();
		roomName.Find("Name").GetComponent<Text>().text = PanelManager.infoList [0].ToString();
		reward.Find("RewardImage").GetComponentInChildren<Text>().text = PanelManager.infoList [6].ToString ();
	}

	// Update is called once per frame
	void Update () {
		Init ();
	}
}
