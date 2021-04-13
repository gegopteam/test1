using UnityEngine;
using System.Collections;
using UnityEngine.UI;
/// <summary>
/// In room init other.初始化房间的数据，其他人的房间
/// </summary>

public class InRoomInitOther : MonoBehaviour {
	private Transform roomNumber;
	private Transform coinMuch;
	private Transform timeMuch;
	private Transform roundMuch;

	private string coin;
	private string time;
	private string round;

	// Use this for initialization
	void Start () {

	}

	void Init()
	{
		switch (UIFind.modelIndex) {
		case 10:
			coinMuch = transform.Find ("Coin");
			timeMuch = transform.Find ("Time");
			roundMuch = transform.Find ("Round");
			break;
		case 11:
			roundMuch = transform.Find ("Round");
			timeMuch = transform.Find ("Time");
			break;
		}
		roomNumber = transform.Find ("Room");

		switch (UIFind.goldType) {
		case 1:
			coin = "1萬";
			break;
		case 2:
			coin = "10萬";
			break;
		case 3:
			coin = "100萬";
			break;
		}

		switch (UIFind.roomTime) {
		case 0:
			time = "3分鐘";
			break;
		case 1:
			time = "5分鐘";
			break;
		}

		switch(UIFind.roomRound)
		{
		case 0:
			round = "1局";
			break;
		case 1:
			round = "3局";
			break;
		case 2:
			round = "5局";
			break;
		}
		InitInfo ();
	}
	void InitInfo()
	{
		//数据从服务器过来
		switch (UIFind.modelIndex) {
		case 10:
			coinMuch.Find ("CoinMuch").GetComponent<Text> ().text = coin;
			timeMuch.Find ("TimeMuch").GetComponent<Text> ().text = time;
			roundMuch.Find ("RoundMuch").GetComponent<Text> ().text = round;
			break;
		case 11:
			roundMuch.Find ("RoundMuch").GetComponent<Text> ().text = round;
			timeMuch.Find ("TimeMuch").GetComponent<Text> ().text = time;
			break;
		}
		//服务器传过来，到时候读取数据
		roomNumber.Find ("Number").GetComponent<Text> ().text = UIFind.roomIndex.ToString();
	}

	// Update is called once per frame
	void Update () {
		Init ();
	}
}
