using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using DG.Tweening;

public class EffectNum : MonoBehaviour {

	public Font sliverFont;

	public int effectNumType=1;
	float destroyTime=2f;

	// Use this for initialization
	void Start () {
		Destroy (this.gameObject, 2f);
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	public void SetInfo(int num,Vector3 pos,bool isLocal,bool isBonus=false ) 
	{
		Text thisText = this.GetComponent<Text> ();
		if (!isLocal) {
			thisText.font = sliverFont;
		}

		thisText.text = "+" + num.ToString ();
		SetUIPosByWorldPos (pos,isBonus);
	}

	public void SetUIPosByWorldPos(Vector3 worldPos ,bool isBouns)
	{
		Vector3 uiPos = ScreenManager.WorldToUIPos (worldPos);
		this.GetComponent<RectTransform> ().localPosition = uiPos;
		//Vector3 originalScale = this.GetComponent<RectTransform> ().localScale;
		this.GetComponent<RectTransform> ().localScale = Vector3.zero;

		if (isBouns) {
			if (effectNumType == 1) {
				transform.DOScale (1f, 0.5f);
				destroyTime = 3f;
			}
			else
				transform.DOScale (0.25f, 0.5f);
		} else {
			if (effectNumType == 1)
				transform.DOScale (0.4f, 0.7f);
			else
				transform.DOScale (0.25f, 0.5f);
		}
		Destroy (this.gameObject, destroyTime);
	}
}
