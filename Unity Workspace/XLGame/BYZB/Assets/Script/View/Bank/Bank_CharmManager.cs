using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using AssemblyCSharp;

public class Bank_CharmManager : IBankItem {

	public Text Charm;
	public Text exchangeTime;
	public InputField exchangeCharmNum;
	public InputField password;
	public Button changeButton;
	int num_exchange;
	long now_charm;
	int now_exchangetime;
	MyInfo myInfo;
	void Awake(){
		myInfo=(MyInfo)Facade.GetFacade ().data.Get ( FacadeConfig.USERINFO_MODULE_ID );
	}
	public override void halfRefresh ()
	{
		now_charm = myInfo.loginInfo.charm;
		now_exchangetime = myInfo.loginInfo.charmExchangeTimes;
		Charm.text = myInfo.loginInfo.charm.ToString ();
		exchangeTime.text = myInfo.loginInfo.charmExchangeTimes.ToString ();
		//exchangeCharmNum.text = "";
		//password.text = "";
		//num_exchange = 0;
	}
	public override void Refresh ()
	{
		now_charm = myInfo.loginInfo.charm;
		now_exchangetime = myInfo.loginInfo.charmExchangeTimes;
		Charm.text = myInfo.loginInfo.charm.ToString ();
		exchangeTime.text = myInfo.loginInfo.charmExchangeTimes.ToString ();
		exchangeCharmNum.text = "";
		password.text = "";
		num_exchange = 0;
	}
	// Use this for initialization
	void Start () {
		Refresh ();
	}

	public void exchangeChange(){
		if (exchangeCharmNum.text != "") {
			num_exchange = int.Parse (exchangeCharmNum.text);
			if (num_exchange <= 0) {
				num_exchange = 0;
			}
			exchangeCharmNum.text=num_exchange.ToString();
		}
		else
			num_exchange = 0;
	}
	
	public void ChangeButtonClick(){
		if (num_exchange == 0) {
			BankManager.instance.CallTips ("魅力兌換值不能為空！");
			return;
		}
		if (num_exchange > now_charm) {
			BankManager.instance.CallTips ("兌換失敗，您的魅力值不足！", true);
			return;
		}
		if (num_exchange > 300) {
			BankManager.instance.CallTips ("每次魅力值兌換上限為300點！", true);
			return;
		}
		if (now_exchangetime <= 0) {
			BankManager.instance.CallTips ("今日魅力兌換次數已用完！", true);
			return;
		}	
		if (password.text == "") {
			BankManager.instance.CallTips ("密碼不能為空！");
			return;
		}
		Facade.GetFacade().message.bank.SendExchangeCharmRequest(num_exchange,Tool.GetMD532(password.text));
		changeButton.interactable = false;




	}
}
