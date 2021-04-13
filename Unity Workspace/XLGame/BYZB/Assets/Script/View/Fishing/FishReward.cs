using UnityEngine;
using System.Collections;
using System.Collections.Generic;

using AssemblyCSharp;

public class FishReward
{
	public static void OpenReward (int userId, Cordinate position, List<FiProperty> array)
	{
		if (null == array)
			return;
		if (PrefabManager._instance == null)
			return;
		if (PrefabManager._instance.GetGunByUserID (userId) == null)
			return;
        
		foreach (FiProperty property in array) {
			if (null != property) {
				//property.type =  FiPropertyType.FISHING_EFFECT_FREEZE; //debugTest
				switch (property.type) {
				case FiPropertyType.GOLD: //加金币
                        //ReturnCoin (userId, position, property.value);
					Tool.OutLogWithToFile ("找不到鱼，强制加金币=" + property.value);

					PrefabManager._instance.GetGunByUserID (userId).gunUI.AddValue (0, property.value, 0);
					break;

				case FiPropertyType.G_POINT:
                       
					PrefabManager._instance.GetGunByUserID (userId).gunUI.AddValue (0, property.value, 0);
                   
					break;
				case FiPropertyType.DIAMOND:
                        //Debug.LogError ("Get diamond drop");
                      
					PrefabManager._instance.GetGunByUserID (userId).gunUI.AddValue (0, 0, property.value);
					break;
				case FiPropertyType.EXP:
                        //                  Debug.LogError ("GetExp:" + property.value);
					break;

				case FiPropertyType.LUCKDRAW_GOLD: //奖金鱼注入奖池的金币
					if (userId == DataControl.GetInstance ().GetMyInfo ().userID) {
						DataControl.GetInstance ().GetMyInfo ().loginInfo.luckyGold += property.value;
						DataControl.GetInstance ().GetMyInfo ().loginInfo.luckyFishNum++;
						if (LuckDrawCanvasScr.Instance != null) {
							LuckDrawCanvasScr.Instance.UpdateData ();
						}
					}
					break;
				//体验场积分金币
				case FiPropertyType.TEST_GOLD:
//					Debug.LogError ("Get TEST_GOLD drop = property.value = " + property.value);
					PrefabManager._instance.GetGunByUserID (userId).gunUI.AddValue (0, 0, 0, false, property.value);
					break;
				default:
					if (userId == DataControl.GetInstance ().GetMyInfo ().userID &&
					    property.type >= FiPropertyType.FISHING_EFFECT_FREEZE && //道具id，始于冰冻，止于鱼雷
					    property.type <= FiPropertyType.TORPEDO_NUCLEAR) {
						if (PrefabManager._instance != null) {
							GameObject dropSkillObj = PrefabManager._instance.GetSkillUIByServerId (property.type).gameObject;
							Skill temp = dropSkillObj.GetComponent<Skill> ();
							PrefabManager._instance.GetSkillUIByType (temp.skillType, temp.torpedoLevel).AddRestNum (property.value);

							// if(dropSkillObj!=null){
							//  Debug.LogError("DropSkillName="+dropSkillObj .name);
							// GameObject temp = GameObject.Instantiate(dropSkillObj);
							// temp.transform.position = new Vector3(position.x, position.y, 0.1f);
							// temp.GetComponent<Skill>().DropAndMove(property.value, userId, new Vector3(position.x, position.y, 0.1f));
							//}else{
							//  Debug.LogError("ErrorDropSkill! id="+property.type);
							// }
                                                                       
						}

					}
					break;
             
				}
			}
		}

	}

	public static void ReturnCoin (int userID, Cordinate position, int coinCount)
	{
		GameObject prefabe = ResControl.GetInstance ().prefabInfo.GetPrefab (TypePrefab.Reward_Gold);
		if (null == prefabe)
			return;
		//这部分需要交给服务器来判断
		//CannonInfo info =  UIFishingMsg.GetInstance ().cannonManage.GetInfo (userID);
		CannonInfo info = UIFishingObjects.GetInstance ().cannonManage.GetInfo (userID);
		if (null != info) {
			if (null != info.cannon) {
				Vector3 coinsPos = new Vector3 (position.x, position.y, 0f);
				GameObject coinGroup = GameObject.Instantiate (prefabe, coinsPos, Quaternion.identity) as GameObject;
				//coinGroup.GetComponent<CoinEffectGroup> ().StartMoveToPlayer ((int)info.cannon.thisSeat, coinCount, 1.7f);
			}
		} else {
			Tool.AddLogMsg ("Error:info=null");
		}
	}
}
