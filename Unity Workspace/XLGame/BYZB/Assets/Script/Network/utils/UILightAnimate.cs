using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class UILightAnimate : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}

	bool bAnimateStart = false;

	public void StartAnimate()
	{
		//bAnimateStart = true;
		Image nImage = GetComponent<Image> ();
		nImage.color = new Color (nImage.color.r, nImage.color.g, nImage.color.b, 1.0f);
	}

	// Update is called once per frame
	void Update () {
		Image nImage = GetComponent<Image> ();
		if ( nImage.color.a <= 0.0f )
			return;
		
		float nAlpha = nImage.color.a - 0.05f;
		if (nAlpha <= 0)
			nAlpha = 0;
		nImage.color = new Color (nImage.color.r, nImage.color.g, nImage.color.b, nAlpha);
	}
}
