using UnityEngine;
using System.Collections;
using UnityEngine.UI;
/// <summary>
/// User interface pk hall mian.Pk场主要管理类
/// </summary>

public class UIPkHallMian : MonoBehaviour {
	private  GameObject WindowClone;
	public Text nickName;

	// Use this for initialization
	void Start () {
		UIStore.HideEvent += Hide;
		UIVIP.SeeEvent += See;
		//nickName.text = UIHall.nickNamestr;
		OneMoreGame ();
	}

	void OneMoreGame()
	{
		MyInfo myInfo = DataControl.GetInstance ().GetMyInfo ();
		if (myInfo.oneMoreGame) {
			
			AppControl.ToView (AppView.PKHALL);
		}
	}


	void Hide()
	{
		transform.gameObject.SetActive (false);
	}

	void See()
	{
		transform.gameObject.SetActive (true);
	}

	public void MatchButton()
	{
		AppControl.ToView (AppView.PKHALL);
	}

	public void FriendButton()
	{
		//跳转到好友约战
		AppControl.ToView(AppView.HALLROOMCARD);
	}
	public void ExitButton()
	{
		AppControl.ToView (AppView.HALL);
	}

	// Update is called once per frame
	void Update () {
	
	}
	void OnDestroy()
	{
		UIStore.HideEvent -= Hide;
		UIVIP.SeeEvent -= See;
	}
}
