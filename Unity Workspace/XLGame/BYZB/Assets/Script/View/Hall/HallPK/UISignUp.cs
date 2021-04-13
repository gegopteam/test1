using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using AssemblyCSharp;
/// <summary>
/// User interface sign up.子弹赛，限时赛的登录
/// </summary>
public class UISignUp : MonoBehaviour,IDispatch , IUiMediator {
	//获取是多少级的房间
	//public static string gameName;
	//获取那个sprite的控件，通过gameName来判断替换哪张图片
	public Image spriteImage;
	public Image titleImage;
	//RoomManager.roomMessage [i].roomState.overrideSprite = Resources.Load ("Room/游戏中", typeof(Sprite))as Sprite;
	public Text currentPerson;//当接收到有人进来就改变其中的人数显示

	private int roomType;
	private int person = 1;
	private int roomindex;

	void Start () 
	{
		MyInfo nInfo = (MyInfo)Facade.GetFacade ().data.Get ( FacadeConfig.USERINFO_MODULE_ID );
		roomType = nInfo.PK_EnterRoomType;

		switch ( nInfo.lastGame.level ) 
		{
		case PKRoomRuleType.GOLD_TYPE_MIDDLE:
			spriteImage.overrideSprite = Resources.Load ("RoomTime/中级图标", typeof(Sprite))as Sprite;
			titleImage.overrideSprite = Resources.Load ("RoomBullet/中级场", typeof(Sprite))as Sprite;
			break;
		case PKRoomRuleType.GOLD_TYPE_HIGH:
			spriteImage.overrideSprite = Resources.Load ("RoomTime/高级图标", typeof(Sprite))as Sprite;
			titleImage.overrideSprite = Resources.Load ("RoomBullet/高级场", typeof(Sprite))as Sprite;
			break;
		default:
			spriteImage.overrideSprite = Resources.Load ("RoomTime/初级图标", typeof(Sprite))as Sprite;
			titleImage.overrideSprite = Resources.Load ("RoomBullet/初级场", typeof(Sprite))as Sprite;
			break;
		}

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
		case FiEventType.RECV_ENTER_PK_ROOM_RESPONSE://自己进房间
			Debug.LogError( "[ UiSignUp ]  RECV_ENTER_PK_ROOM_RESPONSE  !!!" );
			RcvEnterRoomResponse(data);
			break;
		case FiEventType.RECV_LEAVE_PK_ROOM_RESPONSE://自己离开房间
			Debug.LogError( "[ UiSignUp ]  RECV_LEAVE_PK_ROOM_RESPONSE  !!!" );
			Debug.Log ("我要退出");
			RcvPKLeaveRoomResponse (data);
			break;
		case FiEventType.RECV_OTHER_ENTER_PK_ROOM_INFORM: //其他人进入PK准备房间通知
			Debug.LogError( "[ UiSignUp ]  RECV_OTHER_ENTER_PK_ROOM_INFORM  !!!" );
			RcvPKOtherEnterRoomInform (data);
			break;
		case FiEventType.RECV_OTHER_LEAVE_PK_ROOM_INFORM: //其他人离开PK准备房间消息通知
			Debug.LogError( "[ UiSignUp ]  RECV_OTHER_LEAVE_PK_ROOM_INFORM  !!!" );
			RcvPKOtherLeaveRoomInform (data);
			break;
		case FiEventType.RECV_START_PK_GAME_INFORM: //接收开始PK游戏通知
			Debug.LogError( "[ UiSignUp ]  RECV_START_PK_GAME_INFORM  !!!" );
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
		/*MyInfo myInfo = (MyInfo)Facade.GetFacade ().data.Get ( FacadeConfig.USERINFO_MODULE_ID );
		myInfo.TargetView = AppView.FISHING;
		AppControl.ToView (AppView.LOADING);*/

		//FiStartPKGameInform enterRoom = (FiStartPKGameInform)data;
		//Debug.Log ("游戏开始了");
		//Invoke ("JumpFish", 1f);
	}

//	void JumpFish()
//	{
//		//Debug.LogError ( "------------------" + DataControl.GetInstance ().GetMyInfo ().gold );
//		AppControl.ToView (AppView.FISHING);
//	}

	public void RcvEnterRoomResponse(object data)
	{
		FiEnterPKRoomResponse enterRoom = (FiEnterPKRoomResponse)data;
		//Debug.LogError ("*****************enterRoom.result" + enterRoom.result + " / " + roomindex );
		if (enterRoom.result == 0) {
			person = enterRoom.others.Count + 1;
			roomindex = enterRoom.roomIndex;
			//Debug.LogError ("roomindex" + enterRoom.roomIndex);
		}
	}

	void RcvPKOtherEnterRoomInform(object data)
	{
		FiOtherEnterPKRoomInform enterRoom = (FiOtherEnterPKRoomInform) data;
		person++;
	}

	void RcvPKOtherLeaveRoomInform(object data)
	{
		FiOtherLeavePKRoomInform enterRoom = (FiOtherLeavePKRoomInform) data;
		person--;
	}



	public void CancalSignUp()
	{
	//	Debug.Log ("+++++退出房间");
	//	Debug.LogError ("leave combat room : roomType"+roomType+"roomindex"+roomindex+"UIGame.goldType"+UIGame.goldType);
		MyInfo nInfo =(MyInfo) Facade.GetFacade ().data.Get ( FacadeConfig.USERINFO_MODULE_ID );
		Facade.GetFacade ().message.fishPkRoom.SendPKLeaveRoomRequest ( roomType , roomindex , nInfo.lastGame.level );
	}
		
	// Update is called once per frame
	void Update () {
		currentPerson.text = person.ToString ();
	}

	void OnDestroy()
	{
		UIHallObjects.GetInstance ().SetRcv (AppFun.UIHALL_INPKROOM, null);
		IUiMediator nMediator=  Facade.GetFacade ().ui.Get (FacadeConfig.UI_PK_ROOM_ID);
		if( nMediator != null && nMediator.Equals(this) )
			Facade.GetFacade ().ui.Remove ( FacadeConfig.UI_PK_ROOM_ID  );
	}
}
