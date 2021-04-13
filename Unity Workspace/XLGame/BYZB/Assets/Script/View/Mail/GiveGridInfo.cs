using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using AssemblyCSharp;
using System;
/// <summary>
/// Give grid info.好友赠送邮件内容初始化
/// </summary>

public class GiveGridInfo : ScrollableCell,IGrid {
	public Image headImage;
	public Text nickName;
	public Text date;
	public Text time;
	public long id;

	public int userId;
	Sprite tempHead;
	private string avatarUrl = "";
	private FiPresentRecord data;
	void Awake(){
		tempHead = headImage.sprite;
	}
	public void SetUrl( int nUserId ,  string nAvatar )
	{
		//Debug.LogError ( "set avatar----------" );
		userId = nUserId;
		avatarUrl = nAvatar;
		AvatarInfo nInfo =(AvatarInfo) Facade.GetFacade ().data.Get ( FacadeConfig.AVARTAR_MODULE_ID );
		nInfo.Load ( userId , nAvatar , OnAvatarCompelete );
	}

	void OnAvatarCompelete( int nResult , Texture2D nImage )
	{
		if (nResult == 0) {
			GetMyHead (nImage);
		} else
			headImage.sprite = tempHead;
	}
		

	//获取头像
	public void GetMyHead(Texture2D image)
	{
		//Debug.LogError ( "set GetMyHead----------" );
		image.filterMode = FilterMode.Bilinear;
		image.Compress (true);
		if( headImage != null )
			headImage.sprite = Sprite.Create(image, new Rect (0, 0, image.width,image.height),new Vector2(0,0));
	}


	public FiPresentRecord record;

	public long getID(){
		return id;
	}
	public override void ConfigureCellData ()
	{
		base.ConfigureCellData ();
		MailDataInfo nInfo = (MailDataInfo)Facade.GetFacade ().data.Get ( FacadeConfig.MAIL_MODULE_ID );
		if (dataObject != null)
			data = nInfo.getRecordList () [(int)dataObject];
		else
			data = null;

		if (data != null) {

			if (dataObject!=null&& FriendGiveManager.instans.lastNumber == (int)dataObject) {
				this.gameObject.GetComponent<Image> ().sprite = UIHallTexturers.instans.Mail[12];
				//transform.FindChild ("NickName").GetComponent<Outline> ().effectColor = new Color (0.965f,0.439f,0.153f,1);
			} else {
				this.gameObject.GetComponent<Image> ().sprite = UIHallTexturers.instans.Mail[13];
				//transform.FindChild ("NickName").GetComponent<Outline> ().effectColor = new Color (0.153f,0.435f,0.784f,1);
			}

			DateTime nSendDate = FiUtil.GetDate (data.timestamp);

			date.text = FiUtil.GetMouthAndDay (nSendDate); //nSendDate.Month<10 ? "0"+nSendDate.Month : nSendDate.Month + "/" + nSendDate.Day;// "9/18";//nMail.sendTime.ToString();
			time.text = FiUtil.GetHourMinute (nSendDate);//nSendDate.Hour + ":" + nSendDate.Minute;// "16:00";//nMail.sendTime.ToString();
			id = data.id;

			nickName.text =Tool.GetName( data.nickname,4);
			record = data;
			if(string.IsNullOrEmpty(data.avatar_url))
				tempHead = UIHallTexturers.instans.Mail[14];
			else
				SetUrl (data.userid, data.avatar_url);
			GetComponent<Button> ().onClick.RemoveAllListeners ();
			GetComponent<Button> ().onClick.AddListener (delegate () { FriendGiveManager.instans.OnItemClick(this.gameObject); });
			//EventTriggerListener.Get (this.gameObject).onClick = FriendGiveManager.instans.OnItemClick;
			//默认选中第一封
			if(FriendGiveManager.instans.mLastSelected==null &&(int)dataObject==0)
				FriendGiveManager.instans.OnItemClick(this.gameObject);
			if(FriendGiveManager.instans.mLastSelected!=null &&(int)dataObject==FriendGiveManager.instans.lastNumber)
				FriendGiveManager.instans.OnItemClick(this.gameObject);
		}

	}
}
