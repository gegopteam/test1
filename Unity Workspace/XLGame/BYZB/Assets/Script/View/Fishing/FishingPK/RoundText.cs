using UnityEngine;
using System.Collections;

public class RoundText : MonoBehaviour {

	public static RoundText _instance;

	public Sprite[] roundGroup; 

	public SpriteRenderer roundShow;

	int roundIndex=0;

	void Awake()
	{
		if (null == _instance)
			_instance = this;
		roundIndex = 0;
	}



	public void SetRoundText(int index)
	{
		roundShow.sprite = roundGroup [index];
	}

	public void SetShow(bool shouldShow)
	{
		if (shouldShow) {
			roundIndex++;
			transform.position = Vector3.zero;
			SetRoundText (roundIndex);
			Invoke ("HideSelf", 1.5f);

		} else {
			transform.position = Vector3.up * 1000f;
		}
	}

	void HideSelf()
	{
		SetShow (false);
		PrefabManager._instance.ResetGunScore ();
	}
}
