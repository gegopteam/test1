//using UnityEngine;
//using System.Collections;
//using UnityEngine.UI;
//
///// <summary>
///// System grid manager.系统邮件的管理
///// </summary>
//
//public class SystemGridManager : MonoBehaviour {
//	
//	private SystemGridInfo [] gridInfo;
//	private GameObject currentMail;
//
//	void FristInfo()
//	{
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
//		//childCount = transform.GetChildCount ();
//		gridInfo = GetComponentsInChildren<SystemGridInfo> ();
//		Debug.Log ("grid的长度"+gridInfo.Length);
//		//给所有的子对象添加点击事件
//		for (int i = 0; i < gridInfo.Length; i++) {
//		
//			EventTriggerListener.Get (gridInfo [i].gameObject).onClick = ClickCallBack;
//		}
//	    //获取所有的子游戏对象并对其对象赋值
//		InitGridInfo();
//	}
//
//	void InitGridInfo()
//	{
//		for (int i = 0; i < gridInfo.Length; i++) {
//			//日期的问题
//			gridInfo [i].date.text = Goodlist.GetInstance ().SystemMailList [i].sendTime.ToString();
//			gridInfo [i].time.text = Goodlist.GetInstance ().SystemMailList [i].sendTime.ToString();
//			gridInfo [i].id = Goodlist.GetInstance ().SystemMailList [i].mailId;
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
//	}
//		
//
//	void OnDestroy()
//	{
//		
//	}
//}
