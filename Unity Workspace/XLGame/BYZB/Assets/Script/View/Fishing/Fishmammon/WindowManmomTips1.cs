using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using AssemblyCSharp;

public class WindowManmomTips1 : MonoBehaviour
{
	public Text rewardnumText;
	public Text time;
	public Text tipString;
	private float timeadd = 0f;
	private float CurrentTime = 0f;
	// Use this for initialization
	public long Curredgold;
	public DragonCardInfo mDragonCardInfo;
	// Use this for initialization
	void Start ()
	{
		mDragonCardInfo = (DragonCardInfo)Facade.GetFacade ().data.Get (FacadeConfig.UI_DRAGONCARD);
//		UIManmonGameShow.instance.ShowEffect (ShwoEffect.JINBIDIAOLUO);
		if (time.text != "") {
			CurrentTime = int.Parse (time.text);
		}
	}

	public void ShowMoney (long money)
	{
		rewardnumText.text = money.ToString ();
	}
	// Update is called once per frame
	void Update ()
	{
		timeadd += Time.deltaTime;	
		if (CurrentTime > 0) {
			if (timeadd >= 1) {
				CurrentTime -= 1;
				time.text = CurrentTime.ToString ();
				timeadd = 0f;
			}
		} else {
//			Debug.Log ("mdddddddddddd" + mDragonCardInfo.CurrendGold);
			PrefabManager._instance.GetLocalGun ().gunUI.RefrshCoin (0, mDragonCardInfo.CurrendGold);
			Destroy (this.gameObject);
			UIManmonGameShow.instance.Btnclose ();
		}

	}

	public void Colcse ()
	{
		Destroy (this.gameObject);
		UIManmonGameShow.instance.Btnclose ();
	}
}
