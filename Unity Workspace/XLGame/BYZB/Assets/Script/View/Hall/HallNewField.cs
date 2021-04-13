using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using AssemblyCSharp;
using UnityEngine.UI;
public class HallNewField : MonoBehaviour
{
	// Use this for initialization
	void Start ()
	{
        //iphonex  适配
        if (Facade.GetFacade().config.isIphoneX2())
        {
            gameObject.GetComponent<GridLayoutGroup>().spacing = new Vector2(188f,0f);
        }
	}
	
	// Update is called once per frame
	void Update ()
	{
		
	}

	public void OnButtonClick (GameObject nObject)
	{

		MyInfo nInfo = (MyInfo)Facade.GetFacade ().data.Get (FacadeConfig.USERINFO_MODULE_ID);
		int nMaxMutiple = nInfo.cannonMultipleMax;
		if (nObject.name.Equals ("yujuanchang")) {//鱼
//			UIHallObjects.GetInstance ().PlayFieldGrade_1 ();	
		} else if (nObject.name.Equals ("BoosOpen")) {//boss场
			UIHallObjects.GetInstance ().PlayHallnewFieldGrade_0 ();
		} else if (nObject.name.Equals ("baoji")) {
//			UIHallObjects.GetInstance ().PlayFieldGrade_3 ();
		} else if (nObject.name.Equals ("Expect")) {
//			UIHallObjects.GetInstance ().PlayFieldGrade_4 ();
		}
		//}
	}

}
