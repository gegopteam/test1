
//消息类型----------------------------------------
public enum MSG
{
	LOGIN = 1000,
}

//数据结构----------------------------------------

public struct stSndHead
{
//	int msg; //消息类型
//	int len; //信息包大小
}

public struct stRcvHead
{
//	int msg; //消息类型
//	int len; //信息包大小
//	int result; //请求结果，成功失败或其他
//	char[] info = new char[30]; //返回信息
}

//登录请求信息
public struct stLogin
{
//	char[] szLoginName = new char[30];
//	char[] szPassword = new char[30];
	//可选
//	int loginType; //normal,wechat,qq,sina
//	int systemType; //ios,android,mac,windows
//	int deviceType; //iphone,huawei,meizu,mi
}

//登录成功后服务器返回的信息
public struct stMyserInfo
{
//	char[] szNickname = new char[30]; //昵称
//	int userID; //ID编号
//	bool sex; //性别：false girl, true boy
//
//	int gradeNormal; //普通等级
//	int gradeVIP; //VIP等级
//	int gradeCannon; //炮等级
//
//	int coin; //金币
//	int brilliant; //钻石
}

//其他人的用户信息
public struct stUserInfo
{
//	char[] szNickname = new char[30]; //昵称
//	int userID; //ID编号
//	bool sex; //性别：false girl, true boy
//
//	int gradeNormal; //普通等级
//	int gradeVIP; //VIP等级
//	int gradeCannon; //炮等级
//
//	int coin; //金币
//	int brilliant; //钻石
}
