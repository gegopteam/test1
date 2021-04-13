using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using AssemblyCSharp;
using System;
using Google.Protobuf;

public class UIGive : MonoBehaviour, IUiMediator
{

	public GameObject  StrangerOn;

	public GameObject  FriendOn;


	public InputField InputCount;

	public GameObject FriendPanel;
	public GameObject SearchPanel;
	public GameObject StangerPanel;

	public Image      ToolIcon;

	public Text TxtFriendEmptyTip;

	public InputField InputSearchUserId;

	int mGiveUnitId;

	void Start()
	{
		InputCount.text = "1";
		Facade.GetFacade ().ui.Add ( FacadeConfig.UI_GIVE_MODULE_ID , this );
		UIColseManage.instance.ShowUI (this.gameObject);
        Facade.GetFacade().message.friend.SendGetFriendList();
        GameManager nFriend =gameObject.GetComponentInChildren<GameManager>();
	}

	public void SetToolInfo( int nUnitId, Sprite nImage )
	{
		mGiveUnitId = nUnitId;
		if (nImage != null) {
			ToolIcon.sprite = nImage;
		}
	}

	public void OnInit()
	{

	}

	public void OnRelease()
	{

	}

	public void OnRecvData( int nType , object nData )
	{
		if (  nType == FiEventType.RECV_GET_USER_INFO_RESPONSE  ) 
		{
			FiGetUserInfoResponse nDataIn = (FiGetUserInfoResponse)nData;
			StangerPanel.SetActive ( true );
			UIGive_Stranger nUserInfo = StangerPanel.GetComponentInChildren<UIGive_Stranger> (  );
			if (nUserInfo != null) 
			{
				nUserInfo.SetGender (nDataIn.nUserInfo.gender );
				string nNickName = Tool.GetName (nDataIn.nUserInfo.nickName, 4);
				nUserInfo.NickName.text = nNickName;
				nUserInfo.VipLevel.text = nDataIn.nUserInfo.vipLevel.ToString();
				nUserInfo.UserId.text = nDataIn.nUserInfo.gameId.ToString();
				nUserInfo.mUserId = nDataIn.nUserInfo.userId;
				nUserInfo.mGameId = nDataIn.nUserInfo.gameId;
				nUserInfo.currentUserId = nDataIn.nUserInfo.userId;
				nUserInfo.UserAvatar = nDataIn.nUserInfo.avatar;
				nUserInfo.DoRefresh ();
			}
			SearchPanel.SetActive ( false );
		}
	}


	public void OnFriendClick()
	{
		FriendOn.SetActive ( true );
		StrangerOn.SetActive ( false );

		FriendPanel.SetActive ( true );
		SearchPanel.SetActive ( false );
		StangerPanel.SetActive ( false );
	}

	public void OnStrangerClick()
	{
		FriendOn.SetActive ( false );
		StrangerOn.SetActive ( true );

		SearchPanel.SetActive ( true );
		FriendPanel.SetActive ( false );
		StangerPanel.SetActive ( false );
	}

	public void OnStrangerReturn()
	{
		SearchPanel.SetActive ( true );
		StangerPanel.SetActive ( false );
	}

	public void OnSend()
	{

        MyInfo myInfo = DataControl.GetInstance().GetMyInfo();
        if (myInfo.isGuestLogin==true)
        {
            GameObject Window = Resources.Load("Window/WindowTips") as GameObject;
            GameObject nWindowClone = Instantiate(Window);
            UITipClickHideManager ClickTip = nWindowClone.GetComponent<UITipClickHideManager>();
            ClickTip.text.text = "當前遊客模式，無法贈送";
        }
        BackpackInfo  nBackInfo = (BackpackInfo) Facade.GetFacade().data.Get( FacadeConfig.BACKPACK_MODULE_ID );
		FiBackpackProperty nProp = nBackInfo.Get ( mGiveUnitId );
		if ( nProp == null ||  nProp.count <= 0) {
			GameObject Window = Resources.Load ("Window/WindowTips")as GameObject;
			GameObject nWindowClone = Instantiate (Window);
			UITipClickHideManager ClickTip = nWindowClone.GetComponent<UITipClickHideManager> ();
			ClickTip.text.text = "道具數量不足，無法贈送";
			return;
		}

		//向服务器发送赠送的消息
		//Debug.Log("向该好友赠送道具");
		int mFriendId = 0;
		if ( FriendPanel.activeSelf ) {
			UIGive_Friends nFriendManager = FriendPanel.GetComponentInChildren< UIGive_Friends > ();
			mFriendId = nFriendManager.getSelectId ();
			if (mGiveUnitId >= FiPropertyType.TORPEDO_MINI && mGiveUnitId <= FiPropertyType.TORPEDO_NUCLEAR) {
				mFriendId = nFriendManager.getSelectGameId();
			}
		} else {
			if (StangerPanel.activeSelf) {
				UIGive_Stranger nUserInfo = StangerPanel.GetComponentInChildren<UIGive_Stranger> ();
				mFriendId = nUserInfo.mUserId;
				if (mGiveUnitId >= FiPropertyType.TORPEDO_MINI && mGiveUnitId <= FiPropertyType.TORPEDO_NUCLEAR) {
					mFriendId = nUserInfo.mGameId;
				}
			}
		}

		int nSendCount = 0;
		try{
			nSendCount = int.Parse ( InputCount.text );
		}catch( Exception e ){

		}
			
		Debug.LogError ( "my select id------" + mFriendId + "/" + mGiveUnitId + "/" + nSendCount);
		if (mFriendId != 0 && nSendCount > 0) {
			//赠送的是鱼雷，message id 更改
			if (mGiveUnitId >= FiPropertyType.TORPEDO_MINI && mGiveUnitId <= FiPropertyType.TORPEDO_NUCLEAR) {
				FiProperty nSendProp = new FiProperty ();
				nSendProp.type = FiPropertyType.GET_CL_TORPDEO_ID(mGiveUnitId);
				nSendProp.value = nSendCount;
				//Debug.LogError ( "nSendProp.type------" + nSendProp.type + "/" + nSendCount );
				Facade.GetFacade ().message.bank.SendCLGiveRequest ( 1 , ByteString.CopyFromUtf8("") , mFriendId , nSendProp  );
			} else {	
				Facade.GetFacade ().message.backpack.SendGiveOtherPropertyRequest (mFriendId, mGiveUnitId, nSendCount);
			}
		}else {
			UnityEngine.GameObject nTipWindow = UnityEngine.Resources.Load ("Window/WindowTipsThree")as UnityEngine.GameObject;
			UnityEngine.GameObject nTipWindowEntity = UnityEngine.GameObject.Instantiate ( nTipWindow );
			UITipAutoNoMask ClickTips1 = nTipWindowEntity.GetComponent<UITipAutoNoMask> ();
			ClickTips1.tipText.text = "請搜索正確的用戶id";
		}
	}

	public void OnSearch()
	{
		Debug.LogError ( "---------------selected id--------" + InputSearchUserId.text );
		string nValue = InputSearchUserId.text;
		if (string.IsNullOrEmpty (nValue)) {
			GameObject Window = UnityEngine.Resources.Load ("Window/WindowTips")as UnityEngine.GameObject;
			GameObject WindowClone = UnityEngine.GameObject.Instantiate (Window);
			UITipClickHideManager ClickTips = WindowClone.GetComponent<UITipClickHideManager> ();
			ClickTips.time.text = "2";
			ClickTips.text.text = "請輸入id帳號";
			return;
		}

		try{
			int nUserId = int.Parse ( nValue );
			if( nUserId != 0 ){
				Facade.GetFacade().message.friend.SendGetUserInfoRequest( nUserId );
			}
		}catch( Exception e ){
			GameObject Window = UnityEngine.Resources.Load ("Window/WindowTips")as UnityEngine.GameObject;
			GameObject WindowClone = UnityEngine.GameObject.Instantiate (Window);
			UITipClickHideManager ClickTips = WindowClone.GetComponent<UITipClickHideManager> ();
			ClickTips.time.text = "2";
			ClickTips.text.text = "請輸入正確的id";
		}
	}

	public void OnCountEnd( string nValue )
	{
		Debug.LogError ( "---------2----------" + nValue );
		int toTxtIn = 0;
		try{
			toTxtIn = int.Parse (InputCount.text);
			BackpackInfo nInfo = (BackpackInfo)Facade.GetFacade ().data.Get ( FacadeConfig.BACKPACK_MODULE_ID );
			FiBackpackProperty nProp = nInfo.Get ( mGiveUnitId );

			if( toTxtIn >  nProp.count ){
				toTxtIn = nProp.count;
				InputCount.text = toTxtIn.ToString();
			}
		}catch( Exception e ){

		}
		if (toTxtIn == 0) {
			InputCount.text = 1+"";
		}
	}

	public void AddTool()
	{
		int value = int.Parse(InputCount.text);
		BackpackInfo nInfo = (BackpackInfo)Facade.GetFacade ().data.Get ( FacadeConfig.BACKPACK_MODULE_ID );
		FiBackpackProperty nProp = nInfo.Get ( mGiveUnitId );
		if (value >= nProp.count)
			return;
		value += 1;
		InputCount.text = value.ToString ();
	}

	public void ReduceTool()
	{
		int value = int.Parse(InputCount.text);
		if (value <= 1)
			return;
		value--;
		InputCount.text = value.ToString ();
	}

	public void OnClose()
	{
		UIColseManage.instance.CloseUI ();
	}


	void OnDestroy()
	{
		Facade.GetFacade ().ui.Remove ( FacadeConfig.UI_GIVE_MODULE_ID );
	} 
}

