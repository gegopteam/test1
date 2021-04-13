using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class FishPool
{
	public  List<EnemyBase> listEnemyBase = null;

	public FishPool()
	{
		Init ();
	}

	~FishPool()
	{
		UnInit ();
	}

	private void Init()
	{//初始化，私有函数
		listEnemyBase = new List<EnemyBase>();
	}

	private void UnInit()
	{//反初始化，私有函数
		Clear ();
		listEnemyBase = null;
	}

	public void Add(EnemyBase enemy)
	{
		if (null == enemy)
			return;
			
		for(int i=listEnemyBase.Count-1; i>=0; i--)
		{
			if(enemy.groupID==listEnemyBase[i].groupID && enemy.id==listEnemyBase[i].id)
			{
				Tool.LogWarning ("============添加了重复的鱼进来 groupId："+enemy.groupID + " fishId:"+enemy.id);
				if(enemy==listEnemyBase[i])
				{
					Tool.LogError ("============对象是同一个的鱼被添加");
				}
			}
		}
		//Tool.LogWarning ("添加鱼 groupId："+enemy.groupID + " fishId:"+enemy.id);

		listEnemyBase.Add (enemy);
	}

	public bool  Remove(int groupId, int fishId, float delay=0f)
	{
		bool removeSuccess = false;
		for(int i=listEnemyBase.Count-1; i>=0; i--)
		{
			if(groupId==listEnemyBase[i].groupID && fishId==listEnemyBase[i].id)
			{
				removeSuccess = true;
				EnemyBase enemyBase = listEnemyBase [i];
				Tool.LogWarning ("Remove 删除鱼 groupId："+enemyBase.groupID + " fishId:"+enemyBase.id);
				listEnemyBase.Remove (enemyBase);
				if (delay != 0)
					MonoBehaviour.Destroy (enemyBase.gameObject, delay);
				else
					MonoBehaviour.Destroy (enemyBase.gameObject);	
			}
		}
		return removeSuccess;
	}

	public void RemoveGroupFish(int groupId, float delay=0f)
	{
		return; //return情况下，所有移除鱼的操作均以本地为标准
		for(int i=listEnemyBase.Count-1; i>=0; i--)
		{
			if(groupId==listEnemyBase[i].groupID)
			{
				EnemyBase enemyBase = listEnemyBase [i];
				Tool.LogWarning ("RemoveGroupFish 删除鱼 groupId："+enemyBase.groupID);
				listEnemyBase.Remove (enemyBase);
				if (enemyBase != null)
					MonoBehaviour.Destroy (enemyBase.gameObject, delay);
				else
					Debug.Log ("无法删除鱼，物体已被移除");
			}
		}
	}

	public  List<EnemyBase>  GetGroupFish(int groupId,bool shouldDebugInfo=true )
	{
		List<EnemyBase >tempEnemyGroup=new List<EnemyBase>();
		for (int i = 0; i < listEnemyBase.Count; i++) {
			if (listEnemyBase [i].groupID == groupId) {
				tempEnemyGroup.Add (listEnemyBase [i]);
			}
		}
		if (tempEnemyGroup.Count == 0) {
			if(shouldDebugInfo)
				Debug.LogError ("Error!!!Can't Get GroupFish:" + groupId);
			//return null;
		}

		return tempEnemyGroup ;
	}

	public EnemyBase Get(int groupId, int fishId)
	{
		EnemyBase info = null;
		for(int i=listEnemyBase.Count-1; i>=0; i--)
		{
			if(groupId==listEnemyBase[i].groupID && fishId==listEnemyBase[i].id)
			{
				info = listEnemyBase [i];
			}
		}
		return info;
	}

	public EnemyBase GetAim()
	{//获取瞄准的鱼
		EnemyBase info = null;
		int maxGold = 0;
		for(int i=listEnemyBase.Count-1; i>=0; i--)
		{
			if(listEnemyBase[i].goldReturn>maxGold && listEnemyBase [i].hasInScreen )
			{
				info = listEnemyBase [i];
				maxGold = listEnemyBase [i].goldReturn;
			}
		}
		return info;
	}

    public EnemyBase GetRepclicationLock(int index){
        EnemyBase info = null;
        while(index>listEnemyBase.Count){
            index--;
        }
        if(index>0){
            info = listEnemyBase[listEnemyBase.Count - index];
            if (!info.hasInScreen){
                info = GetAim();
            }
        }
        return info;
    }

	public void SetAllFishLeaveScreen(){
		for (int i = 0; i < listEnemyBase.Count; i++) {
			listEnemyBase [i].LeaveScreen ();
		}
	}

	public void Clear()
	{
		if(null!=listEnemyBase)
			listEnemyBase.Clear ();
	}
}
