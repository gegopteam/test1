using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using AssemblyCSharp;
using System.Runtime.InteropServices;
using System;

public class UIPhoneNumberLogin : MonoBehaviour
{

	private GameObject WindowClone;
	public GameObject InputPhone;
	public GameObject LoginWay;
	public GameObject LoginError;
	private Button QQLogin;
	private Button WechatLogin;
	public Text CustomerPhoneNumber;

	public static UIPhoneNumberLogin Instance;

	void Awake()
	{
		if (GameController._instance == null) //渔场和大厅里赋值的摄像机不同
			gameObject.GetComponent<Canvas>().worldCamera = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<Camera>();
		else
			gameObject.GetComponent<Canvas>().worldCamera = ScreenManager.uiCamera;
		UIColseManage.instance.ShowUI(this.gameObject);
		Instance = this;
	}

	// Use this for initialization
	void Start()
	{
		QQLogin = transform.Find("Frame/ChooseLogin/BtnQQLogin").GetComponent<Button>();
		QQLogin.onClick.AddListener(QQButton);
		WechatLogin = transform.Find("Frame/ChooseLogin/BtnWechatLogin").GetComponent<Button>();
		WechatLogin.onClick.AddListener(WechatButton);
		CustomerPhoneNumber.text = "客服電話：" + UIUpdate.WebUrlDic[WebUrlEnum.PhoneNumber];
		//DontDestroyOnLoad (WindowClone);
	}

	// Update is called once per frame
	void Update()
	{

	}

	void ShowErrorMsg() {
		LoginWay.SetActive(false);
		LoginError.SetActive(true);
	}

	public void OnExit()
	{
		gameObject.SetActive(false);
	}

	public void OnOpen()
	{
		gameObject.SetActive(true);
		//transform.parent.FindChild ("AccountLoginFrame").gameObject.SetActive ( true );
	}

	void QQButton()
	{
		Debug.Log(" QQButton ");
		//UILogin.Instance.OnQQLogin();
		ShowErrorMsg();
	}

	void WechatButton()
	{
		Debug.Log(" WechatButton ");
		//UILogin.Instance.ToWeChatLogin();
		ShowErrorMsg();
	}

	public void OnOtherButton()
	{
		gameObject.SetActive(false);
		transform.parent.Find("OtherLoginFrame").gameObject.SetActive(true);
	}

	public void OnOpenInputPhone()
    {
		BtnloseUI();
		GameObject window = Resources.Load("Window/InputPhoneNubmer") as GameObject;
		GameObject.Instantiate(window);

		//InputPhone.gameObject.SetActive(true);
		//this.gameObject.SetActive(false);
	}

	void BtnloseUI()
	{
		Debug.LogError("Btnlose");
		UIColseManage.instance.CloseUI();
	}
}
