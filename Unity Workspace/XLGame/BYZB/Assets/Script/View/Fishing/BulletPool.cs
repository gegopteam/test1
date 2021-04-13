using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class BulletPool
{
	private List<Bullet> listBullet = null;

	public BulletPool ()
	{
		Init ();
	}

	~BulletPool ()
	{
		UnInit ();
	}

	private void Init ()
	{//初始化，私有函数
		listBullet = new List<Bullet> ();
	}

	private void UnInit ()
	{//反初始化，私有函数
		Clear ();
		listBullet = null;
	}

	public void Add (Bullet bullet)
	{
		if (null == bullet)
			return;
	
		listBullet.Add (bullet);
	}

	public void Remove (int bulletId, int userId, bool isshandian = false)
	{
		Bullet bullet = null;
		for (int i = listBullet.Count - 1; i >= 0; i--) {
			bullet = listBullet [i];
			if (bulletId == bullet.bulletID && userId == bullet.userID) {
//				Debug.LogError ("11122121212121212121221x:" + bulletId + "userid:" + userId + isshandian);
				bullet.HitFish (isshandian);
				listBullet.Remove (bullet);
				MonoBehaviour.Destroy (bullet.gameObject);

			}
		}
	}

	public Bullet GetBullet (int userId, int bulletId)
	{
		Bullet infoGet = null;
		Bullet bullet = null;
		for (int i = listBullet.Count - 1; i >= 0; i--) {
			bullet = listBullet [i];
			if (userId == bullet.userID && bulletId == bullet.bulletID) {
				infoGet = bullet;
			}
		}
		return infoGet;
	}

	public void Clear ()
	{
		if (null != listBullet)
			listBullet.Clear ();
	}
}
