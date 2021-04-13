using System;
using System.Collections.Generic;

//author ysq
//date 2017/08/03
//服务器消息事件监听，用户通过对应接口，注册处理的handler，当客户端收到此数据后，处理相关对消息


public class EventControl
{
	private object mMutex = new object();

	private static EventControl mInstance = new EventControl();

	public delegate void FiEventHandler( object data );

	Dictionary<int ,List<FiEventHandler> >  mEventMap = new Dictionary<int, List<FiEventHandler>>();

	private EventControl ()
	{
//		addEventHandler ( 22, OnEventHandler  );
//		addEventHandler ( 22, OnEvent2Handler  );
//		DispatchData nData = new DispatchData ();
//		nData.type = 22;
//		nData.data = "lalaalalalalalalaltest";
//		this.dispatch ( nData );
//		removeEventHandler ( 22, OnEventHandler );
//		this.dispatch ( nData );
//		removeEventHandler ( 22, OnEvent2Handler );
//		this.dispatch ( nData );
	}

//	private void OnEvent2Handler( object o )
//	{
//		UnityEngine.Debug.LogError ("-------------OnEvent2Handler---------------" + o );
//	}
//
//	private void OnEventHandler( object o )
//	{
//		UnityEngine.Debug.LogError ("-------------EventControl---------------" + o );
//	}

	public void removeEventHandler( int nType , FiEventHandler nHandler )
	{
		lock (mMutex) {
			if (mEventMap.ContainsKey (nType)) {
				if (mEventMap [nType].Contains (nHandler)) {
					mEventMap [nType].Remove (nHandler);
				}
			}
		}
	}

	public void ClearAllListener()
	{
		mEventMap.Clear ();
		UnityEngine.Debug.LogError ( "-------ClearAllListener--------" + mEventMap.Count );
	}

	public static EventControl instance()
	{
		return  mInstance;
	}


	public void addEventHandler( int nEventType , FiEventHandler nHandle )
	{
		lock (mMutex) {
			if (!mEventMap.ContainsKey (nEventType)) {
				mEventMap.Add (nEventType, new List<FiEventHandler> ());
			}
			if( !mEventMap [nEventType].Contains(nHandle) )
				mEventMap [nEventType].Add ( nHandle );
		}
	}


	public void dispatch( DispatchData nData )
	{
		IEnumerator<FiEventHandler> nEum = null;
		lock (mMutex) {
			if (mEventMap.ContainsKey (nData.type)) {
				nEum = mEventMap [nData.type].GetEnumerator ();
			}
		}

		if (nEum != null) 
		{
			while( nEum.MoveNext() )
			{
				nEum.Current.Invoke ( nData.data );
			}
		}
	}

}

