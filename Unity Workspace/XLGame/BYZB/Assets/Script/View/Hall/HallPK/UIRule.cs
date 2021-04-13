using UnityEngine;
using System.Collections;
using AssemblyCSharp;
/// <summary>
/// User interface rule.所有的规则窗口
/// </summary>
public class UIRule : MonoBehaviour {
	public GameObject bulletWindow;
	public GameObject timeWindow;
	public GameObject combatWindow;

	public void SetRuleType( int nType )
	{
		switch( nType )
		{
		case PKRuleType.ROOM_BULLET:
			timeWindow.SetActive (false);
			combatWindow.SetActive (false);
			bulletWindow.SetActive (true);
			break;
		case PKRuleType.ROOM_TIME:
			bulletWindow.SetActive (false);
			combatWindow.SetActive (false);
			timeWindow.SetActive (true);
			break;
		case PKRuleType.ROOM_COMBAT:
			timeWindow.SetActive (false);
			bulletWindow.SetActive (false);
			combatWindow.SetActive (true);
			break;
		}
	}

	void Start () {

	}

	public void ExitButton()
	{
		Destroy ( gameObject );
		//transform.gameObject.SetActive (false);
	}
}
