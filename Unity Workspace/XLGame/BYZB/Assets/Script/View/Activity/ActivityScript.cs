using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using LitJson;
using UnityEngine.UI;
using AssemblyCSharp;


public class ActivityScript : MonoBehaviour
{
	//json解析类
	ActiveItemClass _activeItem;
	NoticeItemClass _noticeItem;
	/// <summary>
	/// 存储活动内容
	/// </summary>
	public static List<ActiveItemClass> _activeList = new List<ActiveItemClass> ();
	/// <summary>
	/// 存储公告内容
	/// </summary>
	public static List<NoticeItemClass> _noticeList = new List<NoticeItemClass> ();
	/// <summary>
	/// 公告脚本类
	/// </summary>
	public ActivityGridUI noticeGridUI;
	/// <summary>
	/// 活动脚本类
	/// </summary>
	public ActivityGridUI activityGridUI;
	Toggle noticeToggle;
	Toggle activeToggle;
	public static ActivityScript Instance;
	/// <summary>
	/// 关闭按钮
	/// </summary>
	Button closeBtn;
	/// <summary>
	/// json数据
	/// </summary>
	JsonData _itemData;
	/// <summary>
	/// 公告总红点
	/// </summary>
	public GameObject noticeRedImg;
	/// <summary>
	/// 活动总红点
	/// </summary>
	public GameObject activityRedImg;
	public static  bool isShowAct = true;
	public static  bool isShowNot = true;

	void Awake ()
	{
		Instance = this;
		_itemData = ActivityInApp.Instance.GetActivity ();
	}

	void Start ()
	{
		Init ();
		AnalysisJson ();
		UIColseManage.instance.ShowUI (this.gameObject);
	}

	/// <summary>
	/// 初始化,绑定toogle
	/// </summary>
	void Init ()
	{
		noticeToggle = transform.Find ("ActivityControl/TopControl/NoticeToggle").GetComponent <Toggle> ();
		activeToggle = transform.Find ("ActivityControl/TopControl/ActivityToggle").GetComponent <Toggle> ();
		closeBtn = transform.Find ("ActivityControl/CloseButton").GetComponent < Button> ();
		noticeToggle.onValueChanged.RemoveAllListeners ();
		activeToggle.onValueChanged.RemoveAllListeners ();
		closeBtn.onClick.RemoveAllListeners ();
		closeBtn.onClick.AddListener (delegate() {
			Destroy (gameObject);
			AppInfo.isFritsInGame = false;

			UIColseManage.instance.CloseUI();
			if (DataControl.GetInstance().GetMyInfo().isNewSevenHand){
				if (DataControl.GetInstance().GetMyInfo().isLevelupCanGet) {
					if (UIHallCore.isFristActivity && AppInfo.trenchNum > 51000000)
					{
						Debug.Log(" 新手升級任務 NewLevelupGrade ");
						//DataControl.GetInstance().PushSocketSnd(FiEventType.SEND_XL_LEVELUP_INFO_NEW_RESPOSE, null);
      //                  string path = "Window/NewLevelupGrade";
      //                  GameObject WindowClone = AppControl.OpenWindow(path);
      //                  WindowClone.SetActive(true);
                        UIHallCore.isFristActivity = false;
					}
				}
			}
			//else {
			//	string path = "MainHall/RankList/BigwinRankWindowCanvas";
			//	GameObject WindowClone = AppControl.OpenWindow(path);
			//	WindowClone.SetActive(true);
			//	UIHallCore.isFristActivity = false;
		 //   }

            //if (DataControl.GetInstance().GetMyInfo().teihui <= 1)
            //{
            //    // string path = "Window/Preferential/Preferential_Canvas12";
            //    string path = "MainHall/RankList/BigwinRankWindowCanvas";
            //    GameObject WindowClone = AppControl.OpenWindow(path);
            //    WindowClone.SetActive(true);
            //    UIFirstRecharge.SetState = DataControl.GetInstance().GetMyInfo().loginInfo.preferencePackBought;
            //}
            //else if (DataControl.GetInstance().GetMyInfo().teihui < 0 || DataControl.GetInstance().GetMyInfo().teihui >= 5)
            //{
            //    string path = "MainHall/RankList/BigwinRankWindowCanvas";
            //    GameObject WindowClone = AppControl.OpenWindow(path);
            //    WindowClone.SetActive(true);
            //}

            

		});
		noticeToggle.onValueChanged.AddListener (delegate {
			CheckNoticeToggle (noticeToggle);
		});
		activeToggle.onValueChanged.AddListener (delegate {
			CheckActivityToggle (activeToggle);
		});
	}

	/// <summary>
	/// 初始化解析
	/// </summary>
	void  AnalysisJson ()
	{
		InitActiveInfo (_itemData);
		activityGridUI.insNum = _itemData ["activity"].Count;
		noticeGridUI.insNum = _itemData ["notice"].Count;
		activityRedImg.SetActive (true);
		noticeRedImg.SetActive (true);
		if (!isShowAct) {
			activityRedImg.SetActive (false);
		}
		if (!isShowNot) {
			noticeRedImg.SetActive (false);
		}
	}

	/// <summary>
	/// 解析json
	/// </summary>
	/// <param name="itemData">Item data.</param>
	void InitActiveInfo (JsonData itemData)
	{
		//将数据添加进集合,
		string activity = "activity";
		for (int i = 0; i < itemData [activity].Count; i++) {
			_activeItem = new ActiveItemClass ();
			_activeItem.imageUrl = itemData [activity] [i] ["image_url"].ToString ();
			_activeItem.jumpUrl = itemData [activity] [i] ["jump_url"].ToString ();
			_activeItem.activityType = (int)itemData [activity] [i] ["activity_type"];
			_activeItem.activityLabel = (int)itemData [activity] [i] ["activity_lable"];
			_activeItem.titleName = itemData [activity] [i] ["title"].ToString ();
			_activeItem.linkBtnText = itemData [activity] [i] ["link_btn_text"].ToString ();
			_activeItem.redStatus = (int)itemData [activity] [i] ["red_status"];
			_activeList.Add (_activeItem);
		}
		string notice = "notice";
		for (int i = 0; i < itemData [notice].Count; i++) {
			_noticeItem = new NoticeItemClass ();
			_noticeItem.imageUrl = itemData [notice] [i] ["image_url"].ToString ();
			_noticeItem.titleName = itemData [notice] [i] ["title"].ToString ();
			_noticeItem.pictureFirst = (int)itemData [notice] [i] ["picture_first"];
			_noticeItem.content = itemData [notice] [i] ["content"].ToString ();
			_noticeItem.redStatus = (int)itemData [notice] [i] ["red_status"];
			_noticeList.Add (_noticeItem);
		}
		_activeItem.instantiateNum = itemData [activity].Count;
		_noticeItem.instantiateNum = itemData [notice].Count;
	}

	/// <summary>
	/// 点击活动按钮
	/// </summary>
	/// <param name="isCheck">Is check.</param>
	void CheckActivityToggle (Toggle isCheck)
	{
		int num = ActivityManager.Instance.laseSelectNum;
		noticeGridUI.gameObject.SetActive (!isCheck);
		activityGridUI.gameObject.SetActive (isCheck);
		//设置显示
		ActivityShowManager.Instance.SetTheActivityShowState (_activeList [num], _activeList [num].linkBtnText, _activeList [num].jumpUrl, _activeList [num].imageUrl);
		if (isCheck.isOn) {
			isCheck.interactable = false;
		} else {
			isCheck.interactable = true;
		}
	}

	/// <summary>
	/// 点击公告
	/// </summary>
	/// <param name="isCheck">Is check.</param>
	void CheckNoticeToggle (Toggle isCheck)
	{
		noticeGridUI.gameObject.SetActive (isCheck);
		activityGridUI.gameObject.SetActive (!isCheck);
		if (isCheck.isOn) {
			isCheck.interactable = false;
		} else {
			isCheck.interactable = true;
		}	
		int num = Act_NoticeManager.Instance.laseSelectNum;
		if (num == -1) {
			return;
		} else {
			ActivityShowManager.Instance.SetTheNoticeShowState (_noticeList [num], _noticeList [num].imageUrl, _noticeList [num].content);
		}
	}

	/// <summary>
	/// 刷新公告
	/// </summary>
	/// <param name="count">Count.</param>
	public void RefreshNoticeUI (int count)
	{
		noticeGridUI.insNum = count;
		noticeGridUI.Refresh ();
	}

	/// <summary>
	/// 刷新活动
	/// </summary>
	/// <param name="count">Count.</param>
	public void RefreshActivityUI (int count)
	{
		activityGridUI.insNum = count;
		activityGridUI.Refresh ();
	}
}
