using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using AssemblyCSharp;
using System;

/// <summary>
/// System grid info.系统邮件的UI信息
/// </summary>

public class SystemGridInfo : ScrollableCell,IGrid
{
	public Text date;
	public Text time;
	public Text title;
	private FiSystemMail data;
	public object infoDetail;
	public Image head;
	public long id;

	public long getID ()
	{
		return id;
	}

	public override void ConfigureCellData ()
	{
		base.ConfigureCellData ();
		MailDataInfo nInfo = (MailDataInfo)Facade.GetFacade ().data.Get (FacadeConfig.MAIL_MODULE_ID);
		if (dataObject != null) {
			data = nInfo.getMailList () [(int)dataObject];
			Debug.LogError ("datacount" + nInfo.getMailList ().Count);
			Debug.LogError ("date" + data.mailId + "/" + data.content + "/" + data.property.Count + "/" + data.title);
			for (int i = 0; i < data.property.Count; i++) {
				Debug.LogError (data.property [i].type + "type/" + data.property [i].value);
			}
		} else
			data = null;
		if (data != null) {
			if (dataObject != null && SystemManger.instans.lastNumber == (int)dataObject) {
				GetComponent<Image> ().sprite = UIHallTexturers.instans.Mail [12];
				head.sprite = UIHallTexturers.instans.Mail [3];
				//transform.FindChild ("NickName").GetComponent<Outline> ().effectColor = new Color (0.965f,0.439f,0.153f,1);
			} else {
				head.sprite = UIHallTexturers.instans.Mail [4];
				GetComponent<Image> ().sprite = UIHallTexturers.instans.Mail [13];
				//transform.FindChild ("NickName").GetComponent<Outline> ().effectColor = new Color (0.153f,0.435f,0.784f,1);
			}
			DateTime nSendDate = FiUtil.GetDate (data.sendTime);

			date.text = FiUtil.GetMouthAndDay (nSendDate);
			time.text = FiUtil.GetHourMinute (nSendDate);
			id = data.mailId;
			title.text = data.title;
			infoDetail = data;
			GetComponent<Button> ().onClick.RemoveAllListeners ();
			GetComponent<Button> ().onClick.AddListener (delegate () {
				SystemManger.instans.OnItemClick (this.gameObject);
			});

			//EventTriggerListener.Get (this.gameObject).onClick = SystemManger.instans.OnItemClick;
			//默认选中第一封
			if (SystemManger.instans.mLastSelected == null && (int)dataObject == 0)
				SystemManger.instans.OnItemClick (this.gameObject);
			if (SystemManger.instans.mLastSelected != null && (int)dataObject == SystemManger.instans.lastNumber) {
				SystemManger.instans.OnItemClick (this.gameObject);
				head.sprite = UIHallTexturers.instans.Mail [3];
			}
		}
	}
}
