using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class PersonalInfo : MonoBehaviour {
	public MiddleInfo middle;
	public ButtomInfo buttom;
	public Button changname;


	// Use this for initialization
	void Start () {
		UIColseManage.instance.ShowUI (this.gameObject);
		if (!LoginUtil.GetIntance ().IsNoteQqorWet) {
			changname.gameObject.SetActive (false);
		}

	}
		
	public void CloseButton(){
		AudioManager._instance.PlayEffectClip(AudioManager.effect_closePanel);
		Destroy(this.gameObject);
		UIColseManage.instance.CloseUI ();
	}
		

}
