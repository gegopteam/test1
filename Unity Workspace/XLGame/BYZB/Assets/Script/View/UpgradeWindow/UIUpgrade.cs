using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using AssemblyCSharp;

public class UIUpgrade : MonoBehaviour {
	public delegate void HideDelegate();
	public static event HideDelegate HideEvent;
	public VIPInfo vipInfo;

	private GameObject WindowClone;

	// Use this for initialization
	void Start () {
		
	}
	public void SetVipInfo(int level){
		vipInfo.InitalInfo (level);
	}
	public void ExitButton()
	{
		DestroyImmediate (gameObject);
		/*string path = "Window/VIP";
		WindowClone = AppControl.OpenWindow (path);
		WindowClone.SetActive (true);
		if (HideEvent != null) {
			HideEvent ();
		}*/
	}
	public void SetGun(){
		Facade.GetFacade ().message.fishCommom.SendChangeCannonStyleRequest ( 3000+vipInfo.Level , DataControl.GetInstance().GetMyInfo().userID );
		ExitButton ();
	}
}
