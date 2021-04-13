using System;
using AssemblyCSharp;
using System.Collections.Generic;
using UnityEngine;

namespace AssemblyCSharp
{
	public class NobelMsgHandle:IMsgHandle
	{
		private List< int > mRequestTypes = new List<int> ();

		public NobelMsgHandle ()
		{

		}

		public void OnInit ()
		{
			EventControl mEventControl = EventControl.instance ();
			mEventControl.addEventHandler (FiEventType.RECV_XL_HORODATA_RESPOSE, RcvHoroDataInfoHandle);
			mEventControl.addEventHandler (FiEventType.RECV_XL_RONGYURANK_RESPOSE, RcvRongYuRankInfoHandle);
			mEventControl.addEventHandler (FiEventType.RECV_XL_PAIWEIPRIZE_RESPOSE, RcvPaiWeiPrizeInfo);
			mEventControl.addEventHandler (FiEventType.RECV_XL_PAIWEIPRIZEINFO_RESPOSE, RcvPaiWeiPrize);
            mEventControl.addEventHandler (FiEventType.RECV_XL_RONGYAOPRIZE_RESQUSET,RcvHonorPrize);
		}

		public void SendGetRankInfoRequest (int nType)
		{
		}

        public void SendGetPaiWeiPrize(int nType = 0)
        {
            //PaiWeiPrizeInfo paiWeiPrizeInfo = new PaiWeiPrizeInfo ();
            //paiWeiPrizeInfo.rewardIndex = nType;
            DataControl.GetInstance().PushSocketSnd(FiEventType.SEND_XL_PAIWEIPRIZEINFO_RESQUSET, null);
        }
        public void SendGetPaiWeiPrizeState(int nType)
        {
            PaiWeiPrizeInfo paiWeiPrizeInfo = new PaiWeiPrizeInfo();
            paiWeiPrizeInfo.rewardIndex = nType;
//            Debug.LogError("FiEventType.SEND_XL_PAIWEIPRIZE_RESQUSET"+FiEventType.SEND_XL_PAIWEIPRIZE_RESQUSET+"nType"+nType);
            DataControl.GetInstance().PushSocketSnd(FiEventType.SEND_XL_PAIWEIPRIZE_RESQUSET, paiWeiPrizeInfo);
        }

		private void RcvRankResponseHandle (object data)
		{
			
		}

		private void RcvHoroDataInfoHandle (object data)
		{
			Debug.Log ("这是荣耀数据接收");
			PaiWeiSaiRankInfos horoinfo = (PaiWeiSaiRankInfos)data;
			Debug.Log ("bossmatchdouble" + horoinfo.bossmatchdouble);
			Debug.Log ("duanwei" + horoinfo.duanwei);
			Debug.Log ("isTopUp" + horoinfo.isTopUp);
			Debug.Log ("monthCardtype" + horoinfo.monthCardtype);
			Debug.Log ("nrank" + horoinfo.nrank);
			Debug.Log ("shenyutime" + horoinfo.shenyutime);
			Debug.Log ("shenyutimenum:::" + horoinfo.rankList.Count);
			Debug.LogError ("horoinfo.lishizuigao" + horoinfo.lishizuigao);
			Debug.LogError ("horoinfo.duanwei" + horoinfo.duanwei);
			Debug.LogError ("horoinfo.Benqizuigao" + horoinfo.beiqizuigao);
			Debug.LogError ("horoinfo.shangqilishipaiming" + horoinfo.shangqipaiming);
            Debug.LogError("horoinfo.qishu当前期数:"+horoinfo.qishu);
			NobelInfo horodatainfo = (NobelInfo)Facade.GetFacade ().data.Get (FacadeConfig.UI_NOBEL_ID);

            horodatainfo.CurrentDuanwei = horoinfo.duanwei;
            horodatainfo.IsToUp = horoinfo.isTopUp;
            horodatainfo.IsMonthType = horoinfo.monthCardtype;
            horodatainfo.Nrankinfo = horoinfo.nrank;
            horodatainfo.ISbossmatchdouble = horoinfo.bossmatchdouble;
            horodatainfo.Showtime = horoinfo.shenyutime;
            horodatainfo.CurrentQi = horoinfo.qishu;
            horodatainfo.Shangqipaiming = horoinfo.shangqipaiming;
            horodatainfo.Lishizuigao = horoinfo.lishizuigao;
            horodatainfo.Beiqizuigao = horoinfo.beiqizuigao;
            horodatainfo.AppendRichData(horoinfo.rankList);

			for (int i = 0; i < horoinfo.rankList.Count; i++) {
				//Debug.LogError ("bigwinarrayinfo" + horoinfo.rankList [i].gold + "username" + horoinfo.rankList [i].nickname + "level" + horoinfo.rankList [i].level + horoinfo.rankList [i].userId);
			}
		}
        GameObject honorWindowObj;

		private void RcvRongYuRankInfoHandle (object data)
		{
			RongYuDianTangRanInfo rongYuInfo = (RongYuDianTangRanInfo)data;
			NobelInfo horodatainfo = (NobelInfo)Facade.GetFacade ().data.Get (FacadeConfig.UI_NOBEL_ID);
			horodatainfo.hotPrizePool = rongYuInfo.hotPrizePool;
			horodatainfo.rongYuRankInfoList = rongYuInfo.rongYuRankList;
			horodatainfo.honorPrizeState = rongYuInfo.result;
			//for (int i = 0; i < rongYuInfo.rongYuRankList.Count - 1; i++) {
			//	int maxIndex = i;
			//	for (int j = i + 1; j < rongYuInfo.rongYuRankList.Count; j++) {
			//		if (rongYuInfo.rongYuRankList [maxIndex].xingxing < rongYuInfo.rongYuRankList [j].xingxing) {
			//			maxIndex = j;
			//		}
			//	}
			//	if (maxIndex != i) {
			//		FiRankInfo tempInfo = rongYuInfo.rongYuRankList [i];
			//		rongYuInfo.rongYuRankList [i] = rongYuInfo.rongYuRankList [maxIndex];
			//		rongYuInfo.rongYuRankList [maxIndex] = tempInfo;
			//	}
			//}
        
            if (honorWindowObj==null)
            {
                string path = "Window/HallOfHonor";
                honorWindowObj = AppControl.OpenWindow(path);
                honorWindowObj.gameObject.SetActive(true);
            }
            //else
            //{
            //    honorWindowObj.GetComponent<Canvas>().sortingLayerID = 12;
            //}
          
			Debug.LogError ("horodatainfo.rongYuRankInfoList " + horodatainfo.rongYuRankInfoList.Count + "horodatainfo.hotPrizePool" + horodatainfo.hotPrizePool + "rongYuInfo.result" + rongYuInfo.result);
		}

		private void RcvPaiWeiPrizeInfo (object data)
		{
			//发送106 10104
			//        string path = "Window/MyDuanWindow";
			//         GameObject windowClone = AppControl.OpenWindow(path);
			//         windowClone.gameObject.SetActive(true);
			PaiWeiPrizeInfo paiWeiPrizeInfo = (PaiWeiPrizeInfo)data;
			Debug.LogError ("paiWeiPrizeInfo.rewardState" + paiWeiPrizeInfo.rewardState);
			Debug.LogError ("paiWeiPrizeInfo.rewardData.Count" + paiWeiPrizeInfo.rewardData.Count);
            Debug.LogError("paiWeiPrizeInfo.rewardIndex"+paiWeiPrizeInfo.rewardIndex);
			if (paiWeiPrizeInfo.rewardState == 0) {
				UnityEngine.GameObject Window = UnityEngine.Resources.Load ("Window/NewsHoroRewardWindow") as UnityEngine.GameObject;
				GameObject WindowClone = UnityEngine.GameObject.Instantiate (Window);
				UIHoroReward reward = WindowClone.GetComponent<UIHoroReward> ();
				reward.SetRewardData (paiWeiPrizeInfo.rewardData);

			} else {
//				Debug.LogError ("paiWeiPrizeInfo.rewardState" + paiWeiPrizeInfo.rewardState); 
			}
     
		}

		private void RcvPaiWeiPrize (object data)
		{
//            Debug.LogError("接收消息recvpaiweiprize");
			
//            
			//发送105 10103
			PaiWeiPrizeState paiWeiPrizeState = (PaiWeiPrizeState)data;
			NobelInfo horodatainfo = (NobelInfo)Facade.GetFacade ().data.Get (FacadeConfig.UI_NOBEL_ID);
			horodatainfo.prizeState = paiWeiPrizeState.rewardList;
            string path = "Window/MyDuanWindow";
            GameObject windowClone = AppControl.OpenWindow(path);
            windowClone.gameObject.SetActive(true);

//			Debug.LogError ("paiWeiPrizeState.rewardList" + paiWeiPrizeState.rewardList.Count);

           
			for (int i = 0; i < paiWeiPrizeState.rewardList.Count; i++) {
				Debug.LogError ("paiWeiPrizeState.rewardList" + paiWeiPrizeState.rewardList [i]);
			}
		}
        private void RcvHonorPrize(object data)
        {
            Debug.LogError("荣耀奖励领取");
            PaiWeiPrizeInfo paiWeiPrizeInfo = (PaiWeiPrizeInfo)data;
            Debug.LogError("paiWeiPrizeInfo.rewardState" + paiWeiPrizeInfo.rewardState);
            Debug.LogError("paiWeiPrizeInfo.rewardData.Count" + paiWeiPrizeInfo.rewardData.Count);
            Debug.LogError("paiWeiPrizeInfo.rewardIndex" + paiWeiPrizeInfo.rewardIndex);
            Debug.LogError("paiWeiPrizeInfo.curCatchFishNum" + paiWeiPrizeInfo.curCatchFishNum);
            Debug.LogError("paiWeiPrizeInfo.maxCatchFishNum" + paiWeiPrizeInfo.maxCatchFishNum);
            if (paiWeiPrizeInfo.rewardState == 0)
            {
                UnityEngine.GameObject Window = UnityEngine.Resources.Load("Window/HoroRewardLevelTypeShow") as UnityEngine.GameObject;
                GameObject WindowClone = UnityEngine.GameObject.Instantiate(Window);
                UIReward reward = WindowClone.GetComponent<UIReward>();
                reward.SetRewardData(paiWeiPrizeInfo.rewardData);

            }
            else if (paiWeiPrizeInfo.rewardState == -200)
            {
                //等待奖励状态
                UnityEngine.GameObject Window = UnityEngine.Resources.Load("Window/GetHonorPrizeTips") as UnityEngine.GameObject;
                Window.GetComponent<GetHonorPrizeTips>().targetNum =paiWeiPrizeInfo.maxCatchFishNum;
                Window.GetComponent<GetHonorPrizeTips>().currentNum =paiWeiPrizeInfo.curCatchFishNum;
                GameObject WindowClone = UnityEngine.GameObject.Instantiate(Window);

                //Debug.LogError("paiWeiPrizeInfo.rewardState" + paiWeiPrizeInfo.rewardState);
            } 
        }

		public void OnDestroy ()
		{
			EventControl mEventControl = EventControl.instance ();
			mEventControl.removeEventHandler (FiEventType.RECV_XL_HORODATA_RESPOSE, RcvHoroDataInfoHandle);
			mEventControl.removeEventHandler (FiEventType.RECV_XL_RONGYURANK_RESPOSE, RcvRongYuRankInfoHandle);
			mEventControl.removeEventHandler (FiEventType.RECV_XL_PAIWEIPRIZE_RESPOSE, RcvPaiWeiPrizeInfo);
			mEventControl.removeEventHandler (FiEventType.RECV_XL_PAIWEIPRIZEINFO_RESPOSE, RcvPaiWeiPrize);
            mEventControl.removeEventHandler (FiEventType.RECV_XL_RONGYAOPRIZE_RESQUSET, RcvHonorPrize);

		}

	}
}


