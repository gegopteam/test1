using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using DG.Tweening;

public class RedpacketForOpen : MonoBehaviour {

	SelectRedpacketPanel panelParent;

	double rmbReturn;
	public int seatIndex=-1;//左中右分别为0，1，2
	public GameObject unOpenedPacket;
	public GameObject openSuccessedPacket;
	public GameObject openFailedPacket;

	public ParticleSystem ps_openEffect;
	public GameObject finishPointEffect;

	// Use this for initialization
	void Start () {
		panelParent = transform.parent.parent.GetComponent<SelectRedpacketPanel> ();
	}

	public void SendOpenRequest(){
		panelParent. SendOpenRedpacket (seatIndex);
        Debug.LogError("Send open redpacket:"+seatIndex);
		transform.Find ("BtnArea").GetComponent<Button> ().enabled = false;
	}
	public void ShowOpenSuccess(double rmbNum){
		unOpenedPacket.SetActive (false);
		ps_openEffect.Play ();
		openSuccessedPacket.SetActive (true);
		rmbReturn = rmbNum;
		openSuccessedPacket.transform.Find ("RedPackage_chai/Kapian/RmbText").GetComponent<Text> ().text = (rmbNum*0.01).ToString ();
		Invoke ("DelayPlayAnim", 1.5f);

	}
		
	void DelayPlayAnim(){
		openSuccessedPacket.GetComponentInChildren<Animator> ().enabled = true;
		finishPointEffect.SetActive (true);
		Invoke ("DelayMove",1f);
	}
	void DelayMove(){
		finishPointEffect.transform.DOMove (LT_GetRedPacket._instance.transform.position,1f);
		Destroy (panelParent.gameObject,1.2f);
		Invoke ("DelaySet", 0.6f);
	}

	void DelaySet(){
		LT_GetRedPacket._instance.GetRedPacketAdd (rmbReturn);
		

	}

	public void DisableBtn(){
		transform.Find ("BtnArea").GetComponent<Button> ().enabled = false;
	}

	public void ShowOpenFailed(double rmbNum){
		unOpenedPacket.SetActive (false);
		openFailedPacket.SetActive (true);
		openFailedPacket.transform.Find ("RedPackage_chai/Kapian/RmbText").GetComponent<Text> ().text = rmbNum.ToString ();
		Invoke ("DelayPopHide", 1.5f);
	}

	void DelayPopHide(){
		openFailedPacket.transform.DOScale (0, 0.5f);
	}
	// Update is called once per frame
	void Update () {
	
	}
}
