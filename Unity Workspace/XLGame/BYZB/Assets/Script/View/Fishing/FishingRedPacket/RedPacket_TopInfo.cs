using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using DG.Tweening;

public class RedPacket_TopInfo : MonoBehaviour {

	public static RedPacket_TopInfo _instance;

	public static int lastGoldCostRest=0;

	public Text countDownText;
	public Image[] progressBarGroup;
	//public Image progressBar2;
	public Text currentCostText;
	public Text targetCostText;
	public Text bulletNumText;

	int currentCostNum=0;
	public  int targetCostNum=10000;

	float percent;

	public GameObject redPacketRainPrefab;
	GameObject redPacketRainObject=null;

	public GameObject effectFullProgress;

 	public  ParticleSystem  ps_ShineRedPackage;

	GunControl localGun;
	int currentMultiple;

	bool progressBarIsFull=false;

	[HideInInspector]public int sumGoldIndex; //0代表未达成任一阶段，1,2,3分别代表完成1，2，3阶段
	public int[] sumGoldGroup = new int[3]{ 25000,50000,100000};

	public GameObject goldHintPanel;
	public Sprite[] goldHintSpriteGroup;

	public ParticleSystem ps_goldHintEffect;

	public GameObject redpacketRulePanel;

	public GameObject[] indicatorGroup;

	Vector3 indicatorOrginalPos;
	float progressLength;

	bool indicatorIsShow=false;

	void Awake(){
		if (null == _instance)
			_instance = this;
		sumGoldIndex = 0;
		indicatorOrginalPos = indicatorGroup [1].transform.position;
		progressLength = indicatorGroup [2].transform.position.x - indicatorGroup [1].transform.position.x;
		SetIndicatorShow (false);

	}

	// Use this for initialization
	void Start () {
		if (!GameController._instance.isRedPacketMode) {
			this.gameObject.SetActive (false);
		} else {
			targetCostText.text = targetCostNum.ToString ();

			currentCostText.text = currentCostNum.ToString ();
			Invoke ("DelayInit", 0.4f);
		}
	}

	void DelayInit(){
		
		localGun = PrefabManager._instance.GetLocalGun ();
		currentMultiple = localGun.cannonMultiple;
		AddGoldCost (lastGoldCostRest);
	}

	public void AddGoldCost(int addNum){
		
		currentCostNum += addNum;
		currentCostText.text = currentCostNum.ToString ();
		if (indicatorIsShow) {
			SetIndicatorShow (false);
		}
		UpdateProgressBar ();
		if (tempHangUpHint != null) {
			Destroy (tempHangUpHint.gameObject);
			tempHangUpHint = null;
		}
	}

	public void UpdateGunMultiple(){
	
	}
	public void UpdateGunMultiple(int newMultiple){
		currentMultiple = newMultiple;
		UpdateProgressBar ();
	}

	void UpdateProgressBar(){

		//if (progressBarIsFull) {
		//	return;
		//}

		targetCostNum = sumGoldGroup [sumGoldIndex];
		percent = (float)currentCostNum / (float)targetCostNum;

		if (percent >= 1) {
			for (int i = 0; i < sumGoldIndex+1; i++) { 
				progressBarGroup [i].fillAmount = 1;
			}

			if (sumGoldIndex >= 2) {//如果index=2，说明已经到达最大限度
				
				percent = 1;
				if (sumGoldIndex == 2) {
					bulletNumText.text = "0";
					bulletNumText.transform.parent.gameObject.SetActive (false);
					if (!progressBarIsFull) { //只触发一次
						Invoke ("ShowHintPanel1", 2f); //您已达到最高阶段
						ShowHintIndex(3);
					}
						
				}
				progressBarIsFull =true;
					
			} else {
		
				Invoke ("ShowHintPanel2", 2f);
				//HintTextPanel._instance.SetTextShow ("继续捕鱼您将获得更高金额的红包！",4f);
				currentCostNum -= targetCostNum; //计算溢出的金币，放到下一级进度条里计算

				targetCostNum = sumGoldGroup [sumGoldIndex ];

				percent =(float)currentCostNum / (float)targetCostNum;
			}



			sumGoldIndex++;
			if(!progressBarIsFull)
				ShowHintIndex (sumGoldIndex);
		
				
			//if (!progressBarIsFull) {//只有第一次触发进度条满时，才播放相关粒子特效，直到下次重置
			if(sumGoldIndex!=3||!progressBarIsFull){
				effectFullProgress.SetActive (true);
				ps_ShineRedPackage.Play ();//每当突破一次层级，触发一次粒子特效
			}
			if (sumGoldIndex == 3) {
				sumGoldIndex = 2;
			}	
		//	}
			//progressBarIsFull = true;
		}

		if (sumGoldIndex < 3) {
			progressBarGroup [sumGoldIndex].fillAmount = percent;
		} else {
			progressBarGroup [2].fillAmount = percent;
		}
			
		int restGold = targetCostNum - currentCostNum; //restGold一定大于0

		int bulletNum=-1;
		//currentMultiple = localGun.cannonMultiple;
		float temp = (float)restGold / (float)currentMultiple;
		if (temp <= 1) {
			bulletNum = 1;
		}
		else if (temp > 1) {
			if ((temp - (int)temp) > 0) {
				bulletNum = (int)temp + 1;
			} else {
				bulletNum = (int)temp;
			}
		} 
		if(!progressBarIsFull)
			bulletNumText.text = bulletNum.ToString (); 
	}

	void ShowHintPanel1()
	{
		HintTextPanel._instance.SetTextShow("您已達最高階段，最高可獲4.8元紅包券！", 3f);
	}
	void ShowHintPanel2()
	{
		HintTextPanel._instance.SetTextShow("繼續捕魚您將獲得更高金額的紅包！", 3f);
	}

	public void SetTime(int seconds)
	{
		//Debug.LogError ("CountDown:" + seconds);
		if (this.gameObject != null) {
			countDownText.text = TopRoomInfo.ChangeTimeFormat (seconds);
		} else {
			Debug.LogWarning ("CountDownValue=null");
		}
	}

	public void ShowRedPacketRain()
	{
	//	redPacketRainObject  = GameObject.Instantiate (redPacketRainPrefab,Vector3.zero,Quaternion.identity)as GameObject ;
		GameObject temp  = GameObject.Instantiate (redPacketRainPrefab,Vector3.zero,Quaternion.identity)as GameObject ;
		Destroy (temp, 3f);
	}
	public void DestroyRedPacketRain(){
		if (redPacketRainObject != null) {
			Destroy (redPacketRainObject);
			redPacketRainObject = null;
		}
	}

	public void ResetProgress(){
		for (int i = 0; i < progressBarGroup.Length; i++) {
			progressBarGroup[i].fillAmount = 0;
		}

		bulletNumText.transform.parent.gameObject.SetActive (true);
		sumGoldIndex = 0;
		currentCostNum = 0;
		currentCostText.text = "0";
		effectFullProgress.SetActive (false);
		progressBarIsFull = false;
		UpdateProgressBar ();
	}

	public GameObject hangUpHintPrefab;
	GameObject tempHangUpHint=null;

	public void DecreaseProgress(int reduceNum){
		//Debug.LogError ("goldIndex=" + sumGoldIndex);
	//	HintTextPanel._instance.SetTextShow ("长时间未发射子弹，红包进度衰减中！",1.1f,false);
		if(tempHangUpHint==null){
			tempHangUpHint = GameObject.Instantiate (hangUpHintPrefab);
			tempHangUpHint.transform.parent= ScreenManager.uiScaler.gameObject.transform;
			tempHangUpHint.GetComponent<RectTransform> ().localPosition = Vector3.zero;
			tempHangUpHint.GetComponent<RectTransform> ().localScale= Vector3.one;
		}

		if (progressBarIsFull) {
			if (currentCostNum + reduceNum  < sumGoldGroup[2]) { //说明从全满状态回退
				progressBarIsFull =false;
				bulletNumText.transform.parent.gameObject.SetActive (true);
				sumGoldIndex = 2;
			}
		}
		if (currentCostNum + reduceNum >= 0) { //如果减少金币后还能保留在当前阶段
			currentCostNum+=reduceNum;
		} else { //如果减少金币后要退到下一个阶段或者变为0
			if (sumGoldIndex > 0) {
				
				sumGoldIndex--;
				currentCostNum= sumGoldGroup[sumGoldIndex]+ (currentCostNum +reduceNum);
				progressBarGroup [sumGoldIndex ].fillAmount = 0; //重置当前阶段的进度条显示
			} else {
				currentCostNum = 0;
			}	
		}
		UpdateProgressBar ();
		SetIndicatorShow (true);
	}


	public GameObject redPacketForMovePrefab;
	GameObject redPacketForMoveTemp;

	int tempRedpacketId; //临时id，移动后会存储在左上角
	int tempIndex;
	public void ShowMovedRedPacket(int tempId){ //生成一个红包移动向左上角
		ShowRedPacketRain();
		redPacketForMoveTemp = GameObject.Instantiate (redPacketForMovePrefab, Vector3.zero, Quaternion.identity) as GameObject ;
		tempRedpacketId = tempId;
		tempIndex= sumGoldIndex;//1,2,3 三个等级
		Invoke ("DelayMove", 1f);
	}
	void DelayMove(){
		//redPacketForMoveTemp.transform.DOMove (redPacketIcon_LT.transform.position, 1f);
		Invoke ("DelayHide", 3f);
	}
	void DelayHide(){ //移动完毕后，改变左上角样式
		Destroy (redPacketForMoveTemp);
		redPacketForMoveTemp = null;
		LT_GetRedPacket._instance.AddOneRedpacket (tempRedpacketId,tempIndex);
	}

	public void ShowHintIndex(int level){
		goldHintPanel.SetActive (true);
		goldHintPanel.transform.Find ("Stage").GetComponent<Image> ().sprite = goldHintSpriteGroup [level - 1];
		ps_goldHintEffect.Play ();
		Invoke ("HideHintIndex", 2f);
	}
	void HideHintIndex(){
		goldHintPanel.SetActive (false);
	}

	public void ShowRedpacketRulePanel(){
		GameObject temp= GameObject.Instantiate (redpacketRulePanel);
		temp.transform.parent = ScreenManager.uiScaler.transform;
		temp.transform.position = Vector3.zero;
		temp.transform.localScale = Vector3.one;
	}

	void UpdateIndicatorPos(){
		//Debug.LogError ("UpdagteIndicatorPos:" + percent);
		indicatorGroup [0].transform.position = indicatorOrginalPos + Vector3.right * progressLength * percent;
	}
	void SetIndicatorShow(bool toShow){
		indicatorGroup [0].gameObject.SetActive (toShow);
		if (toShow) {
			UpdateIndicatorPos ();
		} 
		indicatorIsShow = toShow;
	}
}
