using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using DG.Tweening;

public class PkRankItem : MonoBehaviour {


	PkRanklistPanel pkRanklistPanel;

	Text nickNameText;
	Text scoreText;
	Image avatorImage;
	GameObject selfBar;

	public  int score=-1;
	public GunSeat seat;
	GunInUI gunUI;

	bool isFirstUpdate=true ;
	public  bool isActivted=true  ;

	// Use this for initialization
	void Start () {

		gunUI = PrefabManager._instance.GetPrefabObj (MyPrefabType.GunInUI, (int)seat).GetComponent<GunInUI> ();

		pkRanklistPanel = transform.parent.GetComponent<PkRanklistPanel> ();
		nickNameText = transform.Find ("NickName").GetComponent<Text>();
		scoreText = transform.Find ("Score").GetComponent<Text>();
		avatorImage = transform.Find ("AvatorBox/Avator").GetComponent<Image>();
		selfBar = transform.Find ("SelfBar").gameObject;
		selfBar.gameObject.SetActive (false);
		//MoveToRankPos (1);
	}

	public void SetNickName(string name)
	{
		nickNameText.text = name;
	}
	public void SetScore(int score)
	{
		this.score = score;
        scoreText.text =  "得分:"+score.ToString ();
	}
	public void MoveToRankPos(int index)
	{
		if (!isActivted)
			return;
		transform.DOLocalMove (pkRanklistPanel.GetOriginalPos (index), 1f);
	}

	public void UpdateData()
	{
		if (!isActivted)
			return;
		if (isFirstUpdate) {
			if (!gunUI.gunControl.isActived) {
				SetShow (false);
			} else {
				SetShow (true);
				SetNickName (gunUI.GetNickName ());
				isFirstUpdate = false;
				if (gunUI.isLocal)
					selfBar.gameObject.SetActive (true);
			}
		}
			
		SetScore (gunUI.GetScore ());
	}

	public  void SetShow(bool toShow)
	{
		if (toShow) {
			avatorImage.sprite = gunUI.gunControl.GetAvatorSprite ();
		} else {
			score = -1;
		}
		nickNameText.gameObject.SetActive (toShow);
		scoreText.gameObject.SetActive (toShow);
		avatorImage.transform.parent.gameObject.SetActive (toShow);
		selfBar.gameObject.SetActive (false);
		isActivted = toShow;
	}
}
