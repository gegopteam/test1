using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using AssemblyCSharp;

public class UICombatGame : MonoBehaviour {

	private Button[] bulletButton;
	//private GameObject WindowClone;
	private int goldType = 0;

	public GameObject primaryGame;
	public GameObject middleGame;
	public GameObject highGame;

	//public static string roomName;
	public GameObject lightPrimary;
	public GameObject lightMiddle;
	public GameObject lightHigh;

	private MyInfo myInfo;
	private int goldMuch = 1000;
	//int level;

	// Use this for initialization
	void Start () 
	{
		bulletButton = GetComponentsInChildren<Button> ();
		Debug.Log (bulletButton.Length);
		for (int i = 0; i < bulletButton.Length; i++) 
		{
			EventTriggerListener.Get(bulletButton [i].gameObject).onClick = ButtonClick;
		}

		SelectLevelLow ();
		OneMoreGame ();
	}

	void SelectLevelLow()
	{
		goldType = 0;
		goldMuch = 1000;
		lightHigh.SetActive (false);
		lightMiddle.SetActive (false);
		lightPrimary.SetActive (true);
		middleGame.gameObject.SetActive (false);
		highGame.gameObject.SetActive (false);
		primaryGame.gameObject.SetActive (true);
	}

	void SelectLevelMiddle()
	{
		goldType = 1;
		goldMuch = 10000;
		lightHigh.SetActive (false);
		lightPrimary.SetActive (false);
		lightMiddle.SetActive (true);
		primaryGame.gameObject.SetActive (false);
		highGame.gameObject.SetActive (false);
		middleGame.gameObject.SetActive (true);
	}

	void SelectLevelHigh()
	{
		goldType = 2;
		goldMuch = 100000;
		lightPrimary.SetActive (false);
		lightMiddle.SetActive (false);
		lightHigh.SetActive (true);
		primaryGame.gameObject.SetActive (false);
		middleGame.gameObject.SetActive (false);
		highGame.gameObject.SetActive (true);
	}

	void OneMoreGame ()
	{
		myInfo = DataControl.GetInstance ().GetMyInfo ();
		if (myInfo.oneMoreGame) 
		{
			//Debug.LogError (level+"++++");

			switch ( myInfo.lastGame.level ) 
			{
			case PKRoomRuleType.GOLD_TYPE_LOW:
				SelectLevelLow ();
				break;
			case PKRoomRuleType.GOLD_TYPE_MIDDLE:
				SelectLevelMiddle ();
				break;
			case PKRoomRuleType.GOLD_TYPE_HIGH:
				SelectLevelHigh ();
				break;
			}

			if ( myInfo.gold >= goldMuch )
			{
				//这个类只在对抗赛中挂载
				int roomtype = PKRoomRuleType.ROOM_COMBAT;
				Debug.Log ("roomtype" + roomtype + "goldType" + goldType);

				//myInfo.PK_SelectLevel = goldType;
				myInfo.PK_EnterRoomType = roomtype;
				Facade.GetFacade ().message.fishPkRoom.SendPKEnterRoomRequest ( roomtype, goldType );
			} else {
				GameObject Window = Resources.Load ( "Window/WindowTips" )as GameObject;
				GameObject nTip = Instantiate (Window);
				UITipClickHideManager ClickTips = nTip.GetComponent<UITipClickHideManager> ();
				ClickTips.text.text = "金幣不足";
			}
		}
		myInfo.oneMoreGame = false;
	}

	void ButtonClick(GameObject go)
	{
		switch (go.name) 
		{
		case "Primary":
			SelectLevelLow ();
			break;
		case "Middle":
			SelectLevelMiddle ();
			break;
		case "High":
			SelectLevelHigh ();
			break;
		case "ExitButton":
//			transform.gameObject.SetActive (false);
//			middleGame.gameObject.SetActive (false);
//			highGame.gameObject.SetActive (false);
//			primaryGame.gameObject.SetActive (true);
			Destroy ( gameObject );
			break;
		case "Rule":
			string path = "Window/RuleWindow";
			GameObject WindowClone1 = AppControl.OpenWindow (path);
			WindowClone1.SetActive (true);
			//弹出规则界面
			break;
		case "JoinIn":
			//弹出允许加入的弹窗
			//要求加入房间
			if ( myInfo.gold >= goldMuch ) {
				int roomtype = PKRoomRuleType.ROOM_COMBAT;
				//myInfo.PK_SelectLevel = goldType;
				myInfo.PK_EnterRoomType = roomtype;
				Facade.GetFacade ().message.fishPkRoom.SendPKEnterRoomRequest ( roomtype, goldType );
			}else {
				GameObject Window = Resources.Load ( "Window/WindowTips" )as GameObject;
				GameObject WindowClone = Instantiate (Window);
				UITipClickHideManager ClickTips = WindowClone.GetComponent<UITipClickHideManager> ();
				ClickTips.text.text = "金幣不足";
			}
			break;
		}
    }
}