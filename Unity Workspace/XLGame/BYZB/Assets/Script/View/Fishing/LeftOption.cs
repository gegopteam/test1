using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using DG.Tweening;
using AssemblyCSharp;

public class LeftOption : MonoBehaviour
{
	public static LeftOption _instance = null;

	public GameObject luckDrawBtn;
	public GameObject unlockGunBtn;
	public GameObject getCoinBtn;
	public GameObject illustratedBtn;
	public GameObject soundBtn;
	public GameObject messageBtn;

	public GameObject leftBorder;
	public GameObject popShowBtn;
	public GameObject popHideBtn;
	public GameObject manmonBtn;
	public GameObject otherLeftPanel;

	//抽奖页面
	public GameObject luckDrawPrefab;
	float popDistance = 0;

	[HideInInspector]
	public bool isShow = true;

	private void Awake ()
	{
		if (_instance != null) {
			Destroy (LeftOption._instance.gameObject);
		}
		_instance = this;
	}
	// Use this for initialization
	void Start ()
	{
		if (GameController._instance.isExperienceMode) {
			manmonBtn.gameObject.SetActive (false);
		}
//		Debug.LogError ("Type=" + GameController._instance.myGameType);
		switch (GameController._instance.myGameType) {
		case GameType.Classical:
			popDistance = popHideBtn.gameObject.transform.position.x - leftBorder.transform.position.x;
//			Debug.LogError ("popDistance=" + popDistance);
			popShowBtn.SetActive (false);
			popHideBtn.SetActive (true);

			if (DataControl.GetInstance ().GetMyInfo ().cannonMultipleMax >= GunControl.limitMultiple) {
				RemoveUnlockMultipleIcon ();
			}

			break;
		case GameType.Bullet:
			
		case GameType.Point:
			
		case GameType.Time:
			this.gameObject.transform.position -= new Vector3 (1000, 0, 0);
			break;
		default:
			break;
		}
		Btn_PopHide ();
	}

	public void Btn_PopShow ()
	{
		Debug.LogError ("PopShow");
		transform.DOMove (transform.position + Vector3.right * popDistance, 0.3f);

		otherLeftPanel.transform.position = Vector3.left * 500f;
	
		popShowBtn.SetActive (false);
		popHideBtn.SetActive (true);
		isShow = true;
		Panel_UnlockMultiples._instance.UpdateDiamondPorgress ();
		AudioManager._instance.PlayEffectClip (AudioManager.ui_click);


	}

	public void Btn_PopHide ()
	{
		//Debug.LogError ("PopHide");
		transform.DOMove (transform.position - Vector3.right * popDistance, 0.3f);

		otherLeftPanel.transform.position = transform.position;

		popShowBtn.SetActive (true);
		popHideBtn.SetActive (false);
		//Panel_UnlockMultiples._instance.SetShow (false); //新版ui不需要这行代码
		//LuckDrawHintBoard._instace.PopShow(false);
		AudioManager._instance.PlayEffectClip (AudioManager.effect_closePanel);
		isShow = false;

	}

	public void RemoveUnlockMultipleIcon ()
	{
		//新版ui不需要再移动其它ui位置
		unlockGunBtn.transform.position = Vector3.left * 1000f;
		//getCoinBtn.GetComponent<RectTransform> ().localPosition = 0.5f * (luckDrawBtn.GetComponent<RectTransform> ().localPosition +
		//illustratedBtn.GetComponent<RectTransform> ().localPosition);
	}

	public GameObject illustratedPrefab;

	public void ShowIllustratedPanel ()
	{
		AudioManager._instance.PlayEffectClip (AudioManager.ui_click);
		GameObject.Instantiate (illustratedPrefab);
	}

	//	public GameObject firstRechargePrefab;
	public GameObject storePrefab;

	public void ShowGetGoldPanel ()
	{

		AudioManager._instance.PlayEffectClip (AudioManager.ui_click);
		if (GameController._instance.isExperienceMode) {
			HintTextPanel._instance.SetTextShow ("體驗場暫未開放商城");
			return;
		}
//		bool isFirstRecharge = DataControl.GetInstance ().GetMyInfo ().loginInfo.preferencePackBought == 0 ? true : false; //是否购买过特惠礼包，如果是，说明不是首充
		if (UIStore.instance != null) {
			return;
		}
		// PrefabManager._instance.ControlNewcomerMissionShow(false);
		GameObject temp = GameObject.Instantiate (storePrefab);
		temp.GetComponent<UIStore> ().CoinButton ();
//		Debug.LogError ("isFirstRecharge=" + isFirstRecharge);
//		if (isFirstRecharge) {
////			UIFirstRecharge.SetState = DataControl.GetInstance().GetMyInfo().loginInfo.preferencePackBought;
////			GameObject.Instantiate (firstRechargePrefab);
//		} else {
//			
//		}
	}

	// public GameObject luckDrawHintBoardPrefab;

	public void ShowLuckDrawHintBoard ()
	{
		if (LuckDrawHintBoard._instace != null) {
			Debug.LogError ("Efrafiasfo");
			LuckDrawHintBoard._instace.ToggleShow ();
		}
		// GameObject temp= GameObject.Instantiate(luckDrawHintBoardPrefab);
		//  temp.transform.SetParent(luckDrawBtn.transform);
		// temp.GetComponent<RectTransform>().localPosition=Vector3.zero;
	}

	public void ShowLuckDrawPanel ()
	{
		
		AudioManager._instance.PlayEffectClip (AudioManager.ui_click);

		//PrefabManager._instance.ShowNormalHintPanel ("抽奖功能将在后续版本中开放!");
		//return;
		GameObject.Instantiate (luckDrawPrefab);


		LuckDrawCanvasScr.Instance.UpdateData ();

	}

	public void ChatOpen ()
	{
		AudioManager._instance.PlayEffectClip (AudioManager.ui_click);

        string path1 = "Window/WindowTipsThree";
        GameObject WindowClone1 = AppControl.OpenWindow(path1);
        WindowClone1.SetActive(true);
        UITipAutoNoMask ClickTips2 = WindowClone1.GetComponent<UITipAutoNoMask>();
        ClickTips2.tipText.text = "暫未開放";

        return;

		string path = "Window/ChatPanel";
		GameObject WindowClone = AppControl.OpenWindow (path);
		WindowClone.SetActive (true);
	}

	public void ShowNoticeWindow ()
	{
		MyInfo myInfo = (MyInfo)Facade.GetFacade ().data.Get (FacadeConfig.USERINFO_MODULE_ID);
		if (GameController._instance.isExperienceMode) {
			HintTextPanel._instance.SetTextShow ("體驗場暫未開放公告");
			return;
		}
		if (myInfo.NoticeWindow == 0) {
			return;
		}
 
		AudioManager._instance.PlayEffectClip (AudioManager.ui_click);
		string path = "Window/NoticeWindow";
		GameObject WindowClone = AppControl.OpenWindow (path);
		WindowClone.SetActive (true);
	}

}
