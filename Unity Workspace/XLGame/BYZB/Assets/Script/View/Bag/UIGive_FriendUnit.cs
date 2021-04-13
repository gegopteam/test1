using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using AssemblyCSharp;

public class UIGive_FriendUnit : MonoBehaviour {
	
	public Sprite[] SrcIcon;


	public Image BackGroud;
	public Image IconUser;
	public Text  vip;
	public Image IconSex;
	public Text  TxtNickName;
	public Text  TxtUserId;
	public Image IconSelected;


	public string UserAvatar;
	public int    userId;
	public int    gameId;

	public void SetGender( int nGender )
	{
		//boy
		if (nGender == 1) {
			IconSex.sprite = SrcIcon[ 2 ];
		} else {
			IconSex.sprite = SrcIcon[ 3 ];
		}
	}

	public void SetSelected( bool nSelected )
	{
		IconSelected.gameObject.SetActive ( nSelected );
		/*if (nSelected) {
			BackGroud.sprite = SrcIcon [ 1 ];
			IconSelected.gameObject.SetActive ( true );
		} else {
			BackGroud.sprite = SrcIcon [ 0 ];
			IconSelected.gameObject.SetActive ( false );
		}*/
	}

	void Start()
	{
		//MyInfo nInfo =(MyInfo) Facade.GetFacade ().data.Get ( FacadeConfig.USERINFO_MODULE_ID );
		//添加头像
		if ( !string.IsNullOrEmpty( UserAvatar ) )
		{
			//Debug.LogError ( "!string.IsNullOrEmpty( UserAvatar )" + userId );
			AvatarInfo nAvaInfo = ( AvatarInfo )Facade.GetFacade ().data.Get ( FacadeConfig.AVARTAR_MODULE_ID );
			nAvaInfo.Load ( userId , UserAvatar , (int nResult, Texture2D nTexture)=>{
				if (nResult == 0 ) {
					//Debug.LogError ( "-----22-----" + userId );
					nTexture.filterMode = FilterMode.Bilinear;
					nTexture.Compress (true);
					IconUser.sprite = Sprite.Create (nTexture, new Rect (0, 0, nTexture.width, nTexture.height), new Vector2 (0, 0));
				}
			}
			);
		}
	}

}
