using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class TalkInput : MonoBehaviour {
	public delegate void InputTextDelegate (string talk);
	public static event InputTextDelegate InputTextEvent;

	public InputField input;
	private string talk;

	// Use this for initialization
	void Start () {
	
	}

	public void SendButton()
	{
		if (InputTextEvent != null) {
		
			InputTextEvent (talk);
		}
	}

	// Update is called once per frame
	void Update () {
		talk = input.text;
	
	}
}
