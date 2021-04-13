using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using AssemblyCSharp;
using UnityEngine.UI;

public class UIHoroReward : MonoBehaviour
{

	public GameObject[] rewardcontainerArray;

	public GameObject Effect;

	public GameObject OpenShow;
	private List<FiProperty> mRewardArray = new List<FiProperty> ();
	// Use this for initialization
	void Start ()
	{
		Invoke ("BoxEffectOver", 1.3f);
		if (GameController._instance != null) {
			if (this.GetComponent<Canvas> ().worldCamera == null) {
				this.GetComponent<Canvas> ().worldCamera = GameObject.FindWithTag (TagManager.uiCamera).GetComponent<Camera> ();
			}
		} else {
			Debug.LogError ("111111");
			if (this.GetComponent<Canvas> ().worldCamera == null) {      
				//Debug.LogError ( "222222" );
				this.GetComponent<Canvas> ().worldCamera = GameObject.FindWithTag ("MainCamera").GetComponent<Camera> ();
			}
		}
	}

	private void BoxEffectOver ()
	{
		OpenShow.gameObject.SetActive (true);
		ShowReward ();
	}

	public void SetRewardData (List<FiProperty> nArray)
	{
		mRewardArray = nArray;
		//Debug.LogError ( "2222222222222222222222" );
	}
	// Update is called once per frame
	void Update ()
	{
		
	}

	void ShowReward ()
	{
		int totalcount = mRewardArray.Count;
		switch (totalcount) {
		case 1:
			rewardcontainerArray [2].gameObject.SetActive (true);
			rewardcontainerArray [0].gameObject.SetActive (false);
			rewardcontainerArray [1].gameObject.SetActive (false);
			ShowReardType (totalcount, rewardcontainerArray [2]);
			break;
		case 2:
			rewardcontainerArray [2].gameObject.SetActive (false);
			rewardcontainerArray [0].gameObject.SetActive (false);
			rewardcontainerArray [1].gameObject.SetActive (true);
			ShowReardType (totalcount, rewardcontainerArray [1]);
			break;
		case 3:
			rewardcontainerArray [2].gameObject.SetActive (false);
			rewardcontainerArray [0].gameObject.SetActive (true);
			rewardcontainerArray [1].gameObject.SetActive (false);
			ShowReardType (totalcount, rewardcontainerArray [0]);
			break;
		default:
			Debug.LogError ("超出越界");
			break;
		}


	}

	void ShowReardType (int totalcount, GameObject content)
	{
		Debug.LogError ("totalcount" + totalcount + "content" + content.name + "jjjjj" + content.transform.GetChildCount ());
		for (int i = 0; i < totalcount; i++) {
			GameObject Day = content.transform.GetChild (i).Find ("Day").gameObject;
			Transform icon = content.transform.GetChild (i).Find ("Icon");
			if (FiPropertyType.TIMELIMTPROTYPE_1 < mRewardArray [i].type && mRewardArray [i].type < FiPropertyType.TIMELIMTPROTYPE_3) {
				Sprite[] itemicon = new Sprite[2];
				itemicon = FiPropertyType.GetTimeSpriteShow (mRewardArray [i].type);
				icon.gameObject.GetComponent<Image> ().sprite = itemicon [0];
				Day.gameObject.SetActive (true);
				Day.gameObject.GetComponent<Image> ().sprite = itemicon [1];
			} else {
				icon.gameObject.GetComponent<Image> ().sprite = FiPropertyType.GetSprite (mRewardArray [i].type);
			}
			int nCountValue = mRewardArray [i].value;
			string nResult = nCountValue.ToString ();
			//			if (nCountValue > 10000) {
			//				nResult = nCountValue / 10000 + "万";
			//			}
			content.transform.GetChild (i).Find ("NumText").gameObject.GetComponent<Text> ().text = nResult;
		}


//		AcceptReward ();
	}

	public void AcceptReward ()
	{
		Debug.LogError ("_______________________________________________time_________________________" + mRewardArray.Count);
		BackpackInfo nBackInfo = (BackpackInfo)Facade.GetFacade ().data.Get (FacadeConfig.BACKPACK_MODULE_ID);
		MyInfo nUserInfo = (MyInfo)Facade.GetFacade ().data.Get (FacadeConfig.USERINFO_MODULE_ID);

		for (int i = 0; i < mRewardArray.Count; i++) {
			FiProperty nReward = mRewardArray [i];
			switch (nReward.type) {
			case FiPropertyType.GOLD:
				nUserInfo.gold += nReward.value;
				Debug.LogError ("--------ReciveButton gold--------" + nReward.value + " / " + nUserInfo.gold);
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
			case FiPropertyType.ROOM_CARD:
				nUserInfo.roomCard += nReward.value;
				Debug.LogError ("--------ReciveButton roomcard--------" + nReward.value + " / " + nUserInfo.roomCard);
				if (Facade.GetFacade ().ui.Get (FacadeConfig.UI_STORE_MODULE_ID) != null) {
					Facade.GetFacade ().ui.Get (FacadeConfig.UI_STORE_MODULE_ID).OnRecvData (FiPropertyType.ROOM_CARD, nUserInfo.roomCard);
				}
				break;
			default:
				Debug.LogError ("--------ReciveButton type--------" + nReward.type + " / " + nReward.value);
				//nBackInfo.Add (nReward.type, nReward.value);
				break;
			}
		}
		IUiMediator nMediator = Facade.GetFacade ().ui.Get (FacadeConfig.UI_BACKPACK_MODULE_ID);
		if (nMediator != null)
			nMediator.OnRecvData (UIBackPack.UPDATE_ALL, null);
	}

	public void OnReceveComnpelte ()
	{
		AcceptReward ();
		Destroy (gameObject);
	}


}
