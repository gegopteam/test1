using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using AssemblyCSharp;
/// <summary>
/// User interface combat sign up.对抗赛登录控制UI的显示
/// </summary>

public class UICombatSignUp : MonoBehaviour,IDispatch , IUiMediator {
//	public delegate void HideGameWindowDelegate();
//	public static event HideGameWindowDelegate HideGameWindowEvent; 
	//获取是多少级的房间
	//public static string gameName;
	public Image spriteImage;
	public Image titleImage;

	private int roomType;
	private int roomindex;
	// Use this for initialization      Resources.Load ("Room/锁", typeof(Sprite))as Sprite;
	void Start () {
		
		MyInfo nInfo = (MyInfo)Facade.GetFacade ().data.Get ( FacadeConfig.USERINFO_MODULE_ID );
		switch ( nInfo.lastGame.level ) 
		{
		case PKRoomRuleType.GOLD_TYPE_LOW:
			spriteImage.overrideSprite = Resources.Load ("RoomCombat/低级场", typeof(Sprite))as Sprite;
			titleImage.overrideSprite = Resources.Load ("RoomBullet/初级场", typeof(Sprite))as Sprite;
			break;
		case PKRoomRuleType.GOLD_TYPE_MIDDLE:
			spriteImage.overrideSprite = Resources.Load ("RoomCombat/中级场", typeof(Sprite))as Sprite;
			titleImage.overrideSprite = Resources.Load ("RoomBullet/中级场", typeof(Sprite))as Sprite;
			break;
		case PKRoomRuleType.GOLD_TYPE_HIGH:
			spriteImage.overrideSprite = Resources.Load ("RoomCombat/高级场", typeof(Sprite))as Sprite;
			titleImage.overrideSprite = Resources.Load ("RoomBullet/高级场", typeof(Sprite))as Sprite;
			break;
		}

		roomType = nInfo.PK_EnterRoomType;
		UIHallObjects.GetInstance ().SetRcv (AppFun.UIHALL_INPKROOM, this);
		Facade.GetFacade ().ui.Add ( FacadeConfig.UI_PK_ROOM_ID , this );
	}


	public void OnRecvData( int nType , object nData )
	{
		switch( nType )
		{
		case FiEventType.RECV_START_PK_GAME_INFORM: //接收开始PK游戏通知
			Debug.LogError( "[ UiSignUp ]  RECV_START_PK_GAME_INFORM  !!!" );
			RcvPKStartGameInform( nData );
			break;
		}
	}

	public void OnInit()
	{
		
	}

	public void OnRelease()
	{
		
	}


	public void OnRcv(int type, object data)
	{
		switch(type)
		{
	//	case FiEventType.RECV_ENTER_PK_ROOM_RESPONSE://自己进房间
	//		RcvEnterRoomResponse(data);
	//		break;
		case FiEventType.RECV_LEAVE_PK_ROOM_RESPONSE://自己离开房间
			//Debug.LogError ("收到离开PK场消息");
			RcvPKLeaveRoomResponse (data);
			break;
		case FiEventType.RECV_OTHER_ENTER_PK_ROOM_INFORM: //其他人进入PK准备房间通知
			RcvPKOtherEnterRoomInform (data);
			break;
		case FiEventType.RECV_OTHER_LEAVE_PK_ROOM_INFORM: //其他人离开PK准备房间消息通知
			RcvPKOtherLeaveRoomInform (data);
			break;
		case FiEventType.RECV_START_PK_GAME_INFORM: //接收开始PK游戏通知
			RcvPKStartGameInform(data);
			break;
		}
	}

	void RcvPKLeaveRoomResponse(object data)
	{
		FiLeavePKRoomResponse leaveRoom = (FiLeavePKRoomResponse)data;
		if (leaveRoom.result == 0) {
			Destroy (this.gameObject);
		}
	}

	void RcvPKStartGameInform(object data)
	{
		//JumpFish ();
//		FiStartPKGameInform enterRoom = (FiStartPKGameInform)data;
//		Debug.Log ("游戏开始了");
//		Invoke ("JumpFish",1f);
	}

	void JumpFish()
	{
//		AppControl.ToView (AppView.FISHING);
//		MyInfo myInfo = (MyInfo)Facade.GetFacade ().data.Get ( FacadeConfig.USERINFO_MODULE_ID );
//		myInfo.TargetView = AppView.FISHING;
//		AppControl.ToView (AppView.LOADING);
	}

	public void RcvEnterRoomResponse(object data)
	{
		FiEnterPKRoomResponse enterRoom = (FiEnterPKRoomResponse)data;
		if (enterRoom.result == 0) {
			//person = enterRoom.others.Count + 1;
			roomindex = enterRoom.roomIndex;
			Debug.LogError ("roomindex" + enterRoom.roomIndex);
		}
	}

	void RcvPKOtherEnterRoomInform(object data)
	{
		FiOtherEnterPKRoomInform enterRoom = (FiOtherEnterPKRoomInform) data;
		Debug.Log("有人进来");
	}

	void RcvPKOtherLeaveRoomInform(object data)
	{
		FiOtherLeavePKRoomInform enterRoom = (FiOtherLeavePKRoomInform) data;
		Debug.Log ("有人退出了房间");

	}

	public void CancalSignUp()
	{
		Debug.LogError ("+++++退出房间" + roomindex);
		MyInfo nInfo =(MyInfo) Facade.GetFacade ().data.Get ( FacadeConfig.USERINFO_MODULE_ID );
		Facade.GetFacade ().message.fishPkRoom.SendPKLeaveRoomRequest ( roomType , roomindex , nInfo.lastGame.level );
		//UIHallMsg.GetInstance().SndPKLeaveRoomRequest(roomType,roomindex,UICombatGame.goldType);
	}

	// Update is called once per frame
	void Update () {
		
	}

	void OnDestroy()
	{
		UIHallObjects.GetInstance ().SetRcv (AppFun.UIHALL_INPKROOM, null);
		IUiMediator nMediator=  Facade.GetFacade ().ui.Get (FacadeConfig.UI_PK_ROOM_ID);
		if( nMediator != null && nMediator.Equals(this) )
	    	Facade.GetFacade ().ui.Remove ( FacadeConfig.UI_PK_ROOM_ID  );
	}
}

