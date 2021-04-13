using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System.Collections.Generic;
using DG.Tweening;

public enum MyPrefabType
{
	CoinGroup,
	NetEffect,
	//FireEffect,
	GunInUI,
	GunGroup,
	SkillEffect,
	PkUIPrefab
}

public class PrefabManager : MonoBehaviour
{

	public static PrefabManager _instance = null;

	public GameObject[] coinGroup;
	public GameObject[] netEffect;
	public GameObject[] fireEffect;
	public GameObject[] gunInUI;
	//gunInUI本身只是场景中的物件，而非Prefab，可能要考虑移到别的地方比较合适
	public GameObject[] pkUIGroup;

	[HideInInspector]public GameObject[] gunGroup = new GameObject[4];
	public GameObject[] skillEffect;
	//SkillUIList是在运行之后动态添加进来的，不需要手动赋值
	[HideInInspector] 
	public List<Skill> skillUIList = new List<Skill> ();
	/// <summary>
	/// 生成鱼的预制体
	/// </summary>
	public GameObject[] enemyGroup;
	///打到奖金鱼时的爆金币特效
	public GameObject bonusGoldEffect;
	///别人打到奖金鱼时爆银币的特效
	public GameObject bonusSliverEffect;
	///打到奖金鱼时在UI上方显示的特效
	public GameObject bonusUIEffect;

	///获得红包时所有人在UI上方显示的特效
	public GameObject RedPackUIEffect_small;

	///获得红包时自己屏幕中央显示的特效
	public GameObject RedPackUIEffect_big;

	/// <summary>
	/// 红包场预制体
	/// </summary>
	public GameObject redPacketEffect;
	/// <summary>
	/// 倒计时预制体
	/// </summary>
	public GameObject countDownEffect;

	//public Sprite genderFemale; //改用Resources.Load<Sprite>("Ranking/女");读取
	///鱼潮来临
	public GameObject fishTideEffect;
	///Boss来临
	public GameObject bossComingEffect;
	/// <summary>
	/// 海浪特效
	/// </summary>
	public GameObject effect_wave;
	/// <summary>
	/// 钻石掉落
	/// </summary>
	public GameObject diamondDropEffect;
	/// <summary>
	/// 暴富啦特效
	/// </summary>
	public GameObject suddenlyRichEffect;
	/// <summary>
	/// 升级
	/// </summary>
	public GameObject levelUpPanel;
	/// <summary>
	/// 掉落技能UI
	/// </summary>
	public GameObject skillUIDropHandle;
	/// <summary>
	/// 奖金鱼对话框
	/// </summary>
	public GameObject bubbleBoxPrefab;
	/// <summary>
	/// 使用技能特效
	/// </summary>
	public GameObject useSkillEffectUI;
	/// <summary>
	/// 性别图标
	/// </summary>
	public Sprite femaleSprite;
	/// <summary>
	/// 使用技能的图片描述
	/// </summary>
	public Sprite[] skillEffectSpriteGroup;

	public Sprite defaultBgSprite;
	/// <summary>
	/// 返回图片
	/// </summary>
	public Sprite backBtnSprite2;

	/// <summary>
	/// 是否显示打死奖金鱼的图片
	/// </summary>
	public bool isLuckDrawUIShow = false;
	/// <summary>
	/// 是否显示暴富的图片
	/// </summary>
	public bool isSuddenlyRichUIShow = false;
	RoomInfo roomInfo;
	//鱼雷的弹窗
	public GameObject TorpedoTipsObj;
   
	public GameObject GoldCoinColumnLeft;
	public GameObject GoldCoinColumnRight;
	//进入boss房间引导
	GameObject IntoBossRoomWindow = null;

	void Awake ()
	{
		if (_instance != null)
			Destroy (PrefabManager._instance.gameObject);
	
		_instance = this;
		AppInfo.isInHall = false;
		roomInfo = DataControl.GetInstance ().GetRoomInfo ();
	}

	/// <summary>
	/// 通过预制体类型的枚举值获取生成的特效
	/// </summary>
	/// <returns>The prefab object.</returns>
	/// <param name="type">Type.</param>
	/// <param name="index">Index.</param>
	public GameObject GetPrefabObj (MyPrefabType type, int index = 0)
	{
		switch (type) {
		case MyPrefabType.CoinGroup:
			return coinGroup [index];
		case MyPrefabType.NetEffect:
			return netEffect [index];
		//case MyPrefabType.FireEffect:
		//return fireEffect [index];
		case MyPrefabType.GunInUI:
			return gunInUI [index];
		case MyPrefabType.GunGroup:
			return gunGroup [index];
		case MyPrefabType.SkillEffect: 
			return skillEffect [index];//0锁定,1狂暴,2全屏冰冻特效,3单个鱼身上的冰块，4召唤，5鱼雷的网格，6鱼雷的锁定图标
		case MyPrefabType.PkUIPrefab:
			return pkUIGroup [index];
		default :
			return null;
		}
	}



	/// <summary>
	/// 获取自己当前炮台的相关信息
	/// </summary>
	/// <returns>The local gun.</returns>
	public GunControl GetLocalGun ()
	{
		for (int i = 0; i < gunGroup.Length; i++) {
			if (gunGroup [i].GetComponent<GunControl> ().isLocal)
				return gunGroup [i].GetComponent<GunControl> ();
		}
		return null;
	}

	/// <summary>
	/// 通过UserID获取炮台
	/// </summary>
	/// <returns>The gun by user I.</returns>
	/// <param name="userID">User I.</param>
	public GunControl GetGunByUserID (int userID)
	{
		for (int i = 0; i < gunGroup.Length; i++) {
			if (gunGroup [i].GetComponent<GunControl> ().userID == userID) {
				return gunGroup [i].GetComponent<GunControl> ();
			}
		}
		Debug.LogError ("Error! GetGunByUserId:" + userID + " failed!");
		return null;
	}

	/// <summary>
	/// 通过鱼群的ID寻找鱼群
	/// </summary>
	/// <returns>The enemy by type I.</returns>
	/// <param name="typeID">Type I.</param>
	public GameObject FindEnemyByTypeID (int typeID)
	{
		for (int i = 0; i < enemyGroup.Length; i++) {
			if (enemyGroup [i].GetComponent<EnemyBase> ().typeID == typeID) {
				return enemyGroup [i];
			}
		}
		Debug.Log ("Error:Can not find enemy by typeID!");
		return null;
	}



	/// <summary>
	/// 添加技能
	/// </summary>
	/// <param name="skill">Skill.</param>
	public void AddSkillUIToList (Skill skill)
	{
		if (!skillUIList.Contains (skill)) {
			skillUIList.Add (skill);
		}

	}

	/// <summary>
	/// 通过技能确定是否是鱼雷
	/// </summary>
	/// <returns>The skill user interface by type.</returns>
	/// <param name="type">Type.</param>
	/// <param name="torpedoLevel">Torpedo level.</param>
	public Skill GetSkillUIByType (SkillType type, int torpedoLevel = 0)
	{
		for (int i = 0; i < skillUIList.Count; i++) {
			if (type != SkillType.Torpedo) {
				if (skillUIList [i].skillType == type) {
					return skillUIList [i];
				}
			} else {
				if (skillUIList [i].torpedoLevel == torpedoLevel) {
					return skillUIList [i];
				}
			}

		}
		return null;
	}

	/// <summary>
	/// 通过服务器id释放技能
	/// </summary>
	/// <returns>The skill user interface by server identifier.</returns>
	/// <param name="serverId">Server identifier.</param>
	public Skill GetSkillUIByServerId (int serverId)
	{
		Skill tempSkill = null;
		for (int i = 0; i < skillUIList.Count; i++) {
			if (skillUIList [i].serverTypeId == serverId) {
				tempSkill = skillUIList [i];
				break;
			}
		}
		if (tempSkill == null)
			Debug.LogError ("Null Error! Can't find Skill by serverId=" + serverId);
		return tempSkill;
	}

	/// <summary>
	/// 倒计时是否初始化
	/// </summary>
	[HideInInspector]public  bool countDownhasInit = false;

	public void CreateCountDownEffect (float destroyTime = 3.5f)
	{
		if (countDownhasInit)
			return;
		countDownhasInit = true;
		GameObject tempEffect = GameObject.Instantiate (countDownEffect);
		AudioManager._instance.PlayEffectClip (AudioManager.effect_countDown);
		//ShowPropHintPanel();
		Destroy (tempEffect, destroyTime);
		Invoke ("SetGameIsReady", 3f);
	}

	/// <summary>
	/// 设置游戏是否已经准备
	/// </summary>
	void SetGameIsReady ()
	{
		GameController._instance.gameIsReady = true;
	}

	/// <summary>
	/// 生成鱼潮来临的警告特效
	/// </summary>
	public void CreateFishTideWarningEffect ()
	{
		if (isLuckDrawUIShow || isSuddenlyRichUIShow)
			return;
		GameObject tempEffect = GameObject.Instantiate (fishTideEffect);
		Destroy (tempEffect, 4f);
		Invoke ("CreateWaveEffect", 3f);
		AudioManager._instance.PlayEffectClip (AudioManager.effect_fishTide);
	}

	/// <summary>
	/// 生成海浪特效
	/// </summary>
	public void CreateWaveEffect ()
	{
		GameObject tempEffect2 = GameObject.Instantiate (effect_wave);
		Destroy (tempEffect2, 5f);
	}

	/// <summary>
	/// 重置分数
	/// </summary>
	public void ResetGunScore ()
	{
		for (int i = 0; i < gunInUI.Length; i++) {
			gunInUI [i].GetComponent<GunInUI> ().SetScoreText (0);
		}
	}

	public Sprite[] bossComingSpriteGroup;
	public Sprite[] bossComingNameSpriteGroup;

	/// <summary>
	/// 播放Boss来临的特效
	/// </summary>
	/// <param name="goldReturn">Gold return.</param>
	/// <param name="bossTypeId">Boss type identifier.</param>
	public void PlayBossComingEffect (int goldReturn, int bossTypeId)
	{
		if (isLuckDrawUIShow || isSuddenlyRichUIShow)
			return;
		GameObject tempEffect = GameObject.Instantiate (bossComingEffect);
		// tempEffect.transform.position = new Vector3(0, 0, -1);
		//tempEffect.transform.FindChild ("_Effect_UI_Boss/NumText").GetComponent<TextMesh> ().text = goldReturn.ToString ();
		//tempEffect.transform.FindChild("_Effect_UI_Boss/BossIcon").GetComponent<SpriteRenderer>().sprite=bossComingSpriteGroup[bossTypeId-19];
		tempEffect.GetComponent<Canvas> ().worldCamera = ScreenManager.uiCamera;
		tempEffect.transform.Find ("UI_Boss_Coming/UI_Boss_tubiao").GetComponent<Image> ().sprite = bossComingSpriteGroup [bossTypeId - 19];
		tempEffect.transform.Find ("UI_Boss_Coming/UI_Boss_zi/UI_Boss_name").GetComponent<Image> ().sprite = bossComingNameSpriteGroup [bossTypeId - 19];

		Destroy (tempEffect, 6.2f);
		if (AudioManager._instance != null)
			AudioManager._instance.PlayEffectClip (AudioManager.effect_bossComing);
        
	}

	GameObject tempEffect = null;
	int sumGoldTor = 0;

	/// <summary>
	/// 播放 暴富啦 特效
	/// </summary>
	/// <param name="sumGold">Sum gold.</param>
	public void PlaySuddenlyRichEffect (int sumGold)
	{
		if (isLuckDrawUIShow)
			return;
		if (tempEffect != null) {
			sumGoldTor = sumGold;
			tempEffect.GetComponentInChildren<TextMesh> ().text = (sumGoldTor + sumGold).ToString ();
		} else {
			Debug.LogError ("SumGold=" + sumGold.ToString ());
			isSuddenlyRichUIShow = true;
			Vector3 pos = new Vector3 (0, 0, 60);
			tempEffect = GameObject.Instantiate (suddenlyRichEffect, pos, Quaternion.identity) as GameObject;
			tempEffect.GetComponentInChildren<TextMesh> ().text = sumGold.ToString ();
			Destroy (tempEffect, 3.8f);
			Invoke ("RichUIDelaySet", 3.8f);
		}

	}

	void RichUIDelaySet ()
	{
		isSuddenlyRichUIShow = false;
	}

	/// <summary>
	/// 游戏升级特效管理
	/// </summary>
	/// <param name="newLevel">等级</param>
	/// <param name="diamondNum">钻石数</param>
	/// <param name="lockNum">锁定技能的获得数</param>
	/// <param name="freezeNum">冰冻技能的获得数</param>
	public void ShowLevelUpPanel (int newLevel, int diamondNum, int lockNum, int freezeNum)
	{
		if (NewcomerRewardPanel._instance == null) {
			GameObject temp = GameObject.Instantiate (levelUpPanel, ScreenManager.uiScaler.transform) as GameObject;
			temp.GetComponent<RectTransform> ().localScale = Vector3.one * 1.271038f;
			temp.GetComponent<LevelUpPanel> ().SetInfo (newLevel, diamondNum, lockNum, freezeNum);
		} else {
			GetLocalGun ().gunUI.AddValue (0, 0, diamondNum);
			GetSkillUIByType (SkillType.Lock).AddRestNum (lockNum);
			GetSkillUIByType (SkillType.Freeze).AddRestNum (freezeNum);
		}
		
	}

	/// <summary>
	/// 新手任务
	/// </summary>
	public GameObject newcomerMissiionPanel;

	/// <summary>
	/// 显示新手任务面板
	/// </summary>
	/// <param name="missionIndex">任务的等级下标</param>
	/// <param name="missionProgress">任务进度</param>
	public void ShowNewcomerMission(int missionIndex, int missionProgress)
	{
		//		Debug.LogError ("ShowNewcomerMission:" + missionIndex + "," + missionProgress);
		if (missionIndex > 9 || roomInfo.roomMultiple != 0)
			return;

		GameObject temp = GameObject.Instantiate(newcomerMissiionPanel);
		newcomerMissiionPanel.SetActive(true);
		temp.GetComponent<NewcormerMissionPanel>().ShowIndexHint(missionIndex, missionProgress);

	}

    /// <summary>
    /// 控制新手任務面板顯示
    /// </summary>
    /// <param name="show"> true:顯示 , false:不顯示 </param>
	public void ControlNewcomerMissionShow(bool show)
    {
		newcomerMissiionPanel.SetActive(show);
	}

	/// <summary>
	/// 新手任务获得面板
	/// </summary>
	public GameObject newcomerRewardPanel;

	/// <summary>
	/// 显示新手获得面板奖励
	/// </summary>
	/// <param name="missionIndex">Mission index.</param>
	/// <param name="rewardNum">奖励数</param>
	public void ShowNewcomerRewardPanel (int missionIndex, float rewardNum = 0)
	{ //rewardNum根据本地判断　
		int rewardType = 0;
		float tempRewardNum = 0;
		switch (missionIndex) {
		case 1:
			rewardType = 4;
			tempRewardNum = 100;
			break;
		case 2:
			rewardType = 4;
			tempRewardNum = 150;
			break;
		case 3:
			rewardType = 4;
			tempRewardNum = 200;
			break;
		case 4:
			rewardType = 4;
			tempRewardNum = 300;
			break;
		case 5:
			rewardType = 4;
			tempRewardNum = 400;
			break;
		case 6:
			rewardType = 4;
			tempRewardNum = 500;
			break;
		case 7:
			rewardType = 4;
			tempRewardNum = 1000;
			break;
		case 8:
			rewardType = 4;
			tempRewardNum = 3000;
			break;
		case 9:
			rewardType = 4;
			tempRewardNum = 3000;
			break;
//		case 10:
//			rewardType = 0;
//			tempRewardNum = 20;
//			break;
//		case 11:
//			rewardType = 0;
//			tempRewardNum = 20;
//			break;
//		case 12:
//			rewardType = 0;
//			tempRewardNum = 30;
			break;
		default:
			break;
		}
		GameObject temp = GameObject.Instantiate (newcomerRewardPanel);
		temp.GetComponent<NewcomerRewardPanel> ().SetRewardInfo (rewardType, tempRewardNum);
	}

	/// <summary>
	/// 返回大厅按钮设置
	/// </summary>
	/// <param name="str">String.</param>
	public void ShowNormalHintPanel (string str)
	{
		Transform backBtnTrans = ScreenManager.uiScaler.transform.Find ("LeftOption/BackBtn");
		if (backBtnTrans == null)
			backBtnTrans = GameObject.FindGameObjectWithTag ("BackBtn").transform;
		GameObject temp = GameObject.Instantiate (backBtnTrans.GetComponent<GameBackBtn> ().backConfirmPanel);

         
		temp.transform.parent = ScreenManager.uiScaler.transform;
		temp.transform.position = Vector3.zero;
		temp.transform.localScale = new Vector3 (1.3f, 1.3f, 1.3f);
		temp.GetComponent<AskBackPanel> ().Show (str, true, false);
	}

	/// <summary>
	/// 发道具面板
	/// </summary>
	public GameObject propHintPanel;

	/// <summary>
	/// 向服务器发送  我要给玩家发道具啦
	/// </summary>
	public void ShowPropHintPanel ()
	{
		for (int i = 0; i < skillUIList.Count; i++) {
			skillUIList [i].HideOutScreen ();
		}
		GameObject.Instantiate (propHintPanel);
	}

	/// <summary>
	/// Boss死亡面板
	/// </summary>
	public GameObject bossDeadHintPanel;

	/// <summary>
	/// 显示Boss死亡面板
	/// </summary>
	/// <param name="bossTypeId">Boss type identifier.</param>
	/// <param name="userName">User name.</param>
	/// <param name="killGold">Kill gold.</param>
	public void ShowBossDeadHintPanel (int bossTypeId, string userName, int killGold)
	{
		GameObject temp = GameObject.Instantiate (bossDeadHintPanel);
		temp.transform.SetParent (GameObject.FindWithTag (TagManager.uiCanvas).transform);
		temp.transform.localPosition = Vector3.zero;
		temp.transform.localScale = Vector3.one;
		temp.GetComponent<BossDeadHint> ().SetBossIconShow (bossTypeId, userName, killGold);

	}

	/// <summary>
	/// 获取4个炮台数 对炮台的数量进行判断(后期机器人会因为炮台满4人就不加人游戏)
	/// </summary>
	/// <returns>The active gun count.</returns>
	public int GetActiveGunCount ()
	{
		int count = 0;
		for (int i = 0; i < gunGroup.Length; i++) {
			if (gunGroup [i].GetComponent<GunControl> ().isActived) {
				count++;
			}
		}
		return count;
	}

	/// <summary>
	/// 救济金预制体
	/// </summary>
	public GameObject AlmsCountDownPrefab;

	/// <summary>
	/// 生成救济金
	/// </summary>
	/// <param name="seconds">Seconds.</param>
	public void CreateAlmsCountDown (int seconds)
	{
		GameObject temp = GameObject.Instantiate (AlmsCountDownPrefab, ScreenManager.uiScaler.transform);
		//temp.transform.SetParent(ScreenManager.uiScaler.transform);
		Vector3 createPos = PrefabManager._instance.GetLocalGun ().gunUI.transform.Find ("BonusEffectPos").position;
		temp.transform.position = createPos;
		temp.GetComponent<AlmsCountDown> ().UpdateTime (seconds);

	}


	//欢迎来到体验场
	public GameObject welcomeExperiencePrefab;

	public void CreateWelcomeExperience ()
	{
		GameObject.Instantiate (welcomeExperiencePrefab);
	}

	/// <summary>
	/// 游客提示
	/// </summary>
	public GameObject guestNoticePrefab;

	public void CreateGuestNotice ()
	{
		GameObject.Instantiate (guestNoticePrefab);
	}

	public void CreateBossMatchPrefab ()
	{
		string path = string.Empty;
		path = "Game/BossMatchCanvas";
		AppControl.OpenWindow (path);
	}

	/// <summary>
	/// 引导进入boss房间
	/// </summary>
	/// <param name="intoMessage">引导消息.</param>
	public void CreateIntoBossRoomPrefab (string intoMessage)
	{   
		if (GameController._instance.isBossMode || GameController._instance.isBossMatchMode || IntoBossRoomWindow != null)
			return;
        
		string path = string.Empty;
		path = "Game/IntoBossRoom";
		IntoBossRoomWindow = AppControl.OpenWindow (path);
		//IntoBossRoomWindow =Resources.Load("Game/IntoBossRoom") as GameObject;
		IntoBossRoomWindow.transform.Find ("Bg/Text").GetComponent<Text> ().text = intoMessage;
		//GameObject.Instantiate(IntoBossRoomWindow);
	}

	private void OnDestroy ()
	{
		_instance = null;
	}

	private void Update ()
	{
		DebugUpdateTest ();
	}

	/// <summary>
	/// 测试
	/// </summary>
	void DebugUpdateTest ()
	{
		return;
		if (Input.GetKeyDown (KeyCode.Alpha5)) {
            
		}
		if (Input.GetKeyDown (KeyCode.Alpha6)) {
			PlaySuddenlyRichEffect (10000);
		}
		if (Input.GetKeyDown (KeyCode.Alpha7)) {
			ShowLevelUpPanel (20, 2, 3, 3);
		}
		if (Input.GetKeyDown (KeyCode.Alpha8)) {
			CreateFishTideWarningEffect ();
		}
		if (Input.GetKeyDown (KeyCode.Alpha9)) {
			PlayBossComingEffect (200, 22);
		}
	}
}