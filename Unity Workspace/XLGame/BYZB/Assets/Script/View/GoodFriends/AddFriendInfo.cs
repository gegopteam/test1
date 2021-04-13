using UnityEngine;
using System.Collections.Generic;
using UnityEngine.UI;
using AssemblyCSharp;
/// <summary>
/// Add friend info.添加好友的信息
/// </summary>

public class AddFriendInfo : MonoBehaviour {
	int ID=0;
	public Image userIcon;
	public Image vip;
	public Text nickName;
	Button btn;
	AvatarInfo nAvaInfo;
	Sprite tempHead;
	// Use this for initialization
	void Awake(){
		tempHead = userIcon.sprite;
	}
	void Start () {
		//userIcon = transform.Find ("UserIcon").GetComponent<Image> ();
		//vipText = transform.Find ("VIP").GetComponentInChildren<Text>();
		//nickName = transform.Find ("Nickname").GetComponent<Text> ();
		//btn=transform.GetChild(3).GetComponent<Button>();
		//btn.onClick.AddListener (ButtonClick);
	}

	public void ButtonClick()
	{
		//向服务器发送添加好友的请求消息，如果服务器允许添加在回复中将按钮变灰
		if (ID == 0) {
			string path = "Window/WindowTipsThree";
			GameObject WindowClone = AppControl.OpenWindow (path);
			WindowClone.SetActive (true);
			UITipAutoNoMask ClickTips1 = WindowClone.GetComponent<UITipAutoNoMask> ();
			ClickTips1.tipText.text = "錯誤的id!";
		} else {
			FriendInfo nInfo = (FriendInfo)Facade.GetFacade ().data.Get ( FacadeConfig.FRIEND_MODULE_ID );
			List<FiFriendInfo> nInfoList = nInfo.GetFriendList ();
			int nInputTxt = ID;
			for (int i = 0; i < nInfoList.Count; i++) 
			{
				if ( nInputTxt == nInfoList [i].gameId )
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

	public void  RefreshView(FiRankInfo data){
		if (data != null) {
			ID = (int) data.gameId;
			nAvaInfo = (AvatarInfo)Facade.GetFacade ().data.Get (FacadeConfig.AVARTAR_MODULE_ID);
			nAvaInfo.Load ((int)data.userId, data.avatarUrl, (int nResult, Texture2D nTexture) => {
				if (userIcon != null) {
					if (nResult == 0) {
						//Debug.LogError (UserAvatar +  "-----22-----" + currentUserId + "/"+nTexture + "/" + UserIcon  + "-----" + UserIcon.GetComponentInParent<Image>() );
						userIcon.sprite = Sprite.Create (nTexture, new Rect (0, 0, nTexture.width, nTexture.height), new Vector2 (0, 0));
					} else
						userIcon.sprite = tempHead;
				}else
					userIcon.sprite = tempHead;
			});
			nickName.text = Tool.GetName (data.nickname, 6);
			vip.sprite = UIHallTexturers.instans.VipFrame [data.vipLevel];
		} else {
			userIcon.sprite = tempHead;
			nickName.text = "guest";
		}
	}
}
