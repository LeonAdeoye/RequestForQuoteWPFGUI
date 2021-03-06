﻿using System;
using System.Collections.Generic;
using System.ServiceModel;
using Microsoft.Practices.Prism.Events;
using Microsoft.Practices.ServiceLocation;
using RequestForQuoteInterfacesLibrary.Constants;
using RequestForQuoteInterfacesLibrary.Enums;
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
        private readonly IEventAggregator eventAggregator;
        private readonly IConfigurationManager configManager;
        public List<IClient> Clients { get; set; }

        //TODO - calculate identifier
        private int startingIdentifier = 20;

        public ClientManagerImpl(IConfigurationManager configManager, IEventAggregator eventAggregator)
        {
            if (configManager == null)
                throw new ArgumentNullException("configManager");

            if (eventAggregator == null)
                throw new ArgumentNullException("eventAggregator");

            this.configManager = configManager;
            this.eventAggregator = eventAggregator;

            Clients = new List<IClient>();
        }

        public void Initialize()
        {
            if (configManager.IsStandAlone)
            {
                Clients.Add(new ClientImpl() { Identifier = 1, Name = "Nomura Securities", Tier = TierEnum.Top.ToString(), IsValid = true });
                Clients.Add(new ClientImpl() { Identifier = 1, Name = "Goldman Sachs", Tier = TierEnum.Top.ToString(), IsValid = true });
                Clients.Add(new ClientImpl() { Identifier = 1, Name = "JP Morgan", Tier = TierEnum.Top.ToString(), IsValid = true });
                Clients.Add(new ClientImpl() { Identifier = 1, Name = "Morgan Stanley", Tier = TierEnum.Top.ToString(), IsValid = true });
            }
            else
            {
                try
                {
                    if (clientControllerProxy != null)
                    {
                        var previouslySavedclients = clientControllerProxy.getAll();
                        if (previouslySavedclients == null)
                            return;

                        foreach (var client in previouslySavedclients)
                        {
                            if(client != null)
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

        public void AddClient(string name, string tier, bool isValid)
        {
            if (String.IsNullOrEmpty(name))
                throw new ArgumentException("name");

            if (String.IsNullOrEmpty(tier))
                throw new ArgumentException("tier");

            //TODO remove ++startingIdentifier
            var newClient = new ClientImpl() {Identifier = ++startingIdentifier, Name = name, IsValid = isValid, Tier = tier};
            
            // Add to collection
            Clients.Add(newClient);

            // Publish event for other observer view models
            eventAggregator.GetEvent<NewClientEvent>().Publish(new NewClientEventPayload()
            {
                NewClient = newClient
            });
        }

        public bool SaveToDatabase(string name, string tier)
        {
            if (String.IsNullOrEmpty(name))
                throw new ArgumentException("name");

            if (String.IsNullOrEmpty(tier))
                throw new ArgumentException("tier");

            return clientControllerProxy.save(name, tier, configManager.CurrentUser);    
        }

        public bool UpdateTier(int identifier, string tier)
        {
            return clientControllerProxy.updateTier(identifier, tier, configManager.CurrentUser);
        }

        public bool UpdateValidity(int identifier, bool isValid)
        {
            return clientControllerProxy.updateValidity(identifier, isValid, configManager.CurrentUser);
        }

        public IClient GetClientWithMatchingIdentifier(int clientId)
        {
            return Clients.Find(client => client.Identifier == clientId);
        }
    }
}
