using UnityEngine;
using System.Collections.Generic;
using UnityEngine.UI;
using AssemblyCSharp;
using System.Collections;


/// <summary>
/// User interface reward.领取奖励
/// </summary>
public class UIReward : MonoBehaviour
{


	public delegate void OnRewardClose ();

	public OnRewardClose OnRewardCompelete;

	//public Image rewardImage;
	//public GameObject textImage;

	public Text textObj;
	public string toolName;
	public Animation ani;
	public Image title;
	public GameObject RewardContainer;

	public static bool isClose;

	public bool IsPurReward = false;
	private GameObject reward;
	private Camera rewardCamera;
	private Canvas mainCanvas;
	public bool IsAutoAccept = true;
	public GameObject Hongli;

	public Text ShwoHongliMoney;

	public Image rewardtype;

	private List<FiProperty> mRewardArray = new List<FiProperty> ();

	//public bool bAddToBackPack = true;

	public void SetTitleImage (Sprite titleImage)
	{
		title.sprite = titleImage;
	}

	public void SetRewardData (List<FiProperty> nArray)
	{
		mRewardArray = nArray;
		//Debug.LogError ( "2222222222222222222222" );
	}

	IEnumerator ReceiveGift ()
	{
		yield return new WaitForSeconds (3f);
		Continue ();
		IsPurReward = false;
	}

	IEnumerator ReceiveGiftPurReward ()
	{
		yield return new WaitForSeconds (13f);
		Continue ();
		IsPurReward = false;
	}

	public void AddRewardData (FiProperty nValue)
	{
		mRewardArray.Add (nValue);
	}

	public void ShowRewrdType (int Rewradtype)
	{
		rewardtype.sprite = FiPropertyType.GetSpriteRewardType (Rewradtype);
	}

	public void SetHoroReaward (long HongliMoney)
	{
		if (HongliMoney > 0) {
			Hongli.gameObject.SetActive (true);
			ShwoHongliMoney.text = "x" + HongliMoney.ToString ();
		} else {
			Hongli.gameObject.SetActive (false);
		}

	}

	void ShowHoroRewardUnits ()
	{
		GameObject nWindow = Resources.Load ("Window/RewardUnitHoro") as GameObject;
		int nTotalCount = mRewardArray.Count > 5 ? 5 : mRewardArray.Count;
		for (int i = 0; i < nTotalCount; i++) {
			GameObject nUnit = Instantiate (nWindow) as GameObject;
			nUnit.name = "entity" + i;
			nUnit.transform.SetParent (RewardContainer.transform);
			nUnit.transform.localScale = new Vector3 (1.35f, 1.35f, 1.35f);
			Transform icon = nUnit.transform.Find ("Icon");
			GameObject Day = nUnit.transform.Find ("Day").gameObject;

//			GameObject frame = nUnit.transform.Find ("frame").gameObject;
			if (FiPropertyType.TIMELIMTPROTYPE_1 < mRewardArray [i].type && mRewardArray [i].type < FiPropertyType.TIMELIMTPROTYPE_3) {
				Sprite[] itemicon = new Sprite[2];
				itemicon = FiPropertyType.GetTimeSpriteShow (mRewardArray [i].type);
				icon.gameObject.GetComponent<Image> ().sprite = itemicon [0];
				Day.gameObject.SetActive (true);
				Day.gameObject.GetComponent<Image> ().sprite = itemicon [1];
			} else {
				icon.gameObject.GetComponent<Image> ().sprite = FiPropertyType.GetSprite (mRewardArray [i].type);
			}

			nUnit.GetComponent<RectTransform> ().localPosition -= new Vector3 (0, 0, nUnit.GetComponent<RectTransform> ().localPosition.z);
			int nCountValue = mRewardArray [i].value;
			string nResult = nCountValue.ToString ();
			//			if (nCountValue > 10000) {
			//				nResult = nCountValue / 10000 + "万";
			//			}
			nUnit.transform.Find ("NumText").gameObject.GetComponent<Text> ().text = nResult;
		}
	}
	//最多显示4个奖励道具
	void ShowRewardUnits ()
	{
		//Debug.LogError ( "display rewrd units!!!" );
		GameObject nWindow = Resources.Load ("Image/tools/RewardUnit") as GameObject;
		int nTotalCount = mRewardArray.Count > 5 ? 5 : mRewardArray.Count;
		for (int i = 0; i < nTotalCount; i++) {
			GameObject nUnit = Instantiate (nWindow) as GameObject;
			nUnit.name = "entity" + i;
			nUnit.transform.SetParent (RewardContainer.transform);
			nUnit.transform.localScale = new Vector3 (1.35f, 1.35f, 1.35f);
			Transform icon = nUnit.transform.Find ("Icon");
			GameObject Day = nUnit.transform.Find ("Day").gameObject;

			GameObject frame = nUnit.transform.Find ("frame").gameObject;
			if (FiPropertyType.TIMELIMTPROTYPE_1 < mRewardArray [i].type && mRewardArray [i].type < FiPropertyType.TIMELIMTPROTYPE_3) {
				Sprite[] itemicon = new Sprite[2];
				itemicon = FiPropertyType.GetTimeSpriteShow (mRewardArray [i].type);
				icon.gameObject.GetComponent<Image> ().sprite = itemicon [0];
				Day.gameObject.SetActive (true);
				Day.gameObject.GetComponent<Image> ().sprite = itemicon [1];
			} else {
				icon.gameObject.GetComponent<Image> ().sprite = FiPropertyType.GetSprite (mRewardArray [i].type);
			}


			if (mRewardArray [i].type == 1001) {//当是钻石时，微调图片位置
				icon.GetComponent<Image> ().SetNativeSize ();
				icon.GetComponent<RectTransform> ().anchoredPosition += Vector2.right * 3;
				icon.GetComponent<RectTransform> ().localScale = Vector3.one * 0.5f;
				//frame.SetActive (false);
			} else if (mRewardArray [i].type == 1000) {//当是金币时，微调图片位置
				icon.GetComponent<Image> ().SetNativeSize ();
				icon.GetComponent<RectTransform> ().anchoredPosition += Vector2.up * 2;
				icon.GetComponent<RectTransform> ().localScale = Vector3.one * 0.35f;
				//frame.SetActive (false);
			} else if (mRewardArray [i].type >= 2004 && mRewardArray [i].type <= 2009) {
				icon.GetComponent<RectTransform> ().localScale = Vector3.one * 0.45f;
				//frame.SetActive (false);
			}
				
				

			nUnit.GetComponent<RectTransform> ().localPosition -= new Vector3 (0, 0, nUnit.GetComponent<RectTransform> ().localPosition.z);
			int nCountValue = mRewardArray [i].value;
			string nResult = nCountValue.ToString ();
//			if (nCountValue > 10000) {
//				nResult = nCountValue / 10000 + "万";
//			}
			nUnit.transform.Find ("NumText").gameObject.GetComponent<Text> ().text = nResult;
			//Debug.LogError ( nUnit.transform.localScale.x + " / " + nUnit.transform.localScale.y  );
		}
	}

	// Use this for initialization
	void Start ()
	{
		isClose = false;
		//Debug.LogError ( "1111111111111111111111" );
		/*reward = GameObject.FindGameObjectWithTag ("MainCamera");
		Debug.Log (reward.name);
		rewardCamera = reward.GetComponent<Camera> ();
		mainCanvas = transform.GetComponentInChildren<Canvas> ();
		mainCanvas.worldCamera = rewardCamera;
		transform.SetAsFirstSibling ();*/
		if (GameController._instance != null) {
			if (this.GetComponent<Canvas> ().worldCamera == null) {
				this.GetComponent<Canvas> ().worldCamera = GameObject.FindWithTag (TagManager.uiCamera).GetComponent<Camera> ();
			}
		} else {
			//Debug.LogError ( "111111" );
			if (this.GetComponent<Canvas> ().worldCamera == null) {      
				//Debug.LogError ( "222222" );
				this.GetComponent<Canvas> ().worldCamera = GameObject.FindWithTag ("MainCamera").GetComponent<Camera> ();
			}
		}
    

//		ShowRewardUnits ();
//		ShowRewardType (0);
		AudioManager._instance.PlayEffectClip (AudioManager.effect_getReward);

		if (IsAutoAccept) {
            //Joey
			ShowRewardUnits ();
			if (IsPurReward) {
				Debug.LogError ("11111111111");
				StartCoroutine (ReceiveGiftPurReward ());
			} else {
				Debug.LogError ("222222222222");
				StartCoroutine (ReceiveGift ());
			}
		} else {
			ShowHoroRewardUnits ();
		}

	}


	public void ShowRewardType (int  type)
	{
		IsPurReward = true;
		rewardtype.sprite = FiPropertyType.GetSpriteRewardType (type);
	}

	public void Continue ()
	{
		AcceptRewards ();
		// 还有没能显示的奖励
		if (mRewardArray.Count > 0) {
			ShowRewardUnits ();
		} else {
			ReciveButton ();
			Debug.LogError ("recive");
		}
	}


	void AcceptRewards ()
	{
		Debug.LogError ("_______________________________________________time_________________________" + mRewardArray.Count);
		BackpackInfo nBackInfo = (BackpackInfo)Facade.GetFacade ().data.Get (FacadeConfig.BACKPACK_MODULE_ID);
		MyInfo nUserInfo = (MyInfo)Facade.GetFacade ().data.Get (FacadeConfig.USERINFO_MODULE_ID);

		int nCount = RewardContainer.transform.childCount;
		Debug.LogError ("ncount" + nCount);
		List<GameObject> nGiftList = new List<GameObject> ();
		for (int i = 0; i < nCount; i++) {
			nGiftList.Add (RewardContainer.transform.GetChild (i).gameObject);
		}
		RewardContainer.transform.DetachChildren ();
		int nCount1 = RewardContainer.transform.childCount;
		Debug.LogError ("ncount1" + nCount1);
		Debug.LogError ("ncount" + nCount);
		while (nGiftList.Count > 0) {
			GameObject nDeleteUnit = nGiftList [0];
			nGiftList.RemoveAt (0);
			Destroy (nDeleteUnit);
			Debug.LogError ("deleted!!!!!!!!");
		}

		for (int i = 0; i < 5; i++) {
			if (mRewardArray.Count > 0) {
				FiProperty nReward = mRewardArray [0];
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
					//Joey
					//if (nReward.type == 8072)
					//	nReward.value = 1;
                    if(nReward.type!=8072)
					    nBackInfo.Add (nReward.type, nReward.value);
					break;
				}
				mRewardArray.RemoveAt (0);
			} else {
				break;
			}
		}
		IUiMediator nMediator = Facade.GetFacade ().ui.Get (FacadeConfig.UI_BACKPACK_MODULE_ID);
		if (nMediator != null)
			nMediator.OnRecvData (UIBackPack.UPDATE_ALL, null);
	}

	public void ReciveButton ()
	{
		
		if (OnRewardCompelete != null)
			OnRewardCompelete ();

		if (isClose) {
			creatWarningWindow ();
		}

		Destroy (this.gameObject);
	}

	public static void creatWarningWindow ()
	{
		PurchaseMsgHandle.CreatWarningWindow ();
	}

	void OnDestroy()
	{
		Debug.LogError(" -------- UIReward -------- OnDestroy -------- ");
		UIHallCore.isNeedToUpdate = true;
		Debug.Log("UIHallCore.isNeedToUpdate = " + UIHallCore.isNeedToUpdate);
	}
}
