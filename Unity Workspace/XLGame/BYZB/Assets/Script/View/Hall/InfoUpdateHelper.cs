using System;

namespace AssemblyCSharp
{
	public class InfoUpdateHelper
	{
		private bool isMessageSend = false;

		public InfoUpdateHelper ()
		{
		}


		public void DoUpdateInfoRequest()
		{
			//获取邮件相关参数 83
			Facade.GetFacade ().message.mail.SendGetSystemMailRequest ();
            // 43
			Facade.GetFacade ().message.mail.SendGetPersentRecordRequest ();
			//获取好友申请的数据 38
			Facade.GetFacade ().message.friend.SendGetFriendApplyList ();
            // 63
			Facade.GetFacade ().message.bank.SendGetBankMessageRequest ();
			//任务系统只需要到大厅的时候，请求一次就可以了
			if (!isMessageSend) {
				Facade.GetFacade ().message.task.SendTaskProcessRequest ();
				isMessageSend = true;
			}
			//UnityEngine.Debug.LogError ( "--------------update client info----------------" );
		}

	}
}

