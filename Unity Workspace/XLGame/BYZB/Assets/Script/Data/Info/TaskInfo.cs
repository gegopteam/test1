using System;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using AssemblyCSharp;

public class TaskInfo:IDataProxy
{
	private List<FiEverydayTaskDetial> mTaskList = new List<FiEverydayTaskDetial>();

	private const int mTaskCount = 8;

	private int mCurrentActivity = 0;

	private bool updated=false;

	private List<int> mStates;

	private bool[] active;

	private Dictionary< int , int  > mTaskMap = new Dictionary<int, int>();

	public TaskInfo ()
	{
		mTaskMap.Add ( 1 , 1 );
		mTaskMap.Add ( 2 , 100 );
		mTaskMap.Add ( 3 , 500 );
		mTaskMap.Add ( 4 , 1000 );
		mTaskMap.Add ( 5 , 5 );
		mTaskMap.Add ( 6 , 1 );
		mTaskMap.Add ( 7 , 1 );
		mTaskMap.Add ( 8 , 100000 );

		InitTask ();
		active = new bool[]{ true, true, true, true };
	}
	public void SetStates(List<int> istates){
		mStates = istates;
	}
	public List<int> GetStates(){
		return mStates;
	}
	public void OnAddData( int nType, object nData )
	{
		FiEverydayTaskProgressInform nInform = (FiEverydayTaskProgressInform)nData;
		for( int i = 0 ; i < nInform.tasks.Count ; i ++ )
		{
			//任务完成了，显示红角标
			if (nInform.tasks [i].progress >= EqualTaskProcess( nInform.tasks [i].taskId ) ) {
				updated = true;
				return;
			}
		}
		isActive (nInform);
		if (nInform.activity > 20 && active [0]) {
			updated = true;
			return;
		} else if (nInform.activity > 50 && active [1]) {
			updated = true;
			return;
		} else if (nInform.activity > 80 && active [2]) {
			updated = true;
			return;
		} else if (nInform.activity == 120 && active [3]) {
			updated = true;
			return;
		} 
		//没有意见完成的任务，那么不显示红点点角标
		updated = false;
	}
	private void isActive(FiEverydayTaskProgressInform nInform){
		for(int i=0;i<nInform.states.Count;i++){
			switch (nInform.states [i]) {
			case 20:
				active [0] = false;
				break;
			case 50:
				active [1] = false;
				break;
			case 80:
				active [2] = false;
				break;
			case 120: 
				active [3] = false;
				break;
			}
		}
	}

	private int EqualTaskProcess( int nTaskId )
	{
		if (mTaskMap.ContainsKey(nTaskId)) {
			return mTaskMap [ nTaskId ];
		}
		return 0;
	}

	public void SetUpdate( bool nValue )
	{
		updated = nValue;
	}

	public void OnInit()
	{

	}

	public void OnDestroy()
	{

	}

	public bool isUpdated()
	{
		return updated;
	}

	public int activity
	{
		get { return mCurrentActivity; }
	}

	public void SetActivity( int nActivity )
	{
		mCurrentActivity = nActivity;
	}

	public void Foreach( IData nRecv )
	{
		if (nRecv == null)
			return;
		foreach( FiEverydayTaskDetial info in mTaskList )
		{
			nRecv.RcvInfo (info);
		}

		nRecv.RcvInfo (null);
	}

	private void InitTask()
	{
		for( int i= 1 ; i <= mTaskCount ; i++ )
		{
			FiEverydayTaskDetial nDetail = new FiEverydayTaskDetial ();
			nDetail.taskId = i;
			nDetail.progress = 0;
			mTaskList.Add ( nDetail );
		}
	}


	public void SetProgress( int nTaskId , int nProcess )
	{
		foreach (FiEverydayTaskDetial nDetail in mTaskList) {
			if (nDetail.taskId == nTaskId) {
				nDetail.progress = nProcess;
				break;
			}
		}
	}

	public int GetProcess( int nTaskId )
	{
		foreach (FiEverydayTaskDetial nDetail in mTaskList) {
			if (nDetail.taskId == nTaskId) {
				return nDetail.progress;
			}
		}
		return 0;
	}

}

