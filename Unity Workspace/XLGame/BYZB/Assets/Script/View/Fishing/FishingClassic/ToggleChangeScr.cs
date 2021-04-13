using UnityEngine;
using System.Collections;
using UnityEngine.UI;

//挂在在CommonImage1,2,3,4,5,6下面  因为public原因,所以每次更改的话要重新挂载一次,或者在Unity中修改
public class ToggleChangeScr : MonoBehaviour
{
	
	public static ToggleChangeScr Instance;


	#region 字段

	public Sprite[] torpedoGroup;

	[HideInInspector]
	public string[] extLotTextDescripeGroup = new string[] {
		"普通抽獎", "青銅抽獎", "白銀抽獎", "黃金抽獎", "白金抽獎", "至尊抽獎"
	};
	[HideInInspector]
	public  string[] torpedoDescripeGroup = new string[] {
		"迷你魚雷", "青銅魚雷", "白銀魚雷", "黃金魚雷", "白金魚雷", "核子魚雷", "鑽石", "金幣"
	};

	[HideInInspector]
	public string[] commonNumDescripeGroup = new string[] {
		"1", "10", "5", "2", "100", "50"
	};
	[HideInInspector]
	public string[] bronzeNumDescripeGroup = new string[] {
		"1", "20", "10", "4", "400", "200"
	};
	[HideInInspector]
	public string[] silverNumDescripeGroup = new string[] {
		"1", "100", "50", "20", "2000", "1000"
	};
	[HideInInspector]
	public string[] goldenNumDescripeGroup = new string[] {
		"1", "200", "100", "40", "4000", "2000"
	};
	[HideInInspector]
	public string[] platinumNumDescripeGroup = new string[] {
		"1", "400", "200", "80", "8000", "4000"
	};
	[HideInInspector]
	public string[] extremeNumDescripeGroup = new string[] {
		"1", "800", "400", "160", "16000", "8000"
	};
	[HideInInspector]
	public int toggleId = 0;

	#endregion

	void Awake ()
	{
		Instance = this;
	}

	
	void Update ()
	{
	
	}
}
