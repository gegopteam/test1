using UnityEngine;
using System.Collections;
using System .Collections.Generic;
/// <summary>
/// Room sort.房间显示单例，直接调用其中的方法
/// </summary>
public class RoomSort{
	private UIRoom uiRoom;
	private static RoomSort instance;
	public static RoomSort GetInstance()
	{
		if (instance == null) {
		
			instance = new RoomSort ();
		}
		return instance;
	}

//	void OnGUI()
//	{
//		Tool.ShowInGUI ();
//	}
	//	roonName
	//	roomCoin
	//	roomBullet
	//	roomMaxPerson
	//	password
	//	roomTime
	//  roomCurrentPerson
	//  roomRound
	//  roomNumber
	public void ShowRoom(Dictionary<int ,List<string>> roomList)
	{
		Debug.Log (roomList.Count);
		Tool.Log ("进房间时显示房间数量");
		for (int index = roomList.Count; index < RoomManager.roomMessage.Length; index++) {
			RoomManager.roomMessage [index].gameObject.SetActive (false);
		}
		Tool.Log ("显示房间列表");
		Debug.Log (RoomManager.roomMessage.Length);//roomList.Count
		for (int i = 0; i < roomList.Count; i++) {
				switch (UIRoom.name) {
				case "Bullet":
					RoomManager.roomMessage [i].roomCoin.text = roomList [i] [1].ToString ();
					RoomManager.roomMessage [i].roomBullet.text = roomList [i] [2].ToString ();
					RoomManager.roomMessage [i].roomCurrentPerson.text = roomList [i] [6].ToString ();
					RoomManager.roomMessage [i].roomMaxperson.text = roomList [i] [3].ToString ();
					Tool.Log (RoomManager.roomMessage [i].roomCoin.text);
					break;
				case "Time":
					RoomManager.roomMessage [i].roomCoin.text = roomList [i] [1].ToString ();
					RoomManager.roomMessage [i].roomTime.text = roomList [i] [5].ToString ();
					RoomManager.roomMessage [i].roomCurrentPerson.text = roomList [i] [6].ToString ();
					RoomManager.roomMessage [i].roomMaxperson.text = roomList [i] [3].ToString ();
					break;
				case "Integral":
					RoomManager.roomMessage [i].roomRound.text = roomList [i] [7].ToString ();
					break;
				}
				RoomManager.roomMessage [i].roomPassWord.text = roomList [i] [4].ToString ();
				RoomManager.roomMessage [i].roomNumber.text = roomList [i] [8].ToString ();
				RoomManager.roomMessage [i].roomName.text = roomList [i] [0].ToString ();
			    RoomManager.roomMessage [i].rewardMuch.text = roomList [i] [9].ToString ();
			    RoomManager.roomMessage [i].roomBulidTime.text = roomList [i] [10].ToString();
				//Lock,密码不为空，则上锁
				if (roomList [i] [4].ToString () != "无") {
					RoomManager.roomMessage [i].roomLock.overrideSprite = Resources.Load ("Room/锁", typeof(Sprite))as Sprite;
				} else {
					RoomManager.roomMessage [i].roomLock.gameObject.SetActive (false);
				}
			//Debug.Log ("RoomManager.roomMessage [i].roomCurrentPerson" + RoomManager.roomMessage [i].roomCurrentPerson.text);
			//Debug.Log ("RoomManager.roomMessage [i].roomMaxperson" + RoomManager.roomMessage [i].roomMaxperson.text);
			if (RoomManager.roomMessage [i].roomCurrentPerson.text == RoomManager.roomMessage [i].roomMaxperson.text) {
					RoomManager.roomMessage [i].roomState.overrideSprite = Resources.Load ("Room/游戏中", typeof(Sprite))as Sprite;
				}
			}
       }
}
