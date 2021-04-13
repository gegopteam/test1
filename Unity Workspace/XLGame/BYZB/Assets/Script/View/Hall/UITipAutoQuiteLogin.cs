using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UITipAutoQuiteLogin : MonoBehaviour
{


	public Text text;
	public bool isQuiteGame = false;

	public void SureButton ()
	{
		if (isQuiteGame) {
			QuitBack.isQuit = true;
			Application.Quit ();
			Destroy (this.gameObject);
		}
		QuitBack.isQuit = true;
		AppControl.ToView (AppView.LOGIN);
		Destroy (this.gameObject);
	}

	public void CancelButton ()
	{
		if (isQuiteGame) {
			QuitBack.isQuit = true;
			Destroy (this.gameObject);
			return;
		}	
		QuitBack.isQuit = true;
		Destroy (this.gameObject);
	}

}
