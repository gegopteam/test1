using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;

using AssemblyCSharp;
/// <summary>
/// User interface in room ready.自己的房间等待消息
/// </summary>

public class UIInRoomReady : MonoBehaviour, IUiMediator {
	public GameObject master;
	public GameObject[] otherPlayers;
	public GameObject coinInfo;
	public GameObject cardInfo;
	public Button startButton;
	//private int person = 0;
	//private GameObject WindowClone;

	//private static int enterPerson = 0;

	private FiPkRoomInfo mRoomInfo;

	//Dictionary< int , FiUserInfo > mEnterUsers = new Dictionary<int, FiUserInfo> ();

	public void SetRoomInfo( FiPkRoomInfo nInfo )
	{
		mRoomInfo = nInfo;
	}

	void Awake()
	{
		
	}

	// Use this for initialization
	void Start () {
		startButton.interactable = false;

		coinInfo.SetActive (false);	
		cardInfo.SetActive (false);

		ShowRoomOwner (  );

		if (mRoomInfo.roomType == 10) {
			InRoomInit nInit = transform.Find ("CoinInfo").gameObject.GetComponentInChildren< InRoomInit > ();
			nInit.RoomType = mRoomInfo.roomType;
			nInit.RoomIndex = mRoomInfo.roomIndex;
		}else if( mRoomInfo.roomType == 11 ) {
			InRoomInit nInit = transform.Find ("CardInfo").gameObject.GetComponentInChildren< InRoomInit > ();
			nInit.RoomType = mRoomInfo.roomType;
			nInit.RoomIndex = mRoomInfo.roomIndex;
		}

		Facade.GetFacade ().ui.Add ( FacadeConfig.UI_FISHING_FRIEND_ID , this );
		//UIHallObjects.GetInstance ().SetRcv (AppFun.UIHALL_INPKROOM, this);
	}

	public void OnRecvData( int nType , object data )
	{
		switch (nType) {
		case FiEventType.RECV_OTHER_ENTER_FRIEND_ROOM_INFORM: //接收其他玩家进入好友约战房间通知
			Debug.LogError ("其他玩家进入房间");
			RcvPKOtherEnterFriendRoomInform (data);
			break;
		case FiEventType.RECV_OTHER_LEAVE_FRIEND_ROOM_INFORM: //接收其他玩家离开好友约战房间通知
			Debug.LogError ("有玩家离开了房间");
			RcvPKOtherLeaveFriendRoomInform (data);
			break;
		case FiEventType.RECV_OTHER_PREPARE_PKGAME_INFORM: //接收其他玩家准备通知
			Debug.LogError("有玩家点击了准备");
			RcvPKOtherPrepareGameInform (data);
			break;
		case FiEventType.RECV_OTHER_CANCEL_PREPARE_PKGAME_INFORM: //接收其他玩家取消准备通知
			Debug.LogError("有玩家取消了准备");
			RcvPKCancelPrepareGameInform (data);
			break;
		case FiEventType.RECV_START_PK_GAME_RESPONSE:
			Debug.LogError ("房主点击了开始游戏");
			RcvStartPKGameResponse (data);
			break;
		case FiEventType.RECV_DISBAND_FRIEND_ROOM_RESPONSE://房间收到解散房间反馈
			Debug.LogError("房主解散了房间");
			RcvPKDisbandFriendRoomResponse (data);
			break;
		}
	}

	public void OnInit()
	{

	}

	public void OnRelease()
	{

	}

	public void DesRoomButton()
	{
		//UIHallMsg.GetInstance ().SndPKDisbandFriendRoomRequest (PanelManager.modelIndex, UIMade.roomIndex);
		//Debug.LogError ( "disband friend room panelManager.modelIndex ========= " + PanelManager.modelIndex + " / " + mRoomInfo.roomType );
		Facade.GetFacade ().message.fishFriend.SendPKDisbandFriendRoomRequest ( mRoomInfo.roomType, mRoomInfo.roomIndex );
	}

	public void FriendButton()
	{
		string path= "Window/InvitingFriendWindow";
		GameObject WindowClone = AppControl.OpenWindow (path);
		WindowClone.SetActive (true);
	}

	public void StartButton()
	{
		//Debug.Log ("开始游戏");
		Debug.LogError ( "StartButton" );
		//当大家都准备可以开始比赛，发送开始游戏的消息,除了房主的person==3就可以进入房间4人房
		UIHallMsg.GetInstance ().SndPKStartGameRequest (mRoomInfo.roomType, mRoomInfo.roomIndex);

	}

	// Update is called once per frame
	void Update () {
		switch (mRoomInfo.roomType) {
		case 10:
			ShowCoin ();
			break;
		case 11:
			ShowCrad ();
			break;
		}
	}

	void ShowCoin()
	{
		cardInfo.gameObject.SetActive (false);
		coinInfo.gameObject.SetActive (true);
	}

	void ShowCrad()
	{
		coinInfo.gameObject.SetActive (false);
		cardInfo.gameObject.SetActive (true);
	}

	string GetDisplayName( string nNameIn )
	{
		return Tool.GetName ( nNameIn , 6 );
	}

	//展示房间主人的信息是主人master的ui显示界面
	void ShowRoomOwner( )
	{
		Debug.Log ("显示房主信息");
		MasterReadyPlayers Master = master.GetComponent<MasterReadyPlayers>(); 
		MyInfo myInfo = DataControl.GetInstance ().GetMyInfo ();

		AvatarInfo nAVInfo = (AvatarInfo)Facade.GetFacade ().data.Get ( FacadeConfig.AVARTAR_MODULE_ID );
		nAVInfo.Load ( myInfo.userID , null , ( (int nResult, Texture2D nTexture) => {
			if (nResult == 0)
			{
				Master.headImage.sprite = Sprite.Create (nTexture, new Rect (0, 0, nTexture.width, nTexture.height), new Vector2 (0, 0));
			}
		}) );

		Master.playerName.text = GetDisplayName(myInfo.nickname);
		Master.cannonImage = null;
		Master.readyImage.gameObject.SetActive (false);

		for (int i = 0; i < otherPlayers.Length; i++) {
			MasterReadyPlayers other = otherPlayers[i].GetComponent<MasterReadyPlayers>();
			//other.show.gameObject.SetActive (false);
			other.waitImage.gameObject.SetActive (true);
		}
	}

	void OnDestroy()
	{
		Debug.LogError ( " ---------UiRoomReady start remove--------- " );
		if(  Facade.GetFacade ().ui.Get ( FacadeConfig.UI_FISHING_FRIEND_ID ).Equals( this ) )
		{
			Debug.LogError ( " ---------UiRoomReady remove success --------- " );
			Facade.GetFacade ().ui.Remove( FacadeConfig.UI_FISHING_FRIEND_ID );
		}
	}
		
	void RcvPKOtherEnterFriendRoomInform(object data)
	{
		FiOtherEnterFriendRoomInform nEnterUserInform = (FiOtherEnterFriendRoomInform)data;
		if (nEnterUserInform == null || nEnterUserInform.other == null) {
			Debug.LogError ("-----------RcvPKOtherEnterFriendRoomInform--------error  info == null ");
			return;
		}
		DisplayFriendInfo ( nEnterUserInform.other );
	}

	void DisplayFriendInfo( FiUserInfo nUserInfo  )
	{
		MasterReadyPlayers nReadyPlayer = null;
		//找到没有被使用的组件
		for (int i = 0; i < otherPlayers.Length; i++) {
			nReadyPlayer = otherPlayers [i].GetComponent<MasterReadyPlayers> ();
			if ( !nReadyPlayer.isActive )
				break;
		}

		if (nReadyPlayer == null) {
			Debug.LogError ( "------------DisplayFriendInfo error user == null-------------" );
			return;
		}

		nReadyPlayer.isActive = true;
		nReadyPlayer.waitImage.gameObject.SetActive (false);
		nReadyPlayer.show.gameObject.SetActive (true);

		AvatarInfo nAVInfo = (AvatarInfo) Facade.GetFacade ().data.Get ( FacadeConfig.AVARTAR_MODULE_ID );
		nAVInfo.Load ( nUserInfo.userId , nUserInfo.avatar , ( (int nResult, Texture2D nTexture) => {
			if (nResult == 0) {
				if (nReadyPlayer.isActive) {
					nReadyPlayer.headImage.sprite = Sprite.Create (nTexture, new Rect (0, 0, nTexture.width, nTexture.height), new Vector2 (0, 0));
				}
			}
		} ) );

		nReadyPlayer.playerName.text = GetDisplayName( nUserInfo.nickName );
		nReadyPlayer.cannonImage = null;
		nReadyPlayer.readyImage.gameObject.SetActive(false);
		nReadyPlayer.useId = nUserInfo.userId;
	}
		
    void RcvPKOtherLeaveFriendRoomInform(object data)
	{
		FiOtherLeaveFriendRoomInform nLeaveInfo = (FiOtherLeaveFriendRoomInform) data;
		Debug.LogError ("--------RcvPKOtherLeaveFriendRoomInform----------");
		for(int i = 0;i< otherPlayers.Length; i++ )
		{
			MasterReadyPlayers nReadyInfo = otherPlayers [i].GetComponent<MasterReadyPlayers> ();
			if ( nLeaveInfo.leaveUserId == nReadyInfo.useId ) {
				nReadyInfo.isActive = false;
				nReadyInfo.isPrepared = false;
				nReadyInfo.useId = 0;
				nReadyInfo.waitImage.gameObject.SetActive (true);
				break;
			}
		}
		CheckToEnableStart ();
	}


	void CheckToEnableStart()
	{
		bool bAllPrepared = true;
		int nRoomUserCount = 0;
		for(int i = 0;i<otherPlayers.Length;i++)
		{
			MasterReadyPlayers otherPlayer = otherPlayers [i].GetComponent<MasterReadyPlayers> ();
			if ( otherPlayer.isActive ) 
			{
				nRoomUserCount++;
				if (!otherPlayer.isPrepared) {
					bAllPrepared = false;
					break;
				}
			}
		}

		if (nRoomUserCount == 0) {
			bAllPrepared = false;
		}

		if (startButton.interactable != bAllPrepared) {
			startButton.interactable = bAllPrepared;
		}
	}

	void RcvPKOtherPrepareGameInform(object data)
	{
		FiPreparePKGame nPrapareInform = (FiPreparePKGame)data;
		Debug.LogError ("--------有玩家点击了准备----------");
		//person++;
		//PrepareGameInform (otherPrepare);

		for(int i = 0;i<otherPlayers.Length;i++)
		{
			MasterReadyPlayers otherPlayer = otherPlayers [i].GetComponent<MasterReadyPlayers> ();
			if ( nPrapareInform.userId == otherPlayer.useId ) {
				otherPlayer.readyImage.gameObject.SetActive (true);
				otherPlayer.readyImage.overrideSprite = Resources.Load ("Room/准备中",typeof(Sprite))as Sprite;
				otherPlayer.isPrepared = true;
				break;
			}
		}
		CheckToEnableStart ();
	}


	void RcvPKCancelPrepareGameInform(object data)
	{
		FiCancelPreparePKGame nCancelInform = (FiCancelPreparePKGame)data;
		for(int i = 0;i<otherPlayers.Length;i++)
		{
			MasterReadyPlayers otherPlayer = otherPlayers [i].GetComponent<MasterReadyPlayers> ();
			if (nCancelInform.userId == otherPlayer.useId) {
				otherPlayer.isPrepared = false;
				otherPlayer.readyImage.gameObject.SetActive (false);
				startButton.interactable = false;
				break;
			}
		}
	}

	void RcvStartPKGameResponse(object data)
	{
//		MyInfo myInfo = (MyInfo)Facade.GetFacade ().data.Get ( FacadeConfig.USERINFO_MODULE_ID );
//		myInfo.TargetView = AppView.FISHING;
//		AppControl.ToView (AppView.LOADING);
		//AppControl.ToView (AppView.FISHING);
		//enterPerson = 0;
		//FiStartPKGameResponse startRoom = (FiStartPKGameResponse)data;
		//Invoke ("JumpScene",1f);
	}

//	void JumpScene()
//	{
//		AppControl.ToView (AppView.FISHING);
//	}

	void RcvPKDisbandFriendRoomResponse(object data)
	{
		Destroy ( gameObject );
	}

}
