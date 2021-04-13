using UnityEngine;
using System.Collections.Generic;
using AssemblyCSharp;
/// <summary>
/// Apply manager.主要控制列表有无好友申请时的显示，不包含玩家信息,每次进来都要重新获取
/// </summary>
public class ApplyManager : MonoBehaviour {

	public GameObject noFriend;
	public GameObject haveFriend;
	//public RectTransform gridTransform;
	public static BackageUI applyBackage;
	void Awake(){
		applyBackage = gameObject.GetComponent<BackageUI> ();
	}
	// Use this for initialization
	void Start () {
		//如果申请列表为空，则显示当前无游戏好友
		FriendInfo nInfo = (FriendInfo)Facade.GetFacade ().data.Get ( FacadeConfig.FRIEND_MODULE_ID );
		List<FiFriendInfo> nInfoList = nInfo.getApplyFriends ();
		if( nInfoList.Count == 0 )
		{
			noFriend.SetActive(true);
		    haveFriend.SetActive(false);
		}else
        {
		    noFriend.SetActive(false);
		    haveFriend.SetActive(true);
        }
	}
		
	public void UpdateInfo()
	{
		FriendInfo nInfo = (FriendInfo)Facade.GetFacade ().data.Get ( FacadeConfig.FRIEND_MODULE_ID );
		List<FiFriendInfo> nInfoList = nInfo.getApplyFriends ();
		if(nInfoList.Count == 0)
		{
			noFriend.SetActive(true);
			haveFriend.SetActive(false);
		}else
		{
			noFriend.SetActive(false);
			haveFriend.SetActive(true);
		}

	}

	// Update is called once per frame
	void Update () {
	
	}

	void OnDestroy()
	{
		
	}
}
