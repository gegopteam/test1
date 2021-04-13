using UnityEngine;
using System.Collections;
using  UnityEngine.UI;
using AssemblyCSharp;
/// <summary>
/// User interface game. 子弹赛、限时赛
/// </summary>

class PKRoomRuleType
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

public class UIGame : MonoBehaviour 
{
	
	private Button[] bulletButton;
	//private GameObject WindowClone;
	private int goldType = 0;

	public GameObject primaryGame;
	public GameObject middleGame;
	public GameObject highGame;

	public GameObject lightPrimary;
	public GameObject lightMiddle;
	public GameObject lightHigh;

	private MyInfo myInfo;
	private int goldMuch = 1000;

	// Use this for initialization
	void Start () {
		
		bulletButton = GetComponentsInChildren<Button> ();
		Debug.Log (bulletButton.Length);
		for (int i = 0; i < bulletButton.Length; i++) {
			EventTriggerListener.Get(bulletButton [i].gameObject).onClick = ButtonClick;
		}

		//默认初级房设置
		OnEnterLowRoom ();
		OneMoreGame ();
	}

	void OneMoreGame ()
	{
		myInfo = DataControl.GetInstance ().GetMyInfo ();
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
			if ( myInfo.gold >= goldMuch ) {
				SendEnterMessage ();
			} else {
				GameObject Window = Resources.Load ( "Window/WindowTips" )as GameObject;
				GameObject WindowClone = Instantiate (Window);
				UITipClickHideManager ClickTips = WindowClone.GetComponent<UITipClickHideManager> ();
				ClickTips.text.text = "金幣不足";
			}
		}
		myInfo.oneMoreGame = false;
	}

	void SendEnterMessage()
	{
		//int roomtype = 0;
		MyInfo nInfo = (MyInfo)Facade.GetFacade ().data.Get ( FacadeConfig.USERINFO_MODULE_ID );
		//int roomtype = nInfo.PK_EnterRoomType;// = UIRule.TIME;
		//Debug.Log ("roomtype" + roomtype + "goldType" + goldType);
		Facade.GetFacade ().message.fishPkRoom.SendPKEnterRoomRequest ( nInfo.PK_EnterRoomType , goldType );
		//GameObject Window = Resources.Load ("Window/ReadyWindow") as GameObject;
		//Instantiate (Window);
	}

	//进入初级场
	void OnEnterLowRoom()
	{
		goldType = PKRoomRuleType.GOLD_TYPE_LOW ;
		goldMuch = PKRoomRuleType.GOLD_COUNT_LOW;// 10000;
	
		lightHigh.SetActive (false);
		lightMiddle.SetActive (false);
		lightPrimary.SetActive (true);
		middleGame.gameObject.SetActive (false);
		highGame.gameObject.SetActive (false);
		primaryGame.gameObject.SetActive (true);
	}


	void OnEnterMiddleRoom()
	{
		goldType = PKRoomRuleType.GOLD_TYPE_MIDDLE;
		goldMuch = PKRoomRuleType.GOLD_COUNT_MIDDLE;// 100000;
		lightHigh.SetActive (false);
		lightPrimary.SetActive (false);
		lightMiddle.SetActive (true);
		primaryGame.gameObject.SetActive (false);
		highGame.gameObject.SetActive (false);
		middleGame.gameObject.SetActive (true);
	}

	void OnEnterHighRoom()
	{
		goldType = PKRoomRuleType.GOLD_TYPE_HIGH;
		goldMuch = PKRoomRuleType.GOLD_COUNT_HIGH ;
		lightPrimary.SetActive (false);
		lightMiddle.SetActive (false);
		lightHigh.SetActive (true);
		primaryGame.gameObject.SetActive (false);
		middleGame.gameObject.SetActive (false);
		highGame.gameObject.SetActive (true);
	}

	void ButtonClick(GameObject go)
	{
		//Debug.Log (go.name);
		//Debug.LogError ( "---------ButtonClick---------" + DataControl.GetInstance ().GetMyInfo ().gold );
		switch (go.name) {
		case "Primary":
			OnEnterLowRoom ();
			break;
		case "Middle":
			OnEnterMiddleRoom ();
			break;
		case "High":
			OnEnterHighRoom ();
			break;
		case "ExitButton":
			middleGame.gameObject.SetActive (false);
			highGame.gameObject.SetActive (false);
			primaryGame.gameObject.SetActive (true);
			Destroy (this.gameObject);
			break;
		case "Rule":
			string path = "Window/RuleWindow";
			AppControl.OpenWindow (path);
			break;
		case "JoinIn":
			//弹出允许加入的弹窗
			//判断金币是否满足要求
			if (myInfo.gold >= goldMuch) {
				SendEnterMessage ();
			}
			else {
				GameObject Window = Resources.Load ( "Window/WindowTips" )as GameObject;
				GameObject WindowClone = Instantiate (Window);
				UITipClickHideManager ClickTips = WindowClone.GetComponent<UITipClickHideManager> ();
				ClickTips.text.text = "金幣不足";
			}
			break;
		}
	}

	// Update is called once per frame
	void Update () {

	}

	void OnDestroy()
	{
		
	}
}
