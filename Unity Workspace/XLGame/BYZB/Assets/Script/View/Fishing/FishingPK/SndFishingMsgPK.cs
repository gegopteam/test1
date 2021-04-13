/* author:KinSen
 * Date:2017.07.26
 */

using UnityEngine;
using System.Collections;
using System.Collections.Generic;

using AssemblyCSharp;

//负责：FishingPK通用的消息发送
public interface ISndFishingMsgPK
{
	
}

public static class SndFishingMsgPK
{
	private static DataControl dataControl = null;
	private static MyInfo myInfo = null;
	private static RoomInfo roomInfo = null;
	static SndFishingMsgPK()
	{
		dataControl= DataControl.GetInstance ();
		myInfo= dataControl.GetMyInfo ();
		roomInfo = dataControl.GetRoomInfo ();
	}

	//---------------------------FishingPK-----------------------------//

	//	public const int SEND_LEAVE_PK_ROOM_REQUEST          = 100015;
	//	public class   FiLeavePKRoomRequest
	//	{
	//		public int roomType;
	//		public int roomIndex;
	//	}
	//
	/*public static void SndPKLeaveRoomRequest<T>(this T example, int roomType, int roomIndex, int goldType) where T: ISndFishingMsgPK
	{
		FiLeavePKRoomRequest leaveRoom = new FiLeavePKRoomRequest ();
		leaveRoom.roomType = roomType;
		leaveRoom.roomIndex = roomIndex;
		leaveRoom.goldType = goldType;

		Debug.LogError ("发送自己离开PK场");

		dataControl.PushSocketSnd (FiEventType.SEND_LEAVE_PK_ROOM_REQUEST, leaveRoom);
	}*/

	//	//发射鱼雷
	//	public class FiLaunchTorpedoRequest
	//	{
	//		public int torpedoId;
	//		public int  torpedoType;
	//		public Cordinate position;
	//	}

	//此时玩家只是点击了屏幕发射鱼雷，鱼雷未爆炸
	public static void SndPKLaunchTorpedoRequest<T>(this T example, int torpedoID, int torpedoType, float  x, float  y) where T: ISndFishingMsgPK 
	{
		FiLaunchTorpedoRequest launchTorpedo = new FiLaunchTorpedoRequest ();
		launchTorpedo.torpedoId = torpedoID;
		launchTorpedo.torpedoType = torpedoType;
		launchTorpedo.position = new Cordinate ();
		launchTorpedo.position.x = x;
		launchTorpedo.position.y = y;
		dataControl.PushSocketSnd (FiEventType.SEND_PK_LAUNCH_TORPEDO_REQUEST, launchTorpedo);
	}

	//	public class  FiTorpedoExplodeRequest
	//	{
	//		public int   torpedoId;
	//		public int   torpedoType;
	//		public List<FiFish> fishes;	
	//	}
	//	//PK场鱼雷爆炸
	//	public const int SEND_PK_TORPEDO_EXPLODE_REQUEST         =  100030;

	//此时鱼雷爆炸，向服务器发送范围内鱼的列表
	public static void SndPKTorpedoExplodeRequest<T>(this T example, int torpedoID, int torpedoType, List<FiFish> fishs) where T: ISndFishingMsgPK
	{
		FiTorpedoExplodeRequest torpedoExplode = new FiTorpedoExplodeRequest ();
		torpedoExplode.torpedoId = torpedoID;
		torpedoExplode.torpedoType = torpedoType;
		torpedoExplode.fishes = new List<FiFish> ();
		foreach(FiFish fish in fishs)
		{
			if(null!=fish)
			{
				torpedoExplode.fishes.Add (fish);
			}
		}

		dataControl.PushSocketSnd (FiEventType.SEND_PK_TORPEDO_EXPLODE_REQUEST, torpedoExplode);
	}

	public static void SndPKEffectRequest<T>(this T example, int effectId) where T: ISndFishingMsgPK
	{//发送特效请求
		FiEffectRequest effectInfo = new FiEffectRequest ();
		effectInfo.userId = myInfo.userID;
		effectInfo.effect = new FiEffectInfo ();
		effectInfo.effect.type = effectId;
		//effectInfo.effect.type = effectId;

		dataControl.PushSocketSnd (FiEventType.SEND_PK_USE_EFFECT_REQUEST, effectInfo);

		Tool.OutLogWithToFile ("发送特效请求 effectId:"+effectId);
	}//*/

}
