using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using DG.Tweening;
using AssemblyCSharp;

//卡片信息
public class CardMsg
{
	public string cardTopText;

	public string CardTopText {
		get {
			return cardTopText;
		}
		set {
			cardTopText = value;
		}
	}

	public Sprite cardSprite;

	public Sprite CardSprite {
		get {
			return cardSprite;
		}
		set {
			cardSprite = value;
		}
	}

	public int cardBottomText;

	public int CardBottomText {
		get {
			return cardBottomText;
		}
		set {
			cardBottomText = value;
		}
	}

	public  CardMsg ()
	{

	}

	public  CardMsg (string _cardTopText, Sprite _cardSprite, int _cardBottomText)
	{
		this.cardTopText = _cardTopText;
		this.cardSprite = _cardSprite;
		this.cardBottomText = _cardBottomText;
	}
}

public class LotteryRandomScript : MonoBehaviour
{

	public static LotteryRandomScript Instance;
	public int activeCardId = -1;
	//字段
	Button Card1;
	Button Card2;
	Button Card3;
	Button Card4;
	Button Card5;
	Button Card6;
	public GameObject _Effect_Fanpai;
	public GameObject _Effect_Fanpai_Chixu;
	public GameObject _Effect_Chixu;
	public GameObject sureBtn;

	public Button[] Cards;

	public Sprite BGSprite;
	public Sprite miniSprite;
	public Sprite bronzeSprite;
	public Sprite silverSprite;
	public Sprite goldenSprite;
	public Sprite platinumSprite;
	public Sprite extremeSprite;
	public Sprite myGoldSprite;
	public Sprite myJewelSprite;
	public Button colseLotBtn;
	Image mask;

	public Animator UIControl;

	#region CardMsg字段


	CardMsg miniJewMsg1;
	CardMsg miniCardMsg;
	CardMsg miniJewMsg3;
	CardMsg miniJewMsg2;
	CardMsg miniGoldMsg2;
	CardMsg miniGoldMsg1;

	CardMsg bronzeCardMsg;
	CardMsg bronzeJewMsg1;
	CardMsg bronzeJewMsg2;
	CardMsg bronzeJewMsg3;
	CardMsg bronzeGoldMsg1;
	CardMsg bronzeGoldMsg2;

	CardMsg silverCardMsg;
	CardMsg silverJewMsg1;
	CardMsg silverJewMsg2;
	CardMsg silverJewMsg3;
	CardMsg silverGoldMsg1;
	CardMsg silverGoldMsg2;

	CardMsg goldenCardMsg;
	CardMsg goldenJewMsg1;
	CardMsg goldenJewMsg2;
	CardMsg goldenJewMsg3;
	CardMsg goldenGoldMsg1;
	CardMsg goldenGoldMsg2;

	CardMsg platinumCardMsg;
	CardMsg platinumJewMsg1;
	CardMsg platinumJewMsg2;
	CardMsg platinumJewMsg3;
	CardMsg platinumGoldMsg1;
	CardMsg platinumGoldMsg2;

	CardMsg extremeCardMsg;
	CardMsg extremeJewMsg1;
	CardMsg extremeJewMsg2;
	CardMsg extremeJewMsg3;
	CardMsg extremeGoldMsg1;
	CardMsg extremeGoldMsg2;


	#endregion

	AudioSource m_audio;

	Transform uiControl;

	List<CardMsg> cardList = new List<CardMsg> ();

	void Awake ()
	{
		Instance = this;

	}

	void Start ()
	{
		m_audio = transform.GetComponent <AudioSource> ();
		uiControl = transform.Find ("UIControll");
		Init ();
		mask = transform.Find ("UIControll/Mask").GetComponent <Image> ();
		CardMsgInit ();

        Invoke("DelaySet", 0.5f);
	}

    void DelaySet(){
        PrefabManager._instance.isLuckDrawUIShow = true;
    }

    private void OnDestroy()
    {
        PrefabManager._instance.isLuckDrawUIShow = false;
    }

    //初始化
    void Init ()
	{


		transform.GetComponent <Canvas> ().worldCamera = ScreenManager.uiCamera;

		Card1 = transform.Find ("UIControll/Card1").GetComponent <Button> ();
		Card2 = transform.Find ("UIControll/Card2").GetComponent <Button> ();
		Card3 = transform.Find ("UIControll/Card3").GetComponent <Button> ();
		Card4 = transform.Find ("UIControll/Card4").GetComponent <Button> ();
		Card5 = transform.Find ("UIControll/Card5").GetComponent <Button> ();
		Card6 = transform.Find ("UIControll/Card6").GetComponent <Button> ();

		Card1.onClick.RemoveAllListeners ();
		Card2.onClick.RemoveAllListeners ();
		Card3.onClick.RemoveAllListeners ();
		Card4.onClick.RemoveAllListeners ();
		Card5.onClick.RemoveAllListeners ();
		Card6.onClick.RemoveAllListeners ();

		//绑定事件
		Card1.onClick.AddListener (ClickLotteryCard1);
		Card2.onClick.AddListener (ClickLotteryCard2);
		Card3.onClick.AddListener (ClickLotteryCard3);
		Card4.onClick.AddListener (ClickLotteryCard4);
		Card5.onClick.AddListener (ClickLotteryCard5);
		Card6.onClick.AddListener (ClickLotteryCard6);



	}

	#region CardMsg赋值


	void CardMsgInit ()
	{

		miniCardMsg = new CardMsg("迷你魚雷", miniSprite, 1);
		miniJewMsg1 = new CardMsg("鑽石", myJewelSprite, 10);
		miniJewMsg2 = new CardMsg("鑽石", myJewelSprite, 5);
		miniJewMsg3 = new CardMsg("鑽石", myJewelSprite, 2);
		miniGoldMsg1 = new CardMsg("金幣", myGoldSprite, 100);
		miniGoldMsg2 = new CardMsg("金幣", myGoldSprite, 50);

		bronzeCardMsg = new CardMsg("青銅魚雷", bronzeSprite, 1);
		bronzeJewMsg1 = new CardMsg("鑽石", myJewelSprite, 20);
		bronzeJewMsg2 = new CardMsg("鑽石", myJewelSprite, 10);
		bronzeJewMsg3 = new CardMsg("鑽石", myJewelSprite, 4);
		bronzeGoldMsg1 = new CardMsg("金幣", myGoldSprite, 400);
		bronzeGoldMsg2 = new CardMsg("金幣", myGoldSprite, 200);

		silverCardMsg = new CardMsg("白銀魚雷", silverSprite, 1);
		silverJewMsg1 = new CardMsg("鑽石", myJewelSprite, 100);
		silverJewMsg2 = new CardMsg("鑽石", myJewelSprite, 50);
		silverJewMsg3 = new CardMsg("鑽石", myJewelSprite, 20);
		silverGoldMsg1 = new CardMsg("金幣", myGoldSprite, 2000);
		silverGoldMsg2 = new CardMsg("金幣", myGoldSprite, 1000);

		goldenCardMsg = new CardMsg("黃金魚雷", goldenSprite, 1);
		goldenJewMsg1 = new CardMsg("鑽石", myJewelSprite, 200);
		goldenJewMsg2 = new CardMsg("鑽石", myJewelSprite, 100);
		goldenJewMsg3 = new CardMsg("鑽石", myJewelSprite, 40);
		goldenGoldMsg1 = new CardMsg("金幣", myGoldSprite, 4000);
		goldenGoldMsg2 = new CardMsg("金幣", myGoldSprite, 2000);

		platinumCardMsg = new CardMsg("白金魚雷", platinumSprite, 1);
		platinumJewMsg1 = new CardMsg("鑽石", myJewelSprite, 400);
		platinumJewMsg2 = new CardMsg("鑽石", myJewelSprite, 200);
		platinumJewMsg3 = new CardMsg("鑽石", myJewelSprite, 80);
		platinumGoldMsg1 = new CardMsg("金幣", myGoldSprite, 8000);
		platinumGoldMsg2 = new CardMsg("金幣", myGoldSprite, 4000);

		extremeCardMsg = new CardMsg("至尊魚雷", extremeSprite, 1);
		extremeJewMsg1 = new CardMsg("鑽石", myJewelSprite, 800);
		extremeJewMsg2 = new CardMsg("鑽石", myJewelSprite, 400);
		extremeJewMsg3 = new CardMsg("鑽石", myJewelSprite, 160);
		extremeGoldMsg1 = new CardMsg("金幣", myGoldSprite, 16000);
		extremeGoldMsg2 = new CardMsg("金幣", myGoldSprite, 8000);

		cardList.Add (miniCardMsg);
		cardList.Add (miniJewMsg1);
		cardList.Add (miniJewMsg2);
		cardList.Add (miniJewMsg3);
		cardList.Add (miniGoldMsg1);
		cardList.Add (miniGoldMsg2);

		cardList.Add (bronzeCardMsg);
		cardList.Add (bronzeJewMsg1);
		cardList.Add (bronzeJewMsg2);
		cardList.Add (bronzeJewMsg3);
		cardList.Add (bronzeGoldMsg1);
		cardList.Add (bronzeGoldMsg2);

		cardList.Add (silverCardMsg);
		cardList.Add (silverJewMsg1);
		cardList.Add (silverJewMsg2);
		cardList.Add (silverJewMsg3);
		cardList.Add (silverGoldMsg1);
		cardList.Add (silverGoldMsg2);

		cardList.Add (goldenCardMsg);
		cardList.Add (goldenJewMsg1);
		cardList.Add (goldenJewMsg2);
		cardList.Add (goldenJewMsg3);
		cardList.Add (goldenGoldMsg1);
		cardList.Add (goldenGoldMsg2);

		cardList.Add (platinumCardMsg);
		cardList.Add (platinumJewMsg1);
		cardList.Add (platinumJewMsg2);
		cardList.Add (platinumJewMsg3);
		cardList.Add (platinumGoldMsg1);
		cardList.Add (platinumGoldMsg2);

		cardList.Add (extremeCardMsg);
		cardList.Add (extremeJewMsg1);
		cardList.Add (extremeJewMsg2);
		cardList.Add (extremeJewMsg3);
		cardList.Add (extremeGoldMsg1);
		cardList.Add (extremeGoldMsg2);


	}

	#endregion

	#region 点击事件

	//Card1
	void ClickLotteryCard1 ()
	{
		EveryCardMsg (0, Card1);
	}
	//Card2
	void ClickLotteryCard2 ()
	{
		EveryCardMsg (1, Card2);
	}
	//Card3
	void ClickLotteryCard3 ()
	{
		EveryCardMsg (2, Card3);
	}
	//Card4
	void ClickLotteryCard4 ()
	{
		EveryCardMsg (3, Card4);
	}
	//Card5
	void ClickLotteryCard5 ()
	{
		EveryCardMsg (4, Card5);
	}
	//Card6
	void ClickLotteryCard6 ()
	{
		EveryCardMsg (5, Card6);
	}

	IEnumerator Create_Effect_Fanpai ()
	{
		yield return new WaitForSeconds (0.1f);
		_Effect_Fanpai.SetActive (true);
		_Effect_Fanpai_Chixu.SetActive (true);
		_Effect_Chixu.SetActive (false);
		Destroy (_Effect_Fanpai, 3f);
	}

	void EveryCardMsg (int index, Button cards)
	{
		CardsMoveAndScale (index);
		_Effect_Fanpai.transform.position = new Vector3 (cards.transform.position.x, cards.transform.position.y, -10f);
		_Effect_Fanpai_Chixu.transform.position = new Vector2 (cards.transform.position.x, cards.transform.position.y);
	}

	#endregion


	#region 方法以及协程

	//卡牌的点击事件处理
	void CardsMoveAndScale (int cardId)
	{
		//请求信息
		Facade.GetFacade ().message.luckyDraw.SendLuckyDrawRequest (ToggleChangeScr.Instance.toggleId);
		activeCardId = cardId;
		return;
	}

	//1.5秒之后翻其他卡牌 ---1执行
	IEnumerator BeginRotateOtherCard (int cardId)
	{
		yield return new WaitForSeconds (0.85f);
		OtherCardsDisapear (cardId);
		Cards [cardId].GetComponent <Image> ().sprite = BGSprite;

	}

	//1.5秒之后翻当前卡牌 ---2执行
	IEnumerator RecoverClickCard (int cardId)
	{

		Cards [cardId].transform.DOLocalRotate (new Vector3 (0, 90f, 0), 0.5f);
		if (Cards [cardId].transform.localRotation == new Quaternion (0, 90f, 0, 0)) {
			WaitRecvResponse (cardId);
			_Effect_Fanpai.SetActive (true);
			_Effect_Fanpai_Chixu.SetActive (true);
			_Effect_Chixu.SetActive (false);
			Cards [cardId].transform.DOLocalRotate (new Vector3 (0, 90f, 0), 0.1f);
		}


		yield return new WaitForSeconds (1f);

		for (int i = 0; i < Cards.Length; i++) {
			Cards [i].transform.Find ("TopText1").GetComponent <Transform> ().localRotation = new Quaternion (0, 180f, 0, 0); 
			Cards [i].transform.Find ("CenterImage1").GetComponent <Transform> ().localRotation = new Quaternion (0, 180f, 0, 0); 
			Cards [i].transform.Find ("BottomText1").GetComponent <Transform> ().localRotation = new Quaternion (0, 180f, 0, 0); 
			Cards [i].transform.Find ("CardImage").gameObject.SetActive (true);
		}

	}

	//除了当前点击的卡片,其他的卡片延迟显示
	void OtherCardsDisapear (int cardId)
	{
		for (int i = 0; i < Cards.Length; i++) {
			if (i == cardId) {
				continue;
			}
			Cards [i].GetComponent <Image> ().sprite = BGSprite;
			Cards [i].GetComponent <Image> ().transform.DOLocalRotate (new Vector3 (0, 180f, 0), 0.25f);
			Cards [i].GetComponent <Image> ().transform.DOScale (new Vector2 (1f, 1f), 1.5f);
		}
	}
	//等待服务器数据
	public void  WaitRecvResponse (int cardId)
	{

		for (int i = 0; i < Cards.Length; i++) {
			if (i == cardId) {
				continue;
			}
			int ran;
			do {
				ran = Random.Range (0, Cards.Length);
			} while (ran == cardId);
			if (i == ran) {
				continue;
			} else {
				Vector3 pos = Cards [i].transform.position;
				Cards [i].transform.position = Cards [ran].transform.position;
				Cards [ran].transform.position = pos;
				//开启翻其他卡牌的协程
				StartCoroutine (BeginRotateOtherCard (i));
			}
		}

		//如果是当前的卡片,直接跳过,遍历下一张,并设置他的interactable为不可点击.
		for (int i = 0; i < Cards.Length; i++) {
			Cards [i].interactable = false;
		}


		//开启翻当前卡牌的协程
		StartCoroutine (RecoverClickCard (cardId));

		StartCoroutine (CardShowReward ());

		StartCoroutine (Create_Effect_Fanpai ());
	}
	//赋值数据
	void ValuationTextAndValue (CardMsg tormsg, CardMsg jewmsg1, CardMsg jewmsg2, CardMsg jewmsg3, CardMsg goldmsg1, CardMsg goldmsg2)
	{

		Cards [0].transform.Find ("TopText1").GetComponent <Text> ().text = tormsg.cardTopText;
		Cards [0].transform.Find ("CenterImage1").GetComponent <Image> ().sprite = tormsg.cardSprite;
		Cards [0].transform.Find ("BottomText1").GetComponent <Text> ().text = tormsg.cardBottomText.ToString ();
		Cards [0].transform.Find ("CenterImage1").GetComponent <Image> ().rectTransform.sizeDelta = new Vector2 (63f, 66f);

		Cards [1].transform.Find ("TopText1").GetComponent <Text> ().text = jewmsg1.cardTopText;
		Cards [1].transform.Find ("CenterImage1").GetComponent <Image> ().sprite = jewmsg1.cardSprite;
		Cards [1].transform.Find ("BottomText1").GetComponent <Text> ().text = jewmsg1.cardBottomText.ToString ();
		Cards [1].transform.Find ("CenterImage1").GetComponent <Image> ().rectTransform.sizeDelta = new Vector2 (74f, 46f);

		Cards [2].transform.Find ("TopText1").GetComponent <Text> ().text = jewmsg2.cardTopText;
		Cards [2].transform.Find ("CenterImage1").GetComponent <Image> ().sprite = jewmsg2.cardSprite;
		Cards [2].transform.Find ("BottomText1").GetComponent <Text> ().text = jewmsg2.cardBottomText.ToString ();
		Cards [2].transform.Find ("CenterImage1").GetComponent <Image> ().rectTransform.sizeDelta = new Vector2 (74f, 46f);

		Cards [3].transform.Find ("TopText1").GetComponent <Text> ().text = jewmsg3.cardTopText;
		Cards [3].transform.Find ("CenterImage1").GetComponent <Image> ().sprite = jewmsg3.cardSprite;
		Cards [3].transform.Find ("BottomText1").GetComponent <Text> ().text = jewmsg3.cardBottomText.ToString ();
		Cards [3].transform.Find ("CenterImage1").GetComponent <Image> ().rectTransform.sizeDelta = new Vector2 (74f, 46f);

		Cards [4].transform.Find ("TopText1").GetComponent <Text> ().text = goldmsg1.cardTopText;
		Cards [4].transform.Find ("CenterImage1").GetComponent <Image> ().sprite = goldmsg1.cardSprite;
		Cards [4].transform.Find ("BottomText1").GetComponent <Text> ().text = goldmsg1.cardBottomText.ToString ();
		Cards [4].transform.Find ("CenterImage1").GetComponent <Image> ().rectTransform.sizeDelta = new Vector2 (64f, 69f);

		Cards [5].transform.Find ("TopText1").GetComponent <Text> ().text = goldmsg2.cardTopText;				
		Cards [5].transform.Find ("CenterImage1").GetComponent <Image> ().sprite = goldmsg2.cardSprite;		
		Cards [5].transform.Find ("BottomText1").GetComponent <Text> ().text = goldmsg2.cardBottomText.ToString ();		
		Cards [5].transform.Find ("CenterImage1").GetComponent <Image> ().rectTransform.sizeDelta = new Vector2 (64f, 69f);

	}
	//---3执行
	IEnumerator  CardShowReward ()
	{
		yield return new WaitForSeconds (1f);

		if (LuckDrawCanvasScr.Instance.isCommonToggle == true) {
			ValuationTextAndValue (miniCardMsg, miniJewMsg1, miniJewMsg2, miniJewMsg3, miniGoldMsg1, miniGoldMsg2);
		}
		if (LuckDrawCanvasScr.Instance.isBronzeToggle == true) {
			ValuationTextAndValue (bronzeCardMsg, bronzeJewMsg1, bronzeJewMsg2, bronzeJewMsg3, bronzeGoldMsg1, bronzeGoldMsg2);
		}

		if (LuckDrawCanvasScr.Instance.isSilverToggle == true) {
			ValuationTextAndValue (silverCardMsg, silverJewMsg1, silverJewMsg2, silverJewMsg3, silverGoldMsg1, silverGoldMsg2);
		}
		if (LuckDrawCanvasScr.Instance.isGoldenToggle == true) {
			ValuationTextAndValue (goldenCardMsg, goldenJewMsg1, goldenJewMsg2, goldenJewMsg3, goldenGoldMsg1, goldenGoldMsg2);
		}
		if (LuckDrawCanvasScr.Instance.isPlatinumToggle == true) {
			ValuationTextAndValue (platinumCardMsg, platinumJewMsg1, platinumJewMsg2, platinumJewMsg3, platinumGoldMsg1, platinumGoldMsg2);
		}
		if (LuckDrawCanvasScr.Instance.isExtremeToggle == true) {
			ValuationTextAndValue (extremeCardMsg, extremeJewMsg1, extremeJewMsg2, extremeJewMsg3, extremeGoldMsg1, extremeGoldMsg2);
		}
	}



	#endregion


	#region 点击各个抽奖对应的奖励


	//抽奖反馈
	public void LotteryFeedback (int cardId, int num)
	{
		StartCoroutine (StartLotteryFeedback (cardId, num));
	}

	//开始赋值 ---4执行
	IEnumerator StartLotteryFeedback (int cardId, int num)
	{
		yield return  new WaitForSeconds (1f);

		for (int i = 0; i < Cards.Length; i++) {

			if (int.Parse (Cards [i].transform.Find ("BottomText1").GetComponent <Text> ().text) == num) {

				Vector3 pos = Cards [i].transform.position;

				Cards [i].transform.position = Cards [cardId].transform.position;

				Cards [cardId].transform.position = pos;

				cardId = i;
				Cards [cardId].transform.DOScale (new Vector2 (1.2f, 1.2f), 5f);
				Cards [cardId].transform.Find ("CardImage").gameObject.SetActive (false);
				break;

			}
		}
		m_audio.Play ();

		yield return new WaitForSeconds (1f);
		colseLotBtn.interactable = true;
		sureBtn.SetActive (true);


	}



	#endregion


	#region 关闭按钮


	IEnumerator ScaleLottery ()
	{
		yield return new WaitForSeconds (5f);
		_Effect_Fanpai_Chixu.SetActive (false);
		//uiControl.DOScale (new Vector2 (0, 0), 0.5f);
		mask.color = new Color (0, 0, 0, 0);
	}

	void CloseThisGameObj ()
	{
		UIControl.SetTrigger ("Close");
	}


	//手动关闭
	public void CloseLotteryRandom ()
	{
		UIControl.SetTrigger ("Close");
		_Effect_Fanpai_Chixu.SetActive (false);
		_Effect_Chixu.SetActive (false);
		mask.color = new Color (0, 0, 0, 0);
		AudioManager._instance.PlayEffectClip (AudioManager.effect_closePanel);
		Destroy (this.gameObject, 3f);
	}
	//自动关闭
	public void ThreeSecondsCloseLottery ()
	{
		StartCoroutine (ScaleLottery ());
		Invoke ("CloseThisGameObj", 5f);
		Destroy (this.gameObject, 6.4f);
	}

	#endregion
}
