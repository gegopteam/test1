using UnityEngine;
using System.Collections;
using AssemblyCSharp;
using System.Collections.Generic;
public class FriendChatInfo : IDataProxy {
	Dictionary<int,List<FiChatMessage>> FriendChatDic;//存储好友聊天信息
	List<FiFriendInfo> FriendInfoDic;//存储好友个人信息，头像等级等。
	public void OnAddData( int nType , object nData ){}
	public  void OnInit(){}
	public FriendChatInfo(){
		FriendChatDic = new Dictionary<int, List<FiChatMessage>> ();
		FriendInfoDic = new List<FiFriendInfo>();
	}
	public void OnDestroy(){
		FriendChatDic.Clear ();
		FriendInfoDic.Clear ();
	}
	//获得好友个人信息
	public FiFriendInfo GetFriendInfo(int listID){
		return FriendInfoDic [listID];
	}
	//获得左侧正在聊天好友个数
	public int GetFriendChatNum{
		get{return FriendChatDic.Count;}
	}
	//添加一个新的好友进行聊天。
	public void AddChatFriend(FiFriendInfo friendInfo){
		if (FriendInfoDic.Contains (friendInfo)||FriendChatDic.ContainsKey(friendInfo.userId))
			return;
		else {
			FriendChatDic.Add (friendInfo.userId, new List<FiChatMessage> ());
			FriendInfoDic.Add (friendInfo);
		}
	}
	//获得好友间聊天列表
	public List<FiChatMessage> GetChatList(int friendID){
		if (FriendChatDic.ContainsKey (friendID))
			return FriendChatDic [friendID];
		Debug.LogError ("warning,can`t find friendID");
		return null;
	} 
	//移除某个正在聊天好友消息
	public void RemoveFriend(int listID){
		RemoveChater (FriendInfoDic [listID].userId);
		FriendInfoDic.RemoveAt (listID);
	}
	void RemoveChater(int friendID){
		if (FriendChatDic.ContainsKey (friendID))
			FriendChatDic.Remove(friendID);
	}

	~FriendChatInfo(){
		FriendChatDic.Clear();
		FriendInfoDic.Clear ();
	}
}
