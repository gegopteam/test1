using System;
//using ProtoBuf;
using System.Runtime.InteropServices;
//using ProtoBuf.Meta;
//using ProtoBuf;
using System.IO;
using UnityEngine;
using Google.Protobuf;

namespace AssemblyCSharp
{
	public class FiProtoEncoder
	{
		public FiProtoEncoder ()
		{
		}

		public byte[] encode( FiProtoType.Head nHead, byte[] dataIn )
		{
			/**序列号头部字段*/
			int nHeadSize = Marshal.SizeOf( nHead );

			int nDataLen = dataIn == null ? 0 : dataIn.Length;

			nHead.data_length = (short)( nDataLen + nHeadSize );

			//创建byte数组
			byte[] bytes = new byte[ nDataLen + nHeadSize ];
			//分配结构体大小的内存空间
			IntPtr structPtr = Marshal.AllocHGlobal( bytes.Length );
			//将结构体拷到分配好的内存空间
			Marshal.StructureToPtr(nHead, structPtr, false);
			//从内存空间拷到byte数组
			Marshal.Copy(structPtr, bytes, 0, nHeadSize );
			//释放内存空间
			Marshal.FreeHGlobal(structPtr);

			if( nDataLen > 0 )
	    		Buffer.BlockCopy (dataIn, 0, bytes, nHeadSize, dataIn.Length);
			return bytes;
		}

	}
}

