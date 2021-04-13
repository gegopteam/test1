using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

namespace AssemblyCSharp
{

	public class BroadCastUnit
	{
		public int type;
		public string nickname;
		public string content;
	}

	public class BroadCastInfo : IDataProxy
	{
		private List<string> mNoticeList = new List<string> ();
		private List<BroadCastUnit> mGameInfoList = new List<BroadCastUnit> ();

		private List<BroadCastUnit> mUserMessageList = new List<BroadCastUnit> ();

		//保存七日的公告信息
		private List<BroadCastUnit> mUserSevenMessageList = new List<BroadCastUnit> ();
		
		//保存七日的公告信息
		private List<BroadCastUnit> mUserSevenMessageLastList = new List<BroadCastUnit> ();

		//保存七日的公告信息
		private List<BroadCastUnit> mUserSevenMessageLastRemoveList = new List<BroadCastUnit> ();

		//保存新手升級獎勵的公告信息
		private List<BroadCastUnit> mUserUpgradeMessageList = new List<BroadCastUnit>();

		//保存新手升級獎勵的公告信息
		private List<BroadCastUnit> mUserUpgradeMessageLastList = new List<BroadCastUnit>();

		//保存新手升級獎勵的公告信息
		private List<BroadCastUnit> mUserUpgradeMessageLastRemoveList = new List<BroadCastUnit>();

		private List<FiScrollingNotice> mRollMessageList = new List<FiScrollingNotice> ();

		private List<FiScrollingNotice> mRollMsgDB = new List<FiScrollingNotice> ();


		private string NextContent;
		string[] split = new string[]{ "<color=#f6fc29ff>", "</color>", "<color=#00eaffff>", "<color=#fff000ff>" };

		public BroadCastInfo ()
		{
		}

		public void OnAddData (int nType, object nData)
		{

		}

		public void OnInit ()
		{
			mGameInfoList.Clear ();
			mUserMessageList.Clear ();
			mNoticeList.Clear ();
			mRollMessageList.Clear ();
			mRollMsgDB.Clear ();
			mUserSevenMessageList.Clear ();
			mUserUpgradeMessageList.Clear();
		}

		public void OnDestroy ()
		{
			mGameInfoList.Clear ();
			mUserMessageList.Clear ();
			mUserSevenMessageList.Clear ();
			mNoticeList.Clear ();
			mRollMessageList.Clear ();
			mRollMsgDB.Clear ();
			mUserUpgradeMessageList.Clear();
		}

		public List<string> GetNoticeList ()
		{
			return mNoticeList;
		}

		public BroadCastUnit GetGameInfo ()
		{
			if (mGameInfoList.Count > 0) {
				BroadCastUnit nResult = mGameInfoList [0];
				mGameInfoList.RemoveAt (0);
				return nResult;
			}
			return null;
		}

		public BroadCastUnit GetUserMessage ()
		{
			if (mUserMessageList.Count > 0) {
				BroadCastUnit nResult = mUserMessageList [0];
				mUserMessageList.RemoveAt (0);
				return nResult;
			}
			return null;
		}

		public BroadCastUnit GetSevenDayUserMessage ()
		{
			Debug.Log ("cout" + mUserSevenMessageLastList.Count + "cout1+" + mUserSevenMessageLastRemoveList);
			if (mUserSevenMessageList.Count > 0) {
				Debug.LogError ("3333333333");
				BroadCastUnit nResult = mUserSevenMessageList [0];
				mUserSevenMessageList.RemoveAt (0);
				return nResult;
			} else {
				if (mUserSevenMessageLastList.Count > 0) {
					Debug.LogError ("11111111");
					BroadCastUnit nResult = mUserSevenMessageLastList [0];
					mUserSevenMessageLastRemoveList.Add (mUserSevenMessageLastList [0]);
					mUserSevenMessageLastList.RemoveAt (0);
					return nResult;
				} else {
					Debug.LogError ("222222222");
//					mUserSevenMessageLastList = mUserSevenMessageLastRemoveList;
					List<BroadCastUnit> item = mUserSevenMessageLastRemoveList;
					if (mUserSevenMessageLastRemoveList.Count == 1) {
						mUserSevenMessageLastList = item;
					}
					BroadCastUnit nResult = mUserSevenMessageLastList [0];
					mUserSevenMessageLastList.RemoveAt (0);
					return nResult;
				}
			}
			return null;
		}

		public BroadCastUnit GetLevelUpgradeMessage()
		{
			Debug.Log("cout " + mUserUpgradeMessageLastList.Count + " cout1+ " + mUserUpgradeMessageLastRemoveList);
			if (mUserUpgradeMessageList.Count > 0)
			{
				Debug.LogError("3333333333");
				BroadCastUnit nResult = mUserUpgradeMessageList[0];
				mUserUpgradeMessageList.RemoveAt(0);
				return nResult;
			}
			else
			{
				if (mUserUpgradeMessageLastList.Count > 0)
				{
					Debug.LogError("11111111");
					BroadCastUnit nResult = mUserUpgradeMessageLastList[0];
					mUserUpgradeMessageLastRemoveList.Add(mUserUpgradeMessageLastList[0]);
					mUserUpgradeMessageLastList.RemoveAt(0);
					return nResult;
				}
				else
				{
					Debug.LogError("222222222");
					List<BroadCastUnit> item = mUserUpgradeMessageLastRemoveList;
					if (mUserUpgradeMessageLastRemoveList.Count == 1)
					{
						mUserUpgradeMessageLastList = item;
					}
					BroadCastUnit nResult = mUserUpgradeMessageLastList[0];
					mUserUpgradeMessageLastList.RemoveAt(0);
					return nResult;
				}
			}
			return null;
		}

		/*
		public FiScrollingNotice GetRollMessage(){
			int time = (int)Time.time;
			for (int i = 0; i < mRollMessageList.Count; i++) {
				if (time % mRollMessageList [i].cycleInterval == 0) {
					return mRollMessageList [i];
				}
			}
			return null;
		}
*/
		public bool IsRollMsgIsEmpty ()
		{
			return mRollMsgDB.Count == 0;
		}

		public FiScrollingNotice GetRollMessage ()
		{
			if (mRollMsgDB.Count > 0) {
				FiScrollingNotice nResult = mRollMsgDB [0];
				mRollMsgDB.RemoveAt (0);
				return nResult;
			}
			return null;
		}

		public void AddRollMessage ()
		{
			int time = (int)Time.time;
			for (int i = 0; i < mRollMessageList.Count; i++) {
				if (time % mRollMessageList [i].cycleInterval == 0) {
					mRollMsgDB.Add (mRollMessageList [i]);
					NextContent = null;
					mNoticeList.Add (Tool.GetContent (null, mRollMessageList [i].content, out NextContent, true));
					if (NextContent != null) {
						string nextText;
						mNoticeList.Add (Tool.GetContent (null, NextContent, out nextText));
					}
				}
			}
		}

		public void SetRollMessageInfo (List<FiScrollingNotice> rollMsg)
		{
			mRollMessageList = rollMsg;
		}

		public void AddGameInfo (string content, int type)
		{
			BroadCastUnit nUnit = new BroadCastUnit ();
			nUnit.content = content;
			nUnit.type = type;
			mGameInfoList.Add (nUnit);
			NextContent = null;

			string[] newContent = content.Split (split, StringSplitOptions.RemoveEmptyEntries);
			StringBuilder strBuilder = new StringBuilder ();
			foreach (var str in newContent) {
				strBuilder.Append (str);
			}
			content = strBuilder.ToString ();

			mNoticeList.Add (Tool.GetContent (null, content, out NextContent, true));
			if (NextContent != null) {
				string nextText;
				mNoticeList.Add (Tool.GetContent (null, NextContent, out nextText));
			}
		}

		public void AddSevenDayMessage (string content, int type)
		{
			BroadCastUnit nUnit = new BroadCastUnit ();
			nUnit.content = content;
			nUnit.type = type;
			mUserSevenMessageList.Add (nUnit);

			if (mUserSevenMessageLastList.Count > 30) {
				Debug.LogError ("公告大于30条");
				mUserSevenMessageLastList.RemoveAt (0);
				mUserSevenMessageLastList.Add (nUnit);
			} else {
				mUserSevenMessageLastList.Add (nUnit);
			}
			mUserSevenMessageLastRemoveList = mUserSevenMessageLastList;
			Debug.LogError ("七日公告数量:" + mUserSevenMessageList.Count + "存储数量" + mUserSevenMessageLastList.Count + "muserremovelias" + mUserSevenMessageLastRemoveList.Count);
			NextContent = null;
		}

		public void AddUpgradeDayMessage(string content, int type)
		{
			BroadCastUnit nUnit = new BroadCastUnit();
			nUnit.content = content;
			nUnit.type = type;
			mUserUpgradeMessageLastList.Add(nUnit);

			if (mUserUpgradeMessageLastList.Count > 30)
			{
				Debug.LogError("公告大于30条");
				mUserUpgradeMessageLastList.RemoveAt(0);
				mUserUpgradeMessageLastList.Add(nUnit);
			}
			else
			{
				mUserUpgradeMessageLastList.Add(nUnit);
			}
			mUserUpgradeMessageLastRemoveList = mUserUpgradeMessageLastList;
			Debug.LogError("七日公告数量:" + mUserUpgradeMessageList.Count + "存储数量" + mUserUpgradeMessageLastList.Count + "muserremovelias" + mUserUpgradeMessageLastRemoveList.Count);
			NextContent = null;
		}

		public void AddUserMessage (string nickname, string content)
		{
			BroadCastUnit nUnit = new BroadCastUnit ();
			nUnit.content = content;
			nUnit.nickname = nickname;
			mUserMessageList.Add (nUnit);
			NextContent = null;

			string[] newContent = content.Split (split, StringSplitOptions.RemoveEmptyEntries);
			StringBuilder strBuilder = new StringBuilder ();
			foreach (var str in newContent) {
				strBuilder.Append (str);
			}
			content = strBuilder.ToString ();


			mNoticeList.Add (Tool.GetContent (Tool.GetName (nickname, 7), content, out NextContent, true, 1));
			if (NextContent != null) {
				string nextText;
				mNoticeList.Add (Tool.GetContent (null, NextContent, out nextText));
			}
		}

	}
}

