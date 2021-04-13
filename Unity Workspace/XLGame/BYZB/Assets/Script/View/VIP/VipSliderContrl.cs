using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System.Collections.Generic;
using AssemblyCSharp;

public class VipSliderContrl : MonoBehaviour {
	private Slider slider;
	public Text nowLevel;
	public Text nextLevel;
	public Text nowExp;
	public Text nextExp;
	public Image needExp;
	public Text isFull;


    public Text needExpText;
	//可能会从服务器端接过来的数据，包括现在的VIP等级和已经累计的vip经验
	private byte level;
	private float exp;

	private float maxValue;

	void Awake(){

		slider = gameObject.GetComponent<Slider> ();
		MyInfo nInfo =(MyInfo) Facade.GetFacade ().data.Get ( FacadeConfig.USERINFO_MODULE_ID );
		level = (byte)nInfo.levelVip;
		exp = nInfo.topupSum;
		maxValue = GetMaxValue (level);
	}
	// Use this for initialization
	void Start () {
		nowLevel.text = level.ToString();
		nextLevel.text = (level + 1).ToString();
		nowExp.text = exp.ToString();
		if (maxValue != -1 && maxValue != 0) {
			nextExp.text = maxValue.ToString ();
			slider.value = (exp / maxValue);
		} else {
			isFull.text = "已达上限";
			slider.value = 1;
			nextLevel.text = "9";
			nextExp.gameObject.SetActive (false);
			nowExp.gameObject.SetActive (false);
		}
        int total = (int)(maxValue - exp);
        Debug.Log(total + ":ShowNeedExp");
        needExpText.text = total.ToString()+"元";


		ShowNeedExp ();
	}

	void ShowNeedExp(){
		
			int total = (int)(maxValue - exp);
            Debug.Log(total + ":ShowNeedExp");
			Sprite[] sprites = UIHallTexturers.instans.VipNum;// Resources.LoadAll<Sprite> ("VIP/rechargeNumber");
			Stack<int> numbers = new Stack<int> ();
			int num1;
			Image tempImg;
		if (maxValue != -1&&maxValue!=0) {
			do {
				num1 = total % 10;
				numbers.Push (num1);
				total = total / 10;
			} while(total != 0);
			int tempCount = numbers.Count;
			for (int i = 1; i <= tempCount; i++) {
				tempImg = CloneGameObject (needExp.gameObject).GetComponent<Image> ();
				tempImg.sprite = sprites [numbers.Pop ()];
                Debug.Log(tempCount + i+"ShowNeedExp");
			}
		} else {
			tempImg = CloneGameObject (needExp.gameObject).GetComponent<Image> ();
			tempImg.sprite = sprites [0];
		}
			
	}

	public static int GetMaxValue(byte level){
		switch (level) {
		case 0:
			return 10;
			break;
		case 1:
			return 100;
			break;
		case 2:
			return 300;
			break;
		case 3:
			return 1000;
			break;
		case 4:
			return 2000;
			break;
		case 5:
			return 5000;
			break;
		case 6:
			return 10000;
			break;
		case 7:
			return 20000;
			break;
		case 8:
			return 50000;
			break;
		case 9:
                UIVIP.VIPLevel.gameObject.SetActive(false);
			return -1;
			break;
		default:
			Debug.LogError ("Data error:The Vip Level not exist");
			return 0;
			break;
		}
	}
	public static GameObject CloneGameObject(GameObject go){
		GameObject temp = Instantiate (go);
		temp.transform.SetParent(go.transform.parent,false);
		temp.SetActive (true);
		return temp;
	}

 
}
