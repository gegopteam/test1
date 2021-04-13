using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System.Runtime.InteropServices;

public class UISetting : MonoBehaviour
{
	public static UISetting Instance;

	private AppControl appControl = null;
	[SerializeField]
	private Button backOpen;
	[SerializeField]
	private Button backClose;
	[SerializeField]
	private Button gameOpen;
	[SerializeField]
	private Button gameClose;
	[SerializeField]
	private Button gunOpen;
	[SerializeField]
	private Button gunClose;
	[SerializeField]
	private Button Switch;
	//切换账号
	[SerializeField]
	private Button Bind;
	//绑定手机
	[SerializeField]
	//private AudioSource myAudio;

	//客服熱線
	public Text phoneNumber;

	void Awake ()
	{
		Instance = this;
		appControl = AppControl.GetInstance ();
		backOpen.onClick.AddListener (BackGroundButtonClick);
		backClose.onClick.AddListener (BackGroundButtonClick);
		gameOpen.onClick.AddListener (gameSoundButtonClick);
		gameClose.onClick.AddListener (gameSoundButtonClick);
		gunOpen.onClick.AddListener (gunSoundButtonClick);
		gunClose.onClick.AddListener (gunSoundButtonClick);

		Switch.onClick.AddListener (Switchaccount);
		Bind.onClick.AddListener (ToBind);
		InitBtnState ();

		SaveBGMusicWithPlayer ();
		SaveGunMusicWithPlayer ();
	}

	void Start ()
	{
		//DontDestroyOnLoad (this.gameObject);
		UIColseManage.instance.ShowUI (this.gameObject);
		phoneNumber.text = "" + UIUpdate.WebUrlDic[WebUrlEnum.PhoneNumber];
	}

	/// <summary>
	/// 每次打开设置界面时判断是否为游客，如果不是游客，关闭绑定和切换按。
	/// </summary>
	private void OnEnable ()
	{
		//parent = transform.Find("root/Hall90Canvas").transform;
		MyInfo myInfo = DataControl.GetInstance ().GetMyInfo ();
		if (!myInfo.isGuestLogin) {
			Switch.gameObject.SetActive (false);
			Bind.gameObject.SetActive (false);
			Debug.Log ("按钮已关闭");
		} else {
			Debug.Log ("按钮已打开");
		}
	}

	/// <summary>
	/// 背景音乐控制
	/// </summary>
	void BackGroundButtonClick ()
	{
		if (backOpen.gameObject.activeInHierarchy) {
			//背景音乐切换到关闭状态的逻辑
			AudioManager._instance.PlayEffectClip (AudioManager.ui_click);
			backOpen.gameObject.SetActive (false);
			backClose.gameObject.SetActive (true);
			AudioManager._instance.StopBgm ();
			AudioManager._instance.useBgm = false;
			PlayerPrefsSetting ("backmusic", 0);
		} else {
			//背景音乐切换到打开状态
			AudioManager._instance.PlayEffectClip (AudioManager.ui_click);
			backClose.gameObject.SetActive (false);
			backOpen.gameObject.SetActive (true);
			AudioManager._instance.useBgm = true;
			AudioManager._instance.PlayBgm (AudioManager.bgm_hall);
			PlayerPrefsSetting ("backmusic", 1);
		}
	}

	/// <summary>
	/// 游戏音效控制
	/// </summary>
	void gameSoundButtonClick ()
	{
		if (gameOpen.gameObject.activeInHierarchy) {
			//游戏音效切换到关闭状态的逻辑
			AudioManager._instance.PlayEffectClip (AudioManager.ui_click);
			gameOpen.gameObject.SetActive (false);
			gameClose.gameObject.SetActive (true);
			AudioManager._instance.useEffectClip = false;
			PlayerPrefsSetting ("effectmusic", 0);
		} else {
			//游戏音效切换到打开状态
			gameClose.gameObject.SetActive (false);
			gameOpen.gameObject.SetActive (true);
			AudioManager._instance.useEffectClip = true;
			AudioManager._instance.PlayEffectClip (AudioManager.ui_click);
			PlayerPrefsSetting ("effectmusic", 1);
		}
	}

	/// <summary>
	/// 子弹音效控制
	/// </summary>
	void gunSoundButtonClick ()
	{
		if (gunOpen.gameObject.activeInHierarchy) {
			//开炮音效切换到关闭状态的逻辑
			AudioManager._instance.PlayEffectClip (AudioManager.ui_click);
			gunOpen.gameObject.SetActive (false);
			gunClose.gameObject.SetActive (true);
			AudioManager._instance.useFireSound = false;
			PlayerPrefsSetting ("gunmusic", 0);
		} else {
			//开炮音效切换到打开状态
			AudioManager._instance.PlayEffectClip (AudioManager.ui_click);
			gunClose.gameObject.SetActive (false);
			gunOpen.gameObject.SetActive (true);
			AudioManager._instance.useFireSound = true;
			PlayerPrefsSetting ("gunmusic", 1);
		}
	}

	public void OnButton ()
	{
		AudioManager._instance.PlayEffectClip (AudioManager.effect_closePanel);
		Destroy (this.gameObject);
		UIColseManage.instance.CloseUI ();
	}

	/// <summary>
	/// 继续游戏
	/// </summary>
	public void ContinueButton ()
	{
		AudioManager._instance.PlayEffectClip (AudioManager.ui_click);
		Destroy (this.gameObject);
		UIColseManage.instance.CloseUI ();
	}

	/// <summary>
	/// 去绑定手机
	/// </summary>
	public void ToBind ()
	{
		AudioManager._instance.PlayEffectClip (AudioManager.ui_click);

		BindWindowCtrl.Instense.GenerateWindow ();
		//string path = "Window/BindPhoneNumber";
		//GameObject WindowClone = AppControl.OpenWindow(path);
		//WindowClone.SetActive(true);
		Destroy (this.gameObject);
	}

	/// <summary>
	/// 切换账号
	/// </summary>
	public void Switchaccount ()
	{
		AudioManager._instance.PlayEffectClip (AudioManager.ui_click);
		//弹出警告弹窗
		GameObject SwitchaccountWarning = Instantiate (Resources.Load<GameObject> ("Window/SwitchaccountWarning"));
		//弹窗确定按钮
		SwitchaccountWarning.transform.GetChild (1).Find ("BtnSure").GetComponent<Button> ().onClick.AddListener (() => {
			Destroy (SwitchaccountWarning);//关闭警告弹窗

			AudioManager._instance.PlayEffectClip (AudioManager.ui_click);
			TaskMsgHandle.DestoryList ();

			LoginUtil.GetIntance ().CancelAuthorize ();

			#if UNITY_IPHONE && !UNITY_EDITOR
			ToSetGuestLogin ();
			//CleanGuestLoginCache();
			#elif UNITY_ANDROID  && !UNITY_EDITOR
			UpdateGuestLoginState ();
			#endif


			AppControl.ToView (AppView.LOGIN);
			Destroy (this.gameObject);
			ChangeState ();
			UIColseManage.instance.CloseAll ();
			
		});
		SwitchaccountWarning.transform.GetChild (1).Find ("BtnEsc").GetComponent<Button> ().onClick.AddListener (() => {
			Destroy (SwitchaccountWarning);//关闭警告弹窗
		});
		SwitchaccountWarning.transform.GetChild (1).Find ("BtnExit").GetComponent<Button> ().onClick.AddListener (() => {
			Destroy (SwitchaccountWarning);//关闭警告弹窗
		});

	}


	/// <summary>
	/// android更改游客状态
	/// </summary>
	void UpdateGuestLoginState ()
	{
		AndroidJavaClass jc = new AndroidJavaClass ("com.unity3d.player.UnityPlayer");
		AndroidJavaObject jo = jc.GetStatic<AndroidJavaObject> ("currentActivity");
		jo.Call ("setGuestLogin");
	}

	/// <summary>
	/// 清除安卓游客登录缓存
	/// </summary>
	void CleanGuestLoginCache ()
	{
		AndroidJavaClass jc = new AndroidJavaClass ("com.unity3d.player.UnityPlayer");
		AndroidJavaObject jo = jc.GetStatic<AndroidJavaObject> ("currentActivity");
		jo.Call ("CleanGuestLogin");
		//AppInfo.GuestDeviceID = "";
	}

#if UNITY_IOS
	/// <summary>
	/// ios更改游客状态
	/// </summary>
	/// <returns><c>true</c>, if set guest login was toed, <c>false</c> otherwise.</returns>
	[ DllImport ("__Internal")]
	public static extern bool ToSetGuestLogin ();
#endif
	/// <summary>
	/// 注销登录
	/// </summary>
	public void LogOff ()
	{
        MyInfo myInfo = DataControl.GetInstance().GetMyInfo();
        myInfo.SetignStatue(0);
        myInfo.account = null;
        myInfo.misHaveDraCard = false;
        myInfo.isShowNumner = 0;
        myInfo.isShowAwesome = false;
        myInfo.isShowDouble = false;
        myInfo.isShowTreasure = false;
		myInfo.connectServerAlr = false;
		myInfo.isconnecting = false;
		


		myInfo.Pay_Drang_RMB = new ArrayList();
		myInfo.Pay_Drang_AddGold = new ArrayList();
		myInfo.Pay_Drang_id = new ArrayList();
		//特惠付款、金幣資訊
		myInfo.Pay_Preferential_RMB = new ArrayList();
		myInfo.Pay_Preferential_AddGold = new ArrayList();
		myInfo.Pay_Preferential_id = new ArrayList();
		//三選一 發現寶藏 付款、金幣資訊
		myInfo.Pay_Three_RMB = new ArrayList();
		myInfo.Pay_Three_AddGold = new ArrayList();
		myInfo.Pay_Three_id = new ArrayList();
		//二選一 雙喜臨門 付款、金幣資訊
		myInfo.Pay_Two_RMB = new ArrayList();
		myInfo.Pay_Two_AddGold = new ArrayList();
		myInfo.Pay_Two_id = new ArrayList();
		//新手七日付款、金幣資訊
		myInfo.Pay_NewSeven_RMB = new ArrayList();
		myInfo.Pay_NewSeven_AddGold = new ArrayList();
		myInfo.Pay_NewSeven_id = new ArrayList();

		myInfo.Pay_Drang_RMB.Clear();
		myInfo.Pay_Drang_AddGold.Clear();
		myInfo.Pay_Drang_id.Clear();
		//特惠付款、金幣資訊
		myInfo.Pay_Preferential_RMB.Clear();
		myInfo.Pay_Preferential_AddGold.Clear();
		myInfo.Pay_Preferential_id.Clear();
		//三選一 發現寶藏 付款、金幣資訊
		myInfo.Pay_Three_RMB.Clear();
		myInfo.Pay_Three_AddGold.Clear();
		myInfo.Pay_Three_id.Clear();
		//二選一 雙喜臨門 付款、金幣資訊
		myInfo.Pay_Two_RMB.Clear();
		myInfo.Pay_Two_AddGold.Clear();
		myInfo.Pay_Two_id.Clear();
		//新手七日付款、金幣資訊
		myInfo.Pay_NewSeven_RMB.Clear();
		myInfo.Pay_NewSeven_AddGold.Clear();
		myInfo.Pay_NewSeven_id.Clear();
		//商城付款資訊
		myInfo.Pay_Store_RMB.Clear();
		myInfo.Pay_Store_AddGold.Clear();
		myInfo.Pay_Store_id.Clear();

		ReSignDay.isFristActive = true;
        ReSignDay.isContinueSign = false;
        ReSignDay.continuedday = 0;
        AudioManager._instance.PlayEffectClip(AudioManager.ui_click);
        TaskMsgHandle.DestoryList();
		LoginUtil.GetIntance().ForStopIE();
		LoginUtil.GetIntance().CancelAuthorize();
        LoginUtil.GetIntance().Ishare = false;
        LoginUtil.GetIntance().IsNoteQqorWet = true;
        AppInfo.isFritsInGame = true;
        if (FileUtilManager.IsExistsFile(LoginInfo.pathUrl, "Login.txt"))
        {
			Debug.Log(" 刪除登入訊息 ");
            FileUtilManager.DeleteFile(LoginInfo.pathUrl, "Login.txt");
            LoginInfo.isQQLogin = false;
            LoginInfo.isWechatLogin = false;
            LoginInfo.isAccoutLogin = false;
            LoginInfo.accountStr = "";
            LoginInfo.passwordStr = "";
        }
        
		//進入大廳走彈框流程
		UIHallCore.isFristNewSevenDay = true;
		UIHallCore.isFristActivity = true;
		UIHallCore.isFristPreferential = true;
		UIHallCore.isFristBigWineer = true;
		UIHallCore.isFristSign = true;

		FangChenMi._instance.StopTimer();
        AppControl.ToView(AppView.LOGIN);
        ChangeState();
        UIColseManage.instance.CloseAll();

#if UNITY_Huawei
                //Huawei退出登录
                OnOPPOExitGameClick();
#endif
#if UNITY_OPPO
                //oppo退出登录
                OnOPPOExitGameClick();
#endif
#if UNITY_VIVO
                //vivo退出登录
                OnVIVOExitGameClick();
#endif
	}

	/// <summary>
	/// 注销登录后全局变量设置为false
	/// </summary>
	void ChangeState ()
	{
		AppInfo.isReciveHelpMsg = false;
//		UsePropTool.isFirstUsePro = false;
		if (UsePropTool.Instance != null) {
			UsePropTool.Instance.proTimeRequest.Clear ();
			UsePropTool.Instance.toolpropArr.allProp.Clear ();
		}
	}


    public void isShow()
    {
        TimeCount.isShow_1 = false;
        TimeCount.isShow_2 = false;
        TimeCount.isShow_3 = false;
        TimeCount.isGiftBag_1 = false;
        TimeCount.isGiftBag_2 = false;
        TimeCount.isGiftBag_3 = false;
    }
    void InitBtnState ()
	{
		backOpen.gameObject.SetActive (AudioManager._instance.useBgm);
		backClose.gameObject.SetActive (!AudioManager._instance.useBgm);

		gameOpen.gameObject.SetActive (AudioManager._instance.useEffectClip);
		gameClose.gameObject.SetActive (!AudioManager._instance.useEffectClip);

		gunOpen.gameObject.SetActive (AudioManager._instance.useFireSound);
		gunClose.gameObject.SetActive (!AudioManager._instance.useFireSound);
	}

	void SaveBGMusicWithPlayer ()
	{
		if (!PlayerPrefs.HasKey ("backmusic")) {
			PlayerPrefsSetting ("backmusic", 1);
			backOpen.gameObject.SetActive (true);
			backClose.gameObject.SetActive (false);
			PlayerPrefs.Save ();
			
		} else {
			if (PlayerPrefs.GetInt ("backmusic") == 0) {
				backOpen.gameObject.SetActive (false);
				backClose.gameObject.SetActive (true);
			} else {
				backOpen.gameObject.SetActive (true);
				backClose.gameObject.SetActive (false);
				AudioManager._instance.useBgm = true;
			}
		}
	}

	void SaveGunMusicWithPlayer ()
	{
		if (!PlayerPrefs.HasKey ("gunmusic")) {
			PlayerPrefsSetting ("gunmusic", 1);
			gunOpen.gameObject.SetActive (true);
			gunClose.gameObject.SetActive (false);
			PlayerPrefs.Save ();
		} else {
			if (PlayerPrefs.GetInt ("gunmusic") == 0) {
				gunOpen.gameObject.SetActive (false);
				gunClose.gameObject.SetActive (true);
			} else {
				gunOpen.gameObject.SetActive (true);
				gunClose.gameObject.SetActive (false);
			}
		}
	}




	void SaveEffectMusicWithPlayer ()
	{
		if (!PlayerPrefs.HasKey ("effectmusic")) {
			PlayerPrefsSetting ("effectmusic", 1);
			gameOpen.gameObject.SetActive (true);
			gameClose.gameObject.SetActive (false);
			PlayerPrefs.Save ();
		} else {
			if (PlayerPrefs.GetInt ("effectmusic") == 0) {
				gameOpen.gameObject.SetActive (false);
				gameClose.gameObject.SetActive (true);
			} else {
				gameOpen.gameObject.SetActive (true);
				gameClose.gameObject.SetActive (false);
			}
		}
	}

	void PlayerPrefsSetting (string name, int type)
	{
		PlayerPrefs.SetInt (name, type);
	}

    /// <summary>
    /// OPPO退出游戏
    /// </summary>
    public void OnOPPOExitGameClick()
    {
        AndroidJavaClass jc = new AndroidJavaClass("com.unity3d.player.UnityPlayer");
        AndroidJavaObject jo = jc.GetStatic<AndroidJavaObject>("currentActivity");
        jo.Call("OnExitGmae");
    }

    /// <summary>
    /// VIVO退出游戏
    /// </summary>
    public void OnVIVOExitGameClick()
    {
        AndroidJavaClass jc = new AndroidJavaClass("com.unity3d.player.UnityPlayer");
        AndroidJavaObject jo = jc.GetStatic<AndroidJavaObject>("currentActivity");
        jo.Call("ExieGame");
    }

}
