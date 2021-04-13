using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UpdateScript : MonoBehaviour
{

	public static UpdateScript Instance;
	public Text contentText;
	Button updateButton;
	Button laterButton;

	void Awake ()
	{
		Instance = this;
		//		contentText = transform.Find ("BG/CenterImage/Scroll View/Viewport/Content/ContentText").GetComponent <Text> ();
		contentText.text = UIUpdate.Instance.contentText;
		updateButton = transform.Find ("BG/BottomControl/UpdateNow").GetComponent <Button> ();
		laterButton = transform.Find ("BG/BottomControl/Later").GetComponent <Button> ();
	}

	// Use this for initialization
	void Start ()
	{
		if (UIUpdate.Instance.isLaterShow) {
			laterButton.gameObject.SetActive (true);
		} else {
			laterButton.gameObject.SetActive (false);
		}
		updateButton.onClick.RemoveAllListeners ();
		updateButton.onClick.AddListener (ClickUpdateButton);
		laterButton.onClick.RemoveAllListeners ();
		laterButton.onClick.AddListener (ClickLaterButton);
		//contentText.text = UIUpdate.Instance.contentText;
	}


	void ClickUpdateButton ()
	{
		Application.OpenURL (UIUpdate.Instance.urlStr);
	}

	void ClickLaterButton ()
	{
		DestroyImmediate (this.gameObject);
	}
}
