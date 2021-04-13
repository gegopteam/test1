using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using AssemblyCSharp;

//
public enum ShwoEffect //分别对应枪座的左下，右下，左上，右上四个方位
{
	JINBIDIAOLUO,
	//0金币掉落
	STATRTSHOW,
	//1开始的时候闪光特效
	XUANZHUAN,
	//2旋转的特效
	WINSHOW,
	//3赢的时候的特效
}

public class UIManmonGameShow : MonoSingleton<UIManmonGameShow>
{

	private Quaternion T;
	private Quaternion V;

	public GameObject A;
	//背面
	public GameObject B;
	//正面


	public Sprite[] imagearray;
	//public GameObject C;
	private bool bol = true;

	public Text wintimeconnt;
	public Text ShowMonEy;

	public bool IsTakeoff = false;
	public long ShowMoneyNum;
	public BackageUI ManmonRankUI;
	RankInfo nInfo;
	//	public  List<FiManmonRankInfo> TestArry = new List<FiManmonRankInfo> ();
	public   List<FiRankInfo> TestArry = new List<FiRankInfo> ();

	//排行榜数组接收
	public List<FiProperty> rankcout = new List<FiProperty> ();

	public long currendgold;

	public Text showText;

	//显示特效
	public GameObject GoldEffect;
	public GameObject StartEffect;
	public GameObject ShowWinEffect;
	public GameObject RotationEffect;
	//下注的区间
	public Button[] BettingArray;

	void Awake ()
	{

//		FiManmonRankInfo s = new FiManmonRankInfo ();
//		s.usename = "1";
//		s.wintime = 11;
//		s.rewardnum = 1111;
//		TestArry.Add (s);
//
//		FiManmonRankInfo s1 = new FiManmonRankInfo ();
//		s1.usename = "2";
//		s1.wintime = 22;
//		s1.rewardnum = 2222;
//		TestArry.Add (s1);
//
//		FiManmonRankInfo s2 = new FiManmonRankInfo ();
//		s2.usename = "3";
//		s2.wintime = 33;
//		s2.rewardnum = 3333;
//		TestArry.Add (s2);
//
//		FiManmonRankInfo s3 = new FiManmonRankInfo ();
//		s3.usename = "4";
//		s3.wintime = 44;
//		s3.rewardnum = 4444;
//		TestArry.Add (s3);
//
//		FiManmonRankInfo s4 = new FiManmonRankInfo ();
//		s4.usename = "5";
//		s4.wintime = 55;
//		s4.rewardnum = 5555;
//		TestArry.Add (s4);
//
//		FiManmonRankInfo s5 = new FiManmonRankInfo ();
//		s5.usename = "6";
//		s5.wintime = 66;
//		s5.rewardnum = 6666;
//		TestArry.Add (s5);
		Facade.GetFacade ().message.rank.SendGetRankInfoRequest (2);
		Facade.GetFacade ().message.fishCommom.SendManMonRankReward ();
	
	}



	void Start ()
	{
		

		RefreshMoney (ShowMoneyNum);
//		wintimeconnt.text = 0.ToString ();
		DragonCardInfo nDragoninfo = (DragonCardInfo)Facade.GetFacade ().data.Get (FacadeConfig.UI_DRAGONCARD);
		wintimeconnt.text = nDragoninfo.ManmonWinTime.ToString ();
		nInfo = (RankInfo)Facade.GetFacade ().data.Get (FacadeConfig.RANK_MODULE_ID);
		B.transform.rotation = Quaternion.Euler (0, 90, 0);

		StartCoroutine (RecvRankInfo ());
		StartCoroutine ("ShowRankShowTime");
	}
	//5miao 请求排行榜一次
	IEnumerator ShowRankShowTime ()
	{
		int nRequestFrequent = 0;
		long rankRequstTime = 0;
		while (true) {
			nRequestFrequent++;
			if (nRequestFrequent >= 10) {
				rankRequstTime += 10;
				nRequestFrequent = 0;
			}
			if (rankRequstTime >= 50) {
				Facade.GetFacade ().message.rank.SendGetRankInfoRequest (2);
				rankRequstTime = 0;
			}
			yield return new WaitForSeconds (1.0f);
		}

	}

	IEnumerator RecvRankInfo ()
	{

		yield return new WaitForSeconds (1.5f);
		TestArry = nInfo.GetManmonArray ();
		ManmonRankUI.cellNumber = nInfo.GetManmonArray ().Count;
		//Debug.LogError ("testarray.cout" + TestArry.Count);
		ManmonRankUI.Refresh ();
	}

	public void ReshRank ()
	{
		TestArry = nInfo.GetManmonArray ();
		ManmonRankUI.cellNumber = nInfo.GetManmonArray ().Count;
		//Debug.LogError ("testarray.cout" + TestArry.Count);
		ManmonRankUI.Refresh ();
	}

	public void Init ()
	{

		A.transform.rotation = Quaternion.Euler (0, 0, 0);
		B.transform.rotation = Quaternion.Euler (0, 90, 0);

	}

	public void RefreshMoney (long moneynum)
	{
		ShowMonEy.text = moneynum.ToString ();
	}

	void Update ()
	{


//		以下两行都是让他绕自身旋转
//		A.transform.Rotate (new Vector3 (0, 90 * Time.deltaTime * 2.5f, 0));
//		A.transform.Rotate (Vector3.down, 3);
	}

	public void BtnClickYaoQianShu ()
	{
		ShowEffect (ShwoEffect.XUANZHUAN, false);
		DragonCardInfo nDragoninfo = (DragonCardInfo)Facade.GetFacade ().data.Get (FacadeConfig.UI_DRAGONCARD);
//		//Debug.LogError("currentGold"+PrefabManager._instance.GetLocalGun ().currentGold)
		if (nDragoninfo.CurrendGold < nDragoninfo.Curcheckgold) {
			string path = "Window/ManmonTps2";
			UnityEngine.GameObject WindowClone = AppControl.OpenWindow (path);
			WindowClone.gameObject.SetActive (true);
			return;
		}

		Facade.GetFacade ().message.fishCommom.SendManmonYaoQianShuType (1);
		EnableButton (false);
		ShowEffect (ShwoEffect.STATRTSHOW);
//		if (bol) {
		// Init();
//		InvokeRepeating ("BE", 0, 0.02f);
		//Debug.LogError ("111111111111111111111");
		StartCoroutine (Rotationstime ());
//			Invoke ("BE", 0.1f);
//			CancelInvoke ("BD");
//		} else {
//			//Init();
//			//Debug.LogError ("222222222222222222222222");
//			InvokeRepeating ("BD", 0.5f, 0.02f);
//			CancelInvoke ("BE");
////			Invoke ("BD", 0.1f);
//
//		}

//		InvokeRepeating ("BE", 0, 0.02f);
//		CancelInvoke ("BD");
	}

	public void BtnClickJuBaoPeng ()
	{
		ShowEffect (ShwoEffect.XUANZHUAN, false);
		DragonCardInfo nDragoninfo = (DragonCardInfo)Facade.GetFacade ().data.Get (FacadeConfig.UI_DRAGONCARD);
		//Debug.LogError ("currentGold" + nDragoninfo.CurrendGold + "Curcheckgold" + nDragoninfo.Curcheckgold);
		if (nDragoninfo.CurrendGold < nDragoninfo.Curcheckgold) {
			string path = "Window/ManmonTps2";
			UnityEngine.GameObject WindowClone = AppControl.OpenWindow (path);
			WindowClone.gameObject.SetActive (true);
			return;
		}
		Facade.GetFacade ().message.fishCommom.SendManmonYaoQianShuType (2);
		EnableButton (false);
		ShowEffect (ShwoEffect.STATRTSHOW);
//		if (bol) {
		// Init();
//		InvokeRepeating ("BE", 0, 0.02f);
		//Debug.LogError ("111111111111111111111");
		StartCoroutine (Rotationstime ());
		//			Invoke ("BE", 0.1f);
		//			CancelInvoke ("BD");
//		} else {
//			//Init();
//			//Debug.LogError ("222222222222222222222222");
//			InvokeRepeating ("BD", 0f, 0.02f);
//			CancelInvoke ("BE");
//			//			Invoke ("BD", 0.1f);
//
//		}

		//		InvokeRepeating ("BE", 0, 0.02f);
		//		CancelInvoke ("BD");
	}


	public void RototionMove ()
	{
		InvokeRepeating ("BE", 0, 0.02f);

	}

	IEnumerator Rotationstime ()
	{
		yield return new WaitForSeconds (2f);
		CancelInvoke ("BE");
		//这里是赢的时候闪光特效 目前有点层级问题以后确定需要就加
//		ShowEffect (ShwoEffect.WINSHOW);
		yield return new WaitForSeconds (1f);
		InvokeRepeating ("BD", 0f, 0.02f);
		yield return new WaitForSeconds (2f);
		EnableButton (true);
		CancelInvoke ("BD");
		ShowEffect (ShwoEffect.XUANZHUAN);

	}

	//刷新服务器给的数据类型然后显示牌面
	public void ShowType (int type, long Result, int showtime, double showtextnum)
	{
		wintimeconnt.text = showtime.ToString ();
		if (showtextnum == 0) {
			showText.gameObject.SetActive (false);
		} else {
			showText.gameObject.SetActive (true);
			showText.text = "X" + (showtextnum * 100).ToString () + "%";
		}

		if (type == 1) {
			if (Result != 0) {
				B.gameObject.transform.GetComponentInChildren<Image> ().sprite = imagearray [0];
			} else {
				B.gameObject.transform.GetComponentInChildren<Image> ().sprite = imagearray [1];
			}
		} else if (type == 2) {
			if (Result != 0) {
				B.gameObject.transform.GetComponentInChildren<Image> ().sprite = imagearray [1];
			} else {
				B.gameObject.transform.GetComponentInChildren<Image> ().sprite = imagearray [0];
			}
		}
	}
	//取消下注按钮功能
	public void EnableButton (bool isenable)
	{
		BettingArray [0].enabled = isenable;
		BettingArray [1].enabled = isenable;
	}

	public void DealtyShow ()
	{
		StartCoroutine (ShowLose ());
	}

	IEnumerator ShowLose ()
	{
		yield return new WaitForSeconds (2.5f);
		DragonCardInfo nDragoninfo = (DragonCardInfo)Facade.GetFacade ().data.Get (FacadeConfig.UI_DRAGONCARD);
	
		string path = "Window/ManmonTps1";

		UnityEngine.GameObject WindowClone = AppControl.OpenWindow (path);
		WindowClone.gameObject.SetActive (true);
		ManmonTps1 show = WindowClone.gameObject.GetComponent<ManmonTps1> ();
		show.textshow.text = "您未能猜中，是否投入" + nDragoninfo.Curcheckgold + "金幣繼續參與競猜";
	}

	public void ShowEffect (ShwoEffect type, bool IsShow = true)
	{
		switch (type) {
		case ShwoEffect.JINBIDIAOLUO:
			GoldEffect.gameObject.SetActive (IsShow);
			GoldEffect.gameObject.transform.GetComponentInChildren<ParticleSystem> ().Play ();
			AudioManager._instance.PlayEffectClip (AudioManager.effect_manmongold);
			break;	
		case ShwoEffect.STATRTSHOW:
			StartEffect.gameObject.SetActive (IsShow);
			StartEffect.gameObject.transform.GetComponentInChildren<ParticleSystem> ().Play ();
			break;
		case ShwoEffect.XUANZHUAN:
			RotationEffect.gameObject.SetActive (IsShow);
			RotationEffect.gameObject.transform.GetComponentInChildren<ParticleSystem> ().Play ();
			break;
		case ShwoEffect.WINSHOW:
			ShowWinEffect.gameObject.SetActive (IsShow);
			ShowWinEffect.gameObject.transform.GetComponentInChildren<ParticleSystem> ().Play ();
			break;
		}


	}



	private void BE ()
	{
//		//Debug.LogError ("______________A______________");

		// C.transform.GetComponent<Button>().enabled = false;
		T = Quaternion.Euler (0, 90, 0);
		V = Quaternion.Euler (0, 0, 0);
	
		A.transform.rotation = Quaternion.RotateTowards (A.transform.rotation, T, 4f);
		if (A.transform.eulerAngles.y > 89 && A.transform.eulerAngles.y < 91) {
			
			B.transform.rotation = Quaternion.RotateTowards (B.transform.rotation, V, 4f);

//			bol = false;
//			BD ();
			//  C.transform.GetComponent<Button>().enabled = true;
		}
	}

	private void BD ()
	{
	
		//返回动作
//		//Debug.LogError ("____________B_______________");
		// C.transform.GetComponent<Button>().enabled = false;
		T = Quaternion.Euler (0, 90, 0);
		V = Quaternion.Euler (0, 0, 0);
		B.transform.rotation = Quaternion.RotateTowards (B.transform.rotation, T, 4f);
		if (B.transform.eulerAngles.y > 89 && B.transform.eulerAngles.y < 91) {//加这个判断是因为写的是90度，但是实际不可能直接是90度整，所以加了一个这样的判断
	
			A.transform.rotation = Quaternion.RotateTowards (A.transform.rotation, V, 4f);
	
			bol = true;
			//C.transform.GetComponent<Button>().enabled = true;
		}
	
	}

	public void OpenRule ()
	{
		string path = "Window/ManmonRule";
		GameObject WindowClone = AppControl.OpenWindow (path);
		WindowClone.gameObject.SetActive (true);
	}

	public void RreshWintime (int timecount)
	{
		wintimeconnt.text = timecount.ToString () + "分";
	}

	//	public void AppendCoinData (List<FiRankInfo> nCoinData)
	//	{
	//		//Debug.LogError ("ncccccccccc" + nCoinData.Count);
	//		TestArry = nCoinData;
	//		ManmonRankUI.cellNumber = TestArry.Count;
	//		ManmonRankUI.Refresh ();
	//	}
	//
	public void Btnclose ()
	{
		Destroy (this.gameObject);
		Facade.GetFacade ().message.fishCommom.SendManmonExitGame ();
	}

	public void RewardMoney ()
	{
		string momey = ShowMonEy.text;
		if (int.Parse (momey) <= 0) {
			Destroy (this.gameObject);
			return;
		}
		//Debug.LogError ("222222");
		string path = "Window/WindowManmomTips1";
		GameObject WindowClone = AppControl.OpenWindow (path);
		WindowClone.gameObject.SetActive (true);
		WindowManmomTips1 tips1 = WindowClone.gameObject.GetComponent<WindowManmomTips1> ();
		//Debug.LogError ("wintime" + ShowMonEy.text);

		tips1.ShowMoney (int.Parse (momey));
	}
}
