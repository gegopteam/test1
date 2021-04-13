using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActivityGridUI : MonoBehaviour
{

	public ScrollableAreaController scrollController;

	List<int> list = new List<int> ();
	public int insNum;

	void Start ()
	{
		if (scrollController == null)
			scrollController = transform.Find ("ScrollRect").GetComponent<ScrollableAreaController> ();
		Refresh ();
		Debug.Log ("Start");
	}

	public void Refresh ()
	{
		list.Clear ();
		for (int i = 0; i < insNum; i++) {
			list.Add (i);
		}
		scrollController.InitializeWithData (list);
	}
}
