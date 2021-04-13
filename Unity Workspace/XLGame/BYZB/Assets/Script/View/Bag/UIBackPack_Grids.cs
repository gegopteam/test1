using System;
using UnityEngine.UI;
using UnityEngine;
using AssemblyCSharp;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine.Events;

public class UsePropTool
{
	//	//道具ID
	//	public	 int propID;
	//	//道具类型
	//	public 	int propType;
	//	//道具使用时间
	//	public	 long useTime;
	//	//道具剩余时间
	//	public	 long remainTime;
	//	//道具时间
	//	public int propTime;

	public List<FiUserGetAllPropTimeResponse> proTimeRequest = new List<FiUserGetAllPropTimeResponse> ();

	public FiUseProTimeArr toolpropArr;
	//是否使用道具
	public static bool isUsePro = false;
	public static bool isFirstUsePro = false;
	public static bool isDelUsePro = false;
	private static UsePropTool instance;

	public static UsePropTool Instance {
		get {
			if (instance == null) {
				instance = new UsePropTool ();
			}
			return instance;
		}
	}

}


public class UIBackPack_Grids : MonoBehaviour
{
    //顯示全部
	public const int TOTAL = 0;
    //顯示砲台
	public const int CANNON = 2;
    //顯示道具
	public const int TOOL = 1;

	Image[] mGridUnitsArray;
	//已经装备好的炮台
	GameObject mEquitCannon;
	//上次选中的道具
	GameObject mSelectedTool;

	int nLastSelectUnitId = -1;

	int mSelectGridType = TOTAL;

	string timeTmp;

	public static UIBackPack_Grids Instance;

	float timer = 0;
	float timerInterval = 1f;

	public UIBackPack_Grids ()
	{
		
	}

	public void ResetSelected ()
	{
		nLastSelectUnitId = -1;
	}

	void Awake ()
	{
		Instance = this;
	}

	void OnDestroy ()
	{
		Instance = null;
	}

	void Start ()
	{
		mGridUnitsArray = GetComponentsInChildren<Image> ();
		Debug.Log ("mGridUnitsArray name " + mGridUnitsArray [0].name);
		UpdateUnits (TOTAL);
	}

	void Update ()
	{
		timer += Time.deltaTime;
//		if (UsePropTool.isFirstUsePro) {
//			if (timer >= timerInterval) {
////				Debug.Log ("UsePropTool.Instance.remainTime = " + UsePropTool.Instance.remainTime);
//				UsePropTool.Instance.remainTime += (int)timer;
//				timer = 0;
//			}
//		}
		if (UsePropTool.Instance.proTimeRequest.Count > 0) {
			if (timer >= timerInterval) {
				//				Debug.Log ("UsePropTool.Instance.remainTime = " + UsePropTool.Instance.remainTime);
				for (int i = 0; i < UsePropTool.Instance.proTimeRequest.Count; i++) {
//					if (nLastSelectUnitId == UsePropTool.Instance.proTimeRequest [i].propID) {
					UsePropTool.Instance.proTimeRequest [i].remainTime += (int)timer;
//					}
					timer = 0;
				}
			}
		}
		//登录下发
		if (UsePropTool.Instance.toolpropArr.allProp.Count > 0) {
			if (timer >= timerInterval) {
				for (int i = 0; i < UsePropTool.Instance.toolpropArr.allProp.Count; i++) {
//					if (nLastSelectUnitId == UsePropTool.Instance.toolpropArr.allProp [i].propID) {
					UsePropTool.Instance.toolpropArr.allProp [i].remainTime += (int)timer;
//					Debug.LogError ("UsePropTool.Instance.toolpropArr.allProp [i].remainTime  = " + UsePropTool.Instance.toolpropArr.allProp [i].remainTime);
//					}
				}
				timer = 0;
			}
		}
	}

	void ClearUnits ()
	{
		Button[] buttons = GetComponentsInChildren<Button> ();
		for (int i = 0; i < buttons.Length; i++) {
			Destroy (buttons [i].gameObject);
		}
	}

	GameObject LoadBagUnit (int nUnitId, int nCount, long propTime, int bagType)
	{
		GameObject nInstance = Resources.Load ("MainHall/BackPack/BagUnits") as GameObject;
		GameObject nEntity = Instantiate (nInstance) as GameObject;
		nEntity.transform.localPosition = new Vector3 (-1000000f, 1000000f, 0f);
		nEntity.transform.localScale = new Vector3 (1f, 1f, 1f);
		//如果是鱼雷道具
		if (nUnitId >= FiPropertyType.TORPEDO_MINI && nUnitId <= FiPropertyType.TORPEDO_NUCLEAR) {
			//显示鱼雷道具特有的紫色背景
			nEntity.transform.Find ("Back").gameObject.SetActive (true);
			int nIndex = nUnitId - FiPropertyType.TORPEDO_MINI;
			if (nIndex <= UIHallTexturers.instans.IconTorpedo.Length) {
				nEntity.transform.Find ("Content").GetComponent<Image> ().sprite = UIHallTexturers.instans.IconTorpedo [nIndex];
			}
			nEntity.transform.Find ("TxtCount").GetComponent<Text> ().text = nCount + "";
		}
		//如果炮台道具，不显示数量
		else if (nUnitId >= FiPropertyType.CANNON_VIP0 && nUnitId <= FiPropertyType.CANNON_VIP9) {
			nEntity.transform.Find ("TxtCount").gameObject.SetActive (false);
			int nIndex = nUnitId - FiPropertyType.CANNON_VIP0;
			if (nIndex <= UIHallTexturers.instans.IconCannon.Length) {
				nEntity.transform.Find ("Content").GetComponent<Image> ().sprite = UIHallTexturers.instans.IconCannon [nIndex];

				MyInfo nInfo = (MyInfo)Facade.GetFacade ().data.Get (FacadeConfig.USERINFO_MODULE_ID);
				if (nInfo.cannonStyle == nUnitId)
					nEntity.transform.Find ("Content").Find ("ImageOn").gameObject.SetActive (true);
			}
		}//如果是限时道具
		else if (nUnitId >= FiPropertyType.TIMELIMTPROTYPE_1 && nUnitId <= FiPropertyType.TIMELIMTPROTYPE_3) {
			MyInfo nInfo = (MyInfo)Facade.GetFacade ().data.Get (FacadeConfig.USERINFO_MODULE_ID);
			GameObject go = nEntity.transform.Find ("DayCount").gameObject;
			//go.GetComponent <Text> ().text = (UsePropTool.Instance.propTime / 60 / 24).ToString () + "天";
			go.SetActive (true);
			GameObject timeCount = nEntity.transform.Find ("PropBG").gameObject;
			Text timeCountText;
			timeCountText = timeCount.transform.Find ("TimeText").GetComponent <Text> ();
			//第一次签到领取
			if (UsePropTool.Instance.proTimeRequest.Count > 0) {
				for (int i = 0; i < UsePropTool.Instance.proTimeRequest.Count; i++) {
					if (nUnitId == UsePropTool.Instance.proTimeRequest [i].propID) {
//						Debug.Log ("UsePropTool.Instance.proTimeRequest1 = ");
//						Debug.Log ("UsePropTool.Instance.proTimeRequest [i].propTime = " + UsePropTool.Instance.proTimeRequest [i].propTime);
//						Debug.Log ("UsePropTool.Instance.proTimeRequest [i].remainTime= " + (UsePropTool.Instance.proTimeRequest [i].remainTime));
						StartCoroutine (UseTimeManager (UsePropTool.Instance.proTimeRequest [i].propTime * 60, timeCountText.text));
						timeCount.SetActive (true);
						timeCountText.text = timeTmp;
						go.SetActive (false);
						if (nInfo.cannonStyle == UsePropTool.Instance.proTimeRequest [i].propType)
							nEntity.transform.Find ("Content").Find ("ImageOn").gameObject.SetActive (true);
						if (nUnitId > 8010 && nUnitId < 8999) {
							if (nInfo.cannonBabetteStyle == (UsePropTool.Instance.proTimeRequest [i].propType % 10)) {
								nEntity.transform.Find ("Content").Find ("ImageOn").gameObject.SetActive (true);
							}
						}
					}
				}
			}
			//登录下发
			if (UsePropTool.Instance.toolpropArr.allProp.Count > 0) {
				for (int i = 0; i <= UsePropTool.Instance.toolpropArr.allProp.Count - 1; i++) {
					if (nUnitId == UsePropTool.Instance.toolpropArr.allProp [i].propID) {
						if (nInfo.cannonStyle == bagType) {
							nEntity.transform.Find ("Content").Find ("ImageOn").gameObject.SetActive (true);
						} else if (nUnitId > 8010 && nUnitId < 8999) {
							if (nInfo.cannonBabetteStyle == (bagType % 10)) {
//								Debug.LogError ("nInfo.cannonBabetteStyle = " + nInfo.cannonBabetteStyle);
//								Debug.LogError ("bagType % 10 = " + (bagType % 10));
								nEntity.transform.Find ("Content").Find ("ImageOn").gameObject.SetActive (true);
							}
						}
//						Debug.LogError (" i = " + i + "UsePropTool.Instance.toolpropArr.allProp [i].propID = " + UsePropTool.Instance.toolpropArr.allProp [i].propID);
//						Debug.LogError ("propTime * 60 = " + propTime * 60);
//						Debug.LogError ("UsePropTool.Instance.toolpropArr.allProp [i].remainTime = " + UsePropTool.Instance.toolpropArr.allProp [i].remainTime);
						StartCoroutine (UseTimeManager ((propTime * 60 - UsePropTool.Instance.toolpropArr.allProp [i].remainTime), timeCountText.text));
						timeCountText.text = timeTmp;
						timeCount.SetActive (true);
						go.SetActive (false);
					}
				}
			}
				
			nEntity.transform.Find ("Content").GetComponent<Image> ().sprite = FiPropertyType.GetTimeSpriteShow (nUnitId) [0];
			//后期如果需要加入天数的话,用下面注释的赋值方法
			//nEntity.transform.Find ("天数").GetComponent<Image> ().sprite = FiPropertyType.GetTimeSpriteShow (nUnitId) [1];
			if (nCount <= 1) {
				nEntity.transform.Find ("TxtCount").GetComponent<Text> ().text = "";
			} else {
				nEntity.transform.Find ("TxtCount").GetComponent<Text> ().text = nCount + "";
			}
		}
		//普通道具了
		else if (nUnitId >= FiPropertyType.FISHING_EFFECT_FREEZE && nUnitId <= FiPropertyType.FISHING_EFFECT_SUMMON) {
			int nIndex = nUnitId - FiPropertyType.FISHING_EFFECT_FREEZE;
			if (nIndex <= UIHallTexturers.instans.IconProperty.Length) {
				nEntity.transform.Find ("Content").GetComponent<Image> ().sprite = UIHallTexturers.instans.IconProperty [nIndex];
			}
			nEntity.transform.Find ("TxtCount").GetComponent<Text> ().text = nCount + "";
		} else if (nUnitId >= FiPropertyType.GIFT_VIP1 && nUnitId <= FiPropertyType.GIFT_VIP9) {
			nEntity.transform.Find ("Content").GetComponent<Image> ().sprite = UIHallTexturers.instans.IconGift [0];
			nEntity.transform.Find ("TxtCount").GetComponent<Text> ().text = nCount + "";
		} else if (nUnitId == FiPropertyType.GIFT_TORPEDO) {
			nEntity.transform.Find ("Content").GetComponent<Image> ().sprite = UIHallTexturers.instans.IconGift [1];
			nEntity.transform.Find ("TxtCount").GetComponent<Text> ().text = nCount + "";
		} else if (nUnitId == FiPropertyType.ROOM_CARD) {
			nEntity.transform.Find ("Content").GetComponent<Image> ().sprite = UIHallTexturers.instans.IconGift [2];
			nEntity.transform.Find ("TxtCount").GetComponent<Text> ().text = nCount + "";
		}
		return nEntity;
	}

	void OnPropertyClick (GameObject nClickTarget, int bagType)
	{
		if (mSelectedTool != null) {
			mSelectedTool.transform.Find ("ImgBorder").gameObject.SetActive (false);
		}
		nClickTarget.transform.Find ("ImgBorder").gameObject.SetActive (true);
		mSelectedTool = nClickTarget;

		try {
//			Debug.LogError ("OnPropertyClick bagType = " + bagType);
			int nUnitId = int.Parse (mSelectedTool.name);
			nLastSelectUnitId = nUnitId;
			UIBackPack_Brief.instance.UpdateInfo (nUnitId, mSelectedTool.transform.Find ("Content").GetComponent<Image> ().sprite, bagType);
			//可以通过这里来获取我所选中的id 还有类型
		} catch {
			Debug.LogError ("----------error-----------" + mSelectedTool.name);
		}
	}

	public void Refresh ()
	{
		UpdateUnits (mSelectGridType);
	}


	IEnumerator UseTimeManager (long timer, string time)
	{
		while (true) {
			if (timer > 0) {
				timer -= 1;
				time = TimeCountDownControl (timer);
				timeTmp = time;
				if (timer == 0) {
					time = TimeCountDownControl (0);
					timeTmp = time;
				}
			} else {
				break;
			}
			yield return new WaitForSeconds (1f);
		}
	}

	/// <summary>
	/// 时间转换
	/// </summary>
	/// <returns>The count down control.</returns>
	/// <param name="time">Time.</param>
	/// <param name="textDown">Text down.</param>
	public  string  TimeCountDownControl (long time)
	{
		string tmp;
		long nDay = time / (3600 * 24);
		long nHours = (time / 3600) - (nDay * 24);
		long nMinutes = (time / 60) - (nHours * 60) - (nDay * 60 * 24);
		long nSecond = time - nMinutes * 60 - nHours * 3600 - nDay * 3600 * 24;

		string dayStr = (nDay < 10) ? "" + nDay : nDay.ToString ();
		string hourStr = (nHours < 10) ? "0" + nHours : nHours.ToString ();
		string minuteStr = (nMinutes < 10) ? "0" + nMinutes : nMinutes.ToString ();
		string secondStr = (nSecond < 10) ? "0" + nSecond : nSecond.ToString ();

		if (nDay >= 1) {
			return	tmp = dayStr + "天" + hourStr + "時";
		} else if (nHours < 1) {
			return	tmp = "僅剩" + minuteStr + "分";
		} else if (nDay < 1) {
			return	tmp = hourStr + "時" + minuteStr + "分";//+ ":" + secondStr;
		} else {
			return "";
		}
	}

	//更新背包中的道具 , @nUnitType  需要更新的类型， 0 全部显示 2 显示炮台，1 显示道具
	public void UpdateUnits (int nUnitType)
	{
		mSelectGridType = nUnitType;
		ClearUnits ();
		BackpackInfo nBackInfo = (BackpackInfo)Facade.GetFacade ().data.Get (FacadeConfig.BACKPACK_MODULE_ID);
		MyInfo nMyInfo = (MyInfo)Facade.GetFacade ().data.Get (FacadeConfig.USERINFO_MODULE_ID);
		List<FiBackpackProperty> nBackArray = nBackInfo.getInfoArray ();

		if (nLastSelectUnitId != -1) {
			if (nBackInfo.Get (nLastSelectUnitId).count == 0) {
				nLastSelectUnitId = -1;
			}
		}

		//过滤数据 TOTAL 0 CANNON 2
		//这里是做一个筛选,如果玩家点击的为炮台或者道具,就将不相关的道具隐藏
		if (nUnitType != TOTAL) {
			for (int nIndex = nBackArray.Count - 1; nIndex >= 0; nIndex--) {
				if (nUnitType == CANNON) {
					if (nBackArray[nIndex].type == nUnitType || (nBackArray[nIndex].type >= 3000 && nBackArray[nIndex].type <= 3010))
					{
						
					}
					else {
						nBackArray.RemoveAt(nIndex);
					}
				}
				else {
					if (nBackArray[nIndex].type != nUnitType)
					{
						nBackArray.RemoveAt(nIndex);
					}
				}
			}
		}

		//这里获取到所有的Grid,在Start中通过Image获取到,然后依次生成背包子物体
		for (int i = 0; i < nBackArray.Count; i++) {
			Transform nCurrent = mGridUnitsArray[i].gameObject.GetComponent<RectTransform>();
			GameObject nTargetEntity;
			Debug.Log("nBackArray [i].type = " + nBackArray[i].type);
			nTargetEntity = LoadBagUnit(nBackArray[i].id, nBackArray[i].count, nBackArray[i].propTime, nBackArray[i].type);
			nTargetEntity.transform.SetParent(nCurrent);
			nTargetEntity.transform.localPosition = new Vector3(-1.5f, 1.5f, 0f);
			nTargetEntity.transform.localScale = new Vector3(1f, 1f, 1f);
			//名字的获取,以物品ID这样的命名格式
			nTargetEntity.name = nBackArray[i].id.ToString();
			int nBackType = nBackArray[i].type;
			Debug.LogError("nBackArray [i].id = " + nTargetEntity.name);
			nTargetEntity.GetComponent<Button>().onClick.AddListener(delegate () {
				OnPropertyClick(nTargetEntity, nBackType);

			});

			if (nLastSelectUnitId == -1 || nLastSelectUnitId == nBackArray[i].id)
			{
				OnPropertyClick(nTargetEntity, nBackType);
			}
		}
	}

}

