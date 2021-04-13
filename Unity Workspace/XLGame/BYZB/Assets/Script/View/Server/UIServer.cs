using UnityEngine;
using System.Collections;

public class UIServer : MonoBehaviour {

	// Use this for initialization
	void Start () {
		//DontDestroyOnLoad (this.gameObject);
		UIColseManage.instance.ShowUI (this.gameObject);
	}

	public void ExitButton()
	{
		AudioManager._instance.PlayEffectClip(AudioManager.effect_closePanel);
		DestroyImmediate (this.gameObject);
		UIColseManage.instance.CloseUI ();
	}
}
