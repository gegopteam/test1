using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using AssemblyCSharp;
using System.Collections.Generic;
using System;
/// <summary>
/// Add manager.添加好友的管理类
/// </summary>

public class AddManager : MonoBehaviour {
	public Text       myGameId;
	public InputField addGameId;
	public GameObject Friends;
	AddFriendInfo[] addFriends;
	RankInfo nRankInfo;
	List<FiRankInfo> friendList;
	int index;
	void Awake(){
		nRankInfo = (RankInfo) Facade.GetFacade ().data.Get ( FacadeConfig.RANK_MODULE_ID );
		friendList = nRankInfo.GetRichArray ();
		addFriends=Friends.GetComponentsInChildren<AddFriendInfo>();
	}
		

	// Use this for initialization
	//显示可以添加的好友的头像以及等级信息
	void Start () {
		MyInfo myinfo = (MyInfo)Facade.GetFacade ().data.Get ( FacadeConfig.USERINFO_MODULE_ID );
		myGameId.text = "您的遊戲ID：" + myinfo.loginInfo.gameId.ToString() + "，告訴別的玩家您的遊戲ID，讓他們添加您為好友吧！";
		//AddFriendInfo[] friendsInfo = GetComponentsInChildren<AddFriendInfo> ();//获取好友列表的挂载函数

		Debug.LogError ("addfriends:"+addFriends.Length);
		RefreshFriends (0);
		index = 0;
	}
		
	void RefreshFriends(int index){
		for (int i = 0; i < 8; i++) {
			if(index+i<friendList.Count)
				addFriends [i].RefreshView (friendList[index+i]);
			else
				addFriends [i].RefreshView (null);
		}
	}

	public void AnotherButton()
	{
		RefreshFriends (index + 8);
		index += 8;
		if (index+8 >= friendList.Count) {
			index = -8;
		}
	}

	public void AddButton()
	{
		//点击添加好友的时候获取输入框的ID
		AudioManager._instance.PlayEffectClip(AudioManager.ui_click);
		string text = addGameId.text;
		//Debug.Log ("添加好友"+int.Parse(text));
		FriendInfo nInfo = (FriendInfo)Facade.GetFacade ().data.Get ( FacadeConfig.FRIEND_MODULE_ID );
		List<FiFriendInfo> nInfoList = nInfo.GetFriendList ();

		if (string.IsNullOrEmpty( text)) {
			GameObject Window =  UnityEngine.Resources.Load ("Window/WindowTips")as UnityEngine.GameObject;
			GameObject WindowClone =  UnityEngine.GameObject.Instantiate (Window);
			UITipClickHideManager ClickTips = WindowClone.GetComponent<UITipClickHideManager> ();
			ClickTips.time.text = "2";
			ClickTips.text.text = "請輸入您要搜索的玩家ID";
			return;
		}

		int nInputTxt = 0;
		try{
		  nInputTxt = int.Parse ( text );
		}catch( Exception e ){

		}
	
		if (nInputTxt == 0) {
			GameObject Window =  UnityEngine.Resources.Load ("Window/WindowTips")as UnityEngine.GameObject;
			GameObject WindowClone =  UnityEngine.GameObject.Instantiate (Window);
			UITipClickHideManager ClickTips = WindowClone.GetComponent<UITipClickHideManager> ();
			ClickTips.time.text = "2";
			ClickTips.text.text = "錯誤的id";
			return;
		}


		for (int i = 0; i < nInfoList.Count; i++) 
		{
			if ( nInputTxt == nInfoList [i].userId )
			{
				GameObject Window =  UnityEngine.Resources.Load ("Window/WindowTips")as UnityEngine.GameObject;
				GameObject WindowClone =  UnityEngine.GameObject.Instantiate (Window);
				UITipClickHideManager ClickTips = WindowClone.GetComponent<UITipClickHideManager> ();
				ClickTips.time.text = "2";
				ClickTips.text.text = "當前ID已經是你的好友了!";
				return;
			}
		}

		MyInfo myinfo = (MyInfo)Facade.GetFacade ().data.Get ( FacadeConfig.USERINFO_MODULE_ID );
		if ( nInputTxt == myinfo.loginInfo.gameId ) {
			GameObject Window = UnityEngine.Resources.Load ("Window/WindowTips")as UnityEngine.GameObject;
			GameObject WindowClone = UnityEngine.GameObject.Instantiate (Window);
			UITipClickHideManager ClickTips = WindowClone.GetComponent<UITipClickHideManager> ();
			ClickTips.time.text = "2";
		    ClickTips.text.text = "當前好友ID不存在!";
			return;
		} 
		Facade.GetFacade ().message.friend.SendAddFriend ( nInputTxt );
	}
}
