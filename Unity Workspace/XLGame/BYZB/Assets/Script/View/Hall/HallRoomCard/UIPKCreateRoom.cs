using UnityEngine;
using System.Collections;
using AssemblyCSharp;
using UnityEngine.UI;
using System .Collections.Generic;
/// <summary>
/// User interface made.确认创建房间,发送创建的信息
/// </summary>
public class UIPKCreateRoom : MonoBehaviour  , IUiMediator {
	
	//private PanelManager panelManager;

	private int mRoomType;

	public GameObject CardPanel;
	public GameObject CoinPanel;

	//public Button BtnCard;
	//public Button BtnCoin;

	public GameObject PageTitleInfo;

	public Sprite[] IconButton;

	//ToggleGroup[] mToggleArray;

	void Start () {
		
		CardPanel.SetActive (true);
		CoinPanel.SetActive (false);
		//BtnCard.GetComponent<Image> ().sprite = IconButton[ 1 ];
		mRoomType = PKRuleType.FRIEND_ROOM_CARD;
		ShowPage ( mRoomType );
		//panelManager = transform.GetComponentInChildren<PanelManager> ();
		Facade.GetFacade ().ui.Add ( FacadeConfig.UI_FISHING_FRIEND_ID , this );

		//mToggleArray = GetComponentsInChildren<ToggleGroup> ();
	}

	void ShowPage( int nRoomType )
	{
		if (mRoomType == PKRuleType.FRIEND_ROOM_CARD) {
			PageTitleInfo.transform.Find ("CardBack").gameObject.SetActive (true);
			PageTitleInfo.transform.Find ("GoldBack").gameObject.SetActive (false);
			CardPanel.SetActive (true);
			CoinPanel.SetActive (false);
		} else {
			PageTitleInfo.transform.Find ("CardBack").gameObject.SetActive (false);
			PageTitleInfo.transform.Find ("GoldBack").gameObject.SetActive (true);
			CardPanel.SetActive (false);
			CoinPanel.SetActive (true);
		}
	}

	public void OnRecvData( int nType , object nData )
	{
		switch ( nType ) {
		case FiEventType.RECV_CREATE_FRIEND_ROOM_RESPONSE: //接收创建好友约战房间回复
			RcvPKCreateFriendRoomResponse ( nData );
			break;
		}
	}

	public void OnInit()
	{
		
	}
	 
	public void OnRelease()
	{
		
	}

	public void OnSelectCard()
	{
		//panelManager = transform.GetComponentInChildren<PanelManager> ();
		mRoomType = PKRuleType.FRIEND_ROOM_CARD;
		ShowPage (mRoomType);
	}

	public void OnSelectCoin()
	{
		//panelManager = transform.GetComponentInChildren<PanelManager> ();
		mRoomType = PKRuleType.FRIEND_ROOM_GOLD;
		ShowPage (mRoomType);
	}

	public void ExitButton()
	{
		Destroy ( gameObject );
	}

	string GetToglleName( IEnumerator<Toggle> nSelectToggle )
	{
		//IEnumerator<Toggle> nSelectToggle = nCardGroup [i].ActiveToggles ().GetEnumerator();
		while( nSelectToggle.MoveNext() ){
			if (nSelectToggle.Current.isOn) {
				return nSelectToggle.Current.name;
			}
		}
		return "0";
	}


	public void OnRoomCostChange()
	{
		ToggleGroup[] nCardGroup = CoinPanel.GetComponentsInChildren<ToggleGroup> ();
		int nSelectGold = 0;
		for( int i = 0 ; i < nCardGroup.Length ; i ++ )
		{
			if (nCardGroup [i].name.Equals ("GoldCost")) {
				nSelectGold = int.Parse (GetToglleName (nCardGroup [i].ActiveToggles ().GetEnumerator ()));
				if (nSelectGold == 1) {
					CoinPanel.transform.Find ("groud3").Find("TxtCost").GetComponent<Text>().text = "1万";
				}else if (nSelectGold == 2) {
					CoinPanel.transform.Find ("groud3").Find("TxtCost").GetComponent<Text>().text = "5万";
				}else if (nSelectGold == 3) {
					CoinPanel.transform.Find ("groud3").Find("TxtCost").GetComponent<Text>().text = "10万";
				}
				break;
			}
		}
	}


	public void OnCreateRoom()
	{
		//判断房卡数量是否满足，只有满足才可以创建房间
		//如果是金币模式，则判断金币是否充足
		if (mRoomType == PKRuleType.FRIEND_ROOM_CARD) {
			MyInfo nInfo = (MyInfo)Facade.GetFacade ().data.Get ( FacadeConfig.USERINFO_MODULE_ID );
			if (nInfo.roomCard <= 0) {
				string path = "Window/WindowTipsThree";
				GameObject WindowClone = AppControl.OpenWindow (path);
				WindowClone.SetActive (true);
				UITipAutoNoMask ClickTips1 = WindowClone.GetComponent<UITipAutoNoMask> ();
				ClickTips1.tipText.text = "房卡不足";
				return;
			}
		}

		if (mRoomType == PKRoomRuleType.FRIEND_ROOM_CARD) 
		{
			ToggleGroup[] nCardGroup = CardPanel.GetComponentsInChildren<ToggleGroup> ();
			int nSelectRound = 0;
			int nSelectTime = 0;
			for( int i = 0 ; i < nCardGroup.Length ; i ++ )
			{
				//获取局数
				if (nCardGroup [i].name.Equals ("RoundCount")) {
					nSelectRound = int.Parse (GetToglleName (nCardGroup [i].ActiveToggles ().GetEnumerator ()));
				} else if( nCardGroup [i].name.Equals ("TimeCost") ){
					nSelectTime =  int.Parse (GetToglleName (nCardGroup [i].ActiveToggles ().GetEnumerator ()));
				}
			}
			Debug.LogError ( mRoomType + "select time == " +nSelectTime  + " / " + nSelectRound );
			Facade.GetFacade ().message.fishFriend.SendPKCreateFriendRoomRequest ( mRoomType, 0 , nSelectTime, nSelectRound );
		} else 
		{ //金币模式
		
			ToggleGroup[] nCardGroup = CoinPanel.GetComponentsInChildren<ToggleGroup> ();
			int nSelectRound = 0;
			int nSelectTime = 0;
			int nSelectGold = 0;
			for( int i = 0 ; i < nCardGroup.Length ; i ++ )
			{
				//获取局数
				if (nCardGroup [i].name.Equals ("RoundCount")) {
					nSelectRound = int.Parse (GetToglleName (nCardGroup [i].ActiveToggles ().GetEnumerator ()));
				} else if (nCardGroup [i].name.Equals ("TimeCost")) {
					nSelectTime = int.Parse (GetToglleName (nCardGroup [i].ActiveToggles ().GetEnumerator ()));
				} else if (nCardGroup [i].name.Equals ("GoldCost")) {
					nSelectGold = int.Parse (GetToglleName (nCardGroup [i].ActiveToggles ().GetEnumerator ()));
				}
			}
			Debug.LogError ( mRoomType +  "select time == " +nSelectTime  + " / round " + nSelectRound + " / gold " + nSelectGold );
			Facade.GetFacade ().message.fishFriend.SendPKCreateFriendRoomRequest ( mRoomType, nSelectGold , nSelectTime, nSelectRound );
		}
	}

	void RcvPKCreateFriendRoomResponse(object data)
	{
		FiCreateFriendRoomResponse nRoomInfo = (FiCreateFriendRoomResponse)data;
		if ( 0 == nRoomInfo.result ) 
		{
			transform.gameObject.SetActive (false);
			GameObject WindowClone = Resources.Load ("PkHall/PKFriendRoomWindow") as GameObject;
			GameObject Window = Instantiate (WindowClone);
			UIPKFriendRoom nReadyRoom = Window.GetComponentInChildren< UIPKFriendRoom > ();
			if (nReadyRoom != null) {
				nReadyRoom.SetRoomInfo( nRoomInfo.room );

				FiUserInfo nUserOwner = new FiUserInfo ();
				MyInfo nInfo = (MyInfo)Facade.GetFacade ().data.Get ( FacadeConfig.USERINFO_MODULE_ID );
				nUserOwner.avatar = nInfo.avatar;
				nUserOwner.userId = nInfo.userID;
				nUserOwner.level = nInfo.level;
				nUserOwner.gender = nInfo.sex;
				nUserOwner.nickName = nInfo.nickname;
				nReadyRoom.SetRoomOwner ( nUserOwner );
				nReadyRoom.ShowGuestButton ( false );
			}
			Destroy (gameObject);
		}
	}

	void OnDestroy()
	{
		Debug.LogError ( " ---------uimade start remove--------- " );
		if(  Facade.GetFacade ().ui.Get ( FacadeConfig.UI_FISHING_FRIEND_ID ).Equals( this ) )
		{
			Debug.LogError ( " ---------uimade remove success --------- " );
			Facade.GetFacade ().ui.Remove( FacadeConfig.UI_FISHING_FRIEND_ID );
		}
	}

}
