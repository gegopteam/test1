using UnityEngine;
using System.Collections;
using DG.Tweening;
using UnityEngine.UI;
public class UIToolTween : MonoBehaviour {
	public Image aim;
	public Image freeze;
	public Image coin;

	// Use this for initialization
	void Start () {
		
	}

	public bool ToolTween()
	{
		Vector3 freezeTool = aim.transform.localPosition;
		Debug.Log ("freezeTool"+freezeTool);
		Debug.Log ("道具动画");
		freeze.transform.DOMove (freezeTool,1.5f);
		freeze.transform.DOScale (Vector3.zero, 1.5f);
		coin.transform.DOMove (freezeTool,1.5f);
		coin.transform.DOScale (Vector3.zero,1.5f);

		return true;
	}

	void ToBag()
	{
		
	}

	// Update is called once per frame
	void Update () {
	
	}
}
