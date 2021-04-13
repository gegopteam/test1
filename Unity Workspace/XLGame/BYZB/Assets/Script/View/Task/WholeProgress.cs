using UnityEngine;
using System.Collections.Generic;
using UnityEngine.UI;
using AssemblyCSharp;
/// <summary>
/// Whole progress.大进度条
/// </summary>
public class WholeProgress : MonoBehaviour {
	public static WholeProgress instance;
	public Slider wholeProgress;
	public Animator twee;
	public Animator five;
	public Animator eight;
	public Animator hund;
	private List<int> states;
	void Awake(){
		instance = this;
	}
	// Use this for initialization
	void Start () {
		twee.enabled = false;
		five.enabled = false;
		eight.enabled = false;
		hund.enabled = false;
		states = DataControl.GetInstance ().getTaskInfo ().GetStates ();
		if (states.Count > 0) {
			for (int i = 0; i < states.Count; i++) {
				UITask.instance.CompleteActive (states [i]);
			}
		}
	}
	
	// Update is called once per frame
	void Update () {
		if(wholeProgress.value >= 0.16f){
			if (twee != null) {
				twee.enabled = true;
			}
		}
		if(wholeProgress.value >= 0.41f){
			if (five != null) {
				five.enabled = true;
			}
		}
		if (wholeProgress.value >= 0.64f){
			if (eight != null) {
				eight.enabled = true;
			}
		}
		if (wholeProgress.value >= 1f) {
			if (hund != null) {
				hund.enabled = true;
			}
		}
	}
	public void Tweenty()
	{
		AudioManager._instance.PlayEffectClip (AudioManager.ui_click);
		Facade.GetFacade ().message.task.SendActivityAwardRequest ( 20 );
	}

	public void  Fivety()
	{
		AudioManager._instance.PlayEffectClip (AudioManager.ui_click);
		Facade.GetFacade ().message.task.SendActivityAwardRequest ( 50 );
	}

	public void Eighty()
	{
		AudioManager._instance.PlayEffectClip (AudioManager.ui_click);
		Facade.GetFacade ().message.task.SendActivityAwardRequest ( 80 );
	}

	public void Hundred()
	{
		AudioManager._instance.PlayEffectClip (AudioManager.ui_click);
		Facade.GetFacade ().message.task.SendActivityAwardRequest ( 120 );
	}
}
