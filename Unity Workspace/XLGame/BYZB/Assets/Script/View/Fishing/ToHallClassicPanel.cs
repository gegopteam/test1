using UnityEngine;
using System.Collections;
//using UnityEngine.SceneManagement;

public class ToHallClassicPanel : MonoBehaviour {

	public void Btn_ToHallClassic(){
		AudioManager._instance.PlayEffectClip (AudioManager.ui_click);
		AudioManager._instance.PlayBgm (AudioManager.bgm_none);

		//经典场操作
	
		DataControl.GetInstance ().GetMyInfo ().gold = PrefabManager._instance.GetLocalGun ().currentGold;
		DataControl.GetInstance ().GetMyInfo ().diamond = PrefabManager._instance.GetLocalGun ().curretnDiamond;
		UIFishingMsg.GetInstance ().SndLeaveRoom ();

        this.GetComponent<Canvas>().enabled = false;

        //---------
        //AppControl.SetView(AppView.FISHING);
        //AppControl.ToView(AppView.LOADING );
        //return;
        //---------

        DontDestroyOnLoad(this.gameObject);
        //DataControl.GetInstance().GetMyInfo().SetState(MyInfo.STATE_IN_HALL);
		AppControl.ToView (AppView.CLASSICHALL);
        Invoke("DelayStartGame", 2f); //有bug，暂时不执行
	}


    void DelayStartGame(){
        if(UIHallObjects.GetInstance()!=null){
            UIHallObjects.GetInstance().PlayFieldGrade_2();
        }else{
            Debug.LogError("Error! UIHallObjects.GetInstance()=null");
        }
        Destroy(this.gameObject);
    }
}
