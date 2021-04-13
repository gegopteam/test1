using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using DG.Tweening;
using AssemblyCSharp;

public class NewcormerMissionPanel : MonoBehaviour
{

	public static NewcormerMissionPanel _instance;

	public GameObject topBoard;
	public GameObject effect_TopBoard;
	public GameObject topEdgePoint;

	public GameObject newcormerIndexHintPrefab;
	[HideInInspector]
	public GameObject tempNewcormerIndexHint = null;

	public GameObject unlockMultipleMission;

	public int currentMissionIndex = -1;


	public static int[] missionTargetCountGroup = new int[] { -1, 10, 30, 1, 1, 1, 1,  50,  100,  1 };
	// public int[] missionRewardTypeGroup = new int[] { -1, 10, 30, 1, 1, 1, 1, 50, 50, 100, 100, 200, 1 }; //目前所有type都是0，即钻石
	public static int[] missionRewardCountGroup = new int[] { -1, 100, 150, 200, 300, 400, 500, 1000,  3000, 3000 };

	public static string[] missionDescripeGroup = new string[] {
		"無效的文字索引", "捕獲10條任意魚", "捕獲30條任意魚", "使用1次冰凍道具", "使用1次召喚道具", "捕獲1只魔鬼魚", "捕獲1只鯊魚",
        "捕獲50條任意魚", "捕獲100條任意魚", "捕獲1只boss"
	};
	//TargetCountGroup	70,100,200,
	//RewardCountGroup 	10,20,30,
	//DescripeGroup		"解锁至70倍炮","解锁至100倍炮","解锁至200倍炮",
	int currentCount = 0;
	int targetCount;
	int rewardType;
	//0=diamond ,1=redpacketTicket,2= lockSkill, 3=freezeSkill
	float rewardNum = -1;

	public Image missionIcon;
	public Text missionProgress;
	public Text missionReward;

	public Sprite[] missionIconGroup;

	public GameObject newcomerGuidePrefab;

	public Image rewardTypeImage;
	public Sprite[] rewardTypeSpriteGroup;

	public GameObject fingerPrefab;

	private void Awake ()
	{
		if (null == _instance)
			_instance = this;
		else {
			Destroy (NewcormerMissionPanel._instance.gameObject);
			_instance = this;
		}
	}




	public void ShowIndexHint (int index, int progress) //出现新手任务X的提示动画
	{
		//index = 4;//debugTest
		this.GetComponent<Canvas> ().worldCamera = ScreenManager.uiCamera;
		this.GetComponent<Canvas> ().planeDistance = 45f;
		topBoard.SetActive (false);
		if (index == 1 && GameController._instance.isExperienceMode) {
			GameObject.Instantiate (newcomerGuidePrefab);
		}
		currentMissionIndex = index;
		currentCount = progress;
		this.GetComponent<Canvas> ().worldCamera = ScreenManager.uiCamera;

		PreSetTopBoard ();
//
//		if (index == 1)
//			Invoke ("DelayShowIndexHint", 5f);
//		else
		DelayShowIndexHint ();
	}

	void DelayShowIndexHint ()
	{
		GameObject temp = GameObject.Instantiate (newcormerIndexHintPrefab, Vector3.zero, Quaternion.identity) as GameObject;
		temp.transform.parent = this.transform;
		temp.GetComponent<RectTransform > ().localPosition = Vector3.zero;
		temp.transform.localScale = Vector3.one;

		// temp.transform.FindChild("TextImage/Num").GetComponent<Image>().sprite = numSpriteGroup[currentMissionIndex - 1];
		temp.transform.GetComponentInChildren<Text> ().text = currentMissionIndex.ToString ();
		tempNewcormerIndexHint = temp;
		Invoke ("IndexHintMoveToTop", 1.5f);
	}

	void IndexHintMoveToTop ()
	{
		float tempTime = 1f;
		tempNewcormerIndexHint.transform.DOMove (topEdgePoint.transform.position, tempTime);
		tempNewcormerIndexHint.transform.DOScale (0, tempTime);
		Invoke ("ShowTopBoard", tempTime - 0.5f);
	}

	void ShowTopBoard ()
	{
		if (tempNewcormerIndexHint != null)
			Destroy (tempNewcormerIndexHint);
		effect_TopBoard.SetActive (true);
		tempNewcormerIndexHint = null;
		topBoard.gameObject.transform.Translate (Vector3.down * 1000);
		topBoard.GetComponent<Animator> ().enabled = true;
		if (currentMissionIndex == 3) {
			ShowFreezeSkillFinger ();
		} else if (currentMissionIndex == 4) {
			ShowLockSkillFinger ();
		}

		if (currentCount > targetCount) //如果玩家上次在游戏里完成任务但没领取奖励，那么这次进入游戏可能触发已经完成任务却无法领取奖励的bug，需要这个判断去修正
            currentCount = targetCount;
        
		SetMissionProgress (currentCount);
	}

	void PreSetTopBoard ()
	{
		topBoard.gameObject.SetActive (true);
		topBoard.gameObject.transform.Translate (Vector3.up * 1000); //设置好信息后隐藏在上方

		missionIcon.sprite = missionIconGroup [currentMissionIndex - 1];

//		if (currentMissionIndex != 7 && currentMissionIndex != 9 && currentMissionIndex != 11) {
//			missionIcon.sprite = missionIconGroup [currentMissionIndex - 1];
//		} else {
//			missionIcon.enabled = false;
//		}

		switch (currentMissionIndex) {
		case 1:
			targetCount = 10;
			rewardType = 4;
			rewardNum = 100;
			break;
		case 2:
			targetCount = 30;
			rewardType = 4;
			rewardNum = 150;
			break;
		case 3:
			targetCount = 1;
			rewardType = 4;
			rewardNum = 200;
               
			break;
		case 4:
			targetCount = 1;
			rewardType = 4;
			rewardNum = 300;
               
			break;
		case 5:
			targetCount = 1;
			rewardType = 4;
			rewardNum = 400;
			break;
		case 6:
			targetCount = 1;
			rewardType = 4;
			rewardNum = 500;
			break;
//		case 7:
//                //targetCount = 50; 
//			targetCount = 70;//需求改动，任务7解锁炮倍数由50改为70
//			rewardType = 4;
//			rewardNum = 10;
//			break;
		case 7:
			targetCount = 50;
			rewardType = 4; 
			rewardNum = 1000;
			break;
//		case 9:
//			targetCount = 100;
//			rewardType = 4;
//			rewardNum = 20;
//			break;
		case 8:
			targetCount = 100;
			rewardType = 4;
			rewardNum = 3000;
			break;
//		case 11:
//			targetCount = 200;
//			rewardType = 4;
//			rewardNum = 20;
//			break;
		case 9:
			targetCount = 1;
			rewardType = 4;
			rewardNum = 3000;
			missionIcon.transform.localScale = Vector3.one * 0.5833918f;
			break;
		default:
			break;
		}
		rewardTypeImage.sprite = rewardTypeSpriteGroup [rewardType];
		missionReward.text = "x" + rewardNum;

		unlockMultipleMission.SetActive (false);

//		if (currentMissionIndex == 7) {  //unlock50
//			unlockMultipleMission.SetActive (true);
//			unlockMultipleMission.GetComponent<Image> ().sprite = missionIconGroup [currentMissionIndex - 1];
//			currentCount = PrefabManager._instance.GetLocalGun ().maxCannonMultiple;
//		} else if (currentMissionIndex == 9) { //unlock100
//			unlockMultipleMission.SetActive (true);
//			unlockMultipleMission.GetComponent<Image> ().sprite = missionIconGroup [currentMissionIndex - 1];
//			if (PrefabManager._instance.GetLocalGun () == null)
//				Debug.LogError ("Error!LocalGun=null");
//			currentCount = PrefabManager._instance.GetLocalGun ().maxCannonMultiple;
//		} else if (currentMissionIndex == 11) {
//			unlockMultipleMission.SetActive (true); //unlock250
//			unlockMultipleMission.GetComponent<Image> ().sprite = missionIconGroup [currentMissionIndex - 1];
//			currentCount = PrefabManager._instance.GetLocalGun ().maxCannonMultiple;
//		}
		//SetMissionProgress();//改为在ShowTopBoard里再更新任务进度，防止任务信息还没显示完就提前放出奖励
	}

	public void AddMissionProgress (int addCount = 1)
	{
		//missionIcon.sprite = missionIconGroup[currentMissionIndex - 1];
		return; // 这里原来是本地做任务进度判断，现在改为完全由服务器判断
		currentCount += addCount;
		if (currentCount == targetCount) { 
			//PrefabManager._instance.ShowNewcomerRewardPanel(currentMissionIndex); //本地直接显示奖励
			Facade.GetFacade ().message.task.SendBeginnerTaskRewardRequest (currentMissionIndex);//向服务器发送请求奖励
		}
		if (currentCount > targetCount) {
			currentCount = targetCount;
		}
       

		if (currentMissionIndex < 7) {
			missionProgress.text = currentCount + "/" + targetCount;
		} else if (currentMissionIndex == 7) {
			missionProgress.text = "";
		} else if (currentMissionIndex == 8) {
			missionProgress.text = "";
		}

	}

	public void SetMissionProgress (int setCount)
	{
		//missionIcon.sprite = missionIconGroup[currentMissionIndex - 1];
		// Debug.LogError("SetProgress:"+currentMissionIndex+","+setCount);
		currentCount = setCount;
		if (currentCount == targetCount) {
			if (currentMissionIndex == 3 || currentMissionIndex == 4) {
				Invoke ("DelayShowReward", 1.5f);
			} else
				PrefabManager._instance.ShowNewcomerRewardPanel (currentMissionIndex); //本地直接显示奖励,但是点击领取时要向服务器确认
			//Facade.GetFacade().message.task.SendBeginnerTaskRewardRequest(currentMissionIndex);//向服务器发送请求奖励,这个在领取的时候发，而不是达成任务
		}
		//		} else if (currentCount > targetCount) {
//			if (currentMissionIndex == 7 || currentMissionIndex == 9 || currentMissionIndex == 11) { //missionIndex=7,9,11
//				PrefabManager._instance.ShowNewcomerRewardPanel (currentMissionIndex); 
//			}
//           
//			currentCount = targetCount;
//		}


		/*if (currentMissionIndex == 7 || currentMissionIndex == 9 || currentMissionIndex == 11) {
			missionProgress.text = "";
		} else {
			missionProgress.text = currentCount + "/" + targetCount;
		}*/
		missionProgress.text = currentCount + "/" + targetCount;
	}

	public void CurrentMissionComplete (int index)
	{
		Debug.LogError ("CurrentMissionComplete:" + index);
	}

	public void DelayShowReward ()
	{
		PrefabManager._instance.ShowNewcomerRewardPanel (currentMissionIndex);
	}

	void AllMissionComplete ()
	{
		_instance = null;
		Destroy (this.gameObject);
	}

	public void RecoverUILayer ()
	{
		//this.GetComponent<Canvas>().sortingLayerName="Default";
		return;
		this.GetComponent<Canvas> ().sortingOrder = 1;
	}


	public GameObject allMissionPanelPrefab;

	public void ShowAllNewcomerMission ()
	{
		GameObject temp = GameObject.Instantiate (allMissionPanelPrefab, this.transform) as GameObject;
		temp.GetComponent<RectTransform> ().localPosition = Vector3.zero;
		temp.GetComponent<RectTransform> ().localScale = new  Vector3 (1.3f, 1.3f, 1.3f);
		temp.GetComponent<AllNewcomerMission> ().InitPanel (currentMissionIndex, currentCount);
	}

	public void DestorySelf ()
	{
		_instance = null;
		// Destroy(this.gameObject);
		DestroyImmediate (this.gameObject);
	}

	public void ShowLockSkillFinger ()
	{
		return;
		GameObject temp = GameObject.Instantiate (fingerPrefab);
		GameObject lockSkill = PrefabManager._instance.GetSkillUIByType (SkillType.Lock).gameObject;
		temp.transform.parent = lockSkill.transform;
		temp.transform.localPosition = Vector3.zero;
		temp.transform.localScale = Vector3.one;
		temp.transform.Find ("TextBox/Text").GetComponent<Text> ().text = "使用自動，提高捕魚概率。";
		Destroy (temp.gameObject, 4f);
	}

	public void ShowFreezeSkillFinger ()
	{
		GameObject temp = GameObject.Instantiate (fingerPrefab);
		GameObject freezeSkill = PrefabManager._instance.GetSkillUIByType (SkillType.Freeze).gameObject;
		temp.transform.parent = freezeSkill.transform;
		temp.transform.localPosition = Vector3.zero;
		temp.transform.localScale = Vector3.one;
		temp.transform.Find ("TextBox/Text").GetComponent<Text> ().text = "使用冰凍，隨機冰凍魚種。";
		Destroy (temp.gameObject, 4f);
	}

	private void OnDestroy ()
	{
		_instance = null;
		DataControl.GetInstance ().GetMyInfo ().beginnerCurTask = currentMissionIndex;
		DataControl.GetInstance ().GetMyInfo ().beginnerProgress = currentCount;
	}
}