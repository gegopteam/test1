using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
/// <summary>
/// User interface room.商城，以及根据name进入不同赛场对条件筛选的显示初始化，获得筛选条件的值
/// </summary>
public class UIRoom : MonoBehaviour {
	public  Sprite lockImage;
	public static string name;



	public GameObject BulletModel;
	public GameObject TimeModel;
	public GameObject IntegralModel;

	private GameObject WindowClone;

	private string useCoinMuch;
	private string useBulletMuch;
	private string useTime;
	private string useRound;
	private string useGamePerson;
	private string useState;

	void Awake()
	{
		//获得筛选条件
		RoomModelManager.CoinEvent += GetCoinMuch;
		RoomModelManager.BulletEvent += GetBulletMuch;
		RoomModelManager.TimeEvent += GetTime;
		RoomModelManager.PersonEvent += GetPersonMuch;
		RoomModelManager.RoundEvent += GetRound;
		RoomModelManager.StateEvent += GetState;

		//Debug.Log (roomMessage.Length);
	}

	void GetCoinMuch (string CoinMuch)
	{
		useCoinMuch = CoinMuch;
		Tool.Log ("入场金币"+CoinMuch,true);
	}

	void GetBulletMuch(string bulletMuch)
	{
		useBulletMuch = bulletMuch;
		Tool.Log ("子弹数量"+useBulletMuch,true);
	}

	void GetTime(string time)
	{
		useTime = time;
		Tool.Log ("时间"+useTime,true);
	}

	void GetPersonMuch(string personMuch)
	{
		useGamePerson = personMuch;
		Tool.Log ("游戏人数"+useGamePerson,true);
	}

	void GetRound(string round)
	{
		useRound = round;
		Tool.Log ("局数"+useRound,true);
	}

	void GetState(string state)
	{
		useState = state;
		Tool.Log ("游戏状态"+useState,true);
	}
		
	// Use this for initialization
	void Start () {
		UIStore.HideEvent += Hide;
		UIVIP.SeeEvent += See;
		BulletModel.SetActive (false);	
		TimeModel.SetActive (false);
		IntegralModel.SetActive (false);
	
	}

	void Hide()
	{
		transform.gameObject.SetActive (false);
	}

	void See()
	{
		transform.gameObject.SetActive (true);
	}

//	void OnGUI()
//	{
//		Tool.ShowInGUI ();
//	}

	void ShowButtle()
	{
		BulletModel.SetActive (true);	
		TimeModel.SetActive (false);
		IntegralModel.SetActive (false);
	}

	void ShowTime()
	{
		BulletModel.SetActive (false);	
		TimeModel.SetActive (true);
		IntegralModel.SetActive (false);
	}

	void ShowIntegral()
	{
		BulletModel.SetActive (false);
		TimeModel.SetActive (false);
		IntegralModel.SetActive (true);
	}

	public void ExitButton()
	{
		AppControl.ToView (AppView.PKHALL);
	}

	public void CreateRoom()
	{
		Tool.Log ("打开创建窗口",true);
		string path = "Window/MadeRoom";
		WindowClone = AppControl.OpenWindow (path);
		WindowClone.SetActive (true);
	}

	public void FindRoom()
	{
		Tool.Log ("打开查找窗口",true);
		string path = "Window/FindRoom";
		WindowClone = AppControl.OpenWindow (path);
		WindowClone.SetActive (true);
	}

	public void QuickJoin()
	{
		Tool.Log ("快速加入");
		//向服务器请求，服务器决定进入到哪个房间
	}

	// Update is called once per frame
	void Update () {
		switch (name) {
		case "Bullet":
			ShowButtle ();
			break;
		case "Time":
			ShowTime ();
			break;
		case "Integral":
			ShowIntegral ();
			break;
		}
	}

	void OnDestroy()
	{
		UIStore.HideEvent -= Hide;
		UIVIP.SeeEvent -= See;
		RoomModelManager.CoinEvent -= GetCoinMuch;
		RoomModelManager.BulletEvent -= GetBulletMuch;
		RoomModelManager.TimeEvent -= GetTime;
		RoomModelManager.PersonEvent -= GetPersonMuch;
		RoomModelManager.RoundEvent -= GetRound;
		RoomModelManager.StateEvent -= GetState;
	}
}
