using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using AssemblyCSharp;
/// <summary>
/// Master ready players.房主信息UI管理
/// </summary>

public class UIPKFriend_UserInfo : MonoBehaviour 
{
	public Image  ImgAvatar;
	public Text   TxtName;
	public Image  ImgGender;
	public Text  TxtLevel;
	public Text  TxtUserId;
	public Image  ImgIsReady;
	public Sprite[] IconGender;


	public int userId;

	public bool isReady()
	{
		return ImgIsReady.gameObject.activeSelf;
	}

	// Use this for initialization
	void Awake () {
		//DisplayPanel = transform.FindChild ("Show");
		ImgAvatar = transform.Find ("ImgAvatar").GetComponent<Image> ();
		TxtName =   transform.Find ("TxtName").GetComponent<Text> ();
		ImgGender =  transform.Find ("ImgGender").GetComponent<Image> ();
		TxtLevel  = transform.Find ("TxtLevelTip").Find("TxtLevelNumber").GetComponent<Text> ();
		TxtUserId = transform.Find ("TxtUserIdTip").Find("TxtUserId").GetComponent<Text> ();
		//ImgCannon = DisplayPanel.FindChild ("CannonImage").FindChild("Cannon").GetComponent<Image>();
		ImgIsReady = transform.Find("IsReady").GetComponent<Image>();
	}

	public void SetReadyState( bool bReady )
	{
		ImgIsReady.gameObject.SetActive ( bReady );
	}

	public void SetOwner(  )
	{
		transform.Find("ImgOwner").GetComponent<Image>().gameObject.SetActive( true );
		SetReadyState (false);
	}

	public void SetUserInfo( FiUserInfo nUser )
	{
		TxtUserId.text = nUser.userId.ToString();
		userId = nUser.userId;
		TxtName.text = nUser.nickName;
		if (nUser.gender == 1) {
			ImgGender.sprite =  IconGender [1];
		} else {
			ImgGender.sprite =  IconGender[ 0 ];
		}

		TxtLevel.text = nUser.level.ToString();
		//添加头像
		if ( !string.IsNullOrEmpty( nUser.avatar ) )
		{
			AvatarInfo nAvaInfo = ( AvatarInfo )Facade.GetFacade ().data.Get ( FacadeConfig.AVARTAR_MODULE_ID );
			nAvaInfo.Load ( nUser.userId , nUser.avatar , (  int nResult , Texture2D nTexture  )=>
				{
					nTexture.filterMode = FilterMode.Bilinear;
					nTexture.Compress (true);
					ImgAvatar.sprite = Sprite.Create (nTexture, new Rect (0, 0, nTexture.width, nTexture.height), new Vector2 (0, 0));
				}
			);
		}

	}

	void Start()
	{
		
	}

	void Init ()
	{

	}

	// Update is called once per frame
	void Update () {

	}
}

