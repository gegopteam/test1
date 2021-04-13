using UnityEngine;
using System.Collections;

using System.Collections.Generic;

using AssemblyCSharp;

public class RoomInfo : IDataProxy
{
	public long roomIndex; //房间ID
	public string roomName; //房间名
	public int roomType; //房间类型
	public int roomMultiple; //房间倍数
	//public bool hasPassword; //房间是否有密码

	public int roomOwnerID; //房主ID
	public int currentPlayerCount; //房间当前玩家数

	public int goldType;
	//public int bulletType;
	public int timeType;
	//public int pointType;
	//public int playerNumType;
	public int roundType;

	public bool begun;
	//public long createTime;

	//public bool isReconnectInPKRoom = false;

	private List<FiUserInfo> listUser = new List<FiUserInfo>();
	//private List<FiOtherGameInfo> listUserReconnect = new List<FiOtherGameInfo>();

	public RoomInfo()
	{
		ClearRoomInfo ();
	}

	~RoomInfo()
	{
		Clear ();
	}

	public void OnAddData( int nType, object nData )
	{

	}

	public void OnInit()
	{

	}

	public void OnDestroy()
	{

	}

	public void ClearRoomInfo()
	{
		roomIndex = 0;
		roomName = "";
		roomType = 0;
		roomMultiple = 0;
		//hasPassword = false;

		roomOwnerID = 0;
		currentPlayerCount = 0;

		goldType = 0;
		//bulletType = 0;
		timeType = 0;
		//pointType = 0;
		//playerNumType = 0;
		roundType = 0;

		begun = false;
		//createTime = 0;
	}

	public void SetRoomInfo(FiPkRoomInfo info)
	{
		roomType = info.roomType;
		roomIndex = info.roomIndex;
		goldType = info.goldType;
		//bulletType = info.bulletType;
		timeType = info.timeType;
		//pointType = info.pointType;
		//playerNumType = info.playerNumType;
		//roomName = info.roomName;
		//hasPassword = info.hasPassword;
		begun = info.begun;
		currentPlayerCount = info.currentPlayerCount;
		//createTime = info.createTime;
	}

	public void InitUser (List<FiUserInfo> userArray)
	{
		FiUserInfo user = null;
		foreach(FiUserInfo userInfo in userArray)
		{
			if(null!=userInfo)
			{
				user = new FiUserInfo (userInfo);
				listUser.Add (user);
			}
		}
	}

	public void AddUser(FiUserInfo userInfo)
	{
		listUser.Add (userInfo);
	}

	public void RemoveUser(int userId)
	{
		if (0 == listUser.Count)
			return;
		for(int i=listUser.Count-1; i>=0; i--)
		{
			if(userId==listUser[i].userId)
			{
				listUser.Remove (listUser [i]);
			}
		}
	}

	public void ClearUser()
	{
		listUser.Clear ();
	}

	public FiUserInfo GetUser(int seatIndex)
	{
		FiUserInfo user = null;
		foreach(FiUserInfo userInfo in listUser)
		{
			if(seatIndex == userInfo.seatIndex)
			{
				user = userInfo;
				break;
			}
		}

		return user;
	}

	public List<FiUserInfo> GetUsers()
	{
		return listUser;
	}

	public void ChangeCannonMultiple(int userId, int cannonMultiple)
	{
		foreach(FiUserInfo userInfo in listUser)
		{
			if(userId == userInfo.userId)
			{
				userInfo.cannonMultiple = cannonMultiple;
				break;
			}
		}
	}

	public void Clear()
	{
		ClearUser ();
		ClearRoomInfo ();
	}

	/*public void AddUserReconnect(FiOtherGameInfo user)
	{
		if (null == user)
			return;
		
		listUserReconnect.Add (user);
	}

	public FiOtherGameInfo GetUserReconnect(int userID)
	{
		FiOtherGameInfo gameInfo = null;
		foreach(FiOtherGameInfo user in listUserReconnect)
		{
			if(userID==user.userId)
			{
				gameInfo = user;
				break;
			}
		}
		return gameInfo;
	}

	public void ClearUserReconnect()
	{
		if(null!=listUserReconnect)
		{
			listUserReconnect.Clear ();
		}

	}

	public List<FiProperty> GetUserReconnectProperty(int userID)
	{
		List<FiProperty> listProperty = null;
		foreach(FiOtherGameInfo user in listUserReconnect)
		{
			if(userID==user.userId)
			{
				listProperty = user.properties;
				break;
			}
		}

		return listProperty;
	}*/

}
