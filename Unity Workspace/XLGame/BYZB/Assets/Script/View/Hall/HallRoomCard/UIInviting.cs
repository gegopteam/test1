using UnityEngine;
using System.Collections;

public class UIInviting : MonoBehaviour {

	// Use this for initialization
	void Start () {
		DontDestroyOnLoad (this.gameObject);
	
	}

	public void ExitButton()
	{
		this.transform.gameObject.SetActive (false);
	}

	public void WeChatButton()
	{
		
	}

	public void QQButton()
	{
		
	}

	// Update is called once per frame
	void Update () {
	
	}
}
