using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class IosPayMoney : MonoBehaviour {
	public Button[] btns;
	// Use this for initialization
	void Start () {
		btns = GetComponentsInChildren<Button> ();
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
