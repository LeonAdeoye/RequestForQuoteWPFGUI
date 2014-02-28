using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Practices.Prism.Events;
using Microsoft.Practices.Prism.Modularity;
using RequestForQuoteInterfacesLibrary.Constants;
using RequestForQuoteInterfacesLibrary.ServiceInterfaces;
using RequestForQuoteServicesModuleLibrary.BookMaintenanceService;
using RequestForQuoteServicesModuleLibrary.GroupMaintenanceService;
using RequestForQuoteServicesModuleLibrary.ServicesImplementation;
using RequestForQuoteServicesModuleLibrary.UserMaintenanceService;
using log4net;
using Microsoft.Practices.Unity;

namespace RequestForQuoteServicesModuleLibrary
{
    class RequestForQuoteServicesModule : IModule
    {
        private static readonly ILog log = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private readonly IUnityContainer container;
        private readonly IEventAggregator eventAggregator;

        public RequestForQuoteServicesModule(IUnityContainer container, IEventAggregator eventAggregator)
        {
            if (container == null)
                throw new ArgumentNullException("container");

            if (eventAggregator == null)
                throw new ArgumentNullException("eventAggregator");

            this.container = container;
            this.eventAggregator = eventAggregator;

            if (log.IsDebugEnabled)
                log.Debug("RequestForQuoteServicesModule constructed successfully.");
        }

        public void Initialize()
        {            
            var configManager = new ConfigurationManagerImpl();
            container.RegisterInstance<IConfigurationManager>(configManager);
            configManager.Initialize();
            
            var tasks = new Task[7];

            var underlyingManager = new UnderlyingManagerImpl(configManager, eventAggregator);
            container.RegisterInstance<IUnderlyingManager>(underlyingManager);
            tasks[0] = Task.Factory.StartNew(() => underlyingManager.Initialize());

            var clientManager = new ClientManagerImpl(configManager, eventAggregator);
            container.RegisterInstance<IClientManager>(clientManager);
            tasks[1] = Task.Factory.StartNew(() => clientManager.Initialize());

            var bookManager = new BookManagerImpl(configManager, eventAggregator, new BookControllerClient());
            container.RegisterInstance<IBookManager>(bookManager);
            tasks[2] = Task.Factory.StartNew(() => bookManager.Initialize());

            var bankHolidayManager = new BankHolidayManagerImpl(configManager, eventAggregator);
            container.RegisterInstance<IBankHolidayManager>(bankHolidayManager);
            tasks[3] = Task.Factory.StartNew(() => bankHolidayManager.Initialize());

            var searchManager = new SearchManagerImpl(configManager, eventAggregator);
            container.RegisterInstance<ISearchManager>(searchManager);
            tasks[4] = Task.Factory.StartNew(() => searchManager.Initialize());

            var userManager = new UserManagerImpl(configManager, eventAggregator, new UserControllerClient());
            container.RegisterInstance<IUserManager>(userManager);
            tasks[5] = Task.Factory.StartNew(() => userManager.Initialize());

            var groupManager = new GroupManagerImpl(configManager, eventAggregator, new GroupControllerClient());
            container.RegisterInstance<IGroupManager>(groupManager);
            tasks[6] = Task.Factory.StartNew(() => groupManager.Initialize());
            
            var optionRequestPersistanceManager = new OptionRequestPersistanceManagerImpl(clientManager, configManager);
            container.RegisterInstance<IOptionRequestPersistanceManager>(optionRequestPersistanceManager);

            container.RegisterType<IOptionRequestParser, OptionRequestParserImpl>(new ContainerControlledLifetimeManager())
                .RegisterType<IOptionRequestPricer, OptionRequestPricerImpl>(new ContainerControlledLifetimeManager())
                .RegisterType<IChatServiceManager, ChatServiceManagerImpl>(new ContainerControlledLifetimeManager())
                .RegisterType<IReportDataManager, ReportDataManagerImpl>(new ContainerControlledLifetimeManager())
                .RegisterInstance(new JsonParserImpl());

            InitializeServerCommunicator(configManager.IsStandAlone);

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
    }
}
