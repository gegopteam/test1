using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using AssemblyCSharp;

public class UIGive_Stranger : MonoBehaviour
{

	//public Image UserIcon;

	public Sprite[] SrcIcon;

	public int currentUserId;

	public Image SexInfo;

	public Text NickName;

	public GameObject UserIcon;

	public Text VipLevel;

	public Text UserId;

	public string UserAvatar;

	public int mUserId;
	public int mGameId;

	bool isInited = false;

	public void DoRefresh()
	{
		if (!isInited)
			return;

		//Debug.LogError ( "----------do referesh----------" );
	    Start ();
	}

	public void SetGender( int nValue )
	{
		if (nValue == 1) {
			SexInfo.sprite = SrcIcon[ 1 ];
		} else {
			SexInfo.sprite = SrcIcon[ 0 ];
		}
	}


	void Start()
	{
		isInited = true;
		//MyInfo nInfo =(MyInfo) Facade.GetFacade ().data.Get ( FacadeConfig.USERINFO_MODULE_ID );
		//添加头像
		//Debug.LogError ( "!string.IsNullOrEmpty( UserAvatar )" + currentUserId + "-----" + UserAvatar );
		if (!string.IsNullOrEmpty (UserAvatar)) {
			//Debug.LogError ( "2    " + currentUserId + "-----" + UserAvatar );
			AvatarInfo nAvaInfo = (AvatarInfo)Facade.GetFacade ().data.Get (FacadeConfig.AVARTAR_MODULE_ID);
			nAvaInfo.Load (currentUserId, UserAvatar, (int nResult, Texture2D nTexture) => {
				if (nResult == 0) {
					//Debug.LogError (UserAvatar +  "-----22-----" + currentUserId + "/"+nTexture + "/" + UserIcon  + "-----" + UserIcon.GetComponentInParent<Image>() );
					nTexture.filterMode = FilterMode.Bilinear;
					nTexture.Compress (true);
					UserIcon.GetComponentInChildren<Image> ().sprite = Sprite.Create (nTexture, new Rect (0, 0, nTexture.width, nTexture.height), new Vector2 (0, 0));
				}
			}
			);
		} else {
			UserIcon.GetComponentInChildren<Image> ().sprite = SrcIcon[ 2 ];
		}
	}

}

