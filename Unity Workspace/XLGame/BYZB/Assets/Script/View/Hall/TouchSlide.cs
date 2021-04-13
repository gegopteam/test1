using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TouchSlide : MonoBehaviour {

	private float fingerActionSensitivity = Screen.width*0.05f; //

	private float fingerBeginX;
	private float fingerBeginY;

	private float fingerCurrentX;
	private float fingerCurrentY;

	private float fingerSegmentX;
	private float fingerSegmentY;

	private int fingerTouchState;

	private int FINGER_STATE_NULL = 0;
	private int FINGER_STATE_TOUCH = 1;
	private int FINGER_STATE_ADD = 2;

	// Use this for initialization
	void Start () {
		fingerActionSensitivity = Screen.width * 0.05f;

		fingerBeginX = 0;
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
