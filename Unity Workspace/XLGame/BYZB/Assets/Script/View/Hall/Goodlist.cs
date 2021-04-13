using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using AssemblyCSharp;
/// <summary>
/// Goodlist.背包数据以及申请、游戏以及添加数据的List
/// </summary>
public class Goodlist {
	private static Goodlist goodList;
	public static Goodlist GetInstance()
	{
		if(goodList == null)
		{
			goodList = new Goodlist ();
		}

		return goodList;
	}
	//public List<FiBackpackProperty> GoodList = new List<FiBackpackProperty>();
	//public static List <> AddFriendList = new List<>();//添加好友
	//public List <FiFriendInfo> ApplyList = new List<FiFriendInfo>();//申请好友
	//public List <FiFriendInfo> GameFriendList = new List<FiFriendInfo>();//游戏好友
	public List<FiRankInfo> RankingList = new List<FiRankInfo>();//排行榜，每半个小时刷新一次
	public List <FiEverydayTaskDetial>TaskDetialList = new List<FiEverydayTaskDetial>();//任务信息的详情
	//public List <FiSystemMail> SystemMailList = new List<FiSystemMail>();//获得系统邮件
	//public List<FiPresentRecord> GiveMailList = new List<FiPresentRecord>();//赠送记录
}
