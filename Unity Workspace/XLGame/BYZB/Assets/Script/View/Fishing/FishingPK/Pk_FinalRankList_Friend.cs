using UnityEngine;
using System.Collections;


using AssemblyCSharp ;

public class Pk_FinalRankList_Friend : MonoBehaviour {

	public static Pk_FinalRankList_Friend _instance;
	public Pk_Friend_RankItem[] itemGroup;

	float sumDistance=0;
	float startXPos;

	void Awake () {
		if (null == _instance)
			_instance = this;
		//startXPos = itemGroup [0].GetRectPosX ();
		//sumDistance = itemGroup[itemGroup.Length-1].GetRectPosX()-startXPos ;
	}


	void SetShow(bool shouldShow)
	{
		if (shouldShow) {
			//this.transform.position = Vector3.zero;
			this.transform.parent = ScreenManager.uiScaler.transform;
			this.GetComponent<RectTransform> ().localScale = Vector3.one*1.14f;
			this.GetComponent<RectTransform> ().localPosition = Vector3.zero;
		} else {
			this.transform.position = Vector3.right * 1000f;
		}
	}

	public void ShowResult(FiFriendRoomGameResult result){
		SetShow (true);
      
		startXPos = itemGroup [0].GetRectPosX ();
		sumDistance = itemGroup[itemGroup.Length-1].GetRectPosX()-startXPos ;

        for (int i = 0; i < itemGroup.Length;i++){
            itemGroup[i].Init();
        }
		//给每个item项赋值
		for (int i = 0; i < result.users.Count; i++) {
			itemGroup [i].SetUserById (result.users [i].userId);

            if(result.users[i].userId==DataControl.GetInstance().GetMyInfo().userID && //金币模式下的好友房间对战完了需要加金币
               DataControl.GetInstance().GetRoomInfo().roomType== 11)
            {
                DataControl.GetInstance().GetMyInfo().gold += result.users[i].sum;
            }

			for (int j = 0; j < result.users [i].roundNum.Count; j++) {
				itemGroup[i].SetRoundScore(j,result.users[i].roundNum[j]);
				Debug.LogError ("id:"+result.users[i].userId + " round:" + j + " num:" + result.users [i].roundNum [j]);
			}
		}

		//对赋值后的item按照积分从大到小排序
		for (int i = 0; i < result.users.Count ; i++) {
		int maxIndex = i;
			for (int j = i + 1; j < result .users .Count ; j++) {
				if (itemGroup[maxIndex].GetSumScore() <itemGroup [j].GetSumScore()) {
					maxIndex = j;
				}
			}
			if (maxIndex!= i) {
				Pk_Friend_RankItem tempItem = itemGroup [i];
				itemGroup [i] = itemGroup [maxIndex];
				itemGroup [maxIndex] = tempItem;
			}
		}

		//排序后对item设置对应排名和位置
		for (int i = 0; i < result.users.Count; i++) {
			
			itemGroup [i].SetRank (i + 1);
			switch (result.users .Count) {
			case 4:
				itemGroup [i].SetRectPosX (startXPos +sumDistance*i/4 );
				break;
			case 3:
				itemGroup [i].SetRectPosX (startXPos + 0.5f * sumDistance+sumDistance* (i - 1) / 3);
				break;
			case 2:
				itemGroup [i].SetRectPosX (startXPos + (0.3f + i * 0.4f) * sumDistance);
				//Debug.LogError ("StartXPos:" + startXPos);
				//Debug.LogError ("i:" + i + " finalPos:" + (startXPos + (0.3f + i* 0.3f) * sumDistance).ToString ());
				break;
			default:
				Debug.LogError ("Error! Null rank");	
				break;
			}

			if (i == result.users.Count - 1) {
				itemGroup [i].HideDividingLine ();
			}
		}
    
		//隐藏没有被赋值的item项
		for (int i = result.users.Count; i < itemGroup.Length; i++) {
			itemGroup [i].Hide ();
      
		}

		for (int i = 0; i < result .users.Count; i++) {
			Debug.LogError ("UserID:"+result .users [i].userId);
            Debug.LogError("roundNum.Count="+result.users[i].roundNum.Count);

			for (int j = 0; j < result .users [i].roundNum.Count ; j++) {
				Debug.LogError ("sum[" + j + "]"+result .users [i].roundNum [j]);
			}
		}
	}

	public void Btn_Quit()
	{
		AudioManager._instance.PlayEffectClip (AudioManager.ui_click);
		AppControl.ToView (AppView.PKHALLMAIN);
		AudioManager._instance.PlayBgm (AudioManager.bgm_none);
	}

}
