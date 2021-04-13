using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;

using AssemblyCSharp;

public class PK_Bullet_FinalRanklist : MonoBehaviour {

	//public static PK_Bullet_FinalRanklist _instance;

	public Pk_Bullet_FinalRankItem[] itemGroup;

	// Use this for initialization
	void Start () {
		//if (null == _instance)
			//_instance = this;
	}

	public void ShowResult(List<FiPlayerInfo> infoList)
	{
		SetShow (true);

		bool sortByScore = false; //决定根据积分排序或者根据金币排序

		if (GameController._instance.myGameType == GameType.Point)
			sortByScore = true;
		
		if (sortByScore) {
			for (int i = 0; i < infoList.Count; i++) {
				if (infoList [i].point < 0) {
					infoList [i].point = 0;
				}
				if (infoList [i].point > (PkPointProgessBar._instance.GetSumPoint ()))
					infoList [i].point = PkPointProgessBar._instance.GetSumPoint ();
			}
			for (int i = 0; i < infoList.Count-1; i++) {
				int maxIndex = i;
				for (int j = i + 1; j < infoList.Count; j++) {
					if (infoList [maxIndex].point <infoList [j].point) {
						maxIndex = j;
					}
				}
				if (maxIndex!= i) {
					FiPlayerInfo tempInfo = infoList [i];
					infoList [i] = infoList [maxIndex];
					infoList [maxIndex] = tempInfo;
				}
			}
		} else {
			//按金币从大到小排序
			for (int i = 0; i < infoList.Count-1; i++) {
				int maxIndex = i;
				for (int j = i + 1; j < infoList.Count; j++) {
					if (infoList [maxIndex].gold <infoList [j].gold) {
						maxIndex = j;
					}
				}
				if (maxIndex!= i) {
					FiPlayerInfo tempInfo = infoList [i];
					infoList [i] = infoList [maxIndex];
					infoList [maxIndex] = tempInfo;
				}
			}
		}

		for (int i = 0; i < 4; i++) { //强制让UI同步赋值
			if (i < infoList.Count) {
				GunInUI tempGunUI = UIFishingObjects.GetInstance ().cannonManage.GetInfo (infoList [i].userId).cannon.gunUI;
                if(tempGunUI.isLocal){
                    DataControl.GetInstance().GetMyInfo().gold += infoList[i].gold;
                }
				string nickName = tempGunUI.GetNickName ();
				itemGroup [i].SetInfo (infoList [i].point, infoList [i].gold, nickName,tempGunUI.gunControl.GetAvatorSprite());

				tempGunUI .SetScoreText(infoList[i].point,true,GameController._instance.gameIsEnd );
			} else {
				itemGroup [i].Hide ();
			}
		}

		AudioManager._instance.PlayEffectClip (AudioManager.effect_pkVictory);
	}

	void SetShow(bool toShow)
	{
		if (toShow) {
            this.transform.SetParent(GameObject.FindWithTag((TagManager.uiCanvas)).transform);
			this.GetComponent<RectTransform> ().localScale = Vector3.one;
			this.GetComponent<RectTransform> ().localPosition = Vector3.zero;
		} else {
			this.transform.position = Vector3.right * 1000f;
		}
	}

	public  void BtnPlayAgain_Click()
	{
		AudioManager._instance.PlayEffectClip (AudioManager.ui_click);
		UIFishingMsg.GetInstance ().OneMoreGame ();
		AudioManager._instance.PlayBgm (AudioManager.bgm_none);
	}
	public void BtnQuit_Click()
	{
		AudioManager._instance.PlayEffectClip (AudioManager.ui_click);
		AppControl.ToView (AppView.PKHALLMAIN);
		//AudioManager._instance.PlayBgm (AudioManager.bgm_none);
	}

}
