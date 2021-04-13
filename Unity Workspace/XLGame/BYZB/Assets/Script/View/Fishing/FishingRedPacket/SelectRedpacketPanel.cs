using UnityEngine;
using System.Collections;
using AssemblyCSharp;
using UnityEngine.UI;
using DG.Tweening;

public class SelectRedpacketPanel : MonoBehaviour {

	public RedpacketForOpen[] packetForOpenGroup;
	int redPacketId;
	int redPacketLevel;

	int currentOpendIndex;

	public static SelectRedpacketPanel _instance;

	public Image[] fadeImageGroup;

	double[][]redpacketTypeGroup=new double[3][];

	void Awake(){
		_instance = this;

		redpacketTypeGroup [0] = new double[]{ 0.3, 0.6, 1.2 };
		redpacketTypeGroup [1] = new double[]{ 0.6, 1.2, 2.4 };
		redpacketTypeGroup [2] = new double[]{ 1.2, 2.4, 4.8 };
	}



	public void SetInfo(int id,int level){
		redPacketId = id;
		redPacketLevel = level;
	}

	public void CloseStaticInfo(){
	
	}


	public void SendOpenRedpacket(int index){ //0,1,2代表三个红包,该方法在点击打开某个红包时，通过红包对象调用 
		Debug.LogError ("发送开红包消息：id=" + redPacketId);
		currentOpendIndex =index ;

		RedPacketMsgHandle msgHandle=new RedPacketMsgHandle();
		msgHandle.SendOpenRedPacketRequest (redPacketId);
		DisableOtherBtn (index);//发送红包后把按钮关闭
	}
	public void RecvOpenRedpacket(double num){
        Debug.LogError("RecvOpen:"+num);
		SetOtherPacketOpenFailed (currentOpendIndex,num);
		ShowOpenSuccess (num);
		for (int i = 1; i < fadeImageGroup.Length; i++) {
			fadeImageGroup [i].DOFade (0, 1f);
		}
		fadeImageGroup [0].enabled = false;
	}



	public void SetOtherPacketOpenFailed(int exceptIndex,double exceptNum){ //
		int tempIndex=-1;
		for (int i = 0; i < 3; i++) {
			if (exceptNum == redpacketTypeGroup [redPacketLevel - 1] [i]) {
				tempIndex = i;
			}
		}
		int failedIndex = 0;
		double failedNum;
		for(int i=0;i<packetForOpenGroup.Length;i++){
			if (i != exceptIndex) {
				if (failedIndex == tempIndex)
					failedIndex++;
				failedNum = redpacketTypeGroup [redPacketLevel - 1] [failedIndex];
				failedIndex++;
				packetForOpenGroup [i].ShowOpenFailed (failedNum);
			} 
		}
	}
	void DisableOtherBtn(int exceptIndex){
		for(int i=0;i<packetForOpenGroup.Length;i++){
			if (i != exceptIndex) {
				packetForOpenGroup [i].DisableBtn ();
			} 
		}
	}

	void ShowOpenSuccess(double rmbNum){
		for(int i=0;i<packetForOpenGroup.Length;i++){
			if (i == currentOpendIndex) {
				packetForOpenGroup [i].ShowOpenSuccess ( rmbNum);
			} 
		}
	}
}
