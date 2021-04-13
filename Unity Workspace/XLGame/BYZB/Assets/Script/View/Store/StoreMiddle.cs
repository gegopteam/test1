using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using AssemblyCSharp;

public class StoreMiddle : MonoBehaviour
{

	public Text nowLevel;
	public Text nextLevel;
	public Slider vipSlider;
	//public Text nowExp;
	//public Text maxExp;//在这里注释了新版商店没有的元素
	public Text needExp;
	//	public Text isFull;
	private byte level;
	private float exp;
	private float maxValue;
	public static StoreMiddle instans;
	public Image tips;
	public Image tipsfull;

	void Awake ()
	{
		instans = this;
	}
	// Use this for initialization
	void Start ()
	{
		Refresh ();

	}

	public  void Refresh ()
	{
		MyInfo nInfo = (MyInfo)Facade.GetFacade ().data.Get (FacadeConfig.USERINFO_MODULE_ID);
		level = (byte)nInfo.levelVip;
		exp = nInfo.topupSum;
		if ((int)level != 9) {
			maxValue = VipSliderContrl.GetMaxValue (level);
		}


		nowLevel.text = level.ToString ();
		nextLevel.text = (level + 1).ToString ();
		//nowExp.text = exp.ToString ();
		if (maxValue != -1 && maxValue != 0) {
			//maxExp.text = maxValue.ToString ();
			vipSlider.value = (exp / maxValue);
			needExp.text = (maxValue - exp).ToString () + "y";
		} else {
			//maxExp.gameObject.SetActive (false);
			//nowExp.gameObject.SetActive (false);
			vipSlider.value = 1;
//			needExp.text = "0" + "元";
			nextLevel.text = "9";
			needExp.gameObject.SetActive (false);
			tips.gameObject.SetActive (false);
			tipsfull.gameObject.SetActive (true);
			//isFull.text = "已达上限";
		}
	}

}
