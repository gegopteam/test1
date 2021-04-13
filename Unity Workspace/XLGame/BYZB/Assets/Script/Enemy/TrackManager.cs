using UnityEngine;
using System.Collections;
using DG.Tweening;

public enum TrackType
{
	typeA,
	typeB,
	typeC,
	typeD,
	typeE,
	typeF,
	typeG,
	typeH
}

public class TrackManager : MonoBehaviour {

	public static TrackManager _instance=null;

	public Transform[] trackAGroup;
	public Transform[] trackBGroup;
	public Transform[] trackCGroup;
	public Transform[] trackDGroup;
	public Transform[] trackEGroup;
	public Transform[] trackFGroup;
	public Transform[] trackGGroup;
	public Transform[] trackHGroup;

	TrackType thisTrackType;

	public Transform[] formationGroup; //用于保存Normal阵型下不同鱼数量对应的不同阵型，数量范围2到5

	void Awake()
	{
		if(null ==_instance)
			_instance = this;
		
	}
	void Start () {
		//GetTrackDuration();
	}


	void Update () {
	
	}
		

	public Transform GetTrack(int trackType, int trackId)
	{
		switch (trackType) {
		case (int)TrackType.typeA:
			if (trackId < trackAGroup.Length) {
				return trackAGroup [trackId];
			} else {
				return null;
			}
		case (int)TrackType.typeB:
			if (trackId < trackBGroup.Length) {
				return trackBGroup [trackId];
			} else {
				return null;
			}
		case (int)TrackType.typeC:
			if (trackId < trackCGroup.Length) {
				return trackCGroup [trackId];
			} else {
				return null;
			}
		case (int)TrackType.typeD:
			if (trackId < trackDGroup.Length) {
				return trackDGroup [trackId];
			} else {
				return null;
			}
		case (int)TrackType.typeE:
			if (trackId < trackEGroup.Length) {
				return trackEGroup [trackId];
			} else {
				return null;
			}
		case (int)TrackType.typeF:
			if (trackId < trackFGroup.Length) {
				return trackFGroup [trackId];
			} else {
				return null;
			}
		case (int)TrackType.typeG:
			if (trackId < trackGGroup.Length) {
				return trackGGroup [trackId];
			} else {
				return null;
			}
		case (int)TrackType.typeH:
			if (trackId < trackHGroup.Length) {
				return trackHGroup [trackId];
			} else {
				return null;
			}
		default:
			return null;
		}
	}

	public Transform GetTrackFromEnemyId(int enemyId)
	{
		int trackIndex = -1;

		if (enemyId >= 1 && enemyId <= 7) {
			thisTrackType = TrackType.typeA;
			for (int i = 0; i < trackAGroup.Length; i++) {
				if (trackAGroup[i].childCount==0) {
					trackIndex = i;
					break;
				} 
			}
		} else if (enemyId >= 8 && enemyId <= 14) {
			thisTrackType = TrackType.typeB;
			for (int i = 0; i < trackBGroup.Length ; i++) {
				if (trackBGroup[i].childCount==0) {
					trackIndex = i;
					break;
				}
			}
		} else if (enemyId >= 15 && enemyId <= 18) {
			thisTrackType = TrackType.typeC;
			for (int i = 0; i < trackCGroup.Length ; i++) {
				if (trackCGroup[i].childCount==0) {
					trackIndex = i;
					break;
				} 
			}
		} else if(enemyId >=19&&enemyId <=24){
			thisTrackType =TrackType.typeD;
			for(int i=0;i<trackDGroup.Length;i++){
				if(trackDGroup[i].childCount==0){
					trackIndex = i;
					break;
				}
			}

		}
		if (trackIndex == -1) {
			return null;
		} else{
			switch(thisTrackType)
			{
			case TrackType.typeA:
				return trackAGroup [trackIndex];
			case TrackType.typeB:
				return trackBGroup [trackIndex];
			case TrackType.typeC:
				return trackCGroup [trackIndex];
			case TrackType.typeD:
				return trackDGroup [trackIndex];
			default:
				return null;
			}
		}
	}

	Vector3[] trackDRot =new Vector3[]{ //轨迹D出现召唤特效时，每条轨迹对应的召唤旋转角
		new Vector3(0,135,0),new Vector3 (-5,135,-15) ,new Vector3 (10,180,20),new Vector3 (8,115,-20),
		new Vector3 (0,150,0),new Vector3 (0,350,5),new Vector3 (0,355,-35),new Vector3 (8,330,15),
		new Vector3 (0,305,5),new Vector3 (0,385,5),new Vector3 (0,390,10)};

	public Vector3 GetTrackDRotation(int trackID)
	{
		return trackDRot [trackID];
	}

	public Vector3 GetFormationGroupPos(int maxCount,int index) //根据maxCount找到对应的阵列，根据index找到阵列中id=index的鱼的位移补偿
	{
		for (int i = 0; i < formationGroup.Length; i++) {
			Formation formation = formationGroup [i].GetComponent<Formation> ();
			if (formation.maxCount == maxCount) {
				return formation.GetOffset (index);
			}
		}

        for (int i = 0; i < formationGroup.Length; i++) //如果找不到对应的数量，就从8这里获取
        {
            Formation formation = formationGroup[i].GetComponent<Formation>();
            if (formation.maxCount == 8)
            {
                return formation.GetOffset(index);
            }
        }

		Debug.LogError ("Error!Can't GetFormationGroup:" + maxCount + "," + index);
		return Vector3.zero;
	}

    public float GetFormationGroupScale(int maxCount,int index){
        for (int i = 0; i < formationGroup.Length; i++)
        {
            Formation formation = formationGroup[i].GetComponent<Formation>();
            if (formation.maxCount == maxCount)
            {
                return formation.GetScale(index);
            }
        }
        Debug.LogError("Error!Can't GetFormationGroup:" + maxCount + "," + index);
        return 1;
    }

    //[ContextMenu("GetTrackDuration")]
    public void GetTrackDuration()
    {       
        Debug.LogError("TrackA:");
        for (int i = 0; i < trackAGroup.Length; i++)
        {
            Debug.LogError(trackAGroup[i].GetComponent<MyTrack>().GetDurationTime() + ",");
            Tool.OutLogToFile(trackAGroup[i].GetComponent<MyTrack>().GetDurationTime() + ",");
        }
    }
}
