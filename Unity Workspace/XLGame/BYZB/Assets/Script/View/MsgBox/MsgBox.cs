/* author:KinSen
 * Date:2017.06.01
 */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UnityEngine.UI;

public class MsgBoxType
{
	public delegate void OnRcvMsgBoxResult(MsgBoxType.Result result);

	public enum Style
	{
		Confirm,
		ConfirmAndCancel,
		NumCount,
	}

	public enum Result
	{
		Confirm,
		Cancel,
		NumCount,
	}
}

public class MsgBox 
{
	public GameObject uiPrefab = null;
	public GameObject ui = null;
	public Text msgTitle;
	public Text msgContent;

	public Button[] btnGroup = new Button[2];
	private Vector3 ptConfirmOnlyConfirm; //只有确定按钮时，确定按钮的位置
	private Vector3 ptConfirmOfConfirmAndCancel; //有确定和取消按钮时，确定按钮的位置

	private MsgBoxType.OnRcvMsgBoxResult msgBoxResult = null;

	public MsgBox()
	{
		
	}

	~MsgBox()
	{
		
	}

	public void Show(Transform transform, string title, string content, MsgBoxType.Style style, MsgBoxType.OnRcvMsgBoxResult callBack=null)
	{
		if(null==ui)
		{
			uiPrefab = Resources.Load ("Prefabs/MsgBox") as GameObject;
			ui = GameObject.Instantiate (uiPrefab) as GameObject;

			ui.transform.parent = transform;
			ui.transform.localPosition = new Vector3 (0, 0, 0);

			UIMsgBox uiMsgBox = ui.GetComponent<UIMsgBox> ();
			msgTitle = uiMsgBox.title;
			msgContent = uiMsgBox.content;
			btnGroup [0] = uiMsgBox.btnGroup [0];
			btnGroup [1] = uiMsgBox.btnGroup [1];

			uiMsgBox.msgBoxConfirm = OnConfirm;
			uiMsgBox.msgBoxCancel = OnCancel;

			ptConfirmOfConfirmAndCancel = new Vector3(btnGroup [0].transform.localPosition.x, btnGroup [0].transform.localPosition.y, btnGroup [0].transform.localPosition.z);
			ptConfirmOnlyConfirm = new Vector3 (0, btnGroup [0].transform.localPosition.y, btnGroup [0].transform.localPosition.z);

		}

		msgTitle.text = title;
		msgContent.text = content;
		msgBoxResult = callBack;

		switch(style)
		{
		case MsgBoxType.Style.Confirm:
			btnGroup [0].interactable = true;
			btnGroup [0].transform.localPosition = ptConfirmOnlyConfirm;
			btnGroup [1].interactable = false;
			btnGroup [1].gameObject.SetActive (false);
			break;

		case MsgBoxType.Style.ConfirmAndCancel:
			btnGroup[0].interactable = true;
			btnGroup [0].transform.localPosition = ptConfirmOfConfirmAndCancel;
			btnGroup[1].interactable = true;
			btnGroup [1].gameObject.SetActive (true);
			break;
		}

		ui.gameObject.SetActive (true);

	}

	void OnConfirm()
	{
		if(null!=msgBoxResult)
		{
			ui.gameObject.SetActive (false);
			msgBoxResult (MsgBoxType.Result.Confirm);
			msgBoxResult = null;
		}

	}

	void OnCancel()
	{
		if(null!=msgBoxResult)
		{
			ui.gameObject.SetActive (false);
			msgBoxResult (MsgBoxType.Result.Cancel);
			msgBoxResult = null;
		}

	}


}
