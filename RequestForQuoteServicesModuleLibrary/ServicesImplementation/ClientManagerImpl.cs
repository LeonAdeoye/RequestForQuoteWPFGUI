using System;
using System.Collections.Generic;
using System.ServiceModel;
using System.Windows;
using Microsoft.Practices.Prism.Events;
using Microsoft.Practices.ServiceLocation;
using RequestForQuoteInterfacesLibrary.EventPayloads;
using RequestForQuoteInterfacesLibrary.Events;
using RequestForQuoteInterfacesLibrary.ModelImplementations;
using RequestForQuoteInterfacesLibrary.ModelInterfaces;
using RequestForQuoteInterfacesLibrary.ServiceInterfaces;
using RequestForQuoteServicesModuleLibrary.ClientMaintenanceService;
using log4net;

namespace RequestForQuoteServicesModuleLibrary.ServicesImplementation
{
    public sealed class ClientManagerImpl : IClientManager
    {
        private static readonly ILog log = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);        
        private readonly ClientControllerClient clientControllerProxy = new ClientControllerClient();
        private readonly IEventAggregator eventAggregator = ServiceLocator.Current.GetInstance<IEventAggregator>();
        public List<IClient> Clients { get; set; }

        //TODO - calculate identifier
        private int startingIdentifier = 20;

        public ClientManagerImpl()
        {
            Clients = new List<IClient>();
        }

        public void Initialize(bool isStandAlone)
        {
            if (isStandAlone)
            {
                Clients.Add(new ClientImpl() { Identifier = 1, Name = "Nomura Securities", Tier = 1, IsValid = true });
                Clients.Add(new ClientImpl() { Identifier = 1, Name = "Goldman Sachs", Tier = 1, IsValid = true });
                Clients.Add(new ClientImpl() { Identifier = 1, Name = "JP Morgan", Tier = 1, IsValid = true });
                Clients.Add(new ClientImpl() { Identifier = 1, Name = "Morgan Stanley", Tier = 1, IsValid = true });
            }
            else
            {
                try
                {
                    if (clientControllerProxy != null)
                    {
                        foreach (var client in clientControllerProxy.getAll())
                        {
                            Clients.Add(new ClientImpl() { Identifier = client.identifier, Name = client.name, Tier = client.tier, IsValid = client.isValid });
                        }                        
                    }
                }
                catch (EndpointNotFoundException exception)
                {
                    log.Error(String.Format("Failed to connect to proxy for remote client controller webservice. Exception thrown {0}", exception));
                    throw;
                }
                catch (TimeoutException timeoutException)
                {
                    log.Error(String.Format("Timeout: failed to connect to proxy for remote client controller webservice. Exception thrown {0}", timeoutException));
                    throw;
                }                
            }
        }

        public bool AddClient(string name, int tier, bool isValid, bool canSaveToDatabase)
        {
            var wasSavedToDatabase = false;
            var newClient = new ClientImpl() {Identifier = ++startingIdentifier, Name = name, IsValid = isValid, Tier = tier};
            
            // Add to collection
            Clients.Add(newClient);

            // Save to database
            if (canSaveToDatabase)
                wasSavedToDatabase = clientControllerProxy.save(name, tier);

            // TODO verify that this needs to be called even if canSaveToDatabase = false;
            // Publish event for other observer view models
            eventAggregator.GetEvent<NewClientEvent>().Publish(new NewClientEventPayload()
            {
                NewClient = newClient
            });

            // if no save is required then this should return true
            // otherwise if saved required the save through web service proxy must succeed.
            return !canSaveToDatabase || wasSavedToDatabase;
        }

        public bool RemoveClient(int identifier)
        {
            return clientControllerProxy.delete(identifier);
        }

        public bool UpdateTier(int identifier, int tier)
        {
            return clientControllerProxy.updateTier(identifier, tier);
        }

        public bool UpdateValidity(int identifier, bool isValid)
        {
            return clientControllerProxy.updateValidity(identifier, isValid);
        }
    }
}
