using System;

namespace AssemblyCSharp
{
	public class FiEventType
	{
		public const int CONNECT_START = 100860;

		public const int CONNECT_SUCCESS = 100861;

		public const int CONNECT_FAIL = 100862;

		public const int CONNECTIONT_CLOSED = 100863;

		public const int CONNECT_SUCCESS_WITHLOGIN = 100864;

		public const int RECV_MESSAGE = 100868;

		public const int SEND_MESSAGE = 100869;


		public const int ERROR_MESSAGE = -2;

		public const int CONNECT_COUNT_OUT = 100870;

		public const int CONNECT_TIMEROUT = 100871;

		public const int START_HEART_BEAT = 100872;


		//登陆请求
		public const int SEND_LOGIN_REQUEST = 100001;
		//登陆反馈
		public const int RECV_LOGIN_RESPONSE = 200001;


		public const int SEND_LOGIN_PASSWD_REQUEST = 64;
		public const int SEND_LOGIN_TOKEN_REQUEST = 65;

		public const int SEND_CL_GIVE_REQUEST = 66;

		public const int RECV_CL_GIVE_RESPONSE = 10061;

		//房间匹配请求
		public const int SEND_ROOM_MATCH_REQUEST = 100002;
		//房间匹配反馈
		public const int RECV_ROOM_MATCH_RESPONSE = 200002;


		//发送用户离开请求
		public const int SEND_USER_LEAVE_REQUEST = 100003;
		//收到用户离开反馈
		public const int RECV_USER_LEAVE_RESPONSE = 200003;


		//玩家发射子弹请求
		public const int SEND_FIRE_BULLET_REQUEST = 100004;

		//玩家击中鱼请求
		public const int SEND_HITTED_FISH_REQUEST = 100005;
		//玩家击中鱼反馈 , 对于玩家自己，如果没有成功，服务器不返回，否则返回成功捕获消息
		//对于其他玩家，根据是否拥有 FishProperty属性判断是否成功捕鱼
		public const int RECV_HITTED_FISH_RESPONSE = 200005;


		//发送改变炮倍数请求
		public const int SEND_CHANGE_CANNON_REQUEST = 100006;
		//收到改变炮倍数回复
		public const int RECV_CHANGE_CANNON_RESPONSE = 200006;


		//玩家发送特效申请
		public const int SEND_USE_EFFECT_REQUEST = 100007;
		//玩家收到特效反馈
		public const int RECV_USE_EFFECT_RESPONSE = 200007;
		//玩家收到其他玩家使用特效通知
		public const int RECV_OTHER_EFFECT_INFORM = 300007;


		//收到其他玩家进入房间通知
		public const int RECV_OTHER_ENTER_ROOM_INFORM = 300008;
		//收到其他玩家离开房间通知
		public const int RECV_OTHER_LEAVE_ROOM_INFORM = 300009;
		//收到其他玩家发送子弹通知
		public const int RECV_OTHER_FIRE_BULLET_INFORM = 300010;


		//发送鱼游出消息请求
		public const int SEND_FISH_OUT_REQUEST = 100011;
		//接收鱼游出消息回复
		public const int RECV_FISH_OUT_RESPONSE = 200011;


		public const int SEND_TOPUP_REQUEST = 100012;
		public const int RECV_TOPUP_RESPONSE = 200012;


		//发送背包数据请求
		public const int SEND_BACKPACK_REQUEST = 100013;
		//接收背包数据回复
		public const int RECV_BACKPACK_RESPONSE = 200013;
		//发送绑定手机
		public const int SEND_CONVERSION_REQUEST = 100014;
		//接收绑定手机
		public const int RECV_CONVERSION_REQUEST = 200014;

		//PK 发送创建房间请求
		public const int SEND_CREATE_PK_ROOM_REQUEST = 100015;
		//PK 接收创建房间回复
		//		public const int RECV_CREATE_PK_ROOM_RESPONSE    = 200015;

		//收到生成鱼群消息
		public const int RECV_FISHS_CREATED_INFORM = 300016;


		//PK 房主发送开始游戏请求
		public const int SEND_START_PK_GAME_REQUEST = 100017;
		//PK 回复房主开始游戏
		public const int RECV_START_PK_GAME_RESPONSE = 200017;
		//PK 开始游戏通知
		public const int RECV_START_PK_GAME_INFORM = 300017;


		//PK 发送进准备房间请求
		public const int SEND_ENTER_PK_ROOM_REQUEST = 100018;
		//PK 接收进准备房间回复
		public const int RECV_ENTER_PK_ROOM_RESPONSE = 200018;
		//PK 接收其他玩家进准备房间通知
		public const int RECV_OTHER_ENTER_PK_ROOM_INFORM = 300018;

		//PK 自己发送离开准备房间请求
		public const int SEND_LEAVE_PK_ROOM_REQUEST = 100019;
		//PK 自己接收离开准备房间请求
		public const int RECV_LEAVE_PK_ROOM_RESPONSE = 200019;
		//PK 接收其他文件离开准备房间的通知
		public const int RECV_OTHER_LEAVE_PK_ROOM_INFORM = 300019;


		//接收其他玩家改变炮倍数通知
		public const int RECV_OTHER_CHANGE_CANNON_INFORM = 300020;

		public const int RECV_OTHER_CHANGE_CANNON_STYLE_INFORM = 3000201;

		public const int RECV_OTHER_UNLOCK_CANNON_MUTIPLE_INFORM = 3000202;

		//接收冰冻超时通知
		public const int RECV_FISH_FREEZE_TIMEOUT_INFORM = 300021;

		//发送准备申请
		public const int SEND_PREPARE_PKGAME_REQUEST =	100022;
		//收到其他玩家准备的通知
		public const int RECV_OTHER_PREPARE_PKGAME_INFORM =	300022;

		//发送取消准备
		public const int SEND_CANCEL_PREPARE_PKGAME =	100023;
		//其他玩家取消准备通知
		public const int RECV_OTHER_CANCEL_PREPARE_PKGAME_INFORM =	300023;

		//游戏开始前的倒计时
		public const int RECV_PRE_PKGAME_COUNTDOWN_INFORM = 300024;
		//游戏倒计时
		public const int RECV_PKGAME_COUNTDOWN_INFORM = 30002401;

		//发送鱼雷消息
		public const int SEND_LAUNCH_TORPEDO_REQUEST = 100025;
		//发送鱼雷回复
		public const int RECV_LAUNCH_TORPEDO_RESPONSE = 200025;
		//其他玩家发送鱼雷广播
		public const int RECV_OTHER_LAUNCH_TORPEDO_INFORM = 300025;


		//发送PK场鱼雷消息
		public const int SEND_PK_LAUNCH_TORPEDO_REQUEST = 100026;
		//发送PK场鱼雷回复
		public const int RECV_PK_LAUNCH_TORPEDO_RESPONSE = 200026;
		//其他PK场玩家发送鱼雷广播
		public const int RECV_PK_OTHER_LAUNCH_TORPEDO_INFORM = 300026;

		//玩家发送特效申请
		public const int SEND_PK_USE_EFFECT_REQUEST = 100027;
		//玩家收到特效反馈
		public const int RECV_PK_USE_EFFECT_RESPONSE = 200027;
		//玩家收到其他玩家使用特效通知
		public const int RECV_PK_OTHER_EFFECT_INFORM = 300027;

		//收到PK场分配统计广播
		public const int RECV_PK_DISTRIBUTE_PROPERTY_INFORM =	300028;

		//鱼雷爆炸
		public const int SEND_TORPEDO_EXPLODE_REQUEST = 100029;

		public const int RECV_TORPEDO_EXPLODE_RESPONSE = 200029;

		public const int RECV_OTHER_TORPEDO_EXPLODE_INFORM = 300029;


		//PK场鱼雷爆炸
		public const int SEND_PK_TORPEDO_EXPLODE_REQUEST = 100030;

		public const int RECV_PK_TORPEDO_EXPLODE_RESPONSE = 200030;

		public const int RECV_PK_OTHER_TORPEDO_EXPLODE_INFORM = 300030;

		public const int RECV_PK_GOLD_GAME_RESULT_INFORM = 300031;

		public const int RECV_PK_POINT_GAME_RESULT_INFORM = 300032;

		public const int RECV_PK_POINT_GAME_ROUND_RESULT_INFORM = 300033;



		public const int SEND_RECONNECT_GAME_REQUEST = 100034;
		public const int RECV_RECONNECT_GAME_RESPONSE = 200034;

		public const int RECV_OTHER_RECONNECT_PKGAME_INFORM = 20022;

		public const int SEND_CREATE_FRIEND_ROOM_REQUEST = 100035;

		public const int RECV_CREATE_FRIEND_ROOM_RESPONSE = 200035;


		public const int SEND_ENTER_FRIEND_ROOM_REQUEST = 100036;

		public const int RECV_ENTER_FRIEND_ROOM_RESPONSE = 200036;

		public const int RECV_OTHER_ENTER_FRIEND_ROOM_INFORM = 300036;


		public const int SEND_LEAVE_FRIEND_ROOM_REQUEST = 100037;

		public const int RECV_LEAVE_FRIEND_ROOM_RESPONSE = 200037;

		public const int RECV_OTHER_LEAVE_FRIEND_ROOM_INFORM = 300037;

		//拥有断线链接的房间消息
		public const int RECV_HAVE_DISCONNECTED_ROOM_INFORM = 300038;


		//房主发送解散房间申请
		public const int SEND_DISBAND_FRIEND_ROOM_REQUEST = 100039;
		//房间收到解散房间反馈
		public const int RECV_DISBAND_FRIEND_ROOM_RESPONSE = 200039;
		//其他玩家收到房间解散的通知
		public const int RECV_DISBAND_FRIEND_ROOM_INFORM = 300039;

		//朋友约战比赛结果
		public const int RECV_FRIEND_ROOM_RESULT_INFORM = 300040;

		public const int SEND_OPEN_REDPACKET_REQUEST = 100041;

		public const int RECV_OPEN_REDPACKET_RESPONSE = 200041;

		public const int RECV_REDPACKET_INFORM = 300041;

		public const int RECV_OTHER_OPEN_RED_PACKET_INFORM = 300042;



		public const int SEND_ENTER_RED_PACKET_ROOM_REQUEST = 100043;

		public const int RECV_ENTER_RED_PACKET_ROOM_RESPONSE = 200043;
		 
		public const int RECV_OTHER_ENTER_RED_PACKET_ROOM_INFORM = 300043;



		public const int SEND_LEAVE_RED_PACKET_ROOM_REQUEST = 100044;

		public const int RECV_LEAVE_RED_PACKET_ROOM_RESPONSE = 200044;

		public const int RECV_OTHER_LEAVE_RED_PACKET_ROOM_INFORM = 300044;


		public const int SEND_GET_FRIEND_LIST_REQUEST = 12;

		public const int RECV_GET_FRIEND_LIST_RESPONSE = 200045;

		public const int SEND_ADD_FRIEND_REQUEST = 13;

		public const int RECV_ADD_FRIEND_RESPONSE = 200046;

		public const int SEND_ACCEPT_FRIEND_REQUEST = 14;
		public const int RECV_ACCEPT_FRIEND_RESPONSE = 200047;

		public const int SEND_DELETE_FRIEND_REQUEST = 15;

		public const int RECV_DELETE_FRIEND_RESPONSE = 2000471;

		public const int RECV_FISH_TIDE_COMING_INFORM = 300048;

		public const int RECV_FISH_TIDE_CLEAN_INFORM = 300049;

		public const int SEND_UNLOCK_CANNON_MULTIPLE_REQUEST = 100050;

		public const int RECV_UNLOCK_CANNON_MULTIPLE_RESPONSE = 200050;

		public const int SEND_GET_RANK_REQUEST = 100051;

		public const int RECV_GET_RANK_RESPONSE = 200051;

		public const int SEND_GET_START_GIFT_REQUEST = 37;

		public const int RECV_GET_START_GIFT_RESPONSE = 200052;

		//获取申请
		public const int SEND_GET_FRIEND_APPLY_LIST_REQUEST = 38;

		public const int RECV_GET_FRIEND_APPLY_LIST_RESPONSE = 200053;


        //發送用戶裝備砲台
		public const int SEND_CHANGE_CANNON_STYLE_REQUEST = 35;
        //接收用戶裝備的砲台
		public const int RECV_CHANGE_CANNON_STYLE_RESPONSE = 200054;

		//新手任务
		//public const int  RECV_BEGINNER_TASK_INFORM                = 100055;

		public const int SEND_EVERYDAY_TASK_REQUEST = 39;

		public const int SEND_EVERYDAY_PROCESS_REQUEST = 42;

		//领取任务奖励，活跃度
		public const int RECV_EVERYDAY_TASK_RESPONSE = 200056;

		public const int RECV_EVERYDAY_TASK_PROCESS_INFORM = 300057;


		public const int SEND_PURCHASE_PROPERTY_REQUEST = 11;

		public const int RECV_PURCHASE_PROPERTY_RESPONSE = 200058;

		public const int SEND_REJECT_FRIEND_REQUEST = 40;

		public const int RECV_REJECT_FRIEND_RESPONSE = 200059;

		public const int SEND_GIVE_OTHER_PROPERTY_REQUEST = 41;

		public const int RECV_GIVE_OTHER_PROPERTY_RESPONSE = 200060;

		public const int RECV_RED_PACKET_COUNTDOWN_INFORM = 300061;




		//获取系统邮件请求
		public const int SEND_GET_SYSTEM_MAIL_REQUEST = 83;
		//获取系统邮件反馈
		public const int RECV_GET_SYSTEM_MAIL_RESPONSE = 10079;
		//获取系统邮件内容并且删除，如果有道具，那么领取道具然后删除邮件
		public const int SEND_PROCESS_SYSTEM_MAIL_REQUEST = 86;
		//领取系统邮件反馈
		public const int RECV_PROCESS_SYSTEM_MAIL_RESPONSE = 10082;


		//获取赠送记录请求，（其他玩家赠送的）
		public const int SEND_GET_GIVE_RECORD_REQUEST = 43;
		//领取其他玩家赠送的邮件记录
		public const int RECV_GET_GIVE_RECORD_RESPONSE = 10039;
		//领取其他玩家赠送道具请求
		public const int SEND_ACCEPT_GIVE_REQUEST = 44;
		//领取其他玩家赠送道具反馈
		public const int RECV_ACCEPT_GIVE_RESPONSE = 10040;

		public const int SEND_IOS_PAY_PROPERTY_REQUEST = 47;

		public const int RECV_IOS_PAY_PROPERTY_RESPONSE = 10043;


		public const int RECV_LEVEL_UP_INFORM = 20033;

		public const int SEND_GET_RED_PACKET_LIST_REQUEST = 49;

		public const int RECV_GET_RED_PACKET_LIST_RESPONSE	= 10045;

		public const int RECV_DECREASE_CONSUMED_GOLD_INFORM	= 20037;

		public const int SEND_GET_USER_INFO_REQUEST = 50;

		public const int RECV_GET_USER_INFO_RESPONSE = 10046;

		public const int SEND_EVERYDAY_ACTIVITY_AWARD_REQUEST = 51;

		public const int RECV_EVERYDAY_ACTIVITY_AWARD_RESPONSE = 10047;

		public const int SEND_SELL_PROPERTY_REQUEST =	52;

		public const int RECV_SELL_PROPERTY_RESPONSE =	10048;

		//这是接受充值协议
		public const int RECV_TOP_UP_INFORM = 20039;


		public const int RECV_NOTIFY_OTHER_BEGINNER_REWARD	= 20041;

		public const int RECV_NOTIFY_BEGINNER_TASK_PROGRESS	= 20040;

		public const int RECV_BEGINNER_TASK_REWARD_RESPONSE	= 10033;

		public const int SEND_BEGINNER_TASK_REWARD_REQUEST	= 53;

		public const int SEND_ROOM_CHAT_MESSAGE = 54;

		public const int RECV_ROOM_CHAT_MESSAGE = 54;

		public const int SEND_OPEN_PACK_REQUEST = 55;

		public const int RECV_OPEN_PACK_RESPONSE = 10049;

		public const int SEND_GET_MONTHLY_PACK_REQUEST	=	56;

		public const int RECV_GET_MONTHLY_PACK_RESPONSE	=	10050;

		public const int RECV_NOTIFY_BROADCAST_USER_MESSAGE	= 20042;
	
		public const int RECV_NOTIFY_BROADCAST_GAME_INFO = 20043;

		//这是7日公告
		public const int RECV_FISHING_GET_SEVEN_REWARD_NOTIFY = 20056;
        //新手升級獎勵公告
		public const int RECV_NEW_USER_LEVELUP_NOTIFY = 20057;

		public const int RECV_BROADCAST_USER_MESSAGE_RESPONSE	= 10051;

		public const int SEND_BROADCAST_USER_MESSAGE_REQUEST	= 57;
		//发送
		public const int SEND_FISH_LUCKY_DRAW_REQUEST = 58;
		//接收转盘抽奖
		public const int RECV_FISH_LUCKY_DRAW_RESPONSE = 10052;
		//FISHING_FISH_LUCKY_DRAW_RESPONSE
		public const int RECV_NOTIFY_FISH_LUCKY_DRAW = 20044;

        //查詢錢包
		public const int SNED_GET_BANKMSG_REQUEST = 63;
        //接收查詢錢包
		public const int RECV_GET_BANKMSG_RESPONSE = 10057;

		//public const int        RECV_NOTIFY_BANK_MESSGAE_INFORM = 0;

		public const int SNED_EXCHANGE_CHARM_REQUEST = 62;

		public const int RECV_EXCHANGE_CHARM_RESPONSE = 10056;
        //發送存款、取款
		public const int SEND_BANK_ACCESS_REQUEST = 60;
        //接收存款、取款
		public const int RECV_BANK_ACCESS_RESPONSE = 10054;

		//		public const int        SEND_GIVE_CHARM_REQUEST     = 61;

		//		public const int        RECV_GIVE_CHARM_RESPONSE     = 10055;

		public const int SEND_SET_PSWD_REQUEST = 59;

		public const int RECV_SET_PSWD_RESPONSE = 10053;

		public const int RECV_NOTIFY_GIVE_CHARM = 20045;

		//这个是购买后像勋哥刷新金币的地方
		public const int SEND_CL_RELOAD_ASSET_INFO_REQUEST	= 67;

		public const int RECV_CL_RELOAD_ASSET_INFO_RESPONSE	= 10062;

		public const int RECV_NOTIFY_SCROLLING_UPDATE =	20046;

		public const int SEND_EXCHANGE_DIAMOND_REQUEST = 68;

		public const int RECV_EXCHANGE_DIAMOND_RESPONSE =	10063;

		public const int RECV_NOTIFY_EXCHANGE_DIAMOND =	20047;

		public const int SEND_CL_MODIFY_SECOND_PSWD_REQUEST =	69;

		public const int SEND_CL_GET_PAY_STATE_REQUEST =	70;
		public const int SEND_GET_FIRST_PAY_REWARD_REQUEST	= 71;

		public const int RECV_GET_PAY_STATE_RESPONSE =	10064;
		public const int RECV_GET_FIRST_PAY_REWARD_RESPONSE	=	10065;


		public const int SEND_SIGN_IN_AWARD_REQUEST = 82;
		//签到已经用了82 后面的不要用了 旧版本1.0.98是48

		public const int RECV_SIGN_IN_AWARD_RESPONSE = 10044;


		/************************robot releation************************/

		public const int SEND_GET_ROBOT_REQUEST = 73;

		public const int SEND_RETURN_ROBOT_REQUEST = 74;

		public const int RECV_GET_ROBOT_RESPONSE = 10067;



		public const int RECV_NOTIFY_PURCHASE_PROPERTY = 20048;

    

		public const int RECV_NOTIFY_TMP = 300000;
		//-------以上编号没有规律，从此分割线以下开始，编号添加一个每次加1，上发编号100070开始，下发编号200070开始，辅助编号300070开始，编号可以不用对应，如：100070对应200070，70对70------------
		public const int	SEND_CL_MODIFY_NICK_REQUEST = 100070;
		//修改昵称请求
		public const int	RECV_CL_MODIFY_NICK_RESPONSE = 200070;
		//修改昵称回复

		public const int RECV_CL_HELP_GOLD_TASK = 200071;
		//救济金任务
		public const int SEND_CL_GET_HELP_TASK_REWARD_REQUEST = 100071;
		//领取救济金请求
		public const int RECV_CL_GET_HELP_TASK_REWARD_RESPONSE = 200072;
		//领取救济金回复

		public const int SEND_CL_SIGNRETROACTIVE_REQUEST = 100072;
		//签到补签请求
		public const int RECV_CL_SIGNRETROACTIVE_RESPONSE = 200073;
		//签到补签回复

		public const int RECV_BIND_PHONE_STATE_RESPONSE = 200074;
		//绑定手机号回复


		//使用限时道具请求
		public const int SEND_CL_USE_PROP_TIMEEX_REQUEST = 100073;

		//接受限时道具
		public const int RECV_USE_PROP_TIME_RESPONSE = 200075;

		//删除道具
		public const int RECV_DEL_PROP_TIME_RESPONSE = 200076;

		//获取所有已经使用的 限时道具信息
		public const int RECV_GET_ALL_PROP_TIME_RESPONSE = 200077;

		//获取奖励信息
		public const int RECV_XL_GET_REWARD_INFO = 200078;

		//获取龙卡信息请求
		public const int SEND_CL_USE_DARGON_CARD_REQUEST = 100074;

		//接收龙卡奖励
		public const int RECV_CL_USE_DARGON_CARD_RESPONSE = 200079;

		//购买成功后龙卡接收
		public const int RECV_PURCHASE_DARGON_CARD_RESPONSE = 200080;

		//获取特惠信息请求
		public const int SEND_CL_PREFERENTIAL_MSG_REQUEST = 100075;
		//接收特惠 数据消息
		public const int RECV_CL_USER_PREFERENTIAL_REQUEST = 200081;

		//购买成功后特惠的接收
		public const int RECV_PURCHASE_TEHUI_CARD_RESPONSE = 200082;

		//兑换炮座请求
		public const int SEND_XL_EXCHANGEBARBETTE_REQUEST = 100076;
		//兑换炮座接收数据
		public const int RECV_XL_EXCHANGEBARBETTE_RESPONSE = 200083;

		//发送装备炮座请求
		public const int SEND_XL_EQUIPMENTBARBETTE_REQUEST = 100077;
		//接受装备炮座请求
		public const int RECV_XL_EQUIPMENTBARBETTE_RESPONSE = 200084;
		//机器人使用分身
		public const int SEND_XL_ROBOTREPLICATION_REQUEST = 1000;
		//接受财神的元宝点亮信息
		public const int RECV_XL_MANMONSTATRT_RESPONSE = 200085;
		//接受财神赌注显示信息
		public const int RECV_XL_MANMONSETTING_RESPONSE = 200086;
		//财神下注数据显示请求
		public const int SEND_XL_MANMONSETTING_REQUEST = 100079;
		//财神下注请求
		public const int SEND_XL_MANMONETTING_REQUEST = 100080;
		//财神下注接收
		public const int RECV_XL_MANMONBETTING_RESPONSE = 200087;
		//财神摇钱树请求
		public const int SEND_XL_MANMONYAOQIANSHU_REQUEST = 100081;
		//财神摇钱树接收
		public const int RECV_XL_MANMONYAOQIANSHU_RESPONSE = 200088;

		//发送获取奖池请求 测试部分
		public const int SEND_XL_TURNTABLEGETPOOL_REQUST = 100082;
		//接收奖池 测试部分
		public const int RECV_XL_TURNTABLEGETPOOL_RESPONSE = 200089;

		//发送财神排名奖励
		public const int SEND_XL_MANMONRANKREWARD_REQUST = 100083;
		//接收财神排名奖励
		public const int RECV_XL_MANMONRANKREWARD_REQUST = 200090;
		//接收流水
		public const int RECV_XL_TURNTABLELIUSHUI_RESPONSE = 200091;
		//
		public const int SEND_XL_TURNTABLELUCKDRAW_REQUST = 100084;

		public const int RECV_XL_TURNTABLELUCKDRAW_RESPOSE = 200092;

		public const int SEND_XL_CHANGELIUSHUITIME_REQUST = 100085;

		public const int RECV_XL_CHANGELIUSHUITIME_RESPOSE = 200093;

		public const int SEND_XL_CANCELOTHERSKILL_REQUST = 100086;

		public const int RECV_XL_CANCELOTHERSKILL_REQUST = 200094;

		public const int SEND_XL_UPDATERANKINFO_REQUST = 100087;

		public const int RECV_XL_UPDATERANKINFO_RESPOSE = 200095;

		public const int RECV_XL_UPDATEMYRANKINFO_RESPOSE = 200096;

		public const int RECV_XL_WINTIMECOUNTFO_RESPOSE = 200097;

		public const int SEND_XL_WINTIMECOUNTFO_RESQUSET = 100088;

		public const int SEND_XL_WINTIMECOUN_RESQUSET = 100089;
		//赋值金币本地协议
		public const int RECV_XL_CHANGEUSERGOLD_RESPOSE = 200098;
		//boss场跳转匹配本地协议
		public const int RECV_XL_BOSSROOMMATCH_RESPOSE = 200099;
		//匹配结果发送消息本地协议
		public const int SEND_XL_NOTIFYSIGNUP_RESQUSET = 100090;
		//匹配结果接受消息本地协议
		public const int RECV_XL_NOTIFYSIGNUP_RESPOSE = 200100;
		//接收引导进入boss场消息本地协议
		public const int RECV_XL_INTOBOSSROMM_RESPOSE = 200101;
		//发送获取boss猎杀排名本地协议
		public const int SEND_XL_BOSSKILLRANK_RESQUSET = 100091;
		//接收猎杀排名本地协议
		public const int RECV_XL_BOSSKILLRANK_RESPOSE = 200102;
		//接收自己猎杀本地协议
		public const int RECV_XL_BOSSKILLMYRANK_RESPOSE = 200103;
		//发送boss匹配场时间本地协议
		public const int SEND_XL_BOSSMATCHTIME_RESQUSET = 100092;
		//接受boss匹配场时间本地协议
		public const int RECV_XL_BOSSMATCHTIME_RESPOSE = 200104;
		//接受boss匹配场结束排行奖励本地协议
		public const int RECV_XL_BOSSMATCHRESULT_RESPOSE = 200105;
		//接受荣耀数据
		public const int RECV_XL_HORODATA_RESPOSE = 200106;
		//发送荣耀协议
		public const int SEND_XL_HORODATA_RESQUSET = 100093;
		//接收荣誉殿堂排名
		public const int RECV_XL_RONGYURANK_RESPOSE = 200107;
		//发送荣誉殿堂排名
		public const int SEND_XL_RONGYURANK_RESQUSET = 100094;
		//发送排位奖励状态
		public const int SEND_XL_PAIWEIPRIZEINFO_RESQUSET = 100095;
		//接收排位奖励状态
		public const int RECV_XL_PAIWEIPRIZEINFO_RESPOSE = 200108;
		//发送排位奖励信息
		public const int SEND_XL_PAIWEIPRIZE_RESQUSET = 100096;
		//接收排位奖励信息
		public const int RECV_XL_PAIWEIPRIZE_RESPOSE = 200109;
		//发送领取荣耀奖励
		public const int SEND_XL_RONGYAOPRIZE_RESQUSET = 100097;
		//接收领取荣耀奖励
		public const int RECV_XL_RONGYAOPRIZE_RESQUSET = 200110;
		//发送购买给力 双喜 宝藏 状态协议
		public const int SEND_XL_TOP_UP_GIFT_BAG_STATE_INFO_NEW_RESPOSE = 100098;
		//接收购买给力 双喜 宝藏 状态协议
		public const int RECV_XL_TOP_UP_GIFT_BAG_STATE_INFO_NEW_RESPOSE = 200111;
		//发送七日领取或者签到 状态协议
		public const int SEND_XL_SEVENDAY_BAG_STATE_INFO_NEW_RESPOSE = 100099;
		//接受七日签到初始状态协议
		public const int RECV_XL_SEVENDAY_START_BAG_STATE_INFO_NEW_RESPOSE = 200112;
		//接受七日签到l领取状态协议
		public const int RECV_XL_SEVENDAY_BAG_STATE_INFO_NEW_RESPOSE = 200113;
		//发送七日领取或者签到 状态协议
		public const int SEND_XL_SEVENDAY_STARTINFO_OPEN_NEW_RESPOSE = 1000100;
		//發送取得新手升級獎勵訊息
		public const int SEND_XL_LEVELUP_INFO_NEW_RESPOSE = 1000102;
		//發送領取新手升級獎勵訊息
		public const int SEND_XL_LEVELUP_GET_NEW_RESPOSE = 1000103;
		//接受七日签到公告协议
		public const int RECV_XL_SEVENDAY_NOTICE_STATE_INFO_NEW_RESPOSE = 200114;
        //发送按钮(防沉迷,排行版)状态请求
        public const int SEND_XL_BUTTON_HIDE_STATE = 1000101;
        //接收按钮(防沉迷,排行版)状态请求
        public const int RECV_XL_BUTTON_HIDE_STATE = 200115;
        //接收掉落红包
        public const int RECV_XL_GET_HONG_BAO_GOLD = 200116;
        //接收手機綁定協議回覆
		public const int RECV_XL_GET_BIND_PHONE_RESPONSE = 10114;
		//接收手機綁定協議回覆
		public const int XL_GET_BIND_PHONE_REQUEST = 20114;
		//接收手機綁定協議回覆
		public const int RECV_XL_BIND_PHONE_RESPONSE = 30114;
		//接收手機登入協定回覆
		public const int XL_GET_PHONE_NUMBER_REQUEST = 20117;
		//接收手機號碼設定密碼協定回覆
		public const int XL_GET_PHONE_NUMBER_PASSWORD_REQUEST = 20118;
		//接收關聯帳號登入協定回覆
		public const int XL_GET_ASSOCIATE_ACC_LOGIN_REQUEST = 20119;
		//接收手機號碼設定密碼協定回覆
		public const int XL_SET_PHONE_LOGIN_PASS_REQUEST = 20120;
		//接收隨機暱稱
		public const int XL_GET_USER_NICK_REQUEST = 10121;
		//發送購買龍卡
		public const int XL_BUY_DRAGONCARD_REQUEST = 100116;

		public FiEventType ()
		{
			
		}
	}
}

