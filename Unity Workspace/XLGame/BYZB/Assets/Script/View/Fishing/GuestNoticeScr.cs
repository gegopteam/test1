using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using AssemblyCSharp;

public class GuestNoticeScr : MonoBehaviour
{
	Button goLevelBtn;
	bool shouldLeave = true;

	void Start ()
	{
		goLevelBtn = transform.Find ("BgControl/ContinueBtn").GetComponent <Button> ();
		goLevelBtn.onClick.RemoveAllListeners ();
		goLevelBtn.onClick.AddListener (ClickGoLevelBtn);
	}

	void ClickGoLevelBtn ()
	{
		QuitBack.isQuit = true;
		if (shouldLeave) {
			GameObject[] bulletGroups = GameObject.FindGameObjectsWithTag (TagManager.bullet);
			if (bulletGroups.Length > 0) {
				for (int i = 0; i < bulletGroups.Length; i++) {
					bulletGroups [i].GetComponent<Bullet> ().isMyRobot = false;
				}
			}
			Invoke ("DelayLeave", 0.2f);//留时间用来清理机器人的遗留子弹
		} else {
			Destroy (this.gameObject);

		}
	}

	void DelayLeave ()
	{
		ChatDataInfo nInfo = (ChatDataInfo)Facade.GetFacade ().data.Get (FacadeConfig.CHAT_MODULE_ID);
		AudioManager._instance.PlayEffectClip (AudioManager.effect_closePanel);
		nInfo.ClearChatMsg ();
		UIFishing._instance.OnBack ();
	}
}
