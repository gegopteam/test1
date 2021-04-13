using UnityEngine;
using System.Collections;
using AssemblyCSharp;
using UnityEngine.UI;

public class UiHallPkSelect : MonoBehaviour {

	public Image AvatarIcon;

	public Text  NickName;
	public GameObject hallWindow;
	void OnAvatarComplete( int nResult , Texture2D nImage )
	{
		if (nResult == 0) {
			nImage.filterMode = FilterMode.Bilinear;
			nImage.Compress (true);
			AvatarIcon.sprite = Sprite.Create(nImage, new Rect (0, 0, nImage.width,nImage.height),new Vector2(0,0));
		}
	}



	// Use this for initialization
	void Start () {
	
		MyInfo nInfo = (MyInfo)Facade.GetFacade ().data.Get ( FacadeConfig.USERINFO_MODULE_ID );

		if (nInfo.oneMoreGame) 
		{
			int type = nInfo.lastGame.type;
			switch (type) {
			case PKRoomRuleType.ROOM_BULLET:
				OnEnterBulletRoom ();
				break;
			case PKRoomRuleType.ROOM_TIME:
				OnEnterTimeLimitRoom ();
				break;
			case PKRoomRuleType.ROOM_COMBAT:
				OnEnterCombatRoom ();
				break;
			}
		} else {
		
			if (NickName != null) {
				NickName.text = nInfo.nickname;
			}

			if (AvatarIcon != null) {
				AvatarInfo nAvatarInfo =(AvatarInfo) Facade.GetFacade ().data.Get ( FacadeConfig.AVARTAR_MODULE_ID );
				nAvatarInfo.Load ( nInfo.userID , nInfo.avatar , OnAvatarComplete );
			}
		}
		UIStore.HideEvent += HideSelf;
		UIVIP.SeeEvent += SeeSelf;
		UIUpgrade.HideEvent += HideSelf;
	}
	void SeeSelf()
	{
		hallWindow.SetActive (true);
	}
	void HideSelf()
	{
		hallWindow.SetActive (false);
	}


	public void OnExit()
	{
		//Debug.LogError ( "OnExit" );
		AppControl.ToView (AppView.HALL);
	}

	public void OnEnterBulletRoom()
	{
		Debug.LogError ( "OnEnterBulletRoom" );
		MyInfo nInfo = (MyInfo)Facade.GetFacade ().data.Get ( FacadeConfig.USERINFO_MODULE_ID );
		nInfo.PK_EnterRoomType = PKRoomRuleType.ROOM_BULLET;
		GameObject nBulletWindow = Resources.Load ("PkHall/PKRoomJoinWindow") as GameObject;
		GameObject nTarget = Instantiate (nBulletWindow);
		nTarget.GetComponentInChildren<UIPkJoinRoom> ().SetRoomType( PKRuleType.ROOM_BULLET );
	}

	void OnDestroy()
	{
		//StopCoroutine ( "UpdateClientInfo" );
		//Facade.GetFacade ().ui.Remove( FacadeConfig.UI_HALL_MODULE_ID );
		UIStore.HideEvent -= HideSelf;
		UIVIP.SeeEvent -= SeeSelf;
		UIUpgrade.HideEvent -= HideSelf;
		//UISetSailGift.SeeHandEvent -= NewHandEffect;
	}
	public void OnEnterTimeLimitRoom()
	{
		Debug.LogError ( "OnEnterTimeLimitRoom" );
		MyInfo nInfo = (MyInfo)Facade.GetFacade ().data.Get ( FacadeConfig.USERINFO_MODULE_ID );
		nInfo.PK_EnterRoomType = PKRoomRuleType.ROOM_TIME;
		GameObject TimeWindow = Resources.Load ("PkHall/PKRoomJoinWindow") as GameObject;
		GameObject nTarget = Instantiate (TimeWindow);
		nTarget.GetComponentInChildren<UIPkJoinRoom> ().SetRoomType( PKRuleType.ROOM_TIME );
	}

	public void OnEnterCombatRoom()
	{
		Debug.LogError ( "OnEnterCombatRoom" );
		MyInfo nInfo = (MyInfo)Facade.GetFacade ().data.Get ( FacadeConfig.USERINFO_MODULE_ID );
		nInfo.PK_EnterRoomType = PKRoomRuleType.ROOM_COMBAT;
		GameObject IntegralWindow = Resources.Load ("PkHall/PKRoomJoinWindow") as GameObject;
		GameObject nTarget = Instantiate (IntegralWindow);
		nTarget.GetComponentInChildren<UIPkJoinRoom> ().SetRoomType( PKRuleType.ROOM_COMBAT );
	}


	public void OnCreateFriendRoom()
	{
		Debug.LogError ( "OnCreateFriendRoom" );
		GameObject Window = Resources.Load ("PkHall/PKCreateWindow")as GameObject;
		Instantiate (Window);
	}


	public void OnEnterFriendRoom()
	{
		Debug.LogError ( "OnEnterFriendRoom" );
		GameObject Window = Resources.Load ("PkHall/PkFriendSearchWindow")as GameObject;
		Instantiate(Window);
	}


}
