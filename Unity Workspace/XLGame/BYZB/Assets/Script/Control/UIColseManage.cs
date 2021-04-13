using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using AssemblyCSharp;
using System;
using System.IO;

public class UIColseManage : MonoSingleton<UIColseManage>
{


	public  Stack<GameObject> windows = new Stack<GameObject> ();
	[HideInInspector]
	public bool isFishingBuyLongCard = false;
	[HideInInspector]
	public int intoBossRommType = -1;

	public void ShowUI (GameObject obj, UnityAction<GameObject> loadFinishHandler = null)
	{

		if (windows.Contains (obj)) {
			return;
		}
		windows.Push (obj);
//		Debug.LogError ("windows" + windows.Count);

		//		if (loadFinishHandler != null) {
		//			loadFinishHandler (types);
		//		}
	}

	public void CloseUI ()
	{
		if (windows.Count == 0)
			return;
		GameObject obj = windows.Pop ();
		Destroy (obj);
	}

	public void CloseAll ()
	{

		if (windows.Count == 0)
			return;

		GameObject obj = windows.Pop ();
		Destroy (obj);
		windows.Clear ();

	}
}
//public enum UIType{
//	None = -1,
//	UserInfo,
//	Mail,
//	PackBag,
//	Friend,
//	Shop,
//	Task,
//	Bank,
//	Rank,
//	Help,
//	Setting,
//	ViPPrivilege,
//	Firstcharge
//}
