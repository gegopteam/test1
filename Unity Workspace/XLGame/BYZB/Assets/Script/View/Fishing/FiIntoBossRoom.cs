using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FiIntoBossRoom : MonoBehaviour {

    Button IntoRoomBtn;
    Button CloseWindowBtn;
    void Awake()
    {
        gameObject.GetComponent<Canvas>().worldCamera = ScreenManager.uiCamera;
    }

    void Start()
    {
        IntoRoomBtn = transform.Find("Bg/IntoRoom").GetComponent<Button>();
        IntoRoomBtn.onClick.RemoveAllListeners();
        IntoRoomBtn.onClick.AddListener(JoinBoosRoom);
        CloseWindowBtn = transform.Find("Bg/Close").GetComponent<Button>();
        CloseWindowBtn.onClick.RemoveAllListeners();
        CloseWindowBtn.onClick.AddListener(CloseWindow);

    }

    public void JoinBoosRoom()
    {
        UIColseManage.instance.intoBossRommType = 0;
        LeaveRoomTool.LeaveRoom();
    }
    public void CloseWindow()
    {
        Destroy(this.gameObject);
    }
}
