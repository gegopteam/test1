using UnityEngine;
using System.Collections;
using DG.Tweening;

public class DiamondDropEffect : MonoBehaviour {

	public GameObject singleDiamondPrefab;
	public TextMesh textNum;
	//public GameObject bgBar;
	public SpriteRenderer bgBar;
	Vector3 textOrignalScale;

	//public GameObject effectNumPrefab;

	//public float destroyTime=0.5f;

	GunInUI uiGun;
	int diamondReturn = -1;
	int dropNum=3;
	int thisSeatID;

	public float xMinRandom=0.5f;
	public float xMaxRandom=2f;

	public float yMinRandom=0.3f;
	public float yMaxRandom=1.0f;

	void Start(){
		//textNum.transform.localScale = Vector3.zero;
	}


	public void StartMoveToPlayer(GunControl targetGun,int diamondNum){
		
	//	textOrignalScale = textNum.transform.localScale;
	//	textNum.transform.localScale = Vector3.zero;
		textNum .gameObject.SetActive(false);
		bgBar.enabled = false;

		uiGun = targetGun.gunUI;
		thisSeatID = (int)uiGun.thisSeat;
		diamondReturn = diamondNum;

		if (diamondNum <= 3) {
			dropNum = 1;
		} else if ((diamondNum >= 4) && (diamondNum <= 7)) {
			dropNum = 2;
		} else if ((diamondNum >= 8) && (diamondNum <= 10)) {
			dropNum = 3;
		} else if (diamondNum >= 11) {
			dropNum = 4;
		}

		 //CreateDiamonds ();
		Invoke("CreateDiamonds",0.5f);
	}

	void CreateDiamonds(){

		Invoke ("PopShow",0.3f);
		Invoke ("HideTextNum", 1.8f);

		for (int i = 0; i < dropNum; i++) {
			float randTime = Random.Range (0, 0.3f);
			Invoke ("CreateOneDiamond", randTime);
		}
		Invoke ("ChangeUserValue", 2.45f);
	}

	void PopShow(){
		bgBar.enabled = true;

		textNum.gameObject.SetActive (true);
		textNum.text = "钻石x" + diamondReturn;
		transform.localScale = Vector3.zero;
		transform.DOScale (1f, 0.5f);
	}

	void CreateOneDiamond(){
		
		Vector3 randomVector = new Vector3 (Random.Range(xMinRandom, xMaxRandom), Random.Range(yMinRandom, yMaxRandom), 51f);
		GameObject diamondGo;
		diamondGo= GameObject.Instantiate (singleDiamondPrefab, transform.position + randomVector, Quaternion.identity, this.transform)as GameObject ;
		diamondGo.GetComponent<SingleDiamondDrop> ().MoveToPos (uiGun, 1.3f, 0.6f);

	}

	void HideTextNum(){
		bgBar.enabled = false;
		textNum.gameObject.SetActive (false);
	}

	void ChangeUserValue()
	{
		uiGun.AddValue (0, 0, diamondReturn);
		//AudioManager._instance.PlayEffectClip (AudioManager.effect_getCoin);
		Destroy (this.gameObject);
	}
}
