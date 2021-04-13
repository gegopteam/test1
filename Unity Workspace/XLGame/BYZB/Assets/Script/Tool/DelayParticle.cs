using UnityEngine;
using System.Collections;

public class DelayParticle : MonoBehaviour {

	public float delayTime=1f;
	ParticleSystem [] part;

	void Awake(){
		part = this.GetComponentsInChildren<ParticleSystem> ();
		for (int i = 0; i < part.Length; i++) {
			part [i].startDelay += delayTime;
		}	
	}

}
