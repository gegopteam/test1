using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using AssemblyCSharp;

public class UIPhoneAuthorize : MonoBehaviour
{

	string key = "72FAWaNZdkl4t65EdvMVCZApcy1UlmlL";

	string userid = "";

	public InputField TxtTelNum;

	public InputField TxtCode;

	public GameObject btnGetCode;

	public Text TimeCode;

	string GetAskString ()
	{
		string nSign = Tool.GetMD532 (TxtTelNum.text + "" + key).ToLower ();
		//return "https://mobile.cl0579.com/api/getcode.aspx?" + "type=1&uid=" + userid + "&phone=" + TxtTelNum.text + "&md5=" + nSign;
		//return "https://mobile.lhzdbg.cn/api/api_sendcode.aspx?" + "type=1&uid=" + userid + "&phone=" + TxtTelNum.text + "&md5=" + nSign;
		return UIUpdate.WebUrlDic[WebUrlEnum.MMSAuth] + "?type=1&uid=&account=" + userid + "&phone=" + TxtTelNum.text + "&md5=" + nSign;
	}

	// Use this for initialization
	void Start ()
	{
		userid = DataControl.GetInstance ().GetMyInfo ().account;
		ShowBtnCode (true);
	}


	IEnumerator GetCode (string url)
	{
		Debug.LogError ("url : " + url);
		string nResult = "";
		WWW www = new WWW (url);
		yield return www;
		if (null != www.error) {
			//Debug.LogError("VerifyServerVersion1====>:"+www.error);
			ShowBtnCode (true);
		} else {
			Debug.LogError (" [ get code success ] " + www.text);
			try
			{
				nResult = www.text.Substring(0, 2);
			}
			catch {
				Debug.Log("nresult1 = null");
			}

			Debug.Log ("nresult1  =  " + nResult);
			if (nResult == "1,") {
				nResult = www.text.Substring(2);
				GameObject Window1 = UnityEngine.Resources.Load("Window/WindowTipsThree") as UnityEngine.GameObject;
				GameObject WindowClone1 = UnityEngine.GameObject.Instantiate(Window1);
				UITipAutoNoMask ClickTips1 = WindowClone1.GetComponent<UITipAutoNoMask>();
				ClickTips1.tipText.text = nResult;
			} else {
				nResult = www.text.Substring(3);
				GameObject Window1 = UnityEngine.Resources.Load("Window/WindowTipsThree") as UnityEngine.GameObject;
				GameObject WindowClone1 = UnityEngine.GameObject.Instantiate(Window1);
				UITipAutoNoMask ClickTips1 = WindowClone1.GetComponent<UITipAutoNoMask>();
				ClickTips1.tipText.text = nResult;
				ShowBtnCode(true);
			}
		}
	}

	public void OnSendVerifyNumber ()
	{
		if (string.IsNullOrEmpty (TxtTelNum.text)) { 
			GameObject Window1 = UnityEngine.Resources.Load ("Window/WindowTipsThree")as UnityEngine.GameObject;
			GameObject WindowClone1 = UnityEngine.GameObject.Instantiate (Window1);
			UITipAutoNoMask ClickTips1 = WindowClone1.GetComponent<UITipAutoNoMask> ();
			ClickTips1.tipText.text = "請輸入手機號";
			return;
		}
		ShowBtnCode (false);
		StartCoroutine (GetCode (GetAskString ()));
	}

	void ShowBtnCode (bool value)
	{
		btnGetCode.SetActive (value); 
		TimeCode.gameObject.SetActive (!value);
		nRemainDuration = 60.0f;
	}

	IEnumerator GetLoginToken (string url)
	{
		Debug.LogError ("url : " + url);
		WWW www = new WWW (url);
		yield return www;
		if (null != www.error) {
			Debug.LogError ("GetLoginToken : " + www.error);
			LoginUtil.GetIntance ().ShowWaitingView (false);
		} else {
			Debug.LogError (" [ get tokens success ] : " + www.text);
			string nResult = www.text;
			string nResult1 = www.text.Substring (0, 2);
			//if (nResult1.Equals ("1")) {
			//	int nStartIndex = nResult.IndexOf (",", 2);
			//	string uUserid = nResult.Substring (2, nStartIndex - 2);
			//	string nToken = nResult.Substring (nStartIndex + 1, nResult.Length - nStartIndex - 1);
			//	DataControl.GetInstance ().GetMyInfo ().mLoginData.userid = int.Parse( uUserid.ToString() );
			//	DataControl.GetInstance ().GetMyInfo ().mLoginData.token = nToken;
			//	DataControl.GetInstance ().GetMyInfo ().account = null;
			//	DataControl.GetInstance().ConnectSvr ( DataControl.GetInstance ().GetMyInfo ().ServerUrl , 50666);
			//	DataControl.GetInstance ().GetMyInfo ().platformType = 9;
			//	//Facade.GetFacade ().message.login.SendLoginRequest ();
			//} else {
			//	LoginUtil.GetIntance ().ShowWaitingView ( false );
			//	GameObject Window1 = UnityEngine.Resources.Load ("Window/WindowTipsThree")as UnityEngine.GameObject;
			//	GameObject WindowClone1 = UnityEngine.GameObject.Instantiate ( Window1 );
			//	UITipAutoNoMask ClickTips1 = WindowClone1.GetComponent<UITipAutoNoMask> ();
			//	ClickTips1.tipText.text = "数据异常";
			//}
			Debug.Log ("nresult1=" + nResult1);
			switch (nResult1) {


			case "1,":
				int nStartIndex = nResult.IndexOf (",", 2);
				string uUserid = nResult.Substring (2, nStartIndex - 2);
				string nToken = nResult.Substring (nStartIndex + 1, nResult.Length - nStartIndex - 1);
				DataControl.GetInstance ().GetMyInfo ().mLoginData.userid = int.Parse (uUserid.ToString ());
				DataControl.GetInstance ().GetMyInfo ().mLoginData.token = nToken;
				DataControl.GetInstance ().GetMyInfo ().account = null;
				Debug.Log (DataControl.GetInstance ().GetMyInfo ().ServerUrl + "____");
				DataControl.GetInstance ().ConnectSvr (DataControl.GetInstance ().GetMyInfo ().ServerUrl, AppInfo.portNumber);
				DataControl.GetInstance ().GetMyInfo ().platformType = 9;
                      //Facade.GetFacade ().message.login.SendLoginRequest ();
				break;
			case "-1":
				LoginUtil.GetIntance().ShowWaitingView(false);
				GameObject Window2 = UnityEngine.Resources.Load("Window/WindowTipsThree") as UnityEngine.GameObject;
				GameObject WindowClone2 = UnityEngine.GameObject.Instantiate(Window2);
				UITipAutoNoMask ClickTips2 = WindowClone2.GetComponent<UITipAutoNoMask>();
				ClickTips2.tipText.text = "驗證碼失效";
				break;
			case "-3":
				LoginUtil.GetIntance().ShowWaitingView(false);
				GameObject Window3 = UnityEngine.Resources.Load("Window/WindowTipsThree") as UnityEngine.GameObject;
				GameObject WindowClone3 = UnityEngine.GameObject.Instantiate(Window3);
				UITipAutoNoMask ClickTips3 = WindowClone3.GetComponent<UITipAutoNoMask>();
				ClickTips3.tipText.text = "此手機未綁定";
				break;
			case "-4":
				LoginUtil.GetIntance().ShowWaitingView(false);
				GameObject Window4 = UnityEngine.Resources.Load("Window/WindowTipsThree") as UnityEngine.GameObject;
				GameObject WindowClone4 = UnityEngine.GameObject.Instantiate(Window4);
				UITipAutoNoMask ClickTips4 = WindowClone4.GetComponent<UITipAutoNoMask>();
				ClickTips4.tipText.text = "此手機不是綁定手機";
				break;

			case "-5":
				LoginUtil.GetIntance().ShowWaitingView(false);
				GameObject Window5 = UnityEngine.Resources.Load("Window/WindowTipsThree") as UnityEngine.GameObject;
				GameObject WindowClone5 = UnityEngine.GameObject.Instantiate(Window5);
				UITipAutoNoMask ClickTips5 = WindowClone5.GetComponent<UITipAutoNoMask>();
				ClickTips5.tipText.text = "驗證碼錯誤";
				break;
			default:
				LoginUtil.GetIntance().ShowWaitingView(false);
				GameObject Window0 = UnityEngine.Resources.Load("Window/WindowTipsThree") as UnityEngine.GameObject;
				GameObject WindowClone0 = UnityEngine.GameObject.Instantiate(Window0);
				UITipAutoNoMask ClickTips0 = WindowClone0.GetComponent<UITipAutoNoMask>();
				ClickTips0.tipText.text = "數據異常";
				break;
			}
		}
	}


	void GetToken ()
	{
		string nSignStr = Tool.GetMD532 (TxtTelNum.text + "" + key).ToLower ();
		string nUrl = "https://mobile.msxfsh.cn/api/getcode.aspx?type=2&uid=" + userid + "&phone=" + TxtTelNum.text + "&code=" + TxtCode.text + "&md5=" + nSignStr;
		StartCoroutine (GetLoginToken (nUrl));
	}

	public void OnLogin ()
	{
		if (string.IsNullOrEmpty (TxtTelNum.text)) { 
			GameObject Window1 = UnityEngine.Resources.Load ("Window/WindowTipsThree")as UnityEngine.GameObject;
			GameObject WindowClone1 = UnityEngine.GameObject.Instantiate (Window1);
			UITipAutoNoMask ClickTips1 = WindowClone1.GetComponent<UITipAutoNoMask> ();
			ClickTips1.tipText.text = "請輸入手機號";
			return;
		}
		if (string.IsNullOrEmpty (TxtCode.text)) { 
			GameObject Window1 = UnityEngine.Resources.Load ("Window/WindowTipsThree")as UnityEngine.GameObject;
			GameObject WindowClone1 = UnityEngine.GameObject.Instantiate (Window1);
			UITipAutoNoMask ClickTips1 = WindowClone1.GetComponent<UITipAutoNoMask> ();
			ClickTips1.tipText.text = "請輸入驗證碼";
			return;
		}
		LoginUtil.GetIntance ().ShowWaitingView (true);
		GetToken ();
	}



	public void OnClose ()
	{
		DataControl.GetInstance ().GetMyInfo ().account = null;
		Destroy (gameObject);
	}



	float nRemainDuration = 60.0f;

	void Update ()
	{
		if (TimeCode.gameObject.activeSelf) {
			nRemainDuration -= Time.deltaTime;
			if (nRemainDuration <= 0) {
				nRemainDuration = 60.0f;
				ShowBtnCode (true);
			} else {
				TimeCode.text = ((int)nRemainDuration).ToString () + "秒後再次獲取";
			}
		}
	}
}
