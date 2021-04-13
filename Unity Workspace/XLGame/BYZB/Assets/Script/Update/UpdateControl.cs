using System.Collections;
using System.Collections.Generic;
using UnityEngine;

class UpdateData
{
	string name; //资源名称
	int version; //版本号
}

public class UpdateControl
{
	private static UpdateControl instance = null;

	public static UpdateControl GetInstance()
	{
		if(null == instance)
		{
			instance = new UpdateControl ();
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

	string versionFileLocal;
	string versionFileSvr;
	string versionLocal;
	string versionSvr;

	private UpdateControl()
	{
		//WWW www = new WWW ("www.baidu.com");
	}

	~UpdateControl()
	{

	}

	public void OpenUpdate()
	{//打开更新
		
	}

}
