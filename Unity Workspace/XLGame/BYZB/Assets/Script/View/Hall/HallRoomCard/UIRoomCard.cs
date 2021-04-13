using UnityEngine;
using System.Collections;
using UnityEngine.UI;
/// <summary>
/// User interface room card.房卡模式
/// </summary>

public class UIRoomCard : MonoBehaviour {
	private GameObject WindowClone;
	public Text nickName;
	// Use this for initialization
	void Start () {
		UIStore.HideEvent += Hide;
		UIVIP.SeeEvent += See;
		//nickName.text = UIHall.nickNamestr;
	}

	void Hide()
	{
		transform.gameObject.SetActive (false);
	}

	void See()
	{
		transform.gameObject.SetActive (true);
	}

	public void CreateButton()
	{
//		string path = "Window/MadeRoom";
//		WindowClone = AppControl.OpenWindow (path);
//		WindowClone.SetActive (true);
		GameObject Window = Resources.Load ("Window/MadeRoom")as GameObject;
		WindowClone = Instantiate (Window);
	}

	public void JoinButton()
	{
		//跳转到好友约战
		GameObject Window = Resources.Load ("Window/FindRoom")as GameObject;
		WindowClone = Instantiate(Window);
	}
	public void ExitButton()
	{
		AppControl.ToView (AppView.PKHALLMAIN);
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
