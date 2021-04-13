using UnityEngine;
using System.Collections;
using System.Runtime.InteropServices;
using System.Net;
using System.Net.Sockets;
using System;
using System.Threading;
using System.IO;
using System.Collections.Generic;
using AssemblyCSharp;

public class FriendInfo : IDataProxy
{
	//申请的好友
	private List<FiFriendInfo> mApplyFriends = new List<FiFriendInfo> ();

	//存在的好友
	private List<FiFriendInfo> mFriends = new List<FiFriendInfo> ();

	private bool bUpdated = false;

	public int countLimits;

	public FriendInfo ()
	{
		
	}

	public void DeleteUser( int userId )
	{
		for( int i = 0 ; i < mFriends.Count ; i ++ )
		{
			if (mFriends [i].userId == userId) {
				mFriends.RemoveAt ( i );
				break;
			}
		}
	}

	public void DeleteApply( int userId )
	{
		for( int i = 0 ; i < mApplyFriends.Count ; i ++ )
		{
			if (mApplyFriends [i].userId == userId) {
				mApplyFriends.RemoveAt ( i );
				break;
			}
		}
	}

	public List<FiFriendInfo> GetFriendList()
	{
		return mFriends;
	}

	public void SetApplyFriend( List<FiFriendInfo> nApplyFriends )
	{
		mApplyFriends = nApplyFriends;
	}

	public void SetFriendData( List<FiFriendInfo> nFriendList )
	{
		List<FiFriendInfo> tmp = nFriendList;
		FiFriendInfo temp;
		for (int i = 0; i < nFriendList.Count; i++) {
			temp = nFriendList [i];
			if (temp.status == 1) {
				tmp.RemoveAt (i);
				tmp.Insert (0,temp);
			}
		}
		mFriends = tmp;
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

	public bool isUpdated()
	{
		return bUpdated;
	}

	public void SetUpdate( bool nValue )
	{
		bUpdated = nValue;
	}

	public void AddApplyInfo( FiGetFriendApplyResponse  nValue )
	{
		if ( nValue.friends.Count != 0 ) {
			bUpdated = true;
		} else {
			bUpdated = false;
		}
	}

	public  List<FiFriendInfo> getApplyFriends()
	{
		return mApplyFriends;
	}
		
}

