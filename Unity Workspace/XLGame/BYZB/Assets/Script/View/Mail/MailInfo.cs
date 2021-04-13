using UnityEngine;
using System.Collections.Generic;
using UnityEngine.UI;
using AssemblyCSharp;
/// <summary>
/// Mail info.获取服务器数据并将其显示
/// </summary>

public class MailInfo : MonoBehaviour {

	public const int SYSTEM_MAIL = 0;

	public const int PRESENT = 1;

	[SerializeField]
	private Text sendName;
	[SerializeField]
	private Text sendTime;
	[SerializeField]
	private Text sendContext;
	//获取到按钮的消息
	public Image buttonImage;

	private long mCurrentMailId = 0;

	private int mMailType = 0;
	public static MailInfo instans;
	public long mailId
	{
		get { return mCurrentMailId; }
	}

	// Use this for initialization
	void Awake () {
		sendName = transform.Find ("SendName").Find ("Text").GetComponent<Text> ();
		sendTime = transform.Find ("Time").Find ("Text").GetComponent<Text> ();
		sendContext = transform.Find ("ContentText").Find ("Text").GetComponent<Text> ();
		instans = this;
	}

	public void SetUserName( string nUserName )
	{
		sendName.text =Tool.GetName( nUserName,6);
	}

	public void SetMailId( long nMailId , int nType )
	{
		mCurrentMailId = nMailId;
		mMailType = nType;
	}

	public void SetContent( string nContent )
	{
		sendContext.text = nContent;
	}

	public void SetDate( string nDateValue )
	{
		sendTime.text = nDateValue;
	}

	public void HideButton( bool bHide )
	{
		buttonImage.gameObject.SetActive ( !bHide );
	}

	public void SetDeleteButton()
	{
		buttonImage.sprite = UIHallTexturers.instans.Mail[2];
	}

	public void SetRecvButton()
	{
		buttonImage.sprite =UIHallTexturers.instans.Mail[5];
	}

	private void DeleteSystemMail( long nId )
	{
		SystemManger nSysMail = transform.parent.transform.GetComponentInChildren<SystemManger> ();
		if (nSysMail != null) {
			nSysMail.DeleteMail ( nId );
		}
	}

	private void DeleteRecord( long nId )
	{
		FriendGiveManager nGiveMail = transform.parent.transform.GetComponentInChildren<FriendGiveManager> ();
		if (nGiveMail != null) {
			nGiveMail.DeleteGive ( nId );
		}
	}

	public void ClickInfoButton(Image objImage)
	{
		Debug.LogError ( "on button clicked-----------" + mCurrentMailId );
		//赠送记录肯定有物品所以没有删除键
		List<long> nMailIds = new List<long> ();
		nMailIds.Add ( mCurrentMailId );
		MailDataInfo nInfo = (MailDataInfo)Facade.GetFacade ().data.Get ( FacadeConfig.MAIL_MODULE_ID );
		AudioManager._instance.PlayEffectClip (AudioManager.ui_click);
		if (mMailType == SYSTEM_MAIL) {
			//DeleteSystemMail ( mCurrentMailId );
			Facade.GetFacade ().message.mail.SendDeleteSystemMailRequest (nMailIds);
			SystemManger.instans.lastNumber = -1;
			SystemManger.instans.mLastSelected = null;
			UIMail.instans.RefreshSystemUI ();
			if(nInfo.getMailList().Count==0)
				SystemManger.instans.ClearView();
		} else {
			//DeleteRecord ( mCurrentMailId );
			Facade.GetFacade ().message.mail.SendAcceptPersentRequest(nMailIds);
			FriendGiveManager.instans.lastNumber = -1;
			FriendGiveManager.instans.mLastSelected = null;
			UIMail.instans.RefreshGiveUI ();
			if(nInfo.getRecordList().Count==0)
				FriendGiveManager.instans.ClearView();
		}



		buttonImage.gameObject.GetComponentInChildren<Button> ().interactable = false;
		Invoke ( "ActiveButton" , 0.5f );
	}

	void ActiveButton()
	{
		buttonImage.gameObject.GetComponentInChildren<Button> ().interactable = true;
	}

	// Update is called once per frame
	void Update () {
	
	}

	void OnDestroy()
	{
		//UIHall.InitMailEvent -= InitFristInfo;
	}
}
