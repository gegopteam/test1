using UnityEngine;
using System.Collections.Generic;
using AssemblyCSharp;
using UnityEngine.UI;
using System;

public class BankNoticeItem : ScrollableCell {
	FiBankMessageInfo Info;
	BankInfo nDataInfo;

	public Text content;
	public Text time;


	void Awake(){
		nDataInfo=(BankInfo) Facade.GetFacade ().data.Get ( FacadeConfig.UI_BANk_MOUDLE_ID );
	}
	public override void ConfigureCellData ()
	{
		base.ConfigureCellData ();
		if (dataObject != null) 
			Info = nDataInfo.GetMsgList () [(int)dataObject];
		if (Info != null && this.gameObject.activeInHierarchy) {
			switch (Info.type) {
			case 0://送别人
				content.text = "您購買了<color=#fcfd54ff>" + Info.giftCount + GetGiftName (Info.giftGold) +
					"</color>贈送給<color=#fcfd54ff>" + Info.nickname + "</color>（ID：" + Info.userId + "），該玩家的魅力值增加<color=#fcfd54ff>" + Info.charmChanged + "</color>點！"
					+ "您的銀行存款減少<color=#fcfd54ff>" + Info.bankChanged / 10000 +"</color>萬。";
				break;
			case 1://别人送我fcfd54
				//content.text = "恭喜您收到<color=#fcfd54ff>" + Info.nickname + "</color>（ID：" + Info.userId + "）赠送的<color=#fcfd54ff>" +
				//Info.giftCount + GetGiftName (Info.giftGold) + "</color>，魅力值增加<color=#fcfd54ff>" + Info.charmChanged + "</color>点！请重新登录银行后查看（如对方没有进行微信授权确认，那你可能实际没有收到礼物，请详细确认魅力值数量！）";
				content.text = "您的好友（<color=#fcfd54ff>" + Info.nickname + "</color>，ID:<color=#fcfd54ff>" + Info.userId + "</color>）贈送您<color=#fcfd54ff>" + Info.giftCount + GetGiftName (Info.giftGold) + "</color>！請重新打開銀行查看（如對方沒有授權確認，那您可能沒有收到禮物，請仔細確認魅力值數量！）";
				break;
			case 2://兑换魅力
				content.text= "恭喜您成功兌換<color=#fcfd54ff>" + Info.charmChanged+ "</color>點魅力值，銀行存款增加<color=#fcfd54ff>" + Info.bankChanged/10000+"</color>萬！";
				break;
			case 3://我送别人鱼雷
				content.text= "您贈送給好友（<color=#fcfd54ff>" + Info.nickname + "</color>，ID:<color=#fcfd54ff>" + Info.userId +
					"</color>）<color=#fcfd54ff>" + Info.giftCount + GetTorpedoName (Info.giftGold) +
					"</color>！請到辰龍遊戲公眾號授權！ （如您沒有授權確認，對方將收不到魚雷）";
				break;
			case 4://别人送我鱼雷
				content.text="您的好友（<color=#fcfd54ff>" + Info.nickname + "</color>，ID:<color=#fcfd54ff>" + Info.userId +
					"</color>）贈送您<color=#fcfd54ff>" + Info.giftCount + GetTorpedoName (Info.giftGold) +
					"</color>！請稍後在背包查看（如對方沒有授權確認，那你可能沒有收到魚雷，請仔細確認魚雷數量！）";
				break;
			default:
				break;
			}
			DateTime nSendDate = FiUtil.GetDate (Info.dateTime);
			time.text = FiUtil.GetYearMouthAndDay (nSendDate)+FiUtil.GetBankHourMinute (nSendDate); 
		}
	}

	string GetGiftName(long giftCost){
		switch (giftCost) {
        case 10000:
            return "<color=#ffffffff>個</color>蛋糕";
            break;
		case 100000:
			return "<color=#ffffffff>輛</color>跑車";
			break;
		case 500000:
			return "<color=#ffffffff>支</color>車隊";
			break;
		case 1000000:
			return "<color=#ffffffff>台</color>飛機";
			break;
		default:
			return null;
			break;
		}
	}
	string GetTorpedoName(long giftCost){
		switch (giftCost) {
		case 20000:
			return "<color=#ffffffff>個</color>迷你魚雷";
			break;
		case 100000:
			return "<color=#ffffffff>個</color>青銅魚雷";
			break;
		case 500000:
			return "<color=#ffffffff>個</color>白銀魚雷";
			break;
		case 1000000:
			return "<color=#ffffffff>個</color>黃金魚雷";
			break;
		case 2000000:
			return "<color=#ffffffff>個</color>白金魚雷";
			break;
		case 4000000:
			return "<color=#ffffffff>個</color>核子魚雷";
			break;
		default:
			return null;
			break;
		}
	}
}
