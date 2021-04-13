using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using AssemblyCSharp;

public class FishingUpTurnTable : MonoBehaviour
{

	public static FishingUpTurnTable _instance;
	public Image turnImg;
	//捕鱼的进度
	public long speedOfProFishingGoldNum = 0;
	public Slider speedOfProInFishing;
	public Text speedOfProInFishingTxt;
	public Sprite[] turnSprite;
	[HideInInspector]
	public int roomType = 0;
	//需要获取重置时间，进度，场次
	//进度条增长也在这里加
	RoomInfo myRoomInfo = null;

	//三种奖池流水
	const long SilverTurnGoldNum = 50000000;
	const long GoldTurnGoldNum = 150000000;
	const long GodTurnGoldNum = 250000000;
	[HideInInspector]
	public long GoldCoinPoolNum = 0;
	[HideInInspector]
	public long liuShuiTime = 0;
	[HideInInspector]
	public int prizeType = 0;
	[HideInInspector]
	public long prizeValue = 0;
	[HideInInspector]
	public int prizeIndex = 0;
	[HideInInspector]
	public DragonCardInfo shenLongData;
	public GameObject effectZhuanPanSilver;
	public GameObject effectZhuanPanGold;
	public GameObject effectZhuanPanGod;
	//倒计时
	int hour = 0;
	int minute = 0;
	int second = 0;
	float timeSpend = 0;
	bool timeStop = false;

	void Awake ()
	{
		if (_instance == null)
			_instance = this;
		Facade.GetFacade ().message.fishCommom.SendLongPoolRewardRequest (0);
		myRoomInfo = DataControl.GetInstance ().GetRoomInfo ();
	}

	void Start ()
	{
		shenLongData = (DragonCardInfo)Facade.GetFacade ().data.Get (FacadeConfig.UI_DRAGONCARD);
		//Facade.GetFacade().message.fishCommom.SendLongPoolRewardRequest(0);
		speedOfProFishingGoldNum = shenLongData.LongLiuShui;
		liuShuiTime = shenLongData.LongTime;
//		Debug.Log (roomType);
		//刷新数据
        SetSpeedOfPro(speedOfProFishingGoldNum);


        if (myRoomInfo.roomMultiple==0||myRoomInfo.roomMultiple==4||GameController._instance.isExperienceMode)
        {

        }
        else
        {
            StartCoroutine(GetRefreshGoldPoolLocal(roomType));
            StartCoroutine(GetRefershGoldPoolServer(roomType));
        }
		//SetTurnTable();
	}

	void SetTurnTable ()
	{
		turnImg.sprite = turnSprite [roomType - 1];
	}

	void Update ()
	{
		//进度倒计时
		if (!timeStop) {
			timeSpend += Time.deltaTime;
			if (timeSpend >= 1) {
				liuShuiTime = liuShuiTime - 1;
				//Debug.Log(liuShuiTime);
				timeSpend = 0;
				if (liuShuiTime <= 0) {
					timeStop = true;
					liuShuiTime = 0;
				}
			}
		}
	}

	//进度条设置
	public void SetSpeedOfPro (long liuShui)
	{
		//        Debug.Log("setSpeedofpro");
		//体验场
		if (GameController._instance.isExperienceMode) {
			this.gameObject.SetActive (false);
			return;
		}
		switch (myRoomInfo.roomMultiple) {
		case 0:
		case 4:
			this.gameObject.SetActive (false);
			return;
		case 1:
			roomType = 1;
			SetRoomUpTurnTale (roomType, liuShui, SilverTurnGoldNum, effectZhuanPanSilver);
			break;
		//三倍场
		case 2:
			roomType = 2;
			SetRoomUpTurnTale (roomType, liuShui, GoldTurnGoldNum, effectZhuanPanGold);
			break;
		//五倍场
		case 3:
        case 5:
			roomType = 3;
			SetRoomUpTurnTale (roomType, liuShui, GodTurnGoldNum, effectZhuanPanGod);
			break;
        case 6:
			this.gameObject.SetActive(false);
			return;
		default:
			break;
		}
		SetTurnTable ();
	}

	GameObject WindowClone;
	long GoldPoolNumLocal;

	public void OpenTurnTable ()
	{
		//Debug.Log(shenLongData.LongLiuShui + "流水---------------------------------------");
		//Debug.Log(shenLongData.LongTime+"时间-------------------------------------------");
		//Debug.Log(shenLongData.userId+"Id___________________----------------------------");
		//Debug.Log("捕鱼进度"+speedOfProFishingGoldNum);
		string path = "Window/ShenlongTurnTableWindow";
		WindowClone = AppControl.OpenWindow (path);
		WindowClone.transform.Find ("bgImg/numBase/Text").GetComponent<Text> ().text = GoldPoolNumLocal.ToString ();
		WindowClone.SetActive (true);
	}

	public void BuyLiuShuiTime ()
	{
		// Debug.Log(shenLongData.LongTime + "时间--------");
		liuShuiTime = shenLongData.LongTime;
	}

	bool firstBool = true;

	void SetRoomUpTurnTale (int nroomType, long nliuShui, long TurnGoldNum, GameObject effectZhuanPan)
	{
		speedOfProInFishing.value = (float)(nliuShui) / TurnGoldNum;
		int numTxt1 = (int)((float)(nliuShui) / TurnGoldNum * 100f);
		if (numTxt1 >= 100) {
			numTxt1 = 100;
			//transform.Find("TurnTable/Effect-zhuanpan-yinlong").gameObject.SetActive(true);
			effectZhuanPan.SetActive (true);
		}
		if (WindowClone != null) {
			WindowClone.GetComponent<ShenlongTurnTable> ().pace.value = (float)(nliuShui) / TurnGoldNum;
			WindowClone.GetComponent<ShenlongTurnTable> ().paceTxt.text = numTxt1.ToString () + "%";
		}
		speedOfProInFishingTxt.text = numTxt1.ToString () + "%";
		if (shenLongData.GoldPool.Count != 0 && firstBool) {
			GoldCoinPoolNum = shenLongData.GoldPool [nroomType - 1];
			transform.Find ("base/Text").GetComponent<Text> ().text = GoldCoinPoolNum.ToString ();
			firstBool = false;
		}
	}

	long gold2 = 0;
	bool numboll = true;
	//本地假数据刷新
    IEnumerator GetRefreshGoldPoolLocal (int nRoomType)
	{
		yield return new WaitForSeconds (0.5f);
		int num = Random.Range (1, 100);
		int rangNum = Random.Range (1000, 5000);
		//if (num < 3) {
		//	rangNum = -rangNum;
		//}
        long gold1 = (long)shenLongData.GoldPool [nRoomType - 1] + (long)rangNum;

		if (numboll) {
            gold2 = gold2 + gold1 - (long)shenLongData.GoldPool [nRoomType - 1];
			numboll = false;
		} else {
            gold2 = gold2 + (long)rangNum - shenLongData.GoldPool [nRoomType - 1];
			numboll = false;
		}
		gold2 = gold1 + gold2;
        GoldPoolNumLocal = shenLongData.GoldPool [nRoomType - 1] + rangNum;
		transform.Find ("base/Text").GetComponent<Text> ().text = (gold2).ToString ();
		if (WindowClone != null) {
			WindowClone.transform.Find ("bgImg/numBase/Text").GetComponent<Text> ().text = (gold2).ToString ();
		}
		//gold2 = gold1 + gold2;
        StartCoroutine (GetRefreshGoldPoolLocal (roomType));
	}

    IEnumerator GetRefershGoldPoolServer (int nRoomTyp)
	{
		yield return new WaitForSeconds (60);
		Facade.GetFacade ().message.fishCommom.SendLongPoolRewardRequest (0);
		gold2 = 0;
		numboll = true;
		if (shenLongData.GoldPool.Count != 0) {
            transform.Find ("base/Text").GetComponent<Text> ().text = shenLongData.GoldPool [nRoomTyp - 1].ToString ();
		}
        Debug.Log ("shenLongData.GoldPool [myRoomInfo.roomMultiple - 1]" + shenLongData.GoldPool [nRoomTyp - 1]);
        StartCoroutine (GetRefershGoldPoolServer (roomType));
	}

	private void OnDestroy ()
	{
		shenLongData.LongTime = liuShuiTime;
	}
}