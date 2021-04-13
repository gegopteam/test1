using UnityEngine;
using System.Collections;

public class GroupFishPathTool : MonoBehaviour {

    public float delayTime = 0f;

    public int trackType=1;
    public int trackId=1;
    public int fishType=1;
    public int fishCount = 4;
    public FormationType formation = FormationType.None;
    public float intervalTime = 0.6f;
    public float scaleValue = 1f;

    public static int groupId = 1;

	// Use this for initialization
	void Start () {
        
	    Invoke("DelayCreate",delayTime);
	}
	
    void DelayCreate(){
        CreateGroupFishTest(trackType, trackId, fishType, fishCount, formation,intervalTime,scaleValue);
    }


    void CreateGroupFishTest(int trackType, int trackId, int fishType, int fishCount,
                             FormationType tempFormation = FormationType.None, float intervalTime = 0.6f, float scaleValue = 0f)
    {

        //if (fishCount % 2 == 0)
        //    tempFormation = FormationType.Bilinear;
        // else
        //    tempFormation = FormationType.Linear;
        for (int i = fishCount-1; i >=0; i--)  //这里i必须要从大到小递减，因为i=0时的轨迹并不是动态生成的
        {
            EnemyPoor._instance.CreateEnemy(trackType, trackId, groupId, fishType, i,tempFormation, 
                                            fishCount,-1,intervalTime,scaleValue);
        }
        groupId++;
    }
}

