using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using AssemblyCSharp;

public class DragonCardInfo : IDataProxy
{
	
	//服务器获取的龙卡奖励数据
	private List<FiRewardStructure> mDragonCardRewardInfo = new List<FiRewardStructure> ();

	//服务器获取龙卡信息
	public  FiDraGonRewardData mFiDragonCardData = new FiDraGonRewardData ();

	//接收特惠信息
	public FiPreferentialData fiPreferentialData = new FiPreferentialData ();

	public List<int> IsPurDragonTypeArray = new List<int> ();

	//转盘奖池
	public List<long> GoldPool = new List<long> ();
	public long CurrendGold;
	//当前身上所有金币
	public long CheckGold;
	//上次下注金币
	public long Curcheckgold;
	//当前下注金币
	//
	public int ManmonWinTime;
	public long LongLiuShui;
	public long LongTime;
	public long userId;
	public int liuShuiDiamond;
	//	public
	public DragonCardInfo ()
	{

	}

	~DragonCardInfo ()
	{

	}

	public void OnAddData (int nType, object nData)
	{

	}

	public void OnInit ()
	{

	}
   
	//登录时显示的信息
	public void AppendDgData (FiDraGonRewardData nFiDragonCardData)
	{
		mFiDragonCardData = nFiDragonCardData;
	}
	//登录时显示特惠数据
	public void LoginShowPreferentialData (FiPreferentialData data)
	{
		fiPreferentialData = data;
	}

	public void AppendDgCardRewardInfo (List<FiRewardStructure> nDragonCardRewardInfo)
	{
		mDragonCardRewardInfo = nDragonCardRewardInfo;
	}


	public void SetCardStatue (int cardstatue)
	{
		Debug.Log(" 購買玩龍卡成功 "+ cardstatue);
		if (cardstatue <= 0) {
			return;
		}
		for (int i = 0; i < mFiDragonCardData.DarGonCardDataArray.Count; i++) {
			if (cardstatue > 1) {
				if (cardstatue == i + 1) {
					mFiDragonCardData.DarGonCardDataArray [i] = 30;
				}
			} else {
				if (cardstatue == i + 1) {
					mFiDragonCardData.DarGonCardDataArray [i] = 7;
				}
			}
		}

	}


	public void OnDestroy ()
	{

	}
}
