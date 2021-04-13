using UnityEngine;
using System.Collections;
using UnityEngine.UI;

//UIOther是不属于任何场景的UI对象
//负责：Other部分UI对象的管理
//比如：(断网提示)弹框等，可能出现在任何一个场景中
public class UIOtherObjects
{
	private static UIOtherObjects instance = null;
	private GameObject WindowClone;

	public static UIOtherObjects GetInstance()
	{
		if(null == instance)
		{
			instance = new UIOtherObjects ();
		}

		return instance;
	}

	public static void DestroyInstance()
	{
		if(null!=instance)
		{
			instance = null;
		}
	}

	//断线
	public void ShowLineTip(string str)
	{
		return;
		string path = "Window/ShowLineTip";
		WindowClone = AppControl.OpenWindow (path);
		WindowClone.SetActive (true);
		Text text =  WindowClone.GetComponentInChildren<Text> ();
		if (str == "") {
			WindowClone.SetActive (false);
		} else {
			text.text = str;
		}
	}

	//没有网络
	public void ShowNowNoWifi(string str)
	{
		return;
		string path = "Window/ShowNowNoWifi";
		WindowClone = AppControl.OpenWindow (path);
		WindowClone.SetActive (true);
		Text text =  WindowClone.GetComponentInChildren<Text> ();
		if (str == "") {
			WindowClone.SetActive (false);
		} else {
			text.text = str;
		}
	}

}
