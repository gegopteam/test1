using UnityEngine;
using System.Collections;

public class RoomAttribute{
    /// <summary>
    /// The coin.
	/// 子弹模式：public int coin;//进入房间的金币数量
	///         public int bullet;//进入房间子弹的数量
	///         public int person;//房间人数
	///         public string passWord;//进入房间密码，如果密码为空则无密码
	///         public string regestTime;//注册时间
	/// 
	/// 时间赛： public int coin;//进入房间的金币数量
	///         public int person;//房间人数
	///         public string passWord;//进入房间密码，如果密码为空则无密码
	///         public string regestTime;//注册时间
	///         public int gameTime;//游戏时间
	/// 
	/// 积分赛／房卡赛：public string passWord;//进入房间密码，如果密码为空则无密码
	///              public int gameTime;//游戏时间
	///              public int Round;//游戏局数
	///              public string regestTime;//注册时间
	///         
    /// </summary>
	public int roomName;//房间名称
	public int roomNumber;//房间号，系统自动分配
	public int coin;//进入房间的金币数量
	public int bullet;//进入房间子弹的数量
	public int person;//房间人数
	public string passWord;//进入房间密码，如果密码为空则无密码
	public string regestTime;//注册时间
	public int gameTime;//游戏时间
	public int round;//游戏局数

	//子弹模式
	public RoomAttribute(int _roomName,int _coin,int _bullet,int _person,string _password,string _regestTime)
	{
		roomName = _roomName;
		coin = _coin;
		bullet = _bullet;
		passWord = _password;
		regestTime = _regestTime;
	}
	//时间赛
	public RoomAttribute(int _roomName,int _coin,int _person,string _password,string _regestTime,int _gameTime)
	{
		roomName = _roomName;
		coin = _coin;
		person = _person;
		passWord = _password;
		regestTime = _regestTime;
		gameTime = _gameTime;

	}
	//房卡赛
	public RoomAttribute(int _roomName,string _password,string _regestTime,int _gameTime,int _round)
	{
		roomName = _roomName;
		passWord = _password;
		regestTime = _regestTime;
		gameTime = _gameTime;
		round = _round;
	}
}
