using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using AssemblyCSharp;

public class Bank_PresentManager : IBankItem {

	public InputField targetID;
	public GameObject rose;
	public InputField giveNum;
	public InputField password;
	public Text tipsTop;
	public Text tipsDown;
	public Button giveButtin;
    public GameObject[] choose_image;
	//赠送数量
	int giveNumber;
	GameObject lastChoose;
	//礼物价格
	int cost;
	//银行存款
	long bankMoney;
	//礼物魅力值总量
    long charmNum;
	MyInfo myInfo;
	void Awake(){
		myInfo=(MyInfo)Facade.GetFacade ().data.Get ( FacadeConfig.USERINFO_MODULE_ID );
        choose_image[1].gameObject.SetActive(true);//默认显示赠送跑車选中框
	}
	// Use this for initialization
	void Start () {
		lastChoose = rose.transform.GetChild (0).gameObject;
		cost = 100000;
		giveNumber = 2;
		//假的银行数值，到时候再取真的过来。
		bankMoney = myInfo.loginInfo.bankGold;
		RefreshTips ();
	}
	public override void halfRefresh ()
	{
		bankMoney = myInfo.loginInfo.bankGold;
		//targetID.text = "";
		//giveNum.text = "1";
		//password.text = "";
		//giveNumber = 1;
		RefreshTips ();
	}
	public override void Refresh ()
	{
		bankMoney = myInfo.loginInfo.bankGold;
		targetID.text = "";
		giveNum.text = "2";
		password.text = "";
		giveNumber = 2;
		RefreshTips ();
	}
	public void PresentChoose(GameObject right){
		//lastChoose.SetActive (false);
		//right.SetActive (true);
		//lastChoose = right;
        if (right.transform.parent.name == "shell_Button")
        {
            //right.SetActive(true);
            choose_image[0].gameObject.SetActive(true);
            choose_image[1].gameObject.SetActive(false);
            choose_image[2].gameObject.SetActive(false);
            choose_image[3].gameObject.SetActive(false);
            cost = 10000;
            giveNum.text = "20";
        } 
        else if (right.transform.parent.name == "rose_Button")
        {
            choose_image[0].gameObject.SetActive(false);
            choose_image[1].gameObject.SetActive(true);
            choose_image[2].gameObject.SetActive(false);
            choose_image[3].gameObject.SetActive(false);
            cost = 100000;
            giveNum.text = "2";
        }
			
        else if (right.transform.parent.name == "diamond_Button")
        {
            choose_image[0].gameObject.SetActive(false);
            choose_image[1].gameObject.SetActive(false);
            choose_image[2].gameObject.SetActive(true);
            choose_image[3].gameObject.SetActive(false);
            cost = 500000; 
            giveNum.text = "2";
        }
			
        else if (right.transform.parent.name == "car_Button")
        {
            choose_image[0].gameObject.SetActive(false);
            choose_image[1].gameObject.SetActive(false);
            choose_image[2].gameObject.SetActive(false);
            choose_image[3].gameObject.SetActive(true);
            cost = 1000000;
            giveNum.text = "2";
        }
			
		RefreshTips ();
	}
	public void giveNumChanges(){
		if (giveNum.text != "") 
        {
			giveNumber = int.Parse (giveNum.text);
			if (giveNumber <= 0) 
            {
				giveNumber = 0;
			}
			giveNum.text=giveNumber.ToString();
		}
		else
			giveNumber = 0;
		RefreshTips ();
	}
	void RefreshTips(){

		if (giveNum.text != "" && int.Parse(giveNum.text) > 1000)
		{
			BankManager.instance.CallTips("單次贈送的魅力值上限為1000點！", true);

			return;
		}
		else if (giveNumber != 0)
		{
			charmNum = giveNumber * cost / 10000;
			long temp = bankMoney - (giveNumber * cost);
			tipsTop.text = "您的銀行存款：" + bankMoney + "，購買禮物後，存款餘額：" + temp;

			tipsDown.text = "受贈方魅力值：+" + charmNum + "點";
		}
		else
		{
			tipsTop.text = "您的銀行存款：" + bankMoney;
			tipsDown.text = "受贈方魅力值：+0點";
			charmNum = 0;
		}

	}

	public void GiveButtonClick(){
		if (targetID.text == "")
		{
			BankManager.instance.CallTips("贈送對象ID不能為空！");
			return;
		}
		if (password.text == "")
		{
			BankManager.instance.CallTips("密碼不能為空！");
			return;
		}
		if ((bankMoney - (giveNumber * cost)) < 0)
		{
			BankManager.instance.CallTips("銀行內存款不足！");
			return;
		}
		if ((giveNumber * cost) < 200000)
		{
			BankManager.instance.CallTips("贈送金幣不能低於20萬");
			return;
		}
		if (charmNum > 1000)
		{
			BankManager.instance.CallTips("單次贈送的魅力值上限為1000點！", true);
			return;
		}
		if (charmNum == 0)
		{
			BankManager.instance.CallTips("贈送數量不能為空！", true);
			return;
		}
		Facade.GetFacade().message.friend.SendGetUserInfoRequest(int.Parse( targetID.text ));
	}
	public int Cost{
		get{ return cost;}
	}
	public int GiveNumber{
		get{ return giveNumber;}
	}
}
