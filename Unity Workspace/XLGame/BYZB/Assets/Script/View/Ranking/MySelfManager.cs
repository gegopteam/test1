using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using AssemblyCSharp;
/// <summary>
/// My self manager.排行榜中自身信息的显示
/// </summary>

public class MySelfManager : InfoManager {
	public Image outOfList;  //榜外
	public Image number;	//前三
	public Text moreNumber; //前100
	private MyInfo myInfo;
	public static MySelfManager instans;
	public GameObject Vip;
	public Image VipLevel;
	private Object[] num;
	// Use this for initialization
	private FiRankInfo mInfo=new FiRankInfo();
	void Awake(){
		instans = this;
		myInfo = DataControl.GetInstance ().GetMyInfo (); 
		userIcon = transform.Find ("UserIcon").GetComponent<Image> ();
		nickName = transform.Find ("MyName").GetComponent<Text> ();
		vipText = transform.Find ("MyLevel").Find ("Text").GetComponent<Text> ();
		sexImage = transform.Find ("Sex").GetComponent<Image> ();
		coinMuch = transform.Find ("TodayCoin").Find ("CoinMuch").GetComponent<Text> ();
		rewardMuch = transform.Find ("Reward").Find ("RewardMuch").GetComponent<Text> ();
		Init ();
		//个人面板弹出～
		//InitmInfo ();
		//userIcon.gameObject.GetComponent<Button> ().onClick.AddListener (() => HeadClick (mInfo));
	}
	void Start () {
	}
	void InitmInfo(){
		mInfo.userId = (long)myInfo.userID;
		mInfo.gold = myInfo.gold;
		mInfo.vipLevel = myInfo.levelVip;
		mInfo.nickname = myInfo.nickname;
		mInfo.avatarUrl = myInfo.avatar;
		mInfo.gender = myInfo.sex;
	}
	void Init()
	{
		nickName.text = Tool.GetName (myInfo.nickname, 6);
		vipText.text = myInfo.level.ToString ();
		//如果是0就是女，如果是1就是男
		if (myInfo.sex == 1) {
			sexImage.overrideSprite = UIHallTexturers.instans.Ranking[7];
		} else {
			sexImage.overrideSprite =UIHallTexturers.instans.Ranking[2];
		}
		coinMuch.text = myInfo.loginInfo.loginGold.ToString ();

		AvatarInfo nAVInfo = (AvatarInfo) Facade.GetFacade ().data.Get ( FacadeConfig.AVARTAR_MODULE_ID );
		nAVInfo.Load ( myInfo.userID , myInfo.avatar , ( (int nResult, Texture2D nTexture) => {
			if (nResult == 0) {
				if (userIcon != null) {
					userIcon.sprite = Sprite.Create (nTexture, new Rect (0, 0, nTexture.width, nTexture.height), new Vector2 (0, 0));
				}
			}
		} ) );


		if (myInfo.levelVip == 0) {
			Vip.SetActive (false);
		} else {
			Vip.SetActive (true);
			VipLevel.sprite = UIHallTexturers.instans.RankNum [myInfo.levelVip];//(Sprite)num [myInfo.levelVip+1];
		}
	}
	public void ChageImage(){
		for (int i = 0; i < UIRanking.nRankData.Count; i++) {
			if (UIRanking.nRankData [i].userId == myInfo.userID) {
				if (i < 3) {
					outOfList.gameObject.SetActive (false);
					number.gameObject.SetActive (true);
					moreNumber.gameObject.SetActive (false);
					number.sprite = UIHallTexturers.instans.Ranking[i+3];//Resources.Load<Sprite> ("Ranking/排名" + (i + 1));
					if(i == 0)
						rewardMuch.text = "200";
					else if(i == 1)
						rewardMuch.text = "100";
					else
						rewardMuch.text = "50";
				} else {
					outOfList.gameObject.SetActive (false);
					number.gameObject.SetActive (false);
					moreNumber.gameObject.SetActive (true);
					moreNumber.text = (i + 1).ToString ();
					if(i <10)
						rewardMuch.text = "30";
					else if(i <30)
						rewardMuch.text = "20";
					else
						rewardMuch.text = "10";

				}
				return;
			}
		
		}
		rewardMuch.text = "0";
		outOfList.gameObject.SetActive (true);
		number.gameObject.SetActive (false);
		moreNumber.gameObject.SetActive (false);

	}
	void HeadClick(FiRankInfo info){
		AudioManager._instance.PlayEffectClip (AudioManager.ui_click);
		string path = "Window/FriendInfo";
		GameObject WindowClone = AppControl.OpenWindow (path);
		WindowClone.SetActive (true);
		WindowClone.GetComponent<RankFriend> ().SetInfo (info);
	}
}
