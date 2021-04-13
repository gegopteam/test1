using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using AssemblyCSharp;

public class EnemyPoor : MonoBehaviour {

	public static EnemyPoor _instance;

	AudioSource thisAudio;

	void Awake()
	{
		if (null == _instance)
			_instance = this;
		thisAudio = this.GetComponent<AudioSource> ();

        List<FiFishsCreatedInform> cache = UIFishingObjects.GetInstance().fishCreatedCache;
        if(cache.Count>0){
            for (int i = 0; i < cache.Count; i++)
            {
                UIFishingObjects.GetInstance().CreateFish(cache[i]);
                //Debug.LogError("Create cache");
            }
        }
        UIFishingObjects.GetInstance().fishCreatedCache.Clear();
	}

	


	public void CreateEnemy(int trackType, int trackID, int groupId, int enemyTypeID, int enemyID, FormationType formation = FormationType.None,
                            int groupLength = 1, int fishTide = -1, float intervalTime = 0.6f, float scaleValue = 1f)
	{
		GameObject enemyObj = GameObject.Instantiate( PrefabManager._instance.FindEnemyByTypeID (enemyTypeID),transform.position,Quaternion.identity)as GameObject ;

		EnemyBase enemyBase = enemyObj.GetComponent<EnemyBase>();
		enemyBase.SetEnemyInfo(trackType-1,trackID-1,enemyTypeID, groupId, enemyID,formation,groupLength,fishTide ,intervalTime);//这里typeID似乎可以不赋值
        if (formation!=FormationType.Circle){
            enemyBase.transform.localScale = enemyBase.transform.localScale * scaleValue ;
        }
		if (trackType == 4) {
			Debug.LogWarning ("Summon:" + enemyTypeID + " name:" + enemyObj.name);
		}
		//Debug.LogError ("addFish,groupId："+groupId+ " fishId:"+enemyID+"track:"+trackType+","+trackID);
		//UIFishingMsg.GetInstance ().fishPool.Add (enemyBase);
		UIFishingObjects.GetInstance ().fishPool.Add (enemyBase);
	}
	//编号1-3的鱼群按照单双走直线或双直线，编号4-7的鱼，按照数量一起移动，同数量走不同的阵列  

	public void PlayEnemyVoice(AudioClip clip){
		if (thisAudio.isPlaying|| !AudioManager._instance.useEffectClip) {
			return;
		}
		thisAudio.PlayOneShot (clip);
	}

}
