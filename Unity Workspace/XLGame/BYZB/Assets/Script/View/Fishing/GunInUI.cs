using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using AssemblyCSharp;
using LitJson;
using DG.Tweening;

public class GunInUI : MonoBehaviour
{

	//用户信息
	FiUserInfo userUIInfo;
	public  int userID = -1;
	public bool isLocal = false;
	//是否为本地玩家
	public  GunSeat thisSeat;

	public GameObject gunPrefab;
	Camera uiCamera;
	Transform childGun;

	GameObject gunObj;
	public  GunControl gunControl;

	Transform userInfoPanel;
	Transform multiplePanel;
	Transform pk_UserInfoPanel;
	[HideInInspector]
	public Transform userRankPanel;
	Transform multipleText;
	public GameObject UserRankObj;

	public GameObject BigwinRank;

	public int mRank;

	public GameObject[] UpOrdown;




	Image gender;
	//1男2女 0是游客默认女
	Text goldNum;
	Text diamondNum;
	Text nickName;
	Text multiple;
	GameObject textInfo;
	GameObject waitJoinText;
	
	Image addBtn;
	Image subBtn;
	//正式
	//string url = "http://admin.xinlongbuyu.com/GameAPI/API.aspx?ajaxaction=UnlockFire";
	string url = "";
	//测试
	//string url = "http://admin.xinlongbuyu.com/GameAPI/API.aspx?ajaxaction=unlockfiretest";

	//渔场玩家头像
	Image handPicture;

	public Sprite[] frameHand;

	//MyInfo myInfo;

	DragonCardInfo dragonCardInfo;


	//Pk
	Text bulletNumText;
	[HideInInspector]public  Text scoreText;
	GameObject doubleSkillEffect;
	Transform doubleSkillPos;
	Image avatorImage;

	[HideInInspector]
	public Transform goldImage;
	//用作金币飞行轨迹的终点
	ParticleSystem goldAcceptPS;
	//金币接收特效
	[HideInInspector]
	public  Transform effectNumPos;
	//显示接收到金币数字时需要获取到位置
	[HideInInspector]
	public GameObject tempBonusEffect = null;
	[HideInInspector]public Transform bonusEffectPos;
	//打死奖金鱼时在UI附近显示的特效
	GameObject tempRedPacketEffect = null;
	GameObject tempUseSkillEffect = null;

	[HideInInspector]
	public Transform diamondImage;
	//显示pk场用户ui信息的位置
	Transform pkUserInfoPos;
	//体验场用户信息
	Transform experienceInfo;
	//显示体验场用户信息位置
	Transform experienceInfoPos;
	//体验场金币数
	Text experienceGoldNum;
	//体验场昵称
	Text experienceNickName;

	public static  int[] multipleGroup;

	public static int[] diamondCostGroup;


	public static int[] goldReturnGroup;

	public static  int[] multipleGroup1 = new int[] {
		1, 2, 3, 4, 5, 10, 15, 20, 25, 30, 35, 40, 45, 50, 60, 80, 100, 150, 200, 300, 500, 800, 1000, 3000, 5000, 7000, 9900, 20000, 30000
	};

	public static int[] diamondCostGroup1 = new int[] { 
		0, 3, 3, 3, 3, 3, 5, 6, 8, 10, 10, 10, 12, 15, 20, 25, 30, 40, 50, 100, 10, 100, 200, 300, 500, 700, 1000
	};

	public static int[] goldReturnGroup1 = new int[] {
		0, 200, 200, 200, 200, 300, 300, 300, 300, 400, 400, 400, 400, 500, 500, 500, 500, 500, 500, 500, 500, 500, 500, 500, 500, 500, 500
	};

	WWWForm wwwForm;


	RoomInfo myRoomInfo = null;

	MyInfo myInfo;

	float openRankTime = 0;
	[HideInInspector]
	public bool openRankTimeBool = true;

	bool openRankMulitpleBool = false;

	void Awake ()
	{
		url = UIUpdate.WebUrlDic[WebUrlEnum.Setting]+ "?ajaxaction=UnlockFire";
		wwwForm = new WWWForm ();
		wwwForm.AddField ("type", 1);
		StartCoroutine (SendPost (url, wwwForm));
		myRoomInfo = DataControl.GetInstance ().GetRoomInfo ();
		dragonCardInfo = (DragonCardInfo)Facade.GetFacade ().data.Get (FacadeConfig.UI_DRAGONCARD);
		myInfo = DataControl.GetInstance ().GetMyInfo ();
		Init ();
		// StartCoroutine("ChangeRankAndMultiple");
	}

	void Start ()
	{
		//StartCoroutine("ChangeRankAndMultiple");
	}

	private void Update ()
	{
		openRankTime += Time.deltaTime;
		if (openRankTime >= 5 && openRankTimeBool) {
			//openRankTime += Time.deltaTime;
			OpenAndCloseRank (openRankMulitpleBool);
			openRankTime = 0;
		}
	}

	/// <summary>
	/// 发送post请求获取炮倍数
	/// </summary>
	/// <returns>The post.</returns>
	/// <param name="_url">URL.</param>
	/// <param name="_wwwForm">Www form.</param>
	IEnumerator SendPost (string _url, WWWForm _wwwForm)
	{
		WWW postData = new WWW (_url, _wwwForm);
		yield return postData;
		//为了防止web崩溃获取不到数据,写了一套备用方案来获取
		if (postData == null) {
			multipleGroup = multipleGroup1;
			diamondCostGroup = diamondCostGroup1;
			goldReturnGroup = goldReturnGroup1;
		} else {
#if UNITY_EDITOR
			Debug.Log(" 泡倍數  "+ postData.text);
#endif
			string content = postData.text;	
			JsonData jd = JsonMapper.ToObject (content);

			int multipleCount = (int)jd ["Data"] [0] ["multiple"].Count;
			int diamondCostCount = (int)jd ["Data"] [0] ["diamond_cost"].Count;
			int goldReturnCount = (int)jd ["Data"] [0] ["gold_reward"].Count;

			multipleGroup = new int[multipleCount];
//			Debug.Log (multipleGroup [0] + " = multipleGroup[0]");
			for (int i = 0; i < multipleCount; i++) {
				multipleGroup [i] = (int)jd ["Data"] [0] ["multiple"] [i];
				//multipleGroup[i] = multipleGroup1[i];
			}

			diamondCostGroup = new int[diamondCostCount];
			for (int i = 0; i < diamondCostCount; i++) {
				diamondCostGroup [i] = (int)jd ["Data"] [0] ["diamond_cost"] [i];
			}

			goldReturnGroup = new int[goldReturnCount];
			for (int i = 0; i < goldReturnCount; i++) {
				goldReturnGroup [i] = (int)jd ["Data"] [0] ["gold_reward"] [i];
			}

		}
	}

	void Init ()
	{
		uiCamera = GameObject.FindGameObjectWithTag (TagManager.uiCamera).GetComponent<Camera> ();
		childGun = transform.Find ("Gun").transform;
		JudgeIsIponeXDec ();
		GetChildUI ();
		textInfo.gameObject.SetActive (false);

		//SetUIInfo ("等待用户加入", 0, 0, 0, 0);
		SetUIShow (false);
		CreateGunInWorld ();
		//MoveGun ();
		Invoke ("MoveGun", 0.2f); //需要延时生成，如果马上生成，容易造成UI还没有根据屏幕比例定位完，就已经在3d世界生成了炮台
		Invoke ("JudgeIsRookie", 1f);
		Invoke ("JudgeIsHighPower", 1f);
		//JudgeIsRookie ();
	}

	/// <summary>
	/// 判断是否是新手场
	/// </summary>
	void  JudgeIsRookie ()
	{
		//return;
		if ((myRoomInfo.roomMultiple == 0 || myRoomInfo.roomMultiple == 6) && gunControl.maxCannonMultiple >= 50 && isLocal) {
			Debug.Log ("JudgeIsRookie isLocal = " + isLocal);
			SetMultiple (50);
		}
        //		return;
        if (myRoomInfo.roomMultiple == 1 && gunControl.maxCannonMultiple >= 1000 && isLocal)
        {
            Debug.Log("isLocal = " + isLocal);
            SetMultiple(1000);
        }
    }

	/// <summary>
	/// 判断是否是新手场
	/// </summary>
	void JudgeIsHighPower ()
	{
        //if ((myRoomInfo.roomMultiple != 0 && myRoomInfo.roomMultiple != 4) && gunControl.maxCannonMultiple >= 15000 && isLocal) {
		if ((myRoomInfo.roomMultiple ==5) && gunControl.maxCannonMultiple >= 15000 && isLocal) {
			Debug.Log ("JudgeIsRookie isLocal = " + isLocal);
			SetMultiple (9900);
		}
	}

	/// <summary>
	/// 是否是iphonex
	/// </summary>
	void JudgeIsIponeXDec ()
	{
		if (Facade.GetFacade ().config.isIphoneX2 ()) {
			foreach (Transform child in transform.transform) {
				float x = child.localPosition.x;	
				float y = child.localPosition.y;
				float z = child.localPosition.z;
	
				if (child.tag == TagManager.userInfo || child == transform) {
					child.localPosition = new Vector3 (x, y, z);
				} else {
					switch (thisSeat) {
					case GunSeat.LB:
						child.localPosition = new Vector3 (x + 45, y, z);
						break;
					case GunSeat.RB:
						child.localPosition = new Vector3 (x - 45, y, z);
						break;
					case GunSeat.RT:
						child.localPosition = new Vector3 (x - 45, y, z);
						break;
					case GunSeat.LT:
						child.localPosition = new Vector3 (x + 45, y, z);
						break;
					default:
						break;
					}
				}
			}
		}
	}

	public void CreateGunInWorld ()
	{
		Vector3 vct3 = new Vector3 (1000f, 0, 0);

		gunObj = GameObject.Instantiate (gunPrefab, vct3, Quaternion.identity) as GameObject;

		//Debug.LogError("CreateGunInWorld thisSeat:"+thisSeat);
		int seatNum = 0;
		switch (thisSeat) {
		case GunSeat.LB:
			seatNum = 0;
			break;
		case GunSeat.RB:
			seatNum = 1;
			break;
		case GunSeat.RT:
			seatNum = 2;
			break;
		case GunSeat.LT:
			seatNum = 3;
			break;
		}

		
		
		// PrefabManager._instance.gunGroup[(int)thisSeat] = gunObj; //根据座位号给GunGroup数组对应的位置赋值
		childGun.gameObject.SetActive (false);  
		SetMultipleInfoShow (0, false, false); //初始化时候只是创建炮台，并没有用户信息，所以炮倍数之类的先隐藏

		gunControl = gunObj.GetComponent<GunControl> ();
		gunControl.gunUI = this;
		gunControl.thisSeat = this.thisSeat;
		//UIFishingMsg.GetInstance ().cannonManage.SetGunControl (gunControl);

		if (null != PrefabManager._instance) { //PrefabManager的GunGroup初始化要比cannonManage快，否则会有bug
			//Debug.LogError("CreateGunInWorld 111 thisSeat:" + seatNum);
			PrefabManager._instance.gunGroup [seatNum] = gunObj;
		} else {
			Debug.LogError ("CreateGunInWorld Error! thisSeat:" + seatNum);
		}

		UIFishingObjects.GetInstance ().cannonManage.SetGunControl (gunControl);
	}

	void MoveGun ()
	{
		if (null != gunObj) {
			Vector3 screenPos = uiCamera.WorldToScreenPoint (childGun.position); //先将UI层的炮台图标由世界坐标转到屏幕坐标
			Vector3 worldPos = Camera.main.ScreenToWorldPoint (screenPos); //之后将该屏幕坐标转到世界坐标，并生成3d世界中的炮台
			gunObj.transform.position = worldPos;
			
			if (thisSeat == GunSeat.LT || thisSeat == GunSeat.RT) {
				gunObj.transform.Rotate (Vector3.forward, 180f);
			}

			gunControl = gunObj.GetComponent<GunControl> ();
			if (null != gunControl) {//显示有用户的炮
				gunControl.ShowUserGun ();
			}

		}

	}

	void GetChildUI ()
	{
		userInfoPanel = transform.Find ("UserInfo");
		multiplePanel = transform.Find ("CannonMultiple");
		userRankPanel = transform.Find ("CannonMultiple/Text");
		multipleText = transform.Find ("CannonMultiple/Multiple");
		//userRankPanel.gameObject.SetActive (true);
		//添加头像
		handPicture = transform.Find ("UserInfo/Hand").GetComponent<Image> ();
	
		nickName = transform.Find ("UserInfo/NickName").GetComponent<Text> ();

		multiple = transform.Find ("CannonMultiple/Multiple").GetComponent<Text> ();
		addBtn = transform.Find ("CannonMultiple/Add").GetComponent<Image> ();
		subBtn = transform.Find ("CannonMultiple/Sub").GetComponent<Image> ();

		goldImage = transform.Find ("UserInfo/Gold/GoldImage").transform;
		goldNum = transform.Find ("UserInfo/Gold/GoldImage/GoldNum").GetComponent<Text> ();
		diamondImage = transform.Find ("UserInfo/Diamond/DiamondImage").transform;
		diamondNum = transform.Find ("UserInfo/Diamond/DiamondImage/DiamondNum").GetComponent<Text> ();

		gender = transform.Find ("UserInfo/Gender").GetComponent<Image> ();
		textInfo = transform.Find ("TextInfo").gameObject;
		waitJoinText = transform.Find ("WaitJoinBg/WaitJoinText").gameObject;
	
		SetLeaveTextShow (false);
		SetBankruptcyShow (false);
		goldAcceptPS = transform.Find ("UserInfo/Gold/GoldImage/Effect_GoldAccept").GetComponentInChildren<ParticleSystem> ();//接收金币时的粒子特效
		effectNumPos = transform.Find ("EffectNumPos");

		bonusEffectPos = transform.Find ("BonusEffectPos");

		//pk部分
		if (GameController._instance.myGameType != GameType.Classical) {
			pk_UserInfoPanel = transform.Find ("Pk_UserInfo");
			pkUserInfoPos = transform.Find ("PkUserInfoPos");
			bulletNumText = pk_UserInfoPanel.Find ("BulletNum_Sprite/BulletNum_Text").GetComponent<Text> ();
		
			doubleSkillPos = pk_UserInfoPanel.Find ("DoubleSprite");
			doubleSkillPos.GetComponent<SpriteRenderer> ().enabled = false;

			if (GameController._instance.myGameType != GameType.Classical) {
				nickName = pk_UserInfoPanel.Find ("NickName").GetComponent<Text> ();
				gender = pk_UserInfoPanel.Find ("Gender").GetComponent<Image> ();
			}

			scoreText = pk_UserInfoPanel.Find ("ScoreBar/Score_Sprite/ScoreText").GetComponent<Text> ();
			avatorImage = pk_UserInfoPanel.Find ("AvatorBox/Avator").GetComponent<Image> ();
		}
		//体验场部分
		if (GameController._instance.isExperienceMode) {
			experienceInfo = transform.Find ("ExperienceInfo");
			Debug.LogError ("experienceInfo = experienceInfo.name" + experienceInfo.name);
			experienceInfoPos = transform.Find ("ExperienceInfoPos");
			experienceNickName = experienceInfo.Find ("NickName").GetComponent <Text> ();
			experienceGoldNum = experienceInfo.Find ("Gold/GoldImage/GoldNum").GetComponent <Text> ();
		}

		switch (GameController._instance.myGameType) {
		case GameType.Classical:
			break;
		case GameType .Bullet:
			SetBulletNum (300);
			SetScoreText (0, false);
			break;
		case GameType .Point:
			bulletNumText.transform.parent.gameObject.SetActive (false);//隐藏子弹数的UI
			SetScoreText (100, false); 
			break;
		case GameType.Time:
			bulletNumText.transform.parent.gameObject.SetActive (false);//隐藏子弹数的UI
			SetScoreText (0, false);
			break;
		default:
			break;
		}

	}


	public void SetUIInfo (FiUserInfo info, bool localFlag = false)
	{
		userUIInfo = info;
		this.nickName.text = Tool.GetName (info.nickName, 6);
		switch (myRoomInfo.roomMultiple) { 
		case 0:
			if (info.cannonMultiple > 50) {
				info.cannonMultiple = 50;
			}
			break;
		case 1:
//			if (info.cannonMultiple > 1000) {
//				info.cannonMultiple = 1000;
//			}
			break;
		default:
			break;
		}
		openRankTimeBool = true;
		this.multiple.text = info.cannonMultiple.ToString ();
		this.isLocal = localFlag;
		this.goldNum.text = info.gold.ToString ();
		this.diamondNum.text = info.diamond.ToString ();

		if (info.gender != 1) {
			//this.gender.sprite = PrefabManager._instance.genderFemale;
			this.gender.sprite = PrefabManager._instance.femaleSprite;
		}
		//if (isLocal) {
		//	SetMultipleInfoShow (info.cannonMultiple, true, true);
		//          Debug.LogError("---------------MYMYMYinfo.cannonMultiplboss场自己的倍数--------------------" + info.cannonMultiple);
		//	userRankPanel.gameObject.SetActive (false);
		//} else {
		//	if (myRoomInfo.roomMultiple == 0) {
		//		if (info.cannonMultiple > 50) {
		//			info.cannonMultiple = 50;
		//		}
		//		SetMultipleInfoShow (info.cannonMultiple, false, true);
		//          } else if (myRoomInfo.roomMultiple==3||myRoomInfo.roomMultiple == 2)
		//          {
		//              if (info.cannonMultiple < 1000)
		//              {
		//                  SetMultipleInfoShow(info.maxCannonMultiple, false, true);
		//              }
		//              else
		//              {
		//                  SetMultipleInfoShow(info.cannonMultiple, false, true);
		//              }
		//          }
		//          else {
                
		//              SetMultipleInfoShow (info.cannonMultiple, false, true);
		//              Debug.LogError("---------------info.cannonMultiplboss场--------------------"+info.cannonMultiple);
		//	}
		//	multiplePanel.gameObject.SetActive (false);
		//}
		if (isLocal) {
			SetMultipleInfoShow (info.cannonMultiple, true, true);
			if (!GameController._instance.isBossMatchMode) {
                
			} else {
				//开始刷新boss猎杀排行的协程
				StartCoroutine (GetRefershBossKillRank ()); 
			}
			userRankPanel.gameObject.SetActive (false);

		} else {
			if (myRoomInfo.roomMultiple == 0) {
				if (info.cannonMultiple > 50) {
					info.cannonMultiple = 50;
				}
				SetMultipleInfoShow (info.cannonMultiple, false, true);
			} else {
				SetMultipleInfoShow (info.cannonMultiple, false, true);
			}
			userRankPanel.gameObject.SetActive (false);
		}
		SetDragoAndNobilityFrame (info);
	}

	/// <summary>
	/// 设置体验场信息
	/// </summary>
	/// <param name="info">Info.</param>
	/// <param name="localFlag">If set to <c>true</c> local flag.</param>
	public void SetExperienceUIInfo (FiUserInfo info, bool localFlag = false)
	{
		userUIInfo = info;
		this.experienceNickName.text = Tool.GetName (info.nickName, 7);	
		this.multiple.text = info.cannonMultiple.ToString ();
		this.isLocal = localFlag;
		//设置体验币
		this.experienceGoldNum.text = info.testCoin.ToString ();
		//体验场隐藏排名
		userRankPanel.gameObject.SetActive (false);
		if (info.gender != 1) {
			//this.gender.sprite = PrefabManager._instance.genderFemale;
			this.gender.sprite = PrefabManager._instance.femaleSprite;
		}
		if (isLocal) {
			SetMultipleInfoShow (1, true, true);
		} else {
			SetMultipleInfoShow (1, false, true);
		}
	}

	public void RefrshCoin (int cannonMut, long gold = 0, long diamond = 0, bool shouldUpdateDiamondProgress = true)
	{
		Debug.Log(" 發炮更新金幣 ");
		gunControl.currentGold = gold;
		this.goldNum.text = gunControl.currentGold.ToString ();
	}

	public void AddValue (int cannonMut, int gold = 0, int diamond = 0, bool shouldUpdateDiamondProgress = true, int test_Coin = 0) //加炮倍改成用SetMultiple加，这里基本上都是0
	{
		Debug.Log(" 修改炮倍數  AddValue 549");
		if (GameController._instance.isExperienceMode) {
			gunControl.currentGold += test_Coin;
		} else {
			gunControl.currentGold += gold;
			if (isLocal) {
				myInfo.gold = gunControl.currentGold;
			}
		}
		if (gunControl.isBankruptcy) {
			if (gunControl.CheckCanFire ()) {//说明此时脱离破产情况
				gunControl.SetBankruptcyState (false);
			}
		}

		if (gunControl.currentGold >= 0) {         

		}

		if (gunControl.currentGold < 0)
			gunControl.currentGold = 0;
        
		switch (GameController._instance.myGameType) {
		case GameType.Classical:
			if (cannonMut != 0) {
				//gunControl.cannonMultiple += cannonMut;
				//this.multiple.text = gunControl.cannonMultiple.ToString ();
				SetMultiple (gunControl.cannonMultiple + cannonMut, false);

			}
			//体验场的部分
			if (GameController._instance.isExperienceMode) {
				if (test_Coin != 0) {
					if (gunControl.currentGold < 0)
						this.experienceGoldNum.text = "0";
					else {
						this.experienceGoldNum.text = gunControl.currentGold.ToString ();
					}
				}
			}
			//经典场部分 
			else {
				if (gold != 0) {
					if (gunControl.currentGold < 0)
						this.goldNum.text = "0";
					else {
						this.goldNum.text = gunControl.currentGold.ToString ();
						//这里要加入破产状态的判断更新
					}
				       
				}
				if (diamond != 0) {
					gunControl.curretnDiamond += diamond;
					myInfo.diamond = gunControl.curretnDiamond;
					if (gunControl.curretnDiamond < 0)
						this.diamondNum.text = "0";
					else
						this.diamondNum.text = gunControl.curretnDiamond.ToString ();
				}
			}
			break;
		case GameType.Bullet:
			SetScoreText ((int)gunControl.currentGold, true);
			//this.scoreText.text =(int.Parse (scoreText.text) + gold).ToString();
			break;
		case GameType.Point:
			SetScoreText ((int.Parse (scoreText.text) + gold), true);
			gunControl.GetOtherGunUI ().ChangeScore (-gold);
			break;
		case GameType.Time:
			SetScoreText ((int.Parse (scoreText.text) + gold), true);
			break;
		
		default:
			break;
		}
		if (diamond != 0 && isLocal && shouldUpdateDiamondProgress) {
			if (Panel_UnlockMultiples._instance.gameObject != null)
				Panel_UnlockMultiples._instance.UpdateDiamondPorgress ();
		}

	}

	public void AddValue (int cannonMut, long gold = 0, long diamond = 0, bool shouldUpdateDiamondProgress = true, int test_Coin = 0)
	{
		Debug.Log(" 修改炮倍數  AddValue 634");
		if (GameController._instance.isExperienceMode) {
			gunControl.currentGold += test_Coin;
		} else {
			gunControl.currentGold += gold;
			if (isLocal) {
				myInfo.gold = gunControl.currentGold;
			}
		}
		if (gunControl.isBankruptcy) {
			if (gunControl.CheckCanFire ()) {//说明此时脱离破产情况
				gunControl.SetBankruptcyState (false);
			}
		}

		if (gunControl.currentGold >= 0) {

		}

		if (gunControl.currentGold < 0)
			gunControl.currentGold = 0;

		switch (GameController._instance.myGameType) {
		case GameType.Classical:
			if (cannonMut != 0) {
				//gunControl.cannonMultiple += cannonMut;
				//this.multiple.text = gunControl.cannonMultiple.ToString ();
				SetMultiple (gunControl.cannonMultiple + cannonMut, false);

			}
                //体验场的部分
			if (GameController._instance.isExperienceMode) {
				if (test_Coin != 0) {
					if (gunControl.currentGold < 0)
						this.experienceGoldNum.text = "0";
					else {
						this.experienceGoldNum.text = gunControl.currentGold.ToString ();
					}
				}
			}
                //经典场部分 
                else {
				if (gold != 0) {
					if (gunControl.currentGold < 0)
						this.goldNum.text = "0";
					else {
						this.goldNum.text = gunControl.currentGold.ToString ();
						//这里要加入破产状态的判断更新
					}

				}
				if (diamond != 0) {
					gunControl.curretnDiamond += diamond;
					myInfo.diamond = gunControl.curretnDiamond;
					if (gunControl.curretnDiamond < 0)
						this.diamondNum.text = "0";
					else
						this.diamondNum.text = gunControl.curretnDiamond.ToString ();
				}
			}
			break;
		case GameType.Bullet:
			SetScoreText ((int)gunControl.currentGold, true);
                //this.scoreText.text =(int.Parse (scoreText.text) + gold).ToString();
			break;
		case GameType.Point:
                //SetScoreText((int.Parse(scoreText.text) + gold), true);
              //  gunControl.GetOtherGunUI().ChangeScore(-gold);
			break;
		case GameType.Time:
                //SetScoreText((int.Parse(scoreText.text) + gold), true);
			break;

		default:
			break;
		}
		if (diamond != 0 && isLocal && shouldUpdateDiamondProgress) {
			if (Panel_UnlockMultiples._instance.gameObject != null)
				Panel_UnlockMultiples._instance.UpdateDiamondPorgress ();
		}
	}

	public void SetWaitTextShow (bool show)
	{
		//Debug.LogError ("111");
		waitJoinText.gameObject.SetActive (show);
		//Debug.LogError ("222");
		waitJoinText.transform.parent.gameObject.SetActive (show);
	}

	public void SetLeaveTextShow (bool show)
	{
		transform.Find ("LeaveText").gameObject.SetActive (show);
	}

	public void SetBankruptcyShow (bool toShow)
	{ 
		transform.Find ("BankruptcyImage").gameObject.SetActive (toShow);
	}


	public  void SetMultipleInfoShow (long multipleNum = 0, bool btnShow = true, bool numShow = true)
	{
		multiplePanel.gameObject.SetActive (numShow);
		userRankPanel.gameObject.SetActive (numShow);
		if (GameController._instance != null && GameController._instance.isExperienceMode) {
			userRankPanel.gameObject.SetActive (false);
		}
		addBtn.gameObject.SetActive (btnShow);
		subBtn.gameObject.SetActive (btnShow);
		multiple.gameObject.SetActive (numShow);
		this.multiple.text = multipleNum.ToString ();

		GameType tempType = GameController._instance.myGameType;
		if (tempType == GameType.Bullet || tempType == GameType.Point || tempType == GameType.Time) {
			addBtn.gameObject.SetActive (false);
			subBtn.gameObject.SetActive (false);
		}
	}

	/// <summary>
	/// 设置贵族头像和龙卡头像调用接口
	/// </summary>
	void SetDragoAndNobilityFrame (FiUserInfo info)
	{
		if (isLocal) {
			int count = dragonCardInfo.IsPurDragonTypeArray.Count;
			if (dragonCardInfo.IsPurDragonTypeArray [count - 1] >= 1) {
				//神龙卡
				handPicture.sprite = frameHand [2];
				handPicture.enabled = true;
//               StartCoroutine(TurnTablePrizeAddition(1));
			} else if (dragonCardInfo.IsPurDragonTypeArray [count - 2] >= 1) {
				//金龙卡
				handPicture.sprite = frameHand [1];
				handPicture.enabled = true;
//                StartCoroutine(TurnTablePrizeAddition(2));
			} else if (dragonCardInfo.IsPurDragonTypeArray [count - 3] >= 1) {
				//银龙卡
				handPicture.sprite = frameHand [0];
				handPicture.enabled = true;
//                StartCoroutine(TurnTablePrizeAddition(3));
			} else {
//				Debug.Log ("m没有龙卡！！！！！" + count);
				handPicture.enabled = false;
//                StartCoroutine(TurnTablePrizeAddition(0));
				//FishingUpTurnTable._instance.isLongCard = false;
				//Debug.Log(FishingUpTurnTable._instance.isLongCard+"给龙卡的bool");
			}	
		} else {
			if (info.monthlyCardType == 0) {
				handPicture.enabled = false;
				return;
			}
//			Debug.LogError ("龙卡！！！！！！！" + info.monthlyCardType);
			handPicture.enabled = true;
			handPicture.sprite = frameHand [info.monthlyCardType - 1];
		}
	}

	bool IsLongCardNull ()
	{
		if (FishingUpTurnTable._instance == null) {
			return true;
		} else {
			return false;
		}
	}

	public void ChangeGoldNum (int changeValue)
	{
		Debug.Log("金幣顯示 ChangeGoldNum "+ changeValue);
		if (GameController._instance.isExperienceMode)
			AddValue (0, 0, 0, false, changeValue);
		else
			AddValue (0, changeValue, 0);
		return;
		int finalNum = int.Parse (goldNum.text) + changeValue;
		if (finalNum < 0)
			finalNum = 0;
		goldNum.text = finalNum.ToString ();
		gunControl.currentGold = finalNum;
	}

	public void ChangeMultiple (bool shouldAdd) //通过UI按钮的方式先在UI层改变炮倍数显示，再传到GunControl
	{
		int finalMultiple = GetFinalMultiple (shouldAdd);
		Debug.Log("   修改砲倍數  " + finalMultiple);
		SetMultiple (finalMultiple, true);
		gunControl.DoStrecthScale ();
		AudioManager._instance.PlayEffectClip (AudioManager.effect_changeEquip);
		OpenAndCloseRank (true);
		openRankTime = 0;
		openRankMulitpleBool = false;
		//openRankTimeBool = true;
	}

	public void SetMultiple (int finalNum, bool shouldSnd = true) //直接赋予改变后的最终炮倍数值,再传到GunControl
	{
		Debug.Log("   SetMultiple 修改砲倍數  " + finalNum);
		this.multiple.text = finalNum.ToString ();
		gunControl.ChangeCannonMultiple (finalNum, shouldSnd);
		if (gunControl.CheckCanFire () && gunControl.isBankruptcy)
			gunControl.SetBankruptcyState (false);
            
		if (GameController._instance.isRedPacketMode && isLocal) {
			RedPacket_TopInfo._instance.UpdateGunMultiple (finalNum);
		}
	}

	public int GetFinalMultiple1 (bool shouldAdd)
	{
		int currentMultiple = int.Parse (multiple.text);
		for (int i = 0; i < multipleGroup.Length; i++) {
			if (currentMultiple == multipleGroup [i]) {
				if (shouldAdd) { //加炮倍数
					if (multipleGroup [i] == gunControl.maxCannonMultiple) {
						currentMultiple = GameController._instance.roomMultiple; //当前渔场的最低下限倍数
					} else {
						currentMultiple = multipleGroup [i + 1];
					}
					break;
				} else { //减炮倍数
					if (i == 0) {
						currentMultiple = gunControl.maxCannonMultiple;
					} else {
						if (currentMultiple <= GameController._instance.roomMultiple) { //小于渔场最小炮倍数限制时，跳到最大
							currentMultiple = gunControl.maxCannonMultiple;
						} else {
							currentMultiple = multipleGroup [i - 1];
						}
					}
					break;
				}
			}
		}
		if (MultipleIsLegal (currentMultiple)) {
			return currentMultiple;
		} else {
			return int.Parse (multiple.text); //如果改变后的炮倍数并不合法，那么不改变炮倍，恢复原来的
		}

	}

	/// <summary>
	/// 进入新手场改变炮倍数为100
	/// </summary>
	/// <returns>The final multiple1.</returns>
	/// <param name="shouldAdd">If set to <c>true</c> should add.</param>
	public int GetFinalMultiple (bool shouldAdd)
	{
		int currentMultiple = int.Parse (multiple.text);
		for (int i = 0; i < multipleGroup.Length; i++) {
			if (currentMultiple == multipleGroup [i]) {
				if (shouldAdd) { //加炮倍数
					//新手场
					Debug.Log("  修改炮倍數 增加 roomMultiple = " + myRoomInfo.roomMultiple);
					Debug.Log("  修改炮倍數 增加 roomMultiple = " + multipleGroup[i]);
					Debug.Log("  修改炮倍數 增加 roomMultiple = " + gunControl.maxCannonMultiple);
					if (myRoomInfo.roomMultiple == 0 || myRoomInfo.roomMultiple == 6)
					{
						if (multipleGroup[i] == gunControl.maxCannonMultiple)
						{
							currentMultiple = GameController._instance.roomMultiple;
						}
						else
						{
							currentMultiple = multipleGroup[i + 1];
							if (multipleGroup[i] == 50)
							{
								currentMultiple = GameController._instance.roomMultiple;
								//return 100;
							}
						}
					}
					else if (myRoomInfo.roomMultiple == 1)
					{
						if (multipleGroup[i] == gunControl.maxCannonMultiple)
						{
							currentMultiple = GameController._instance.roomMultiple;
						}
						else
						{
							currentMultiple = multipleGroup[i + 1];
							if (multipleGroup[i] == 1000)
							{
								currentMultiple = GameController._instance.roomMultiple;
								//return 100;
							}
						}
					}
					else
					{
						if (multipleGroup[i] == gunControl.maxCannonMultiple)
						{
							currentMultiple = GameController._instance.roomMultiple; //当前渔场的最低下限倍数
						}
						else
						{
							currentMultiple = multipleGroup[i + 1];
							//							Debug.LogError ("multipleGroup [i+1] = " + multipleGroup [i + 1]);
							if (currentMultiple == 20000)
							{
								Debug.LogError("currentMultiple15000 = " + currentMultiple);
								ShowLocalGunTextPanel("使用此炮高風險,高回報");
							}
							else if (currentMultiple == 30000)
							{
								ShowLocalGunTextPanel("使用此炮超高風險,超高回報");
							}
						}
					}
					break;
				} else { //减炮倍数
					Debug.Log("  修改炮倍數 減少 roomMultiple = " + myRoomInfo.roomMultiple);
					//新手场
					if (myRoomInfo.roomMultiple == 0 || myRoomInfo.roomMultiple == 6) {
						if (i == 0) {
							currentMultiple = 50;
						} else {
							if (currentMultiple <= GameController._instance.roomMultiple) { //小于渔场最小炮倍数限制时，跳到最大
								currentMultiple = 50;

							} else {
								currentMultiple = multipleGroup [i - 1];
							}
						}					
					}
					else if (myRoomInfo.roomMultiple == 1)
					{
                        if (i == 0)
                        {
                            currentMultiple = 1000;
                        }
                        else
                        {
                            if (currentMultiple <= GameController._instance.roomMultiple)
                            { //小于渔场最小炮倍数限制时，跳到最大
                                currentMultiple = 1000;
                            }
                            else
                            {
                                currentMultiple = multipleGroup[i - 1];
                            }
                        }
                    }
                    else
					{
						if (i == 0) {
							currentMultiple = gunControl.maxCannonMultiple;
						} else {
							if (currentMultiple <= GameController._instance.roomMultiple) { //小于渔场最小炮倍数限制时，跳到最大
								currentMultiple = gunControl.maxCannonMultiple;	
//								Debug.LogError ("currentMultiple -------- = " + currentMultiple);
								if (currentMultiple == 20000) {
									ShowLocalGunTextPanel ("使用此炮超高風險,超高回報");
								} else if (currentMultiple == 15000) {
									ShowLocalGunTextPanel ("使用此炮高風險,高回報");
								}
							} else {
								currentMultiple = multipleGroup [i - 1];
								if (currentMultiple == 15000) {
									ShowLocalGunTextPanel ("使用此炮高風險,高回報");
								}
							}
						}
					}	
					break;
				}
			}
		}
		if (MultipleIsLegal (currentMultiple)) {
			return currentMultiple;
		} else {
			return int.Parse (multiple.text); //如果改变后的炮倍数并不合法，那么不改变炮倍，恢复原来的
		}

	}

	public int GetNextUnlockMultiples ()
	{
		int maxMultiples = gunControl.maxCannonMultiple;
		int finalReturn = -1;
		for (int i = 0; i < multipleGroup.Length; i++) {
			if (maxMultiples == multipleGroup [i]) {
				if (i < multipleGroup.Length - 1)
					finalReturn = multipleGroup [i + 1];
				else
					finalReturn = multipleGroup [i];
			}
		}
//		Debug.LogError ("GetNextUnlockMultiples finalReturn = " + finalReturn);
		return finalReturn;
	}

	public  int GetMultiplesUnlockIndex (int multiplesNum)
	{
		int index = -1;
		for (int i = 0; i < multipleGroup.Length; i++) {
			if (multipleGroup [i] == multiplesNum) {
				index = i;
				break;
			}
		}	
		return index;
	}

	bool MultipleIsLegal (int num) //检测炮倍数是否大于最大炮倍
	{
		if (num > gunControl.maxCannonMultiple)
			return false;
		else
			return true;
	}

	public void PlayGoldAcceptEffect ()
	{
		if (goldAcceptPS != null)
			goldAcceptPS.Play ();
	}

	public void SetUIShow (bool shouldShow)
	{
		switch (GameController._instance.myGameType) {
		case GameType.Classical:
			if (GameController._instance.isExperienceMode) {
				userInfoPanel.gameObject.SetActive (false);
				SetExperienceInfoShow (shouldShow);
			} else {
				userInfoPanel.gameObject.SetActive (shouldShow);
			}
			multiplePanel.gameObject.SetActive (shouldShow);
			userRankPanel.gameObject.SetActive (shouldShow);
			break;
		case GameType.Point:
			if (thisSeat == GunSeat.LT || thisSeat == GunSeat.RT) {
				transform.position = Vector3.up * 1000f;
			}
			userInfoPanel.gameObject.SetActive (false);
			multiplePanel.gameObject.SetActive (shouldShow);
			SetPkUserInfoShow (shouldShow);
			break;
		case GameType.Time:
			userInfoPanel.gameObject.SetActive (false);
			multiplePanel.gameObject.SetActive (shouldShow);
			SetPkUserInfoShow (shouldShow);
			break;
		case GameType.Bullet:
			userInfoPanel.gameObject.SetActive (false);
			multiplePanel.gameObject.SetActive (shouldShow);
			SetPkUserInfoShow (shouldShow);
			break;
		default:
			break;
		}
	}

	public void ShowBonusEffect (int goldNum, int enemyTypeId) //UI附近出现捕获奖金鱼的特效
	{
//		if (tempRedPacketEffect != null)
//			return;
//		if (tempBonusEffect != null) {
//			DestroyBounsUIEffect ();
//		}
//		if (tempUseSkillEffect != null) {
//			DestroyUseSkillEffect ();
//		}
//		tempBonusEffect = GameObject.Instantiate (PrefabManager._instance.bonusUIEffect, bonusEffectPos.position - Vector3.forward * 10f, Quaternion.identity)as GameObject;
//		tempBonusEffect.transform.parent = ScreenManager.uiScaler.transform;
//		tempBonusEffect.GetComponent<BonusUIEffect> ().SetInfo (goldNum, enemyTypeId);
//		//Destroy (bonusUIEffect, 5f); //在BonusUIEffect里已经有Destroy功能里，这里不需要再Destroy一遍
//		Invoke ("DestroyBounsUIEffect", 4.8f); 
		if (tempBonusEffect != null) {
			DestroyBounsUIEffect ();
		}
		if (tempUseSkillEffect != null) {
			DestroyUseSkillEffect ();
		}
		//tempBonusEffect= GameObject.Instantiate (PrefabManager._instance.bonusUIEffect,bonusEffectPos.position-Vector3.forward*10f,Quaternion.identity)as GameObject;
		//tempBonusEffect.transform.parent = ScreenManager.uiScaler.transform;

		tempBonusEffect = GameObject.Instantiate (PrefabManager._instance.bonusUIEffect, bonusEffectPos);
		tempBonusEffect.transform.localPosition = new Vector3 (0, 0, -10);
		tempBonusEffect.transform.localScale = Vector3.one * 45f;
		tempBonusEffect.GetComponent<BonusUIEffect> ().SetInfo (goldNum, enemyTypeId);

		//Destroy (bonusUIEffect, 5f); //在BonusUIEffect里已经有Destroy功能里，这里不需要再Destroy一遍
		Invoke ("DestroyBounsUIEffect", 4.8f);
	}

	/// <summary>
	/// 天将横财（跟奖金鱼特效是一样一样的）
	/// </summary>
	/// <param name="goldNum">Gold number.</param>
	/// <param name="enemyTypeId">Enemy type identifier.</param>
	public void ShowRedPackEffect (int goldNum) //UI附近出现捕获奖金鱼的特效
	{
		if (tempBonusEffect != null) {
			DestroyBounsUIEffect ();
		}
		if (tempUseSkillEffect != null) {
			DestroyUseSkillEffect ();
		}

		tempBonusEffect = GameObject.Instantiate (PrefabManager._instance.RedPackUIEffect_small, bonusEffectPos);
		tempBonusEffect.transform.localPosition = new Vector3 (0, 0, -10);
		tempBonusEffect.transform.localScale = Vector3.one * 45f;
		tempBonusEffect.GetComponent<BonusUIEffect> ().SetInfo1 (goldNum);

		//Destroy (bonusUIEffect, 5f); //在BonusUIEffect里已经有Destroy功能里，这里不需要再Destroy一遍
		Invoke ("DestroyBounsUIEffect", 4.8f);
	}


	void DestroyBounsUIEffect ()
	{
		Destroy (tempBonusEffect);
		tempBonusEffect = null;
	}

	public void ShowRedPacketEffect (float rmbNum)
	{
		Debug.LogError ("GunGetRedpacket,id=" + userID + " seat=" + thisSeat);
		if (tempBonusEffect != null) {
			DestroyBounsUIEffect ();
		}
		if (tempUseSkillEffect != null) {
			DestroyUseSkillEffect ();
		}
		tempRedPacketEffect = GameObject.Instantiate (PrefabManager._instance.redPacketEffect, bonusEffectPos.transform.position, Quaternion.identity)as GameObject;
	
		//tempRedPacketEffect.transform.position = new Vector3 (tempRedPacketEffect.transform.position.x,
		//tempRedPacketEffect.transform.position.y, 100f);
		string numShowStr = null;
		if (rmbNum - (int)rmbNum < 0.01f) { //说明是小数位为0的整数，但为了美观还是要显示小数点后一位∫
			numShowStr = rmbNum.ToString () + ".0";
		} else {
			numShowStr = rmbNum.ToString ();
		}
		tempRedPacketEffect.GetComponentInChildren<TextMesh> ().text = numShowStr;
		Invoke ("DestroyRedPacketEffect", 5.5f);
	}

	void DestroyRedPacketEffect ()
	{
		if (tempRedPacketEffect != null)
			Destroy (tempRedPacketEffect);
		tempRedPacketEffect = null;
	}

	public void ShowUseSkillEffect (SkillType type)
	{
		if (tempBonusEffect != null || tempRedPacketEffect != null) {
			return;
		}
		if (tempUseSkillEffect != null) {
			DestroyUseSkillEffect ();
			CancelInvoke ("DestroyUseSkillEffect"); 
		}
		tempUseSkillEffect = GameObject.Instantiate (PrefabManager._instance.useSkillEffectUI, bonusEffectPos.transform.position, Quaternion.identity) as GameObject;
		tempUseSkillEffect.transform.SetParent (ScreenManager.uiScaler.transform);
		tempUseSkillEffect.transform.localScale = Vector3.one;

		switch (type) {
		case SkillType.Berserk:
			tempUseSkillEffect.transform.Find ("SkillIcon").GetComponent<SpriteRenderer> ().sprite = PrefabManager._instance.skillEffectSpriteGroup [0];
			tempUseSkillEffect.transform.Find ("SkillIcon_Berserk").gameObject.SetActive (false);//2017.11.27  狂暴去除概率增加提示
			break;
		case SkillType.Summon:
			tempUseSkillEffect.transform.Find ("SkillIcon").GetComponent<SpriteRenderer> ().sprite = PrefabManager._instance.skillEffectSpriteGroup [1];
			tempUseSkillEffect.transform.Find ("SkillIcon_Berserk").gameObject.SetActive (false);
			break;
		case SkillType.Freeze:
			tempUseSkillEffect.transform.Find ("SkillIcon").GetComponent<SpriteRenderer> ().sprite = PrefabManager._instance.skillEffectSpriteGroup [2];
			tempUseSkillEffect.transform.Find ("SkillIcon_Berserk").gameObject.SetActive (false);
			break;
		case SkillType.Replication:
			tempUseSkillEffect.transform.Find ("SkillIcon").GetComponent<SpriteRenderer> ().sprite = PrefabManager._instance.skillEffectSpriteGroup [3];
			tempUseSkillEffect.transform.Find ("SkillIcon_Berserk").gameObject.SetActive (false);
			break;
		default:
			break;
		}

		Invoke ("DestroyUseSkillEffect", 4f);
	}

	void DestroyUseSkillEffect ()
	{
		if (tempUseSkillEffect != null)
			Destroy (tempUseSkillEffect);
		tempUseSkillEffect = null;
	}


	public void SetTextInfoShow (bool shouldShow) //控制自动开炮文字ui的显示
	{
		textInfo.gameObject.SetActive (shouldShow);
	}

	/// <summary>
	/// 设置PK场用户信息
	/// </summary>
	/// <param name="shouldShow">If set to <c>true</c> should show.</param>
	void SetPkUserInfoShow (bool shouldShow)
	{
		if (shouldShow) {
			pk_UserInfoPanel.GetComponent<RectTransform> ().localPosition = pkUserInfoPos.GetComponent<RectTransform> ().localPosition;
		} else {
			pk_UserInfoPanel.GetComponent<RectTransform> ().localPosition = Vector3.down * 1000f;
		}
	}

	/// <summary>
	/// 设置体验场用户信息
	/// </summary>
	/// <param name="isShow">If set to <c>true</c> is show.</param>
	void SetExperienceInfoShow (bool isShow)
	{
		if (isShow) {
			experienceInfo.GetComponent<RectTransform> ().localPosition = experienceInfoPos.GetComponent<RectTransform> ().localPosition;
		} else {
			experienceInfo.GetComponent<RectTransform> ().localPosition = Vector3.down * 1000f;
		}
	}

	public  void SetBulletNum (int num)
	{
		bulletNumText.text = num.ToString ();
	}

	bool gameIsEnd = false;

	public void SetScoreText (int num, bool shouldUpdateRank = true, bool gameEndFlag = false) //当gameEndFlag为true时，表示进行这次积分设置后，即便再打到鱼，也不会更改积分UI
	{
		if (gameIsEnd)
			return;
		scoreText.text = num.ToString ();
		switch (GameController._instance.myGameType) {
		case GameType.Classical:
			break;
		case GameType.Bullet:
			if (shouldUpdateRank)
				PkRanklistPanel._instance.UpdateRankShow ();
			break;
		case GameType.Point:
			if (shouldUpdateRank)
				PkPointProgessBar._instance.UpdateUserScore (thisSeat, num);
			break;
		case GameType.Time:
			if (shouldUpdateRank)
				PkRanklistPanel._instance.UpdateRankShow ();
			break;
		default:
			break;
		}
		gameIsEnd = gameEndFlag;
	}

	public string GetNickName ()
	{
		if (GameController._instance.isExperienceMode)
			return Tool.GetName (experienceNickName.text, 7);
		else
			return nickName.text.ToString ();
	}

	public int GetScore ()
	{
		return int.Parse (scoreText.text);
	}

	public void ChangeScore (int changeNum)
	{
		SetScoreText (GetScore () + changeNum, true);
	}

	public void SetDoubleEffectShow (float duration)
	{
		GameObject temp = GameObject.Instantiate (PrefabManager._instance.GetPrefabObj (MyPrefabType.SkillEffect, 7));
		temp.transform.position = doubleSkillPos.position;
		temp.transform.localScale = Vector3.one;
		Destroy (temp.gameObject, duration);
	}

	bool otherUserInfoIsShow = false;

	public void ShowChangeGunPanel ()
	{
		if (!gunControl.isActived)
			return;
		
        
		AudioManager._instance.PlayEffectClip (AudioManager.ui_click);
		if (isLocal) {
			//string path = "Window/BatteryUnlockWindow";
			//GameObject WindowClone = AppControl.OpenWindow (path);
			//WindowClone.SetActive (true);
			if (gunControl.replicationCount > 0) {
				HintTextPanel._instance.SetTextShow ("分身狀態下暫無法切換砲台樣式");
				return;
			}
			if (gunControl.isBerserk) {
				HintTextPanel._instance.SetTextShow ("狂暴狀態下無法切換砲台");
				return;
			}
			GameObject.Instantiate (gunControl.unlockGunPanel);
		} else {
			if (otherUserInfoIsShow) {
				OtherUserInfoBox._instance.Hide ();
				otherUserInfoIsShow = false;
			} else {
				OtherUserInfoBox._instance.Show (gunControl);
				otherUserInfoIsShow = true;
			}

		}
	}

	bool isShowTipsInLocalGun = false;

	void  ShowLocalGunTextPanel (string _str)
	{
		if (!gunControl.isActived) {
			return;
		}
		if (isLocal) {
			LocalGunTextPanel.Instance.ShowTipsInLocalGun (_str, gunControl);
		}
	}

	public void SetPkAvator (Sprite avatorSprite)
	{
		avatorImage.sprite = avatorSprite;
	}

	GameObject uiChatBox = null;

	public void ShowChatBubbleBox (string str, float duration = 3f, int voiceIndex = -1)
	{
		if (uiChatBox != null) {
			CancelInvoke ("DelayDestroyChatBox");
			uiChatBox.transform.Find ("Empty/Text").GetComponent<Text> ().text = str;
			Invoke ("DelayDestroyChatBox", duration);
			return;
		}

		if (voiceIndex >= 1 && voiceIndex <= 5) {
			AudioManager._instance.PlaySpeakClip (voiceIndex - 1);
		}

		GameObject temp = GameObject.Instantiate (gunControl.chatBoxPrefab, this.transform) as GameObject;
		//temp.transform.parent = this.transform;
		temp.transform.GetComponent<RectTransform> ().localScale = Vector3.one;
		Vector3 posAdjust;
		if (thisSeat == GunSeat.LB || thisSeat == GunSeat.RB) {
			posAdjust = new Vector3 (0, -40, 0);

		} else {
			posAdjust = new Vector3 (0, 40, 0);
			temp.transform.GetComponent<RectTransform> ().Rotate (Vector3.forward, 180f);
			temp.transform.Find ("Empty/Text").GetComponent<RectTransform> ().Rotate (Vector3.forward, 180f);
			temp.transform.Find ("GameObject").GetComponent<RectTransform> ().Rotate (Vector3.forward, 180f);
			temp.transform.Find ("GameObject").GetComponent<RectTransform> ().localPosition = Vector3.up * 8;
		}
		temp.GetComponent<RectTransform > ().localPosition = bonusEffectPos.GetComponent<RectTransform> ().localPosition + posAdjust;
		temp.transform.Find ("Empty/Text").GetComponent<Text> ().text = str;
		uiChatBox = temp;
		Invoke ("DelayDestroyChatBox", duration);
		//Destroy(temp,duration); 
	}

	void DelayDestroyChatBox ()
	{
		if (uiChatBox != null) {
			Destroy (uiChatBox.gameObject);
			uiChatBox = null;
		}
	}

	/// <summary>
	/// 捕鱼争霸和boss猎杀排名
	/// </summary>
	/// <param name="userId">User identifier.</param>
	/// <param name="rank">Rank.</param>
	/// <param name="isBossKill">If set to <c>true</c> is boss kill.</param>
	public void GetFishingUserRank (int userId, int rank, bool isBossKill = false)
	{
		if (isBossKill) {
			//userRankPanel.gameObject.SetActive(false);
			if (rank <= 0) {
				userRankPanel.GetComponent<Text> ().text = "未 上 榜";
			} else {
				userRankPanel.GetComponent<Text> ().text = "獵殺" + rank + "名";
			}

		} else {
			if (myInfo.userID == userId) {
				userRankPanel.gameObject.SetActive (false);
				if (rank <= 0) {
					UserRankObj.GetComponent<Text> ().text = "未上榜";
					UserRankObj.transform.parent.gameObject.SetActive (false);
				} else {
					UserRankObj.GetComponent<Text> ().text = rank.ToString () + "名";
					UserRankObj.transform.parent.gameObject.SetActive (true);
				}
			} else {
				if (rank <= 0) {
					userRankPanel.GetComponent<Text> ().text = "未 上 榜";
				} else {
					userRankPanel.GetComponent<Text> ().text = "捕魚爭霸" + rank + "名";
				}
			} 
		}
	}

	public void GetBigwinRank (int duanwei, int rank, bool Isdown)
	{
		if (Isdown) {
			UpOrdown [0].gameObject.SetActive (true);
			UpOrdown [1].gameObject.SetActive (false);
			Tween tween = null;
			UpOrdown [0].gameObject.transform.localPosition = new Vector3 (UpOrdown [0].gameObject.transform.localPosition.x, -156f, UpOrdown [0].gameObject.transform.localPosition.z);
			tween = UpOrdown [0].gameObject.transform.DOLocalMoveY (0f, 5f);
			tween.SetEase (Ease.Linear);
		} else {
			UpOrdown [1].gameObject.SetActive (true);
			UpOrdown [0].gameObject.SetActive (false);
			Tween tween = null;
			UpOrdown [1].gameObject.transform.localPosition = new Vector3 (UpOrdown [1].gameObject.transform.localPosition.x, 0f, UpOrdown [1].gameObject.transform.localPosition.z);
			tween = UpOrdown [1].gameObject.transform.DOLocalMoveY (-160f, 5f);
			tween.SetEase (Ease.Linear);
		}
		UserRankObj.transform.parent.gameObject.SetActive (false);
		BigwinRank.transform.gameObject.SetActive (true);
		BigwinRank.transform.Find ("Text").GetComponent<Text> ().text = duanwei + "段" + rank + "名";
		Invoke ("HindbigRank", 5f);
	}

	void HindbigRank ()
	{
		BigwinRank.transform.gameObject.SetActive (false);
		if (mRank > 0) {
			UserRankObj.transform.parent.gameObject.SetActive (true);
		}

	}
	//排名的显示和关闭
	void OpenAndCloseRank (bool NoOff)
	{
		if (!GameController._instance.isBossMatchMode) {  
			if (!isLocal) {
				userRankPanel.gameObject.SetActive (!NoOff);
				multipleText.gameObject.SetActive (NoOff);
				openRankMulitpleBool = !NoOff;
			}
		} else {
			userRankPanel.gameObject.SetActive (!NoOff);
			multipleText.gameObject.SetActive (NoOff);
			openRankMulitpleBool = !NoOff;
		}
		//boss猎杀判断
		
	}

	IEnumerator GetRefershBossKillRank ()
	{

		Facade.GetFacade ().message.fishCommom.SendGetFishingBossKill ();
		yield return new WaitForSeconds (10);

		StartCoroutine (GetRefershBossKillRank ());
	}

}
