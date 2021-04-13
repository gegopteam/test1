using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using AssemblyCSharp;

public class UIGive_Friends : MonoBehaviour {

	public GameObject FriendPanel;

	public GameObject FriendEmptyTip;

	public  RectTransform UnitParent;

	//private UIGameFriendManager  [] friendInfo;

	private GameObject mSelectedUser;

	private int mSelectUserId = 0;

    void Start()
    {

        StartCoroutine(ShowFriend());


    }
    IEnumerator ShowFriend()
    {
        yield return new WaitForSeconds(0.1f);
        FriendInfo nInfo = (FriendInfo)Facade.GetFacade().data.Get(FacadeConfig.FRIEND_MODULE_ID);
        if (nInfo.GetFriendList() != null && nInfo.GetFriendList().Count == 0)
        {
            FriendEmptyTip.SetActive(true);
            FriendPanel.SetActive(false);
        }
        else
        {
            FriendEmptyTip.SetActive(false);
            FriendPanel.SetActive(true);
            InitFriendArea();
        }
    }

	public int getSelectId()
	{
		if (mSelectedUser != null) {
			UIGive_FriendUnit nInfo = mSelectedUser.GetComponent<UIGive_FriendUnit> ();
			if (nInfo != null) {
				return nInfo.userId;
			}
		}
		return 0;
	}

	public int getSelectGameId()
	{
		if (mSelectedUser != null) {
			UIGive_FriendUnit nInfo = mSelectedUser.GetComponent<UIGive_FriendUnit> ();
			if (nInfo != null) {
				return nInfo.gameId;
			}
		}
		return 0;
	}

	//好友点击处理
	void GridClickCallBack(GameObject nClickUser)
	{
		if ( mSelectedUser != null && mSelectedUser.Equals (nClickUser)) {
			return;
		}
		UIGive_FriendUnit nCurrentFriend = nClickUser.GetComponent<UIGive_FriendUnit> ();
		if ( mSelectedUser != null ) {
			UIGive_FriendUnit nLastFriend = mSelectedUser.GetComponent<UIGive_FriendUnit> ();
			nLastFriend.SetSelected ( false );
		}
		nCurrentFriend.SetSelected ( true );
		mSelectedUser = nClickUser;
	}

	public void SetSelectedUserId( int nUserId )
	{
		mSelectUserId = nUserId;
	}

	//初始化好友栏目
	void InitFriendArea()
	{
		FriendInfo nInfo = (FriendInfo)Facade.GetFacade ().data.Get ( FacadeConfig.FRIEND_MODULE_ID );

		/*nInfo.GetFriendList ().Add ( nInfo.GetFriendList()[1] );
		nInfo.GetFriendList ().Add ( nInfo.GetFriendList()[1] );
		nInfo.GetFriendList ().Add ( nInfo.GetFriendList()[1] );
		nInfo.GetFriendList ().Add ( nInfo.GetFriendList()[1] );
		nInfo.GetFriendList ().Add ( nInfo.GetFriendList()[1] );
		nInfo.GetFriendList ().Add ( nInfo.GetFriendList()[1] );
		nInfo.GetFriendList ().Add ( nInfo.GetFriendList()[1] );
		nInfo.GetFriendList ().Add ( nInfo.GetFriendList()[1] );*/

		for ( int i = 0; i <nInfo.GetFriendList().Count; i++ ) {
			GameObject window = Resources.Load ("MainHall/BackPack/GiveFriendUnit") as GameObject;
			GameObject windowClone = Instantiate (window);
			windowClone.transform.SetParent ( UnitParent );
			windowClone.transform.localScale = Vector3.one;

			UIGive_FriendUnit nUnit = windowClone.GetComponentInChildren< UIGive_FriendUnit > ();
			nUnit.SetGender ( nInfo.GetFriendList()[ i ].gender );
			nUnit.TxtNickName.text =   Tool.GetName  (  nInfo.GetFriendList()[i].nickname , 4 );
			nUnit.TxtUserId.text = nInfo.GetFriendList()[i].gameId.ToString();
			nUnit.userId = nInfo.GetFriendList () [i].userId;
			nUnit.gameId =  nInfo.GetFriendList () [i].gameId;
			nUnit.UserAvatar = nInfo.GetFriendList () [i].avatar;


			windowClone.GetComponent<Button> ().onClick.AddListener ( delegate {
				GridClickCallBack( windowClone );
			} );

			//EventTriggerListener.Get ( nUnit.gameObject ).onClick = GridClickCallBack;

			if ( mSelectUserId == 0 ) {
				GridClickCallBack ( nUnit.gameObject );
				mSelectUserId = nUnit.userId;
			}else if( mSelectUserId == nInfo.GetFriendList () [i].userId ){
				GridClickCallBack ( nUnit.gameObject );
			}
		}

		//UIGive_FriendUnit[] friendInfo = GetComponentsInChildren<UIGive_FriendUnit> ();
		if ( nInfo.GetFriendList ().Count > 6 ) {
			int nListCount = ( nInfo.GetFriendList ().Count - 1 ) / 2 + 1;
			UnitParent.sizeDelta = new Vector2( UnitParent.sizeDelta.x , nListCount * 110 );
			UnitParent.localPosition = new Vector3( UnitParent.localPosition.x , -UnitParent.sizeDelta.y / 2 , 0 );
		}
	}

	// Update is called once per frame
	void Update () {
	
	}
}
