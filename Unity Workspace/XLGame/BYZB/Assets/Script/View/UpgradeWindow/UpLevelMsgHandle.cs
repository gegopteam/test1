using System;
using AssemblyCSharp;
using System.Collections.Generic;
using UnityEngine;

namespace AssemblyCSharp
{
	public class UpLevelMsgHandle : IMsgHandle
	{
		private List<UpLevelTaskInfos> mLevelInfo = new List<UpLevelTaskInfos>();
		private MyInfo userInfo;

		public UpLevelMsgHandle()
		{

		}

		public void OnInit()
		{
			userInfo = (MyInfo)Facade.GetFacade().data.Get(FacadeConfig.USERINFO_MODULE_ID);
			EventControl mEventControl = EventControl.instance();
			mEventControl.addEventHandler(FiProtoType.XL_GET_UP_LEVEL_ACTIVY_INFO, RcvUpLevelInfo);
			mEventControl.addEventHandler(FiProtoType.XL_GET_UP_LEVEL_BAG_STATE, RecvUpLevelBagResponseHandle);
		}

		public void SendNewUpLevelMessage(int index)
		{
			UpLevelRewards nRequest = new UpLevelRewards();
			nRequest.taskID = index;
			Debug.LogError("nsenindex" + nRequest.taskID + "nRequest.UserDay" + nRequest.taskID);
			DataControl.GetInstance().PushSocketSnd(FiEventType.SEND_XL_LEVELUP_GET_NEW_RESPOSE, nRequest);
		}

		public void RcvUpLevelInfo(object data)
		{
			UpLevelInfo upLevelInfo = (UpLevelInfo)Facade.GetFacade().data.Get(FacadeConfig.UI_UPLEVEL);
			DbGetUpLevelActivityInfos LevelInfoS = (DbGetUpLevelActivityInfos)data;
			bool level10 = false;
			bool level15 = false;
			bool level20 = false;

			bool levelget10 = false;
			bool levelget15 = false;
			bool levelget20 = false;
			if (LevelInfoS.levelList.Count > 0)
			{
				for (int levelActivity = 0; levelActivity < LevelInfoS.levelList.Count; levelActivity++)
				{
					UpLevelTaskInfos levelInfo = (UpLevelTaskInfos)LevelInfoS.levelList[levelActivity];
					switch (levelInfo.taskID) {
						case 1:
							if (levelInfo.rewardState == 0){
								UILevelUpgradeControl.levelRewardState[levelInfo.taskID - 1] = levelInfo.rewardState;
							}
							else if (levelInfo.rewardState == 1) {
								levelget10 = true;
								UILevelUpgradeControl.levelRewardState[levelInfo.taskID - 1] = levelInfo.rewardState;
							}
							else if (levelInfo.rewardState == 3){
								level10 = true;
								UILevelUpgradeControl.levelRewardState[levelInfo.taskID - 1] = levelInfo.rewardState;
							}
							break;

						case 2:
                            if (levelInfo.rewardState == 0){
								UILevelUpgradeControl.levelRewardState[levelInfo.taskID - 1] = levelInfo.rewardState;
							}
							else if (levelInfo.rewardState == 1) {
								levelget15 = true;
								UILevelUpgradeControl.levelRewardState[levelInfo.taskID - 1] = levelInfo.rewardState;
							}
							else if (levelInfo.rewardState == 3){
								level15 = true;
								UILevelUpgradeControl.levelRewardState[levelInfo.taskID - 1] = levelInfo.rewardState;
							}
							break;

						case 3:
							if (levelInfo.rewardState == 0)
							{
								UILevelUpgradeControl.levelRewardState[levelInfo.taskID - 1] = levelInfo.rewardState;
							}
							else if (levelInfo.rewardState == 1)
							{
								levelget20 = true;
								UILevelUpgradeControl.levelRewardState[levelInfo.taskID - 1] = levelInfo.rewardState;
							}
							else if (levelInfo.rewardState == 3)
							{
								level20 = true;
								UILevelUpgradeControl.levelRewardState[levelInfo.taskID - 1] = levelInfo.rewardState;
							}
							break;
					}
				}

                //判斷升級獎勵是不是領完了
				if (level10 && level15 && level20)
				{
					if (UIHallCore.isShowLevelUpgrade)
                    {
						userInfo.isUserGetAllUpLevel = true;
						UIHallCore.isNeedToUpdate = true;
						//UIHallCore.isShowLevelUpgrade = false;
						Debug.Log("UIHallCore.isNeedToUpdate = " + UIHallCore.isNeedToUpdate);
					}
				}
				else
				{
					userInfo.isUserGetAllUpLevel = false;
					UIHallCore.isNeedToUpdate = false;
				}

				//判斷升級獎勵有沒有可以領還沒領
				if (levelget10 || levelget15 || levelget20)
				{
					userInfo.isLevelupCanGet = true;
				}
				else
				{
					userInfo.isLevelupCanGet = false;
				}


				Debug.Log("UpLevelMsgHandle @ RcvUpLevelInfo");
				IUiMediator nMediator = Facade.GetFacade().ui.Get(FacadeConfig.UI_UPLEVEL);
				if (nMediator != null)
				{
					nMediator.OnRecvData(1000, data);
				}
			}
			else
            {
				IUiMediator nMediator = Facade.GetFacade().ui.Get(FacadeConfig.UI_UPLEVEL);
				//Debug.Log("RecvUpLevelBagResponseHandle @ nMediator = " + nMediator);
				if (nMediator != null)
				{
					nMediator.OnRecvData(1001, data);
				}
			}

			

			//PaiWeiPrizeInfo paiWeiPrizeInfo = (PaiWeiPrizeInfo)data;
			//Debug.LogError("paiWeiPrizeInfo.rewardState" + paiWeiPrizeInfo.rewardState);
			//Debug.LogError("paiWeiPrizeInfo.rewardData.Count" + paiWeiPrizeInfo.rewardData.Count);
			//Debug.LogError("paiWeiPrizeInfo.rewardIndex" + paiWeiPrizeInfo.rewardIndex);
			//if (paiWeiPrizeInfo.rewardState == 0)
			//{
			//	UnityEngine.GameObject Window = UnityEngine.Resources.Load("Window/NewsHoroRewardWindow") as UnityEngine.GameObject;
			//	GameObject WindowClone = UnityEngine.GameObject.Instantiate(Window);
			//	UIHoroReward reward = WindowClone.GetComponent<UIHoroReward>();
			//	reward.SetRewardData(paiWeiPrizeInfo.rewardData);

			//}

		}

		public void SetLevelUpInfoData(List<UpLevelTaskInfos> nArray)
		{
			mLevelInfo = nArray;
		}

		//新手升級禮包初始话协议
		public void RecvUpLevelBagResponseHandle(object data)
		{
			Debug.Log("RecvUpLevelBagResponseHandle @ UpLevelMsgHandle");
			//UpLevelReward upLevelReward = (UpLevelReward)Facade.GetFacade().data.Get(FacadeConfig.UI_UPLEVEL_REWARD);
			//UpLevelRewardGets uplevelifno = (UpLevelRewardGets)data;
			//upLevelReward.result = uplevelifno.result;
			//upLevelReward.taskID = uplevelifno.taskID;
			//upLevelReward.taskLevel = uplevelifno.taskLevel;
			//upLevelReward.gold = uplevelifno.gold;

			//Debug.Log("RecvUpLevelBagResponseHandle @ result = " + nUpLevelinfo.result);
			//Debug.Log("RecvUpLevelBagResponseHandle @ taskID = " + nUpLevelinfo.taskID);
			//Debug.Log("RecvUpLevelBagResponseHandle @ taskLevel = " + nUpLevelinfo.taskLevel);
			//Debug.Log("RecvUpLevelBagResponseHandle @ gold = " + nUpLevelinfo.gold);

			IUiMediator nMediator = Facade.GetFacade().ui.Get(FacadeConfig.UI_UPLEVEL);
			//Debug.Log("RecvUpLevelBagResponseHandle @ nMediator = " + nMediator);
			if (nMediator != null)
			{
				nMediator.OnRecvData(1001, data);
			}
		}

		public void OnDestroy()
		{
			EventControl mEventControl = EventControl.instance();
			mEventControl.removeEventHandler(FiProtoType.XL_GET_UP_LEVEL_ACTIVY_INFO, RcvUpLevelInfo);
			mEventControl.removeEventHandler(FiProtoType.XL_GET_UP_LEVEL_BAG_STATE, RecvUpLevelBagResponseHandle);

		}

	}
}




