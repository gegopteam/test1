using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using AssemblyCSharp;

//大赢家的数据类
public class NobelInfo : IDataProxy
{
	//每日土豪版数据
	public List<FiRankInfo> mBigWinArray = new List<FiRankInfo> ();

	//当前段位
	public int CurrentDuanwei;
	//当前是否充值
	public int IsToUp;
	//是否是月卡类型
	public int IsMonthType;
	//是否是猎杀赛
	public int ISbossmatchdouble;
	//当前排名
	public int Nrankinfo;
	//当时倒计时时间
	public long Showtime;
	//当前期数
	public int CurrentQi;

	public int Shangqipaiming;

	public int Lishizuigao;

	public int Beiqizuigao;

	public int Myselfindex;
	public List<FiRankInfo> rongYuRankInfoList = new List<FiRankInfo> ();

	public List<int> prizeState = null;
	//r荣耀奖励可领的状态 ==0 可领
	public int honorPrizeState;

	public long hotPrizePool;


	public NobelInfo ()
	{

	}

	~NobelInfo ()
	{

	}

	//这里设置是因为可能在渔场充值后,为了及时刷新用户是否充值
	public int IsToUps ()
	{
		return IsToUp = 1;
	}


	public void OnAddData (int nType, object nData)
	{

	}

	public void OnInit ()
	{
		
	}

	public List<FiRankInfo>  AppendRichData (List<FiRankInfo> nRichData)
	{
		
		mBigWinArray = ParamArray (nRichData);
		return mBigWinArray;

	}

	public int IndexMyselfRank (List<FiRankInfo> nRichData)
	{
		
		if (nRichData.Count != 0) {
			Myselfindex = InDexSelf (nRichData);
		}
		return Myselfindex;
			
	}

	public void OnDestroy ()
	{

	}

	private int InDexSelf (List<FiRankInfo> nRichData)
	{
		MyInfo myInfo = DataControl.GetInstance ().GetMyInfo ();
		for (int i = 0; i < nRichData.Count; i++) {
			if (nRichData [i].userId == myInfo.userID) {
				return i;
			}
		}
		return 0;

	}

	List<FiRankInfo> ParamArray (List<FiRankInfo> nRichData)
	{
		Debug.LogError ("nrichcout" + nRichData.Count);
		MyInfo myInfo = DataControl.GetInstance ().GetMyInfo ();
		if (nRichData.Count == 16) {
			for (int i = 0; i < nRichData.Count - 1; i++) {
				for (int j = 0; j < nRichData.Count - 1 - i; j++) {
					if (nRichData [j].gold < nRichData [j + 1].gold) {
						FiRankInfo s = nRichData [j];
						nRichData [j] = nRichData [j + 1];
						nRichData [j + 1] = s;
					}
				}
			}
			if (nRichData [15].userId == myInfo.userID) {
				nRichData.Remove (nRichData [14]);
			} else {
				nRichData.Remove (nRichData [15]);
			}
		} else {
			for (int i = 0; i < nRichData.Count - 1; i++) {
				for (int j = 0; j < nRichData.Count - i - 1; j++) {
					if (nRichData [j].gold < nRichData [j + 1].gold) {
						FiRankInfo s = nRichData [j];
						nRichData [j] = nRichData [j + 1];
						nRichData [j + 1] = s;
					}
				}
			}
		}
		return nRichData;
	}

}
