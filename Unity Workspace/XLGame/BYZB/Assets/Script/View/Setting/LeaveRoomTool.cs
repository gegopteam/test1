/***
 *
 *   Title: boss匹配机智通用接口
 *
 *   Description:用来接受返回大厅消息接口
 *
 *   Author:jin
 *
 *   Date: 2019.1.16
 *
 *   Modify： 
 * 
 */
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using AssemblyCSharp;

public  class LeaveRoomTool
{
	/// <summary>
	/// 离开房间通用接口,需要自行测试
	/// </summary>
	public static void LeaveRoom ()
	{
		if (PrefabManager._instance != null) {
			GameObject[] bulletGroups = GameObject.FindGameObjectsWithTag (TagManager.bullet);
			if (bulletGroups.Length > 0) {
				for (int i = 0; i < bulletGroups.Length; i++) {
					bulletGroups [i].GetComponent<Bullet> ().isMyRobot = false;
				}
			}
			ReturnRobot ();
			DelayLeave ();//留时间用来清理机器人的遗留子弹
		}
	}

	/// <summary>
	/// 延迟离开
	/// </summary>
	static	void DelayLeave ()
	{
		ChatDataInfo nInfo = (ChatDataInfo)Facade.GetFacade ().data.Get (FacadeConfig.CHAT_MODULE_ID);
		AudioManager._instance.PlayEffectClip (AudioManager.effect_closePanel);
		nInfo.ClearChatMsg ();
		UIFishing._instance.OnBack ();
	}

	/// <summary>
	/// 移除机器人
	/// </summary>
	static	void ReturnRobot ()
	{
		for (int i = 0; i < PrefabManager._instance.gunGroup.Length; i++) {
			GunControl tempGun = PrefabManager._instance.gunGroup [i].GetComponent<GunControl> ();
			if (tempGun != null) {
				if (tempGun.isRobot) {
					tempGun.RobotSendLeaveSelf ();
				}
			}
		}
	}
}
