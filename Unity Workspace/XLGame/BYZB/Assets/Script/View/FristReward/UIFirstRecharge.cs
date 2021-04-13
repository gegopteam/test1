using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using AssemblyCSharp;

public class UIFirstRecharge : MonoBehaviour
{
	public Button reciveBtn;
	Image buttonImg;
	static int state = 0;
	MyInfo myInfo;

	void Awake ()
	{
		myInfo = (MyInfo)Facade.GetFacade ().data.Get (FacadeConfig.USERINFO_MODULE_ID);
		buttonImg = reciveBtn.GetComponent<Image> ();
	}
	//外部设置按钮贴图与点击事件，0前往充值，1领取
	public static int SetState {
		set {
			state = value;
		}
	}

	void Start ()
	{
		UIColseManage.instance.ShowUI (this.gameObject);
		ChangeButton (state);
	}



	public void OnClickExitButton ()
	{
		AudioManager._instance.PlayEffectClip (AudioManager.effect_closePanel);
		Destroy (gameObject);
		UIColseManage.instance.CloseUI ();
	}

	void ChangeButton (int state)
	{
		buttonImg.sprite = UIHallTexturers.instans.FirstRecharge [state];
		switch (state) {
		case 0:
			reciveBtn.onClick.AddListener (GoRecharge);
			break;
		case 1:
			reciveBtn.onClick.AddListener (Recive);
			break;
		}
	}
	//前往充值点击事件
	void GoRecharge ()
	{
		Debug.LogError ("go charge");
		Destroy (gameObject);
		UIHallCore nHallCore = (UIHallCore)Facade.GetFacade ().ui.Get (FacadeConfig.UI_HALL_MODULE_ID);
		if (nHallCore != null) {
			nHallCore.ToStore ();
		} else {
			AudioManager._instance.PlayEffectClip (AudioManager.ui_click);
			if (myInfo.isGuestLogin) {
				StartGiftManager.GuestToStoreManager ();
				//return;
			}
			//先判断是否有没有首充过，如果首充过，则先弹出首充特惠的窗口，首充过则直接弹出商城的界面
            //string path = "Window/StoreWindow";//商店预制体名称更换
            string path = "Window/NewStoreCanvas";
			GameObject WindowClone = AppControl.OpenWindow (path);
			WindowClone.SetActive (true);
			WindowClone.GetComponent<UIStore> ().CoinButton ();
		}
	}

	//领取点击事件
	void Recive ()
	{
		Debug.LogError ("go Recive");
		Facade.GetFacade ().message.toolPruchase.SendGetPayRewardRequest ();
		Destroy (gameObject);
	}
}
