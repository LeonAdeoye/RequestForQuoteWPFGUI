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
        private static readonly IBankHolidayManager bankHolidayManager = ServiceLocator.Current.GetInstance<IBankHolidayManager>();
        private static readonly ISearchManager searchManager = ServiceLocator.Current.GetInstance<ISearchManager>();        
        private static Dictionary<string, Action<string>> actions;

        private const string NEW_CHAT_MESSAGE = "NewChatMessage";
        private const string NEW_BOOK_UPDATE = "NewBookUpdate";
        private const string NEW_CLIENT_UPDATE = "NewClientUpdate";
        private const string NEW_UNDERLYIER_UPDATE = "NewUnderlyierUpdate";
        private const string NEW_REQUEST_UPDATE = "NewRequestUpdate";
        private const string NEW_HOLIDAY_UPDATE = "NewHolidayUpdate";
        private const string NEW_CRITERION_UPDATE = "NewCriterionUpdate";
        private readonly Object lockObject = new Object();


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
                    {NEW_CHAT_MESSAGE, ProcessNewChatMessage},
                    {NEW_BOOK_UPDATE, ProcessNewBookUpdate},
                    {NEW_CLIENT_UPDATE, ProcessNewClientUpdate},
                    {NEW_UNDERLYIER_UPDATE, ProcessNewUnderlyierUpdate},
                    {NEW_HOLIDAY_UPDATE, ProcessNewHolidayUpdate},
                    {NEW_REQUEST_UPDATE, ProcessNewRequestUpdate},
                    {NEW_CRITERION_UPDATE, ProcessNewCriterionUpdate}
                };
        }

        private void ProcessNewCriterionUpdate(string json)
        {
            try
            {
                var serializer = new DataContractJsonSerializer(typeof(SearchCriterionImpl));
                ISearchCriterion newCriterion = (SearchCriterionImpl) serializer.ReadObject(new MemoryStream(Encoding.ASCII.GetBytes(json)));
                searchManager.AddSearch(newCriterion.Owner, newCriterion.DescriptionKey, newCriterion.IsPrivate, newCriterion.IsFilter, 
                    newCriterion.ControlName, newCriterion.ControlValue);
            }
            catch (Exception exc)
            {
                log.Error(String.Format("Failed to deserialize json [{0}] into new search criterion update. Exception raised [{1}]", json, exc.Message));
            } 
        }

        private void ProcessNewRequestUpdate(string json)
        {
            try
            {
                var serializer = new DataContractJsonSerializer(typeof(RequestForQuoteImpl));
                IRequestForQuote newRequest = (RequestForQuoteImpl)serializer.ReadObject(new MemoryStream(Encoding.ASCII.GetBytes(json)));
            }
            catch (Exception exc)
            {
                log.Error(String.Format("Failed to deserialize json [{0}] into new request update. Exception raised [{1}]", json, exc.Message));
            } 
        }

        private void ProcessNewHolidayUpdate(string json)
        {
            try
            {
                var serializer = new DataContractJsonSerializer(typeof(BankHolidayImpl));
                IBankHoliday newHoliday = (BankHolidayImpl)serializer.ReadObject(new MemoryStream(Encoding.ASCII.GetBytes(json)));
                bankHolidayManager.AddHoliday(newHoliday.HolidayDate, newHoliday.Location);
            }
            catch (Exception exc)
            {
                log.Error(String.Format("Failed to deserialize json [{0}] into new holiday update. Exception raised [{1}]", json, exc.Message));
            } 
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
                bookManager.AddBook(newBook.BookCode, newBook.Entity, newBook.IsValid);
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
                clientManager.AddClient(newClient.Name, newClient.Tier, newClient.IsValid);
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
                if (!underlyingManager.AddUnderlyier(newUnderlyier.RIC, newUnderlyier.BBG, newUnderlyier.Description, newUnderlyier.IsValid, RequestForQuoteConstants.DO_NOT_SAVE_TO_DATABASE))
                {
                    log.Error("Failed to add underlying with RIC " + newUnderlyier.RIC + " from properly deserialized json.");
                }                    
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

            if (index >= 0)
            {
                var typeOfMessage = serverUpdate.Content.Substring(0, index);
                var json = serverUpdate.Content.Substring(index + 1);

                if (actions.ContainsKey(typeOfMessage))
                {
                    lock (lockObject)
                    {
                        actions[typeOfMessage](json);    
                    }                    
                }                    
                else
                    log.Error(String.Format("Unrecognized message type [{0}] recieved. Cannot action update.", typeOfMessage));                
            }
            else
                throw new InvalidDataException("Missing message type prefix in : " + serverUpdate.Content);
        }
    }
}
