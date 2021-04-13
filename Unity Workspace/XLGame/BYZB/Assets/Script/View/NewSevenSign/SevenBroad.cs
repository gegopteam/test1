using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using DG.Tweening;
using AssemblyCSharp;

public class SevenBroad : MonoBehaviour
{

	Vector3 originPos;
	public Text content;
	public bool isRoll = false;
	public static SevenBroad instans;
	BroadCastInfo nDataInfo;

	Tweener tween;
	Tweener tweenerInHall;


	void Awake ()
	{
		if (instans != null) {
			DestroyImmediate (instans.gameObject);
		}
		instans = this;
		content.transform.GetComponent<RectTransform> ().anchoredPosition = Vector3.right * 400f;
		nDataInfo = (BroadCastInfo)Facade.GetFacade ().data.Get (FacadeConfig.BROADCAST_MODULE_ID);
		tweenerInHall.Complete ();
		tweenerInHall = null;
	}

	void Start ()
	{
		

//		StartCoroutine (StartRollMsg ());
		RollComplete ();
	}
	//播放下一条消息
	public void RollMsg (string msg, int type = 0)
	{
		isRoll = true;
		content.text = msg;
	
		content.transform.GetComponent<RectTransform> ().anchoredPosition = Vector3.right * 41f;
		DoRollAni ();
	}

	//播放动画
	void DoRollAni ()
	{
		
		//tween = content.rectTransform.DOLocalMoveY (content.preferredHeight, 1);
		int time = 2;
		if (content.preferredWidth > 400) {
			time = (int)content.preferredWidth / 85;
		}


		if (tweenerInHall == null) {
			//Debug.LogError("调一次动画在大厅");
			tweenerInHall = content.rectTransform.DOLocalMoveX (-content.preferredWidth * 2f, time);
			tweenerInHall.SetEase (Ease.Linear);
			tweenerInHall.OnComplete (delegate () {
				//Invoke("RollComplete",4f);
				tweenerInHall = null;
				//Debug.LogError("动画结束了在大厅");
				RollComplete ();

			});  
		}

	}
	//播放完毕，判断是否存在下一条消息
	public void RollComplete ()
	{
		//broadCastObj.SetActive (false);
		isRoll = false;
		BroadCastUnit data = nDataInfo.GetSevenDayUserMessage ();
		Debug.LogError ("data:" + data.content);
		if (data != null) {
			RollMsg (data.content, data.type);
		} 
	}

	/*	IEnumerator StartRollMsg(){
		while (true) {
			FiScrollingNotice data = nDataInfo.GetRollMessage ();
			if (data != null) {
				if(title!=null)
					title.sprite = UIHallTexturers.instans.Notice [0];
				RollMsg (data.content);
				Debug.LogError ("rollmsg");
			}
			Debug.LogError ("wait 1s");
			yield return  waitTime;
		}
	}
//*/
	//	IEnumerator StartRollMsg ()
	//	{
	//		while (true) {
	//			nDataInfo.AddRollMessage ();
	//			if (!isRoll && !nDataInfo.IsRollMsgIsEmpty ()) {
	//				FiScrollingNotice data = nDataInfo.GetRollMessage ();
	//				RollMsg (data.content);
	//				//				Debug.LogError ("rollmsg");
	//			}
	//			//Debug.LogError ("wait 1s");
	//			yield return  waitTime;
	//		}
	//	}


	private void OnDestroy ()
	{
		tween.Complete ();//把可能还没销毁的公告动画跑完。修复公告跑的慢的bug
		tweenerInHall.Complete ();
	}


}
