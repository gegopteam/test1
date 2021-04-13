/* author:KinSen
 * Date:2017.06.01
 */

using UnityEngine;
using System.Collections;

//负责资源的加载
public class ResControl
{
	private static ResControl instance = null;

	public static ResControl GetInstance()
	{
		if(null == instance)
		{
			instance = new ResControl ();
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

	public PrefabInfo prefabInfo = null;

	private ResControl()
	{
		Init ();
	}

	~ResControl()
	{
		Clear ();
	}

	private void Init ()
	{
		prefabInfo = new PrefabInfo();
	}

	private void Clear()
	{
		prefabInfo = null;
	}

	public PrefabInfo GetPrefabInfo()
	{
		return prefabInfo;
	}
}
