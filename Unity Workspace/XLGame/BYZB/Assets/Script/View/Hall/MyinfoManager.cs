using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using AssemblyCSharp;

public class MyinfoManager : MonoBehaviour {
	public Slider LVSlider;
	public Text LvText;
	public Text VipText;
	public Image VipImage;

    public GameObject Vip;

	public Image Head;
	//public Text TimeText;

	private int hour;
	private int minute;
	private int second;

	private MyInfo myInfo;

	void OnRecvAvatarResponse( int nResult , Texture2D nImage )
	{
		if (nResult == 0) {
			Head.sprite = Sprite.Create (nImage, new Rect (0, 0, nImage.width, nImage.height), new Vector2 (0, 0));
		}
	}

	void AddAvatar()
	{
		if (Head != null) 
		{
			AvatarInfo nInfo =(AvatarInfo)  Facade.GetFacade ().data.Get ( FacadeConfig.AVARTAR_MODULE_ID );
			nInfo.Load ( myInfo.userID , myInfo.avatar , OnRecvAvatarResponse );
		}
	}

	// Use this for initialization
	void Awake () {
		myInfo = DataControl.GetInstance ().GetMyInfo ();

		AddAvatar ();

		if( LvText != null )
		    LvText.text = myInfo.level.ToString();
		if (myInfo.levelVip >= 1 && myInfo.nextLevelExp != 0 && LVSlider!= null ) {
			LVSlider.value = myInfo.experience / myInfo.nextLevelExp;
		    //Debug.LogError ( "myInfo.nextLevelExp " + myInfo.nextLevelExp + " / " + myInfo.experience );
		}

		if ( myInfo.levelVip != 0 && Vip != null ) {
			Vip.SetActive (true);
			VipText.text = myInfo.levelVip.ToString();
			if( VipImage!= null )
			    VipImage.sprite = Resources.Load ("Image/vip边框", typeof(Sprite))as Sprite;
		} else {
			if( Vip != null )
			    Vip.SetActive (false);
			if( VipImage != null )
			    VipImage.sprite = Resources.Load ("Image/头像框", typeof(Sprite))as Sprite;
		}
	}

	//void Update()
	//{
		//if (TimeText != null) {
			//int hour   = int.Parse (System.DateTime.Now.ToString ("hh"));
			//int minute = int.Parse (System.DateTime.Now.ToString ("mm"));
			//int second = int.Parse (System.DateTime.Now.ToString ("ss"));
			//TimeText.text = System.DateTime.Now.ToString ("hh:mm:ss");
		//}
	//}
}