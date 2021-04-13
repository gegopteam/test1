using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System.Collections.Generic;

using AssemblyCSharp;
using DG.Tweening;

public enum SkillType
{
	Error,
	//0,
	Freeze,
	//1冰冻
	Lock,
	//2锁定
	Berserk,
	//3狂化,在pk场时功能为双倍
	Summon,
	//4召唤,
	Torpedo,
	//5鱼雷
	AutoFire,
	//6自动开火
	Double,
	//x2
	Replication,
	//7pk分身
}

public class Skill : MonoBehaviour
{
	public static bool isReceiveServerLock = false;

	public SkillType skillType;
	public  int serverTypeId = -1;
	//在服务端的Id
	public float skillDuration = 5f;
	public float cdTime = 10f;
	bool repareToUse = false;
	bool isInActive = false;
	bool isLock = false;
	bool isautoFire = false;


	public Image mask;

	float timer = 0;

	Text restNumText;
	//持有数
	[HideInInspector]
	public int restNum;

	Text priceNumText;
	//购买一个所需要的价格
	[HideInInspector]public int priceNum;
	public GameObject quadImage;

	GameObject torpedoBar = null;
	public int torpedoLevel = -1;
	//0代表是总的鱼雷bar，不是道具
	Vector3 torpedoBarOriginPos;
	bool torpedoBarIsShow = true;

	public Sprite greyTorpedoIcon;
	public Sprite orginalTorpedoIcon;

	private BackpackInfo backpackInfo = null;
	private DataControl dataControl = null;

	[HideInInspector]public Vector3 orginalPos;
	// Use this for initialization
	MyInfo myInfo;
	//技能开关
	bool stopSkill = false;
	public static Skill Instance;
	/// <summary>
	/// 鱼雷总数Text
	/// </summary>
	Text torpedoCountText;
	/// <summary>
	/// 鱼雷总数
	/// </summary>
	static int torpedoAllCount = 0;

	void  Awake ()
	{
		Instance = this;
		dataControl = DataControl.GetInstance ();
		myInfo = (MyInfo)Facade.GetFacade ().data.Get (FacadeConfig.USERINFO_MODULE_ID);
		backpackInfo = dataControl.GetBackpackInfo ();
		if (skillType == SkillType.Torpedo) {
			if (transform.name == "Skill_Torpedo") {
				repareToUse = true;
			}
		}
	}


	void Start ()
	{
		if (GameController._instance.isExperienceMode) {
			if (skillType == SkillType.Torpedo) {
				gameObject.SetActive (false);
			}
		}
		if (dropAddNum > 0) { //此时说明这个道具是掉落出来的不是场景里原来存在的
			Debug.Log ("dropAddNum");
			SetRestNumShow (false);
			SetPriceNumShow (false);
            //joey
			//SetLockIconShow (false);
			if (skillType == SkillType.Torpedo) {
				transform.Find ("Image").GetComponent<Image> ().sprite = orginalTorpedoIcon;
			}
			return;
		}

		orginalPos = transform.position;
		if (skillType == SkillType.AutoFire) { //自动开火功能暂时不需要获取这些组件
			//这是vip1特权自动开炮
//			if (DataControl.GetInstance ().GetMyInfo ().levelVip > 0) {
//				isLock = false;
//				isReceiveServerLock = false;
//			} else {
//				isReceiveServerLock = true;
//				isLock = true;
//			}
//			SetLockIconShow (isLock);

			transform.Find ("Name_CancleAuto").gameObject.SetActive (false);
			transform.Find ("Name_AutoFire").gameObject.SetActive (true);
			//transform.FindChild ("LockImage").gameObject.SetActive (false);//后期锁定图标要根据服务器里获取技能权限决定显示
			GameType tempType = GameController._instance.myGameType;
			switch (tempType) {
			case GameType.Classical:
				break;
			case GameType.Bullet:
				this.GetComponent<RectTransform> ().localPosition = Vector3.zero;
				Transform rightSkillPanel_Bullet = this.transform.parent;
				transform.parent = GameObject.FindGameObjectWithTag (TagManager.uiCanvas).transform;
				rightSkillPanel_Bullet.transform.position = Vector3.right * 1000;

				break;
			case GameType.Time:
				this.GetComponent<RectTransform> ().localPosition = Vector3.zero;
				Transform rightSkillPanel_Time = this.transform.parent;
				transform.parent = GameObject.FindGameObjectWithTag (TagManager.uiCanvas).transform;
				rightSkillPanel_Time.transform.position = Vector3.right * 1000f;
				break;
			case GameType.Point:
				this.GetComponent<RectTransform> ().localPosition = Vector3.zero;
				Transform rightSkillPanel_Point = this.transform.parent;
				transform.parent = GameObject.FindGameObjectWithTag (TagManager.uiCanvas).transform;
				rightSkillPanel_Point.transform.position = Vector3.right * 1000f;
				break;
			default:
				break;
			}

			if (tempType != GameType.Classical) {
				Transform backBtn = ScreenManager.uiScaler.transform.Find ("LeftOption/BackBtn");
				backBtn.SetParent (this.transform.Find ("BackBtnPos"));
				backBtn.localPosition = Vector3.zero;
				backBtn.GetComponent<Image> ().sprite = PrefabManager._instance.backBtnSprite2;
				backBtn.localScale *= 1.3f;
			}
			if (skillType == SkillType.AutoFire && (myInfo.misHaveDraCard || GameController._instance.isExperienceMode)) {
				PrefabManager._instance.AddSkillUIToList (this);
				this.GetComponent<Button> ().onClick.AddListener (delegate() {
					DraUseAuFire ();

				});
                //joey
				transform.Find ("PriceNum").gameObject.SetActive (true);
				transform.Find ("RestNum").gameObject.SetActive (false);	
				return;
			} else {
				this.GetComponent<Button> ().onClick.AddListener (delegate() {
					UseAutoFire ();
				});
			}
//			PrefabManager._instance.AddSkillUIToList (this);
//			return;
		}
		//————————————————————上方为只有自动开炮才会设置的内容————————————————
		//鱼雷部分
		if (skillType == SkillType.Torpedo) {
			//Debug.LogError("repareToUse = "+repareToUse);
			if (repareToUse) {//如果是鱼雷的总图标
				//PrefabManager._instance.AddSkillUIToList (this);
				torpedoBar = transform.Find ("TorpedoBar").gameObject;
				torpedoBarOriginPos = torpedoBar.GetComponent<RectTransform> ().localPosition;
				SetTorpedoBarShow (false);
				torpedoCountText = transform.Find ("RestNum").GetComponent<Text> ();
				torpedoAllCount = backpackInfo.GetTorperdoCountNum ();
				if (torpedoAllCount == 0) {
					torpedoCountText.text = "";
				} else {
					torpedoCountText.text = torpedoAllCount.ToString ();
				}
				return;
			} else {//如果是单个鱼雷道具对应的图标
				//PrefabManager._instance.AddSkillUIToList(this);
				torpedoBar = transform.parent.gameObject;
				//orginalTorpedoIcon = transform.FindChild("Image").GetComponent<Image>().sprite; //直接public出来在编辑器里赋值
			}
		}
        //狂暴部分
        else if (skillType == SkillType.Berserk) {
			//GameController._instance.myGameType == GameType.Classical &&
			if (myInfo.misHaveDraCard) {
				isLock = false; //需求改动，狂暴无VIP限制  2019.1.21--有龙卡才能使用
			} else {
				isLock = true;
			}
			SetLockIconShow (isLock);
			transform.Find ("Berserk").gameObject.SetActive (true);
			transform.Find ("CancleBerserk").gameObject.SetActive (false);
			this.GetComponent<Button> ().onClick.AddListener (delegate () {
				BerserkSkillContrl ();
			});
		}

        //分身部分
        else if (skillType == SkillType.Replication) {
			Debug.Log("-----------------------使用分身");
			//joey
			isLock = false;
			isReceiveServerLock = false;
			//if (GameController._instance.myGameType == GameType.Classical &&
   //             DataControl.GetInstance().GetMyInfo().levelVip < 2)
   //         {
   //             isLock = true;
   //             isReceiveServerLock = true;
   //         }
   //         else
   //         {
   //             isLock = false;
   //             isReceiveServerLock = false;
   //         }
   //         SetLockIconShow(isLock);
            transform.Find ("Replication").gameObject.SetActive (true);
			transform.Find ("CancleReplication").gameObject.SetActive (false);
			this.GetComponent<Button> ().onClick.AddListener (delegate () {
				ReplicationSkillContrl ();
			});
		
		} else if (skillType == SkillType.Lock) {
			//锁定的部分
			if (myInfo.misHaveDraCard) {
				transform.Find ("PriceNum").gameObject.SetActive (false);
				transform.Find ("RestNum").gameObject.SetActive (false);
				isLock = false;
				isReceiveServerLock = false;
				PrefabManager._instance.AddSkillUIToList (this);
				transform.Find ("Lock").gameObject.SetActive (true);
				transform.Find ("CanleLock").gameObject.SetActive (false);
				this.GetComponent<Button> ().onClick.AddListener (delegate () {
					LockAndAudio ();
				});
			} else {
				transform.Find ("Lock").gameObject.SetActive (true);
				transform.Find ("CanleLock").gameObject.SetActive (false);
				isLock = true;
				isReceiveServerLock = true;
				this.GetComponent<Button> ().onClick.AddListener (delegate () {
					NoLongCardLockAndAudio ();
				});
			}
			SetLockIconShow (isLock);
		}

		mask = transform.Find ("Mask").GetComponent<Image> ();
		if (GameController._instance.myGameType != GameType.Classical) {
			//mask.sprite = Resources.Load("unity_builtin_extra/UISprite") as Sprite;
			mask.sprite = PrefabManager._instance.defaultBgSprite;
			mask.type = Image.Type.Filled;
			mask.fillMethod = Image.FillMethod.Radial360;
			// mask.fillOrigin= 
			mask.fillAmount = 0f;
			mask.fillClockwise = false;
		}
		restNumText = transform.Find ("RestNum").GetComponent<Text> ();
		//restNum = int.Parse (restNumText.text); //单机模式下先从本地的Text上获取数量，正式游戏里需要从服务器获得信息
		priceNumText = transform.Find ("PriceNum").GetComponentInChildren<Text> ();
		priceNum = int.Parse (priceNumText.text);

		serverTypeId = ChangeSkillTypeToServer ((int)skillType, this.torpedoLevel);
		//Debug.Log ("____________________33" + skillType + "111111" + this.torpedoLevel + "num" + restNum + "serverTypeId" + serverTypeId);
		PrefabManager._instance.AddSkillUIToList (this);//将技能加入PrefabManager管理

		if (GameController._instance.myGameType == GameType.Classical)
			InitNum ();
		
		if (restNum == 0) {
			SetRestNumShow (false);
			SetPriceNumShow (true);
  
		} else {
            
			SetRestNumShow (true);
			SetPriceNumShow (false);
		}
		//体验场都不显示
		if (GameController._instance.isExperienceMode) {
			SetRestNumShow (false);
			SetPriceNumShow (false);
		}

		switch (GameController._instance.myGameType) {  
		case GameType.Classical:
			//SetPropBoxShow (false);
			break;
		case GameType.Bullet:
			if (skillType == SkillType.Freeze || skillType == SkillType.Lock) {
				transform.position = Vector3.down * 1000f;
			}
			break;
		case GameType.Point:
			if (skillType == SkillType.Freeze || skillType == SkillType.Lock) {
				transform.position = Vector3.down * 1000f;
			}
			break;
		case GameType.Time:
			if (skillType == SkillType.Freeze || skillType == SkillType.Lock) {
				transform.position = Vector3.down * 1000f;
			}
			break;
		default:
			break;
		}
	}

	public void RefreshAutoFireShow ()
	{
		this.GetComponent<Button> ().onClick.RemoveAllListeners ();
		if (skillType == SkillType.AutoFire && myInfo.misHaveDraCard) {
			PrefabManager._instance.AddSkillUIToList (this);
			this.GetComponent<Button> ().onClick.AddListener (delegate() {
				DraUseAuFire ();

			});
			transform.Find ("PriceNum").gameObject.SetActive (false);
			transform.Find ("RestNum").gameObject.SetActive (false);
			return;
		} else {
			this.GetComponent<Button> ().onClick.AddListener (delegate() {
				UseAutoFire ();
			});
		}
	}

	// Update is called once per frame
	void Update ()
	{
//		if (Input.GetKeyDown (KeyCode.L)) {
//			isReceiveServerLock = false;
//			PrefabManager._instance.GetSkillUIByType (SkillType.Replication).SetLockIconShow (Skill.isReceiveServerLock);
//		}
		if (isautoFire) {
			return;
		}
		if (isInActive) {
//			Debug.Log (timer + "timer" + cdTime);
			timer += Time.deltaTime;
			mask.fillAmount = (1 - timer / cdTime);
			if (timer > cdTime) {
				isInActive = false;
				switch (skillType) {
				case SkillType.AutoFire:
					{
						SetLocalGunUseSkill (false);
					}
					break;
				case SkillType.Replication:
					{
						transform.Find ("Replication").gameObject.SetActive (true);
						transform.Find ("CancleReplication").gameObject.SetActive (false); 
					}
					break;
				case SkillType.Berserk:
					{
						transform.Find ("Berserk").gameObject.SetActive (true);
						transform.Find ("CancleBerserk").gameObject.SetActive (false);
					}
					break;
				default:
					break;
				}
				//if (skillType == SkillType.AutoFire) {
				//	SetLocalGunUseSkill (false);
				//}
				timer = 0f;
				mask.fillAmount = 0;
			}	
		}
	}

	public void cancleAutoFire ()
	{
		if (!isInActive) {
			return;
		}

		Debug.LogError ("222222222222");
		isInActive = false;
		timer = 0;
		mask.fillAmount = 0f;
		if (myInfo.misHaveDraCard && skillType == SkillType.Lock) {
			transform.Find ("Lock").gameObject.SetActive (true);
			transform.Find ("CanleLock").gameObject.SetActive (false);
		} else {
//			ReduceRestNum ();
		}
		if (skillType == SkillType.Replication) {
			transform.Find ("Replication").gameObject.SetActive (true);
			transform.Find ("CancleReplication").gameObject.SetActive (false);
			Facade.GetFacade ().message.fishCommom.SendOtherCancelSkill ((int)skillType, myInfo.userID);
		}
		GunControl gunControl = PrefabManager._instance.GetGunByUserID (myInfo.userID);
		gunControl.UserCanCelSkill (SkillType.Replication, skillDuration, false);
	}

	public void SendUseSkillRequest () //向服务器发送自己使用技能的请求
	{
//		Debug.LogError (" restNum3333 = " + restNum);

		if (isInActive) {
			return;
		}

		if (GameController._instance.isDebugMode)
			isLock = false;
		if (GameController._instance.myGameType != GameType.Classical) {
			if (restNum <= 0)
				return;
		}
	
		if (skillType == SkillType.Berserk && myInfo.misHaveDraCard) {
			isLock = false;
		} else if (myInfo.levelVip >= 2 && skillType == SkillType.Replication) {
			isLock = false;
		}

		if (isLock) {
			if (skillType == SkillType.Berserk) {
				PrefabManager._instance.ShowNormalHintPanel ("提示：購買龍卡可使用此功能");
				return;
			}
            //joey
    //        else if (skillType == SkillType.Replication) {
				//PrefabManager._instance.ShowNormalHintPanel ("提示：升級到vip2可使用此功能");
				//return;
			//}


//			if (PrefabManager._instance.GetLocalGun ().currentGunStyle == 6 || PrefabManager._instance.GetLocalGun ().currentGunStyle == 7) {
//				PrefabManager._instance.ShowNormalHintPanel ("提示：该炮台不能使用此功能");
//				return;
//			}

		}
		//DebugTest
		// if(skillType==SkillType.Double){
		// SetLocalGunUseSkill();
		//  }

		AudioManager._instance.PlayEffectClip (AudioManager.ui_click);
		switch (skillType)
		{
			case SkillType.Lock:
				EnemyBase tempEnemy = UIFishingObjects.GetInstance().fishPool.GetAim();
				if (tempEnemy == null)
				{
					HintText._instance.ShowHint("暫未找到可以鎖定的魚");
					return;
				}
				if (PrefabManager._instance.GetLocalGun().replicationCount > 0)
				{
					HintTextPanel._instance.SetTextShow("分身狀態下無法使用鎖定");
					return;
				}
				if (PrefabManager._instance.GetLocalGun().isBankruptcy)
				{//破產狀態下限制使用鎖定道具
					HintTextPanel._instance.SetTextShow("破產狀態下無法使用鎖定");
					return;
				}
				break;
			case SkillType.Replication:
				if (PrefabManager._instance.GetLocalGun().isBankruptcy)
				{//破產狀態下限制使用分身道具
					HintTextPanel._instance.SetTextShow("破產狀態下無法使用分身");
					return;
				}
				break;
			case SkillType.AutoFire:
				if (PrefabManager._instance.GetLocalGun().isBankruptcy)
				{//破產狀態下限制使用自動道具
					HintTextPanel._instance.SetTextShow("破產狀態下無法使用自動");
					return;
				}
				break;
			case SkillType.Summon:
				if (PrefabManager._instance.GetLocalGun().isBankruptcy)
				{//破產狀態下限制使用召喚道具
					HintTextPanel._instance.SetTextShow("破產狀態下無法使用召喚");
					return;
				}
				if (GameController._instance.isBossMode || GameController._instance.isBossMatchMode)
				{
					HintTextPanel._instance.SetTextShow("BOSS場無法使用召喚");
					return;
				}
				break;
			case SkillType.Berserk:
				if (PrefabManager._instance.GetLocalGun().isBankruptcy)
				{//破產狀態下限制使用狂暴道具
					HintTextPanel._instance.SetTextShow("破產狀態下無法使用狂暴");
					return;
				}
				break;
			default:
				break;
		}
		//		Debug.LogError (restNum + "resnum");
		if (restNum <= 0) {
			if (PrefabManager._instance.GetLocalGun ().curretnDiamond >= priceNum) {
//				Debug.LogError ("====" + PrefabManager._instance.GetLocalGun ().curretnDiamond);
				SendBuyOneRequest ();
				transform.GetComponent<Button> ().enabled = false;
				Invoke ("EnableButton", 0.5f);
				return;
			} else {
				if (skillType != SkillType.Torpedo && GameController._instance.myGameType == GameType.Classical) {
					//HintTextPanel._instance.SetTextShow("购买道具所需钻石数不足", 2f);
					if (myInfo.isGuestLogin) {
						StartGiftManager.GuestToStoreManager ();
						//return;
					}
					GameObject WindowClone = AppControl.OpenWindow ("Window/NewStoreCanvas");
					WindowClone.GetComponent<UIStore> ().DiamondButton ();
					transform.GetComponent<Button> ().enabled = false;
					Invoke ("EnableButton", 0.5f);
				}                  
				return;
			}

		}

		if (GameController._instance.isFishTideComing1) {
			switch (this.skillType)
			{
				case SkillType.Freeze:
					Debug.LogError("22222222222");
					HintTextPanel._instance.SetTextShow("魚潮來臨，請先休息會哦！");
					return;
				case SkillType.Summon:
					Debug.LogError("2222222333333");
					HintTextPanel._instance.SetTextShow("魚潮來臨，請先休息會哦！");
					return;
				default:
					break;
			}
		}

		if (skillType != SkillType.Summon)
			mask.fillAmount = 1;

		transform.GetComponent<Button> ().enabled = false;
		Invoke ("EnableButton", 0.5f);


		switch (GameController._instance.myGameType) {
		case GameType.Classical:
			UIFishingMsg.GetInstance ().SndEffectRequest (serverTypeId);
			break;
		case GameType .Bullet:
			UIFishingMsg.GetInstance ().SndPKEffectRequest (serverTypeId);
			break;
		case GameType.Point:
			UIFishingMsg.GetInstance ().SndPKEffectRequest (serverTypeId);
			break;
		case GameType.Time:
			UIFishingMsg.GetInstance ().SndPKEffectRequest (serverTypeId);
			break;
		default:
			UIFishingMsg.GetInstance ().SndPKEffectRequest (serverTypeId);
			break;
		}

		//Debug.LogError ("SendSkill:" + skillType.ToString ()+" serverTypeId:"+serverTypeId);
	}



	public void ActiveSkill (List <EnemyBase> enemyList = null) //只应该被作用于本地用户自己的炮台,enemyList只在使用冰冻和鱼雷技能时有效
	{
		if (skillType == SkillType.Error || !ReduceRestNum ())
			return;
		if (isInActive) {
			return;
		}

		isInActive = true;   //控制技能cd显示
		mask.fillAmount = 1;

		if (skillType == SkillType.Freeze && enemyList != null) {
			FreezeEnemy (enemyList);
		}
		if (skillType != SkillType.Lock) //锁定技能已经使用过SetLocalGunUseSkill了
		    SetLocalGunUseSkill ();
	}
	//控制分身开始和关闭
	public void ReplicationSkillContrl ()
	{
		Debug.Log("---------- ReplicationSkillContrl 你點了分身");
		if (PrefabManager._instance.GetLocalGun().currentGunStyle == 6 || PrefabManager._instance.GetLocalGun().currentGunStyle == 7)
		{
			HintTextPanel._instance.SetTextShow("該炮台不能使用分身");
			return;
		}
		if (PrefabManager._instance.GetLocalGun().isBankruptcy)
		{//破產狀態下限制使用分身道具
			HintTextPanel._instance.SetTextShow("破產狀態下無法使用分身");
			return;
		}

        //joey
        if (!isInActive)
        {
            if (!isLock && PrefabManager._instance.GetLocalGun().curretnDiamond >= priceNum)
            {
                transform.Find("Replication").gameObject.SetActive(false);
                transform.Find("CancleReplication").gameObject.SetActive(true);
            }
            stopSkill = false;
            Debug.Log("isLock" + isLock);
            SendUseSkillRequest();
            //isInActive = true;
            //return;
        }
        else
        {
            transform.Find ("Replication").gameObject.SetActive (true);
			transform.Find ("CancleReplication").gameObject.SetActive (false);
			Facade.GetFacade ().message.fishCommom.SendOtherCancelSkill ((int)skillType, myInfo.userID);
			//GameController._instance.isStopSkillReplication = true;
			stopSkill = true;
			isInActive = false;
			if (PrefabManager._instance == null) {
				return;
			}
			GunControl gunControl = PrefabManager._instance.GetGunByUserID (myInfo.userID);
			gunControl.UserCanCelSkill (SkillType.Replication, skillDuration, false);
			//SetLocalGunUseSkill();
			timer = 0f;
			mask.fillAmount = 0;
            //return;
        }
    }

	//控制狂暴技能开始和关闭
	public void BerserkSkillContrl ()
	{
		if (PrefabManager._instance.GetLocalGun().isBankruptcy)
		{//破產狀態下限制使用分身道具
			HintTextPanel._instance.SetTextShow("破產狀態下無法使用狂暴");
			return;
		}
		if (PrefabManager._instance.GetLocalGun().currentGunStyle == 6 || PrefabManager._instance.GetLocalGun().currentGunStyle == 7)
		{//破產狀態下限制使用分身道具
			HintTextPanel._instance.SetTextShow("該炮台無法使用此功能");
			return;
		}
		if (!isInActive) {
			if (!isLock && PrefabManager._instance.GetLocalGun ().curretnDiamond >= priceNum) {
				transform.Find ("Berserk").gameObject.SetActive (false);
				transform.Find ("CancleBerserk").gameObject.SetActive (true);
			}
			//GameController._instance.isStopSkill = false;
			stopSkill = false;
			SendUseSkillRequest ();
			//isInActive = true;
			//return;
		} else {
			Facade.GetFacade ().message.fishCommom.SendOtherCancelSkill ((int)skillType, myInfo.userID);
			//GameController._instance.isStopSkill = true;
			stopSkill = true;
			transform.Find ("Berserk").gameObject.SetActive (true);
			transform.Find ("CancleBerserk").gameObject.SetActive (false);
			isInActive = false;
			if (PrefabManager._instance == null) {
				return;
			}
			GunControl gunControl = PrefabManager._instance.GetGunByUserID (myInfo.userID);
			gunControl.UserCanCelSkill (SkillType.Berserk, skillDuration, false);
			//SetLocalGunUseSkill();
			timer = 0f;
			mask.fillAmount = 0;
		}
	}
	//锁定新的bufen 有龙卡
	public void LockAndAudio ()
	{
		if (PrefabManager._instance.GetLocalGun().isBankruptcy)
		{//破產狀態下限制使用分身道具
			HintTextPanel._instance.SetTextShow("破產狀態下無法使用鎖定自動開砲");
			return;
		}
		if (PrefabManager._instance.GetLocalGun().replicationCount > 0)
		{
			HintTextPanel._instance.SetTextShow("分身狀態下無法使用自動開砲");
			return;
		}
		if (PrefabManager._instance.GetLocalGun().isBankruptcy)
		{//破產狀態下限制使用鎖定道具
			HintTextPanel._instance.SetTextShow("破產狀態下無法使用自動開砲");
			return;
		}
		if (skillType == SkillType.Lock) {
			transform.Find ("Lock").gameObject.SetActive (false);
			transform.Find ("CanleLock").gameObject.SetActive (true);
			transform.Find ("PriceNum").gameObject.SetActive (false);
			transform.Find ("RestNum").gameObject.SetActive (false);
			//GameController._instance.isStopLockAndAudio = false;
			//SendUseSkillRequest();
			mask.enabled = false;
			Debug.Log ("LockAndAudio++++++++isInActive" + isInActive);
			if (!isInActive) {
				transform.Find ("Lock").gameObject.SetActive (false);
				transform.Find ("CanleLock").gameObject.SetActive (true);
				//GameController._instance.isStopLockAndAudio = false;
				stopSkill = false;
				SetLocalGunUseSkill (true);
				//如果购买龙卡的用户则为true;
				isautoFire = true;
				isInActive = true;
				return;
			} else {
				transform.Find ("Lock").gameObject.SetActive (true);
				transform.Find ("CanleLock").gameObject.SetActive (false);
				//GameController._instance.isStopLockAndAudio = true;
				stopSkill = true;
				SetLocalGunUseSkill (false);
				isInActive = false;

				return;
			}
		}
	}
	//锁定的更新部分 非龙卡
	public void NoLongCardLockAndAudio ()
	{

		if (PrefabManager._instance.GetLocalGun().isBankruptcy)
		{//破產狀態下限制使用分身道具
			HintTextPanel._instance.SetTextShow("破產狀態下無法使用自動開砲");
			return;
		}
		if (PrefabManager._instance.GetLocalGun().replicationCount > 0)
		{
			HintTextPanel._instance.SetTextShow("分身狀態下無法使用自動開砲");
			return;
		}
		if (PrefabManager._instance.GetLocalGun().isBankruptcy)
		{//破產狀態下限制使用鎖定道具
			HintTextPanel._instance.SetTextShow("破產狀態下無法使用自動開砲");
			return;
		}
		if (PrefabManager._instance.GetLocalGun ().curretnDiamond < priceNum && restNum <= 0) {
			GameObject WindowClone = AppControl.OpenWindow ("Window/NewStoreCanvas");
			WindowClone.GetComponent<UIStore> ().DiamondButton ();
			transform.GetComponent<Button> ().enabled = false;
			Invoke ("EnableButton", 0.5f);
			return;
		}

		
		if (!isInActive) {
			Debug.LogError (" restNum2222 = " + restNum);
			SendUseSkillRequest ();
			stopSkill = false;
			//SetLocalGunUseSkill (true);
			mask.fillAmount = 1;
			//如果购买龙卡的用户则为true;
			isautoFire = false;
			//isInActive = true;
			return;
		}
		//else//非龙卡不能取消自动
		//{
		//    mask.fillAmount = 0;
		//    transform.Find("Lock").gameObject.SetActive(true);
		//    transform.Find("CanleLock").gameObject.SetActive(false);
		//    GameController._instance.isStopLockAndAudio = true;
		//    SetLocalGunUseSkill(false);
		//    isInActive = false;

		//    return;
		//}
	}

	/// <summary>
	/// 取消所有技能，取消技能根据Type
	/// </summary>
	/// <param name="skillType">Skill type.</param>
	public  void CompleteAllUseSkill (SkillType skillType)//
	{
		GunControl gunControl = PrefabManager._instance.GetLocalGun ();

		switch (skillType) {
		case SkillType.Berserk:
			if (isInActive) {
				Facade.GetFacade ().message.fishCommom.SendOtherCancelSkill ((int)skillType, myInfo.userID);
				stopSkill = true;
				transform.Find ("Berserk").gameObject.SetActive (true);
				transform.Find ("CancleBerserk").gameObject.SetActive (false);
				isInActive = false;
				if (PrefabManager._instance == null) {
					return;
				}
				gunControl.UserCanCelSkill (SkillType.Berserk, skillDuration, false);
				timer = 0f;
				mask.fillAmount = 0;	
			}
			break;
		case SkillType.Replication:
			if (isInActive) {
				transform.Find ("Replication").gameObject.SetActive (true);
				transform.Find ("CancleReplication").gameObject.SetActive (false);
				Facade.GetFacade ().message.fishCommom.SendOtherCancelSkill ((int)skillType, myInfo.userID);
				stopSkill = true;
				isInActive = false;
				if (PrefabManager._instance == null) {
					return;
				}
				gunControl.UserCanCelSkill (SkillType.Replication, skillDuration, false);
				timer = 0f;
				mask.fillAmount = 0;
			}
			break;
		case SkillType.Lock:
			if (isInActive) {
				if (myInfo.misHaveDraCard) {
					transform.Find ("Lock").gameObject.SetActive (true);
					transform.Find ("CanleLock").gameObject.SetActive (false);
					stopSkill = true;
					SetLocalGunUseSkill (false);
					isInActive = false;
				} else {
					timer = 0f;
					mask.fillAmount = 0;
					stopSkill = true;
					if (PrefabManager._instance == null) {
						return;
					}
					gunControl.CompleteLockSkill ();
					//SetLocalGunUseSkill (false);
					isInActive = false;
				}	
			}
			break;
		default:
			break;
		}
	}

	public void DraUseAuFire ()
	{
		if (PrefabManager._instance.GetLocalGun ().isBankruptcy) {//破产状态下限制使用分身道具
			HintTextPanel._instance.SetTextShow("破產狀態下無法使用自動開砲");
			return;
		}
		if (skillType == SkillType.AutoFire) {

			if (GameController._instance.isDebugMode)
				isLock = false;
//			if (isLock) {
//				//PrefabManager._instance.ShowNormalHintPanel("提示：月卡用户可使用此功能");
//				PrefabManager._instance.ShowNormalHintPanel ("提示：升级到vip1可使用此功能");
//				return;
//			}

			if (!isInActive) {
				transform.Find ("Name_CancleAuto").gameObject.SetActive (true);
				transform.Find ("Name_AutoFire").gameObject.SetActive (false);
				SetLocalGunUseSkill (true);
				//如果购买龙卡的用户则为true;
				isautoFire = true;
				isInActive = true;
				return;
			} else {
				transform.Find ("Name_CancleAuto").gameObject.SetActive (false);
				transform.Find ("Name_AutoFire").gameObject.SetActive (true);
				SetLocalGunUseSkill (false);
				isInActive = false;
				return;
			}
		}

	}

	public void UseAutoFire ()
	{
		if (PrefabManager._instance.GetLocalGun ().isBankruptcy) {//破产状态下限制使用分身道具
			HintTextPanel._instance.SetTextShow("破產狀態下無法使用自動開砲");
			return;
		}
		Debug.LogError ("是否点击了????");
		if (skillType == SkillType.AutoFire) {
//			if (GameController._instance.isDebugMode)
//				isLock = false;
//			isLock = isReceiveServerLock;
//			if (isLock) {
//				//PrefabManager._instance.ShowNormalHintPanel("提示：月卡用户可使用此功能");
//				PrefabManager._instance.ShowNormalHintPanel ("提示：升级到vip1可使用此功能");
//				return;
//			}
			Debug.LogError ("isInActive = " + isInActive);
			if (isInActive) {
				return;
			}
		
			if (GameController._instance.isDebugMode)
				isLock = false;
			//等待服务器添加自动开炮的协议内容
			if (restNum <= 0) {
				if (PrefabManager._instance.GetLocalGun ().curretnDiamond >= priceNum) {
					Debug.LogError ("====" + PrefabManager._instance.GetLocalGun ().curretnDiamond);
					SendBuyOneRequest ();
					transform.GetComponent<Button> ().enabled = false;
					Invoke ("EnableButton", 0.5f);
					return;
				} else {
					GameObject WindowClone = AppControl.OpenWindow ("Window/NewStoreCanvas");
					WindowClone.GetComponent<UIStore> ().DiamondButton ();
					transform.GetComponent<Button> ().enabled = false;
					Invoke ("EnableButton", 0.5f);
				}
			}
		}

	}


	public void CancleAutoFire ()
	{
		Debug.LogError ("----------1--1---------");
		if (skillType == SkillType.AutoFire) {
			if (isInActive) {
				UseAutoFire ();
			}
		}
	}

	public void RepareToUseSkill ()
	{
		if (skillType == SkillType.Error)
			return;
		if (isInActive || restNum <= 0) {
			return;
		}
		if (skillType == SkillType.Lock) {
			mask.fillAmount = 1f;
		} else if (skillType == SkillType.Torpedo) {
			
		}
		//与ActiveSkill的区别在于这里不需要做技能cd处理，也不需要减少技能剩余数量
		SetLocalGunUseSkill ();
	}

	void CancelSkill ()
	{
		switch (skillType) {
		case SkillType.AutoFire:
			break;
		default:
			break;
		}
	}


	public  void SetLocalGunUseSkill (bool shouldUse = true)
	{
		GunControl gun = PrefabManager._instance.GetLocalGun ();
		if (null == gun) {
			Debug.Log ("Error!Can't find localGun");
			return;
		}
		if (skillType != SkillType.Torpedo) {
			switch (skillType) {
			case SkillType.Lock:
			case SkillType.Berserk:
			case SkillType.Replication:
				gun.UseSkill (skillType, skillDuration, stopSkill);
				break;
			default:
				gun.UseSkill (skillType, skillDuration, shouldUse);
				break;
			}

		} else { //如果是鱼雷，在这里才触发钻石购买
			if (restNum > 0) {
//				ReduceRestNum ();
				if (restNum == 0) {
					GameController._instance.isHaveTorpedo = false;
					//  Debug.Log("没有鱼雷");
				} else {
					GameController._instance.isHaveTorpedo = true;
					//  Debug.Log("还有鱼雷");
				}
				gun.UseSkill (skillType, skillDuration, shouldUse, torpedoLevel);
				//发射鱼雷后不收回鱼雷面板
				//SetTorpedoBarShow (false);
			} else {
				//GameController._instance.isHaveTorpedo = false;
				if (GameController._instance.myGameType == GameType.Classical) {
					if (PrefabManager._instance.GetLocalGun ().curretnDiamond >= priceNum) {
						SendBuyOneRequest ();
						this.GetComponent<Button> ().enabled = false;
						Invoke ("EnableButton", 0.5f);
					} else {
						Debug.LogError ("钻石数不足够买道具");
					}
				}
				
			}

		}

	}

	public  void AddRestNum (int addNum)
	{
		Debug.LogError ("1111111111111111111111111111");
		restNum += addNum;
		restNumText.text = restNum.ToString ();
		if (skillType == SkillType.Torpedo) {
            
			string sTropedoAllCount = transform.parent.parent.Find ("RestNum").GetComponent<Text> ().text;
			int nTropedoAllCount = 0;
			if (sTropedoAllCount == "") {
				nTropedoAllCount = 0;
			} else {
				nTropedoAllCount = int.Parse (sTropedoAllCount);
			}
			nTropedoAllCount += addNum;
			// Debug.LogError("sTropedoAllCountssssssss" + sTropedoAllCount);
			// Debug.LogError("nTropedoAllCountnnnnnn"+nTropedoAllCount);
			transform.parent.parent.Find ("RestNum").GetComponent<Text> ().text = nTropedoAllCount.ToString ();
			// Debug.LogError("鱼雷总个数"+transform.parent.parent.Find("RestNum").GetComponent<Text>().text);
		}
		if (restNum > 0)
			SetPriceNumShow (false);
	}

	public bool ReduceRestNum (bool firstJoin = false) //技能剩余数量大于等于1时，减1，否则不减
	{
		//如果是龙卡的话 使用锁定是不消耗的
		if (myInfo.misHaveDraCard && skillType == SkillType.Lock) {
			return true;
		}
			
		if (GameController._instance.isExperienceMode) {
			return true;
		}
		if (restNum >= 1) {
			restNum -= 1;
			//Debug.LogError ("ReduceSkillRestNum Sucess:" + restNum.ToString ());
			restNumText.text = restNum.ToString ();
			//         if (IsRepareTorpedo ()) {
			//             torpedoAllCount -= 1;
			//	RefreshTorpedoCount (torpedoAllCount);          
			//}
			if (skillType == SkillType.Torpedo && !repareToUse) {
				if (!firstJoin) {
					torpedoAllCount -= 1;
				}
				RefreshTorpedoCount (torpedoAllCount);  
			}
			if (restNum == 0) {
				if (GameController._instance.myGameType == GameType.Classical) {
					SetRestNumShow (false);
					SetPriceNumShow (true);
				}
			}
			return true;
		} else {
			return false; //剩余数量少于1时不做减少处理，返还失败结果
		}
	}

	void RefreshTorpedoCount (int count)
	{
//		Debug.LogError ("RefreshTorpedoCount repareToUse = " + repareToUse);
//		Debug.LogError ("RefreshTorpedoCount count = " + count);
		//	if (repareToUse) {
		if (torpedoCountText == null) {
			torpedoCountText = transform.Find ("RestNum").GetComponent<Text> ();
		}
		if (count <= 0) {
			//torpedoCountText.text = "";
			transform.parent.parent.Find ("RestNum").GetComponent<Text> ().text = "";
		} else {
			transform.parent.parent.Find ("RestNum").GetComponent<Text> ().text = torpedoAllCount.ToString ();
			//torpedoCountText.text = count.ToString ();
		}
		//	}
	}

	void SetRestNumShow (bool toShow)
	{
		if (toShow) {
		
		} else {
			if (restNumText == null)
				restNumText = transform.Find ("RestNum").GetComponent<Text> ();
			restNumText.text = "";
//			Debug.LogError ("SetRestNumShow=0");
		}
		//restNumText.gameObject.SetActive (toShow);
	}

	void SetPriceNumShow (bool toShow)
	{
		if (priceNumText == null)
			priceNumText = transform.Find ("PriceNum").GetComponentInChildren<Text> ();
	
        
		if (skillType == SkillType.Torpedo && dropAddNum == 0) { //如果是掉落出来的鱼雷不执行这些
			if (toShow) {
				if (greyTorpedoIcon != null)
					transform.Find ("Image").GetComponent<Image> ().sprite = greyTorpedoIcon;
			} else {
				transform.Find ("Image").GetComponent<Image> ().sprite = orginalTorpedoIcon;
			}
			priceNumText.transform.parent.gameObject.SetActive (false);
			return;
		}

		switch (GameController._instance.myGameType) {
		case GameType.Classical:
			priceNumText.transform.parent.gameObject.SetActive (toShow);
			break;
		case GameType .Bullet:
			priceNumText.transform.parent.gameObject.SetActive (false);
			break;
		case GameType.Point:
			priceNumText.transform.parent.gameObject.SetActive (false);
			break;
		case GameType.Time:
			priceNumText.transform.parent.gameObject.SetActive (false);
			break;
		default:
			priceNumText.transform.parent.gameObject.SetActive (toShow);
			break;
		}

	}

	public  void FreezeEnemy (List<EnemyBase> tempEnemyGroup)
	{
		if (tempEnemyGroup == null) {
			return;
		}
		for (int i = 0; i < tempEnemyGroup.Count; i++) {
			tempEnemyGroup [i].BeFrozen ();
		}
		AudioManager._instance.PlayEffectClip (AudioManager.effect_freeze);
	}

	public  void ThawEnemy (List<EnemyBase> tempEnemyGroup)
	{
		if (tempEnemyGroup == null) {
			return;
		}
		for (int i = 0; i < tempEnemyGroup.Count; i++) {
			tempEnemyGroup [i].BeThaw ();
		}
	}

	public void ToggleShowTorpedoBar ()
	{
		PrefabManager._instance.TorpedoTipsObj.SetActive (false);
		if (torpedoBarIsShow) {
			SetTorpedoBarShow (false);
		} else {
			SetTorpedoBarShow (true);
		}
	}

	void SetTorpedoBarShow (bool shouldShow)
	{
		float x = PrefabManager._instance.TorpedoTipsObj.GetComponent<RectTransform> ().localPosition.x;
		float y = PrefabManager._instance.TorpedoTipsObj.GetComponent<RectTransform> ().localPosition.y;
		if (shouldShow) {
			torpedoBar.GetComponent<RectTransform> ().localPosition = torpedoBarOriginPos;
			PrefabManager._instance.TorpedoTipsObj.GetComponent<RectTransform> ().localPosition = new Vector3 (x - 50, y - 50);
			torpedoBarIsShow = true;
		} else {
			torpedoBar.GetComponent<RectTransform> ().localPosition = Vector3.right * 10000f;
			PrefabManager._instance.TorpedoTipsObj.GetComponent<RectTransform> ().localPosition = new Vector3 (x + 50, y + 50);
			torpedoBarIsShow = false;
		}

	}

	public static int ChangeSkillTypeToClient (int typeId)//将服务端的技能type转为本地客户端的type
	{
		int skillChangeType = -1;
		switch (typeId) {
		case FiPropertyType.FISHING_EFFECT_FREEZE:
			skillChangeType = (int)SkillType.Freeze;
			break;
		case FiPropertyType.FISHING_EFFECT_AIM:
			skillChangeType = (int)SkillType.Lock;
			break;
		case FiPropertyType.FISHING_EFFECT_VIOLENT:
			skillChangeType = (int)SkillType.Berserk;
			break;
		case FiPropertyType.FISHING_EFFECT_SUMMON:
			skillChangeType = (int)SkillType.Summon;
			break;
		case FiPropertyType.FISHING_EFFECT_DOUBLE:
			skillChangeType = (int)SkillType.Double;
			break;
		case FiPropertyType.FISHING_AUTOFIRE:
			skillChangeType = (int)SkillType.AutoFire;
			break;
		default:
			if (typeId >= FiPropertyType.TORPEDO_MINI && typeId <= FiPropertyType.TORPEDO_PK) {
				skillChangeType = (int)SkillType.Torpedo;
			}
			break;
		}
		return skillChangeType;
	}

	public static int ChangeSkillTypeToServer (int typeId, int torpedoLevel = 1)//将客户端的技能type转为服务端的type,注意鱼雷是lv1的，还要根据level转化到对应等级
	{
		int skillChangeType = -1;
		switch (typeId) {
		case (int)SkillType.Error:
			break;
		case (int)SkillType.Freeze:
			skillChangeType = FiPropertyType.FISHING_EFFECT_FREEZE;
			break;
		case (int)SkillType.Lock:
			skillChangeType = FiPropertyType.FISHING_EFFECT_AIM;
			break;
		case (int)SkillType.Berserk:
			skillChangeType = FiPropertyType.FISHING_EFFECT_VIOLENT;
			break;
		case (int)SkillType.Summon:
			skillChangeType = FiPropertyType.FISHING_EFFECT_SUMMON;
			break;
		case (int)SkillType.Torpedo:
			skillChangeType = FiPropertyType.TORPEDO_MINI;
			skillChangeType = skillChangeType + torpedoLevel - 1;
			break;
		case (int)SkillType.Double:
			skillChangeType = FiPropertyType.FISHING_EFFECT_DOUBLE;
			break;
		case (int)SkillType.Replication:
			skillChangeType = FiPropertyType.FISHING_EFFECT_REPLICATE;
			break;
		case (int)SkillType.AutoFire:
			skillChangeType = FiPropertyType.FISHING_AUTOFIRE;
			break;
		default:
			break;
		}
		return skillChangeType;
	}

	public static int GetTorpedoLevelFromServerId (int clientSkillType) //根据服务端的鱼雷id获取客户端里鱼雷对应的等级
	{
		int level = -1;
		if (clientSkillType >= FiPropertyType.TORPEDO_MINI && clientSkillType <= FiPropertyType.TORPEDO_PK) {
			level = clientSkillType - FiPropertyType.TORPEDO_MINI + 1;
		}
		return level;
	}

	public static int GetServerIdFromTorpedoLevel (int tempLevel)
	{
		return FiPropertyType.TORPEDO_MINI + tempLevel - 1;
	}


	public void SetPkInfoShow (int restNum)
	{
		// transform.parent = ScreenManager.uiScaler.transform;  //是否有必要设置子物体在根目录？
		this.SetPriceNumShow (false);
		this.restNum = restNum;
		restNumText.text = restNum.ToString ();
		//SetPropBoxShow (true); //新版ui不需要这段外框
	}


	public void SetClickable (bool canClick)
	{
		this.GetComponent<Button> ().enabled = canClick;
	}

	public void SetPropBoxShow (bool shouldShow)
	{
		if (quadImage != null) {
			quadImage.gameObject.SetActive (shouldShow);
		}
		if (transform.Find ("BgBubble") != null && shouldShow) {
			transform.Find ("BgBubble").GetComponent<Image> ().color = Color.clear;
		}
	}

	void InitNum ()
	{//刚进入渔场时从背包初始化数量
		FiBackpackProperty tempInfo = DataControl.GetInstance ().GetBackpackInfo ().Get (serverTypeId);
		if (tempInfo == null) {
			//Debug.Log ("获取背包道具数据失败！" + this.skillType);
			SetZeroRest (true);
			return;
		}


		if (tempInfo.count == 0) {
			//Debug.Log (tempInfo + "tempinfo");
			SetZeroRest (true);
		} else {
			if (skillType == SkillType.Torpedo) {
				GameController._instance.isHaveTorpedo = true;
				//  Debug.Log("z这是鱼雷啊啊啊啊"+tempInfo.count);
			}
			//Debug.Log ("tempInfo.count" + tempInfo.count);
			restNum = tempInfo.count;
			restNumText.text = restNum.ToString ();
			if (skillType == SkillType.AutoFire) {
				restNum = 0;
				restNumText.text = restNum.ToString ();
			}

			//         ////id在迷你鱼雷和pk鱼雷之间的话,计算鱼雷总数
			//         if (repareToUse) {
			//         ////Debug.LogError("tempInfo.id = "+tempInfo.id);
			//         ////Debug.LogError("tempInfo.count= " + tempInfo.count);
			//         //if (tempInfo.id ==FiPropertyType.TORPEDO_MINI )
			//         //{
			//         //    Debug.LogError("mini.count= " + tempInfo.count);
			//         //}
			//             torpedoAllCount = 
			//}
			//Debug.LogError("Init Num torpedoAllCount = "+torpedoAllCount);
		}

	}

	public void UpdateNumToBackup ()
	{//离开渔场时把数量传至背包
		
	}

	void SetZeroRest (bool firstJoin = false)
	{
		restNum = 1;
		ReduceRestNum (firstJoin);
	}

	void SendBuyOneRequest ()
	{
		//BuySucess ();
		PurchaseMsgHandle temp = new PurchaseMsgHandle ();
		temp.SendPurchaseRequest (serverTypeId, 1);//这个协议要加个自动登录的消费id 应该是为-1
	}

	//自动开炮的地方就走这吧
	public void BuySucess (int diamondCost, int addNum, bool isAutoFire = false)
	{
		this.restNum += addNum;
		GunControl gun = PrefabManager._instance.GetLocalGun ();
		if (null == gun) {
			Debug.Log ("Error!Can't find localGun");
			return;
		}
		gun.gunUI.AddValue (0, 0, -priceNum);
		if (skillType != SkillType.Torpedo) {
			SendUseSkillRequest ();
		} else {
			SetLocalGunUseSkill ();
		}
		return;
		Debug.LogError ("BuySuccess:cost=" + diamondCost + " addNum=" + addNum);
		if (skillType != SkillType.Lock) {
			this.restNum += addNum;
		}
		//	GunControl gun = PrefabManager._instance.GetLocalGun ();
		if (null == gun) {
			Debug.Log ("Error!Can't find localGun");
			return;
		}
		gun.gunUI.AddValue (0, 0, -priceNum);

		if (skillType == SkillType.AutoFire || skillType == SkillType.Lock) {
			Debug.LogError ("------receve_____AutoFire");
			AutoFireSkill ();
			SetLocalGunUseSkill (true);
			return;
		} else if (skillType != SkillType.Torpedo) {
			Debug.LogError ("------receve_____");
			SendUseSkillRequest ();
		} else {
			SetLocalGunUseSkill (true);
		}
	
	}

	int dropAddNum = 0;
	GunControl dropMoveTargetGun = null;

	public void DropAndMove (int num, int userId, Vector3 dropWorldPos)
	{ //当鱼掉落的道具时，会找到对应的Skill对象，复制一份GameObject，然后执行该函数转化到掉落模式（不允许点击按钮，不显示钻石）
     
		//Vector3 dropUIPos = ScreenManager.WorldToUIPos (dropWorldPos)+Vector3.down*2;
		Vector3 dropUIPos = ScreenManager.WorldToUIPos (dropWorldPos);

		//生成装载技能图标的动画容器
		Transform canvasTrans = GameObject.FindGameObjectWithTag (TagManager.uiCanvas).transform;
		GameObject tempHandle = GameObject.Instantiate (PrefabManager._instance.skillUIDropHandle, canvasTrans) as GameObject;
		tempHandle.GetComponent<RectTransform> ().localScale = Vector3.one;
		tempHandle.GetComponent<RectTransform> ().localPosition = dropUIPos;
	
		transform.parent = tempHandle.transform.Find ("_Effect_UI_PropFull_P");
		transform.GetComponent<RectTransform> ().localPosition = Vector3.zero;

		if (skillType == SkillType.Torpedo) {
			this.GetComponent<RectTransform> ().localScale = Vector3.one * 2f;
		} else if (skillType == SkillType.Lock || skillType == SkillType.Freeze) {
			this.GetComponent<RectTransform> ().localScale = Vector3.one * 2f;
		} else {
			this.GetComponent<RectTransform> ().localScale = Vector3.one;
		}

		dropAddNum = num;
		SetClickable (false);
		transform.Find ("RestNum").GetComponent<Text> ().color = new Color (0, 0, 0, 0);
		SetPriceNumShow (false);
		transform.Find ("Mask").GetComponent<Image> ().enabled = false;

		
	
		if (restNumText == null) {
			restNumText = transform.Find ("RestNum").GetComponent<Text> ();
		}

		this.restNumText.text = num.ToString ();
		dropMoveTargetGun = UIFishingObjects.GetInstance ().cannonManage.GetInfo (userId).cannon;
		if (skillType == SkillType.Torpedo) {
			Invoke ("DelayMove", 1.5f);//鱼雷要停留久一点，配合播放粒子特效
		} else {
			tempHandle.transform.Find ("_Effect_UI_PropFull_P/_Effect_UI_PropFull_yulei").gameObject.SetActive (false);
			Invoke ("DelayMove", 0.6f);//先让图标蹦跶一下再移动
		}
		
	}

	void DelayMove ()
	{
		float dropMoveDuration = Random.Range (0.5f, 0.9f);

		//transform.DOMove (dropMoveTargetGun.transform.position, dropMoveDuration);
		if (dropMoveTargetGun.gameObject == null) {
			Destroy (this.gameObject);
			return;
		}
            
		transform.parent.parent.DOMove (dropMoveTargetGun.transform.position, dropMoveDuration);
		if (dropMoveTargetGun.isLocal) {//
			Invoke ("AddOneLocalSkillNum", dropMoveDuration);
		} else {
			Destroy (this.gameObject, dropMoveDuration);
		}
	}

	void AddOneLocalSkillNum ()
	{
		PrefabManager._instance.GetSkillUIByType (this.skillType, this.torpedoLevel).AddRestNum (dropAddNum);
		//backpackInfo.Add (torpedoLevel, dropAddNum);
		//back
		Destroy (transform.parent.parent.gameObject);
		Destroy (this.gameObject);
	}

	public void StopLockSkill (bool shouldReduceNum = false) //当启用分身时，如果正在锁定中，则启用此方法强制中断锁定
	{
		isInActive = false;   
		mask.fillAmount = 0;
		if (myInfo.misHaveDraCard) {
			transform.Find ("Lock").gameObject.SetActive (true);
			transform.Find ("CanleLock").gameObject.SetActive (false);
		}
		if (shouldReduceNum)
			ReduceRestNum ();
	}

	public void AutoFireSkill ()
	{
		if (isInActive) {
			return;
		}
//
		isInActive = true;   //控制技能cd显示
		mask.fillAmount = 1;

	}

	void ShowMouthCardPanel ()
	{
//        AudioManager._instance.PlayEffectClip(AudioManager.ui_click);
//        string path = "Window/MouthCardWindow";
//        GameObject WindowClone = AppControl.OpenWindow(path);
//        WindowClone.SetActive(true);
//
//        UIMouthCard nMonthData = WindowClone.GetComponent<UIMouthCard>();
//        //如果已经领取了，那么显示获取按钮，可以继续购买月卡礼包，增加持续时间
//        MyInfo myInfo = DataControl.GetInstance().GetMyInfo();
//        nMonthData.SetRemianDays((int)myInfo.loginInfo.monthlyCardDurationDays, myInfo.loginInfo.monthlyPackGot);
	}

	public	void SetLockIconShow (bool toShow)
	{
//		Debug.Log ("toShow = " + toShow);
		if (transform.Find ("LockIcon") != null) {
			transform.Find ("LockIcon").GetComponent<Image> ().enabled = toShow;
		}
	}

	public void HideOutScreen ()
	{
		if (skillType != SkillType.AutoFire)
			transform.position = new Vector3 (10000, 0, 0);
		else {
            
		}
	}

	bool IsRepareTorpedo ()
	{
		if (skillType == SkillType.Torpedo && repareToUse)
			return true;
		else
			return false;
	}

	void EnableButton ()
	{
		this.GetComponent<Button> ().enabled = true;
	}

	void OnDestroy ()
	{
		torpedoAllCount = 0;
	}
}
