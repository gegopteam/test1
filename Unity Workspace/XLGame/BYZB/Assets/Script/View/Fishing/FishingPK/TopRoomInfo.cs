using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class TopRoomInfo : MonoBehaviour {

	public static TopRoomInfo _instance;
	public GameObject pkName_Bullet;
	public GameObject pkName_Point;
	public GameObject pkName_Time;

	public Text roomIdValue;
	public Text countDownValue;

	// Use this for initialization
	void Start () {
		if (null == _instance)
			_instance = this;

		roomIdValue.text = DataControl.GetInstance ().GetRoomInfo ().roomIndex.ToString ();
		switch (GameController._instance.myGameType) {
		case GameType.Classical:
			this.gameObject.SetActive (false);
			break;
		case GameType.Bullet:
			pkName_Point.SetActive (false);
			pkName_Bullet.SetActive (true);
			pkName_Time.SetActive (false);
			break;
		case GameType.Time:
			pkName_Point.SetActive (false);
			pkName_Bullet.SetActive (false);
			pkName_Time.SetActive (true);
			break;
		case GameType.Point:
			pkName_Point.SetActive (true);
			pkName_Bullet.SetActive (false);
			pkName_Time.SetActive (false);
			break;
		default:
			break;
		}
	}


	public static  string ChangeTimeFormat(int seconds)
	{
		int  m, s;
		m = (int)(seconds /60);
		s = seconds % 60;
		if (s >= 10)
			return m + ":" + s;
		else
			return m + ":0" + s;
	}

	public void SetTime(int seconds)
	{
		if (this.gameObject != null) {
			countDownValue.text = ChangeTimeFormat (seconds);
		}
			
		else
			Debug.LogWarning ("CountDownValue=null");
		if (GameController._instance.isFriendMode) {
			if (seconds == 0) {
				RoundText._instance.SetShow (true);
				GameController._instance.gameIsReady = false;
				PrefabManager._instance.countDownhasInit = false;

			}
		}
	}
	// Update is called once per frame
	void Update () {
	
	}
}
