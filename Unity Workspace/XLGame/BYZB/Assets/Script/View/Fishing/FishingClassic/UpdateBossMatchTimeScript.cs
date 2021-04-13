/***
 *
 *   Title: Boss场刷新时间
 *
 *   Description: 刷新Boss场倒计时时间(只在Boss匹配限时场中使用) Fishing场景中 -> Canvas -> BossMatchTimeObj 中挂载
 *
 *   Author: bw
 *
 *   Date: 2019.1.23
 *
 *   Modify:
 *          1.23: 目前只是做一个刷新操作,(后期有开始时间,结束时间,暂不处理)
 * 
 */
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UpdateBossMatchTimeScript : MonoBehaviour
{
	/// <summary>
	/// 时间计时器
	/// </summary>
	Text timeText;
	public static UpdateBossMatchTimeScript Instance;

	void Awake ()
	{
		Instance = this;
	}

	void Start ()
	{
		//不是boss就直接摧毁
		if (!GameController._instance.isBossMatchMode) {
			Destroy (gameObject);
			return;
		}
		if (PrefabManager._instance != null) {
			if (PrefabManager._instance.GetLocalGun ().isLocal) {
				//本地玩家发送刷新时间消息
				UIFishingMsg.GetInstance ().SndUpdateBossMatchTimeRequest ();
				//在这里生成规则弹窗
				string path = string.Empty;
				path = "Game/BossRuleCanvas";
				AppControl.OpenWindow (path);	
			}	
		}
		timeText = transform.Find ("TimeImage/TimeText").GetComponent<Text> ();
		StartCoroutine (SendUpdateTime ());
	}

	/// <summary>
	/// 刷新时间调用,当每次刷新的时候,刷新UI显示时间
	/// </summary>
	/// <param name="time">Time.</param>
	public void UpdateTime (long time)
	{
//		StopCoroutine (UseTimeManager (time));
		StopAllCoroutines ();
		StartCoroutine (UseTimeManager (time));
	}

	/// <summary>
	/// 5分钟刷新一次 (暂定)
	/// </summary>
	/// <returns>The update time.</returns>
	IEnumerator SendUpdateTime ()
	{
		yield return new WaitForSeconds (2f);
		if (PrefabManager._instance != null) {
			if (PrefabManager._instance.GetLocalGun ().isLocal) {
				UIFishingMsg.GetInstance ().SndUpdateBossMatchTimeRequest ();
			}	
		}
		StartCoroutine (SendUpdateTime ());
	}

	/// <summary>
	/// 开始进行倒计时
	/// </summary>
	/// <returns>The time manager.</returns>
	/// <param name="timer">Timer.</param>
	IEnumerator UseTimeManager (long timer)
	{
		while (true) {
			if (timer > 0) {
//				Debug.LogError ("timer ==== " + timer);
				timer -= 1;
				timeText.text = TimeCountDownControl (timer);
//				 = time;
				if (timer == 0) {
					timeText.text = TimeCountDownControl (0);
//					timeText.text = time;
				}
			} else {
				break;
			}
			yield return new WaitForSeconds (1f);
		}
	}

	/// <summary>
	/// 时间转换
	/// </summary>
	/// <returns>The count down control.</returns>
	/// <param name="time">Time.</param>
	/// <param name="textDown">Text down.</param>
	string  TimeCountDownControl (long time)
	{
		long nDay = time / (3600 * 24);
		long nHours = (time / 3600) - (nDay * 24);
		long nMinutes = (time / 60) - (nHours * 60) - (nDay * 60 * 24);
		long nSecond = time - nMinutes * 60 - nHours * 3600 - nDay * 3600 * 24;

//		string dayStr = (nDay < 10) ? "" + nDay : nDay.ToString ();
		string hourStr = (nHours < 10) ? "0" + nHours : nHours.ToString ();
		string minuteStr = (nMinutes < 10) ? "0" + nMinutes : nMinutes.ToString ();
		string secondStr = (nSecond < 10) ? "0" + nSecond : nSecond.ToString ();

//		if (nDay >= 1) {
//			return	tmp = dayStr + "天" + hourStr + "时";
//		} else
		if (nHours < 1) {
			return	minuteStr + ":" + secondStr;
		} else if (nDay < 1) {
			return	hourStr + ":" + minuteStr + ":" + secondStr;//+ ":" + secondStr;
		} else {
			return "";
		}
	}
}
