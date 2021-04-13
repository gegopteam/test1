using UnityEngine;
using System.Collections;
using System.Collections.Generic;

using AssemblyCSharp;

public class CannonInfo
{
	public int userId;
	public int seatIndex;
	public long cannonMultiple;
	public FiUserInfo userInfo;
	public GunControl cannon;
}

public class CannonManager
{
	private List<CannonInfo> mCannonList = new List<CannonInfo>();

	private GunControl[] gunCannon = new GunControl[ 4 ];

	private int mCannonCount = 0;

	public CannonManager()
	{

	}

	public void ClearAll()
	{
		ClearInfo ();
		ClearCannon ();
	}

	public void AddInfo(CannonInfo info)
	{
		if ( null == info )
			return;
		RemoveInfo (info.userId);
		mCannonList.Add (info);
	}

	public void RemoveInfo( int userId )
	{
		if (0 == mCannonList.Count)
			return;
		for( int i= mCannonList.Count-1; i >= 0; i-- )
		{
			if(userId==mCannonList[i].userId)
			{
				mCannonList.Remove (mCannonList [i]);
			}
		}

	}

	public CannonInfo GetInfo(int userId)
	{
		CannonInfo cannonInfo = null;
		foreach(CannonInfo info in mCannonList)
		{
			if(userId == info.userId)
			{
				cannonInfo = info;
			}
		}
		return cannonInfo;
	}

	public void ClearInfo()
	{
		if(null!=mCannonList)
			mCannonList.Clear ();
	}

	//渔场回调的接口，把具体的炮台参数设置进去
	public void SetGunControl(GunControl cannon)
	{
		mCannonCount ++;
		//Debug.LogError (" [ cannon manager ]SetGunControl ==========:" + mCannonCount + " / " + cannon.thisSeat );
		//Tool.OutLogWithToFile ("设置创建的炮");
//		Debug.LogError("SetGunControl Seat:"+cannon.thisSeat);
		switch(cannon.thisSeat)
		{
		case GunSeat.LB:
			gunCannon [0] = cannon;
			break;
		case GunSeat.RB:
			gunCannon [1] = cannon;
			break;
		case GunSeat.RT:
			gunCannon [2] = cannon;
			break;
		case GunSeat.LT:
			gunCannon [3] = cannon;
			break;
		}
		//Debug.LogError ("SetGunControlInfo==============》" + gunCannon[ 0 ] + " / " + gunCannon[ 1 ] + " / " + gunCannon[ 2 ] + " / " + gunCannon[ 3 ] );
		if( 4 == mCannonCount )
		{//炮全部创建好后初始化玩家的炮
//			Tool.Log ("炮全部创建好后初始化玩家的炮");
			CreateCannons ();
		}

		//Debug.LogError ( "------------mCannonCount------------" + mCannonCount );
	
		MyInfo nInfo = (MyInfo)Facade.GetFacade ().data.Get (FacadeConfig.USERINFO_MODULE_ID);
		if ( nInfo.lastGame.type == TypeFishing.CLASSIC ) {
			if (nInfo.platformType == 22 || nInfo.platformType == 24) {
				if (  mCannonCount == 1 && (nInfo.sailDay == -1 ||  nInfo.sailDay == -2 )  && nInfo.cannonMultipleMax >= 5 ) {
					StartGiftManager.OpenCountDownBoxTip ( Panel_UnlockMultiples._instance.transform.parent.parent , false );
				}
			}
		}
	}

	//初始化炮
	public void CreateCannons()
	{
		Tool.OutLogWithToFile ("Fishing 初始化大炮");

		//Debug.LogError ( "--------mCannonList.Count-----------" + mCannonList.Count );
		foreach(CannonInfo info in mCannonList)
		{
			//创建炮
			info.cannon = CreateSingleCannon (info.userInfo);
		}
	}

	GunControl SelectGun( int nSeatIndex )
	{
        if (GameController._instance == null)
            return null;
        
		//Debug.LogError ( "------------nSeatIndex---------------" + nSeatIndex );
       
        if(GameController._instance.isOverTurn){
            switch (nSeatIndex)
            {
                case 1:
                    Debug.LogError("gunCannon2:" + gunCannon[2].gunUI.thisSeat);
                    return gunCannon[2];
                case 2:
                    Debug.LogError("gunCannon3:" + gunCannon[3].gunUI.thisSeat);
                    return gunCannon[3];
                case 3:
                    Debug.LogError("gunCannon0:" + gunCannon[0].gunUI.thisSeat);
                    return gunCannon[0];
                case 4:
                    Debug.LogError("gunCannon1:" + gunCannon[1].gunUI.thisSeat);
                    return gunCannon[1];
            }
        }else{
            switch (nSeatIndex)
            {
                case 1:
                    Debug.LogError("gunCannon1:" + gunCannon[0].gunUI.thisSeat);
                    return gunCannon[0];
                case 2:
                    Debug.LogError("gunCannon2:" + gunCannon[1].gunUI.thisSeat);
                    return gunCannon[1];
                case 3:
                    Debug.LogError("gunCannon3:" + gunCannon[2].gunUI.thisSeat);
                    return gunCannon[2];
                case 4:
                    Debug.LogError("gunCannon4:" + gunCannon[3].gunUI.thisSeat);
                    return gunCannon[3];
            }
        }
		
		Debug.LogError ( "------------null---------------" );
		return null;
	}

	//创建炮
	public GunControl CreateSingleCannon(FiUserInfo info)
	{
		Tool.OutLogWithToFile ("Fishing 创建大炮");
		//Debug.LogError ( "Fishing 创建大炮" + info.ToString());

		if (null == info)
			return null;
        //info.seatIndex = 3;//debugTest


        int finalSeatIndex = info.seatIndex;
        if (GameController._instance.isOverTurn && false){
            switch (info.seatIndex)
            {
                case 1:
                    finalSeatIndex = 3;
                    break;
                case 2:
                    finalSeatIndex = 4;
                    break;
                case 3:
                    finalSeatIndex = 1;
                    break;
                case 4:
                    finalSeatIndex = 2;
                    break;
                default:
                    break;
            }
        }
        //finalSeatIndex = 3;//debugTest
        GunControl nCannon = SelectGun( finalSeatIndex );
	
		if(null!=nCannon)
		{
			//Debug.LogError ( " OnUserJoin info  == > " + info.ToString() );
			nCannon.OnUserJoin (info);
			Tool.OutLogWithToFile ("CreateCannon success seatIndex:"+info.seatIndex);
		}
		else
		{
			//Debug.LogError ( " nCannon   == >  null " );
			Tool.OutLogWithToFile ("CreateCannon fail null==gun seatIndex:"+info.seatIndex);
		}
		return nCannon;
	}

	//删除炮
	public void RemoveSingleCannon(int userId)
	{
		foreach(GunControl gun in gunCannon)
		{
			if(null!=gun)
			{
				if(userId==gun.userID)
				{
					gun.OnUserLeave ();
					break;
				}
			}
		}
	}

	public void ClearCannon()
	{
		//Debug.LogError ("ClearCannon");
		for(int i=0; i<4; i++)
		{
			if(null!=gunCannon)
				gunCannon [i] = null;
		}
		mCannonCount = 0;
	}

	public GunControl GetLocalGun(){
		for (int i = 0; i < gunCannon.Length; i++) {
			if (gunCannon [i].isLocal)
				return gunCannon [i];
		}
		return null;
	}
}
