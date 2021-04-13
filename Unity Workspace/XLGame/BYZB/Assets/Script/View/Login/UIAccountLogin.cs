using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using AssemblyCSharp;

public class UIAccountLogin : MonoBehaviour
{

	public InputField TxtAccount;

	public InputField TxtPSW;

	public Toggle TgRemember;
	public static	UIAccountLogin Instance;

	void Awake ()
	{
		Instance = this;
	}

	void Start ()
	{
		string nAccount = PlayerPrefs.GetString ("buyu_account");
		string nPswd = PlayerPrefs.GetString ("buyu_pswd");
		//Debug.LogError("buyu_account = " + nAccount + " ;buyu_pswd = "+ nPswd);

		if (!string.IsNullOrEmpty (nAccount)) {
			TxtAccount.text = nAccount;
		}

		if (!string.IsNullOrEmpty (nPswd)) {
			TxtPSW.text = nPswd;
		}
	}

	public void OnExit ()
	{
		gameObject.SetActive (false);
	}

	public void OnDisplay ()
	{
		gameObject.SetActive (true);
	}

	public void OnUserLogin ()
	{
		if (string.IsNullOrEmpty (TxtAccount.text) || LoginUtil.GetIntance () == null) {
			return;
		}

		PlayerPrefs.SetString ("buyu_account", TxtAccount.text);
		if (TgRemember.isOn) {
			PlayerPrefs.SetString ("buyu_pswd", TxtPSW.text);
		}
		MyInfo myInfo = DataControl.GetInstance ().GetMyInfo ();
		myInfo.isGuestLogin = false;
		myInfo.platformType = 0;
		myInfo.isPhoneNumberLogin = false;
		myInfo.connectServerAlr = false;
		myInfo.isconnecting = false;
		myInfo.account = TxtAccount.text;
		LoginUtil.GetIntance ().LoginWithAccount (TxtAccount.text, TxtPSW.text);
        //Facade.GetFacade ().message.login.SendAccountLogin ( TxtAccount.text , TxtPSW.text , 1 , "test" );
        //Debug.LogError("[ account ] ------- " + TxtAccount.text + " / " + TxtPSW.text + " / " + TgRemember.isOn);
        Debug.Log ("============OnUserLogin==========");
		LoginInfo.isAccoutLogin = true;
	}



	/// <summary>
	/// 修改密码
	/// </summary>
	public void OnForgetPasswordClick ()
	{
		string path = "Window/ChangePassWord";
		GameObject windowclone = AppControl.OpenWindow (path);
		windowclone.SetActive (true);
	}



}
