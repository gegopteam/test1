using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System.Collections.Generic;
using AssemblyCSharp;
using System;
/// <summary>
/// 查找房间，以及删除和重写功能
/// </summary>
public class UIFind : MonoBehaviour, IUiMediator {
	
	public InputField TxtRoomIndex; 

	public static int modelIndex;
	public static int roomIndex;
	public static int goldType;
	public static int roomTime;
	public static int roomRound;

	public GameObject NumberGroup;

	private int mLastInputNumber = 0;


	void Start () 
	{
		if ( NumberGroup  != null ) {
			Button[] nBtnIndex = NumberGroup.GetComponentsInChildren<Button> ();
			for (int i = 0; i < nBtnIndex.Length; i++) {
				EventTriggerListener.Get (nBtnIndex [i].gameObject).onClick = ClickCallBack;
			}
		}
		Facade.GetFacade ().ui.Add ( FacadeConfig.UI_FISHING_FRIEND_ID , this );
	}

	public void OnRecvData( int nType , object nData )
	{
		switch ( nType ) {
		case FiEventType.RECV_ENTER_FRIEND_ROOM_RESPONSE: //接收进入好友约战房间回复
			RcvPKEnterFriendRoomResponse ( nData );
			break;
		}
	}


	public void OnInputComfirm()
	{
		//匹配房间号查找房间,如果可以则进入其他人等待的房间
		//如果是房卡模式判断房卡数量是否满足，只有满足才可以进入房间
		//如果是金币模式，则判断金币是否满足
		try{
			int nRoomIndex = int.Parse (TxtRoomIndex.text);
			Debug.LogError( "-----------" + nRoomIndex );
			Facade.GetFacade().message.fishFriend.SendPKEnterFriendRoomRequest(  nRoomIndex  );
		}catch( Exception e ) {
			
		}
	}

	public void OnInputDelete()
	{
		int nStringLen = TxtRoomIndex.text.Length;
		if (nStringLen > 0) {
			if (nStringLen == 1) {
				mLastInputNumber = 0;
			}
			TxtRoomIndex.text = TxtRoomIndex.text.Substring (0, TxtRoomIndex.text.Length - 1);
		}
	}

	public void OnExit()
	{
		Destroy (this.gameObject);
	}


	public void OnInputChange()
	{
		//Debug.LogError ( "------change------" + value.text );
		try{
			if( string.IsNullOrEmpty( TxtRoomIndex.text ) ){
				mLastInputNumber = 0;
			}
			int nParaseValue = int.Parse( TxtRoomIndex.text );
			mLastInputNumber = nParaseValue;
		}catch( Exception e ){
			//Debug.LogError ( mLastInputNumber + "------change exception------" + value.text );
			if (mLastInputNumber > 0) {
				TxtRoomIndex.text = mLastInputNumber + "";
			} else if (mLastInputNumber == 0) {
				TxtRoomIndex.text = "";
			}
		}
	}

	public void OnInputEnd()
	{
		//Debug.LogError ( "------end------" + value.text );
	}

	public void OnInit()
	{
		
	}

	public void OnRelease()
	{
		
	}

	void RcvPKEnterFriendRoomResponse(object data)
	{
		FiEnterFriendRoomResponse enterFriendRoom = (FiEnterFriendRoomResponse)data;
		if(0==enterFriendRoom.result)
		{
			transform.gameObject.SetActive (false);
			GameObject WindowClone = Resources.Load ("PkHall/PKFriendRoomWindow") as GameObject;
			GameObject Window = Instantiate (WindowClone);
			//Debug.Log ("modeIndex"+enterFriendRoom.room.roomType);
			//Debug.Log ("roomIndex"+enterFriendRoom.room.roomIndex);
			modelIndex = enterFriendRoom.room.roomType;
			roomIndex = enterFriendRoom.room.roomIndex;
			goldType = enterFriendRoom.room.goldType;
			roomTime = enterFriendRoom.room.timeType;
			roomRound = enterFriendRoom.room.roundType;

			UIPKFriendRoom nReadyRoom = Window.GetComponentInChildren< UIPKFriendRoom > ();

			foreach( FiUserInfo user in enterFriendRoom.others ){
				if (user.userId == enterFriendRoom.roomOwnerId) {
					nReadyRoom.SetRoomOwner (user);
					//break;
				} else {
					nReadyRoom.AppendUser ( user );
				}
			}

			FiUserInfo nUserSelf = new FiUserInfo ();
			MyInfo nInfo = (MyInfo)Facade.GetFacade ().data.Get ( FacadeConfig.USERINFO_MODULE_ID );
			nUserSelf.avatar = nInfo.avatar;
			nUserSelf.userId = nInfo.userID;
			nUserSelf.level = nInfo.level;
			nUserSelf.gender = nInfo.sex;
			nUserSelf.nickName = nInfo.nickname;
			nReadyRoom.AppendUser ( nUserSelf );

			nReadyRoom.SetRoomInfo ( enterFriendRoom.room );
			nReadyRoom.ShowGuestButton ( true );
		}
	}

	void ClickCallBack(GameObject nTarget )
	{
//		if ( go.name.Equals ("ExitButton") ) {
//			
//		} else if (go.name.Equals ("Sure")) {
//			//匹配房间号查找房间,如果可以则进入其他人等待的房间
//			//如果是房卡模式判断房卡数量是否满足，只有满足才可以进入房间
//			//如果是金币模式，则判断金币是否满足
//			try{
//				int nRoomIndex = int.Parse (TxtRoomIndex.text);
//				Facade.GetFacade().message.fishFriend.SendPKEnterFriendRoomRequest(  nRoomIndex  );
//			}catch( Exception e ) {
//				
//			}
//		}
		{
			Text number = nTarget.GetComponentInChildren<Text> ();
			if ( TxtRoomIndex.text.Length < 6 ) {
				TxtRoomIndex.text += number.text;
			}
		}
	}

	// Update is called once per frame
	void Update()
	{
		
	}

	void OnDestroy()
	{
		Debug.LogError ( " ---------UiFind start remove--------- " );
		IUiMediator nMediator = Facade.GetFacade ().ui.Get (FacadeConfig.UI_FISHING_FRIEND_ID);
		if( nMediator != null && nMediator.Equals( this ) )
		{
			Debug.LogError ( " ---------UiFind remove success --------- " );
			Facade.GetFacade ().ui.Remove( FacadeConfig.UI_FISHING_FRIEND_ID );
		}
	}

}
