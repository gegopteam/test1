using UnityEngine;
using System.Collections;
using System.Collections.Generic;

using AssemblyCSharp;

public class RoomInfoListPK
{
	public const int TYPE_BULLET = 1;
	public const int TYPE_TIME = 2;
	public const int TYPE_POINT = 3;

	private List<FiPkRoomInfo> listRoomInfo = null;
	private int roomType = 0;
	private int pageNum = 0;
	private IData rcvData = null;

	public RoomInfoListPK()
	{
		Init ();
	}

	~RoomInfoListPK()
	{
		UnInit ();
	}

	private void Init()
	{
		listRoomInfo = new List<FiPkRoomInfo> ();
	}

	private void UnInit()
	{
		Clear ();
		listRoomInfo = null;

	}

	public void SetRcv(object obj)
	{
		rcvData = (IData) obj;
	}

	public void OpenRcvInfo()
	{
		if (null == rcvData)
			return;
		foreach(FiPkRoomInfo info in listRoomInfo)
		{
			rcvData.RcvInfo (info);
		}
		rcvData.RcvInfo (null);
		return;
	}

	public void SetRoomType(int type)
	{
		roomType = type;
	}

	public int GetRoomType()
	{
		return roomType;
	}

	public void SetPage(int num)
	{
		pageNum = num;
	}

	public int GetPageNum()
	{
		return pageNum;
	}

	public int GetPageNumPrevious()
	{
		int previousPageNum = pageNum-1;
		if (previousPageNum<0)
		{
			previousPageNum = pageNum;
		}
		return previousPageNum;
	}

	public int GetPageNumNext()
	{
		int pageNumNext = pageNum + 1;
		return pageNumNext;
	}

	public void Add(FiPkRoomInfo roomInfo)
	{
		if (null == roomInfo)
			return;

		FiPkRoomInfo infoGet = Get (roomInfo.roomIndex);
		if(null==infoGet)
		{
			listRoomInfo.Add (new FiPkRoomInfo(roomInfo));
		}
		else
		{
			infoGet.Copy (roomInfo);
		}
	}

	public FiPkRoomInfo Get(int id)
	{
		FiPkRoomInfo infoGet = null;
		foreach(FiPkRoomInfo info in listRoomInfo)
		{
			if(id==info.roomIndex)
			{
				infoGet = info;
			}
		}

		return infoGet;
	}

	public void Remove(int id)
	{
		for(int i=listRoomInfo.Count-1; i>=0; i--)
		{
			if(id==listRoomInfo[i].roomIndex)
			{
				listRoomInfo.Remove (listRoomInfo [i]);
			}
		}
	}

	public void Clear()
	{
		listRoomInfo.Clear ();
	}
}
