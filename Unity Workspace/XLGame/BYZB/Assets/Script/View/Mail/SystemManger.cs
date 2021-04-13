using UnityEngine;
using System.Collections;
using UnityEngine.UI;

/// <summary>
/// System manger.系统邮件管理
/// </summary>
using AssemblyCSharp;
using System;

public class SystemManger : MonoBehaviour
{
	public GameObject scrollNoMail;
	public GameObject scrollMail;
	public GameObject showMailInformation;
	public RectTransform parent;
	public GameObject showMailGift;
	public static SystemManger instans;
	public GameObject mLastSelected;
	public int lastNumber = -1;
	public long lastID = 0;
	// Use this for initialization
	void Awake ()
	{
		instans = this;
		//UIMail.instans.RefreshSystemUI();
	}

	void Start ()
	{
        //获取所有的button，通过button来获取父节点通过for循环来判断是否是这个，如果是这个其他的节点图片更换原来未点击，与之相匹配的节点的换点击后的图片
        //如果有邮件则让没有邮件的信息隐藏
        //如果没有邮件让邮件信息的一项也隐藏
        Debug.LogError("----------SystemManger start--------");

    }

	//添加新邮件接口
	public void AppendMail (FiSystemMail nMail)
	{
		if (scrollNoMail.activeSelf)
			scrollNoMail.SetActive (false);
		if (!scrollMail.activeSelf)
			scrollMail.SetActive (true);
	}

	public void OnSwitch ()
	{
		if (mLastSelected != null) {
			DisplayMailInfo (mLastSelected);
		} else {
			showMailInformation.SetActive (false);
		}
	}

	public void ClearView ()
	{
		scrollNoMail.SetActive (true);
		scrollMail.SetActive (false);
		showMailInformation.SetActive (false);
		if (mLastSelected != null) {
			Image imageLast = mLastSelected.GetComponent<Image> ();
			if (imageLast != null) {
				imageLast.sprite = UIHallTexturers.instans.Mail [4];
			}
		}
	}

	public void DeleteMail (long mailId)
	{
		SystemGridInfo[] nInfoArray = parent.GetComponentsInChildren<SystemGridInfo> ();
		for (int i = 0; i < nInfoArray.Length; i++) {
			//列表已经存在记录了
			if (nInfoArray [i].id == mailId) {
				Destroy (nInfoArray [i].gameObject);
				return;
			}
		}
	}

	void AddGiftToInfo (Transform nParent, int nType, int nValue)
	{
		GameObject nGiftObject = Resources.Load ("MainHall/Mail/MailGift", typeof(GameObject)) as GameObject;
		GameObject nGiftInstance = Instantiate (nGiftObject);
		nGiftInstance.transform.SetParent (nParent);
		nGiftInstance.transform.localPosition = new Vector3 (-1, -3, 1);

		nGiftInstance.transform.localScale = Vector3.one;

		Image nPicture = nGiftInstance.transform.GetChild (0).GetChild (0).GetComponent<Image> ();
		Text nText = nGiftInstance.GetComponentInChildren <Text> ();
		RectTransform temps = nPicture.GetComponent<RectTransform> ();
		string nToolName = FiPropertyType.GetToolPath (nType);
		Debug.LogError ("-------------AddGiftToInfo : " + nToolName + " / " + nType);
		if (nToolName != null && !nToolName.Equals ("")) {
			Debug.LogError ("----------------AddGiftToInfo : " + nValue + " / "+nToolName );
			nPicture.sprite = FiPropertyType.GetSprite (nType);

			nText.text = SetGold (nValue);// + "×"+ FiPropertyType.GetToolName(  nType  );

			if (nToolName == "金幣") {
				temps.sizeDelta = new Vector2 (50, 50);
				temps.anchoredPosition = new Vector2 (1, 3);
			} else if (nToolName == "鑽石") {

//				temps.sizeDelta = new Vector2 (40, 40);
//				temps.anchoredPosition = Vector2.right * 2;
			}
			//nPicture.GetComponent<RectTransform> ().anchoredPosition =new Vector2(-20,23);
		}
	}

	public string SetGold (int  nCount)
	{
		string TxtGold = "";
		if (nCount > 100000000) {

			TxtGold = "" + (float)(nCount / 1000000) / 100 + "億";

		} else if (nCount >= 100000) {
			TxtGold = "" + (int)(nCount / 10000) + "萬";
		} else {
			TxtGold = "" + nCount;
		}
		return TxtGold;
	}
	//在内容页面呈现邮件详细信息
	void DisplayMailInfo (GameObject nObject)
	{
		Debug.LogError("----------------------- DisplayMailInfo");
		MailInfo nMailInfo = showMailInformation.GetComponent<MailInfo> ();
		int giftlength =	showMailGift.transform.childCount;
		for (int i = 0; i < giftlength; i++) {
			if (nMailInfo != null) {
				Transform nGift = nMailInfo.transform.Find ("MailGiftGroup/MailGift(Clone)");
				if (nGift != null) {
					Debug.LogError ("------1111------");
					DestroyImmediate (nGift.gameObject);
				}
			}
		}
//		if (nMailInfo != null) {
//			Transform nGift = nMailInfo.transform.Find ("MailGiftGroup/MailGift(Clone)");
//			if (nGift != null) {
//				Debug.LogError ("------1111------");
//				DestroyImmediate (nGift.gameObject);
//			}
//		}
		SystemGridInfo nGridInfo = nObject.GetComponentInChildren<SystemGridInfo> ();
		if (nMailInfo != null && nGridInfo != null) {
			FiSystemMail nMail = (FiSystemMail)nGridInfo.infoDetail;
			//Debug.LogError ( "-----------------------" +nMail.sendTime );
			nMailInfo.SetDate (FiUtil.GetDetail (nMail.sendTime));
			nMailInfo.SetContent (nMail.content);
			nMailInfo.SetUserName ("系統");
			nMailInfo.SetMailId (nGridInfo.id, MailInfo.SYSTEM_MAIL);
			Debug.LogError ("系統消息中的道具id為：" + nMail.property.Count);

			if (nMail.property.Count > 0) {
				nMailInfo.SetRecvButton ();
				for (int i = 0; i < nMail.property.Count; i++) {
					Debug.LogError("----------------------- DisplayMailInfo "+ showMailGift.transform +" / "+ nMail.property[i].type + " / " + nMail.property[i].value);
					AddGiftToInfo (showMailGift.transform, nMail.property [i].type, nMail.property [i].value);
				}
			} else {
				nMailInfo.SetDeleteButton ();
			}
		}

		if (!showMailInformation.activeSelf)
			showMailInformation.SetActive (true);
	}

	public void OnItemClick (GameObject nObject)
	{
		Debug.LogError ("---------OnItemClick---------" + nObject.name);
		AudioManager._instance.PlayEffectClip (AudioManager.ui_click);

		if (mLastSelected != null && lastID == nObject.GetComponent<IGrid> ().getID ())
			return;
		
		DisplayMailInfo (nObject);

		if (mLastSelected != null) {
			Image imageLast = mLastSelected.transform.GetChild (0).GetComponent<Image> ();
			if (imageLast != null) {
				imageLast.sprite = UIHallTexturers.instans.Mail [4];
				mLastSelected.GetComponent<Image> ().sprite = UIHallTexturers.instans.Mail [13];
				//imageLast.transform.FindChild ("NickName").GetComponent<Outline> ().effectColor = new Color (0.153f,0.435f,0.784f,1);
			}
		}

		Image image = nObject.transform.GetChild (0).GetComponent<Image> ();
		if (image != null) {
			image.sprite = UIHallTexturers.instans.Mail [3];
			nObject.GetComponent<Image> ().sprite = UIHallTexturers.instans.Mail [12];
			//image.transform.FindChild ("NickName").GetComponent<Outline> ().effectColor = new Color (0.965f,0.439f,0.153f,1);
		}
		mLastSelected = nObject;
		lastID = mLastSelected.GetComponent<IGrid> ().getID ();
		lastNumber = (int)nObject.GetComponent<SystemGridInfo> ().DataObject;
	}

	public void  DestroyGameobject (GameObject go)
	{
		Destroy (go); 
  
	}

	public void StartIEnumerator (IEnumerator enumerator)
	{
		StartCoroutine (enumerator);
	}
}
