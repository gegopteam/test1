using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using AssemblyCSharp;
using System.Runtime.InteropServices;
using System;

namespace AssemblyCSharp
{
	public class PayInfoBody : IMsgHandle
	{
		MyInfo myInfo;

		public PayInfoBody()
		{

		}

		public void OnInit()
		{
			Debug.Log("~~~~~PayInfoBody~~~~~");
			myInfo = (MyInfo)Facade.GetFacade().data.Get(FacadeConfig.USERINFO_MODULE_ID);
			EventControl nControl = EventControl.instance();
			nControl.addEventHandler(FiProtoType.XL_GET_PAY_INFO, RecvPayInfoBack);
		}

		// Update is called once per frame
		void Update()
		{

		}

		/// <summary>
		/// 回调
		/// </summary>
		/// <param name="data">Data.</param>
		private void RecvPayInfoBack(object data)
		{
			Debug.Log(" PayInfoBody 接收回调 ");
			PropPayInfoArray infoArray = (PropPayInfoArray)data;
			Debug.Log(" ~~~~~ PropPayInfoArray infoArray Count = "+infoArray.payInfoArray.Count);
			if (infoArray.payInfoArray.Count > 0)
			{
				for (int ArrayIndex = 0; ArrayIndex < infoArray.payInfoArray.Count; ArrayIndex++)
				{
					PropPayInfo payinfo = (PropPayInfo)infoArray.payInfoArray[ArrayIndex];
					//if (payinfo.changeNum == AppInfo.version) {

						switch (payinfo.payType)
						{
							case 1:
							    if (!myInfo.Pay_Drang_id.Contains(payinfo.id))
							    {
								    myInfo.Pay_Drang_RMB.Add(payinfo.rmb);
								    myInfo.Pay_Drang_AddGold.Add(payinfo.addGold);
								    myInfo.Pay_Drang_id.Add(payinfo.id);
							    }
							    break;
							case 2:
							    if (!myInfo.Pay_Preferential_id.Contains(payinfo.id))
							    {
								    myInfo.Pay_Preferential_RMB.Add(payinfo.rmb);
								    myInfo.Pay_Preferential_AddGold.Add(payinfo.addGold);
								    myInfo.Pay_Preferential_id.Add(payinfo.id);
							    }
							    break;
							case 3:
							    if (!myInfo.Pay_Three_id.Contains(payinfo.id))
							    {
								    myInfo.Pay_Three_RMB.Add(payinfo.rmb);
								    myInfo.Pay_Three_AddGold.Add(payinfo.addGold);
								    myInfo.Pay_Three_id.Add(payinfo.id);
							    }
							    break;
							case 4:
							    if (!myInfo.Pay_Two_id.Contains(payinfo.id))
							    {
								    myInfo.Pay_Two_RMB.Add(payinfo.rmb);
								    myInfo.Pay_Two_AddGold.Add(payinfo.addGold);
								    myInfo.Pay_Two_id.Add(payinfo.id);
							    }
							    break;
							case 5:
							    if (!myInfo.Pay_NewSeven_id.Contains(payinfo.id))
							    {
								    myInfo.Pay_NewSeven_RMB.Add(payinfo.rmb);
								    myInfo.Pay_NewSeven_AddGold.Add(payinfo.addGold);
								    myInfo.Pay_NewSeven_id.Add(payinfo.id);
							    }
							    break;
							case 6:
							    if (!myInfo.Pay_Store_id.Contains(payinfo.id)) {
								    myInfo.Pay_Store_RMB.Add(payinfo.rmb);
								    myInfo.Pay_Store_AddGold.Add(payinfo.addGold);
								    myInfo.Pay_Store_id.Add(payinfo.id);
							    }
							    break;
						}
					//}
				}
			}
		}

		public void OnDestroy()
		{
			EventControl nControl = EventControl.instance();
			nControl.removeEventHandler(FiProtoType.XL_GET_PAY_INFO, RecvPayInfoBack);
		}
	}
}
