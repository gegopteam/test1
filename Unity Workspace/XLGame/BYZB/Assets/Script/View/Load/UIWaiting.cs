using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class UIWaiting : MonoBehaviour {

	public Image Backgroud;


	float mDuration = 30.0f;
	string nTipInfo = "請求超時";
	// Use this for initialization
	void Start () {
		Invoke ( "CloseWaitingView" , mDuration );
	}

	public void HideBackGround()
	{
		Backgroud.gameObject.SetActive ( false );
	}

	public void SetInfo( float nDuration , string nTipDetail )
	{
		mDuration = nDuration;
        nTipInfo = nTipDetail;
	}

	void CloseWaitingView()
	{
		Destroy ( gameObject );
		GameObject Window1 = UnityEngine.Resources.Load ("Window/WindowTipsThree")as UnityEngine.GameObject;
		GameObject WindowClone1 = UnityEngine.GameObject.Instantiate ( Window1 );
		UITipAutoNoMask ClickTips1 = WindowClone1.GetComponent<UITipAutoNoMask> ();
		ClickTips1.tipText.text = nTipInfo;
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
