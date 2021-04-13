using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BackageUI : MonoBehaviour
{

	public ScrollableAreaController scrollController;

	List<int> list = new List<int> ();
	public int cellNumber;

	void Start ()
	{
		if (scrollController == null)
			scrollController = transform.Find ("ScrollRect").GetComponent<ScrollableAreaController> ();
//		Debug.Log ("start");
//		Refresh ();
	}

	public void Refresh ()
	{
//		Debug.LogError ("refreshing" + "cellNumber" + cellNumber);
		list.Clear ();
		for (int i = 0; i < cellNumber; i++) {
			list.Add (i);
		}
		scrollController.InitializeWithData (list);
	
	}

	public void RefeshIndex (int index)
	{
		list.Clear ();
		for (int i = 0; i < cellNumber; i++) {
			list.Add (i);
		}
		scrollController.InitializeWithData (list, index);
	}
}
