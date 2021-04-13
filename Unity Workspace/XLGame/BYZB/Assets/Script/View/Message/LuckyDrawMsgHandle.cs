using System;
using DG.Tweening;

namespace AssemblyCSharp
{
	public class LuckyDrawMsgHandle:IMsgHandle
	{
		public LuckyDrawMsgHandle ()
		{
		}

		public void SendLuckyDrawRequest (int nType)
		{
			FiGetFishLuckyDrawRequest nRequest = new FiGetFishLuckyDrawRequest ();
			nRequest.type = nType;

			DataControl.GetInstance ().PushSocketSndByte (FiEventType.SEND_FISH_LUCKY_DRAW_REQUEST, nRequest.serialize ());
		}
		//其他玩家抽奖广播的通知
		void RecvOtherLuckyDrawInform (object data)
		{
			FiFishLuckyDrawInform nInform = (FiFishLuckyDrawInform)data;
			GetLuckyDrawReward (nInform.property.type, nInform.property.value, nInform.userId);
		}

		//自己抽奖反馈
		void RecvGetLuckyDrawResponse (object data)
		{
			FiGetFishLuckyDrawResponse nResponse = (FiGetFishLuckyDrawResponse)data;

			UnityEngine.Debug.LogError ("nResponse.result ====== sadasdas" + nResponse.result);


			//抽奖返回值为0,就让他执行一下操作
			if (nResponse.result == 0) {

				LotteryRandomScript.Instance.colseLotBtn.interactable = false;

				LotteryRandomScript.Instance.WaitRecvResponse (LotteryRandomScript.Instance.activeCardId);
				LuckDrawHintBoard._instace.effect_UI_Lottery.SetActive (false);

				if (LuckDrawCanvasScr.Instance.isCommonToggle == true) {
					ToggleChooseMsgHandle (nResponse.property.value);
				}
				if (LuckDrawCanvasScr.Instance.isBronzeToggle == true) {
					ToggleChooseMsgHandle (nResponse.property.value);	
				}
				if (LuckDrawCanvasScr.Instance.isSilverToggle == true) {
					ToggleChooseMsgHandle (nResponse.property.value);
				}
				if (LuckDrawCanvasScr.Instance.isGoldenToggle == true) {
					ToggleChooseMsgHandle (nResponse.property.value);
				}
				if (LuckDrawCanvasScr.Instance.isPlatinumToggle == true) {
					ToggleChooseMsgHandle (nResponse.property.value);
				}
				if (LuckDrawCanvasScr.Instance.isExtremeToggle == true) {
					ToggleChooseMsgHandle (nResponse.property.value);
				}


				//减少鱼
				LuckDrawCanvasScr.Instance.MinusLotteryFish ();
				//减少金币
				LuckDrawCanvasScr.Instance.MinusLotteryGold ();
				//根据种类和数量添加奖励

				GetLuckyDrawReward (nResponse.property.type, nResponse.property.value, DataControl.GetInstance ().GetMyInfo ().userID); //根据种类和数量添加奖励


				LuckDrawHintBoard._instace.UpdateData ();
				//自动关闭
				LotteryRandomScript.Instance.ThreeSecondsCloseLottery ();
			}
		}

		void GetLuckyDrawReward (int rewardServerType, int rewardNum, int userId)//如果id是自己的，可以加金币钻石和鱼雷，如果是别人的，只能加金币钻石
		{
			GunControl tempGun = PrefabManager._instance.GetGunByUserID (userId);
			if (tempGun == null) {
				UnityEngine.Debug.LogError ("Error! Can't find gun by userId:" + userId);
			}
			switch (rewardServerType) {
			case FiPropertyType.DIAMOND:
				tempGun.gunUI.AddValue (0, 0, rewardNum);
				break;
			case FiPropertyType .GOLD:
				tempGun.gunUI.AddValue (0, rewardNum, 0);
				break;

			default:
				if (userId == DataControl.GetInstance ().GetMyInfo ().userID) {
					if (rewardServerType >= FiPropertyType.TORPEDO_MINI && rewardServerType <= FiPropertyType.TORPEDO_NUCLEAR) {
						Skill torpedoSkill = PrefabManager._instance.GetSkillUIByServerId (rewardServerType);
						if (torpedoSkill == null) {
							UnityEngine.Debug.LogError ("Error! Skill=null:" + rewardServerType);
						} else {
							torpedoSkill.AddRestNum (rewardNum);
						}
					}

				}
				break;
			}
		}

		void ToggleChooseMsgHandle (int va)
		{
			LotteryRandomScript.Instance.LotteryFeedback (LotteryRandomScript.Instance.activeCardId, va);
		}

		public void OnInit ()
		{
			EventControl nControl = EventControl.instance ();
			nControl.addEventHandler (FiEventType.RECV_NOTIFY_FISH_LUCKY_DRAW, RecvOtherLuckyDrawInform);
			nControl.addEventHandler (FiEventType.RECV_FISH_LUCKY_DRAW_RESPONSE, RecvGetLuckyDrawResponse);
		}

		public void OnDestroy ()
		{
			EventControl nControl = EventControl.instance ();
			nControl.removeEventHandler (FiEventType.RECV_NOTIFY_FISH_LUCKY_DRAW, RecvOtherLuckyDrawInform);
			nControl.removeEventHandler (FiEventType.RECV_FISH_LUCKY_DRAW_RESPONSE, RecvGetLuckyDrawResponse);

		}
	}
}

