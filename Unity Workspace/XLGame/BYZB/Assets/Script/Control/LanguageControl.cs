/* author:KinSen
 * Date:2017.06.01
 */

using UnityEngine;
using System.Collections;

//负责多语言支持，通过配置文件读取app中需要的字符和图片(含有某种非通用语言字符)，尽量不采用带有文字的图片
public class LanguageControl 
{
	private static LanguageControl instance = null;

	public static LanguageControl GetInstance()
	{
		if(null == instance)
		{
			instance = new LanguageControl ();
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

	private LanguageControl()
	{

	}

	~LanguageControl()
	{

	}

}
