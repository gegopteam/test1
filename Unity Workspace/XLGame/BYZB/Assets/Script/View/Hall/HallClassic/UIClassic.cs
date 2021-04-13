using UnityEngine;
using System.Collections;

using UnityEngine.UI;
using AssemblyCSharp;

public class UIClassic : MonoBehaviour
{

	public Button[] fieldGrade;
	public Button[] FieldGrade_;

	private DataControl dataControl = null;
	private MyInfo myInfo = null;
	private static UIClassic instance;


	public  GameObject NewLock;
	public  GameObject DeepLock;
	public  GameObject GodLock;
	public  GameObject Goldmedal;
	public Text nickName;

	public GameObject TestUrlObj;

	//判断是否是boss场界面
	public bool IsNewPlay = false;

	public static UIClassic Instance {
		get {
			return instance;
		}
	}

	void Awake ()
	{
		//Debug.Log("--------------");
		//Debug.Log(Screen.width);
		//Debug.Log(Screen.height);
		//iPhone X屏幕适配
		if (Screen.width == 2436 && Screen.height == 1125) {
			Debug.Log ("111111");
			var canvesSancel = GetComponent<CanvasScaler> ();
			canvesSancel.referenceResolution = new Vector3 (Screen.width, Screen.height, 0);
		}
		instance = this;
		dataControl = DataControl.GetInstance ();
		myInfo = dataControl.GetMyInfo ();
		InitFieldGrade ();
		foreach (var t in GetComponentsInChildren<Transform>()) {
			if (t.name == "RoomScroll") {
				IsNewPlay = true;
			}
		}
//		Debug.LogError ("_________________________+" + IsNewPlay);
	}

	// Use this for initialization
	void Start ()
	{
		UIStore.HideEvent += Hide;
		UIVIP.SeeEvent += See;

		if (nickName != null)
			nickName.text = UIHall.nickNamestr;


		//if (myInfo.cannonMultipleNow > 50 && AppInfo.trenchNum > 51000000) {
		//	FieldGrade_[0].gameObject.SetActive(false);
		//	FieldGrade_[4].gameObject.SetActive(true);
		//}

		//测试代码
//		SetTestObj ();
	}
	//测试使用的返回大厅的按钮
	public void RetBtnHall ()
	{
		AppControl.ToView (AppView.HALL);
	}

	// Update is called once per frame
	void Update ()
	{

	}

	void Hide ()
	{
		transform.gameObject.SetActive (false);
	}

	void See ()
	{
		transform.gameObject.SetActive (true);
	}

	public void ExitButton ()
	{
		AppControl.ToView (AppView.HALL);
	}

	public void StartGame ()
	{
		UIHallObjects.GetInstance ().StartGame ();
	}

	public  void InitFieldGrade ()
	{
		if (null != fieldGrade [0]) {
			fieldGrade [0].interactable = false;
		}
		if (null != fieldGrade [1]) {
			fieldGrade [1].interactable = false;
		}
		if (null != fieldGrade [2]) {
			fieldGrade [2].interactable = false;
		}
		if (null != myInfo) {
			int multiple = myInfo.cannonMultipleMax;
//			Debug.LogError ("myInfo.cannonMultipleMax" + myInfo.cannonMultipleMax);
			//炮倍数大于0，新手场就解锁了，大于100，无法进入，点击后弹框提示
			if (multiple > 0) {
				if (null != fieldGrade [0]) {
//					Debug.Log ("fieldGrade [0].interactable = true;");
					fieldGrade [0].interactable = true;
					NewLock.SetActive (false);
				}
			}

			if (multiple >= CannonMultiple.DEEPSEA) {//深海遗址 || myInfo.levelVip >= 2
				if (null != fieldGrade [1]) {
					fieldGrade [1].interactable = true;
					DeepLock.SetActive (false);
//					Debug.Log ("vip等级" + myInfo.levelVip);
					//fieldGrade [1].transform.Find ("Mask").gameObject.SetActive( false );
				}
			}

			if (multiple >= CannonMultiple.POSEIDON) {//海神宝藏  去掉了vip的进场|| myInfo.levelVip >= 2
				if (null != fieldGrade [2]) {
					fieldGrade [2].interactable = true;
					GodLock.SetActive (false);

					//fieldGrade [2].transform.Find ("Mask").gameObject.SetActive( false );
				}
			}

			if (multiple >= CannonMultiple.GOLDMEDAL) {//夺金岛// || myInfo.levelVip >= 2
				if (null != fieldGrade [3]) {
					fieldGrade [3].interactable = true;
					Goldmedal.SetActive (false);
					//fieldGrade [2].transform.Find ("Mask").gameObject.SetActive( false );
				}
			}

		}
	}
	/*
	public void PlayFieldGrade_1()
	{//新手海湾
		Tool.LogError ("新手海湾");
		UIHallObjects.GetInstance ().PlayFieldGrade_1 ();
	}

	public void PlayFieldGrade_2()
	{//深海遗址
		Tool.LogError ("深海遗址");
		UIHallObjects.GetInstance ().PlayFieldGrade_2 ();
	}

	public void PlayFieldGrade_3()
	{//海神宝藏
		Tool.LogError ("海神宝藏");
		UIHallObjects.GetInstance ().PlayFieldGrade_3 ();
	}
    */
	void OnDestroy ()
	{
		UIStore.HideEvent -= Hide;
		UIVIP.SeeEvent -= See;
	}

	////解锁炮
	//void OnUnlockGun()
	//{
	//    int multiple = myInfo.cannonMultipleMax;
	//    if (multiple >= CannonMultiple.POSEIDON)
	//    {
	//        NewLock.SetActive(false);
	//        DeepLock.SetActive(false);
	//        GodLock.SetActive(false);
	//    }
	//}

	public void opentestobj ()
	{
		TestUrlObj.SetActive (true);
	}

	public void CloseTestObj ()
	{
		Debug.Log ("11111111");
		TestUrlObj.SetActive (false);
	}

	void SetTestObj ()
	{
		Text temptext = TestUrlObj.transform.Find ("Text").GetComponent<Text> ();
		foreach (var item in UIUpdate.WebUrlDic) {
			temptext.text += string.Format ("{0}的Url:{1}\n\n", item.Key, item.Value);
		}
	}
}
