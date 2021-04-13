using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using AssemblyCSharp;

public class NewcomerRewardPanel : MonoBehaviour
{

	public static NewcomerRewardPanel _instance = null;

	public Image rewardImage;
	public Sprite[] rewardSpriteGroup;

	public Text rewardNumText;

	int rewardType;
	float rewardNum;
	bool haveSend = false;
	// Use this for initialization

	private DataControl dataControl = null;
	private MyInfo myInfo = null;

	void Init ()
	{
		if (null != _instance) {
			DestroyImmediate (NewcomerRewardPanel._instance.gameObject);
			Debug.LogError ("Error! NewcomerReward already exist!!!");
		}
		_instance = this;
		this.GetComponent<Canvas> ().worldCamera = ScreenManager.uiCamera;

		dataControl = DataControl.GetInstance();
		myInfo = dataControl.GetMyInfo();
	}

	public void SetRewardInfo (int rewardType, float rewardNum)
	{ //rewardType:0=diamond,1=redpacketTicket,2=lock,3=freeze
		Init ();
		this.rewardType = rewardType;
		this.rewardNum = rewardNum;
		rewardImage.sprite = rewardSpriteGroup [rewardType];
		rewardNumText.text = rewardNum.ToString ();
	}

	public void Btn_GetReward ()
	{
		if (!haveSend) {
			NewcormerMissionPanel._instance.currentMissionIndex = myInfo.beginnerCurTask;
			Facade.GetFacade ().message.task.SendBeginnerTaskRewardRequest (NewcormerMissionPanel._instance.currentMissionIndex);
			haveSend = true;
			Invoke ("ResetSendFlag", 3f);
		}
	}

	void ResetSendFlag ()
	{
		haveSend = false;
	}

	public void RecvReward (int rewardValue)
	{
		haveSend = true;
		if (rewardValue != rewardNum) {
			Debug.LogError ("ErrorDiamondNum! server=" + rewardValue + " clinet=" + rewardNum);
			HintTextPanel._instance.SetTextShow("鑽石不同步:" + "服務器=" + rewardValue + " 本地=" + rewardNum, 2f);
		}
		rewardNum = rewardValue;
		switch (rewardType) {
		case 0:
			PrefabManager._instance.GetLocalGun ().gunUI.AddValue (0, 0, (int)rewardNum);
			break;
		case 1:
			DataControl.GetInstance ().GetMyInfo ().redPacketTicket += rewardNum; //double和float可能有问题
			break;
		case 2:
			break;
		case 3:
			break;
		case 4:
			PrefabManager._instance.GetLocalGun ().gunUI.AddValue (0, (int)rewardNum, 0);
			break;
		default:
			break;
		}
		int currentMissionIndex = NewcormerMissionPanel._instance.currentMissionIndex;

		NewcormerMissionPanel._instance.currentMissionIndex++;
		NewcormerMissionPanel._instance.DestorySelf ();

		if (currentMissionIndex < 6)
			PrefabManager._instance.ShowNewcomerMission (currentMissionIndex + 1, 0);
//        else if (currentMissionIndex == 6)
//            PrefabManager._instance.ShowNewcomerMission(7, PrefabManager._instance.GetLocalGun().maxCannonMultiple);
        else if (currentMissionIndex == 6)
			PrefabManager._instance.ShowNewcomerMission (currentMissionIndex + 1, 0);
//        else if (currentMissionIndex == 8)
//            PrefabManager._instance.ShowNewcomerMission(9, PrefabManager._instance.GetLocalGun().maxCannonMultiple);
        else if (currentMissionIndex == 7)
			PrefabManager._instance.ShowNewcomerMission (currentMissionIndex + 1, 0);
//        else if (currentMissionIndex == 10)
//            PrefabManager._instance.ShowNewcomerMission(11, PrefabManager._instance.GetLocalGun().maxCannonMultiple);
        else if (currentMissionIndex == 8)
			PrefabManager._instance.ShowNewcomerMission (currentMissionIndex + 1, 0);
		else if (currentMissionIndex == 9)
			PrefabManager._instance.ShowNewcomerMission (currentMissionIndex + 1, 0);

		if (currentMissionIndex == 9) {
			// DataControl.GetInstance().GetMyInfo().beginnerCurTask = 10;
			// Panel_UnlockMultiples._instance.DelayShowToHallPanel(); //跳转条件已经改为150炮倍，改为在UnlockPanel里跳转
		}

		_instance = null;
		Destroy (this.gameObject);
	}

}
