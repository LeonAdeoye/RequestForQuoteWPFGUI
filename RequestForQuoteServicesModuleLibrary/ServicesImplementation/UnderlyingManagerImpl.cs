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
        public List<IUnderlyier> Underlyings { get; set; }
        
        // TODO:identifier

        public UnderlyingManagerImpl()
        {
            Underlyings = new List<IUnderlyier>();
        }

        public void Initialize()
        {
            // TODO remove !
            if (!configManager.IsStandAlone())
            {
                Underlyings.Add(new UnderlyierImpl() { Description = "HSBC Ltd", RIC = "0005.HK" });
                Underlyings.Add(new UnderlyierImpl() { Description = "Bank Of China", RIC = "0001.HK" });
                Underlyings.Add(new UnderlyierImpl() { Description = "Nomura High Yield ETF", RIC = "1577.OS" });                
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

        public bool UpdateValidity(string ric, bool isValid)
        {
            if (String.IsNullOrEmpty(ric))
                throw new ArgumentException("ric");

            return true;
        }

        public void AddUnderlying(string ric, string description, bool isValid)
        {
            if (String.IsNullOrEmpty(ric))
                throw new ArgumentException("ric");

            if (String.IsNullOrEmpty(description))
                throw new ArgumentException("description");

            var newUnderlyier = new UnderlyierImpl() {Description = description, RIC = ric, IsValid = isValid};

            Underlyings.Add(newUnderlyier);

            eventAggregator.GetEvent<NewUnderlyierEvent>().Publish(new NewUnderlyierEventPayload()
            {
                NewUnderlyier = newUnderlyier
            });
        }

        public bool SaveToDatabase(string ric, string description)
        {
            if (String.IsNullOrEmpty(ric))
                throw new ArgumentException("ric");

            if (String.IsNullOrEmpty(description))
                throw new ArgumentException("description");

            return true;
        }
    }
}
