using UnityEngine;
using System.Collections;
using UnityEngine.UI;

/// <summary>
/// 挂在至LuckDrawCanvas下
/// </summary>
public class ToggleChooseScr : MonoBehaviour
{

	#region 字段

	//普通抽奖
	Toggle commonToggle;
	//青铜抽奖
	Toggle bronzeToggle;
	//白银抽奖
	Toggle silverToggle;
	//黄金抽奖
	Toggle goldenToggle;
	//白金抽奖
	Toggle platinumToggle;
	//至尊抽奖
	Toggle extremeToggle;



	public Text commonText;
	public	Text bronzeText;
	public Text silverText;
	public Text goldenText;
	public Text platinumText;
	public Text extremeText;

	public GameObject accumulateText;


	public Text[] commonBottomText;
	public Text commonTopText;
	public Text extremeLotteryText;
	public  Image torpedoImage;
	public GameObject searchBonusFish;

	public GameObject extremeLotterySliders;
	public GameObject bonusFishSlider;
	public GameObject leaveForLottery;

	Color32 c;
	Color32 outLineColor;
	Color32 currentLineColor;


	Outline[] commonOutline;
	Outline[] bronzeOutline;
	Outline[] silverOutline;
	Outline[] goldenOutline;
	Outline[] platinumOutline;
	Outline[] extremeOutline;


	Animator lotterAnim;

	long commonGold = 0;
	long bronzeGold = 50000;
	long silverGold = 250000;
	long goldenGold = 500000;
	long platinumGold = 1000000;
	long extremeGold = 2000000;

	#endregion

	void Start ()
	{
		UpdateToggle ();
		LuckDrawCanvasScr.Instance.UpdateData ();

	}

	#region 初始化

	void Init ()
	{
		//查找Toggle
		commonToggle = transform.Find ("LuckDrawControl/LotteryPanel/ToggleGroup/CommonToggle").GetComponent<Toggle> ();
		bronzeToggle = transform.Find ("LuckDrawControl/LotteryPanel/ToggleGroup/BronzeToggle").GetComponent<Toggle> ();
		silverToggle = transform.Find ("LuckDrawControl/LotteryPanel/ToggleGroup/SilverToggle").GetComponent<Toggle> ();
		goldenToggle = transform.Find ("LuckDrawControl/LotteryPanel/ToggleGroup/GoldenToggle").GetComponent<Toggle> ();
		platinumToggle = transform.Find ("LuckDrawControl/LotteryPanel/ToggleGroup/PlatinumToggle").GetComponent<Toggle> ();
		extremeToggle = transform.Find ("LuckDrawControl/LotteryPanel/ToggleGroup/ExtremeToggle").GetComponent<Toggle> ();


		//更改外边轮廓
		commonOutline = transform.Find ("LuckDrawControl/LotteryPanel/ToggleGroup/CommonToggle/CommonLabel").GetComponents <Outline> ();
		bronzeOutline = transform.Find ("LuckDrawControl/LotteryPanel/ToggleGroup/BronzeToggle/BronzeLabel").GetComponents <Outline> ();
		silverOutline = transform.Find ("LuckDrawControl/LotteryPanel/ToggleGroup/SilverToggle/SilverLabel").GetComponents <Outline> ();
		goldenOutline = transform.Find ("LuckDrawControl/LotteryPanel/ToggleGroup/GoldenToggle/GoldenLabel").GetComponents <Outline> ();
		platinumOutline = transform.Find ("LuckDrawControl/LotteryPanel/ToggleGroup/PlatinumToggle/PlatinumLabel").GetComponents <Outline> ();
		extremeOutline = transform.Find ("LuckDrawControl/LotteryPanel/ToggleGroup/ExtremeToggle/ExtremeLabel").GetComponents <Outline> ();

		//动画
		lotterAnim = transform.Find ("LuckDrawControl/LotteryPanel/PanelCounts/CommonPanel").GetComponent<Animator> ();

		//清空绑定
		commonToggle.onValueChanged.RemoveAllListeners ();
		bronzeToggle.onValueChanged.RemoveAllListeners ();
		silverToggle.onValueChanged.RemoveAllListeners ();
		goldenToggle.onValueChanged.RemoveAllListeners ();
		platinumToggle.onValueChanged.RemoveAllListeners ();
		extremeToggle.onValueChanged.RemoveAllListeners ();

		//绑定事件
		commonToggle.onValueChanged.AddListener (Tog_ChangeCommonValue);
		bronzeToggle.onValueChanged.AddListener (Tog_ChangeBronzeValue);
		silverToggle.onValueChanged.AddListener (Tog_ChangeSilverValue);
		goldenToggle.onValueChanged.AddListener (Tog_ChangeGoldenValue);
		platinumToggle.onValueChanged.AddListener (Tog_ChangePlatinumValue);
		extremeToggle.onValueChanged.AddListener (Tog_ChangeExtremeValue);

		JudgeGoldNum ();

	}

	#endregion




	#region 绑定事件的方法

	//显示普通的Panel
	void Tog_ChangeCommonValue (bool check)
	{
		if (check == true) {

			LuckDrawCanvasScr.Instance.SetToggleTrue (1);

			TextWithGroup (commonBottomText, ToggleChangeScr.Instance.commonNumDescripeGroup);

			TextSetting (1);

			ToggeSetMsg (0, ToggleChangeScr.Instance.extLotTextDescripeGroup [0], ToggleChangeScr.Instance.torpedoDescripeGroup [0],
				ToggleChangeScr.Instance.torpedoGroup [0], "/0", commonGold);
			if (LuckDrawCanvasScr.Instance.nInfo.loginInfo.luckyFishNum < 5 && LuckDrawCanvasScr.Instance.nInfo.loginInfo.luckyGold >= commonGold) {
				LuckDrawCanvasScr.Instance.immediatelyButton.SetActive (false);
				LuckDrawCanvasScr.Instance.sliderCounts.SetActive (true);
				LuckDrawCanvasScr.Instance.moreLotteryText.SetActive (false);
				leaveForLottery.SetActive (false);
				accumulateText.SetActive (false);
				searchBonusFish.SetActive (true);
			}

			if (LuckDrawCanvasScr.Instance.nInfo.loginInfo.luckyGold >= commonGold &&
			    LuckDrawCanvasScr.Instance.nInfo.loginInfo.luckyGold < bronzeGold &&
			    LuckDrawCanvasScr.Instance.nInfo.loginInfo.luckyFishNum >= 5) {
				LuckDrawCanvasScr.Instance.immediatelyButton.SetActive (true);
				LuckDrawCanvasScr.Instance.sliderCounts.SetActive (true);
				LuckDrawCanvasScr.Instance.moreLotteryText.SetActive (false);
				leaveForLottery.SetActive (false);
				searchBonusFish.SetActive (false);
				accumulateText.SetActive (true);
			}

			if (LuckDrawCanvasScr.Instance.nInfo.loginInfo.luckyGold >= bronzeGold && LuckDrawCanvasScr.Instance.nInfo.loginInfo.luckyFishNum >= 5) {
				LuckDrawCanvasScr.Instance.immediatelyButton.SetActive (false);
				LuckDrawCanvasScr.Instance.sliderCounts.SetActive (false);
				LuckDrawCanvasScr.Instance.moreLotteryText.SetActive (true);
				leaveForLottery.SetActive (true);
				accumulateText.SetActive (false);
				searchBonusFish.SetActive (false);
			}
		}
	}
	//青铜
	void Tog_ChangeBronzeValue (bool check = true)
	{

		if (check == true) {
			
			LuckDrawCanvasScr.Instance.SetToggleTrue (2);

			TextWithGroup (commonBottomText, ToggleChangeScr.Instance.bronzeNumDescripeGroup);

			TextSetting (2);

			ToggeSetMsg (1, ToggleChangeScr.Instance.extLotTextDescripeGroup [1], ToggleChangeScr.Instance.torpedoDescripeGroup [1],
				ToggleChangeScr.Instance.torpedoGroup [1], "/50000", bronzeGold);
			
			GoldAndFishJudge (bronzeGold, silverGold);

		}
	}
	//白银
	void Tog_ChangeSilverValue (bool check = true)
	{
		if (check == true) {
			LuckDrawCanvasScr.Instance.SetToggleTrue (3);

			TextWithGroup (commonBottomText, ToggleChangeScr.Instance.silverNumDescripeGroup);

			TextSetting (3);

			ToggeSetMsg (2, ToggleChangeScr.Instance.extLotTextDescripeGroup [2], ToggleChangeScr.Instance.torpedoDescripeGroup [2],
				ToggleChangeScr.Instance.torpedoGroup [2], "/250000", silverGold);
			
			GoldAndFishJudge (silverGold, goldenGold);
				
		}
	}
	//黄金
	void Tog_ChangeGoldenValue (bool check = true)
	{

		if (check == true) {

			LuckDrawCanvasScr.Instance.SetToggleTrue (4);

			TextWithGroup (commonBottomText, ToggleChangeScr.Instance.goldenNumDescripeGroup);

			TextSetting (4);

			ToggeSetMsg (3, ToggleChangeScr.Instance.extLotTextDescripeGroup [3], ToggleChangeScr.Instance.torpedoDescripeGroup [3],
				ToggleChangeScr.Instance.torpedoGroup [3], "/500000", goldenGold);

			GoldAndFishJudge (goldenGold, platinumGold);

		}
	}
	//白金
	void Tog_ChangePlatinumValue (bool check = true)
	{

		if (check == true) {

			LuckDrawCanvasScr.Instance.SetToggleTrue (5);

			TextWithGroup (commonBottomText, ToggleChangeScr.Instance.platinumNumDescripeGroup);
			
			TextSetting (5);
				
			ToggeSetMsg (4, ToggleChangeScr.Instance.extLotTextDescripeGroup [4], ToggleChangeScr.Instance.torpedoDescripeGroup [4],
				ToggleChangeScr.Instance.torpedoGroup [4], "/1000000", platinumGold);

			GoldAndFishJudge (platinumGold, extremeGold);

		}
	}
	//至尊
	void Tog_ChangeExtremeValue (bool check = true)
	{

		if (check == true) {

			LuckDrawCanvasScr.Instance.SetToggleTrue (6);

			TextWithGroup (commonBottomText, ToggleChangeScr.Instance.extremeNumDescripeGroup);

			TextSetting (6);

			ToggeSetMsg (5, ToggleChangeScr.Instance.extLotTextDescripeGroup [5], ToggleChangeScr.Instance.torpedoDescripeGroup [5],
				ToggleChangeScr.Instance.torpedoGroup [5], "/2000000", extremeGold);

			GoldAndFishJudge (extremeGold, 73217831283291);
		}

	}

	void ToggeSetMsg (int id, string group, string torDesGroup, Sprite torGroup, string goldmsg, long num)
	{
		lotterAnim.SetTrigger ("LuckDrawCanvas_Fanpanxiaoguo"); 

		LuckDrawCanvasScr.Instance.immediatelyButton.SetActive (false);

		ToggleChangeScr.Instance.toggleId = id;
		LuckDrawCanvasScr.Instance.extremeLotteryNumText.text = LuckDrawCanvasScr.Instance.nInfo.loginInfo.luckyGold.ToString () + goldmsg;
		LuckDrawCanvasScr.Instance.extremeLotterySlider.value = (float)LuckDrawCanvasScr.Instance.nInfo.loginInfo.luckyGold / num;

		extremeLotteryText.text = group;
		commonTopText.text = torDesGroup;
		torpedoImage.sprite = torGroup;

	}

	//根据金币和奖金鱼切换toggle
	void GoldAndFishJudge (long curgold, long nextgold)
	{
		
		if (LuckDrawCanvasScr.Instance.nInfo.loginInfo.luckyGold < curgold ||
		    LuckDrawCanvasScr.Instance.nInfo.loginInfo.luckyFishNum < 5) {
			LuckDrawCanvasScr.Instance.immediatelyButton.SetActive (false);
			LuckDrawCanvasScr.Instance.moreLotteryText.SetActive (false);	
			leaveForLottery.SetActive (false);
			accumulateText.SetActive (false);
			LuckDrawCanvasScr.Instance.sliderCounts.SetActive (true);
			searchBonusFish.SetActive (true);

		}

		if (LuckDrawCanvasScr.Instance.nInfo.loginInfo.luckyGold >= curgold && LuckDrawCanvasScr.Instance.nInfo.loginInfo.luckyGold < nextgold && LuckDrawCanvasScr.Instance.nInfo.loginInfo.luckyFishNum >= 5) {
			LuckDrawCanvasScr.Instance.immediatelyButton.SetActive (true);
			LuckDrawCanvasScr.Instance.sliderCounts.SetActive (true);
			LuckDrawCanvasScr.Instance.moreLotteryText.SetActive (false);
			leaveForLottery.SetActive (false);
			accumulateText.SetActive (true);
			searchBonusFish.SetActive (false);
		}
		if (LuckDrawCanvasScr.Instance.nInfo.loginInfo.luckyGold >= nextgold && LuckDrawCanvasScr.Instance.nInfo.loginInfo.luckyFishNum >= 5) {
			LuckDrawCanvasScr.Instance.immediatelyButton.SetActive (false);
			LuckDrawCanvasScr.Instance.sliderCounts.SetActive (false);
			LuckDrawCanvasScr.Instance.moreLotteryText.SetActive (true);
			leaveForLottery.SetActive (true);
			accumulateText.SetActive (false);
			searchBonusFish.SetActive (false);
		}

	}

	void TextWithGroup (Text[] tex, string[] group)
	{
		for (int i = 0; i < 6; i++) {
			tex [i].text = group [i];
		}
	}

	#endregion

	#region 更改字体颜色和外边框


	void TextSetting (int index)
	{
		for (int i = 0; i < 2; i++) {
			commonOutline [i].effectColor = outLineColor;
			bronzeOutline [i].effectColor = outLineColor;
			silverOutline [i].effectColor = outLineColor;
			goldenOutline [i].effectColor = outLineColor;
			platinumOutline [i].effectColor = outLineColor;
			extremeOutline [i].effectColor = outLineColor;
		}

		switch (index) {
		case 1:
			commonText.color = c;
			bronzeText.color = Color.white;
			silverText.color = Color.white;
			goldenText.color = Color.white;
			platinumText.color = Color.white;
			extremeText.color = Color.white;
			commonOutline [0].effectColor = currentLineColor;
			commonOutline [1].effectColor = currentLineColor;
			break;
		case 2:
			commonText.color = Color.white;
			bronzeText.color = c;
			silverText.color = Color.white;
			goldenText.color = Color.white;
			platinumText.color = Color.white;
			extremeText.color = Color.white;
			
			bronzeOutline [0].effectColor = currentLineColor;
			bronzeOutline [1].effectColor = currentLineColor;

			break;
		case 3:
			commonText.color = Color.white;
			bronzeText.color = Color.white;
			silverText.color = c;
			goldenText.color = Color.white;
			platinumText.color = Color.white;
			extremeText.color = Color.white;

			silverOutline [0].effectColor = currentLineColor;
			silverOutline [1].effectColor = currentLineColor;


			break;
		case 4:
			commonText.color = Color.white;
			bronzeText.color = Color.white;
			silverText.color = Color.white;
			goldenText.color = c;
			platinumText.color = Color.white;
			extremeText.color = Color.white;
			goldenOutline [0].effectColor = currentLineColor;
			goldenOutline [1].effectColor = currentLineColor;

			break;
		case 5:
			commonText.color = Color.white;
			bronzeText.color = Color.white;
			silverText.color = Color.white;
			goldenText.color = Color.white;
			platinumText.color = c;
			extremeText.color = Color.white;
			platinumOutline [0].effectColor = currentLineColor;
			platinumOutline [1].effectColor = currentLineColor;
			break;
		case 6:
			commonText.color = Color.white;
			bronzeText.color = Color.white;
			silverText.color = Color.white;
			goldenText.color = Color.white;
			platinumText.color = Color.white;
			extremeText.color = c;
			extremeOutline [0].effectColor = currentLineColor;
			extremeOutline [1].effectColor = currentLineColor;
			break;
		default:
			break;
		}
		
	}




	#endregion

	#region 更新和切换

	//前往抽奖
	public void LeaveForLotteryBtn ()
	{
		if (LuckDrawCanvasScr.Instance.nInfo.loginInfo.luckyGold >= bronzeGold) {
			LuckDrawCanvasScr.Instance.bronzeToggle.isOn = true;
		}
		if (LuckDrawCanvasScr.Instance.nInfo.loginInfo.luckyGold >= silverGold) {
			LuckDrawCanvasScr.Instance.silverToggle.isOn = true;
		}
		if (LuckDrawCanvasScr.Instance.nInfo.loginInfo.luckyGold >= goldenGold) {
			LuckDrawCanvasScr.Instance.goldenToggle.isOn = true;
		}
		if (LuckDrawCanvasScr.Instance.nInfo.loginInfo.luckyGold >= platinumGold) {
			LuckDrawCanvasScr.Instance.platinumToggle.isOn = true;
		}
		if (LuckDrawCanvasScr.Instance.nInfo.loginInfo.luckyGold >= extremeGold) {
			LuckDrawCanvasScr.Instance.extremeToggle.isOn = true;
		}
	}

	//判断金币切换toggle
	void JudgeGoldNum ()
	{

		if (LuckDrawCanvasScr.Instance.nInfo.loginInfo.luckyGold >= commonGold && LuckDrawCanvasScr.Instance.nInfo.loginInfo.luckyGold < bronzeGold) {
			LuckDrawCanvasScr.Instance.extremeLotterySlider.value = (float)LuckDrawCanvasScr.Instance.nInfo.loginInfo.luckyGold / 0;
			TextSetting (1);
			LuckDrawCanvasScr.Instance.commonToggle.isOn = true;
		}

		if (LuckDrawCanvasScr.Instance.nInfo.loginInfo.luckyGold >= bronzeGold && LuckDrawCanvasScr.Instance.nInfo.loginInfo.luckyGold < silverGold) {

			Tog_ChangeBronzeValue (LuckDrawCanvasScr.Instance.commonToggle.isOn);
			TextSetting (2);
			LuckDrawCanvasScr.Instance.extremeLotterySlider.value = (float)LuckDrawCanvasScr.Instance.nInfo.loginInfo.luckyGold / bronzeGold;
			LuckDrawCanvasScr.Instance.bronzeToggle.isOn = true;
		}


		if (LuckDrawCanvasScr.Instance.nInfo.loginInfo.luckyGold >= silverGold && LuckDrawCanvasScr.Instance.nInfo.loginInfo.luckyGold < goldenGold) {

			Tog_ChangeBronzeValue (LuckDrawCanvasScr.Instance.bronzeToggle.isOn);
			TextSetting (3);
			LuckDrawCanvasScr.Instance.silverToggle.isOn = true;

		}
		if (LuckDrawCanvasScr.Instance.nInfo.loginInfo.luckyGold >= goldenGold && LuckDrawCanvasScr.Instance.nInfo.loginInfo.luckyGold < platinumGold) {
			Tog_ChangeSilverValue (LuckDrawCanvasScr.Instance.silverToggle.isOn);
			TextSetting (4);
			LuckDrawCanvasScr.Instance.goldenToggle.isOn = true;


		}
		if (LuckDrawCanvasScr.Instance.nInfo.loginInfo.luckyGold >= platinumGold && LuckDrawCanvasScr.Instance.nInfo.loginInfo.luckyGold < extremeGold) {

			Tog_ChangeGoldenValue (LuckDrawCanvasScr.Instance.goldenToggle.isOn);
			TextSetting (5);
			LuckDrawCanvasScr.Instance.platinumToggle.isOn = true;

		}
		if (LuckDrawCanvasScr.Instance.nInfo.loginInfo.luckyGold >= extremeGold) {
			Tog_ChangePlatinumValue (LuckDrawCanvasScr.Instance.platinumToggle.isOn);
			TextSetting (6);
			LuckDrawCanvasScr.Instance.extremeToggle.isOn = true;
		}

	}

	//更新toggle
	public	void UpdateToggle ()
	{
		Init ();
	
		c = new Color32 (253, 248, 220, 255);

		outLineColor = new Color32 (73, 67, 189, 255);

		currentLineColor = new Color32 (121, 76, 40, 127);

		if (LuckDrawCanvasScr.Instance.nInfo.loginInfo.luckyFishNum < 5) {
			extremeLotterySliders.SetActive (false);
			bonusFishSlider.SetActive (true);
			LuckDrawCanvasScr.Instance.immediatelyButton.SetActive (false);
			LuckDrawCanvasScr.Instance.searchBonusFishButton.SetActive (true);
		}
		if (LuckDrawCanvasScr.Instance.nInfo.loginInfo.luckyFishNum >= 5) {
			accumulateText.SetActive (true);
			extremeLotterySliders.SetActive (true);
			bonusFishSlider.SetActive (false);
			LuckDrawCanvasScr.Instance.searchBonusFishButton.SetActive (false);
		}

	}


	#endregion
}
