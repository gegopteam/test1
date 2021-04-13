using System;

namespace AssemblyCSharp
{
	public class ConnectVars
	{
		public const int PROTOCOL_TCP = 0;

		public const int PROTOCOL_HTTP = 1;

		public int protocol = 0;

		public ConnectVars ()
		{
			protocol = PROTOCOL_TCP;
		}
			
		public string serverAddress;

		public int serverProt;
	}
}

