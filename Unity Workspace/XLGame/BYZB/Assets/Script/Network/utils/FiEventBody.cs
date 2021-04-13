using System;
using System.Collections.Generic;
using Google.Protobuf;
using UnityEngine;
using UnityEngine.UI;
using System.Text;

namespace AssemblyCSharp
{
	public class Cordinate
	{
		public float x;
		public float y;

		public Cordinate()
		{
			x = 0;
			y = 0;
		}

		public Cordinate(Cordinate c)
		{
			this.x = c.x;
			this.y = c.y;
		}

		public Cordinate(float x, float y)
		{
			this.x = x;
			this.y = y;
		}

		public override string ToString()
		{
			return "{ " +
			" x:" + x +
			" y:" + y +
			" }";
		}
	}


	public class FiBaseMessage
	{
		public virtual byte[] serialize()
		{
			return null;
		}

		//		public virtual T Deserialize( byte[] data )
		//		{
		//			return default( T );
		//		}
	}


	public class FiNetworkInfo
	{
		public int code;
		public string describe;
		public int nConnectCount;

		public FiNetworkInfo()
		{
		}

		public FiNetworkInfo(int nCode, string nDescribe, int nCount)
		{
			code = nCode;
			describe = nDescribe;
			nConnectCount = nCount;
		}

	}


	/**登陆消息*/
	public class FiLoginRequest : FiBaseMessage
	{
		public string openId;
		public string nickname;
		public string accessToken;
		public string avatarUrl;
		public int gender;
		//boy 1 girl 0
		public int platfrom;
		//1 = wechat 2 = qq  3 = chenglong

		public FiLoginRequest()
		{
			openId = "";
			nickname = "";
			accessToken = "";
			avatarUrl = "";
			gender = 1;
			platfrom = 3;
		}

		public FiLoginRequest(FiLoginRequest info)
		{
			openId = info.openId;
			nickname = info.nickname;
			accessToken = info.accessToken;
			avatarUrl = info.avatarUrl;
			gender = info.gender;
			platfrom = info.platfrom;
		}

		public override string ToString()
		{
			return "{ " +
			" openId:" + openId +
			" nickname:" + nickname +
			" accessToken:" + accessToken +
			" avatarUrl:" + avatarUrl +
			" gender:" + gender +
			" platfrom:" + platfrom +
			" }";
		}

		public override byte[] serialize()
		{
			byte[] nByteBody = FiProtoHelper.toProto_LoginRequest(this).ToByteArray();
			return nByteBody;
		}
	}

	/**登陆反馈*/
	public class FiLoginResponse
	{
		public int reuslt;                                           //请求结果
		public string nickname;                                      //名字
		public string avatar;                                        //头像
		public int vipLevel;                                         //VIP等级
		public int level;                                            //玩家等级
		public int experience;                                       //经验
		public int userId;                                           //用户id
		public long gold;                                            //金币
		public long diamond;                                         //钻石
		public int topupSum;
		public int maxCanonMultiple;                                 //炮等级
		public long redPacketTicket;                                 //服务端下发的红包,单位分

		public string errorMsg = "";                                 //错误事件
		public int gender;                                           //性别

		public int nextLevelExp;                                     //升下一级需要的经验

		public int sailDay;                                          //签到系统,起航礼包
		public int cannonStyle;                                      //炮台种类

		public int beginnerCurTask;                                  //任务等级下标   
		public int beginnerProgress;                                 //任务进度
		public long roomCard;

		public long monthlyCardDurationDays;                         //月卡礼包购买时间
		public int monthlyPackGot;                                   //月卡礼包领取时间
		public int preferencePackBought;                             //首充特惠

		public long loginGold;                                       //今日流水

		public List<FiDailySignIn> signInArray = new List<FiDailySignIn>();//签到初始状态数据

		public List<int> firstPayProducts = new List<int>();        //首冲奖励列表

		public long luckyGold;                                       //免费抽奖奖池

		public int luckyFishNum;                                     //获奖金鱼数量

		public long charm;                                           //魅力值
		public int charmExchangeTimes;                               //魅力值兑换次数
		public long bankGold;                                        //银行存款数量
		public bool hasBankPswd;                                     //是否有银行密码

		public long gameId;                                          //游戏id

		public long testCoin;                                         //体验场金币

		public int isTestRoom;                                        //是否是体验房间

		public int cannonBabetteStyle;                                //炮座

		public int nmanmonnum;                                        //财神相关,控制元宝图片

		/// <summary>
		/// Boss匹配状态
		/// </summary>
		public int bossMatchState;
		//是否是新手用户
		public int isNewUser;

		public int isRongYuDianTangUser;                              //是不是荣誉殿堂用户

		public int isPaiWeiTopUpJiaCeng;                              //(这玩意不确定啥,排位排行榜上)
																	  //是否是七日新手用户
		public int IsResterUserSteate;

		public override string ToString()
		{
			return "{ " +
			" reuslt:" + reuslt +
			" nickname:" + nickname +
			" avatar:" + avatar +
			" vipLevel:" + vipLevel +
			" level:" + level +
			" experience:" + experience +
			" userId:" + userId +
			" gold:" + gold +
			" diamond:" + diamond +
			" topupSum:" + topupSum +
			" maxCanonMultiple:" + maxCanonMultiple +
			"sailDay :" + sailDay +
			"signInArray" + signInArray.Count +
			"cannonStyle" + cannonStyle +
			"monthlyCardDurationDays: " + monthlyCardDurationDays +
			"monthlyPackGotTime: " + monthlyPackGot +
			"preferencePackBought " + preferencePackBought +
			"testCoin: " + testCoin +
			"isTestRoom: " + isTestRoom +
			"cannonBabetteStyle " + cannonBabetteStyle +
			"manmonnum: " + nmanmonnum +
			"bossMatchState: " + bossMatchState +
			"isNewUser" + isNewUser +
			" }";
		}
	}

	public class FiGetFishLuckyDrawRequest : FiBaseMessage
	{
		public int type;

		public override byte[] serialize()
		{
			PB_GetFishLuckyDrawRequest nRequest = new PB_GetFishLuckyDrawRequest();
			nRequest.Type = type;
			return nRequest.ToByteArray();
		}
	}

	public class FiGetFishLuckyDrawResponse
	{
		public int result;
		public int type;
		public FiProperty property = new FiProperty();
	}

	public class FiFishLuckyDrawInform
	{
		public int userId;
		public int type;
		public FiProperty property = new FiProperty();
	}

	/**服务器推送其他用户信息*/
	public class FiUserInfo
	{
		public int userId;
		public int seatIndex;
		public int gender;
		public string nickName;
		public long gold;
		public long diamond;
		public int cannonMultiple;
		public string avatar;

		public int level;
		public int experience;
		public int vipLevel;
		public int maxCannonMultiple;

		public int cannonStyle;

		public bool prepared;
		public int gameId;
		public List<FiProperty> properties = null;
		public int monthlyCardType;
		public int cannonBabetteStyle;
		public long testCoin;
		public int isRoomTest;
		public int userChampionsRank;

		public FiUserInfo()
		{
			userId = 0;
			seatIndex = 0;
			gender = 0;
			nickName = "";
			gold = 0;
			diamond = 0;
			cannonMultiple = 0;
			avatar = "";

			level = 0;
			experience = 0;
			vipLevel = 0;
			maxCannonMultiple = 0;
			cannonStyle = 0;
			monthlyCardType = 0;
			cannonBabetteStyle = 0;
			testCoin = 0;
			isRoomTest = 0;
			userChampionsRank = 0;
		}

		public FiUserInfo(FiUserInfo info)
		{
			userId = info.userId;
			seatIndex = info.seatIndex;
			gender = info.gender;
			nickName = info.nickName;
			gold = info.gold;

			diamond = info.diamond;
			cannonMultiple = info.cannonMultiple;
			avatar = info.avatar;
			level = info.level;
			experience = info.experience;

			vipLevel = info.vipLevel;
			maxCannonMultiple = info.maxCannonMultiple;
			cannonStyle = info.cannonStyle;
			gameId = info.gameId;
			monthlyCardType = info.monthlyCardType;
			cannonBabetteStyle = info.cannonBabetteStyle;
			testCoin = info.testCoin;
			isRoomTest = info.isRoomTest;
			userChampionsRank = info.userChampionsRank;
		}

		public override string ToString()
		{
			return "{ " +
			" userId:" + userId +
			" seatIndex:" + seatIndex +
			" gender:" + gender +
			" nickName:" + nickName +
			" gold:" + gold +
			" diamond:" + diamond +
			" cannonMultiple:" + cannonMultiple +
			" avatar:" + avatar +
			" level:" + level +
			" experience:" + experience +
			" vipLevel:" + vipLevel +
			" maxCannonMultiple:" + maxCannonMultiple +
			" cannonStyle:" + cannonStyle +
			" gameId:" + gameId +
			" monthlyCardType:" + monthlyCardType +
			" cannonBabetteStyle:" + cannonBabetteStyle +
			" testCoin:" + testCoin +
			" isRoomTest:" + isRoomTest +
			" userChampionsRank:" + userChampionsRank +
			" }";
		}
	}

	/**房间匹配*/
	public class FiRoomMatchRequest
	{
		public int enterType;
		public int roomMultiple;

		public override string ToString()
		{
			return "{ " +
			" enterType:" + enterType +
			" roomMultiple:" + roomMultiple +
			" }";
		}
	}

	/**房间匹配反馈*/
	public class FiRoomMatchResponse
	{
		public int result;
		public long roomIndex;
		public int seatIndex;
		public long gold;
		public List<FiUserInfo> userArray;

		public override string ToString()
		{
			string content = "{ " +
							 " result:" + result +
							 " roomIndex:" + roomIndex +
							 " seatIndex:" + seatIndex +
							 " gold:" + gold +
							 " userArray:\n";
			foreach (FiUserInfo user in userArray) {
				content += user.ToString() + "\n";
			}
			content += " }";
			return content;
		}
	}

	/**离开游戏房间*/
	public class FiLeaveRoomRequest
	{
		public int leaveType;
		//the same as enter_type in @message EnterClassicalRoomRequest
		public int roomMultiple;
		public long roomIndex;

		public override string ToString()
		{
			return "{ " +
			" leaveType:" + leaveType +
			" roomMultiple:" + roomMultiple +
			" roomIndex:" + roomIndex +
			" }";
		}
	}
	/***离开反馈*/
	public class FiLeaveRoomResponse
	{
		public int result;
		public long gold;

		public override string ToString()
		{
			return "{ " +
			" result:" + result +
			" gold:" + gold +
			" }";
		}
	}

	//其他用户进入渔场
	public class FiOtherEnterRoom
	{
		public int enterType;
		public int roomMultiple;
		public long roomIndex;
		public FiUserInfo user;

		public override string ToString()
		{
			return "{ " +
			" enterType:" + enterType +
			" roomMultiple:" + roomMultiple +
			" roomIndex:" + roomIndex +
			" user:" + user.ToString() +
			" }";
		}
	}

	//用户离开
	public class FiOtherLeaveRoom
	{
		public int userId;
		public int roomRatio;
		public long roomIndex;
		public int seatIndex;
		public int leaveType;

		public override string ToString()
		{
			return "{ " +
			" userId:" + userId +
			" roomRatio:" + roomRatio +
			" roomIndex:" + roomIndex +
			" seatIndex:" + seatIndex +
			" leaveType:" + leaveType +
			" }";
		}
	}

	/**子弹发射消息*/
	public class FiFireBulletRequest
	{
		public int userId;
		public int cannonMultiple;
		public int bulletId;
		public Cordinate position;

		public int groupId;
		public int fishId;

		public bool violent;

		public override string ToString()
		{
			return "{ " +
			" userId:" + userId +
			" cannonMultiple:" + cannonMultiple +
			" bulletId:" + bulletId +
			" position:" + position.ToString() +
			" groupId:" + groupId +
			" fishId:" + fishId +
			" }";
		}
	}

	/**子弹发射消息回复*/
	public class FiFireBulletResponse
	{
		public int result;
		public int userId;
		//		public int    gold;
		//		public int    cannonLevel;
		//		public int    bulletId;
		//		public Vect2D position;

		public override string ToString()
		{
			return "{ " +
			" result:" + result +
			" userId:" + userId +
			" }";
		}
	}

	/**子弹击中消息*/
	public class FiHitFishRequest
	{
		public int fishId;
		public int userId;
		public int bulletId;
		public int groupId;
		public int cannonMultiple;
		public Cordinate position;
		public bool violent = false;
		public long longLiuShuiGold;
		public int beiyongFishID;

		public override string ToString()
		{
			return "{ " +
			" fishId:" + fishId +
			" userId:" + userId +
			" bulletId:" + bulletId +
			" groupId:" + groupId +
			" cannonMultiple:" + cannonMultiple +
			" position:" + position.ToString() +
			"longLiuShuiGold:" + longLiuShuiGold.ToString() +
			"beiyongFishID:" + beiyongFishID.ToString() +
			" }";
		}
	}

	public class FiPropertyType
	{
		/*
		public const int GOLD = 1000; //金币
		public const int DIAMOND = 1001; //钻石
		public const int EXP = 1002; //经验
		public const int POINT = 10003; //积分


		public const int CANNON_LEVEL_0 = 1; //捕鱼先锋
		public const int CANNON_LEVEL_1 = 2; //加农火炮
		public const int CANNON_LEVEL_2 = 3; //冰冻新星
		public const int CANNON_LEVEL_3 = 4; //绿野仙踪
		public const int CANNON_LEVEL_4 = 5; //苍鹰之眸
		public const int CANNON_LEVEL_5 = 6; //凤凰之影
		public const int CANNON_LEVEL_6 = 7; //烈焰风暴
		public const int CANNON_LEVEL_7 = 8; //冰霜风暴
		public const int CANNON_LEVEL_8 = 9; //天使之翼
		public const int CANNON_LEVEL_9 = 10; //神圣之光
		public const int FREEZE = 11; //冰冻
		public const int AIM = 12; //锁定
		public const int FURIOUS = 13; //狂暴
		public const int BOMB_LEVEL_1 = 14; //迷你鱼雷
		public const int BOMB_LEVEL_2 = 15; //青铜鱼雷
		public const int BOMB_LEVEL_3 = 16; //白银鱼雷
		public const int BOMB_LEVEL_4 = 17; //黄金鱼雷
		public const int BOMB_LEVEL_5 = 18; //白金鱼雷
		public const int BOMB_LEVEL_6 = 19; //核子鱼雷
		public const int BOSS_NUCLEUS_YELLOW = 20; //黄色Boss晶核
		public const int BOSS_NUCLEUS_RED = 21; //红色Boss晶核
		public const int BOSS_NUCLEUS_BLUE = 22; //蓝色Boss晶核
		public const int BOSS_NUCLEUS_CORE = 23; //核心晶核
		public const int BOMB_GIFT_BOX = 24; //鱼雷礼包
		public const int SUMMON = 25; //召唤/*/

		public const int GOLD = 1000;
		//金币
		public const int DIAMOND = 1001;
		//钻石
		public const int EXP = 1002;
		//经验
		public const int G_POINT = 1003;
		//积分
		public const int TICKET = 1004;
		//奖券
		public const int ROOM_CARD = 1005;
		//房卡

		public const int BANK_GOLD = 1008;
		//银行金币
		public const int BANK_CHARM = 1007;
		//银行魅力值

		//体验场金币
		public const int TEST_GOLD = 1010;

		public const int FISHING_EFFECT_FREEZE = 2000;
		//冰冻
		public const int FISHING_EFFECT_AIM = 2001;
		//锁定
		public const int FISHING_EFFECT_VIOLENT = 2002;
		//狂暴
		public const int FISHING_EFFECT_SUMMON = 2003;
		//召唤

		//凤凰之影
		public const int FISHING_EFFECT_GUNMOUNT = 9036;

		public const int TORPEDO_MINI = 2004;
		//迷你鱼雷
		public const int TORPEDO_BRONZE = 2005;
		//青铜鱼雷
		public const int TORPEDO_SILVER = 2006;
		//白银鱼雷
		public const int TORPEDO_GOLD = 2007;
		//黄金鱼雷
		public const int TORPEDO_PLATINUM = 2008;
		//白金鱼雷
		public const int TORPEDO_NUCLEAR = 2009;
		//核子鱼雷
		public const int TORPEDO_PK = 2010;
		//竞技场专业鱼雷

		public static int GET_CL_TORPDEO_ID(int nUnitId)
		{
			return nUnitId - 1003;
		}

		public const int FISHING_EFFECT_DOUBLE = 2011;
		//双倍
		public const int FISHING_BULLET = 2012;
		//子弹
		public const int FISHING_EFFECT_REPLICATE = 2013;
		//分身
		//
		public const int FISHING_AUTOFIRE = 2020;

		public const int CANNON_VIP0 = 3000;
		//捕鱼先锋
		public const int CANNON_VIP1 = 3001;
		//加农火炮
		public const int CANNON_VIP2 = 3002;
		//冰冻新星
		public const int CANNON_VIP3 = 3003;
		//绿野仙踪
		public const int CANNON_VIP4 = 3004;
		//苍鹰之眸
		public const int CANNON_VIP5 = 3005;
		//凤凰之影
		public const int CANNON_VIP6 = 3006;
		//烈焰风暴
		public const int CANNON_VIP7 = 3007;
		//冰霜风暴
		public const int CANNON_VIP8 = 3008;
		//天使之翼
		public const int CANNON_VIP9 = 3009;
		//神圣之光

		public const int TORPEDO_GIFT_PACK = 4000;
		//鱼雷礼包
		public const int NUCLEUS_CORE = 4001;
		//核心晶核
		public const int NUCLEUS_YELLOW = 4002;
		//黄色Boss晶核
		public const int NUCLEUS_RED = 4003;
		//红色Boss晶核
		public const int NUCLEUS_BLUE = 4004;
		//蓝色Boss晶核

		public const int CANNON_SEAT_BRONZE = 4005;
		//青铜炮座
		public const int CANNON_SEAT_SILVER = 4006;
		//白银炮座
		public const int CANNON_SEAT_GOLD = 4007;
		//黄金炮座

		public const int GIFT_TORPEDO = 5030;
		//鱼雷礼包
		public const int GIFT_VIP1 = 5001;
		//VIP礼包
		public const int GIFT_VIP2 = 5002;
		//VIP礼包
		public const int GIFT_VIP3 = 5003;
		//VIP礼包
		public const int GIFT_VIP4 = 5004;
		//VIP礼包
		public const int GIFT_VIP5 = 5005;
		//VIP礼包
		public const int GIFT_VIP6 = 5006;
		//VIP礼包
		public const int GIFT_VIP7 = 5007;
		//VIP礼包
		public const int GIFT_VIP8 = 5008;
		//VIP礼包
		public const int GIFT_VIP9 = 5009;
		//VIP礼包

		//暂时定义龙卡和特惠等服务器决定
		public const int TEGHUI = 0;
		public const int SOLIDERDARGON_CARD = 1;
		public const int GOLDDARGON_CARD = 2;
		public const int SHENGLONG_CARD = 3;


		//限时道具的区间
		public const int TIMELIMTPROTYPE_1 = 8000;
		public const int TIMELIMTPROTYPE_3 = 9999;


		public const int PACK_PREFERENCE = 5031;
		public const int PACK_MONTH = 5032;

		public const int LUCKDRAW_GOLD = 10000;

		public static string GetCannonName(int nType)
		{
			switch (nType) {
				case CANNON_VIP0:
					return "捕魚先鋒";
				case CANNON_VIP1:
					return "加農火炮";
				case CANNON_VIP2:
					return "冰凍新星";
				case CANNON_VIP3:
					return "綠野仙踪";
				case CANNON_VIP4:
					return "蒼鷹之眸";
				case CANNON_VIP5:
					return "鳳凰之影";
				case CANNON_VIP6:
					return "烈焰風暴";
				case CANNON_VIP7:
					return "冰霜風暴";
				case CANNON_VIP8:
					return "天使之翼";
				case CANNON_VIP9:
					return "神聖之光";
			}
			return "";
		}

		public static string GetCannonPath(int nType)
		{
			switch (nType) {
				case CANNON_VIP0:
					return "cannon/preb捕魚先鋒";
				case CANNON_VIP1:
					return "cannon/preb加農火炮";
				case CANNON_VIP2:
					return "cannon/preb冰凍新星";
				case CANNON_VIP3:
					return "cannon/preb綠野仙踪";
				case CANNON_VIP4:
					return "cannon/preb蒼鷹之眸";
				case CANNON_VIP5:
					return "cannon/preb鳳凰之影";
				case CANNON_VIP6:
					return "cannon/preb烈焰風暴";
				case CANNON_VIP7:
					return "cannon/preb冰霜風暴";
				case CANNON_VIP8:
					return "cannon/preb天使之翼";
				case CANNON_VIP9:
					return "cannon/preb神聖之光";
			}
			return "";
		}


		public static string GetDescribtion(int nPropId)
		{
			if (nPropId >= CANNON_VIP0 && nPropId <= CANNON_VIP9) {
				if (nPropId == CANNON_VIP0) {
					return "新手炮台";
				} else {
					return "VIP" + (nPropId - CANNON_VIP0) + "級專屬炮台";
				}
			}
			switch (nPropId) {
				case FiPropertyType.FISHING_EFFECT_AIM:
					return "使用後在效果期間可鎖定任意一條魚，持續100s \n"; //從20S 更改為 100S;
				case FiPropertyType.FISHING_EFFECT_FREEZE:
					return "隨機冰凍一些魚 \n";
				case FiPropertyType.FISHING_EFFECT_VIOLENT:
					return "使用後增加一定的捕魚概率，持續30s \n"; //從20S更改為30S
				case FiPropertyType.FISHING_EFFECT_SUMMON:
					return "使用後召喚一條大魚 \n數量上限：999";
				case FiPropertyType.TORPEDO_MINI:
					return "出售可獲得2萬金幣。\n漁場中使用可獲得1.9萬~2.1萬金幣。";
				case FiPropertyType.TORPEDO_BRONZE:
					return "出售可獲得10萬金幣。\n漁場中使用可獲得9.5萬~10.5萬金幣。";
				case FiPropertyType.TORPEDO_SILVER:
					return "出售可獲得50萬金幣。\n漁場中使用可獲得47.5萬~52.5萬金幣。";
				case FiPropertyType.TORPEDO_GOLD:
					return "出售可獲得100萬金幣。\n漁場中使用可獲得95萬~105萬金幣。";
				case FiPropertyType.TORPEDO_PLATINUM:
					return "出售可獲得200萬金幣。\n漁場中使用可獲得190萬~210萬金幣。";
				case FiPropertyType.TORPEDO_NUCLEAR:
					return "出售可獲得400萬金幣。\n漁場中使用可獲得380萬-420萬金幣。";
				case FISHING_EFFECT_REPLICATE://分身
					return "使用後分出多管砲台攻擊魚，持續30S";
				case ROOM_CARD:
					return "PK場房卡";//房卡
				case GIFT_TORPEDO:
					return "打開後隨機獲得一個魚雷";
				case GIFT_VIP1:
					return "VIP1每日禮包：內含冰凍、鎖定、狂暴道具各1個";
				case GIFT_VIP2:
					return "VIP2每日禮包：內含冰凍、鎖定、狂暴道具各2個";
				case GIFT_VIP3:
					return "VIP3每日禮包：內含冰凍、鎖定、狂暴道具各3個";
				case GIFT_VIP4:
					return "VIP4每日禮包：內含冰凍、鎖定、狂暴道具各4個";
				case GIFT_VIP5:
					return "VIP5每日禮包：內含冰凍、鎖定、狂暴道具各5個";
				case GIFT_VIP6:
					return "VIP6每日禮包：內含冰凍、鎖定、狂暴道具各8個";
				case GIFT_VIP7:
					return "VIP7每日禮包：內含冰凍、鎖定、狂暴道具各10個";
				case GIFT_VIP8:
					return "VIP8每日禮包：內含冰凍、鎖定、狂暴道具各12個";
				case GIFT_VIP9:
					return "VIP9每日禮包：內含冰凍、鎖定、狂暴道具各15個";
					//			case TIMELIMTPROTYPE_1:
					//				return "VIP5级专属炮台(1天使用权)";
					//			case TIMELIMTPROTYPE_3:
					//				return "VIP6级专属炮台(3天使用权)";
			}
			return "";
		}

		public static string GetVipSignReward(int nVipLevel)
		{
			switch (nVipLevel) {
				case 1:
					return "內含:冰凍X1、鎖定X1、狂暴X1";
				case 2:
					return "內含:冰凍X2、鎖定X2、狂暴X2";
				case 3:
					return "內含:冰凍X3、鎖定X3、狂暴X3";
				case 4:
					return "內含:冰凍X4、鎖定X4、狂暴X4";
				case 5:
					return "內含:冰凍X5、鎖定X5、狂暴X5";
				case 6:
					return "內含:冰凍X8、鎖定X8、狂暴X8";
				case 7:
					return "內含:冰凍X10、鎖定X10、狂暴X10";
				case 8:
					return "內含:冰凍X12、鎖定X12、狂暴X12";
				case 9:
					return "內含:冰凍X15、鎖定X15、狂暴X15";
			}
			return "";
		}

		public static int GetSellCost(int nPropId)
		{
			switch (nPropId) {
				case FiPropertyType.TORPEDO_MINI:
					return 20000;//"出售可获得10000金币。渔场中使用可获得5000-15000金币。";
				case FiPropertyType.TORPEDO_BRONZE:
					return 100000;//"出售可获得30000金币。渔场中使用可获得10000-50000金币。";
				case FiPropertyType.TORPEDO_SILVER:
					return 500000;//"出售可获得10万金币。渔场中使用可获得8万-12万金币。";
				case FiPropertyType.TORPEDO_GOLD:
					return 1000000;// "出售可获得25万金币。渔场中使用可获得20万-30万金币。";
				case FiPropertyType.TORPEDO_PLATINUM:
					return 2000000;// "出售可获得50万金币。渔场中使用可获得40万-60万金币。";
				case FiPropertyType.TORPEDO_NUCLEAR:
					return 4000000;//"出售可获得100万金币。渔场中使用可获得80万-120万金币。";
			}
			return 0;
		}

		public static string GetToolName(int nPropId)
		{
			if (nPropId >= GIFT_VIP1 && nPropId <= GIFT_VIP9) {
				return "VIP禮包";
			}

			switch (nPropId) {
				case FiPropertyType.GOLD:
					return "金幣";
				case FiPropertyType.DIAMOND:
					return "鑽石";
				case FiPropertyType.FISHING_EFFECT_AIM:
					return "鎖定";
				case FiPropertyType.FISHING_EFFECT_FREEZE:
					return "冰凍";
				case FiPropertyType.FISHING_EFFECT_VIOLENT:
					return "狂暴";
				case FiPropertyType.FISHING_EFFECT_SUMMON:
					return "召喚";
				case FiPropertyType.FISHING_EFFECT_GUNMOUNT:
					return "烈焰風暴";
				case FiPropertyType.TORPEDO_MINI:
					return "迷你魚雷";
				case FiPropertyType.TORPEDO_BRONZE:
					return "青銅魚雷";
				case FiPropertyType.TORPEDO_SILVER:
					return "白銀魚雷";
				case FiPropertyType.TORPEDO_GOLD:
					return "黄金魚雷";
				case FiPropertyType.TORPEDO_PLATINUM:
					return "白金魚雷";
				case FiPropertyType.TORPEDO_NUCLEAR:
					return "核子魚雷";
				case FiPropertyType.FISHING_EFFECT_REPLICATE:
					return "分身";
				case ROOM_CARD:
					return "房卡";//房卡
				case FiPropertyType.GIFT_VIP1:
					return "VIP1禮包";
				case FiPropertyType.GIFT_VIP2:
					return "VIP2禮包";
				case FiPropertyType.GIFT_VIP3:
					return "VIP3禮包";
				case FiPropertyType.GIFT_VIP4:
					return "VIP4禮包";
				case FiPropertyType.GIFT_VIP5:
					return "VIP5禮包";
				case FiPropertyType.GIFT_VIP6:
					return "VIP6禮包";
				case FiPropertyType.GIFT_VIP7:
					return "VIP7禮包";
				case FiPropertyType.GIFT_VIP8:
					return "VIP8禮包";
				case FiPropertyType.GIFT_VIP9:
					return "VIP9禮包";
				case CANNON_VIP0:
					return "捕魚先鋒";
				case CANNON_VIP1:
					return "加農火砲";
				case CANNON_VIP2:
					return "冰凍新星";
				case CANNON_VIP3:
					return "綠野仙蹤";
				case CANNON_VIP4:
					return "蒼鷹之眸";
				case CANNON_VIP5:
					return "鳳凰之影";
				case CANNON_VIP6:
					return "烈焰風暴";
				case CANNON_VIP7:
					return "雷霆之怒";
				case CANNON_VIP8:
					return "天使之翼";
				case CANNON_VIP9:
					return "神聖之光";
				case GIFT_TORPEDO:
					return "魚雷禮包";
			}
			return "";
		}
		//将显示道具ID解析
		private static int[] GetTimeSprite(int TimeProId)
		{
			int[] TimeIcon = new int[2];

			if (TimeProId < 7010) {
				return null;
			}
			int id = TimeProId % 1000;
			if (10 < id && id < 100) {
				//				Debug.LogError ("iddddddd" + id);
				TimeIcon[0] = id % 10;
				TimeIcon[1] = id / 10;
			} else if (100 < id && id < 1000) {
				TimeIcon[0] = id % 10;
				TimeIcon[1] = id / 10;
			}
			return TimeIcon;
		}
		//限时道具解析
		public static Sprite[] GetTimeSpriteShow(int timeProId)
		{
			int[] TimeProId = GetTimeSprite(timeProId);
			Sprite[] TimeIcon = new Sprite[2];
			if (timeProId > 8010 && timeProId < 8999) {
				//1,2,3分别代表这8071,8072,8073
				switch (TimeProId[0]) {
					case 1:
						TimeIcon[0] = UIHallTexturers.instans.barbettePropTimeIcon[0];
						break;
					case 2:
						TimeIcon[0] = UIHallTexturers.instans.barbettePropTimeIcon[1];
						break;
					case 3:
						TimeIcon[0] = UIHallTexturers.instans.barbettePropTimeIcon[2];
						break;
					default:
						break;
				}
				switch (TimeProId[1]) {
					case 1:
						TimeIcon[1] = UIHallTexturers.instans.barbettePropTimeIcon[3];
						break;
					case 3:
						TimeIcon[1] = UIHallTexturers.instans.barbettePropTimeIcon[4];
						break;
					case 7:
						TimeIcon[1] = UIHallTexturers.instans.barbettePropTimeIcon[5];
						break;
					case 15:
						TimeIcon[1] = UIHallTexturers.instans.barbettePropTimeIcon[6];
						break;
					case 30:
						TimeIcon[1] = UIHallTexturers.instans.barbettePropTimeIcon[7];
						break;
				}

			} else if (timeProId > 9010 && timeProId < 9999) {

				switch (TimeProId[0]) {
					case 1:
						TimeIcon[0] = UIHallTexturers.instans.PropTimeIcon[0];
						break;
					case 2:
						TimeIcon[0] = UIHallTexturers.instans.PropTimeIcon[1];
						break;
					case 3:
						TimeIcon[0] = UIHallTexturers.instans.PropTimeIcon[2];
						break;
					case 4:
						TimeIcon[0] = UIHallTexturers.instans.PropTimeIcon[3];
						break;
					case 5:
						TimeIcon[0] = UIHallTexturers.instans.PropTimeIcon[4];
						break;
					case 6:
						TimeIcon[0] = UIHallTexturers.instans.PropTimeIcon[5];
						break;
					case 7:
						TimeIcon[0] = UIHallTexturers.instans.PropTimeIcon[6];
						break;
					case 8:
						TimeIcon[0] = UIHallTexturers.instans.PropTimeIcon[7];
						break;
					case 9:
						TimeIcon[0] = UIHallTexturers.instans.PropTimeIcon[8];
						break;
				}
				switch (TimeProId[1]) {
					case 1:
						TimeIcon[1] = UIHallTexturers.instans.PropTimeIcon[9];
						break;
					case 3:
						TimeIcon[1] = UIHallTexturers.instans.PropTimeIcon[10];
						break;
					case 7:
						TimeIcon[1] = UIHallTexturers.instans.PropTimeIcon[11];
						break;
					case 15:
						TimeIcon[1] = UIHallTexturers.instans.PropTimeIcon[12];
						break;
					case 30:
						TimeIcon[1] = UIHallTexturers.instans.PropTimeIcon[13];
						break;
				}
			} else if (timeProId > 7010 && timeProId < 7999) {
				//1,2,3分别代表这这里代表的背包或者商场显示的图标炮座
				switch (TimeProId[0]) {
					case 1:
						TimeIcon[0] = UIHallTexturers.instans.barbettePropTimeIcon[8];
						break;
					case 2:
						TimeIcon[0] = UIHallTexturers.instans.barbettePropTimeIcon[9];
						break;
					case 3:
						TimeIcon[0] = UIHallTexturers.instans.barbettePropTimeIcon[10];
						break;
					default:
						break;
				}
				switch (TimeProId[1]) {
					case 1:
						TimeIcon[1] = UIHallTexturers.instans.barbettePropTimeIcon[3];
						break;
					case 3:
						TimeIcon[1] = UIHallTexturers.instans.barbettePropTimeIcon[4];
						break;
					case 7:
						TimeIcon[1] = UIHallTexturers.instans.barbettePropTimeIcon[5];
						break;
					case 15:
						TimeIcon[1] = UIHallTexturers.instans.barbettePropTimeIcon[6];
						break;
					case 30:
						TimeIcon[1] = UIHallTexturers.instans.barbettePropTimeIcon[7];
						break;
				}
			}
			return TimeIcon;

		}
		//限时道具描述解析
		public static string GetTimeSpriteDescripe(int timeProId)
		{
			StringBuilder stringBuilder = new StringBuilder();
			string temp = "";
			int[] TimeProId = GetTimeSprite(timeProId);
			if (timeProId > 8010 && timeProId < 8999) {
				switch (TimeProId[0]) {
					case 1:
						stringBuilder = stringBuilder.Append("1秒4炮");
						break;
					case 2:
						stringBuilder = stringBuilder.Append("1秒5炮");
						break;
					case 3:
						stringBuilder = stringBuilder.Append("1秒7炮");
						break;
				}
				switch (TimeProId[1])
				{
					case 1:
						stringBuilder.Append("(1天使用權)");
						break;
					case 3:
						stringBuilder.Append("(3天使用權)");
						break;
					case 7:
						stringBuilder.Append("(7天使用權)");
						break;
					case 15:
						stringBuilder.Append("(15天使用權)");
						break;
					case 30:
						stringBuilder.Append("(30天使用權)");
						break;
				}
			} else if (timeProId > 9010 && timeProId < 9999) {
				switch (TimeProId[0]) {
					case 1:
						stringBuilder = stringBuilder.Append("VIP1級專屬炮台");
						break;
					case 2:
						stringBuilder = stringBuilder.Append("VIP2級專屬炮台");
						break;
					case 3:
						stringBuilder = stringBuilder.Append("VIP3級專屬炮台");
						break;
					case 4:
						stringBuilder = stringBuilder.Append("VIP4級專屬炮台");
						break;
					case 5:
						stringBuilder = stringBuilder.Append("VIP5級專屬炮台");
						break;
					case 6:
						stringBuilder = stringBuilder.Append("VIP6級專屬炮台");
						break;
					case 7:
						stringBuilder = stringBuilder.Append("VIP7級專屬炮台");
						break;
					case 8:
						stringBuilder = stringBuilder.Append("VIP8級專屬炮台");
						break;
					case 9:
						stringBuilder = stringBuilder.Append("VIP9級專屬炮台");
						break;
				}

				switch (TimeProId[1]) {
					case 1:
						stringBuilder.Append("(1天使用權)");
						break;
					case 3:
						stringBuilder.Append("(3天使用權)");
						break;
					case 7:
						stringBuilder.Append("(7天使用權)");
						break;
					case 15:
						stringBuilder.Append("(15天使用權)");
						break;
					case 30:
						stringBuilder.Append("(30天使用權)");
						break;
				}
			}
			temp = stringBuilder.ToString();
			return temp;
		}
		//限时道具名字和天数解析
		public static string GetTimeSpriteName(int timeProId)
		{
			int[] TimeProId = GetTimeSprite(timeProId);
			if (timeProId > 8010 && timeProId < 8999) {
				switch (TimeProId[0]) {
					case 1:
						return "青銅砲座";
					case 2:
						return "白銀砲座";
					case 3:
						return "黃金砲座";
				}
				switch (TimeProId[1]) {
					case 1:
						return "1天";
					case 3:
						return "3天";
					case 7:
						return "7天";
					case 15:
						return "15天";
					case 30:
						return "30天";
				}
			} else if (timeProId > 9010 && timeProId < 9999) {
				switch (TimeProId[0]) {
					case 1:
						return "加農火";
					case 2:
						return "冰凍新星";
					case 3:
						return "綠野仙踪";
					case 4:
						return "蒼鷹之眸";
					case 5:
						return "鳳凰之影";
					case 6:
						return "烈焰風暴";
					case 7:
						return "雷霆之怒";
					case 8:
						return "天使之翼";
					case 9:
						return "神聖之光";
				}
				switch (TimeProId[1]) {
					case 1:
						return "1天";
					case 3:
						return "3天";
					case 7:
						return "7天";
					case 15:
						return "15天";
					case 30:
						return "30天";
				}
			}
			return "";
		}

		public static Sprite GetSignSprite(int propertyId)
		{
			switch (propertyId) {
				case FiPropertyType.GOLD:
					return UIHallTexturers.instans.Sign[0];
				case FiPropertyType.DIAMOND:
					return UIHallTexturers.instans.Sign[1];
				case FiPropertyType.FISHING_EFFECT_FREEZE:
					return UIHallTexturers.instans.Sign[2];
				case FiPropertyType.FISHING_EFFECT_SUMMON:
					return UIHallTexturers.instans.Sign[3];
				case FiPropertyType.FISHING_EFFECT_AIM:
					return UIHallTexturers.instans.Sign[4];
				case FiPropertyType.FISHING_EFFECT_VIOLENT:
					return UIHallTexturers.instans.Sign[5];
				case FiPropertyType.TIMELIMTPROTYPE_1:
					return UIHallTexturers.instans.Sign[6];
				case FiPropertyType.TIMELIMTPROTYPE_3:
					return UIHallTexturers.instans.Sign[7];
			}
			return null;
		}

		public static Sprite GetSigncontiuenSprite(int properid)
		{
			switch (properid) {
				case FiPropertyType.DIAMOND:
					return UIHallTexturers.instans.ContinueDaySign[0];
				case FiPropertyType.GIFT_TORPEDO:
					return UIHallTexturers.instans.ContinueDaySign[1];
			}
			return null;
		}

		public static Sprite GetSpriteRewardType(int properyId)
		{
			switch (properyId) {
				case FiPropertyType.TEGHUI:
					return UIHallTexturers.instans.RewardTypeIcon[1];
					break;
				case FiPropertyType.SOLIDERDARGON_CARD:
					return UIHallTexturers.instans.RewardTypeIcon[2];
					break;
				case FiPropertyType.GOLDDARGON_CARD:
					return UIHallTexturers.instans.RewardTypeIcon[3];
					break;
				case FiPropertyType.SHENGLONG_CARD:
					return UIHallTexturers.instans.RewardTypeIcon[4];
					break;
				default:
					return UIHallTexturers.instans.RewardTypeIcon[0];
					break;
			}
		}

		public static Sprite GetDaiKuangSprite(int propertyId)
		{
			switch (propertyId) {
				case FiPropertyType.GOLD:
					return UIHallTexturers.instans.DaiKuangProperty[0];
				case FiPropertyType.DIAMOND:
					return UIHallTexturers.instans.DaiKuangProperty[1];
				case FiPropertyType.FISHING_EFFECT_AIM:
					return UIHallTexturers.instans.DaiKuangProperty[2];
				case FiPropertyType.FISHING_EFFECT_FREEZE:
					return UIHallTexturers.instans.DaiKuangProperty[3];
				case FiPropertyType.FISHING_EFFECT_VIOLENT:
					return UIHallTexturers.instans.DaiKuangProperty[4];
				case FiPropertyType.FISHING_EFFECT_SUMMON:
					return UIHallTexturers.instans.DaiKuangProperty[5];
				case FiPropertyType.FISHING_EFFECT_REPLICATE:
					return UIHallTexturers.instans.DaiKuangProperty[6];
				case FiPropertyType.TORPEDO_MINI:
					return UIHallTexturers.instans.IconTorpedo[0];
				case FiPropertyType.TORPEDO_BRONZE:
					return UIHallTexturers.instans.IconTorpedo[1];
				case FiPropertyType.TORPEDO_SILVER:
					return UIHallTexturers.instans.IconTorpedo[2];
				case FiPropertyType.TORPEDO_GOLD:
					return UIHallTexturers.instans.IconTorpedo[3];
				case FiPropertyType.TORPEDO_PLATINUM:
					return UIHallTexturers.instans.IconTorpedo[4];
				case FiPropertyType.TORPEDO_NUCLEAR:
					return UIHallTexturers.instans.IconTorpedo[5];
				case ROOM_CARD:
					return UIHallTexturers.instans.IconGift[2];//房卡
				case GIFT_TORPEDO:
					return UIHallTexturers.instans.IconGift[1];
			}
			return null;
		}

		public static Sprite GetSprite(int propertyId)
		{

			if (propertyId >= GIFT_VIP1 && propertyId <= GIFT_VIP9) {
				return UIHallTexturers.instans.IconGift[0];
			}

			if (propertyId >= TIMELIMTPROTYPE_1) {
				switch (propertyId) {
					case FiPropertyType.TIMELIMTPROTYPE_1:
						return UIHallTexturers.instans.Sign[6];
					case FiPropertyType.TIMELIMTPROTYPE_3:
						return UIHallTexturers.instans.Sign[7];
					case FiPropertyType.FISHING_EFFECT_GUNMOUNT:
						return UIHallTexturers.instans.Sign[7];
				}
			}

			switch (propertyId) {
				case FiPropertyType.GOLD:
					return UIHallTexturers.instans.IconProperty[4];
				case FiPropertyType.DIAMOND:
					return UIHallTexturers.instans.IconProperty[5];
				case FiPropertyType.FISHING_EFFECT_AIM:
					return UIHallTexturers.instans.IconProperty[1];
				case FiPropertyType.FISHING_EFFECT_FREEZE:
					return UIHallTexturers.instans.IconProperty[0];
				case FiPropertyType.FISHING_EFFECT_VIOLENT:
					return UIHallTexturers.instans.IconProperty[2];
				case FiPropertyType.FISHING_EFFECT_SUMMON:
					return UIHallTexturers.instans.IconProperty[3];
				case FiPropertyType.TORPEDO_MINI:
					return UIHallTexturers.instans.IconTorpedo[0];
				case FiPropertyType.TORPEDO_BRONZE:
					return UIHallTexturers.instans.IconTorpedo[1];
				case FiPropertyType.TORPEDO_SILVER:
					return UIHallTexturers.instans.IconTorpedo[2];
				case FiPropertyType.TORPEDO_GOLD:
					return UIHallTexturers.instans.IconTorpedo[3];
				case FiPropertyType.TORPEDO_PLATINUM:
					return UIHallTexturers.instans.IconTorpedo[4];
				case FiPropertyType.TORPEDO_NUCLEAR:
					return UIHallTexturers.instans.IconTorpedo[5];
				case FiPropertyType.FISHING_EFFECT_REPLICATE:
					return UIHallTexturers.instans.IconProperty[6];

				case ROOM_CARD:
					return UIHallTexturers.instans.IconGift[2];//房卡
				case GIFT_TORPEDO:
					return UIHallTexturers.instans.IconGift[1];
			}
			return null;
		}

		public static string GetToolPath(int propertyId)
		{
			if (propertyId >= GIFT_VIP1 && propertyId <= GIFT_VIP9) {
				return "tools/VIP禮包";
			}

			switch (propertyId) {
				case FiPropertyType.GOLD:
					return "金幣";
				case FiPropertyType.DIAMOND:
					return "鑽石";
				case FiPropertyType.FISHING_EFFECT_AIM:
					return "tools/Image鎖定";
				case FiPropertyType.FISHING_EFFECT_FREEZE:
					return "tools/Image冰凍";
				case FiPropertyType.FISHING_EFFECT_VIOLENT:
					return "tools/Image狂暴";
				case FiPropertyType.FISHING_EFFECT_SUMMON:
					return "tools/Image召喚";
				case FiPropertyType.TORPEDO_MINI:
					return "tools/Image迷你";
				case FiPropertyType.TORPEDO_BRONZE:
					return "tools/Image青銅";
				case FiPropertyType.TORPEDO_SILVER:
					return "tools/Image白銀";
				case FiPropertyType.TORPEDO_GOLD:
					return "tools/Image黃金";
				case FiPropertyType.TORPEDO_PLATINUM:
					return "tools/Image白金";
				case FiPropertyType.TORPEDO_NUCLEAR:
					return "tools/Image核子";
				case FiPropertyType.FISHING_EFFECT_REPLICATE:
					return "tools/ImageReplication";
				case FiPropertyType.FISHING_EFFECT_GUNMOUNT:
					return "烈焰風暴3天";
				case ROOM_CARD:
					return "tools/房卡禮包";//房卡
				case GIFT_TORPEDO:
					return "tools/魚雷禮包";

			}
			return "";
		}


		public static SkillType GetGiftSkill(int propertyId)
		{

			switch (propertyId) {
				case FiPropertyType.FISHING_EFFECT_AIM:
					return SkillType.Lock;//锁定
				case FiPropertyType.FISHING_EFFECT_FREEZE:
					return SkillType.Freeze;//冰冻
				case FiPropertyType.FISHING_EFFECT_VIOLENT:
					return SkillType.Berserk;//狂暴
				case FiPropertyType.FISHING_EFFECT_SUMMON:
					return SkillType.Summon;//召唤
				case FiPropertyType.FISHING_EFFECT_REPLICATE:
					return SkillType.Replication;//分身
				default:
					return SkillType.Error;
			}
		}
	}

	public class FiProperty
	{
		public int type;
		//GOLD = 1000, DIAMOND = 1001, EXP(经验) = 1002, FREEZE = 1003, AIM(锁定) = 1004, CALL(召唤) = 1005,
		public int value;

		public FiProperty()
		{
			type = 0;
			value = 0;
		}

		public FiProperty(FiProperty info)
		{
			type = info.type;
			value = info.value;
		}

		public override string ToString()
		{
			return "{ " +
			" type:" + type +
			" value:" + value +
			" }";
		}
	}

	public class FiPropertyEx
	{
		public int type;
		//GOLD = 1000, DIAMOND = 1001, EXP(经验) = 1002, FREEZE = 1003, AIM(锁定) = 1004, CALL(召唤) = 1005,
		public long value;

		public FiPropertyEx()
		{
			type = 0;
			value = 0;
		}

		public FiPropertyEx(FiPropertyEx info)
		{
			type = info.type;
			value = info.value;
		}

		public override string ToString()
		{
			return "{ " +
			" type:" + type +
			" value:" + value +
			" }";
		}
	}
	//子弹击中鱼消息 , 成功捕获鱼 propertyArray.count > 0
	public class FiHitFishResponse
	{
		public int fishId;
		public int userId;
		public int bulletId;
		public int groupId;
		public int cannonMultiple;
		public Cordinate position;
		public bool violent = false;
		public List<FiProperty> propertyArray = new List<FiProperty>();

		public override string ToString()
		{
			string content = "{ " +
							 " fishId:" + fishId +
							 " userId:" + userId +
							 " bulletId:" + bulletId +
							 " groupId:" + groupId +
							 " cannonMultiple:" + cannonMultiple +
							 " position:" + position.ToString() +
							 " propertyArray:";
			foreach (FiProperty info in propertyArray) {
				content += info.ToString() + "\n";
			}
			content += " }";
			return content;
		}
	}

	//其他玩家发射子弹,消息字段同发射
	public class FiOtherFireBulletInform : FiFireBulletRequest
	{
		//		public string toString()
		//		{
		//			return "userId:"+userId+"cannonLevel"+cannonMultiple+"bulletId"+bulletId+"_x"+position.x + "_y"+position.y;
		//		}
	}

	//鱼游出界面提示
	public class FiFishsOutRequest
	{
		public int groupId;

		public override string ToString()
		{
			return "{ " +
			" groupId:" + groupId +
			" }";
		}
	}

	//鱼出屏幕了反馈
	public class FiFishsOutResponse
	{
		public int groupId;

		public override string ToString()
		{
			return "{ " +
			" groupId:" + groupId +
			" }";
		}
	}


	//创建鱼群消息//鱼群Id默认从0开始递增，如果只有一条就是0
	public class FiFishsCreatedInform
	{
		public int groupId;
		public int fishType;
		public int fishNum;
		public int trackId;
		public int trackType;
		public int tideType;

		public override string ToString()
		{
			return "{ " +
			" groupId:" + groupId +
			" fishType:" + fishType +
			" fishNum:" + fishNum +
			" trackId:" + trackId +
			" trackType:" + trackType +
			" }";
		}
	}

	//
	public class FiChangeCannonMultipleRequest
	{
		public int cannonMultiple;

		public int userId;

		public override string ToString()
		{
			return "{ " +
			" cannonMultiple:" + cannonMultiple +
			" }";
		}
	}


	public class FiChangeCannonMultipleResponse
	{
		public int result;
		public int cannonMultiple;

		public override string ToString()
		{
			return "{ " +
			" result:" + result +
			" cannonMultiple:" + cannonMultiple +
			" }";
		}
	}

	public class FiOtherChangeCannonMultipleInform
	{
		public int userId;
		public int cannonMultiple;

		public override string ToString()
		{
			return "{ " +
			" userId:" + userId +
			" cannonMultiple:" + cannonMultiple +
			" }";
		}
	}

	//	public class FiEffectInfoType
	//	{
	//		public const int FREEZE = 1; //冰冻
	//		public const int AIM = 2; //锁定
	//		public const int FURIOUS = 3; //狂暴
	//		public const int SUMMON = 4; //召唤
	//	}

	public class FiEffectInfo
	{
		public int type;
		//1冰冻 2锁定 3狂暴
		public List<int> value;
		//只有冰冻的时候有效，value为鱼的groupId

		public override string ToString()
		{
			string content = "{ " +
							 " type:" + type +
							 " value:";
			foreach (int i in value) {
				content += i + "\n";
			}
			content += " }";
			return content;
		}
	}

	//玩家发起特效请求  冰冻1 ，锁定2 ，狂暴3
	public class FiEffectRequest
	{
		public int userId;
		public FiEffectInfo effect;

		public override string ToString()
		{
			return "{ " +
			" userId:" + userId +
			" effect:" + effect.ToString() +
			" }";
		}
	}

	//玩家收到特效回复
	public class FiEffectResponse
	{
		public int result;
		public FiEffectInfo info;

		public override string ToString()
		{
			return "{ " +
			" result:" + result +
			" info:" + info.ToString() +
			" }";
		}
	}

	//收到房间到其他玩家到特效通知
	public class FiOtherEffectInform
	{
		public int userId;
		public FiEffectInfo info;

		public override string ToString()
		{
			return "{ " +
			" userId:" + userId +
			" info:" + info.ToString() +
			" }";
		}
	}

	/*	//其他玩家子弹击中鱼，字段同
	public class FiOtherHitFishInfrom:FiHitFishRequest
	{
		public string toString()
		{
			return "fishId"+fishId+"userId:"+userId+"cannonLevel"+cannonLevel+"bulletId"+bulletId+"_x"+position.x + "_y"+position.y;
		}
	} */


	public class FiFreezeTimeOutInform
	{
		public List<int> value;
		//冻住鱼的groupId

		public override string ToString()
		{
			string content = "{ " +
							 " value:";
			foreach (int i in value) {
				content += i + "\n";
			}
			content += " }";
			return content;
		}

	}

	public class FiBackpackProperty
	{
		public int id;
		public string name;
		public int type;
		//1.炮台 2.道具 3.锻造 4.礼包
		public string description;
		public bool useable;
		public bool canGiveAway;
		public long diamondCost;
		public int count;
		public long propTime;

		public FiBackpackProperty()
		{
			id = 0;
			name = "";
			type = 0;//1cannon 2toolgame 3锻造4礼包
			description = "";
			useable = false;
			canGiveAway = false;
			diamondCost = 0;
			count = 0;
			propTime = 0;
		}

		public FiBackpackProperty(FiBackpackProperty info)
		{
			id = info.id;
			name = info.name;
			type = info.type;
			description = info.description;
			useable = info.useable;
			canGiveAway = info.canGiveAway;
			diamondCost = info.diamondCost;
			count = info.count;
			propTime = info.propTime;
		}

		public override string ToString()
		{
			return "{ " +
			" id:" + id +
			" name:" + name +
			" type:" + type +
			" description:" + description +
			" useable:" + useable +
			" canGiveAway:" + canGiveAway +
			" diamondCost:" + diamondCost +
			" count:" + count +
			"propTime:" + propTime +
			" }";
		}
	}

	public class FiBackpackResponse
	{
		public int result;
		public List<FiBackpackProperty> properties;

		public override string ToString()
		{
			string content = "{ " +
							 " result:" + result;
			foreach (FiBackpackProperty info in properties) {
				content += info.ToString() + "\n";
			}
			content += " }";
			return content;
		}
	}


	//点券充值请求
	public class FiTopUpRequest : FiBaseMessage
	{
		public int type;
		public long RMB;

		public override string ToString()
		{
			return "{ " +
			" type:" + type +
			" value:" + RMB +
			" }";
		}

		public override byte[] serialize()
		{
			TopUpRequest nRequest = new TopUpRequest();
			nRequest.Type = type;
			nRequest.RMB = RMB;
			return nRequest.ToByteArray();
		}
	}

	//点券充值反馈
	public class FiTopUpResponse
	{
		public int result;
		public int type;
		public long sum;

		public override string ToString()
		{
			return "{ " +
			" result:" + result +
			" type:" + type +
			" sum:" + sum +
			" }";
		}
	}


	public class FiPkRoomInfo
	{
		public int roomIndex;
		public int goldType;
		//public int  bulletType;
		public int timeType;
		//public int pointType;
		//public int  playerNumType;
		//public string roomName;
		//public bool hasPassword;
		public bool begun;
		public int currentPlayerCount;
		//public long  createTime;

		public int roundType;
		public int roomType;

		public FiPkRoomInfo()
		{
			roomIndex = 0;
			goldType = 0;
			//bulletType = 0;
			timeType = 0;
			//pointType = 0;
			//playerNumType = 0;
			//roomName = "";
			//hasPassword = false;
			begun = false;
			currentPlayerCount = 0;
			//createTime = 0;
			roundType = 0;
			roomType = 0;
		}

		public FiPkRoomInfo(FiPkRoomInfo info)
		{
			roomIndex = info.roomIndex;
			goldType = info.goldType;
			//bulletType = info.bulletType;
			timeType = info.timeType;
			//pointType = info.pointType;
			//playerNumType = info.playerNumType;
			//roomName = info.roomName;
			//hasPassword = info.hasPassword;
			begun = info.begun;
			currentPlayerCount = info.currentPlayerCount;
			//createTime = info.createTime;

			roundType = info.roundType;
			roomType = info.roomType;
		}

		public void Copy(FiPkRoomInfo info)
		{
			roomIndex = info.roomIndex;
			goldType = info.goldType;
			//bulletType = info.bulletType;
			timeType = info.timeType;
			//pointType = info.pointType;
			//playerNumType = info.playerNumType;
			//roomName = info.roomName;
			//hasPassword = info.hasPassword;
			begun = info.begun;
			currentPlayerCount = info.currentPlayerCount;
			//createTime = info.createTime;

			roundType = info.roundType;
			roomType = info.roomType;
		}

		public override string ToString()
		{
			return "{ " +
			" roomIndex:" + roomIndex +
			" goldType:" + goldType +
			//" bulletType:" + bulletType +
			" timeType:" + timeType +
			//" pointType:" + pointType +
			//" playerNumType:" + playerNumType +
			//" hasPassword:" + hasPassword +
			" begun:" + begun +
			" currentPlayerCount:" + currentPlayerCount +
			//" createTime:" + createTime +
			" roundType:" + roundType +
			" roomType:" + roomType +
			" }";
		}
	}

	//废弃的协议
	/*public class FiGetPkRoomListRequest
	{
		public int   roomType;
		public int   pageNum;

		public string ToString()
		{
			return "{ "+
				" roomType:" + roomType +
				" pageNum:" + pageNum +
				" }";
		}
	}*/

	/*public class FiGetPkRoomListResponse
	{
		public int    result;
		public int    roomType;
		public int    pageNum;
		public List<FiPkRoomInfo> infoArray;

		public string ToString()
		{
			string content = "{ " +
			                 " result:" + result +
			                 " roomType:" + roomType +
			                 " pageNum:" + pageNum +
			                 " infoArray:";
			foreach(FiPkRoomInfo info in infoArray)
			{
				content += info.ToString ();
			}
			content += " }";
			return content;
		}
	}*/

	public class FiCreatePKRoomRequest
	{
		public int roomType;
		public int goldType;
		public int bulletType;
		public int timeType;
		public int playerNumType;
		public int pointType;
		public string roomName;
		public string roomPassword;

		public override string ToString()
		{
			return "{ " +
			" roomType:" + roomType +
			" goldType:" + goldType +
			" bulletType:" + bulletType +
			" timeType:" + timeType +
			" playerNumType:" + playerNumType +
			" pointType:" + pointType +
			" roomName:" + roomName +
			" roomPassword:" + roomPassword +
			" }";
		}
	}

	public class FiCreatePKRoomResponse
	{
		public int result;
		public int roomType;
		public int seatIndex;
		public FiPkRoomInfo info;

		public override string ToString()
		{
			return "{ " +
			" result:" + result +
			" roomType:" + roomType +
			" seatIndex:" + seatIndex +
			" info:" + info.ToString() +
			" }";
		}
	}


	public class FiStartPKGameRequest
	{
		public int roomType;
		public int roomIndex;

		public override string ToString()
		{
			return "{ " +
			" roomType:" + roomType +
			" roomIndex:" + roomIndex +
			" }";
		}
	}

	public class FiStartPKGameResponse
	{
		public int result;
		public int roomType;
		public int roomIndex;

		public override string ToString()
		{
			return "{ " +
			" result:" + result +
			" roomType:" + roomType +
			" roomIndex:" + roomIndex +
			" }";
		}
	}

	public class FiStartPKGameInform
	{
		public int roomType;
		public int roomIndex;

		public override string ToString()
		{
			return "{ " +
			" roomType:" + roomType +
			" roomIndex:" + roomIndex +
			" }";
		}
	}


	public class FiEnterPKRoomRequest
	{
		public int roomType;
		public int goldType;

		public override string ToString()
		{
			return "{ " +
			" roomType:" + roomType +
			" goldType:" + goldType +
			" }";
		}
	}

	public class FiEnterPKRoomResponse
	{
		public int result;
		public int roomIndex;
		public int roomType;
		public int seatindex;
		public int goldType;
		public List<FiUserInfo> others;

		public override string ToString()
		{
			string content = "{ " +
							 " result:" + result +
							 " roomIndex:" + roomIndex +
							 " roomType:" + roomType +
							 " seatindex:" + seatindex +
							 " goldType:" + goldType +
							 " others:";
			foreach (FiUserInfo user in others) {
				content += user.ToString() + "\n";
			}
			content += " }";
			return content;
		}
	}

	public class FiOtherEnterPKRoomInform
	{
		public int enterType;
		public int roomIndex;
		public int goldType;
		public FiUserInfo other;

		public override string ToString()
		{
			return "{ " +
			" enterType:" + enterType +
			" roomIndex:" + roomIndex +
			" goldType:" + goldType +
			" other:" + other.ToString() +
			" }";
		}
	}

	public class FiLeavePKRoomRequest
	{
		public int roomType;
		public int goldType;
		public int roomIndex;

		public override string ToString()
		{
			return "{ " +
			" roomType:" + roomType +
			" goldType:" + goldType +
			" roomIndex:" + roomIndex +
			" }";
		}
	}

	public class FiLeavePKRoomResponse
	{
		public int result;
		public int roomType;
		public int goldType;
		public int roomIndex;

		public override string ToString()
		{
			return "{ " +
			" result:" + result +
			" roomType:" + roomType +
			" goldType:" + goldType +
			" roomIndex:" + roomIndex +
			" }";
		}
	}

	public class FiOtherLeavePKRoomInform
	{
		public int seatIndex;
		public int leaveUserId;
		//		public int  roomOwnerUserId;

		public override string ToString()
		{
			return "{ " +
			" seatIndex:" + seatIndex +
			" leaveUserId:" + leaveUserId +
			" }";
		}
	}

	public class FiPreparePKGame
	{
		public int userId;
		public int roomType;
		public int roomIndex;

		public override string ToString()
		{
			return "{ " +
			" userId:" + userId +
			" roomType:" + roomType +
			" roomIndex:" + roomIndex +
			" }";
		}
	}

	//其他玩家取消准备通知
	public class FiCancelPreparePKGame
	{
		public int userId;
		public int roomType;
		public int roomIndex;

		public override string ToString()
		{
			return "{ " +
			" userId:" + userId +
			" roomType:" + roomType +
			" roomIndex:" + roomIndex +
			" }";
		}
	}



	//发射鱼雷
	public class FiLaunchTorpedoRequest
	{
		public int torpedoId;
		public int torpedoType;
		public Cordinate position;

		public override string ToString()
		{
			return "{ " +
			" torpedoId:" + torpedoId +
			" torpedoType:" + torpedoType +
			" position:" + position.ToString() +
			" }";
		}
	}

	//玩家自己发射鱼雷返回
	public class FiLaunchTorpedoResponse
	{
		public int result;
		public int torpedoId;
		public int torpedoType;
		public Cordinate position;

		public override string ToString()
		{
			return "{ " +
			" result:" + result +
			" torpedoId:" + torpedoId +
			" torpedoType:" + torpedoType +
			" position:" + position.ToString() +
			" }";
		}
	}

	//收到其他玩家发射鱼雷的通知
	public class FiOtherLaunchTorpedoInform
	{
		public int userId;
		public int torpedoId;
		public int torpedoType;
		public Cordinate position;

		public override string ToString()
		{
			return "{ " +
			" userId:" + userId +
			" torpedoId:" + torpedoId +
			" torpedoType:" + torpedoType +
			" position:" + position.ToString() +
			" }";
		}
	}


	public class FiFish
	{
		public int groupId;
		public int fishId;

		public override string ToString()
		{
			return "{ " +
			" groupId:" + groupId +
			" fishId:" + fishId +
			" }";
		}
	}


	public class FiTorpedoExplodeRequest
	{
		public int torpedoId;
		public int torpedoType;
		public List<FiFish> fishes;

		public override string ToString()
		{
			string content = "{ " +
							 " torpedoId:" + torpedoId +
							 " torpedoType:" + torpedoType +
							 " fishes:";
			foreach (FiFish fish in fishes) {
				content += fish.ToString() + "\n";
			}
			content += " }";
			return content;
		}
	}

	public class FiFishReward
	{
		public int groupId;
		public int fishId;
		public List<FiProperty> properties;

		public override string ToString()
		{
			string content = "{ " +
							 " groupId:" + groupId +
							 " fishId:" + fishId +
							 " properties:";
			foreach (FiProperty info in properties) {
				content += info.ToString() + "\n";
			}
			content += " }";
			return content;
		}
	}

	public class FiTorpedoExplodeResponse
	{
		public int result;
		public int torpedoId;
		public int torpedoType;
		public List<FiFishReward> rewards;

		public override string ToString()
		{
			string content = "{ " +
							 " result:" + result +
							 " torpedoId:" + torpedoId +
							 " torpedoType:" + torpedoType +
							 " rewards:";
			foreach (FiFishReward info in rewards) {
				content += info.ToString() + "\n";
			}
			content += " }";
			return content;
		}
	}

	public class FiOtherTorpedoExplodeInform
	{
		public int userId;
		public int torpedoId;
		public int torpedoType;
		public List<FiFishReward> rewards;

		public override string ToString()
		{
			string content = "{ " +
							 " userId:" + userId +
							 " torpedoId:" + torpedoId +
							 " torpedoType:" + torpedoType +
							 " rewards:";
			foreach (FiFishReward info in rewards) {
				content += info.ToString() + "\n";
			}
			content += " }";
			return content;
		}
	}

	public class FiPkGameCountDownInform
	{
		public int countdown;

		public override string ToString()
		{
			return "{ " +
			" countdown:" + countdown +
			" }";
		}
	}


	public class FiDistributePKProperty
	{
		public int roomIndex;
		public List<FiProperty> properties = new List<FiProperty>();

		public override string ToString()
		{
			string content = "{ " +
							 " roomIndex:" + roomIndex +
							 " properties:";
			foreach (FiProperty info in properties) {
				content += info.ToString() + "\n";
			}
			content += " }";
			return content;
		}
	}

	public class FiPlayerInfo
	{
		public int userId;
		public int point;
		public long gold;

		public override string ToString()
		{
			return "{ " +
			" userId:" + userId +
			" point:" + point +
			" gold:" + gold +
			" }";
		}
	}

	public class FiGoldGameResult
	{
		//子弹赛和限时赛结算
		public List<FiPlayerInfo> info;

		public override string ToString()
		{
			string content = "{ " +
							 " info:";
			foreach (FiPlayerInfo player in info) {
				content += player.ToString() + "\n";
			}
			content += " }";
			return content;
		}
	}

	public class FiPointGameResult
	{
		//几分赛赛结算
		public List<int> winnerUserId;

		public override string ToString()
		{
			string content = "{ " +
							 " winnerUserId:";
			foreach (int id in winnerUserId) {
				content += " " + id + "\n";
			}
			content += " }";
			return content;
		}
	}

	public class FiPointGameRoundResult
	{
		//积分赛阶段下发胜利者
		public int round;
		public int winnerUserId;
		//0时，表示平局

		public override string ToString()
		{
			return "{ " +
			" round:" + round +
			" winnerUserId:" + winnerUserId +
			" }";
		}
	}



	public class FiOtherGameInfo
	{
		//用户断线重连，下发的消息
		public int seatIndex;
		public int gender;
		public string nickname;
		public string avatar;
		public int userId;
		public int vipLevel;
		public List<FiProperty> properties = new List<FiProperty>();
		public int cannonStyle = 0;
		public int gameId;

		public FiOtherGameInfo()
		{
			seatIndex = 0;
			gender = 0;
			nickname = "";
			avatar = "";
			userId = 0;
			vipLevel = 0;
			properties = new List<FiProperty>();
		}

		public FiOtherGameInfo(FiOtherGameInfo info)
		{
			if (null == info)
				return;

			seatIndex = info.seatIndex;
			gender = info.gender;
			nickname = info.nickname;
			avatar = info.avatar;
			userId = info.userId;
			vipLevel = info.vipLevel;
			properties = new List<FiProperty>();
			if (null != info.properties) {
				foreach (FiProperty property in info.properties) {
					properties.Add(new FiProperty(property));
				}
			}

		}

		~FiOtherGameInfo()
		{
			seatIndex = 0;
			gender = 0;
			nickname = "";
			avatar = "";
			userId = 0;
			vipLevel = 0;
			if (null != properties) {
				properties.Clear();
			}
		}

		public override string ToString()
		{
			string content = "{ " +
							 " seatIndex:" + seatIndex +
							 " gender:" + gender +
							 " nickname:" + nickname +
							 " avatar:" + avatar +
							 " userId:" + userId +
							 " vipLevel:" + vipLevel +
							 " properties:";
			foreach (FiProperty property in properties) {
				content += properties.ToString();
			}
			content += " }";

			return content;
		}
	}



	public class FiReconnectResponse
	{
		//在PKFishing游戏中断网后，重新登录下发的消息
		public int result;
		public int roomType;
		public int goldType;
		public int roomIndex;
		public int seatIndex;
		public List<FiProperty> properties = new List<FiProperty>();
		public List<FiOtherGameInfo> others = new List<FiOtherGameInfo>();

		public override string ToString()
		{
			string content = "{ " +
							 " roomType:" + roomType +
							 " goldType:" + goldType +
							 " roomIndex:" + roomIndex +
							 " properties:";
			foreach (FiProperty property in properties) {
				content += properties.ToString();
			}
			content += "others:";
			foreach (FiOtherGameInfo gameInfo in others) {
				content += gameInfo.ToString();
			}
			content += " }";

			return content;
		}
	}

	//其他玩家重连进入渔场消息
	public class FiOtherReconnectPKGameInform
	{
		public int roomType;
		public int goldType;
		public int roomIndex;
		public FiOtherGameInfo other = new FiOtherGameInfo();
	}


	public class FiCreateFriendRoomRequest
	{
		public int roomType;
		public int goldType;
		public int timeType;
		public int roundType;

		public override string ToString()
		{
			return "{ " +
			" roomType:" + roomType +
			" goldType:" + goldType +
			" timeType:" + timeType +
			" roundType:" + roundType +
			" }";
		}
	}

	public class FiCreateFriendRoomResponse
	{
		public int result;
		public int seatIndex;
		public FiPkRoomInfo room = new FiPkRoomInfo();
	}


	public class FiEnterFriendRoomRequest
	{
		public int roomIndex;
	}

	public class FiEnterFriendRoomResponse
	{

		public int result;
		public int seatIndex;
		public int roomOwnerId;
		public FiPkRoomInfo room = new FiPkRoomInfo();
		public List<FiUserInfo> others = new List<FiUserInfo>();


		public override string ToString()
		{
			string content = "{ " +
							 " result:" + result +
							 " seatIndex:" + seatIndex;

			if (null != room) {
				content = " room:" + room.ToString();
			} else {
				content = " room: null";
			}

			if (null != others) {
				foreach (FiUserInfo user in others) {
					content += user.ToString();
				}
			}
			content += " }";

			return content;
		}
	}

	public class FiOtherEnterFriendRoomInform
	{
		public int roomType;
		public long roomIndex;
		public FiUserInfo other = new FiUserInfo();
	}


	public class FiLeaveFriendRoomRequest
	{
		public int roomType;
		public int roomIndex;
	}

	public class FiLeaveFriendRoomResponse
	{
		public int result;
		public int roomType;
		public int roomIndex;
	}


	public class FiOtherLeaveFriendRoomInform
	{
		public int seatIndex;
		public int leaveUserId;
	}


	public class FiUserRoundResult
	{
		public int userId;
		public long sum;
		//积分或者金币的总数

		//每一轮的结果
		public List<int> roundNum = new List<int>();
	}

	public class FiFriendRoomGameResult
	{
		public int roomType;
		public List<FiUserRoundResult> users = new List<FiUserRoundResult>();
	}


	public class FiDisbandFriendRoomRequest
	{
		public int roomType;
		public int roomIndex;
	}

	public class FiDisbandFriendRoomResponse
	{
		public int result;
		public int roomType;
		public int roomIndex;
	}

	public class FiDisbandFriendRoomInform
	{
		public int roomType;
		public int roomIndex;
	}

	public class FiOpenRedPacketResponse
	{
		public int result;
		public long packetId;
		public double redPacketTicket;
	}

	public class FiGetRedPacketListResponse
	{
		public int result;
		public List<long> packets = new List<long>();
	}

	public class FiOpenRedPacketRequest
	{
		//public int   roomType;
		//public int   roomIndex;

		public long packetId;
	}


	public class FiRedPacketInform
	{
		public long packetId;
		// id==0 ,表示还没有满足条件
		public long consumedGold;
	}


	public class FiOtherOpenRedPacketInform
	{
		public int userId;
		public double redPacketTicket;
	}


	public class FiEnterRedPacketRoomRequest
	{
		public int roomType;
	}

	public class FiEnterRedPacketRoomResponse
	{
		public int result;
		public int roomType;
		public long roomIndex;
		public int seatIndex;
		public long gold;
		public List<FiUserInfo> others = new List<FiUserInfo>();
		public long roomConsumedGold;
	}

	public class FiOtherEnterRedPacketRoomInform
	{
		public int roomType;
		public long roomIndex;
		public FiUserInfo other = new FiUserInfo();
	}



	public class FiLeaveRedPacketRoomRequest
	{
		public int leaveType;
		public int roomIndex;
	}


	public class FiLeaveRedPacketRoomResponse
	{
		public int result;
		public long gold;
		public int roomIndex;
	}



	public class FiOtherLeaveRedPacketRoomInform
	{
		public int seatIndex;
		public int userId;
	}

	public class FiGetRankRequest
	{
		public int type;
	}


	public class FiGetRankResponse
	{
		public int result;
		public List<FiRankInfo> rankList = new List<FiRankInfo>();
	}


	public class FiRankInfo
	{
		public long userId;
		public long gold;
		public int vipLevel;
		public string nickname;
		public string avatarUrl;
		public int gender;
		public int rewards;
		public int level;
		public int maxMultiple;
		public int gameId;
		public int duanwei;
		public int duanweirank;
		public int xingxing;
		public long shangLiuShui;
	}



	public class FiUnlockCannonRequest
	{
		public int targetMultiple;
	}

	public class FiUnlockCannonResponse
	{
		public int result;
		public int currentMaxMultiple;
		public long rewardGold;
		public int needDiamond;
	}


	public class FiStartGiftInform
	{
		public int dayOffset;
		public List<FiProperty> data = new List<FiProperty>();
	}

	public class FiGetStartGiftRequest : FiBaseMessage
	{
		public int dayOffset;

		public override byte[] serialize()
		{
			PB_GetStartGiftRequest nRequest = new PB_GetStartGiftRequest();

			nRequest.DayOffset = dayOffset;
			return nRequest.ToByteArray();
		}
	}

	public class FiGetStartGiftResponse
	{
		public int result;
		public int dayOffset;
		public List<FiProperty> data = new List<FiProperty>();
	}


	public class FiAddFriendRequest : FiBaseMessage
	{
		public int userId;

		public override byte[] serialize()
		{
			PB_AddFriendRequest nRequest = new PB_AddFriendRequest();
			nRequest.UserId = userId;
			return nRequest.ToByteArray();
		}
	}

	public class FiAddFriendResponse
	{
		public int userId;
		public int result;
	}

	public class FiDeleteFriendRequest : FiBaseMessage
	{
		public int userId;

		public override byte[] serialize()
		{
			PB_DeleteFriendRequest nRequest = new PB_DeleteFriendRequest();
			nRequest.UserId = userId;
			return nRequest.ToByteArray();
		}
	}

	public class FiDeleteFriendResponse
	{
		public int userId;
		public int result;
	}

	public class FiFriendInfo
	{
		public int userId;
		public string nickname;
		public string avatar;
		public int gender;
		public int level;
		public int vipLevel;
		public int status;
		public bool hasGivenGold;
		public int gameId;
	}

	public class FiTopUpInform
	{
		public int userId;
		public int money;
		public int currentVip;
		public FiProperty property = new FiProperty();
	}

	public class FiGetFriendListResponse
	{
		public int result;
		public int friendLimit;
		public List<FiFriendInfo> friends = new List<FiFriendInfo>();
	}

	//获取好友申请列表
	public class FiGetFriendApplyResponse
	{
		public int result;
		public List<FiFriendInfo> friends = new List<FiFriendInfo>();
	}


	public class FiAcceptFriendRequest : FiBaseMessage
	{
		public int userId;

		public override byte[] serialize()
		{
			PB_AcceptFriendRequest nRequest = new PB_AcceptFriendRequest();
			nRequest.UserId = userId;
			return nRequest.ToByteArray();
		}
	}

	public class FiAcceptFriendResponse
	{
		public int result;
		public int userId;
	}

	public class FiRejectFriendRequest : FiBaseMessage
	{
		public int userId;

		public override byte[] serialize()
		{
			PB_RejectFriendRequest nData = new PB_RejectFriendRequest();
			nData.UserId = userId;
			return nData.ToByteArray();
		}
	}

	public class FiRedPacketDistributionCountdown
	{
		public int countdown;
	}

	public class FiRejectFriendResponse
	{
		public int userId;
	}

	public class FiPurchasePropertyRequest : FiBaseMessage
	{
		public FiProperty info = new FiProperty();

		public override byte[] serialize()
		{
			PB_PurchasePropertyRequest nRequest = new PB_PurchasePropertyRequest();
			if (info != null) {
				nRequest.Property = new PB_Property();
				nRequest.Property.PropertyType = info.type;
				nRequest.Property.Sum = info.value;
			}
			return nRequest.ToByteArray();
		}
	}

	public class FiPurchasePropertyResponse
	{
		public int result;
		public FiProperty info = new FiProperty();
		public long diamondCost;
	}

	public class FiEveryDayActivityRequest : FiBaseMessage
	{
		public List<int> taskId = new List<int>();

		public override byte[] serialize()
		{
			PB_EverydayActivityRequest nRequest = new PB_EverydayActivityRequest();

			for (int i = 0; i < taskId.Count; i++) {
				nRequest.TaskId.Add(taskId[i]);
			}
			//nRequest.TaskId = taskId;
			return nRequest.ToByteArray();
		}
	}

	public class FiEverydayActivityAwardRequest : FiBaseMessage
	{
		public int activity;

		public override byte[] serialize()
		{
			PB_EverydayActivityAwardRequest nRequest = new PB_EverydayActivityAwardRequest();
			nRequest.Activity = activity;
			return nRequest.ToByteArray();
		}
	}

	public class FiEverydayActivityAwardResponse
	{
		public int result;
		public int activity;
		public List<FiProperty> property = new List<FiProperty>();
	}

	public class FiEveryDayActivityResponse
	{
		public int result;
		public int activity;
		//活跃度奖励
		public List<int> taskId = new List<int>();
	}

	public class FiEverydayTaskDetial
	{
		public int taskId;
		public int progress;
	}

	public class FiEverydayTaskProgressInform
	{
		public int result;
		public int activity;
		public List<int> states = new List<int>();
		public List<FiEverydayTaskDetial> tasks = new List<FiEverydayTaskDetial>();
	}

	public class FiGiveOtherPropertyRequest : FiBaseMessage
	{
		public int userId;
		public FiProperty property = new FiProperty();

		public override byte[] serialize()
		{
			PB_GiveOtherPropertyRequest nRequest = new PB_GiveOtherPropertyRequest();
			nRequest.UserId = userId;
			nRequest.Property = new PB_Property();
			nRequest.Property.PropertyType = property.type;
			nRequest.Property.Sum = property.value;
			return nRequest.ToByteArray();
		}
	}



	public class FiGiveOtherPropertyResponse
	{
		public int result;
		public int userId;
		public FiProperty property = new FiProperty();
	}







	public class FiSystemMail
	{
		public long mailId;
		public string title;
		public string content;
		public List<FiProperty> property = new List<FiProperty>();
		public long sendTime;
	}



	public class FiGetSystemMailResponse
	{
		public int result;
		public List<FiSystemMail> mails = new List<FiSystemMail>();
	}



	public class FiGetMailAwardsAndDeleteRequest : FiBaseMessage
	{
		public List<long> mailId = new List<long>();

		public override byte[] serialize()
		{
			PB_DelMailGetAwardRequest nRequest = new PB_DelMailGetAwardRequest();
			for (int i = 0; i < mailId.Count; i++) {
				nRequest.MailId.Add(mailId[i]);
			}
			return nRequest.ToByteArray();
		}
	}


	public class FiGetMailAwardsAndDeleteResponse
	{
		public int result;
		public List<long> mailId = new List<long>();
		public List<FiProperty> property = new List<FiProperty>();
	}


	public class FiPresentRecord
	{
		public long id;
		public int userid;
		public FiProperty property = new FiProperty();
		public long timestamp;
		public string nickname;
		public string avatar_url;
	}

	public class FiGetPresentRecordResponse
	{
		public int result;
		public List<FiPresentRecord> records = new List<FiPresentRecord>();
	}


	public class FiAcceptPresentRequest : FiBaseMessage
	{
		public List<long> giveId = new List<long>();

		public override byte[] serialize()
		{
			PB_GetGiveRequest nRequest = new PB_GetGiveRequest();
			for (int i = 0; i < giveId.Count; i++) {
				nRequest.GiveId.Add(giveId[i]);
			}
			return nRequest.ToByteArray();
		}

	}

	public class FiAcceptPresentResponse
	{
		public int result;
		public List<FiProperty> properties = new List<FiProperty>();
	}



	public class FiIosPayPropertyResponse
	{
		public int result;
		public FiProperty property = new FiProperty();
		public int firstAward;
	}


	public class FiBeginnerTaskRewardRequest : FiBaseMessage
	{
		public int beginnerCurTask;
		//当前新手任务

		public override byte[] serialize()
		{
			PB_BeginnerTaskRewardRequest nRequest = new PB_BeginnerTaskRewardRequest();
			nRequest.BeginnerCurTask = beginnerCurTask;
			return nRequest.ToByteArray();
		}
	}

	public class FiBeginnerTaskRewardResponse
	{
		public int result;
		public int beginnerCurTask;
		//当前新手任务
		public FiProperty properties = new FiProperty();
		//奖励
	}

	public class FiNotifyBeginnerTaskProgress
	{
		public int beginnerCurTask;
		//当前新手任务
		public int beginnerTaskProgress;
		//当前新手任务进度
	}

	public class FiNotifyOtherBeginnerReward
	{
		public int userId;
		public int beginnerCurTask;
		//当前新手任务
		public FiProperty property = new FiProperty();
		//奖励
	}

	public class FiIosPayPropertyRequest
	{
		public long payId;
		public int productId;
		public string orderNum;
		public string pKey;
	}


	public class FiDailySignIn
	{
		public int day;
		//1-7:周一至周日 负数的绝对值表示连续天数
		public int status;
		//0:连续天数 1:未领取 2:已领取 3:错过

	}


	public class FiDgonCard
	{
		//代表购买龙卡剩余的天数
		public int day;
		//0:没购买 1:银龙 2:金龙 3:神龙
		public int status;
	}

	public class FiSignInAwardResponse
	{
		public int result;
		public FiDailySignIn singIn = new FiDailySignIn();
		public List<FiProperty> properties = new List<FiProperty>();
	}

	public class FiLevelUpInfrom
	{
		public int userId;
		public int level;
		public int experience;
		public int nextLevelExp;
		public List<FiProperty> properties = new List<FiProperty>();
	}

	public class FiSignInAwardRequest : FiBaseMessage
	{
		public int day;

		public override byte[] serialize()
		{
			PB_SignInAwardRequest nSignRequest = new PB_SignInAwardRequest();
			nSignRequest.Day = day;
			return nSignRequest.ToByteArray();
		}
	}


	public class FiOtherUnlockCannonMutipleInform
	{
		public int userId;
		public int maxCannonMultiple;
		public int rewardGold;
		public int needDiamond;
	}

	public class FiOtherChangeCannonStyleInform
	{
		public int userId;
		public int currentCannonStyle;
	}

	public class FiChangeCannonStyleRequest : FiBaseMessage
	{
		public int cannonStyle;

		public int userId;

		public override byte[] serialize()
		{
			PB_ChangeCannonStyleRequest nRequest = new PB_ChangeCannonStyleRequest();
			nRequest.CannonStyle = cannonStyle;
			return nRequest.ToByteArray();
		}
	}

	public class FiBeginTaskInform
	{
		public int currentTask;
		public int diamond;
	}

	public class FiChangeCannonStyleResponse
	{
		public int result;
		public int currentCannonStyle;
	}

	public class FiGetUserInfoRequest : FiBaseMessage
	{
		public int userId;

		public override byte[] serialize()
		{
			PB_GetUserInfoRequest nRequest = new PB_GetUserInfoRequest();
			nRequest.UserId = userId;
			return nRequest.ToByteArray();
		}
	}

	public class FiDecreaseConsumedGoldInform
	{
		public long decreasedGold;
	}

	public class FiGetUserInfoResponse
	{
		public int reuslt;
		public FiUserInfo nUserInfo = new FiUserInfo();
	}

	public class FiSellPropertyRequest : FiBaseMessage
	{
		public List<FiProperty> mSellArray = new List<FiProperty>();

		public override byte[] serialize()
		{
			PB_SellPropertyRequest nSellOut = new PB_SellPropertyRequest();
			for (int i = 0; i < mSellArray.Count; i++) {
				PB_Property nEntity = new PB_Property();
				nEntity.PropertyType = mSellArray[i].type;
				nEntity.Sum = mSellArray[i].value;
				nSellOut.Properties.Add(nEntity);
			}
			return nSellOut.ToByteArray();
		}
	}

	public class FiSellPropertyResponse
	{
		public int result;
		public List<FiProperty> properties = new List<FiProperty>();
		public long gold;
	}

	public class FiOpenPackRequest : FiBaseMessage
	{
		public int packId;

		public override byte[] serialize()
		{
			PB_OpenPackRequest nRequest = new PB_OpenPackRequest();
			nRequest.PackId = packId;
			return nRequest.ToByteArray();
		}
	}

	public class FiGetMonthlyPackResponse
	{
		public int result;
		public List<FiProperty> properties = new List<FiProperty>();
	}

	public class FiChatMessage : FiBaseMessage
	{
		public int userId;
		public string message = "";

		public override byte[] serialize()
		{
			PB_NotifyRoomChatMessage nRequest = new PB_NotifyRoomChatMessage();
			nRequest.UserId = userId;
			nRequest.Message = message;
			return nRequest.ToByteArray();
		}
	}

	public class FiOpenPackResponse
	{
		public int result;
		public int packId;
		public List<FiProperty> properties = new List<FiProperty>();
	}

	public class FiBroadCastUserMsgRequest : FiBaseMessage
	{
		public string content = "";

		public override byte[] serialize()
		{
			if (string.IsNullOrEmpty(content)) {
				return new byte[0];
			}
			PB_BroadcastUserMessageRequest nRequest = new PB_BroadcastUserMessageRequest();
			nRequest.Content = content;
			return nRequest.ToByteArray();
		}
	}

	public class FiBroadCastUserMsgResponse
	{

		public int result;

	}

	public class FiBroadCastGameInfo
	{
		public int type;
		public string content;
	}

	public class FiBroadCastSevenInfo
	{
		public int type;
		public string content;
	}

	public class FiBroadCastUpgradeInfo
	{
		public int type;
		public string content;
	}

	public class FiBroadCastUserMsgInfrom
	{
		public string nickname;
		public string content;
	}

	public class FiBankMessageInfo
	{
		public int type;
		public long userId;
		public string nickname;
		public long giftGold;
		public int giftCount;
		public long charmChanged;
		public long bankChanged;
		public long dateTime;
	}

	public class FiGetBankMessageResponse
	{
		public int reuslt;
		public List<FiBankMessageInfo> messages = new List<FiBankMessageInfo>();
	}

	public class FiBankMessageInform
	{
		public FiBankMessageInfo data = new FiBankMessageInfo();
	}

	public class FiExchangeCharmRequest : FiBaseMessage
	{
		public long charm;
		public string password;

		public override byte[] serialize()
		{
			PB_ExchangeCharmRequest nRequest = new PB_ExchangeCharmRequest();
			nRequest.Charm = charm;
			nRequest.Password = password;
			return nRequest.ToByteArray();
		}
	}

	public class FiExchangeCharmResponse
	{
		public int result;
		public long charm;
		public long bankGold;
		public string errorMsg;
	}

	public class FiBankAccessRequest : FiBaseMessage
	{
		public long gold;
		public string pswd;

		public override byte[] serialize()
		{
			PB_BankAccessRequest nRequest = new PB_BankAccessRequest();
			nRequest.Gold = gold;
			nRequest.Pswd = pswd;
			return nRequest.ToByteArray();
		}
	}

	public class FiBankAccessResponse
	{
		public int result;
		public long gold;
		public string errorMsg;
	}

	public class FiGiveCharmRequest : FiBaseMessage
	{
		public long userId;
		public long giftGold;
		public int giftCount;
		public string pswd;

		public override byte[] serialize()
		{
			PB_GiveCharmRequest nRequest = new PB_GiveCharmRequest();
			nRequest.UserId = userId;
			nRequest.GiftCount = giftCount;
			nRequest.GiftGold = giftGold;
			nRequest.Pswd = pswd;
			return nRequest.ToByteArray();
		}
	}

	public class FiGiveCharmResponse
	{
		public int result;
		public long goldDec;
		public long charmInc;
	}


	public class FiManmonRankInfo
	{
		public string usename;
		public int rewardnum;
		public int wintime;

	}

	public class FiSetBankPswdRequest : FiBaseMessage
	{
		public string password;

		public override byte[] serialize()
		{
			PB_SetBankPswdRequest nRequest = new PB_SetBankPswdRequest();
			nRequest.Password = password;
			return nRequest.ToByteArray();
		}
	}

	public class FiGiveCharmInform
	{
		public FiBankMessageInfo data = new FiBankMessageInfo();
	}

	public class FiSetBankPswdResponse
	{
		public int result;
	}

	public class FiUtil
	{

		public static string GetDetail(long unixTimeStamp)
		{
			DateTime nSendDate = FiUtil.GetDate(unixTimeStamp);
			string nStrDate = FiUtil.GetMouthAndDay(nSendDate);
			nStrDate += "/" + nSendDate.Year + "   ";
			nStrDate += FiUtil.GetHourMinute(nSendDate) + ":";
			if (nSendDate.Second < 10) {
				nStrDate += "0";
			}
			nStrDate += nSendDate.Second;
			return nStrDate;
		}

		public static FiBackpackProperty CreateBagTool(int id, int nCount, int propTime = 0)
		{
			FiBackpackProperty nResult = new FiBackpackProperty();
			nResult.type = 1;
			nResult.count = nCount;
			nResult.id = id;
			nResult.name = FiPropertyType.GetToolName(id);
			nResult.canGiveAway = true;
			nResult.useable = true;
			nResult.description = FiPropertyType.GetDescribtion(id);
			nResult.propTime = propTime;
			//UnityEngine.Debug.LogError ( nResult.name + "--------CreateBagTool--------" + id + " / " + nCount );
			return nResult;
		}

		public static DateTime GetDate(long unixTimeStamp)
		{
			//减去8小时 -8 不知道之前有什么用，现在去掉了
			//unixTimeStamp -= 28800;
			DateTime dt = System.DateTime.Now;
			try {
				System.DateTime startTime = TimeZone.CurrentTimeZone.ToLocalTime(new System.DateTime(1970, 1, 1)); // 当地时区
				dt = startTime.AddSeconds(unixTimeStamp);
			} catch (Exception e) {

			}
			return dt;
		}

		public static string GetMouthAndDay(DateTime data)
		{

			string nResult = data.Month < 10 ? "0" + data.Month : data.Month.ToString();
			nResult += "/";
			nResult += data.Day < 10 ? "0" + data.Day : data.Day.ToString();
			return nResult;
		}

		public static string GetYearMouthAndDay(DateTime data)
		{
			string nResult = data.Year + "年";
			nResult += data.Month < 10 ? "0" + data.Month : data.Month.ToString();
			nResult += "月";
			nResult += data.Day < 10 ? "0" + data.Day : data.Day.ToString();
			nResult += "日";
			return nResult;
		}

		public static string GetBankHourMinute(DateTime data)
		{
			string nResult = data.Hour < 10 ? "0" + data.Hour : data.Hour.ToString();
			nResult += "時";
			nResult += data.Minute < 10 ? "0" + data.Minute : data.Minute.ToString();
			nResult += "分";
			return nResult;
		}

		public static string GetHourMinute(DateTime data)
		{
			string nResult = data.Hour < 10 ? "0" + data.Hour : data.Hour.ToString();
			nResult += ":";
			nResult += data.Minute < 10 ? "0" + data.Minute : data.Minute.ToString();
			return nResult;
		}

		public static string sliceNickName(string nickname)
		{
			string nNickName = nickname;
			if (nNickName.Length > 4) {
				char[] nNameChar = nNickName.ToCharArray();
				string nResult = "";
				for (int i = 0; i < 4; i++) {
					nResult += nNameChar[i] + "";
				}
				nNickName = nResult + "...";
			}
			return nNickName;
		}

	}


	public class FiCLGiveGiftRequest : FiBaseMessage
	{
		public int giveType;
		public FiProperty gift = new FiProperty();
		public int toGameId;
		public ByteString secondPasswd;


		public override byte[] serialize()
		{
			PB_CLGiveGiftRequest nSend = new PB_CLGiveGiftRequest();
			nSend.Gift = new PB_Property();
			nSend.Gift.PropertyType = gift.type;
			nSend.Gift.Sum = gift.value;
			nSend.SecondPasswd = secondPasswd;
			nSend.ToGameId = toGameId;
			nSend.GiveType = giveType;
			return nSend.ToByteArray();
		}
	}

	public class FiCLGiveGiftResponse
	{
		public int result;
		public string errorMsg;
		public int giveType;
		public FiProperty gift;
		public long currentCount;
	}

	public class FiReloadAssetInfoResponse
	{
		public int result;
		public List<FiProperty> prop = new List<FiProperty>();
	}


	public class FiScrollingNotice
	{
		public string content;
		public int cycleInterval;
	}

	public class FiNotifyScrollingNoticesUpdate
	{
		public List<FiScrollingNotice> noticeArray = new List<FiScrollingNotice>();
	}


	public class FiConvertFormalAccount
	{
		public int userID;
		public string code;
		public string mobile;
		public string pwd;
		public string token;
		public int propID;
		public int propCount;
	}

	public class FiConvertFormalBindAccount
	{
		public int result;
		public long userID;
		public string phone;
		public string code;
	}

	//   public class FiConvertFormalBindAccount
	//{
	//	public int result;
	//	public string userID;
	//	public string phone;
	//	public string code;
	//}

	public class FiSystemReward
	{
		public int resultCode;
		public int propID;
		public int propCount;
		public string msg;
	}

	public class FiModifyNick
	{
		public int userID;
		public int loginType;
		public int propID;
		public int propCount;
		public string modifyNick;

		//public ByteString modifyNick;

		//		public override byte[] serialize ()
		//		{
		//			PB_ModifyNick nNick = new PB_ModifyNick ();
		//			nNick.UserID = userID;
		//			nNick.LoginType = loginType;
		//			nNick.PropID = propID;
		//			nNick.PropCount = propCount;
		//			nNick.ModifyNick = modifyNick;
		//			return nNick.ToByteArray ();
		//		}
		//
	}

	public class FiGetHelpGodlReward
	{
		public int userID;
		public int taskID;
		public int propID;
		public int count;
	}

	public class FiHelpGoldTaskData
	{
		public int resultCode;
		public int taskID;
		public int nValue;
		public int propID;
		public int count;
		public string dec;
	}

	public class FiRetroactive
	{
		public int userID;
		public int retRoactivetype;
		public int propID;
		public int count;
		public int reday;
	}

	public class FiRetroactiveReward
	{
		public int result;
		public FiDailySignIn singIn = new FiDailySignIn();
		public List<FiProperty> properties = new List<FiProperty>();
	}

	public class FiIsBindPhoneResponse
	{
		public int isBindPhone;
		public string strPhoneNum;
	}

	//获取所有已经使用的 限时道具信息
	public class FiUserGetAllPropTimeResponse
	{
		//道具ID
		public int propID;
		//道具类型
		public int propType;
		//道具使用时间
		public long useTime;
		//道具剩余时间
		public long remainTime;
		//限时道具总时间
		public int propTime;
	}

	public class FiUseProTimeArr
	{
		public List<FiUserGetAllPropTimeResponse> allProp = new List<FiUserGetAllPropTimeResponse>();
	}

	//使用限时道具
	public class FiUseProTimeRequest
	{
		public int userID;
		public int resultCode;
		public FiUserGetAllPropTimeResponse useProp;
	}

	//使用道具请求
	public class FiUsePropTimeResponse
	{
		public int userID;
		public int resultCode;
		public FiUserGetAllPropTimeResponse useProp;
	}
	//删除道具
	public class FiDelPropTimeResponse
	{
		public int userID;
		public int resultCode;
		public int delPropID;
	}

	public class FiRewardStructure
	{
		public int RewardType;
		public int TaskID;
		public int TaskValue;
		public List<FiProperty> rewardPro;
	}

	public class FiRewardAllData
	{
		public List<FiRewardStructure> rewardAll;
	}
	//龙卡信息 数组的序列代表卡 数组的值0 为未做操作,正数代表天数
	public class FiDraGonRewardData
	{
		public int cannonmultiplemax;
		public List<int> DarGonCardDataArray = new List<int>();
	}
	//购买后的龙卡信息
	public class FiPurChaseDraGonRewradData
	{
		public int result;
		public long total_recharge;
		//这是
		public int current_vip;
		public int cardType;
		public int userid;
		public int cannonmultiplemax;
		public List<FiProperty> prop = new List<FiProperty>();
	}
	//特惠接收数据信息
	public class FiPreferentialData
	{
		public int cannonmultiplemax;
		//接受特惠当前显示的哪个界面
		public List<int> preferentialDataArray = new List<int>();
	}
	//购买teihuixieyi
	public class FiPurChaseTehuiRewradData
	{
		public int result;
		public long total_recharge;
		//这是
		public int current_vip;
		public int cardType;
		public int userid;
		public int cannonmultiplemax;
		public List<FiProperty> prop = new List<FiProperty>();
	}

	/// <summary>
	/// 兑换炮座
	/// </summary>
	public class FiExchangeBarbette
	{
		public int buyType;
		public long goldCost;
		public int result;

		public override string ToString()
		{
			return "{ " +
			" buyType:" + buyType +
			" goldCost:" + goldCost +
			" result:" + result +
			" }";
		}
	}

	/// <summary>
	/// 改变炮座
	/// </summary>
	public class FiChangeBarbetteStyle
	{
		public int EquipmentType;
		public long propID;
		public int result;
		public int removeChangeBarbetteStyle;

		public override string ToString()
		{
			return "{ " +
			" EquipmentType:" + EquipmentType +
			" propID:" + propID +
			" result:" + result +
			" removeChangeBarbetteStyle:" + removeChangeBarbetteStyle +
			" }";
		}
	}

	/// <summary>
	/// 机器人使用分身
	/// </summary>
	public class FiChangeRobotReplicationFishID
	{
		public int robotFishID;
		public List<FiProperty> target = new List<FiProperty>();

		public override string ToString()
		{
			string content = "{ " +
							 " robotFishID:" + robotFishID +
							 " target:";
			foreach (FiProperty property in target) {
				content += target.ToString();
			}
			content += " }";
			return content;
		}
	}

	/// <summary>
	/// 财神接收下注显示
	/// </summary>
	public class GetManmonChipGolds
	{
		public long userid;
		//当前金币数量
		public long curGold;

		public int result;
		//投注的数组显示金币
		public List<long> shipGoldDataArray = new List<long>();
		//显示元宝点亮数量
		public int nManmonCount;

	}


	public class FiFishRoomShow
	{
		public long userId;
		public int Startnum;

		public override string ToString()
		{
			return "{ " +
			" userId:" + userId +
			" Startnum:" + Startnum +
			" }";
		}
	}
	//下注显示和摇钱树的显示都用这个
	public class FiFishGoldShow
	{
		public long userid;
		//当前身上金币数量
		public long selfGold;
		//显示的奖励金币
		public long nWinGold;
		//下注区域
		public int chipIndex;
		//结果
		public int result;
		//显示差值
		public long nChaValue;
		//战时不用
		public long nTax;

		public int showTime;
		//显示剩余元宝
		public int nManmoncount;

		public double mbeishu;
	}
	//下注显示和摇钱树的显示都用这个
	public class FiFishYGoldShow
	{
		public long userid;
		//当前身上金币数量
		public long selfGold;
		//显示的奖励金币
		public long nWinGold;
		//下注区域
		public int chipIndex;
		//结果
		public int result;
		//显示差值
		public long nChaValue;
		//战时不用
		public long nTax;
		//显示连胜次数
		public int showTime;
		//显示剩余元宝
		public int nManmoncount;
		//新加显示奖励倍数
		public double mbeishu;
	}

	public class FiFishGetCoinPool
	{

		public long userID;
		//用户ID
		public int result;
		public int type;
		public List<long> saveLongRewardPoolGold = new List<long>();
	}

	public class FishGetRankReward
	{
		public int result;
		public List<FiProperty> rankcout = new List<FiProperty>();
	}

	public class FishGetGoldLiuShui
	{
		public long lUserID = 1;
		public long lLongLiuShui = 2;
		public long lTimeData = 3;
		public long lManmonExp = 4;
		public int ShengJiDuanWei = 5;
		public int nIsUserTopUpState = 6;
		public int nTwoSelectOneTopUpState = 7;
		public int nThreeSelectOneTopUpdate = 8;
		public int nChangeCurDuanWei = 9;
		public int nCurRank = 10;
		public int nCurMax = 11;
		public int nSevenTaskID = 12;
		public long nSevenTaskValue = 13;
	}

	public class FishTurnTableLuckyDraw
	{
		public int result = 1;
		public int type = 2;
		public List<FiPropertyEx> prpo2 = new List<FiPropertyEx>();
	}

	public class FishChangeLiuShuiTime
	{
		public long lUserID = 1;
		public int lDiamond = 2;
		public int nReuslt = 3;
		public long nTimeData = 4;
	}

	public class InformOtherCancelSKill
	{
		public long lUserID = 1;
		public int nState = 2;
		public int skillID = 3;
	}

	public class FishingUserRank
	{
		public long lUserID;
		public int nRank;
		public int vip;
		public int longCard;
		public long nGold;
		public long nRewardGold;
		public ByteString nickname;

		//public override byte[] serialize()
		//{
		//    UserRankInfo nSend = new UserRankInfo();
		//    nSend.LUserID = lUserID;
		//    nSend.NRank = nRank;
		//    nSend.Vip = vip;
		//    nSend.LongCard = longCard;
		//    nSend.NGold = nGold;
		//    return nSend.ToByteArray();
		//}
	}

	public class MamonMaxWinCounts
	{
		public long useid = 1;
		public int wintime = 2;
	}

	/// <summary>
	/// 服务器主动下发赋值操作
	/// </summary>
	public class FiChangeUserGold
	{
		public int userID;
		public int propertyID;
		public long count;

		public override string ToString()
		{
			return "{ " +
			" userID:" + userID +
			" propertyID:" + propertyID +
			" count:" + count +
			" }";
		}
	}

	/// <summary>
	/// 服务器主动下发Boss场匹配
	/// </summary>
	public class FiBossRoomMatchInfo
	{
		public int type;
		public ByteString content;
		public long selfGold;
		public List<int> roomArrayID;
	}

	/// <summary>
	/// 接受发送匹配和离开消息
	/// </summary>
	public class FiNotifySignUp
	{
		public int type;
		public int roomIndex;
		public long gameType;
		public long signUpGold;
	}

	/// <summary>
	/// 引导进入boss场
	/// </summary>
	public class IntoBossRoomMessage
	{
		public int nType;
		public int nRoomIndex;
		public ByteString modifyNick;
	}

	/// <summary>
	/// Boss猎杀排名
	/// </summary>
	public class FishingBossKillRank
	{
		public long lUserID = 1;
		public int nRank = 2;
		public int vip = 3;
		public int longCard = 4;
		public int nGold = 5;
		public ByteString nickname;
	}

	/// <summary>
	/// Boss场时间
	/// </summary>
	public class FiUpdateBossMatchTime
	{
		public long chaTime;
		public long startTime;
		public long endTime;
		public int roomIndex;

		public override string ToString()
		{
			return "{ " +
			" chaTime:" + chaTime +
			" startTime:" + startTime +
			" endTime:" + endTime +
			" roomIndex:" + roomIndex +
			" }";
		}
	}

	/// <summary>
	/// Boss场结束Rank提示
	/// </summary>
	public class FiUserRankArray
	{
		public List<FishingUserRank> rankArray;
		public ByteString content;
	}
	//	GameRank
	public class PaiWeiSaiRankInfos
	{
		public int result;
		public int duanwei;
		public int nrank;
		public int isTopUp;
		public int monthCardtype;
		public int bossmatchdouble;
		public long shenyutime;
		public int shangqipaiming;
		public int lishizuigao;
		public int qishu;
		public int beiqizuigao;
		public List<FiRankInfo> rankList = new List<FiRankInfo>();
	}

	public class RongYuDianTangRanInfo
	{
		public int result;
		public long hotPrizePool;
		public List<FiRankInfo> rongYuRankList = new List<FiRankInfo>();

	}

	public class PaiWeiPrizeInfo
	{
		public int rewardIndex = 1;
		public int rewardState = 2;
		public int curCatchFishNum = 3;
		public int maxCatchFishNum = 4;
		public List<FiProperty> rewardData = new List<FiProperty>();
	}

	public class PaiWeiPrizeState
	{
		public List<int> rewardList = new List<int>();
	}


	public class GetTopUpGiftBagState
	{
		public int one_giftBagState;
		//超给力状态
		public int two_select_oneBagState;
		//双喜临门状态
		public int three_select_oneBagState;
		//发现宝藏礼包状态
	}

	public class InitSevenDayInfos
	{
		public int result;
		public long curDay;
		public int userDay;
		public int userDayState;
		public int taskDay;
		public long taskValue;
		public int taskDayState;
		public int userGiftDay;
		public int userGiftDyaState;
	}


	/// <summary>
	/// 七日礼包的接受和发送
	/// </summary>
	public class FiSevenDaysPage
	{
		//发送0代表签到1代表任务
		public int SendIndex;
		public int UserDay;
		public List<FiProperty> target = new List<FiProperty>();
	}

	/// <summary>
	/// 取得新手升級獎勵資訊 ---種類
	/// </summary>
	public class DbGetUpLevelActivityInfos
	{
		public List<UpLevelTaskInfos> levelList = new List<UpLevelTaskInfos>();
	}


	/// <summary>
	/// 取得新手升級獎勵資訊 ---細節
	/// </summary>
	public class UpLevelTaskInfos
	{
		public int taskID;
		public int taskCurValue;
		public int taskMaxValue;
		public int rewardState;
		public long showInfoMaxValue;
	}

	/// <summary>
	/// 發送用戶領取升級獎勵
	/// </summary>
	public class UpLevelRewards
	{
		public int taskID;
	}

	/// <summary>
	/// 接收用戶領取升級獎勵
	/// </summary>
	public class UpLevelRewardGets
	{
		public int result;
		public int taskID;
		public int taskLevel;
		public long gold;
	}

	/// <summary>
	/// 按钮状态,Index
	/// 0.防沉迷开关(0. 是开;1.是关)
	/// 1.排行榜按钮(0.是关;1.是开)
	/// 2.商城购买金币档位
	/// </summary>
	public class GetButtonState : IDataProxy
	{
		public long Count;
		public List<int> nButtonStateArray = new List<int>();

		void IDataProxy.OnAddData(int nType, object nData)
		{

		}

		void IDataProxy.OnDestroy()
		{

		}

		void IDataProxy.OnInit()
		{

		}
	}

    /// <summary>
    /// 下發付款、金幣陣列
    /// </summary>
	public class PropPayInfoArray
	{
		public List<PropPayInfo> payInfoArray = new List<PropPayInfo>();
	}

	/// <summary>
	/// 下發付款、金幣陣列結構
	/// </summary>
	public class PropPayInfo
    {
		public int changeNum;
		public int payType;
		public int id;
		public int rmb;
		public long addGold;
    }

    /// <summary>
    /// 手機號碼登入
    /// </summary>
	public class PhoneNumberLoginArray {
		public int result;
		public List<PhoneNumberLoginInfo> PhoneNumberInfo = new List<PhoneNumberLoginInfo>();
	}

    /// <summary>
    /// 設定手機號碼帳號密碼
    /// </summary>
	public class PhoneNumberPass
	{
		public int result;
		public List<PhoneNumberLoginInfo> PhoneNumberPassInfo = new List<PhoneNumberLoginInfo>();
	}

    /// <summary>
    /// 根據手機號碼關聯的所有帳號資訊
    /// </summary>
	public class PhoneNumberLoginInfo {
		public int resutl;
		public int accountType;
		public int user_id;
		public string accountName;
		public string strToken;
		public string nickname;
	}

    /// <summary>
    /// 選擇的關連帳號資訊回傳
    /// </summary>
	public class LoginAccountAssociateChoise
	{
		public int result;
		public int accountType;
		public int user_id;
		public string accountName;
		public string strToken;
		public string nickname;
	}

    /// <summary>
    /// 手機
    /// </summary>
	public class ConvertFormalPhoneNumber
	{
		public string phone;
		public string code;
	}

    /// <summary>
    /// 
    /// </summary>
	public class SetPhoneNumberPassword
	{
		public string phone;
		public string pass;
	}

	/// <summary>
	/// 隨機的暱稱選項
	/// </summary>
	public class LoginAccountNickChoice
	{
		public int languageType;
		public List<string> nickArray = new List<string>();
	}

	/// <summary>
	/// 設定手機號碼、密碼、暱稱
	/// </summary>
	public class SetPhoneLoginPass
	{
		public string phone;
		public string pass;
		public string nickname;
	}
}