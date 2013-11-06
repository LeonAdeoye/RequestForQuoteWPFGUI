using System;
using System.Collections.Generic;
using System.ServiceModel;
using Microsoft.Practices.Prism.Events;
using RequestForQuoteInterfacesLibrary.EventPayloads;
using RequestForQuoteInterfacesLibrary.Events;
using RequestForQuoteInterfacesLibrary.ModelImplementations;
using RequestForQuoteInterfacesLibrary.ModelInterfaces;
using RequestForQuoteInterfacesLibrary.ServiceInterfaces;
using RequestForQuoteServicesModuleLibrary.BookMaintenanceService;
using log4net;

namespace RequestForQuoteServicesModuleLibrary.ServicesImplementation
{
    public sealed class BookManagerImpl : IBookManager
    {
        private static readonly ILog log = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private readonly BookControllerClient bookControllerProxy;
        private readonly IEventAggregator eventAggregator;
        private readonly IConfigurationManager configManager;
        public List<IBook> Books { get; set; }

        /// <summary>
        /// constructor
        /// </summary>
        /// <param name="configManager"> used to pass current user to web service save and update operations.</param>
        /// <param name="eventAggregator"> used to publish new book updates to listening components.</param>
        /// <param name="bookControllerProxy"> proxy for web service operations.</param>
        /// <exception cref="ArgumentNullException"> if configManager or eventAggregator or bookControllerProxy is null.</exception>
        public BookManagerImpl(IConfigurationManager configManager, IEventAggregator eventAggregator, BookControllerClient bookControllerProxy)
        {
            if (configManager == null)
                throw new ArgumentNullException("configManager");

            if (eventAggregator == null)
                throw new ArgumentNullException("eventAggregator");

            if (bookControllerProxy == null)
                throw new ArgumentNullException("bookControllerProxy");

            this.configManager = configManager;
            this.eventAggregator = eventAggregator;
            this.bookControllerProxy = bookControllerProxy;

            Books = new List<IBook>();
        }

        /// <summary>
        /// Initializes the Books collection.
        /// In STANDALONE mode the Books collection is populated with dummy data.
        /// in WEB SERVICE mode the Books collection is populated with books returned by the web service getAll method.
        /// </summary>
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

        /// <summary>
        /// Adds a book to the Books collection referenced by components in the GUI,
        /// and publishes the details of the book to all listener components.
        /// </summary>
        /// <param name="bookCode"> the book code of the book to be published.</param>
        /// <param name="entity"> the entity of the book to to be published.</param>
        /// <param name="isValid"> the validity state of the book to be published.</param>
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

        /// <summary>
        /// Saves the new book to the database via a web service call.
        /// </summary>
        /// <param name="bookCode"> the bookCode of the book to be saved.</param>
        /// <param name="entity"> the entity of the book to be saved.</param>
        /// <param name="isValid"> the validity state of the book to be saved.</param>
        /// <returns> true if the save operation is successful; false otherwise.</returns>
        /// <exception cref="ArgumentException"> thrown if the bookCode or the entity is null or empty.</exception>
        public bool SaveToDatabase(string bookCode, string entity, bool isValid)
        {
            if (String.IsNullOrEmpty(bookCode))
                throw new ArgumentException("bookCode");

            if (String.IsNullOrEmpty(entity))
                throw new ArgumentException("entity");

            return bookControllerProxy.save(bookCode, entity, configManager.CurrentUser);
        }

        /// <summary>
        /// Updates the validity of a book via a web service call.
        /// </summary>
        /// <param name="bookCode"> the book code of the book that the update will be applied to.</param>
        /// <param name="isValid"> the valid/invalid flag.</param>
        /// <returns> true if the update is successful; false otherwise</returns>
        /// <exception cref="ArgumentException"> thrown if the bookCode is null or empty.</exception>
        public bool UpdateValidity(string bookCode, bool isValid)
        {
            if (String.IsNullOrEmpty(bookCode))
                throw new ArgumentException("bookCode");

            return bookControllerProxy.updateValidity(bookCode, isValid);
        }

        /// <summary>
        /// Deletes a book via a web service call.
        /// </summary>
        /// <param name="bookCode"> the book to be deleted.</param>
        /// <returns> return true if the book is deleted; false otherwise.</returns>
        /// <exception cref="ArgumentException"> thrown if the bookCode is null or empty.</exception>
        bool IBookManager.RemoveBook(string bookCode)
        {
            if (String.IsNullOrEmpty(bookCode))
                throw new ArgumentException("bookCode");

            return bookControllerProxy.delete(bookCode);
        }
    }
}
