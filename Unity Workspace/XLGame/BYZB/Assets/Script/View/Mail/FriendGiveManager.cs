using UnityEngine;
using System.Collections;

/// <summary>
/// Friend give manager.好友赠送邮件
/// </summary>
using AssemblyCSharp;
using System.Collections.Generic;
using UnityEngine.UI;
using System;

public class FriendGiveManager : MonoBehaviour
{
	public GameObject scrollNoMail;
	public GameObject scrollMail;
	public GameObject showMailInformation;
	public GameObject showMailGiftGroup;
	public RectTransform parent;
	public static FriendGiveManager instans;
	public  GameObject mLastSelected;
	public int lastNumber = -1;
	public long lastID = 0;

	void Awake ()
	{
		instans = this;
		//UIMail.instans.RefreshSystemUI();
		//showMailGiftGroup.
	}

	void Start ()
	{

	}


	public void OnSwitch ()
	{
		if (mLastSelected != null) {
			Debug.LogError ( "------------OnSwitch-------------" );
			DisplayMailInfo (mLastSelected);
		} else {
			showMailInformation.SetActive (false);
		}
	}


	void AddGiftToInfo (Transform nParent, int nType, int nValue)
	{
		Debug.Log(" 贈送紀錄 AddGiftToInfo "+ nParent + " : "+ nType + " : " + nValue);
		MailInfo nMailInfo = showMailInformation.GetComponent<MailInfo> ();
		GameObject nGiftInstance = null;
		if (nMailInfo != null) {
			Transform temp = nMailInfo.transform.Find ("MailGift(Clone)");
			if (temp != null)
				nGiftInstance = temp.gameObject;
		}
		if (nGiftInstance == null) {
			GameObject nGiftObject = Resources.Load ("MainHall/Mail/MailGift", typeof(GameObject)) as GameObject;
			nGiftInstance = Instantiate (nGiftObject);
			nGiftInstance.transform.SetParent (nParent);

			nGiftInstance.transform.localPosition = new Vector3 (-10, -30, 1);

			nGiftInstance.transform.localScale = Vector3.one;
		}



		Image nPicture = nGiftInstance.transform.GetChild (0).GetChild (0).GetComponent<Image> ();
		Text nText = nGiftInstance.GetComponentInChildren <Text> ();
		RectTransform temps = nPicture.GetComponent<RectTransform> ();
		Debug.LogError ( "--------------------" + nType + " / " + nValue + " / " + nGiftInstance.transform.position.x + " / " + nGiftInstance.transform.localPosition.x );
		string nToolName = FiPropertyType.GetToolPath (nType);
		Debug.Log(" 贈送紀錄 AddGiftToInfo nToolName = " + nToolName);
		if (!nToolName.Equals ("")) {
			nPicture.sprite = FiPropertyType.GetSprite (nType);
			string nResultStr = FiPropertyType.GetToolName (nType);
			nText.text = nValue.ToString ();// + "×"+nResultStr;
			if (nToolName == "金幣") {
				temps.sizeDelta = new Vector2 (50, 50);
				temps.anchoredPosition = new Vector2 (1, 3);
			} else if (nToolName == "鑽石") {
				temps.sizeDelta = new Vector2 (40, 40);
				temps.anchoredPosition = Vector2.right * 2;
			}
			//nPicture.GetComponent<RectTransform> ().anchoredPosition =new Vector2(-20,23);
		}
	}

	void DisplayMailInfo (GameObject nObject)
	{
		Debug.Log("Mail Mail Mail DisplayMailInfo nObject = "+ nObject.name);
		MailInfo nMailInfo = showMailInformation.GetComponent<MailInfo> ();
		int giftlength = showMailGiftGroup.transform.childCount;
		for (int i = 0; i < giftlength; i++)
		{
			if (nMailInfo != null)
			{
				Transform nGift = nMailInfo.transform.Find("MailGiftGroup/MailGift(Clone)");
				if (nGift != null)
				{
					Debug.LogError("------1111------");
					DestroyImmediate(nGift.gameObject);
				}
			}
		}
		if (nMailInfo != null) {
			Transform nGift = showMailGiftGroup.transform.Find ("MailGift(Clone)");
			if (nGift != null) {
				DestroyImmediate (nGift.gameObject);
			}
		}
		GiveGridInfo nGridInfo = nObject.GetComponentInChildren<GiveGridInfo> ();
		if (nMailInfo != null && nGridInfo != null) {
			//nMailInfo.SetDate ( GetDateString() );
			string nContent = "贈送了你【 " + nGridInfo.record.property.value + " 】" + FiPropertyType.GetToolName (nGridInfo.record.property.type);
			//赠送记录肯定有物品
			nMailInfo.SetContent (nContent);

			nMailInfo.SetDate (FiUtil.GetDetail (nGridInfo.record.timestamp));
			nMailInfo.SetUserName (nGridInfo.record.nickname);
			nMailInfo.SetMailId (nGridInfo.id, MailInfo.PRESENT);
			nMailInfo.SetRecvButton ();

			AddGiftToInfo (showMailGiftGroup.transform, nGridInfo.record.property.type, nGridInfo.record.property.value);
		}

		if (!showMailInformation.activeSelf)
			showMailInformation.SetActive (true);
	}

	public void ClearView ()
	{
		scrollNoMail.SetActive (true);
		scrollMail.SetActive (false);
		showMailInformation.SetActive (false);
	}

	public void OnItemClick (GameObject nObject)
	{
		AudioManager._instance.PlayEffectClip (AudioManager.ui_click);

		//Debug.LogError ( "give clicked-------------" );
		if (mLastSelected != null && lastID == nObject.GetComponent<IGrid> ().getID ())
			return;
		
		DisplayMailInfo (nObject);
		//恢复选中的按钮状态
		if (mLastSelected != null) {
			Image imageLast = mLastSelected.GetComponent<Image> ();
			if (imageLast != null) {
				imageLast.sprite = UIHallTexturers.instans.Mail [13];
				//imageLast.transform.FindChild ("NickName").GetComponent<Outline> ().effectColor = new Color (0.153f,0.435f,0.784f,1);//  "#12def9";
			}
		}
		//更改列表选中状态
		Image image = nObject.GetComponent<Image> ();
		if (image != null) {
			image.sprite = UIHallTexturers.instans.Mail [12];
			//image.transform.FindChild ("NickName").GetComponent<Outline> ().effectColor = new Color (0.965f,0.439f,0.153f,1);//  "#12def9";
		}
		mLastSelected = nObject;
		lastID = mLastSelected.GetComponent<IGrid> ().getID ();
		lastNumber = (int)mLastSelected.GetComponent<GiveGridInfo> ().DataObject;
	}

	public void DeleteGive (long mailId)
	{
		GiveGridInfo[] nInfoArray = parent.GetComponentsInChildren<GiveGridInfo> ();
		for (int i = 0; i < nInfoArray.Length; i++) {
			//列表已经存在记录了
			if (nInfoArray [i].id == mailId) {
				Destroy (nInfoArray [i].gameObject);
				return;
			}
		}
	}

	public void AppenGiveMail (FiPresentRecord nRecord)
	{
		if (scrollNoMail.activeSelf)
			scrollNoMail.SetActive (false);
		if (!scrollMail.activeSelf)
			scrollMail.SetActive (true);
	}


}
