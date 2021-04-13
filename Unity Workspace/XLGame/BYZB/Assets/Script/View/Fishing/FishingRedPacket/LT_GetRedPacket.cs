using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using DG.Tweening;
using System.Collections.Generic;

class RedPacket
{
	public int id;
	public int level;
}

public class LT_GetRedPacket : MonoBehaviour {

	public static LT_GetRedPacket _instance;

	public Text rmbText;
	public GameObject exchangePanelPrefab;

	double totalRmb=0;

	//public int sumRedpacketNum;
	public Text sumRedpacketNumText;

	public GameObject stylePanel1;
	public GameObject stylePanel2;

	//List<int> redpacketIdList=new List<int>();
	List<RedPacket>redpacketList=new List<RedPacket>();

	public GameObject selectRedpacketPanelPrefab;

	void Awake(){
		_instance = this;
	}

	void Start () {
		if (!GameController._instance.isRedPacketMode) {
			this.gameObject.SetActive (false);
		} else {
			//totalRmb =??? //需要从某处读取初始化可兑换值
		}
  
        totalRmb = 0;
		SetShowStyle(1);
        totalRmb = DataControl.GetInstance().GetMyInfo().redPacketTicket;
        rmbText.text = (totalRmb * 0.01).ToString();
	}

	public void SetShowStyle(int styleIndex){  //1显示可领取金额，2显示当前红包数量
		switch (styleIndex) {
		case 1:
			stylePanel1.SetActive (true);
			stylePanel2.SetActive (false);
			break;
		case 2:
			stylePanel1.SetActive (false);
			stylePanel2.SetActive (true);

			break;
		}
	}

	public void GetRedPacketAdd(double rmbNum){ //拆开红包后，执行该方法，切换到界面1，加金额，然后判断剩余红包数量
		SetShowStyle (1);
		AddRmbNum (rmbNum);
		if (redpacketList.Count >= 1) {
			Invoke ("DelayChangeShow", 2f);
		}
	}

	void DelayChangeShow(){
		SetShowStyle (2);
		SetSumRedPacketNumText (redpacketList.Count);
	}

	public void Btn_LuckDrawRedpacket(){
		//sumRedpacketNum--;
		ShowRedpacketLuckDrawPanel ();
	}
	void ShowRedpacketLuckDrawPanel(){
		Debug.LogError ("ShowRedpacketLuckDrawPanel");
	}

	public void AddRmbNum(double num){ //已知Num一定是 a.b 形势的

		totalRmb += num;
        rmbText.text = (totalRmb * 0.01).ToString();

		transform.DOShakeScale (0.2f);
		DataControl.GetInstance ().GetMyInfo ().redPacketTicket += num;
	}

	public void ShowExchangePanel(){
		GameObject temp = GameObject.Instantiate (exchangePanelPrefab);
		AudioManager._instance.PlayEffectClip (AudioManager.ui_click);
		//exchangePanel.SetActive (true);
	}

	public void AddOneRedpacket(int id,int redpacketLevel){
		RedPacket temp = new RedPacket ();
		temp.id = id;
		temp.level = redpacketLevel;

		redpacketList.Add (temp);
		SetShowStyle (2);
		SetSumRedPacketNumText (redpacketList.Count);
	}

	public  void ShowRedPacketSelectPanel(){ //出现3选1界面
		AudioManager._instance.PlayEffectClip (AudioManager.ui_click);
		if (redpacketList.Count <= 0) {
			Debug.LogError ("ErrorRequest");
			return;
		}
			
		GameObject temp= GameObject.Instantiate(selectRedpacketPanelPrefab); 	
		temp.transform.parent =ScreenManager.uiScaler.transform;
		temp.transform.localScale = Vector3.one * 0.74f;
		temp.GetComponent<SelectRedpacketPanel>().SetInfo( redpacketList[0].id,redpacketList[0].level);
	
		redpacketList.Remove(redpacketList[0]);
		if (redpacketList.Count == 0) {
			SetShowStyle (1);
		} else {
			SetSumRedPacketNumText (redpacketList.Count);
			SetShowStyle (1); 
		}

	}

	void SetSumRedPacketNumText(int num){
		if (num > 0) {
			if (num == 1) {
				sumRedpacketNumText.text = "";
			} else {
				sumRedpacketNumText.text = num.ToString ();
			}
		} else {
			sumRedpacketNumText.text = "";
		}
	}

	public int GetRedpacketListCount(){
		return redpacketList.Count;
	}
}
