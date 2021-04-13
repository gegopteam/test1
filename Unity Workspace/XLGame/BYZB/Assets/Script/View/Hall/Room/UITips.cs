using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class UITips : MonoBehaviour {
	private InputField PassWord;

	public GameObject passWrodWrong;

	// Use this for initialization
	void Start () {
		DontDestroyOnLoad (transform.gameObject);
		PassWord = GetComponentInChildren<InputField> ();
		passWrodWrong.SetActive (false);
	}

	public void ExitButton()
	{
		transform.gameObject.SetActive (false);
	}

	public void SureButton()
	{	
		int roomType = 0;
		switch (UIRoom.name) {
		case "Bullet":
			roomType = 4;
			break;
		case "Time":
			roomType = 5;
			break;
		case "Integral":
			roomType = 6;
			break;
		}
		//UIHallMsg.GetInstance ().SndPKEnterRoomRequest (roomType, RoomListButton.roomNumber,RoomListButton.buildTime, PassWord.text);
//进入房间,向服务器发送请求，把密码传过去匹配
//		if (PassWord.text) {
//		} else {
//			passWrodWrong.SetActive (true);
//			Invoke ("HideTips", 2f);
//		}
		//如果通过，打开准备房间
	}

	public void CanCelButton()
	{
		PassWord.text = null;
	}

	void HideTips()
	{
		passWrodWrong.SetActive (false);
	}

	// Update is called once per frame
	void Update () {
	
	}
}
