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

	public class AppConfig
	{

		#if UNITY_IPHONE
		[ DllImport ("__Internal")]
		public static extern string GetDeviceName ();
		#endif

		//暂时废弃掉
		//		public bool isIphoneX ()
		//		{
		//			bool bIphonex = false;
		//			return false;
		//			float nRatio = 2;
		//			float nRatioScreen = (float)Screen.width / (float)Screen.height;
		//			//Debug.LogError ( "[------------]" + nRatioScreen + "/" + Screen.width + "/" + Screen.height );
		//			if (nRatioScreen >= nRatio) {
		//				return true;
		//			}
		//
		//			if (UnityEngine.Application.isEditor) {
		//				return bIphonex;
		//			}
		//			#if UNITY_IPHONE
		//			try {
		//				string nIphoneStr = GetDeviceName ();
		//				if (nIphoneStr.Equals ("iPhone10,3") || nIphoneStr.Equals ("iPhone10,6")) {
		//					bIphonex = true;
		//				}
		//			} catch {
		//				Debug.LogError ("[ iphone version ] error!!! ");
		//			}
		//			#endif
		//			return bIphonex;
		//		}

		public bool isIphoneX2 ()
		{
			bool bIphonex = false;
			float nRatio = 1.98f;
			float nRatioScreen = (float)Screen.width / (float)Screen.height;
//			Debug.LogError ("[------------]" + nRatioScreen + "/" + Screen.width + "/" + Screen.height);
			if (nRatioScreen >= nRatio) {
				return true;
			}

			if (UnityEngine.Application.isEditor) {
				return bIphonex;
			}
			#if UNITY_IPHONE
			try {
				string nIphoneStr = GetDeviceName ();
				Debug.Log ("nIphoneStr = " + nIphoneStr);
				if (nIphoneStr.Equals ("iPhone10,3") || nIphoneStr.Equals ("iPhone10,6")) {
					bIphonex = true;
				}
			} catch {
				Debug.LogError ("[ iphone version ] error!!! ");
			}
			#endif
			return bIphonex;
		}

	}

	public class Facade
	{
		private static Facade mInstacne = new Facade ();

		private CDataModule mDataModule;

		private CUiViewModule mUiModule;

		private CMessageModule mMsgModule;

		private AppConfig mConfig = new AppConfig ();

		private Facade ()
		{
			
		}

		public static Facade GetFacade ()
		{
			
			return mInstacne;
		}

		public CDataModule data {
			get { return mDataModule; }
		}

		public CUiViewModule ui {
			get { return mUiModule; }
		}

		public AppConfig config {
			get { return mConfig; }
		}

		public CMessageModule message {
			get { return mMsgModule; }
		}


		public void InitAll ()
		{
			mDataModule = new CDataModule ();
			mUiModule = new CUiViewModule ();
			mMsgModule = new CMessageModule ();

			mDataModule.InitModule ();
			mUiModule.InitModule ();
			mMsgModule.InitModule ();
		}

		public void DestroyAll ()
		{
			if (mMsgModule != null)
				mMsgModule.DestroyModule ();
			if (mUiModule != null)
				mUiModule.DestroyModule ();
			if (mDataModule != null)
				mDataModule.DestroyModule ();
			mMsgModule = null;
			mUiModule = null;
			mDataModule = null;
			//EventControl.instance ().ClearAllListener ();
		}

	}
}

