using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;
using AssemblyCSharp;

public class UIUserDetail : MonoBehaviour, IUiMediator
{

	public Text TxtGold;

	public Text TxtDiamond;

	public Text TxtCard;

	public Text TxtName;

	public Text TxtLevel;

	public Slider SliLevel;

	public Image IconAvatar;

	public Image Frame;

	public GameObject effect;

	public long goldTemp;

	public static UIUserDetail instace;
	MyInfo myInfo;

	void Awake ()
	{
		instace = this;
		myInfo = (MyInfo)Facade.GetFacade ().data.Get (FacadeConfig.USERINFO_MODULE_ID);
	}

	void Start ()
	{
		Facade.GetFacade ().ui.Add (FacadeConfig.UI_STORE_MODULE_ID, this);
		MyInfo myInfo = (MyInfo)Facade.GetFacade ().data.Get (FacadeConfig.USERINFO_MODULE_ID);
//		//Debug.Log ("UIUserDetail myInfo.diamond = " + myInfo.diamond);
		SetDiamond (myInfo.diamond);
//		//Debug.Log ("UIUserDetail myInfo.gold = " + myInfo.gold);

		SetGold (myInfo.gold);
		SetRoomCard (myInfo.roomCard);

		MyInfo nInfo = (MyInfo)Facade.GetFacade ().data.Get (FacadeConfig.USERINFO_MODULE_ID);
		//Debug.LogError (nInfo.nickname);
		TxtName.text = Tool.GetName (nInfo.nickname, 10);// "你好呀";
		TxtLevel.text = nInfo.level + "";// "100";
		Frame.sprite = UIHallTexturers.instans.VipFrame [nInfo.levelVip];
		if (nInfo.levelVip == 9)
			effect.SetActive (true);
		else
			effect.SetActive (false);
		SliLevel.value = (float)nInfo.experience / (float)nInfo.nextLevelExp;
		//Debug.LogError (SliLevel.value + "/" + nInfo.experience + "/" + nInfo.nextLevelExp);

		//添加头像
		if (!string.IsNullOrEmpty (nInfo.avatar)) {
			AvatarInfo nAvaInfo = (AvatarInfo)Facade.GetFacade ().data.Get (FacadeConfig.AVARTAR_MODULE_ID);
			nAvaInfo.Load (nInfo.userID, nInfo.avatar, (int nResult, Texture2D nTexture) => {
				if (nResult == 0 && IconAvatar != null && IconAvatar.gameObject.activeSelf) {
					if (nTexture.width == 8 && nTexture.height == 8) {
						return;
					}
					nTexture.filterMode = FilterMode.Bilinear;
					nTexture.Compress (true);
					IconAvatar.sprite = Sprite.Create (nTexture, new Rect (0, 0, nTexture.width, nTexture.height), new Vector2 (0, 0));
				}
			}
			);

		}

	}

	void OnDestroy ()
	{
		Facade.GetFacade ().ui.Remove (FacadeConfig.UI_STORE_MODULE_ID);
	}

	public void OnInit ()
	{

	}

	public void OnRelease ()
	{

	}

	public void OnChangeName (string name)
	{
		TxtName.text = Tool.GetName (name, 8);
	}

	public void OnRecvData (int nType, object nData)
	{
		if (nType == FiPropertyType.GOLD) {
			SetGold ((long)nData);
		} else if (nType == FiPropertyType.DIAMOND) {
			SetDiamond ((long)nData);
		} else if (nType == FiPropertyType.ROOM_CARD) {
			SetRoomCard ((long)nData);
		}
	}

	public void FreshVip (Sprite sprite)
	{
		Frame.sprite = sprite;
	}


	public void SetGold (long nCount)
	{
		if (nCount >= 100000000) {
			TxtGold.text = "" + (float)(nCount / 1000000) / 100 + "億";
		} else if (nCount >= 1000000) {
			TxtGold.text = "" + (int)(nCount / 10000) + "萬";
		} else {
			TxtGold.text = "" + nCount;
		}
		goldTemp = nCount;
	}

	public void UpdateGold(long nCount)
    {
		nCount = goldTemp + nCount;
		if (nCount >= 100000000)
		{
			TxtGold.text = "" + (float)(nCount / 1000000) / 100 + "億";
		}
		else if (nCount >= 1000000)
		{
			TxtGold.text = "" + (int)(nCount / 10000) + "萬";
		}
		else
		{
			TxtGold.text = "" + nCount;
		}
		goldTemp = nCount;
	}

	//public void UpdateGold()
	//{
	//	MyInfo myInfo = (MyInfo)Facade.GetFacade().data.Get(FacadeConfig.USERINFO_MODULE_ID);
	//	long nCount = myInfo.gold;

	//	if (nCount >= 100000000)
	//	{

	//		TxtGold.text = "" + (float)(nCount / 1000000) / 100 + "亿";

	//	}
	//	else if (nCount >= 1000000)
	//	{
	//		TxtGold.text = "" + (int)(nCount / 10000) + "万";
	//	}
	//	else
	//	{
	//		TxtGold.text = "" + nCount;
	//	}
	//}

	public void SetRoomCard (long nCount)
	{
		if (nCount > 100000000) {
			TxtCard.text = "" + (int)(nCount / 100000000) + "億";
		} else if (nCount >= 1000000) {
			TxtCard.text = "" + (int)(nCount / 10000) + "萬";
		} else {
			TxtCard.text = "" + nCount;
		}
	}

	public void SetDiamond (long nCount)
	{
		if (nCount > 100000000) {
			TxtDiamond.text = "" + (float)(nCount / 1000000) / 100 + "億";
		} else if (nCount >= 1000000) {
			TxtDiamond.text = "" + (int)(nCount / 10000) + "萬";
		} else {
			TxtDiamond.text = "" + nCount;
		}
	}

	public void OnBuyGold ()
	{
		if (myInfo.isGuestLogin) {
			StartGiftManager.GuestToStoreManager ();
			//return;
		}
		//在这里更改了商店预设体名称
		string path = "Window/NewStoreCanvas";
		GameObject WindowClone = AppControl.OpenWindow (path);
		WindowClone.SetActive (true);
		UIColseManage.instance.ShowUI (WindowClone);
		WindowClone.GetComponent<UIStore> ().CoinButton ();
	}
	//打来贵族购买界面
	public void OnOpenBuyNobel ()
	{
		if (myInfo.isGuestLogin) {
			StartGiftManager.GuestToStoreManager ();
			//return;
		}
		//在这里更改了商店预设体名称
		string path = "Window/NewStoreCanvas";
		GameObject WindowClone = AppControl.OpenWindow (path);
		WindowClone.SetActive (true);
		UIColseManage.instance.ShowUI (WindowClone);
		WindowClone.GetComponent<UIStore> ().OpenNobel (1);
	}

	public void OnBuyDiamond ()
	{
		if (myInfo.isGuestLogin) {
			StartGiftManager.GuestToStoreManager ();
			//return;
		}
		//在这里更换了商店预设体名称
		string path = "Window/NewStoreCanvas";
		GameObject WindowClone = AppControl.OpenWindow (path);
		WindowClone.SetActive (true);
		UIColseManage.instance.ShowUI (WindowClone);
		WindowClone.GetComponent<UIStore> ().DiamondButton ();
	}

	public void OnBuyRoomCard ()
	{
		string path = "Window/RoomCardTip";
		GameObject WindowClone = AppControl.OpenWindow (path);
		WindowClone.SetActive (true);
	}

	public void OnPersonalInfo ()
	{
		AudioManager._instance.PlayEffectClip (AudioManager.ui_click);
		string path = "Window/PersonalInfo";
		GameObject WindowClone = AppControl.OpenWindow (path);
		UIColseManage.instance.ShowUI (WindowClone);
		WindowClone.SetActive (true);

	}
}

