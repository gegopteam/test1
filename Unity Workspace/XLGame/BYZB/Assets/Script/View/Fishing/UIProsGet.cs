using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class UIProsGet : MonoBehaviour {
	public Text toolOne;
	public Text toolTwo;
	public Text toolthree;
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	public void OnButton()
	{
		transform.gameObject.SetActive (false);
	}
}
