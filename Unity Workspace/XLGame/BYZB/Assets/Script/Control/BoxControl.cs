/* author:KinSen
 * Date:2017.05.15
 */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//管理所有弹框界面
public class BoxControl {
	private static BoxControl instance = null;

	public static BoxControl GetInstance()
	{
		if(null == instance)
		{
			instance = new BoxControl ();
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

	private BoxControl()
	{

	}

	~BoxControl()
	{

	}

}
