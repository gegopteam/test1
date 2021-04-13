using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using DG.Tweening;
using AssemblyCSharp ;

public class LevelUpPanel : MonoBehaviour {

	public Text newLevelText;

	public GameObject rewardObj1;
	public GameObject rewardObj2;
	public GameObject rewardObj3;

	Text numText1;
	Text numText2;
	Text numText3;

	int tempNum1;
	int tempNum2;
	int tempNum3;

    bool hasClick = false;

    private void Start()
    {
       // AudioManager._instance.PlayEffectClip(AudioManager.effect_levelUp, 2f);
        Invoke("Btn_GetReward", 5f);
    }

    public void SetInfo(int newLevel ,int diamondNum,int lockNum,int freezeNum){
        AudioManager._instance.PlayEffectClip(AudioManager.effect_levelUp, 2f);
		numText1 = rewardObj1.transform.Find ("NumText").GetComponent<Text> ();
		numText2 = rewardObj2.transform.Find ("NumText").GetComponent<Text> ();
		numText3 = rewardObj3.transform.Find ("NumText").GetComponent<Text> ();

		newLevelText.text = newLevel.ToString ();

		numText1.text = diamondNum.ToString ();
		tempNum1 = diamondNum;

		numText2.text = lockNum.ToString ();
		tempNum2 = lockNum;

		numText3.text = freezeNum.ToString ();
		tempNum3 = freezeNum;
	}

	public void Btn_GetReward(){
        if (hasClick)
            return;
        hasClick = true;
     
        AudioManager._instance.PlayEffectClip(AudioManager.effect_getReward);

		Transform canvasTrans = GameObject.FindGameObjectWithTag (TagManager.uiCanvas).transform;
		rewardObj1.transform.parent = canvasTrans;
		rewardObj2.transform.parent = canvasTrans;
		rewardObj3.transform.parent = canvasTrans;

		transform.position = Vector3.up * 1000f;

	//	Vector3 pos1 = PrefabManager._instance.GetLocalGun ().gunUI.diamondImage.position;
		Vector3 pos1 = PrefabManager._instance.GetLocalGun ().transform.position;
		Vector3 pos2 = PrefabManager._instance.GetSkillUIByType (SkillType.Lock).transform.position;
		Vector3 pos3 = PrefabManager._instance.GetSkillUIByType (SkillType.Freeze).transform.position;

		rewardObj1.transform.DOMove (pos1, 1f);
		rewardObj2.transform.DOMove (pos1, 1f);
		rewardObj3.transform.DOMove (pos1, 1f);

		Invoke ("ChangeValue", 0.6f);
	}

	void ChangeValue(){
		PrefabManager._instance.GetLocalGun ().gunUI.AddValue (0, 0, tempNum1);
		PrefabManager._instance.GetSkillUIByType (SkillType.Lock).AddRestNum (tempNum2);
		PrefabManager._instance.GetSkillUIByType (SkillType.Freeze).AddRestNum (tempNum3);

		Destroy (rewardObj1);
		Destroy (rewardObj2);
		Destroy (rewardObj3);

		Destroy (this.gameObject,10f);
	}

}
