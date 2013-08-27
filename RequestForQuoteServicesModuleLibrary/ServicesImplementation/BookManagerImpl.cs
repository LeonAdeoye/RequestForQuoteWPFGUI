using System.Collections.Generic;
using Microsoft.Practices.Prism.Events;
using Microsoft.Practices.ServiceLocation;
using RequestForQuoteInterfacesLibrary.Constants;
using RequestForQuoteInterfacesLibrary.EventPayloads;
using RequestForQuoteInterfacesLibrary.Events;
using RequestForQuoteInterfacesLibrary.ModelImplementations;
using RequestForQuoteInterfacesLibrary.ModelInterfaces;
using RequestForQuoteInterfacesLibrary.ServiceInterfaces;
using RequestForQuoteServicesModuleLibrary.BookMaintenanceService;

namespace RequestForQuoteServicesModuleLibrary.ServicesImplementation
{
    class BookManagerImpl : IBookManager
    {
        public List<IBook> Books { get; set; }
        private readonly BookControllerClient bookControllerProxy = new BookControllerClient();
        private readonly IEventAggregator eventAggregator = ServiceLocator.Current.GetInstance<IEventAggregator>();

        public BookManagerImpl()
        {
            Books = new List<IBook>();
        }
        
        public void Initialize()
        {
            // No need to add to GUI thread
            foreach (bookDetail book in bookControllerProxy.getAll())
            {
                Books.Add(new BookImpl() { BookCode = book.bookCode, Entity = book.entity, IsValid = book.isValid });
            } 
        }

        public void AddBook(string bookCode, string entity, bool isValid, bool canSaveToDatabase)
        {
            var newBook = new BookImpl() { BookCode = bookCode, Entity = entity, IsValid = isValid };
            
            // Add to collection...
            Books.Add(newBook);

            // Add to database...
            if (canSaveToDatabase)
                bookControllerProxy.save(bookCode, entity, RequestForQuoteConstants.MY_USER_NAME);

            // Publish event for other observer view models
            // TODO - does this not need to be inside the above if statement?
            eventAggregator.GetEvent<NewBookEvent>().Publish(new NewBookEventPayload()
            {
                NewBook = newBook
            });
        }

        public void UpdateValidity(string bookCode, bool isValid)
        {
            bookControllerProxy.updateValidity(bookCode, isValid);
        }
    }
}
