
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using AssemblyCSharp;

//大赢家的数据类
public class NewSevenDayInfo : IDataProxy
{
	//这个结果因为 服务器可能会发非0=true判断是否需要弹出新手签到界面
	public int nresult;

	public bool nIsOpenseven = true;
	//显示倒计天数 待用
	public long ncurDay;

	//签到的天数
	public int nuserDay;
	//是否签到
	public int nuserDayState;

	//任务的天数
	public int ntaskDay;
	//任务的值
	public long ntaskValue;
	//是否领取任务
	public int ntaskDayState;
	//礼包的天数
	public int nuserGiftDay;
	//是否购买礼包
	public int nuserGiftDyaState;

	public List<int> SevenDayState;

	public NewSevenDayInfo ()
	{

	}

	~NewSevenDayInfo ()
	{

	}


	public void OnAddData (int nType, object nData)
	{

	}

	public void OnInit ()
	{

	}

	public void OnDestroy ()
	{
//		SevenDayState.Clear ();
	}




}

