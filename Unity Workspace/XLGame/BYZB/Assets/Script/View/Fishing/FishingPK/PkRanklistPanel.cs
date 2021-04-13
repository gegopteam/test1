using UnityEngine;
using System.Collections;
using DG.Tweening;

public class PkRanklistPanel : MonoBehaviour {

	public static PkRanklistPanel _instance;

	float popLength;
	Vector3[] originalRankPos=new Vector3[4];
	public PkRankItem[] rankItem;

	public Transform leftBorder;
	public Transform rightBorder;

	bool isPopShow=false;

	public GameObject btn_PopShow1;
	public GameObject btn_PopShow2;

	void Awake()
	{
		if (null == _instance)
			_instance = this;
		originalRankPos [0] = transform.Find ("RankItemLB").localPosition;
		originalRankPos [1] = transform.Find ("RankItemRB").localPosition;
		originalRankPos [2] = transform.Find ("RankItemLT").localPosition;
		originalRankPos [3] = transform.Find ("RankItemRT").localPosition;

		popLength = rightBorder.position.x - leftBorder.position.x;

		ShowBtnPop2 ();
	}
	void Start()
	{
		switch (GameController._instance.myGameType) {
		case GameType.Classical:
			//SetShow (false);
			Destroy(this.gameObject);
			break;
		case GameType.Bullet:
			//UpdateRankShow ();
			//for (int i = 0; i < rankItem.Length; i++) {
				//rankItem [i].SetShow (false);
			//}
			Invoke ("UpdateRankShow", 0.8f);
			Invoke ("InitPopShow", 1f);
			break;
		case GameType.Point:
			//积分场用不到动态排行榜，改用积分对抗条
			SetShow(false);
			break;
		case GameType.Time:
			Invoke ("UpdateRankShow", 0.8f);
			Invoke ("InitPopShow", 1f);
			break;
		default:
			break;
		}
	}

	void SetShow(bool toShow)
	{
		if (toShow) {
			
		} else {
			this.transform.position = Vector3.right * 1000f;
		}
	}

	public static PkRanklistPanel GetInstance()
	{
		if (null == _instance)
			_instance = new PkRanklistPanel ();
		return _instance;
	}

	public  Vector3 GetOriginalPos(int index)
	{
		if (index < originalRankPos.Length) {
			return originalRankPos [index];
		} else {
			Debug.LogError ("Error: index>=originalRankPos.Length");
			return Vector3.zero;
		}
	}

	public  void UpdateRankShow(){
		UpdateRankItemData ();
		for (int i = 0; i < rankItem.Length-1; i++) {
			int maxIndex = i;
			for (int j = i + 1; j < rankItem.Length; j++) {
				if (rankItem [maxIndex].score <rankItem[j].score) {
					maxIndex = j;
				}
			}
			if (maxIndex!= i) {
				PkRankItem tempItem = rankItem [i];
				rankItem [i] = rankItem [maxIndex];
				rankItem [maxIndex] = tempItem;
			}
		}

		for (int i = 0; i < rankItem.Length; i++) {
			rankItem [i].MoveToRankPos (i);
		}
	}

	void UpdateRankItemData()
	{
		for (int i = 0; i < rankItem.Length; i++) {
			rankItem [i].UpdateData ();
		}
	}

	void PopShow(bool toShow){
		if (toShow) {
			transform.DOMove (transform.position+Vector3.right * popLength, 0.3f);
			Invoke ("ShowBtnPop1", 0.3f);
		} else {
			transform.DOMove (transform .position-Vector3.right * popLength, 0.3f);
			Invoke ("ShowBtnPop2", 0.3f);
		}
		isPopShow = toShow;
	}

	public void TogglePopShow()
	{
		AudioManager._instance.PlayEffectClip (AudioManager.ui_click);
		PopShow (!isPopShow);
	}

	public void InitPopShow()
	{
		PopShow (true);
	}

	void ShowBtnPop1()
	{
		btn_PopShow1.gameObject.SetActive (true);
		btn_PopShow2.gameObject.SetActive (false);
	}

	void ShowBtnPop2()
	{
		btn_PopShow1.gameObject.SetActive (false);
		btn_PopShow2.gameObject.SetActive (true);
	}
}
