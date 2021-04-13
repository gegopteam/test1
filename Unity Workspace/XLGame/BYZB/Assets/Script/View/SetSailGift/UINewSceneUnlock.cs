using UnityEngine;
using System.Collections;
using DG.Tweening;
using UnityEngine.UI;
using AssemblyCSharp;

public class UINewSceneUnlock : MonoBehaviour {

	public int Mutiple;
	MyInfo nInfo;
	public static bool isToLoad500 = false;
	public static bool isToLoad300 = false;

	/// <summary>
	/// 倒计时时间
	/// </summary>
	//	Text cutDownTimeText;
	//	int time = 30;
	public static int beginType = 0;

	public static UINewSceneUnlock Instance;

	void Awake()
	{
		Instance = this;
	}

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	public void OnSure()
	{
		nInfo = (MyInfo)Facade.GetFacade().data.Get(FacadeConfig.USERINFO_MODULE_ID);
		int nMaxMutiple = nInfo.cannonMultipleMax;

		QuitBack.isQuit = true;
		GameObject[] bulletGroups = GameObject.FindGameObjectsWithTag(TagManager.bullet);
		if (bulletGroups.Length > 0)
		{
			for (int i = 0; i < bulletGroups.Length; i++)
			{
				bulletGroups[i].GetComponent<Bullet>().isMyRobot = false;
			}
		}
		ReturnRobot();
		transform.Find("BtnSure").GetComponent<Image>().raycastTarget = false;
		Invoke("DelayLeave", 0.2f);//留时间用来清理机器人的遗留子弹
		QuitBack.isQuit = true;


		if (Mutiple == 500)
		{
			GradeField.unlockScene = true;
			GradeField.whichGame = 3;
            nInfo.isShowNumner = 3;
			LeaveRoomTool.LeaveRoom();
		}
		else if (Mutiple == 300)
		{
			GradeField.unlockScene = true;
			GradeField.whichGame = 2;
            nInfo.isShowNumner = 2;
			LeaveRoomTool.LeaveRoom();
		}
		Destroy(gameObject);
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
				if (tempGun.isRobot)
				{

					tempGun.RobotSendLeaveSelf();
				}

			}
		}
	}

	void DelayLeave()
	{
		ChatDataInfo nInfo = (ChatDataInfo)Facade.GetFacade().data.Get(FacadeConfig.CHAT_MODULE_ID);
		AudioManager._instance.PlayEffectClip(AudioManager.effect_closePanel);
		nInfo.ClearChatMsg();
		UIFishing._instance.OnBack();
	}

	public void OnClose()
	{
		AudioManager._instance.PlayEffectClip(AudioManager.effect_closePanel);
		//DataControl.GetInstance ().GetMyInfo ().account = null;
		Destroy(gameObject);
	}
}
