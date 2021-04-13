using System;
using System.Collections.Generic;
using AssemblyCSharp;

public class RankInfo:IDataProxy
{
	//	public delegate void InitRankDelegate();
	//	public static event InitRankDelegate InitRankEvent;

	public delegate void OnForeachData (FiRankInfo nInfo);

	EventControl mEventControl;
	private FiRankInfo rank;

	//每日土豪版数据
	private List<FiRankInfo> mRichArray = new List<FiRankInfo> ();
	//每日爆金版数据
	private List<FiRankInfo> mCoinArray = new List<FiRankInfo> ();
	//每日爆金版数据
	private List<FiRankInfo> mManmonArray = new List<FiRankInfo> ();
	//Boss匹配数据
	private List<FishingUserRank> bossRankArray = new List<FishingUserRank> ();


	public RankInfo ()
	{
		
	}

	~RankInfo ()
	{
		
	}

	public void OnAddData (int nType, object nData)
	{
		
	}


	public void OnInit ()
	{
		mRichArray.Clear ();
		mCoinArray.Clear ();
		mManmonArray.Clear ();
		bossRankArray.Clear ();
	}

	public void OnDestroy ()
	{
		mRichArray.Clear ();
		mCoinArray.Clear ();
		mManmonArray.Clear ();
		bossRankArray.Clear ();
	}


	public List<FiRankInfo> GetRichArray ()
	{
		return mRichArray;
	}

	public List<FiRankInfo> GetCoinArray ()
	{
		return mCoinArray;
	}

	public List<FiRankInfo> GetManmonArray ()
	{
		return mManmonArray;
	}

	public List<FishingUserRank>GetBossRankArray ()
	{
		return bossRankArray;
	}

	public void AppendRichData (List<FiRankInfo> nRichData)
	{
		mRichArray = nRichData;
	}

	//每日爆金版数据
	public void AppendCoinData (List<FiRankInfo> nCoinData)
	{
		mCoinArray = nCoinData;
	}
	//财神排行榜数据
	public void AppendManmonData (List<FiRankInfo> mManmoData)
	{
		mManmonArray = mManmoData;
	}
	//Boss数据
	public void AppendBossRankData (List<FishingUserRank> _bossRankData)
	{
		bossRankArray = _bossRankData;
	}
}

