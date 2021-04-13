using UnityEngine;
using System.Collections;
using AssemblyCSharp;
using UnityEngine.UI;


//挂在至LuckDrawCanvas下
public class LuckDrawCanvasScr : MonoBehaviour
{

	#region 字段


	public static LuckDrawCanvasScr Instance;

	public  GameObject beginLotteryCanvas;

	[HideInInspector]
	public MyInfo nInfo;

	public Slider bonusFishSlider;
	public Slider extremeLotterySlider;


	public Text bonusFishText;
	public Text extremeLotteryNumText;

	public GameObject moreLotteryText;
	public GameObject sliderCounts;

	public Text shadowText;
	public GameObject immediatelyButton;
	public GameObject searchBonusFishButton;

	//普通抽奖
	public Toggle commonToggle;
	//青铜抽奖
	public Toggle bronzeToggle;
	//白银抽奖
	public Toggle silverToggle;
	//黄金抽奖
	public	Toggle goldenToggle;
	//白金抽奖
	public Toggle platinumToggle;
	//至尊抽奖
	public Toggle extremeToggle;

	public bool isCommonToggle = false;
	public bool isBronzeToggle = false;
	public bool isSilverToggle = false;
	public bool isGoldenToggle = false;
	public bool isPlatinumToggle = false;
	public bool isExtremeToggle = false;

	#endregion

	void Awake ()
	{
		Instance = this;

		//获取信息
		nInfo = DataControl.GetInstance ().GetMyInfo ();

		//给slider赋值
		bonusFishSlider.value = nInfo.loginInfo.luckyFishNum;

        PrefabManager._instance.isLuckDrawUIShow = true;
	}

    private void OnDestroy()
    {
        PrefabManager._instance.isLuckDrawUIShow = false;
    }


    //关闭画布
    public void Btn_CloseCanvas ()
	{
		AudioManager._instance.PlayEffectClip (AudioManager.effect_closePanel);
		Destroy (this.gameObject);

	}
	//点击开始按钮,更新数据
	public void Btn_ImmediatelyLottery ()
	{
		
		
		if (nInfo.loginInfo.luckyFishNum >= 5) {
			if (LuckDrawHintBoard._instace != null) {
				LuckDrawHintBoard._instace.UpdateData ();
			}
		}
		AudioManager._instance.PlayEffectClip (AudioManager.effect_closePanel);

		Destroy (this.gameObject);
		GameObject.Instantiate (beginLotteryCanvas);

	}


	#region 金币和奖金鱼更新方法

	void UpdateLotteryNumText (long luckGold, string Gold)
	{
		extremeLotteryNumText.text = luckGold + Gold;
	}

	void UpdateLotterySlider (float luckGold, float Gold)
	{
		extremeLotterySlider.value = luckGold / Gold;
	}

	public void MinusLotteryFish ()
	{
		nInfo.loginInfo.luckyFishNum = 0;
	}

	public	void MinusLotteryGold ()
	{
		nInfo.loginInfo.luckyGold = 0;
		return;
	}

	#endregion

	#region 更新数据

	//更新数据
	public void UpdateData ()
	{
		transform.GetComponent <ToggleChooseScr> ().UpdateToggle ();

		if (nInfo.loginInfo.luckyGold != null) {
			shadowText.text = nInfo.loginInfo.luckyGold.ToString ();
		}

		if (nInfo.loginInfo.luckyFishNum != null) {
			bonusFishText.text = nInfo.loginInfo.luckyFishNum.ToString () + "/5";
			bonusFishSlider.value = (float)nInfo.loginInfo.luckyFishNum / 5;

		}
		if (isCommonToggle == true) {
			UpdateSliderAndNum ("/0", 0);		
		}
		if (isBronzeToggle == true) {
			UpdateSliderAndNum ("/50000", 50000);
		}
		if (isSilverToggle == true) {
			UpdateSliderAndNum ("/250000", 250000);
		}
		if (isGoldenToggle == true) {
			UpdateSliderAndNum ("/500000", 500000);
		}
		if (isPlatinumToggle == true) {
			UpdateSliderAndNum ("/1000000", 1000000f);
		}
		if (isExtremeToggle == true) {
			UpdateSliderAndNum ("/2000000", 2000000f);
		}
	}

	void UpdateSliderAndNum (string goldStr, float goldNum)
	{
		UpdateLotteryNumText (nInfo.loginInfo.luckyGold, goldStr);
		UpdateLotterySlider ((float)nInfo.loginInfo.luckyGold, goldNum);
	}


	public void SearchBonusFishNum ()
	{
		LeftOption._instance.ShowIllustratedPanel ();

	}
	//选择toggle
	public void SetToggleTrue (int index)
	{

		isCommonToggle = false;
		isBronzeToggle = false;
		isSilverToggle = false;
		isGoldenToggle = false;
		isPlatinumToggle = false;
		isExtremeToggle = false;

		switch (index) {
		case 1:
			isCommonToggle = true;
			break;
		case 2:
			isBronzeToggle = true;
			break;
		case 3:
			isSilverToggle = true;
			break;
		case 4:
			isGoldenToggle = true;
			break;
		case 5:
			isPlatinumToggle = true;
			break;
		case 6:
			isExtremeToggle = true;
			break;
		default:
			break;
		}

	}

	#endregion

}
