using UnityEngine;
using System.Collections;

public class GameBackBtn : MonoBehaviour
{

	public static GameBackBtn instance;
	public GameObject backConfirmPanel;
	public GameObject backExperiencePanel;

	void Awake ()
	{
		instance = this;
	}

	public void Btn_ShowBackConfirm ()
	{
		AudioManager._instance.PlayEffectClip (AudioManager.ui_click);
		if (!GameController._instance.isExperienceMode) {
			GameObject temp = GameObject.Instantiate (backConfirmPanel);
			temp.transform.parent = ScreenManager.uiScaler.transform;
			temp.transform.position = Vector3.zero;
			temp.transform.localScale = new Vector3 (1.3f, 1.3f, 1.3f);
			string str = "真的不想再多玩一會了嗎？";
			if (GameController._instance.myGameType != GameType.Classical) {
				str = "是否確認退出比賽?退出後將直接判負,無法重新進入!";
			} else {
				if (GameController._instance.isRedPacketMode) {
					if (LT_GetRedPacket._instance.GetRedpacketListCount () > 0) {
						str = "當前有紅包尚未打開，是否離開？";
					} else {
						str = "離開將清空紅包場進度條，是否離開？";
					}
				}
			}
			temp.GetComponent<AskBackPanel> ().Show (str);	
		} else {
			GameObject temp = GameObject.Instantiate (backExperiencePanel);
			temp.transform.parent = ScreenManager.uiScaler.transform;
			temp.transform.position = Vector3.zero;
			temp.transform.localScale = new Vector3 (.6f, .6f, .6f);
		}
	}


}
