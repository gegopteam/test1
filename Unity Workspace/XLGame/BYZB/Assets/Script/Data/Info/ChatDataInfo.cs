using UnityEngine;
using System.Collections;
using AssemblyCSharp;
using System.Collections.Generic;

public class ChatDataInfo : IDataProxy {

	private List<FiChatMessage> msgList=new List<FiChatMessage>();

	public List<FiChatMessage> getChatList(){
		return msgList;
	}
	public void addChatMsg(FiChatMessage msg){
		//这里可以进行判断最多保存多少条数据，if(count==10)   removeat 0  add new 
		msgList.Add (msg);
	}
	//这个函数在退出房间时调用
	public void ClearChatMsg(){
		msgList.Clear ();
	}
	public void OnAddData( int nType , object nData ){}
	public  void OnInit(){}

	public void OnDestroy(){}
}
