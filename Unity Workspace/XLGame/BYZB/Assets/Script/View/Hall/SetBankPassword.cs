using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using AssemblyCSharp;
using System.Text;

public class SetBankPassword : MonoBehaviour {
	public InputField password;
	public InputField again;
	public Button sure;
	public Button refuse;
	public static SetBankPassword instanse;
	void Awake(){
		instanse = this;
	}
	void Start(){
		sure.onClick.AddListener (SureClick);
		refuse.onClick.AddListener (RefuseClick);
		sure.interactable = false;
		UIColseManage.instance.ShowUI (this.gameObject);
	}
	void SureClick(){
		char[] symbolc = { '!', '@', '#', '$', '%', '^', '&', '*', '(', ')', '_', '-', '[', ']', '{', '}', '/', '?', ',', '.' };
		string[] symbolS = new string[20]{ "!", "@", "#", "$", "%", "^", "&", "*", "(", ")", "'", "-", "[", "]", "{", "}", "/", "?", ",", "." };
		bool havesymbol = false;
		//Debug.Log("My password = " + password.text);
		//Debug.Log("My again = " + again.text);
		for (int s=0; s< symbolS.Length; s++) {
			if (password.text.IndexOf(symbolS[s]) > 0 || again.text.IndexOf(symbolS[s]) > 0) {
				havesymbol = true;
			}
        }
		if (havesymbol)
		{
			CallTips("密码不能包含符号！", true);
		}
		else {
			if (password.text == again.text)
			{
				//发送消息
				//Debug.Log("finaly password = " + password.text);
				Facade.GetFacade().message.bank.SendSetPswdRequest(Tool.GetMD532(password.text));
				//Debug.LogError("发送的密码为：" + Tool.GetMD532(password.text));
			}
			else
			{
				CallTips("输入错误，两次密码输入不一致！", true);
			}
		}
    }
	void RefuseClick(){
		Destroy (this.gameObject);
	}
	public void PasswordChange(){
		if (password.text.Length >= 6) {
			sure.interactable = true;
		} else
			sure.interactable = false;
	}

	public void SetPasswordSucess(){
		//GameObject.FindGameObjectWithTag ("MainCamera").GetComponent<UIHall>().ToBank();
		UIHallCore uihall= (UIHallCore)Facade.GetFacade ().ui.Get ( FacadeConfig.UI_HALL_MODULE_ID );
		uihall.ToBank ();
		Destroy (this.gameObject);
	}

	void CallTips(string content,bool isNeedExtend=false){
		string path = "Window/WindowTipsThree";
		GameObject WindowClone = AppControl.OpenWindow (path);
		WindowClone.SetActive (true);
		UITipAutoNoMask ClickTips1 = WindowClone.GetComponent<UITipAutoNoMask> ();
		ClickTips1.tipText.text = " "+content;
		if (isNeedExtend) {
			RectTransform ImageRect = WindowClone.transform.GetChild (0).GetComponent<RectTransform> ();
			ImageRect.sizeDelta = (ClickTips1.tipText.preferredWidth+300) * Vector2.right + Vector2.up * 53;
		}
	}

}
