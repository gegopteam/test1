using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using AssemblyCSharp;
using DG.Tweening;

public class AlmsCountID
{
	public int taskId;
	public int value;
	public int propID;
	public int almsGold;
	private static AlmsCountID instance;

	public static AlmsCountID Instance {
		get {
			if (instance == null) {
				instance = new AlmsCountID ();
			}
			return instance;
		}
	}
}

public class AlmsCountDown : MonoBehaviour
{

	//public Text timeText;
	bool canGetAlms = false;
	[HideInInspector]
	//领取的救济金金额，由服务器决定
//	public int almsGold = 0;


	MyInfo myInfo = null;
	Transform BG;
	Transform timeTextTran;
	Button getBtn;
	Image bgImage;
	Text noArrivaText;
	Text timeText;

	/// <summary>
	/// 获得救济金图片
	/// </summary>
	public Sprite almsSprite;
	/// <summary>
	/// 可以领取图片
	/// </summary>
	public Sprite receiveSprite;
	/// <summary>
	/// 奖励图片
	/// </summary>
	public Sprite almsRewardSprite;
	/// <summary>
	/// 救济金的获得图片
	/// </summary>
	public Sprite almsMoneySprite;
    public GameObject BuyLongCardTips;
	GameObject rewardContainer;
	float scaleTime = 1f;
	public static AlmsCountDown Instance;


	float timer = 0;

	bool isShow;

	void Awake ()
	{
		Instance = this;
	}

	void Start ()
	{
		myInfo = (MyInfo)Facade.GetFacade ().data.Get (FacadeConfig.USERINFO_MODULE_ID);
		//UpdateTime (666);
		BG = transform.Find ("Bg");
		timeTextTran = transform.Find ("ImgTimeTip");
		timeText = transform.Find ("ImgTimeTip/TimeText").GetComponent <Text> ();
		getBtn = transform.Find ("GetAlmsBtn").GetComponent <Button> ();
		bgImage = transform.Find ("NoArrivalmage").GetComponent <Image> ();
		noArrivaText = transform.Find ("NoArrivalmage/NoArrivalText").GetComponent <Text> ();
		Init ();
		if (AlmsCountID.Instance.value <= 0) {
			canGetAlms = true;
		}
		//Debug.Log ("AlmsCountID.Instance.value = " + AlmsCountID.Instance.value);
		//StartCoroutine (UpdateTheTime (1));

	}

	void Init ()
	{
		if (AppInfo.isReciveHelpMsg) {
			//bgImage.gameObject.SetActive (false);
		} 
		timeTextTran.gameObject.SetActive (true);
		//getBtn.interactable = false;
		getBtn.image.sprite = almsSprite;
		bgImage.transform.localScale = new Vector3 (0, 1, 1);
		bgImage.gameObject.SetActive (false);
		StartCoroutine (UpdateTheTime ());
	}

	void Update ()
	{
		timer += Time.deltaTime;
		if (timer >= scaleTime) {
			timer = 0;
			if (AlmsCountID.Instance.value > 0) {
				AlmsCountID.Instance.value -= 1;
				UpdateTime (AlmsCountID.Instance.value);
			} else {
				AlmsCountID.Instance.value = 0;
				getBtn.image.sprite = receiveSprite;
				timeTextTran.gameObject.SetActive (false);
				getBtn.interactable = true;
			}
		}
//		if (Input.GetKeyDown (KeyCode.A)) {
//			ShowRewardUnits (1000, 10000);
//		}
//		if (Input.GetKeyDown (KeyCode.K)) {
//			ShowRewardUnits (1000, 100000);
//		}
	}

	IEnumerator UpdateTheTime ()
	{
		while (true) {
			if (AlmsCountID.Instance.value > 0) {
				AlmsCountID.Instance.value -= 1;
				UpdateTime (AlmsCountID.Instance.value);
				break;
			} else {
				AlmsCountID.Instance.value = 0;
				getBtn.image.sprite = receiveSprite;
				timeTextTran.gameObject.SetActive (false);
				getBtn.interactable = true;
			}
			yield return  new WaitForSeconds (1f);
		}
	}

	public void UpdateTime (int timeSeconds)
	{
//		timeText.text = "还需等 <color=#FFFF33FF>" + ChangeTimeFormat (timeSeconds)
//		+ "</color> 才可领取<color=#FFFF33FF>救济金</color>";
		timeText.text = ChangeTimeFormat (timeSeconds);
          
	}

	/// <summary>
	/// 倒计时换算
	/// </summary>
	/// <returns>The time format.</returns>
	/// <param name="seconds">Seconds.</param>
	string ChangeTimeFormat (int seconds)
	{
//		Debug.Log ("seconds = " + seconds);
		if (seconds <= 0) {
			canGetAlms = true;
		}
		string temp = "";
		int m, s;
		m = (int)(seconds / 60);
		s = seconds % 60;
		//if (s >= 10)
		//	temp = "0"+m + ":" + s;
		//else
		//temp = "0"+m + ":0" + s;

		if (m < 10 && s < 10) {
			temp = "0" + m + ":0" + s;
		} else if (m < 10 && s >= 10) {
			temp = "0" + m + ":" + s;
		} else if (m >= 10 && s < 10) {
			temp = m + ":0" + s;
		} else {
			temp = m + ":" + s;
		}


		return temp;
	}

	public void Btn_GetAlms ()
	{
		Debug.Log ("canGetAlms = " + canGetAlms);
		if (canGetAlms) {
			//发送获取到的物品id,用户id,金额,任务id
			Facade.GetFacade ().message.task.SendGetHelpTaskReawrdRequest (AlmsCountID.Instance.propID, myInfo.userID, AlmsCountID.Instance.almsGold, AlmsCountID.Instance.taskId);
			bgImage.gameObject.SetActive (false);
			//Facade.GetFacade ().message.task.SendGetHelpTaskReawrdRequest (1, myInfo.userID, 10000, 9);
			//Debug.LogError ("AlmsCountID.Instance.propID, myInfo.userID, AlmsCountID.Instance.almsGold, AlmsCountID.Instance.taskId " + AlmsCountID.Instance.propID + " = " + myInfo.userID + " = " + AlmsCountID.Instance.almsGold + " = " + AlmsCountID.Instance.taskId);
		} else {
			//SetShow (!isShow);
			//Invoke ("HideBGImage", 2.5f);
			GameObject Window1 = UnityEngine.Resources.Load ("Window/WindowTipsThree")as UnityEngine.GameObject;
			GameObject WindowClone1 = UnityEngine.GameObject.Instantiate (Window1);
			UITipAutoNoMask ClickTips1 = WindowClone1.GetComponent<UITipAutoNoMask> ();
			ClickTips1.tipText.text = "領取時間未到,請耐心等待!";
		}
	}

	/// <summary>
	/// 动画
	/// </summary>
	/// <param name="toShow">If set to <c>true</c> to show.</param>
	void SetShow (bool toShow)
	{
		isShow = toShow;
		if (toShow) {
			bgImage.transform.DOScaleX (scaleTime, 0.3f); 
			noArrivaText.text = "領取時間未到,請耐心等待!";
			BGImageDoFade (1);
		} else {
			bgImage.transform.DOScaleX (0, 0.3f);
			BGImageDoFade (0);
		}
	}

	void HideBGImage ()
	{
		if (isShow) {
			bgImage.transform.DOScaleX (0, 0.3f);
			BGImageDoFade (0);
		}
	}

	void BGImageDoFade (float value)
	{
		bgImage.DOFade (value, 1f);
	}

	/// <summary>
	/// 销毁
	/// </summary>
	public void DestoryAlms ()
	{
		Destroy (this.gameObject); 
	}

	/// <summary>
	/// 领取失败提示
	/// </summary>
	public void TipsControl (string text)
	{
		GameObject Window = Resources.Load ("Window/WindowTips")as GameObject;
		GameObject nWindowClone = Instantiate (Window);
		UITipClickHideManager ClickTip = nWindowClone.GetComponent<UITipClickHideManager> ();
		ClickTip.text.text = text;
	}

	/// <summary>
	/// 显示奖励
	/// </summary>
	public void ShowRewardUnits (int propID, int propCount)
	{
        //在这里判断是不是龙卡 然后弹窗
		//奖励
		GameObject nWindow = Resources.Load ("Image/tools/RewardUnit") as GameObject; 
		//弹窗
		GameObject Window = Instantiate (Resources.Load ("Window/RewardWindow") as GameObject);//获得显示的外框
		//设置奖励的位置
		GameObject nUnit = Instantiate (nWindow, Window.transform) as GameObject;    //生成的奖励
		nUnit.GetComponent<RectTransform> ().anchorMax = new Vector2 (.5f, .5f);
		nUnit.GetComponent<RectTransform> ().anchorMin = new Vector2 (.5f, .5f);
//		nUnit.transform.localPosition = new Vector3 (14f, 28f, 0);
		nUnit.GetComponent<RectTransform> ().localPosition = new Vector3 (14f, 28f, nUnit.GetComponent<RectTransform> ().localPosition.z);

		//标题赋值为救济金标题
		Window.transform.Find ("RewardIn/Panel/Title").GetComponent <Image> ().sprite = almsRewardSprite;
		//设置标题宽高
		Window.transform.Find ("RewardIn/Panel/Title").GetComponent <RectTransform> ().sizeDelta = new Vector2 (506.8f, 218);

		Window.transform.Find ("RewardIn/Panel/Title/rewardtype").gameObject.SetActive (false);

		//icon图标获取
		Transform icon = nUnit.transform.Find ("Icon");
		//外框获取
		GameObject frame = nUnit.transform.Find ("frame").gameObject;
		//icon图标设置为救济金图标
		icon.gameObject.GetComponent<Image> ().sprite = almsMoneySprite;
		//如果服务器穿的数据为金币
		if (propID == FiPropertyType.GOLD) {
			//调整图形大小使其像素完美 官方api解释
			icon.GetComponent<Image> ().SetNativeSize ();
			icon.GetComponent<RectTransform> ().anchoredPosition += Vector2.up * 2;
			icon.GetComponent<RectTransform> ().localScale = Vector3.one * 0.35f;
		}
		int nCountValue = propCount;
		string nResult = nCountValue.ToString ();
		if (nCountValue > 10000) {
			nResult = nCountValue / 10000 + "萬";
		}
		nUnit.transform.Find ("NumText").gameObject.GetComponent<Text> ().text = "×" + nResult;
	
		Button temp = Window.transform.Find ("RewardIn/Panel/RewardButton").GetComponent <Button> ();

		temp.onClick.RemoveAllListeners ();
		temp.onClick.AddListener (delegate {
			Destroy (nUnit);
		});
	}


}
