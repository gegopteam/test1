using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;

using AssemblyCSharp ;

public class Pk_Point_FinalRanklist : MonoBehaviour {

	public static Pk_Point_FinalRanklist _instance;

	public Pk_Bullet_FinalRankItem[] itemGroup;

	public SpriteRenderer winLoseDrawSprite;

	public SpriteRenderer[] winSpriteRendererGroup;
	public Sprite[] loseSpriteGroup;
	public Sprite[] drawSpriteGroup;

	public Sprite[] topBarGroup;

	int resultIndex=-1;
	// Use this for initialization
	void Start () {
		if (null == _instance)
			_instance = this;
		//SetTopResult (1);
	}
	
	public void ShowResult(List<FiPlayerInfo> infoList)
	{
		SetShow (true);

        //限制最大最小值
        //	for (int i = 0; i < infoList.Count; i++) {
        //		if (infoList [i].point < 0) {
        //			infoList [i].point = 0;
        //		}
        //		if (infoList [i].point > (PkPointProgessBar._instance.GetSumPoint ()))
        //			infoList [i].point = PkPointProgessBar._instance.GetSumPoint ();
        //	}
     
		//给infoList按从大到小排序
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

		for (int i = 0; i < 2; i++) { //强制让UI同步赋值
			if (i < infoList.Count) {
				GunInUI tempGunUI = UIFishingObjects.GetInstance ().cannonManage.GetInfo (infoList [i].userId).cannon.gunUI;
                if(tempGunUI.isLocal){
                    DataControl.GetInstance().GetMyInfo().gold += infoList[i].gold;
                }
				string nickName = tempGunUI.GetNickName ();
				itemGroup [i].SetInfo (infoList [i].point, infoList [i].gold, nickName,tempGunUI.gunControl.GetAvatorSprite());
				tempGunUI .SetScoreText(infoList[i].point,true,GameController._instance.gameIsEnd );

				if (tempGunUI.isLocal) {
					if (i == 0) {
						if (infoList [i].point > infoList [i + 1].point) {
							resultIndex = 0;
						} else if (infoList [i].point < infoList [i + 1].point) {
							resultIndex = 1;
						} else if (infoList [i].point == infoList [i + 1].point) {
							resultIndex = 2;
						}
					}
					if(i==1){
						if (infoList [i].point > infoList [i - 1].point) {
							resultIndex = 0;
						} else if (infoList [i].point < infoList [i - 1].point) {
							resultIndex = 1;
						} else if (infoList [i].point == infoList [i - 1].point) {
							resultIndex = 2;
						}
					}
				}

			} else {
				itemGroup [i].Hide ();
			}
		}
		SetTopResult (resultIndex);

	}

	void SetShow(bool toShow)
	{
		if (toShow) {
			//this.transform.position = Vector3.zero;
			this.transform.parent = ScreenManager.uiScaler.transform;
			this.GetComponent<RectTransform> ().localScale = Vector3.one*0.47f;
			this.GetComponent<RectTransform> ().localPosition = Vector3.zero;
			this.GetComponent<Animator> ().enabled = true;
		} else {
			this.transform.position = Vector3.right * 1000f;
		}
	}

	public void SetTopResult(int resultIndex) //0胜利 1失败 2平局
	{
		Debug.LogError ("ResultIndex:" + resultIndex);
		//winLoseDrawSprite.sprite = winSpriteGroup [resultIndex];
	//	topBar.sprite = topBarGroup [resultIndex]; //美术改了动画，这行代码不能用了 
		switch (resultIndex) {
		case 0:
			AudioManager._instance.PlayEffectClip (AudioManager.effect_pkVictory);
			break;
		case 1:
			for (int i = 0; i < winSpriteRendererGroup.Length; i++) {
				winSpriteRendererGroup [i].sprite = loseSpriteGroup [i];
			}
			transform.Find ("_Effect_UI_Win").gameObject.SetActive (false);
			transform.Find ("TopBar_glow").gameObject.SetActive (false);
			AudioManager._instance.PlayEffectClip (AudioManager.effect_pkFail);
			break;
		case 2:
			for (int i = 0; i < winSpriteRendererGroup.Length; i++) {
				winSpriteRendererGroup [i].sprite = drawSpriteGroup [i];
			}
			transform.Find ("_Effect_UI_Win").gameObject.SetActive (false);
			transform.Find ("TopBar_glow").gameObject.SetActive (false);
			AudioManager._instance.PlayEffectClip (AudioManager.effect_pkVictory);
			break;
		default:
			Debug.LogError ("Error result:" + resultIndex.ToString ());
			break;
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
