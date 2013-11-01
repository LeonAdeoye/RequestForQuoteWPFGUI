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
using RequestForQuoteServicesModuleLibrary.UnderlyingMaintenanceService;
using log4net;

namespace RequestForQuoteServicesModuleLibrary.ServicesImplementation
{
    sealed class UnderlyingManagerImpl : IUnderlyingManager
    {
        private static readonly ILog log = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private readonly IEventAggregator eventAggregator = ServiceLocator.Current.GetInstance<IEventAggregator>();
        private readonly IConfigurationManager configManager = ServiceLocator.Current.GetInstance<IConfigurationManager>();
        private readonly UnderlyingControllerClient underlyingControllerProxy = new UnderlyingControllerClient();
        public List<IUnderlyier> Underlyings { get; set; }
        
        // TODO:identifier

        public UnderlyingManagerImpl()
        {
            Underlyings = new List<IUnderlyier>();
        }

        /// <summary>
        /// Initializes the underlyings collection.
        /// </summary>
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
                    foreach(var underlying in underlyingControllerProxy.getAll())
                    {
                        Underlyings.Add(new UnderlyierImpl()
                            {
                                RIC = underlying.ric,
                                Description = underlying.description,
                                IsValid = underlying.isValid
                            });    
                    }
                }
                catch (EndpointNotFoundException exception)
                {
                    log.Error(String.Format("Failed to connect to proxy for remote search controller webservice. Exception thrown {0}", exception));
                    throw;
                }                
            }            
        }

        /// <summary>
        /// Updates the validity of the underlying.
        /// </summary>
        /// <param name="ric"> the ric of the underlying that will be changed.</param>
        /// <param name="isValid"> the validity of the underlying.</param>
        /// <returns> true if the update was successful; false otherwise.</returns>
        /// <exception cref="ArgumentException"> thrown if the ric is null or empty</exception>
        public bool UpdateValidity(string ric, bool isValid)
        {
            if (String.IsNullOrEmpty(ric))
                throw new ArgumentException("ric");

            return underlyingControllerProxy.updateValidity(ric, isValid, configManager.GetCurrentUser());
        }

        /// <summary>
        /// Adds the underlying to the collection of underlyings maintained by the underlying manager.
        /// Publishes details of this underlying to all listeners.
        /// </summary>
        /// <param name="ric"> the ric of the underlying that will be added.</param>
        /// <param name="description"> the description of the underlying that will be added.</param>
        /// <param name="isValid"> the validity of the underlying that will be added.</param>
        /// <exception cref="ArgumentException"> thrown if the ric/description is null or empty</exception
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

        /// <summary>
        /// Saves the underlying to the database.
        /// </summary>
        /// <param name="ric"> the ric of the underlying that will be saved.</param>
        /// <param name="description"> the description of the underlying that will be saved.</param>
        /// <returns> true if the save was successful; false otherwise.</returns>
        /// <exception cref="ArgumentException"> thrown if the ric/description is null or empty</exception>
        public bool SaveToDatabase(string ric, string description)
        {
            if (String.IsNullOrEmpty(ric))
                throw new ArgumentException("ric");

            if (String.IsNullOrEmpty(description))
                throw new ArgumentException("description");

            return underlyingControllerProxy.save(ric, description, configManager.GetCurrentUser());
        }
    }
}
