using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class Pk_Bullet_FinalRankItem : MonoBehaviour {

	public int index=0;
	Text scoreText;
	Text goldNumText;
	Text nickNameText;

	public void Hide()
	{
		transform.gameObject.SetActive (false);
	}

	public void SetInfo(int score,long goldNum,string nickName,Sprite avatorSprite)
	{
		scoreText = transform.Find ("Score").GetComponent<Text> ();
		goldNumText = transform.Find ("GoldSprite/Text").GetComponent<Text> ();
		nickNameText = transform.Find ("AvatorBox/NickName").GetComponent<Text> ();

		scoreText.text = score.ToString ();
		goldNumText.text = goldNum.ToString ();
		nickNameText.text = nickName.ToString ();

        if (avatorSprite != null)
            transform.Find("AvatorBox/Avator").GetComponent<Image>().sprite = avatorSprite;
        else
            Debug.LogError("Error! Pk_Bullet_FinalRankItem:avatorSprite=null");
	}
		
}
