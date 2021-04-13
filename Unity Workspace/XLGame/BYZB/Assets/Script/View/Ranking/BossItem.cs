/***
 *
 *   Title: Boss排行榜单个用户信息
 *
 *   Description: 用来刷新Boss排行信息的Grid
 * 				 (挂载在 -> BossRankWindowCanvas -> Rank -> RankView -> ScrollRect -> Viewport -> Content -> GridImage 中)
 *
 *   Author: bw
 *
 *   Date: 2019.1.30
 *
 *   Modify: 刷新用户的奖励信息,通过服务器下发数据,来进行滚动刷新
 * 
 */
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using AssemblyCSharp;
using System.Text;

public class BossItem : ScrollableCell
{
	/// <summary>
	/// 排名
	/// </summary>
	private Text RankNumText;
	/// <summary>
	/// 玩家名称
	/// </summary>
	private Text UserNameText;
	/// <summary>
	/// 奖励
	/// </summary>
	private Text RewardText;
	/// <summary>
	/// 击杀鱼价值
	/// </summary>
	private Text ValueNumText;
	/// <summary>
	/// 奖励图片  1为钻石 0为金币
	/// </summary>
	private Image RewardImage;
	/// <summary>
	/// 排行榜
	/// </summary>
	FishingUserRank userRank;
	/// <summary>
	/// 奖励Sprite
	/// </summary>
	public Sprite[] rewardSpriteGroup;

	void Awake ()
	{
		//初始化赋值
		RankNumText = transform.Find ("RankNumText").GetComponent <Text> ();
		UserNameText = transform.Find ("UserNameText").GetComponent <Text> ();
		RewardText = transform.Find ("RewardText").GetComponent <Text> ();
		ValueNumText = transform.Find ("ValueNumText").GetComponent <Text> ();
		RewardImage = transform.Find ("RewardImage").GetComponent <Image> ();

	}

	/// <summary>
	/// 继承,用来刷新信息
	/// </summary>
	public override void ConfigureCellData ()
	{
		base.ConfigureCellData ();
//		Debug.LogError ("BossRankingScript.userRankList.count = " + BossRankingScript.userRankList.Count);

		if (dataObject != null)
			userRank = BossRankingScript.userRankList [(int)dataObject];
		if (userRank != null && this.gameObject.activeInHierarchy) {
			//设置排行榜信息
			RankNumText.text = userRank.nRank.ToString ();
			//设置奖励信息
			RewardText.text = userRank.nRewardGold.ToString ();
			//设置击杀鱼价值信息
			ValueNumText.text = userRank.nGold.ToString ();
			//设置昵称
			byte[] byteString = userRank.nickname.ToByteArray ();
			string name = Encoding.UTF8.GetString (byteString);
			if (name == null || name == "") {
				UserNameText.text = Tool.GetName (userRank.lUserID.ToString (), 6);
			} else {
				UserNameText.text = Tool.GetName (name, 5);
			}
			//奖励图片信息
			switch (userRank.longCard) {
			case FiPropertyType.GOLD:
				RewardImage.sprite = rewardSpriteGroup [0];
				RewardImage.SetNativeSize ();
				break;
			case FiPropertyType.DIAMOND:
				RewardImage.sprite = rewardSpriteGroup [1];
				RewardImage.SetNativeSize ();
				break;
			default:
				break;
			}
		}
	}
}
