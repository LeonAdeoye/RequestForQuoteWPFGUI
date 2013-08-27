using System.Collections.Generic;
using Microsoft.Practices.Prism.Events;
using Microsoft.Practices.ServiceLocation;
using RequestForQuoteInterfacesLibrary.EventPayloads;
using RequestForQuoteInterfacesLibrary.Events;
using RequestForQuoteInterfacesLibrary.ModelImplementations;
using RequestForQuoteInterfacesLibrary.ModelInterfaces;
using RequestForQuoteInterfacesLibrary.ServiceInterfaces;
using RequestForQuoteServicesModuleLibrary.ClientMaintenanceService;

namespace RequestForQuoteServicesModuleLibrary.ServicesImplementation
{
    public class ClientManagerImpl : IClientManager
    {
        public List<IClient> Clients { get; set; }
        private readonly ClientControllerClient clientControllerProxy = new ClientControllerClient();
        private readonly IEventAggregator eventAggregator = ServiceLocator.Current.GetInstance<IEventAggregator>();
        //TODO - calculate identifier
        private int identifier = 20;

        public ClientManagerImpl()
        {
            Clients = new List<IClient>();
        }

        public void Initialize()
        {
            // TO-DO: Do we need to add to GUI thread
            foreach (clientDetail client in clientControllerProxy.getAll())
            {
                Clients.Add(new ClientImpl() { Identifier = client.identifier, Name = client.name, Tier = client.tier, IsValid = client.isValid });
            }
        }

        public void AddClient(string name, int tier, bool isValid, bool canSaveToDatabase)
        {
            var newClient = new ClientImpl() {Identifier = ++identifier, Name = name, IsValid = isValid, Tier = tier};
            
            // Add to collection
            Clients.Add(newClient);

            // Save to database
            if (canSaveToDatabase)
                clientControllerProxy.save(name, tier);
    
            // Publish event for other observer view models
            eventAggregator.GetEvent<NewClientEvent>().Publish(new NewClientEventPayload()
            {
                NewClient = newClient
            });
        }

        public void RemoveClient(int identifier)
        {
            clientControllerProxy.delete(identifier);    
        }

        public void UpdateTier(int identifier, int tier)
        {
            clientControllerProxy.updateTier(identifier, tier);
        }

        public void UpdateValidity(int identifier, bool isValid)
        {
            clientControllerProxy.updateValidity(identifier, isValid);
        }
    }
}
