/* author:KinSen
 * Date:2017.03.21
 */

using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;
using AssemblyCSharp;

//负责：Fishing部分UI对象的管理
public class UIFishingObjects
{
	private static UIFishingObjects instance = null;

	public static UIFishingObjects GetInstance ()
	{
		if (null == instance) {
			instance = new UIFishingObjects ();
		}

		return instance;
	}

	public static void DestroyInstance ()
	{
		if (null != instance) {
			instance = null;
		}
	}


	public CannonManager cannonManage = null;

	public BulletPool bulletPool = null;
	public TorpedoPool torpedoPool = null;
	public FishPool fishPool = null;

	private DataControl dataControl = null;
	private MyInfo myInfo = null;
	private RoomInfo roomInfo = null;

	private UIFishingObjects ()
	{
		Init ();
	}

	~UIFishingObjects ()
	{
		UnInit ();
	}

	void Init ()
	{
		dataControl = DataControl.GetInstance ();
		myInfo = dataControl.GetMyInfo ();
		roomInfo = dataControl.GetRoomInfo ();

		cannonManage = new CannonManager ();
		bulletPool = new BulletPool ();
		torpedoPool = new TorpedoPool ();
		fishPool = new FishPool ();

	}

	void UnInit ()
	{
		Clear ();

		dataControl = null;
		myInfo = null;
		roomInfo = null;

		cannonManage = null;
		bulletPool = null;
		torpedoPool = null;
		fishPool = null;
		tempDistribute = null;//道具信息
	}

	public void Clear ()
	{
		if (null != cannonManage) {
			cannonManage.ClearAll ();
		}

		if (null != bulletPool) {
			bulletPool.Clear ();
		}

		if (null != torpedoPool) {
			torpedoPool.Clear ();
		}

		if (null != fishPool) {
			fishPool.Clear ();
		}

	}

	//进入经典场入口
	public void EnterClassicRoom (long nGold, long nRoomIndex, int nSeatIndex, List<FiUserInfo> nUserArray)
	{
		//MyInfo nInfo =(MyInfo) Facade.GetFacade ().data.Get ( FacadeConfig.USERINFO_MODULE_ID );
		myInfo.gold = nGold;
		myInfo.seatIndex = nSeatIndex;

		roomInfo.roomIndex = nRoomIndex;
		roomInfo.InitUser (nUserArray);
		UIFishingMsg.GetInstance ().SetFishing (TypeFishing.CLASSIC);
		myInfo.lastGame.type = TypeFishing.CLASSIC;
		//初始化炮台信息
		UIFishingObjects.GetInstance ().InitCannonInfo ();
		//跳转到渔场
		myInfo.TargetView = AppView.FISHING;
		AppControl.ToView (AppView.LOADING);
	}

	public void EnterPkFishingRoom (int nRoomIndex, int nRoomType)
	{
		//RoomInfo roomInfo =(RoomInfo)Facade.GetFacade ().data.Get ( FacadeConfig.ROOMINFO_MODULE_ID );
		//MyInfo myInfo =(MyInfo)Facade.GetFacade ().data.Get ( FacadeConfig.USERINFO_MODULE_ID );
		if (nRoomIndex != -1) {
			roomInfo.roomIndex = nRoomIndex;
		}
		roomInfo.roomType = nRoomType;
		myInfo.lastGame.type = nRoomType;
		Debug.LogError ("-------roomInfo.roomIndex-----" + roomInfo.roomIndex);
		UIFishingMsg.GetInstance ().SetFishing (nRoomType);
		InitCannonInfo ();
		//MyInfo myInfo = (MyInfo)Facade.GetFacade ().data.Get ( FacadeConfig.USERINFO_MODULE_ID );
		myInfo.TargetView = AppView.FISHING;
		AppControl.ToView (AppView.LOADING);
	}




	//废弃
	/*public void CreateAllCannonInfo()
//	{
//		FiUserInfo mSelfInfo = new FiUserInfo ();
//		mSelfInfo.userId = myInfo.userID;
//		mSelfInfo.seatIndex = myInfo.seatIndex;
//		mSelfInfo.gender = myInfo.sex;
//		mSelfInfo.nickName = myInfo.nickname;
//		mSelfInfo.gold = myInfo.gold;
//		mSelfInfo.diamond = myInfo.diamond;
//		mSelfInfo.cannonMultiple = myInfo.cannonMultipleMax;
//		mSelfInfo.avatar = myInfo.avatar;
//
//		mSelfInfo.level = myInfo.level;
//		mSelfInfo.experience = myInfo.experience;
//		mSelfInfo.vipLevel = myInfo.levelVip;
//		mSelfInfo.maxCannonMultiple = myInfo.cannonMultipleMax;
//		mSelfInfo.cannonStyle = myInfo.cannonStyle;
//		AddCannonInfo (mSelfInfo);
//
//		List<FiUserInfo> listUser = roomInfo.GetUsers ();
//		Debug.LogError ("RcvPKStartGameResponse userID------------:"+ listUser.Count );
//		if (null != listUser)
//		{
//			foreach(FiUserInfo user in listUser)
//			{
//				Debug.LogError ("RcvPKStartGameResponse userID:"+user.userId+" seatIndex:"+user.seatIndex);
//				AddCannonInfo (new FiUserInfo (user));
//			}
//		}
//	}*/

	//根据加入渔场的玩家信息，初始化渔场需要的炮台信息
	public void InitCannonInfo ()
	{
		FiUserInfo myUserInfo = new FiUserInfo ();
		myUserInfo.userId = myInfo.userID;
		myUserInfo.seatIndex = myInfo.seatIndex;
		myUserInfo.gender = myInfo.sex;
		myUserInfo.nickName = myInfo.nickname;
		myUserInfo.gold = myInfo.gold;
		myUserInfo.diamond = myInfo.diamond;
		myUserInfo.cannonMultiple = myInfo.cannonMultipleMax;
		myUserInfo.avatar = myInfo.avatar;

		myUserInfo.level = myInfo.level;
		myUserInfo.experience = myInfo.experience;
		myUserInfo.vipLevel = myInfo.levelVip;
		myUserInfo.maxCannonMultiple = myInfo.cannonMultipleMax;
		myUserInfo.cannonStyle = myInfo.cannonStyle;
		myUserInfo.cannonBabetteStyle = myInfo.cannonBabetteStyle;

		myUserInfo.testCoin = myInfo.testCoin;
		myUserInfo.isRoomTest = myInfo.isTestRoom;
		//Debug.LogError ("myUserInfo.cannonStyle:"+myInfo.cannonStyle);
      
		AddCannonInfo (myUserInfo);
//		Debug.LogError ("myInfo.levelVip = " + myInfo.levelVip);

		List<FiUserInfo> listUser = roomInfo.GetUsers ();
		if (null != listUser) {
			foreach (FiUserInfo user in listUser) {
//				Debug.LogError ("RcvPKStartGameResponse userID:" + user.userId + " seatIndex:" + user.seatIndex);
				AddCannonInfo (new FiUserInfo (user));
			}
		}

//		Debug.LogError ("-------InitCannon---------" + listUser.Count);
		//cannonManage.InitCannon ();
		//CreateCannons ();

	}

	void AddCannonInfo (FiUserInfo userInfo)
	{
		CannonInfo cannon = new CannonInfo ();
		cannon.userId = userInfo.userId;
		cannon.seatIndex = userInfo.seatIndex;
		cannon.cannonMultiple = userInfo.cannonMultiple;
		cannon.userInfo = new FiUserInfo (userInfo);
		cannonManage.AddInfo (cannon);
	}

	/*public void CreateCannons()
	{
		cannonManage.CreateCannons ();
	}*/

	/**其他玩家进入渔场，创建炮台*/
	public void CreateCannon (FiUserInfo userInfo)
	{
		//Tool.LogError ("AddCannon");
		if (null == userInfo)
			return;
		CannonInfo cannonInfo = new CannonInfo ();
		cannonInfo.userId = userInfo.userId;
		cannonInfo.seatIndex = userInfo.seatIndex;
		cannonInfo.cannonMultiple = userInfo.cannonMultiple;
		cannonInfo.userInfo = userInfo;

		RemoveCannon (userInfo.userId);
		cannonManage.AddInfo (cannonInfo);
		cannonInfo.cannon = cannonManage.CreateSingleCannon (cannonInfo.userInfo);
	}

	public void RemoveCannon (int userID)
	{
		cannonManage.RemoveSingleCannon (userID);
		cannonManage.RemoveInfo (userID);
	}

	public List<FiFishsCreatedInform> fishCreatedCache = new List<FiFishsCreatedInform> ();


	public void CreateFish (FiFishsCreatedInform info)
	{
		if (null == info)
			return;
		if (null != EnemyPoor._instance) {
			FormationType tempFormation = FormationType.None;

			if (info.fishNum > 1) { //如果数量大于1，说明鱼是有队列的
				if (info.fishType >= 1 && info.fishType <= 3) { //编号1-3的鱼群按照单双走直线或双直线
					if (info.fishNum % 2 == 0) {
						tempFormation = FormationType.Bilinear;
					} else {
						tempFormation = FormationType.Linear;
					}
				} else if (info.fishType >= 4 && info.fishType <= 7) {//编号4-7的鱼，按照数量一起移动，同数量走不同的阵列  
					if (info.fishNum > 5) { //超过5，说明是鱼潮的鱼，此时还是要走直线
						tempFormation = FormationType.Linear;
					} else {
						tempFormation = FormationType.Normal;
					}
						
				} else if (info.fishNum > 1) { //说明是鱼潮的鱼，但编号>7
					tempFormation = FormationType.Linear;
				} else
					Debug.LogError ("Error!" + "fishNum:" + info.fishNum + " fishType:" + info.fishType + " groupId:" + info.groupId);
			}

			for (int i = info.fishNum - 1; i >= 0; i--) { //这里i必须要从大到小递减，因为i=0时的轨迹并不是动态生成的
				EnemyPoor._instance.CreateEnemy (info.trackType, info.trackId, info.groupId, info.fishType, i, tempFormation, info.fishNum, info.tideType);
			}

		} else {
			// Debug.LogError("Error! EnemyPoor=null");
			fishCreatedCache.Add (info);
		}
	}

	public void ToFireBullet (FiFireBulletRequest info)
	{
		CannonInfo cannonInfo = cannonManage.GetInfo (info.userId);
		//Debug.LogError("FireToPos Id=" + info.userId + " pos=(" + info.position.x + "," + info.position.y+")");
		if (null != cannonInfo) {
			if (null != cannonInfo.cannon) {
				//Tool.OutLogWithToFile ("找到该用户的炮 发射子弹 userId:"+info.userId+" bulletId:"+info.bulletId);
				if (GameController._instance.isOverTurn) {
					info.position.x = -info.position.x;
					info.position.y = -info.position.y;

				}
				cannonInfo.cannon.FireToPos (info.bulletId, info.cannonMultiple, info.position.x, info.position.y, info.violent, info.groupId, info.fishId);
			} else {
				//HintTextPanel._instance.SetTextShow("找不到用户id:" + info.userId);
//				Debug.LogError ("Can't find user,id=" + info.userId);
			}
		} else {
			Tool.OutLogWithToFile("找不到該用戶炮的信息 userId:" + info.userId);
			Debug.LogError ("FireToPos Id:" + info.userId + "=null");
		}
	}

	public void ToHitFish (FiHitFishResponse info)
	{
		if (GameController._instance == null) { //如果检测不到GameController，说明渔场还没初始化
			Debug.LogError ("Error! ToHitFish:GameControl=null");
			return;
		}
		//先找到对应bulletId的子弹 
		Bullet bullet = bulletPool.GetBullet (info.userId, info.bulletId);
		if (null != bullet) {//在对应的坐标位子爆炸渔网
			if (bullet.thisType == Bullet.BulletType.LaserLight) {
//				bullet.LaserLightHitFish ();
			} else {
				bullet.HitFish ();
				bulletPool.Remove (info.bulletId, info.userId);
			}
			//Tool.OutLogWithToFile ("找到对应的子弹在对应的坐标位子爆炸渔网 userId:"+info.userId+" bulletId:"+info.bulletId);
		} else {
			Tool.OutLogWithToFile("找不到對應的子彈在對應的坐標位子爆炸漁網 userId:" + info.userId + " bulletId:" + info.bulletId);
            //Debug.Log("找不到对应的子弹在对应的坐标位子爆炸渔网 userId:" + info.userId + " bulletId:" + info.bulletId);
		}

		bool shouldDead = false;
		if (null != info.propertyArray) {
			if (0 != info.propertyArray.Count) {
				EnemyBase enemy = fishPool.Get (info.groupId, info.fishId);
				if (null != enemy) {
					shouldDead = true;
					enemy.Hitted (info.userId, shouldDead, info.propertyArray);
					//Debug.Log ("成功捕到鱼 2 groupId:"+info.groupId + " fhisId"+info.fishId+"myinfo.gold"+myInfo.gold);
                   
					//FishReward.OpenReward (info.userId, new Cordinate (enemy.transform.position.x,enemy.transform.position.y), info.propertyArray);
					
				} else {
					Tool.OutLogWithToFile ("找不到鱼,group=" + info.groupId + "id=" + info.fishId);
                    //Debug.Log("找不到鱼,group=" + info.groupId + "id=" + info.fishId);
					//Tool.OutLogWithToFile ("找不到鱼 成功捕到鱼 groupId:"+info.groupId + " fhisId"+info.fishId);
					FishReward.OpenReward (info.userId, info.position, info.propertyArray);
				}

			
			}
		} else {
			Tool.OutLogWithToFile ("RcvHitFishResponse null==info.propertyArray");
		}

	}

	/// <summary>
	/// 天降横财特效以及金币值变化的地方
	/// </summary>
	/// <param name="info">Info.</param>
	public void ToHitFish_RedPack (FiHitFishResponse info)
	{



		int goldnum = 0;
//		Debug.Log ("开始播放特效~~~~~~~~~~~");
		for (int i = 0; i < info.propertyArray.Count; i++) {
//			Debug.Log (string.Format ("类型：{0}数值：{1}", info.propertyArray [i].type, info.propertyArray [i].value));
			if (info.propertyArray [i].type == 1000) {
				CannonInfo userinfo = cannonManage.GetInfo (info.userId);
				userinfo.cannon.gunUI.ShowRedPackEffect (info.propertyArray [i].value);
				goldnum = info.propertyArray [i].value;
                //Debug.Log("我的id:" + myInfo.userID +",中奖的id:" + info.userId);
      
                PrefabManager._instance.GetGunByUserID(info.userId).gunUI.AddValue(0, goldnum);
                //if (userinfo.cannon.isLocal)
                //{

                //    if (userinfo.cannon.thisSeat == GunSeat.RB || userinfo.cannon.thisSeat == GunSeat.LT)
                //    {
                //        PrefabManager._instance.GoldCoinColumnRight.GetComponent<GoldCoinColumn>().BuildGoldCoinColumn(userinfo, goldnum, 0, userinfo.cannon.thisSeat);
                //    }
                //    else if (userinfo.cannon.thisSeat == GunSeat.LB || userinfo.cannon.thisSeat == GunSeat.RT)
                //    {
                //        PrefabManager._instance.GoldCoinColumnLeft.GetComponent<GoldCoinColumn>().BuildGoldCoinColumn(userinfo, goldnum, 0, userinfo.cannon.thisSeat);
                //    }
                //    //   Debug.Log("自己的金币柱"+PrefabManager._instance.GetLocalGun().thisSeat);
                //}
			}
		}
        Debug.Log("金幣增加:" + goldnum);

        //		Debug.Log (string.Format ("info.userId:{0},myInfo.userID:{1}", info.userId, myInfo.userID));

        AudioManager._instance.PlayRedPackAudio();

		if (info.userId == myInfo.userID) {
//			Debug.Log ("播放聚宝盆动画~~~~~~~~~~~");
//			Vector3 dropUIPos = ScreenManager.WorldToUIPos (new Vector3 (18, -46, 1402));
			Transform canvasTrans = GameObject.FindGameObjectWithTag (TagManager.uiCanvas).transform;
			GameObject tempHandle = GameObject.Instantiate (PrefabManager._instance.RedPackUIEffect_big, canvasTrans) as GameObject;
			tempHandle.GetComponent<RectTransform> ().localScale = Vector3.one;
			tempHandle.GetComponent<RectTransform> ().localPosition = new Vector3 (18, -46, 1402);

            tempHandle.transform.Find("Gold_Num").GetComponent<Text>().text = goldnum.ToString();
			//= System.Text.RegularExpressions.Regex.Replace (goldnum.ToString (), @"(\w{3})", "$1,").Trim (',');

			GameObject.Destroy (tempHandle, 5.0f);
		}

	}

 
	public void ChangeCannonMultiple (int userID, int multiple)
	{
		CannonInfo info = null;
		if (userID == myInfo.userID) {
			info = myInfo.GetCannonInfo ();
		} else {
			info = cannonManage.GetInfo (userID);
		}
			
		//Tool.OutLogWithToFile ("接收其他玩家改变炮倍数 multiple:"+multiple);
		if (null != info) {
			if (null != info.cannon) {
				info.cannon.gunUI.SetMultiple (multiple, false);
			}
		} else {
			Tool.OutLogWithToFile ("接收其他玩家改變炮倍數 找不到該玩家的砲信息 userId:" + userID);
		}
//		Debug.Log (userID + "其他玩家改变倍数");
		//改变炮倍数时隐藏排名 又不这样做了
		//if (PrefabManager._instance != null) {
		//	GunControl gunControl = PrefabManager._instance.GetGunByUserID (userID);
		//	gunControl.gunUI.OpenAndCloseRank (false);
		//}
	}

	public void ToEffect (FiEffectInfo info, int userID = 0)
	{
		if (null == info)
			return;

		if (GameController._instance == null) {
			Debug.LogError ("Error! ToEffect: GameController=null");
			return;
		}

		bool my = false;
		if (0 == userID)
			my = true;


		if (!my && false) { //其他人购买道具扣钻石有了专门的协议，不需要在这里扣钻石了
			if (GameController._instance != null && PrefabManager._instance != null) {
				if (GameController._instance.myGameType == GameType.Classical) {
					Skill tempSkill = PrefabManager._instance.GetSkillUIByServerId (info.type);
					if (tempSkill != null) {
						PrefabManager._instance.GetGunByUserID (userID).gunUI.AddValue (0, 0, -tempSkill.priceNum);
					}
                
				}
			}
		}

		switch (info.type) {
		case FiPropertyType.FISHING_EFFECT_FREEZE://冰冻
			List<EnemyBase > tempGroup = new List<EnemyBase> ();
			if (info.value.Count > 0) { //info.value为int类型的List，代表鱼的GroupId
				foreach (int index in info.value) {
					List<EnemyBase > listGet = fishPool.GetGroupFish (index);
					if (null != listGet) {
						tempGroup.AddRange (listGet);
					}
				}
			}
			if (PrefabManager._instance == null) {
				Debug.LogError ("Error!PrefabManager=null");
				return;
			}
			Skill tempSkill = PrefabManager._instance.GetSkillUIByType (SkillType.Freeze);
			if (my) {
				tempSkill.ActiveSkill (tempGroup);//在skill类里执行冰冻鱼的操作,并且道具数量-1
			} else {
				tempSkill.FreezeEnemy (tempGroup);//区别在于不用减道具数量
			}

			break;
		case FiPropertyType.FISHING_EFFECT_AIM://锁定,只有自己可用
			{
				if (my) {
					PrefabManager._instance.GetSkillUIByType (SkillType.Lock).RepareToUseSkill ();
				}
			}
			break;
		case FiPropertyType.FISHING_EFFECT_VIOLENT://狂暴
			{
				if (my) {
					PrefabManager._instance.GetSkillUIByType (SkillType.Berserk).ActiveSkill ();
				} else {
					Skill berserk = PrefabManager._instance.GetSkillUIByType (SkillType.Berserk);
					cannonManage.GetInfo (userID).cannon.UseSkill (berserk.skillType, berserk.skillDuration);
				}
			}

			break;
		case FiPropertyType.FISHING_EFFECT_SUMMON://只有自己可用//召唤，召唤阵的特效在鱼生成的时候，根据自己的轨迹是否属于召唤轨迹而自行决定，所以这里只是激活技能图标的cd效果，并无其它功能
			{
				if (my) {
					PrefabManager._instance.GetSkillUIByType (SkillType.Summon).ActiveSkill ();//
				}
			}
			break;
		case FiPropertyType.TORPEDO_MINI:
			Debug.LogError ("Rcv Bomb1");
			if (my) {
				PrefabManager._instance.GetSkillUIByType (SkillType.Torpedo, 1).ActiveSkill ();
			} else {
			
			}

			break;
		case FiPropertyType .FISHING_EFFECT_DOUBLE:
			if (my) {
				PrefabManager._instance.GetSkillUIByType (SkillType.Double).ActiveSkill ();
			} else {
				Skill doubleSkill = PrefabManager._instance.GetSkillUIByType (SkillType.Double);
				cannonManage.GetInfo (userID).cannon.UseSkill (doubleSkill.skillType, doubleSkill.skillDuration);
			}
			break;
		case FiPropertyType.FISHING_EFFECT_REPLICATE:
			if (my) {
				PrefabManager._instance.GetSkillUIByType (SkillType.Replication).ActiveSkill ();
			} else {
				Skill replicateSkill = PrefabManager._instance.GetSkillUIByType (SkillType.Replication);
				cannonManage.GetInfo (userID).cannon.UseSkill (replicateSkill.skillType, replicateSkill.skillDuration);
			}
			break;    
		default:
			break;
		}
	}

	public void ToFishOut (int groupID)
	{
		fishPool.RemoveGroupFish (groupID);
	}

	public void FreezeTimeOut (FiFreezeTimeOutInform freezeTimeOut)
	{
		List<EnemyBase > tempGroup = new List<EnemyBase> ();
		foreach (int index in freezeTimeOut.value) {
			List<EnemyBase > listGet = fishPool.GetGroupFish (index);
			if (null != listGet) {
				tempGroup.AddRange (listGet);
				//Debug.LogError ("冰冻超时:" + index);
			}

		}
		//freezeTimeOut.value; //冻住鱼的groupId
		Tool.OutLogWithToFile("接收冰凍時間超時消息");
		if (PrefabManager._instance == null)
			return;
		PrefabManager._instance.GetSkillUIByType (SkillType.Freeze).ThawEnemy (tempGroup);
	}


	public List<EnemyBase> GetGroupFish (int groupID)
	{
		return fishPool.GetGroupFish (groupID, false);
	}
	//	public class FiLaunchTorpedoResponse
	//	{
	//		public int  result;
	//		public int  torpedoId;
	//		public int  torpedoType;
	//		public Cordinate position;
	//	}
	public void ToLaunchTorpedo (FiLaunchTorpedoResponse launchTorpedo)
	{//发射鱼雷
		//自己发的鱼雷，无论结果是成功还是失败，都不需要做处理，因为鱼雷已经发出去了，具体处理要在收到爆炸恢复回复结果的地方
		if (0 == launchTorpedo.result) {
			//Debug.LogError ("收到自己发送鱼雷成功");
		} else {
			Debug.LogError("收到自己發送魚雷失敗，result=" + launchTorpedo.result);
		}
	}
	//	public class  FiOtherLaunchTorpedoInform
	//	{
	//		public int userId;
	//		public int torpedoId;
	//		public int torpedoType;
	//		public Cordinate position;
	//	}
	public void ToOtherLaunchTorpedo (FiOtherLaunchTorpedoInform otherLaunchTorpedo)
	{//其他玩家发射鱼雷
		int level = Skill.GetTorpedoLevelFromServerId (otherLaunchTorpedo.torpedoType);

		GunControl otherGun = UIFishingObjects.GetInstance ().cannonManage.GetInfo (otherLaunchTorpedo.userId).cannon;

		Vector3 tempPos = new Vector3 (otherLaunchTorpedo.position.x, otherLaunchTorpedo.position.y, 0);

		otherGun.FireOneTorpedo (tempPos, level);
	}

	//	public class  FiTorpedoExplodeResponse
	//	{
	//		public int     result;
	//		public int     torpedoId;
	//		public int     torpedoType;
	//		public List<FiFishReward>  rewards;
	//	}
	public void ToTorpedoExplode (FiTorpedoExplodeResponse torpedoExplode)
	{
		//触发鱼雷爆炸
		if (0 == torpedoExplode.result) {
			//Debug.LogError ("接收自己鱼雷爆炸成功");
			int sumGold = 0;
			int torpedoID = torpedoExplode.torpedoType - 2003;
			PrefabManager._instance.GetSkillUIByType (SkillType.Torpedo, torpedoID).ReduceRestNum ();
			for (int i = 0; i < torpedoExplode.rewards.Count; i++) {

				EnemyBase tempEnemy = UIFishingObjects.GetInstance ().fishPool.Get (torpedoExplode.rewards [i].groupId,
					                      torpedoExplode.rewards [i].fishId);
				if (torpedoExplode.rewards [i].groupId + torpedoExplode.rewards [i].fishId == 0) {
					foreach (FiProperty property in torpedoExplode.rewards[i].properties) {
						if (null != property) {
							switch (property.type) {
							case FiPropertyType.GOLD: //加金币
								sumGold += property.value;
								myInfo.gold += sumGold;//在这里加币试试
//								Debug.Log ("金币111111" + property.value + "myinfo.gold" + myInfo.gold);
//								Debug.LogError ("GetTorpedoGold:" + property.value);
								break;
							case FiPropertyType.EXP:
//								Debug.Log ("经验111111" + property.value);
//								Debug.LogError ("GetTorpedoExp:" + property.value);
								break;
							}
						}
					}

				}


				if (tempEnemy != null) {
					tempEnemy.Hitted (myInfo.userID, true, torpedoExplode.rewards [i].properties, true);
					Debug.LogError ("FishDead: groupId=" + torpedoExplode.rewards [i].groupId +
					" fishId=" + torpedoExplode.rewards [i].fishId + "name=" + tempEnemy.gameObject.name); 
					foreach (FiProperty property in torpedoExplode.rewards [i].properties) {
						if (null != property) {
							switch (property.type) {
							case FiPropertyType.GOLD: //加金币
								sumGold += property.value;
//								Debug.LogError ("GetTorpedoGold:" + property.value);
								break;
							case FiPropertyType.EXP:
//								Debug.LogError ("GetTorpedoExp:" + property.value);
								break;
							}
						}
					}

				} else {

				}
			}

//			Debug.LogError ("Gold=" + torpedoExplode.rewards [0].properties [0].value); 
			GameObject[] torpedoObjGroup = GameObject.FindGameObjectsWithTag (TagManager.torpedoBullet);

			for (int i = 0; i < torpedoObjGroup.Length; i++) {
				TorpedoBullet tempTorpedo = torpedoObjGroup [i].GetComponent<TorpedoBullet> ();
				if (tempTorpedo.bulletId == torpedoExplode.torpedoId && tempTorpedo.isLocal) {
					torpedoObjGroup [i].GetComponent<TorpedoBullet> ().ReturnGold (sumGold, DataControl.GetInstance ().GetMyInfo ().userID);
				}
			}
			if (sumGold >= 1000)
				PrefabManager._instance.PlaySuddenlyRichEffect (sumGold);
		} else if (torpedoExplode.result == -100) {
			HintTextPanel._instance.SetTextShow("今日魚雷使用已達上限", 2f);
		} else {
			Debug.LogError ("Error! torpedoExplode.result=" + torpedoExplode.result);
		}

	}
	//	public class  FiOtherTorpedoExplodeInform
	//	{
	//		public int      userId;
	//		public int      torpedoId;
	//		public int      torpedoType;
	//		public List<FiFishReward> rewards;
	//	}
	public void ToOtherTorpedoExplode (FiOtherTorpedoExplodeInform otherTorpedoExplode)
	{//触发其他玩家的鱼雷爆炸
		
//		Debug.LogError ("OtherTorpedoExplode:userId=" + otherTorpedoExplode.userId + " count=" + otherTorpedoExplode.rewards.Count);

		int sumGold = 0;
		for (int i = 0; i < otherTorpedoExplode.rewards.Count; i++) {
			if (otherTorpedoExplode.rewards [i].groupId + otherTorpedoExplode.rewards [i].fishId == 0) {
				foreach (FiProperty property in otherTorpedoExplode.rewards[i].properties) {
					if (null != property) {
						switch (property.type) {
						case FiPropertyType.GOLD: //加金币
							sumGold += property.value;
//							Debug.LogError ("OtherGetTorpedoGold:" + property.value);
							break;
						case FiPropertyType.EXP:
//							Debug.LogError ("OtherGetTorpedoExp:" + property.value);
							break;
						}
					}
				}

			} else {
				EnemyBase tempEnemy = UIFishingObjects.GetInstance ().fishPool.Get (otherTorpedoExplode.rewards [i].groupId,
					                      otherTorpedoExplode.rewards [i].fishId);
				if (tempEnemy != null) {
					tempEnemy.Dead ();
				}
			}
		}

		GameObject[] torpedoObjGroup = GameObject.FindGameObjectsWithTag (TagManager.torpedoBullet);
		for (int i = 0; i < torpedoObjGroup.Length; i++) {
			TorpedoBullet tempTorpedo = torpedoObjGroup [i].GetComponent<TorpedoBullet> ();

			if (tempTorpedo.bulletId == otherTorpedoExplode.torpedoId && tempTorpedo.userId == otherTorpedoExplode.userId) {
				torpedoObjGroup [i].GetComponent<TorpedoBullet> ().ReturnGold (sumGold, otherTorpedoExplode.userId);

			}
		}

		return;
		//老的鱼雷代码，已废弃
		for (int i = 0; i < otherTorpedoExplode.rewards.Count; i++) {
			EnemyBase tempEnemy = UIFishingObjects.GetInstance ().fishPool.Get (otherTorpedoExplode.rewards [i].groupId,
				                      otherTorpedoExplode.rewards [i].fishId);
			if (tempEnemy != null) {
				tempEnemy.Hitted (otherTorpedoExplode.userId, true, otherTorpedoExplode.rewards [i].properties, true);
			}
		}

	}


	//---------------------------FishingPK-----------------------------//

	//	public class FiPkGameCountDownInform
	//	{
	//		public int countdown;
	//	}
	public void ToPKPreGameCountdown (FiPkGameCountDownInform countdown)
	{//游戏开始前的倒计时
		//Debug.LogError("countDown:"+countdown.countdown);
		//Debug.LogError("游戏开始倒计时间:"+countdown.countdown+ "Time.time:"+Time.time);
		if (countdown.countdown > 0) {
			if (PrefabManager._instance != null)
				PrefabManager._instance.CreateCountDownEffect ();
           
			if (countdown.countdown == 2) {
				if (GameController._instance == null || GameController._instance.myGameType == GameType.Point)
					return;
				if (PrefabManager._instance != null)
					PrefabManager._instance.ShowPropHintPanel ();
				else
					Debug.LogError ("Error!PrefabManager=null");
			}
		}
	}

	public void ToPKGameCountdown (FiPkGameCountDownInform countdown)
	{//游戏倒计时
		if (TopRoomInfo._instance != null)
			TopRoomInfo._instance.SetTime (countdown.countdown);
		//Debug.LogError("游戏内倒计时:"+ countdown.countdown);
	}
	//	public class FiDistributePKProperty
	//	{
	//		public int roomIndex;
	//		public List<FiProperty> properties;
	//	}
	public FiDistributePKProperty tempDistribute = null;

	public void SetPKDistributePropertyInfo (FiDistributePKProperty distribute)
	{//设置道具信息
		//Type:GOLD = 1, DIAMOND = 2, EXP(经验) = 3, FREEZE = 4, AIM(锁定) = 5, CALL(召唤) = 6,
		tempDistribute = distribute;
	}
	//	public class FiLaunchTorpedoResponse
	//	{
	//		public int  result;
	//		public int  torpedoId;
	//		public int  torpedoType;
	//		public Cordinate position;
	//	}
	public void ToPKLaunchTorpedo (FiLaunchTorpedoResponse launchTorpedo)
	{//FishingPK 发射鱼雷
		if (0 == launchTorpedo.result)
		{
			Debug.LogError("收到自己發送魚雷成功");
		}
		else
		{
			Debug.LogError("收到自己發送魚雷失敗，result=" + launchTorpedo.result);
		}
	}
	//	public class  FiOtherLaunchTorpedoInform
	//	{
	//		public int userId;
	//		public int torpedoId;
	//		public int torpedoType;
	//		public Cordinate position;
	//	}
	public void ToPKOtherLaunchTorpedo (FiOtherLaunchTorpedoInform otherLaunchTorpedo)
	{//FishingPK 其他玩家发射鱼雷
		int level = Skill.GetTorpedoLevelFromServerId (otherLaunchTorpedo.torpedoType);
		Debug.LogError("收到其他玩家發射魚雷: level=" + level + ",userId=" + otherLaunchTorpedo.userId);
		GunControl otherGun = UIFishingObjects.GetInstance ().cannonManage.GetInfo (otherLaunchTorpedo.userId).cannon;

		Vector3 tempPos = new Vector3 (otherLaunchTorpedo.position.x, otherLaunchTorpedo.position.y, 0);

		otherGun.FireOneTorpedo (tempPos, level);
	}
	//	public class  FiTorpedoExplodeResponse
	//	{
	//		public int     result;
	//		public int     torpedoId;
	//		public int     torpedoType;
	//		public List<FiFishReward>  rewards;
	//	}
	public void ToPKTorpedoExplode (FiTorpedoExplodeResponse torpedoExplode)
	{//FishingPK 触发鱼雷爆炸
		if (0 == torpedoExplode.result) {
			Debug.LogError("接收自己魚雷爆炸成功");
			for (int i = 0; i < torpedoExplode.rewards.Count; i++) {
				EnemyBase tempEnemy = UIFishingObjects.GetInstance ().fishPool.Get (torpedoExplode.rewards [i].groupId,
					                      torpedoExplode.rewards [i].fishId);
				if (tempEnemy != null) {
					tempEnemy.Hitted (myInfo.userID, true, torpedoExplode.rewards [i].properties, true);
					//Debug.LogError("Rewards:"+ torpedoExplode .reward
				}
			}
		} else {
			Debug.LogError ("Error! torpedoExplode.result=" + torpedoExplode.result);
		}
	}
	//	public class  FiOtherTorpedoExplodeInform
	//	{
	//		public int      userId;
	//		public int      torpedoId;
	//		public int      torpedoType;
	//		public List<FiFishReward> rewards;
	//	}
	public void ToPKOtherTorpedoExplode (FiOtherTorpedoExplodeInform otherTorpedoExplode)
	{//FishingPK 触发其他玩家的鱼雷爆炸
		for (int i = 0; i < otherTorpedoExplode.rewards.Count; i++) {
			EnemyBase tempEnemy = UIFishingObjects.GetInstance ().fishPool.Get (otherTorpedoExplode.rewards [i].groupId,
				                      otherTorpedoExplode.rewards [i].fishId);
			if (tempEnemy != null) {
				tempEnemy.Hitted (otherTorpedoExplode.userId, true, otherTorpedoExplode.rewards [i].properties, true);
			}
		}
		Debug.LogError("觸發其它玩家魚雷爆炸通知:userId=" + otherTorpedoExplode.userId + " count=" + otherTorpedoExplode.rewards.Count);
	}

	/// <summary>
	/// 赋值金币
	/// </summary>
	/// <param name="changeUserGold">Change user gold.</param>
	public void ToChangeUserGold (FiChangeUserGold changeUserGold)
	{
		switch (changeUserGold.propertyID) {
		case FiPropertyType.GOLD: //加金币
			if (PrefabManager._instance != null) {
				PrefabManager._instance.GetGunByUserID (changeUserGold.userID).gunUI.RefrshCoin (0, changeUserGold.count);	
			}
			break;
		case FiPropertyType.DIAMOND: //加钻石
			if (PrefabManager._instance != null) {
				PrefabManager._instance.GetGunByUserID (changeUserGold.userID).gunUI.AddValue (0, 0, changeUserGold.count);	
			}
			break;
		default:
			break;
		}
	}

	/// <summary>
	/// 赋值金币
	/// </summary>
	/// <param name="changeUserGold">Change user gold.</param>
	public void ToUpdateBossMatch (FiUpdateBossMatchTime _matchTime)
	{
//		UnityEngine.Debug.LogError ("ToUpdateBossMatch _matchTime.chaTime = " + _matchTime.chaTime);
		if (UpdateBossMatchTimeScript.Instance != null) {
			UpdateBossMatchTimeScript.Instance.UpdateTime (_matchTime.chaTime);
		} 
	}
}
