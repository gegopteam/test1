using System;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;

using AssemblyCSharp;
/// <summary>
/// User interface in room ready.自己的房间等待消息
/// </summary>

public class UIPKFriendRoom : MonoBehaviour, IUiMediator {

	public Sprite[] IconRoomType;

	public Button BtnStart;

	private FiPkRoomInfo mRoomInfo;

	public GameObject[] UserSeat;

	public Image ImgTitle;

	public Text RoomIndex;

	public Text RoundCout;

	public Text TimeCost;

	void Awake()
	{
		BtnStart.interactable = false;
	}

	// Use this for initialization
	void Start () {
		Facade.GetFacade ().ui.Add ( FacadeConfig.UI_FISHING_FRIEND_ID , this );
	}

	void SetTitle( int nRoomType )
	{
		if (nRoomType == PKRoomRuleType.FRIEND_ROOM_CARD) {
			ImgTitle.sprite = IconRoomType [1];
		} else {
			ImgTitle.sprite = IconRoomType [0];
		}
	}

	public void SetRoomOwner( FiUserInfo nOwner )
	{
		AddUserEntity ( UserSeat [0].gameObject , nOwner ,  true );
	}

	public void ShowGuestButton( bool nValue )
	{
		transform.Find ( "ContentInfo" ).Find("OwnerGroup").gameObject.SetActive( !nValue );
		transform.Find ( "ContentInfo" ).Find("FriendGroup").gameObject.SetActive( nValue );
	}

	//设置房主房间信息，表明本人就是房主
	public void SetRoomInfo( FiPkRoomInfo nInfo )
	{
		mRoomInfo = nInfo;
		SetTitle ( nInfo.roomType );
		if (nInfo.roundType == 0) {
			RoundCout.text = 1 + "局";
		} else if (nInfo.roundType == 1) {
			RoundCout.text = 3 + "局";
		}else if (nInfo.roundType == 2) {
			RoundCout.text = 5 + "局";
		}
		if (nInfo.timeType == 0) {
			TimeCost.text = 3+"分钟";
		}else if( nInfo.timeType == 1 ){
			TimeCost.text = 5+"分钟";
		}
		RoomIndex.text = nInfo.roomIndex.ToString();
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
		case FiEventType.RECV_LEAVE_FRIEND_ROOM_RESPONSE: //接收离开好友约战房间回复
			Debug.LogError ("自己退出了游戏");
			Destroy ( this.gameObject );
			break;
			//RcvPKLeaveFriendRoomResponse (data);
		case FiEventType.RECV_DISBAND_FRIEND_ROOM_RESPONSE://房间收到解散房间反馈
			Debug.LogError("房主解散了房间");
			RcvPKDisbandFriendRoomResponse (data);
			break;
		case FiEventType.RECV_DISBAND_FRIEND_ROOM_INFORM://其他玩家收到房间解散的通知
			Debug.LogError("房主解散了房间");
			RcvPKDisbandFriendRoomInform (data);
			break;
		}
	}

	void RcvPKDisbandFriendRoomInform(object data)
	{
		GameObject Window = Resources.Load ("Window/WindowTips")as GameObject;
		GameObject WindowClone = GameObject.Instantiate (Window);
		UITipClickHideManager ClickTip = WindowClone.GetComponent<UITipClickHideManager> ();
		ClickTip.SetClickCallback ( 
			()=>{
				Destroy (this.gameObject);
			}
		);
		ClickTip.text.text = "房主已解散房間!";
		ClickTip.time.text = "";
		ClickTip.tipString.text = "";
	}

	public void OnInit()
	{

	}

	public void OnRelease()
	{

	}

	public void OnDisbandRoom()
	{
		Debug.LogError ( "--------" +  mRoomInfo.roomType + " / " + mRoomInfo.roomIndex );
		Facade.GetFacade ().message.fishFriend.SendPKDisbandFriendRoomRequest ( mRoomInfo.roomType, mRoomInfo.roomIndex );
	}

	public void OnGameStart()
	{
		UIHallMsg.GetInstance ().SndPKStartGameRequest (mRoomInfo.roomType, mRoomInfo.roomIndex);
	}

	public void OnPrepareGame()
	{
		transform.Find ("ContentInfo").Find ("FriendGroup").Find ("BtnCancel").gameObject.SetActive( true ); //gameObject.SetActive( true );
		transform.Find ("ContentInfo").Find ("FriendGroup").Find ("BtnPrepare").gameObject.SetActive( false );
		MyInfo nMyInfo = (MyInfo) Facade.GetFacade ().data.Get ( FacadeConfig.USERINFO_MODULE_ID );
		Facade.GetFacade ().message.fishFriend.SendPKPrepareGameRequest( nMyInfo.userID, mRoomInfo.roomType ,mRoomInfo.roomIndex );

		for(int i = 1;i < UserSeat.Length; i++ )
		{
			if (UserSeat [i].transform.Find ("User") != null) 
			{
				UIPKFriend_UserInfo nUserInfo = UserSeat [i].transform.Find ("User").GetComponentInChildren<UIPKFriend_UserInfo> ();
				if (nUserInfo.userId == nMyInfo.userID ) {
					nUserInfo.SetReadyState ( true );
					break;
				}
			}
		}

	}

	public void OnCancelPrepare()
	{
		transform.Find ("ContentInfo").Find ("FriendGroup").Find ("BtnCancel").gameObject.SetActive( false ); //gameObject.SetActive( true );
		transform.Find ("ContentInfo").Find ("FriendGroup").Find ("BtnPrepare").gameObject.SetActive( true );
		MyInfo nMyInfo = (MyInfo) Facade.GetFacade ().data.Get ( FacadeConfig.USERINFO_MODULE_ID );
//		Facade.GetFacade ().message.fishFriend.SendPKPrepareGameRequest( nMyInfo.userID, mRoomInfo.roomType ,mRoomInfo.roomIndex );
		UIHallMsg.GetInstance ().SndPKCancelPrepareGame (nMyInfo.userID, mRoomInfo.roomType ,mRoomInfo.roomIndex  );
		for(int i = 1;i < UserSeat.Length; i++ )
		{
			if (UserSeat [i].transform.Find ("User") != null) 
			{
				UIPKFriend_UserInfo nUserInfo = UserSeat [i].transform.Find ("User").GetComponentInChildren<UIPKFriend_UserInfo> ();
				if (nUserInfo.userId == nMyInfo.userID ) {
					nUserInfo.SetReadyState ( false );
					break;
				}
			}
		}
	}

	public void OnExitRoom()
	{
		Facade.GetFacade ().message.fishFriend.SendPKLeaveFriendRoomRequest ( mRoomInfo.roomType, mRoomInfo.roomIndex );
	}


	// Update is called once per frame
	void Update () {

	}

	string GetDisplayName( string nNameIn )
	{
		return Tool.GetName ( nNameIn , 6 );
	}

	GameObject GetEmptySeat()
	{
		for( int i = 1 ; i <= 3 ; i ++ )
		{
			if (UserSeat [i].transform.Find ("User") == null) {
				return (UserSeat [i] );
			}
		}
		return null;
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
		AddUserEntity ( GetEmptySeat() , nEnterUserInform.other , false );
	}

	public void AppendUser( FiUserInfo userInfo )
	{
		AddUserEntity ( GetEmptySeat() , userInfo , false , userInfo.prepared );
	}

	void AddUserEntity( GameObject nSeat , FiUserInfo userInfo , bool isOwner , bool nReady = false )
	{
		GameObject nUser = Resources.Load ("PkHall/PkUserUnit") as GameObject;
		nSeat.transform.Find ("Waiting").gameObject.SetActive( false );
		GameObject mUserEntity = Instantiate ( nUser );
		mUserEntity.name = "User";
		mUserEntity.transform.SetParent ( nSeat.transform );
		mUserEntity.transform.localPosition = new Vector3 (0, 0, 0);
		mUserEntity.transform.localScale = new Vector3 ( 1 , 1 , 1);
		UIPKFriend_UserInfo nUserInfo = mUserEntity.GetComponentInChildren< UIPKFriend_UserInfo > ();
		nUserInfo.SetUserInfo ( userInfo );
		nUserInfo.SetReadyState ( nReady );
		if( isOwner )
		    nUserInfo.SetOwner ();
	}

	void RcvPKOtherLeaveFriendRoomInform(object data)
	{
		FiOtherLeaveFriendRoomInform nLeaveInfo = (FiOtherLeaveFriendRoomInform) data;
		Debug.LogError ("--------RcvPKOtherLeaveFriendRoomInform----------");
		for(int i = 1 ; i< UserSeat.Length; i++ )
		{
			if (UserSeat [i].transform.Find ("User") != null) 
			{
				UIPKFriend_UserInfo nUserInfo = UserSeat [i].transform.Find ("User").GetComponentInChildren<UIPKFriend_UserInfo> ();
				if ( nLeaveInfo.leaveUserId == nUserInfo.userId ) {
					nUserInfo.gameObject.transform.parent = null;
					Destroy ( nUserInfo.gameObject );
					UserSeat [i].transform.Find ("Waiting").gameObject.SetActive( true );
					break;
				}
			}
		}
		CheckToEnableStart ();
	}


	void CheckToEnableStart()
	{
		bool bAllPrepared = true;
		int nRoomUserCount = 0;

		for(int i = 1;i < UserSeat.Length; i++ )
		{
			if (UserSeat [i].transform.Find ("User") != null) 
			{
				UIPKFriend_UserInfo nUserInfo = UserSeat [i].transform.Find ("User").GetComponentInChildren<UIPKFriend_UserInfo> ();
				if (!nUserInfo.isReady ()) {
					bAllPrepared = false;
					break;
				} else {
					nRoomUserCount ++;
				}
			}
		}

		if ( nRoomUserCount == 0 ) {
			bAllPrepared = false;
		}

		if (BtnStart.interactable != bAllPrepared) {
			BtnStart.interactable = bAllPrepared;
		}
	}

	void RcvPKOtherPrepareGameInform(object data)
	{
		FiPreparePKGame nPrapareInform = (FiPreparePKGame)data;
		for(int i = 1;i < UserSeat.Length; i++ )
		{
			if (UserSeat [i].transform.Find ("User") != null) 
			{
				UIPKFriend_UserInfo nUserInfo = UserSeat [i].transform.Find ("User").GetComponentInChildren<UIPKFriend_UserInfo> ();
				if (nUserInfo.userId == nPrapareInform.userId) {
					nUserInfo.SetReadyState ( true );
					break;
				}
			}
		}
		CheckToEnableStart ();
	}


	void RcvPKCancelPrepareGameInform(object data)
	{
		FiCancelPreparePKGame nCancelInform = (FiCancelPreparePKGame)data;

		for(int i = 1;i < UserSeat.Length; i++ )
		{
			if (UserSeat [i].transform.Find ("User") != null) 
			{
				UIPKFriend_UserInfo nUserInfo = UserSeat [i].transform.Find ("User").GetComponentInChildren<UIPKFriend_UserInfo> ();
				if (nUserInfo.userId == nCancelInform.userId) {
					nUserInfo.SetReadyState ( false );
					BtnStart.interactable = false;
					break;
				}
			}
		}
	}

	//owner
	void RcvPKDisbandFriendRoomResponse(object data)
	{
		Destroy ( gameObject );
	}

	//玩家模式：离开好友房
	public void OnLeaveFriendRoom()
	{
		
	}

}


