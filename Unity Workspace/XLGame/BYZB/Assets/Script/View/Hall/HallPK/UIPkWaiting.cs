using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using AssemblyCSharp;
/// <summary>
/// User interface sign up.子弹赛，限时赛的登录
/// </summary>
public class UIPkWaiting : MonoBehaviour,IDispatch , IUiMediator {
	//获取是多少级的房间
	//public static string gameName;
	//获取那个sprite的控件，通过gameName来判断替换哪张图片
	//public Image spriteImage;
	//public Image titleImage;
	//RoomManager.roomMessage [i].roomState.overrideSprite = Resources.Load ("Room/游戏中", typeof(Sprite))as Sprite;
	public Text TxtUserCount;//当接收到有人进来就改变其中的人数显示

	int mRoomType;
	int mRoomLevel;
	int mRoomIndex;

	private int mUserCount = 1;

	public Sprite[] IconPKLevel;
	public Sprite[] IconPKBackGroud;
	public Sprite[] IconCombatBack;

	public Image ImgLevel;
	public Image ImgBack;


	public GameObject CombatPanel;
	public GameObject PkPanel;

	public Image ImgCombatLevel;
	public Image ImgCombatBack;

	public void SetRoomInfo( int nRoomType , int nLevel )
	{
		Sprite[] nTargetBack = IconPKBackGroud;

		Image nImgBack = ImgBack;
		Image nImgLevel = ImgLevel;

		if (nRoomType == PKRuleType.ROOM_COMBAT) 
		{
			nTargetBack = IconCombatBack;
			PkPanel.SetActive (false);
			CombatPanel.SetActive (true);
			nImgBack = ImgCombatBack;
			nImgLevel = ImgCombatLevel;
		} else 
		{
			PkPanel.SetActive (true);
			CombatPanel.SetActive ( false );
		}

		switch ( nLevel ) 
		{
		case PKRoomRuleType.GOLD_TYPE_MIDDLE:
			nImgBack.overrideSprite = nTargetBack [1];
			nImgLevel.overrideSprite = IconPKLevel [1];
			break;
		case PKRoomRuleType.GOLD_TYPE_HIGH:
			nImgBack.overrideSprite = nTargetBack[2];
			nImgLevel.overrideSprite =  IconPKLevel [2];
			break;
		default:
			nImgBack.overrideSprite = nTargetBack [0];
			nImgLevel.overrideSprite =  IconPKLevel [0];
			break;
		}
		mRoomType = nRoomType;
		mRoomLevel = nLevel;
	}

	void Start () 
	{
		UIHallObjects.GetInstance ().SetRcv (AppFun.UIHALL_INPKROOM, this);
		Facade.GetFacade ().ui.Add ( FacadeConfig.UI_PK_ROOM_ID , this );
	}

	public void OnRecvData( int nType , object nData )
	{
//		switch( nType )
//		{
//		case FiEventType.RECV_START_PK_GAME_INFORM: //接收开始PK游戏通知
//			Debug.LogError( "[ UiSignUp ]  RECV_START_PK_GAME_INFORM  !!!" );
//			RcvPKStartGameInform( nData );
//			break;
//		}
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
//			RcvPKStartGameInform(data);
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

	public void RcvEnterRoomResponse(object data)
	{
		FiEnterPKRoomResponse enterRoom = (FiEnterPKRoomResponse)data;
		//Debug.LogError ("*****************enterRoom.result" + enterRoom.result + " / " + roomindex );
		if (enterRoom.result == 0) {
			mUserCount = enterRoom.others.Count + 1;
			mRoomIndex = enterRoom.roomIndex;
			TxtUserCount.text = mUserCount.ToString ();
		}
	}

	void RcvPKOtherEnterRoomInform(object data)
	{
		FiOtherEnterPKRoomInform enterRoom = (FiOtherEnterPKRoomInform) data;
		mUserCount++;
		TxtUserCount.text = mUserCount.ToString ();
	}

	void RcvPKOtherLeaveRoomInform(object data)
	{
		FiOtherLeavePKRoomInform enterRoom = (FiOtherLeavePKRoomInform) data;
		mUserCount--;
		TxtUserCount.text = mUserCount.ToString ();
	}



	public void CancalSignUp()
	{
	//	Debug.Log ("+++++退出房间");
	//	Debug.LogError ("leave combat room : roomType"+roomType+"roomindex"+roomindex+"UIGame.goldType"+UIGame.goldType);
		//MyInfo nInfo =(MyInfo) Facade.GetFacade ().data.Get ( FacadeConfig.USERINFO_MODULE_ID );
		Facade.GetFacade ().message.fishPkRoom.SendPKLeaveRoomRequest ( mRoomType , mRoomIndex , mRoomLevel );
	}

	void OnDestroy()
	{
		UIHallObjects.GetInstance ().SetRcv (AppFun.UIHALL_INPKROOM, null);
		IUiMediator nMediator=  Facade.GetFacade ().ui.Get (FacadeConfig.UI_PK_ROOM_ID);
		if( nMediator != null && nMediator.Equals(this) )
			Facade.GetFacade ().ui.Remove ( FacadeConfig.UI_PK_ROOM_ID  );
	}
}
