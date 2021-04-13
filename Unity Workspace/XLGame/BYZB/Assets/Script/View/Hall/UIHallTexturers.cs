using UnityEngine;
using System.Collections;
using AssemblyCSharp;
using UnityEngine.UI;

public class UIHallTexturers : MonoBehaviour
{

	public static UIHallTexturers instans;

	public Sprite[] DaiKuangProperty;
	public Sprite[] Notice;
	//道具
	public Sprite[] IconProperty;
	//炮台


	public Sprite[] IconCannon;
	public Sprite[] IconTorpedo;
	public Sprite[] IconGift;

	public Sprite[] Bank;
	public Sprite[] Mail;
	public Sprite[] Ranking;
	public Sprite[] Vip;
	public Sprite[] Task;
	public Sprite[] RankNum;
	public Sprite[] VipNum;
	public Sprite[] VipFrame;
	public Sprite[] FirstRecharge;
	public Sprite[] Sign;
	public Sprite[] SignRewardType;
	public Sprite[] ContinueDaySign;

	/// <summary>
	/// 限时道具
	/// </summary>
	public Sprite[] PropTimeIcon;
	/// <summary>
	///领取奖励类型
	/// </summary>
	public Sprite[] RewardTypeIcon;
	/// <summary>
	/// 炮座道具
	/// </summary>
	public Sprite[] barbettePropTimeIcon;

	//公告面板
	void Awake ()
	{
		instans = this;
	}

}
