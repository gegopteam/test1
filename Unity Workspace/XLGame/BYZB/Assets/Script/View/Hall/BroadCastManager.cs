using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using DG.Tweening;
using AssemblyCSharp;

public class BroadCastManager : MonoBehaviour
{

	public GameObject broadCastObj;
	public Text content;
	public bool isRoll = false;
	public static BroadCastManager instans;
	BroadCastInfo nDataInfo;
	public Image title;
	bool courtineLock = false;
	Tweener tween;
    Tweener tweenerInHall;
	WaitForSeconds waitTime;
	Vector3 originPos;
	public Sprite[] ShowNoticeRol;
	public Image showNoticeImage;

	void Awake ()
	{
		if (instans != null) {
			DestroyImmediate (instans.gameObject);
		}
		instans = this;
		content.transform.GetComponent<RectTransform> ().anchoredPosition = Vector3.right * 40f;
		nDataInfo = (BroadCastInfo)Facade.GetFacade ().data.Get (FacadeConfig.BROADCAST_MODULE_ID);
        tween.Complete();
        tweenerInHall.Complete();
        tween = null;
        tweenerInHall = null;
	}

	void Start ()
	{
		if (GameController._instance != null) { //如果在渔场，并且是pk场，直接摧毁
			if (GameController._instance.myGameType != GameType.Classical) {
				//transform.position = Vector3.up * 1000f;
				Destroy (this.gameObject);
			} else { //如果是在普通场
				originPos = transform.position;
				SetShowInFishingScene (false);
			}
		}
		waitTime = new WaitForSeconds (1.0f);
		StartCoroutine (StartRollMsg ());
		RollComplete ();
	}
	//播放下一条消息
	public void RollMsg (string msg, int type = 0)
	{
		if (isRoll)
			return;
		//broadCastObj.SetActive (true);
		isRoll = true;
		if (type == 0) {
//			Debug.LogError ("11111111111111");
			showNoticeImage.gameObject.SetActive (true);
			showNoticeImage.SetNativeSize ();
			showNoticeImage.rectTransform.localScale = new Vector3 (0.6f, 0.6f, 0.6f);
		} else if (type == 1) {
			showNoticeImage.gameObject.SetActive (true);
			showNoticeImage.sprite = ShowNoticeRol [0];
			showNoticeImage.rectTransform.sizeDelta = new Vector2 (50f, 27f);
			showNoticeImage.rectTransform.localScale = new Vector3 (0.6f, 0.6f, 0.6f);

		} else if (type == 2) {
			showNoticeImage.gameObject.SetActive (true);
			showNoticeImage.sprite = ShowNoticeRol [1];
			showNoticeImage.SetNativeSize ();
			showNoticeImage.rectTransform.localScale = new Vector3 (0.4f, 0.4f, 0.4f);
		}

		content.text = msg;
		if (title != null)
			content.transform.GetComponent<RectTransform> ().anchoredPosition = Vector3.right * 41f;
		else
			content.transform.GetComponent<RectTransform> ().anchoredPosition = Vector3.right * 2.5f;
		DoRollAni ();
	}

	//播放动画
	void DoRollAni ()
	{
		SetShowInFishingScene (true);
		//tween = content.rectTransform.DOLocalMoveY (content.preferredHeight, 1);
		int time = 12;
		if (content.preferredWidth > 400) {
			time = (int)content.preferredWidth / 45;
		}

        if (GameController._instance != null)
        {
            if (tween==null)
            {
                //Debug.LogError("调一次动画在渔场");
                tween = content.rectTransform.DOLocalMoveX(-content.preferredWidth, time);
                tween.SetEase(Ease.Linear);
                tween.OnComplete(delegate () {
                //Invoke("RollComplete",4f);
                //Debug.LogError("动画结束了在渔场");
                tween = null;
                RollComplete();
            }); 
            }
        }
        else
        {
            if (tweenerInHall==null)
            {
                //Debug.LogError("调一次动画在大厅");
                tweenerInHall = content.rectTransform.DOLocalMoveX(-content.preferredWidth, time);
                tweenerInHall.SetEase(Ease.Linear);
                tweenerInHall.OnComplete(delegate () {
                //Invoke("RollComplete",4f);
                tweenerInHall = null;
                //Debug.LogError("动画结束了在大厅");
                RollComplete();
               
                });  
            }
          
        }
	}
	//播放完毕，判断是否存在下一条消息
	public void RollComplete ()
	{
		//broadCastObj.SetActive (false);
		isRoll = false;
		BroadCastUnit data = nDataInfo.GetUserMessage ();
		if (data == null) {
			data = nDataInfo.GetGameInfo ();
			if (data == null) {
				FiScrollingNotice rollData = nDataInfo.GetRollMessage ();
				if (rollData == null) {
					content.transform.GetComponent<RectTransform> ().anchoredPosition = Vector3.right * 40f;
					//SetShowInFishingScene(false);
					Invoke ("DelayHide", 2f);
					return;
				}
				if (title != null)
					title.sprite = UIHallTexturers.instans.Notice [0];
				RollMsg (rollData.content);
				return;
			}
			if (title != null)
				title.sprite = UIHallTexturers.instans.Notice [0];
			RollMsg (data.content, data.type);
		} else {
			if (title != null)
				title.sprite = UIHallTexturers.instans.Notice [1];
			RollMsg ("<color=#f6fc29ff>" + Tool.GetName (data.nickname, 6) + ":</color>" + data.content);
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
*/
	IEnumerator StartRollMsg ()
	{
		while (true) {
			nDataInfo.AddRollMessage ();
			if (!isRoll && !nDataInfo.IsRollMsgIsEmpty ()) {
				FiScrollingNotice data = nDataInfo.GetRollMessage ();
				if (title != null)
					title.sprite = UIHallTexturers.instans.Notice [0];
				RollMsg (data.content);
//				Debug.LogError ("rollmsg");
			}
			//Debug.LogError ("wait 1s");
			yield return  waitTime;
		}
	}

	void SetShowInFishingScene (bool toShow)
	{
		if (GameController._instance == null)
			return;
		if (toShow) {
			transform.position = originPos;
		} else {
			transform.position = Vector3.up * 1000f;
		}
	}

	void DelayHide ()
	{
		SetShowInFishingScene (false);
	}

    private void OnDestroy()
    {
        tween.Complete();//把可能还没销毁的公告动画跑完。修复公告跑的慢的bug
        tweenerInHall.Complete();
    }


}
