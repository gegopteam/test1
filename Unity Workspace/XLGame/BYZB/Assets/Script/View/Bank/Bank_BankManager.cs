using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using AssemblyCSharp;
using System;

public class Bank_BankManager :IBankItem
{
	
	public Text Coin;
	public Text Bank;
	public InputField setOrGetCoin;
	public InputField password;
	public Button setCoin;
	public Button getCoin;
		
	long setOrGetCoinNum;
	long GGetCoinNum;
	long coinNum;
	long bankNum;
	MyInfo myInfo;
	public Slider popslider;

	public Slider pushslider;

	public Text popText;

	public Text pupText;

	void Awake ()
	{
		myInfo = (MyInfo)Facade.GetFacade ().data.Get (FacadeConfig.USERINFO_MODULE_ID);
	}

	public override void halfRefresh ()
	{
		coinNum = myInfo.gold;
		bankNum = myInfo.loginInfo.bankGold;
		Coin.text = myInfo.gold.ToString ();
		Bank.text = myInfo.loginInfo.bankGold.ToString ();

		//setOrGetCoin.text = "";
		//password.text = "";
		//setOrGetCoinNum = 0;
	}

	public override void Refresh ()
	{
		coinNum = myInfo.gold;
		bankNum = myInfo.loginInfo.bankGold;
		Coin.text = myInfo.gold.ToString ();
		Bank.text = myInfo.loginInfo.bankGold.ToString ();
		setOrGetCoin.text = "";
		password.text = "";
		setOrGetCoinNum = 0;
		GGetCoinNum = 0;
		popText.text = "0";
		pupText.text = "0";
		popslider.value = 0;
		pushslider.value = 0;
	}
	// Use this for initialization
	void Start ()
	{
		
		popslider.onValueChanged.AddListener (ShowMoneypopmoney);

		pushslider.onValueChanged.AddListener (ShowMoneypushmoney);
		Refresh ();
	}
	//set coin
	public void SetCoinClick ()
	{
		Debug.LogError ("----setOrGetCoinNum__" + setOrGetCoinNum + "wwww" + coinNum);
		//setOrGetCoinNum = int.Parse (setOrGetCoin.text);
		if (setOrGetCoinNum < 50000) {
			BankManager.instance.CallTips ("存入金幣數最低5萬哦！");
			return;
		}
	
		//要存的金币超过身上金币
		if (setOrGetCoinNum > coinNum) {
			BankManager.instance.CallTips ("您的金幣不足，存入失敗！");
			return;
		}
		//if (password.text == "") {
		//	BankManager.instance.CallTips( "密码不能为空！");
		//	return;
		//}
		Facade.GetFacade ().message.bank.SendBankAccessRequest (setOrGetCoinNum, Tool.GetMD532 (password.text));
		popslider.value = 0;

		//Facade.GetFacade ().message.bank. 



	}

	public void ShowMoneypopmoney (float value)
	{
		
		setOrGetCoinNum = (long)((float)coinNum * value);

		setOrGetCoinNum = long.Parse (SetGold (setOrGetCoinNum, popText));

		if (value == 1) {
			setOrGetCoinNum = coinNum;
			SetGold (setOrGetCoinNum, popText, true, coinNum);
		} 

	}

	public void ShowMoneypushmoney (float value)
	{
		
		GGetCoinNum = (long)((float)bankNum * value);

		GGetCoinNum = long.Parse (SetGold (GGetCoinNum, pupText));


	
//		Debug.LogError ("value" + value + "setorgetcoin" + GGetCoinNum + "goinnum" + bankNum);
	
		if (value == 1) {
			GGetCoinNum = bankNum;
			SetGold (GGetCoinNum, pupText, true, bankNum);
		} 


//		pupText.text = setOrGetCoinNum.ToString ();
	}


	public void setOrGetCoinChange ()
	{
		if (setOrGetCoin.text != "") {
			try {
				setOrGetCoinNum = long.Parse (setOrGetCoin.text);
			} catch (Exception e) {
			}
			if (setOrGetCoinNum <= 0) {
				setOrGetCoinNum = 0;
			}
			setOrGetCoin.text = setOrGetCoinNum.ToString ();
		} else
			setOrGetCoinNum = 0;
	}

	public void OpenPassWard ()
	{
		string path = "MainHall/Common/WindowTipsBankIput";//"Window/BagWindow";
		GameObject WindowClone = AppControl.OpenWindow (path);
		WindowClone.SetActive (true);
		WindowTipsBankIput tips = WindowClone.transform.GetComponent<WindowTipsBankIput> ();
		tips.bankmange = this;


	}
	//get coin
	public void GetCoinClick (String passwardinput)
	{
		Debug.Log("GetCoinClick passwardinput = "+ passwardinput);
		password.text = passwardinput;
		if (GGetCoinNum == 0) {
			BankManager.instance.CallTips ("取出金額為空！");
			return;
		}
//		Debug.LogError ("----setOrGetCoinNum__" + GGetCoinNum + "sssssss" + bankNum);
		//要取的金币超过银行金币
		if (GGetCoinNum > bankNum) {
			BankManager.instance.CallTips ("銀行內金幣不足，取出失敗！", true);
			return;
		}

		string[] symbolS = new string[20] { "!", "@", "#", "$", "%", "^", "&", "*", "(", ")", "'", "-", "[", "]", "{", "}", "/", "?", ",", "." };
		bool havesymbol = false;
		for (int s = 0; s < symbolS.Length; s++)
		{
			if (passwardinput.IndexOf(symbolS[s]) > 0)
			{
				havesymbol = true;
			}
		}
		if (havesymbol)
		{
			BankManager.instance.CallTips("密碼不能包含符號！");
			return;
		}

		if (password.text == "") {
			Debug.Log("密碼不能為空 passwardinput = " + password.text);
			BankManager.instance.CallTips ("密碼不能為空！");
			return;
		}
		

		Facade.GetFacade ().message.bank.SendBankAccessRequest (GGetCoinNum * -1, Tool.GetMD532 (password.text));
		pushslider.value = 0;
	}

	public string SetGold (long nCount, Text ShowMoney, bool Isfull = false, long fullmaoney = 0)
	{

//		if (nCount < 100000) {
//			ShowMoney.text = nCount;
//		}
		string money = "";

		if (Isfull) {
			if (fullmaoney >= 100000000) {
				ShowMoney.text = "" + (float)(nCount / 1000000) / 100 + "億";
			
			} else if (nCount >= 100000 && fullmaoney < 100000000) {
				ShowMoney.text = "" + (int)(nCount / 10000) + "萬";

			} else if (nCount >= 10000 && fullmaoney < 100000) {
				ShowMoney.text = "" + (float)(nCount / 100) / 100 + "萬";

			}

			return money;
		}
		if (nCount >= 100000000) {

			ShowMoney.text = "" + (float)(nCount / 1000000) / 100 + "億";
			money = "" + (float)(nCount / 1000000) + "000000";
		} else if (nCount >= 10000 && nCount < 100000) {
			ShowMoney.text = "" + (int)(nCount / 10000) + "萬";
			money = "" + (int)(nCount / 10000) + "0000";
		} else if (nCount >= 100000 && nCount < 1000000) {
			ShowMoney.text = "" + (int)(nCount / 100000) * 10 + "萬";
			money = "" + (int)(nCount / 100000) * 10 + "0000";
		} else if (nCount >= 1000000 && nCount < 10000000) {
			ShowMoney.text = "" + (int)(nCount / 1000000) * 100 + "萬";
			money = "" + (int)(nCount / 1000000) * 100 + "0000";
		} else if (nCount >= 10000000 && nCount < 100000000) {
			ShowMoney.text = "" + (int)(nCount / 10000000) * 1000 + "萬";
			money = "" + (int)(nCount / 10000000) * 1000 + "0000";
		} else {
			ShowMoney.text = "" + nCount;
			money = "" + nCount;
		}
		return money;
	}

}
