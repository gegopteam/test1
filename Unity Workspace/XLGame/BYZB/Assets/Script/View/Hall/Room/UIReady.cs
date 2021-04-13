//using UnityEngine;
//using System.Collections;
//using System.Collections.Generic;
//using UnityEngine.UI;
//
//using AssemblyCSharp;
//
///// <summary>
///// User interface ready.
///// 准备房间数据初始化以及创建房间的数据显示
///// </summary>
//public class UIReady : MonoBehaviour,IDispatch {
//	
//	public GameObject Buttle;
//	public GameObject Time;
//	public GameObject Intergral;
//	public List<GameObject> players = new List<GameObject>();
//	public GameObject coinGame;
//	public GameObject intrgralGame;
//	public Button[] startButton;
//
//	private static  int person = 0;
//	private static int truePerson = 0;
//	private int count;
//
//	// Use this for initialization
//	void Start () {
//		for (int i = 0; i < startButton.Length; i++) {
//
//			startButton [i].interactable = true;
//		}
//		DontDestroyOnLoad (transform.gameObject);
//		Buttle.SetActive (false);	
//		Time.SetActive (false);
//		Intergral.SetActive (false);
//
//		coinGame.SetActive (false);
//		intrgralGame.SetActive (false);
//
//		Debug.LogError ("UIReady AppFun.UIHALL_INPKROOM");
//		UIHallObjects.GetInstance ().SetRcv (AppFun.UIHALL_INPKROOM, this);
//	}
//
//	public void OnRcv(int type, object data)
//	{
//		switch(type)
//		{
//		case FiEventType.RECV_OTHER_ENTER_PK_ROOM_INFORM: //其他人进入PK准备房间通知
//			RcvPKOtherEnterRoomInform (data);
//			break;
//		case FiEventType.RECV_LEAVE_PK_ROOM_RESPONSE: //离开PK准备房间消息回复
//			RcvPKLeaveRoomResponse (data);
//			break;
//		case FiEventType.RECV_OTHER_LEAVE_PK_ROOM_INFORM: //其他人离开PK准备房间消息通知
//			RcvPKOtherLeaveRoomInform (data);
//			break;
//		case FiEventType.RECV_START_PK_GAME_RESPONSE: //接收房主开始PK游戏消息
//			RcvPKStartGameResponse (data);
//			break;
//		case FiEventType.RECV_OTHER_CANCEL_PREPARE_PKGAME_INFORM: //接收其他玩家取消准备通知
//			RcvPKCancelPrepareGameInform (data);
//			break;
//		}
//	}
//
//	void RcvPKEnterRoomResponse(object data)
//	{
//		FiEnterPKRoomResponse enterRoom = (FiEnterPKRoomResponse) data;
//	}
//
//	void RcvPKOtherEnterRoomInform(object data)
//	{
//		FiOtherEnterPKRoomInform enterRoom = (FiOtherEnterPKRoomInform) data;
//		Debug.Log("有人进来");
//		person++;
//		OtherPlayers (enterRoom);
//
//		OpenStartButton ();
//	}
//
//	void RcvPKLeaveRoomResponse(object data)
//	{
//		FiLeavePKRoomResponse enterRoom = (FiLeavePKRoomResponse) data;
//		Debug.Log ("房主离开房间");
//	}
//
//	void RcvPKOtherLeaveRoomInform(object data)
//	{
//		FiOtherLeavePKRoomInform enterRoom = (FiOtherLeavePKRoomInform) data;
//		Debug.Log ("有人退出了房间");
//		person--;
//		SomeBodyExitRoom (enterRoom);
//
//	}
//
//	void RcvPKStartGameResponse(object data)
//	{
//		FiStartPKGameResponse startGame = (FiStartPKGameResponse)data;
//		if (startGame.result == 0) {
//			AppControl.ToView (AppView.FISHING);
//			transform.gameObject.SetActive (false);
//		}else
//		{
//			Debug.Log("startGame.result"+startGame.result);
//		}
//	}
//
//	void RcvPKCancelPrepareGameInform(object data)
//	{
//		FiCancelPreparePKGame prepareGame = (FiCancelPreparePKGame)data;
//		truePerson--;
//		Debug.Log ("有玩家取消准备");
//		OtherCancleprepareGame (prepareGame);
//	}
//
//	void OtherCancleprepareGame(FiCancelPreparePKGame prepare)
//	{
//		for (int i = 1; i <= person; i++) {
//			MasterReadyPlayers otherPlayer = players [i].GetComponent<MasterReadyPlayers> ();
//			if (otherPlayer.useId == prepare.userId) {
//
//				otherPlayer.readyImage.gameObject.SetActive (false);
//			}
//		}
//	}
//		
//	void SomeBodyExitRoom(FiOtherLeavePKRoomInform exitRoomInfo)
//	{
//		for (int i = 1; i < person+1; i++) {
//			MasterReadyPlayers otherPlayer = players [i].GetComponent<MasterReadyPlayers> ();
//			if (otherPlayer.useId == exitRoomInfo.leaveUserId) {
//				otherPlayer.show.gameObject.SetActive (false);
//				otherPlayer.waitImage.gameObject.SetActive (true);
//			}
//		}
//	}
//
//	void OtherPlayers(FiOtherEnterPKRoomInform otherPlayerInfo)
//	{
//		for (int i = 1; i <= person; i++) {
//			MasterReadyPlayers otherPlayer = players [i].GetComponent<MasterReadyPlayers> ();
//			otherPlayer.waitImage.gameObject.SetActive (false);
//			otherPlayer.show.gameObject.SetActive (true);
//			otherPlayer.headImage = null;
//			otherPlayer.playerName.text = otherPlayerInfo.other.nickName;
//			otherPlayer.cannonImage = null;
//			otherPlayer.readyImage.gameObject.SetActive(false);
//			otherPlayer.useId = otherPlayerInfo.other.userId;
//		}
//	}
//
//	public void ExitButton()
//	{
//		transform.gameObject.SetActive (false);
//		//向服务发送房主退出，随机分配房主
//		//需要协议，其他玩家获取房主离开房间，房主创建的等待房间直接关闭
//		int roomtype = 0;
//		switch (UIRoom.name) {
//		case "Bullet":
//			roomtype = 4;
//			break;
//		case "Time":
//			roomtype = 5;
//			break;
//		case "Integral":
//			roomtype = 6;
//			break;
//		}
////		UIHallMsg.GetInstance ().SndPKLeaveRoomRequest (roomtype,int.Parse (PanelManager.infoList [8].ToString ()));
//	}
//
//	void OpenStartButton()
//	{
//		Debug.LogError ("房间人数"+person);
//		Debug.LogError ("Count"+count);
//		if (person == count - 1) {
//			//开始图标变亮；
//			for (int i = 0; i < startButton.Length; i++) {
//				startButton [i].interactable = true;
//			}
//		}
//	     else {
//		    //开始图标始终为暗的 ；
//		    for (int i = 0; i < startButton.Length; i++) {
//			    startButton [i].interactable = false;
//		    }
//	    }
//    }
//
//	public void StartButton()
//	{
//		Debug.Log ("开始按钮");
//		int roomType = 0;
//		//进来的人数等于Count
//			switch (UIRoom.name) {
//			case "Bullet":
//				roomType = 4;
//				break;
//			case "Time":
//				roomType = 5;
//				break;
//			case "Integral":
//				roomType = 6;
//				break;
//			}
//			UIHallMsg.GetInstance ().SndPKStartGameRequest (roomType, int.Parse (PanelManager.infoList [7].ToString ()));
//
//	}
//
//	// Update is called once per frame
//	void Update () {
//		switch(UIRoom.name)
//		{
//		case "Bullet":
//			ShowButtle ();
//			ShowPlayer ();
//			break;
//		case "Time":
//			ShowTime ();
//			ShowPlayer ();
//			break;
//		case "Integral":
//			ShowIntegral ();
//			players [0].SetActive (true);
//			players [1].SetActive (true);
//			break;
//		}
//	}
//	void ShowButtle()
//	{
//		Buttle.SetActive (true);	
//		Time.SetActive (false);
//		Intergral.SetActive (false);
//
//		coinGame.SetActive (true);
//		intrgralGame.SetActive (false);
//	}
//
//	void ShowTime()
//	{
//		Buttle.SetActive (false);	
//		Time.SetActive (true);
//		Intergral.SetActive (false);
//
//		coinGame.SetActive (true);
//		intrgralGame.SetActive (false);
//	}
//
//	void ShowIntegral()
//	{
//		Buttle.SetActive (false);	
//		Time.SetActive (false);
//		Intergral.SetActive (true);
//
//		coinGame.SetActive (false);
//		intrgralGame.SetActive (true);
//	}
//
//	void ShowPlayer()
//	{
//		for (int i = 0; i < players.Count; i++) {
//
//			players [i].SetActive (false);
//		}
//		count = 2;
//		switch (PanelManager.infoList [3].ToString ()) {
//
//		case "4人":
//			count = 4;
//			break;
//		case "3人":
//			count = 3;
//			break;
//		}
//		for (int i = 0; i <count; i++) {
//			players [i].SetActive (true);
//		}
//	}
//	//展示创建这个房间主人的信息players[0]是主人的ui显示界面
//	void ShowMaster()
//	{
//		MyInfo myInfo = DataControl.GetInstance ().GetMyInfo ();
//		MasterReadyPlayers masterPlayer = players [0].GetComponent<MasterReadyPlayers> ();
//		masterPlayer.waitImage.gameObject.SetActive (false);
//		masterPlayer.headImage = null;
//		masterPlayer.playerName.text = myInfo.nickname;
//		masterPlayer.cannonImage = null;
//		masterPlayer.readyImage.gameObject.SetActive (false);
//		for (int i = 1; i < count; i++) {
//		    MasterReadyPlayers otherPlayer = players [i].GetComponent<MasterReadyPlayers> ();
//			otherPlayer.show.gameObject.SetActive (false);
//			otherPlayer.waitImage.gameObject.SetActive (true);
//		}
//	}
//
//	void OnDestroy()
//	{
//		
//	}
//}
