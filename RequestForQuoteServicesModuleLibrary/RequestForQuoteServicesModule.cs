using System;
using System.Threading;
using System.Threading.Tasks;
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
            var isStandAlone = (Environment.GetCommandLineArgs().Length > 1
                && Environment.GetCommandLineArgs()[1] == RequestForQuoteConstants.STANDALONE_MODE_WITHOUT_WEB_SERVICE);
            
            var tasks = new Task[5];

            var underlyingManager = new UnderlyingManagerImpl();
            container.RegisterInstance<IUnderlyingManager>(underlyingManager);
            tasks[0] = Task.Factory.StartNew(()=>underlyingManager.Initialize(true));

            var clientManager = new ClientManagerImpl();
            container.RegisterInstance<IClientManager>(clientManager);
            tasks[1] = Task.Factory.StartNew(() => clientManager.Initialize(isStandAlone));

            var bookManager = new BookManagerImpl();
            container.RegisterInstance<IBookManager>(bookManager);
            tasks[2] = Task.Factory.StartNew(() => bookManager.Initialize(isStandAlone));

            var bankHolidayManager = new BankHolidayManagerImpl();
            container.RegisterInstance<IBankHolidayManager>(bankHolidayManager);
            tasks[3] = Task.Factory.StartNew(() => bankHolidayManager.Initialize(isStandAlone));

            var searchManager = new SearchManagerImpl();
            container.RegisterInstance<ISearchManager>(searchManager);
            tasks[4] = Task.Factory.StartNew(() => searchManager.Initialize(isStandAlone));

            var optionRequestPersistanceManager = new OptionRequestPersistanceManagerImpl(clientManager);
            container.RegisterInstance<IOptionRequestPersistanceManager>(optionRequestPersistanceManager);

            container.RegisterType<IOptionRequestParser, OptionRequestParserImpl>(new ContainerControlledLifetimeManager())
                .RegisterType<IOptionRequestPricer, OptionRequestPricerImpl>(new ContainerControlledLifetimeManager())
                .RegisterType<IChatServiceManager, ChatServiceManagerImpl>(new ContainerControlledLifetimeManager())
                .RegisterInstance(new JsonParserImpl());

            InitializeServerCommunicator(isStandAlone);

            // Exceptions thrown by tasks will be propagated to the main thread 
            // while it waits for the tasks. The actual exceptions will be wrapped in AggregateException. 
            try
            {
                // Wait for all the tasks to finish.
                Task.WaitAll(tasks);
                
                if(log.IsDebugEnabled)
                    log.Debug("Successfully completed initialization of all service implementations");
            }
            catch (AggregateException e)
            {
                foreach (var exception in e.InnerExceptions)
                {
                    log.Error("Catastrophic failure! Exception thrown: " + exception);
                    throw new ModuleInitializeException();
                }
            }
        }

        private void InitializeServerCommunicator(bool isStandAlone)
        {
            var serverCommunicator = new ServerCommunicatorImpl(RequestForQuoteConstants.SERVER_IP_ADDRESS, 
                RequestForQuoteConstants.SERVER_PORT_NUMBER, RequestForQuoteConstants.SERVER_SLEEP_INTERVAL);

            container.RegisterInstance<IServerCommunicator>(serverCommunicator);

            if (isStandAlone)
                return;
                
            serverCommunicator.ConnectToServer();

            if (serverCommunicator.IsConnected())
            {                
                var thread = new Thread(serverCommunicator.ListenForUpdatesContinuously);
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
