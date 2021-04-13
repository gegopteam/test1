using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using AssemblyCSharp;
using UnityEngine.UI;

public enum RewardSignType
{
	NONE = -1,
	CONTINUEUNIT,
	VIPUNIT,
	DAILYUNIT
}

public class UISignReward : MonoBehaviour
{

	//public Sprite[] RewardIcon; // 0 金币 1 钻石 2 鱼雷 3 vip
	public UIReward.OnRewardClose OnRewardCompelete;

	public Transform rewardparent;

	public GameObject rewardobj;

	public Image rewardtype;

	// Use this for initialization
	void Start ()
	{
		GameObject sign = GameObject.FindGameObjectWithTag ("MainCamera");
		//Debug.Log (sign.name);
		Camera signCamera = sign.GetComponent<Camera> ();
		Canvas mainCanvas = transform.GetComponentInChildren<Canvas> ();
		mainCanvas.worldCamera = signCamera;
	}

	public void SetRewardData (List<FiProperty> nRewards, bool bContinueReward = false)
	{
		for (int i = 0; i < nRewards.Count; i++) {
			Debug.LogError ("s" + nRewards [i].type + "sss" + nRewards [i].value);
		}
			
		for (int i = 0; i < nRewards.Count; i++) {
			if (bContinueReward) {
				SetUnitData (InstainReward (rewardobj), nRewards [i], RewardSignType.CONTINUEUNIT);
			} else {
				if (nRewards [i].type >= 5001 && nRewards [i].type <= 5009) {
					Debug.LogError ("111111");
					GameObject obj = InstainReward (rewardobj);
					SetUnitData (obj, nRewards [i], RewardSignType.VIPUNIT);
				} else {
					Debug.LogError ("2222222");
					SetUnitData (InstainReward (rewardobj), nRewards [i], RewardSignType.DAILYUNIT);
				}

			}
		}
//
//
		BackpackInfo nBackInfo = (BackpackInfo)Facade.GetFacade ().data.Get (FacadeConfig.BACKPACK_MODULE_ID);
		MyInfo nUserInfo = (MyInfo)Facade.GetFacade ().data.Get (FacadeConfig.USERINFO_MODULE_ID);

		for (int i = 0; i < nRewards.Count; i++) {
			FiProperty nReward = nRewards [i];
			Debug.LogError ("--------ReciveButton gold--------" + nReward.value + " / " + nUserInfo.gold);
			switch (nReward.type) {
			case FiPropertyType.GOLD:
				nUserInfo.gold += nReward.value;
//				Debug.LogError ( "--------ReciveButton gold--------" + nReward.value + " / " + nUserInfo.gold );
				if (Facade.GetFacade ().ui.Get (FacadeConfig.UI_STORE_MODULE_ID) != null) {
					Facade.GetFacade ().ui.Get (FacadeConfig.UI_STORE_MODULE_ID).OnRecvData (FiPropertyType.GOLD, nUserInfo.gold);
				}
				break;
			case FiPropertyType.DIAMOND:
				nUserInfo.diamond += nReward.value;
				Debug.LogError ("--------ReciveButton diamond--------" + nReward.value + " / " + nUserInfo.diamond);
				if (Facade.GetFacade ().ui.Get (FacadeConfig.UI_STORE_MODULE_ID) != null) {
					Facade.GetFacade ().ui.Get (FacadeConfig.UI_STORE_MODULE_ID).OnRecvData (FiPropertyType.DIAMOND, nUserInfo.diamond);
				}
				break;
			default:
				Debug.LogError ("--------ReciveButton type--------" + nReward.type + " / " + nReward.value);
				nBackInfo.Add (nReward.type, nReward.value);
				break;
			}
		}
	}

	public void OnRecvUnits ()
	{
		if (OnRewardCompelete != null) {
			OnRewardCompelete ();
		}
		Destroy (gameObject);
	}

	public GameObject InstainReward (GameObject reward)
	{
		GameObject obj = GameObject.Instantiate (reward)as GameObject;
		obj.transform.SetParent (rewardparent);
		obj.transform.localScale = new Vector3 (1.9f, 1.9f, 1.9f);
		obj.transform.localPosition = Vector3.zero;
		obj.gameObject.SetActive (true);
		return obj;
	}

	void SetUnitData (GameObject nTarget, FiProperty nDataIn, RewardSignType type)
	{

		switch (type) {
		case RewardSignType.CONTINUEUNIT:
			nTarget.transform.Find ("Title").GetComponent<Image> ().sprite = UIHallTexturers.instans.SignRewardType [0];
			break;
		case RewardSignType.DAILYUNIT:
			
			nTarget.transform.Find ("Title").GetComponent<Image> ().sprite = UIHallTexturers.instans.SignRewardType [1];
			break;
		case RewardSignType.VIPUNIT:
			nTarget.transform.Find ("Title").GetComponent<Image> ().sprite = UIHallTexturers.instans.SignRewardType [2];
			nTarget.transform.Find ("Title/Vip").GetComponent<Image> ().gameObject.SetActive (true);
			break;
		}
		nTarget.transform.Find ("Text").GetComponent<Text> ().text = "X" + nDataIn.value.ToString ();
		if (FiPropertyType.TIMELIMTPROTYPE_1 < nDataIn.type && nDataIn.type < FiPropertyType.TIMELIMTPROTYPE_3) {
			Sprite[] itemicon = new Sprite[2];
			itemicon = FiPropertyType.GetTimeSpriteShow (nDataIn.type);
			nTarget.transform.Find ("Content/day").GetComponent<Image> ().sprite = itemicon [1];
			nTarget.transform.Find ("Content/day").gameObject.SetActive (true);
			nTarget.transform.Find ("Content").GetComponent<Image> ().sprite = itemicon [0];
		} else {
			nTarget.transform.Find ("Content").GetComponent<Image> ().sprite = FiPropertyType.GetSprite (nDataIn.type);
		}

	}

	// Update is called once per frame
	void Update ()
	{
	
	}
}
