using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class UITipClickHideManager : MonoBehaviour {

	public delegate void OnButtonClick();

	OnButtonClick nClickHandle;

	public Text text;
	public Text time;
	public Text tipString;

    public static bool isClose;

	private int currentTime;
	private float timeAdd;

	private bool bClickCloseMode = false;

	public void SetClickClose()
	{
		bClickCloseMode = true;
	}

	public void SetClickCallback( OnButtonClick nValue )
	{
		SetClickClose ();
		nClickHandle = nValue;
	}

	// Use this for initialization
	void Start () {
        isClose = false;

		timeAdd = 0f;
		if (time.text != "") {
			currentTime = int.Parse(time.text);
		}
	}



	// Update is called once per frame
	void Update () {
		if (bClickCloseMode) {
			time.gameObject.SetActive ( false );
			tipString.gameObject.SetActive ( false );
			return;
		}

		timeAdd += Time.deltaTime;
		if (currentTime > 0) {
			if (timeAdd >= 1f) {
				currentTime -= 1;
				time.text = currentTime.ToString ();
				timeAdd = 0f;
			} 
		} else {
			Destroy (this.gameObject);
            if (isClose)
            {
                UIReward.creatWarningWindow();
                return;
            }
		}

	}

	public void SureButton()
	{
		if (nClickHandle != null) {
			nClickHandle.Invoke ();
		}
        if(isClose)
        {
            UIReward.creatWarningWindow();
        }
		Destroy (this.gameObject);
	}
}
