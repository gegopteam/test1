using UnityEngine;
using System.Collections;

using AssemblyCSharp;

public class UIFishing : MonoBehaviour
{
	public static UIFishing _instance;
	private AppControl appControl = null;
	private DispatchControl dispatchControl = null;
	private DataControl dataControl = null;
	private MyInfo myInfo = null;
	private RoomInfo roomInfo = null;

	private GameObject WindowClone;

	void Awake ()
	{
		_instance = this;
		appControl = AppControl.GetInstance ();
		dispatchControl = DispatchControl.GetInstance ();
		dataControl = DataControl.GetInstance ();
		myInfo = dataControl.GetMyInfo ();
		roomInfo = dataControl.GetRoomInfo ();
	}

	// Use this for initialization
	void Start ()
	{
		//初始化设置Fishing场景消息接收对象
		UIFishingMsg.GetInstance ();
	}

	// Update is called once per frame
	void Update ()
	{
		if (Input.GetKey (KeyCode.Q)) {
		
			string path = "Window/PropsGetWindow";
			WindowClone = AppControl.OpenWindow (path);
			WindowClone.SetActive (true);
		}
	}

	private void Init ()
	{
	
	}

	private void UnInit ()
	{
		
	}

	public void OnBack ()
	{
		AudioManager._instance.PlayBgm (AudioManager.bgm_none);
		//经典场操作
		if (roomInfo.roomType == TypeFishing.CLASSIC || roomInfo.roomType == TypeFishing.REDPACKET) {
			if (GameController._instance.isExperienceMode) {
				DataControl.GetInstance ().GetMyInfo ().levelVip = 0;
				DataControl.GetInstance ().GetMyInfo ().cannonMultipleMax = myInfo.cannonMultipleMax;
				DataControl.GetInstance ().GetMyInfo ().isTestRoom = 1;
			}
			DataControl.GetInstance ().GetMyInfo ().gold = PrefabManager._instance.GetLocalGun ().currentGold;
			DataControl.GetInstance ().GetMyInfo ().diamond = PrefabManager._instance.GetLocalGun ().curretnDiamond;
			UIFishingMsg.GetInstance ().SndLeaveRoom ();

		}//pk场子弹赛，积分赛，时间赛操作
		else if (roomInfo.roomType == TypeFishing.PKBullet || roomInfo.roomType == TypeFishing.PKPoint || roomInfo.roomType == TypeFishing.PKTime) {
			//UIFishingMsg.GetInstance ().SndPKLeaveRoomRequest (roomInfo.roomType, (int)roomInfo.roomIndex, roomInfo.goldType);
			Facade.GetFacade ().message.fishPkRoom.SendPKLeaveRoomRequest (roomInfo.roomType, (int)roomInfo.roomIndex, roomInfo.goldType);

		} else if (roomInfo.roomType == TypeFishing.PKFriendCard || roomInfo.roomType == TypeFishing.PKFriendGold) {
			//UIHallMsg.GetInstance ().SndPKLeaveFriendRoomRequest (roomInfo.roomType, (int)roomInfo.roomIndex);
			Facade.GetFacade ().message.fishFriend.SendPKLeaveFriendRoomRequest (roomInfo.roomType, (int)roomInfo.roomIndex);
		} else if (roomInfo.roomType == TypeFishing.REDPACKET) {
			new RedPacketMsgHandle ().SendLeaveRedPacketRoomRequest ((int)roomInfo.roomIndex, (int)roomInfo.roomType);
		}
		//myInfo.SetState ( MyInfo.STATE_IN_HALL );
	}

	void OnDestroy ()
	{
		Tool.OutLogWithToFile ("销毁数据");
		UIFishingObjects.GetInstance ().Clear ();

		if (null != roomInfo) {
			roomInfo.Clear ();
		}

		if (null != myInfo) {
			myInfo.SetCannonInfo (null);
			myInfo.ClearFishingInfo ();
		}
	}

	void OnGUI ()
	{
		//Tool.ShowInGUI ();
	}
		

}