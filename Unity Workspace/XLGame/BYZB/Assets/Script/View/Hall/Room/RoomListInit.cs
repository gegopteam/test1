using UnityEngine;
using System.Collections;
using System .Collections.Generic;
using AssemblyCSharp;
/// <summary>
/// Room list init.初始化房间列表信息，更新房间时调用RoomSort中的ShowRoom,需要写一个请求更新数据的接口，在点击刷新的时候调用，在一定的时间间隔内调用
/// </summary>
public class RoomListInit : MonoBehaviour,IData {
	//获取一个保存一个
	//按顺序存储
	private string roomName;
	private string roomCoin;
	private string roomBullet;
	private string roomMaxPerson;
	private string roomPassword;
	private string roomTime;
	private string roomCurrentPerson;
	private string roomRound;
	private string roomNumber;
	private string roomReward;

	public static int index = -1;

	private DataControl dataControl = null;
	private RoomInfoListPK roomInfo;
	private FiPkRoomInfo pkRoom;
	private int room;

	private  List<string> roomList = new List<string>();//保存一个房间的数据
	//存储所有房间
	private Dictionary<int ,List<string>> roomDic = new Dictionary<int , List<string>> ();//Key值one。。，value：roomList

	//public GameObject NothingRoom;

	// Use this for initialization
	void Start () {
		dataControl = DataControl.GetInstance ();
		RenewList();
	}

//	void OnGUI()
//	{
//		Tool.ShowInGUI ();
//	}
	//刷新的接口
	public void RenewList()
	{
		Debug.Log("初始化房间");
		//调用RoomSort中的ShowRoom
		switch (UIRoom.name) {
		case"Bullet":
			room = 4;
			if (dataControl.GetRoomInfoListPK (RoomInfoListPK.TYPE_BULLET) == null)
				Debug.Log ("+++++++");
			roomInfo = dataControl.GetRoomInfoListPK(RoomInfoListPK.TYPE_BULLET);
			break;
		case "Time":
			room = 5;
			roomInfo = dataControl.GetRoomInfoListPK(RoomInfoListPK.TYPE_TIME);
			break;
		case "Integral":
			room = 6;
			roomInfo = dataControl.GetRoomInfoListPK(RoomInfoListPK.TYPE_POINT);
			break;
		}
		index = -1;
		if (roomInfo == null) {
		
			Tool.Log ("roominfo is null");
		}
		Debug.Log("初始化房间 111");
		roomInfo.SetRcv (this);
	//	UIHallMsg.GetInstance().SndGetPKRoomsRequest(room,0);
	}

	public void RcvInfo (object data)
	{
		int coin =0 ;
		Debug.LogError("传数据");
		if (data == null) {
			RoomSort.GetInstance ().ShowRoom (roomDic);
			return;
		}

		pkRoom = (FiPkRoomInfo)data;

		switch (pkRoom.goldType) {
		case 0:
			roomCoin = "1萬";
			coin = 10000;
			break;
		case 1:
			roomCoin = "10萬";
			coin = 100000;
			break;
		case 2:
			roomCoin = "100萬";
			coin = 1000000;
			break;
		}
//		switch(pkRoom.bulletType)
//		{
//		case 0:
//			roomBullet = "400";
//			break;
//		case 1:
//			roomBullet = "800";
//			break;
//		}
//		switch(pkRoom.playerNumType)
//		{
//		case 0:
//			roomMaxPerson = "2";
//			break;
//		case 1:
//			roomMaxPerson = "3";
//			break;
//		case 2:
//			roomMaxPerson = "4";
//			break;
//		}
//		if (pkRoom.hasPassword) {
//			roomPassword = "有密码";
//		} else {
//			roomPassword = "无";
//		}
		switch(pkRoom.timeType)
		{
		case 0:
			if (UIRoom.name.Equals ("Time")) {
				roomTime = "3分鐘";
			} else {
				roomTime = "5分鐘";
			}
			break;
		case 1:
			if (UIRoom.name.Equals ("Time")) {
				roomTime = "5分鐘";
			}else
			{
				roomTime = "10分鐘";
			}
			break;
		}
		roomCurrentPerson = pkRoom.currentPlayerCount.ToString ();
//		switch (pkRoom.pointType) {
//
//		case 0:
//			roomRound = "1局";
//			break;
//		case 1:
//			roomRound = "3局";
//			break;
//		case 2:
//			roomRound = "4局";
//			break;
//		}
//		roomNumber = pkRoom.roomIndex.ToString ();
//		roomName = pkRoom.roomName;
//		int person = int.Parse(roomMaxPerson);
//		roomReward = (coin * person * 0.9f).ToString();
//		roomList = new List<string> ();
//		roomList.Add (roomName);
//		roomList.Add (roomCoin);
//		roomList.Add (roomBullet);
//		roomList.Add (roomMaxPerson);
//		roomList.Add (roomPassword);
//		roomList.Add (roomTime);
//		roomList.Add (roomCurrentPerson);
//		roomList.Add (roomRound);
//		roomList.Add (roomNumber);
//		roomList.Add (roomReward);
//		Debug.LogError (pkRoom.createTime);
//		roomList.Add (pkRoom.createTime.ToString());
		//Debug.LogError ("房间号"+roomNumber);
		//Debug.LogError ("房间人数"+roomMaxPerson);

		Debug.Log (index);
//		if (roomDic.ContainsKey (index)) {
//			Debug.Log (index);
//		} else {
			roomDic.Add (++index, roomList);
		//}
	    Tool.Log ("執行進入房間列表腳本");

	}
	//	roonName
	//	roomCoin
	//	roomBullet
	//	roomMaxPerson
	//	password
	//	roomTime
	//  roomCurrentPerson
	//  roomRound
	//  roomNumber
	// Update is called once per frame
	void Update () {
	
	}
}
