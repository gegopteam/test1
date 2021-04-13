using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using AssemblyCSharp;

public class UIAssociateAccount : MonoBehaviour, IMsgHandle
{

	public GameObject[] Associate;
	public Text[] aToken;
	public Image[] aType;
	public Text[] aGid;
	public Sprite[] AccountType;

	MyInfo myInfo;
	GameObject Window1;
	GameObject mWaitingView;
	public static UIAssociateAccount Instance;

	void Awake()
	{
		if (GameController._instance == null) //渔场和大厅里赋值的摄像机不同
			gameObject.GetComponent<Canvas>().worldCamera = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<Camera>();
		else
			gameObject.GetComponent<Canvas>().worldCamera = ScreenManager.uiCamera;

		Instance = this;
		UIColseManage.instance.ShowUI(this.gameObject);
		Window1 = UnityEngine.Resources.Load("Window/WindowTipsThree") as UnityEngine.GameObject;
		myInfo = (MyInfo)Facade.GetFacade().data.Get(FacadeConfig.USERINFO_MODULE_ID);
	}

	// Use this for initialization
	void Start () {
		//初始化帳號類型
		for (int associate_count=0; associate_count < myInfo.Associate_Type.Count; associate_count++)
        {
			Associate[associate_count].SetActive(true);
			aToken[associate_count].text = "" + myInfo.Associate_Name[associate_count];
			aGid[associate_count].text = "暱稱 : " + myInfo.Associate_Nickname[associate_count];
			if (myInfo.Associate_Type[associate_count].ToString().Equals("0"))
			{
                //帳號登入
				aType[associate_count].sprite = AccountType[0];
			}
			else if (myInfo.Associate_Type[associate_count].ToString().Equals("1"))
			{
                //微信帳號
				aType[associate_count].sprite = AccountType[1];
			}
			else if (myInfo.Associate_Type[associate_count].ToString().Equals("2"))
			{
                //QQ帳號
				aType[associate_count].sprite = AccountType[2];
			}
		}
		OnInit();
	}

	public void OnInit()
	{
		Debug.Log(" UICheckPhoneNumber OnInit ");
		EventControl nControl = EventControl.instance();
		nControl.addEventHandler(FiProtoType.XL_CHOISE_ACCOUNT_LOGIN_RESPON, RecvBackAssociateToken);
	}

	// Update is called once per frame
	void Update () {
		
	}

	private void RecvBackAssociateToken(object data)
	{
		ShowWaitingView(false);
		LoginAccountAssociateChoise info = (LoginAccountAssociateChoise)data;
//#if UNITY_EDITOR
        Debug.Log(" ========== RecvBackAssociateToken ========== result = " + info.result);
        Debug.Log(" ========== RecvBackAssociateToken ========== accountType = " + info.accountType);
        Debug.Log(" ========== RecvBackAssociateToken ========== user_id = " + info.user_id);
        Debug.Log(" ========== RecvBackAssociateToken ========== accountName = " + info.accountName);
        Debug.Log(" ========== RecvBackAssociateToken ========== strToken = " + info.strToken);
		Debug.Log(" ========== RecvBackAssociateToken ========== nickName = " + info.nickname);
		//#endif

		if (info.result == 0)
		{
			//ShowAuthMsg("token取得成功");
			if (!string.IsNullOrEmpty(info.strToken)) {
				myInfo.acessToken = info.strToken;
				myInfo.openId = info.user_id.ToString();
				myInfo.userID = info.user_id;
				myInfo.nickname = info.nickname;
				myInfo.isPhoneNumberLogin = true;

				if (info.accountType == 1)
				{
					myInfo.platformType = 22;
				}
				else if (info.accountType == 2)
				{
					myInfo.platformType = 24;
				}
				else
				{
					myInfo.platformType = 0;
				}

//#if UNITY_EDITOR
				Debug.Log(" ========== RecvBackAssociateToken ========== platformType = " + myInfo.platformType);
				Debug.Log(" ========== RecvBackAssociateToken ========== isPhoneNumberLogin = " + myInfo.isPhoneNumberLogin);
				Debug.Log(" ========== RecvBackAssociateToken ========== user_id = " + myInfo.openId);
				Debug.Log(" ========== RecvBackAssociateToken ========== accountName = " + myInfo.nickname);
				Debug.Log(" ========== RecvBackAssociateToken ========== strToken = " + myInfo.acessToken);
//#endif
				Facade.GetFacade().message.login.SendLoginRequest();
				ShowWaitingView(true);
			}
		}
		else 
		{
			ShowAuthMsg("登入失敗");
		}
	}

	public void OnGetLoginToken(int Ass_index)
	{
		ShowWaitingView(true);
		myInfo.associateIndex = Ass_index;
		AudioManager._instance.PlayEffectClip(AudioManager.ui_click);
#if UNITY_EDITOR
		Debug.Log("OnGetLoginToken Associate_Type = " + myInfo.Associate_Type[Ass_index]);
		Debug.Log("OnGetLoginToken result = " + myInfo.Associate_Userid[Ass_index]);
		Debug.Log("OnGetLoginToken result = " + myInfo.Associate_Name[Ass_index]);
		Debug.Log("OnGetLoginToken result = " + myInfo.Associate_Token[Ass_index]);
#endif
		
		if ((int)myInfo.Associate_Type[Ass_index] == 1)
		{
			myInfo.platformType = 22;
		}
		else if ((int)myInfo.Associate_Type[Ass_index] == 2)
		{
			myInfo.platformType = 24;
		}
		else
		{
			myInfo.platformType = 0;
		}
		LoginAccountAssociateChoise nRequest = new LoginAccountAssociateChoise();
		nRequest.result = 0;
		nRequest.accountType = (int)myInfo.Associate_Type[Ass_index];
		nRequest.user_id = (int)myInfo.Associate_Userid[Ass_index];
		nRequest.accountName = "" + myInfo.Associate_Name[Ass_index];
		nRequest.strToken = "" + myInfo.Associate_Token[Ass_index];
		myInfo.nickname =""+ myInfo.Associate_Nickname[Ass_index];
		Debug.Log("OnGetLoginToken Associate_Type = " + nRequest.accountType);
		Debug.Log("OnGetLoginToken result = " + nRequest.result);
		Debug.Log("OnGetLoginToken user_id = " + nRequest.user_id);
		Debug.Log("OnGetLoginToken accountName = " + nRequest.accountName);
		Debug.Log("OnGetLoginToken strToken = " + nRequest.strToken);
		DataControl.GetInstance().PushSocketSnd(FiEventType.XL_GET_ASSOCIATE_ACC_LOGIN_REQUEST, nRequest);
		Debug.Log("已發送！");
	}

	/// <summary>
	/// 顯示提示
	/// </summary>
	/// <param name="msg">顯示訊息</param>
	public void ShowAuthMsg(string msg)
	{
		GameObject WindowClone1 = Instantiate(Window1);
		UITipAutoNoMask ClickTips1 = WindowClone1.GetComponent<UITipAutoNoMask>();
		ClickTips1.tipText.text = msg;
	}

	/// <summary>
	/// 顯示等待
	/// </summary>
	public void ShowWaitingView(bool nValue)
	{
		if (nValue)
		{
			mWaitingView = Instantiate(Resources.Load<GameObject>("MainHall/Common/WaitingView"));
			UIWaiting nData = mWaitingView.GetComponentInChildren<UIWaiting>();
			nData.HideBackGround();
			nData.SetInfo(10.0f, "請重新嘗試");
		}
		else
		{
			if (mWaitingView != null && mWaitingView.activeSelf)
			{
				Destroy(mWaitingView);
			}
			mWaitingView = null;
		}
	}

	public void OnExit()
	{
		//gameObject.SetActive(false);
		OnDestroy();
		UIColseManage.instance.CloseUI();
	}

	public void OnDisplay()
	{
		gameObject.SetActive(true);
	}

	public void OnDestroy()
	{
		EventControl nControl = EventControl.instance();
		nControl.removeEventHandler(FiProtoType.XL_CHOISE_ACCOUNT_LOGIN_RESPON, RecvBackAssociateToken);
	}
}
