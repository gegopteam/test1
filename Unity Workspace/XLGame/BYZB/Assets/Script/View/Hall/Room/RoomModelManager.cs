using UnityEngine;
using System.Collections;
/// <summary>
/// Room model manager.根据Toggle改变值时发送不同的事件，传送不同的值，针对的是主界面筛选房间
/// </summary>
public class RoomModelManager : MonoBehaviour {
	
	public delegate void CoinDelegate (string coinMuch);
	public static event CoinDelegate CoinEvent;

	public delegate void BulletDelegate (string bulletMuch);
	public static event BulletDelegate BulletEvent;

	public delegate void TimeDelegate (string time);
	public static event TimeDelegate TimeEvent;

	public delegate void RoundDelegate (string round);
	public static event RoundDelegate RoundEvent;

	public delegate void PersonDelegate (string PersonMuch);
	public static event PersonDelegate PersonEvent;

	public delegate void StateDelegate (string state);
	public static event StateDelegate StateEvent;

	// Use this for initialization
	void Start () {
	
	}

	public void CoinChange(string coinMuch)
	{
		if (CoinEvent != null) {
		
			CoinEvent (coinMuch);
		}
	}
		
	public void BulletChange(string bulletMuch)
	{
		if (BulletEvent != null) {
		
			BulletEvent (bulletMuch);
		}
	}

	public void GamePerson(string gamePerson)
	{
		if (PersonEvent != null) {
		
			PersonEvent (gamePerson);
		}
	}

	public void State(string state)
	{
		if (StateEvent != null) {
		  
			StateEvent (state);
		}
	}

	public void TimeGame(string time)
	{
		if (TimeEvent != null) {
		
			TimeEvent (time);
		}
	}

	public void Round(string round)
	{
		if (RoundEvent != null) {
		
			RoundEvent (round);
		}
	}

	// Update is called once per frame
	void Update () {
	
	}
}
