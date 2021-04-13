using UnityEngine;
using System.Collections;
using DG.Tweening;
using AssemblyCSharp ;

public class PropHintPanel : MonoBehaviour {

	public Transform propBox1;
	public Transform propBox2;

	public Vector3 leftPos;
	public Vector3 rightPos;

	Transform skill1;
	Transform skill2;

	int moveIndex=1;

	// Use this for initialization
	void Awake () {
		moveIndex = 1;
        transform.parent = ScreenManager.uiScaler.transform;
        transform.localPosition = Vector3.zero;
        transform.localScale = Vector3.one * 45;
        leftPos = PrefabManager._instance.GetSkillUIByType(SkillType.Freeze).orginalPos;
        rightPos = PrefabManager._instance.GetSkillUIByType(SkillType.Lock).orginalPos;
        GetInfo();

	}
	
	

	public void ShowSkillIcon(int skillType, int restNum,int torpedoLevel=-1) //这里的skillType已经转成客户端的skillType了
	{
		Debug.LogError ("ShowSkillIcon:" + skillType);
		Skill tempSkill = PrefabManager._instance.GetSkillUIByType ((SkillType)skillType, torpedoLevel);
		if (tempSkill == null) {
			Debug.LogError ("Error!" + (SkillType)skillType +"TorpedoLevel:"+torpedoLevel +"=null");
		}
       
		tempSkill.SetPkInfoShow (restNum);
		tempSkill.SetClickable (false);
		switch (moveIndex) {
		case 1:
			skill1 = tempSkill.transform;
			skill1.position = propBox1.transform.position;
			moveIndex++;
			break;
		case 2:
			skill2 = tempSkill.transform;
			skill2.position = propBox2.transform.position;
			Invoke ("MoveSkillIcon", 3f);
			break;
		default:
			Debug.LogError ("Error!MoveIndex is out of range");
			break;
		}

	}

	public void MoveSkillIcon()
	{
		//skill1.DOMove (leftPos, 1f);
		//skill2.DOMove (rightPos, 1f);
		AudioManager._instance.PlayEffectClip (AudioManager.ui_click);
		skill1.GetComponent<Skill>().SetClickable(true);
		skill1.DOMove(leftPos,1f);
		skill2.DOMove (rightPos, 1f);
		skill2.GetComponent<Skill> ().SetClickable (true);
		Destroy(this.gameObject);
	}




	public void GetInfo()
	{
		if (GameController._instance.myGameType == GameType.Classical||GameController._instance.myGameType ==GameType.Point)
			return;
		FiDistributePKProperty distribute = UIFishingObjects.GetInstance ().tempDistribute;
        if (distribute == null){
            Debug.LogError("Error! distribute=null");
            return;
        }
			
		for (int i = 0; i < distribute.properties.Count; i++) {
			Debug.LogError ("Skill：" + distribute.properties [i].type + "Num:" + distribute.properties [i].value);
			int skillType = distribute.properties [i].type;
			//int torpedoLevel = Skill.GetTorpedoLevelFromServerId(skillType);//如果是鱼雷，则返回对应的鱼雷等级，如果不是，返回0
			skillType = Skill.ChangeSkillTypeToClient (skillType);
			int torpedoLevel = 7;
			ShowSkillIcon (skillType, distribute.properties [i].value, torpedoLevel);
		}
	
	}

	void TempTest()
	{
		ShowSkillIcon ((int)SkillType.Torpedo,4,7);
        ShowSkillIcon((int)SkillType.Freeze,2);
	}



}
