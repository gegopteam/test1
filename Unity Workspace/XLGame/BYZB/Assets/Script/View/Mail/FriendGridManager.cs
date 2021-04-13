//using UnityEngine;
//using System.Collections;
//using AssemblyCSharp;
//using UnityEngine.UI;
///// <summary>
///// Friend grid manager.邮件系统邮件的初始化
///// </summary>
//
//public class FriendGridManager : MonoBehaviour {
//	private int childCount;
//	private GiveGridInfo [] gridInfo;
//	private GameObject currentMail;
//
//	//public delegate void SendGiveMailDelegate(GameObject go);
//	//public static event SendGiveMailDelegate SendGiveEvent;
//
//	// Use this for initialization
//	void Awake () {
//		
//		//初始化显示系统邮件的点击按钮
//		//FriendGiveManager.GetGridEvent += GetCount;
//	}
//
//	void FristInfo()
//	{
//		//FriendInfo nFriInfo = (FriendInfo)Facade.GetFacade ().data.Get ( FacadeConfig.FRIEND_MODULE_ID );
//		//List<FiFriendInfo> nInfoList = nFriInfo.GetFriendList();
//		if (Goodlist.GetInstance ().SystemMailList.Count != 0) {
//
//			GameObject frist = transform.GetChild (0).gameObject;
//			Image image = frist.GetComponent<Image> ();
//			image.sprite = Resources.Load ("Mail/已读邮件框",typeof(Sprite))as Sprite;
//			for (int i = 1; i < transform.GetChildCount (); i++) {
//				image.sprite = Resources.Load ("Mail/未读邮件框",typeof(Sprite))as Sprite;
//			}
//		}
//	}
//
//	void GetCount()
//	{
//		childCount = transform.GetChildCount ();
//		gridInfo = GetComponentsInChildren<GiveGridInfo> ();
//		Debug.Log ("grid的长度"+gridInfo.Length);
//		//给所有的子对象添加点击事件
//		for (int i = 0; i < gridInfo.Length; i++) {
//
//			EventTriggerListener.Get (gridInfo [i].gameObject).onClick = ClickCallBack;
//		}
//		//获取所有的子游戏对象并对其对象赋值
//		InitGridInfo();
//	}
//
//	void InitGridInfo()
//	{
//		for (int i = 0; i < gridInfo.Length; i++) {
//			//日期的问题,头像图片的替换
//			gridInfo[i].nickName.text = Goodlist.GetInstance().GiveMailList[i].nickname;
//			gridInfo [i].date.text = Goodlist.GetInstance ().GiveMailList [i].timestamp.ToString ();
//			gridInfo [i].time.text = Goodlist.GetInstance ().GiveMailList [i].timestamp.ToString ();
//			gridInfo [i].id = Goodlist.GetInstance ().GiveMailList [i].id;
//		}
//	}
//
//	void ClickCallBack(GameObject go)
//	{
//		currentMail = go;
//		Image image;
//		for (int i = 0; i < gridInfo.Length; i++) {
//			if (go == gridInfo [i].gameObject) {
//				image = go.GetComponent<Image> ();
//				image.sprite = Resources.Load ("Mail/已读邮件框", typeof(Sprite))as Sprite;
//			} else {
//				image = gridInfo [i].gameObject.GetComponent<Image> ();
//				image.sprite = Resources.Load ("Mail/未读邮件框", typeof(Sprite))as Sprite;
//			}
//		}
//		//if (SendGiveEvent != null) {
//		//	SendGiveEvent (go);
//		//}
//	}
//
//	// Update is called once per frame
//	void Update () {
//
//	}
//
//	void OnDestroy()
//	{
//		
//	}
//}
