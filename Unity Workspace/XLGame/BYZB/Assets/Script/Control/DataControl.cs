/* author:KinSen
 * Date:2017.05.15
 */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DispatchData
{
	public int type;
	public object data;
}

//负责数据的管理
public class DataControl
{
	private static DataControl instance = null;
	private static object objGet = new object ();

	public static DataControl GetInstance ()
	{
		lock (objGet) {
			if (null == instance) {
				instance = new DataControl ();
			}
		}

		return instance;
	}

	public static void DestroyInstance ()
	{
		if (null != instance) {
			instance = null;
		}
	}

	private MyInfo myInfo = null;
	private BackpackInfo backpackInfo = null;
	private RoomInfo roomInfo = null;
	private RankInfo mRankInfo;
	private DragonCardInfo mDragonCardInfo;
	private FriendInfo mFriendInfo;
	private TaskInfo mTaskInfo;
	private RoomInfoListPK roomListPKBullet = null;
	private RoomInfoListPK roomListPKTime = null;
	private RoomInfoListPK roomListPKPoint = null;
	private FriendChatInfo mFriendChatInfo = null;

	private Queue<DispatchData> socketRcv = new Queue<DispatchData> ();
	private Queue<DispatchData> socketSnd = new Queue<DispatchData> ();
	private object objSocketRcv = new object ();
	private object objSocketSnd = new object ();

	private NetControl mNetControl = null;

	private DataControl ()
	{
		myInfo = new MyInfo ();
		backpackInfo = new BackpackInfo ();
		roomInfo = new RoomInfo ();
		mRankInfo = new RankInfo ();
		mDragonCardInfo = new DragonCardInfo ();
		roomListPKBullet = new RoomInfoListPK ();
		roomListPKTime = new RoomInfoListPK ();
		roomListPKPoint = new RoomInfoListPK ();
		mTaskInfo = new TaskInfo ();
		mFriendInfo = new FriendInfo ();
		mFriendChatInfo = new FriendChatInfo ();
		roomListPKBullet.SetRoomType (RoomInfoListPK.TYPE_BULLET);
		roomListPKTime.SetRoomType (RoomInfoListPK.TYPE_TIME);
		roomListPKPoint.SetRoomType (RoomInfoListPK.TYPE_POINT);
	}

	public TaskInfo getTaskInfo ()
	{
		return mTaskInfo;
	}


	~DataControl ()
	{
		if (null != myInfo) {
			myInfo = null;
		}
		if (null != roomInfo) {
			roomInfo = null;
		}
		if (null != roomListPKBullet) {
			roomListPKBullet = null;
		}
		if (null != roomListPKTime) {
			roomListPKTime = null;
		}
		if (null != roomListPKPoint) {
			roomListPKPoint = null;
		}
		CleanSocketSnd ();
		CleanSocketRcv ();
		mRankInfo = null;
	}

	public void ConnectSvr(string addr, int port)
	{
		Debug.Log("--------------ConnectSvr---------------");
		if (mNetControl != null) {
			mNetControl.close ();
			mNetControl = null;
		}
		mNetControl = NetControl.instance ();
		mNetControl.connect (addr, port);
	}

	public void ShutDown ()
	{
		if (mNetControl != null) {
			mNetControl.close ();
			mNetControl = null;
		}
		CleanSocketSnd ();
		CleanSocketRcv ();
	}

	public FriendChatInfo getFriendChatInfo ()
	{
		return mFriendChatInfo;
	}

	public FriendInfo getFriendInfo ()
	{
		return mFriendInfo;
	}

	public MyInfo GetMyInfo ()
	{
		return myInfo;
	}

	public RankInfo GetRankInfo ()
	{
		return mRankInfo;
	}

	public DragonCardInfo GetDragonCardInfo ()
	{
		return mDragonCardInfo;
	}

	public BackpackInfo GetBackpackInfo ()
	{
		return backpackInfo;
	}

	public RoomInfo GetRoomInfo ()
	{
		return roomInfo;
	}

	public void PushSocketRcv (DispatchData data)
	{
		lock (objSocketRcv) {
			socketRcv.Enqueue (data);
		}

	}

	public DispatchData GetSocketRcv ()
	{
		DispatchData data = null;
		lock (objSocketRcv) {
			if (socketRcv.Count > 0)
				data = (DispatchData)socketRcv.Dequeue ();
		}

		return data;
	}

	void CleanSocketRcv ()
	{
		lock (objSocketRcv) {
			socketRcv.Clear (); //需要测试一下，内存有没有泄露
		}
	}

	public void PushSocketSnd (int type, object data)
	{
		DispatchData dispatchData = new DispatchData ();
		dispatchData.type = type;
		dispatchData.data = data;
		PushSocketSnd (dispatchData);
	}

	public void PushSocketSnd (DispatchData data)
	{
		if (mNetControl != null)
			mNetControl.sendData (data);
//		NetControl.instance ().sendData ( data );
		//不需要维护发送数据队列，直接发送
//		lock(objSocketSnd)
//		{
//			socketSnd.Enqueue(data);
//
//		}

	}

	public void PushSocketSndByte (int nType, byte[] data)
	{
		if (mNetControl != null)
			mNetControl.SendBytes (nType, data);
		//		NetControl.instance ().sendData ( data );
		//不需要维护发送数据队列，直接发送
		//		lock(objSocketSnd)
		//		{
		//			socketSnd.Enqueue(data);
		//
		//		}

	}



	public DispatchData GetSocketSnd ()
	{
		DispatchData data = null;
		lock (objSocketSnd) {
			data = (DispatchData)socketSnd.Dequeue ();
		}

		return data;
	}

	void CleanSocketSnd ()
	{
		lock (objSocketSnd) {
			socketSnd.Clear (); //需要测试一下，内存有没有泄露
		}
	}

	public RoomInfoListPK GetRoomInfoListPK (int type)
	{
		RoomInfoListPK infoGet = null;
		switch (type) {
		case RoomInfoListPK.TYPE_BULLET:
			infoGet = roomListPKBullet;
			break;
		case RoomInfoListPK.TYPE_TIME:
			infoGet = roomListPKTime;
			break;
		case RoomInfoListPK.TYPE_POINT:
			infoGet = roomListPKPoint;
			break;
		}

		return infoGet;
	}

}
