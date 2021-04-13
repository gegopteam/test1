using System;
using System.Runtime.InteropServices;
using System.IO;
using UnityEngine;

namespace AssemblyCSharp
{
	public class FiProtoDecoder
	{
		public FiProtoDecoder ()
		{
		}

		public byte[] toByteContent( FiProtoType.Head nHead, byte[] srcData )
		{
			int nHeadLen = Marshal.SizeOf ( nHead );
			byte[] data = new byte[ nHead.data_length - nHeadLen ];
			Buffer.BlockCopy ( srcData , 16 , data , 0 , data.Length );
			return data;
		}

		public FiProtoType.Head? ByteToStruct(byte[] bytes )
		{
			int size = Marshal.SizeOf( new FiProtoType.Head() );
			if ( size > bytes.Length ){
				return null;
			}
			//分配结构体内存空间
			IntPtr structPtr = Marshal.AllocHGlobal(size);
			//将byte数组拷贝到分配好的内存空间
			Marshal.Copy(bytes, 0, structPtr, size);
			//将内存空间转换为目标结构体

			FiProtoType.Head obj = (FiProtoType.Head)Marshal.PtrToStructure(structPtr, typeof(FiProtoType.Head));
			//释放内存空间
			Marshal.FreeHGlobal(structPtr);
			return obj;
		}

		/*public FiProtoMessage decode<T>( byte[] dataIn )
		{
		/*	int nHeadLen = Marshal.SizeOf( new FiProtoType.Head() );
			if ( dataIn.Length < nHeadLen )
				return null;
			FiProtoType.Head? nHeadRead = ByteToStruct ( dataIn );
			if (nHeadRead != null) {
				FiProtoType.Head nHead = nHeadRead.GetValueOrDefault ();
				byte[]  nBody = new byte[ nHead.data_length - nHeadLen ];
				Buffer.BlockCopy ( dataIn , nHeadLen , nBody , 0 , nBody.Length );
				T result;
				try {
					using (MemoryStream ms = new MemoryStream()) {
						//将消息写入流中
						ms.Write (nBody, 0, nBody.Length);
						//将流的位置归0
						ms.Position = 0;
						//使用工具反序列化对象
						result = ProtoBuf.Serializer.Deserialize<T> (ms);
					}
				} catch (Exception ex) {  
					Debug.Log("反序列化失败: " + ex.ToString());
					return null;
				}
				return new FiProtoMessage<T>( nHead , result );
			}
			return null;
		}*/

	}
}

