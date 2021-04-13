using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using AssemblyCSharp;

public class AskBackExperiencePanel : MonoBehaviour
{
	Button continueBtn;
	Button leaveBtn;
	bool shouldLeave = true;

	void Start ()
	{
		leaveBtn = transform.Find ("BgControl/ExitBtn").GetComponent <Button> ();
		continueBtn = transform.Find ("BgControl/ContinueBtn").GetComponent <Button> ();

		leaveBtn.onClick.RemoveAllListeners ();
		leaveBtn.onClick.AddListener (ClickLeaveBtn);
		continueBtn.onClick.RemoveAllListeners ();
		continueBtn.onClick.AddListener (ClickContinueBtn);
	}

	void ClickLeaveBtn ()
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

	void ClickContinueBtn ()
	{
		QuitBack.isQuit = true;
		AudioManager._instance.PlayEffectClip (AudioManager.ui_click);
		Destroy (gameObject);
	}
}
