using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ChangeSeverUrl : MonoBehaviour
{
	public InputField Severurl;
	public InputField portNumber;
	public Text severurl;
	public Text portnumber;

	// Use this for initialization
	void Start ()
	{
		
	}

	public void Sure ()
	{
		Debug.LogError ("2222222222222");
		if (Severurl.text != "") {
			Debug.LogError ("Severurl.text" + Severurl.text + "portNumber.text" + portNumber.text);
			UILogin.Instance.mServerUrl = Severurl.text;
			UILogin.Instance.IschangeSever = true;
		}
		if (portNumber.text != "") {
			Debug.LogError ("11111111111");
			Debug.LogError ("Severurl.text" + Severurl.text + "portNumber.text" + portNumber.text);
			AppInfo.portNumber = int.Parse (portNumber.text);
			UILogin.Instance.IschangeSever = true;
		}
	}

	public void ceshi ()
	{
		UILogin.Instance.mServerUrl = "183.131.69.72";
		AppInfo.portNumber = 50666;
		UILogin.Instance.IschangeSever = true;
	}

	public void zhengshi ()
	{
		UILogin.Instance.mServerUrl = "183.131.69.234";
		AppInfo.portNumber = 50666;
		UILogin.Instance.IschangeSever = true;
	}


	public void banzhengshi ()
	{
		UILogin.Instance.mServerUrl = "183.131.69.234";
		AppInfo.portNumber = 50776;
		UILogin.Instance.IschangeSever = true;
	}

	// Update is called once per frame
	void Update ()
	{
		
	}
}
