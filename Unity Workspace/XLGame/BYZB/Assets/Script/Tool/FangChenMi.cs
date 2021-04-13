using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FangChenMi : MonoBehaviour {

    public static FangChenMi _instance;

    Coroutine timerCoroutine;

    //IEnumerator timer;

    void Awake()
    {
        if(_instance  == null)
        {
            _instance = this;
        }
        DontDestroyOnLoad(this.gameObject);

    }

	// Use this for initialization
	void Start () {
        //timer = Timer();
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public void StartTimer()
    {
        timerCoroutine = StartCoroutine(Timer());
    }
    public void StopTimer()
    {
        StopCoroutine(timerCoroutine);
    }

    IEnumerator Timer()
    {
        yield return new WaitForSeconds(7200);
        SetWindowInfo(2);
        yield return new WaitForSeconds(7200);
        SetWindowInfo(4);
        yield return new WaitForSeconds(7200);
        SetWindowInfo(6);
        yield return new WaitForSeconds(7200);
        SetWindowInfo(8);
    }

    void SetWindowInfo(int hour)
    {
        GameObject WindowClone;
        if(FCMLife.IsLive)
        {
            WindowClone = FCMLife._instance.gameObject;
        }
        else
        {
            string path = "Window/WindowTIpsTime";
            WindowClone = AppControl.OpenWindow(path);
            FCMLife.IsLive = true;
        }

 

        WindowClone.SetActive(true);
        Text text = WindowClone.transform.Find("Label").GetComponent<Text>();

        text.text = string.Format("{0}{1}{2}", "您今日已連續在線", hour.ToString(), "小時,\n請合理安排遊戲時間，注意休息");

        Button button = WindowClone.transform.Find("Button").GetComponent<Button>();

        button.onClick.RemoveAllListeners();

        button.onClick.AddListener(delegate{
            ButtonEvent(hour, WindowClone);
        });

    }

    void ButtonEvent(int hour,GameObject obj)
    {
        switch(hour)
        {
            case 2:
            case 4:
            case 6:
                FCMLife.IsLive = false;
                Destroy(obj);
                break;
            case 8:
                DataControl.GetInstance().ShutDown();
                Application.Quit();
                break;
        }
    }
}
