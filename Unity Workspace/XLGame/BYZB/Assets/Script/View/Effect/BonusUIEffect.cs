using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class BonusUIEffect : MonoBehaviour
{

	public Text numText;
	public SpriteRenderer enemySprite;

	public Sprite[] bounsUIGroup;

	public void SetInfo (int num, int enemyTypeId = -1)
	{
		this.transform.localScale = Vector3.one * 45f;
		numText.text = num.ToString ();

		int uiIndex = 0;

		if (enemyTypeId >= 15 && enemyTypeId <= 24) {
			uiIndex = enemyTypeId - 15;
		} else {
			uiIndex = 0;
			Debug.LogError ("无法找到对应奖金鱼的UI");
		}

		//根据typeId来改变Sprite对应的图片
		enemySprite.sprite = bounsUIGroup [uiIndex];
	}


	public void SetInfo1 (int num)
	{
		this.transform.localScale = Vector3.one * 45f;
		numText.text = num.ToString ();
	}
}
