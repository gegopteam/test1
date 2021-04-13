/* author:KinSen
 * Date:2017.07.25
 */

using UnityEngine;
using System.Collections;
using System.Collections.Generic;

using AssemblyCSharp;


//负责：FishingPKTime特有的消息处理
public class RcvFishingMsgPKTime : RcvFishingMsgPK
{
	public RcvFishingMsgPKTime()
	{

	}

	~RcvFishingMsgPKTime()
	{

	}

	public void RcvPKStartGameResponse(object data)
	{
		FiStartPKGameResponse startGame = (FiStartPKGameResponse)data;
		if(0==startGame.result)
		{
			fishingObjects.InitCannonInfo ();
		}
		else
		{//非零房间匹配失败
			Debug.LogError ("Fishing 房间匹配失败");

			//先提示房间匹配失败
			Tool.OutLogWithToFile ("Fishing 房间匹配失败");

			//然后跳转到大厅界面
			AppControl.ToView (AppView.HALL);

		}

	}

	//	public class  FiGoldGameResult
	//	{//子弹赛和限时赛结算
	//		public List<FiPlayerInfo> info;	
	//	}
	//public const int RECV_PK_GOLD_GAME_RESULT_INFORM         =  300031;
	public void RcvPKGoldGameResultInform(object data) //和子弹赛共用同一套
	{
		FiGoldGameResult goldGame = (FiGoldGameResult)data;
		Debug.LogError("TimeGameEnd:"+ goldGame.ToString ());
		GameController._instance.GameEnd (goldGame );
	}

	//	public class   FiPointGameRoundResult
	//	{//积分赛阶段下发胜利者
	//		public int round;
	//		public int winnerUserId;	
	//	}
	//public const int RECV_PK_POINT_GAME_ROUND_RESULT_INFORM	 =  300033;
	public void RcvPKPointGameRoundResultInform(object data)
	{
		FiPointGameRoundResult pointGameRound = (FiPointGameRoundResult)data;
	
	}

	//	public class  FiPointGameResult
	//	{//几分赛赛结算
	//		public List<int> winnerUserId;	
	//	}
	//public const int RECV_PK_POINT_GAME_RESULT_INFORM        =  300032;
	public void RcvPKPointGameResultInform(object data)
	{
		FiPointGameResult pointGame = (FiPointGameResult)data;
	}
}
