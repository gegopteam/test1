using UnityEngine;
using System.Collections;

public class UIAction : MonoBehaviour {
	public GameObject exchangeWindow;
	public GameObject spreeWindow;

	private AppControl appControl = null;

	void Awake()
	{
		appControl = AppControl.GetInstance ();
	}
		
	void Start()
	{
		exchangeWindow.SetActive (true);
		spreeWindow.SetActive (false);
	}

	// Update is called once per frame
	void Update () {

	}

	public void OnButton()
	{
		//Tweener tweener = ugui.DOScale (new Vector3 (1.2f, 1.2f, 1.2f), 0.5f);
		//Invoke ("Hide", 0.6f);
		transform.gameObject.SetActive (false);
		exchangeWindow.SetActive (true);
	}

	public void OpenExchange()
	{
		spreeWindow.SetActive (false);
		exchangeWindow.SetActive (true);
	}

	public void OpenSpree()
	{
		exchangeWindow.SetActive (false);
		spreeWindow.SetActive (true);
	}
}
