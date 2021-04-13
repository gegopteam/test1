using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public static class TaskControl{
	static List<TaskConfig> taskList;
	public static void Initial(){
		if (taskList == null) {
			taskList = new List<TaskConfig> ();
			for (int i = 0; i < 8; i++) {
				TaskConfig temp = new TaskConfig ();
				temp.taskId = i + 1;
				switch (temp.taskId) {
				case 1:
					temp.taskName = "登入遊戲";
					temp.activeValue = 10;
					temp.maxValue = 1;
					break;
				case 2:
					temp.taskName = "捕獲任意魚累計100條";
					temp.activeValue = 10;
					temp.maxValue = 100;
					break;
				case 3:
					temp.taskName = "捕獲任意魚累計500條";
					temp.activeValue = 20;
					temp.maxValue = 500;
					break;
				case 4:
					temp.taskName = "捕獲任意魚累計1000條";
					temp.activeValue = 20;
					temp.maxValue = 1000;
					break;
				case 5:
					temp.taskName = "使用任意道具累計5次";
					temp.activeValue = 20;
					temp.maxValue = 5;
					break;
				case 6:
					temp.taskName = "捕獲任意BOSS 1條";
					temp.activeValue = 20;
					temp.maxValue = 1;
					break;
				case 7:
					temp.taskName = "領取月卡獎勵";
					temp.activeValue = 20;
					temp.maxValue = 1;
					break;
				case 8:
					temp.taskName = "捕魚累計獲得10萬金幣";
					temp.activeValue = 20;
					temp.maxValue = 100000;
					break;
				default:
					Debug.LogError ("no task");
					break;

				}
				taskList.Add (temp);
			}
		}
	}
	public  static TaskConfig getTaskInfo(int taskId){
		if (taskId < 9 && taskId > 0)
			return taskList [taskId - 1];
		else
			return null;
		
	}

}
public class TaskConfig{
	internal int taskId;
	internal int progress;
	internal int maxValue;
	internal int activeValue;
	internal string taskName;
}