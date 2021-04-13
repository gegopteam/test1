using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class TorpedoPool
{
	private List<object> listTorpedo = null;

	public TorpedoPool()
	{
		Init ();
	}

	~TorpedoPool()
	{
		UnInit ();
	}

	private void Init()
	{//初始化，私有函数
		listTorpedo = new List<object>();
	}

	private void UnInit()
	{//反初始化，私有函数
		Clear ();
		listTorpedo = null;
	}

	public void Add(object data)
	{
		if (null == data)
			return;
	
		listTorpedo.Add (data);
	}

	public void Remove(int bulletId, int userId)
	{
		Bullet bullet = null;
		for(int i=listTorpedo.Count-1; i>=0; i--)
		{
			bullet = (Bullet) listTorpedo [i];
			if(bulletId==bullet.bulletID && userId==bullet.userID)
			{
				bullet.HitFish ();
				listTorpedo.Remove (bullet);
				MonoBehaviour.Destroy (bullet.gameObject);

			}
		}
	}

	public Bullet Get(int userId, int bulletId)
	{
		Bullet infoGet = null;
		Bullet bullet = null;
		for(int i=listTorpedo.Count-1; i>=0; i--)
		{
			bullet = (Bullet) listTorpedo [i];
			if(userId==bullet.userID && bulletId==bullet.bulletID)
			{
				infoGet = bullet;
			}
		}
		return infoGet;
	}

	public void Clear()
	{
		if(null!=listTorpedo) 
		{
			listTorpedo.Clear ();
		}
	}
}
