using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using AssemblyCSharp;
using DG.Tweening;
/// <summary>
/// User interface ranking.排行榜整体管理
/// </summary>
public class UIRanking : MonoBehaviour , IUiMediator {
	//每日爆金版 0
	public const int COIN = 0;
	//土豪版 1
	public const int RICH = 1;
	//public static UIRanking instanse;
	private AppControl appControl = null;
	private DataControl dataControl = null;
	private RankInfo rankInfo = null;
	private MySelfManager mySelf;
	private RankingManager [] otherRank;

	public static List<FiRankInfo> nRankData;
	public BackageUI richMan;
	public BackageUI todayCoin;
	private InfinityGridLayoutGroup infinityGridLayoutGroup;
	//public ResetScroll resetScroll;
	public Image whichList;
	public Image dayRich;
	public Image dayCoin;
	public bool canClick=true;
	float limitTime;
	CanvasGroup coinGroup;
	CanvasGroup richGroup;
	RankInfo nInfo;
	public int clicktime=0;

	private GameObject rank;
	public Image titlerichclick;
	public Image titlecoinclick;

    [SerializeField]
    private Camera rankCamera;

    private Canvas mainCanvas;
	void Awake()
	{
		//instanse = this;
		Debug.LogError ("awake");
		appControl = AppControl.GetInstance ();
		Facade.GetFacade ().ui.Add ( FacadeConfig.UI_RANK_MODULE_ID ,this );

	}

	void Start()
	{
        rank = GameObject.FindGameObjectWithTag("MainCamera");
        Debug.Log(rank.name);
        rankCamera = rank.GetComponent<Camera>();
        mainCanvas = transform.GetComponentInChildren<Canvas>();
        mainCanvas.worldCamera = rankCamera;

		Debug.LogError ("start");
		//DontDestroyOnLoad (this.gameObject);
//		RankInfo.InitRankEvent += InitOtherRank;
		dayRich.sprite = UIHallTexturers.instans.Ranking[14];
		dayCoin.sprite = UIHallTexturers.instans.Ranking[13];
		dataControl = DataControl.GetInstance ();
		rankInfo = dataControl.GetRankInfo ();
		richMan.gameObject.SetActive (true);
		todayCoin.gameObject.SetActive (false);
		//mySelf = GetComponentInChildren<MySelfManager> ();
		coinGroup = todayCoin.GetComponent<CanvasGroup> ();
		richGroup = richMan.GetComponent<CanvasGroup> ();

		UIColseManage.instance.ShowUI (this.gameObject);
		nInfo = (RankInfo) Facade.GetFacade ().data.Get ( FacadeConfig.RANK_MODULE_ID );

		//默认界面在土豪版，如果没有土豪版数据，发送请求，消息层收到数据后，会调用OnRecvData 接口

		//if (nInfo.GetCoinArray ().Count == 0) {
		//	Facade.GetFacade ().message.rank.SendGetRankInfoRequest (RICH);
		//} else { //已经存在土豪版数据了，初始化UI组件
		//OnRecvData( COIN , nInfo.GetRichArray () );
		Invoke ("RichMan", 0.1f);
		//}

		//resetScroll = GetComponentInChildren<ResetScroll> ();
		//resetScroll.SetAmount ();
	}
	void OnEnable(){
		nInfo = (RankInfo) Facade.GetFacade ().data.Get ( FacadeConfig.RANK_MODULE_ID );
		coinGroup = todayCoin.GetComponent<CanvasGroup> ();
		richGroup = richMan.GetComponent<CanvasGroup> ();
		//RichMan ();
	}

	//发送获取排行榜消息后，推送进来的数据
	public void OnRecvData( int nType , object nData )
	{
		Debug.LogError ( "-------------recv------------------ type : " + nType );
		//nRankData = (List<FiRankInfo>)nData;
		if ( nType == COIN ) {
			nRankData = nInfo.GetCoinArray ();
			print ("coin的长度是"+nRankData.Count);
			todayCoin.cellNumber = nRankData.Count;
			todayCoin.Refresh ();
			richGroup.alpha = 0;
			coinGroup.DOFade (1, 1);

		} else if ( nType == RICH ) {
			nRankData = nInfo.GetRichArray ();
			print ("rich的长度是"+nRankData.Count);
			richMan.cellNumber = nRankData.Count;
			richMan.Refresh ();
			coinGroup.alpha = 0;
			richGroup.DOFade (1, 1);

		}
		//MySelfManager.instans.ChageImage ();
	}

	public void OnInit()
	{
		Facade.GetFacade().message.rank.SendGetRankInfoRequest( RICH );
		Facade.GetFacade().message.rank.SendGetRankInfoRequest( COIN );
	}

	public void OnRelease()
	{
		
	}

    void InitOtherRank()
	{
		//重新获取当前的排行对象
		otherRank = GetComponentsInChildren<RankingManager> ();

		//获取GoodList中RankingList的数据，并显示出来，排行榜每30分钟更新一次
		Debug.Log ("UIRanking排行榜人数的长度"+Goodlist.GetInstance().RankingList.Count);
		for (int i = 0; i < Goodlist.GetInstance().RankingList.Count; i++) {
			//更换图片

			//目前，客户端只支持10个下拉列表选项
			if (i >= 10)
				return;

			string nAvatarUrl = Goodlist.GetInstance ().RankingList [i].avatarUrl;
		//	Debug.LogError ( "nAvatarUrl ==== >" + nAvatarUrl );

			if (nAvatarUrl != null && !nAvatarUrl.Equals ("") && nAvatarUrl.Length > 0) {
				StartCoroutine (ChangeImage (otherRank [i], nAvatarUrl));
			}
		
			otherRank [i].nickName.text = Goodlist.GetInstance().RankingList [i].nickname;
			otherRank [i].vipText.text = Goodlist.GetInstance().RankingList [i].vipLevel.ToString();
			switch (Goodlist.GetInstance().RankingList [i].gender) 
			{
			case 2://女
				
				break;
			case 1:
				otherRank[i].sexImage.sprite = UIHallTexturers.instans.Ranking[7];
				break;
                default:
				otherRank[i].sexImage.sprite = UIHallTexturers.instans.Ranking[2];
                    break;
				
			}
			otherRank[i].coinMuch.text = Goodlist.GetInstance().RankingList[i].gold.ToString();
			otherRank [i].rewardMuch.text = "0";
		}
		for (int index = Goodlist.GetInstance().RankingList.Count; index < otherRank.Length; index++) {
		
			otherRank [index].gameObject.SetActive (false);
		}
	}

	IEnumerator ChangeImage(RankingManager rankImage,string path)
	{
		WWW www = new WWW (path);
		yield return www;

		if (www.error == null) {
			Texture2D image = www.texture;
			rankImage.userIcon.sprite = Sprite.Create (image, new Rect (0, 0, image.width, image.height), new Vector2 (0, 0));
		}
	}

	// Update is called once per frame
	void Update () {
		limitTime +=Time.deltaTime;
		if (limitTime > 2) {
			canClick = true;
			limitTime = 0;
		}
		if (clicktime > 10) {
			clicktime = 0;
			Resources.UnloadUnusedAssets ();
		}

	}

	public void OnButton()
	{
		AudioManager._instance.PlayEffectClip (AudioManager.effect_closePanel);
		Destroy (this.gameObject);
		UIColseManage.instance.CloseUI ();
		//transform.gameObject.SetActive (false);
	}

	public void RichMan()
	{
		AudioManager._instance.PlayEffectClip (AudioManager.ui_click);
		richMan.gameObject.SetActive (true);
		todayCoin.gameObject.SetActive (false);
		if (UIHallTexturers.instans) {
			dayRich.sprite = UIHallTexturers.instans.Ranking [14];
			dayCoin.sprite = UIHallTexturers.instans.Ranking [13];
			whichList.sprite = UIHallTexturers.instans.Ranking [8];
			titlerichclick.gameObject.SetActive (true);
			titlecoinclick.gameObject.SetActive (false);
		}
		//Facade.GetFacade().message.rank.SendGetRankInfoRequest( RICH );
		OnRecvData(RICH,null);
	}

	public void TodayCoin()
	{
		AudioManager._instance.PlayEffectClip (AudioManager.ui_click);
		richMan.gameObject.SetActive (false);
		todayCoin.gameObject.SetActive (true);
		dayRich.sprite = UIHallTexturers.instans.Ranking[15];
		dayCoin.sprite = UIHallTexturers.instans.Ranking[12];
		//resetScroll = GetComponentInChildren<ResetScroll> ();
		//rankInfo.SendGetRankInfoRequest(0);
		whichList.sprite=UIHallTexturers.instans.Ranking[6];
		//Facade.GetFacade().message.rank.SendGetRankInfoRequest( COIN );
		OnRecvData(COIN,null);
		titlecoinclick.gameObject.SetActive (true);
		titlerichclick.gameObject.SetActive (false);
	}

	void OnDestroy(){
//		RankInfo.InitRankEvent -= InitOtherRank;
		Facade.GetFacade ().ui.Remove( FacadeConfig.UI_RANK_MODULE_ID );
	}

    public void OnBtnClose()
    {
        Destroy(this.gameObject);
    }
}
