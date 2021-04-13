using UnityEngine;
using System.Collections;
using AssemblyCSharp;
using UnityEngine.UI;
/// <summary>
/// User interface made.确认创建房间,发送创建的信息
/// </summary>
public class UIMade : MonoBehaviour  , IUiMediator {
	private GameObject WindowClone;
	private PanelManager panelManager;
	private int model;

	public GameObject roomCardModel;
	public GameObject coinModel;

	public Button cardButton;
	public Button coinButton;

	// Use this for initialization
	void Start () {
		roomCardModel.SetActive (true);
		coinModel.SetActive (false);
		cardButton.GetComponent<Image> ().sprite = Resources.Load ("Room/房卡模式_选中",typeof(Sprite))as Sprite;
		model = 11;
		panelManager = transform.GetComponentInChildren<PanelManager> ();

		Facade.GetFacade ().ui.Add ( FacadeConfig.UI_FISHING_FRIEND_ID , this );
	}

	public void OnRecvData( int nType , object nData )
	{
		switch ( nType ) {
		case FiEventType.RECV_CREATE_FRIEND_ROOM_RESPONSE: //接收创建好友约战房间回复
			RcvPKCreateFriendRoomResponse ( nData );
			Debug.Log ("RcvPKCreateFriendRoomResponse");
			break;
		}
	}

	public void OnInit()
	{
		
	}
	 
	public void OnRelease()
	{
		
	}


	// Update is called once per frame
	void Update () {
	}
		
//	void OnGUI()
//	{
//		Tool.ShowInGUI ();
//	}

	public void CardModelButton()
	{
		roomCardModel.SetActive (true);
		coinModel.SetActive (false);
		panelManager = transform.GetComponentInChildren<PanelManager> ();
		cardButton.GetComponent<Image> ().sprite = Resources.Load ("Room/房卡模式_选中",typeof(Sprite))as Sprite;
		coinButton.GetComponent<Image> ().sprite = Resources.Load ("Room/金币模式_未选中",typeof(Sprite))as Sprite;
		model = 11;
	}

	public void CoinModelButton()
	{
		roomCardModel.SetActive (false);
		coinModel.SetActive (true);
		panelManager = transform.GetComponentInChildren<PanelManager> ();
		cardButton.GetComponent<Image> ().sprite = Resources.Load ("Room/房卡模式_未选中",typeof(Sprite))as Sprite;
		coinButton.GetComponent<Image> ().sprite = Resources.Load ("Room/金币模式_选中",typeof(Sprite))as Sprite;
		model = 10;
	}

	public void ExitButton()
	{
		Destroy ( gameObject );
	}

	public void Create()
	{
		//判断房卡数量是否满足，只有满足才可以创建房间
		//如果是金币模式，则判断金币是否充足
		Debug.Log (panelManager.name);
		panelManager.SetInfoList (model);
		//UIHallMsg.GetInstance().SndPKCreateFriendRoomRequest(PanelManager.modelIndex,
	}


	void RcvPKCreateFriendRoomResponse(object data)
	{
		FiCreateFriendRoomResponse nRoomInfo = (FiCreateFriendRoomResponse)data;
		if (0 == nRoomInfo.result) {
			transform.gameObject.SetActive (false);
			WindowClone = Resources.Load ("Window/ReadyWindowInRoom") as GameObject;
			GameObject Window = Instantiate (WindowClone);
			UIInRoomReady nReadyRoom = Window.GetComponentInChildren< UIInRoomReady > ();
			if (nReadyRoom != null) {
				nReadyRoom.SetRoomInfo (nRoomInfo.room);
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
