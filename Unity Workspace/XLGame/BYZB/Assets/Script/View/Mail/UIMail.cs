using UnityEngine;
using System.Collections.Generic;
using UnityEngine.UI;
using AssemblyCSharp;

/// <summary>
/// User interface mail.邮件系统的整体管理
/// </summary>

public class UIMail : MonoBehaviour , IUiMediator {
	
	public GameObject systemMail;
	public GameObject giveMail;
	public Image systemImage;
	public Image giveImage;
	public BackageUI systemUI;
	public BackageUI giveUI;
	private MailDataInfo nInfo;
	private int mSelectedTap = MailInfo.SYSTEM_MAIL;
	public static UIMail instans;
	private GameObject reward;
	private Camera rewardCamera;
	private Canvas mainCanvas;
	public Image OneKey;
	public GameObject systemPoint;
	public GameObject givePoint;
	public Image systemFont;
	public Image giveFont;
	public Animator ani;


	void Awake()
	{
		instans = this;
//		Debug.LogError ( "---------------mail awake--------------" );
	}

	void Start()
	{

		reward = GameObject.FindGameObjectWithTag ("MainCamera");
		Debug.Log (reward.name);
		rewardCamera = reward.GetComponent<Camera> ();
		mainCanvas = transform.GetComponentInChildren<Canvas> ();
		mainCanvas.worldCamera = rewardCamera;

		systemImage.sprite = UIHallTexturers.instans.Mail[6];
		systemFont.sprite=UIHallTexturers.instans.Mail[8];
		giveImage.sprite = UIHallTexturers.instans.Mail[7];
		giveFont.sprite=UIHallTexturers.instans.Mail[11];
		giveImage.color = Color.clear;
		systemMail.SetActive (true);
		giveMail.SetActive (false);
		mSelectedTap = MailInfo.SYSTEM_MAIL;
		Facade.GetFacade ().ui.Add ( FacadeConfig.UI_MAIL_MODULE_ID , this );
		UIColseManage.instance.ShowUI (this.gameObject);
	}

	//在初始化的时候，我们从本地数据层获取邮件，赠送记录等的数据
	public  void OnInit()
	{
		nInfo = (MailDataInfo)Facade.GetFacade ().data.Get ( FacadeConfig.MAIL_MODULE_ID );
		if (nInfo != null) {
			SystemManger nMailManager = systemMail.transform.GetComponentInChildren< SystemManger > ();
			nMailManager.ClearView ();
			FriendGiveManager nGiveManager = giveMail.transform.GetComponentInChildren<FriendGiveManager>( );
			nGiveManager.ClearView ();
			RefreshSystemUI ();
		}
		RefreshGivePoint ();
	}
	//刷新ui界面，减少刷新次数。数据存储在MailDataInfo的list中。
	public void RefreshSystemUI(){
		SystemManger nMailManager = systemMail.transform.GetComponentInChildren< SystemManger > ();


		//nInfo = (MailDataInfo)Facade.GetFacade ().data.Get (FacadeConfig.MAIL_MODULE_ID);
		systemUI.cellNumber = nInfo.getMailList ().Count;
		systemUI.Refresh ();
		if (nMailManager != null && systemUI.cellNumber > 0)
			nMailManager.AppendMail (null);
		if (systemUI.cellNumber == 0) {
			OneKey.sprite = UIHallTexturers.instans.Mail[1];
			OneKey.gameObject.GetComponent<Button> ().interactable = false;
		} else {
			OneKey.sprite = UIHallTexturers.instans.Mail[0];
			OneKey.gameObject.GetComponent<Button> ().interactable = true;
		}
		RefreshSystemPoint ();
	}
	public void RefreshSystemPoint(){
		if (nInfo.getMailList().Count>0)
			systemPoint.SetActive (true);
		else
			systemPoint.SetActive (false);
	}

	//收到服务器邮件消息后，更新数据，如果id已经存在，那么不处理
	public  void OnRecvData( int nType , object nData )
	{
	}

	private void OnClickExitButton()
	{
		
		//transform.gameObject.SetActive (false);
		UIColseManage.instance.CloseUI ();
	}

	//退出按钮操作
	public void OnButton()
	{
		AudioManager._instance.PlayEffectClip (AudioManager.effect_closePanel);
		ani.SetBool ("isClose", true);
		transform.GetChild (0).gameObject.SetActive (false);
		Invoke ("OnClickExitButton",0.3f); 
	}

	//点击邮件系统按钮，显示邮件界面的信息
	public void SystemButton()
	{
		//赠送第一个显示为已读,将自己的第一条信息在面板显示
//		Facade.GetFacade().message.mail.SendGetSystemMailRequest();
		MailDataInfo nInfo = (MailDataInfo)Facade.GetFacade ().data.Get ( FacadeConfig.MAIL_MODULE_ID );
		if( nInfo.getMailList () .Count==0)
			AudioManager._instance.PlayEffectClip (AudioManager.ui_click);
		mSelectedTap = MailInfo.SYSTEM_MAIL;
		systemImage.sprite = UIHallTexturers.instans.Mail[6];
		systemImage.color = Color.white;
		systemFont.sprite=UIHallTexturers.instans.Mail[8];
		giveImage.sprite = UIHallTexturers.instans.Mail[7];
		giveFont.sprite=UIHallTexturers.instans.Mail[11];
		giveImage.color = Color.clear;
		systemMail.SetActive (true);
		giveMail.SetActive (false);
		//更新邮件信息面板
		SystemManger nMailManager = systemMail.transform.GetComponentInChildren< SystemManger > ();
		if (nMailManager != null)
			nMailManager.OnSwitch ();
		
		RefreshSystemUI ();


	}

	//点击赠送记录，显示赠送记录的信息
	public void GiveButton()
	{
		//邮件第一个显示为已读，将自己的第一条信息在面板显示
//		Facade.GetFacade().message.mail.SendGetPersentRecordRequest();
		MailDataInfo nInfo = (MailDataInfo)Facade.GetFacade ().data.Get ( FacadeConfig.MAIL_MODULE_ID );
		if( nInfo.getRecordList () .Count==0)
			AudioManager._instance.PlayEffectClip (AudioManager.ui_click);
		mSelectedTap = MailInfo.PRESENT;
		systemImage.sprite =UIHallTexturers.instans.Mail[7];
		systemImage.color = Color.clear;
		systemFont.sprite=UIHallTexturers.instans.Mail[9];
		giveImage.sprite = UIHallTexturers.instans.Mail[6];
		giveFont.sprite=UIHallTexturers.instans.Mail[10];
		giveImage.color = Color.white;
		systemMail.SetActive (false);
		giveMail.SetActive (true);
		//更新赠送记录信息面板
		FriendGiveManager nGiveManager = giveMail.transform.GetComponentInChildren<FriendGiveManager>( );
		if (nGiveManager != null)
			nGiveManager.OnSwitch ();
		
		RefreshGiveUI ();

	}
	//刷新赠送道具ui
	public void RefreshGiveUI(){
		FriendGiveManager nGiveManager = giveMail.transform.GetComponentInChildren<FriendGiveManager>( );


		//nInfo = (MailDataInfo)Facade.GetFacade ().data.Get ( FacadeConfig.MAIL_MODULE_ID );
		giveUI.cellNumber = nInfo.getRecordList().Count;
		Debug.LogError ("赠送列表长度："+nInfo.getRecordList().Count);
		giveUI.Refresh ();
		if (nGiveManager != null&&giveUI.cellNumber>0)
			nGiveManager.AppenGiveMail ( null );
		if (giveUI.cellNumber == 0) {
			OneKey.sprite = UIHallTexturers.instans.Mail[1];
			OneKey.gameObject.GetComponent<Button> ().interactable = false;
		} else {
			OneKey.sprite = UIHallTexturers.instans.Mail[0];
			OneKey.gameObject.GetComponent<Button> ().interactable = true;
		}
		RefreshGivePoint ();
	}
	public void RefreshGivePoint(){
		if (nInfo.getRecordList().Count>0)
			givePoint.SetActive (true);
		else
			givePoint.SetActive (false);
	}

	//一键领取
	public void OneKeyUseBUtton()
	{
		//点击了一键领取，首先我要获得当前邮件所有的奖励
		//在获取List消息的时候需要读取所有邮件中的奖励，并存放在一个list中或者字典中
		MailDataInfo nInfo = (MailDataInfo)Facade.GetFacade ().data.Get ( FacadeConfig.MAIL_MODULE_ID );
		if (mSelectedTap == MailInfo.SYSTEM_MAIL) {
			Debug.Log(" ----- UIMail ----- OneKeyUseBUtton ----- nInfo.getAllMailId = "+ nInfo.getAllMailId());
			Facade.GetFacade ().message.mail.SendDeleteSystemMailRequest ( nInfo.getAllMailId () );
			RefreshSystemUI ();
			SystemManger.instans.ClearView ();
			SystemManger.instans.mLastSelected = null;
		} else {
			Debug.Log(" ----- UIMail ----- OneKeyUseBUtton ----- nInfo.getAllRecordId = " + nInfo.getAllRecordId());
			Facade.GetFacade ().message.mail.SendAcceptPersentRequest ( nInfo.getAllRecordId () );
			RefreshGiveUI ();
			FriendGiveManager.instans.ClearView ();
			FriendGiveManager.instans.mLastSelected = null;
		}
			AudioManager._instance.PlayEffectClip (AudioManager.ui_click);

	}

	public void OnRelease()
	{

	}

	void OnDestroy()
	{
		//Debug.LogError ( "--------------OnDestroy----------------" );
		Facade.GetFacade ().ui.Remove( FacadeConfig.UI_MAIL_MODULE_ID );
	}

}
