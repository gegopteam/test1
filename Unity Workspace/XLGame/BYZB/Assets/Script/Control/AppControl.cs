/* author:KinSen
 * Date:2017.05.15
 */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using AssemblyCSharp;
using UnityEngine.SceneManagement;

public class AppControl
{
	private static AppControl instance = null;
	private static GameObject window;
	private static GameObject WindowClone;
	private static Dictionary<string,GameObject> WindowDictory = new Dictionary<string, GameObject> ();
	private static AppView appView = AppView.NONE;
	public static bool miniGameState = false;

	public static AppControl GetInstance ()
	{
		if (null == instance) {
			instance = new AppControl ();
		}
		return instance;
	}

	public static void DestroyInstance ()
	{
		if (null != instance) {
			instance = null;
		}
	}

	private AppControl ()
	{
		
	}

	~AppControl ()
	{
		WindowDictory.Clear ();
	}

	public AppView getActiveView ()
	{
		return appView;
	}

	public static void SetView (AppView view)
	{
		appView = view;
	}

	//跳转界面
	public static void ToView (AppView view)
	{
		if (appView == view)
			return;

		if (SceneManager.GetActiveScene ().name == "Fishing") { //说明要从渔场出去，需要保存信息
            
		}
		bool shouldLoadAsync = false;
		string viewName = "";
		switch (view) {
		case AppView.LOAD:
			{
				viewName = "Load";
			}
			break;
		case AppView.LOADING:
			viewName = "Loading";
			break;
		case AppView.LOGIN:
			{
				viewName = "WeChatLogin";
				AudioManager._instance.PlayBgm (AudioManager.bgm_hall);
			}
			break;
		case AppView.HALL:
			{
				viewName = "Hall";//"Hall";
				AudioManager._instance.PlayBgm (AudioManager.bgm_hall);
			}
			break;
		case AppView.FISHING:
			{
				shouldLoadAsync = true;
				viewName = "Fishing";
			}
			break;
		case AppView.PKHALLMAIN:
			AudioManager._instance.PlayBgm (AudioManager.bgm_hall);
			viewName = "HallPkSelect";//"HallPkMain";
			break;
		case AppView.HALLROOMCARD:
			{
				AudioManager._instance.PlayBgm (AudioManager.bgm_hall);
				viewName = "HallRoomCard";
			}
			break;
		case AppView.CLASSICHALL:
			AudioManager._instance.PlayBgm (AudioManager.bgm_hall);
			viewName = "HallClassic";
			break;
		case AppView.PKHALL:
			{
				AudioManager._instance.PlayBgm (AudioManager.bgm_hall);
				viewName = "HallPk";
			}
			break;
		case AppView.ROOM:
			{
				AudioManager._instance.PlayBgm (AudioManager.bgm_hall);
				viewName = "Room";
			}
			break;
            case AppView.HALLNEWPLAY:
                {   //添加新的场景
                    AudioManager._instance.PlayBgm(AudioManager.bgm_hall);
                    viewName = "HallNewPlay";
                }
                break;
		}
		if ("" != viewName) {
			appView = view;
			//Debug.Log ("ToView -> " + viewName);
			//Application.LoadLevel(viewName); //过时
			if (shouldLoadAsync) {
				SceneManager.LoadSceneAsync (viewName);
			} else {
				SceneManager.LoadScene (viewName);
			}
				
		}

	}
	//添加小界面
	public  static  GameObject OpenWindow (string path)
	{

		window = Resources.Load (path) as GameObject;
		WindowClone = GameObject.Instantiate (window)as GameObject;
		return WindowClone;

		if (WindowDictory.ContainsKey (path)) {
			Debug.Log ("OpenWindow");
			return WindowDictory [path];
		} else {
			window = Resources.Load (path) as GameObject;
			WindowClone = GameObject.Instantiate (window)as GameObject;
			WindowDictory.Add (path, WindowClone);
			return window;
		}
	}
	//删除小界面
	public static void CloseWindow (string path)
	{
		WindowDictory.Remove (path);
	}
}
