using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ManmonTps2 : MonoBehaviour
{


	// Use this for initialization
	void Start ()
	{
		
	}
	
	// Update is called once per frame
	void Update ()
	{
		
	}

	public void GoShop ()
	{
		string path = "Window/NewStoreCanvas";
		GameObject WindowClone = AppControl.OpenWindow (path);
		WindowClone.gameObject.SetActive (true);
		Destroy (this.gameObject);
	}

	public void btnClose ()
	{
		Destroy (this.gameObject);
	}
}
