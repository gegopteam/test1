using System;
using UnityEngine;
using Google.Protobuf;
using LitJson;

namespace AssemblyCSharp
{
	public class LoginMsgHandle : IMsgHandle
	{
		public LoginMsgHandle ()
		{
			
		}


		//渠道号
		//android  101010  33 (34)(37)
		//ios   10101020
		public static long getChannelNumber ()
		{
			return AppInfo.trenchNum;
			//return 10101020;
		}

		public void OnInit ()
		{
			/*EventControl mControl = EventControl.instance ();
			mControl.addEventHandler (  FiEventType.RECV_LOGIN_RESPONSE , OnRecvLoginResponse );
			mControl.addEventHandler (  FiEventType.CONNECT_SUCCESS , RcvConnectSuccess );
			mControl.addEventHandler (  FiEventType.RECV_HAVE_DISCONNECTED_ROOM_INFORM , RcvPKHaveDisconnectedRoomInform );
			mControl.addEventHandler (  FiEventType.RECV_RECONNECT_GAME_RESPONSE , IMRcvPKReconnectGameResponse );
			mControl.addEventHandler (  FiEventType.CONNECTIONT_CLOSED,   RcvConnectionClose );//只有在登陆成功后，才会*/
		}

		public void SendAccountLogin (string nAccount, string nPswd, int nType, string nMachineSerial)
		{
			DispatchData data = new DispatchData ();
			PB_CLLoginPasswdRequest nLoginRequest = new PB_CLLoginPasswdRequest ();
			nLoginRequest.Accounts = nAccount;
			Debug.Log ("SendAccountLogin = " + nAccount);
			nLoginRequest.Passwd = nPswd;
			nLoginRequest.DeviecType = nType;
			nLoginRequest.MachineSerial = nMachineSerial;
			nLoginRequest.VersionNumber = AppInfo.version;
			nLoginRequest.ChannelNumber = getChannelNumber ();
			DataControl.GetInstance ().PushSocketSndByte (FiEventType.SEND_LOGIN_PASSWD_REQUEST, nLoginRequest.ToByteArray ());
		}

		int GetDeviceType ()
		{
			int nDeviceType = 50;
			#if UNITY_IPHONE
			nDeviceType = 51;
			#endif
			return nDeviceType;
		}

		public void SendLoginRequest ()
		{
            Debug.LogError("到这里了!!!" +
                           LoginUtil.GetIntance().APPToAPPUserInfo.openId);
			MyInfo myInfo = (MyInfo)Facade.GetFacade().data.Get(FacadeConfig.USERINFO_MODULE_ID);
			Debug.Log("PhoneNumber " + myInfo.isPhoneNumberLogin);
			if (LoginUtil.GetIntance().APPToAPPUserInfo.openId == "APPToAPP")
			{
				Debug.Log("openId = APPToAPP");
				//MyInfo myInfo = LoginUtil.GetIntance().APPToAPPUserInfo;
				PB_CLLoginTokenRequest nClLogin = new PB_CLLoginTokenRequest();
				nClLogin.ChannelNumber = getChannelNumber();

				nClLogin.DeviecType = GetDeviceType();
				nClLogin.MachineSerial = SystemInfo.deviceUniqueIdentifier;
				nClLogin.Token = myInfo.acessToken;
				nClLogin.UserId = myInfo.userID;
				nClLogin.Username = myInfo.nickname;
				nClLogin.Avatar = myInfo.avatar;
				nClLogin.VersionNumber = AppInfo.version;
				//Debug.LogError("--nClLogin.Avatar--" + nClLogin.Avatar);
				//Debug.LogError("--nClLogin.TokenTyp--" + nClLogin);
				DataControl.GetInstance().PushSocketSndByte(FiEventType.SEND_LOGIN_TOKEN_REQUEST, nClLogin.ToByteArray());
			}
            else if (myInfo.isPhoneNumberLogin)
            {
                Debug.Log("PhoneNumber");
				//MyInfo myInfo = (MyInfo)Facade.GetFacade().data.Get(FacadeConfig.USERINFO_MODULE_ID);
				PB_CLLoginTokenRequest nClLogin = new PB_CLLoginTokenRequest();
				nClLogin.ChannelNumber = getChannelNumber();

				nClLogin.DeviecType = GetDeviceType();
				nClLogin.MachineSerial = SystemInfo.deviceUniqueIdentifier;
				nClLogin.Token = myInfo.acessToken;
				//Debug.LogError ( "nClLogin.Token ： " + nClLogin.Token );
				nClLogin.UserId = myInfo.userID;
				if(myInfo.Associate_Nickname.Count>0)
				{
					nClLogin.Username = "" + myInfo.Associate_Nickname[myInfo.associateIndex];
				}
                else
				{
					nClLogin.Username = myInfo.nickname;
				}
				nClLogin.Avatar = myInfo.avatar;
				//Debug.LogError("--nClLogin.Avatar--" + nClLogin.Avatar);
				Debug.Log("openId = else platformType =" + myInfo.platformType);
				if (myInfo.platformType == 9)
				{
					nClLogin.TokenType = 1;
					nClLogin.Username = "";
				}
				else if (myInfo.platformType == 22)
				{
					nClLogin.TokenType = 5;
				}
				else if (myInfo.platformType == 24)
				{
					nClLogin.TokenType = 5;
				}
				else if (myInfo.platformType == 8)
				{
					nClLogin.TokenType = 8;
				}
				else{
					nClLogin.TokenType = 5;
				}

				nClLogin.VersionNumber = AppInfo.version;
				//Debug.LogError ( "--nClLogin.TokenTyp--" + nClLogin );
				DataControl.GetInstance().PushSocketSndByte(FiEventType.SEND_LOGIN_TOKEN_REQUEST, nClLogin.ToByteArray());
			}
            else
			{
				Debug.Log("openId = else");
				MyInfo nUserInfo = (MyInfo)Facade.GetFacade().data.Get(FacadeConfig.USERINFO_MODULE_ID);
				if (!string.IsNullOrEmpty(nUserInfo.account))
				{
					Debug.Log("openId = else if");
					SendAccountLogin(nUserInfo.account, nUserInfo.password, GetDeviceType(), SystemInfo.deviceUniqueIdentifier);
					return;
				}
				//MyInfo myInfo = (MyInfo)Facade.GetFacade().data.Get(FacadeConfig.USERINFO_MODULE_ID);
				PB_CLLoginTokenRequest nClLogin = new PB_CLLoginTokenRequest();
				try
				{
					nClLogin.ChannelNumber = getChannelNumber();
					nClLogin.DeviecType = GetDeviceType();
					nClLogin.MachineSerial = SystemInfo.deviceUniqueIdentifier;
					nClLogin.Token = myInfo.mLoginData.token;
					//Debug.LogError ( "nClLogin.Token ： " + nClLogin.Token );
					nClLogin.UserId = myInfo.mLoginData.userid;
					nClLogin.Username = myInfo.nickname;
					nClLogin.Avatar = myInfo.avatar;
				}
				catch {
					LoginUtil.GetIntance().ForStopIE();
				}
				//Debug.LogError("--nClLogin.Avatar--" + nClLogin.Avatar);
				Debug.Log("openId = else platformType =" + myInfo.platformType);
				if (myInfo.platformType == 9)
				{
					nClLogin.TokenType = 1;
					nClLogin.Username = "";
				}
				else if (myInfo.platformType == 22)
				{
					nClLogin.TokenType = 2;
				}
				else if (myInfo.platformType == 24)
				{
					nClLogin.TokenType = 3;
				}
				else if (myInfo.platformType == 8)
				{
					nClLogin.TokenType = 8;
				}
				else
				{
					nClLogin.TokenType = 5;
				}

				nClLogin.VersionNumber = AppInfo.version;
				//Debug.LogError ( "--nClLogin.TokenTyp--" + nClLogin );
				DataControl.GetInstance().PushSocketSndByte(FiEventType.SEND_LOGIN_TOKEN_REQUEST, nClLogin.ToByteArray());
			}
			
		}



		private void OnRecvLoginResponse (object data)
		{
			FiLoginResponse loginResponse = (FiLoginResponse)data;
			MyInfo myInfo = (MyInfo)Facade.GetFacade ().data.Get (FacadeConfig.USERINFO_MODULE_ID);
			if (loginResponse.reuslt == 0) {
				//登陆成功过，重连后发送登陆的消息
				if (null != myInfo) {
					myInfo.userID = loginResponse.userId;
					myInfo.nickname = loginResponse.nickname;
					myInfo.avatar = loginResponse.avatar;
					myInfo.levelVip = loginResponse.vipLevel;
					myInfo.level = loginResponse.level;
					myInfo.experience = loginResponse.experience;
					myInfo.gold = loginResponse.gold;
					myInfo.diamond = loginResponse.diamond;
					myInfo.topupSum = loginResponse.topupSum;
					myInfo.cannonMultipleMax = loginResponse.maxCanonMultiple;
					myInfo.cannonMultipleNow = myInfo.cannonMultipleMax;

					Tool.Log ("InfoMsg 用户ID：" + myInfo.userID);
					Tool.Log ("InfoMsg 昵称：" + myInfo.nickname);
					Tool.Log ("InfoMsg 炮等级：" + myInfo.cannonMultipleMax);
					Tool.Log ("InfoMsg avatar：" + myInfo.avatar);
				}
				//DataControl.GetInstance().PushSocketSnd (FiEventType.SEND_BACKPACK_REQUEST, null);
				DataControl.GetInstance ().PushSocketSnd (FiEventType.START_HEART_BEAT, null);
				//DataControl.GetInstance ().GetMyInfo ().SetState ( MyInfo.STATE_IN_HALL );
				AppControl.ToView (AppView.HALL);
			}

		}


		private void RcvConnectSuccess (object nValue)
		{
			/*FiNetworkInfo nInfo = (FiNetworkInfo)nValue;
			//如果是重连后的成功，重新发送登陆消息
			Debug.LogError("------------RcvConnectSuccess----------- need send login message : " +  (nInfo.nConnectCount!= 0 ));
			if ( nInfo.nConnectCount != 0 )
			{
				SendLoginRequest ();
			}*/
		}

		/*private void RcvPKHaveDisconnectedRoomInform(object data)
		{
			DataControl.GetInstance().PushSocketSnd (FiEventType.SEND_RECONNECT_GAME_REQUEST, null);
		}*/

		private void IMRcvPKReconnectGameResponse (object data)
		{
			/*FiReconnectResponse reconnect = (FiReconnectResponse)data;
			RoomInfo roomInfo = (RoomInfo)Facade.GetFacade ().data.Get ( FacadeConfig.ROOMINFO_MODULE_ID );
			roomInfo.roomType = reconnect.roomType;
			roomInfo.goldType = reconnect.goldType;
			roomInfo.roomIndex = reconnect.roomIndex;
			if(null!=reconnect.others)
			{
				foreach(FiOtherGameInfo gameInfo in reconnect.others)
				{
					roomInfo.AddUserReconnect (gameInfo);
				}
			}

			MyInfo myInfo = (MyInfo)Facade.GetFacade ().data.Get ( FacadeConfig.USERINFO_MODULE_ID );
			myInfo.SetProperties (reconnect.properties);*/
		}

		//只有在连接成功后，网络异常的情况下才能收到这条消息
		/*public void RcvConnectionClose( object data)
		{
			Debug.LogError("------------RcvConnectionClose-----------!!!!!!!!!!!" );
			//连接断开啦，那么跳到登陆页面哦
			MyInfo myInfo = (MyInfo)Facade.GetFacade ().data.Get ( FacadeConfig.USERINFO_MODULE_ID );
			myInfo.SetState( MyInfo.STATE_IN_LOGIN );
			AppControl.ToView (AppView.LOGIN);
			Facade.GetFacade ().DestroyAll ();
		}*/

		public void OnDestroy ()
		{
			EventControl mControl = EventControl.instance ();
			mControl.removeEventHandler (FiEventType.RECV_LOGIN_RESPONSE, OnRecvLoginResponse);
			mControl.removeEventHandler (FiEventType.CONNECT_SUCCESS, RcvConnectSuccess);
			//mControl.removeEventHandler (  FiEventType.RECV_HAVE_DISCONNECTED_ROOM_INFORM , RcvPKHaveDisconnectedRoomInform );
			//mControl.removeEventHandler (  FiEventType.RECV_RECONNECT_GAME_RESPONSE , IMRcvPKReconnectGameResponse );
			//mControl.removeEventHandler (  FiEventType.CONNECTIONT_CLOSED,   RcvConnectionClose );//只有在登陆成功后，才会
		}
	}
}

