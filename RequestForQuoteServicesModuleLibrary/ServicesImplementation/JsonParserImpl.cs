using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Json;
using System.Text;
using Microsoft.Practices.Prism.Events;
using Microsoft.Practices.ServiceLocation;
using RequestForQuoteInterfacesLibrary.Constants;
using RequestForQuoteInterfacesLibrary.EventPayloads;
using RequestForQuoteInterfacesLibrary.Events;
using RequestForQuoteInterfacesLibrary.ModelImplementations;
using RequestForQuoteInterfacesLibrary.ModelInterfaces;
using RequestForQuoteInterfacesLibrary.ServiceInterfaces;
using log4net;

namespace RequestForQuoteServicesModuleLibrary.ServicesImplementation
{
    public sealed class JsonParserImpl
    {
        private static readonly ILog log = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private static readonly IEventAggregator eventAggregator = ServiceLocator.Current.GetInstance<IEventAggregator>();
        private static readonly IBookManager bookManager = ServiceLocator.Current.GetInstance<IBookManager>();
        private static readonly IClientManager clientManager = ServiceLocator.Current.GetInstance<IClientManager>();
        private static readonly IUnderlyingManager underlyingManager = ServiceLocator.Current.GetInstance<IUnderlyingManager>();        
        private static Dictionary<string, Action<string>> actions;

        public JsonParserImpl()
        {
            eventAggregator.GetEvent<ServerUpdateEvent>()
                           .Subscribe(HandleServerUpdateEvent, ThreadOption.PublisherThread, RequestForQuoteConstants.MAINTAIN_STRONG_REFERENCE);

            InitializeActions();
        }

        private void InitializeActions()
        {
            actions = new Dictionary<string, Action<string>>
                {
                    {RequestForQuoteConstants.NEW_CHAT_MESSAGE, ProcessNewChatMessage},
                    {RequestForQuoteConstants.NEW_BOOK_UPDATE, ProcessNewBookUpdate},
                    {RequestForQuoteConstants.NEW_CLIENT_UPDATE, ProcessNewClientUpdate},
                    {RequestForQuoteConstants.NEW_UNDERLYIER_UPDATE, ProcessNewUnderlyierUpdate}
                };
        }

        private void ProcessNewChatMessage(string json)
        {
            try
            {
                var serializer = new DataContractJsonSerializer(typeof(ChatMessageImpl));
                eventAggregator.GetEvent<NewChatMessageEvent>().Publish(new NewChatMessageEventPayload()
                {
                    NewChatMessage = (ChatMessageImpl)serializer.ReadObject(new MemoryStream(Encoding.ASCII.GetBytes(json)))
                });  
            }
            catch (Exception exc)
            {                
                log.Error(String.Format("Failed to deserialize json [{0}] into new chat message. Exception raised [{1}]", json, exc.Message));
            }               
        }

        private void ProcessNewBookUpdate(string json)
        {
            try
            {
                var serializer = new DataContractJsonSerializer(typeof(BookImpl));
                IBook newBook = (BookImpl)serializer.ReadObject(new MemoryStream(Encoding.ASCII.GetBytes(json)));
                bookManager.AddBook(newBook.BookCode, newBook.Entity, newBook.IsValid, false);
            }
            catch (Exception exc)
            {
                log.Error(String.Format("Failed to deserialize json [{0}] into new book update. Exception raised [{1}]", json, exc.Message));
            }            
        }

        private void ProcessNewClientUpdate(string json)
        {
            try
            {
                var serializer = new DataContractJsonSerializer(typeof(ClientImpl));
                IClient newClient = (ClientImpl)serializer.ReadObject(new MemoryStream(Encoding.ASCII.GetBytes(json)));
                clientManager.AddClient(newClient.Name, newClient.Tier, newClient.IsValid, false);
            }
            catch (Exception exc)
            {
                log.Error(String.Format("Failed to deserialize json [{0}] into new client update. Exception raised [{1}]", json, exc.Message));
            }            
        }

        private void ProcessNewUnderlyierUpdate(string json)
        {
            try
            {
                var serializer = new DataContractJsonSerializer(typeof(UnderlyierImpl));
                IUnderlyier newUnderlyier = (UnderlyierImpl)serializer.ReadObject(new MemoryStream(Encoding.ASCII.GetBytes(json)));
                if(!underlyingManager.AddUnderlyier(newUnderlyier.RIC, newUnderlyier.BBG, newUnderlyier.Description, newUnderlyier.IsValid, false))
                    log.Error("Failed to add undelyier with RIC " + newUnderlyier.RIC + " from properly deserialized json.");
            }
            catch (Exception exc)
            {
                log.Error(String.Format("Failed to deserialize json [{0}] into new underlyier update. Exception raised [{1}]", json, exc.Message));
            }
        }

        private void HandleServerUpdateEvent(ServerUpdateEventPayload serverUpdate)
        {
            if(log.IsDebugEnabled)
                log.Debug(String.Format("JSON parser recieved server update: [{0}]", serverUpdate.Content));

            var index = serverUpdate.Content.IndexOf("=");
            var typeOfMessage = serverUpdate.Content.Substring(0, index);
            var json = serverUpdate.Content.Substring(index + 1);

            if (actions.ContainsKey(typeOfMessage))
                actions[typeOfMessage](json);
            else
                log.Error(String.Format("Unrecognized message type [{0}] recieved. Cannot action update.", typeOfMessage));

        }
    }
}
