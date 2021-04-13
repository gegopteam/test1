using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using AssemblyCSharp;

public class SignCofing{
	public int SignDay;//
	public List<FiProperty>  Rewardtype;//
	public int Rewardtcount;//
}
public class DaySignInfo : MonoBehaviour {

	public Image[] DoubleReward;//
	public Image SingleReward;//
	public Image Sign;//
	public Image NoSign;//
	public Text SingleRewardValue;//
	public Text[] DoubleRewardValue;//
	public Image Mask;//
	public int statue;
	public int SignCurrentDay;
	public Image onclick;
	public Button signbutton;

	DailySignInfo signinfo;
	private Sprite[] timeicon;
//	private int currentday;



	void Awake()
	{
		
	}
	// Use this for initialization
	void Start () {
		
	}

	
	// Update is called once per frame
	void Update () {
		
	}





	public void SignShowInit(SignCofing sign)
	{
			if (sign.Rewardtcount != 1) {
				SingleReward.gameObject.SetActive (false);
				for (int j = 0; j < DoubleReward.Length; j++) {
				if (9010 < sign.Rewardtype [j].type) {
					timeicon = FiPropertyType.GetTimeSpriteShow (sign.Rewardtype [j].type);
					Debug.LogError ("-------------" + DoubleReward [j].gameObject.name + "------------------");
					DoubleReward [j].gameObject.transform.Find ("day").gameObject.SetActive (true);
					DoubleReward [j].gameObject.transform.Find ("day").GetComponent<Image> ().sprite = timeicon [1];
					DoubleReward [j].sprite = timeicon [0];
					DoubleRewardValue[j].text = "X"+sign.Rewardtype[j].value.ToString ();
					DoubleReward [j].gameObject.SetActive (true);
				} else {
					DoubleReward [j].sprite = FiPropertyType.GetSignSprite (sign.Rewardtype [j].type);
//					DoubleReward [j].SetNativeSize ();
					DoubleRewardValue[j].text = "X"+sign.Rewardtype[j].value.ToString ();
					DoubleReward [j].gameObject.SetActive (true);
				}

				}
			} else {
				for (int j = 0; j < DoubleReward.Length; j++) {
					DoubleReward [j].gameObject.SetActive (false);
				}
			if (9010 < sign.Rewardtype [0].type) {
				timeicon = FiPropertyType.GetTimeSpriteShow (sign.Rewardtype [0].type);
				SingleReward.gameObject.transform.Find ("day").gameObject.SetActive (true);
				SingleReward.gameObject.transform.Find ("day").GetComponent<Image> ().sprite = timeicon [1];
				SingleReward.sprite = timeicon [0];
				SingleRewardValue.text = "X"+sign.Rewardtype [0].value.ToString ();
				SingleReward.gameObject.SetActive (true);
			} else {
				SingleReward.sprite=FiPropertyType.GetSignSprite (sign.Rewardtype[0].type);
				SingleRewardValue.text = "X"+sign.Rewardtype [0].value.ToString();
				SingleReward.gameObject.SetActive (true);
			}

			}	
				SignCurrentDay = sign.SignDay;
	
	}

//	public  SignCofing getTaskInfo(int SignDay){
//		if (SignDay < 8 && SignDay > 0)
//			return sign [SignDay - 1];
//		else
//			return null;
//	}

}
