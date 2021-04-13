using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using AssemblyCSharp;
using UnityEngine.UI;

public class ManmonTps1 : MonoBehaviour
{
	private MyInfo myInfo;
	public Text textshow;
	// Use this for initialization
	void Start ()
	{
		myInfo = (MyInfo)Facade.GetFacade ().data.Get (FacadeConfig.USERINFO_MODULE_ID);
	}
	
	// Update is called once per frame
	void Update ()
	{
		
	}

	public void Exit ()
	{
		UIManmonGameShow.instance.Btnclose ();
		btnClose ();
	}

	public void Continue ()
	{
		btnClose ();
	}

	public void btnClose ()
	{
		Destroy (this.gameObject);
	}
}
