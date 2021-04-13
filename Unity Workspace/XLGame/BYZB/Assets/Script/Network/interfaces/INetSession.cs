using System;

namespace AssemblyCSharp
{

	public interface OnNetworkListener
	{
		void setCollect( CollectNetInfo nInfo );

		int onRecvMessage( int type , byte[] dataRecv , int len );

		//void setState( bool isRunning );
	}

	public interface INetSession
	{

	    void setStateListener (OnNetworkListener nNetCallback);

		void connect( ConnectVars nVars );

		bool SendIsOK();

		void sendBytes( byte[] dataIn );

		void ToRcv();

		void close();

		//void setActive( bool bActive );

		bool isConnected();

		bool isConnecting();

	}
}

