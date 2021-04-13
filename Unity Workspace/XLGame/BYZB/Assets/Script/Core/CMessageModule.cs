using System;
using UnityEngine;
using System.Collections;
using System.Runtime.InteropServices;
using System.Net;
using System.Net.Sockets;
using System.Threading;
using System.IO;
using System.Collections.Generic;

namespace AssemblyCSharp
{
	public class CMessageModule
	{

		private Dictionary<int , IMsgHandle> mHandleMap = new Dictionary<int, IMsgHandle> ();

		public CMessageModule ()
		{
		}

		private IMsgHandle Get (int nType)
		{
			return mHandleMap [nType];
		}

		public LoginMsgHandle login {
			get { return  (LoginMsgHandle)Get (FacadeConfig.LOGIN_MODULE__ID); }
		}

		public BackPackMsgHandle backpack {
			get { return  (BackPackMsgHandle)Get (FacadeConfig.BACKPACK_MODULE_ID); }
		}

		public FishingClassicMsgHandle fishClassical {
			get { return  (FishingClassicMsgHandle)Get (FacadeConfig.FISHING_CLASSICAL_MODULE_ID); }
		}

		public FishingCommonMsgHandle fishCommom {
			get { return  (FishingCommonMsgHandle)Get (FacadeConfig.FISHING_COMMON_MODULE_ID); }
		}

		public FishingFriendMsgHandle fishFriend {
			get { return  (FishingFriendMsgHandle)Get (FacadeConfig.FISHING_FRIEND_MODULE_ID); }
		}

		public FishingPkRoomMsgHandle fishPkRoom {
			get { return  (FishingPkRoomMsgHandle)Get (FacadeConfig.FISHING_PKROOM_MODULE_ID); }
		}

		public FriendMsgHandle friend {
			get { return  (FriendMsgHandle)Get (FacadeConfig.FRIEND_MODULE_ID); }
		}

		public MailMsgHandle mail {
			get { return  (MailMsgHandle)Get (FacadeConfig.MAIL_MODULE_ID); }
		}

		public LuckyDrawMsgHandle luckyDraw {
			get { return  (LuckyDrawMsgHandle)Get (FacadeConfig.LUCKYDRAW_MODULE_ID); }
		}

		public PurchaseMsgHandle toolPruchase {
			get { return  (PurchaseMsgHandle)Get (FacadeConfig.PURCHASE_MODULE_ID); }
		}

		public RankMsgHandle rank {
			get { return  (RankMsgHandle)Get (FacadeConfig.RANK_MODULE_ID); }
		}

		public RedPacketMsgHandle redPacket {
			get { return  (RedPacketMsgHandle)Get (FacadeConfig.FISHING_REDPACKET_MODULE_ID); }
		}

		public StartGiftMsgHandle gift {
			get { return  (StartGiftMsgHandle)Get (FacadeConfig.START_GIFT_MODULE_ID); }
		}

		public TaskMsgHandle task {
			get { return  (TaskMsgHandle)Get (FacadeConfig.TASK_MODULE_ID); }
		}

		public DailySignMsgHandle signIn {
			get { return (DailySignMsgHandle)Get (FacadeConfig.SIGN_IN_MODULE_ID); }
		}

		public BankMsgHandle bank {
			get { return (BankMsgHandle)Get (FacadeConfig.UI_BANk_MOUDLE_ID); }
		}

		public BroadCastMsgHandle broadcast {
			get { return (BroadCastMsgHandle)Get (FacadeConfig.BROADCAST_MODULE_ID); }
		}

		public CopyCode code {
			get{ return (CopyCode)Get (FacadeConfig.UI_BIND_PHONE_ID); }
		}

		public RewardMsgHandle reward {
			get{ return (RewardMsgHandle)Get (FacadeConfig.REWARD_ID); }
		}

		public DarGonMsgHandle dargoncard {
			get{ return (DarGonMsgHandle)Get (FacadeConfig.UI_DRAGONCARD); }
		}

		public NobelMsgHandle nobel {
			get{ return (NobelMsgHandle)Get (FacadeConfig.UI_NOBEL_ID); }
		}

		public SevenSignMsgHandle sevensign {
			get{ return (SevenSignMsgHandle)Get (FacadeConfig.UI_SEVENPART); }
		}

		public UpLevelMsgHandle upLevel
		{
			get { return (UpLevelMsgHandle)Get(FacadeConfig.UI_UPLEVEL); }
		}

		public PayInfoBody payallinfo
		{
			get { return (PayInfoBody)Get(FacadeConfig.UI_PAY_INFO); }
		}

		//public UpLevelMsgHandle upLevelget
		//{
		//    get { return (UpLevelMsgHandle)Get(FacadeConfig.UI_UPLEVEL_REWARD); }
		//}

		public void InitModule ()
		{
			
			LoginMsgHandle nLoginHandle = new LoginMsgHandle ();
			Add (FacadeConfig.LOGIN_MODULE__ID, nLoginHandle);

			MailMsgHandle nMailHandle = new MailMsgHandle ();
			Add (FacadeConfig.MAIL_MODULE_ID, nMailHandle);

			FishingCommonMsgHandle nFishCommonHandle = new FishingCommonMsgHandle ();
			Add (FacadeConfig.FISHING_COMMON_MODULE_ID, nFishCommonHandle);

			StartGiftMsgHandle nGiftHandle = new StartGiftMsgHandle ();
			Add (FacadeConfig.START_GIFT_MODULE_ID, nGiftHandle);

			PurchaseMsgHandle nPurchaseHandle = new PurchaseMsgHandle ();
			Add (FacadeConfig.PURCHASE_MODULE_ID, nPurchaseHandle);

			DailySignMsgHandle nSignInHandle = new DailySignMsgHandle ();
			Add (FacadeConfig.SIGN_IN_MODULE_ID, nSignInHandle);

			FriendMsgHandle nFriendHandle = new FriendMsgHandle ();
			Add (FacadeConfig.FRIEND_MODULE_ID, nFriendHandle);

			TaskMsgHandle nTaskHandle = new TaskMsgHandle ();
			Add (FacadeConfig.TASK_MODULE_ID, nTaskHandle);

			BackPackMsgHandle nBackHandle = new BackPackMsgHandle ();
			Add (FacadeConfig.BACKPACK_MODULE_ID, nBackHandle);

			RankMsgHandle nRank = new RankMsgHandle ();
			Add (FacadeConfig.RANK_MODULE_ID, nRank);

			FishingPkRoomMsgHandle nPkHandle = new FishingPkRoomMsgHandle ();
			Add (FacadeConfig.FISHING_PKROOM_MODULE_ID, nPkHandle);

			FishingFriendMsgHandle nFishFriendHandle = new FishingFriendMsgHandle ();
			Add (FacadeConfig.FISHING_FRIEND_MODULE_ID, nFishFriendHandle);


			RedPacketMsgHandle nRedPacketHandle = new RedPacketMsgHandle ();
			Add (FacadeConfig.FISHING_REDPACKET_MODULE_ID, nRedPacketHandle);

			BroadCastMsgHandle nBroadCastHandle = new BroadCastMsgHandle ();
			Add (FacadeConfig.BROADCAST_MODULE_ID, nBroadCastHandle);

			LuckyDrawMsgHandle nLuckyMsgHanlde = new LuckyDrawMsgHandle ();
			Add (FacadeConfig.LUCKYDRAW_MODULE_ID, nLuckyMsgHanlde);

			BankMsgHandle nBank = new BankMsgHandle ();
			Add (FacadeConfig.UI_BANk_MOUDLE_ID, nBank);

			CopyCode GetBindPhone = new CopyCode ();
			Add (FacadeConfig.UI_BIND_PHONE_ID, GetBindPhone);

			RewardMsgHandle rewardMsg = new RewardMsgHandle ();
			Add (FacadeConfig.REWARD_ID, rewardMsg);

			DarGonMsgHandle dargonMsg = new DarGonMsgHandle ();
			Add (FacadeConfig.UI_DRAGONCARD, dargonMsg);

			NobelMsgHandle nobelmsg = new NobelMsgHandle ();
			Add (FacadeConfig.UI_NOBEL_ID, nobelmsg);

			SevenSignMsgHandle sevensignmsg = new SevenSignMsgHandle ();
			Add (FacadeConfig.UI_SEVENPART, sevensignmsg);

            UpLevelMsgHandle uplevelmsg = new UpLevelMsgHandle();
            Add(FacadeConfig.UI_UPLEVEL, uplevelmsg);

			PayInfoBody payallinfo = new PayInfoBody();
			Add(FacadeConfig.UI_PAY_INFO, payallinfo);

   //         UpLevelMsgHandle uplevelmsg2 = new UpLevelMsgHandle();
			//Add(FacadeConfig.UI_UPLEVEL_REWARD, uplevelmsg2);
			/*FishingClassicMsgHandle nClzHandle = new FishingClassicMsgHandle ();
			Add ( FacadeConfig.FISHING_CLASSICAL_MODULE_ID , nClzHandle );
			Debug.LogError ( MD_TAG + " message init complete !!! " );*/
		}

		private const string MD_TAG = "[ message module ]";

		public void DestroyModule ()
		{
			Dictionary<int , IMsgHandle>.Enumerator nEumDic = mHandleMap.GetEnumerator ();
			while (nEumDic.MoveNext ()) {
				nEumDic.Current.Value.OnDestroy ();
			}
			mHandleMap.Clear ();
		}

		public void Add (int nType, IMsgHandle nEventHandle)
		{
			mHandleMap.Add (nType, nEventHandle);
			nEventHandle.OnInit ();
		}

		public void Remove (int nType)
		{
			if (mHandleMap.ContainsKey (nType)) {
				mHandleMap [nType].OnDestroy ();
				mHandleMap.Remove (nType);
			}
		}
	}
}

