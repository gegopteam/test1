using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using DG.Tweening;
using AssemblyCSharp;


public class UpgradeBroad : MonoBehaviour {

	Vector3 originPos;
	public Text content;
	public bool isRoll = false;
	public static UpgradeBroad instans;
	BroadCastInfo nDataInfo;

	//int i = 1;

	Tweener tween;
	Tweener tweenerInHall;

	private void Awake()
    {
		if (instans != null)
		{
			DestroyImmediate(instans.gameObject);
		}
		instans = this;
		content.transform.GetComponent<RectTransform>().anchoredPosition = Vector3.right * 400f;
		
		nDataInfo = (BroadCastInfo)Facade.GetFacade().data.Get(FacadeConfig.BROADCAST_MODULE_ID);
		tweenerInHall.Complete();
		tweenerInHall = null;
	}

    // Use this for initialization
    void Start () {
		Debug.Log(" ~~~~~UpgradeBroad~~~~~Start~~~~~ ");
		RollComplete();
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	//播放完毕，判断是否存在下一条消息
	public void RollComplete()
	{
		Debug.Log("~~~~~UpgradeBroad~~~~~RollComplete");
        //broadCastObj.SetActive (false);

        //test
        //RollMsg("1234567890 abcdefghijklmnopqrstuvwxyz", 0);
        //switch ((i%10))
        //      {
        //	case 1:
        //		RollMsg("1234567890 abcdefghijklmnopqrstuvwxyz", 0);
        //		break;
        //	case 2:
        //		RollMsg("abcdefghijklmnopqrstuvwxyz 1234567890", 0);
        //		break;
        //	case 3:
        //		RollMsg("qaz abcdefghijklmnopqrstuvwxyz", 0);
        //		break;
        //	case 4:
        //		RollMsg("wsx abcdefghijklmnopqrstuvwxyz", 0);
        //		break;
        //	case 5:
        //		RollMsg("edc abcdefghijklmnopqrstuvwxyz", 0);
        //		break;
        //	case 6:
        //		RollMsg("rfv abcdefghijklmnopqrstuvwxyz", 0);
        //		break;
        //	case 7:
        //		RollMsg("tgb abcdefghijklmnopqrstuvwxyz", 0);
        //		break;
        //	case 8:
        //		RollMsg("yhn abcdefghijklmnopqrstuvwxyz", 0);
        //		break;
        //	case 9:
        //		RollMsg("ujm abcdefghijklmnopqrstuvwxyz", 0);
        //		break;
        //}

        isRoll = false;
        BroadCastUnit data = nDataInfo.GetLevelUpgradeMessage();
        Debug.LogError("data:" + data.content);
        if (data != null)
        {
            RollMsg(data.content, data.type);
        }
    }

	public void RollMsg(string msg, int type = 0)
	{
		//Debug.Log("~~~~~UpgradeBroad~~~~~RollMsg~~~~~type = " + type);
		//Debug.Log("~~~~~UpgradeBroad~~~~~RollMsg~~~~~msg = " + msg);
		isRoll = true;
		content.text = msg;

		content.transform.GetComponent<RectTransform>().anchoredPosition = Vector3.right * 41f;
		DoRollAni();
	}

	//播放动画
	void DoRollAni()
	{
		//Debug.Log("~~~~~UpgradeBroad~~~~~DoRollAni");
		//Debug.Log("~~~~~UpgradeBroad~~~~~DoRollAni content.preferredWidth = "+ content.preferredWidth);
		//tween = content.rectTransform.DOLocalMoveY (content.preferredHeight, 1);

        //播放速度
        int time = 10;
		//if (content.preferredWidth > 400)
		//{
		//	time = (int)content.preferredWidth / 85;
		//}
		//Debug.Log("~~~~~UpgradeBroad~~~~~DoRollAni time = "+ time);

		if (tweenerInHall == null)
		{
			//Debug.LogError("调一次动画在大厅");
			tweenerInHall = content.rectTransform.DOLocalMoveX(-content.preferredWidth * 2f, time);
			tweenerInHall.SetEase(Ease.Linear);
			tweenerInHall.SetLoops(1);
			tweenerInHall.OnComplete(delegate () {
				//Invoke("RollComplete",4f);
				tweenerInHall = null;
				//Debug.LogError("动画结束了在大厅");
				RollComplete();
			});
		}
		//i++;
	}

	private void OnDestroy()
	{
		tween.Complete();//把可能还没销毁的公告动画跑完。修复公告跑的慢的bug
		tweenerInHall.Complete();
	}
}
