using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using AssemblyCSharp;

public class ChangeScale : MonoBehaviour
{

	public bool isSprite = false;

	float match = 0.25f;
	public float scaleValue = 1.1f;

	public float Match {
		get{ return match; }
		set{ match = value; }
	}

	void Start ()
	{
		//if (Facade.GetFacade ().config.isIphoneX ()) {
		if (isSprite) {
			transform.localScale *= scaleValue;
		} else {
			if (GetComponent<CanvasScaler> () != null)
				GetComponent<CanvasScaler> ().matchWidthOrHeight = match;
		}

		//}
	}


}
