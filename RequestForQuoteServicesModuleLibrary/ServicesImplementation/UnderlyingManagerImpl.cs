using System;
using System.Collections.Generic;
using System.ServiceModel;
using Microsoft.Practices.Prism.Events;
using Microsoft.Practices.ServiceLocation;
using RequestForQuoteInterfacesLibrary.EventPayloads;
using RequestForQuoteInterfacesLibrary.Events;
using RequestForQuoteInterfacesLibrary.ModelImplementations;
using RequestForQuoteInterfacesLibrary.ModelInterfaces;
using RequestForQuoteInterfacesLibrary.ServiceInterfaces;
using log4net;

namespace RequestForQuoteServicesModuleLibrary.ServicesImplementation
{
    sealed class UnderlyingManagerImpl : IUnderlyingManager
    {
        private static readonly ILog log = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private readonly IEventAggregator eventAggregator = ServiceLocator.Current.GetInstance<IEventAggregator>();
        private readonly IConfigurationManager configManager = ServiceLocator.Current.GetInstance<IConfigurationManager>();
        public List<IUnderlyier> Underlyiers { get; set; }
        
        // TODO:identifier

        public UnderlyingManagerImpl()
        {
            Underlyiers = new List<IUnderlyier>();
        }

        public void Initialize()
        {
            // TODO remove !
            if (!configManager.IsStandAlone())
            {
                Underlyiers.Add(new UnderlyierImpl() { Description = "HSBC Ltd", RIC = "0005.HK" });
                Underlyiers.Add(new UnderlyierImpl() { Description = "Bank Of China", RIC = "0001.HK" });
                Underlyiers.Add(new UnderlyierImpl() { Description = "Nomura High Yield ETF", RIC = "1577.OS" });                
            }
            else
            {
                try
                {
                }
                catch (EndpointNotFoundException exception)
                {
                    log.Error(String.Format("Failed to connect to proxy for remote search controller webservice. Exception thrown {0}", exception));
                    throw;
                }                
            }            
        }

        public bool AddUnderlyier(string RIC, string description, bool isValid, bool saveToDatabase)
        {
            if (String.IsNullOrEmpty(RIC))
                throw new ArgumentException("RIC");

            if (String.IsNullOrEmpty(description))
                throw new ArgumentException("description");

            //var wasSavedToDatabse = false;
            var newUnderlyier = new UnderlyierImpl() {Description = "Nomura Securities", RIC = "5678.T"};

            // Add to collection
            Underlyiers.Add(newUnderlyier);

            // TODO
            // if (saveToDatabase)
                // Save to database

            // Publish event for other observer view models
            eventAggregator.GetEvent<NewUnderlyierEvent>().Publish(new NewUnderlyierEventPayload()
            {
                NewUnderlyier = newUnderlyier
            });

            // TODO return !saveToDatabase || wasSavedToDatabase
            return true;
        }

        public bool RemoveUnderlyier(string RIC)
        {
            return true;
        }
    }
}
