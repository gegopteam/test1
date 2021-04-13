using UnityEngine;
using System.Collections.Generic;
using AssemblyCSharp;
using UnityEngine.UI;
using System.Collections;
using System;

public class RankFriend : MonoBehaviour {
	public Image head;
	public GameObject VIP;
	public Image VipLevel;
	public Text Level;
	public Image sex;
	public Text name;
	public Text gun;
	public Text userID;
	private UnityEngine.Object[] num;
	public GameObject addFriend;
	void Awake(){
	}

	// Use this for initialization
	void Start () {
	
	}
	public void CloseClick(){
		//Resources.UnloadAsset (head.sprite.texture);
		Destroy (this.gameObject);
		UIColseManage.instance.CloseUI ();
	
	}
	public  void SetInfo(FiRankInfo nInfo){
		if ( !string.IsNullOrEmpty( nInfo.avatarUrl) )
		{
			AvatarInfo nAvaInfo = ( AvatarInfo )Facade.GetFacade ().data.Get ( FacadeConfig.AVARTAR_MODULE_ID );
			nAvaInfo.Load ( (int)nInfo.userId , nInfo.avatarUrl ,(int nResult, Texture2D nTexture)=>{
				if (nResult == 0 && head != null) {
					nTexture.filterMode = FilterMode.Bilinear;
					nTexture.Compress (true);
					head.sprite = Sprite.Create (nTexture, new Rect (0, 0, nTexture.width, nTexture.height), new Vector2 (0, 0));
				}
			}
			);
		}
		if (nInfo.vipLevel == 0) {
			//VIP.SetActive (true);
			VIP.GetComponent<Image>().sprite=UIHallTexturers.instans.VipFrame[0];
		} else {
			//VIP.SetActive (true);
			VIP.GetComponent<Image> ().sprite = UIHallTexturers.instans.VipFrame [nInfo.vipLevel];
			//VipLevel.sprite = UIHallTexturers.instans.RankNum [nInfo.vipLevel];//(Sprite)num [nInfo.vipLevel+1];
		}
		if (nInfo.gender == 1) {
			sex.sprite = UIHallTexturers.instans.Ranking[7];
		} else  {
			sex.sprite = UIHallTexturers.instans.Ranking[2];
		} 
		name.text =Tool.GetName( nInfo.nickname,10);
		//最大炮倍数目前没有数据 
		gun.text=nInfo.maxMultiple.ToString();
		//等级目前没有数据  
		Level.text=nInfo.level.ToString();
		userID.text=nInfo.gameId.ToString();
		if (nInfo.userId == (long)DataControl.GetInstance ().GetMyInfo ().userID)
			addFriend.SetActive (false);
	}
	public void AddFriendButtonClick(){
		string text = userID.text;
		Debug.Log ("添加好友"+int.Parse(text));
		FriendInfo nInfo = (FriendInfo)Facade.GetFacade ().data.Get ( FacadeConfig.FRIEND_MODULE_ID );
		List<FiFriendInfo> nInfoList = nInfo.GetFriendList ();

		int nInputTxt = 0;
		try{
			nInputTxt = int.Parse ( text );
		}catch( Exception e ){

		}

		if (nInputTxt == 0) {
			GameObject Window =  UnityEngine.Resources.Load ("Window/WindowTips")as UnityEngine.GameObject;
			GameObject WindowClone =  UnityEngine.GameObject.Instantiate (Window);
			UITipClickHideManager ClickTips = WindowClone.GetComponent<UITipClickHideManager> ();
			ClickTips.time.text = "錯誤的id";
			return;
		}


		for (int i = 0; i < nInfoList.Count; i++) 
		{
			if ( nInputTxt == nInfoList [i].userId )
			{
				GameObject Window = UnityEngine.Resources.Load ("Window/WindowTipsThree")as UnityEngine.GameObject;
				GameObject WindowClone = UnityEngine.GameObject.Instantiate (Window);
				UITipAutoNoMask ClickTips1 = WindowClone.GetComponent<UITipAutoNoMask> ();
				ClickTips1.tipText.text = "當前ID已經是你的好友了!";
				return;
			}
		}

		MyInfo myinfo = (MyInfo)Facade.GetFacade ().data.Get ( FacadeConfig.USERINFO_MODULE_ID );
		if ( nInputTxt == myinfo.userID ) {
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
