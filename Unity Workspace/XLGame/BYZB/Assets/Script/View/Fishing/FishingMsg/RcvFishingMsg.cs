/* author:KinSen
 * Date:2017.03.21
 */

using UnityEngine;
using System.Collections;
using System.Collections.Generic;

using AssemblyCSharp;

//负责：各类捕鱼房间通用消息的处理，非通用信息各自写入对应的RcvFishingMsg
//该类用于继承，所有消息函数设置public权限
/*
 *  2020/03/13 Joey 新場景解鎖福利彈出視窗，分別是炮倍數解鎖50、1000時彈出
 */
public class RcvFishingMsg
{
	protected AppControl appControl = null;
	protected DispatchControl dispatchControl = null;
	protected DataControl dataControl = null;
	protected MyInfo myInfo = null;
	protected RoomInfo roomInfo = null;

	protected UIFishingObjects fishingObjects = null;

	//	public CannonManage cannonManage = null;
	//	public BulletPool bulletPool = null;
	//	public FishPool fishPool = null;


	protected RcvFishingMsg ()
	{//构造函数protected，不能自行创建该类实例
		Init ();
	}

	~RcvFishingMsg ()
	{
		UnInit ();
	}

	void Init ()
	{
		appControl = AppControl.GetInstance ();
		dataControl = DataControl.GetInstance ();
		myInfo = dataControl.GetMyInfo ();
		roomInfo = dataControl.GetRoomInfo ();

		fishingObjects = UIFishingObjects.GetInstance ();

		//		cannonManage = new CannonManage();
		//		bulletPool = new BulletPool();
		//		fishPool = new FishPool();

	}

	void UnInit ()
	{
		Clear ();
		appControl = null;
		dataControl = null;
		myInfo = null;
		roomInfo = null;
	}

	void Clear ()
	{
		if (null != myInfo) {
			myInfo.SetCannonInfo (null);
		}
	}



	public void RcvFishCreateInfo (object data)
	{//创建鱼
		//Tool.OutLogWithToFile ("开始创建鱼");
		FiFishsCreatedInform info = (FiFishsCreatedInform)data;
		fishingObjects.CreateFish (info);
	}


	public void SndFishOut (int groupId, int fishId = 0)
	{//发送鱼群游出屏幕

		//List<EnemyBase> listFish = fishPool.GetGroupFish (groupId,false);
		List<EnemyBase> listFish = fishingObjects.GetGroupFish (groupId);
		if (null != listFish) {
			if (listFish.Count > 1) {//如果组里还有除这条鱼外的其它鱼，不发送游出屏幕外的消息
				return;
			}
		}

		//		Debug.LogError ("SndFishOutGroup:" + groupId + "," + fishId);
		FiFishsOutRequest fishOut = new FiFishsOutRequest ();
		fishOut.groupId = groupId;

		Tool.OutLogWithToFile ("SndFishOut groupId:" + groupId);

		dataControl.PushSocketSnd (FiEventType.SEND_FISH_OUT_REQUEST, fishOut);
	}

	public void RcvFishOutResponse (object data)
	{//接收鱼群游出屏幕
		FiFishsOutResponse fishOut = (FiFishsOutResponse)data;
		//		fishPool.RemoveGroupFish (fishOut.groupId);
		//
		//		Tool.OutLogWithToFile ("RcvFishOutResponse groupId:"+fishOut.groupId);
		fishingObjects.ToFishOut (fishOut.groupId);

	}


	public void RcvOtherEnterRoom (object data)
	{//其他用户进房间
		FiOtherEnterRoom info = (FiOtherEnterRoom)data;
		FiUserInfo user = info.user;

		Tool.OutLogWithToFile ("其他用戶進房間:" + user.userId);

		if (null != user) {
			//			CannonInfo cannonInfo = new CannonInfo ();
			//			cannonInfo.userId = user.userId;
			//			cannonInfo.seatIndex = user.seatIndex;
			//			cannonInfo.cannonMultiple = user.cannonMultiple;
			//			cannonInfo.userInfo = new FiUserInfo (user);
			//			cannonInfo.cannon = cannonManage.CreateCannon (info.user);
			//
			//			cannonManage.RemoveCannon (user.userId);
			//			cannonManage.RemoveInfo (user.userId);
			//			cannonManage.AddInfo (cannonInfo);
			//			cannonManage.CreateCannon (cannonInfo.userInfo);
			fishingObjects.CreateCannon (user);
		}

	}

	public void RcvOtherLeaveRoom (object data)
	{//其他用户离开房间
		FiOtherLeaveRoom info = (FiOtherLeaveRoom)data;

		//		cannonManage.RemoveCannon(info.userId);
		//		cannonManage.RemoveInfo (info.userId);
		fishingObjects.RemoveCannon (info.userId);

		Tool.OutLogWithToFile ("其他用戶離開房間:" + info.userId);

	}

	/*public void SndLeaveRoom()
	{//发送自己离开房间
		if(null!=roomInfo)
		{
			FiLeaveRoomRequest leaveRoom = new FiLeaveRoomRequest();
			leaveRoom.leaveType = roomInfo.roomType;
			leaveRoom.roomIndex = roomInfo.roomIndex;
			leaveRoom.roomMultiple = roomInfo.roomMultiple;

			dataControl.PushSocketSnd (FiEventType.SEND_USER_LEAVE_REQUEST, leaveRoom);
			Tool.OutLogWithToFile ("发送自己离开房间");
		}

	}*/




	public void RcvUserLeaveRoom (object data)
	{//接收自己离开房间
		FiLeaveRoomResponse fiLeave = (FiLeaveRoomResponse)data;
//		Debug.LogError ("RcvUserLeaveRoom fiLeave.result = " + fiLeave.result);
		if (fiLeave.result != 0) {
			UnityEngine.GameObject Window1 = UnityEngine.Resources.Load ("Window/WindowTipsThree")as UnityEngine.GameObject;
			UnityEngine.GameObject WindowClone1 = UnityEngine.GameObject.Instantiate (Window1);
			UITipAutoNoMask ClickTips1 = WindowClone1.GetComponent<UITipAutoNoMask> ();
			ClickTips1.tipText.text = "離開房間失敗，錯誤碼:" + fiLeave.result;
		} else {
			MyInfo nInfo = (MyInfo)Facade.GetFacade ().data.Get (FacadeConfig.USERINFO_MODULE_ID);
			nInfo.gold = fiLeave.gold;
			if (Facade.GetFacade ().ui.Get (FacadeConfig.UI_STORE_MODULE_ID) != null) {
				Facade.GetFacade ().ui.Get (FacadeConfig.UI_STORE_MODULE_ID).OnRecvData (FiPropertyType.GOLD, nInfo.gold);
			}
//			Debug.LogError ("-----------------nInfo.gold--------------------" + nInfo.gold);

			DelayLeave ();
			AppControl.ToView (AppView.HALL);
		}
		Tool.OutLogWithToFile ("接收自己離開房間:" + fiLeave.result);
		Clear ();
	}

	void DelayLeave ()
	{
		//BossMatchScript.beginType = 0;
		ReturnRobot ();
		ChatDataInfo nInfo = (ChatDataInfo)Facade.GetFacade ().data.Get (FacadeConfig.CHAT_MODULE_ID);
		AudioManager._instance.PlayEffectClip (AudioManager.effect_closePanel);
		nInfo.ClearChatMsg ();
	}

	void ReturnRobot ()
	{
		// Debug.LogError("LocalLeave");
		if (PrefabManager._instance != null) {

			for (int i = 0; i < PrefabManager._instance.gunGroup.Length; i++) {
				GunControl tempGun = PrefabManager._instance.gunGroup [i].GetComponent<GunControl> ();
				if (tempGun != null) {
					if (tempGun.isRobot) {

						tempGun.RobotSendLeaveSelf ();
					}

				}
			}	
		}
	}
	/*public void SndFireBullet(int bulletId, float x, float y,int groupId=-1,int fishId=-1)
	{//自己发送子弹
		//Debug.Log ("发送子弹ID:"+bulletId);
		//Tool.OutLogWithToFile ("自己发送子弹 bulletId:"+bulletId);
		FiFireBulletRequest fireBullet = new FiFireBulletRequest ();
		fireBullet.userId = myInfo.userID;
		fireBullet.cannonMultiple = myInfo.cannonMultipleNow;
		fireBullet.bulletId = bulletId;
		fireBullet.position = new Cordinate ();
		fireBullet.position.x = x;
		fireBullet.position.y = y;
		fireBullet.groupId = groupId;
		fireBullet.fishId = fishId;
		dataControl.PushSocketSnd (FiEventType.SEND_FIRE_BULLET_REQUEST, fireBullet);
	}*/

	public void RcvOtherFireBullet (object data)
	{//接收别人发送的子弹信息
		FiFireBulletRequest info = (FiFireBulletRequest)data;
		//Tool.OutLogWithToFile ("接收别人发送的子弹");

		//如果子弹的userId是自己，不进行处理
		if (info.userId == myInfo.userID) {
//			if (PrefabManager._instance != null) {
//				if (PrefabManager._instance.GetLocalGun ().isBerserk)
//					PrefabManager._instance.GetLocalGun ().gunUI.ChangeGoldNum (-2 * info.cannonMultiple);
//				else
//					PrefabManager._instance.GetLocalGun ().gunUI.ChangeGoldNum (-info.cannonMultiple);
//			}
			return;
		}


		fishingObjects.ToFireBullet (info);

		//		CannonInfo cannonInfo = cannonManage.GetInfo(info.userId);
		//		Tool.OutLogWithToFile ("查找该用户炮的信息 userId:"+info.userId);
		//		if(null!=cannonInfo)
		//		{
		//			if (null != cannonInfo.cannon) {
		//				Tool.OutLogWithToFile ("找到该用户的炮 发射子弹 userId:"+info.userId+" bulletId:"+info.bulletId);
		//				cannonInfo.cannon.FireToPos (info.bulletId, info.cannonMultiple, info.position.x, info.position.y, info.groupId, info.fishId);
		//			} else {
		//				Tool.OutLogWithToFile ("找不到该用户的炮 userId:"+info.userId);
		//			}
		//		}
		//		else
		//		{
		//			Tool.OutLogWithToFile ("找不到该用户炮的信息 userId:"+info.userId);
		//		}

	}

	public void SndHitFish (int userId, int groupId, int fishId, int bulletId, float x, float y)
	{//发送子弹打到鱼请求
		if (userId != myInfo.userID)
			return;
		//PrefabManager._instance.ShowNetEffect (new Vector3 (x,y,0));
		FiHitFishRequest hitFish = new FiHitFishRequest ();
		hitFish.groupId = groupId;
		hitFish.fishId = fishId;
		hitFish.userId = userId;
		hitFish.bulletId = bulletId;
		hitFish.cannonMultiple = myInfo.cannonMultipleNow;
		hitFish.position = new Cordinate ();
		hitFish.position.x = x;
		hitFish.position.y = y;

		dataControl.PushSocketSnd (FiEventType.SEND_HITTED_FISH_REQUEST, hitFish);

		//Tool.OutLogWithToFile ("发送子弹打到鱼请求 userId:"+myInfo.userID+" bulletId:"+bulletId);
	}

	/// <summary>
	/// 天横财降专用（用的是打中鱼的结构）
	/// </summary>
	/// <param name="data">Data.</param>
	public void RveHieFish_RedPack_Response (object data)
	{
		FiHitFishResponse info = (FiHitFishResponse)data;

		fishingObjects.ToHitFish_RedPack (info);
	}

	public void RcvHitFishResponse (object data)
	{//子弹打到鱼
		FiHitFishResponse info = (FiHitFishResponse)data;

		//Tool.OutLogWithToFile ("接收别人打到鱼 userId:"+info.userId+" bulletId:"+info.bulletId);
		fishingObjects.ToHitFish (info);

		/*
		//先找到对应bulletId的子弹
		Bullet bullet = bulletPool.GetBullet (info.userId, info.bulletId);
		if(null!=bullet)
		{//在对应的坐标位子爆炸渔网
			bullet.HitFish ();
			bulletPool.Remove (info.bulletId, info.userId);
			Tool.OutLogWithToFile ("找到对应的子弹在对应的坐标位子爆炸渔网 userId:"+info.userId+" bulletId:"+info.bulletId);
		}
		else
		{
			Tool.OutLogWithToFile ("找不到对应的子弹在对应的坐标位子爆炸渔网 userId:"+info.userId+" bulletId:"+info.bulletId);
		}

		bool shouldDead = false;
		if(null!=info.propertyArray)
		{
			if(0!=info.propertyArray.Count)
			{
				EnemyBase enemy = fishPool.Get (info.groupId, info.fishId);
				if(null!=enemy)
				{
					shouldDead = true;
					Debug.Log ("成功捕到鱼 1 groupId:"+info.groupId + " fhisId"+info.fishId);

					enemy.Hitted (info.userId, shouldDead, info.propertyArray);
					Debug.Log ("成功捕到鱼 2 groupId:"+info.groupId + " fhisId"+info.fishId);
					FishReward.OpenReward (info.userId, new Cordinate (enemy.transform.position.x,enemy.transform.position.y), info.propertyArray);
					Tool.OutLogWithToFile ("找到鱼 成功捕到鱼 groupId:"+info.groupId + " fhisId"+info.fishId);
				}
				else
				{
					Tool.OutLogWithToFile ("找不到鱼 成功捕到鱼 groupId:"+info.groupId + " fhisId"+info.fishId);
					FishReward.OpenReward (info.userId, info.position, info.propertyArray);
				}

				//获取掉落
				foreach(FiProperty one in info.propertyArray)
				{
					//GOLD = 1, DIAMOND = 2, EXP(经验) = 3, FREEZE = 4, AIM(锁定) = 5, CALL(召唤) = 6,
					switch(one.type)
					{
					case FiPropertyType.GOLD:
						Tool.Log ("掉落 获得金币");
						break;
					case FiPropertyType.DIAMOND:
						Tool.Log ("掉落 获得钻石");
						break;
					case FiPropertyType.EXP:
						Tool.Log ("掉落 获得经验");
						break;
					case FiPropertyType.FREEZE:
						Tool.Log ("掉落 获得冰冻");
						break;
					case FiPropertyType.AIM:
						Tool.Log ("掉落 获得瞄准");
						break;
					case FiPropertyType.SUMMON:
						Tool.Log ("掉落 获得召唤");
						break;
					}
				}
			}
		}
		else
		{
			Tool.OutLogWithToFile ("RcvHitFishResponse null==info.propertyArray");
		}//*/

	}

	public void SndChangeCannonMultiple (int cannonMultiple)
	{//发送改变炮倍数
		FiChangeCannonMultipleRequest changeCannon = new FiChangeCannonMultipleRequest ();
		changeCannon.cannonMultiple = cannonMultiple;
		dataControl.PushSocketSnd (FiEventType.SEND_CHANGE_CANNON_REQUEST, changeCannon);

		Tool.OutLogWithToFile ("發送改變炮倍數 multiple:" + cannonMultiple);
	}

	public void RcvChangeCannonMultiple (object data)
	{//接收改变炮倍数
		FiChangeCannonMultipleResponse changeCannon = (FiChangeCannonMultipleResponse)data;
		int multiple = changeCannon.cannonMultiple;
		if (0 == changeCannon.result) {//改变炮倍数成功
			myInfo.cannonMultipleNow = multiple;
			//			CannonInfo info = myInfo.GetCannonInfo ();
			//			if(null!=info)
			//			{
			//				if(null!=info.cannon)
			//				{
			//					info.cannon.gunUI.SetMultiple (multiple,false);//接受自己改变炮倍数时，不需要再循环上传
			//				}
			//			}
			fishingObjects.ChangeCannonMultiple (myInfo.userID, multiple);
		} else {//改变炮倍数失败
			Tool.OutLogWithToFile ("接收改變炮倍數 multiple:" + multiple);
		}

	}

	public void RcvOtherChangeCannonMultiple (object data)
	{//接收其他玩家改变炮倍数
		FiOtherChangeCannonMultipleInform otherChangeCannon = (FiOtherChangeCannonMultipleInform)data;
		fishingObjects.ChangeCannonMultiple (otherChangeCannon.userId, otherChangeCannon.cannonMultiple);
		//CannonInfo info = cannonManage.GetInfo (otherChangeCannon.userId);

		//		Tool.OutLogWithToFile ("接收其他玩家改变炮倍数 multiple:"+otherChangeCannon.cannonMultiple);
		//		if (null != info) {
		//			if (null != info.cannon) {
		//				info.cannon.gunUI.SetMultiple (otherChangeCannon.cannonMultiple, false);
		//			}
		//		} else {
		//			Tool.OutLogWithToFile ("接收其他玩家改变炮倍数 找不到该玩家的炮信息 userId:"+otherChangeCannon.userId);
		//		}
	}

	public void SndEffectRequest (int effectId)
	{//发送特效请求
		FiEffectRequest effectInfo = new FiEffectRequest ();
		effectInfo.userId = myInfo.userID;
		effectInfo.effect = new FiEffectInfo ();
		effectInfo.effect.type = effectId;
		dataControl.PushSocketSnd (FiEventType.SEND_USE_EFFECT_REQUEST, effectInfo);
		//Debug.LogError ("SendEffectId:" + effectId);

		Tool.OutLogWithToFile ("發送特效請求 effectId:" + effectId);
	}

	public void RcvUnlockCannonResponse (object data)
	{
        //接收協議 10028
		FiUnlockCannonResponse nResponse = (FiUnlockCannonResponse)data;
		if (nResponse.result == 0) {
			//Debug.LogError ("Rcv: CurrentMax=" + nResponse.currentMaxMultiple + " Gold=" + nResponse.rewardGold);
			myInfo.cannonMultipleMax = nResponse.currentMaxMultiple;
			myInfo.cannonMultipleNow = nResponse.currentMaxMultiple;
			Panel_UnlockMultiples._instance.RcvUnlockInfo (nResponse.currentMaxMultiple, nResponse.rewardGold, nResponse.needDiamond);

			//如果领取起航礼包的第一天，并且解锁到5倍炮了，显示起航礼包倒计时图标
			if (nResponse.currentMaxMultiple == 5 && myInfo.sailDay == -1 && Panel_UnlockMultiples._instance != null && Panel_UnlockMultiples._instance.gameObject.activeSelf) {
				GameObject nWindow = StartGiftManager.OpenCountDownWindow (2);
				UITomorrow nScript = nWindow.GetComponentInChildren<UITomorrow> ();
				nScript.RegisterCloseCallBack (() => {
					MyInfo nInfo = (MyInfo)Facade.GetFacade ().data.Get (FacadeConfig.USERINFO_MODULE_ID);
					if (nInfo.lastGame.type == TypeFishing.CLASSIC) {
						if (nInfo.platformType == 22 || nInfo.platformType == 24) {
							StartGiftManager.OpenCountDownBoxTip (Panel_UnlockMultiples._instance.transform.parent.parent, false, true);
						}
					}
				});
			}

            //2020/03/13 Joey 新場景解鎖福利彈出視窗，分別是炮倍數解鎖50、1000時彈出
			if (nResponse.currentMaxMultiple == 50)
			{
				GameObject window = Resources.Load("Window/NewSceneUnlock300") as GameObject;
				GameObject.Instantiate(window);
			}
			else if (nResponse.currentMaxMultiple == 1000)
			{
				GameObject window = Resources.Load("Window/NewSceneUnlock500") as GameObject;
				GameObject.Instantiate(window);
			}
		} else {
			//HintText._instance.ShowHint ("解锁炮倍数失败，错误码:" + nResponse.result);
			Debug.LogError ("解鎖炮倍數失敗，錯誤碼:" + nResponse.result);
			Panel_UnlockMultiples._instance.SetShow (false);
		}
	}

	public void RcvEffectResponse (object data)
	{//接收自己特效
		FiEffectResponse effectInfo = (FiEffectResponse)data;

		FiEffectInfo info = effectInfo.info; 
		if (null == info)
			return;
		if (0 == effectInfo.result) {
			fishingObjects.ToEffect (info);
			/*
			Tool.OutLogWithToFile ("接收自己特效 effectType:"+info.type);

			switch (info.type) {//1冰冻 2锁定 3狂暴 4召唤
			case FiPropertyType.FREEZE://冰冻
				List<EnemyBase > tempGroup = new List<EnemyBase> ();
				if (info.value.Count > 0) { //info.value为int类型的List，代表鱼的GroupId
					foreach (int index in info.value) {
						List<EnemyBase > listGet = fishPool.GetGroupFish (index);
						if(null!=listGet)
						{
							tempGroup.AddRange (listGet);
						}
					}
				}
				PrefabManager._instance.GetSkillUIByType (SkillType.Freeze).ActiveSkill (tempGroup);//在skill类里执行冰冻鱼的操作
				break;
			case FiPropertyType.AIM://锁定
				PrefabManager._instance.GetSkillUIByType (SkillType.Lock).RepareToUseSkill ();
				break;
			case FiPropertyType.FURIOUS://狂暴
				PrefabManager._instance.GetSkillUIByType (SkillType.Berserk).ActiveSkill ();
				break;
			case FiPropertyType.SUMMON://召唤，召唤阵的特效在鱼生成的时候，根据自己的轨迹是否属于召唤轨迹而自行决定，所以这里只是激活技能图标的cd效果，并无其它功能
				PrefabManager._instance.GetSkillUIByType (SkillType.Summon).ActiveSkill ();//
				break;
			case FiPropertyType.BOMB_LEVEL_1:
				Debug.LogError ("Rcv Bomb1");
				break;
			default:
				break;
			}//*/


		} else {

			if (effectInfo.result == 40004)
			{
				HintTextPanel._instance.SetTextShow("魚箱中的魚已經滿了，請稍後使用召喚", 2f);
			}
			else if (effectInfo.result == -1000)
			{
				HintTextPanel._instance.SetTextShow("今日魚雷使用已達上限", 2f);
			}
			else
			{
				//HintText._instance.ShowHint ("道具使用失敗，道具編號:" + info.type + " 錯誤結果:" + effectInfo.result);
			}
		}

	}

	public void RcvOtherEffectResponse (object data)
	{//接收其他玩家特效
		FiOtherEffectInform otherEffectInfo = (FiOtherEffectInform)data;
		//otherEffectInfo.userId
		FiEffectInfo info = otherEffectInfo.info;
		if (null == info)
			return;

		fishingObjects.ToEffect (info, otherEffectInfo.userId);

		/*
		Tool.OutLogWithToFile ("接收其他玩家特效 effectType:"+info.type);
		switch (info.type) { //1冰冻 2锁定 3狂暴 4召唤
		case FiPropertyType.FREEZE://冰冻
			if(info.value.Count>0)
			{
				List<EnemyBase > tempGroup = new List<EnemyBase> ();
				foreach(int index in info.value) 
				{
					List<EnemyBase > listGet = fishPool.GetGroupFish (index);
					if(null!=listGet)
					{
						tempGroup.AddRange (listGet);
					}

				}
				PrefabManager._instance.GetSkillUIByType(SkillType.Freeze).FreezeEnemy(tempGroup);//因为是接收其他玩家的特效，所以直接冻鱼，不需要改变技能自己的技能图标cd	
			}
			break;
		case FiPropertyType.AIM://锁定
			//目前只要a玩家使用锁定技能后，会给发射的子弹赋予锁定属性，b玩家接收到a玩家的子弹后自然而然会看到锁定效果，不需要在技能这里做同步
			break;
		case FiPropertyType.FURIOUS://狂暴
			Skill berserk = PrefabManager._instance.GetSkillUIByType (SkillType.Berserk);
			cannonManage.GetInfo (otherEffectInfo.userId).cannon.UseSkill (berserk.skillType, berserk.skillDuration);
			break;
		case FiPropertyType.SUMMON://召唤，自己接收到他人的召唤请求时不需要做任何处理，召唤的特效已经被绑定在鱼上面了
			break;
		default:
			break;
		}//*/

	}

	public void RcvFishFreezeTimeOutResponse (object data)
	{//接收冰冻时间超时消息
		//Debug.LogError("接收冰冻超时信息");
		FiFreezeTimeOutInform freezeTimeOut = (FiFreezeTimeOutInform)data;
		fishingObjects.FreezeTimeOut (freezeTimeOut);
		/*
		List<EnemyBase > tempGroup = new List<EnemyBase> ();
		foreach (int index in freezeTimeOut.value) {
			List<EnemyBase > listGet = fishPool.GetGroupFish (index);
			if(null!=listGet)
			{
				tempGroup.AddRange (listGet);
				//Debug.LogError ("冰冻超时:" + index);
			}

		}
		//freezeTimeOut.value; //冻住鱼的groupId
		Tool.OutLogWithToFile ("接收冰冻时间超时消息");

		PrefabManager._instance.GetSkillUIByType (SkillType.Freeze).ThawEnemy (tempGroup);
		//*/
	}

	/// <summary>
	/// 金币赋值操作
	/// </summary>
	/// <param name="data">Data.</param>
	public void RcvChangeUserGold (object data)
	{
		FiChangeUserGold changeUserGold = (FiChangeUserGold)data;
		fishingObjects.ToChangeUserGold (changeUserGold);
	}

	/// <summary>
	/// 刷新Boss匹配场时间
	/// </summary>
	/// <param name="data">Data.</param>
	public void RcvUpdateBossMatchTime (object data)
	{
		FiUpdateBossMatchTime matchTime = (FiUpdateBossMatchTime)data;
		fishingObjects.ToUpdateBossMatch (matchTime);
	}
}
