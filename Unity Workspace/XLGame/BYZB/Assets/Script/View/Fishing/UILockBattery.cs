using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System;

public class UILockBattery : MonoBehaviour
{

	public  Sprite toEquipSprite;
	public  Sprite unlockSprite;
	public  Sprite onEquipSprite;

	public static UILockBattery _instance;

	public List<UnlockGunGrid> unlockGunGroup;

	public List<Transform> unlockGunGroupobj;

	public List<UnlockGunGrid> UnlockGunGridCanUselist;

	public List<UnlockGunGrid> UnlockGunGridNoUselist;
	public int lastEquipIndex = -1;

	public GameObject vipPanel;

	void Awake ()
	{
		if (null == _instance)
			_instance = this;
	}

	void Start ()
	{
//		ShowList ();
		//	Dont9DestroyOnLoad (this.gameObject);
		lastEquipIndex = PrefabManager._instance.GetLocalGun ().currentGunStyle;
		for (int i = 0; i < unlockGunGroup.Count; i++) {
			unlockGunGroup [i].Init (i);
		}
	}

	public void ClosePanel ()
	{
		//this.gameObject.SetActive (false);
		AudioManager._instance.PlayEffectClip (AudioManager.ui_click);
		_instance = null;
		Destroy (this.gameObject);
	}

	public void ShowList ()
	{
		for (int i = 0; i < unlockGunGroup.Count; i++) {
			unlockGunGroupobj [i] = unlockGunGroup [i].gameObject.transform;
		}
		Debug.LogError ("DataControl.GetInstance ().GetMyInfo ().levelVip" + DataControl.GetInstance ().GetMyInfo ().levelVip);
		for (int i = 0; i < unlockGunGroup.Count - 2; i++) {
			if (DataControl.GetInstance ().GetMyInfo ().levelVip >= unlockGunGroup [2 + i].thisVipLevel) {
				UnlockGunGridCanUselist.Add (unlockGunGroup [i + 2]);

			} else {
				UnlockGunGridNoUselist.Add (unlockGunGroup [i + 2]);
			}
		}
		for (int i = 0; i < UnlockGunGridCanUselist.Count; i++) {
			Debug.LogError (UnlockGunGridCanUselist [i].thisVipLevel);

		}
		if (UnlockGunGridCanUselist.Count == 0) {
			SetArray (UnlockGunGridNoUselist);
		} else if (UnlockGunGridNoUselist.Count == 0) {
			SetArray (UnlockGunGridCanUselist);
		} else {
			SetArray (UnlockGunGridCanUselist, UnlockGunGridNoUselist);
		}
//		for (int i = 0; i < unlockGunGroup.Count; i++) {
//			Debug.LogError (unlockGunGroup [i].thisVipLevel);
//		}
	}

	private void SetArray (List<UnlockGunGrid> list, List<UnlockGunGrid> list1 = null)
	{
		if (list1 != null) {
			for (int i = 0; i < unlockGunGroup.Count - 2; i++) {
				
				if (i >= list.Count) {
					Debug.LogError ("list1 [i - list.Count].gameObject.transform.localPosition" + list1 [i - list.Count].gameObject.transform.localPosition);

					unlockGunGroup [i + 2].gameObject.transform.localPosition = list1 [i - list.Count].gameObject.transform.localPosition;
					unlockGunGroup [i + 2] = list1 [i - list.Count];
//					unlockGunGroup [i + 2].gameObject.transform.position = unlockGunGroupobj [i + 2].position;
				} else {
					unlockGunGroup [i + 2].gameObject.transform.localPosition = list [i].gameObject.transform.localPosition;
					unlockGunGroup [i + 2] = list [i];
//					unlockGunGroup [i + 2].gameObject.transform.position = unlockGunGroupobj [i + 2].position;
				}
			}

		} else {
			for (int i = 0; i < list.Count; i++) {
				unlockGunGroup [i + 2] = list [i];
			}
		}

	}

	public void ResetLastEquipGunIcon (int currentGunIndex)
	{ //参数代表新切换至的炮台等级
		for (int i = 0; i < unlockGunGroup.Count; i++) {
			if (lastEquipIndex == unlockGunGroup [i].thisVipLevel) {
				unlockGunGroup [i].ResetBtnIcon ();
				break;
			}
		}
		lastEquipIndex = currentGunIndex;
	}

	public void ShowVipPanel (int index=1)
	{
        GameObject vipWindowClone= GameObject.Instantiate (vipPanel);
        vipWindowClone.transform.Find("Weapon_Canvas/ScrollRect").GetComponent<StoreRectHelper>().JumpCurrentGun(index);

		ClosePanel ();
	}
}
