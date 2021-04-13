using UnityEngine;
using System.Collections.Generic;
using UnityEngine.UI;
using AssemblyCSharp;
/// <summary>
/// Apply friend info.申请好友的信息
/// </summary>

public class ApplyFriendInfo : ScrollableCell
{
	public int useId;
	public Image userIcon;
	public Text vipText;
	public Text nickName;
	public Image sex;
	public GameObject Vip;
	public Image VipLevel;
	private GameObject WindowClone;
	private Button[] btns;
	//private FriendMsgHandle friendMsgHandle = new FriendMsgHandle ();
	//public static int friendLimit;
	private FriendInfo nInfo;
	private List<FiFriendInfo> nInfoList;
	string mAvatarUrl;
	private Object[] num;

	Sprite tempHead;
	public void SetAvatarInfo( string nUrl )
	{
		mAvatarUrl = nUrl;	
		AvatarInfo nAvInfo =(AvatarInfo) Facade.GetFacade ().data.Get ( FacadeConfig.AVARTAR_MODULE_ID );
		nAvInfo.Load ( useId , mAvatarUrl , OnAvatarComplete );
	}

	void OnAvatarComplete( int nResult , Texture2D nImage )
	{
		if (nResult == 0) {
			nImage.filterMode = FilterMode.Bilinear;
			nImage.Compress (true);
			userIcon.sprite = Sprite.Create (nImage, new Rect (0, 0, nImage.width, nImage.height), new Vector2 (0, 0));
		} else
			userIcon.sprite = tempHead;
	}

	// Use this for initialization
	void Awake ()
	{
		//UIHallMsg.GetInstance ().friendMsgHandle.AcceptAndRefuseFriendEvent += Des;
		//userIcon = transform.Find ("UserIcon").GetComponent<Image> ();
		vipText = transform.Find ("VIP").GetComponentInChildren<Text> ();
		nickName = transform.Find ("NickName").GetComponent<Text> ();
		sex = transform.Find ("Sex").GetComponent<Image> ();
		btns = GetComponentsInChildren<Button> ();
		for (int i = 0; i < btns.Length; i++) {
			EventTriggerListener.Get (btns [i].gameObject).onClick = ClickcallBack;
		}
		Debug.Log (btns.Length);
		//Init ();//初始化好友申请的信息
		tempHead=userIcon.sprite;
	}

	void ClickcallBack (GameObject go)
	{
		switch ( go.name ) 
		{

		case "Refuse":
			//拒绝添加其为好友，并向服务器发送
			Debug.Log ("拒绝申请");
			Debug.Log ("UseId" + useId);
			FriendInfo nInfo = (FriendInfo)Facade.GetFacade ().data.Get ( FacadeConfig.FRIEND_MODULE_ID );
			nInfo.DeleteApply ( useId );
			Facade.GetFacade ().message.friend.SendRefuseFriendApply ( useId );
			//friendMsgHandle.SendRefuseFriendApply (useId);
			//Des (useId);
			ApplyManager.applyBackage.cellNumber = nInfo.getApplyFriends().Count;
			ApplyManager.applyBackage.Refresh ();
			if (nInfo.getApplyFriends().Count > 0)
				UIGoodFriends.instans.redPoint.SetActive (true);
			else
				UIGoodFriends.instans.redPoint.SetActive (false);
			break;
		case "Add":
//			Debug.Log ("FriendLimit"+friendLimit);
//			//判断当前好友的数量是否已经超过当前等级的好友数量，如果超过则提示不可以添加
//			//如果没有超过则添加向服务器发送添加该好友，并将其放入好友GoodList中的GameFriendKlist中
//			if (Goodlist.GameFriendList.Count >= friendLimit) {
//				string path = "Window/MutipleWindowTips";
//				WindowClone = AppControl.OpenWindow (path);
//				WindowClone.SetActive (true);
//				//弹出VIP等级不足，不可以再加好友
//			} else {
//
//			}
			Debug.Log ("接收申请");
			Facade.GetFacade ().message.friend.SendAcceptFriend (useId);
			//Des (useId);
			break;
		}
	}

	void Refuse()
	{
	}

	void Des (int UseId)
	{
		if (UseId == useId) {
			Destroy (this.gameObject);
		}
	}

	void OnDestroy ()
	{
		
	}
	public override void ConfigureCellData ()
	{
		base.ConfigureCellData ();

		nInfo = (FriendInfo)Facade.GetFacade ().data.Get (FacadeConfig.FRIEND_MODULE_ID);
		nInfoList = nInfo.getApplyFriends ();


		if (dataObject != null) {
			FiFriendInfo nFriInfo = nInfoList [(int)dataObject];
			userIcon.sprite = tempHead;
			useId = nFriInfo.userId;
			nickName.text =Tool.GetName( nFriInfo.nickname,6);
			SetAvatarInfo (nFriInfo.avatar);
			if (nFriInfo.gender!=1) {
				sex.sprite = UIHallTexturers.instans.Ranking [2];// Resources.Load ("Ranking/女", typeof(Sprite))as Sprite;
			} else {
				sex.sprite = UIHallTexturers.instans.Ranking [7];//Resources.Load ("Ranking/男", typeof(Sprite))as Sprite;
			}
			vipText.text = nFriInfo.level.ToString ();

			if (nFriInfo.vipLevel == 0) {
				//Vip.SetActive (false);
				Vip.GetComponent<Image>().sprite=UIHallTexturers.instans.VipFrame[0];
			} else {
				//Vip.SetActive (true);
				Vip.GetComponent<Image>().sprite=UIHallTexturers.instans.VipFrame[nFriInfo.vipLevel];
				//VipLevel.sprite = UIHallTexturers.instans.RankNum [nFriInfo.vipLevel];
			}
		}
	}
}
