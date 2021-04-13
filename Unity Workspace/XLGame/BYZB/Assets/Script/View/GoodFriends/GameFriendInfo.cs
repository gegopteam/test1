using UnityEngine;
using System.Collections.Generic;
using UnityEngine.UI;
using AssemblyCSharp;
/// <summary>
/// Game friend info.显示游戏好友的信息	,有多少个好友，实例化多少个
/// </summary>
public class GameFriendInfo : ScrollableCell {
	
	public Image userIcon;
	public Text vipText;
	public Text nickName;
	public Image sex;
	public int useId;

	private GameObject WindowClone;
	private Button[] btns;
	private FriendInfo mFriInfo;
	private List<FiFriendInfo> nInfoList;
	string AvatarUrl;
	Material material;
	GameObject Windows;
	public GameObject Vip;
	public Image VipLevel;
	private Object[] num;
	UITipAutoHideManager ClickTips;
	Sprite tempHead;
	public void SetAvatarInfo( string nUrl )
	{
		AvatarUrl = nUrl;	
		AvatarInfo nAvInfo =(AvatarInfo) Facade.GetFacade ().data.Get ( FacadeConfig.AVARTAR_MODULE_ID );
		nAvInfo.Load ( useId , AvatarUrl , OnAvatarComplete );
	}

	void OnAvatarComplete( int nResult , Texture2D nImage )
	{
		if (nResult == 0) {
			nImage.filterMode = FilterMode.Bilinear;
			nImage.Compress (true);
			userIcon.sprite = Sprite.Create (nImage, new Rect (0, 0, nImage.width, nImage.height), new Vector2 (0, 0));
		} else {
			userIcon.sprite = tempHead;
		}
	}

	// Use this for initialization
	void Awake () {
	    //userIcon = transform.Find ("UserIcon").GetComponent<Image> ();
		vipText = transform.Find ("VIP").GetComponentInChildren<Text> ();
		nickName = transform.Find ("NickName").GetComponent<Text> ();
		sex = transform.Find ("Sex").GetComponent<Image> ();
		btns = GetComponentsInChildren<Button> ();
		material = userIcon.material;
		for (int i = 0; i < btns.Length; i++) {
			EventTriggerListener.Get (btns [i].gameObject).onClick = ClickcallBack;
		}
		tempHead = userIcon.sprite;

		Debug.Log (tempHead);
	}

	void ClickcallBack(GameObject go)
	{
		switch (go.name) {

		case "Talk":
			//判断该好友是否在线，如果不在线弹出该好友不在线不可以聊天，如果在线向服务器发送我要与该好友聊天，弹出聊天框。
			//每次打开这个窗口都需要重新读取好友的信息，显示在线的好友和不在线的好友
			mFriInfo = (FriendInfo)Facade.GetFacade ().data.Get (FacadeConfig.FRIEND_MODULE_ID);
			nInfoList = mFriInfo.GetFriendList ();
			FiFriendInfo nFriendInfo = nInfoList [(int)dataObject];
			if (nFriendInfo.status == 1) {
				FriendChatInfo mChatInfo = (FriendChatInfo)Facade.GetFacade ().data.Get (FacadeConfig.FRIENDCHAT_MODULE_ID);
				mChatInfo.AddChatFriend (nFriendInfo);
				string paths = "Window/FriendChat";
				AppControl.OpenWindow (paths);
				//WindowClone.SetActive (true);
			} else {
				Windows = Resources.Load ("Window/WindowTips")as GameObject;
				GameObject WindowClone = Instantiate (Windows);
				UITipClickHideManager ClickTips = WindowClone.GetComponent<UITipClickHideManager> ();
					ClickTips.text.text = "該好友不在線，無法進行聊天！";
					ClickTips.time.text = "3";
			}
			break;
		case "Tool":
			Debug.Log ("赠送道具");
			//Goodlist.GetInstance().GoodList.Clear ();
			//UIHall.arrayDic.Clear ();
			//backpackInfo.SetRcv (this);
			//backpackInfo.OpenRcvInfo ();
			//其他赠送走背包流程，弹出背包界面，如果不是VIP界面则弹出VIP1解锁道具赠送按钮,赠送成功后按钮变灰
			//string path = "Window/BagWindow";
			string path = "MainHall/BackPack/BagMainWindow";//"Window/BagWindow";
			GameObject nWindow = AppControl.OpenWindow (path);
			//WindowClone = AppControl.OpenWindow (path);
			//WindowClone.SetActive (true);
			nWindow.GetComponentInChildren<UIBackPack> ().DefaultUserId = useId;
			UIGoodFriends.instans.OnButton ();
			break;
		case "Coin":
			//Debug.Log("赠送金币");
			//点击赠送按钮向服务器发送赠送消息，如果已经赠送过则图标显示false
			//coinButton.interactable = false;
			if (useId != 0 && transform.Find ("Coin").GetComponent<Button> ().interactable)
				Facade.GetFacade ().message.backpack.SendGiveOtherPropertyRequest (useId, FiPropertyType.GOLD, 100);
			transform.Find ("Coin").GetComponent<Button> ().interactable = false;

			mFriInfo = (FriendInfo)Facade.GetFacade ().data.Get (FacadeConfig.FRIEND_MODULE_ID);
			nInfoList = mFriInfo.GetFriendList ();
			FiFriendInfo nFriInfo = nInfoList [(int)dataObject];
			nFriInfo.hasGivenGold = true;

			break;
		case "Delete":
			//向服务发送删除该好友的操作，在服务器返回的消息中弹出是否删除该好友，点击确认则删除该好友
			//删除成功则执行删除操作Destroy(this.gameObject);
			Windows = Resources.Load ("Window/WindowTipsSecond")as GameObject;
			WindowClone = GameObject.Instantiate (Windows);
			ClickTips = WindowClone.GetComponent<UITipAutoHideManager> ();
			ClickTips.text.text = "是否確認刪除該好友!";
			ClickTips.useId = useId;
			break;
		}
	}

	public void RcvInfo (object data)
	{
		if (data == null)
			return;
//		backPack = (FiBackpackProperty)data;
//		Goodlist.GetInstance().GoodList.Add (backPack);
//		UIHall.arrayDic.Add (backPack.name, backPack);
//		Debug.Log ("背包数据" + Goodlist.GetInstance ().GoodList.Count);
	}

	void Des(int useid)
	{
		if (useid == useId) {
			Destroy (this.gameObject);
		}
	}

	void OnDestroy()
	{
		//UIHallMsg.GetInstance().friendMsgHandle.DesGridForId -= Des;
	}
	public override void ConfigureCellData ()
	{
		base.ConfigureCellData ();


		mFriInfo = (FriendInfo)Facade.GetFacade ().data.Get (FacadeConfig.FRIEND_MODULE_ID);
		nInfoList = mFriInfo.GetFriendList ();

		if (dataObject != null) {
			FiFriendInfo nFriInfo = nInfoList [(int)dataObject];
			userIcon.sprite = tempHead;
			transform.Find ("Coin").GetComponent<Button> ().interactable = !nFriInfo.hasGivenGold;
			useId = nFriInfo.userId;
			nickName.text =Tool.GetName( nFriInfo.nickname,6);
			SetAvatarInfo (nFriInfo.avatar);
			Debug.LogError ("数据的序号为："+(int)dataObject+"/是否赠送过金币："+nFriInfo.hasGivenGold);
			if (nFriInfo.hasGivenGold) {
				transform.Find ("Coin").GetComponent<Button> ().interactable = false;
			}
			//Debug.LogError ( nFriInfo.nickname + "----------" + nFriInfo.gender );

			if (nFriInfo.gender == 1) {
				sex.sprite = UIHallTexturers.instans.Ranking [7];
			} else {
				sex.sprite = UIHallTexturers.instans.Ranking [2];
			}
			vipText.text = nInfoList [(int)dataObject].level.ToString ();
			//头像置为灰色或高亮，现在接到的status是1  1表示在线
			if (nFriInfo.status == 1)
				userIcon.material = null;
			else
				userIcon.material = material;

			if (nFriInfo.vipLevel == 0) {
				Vip.SetActive (true);
				Vip.GetComponent<Image>().sprite=UIHallTexturers.instans.VipFrame[0];
			} else {
				Vip.SetActive (true);
				Vip.GetComponent<Image> ().sprite = UIHallTexturers.instans.VipFrame [nFriInfo.vipLevel];
				//VipLevel.sprite = UIHallTexturers.instans.RankNum [nFriInfo.vipLevel];
			}
		}
	}
}
