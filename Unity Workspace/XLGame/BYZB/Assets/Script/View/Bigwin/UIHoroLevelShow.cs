using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using AssemblyCSharp;
using UnityEngine.UI;

public class UIHoroLevelShow : MonoBehaviour
{

	public Text Level;
	private List<FiProperty> mRewardArray = new List<FiProperty> ();
	public GameObject ShowISnull;
	public Image[] ShowIsnullimage;

	//个人段位奖励数据
	private List<FiRewardStructure> rewardinfo;

	private FiRewardStructure reward;
	//	//当前是否充值
	//	public int IsToUp;
	//	//是否是月卡类型
	//	public int IsMonthType;
	//	//是否是猎杀赛
	//	public int ISbossmatchdouble;

	public Image[] IsShowType;
	//	public Image[] IsshowType2;
	public Image[] isShowtype3;
	NobelInfo horodatainfo;
	private int Levelnum;
	private bool isShowReward = false;
	public GameObject RewardConter;
	public Image ShowTypeConter;
	Transform dragonControl;

	void Awake ()
	{
		horodatainfo = (NobelInfo)Facade.GetFacade ().data.Get (FacadeConfig.UI_NOBEL_ID);
	}
	// Use this for initialization
	void Start ()
	{
		dragonControl = transform.Find ("RewardIn");
		if (Facade.GetFacade ().config.isIphoneX2 ()) {
			dragonControl.localScale = new Vector3 (.9f, .9f, .9f);
		}
		this.gameObject.transform.Find ("RewardIn/Panel/RewardButton").GetComponent<Button> ().onClick.AddListener (Onclose);


		Debug.LogError ("horodatainfo" + horodatainfo.IsMonthType);
		if (horodatainfo.IsMonthType != 0) {
			IsShowType [0].gameObject.SetActive (false);
//			IsshowType2 [0].gameObject.SetActive (true);
			isShowtype3 [0].gameObject.SetActive (true);

		}
		if (horodatainfo.IsToUp != 0) {
			IsShowType [1].gameObject.SetActive (false);
//			IsshowType2 [1].gameObject.SetActive (true);
			isShowtype3 [1].gameObject.SetActive (true);
		}
		if (horodatainfo.ISbossmatchdouble != 0) {
			IsShowType [2].gameObject.SetActive (false);
//			IsshowType2 [2].gameObject.SetActive (true);
			isShowtype3 [2].gameObject.SetActive (true);
		}

//		Debug.LogError ("reward" + reward.rewardPro.Count);
		if (isShowReward) {
			ShowRewad ();
			ShowIsnullimage [0].gameObject.SetActive (true);
			ShowIsnullimage [1].gameObject.SetActive (false);
		} else {
//			ShowTypeConter.rectTransform.localPosition = new Vector3 (0, -100f, 0);
			ShowIsnullimage [0].gameObject.SetActive (false);
			ShowIsnullimage [1].gameObject.SetActive (true);
			ShowISnull.gameObject.SetActive (true);
		}

		
	}

	public void SetDuanWeiShow (string level)
	{
		Level.text = level;
		Levelnum = int.Parse (level);
		Levelnum += 7000;
		Debug.LogError ("levelnum" + Levelnum);
		rewardinfo = Facade.GetFacade ().message.reward.GetHoroduanweiArray ();
		Debug.LogError ("rewardinfo" + rewardinfo.Count);
		for (int i = 0; i < rewardinfo.Count; i++) {
			if (rewardinfo [i].RewardType == Levelnum) {
				reward = rewardinfo [i];
				isShowReward = true;
				Debug.LogError ("ishowreward");
			}
		}
	}

	private void ShowRewad ()
	{
		if (reward == null) {
			return;
		}
		GameObject nWindow = Resources.Load ("Window/RewardUnitHoroSpecical") as GameObject;
		for (int i = 0; i < reward.rewardPro.Count; i++) {
			GameObject nUnit = Instantiate (nWindow) as GameObject;
			nUnit.name = "entity" + i;
			nUnit.transform.SetParent (RewardConter.transform);
			nUnit.transform.localScale = new Vector3 (1.35f, 1.35f, 1.35f);

			GameObject Day = RewardConter.transform.GetChild (i).Find ("Day").gameObject;
			Transform icon = RewardConter.transform.GetChild (i).Find ("Icon");
			if (FiPropertyType.TIMELIMTPROTYPE_1 < reward.rewardPro [i].type && reward.rewardPro [i].type < FiPropertyType.TIMELIMTPROTYPE_3) {
				Sprite[] itemicon = new Sprite[2];
				itemicon = FiPropertyType.GetTimeSpriteShow (reward.rewardPro [i].type);
				icon.gameObject.GetComponent<Image> ().sprite = itemicon [0];
				Day.gameObject.SetActive (true);
				Day.gameObject.GetComponent<Image> ().sprite = itemicon [1];
			} else {
				icon.gameObject.GetComponent<Image> ().sprite = FiPropertyType.GetSprite (reward.rewardPro [i].type);
			}
			int nCountValue = reward.rewardPro [i].value;
			string nResult = nCountValue.ToString ();
			//			if (nCountValue > 10000) {
			//				nResult = nCountValue / 10000 + "万";
			//			}
			RewardConter.transform.GetChild (i).Find ("NumText").gameObject.GetComponent<Text> ().text = nResult;
		}
	}

	public void SetRewaArray (List<FiProperty> array)
	{
		mRewardArray = array;
	}

	private void Onclose ()
	{
		Destroy (this.gameObject);
	}
	// Update is called once per frame
	void Update ()
	{
		
	}
}
