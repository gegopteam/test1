using UnityEngine;
using System.Collections;
using UnityEngine.UI;
/// <summary>
/// Room message.获取12个房间的text以及image，为RoomSort做排序准备
/// </summary>
public class RoomMessage : MonoBehaviour {
	public Text roomNumber;
	public  Text  roomName;
	public  Image roomState;
	public  Text roomCoin;
	public  Text roomBullet;
	public  Text roomTime;
	public  Image rewardImage;
	public  Text rewardMuch;
	public  Image roomLock;
	public  Text roomRound;
	public  Text roomCurrentPerson;
	public Text roomMaxperson;
	public Text roomPassWord;
	public Text roomBulidTime;
	// Use this for initialization
	void Start () {
		switch (UIRoom.name) {

		case "Bullet":
			roomCoin = transform.Find ("CoinLabel").transform.Find ("CoinMuch").GetComponent<Text> ();
			roomCurrentPerson = transform.Find ("PersonLabel").transform.Find ("CurrentMuch").GetComponent<Text> ();
			roomMaxperson = transform.Find ("PersonLabel").transform.Find ("PersonMuch").GetComponent<Text> ();
			rewardImage  = transform.Find ("Reward").transform.Find ("RewardImage").GetComponent<Image> ();
			rewardMuch  = transform.Find ("Reward").transform.Find ("RewardMuch").GetComponent<Text> ();
			roomBullet  = transform.Find ("BulletLabel").transform.Find ("BulletMuch").GetComponent<Text> ();
			break;
		case "Time":
			roomCoin  = transform.Find ("CoinLabel").transform.Find ("CoinMuch").GetComponent<Text> ();
			roomCurrentPerson = transform.Find ("PersonLabel").transform.Find ("CurrentMuch").GetComponent<Text> ();
			roomMaxperson = transform.Find ("PersonLabel").transform.Find ("PersonMuch").GetComponent<Text> ();
			roomTime  = transform.Find ("TimeLabel").transform.Find ("TimeMuch").GetComponent<Text> ();
			rewardImage  = transform.Find ("Reward").transform.Find ("RewardImage").GetComponent<Image> ();
			rewardMuch  = transform.Find ("Reward").transform.Find ("RewardMuch").GetComponent<Text> ();
			break;
		case "Integral":
			roomTime  = transform.Find ("TimeLabel").transform.Find ("TimeMuch").GetComponent<Text> ();
			roomRound = transform.Find ("RoundLabel").transform.Find ("RoundMuch").GetComponent<Text> ();
			break;
		}
		roomBulidTime = transform.Find ("BuildTime").GetComponent<Text> ();
		roomNumber = transform.Find ("NumberLabel").transform.Find ("Number").GetComponent<Text>();
		roomName = transform.Find ("Name").GetComponent<Text>();
		roomState  = transform.Find ("State").GetComponent<Image>();
		roomLock = transform.Find ("Lock").transform.GetComponent<Image>();
		roomPassWord = transform.Find ("Lock").transform.Find ("PassWord").GetComponent<Text> ();; 
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
