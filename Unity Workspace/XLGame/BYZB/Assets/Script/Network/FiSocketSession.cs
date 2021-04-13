using System;
using System.Runtime.InteropServices;

using System.Collections;
using System.Net.Security;
using System.Net.Sockets;
using System.Security.Authentication;
using System.Text;
using System.Security.Cryptography.X509Certificates;

using System.Collections.Generic;

using AssemblyCSharp;

namespace AssemblyCSharp
{
	using UnityEngine;
	using System.Net;
	using System.Net.Sockets;
	using System;
	using System.Threading;
	using System.Text;

	using System.Collections.Generic;

	public class FiSocketSession : INetSession
	{
		private object mLockSocket = new object();
		private Socket mSocEntity = null;
		public static String TAG = "FiSocketSession";

		private byte[] mReadBuffer = new byte[8192];
		//	private int               mBufPosition = 0;

		private ConnectVars mConnectInfo;
		private OnNetworkListener mNetCallback;

		private bool mLinkState = false;

		private bool mIsSend = false;
		private object mLockData = new object();
		private List<byte[]> mDataArray = null;

		MyInfo myInfo;

		//private bool mIsActive = true;

		public FiSocketSession()
		{
			mDataArray = new List<byte[]>();
		}

		static bool mIsInIp4 = true;

		static string mConnectSvrUrl = "";

		public static string CheckNetWorkVersion(string nHostString)
		{
			Debug.LogError("----------- FiSocketSession CheckNetWorkVersion-----------");
			//string nTargetIPString = "www.baidu.com";
			string nTargetIPString = nHostString;//"www.baidu.com";
			//Debug.LogError("[ 55555555555socket network ] --- ip addr --- " + nTargetIPString);
			IPAddress[] address = Dns.GetHostAddresses(nTargetIPString);
			if (address[0].AddressFamily == AddressFamily.InterNetworkV6)
			{
				Debug.LogError("[ socket network ]------ipv6------");
				mIsInIp4 = false;
			}
			else if (address[0].AddressFamily == AddressFamily.InterNetwork)
			{
				Debug.LogError("[ socket network ]------ipv4------");
			}

			string nOutHost = address[0].ToString();//nHostString;
													//如果是ipv6 网络
			if (!mIsInIp4)
			{
				System.Net.IPHostEntry host;
				try
				{
					//host = System.Net.Dns.GetHostEntry (nHostString);
					host = System.Net.Dns.GetHostEntry(nOutHost);
					foreach (System.Net.IPAddress ip in host.AddressList)
					{
						if (ip.AddressFamily == System.Net.Sockets.AddressFamily.InterNetworkV6)
						{
							nOutHost = ip.ToString();
							break;
						}
					}
				}
				catch (Exception e)
				{
					Debug.LogErrorFormat("[ socket network ] error , GetIPAddress error: {0}", e.Message);
				}
			}
			//Debug.LogError("[ socket network ] --- ip addr --- " + nOutHost);
			mConnectSvrUrl = nOutHost;
			return nOutHost;
		}

		public bool isConnecting()
		{
			//Debug.LogError("----------- FiSocketSession isConnecting-----------");
			return mLinkState;
		}

		public bool isConnected()
		{
			
			if (mSocEntity != null)
			{
				return mSocEntity.Connected;
			}
			return false;
		}

		/**连接socket服务器*/
		public void connect(ConnectVars nVars)
		{
			if (mNetCallback == null)
			{
				return;
			}
            Debug.LogError("----------- FiSocketSession connect----------- 1");
            mLinkState = true;
			mConnectInfo = nVars;
			//Debug.LogError("-----------mIsInIp4-----------" + mIsInIp4 + " /" + mConnectInfo.serverAddress);
			if (mIsInIp4)
			{
				Debug.LogError("----------- FiSocketSession connect----------- if");
				mSocEntity = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
			}
			else
			{
				Debug.LogError("----------- FiSocketSession connect----------- else");
				mSocEntity = new Socket(AddressFamily.InterNetworkV6, SocketType.Stream, ProtocolType.Tcp);
			}

			Debug.LogError("----------- FiSocketSession connect----------- 2");
			mReadBuffer = new byte[4096];
			if (null == mSocEntity)
				return;

			Debug.LogError("----------- FiSocketSession connect-----------3");
			mNetCallback.onRecvMessage(FiEventType.CONNECT_START, null, 0);
			//Debug.Log("[ " + TAG + " ] server ip =" + mConnectSvrUrl + " server port=" + mConnectInfo.serverProt);
			//IPHostEntry IPHost = Dns.Resolve (mConnectSvrUrl);  
			//IPAddress[] addr = IPHost.AddressList;   
			//IPEndPoint ep = new IPEndPoint(addr[0],80);
			//Debug.Log ("[ " + TAG + " ] 222222server ip =" + addr [0] + " server port=" + mConnectInfo.serverProt);
			Debug.LogError("----------- FiSocketSession connect-----------4 ");
			IPEndPoint nEndPoint = new IPEndPoint(IPAddress.Parse(mConnectSvrUrl), mConnectInfo.serverProt);
			Debug.LogError("----------- FiSocketSession connect-----------5");
			//IPEndPoint nEndPoint = new IPEndPoint (addr [0], mConnectInfo.serverProt);
			mSocEntity.BeginConnect(nEndPoint, new System.AsyncCallback(OnSocketConnected), mSocEntity);
			Debug.LogError("----------- FiSocketSession connect-----------6");
		}

		/**设置网络状态监听*/
		public void setStateListener(OnNetworkListener nNetCallback)
		{
			Debug.LogError("----------- FiSocketSession setStateListener-----------");
			mNetCallback = nNetCallback;
		}

		void OnSocketConnected(System.IAsyncResult ar)
		{
			Debug.LogError("----------- FiSocketSession OnSocketConnected-----------");
			myInfo = (MyInfo)Facade.GetFacade().data.Get(FacadeConfig.USERINFO_MODULE_ID);
			OnNetworkListener nNetState = mNetCallback;
			if (nNetState == null)
			{
				Debug.Log("[" + TAG + "] OnSocketConnected  nNetState" + " // " + nNetState);
				return;
			}
			Debug.Log("[" + TAG + "] OnSocketConnected  nNetState" + " // " + nNetState);
			Socket nSocket = ar.AsyncState as Socket;
			nSocket.EndConnect(ar);
			if (nSocket.Connected && !myInfo.isPhoneNumberLogin)
			{
				Debug.Log("[" + TAG + "] OnSocketConnected  success" + " // " + mReadBuffer.Length);

				nNetState.onRecvMessage(FiEventType.CONNECT_SUCCESS, null, 0);
				//				lock (mLockSocket) {
				//					nSocket.BeginReceive (mReadBuffer, 0, mReadBuffer.Length, SocketFlags.None, new System.AsyncCallback (OnRecvMessage), nSocket);
				//				}
			}
			else if (nSocket.Connected && myInfo.isPhoneNumberLogin)
            {
				Debug.Log("[" + TAG + "] OnSocketConnected  success" + " // " + mReadBuffer.Length);
				nNetState.onRecvMessage(FiEventType.CONNECT_SUCCESS_WITHLOGIN, null, 0);
			}
			else
			{
				nNetState.onRecvMessage(FiEventType.CONNECT_FAIL, null, 0);
				mLinkState = false;
			}
		}

		public void ToRcv()
		{
			//Debug.LogError("----------- FiSocketSession ToRcv-----------");
			OnNetworkListener nNetState = mNetCallback;
			if (nNetState == null)
			{
				return;
			}

			int nRcv = 0;
			//			lock (mLockSocket) {
			if (mSocEntity.Poll(10, SelectMode.SelectRead))
			{
				nRcv = mSocEntity.Receive(mReadBuffer);
			}
			//			}
			if (nRcv > 0)
			{
				nNetState.onRecvMessage(FiEventType.RECV_MESSAGE, mReadBuffer, nRcv);
			}
		}

		/**收到服务器数据处理*/
		void OnRecvMessage(System.IAsyncResult ar)
		{
			Debug.LogError("----------- FiSocketSession OnRecvMessage-----------");
			OnNetworkListener nNetState = mNetCallback;
			if (nNetState == null)
			{
				return;
			}

			Socket nSocket = ar.AsyncState as Socket;
			int nRecvLen = 0;
			try
			{
				nRecvLen = nSocket.EndReceive(ar);
			}
			catch (Exception ex)
			{
				nNetState.onRecvMessage(FiEventType.CONNECTIONT_CLOSED, null, 0);
				Debug.Log("[  " + TAG + "  ]Rcv Error:" + ex.Message + " / " + ex);
			}

			//Debug.Log ( "["+TAG+"] ********************************************  recv length=== >" + nRecvLen + " / nSocket : ==" + nSocket.Connected ); 

			if (nRecvLen > 0)
			{
				nNetState.onRecvMessage(FiEventType.RECV_MESSAGE, mReadBuffer, nRecvLen);
			}

			

			//如果远程主机使用 Shutdown 方法关闭了 Socket 连接，并且所有可用数据均已收到 方法将立即完成并返回零字节。
			if (nSocket.Connected && nRecvLen > 0)
			{

				//Debug.Log ("[" + TAG + "]********************************************BeginReceive=== >" ); 
				lock (mLockSocket)
				{
					nSocket.BeginReceive(mReadBuffer, 0, mReadBuffer.Length, SocketFlags.None, new System.AsyncCallback(OnRecvMessage), nSocket);
				}

			}
			else
			{
				//Debug.Log ( "["+TAG+"] ********************************************  closeed=== >" ); 
				nNetState.onRecvMessage(FiEventType.CONNECTIONT_CLOSED, null, 0);
				close();
				mLinkState = false;
			}
		}


		private void EndDisconnect(IAsyncResult result)
		{
			Debug.Log("-----------EndDisconnect begin------------");
			try
			{
				mSocEntity.EndDisconnect(result);
				mSocEntity.Close();
			}
			catch
			{
				Debug.LogError("-----------EndDisconnect exception------------");
			}
			Debug.Log("-----------EndDisconnect end------------");
			//是否同步
			//bool sync = (bool)result.AsyncState;
			mSocEntity = null;
		}

		/**连接关闭处理*/
		public void close()
		{
			mNetCallback = null;
			Debug.LogError("-------------mSocEntity.Close();--------------");
			if (mSocEntity != null && mSocEntity.Connected)
			{
				mSocEntity.BeginDisconnect(true, EndDisconnect, false);
			}
			mLinkState = false;
		}

		void Snd(System.IAsyncResult ar)
		{
			//			OnNetworkListener nCurCallback = mNetCallback;
			//			if (nCurCallback == null)
			//				return;
			Socket socketSnd = ar.AsyncState as Socket;
			int nSend = socketSnd.EndSend(ar);

			ToSendBytes();
		}

		public bool SendIsOK()
		{
			bool isOK = false;
			if (mSocEntity.Poll(10, SelectMode.SelectWrite))
			{
				isOK = true;
			}
			return isOK;
		}

		public void sendBytes(byte[] dataIn)
		{
			if (null == dataIn)
				return;

			//			lock (mLockSocket) {
			//				if (mSocEntity.Poll (10, SelectMode.SelectWrite)) {
			mSocEntity.Send(dataIn); //发包，不做if判断阻塞发送不然就有可能丢包，比如：当前无法发送超时返回的话，此时要发送的数据包就丢了。
									 //				}
									 //			}

			//			//添加数据到 list 中
			//			lock (mLockData) {
			//				mDataArray.Add(dataIn);
			//			}
			//			//如果没有在发送，则进行数据发送
			//			if (!mIsSend)
			//				ToSendBytes();
			//            return;

			//            try
			//            {
			//                if (mSocEntity != null && mSocEntity.Connected) {
			//                    //Debug.Log ("send message length==>" + dataIn.Length );
			//                    mSocEntity.BeginSend (dataIn, 0, dataIn.Length, SocketFlags.None, new System.AsyncCallback(Snd), mSocEntity);
			//                } else {
			//                    Debug.Log ( "["+TAG+"] socket closed =======" );
			//                }
			//            }catch( Exception e ) {
			//                Debug.LogError ( "["+TAG+"] socket closed =======" );
			//            }
		}

		public void ToSendBytes()
		{
			byte[] dataIn = null;
			lock (mLockData)
			{
				while (mDataArray.Count > 0)
				{
					dataIn = mDataArray[0];
					mDataArray.RemoveAt(0);
					if (null != dataIn)
						break; //取一个非空值
				}
			}

			if (null == dataIn)
			{//如果取不到数据则返回
				mIsSend = false;
				return;
			}

			try
			{
				mIsSend = true;
				if (mSocEntity != null && mSocEntity.Connected)
				{
					//Debug.Log ("send message length==>" + dataIn.Length );
					mSocEntity.BeginSend(dataIn, 0, dataIn.Length, SocketFlags.None, new System.AsyncCallback(Snd), mSocEntity);
				}
				else
				{
					Debug.Log("[" + TAG + "] socket closed =======");
				}
			}
			catch (Exception e)
			{
				Debug.LogError("[" + TAG + "] socket closed =======");
			}
		}

	}
}

