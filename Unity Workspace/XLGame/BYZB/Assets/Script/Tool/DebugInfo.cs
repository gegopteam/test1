using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class DebugInfo : MonoBehaviour {

	public static DebugInfo _instance;
	public static  Text thisText;
	static int lineCount;

	public bool showDebugInfo = false;

	float updateInterval=0.5f;
	float accum=0;
	float frames=0;
	float timeLeft;

	void Start () {
		thisText = this.GetComponent<Text> ();
		thisText.text = "DebugInfo:";
		lineCount = 0;
		if (!showDebugInfo) {
			this.GetComponent<Text> ().enabled = false;
		}

		timeLeft = updateInterval;
	}
	
	// Update is called once per frame
	void Update () {
		timeLeft -= Time.deltaTime;
		accum += Time.timeScale / Time.deltaTime;
		frames++;
		if (timeLeft <= 0f) {
			Show ("fps:" + (accum / frames).ToString ("f2"));
			timeLeft = updateInterval;
			accum = 0f;
			frames = 0;
		}
	}

	public static  void Show(string infoText)
	{
		thisText.text =infoText;
		lineCount = 0;
	}

	public static void Add(string infoText)
	{
		if (lineCount >= 10) 
		{
			thisText.text = "DebugInfo:";
			lineCount = 0;
		}
		string newString = thisText.text + "\n" + infoText;
		thisText.text = newString;
		lineCount++;
	}
}
