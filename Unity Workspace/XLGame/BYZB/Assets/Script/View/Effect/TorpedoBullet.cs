using UnityEngine;
using System.Collections;
using System.Collections.Generic;

using AssemblyCSharp;

public class TorpedoBullet : MonoBehaviour
{

	[HideInInspector]
	public int bulletId;
	[HideInInspector]
	public int userId;
	[HideInInspector]
	public bool isLocal;
	public int torpedoLevel;

	public GameObject explodeEffect;

	float attackRange;

	public List<EnemyBase> enemyHittedList = null;

	public GameObject torpedoLockEffect;

	public GameObject coinsEffectPrefab;

	// Use this for initialization
	void Start ()
	{
		//Invoke ("Explosion", 1.5f); //通过在动画文件中调用，不需要代码控制
		
	}

	public void SetInfo (int bulletId, int userId, int torpedoLevel, bool isLocal)
	{
		this.bulletId = bulletId;
		this.userId = userId;
		this.torpedoLevel = torpedoLevel;
		this.isLocal = isLocal;

		if (isLocal) {
			this.gameObject.tag = TagManager.torpedoBullet;
		}

		if (torpedoLevel == 7) {
			attackRange = 2.5f;
		} else {
			attackRange = 2f + torpedoLevel * 0.3f;
		}
	}

	public void Explosion ()
	{ //虽然代码里没有引用，但是在动画文件结束时是会调用该方法的
		GameObject tempEffect = GameObject.Instantiate (explodeEffect, transform.position, Quaternion.identity)as GameObject;
		GetHittedList ();
		Destroy (tempEffect, 1f);
		//Destroy (transform.parent.gameObject,1.2f);
		Destroy (torpedoLockEffect, 1.2f); 
	}

	public void ReturnGold (int coinCount, int userID)
	{
//        Debug.LogError("TorpedoReturnGold="+coinCount);
		CannonInfo info = UIFishingObjects.GetInstance ().cannonManage.GetInfo (userID);

		if (null != info) {
			if (null != info.cannon) {
				Vector3 coinsPos = new Vector3 (transform.position.x, transform.position.y, 0f);


				GameObject coinGroup = GameObject.Instantiate (coinsEffectPrefab, coinsPos, Quaternion.identity) as GameObject;

				coinGroup.GetComponent<CoinEffectGroup> ().StartMoveToPlayer (info, coinCount, 1000, 1.7f, false);
				AudioManager._instance.PlayEffectClip (AudioManager.effect_getCoin, 0.5f);
			}
		} else {
			Debug.LogError ("Error:info=null");
		}

		Destroy (transform.parent.gameObject, 1f); //延迟销毁试试
	}

	public void  GetHittedList ()
	{
		
		if (isLocal) {
			List <EnemyBase> enemyList = UIFishingObjects.GetInstance ().fishPool.listEnemyBase;
			List<FiFish> fishGroup = new List<FiFish> ();

			for (int i = 0; i < enemyList.Count; i++) {
				if (GetPlaneDistance (enemyList [i].transform.position, this.transform.position) < attackRange) {
					if (enemyList [i].isBoss) //boss鱼不在鱼雷攻击范围内
						continue;
					enemyHittedList.Add (enemyList [i]);

					FiFish tempItem = new FiFish ();
					tempItem.groupId = enemyList [i].groupID;
					tempItem.fishId = enemyList [i].id;
					fishGroup.Add (tempItem);
				}
			}
			int serverSkillId = Skill.GetServerIdFromTorpedoLevel (torpedoLevel);
			if (GameController._instance.myGameType == GameType.Classical) {
				UIFishingMsg.GetInstance ().SndTorpedoExplodeRequest (bulletId, serverSkillId, fishGroup);
			} else {
				UIFishingMsg.GetInstance ().SndPKTorpedoExplodeRequest (bulletId, serverSkillId, fishGroup);
			}
		}


	}


	public float GetPlaneDistance (Vector3 pos1, Vector3 pos2)//获取两个3d坐标转化到同一平面之后的距离
	{
		pos2 = new Vector3 (pos2.x, pos2.y, pos1.z);
		return Vector3.Distance (pos1, pos2);
	}
}
