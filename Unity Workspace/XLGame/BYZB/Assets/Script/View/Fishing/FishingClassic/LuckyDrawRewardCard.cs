using UnityEngine;
using System.Collections;
using UnityEngine.UI;


public class LuckyDrawRewardCard : MonoBehaviour
{

	public int cardIndex;

	CardMsg myMsg;

	Text topNameText;
	Text bottomNumText;
	Image centerIconImage;

	void Init ()
	{ //初始化获取组件
		topNameText = transform.Find ("TopText1").GetComponent<Text> ();
		bottomNumText = transform.Find ("BottomText1").GetComponent<Text> ();
		centerIconImage = transform.Find ("CenterImage1").GetComponent<Image> ();
	}

	public void SetUIData (string topName, Sprite centerImage, int bottomNum)
	{ //设置UI信息显示
		topNameText.text = topName;
		centerIconImage.sprite = centerImage;
		bottomNumText.text = bottomNum.ToString ();
	}

	public void SetCardMsg (CardMsg _msg) //把预先写好的msg赋值显示
	{
		Init ();
		myMsg = _msg;
		SetUIData (_msg.cardTopText, _msg.cardSprite, _msg.cardBottomText);
	}

	/*public bool MatchSuccess (int rewardTypeId, int rewardNum) //匹配是否为服务器的奖励
	{
		if (rewardTypeId == myMsg.cardType && rewardNum == myMsg.cardBottomText) {
			return true;
		} else {
			return false;
		}

	}*/

}
