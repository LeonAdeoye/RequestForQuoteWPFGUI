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
    class UnderlyingManagerImpl : IUnderlyingManager
    {
        private static readonly ILog log = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private readonly IEventAggregator eventAggregator = ServiceLocator.Current.GetInstance<IEventAggregator>();
        public List<IUnderlyier> Underlyiers { get; set; }
        
        // TODO:identifier

        public UnderlyingManagerImpl()
        {
            Underlyiers = new List<IUnderlyier>();
        }        

        public void Initialize()
        {
            try
            {
                Underlyiers.Add(new UnderlyierImpl() { BBG = "HK0005", Description = "HSBC Ltd", RIC = "0005.HK" });
                Underlyiers.Add(new UnderlyierImpl() { BBG = "HK0001", Description = "Bank Of China", RIC = "0001.HK" });
                Underlyiers.Add(new UnderlyierImpl() { BBG = "JP 1577", Description = "Nomura High Yield ETF", RIC = "1577.OS" });
            }
            catch (EndpointNotFoundException exception)
            {
                log.Error(String.Format("Failed to connect to proxy for remote search controller webservice. Exception thrown {0}", exception));
                throw;
            }            
        }

        public void AddUnderlyier(string RIC, string BBG, string description, bool isValid, bool saveToDatabase)
        {
            var newUnderlyier = new UnderlyierImpl() {BBG = "JP 5678", Description = "Nomura Securities", RIC = "5678.T"};

            // Add to collection
            Underlyiers.Add(newUnderlyier);

            
            //if (saveToDatabase)
                // Save to database

            // Publish event for other observer view models
            eventAggregator.GetEvent<NewUnderlyierEvent>().Publish(new NewUnderlyierEventPayload()
            {
                NewUnderlyier = newUnderlyier
            });
        }

        public void RemoveUnderlyier()
        {

        }
    }
}
