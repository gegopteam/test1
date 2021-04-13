/* author:KinSen
 * Date:2017.05.23
 */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using System.Net;
using System.Net.Sockets;
using System;
using System.Threading;
using System.Text;

/*
 * 功能
 * 负责socket通信
 * 打包数据发送和接收数据解包
 * 
 */

//需要考虑ipv6的问题

public class SocketC {
	public string ipSvr;
	public int portSvr;

	private Socket socketClient = null;
	private Thread threadSocket = null;

	private string msgSnd = ""; //发送信息的队列，暂时用string代替
	private string msgRcv = ""; //接收信息的队列，暂时用string代替
	
	private static object objMsgSnd = new object(); //发送信息的信息队列锁
	private static object objMsgRcv = new object(); //接收信息的信息队列锁

	private byte[] dataSnd = null; //new byte[1024];
	private byte[] dataRcv = new byte[1024];

	public SocketC()
	{
		
	}

	~SocketC()
	{
		
	}

	public void setInfo(string ip, int port)
	{
		this.ipSvr = ip;
		this.portSvr = port;
	}

	public void Open()
	{
		Debug.Log ("Open Socket");
		socketClient = new Socket (AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);

		OpenSocket ();

		//socketClient.BeginSend (dataSnd, 0, dataSnd.Length, SocketFlags.None, new System.AsyncCallback(), socketClient);
		socketClient.BeginReceive (dataRcv, 0, dataRcv.Length, SocketFlags.None, new System.AsyncCallback(Rcv), socketClient);

	}

	public void Open(Socket socket)
	{
		socketClient = socket;
		ipSvr = socketClient.RemoteEndPoint.ToString ();

	}

	public void Close()
	{
		threadSocket.Abort ();
	}

	public void SndMsg(string msg)
	{
		dataSnd = Encoding.UTF8.GetBytes (msg);
		socketClient.BeginSend (dataSnd, 0, dataSnd.Length, SocketFlags.None, new System.AsyncCallback(Snd), socketClient);
		//AddSndMsgToList (msg);
	}

	void AddSndMsgToList(string msg)
	{
		lock(objMsgSnd)
		{
			msgSnd = msg;
		}
		//Debug.Log ("AddSndMsgToList: Snd");
		
		return;
	}

	string GetSndMsgFrList()
	{
		string msg = "";
		lock(objMsgSnd)
		{
			msg = msgSnd;
			msgSnd = "";
		}
		//Debug.Log ("GetSndMsgFrList: Snd");
		return msg;
	}

	void AddRcvMsgToList(string msg)
	{
		lock(objMsgRcv)
		{
			msgRcv = msg;
		}
		//Debug.Log ("AddRcvMsgToList: Rcv");
		return;
	}

	string GetRcvMsgFrList()
	{
		string msg = "";
		lock(objMsgRcv)
		{
			msg = msgRcv;
			msgRcv = "";
		}
		//Debug.Log ("AddRcvMsgToList: Rcv");
		return msg;
	}

	bool OpenSocket()
	{
		return Connect (ipSvr, portSvr);
	}

	void CloseSocket()
	{
		Disconnect ();
	}

	bool Connect(string ip, int port)
	{
		if (null == socketClient)
			return false;

		Debug.Log ("IP:"+ip+" port:"+port);
		
		IPEndPoint ipEndPoint = new IPEndPoint (IPAddress.Parse (ip), port);
		try
		{
			socketClient.Connect(ipEndPoint);
		}
		catch(SocketException ex)
		{
			Debug.Log ("Connect Error:" + ex.Message);
			return false;
		}
		
		return true;
	}

	void Disconnect()
	{
		if (null == socketClient)
			return;
		//关闭socket
	}

	void Snd(System.IAsyncResult ar)
	{
		Socket socketSnd = ar.AsyncState as Socket;
		socketSnd.EndSend (ar);

		return;
	}

	void Rcv(System.IAsyncResult ar)
	{
		Socket socketRcv = ar.AsyncState as Socket;
		int len = 0;
		try
		{
			len = socketRcv.EndReceive(ar);
		}
		catch(Exception ex)
		{
			Debug.Log ("Rcv Error:" + ex.Message);
		}

		if(len>0)
		{
			//string msg = Encoding.Default.GetString(dataRcv);
			string msg = System.Text.Encoding.UTF8.GetString (dataRcv).Substring (0, len);
			//添加信息到接收信息队列
			AddRcvMsgToList (msg);
		}
		socketRcv.BeginReceive (dataRcv, 0, dataRcv.Length, SocketFlags.None, new System.AsyncCallback(Rcv), socketRcv);

		return;
	}

	protected virtual void RcvMsg(string msg)
	{//由派生类来处理消息
	}

	//接收消息
	public void ToRcvMsg()
	{
		//Debug.Log ("TimerRcvMsg");
		string msg = "";
		msg = GetRcvMsgFrList ();
		if(""!=msg)
		{
			RcvMsg (msg);
			msg = "";
		}
	}

}
