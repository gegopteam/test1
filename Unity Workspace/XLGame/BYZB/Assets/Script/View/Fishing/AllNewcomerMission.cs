using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class AllNewcomerMission : MonoBehaviour {


    public ScrollRect scrollRect;
    //public GameObject missionGridPrefab;

    public void InitPanel(int currentMissionIndex,int currentProgress)
    {
        
        NewcomerMissionGrid[] group = this.transform.GetComponentsInChildren<NewcomerMissionGrid>();

        scrollRect.verticalNormalizedPosition = 1 - (float)(currentMissionIndex-1) / (float)12;

        for (int i = 0; i < group.Length;i++){
            if(group[i].misssionIndex<currentMissionIndex){
                group[i].SetMissionState(2);
            }else if (group[i].misssionIndex ==currentMissionIndex){
                group[i].SetMissionState(1, currentProgress);
            }else if (group[i].misssionIndex>currentMissionIndex){
                group[i].SetMissionState(0);
            }
        }
    }

    public void Btn_ClosePanel(){
        Destroy(this.gameObject);
    }


}
