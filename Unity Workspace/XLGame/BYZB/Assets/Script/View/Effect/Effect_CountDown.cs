using UnityEngine;
using System.Collections;

public class Effect_CountDown : MonoBehaviour {

	public Transform[] numGroup;
	int numIndex;
	// Use this for initialization
	void Start () {
		numIndex = numGroup.Length-1;
		StartCountDown ();
	}

	void StartCountDown()
	{
		InvokeRepeating ("ShowNumEffect", 0, 1f);
	}

	void ShowNumEffect()
	{
		HideAllChild ();
		numGroup [numIndex].gameObject.SetActive (true);
		numIndex--;
		if (numIndex < 0) {
			CancelInvoke ("ShowNumEffect");
			Destroy (this.gameObject, 1f);
		}
			
	}

	void HideAllChild()
	{
		for (int i = 0; i < numGroup.Length; i++) {
			numGroup [i].gameObject.SetActive (false);
		}
	}
	// Update is called once per frame
	void Update () {
	
	}
}
