/* author:KinSen
 * Date:2017.07.25
 */

using UnityEngine;
using System.Collections;
using System.Collections.Generic;

using AssemblyCSharp;

//FishingPK规则：
//FishingPKTime，可匹配，可创建，人数>2人
//FishingPKBullet，可匹配，不可创建，人数=4人
//FishingPKPoint，可匹配，不可创建，人数=2人

//负责：FishingPK通用的消息接收处理
public class RcvFishingMsgPK : RcvFishingMsg
{
	public RcvFishingMsgPK ()
	{

	}

	~RcvFishingMsgPK ()
	{

	}

	public void RcvPKStartGameInform (object data)
	{
		FiStartPKGameInform startGame = (FiStartPKGameInform)data;
		Tool.LogError ("RcvPKStartGameInform 111");
		fishingObjects.InitCannonInfo ();
	}

	public void RcvPKPreGameCountdownInform (object data)
	{//游戏开始前的倒计时
		FiPkGameCountDownInform countdown = (FiPkGameCountDownInform)data;
		fishingObjects.ToPKPreGameCountdown (countdown);
		return;
	}

	public void RcvPKGameCountdownInform (object data)
	{//游戏倒计时
		FiPkGameCountDownInform countdown = (FiPkGameCountDownInform)data;
		fishingObjects.ToPKGameCountdown (countdown);
		return;
	}

	public void RcvPKUseEffectResponse (object data)
	{
		FiEffectResponse effectInfo = (FiEffectResponse)data;
		FiEffectInfo info = effectInfo.info; 
		if (null == info)
			return;

		if (0 == effectInfo.result) {
			fishingObjects.ToEffect (info);

		} else {
			if (effectInfo.result == 40004) {
				HintTextPanel._instance.SetTextShow("魚箱中的魚已經滿了，請稍後使用召喚", 2f);
			} else {
				//HintText._instance.ShowHint("道具使用失败，道具编号:" + info.type + " 错误结果:" + effectInfo.result);
			}
		

		}
	}

	public void RcvPKOtherEffectInform (object data)
	{
		FiOtherEffectInform otherEffectInfo = (FiOtherEffectInform)data;
		//otherEffectInfo.userId
		FiEffectInfo info = otherEffectInfo.info;
		if (null == info)
			return;

		fishingObjects.ToEffect (info, otherEffectInfo.userId);
	}

	public void RcvPKDistributePropertyInform (object data)
	{//收到PK场分配技能统计广播
		FiDistributePKProperty distribute = (FiDistributePKProperty)data;
		fishingObjects.SetPKDistributePropertyInfo (distribute);
		return;
	}

	public void SndPKLaunchTorpedoRequest (int torpedoID, int torpedoType, int x, int y)
	{//发送鱼雷请求
		FiLaunchTorpedoRequest launchTorpedo = new FiLaunchTorpedoRequest ();
		launchTorpedo.torpedoId = torpedoID;
		launchTorpedo.torpedoType = torpedoType;
		launchTorpedo.position = new Cordinate ();
		launchTorpedo.position.x = x;
		launchTorpedo.position.y = y;
		dataControl.PushSocketSnd (FiEventType.SEND_PK_LAUNCH_TORPEDO_REQUEST, launchTorpedo);
	}

	public void RcvPKLaunchTorpedoResponse (object data)
	{//发送PK场鱼雷回复
		FiLaunchTorpedoResponse launchTorpedo = (FiLaunchTorpedoResponse)data;
		fishingObjects.ToPKLaunchTorpedo (launchTorpedo);
		return;
	}

	public void RcvPKOtherLaunchTorpedoInform (object data)
	{//其他PK场玩家发送鱼雷通知
		FiOtherLaunchTorpedoInform otherLaunchTorpedo = (FiOtherLaunchTorpedoInform)data;
		fishingObjects.ToPKOtherLaunchTorpedo (otherLaunchTorpedo);
		return;
	}

	public void SndPKTorpedoExplodeRequest (int torpedoID, int torpedoType, List<FiFish> fishs)
	{//发送PK场鱼雷爆炸请求
		FiTorpedoExplodeRequest torpedoExplode = new FiTorpedoExplodeRequest ();
		torpedoExplode.torpedoId = torpedoID;
		torpedoExplode.torpedoType = torpedoType;
		torpedoExplode.fishes = new List<FiFish> ();
		foreach (FiFish fish in fishs) {
			if (null != fish) {
				torpedoExplode.fishes.Add (fish);
			}
		}

		dataControl.PushSocketSnd (FiEventType.SEND_PK_TORPEDO_EXPLODE_REQUEST, torpedoExplode);
	}

	public void RcvPKTorpedoExplodeResponse (object data)
	{//接收鱼雷爆炸回复
		FiTorpedoExplodeResponse torpedoExplode = (FiTorpedoExplodeResponse)data;
		fishingObjects.ToPKTorpedoExplode (torpedoExplode);
		return;
	}

	public void RcvPKOtherTorpedoExplodeInform (object data)
	{//接收其他玩家鱼雷爆炸通知
		FiOtherTorpedoExplodeInform otherTorpedoExplode = (FiOtherTorpedoExplodeInform)data;
		fishingObjects.ToPKOtherTorpedoExplode (otherTorpedoExplode);
		return;
	}


	public void RcvPKLeaveRoom (object data)
	{
		
	}


	public void RcvPKOtherLeaveRoom (object data)
	{
		FiOtherLeavePKRoomInform nInform = (FiOtherLeavePKRoomInform)data;
		if (GameController._instance != null) {
			Debug.LogError ("User leave: inGame=true,id=" + nInform.leaveUserId);
			UIFishingObjects.GetInstance ().cannonManage.GetInfo (nInform.leaveUserId).cannon.gunUI.SetLeaveTextShow (true);
		} else {
			Debug.LogError ("User leave: inGame=false,id=" + nInform.leaveUserId);
		}
	}


}
