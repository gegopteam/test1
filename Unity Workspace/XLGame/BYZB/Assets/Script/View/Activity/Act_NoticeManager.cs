using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Act_NoticeManager : MonoBehaviour
{

	public static Act_NoticeManager Instance;
	public int laseSelectNum = 0;
	public static List<int> notDataList = new List<int> ();

	void Awake ()
	{
		Instance = this;
	}

	void Start ()
	{

	}

	public void NoticeToggleClick (Toggle toggle)
	{
		AudioManager._instance.PlayEffectClip (AudioManager.ui_click);
		laseSelectNum = (int)toggle.GetComponent<NoticeGrid> ().DataObject;
		if (notDataList.Contains ((int)toggle.GetComponent<NoticeGrid> ().DataObject)) {
			notDataList.Remove ((int)toggle.GetComponent<NoticeGrid> ().DataObject);
		}
	}
}
