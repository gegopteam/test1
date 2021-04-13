using UnityEngine;
using System.Collections;
using DG.Tweening;

public class SingleDiamondDrop : MonoBehaviour {

	Vector3 targetPos;
	float duration;
	GunInUI uiGun;

	public void MoveToPos(GunInUI uiGun,float delay, float duration)
	{
		this.uiGun = uiGun;
		targetPos = uiGun.diamondImage.position;
		this.duration = duration;
		Invoke ("StartToMove", delay);
	}

	void StartToMove()
	{
		transform.DOMove (targetPos, duration);
		//moveTweener.SetEase (Ease.InQuad);
		Destroy (this.gameObject, duration);
	}

	void OnDestroy()
	{
		//uiGun.PlayGoldAcceptEffect ();
	}
}
