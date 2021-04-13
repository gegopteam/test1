using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class PkPointProgessBar : MonoBehaviour {

	public static PkPointProgessBar _instance;

	public Transform indicator;

	public Transform[] checkpointGroup;
	float[] distanceGroup=new float[5];
	float sumDistance=0;
	public  float tempPercent=0f;

	int sumScoreValue;

	public int scoreValueLB=300;
	public Text scoreTextLB;
	public Text nickNameTextLB;
	public Image genderImageLB;
	public Image barLB;

	public int scoreValueRB=300;
	public Text scoreTextRB;
	public Text nickNameTextRB;
	public Image genderImageRB;
	public Image barRB;

	public Image avatorLB;
	public Image avatorRB;

	void Awake()
	{
        if (_instance != null)
            Destroy(PkPointProgessBar._instance.gameObject);
		_instance = this;
	}
	// Use this for initialization
	void Start () {
		if (GameController._instance.myGameType != GameType.Point) { //如果不是积分对抗赛，隐藏对抗进度条
			Destroy (this.gameObject);
			return;
		}
		scoreTextLB.text = scoreValueLB.ToString ();
		scoreTextRB.text = scoreValueRB.ToString ();
		sumScoreValue = scoreValueLB + scoreValueRB;

		for (int i = 0; i < 2; i++) {
			GunInUI gunUI = PrefabManager._instance.gunInUI [i].GetComponent<GunInUI> ();
			gunUI.SetScoreText (scoreValueLB);
		}

		for (int i = 0; i < checkpointGroup.Length-1; i++) {
			//distanceGroup [i] = Vector3.Distance (checkpointGroup [i].position, checkpointGroup [i + 1].position);
			distanceGroup [i] = GetHorizontalDistance (checkpointGroup [i].position, checkpointGroup [i + 1].position);
			sumDistance += distanceGroup [i];
		}
		SetIndicatorPos (0.5f);
	
	}


		

	public void SetIndicatorPos(float percent)
	{
		if (percent >= 1)
			percent = 0.9999f;
		else if (percent <= 0) {
			percent = 0;
		}
		float tempDistance = sumDistance * percent;

		float tempPercent = 0f;
		for (int i = 0; i < distanceGroup.Length; i++) {
			if (tempDistance > distanceGroup [i]) {
				tempDistance -= distanceGroup [i];
			} else {
				tempPercent = tempDistance  / distanceGroup [i];
				indicator.localPosition = (checkpointGroup [i + 1].localPosition - checkpointGroup [i].localPosition) * tempPercent
					+checkpointGroup[i].localPosition;
				break;
			}
		}
		barLB.fillAmount = percent;
		barRB.fillAmount = (1-percent);
	}

	public void SetShow(bool toShow)
	{
		
	}

	public void UpdateUserScore(GunSeat seat,int score)
	{
		if (score < 0)
			score = 0;
		if (score > (scoreValueLB + scoreValueRB)) {
			score = scoreValueLB + scoreValueRB;
		}
		switch (seat) {
		case GunSeat.LB:
			scoreValueLB = score;
			scoreTextLB.text = score.ToString ();
			break;
		case GunSeat.RB:
			scoreValueRB = score;
			scoreTextRB.text = score.ToString ();
			break;
		default:
			Debug.LogError ("Error!Can't find seat:" + seat);
			break;
		}
		UpdateIndicatorPos ();
	}

	public void InitData(GunSeat seat,string nickName,int gender,Sprite avator)
	{
        Debug.LogError("PkPointBarInit:" + seat+ "/"+nickName);
		switch (seat) {
		case GunSeat.LB:
			//scoreValueLB = score;
			//scoreTextLB.text = score.ToString ();
			
			nickNameTextLB.text = nickName.ToString ();
			SetLeftAvator(avator);
			if (gender !=1) genderImageLB.sprite = PrefabManager._instance.femaleSprite;
			break;
		case GunSeat.RB:
			//scoreValueRB = score;
			//scoreTextRB.text = score.ToString ();

			nickNameTextRB.text = nickName.ToString ();
			SetRightAvator(avator);
			if (gender!=1) genderImageRB.sprite = PrefabManager._instance.femaleSprite;
			break;
		default:
			Debug.LogError ("Error!Can't find seat:" + seat);
			break;
		}
	}

	void UpdateIndicatorPos()
	{
	//	return;
		tempPercent = (float )scoreValueLB / ((float )scoreValueLB +(float) scoreValueRB);
		SetIndicatorPos (tempPercent);
	}

	float GetHorizontalDistance(Vector3 pos1,Vector3 pos2)
	{
		pos1.y = pos2.y;
		return Vector3.Distance (pos1, pos2);
	}

	public int GetSumPoint()
	{
		return sumScoreValue;
	}

	public void SetLeftAvator(Sprite avatorSprite){
		avatorLB.sprite= avatorSprite;
	}
	public void SetRightAvator(Sprite avatorSprite){
		avatorRB.sprite = avatorSprite;
	}
}
