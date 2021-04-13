using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UILoad : MonoBehaviour {

	private AppControl appControl = null;
	public Slider slider;
	public Text showprogress;
	private float times;

	void Awake()
	{
		appControl = AppControl.GetInstance ();
		if (slider == null) 
		{
			return;		
		}
	}

	// Use this for initialization
	void Start () {
	}
		
	// Update is called once per frame
	void Update () {
		times += Time.deltaTime;
		if (times >= 2f) 
		{
			slider.value += 0.02f;
			showprogress.text = (int)(slider.value*100)+"%";
		}

		if (slider.value == 1)
		{
			OnButton ();
		}
	}

    void OnButton()
	{
		AppControl.ToView (AppView.HALL);
	}
}
