using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UITipBuyDragon : MonoBehaviour
{
	Button continueBtn;
	Button goBuyBtn;

	void Start ()
	{
		goBuyBtn = transform.Find ("BgControl/GoBuyBtn").GetComponent <Button> ();
		continueBtn = transform.Find ("BgControl/ContinueBtn").GetComponent <Button> ();

		goBuyBtn.onClick.RemoveAllListeners ();
		goBuyBtn.onClick.AddListener (ClickGoBuyBtn);
		continueBtn.onClick.RemoveAllListeners ();
		continueBtn.onClick.AddListener (ClickContinueBtn);
	}

	void ClickGoBuyBtn ()
	{
		AudioManager._instance.PlayEffectClip (AudioManager.ui_click);
		string path = "MainHall/MothlyCard/DragonCardWindow";
		GameObject WindowClone = AppControl.OpenWindow (path);
		WindowClone.SetActive (true);
		Destroy (gameObject);
	}

	void ClickContinueBtn ()
	{
		AudioManager._instance.PlayEffectClip (AudioManager.ui_click);
		QuitBack.isQuit = true;
		Destroy (gameObject);
	}
}
