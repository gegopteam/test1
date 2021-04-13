using System;
using System.Collections.Generic;
using UnityEngine;

namespace AssemblyCSharp
{
	class AvatarDetail
	{
		public int userId;

		public bool isCallbackExcuted = false;

		public byte[] pngCodedData = null;

		public int width;

		public int height;

		public string nUrl;

		public List<SocketToUI.OnHttpCompelete> nCallBackArray = new List<SocketToUI.OnHttpCompelete>();

	//	public SocketToUI.OnHttpCompelete nCallBack;
	}

	public class AvatarInfo : IDataProxy
	{
		private Dictionary< int , AvatarDetail > mAvatarMap = new Dictionary< int, AvatarDetail >();

		private AvatarDetail mLastDetail = null;

		public AvatarInfo ()
		{
			
		}

		public void OnAddData( int nType , object nData )
		{
			
		}

		public void OnInit()
		{
		}

		public void OnDestroy()
		{
			
		}

		Texture2D ScaleTexture(Texture2D source, int targetWidth, int targetHeight)
		{
			Texture2D result = new Texture2D(targetWidth, targetHeight, source.format, false);

			float incX = (1.0f / (float)targetWidth);
			float incY = (1.0f / (float)targetHeight);

			for (int i = 0; i < result.height; ++i)
			{
				for (int j = 0; j < result.width; ++j)
				{
					Color newColor = source.GetPixelBilinear((float)j / (float)result.width, (float)i / (float)result.height);
					result.SetPixel(j, i, newColor);
				}
			}

			result.Apply();
			return result;
		}

		private void OnImageLoadingResponse( int nResult , Texture2D nImage )
		{
			//如果图像数据加载失败了
			if (nResult != 0) {
				mAvatarMap.Remove ( mLastDetail.userId );
			} else {
				mLastDetail.isCallbackExcuted = true;
				if (nImage.width > 100 && nImage.height > 100) {
					mLastDetail.width = 100;//nImage.width;
					mLastDetail.height = 100;//nImage.height;
					nImage = ScaleTexture (nImage, 100, 100);
				} else {
					mLastDetail.width = nImage.width;
					mLastDetail.height = nImage.height;
				}
				mLastDetail.pngCodedData = nImage.EncodeToPNG ();
			}
			//Debug.LogError ( nResult + "-----OnImageLoadingResponse------" + mLastDetail.nCallBackArray.Count );
			if (mLastDetail.nCallBackArray != null) {
				List<SocketToUI.OnHttpCompelete>.Enumerator nBackEnum = mLastDetail.nCallBackArray.GetEnumerator ();
				while (nBackEnum.MoveNext ()) {
					nBackEnum.Current.Invoke ( nResult , nImage );
				}
				mLastDetail.nCallBackArray.Clear ();
			}
			mLastDetail = null;
			//如果还有没有加载的头像，那么继续加载
			foreach (KeyValuePair< int , AvatarDetail > nData in mAvatarMap)
			{
				//Debug.LogError ( nData.Value.userId + "-----OnImageLoadingResponse------" + mAvatarMap.Count + " / " + nData.Value.isCallbackExcuted );
				if ( !nData.Value.isCallbackExcuted ) {
					mLastDetail = nData.Value;
					SocketToUI.GetInstance ().LoadAvatar ( mLastDetail.nUrl , OnImageLoadingResponse );
					break;
				}
			}
//			Dictionary< int , AvatarDetail >.Enumerator nLoadEnum = mAvatarMap.GetEnumerator ();
//			while (nLoadEnum.MoveNext ()) {
//
//				Debug.LogError ( nLoadEnum.Current.Value.userId + "-----OnImageLoadingResponse------" + nLoadEnum.Current.Value.isCallbackExcuted );
//
//			}
		}

		private AvatarDetail AddTaskToMap( int userId , string url , SocketToUI.OnHttpCompelete nCallBack )
		{
			AvatarDetail nDetailInfo = new AvatarDetail ();
			nDetailInfo.isCallbackExcuted = false;
			nDetailInfo.userId    = userId;
			nDetailInfo.nUrl      = url;
			nDetailInfo.nCallBackArray.Add( nCallBack );
			mAvatarMap.Add ( userId , nDetailInfo );
			//Debug.LogError ( "[ avatar info ] " + userId + " / " + mAvatarMap.Count );
			return nDetailInfo;
		}

		public void Load( int userId , string nAvatarUrl , SocketToUI.OnHttpCompelete nCallBack  )
		{
			//获取已经加载过的 或者 正在 加载 的userid 头像
			if (string.IsNullOrEmpty (nAvatarUrl)) {
				nCallBack ( -1 ,  null );
				return;
			}

			if (mAvatarMap.ContainsKey (userId)) {
				//如果已经回调完成了，返回数据 , 数据正在加载中，那么更新回调接口
				if (mAvatarMap [userId].isCallbackExcuted) {
					Texture2D nImage = new Texture2D (mAvatarMap [userId].width, mAvatarMap [userId].height);
					nImage.LoadImage (mAvatarMap [userId].pngCodedData);
					nCallBack (0, nImage);
				} else {//正在加载中
					//Debug.LogError ( "--------[ avatar info ] add---------" );
					mAvatarMap [userId].nCallBackArray.Add( nCallBack );
				}
				return;
			}

			if ( string.IsNullOrEmpty (nAvatarUrl) ) {
				nCallBack ( -1 , null );
				//Debug.LogError ( "[ avatar info ]  user url error!!!!" + nAvatarUrl );
				return;
			}
			//Debug.LogError ( "[ avatar info ] " + nCallBack );
			//将新任务添加到加载列表中
			AvatarDetail nInfo =  AddTaskToMap (userId, nAvatarUrl, nCallBack);
			//如果当前没有加载任务，那么加载新任务
			if (mLastDetail == null) {
				mLastDetail = nInfo;
				SocketToUI.GetInstance ().LoadAvatar (nAvatarUrl, OnImageLoadingResponse);
			}
		}

	}
}

