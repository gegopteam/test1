using UnityEngine;
using System.Collections;
using AssemblyCSharp;
using UnityEngine.UI;
public class FriendItem : ScrollableCell {
	FiFriendInfo dataInfo;
	public Image head;
	public Text nickname;
	public Text level;
	public Image vipLevel;
	public GameObject VIP;
	FriendChatInfo mChatInfo ;
	private Object[] num;
	AvatarInfo nAvaInfo;
	void Awake(){
	}
	public override void ConfigureCellData ()
	{
		base.ConfigureCellData ();
		if(dataObject!=null){
			mChatInfo= (FriendChatInfo)Facade.GetFacade ().data.Get (FacadeConfig.FRIENDCHAT_MODULE_ID);
			dataInfo = mChatInfo.GetFriendInfo ((int)dataObject);
			nickname.text = dataInfo.nickname;
			level.text = dataInfo.level.ToString();

			if (dataInfo.vipLevel == 0) {
				VIP.SetActive (false);
			} else {
				VIP.SetActive (true);
				vipLevel.sprite = UIHallTexturers.instans.RankNum [dataInfo.vipLevel];
			}


			nAvaInfo = ( AvatarInfo )Facade.GetFacade ().data.Get ( FacadeConfig.AVARTAR_MODULE_ID );
			nAvaInfo.Load ( dataInfo.userId , dataInfo.avatar , (int nResult, Texture2D nTexture)=>{
				if (nResult == 0 ) {
					//Debug.LogError (UserAvatar +  "-----22-----" + currentUserId + "/"+nTexture + "/" + UserIcon  + "-----" + UserIcon.GetComponentInParent<Image>() );
					nTexture.filterMode = FilterMode.Bilinear;
					nTexture.Compress (true);
					head.sprite = Sprite.Create (nTexture, new Rect (0, 0, nTexture.width, nTexture.height), new Vector2 (0, 0));
				}
			}
			);

			this.GetComponent<Button> ().onClick.AddListener (()=>{FriendChatManager.instance.ChooseChater (dataInfo.userId);});
		}
	}
}
