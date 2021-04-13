/* author:KinSen
 * Date:2017.07.28
 */

using UnityEngine;
using System.Collections;


//负责：Fishing其他的一些操作，与UI打鱼无关
public interface IFishingOther
{

}

public static class UIFishingOther
{
	private static DataControl dataControl = null;
	private static MyInfo myInfo = null;
	private static RoomInfo roomInfo = null;
	static UIFishingOther()
	{
		dataControl= DataControl.GetInstance ();
		myInfo= dataControl.GetMyInfo ();
		roomInfo = dataControl.GetRoomInfo ();
	}
		
	public static void OneMoreGame<T>(this T example) where T: IFishingOther
	{
		AppControl.ToView (AppView.PKHALLMAIN);
		myInfo.oneMoreGame = true;
	}
}
