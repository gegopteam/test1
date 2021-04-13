/* author:KinSen
 * Date:2017.07.25
 */

using UnityEngine;
using System.Collections;
using System.Collections.Generic;

using AssemblyCSharp;

//负责：FishingPKBullet特有的消息处理
public class RcvFishingMsgPKBullet : RcvFishingMsgPK 
{
	public RcvFishingMsgPKBullet()
	{

	}

	~RcvFishingMsgPKBullet()
	{

	}
	//	public class  FiGoldGameResult
	//	{//子弹赛和限时赛结算
	//		public List<FiPlayerInfo> info;	
	//	}
	//public const int RECV_PK_GOLD_GAME_RESULT_INFORM         =  300031;
	public void RcvPKGoldGameResultInform(object data)
	{
		FiGoldGameResult goldGame = (FiGoldGameResult)data;

		Debug.LogError("BulletEnd:"+ goldGame.ToString ());
		GameController._instance.GameEnd (goldGame );
	}
}
