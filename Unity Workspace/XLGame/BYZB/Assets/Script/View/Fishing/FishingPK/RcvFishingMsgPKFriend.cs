using UnityEngine;
using System.Collections;

using AssemblyCSharp;

//负责：FishingPKFriend通用的消息接收处理
public class RcvFishingMsgPKFriend : RcvFishingMsgPK
{
	public void RcvPKStartGameResponse(object data)
	{
		FiStartPKGameResponse startGame = (FiStartPKGameResponse)data;
		if(0==startGame.result)
		{
			fishingObjects.InitCannonInfo ();
		}
		else
		{
			
		}
	}

	public void RcvPKFriendLeaveRoom( object data )
	{
		
	}

}
