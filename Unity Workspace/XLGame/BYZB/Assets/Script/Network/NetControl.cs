using UnityEngine;
using System.Collections;
using System.Runtime.InteropServices;
using AssemblyCSharp;
using System.Net;
using System.Net.Sockets;
using System;
using System.Threading;
using System.Text;
using System.IO;

using Google.Protobuf;

public class NetControl 
{
	private static object     mInsMutex       = new object ();

	private OnNetworkListener mNetListner     = null;
//	private static NetControl mManager        = null;

	private FiSocketThread mSockThread        = null;

	public static NetControl instance()
	{
		Debug.Log("----------------NetControl instance-----------------");
		return new NetControl ();
	}

	private bool isStoped = false;

	private NetControl()
	{
		Debug.Log("----------------NetControl-----------------");
		mNetListner = new NetStateListener ( this );
	}

	public void connect( string addr , int port )
	{
		Debug.Log ("----------------NetControl connect begin-----------------");
		if (mSockThread != null)
			return;
		mSockThread = new FiSocketThread ( this );
		ConnectVars nValue = new ConnectVars ();
		nValue.serverAddress = addr;
		nValue.serverProt = port;
		mSockThread.setConfig ( nValue , mNetListner );
		mSockThread.Setup ();
		isStoped = false;
		Debug.Log ("----------------NetControl connect end-----------------");
	}

	public bool isConnected(){
		return mSockThread.isConnected();
	}

	public void close()
	{
		if (null != mSockThread) {
			mSockThread.stop ();
			mSockThread = null;
		}
		isStoped = true;
	}

	public void setUserId( uint nUserId )
	{
		mSockThread.mUserId =(int) nUserId;
	}

	private string Tag = "[---------------dispatchEvent----------------]";

	public void SendBytes( int nMsgId ,  byte[] nByteSend )
	{
		DispatchData nDataIn = new DispatchData ();
		nDataIn.type = nMsgId;
		nDataIn.data = nByteSend;
		mSockThread.sendMessage ( nDataIn );
		//mSockThread.SendByteArray ( nMsgId, nByteSend );
	}

	public void dispatchEvent( int nType , object nValue )
	{
		if (isStoped)
			return;
		DispatchData nData = new DispatchData ();
		nData.type = nType;
		nData.data = nValue;
		//Debug.LogError ( " -dispatchEvent- " + nType );

		DataControl.GetInstance ().PushSocketRcv ( nData );
	}

	public void sendData( DispatchData nDataIn )
	{
		mSockThread.sendMessage ( nDataIn );
	}
}
