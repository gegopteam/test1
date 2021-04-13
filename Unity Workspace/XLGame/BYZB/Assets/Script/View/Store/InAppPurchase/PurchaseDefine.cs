
public delegate void PurchaseEvent (object data, int type = 0);

public class PurchaseMsg
{
	//注：IOS_INIT_BACK和IOS_BUY_BACK需要与ISO部分的编号统一
	public const int IOS_INIT_BACK = 1;
	//IOS内购初始化完成
	public const int IOS_BUY_BACK = 2;
	//IOS内购购买完成
	//	public const int BUY_FOR_IOS = 3; //IOS支付
	//	public const int BUY_FOR_ALIPAY = 4; //支付宝支付
	//	public const int BUY_FOR_WEIXIN = 5; //微信支付
	//	public const int FOR_BUY_BACK = 6; //回复UI购买结果
}

public class PurchaseStep
{
	public const int Ready = 0;
	//准备就绪
	public const int OrderForWeb_Request = 1;
	//请求web订单号
	public const int OrderForWeb_Response = 2;
	//web订单号回复
	public const int BuyProduct_Request = 3;
	//请求ios购买
	public const int BuyProduct_Response = 4;
	//ios购买回复
	public const int ReceiptToWeb_Request = 5;
	//请求验证购买凭证
	public const int ReceiptToWeb_Response = 6;
	//验证购买凭证回复
}

public class PurchaseInfo
{
	public int msg;
	public string data;
}

public class PurchaseData
{
	public int result;
	public string data;
}

public class PurchaseReceipt
{
	public string orderId;
	public string receipt;
}

public class ReceiptInfo
{
	public int userId;
	public int webOrderId;
	public string receiptData;
}

public struct ProductID
{
	//------产品ID名----------------------------------------------------------------------//
	//金币
	public const string Gold_CNY_6 = "Gold_CNY_6";
	public const string Gold_CNY_30 = "Gold_CNY_30";
	public const string Gold_CNY_68 = "Gold_CNY_68";
	public const string Gold_CNY_118 = "Gold_CNY_118";
	public const string Gold_CNY_198 = "Gold_CNY_198";
	public const string Gold_CNY_348 = "Gold_CNY_348";


	public const string CL_GOLD_6 = "CL_GOLD_6";
	public const string CL_GOLD_18 = "CL_GOLD_18";
	public const string CL_GOLD_50 = "CL_GOLD_50";
	public const string CL_GOLD_98 = "CL_GOLD_98";
	public const string CL_GOLD_198 = "CL_GOLD_198";
	// public const string CL_GOLD_298 = "CL_GOLD_298";
	public const string CL_GOLD_488 = "CL_GOLD_488";

	//---
	// public const string Gold_CNY_128 = "Gold_CNY_128";
	// public const string Gold_CNY_328 = "Gold_CNY_328";
	// public const string Gold_CNY_648 = "Gold_CNY_648";

	//钻石
	public const string Diamond_CNY_6 = "Diamond_CNY_6";
	public const string Diamond_CNY_30 = "Diamond_CNY_30";
	public const string Diamond_CNY_68 = "Diamond_CNY_68";
	public const string Diamond_CNY_118 = "Diamond_CNY_118";
	public const string Diamond_CNY_198 = "Diamond_CNY_198";
	public const string Diamond_CNY_348 = "Diamond_CNY_348";
	//---
	// public const string Diamond_CNY_128 = "Diamond_CNY_128";
	// public const string Diamond_CNY_328 = "Diamond_CNY_328";
	// public const string Diamond_CNY_648 = "Diamond_CNY_648";

	//银龙卡
	public const string Dragon_SilverCard = "Dragon_SilverCard";
	//金龙卡
	public const string Dragon_GlodCard = "Dragon_GlodCard";
	//神龙卡
	public const string Dragon_ShenlongCard = "Dragon_ShenlongCard";

	public const string Card_Room_CNY_6 = "Card_Room_CNY_6";
	//房卡
	public const string Pack_Preference_CNY_6 = "Pack_Preference_CNY_6";
	//特惠礼包

	/*//------产品ID----------------------------------------------------------------------//
    //金币
    public const int NGold_CNY_6 = 1001;
    public const int NGold_CNY_30 = 1002;
    public const int NGold_CNY_68 = 1003;
    public const int NGold_CNY_118 = 1004;
    public const int NGold_CNY_198 = 1005;
    public const int NGold_CNY_348 = 1006;

    //钻石
    public const int NDiamond_CNY_6 = 2001;
    public const int NDiamond_CNY_30 = 2002;
    public const int NDiamond_CNY_68 = 2003;
    public const int NDiamond_CNY_118 = 2004;
    public const int NDiamond_CNY_198 = 2005;
    public const int NDiamond_CNY_348 = 2006;

    //卡
	public const int NCard_Room_CNY_6 = 3001; //房卡
    public const int NCard_Month_CNY_28 = 3002; //月卡
    public const int NPack_Preference_CNY_6 = 3003; //特惠礼包
    //*/

}


