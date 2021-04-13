using System;

namespace AssemblyCSharp
{
	public interface IUiMediator
	{
		void OnRecvData( int nType , object nData );

		void OnInit();

		void OnRelease();
	}
}

