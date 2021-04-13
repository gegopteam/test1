using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Text.RegularExpressions;
using System.Text;
using UnityEngine.EventSystems;
using UnityEngine.Events;
using System;
using Random = UnityEngine.Random;
using AssemblyCSharp;

public class Loading : MonoBehaviour
{
	//异步对象
	public GameObject effect;
	//[Range(0,1)]
	//public float InvokeTime=1;
	private AsyncOperation _asyncOperation;
	//显示tips的文本
	private Text _tip;
	private Image backgroundImage;
	//tips的集合
	private string[] _Halltips = new string[] {
		"每日登錄就可領大量金幣，不要錯過哦!",
        "完成每日任務，領取活躍獎勵！",
        "每日可以領取好友贈送的免費金幣，VIP等級越高，好友位上限越高！"
	};
	private string[] _tips = new string[] {
		//"購買月卡或者VIP等級達到1級即可解鎖自動開砲功能，解放雙手！",
        "VIP等級越高，可以解鎖更多炫酷的強力砲台！",
        "使用炮倍數越高，捕魚獲得的金幣也越多！",
        // "使用鑽石也可解鎖炮倍數哦！",
        "特惠禮包贈送金幣比例是最高的，千萬不要錯過！",
        "自動開砲功能可通過購買龍卡解鎖！",
        "每日登錄就可領大量金幣，不要錯過哦!",
        // "完成每日任務，領取活躍獎勵！",
        // "每日可以領取好友贈送的免費金幣，VIP等級越高，好友位上限越高！",
        // "海神寶藏全新升級3倍場，開心爆金",
        // "BOSS場全是大魚，讓你打到爽",
        "購買龍卡可以讓您享受更多福利",
        "新增5倍BOSS場，最高1炮可爆1億金幣",
        "每日的限時獵殺賽可獲得3500萬的金幣獎勵！",
        "BOSS場爆率提高600%，BOSS更容易擊殺！",
        "通過幸運財神玩法，可以輕鬆獲得雙倍的獎勵！",
        "神龍寶藏的轉盤玩法每天送出5億獎金，不容錯過。",
	};
	//更新tips的时间间隔
	private const float _updateTime = 3f;
	//上次更新的时间
	private float _lastUpdateTime = 0;
	//滑动条
	public Slider _slider;
	//显示进度的文本
	//private Text _progress;
	// Use this for initialization
	public Sprite[] bgSprite;
	Image logoImage;
	MyInfo mInfo = null;

	void Start ()
	{
        Screen.autorotateToLandscapeLeft = true;
        Screen.autorotateToLandscapeRight = true;
        Screen.autorotateToPortrait = false;
        Screen.autorotateToPortraitUpsideDown = false;
        _slider.value = 0;
		_tip = transform.Find ("Text").GetComponent<Text> ();
		backgroundImage = transform.Find ("backGround_image").GetComponent <Image> ();
		logoImage = transform.Find ("backGround_image/logo_image").GetComponent <Image> ();
		//_slider = transform.Find("Slider").GetComponent<Slider>();
		//_progress = GameObject.Find("Progress").GetComponent<Text>();
		SetTip ();
		SetBackGround ();

		//如果是要去渔场，那么移步加载渔场，如果要去大厅，那么等待大厅完成后，加载大厅
		mInfo = (MyInfo)Facade.GetFacade ().data.Get (FacadeConfig.USERINFO_MODULE_ID);
		if (mInfo.TargetView == AppView.FISHING) {
			AppControl.SetView (AppView.FISHING);
			StartCoroutine (LoadAsync ("Fishing"));
		} else if (mInfo.TargetView == AppView.HALL) {
			StartCoroutine (CheckLoginState ());
		}
		//Invoke ("InvokeMethod", InvokeTime);
	}

	void InvokeMethod ()
	{
		effect.SetActive (true);
	}

	IEnumerator CheckLoginState ()
	{
		//登陆成功了，那么加载渔场
		while (true) {
			yield return new WaitForEndOfFrame ();
			//登陆成功了，那么加载大厅界面
			if (mInfo.loginInfo != null) {
				//Debug.LogError ("start enter hall sence == : " + mInfo.loginInfo.reuslt);
				if (mInfo.loginInfo.reuslt == 0) {
					//Debug.LogError ("start enter hall sence");
					AppControl.SetView (AppView.HALL);
					StartCoroutine (LoadAsync ("Hall"));
				} else {
					//Debug.LogError ("start enter hall sence error");
					Invoke ("ReturnToLoginView", 1);
				}
				break;
			}
		}
	}

	void ReturnToLoginView ()
	{
		AppControl.ToView (AppView.LOGIN);
	}

	// Update is called once per frame
	void Update ()
	{
		//首先判断是否为空，其次判断是否加载完毕
		if (_asyncOperation != null && !_asyncOperation.allowSceneActivation) {
			//开始更新tips
			if (Time.time - _lastUpdateTime >= _updateTime) {
				_lastUpdateTime = Time.time;
				SetTip ();
			}
		}
		if (_slider.value > 0.11f && !effect.activeInHierarchy) {
			effect.SetActive (true);
		}
	}

	/// <summary>
	/// 设置加载标签
	/// </summary>
	private void SetTip ()
	{
		int temp = Random.Range (0, _tips.Length);
		_tip.text = _tips [temp];
	}

	void  SetBackGround ()
	{
		if (BossMatchScript.isToLoadBoss) {
			if (logoImage.gameObject.activeSelf) {
				logoImage.gameObject.SetActive (false);
			}
			backgroundImage.sprite = bgSprite [1];
			BossMatchScript.isToLoadBoss = false;
		} else {
			if (!logoImage.gameObject.activeSelf) {
				logoImage.gameObject.SetActive (true);
			}
			backgroundImage.sprite = bgSprite [0];
		}
	}

	/// <summary>
	/// 携程进行异步加载场景
	/// </summary>
	/// <param name="sceneName">需要加载的场景名</param>
	/// <returns></returns>
	IEnumerator LoadAsync (string sceneName)
	{
//		Debug.LogError ("startcon");
		//当前进度
		int currentProgress = 0;
		//目标进度
		int targetProgress = 0;
		_asyncOperation = Application.LoadLevelAsync (sceneName);
		//unity 加载90%
		_asyncOperation.allowSceneActivation = false;
		while (_asyncOperation.progress < 0.9f) {
			targetProgress = (int)_asyncOperation.progress * 100;
			//平滑过渡
			while (currentProgress < targetProgress) {
				++currentProgress;
				//_progress.text= String.Format("{0}{1}",currentProgress.ToString(),"%");
				_slider.value = (float)currentProgress / 100;
				yield return new WaitForEndOfFrame ();
			}
		}
		//自行加载剩余的10%
		targetProgress = 100;
		while (currentProgress < targetProgress) {
			++currentProgress;
			//_progress.text = String.Format("{0}{1}", currentProgress.ToString(), "%");
			_slider.value = (float)currentProgress / 100;
			yield return new WaitForEndOfFrame ();
		}
		_asyncOperation.allowSceneActivation = true;
	}
}
//char.IsLetterOrDigit (arr [i])||