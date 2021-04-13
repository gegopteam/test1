using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using AssemblyCSharp;

//大赢家的数据类
public class UpLevelReward : IDataProxy
{
	////返回结果 0表示成功， -1 领取失败，-2已领取过，-3未达.   到条件，-4领取异常请联系客服
	public int result;
	//
	public int taskID;
	//
	public int taskLevel;
	//
	public long gold;

	public List<int> UpLevelreward;

	public UpLevelReward()
	{

	}

	~UpLevelReward()
	{

	}


	public void OnAddData(int nType, object nData)
	{

	}

	public void OnInit()
	{

	}

	public void OnDestroy()
	{
		//		UpLevelreward.Clear ();
	}




}
