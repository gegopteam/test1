using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class GetHonorPrizeTips : MonoBehaviour {

    Button closeBtn;
    Button joinRoom;
    Text bossNumText;
    [HideInInspector]
    public  int targetNum= 0;//目标数量
    [HideInInspector]
    public  int currentNum = 0;//当前数量
	void Start () {

        if (GameController._instance == null) //渔场和大厅里赋值的摄像机不同
            gameObject.GetComponent<Canvas>().worldCamera = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<Camera>();
        else
            gameObject.GetComponent<Canvas>().worldCamera = ScreenManager.uiCamera;
        UIColseManage.instance.ShowUI(this.gameObject);

        closeBtn = transform.Find("CloseBtn").GetComponent<Button>();
        closeBtn.onClick.RemoveAllListeners();
        closeBtn.onClick.AddListener(CloseWindow);

        joinRoom = transform.Find("JoinRoom").GetComponent<Button>();
        joinRoom.onClick.RemoveAllListeners();
        joinRoom.onClick.AddListener(JoinBossRoom);

        bossNumText = transform.Find("BossNum").GetComponent<Text>();
        bossNumText.text = "已击杀的BOSS数：" + currentNum + "/" + targetNum;

	}
	
    void JoinBossRoom()
    {
        UIHallObjects.GetInstance().PlayFieldGrade_3();
        UIColseManage.instance.CloseUI (); 
    }
    void CloseWindow()
    { 
        UIColseManage.instance.CloseUI (); 
    }

}
