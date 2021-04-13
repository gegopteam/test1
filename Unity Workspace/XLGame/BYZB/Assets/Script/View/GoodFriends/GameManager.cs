using UnityEngine;
using System.Collections.Generic;
using UnityEngine.UI;
using AssemblyCSharp;
/// <summary>
/// Game manager.主要控制列表有无好友时的显示，不包含玩家信息，每次打开GameFriend窗口需要重新排序好友需要将在线的提前加入，不在线的后加入
/// </summary>

public class GameManager : MonoBehaviour {
	//private GridLayoutGroup gridLayoutGroup;

	//[SerializeField]
	//InfinityGridLayoutGroup infinityGridLayoutGroup;

	public GameObject noFriend;
	public GameObject haveFriend;
	public static GameManager instans;
	public Text friendsMuch;//根据GoodList中好友数量的count来显示
	public Text wholeFriendsMuch;//根据等级来判断
	//长度等于GoodList游戏好友列表的长度，随时变动，当同意好友申请，则增加其长度
	//public RectTransform gridTransform;
	//int amount = ;
	private FriendInfo mFriInfo;
	private List<FiFriendInfo> nInfoList;
	public static BackageUI friendBackage;
	void Awake(){
		friendBackage = gameObject.GetComponent<BackageUI>();
		instans = this;
	}
	// Use this for initialization
	void Start () {
		//UIHallMsg.GetInstance().friendMsgHandle.FriendListEvent += Init;
		//gridLayoutGroup = gridTransform.GetComponentInChildren<GridLayoutGroup> ();
		//如果申请列表为空，则显示当前无游戏好友

		//infinityGridLayoutGroup = gameObject.GetComponentInChildren<InfinityGridLayoutGroup> ();

		FriendInfo nInfo = (FriendInfo)Facade.GetFacade ().data.Get ( FacadeConfig.FRIEND_MODULE_ID );
		List<FiFriendInfo> nInfoList = nInfo.GetFriendList();

		if(nInfoList.Count== 0)
		{
			noFriend.SetActive(true);
			haveFriend.SetActive(false);
		}else
		{
			noFriend.SetActive(false);
		    haveFriend.SetActive(true);
		}
	}
		
	public void DeleteUser( int userId )
	{
		GameFriendInfo[] infoArray = haveFriend.GetComponentsInChildren< GameFriendInfo > ();
		foreach (GameFriendInfo nInfo in infoArray) {
			if (nInfo.useId == userId) {
				Debug.LogError ( "---------do delete-------------" + userId );
				Destroy ( nInfo.gameObject );
				break;
			}
		}
	}

	//更新好友窗口
	public void UpdateInfo()
	{

		//Debug.LogError ( "--------------UpdateFriendWindow-------------" );

		mFriInfo = (FriendInfo)Facade.GetFacade ().data.Get (FacadeConfig.FRIEND_MODULE_ID);
		nInfoList = mFriInfo.GetFriendList ();
		if (nInfoList.Count == 0) {
			noFriend.SetActive (true);
			haveFriend.SetActive (false);
		} else {
			noFriend.SetActive (false);
			haveFriend.SetActive (true);
		}

	}

	// Update is called once per frame
	void Update () {
		mFriInfo = (FriendInfo)Facade.GetFacade ().data.Get ( FacadeConfig.FRIEND_MODULE_ID );
		nInfoList = mFriInfo.GetFriendList();
		friendsMuch.text = nInfoList.Count.ToString();
		wholeFriendsMuch.text = mFriInfo.countLimits.ToString(); //ApplyFriendInfo.friendLimit.ToString();
	}

	void OnDestroy()
	{
		//UIHallMsg.GetInstance().friendMsgHandle.FriendListEvent -= Init;
	}
}
