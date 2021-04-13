using UnityEngine;
using System.Collections.Generic;
using UnityEngine.UI;
using AssemblyCSharp;

public class BankManager : MonoBehaviour {
    
	public Image BankBtn;
	public Image PresentBtn;
	public Image CharmBtn;
	public Image NoticeBtn;
	public GameObject BankGameObject;
	public GameObject PresentGameObject;
	public GameObject CharmGameObject;
	public GameObject NoticeGameObject;
	public GameObject NoneNoticeTips;
	public static BankManager instance;
	public GameObject RedPoint;
    //魅力兑换
    public GameObject Charm;
    //礼物赠送
    public GameObject Gift;

    MyInfo myInfo;


	Dictionary<Image,Image> ButtonDic ;//cun fang zi ti
	Image lastchoose;
	int num;
	BankInfo nDataInfo;
	BackageUI msgBackage;
	void Awake(){
		if (instance != null) {
			Destroy (instance.gameObject);
		}
		instance = this;
		ButtonDic = new Dictionary<Image, Image> ();
		ButtonDic.Add (BankBtn, BankBtn.transform.GetChild(0).GetComponent<Image> ());
		ButtonDic.Add (PresentBtn, PresentBtn.transform.GetChild(0).GetComponent<Image> ());
		ButtonDic.Add (CharmBtn, CharmBtn.transform.GetChild(0).GetComponent<Image> ());
		ButtonDic.Add (NoticeBtn, NoticeBtn.transform.GetChild(0).GetComponent<Image> ());
		gameObject.GetComponent<Canvas>().worldCamera=GameObject.FindGameObjectWithTag ("MainCamera").GetComponent<Camera>();

		nDataInfo=(BankInfo) Facade.GetFacade ().data.Get ( FacadeConfig.UI_BANk_MOUDLE_ID );
	}
	void Start(){
		UIColseManage.instance.ShowUI (this.gameObject);
		lastchoose = BankBtn;
		num = 0;
		msgBackage = NoticeGameObject.GetComponentInChildren<BackageUI> ();
		RedPoint.SetActive (nDataInfo.isUpdate);
        //判定是否关闭魅力兑换，礼物赠送功能
        myInfo = (MyInfo)Facade.GetFacade().data.Get(FacadeConfig.USERINFO_MODULE_ID);
        if(myInfo.Charm == 0)
        {
            Charm.transform.GetComponent<Button>().enabled = false;
        }
        if(myInfo.Gift == 0)
        {
            Gift.transform.GetComponent<Button>().enabled = false;
        }
	}
	void CloseAllGameObject(){
		BankGameObject.SetActive (false);
		PresentGameObject.SetActive (false);
		CharmGameObject.SetActive (false);
		NoticeGameObject.SetActive (false);
	}
	public void BtnClick(Image btn){
		if (lastchoose == btn)
			return;
		CloseAllGameObject ();
		lastchoose.sprite = UIHallTexturers.instans.Bank [0];
		//lastchoose.GetComponent<Image> ().SetNativeSize ();
		ButtonDic [lastchoose].sprite = UIHallTexturers.instans.Bank [num + 2];
		if (btn.name == "Bank_Button") {
			num = 0;
			BankGameObject.SetActive (true);
			BankGameObject.GetComponent<IBankItem> ().Refresh ();
		} else if (btn.name == "Present_Button") {
			num = 1;
			PresentGameObject.SetActive (true);
			PresentGameObject.GetComponent<IBankItem> ().Refresh ();
		} else if (btn.name == "Charm_Button") {
			num = 2;
			CharmGameObject.SetActive (true);
			CharmGameObject.GetComponent<IBankItem> ().Refresh ();
		} else {
			num = 3;
			NoticeGameObject.SetActive (true);
			msgBackage.cellNumber = nDataInfo.GetMsgList ().Count;
			msgBackage.Refresh ();
			if (msgBackage.cellNumber == 0)
				NoneNoticeTips.SetActive (true);
			else
				NoneNoticeTips.SetActive (false);
			nDataInfo.isUpdate = false;
			RedPoint.SetActive (false);
			Facade.GetFacade().message.bank.SendGetBankMessageRequest();

		}
		btn.sprite = UIHallTexturers.instans.Bank [1];
		//btn.GetComponent<Image> ().SetNativeSize ();
		ButtonDic [btn].sprite = UIHallTexturers.instans.Bank [num + 6];
		lastchoose = btn;

	}

	public void CloseBtnClick(){
		UIColseManage.instance.CloseUI ();
	}

	public void CallTips(string content, bool isNeedExtend=false){
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
