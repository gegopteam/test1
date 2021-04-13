//using System;
using System.Runtime.InteropServices;
using System;
using System.IO;
using UnityEngine;

namespace AssemblyCSharp
{
	//基于 protobuf 的数据封装处理
	public class FiProtoType
	{
		public const int FISHING_HEARTBEAT = 0;
		public const int FISHING_LOGIN_REQ = 1;
		public const int FISHING_ENTER_CLASSICAL_ROOM_REQ = 2;
		public const int FISHING_LEAVE_CLASSICAL_ROOM_REQ = 3;
		public const int FISHING_NOTIFY_FIRE = 4;
		public const int FISHING_NOTIFY_ON_FISH_HIT = 5;
		public const int FISHING_NOTIFY_FISH_OUT_OF_SCENE = 6;
        //砲倍數修改
		public const int FISHING_CHANGE_CANNON_MULTIPLE_REQUEST = 7;
		public const int FISHING_EFFECT_REQUEST = 8;
		public const int FISHING_TOP_UP_REQUEST = 9;
        //背包刷新
		public const int FISHING_GET_BACKPACK_PROPERTY_REQUEST	= 10;
		public const int FISHING_PURCHASE_PROPERTY_REQUEST = 11;
		public const int FISHING_GET_FRIEND_LIST_REQUEST = 12;
		public const int FISHING_ADD_FRIEND_REQUEST = 13;
		public const int FISHING_ACCEPT_FRIEND_REQUEST = 14;

		public const int FISHING_ACCEPT_FRIEND_RESPONSE = 10038;

		public const int FISHING_DELETE_FRIEND_REQUEST = 15;
		//public const int      FISHING_GET_PKROOM_LIST_REQUEST		= 15;

		public const int FISHING_CREATE_PKROOM_REQUEST = 16;
		public const int FISHING_ENTER_PKROOM_REQUEST = 17;
		public const int FISHING_LEAVE_PKROOM_REQUEST = 18;
		public const int FISHING_START_PKGAME_REQUEST = 19;
		public const int FISHING_PK_EFFECT_REQUEST = 20;


		public const int FISHING_LOGIN_RES = 10001;
		public const int FISHING_ENTER_CLASSICAL_ROOM_RES = 10002;
		public const int FISHING_LEAVE_CLASSICAL_ROOM_RES = 10003;
		public const int FISHING_CHANGE_CANNON_MULTIPLE_RESPONSE = 10004;
		public const int FISHING_EFFECT_RESPONSE = 10005;
		public const int FISHING_TOP_UP_RESPONSE = 10006;
		public const int FISHING_GET_BACKPACK_PROPERTY_RESPONSE = 10007;
		public const int FISHING_PURCHASE_PROPERTY_RESPONSE	= 10008;
		public const int FISHING_GET_FRIEND_LIST_RESPONSE = 10009;
		public const int FISHING_ADD_FRIEND_RESPONSE = 10010;
		public const int FISHING_GET_PKROOM_LIST_RESPONSE = 10011;
		//		public const int      FISHING_CREATE_PKROOM_RESPONSE		= 10012;
		public const int FISHING_ENTER_PKROOM_RESPONSE = 10013;
		public const int FISHING_LEAVE_PKROOM_RESPONSE = 10014;
		public const int FISHING_START_PKGAME_RESPONSE = 10015;
		public const int FISHING_PK_EFFECT_RESPONSE = 10016;

        public const int XL_GET_HONG_BAO_GOLD = 15000;        //红包掉落

		public const int FISHING_NOTIFY_ENTER_CLASSICAL_ROOM	=	20001;
		public const int FISHING_NOTIFY_LEAVE_CLASSICAL_ROOM	=	20002;
		public const int FISHING_NOTIFY_FISH_GROUP = 20003;
		public const int FISHING_OTHER_CHANGE_CANNON_MULTIPLE	= 20004;
		public const int FISHING_OTHER_EFFECT = 20005;
		public const int FISHING_FREEZE_TIMEOUT =	20006;
		public const int FISHING_NOTIFY_ENTER_PKROOM =	20007;
		public const int FISHING_NOTIFY_LEAVE_PKROOM =	20008;
		public const int FISHING_NOTIFY_PKGAME_START =	20009;
		public const int FISHING_PK_DISTRIBUTE_PROPERTY =	20010;

		public const int FISHING_LAUNCH_TORPEDO_REQUEST =	21;
		public const int FISHING_PK_LAUNCH_TORPEDO_REQUEST = 22;
		public const int FISHING_TORPEDO_EXPLODE_REQUEST =	23;
		public const int FISHING_PK_TORPEDO_EXPLODE_REQUEST	=	24;

		public const int FISHING_LAUNCH_TORPEDO_RESPONSE =	10017;
		public const int FISHING_PK_LAUNCH_TORPEDO_RESPONSE	=	10018;
		public const int FISHING_TORPEDO_EXPLODE_RESPONSE = 10019;
		public const int FISHING_PK_TORPEDO_EXPLODE_RESPONSE	=	10020;

		public const int FISHING_NOTIFY_LAUNCH_TORPEDO = 20011;
		public const int FISHING_NOTIFY_PK_LAUNCH_TORPEDO = 20012;
		public const int FISHING_NOTIFY_TORPEDO_EXPLODE = 20013;
		public const int FISHING_NOTIFY_PK_TORPEDO_EXPLODE = 20014;
		public const int FISHING_OTHER_PK_EFFECT = 20015;
		public const int FISHING_PREPARE_PKGAME = 25;
		public const int FISHING_CANCEL_PREPARE_PKGAME = 26;
	
		public const int FISHING_PRE_START_COUNTDOWN = 20016;
		public const int FISHING_PK_GAME_COUNTDOWN = 20017;

		public const int FISHING_GOLD_GAME_RESULT = 20018;
		public const int FISHING_POINT_GAME_RESULT = 20019;

		public const int FISHING_POINT_GAME_ROUND_RESULT = 20020;

		public const int FISHING_RECONNECT_GAME_RESPONSE = 10021;

		public const int FISHING_RECONNECT_GAME_REQUEST =	27;

		public const int FISHING_CREATE_FRIEND_ROOM_REQUEST	=	16;

		public const int FISHING_CREATE_FRIEND_ROOM_RESPONSE	=	10012;

		public const int FISHING_ENTER_FRIEND_ROOM_REQUEST = 28;
		public const int FISHING_LEAVE_FRIEND_ROOM_REQUEST = 29;

		public const int FISHING_ENTER_FRIEND_ROOM_RESPONSE	= 10022;
		public const int FISHING_LEAVE_FRIEND_ROOM_RESPONSE	= 10023;

		public const int FISHING_NOTIFY_ENTER_FRIEND_ROOM = 20023;
		public const int FISHING_NOTIFY_LEAVE_FRIEND_ROOM = 20024;


		public const int FISHING_DISBAND_FRIEND_ROOM_REQUEST =	30;

		public const int FISHING_DISBAND_FRIEND_ROOM_RESPONSE = 10024;

		public const int FISHING_NOTIFY_DISBAND_FRIEND_ROOM =	20026;

		public const int FISHING_NOTIFY_HAVE_DISCONNECTED_ROOM =	20021;

		public const int FISHING_FRIEND_ROOM_GAME_RESULT =	20025;

		public const int FISHING_NOTIFY_RED_PACKET =	20027;
		  
		public const int FISHING_OPEN_RED_PACKET_REQUEST =	31;

		public const int FISHING_OPEN_RED_PACKET_RESPONSE = 10025;

		public const int FISHING_NOTIFY_OTHER_OPEN_RED_PACKET = 20030;


		public const int FISHING_ENTER_RED_PACKET_ROOM_REQUEST	= 32;
		public const int FISHING_LEAVE_RED_PACKET_ROOM_REQUEST	= 33;

		public const int FISHING_ENTER_RED_PACKET_ROOM_RESPONSE	= 10026;
		public const int FISHING_LEAVE_RED_PACKET_ROOM_RESPONSE	= 10027;

		public const int FISHING_NOTIFY_ENTER_RED_PACKET_ROOM = 20028;
		public const int FISHING_NOTIFY_LEAVE_RED_PACKET_ROOM = 20029;


		public const int FISHING_NOTIFY_FISH_TIDE_COMING = 20031;

		public const int FISHING_NOTIFY_FISH_TIDE_CLEAN_FISH = 20032;

		public const int FISHING_UNLOCK_CANNON_MULTIPLE_REQUEST	= 34;

        //解鎖砲台
		public const int FISHING_UNLOCK_CANNON_MULTIPLE_RESPONSE	= 10028;

		public const int FISHING_GET_RANK_REQUEST = 36;

		public const int FISHING_GET_RANK_RESPONSE = 10029;

		public const int FISHING_NOTIFY_LEVEL_UP = 20033;

		public const int FISHING_GET_START_GIFT_REQUEST = 37;

		public const int FISHING_NOTIFY_CHANGE_CANNON_STYLE = 20034;

		public const int FISHING_NOTIFY_START_GIFT = 20035;

		public const int FISHING_CHANGE_CANNON_STYLE_REQUEST = 35;

		public const int FISHING_GAME_RANK_RESPONSE = 10029;

		public const int FISHING_CHANGE_CANNON_STYLE_RESPONSE	= 10030;

		public const int FISHING_GET_START_GIFT_RESPONSE = 10031;

		public const int FISHING_GET_ADD_FRIEND_LIST_REQUEST = 38;

		public const int FISHING_GET_ADD_FRIEND_LIST_RESPONSE	= 10032;

		//public const int        FISHING_BEGINNER_TASK_RESPONSE			= 10033;

		public const int FISHING_DELETE_FRIEND_RESPONSE = 10011;

		public const int FISHING_EVERYDAY_TASK_REQUEST = 39;

		public const int FISHING_EVERYDAY_TASK_RESPONSE = 10034;

		public const int FISHING_EVERYDAY_TASK_PROGRESS_RESPONSE = 10035;

		public const int FISHING_EVERYDAY_TASK_PROGRESS_REQUEST = 42;

		public const int FISHING_REJECT_FRIEND_RESPONSE = 10036;

		public const int FISHING_REJECT_FRIEND_REQUEST = 40;

		public const int FISHING_RED_PACKET_COUNTDOWN = 20036;

		//道具赠送请求
		public const int FISHING_GIVE_OTHER_PROPERTY_REQUEST = 41;
		//道具赠送失败
		public const int FISHING_GIVE_OTHER_PROPERTY_RESPONSE = 10037;

		//获取系统邮件请求
		public const int FISHING_GET_MAIL_REQUEST = 83;
		//获取系统邮件反馈
		public const int FISHING_GET_MAIL_RESPONSE = 10079;
		//获取系统邮件内容并且删除，如果有道具，那么领取道具然后删除邮件
		public const int FISHING_DEL_MAIL_GET_AWARD_REQUEST = 86;
		//领取系统邮件反馈
		public const int FISHING_DEL_MAIL_GET_AWARD_RESPONSE = 10082;
		//获取赠送记录请求，（其他玩家赠送的）
		public const int FISHING_GET_GIVE_RECORD_REQUEST = 43;
		//领取其他玩家赠送的邮件记录
		public const int FISHING_GET_GIVE_RECORD_RESPONSE = 10039;
		//领取其他玩家赠送道具请求
		public const int FISHING_GET_GIVE_REQUEST = 44;
		//领取其他玩家赠送道具反馈
		public const int FISHING_GET_GIVE_RESPONSE = 10040;

		public const int FISHING_IOS_PAY_PROPERTY_REQUEST = 47;

		public const int FISHING_IOS_PAY_PROPERTY_RESPONSE = 10043;


		//		public const int        FISHING_SIGN_IN_AWARD_REQUEST	         = 49;

		public const int FISHING_SIGN_IN_AWARD_RESPONSE = 10044;

		//public const int        FISHING_NOTIFY_CHANGE_CANNON_STYLE		= 20034

		public const int FISHING_GET_RED_PACKET_LIST_REQUEST = 49;

		public const int FISHING_GET_RED_PACKET_LIST_RESPONSE	= 10045;

		public const int FISHING_NOTIFY_DECREASE_CONSUMED_GOLD	= 20037;

		public const int FISHING_GET_USER_INFO_REQUEST = 50;

		public const int FISHING_GET_USER_INFO_RESPONSE = 10046;

		public const int FISHING_EVERYDAY_ACTIVITY_AWARD_RESPONSE = 10047;

		public const int FISHING_OTHER_UNLOCK_CANNON_MULTIPLE	= 20038;

		public const int FISHING_SELL_PROPERTY_RESPONSE = 10048;

		public const int FISHING_NOTIFY_TOP_UP = 20039;

		//		public const int        FISHING_NOTIFY_OTHER_BEGINNER_REWARD	= 20041;
		//
		//		public const int        FISHING_NOTIFY_BEGINNER_TASK_PROGRESS	= 20040;
		//
		//		public const int        FISHING_BEGINNER_TASK_REWARD_RESPONSE	= 10033;
		//
		//		public const int        FISHING_BEGINNER_TASK_REWARD_REQUEST	= 53;
		//绑定手机请求
		public const int YK_CONVERSION_REQUEST = 77;
		//绑定手机回复
		public const int YK_CONVERSION_RESPONSE = 10069;
		//修改昵称请求
		public const int CL_MODIFY_NICK_REQUEST = 78;
		//修改昵称回复
		public const int CL_MODIFY_NICK_RESPONSE = 10070;
		//救济金任务
		public const int CL_HELP_GOLD_TASK = 10071;
		//领取救济金请求
		public const int CL_GET_HELP_TASK_REWARD_REQUEST = 79;
		//领取救济金回复
		public const int CL_GET_HELP_TASK_REWARD_RESPONSE = 10072;
		//补签
		public const int CL_SIGNRETROACTIVE_REQUEST = 80;
		//补签
		public const int CL_SIGNRETROACTIVE_REWARD_RESPONSE = 10073;
		//是否绑定手机回复
		public const int CL_BIND_PHONE_STATE_RESPONSE = 10074;

		//使用 限时道具
		public const int CL_USE_PROP_TIMEEX_REQUEST = 81;
		// 使用限时道具 消息回复
		public const int CL_USE_PROP_TIME_RESPONSE = 10075;
		// 删除 道具  只有接受消息
		public const int CL_DEL_PROP_TIME_RESPONSE = 10076;
		//接受消息-获取所有已经使用的 限时道具信息
		public const int CL_GET_ALL_PROP_TIME_RESPONSE = 10077;
		//获取奖励信息
		public const int XL_GET_REWARD_INFO = 10078;
		//龙卡请求
		public const int CL_DARGON_CARD_REQUEST = 84;
		//龙卡接收
		public const int CL_DARGON_CARD_RESPONSE = 10080;

		//购买龙卡接收信息
		public const int CL_PURCHASE_DARGON_CARD_RESPONSE = 20049;

		//特惠请求
		public const int CL_GET_PREFERENTIAL_REQUEST = 85;
		//特惠数据接收
		public const int XL_MONTHLY_GET_GIFT_BAG_INFO = 10081;
		//特惠购买成功后下发数据
		public const int FISHING_NOTIFY_BUY_SALE_GIFTBAG = 20050;

		//兑换炮座请求
		public const int XL_GET_EXCHANGEBARBETTE_REQUEST = 87;
		//兑换炮座回复
		public const int XL_GET_EXCHANGEBARBETTE_RESPONSE = 10083;
		//切换炮座样式请求
		public const int XL_CHANGEBARBETTESTYLE_REQUEST = 88;
		//机器人使用分身
		public const int XL_ROBOTREPLICATION_REQUEST = 89;
		//切换炮座样式回复
		public const int XL_CHANGEBARBETTESTYLE_RESPONSE = 10084;

		//财神点亮元宝接收
		public const int CL_MANMONSTARTSHOW_RESPONSE = 10085;
		//财神点亮元宝接收
		public const int CL_MANMONBETTINGSHOW_RESPONSE = 10086;
		//点击财神按钮请求下注数据
		public const int XL_MANMONSETTING_REQUEST = 90;
		//点击下注请求下注数据
		public const int XL_MANMONBETTING_REQUEST = 91;
		//点击摇钱树请求数据
		public const int XL_MANMONYAOQIANSHU_REQUEST = 92;
		//下注接收数据接收
		public const int CL_MANMONBETTING_RESPONSE = 10087;
		//摇钱树接收数据接收
		public const int CL_MANMONYAOQIANSHU_RESPONSE = 10088;
		//神龙转盘接收
		public const int XL_GET_LONG_POOL_REWARD_INFO = 10089;
		//神龙转盘fasong请求
		public const int XL_SENDLONG_POOL_REWARD_REQUEST = 93;
		//财神排名奖励
		public const int XL_SENMANMON_RANKREWARD_REQUEST = 94;
		//财神排名奖励
		public const int XL_SENMANMON_RANKREWARD_RESPONSE = 10090;
		//流水
		public const int XL_GET_LONG_LIU_SHUI_GOLD = 10091;
		//接收修改时间
		public const int XL_CHANGE_LIU_SHUI_TIME = 10092;
		//发送
		public const int XL_SENDCHANGE_LIU_SHUI_TIME = 95;
		//取消技能
		public const int XL_SENDCANCEL_OTHER_SKILL = 96;
		//
		public const int FISHING_CANCEL_SKILLS = 20051;
		//请求获取排名
		public const int SEND_FISHING_UPDATE_WINER_RANK_INFO = 97;

		public const int XL_SEND_MANMONEXIT = 98;

		public const int FISHING_UPDATE_WINER_RANK_INFO = 20052;

		public const int XL_SELF_UPDATE_RANK_INFO = 10093;

		public const int XL_SELF_WINTIME_COUNT_INFO = 10094;

		public const int XL_SEND_MANMONWINTIMECOUT = 99;
		//赋值金币操作
		public const int XL_CHANGEUSERGOLD_RESPONSE = 20053;
		//Boss场匹配
		public const int XL_BOSSROOMATCH_RESPONSE = 20054;
		//Boss场离开加入发送消息
		public const int XL_BOSSROOMSIGNUP_REQUEST = 100;
		//Boss场离开加入接受消息
		public const int XL_BOSSROOMSIGNUP_RESPONSE = 10095;
		//接收引导进入boss场消息
		public const int XL_ENTER_BOSS_ROOM_MESSAGE_RESPONSE = 10096;
		//发送获取boss猎杀排名
		public const int XL_SEND_BOSS_KILL_RANK = 101;
		//接收其他人boss猎杀排名
		public const int FISHING_GET_BOSS_MATCH_RANK_INFO = 20055;
		//接收自己boss猎杀排名
		public const int XL_GET_BOSS_MATCH_RANK_RESPONSE = 10097;
		//接收Boss时间
		public const int XL_UPDATEBOSSMATCHTIME_RESPONSE = 10098;
		//接收自己boss猎杀时间
		public const int XL_UPDATEBOSSMATCHTIME_REQUEST = 102;
		//接受boss猎杀结束的排名
		public const int XL_BOSS_MATCH_LEAVE_MESSAGE_RESPONSE =	10100;
		//接受荣耀排位数据
		public const int XL_API_WEI_SAI_RANK_RESPONSE =	10101;
		//发送荣耀排位协议
		public const int XL_API_WEI_SAI_RANK_REQUEST = 103;
		//发送荣誉殿堂排名
		public const int XL_RONGYUDIANTANG_RANK_REQUEST = 104;
		//接收荣誉殿堂排名
		public const int XL_RONGYUDIANTANG_RANK_RESPONSE = 10102;
		//获取奖励信息
		public const int XL_GET_PAI_WEI_REWARD_INFO_RESPONSE = 10103;
		//获取奖励状态
		public const int XL_GET_PAI_WEI_REWARD_RESPONSE = 10104;
		//发送奖励信息
		public const int XL_SEND_PAI_WEI_REWARD_INFO_REQUEST = 105;
		//发送奖励状态
		public const int XL_SEND_PAI_WEI_REWAED_REQUEST = 106;
		//发送领取荣耀奖励
		public const int XL_SEND_RONGYAO_PRIZE_REQUEST = 107;
		//接收领取荣耀奖励
		public const int XL_RECV_RONGYAO_PRIZE_RESPONSE = 10105;
		//发送购买给力 双喜 宝藏 状态协议
		public const int XL_SEND_TOP_UP_GIFT_BAG_STATE_INFO_NEW = 108;
		//接收购买给力 双喜 宝藏 状态协议
		public const int XL_GET_TOP_UP_GIFT_BAG_STATE_INFO_NEW = 10106;
		//初始接收协议
		public const int XL_GET_SEVEN_DAY_INIT_INFO = 10107;
		//发送七日礼包协议
		public const int XL_SEND_SEVENDAY_BAG_STATE_INFO_NEW = 109;
		//接收七日礼包协议
		public const int XL_GET_SEVENDAY_BAG_STATE_INFO_NEW = 10108;

		//发送七日礼包协议
		public const int XL_SEND_SEVENDAY_STARTINFO_STATE_INFO_NEW = 110;
		//發送获取活动 升级任务进度信息 客户端发送 协议号
		public const int XL_GET_LEVEL_UPGRADE_INFO = 112;
		//發送领取活动 升级活动 奖励 客户端发送 协议号
		public const int XL_SEND_LEVEL_UPGRADE_OPEN = 113;

		//防沉迷.排行榜功能开关
		public const int XL_SEND_BUTTON_HIDE_STATE = 111;
		
		public const int XL_GET_BUTTON_HIDE_STATE = 10109;

		//接收获取活动 升级任务进度信息
		public const int XL_GET_UP_LEVEL_ACTIVY_INFO = 10112;
		//接收领取活动 升级活动 奖励
		public const int XL_GET_UP_LEVEL_BAG_STATE = 10113;
		//接收付款、金幣資訊
		public const int XL_GET_PAY_INFO = 10115;
        //發送手機綁定協議
		public const int XL_SEND_BIND_PHONE = 114;
		//購買龍卡協議
		public const int XL_BUY_DRAGON_CARD = 116;
        //發送手機登入
		public const int XL_SEND_PHONE_LOGIN = 117;
		//接收手機登入回傳
		public const int XL_GET_PHONE_LOGIN = 10117;
		//設置手機密碼
		public const int XL_SET_PHONE_PASSWORD = 118;
		//接收設置手機密碼回傳
		public const int XL_GET_PHONE_PASSWORD = 10118;
		//根據手機號顯示出關聯帳號後，選擇要登入的帳號
		public const int XL_CHOISE_ACCOUNT_LOGIN = 119;
		//根據手機號顯示出關聯帳號後，選擇要登入的帳號 回傳
		public const int XL_CHOISE_ACCOUNT_LOGIN_RESPON = 10119;
		//設置手機號碼、密碼、暱稱
		public const int XL_SET_PHONE_LOGINPASS = 120;
		//獲取隨機暱稱
		public const int XL_GET_NEW_NICK = 121;
		//獲取隨機暱稱 回傳
		public const int XL_GET_NEW_NICK_RESPON = 10121;


		public FiProtoType ()
		{
			
		}

		//头部字段
		[StructLayoutAttribute (LayoutKind.Sequential, CharSet = CharSet.Ansi, Pack = 4)]
		public struct Head
		{
			public short message_id;
			public short data_length;
			public int userid;
			public long timetick;
		};

	}
}

