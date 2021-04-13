using UnityEngine;
using System.Collections;

using DG.Tweening;
using UnityEngine.UI;

public class Panel_UnlockMultiples : MonoBehaviour
{

	public static Panel_UnlockMultiples _instance;

	bool isShow = false;

	public Text multipleNumText;
	public Text goldReturnText;

	public Image bgImage;
	public GameObject diamondProgessPanel;
	public GameObject returnGoldPanel;

	public GameObject outlineEffect;
	public GameObject outlineBgEffect;
	public GameObject fingerEffect;

	public GameObject unLockGunBtn;

	public Image diamondProgessBar;
	public Text diamondNumText;

	public Button bgBtn;

	GunControl localGun = null;
	MyInfo myInfo;

	int targetMultiples = -1;
	int targetDiamond = -1;
	long goldReturn = -1;
	public long tempUnlockGoldReturn;
	int currentDiamond = -1;

	GameObject coinsEffectPrefab;

	Animator btnAnimator;

	public float orignalScale = 0.4f;

	RoomInfo roomInfo = null;

	public GameObject toHallPanel;
	int toHallMultiple = 150;
	//100 to 150

	void Awake ()
	{
		if (null == _instance)
			_instance = this;
		roomInfo = DataControl.GetInstance ().GetRoomInfo ();
	}

	void Start ()
	{
		if (GameController._instance.myGameType != GameType.Classical) {
			transform.position = Vector3.up * 1000f;
		}
		if (GameController._instance.isExperienceMode) {
			ForeverHide ();
		}
		// transform.DOScaleX(0,0);
		transform.GetComponent<RectTransform> ().localScale = new Vector3 (0, orignalScale, orignalScale);
		fingerEffect.SetActive (false);
		bgBtn.enabled = false;
		coinsEffectPrefab = PrefabManager._instance.GetPrefabObj (MyPrefabType.CoinGroup, 0);
		btnAnimator = transform.parent.GetComponent<Animator> ();
		//ShowToHallPanel();//debugTest
	}

	public void DelayInit ()
	{
		Invoke ("Init", 1f);
	}

	void Init ()
	{
		transform.parent.Find ("UnlockGunBtn/BtnArea").GetComponent<Button> ().enabled = true;
		UpdateDiamondPorgress (); //又可能会出现获取不到LocalGun的情况，解决方案：通过GunContrl初始化后再调用
		multipleNumText.text = GetLocalGun ().gunUI.GetNextUnlockMultiples ().ToString ();

		int index = GetLocalGun ().gunUI.GetMultiplesUnlockIndex (GetLocalGun ().maxCannonMultiple);
		if (index + 1 < GunInUI.goldReturnGroup.Length) {
			goldReturn = GunInUI.goldReturnGroup [index + 1];
		} else {
			goldReturn = 0;
		}

		goldReturnText.text = goldReturn.ToString ();
	}

	public void SetShow (bool toShow)
	{
		//if (transform.parent.GetComponent<LeftOption>().isShow == false)
		// return;
 
		isShow = toShow;
		//if (targetMultiples >= 1500)
		//	isShow = false;
		if (toShow) {
			//transform.DOScale (0.5f, 0.3f);
			transform.DOScaleX (orignalScale, 0.3f); 
			if (returnGoldPanel.activeSelf) {
				Invoke ("OutlineEffectToggleShow", 0.2f);
			}
			
			multipleNumText.text = localGun.gunUI.GetNextUnlockMultiples ().ToString ();
			//btnAnimator.enabled = false;
			AllDoFade (1);
		} else {
			//transform.DOScale (0f, 0.3f);
			transform.DOScaleX (0, 0.3f);//ui改版后不需要该功能
			outlineBgEffect.SetActive (false);
			Invoke ("OutlineEffectToggleShow", 0.4f);
			fingerEffect.SetActive (false);
			AllDoFade (0);
		}

	}

	void AllDoFade (float value)
	{
		bgImage.DOFade (value, 1f);
	}

	public void SendUnlockRequest ()
	{
		targetMultiples = GetLocalGun ().gunUI.GetNextUnlockMultiples ();
		Debug.LogError ("SndTargetMultiple:" + targetMultiples + "/ diamond=" + targetDiamond);
		UIFishingMsg.GetInstance ().SndUnlockCannon (targetMultiples);
	}

	public void RcvUnlockInfo (int targetMul, long goldReturn, int diamondCost)
	{

		Debug.LogError ("RcvUnlockInfo:" + targetMul + "/" + goldReturn + "/" + diamondCost);
		if (targetMultiples != targetMul || this.goldReturn != goldReturn || targetDiamond != diamondCost) {
			Debug.LogError ("Error! server=" + targetMul + "/" + goldReturn + "/" + diamondCost +
			" client=" + targetMultiples + "/" + this.goldReturn + "/" + targetDiamond);
			//HintText._instance.ShowHint ("Error! server=" + targetMul + "/" + goldReturn + "/" + diamondCost +
//			" client=" + targetMultiples + "/" + this.goldReturn + "/" + targetDiamond);
		}

		this.targetMultiples = targetMul;
		tempUnlockGoldReturn = goldReturn;
		targetDiamond = diamondCost;
       

		bgBtn.enabled = true;
		if (targetMul == toHallMultiple) {//当触发到新手场最大炮倍数时，弹出界面强制要求去别的场次
			Invoke ("ShowToHallPanel", 4f);  
		}

		UnlockMultiples ();
	}

	void ShowToHallPanel ()
	{
		return; //新版本不需要再跳转了
		GameObject.Instantiate (toHallPanel);
	}

	public void UnlockMultiples ()
	{
		Debug.Log(" 修改炮倍數  UnlockMultiples ");
		if (roomInfo.roomMultiple == 0 && GetLocalGun ().maxCannonMultiple >= 50) {
			GetLocalGun ().maxCannonMultiple = targetMultiples;
			GetLocalGun ().gunUI.SetMultiple (50);
		} else {
			GetLocalGun ().maxCannonMultiple = targetMultiples;
			GetLocalGun ().gunUI.SetMultiple (targetMultiples);
		}
		GetLocalGun ().gunUI.AddValue (0, 0, -targetDiamond, false); //这一步末尾参数要设置fasle，否则会执行UpdateDiamondProgress

		AudioManager._instance.PlayEffectClip (AudioManager.effect_getCoin);

		GameObject coinGroup = GameObject.Instantiate (coinsEffectPrefab, returnGoldPanel.transform.position, Quaternion.identity) as GameObject;
		coinGroup.GetComponent<CoinEffectGroup > ().StartMoveToPlayer (GetLocalGun (), (int)tempUnlockGoldReturn, true);

		fingerEffect.SetActive (false);
       
		if (targetMultiples >= GunControl.limitMultiple) {//此时说明已经解锁到最大倍，永久隐藏不显示
			Invoke ("ForeverHide", 0.5f);
			return;
		}
		if (UpdateDiamondPorgress ()) { //如果还能继续解锁
			SetShow (false);
			Invoke ("PanelToggleShow", 3f);
		} else {//如果不能继续解锁
			SetShow (false);
		}

	}

	void ForeverHide ()
	{
		SetShow (false);
//		DestroyImmediate (transform.gameObject);
		transform.GetComponent<RectTransform> ().localScale = new Vector3 (0, orignalScale, orignalScale);
		GameObject.Find ("Canvas/LeftOption").GetComponent<LeftOption> ().RemoveUnlockMultipleIcon ();
	}

	public bool  UpdateDiamondPorgress ()
	{

		if (GetLocalGun ().maxCannonMultiple >= GunControl.limitMultiple) { //目前上限设置1000
			return false;
		}
		int index = GetLocalGun ().gunUI.GetMultiplesUnlockIndex (GetLocalGun ().maxCannonMultiple);
		targetDiamond = GunInUI.diamondCostGroup [index + 1];

		currentDiamond = (int)localGun.curretnDiamond;


		goldReturn = GunInUI.goldReturnGroup [index + 1];
	
		goldReturnText.text = goldReturn.ToString ();

		diamondNumText.text = currentDiamond.ToString () + "/" + targetDiamond.ToString ();
		float percent = (float)currentDiamond / (float)targetDiamond;
		//zzzzzz sleeping  

		if (percent >= 1) {//当钻石数到达解锁条件时，主动弹出该界面
			SetReturnGoldShow (true);
			SetDiamondProgressShow (false);
			SetShow (true);
			return true;
		} else {
			SetReturnGoldShow (false);
			SetDiamondProgressShow (true);
			diamondProgessBar.fillAmount = percent;
			return false;
		}

	}

	public void OutlineEffectToggleShow ()
	{
		return;//ui改版后需要去掉outLine特效
		outlineEffect.SetActive (isShow);
		if (isShow)
			outlineBgEffect.SetActive (true);
	}


	public  void PanelToggleShow ()
	{
		SetShow (!isShow);
	}

	void CancleAutoPopShow ()
	{
		
	}

	void SetReturnGoldShow (bool toShow)
	{
		returnGoldPanel.SetActive (toShow);
		bgBtn.enabled = toShow;
		if (toShow) {
			if (targetMultiples <= 30) {
				fingerEffect.SetActive (true);
			} 
		} else {
			fingerEffect.SetActive (false);
		}
		targetMultiples = GetLocalGun ().gunUI.GetNextUnlockMultiples ();
	}

	void SetDiamondProgressShow (bool toShow)
	{
		diamondProgessPanel.SetActive (toShow);
	}

	GunControl GetLocalGun ()
	{
		if (localGun == null)
			return localGun = PrefabManager._instance.GetLocalGun ();
		return localGun;
	}

	public void PlayUIClickAudio ()
	{
		AudioManager._instance.PlayEffectClip (AudioManager.ui_click);
	}
}
