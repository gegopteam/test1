using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class HintText : MonoBehaviour {

	public static HintText _instance;
	public Text thisText;

	void Awake(){
		if (_instance == null)
			_instance = this;
	}


	public void ShowHint(string str,float hideTime=3f)
	{
		//return;//debug的时候再用，演示版本的时候return了
		thisText.text = str;
		this.GetComponent<RectTransform> ().localPosition = Vector3.zero;
		Invoke ("HideHint", hideTime);
	}

	public void HideHint()
	{
		this.GetComponent<RectTransform> ().localPosition = Vector3.up * 1000f;
	}


}
