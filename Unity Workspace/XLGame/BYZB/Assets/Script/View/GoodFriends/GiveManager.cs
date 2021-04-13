using UnityEngine;
using System.Collections;

public class GiveManager : MonoBehaviour {
	private GameObject WindowClone;
	public GameObject noFriend;
	public GameObject haveFriend;
	public InfinityGridLayoutGroup infinityGridLayout;
	//长度等于GoodList游戏好友列表的长度，随时变动，当同意好友申请，则增加其长度
	//int amount = ;

	// Use this for initialization
	void Start () {
		//		//如果申请列表为空，则显示当前无游戏好友
		//		if( == 0)
		//		{
		//			noFriend.setActive(true);
		//          haveFriend.SetActive(false);
		//		}else
		//		{
		//		    noFriend.setActive(false);
		//          haveFriend.SetActive(true);
		//		}
		//infinityGridLayout.SetAmount(amount);
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	public void GiveCoin()
	{
		//Debug.Log("赠送金币");
	}

	public void GiveTool()
	{
		//Debug.Log ("赠送道具");
		string path = "Window/BagWindow";
		WindowClone = AppControl.OpenWindow (path);
		WindowClone.SetActive (true);
	}

	public void ScreenButton()
	{
		//向服务发送屏蔽该好友的操作
	}

	public void TalkButton()
	{
		//与该好友聊天，弹出聊天框
	}
}
