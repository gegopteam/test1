using System;
using UnityEngine;
using UnityEngine.UI;
using AssemblyCSharp;

public class UIUserInfo : MonoBehaviour
{
	public Text UserName;

	public Text UserLevel;

	public GameObject UserExpObject;

	public Image UserAvatar;

	public Text VipLevel;

	public static UIUserInfo instance = null;

	public UIUserInfo ()
	{
	}

	void Awake()
	{
		instance = this;
	}

	void OnDestroy()
	{
		instance = null;
	}
		
	public void SetVipLevel( int nLevel )
	{
		VipLevel.text = nLevel + "";
	}

	void Start()
	{
		MyInfo nInfo =(MyInfo) Facade.GetFacade ().data.Get ( FacadeConfig.USERINFO_MODULE_ID );

		UserName.text =Tool.GetName( nInfo.nickname,6);// "你好呀";
		UserLevel.text = nInfo.level + "";// "100";
		Slider nExpSilder = UserExpObject.GetComponentInChildren<Slider> ();
		nExpSilder.value = (float )nInfo.experience /  (float )nInfo.nextLevelExp;
		//Debug.LogError ( nExpSilder.value + "/" + nInfo.experience +"/"+ nInfo.nextLevelExp );

		VipLevel.text = nInfo.levelVip.ToString();

		//添加头像
		if ( !string.IsNullOrEmpty( nInfo.avatar) )
		{
			AvatarInfo nAvaInfo = ( AvatarInfo )Facade.GetFacade ().data.Get ( FacadeConfig.AVARTAR_MODULE_ID );
			nAvaInfo.Load ( nInfo.userID , nInfo.avatar ,(int nResult, Texture2D nTexture)=>{
				if (nResult == 0 && UserAvatar != null) {
					nTexture.filterMode = FilterMode.Bilinear;
					nTexture.Compress (true);
					UserAvatar.sprite = Sprite.Create (nTexture, new Rect (0, 0, nTexture.width, nTexture.height), new Vector2 (0, 0));
				}
			}
			);

		}
	}

}

