using UnityEngine;
using System.Collections;
using System.Collections.Generic;

using DG.Tweening;
using AssemblyCSharp;

public enum GameType
{
	Classical,
	Bullet,
	Time,
	Point,
}

public class GameController : MonoBehaviour
{

	public static GameController _instance = null;

	public bool gameIsReady = false;
	public bool gameIsEnd = false;

	public static int enemyLayer;
	public bool isDebugMode = false;

	public  GameType myGameType = GameType.Classical;
	public bool isFriendMode = false;
	public bool isRedPacketMode = false;
	/// <summary>
	/// Boss场
	/// </summary>
	public bool isBossMode = false;
	/// <summary>
	/// Boss匹配场
	/// </summary>
	public bool isBossMatchMode = false;
	//体验场
	public bool isExperienceMode = false;
	public bool isFishTideComing1 = false;
	//提前30秒通知鱼潮要来时对应的flag
	public bool isFishTideComing2 = false;
	//提前7秒通知鱼潮来时对应的flag，此时要执行清场

	//public int fishTideIndex = 0;//0说明不是鱼潮，1，2，3对应不同编号的鱼潮

	Transform uiCanvas;

	public int roomMultiple = 1;

	public Sprite[] bgGroup;
	public SpriteRenderer bgSpriteRenderer;

	public bool isOverTurn = false;

	//判断是否有鱼雷
	public bool isHaveTorpedo = false;

	void Awake ()
	{
		//if (null != _instance)
		//Destroy(GameController._instance.gameObject);
		_instance = this;
		if (DataControl.GetInstance ().GetMyInfo ().seatIndex >= 3) {
			isOverTurn = true;
		} else {
			isOverTurn = false;
		}
		//Debug.LogError ("GameController Init.isOverturn=" + isOverTurn);
		Application.targetFrameRate = 30; //安卓强制30帧以保证流畅
		Application.runInBackground = true; //设置后台继续运行
		uiCanvas = GameObject.FindGameObjectWithTag (TagManager.uiCanvas).transform;

		RoomInfo myRoomInfo = DataControl.GetInstance ().GetRoomInfo ();
//		Debug.LogError ("myRoomInfo.roomMultiple = " + myRoomInfo.roomMultiple);
		bgSpriteRenderer.sprite = bgGroup [myRoomInfo.roomMultiple];
		switch (myRoomInfo.roomMultiple) { //0,1,2分别代表新手场，30倍场，100倍场,如果是红包场，不过roomMultiple多少最后都强制为300
		case 0:
			roomMultiple = 1;
			break;
		case 1:
			roomMultiple = 50;
			break;
		case 2:
			roomMultiple = 1000;
			break;
		case 3:
			isBossMode = true;
			roomMultiple = 1000;
			break;
		case 4:
			isExperienceMode = true;
			roomMultiple = 1;
			break;
		case 5:
			isBossMatchMode = true;
			roomMultiple = 1000;
			break;
		default:
			break;
		}


		//Debug.LogError ("RoomMultiple:" + roomMultiple);
		Bullet.bulletSumNum = 0;
		//myRoomInfo.roomType = 4;
		//Debug.LogError("RoomType:"+ myRoomInfo .roomType);
		switch (myRoomInfo.roomType) {
		case 0:
			myGameType = GameType.Classical;
			if (AudioManager._instance != null)
				AudioManager._instance.PlayBgm (AudioManager.bgm_classical);
		//	gameIsReady = true; //只有普通场可以马上GameReady，其它pk场要通过PrefabManager的倒计时结束以后才会GameReady
			Invoke ("SetGameReady", 1f);
			break;
		case 1:
			myGameType = GameType.Classical;
			isRedPacketMode = true;
			roomMultiple = 300;
			AudioManager._instance.PlayBgm (AudioManager.bgm_classical);
		//	gameIsReady = true;
			Invoke ("SetGameReady", 1f);
			break;
		case 4:
			myGameType = GameType.Bullet;
			AudioManager._instance.PlayBgm (AudioManager.bgm_pk);
			break;
		case 5:
			myGameType = GameType.Time;
			isFriendMode = false;
			AudioManager._instance.PlayBgm (AudioManager.bgm_pk);
			break;
		case 6:
			myGameType = GameType.Point;
			AudioManager._instance.PlayBgm (AudioManager.bgm_pk);
			break;
		case 10: //10和11虽然规则也是时间赛，但是是好友模式下的，部分UI显示有改动
			myGameType = GameType.Time;
			isFriendMode = true;
			AudioManager._instance.PlayBgm (AudioManager.bgm_pk);
			break;
		case 11:
			myGameType = GameType.Time;
			isFriendMode = true;
			AudioManager._instance.PlayBgm (AudioManager.bgm_pk);
			break;
		default:
			break;
		}
		Debug.LogWarning ("myGameType : " + myGameType);
		enemyLayer = LayerMask.GetMask (TagManager.enemy);

	}
	// Use this for initialization
	void Start ()
	{
		//Invoke ("FishTideComing", 3f);
		//InvokeRepeating ("FishTideComing", 3f,3f);
		if (AppInfo.isReciveHelpMsg && AppInfo.isInHall == false) {
			OpenHelpTaskReward ();
		}
	}

	void SetGameReady ()
	{
		gameIsReady = true;
	}

	public void GameEnd (object data = null)
	{
		UIFishingObjects.GetInstance ().tempDistribute = null;
		if (AskBackPanel._instance != null) {
			Destroy (AskBackPanel._instance.gameObject); //如果有提示面板存在，强制删除以防止跟结算面板冲突
		}
		switch (myGameType) {
		case GameType.Classical:
			break;
		case GameType.Bullet:
			gameIsReady = false;
			gameIsEnd = true;
			FiGoldGameResult bulletGameResult = (FiGoldGameResult)data;
			//PK_Bullet_FinalRanklist._instance.ShowResult (bulletGameResult.info );
			GameObject tempBulletResult = GameObject.Instantiate (PrefabManager._instance.GetPrefabObj (MyPrefabType.PkUIPrefab, 0));
			tempBulletResult.GetComponent<PK_Bullet_FinalRanklist> ().ShowResult (bulletGameResult.info);
			break;
		case GameType.Point:
			gameIsReady = false;
			gameIsEnd = true;
			FiGoldGameResult pointGameResult = (FiGoldGameResult)data;
			//PK_Bullet_FinalRanklist._instance.ShowResult (pointGameResult.info);
			//Pk_Point_FinalRanklist._instance.ShowResult(pointGameResult.info);
			GameObject tempPointResult = GameObject.Instantiate (PrefabManager._instance.GetPrefabObj (MyPrefabType.PkUIPrefab, 2));
			tempPointResult.GetComponent<Pk_Point_FinalRanklist> ().ShowResult (pointGameResult.info);
			break;
		case GameType.Time: //需要判断是否为好友对战下的限时赛
			gameIsReady = false;
			gameIsEnd = true;
			if (isFriendMode) {
				FiFriendRoomGameResult friendGameResult = (FiFriendRoomGameResult)data;
				//Pk_FinalRankList_Friend._instance.ShowResult (friendGameResult);
				GameObject tempFriendResult = GameObject.Instantiate (PrefabManager._instance.GetPrefabObj (MyPrefabType.PkUIPrefab, 1));
				tempFriendResult.GetComponent<Pk_FinalRankList_Friend> ().ShowResult (friendGameResult);
			} else {
				FiGoldGameResult timeGameResult = (FiGoldGameResult)data;
				//PK_Bullet_FinalRanklist._instance.ShowResult (timeGameResult.info );
				GameObject tempTimeResult = GameObject.Instantiate (PrefabManager._instance.GetPrefabObj (MyPrefabType.PkUIPrefab, 0));
				tempTimeResult.GetComponent<PK_Bullet_FinalRanklist> ().ShowResult (timeGameResult.info);
			}
			break;
		default:
			break;
		}
	}

	public void FishTideComing1 () //提前30秒通知鱼潮来临，此时要禁用一些道具
	{
		Debug.LogError ("FishTideComing1");
		isFishTideComing1 = true;
		Invoke ("ResetFishTideFlag1", 50f);
	}

	public void FishTideComing2 ()//提前7秒通知鱼潮来临，此时执行清场操作
	{
		//DOTween.timeScale = 2f;
		//Invoke ("ResetDoTweenTimeScale", 4f);
		Debug.LogError ("FishTideComing2");
		isFishTideComing2 = true;
		PrefabManager._instance.CreateFishTideWarningEffect ();
		UIFishingObjects.GetInstance ().fishPool.SetAllFishLeaveScreen ();  //只对收到消息时当时已经存在的鱼执行驱逐操作
		//fishTideIndex = 1;//这里后期会根据服务器发送的编号而变化
		Invoke ("ResetFishTideFlag2", 6f);
	}


	public void ResetFishTideFlag1 ()
	{
		isFishTideComing1 = false;
		//fishTideIndex = 0;
	}

	public void ResetFishTideFlag2 ()
	{
		isFishTideComing2 = false;
	}

	public void ResetDoTweenTimeScale ()
	{
		DOTween.timeScale = 1f;
	}

	/// <summary>
	/// 开启救济金
	/// </summary>
	void OpenHelpTaskReward ()
	{
		return;
		StartGiftManager.HelpTaskRewardWindow (AppInfo.isInHall);
	}

	private void OnDestroy ()
	{
		_instance = null;
	}
}
