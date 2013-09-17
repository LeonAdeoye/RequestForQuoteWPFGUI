using System;
using System.Net;
using System.Net.Sockets;
using System.Threading;
using System.Text;
using Microsoft.Practices.Prism.Events;
using Microsoft.Practices.ServiceLocation;
using RequestForQuoteInterfacesLibrary.EventPayloads;
using RequestForQuoteInterfacesLibrary.Events;
using RequestForQuoteInterfacesLibrary.ServiceInterfaces;
using log4net;

namespace RequestForQuoteServicesModuleLibrary.ServicesImplementation
{
    public sealed class ServerCommunicatorImpl : IServerCommunicator
    {
        // State object for receiving data from remote device.
        private class StateObject
        {
            // Client socket.
            public Socket workSocket = null;
            // Size of receive buffer.
            public const int BufferSize = 1024;
            // Receive buffer.
            public byte[] buffer = new byte[BufferSize];
            // Received data string.
            public StringBuilder sb = new StringBuilder();
        }

        private static readonly ILog log = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private readonly IEventAggregator eventAggregator = ServiceLocator.Current.GetInstance<IEventAggregator>();
        private readonly int remotePort;
        private readonly string remoteAddress;
        private readonly int sleepIntervalInMilliseconds;
        private readonly Socket socketConnection = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
        private readonly Object lockObject = new object();
        private readonly static ManualResetEvent receiveDone = new ManualResetEvent(false);

        public ServerCommunicatorImpl(string remoteAddress, int remotePort, int sleepIntervalInMilliseconds)
        {
            this.remoteAddress = remoteAddress;
            this.remotePort = remotePort;
            this.sleepIntervalInMilliseconds = sleepIntervalInMilliseconds;
        }

        public void ConnectToServer()
        {
            if(!socketConnection.Connected)
            {
                try
                {
                    var ipAddress = IPAddress.Parse(remoteAddress);
                    var endPoint = new IPEndPoint(ipAddress, remotePort);
                    socketConnection.Connect(endPoint);
                    socketConnection.SetSocketOption(SocketOptionLevel.Socket, SocketOptionName.KeepAlive, true);

                    if(log.IsDebugEnabled)
                        log.Debug(String.Format("Connected socket to server with hostname: {0} and port number: {1}.", remoteAddress, remotePort));
                }
                catch (Exception e)
                {
                    log.Error(String.Format("Failed to connect socket to server with hostname: {0} and port number: {1}. Exception raised: {2}", remoteAddress, remotePort, e.Message));
                }
            }
        }

        public bool IsConnected()
        {
            return socketConnection.Connected;
        }

        public void DisconnectFromServer()
        {
            try
            {
                socketConnection.Shutdown(SocketShutdown.Both);
            }
            catch (Exception e)
            {
                log.Error(string.Format("Exception raised when shutting down in/out traffic through the socket: {0}", e.Message));
            }
            finally
            {
                if(socketConnection.Connected)
                    socketConnection.Close();

                receiveDone.Set();
            }
        }

        public void ListenForUpdatesContinuously()
        {
            try
            {
                while (socketConnection.Connected)
                {
                    Receive();
                    receiveDone.WaitOne();
                    Thread.Sleep(sleepIntervalInMilliseconds);
                }
            }
            catch (Exception e)
            {
                log.Error(String.Format("Exception raised: {0}", e.Message));
            }
            finally
            {
                if(socketConnection.Connected)
                    DisconnectFromServer();
            }
        }

        private void Receive()
        {
            try
            {
                // Create the state object.
                var state = new StateObject {workSocket = socketConnection};
                // Begin receiving the data from the remote device.
                socketConnection.BeginReceive(state.buffer, 0, StateObject.BufferSize, 0, new AsyncCallback(ReceiveCallback), state);
            }
            catch (Exception e)
            {
                log.Error(String.Format("Exception raised before reading data: {0}", e.Message));
            }
        }

        private  void ReceiveCallback(IAsyncResult ar)
        {
            var state = (StateObject)ar.AsyncState;
            var clientSocket = state.workSocket;
            try
            {
                if (!clientSocket.Connected)
                {
                    receiveDone.Set();
                    return;
                }

                var bytesRead = clientSocket.EndReceive(ar);

                if (bytesRead > 0)
                {                    
                    state.sb.Append(Encoding.ASCII.GetString(state.buffer, 0, bytesRead));
                    ProcessMessage(state);
                    clientSocket.BeginReceive(state.buffer, 0, StateObject.BufferSize, SocketFlags.None, new AsyncCallback(ReceiveCallback), state);
                }
            }
            catch (Exception e)
            {
                log.Error(String.Format("Failed to read and process new data [{0}] sent over socket connection. Exception raised [{1}]", state.sb, e.Message));
            }
        }

        private void ProcessMessage(StateObject state)
        {
            int sizeOfMessage;
            if (Int32.TryParse(state.sb.ToString().Substring(0, 3), out sizeOfMessage))
            {
                if (state.sb.Length > sizeOfMessage + 3)
                {
                    PublishMessageToJSONParser(state.sb.ToString().Substring(3, sizeOfMessage));
                    state.sb.Remove(0, sizeOfMessage + 3);
                }
                else if (state.sb.Length == sizeOfMessage + 3)
                {
                    PublishMessageToJSONParser(state.sb.ToString().Substring(3, sizeOfMessage));
                    state.sb.Remove(0, sizeOfMessage + 3);
                    receiveDone.Set();
                }
            }
            else
            {
                log.Error(String.Format("Size prefix of new message [{0}] is missing", state.sb));
                throw new ArgumentException(String.Format("Size prefix is missing in new message [{0}]", state.sb));    
            }
        }

        private void PublishMessageToJSONParser(string message)
        {
            eventAggregator.GetEvent<ServerUpdateEvent>().Publish(new ServerUpdateEventPayload()
            {
                Content = message
            });
        }
    }
}
