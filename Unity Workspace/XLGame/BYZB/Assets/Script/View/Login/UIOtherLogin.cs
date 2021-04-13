using UnityEngine;
using System.Collections;

public class UIOtherLogin : MonoBehaviour {

	private GameObject WindowClone;
	// Use this for initialization
	void Start () {
		//DontDestroyOnLoad (WindowClone);
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	public void OnExit()
	{
		gameObject.SetActive (false);
	}

	public void OnOpen()
	{
		gameObject.SetActive ( true );
		//transform.parent.FindChild ("AccountLoginFrame").gameObject.SetActive ( true );
	}

	public void QQButton()
	{
		
	}

	public void OnAccoutButton()
	{
		gameObject.SetActive ( false );
		transform.parent.Find ("AccountLoginFrame").gameObject.SetActive ( true );
	}
}
