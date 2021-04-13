using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Spine.Unity;

public class ManmonEffect : MonoBehaviour
{
	public SkeletonGraphic sgp;
	public long ShowNum;

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
		mainCanvas.worldCamera = storeCamera;
		sgp = sgp ?? gameObject.GetComponent<SkeletonGraphic> ();
		PlayAnim ("godofwealth2");
		StartCoroutine (Destorys ());
	}
	
	// Update is called once per frame
	void Update ()
	{
		
	}

	public void PlayAnim (string animName)
	{
		sgp.AnimationState.SetAnimation (0, animName, false);
		StartCoroutine (Destorys ());
	}

	IEnumerator Destorys ()
	{
		yield return new WaitForSeconds (4f);
		string path = "Window/WindowManmomTips";
		UnityEngine.GameObject WindowClone = AppControl.OpenWindow (path);
		WindowClone.gameObject.SetActive (true);
		WindowManmomTips tips = WindowClone.gameObject.GetComponent<WindowManmomTips> ();
		if (ShowNum != null) {
			tips.rewardnum = ShowNum;
		}

//		Destroy (this.gameObject);
		this.gameObject.SetActive (false);
	}
}
