using System;
using UnityEngine;
using System.Collections;
using System.Runtime.InteropServices;
using System.Net;
using System.Net.Sockets;
using System;
using System.Threading;
using System.IO;
using System.Collections.Generic;

namespace AssemblyCSharp
{
	public class CUiViewModule
	{
		private Dictionary<int , IUiMediator> mUiMap= new Dictionary<int, IUiMediator>();

		public CUiViewModule ()
		{
		}

		public void InitModule()
		{

		}

		public void DestroyModule()
		{
			foreach(var value in mUiMap)  
			{  
				value.Value.OnRelease ();
			} 
			mUiMap.Clear ();
		}

		public void Add( int nType , IUiMediator nUi )
		{
			if (mUiMap.ContainsKey (nType))
				mUiMap.Remove ( nType );
			mUiMap.Add ( nType , nUi );
			nUi.OnInit ();
		}

		public IUiMediator Get( int nType )
		{
			if( mUiMap.ContainsKey( nType ) )
				return mUiMap[ nType ];
			return null;
		}

		public void Remove( int nType )
		{
			if (mUiMap.ContainsKey (nType)) {
				mUiMap [nType].OnRelease ();
				mUiMap.Remove (nType);
			}
		}
	}
}

