using UnityEngine;
using System.Collections.Generic;
using AssemblyCSharp;
using UnityEngine.UI;

public class FriendChatManager : MonoBehaviour {
	public GameObject spriteGrapic;
	public BackageUI ChatFriendItem;
	public BackageUI ChatPanel;
	FriendChatInfo mChatInfo ;
	public static FriendChatManager instance;
	public int chooseID;
	public InputField input;
	MyInfo myInfo;
	private Transform graphicParent;
	// Use this for initialization
	//public int ChooseID{
	//	set{chooseID = value; }
	//}
	void Awake(){
		instance = this;
		myInfo =(MyInfo) Facade.GetFacade ().data.Get ( FacadeConfig.USERINFO_MODULE_ID );
		graphicParent = spriteGrapic.transform.parent;
	}
	void Start () {
		mChatInfo = (FriendChatInfo)Facade.GetFacade ().data.Get (FacadeConfig.FRIENDCHAT_MODULE_ID);
		ChatFriendItem.cellNumber = mChatInfo.GetFriendChatNum;
		ChatFriendItem.Refresh ();
		//refresh();
		spriteGrapic.transform.SetSiblingIndex (-1);
	}
	public void CloseBtn(){
		AudioManager._instance.PlayEffectClip(AudioManager.effect_closePanel);
		Destroy (this.gameObject);
	}
	//选中哪个好友聊天
	public void ChooseChater(int friendId){
		chooseID = friendId;
		var tempList= mChatInfo.GetChatList (friendId);
		ChatPanel.cellNumber = tempList.Count;
		ChatPanel.Refresh ();
	}
	public void SendButtonClick(){
		if (!string.IsNullOrEmpty(input.text)) {
			Debug.LogError (input.text);
			//Facade.GetFacade ().message.friend.SendChatMessage (input.text);
			FiChatMessage temp = new FiChatMessage ();
			temp.userId = myInfo.userID;
			temp.message = input.text;
			input.text = "";
			mChatInfo.GetChatList (chooseID).Add (temp);

			if (PrefabManager._instance != null)
			{
				//Debug.LogError("ShowChat:"+temp.message);
				PrefabManager._instance.GetLocalGun().gunUI.ShowChatBubbleBox(temp.message, 3f);
			}

			RefreshList ();
		}
	}
	public void RefreshList(){
		spriteGrapic.transform.SetParent (transform);
		ChatPanel.cellNumber = mChatInfo.GetChatList (chooseID).Count;
		ChatPanel.Refresh ();
		spriteGrapic.transform.SetParent (graphicParent);
		//if (content.sizeDelta.y > 202.5f) {
			//content.localPosition = Vector3.up *125+Vector3.up * (scroll.cellNumber - 3) * 67.5f+Vector3.right*-241.9f;
		//}
	}
}
