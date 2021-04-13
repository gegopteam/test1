using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class Pk_Friend_RankItem : MonoBehaviour {

	public Text rankText;
	public Text nickNameText;
	public Image avatorImage;
	public Text sumScoreText;
	public GameObject dividingLine;

	public GameObject [] roundScoreGroup;

	int sum=0;

	
	public void Init () {
		for (int i = 0; i < roundScoreGroup.Length; i++) {
			roundScoreGroup [i].gameObject.SetActive (false);
		}
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	public int GetSumScore()
	{
		return sum;
	}

	public void SetUserById(int userId){
        GunControl tempGun = PrefabManager._instance.GetGunByUserID(userId);
        if(tempGun!=null){
            GunInUI gunUI = tempGun.gunUI;
            this.nickNameText.text = gunUI.GetNickName();
            this.avatorImage.sprite = gunUI.gunControl.GetAvatorSprite();

        }else{
            Debug.LogError("Can't find gun [userId=]"+userId);
        }

		
	}

	public void SetRoundScore(int round,int score){
		roundScoreGroup [round ].gameObject.SetActive (true);
		roundScoreGroup [round].transform.Find ("Value").GetComponent<Text> ().text = score.ToString ();
		sum += score;
		sumScoreText.text = sum.ToString ();
	}

	public void SetRank(int rankNum)
	{
		this.rankText.text = rankNum.ToString ();	
	}

	public void Hide()
	{
		this.gameObject.SetActive (false);
	}

	public void HideDividingLine()
	{
		dividingLine.SetActive (false);
	}

	public float GetRectPosX()
	{
		return this.GetComponent<RectTransform> ().localPosition.x;
	}

	public void SetRectPosX(float xValue)
	{
		Vector3 tempPos = this.GetComponent<RectTransform> ().localPosition;
		this.GetComponent<RectTransform> ().localPosition = new Vector3 (xValue, tempPos.y, tempPos.z);
	}
}
