using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Text.RegularExpressions;
using AssemblyCSharp;
public class NoticeInputName : MonoBehaviour {
	public Text content;
	public Button sure;//右边的确定按钮
	public Button refuse;//左边的取消按钮
	public InputField inputname;
	public Text cost;
	public Image costicon;

	private int    userid;
	private int    logtype;
	private string newname;
	public delegate void  ChangeName(string name);
	private ChangeName mNameCallback; 
	// Use this for initialization
	void Start () {
		List<FiRewardStructure>  rewardinfo  = Facade.GetFacade ().message.reward.GetRewardInfo (RewardMsgHandle.MODIFY_NICK_TYPE);
//		Debug.LogError ("sss" + rewardinfo.Count);
//		for (int i = 0; i < rewardinfo.Count; i++) {
//			Debug.LogError ("----RewardType------" + rewardinfo [i].RewardType + "----rewardinfo[i].TaskID------" + rewardinfo [i].TaskID
//				+ "---rewardinfo[i].TaskValue----" + rewardinfo [i].TaskValue + "----rewardinfo[i].rewardPro.Count---------" + rewardinfo [i].rewardPro.Count);
//			for (int j = 0; j < rewardinfo [i].rewardPro.Count; j++) {
//				Debug.LogError ("rewardinfo [i].rewardPro" + rewardinfo [i].rewardPro [j].type + " rewardinfo [i].rewardPro [j].value" + rewardinfo [i].rewardPro [j].value);
//			}
//		}
		cost.text = rewardinfo [0].rewardPro [0].value.ToString();
		costicon.sprite = FiPropertyType.GetSprite (rewardinfo [0].rewardPro [0].type);
		sure.onClick.AddListener (SureButtonClick);
		refuse.onClick.AddListener (RefuseClick);
		UIColseManage.instance.ShowUI (this.gameObject);
	}
	
	// Update is called once per frame
	void Update () {
		
	}
	void RefuseClick(){
		AudioManager._instance.PlayEffectClip (AudioManager.ui_click);
		UIColseManage.instance.CloseUI ();
	}

	void SureButtonClick(){

		if (inputname.text.Length <= 0) {
			GameObject Window1 = UnityEngine.Resources.Load ("Window/WindowTipsThree")as UnityEngine.GameObject;
			GameObject WindowClone1 = UnityEngine.GameObject.Instantiate ( Window1 );
			UITipAutoNoMask ClickTips1 = WindowClone1.GetComponent<UITipAutoNoMask> ();
			ClickTips1.tipText.text = "修改的暱稱不能為空！";
			return;
		}

//		string nicTrim = Regex.Replace (inputname.text, @"\s", "");

		string nicTrim = inputname.text.Trim (new char[]{ ' ' });
		Debug.LogError ("nictrim" + nicTrim.Length);
		if (nicTrim.Length == 0) {
			GameObject Window1 = UnityEngine.Resources.Load ("Window/WindowTipsThree")as UnityEngine.GameObject;
			GameObject WindowClone1 = UnityEngine.GameObject.Instantiate ( Window1 );
			UITipAutoNoMask ClickTips1 = WindowClone1.GetComponent<UITipAutoNoMask> ();
			ClickTips1.tipText.text = "你輸入的是非法字符";
			return;
		}
		AudioManager._instance.PlayEffectClip (AudioManager.ui_click);
		MyInfo myInfo = (MyInfo)Facade.GetFacade ().data.Get (FacadeConfig.USERINFO_MODULE_ID);

		if (myInfo.diamond < int.Parse(cost.text) ) {
			GameObject Window1 = UnityEngine.Resources.Load ("Window/WindowTipsThree")as UnityEngine.GameObject;
			GameObject WindowClone1 = UnityEngine.GameObject.Instantiate ( Window1 );
			UITipAutoNoMask ClickTips1 = WindowClone1.GetComponent<UITipAutoNoMask> ();
			ClickTips1.tipText.text = "鑽石不足";
			return;
		}
		if (myInfo.isGuestLogin) {
			logtype = 1000;
		} else {
			int logtype = myInfo.loginType;
		}
		userid = myInfo.userID;

		newname = nicTrim;
		Facade.GetFacade ().message.broadcast.SendChangeNameRequset (newname,userid,logtype);
		UIColseManage.instance.CloseUI ();
	}
		
}
