using UnityEngine;
using System.Collections.Generic;
using AssemblyCSharp;

public class BankInfo : IDataProxy {
	bool update=false;
	List<FiBankMessageInfo> BankMessageList = new List<FiBankMessageInfo> ();

	public  void OnAddData( int nType , object nData ){
		List<FiBankMessageInfo> newList = (List<FiBankMessageInfo>)nData;
		if (newList == null||BankMessageList.Count==0)
			return;
		if (newList.Count != BankMessageList.Count)
			update = true;
		Debug.LogError ("update:~~~~~~" + update);
	}

	public  void OnInit(){
	}

	public  void OnDestroy(){}

	public void SetMsgList(List<FiBankMessageInfo> msgList){
		msgList.Sort (CompareByDate);

		BankMessageList = msgList;
	}
	public void AddMessage(FiBankMessageInfo message){
		BankMessageList.Add (message);
		update = true;
	}
	public List<FiBankMessageInfo> GetMsgList(){
		return BankMessageList;
	}
	public bool isUpdate{
		get{
			return update;
		}
		set{ 
			update = value;
		}
	}


	public static int CompareByDate(FiBankMessageInfo x, FiBankMessageInfo y)//从da到xiao排序器  
	{  
		if (x == null)  
		{  
			if (y == null)  
			{  
				return 0;  
			}  

			return 1;  

		}  
		if (y == null)  
		{  
			return -1;  
		}  
		int retval = y.dateTime.CompareTo(x.dateTime);  
		return retval;  
	}  
}
