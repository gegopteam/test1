using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using AssemblyCSharp;

public class FishingForHegemony : MonoBehaviour {

    void Awake()
    {
        if (GameController._instance.isExperienceMode||GameController._instance.isBossMatchMode)
        {
            this.gameObject.SetActive(false);
            return;
        }
    }
    void Start () {
       
        Facade.GetFacade().message.fishCommom.SendGetFishingRankInfo();
        StartCoroutine(GetRefershRank());
	}
    //五分钟刷新一次
    IEnumerator GetRefershRank()
    {
        yield return new WaitForSeconds(300);
        Facade.GetFacade().message.fishCommom.SendGetFishingRankInfo();
        StartCoroutine(GetRefershRank());
    }

	void Update () {
		
	}
    bool openAndClose = true;
    public void Openeward()
    {
        transform.Find("reward").gameObject.SetActive(openAndClose);
        openAndClose = !openAndClose;
    }
}
