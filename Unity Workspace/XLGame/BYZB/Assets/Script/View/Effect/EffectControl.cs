using UnityEngine;
using System.Collections;

public class EffectControl : MonoBehaviour {

	public float destroyTime=1f;
	public bool isAutoDestory = true;

	void Start()
	{
		if (isAutoDestory) {
			Destroy (this.gameObject, destroyTime);
		}
	}

	public void ManualDestroy(float time)
	{
		Destroy (this.gameObject, time);
	}
}
