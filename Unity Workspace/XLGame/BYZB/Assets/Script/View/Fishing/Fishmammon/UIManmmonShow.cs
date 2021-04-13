using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using AssemblyCSharp;

//public class ManmonDataInfo
//{
//	public FiFishRoomShow showinfo;
//}

public class UIManmmonShow : MonoSingleton<UIManmmonShow>
{

	public Image[] ShowManmonStart;
	public Sprite[] ShowManstartSprite;
	public List<FiFishRoomShow> manmonlist = new List<FiFishRoomShow> ();
	public int nManmonNum;
	private Button ManmonOpenShow;
	public GameObject ManmonEffectObj;
    public bool IsFirstShow = true;//是否是主动申请
	MyInfo myinfo;
	// Use this for initialization
	public GameObject showManEffect;

	void Awake ()
	{
		ManmonOpenShow = transform.Find ("ManmonOpenButton").GetComponent<Button> ();
		ManmonOpenShow.onClick.AddListener (OpenManmomnButton);

	}

	void Start ()
	{
		MyInfo myinfo = DataControl.GetInstance ().GetMyInfo ();
//		Debug.LogError ("myinfogold" + myinfo.gold + "myinfonmanmonnun" + myinfo.nManmon);
		nManmonNum = myinfo.nManmon;
		Refreshstatrnum (nManmonNum);
	}
	
	// Update is called once per frame
	void Update ()
	{
		
	}


	public void OpenManmomnButton ()
	{
//		PrefabManager._instance.GetLocalGun ().CompleteLockSkill ();
//		PrefabManager._instance.GetLocalGun ().UserCanCelSkill (SkillType.Replication, 0f);
		PrefabManager._instance.GetSkillUIByType (SkillType.Lock).cancleAutoFire ();

		PrefabManager._instance.GetSkillUIByType (SkillType.Replication).cancleAutoFire ();


		//		GunControl..UserCanCelSkill ();
		//		PrefabManager._instance.GetSkillUIByType (SkillType.Lock).SetLocalGunUseSkill (false);
		GunControl gunUi = PrefabManager._instance.GetLocalGun ();
		gunUi.CancleAuto ();
		nManmonNum = DataControl.GetInstance ().GetMyInfo ().nManmon;
		Debug.LogError ("nmanmon" + nManmonNum);
		if (nManmonNum >= 1) {
			Facade.GetFacade ().message.fishCommom.SendManmonsettingRequest ();
            UIManmmonShow.instance.IsFirstShow = false;
		} else {
			
			string path = "Window/ManmonTps3";
			GameObject WindowClone = AppControl.OpenWindow (path);
			WindowClone.gameObject.SetActive (true);
			if (GameController._instance.isBossMode) {
				Debug.LogError ("22222222222222");
				WindowClone.gameObject.GetComponent<ManmonTps3> ().textnum.text = "击杀1条任意鱼";
			} else {
				WindowClone.gameObject.GetComponent<ManmonTps3> ().textnum.text = "击杀10条任意鱼";
			}

		}

		
	}

	public void Refreshstatrnum (int count)
	{
//		Debug.LogError ("count" + count);
		if (count >= 6) {
			showManEffect.gameObject.SetActive (true);
		} else {
			showManEffect.gameObject.SetActive (false);
		}

		if (count == 0) {
            IsFirstShow = true;
			for (int i = 0; i < ShowManmonStart.Length; i++) {
				ShowManmonStart [i].sprite = ShowManstartSprite [0];
			}
			return;
		}

		for (int i = 0; i < ShowManmonStart.Length; i++) {
			ShowManmonStart [i].sprite = ShowManstartSprite [0];
		}
		for (int i = 0; i < count; i++) {
			ShowManmonStart [i].sprite = ShowManstartSprite [1];
		}
	}


}
