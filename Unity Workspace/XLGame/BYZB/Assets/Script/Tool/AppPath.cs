using UnityEngine;
using System.Collections;

public class TypePath
{
	public const int NONE = 0;
	public const int PERSISTENT_DATA_PATH = 1;
	public const int TEMPORARY_CACHE_PATH = 2;
	public const int DATA_PATH = 3;
	public const int STREAMING_ASSETS_PATH = 4;
	public const int RESOURCES = 5;
	public const int RESOURCES_PREFABS = 6;

}

public class AppPath
{
	public static string Get(int typePath)
	{
		string path = "";

		return path;
	}
}
