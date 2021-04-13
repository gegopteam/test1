using UnityEngine;
using System.Collections;

public class NewcomerGuide : MonoBehaviour {
    
    public GameObject[] guideInfoGroup;

    public int currentIndex = 1;
    float perTimeDuration = 1.5f;

	// Use this for initialization
	void Start () {
        this.GetComponent<Canvas>().worldCamera=ScreenManager.uiCamera;
	    StartShow();

	}
	
	// Update is called once per frame
	void Update () {
	
	}

    void StartShow()
    {
        Invoke("ShowGuide", 1f);
    }

    void ShowGuide(){
        ShowGuideByIndex(currentIndex);
        currentIndex++;
        if (currentIndex < 4){
            Invoke("ShowGuide", perTimeDuration);
           
        }else{
            Invoke("RecoverNewcomerMissionLayer", 1f);
            Destroy(this.gameObject, 1f);
        }
            
    }

    void RecoverNewcomerMissionLayer(){
        if(NewcormerMissionPanel._instance!=null){
            NewcormerMissionPanel._instance.RecoverUILayer();
        }
    }

    void ShowGuideByIndex(int index){
        guideInfoGroup[index - 1].gameObject.SetActive(true); 
    }
}
