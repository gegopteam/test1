using System;
using System.Runtime.InteropServices;
using System;
using System.IO;
using UnityEngine;

namespace AssemblyCSharp
{
	public class FiProtoMessage
	{
		/*public class LoginRequest
		{
			public Head mHead;

			public NetLoginRequest mRequest;

			public LoginRequest( long timeTick , string openId , string access_token )
			{
				mHead = new Head();
				mHead.message_id = FISHING_LOGIN_REQ;
				mHead.userid     = 0;
				mHead.timetick   = timeTick;

				mRequest = new NetLoginRequest(){open_id = openId, access_token = access_token };
			}
				
			public MessageEntity<NetLoginRequest> convert()
			{
				return new MessageEntity<NetLoginRequest> ( mHead , mRequest );
			}
		}*/


//		public class MessageEntity<T>
//		{
//			public Head head;
//
//			public T    body;
//
//			public MessageEntity( Head nHead , T nBody )
//			{
//				head = nHead;
//				body = nBody;
//			}
//		}

		//public FiProtoType.Head head;

		public FiProtoType.Head head;
		public byte[]           body;
		//public T                result;


		public FiProtoMessage(FiProtoType.Head nHead , byte[] nBody  )
		{
			head = nHead;
			body = nBody;
		}
	}
}

