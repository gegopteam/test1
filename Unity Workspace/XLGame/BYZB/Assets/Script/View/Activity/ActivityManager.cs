using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ActivityManager : MonoBehaviour
{
	public static ActivityManager Instance;
	[HideInInspector]
	public Toggle selectToggle;
	public int laseSelectNum = 0;
	public static List<int> actDataList = new List<int> ();

	void Awake ()
	{
		Instance = this;
	}

	void Start ()
	{
		
	}

	public void ActivityToggleClick (Toggle toggle)
	{
		AudioManager._instance.PlayEffectClip (AudioManager.ui_click);
		laseSelectNum = (int)toggle.GetComponent<ActivityGrid> ().DataObject;
		if (actDataList.Contains ((int)toggle.GetComponent<ActivityGrid> ().DataObject)) {
			actDataList.Remove ((int)toggle.GetComponent<ActivityGrid> ().DataObject);
		}
	}
}
