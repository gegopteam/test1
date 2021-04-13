using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;

using AssemblyCSharp;

public class UnlockGunGrid : MonoBehaviour
{

	int userVipLevel = -1;

	//	Image maskImage;
	Image btnImage;
	//	Image lockImage;

	Sprite toEquipSprite;
	Sprite unlockSprite;
	Sprite onEquipSprite;

	GameObject Window1;
	MyInfo myInfo;

	public int thisVipLevel = -1;

	bool isLock = true;

	bool isEquiping = false;
	List<int> typeList = new List<int> ();
	int type;
	int[] LibitedTimeBatteryID = {9036, 9307 };

	void OnEnable ()
	{
		//Init ();
	}


	public  void Init (int x)
	{
//		maskImage = transform.Find ("Mask").GetComponent<Image> ();
		btnImage = transform.Find ("Equip").GetComponent<Image> ();
        //		lockImage = transform.Find ("Lock").GetComponent<Image> ();

        btnImage.transform.GetComponent<Button>().onClick.RemoveAllListeners();
        btnImage.transform.GetComponent<Button>().onClick.AddListener(delegate() {
            EquipOrUnlock(x);
        });

		toEquipSprite = UILockBattery._instance.toEquipSprite;
		unlockSprite = UILockBattery._instance.unlockSprite;
		onEquipSprite = UILockBattery._instance.onEquipSprite;

		userVipLevel = DataControl.GetInstance ().GetMyInfo ().levelVip;
		//Debug.LogError("USerVipLevel="+userVipLevel);

        //判斷砲台顯示 裝備、獲取、已裝備
		BackpackInfo nBackInfo = (BackpackInfo)Facade.GetFacade().data.Get(FacadeConfig.BACKPACK_MODULE_ID);
		MyInfo nMyInfo = (MyInfo)Facade.GetFacade().data.Get(FacadeConfig.USERINFO_MODULE_ID);
		List<FiBackpackProperty> nBackArray = nBackInfo.getInfoArray();

		Debug.Log("===  正在初始化  " + x + " nBackArray = "+ nBackArray.Count);
		
		if (x == 1) {
			for (int i=0; i< nBackArray.Count; i++) {
				Debug.Log(" 揹包所有道具："+ nBackArray[i].id);
				Debug.Log(" 揹包所有道具：" + nBackArray[i].name);
				Debug.Log(" 揹包所有道具：" + nBackArray[i].type);
				Debug.Log(" ============");
			}
			
        }

		isLock = true;
        btnImage.sprite = unlockSprite;
        btnImage.transform.localScale = Vector3.one;

        
        if (x == 2)
		{
            //新手砲台
            isLock = false;
			if (nMyInfo.cannonStyle == 3000)
			{
				btnImage.sprite = onEquipSprite;
				btnImage.transform.localScale = Vector3.one * 1.35f;
			}
			else
			{
				btnImage.sprite = toEquipSprite;
				btnImage.transform.localScale = Vector3.one;
			}
		}
		else {
			//Debug.Log("===  正在初始化  " + x + " nBackArray = " + nBackArray[x]);
			for (int info = 0; info < nBackArray.Count; info++)
			{
				if (nBackArray[info].id >=3000 && nBackArray[info].id < 3010) {
					Debug.Log("----------- "+ nBackArray[info].id);
					if (nBackArray[info].id == (UIBatteryBuy.GunID[x]-3000)) {
						Debug.Log(nBackArray[info].id + " : " + UIBatteryBuy.GunID[x]);
						isLock = false;
						if (nMyInfo.cannonStyle == nBackArray[info].id)
						{
							btnImage.sprite = onEquipSprite;
							btnImage.transform.localScale = Vector3.one * 1.35f;
						}
						else
						{
							btnImage.sprite = toEquipSprite;
							btnImage.transform.localScale = Vector3.one;
						}
					}
                }

				if (nBackArray[info].type >= 3000 && nBackArray[info].type < 3010) {
					Debug.Log("----------- " + nBackArray[info].type);
					if (nBackArray[info].type == (UIBatteryBuy.GunID[x] - 3000))
					{
						Debug.Log(nBackArray[info].type + " : " + UIBatteryBuy.GunID[x]);
						isLock = false;
						if (nMyInfo.cannonStyle == nBackArray[info].type)
						{
							btnImage.sprite = onEquipSprite;
							btnImage.transform.localScale = Vector3.one * 1.35f;
						}
						else
						{
							btnImage.sprite = toEquipSprite;
							btnImage.transform.localScale = Vector3.one;
						}
					}
				}
			}
		}
		

//		if (userVipLevel >= thisVipLevel) { //有权限使用
//		    isLock = false;
////			lockImage.enabled = false;
////			maskImage.enabled = false;


//		    if (PrefabManager._instance.GetLocalGun ().currentGunStyle == thisVipLevel) {
//			    btnImage.sprite = onEquipSprite;
//			    btnImage.transform.localScale = Vector3.one * 1.35f;
//		    } else {
//			    btnImage.sprite = toEquipSprite;
//			    btnImage.transform.localScale = Vector3.one;
//		    }

//		} else { 
//			//无权限使用，出现锁定图标
//			isLock = true;	
////			lockImage.enabled = true;
////			maskImage.enabled = true;
//			btnImage.sprite = unlockSprite;
//			btnImage.transform.localScale = Vector3.one;
//		}
		//判断时间是否有效,有效的话开启特定的炮台

		//Debug.Log ("UsePropTool.isFirstUsePro = " + UsePropTool.isFirstUsePro);
	
		if (UsePropTool.Instance.proTimeRequest.Count > 0) {
			for (int i = 0; i < UsePropTool.Instance.proTimeRequest.Count; i++) {
				type = UsePropTool.Instance.proTimeRequest [i].propType;
				//添加集合
				typeList.Add (type);
			}
		}

		//登录下发
		if (UsePropTool.Instance.toolpropArr.allProp.Count > 0) {
			for (int i = 0; i < UsePropTool.Instance.toolpropArr.allProp.Count; i++) {
				//赋值
				type = UsePropTool.Instance.toolpropArr.allProp [i].propType;
				//添加集合
				typeList.Add (type);
			}
		}

//		for (int i = 0; i < UILockBattery._instance.unlockGunGroup.Count; i++) {
			
//			//UsePropTool.Instance.propType - 3000
//			//判断是否有type
//			if (typeList.Contains (thisVipLevel + 3000)) {
//				if ((UsePropTool.isUsePro == true || UsePropTool.isFirstUsePro == true) && i == thisVipLevel) {
//					{
//						isLock = false;
////						lockImage.enabled = false;
////						maskImage.enabled = false;

//						//获取炮台属性,如果当前装备的炮台样式为已装备的限时道具那么就显示为 已装备 的图片
//						if (PrefabManager._instance.GetLocalGun ().currentGunStyle == thisVipLevel) {
//							btnImage.sprite = onEquipSprite;
//							btnImage.transform.localScale = Vector3.one * 1.35f;
//						} else {
//							//显示装备的图片
//							btnImage.sprite = toEquipSprite;
//							btnImage.transform.localScale = Vector3.one;
//						}
//					} 
//				} 
//			} 
//		}
	}

	public void EquipOrUnlock (int Sroll_index)
	{
		myInfo = (MyInfo)Facade.GetFacade().data.Get(FacadeConfig.USERINFO_MODULE_ID);
		myInfo.ChooseGunTemp = Sroll_index;
		AudioManager._instance.PlayEffectClip (AudioManager.ui_click);
		if (isLock) {
			//ToUnlcok ();
			ToBuyGun();
		} else {
			if (!isEquiping)
				ToEquip (Sroll_index);
		}	
	}

	public void SendChangeGunSpine (int index)
	{
		myInfo = (MyInfo)Facade.GetFacade().data.Get(FacadeConfig.USERINFO_MODULE_ID);
		FishingCommonMsgHandle temp = new FishingCommonMsgHandle ();
		temp.SendChangeCannonStyleRequest (UIBatteryBuy.GunID[myInfo.ChooseGunTemp], DataControl.GetInstance ().GetMyInfo ().userID);
		Debug.LogError(" 发送更换炮台样式：" + UIBatteryBuy.GunID[myInfo.ChooseGunTemp] + " : " + index);
	}

	void ToEquip (int index)
	{
		SendChangeGunSpine (index);
		btnImage.sprite = onEquipSprite;
		btnImage.transform.localScale = Vector3.one * 1.35f;
		//PrefabManager._instance.GetLocalGun ().SetGunSpine (thisVipLevel); //改成接受到服务器权限之后才切换炮台
		UILockBattery._instance.ResetLastEquipGunIcon (thisVipLevel);
		isEquiping = true;

		UILockBattery._instance.ClosePanel ();
	}

	void ToBuyGun()
	{
		GameObject window = Resources.Load("Window/BatteryBuyWindow") as GameObject;
		GameObject.Instantiate(window);
		UILockBattery._instance.ClosePanel();
	}

	void ToUnlcok ()
	{
        if (thisVipLevel>0)
        {
            UILockBattery._instance.ShowVipPanel(thisVipLevel - 1);
        }
        else
        {
            UILockBattery._instance.ShowVipPanel();
        }

		Debug.LogError ("To Unlock");
        //switch (thisVipLevel)
        //{
        //    case 6:
        //    case 7:
        //        //这里是为了移动到特定的炮台
        //        break;
        //    default:
        //        
        //        break;
        //}
       
	}

	void PayCoinUnlockView() {

    }

	public void ResetBtnIcon ()
	{ //从已装备变回装备
		btnImage.sprite = toEquipSprite;
		btnImage.transform.localScale = Vector3.one;
		isEquiping = false;
	}

}
