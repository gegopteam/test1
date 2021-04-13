using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIShowTeXiao : MonoBehaviour
{
	//这里是将camaer转换
	private GameObject store;
	private Camera storeCamera;
	private Canvas mainCanvas;
	// Use this for initialization
	void Start ()
	{
		store = GameObject.FindGameObjectWithTag ("UICamera");
		Debug.Log (store.name);
		storeCamera = store.GetComponent<Camera> ();
		mainCanvas = transform.GetComponentInChildren<Canvas> ();
		mainCanvas.planeDistance = 30f;
		mainCanvas.worldCamera = storeCamera;
	}
	
	// Update is called once per frame
	void Update ()
	{
		
	}
}
