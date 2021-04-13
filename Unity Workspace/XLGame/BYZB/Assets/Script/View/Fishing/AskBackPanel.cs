using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using AssemblyCSharp;

public class AskBackPanel : MonoBehaviour
{
    public static AskBackPanel _instance = null;
	public Text infoText;

	public GameObject confirmBtn;
	public GameObject cancleBtn;
	public GameObject redforkBtn;

	bool shouldLeave = true;

    private void Awake()
    {
        _instance = this;
    }
    private void OnDestroy()
    {
        _instance = null;
    }

    public void Show (string str, bool hideCancle = false, bool leaveFlag = true)
	{
		QuitBack.isQuit = false;
		infoText.text = str;
		shouldLeave = leaveFlag;
		if (hideCancle) {
			transform.Find ("Mask").GetComponent<Image> ().enabled = true;
			cancleBtn.SetActive (false);
			redforkBtn.SetActive (false);
			Vector3 temp = confirmBtn.GetComponent<RectTransform> ().localPosition;

			confirmBtn.GetComponent<RectTransform> ().localPosition = new Vector3 (0, temp.y, temp.z);
		}
	}

	public void Btn_Confirm ()
	{
		QuitBack.isQuit = true;
        UIHallCore.GameComeOutFromHall = true;

        if (shouldLeave) {
            GameObject[] bulletGroups = GameObject.FindGameObjectsWithTag(TagManager.bullet);
            if (bulletGroups.Length > 0)
            {
                for (int i = 0; i < bulletGroups.Length; i++)
                {
                    bulletGroups[i].GetComponent<Bullet>().isMyRobot = false;
                }
            }
            ReturnRobot();
            transform.Find("Btn_Confirm").GetComponent<Image>().raycastTarget=false;
            if (UIHallCore.PopSevenDaySignForFrist)
                UIHallCore.FristTimeLeaveGame = true;
            //DelayLeave();
            Invoke("DelayLeave", 0.2f);//留时间用来清理机器人的遗留子弹
		} else {
            Destroy (this.gameObject);
		}
		
	}

    void DelayLeave(){
        ChatDataInfo nInfo = (ChatDataInfo)Facade.GetFacade().data.Get(FacadeConfig.CHAT_MODULE_ID);
        AudioManager._instance.PlayEffectClip(AudioManager.effect_closePanel);
        nInfo.ClearChatMsg();
        UIFishing._instance.OnBack();
    }

	public void Btn_Cancle ()
	{
		QuitBack.isQuit = true;
		AudioManager._instance.PlayEffectClip (AudioManager.ui_click);
		Destroy (this.gameObject);
	}

    void ReturnRobot()
    {
       // Debug.LogError("LocalLeave");

        FishingCommonMsgHandle temp = new FishingCommonMsgHandle();
        for (int i = 0; i < PrefabManager._instance.gunGroup.Length; i++)
        {
            GunControl tempGun = PrefabManager._instance.gunGroup[i].GetComponent<GunControl>();
            if (tempGun != null)
            {
                if (tempGun.isRobot){
                   
                  tempGun.RobotSendLeaveSelf();
                }
                    
            }
        }
    }
	
}