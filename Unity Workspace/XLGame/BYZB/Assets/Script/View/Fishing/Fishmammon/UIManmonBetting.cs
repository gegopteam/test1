using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using AssemblyCSharp;

public class UIManmonBetting : MonoSingleton<UIManmonBetting>
{

	//	private MyInfo myInfo;
	public Button[] BettingShowArray;
	public long gold = 0;
	public Sprite[] BettingShowSpriteArray;
	public List<long> BettingGoldShowArray = new List<long> ();
	public int nManmonStartNum;

	void Awake ()
	{

	}
	// Use this for initialization
	void Start ()
	{
		
//		myInfo = (MyInfo)Facade.GetFacade ().data.Get (FacadeConfig.USERINFO_MODULE_ID);
		InitManmonSetting ();
//		if (myInfo.gold < 500) {
//			for (int i = 0; i < BettingShowArray.Length; i++) {
//				//这是将按钮给置灰
//
//				BettingShowArray [i].gameObject.transform.GetComponent<Button> ().enabled = false;
//
//	
//			}
//		}

	}
	
	// Update is called once per frame
	void Update ()
	{
		
	}

	private void InitManmonSetting ()
	{
		Debug.LogError ("nenenenenenenneneen" + PrefabManager._instance.GetLocalGun ().currentGold);
		for (int i = 0; i < BettingShowArray.Length; i++) {
			if (i == 0) {
				long gold = 0;
				if (PrefabManager._instance.GetLocalGun ().currentGold < 1000) {
					gold = 50;
					BettingShowArray [i].gameObject.transform.GetComponent<Button> ().enabled = false;
					BettingShowArray [i].gameObject.transform.Find ("Label").GetComponent<Outline> ().effectColor = new Color (73 / 255f, 73 / 255f, 73 / 255f);
					BettingShowArray [i].gameObject.transform.GetComponent<Image> ().sprite = BettingShowSpriteArray [1];
				} else {
					gold = BettingGoldShowArray [i];

				}
				Debug.LogError ("gold" + gold);
				SetGold (BettingShowArray [i].gameObject.transform.Find ("Label").GetComponent<Text> (), gold);
				BettingShowArray [i].gameObject.transform.GetComponent<Button> ().onClick.AddListener (delegate() {
					SendMoneySetting (1);
				});
			} else if (i == 1) {
				long gold = 0;
				if (PrefabManager._instance.GetLocalGun ().currentGold < 1000) {
					gold = 100;
					BettingShowArray [i].gameObject.transform.GetComponent<Button> ().enabled = false;
					BettingShowArray [i].gameObject.transform.Find ("Label").GetComponent<Outline> ().effectColor = new Color (73 / 255f, 73 / 255f, 73 / 255f);
					BettingShowArray [i].gameObject.transform.GetComponent<Image> ().sprite = BettingShowSpriteArray [1];
				} else {
					gold = BettingGoldShowArray [i];

				}
				Debug.LogError ("goldz" + gold);	
				SetGold (BettingShowArray [i].gameObject.transform.Find ("Label").GetComponent<Text> (), gold);
				BettingShowArray [i].gameObject.transform.GetComponent<Button> ().onClick.AddListener (delegate() {
					SendMoneySetting (2);
				});
			} else if (i == 2) {
				long gold = 0;
				if (PrefabManager._instance.GetLocalGun ().currentGold < 1000) {
					gold = 200;
					BettingShowArray [i].gameObject.transform.GetComponent<Button> ().enabled = false;
					BettingShowArray [i].gameObject.transform.Find ("Label").GetComponent<Outline> ().effectColor = new Color (73 / 255f, 73 / 255f, 73 / 255f);
					BettingShowArray [i].gameObject.transform.GetComponent<Image> ().sprite = BettingShowSpriteArray [1];
				} else {
					gold = BettingGoldShowArray [i];
				}
				Debug.LogError ("goldks" + gold);	
				SetGold (BettingShowArray [i].gameObject.transform.Find ("Label").GetComponent<Text> (), gold);
				BettingShowArray [i].gameObject.transform.GetComponent<Button> ().onClick.AddListener (delegate() {
					SendMoneySetting (3);
				});
			}

		}
	}

	public void SendMoneySetting (int  type)
	{
		Debug.LogError ("________gold____" + type);

		Facade.GetFacade ().message.fishCommom.SendManmonBettingType (type);
		Facade.GetFacade ().message.fishCommom.SendManmonWinriemcout ();
		Destroy (this.gameObject);
		//这里其实只做发协议和播放动画的操作 我这打开预设是为了测试流程
//		string path = "Window/ManmonEffect";
//		GameObject WindowClone = AppControl.OpenWindow (path);
//		WindowClone.gameObject.SetActive (true);
//		Debug.LogError ("UIManmmonShow.instance.manmonlist [0].Startnum" + UIManmmonShow.instance.nManmonNum);
//		UIManmmonShow.instance.nManmonNum -= 1;
//		UIManmmonShow.instance.Refreshstatrnum (UIManmmonShow.instance.nManmonNum);
	}

	public void OnCloseBtn ()
	{
		Destroy (this.gameObject);
	}

	public void SetGold (Text TxtGold, long nCount)
	{
		if (nCount > 100000000) {

			TxtGold.text = "" + (float)(nCount / 1000000) / 100 + "億";

		} else if (nCount >= 10000) {
			TxtGold.text = "" + (int)(nCount / 10000) + "萬";
		} else {
			TxtGold.text = "" + nCount;
		}
	}
}
