using UnityEngine;
using System.Collections;

public class UIAccoutWIndow : MonoBehaviour {
	private AppControl appControl = null;
	public GameObject yes;
	// Use this for initialization
	void Start () {
		appControl = AppControl.GetInstance ();
		yes.SetActive (false);
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	public  void AccoutLogin()
	{
		AppControl.ToView (AppView.HALL);
	}

	public void YesButton()
	{
		yes.SetActive (true);
	}

	public void CloseButton()
	{
		transform.gameObject.SetActive (false);
	}
}
