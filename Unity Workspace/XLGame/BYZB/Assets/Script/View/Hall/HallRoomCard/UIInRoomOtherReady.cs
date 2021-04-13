using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using AssemblyCSharp;
/// <summary>
/// User interface in room other ready.其他人的房间等待消息
/// </summary>

public class UIInRoomOtherReady : MonoBehaviour, IUiMediator {

	public Sprite IconReady;

	public GameObject master;
	public GameObject[] otherPlayers;
	public GameObject coinInfo;
	public GameObject cardInfo;

	public Button readyButton;
	public Button cancelButton;

	private FiPkRoomInfo mRoomInfo;

	private List<FiUserInfo> mUserList;

	private int mOwnerId = 0;

	void Awake()
	{
		
	}
		
	// Use this for initialization
	void Start () {
		coinInfo.SetActive (false);	
		cardInfo.SetActive (false);
		readyButton.gameObject.SetActive (true);
		cancelButton.gameObject.SetActive (false);
		Facade.GetFacade ().ui.Add ( FacadeConfig.UI_FISHING_FRIEND_ID , this );
		ShowInfo (  );
	}

	public void OnRecvData( int nType , object data )
	{
		switch (nType) {
			case FiEventType.RECV_OTHER_ENTER_FRIEND_ROOM_INFORM: //接收其他玩家進入好友約戰房間通知
				Debug.LogError("其他玩家進入房間");
				RcvPKOtherEnterFriendRoomInform(data);
				break;
			case FiEventType.RECV_LEAVE_FRIEND_ROOM_RESPONSE: //接收離開好友約戰房間回复
				Debug.LogError("自己退出了遊戲");
				RcvPKLeaveFriendRoomResponse(data);
				break;
			case FiEventType.RECV_OTHER_LEAVE_FRIEND_ROOM_INFORM: //接收其他玩家離開好友約戰房間通知
				Debug.LogError("其他玩家離開了房間");
				RcvPKOtherLeaveFriendRoomInform(data);
				break;
			case FiEventType.RECV_OTHER_PREPARE_PKGAME_INFORM: //接收其他玩家準備通知
				Debug.LogError("其他玩家點擊了準備");
				RcvPKOtherPrepareGameInform(data);
				break;
			case FiEventType.RECV_OTHER_CANCEL_PREPARE_PKGAME_INFORM: //接收其他玩家取消準備通知
				Debug.LogError("其他玩家取消了準備");
				RcvPKCancelPrepareGameInform(data);
				break;
			case FiEventType.RECV_START_PK_GAME_INFORM://接收遊戲開始
				Debug.LogError("房主點擊開始遊戲");
				RcvStartPkGmaeInform(data);
				break;
			case FiEventType.RECV_DISBAND_FRIEND_ROOM_INFORM://其他玩家收到房間解散的通知
				Debug.LogError("房主解散了房間");
				RcvPKDisbandFriendRoomInform(data);
				break;

		}
	}

	public void OnInit()
	{

	}

	public void OnRelease()
	{

	}

	public void SetRoomInfo( int nOwnerId , FiPkRoomInfo nRoomInfo ,  List<FiUserInfo>  nUsers )
	{
		mUserList = nUsers;
		mOwnerId = nOwnerId;
		mRoomInfo = nRoomInfo;
	}

	public void ExitButton()
	{
		readyButton.gameObject.SetActive (true);
		cancelButton.gameObject.SetActive (false);
		Facade.GetFacade ().message.fishFriend.SendPKLeaveFriendRoomRequest ( mRoomInfo.roomType, mRoomInfo.roomIndex );
		Debug.LogError ("--------ExitButton-------");
	}

	public void ReadyButton()
	{
		readyButton.gameObject.SetActive (false);
		cancelButton.gameObject.SetActive (true);

		MyInfo nUserInfo = (MyInfo) Facade.GetFacade ().data.Get ( FacadeConfig.USERINFO_MODULE_ID );
		Facade.GetFacade ().message.fishFriend.SendPKPrepareGameRequest( nUserInfo.userID, mRoomInfo.roomType ,mRoomInfo.roomIndex );

		for (int i = 0; i < otherPlayers.Length; i++) 
		{
			MasterReadyPlayers otherPlayer = otherPlayers [i].GetComponent<MasterReadyPlayers> ();
			if ( nUserInfo.userID == otherPlayer.useId ) {
				otherPlayer.readyImage.gameObject.SetActive (true);
				otherPlayer.readyImage.overrideSprite = IconReady;//;Resources.Load ("Room/准备中", typeof(Sprite))as Sprite;
				break;
			}
		}
	}

	public void CancelButton()
	{
		readyButton.gameObject.SetActive (true);
		cancelButton.gameObject.SetActive (false);

		MyInfo nUserInfo = (MyInfo) Facade.GetFacade ().data.Get ( FacadeConfig.USERINFO_MODULE_ID );
		UIHallMsg.GetInstance ().SndPKCancelPrepareGame (nUserInfo.userID, mRoomInfo.roomType ,mRoomInfo.roomIndex  );
		for (int i = 0; i < otherPlayers.Length; i++) 
		{
			MasterReadyPlayers otherPlayer = otherPlayers [i].GetComponent<MasterReadyPlayers> ();
			if (nUserInfo.userID == otherPlayer.useId) {
				otherPlayer.readyImage.gameObject.SetActive (false);
				break;
			}
		}
	}

	// Update is called once per frame
	void Update () {
		//Debug.Log (UIFind.modelIndex);
		switch (UIFind.modelIndex) {
		case 10:
			ShowCoin ();
			break;
		case 11:
			ShowCrad ();
			break;
		}
		//服务器发过来的数据

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

		//FiDisbandFriendRoomInform disbandRoom = (FiDisbandFriendRoomInform)data;
	}
		
	string GetDisplayName( string nNameIn )
	{
		return Tool.GetName ( nNameIn , 6 );
	}

	void ShowRoomOwnerInfo()
	{
		for (int i = 0; i < mUserList.Count; i++) {
			if (mOwnerId == mUserList [i].userId) 
			{
				MasterReadyPlayers Master = master.GetComponent<MasterReadyPlayers> ();
				//加载房主头像
				AvatarInfo nAVInfo = (AvatarInfo)Facade.GetFacade ().data.Get (FacadeConfig.AVARTAR_MODULE_ID);
				nAVInfo.Load (mUserList [i].userId, mUserList [i].avatar, ((int nResult, Texture2D nTexture) => {
					if (nResult == 0) {
						Master.headImage.sprite = Sprite.Create (nTexture, new Rect (0, 0, nTexture.width, nTexture.height), new Vector2 (0, 0));
					}

				}));
				Master.playerName.text = GetDisplayName (mUserList [i].nickName );
				Master.cannonImage = null;
				Master.readyImage.gameObject.SetActive (false);
				break;
			}
		}
	}

	MasterReadyPlayers GetEmptySeat()
	{
		for( int i = 0 ; i < otherPlayers.Length ; i ++ )
		{
			MasterReadyPlayers nEmptySeat = otherPlayers [ i ].GetComponent<MasterReadyPlayers> ();
			if (!nEmptySeat.isActive)
				return nEmptySeat;
		}
		return null;
	}

	void ShowFriendInfo()
	{
		//展示房主的信息
		for (int i = 0; i < mUserList.Count; i++) 
		{
			if (mOwnerId != mUserList [i].userId) {
				MasterReadyPlayers nFirendSeat = GetEmptySeat ();
				if (nFirendSeat == null) {
					Debug.LogError ( "----------------- ShowFriendInfo error----------------" );
					return;
				}
				RenderSeat ( nFirendSeat , mUserList [i] );
			}
		}
	}

	void ShowInfo(  )
	{
		if ( mUserList == null )
			return;
		
		ShowRoomOwnerInfo ();
		ShowFriendInfo ();

		//显示自己的界面
		MyInfo nUserInfo = (MyInfo) Facade.GetFacade ().data.Get ( FacadeConfig.USERINFO_MODULE_ID );
		MasterReadyPlayers nSelfSeat = GetEmptySeat ();
		FiUserInfo nSelfInfo = new FiUserInfo ();
		nSelfInfo.userId = nUserInfo.userID;
		nSelfInfo.nickName = nUserInfo.nickname;
		nSelfInfo.avatar = nUserInfo.avatar;
		RenderSeat ( nSelfSeat , nSelfInfo );

//		默认先等待
		for (int i = 0 ; i < otherPlayers.Length; i ++ ) {
			MasterReadyPlayers other = otherPlayers[ i ].GetComponent<MasterReadyPlayers>();
			if( !other.isActive )
			    other.waitImage.gameObject.SetActive (true);
		}
	}

	void RenderSeat( MasterReadyPlayers nSeat , FiUserInfo nInfo )
	{
		AvatarInfo nAVInfo = (AvatarInfo)Facade.GetFacade ().data.Get (FacadeConfig.AVARTAR_MODULE_ID);
		nAVInfo.Load (nInfo.userId, nInfo.avatar, ((int nResult, Texture2D nTexture) => {
			if (nResult == 0) {
				nSeat.headImage.sprite = Sprite.Create (nTexture, new Rect (0, 0, nTexture.width, nTexture.height), new Vector2 (0, 0));
			}
		}));
		nSeat.waitImage.gameObject.SetActive (false);
		nSeat.show.gameObject.SetActive (true);
		nSeat.playerName.text = GetDisplayName( nInfo.nickName );
		nSeat.cannonImage = null;
		nSeat.readyImage.gameObject.SetActive (false);
		nSeat.useId = nInfo.userId;
		nSeat.isActive = true;
		if ( nInfo.prepared ) {
			nSeat.isPrepared = true;
			nSeat.readyImage.gameObject.SetActive (true);
			nSeat.readyImage.overrideSprite = Resources.Load ("Room/準備中", typeof(Sprite))as Sprite;
		}
	}

	void RcvPKOtherEnterFriendRoomInform(object data)
	{
		FiOtherEnterFriendRoomInform nEnterInform = (FiOtherEnterFriendRoomInform)data;
		MasterReadyPlayers nEmptySeat = GetEmptySeat ();
		if ( nEmptySeat != null ) {
			RenderSeat ( nEmptySeat , nEnterInform.other );
		}
	}

	void RcvPKLeaveFriendRoomResponse(object data)
	{
		FiLeaveFriendRoomResponse leaveFriendRoom = (FiLeaveFriendRoomResponse) data;
		if (0 == leaveFriendRoom.result) {
			Destroy (this.gameObject);
		}
	}

	void RcvPKOtherLeaveFriendRoomInform(object data)
	{
		FiOtherLeaveFriendRoomInform leaveFriendRoom = (FiOtherLeaveFriendRoomInform) data;
		for( int i = 0; i < otherPlayers.Length; i++)
		{
			MasterReadyPlayers otherPlayer = otherPlayers [i].GetComponent<MasterReadyPlayers> ();
			if (otherPlayer.useId == leaveFriendRoom.leaveUserId) {
				otherPlayer.isPrepared = false;
				otherPlayer.isActive = false;
				otherPlayer.show.gameObject.SetActive (false);
				otherPlayer.waitImage.gameObject.SetActive (true);
				break;
			}
		}
	}
		

	void RcvPKOtherPrepareGameInform(object data)
	{
		FiPreparePKGame otherPrepare = (FiPreparePKGame)data;
		for(int i = 0;i < otherPlayers.Length;i++)
		{
			MasterReadyPlayers nSelectPlayer = otherPlayers [i].GetComponent<MasterReadyPlayers> ();
			if (otherPrepare.userId == nSelectPlayer.useId) {
				nSelectPlayer.isPrepared = true;
				nSelectPlayer.readyImage.gameObject.SetActive (true);
				nSelectPlayer.readyImage.overrideSprite = Resources.Load ("Room/準備中", typeof(Sprite))as Sprite;
				break;
			}
		}
	}

	void RcvStartPkGmaeInform(object data)
	{
		/*MyInfo myInfo = (MyInfo)Facade.GetFacade ().data.Get ( FacadeConfig.USERINFO_MODULE_ID );
		myInfo.TargetView = AppView.FISHING;
		AppControl.ToView (AppView.LOADING);*/
		//FiStartPKGameInform startInform = (FiStartPKGameInform)data;
		//JumpScene ();
		//Invoke ("JumpScene", 1f);
	}

	void OnDestroy()
	{
		Debug.LogError ( " ---------ui other room start remove--------- " );
		if(  Facade.GetFacade ().ui.Get ( FacadeConfig.UI_FISHING_FRIEND_ID ).Equals( this ) )
		{
			Debug.LogError ( " ---------ui other room remove success --------- " );
			Facade.GetFacade ().ui.Remove( FacadeConfig.UI_FISHING_FRIEND_ID );
		}
	}

	void RcvPKCancelPrepareGameInform(object data)
	{
		FiCancelPreparePKGame cancelPrepare = (FiCancelPreparePKGame)data;
		for(int i = 0;i<otherPlayers.Length;i++)
		{
			MasterReadyPlayers otherPlayer = otherPlayers [i].GetComponent<MasterReadyPlayers> ();
			if (cancelPrepare.userId == otherPlayer.useId) {
				otherPlayer.isPrepared = false;
				otherPlayer.readyImage.gameObject.SetActive (false);
				break;
			}
		}
	}
		
}
