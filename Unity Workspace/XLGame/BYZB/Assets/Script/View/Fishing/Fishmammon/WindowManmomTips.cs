using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using AssemblyCSharp;

public class WindowManmomTips : MonoSingleton<WindowManmomTips>
{

	public Text rewardnumText;
	public Text time;
	public Text tipString;
	public long rewardnum;
	private float timeadd = 0f;
	private float CurrentTime = 0f;
	// Use this for initialization
	void Start ()
	{
		rewardnumText.text = rewardnum.ToString ();
		if (time.text != "") {
			CurrentTime = int.Parse (time.text);
		}
	}
	
	// Update is called once per frame
	void Update ()
	{
		timeadd += Time.deltaTime;	
		if (CurrentTime > 0) {
			if (timeadd >= 1) {
				CurrentTime -= 1;
				time.text = CurrentTime.ToString ();
				timeadd = 0f;
			}
		} else {
			Destroy (this.gameObject);
		}

		
	}

	public void OnCliclDouble ()
	{
		Facade.GetFacade ().message.rank.SendGetRankInfoRequest (2);
		Facade.GetFacade ().message.fishCommom.SendManMonRankReward ();
		string path = "Window/WindowManmonShow";
		GameObject WindowClone = AppControl.OpenWindow (path);
		WindowClone.gameObject.SetActive (true);
		UIManmonGameShow gameshow = WindowClone.gameObject.GetComponent<UIManmonGameShow> ();
		gameshow.ShowMoneyNum = rewardnum;
		Destroy (this.gameObject);
	}
}
