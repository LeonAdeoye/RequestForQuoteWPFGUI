using System;

namespace RequestForQuoteInterfacesLibrary.ServiceInterfaces
{
    public interface IServerCommunicator
    {
        void ConnectToServer();
        void DisconnectFromServer();
        void ListenForUpdatesContinuously();
        bool IsConnected();
    }
}