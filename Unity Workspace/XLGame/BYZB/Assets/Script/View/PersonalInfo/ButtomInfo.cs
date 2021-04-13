using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using AssemblyCSharp;

public class ButtomInfo : MonoBehaviour {
	public Text exp;
	public Text gun;
	public Button record;
	// Use this for initialization
	void Start () {
		MyInfo nInfo =(MyInfo) Facade.GetFacade ().data.Get ( FacadeConfig.USERINFO_MODULE_ID );
		exp.text = nInfo.experience.ToString();
		gun.text = nInfo.cannonMultipleMax.ToString ();
	}

}
