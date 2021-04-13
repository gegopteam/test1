using UnityEngine;
using System.Collections;
using UnityEngine.UI;
/// <summary>
/// In room init.初始化房间数据，房主的房间
/// </summary>

public class UIPKFriend_RoomInfo : MonoBehaviour {
	[SerializeField]
	private Transform roomNumber;
	[SerializeField]
	private Transform coinMuch;
	[SerializeField]
	private Transform timeMuch;
	[SerializeField]
	private Transform roundMuch;


	// Use this for initialization
	void Start () {

	}

	public void SetRoomInfo( int nRoomType , int nRoomIndex )
	{
		switch (nRoomType) {
		case PKRuleType.FRIEND_ROOM_GOLD:
			coinMuch = transform.Find ("Coin");
			timeMuch = transform.Find ("Time");
			roundMuch = transform.Find ("Round");
			break;
		case PKRuleType.FRIEND_ROOM_CARD:
			roundMuch = transform.Find ("Round");
			timeMuch = transform.Find ("Time");
			break;
		}
		roomNumber = transform.Find ("Room");

		RoomIndex = nRoomIndex;
		RoomType = nRoomType;

		InitInfo ();
	}

	int RoomIndex = 0;

	int RoomType = 0;

	void InitInfo()
	{
		switch ( RoomType ) {
		case 10:
			timeMuch.Find("TimeMuch").GetComponent<Text>().text = PanelManager.infoList [1].ToString();
			roundMuch.Find("RoundMuch").GetComponent<Text>().text = PanelManager.infoList [2].ToString();
			coinMuch.Find ("CoinMuch").GetComponent<Text> ().text = PanelManager.infoList[0].ToString();
			break;
		case 11:
			roundMuch.Find("RoundMuch").GetComponent<Text>().text = PanelManager.infoList [2].ToString();
			timeMuch.Find("TimeMuch").GetComponent<Text>().text = PanelManager.infoList [1].ToString();
			break;
		}
		//服务器传过来，到时候读取数据
		roomNumber.Find ("Number").GetComponent<Text> ().text = RoomIndex.ToString();
	}

	// Update is called once per frame
	void Update () {
		//Init ();
	}
}

