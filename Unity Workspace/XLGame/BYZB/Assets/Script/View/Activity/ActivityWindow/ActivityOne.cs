using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using LitJson;
using System.Collections.Generic;

public class ActivityOne : MonoBehaviour
{
	[SerializeField]
	private GameObject mouthcard;
	[SerializeField]
	private Camera mouthcardCamera;
	[SerializeField]
	private Canvas mainCanvas;
	private WebViewObject webViewObject;
	private static bool isActivity = true;
	private MyInfo userInfo;
	/// 正式地址
	//    private string _url = "http://183.131.69.227:8003/GameConfig/GetNoticeAndActivity";
	private string _url;

	private string pkey = "2803852F21864BC6B0CDDA5E843349EE";
	private JsonData _itemData = null;
	private Dictionary<string, string> _formDic = new Dictionary<string, string> ();
	//存储活动图片
	private List<string> data = new List<string> ();
	//存储活动标题
	private List<string> title = new List<string> ();
	//存储活动跳转网页图片
	private List<string> jump_url = new List<string> ();
	//存储活动btn文字
	private List<string> link_btn_text = new List<string> ();
	public Image activityImage;
	public GameObject[] activityObj;
	private  WWWForm _wForm;
	public Text btnText;
	//初始化设置Canvas为摄像机模式
	private void Awake ()
	{
		userInfo = DataControl.GetInstance ().GetMyInfo ();
		if (GameController._instance == null) {
			mouthcard = GameObject.FindGameObjectWithTag ("MainCamera");
		} else {
			mouthcard = GameObject.FindGameObjectWithTag (TagManager.uiCamera);
		}
		Debug.Log (mouthcard.name);
		mouthcardCamera = mouthcard.GetComponent<Camera> ();
		mainCanvas = transform.GetComponentInChildren<Canvas> ();
		mainCanvas.worldCamera = mouthcardCamera;
		gameObject.AddComponent<OpenWebScript> ();
	}
	//初始化
	void Start ()
	{
		if (UIUpdate.WebUrlDic.ContainsKey (WebUrlEnum.Activty)) {
			_url = UIUpdate.WebUrlDic [WebUrlEnum.Activty];
		}
		
		activityImage.gameObject.SetActive (false);
		activityObj [0].gameObject.SetActive (false);
		activityObj [1].gameObject.SetActive (false);
		activityObj [2].gameObject.SetActive (false);
		activityObj [3].gameObject.SetActive (false);
		//获取Unix时间戳
		_formDic.Add ("ts", Tool.GetUnixTimestamp ().ToString ());
		_formDic.Add ("sign", Tool.GetMD532 (Tool.GetUnixTimestamp () + pkey).ToString ());
		StartCoroutine (AnalysisJson (_url, _formDic));
		BindWindowCtrl.isStartActivity = false;
	}

	/// 解析地址里的消息
	IEnumerator AnalysisJson (string url, Dictionary<string, string> formDic)
	{
		Debug.Log ("12334556");
		_wForm = new WWWForm ();
		foreach (KeyValuePair<string, string> itemArg in formDic) {
			_wForm.AddField (itemArg.Key, itemArg.Value);
		}
		WWW _www = new WWW (url, _wForm);
		yield return _www;
		if (_www.isDone && _www.error == null) {
			string content = _www.text;
			Debug.Log ("content==" + content);
			_itemData = JsonMapper.ToObject (content);
			for (int i = 0; i < _itemData ["activity"].Count; i++) {
				//ActivityDownLoad.Instance.SetAsyncImage(_itemData["activity"][i]["image_url"].ToString());
				data.Add (_itemData ["activity"] [i] ["image_url"].ToString ());
				title.Add (_itemData ["activity"] [i] ["title"].ToString ());
				jump_url.Add (_itemData ["activity"] [i] ["jump_url"].ToString ());
				link_btn_text.Add (_itemData ["activity"] [i] ["link_btn_text"].ToString ());
			}
			btnText.text = link_btn_text [0];
			StartCoroutine (GetImageOne (data [0], activityImage.sprite));
			StartCoroutine (SetTimeActivity ());
		}
	}
	//设置weburl
	public void SetWenUrlView (string url)
	{
		webViewObject.Init ();
		webViewObject.LoadURL (url);
		webViewObject.SetVisibility (true);
		//左 上 右 下
		webViewObject.SetMargins (0, 0, 0, 0);
	}
	//点击跳转网页
	public void OnActivityWindowClick ()
	{
		if (title [0] == "龍卡八折售") {
			UIHallCore.Instance.MouthCard ();
			return;
		}
		GameObject activity = Resources.Load<GameObject> ("Window/Activity_1");
		GameObject go_1 = Instantiate (activity);
		webViewObject = go_1.AddComponent<WebViewObject> ();
		string tempStr;
		if (userInfo.platformType == 22 || userInfo.platformType == 24) {
			tempStr = jump_url [0] + "?UserID=" + userInfo.userID + "&pwd=" + userInfo.mLoginData.token + "&type=" + userInfo.GetTokenType () + "&uname=" + WWW.EscapeURL (userInfo.nickname);
		} else {
			tempStr = jump_url [0] + "?UserID=" + userInfo.userID + "&pwd=" + userInfo.password + "&type=" + 0 + "&uname=" + WWW.EscapeURL (userInfo.nickname);
		}
		OpenWebScript.Instance.SetActivityWebUrl (tempStr);

		if (UIHallCore.Instance != null) {
			UIHallCore.Instance.LogOff ();
		}
	}
	//加载图片
	public IEnumerator GetImageOne (string path, Sprite temp)
	{
		WWW www = new WWW (path);
		yield return www;
		if (string.IsNullOrEmpty (www.error)) {
			Texture2D tex = www.texture;
			temp = Sprite.Create (tex, new Rect (0, 0, tex.width, tex.height), new Vector2 (0, 0));
			this.activityImage.sprite = temp; //设置的图片，显示从URL图片
		}
	}
	//打开第二个活动页面
	public void OnActivityWindowClick_1 ()
	{
		GameObject activity = Resources.Load<GameObject> ("Window/Canvas_2");
		GameObject go_1 = Instantiate (activity);
		go_1.gameObject.SetActive (true);
		Destroy (gameObject);
	}
	//设置几秒钟显示UI
	private IEnumerator SetTimeActivity ()
	{
		yield return new WaitForSeconds (0.6f);
		activityImage.gameObject.SetActive (true);
		activityObj [0].gameObject.SetActive (true);
		activityObj [1].gameObject.SetActive (true);
		activityObj [2].gameObject.SetActive (true);
		activityObj [3].gameObject.SetActive (true);
	}

}
