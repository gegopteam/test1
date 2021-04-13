using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Bullet : MonoBehaviour
{

	public enum BulletType
	{
		Normal,
		Lock,
		//激光炮
		LaserLight,
		//闪电链
		LightningChain,
	}

	public static int bulletSumNum = 0;
	List<EnemyBase> enemyHitSumList = new List<EnemyBase> ();
	List<EnemyBase> enemyHitSanDianSumList = new List<EnemyBase> ();
	//统计屏幕中子弹总数
	//基础属性
	public int bulletID = 0;
	public int userID = 0;
	public int cannonMultiple = 0;
	public int currentGunStyle = 0;
	bool isLocal;
	public bool isMyRobot = false;
	RaycastHit hitinfo;

	public float moveSpeed = 2f;
	public BulletType thisType = BulletType.Normal;
	int enemyLayer;
	bool hasHit = false;
	Vector3 moveDir = Vector3.zero;
	Vector3 currentPos;

	//组件获取
	[HideInInspector]public EnemyBase lockEnemy = null;

	public GameObject netEffect;
	//鱼网特效
	Transform topPoint;

	bool isBerserkBullet = false;
	//public Sprite [] bulletSpriteGroup;
	//bool hasParticleEffect=false;
	/// <summary>
	/// 房间信息
	/// </summary>
	RoomInfo myRoomInfo = null;
	Material	lineMaterial;

	void Awake ()
	{
		Init ();
		myRoomInfo = DataControl.GetInstance ().GetRoomInfo ();
		lineMaterial = transform.GetComponent <Material> ();
		enemyHitSumList.Clear ();
		enemyHitSanDianSumList.Clear ();
	}

	void Init ()
	{
		topPoint = transform.Find ("TopPoint");
	
		//netEffect = PrefabManager._instance.GetPrefabObj (MyPrefabType.NetEffect);
		enemyLayer = GameController.enemyLayer;
		moveDir = transform.up;
	}


	void FixedUpdate ()
	{
//		Debug.LogError ("enemyHitSumList.count1 = " + enemyHitSumList.Count);
		if (hasHit)
			return;
		JudgeBulletType ();
	}

	void JudgeBulletType ()
	{
		switch (thisType) {
		case BulletType.Normal://普通子弹
			currentPos = transform.position;
			if (!ScreenManager.IsInScreen (currentPos)) {
				if (currentPos.x < ScreenManager.leftBorder && moveDir.x < 0 || currentPos.x > ScreenManager.rightBorder && moveDir.x > 0)
					moveDir = new Vector3 (-moveDir.x, moveDir.y, 0);
				else if (currentPos.y < ScreenManager.downBorder && moveDir.y < 0 || currentPos.y > ScreenManager.topBorder && moveDir.y > 0)
					moveDir = new Vector3 (moveDir.x, -moveDir.y, 0);
				SetRotation (moveDir);
			}
			transform.Translate (moveDir * moveSpeed * Time.deltaTime, Space.World); 
			RaycastCheck (); 
			break;

		case BulletType.Lock://锁定子弹
			if (null == lockEnemy) {
				thisType = BulletType.Normal;
				return;
			} else if (lockEnemy.thisState == EnemyBase.EnemyState.Dead) {
				thisType = BulletType.Normal;
				lockEnemy = null;
				return;
			}	

			Vector3 planePos = GetPlanePos (lockEnemy.transform.position);
			float distance = Vector3.Distance (transform.position, planePos);
			if (distance < 1f) { //一开始赋值的时候是GameObject本体，后来重新选取的目标其实是Enemy下的ChildModel子物体，需要去父物体获取
				lockEnemy.Hitted (userID);
			
				HitFish ();
				UIFishingMsg.GetInstance ().SndHitFish (userID, lockEnemy.groupID, lockEnemy.id, bulletID, transform.position.x, transform.position.y, 
					this.cannonMultiple, isBerserkBullet, lockEnemy.typeID, isMyRobot);
				//UIFishingMsg.GetInstance().bulletPool.Remove (bulletID, userID);
				UIFishingObjects.GetInstance ().bulletPool.Remove (bulletID, userID);
				return;
			}
			moveDir = (planePos - transform.position).normalized;
			SetRotation (moveDir);
			transform.Translate (moveDir * moveSpeed * Time.deltaTime, Space.World); 
			break;
		//激光炮
		case BulletType.LaserLight:
			{
				currentPos = transform.position;
				RaycastCheckLaserLight ();
				if (!ScreenManager.IsInScreen (currentPos)) {
					if (currentPos.x < ScreenManager.leftBorder && moveDir.x < 0 || currentPos.x > ScreenManager.rightBorder && moveDir.x > 0) {
						
						HitFish ();
						UIFishingObjects.GetInstance ().bulletPool.Remove (bulletID, userID);
					} else if (currentPos.y < ScreenManager.downBorder && moveDir.y < 0 || currentPos.y > ScreenManager.topBorder && moveDir.y > 0) {
						
						HitFish ();
						UIFishingObjects.GetInstance ().bulletPool.Remove (bulletID, userID);
					}
				}
				transform.Translate (moveDir * moveSpeed * Time.deltaTime, Space.World); 
			}
			break;
		case BulletType.LightningChain:
			{
				currentPos = transform.position;
				if (!ScreenManager.IsInScreen (currentPos)) {
					if (currentPos.x < ScreenManager.leftBorder && moveDir.x < 0 || currentPos.x > ScreenManager.rightBorder && moveDir.x > 0)
						moveDir = new Vector3 (-moveDir.x, moveDir.y, 0);
					else if (currentPos.y < ScreenManager.downBorder && moveDir.y < 0 || currentPos.y > ScreenManager.topBorder && moveDir.y > 0)
						moveDir = new Vector3 (moveDir.x, -moveDir.y, 0);
					SetRotation (moveDir);
				}
				transform.Translate (moveDir * moveSpeed * Time.deltaTime, Space.World); 
				RaycastCheckLightningChain (); 
			}
			break;
		}
	}


	//	void RaycastCheckAll () //改用RaycastCheck提升性能
	//	{
	//		if (hasHit)
	//			return;
	//		RaycastHit[] hitInfos = Physics.RaycastAll (topPoint.position, Vector3.forward, 1000f, enemyLayer);
	//		if (hitInfos.Length > 0) {
	//			EnemyBase enemyBase = hitInfos [0].collider.transform.parent.GetComponent<EnemyBase> ();
	//			enemyBase.Hitted (userID);
	//			HitFish ();
	//			UIFishingMsg.GetInstance ().SndHitFish (userID, enemyBase.groupID, enemyBase.id, bulletID, transform.position.x, transform.position.y,
	//				this.cannonMultiple, isBerserkBullet, enemyBase.typeID, isMyRobot);
	//			UIFishingObjects.GetInstance ().bulletPool.Remove (bulletID, userID);
	//		}
	//	}

	void RaycastCheck ()
	{
		if (hasHit)
			return;
		if (Physics.Raycast (topPoint.position, Vector3.forward, out hitinfo, 1000f, enemyLayer)) {
			EnemyBase enemyBase = hitinfo.collider.transform.parent.GetComponent<EnemyBase> ();
			enemyBase.Hitted (userID);
			HitFish ();
			UIFishingMsg.GetInstance ().SndHitFish (userID, enemyBase.groupID, enemyBase.id, bulletID, 
				transform.position.x, transform.position.y,
				this.cannonMultiple, isBerserkBullet, enemyBase.typeID, isMyRobot);
			UIFishingObjects.GetInstance ().bulletPool.Remove (bulletID, userID);
		}
	}

	/// <summary>
	/// 检测穿透炮击中鱼
	/// </summary>
	void RaycastCheckLaserLight ()
	{
		if (hasHit)
			return;
		if (Physics.Raycast (topPoint.position, Vector3.forward, out hitinfo, 1000f, enemyLayer)) {
			EnemyBase enemyBase = hitinfo.collider.transform.parent.GetComponent<EnemyBase> ();
			enemyBase.Hitted (userID);
			if (!enemyHitSumList.Contains (enemyBase)) {
				enemyHitSumList.Add (enemyBase);
//				Debug.LogError ("enemyHitSumList.count = " + enemyHitSumList.Count);
				LaserLightHitFish ();
				UIFishingMsg.GetInstance ().SndHitFish (userID, enemyBase.groupID, enemyBase.id, bulletID, 
					transform.position.x, transform.position.y, this.cannonMultiple, isBerserkBullet, enemyBase.typeID, isMyRobot);
			}
		}
	}

	/// <summary>
	/// 检测闪电链击中鱼
	/// </summary>
	void RaycastCheckLightningChain ()
	{
		if (hasHit)
			return;
		if (Physics.Raycast (topPoint.position, Vector3.forward, out hitinfo, 1000f, enemyLayer)) {
			EnemyBase enemyBase = hitinfo.collider.transform.parent.GetComponent<EnemyBase> ();

			enemyBase.Hitted (userID);
//			if (!enemyHitSumList.Contains (enemyBase)) {
//				enemyHitSumList.Add (enemyBase);
//				Debug.LogError ("enemyHitSumList.count = " + enemyHitSumList.Count);
			LightningChainHitFish (enemyBase);
//			}

			UIFishingObjects.GetInstance ().bulletPool.Remove (bulletID, userID, true);


		}
	}

	void CameraRaycastCheck ()
	{
		Physics.Raycast (Camera.main.ViewportPointToRay (topPoint.position));
	}

	bool isAdd = true;
	int index;

	/// <summary>
	/// 闪电链击中鱼的处理
	/// </summary>
	void LightningChainHitFish (EnemyBase _enemyBase)
	{
//		hasHit = true;
		List<int> dic = new List<int> ();
		//获取鱼池总数量
		List <EnemyBase> enemyListAllCount = UIFishingObjects.GetInstance ().fishPool.listEnemyBase;
		//单独做特殊处理的List列表,用来直接判断鱼在屏幕
		List <EnemyBase> enemyBaseList = new List<EnemyBase> ();
		for (int i = 0; i < enemyListAllCount.Count; i++) {
			//判断鱼是否在屏幕和是否重复添加列表
			if (!enemyBaseList.Contains (enemyListAllCount [i]) && enemyListAllCount [i].hasInScreen) {
				enemyBaseList.Add (enemyListAllCount [i]);
			}
		}
//		//打印列表对比,不使用时注释掉
//		Debug.LogError ("enemyBaseList.Count = " + enemyBaseList.Count);	
//		Debug.LogError ("enemyListAllCount.Count = " + enemyListAllCount.Count);
		//所要添加的鱼的位置
		List <Transform> enemyTrans = new List<Transform> ();
		if (enemyBaseList != null) {
			//当在屏幕里的鱼超过或者等于5条的时候,就要进行闪电链操作
//			if (enemyBaseList.Count >= 5) {
			for (int i = 0; i < 5; i++) {
//					index = Random.Range (0, enemyBaseList.Count - 1);
//					while (isAdd) {
//						isAdd = false;
//						//随机屏幕内鱼的数量
//						//如果已经添加,或者不在屏幕,就要重新遍历
//						for (int j = 0; j < enemyHitSanDianSumList.Count; j++) {
//							if (enemyHitSanDianSumList [j] == enemyBaseList [index] || !enemyBaseList [index].hasInScreen) {
//								isAdd = true;
//							}
//						}
//					}
				System.Random r = new System.Random ();

		

				while (isAdd) {
					int dicdex = r.Next (0, enemyBaseList.Count);
					if (!dic.Contains (dicdex)) {
						dic.Add (dicdex);
						index = dicdex;

						isAdd = false;
					} else {
						if (enemyBaseList.Count < 5 && dic.Count >= enemyBaseList.Count) {
							isAdd = false;
						} else {
							isAdd = true;
						}

					}
				}

//					index = Random.Range (0, enemyBaseList.Count - 1);
//				Debug.LogError ("enemyHitSanDianSumList" + enemyHitSanDianSumList.Count + "enemyBaseList" + enemyBaseList.Count);
				if (enemyHitSanDianSumList.Count < 5 && !enemyHitSanDianSumList.Contains (enemyBaseList [index])) {
					enemyHitSanDianSumList.Add (enemyBaseList [index]);
					enemyTrans.Add (enemyBaseList [index].GetComponent <Transform> ());
					EnemyBase enemyBase = enemyBaseList [index];
					//发送击中鱼消息
					UIFishingMsg.GetInstance ().SndHitFish (userID, enemyBase.groupID, enemyBase.id, bulletID, 
						transform.position.x, transform.position.y, this.cannonMultiple, isBerserkBullet, enemyBase.typeID, isMyRobot);
					//生成特效
				
					GameObject temp = GameObject.Instantiate (netEffect, enemyBase.transform.position, Quaternion.identity)as GameObject;
					temp.transform.localPosition = new Vector3 (temp.transform.localPosition.x, temp.transform.localPosition.y, 70f);
//					Debug.LogError ("_________________________________________" + temp.name);
					if (i >= 1) {
//						Debug.Log ("-------------------------------------------");
						temp.gameObject.transform.Find ("LineRender").gameObject.SetActive (false);
					}
					//闪电链初始位置
					ChainLightning.Instance.start = _enemyBase.transform;
					//闪电链目标位置
					ChainLightning.Instance.target = enemyTrans;//问题1 会因为鱼毁灭导致transform报空

					Destroy (temp, 1f);
				}
//					//判断是否在屏幕,可以移除
//					if (enemyBaseList [index].hasInScreen) {
//						enemyHitSanDianSumList.Add (enemyBaseList [index]);
//						enemyTrans.Add (enemyBaseList [index].GetComponent <Transform> ());
//						EnemyBase enemyBase = enemyBaseList [index];
//						//发送击中鱼消息
//						UIFishingMsg.GetInstance ().SndHitFish (userID, enemyBase.groupID, enemyBase.id, bulletID, 
//							transform.position.x, transform.position.y, this.cannonMultiple, isBerserkBullet, enemyBase.typeID, isMyRobot);
//						//生成特效
//						GameObject temp = GameObject.Instantiate (netEffect, enemyBase.transform.position, Quaternion.identity)as GameObject;
//						//闪电链初始位置
//						ChainLightning.Instance.start = _enemyBase.transform;
//						//闪电链目标位置
//						ChainLightning.Instance.target = enemyTrans;
//						Destroy (temp, 1f);
//						isAdd = true;	
//					} else {
////						thisType = BulletType.Normal;
//					}
				isAdd = true;
			}	
//			} 
//			//这里面是要 switch 1,2,3,4
//			else {
//				Debug.LogError ("enemyHitSanDianSumList:+" + enemyHitSanDianSumList.Count);
//				enemyHitSanDianSumList = enemyBaseList;
//				for (int i = 0; i < enemyHitSanDianSumList.Count; i++) {
//					EnemyBase enemyBase = enemyBaseList [i];
//					UIFishingMsg.GetInstance ().SndHitFish (userID, enemyBase.groupID, enemyBase.id, bulletID, 
//						transform.position.x, transform.position.y, this.cannonMultiple, isBerserkBullet, enemyBase.typeID, isMyRobot);
//					GameObject temp = GameObject.Instantiate (netEffect, enemyBase.transform.position, Quaternion.identity)as GameObject;
////					UVChainLightning chainLightning = temp.GetComponent <UVChainLightning> ();
////					chainLightning.SetPosition ();
////					Destroy (temp, 1f);
//				}
//			}
//			Debug.LogError ("enemyHitSanDianSumList.count2 = " + enemyHitSanDianSumList.Count);
		}

	}

	/// <summary>
	/// 激光炮穿透效果,这里不能使用HitFish那一套,会被return掉
	/// </summary>
	void LaserLightHitFish ()
	{
		GameObject temp = GameObject.Instantiate (netEffect, transform.position, Quaternion.identity)as GameObject;
		Destroy (temp, 1f);
	}

	public void HitFish (bool ishandianlian = false)
	{
		if (hasHit)
			return;
		if (ishandianlian) {
			
		} else {
			GameObject temp = GameObject.Instantiate (netEffect, transform.position, Quaternion.identity)as GameObject;
			Destroy (temp, 1f);
//			Debug.LogError ("temp" + temp.name);
			hasHit = true;
		}

	}

	//	public void ShandianlianHitFish ()
	//	{
	//
	//	}

	void SetRotation (Vector3 direction)//设置子弹的旋转角度，只有在碰到边界反弹和锁定鱼时才会使用
	{
		float angel = Mathf.Atan (direction.x / direction.y) * 180 / 3.14159f;
		if (direction.y < 0)
			angel += 180;
		if (float.IsNaN (angel)) {
			Debug.LogWarning ("Rotate Error! ");
		} else {
			transform.localEulerAngles = new Vector3 (0, 0, -angel);
		}
	}

	public void SetInfo (int userID, int bulletID, int cannonMultiple, int _currentGunStyle, GameObject target = null, bool localFlag = false, bool isBerserk = false,
	                     bool isMyRobot = false)
	{
		//Debug.LogError ("enemyHitSumList.count1________________ = " + enemyHitSumList.Count);
		this.userID = userID;
		this.bulletID = bulletID;
		this.cannonMultiple = cannonMultiple;
		this.isBerserkBullet = isBerserk;
		this.currentGunStyle = _currentGunStyle;
		this.isMyRobot = isMyRobot;
		if (target != null) {
			EnemyBase temp = target.transform.GetComponent<EnemyBase> ();
			if (temp == null)
				lockEnemy = target.transform.parent.GetComponent<EnemyBase> ();
			else
				lockEnemy = temp;
			if (lockEnemy == null)
				Debug.LogError ("Error! Can't get lockEnemy:" + target.gameObject.name);
			thisType = BulletType.Lock;
		}
//		Debug.Log ("_currentGunStyle = " + _currentGunStyle);
		if (currentGunStyle != 0) {
			//激光炮
			if (currentGunStyle == 6) {
				thisType = BulletType.LaserLight;
			}
			//闪电链
			else if (currentGunStyle == 7) {
				thisType = BulletType.LightningChain;
			}
		}
		isLocal = localFlag;
		if (isLocal)
			bulletSumNum++;
	}


	/// <summary>
	/// 设置场次的发出炮的时候,GunInUI上的金币数值也会变动   因为新加了激光炮,所有减金币的操作 放到Bullet进行
	/// 2018.10.11需求
	/// 添加3倍场和5倍场,所以在这里乘以一个产品需求对的数值
	/// 2019.2.26需求
	/// 新加激光炮,放在这里来进行数值换算
	/// </summary>
	/// <param name="isberserkFlag">If set to <c>true</c> isberserk flag.</param>
	/// <param name="tempMultiple">Temp multiple.</param>
	/// <param name="fieldMultiple">Field multiple.</param>
	void SetFieldMultiple (bool isberserkFlag, int tempMultiple, int fieldMultiple = 1)
	{
		Debug.Log("金幣修改 SetFieldMultiple " + tempMultiple + " : " + isberserkFlag + " : " + fieldMultiple);
		//激光子弹
		if (thisType == BulletType.LaserLight) {
			int countNum = 0;
			countNum =	enemyHitSumList.Count;
//			Debug.LogError ("SetFieldMultiple countNum11 = " + countNum);
			ChangeGunInUIGoldNum (isberserkFlag, tempMultiple, fieldMultiple, countNum);
		}
		//闪电链
		else if (thisType == BulletType.LightningChain) {
			if (enemyHitSanDianSumList.Count > 0) {
				ChangeGunInUIGoldNum (isberserkFlag, tempMultiple, fieldMultiple, enemyHitSanDianSumList.Count);
			}
		} 
		//普通子弹
		else {
			ChangeGunInUIGoldNum (isberserkFlag, tempMultiple, fieldMultiple);
		}
	}

	/// <summary>
	/// 设置减金币的属性
	/// </summary>
	/// <param name="isberserkFlag">If set to <c>true</c> 是否是狂暴 </param>
	/// <param name="tempMultiple"> 当前炮倍数 </param>
	/// <param name="fieldMultiple"> 3倍场,5倍场 </param>
	/// <param name="changeNum"> 乘以的炮倍数 </param>
	void ChangeGunInUIGoldNum (bool isberserkFlag, int tempMultiple, int fieldMultiple, int changeNum = 1)
	{
		Debug.Log("金幣修改 ChangeGunInUIGoldNum "+ tempMultiple + " : "+ fieldMultiple + " : "+ changeNum);
		if (isberserkFlag) {
			// 狂暴状态下消耗两倍金币→2017.11.1改回一倍了→2017.11.23又改回两倍了
			if (PrefabManager._instance != null && PrefabManager._instance.GetGunByUserID (userID) != null) {
				PrefabManager._instance.GetGunByUserID (userID).gunUI.ChangeGoldNum (-2 * tempMultiple * fieldMultiple * changeNum);
				if (GameController._instance.isRedPacketMode && isLocal) {
					//红包场废弃掉,但是还是加上的场次倍数,万一需求需要呢?
					RedPacket_TopInfo._instance.AddGoldCost (2 * tempMultiple * fieldMultiple * changeNum);
				}
			}
		} else {
			//这里赋值的是金币改变值，而不是最终值，是为了配合鱼死后返回金币值时只能知道改变值
			if (PrefabManager._instance != null && PrefabManager._instance.GetGunByUserID (userID) != null) {
//				PrefabManager._instance.GetGunByUserID (userID).gunUI.ChangeGoldNum (-tempMultiple * fieldMultiple * changeNum); 
				if (PrefabManager._instance.GetLocalGun ().currentGold < tempMultiple * fieldMultiple * changeNum) {
					Debug.LogError ("change" + changeNum + "currengold" + PrefabManager._instance.GetLocalGun ().currentGold);
					if (changeNum > 1 && PrefabManager._instance.GetLocalGun ().currentGunStyle == 7) {
						Debug.LogError ("chagen" + changeNum + "tempulie" + tempMultiple * fieldMultiple);
//						PrefabManager._instance.GetGunByUserID (userID).isBankruptcy = true;
						PrefabManager._instance.GetGunByUserID (userID).GoldTip = 1;
						int gold = (int)PrefabManager._instance.GetLocalGun ().currentGold - tempMultiple * fieldMultiple;
						Debug.LogError ("gold:::" + gold);
						if (gold <= 0) {
							
						} else {
							if (gold <= tempMultiple * fieldMultiple) {
								Debug.LogError ("gold1" + gold);
								if (gold == tempMultiple * fieldMultiple) {
									PrefabManager._instance.GetGunByUserID (userID).gunUI.ChangeGoldNum (-(int)PrefabManager._instance.GetLocalGun ().currentGold);
								} else {
									PrefabManager._instance.GetGunByUserID (userID).gunUI.ChangeGoldNum (-tempMultiple * fieldMultiple);
								}
//								PrefabManager._instance.GetGunByUserID (userID).gunUI.ChangeGoldNum (-tempMultiple * fieldMultiple);
							} else if (gold <= tempMultiple * fieldMultiple * 2) {
								Debug.LogError ("gold1" + gold);
								if (gold == tempMultiple * fieldMultiple * 2) {
									PrefabManager._instance.GetGunByUserID (userID).gunUI.ChangeGoldNum (-(int)PrefabManager._instance.GetLocalGun ().currentGold);
								} else {
									PrefabManager._instance.GetGunByUserID (userID).gunUI.ChangeGoldNum (-tempMultiple * fieldMultiple * 2);
								}
							} else if (gold <= tempMultiple * fieldMultiple * 3) {
								Debug.LogError ("gold1" + gold);
								if (gold == tempMultiple * 3) {
									PrefabManager._instance.GetGunByUserID (userID).gunUI.ChangeGoldNum (-(int)PrefabManager._instance.GetLocalGun ().currentGold);
								} else {
									PrefabManager._instance.GetGunByUserID (userID).gunUI.ChangeGoldNum (-tempMultiple * fieldMultiple * 3);
								}

							} else if (gold <= tempMultiple * fieldMultiple * 4) {
								Debug.LogError ("gold1" + gold);
								if (gold == tempMultiple * 4) {
									PrefabManager._instance.GetGunByUserID (userID).gunUI.ChangeGoldNum (-(int)PrefabManager._instance.GetLocalGun ().currentGold);
								} else {
									PrefabManager._instance.GetGunByUserID (userID).gunUI.ChangeGoldNum (-tempMultiple * fieldMultiple * 4);
								}
							}
						}

//						PrefabManager._instance.GetGunByUserID (userID).gunUI.ChangeGoldNum()
					}
				} else {
					PrefabManager._instance.GetGunByUserID (userID).gunUI.ChangeGoldNum (-tempMultiple * fieldMultiple * changeNum); 
				}
				if (GameController._instance.isRedPacketMode && isLocal) {
					RedPacket_TopInfo._instance.AddGoldCost (tempMultiple * fieldMultiple * changeNum);
				}	
			}
		}
	}

	Vector3 GetPlanePos (Vector3 pos)//将3d坐标的z轴与自己相等，切换到一个平面方便计算
	{
		return new Vector3 (pos.x, pos.y, transform.position.z);
	}

	void OnDestroy ()
	{
		switch (myRoomInfo.roomMultiple) {
            //新手港灣
		case 0:

            //深海遺址
		case 1:
			SetFieldMultiple (isBerserkBullet, cannonMultiple, 5);
			break;
		//海神寶藏
		case 2:
			SetFieldMultiple (isBerserkBullet, cannonMultiple, 10);
			break;
		//奪金島
		case 3:
			SetFieldMultiple(isBerserkBullet, cannonMultiple, 20);
			break;
		case 5:
			SetFieldMultiple (isBerserkBullet, cannonMultiple, 20);
			break;
            //海盜港灣
		case 6:
			SetFieldMultiple(isBerserkBullet, cannonMultiple, 3);
			break;
		//体验场
		case 4:
			SetFieldMultiple (isBerserkBullet, cannonMultiple);
			break;
		default:
			break;

		}
		if (isLocal) {
			bulletSumNum--;
		}
//		enemyHitSumList.Clear ();
//		enemyHitSanDianSumList.Clear ();
	}
}
