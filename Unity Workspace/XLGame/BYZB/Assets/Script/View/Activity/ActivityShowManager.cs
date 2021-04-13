using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ActivityShowManager : MonoBehaviour
{
	/// <summary>
	/// 显示的图片
	/// </summary>
	Image showImage;
	/// <summary>
	/// 点击的button
	/// </summary>
	Button goButton;
	/// <summary>
	/// 标题文本
	/// </summary>
	Text titleText;
	/// <summary>
	/// 公告文本
	/// </summary>
	Text noticeText;
	/// <summary>
	/// 按钮文本
	/// </summary>
	Text linkBtnText;
	public static ActivityShowManager Instance;
	/// <summary>
	/// 调转的预制体
	/// </summary>
	GameObject jumpObj;
	MyInfo myInfo;

	void Awake ()
	{
		Instance = this;
		myInfo = DataControl.GetInstance ().GetMyInfo ();
	}

	void Start ()
	{
		showImage = transform.Find ("ShowImage").GetComponent <Image> ();
		goButton = transform.Find ("GOButton").GetComponent <Button> ();
		linkBtnText = transform.Find ("GOButton/Text").GetComponent <Text> ();
		noticeText = transform.Find ("NoticeText").GetComponent <Text> ();
	}

	/// <summary>
	/// 设置活动显示
	/// </summary>
	/// <param name="activityitem">Activityitem.</param>
	/// <param name="_linkBtnText">Button的文字</param>
	/// <param name="_jumpUrl">调转链接</param>
	/// <param name="_imageUrl">图片链接</param>
	public void SetTheActivityShowState (ActiveItemClass activityitem, string _linkBtnText = "", string _jumpUrl = "", string _imageUrl = "")
	{
#if UNITY_EDITOR
		Debug.Log(" 活動頁面 activityitem：" + activityitem);
		Debug.Log(" 活動頁面 _linkBtnText：" + _linkBtnText);
		Debug.Log(" 活動頁面 _jumpUrl：" + _jumpUrl);
		Debug.Log(" 活動頁面 _imageUrl：" + _imageUrl);
#endif
		showImage.gameObject.SetActive (true);
		noticeText.gameObject.SetActive (false);
		linkBtnText.text = _linkBtnText;
		ActivityDownLoad.Instance.SetAsyncImage (_imageUrl, showImage);
		goButton.onClick.RemoveAllListeners ();
		if (activityitem.activityType == 2) {
			goButton.gameObject.SetActive (true); 
			goButton.onClick.AddListener (delegate() {
				if (myInfo.isGuestLogin) {
					string path = "Window/BindPhoneNumber";
					jumpObj = AppControl.OpenWindow (path);
					jumpObj.SetActive (true);
					DestroyImmediate (this.transform.parent.parent.gameObject);
				} else {
					string path = "Window/OpenUrlCanvas";
					jumpObj = AppControl.OpenWindow (path);
					jumpObj.SetActive (true);
					//先创建控件,在进行浏览
					if (OpenWebScript.Instance != null) {
						string urlTemp;

                        if (myInfo.platformType == 22 || myInfo.platformType == 24 || myInfo.isPhoneNumberLogin) {
                        	//这里是由于微信,qq没有密码,所以拿到服务器下发的token进行小游戏的登录
                            //urlTemp = _jumpUrl + "?UserID=" + myInfo.userID + "&pwd=" + myInfo.mLoginData.token + "&type=" + myInfo.GetTokenType () + "&uname=" + WWW.EscapeURL (myInfo.nickname);
							urlTemp = _jumpUrl + "?UserID=" + myInfo.userID + "&pwd=" + myInfo.acessToken + "&type=" + myInfo.GetTokenType() + "&uname=" + WWW.EscapeURL(myInfo.nickname);
						} else {
                        	//用 WWW.EscapeURL 是因为拼接的时候,如果昵称含有中文,这个时候会打不开web
                            urlTemp = _jumpUrl + "?UserID=" + myInfo.userID + "&pwd=" + myInfo.password + "&type=" + 0 + "&uname=" + WWW.EscapeURL (myInfo.nickname);
                        }

						//Debug.Log(" 活動網址 = "+ urlTemp);

						//渠道宏定义 这里由于是渠道登录所以不能用 账号和微信QQ的pwd
                        #if UNITY_Huawei
                        urlTemp = _jumpUrl + "?UserID=" + myInfo.mLoginData.userid + "&pwd=" + myInfo.acessToken + "&type=" + myInfo.GetTokenType() + "&uname=" + WWW.EscapeURL(myInfo.nickname);
                        #elif UNITY_OPPO
                        urlTemp = _jumpUrl + "?UserID=" + myInfo.mLoginData.userid + "&pwd=" + myInfo.acessToken + "&type=" + myInfo.GetTokenType() + "&uname=" + WWW.EscapeURL(myInfo.nickname);
                        #elif UNITY_VIVO
                        urlTemp = _jumpUrl + "?UserID=" + myInfo.mLoginData.userid + "&pwd=" + myInfo.acessToken + "&type=" + myInfo.GetTokenType() + "&uname=" + WWW.EscapeURL(myInfo.nickname);
                        #endif
						//打开webview控件
						//Debug.Log(" ActivityShowManager urlTemp = "+ urlTemp);
                        OpenWebScript.Instance.SetActivityWebUrl (urlTemp);

						if (UIHallCore.Instance != null)
						{
							UIHallCore.Instance.LogOff();//因渠道不能使用第三方SDK
#if UNITY_Huawei
                            UIHallCore.Instance.LogOffTow();
#elif UNITY_OPPO
                            UIHallCore.Instance.LogOffTow();
#elif UNITY_VIVO
                            UIHallCore.Instance.LogOffTow();
#endif
						}
					}
                }
			});
		} else if (activityitem.activityType == 1) {
			//调转游戏内部
			goButton.gameObject.SetActive (true); 
			goButton.onClick.RemoveAllListeners ();
			switch (activityitem.jumpUrl) {
			//跳转龙卡
			case "DRAGONCARD":
				goButton.onClick.AddListener (delegate() {
					UIHallCore.Instance.MouthCard ();
					DestroyImmediate (this.transform.parent.parent.gameObject);
				});
				break;
			//调转游戏
			case "QUICKSTART":
				goButton.onClick.AddListener (delegate() {
					UIHallCore.Instance.StartGame ();
					DestroyImmediate (this.transform.parent.parent.gameObject);
				});
				break;
			case "3":
				break;
			default:
				break;
			} 

		} else {
			goButton.gameObject.SetActive (false);
			showImage.gameObject.SetActive (true);
		}
	}

	/// <summary>
	/// 设置公告的显示
	/// </summary>
	/// <param name="noticeitem">Noticeitem.</param>
	/// <param name="_imageUrl">图片链接</param>
	/// <param name="_noticeText">公告文本</param>
	public void SetTheNoticeShowState (NoticeItemClass noticeitem, string _imageUrl = "", string _noticeText = "")
	{
		goButton.gameObject.SetActive (false);
		showImage.gameObject.SetActive (false);
		noticeText.gameObject.SetActive (false);
		//图片优先 1 表示显示图片 0 表示显示文字
		if (noticeitem.pictureFirst == 1) {
			showImage.gameObject.SetActive (true);
			ActivityDownLoad.Instance.SetAsyncImage (_imageUrl, showImage);
		} else if (noticeitem.pictureFirst == 0) {
			noticeText.gameObject.SetActive (true);
			noticeText.text = _noticeText;
		}
	}
}
