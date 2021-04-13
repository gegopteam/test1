using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FCMLife : MonoBehaviour {

    public static bool IsLive = false;

    public static FCMLife _instance;

    GameObject fcm;

    private void Awake()
    {
        if(_instance == null)
        {
            _instance = this;
            fcm = this.gameObject;
        }
    }


    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    private void OnDestroy()
    {
        _instance = null;
        fcm = null;
        IsLive = false;
    }
}
