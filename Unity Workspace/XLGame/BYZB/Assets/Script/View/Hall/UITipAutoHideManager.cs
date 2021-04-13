using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using AssemblyCSharp;

public class UITipAutoHideManager : MonoBehaviour {

	public Text text;
	private GameObject WindowClone;
	public int useId;
	// Use this for initialization
	void Start () {
		UIColseManage.instance.ShowUI (this.gameObject);
	}

	// Update is called once per frame
	public void SureButton()
	{
		//Debug.LogError ( "-----------SendDeleteFriend--------------" + useId );
		/*GameObject Window = Resources.Load ("Window/WindowTips")as GameObject;
		WindowClone = GameObject.Instantiate (Window);
		UITipClickHideManager ClickTips = WindowClone.GetComponent<UITipClickHideManager> ();
		ClickTips.time.text = "2";
		ClickTips.text.text = "好友删除成功!";*/

		Facade.GetFacade ().message.friend.SendDeleteFriend ( useId );
		UIColseManage.instance.CloseUI ();
	}

	public void CancelButton()
	{
		UIColseManage.instance.CloseUI ();
	}
}
