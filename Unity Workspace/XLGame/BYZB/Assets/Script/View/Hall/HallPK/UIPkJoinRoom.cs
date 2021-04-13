using UnityEngine;
using System.Collections;
using  UnityEngine.UI;
using AssemblyCSharp;
/// <summary>
/// User interface game. 子弹赛、限时赛
/// </summary>

class PKRuleType
{
	public const int ROOM_BULLET     = 4;

	public const int ROOM_TIME       = 5;

	public const int ROOM_COMBAT     = 6;

	public const int FRIEND_ROOM_CARD   = 11;

	public const int FRIEND_ROOM_GOLD   = 10;

	public const int GOLD_COUNT_LOW     = 1000;

	public const int GOLD_COUNT_MIDDLE  = 10000;

	public const int GOLD_COUNT_HIGH    = 100000;

	public const int GOLD_COUNT_MASTER  = 1000000;

	public const int GOLD_TYPE_LOW    = 0;

	public const int GOLD_TYPE_MIDDLE = 1;

	public const int GOLD_TYPE_HIGH   = 2;

	public const int GOLD_TYPE_MASTER = 3;
}

public class UIPkJoinRoom : MonoBehaviour 
{

	public Sprite[] IconRoomType;

	public Sprite[] IconLevelType;

	public Text TxtRoomType;

	public Image IconTitle;

	public Image IconLevel;

	private Button[] bulletButton;

	private int mLevelType = PKRuleType.GOLD_TYPE_LOW;

	public Image[] ImgSelectedBack;

	public GameObject btnLow;
	public GameObject btnMiddle;
	public GameObject btnHigh;
	GameObject mSelectButton;

	public Text TxtGoldCost;

	public Text TxtUserNeed;

	//private MyInfo myInfo;
	private int mGoldCost = PKRuleType.GOLD_COUNT_LOW;

	int mRoomType = PKRuleType.ROOM_BULLET;

	public void SetRoomType( int nType )
	{
		TxtUserNeed.text = "4";
		if (nType == PKRuleType.ROOM_BULLET)
		{
			// IconTitle.sprite = IconRoomType [0];
			TxtRoomType.text = "子彈賽";
		}
		else if (nType == PKRuleType.ROOM_TIME)
		{
			// IconTitle.sprite = IconRoomType [1];
			TxtRoomType.text = "限時賽";
		}
		else
		{
			// IconTitle.sprite = IconRoomType [2];
			TxtRoomType.text = "對抗賽";
			TxtUserNeed.text = "2";
		}
		mRoomType = nType;
	}

	// Use this for initialization
	void Start () {
		SetRoomType (mRoomType);
		//默认初级房设置
		OnEnterLowRoom ();

		OneMoreGame ();
	}

	void OneMoreGame ()
	{
		MyInfo myInfo = DataControl.GetInstance ().GetMyInfo ();
		if (myInfo.oneMoreGame) 
		{
			int nLastLevel = myInfo.lastGame.level;
			Debug.LogError ( "[ UiGame ] restart game agian !!!!!! " + nLastLevel + "++++");
			switch ( nLastLevel ) 
			{
			case 0:
				
				OnEnterLowRoom ();
				break;
			case 1:
				
				OnEnterMiddleRoom ();
				break;
			case 2:
				
				OnEnterHighRoom ();
				break;
			}

			OnStartGame ();
//			if ( myInfo.gold >= mGoldCost ) {
//				SendEnterMessage ();
//			} else {
//				GameObject Window = Resources.Load ( "Window/WindowTips" )as GameObject;
//				GameObject WindowClone = Instantiate (Window);
//				UITipClickHideManager ClickTips = WindowClone.GetComponent<UITipClickHideManager> ();
//				ClickTips.text.text = "金币不足";
//			}
		}
		myInfo.oneMoreGame = false;
	}

	public void OnStartGame()
	{
		//弹出允许加入的弹窗
		//判断金币是否满足要求
		MyInfo nInfo = (MyInfo)Facade.GetFacade().data.Get( FacadeConfig.USERINFO_MODULE_ID );
		if ( nInfo.gold >= mGoldCost ) {
			Facade.GetFacade ().message.fishPkRoom.SendPKEnterRoomRequest ( mRoomType , mLevelType );
		}else {
			GameObject Window = Resources.Load ( "Window/WindowTips" )as GameObject;
			GameObject WindowClone = Instantiate (Window);
			UITipClickHideManager ClickTips = WindowClone.GetComponent<UITipClickHideManager> ();
			ClickTips.text.text = "金幣不足";
		}
	}

	void ShowSelectBack( int nIndex )
	{
		for( int i = 0 ; i < ImgSelectedBack.Length ; i ++ ){
			if (nIndex != i) {
				ImgSelectedBack [i].gameObject.SetActive (false);
			} else {
				ImgSelectedBack [ i ].gameObject.SetActive ( true );
			}
		}
	}

	//进入初级场
	public void OnEnterLowRoom()
	{
		ShowSelectBack ( 0 );

//		if (mSelectButton) {
//			mSelectButton.transform.FindChild ("Border").gameObject.SetActive (false);
//		}
//		mSelectButton = btnLow;
//		mSelectButton.transform.FindChild ("Border").gameObject.SetActive(true);
//		IconLevel.sprite = IconLevelType[ 0 ];
		mLevelType = PKRoomRuleType.GOLD_TYPE_LOW ;
		mGoldCost = PKRoomRuleType.GOLD_COUNT_LOW;// 10000;
		TxtGoldCost.text = mGoldCost.ToString();
	}


	public void OnEnterMiddleRoom()
	{
		ShowSelectBack ( 1 );
//		if (mSelectButton) {
//			mSelectButton.transform.FindChild ("Border").gameObject.SetActive (false);
//		}
//		mSelectButton = btnMiddle;
//		mSelectButton.transform.FindChild ("Border").gameObject.SetActive(true);
		mLevelType = PKRoomRuleType.GOLD_TYPE_MIDDLE;
		mGoldCost = PKRoomRuleType.GOLD_COUNT_MIDDLE;// 100000;
//		IconLevel.sprite = IconLevelType[ 1 ];
		TxtGoldCost.text = mGoldCost.ToString();
	}

	public void OnEnterHighRoom()
	{
		ShowSelectBack ( 2 );
//		if (mSelectButton) {
//			mSelectButton.transform.FindChild ("Border").gameObject.SetActive (false);
//		}
//		mSelectButton = btnHigh;
//		mSelectButton.transform.FindChild ("Border").gameObject.SetActive(true);
		mLevelType = PKRoomRuleType.GOLD_TYPE_HIGH;
		mGoldCost = PKRoomRuleType.GOLD_COUNT_HIGH ;
//		IconLevel.sprite = IconLevelType[ 2 ];
		TxtGoldCost.text = mGoldCost.ToString();
	}

	public void OnOpenRule()
	{
		string path = "PkHall/RuleWindow";
		GameObject nEntity = AppControl.OpenWindow (path);
		nEntity.GetComponentInChildren<UIRule> ().SetRuleType( mRoomType );
	}

	public void OnClose()
	{
		Destroy ( gameObject );
	}

	void OnDestroy()
	{
		
	}
}
