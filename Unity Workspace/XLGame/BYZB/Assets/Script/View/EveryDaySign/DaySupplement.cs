using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using cn.sharesdk.unity3d;
using UnityEngine.UI;
using AssemblyCSharp;
public class DaySupplement : MonoBehaviour {


	public Image ShareSuceed;
	public Image Sharenormal;
	public Text  tips;
	public bool  issupplementDay;
	public static bool isMoneySign=false;
	public static bool isDiamondSign = false;
	public Text sharetip;
	public Text shareone;


	void Awake()
	{
		LoginUtil.GetIntance ().sharesd += Showmage;
	}
	void Start()
	{
		if (LoginUtil.GetIntance ().Ishare) {
			shareone.text = "每周一次";
			sharetip.gameObject.SetActive (false);
		}
		UIColseManage.instance.ShowUI (this.gameObject);
	}

	public void ShareSP()
	{
		//Debug.Log(" ~~~~~!!!!! DaySupplement !!!!!~~~~~ ShareSP LoginUtil.GetIntance = " + LoginUtil.GetIntance());
		if (LoginUtil.GetIntance () != null) {
			//Debug.Log(" ~~~~~!!!!! DaySupplement !!!!!~~~~~ ShareSP issupplementDay = " + issupplementDay);
			if (issupplementDay) {
				LoginUtil.GetIntance ().Capture ();
				isMoneySign = false;
			} else {
				GameObject Window1 = UnityEngine.Resources.Load ("Window/WindowTipsThree")as UnityEngine.GameObject;
				GameObject WindowClone1 = UnityEngine.GameObject.Instantiate ( Window1 );
				UITipAutoNoMask ClickTips1 = WindowClone1.GetComponent<UITipAutoNoMask> ();
				ClickTips1.tipText.text = "你不能分享 請返回點擊首次補簽天數";
			}
		}

	}

	public void MonenySP()
	{
		Debug.LogError (issupplementDay);

		MyInfo myInfo = (MyInfo)Facade.GetFacade ().data.Get (FacadeConfig.USERINFO_MODULE_ID);

		if (myInfo.gold < 5000) {
			GameObject Window1 = UnityEngine.Resources.Load ("Window/WindowTipsFour")as UnityEngine.GameObject;
			GameObject WindowClone1 = UnityEngine.GameObject.Instantiate ( Window1 );
			WinDowTipGoshop wintip = WindowClone1.gameObject.AddComponent<WinDowTipGoshop> ();
			wintip.tipText.text = "你的金幣不足！你是否前往商城購買";
			UIColseManage.instance.CloseUI ();
			return;
		}

		//发送金币补签协议
		if (issupplementDay) {
			Facade.GetFacade ().message.signIn.SendSignRetroactiveRequest (myInfo.userID, 2, 0, 0, ReSignDay.reday);
			isMoneySign = true;
			isDiamondSign = false;
			StartCoroutine (Close ());
		} else {
			GameObject Window1 = UnityEngine.Resources.Load ("Window/WindowTipsThree")as UnityEngine.GameObject;
			GameObject WindowClone1 = UnityEngine.GameObject.Instantiate ( Window1 );
			UITipAutoNoMask ClickTips1 = WindowClone1.GetComponent<UITipAutoNoMask> ();
			ClickTips1.tipText.text = "你不能分享 請返回點擊首次補簽天數";
		}
	}

    public void DiamondSP()
    {
		Debug.LogError(issupplementDay);
		MyInfo myInfo = (MyInfo)Facade.GetFacade().data.Get(FacadeConfig.USERINFO_MODULE_ID);

		if (myInfo.diamond < 5)
		{
			GameObject Window1 = UnityEngine.Resources.Load("Window/WindowTipsFour") as UnityEngine.GameObject;
			GameObject WindowClone1 = UnityEngine.GameObject.Instantiate(Window1);
			WinDowTipGoshop wintip = WindowClone1.gameObject.AddComponent<WinDowTipGoshop>();
			wintip.tipText.text = "你的鑽石不足！你是否前往商城購買";
			UIColseManage.instance.CloseUI();
			return;
		}

		//发送金币补签协议
		if (issupplementDay)
		{
			Facade.GetFacade().message.signIn.SendSignRetroactiveRequest(myInfo.userID, 1, 0, 0, ReSignDay.reday);
			isDiamondSign = true;
			isMoneySign = false;
			StartCoroutine(Close());
		}
		else
		{
			GameObject Window1 = UnityEngine.Resources.Load("Window/WindowTipsThree") as UnityEngine.GameObject;
			GameObject WindowClone1 = UnityEngine.GameObject.Instantiate(Window1);
			UITipAutoNoMask ClickTips1 = WindowClone1.GetComponent<UITipAutoNoMask>();
			ClickTips1.tipText.text = "你不能分享 請返回點擊首次補簽天數";
		}
	}

	public void Showmage()
	{
//		Sharenormal.gameObject.SetActive (false);
//		ShareSuceed.gameObject.SetActive (true);
		OnbtnClose();

	}


	void OnDestroy()
	{
		LoginUtil.GetIntance ().sharesd -= Showmage;
	}

	public void OnbtnClose()
	{
		UIColseManage.instance.CloseUI ();
	}
	IEnumerator Close()
	{
		yield return new WaitForSeconds (0.2f);
		UIColseManage.instance.CloseUI ();
	}

}
