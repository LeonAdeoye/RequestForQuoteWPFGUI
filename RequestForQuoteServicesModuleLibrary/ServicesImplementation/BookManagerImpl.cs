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
using RequestForQuoteServicesModuleLibrary.BookMaintenanceService;
using log4net;

namespace RequestForQuoteServicesModuleLibrary.ServicesImplementation
{
    sealed class BookManagerImpl : IBookManager
    {
        private static readonly ILog log = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private readonly BookControllerClient bookControllerProxy = new BookControllerClient();
        private readonly IEventAggregator eventAggregator;
        private readonly IConfigurationManager configManager;
        public List<IBook> Books { get; set; }

        public BookManagerImpl(IConfigurationManager configManager, IEventAggregator eventAggregator)
        {
            if (configManager == null)
                throw new ArgumentNullException("configManager");

            if (eventAggregator == null)
                throw new ArgumentNullException("eventAggregator");

            this.configManager = configManager;
            this.eventAggregator = eventAggregator;

            Books = new List<IBook>();
        }
        
        public void Initialize()
        {
            try
            {
                if (configManager.IsStandAlone)
                {
                    Books.Add(new BookImpl() { BookCode = "AB01", Entity = "AB01", IsValid = true });
                    Books.Add(new BookImpl() { BookCode = "AB02", Entity = "AB02", IsValid = true });
                    Books.Add(new BookImpl() { BookCode = "AB03", Entity = "AB02", IsValid = true });
                }
                else
                {
                    if (bookControllerProxy != null)
                    {
                        var previouslySavedBooks = bookControllerProxy.getAll();
                        if (previouslySavedBooks == null)
                            return;

                        foreach (var book in previouslySavedBooks)
                        {
                            Books.Add(new BookImpl() { BookCode = book.bookCode, Entity = book.entity, IsValid = book.isValid });
                        }                        
                    }                    
                }
            }
            catch (EndpointNotFoundException exception)
            {
                log.Error(String.Format("Failed to connect to proxy for remote book controller webservice. Exception thrown {0}", exception));
                throw;
            }
            catch(TimeoutException timeoutException)
            {
                log.Error(String.Format("Timeout: failed to connect to proxy for remote book controller webservice. Exception thrown {0}", timeoutException));
                throw;
            }
        }

        public void AddBook(string bookCode, string entity, bool isValid)
        {
            if (String.IsNullOrEmpty(bookCode))
                throw new ArgumentException("bookCode");

            if (String.IsNullOrEmpty(entity))
                throw new ArgumentException("entity");

            var newBook = new BookImpl() { BookCode = bookCode, Entity = entity, IsValid = isValid };
            
            // Add to book maintenance manager's collection (master copy)...
            Books.Add(newBook);

            // Publish event for other observer view models
            eventAggregator.GetEvent<NewBookEvent>().Publish(new NewBookEventPayload()
            {
                NewBook = newBook
            });
        }

        public bool SaveToDatabase(string bookCode, string entity, bool isValid)
        {
            if (String.IsNullOrEmpty(bookCode))
                throw new ArgumentException("bookCode");

            if (String.IsNullOrEmpty(entity))
                throw new ArgumentException("entity");

            return bookControllerProxy.save(bookCode, entity, configManager.CurrentUser);
        }

        public bool UpdateValidity(string bookCode, bool isValid)
        {
            return bookControllerProxy.updateValidity(bookCode, isValid);
        }

        bool IBookManager.RemoveBook(string bookCode)
        {
            return bookControllerProxy.delete(bookCode);
        }
    }
}
