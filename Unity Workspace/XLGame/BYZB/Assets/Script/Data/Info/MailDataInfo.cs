using System;
using System.Collections.Generic;

namespace AssemblyCSharp
{
	public class MailDataInfo : IDataProxy
	{

		public delegate void ForeachInfo ( object data );

		private List< FiSystemMail > mMailList = new List<FiSystemMail>();

		private List< FiPresentRecord > mRecordList = new List<FiPresentRecord>();

		private bool updated= false;

		private bool bHaveMail = false;
		private bool bHaveRecord = false;

		public MailDataInfo ()
		{
			
		}

		public bool isUpdated()
		{
			return updated;
		}
		public bool isHaveMail(){
			return bHaveMail;
		}
		public bool isHaveRecord(){
			return bHaveRecord;
		}
		public void SetUpdate( bool nValue )
		{
			updated = nValue;
		}
		public List< FiSystemMail > getMailList(){
			return mMailList;
		
		}
		public List< FiPresentRecord > getRecordList(){
			return mRecordList;
		}
		public List<long> getAllMailId()
		{
			List<long> nMailIdList = new List<long> ();
			foreach( FiSystemMail nMail in mMailList ){
				nMailIdList.Add ( nMail.mailId );
			}
			return nMailIdList;
		}

		public List<long> getAllRecordId()
		{
			List<long> nRecordIdList = new List<long> ();
			foreach( FiPresentRecord nRecord in mRecordList ){
				nRecordIdList.Add ( nRecord.id );
			}
			return nRecordIdList;
		}
 
		public void OnAddData( int nType , object nData )
		{
			if (nData != null)
			{
				if (nType == FiEventType.RECV_GET_GIVE_RECORD_RESPONSE) {
					mRecordList.Clear ();
					List< FiPresentRecord > nRecordList = (List< FiPresentRecord >)nData;
					//如果有新邮件，那么给出红点提示
					if (nRecordList.Count > 0) {
						bHaveRecord = true;
					} else {
						bHaveRecord = false;
					}
					foreach( FiPresentRecord nRecord in nRecordList ){
						mRecordList.Insert (0, nRecord );
					}
				} else if (nType == FiEventType.RECV_GET_SYSTEM_MAIL_RESPONSE) {
					mMailList.Clear ();
					List< FiSystemMail > nRecvMailList = (List< FiSystemMail >)nData;
					if (nRecvMailList.Count > 0) {
						bHaveMail = true;
					} else {
						bHaveMail = false;
					}
					foreach( FiSystemMail nMail in nRecvMailList ){
						mMailList.Insert (0, nMail );
					}
				}
			}
			updated = bHaveMail | bHaveRecord;
//			UnityEngine.Debug.LogError ( "------------updated-------------===>" + updated );
		}

		public void ForeachMailData( ForeachInfo nInfoCallBack )
		{
			if (nInfoCallBack == null)
				return;
			IEnumerator<FiSystemMail> nEumPtr = mMailList.GetEnumerator();
			while( nEumPtr.MoveNext() )
			{
				nInfoCallBack.Invoke ( nEumPtr.Current );
			}
		}

		public void ForeachPersentRecord( ForeachInfo nInfoCallBack )
		{
			if (nInfoCallBack == null)
				return;
			IEnumerator<FiPresentRecord> nEumPtr = mRecordList.GetEnumerator();
			while( nEumPtr.MoveNext() )
			{
				nInfoCallBack.Invoke ( nEumPtr.Current );
			}
		}

		public void RemoveMail( int dataId )
		{
			foreach (FiSystemMail nMail in mMailList) {
				if (nMail.mailId == dataId) {
					mMailList.Remove (nMail);
					break;
				}
			}
		}


		public void RemoveRecord( int dataId )
		{
			foreach (FiPresentRecord nRecord in mRecordList) {
				if (nRecord.id == dataId) {
					mRecordList.Remove (nRecord);
					break;
				}
			}
		}


		public void OnInit()
		{
			
		}

		public void OnDestroy()
		{
			
		}

	}
}

