using System.Collections;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using AssemblyCSharp;
using LitJson;
using UnityEngine;
using UnityEngine.UI;

public class ChangePassWord : MonoBehaviour
{
	string KEY = "72FAWaNZdkl4t65EdvMVCZApcy1UlmlL";

	public InputField Account;
	public InputField TelNum;
	public InputField Code;
	public Button getcode;
	public Text time;

	public InputField PassWord;
	public InputField againPassWord;

	public Button Sure;
	public Button Exit;

	public GameObject CODE;
	public GameObject PSWD;

	MyInfo myInfo;
	string userid;
	GameObject Window1;

	/// <summary>
	/// 初始化
	/// </summary>
	void Start ()
	{
		userid = DataControl.GetInstance ().GetMyInfo ().account;

		Window1 = UnityEngine.Resources.Load ("Window/WindowTipsThree") as UnityEngine.GameObject;
		myInfo = (MyInfo)Facade.GetFacade ().data.Get (FacadeConfig.USERINFO_MODULE_ID);

		Sence (true);
		Button (true);

		//账号弹窗输入完成后的检测,如果为纯数字,自动填充手机号
		Account.onEndEdit.AddListener (AutomaticFilling);

		Sure.onClick.AddListener (() => {
			//输入账号手机号验证码
			if (CODE.gameObject.activeSelf) {
				if (Account.text == "" || TelNum.text == "" || Code.text == "") {
					GameObject WindowClone1 = Instantiate (Window1);
					UITipAutoNoMask ClickTips1 = WindowClone1.GetComponent<UITipAutoNoMask> ();
					ClickTips1.tipText.text = "請輸入正確的手機號,遊戲帳號和驗證碼";
					return;
				}
				Sence (false);
			}
            //输入密码
            else {
				HttpSend ();
			}
		});

		Exit.onClick.AddListener (() => {
			OnDestroy ();
		});

		getcode.onClick.AddListener (getCode);
	}

	/// <summary>
	/// 判断能否自动填写手机号
	/// </summary>
	void AutomaticFilling (string str)
	{
		if (Regex.IsMatch (str, "^[0-9]*$")) {
			TelNum.text = str;
		}
	}

	/// <summary>
	/// 修改密码入口
	/// </summary>
	void HttpSend ()
	{
		if (PassWord.text != againPassWord.text) {
			GameObject WindowClone1 = Instantiate (Window1);
			UITipAutoNoMask ClickTips1 = WindowClone1.GetComponent<UITipAutoNoMask> ();
			ClickTips1.tipText.text = "兩次密碼不一致";
			Debug.Log("兩次密碼不正確");
			return;
		}

		//string UserId = Tool.GetMD532(Account.text).ToLower();
		string secpwd = Tool.GetMD532 (PassWord.text).ToLower ();
		string nSign = Tool.GetMD532 (Account.text + secpwd + KEY).ToLower ();
		string url = "https://mobile.msxfsh.cn/api/pwdmodify.aspx?" + "type=2&userID=" + Account.text + "&secpwd=" + secpwd + "&pwd=" + secpwd + "&phonenum=" + TelNum.text + "&code=" + Code.text + "&&m=" + nSign;
		//string url = "https://mobile.cl0579.com/api/pwdmodify.aspx?type=2&userID=" + Account.text + "&secpwd=" + secpwd + "&pwd=" + secpwd + "&phonenum=" + TelNum.text + "&code=" + Code.text + "&&m=" + nSign;
		//string url = "https://interface.clgame.cc/api/pwdmodify.aspx?type=2&userID=" + Account.text + "&secpwd=" + secpwd + "&pwd=" + secpwd + "&phonenum=" + TelNum.text + "&code=" + Code.text + "&&m=" + nSign;
		//https://mobile.cl0579.com/api/pwdmodify.aspx?type=2&userID=wangxun222 &secpwd=8f66f9d81430a22f64396beff8773a4e&pwd=xxxx  &phonenum=130xxx     &code=11110 &&m=57777f9700c9c25dc24b8f45f393db9a

		Debug.Log("明碼=" + Account.text + secpwd + KEY);
		Debug.Log("UID:" + Account.text);
		Debug.Log("PassWord:" + PassWord.text);
		Debug.Log("KEY:" + KEY);
		Debug.Log("加密密碼:" + secpwd);
		StartCoroutine (Send (url));
	}

	/// <summary>
	/// 修改密码请求
	/// </summary>
	/// <returns>The send.</returns>
	/// <param name="url">URL.</param>
	IEnumerator Send (string url)
	{
		WWWForm wwwform = new WWWForm ();
		WWW www = new WWW (url, wwwform);
		yield return www;
		Debug.Log (url);
		if (www.error != null) {
			Debug.Log (www.text);
			Debug.Log ("密码修改失败");
		} else {
			string wwwtext = www.text;
			Debug.Log (www.text);
			JsonData jd = JsonMapper.ToObject (wwwtext);

			string result = jd ["result"].ToString ();
			string content = jd ["content"].ToString ();

			Debug.Log (result);
			Debug.Log (content);

			if (result == "True") {
				GameObject WindowClone1 = Instantiate (Window1);
				UITipAutoNoMask ClickTips1 = WindowClone1.GetComponent<UITipAutoNoMask> ();
				ClickTips1.tipText.text = "修改成功";
				OnDestroy();
			} else {
				GameObject WindowClone1 = Instantiate (Window1);
				UITipAutoNoMask ClickTips1 = WindowClone1.GetComponent<UITipAutoNoMask> ();
				ClickTips1.tipText.text = "修改失敗," + content;
				Sence (true);
			}
		}
	}

	/// <summary>
	/// 验证码获取入口
	/// </summary>
	void getCode ()
	{
		if (Account.text == "") {
			GameObject WindowClone1 = Instantiate (Window1);
			UITipAutoNoMask ClickTips1 = WindowClone1.GetComponent<UITipAutoNoMask> ();
			ClickTips1.tipText.text = "請輸入遊戲帳號";
			return;
		}

		if (TelNum.text == "" || TelNum.text.Length != 11) {
			GameObject WindowClone1 = Instantiate (Window1);
			UITipAutoNoMask ClickTips1 = WindowClone1.GetComponent<UITipAutoNoMask> ();
			ClickTips1.tipText.text = "請輸入正確的手機號";
			Debug.Log("沒有輸入正確手機號");
			return;
		}

		sendCode ();
		Button (false);
	}

	/// <summary>
	/// 发送 验证码
	/// </summary>
	void sendCode ()
	{
		//userid = DataControl.GetInstance().GetMyInfo().account;

		string nSign = Tool.GetMD532 (TelNum.text + "" + KEY).ToLower ();

		string url = UIUpdate.WebUrlDic[WebUrlEnum.MMSAuth] + "?type=1&uid=&account=" + Account.text + "&phone=" + TelNum.text + "&md5=" + nSign;
		Debug.LogError (url);
		StartCoroutine (GetCode (url));
	}

	/// <summary>
	/// 获取验证码
	/// </summary>
	/// <returns>The code.</returns>
	/// <param name="url">URL.</param>
	IEnumerator GetCode (string url)
	{
		Debug.LogError ("url : " + url);
		WWW www = new WWW (url);
		yield return www;
		if (www.error != null) {
			Debug.LogError ("VerifyServerVersion1====>:" + www.error);
			GameObject WindowClone1 = UnityEngine.GameObject.Instantiate (Window1);
			UITipAutoNoMask ClickTips1 = WindowClone1.GetComponent<UITipAutoNoMask> ();
			ClickTips1.tipText.text = www.error;
			//ShowBtnCode(true);
			Debug.Log ("驗證碼獲取失敗！");
			Button (true);
		} else {
			Debug.LogError (" [ get code success ] " + www.text);

			string nResult = www.text.Substring (0, 2);
			Debug.Log ("nresult=" + nResult);
			switch (nResult) {
			case "1,":
                
				break;

			case "-1":
				GameObject WindowClone1 = UnityEngine.GameObject.Instantiate (Window1);
				UITipAutoNoMask ClickTips1 = WindowClone1.GetComponent<UITipAutoNoMask> ();
				ClickTips1.tipText.text = "帳號或手機號輸入有誤";
				Button (true);

				break;

			case "-2":
				GameObject WindowClone2 = UnityEngine.GameObject.Instantiate (Window1);
				UITipAutoNoMask ClickTips2 = WindowClone2.GetComponent<UITipAutoNoMask> ();
				ClickTips2.tipText.text = "輸入的用戶名不存在";
				Debug.Log("輸入的用戶名不存在");
				Button (true);
				break;

			case "-3":
				GameObject WindowClone3 = UnityEngine.GameObject.Instantiate (Window1);
				UITipAutoNoMask ClickTips3 = WindowClone3.GetComponent<UITipAutoNoMask> ();
				ClickTips3.tipText.text = "此帳號沒有綁定手機";
				Debug.Log("此帳號沒有綁定手機");
				Button (true);
				break;

			case "-4":
				GameObject WindowClone0 = UnityEngine.GameObject.Instantiate (Window1);
				UITipAutoNoMask ClickTips0 = WindowClone0.GetComponent<UITipAutoNoMask> ();
				ClickTips0.tipText.text = "此手機不是綁定手機";
				Debug.Log("此手機不是綁定手機");
				Button (true);
				break;

			//case "-11":
			//GameObject WindowClone4 = UnityEngine.GameObject.Instantiate(Window1);
			//UITipAutoNoMask ClickTips4 = WindowClone4.GetComponent<UITipAutoNoMask>();
			//ClickTips4.tipText.text = "数据异常";
			//Debug.Log("数据异常");
			//Button(true);
			//break;

			default:
				GameObject WindowClone = UnityEngine.GameObject.Instantiate (Window1);
				UITipAutoNoMask ClickTips = WindowClone.GetComponent<UITipAutoNoMask> ();
				ClickTips.tipText.text = "數據異常";
				Button (true);

				break;
			}
		}
	}

	private void OnDestroy ()
	{
		TelNum.text = "";
		Account.text = "";
		Code.text = "";
		PassWord.text = "";
		againPassWord.text = "";

		Destroy (gameObject);
	}

	/// <summary>
	/// 两个界面,true时第一个界面显示
	/// </summary>
	void Sence (bool istrue)
	{
		CODE.gameObject.SetActive (istrue);
		PSWD.gameObject.SetActive (!istrue);
	}

	/// <summary>
	/// true时按钮显示
	/// </summary>
	void Button (bool istrue)
	{
		getcode.gameObject.SetActive (istrue);
		time.gameObject.SetActive (!istrue);
	}

	float nRemainDuration = 60.0f;

	void Update ()
	{
		if (time.gameObject.activeSelf) {
			nRemainDuration -= Time.deltaTime;
			if (nRemainDuration <= 0) {
				nRemainDuration = 60.0f;
				Button (true);
			} else {
				time.text = ((int)nRemainDuration).ToString ();
			}
		}
	}
}
