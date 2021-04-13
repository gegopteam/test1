using System;

namespace AssemblyCSharp
{
	public class SessionFactory
	{

		public const int PROTO_TCP = 1;

		public SessionFactory ()
		{
		}

		public static INetSession createSession( int nValueType ){
			if (nValueType == 1) {
				return new FiSocketSession ();
			}
			return null;
		}

	}
}

