/***
 *
 *   Title: 切换炮倍数提示框
 *
 *   Description:用来切换炮倍数的提示
 *
 *   Author:bw
 *
 *   Date: 2019.1.29	
 *
 *   Modify: 在自己的炮台上显示该提示
 * 
 */
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class LocalGunTextPanel : MonoBehaviour
{
	bool isShow = false;
	/// <summary>
	/// 当前炮台
	/// </summary>
	GunControl localGun = null;
	/// <summary>
	/// 提示字
	/// </summary>
	Text tipsText;
	/// <summary>
	/// 提示背景图
	/// </summary>
	Image bgImage;
	public static LocalGunTextPanel Instance;

	void Awake ()
	{
		if (Instance == null) {
			Instance = this;
		}
		//初始化获取
		tipsText = transform.Find ("CenterPanel/Text").GetComponent <Text> ();
		bgImage = transform.Find ("CenterPanel").GetComponent <Image> ();
		//隐藏掉
		Hide ();
	}

	/// <summary>
	/// 设置当前的显示
	/// </summary>
	/// <param name="_str">String.</param>
	/// <param name="gun">Gun.</param>
	/// <param name="_isUseDoTween">If set to <c>true</c> is use do tween.</param>
	public void ShowTipsInLocalGun (string _str, GunControl gun, float _hideTime = 3f, bool _isUseDoTween = true)
	{
		if (isShow) {
			CancelInvoke ("Hide");//如果a信息显示到中途，b信息也要通过刚方法显示，则替换text内容，重置延长隐藏时间
		} else {

		}
		isShow = true;
		localGun = gun;
		tipsText.text = _str;
		DotweenSetShow (true, _isUseDoTween);
		transform.position = gun.gunUI.bonusEffectPos.position;
		//设置显示问题,根据位置,需要进行翻转
		if (localGun.thisSeat == GunSeat.LB || localGun.thisSeat == GunSeat.RB) {
			bgImage.rectTransform.localScale = new Vector3 (1, 1, 1);
			bgImage.rectTransform.localPosition = new Vector3 (-6, 12, 0);
		} else {
			bgImage.rectTransform.localScale = new Vector3 (1, -1, 1);
			bgImage.rectTransform.localPosition = new Vector3 (-6, -12, 0);
		}
		if (_hideTime > 0) { 
			Invoke ("Hide", _hideTime);
		}
	}

	/// <summary>
	/// 动画设置
	/// </summary>
	/// <param name="_isShow">是否显示</param>
	/// <param name="_isUseDoTween">是否使用时间</param>
	/// <param name="_lerpDuration">持续时间</param>
	void DotweenSetShow (bool _isShow, bool _isUseDoTween = true, float _lerpDuration = .3f)
	{
		isShow = _isShow;
		if (_isShow) {
			bgImage.enabled = true;
			tipsText.gameObject.SetActive (true);
			//使用动画,就让他渐现
			if (_isUseDoTween) {
				bgImage.color = new Color (0, 0, 0, 0);
				bgImage.DOColor (Color.white, _lerpDuration);
				tipsText.color = new Color (0, 0, 0, 0);
				tipsText.DOColor (Color.white, _lerpDuration);
			} else {
				bgImage.color = Color.white;
				tipsText.color = Color.white;
			}
		} else {
			//渐隐
			if (_isUseDoTween) {
				bgImage.DOFade (0, _lerpDuration);
				tipsText.DOFade (0, _lerpDuration);
			} else {
				bgImage.color = new Color (0, 0, 0, 0);
				tipsText.color = new Color (0, 0, 0, 0);
			}
		}
	}

	/// <summary>
	/// 隐藏掉,并置空参数
	/// </summary>
	public void Hide ()
	{
		DotweenSetShow (false);
		isShow = false;
		localGun = null;
		transform.position = Vector3.up * 1000;
	}
}
