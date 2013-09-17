using System;
using System.Collections.Generic;
using System.ServiceModel;
using Microsoft.Practices.Prism.Events;
using Microsoft.Practices.ServiceLocation;
using RequestForQuoteInterfacesLibrary.Constants;
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
        private readonly IEventAggregator eventAggregator = ServiceLocator.Current.GetInstance<IEventAggregator>();
        public List<IBook> Books { get; set; }

        public BookManagerImpl()
        {
            Books = new List<IBook>();
        }
        
        public void Initialize(bool isStandAlone)
        {
            try
            {
                if (isStandAlone)
                {
                    Books.Add(new BookImpl() { BookCode = "AB01", Entity = "AB01", IsValid = true });
                    Books.Add(new BookImpl() { BookCode = "AB02", Entity = "AB02", IsValid = true });
                    Books.Add(new BookImpl() { BookCode = "AB03", Entity = "AB02", IsValid = true });
                }
                else
                {
                    if (bookControllerProxy != null)
                    {
                        foreach (bookDetail book in bookControllerProxy.getAll())
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

        public bool AddBook(string bookCode, string entity, bool isValid, bool canSaveToDatabase)
        {
            var wasSavedToDatabase = false;
            var newBook = new BookImpl() { BookCode = bookCode, Entity = entity, IsValid = isValid };
            
            // Add to collection...
            Books.Add(newBook);

            // Add to database...
            if (canSaveToDatabase)
                wasSavedToDatabase = bookControllerProxy.save(bookCode, entity, RequestForQuoteConstants.MY_USER_NAME);

            // Publish event for other observer view models
            // TODO - does this not need to be inside the above if statement? Verify
            eventAggregator.GetEvent<NewBookEvent>().Publish(new NewBookEventPayload()
            {
                NewBook = newBook
            });

            // if no save is required then this should return true
            // otherwise if saved required the save through web service proxy must succeed.
            return !canSaveToDatabase || wasSavedToDatabase;
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
