using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using AssemblyCSharp;

public enum BackScene //分别对应枪座的左下，右下，左上，右上四个方位
{
	CLASSICKTOHALL,
	//0
	RBPkSELETOHALL,
	//1
	FISHTOHALL,
	//2
	HALLTOLOADING,
	//3
	LOGINTOQUITE,
	//4


}

public class QuitBack : MonoBehaviour
{
	public BackScene sBackScene;

	public static bool isQuit = true;

	public delegate void Seedelegate ();

	public static event Seedelegate SeeEvent;
	// Use this for initialization
	void Awake ()
	{

	}

	// Update is called once per frame
	void Update ()
	{

		if (Input.GetKeyDown (KeyCode.Escape)) {
			switch (sBackScene) {
			case BackScene.CLASSICKTOHALL:
				if (UIColseManage.instance.windows.Count != 0) {
					if (UIColseManage.instance.windows.Count == 1) {
						UIColseManage.instance.CloseUI ();
					} else {
						UIColseManage.instance.CloseAll ();
					}
					return;
				}
				AppControl.ToView (AppView.HALL);
				break;
			case BackScene.RBPkSELETOHALL:
				if (UIColseManage.instance.windows.Count != 0) {
					if (UIColseManage.instance.windows.Count == 1) {
						UIColseManage.instance.CloseUI ();
					} else {
						UIColseManage.instance.CloseAll ();
					}
					return;
				}
				AppControl.ToView (AppView.HALL);
				break;
			case BackScene.FISHTOHALL:
				if (isQuit) {
					GameBackBtn.instance.Btn_ShowBackConfirm ();
				}
				break;
			case BackScene.LOGINTOQUITE:
				if (isQuit) {
					isQuit = false;
					GameObject Window1 = UnityEngine.Resources.Load ("Window/WindowTipsQuiteLogin") as UnityEngine.GameObject;
					GameObject WindowClone1 = UnityEngine.GameObject.Instantiate (Window1);
					UITipAutoQuiteLogin ClickTips1 = WindowClone1.GetComponent<UITipAutoQuiteLogin> ();
					ClickTips1.text.text = "確定要結束遊戲嗎？";
					ClickTips1.isQuiteGame = true;
				}
				break;
			case BackScene.HALLTOLOADING:
				Debug.LogError(isQuit);
				Debug.LogError (UIColseManage.instance.windows.Count);
				if (UIColseManage.instance.windows.Count != 0) {
						UIColseManage.instance.CloseUI ();
		
					if (UIVIP.isopen) {
						if (SeeEvent != null) {
							SeeEvent ();
						}
					}
					return;
				}
				if (isQuit) {
					MyInfo myInfo = (MyInfo)Facade.GetFacade ().data.Get (FacadeConfig.USERINFO_MODULE_ID);
					if (myInfo.isGuestLogin) {
						isQuit = false;
						GameObject Window1 = UnityEngine.Resources.Load ("Window/WindowTipsQuiteLogin") as UnityEngine.GameObject;
						GameObject WindowClone1 = UnityEngine.GameObject.Instantiate (Window1);
						UITipAutoQuiteLogin ClickTips1 = WindowClone1.GetComponent<UITipAutoQuiteLogin> ();
						ClickTips1.text.text = "確定要結束遊戲嗎？";
						ClickTips1.isQuiteGame = true;
					} else {
						OpenQuitenotice ();
					}

				}
				break;

			}

		}
	}

	void OpenQuitenotice ()
	{
		isQuit = false;
		GameObject Window1 = UnityEngine.Resources.Load ("Window/WindowTipsQuiteLogin") as UnityEngine.GameObject;
		GameObject WindowClone1 = UnityEngine.GameObject.Instantiate (Window1);
		UITipAutoQuiteLogin ClickTips1 = WindowClone1.GetComponent<UITipAutoQuiteLogin> ();
		ClickTips1.text.text = "你確定要退出遊戲嗎?";
	}



}
