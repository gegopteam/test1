/***
 *
 *   Title: Boss排行榜界面脚本
 *
 *   Description: Boss排行信息的界面功能(挂载在 -> BossRankWindowCanvas 中)
 *
 *   Author: bw
 *
 *   Date: 2019.1.30
 *
 *   Modify: 实现UI界面的功能
 * 
 */
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using AssemblyCSharp;
using Google.Protobuf;
using UnityEngine.UI;

public class BossRankingScript : MonoBehaviour,IUiMediator
{
	RankInfo rankInfo = null;
	public BackageUI bossBack;
	public static List<FishingUserRank> userRankList = new List<FishingUserRank> ();
	Text TipsText;
	Button closeBtn;

	void Awake ()
	{
		//获取数据初始化
		Facade.GetFacade ().ui.Add (FacadeConfig.UI_RANK_MODULE_ID, this);
		rankInfo = (RankInfo)Facade.GetFacade ().data.Get (FacadeConfig.RANK_MODULE_ID);
		TipsText = transform.Find ("TipsImage/TipsText").GetComponent <Text> ();
		closeBtn = transform.Find ("BGImage/CloseButton").GetComponent <Button> ();
	}

	void Start ()
	{
		closeBtn.onClick.RemoveAllListeners ();
		closeBtn.onClick.AddListener (ClickCloseBtn);
	}

	/// <summary>
	/// 接受数据
	/// </summary>
	/// <param name="nType">N type.</param>
	/// <param name="nData">N data.</param>
	public void OnRecvData (int nType, object nData)
	{
		//		Debug.LogError ("BossRankingScript——————recv————————— type : " + nType);
		if (nType == FiEventType.RECV_XL_BOSSMATCHRESULT_RESPOSE) {
			userRankList = rankInfo.GetBossRankArray ();
			//			Debug.LogError ("userRankList count = " + userRankList.Count);
			bossBack.cellNumber = userRankList.Count;
			bossBack.Refresh ();
			//			Debug.LogError ("nData = " + nData.ToString ());
			//提示信息的赋值
			TipsText.text = nData.ToString ();
		}
	}

	public	void OnInit ()
	{

	}

	public	void OnRelease ()
	{

	}

	/// <summary>
	/// 点击关闭按钮
	/// </summary>
	void ClickCloseBtn ()
	{
		AudioManager._instance.PlayEffectClip (AudioManager.effect_closePanel);
		Destroy (gameObject);
	}

	/// <summary>
	/// 清除数据
	/// </summary>
	void  OnDestroy ()
	{
		Facade.GetFacade ().ui.Remove (FacadeConfig.UI_RANK_MODULE_ID);
		userRankList.Clear ();
	}
}