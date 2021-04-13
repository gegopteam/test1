using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WelcomeExperienceScr : MonoBehaviour
{
	public GameObject newcomerGuidePrefab;

	public void ClickCloseButton ()
	{
		Destroy (this.gameObject);
		GameObject.Instantiate (newcomerGuidePrefab);
	}
}
