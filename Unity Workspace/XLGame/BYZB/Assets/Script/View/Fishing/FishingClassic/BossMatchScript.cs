/***
 *
 *   Title: 
 *
 *   Description:
 *
 *   Author:
 *
 *   Date: 
 *
 *   Modify： 
 * 
 */
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using AssemblyCSharp;

public class BossMatchScript : MonoBehaviour
{
	/// <summary>
	/// 头部标题
	/// </summary>
	//	Text topTitleText;
	/// <summary>
	/// 中间标题
	/// </summary>
	//	Text titleText;
	/// <summary>
	/// 时间
	/// </summary>
	Text timeText;
	/// <summary>
	/// 奖励
	/// </summary>
	Text rewardText;
	/// <summary>
	/// 规则
	/// </summary>
	Text ruleText;
	/// <summary>
	/// 橙色文字
	/// </summary>
	Text orangeText;
	/// <summary>
	/// 蓝色文字
	/// </summary>
	Text blueText;
	/// <summary>
	/// 橙色按钮
	/// </summary>
	Button orangeButton;
	/// <summary>
	/// 蓝色按钮
	/// </summary>
	Button blueButton;
	/// <summary>
	/// 结束文字
	/// </summary>
	Text endText;
	/// <summary>
	/// 中部总管理
	/// </summary>
	Transform centerControl;
	public static BossMatchScript Instance;
	/// <summary>
	/// 倒计时时间
	/// </summary>
	//	Text cutDownTimeText;
	//	int time = 30;
	public static int beginType = 0;
	/// <summary>
	/// 自己的信息
	/// </summary>
	MyInfo myInfo = null;
	/// <summary>
	/// 红字描述
	/// </summary>
	Text leaveGameTipsText;
	/// <summary>
	/// 游戏开始但是玩家没有进入游戏时的提示
	/// </summary>
	Text matchBeginText;
	public static bool isToLoadBoss = false;

	void Awake ()
	{
		Instance = this;
		myInfo = (MyInfo)Facade.GetFacade ().data.Get (FacadeConfig.USERINFO_MODULE_ID);
		orangeText = transform.Find ("ButtonControl/OrangeButton/OrangeText").GetComponent <Text> ();
		blueText = transform.Find ("ButtonControl/BlueButton/BlueText").GetComponent <Text> ();
//		topTitleText = transform.Find ("TopControl/TitleText").GetComponent <Text> ();
//		titleText = transform.Find ("CenterControl/TitleText").GetComponent <Text> ();
		timeText = transform.Find ("CenterControl/Time/TimeText").GetComponent <Text> ();
		rewardText = transform.Find ("CenterControl/Reward/RewardText").GetComponent <Text> ();
		ruleText = transform.Find ("CenterControl/Rule/RuleText").GetComponent <Text> ();
		endText = transform.Find ("EndText").GetComponent <Text> ();
		matchBeginText = transform.Find ("MatchBeginText").GetComponent <Text> ();
//		cutDownTimeText = transform.Find ("CutDownTimeText").GetComponent <Text> ();
		leaveGameTipsText = transform.Find ("LeaveGameTipsText").GetComponent <Text> ();
		centerControl = transform.Find ("CenterControl");
		centerControl.gameObject.SetActive (true);
	}

	void Start ()
	{
		orangeButton = transform.Find ("ButtonControl/OrangeButton").GetComponent <Button> ();
		blueButton = transform.Find ("ButtonControl/BlueButton").GetComponent <Button> ();
		orangeButton.onClick.RemoveAllListeners ();
		blueButton.onClick.RemoveAllListeners ();
		orangeButton.onClick.AddListener (ClickOrangeButton);
		blueButton.onClick.AddListener (ClickBlueButton);
		endText.gameObject.SetActive (false);

//		if (beginType == 1) {
//			endText.gameObject.SetActive (false);
//			centerControl.gameObject.SetActive (true);
//			leaveGameTipsText.gameObject.SetActive (false);
//		} else if (beginType == 2) {
//			endText.gameObject.SetActive (true);
//			centerControl.gameObject.SetActive (false);
//			leaveGameTipsText.gameObject.SetActive (false);
//		}
	
		//	StartCoroutine (TimeCutDown ());
	}

	//	IEnumerator TimeCutDown ()
	//	{
	//		while (true) {
	//			if (time > 0) {
	//				time -= 1;
	//				cutDownTimeText.text = "倒计时" + time + "秒";
	//			} else {
	//				Destroy (gameObject);
	//			}
	//			yield return  new WaitForSeconds (1f);
	//		}
	//	}

	/// <summary>
	/// 设置文字描述
	/// </summary>
	/// <param name="_topTitleStr">头部标题文字描述</param>
	/// <param name="_titleStr">标题</param>
	/// <param name="_timeStr">时间文字</param>
	/// <param name="_rewardStr">奖励文字</param>
	/// <param name="_ruleStr">规则文字</param>
	/// , string _titleStr = null
	public void SetTextDescription (bool isBegin, string _orangeStr, string _blueStr, string _timeStr = null, string _rewardStr = null, string _ruleStr = null)
	{
		//这个时候触发开始,那么就显示服务器下发的显示
		if (isBegin) {
			centerControl.gameObject.SetActive (true);
			leaveGameTipsText.gameObject.SetActive (false);
			matchBeginText.gameObject.SetActive (false);
//		topTitleText.text = _topTitleStr;
//			titleText.text	= _titleStr;
			timeText.text	= _timeStr;
			rewardText.text	= _rewardStr;
			ruleText.text	= _ruleStr;
		} else {
			if (AppInfo.isInHall) {
				//如果是中途退出
				if (myInfo.myBossMatchState > 0 && myInfo.myBossMatchState < 5) {
					centerControl.gameObject.SetActive (false);
					leaveGameTipsText.gameObject.SetActive (true);
					matchBeginText.gameObject.SetActive (false);
				}//游戏开始,但是开始时 玩家没有在我们游戏 这个时候给他提示游戏开始 
				else if (myInfo.myBossMatchState == 5) {
					centerControl.gameObject.SetActive (false);
					leaveGameTipsText.gameObject.SetActive (false);
					matchBeginText.gameObject.SetActive (true);
				}	
			}
		}
		orangeText.text = _orangeStr;
		blueText.text = _blueStr;
	}

	void ClickOrangeButton ()
	{
		QuitBack.isQuit = true;
		//进行游戏调转
//		if (beginType == 1) {
//			UIHallObjects.GetInstance ().SndNotifySignUpRequest (1);
//		} else if (beginType == 2) {		
//			UIHallObjects.GetInstance ().SndNotifySignUpRequest (2);
//		}
//		Debug.LogError ("myInfo.myBossMatchState = " + myInfo.myBossMatchState);
//		if (myInfo.myBossMatchState > 0) {
//		}
		if (AppInfo.isInHall) {
			UIHallObjects.GetInstance ().PlayBossMatch ();
			UIFishingMsg.GetInstance ().SndNotifySignUpRequest (1);
		} else {
			UIFishingMsg.GetInstance ().SndNotifySignUpRequest (1);
			//LeaveRoomTool.LeaveRoom ();
		}
		isToLoadBoss = true;
		AudioManager._instance.PlayEffectClip (AudioManager.ui_click);
		beginType = 1;

		Destroy (gameObject);
	}

	void ClickBlueButton ()
	{
		beginType = 0;
		Destroy (gameObject);
		myInfo.myBossMatchState = 0;
	}

	//	void OnDestroy ()
	//	{
	//		beginType = 0;
	//	}
}
