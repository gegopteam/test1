using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using AssemblyCSharp;

//大赢家的数据类
public class UpLevelInfo : IDataProxy
{
	//任务ID
	public int taskid;
	//当前值
	public int taskCurValue;
	//最大任务值
	public int taskMaxValue;
	//领奖状态
	public int rewardState;
	//右侧显示 最大获取金币额度
	public long showInfoMaxValue;


    //領取結果
	public int result;
	//任務ID
	public int taskID;
	//任務等級
	public int taskLevel;
	//任務獎勵
	public long gold;

	public List<int> UpLevelreward;

	public UpLevelInfo()
	{

	}

	~UpLevelInfo()
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
		//		UpLevelState.Clear ();
	}
}