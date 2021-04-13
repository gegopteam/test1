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
	public class BackpackDataProvider
	{
		public BackpackDataProvider ()
		{
			
		}

		//获取道具数组
		public List<FiBackpackProperty> GetProperty()
		{
			BackpackInfo nInfo = DataControl.GetInstance ().GetBackpackInfo ();

			List<FiBackpackProperty> nResult = new List<FiBackpackProperty> ();
			List<FiBackpackProperty> nBackArray = nInfo.getInfoArray();

			for( int i = 0 ; i < nBackArray.Count ; i ++ )
			{
				FiBackpackProperty nSingle = new FiBackpackProperty ( nBackArray[ i ] );
				nResult.Add ( nSingle );
			}
			return nResult;
		}

		//获取单个道具信息 id 详见FiPropertyType
		public FiBackpackProperty GetPropertyOfId( int id )
		{
			BackpackInfo nInfo = DataControl.GetInstance ().GetBackpackInfo ();
			FiBackpackProperty nResult = nInfo.Get ( id );
			return nResult;
		}

	}
}

