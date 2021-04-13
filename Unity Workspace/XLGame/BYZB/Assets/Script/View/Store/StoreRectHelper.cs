using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System.Collections.Generic;
using System;
using AssemblyCSharp;

public class StoreRectHelper : MonoBehaviour, IBeginDragHandler, IEndDragHandler
{
	public static int currentIndex;
	public float smooting = 5;
	//滑动速度
	public List<GameObject> listItem;
	//scrollview item
	public int pageCount = 3;
	//每页显示的项目

	public  ScrollRect srect;
	public Image tital;
	public Image pointInfo;
	public Text VIPLevel;
	float pageIndex;
	//总页数
	bool isDrag = false;
	//是否拖拽结束
	List<float> listPageValue = new List<float> { 0 };
	//总页数索引比列 0-1
	float targetPos = 0;
	//滑动的目标位置
	float nowindex = 0;
	//当前位置索引

	Vector3 notReadyScale = new Vector3 (0.8f, 0.8f, 1);
	Color dark;
	Color lights;
	Image[] VIPImage;
	Text[] VIPInfo;
	Transform Cannon;
	List<GameObject> lightList;
	List<Image> imageList;
	List<MeshRenderer> meshList;
	List<GameObject> lockList;
	List <GameObject> effectList;
	int viplevel;

	void Awake ()
	{
		lightList = new List<GameObject> ();
		imageList = new List<Image> ();
		meshList = new List<MeshRenderer> ();
		lockList = new List<GameObject> ();
		effectList = new List<GameObject> ();
		//standard = Shader.Find ("Standard");
		lights = new Color (0.718f, 0.718f, 0.718f);
		dark = new Color (0.18f, 0.18f, 0.18f);
		//if(light==null)
		//light = listItem [0].transform.FindChild ("a").GetComponent<MeshRenderer> ().material.shader;
		ListPageValueInit ();
		InitalInfo ();
		for (int i = 0; i < listItem.Count; i++) {
			lightList.Add (listItem [i].transform.Find ("Light").gameObject);
			imageList.Add (listItem [i].transform.Find ("image").GetComponent<Image> ());
			meshList.Add (listItem [i].transform.Find ("a").GetComponent<MeshRenderer> ());
			lockList.Add (listItem [i].transform.Find ("Lock").gameObject);
			//effectList.Add (listItem [ i].transform.Find ("_Effect").gameObject);
		}
		MyInfo nInfo = (MyInfo)Facade.GetFacade ().data.Get (FacadeConfig.USERINFO_MODULE_ID);
		viplevel = nInfo.levelVip;
		//if (viplevel != 0) {
		//	nowindex = viplevel - 1;
		//	targetPos = listPageValue [Convert.ToInt32 (nowindex)];
		//}
		ChooseGun ();
	}

	void RefreshLock ()
	{
		for (int i = 0; i < viplevel; i++) {
			lockList [i].SetActive (false);
		}
	
	}

	void Start ()
	{
		RefreshLock ();

	}


	//每页比例
	void ListPageValueInit ()
	{//9/1=9-1=8
		pageIndex = (listItem.Count / pageCount) - 1;
		if (listItem != null && listItem.Count != 0) {
			for (float i = 1; i <= pageIndex; i++) {
				listPageValue.Add ((i / pageIndex));
			}
		}
	}

	void ChooseGun ()
	{
		for (int i = 0; i < listItem.Count; i++) {
			if (i == nowindex) {
				//listItem [Convert.ToInt32 (i)].transform.FindChild ("Light").gameObject.SetActive (true);
				lightList [i].SetActive (true);
				//effectList [i].SetActive (true);
				Cannon = meshList [i].transform;
				imageList [i].color = Color.white;
				//插值让炮台变大
				//修改模型shader,达到变亮效果
				//meshList[i].material.shader = light;
				meshList [i].material.SetColor ("_Color", lights);
				tital.sprite = UIHallTexturers.instans.Vip [i + 1];// Resources.Load <Sprite> (@"VIP/VIP" + (i + 1));
				VIPLevel.text = (i + 1).ToString ();
				GetTipsInfo (i + 1);

				Cannon.Rotate (Vector3.up);

			} else {
				//listItem [Convert.ToInt32 (i)].transform.FindChild ("Light").gameObject.SetActive (false);
				lightList [i].SetActive (false);
				//effectList [i].SetActive (false);
				Cannon = meshList [i].transform;
				imageList [i].color = Color.gray;
				Cannon.parent.localScale = notReadyScale;
				//修改模型shader,达到变暗效果
				meshList [i].material.SetColor ("_Color", dark);
				Cannon.localEulerAngles = 20 * Vector3.forward + 180 * Vector3.up;
			}
		}
	}


	void Update ()
	{
		currentIndex = Convert.ToInt32 (nowindex);
		if (!isDrag)
			srect.horizontalNormalizedPosition = Mathf.Lerp (srect.horizontalNormalizedPosition, targetPos, Time.deltaTime * smooting);
		Cannon = meshList [Convert.ToInt32 (nowindex)].transform;
		Cannon.parent.localScale = Vector3.Slerp (Cannon.parent.localScale, Vector3.one, Time.deltaTime * smooting);
		Cannon.Rotate (Vector3.up);
	}

	/// <summary>
	/// 拖动开始
	/// </summary>
	/// <param name="eventData"></param>
	public void OnBeginDrag (PointerEventData eventData)
	{
		print ("start");
		isDrag = true;
	}

	/// <summary>
	/// 拖拽结束
	/// </summary>
	/// <param name="eventData"></param>
	public void OnEndDrag (PointerEventData eventData)
	{
		isDrag = false;
		var tempPos = srect.horizontalNormalizedPosition; //获取拖动的值
		var index = 0;
		float offset = Mathf.Abs (listPageValue [index] - tempPos);    //拖动的绝对值
		for (int i = 1; i < listPageValue.Count; i++) {
			float temp = Mathf.Abs (tempPos - listPageValue [i]);
			if (temp < offset) {
				index = i;
				offset = temp;
			}
		}
		targetPos = listPageValue [index];
		nowindex = index;
		ChooseGun ();
	}

	public void BtnLeftGo ()
	{
		AudioManager._instance.PlayEffectClip (AudioManager.ui_click);
		nowindex = Mathf.Clamp (nowindex - 1, 0, pageIndex);
		targetPos = listPageValue [Convert.ToInt32 (nowindex)];
		ChooseGun ();
	}

	public void BtnRightGo ()
	{
		AudioManager._instance.PlayEffectClip (AudioManager.ui_click);
        nowindex = Mathf.Clamp (nowindex + 1, 0, pageIndex);
		targetPos = listPageValue [Convert.ToInt32 (nowindex)];
		ChooseGun ();

	}
    public void JumpCurrentGun(int index = 1)
    {
        AudioManager._instance.PlayEffectClip(AudioManager.ui_click);
        nowindex = Mathf.Clamp(nowindex + index, 0, pageIndex);
        targetPos = listPageValue[Convert.ToInt32(nowindex)];
        ChooseGun();

    }

	void InitalInfo ()
	{
		VIPImage = new Image[8];
		VIPInfo = new Text[8];
		for (int i = 0; i < VIPInfo.Length; i++) {
			VIPImage [i] = VipSliderContrl.CloneGameObject (pointInfo.gameObject).GetComponent<Image> ();
			VIPInfo [i] = VIPImage [i].GetComponentInChildren<Text> ();
			VIPImage [i].gameObject.SetActive (false);
		}
	}

	void GetTipsInfo (int level)
	{
		//VipSliderContrl.CloneGameObject(pointInfo)
		for (int i = 0; i < VIPImage.Length; i++) {
			VIPImage [i].gameObject.SetActive (false);
		}
		int num;
		string[] temp = GetVipInfo (level);
		num = temp.Length;
		Debug.LogError ("level" + level);
		Debug.LogError ("num" + num);
		for (int i = 0; i < num; i++) {
			VIPInfo [i].text = temp [i];
			VIPImage [i].gameObject.SetActive (true);
			VIPImage [i].rectTransform.sizeDelta = new Vector2 (42, 44);
		}
	}

	public static string[] GetVipInfo (int level)
	{
		string[] tmp;
		switch (level) {
		case 1:
			tmp = new string[4];
			tmp[0] = "VIP1每日禮包:";
			tmp[1] = "—冰凍,鎖定,狂暴道具各1個";
			tmp[2] = "解鎖贈送道具功能";
			// tmp [3] = "解鎖自動開砲功能";
			tmp[3] = "遊戲好友上限提升至30人";
			//tmp [4] = "每日魅力兌換的次數提升至20次";
			//tmp [5] = "測試測試測試測試測試測試測試";
			break;
		case 2:
			tmp = new string[6];
			tmp[0] = "VIP2每日禮包:";
			tmp[1] = "—冰凍,鎖定,狂暴道具各2個";
			tmp[2] = "解鎖分身功能";
			tmp[3] = "遊戲好友上限提升至40人";
			//tmp [3] = "每日魅力兌換的次數提升至30次";
			tmp[4] = "高級VIP享有全部低級特權";
			tmp[5] = "自動解鎖1～9900倍炮";
			break;
		case 3:
			tmp = new string[4];
			//tmp [0] = "解鎖喇叭喊話功能";
			tmp[0] = "VIP3每日禮包:";
			tmp[1] = "—冰凍,鎖定,狂暴道具各3個";
			tmp[2] = "遊戲好友上限提升至50人";
			//tmp [3] = "每日魅力兌換的次數提升至50次";
			tmp[3] = "高級VIP享有全部低級特權";
			break;
		case 4:
			tmp = new string[5];
			tmp[0] = "VIP4每日禮包:";
			tmp[1] = "—冰凍,鎖定,狂暴道具各4個";
			tmp[2] = "解鎖三重分身功能";
			tmp[3] = "遊戲好友上限提升至60人";
			//tmp [3] = "每日魅力兌換的次數提升至100次";
			tmp[4] = "高級VIP享有全部低級特權";
			break;
		case 5:
			tmp = new string[4];
			tmp[0] = "VIP5每日禮包:";
			tmp[1] = "—冰凍,鎖定,狂暴道具各5個";
			tmp[2] = "遊戲好友上限提升至70人";
			//tmp [2] = "每日魅力兌換的次數提升至150次";
			tmp[3] = "高級VIP享有全部低級特權";
			break;
		case 6:
			tmp = new string[7];
			tmp[0] = "噴射熾熱火焰";
			tmp[1] = "可攻擊直線上所有魚";
			tmp[2] = "龍卡用戶享受30天特權 VIP永久解鎖";
			tmp[3] = "VIP6每日禮包:";
			tmp[4] = "—冰凍,鎖定,狂暴道具各8個";
			tmp[5] = "遊戲好友上限提升至80人";
			//tmp [2] = "每次破產補助領取的金額增加至20000";
			tmp[6] = "高級VIP享有全部低級特權";
			break;
		case 7:
			tmp = new string[7];
			tmp[0] = "噴射閃電";
			tmp[1] = "可同時攻擊五條魚";
			tmp[2] = "龍卡用戶享受30天特權 VIP永久解鎖";
			tmp[3] = "VIP7每日禮包:";
			tmp[4] = "—冰凍,鎖定,狂暴道具各10個";
			tmp[5] = "遊戲好友上限提升至90人";
			tmp[6] = "高級VIP享有全部低級特權";
			break;
		case 8:
			tmp = new string[5];
			//tmp [0] = "金幣不足100萬時，每日首次登錄補足至100萬";
			tmp[0] = "提高擊殺概率";
			tmp[1] = "VIP8每日禮包:";
			tmp[2] = "—冰凍,鎖定,狂暴道具各12個";
			tmp[3] = "遊戲好友上限提升至100人";
			tmp[4] = "高級VIP享有全部低級特權";
			break;
		case 9:
			tmp = new string[5];
			//tmp [0] = "金幣不足200萬時，每日首次登錄補足至200萬";
			tmp[0] = "提高擊殺概率";
			tmp[1] = "VIP9每日禮包:";
			tmp[2] = "—冰凍,鎖定,狂暴道具各15個";
			tmp[3] = "遊戲好友上限提升至110人";
			tmp[4] = "高級VIP享有全部低級特權";
			break;
		default:
			tmp = new string[0];
			break;
		}
		return tmp;
	}
}
