using UnityEngine;
using System.Collections;
using UnityEngine.UI;
/// <summary>
/// Room manager.进入什么比赛显示什么比赛的房间列表
/// </summary>
public class RoomManager : MonoBehaviour {
	[SerializeField]
	InfinityGridLayoutGroup infinityGridLayoutGroup;

	int amount = 12;

	public GameObject bulletRect;
	public GameObject timeRect;
	public GameObject integralRect;

	public static  RoomMessage[] roomMessage;

	private RoomListInit roomListInit = new RoomListInit();
	// Use this for initialization
	void Start()
	{
		Tool.Log ("根据进入不同的场次初始化不同的房间的滑动",true);
		Debug.Log (UIRoom.name);
		switch (UIRoom.name) {
		case "Bullet":
			bulletRect.SetActive (true);
			roomMessage = GetComponentsInChildren<RoomMessage> ();
			infinityGridLayoutGroup = bulletRect.GetComponentInChildren<InfinityGridLayoutGroup> ();
			timeRect.SetActive (false);
			integralRect.SetActive(false);
			Init ();
			break;
		case "Time":
			timeRect.SetActive (true);
			roomMessage = GetComponentsInChildren<RoomMessage> ();
			infinityGridLayoutGroup = timeRect.GetComponentInChildren<InfinityGridLayoutGroup> ();
			bulletRect.SetActive (false);
			integralRect.SetActive(false);
			Init ();
			break;
		case "Integral":
			integralRect.SetActive(true);
			roomMessage = GetComponentsInChildren<RoomMessage> ();
			infinityGridLayoutGroup = integralRect.GetComponentInChildren<InfinityGridLayoutGroup> ();
			timeRect.SetActive (false);
			bulletRect.SetActive (false);
			Init ();
			break;

		}
		Debug.Log (roomMessage.Length);
	}

	void Init()
	{
		////初始化数据列表;
		infinityGridLayoutGroup.SetAmount(amount);
		infinityGridLayoutGroup.updateChildrenCallback = UpdateChildrenCallback;
	}

//	void OnGUI()
//	{
//		Tool.ShowInGUI ();
//	}

	public void ReNewButton()
	{
		roomListInit.RenewList ();
		infinityGridLayoutGroup.SetAmount(amount);
	}

	void UpdateChildrenCallback(int index, Transform trans)
	{

	}

	// Update is called once per frame
	void Update()
	{

     }
}


