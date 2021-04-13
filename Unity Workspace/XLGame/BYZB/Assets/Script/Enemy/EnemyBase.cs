using UnityEngine;
using System.Collections;
using DG.Tweening;
using System.Collections.Generic;
using AssemblyCSharp;

public enum FormationType
{
	// 鱼群的队形
	None,
	//非群组的
	Linear,
	//单直线
	Bilinear,
	//双直线
	Normal,
	//最常用的阵型，同组内的鱼都基于一个父节点移动
	Circle
	//圈型
}

public class EnemyBase : MonoBehaviour
{

	public enum EnemyState
	{
		Idle,
		Move,
		Hitted,
		Dash,
		//受伤后有一定概率触发冲刺游
		Dead
	}

	[HideInInspector]
	public EnemyState thisState = EnemyState.Idle;

	public Color hitColor = Color.red;
	//组件获取
	public GameObject childModel;
	GameObject coinsEffectPrefab;
	BoxCollider childCollider;

	//轨迹控制
	Transform trackTrans;
	MyTrack myTrack;
	int trackType = -1;
	int trackID;
	[HideInInspector]public  bool hasInScreen = false;
	//判断是否已经游入屏幕

	//动画控制
	Animator childAnimator;
	AnimatorStateInfo animatorInfo;

	//皮肤相关设置
	SkinnedMeshRenderer skinRenderer;
	Color defaultSkinColor;

	//行为参数设置

	public int groupID;
	//鱼群Id
	public int id = 0;
	//服务器控制，每条鱼唯一一个id，同种类的鱼之间id也不同
	public int goldReturn = 0;
	//鱼死后自身返还的金币值
	public int typeID;
	//鱼的种类id，例如乌龟的id都为8，小丑鱼的id都为9
	public bool isGrouped = false;
	//最后根据id判断是否为结队游的鱼群
	public float intervalTime = 0.6f;

	float originMoveSpeed;
	//在初始movespeed一定随机范围内生成的速度，作为移动的初始速度
	float moveSpeed = 10f;
	//实际游动速度

	//int coinReturn=100; //返还金币
	float deadTimer = 0;
	//用作死亡状态持续时间最小值的计时器
	public bool isBonus = false;
	//是否是奖金鱼
	public int groupLength = 1;
	//群组鱼生成时拥有的数量

	FishPool fishPool = null;

	//用于控制竖直方向的模型旋转
	Vector3 currentPos;
	Vector3 lastPos;
	Vector3 deltaPos;

	GameObject iceBlock = null;
	//被冰冻时生成的冰块
	bool isFrozen = false;
	//是否在冰冻状态中

	//阵型控制
	public  FormationType formation = FormationType.None;
	Transform formationParent;
	//阵型的父节点
	float linearDistance = 3f;
	//线性阵列时前后鱼之间的距离差
	GameObject newTrack = null;

	//语音
	int speakProbability = 2;
	int speakProbabilityInBoss = 1;
	int speakPlay = 0;
	//代表受伤后触发语音概率10%
	public AudioClip normalVoice;
	public AudioClip deadVoice;

	public bool isBoss = false;

	bool leaveScreenFlag = false;

	public int fishTideType = -1;
	bool torpedoHit = false;

	public Transform bubblePos = null;
	GameObject bubbleBox = null;

	float awayDistance = 1.5f;
	RoomInfo myRoomInfo = null;

	void Awake ()
	{
		//fishPool = UIFishingMsg.GetInstance ().fishPool;
		fishPool = UIFishingObjects.GetInstance ().fishPool;
		myRoomInfo = DataControl.GetInstance ().GetRoomInfo ();
	}

	void Start ()
	{
        
		Init ();
		StartMove ();
		// isBonus = true; ;//debugTest
		return;
		if (isBoss && (!GameController._instance.isBossMode && !GameController._instance.isBossMatchMode && myRoomInfo.roomMultiple != 2)) {
			PrefabManager._instance.PlayBossComingEffect (goldReturn, typeID);
			awayDistance = 3f;
			//speakProbability = (int) (speakProbability*0.5f);
		}
		//PlayDeadVoice (); //测试死亡声音
	}

	void Init ()
	{
        
		thisState = EnemyState.Idle;	
		originMoveSpeed = moveSpeed;  //记录最初生成的速度，方便加速减速后的速度还原
		skinRenderer = this.GetComponentInChildren<SkinnedMeshRenderer> ();
		defaultSkinColor = skinRenderer.materials [0].color; //获取材质初始颜色，方便受伤出现红光后还原

		deadTimer = 0;

		if (null == childModel) {
			childModel = transform.Find ("ChildModel").gameObject;	
		}
		childModel.SetActive (false);
		if (GameController._instance.isOverTurn) {
			if (typeID == 12)//水母
                childModel.transform.localScale = new Vector3 (1, 1, -1);
		}
		childCollider = childModel.GetComponent<BoxCollider> ();
		//childCollider= this.GetComponentInChildren<BoxCollider> ();
		if (null == childCollider) {
			childCollider = this.GetComponent<BoxCollider> ();
			if (null == childCollider)
				Tool.OutLogWithToFile ("No collider:" + this.name);
		}
		lastPos = childModel.transform.position;
		childAnimator = transform.GetComponentInChildren<Animator> ();

		coinsEffectPrefab = PrefabManager._instance.GetPrefabObj (MyPrefabType.CoinGroup, 0);

		Invoke ("ForceSetInScreen", 120f);

		if (isBonus) {
			string finalStr = "不要猶豫，向我開砲！";

			if (isBoss) {
				switch (typeID) {
				case 19:
					finalStr = "我還能吃下一隻磷蝦！";
					break;
				case 20:
					finalStr = "潮汐湧動，巨變將至！";
					break;
				case 21:
					finalStr = "我來自大海，狩獵開始了！";
					break;
				case 22:
					finalStr = "身攜巨款，壓力山大！";
					break;
				case 23:
					finalStr = "唱響海妖之歌！";
					break;
				case 24:
					finalStr = "三叉戟在渴望戰鬥！";
					break;
				}
				CreateBubbleBox (finalStr);
			} else {
				if (Random.Range (0, 100) > 90) {
					string[] strGroup = new string[] {"高倍炮更容易掉落魚雷哦！", "使用鎖定更容易抓到我哦！", "狂暴可以提高捕獲概率！", "捕獲我可以獲得更多獎勵！",
                                                        "我是華麗的獎金魚！", "不要猶豫，向我開砲！", "一分子彈，一分收穫！", "水里的朋友們餓著呢！"
					};
					finalStr = strGroup [Random.Range (0, strGroup.Length - 1)];
					CreateBubbleBox (finalStr);
				}
			}


		}
	}

	void CreateBubbleBox (string str)
	{
		if (bubblePos == null)
			bubblePos = childModel.transform.Find ("BubblePos");
		if (bubblePos == null) {
			Debug.LogError ("bubble null:" + this.name);
			return;
		}
		GameObject tempObj = GameObject.Instantiate (PrefabManager._instance.bubbleBoxPrefab, bubblePos.position, Quaternion.identity) as GameObject;
		tempObj.transform.parent = bubblePos;
		tempObj.GetComponentInChildren<TextMesh> ().text = str;
		//  tempObj.transform.localScale = Vector3.one;
		bubbleBox = tempObj;
		Destroy (tempObj, 10f);
	}

	void ForceSetInScreen () //强制设置在屏幕内，为了修正一部分enemy无法正确判断是否在屏幕内的bug
	{
		if (!hasInScreen)
			hasInScreen = true;
        
	}

	public void SetEnemyInfo (int trackType, int trackID, int enemyTypeID, int groupId, int enemyID, FormationType formation = FormationType.None,
	                          int groupLength = 1, int fishTide = -1, float intervalTime = 0.6f)
	{
		this.trackType = trackType;
		this.trackID = trackID;
		this.typeID = enemyTypeID;
		this.groupID = groupId;
		this.id = enemyID;
		this.formation = formation;
		this.groupLength = groupLength;
		this.fishTideType = fishTide;
		this.intervalTime = intervalTime;
		//Debug.LogError("trackType="+trackType +" trackID="+trackID+ " enemyID="+enemyID );
	}


	// Update is called once per frame
	void Update ()
	{
		if (bubbleBox != null) {
			bubbleBox.transform.LookAt (bubbleBox.transform.position + Vector3.forward);
		}
		switch (thisState) {
		case EnemyState.Idle:
			break;
		case EnemyState.Move:
			if (leaveScreenFlag) {
				transform.Translate (childModel.transform.forward * moveSpeed * Time.deltaTime);
			}
			break;
		case EnemyState.Hitted:
			break;
		case EnemyState.Dash:
			if (leaveScreenFlag) {
				transform.Translate (childModel.transform.forward * moveSpeed * Time.deltaTime);
			}
			animatorInfo = childAnimator.GetCurrentAnimatorStateInfo (0); //检测冲刺动画是否播放完毕，若完毕，返回普通移动状态
			if (animatorInfo.IsName ("Dash")) {
				if (animatorInfo.normalizedTime > 1.0f) { 
					thisState = EnemyState.Move;
				}
			}
			break;
		case EnemyState.Dead: //原先采用根据死亡动画进度来判断销毁时间，现在改为死亡后全部固定一个时间后销毁
			//animatorInfo = childAnimator.GetCurrentAnimatorStateInfo (0);
			//deadTimer += Time.deltaTime;
			//if (animatorInfo.IsName ("Die")) {
				//if (animatorInfo.normalizedTime > 0.95f&&deadTimer >1.8f) { //检测死亡动画是否播放完毕	，若完毕，则销毁或者放回对象池
					//DestorySelf ();
				//}
			//}
			//皮肤变暗的代码，很奇怪有一定概率会无法生效，而且这个概率还不小
			skinRenderer.materials [0].color = Color.Lerp (skinRenderer.materials [0].color, Color.clear, Time.deltaTime * 1f);

			break;
		}
	}


	int errorIndex = 0;

	void FixedUpdate ()
	{
		FixedRotation ();//竖直方向的旋转控制，可能会比较耗性能，所以放在FixedUpdate里
		if (hasInScreen) {//检测是否在屏幕内，同样因为耗性能的关系，放在FixedUpdate里
			if (ScreenManager.IsAwayFromScreen (currentPos, awayDistance)) {
				
				if (groupLength == 1) { //此时为静态轨迹上的鱼
//					Debug.LogError ("FishOutGroup:" + this.groupID+","+this.id+"/"+groupLength);
					ResetPath ();
					UIFishingMsg.GetInstance ().SndFishOut (this.groupID);
					errorIndex++;

				} else { 
					if (this.id != 0) {//这部分鱼的轨迹都是动态生成的
						//在SndFishOut里会判断，该id的鱼是不是组里剩下的最后一条鱼，如果是，则向服务器发送该组鱼全部游出屏幕的信息
						UIFishingMsg.GetInstance ().SndFishOut (this.groupID, this.id);
						myTrack.DoKillPath ();
						Destroy (trackTrans.gameObject);
						errorIndex++;
					} else {
						ResetPath (); //群组鱼中id=0的鱼是在静态轨迹上的，所以游出屏幕时需要重置轨迹
						UIFishingMsg.GetInstance ().SndFishOut (this.groupID, 0);
						errorIndex++;
					}
				}
				if (errorIndex >= 5) {
					Debug.LogError ("Error!" + " groupId:" + this.groupID + " fishId:" + this.id +
					"track:" + trackType + "," + trackID + "length:" + this.groupLength);
				}	
				if (null != fishPool) {
					//fishPool.Remove (groupID, id);
					if (!fishPool.Remove (this.groupID, this.id)) {
				
						Tool.OutLogToFile ("Remove Failed! groupId:" + this.groupID + " fishId:" + this.id +
						"track:" + trackType + "," + trackID + "length:" + this.groupLength);
						Tool.LogWarning ("Remove Failed! groupId:" + this.groupID + " fishId:" + this.id +
						"track:" + trackType + "," + trackID + "length:" + this.groupLength, true);
						
						Destroy (this.gameObject);
					}
					//Destroy (this.gameObject);
				}
			}
		} else if (ScreenManager.IsInScreen (currentPos)) {
			hasInScreen = true;
		}
	}

	void FixedRotation ()
	{
		currentPos = childModel.transform.position;
		deltaPos = currentPos - lastPos;

		lastPos = currentPos;

		childModel.transform.LookAt (currentPos + deltaPos);
	}


	public void Hitted (int userID, bool shouldDead = false, List<FiProperty> array = null, bool torpedoHit = false)
	{
		//Debug.Log("打到"+userID);
		if (thisState == EnemyState.Dead)
			return;
		ShowRedSkin (0.2f);
		if (shouldDead) {
			this.torpedoHit = torpedoHit;
			Dead ();

			DoReward (userID, array);
		} else {  //受伤后未死亡情况下，有几率触发dash动画并播放语音
			//普通场为2% boss场为1%     
			//         if (Random.Range (0, 100) < speakProbability) {
			//	thisState = EnemyState.Dash;
			//	childAnimator.SetTrigger ("dashTrigger");
			//	PlayNormalVoice ();
			//}
			int rangeNum = Random.Range (0, 100);
			if (GameController._instance.isBossMode || GameController._instance.isBossMatchMode) {
//                if (Random.Range(0, 100) < speakProbabilityInBoss)
//                {
//                    speakPlay++;
//                    if (speakPlay >= 2 )
//                    {
//                        thisState = EnemyState.Dash;
//                        childAnimator.SetTrigger("dashTrigger");
//                        PlayNormalVoice();
//                        speakPlay = 0;
//                    }
//                }
			} else {
				if (Random.Range (0, 100) < speakProbability) {
					speakPlay++;
					if (speakPlay >= 2) {
						thisState = EnemyState.Dash;
						childAnimator.SetTrigger ("dashTrigger");
						PlayNormalVoice ();
						speakPlay = 0;
					}
				}
			}
		}

	}

	void DoReward (int userId, List<FiProperty> array)
	{
		if (null == array)
			return;
//        DropOutTorpedo(userId,array);
		int sumSkillDrop = 0;
		int fishGold = 0;
		foreach (FiProperty property in array) {
			if (null != property) {
				switch (property.type) {
				case FiPropertyType.GOLD: //加金币
					ReturnCoin (property.value, userId);
					fishGold = property.value;
					if (isBoss) {
						string userName = PrefabManager._instance.GetGunByUserID (userId).gunUI.GetNickName ();
						PrefabManager._instance.ShowBossDeadHintPanel (this.typeID, userName, property.value);
					}
                       // Tool.OutLogWithToFile("GoldGet=" + property.value);
                        //测试代码
					//ReturnDiamond (Random.Range(2,5), userId);

                      // GameObject dropSkillObj2 = PrefabManager._instance.GetSkillUIByServerId (FiPropertyType.TORPEDO_BRONZE).gameObject;
					//GameObject temp2 = GameObject.Instantiate (dropSkillObj2, transform.position, Quaternion.identity) as GameObject;
				//	temp2.GetComponent<Skill> ().DropAndMove (property.value, userId,transform.position+Vector3.one*sumSkillDrop*0.5f);
					//sumSkillDrop++;
                        //测试代码
					break;
				case FiPropertyType.G_POINT:
					
					ReturnCoin (property.value, userId);

					//Debug.LogError ("Point:" + property.value);
					break;
				case FiPropertyType.DIAMOND:
					//Debug.LogError ("Get diamond drop");
					ReturnDiamond (property.value, userId);
					AudioManager._instance.PlayEffectClip (AudioManager.effect_getItem);
					break;
				case FiPropertyType.EXP:
//				    Debug.LogError ("GetExp:" + property.value);
					break;	
				case FiPropertyType.LUCKDRAW_GOLD: //奖金鱼注入奖池的金币,只有自己打死的时候才能触发
					if (userId == DataControl.GetInstance ().GetMyInfo ().userID) {
						//Debug.LogError("LuckDrawGold=" + property.value + " luckyFishNum=" + DataControl.GetInstance().GetMyInfo().loginInfo.luckyFishNum);
						DataControl.GetInstance ().GetMyInfo ().loginInfo.luckyGold += property.value;
						DataControl.GetInstance ().GetMyInfo ().loginInfo.luckyFishNum++;
						if (LuckDrawCanvasScr.Instance != null) {
							LuckDrawCanvasScr.Instance.UpdateData ();
						}
						if (LuckDrawHintBoard._instace != null) {
							LuckDrawHintBoard._instace.UpdateData ();
							LuckDrawHintBoard._instace.ShowPop ();
						}
					}
                       
					break;
				//体验场积分金币
				case FiPropertyType.TEST_GOLD:
					ReturnCoin (property.value, userId);
					if (isBoss) {
						string userName = PrefabManager._instance.GetGunByUserID (userId).gunUI.GetNickName ();
						PrefabManager._instance.ShowBossDeadHintPanel (this.typeID, userName, property.value);
					}
					break;
				default:
					if (property.type >= FiPropertyType.FISHING_EFFECT_FREEZE && //道具id，始于冰冻，止于鱼雷
					    property.type <= FiPropertyType.TORPEDO_NUCLEAR) { 
						Debug.Log ("道具掉落：" + property.type.ToString ()); //产品已经确定道具不可能掉鱼雷      一个半月后：老板说需要能掉鱼雷
						AudioManager._instance.PlayEffectClip (AudioManager.effect_getReward);
						GameObject dropSkillObj = PrefabManager._instance.GetSkillUIByServerId (property.type).gameObject;
						GameObject temp = GameObject.Instantiate (dropSkillObj, transform.position, Quaternion.identity) as GameObject;
						temp.GetComponent<Skill> ().DropAndMove (property.value, userId, transform.position + Vector3.one * sumSkillDrop * 1f);
						sumSkillDrop++;  
					}
					break;
				}
			}
		}
		if (fishGold != 0) {
			BuildCoinColumn (userId, fishGold);

		}

//        Debug.Log("打到鱼了" + userId);
		//	Debug.LogError ("SumSkillDrop:" + sumSkillDrop.ToString ());
	}

	void ShowRedSkin (float duration)
	{
		if (skinRenderer != null) {
			skinRenderer.materials [0].color = hitColor; //部分shader并不支持更改材质颜色的功能，现在默认用的是Sprites/Default这个shader做的测试
			Invoke ("RecoverSkinColor", duration);
		} else {
			Debug.LogError ("Error! skinRender=null:" + this.gameObject.name);
		}
	}

	void RecoverSkinColor ()
	{
        
		skinRenderer.materials [0].color = defaultSkinColor;
	}

	void DoSkinFade (float duration)
	{
		
	}

	void DoModelScale ()
	{
		Vector3 orginScale = childModel.transform.localScale;
		childModel.transform.DOBlendableScaleBy (-orginScale * 0.6f, 1f);
	}


	void StartMove ()
	{

		if (trackType == -1) {
			Debug.LogError ("Error! trackType=-1,name=" + this.gameObject.name);
			Destroy (this.gameObject);
			return;
		}
            
		if (GameController._instance.isDebugMode) {
			SetEnemyInfo (0, 0, typeID, 0, 0);
			trackTrans = transform.parent;
			childModel.SetActive (true);
			trackTrans.GetComponent<MyTrack> ().PlayPath ();
		} else {
			trackTrans = TrackManager._instance.GetTrack (trackType, trackID);  
		}
		
		if (trackTrans == null) {
			Debug.LogError ("Error!TrackType:" + trackType + " TrackID:" + trackID + " =null");
			Tool.OutLogToFile ("Error!TrackType:" + trackType + " TrackID:" + trackID + " =null");
			return; 
		} 
		myTrack = trackTrans.GetComponent<MyTrack> ();

		if (trackTrans.childCount > 0) {
			if (formation == FormationType.None) { //如果不是群组鱼，则需要清空原来轨迹上的鱼
				
				Tool.AddLogMsg ("Error!TrackType:" + trackType + " TrackID:" + trackID);
				Debug.LogWarning ("newFish groupId:" + groupID + " fishId:" + id + " track:" + trackType + "," + trackID);

				UIFishingMsg.GetInstance ().SndFishOut (this.groupID, this.id);//如果原来静态轨迹上还存在鱼，则新生成的鱼无效，视为游出屏幕
				fishPool.Remove (this.groupID, this.id);
				return;
				//myTrack.ClearChildObj (); 使用该方法时间久了之后会造成卡顿bug
			} else { //如果是群组鱼，但是轨迹上已经有鱼了，那可能是因为鱼潮需要共用同一条轨迹，需要执行复制轨迹的操作
                
			}
		}

		float tempValue = 0.6f;
		// tempValue = intervalTime;//以后可能间隔时间都交给服务器判断而不是本地，在新版鱼潮协议完成前还是要先注释

		switch (formation) {
		case FormationType.None:
                //trackTrans.GetComponent<DOTweenPath> ().DORewind ();
                //myTrack.ResetPath();//轨迹强制重置
			ResetPath ();
			this.transform.parent = trackTrans;
			this.transform.localPosition = Vector3.zero;//设置Enemy与轨迹起点位置一致
			if (myTrack.GetDelayTime () > 0) {
				Invoke ("PathDoPlay", myTrack.GetDelayTime ()); //对于翻转后动态生成的轨迹，需要手动delay 
			} else {
				PathDoPlay ();
			}
			
			break;
		case FormationType.Linear:
			if (this.id != 0) {
				newTrack = GameObject.Instantiate (trackTrans.gameObject, trackTrans.GetComponent<MyTrack> ().GetOriginPos (),
					trackTrans.rotation)as GameObject;
				if (newTrack == null)
					Debug.LogError ("newTrack=null", this.transform);
				trackTrans = newTrack.transform;
			}
			myTrack = trackTrans.GetComponent<MyTrack> ();
			myTrack.SetPathAutoKill (true);
			//myTrack.ResetPath ();
			ResetPath ();
			this.transform.parent = trackTrans;
			this.transform.localPosition = Vector3.zero;//设置Enemy与轨迹起点位置一致

			//鱼潮的鱼相关设置
			
			switch (fishTideType) {
			case 0:
				switch (typeID) {
				case 7:
					tempValue = 1.5f;
					break;
				case 17:
					tempValue = 5f;
					break;
				default:
					break;
				}
				break;
			case 1:
				switch (typeID) {
				case 1:
					tempValue = 1.5f;
					break;
				case 2:
					tempValue = 1.5f;
					break;
				case 17:
					tempValue = 9f;
					break;
				case 18:
					tempValue = 10f;
					break;
				default:
					break;
				}
				break;
			case 2:
				switch (typeID) {
				case 1:
					tempValue = 1.2f;
					break;
				case 18:
					tempValue = 0f;
					break;
				default:
					break;
				}
				break;
			case 3:
				switch (typeID) {
				case 2:
					tempValue = 1.5f;
					break;
				case 18:
					tempValue = 0f;
					break;
				default:
					break;
				}
				break;
			case 4:
				switch (typeID) {
				case 16:
					tempValue = 2.5f;
					break;
				case 17:
					tempValue = 10f;
					break;
				default:
					break;
				}
				break;
			case 5:
				switch (typeID) {
				case 3:
					tempValue = 1f;
					break;
				case 4:
					tempValue = 1f;
					break;
				case 10:
					tempValue = 3f;
					break;
				case 13:
					tempValue = 6f;
					break;
				default:
					break;
				}
				break;
			case 6:
				switch (typeID) {
				case 4:
					tempValue = 1.4f;
					break;
				case 7:
					tempValue = 1.4f;
					break;
				case 9:
					tempValue = 1.4f;
					break;
				case 15:
					tempValue = 0f;
					break;
				case 17:
					tempValue = 0f;
					break;
				default:
					break;
				}
				break;
			case 7:
				switch (typeID) {
				case 1:
					tempValue = 1.2f;
					break;
				case 18:
					tempValue = 10f;
					break;
				default:
					break;
				}
				break;
			
			default:
				break;
			}
               // tempValue = intervalTime;//以后可能都改用这个写法做时间间隔，在新版鱼潮协议完成前还是要先注释
              //  Debug.LogError("name:"+gameObject.name + "id:"+this.id + " tempValue:"+tempValue);
			Invoke ("PathDoPlay", this.id * tempValue);
			break;
		case FormationType.Bilinear:
			if (this.id != 0) {
				newTrack = GameObject.Instantiate (trackTrans.gameObject, trackTrans.GetComponent<MyTrack> ().GetOriginPos () + this.id % 2 * Vector3.up * 1f,
					trackTrans.rotation)as GameObject;
				if (newTrack == null)
					Debug.LogError ("newTrack=null", this.transform);
				trackTrans = newTrack.transform;
			}
			myTrack = trackTrans.GetComponent<MyTrack> ();
			myTrack.SetPathAutoKill (true);
			//myTrack.ResetPath ();
			ResetPath ();
			this.transform.parent = trackTrans;
			this.transform.localPosition = Vector3.zero;//设置Enemy与轨迹起点位置一致
			Invoke ("PathDoPlay", this.id * tempValue * 0.5f);
			break;
		case FormationType.Normal:
			if (this.id != 0) {
				newTrack = GameObject.Instantiate (trackTrans.gameObject, trackTrans.GetComponent<MyTrack> ().GetOriginPos (), 
					trackTrans.rotation)as GameObject;
				if (newTrack == null)
					Debug.LogError ("newTrack=null", this.transform);
				trackTrans = newTrack.transform;
			}
			myTrack = trackTrans.GetComponent<MyTrack> ();
			myTrack.SetPathAutoKill (true);
			//myTrack.ResetPath ();
			ResetPath ();
			this.transform.parent = trackTrans;
			this.transform.localPosition = TrackManager._instance.GetFormationGroupPos (5, this.id);
			float randomTime = Random.Range (0, 0.3f);
			Invoke ("PathDoPlay", randomTime);
			break;
            
		case FormationType.Circle:
			if (this.id != 0) { //环形鱼阵列经常会公用同一条轨迹，需要做特殊处理
				newTrack = GameObject.Instantiate (trackTrans.gameObject, trackTrans.GetComponent<MyTrack> ().GetOriginPos (),
					trackTrans.rotation) as GameObject;
				if (newTrack == null)
					Debug.LogError ("newTrack=null", this.transform);
                    
				trackTrans = newTrack.transform;
			} else if (myTrack.transform.childCount > 0) {
				newTrack = GameObject.Instantiate (trackTrans.gameObject, trackTrans.GetComponent<MyTrack> ().GetOriginPos (),
					trackTrans.rotation) as GameObject;
				if (newTrack == null)
					Debug.LogError ("newTrack=null", this.transform);
				if (newTrack.GetComponentInChildren<EnemyBase> () != null) {
					Destroy (newTrack.GetComponentInChildren<EnemyBase> ().gameObject); 

				}
				trackTrans = newTrack.transform;
			}
			myTrack = trackTrans.GetComponent<MyTrack> ();
			myTrack.SetPathAutoKill (true);


                //myTrack.ResetPath ();
			ResetPath ();
			this.transform.parent = trackTrans;
			this.transform.localPosition = TrackManager._instance.GetFormationGroupPos (groupLength, this.id);
			this.transform.localScale = transform.localScale * TrackManager._instance.GetFormationGroupScale (8, this.id) * 2;
			PathDoPlay ();
			break;
		default:
			break;
		}

		if (trackType == 3) { //如果处在“被召唤”的轨迹，则生成轨迹特效
			ShowSummonEffect ();
		}
			
		childModel.SetActive (true);
		childCollider.enabled = true;
		childAnimator = transform.GetComponentInChildren<Animator> ();
		childAnimator.SetBool ("isMove", true);
		thisState = EnemyState.Move;
	}

	void PathDoPlay ()
	{
		if (isFrozen)//PathDoPlay用于初始化群组鱼时调用，由于后面的鱼会等待前面的鱼出发后才开始播放轨迹，如果在出发前被冻住，之后又会执行PathDoplay导致鱼被冻住时还能移动
			return;
		if (thisState == EnemyState.Dead)
			return;
		if (trackTrans != null) {
			myTrack.PlayPath ();
		} else {
			if (trackTrans.gameObject == null) {
				Debug.LogError ("Error!MyTrack.gameObject=null,FishName:" + this.name + "trackType:" + trackType + "trackId:" + trackID);
			} else {
				Debug.LogError ("Error!trackTrans=null,FishName:" + this.name + "trackType:" + trackType + "trackId:" + trackID);
			}
		}

	}

	bool GetHitResult () //测试阶段默认击中后40%几率死亡
	{
		int randomNum = Random.Range (0, 100);
		if (randomNum > 40) {
			return true;
		} else {
			return false;
		}
	}


	public void Dead ()
	{
		if (thisState == EnemyState.Dead)
			return;

		if (NewcormerMissionPanel._instance != null) {
			switch (NewcormerMissionPanel._instance.currentMissionIndex) {
			case 1:
				NewcormerMissionPanel._instance.AddMissionProgress (1);
				break;
			case 2:
				NewcormerMissionPanel._instance.AddMissionProgress (1);
				break;
			case 5:
				if (typeID == 16) //Enemy_MoGuiYu
                        NewcormerMissionPanel._instance.AddMissionProgress (1);
				break;
			case 6:
				if (typeID == 18) //Enemy_ShaYu
                        NewcormerMissionPanel._instance.AddMissionProgress (1);
				break;
			default:
				break;
			}
		}

		if (bubbleBox != null) {
			Destroy (bubbleBox.gameObject);
		}
		childAnimator.enabled = true;
		childAnimator.SetTrigger ("deadTrigger");
		Invoke ("DoModelScale", 0.8f);
		thisState = EnemyState.Dead;
		if (this.id > 0) { //如果是群组鱼，且不是第一条鱼，说明轨迹是动态生成的，需要删除
			myTrack.DoKillPath ();
			Destroy (trackTrans.gameObject);
			trackTrans = null;
		}
		transform.parent = null;
		childCollider.enabled = false;
		PlayDeadVoice ();
//		Debug.LogError ("fishDead:" + groupID + "," + id+"/"+groupLength+" "+this.name);
		//UIFishingMsg.GetInstance ().fishPool.Remove (groupID, id, 2f);
		UIFishingObjects.GetInstance ().fishPool.Remove (groupID, id, 2f);
		UIFishingMsg.GetInstance ().SndFishOut (this.groupID, this.id);

	}

	void ReturnCoin (int coinCount, int userID)
	{
		//这部分需要交给服务器来判断
		//CannonInfo info =  UIFishingMsg.GetInstance ().cannonManage.GetInfo (userID);
		CannonInfo info = UIFishingObjects.GetInstance ().cannonManage.GetInfo (userID);

		if (null != info) {
			if (null != info.cannon) {
				Vector3 coinsPos = new Vector3 (transform.position.x, transform.position.y, 51.1f);


				int enemyReturn = this.goldReturn;

				GameObject coinGroup = GameObject.Instantiate (coinsEffectPrefab, coinsPos, Quaternion.identity) as GameObject;
				if (isBonus) {
					GameObject bonusEffect = null;
					if (info.cannon.isLocal) {
						bonusEffect = GameObject.Instantiate (PrefabManager._instance.bonusGoldEffect, 
							transform.position, Quaternion.identity) as GameObject;
					} else {
						bonusEffect = GameObject.Instantiate (PrefabManager._instance.bonusSliverEffect, 
							transform.position, Quaternion.identity) as GameObject;
					}
					ScreenManager.ShakeCamera (); //只有奖金鱼会震屏
					AudioManager._instance.PlayEffectClip (AudioManager.effect_eliterKilled, 1.5f);
					Destroy (bonusEffect, 4.8f);
					if (!torpedoHit) //鱼雷打到的情况不播该特效
						info.cannon.gunUI.ShowBonusEffect (coinCount, typeID);
				} else if (typeID == -10 || typeID == -13 || typeID == -8) { //金枪鱼，剑鱼，灯笼鱼    //特效滥用过多，暂时关闭if成立条件
                    
					GameObject bonusEffect = null;
					if (info.cannon.isLocal) {
						bonusEffect = GameObject.Instantiate (PrefabManager._instance.bonusGoldEffect,
							transform.position, Quaternion.identity) as GameObject;
						bonusEffect.transform.Find ("_Effect_Buyu_Bao/_Effect_Buyu_Bao_jinbi").gameObject.SetActive (false);
						if (!isBoss)
							bonusEffect.transform.Find ("_Effect_Buyu_Bao/_Effect_Buyu_Bao_jinbi_chixu").gameObject.SetActive (false);
					} else {
						bonusEffect = GameObject.Instantiate (PrefabManager._instance.bonusSliverEffect,
							transform.position, Quaternion.identity) as GameObject;
						bonusEffect.transform.Find ("_Effect_Buyu_Bao2/_Effect_Buyu_Bao_jinbi").gameObject.SetActive (false);
					}


					AudioManager._instance.PlayEffectClip (AudioManager.effect_eliterKilled, 1.5f);
					Destroy (bonusEffect, 4.8f);
				}

				coinGroup.GetComponent<CoinEffectGroup> ().StartMoveToPlayer (info, coinCount, enemyReturn, 1.7f, isBonus);
				//生成金币柱
				//BuildCoinColumn ( coinCount, enemyReturn);

				AudioManager._instance.PlayEffectClip (AudioManager.effect_getCoin, 0.5f);
			}
		} else {
			Debug.LogError ("Error:info=null");
		}
	}

	void ReturnDiamond (int diamondNum, int userId)
	{
		CannonInfo info = UIFishingObjects.GetInstance ().cannonManage.GetInfo (userId);

		if (null != info) {
			if (null != info.cannon) {
				Vector3 diamondPos = new Vector3 (transform.position.x + 1.5f, transform.position.y + 0.5f, 51f);

				GameObject diamondDropPrefab = PrefabManager._instance.diamondDropEffect;
				GameObject diamondDrop = GameObject.Instantiate (diamondDropPrefab, diamondPos, Quaternion.identity) as GameObject;
	
				diamondDrop.GetComponent<DiamondDropEffect> ().StartMoveToPlayer (info.cannon, diamondNum);
				//AudioManager._instance.PlayEffectClip (AudioManager.effect_getCoin,0.5f);
			}
		} else {
			Debug.LogError ("Error:info=null");
		}
	}

	void ShowSummonEffect ()
	{
		AudioManager._instance.PlayEffectClip (AudioManager.effect_summon);
		GameObject summonEffectPrefab = PrefabManager._instance.GetPrefabObj (MyPrefabType.SkillEffect, 4);
		Vector3 summonEulerAngle = TrackManager._instance.GetTrackDRotation (trackID);//从数组中获取每条D类轨迹对应的旋转角
		summonEulerAngle = new Vector3 (summonEulerAngle.x + 90f, 
			summonEulerAngle.y, summonEulerAngle.z - 90f);//我也不知道这里为什么要转两个90度，反正转了之后效果就实现了

		Quaternion summonRotation = Quaternion.Euler (summonEulerAngle);
		GameObject summonEffect = GameObject.Instantiate (summonEffectPrefab, transform.position, summonRotation)as GameObject;
		Destroy (summonEffect, 2.5f);
	}

	void ResetPath () //当鱼离开时，重置轨迹
	{
		if (trackTrans != null) {
			myTrack.ResetPath ();
		} else {
			Tool.OutLogToFile ("Can't reset path:" + trackType + "," + trackID);
		}
	}



	public void BeFrozen () //触发被冰冻时使用
	{
		if (myTrack != null)
			myTrack.PausePath ();
		else
			Debug.LogError ("Error!myTrack=null:" + "groupId:" + this.groupID + ",id:" + this.id);
		if (isFrozen) { //如果已经在冰冻中了，延长时间
			CancelInvoke ("ForceThaw");
			Invoke ("ForceThaw", 11f);//强制解冻
			return;
		}
		isFrozen = true;
		moveSpeed = 0;
		Vector3 frozenPos = childModel.transform.position - Vector3.forward * 10f;
		iceBlock = GameObject.Instantiate (PrefabManager._instance.GetPrefabObj (MyPrefabType.SkillEffect, 3),
			frozenPos, Quaternion.identity, this.transform)as GameObject;
		iceBlock.transform.rotation = childModel.transform.rotation;
		iceBlock.transform.localScale = (childCollider.size * 2f + Vector3.one * 0.5f) * 0.4f;
		if (childAnimator != null)
			childAnimator.enabled = false;
		//Invoke ("ForceFreeze", 1f);

		Invoke ("ForceThaw", 11f);//强制解冻

	}

	void ForceFreeze ()
	{
		if (myTrack != null) {
			myTrack.PausePath ();
			moveSpeed = 0;
		} else
			Debug.LogWarning ("Error!myTrack=null:" + "groupId:" + this.groupID + ",id:" + this.id + "track:" + trackType + "," + trackID);
	}

	public  void BeThaw ()//解冻
	{
		if (isFrozen) {
			isFrozen = false;
			moveSpeed = originMoveSpeed;
			if (myTrack != null)
				myTrack.ThawPlayPath ();
			childAnimator.enabled = true;
			if (iceBlock != null)
				Destroy (iceBlock);
		}
	}

	public void ForceThaw ()
	{
		if (isFrozen) {
			Debug.LogWarning ("ForceThaw:" + "groupId=" + groupID + " id" + id, this.transform);
			BeThaw ();
		}

	}

	void OnDestroy () //对于动态生成轨迹的鱼，在其被销毁时，要保证一同摧毁其动态生成的轨迹
	{
		// Debug.LogError("name:"+this.gameObject.name + "hasInScreen="+hasInScreen);
		if (this.id > 0 && trackTrans != null) {
			if (myTrack == null) {
				Debug.LogError ("ForceDestroy TrackTrans(myTrack=null):" + trackType + "," + trackID, trackTrans);
			} else {
				//Debug.LogError ("ForceDestroy TrackTrans:"+trackType+","+trackID,trackTrans);
			}

			myTrack.DoKillPath ();
			Destroy (trackTrans.gameObject);
		}

	}

	void PlayNormalVoice ()
	{
		if (normalVoice != null) {
			EnemyPoor._instance.PlayEnemyVoice (normalVoice);
		}
	}

	void PlayDeadVoice ()
	{
		if (deadVoice != null) {
			EnemyPoor._instance.PlayEnemyVoice (deadVoice);
		}
	}

	public void LeaveScreen ()
	{
		leaveScreenFlag = true;
	}
	//增加金币柱的方法
	void BuildCoinColumn (int userID, int ncoinCount)
	{
		int nenemyReturn = this.goldReturn;
		CannonInfo ninfo = UIFishingObjects.GetInstance ().cannonManage.GetInfo (userID);
		//PrefabManager._instance.GetLocalGun().thisSeat;
		if (ninfo.cannon.isLocal) {
			if (ninfo.cannon.thisSeat == GunSeat.RB || ninfo.cannon.thisSeat == GunSeat.LT) {
				PrefabManager._instance.GoldCoinColumnRight.GetComponent<GoldCoinColumn> ().BuildGoldCoinColumn (ninfo, ncoinCount, nenemyReturn, ninfo.cannon.thisSeat);
			} else if (ninfo.cannon.thisSeat == GunSeat.LB || ninfo.cannon.thisSeat == GunSeat.RT) {
				PrefabManager._instance.GoldCoinColumnLeft.GetComponent<GoldCoinColumn> ().BuildGoldCoinColumn (ninfo, ncoinCount, nenemyReturn, ninfo.cannon.thisSeat);
			}
			//   Debug.Log("自己的金币柱"+PrefabManager._instance.GetLocalGun().thisSeat);
		} else {
			switch (PrefabManager._instance.GetLocalGun ().thisSeat) {
			case GunSeat.RB:
				{
					if (ninfo.cannon.thisSeat == GunSeat.LB) {
						PrefabManager._instance.GoldCoinColumnLeft.GetComponent<GoldCoinColumn> ().BuildGoldCoinColumn (ninfo, ncoinCount, nenemyReturn, ninfo.cannon.thisSeat);
					}
					break;
				}
			case GunSeat.LB:
				{
					if (ninfo.cannon.thisSeat == GunSeat.RB) {
						PrefabManager._instance.GoldCoinColumnRight.GetComponent<GoldCoinColumn> ().BuildGoldCoinColumn (ninfo, ncoinCount, nenemyReturn, ninfo.cannon.thisSeat);
					}
					break;
				}
			case GunSeat.LT:
				{
					if (ninfo.cannon.thisSeat == GunSeat.RT) {
						PrefabManager._instance.GoldCoinColumnRight.GetComponent<GoldCoinColumn> ().BuildGoldCoinColumn (ninfo, ncoinCount, nenemyReturn, ninfo.cannon.thisSeat);
					}
					break;
				}
			case GunSeat.RT:
				{
					if (ninfo.cannon.thisSeat == GunSeat.LT) {
						PrefabManager._instance.GoldCoinColumnRight.GetComponent<GoldCoinColumn> ().BuildGoldCoinColumn (ninfo, ncoinCount, nenemyReturn, ninfo.cannon.thisSeat);
					}
					break;
				}
			default:
				break;
			}
//            Debug.Log("其他人的金币柱"+userID);
		}
	}
}
