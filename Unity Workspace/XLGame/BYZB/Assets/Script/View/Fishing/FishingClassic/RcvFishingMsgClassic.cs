/* author:KinSen
 * Date:2017.03.21
 */

using UnityEngine;
using System.Collections;
using System.Collections.Generic;

using AssemblyCSharp;

public class RcvFishingMsgClassic : RcvFishingMsg
{
	public RcvFishingMsgClassic ()
	{

	}

	~RcvFishingMsgClassic ()
	{

	}

	public void RcvRoomMatchResponse (object data)
	{//进捕鱼房间

		//MyInfo nInfo =	(MyInfo)Facade.GetFacade ().data.Get ( FacadeConfig.USERINFO_MODULE_ID );
		//nInfo.lockScene = false;
		FiRoomMatchResponse roomMatchReply = (FiRoomMatchResponse)data;
        Debug.LogError("roomMatchReply.result = " + roomMatchReply.result);
        if (0 == roomMatchReply.result) {//房间匹配成功
			fishingObjects.EnterClassicRoom (roomMatchReply.gold, roomMatchReply.roomIndex, roomMatchReply.seatIndex, roomMatchReply.userArray);
			//roomInfo.roomIndex = roomMatchReply.roomIndex;
			/*	Debug.Log ("我的座位号："+roomMatchReply.seatIndex);
			MyInfo nInfo =(MyInfo) Facade.GetFacade ().data.Get ( FacadeConfig.USERINFO_MODULE_ID );
			nInfo.gold = roomMatchReply.gold;

			myInfo.seatIndex = roomMatchReply.seatIndex;
			roomInfo.InitUser (roomMatchReply.userArray);
			UIFishingMsg.GetInstance ().SetFishing (TypeFishing.CLASSIC);
			myInfo.lastGame.type = TypeFishing.CLASSIC;
			//myInfo.SetState( MyInfo.STATE_IN_CLASSICROOM );
			fishingObjects.InitCannonInfo ();

			myInfo.TargetView = AppView.FISHING;
			AppControl.ToView (AppView.LOADING);*/
		}
		else if (2 == roomMatchReply.result)
		{
			GameObject Window1 = UnityEngine.Resources.Load("Window/WindowTipsThree") as UnityEngine.GameObject;
			GameObject WindowClone1 = UnityEngine.GameObject.Instantiate(Window1);
			UITipAutoNoMask ClickTips1 = WindowClone1.GetComponent<UITipAutoNoMask>();
			ClickTips1.tipText.text = "當前炮倍數小於50倍，錯誤碼 " + roomMatchReply.result;
		}
		else if (3 == roomMatchReply.result)
		{//非零房間匹配失敗
		 //先提示房間匹配失敗
			Tool.OutLogWithToFile("Fishing 房間匹配失敗");
			//myInfo.SetState( MyInfo.STATE_IN_HALL );
			GameObject Window1 = UnityEngine.Resources.Load("Window/WindowTipsThree") as UnityEngine.GameObject;
			GameObject WindowClone1 = UnityEngine.GameObject.Instantiate(Window1);
			UITipAutoNoMask ClickTips1 = WindowClone1.GetComponent<UITipAutoNoMask>();
			ClickTips1.tipText.text = "當前炮倍數小於1000倍，錯誤碼 " + roomMatchReply.result;
			//然後跳轉到大廳界面
			//AppControl.ToView (AppView.HALL);
			//return;
		}
		else if (4 == roomMatchReply.result)
		{
			GameObject Window1 = UnityEngine.Resources.Load("Window/WindowTipsThree") as UnityEngine.GameObject;
			GameObject WindowClone1 = UnityEngine.GameObject.Instantiate(Window1);
			UITipAutoNoMask ClickTips1 = WindowClone1.GetComponent<UITipAutoNoMask>();
			ClickTips1.tipText.text = "獵殺賽已經結束";
		} else if (5 == roomMatchReply.result) {
			//boss场限定
			GameObject Window1 = UnityEngine.Resources.Load ("Window/WindowTipsFive")as UnityEngine.GameObject;
			GameObject WindowClone1 = UnityEngine.GameObject.Instantiate (Window1);
		}
		LoginUtil.GetIntance ().ShowWaitingView (false);
	}

	public void SndLaunchTorpedoRequest (int torpedoId, int torpedoType, int x, int y)
	{//发送鱼雷请求
		FiLaunchTorpedoRequest launchTorpedo = new FiLaunchTorpedoRequest ();
		launchTorpedo.position = new Cordinate ();
		launchTorpedo.torpedoId = torpedoId;
		launchTorpedo.torpedoType = torpedoType;
		launchTorpedo.position.x = x;
		launchTorpedo.position.y = y;
		dataControl.PushSocketSnd (FiEventType.SEND_LAUNCH_TORPEDO_REQUEST, launchTorpedo);

	}

	public void RcvLaunchTorpedoResponse (object data)
	{//接收发送的鱼雷回复
		FiLaunchTorpedoResponse launchTorpedo = (FiLaunchTorpedoResponse)data;
		fishingObjects.ToLaunchTorpedo (launchTorpedo);
		if (launchTorpedo.result == 0) {
			BackpackInfo backpackInfo = (BackpackInfo)Facade.GetFacade ().data.Get (FacadeConfig.BACKPACK_MODULE_ID);
			backpackInfo.Delete (launchTorpedo.torpedoType, 1);
		}
	}

	public void RcvOtherLaunchTorpedoInform (object data)
	{//接收其他玩家发送的鱼雷
		FiOtherLaunchTorpedoInform otherLaunchTorpedo = (FiOtherLaunchTorpedoInform)data;
		fishingObjects.ToOtherLaunchTorpedo (otherLaunchTorpedo);
		return;
	}

	public void SndTorpedoExplodeRequest (int torpedoID, int torpedoType, List<FiFish> fishs)
	{//发送鱼雷爆炸请求
		FiTorpedoExplodeRequest torpedoExplode = new FiTorpedoExplodeRequest ();
		torpedoExplode.torpedoId = torpedoID;
		torpedoExplode.torpedoType = torpedoType;
		torpedoExplode.fishes = new List<FiFish> ();
		foreach (FiFish fish in fishs) {
			if (null != fish) {
				torpedoExplode.fishes.Add (fish);
			}
		}

		dataControl.PushSocketSnd (FiEventType.SEND_TORPEDO_EXPLODE_REQUEST, torpedoExplode);
	}

	public void RcvTorpedoExplodeResponse (object data)
	{//接收鱼雷爆炸回复
		FiTorpedoExplodeResponse torpedoExplode = (FiTorpedoExplodeResponse)data;
		fishingObjects.ToTorpedoExplode (torpedoExplode);
		return;
	}

	public void RcvOtherTorpedoExplodeInform (object data)
	{//接收其他玩家鱼雷爆炸通知
		FiOtherTorpedoExplodeInform otherTorpedoExplode = (FiOtherTorpedoExplodeInform)data;
//		Debug.LogError ("接收到其它玩家鱼雷爆炸结果通知");
		fishingObjects.ToOtherTorpedoExplode (otherTorpedoExplode);

		return;
	}
}
