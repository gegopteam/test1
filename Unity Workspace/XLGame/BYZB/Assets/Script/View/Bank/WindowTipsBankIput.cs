using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class WindowTipsBankIput : MonoBehaviour
{
	

	public Button clolse;

	public  Bank_BankManager bankmange;

	public InputField passward;
	// Use this for initialization
	void Start ()
	{
		UIColseManage.instance.ShowUI (this.gameObject);
	}


	public void GetMoney ()
	{
		bankmange.GetCoinClick (passward.text);
		//Debug.Log("i put the password is = "+ passward.text);
		UIColseManage.instance.CloseUI ();
	}


	public void Close ()
	{
		UIColseManage.instance.CloseUI ();

	}
	// Update is called once per frame
	void Update ()
	{
		
	}
}
