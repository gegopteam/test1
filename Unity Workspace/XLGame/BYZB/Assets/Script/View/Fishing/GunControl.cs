using UnityEngine;
using System.Collections;
using UnityEngine.EventSystems;
using AssemblyCSharp;
using Spine.Unity;
using UnityEngine.UI;
using DG.Tweening;

public enum GunSeat //分别对应枪座的左下，右下，左上，右上四个方位
{
	LB,
	//0
	RB,
	//1
	RT,
	//2
	LT,
	//3
	Error
	//4
}

public enum RobotBehavior
{
	UseLock,
	//25
	UseBerserk,
	//25
	UseRepclication,
	//15
	UseSummon,
	//10
	ChangeMultiple,
	//10
	ChangeStyle,
	//5
	LeaveRoom
	//10
}

public class GunControl : MonoBehaviour
{

	public const int limitMultiple = 9900;
	//基本属性参数
	public float rotateLerpSpeed = 15f;
	//做平滑旋转时的插值参数
	Vector3 targetPos;
	bool isInFire = false;
	float changeAngle;
	//玩家每次点击屏幕时获取的和正右方向轴的相差角

	//子弹和炮台组件获取
	public GameObject[] bulletPrefabGroup;
	public Transform childGun;
	public Transform invisibleGun;
	public Transform gunpoint;
	//枪口
	[HideInInspector]
	public GunInUI gunUI;
	//创建该炮台的UI层的炮
	//int currentGunBaseIndex = 0;
	public Sprite[] gunBaseGroup;
	public SpriteRenderer gunBaseSprite;
	//炮台底座

	public int GoldTip = 0;
	public SkeletonAnimation skeletonAnim;
	public GameObject[] gunSpineGroup;
	//所有炮台的spine集，用于切换炮台样式
	public GameObject unlockGunPanel;
	Animator childGunAnimator;
	//炮座图片
	public Sprite[] barbetteGroup;
	public SpriteRenderer barbetteSprite;

	//开火特效
	public GameObject[] fireEffectGroup;
	GameObject fireEffect = null;
	ParticleSystem ps_fireEffect;

	//用户信息
	FiUserInfo userInfo = null;
	[HideInInspector] public int userID = -1;
	[HideInInspector] public int gameID = -1;
	[HideInInspector] public int cannonMultiple = 0;
	[HideInInspector] public int maxCannonMultiple = 0;
	[HideInInspector] public long currentGold = 0;
	[HideInInspector] public long curretnDiamond = 0;
	[HideInInspector] public bool isLocal = false;
	//是否为本地玩家
	[HideInInspector] public int vipLevel = -1;
	[HideInInspector] public GunSeat thisSeat;
	[HideInInspector] public int level = -1;
	[HideInInspector] public int currentGunStyle = -1;
	[HideInInspector] public int currentBarbetteStyle = -1;
	public bool isRobot = false;
	int gender = 0;
	Sprite avatorSprite = null;

	private DataControl dataControl = null;
	private MyInfo myInfo = null;

	[HideInInspector] public bool isActived = false;
	//是否被启用，可能是本地玩家，也可能是联网玩家启用的
	bool isOverUI = false;
	//是否点在ui上，如果上，则不旋转炮台
	int bulletID = 0;
	int torpedoBulletID = 0;

	//锁定技能控制
	Transform lockUIPrefab = null;
	//锁定准星的Prefab
	Transform lockUI = null;
	//锁定时出现的准星UI
	public GameObject lockTarget = null;
	//锁定目标
	bool isRepareToAttackLock = true;
	//为true时只是出现准星锁定一个目标，要等到玩家选中一条鱼后才会开火
	bool isInLockTarget = false;
	//是否在锁定开火中
	bool useAutoFire = false;
	//是否自动开炮
	float lockDuration = 0f;
	//已经在锁定状态，并且玩家点击了一条鱼后开始计算的持续时间
	Skill lockSkill = null;



	//分身技能控制
	[HideInInspector]
	public int replicationCount = 0;
	//0代表没有分身，2代表双重分身，3代表三重分身
	int thisReplicationIndex = 0;
	//0代表不在分身状态，1，2，3分别代表分身编号
	int currentLockIndex = 0;
	GunControl motherGun = null;

	GameObject replicationGun2;
	GameObject replicationGun3;

	//狂暴技能控制
	Transform berserkEffectPrefab = null;
	[HideInInspector]
	public bool isBerserk = false;
	Transform berserkEffect = null;

	//双倍技能控制
	bool isDouble = false;

	//冰冻技能控制
	GameObject icePanelPrefab = null;
	GameObject tempIcePanel = null;

	//召唤技能控制

	//鱼雷技能控制
	bool isRepareToFireTorpedo = false;
	//等待鱼雷发射状态
	int torpedoLevel = -1;
	//发射时的鱼雷等级
	GameObject torpedoGrids = null;
	//发射鱼雷之前的网格特效

	public GameObject changeGunStyleEffectPrefab;

	//用fireTimer计时器控制开火频率 炮的射速
	float fireTimer = 0f;
    [HideInInspector]
    public float fireRateTime = 0.17f;
	//每fireRateTime秒开一次火

	Transform bulletPool;

	public GameObject startHintCirclePrefab;
	public GameObject startHintTextPrefab;
	public Transform startHintTextPos;
	[HideInInspector]
	public bool isBankruptcy = false;
	//pk场信息
	[HideInInspector]
	public int bulletNum = 300;

	int hangUpTimer = 1200;

	[HideInInspector] public GunInUI otherGunUI = null;
	/// <summary>
	/// 聊天气泡
	/// </summary>
	public GameObject chatBoxPrefab;
	/// <summary>
	/// 房间信息
	/// </summary>
	RoomInfo myRoomInfo = null;
	MeshRenderer gunSpineMeshRender;

	void Awake ()
	{
		dataControl = DataControl.GetInstance ();
		myInfo = dataControl.GetMyInfo ();
		myRoomInfo = DataControl.GetInstance ().GetRoomInfo ();
	}

	void Start ()
	{
		Init ();
		if (GameController._instance.myGameType == GameType.Point && isLocal) {
			PkPointProgessBar._instance.InitData (thisSeat, userInfo.nickName, gender, GetAvatorSprite ());
		}
	}

	// Update is called once per frame
	void Update ()
	{
		if (!GameController._instance.gameIsReady)
			return;
		// if(Input.GetKeyDown(KeyCode.M)){
		// gunUI.AddValue(0,10000,0);
		// }

		if (isActived) {

			if (isInLockTarget) {
				if (null == lockUI) {
					lockUI = (Transform)GameObject.Instantiate (lockUIPrefab); 
					lockUI.GetComponentInChildren<LockTrack> ().SetStartPoint (transform.position);
				}
                   
                
				if (replicationCount > 0) {
                    
				} else {
                    
				}

				if (null == lockTarget) {
					//lockTarget = UIFishingMsg.GetInstance ().fishPool.GetAim().transform.FindChild("ChildModel").gameObject;
					EnemyBase tempEnemy = null;
					if (thisReplicationIndex > 0) {
						tempEnemy = UIFishingObjects.GetInstance ().fishPool.GetRepclicationLock (thisReplicationIndex);
						if (tempEnemy == null)
							Debug.LogError ("Error! Can't get replication Lock,ReplicationIndex=" + thisReplicationIndex);
					} else {
						tempEnemy = UIFishingObjects.GetInstance ().fishPool.GetAim ();
					}

					//EnemyBase tempEnemy = UIFishingObjects.GetInstance().fishPool.GetAim();
					if (tempEnemy != null) {
						lockTarget = tempEnemy.transform.Find ("ChildModel").gameObject;
					} else {
						lockTarget = null;
						HintText._instance.ShowHint ("暫未找到可以鎖定的魚");
						return;
					}

					// if (null == lockUI)
					//    lockUI = (Transform)GameObject.Instantiate(lockUIPrefab);
				}
				FollowAttackLockTarget (lockTarget);
			}

			if (isLocal) {
				fireTimer += Time.deltaTime;
				InputCheck ();
			}

			childGun.rotation = Quaternion.Lerp (childGun.rotation, invisibleGun.rotation, rotateLerpSpeed * Time.deltaTime);
			if (isRobot) { //如果是机器人，打子弹前发现金币不够打话迅速离开
				if (myRoomInfo.roomMultiple == 3 || myRoomInfo.roomMultiple == 5) {
					if (currentGold - cannonMultiple * 5 <= 0) {
//						Debug.LogError ("机器人，打子弹前发现金币不够打话迅速离开222");
//						Debug.LogError ("currentGold - cannonMultiple * 5 = " + (currentGold - cannonMultiple * 2));
						RobotSendLeaveSelf ();
						return;
					}
				} else if (myRoomInfo.roomMultiple == 2) {
					if (currentGold - cannonMultiple * 3 <= 0) {
//						Debug.LogError ("机器人，打子弹前发现金币不够打话迅速离开222");
						//Debug.LogError("currentGold - cannonMultiple * 2 = " + (currentGold - cannonMultiple * 2));
						RobotSendLeaveSelf ();
						return;
					}
				} else {
					if (currentGold - cannonMultiple * 2 <= 0) {
//						Debug.LogError ("机器人，打子弹前发现金币不够打话迅速离开222");
//						Debug.LogError ("currentGold - cannonMultiple * 2 = " + (currentGold - cannonMultiple * 2));
						RobotSendLeaveSelf ();
						return;
					} 
				}
				//if (currentGold - cannonMultiple * 2 <= 0) {
				//                Debug.LogError("机器人，打子弹前发现金币不够打话迅速离开222");
				//	Debug.LogError ("currentGold - cannonMultiple * 2 = " + (currentGold - cannonMultiple * 2));
				//	RobotSendLeaveSelf ();
				//	return;
				//}
			}
		}

	}

	void Init ()
	{
		isInFire = false;
		fireTimer = 0f;
		//invisibleGun = transform.FindChild("InvisibleGun");
		//gunpoint = invisibleGun.FindChild ("Gunpoint");
		bulletPool = GameObject.FindGameObjectWithTag (TagManager.bulletPool).transform;

		lockUIPrefab = PrefabManager._instance.GetPrefabObj (MyPrefabType.SkillEffect, 0).transform;
		berserkEffectPrefab = PrefabManager._instance.GetPrefabObj (MyPrefabType.SkillEffect, 1).transform;
		icePanelPrefab = PrefabManager._instance.GetPrefabObj (MyPrefabType.SkillEffect, 2);

		childGunAnimator = childGun.GetComponent<Animator> ();
		gunSpineMeshRender = skeletonAnim.transform.GetComponent <MeshRenderer> ();
		gunSpineMeshRender.sortingOrder = 2;
		//SetGunShow (false);
		if (thisReplicationIndex == 0)
			childGun.gameObject.SetActive (false);
		if (GameController._instance.myGameType != GameType.Classical) { //先执行OnUserJoin，再执行Init
			if (!isActived) {
				gunBaseSprite.enabled = false;
				if (gunUI != null)
					gunUI.SetWaitTextShow (false);
				else
					Debug.LogError (this.name + " gunUI=null");
			}
		} else {
			if (!isActived) {
				gunBaseSprite.enabled = false;
				if (gunUI != null)
					gunUI.SetWaitTextShow (true);
				else
					Debug.LogError (this.name + " gunUI=null");
			}
		}
		//		if(null==userInfo)
		//		{
		//			Tool.LogError ("Init SetGunShow 111");
		//			SetGunShow (false);
		//		}
		if (isLocal && !GameController._instance.isExperienceMode) {
			if (thisReplicationIndex <= 1) {
                
				Panel_UnlockMultiples._instance.DelayInit ();
				if (myInfo.beginnerCurTask > 1) {
					GameObject temp = GameObject.Instantiate (startHintCirclePrefab, transform.position, Quaternion.identity, this.transform) as GameObject;
					if (thisSeat == GunSeat.LT || thisSeat == GunSeat.RT) {
						temp.transform.Find ("_Effect_Buyu_zhishikuang/StartHintTextPrefab").localScale = new Vector3 (-1, -1, 1);
					}

					Destroy (temp, 3f);
				}
			}


		}

	}

	void InputCheck ()
	{
		
		if (Input.GetMouseButtonDown (0)) {

			if (EventSystem.current.IsPointerOverGameObject ()) { //点击到UI层时直接return，视为无效输入
				isOverUI = true;
				return;
			}
#if !UNITY_EDITOR //后期可能要对IOS和安卓平台单独检测
			if (ScreenManager.IsPointOverUI (Input.GetTouch (0).fingerId)) {
				isOverUI=true;
				return;
			}
#endif
			isOverUI = false;
			//如果已经破产了却还要点发射,并且是游客,弹出转正提示,否则,弹出获取金币面板
			if (isBankruptcy) { 
				if (myInfo.isGuestLogin && isLocal) {
					PrefabManager._instance.CreateGuestNotice ();
				} else {
					if (!CheckCanFire () && thisReplicationIndex <= 1) {
						Debug.LogError ("CheckthisReplicationIndex,彈出轉正提示,否則,彈出獲取金幣面板");
						LeftOption._instance.ShowGetGoldPanel ();
					}
				}
			}

			RaycastHit hitinfo;
			//	Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
			Ray ray = ScreenManager.uiCamera.ScreenPointToRay (Input.mousePosition);
			if (isRepareToFireTorpedo) {
				isRepareToFireTorpedo = false;
				if (Physics.Raycast (ray, out hitinfo, 1000f)) {
					FireOneTorpedo (hitinfo.point, this.torpedoLevel);
				}
				return;
			}

			if (isInLockTarget) {
				if (Physics.Raycast (ray, out hitinfo, 1000f, GameController.enemyLayer)) {
					//CancelInvoke("AutoUseLockSkill");
					isInFire = true;
					if (thisReplicationIndex == 1) {
						switch (currentLockIndex) { //由分身的母体负责控制子物体锁定对象的转移
						case 0:
							lockTarget = hitinfo.collider.gameObject;
							break;
						case 1:
							lockTarget = hitinfo.collider.gameObject;
							break;
						case 2:
							replicationGun2.GetComponent<GunControl> ().lockTarget = hitinfo.collider.gameObject;
							break;
						case 3:
							replicationGun3.GetComponent<GunControl> ().lockTarget = hitinfo.collider.gameObject;
							break;
						default:
							break;
						}
						currentLockIndex++;
						if (currentLockIndex > replicationCount) {
							currentLockIndex = 1;
						}
					} else if (thisReplicationIndex == 0) {
						lockTarget = hitinfo.collider.gameObject;
					}
					if (isRepareToAttackLock && false) { //isRepareToAttackLock暂时为无效变量，弃用这段条件
						lockSkill.ActiveSkill ();//正式启用使图标进入cd读条
						Invoke ("CompleteLockSkill", lockDuration);//完成锁定技能后是否也需要上传消息？还是本地获得B用户开始锁定技能后自行判断时间?
						isRepareToAttackLock = false; //此时说明玩家已经选取到一个目标
					}
				} else { //如果没有点击到怪物层，则普通开一炮
					isInFire = true;
				}
				return;
			}

			isInFire = true;
			//fireTimer += 0.1f ; //设置0，1f的操作延迟，避免无限连点
		}


		if (Input.GetMouseButtonUp (0)) {
			if (!useAutoFire || isInLockTarget)
				isInFire = false;
		}
		if (thisReplicationIndex == 1)
			return;
		if (isInFire) {  //若一直触摸屏幕，则持续开火，并实时更新炮口旋转
			RaycastHit hitinfo2;
			//Ray ray = Camera.main.ScreenPointToRay (Input.mousePosition);
			Ray ray2 = ScreenManager.uiCamera.ScreenPointToRay (Input.mousePosition);
			if (Physics.Raycast (ray2, out hitinfo2, 1000f)) {
				if (!isOverUI) { //只有玩家没有点到UI时，才让炮台转向
					if (!isInLockTarget)
						GunRotate (hitinfo2.point);
				}
				HoldOnFire ();
			} else {

			}
		}
	}

	/// <summary>
	/// 其他玩家发射子弹
	/// </summary>
	/// <param name="bulletID">Bullet I.</param>
	/// <param name="cannonMultiple">Cannon multiple.</param>
	/// <param name="x">The x coordinate.</param>
	/// <param name="y">The y coordinate.</param>
	/// <param name="berserkFlag">If set to <c>true</c> berserk flag.</param>
	/// <param name="groupID">Group I.</param>
	/// <param name="enemyID">Enemy I.</param>
	public void FireToPos (int bulletID, int cannonMultiple, float x, float y, bool berserkFlag = false, int groupID = -1, int enemyID = -1) //用作同步其他网络用户的攻击
	{
        
		if (thisReplicationIndex == 1) { //说明是分身母体
			switch (currentLockIndex) {
			case 0:
			case 1:
				currentLockIndex++;
				if (currentLockIndex > replicationCount) {
					currentLockIndex = 1;
				}
				break;
			case 2:
				replicationGun2.GetComponent<GunControl> ().FireToPos (bulletID, cannonMultiple, x, y, berserkFlag, groupID, enemyID);
				currentLockIndex++;
				if (currentLockIndex > replicationCount) {
					currentLockIndex = 1;
				}
				return;
			case 3:
				replicationGun3.GetComponent<GunControl> ().FireToPos (bulletID, cannonMultiple, x, y, berserkFlag, groupID, enemyID);
				currentLockIndex++;
				if (currentLockIndex > replicationCount) {
					currentLockIndex = 1;
				}
				return;
			default:
				break;
			}
		}

		Vector3 pos = new Vector3 (x, y, transform.position.z);

		if (groupID != -1 && enemyID != -1) {
			//EnemyBase enemy = UIFishingMsg.GetInstance ().fishPool.Get(groupID,enemyID);
			EnemyBase enemy = UIFishingObjects.GetInstance ().fishPool.Get (groupID, enemyID);
			if (enemy != null) {
				lockTarget = enemy.gameObject;
				GunRotate (enemy.transform.position);
			} else {
				GunRotate (pos);
			}
             
		} else {
			GunRotate (pos);
		}
		FireOneBullet (bulletID, cannonMultiple, (int)thisSeat, userID, isBerserk, lockTarget);
	}

	/// <summary>
	/// 寻找攻击目标,时时检测
	/// </summary>
	/// <param name="target">Target.</param>
	void FollowAttackLockTarget (GameObject target)
	{
		if (null == target) {
			ChangeLockTarget ();
		} else {
			EnemyBase temp = target.GetComponent<EnemyBase> ();
			if (temp == null)
				temp = target.transform.parent.GetComponent<EnemyBase> ();
			//当鱼死亡,或者鱼不在屏幕的话,就让他切换一个目标
			if (temp.thisState == EnemyBase.EnemyState.Dead || ScreenManager.IsInScreen (target.transform.position) == false) {
				ChangeLockTarget ();
			}
		}
		Vector3 newPos = new Vector3 (target.transform.position.x, target.transform.position.y, 0f);

		GunRotate (newPos);
		lockUI.position = newPos;
		// if (!isRepareToAttackLock &&!isBankruptcy )
		//  HoldOnFire();  //新版锁定不能一直打
		if ((thisReplicationIndex > 0 || useAutoFire) && !isBankruptcy)
			HoldOnFire ();

	}

	/// <summary>
	/// 为空时重新选取一个Target,鱼死亡,鱼出屏幕,重新选取
	/// </summary>
	/// <param name="target">Target.</param>
	void ChangeLockTarget (GameObject target = null)
	{
		//lockTarget =UIFishingMsg.GetInstance ().fishPool.GetAim ().gameObject;
		EnemyBase temp = UIFishingObjects.GetInstance ().fishPool.GetAim ();
		if (temp != null)
			lockTarget = temp.gameObject;
		//  lockTarget = UIFishingObjects.GetInstance().fishPool.GetAim().gameObject;
	}

	void GunRotate (Vector3 pos)
	{
		// GunRotate2(pos);
		// return;
		pos = new Vector3 (pos.x, pos.y, transform.position.z);
		targetPos = pos;
		if (thisSeat == GunSeat.LB || thisSeat == GunSeat.RB) {
			changeAngle = Vector3.Angle (Vector3.right, pos - invisibleGun.position);
			invisibleGun.localEulerAngles = new Vector3 (0, 0, changeAngle - 90f);
		} else {
			changeAngle = Vector3.Angle (Vector3.left, pos - invisibleGun.position);
			invisibleGun.localEulerAngles = new Vector3 (0, 0, changeAngle - 90f);
		}
	}

	void GunRotate2 (Vector3 pos)
	{
		pos = new Vector3 (pos.x, pos.y, transform.position.z); //把传入的坐标点先同步到和炮台一个平面
		targetPos = pos;

		changeAngle = Mathf.Rad2Deg * Mathf.Atan ((transform.position.y - pos.y) /
		(transform.position.x - pos.x));

		if (transform.position.x - pos.x < 0)
			changeAngle = changeAngle - 90f;
		else
			changeAngle = changeAngle + 90f;
		invisibleGun.localEulerAngles = new Vector3 (0, 0, changeAngle);

	}

	/// <summary>
	/// 持续开火，子弹初期用动态生成
	/// </summary>
	void HoldOnFire ()
	{
		//fireTimer += Time.deltaTime;
		//if (thisReplicationIndex > 1)
		//Debug.LogError(fireTimer);

		if (isBankruptcy)
			return;
		if (isBerserk) { //狂暴时提高开火频率
			if (fireTimer > fireRateTime * 0.8f) {
				fireTimer = 0;
				FireOneBullet (GetBulletID (), cannonMultiple, (int)thisSeat, userID, true, lockTarget);
			}
		} else {
			if (fireTimer > fireRateTime) {
				fireTimer = 0;
				FireOneBullet (GetBulletID (), cannonMultiple, (int)thisSeat, userID, false, lockTarget);
			}
		}

	}

	/// <summary>
	/// 发射子弹
	/// </summary>
	/// <param name="bulletID">Bullet I.</param>
	/// <param name="multiple">Multiple.</param>
	/// <param name="seatID">Seat I.</param>
	/// <param name="userID">User I.</param>
	/// <param name="berserkFlag">If set to <c>true</c> berserk flag.</param>
	/// <param name="lockTarget">Lock target.</param>
	void FireOneBullet (int bulletID, int multiple, int seatID, int userID, bool berserkFlag = false, GameObject lockTarget = null)
	{
		if (isLocal) {
       //     Debug.LogError("multiple=" + multiple + "userID" + userID);
			hangUpTimer = 1200;
			if (Bullet.bulletSumNum >= 60) {
				//HintText._instance.ShowHint ("屏幕中子弹数超过20，暂时无法发射！");
				isInFire = false;
				HintTextPanel._instance.SetTextShow ("屏幕中子彈數超過60，暫時無法發射！");
				if (useAutoFire) {
					PrefabManager._instance.GetSkillUIByType (SkillType.AutoFire).CancleAutoFire ();
				}
				return;
			}
			//Debug.Log ("multiple" + multiple);
			//FishingAddScore (multiple);

			if (thisReplicationIndex > 1) {
				bulletID = motherGun.GetBulletID ();
			}
		}
			
		//金币改变判断和UI显示
		switch (GameController._instance.myGameType) {
		case GameType.Classical:
			if (thisReplicationIndex > 1) {
				currentGold = motherGun.currentGold;
			}
                //金币不足时
			if (!CheckCanFire ()) {
				if (isLocal) {
//					Debug.Log ("是否已经破产1");
					if (FitMultiple () == false) { //匹配失败，说明金币已经小于等于0
						//Debug.Log("是否已经破产"+Skill.Instance.isHaveTorpedo);
						//bool isHave=  Skill.Instance.GetIsHaveTorpedo();
//						Debug.Log ("GameController._instance.isHaveTorpedo" + GameController._instance.isHaveTorpedo);
						if (GameController._instance.isHaveTorpedo) {
							PrefabManager._instance.TorpedoTipsObj.SetActive (true);
//							Debug.Log ("破产了但是有鱼雷");
						}
						PrefabManager._instance.GetSkillUIByType (SkillType.Lock).CompleteAllUseSkill (SkillType.Lock);
						Debug.LogError ("ThisReplicatinIndex=" + thisReplicationIndex + "isBankruptcy" + isBankruptcy);
						if (isBankruptcy) { //是否已经破产
                               
						} else {
							//HintTextPanel._instance.SetTextShow("金币不足，可通过[获取金币]按钮充值");

							isInFire = false;
							if (thisReplicationIndex <= 1)
								SetBankruptcyState (true);
							if (thisReplicationIndex <= 1) {  
								
								if (GameController._instance.isExperienceMode) {
									return;
								}
								if (myInfo.isGuestLogin) {
									PrefabManager._instance.CreateGuestNotice ();
								} else
									LeftOption._instance.ShowGetGoldPanel ();
							}

							//gunUI.SetBankruptcyShow(true); //破产部分的功能，暂时不做
							//isBankruptcy = true;
						}

						//if (useAutoFire) {
						//	if (myInfo.misHaveDraCard)
						//		PrefabManager._instance.GetSkillUIByType (SkillType.Lock).LockAndAudio ();
						//	else
						//		PrefabManager._instance.GetSkillUIByType (SkillType.Lock).NoLongCardLockAndAudio ();
						//}

						return;
					}

				} else {
                        
					return;
				}
			}
//			if (bullet.thisType != Bullet.BulletType.LaserLight) {
////            //如果触发了这之后的，说明金币是足够的
//				switch (myRoomInfo.roomMultiple) {
//				case 0:
//				case 1:
//					SetFieldMultiple (berserkFlag, multiple);
//					break;
//				//三倍场
//				case 2:
//					SetFieldMultiple (berserkFlag, multiple, 3);
//					break;
//				//五倍场
//				case 3:
//				case 5:
//					SetFieldMultiple (berserkFlag, multiple, 5);
//					break;
//				//体验场
//				case 4:
//					SetFieldMultiple (berserkFlag, multiple);
//					break;
//				default:
//					break;
//				}	
//			}
//			------------------这里的逻辑嵌出到 SetFieldMultiple 中      
//			if (berserkFlag) {
//				gunUI.ChangeGoldNum (-2 * multiple);// 狂暴状态下消耗两倍金币→2017.11.1改回一倍了→2017.11.23又改回两倍了
//				if (GameController._instance.isRedPacketMode && isLocal) {
//					RedPacket_TopInfo._instance.AddGoldCost (2 * multiple);
//				}
//			} else {
//				gunUI.ChangeGoldNum (-multiple); //这里赋值的是金币改变值，而不是最终值，是为了配合鱼死后返回金币值时只能知道改变值
//				if (GameController._instance.isRedPacketMode && isLocal) {
//					RedPacket_TopInfo._instance.AddGoldCost (multiple);
//				}
//			}
//                //Tool.LogError("GoldCost="+ multiple* (isBerserk?2:1)+ " GoldRest="+currentGold);
//               // Tool.OutLogWithToFile("GoldCost=" + multiple * (isBerserk ? 2 : 1) + " GoldRest=" + currentGold);
//			if (currentGold - multiple < 0) { //打完子弹减小金币后，还要再做一次炮倍数适配
//				FitMultiple ();
//			}
			break;
		case GameType.Bullet:
			if (bulletNum <= 0) {
				if (isLocal) {
					HintTextPanel._instance.SetTextShow ("子彈已經打完，請耐心等待結算結果", 5f);
					CompleteLockSkill ();
					if (useAutoFire) {
						PrefabManager._instance.GetSkillUIByType (SkillType.AutoFire).CancleAutoFire ();
					}
				}
				return;
			}
			bulletNum--;
			gunUI.SetBulletNum (bulletNum);
			break;
		case GameType.Time://无限子弹
			break;
		case GameType.Point:
			int tempScore = gunUI.GetScore ();
			int tempMultiple = isDouble ? 2 : 1;
			if (tempScore - tempMultiple < 0) {
				HintTextPanel._instance.SetTextShow ("積分不夠，無法開砲", 3f);
				return;
			}
			tempScore -= tempMultiple;
			if (otherGunUI == null) {
				if (thisSeat == GunSeat.LB) {
					otherGunUI = PrefabManager._instance.GetPrefabObj (MyPrefabType.GunInUI, (int)GunSeat.RB).GetComponent<GunInUI> ();
				} else if (thisSeat == GunSeat.RB) {
					otherGunUI = PrefabManager._instance.GetPrefabObj (MyPrefabType.GunInUI, (int)GunSeat.LB).GetComponent<GunInUI> ();
				}
			}
			otherGunUI.ChangeScore (tempMultiple);

			gunUI.SetScoreText (tempScore, true);
			break;
		default:
			break;
		}

		//生成子弹并设定子弹属性
		int bulletVipIndex;
		if (berserkFlag)
			bulletVipIndex = bulletPrefabGroup.Length - 1;
		else
			bulletVipIndex = currentGunStyle;

		GameObject tempBullet = GameObject.Instantiate (bulletPrefabGroup [bulletVipIndex], gunpoint.position, invisibleGun.rotation, bulletPool) as GameObject;
		tempBullet.transform.tag = TagManager.bullet;
		Bullet bullet = tempBullet.GetComponent<Bullet> ();
		bullet.SetInfo (userID, bulletID, multiple, currentGunStyle, lockTarget, isLocal, berserkFlag, isRobot); //赋予子弹用户属性
		UIFishingObjects.GetInstance ().bulletPool.Add (bullet);
		if (isLocal) {
			if (thisReplicationIndex > 0)
				AudioManager._instance.PlayEffectClip (AudioManager.effect_fire, 0.5f);
			else
				AudioManager._instance.PlayEffectClip (AudioManager.effect_fire, 1f);
            
			//FitMultiple();
		}

		if (thisReplicationIndex == 0 || true) { //原来默认分身关闭狂暴特效，现在开启
			//FireParticleFixRoatition();
			ps_fireEffect.Play ();
		}

		PlayAttackAnim ();
		//如果是本地用户发射的子弹，需要向服务器发送消息
		if (isLocal) { 
			//锁定状态下发送锁定的鱼的两个id
			if (lockTarget != null) {
				EnemyBase enemy = lockTarget.GetComponent<EnemyBase> ();
				if (enemy == null) {
					enemy = lockTarget.transform.parent.GetComponent<EnemyBase> ();
				}
				if (enemy == null)
					Debug.LogError ("Enemy=null!");
				if (!ScreenManager.IsInScreen (lockTarget.transform.position)) {
					return;
				}
				UIFishingMsg.GetInstance ().SndFireBullet (this.userID, bulletID, targetPos.x, targetPos.y, multiple, isBerserk, enemy.groupID, enemy.id);
			} else {//非锁定状态下直线打出普通子弹
				UIFishingMsg.GetInstance ().SndFireBullet (this.userID, bulletID, targetPos.x, targetPos.y, multiple, berserkFlag);
			}

		}
	}


	/// <summary>
	/// 设置场次的发出炮的时候,GunInUI上的金币数值也会变动   因为新加了激光炮,所有减金币的操作 放到Bullet进行
	/// 2018.10.11需求
	/// 添加3倍场和5倍场,所以在这里乘以一个产品需求对的数值
	/// </summary>
	/// <param name="isberserkFlag">If set to <c>true</c> 是否为狂暴</param>
	/// <param name="tempMultiple">炮倍数</param>
	/// <param name="fieldMultiple">场次倍数</param>
	void SetFieldMultiple (bool isberserkFlag, int tempMultiple, int fieldMultiple = 1)
	{
		return;
		if (isberserkFlag) {
			// 狂暴状态下消耗两倍金币→2017.11.1改回一倍了→2017.11.23又改回两倍了
			gunUI.ChangeGoldNum (-2 * tempMultiple * fieldMultiple);
			if (GameController._instance.isRedPacketMode && isLocal) {
				//红包场废弃掉,但是还是加上的场次倍数,万一需求需要呢?
				RedPacket_TopInfo._instance.AddGoldCost (2 * tempMultiple * fieldMultiple);
			}
		} else {
			//这里赋值的是金币改变值，而不是最终值，是为了配合鱼死后返回金币值时只能知道改变值
			gunUI.ChangeGoldNum (-tempMultiple * fieldMultiple); 
			if (GameController._instance.isRedPacketMode && isLocal) {
				RedPacket_TopInfo._instance.AddGoldCost (tempMultiple * fieldMultiple);
			}
		}
		//Tool.LogError("GoldCost="+ multiple* (isBerserk?2:1)+ " GoldRest="+currentGold);
		// Tool.OutLogWithToFile("GoldCost=" + multiple * (isBerserk ? 2 : 1) + " GoldRest=" + currentGold);
		//打完子弹减小金币后，还要再做一次炮倍数适配
		if (currentGold - tempMultiple < 0) { 
			FitMultiple ();
		}
	}

	void FireParticleFixRoatition ()
	{
		//开火粒子特效部分，在不使用面片的情况下需要通过代码对粒子进行旋转
		if (currentGunStyle == 5000) {
			return;
		}
		if (thisSeat == GunSeat.LB || thisSeat == GunSeat.RB) {
			ps_fireEffect.startRotation = 0.0174533f * (-changeAngle + 90f); // Mathf.PI / 180f=0.0174533f
		} else {
			ps_fireEffect.startRotation = 0.0174533f * (-changeAngle - 90f);
		}
	}

	int GetBulletID ()
	{
		if (bulletID > 1000000)
			bulletID = 1;
		bulletID++;
		return bulletID;
	}

	int GetTorpedoBulletID ()
	{
		if (torpedoBulletID > 100000)
			torpedoBulletID = 1;
		torpedoBulletID++;
		return torpedoBulletID;
	}


	//玩家加入
	public void OnUserJoin (FiUserInfo info)
	{
//		Debug.LogError ("UserJoin:" + info.userId + " name:" + info.nickName + " GunStyle:" + info.cannonStyle + " gender:" + info.gender);
		info.nickName = Tool.GetName (info.nickName, 10);
		// gunUI.ShowChatBubbleBox("测试测测撒道具啊家哦赛道", 5f);//debugTest
		// info.gold = 30000; //debugTest
		// info.diamond = 10;//debugTest
		switch (GameController._instance.myGameType) {
		case GameType.Classical:
			break;
		case GameType.Bullet:
			info.cannonMultiple = 1;
			info.gold = 0;
			break;
		case GameType.Point:
			info.cannonMultiple = 1;
			info.gold = 0;
			break;
		case GameType.Time:
			info.cannonMultiple = 1;
			info.gold = 0;
			break;
		default:
			break;
		}
		SetGunShow (true);
		gunUI.SetWaitTextShow (false);
		gunBaseSprite.enabled = true;
		this.userInfo = info;
		bool localFlag;
		this.userID = info.userId;
		if (info.userId == myInfo.userID) { //自己
			localFlag = true;
			SetLocalAvator ();
			InvokeRepeating ("HangUpCheck", 1f, 10f); //挂机检测，20分钟不发射子弹则踢出游戏
			SetGunBase (0);
			gameID = (int)myInfo.loginInfo.gameId;
			//Debug.LogError("MyGameID=" + gameID);
			Invoke ("CheckSummonRobot", 0.1f); //等所有炮台信息加入初始化完之后，再检查是否需要机器人，所以这里保险起见等一会儿
			//CheckSummonRobot();
		} else {
			localFlag = false;
			SetOtherAvator (info.avatar);
			SetGunBase (1);
			gameID = info.gameId;
			if (!GameController._instance.isBossMatchMode) {
				gunUI.GetFishingUserRank (userID, info.userChampionsRank);
			}
			// Debug.LogError("OtherGameID=" + gameID);
		}
		//如果是体验场的,开启vip,最大炮
		if (GameController._instance.isExperienceMode && localFlag) {
			myInfo.levelVip = 9;
			info.maxCannonMultiple = 9900;
			PrefabManager._instance.CreateWelcomeExperience ();
		}
		//Debug.LogError("GameId="+gameID);
		SetGunInfo (true, localFlag, info);
		myInfo.cannonMultipleNow = info.cannonMultiple;

		gunUI.SetUIShow (true);
		if (GameController._instance.isExperienceMode) {
			gunUI.SetExperienceUIInfo (info, localFlag);
		} else {
			gunUI.SetUIInfo (info, localFlag);
		}
		if (localFlag) {
			FitMultiple (); //不需要自动适配炮倍，已经暂时废弃
			Debug.Log ("myInfo.beginnerCurTask = " + myInfo.beginnerCurTask);
			if (myInfo.beginnerCurTask >= 1 && myInfo.beginnerCurTask <= 9) {
				if (GameController._instance.myGameType == GameType.Classical && !GameController._instance.isRedPacketMode) {
//					Debug.Log ("是否走到这里新手任务");
					PrefabManager._instance.ShowNewcomerMission (myInfo.beginnerCurTask, myInfo.beginnerProgress);
				}
			}
		}
	}

	//玩家离开
	public void OnUserLeave ()
	{
   
		if (isRobot) {
			isRobot = false;
			CancelInvoke ();
		}
		float randomTime = Random.Range (5, 16f);
		Invoke ("CheckSummonRobot", randomTime);

		Debug.LogError ("UserLeave,id=" + userID + " name=" + gunUI.GetNickName ());
		//分身控制
		if (replicationGun2 != null) {
			Destroy (replicationGun2);
		}

		if (replicationGun3 != null) {
			Destroy (replicationGun3);
		}

		SetGunShow (false);
		//玩家离开把炮座信息也清除一下
		SetBarbette (0);
		this.userInfo = null;
		this.userID = -1; //如果pk赛中途有人离场，userID变为-1的话，那么有可能排行榜无法正常读取数据？
		if (berserkEffect != null) {
			Destroy (berserkEffect.gameObject);
			berserkEffect = null;
		}
     
		gunUI.SetUIShow (false);
		gunUI.userRankPanel.GetComponent<Text> ().text = "未 上 榜";
		gunUI.openRankTimeBool = false;//防止离开前切换了炮倍数，离开后五秒显示出排行榜
		//gunUI.StopCoroutine ("ChangeRankAndMultiple");
		switch (GameController._instance.myGameType) {
		case GameType.Classical:
			gunBaseSprite.enabled = false;
			gunUI.SetWaitTextShow (true);
			break;
		case GameType.Bullet:

			break;
		case GameType.Point:

			break;
		case GameType.Time:

			break;
		default:
			break;
		}

		if (OtherUserInfoBox._instance != null) {
			if (OtherUserInfoBox._instance.GetSeat () == thisSeat) {
				OtherUserInfoBox._instance.Hide ();
			}
		}

	}

	void SetGunShow (bool toShow)
	{
		//Debug.LogError ("SetGunShow:" + toShow+"Seat:"+thisSeat );
		this.isActived = toShow;
		if (toShow) {
			childGun.gameObject.SetActive (true);
		} else {
			childGun.gameObject.SetActive (false);
		}
	}

	public void ShowUserGun ()
	{
		SetGunShow (null != userInfo);
	}

	//设置3d世界的炮台本身的属性
	public void SetGunInfo (bool _isActived, bool _isLocal, FiUserInfo info)
	{
		//thisSeat = seat;
		this.isActived = _isActived;
		this.isLocal = _isLocal;
		this.maxCannonMultiple = info.maxCannonMultiple;
//		Debug.LogError ("SetGunInfo =info.maxCannonMultiple =  " + info.maxCannonMultiple);
		this.cannonMultiple = info.cannonMultiple;
		this.userID = info.userId;
		if (GameController._instance.isExperienceMode)
			this.currentGold = info.testCoin;
		else
			this.currentGold = info.gold;
//		Debug.LogError ("info.testCoin = " + info.testCoin);
		this.curretnDiamond = info.diamond;
		this.level = info.level;
		this.vipLevel = info.vipLevel;
		this.currentGunStyle = info.cannonStyle - 3000;
		this.gender = info.gender;
		this.gameID = info.gameId;
		this.currentBarbetteStyle = info.cannonBabetteStyle;
		SetGunSpine (currentGunStyle);
		if (isLocal) {
			SetBarbette (myInfo.cannonBabetteStyle);
		} else {
//			Debug.LogError ("SetGunInfo currentBarbetteStyle = " + currentBarbetteStyle);
			SetBarbette (currentBarbetteStyle);
		}
		//SetBarbette (3);
	}


	//由GunUI计算过炮倍数后返还给自己
	public void ChangeCannonMultiple (int finalValue, bool shouldSnd = true)
	{
		this.cannonMultiple = finalValue;
		myInfo.cannonMultipleNow = finalValue;
		if (shouldSnd) {
			UIFishingMsg.GetInstance ().SndChangeCannonMultiple (finalValue, userID);
//			Debug.LogError ("Send changeMul=" + finalValue);
		}
        Debug.LogError("cannonMultiple = "+cannonMultiple);
        //改变炮倍数时 也改变分身的炮倍数
        if (replicationCount>=1)
        {
            replicationGun2.GetComponent<GunControl>().cannonMultiple = finalValue;
        }
        if (replicationCount>=2)
        {
            replicationGun3.GetComponent<GunControl>().cannonMultiple = finalValue;
        }

    }
	//取消技能时关闭身上的技能和特效
	public void UserCanCelSkill (SkillType skillType, float duration, bool shouldUse = true)
	{
		switch (skillType) {
		case SkillType.Berserk:
			{
				StopCoroutine ("CloseBerserk");
				CompleteBerserkSkill ();
			}
			break;
		case SkillType.Replication:
			{
				StopCoroutine ("CloseReplication");
				ReplicationComplete ();
			}
			break;
		default:
			break;
		}
	}

	public void CancleAuto ()
	{
		//UseSkill (SkillType.AutoFire, 1f, false);
		//UseSkill (SkillType.Lock, 1f, false);
		StopCoroutine ("CloseLockAndAudio");
	}

	public void UseSkill (SkillType skillType, float duration, bool shouldUse = true, int torpedoLevel = 0)
	{
		if (skillType == SkillType.Berserk || skillType == SkillType.Freeze
		    || skillType == SkillType.Summon || skillType == SkillType.Replication) {
			gunUI.ShowUseSkillEffect (skillType);
		}

		switch (skillType) {
		case SkillType.Error:
			break;
		case SkillType.Freeze://冻鱼的功能与炮台本身无关，所以这里不需要执行什么，在技能UI上已经执行了冻鱼的操作
			if (tempIcePanel != null) {
				Destroy (tempIcePanel);
			}
			tempIcePanel = GameObject.Instantiate (icePanelPrefab);

			if (isLocal && NewcormerMissionPanel._instance != null) {
				if (NewcormerMissionPanel._instance.currentMissionIndex == 3) {
					NewcormerMissionPanel._instance.AddMissionProgress (1);
				}
			}

			Destroy (tempIcePanel, duration);
			break;
		case SkillType.Lock:
                //lockSkill = skill;
			if (isLocal) { //如果是本地用户接到自己使用锁定技能的请求，只是进入锁定但不攻击的等待状态
				isRepareToAttackLock = true;
				//isInFire = true;//自动开炮模式下使用锁定，则停止开火，现在改成锁定自动攻击
				SetAutoFire (true);//自动开火的功能直接在这里调用
				lockSkill = PrefabManager._instance.GetSkillUIByType (SkillType.Lock);
				lockDuration = duration;
				isInLockTarget = true; //此时只是进入锁定状态，到玩家选取鱼开火前，都不需要上传消息
				//Invoke("AutoUseLockSkill", 0.5f); //20s秒后强制使用→2017.12.4需求变更，要求使用后马上开炮→2018.3.27需求变更，本行作废
				//---------上述代码改为这段替代
				lockSkill.ActiveSkill ();//正式启用使图标进入cd读条
				//Invoke ("CompleteLockSkill", lockDuration);//完成锁定技能后是否也需要上传消息？还是本地获得B用户开始锁定技能后自行判断时间?
                
				isRepareToAttackLock = false; //此时说明玩家已经选取到一个目标
				//----------
//				if (NewcormerMissionPanel._instance != null) {
//					if (NewcormerMissionPanel._instance.currentMissionIndex == 4) {
//						NewcormerMissionPanel._instance.AddMissionProgress (1);
//					}
//				}
				waitTime = lockDuration;
				if (!myInfo.misHaveDraCard) {
					if (!shouldUse) {
						StartCoroutine ("CloseLockAndAudio");
						// Invoke("CompleteBerserkSkill", duration);
					} else {
						StopCoroutine ("CloseLockAndAudio");
						CompleteLockSkill ();
					}
				} else {
					if (shouldUse) {
						CompleteLockSkill ();
					}
				}
                 
			} else { //如果是本地用户接到其他玩家使用锁定技能的请求，那么？？？

			}
			break;
		case SkillType.Berserk:
			if (replicationGun2 != null) {
				replicationGun2.GetComponent<GunControl> ().UseSkill (SkillType.Berserk, duration, true);
			}
			if (replicationGun3 != null) {
				replicationGun3.GetComponent<GunControl> ().UseSkill (SkillType.Berserk, duration, true);
			}

			fireTimer = 0;
			isBerserk = true;
			if (berserkEffect != null) {
				Destroy (berserkEffect.gameObject);
				berserkEffect = null;
			}
			if (thisReplicationIndex <= 1) {
				berserkEffect = (Transform)GameObject.Instantiate (berserkEffectPrefab, transform.position, Quaternion.identity);    
			}

			if (fireEffect != null) {
				Destroy (fireEffect.gameObject);
			}
			fireEffect = GameObject.Instantiate (fireEffectGroup [10], gunpoint.position, invisibleGun.rotation, gunpoint) as GameObject;
			ps_fireEffect = fireEffect.GetComponentInChildren<ParticleSystem> ();
			waitTime = duration;
			if (isLocal) {
				if (!shouldUse) {
					StartCoroutine ("CloseBerserk");
				} else {
					StopCoroutine ("CloseBerserk");
					CompleteBerserkSkill ();
				}
			} else {
				if (shouldUse) {
					StartCoroutine ("CloseBerserk");
				}
			}
			//Invoke ("CompleteBerserkSkill", duration);
			break;
		case SkillType.Summon:
			if (NewcormerMissionPanel._instance != null) {
				if (NewcormerMissionPanel._instance.currentMissionIndex == 4) {
					NewcormerMissionPanel._instance.AddMissionProgress (1);
				}
			}
			break;
		case SkillType.Torpedo:
			torpedoGrids = GameObject.Instantiate (PrefabManager._instance.GetPrefabObj (MyPrefabType.SkillEffect, 5));//鱼雷的网格特效

			ScreenManager.uiScaler.gameObject.GetComponent<GraphicRaycaster> ().enabled = false;
//			isInFire = false;
			isRepareToFireTorpedo = true;
			this.torpedoLevel = torpedoLevel;
			break;
		case SkillType.AutoFire:
			SetAutoFire (shouldUse);
			break;
		case SkillType.Double:
			isDouble = true;
			gunUI.SetDoubleEffectShow (duration);
			break;
		case SkillType.Replication:
			SetGunBase (gunBaseGroup.Length - 1);
			waitTime = duration;
			if (isLocal) {
				if (!shouldUse) {
					StartCoroutine ("CloseReplication");
				} else {
					StopCoroutine ("CloseReplication");
					ReplicationComplete ();
					return;
				}
			} else {
				if (shouldUse) {
					StartCoroutine ("CloseReplication");
				}
			}
              
			if (isLocal) {
				if (isInLockTarget) {
					if (isRepareToAttackLock) {
						PrefabManager._instance.GetSkillUIByType (SkillType.Lock).StopLockSkill (true);
					} else {
						PrefabManager._instance.GetSkillUIByType (SkillType.Lock).StopLockSkill (false);
					}
				}
			}

                //VIP2-VIP3为2重分身，VIP4级以上为3重分身
			if (vipLevel >= 4)
				replicationCount = 3;
			else
				replicationCount = 2; 
               // Debug.LogError("Viplevel="+vipLevel+ " count="+replicationCount);
           
			for (int i = 2; i <= replicationCount; i++) {
				// Debug.LogError("Do copy:"+i);
				GameObject replicatiGun = GameObject.Instantiate (this.gameObject, transform.position, transform.rotation)as GameObject;
				replicatiGun.GetComponent<GunControl> ().SetReplicationGunInfo (i, replicationCount, this, isBerserk);
				if (i == 2) {
					replicationGun2 = replicatiGun;
				} else if (i == 3) {
					replicationGun3 = replicatiGun;
				}
			}
			this.currentLockIndex = 1;
			this.thisReplicationIndex = 1; //使用技能的作为母体进行复制
			if (replicationCount == 2) {
				childGun.transform.position += Vector3.left * 0.5f;
				invisibleGun.transform.position += Vector3.left * 0.5f;
			}
			SetGunSpine (10);
			SetBarbette (0);

			if (isLocal) {
				CancelInvoke ("AutoUseLockSkill");//可能还需要做恢复UI遮罩的处理
				CancelInvoke ("CompleteLockSkill");
				isInLockTarget = true;
				isRepareToAttackLock = false;
				isInFire = true;
			}
			//Invoke ("ReplicationComplete", duration);
			break;
		default:
			break;
		}
	}

	float waitTime = 0;

	IEnumerator CloseBerserk ()
	{
		yield return new WaitForSeconds (waitTime);
		CompleteBerserkSkill ();
	}

	IEnumerator CloseReplication ()
	{
		yield return new WaitForSeconds (waitTime);
		ReplicationComplete ();

	}

	IEnumerator CloseLockAndAudio ()
	{
		yield return new WaitForSeconds (waitTime);
		CompleteLockSkill ();
	}

	void SetReplicationGunInfo (int index, int sumCount, GunControl _motherGun, bool berserkFlag)
	{
		thisReplicationIndex = index;
		isBerserk = berserkFlag;
		if (isBerserk) {
            
		}
		this.replicationCount = sumCount;
		this.motherGun = _motherGun;
		Transform temp = transform.Find ("_Effetc_Buyu_Zhishikuang(Clone)");
		if (temp != null)
			Destroy (temp.gameObject);
		currentLockIndex = 1;
		EnemyBase tempEnemy = UIFishingObjects.GetInstance ().fishPool.GetRepclicationLock (index);
		if (tempEnemy != null)
			lockTarget = tempEnemy.transform.Find ("ChildModel").gameObject;
		SetGunShow (true);
		SetGunSpine (10);
		gunBaseSprite.enabled = false;

		if (_motherGun.isLocal) {
			isInLockTarget = true;
			isRepareToAttackLock = false;
			isInFire = true;
		}

		if (sumCount == 2) {
			childGun.transform.position += Vector3.right * 0.5f;
			invisibleGun.transform.position += Vector3.right * 0.5f;
		} else if (sumCount == 3) {
			switch (index) {
			case 2:
				this.transform.position += Vector3.left;
				break;
			case 3:
				this.transform.position += Vector3.right;
				break;
			default:
				break;
			}
		}
		SetBarbette (0);
	}

	void ReplicationComplete ()
	{
		if (isLocal)
			SetGunBase (0);
		else
			SetGunBase (1);
		if (replicationGun2 != null) {
			replicationGun2.GetComponent<GunControl> ().OtherReplicationComplete ();
			replicationGun2 = null;
		}
            
		if (replicationGun3 != null) {
			replicationGun3.GetComponent<GunControl> ().OtherReplicationComplete ();
			replicationGun3 = null;
		}

		if (replicationCount == 2) {
			childGun.transform.position += Vector3.right * 0.5f;
			invisibleGun.transform.position += Vector3.right * 0.5f;
		}
		if (!useAutoFire)
			isInFire = false;
		replicationCount = 0;
		thisReplicationIndex = 0;
		SetGunSpine (currentGunStyle);
		if (isLocal) {
			SetBarbette (myInfo.cannonBabetteStyle);
		} else {
			SetBarbette (currentBarbetteStyle);
		}
//		SetBarbette (currentBarbetteStyle);
		//SetBarbette (3);
		currentLockIndex = 0;
		CompleteLockSkill ();
	}

	public void OtherReplicationComplete ()
	{ //子物体结束分身
		if (lockUI != null)
			Destroy (lockUI.gameObject);
		Destroy (this.gameObject);
	}


	void AutoUseLockSkill ()
	{
		if (isRepareToAttackLock && lockTarget != null) {
			lockSkill.ActiveSkill ();//正式启用使图标进入cd读条
			Invoke ("CompleteLockSkill", lockDuration);//完成锁定技能后是否也需要上传消息？还是本地获得B用户开始锁定技能后自行判断时间?
			isRepareToAttackLock = false; //此时说明玩家已经选取到一个目标
		}
	}

	public void FireOneTorpedo (Vector3 firePos, int torpedoLevel)//向位置为firePos的地方发射等级为torpedoLevel的鱼雷
	{
		//firePos = new Vector3 (firePos.x, firePos.y, 10f);//由于鱼雷的连续发射废弃掉
		firePos = Vector3.zero;//只向屏幕中心发射鱼雷
		GameObject torpedo = GameObject.Instantiate (PrefabManager._instance.GetPrefabObj (MyPrefabType.SkillEffect, 6),
			                     firePos, Quaternion.identity) as GameObject;
		int tempId = GetTorpedoBulletID ();
		torpedo.GetComponentInChildren<TorpedoBullet> ().SetInfo (tempId, this.userID, torpedoLevel, isLocal);

		int serverSkillId = Skill.GetServerIdFromTorpedoLevel (torpedoLevel);
		if (isLocal) { //只有本地用户发射的鱼雷才会上传服务端，如果是其它用户，只是显示一个效果而已
            
			if (GameController._instance.myGameType == GameType.Classical) {
				UIFishingMsg.GetInstance ().SndLaunchTorpedoRequest (tempId, serverSkillId, firePos.x, firePos.y);
			} else {
				UIFishingMsg.GetInstance ().SndPKLaunchTorpedoRequest (tempId, serverSkillId, firePos.x, firePos.y);
				Debug.LogError ("發送pk魚雷消息:" + tempId + "," + torpedoLevel + "," + firePos.x + "," + firePos.y);
			}

		}

		if (torpedoGrids != null) {
			Destroy (torpedoGrids);
			ScreenManager.uiScaler.gameObject.GetComponent<GraphicRaycaster> ().enabled = true;
			torpedoGrids = null;
		}

		AudioManager._instance.PlayEffectClip (AudioManager.effect_fireTorpedo);
	}

	public void ForceFireTorpedo ()
	{
		isRepareToFireTorpedo = false;
		FireOneTorpedo (Vector3.zero, this.torpedoLevel);
	}

	public void CompleteSkill (SkillType skill)
	{
		switch (skill) {
		case SkillType.Lock:
			CompleteLockSkill ();
			break;
		case SkillType.Freeze:
			break;
		case SkillType.Error:
			break;
		default:
			break;
		}
	}

	public void CompleteLockSkill ()
	{
		lockSkill = null;
		isInLockTarget = false;
		isRepareToAttackLock = true;
		lockTarget = null;
		SetAutoFire (false);
		if (useAutoFire)
			isInFire = true;
		if (lockUI != null) {
			Destroy (lockUI.gameObject);
			lockUI = null;
		}
            
	}

	/// <summary>
	/// 取消所有技能接口
	/// </summary>
	public void CompleteAllSkill ()
	{
		CompleteBerserkSkill ();
		CompleteLockSkill ();
		ReplicationComplete ();

	}

	void CompleteBerserkSkill ()
	{
		isBerserk = false;
		if (fireEffect != null) //狂暴结束后，开火特效会重新生成，需要删除原来的特效
            Destroy (fireEffect.gameObject); 
		if (fireEffectGroup [currentGunStyle] != null) {
			fireEffect = GameObject.Instantiate (fireEffectGroup [currentGunStyle], gunpoint.position, invisibleGun.rotation, gunpoint) as GameObject;
		} else {
			fireEffect = GameObject.Instantiate (fireEffectGroup [0], gunpoint.position, invisibleGun.rotation, gunpoint) as GameObject;
		}
		ps_fireEffect = fireEffect.GetComponentInChildren<ParticleSystem> ();

		if (berserkEffect != null)
			Destroy (berserkEffect.gameObject);
		if (replicationGun2 != null) {
			replicationGun2.GetComponent<GunControl> ().CompleteBerserkSkill ();
		}
		if (replicationGun3 != null) {
			replicationGun3.GetComponent<GunControl> ().CompleteBerserkSkill ();
		}

		berserkEffect = null;
	}


	void SetAutoFire (bool shouldAuto)
	{
		//gunUI.SetTextInfoShow (shouldAuto);
		if (shouldAuto) {
			isInFire = true;
			useAutoFire = true;
		} else {
			isInFire = false;
			fireTimer = 0f;
			useAutoFire = false;
		}
	}


	void DebugTest ()
	{

	}


	public void SetGunSpine (int index)
	{
		//Debug.LogError("SetGunSpine:" + index);
		if (index < 0 || index > 10) {
			index = 0;
		}

		GameObject tempEffect = GameObject.Instantiate (changeGunStyleEffectPrefab, transform.position, Quaternion.identity)as GameObject;
		Destroy (tempEffect, 1f);

		GameObject tempGun = skeletonAnim.gameObject;
		GameObject newGun = GameObject.Instantiate (gunSpineGroup [index], tempGun.transform.position,
			                    tempGun.transform.rotation, tempGun.transform.parent) as GameObject;
		newGun.name = "GunSpine";
		skeletonAnim = newGun.GetComponent<SkeletonAnimation> ();
		gunSpineMeshRender = skeletonAnim.transform.GetComponent <MeshRenderer> ();
		gunSpineMeshRender.sortingOrder = 2;

		bool isBerserk = false;
		if (index == 10) {
			isBerserk = true;
			index = currentGunStyle;
		} else {
			currentGunStyle = index;
		}
       
		Destroy (tempGun.gameObject);

		if (fireEffect != null) {
			Destroy (fireEffect.gameObject);
		}

		if (isBerserk) {
			fireEffect = GameObject.Instantiate (fireEffectGroup [10], gunpoint.position, invisibleGun.rotation, gunpoint) as GameObject;
		} else {
			if (fireEffectGroup [index] != null) {
				fireEffect = GameObject.Instantiate (fireEffectGroup [index], gunpoint.position, invisibleGun.rotation, gunpoint) as GameObject;
			} else {
				fireEffect = GameObject.Instantiate (fireEffectGroup [0], gunpoint.position, invisibleGun.rotation, gunpoint) as GameObject;
			}
		}
		ps_fireEffect = fireEffect.GetComponentInChildren<ParticleSystem> ();

		if (isLocal) {
			myInfo.cannonStyle = index + 3000;
		}

	}
    //射速 不同炮座的射速
	public void SetBarbette (int index)
	{
		if (index < 0 || index > 3) {
			index = 0;
		}
		switch (index) {
		case 1:
			    barbetteSprite.sprite = barbetteGroup [index - 1];
                //fireRateTime = .25f;
                fireRateTime = 0.166f;//每秒6发
			break;
		case 2:
			    barbetteSprite.sprite = barbetteGroup [index - 1];
			    //fireRateTime = .2f;
                fireRateTime = 0.142f;//每秒7发
			break;
		case 3:
			    barbetteSprite.sprite = barbetteGroup [index - 1];
                //fireRateTime = .142f;//
                fireRateTime = 0.125f;//每秒8发
			break;
		default:
			break;
		}
		if (index == 0) {
			barbetteSprite.gameObject.SetActive (false);
		} else {
			barbetteSprite.gameObject.SetActive (true);
		}
	}

	void PlayAttackAnim ()
	{
		//spineAnim.AnimationName = "attack";
		skeletonAnim.state.SetAnimation (0, "attack", false);
	}

	void SetLocalAvator ()
	{
		if (myInfo.avatar != null && myInfo.avatar != "") {
			AvatarInfo nInfo = (AvatarInfo)Facade.GetFacade ().data.Get (FacadeConfig.AVARTAR_MODULE_ID);
			nInfo.Load (myInfo.userID, myInfo.avatar, OnRecvAvatarResponse);

			//	HintText._instance.ShowHint ("开始读取自己头像");
		}
	}

	void OnRecvAvatarResponse (int nResult, Texture2D nImage)
	{
		if (nResult == 0) {
			nImage.Compress (true);
			avatorSprite = Sprite.Create (nImage, new Rect (0, 0, nImage.width, nImage.height), new Vector2 (0, 0));
			if (GameController._instance.myGameType != GameType.Classical) {
				gunUI.SetPkAvator (avatorSprite);
             
			}

		} else {
			// gunUI.SetAvator(defaultAvatorSprite);
          
			Debug.LogError ("Error! Can't load localAvator");
		}
	}

	void SetOtherAvator (string avatorStr)
	{
		//HintText._instance.ShowHint ("开始读取他人头像");
		AvatarInfo nInfo = (AvatarInfo)Facade.GetFacade ().data.Get (FacadeConfig.AVARTAR_MODULE_ID);
		//Debug.LogError("LoadOtherAvator:"+this .userID+ "str="+avatorStr);
		nInfo.Load (this.userID, avatorStr, OnRecvOtherAvatarResponse);
	}

	void OnRecvOtherAvatarResponse (int nResult, Texture2D nImage)
	{
		//Debug.LogError("RecvOtherAvatorResult="+nResult);
		if (nResult == 0) {
			nImage.Compress (true);
			avatorSprite = Sprite.Create (nImage, new Rect (0, 0, nImage.width, nImage.height), new Vector2 (0, 0));
			if (GameController._instance.myGameType != GameType.Classical) {
				gunUI.SetPkAvator (avatorSprite);
				if (GameController._instance.myGameType == GameType.Point) {
					PkPointProgessBar._instance.InitData (thisSeat, userInfo.nickName, gender, avatorSprite);
				}
			}
			//HintText._instance.ShowHint ("他人头像设置完毕");
		} else {
			// gunUI.SetAvator(defaultAvatorSprite);
			avatorSprite = defaultAvatorSprite;
			if (GameController._instance.myGameType == GameType.Point) {
				PkPointProgessBar._instance.InitData (thisSeat, userInfo.nickName, gender, defaultAvatorSprite);
			}
			Debug.LogError ("Error! Can't load otherAvator");
		}
	}

	public Sprite defaultAvatorSprite;

	public Sprite GetAvatorSprite ()
	{
		if (avatorSprite != null)
			return avatorSprite;
		else
			return defaultAvatorSprite;
	}

	IEnumerator ChangeImage (string path)
	{
		WWW www = new WWW (path);
		yield return www;
		Texture2D image = www.texture;
		myInfo.headTexture = image;
		GetMyHead (image);
	}

	public void GetMyHead (Texture2D image)
	{
		image.filterMode = FilterMode.Bilinear;
		image.Compress (true);
		gunUI.SetPkAvator (Sprite.Create (image, new Rect (0, 0, image.width, image.height), new Vector2 (0, 0)));
	}



	void HangUpCheck ()
	{
		if (GameController._instance.isExperienceMode) {
			return;
		}
		hangUpTimer -= 10;
		//Debug.LogError ("hangUpTimer=" + hangUpTimer);
		if (hangUpTimer == 0) {
			GameObject temp = GameObject.Instantiate (ScreenManager.uiScaler.transform.Find ("BackBtn").GetComponent<GameBackBtn> ().backConfirmPanel);
			temp.transform.parent = ScreenManager.uiScaler.transform;
			temp.transform.position = Vector3.zero;
			temp.transform.localScale = Vector3.one;
			temp.GetComponent<AskBackPanel> ().Show ("長時間未發射子彈，已退出遊戲。", true);
		}
	}

	bool FitMultiple ()
	{ //当前炮倍数小于金币时，自定降低炮倍数
		return false;//暂时屏蔽适配功能
		int result = cannonMultiple;
		bool success = false;
		for (int i = gunUI.GetMultiplesUnlockIndex (cannonMultiple); i >= 0; i--) {  //获取当前炮倍数，然后慢慢减少炮倍数直到找到匹配的
			if (isBerserk) {
				if (GunInUI.multipleGroup [i] * 2 <= currentGold) {
					gunUI.SetMultiple (GunInUI.multipleGroup [i]);
					success = true;
					break;
				}
			} else {
				if (GunInUI.multipleGroup [i] <= currentGold) {
					gunUI.SetMultiple (GunInUI.multipleGroup [i]);
					success = true;
					break;
				}
			}

		}
		return success;
	}


	public GunInUI GetOtherGunUI ()
	{
		if (otherGunUI == null) {
			if (thisSeat == GunSeat.LB) {
				otherGunUI = PrefabManager._instance.GetPrefabObj (MyPrefabType.GunInUI, (int)GunSeat.RB).GetComponent<GunInUI> ();
			} else if (thisSeat == GunSeat.RB) {
				otherGunUI = PrefabManager._instance.GetPrefabObj (MyPrefabType.GunInUI, (int)GunSeat.LB).GetComponent<GunInUI> ();
			}
		}
		return otherGunUI;
	}

	public void DoStrecthScale ()
	{

		childGunAnimator.SetTrigger ("scaleTrigger");
	}

	public void SetGunBase (int index)
	{
		if (index >= gunBaseGroup.Length) {
			index = 0;
		}
		gunBaseSprite.sprite = gunBaseGroup [index];
	}

	public  int goldCost;

	public bool CheckCanFire ()
	{
		if (GoldTip != 0) {
			GoldTip = 0;
			return false;
		}
		SetCheckCanFire (cannonMultiple);
		if (currentGold >= goldCost)
			return true;
		else
			return false;
	}

    //修改在漁場中更換砲倍數，修改消耗金幣的算法
	void SetCheckCanFire (int cannonMultipleTemp)
	{
		Debug.Log(" 修改炮倍數消耗計算  cannonMultipleTemp = " + cannonMultipleTemp);
		Debug.Log(" 修改炮倍數消耗計算 roomMultiple = " + myRoomInfo.roomMultiple);
        //switch (myRoomInfo.roomMultiple)
        //{
        //    case 0:
        //        Debug.Log("0 isBerserk = " + isBerserk);
        //        break;
        //    case 1:
        //        Debug.Log("1 isBerserk = " + isBerserk);
        //        goldCost = isBerserk ? cannonMultipleTemp * 2 : cannonMultipleTemp;
        //        break;
        //    //三倍场
        //    case 2:
        //        Debug.Log("2 isBerserk = " + isBerserk);
        //        goldCost = isBerserk ? cannonMultipleTemp * 2 * 3 : cannonMultipleTemp * 3;
        //        break;
        //    //五倍场
        //    case 3:
        //        Debug.Log("3 isBerserk = " + isBerserk);
        //        break;
        //    case 5:
        //        Debug.Log("5 isBerserk = " + isBerserk);
        //        goldCost = isBerserk ? cannonMultipleTemp * 2 * 5 : cannonMultipleTemp * 5;
        //        break;
        //    //体验场
        //    case 4:
        //        Debug.Log("4 isBerserk = " + isBerserk);
        //        goldCost = isBerserk ? cannonMultipleTemp * 2 : cannonMultipleTemp;
        //        break;
        //    default:
        //        break;
        //}

        switch (myRoomInfo.roomMultiple)
        {
            case 0:
                Debug.Log("0 isBerserk = " + isBerserk);
                //goldCost = isBerserk ? cannonMultipleTemp * 2 * 3 : cannonMultipleTemp * 3;
                break;
            case 1:
                Debug.Log("1 isBerserk = " + isBerserk);
                goldCost = isBerserk ? cannonMultipleTemp * 2 * 5 : cannonMultipleTemp * 5;
                break;
            //三倍场
            case 2:
                Debug.Log("2 isBerserk = " + isBerserk);
                goldCost = isBerserk ? cannonMultipleTemp * 2 * 10 : cannonMultipleTemp * 10;
                break;
            //五倍场
            case 3:
                Debug.Log("3 isBerserk = " + isBerserk);
                goldCost = isBerserk ? cannonMultipleTemp * 2 * 20 : cannonMultipleTemp * 20;
                break;
            //体验场
            case 4:
                Debug.Log("4 isBerserk = " + isBerserk);
                goldCost = isBerserk ? cannonMultipleTemp * 2 : cannonMultipleTemp;
                break;
            case 5:
                Debug.Log("5 isBerserk = " + isBerserk);
                goldCost = isBerserk ? cannonMultipleTemp * 2 * 20 : cannonMultipleTemp * 20;
                break;
            case 6:
                Debug.Log("6 isBerserk = " + isBerserk);
                goldCost = isBerserk ? cannonMultipleTemp * 2 * 3 : cannonMultipleTemp * 3;
                break;
            default:
                break;
        }
        Debug.Log(" 修改炮倍數消耗計算 " + goldCost);
	}

	public void SetBankruptcyState (bool state)
	{
		isBankruptcy = state;

		gunUI.SetBankruptcyShow (state); 
		if (state) {
			if (thisReplicationIndex == 1) {
				if (replicationGun2 != null) {
					GunControl gun2 = replicationGun2.GetComponent<GunControl> ();
					gun2.isBankruptcy = true;
					gun2.isInFire = false;
				}
				if (replicationGun3 != null) {
					GunControl gun3 = replicationGun3.GetComponent<GunControl> ();
					gun3.isBankruptcy = true;
					gun3.isInFire = false;
				}
			}
		} else {
			if (thisReplicationIndex == 1) {
				if (replicationGun2 != null) {
					GunControl gun2 = replicationGun2.GetComponent<GunControl> ();
					gun2.isBankruptcy = false;
					gun2.isInFire = true;
				}
				if (replicationGun3 != null) {
					GunControl gun3 = replicationGun3.GetComponent<GunControl> ();
					gun3.isBankruptcy = false;
					gun3.isInFire = true;
				}   
			}
		}
	}


	void CheckSummonRobot ()
	{
		//return;
		if (GameController._instance.isExperienceMode) {
			return;
		}
		if (PrefabManager._instance.GetActiveGunCount () == 1) {
			int summonNum = 1;
			int randomNum = Random.Range (0, 100);
			if (randomNum > 60)
				summonNum = 2;
			SummonRobot (summonNum);
		}
	}

	void SummonRobot (int num)
	{
		FishingCommonMsgHandle temp = new FishingCommonMsgHandle ();
//		Debug.Log ("SummonRobotNum=" + num);
		for (int i = 0; i < num; i++) {
			temp.SendGetRobotRequest ();
		}
            
	}


	public void ActiveRobotMode ()
	{
//		Debug.Log ("BiBiBi————" + userID);
		isRobot = true;
		//UIFishingMsg.GetInstance().SndFireBullet()
//		UpdateRandomTargetPos ();
//		InvokeRepeating ("RobotSndFireOneBullet", 0.2f, 0.27f);
//		float randomNum = Random.Range (4f, 12f);
//		InvokeRepeating ("UpdateRobotRandomState", randomNum, 16.31f); 
		//Invoke("RobotSendLeaveSelf", 3f);//debugTest
	}

	void RobotSndFireOneBullet ()
	{
        
		//每次要发子弹前，有10%的概率不开炮，%20的概率转向
		int randomNum = Random.Range (0, 100);
		if (randomNum <= 20)
			UpdateRandomTargetPos ();
		if (randomNum >= 90)
			return;
        
		int enemyGroupId = -1;
		int enemyId = -1;

		if (robotIsLock) {
			EnemyBase tempEnemy = UIFishingObjects.GetInstance ().fishPool.GetAim ();
			if (tempEnemy != null) {
				enemyGroupId = tempEnemy.groupID;
				enemyId = tempEnemy.id;
				//Debug.LogError("robotlockEnemy:"+tempEnemy.name);
			}
		} else if (robotIsRepcication) {
           
			EnemyBase tempEnemy = UIFishingObjects.GetInstance ().fishPool.GetRepclicationLock (userID % 2);
			if (tempEnemy != null) {
				enemyGroupId = tempEnemy.groupID;
				enemyId = tempEnemy.id;
				//Debug.LogError("robotlockEnemy:"+tempEnemy.name);
			}
		}

       
		UIFishingMsg.GetInstance ().SndFireBullet (this.userID, GetBulletID (), targetPos.x, targetPos.y, cannonMultiple, robotIsBerserk, enemyGroupId, enemyId);
	}

	void UpdateRandomTargetPos ()
	{
		float randomX = Random.Range (-6f, 6f);
		float randomY = Random.Range (-3f, 3f);
		targetPos = new Vector3 (randomX, randomY, transform.position.z);
	}

	bool robotIsBerserk = false;
	bool robotIsLock = false;
	bool robotIsRepcication = false;

	void UpdateRobotRandomState ()
	{
		int randomNum = Random.Range (0, 100);
		//  Debug.LogError("robotRandomStateNum="+randomNum);
		int skillId;
		if (randomNum > 0 && randomNum <= 25) { //使用锁定25
			if (!robotIsLock && !robotIsRepcication) { //如果没有在锁定状态中才使用，已经在锁定了就不进行任何操作　
				//Debug.LogError("RobotUseLock:" + gunUI.GetNickName());
				skillId = PrefabManager._instance.GetSkillUIByType (SkillType.Lock).serverTypeId;
				UIFishingMsg.GetInstance ().SndEffectRequest (skillId, this.userID);
				robotIsLock = true;
				Invoke ("ResetRobotLockState", 100f);
			}
		} else if (randomNum > 25 && randomNum <= 50 && !robotIsBerserk) { //使用狂暴25
            
			skillId = PrefabManager._instance.GetSkillUIByType (SkillType.Berserk).serverTypeId;
			UIFishingMsg.GetInstance ().SndEffectRequest (skillId, this.userID);
			CancelInvoke ("RobotSndFireOneBullet");
			InvokeRepeating ("RobotSndFireOneBullet", 0.27f, 0.27f * 0.8f);
			robotIsBerserk = true;
			Invoke ("ResetRobotFireRateTime", 30f);

		} else if (randomNum > 50 && randomNum <= 65) { //使用分身15
			if (!robotIsLock && !robotIsRepcication) {
                
				robotIsRepcication = true;
				CancelInvoke ("RobotSndFireOneBullet");
				float speedChange = vipLevel >= 4 ? 0.33f : 0.5f;
				InvokeRepeating ("RobotSndFireOneBullet", 0.27f, 0.27f * speedChange);
				Invoke ("ResetRobotReplicationState", 30f);


				skillId = PrefabManager._instance.GetSkillUIByType (SkillType.Replication).serverTypeId;
				UIFishingMsg.GetInstance ().SndEffectRequest (skillId, this.userID);   
			}
		} else if (randomNum > 65 && randomNum <= 75) { //使用召唤10
			//Debug.LogError("RobotUseSummon");
			skillId = PrefabManager._instance.GetSkillUIByType (SkillType.Summon).serverTypeId;
			UIFishingMsg.GetInstance ().SndEffectRequest (skillId, this.userID);
		} else if (randomNum > 75 && randomNum <= 85) { //切换炮台倍数10
			int finalMul = cannonMultiple;
			finalMul = gunUI.GetFinalMultiple (false);
			//Debug.LogError("RobotFinalMul=" + finalMul);
			UIFishingMsg.GetInstance ().SndChangeCannonMultiple (finalMul, userID);
		} else if (randomNum > 85 && randomNum <= 90) { //切换炮台样式5
            
			if (vipLevel > 0 && !robotIsRepcication) {
				int randomGunSpineIndex = Random.Range (0, vipLevel);
				// SetGunSpine(randomGunSpineIndex);

				FishingCommonMsgHandle temp = new FishingCommonMsgHandle ();
				temp.SendChangeCannonStyleRequest (randomGunSpineIndex + 3000, this.userID);
			}

		} else if (randomNum > 90 && randomNum <= 100) { //离开房间10
			if (PrefabManager._instance.GetActiveGunCount () >= 4) { //如果此时房间人满，则机器人自己离开
				RobotSendLeaveSelf ();
			}
		}
	}

	void ResetRobotFireRateTime ()
	{
		CancelInvoke ("RobotSndFireOneBullet");
		InvokeRepeating ("RobotSndFireOneBullet", 0.27f, 0.27f);
		robotIsBerserk = false;
	}

	void ResetRobotLockState ()
	{
		robotIsLock = false;
	}

	void ResetRobotReplicationState ()
	{
		// Debug.LogError("ResetRobotReplicaitonState");
		robotIsRepcication = false;
		CancelInvoke ("RobotSndFireOneBullet");
		InvokeRepeating ("RobotSndFireOneBullet", 0.27f, 0.27f);
	}


	public void RobotSendLeaveSelf ()
	{
		Debug.LogError ("RobotSendLeaveSelf");
//		CancelInvoke ("RobotSndFireOneBullet");
//		if (robotIsBerserk)
//			CancelInvoke ("ResetRobotFireRateTime");
		GameObject[] bulletGroups = GameObject.FindGameObjectsWithTag (TagManager.bullet);
		if (bulletGroups.Length > 0) {
			for (int i = 0; i < bulletGroups.Length; i++) {
				if (bulletGroups [i].GetComponent<Bullet> ().userID == this.userID) {
					bulletGroups [i].GetComponent<Bullet> ().isMyRobot = false;
				}
			}
		}
		Invoke ("DelayRobotSendLeaveSelf", .2f);
	}

	void DelayRobotSendLeaveSelf ()
	{
		FishingCommonMsgHandle temp = new FishingCommonMsgHandle ();
		temp.SendReturnRobotRequest (this.userID);
	}

	void xxxx ()
	{
		//		if (isRobot) {
		//			if (robotIsRepcication) {
		//				EnemyBase temp = UIFishingObjects.GetInstance ().fishPool.GetRepclicationLock (thisReplicationIndex);
		//				if (temp.thisState == EnemyBase.EnemyState.Dead || ScreenManager.IsInScreen (lockTarget.transform.position) == false) {
		//					Debug.LogError ("是否执行 robotIsRepcication = " + robotIsRepcication);
		//					//			EnemyBase tempEnemy = UIFishingObjects.GetInstance ().fishPool.GetRepclicationLock (userID % 2);
		//					FiProperty property;
		//					if (temp != null) {
		//						Debug.LogError ("tempEnemy.name = " + temp.name);
		//						//				enemyGroupId = tempEnemy.groupID;
		//						//				enemyId = tempEnemy.id;
		//						for (int i = 0; i < replicationCount; i++) {
		//							property = new FiProperty ();
		//							property.value = temp.groupID;
		//							property.type = temp.id;
		//
		//							if (!propertyList.Contains (property)) {
		//								propertyList.Add (property);
		//							}
		//
		//							Debug.LogError ("机器人分身 property.value = " + property.value);
		//							Debug.LogError ("机器人分身 property.type = " + property.type);
		//							Debug.LogError ("机器人数量 propertyList.Count = " + propertyList.Count);
		//						}
		//						Debug.LogError ("robotlockEnemy:" + temp.name);
		//						Facade.GetFacade ().message.fishCommom.SendRobotReplicationRequest (userID, propertyList);
		//					}
		//				}
		//			}	
		//		}
	}
}
