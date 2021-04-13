using UnityEngine;
using System.Collections;
using DG.Tweening;
using UnityEngine.UI;

using AssemblyCSharp;

public class RedPacket_Unpack : MonoBehaviour {

	public GameObject redPacketForMovePrefab;
	GameObject redPacketForMoveTemp=null;

	public GameObject closedRedPacket;
	public GameObject openedRedPacket;

	public GameObject finishPoint;

	public GameObject redPacketIcon_LT;

	public  ParticleSystem  ps_OpenEffect;
	public GameObject openEffectPrefab;

	public static RedPacket_Unpack _instance;

	float rmbReturn=0;
	public Text intergerNumText;
	public Text decimalNumText;

	bool shouldCheckInput=false;

	void Awake(){
		if (null == _instance)
			_instance = this;
	}

	void Update(){
		if (shouldCheckInput) {
			if (Input.GetMouseButtonDown (0)) {
				PopHide ();
			}
		}

	}

	//------------------这段代码暂时舍弃!!!!!!!!!!!!!!!!!!!!
	int tempRepacketId; //临时id，移动后会存储在左上角
	public void ShowRedPacket(int tempId){ //生成一个红包移动向左上角
		redPacketForMoveTemp = GameObject.Instantiate (redPacketForMovePrefab, Vector3.zero, Quaternion.identity) as GameObject ;
		tempRepacketId = tempId;
		Invoke ("DelayMove", 1f);
	}
	void DelayMove(){
	//	redPacketForMoveTemp.transform.DOMove (redPacketIcon_LT.transform.position, 1f);
		//Invoke ("DelayHide", 4f);
	}
	void DelayHide(){ //移动完毕后，改变左上角样式
		Destroy (redPacketForMoveTemp);
		redPacketForMoveTemp = null;
		//redPacketIcon_LT.GetComponent<LT_GetRedPacket> ().AddOneRedpacket (tempRepacketId);
	}
	public void ShowRedPacket2(){ //初版的红包场功能
	//	RedPacket_TopInfo._instance.ShowRedPacketRain ();
		closedRedPacket.SetActive (true);
		GameController._instance.gameIsReady = false;
		Invoke ("AutoHide", 10f); //这个界面不抢红包，会不会累计金币?
	}

	void AutoHide(){
		closedRedPacket.SetActive (false);
		GameController._instance.gameIsReady = true;
		//RedPacket_TopInfo._instance.DestroyRedPacketRain ();
	}
	//--------------

	public void SendOpenRedPacket(int redpacketId){
		//RedPacket_TopInfo._instance.DestroyRedPacketRain ();

		RedPacketMsgHandle msgHandle=new RedPacketMsgHandle();
		RoomInfo myRoomInfo= DataControl.GetInstance ().GetRoomInfo ();
		RedPacket_TopInfo._instance.ResetProgress ();

		msgHandle.SendOpenRedPacketRequest (redpacketId);

		//ps_OpenEffect.Play ();
		GameObject tempEffect = GameObject.Instantiate (openEffectPrefab);
		tempEffect.transform.position = Vector3.zero;
		tempEffect.transform.localScale = Vector3.one;
		Destroy (tempEffect, 2f);
		//GetOpenedRedPacketGold (4.6f);	
	}

	public void GetOpenedRedPacketGold(float num,int redpacketID){
		ps_OpenEffect.Play ();
		AudioManager._instance.PlayEffectClip (AudioManager.effect_redPacketOpen);
		rmbReturn = num;
		openedRedPacket.SetActive (true);
		openedRedPacket.GetComponent<RectTransform> ().localScale = Vector3.one;
		openedRedPacket.GetComponent<Animator> ().enabled = false;
		closedRedPacket.SetActive (false);
		SetRmbNum (num);
		shouldCheckInput = true;
	}

	public void PopHide(){
		openedRedPacket.GetComponent<Animator> ().enabled = true;
		GunControl tempGun = UIFishingObjects.GetInstance ().cannonManage.GetLocalGun ();
		tempGun.gunUI.ShowRedPacketEffect (rmbReturn);
		shouldCheckInput = false;
		GameController._instance.gameIsReady = true;
		Invoke ("ShowFinishPoint", 0.1f);
	}

	public void ShowFinishPoint(){
		finishPoint.transform.localPosition = Vector3.zero;
		finishPoint.SetActive (true);

		Invoke ("MoveFinishPoint", 1f);
	}

	void MoveFinishPoint(){
		finishPoint.transform.DOMove (redPacketIcon_LT.transform.position, 1f);
		openedRedPacket.SetActive (false);
		Invoke ("GiveRmb", 1f);
	}

	public void GiveRmb(){
		finishPoint.SetActive (false);
		redPacketIcon_LT.GetComponent<LT_GetRedPacket> ().AddRmbNum (rmbReturn);
	}

	public void SetRmbNum(float num){ //已知Num一定是 a.b 形势的
		int intergerNum;
		int decimalNum;

		intergerNum = (int)num;
		decimalNum = Mathf.RoundToInt ( (num - intergerNum) * 10);

		intergerNumText.text = intergerNum.ToString ();
		decimalNumText.text = decimalNum.ToString ();
	}


}
