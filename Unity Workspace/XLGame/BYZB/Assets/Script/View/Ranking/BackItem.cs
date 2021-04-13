using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using AssemblyCSharp;
using System.Text;
using System;

public class BackItem : ScrollableCell {

    private Text text;//cell上显示的文本
	public Image userIcon;
	public Text nickName;
	public GameObject effect;
	public Image sexImage;
	public Image coinImage;
	public Text coinMuch;
	public Image rewardImage;
	public Text rewardMuch;
	public Image numberImage;
	private FiRankInfo data;
	public  Text numberText;
	public  Text levetext;

	public GameObject Vip;
	public Image VipLevel;
	private UnityEngine.Object[] num;
	Sprite tempHead;
	AvatarInfo nAvaInfo ;
	UIRanking rankManager;
    void Awake()
	{
        //初始化
        //text = transform.FindChild("Text").GetComponent<Text>();
		numberImage=transform.Find("Number").GetComponent<Image>();
		userIcon = transform.Find ("UserIcon_mask").GetChild(0).GetComponent<Image> ();
		nickName = transform.Find ("MyName").GetComponent<Text> ();
		//vipText = transform.FindChild ("MyLevel").FindChild ("Text").GetComponent<Text> ();
		//sexImage = transform.FindChild ("Sex").GetComponent<Image> ();
		coinMuch = transform.Find ("CoinMuch").GetComponent<Text> ();
		//rewardMuch = transform.FindChild ("Reward").Find ("RewardMuch").GetComponent<Text> ();
		numberText = transform.Find ("Text").GetComponent<Text> ();
		tempHead = userIcon.sprite;

    }
	void Start(){
		rankManager = (UIRanking)Facade.GetFacade ().ui.Get (FacadeConfig.UI_RANK_MODULE_ID);
	}
	IEnumerator ChangeImage(Image headImage,string path)
	{
		WWW www = new WWW (path);
		yield return www;

		if (www.error == null) {
			Texture2D image = www.texture;
			headImage.sprite = Sprite.Create (image, new Rect (0, 0, image.width, image.height), new Vector2 (0, 0));
		}else
			headImage.sprite =tempHead;
	}
    public override void ConfigureCellData()
    {
		try{
		userIcon.gameObject.GetComponent<Button> ().onClick.RemoveAllListeners();
        base.ConfigureCellData();
		if (dataObject != null)
			data= UIRanking.nRankData[(int)dataObject];
		if (data != null && this.gameObject.activeInHierarchy) {
			if ((int)dataObject < 3) {
				numberImage.gameObject.SetActive (true);
				numberImage.sprite = UIHallTexturers.instans.Ranking[(int)dataObject+3];//Resources.Load<Sprite> ("Ranking/排名" + ((int)dataObject + 1));
				numberText.text = "";
				if((int)dataObject == 0)
					rewardMuch.text = "200";
				else if((int)dataObject == 1)
					rewardMuch.text = "100";
				else
					rewardMuch.text = "50";
			} else {
				numberImage.gameObject.SetActive (false);
				numberText.text = ((int)dataObject + 1).ToString ();
				if((int)dataObject <10)
					rewardMuch.text = "30";
				else if((int)dataObject <30)
					rewardMuch.text = "20";
				else
					rewardMuch.text = "10";
			}
				
			nickName.text =Tool.GetName( data.nickname,10);


			//nickName.text = data.nickname;
			levetext.text = data.level.ToString();
			switch (data.gender) 
			{
			case 1:
				sexImage.sprite = UIHallTexturers.instans.Ranking[7];
				break;
			default:
				sexImage.sprite =UIHallTexturers.instans.Ranking[2];
				break;
			}
			//Debug.LogError((int)dataObject+":"+data.gold);
			coinMuch.text = Tool.GetRankCoinNum( data.gold);
			//StartCoroutine (ChangeImage (userIcon, data.avatarUrl));
			nAvaInfo = ( AvatarInfo )Facade.GetFacade ().data.Get ( FacadeConfig.AVARTAR_MODULE_ID );
			nAvaInfo.Load ((int)data.userId, data.avatarUrl, (int nResult, Texture2D nTexture) => {
				if(userIcon!=null){
				if (nResult == 0) {
					//Debug.LogError (UserAvatar +  "-----22-----" + currentUserId + "/"+nTexture + "/" + UserIcon  + "-----" + UserIcon.GetComponentInParent<Image>() );
					userIcon.sprite = Sprite.Create (nTexture, new Rect (0, 0, nTexture.width, nTexture.height), new Vector2 (0, 0));
				}else
					userIcon.sprite =tempHead;
				}});


			userIcon.gameObject.GetComponent<Button> ().onClick.AddListener (() => HeadClick (data));
			if (data.vipLevel == 0) {
				//Vip.SetActive (true);
				Vip.GetComponent<Image> ().sprite = UIHallTexturers.instans.VipFrame [0];
				effect.SetActive (false);
			} else {
				//Vip.SetActive (true);
				Vip.GetComponent<Image> ().sprite = UIHallTexturers.instans.VipFrame [data.vipLevel];
				if (data.vipLevel == 9) 
					effect.SetActive (true);
				else
					effect.SetActive (false);
			//	VipLevel.sprite = UIHallTexturers.instans.RankNum [data.vipLevel];
			}
		}
		}catch(Exception e){
			Debug.LogError (e);
		}
            //text.text = ((int)dataObject).ToString();
    }
	void HeadClick(FiRankInfo info){
		if (rankManager.canClick == false) {
			string paths = "Window/WindowTipsThree";
			GameObject WindowClones = AppControl.OpenWindow (paths);
			WindowClones.SetActive (true);
			UITipAutoNoMask ClickTips1 = WindowClones.GetComponent<UITipAutoNoMask> ();
			ClickTips1.tipText.text = "操作過快，請稍後再試";
			return;
		}
		rankManager.canClick = false;
		rankManager.clicktime += 1;
		AudioManager._instance.PlayEffectClip (AudioManager.ui_click);
		string path = "Window/FriendInfo";
		GameObject WindowClone = AppControl.OpenWindow (path);
		UIColseManage.instance.ShowUI (WindowClone);
		WindowClone.SetActive (true);
		WindowClone.GetComponent<RankFriend> ().SetInfo (info);
	}
	string GetCoinMuch(long gold){
		StringBuilder strBlider = new  StringBuilder ();
		string temp;
		bool isOver=false;
		while (gold % 1000 != 0||gold!=0) {
			temp = (gold % 1000).ToString ();
			if (!isOver) {
				switch (temp.Length) {
				case 1:
					strBlider.Insert (0, "00" + temp); 
					break;
				case 2:
					strBlider.Insert (0, "0" + temp); 
					break;
				default:
					strBlider.Insert (0, temp); 
					break;
				}
			} else 
				strBlider.Insert (0, temp);
			gold = gold / 1000;
			if (gold != 0) {
				strBlider.Insert (0, ",");
				if (gold / 1000 == 0)
					isOver = true;
			}
		}
		if(strBlider.Length==0)
			return "0";
		return strBlider.ToString();
	}
}
/*		public long   userId;
		public long   gold;
		public int    vipLevel;
		public string nickname;
		public string avatarUrl;
		public int    gender;
		public int    rewards;*/