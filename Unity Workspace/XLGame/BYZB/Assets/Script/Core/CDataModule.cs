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
	public class CDataModule
	{

		private Dictionary<int , IDataProxy> mDataMap = new Dictionary<int, IDataProxy> ();

		public CDataModule ()
		{
		}

		public void InitModule ()
		{

			AvatarInfo nAvatar = new AvatarInfo ();
			Add (FacadeConfig.AVARTAR_MODULE_ID, nAvatar);

			BackpackInfo nBackPack = DataControl.GetInstance ().GetBackpackInfo ();//new BackpackInfo ();
			Add (FacadeConfig.BACKPACK_MODULE_ID, nBackPack);

			FriendInfo nFriend = DataControl.GetInstance ().getFriendInfo (); //new FriendInfo ();
			Add (FacadeConfig.FRIEND_MODULE_ID, nFriend);

			RankInfo nRank = DataControl.GetInstance ().GetRankInfo ();//new RankInfo ();
			Add (FacadeConfig.RANK_MODULE_ID, nRank);

			MyInfo nUserInfo = DataControl.GetInstance ().GetMyInfo ();//new MyInfo ();
			Add (FacadeConfig.USERINFO_MODULE_ID, nUserInfo);

			TaskInfo nTaskInfo = DataControl.GetInstance ().getTaskInfo ();//new TaskInfo ();
			Add (FacadeConfig.TASK_MODULE_ID, nTaskInfo);

			RoomInfo nRoomInfo = DataControl.GetInstance ().GetRoomInfo ();//new RoomInfo ();
			Add (FacadeConfig.ROOMINFO_MODULE_ID, nRoomInfo);

			MailDataInfo nMailInfo = new MailDataInfo ();
			Add (FacadeConfig.MAIL_MODULE_ID, nMailInfo);

			ChatDataInfo nChatInfo = new ChatDataInfo ();
			Add (FacadeConfig.CHAT_MODULE_ID, nChatInfo);

			FriendChatInfo nFriendChatInfo = DataControl.GetInstance ().getFriendChatInfo ();
			Add (FacadeConfig.FRIENDCHAT_MODULE_ID, nFriendChatInfo);

			BroadCastInfo nCastInfo = new BroadCastInfo ();
			Add (FacadeConfig.BROADCAST_MODULE_ID, nCastInfo);

			BankInfo nBankInfo = new BankInfo ();
			Add (FacadeConfig.UI_BANk_MOUDLE_ID, nBankInfo);

			DragonCardInfo mDragonCardInfo = new DragonCardInfo ();
			Add (FacadeConfig.UI_DRAGONCARD, mDragonCardInfo);

			DailySignInfo mDailySignInfo = new DailySignInfo ();
			Add (FacadeConfig.SIGN_IN_MODULE_ID, mDailySignInfo);

			NobelInfo mNobelInfo = new NobelInfo ();
			Add (FacadeConfig.UI_NOBEL_ID, mNobelInfo);

			NewSevenDayInfo mSeveninfo = new NewSevenDayInfo ();
			Add (FacadeConfig.UI_SEVENPART, mSeveninfo);

            UpLevelInfo mUplevelinfo = new UpLevelInfo();
            Add(FacadeConfig.UI_UPLEVEL, mUplevelinfo);


			//GetButtonState mGetButtonState = new GetButtonState();
			//Add(FacadeConfig.UI_AND_BUTTON_CLOSE_OR_OPEN, mGetButtonState);

			//         UpLevelReward mUpLevelReward = new UpLevelReward();
			//Add(FacadeConfig.UI_UPLEVEL_REWARD, mUpLevelReward);

			Debug.LogError (MD_TAG + " init success !!! ");
		}

		private const string MD_TAG = "[ data module ]";

		public void DestroyModule ()
		{
			foreach (var value in mDataMap) {  
				value.Value.OnDestroy ();
			} 
			mDataMap.Clear ();
			Debug.LogError ("====================>  mDataMap.Count : " + mDataMap.Count);
		}


		public void Add (int nType, IDataProxy nDataProxy)
		{
			mDataMap.Add (nType, nDataProxy);
		}

		public IDataProxy Get (int nType)
		{
			if (mDataMap.ContainsKey (nType))
				return mDataMap [nType];
			return null;
		}

		public void Remove (int nType)
		{
			if (mDataMap.ContainsKey (nType))
				mDataMap.Remove (nType);
		}
	}
}

