using System;
using AssemblyCSharp;
using System.Collections.Generic;
using UnityEngine;

public class TaskMsgHandle : IMsgHandle
{
	public delegate void ChangeSilderValue (int active);

	public event ChangeSilderValue ChangeSliderValueEvent;

	public delegate void InitSilderValue ();

	public event InitSilderValue InitSliderValueEvent;


	private static List<FiEverydayTaskDetial> task = new List<FiEverydayTaskDetial> ();

	public TaskMsgHandle ()
	{
		if (task.Count == 0) {
			for (int i = 0; i < 8; i++) {
				FiEverydayTaskDetial ntask = new FiEverydayTaskDetial ();
				ntask.taskId = i + 1;
				ntask.progress = 0;
				if (ntask.taskId != 7)
					task.Add (ntask);

			}

		}
	}

	public static List<FiEverydayTaskDetial> GetList {
		get{ return task; }

	}

	public static void DestoryList ()
	{
		task = new List<FiEverydayTaskDetial> ();
	}


	public void OnInit ()
	{
		//		AddMsgEventHandle ();
		//Debug.LogError("Joey Test TaskMsgHandle OnInit @@@@@@@@@@@@@@@@@@@@@@@@@@");
		EventControl mControl = EventControl.instance ();
		mControl.addEventHandler (FiEventType.RECV_EVERYDAY_TASK_RESPONSE, RecvEveryDayTaskResponse);
		mControl.addEventHandler (FiEventType.RECV_EVERYDAY_TASK_PROCESS_INFORM, RecvEveryDayTaskProcessInform);
		mControl.addEventHandler (FiEventType.RECV_EVERYDAY_ACTIVITY_AWARD_RESPONSE, RecvEveryDayAwardResponse);

		mControl.addEventHandler (FiEventType.RECV_NOTIFY_BEGINNER_TASK_PROGRESS, RecvBenginnerTaskProcess);
		mControl.addEventHandler (FiEventType.RECV_BEGINNER_TASK_REWARD_RESPONSE, RecvBenginnerTaskRewardResponse);
		mControl.addEventHandler (FiEventType.RECV_NOTIFY_OTHER_BEGINNER_REWARD, RecvOtherBenginnerTaskRewardInform);
		//救济金
		mControl.addEventHandler (FiEventType.RECV_CL_HELP_GOLD_TASK, RecvHelpGoldTask);
		mControl.addEventHandler (FiEventType.RECV_CL_GET_HELP_TASK_REWARD_RESPONSE, RecvGetHelpTaskReawrdResponse);
		TaskControl.Initial ();
	}

	public void OnDestroy ()
	{
		DestoryList ();
		EventControl mControl = EventControl.instance ();
		//mControl.removeEventHandler( FiEventType.RECV_EVERYDAY_TASK_RESPONSE ,       RecvEveryDayTaskResponse );
		//mControl.removeEventHandler ( FiEventType.RECV_EVERYDAY_TASK_PROCESS_INFORM , RecvEveryDayTaskProcessInform );
		mControl.removeEventHandler (FiEventType.RECV_EVERYDAY_TASK_RESPONSE, RecvEveryDayTaskResponse);
		mControl.removeEventHandler (FiEventType.RECV_EVERYDAY_TASK_PROCESS_INFORM, RecvEveryDayTaskProcessInform);
		mControl.removeEventHandler (FiEventType.RECV_EVERYDAY_ACTIVITY_AWARD_RESPONSE, RecvEveryDayAwardResponse);
		//mControl.removeEventHandler ( FiEventType.RECV_BEGINNER_TASK_INFORM , RecvBenginnerTaskInform );

		mControl.removeEventHandler (FiEventType.RECV_NOTIFY_BEGINNER_TASK_PROGRESS, RecvBenginnerTaskProcess);
		mControl.removeEventHandler (FiEventType.RECV_BEGINNER_TASK_REWARD_RESPONSE, RecvBenginnerTaskRewardResponse);
		mControl.removeEventHandler (FiEventType.RECV_NOTIFY_OTHER_BEGINNER_REWARD, RecvOtherBenginnerTaskRewardInform);
		//救济金
		mControl.removeEventHandler (FiEventType.RECV_CL_HELP_GOLD_TASK, RecvHelpGoldTask);
		mControl.removeEventHandler (FiEventType.RECV_CL_GET_HELP_TASK_REWARD_RESPONSE, RecvGetHelpTaskReawrdResponse);


	}


	public void SendTaskProcessRequest ()
	{
		//UnityEngine.Debug.LogError ("-----------------SendTaskProcessRequest------------------" );
		DataControl.GetInstance ().PushSocketSndByte (FiEventType.SEND_EVERYDAY_PROCESS_REQUEST, null);
	}

	public void SendEveryDayTaskRequest (int nTaskId)
	{
		FiEveryDayActivityRequest nRequest = new FiEveryDayActivityRequest ();
		List<int> nTaskArray = new List<int> ();
		nTaskArray.Add (nTaskId);
		nRequest.taskId = nTaskArray;
		DataControl.GetInstance ().PushSocketSndByte (FiEventType.SEND_EVERYDAY_TASK_REQUEST, nRequest.serialize ());
	}

	public void SendActivityAwardRequest (int nActivity)
	{
		FiEverydayActivityAwardRequest nRequest = new FiEverydayActivityAwardRequest ();
		nRequest.activity = nActivity;
		DataControl.GetInstance ().PushSocketSndByte (FiEventType.SEND_EVERYDAY_ACTIVITY_AWARD_REQUEST, nRequest.serialize ());
	}

	public void SendEveryDayTaskRequest (List<int> nTaskArray)
	{
		FiEveryDayActivityRequest nRequest = new FiEveryDayActivityRequest ();
		nRequest.taskId = nTaskArray;
		DataControl.GetInstance ().PushSocketSndByte (FiEventType.SEND_EVERYDAY_TASK_REQUEST, nRequest.serialize ());
	}

	//发送新手任务奖励
	public void SendBeginnerTaskRewardRequest (int nBeginnerTaskId)
	{
		FiBeginnerTaskRewardRequest nRequest = new FiBeginnerTaskRewardRequest ();
		nRequest.beginnerCurTask = nBeginnerTaskId;
		DataControl.GetInstance ().PushSocketSndByte (FiEventType.SEND_BEGINNER_TASK_REWARD_REQUEST, nRequest.serialize ());
	}

	public void SendGetHelpTaskReawrdRequest (int propid, int userid, int count, int  taskid)
	{
		Debug.Log ("SendGetHelpTaskReawrdRequest propid = " + propid + "userid = " + userid + "count = " + count + "taskid  = " + taskid);
		FiGetHelpGodlReward nReward = new FiGetHelpGodlReward ();
		nReward.propID = propid;
		nReward.userID = userid;
		nReward.count = count;
		nReward.taskID = taskid;
		DataControl.GetInstance ().PushSocketSnd (FiEventType.SEND_CL_GET_HELP_TASK_REWARD_REQUEST, nReward);
	}

	//	public void AddMsgEventHandle()
	//	{
	//
	//
	//	}

	//新手任务进度通知
	private void RecvBenginnerTaskProcess (object data)
	{
		FiNotifyBeginnerTaskProgress nResult = (FiNotifyBeginnerTaskProgress)data;

		//Tool.LogError("RecvBenginner:"+ nResult.beginnerCurTask+","+nResult.beginnerTaskProgress);
		if (NewcormerMissionPanel._instance != null)
			NewcormerMissionPanel._instance.SetMissionProgress (nResult.beginnerTaskProgress);
	}

	//领取新手任务奖励反馈
	private void RecvBenginnerTaskRewardResponse (object data)
	{
		FiBeginnerTaskRewardResponse nResult = (FiBeginnerTaskRewardResponse)data;
		if (nResult.result != 0) {
			Tool.LogError ("ErrorResult:" + nResult.result);
			return;
		}
		if (NewcomerRewardPanel._instance != null) {
			NewcomerRewardPanel._instance.RecvReward (nResult.properties.value);
		} else {
			Tool.LogError ("Error! RewardPanel=null");
		}
	}

	//收到其他玩家完成新手任务获取奖励的通知
	private void RecvOtherBenginnerTaskRewardInform (object data)
	{
		FiNotifyOtherBeginnerReward nResult = (FiNotifyOtherBeginnerReward)data;
		if (nResult.property.type == FiPropertyType.DIAMOND) {
			GunControl tempGun = PrefabManager._instance.GetGunByUserID (nResult.userId);
			if (tempGun != null)
				tempGun.gunUI.AddValue (0, 0, nResult.property.value);
		}
	}

	//废弃
	private void RecvBenginnerTaskInform (object data) //这里是已经做完任务，领取奖励了
	{
		FiBeginTaskInform nInform = (FiBeginTaskInform)data; 
		//nInform.diamond

		return;//DebugTest 暂时改为本地判断  
			   //nInform.diamond改为当前进度的进度数量
		HintTextPanel._instance.SetTextShow("任務：" + nInform.currentTask + " 數量：" + nInform.diamond);
		if (PrefabManager._instance != null)
			PrefabManager._instance.ShowNewcomerRewardPanel (nInform.currentTask, nInform.diamond);
		if (NewcormerMissionPanel._instance != null) { //服务器这边还需要获取当前任务进度，任务奖励类型
			NewcormerMissionPanel._instance.CurrentMissionComplete (nInform.currentTask);
		} else {
			Tool.LogError ("Error! PrefabManager=null");
		}
	}

	private void RecvEveryDayAwardResponse (object data)
	{
		FiEverydayActivityAwardResponse nResponse = (FiEverydayActivityAwardResponse)data;
		IUiMediator nMediator = Facade.GetFacade ().ui.Get (FacadeConfig.TASK_MODULE_ID);
		UnityEngine.Debug.LogError("請求成功：" + nResponse.result);
		UnityEngine.Debug.LogError("請求成功active：" + nResponse.activity);
		if (nMediator != null) {
			nMediator.OnRecvData (UITask.ACTIVITY, data);
		}
		//刷新model
		SendTaskProcessRequest ();
	}

	private void RecvEveryDayTaskResponse (object data)
	{
		
		FiEveryDayActivityResponse nResponse = (FiEveryDayActivityResponse)data;
		if (nResponse.result == 0)
		{
			if (ChangeSliderValueEvent != null)
			{
				UnityEngine.Debug.LogError("增加了活躍度：" + nResponse.activity);
				ChangeSliderValueEvent(nResponse.activity);
				SendTaskProcessRequest();
			}
		}
		else
		{

			UnityEngine.Debug.Log("領取失敗");
		}
	}

	private void RecvEveryDayTaskProcessInform (object data)
	{
		FiEverydayTaskProgressInform nInform = (FiEverydayTaskProgressInform)data;
		DataControl.GetInstance ().getTaskInfo ().SetActivity (nInform.activity);
		DataControl.GetInstance ().getTaskInfo ().SetStates (nInform.states);
//		UnityEngine.Debug.LogError ("接到任务消息：listcount：" + nInform.tasks.Count);
		if (nInform.tasks.Count != 0) {
			TaskInfo nInfo = (TaskInfo)Facade.GetFacade ().data.Get (FacadeConfig.TASK_MODULE_ID);
			nInfo.OnAddData (FiEventType.RECV_EVERYDAY_TASK_PROCESS_INFORM, nInform);
			for (int i = 0; i < nInform.tasks.Count; i++) {
				for (int j = 0; j < task.Count; j++) {
					if (nInform.tasks [i].taskId == task [j].taskId) {
						if (nInform.tasks [i].taskId == 7)
							continue;
						task [j].progress = nInform.tasks [i].progress;
						if (task [j].progress == -1) {
							FiEverydayTaskDetial temp = task [j];
							task.RemoveAt (j);
							task.Add (temp);
						}

						if (task [j].progress >= TaskControl.getTaskInfo (task [j].taskId).maxValue) {
							FiEverydayTaskDetial temp = task [j];
							task.RemoveAt (j);
							task.Insert (0, temp);
						}
					}
				}
			}
		}
		IUiMediator nMediator = Facade.GetFacade ().ui.Get (FacadeConfig.TASK_MODULE_ID);
		if (nMediator != null) {
			nMediator.OnRecvData (UITask.TASK, task);
		}
	}

	private void  RecvHelpGoldTask (object data)
	{
		FiHelpGoldTaskData nData = (FiHelpGoldTaskData)data;
		//	Debug.Log ("RecvHelpGoldTask.nData.resultCode = " + nData.resultCode);
		if (nData.resultCode == 0) {
			
			AppInfo.isReciveHelpMsg = true;
			//	Debug.Log ("RecvHelpGoldTask.nData.nValue = " + nData.nValue);
			AlmsCountID.Instance.almsGold = nData.count;
			AlmsCountID.Instance.value = nData.nValue;
			AlmsCountID.Instance.propID = nData.propID;
			AlmsCountID.Instance.taskId = nData.taskID;
//			Debug.Log ("RecvHelpGoldTask.nData.count = " + nData.count);
//			Debug.Log ("RecvHelpGoldTask.nData.propID = " + nData.propID);
//			Debug.Log ("RecvHelpGoldTask.AlmsCountID.Instance = " + AlmsCountID.Instance.taskId);
//			Debug.Log ("RecvHelpGoldTask.nData.value = " + nData.nValue);
			if (AppInfo.isInHall == false) {
				StartGiftManager.HelpTaskRewardWindow (false);
			}
		} else {
			Debug.LogError("服務器下發消息錯誤");
		}

	}

	private void RecvGetHelpTaskReawrdResponse (object data)
	{
		FiHelpGoldTaskData nData = (FiHelpGoldTaskData)data;
		Debug.Log ("nData.resultCode = " + nData.resultCode);

		MyInfo myInfo = (MyInfo)Facade.GetFacade ().data.Get (FacadeConfig.USERINFO_MODULE_ID);
		if (nData.resultCode == 0) {
			DailySignInfo nInfo = (DailySignInfo)Facade.GetFacade ().data.Get (FacadeConfig.SIGN_IN_MODULE_ID);
			nInfo.Setisalms (false);
			//不在渔场就直接增加
			switch (nData.propID) {
			case FiPropertyType.GOLD:
				myInfo.gold += nData.count;

				Debug.Log ("RecvGetHelpTaskReawrdResponse myInfo.gold  = " + myInfo.gold);

				//在渔场就加金币
				if (PrefabManager._instance != null && PrefabManager._instance.GetLocalGun () != null) {
					PrefabManager._instance.GetLocalGun ().gunUI.AddValue (0, nData.count);
				}

				if (Facade.GetFacade ().ui.Get (FacadeConfig.UI_STORE_MODULE_ID) != null) {
					Facade.GetFacade ().ui.Get (FacadeConfig.UI_STORE_MODULE_ID).OnRecvData (FiPropertyType.GOLD, myInfo.gold);
				}
				break;
			default:
				break;
			}
//            Debug.Log("是不是龙卡"+myInfo.misHaveDraCard);
			IsLongCardBuyTips (myInfo.misHaveDraCard);
			//返回成功关闭界面
			AppInfo.isReciveHelpMsg = false;
			AlmsCountDown.Instance.DestoryAlms ();
			AlmsCountDown.Instance.ShowRewardUnits (nData.propID, nData.count);
           
		} else {
			AlmsCountDown.Instance.TipsControl("領取失敗");
		}
	}
	//不是龙卡就打开提示购买龙卡
	void IsLongCardBuyTips (bool isLongCard)
	{
		Debug.Log (isLongCard);
		if (!isLongCard) {
			Debug.Log ("不是");
			GameObject tipsObj = GameObject.Instantiate (AlmsCountDown.Instance.BuyLongCardTips);
		}
	}
}

