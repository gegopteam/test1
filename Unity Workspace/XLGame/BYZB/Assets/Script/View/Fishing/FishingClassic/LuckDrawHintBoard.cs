using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using AssemblyCSharp;
using DG.Tweening;

public class LuckDrawHintBoard : MonoBehaviour
{

	public static LuckDrawHintBoard _instace = null;

	MyInfo myInfo;
	bool isShow = false;

	public GameObject effect_UI_Lottery;

	public Text upAreaText;

	public Text bonusGoldText;
	public Text bonusFishText;

	public GameObject bonusFishProgessBar;
	public Image progessbarImage;
	Button bgBtn;

	long bonusGold;
	int bonusFishNum;

	public float orignalScale = 1f;


	private void Awake ()
	{
		if (null != _instace) {
			Destroy (LuckDrawHintBoard._instace.gameObject); 
		}
		_instace = this;
	}

	// Use this for initialization
	void Start ()
	{
		transform.GetComponent<RectTransform> ().localScale = new Vector3 (0, orignalScale, orignalScale);
		bgBtn = transform.Find ("Bg").GetComponent<Button> ();
		//transform.FindChild("Bg").GetComponent<Button>().enabled = false;
		myInfo = DataControl.GetInstance ().GetMyInfo ();
		UpdateData ();
	}

	private void Update ()
	{
        return;
		if (Input.GetKeyDown (KeyCode.Y)) {
			myInfo.loginInfo.luckyGold += 9999;
			myInfo.loginInfo.luckyFishNum++;
			UpdateData ();
			ShowPop ();
		}
	}

	public void UpdateData ()
	{
		bonusGold = myInfo.loginInfo.luckyGold;
		bonusFishNum = myInfo.loginInfo.luckyFishNum;

		//bonusGoldText.text = bonusGold.ToString ();
		bonusGoldText.DOText (bonusGold.ToString (), 2f, true, ScrambleMode.Numerals);


		bonusFishText.text = bonusFishNum.ToString () + "/5";

		if (bonusFishNum >= 5) {
			bonusFishProgessBar.SetActive (false);
			bgBtn.enabled = true;
			upAreaText.text = "点击免费抽奖";

		} else {
			upAreaText.text = "奖金鱼";
			bonusFishProgessBar.SetActive (true);
			progessbarImage.fillAmount = (float)bonusFishNum / 5;
			//bgBtn.enabled = false;
			bgBtn.enabled = true;
		}
	}


	public void PopShow (bool toShow)
	{
		//if (!LeftOption._instance.isShow) {
			//return;
		//}
		if (toShow) {
			//Debug.LogError("SetShow");
			//UpdateData();
			transform.DOScaleX (orignalScale, 0.3f);
		} else {
			transform.DOScaleX (0, 0.3f);
		}
		isShow = toShow;
	}

	public void ShowPop ()
	{
		StartCoroutine (AutoHideAndShow ());
	
	}

	IEnumerator AutoHideAndShow ()
	{
		if (!isShow) {
			if (myInfo.loginInfo.luckyFishNum < 5) {
				effect_UI_Lottery.SetActive (false);
				PopShow (true);
				bonusGoldText.DOText (bonusGold.ToString (), 2f, true, ScrambleMode.Numerals);
				yield return new WaitForSeconds (5f);
				PopShow (false);
			} else if (myInfo.loginInfo.luckyFishNum == 5) {
				effect_UI_Lottery.SetActive (true);
				PopShow (true);
				yield return new WaitForSeconds (5f);
				PopShow (false);
			} else if (myInfo.loginInfo.luckyFishNum > 5) {
				PopShow (false);
				effect_UI_Lottery.SetActive (true);
			}
		}
		if (isShow) {
			if (myInfo.loginInfo.luckyFishNum < 5) {
				effect_UI_Lottery.SetActive (false);
				PopShow (true);
				bonusGoldText.DOText (bonusGold.ToString (), 2f, true, ScrambleMode.Numerals);
				yield return new WaitForSeconds (5f);
				PopShow (false);
			}
			if (myInfo.loginInfo.luckyFishNum == 5) {
				effect_UI_Lottery.SetActive (true);
				PopShow (true);
				yield return new WaitForSeconds (5f);
				PopShow (false);
			}
		}
	}

	IEnumerator HidePopShow ()
	{
		yield return new WaitForSeconds (5f);
		if (isShow == true) {
			PopShow (false);
		}
	}

	public void ToggleShow ()
	{
		PopShow (!isShow);
		StartCoroutine (HidePopShow ());
	}

	public GameObject luckDrawCanvasPrefab;

	public void ShowLuckDrawCanvas ()
	{
		GameObject.Instantiate (luckDrawCanvasPrefab);
		PopShow (false);
	}

	private void OnDestroy ()
	{
		_instace = null;
	}
}
