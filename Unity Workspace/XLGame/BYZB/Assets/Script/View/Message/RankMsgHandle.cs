using System;
using AssemblyCSharp;
using System.Collections.Generic;
using UnityEngine;
using System.Text;

namespace AssemblyCSharp
{
	public class RankMsgHandle:IMsgHandle
	{
		private List< int > mRequestTypes = new List<int> ();

		public RankMsgHandle ()
		{
			
		}

		public void OnInit ()
		{
			EventControl mEventControl = EventControl.instance ();
			mEventControl.addEventHandler (FiEventType.RECV_GET_RANK_RESPONSE, RcvRankResponseHandle);
			//接受Boss结束排名
			mEventControl.addEventHandler (FiEventType.RECV_XL_BOSSMATCHRESULT_RESPOSE, RecvBosRankResult);
		}

		public void SendGetRankInfoRequest (int nType)
		{
			mRequestTypes.Add (nType);
			FiGetRankRequest nRequest = new FiGetRankRequest ();
			nRequest.type = nType;
			DispatchData nData = new DispatchData ();
			nData.type = FiEventType.SEND_GET_RANK_REQUEST;
			nData.data = nRequest;
			DataControl.GetInstance ().PushSocketSnd (nData);
		}

		private void RcvRankResponseHandle (object data)
		{
			FiGetRankResponse nResult = (FiGetRankResponse)data;
			RankInfo nRankInfo = (RankInfo)Facade.GetFacade ().data.Get (FacadeConfig.RANK_MODULE_ID);
//			for (int i = 0; i < nResult.rankList.Count; i++) {
//				Debug.LogError ("nRESSSSSSSS" + nResult.rankList [i].gold + "nresult" + nResult.rankList [i].gameId);
//			}
			if (mRequestTypes.Count > 0) {
				int nRequestType = mRequestTypes [0];
				mRequestTypes.RemoveAt (0);

				if (nResult.rankList.Count == 0) {
					return;
				}
				//爆金版0
				if (nRequestType == 0) {
					FiRankInfo temp = nResult.rankList [0];
					if (MySelfManager.instans != null)
						MySelfManager.instans.coinMuch.text = temp.gold.ToString ();
//					nResult.rankList.RemoveAt (0);
					if (nRankInfo.GetCoinArray ().Count == 0) {
						nRankInfo.AppendCoinData (nResult.rankList);
						//if (UIRanking.instanse != null) {
						//	UIRanking.instanse.OnRecvData( 0 , null );
						//}
					} else
						nRankInfo.AppendCoinData (nResult.rankList);
				} else if (nRequestType == 1) {//土豪版是1
					nRankInfo.AppendRichData (nResult.rankList);
				} else if (nRequestType == 2) {
					nRankInfo.AppendManmonData (nResult.rankList);
//					UIManmonGameShow.instance.ReshRank ();
				}

				//IUiMediator nMediator = Facade.GetFacade ().ui.Get (FacadeConfig.UI_RANK_MODULE_ID);
				//if (nMediator != null) {
				//	nMediator.OnRecvData( nRequestType , nResult.rankList );
				//}
			}


		}

		/// <summary>
		/// 接受BossRank信息
		/// </summary>
		/// <param name="data">Data.</param>
		private void RecvBosRankResult (object data)
		{
//			Debug.LogError ("RecvBosRankResult 执行几次= ");
			RankInfo nRankInfo = (RankInfo)Facade.GetFacade ().data.Get (FacadeConfig.RANK_MODULE_ID);
			FiUserRankArray mRankArray = (FiUserRankArray)data;
			byte[] byteString = mRankArray.content.ToByteArray ();
			string des = Encoding.UTF8.GetString (byteString);
//			Debug.LogError ("des = +des" + des);
			string strArr = des.Replace ("&", "\n");

			//添加Boss数据
			nRankInfo.AppendBossRankData (mRankArray.rankArray);
			//生成Boss排行榜
			string path = "MainHall/RankList/BossRankWindowCanvas";
			GameObject WindowClone = AppControl.OpenWindow (path);
			WindowClone.SetActive (true);

			IUiMediator nMediator = Facade.GetFacade ().ui.Get (FacadeConfig.UI_RANK_MODULE_ID);
			if (nMediator != null) {
				nMediator.OnRecvData (FiEventType.RECV_XL_BOSSMATCHRESULT_RESPOSE, strArr);
			}
		}

		public void OnDestroy ()
		{
			EventControl mEventControl = EventControl.instance ();
			mEventControl.removeEventHandler (FiEventType.RECV_GET_RANK_RESPONSE, RcvRankResponseHandle);
			//接受Boss结束排名
			mEventControl.removeEventHandler (FiEventType.RECV_XL_BOSSMATCHRESULT_RESPOSE, RecvBosRankResult);
		}

	}
}

