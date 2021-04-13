/* author:KinSen
 * Date:2017.07.25
 */

using UnityEngine;
using System.Collections;
using System.Collections.Generic;

using AssemblyCSharp;

//负责：FishingPKPoint特有的消息处理
public class RcvFishingMsgPKPoint : RcvFishingMsgPK
{
	public RcvFishingMsgPKPoint()
	{

	}

	~RcvFishingMsgPKPoint()
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
		Debug.LogError("PointEnd:"+ goldGame.ToString ());
		GameController._instance.GameEnd (goldGame );
	}




}