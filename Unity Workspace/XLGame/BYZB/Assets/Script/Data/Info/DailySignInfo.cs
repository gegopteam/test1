using System;
using System.Collections.Generic;
using UnityEngine;

namespace AssemblyCSharp
{
	//每日签到数据
	public class DailySignInfo:IDataProxy
	{
		private List<FiRewardStructure> rewardinfo;

		private List<SignCofing> signList = new List<SignCofing> ();

		private bool updated = false;

		private bool issign = false;
		private bool istowwor = false;
		private bool isalms = false;

		public bool IsUpdated ()
		{
			updated = issign | istowwor | isalms;
			return updated;
		}

		public void Setissign (bool nValue)
		{
			issign = nValue;
		}

		public void Setistowwor (bool nValue)
		{
			istowwor = nValue;
		}

		public void Setisalms (bool nValue)
		{
			isalms = nValue;
		}

		public void SetUpdate (bool nValue)
		{
			updated = nValue;
		}

		public DailySignInfo ()
		{
//			rewardinfo  = Facade.GetFacade ().message.reward.GetRewardInfo (RewardMsgHandle.SIGN_IN_REWARD_TYPE);
//			Debug.LogError ("sss" + rewardinfo.Count);
//			for (int i = 0; i < rewardinfo.Count; i++) {
//				Debug.LogError ("----RewardType------" + rewardinfo [i].RewardType + "----rewardinfo[i].TaskID------" + rewardinfo [i].TaskID
//					+ "---rewardinfo[i].TaskValue----" + rewardinfo [i].TaskValue + "----rewardinfo[i].rewardPro.Count---------" + rewardinfo [i].rewardPro.Count);
//				for (int j = 0; j < rewardinfo [i].rewardPro.Count; j++) {
//					Debug.LogError ("rewardinfo [i].rewardPro" + rewardinfo [i].rewardPro [j].type +" rewardinfo [i].rewardPro [j].value" +rewardinfo [i].rewardPro [j].value);
//				}
//			}
		}

		public void OnAddData (int nType, object nData)
		{

		}

		public void OnInit ()
		{
//			rewardinfo  = Facade.GetFacade ().message.reward.GetRewardInfo (RewardMsgHandle.SIGN_IN_REWARD_TYPE);
//			Debug.LogError ("sss" + rewardinfo.Count);
//			for (int i = 0; i < rewardinfo.Count; i++) {
//				Debug.LogError ("----RewardType------" + rewardinfo [i].RewardType + "----rewardinfo[i].TaskID------" + rewardinfo [i].TaskID
//					+ "---rewardinfo[i].TaskValue----" + rewardinfo [i].TaskValue + "----rewardinfo[i].rewardPro.Count---------" + rewardinfo [i].rewardPro.Count);
//				for (int j = 0; j < rewardinfo [i].rewardPro.Count; j++) {
//					Debug.LogError ("rewardinfo [i].rewardPro" + rewardinfo [i].rewardPro [j].type +" rewardinfo [i].rewardPro [j].value" +rewardinfo [i].rewardPro [j].value);
//				}
//			}
		}
		//		public List<SignCofing> GetSignDayArrayInfo()
		//		{
		//			return null;
		//		}
			
		public void OnDestroy ()
		{
			
		}

	}
}

