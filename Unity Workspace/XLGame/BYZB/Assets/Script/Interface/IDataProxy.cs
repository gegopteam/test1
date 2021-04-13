using System;

namespace AssemblyCSharp
{
	public interface IDataProxy
	{
		void OnAddData( int nType , object nData );

		void OnInit();

		void OnDestroy();
	}
}

