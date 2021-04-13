using UnityEngine;
using System.Collections;

using AssemblyCSharp;

public class UILoginMsg : IDispatch
{
	private static UILoginMsg instance = null;

	public static UILoginMsg GetInstance()
	{
		if(null == instance)
		{
			instance = new UILoginMsg ();
		}

		return instance;
	}

	public static void DestroyInstance()
	{
		if(null!=instance)
		{
			instance = null;
		}
	}

	private DispatchControl dispatchControl = null;

	public UILoginMsg()
	{
		dispatchControl = DispatchControl.GetInstance ();
		dispatchControl.AddRcv (AppFun.LOGIN, this);
	}

	~UILoginMsg()
	{
		
	}

	public void OnRcv(int type, object data)
	{
		
	}

}
