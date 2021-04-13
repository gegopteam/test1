using UnityEngine;
using System.Collections.Generic;
using AssemblyCSharp;
using UnityEngine.UI;
/// <summary>
/// User interface good friends.好友弹窗的的切换
/// </summary>

public class UIGoodFriends : MonoBehaviour , IUiMediator {

	private const int PAGE_FRIEND = 0;

	private const int PAGE_APPLY = 1;

	private const int PAGE_ADD = 2;

	public GameObject addWindow;
	public GameObject gameFriends;
	public GameObject applyWindow;

	public GameObject redPoint;

	public Image gameImage;
	public Image applyImage;
	public Image addImage;

	public static string currentButtonName;
	public static UIGoodFriends instans;
	public Animator ani;

	private List<GameObject> nWinList = new List<GameObject> ();
	private List<Image> nImageList = new List<Image> ();
	Color textColor;
	void Awake()
	{
		instans = this;
		textColor = new Color (1, 1, 1, 0.4f);
	}
	//这里删除了一个ganeobject
	public void OnRecvData( int nType , object nData )
	{
		switch (nType) {
		case FiEventType.RECV_DELETE_FRIEND_RESPONSE:
			//DoDeleteUserInFriend ( nData );取消删除game object 转而刷新列表
			break;
		}
	}

	void DoDeleteUserInFriend( object data )
	{
		FiDeleteFriendResponse nResult = (FiDeleteFriendResponse)data;
		GameManager nManager = gameFriends.transform.GetComponentInChildren< GameManager > ();
		if (nManager != null) {
			nManager.DeleteUser ( nResult.userId );
		}
	}

	public void OnInit()
	{
		
	}

	public void OnRelease()
	{
		
	}

	// Use this for initialization
	void Start () {
		Facade.GetFacade ().ui.Add ( FacadeConfig.UI_FRIEND_MODULE_ID , this );
		UIColseManage.instance.ShowUI (this.gameObject);
		//applyWindow.SetActive ( false );
		//gameFriends.SetActive ( true );
		//addWindow.SetActive ( false );
		nWinList.Add (addWindow);
		nWinList.Add (gameFriends);
		nWinList.Add (applyWindow);

		nImageList.Add ( addImage );
		nImageList.Add ( gameImage );
		nImageList.Add ( applyImage );

		Facade.GetFacade ().message.friend.SendGetFriendList ();
		currentButtonName = "遊戲好友";

		SwitchToPage ( PAGE_FRIEND );

		//更新好友列表数据
		GameManager nFriend = gameFriends.gameObject.GetComponentInChildren< GameManager > ();
		if (nFriend != null)
			nFriend.UpdateInfo();
		FriendInfo nInfo = (FriendInfo)Facade.GetFacade ().data.Get ( FacadeConfig.FRIEND_MODULE_ID );
		if (nInfo.getApplyFriends ().Count > 0)
			redPoint.SetActive (true);
		else
			redPoint.SetActive (false);
	}

	void SwitchToPage( int nPageType )
	{
		GameObject toSelectObject = null;
		Image      toSelectImage = null;
		switch (nPageType) {
		case PAGE_ADD:
			toSelectObject = addWindow;
			toSelectImage = addImage;
			break;
		case PAGE_APPLY:
			toSelectObject = applyWindow;
			toSelectImage = applyImage;
			break;
		case PAGE_FRIEND:
			toSelectObject = gameFriends;
			toSelectImage = gameImage;
			break;
		}

        for (int i = 0; i < nWinList.Count; i++){
            if (nWinList[i].Equals(toSelectObject)) {
                toSelectObject.SetActive(true);
                nImageList[i].sprite = UIHallTexturers.instans.Ranking[10];
				nImageList [i].color = Color.white;
				nImageList[i].transform.GetChild(0).GetComponent<Image>().color= Color.white;
            }
            else{
                nWinList[i].SetActive(false);
                nImageList[i].sprite = UIHallTexturers.instans.Ranking[11];
				nImageList [i].color = Color.clear;
				nImageList[i].transform.GetChild(0).GetComponent<Image>().color = textColor;
            }
        }
    }

	public void OnButton()
	{
		//AppControl.CloseWindow ("Window/GoodFriends");
		AudioManager._instance.PlayEffectClip(AudioManager.effect_closePanel);
		ani.SetBool ("isClose", true);
		transform.GetChild (0).GetChild(0).gameObject.SetActive (false);
		UIColseManage.instance.CloseUI ();
//		Invoke ("destroyGameObject", 0.5f);
	}
//	void destroyGameObject(){
//		Destroy(this.gameObject);
//	}
	public void AddFriends()
	{
		//Debug.LogError ( "add" );
		AudioManager._instance.PlayEffectClip(AudioManager.ui_click);
		SwitchToPage ( PAGE_ADD );
	}
	//点击好友按钮，这里的刷新数据应该放到接收到服务器发过来的数据之后再进行刷新，
	public void GameFriends()
	{
		//Debug.LogError ( "friend" );
		AudioManager._instance.PlayEffectClip(AudioManager.ui_click);
		Facade.GetFacade ().message.friend.SendGetFriendList ();
		currentButtonName = "遊戲好友";
		SwitchToPage ( PAGE_FRIEND );

		//更新好友列表数据
		GameManager nFriend = gameFriends.gameObject.GetComponentInChildren< GameManager > ();
		if (nFriend != null)
			nFriend.UpdateInfo();
		//Debug.LogError ( "-------friend--------" + nFriend );
	}

	public void ApplyWindow()
	{
		//Debug.LogError ( "apply" );
		AudioManager._instance.PlayEffectClip(AudioManager.ui_click);
		currentButtonName = "申請好友";
		Facade.GetFacade ().message.friend.SendGetFriendApplyList ();
		SwitchToPage ( PAGE_APPLY );
		//更新好友申请数据

	}
	public void RefreshApply(){
		ApplyManager nApply = applyWindow.gameObject.GetComponentInChildren< ApplyManager > ();
		if (nApply != null)
			nApply.UpdateInfo();
	}

	void OnDestroy()
	{
		Facade.GetFacade ().ui.Remove ( FacadeConfig.UI_FRIEND_MODULE_ID );
	}
}
