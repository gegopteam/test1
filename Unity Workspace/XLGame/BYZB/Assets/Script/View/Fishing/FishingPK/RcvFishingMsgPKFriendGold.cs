using UnityEngine;
using System.Collections;
using AssemblyCSharp;

//负责：FishingPKFriendGold特有的消息处理
public class RcvFishingMsgPKFriendGold : RcvFishingMsgPKFriend
{
	public void RcvPKFriendGameResultInform( object data )
	{
		FiFriendRoomGameResult nResult = (FiFriendRoomGameResult) data;
		Debug.LogError ("---------RcvPKFriendGameResultInform-------" + nResult.roomType+ "==count==" +nResult.users.Count);
		GameController._instance.GameEnd ( data );
	} 
}
