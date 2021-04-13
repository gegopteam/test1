using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class UITipAutoNoMask : MonoBehaviour
{
	public Text tipText;
	public Image bgshow;
	private float time;

	// Use this for initialization
	void Start ()
	{
		time = 0f;

	}
	
	// Update is called once per frame
	void Update ()
	{
		if (tipText.text.Length >= 20) {
			bgshow.gameObject.GetComponent<RectTransform> ().sizeDelta = new Vector2 (1600f, 88f);
		}
		time += Time.deltaTime;
		if (time >= 2f) {
			Destroy (this.gameObject);
		}
	}
}
