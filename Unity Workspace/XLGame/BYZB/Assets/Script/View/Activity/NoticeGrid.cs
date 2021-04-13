using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class NoticeGrid : ScrollableCell
{
	public Sprite[] titleImageGroup;
	Text labelText;
	Image titleImage;
	Image redImage;
	Toggle toggleGrid;
	//public GameObject goButton;
	public Sprite[] test;
	/// <summary>
	/// 默认的top颜色 要改变的top颜色
	/// </summary>
	Color32 topColor = new Color32 (255, 255, 255, 255);
	/// <summary>
	/// 默认的bottom颜色
	/// </summary>
	Color32 botDefaColor = new Color32 (182, 205, 252, 255);
	/// <summary>
	/// 要改变的bottom颜色
	/// </summary>
	Color32 botChanColor = new Color32 (255, 227, 166, 255);
	/// <summary>
	/// 描边默认
	/// </summary>
	Color32 outDefaColor = new Color32 (38, 40, 192, 255);
	/// <summary>
	/// 描边改变
	/// </summary>
	Color32 outChanColor = new Color32 (199, 63, 10, 255);
	/// <summary>
	/// 组件
	/// </summary>
	Outline outLine;
	/// <summary>
	/// 字体颜色
	/// </summary>
	TextVerticalGradientTwoColor twoColor;
	NoticeItemClass _noticeitem;
	public static NoticeGrid Instance;
	int id;

	void Awake ()
	{
		Instance = this;
		Init ();
	}

	void Start ()
	{
		InitColor ();
		//goButton.GetComponent <Image> ().sprite = test [0];
		id = (int)DataObject;
		if (id == 0) {
			redImage.gameObject.SetActive (false);
		}
	}

	void Init ()
	{
		labelText = transform.Find ("Label").GetComponent <Text> ();
		redImage = transform.Find ("RedImage").GetComponent <Image> ();
		titleImage = transform.Find ("TitleImage").GetComponent <Image> ();
		toggleGrid = transform.GetComponent <Toggle> ();
		twoColor =	labelText.GetComponent <TextVerticalGradientTwoColor> ();
		outLine = labelText.GetComponent <Outline> ();
	}

	void InitColor ()
	{
		SetToggleState (toggleGrid);
		ActivityShowManager.Instance.SetTheNoticeShowState (ActivityScript._noticeList [Act_NoticeManager.Instance.laseSelectNum], ActivityScript._noticeList [Act_NoticeManager.Instance.laseSelectNum].imageUrl, ActivityScript._noticeList [Act_NoticeManager.Instance.laseSelectNum].content);
	}

	/// <summary>
	/// 设置Grid状态
	/// </summary>
	public void SetGridState (int titleState, int redState)
	{
		//demo设置
		if (redState == 1) {
			redImage.gameObject.SetActive (false);
		}
		if (titleState == 0) {
			titleImage.gameObject.SetActive (false);
		} else {
			titleImage.sprite = titleImageGroup [titleState];
		}
	}

	public override void ConfigureCellData ()
	{
		try {
			base.ConfigureCellData ();
			if (DataObject != null) {
				_noticeitem = ActivityScript._noticeList [(int)DataObject];
			}
			if (_noticeitem != null && this.gameObject.activeInHierarchy) {
				//					switch (_activityitem.titleStatus) {
				//					case 0:
				//						Debug.Log ("titleImage.sprite  = " + titleImage.sprite);
				//						titleImage.sprite = titleImageGroup [0];
				//						break;
				//					case 1:	
				//						titleImage.sprite = titleImageGroup [1];
				//						break;
				//					case 2:
				//						titleImage.gameObject.SetActive (false);
				//						break;
				//					default:
				//						break;
				//					}
//				switch (_activityitem.titleStatus) {
//				case 0:
//					Debug.Log ("titleImage.sprite  = " + titleImage.sprite);
//					titleImage.sprite = titleImageGroup [0];
//					break;
//				case 1:	
//					titleImage.sprite = titleImageGroup [1];
//					break;
//				case 2:
//					titleImage.gameObject.SetActive (false);
//					break;
//				default:
//					break;
//				}
				//第一个红点默认不显示
				if ((int)DataObject == 0) {
					_noticeitem.redStatus = 0;
					redImage.gameObject.SetActive (false);
				}
				switch (_noticeitem.redStatus) {
				case 0:
					redImage.gameObject.SetActive (false);
					break;
				case 1:
					redImage.gameObject.SetActive (true);
					if (redImage.gameObject.activeSelf) {
						if (!Act_NoticeManager.notDataList.Contains ((int)DataObject)) {
							Act_NoticeManager.notDataList.Add ((int)DataObject);
						}
					}
					break;
				default:
					break;
				}
				//					if (_activityitem.titleStatus == 1) {
				//						titleImage.sprite = titleImageGroup [1];
				//					} else if (_activityitem.titleStatus == 0) {
				//						titleImage.sprite = titleImageGroup [0];
				//					} else {
				//						titleImage.gameObject.SetActive (false);
				//					}

				if (!string.IsNullOrEmpty (_noticeitem.titleName)) {
					labelText.text = _noticeitem.titleName;
				}
				toggleGrid.isOn = false;
				toggleGrid.onValueChanged.RemoveAllListeners ();
				toggleGrid.onValueChanged.AddListener (delegate {
					ToggleClick (toggleGrid);
				});
			}
		} catch (System.Exception ex) {
			Debug.LogError (ex);
		}
	}

	/// <summary>
	/// 点击toggle
	/// </summary>
	/// <param name="toggle">Toggle.</param>
	void ToggleClick (Toggle toggle)
	{
		SetToggleState (toggle);
		if (toggle.isOn) {
			Act_NoticeManager.Instance.laseSelectNum = (int)toggle.GetComponent<NoticeGrid> ().DataObject;
			Act_NoticeManager.Instance.NoticeToggleClick (toggle);
			if (redImage.gameObject.activeSelf) {
				redImage.gameObject.SetActive (false);
			}
			if (Act_NoticeManager.notDataList.Count <= 0) {
				ActivityScript.isShowNot = false;
				ActivityScript.Instance.noticeRedImg.SetActive (false);
			}
		}
		_noticeitem.redStatus = 0;
	}

	/// <summary>
	/// toggle被选中了,那么描边,字体颜色会改变
	/// </summary>
	void SetToggleState (Toggle toggle)
	{
		if (toggle.isOn) {
			outLine.effectColor = outChanColor;
			twoColor.colorTop = topColor;
			twoColor.colorBottom = botChanColor;
			//设置显示
			ActivityShowManager.Instance.SetTheNoticeShowState (_noticeitem, _noticeitem.imageUrl, _noticeitem.content);
		} else {
			outLine.effectColor = outDefaColor;
			twoColor.colorTop = topColor;
			twoColor.colorBottom = botDefaColor;
		}
	}
}