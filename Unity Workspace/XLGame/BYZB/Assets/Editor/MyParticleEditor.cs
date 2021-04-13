using UnityEngine;
using System.Collections;
using UnityEditor;

[ExecuteInEditMode]
public class MyParticleEditor :Editor {

	public float delayTime=1f;
	ParticleSystem [] part;

	// Use this for initialization
	void Start () {
		//part = this.GetComponentsInChildren<ParticleSystem> ();
		for (int i = 0; i < part.Length; i++) {
			//part [i].enableEmission = false;
		//	part[i].startDelay=delayTime;
		}	
		//Invoke ("DelayPlay", delayTime);
	}
	

}
