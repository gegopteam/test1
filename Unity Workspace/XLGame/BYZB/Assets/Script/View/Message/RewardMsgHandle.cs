using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AssemblyCSharp
{
	public class RewardMsgHandle : IMsgHandle
	{
		//奖励配置信息，如：签到相关的奖励信息，用于显示
		public const int SIGN_IN_REWARD_TYPE = 1;
		//签到奖励
		public const int YK_CONVERT_FORMAL_REWARD_TYPE = 2000;
		//转正奖励
		public const int BENEFITS_REWARD_TYPE = 1000;
		//救济金奖励
		public const int BIND_WX_GONGZHONG_REWARD_TYPE = 4000;
		//修改昵称扣除 额度
		public const int MODIFY_NICK_TYPE = 3000;
	
	

		List<FiRewardStructure> m_rewardInfo = null;



		public RewardMsgHandle ()
		{
		}

		public void OnInit ()
		{
			EventControl nControl = EventControl.instance ();
			nControl.addEventHandler (FiEventType.RECV_XL_GET_REWARD_INFO, RcvRewardResponse);
		}

		public void OnDestroy ()
		{
			EventControl nControl = EventControl.instance ();
			nControl.removeEventHandler (FiEventType.RECV_XL_GET_REWARD_INFO, RcvRewardResponse);
		}

		private void RcvRewardResponse (object data)
		{
			Debug.Log ("--- RewardMsgHandle RcvRewardResponse ---");
			FiRewardAllData info = (FiRewardAllData)data;
			if (null != info.rewardAll) {
				m_rewardInfo = info.rewardAll;
				List<FiRewardStructure> listReward = info.rewardAll;
				for (int i = 0; i < listReward.Count; i++) {
					FiRewardStructure rewardInfo = listReward [i];
//					Debug.Log ("rewardInfo i:" + i + " RewardType:" + rewardInfo.RewardType + " TaskID:" + rewardInfo.TaskID + " TaskValue:" + rewardInfo.TaskValue);
					List<FiProperty> listProperty = rewardInfo.rewardPro;
					for (int j = 0; j < listProperty.Count; j++) {
						//Debug.Log ("listReward.Count = " + listReward.Count);
						FiProperty propertyInfo = listProperty [j];
//						//Debug.Log ("propertyInfo i:" + i + " j:" + j + " type:" + propertyInfo.type + " value:" + propertyInfo.value);
//						DoRewardInfo (propertyInfo.type, propertyInfo.value);
					}

				}

			}
		}

		//这是荣耀总奖励额
		public List<FiRewardStructure> GetHoroRewardInfoArray ()
		{
			List<FiRewardStructure> listInfos = new List<FiRewardStructure> ();
			List<FiRewardStructure> listReward = m_rewardInfo;
			if (null != listReward) {
				for (int i = 0; i < listReward.Count; i++) {
					FiRewardStructure rewardInfo = listReward [i];
					if (rewardInfo.RewardType >= 6001 && rewardInfo.RewardType <= 6025) {
						listInfos.Add (rewardInfo);
					}
				}
			}
			return listInfos;
		}

		//这是荣耀段位励额
		public List<FiRewardStructure> GetHoroduanweiArray ()
		{
			List<FiRewardStructure> listInfo = new List<FiRewardStructure> ();
			List<FiRewardStructure> listReward = m_rewardInfo;
			if (null != listReward) {
				for (int i = 0; i < listReward.Count; i++) {
					FiRewardStructure rewardInfo = listReward [i];
					if (rewardInfo.RewardType >= 7001 && rewardInfo.RewardType < 8000) {
						listInfo.Add (rewardInfo);
					}
				}
			}
			return listInfo;
		}
		//这是荣耀励额
		public List<FiRewardStructure> GetHoroArray ()
		{
			List<FiRewardStructure> listInfo = new List<FiRewardStructure> ();
			List<FiRewardStructure> listReward = m_rewardInfo;
			if (null != listReward) {
				for (int i = 0; i < listReward.Count; i++) {
					FiRewardStructure rewardInfo = listReward [i];
					if (rewardInfo.RewardType == 8000) {
						listInfo.Add (rewardInfo);
					}
				}
			}
			return listInfo;
		}


		public List<FiRewardStructure> GetRewardInfo (int type)
		{
			
			List<FiRewardStructure> listInfo = new List<FiRewardStructure> ();

			List<FiRewardStructure> listReward = m_rewardInfo;
			if (null != listReward) {
				for (int i = 0; i < listReward.Count; i++) {
					FiRewardStructure rewardInfo = listReward [i];
					if (type == rewardInfo.RewardType) {
						listInfo.Add (rewardInfo);
					}
				}
			}
			return listInfo;
		}

	}
}


