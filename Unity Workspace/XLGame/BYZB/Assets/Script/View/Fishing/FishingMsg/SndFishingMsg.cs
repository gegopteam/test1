/* author:KinSen
 * Date:2017.07.26
 */

using UnityEngine;
using System.Collections;
using System.Collections.Generic;

using AssemblyCSharp;


//负责：各类捕鱼房间通用消息的发送，非通用信息各自写入对应的SndFishingMsg
//该类用于继承，所有消息函数设置public权限
public interface ISndFishingMsg
{
	
}

public static class SndFishingMsg
{
	private static DataControl dataControl = null;
	private static MyInfo myInfo = null;
	private static RoomInfo roomInfo = null;

	static SndFishingMsg ()
	{
		dataControl = DataControl.GetInstance ();
		myInfo = dataControl.GetMyInfo ();
		roomInfo = dataControl.GetRoomInfo ();
	}

	//*
	public static void SndUnlockCannon<T> (this T example, int nMultiple) where T: ISndFishingMsg
	{
		FiUnlockCannonRequest nRequest = new FiUnlockCannonRequest ();
		nRequest.targetMultiple = nMultiple;
		dataControl.PushSocketSnd (FiEventType.SEND_UNLOCK_CANNON_MULTIPLE_REQUEST, nRequest);
	}

	//*
	public static void SndFishOut<T> (this T example, int groupId, int fishId = 0) where T: ISndFishingMsg
	{//发送鱼群游出屏幕

		List<EnemyBase> listFish = UIFishingObjects.GetInstance ().fishPool.GetGroupFish (groupId, false);
		if (null != listFish) {
			if (listFish.Count > 1) {//如果组里还有除这条鱼外的其它鱼，不发送游出屏幕外的消息
				return;
			}
		}

	
		FiFishsOutRequest fishOut = new FiFishsOutRequest ();
		fishOut.groupId = groupId;

		Tool.OutLogWithToFile ("SndFishOut groupId:" + groupId);
		Debug.LogWarning ("SndFishOutGroup:" + groupId + "," + fishId);
		dataControl.PushSocketSnd (FiEventType.SEND_FISH_OUT_REQUEST, fishOut);
	}

	public static void SndLeaveRoom<T> (this T example) where T: ISndFishingMsg
	{//发送自己离开房间
		if (null != roomInfo) {
			FiLeaveRoomRequest leaveRoom = new FiLeaveRoomRequest ();
			leaveRoom.leaveType = roomInfo.roomType;
			leaveRoom.roomIndex = roomInfo.roomIndex;
			leaveRoom.roomMultiple = roomInfo.roomMultiple;

			dataControl.PushSocketSnd (FiEventType.SEND_USER_LEAVE_REQUEST, leaveRoom);
			Tool.OutLogWithToFile ("发送自己离开房间");
		}

	}

	public static void SndFireBullet<T> (this T example, int fireUserId, int bulletId, float x, float y, int mul, bool isBerserk = false, int groupId = -1, int fishId = -1) where T: ISndFishingMsg
	{//自己发送子弹
		//Debug.Log ("发送子弹ID:"+bulletId);
		//Tool.OutLogWithToFile ("自己发送子弹 bulletId:"+bulletId);
		FiFireBulletRequest fireBullet = new FiFireBulletRequest ();
		if (GameController._instance.isOverTurn) {
			x = -x;
			y = -y;
		}
		fireBullet.userId = fireUserId;
		fireBullet.cannonMultiple = mul;
		fireBullet.bulletId = bulletId;
		fireBullet.position = new Cordinate ();
		fireBullet.position.x = x;
		fireBullet.position.y = y;
		fireBullet.groupId = groupId;
		fireBullet.fishId = fishId;
		fireBullet.violent = isBerserk;
		dataControl.PushSocketSnd (FiEventType.SEND_FIRE_BULLET_REQUEST, fireBullet);
	}

	public static void SndHitFish<T> (this T example, int userId, int groupId, int fishId, int bulletId, float x, float y, int mul, bool berserk, int tempFishID, bool isMyRobot = false) where T: ISndFishingMsg
	{//发送子弹打到鱼请求
		//Debug.LogError("IsMyRobot=" + isMyRobot);
		if (userId != myInfo.userID && !isMyRobot)
			return;
		// Debug.LogError("RobotSndHitFish");
		//PrefabManager._instance.ShowNetEffect (new Vector3 (x,y,0));
		FiHitFishRequest hitFish = new FiHitFishRequest ();
		hitFish.groupId = groupId;
		hitFish.fishId = fishId;
		hitFish.userId = userId;
		hitFish.bulletId = bulletId;
		hitFish.cannonMultiple = mul;
		hitFish.position = new Cordinate ();
		hitFish.position.x = x;
		hitFish.position.y = y;
		hitFish.violent = berserk;
		hitFish.beiyongFishID = tempFishID;
		dataControl.PushSocketSnd (FiEventType.SEND_HITTED_FISH_REQUEST, hitFish);

		//Tool.OutLogWithToFile ("发送子弹打到鱼请求 userId:"+myInfo.userID+" bulletId:"+bulletId);
	}

	public static void SndChangeCannonMultiple<T> (this T example, int cannonMultiple, int userId) where T: ISndFishingMsg
	{//发送改变炮倍数
		FiChangeCannonMultipleRequest changeCannon = new FiChangeCannonMultipleRequest ();
		changeCannon.cannonMultiple = cannonMultiple;
		changeCannon.userId = userId;
		dataControl.PushSocketSnd (FiEventType.SEND_CHANGE_CANNON_REQUEST, changeCannon);

		Tool.OutLogWithToFile ("发送改变炮倍数 multiple:" + cannonMultiple);
	}

	public static void SndEffectRequest<T> (this T example, int effectId, int skillUserId = -1) where T: ISndFishingMsg
	{//发送特效请求
		FiEffectRequest effectInfo = new FiEffectRequest ();
		if (skillUserId == -1)
			effectInfo.userId = myInfo.userID;
		else
			effectInfo.userId = skillUserId;
		effectInfo.effect = new FiEffectInfo ();
		effectInfo.effect.type = effectId;
		//effectInfo.effect.type = effectId;

		dataControl.PushSocketSnd (FiEventType.SEND_USE_EFFECT_REQUEST, effectInfo);
		//Debug.LogError ("SendEffectId:" + effectId);

		Tool.OutLogWithToFile ("发送特效请求 effectId:" + effectId);
	}

	//	//发射鱼雷
	//	public class FiLaunchTorpedoRequest
	//	{
	//		public int torpedoId;
	//		public int  torpedoType;
	//		public Cordinate position;
	//	}
	//	//发送鱼雷消息
	//	public const int SEND_LAUNCH_TORPEDO_REQUEST          = 100025;
	public static void SndLaunchTorpedoRequest<T> (this T example, int torpedoId, int torpedoType, float  x, float  y) where T: ISndFishingMsg
	{
		FiLaunchTorpedoRequest launchTorpedo = new FiLaunchTorpedoRequest ();
		launchTorpedo.position = new Cordinate ();
		launchTorpedo.torpedoId = torpedoId;
		launchTorpedo.torpedoType = torpedoType;
		launchTorpedo.position.x = x;
		launchTorpedo.position.y = y;
		dataControl.PushSocketSnd (FiEventType.SEND_LAUNCH_TORPEDO_REQUEST, launchTorpedo);
//        Debug.LogError("snd torpedoId="+torpedoId);
	}

	//	public class  FiTorpedoExplodeRequest
	//	{
	//		public int   torpedoId;
	//		public int   torpedoType;
	//		public List<FiFish> fishes;
	//	}
	public static void SndTorpedoExplodeRequest<T> (this T example, int torpedoID, int torpedoType, List<FiFish> fishs) where T: ISndFishingMsg
	{//鱼雷爆炸
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

	public static void SndNotifySignUpRequest<T> (this T example, int type) where T: ISndFishingMsg
	{
		FiNotifySignUp signUp = new FiNotifySignUp ();
		signUp.type = type;
//		signUp.roomIndex = roomIndex;
//		signUp.signUpGold = signUpGold;
//		signUp.gameType = gameType;
//		UnityEngine.Debug.LogError ("type = " + type);
		dataControl.PushSocketSnd (FiEventType.SEND_XL_NOTIFYSIGNUP_RESQUSET, signUp);
	}

	/// <summary>
	/// 刷新时间
	/// </summary>
	/// <param name="example">Example.</param>
	/// <typeparam name="T">The 1st type parameter.</typeparam>
	public static void SndUpdateBossMatchTimeRequest<T> (this T example) where T: ISndFishingMsg
	{
		dataControl.PushSocketSnd (FiEventType.SEND_XL_BOSSMATCHTIME_RESQUSET, null);
	}
	/*public static void SndPurchaseRequest<T>(this T example, int nType , int nValue ) where T: ISndFishingMsg
	{
		FiPurchasePropertyRequest nRequest = new FiPurchasePropertyRequest ();
		nRequest.info.type = nType;
		nRequest.info.value = nValue;
		dataControl.PushSocketSndByte ( FiEventType.SEND_PURCHASE_PROPERTY_REQUEST , nRequest.serialize() );
	}*/


	//	//---------------------------FishingPK-----------------------------//
	//
	//	//	public const int SEND_LEAVE_PK_ROOM_REQUEST          = 100015;
	//	//	public class   FiLeavePKRoomRequest
	//	//	{
	//	//		public int roomType;
	//	//		public int roomIndex;
	//	//	}
	//	//
	//
	//	//	//发射鱼雷
	//	//	public class FiLaunchTorpedoRequest
	//	//	{
	//	//		public int torpedoId;
	//	//		public int  torpedoType;
	//	//		public Cordinate position;
	//	//	}
	//	public static void SndPKLaunchTorpedoRequest<T>(this T example, int torpedoID, int torpedoType, int x, int y) where T: ISndFishingMsg
	//	{
	//		FiLaunchTorpedoRequest launchTorpedo = new FiLaunchTorpedoRequest ();
	//		launchTorpedo.torpedoId = torpedoID;
	//		launchTorpedo.torpedoType = torpedoType;
	//		launchTorpedo.position = new Cordinate ();
	//		launchTorpedo.position.x = x;
	//		launchTorpedo.position.y = y;
	//		dataControl.PushSocketSnd (FiEventType.SEND_PK_LAUNCH_TORPEDO_REQUEST, launchTorpedo);
	//	}
	//	//	public class  FiTorpedoExplodeRequest
	//	//	{
	//	//		public int   torpedoId;
	//	//		public int   torpedoType;
	//	//		public List<FiFish> fishes;
	//	//	}
	//	//	//PK场鱼雷爆炸
	//	//	public const int SEND_PK_TORPEDO_EXPLODE_REQUEST         =  100030;
	//	public static void SndPKTorpedoExplodeRequest<T>(this T example, int torpedoID, int torpedoType, List<FiFish> fishs) where T: ISndFishingMsg
	//	{
	//		FiTorpedoExplodeRequest torpedoExplode = new FiTorpedoExplodeRequest ();
	//		torpedoExplode.torpedoId = torpedoID;
	//		torpedoExplode.torpedoType = torpedoType;
	//		torpedoExplode.fishes = new List<FiFish> ();
	//		foreach(FiFish fish in fishs)
	//		{
	//			if(null!=fish)
	//			{
	//				torpedoExplode.fishes.Add (fish);
	//			}
	//		}
	//
	//		dataControl.PushSocketSnd (FiEventType.SEND_PK_TORPEDO_EXPLODE_REQUEST, torpedoExplode);
	//	}
	//
	//	public static void SndPKEffectRequest<T>(this T example, int effectId) where T: ISndFishingMsg
	//	{//发送特效请求
	//		FiEffectRequest effectInfo = new FiEffectRequest ();
	//		effectInfo.userId = myInfo.userID;
	//		effectInfo.effect = new FiEffectInfo ();
	//		effectInfo.effect.type = effectId;
	//		//effectInfo.effect.type = effectId;
	//
	//		dataControl.PushSocketSnd (FiEventType.SEND_PK_USE_EFFECT_REQUEST, effectInfo);
	//
	//		Tool.OutLogWithToFile ("发送特效请求 effectId:"+effectId);
	//	}//*/

}
