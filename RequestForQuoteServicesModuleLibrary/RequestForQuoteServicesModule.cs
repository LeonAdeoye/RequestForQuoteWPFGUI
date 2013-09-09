using System;
using System.ServiceModel;
using System.Threading;
using Microsoft.Practices.Prism.Modularity;
using RequestForQuoteInterfacesLibrary.Constants;
using RequestForQuoteInterfacesLibrary.ServiceInterfaces;
using RequestForQuoteServicesModuleLibrary.ServicesImplementation;
using log4net;
using Microsoft.Practices.Unity;

namespace RequestForQuoteServicesModuleLibrary
{
    class RequestForQuoteServicesModule : IModule
    {
        private static readonly ILog log = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        public RequestForQuoteServicesModule(IUnityContainer container)
        {
            this.container = container;

            if (log.IsDebugEnabled)
                log.Debug("RequestForQuoteServicesModule constructed successfully.");
        }

        public void Initialize()
        {
            var underlyingManager = new UnderlyingManagerImpl();
            underlyingManager.Initialize();
            container.RegisterInstance<IUnderlyingManager>(underlyingManager);

            var clientManager = new ClientManagerImpl();
            clientManager.Initialize();
            container.RegisterInstance<IClientManager>(clientManager);

            var bookManager = new BookManagerImpl();
            bookManager.Initialize();
            container.RegisterInstance<IBookManager>(bookManager);

            var bankHolidayManager = new BankHolidayManagerImpl();
            bankHolidayManager.Initialize();
            container.RegisterInstance<IBankHolidayManager>(bankHolidayManager);

            container.RegisterType<IOptionRequestParser, OptionRequestParserImpl>(new ContainerControlledLifetimeManager());
            container.RegisterType<IOptionRequestPricer, OptionRequestPricerImpl>(new ContainerControlledLifetimeManager());
            container.RegisterType<ISearchManager, SearchManagerImpl>(new ContainerControlledLifetimeManager());
            container.RegisterType<IChatServiceManager, ChatServiceManagerImpl>(new ContainerControlledLifetimeManager());
            container.RegisterInstance(new JsonParserImpl());

            InitializeServerCommunicator();

            if (log.IsDebugEnabled)
                log.Debug("RequestForQuoteServicesModule initialized successfully.");
        }

        private void InitializeServerCommunicator()
        {
            ServerCommunicatorImpl serverCommunicator = new ServerCommunicatorImpl(RequestForQuoteConstants.SERVER_IP_ADDRESS, 
                RequestForQuoteConstants.SERVER_PORT_NUMBER, RequestForQuoteConstants.SERVER_SLEEP_INTERVAL);

            serverCommunicator.ConnectToServer();

            if (serverCommunicator.IsConnected())
            {
                container.RegisterInstance<IServerCommunicator>(serverCommunicator);

                Thread thread = new Thread(serverCommunicator.ListenForUpdatesContinuously);
                thread.Start();

                if (log.IsDebugEnabled)
                    log.Debug(string.Format("ServerCommunicatorImpl is now running on a separate thread, and is listening for messages from the server."));
            }
            else
                log.Error("Could not initialize and start socket communication with back-end service. This client is not connected.");
        }

        private readonly IUnityContainer container;
    }
}
