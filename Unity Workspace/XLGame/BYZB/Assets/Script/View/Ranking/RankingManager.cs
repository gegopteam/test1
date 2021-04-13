using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class RankingManager : MonoBehaviour {
	public Image userIcon;
	public Text nickName;
	public Text vipText;
	public Image sexImage;
	public Image coinImage;
	public Text coinMuch;
	public Image rewardImage;
	public Text rewardMuch;
	// Use this for initialization
	void Awake () {
		userIcon = transform.Find ("UserIcon").GetComponent<Image> ();
		nickName = transform.Find ("MyName").GetComponent<Text> ();
		vipText = transform.Find ("MyLevel").Find ("Text").GetComponent<Text> ();
		sexImage = transform.Find ("Sex").GetComponent<Image> ();
		coinMuch = transform.Find ("TodayCoin").Find ("CoinMuch").GetComponent<Text> ();
		rewardMuch = transform.Find ("Reward").Find ("RewardMuch").GetComponent<Text> ();
	}

	// Update is called once per frame
	void Update () {
	
	}
}
