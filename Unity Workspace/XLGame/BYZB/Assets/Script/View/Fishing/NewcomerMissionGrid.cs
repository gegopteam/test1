using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class NewcomerMissionGrid : MonoBehaviour
{

	public Sprite[] rewardGroup;
	//0钻石 1红包券 2金币
	public Sprite[] missionStateGroup;
	//0未开启 1进行中 2已完成
	public Sprite[] missionIconGroup;

	Text progressText;
	Text rewardNumText;
	Image missionState;
	Image progressBar;
	Image missionIcon;
	Image rewardTypeIcon;
	public int misssionIndex;
	int currentCount;
	int targetCount;


	void Init ()
	{
		targetCount = NewcormerMissionPanel.missionTargetCountGroup [misssionIndex];
		transform.Find ("missionDescription").GetComponent<Text> ().text = NewcormerMissionPanel.missionDescripeGroup [misssionIndex];
		progressText = transform.Find ("ProgressBar/ProgresssText").GetComponent<Text> ();

		rewardNumText = transform.Find ("RewardTypeImage/RewardNum").GetComponent<Text> ();
		rewardTypeIcon = transform.Find ("RewardTypeImage").GetComponent<Image> ();

		rewardNumText.text = "x" + NewcormerMissionPanel.missionRewardCountGroup [misssionIndex];

		missionState = transform.Find ("MissionState").GetComponent<Image> ();
		progressBar = transform.Find ("ProgressBar/image").GetComponent<Image> ();
		missionIcon = transform.Find ("MissionIcon").GetComponent<Image> ();

		switch (misssionIndex) {
		case 5: //bonuseFish
			missionIcon.transform.localScale *= 1.25f;
			break;
		case 6://bonusFish
			missionIcon.transform.localScale *= 1.25f;
			break;
//		case 7: //unlcok
//			missionIcon.transform.localScale *= 1.3f;
//			break;
//		case 9://unlcok
//			missionIcon.transform.localScale *= 1.3f;
//			break;
//      
//		case 11://unlcok
//			missionIcon.transform.localScale *= 1.3f;
//			break;
		case 9://boss
			missionIcon.transform.localScale *= 1.4f;
			break;
		}
		switch (misssionIndex) {
//		case 3:
//		case 4:
//		case 7:
//		case 8:
//			rewardTypeIcon.sprite = rewardGroup [0];
//			break;
		default:
			rewardTypeIcon.sprite = rewardGroup [2];
			break;
		}
	}


	public void SetMissionState (int stateIndex, int missionProgress = -1)
	{ // 0=unOpen,1=inProgress,2=complete
		Init ();

		missionState.sprite = missionStateGroup [stateIndex];
		missionIcon.sprite = missionIconGroup [misssionIndex - 1];
       

		switch (stateIndex) {
		case 0:
			currentCount = 0;
			missionState.GetComponent<RectTransform> ().localScale *= 1.4f;
			SetMaskShow (false);
			break;
		case 1:
			currentCount = missionProgress;
			SetMaskShow (false);
			break;
		case 2:
			currentCount = targetCount;
			SetMaskShow (true); 
			break;
		}
		progressText.text = currentCount + "/" + targetCount;
		progressBar.fillAmount = (float)currentCount / (float)targetCount;

		//  this.GetComponentInChildren<ScrollRect>().normalizedPosition = new Vector2(0.5f, 0.5f);
		//this.GetComponentInChildren<ScrollRect>().verticalNormalizedPosition = 0.5f;
         

	}

	void SetMaskShow (bool toShow)
	{
		transform.Find ("Mask").GetComponent<Image> ().enabled = toShow;
	}

   

	void SetInfo (int currentCount, int targetCount)
	{
        
	}
}
