using UnityEngine;
using System.Collections;
using DG.Tweening;

public class CoinEffectGroup : MonoBehaviour
{


	//public Transform[] gunGroupInUI;
	GunInUI uiGun;
	public GameObject coinGold;
	public GameObject coinSliver;
	public GameObject effectNumPrefab;
	//怪物死亡时出现的金币数字
	public GameObject effectNumPrefab2;
	//金币飞到炮台UI处时出现的数字
	public float destroyTime = 0.5f;
	//改变了玩家金币数之后开始执行摧毁的倒计时时间
	int thisCoinReturn;
	int coinCount = 3;

	bool isLocal;
	bool isBonus = false;

	public float xRandom = 1.5f;
	public float yRamdom = 1.3f;

	bool isUnlockMultiple = false;

	public void StartMoveToPlayer (CannonInfo info, int coinReturn, int enemyReturn, float delayTime, bool isBonus = false) //打死鱼时的金币返回方式
	{
		

		this.isBonus = isBonus;
		uiGun = info.cannon.gunUI.GetComponent<GunInUI> ();
		transform.position -= new Vector3 (0, 0, uiGun.transform.position.z); //保证跟uiGun一个平面

		this.thisCoinReturn = coinReturn;
		this.isLocal = info.cannon.isLocal;
		if (enemyReturn <= 5) {
			coinCount = 6;
		} else if ((enemyReturn >= 6) && (enemyReturn < 10)) {
			coinCount = 8;
		} else if ((enemyReturn >= 10) && (enemyReturn < 20)) {
			coinCount = 10;
		} else if ((enemyReturn >= 20) && (enemyReturn < 100)) {
			coinCount = 12;
		} else if ((enemyReturn >= 100) && (enemyReturn < 200)) {
			coinCount = 17;
		} else if (enemyReturn >= 200) {
			coinCount = 23;
		}
      
		CreateCoins (coinCount); //每个金币有0.5秒生成时间差，金币本身出现到消失时间1.9秒，可以保证2.4秒时间内金币会全部消失
		//Invoke ("ChangeUserValue", 2.45f);
		ChangeUserValue ();
	}

	public void StartMoveToPlayer (GunControl targetGun, int coinReturn, bool isUnlockMultiples = false)
	{
 
		uiGun = targetGun.gunUI;
		thisCoinReturn = coinReturn;
		this.isLocal = targetGun.isLocal;
		int bonusNum = 12;

		this.isUnlockMultiple = isUnlockMultiples;

		if (isUnlockMultiples)
			bonusNum = 10;
		if (coinReturn <= 500) {
			coinCount = 1;
		} else if ((coinReturn >= 600) && (coinReturn < 1000)) {
			coinCount = 2;
		} else if ((coinReturn >= 1500) && (coinReturn < 4500)) {
			coinCount = 4;
		} else if (coinReturn >= 4500) {
			coinCount = 10;
		}
		coinCount += bonusNum;
		CreateCoins (coinCount); //每个金币有0.5秒生成时间差，金币本身出现到消失时间1.9秒，可以保证2.4秒时间内金币会全部消失
		//Invoke ("ChangeUserValue", 0.5f);
		ChangeUserValue ();
	}

	void CreateCoins (int count)
	{
		GameObject effectNum = GameObject.Instantiate (effectNumPrefab, Vector3.right * 10000f, Quaternion.identity,
			                       ScreenManager.uiScaler.gameObject.transform) as GameObject;

		effectNum.GetComponent<EffectNum> ().SetInfo (thisCoinReturn, transform.position, isLocal, isBonus);
		for (int i = 0; i < count; i++) {
			float randomTime;
			if (isUnlockMultiple) {
				randomTime = Random.Range (0, 0.7f);
			} else {
				randomTime = Random.Range (0, 0.5f);
			}
			Invoke ("CreateOneCoin", randomTime);
		}
	}

	void CreateOneCoin ()
	{
		if (isUnlockMultiple) {
			yRamdom = 2.5f;
		}

		Vector3 randomVector = new Vector3 (Random.Range (-xRandom, xRandom), Random.Range (-yRamdom, yRamdom), 10f);
    
		GameObject coinGo;
		if (isLocal) {
			coinGo = GameObject.Instantiate (coinGold, transform.position + randomVector, Quaternion.identity, this.transform)as GameObject;
		} else {
			coinGo = GameObject.Instantiate (coinSliver, transform.position + randomVector, Quaternion.identity, this.transform)as GameObject;
		}

		if (isUnlockMultiple) {
			coinGo.GetComponent<CoinEffect> ().MoveToPos (uiGun, 1.3f, 0.4f);
		} else {
			coinGo.GetComponent<CoinEffect> ().MoveToPos (uiGun, 1.3f, 0.4f);
		}
		
	}

	void ChangeUserValue () //需求改动，金币飞到炮台时不需要再显示金币数字了
	{
		//GameObject effectNum2= GameObject.Instantiate (effectNumPrefab2,uiGun .effectNumPos.position,Quaternion.identity,
		//ScreenManager.uiScaler.gameObject.transform) as GameObject;
		//effectNum2.GetComponent<EffectNum> ().SetInfo (thisCoinReturn, uiGun.effectNumPos.position,isLocal,false);
		if (!GameController._instance.gameIsEnd) {
			if (GameController._instance.isExperienceMode)
				uiGun.AddValue (0, 0, 0, false, thisCoinReturn);
			else
				uiGun.AddValue (0, thisCoinReturn, 0);

			//	Debug.LogError ("ThisCointReturn=" + thisCoinReturn);
		}

		Invoke ("PlayGetCoinAudio", destroyTime - 0.2f);
		//AudioManager._instance.PlayEffectClip (AudioManager.effect_getCoin);
		Destroy (this.gameObject, destroyTime);
	}

	void PlayGetCoinAudio ()
	{
		AudioManager._instance.PlayEffectClip (AudioManager.effect_getCoin2);
	}

}
