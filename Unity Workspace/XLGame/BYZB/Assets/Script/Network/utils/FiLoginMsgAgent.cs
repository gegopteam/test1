using UnityEngine;
using System.Collections;
using System.Runtime.InteropServices;
using System.Net;
using System.Net.Sockets;
using System;
using System.Threading;
using System.IO;
using System.Collections.Generic;
using Google.Protobuf;

namespace AssemblyCSharp
{
	public class FiLoginMsgAgent:MsgBaseAgent
	{
		public FiLoginMsgAgent ( NetControl nvalue )
		{
			SetDispatch ( nvalue );
		}

//		public void SendLoginRequest( DispatchData nDataIn )
//		{
//			byte[] nByteBody = FiProtoHelper.toProto_LoginRequest ( (  FiLoginRequest) nDataIn.data ).ToByteArray();
//			SendByteArray ( FiProtoType.FISHING_LOGIN_REQ ,nByteBody );
//		}

		public void RecvLoginResponse( byte[] data )
		{
			try {
				PB_LoginResponse nLoginResult = PB_LoginResponse.Parser.ParseFrom (data);  

				FiLoginResponse nLoginReply = FiProtoHelper.toLocal_LoginReply (nLoginResult);
				//Debug.LogError ( nLoginResult.GameId + "test [ network ] recv message== " + nLoginResult );
				mNetCtrl.setUserId (nLoginResult.UserId);
				mNetCtrl.dispatchEvent (FiEventType.RECV_LOGIN_RESPONSE, nLoginReply);
			} catch (Exception e) {
				Debug.Log ("[ network ] recv message== FISHING_LOGIN_RES error" + e.Message);
			}
		}

	}
}

