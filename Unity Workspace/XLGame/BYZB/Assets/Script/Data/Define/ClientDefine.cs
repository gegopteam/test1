
//public delegate void EventHandler(object data, int type=0);
using UnityEngine;

public class AppInfo
{
	/*
     * ADHUB-钧云科技   -- 渠道号：51000011         adhub
     * http://admin.xinlongbuyu.com/GameAPI/API.aspx?ajaxaction=CheckVersion_New
     * 百度商桥         -- 渠道号：51000021		baidu
     * http://xladmin.lhzdbg.cn/GameAPI/API.aspx?ajaxaction=checkversion_new1
     * 内地网络         -- 渠道号：51000031		nadi
     * http://xladmin.lhzdbg.cn/GameAPI/API.aspx?ajaxaction=checkversion_new2
     * 昭通网络         -- 渠道号：51000041		jautung
     * http://xladmin.lhzdbg.cn/GameAPI/API.aspx?ajaxaction=checkversion_new3
     * 点易广告联盟      -- 渠道号：51000051	        de
     * http://xladmin.lhzdbg.cn/GameAPI/API.aspx?ajaxaction=checkversion_new4
     * 深茂网络推广      -- 渠道号：51000061	        sm
     * http://xladmin.lhzdbg.cn/GameAPI/API.aspx?ajaxaction=checkversion_new5
     * 有米科技         -- 渠道号：51000071		ym
     * http://xladmin.lhzdbg.cn/GameAPI/API.aspx?ajaxaction=checkversion_new6
     * 峰合网络         -- 渠道号：51000081		fenghec
     * http://xladmin.lhzdbg.cn/GameAPI/API.aspx?ajaxaction=checkversion_new7
     * 香港商亚思        -- 渠道号：51000091	    yass
     * http://xladmin.lhzdbg.cn/GameAPI/API.aspx?ajaxaction=checkversion_new8
     * 恒顺广告联盟      -- 渠道号：51000101	    hengshun
     * http://xladmin.lhzdbg.cn/GameAPI/API.aspx?ajaxaction=checkversion_new9
     * 线上包          -- 渠道号：51000000           Online
     * http://admin.xinlongbuyu.com/GameAPI/API.aspx?ajaxaction=CheckVersion
     * 繁體包          -- 渠道号：51000002           TC
     * https://xlconfig.lhzdbg.cn/GameAPI/API.aspx?ajaxaction=CheckVersion1
     */
	//端口号
	//	public static int portNumber = 50776;

	//正式版 port v1.6.6.0
	public static int portNumber = 50666;

    //推廣版 port 
    //public static int portNumber = 50668;
    //public static int portNumber = 50777;
    //public static int portNumber = 50701;

    //大厅端口号
    public static int hallPortNumber = 51000;

	//public const int version = 105;
	//传给服务器的版本号
	public const int version = 10609006;
	//100 -> 1.0.0
#if UNITY_IPHONE
	//GUI显示版本号
	public const string appVersion = "1.6.9.8";
	//ios渠道号
	public const long trenchNum = 51000002;
#elif UNITY_ANDROID
	//GUI显示版本号
	public const string appVersion = "1.6.9.8";
	//安卓渠道号
	//public const long trenchNum = 51000000;

    //推廣版 android channel number
	public const int trenchNum = 51000002;

#endif



#if UNITY_YSDK
           public const long trenchNum = 51000011;
#endif

#if UNITY_OPPO
        public const long trenchNum = 51000011;
#endif

#if UNITY_Huawei
        public const long trenchNum = 51000011;
#endif

#if UNITY_VIVO
        public const long trenchNum = 51000011;
#endif

	public static bool isInHall = true;
	public static bool isReciveHelpMsg = false;
	public static bool isFrist = true;
	public static bool isFritsInGame = true;
	public static string phoneUUID;

	public static string GuestDeviceID;
}

public enum AppView
{
	//场景界面
	NONE,
	LOAD,
	LOGIN,
	HALL,
	FISHING,
	PKHALLMAIN,
	HALLROOMCARD,
	CLASSICHALL,
	PKHALL,
	ROOM,
	LOADING,
	HALLNEWPLAY,
}

public class ProductPayId
{
	public const int GLOD_6 = 1001;
	public const int GLOD_30 = 1002;
	public const int GLOD_68 = 1003;
	public const int GLOD_118 = 1004;
	public const int GLOD_198 = 1005;
	public const int GLOD_348 = 1006;
}

public enum AppFun
{
	//功能模块消息分发标识
	//大模块
	DATA,
	LOGIN,
	HALL,
	FISHING,

	//小模块
	UIHALL_PKROOM,
	UIHALL_INPKROOM,
	//房间准备界面
	UIHALL_ENTER_FRIENDROOM,
	UIHALL_CREATE_FRIENDROOM,
	//	UIHALL_STORE,
	//	UIHALL_IOSPURCHASE_INIT, //IOS内购初始化完成
	//	UIHALL_IOSPURCHASE_PAY, //IOS内购购买完成
}

public enum AppLogin
{
	//登录界面功能消息分发标识
	
}

public enum AppHall
{
	//登录界面功能消息分发标识

}

public enum AppFishing
{
	//登录界面功能消息分发标识

}

public enum AppWindow
{
	//场景窗口模块
	LOGINWINDOW,
}

public struct CannonMultiple
{
	//炮的倍数
	public const int NEWBIE = 100;
	//新手海湾
	public const int DEEPSEA = 50;
	//深海遗址
	public const int POSEIDON = 1000;
	//海神宝藏
	public const int GOLDMEDAL = 9900;
}

public class TypeFishing
{
	//捕鱼房间类型
	public const int CLASSIC = 0;
	public const int REDPACKET = 1;
	public const int PKBullet = 4;
	public const int PKTime = 5;
	public const int PKPoint = 6;
	public const int PKFriendGold = 10;
	public const int PKFriendCard = 11;
	public const int BOSS = 12;
}

public class LevelFishing
{
	//游戏等级
	public const int LEVEL_1 = 1;
	public const int LEVEL_2 = 2;
	public const int LEVEL_3 = 3;
}

public class GameInfo
{
	//游戏信息
	public int type;
	public int level;

	public GameInfo ()
	{
		type = 0;
		level = 0;
	}

	public GameInfo (int type, int level)
	{
		this.type = type;
		this.level = level;
	}

	public GameInfo (GameInfo info)
	{
		type = info.type;
		level = info.level;
	}
}


public enum WebUrlEnum
{
	/// <summary>
    /// 
    /// </summary>
    Zero = 0,

    /// <summary>
    /// 
    /// </summary>
    One = 1,

	/// <summary>
	/// 其他不需要保存
	/// </summary>
	Other = 2,

	/// <summary>
	/// 小游戏入口链接
	/// </summary>
	SmallGame = 3,

	/// <summary>
	/// 后台配置管理
	/// </summary>
	Setting = 4,

	/// <summary>
	/// 支付链接
	/// </summary>
	PayWeb = 5,

	/// <summary>
	/// 正式服活动地址
	/// </summary>
	Activty = 6,

	/// <summary>
	/// 测试服活动地址
	/// </summary>
	ActivtyTest = 7,

	/// <summary>
	/// 手机注册
	/// </summary>
	RegisterWithPhone = 8,

	/// <summary>
	/// 域名位置
	/// </summary>
	LoginAddrIp = 9,

	/// <summary>
	/// 推廣用的商城配置網址
	/// </summary>
	PromotePayWeb = 10,

	/// <summary>
	/// 客服熱線
	/// </summary>
	PhoneNumber = 11,

	/// <summary>
	/// 第三方登入token
	/// </summary>
	ThirdToken = 12,

	/// <summary>
	/// 在線客服連結
	/// </summary>
	OnlineCostomerService = 13,

	/// <summary>
	/// 下發配置連結1
	/// </summary>
	Configuration_back1 = 14,

	/// <summary>
	/// 小助手連結
	/// </summary>
	LittleHelper = 15,

	/// <summary>
	/// 獲取驗證碼連結
	/// </summary>
	MMSAuth = 16,

	/// <summary>
	/// 其他登入顯示控制
	/// </summary>
    OtherLogin = 17,

	/// <summary>
	/// 小遊戲重新登入
	/// </summary>
	SmallRelogin = 18
}
		
