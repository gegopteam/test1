using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using LitJson;

public class ActiveItemClass
{
	/// <summary>
	/// 生成数
	/// </summary>
	public int instantiateNum;
	/// <summary>
	/// 调转链接
	/// </summary>
	public string jumpUrl;
	/// <summary>
	/// 显示的image
	/// </summary>
	public string imageUrl;
	/// <summary>
	/// 标题名
	/// </summary>
	public string titleName;
	/// <summary>
	/// 活动类型   0 是图片的    1是跳转游戏内的   2 跳转游戏内嵌网页的
	/// </summary>
	public int activityType;
	/// <summary>
	/// 活动标签 0 常规 1 热门 2 推荐
	/// </summary>
	public int activityLabel;
	/// <summary>
	/// 调转按钮文本
	/// </summary>
	public string linkBtnText;
	/// <summary>
	/// 红点
	/// </summary>
	public int redStatus;
}

public class NoticeItemClass
{
	/// <summary>
	/// 生成数
	/// </summary>
	public int instantiateNum;
	/// 标题名
	/// </summary>
	public string titleName;
	/// <summary>
	/// 图片的网络地址 picture_first为1是读取
	/// </summary>
	public string imageUrl;
	/// <summary>
	/// 图片优先 1 表示显示图片 0 表示显示文字
	/// </summary>
	public int pictureFirst;
	/// <summary>
	/// 控制热门标签显示 
	/// </summary>
	public int activityLabel;
	/// <summary>
	/// 游戏公告内容 picture_first为0是读取
	/// </summary>
	public string content;
	/// <summary>
	/// 红点
	/// </summary>
	public int redStatus;
}

public class ActivityInApp : MonoBehaviour
{
	/// <summary>
	/// 测试地址
	/// </summary>
	//string _url = "http://183.131.69.227:8004/GameConfig/GetNoticeAndActivity";
	/// <summary>
	/// 正式地址
	/// </summary>
	//	string _url = "http://183.131.69.227:8003/GameConfig/GetNoticeAndActivity";
	string _url;

	string pkey = "2803852F21864BC6B0CDDA5E843349EE";
	JsonData _itemData = null;
	Dictionary<string,string> _formDic = new Dictionary<string, string> ();
	WWWForm _wForm;
	public static ActivityInApp Instance;

	void Awake ()
	{
		Instance = this;
	}

	void Start ()
	{
		StartCoroutine(delayStart());
		//if (UIUpdate.WebUrlDic.ContainsKey (WebUrlEnum.Activty)) {
		//	_url = UIUpdate.WebUrlDic [WebUrlEnum.Activty];
		//	Debug.LogError ("ActivityInApp");
		//	//Debug.LogError (_url);
		//}

		//获取Unix时间戳
		//_formDic.Add ("ts", Tool.GetUnixTimestamp ().ToString ());
		//_formDic.Add ("sign", Tool.GetMD532 (Tool.GetUnixTimestamp () + pkey).ToString ());
		//StartCoroutine (AnalysisJson (_url, _formDic));
	}

	IEnumerator delayStart() {
		yield return new WaitForSeconds(3.0f);

		if (UIUpdate.WebUrlDic.ContainsKey(WebUrlEnum.Activty))
		{
			_url = UIUpdate.WebUrlDic[WebUrlEnum.Activty];
			Debug.LogError("ActivityInApp");
			//Debug.LogError (_url);
			
			//获取Unix时间戳
			_formDic.Add("ts", Tool.GetUnixTimestamp().ToString());
			_formDic.Add("sign", Tool.GetMD532(Tool.GetUnixTimestamp() + pkey).ToString());
			StartCoroutine(AnalysisJson(_url, _formDic));
		}
	}

	/// <summary>
	/// 解析地址里的消息
	/// </summary>
	/// <returns>The json.</returns>
	/// <param name="url">URL.</param>
	/// <param name="formDic">Form dic.</param>
	IEnumerator AnalysisJson (string url, Dictionary<string,string> formDic)
	{
		_wForm = new WWWForm ();
		foreach (KeyValuePair<string,string> itemArg in formDic) {
			_wForm.AddField (itemArg.Key, itemArg.Value);
		}
		WWW _www = new WWW (url, _wForm);
		yield return _www;
		if (_www.isDone && _www.error == null) {
			string content = _www.text;
			_itemData = JsonMapper.ToObject (content);
			for (int i = 0; i < _itemData ["activity"].Count; i++) {
				ActivityDownLoad.Instance.SetAsyncImage (_itemData ["activity"] [i] ["image_url"].ToString ());
			}
			for (int i = 0; i < _itemData ["notice"].Count; i++) {
				ActivityDownLoad.Instance.SetAsyncImage (_itemData ["notice"] [i] ["image_url"].ToString ());
			}
		}
	}

	public JsonData GetActivity ()
	{
		return _itemData;
	}
}
