using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using AssemblyCSharp;
using UnityEngine.UI;

public class UIBigWinner : MonoBehaviour,IUiMediator
{

	public BackageUI BigWinRankUI;
	public const int RICH = 1;
	//荣耀段位排行榜数据
	public static List<FiRankInfo> nRankData;
	//个人段位奖励数据
	private List<FiRewardStructure> rewardinfo;

	NobelInfo horodatainfo;
	RankInfo nInfo;
	//未上榜标志
	public Image NointoDuanwei;
	//段位显示数字
	public Text[] duanweitextshow;
	//段位奖励类型
	public Image[] rewardtype;
	//段位奖励数目
	public Text[] rewardtext;
	//是否充值或者龙卡项目火焰
	public Image[] accelerate;

	public Text ShowDuanwei;

	public Text ShowTime;

	public Text Qishu;
	string timeTmp;
	Transform dragonControl;
	bool isopenTip = true;
	// Use this for initialization
	void Start ()
	{
		dragonControl = transform.Find ("BGplane");
		//if (Facade.GetFacade ().config.isIphoneX2 ()) {
		//	dragonControl.localScale = new Vector3 (.9f, .9f, .9f);
		//}
		if (GameController._instance == null) //渔场和大厅里赋值的摄像机不同
            gameObject.GetComponent<Canvas> ().worldCamera = GameObject.FindGameObjectWithTag ("MainCamera").GetComponent<Camera> ();
		else
			gameObject.GetComponent<Canvas> ().worldCamera = ScreenManager.uiCamera;

		Facade.GetFacade ().ui.Add (FacadeConfig.UI_NOBEL_ID, this);
		UIColseManage.instance.ShowUI (this.gameObject);
		horodatainfo = (NobelInfo)Facade.GetFacade ().data.Get (FacadeConfig.UI_NOBEL_ID);

		nInfo = (RankInfo)Facade.GetFacade ().data.Get (FacadeConfig.RANK_MODULE_ID);

		Invoke ("ReceveRankData", 0.1f);
		Invoke ("ReceveSatrtData", 0.2f);


	}
	
	// Update is called once per frame
	void Update ()
	{
		
	}

	private void ReceveSatrtData ()
	{
		ShowStartInfo ();
		ShowReward (horodatainfo.CurrentDuanwei);
	}

	private void ShowStartInfo ()
	{
//		ShowTime.text = TimeCountDownControl (horodatainfo.Showtime);
	//	StartCoroutine (UseTimeManager (horodatainfo.Showtime));

        StartCoroutine(UseTimeManagerEx(horodatainfo.Showtime));//新的晋级赛时间刷新
		Qishu.text = "A " + horodatainfo.CurrentQi.ToString () + "BCD";
		if (horodatainfo.IsMonthType != 0) {
			accelerate [0].gameObject.SetActive (false);
			accelerate [4].gameObject.SetActive (true);
		}
		if (horodatainfo.IsToUp != 0) {
			accelerate [1].gameObject.SetActive (false);
			accelerate [5].gameObject.SetActive (true);
		}
		if (horodatainfo.ISbossmatchdouble != 0) {
			accelerate [2].gameObject.SetActive (false);
			accelerate [6].gameObject.SetActive (true);
		}

		ShowDuanweiInfo ();
	}

	public void OpenRule (int type)
	{
		string path = "Window/MainExplain";
		GameObject windowClone = AppControl.OpenWindow (path);
		if (type == 1) {
			windowClone.transform.Find ("HallOfHonriExplain").gameObject.SetActive (false);
			windowClone.transform.Find ("MatchExplain").gameObject.SetActive (false);
			windowClone.transform.Find ("DuanExplain").gameObject.SetActive (true);
		} else {
			windowClone.transform.Find ("HallOfHonriExplain").gameObject.SetActive (false);
			windowClone.transform.Find ("MatchExplain").gameObject.SetActive (true);
			windowClone.transform.Find ("DuanExplain").gameObject.SetActive (false);
		}


	}

	public void OpenHoro ()
	{
		DataControl.GetInstance ().PushSocketSnd (FiEventType.SEND_XL_RONGYURANK_RESQUSET, null);
		transform.Find ("BGplane/rightButton/HonorHall").GetComponent<Button> ().enabled = false; //点击发送消息后先暂时将按钮关闭
		StartCoroutine (CloseBtn ());
	}

	/// <summary>
	/// 打开我的段位窗口,打开前先发送消息,收到后再打开
	/// </summary>
	public void OpenMyDuanwei ()
	{
		Facade.GetFacade ().message.nobel.SendGetPaiWeiPrize ();
		transform.Find ("BGplane/rightButton/CheckRank").GetComponent<Button> ().enabled = false;//点击发送消息后先暂时将按钮关闭
		StartCoroutine (CloseBtn ());
	}



	void ShowDuanweiInfo ()
	{
		duanweitextshow [0].text = (horodatainfo.CurrentDuanwei - 1).ToString ();
		ShowDuanwei.text = horodatainfo.CurrentDuanwei.ToString ();	
		if (horodatainfo.Nrankinfo == 0) {
			NointoDuanwei.gameObject.SetActive (true);
			duanweitextshow [1].gameObject.SetActive (false);
		} else {
			duanweitextshow [1].text = horodatainfo.CurrentDuanwei.ToString ();
		}

		duanweitextshow [2].text = (horodatainfo.CurrentDuanwei + 1).ToString ();

		if (horodatainfo.CurrentDuanwei == 0) {
			duanweitextshow [0].text = "0";
			ShowDuanwei.text = "1";
			NointoDuanwei.gameObject.SetActive (true);
		} else if (horodatainfo.CurrentDuanwei == 25) {
			duanweitextshow [2].text = "25";
			duanweitextshow [0].gameObject.SetActive (false);
			duanweitextshow [2].gameObject.SetActive (false);
		}

	}

	private void ShowReward (int CurrentDuanwei)
	{
		if (CurrentDuanwei == 0) {
			CurrentDuanwei = 6001;
		} else {
			CurrentDuanwei = CurrentDuanwei + 6000;
		}
		rewardinfo = Facade.GetFacade ().message.reward.GetRewardInfo (CurrentDuanwei);

		for (int i = 0; i < rewardinfo.Count; i++) {
			Debug.LogError ("rewardcount" + rewardinfo [i].rewardPro.Count);
			for (int j = 0; j < rewardinfo [i].rewardPro.Count; j++) {
				rewardtype [j].sprite = FiPropertyType.GetSprite (rewardinfo [i].rewardPro [j].type);
				Debug.LogError ("rewardtype" + rewardinfo [i].rewardPro [j].type);
				rewardtext [j].text = rewardinfo [i].rewardPro [j].value.ToString ();

			}
		}
	}

	public void OnButton ()
	{
		AudioManager._instance.PlayEffectClip (AudioManager.effect_closePanel);
		//		Destroy (this.gameObject);
		UIColseManage.instance.CloseUI();
		//if (DataControl.GetInstance().GetMyInfo().isLevelupCanGet) {
		//	string path = "Window/NewLevelupGrade";
		//	GameObject WindowClone = AppControl.OpenWindow(path);
		//	WindowClone.SetActive(true);
		//}

		if (UIHallCore.Instance != null)
		{
			if (DataControl.GetInstance().GetMyInfo().isLevelupCanGet)
			{
				if (UIHallCore.isFristBigWineer) {
					UIHallCore.Instance.NewhandUpgrade();
					UIHallCore.isFristBigWineer = false;
				}
			}
		}

		
		//transform.gameObject.SetActive (false);
	}

	public void OnRecvData (int nType, object nData)
	{
		if (nType == RICH) {
			nRankData = horodatainfo.mBigWinArray;
			int selfindex = horodatainfo.IndexMyselfRank (nRankData);
			Debug.LogError ("coin的長度是" + nRankData.Count + "index" + selfindex);
			BigWinRankUI.cellNumber = nRankData.Count;
			BigWinRankUI.RefeshIndex (selfindex);
		}

	}

	public void OnInit ()
	{
		Facade.GetFacade ().message.rank.SendGetRankInfoRequest (RICH);
	}

	public void ReceveRankData ()
	{
		OnRecvData (RICH, null);
	}

	//获取时间
	public  string  TimeCountDownControl (long time)
	{
		string tmp;
		long nDay = time / (3600 * 24);
		long nHours = (time / 3600) - (nDay * 24);
		long nMinutes = (time / 60) - (nHours * 60) - (nDay * 60 * 24);
		long nSecond = time - nMinutes * 60 - nHours * 3600 - nDay * 3600 * 24;

		string dayStr = (nDay < 10) ? "" + nDay : nDay.ToString ();
		string hourStr = (nHours < 10) ? "0" + nHours : nHours.ToString ();
		string minuteStr = (nMinutes < 10) ? "0" + nMinutes : nMinutes.ToString ();
		string secondStr = (nSecond < 10) ? "0" + nSecond : nSecond.ToString ();

		if (nDay >= 1) {
			return	tmp = hourStr + "時" + minuteStr + "分" + secondStr + "秒";
		} else if (nHours < 1) {
			return	tmp = minuteStr + "分" + secondStr + "秒";
		} else if (nDay < 1) {
			return	tmp = hourStr + "時" + minuteStr + "分" + secondStr + "秒";
		} else {
			return "";
		}
	}

    //获取时间 晋级赛的时间修改
    public string TimeCountDownControlEx(long time)
    {
        string tmp;
        //long nDay = time / (3600 * 24);
        long nHours = (time / 3600);// - (nDay * 24);
        long nMinutes = (time / 60) - (nHours * 60) ;//- (nDay * 60 * 24);
        long nSecond = time - nMinutes * 60 - nHours * 3600;// - nDay * 3600 * 24;

       // string dayStr = (nDay < 10) ? "" + nDay : nDay.ToString();
        string hourStr = (nHours < 10) ? "0" + nHours : nHours.ToString();
        string minuteStr = (nMinutes < 10) ? "0" + nMinutes : nMinutes.ToString();
        string secondStr = (nSecond < 10) ? "0" + nSecond : nSecond.ToString();
        return tmp = hourStr + "時" + minuteStr + "分" + secondStr + "秒";

        //if (nDay >= 1)
        //{
        //    return tmp = hourStr + "时" + minuteStr + "分" + secondStr + "秒";
        //}
        //else if (nHours < 1)
        //{
        //    return tmp = minuteStr + "分" + secondStr + "秒";
        //}
        //else if (nDay < 1)
        //{
        //    return tmp = hourStr + "时" + minuteStr + "分" + secondStr + "秒";
        //}
        //else
        //{
        //    return "";
        //}
    }
    //新加的晋级赛时间显示
    IEnumerator UseTimeManagerEx(long timer)
    {
        while (true)
        {
            if (timer > 0)
            {
                timer -= 1;
                ShowTime.text = TimeCountDownControlEx(timer);

                if (timer == 0)
                {
                    ShowTime.text = TimeCountDownControlEx(0);
                }
            }
            else
            {
                break;
            }
            yield return new WaitForSeconds(1f);
        }
    }

	IEnumerator UseTimeManager (long timer)
	{
		while (true) {
			if (timer > 0) {
				timer -= 1;
				ShowTime.text = TimeCountDownControl (timer);

				if (timer == 0) {
					ShowTime.text = TimeCountDownControl (0);
				}
			} else {
				break;
			}
			yield return new WaitForSeconds (1f);
		}
	}


	public void OnRelease ()
	{
		
	}

	/// <summary>
	/// 点击按钮后按钮组件等待打开
	/// </summary>
	/// <returns>The button.</returns>
	IEnumerator CloseBtn ()
	{
		yield return new WaitForSeconds (0.5f);
		transform.Find ("BGplane/rightButton/CheckRank").GetComponent<Button> ().enabled = true;
		transform.Find ("BGplane/rightButton/HonorHall").GetComponent<Button> ().enabled = true;
	}

	public void OpenTip ()
	{
		
		if (isopenTip) {
			accelerate [3].gameObject.SetActive (true);
			isopenTip = false;
			StartCoroutine ("ShowTip");

		} else {
			accelerate [3].gameObject.SetActive (false);
			StopCoroutine ("ShowTip");
			isopenTip = true;
		}
	}

	IEnumerator  ShowTip ()
	{
//		if (isopenTip) {
//			Debug.LogError ("11111111");
//			yield return null;
//		} else {
		yield return new WaitForSeconds (5f);
		accelerate [3].gameObject.SetActive (false);
//			isopenTip = true;
//		}

	}

	bool WaitePrizeData ()
	{
		return horodatainfo.prizeState == null;
	}

}
