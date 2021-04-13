/* author:KinSen
 * Date:2017.07.04
 */

using UnityEngine;
using System.Collections;
using System.Collections.Generic;

using LitJson;

public class TypePrefab
{
	public const int NONE = 0;
	public const int NUM_NORMAL = NONE;
	public const int NUM_EFFECT = 100000;
	public const int NUM_FISH = 200000;

	public const int SocketToUI = NUM_NORMAL+1;

	public const int Reward_Gold = NUM_EFFECT+1; //金币效果
	public const int Reward_Diamond = NUM_EFFECT+2; //钻石效果

	public const int Fish_MoGuiYv = NUM_FISH+1; //魔鬼鱼
	public const int Fish_HaiMa = NUM_FISH+2; //海马
	public const int Fish_WuGui = NUM_FISH+3; //乌龟

}

public class PrefabsFile
{
	public string updateUrl;
	public List<PrefabData> prefabs;
}

public class PrefabData
{
	public int index;
	public string name;
	public string file;

	public int typePath; //决定在哪个大目录下
	public GameObject prefab; //预制件

	public PrefabData()
	{
		index = TypePrefab.NONE;
		name = "";
		file = "";

		typePath = TypePath.NONE;
		prefab = null;
	}

	public PrefabData(PrefabData info)
	{
		index = info.index;
		name = info.name;
		file = info.file;

		typePath = info.typePath;
		prefab = info.prefab;
	}

	public PrefabData(int index, string name,  string file)
	{
		this.index = index;
		this.name = name;
		this.file = file;

		typePath = TypePath.NONE;
		prefab = null;
	}

	public PrefabData(int index, string name,  string file, int typePath, GameObject prefab)
	{
		this.index = index;
		this.name = name;
		this.file = file;

		this.typePath = TypePath.NONE;
		this.prefab = null;
	}

	public string ToJson()
	{
		return string.Format ("index:{0}, file:{1}, name:{2}", index, name, file);
	}
}

public class PrefabInfo
{
	private List<PrefabData> listPrefab = null;

	private string pathPrefab = ""; //普通本地预制件
	private string pathPrefabUpdate = ""; //服务器更新的预制件
	private string updateUrl = "";

	public PrefabInfo()
	{
		Init ();
	}

	~PrefabInfo()
	{
		Clear ();
	}

	private void Init()
	{
		listPrefab = new List<PrefabData>();

	}

	void Fun()
	{
//		AssetBundle.Unload (false); //释放AssetBundle文件内存镜像; 
//		AssetBundle.Unload(true); //释放AssetBundle文件内存镜像同时销毁所有已经Load的Assets内存对象 
//		Resources.UnloadAsset (Object); //显式的释放已加载的Asset对象，只能卸载磁盘文件加载的Asset对象
//		Resources.UnloadUnusedAssets(); //用于释放所有没有引用的Asset对象 
//		System.GC.Collect (); //主动调用回收机制释放内存
	}

	private void Clear()
	{
//		object prefab = null;
//		foreach(PrefabData tmp in listPrefab)
//		{
//			Resources.UnloadAsset (tmp.prefab);
//			tmp.prefab = null;
//		}
		listPrefab.Clear ();
		//Resources.UnloadUnusedAssets(); //用于释放所有没有引用的Asset对象 
		//System.GC.Collect (); //主动调用回收机制释放内存
	}

	private void LoadConfigLocal()
	{//加载本地预制件配置文件
//		int type = TypePrefab.NONE;
//		GameObject prefab = null;
//
//		type = TypePrefab.SocketToUI;
//		prefab = (GameObject)Resources.Load ("Prefabs/SocketToUI");
//		Add (type, prefab);
		string prefabsStr = Resources.Load<TextAsset>("File/Prefabs").text;
		Tool.Log (prefabsStr);
		//JsonData data = JsonMapper.ToObject (prefabsStr);
		//if(((IDictionary)data).Contains("updateUrl"))
		PrefabsFile dataFile = JsonMapper.ToObject<PrefabsFile> (prefabsStr);
		if(null!=dataFile)
		{
			updateUrl = dataFile.updateUrl;
			Tool.Log (dataFile.updateUrl);
			Tool.Log ("listCount:"+dataFile.prefabs.Count);
			foreach(PrefabData data in dataFile.prefabs)
			{
				Tool.Log ("Index:"+data.index+" name:"+data.name+" file:"+data.file);
				listPrefab.Add (new PrefabData (data));
			}
		}
			
		return;
	}

	private void LoadConfigUpdate()
	{//加载服务器预制件配置文件
		string path = Application.persistentDataPath;
		
	}

	private void LoadConfig()
	{
		LoadConfigLocal ();
		LoadConfigUpdate ();
	}

	private void LoadPrefabs()
	{//加载预制件
		foreach(PrefabData info in listPrefab)
		{
			if(0==info.typePath)
			{
				info.prefab = (GameObject)Resources.Load ("Prefabs/"+info.file);
			}
			else
			{
				//info.prefab = (GameObject)Resources.Load ("Prefabs/"+info.file);
			}

		}
	}

	public void Load()
	{
		LoadConfig ();
		LoadPrefabs ();
	}

	public GameObject GetPrefab(int index)
	{
		object prefab = null;
		foreach(PrefabData tmp in listPrefab)
		{
			if(index==tmp.index)
			{
				prefab = tmp.prefab;
				break;
			}
		}

		return (GameObject) prefab;
	}

	public PrefabData Get(int index)
	{
		PrefabData info = null;
		foreach(PrefabData tmp in listPrefab)
		{
			if(index==tmp.index)
			{
				info = tmp;
				break;
			}
		}

		return info;
	}

	public void Add(PrefabData info)
	{
		listPrefab.Add (info);
	}

	public void Add(int index, string name, string file, GameObject prefab)
	{
		if (null == prefab)
			return;
		PrefabData data = new PrefabData ();
		data.index = index;
		data.prefab = prefab;
		Add (data);
	}

	public void Remove(int index)
	{
		PrefabData data = null;
		for(int i=listPrefab.Count-1; i>=0; i--)
		{
			if(index==data.index)
			{
				listPrefab.Remove (listPrefab [i]);
			}
		}
	}

}
