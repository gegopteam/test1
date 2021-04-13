using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ActivityGrid : ScrollableCell
{
	public Sprite[] titleImageGroup;
	Text labelText;
	Image titleImage;
	Image redImage;
	Toggle toggleGrid;
	public GameObject goButton;
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
	[HideInInspector]
	public ActiveItemClass _activityitem;
	public static ActivityGrid Instance;
	Image image;
	int id;

	void Awake ()
	{
		Instance = this;
		Init ();
	}

	void Start ()
	{
		InitColor ();
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
		//Debug.Log ("变颜色了");
		ActivityShowManager.Instance.SetTheActivityShowState (ActivityScript._activeList [0], ActivityScript._activeList [0].linkBtnText, ActivityScript._activeList [0].jumpUrl, ActivityScript._activeList [0].imageUrl);
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
				_activityitem = ActivityScript._activeList [(int)DataObject];
			}
			if (_activityitem != null && this.gameObject.activeInHierarchy) {
				
				switch (_activityitem.activityLabel) {
				case 0:
					titleImage.gameObject.SetActive (false);
					break;
				case 1:	
					titleImage.sprite = titleImageGroup [1];
					break;
				case 2:
					titleImage.sprite = titleImageGroup [0];
					break;
				default:
					break;
				}
				//第一个红点默认不显示
				if ((int)DataObject == 0) {
					_activityitem.redStatus = 0;
					redImage.gameObject.SetActive (false);
				}
				switch (_activityitem.redStatus) {
				case 0:
					redImage.gameObject.SetActive (false);
					break;
				case 1:
					redImage.gameObject.SetActive (true);
					if (redImage.gameObject.activeSelf) {
						//		Debug.LogError ("activeSelf idList.count = " + ActivityManager.actDataList.Count);
						if (!ActivityManager.actDataList.Contains ((int)DataObject)) {
							ActivityManager.actDataList.Add ((int)DataObject);
							//			Debug.LogError ("activeSelf idList.count222 = " + ActivityManager.actDataList.Count);
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
				toggleGrid.isOn = false;
				toggleGrid.onValueChanged.RemoveAllListeners ();
				toggleGrid.onValueChanged.AddListener (delegate {
					ToggleClick (toggleGrid);
				});

				if (!string.IsNullOrEmpty (_activityitem.titleName)) {
					labelText.text = _activityitem.titleName;
				}	
				//默认选中第一个
//				if (ActivityManager.Instance.selectToggle == null && (int)DataObject == 0)
//					ActivityManager.Instance.ActivityToggleClick (toggleGrid);
//				if (ActivityManager.Instance.selectToggle != null && (int)DataObject == ActivityManager.Instance.laseSelectNum) {
//					ActivityManager.Instance.ActivityToggleClick (toggleGrid);
//				}
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
			ActivityManager.Instance.laseSelectNum = (int)toggle.GetComponent<ActivityGrid> ().DataObject;		
			ActivityManager.Instance.ActivityToggleClick (toggle);
			if (redImage.gameObject.activeSelf) {
				redImage.gameObject.SetActive (false);
			}
//			Debug.LogError ("actDataList.count1 = " + ActivityManager.actDataList.Count); 
			if (ActivityManager.actDataList.Count <= 0) {
				ActivityScript.isShowAct = false;
				ActivityScript.Instance.activityRedImg.SetActive (false);
			}
			//		Debug.LogError ("actDataList.count2 = " + ActivityManager.actDataList.Count);

			_activityitem.redStatus = 0;
		}	
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
			ActivityShowManager.Instance.SetTheActivityShowState (_activityitem, _activityitem.linkBtnText, _activityitem.jumpUrl, _activityitem.imageUrl);
			//Debug.LogError ("i-------------------------------d------------  = " + id);
		} else {
			outLine.effectColor = outDefaColor;
			twoColor.colorTop = topColor;
			twoColor.colorBottom = botDefaColor;
		}
	}
}
