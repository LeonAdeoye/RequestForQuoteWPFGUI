using System;
using Microsoft.Practices.Prism.Events;
using Moq;
using NUnit.Framework;
using RequestForQuoteInterfacesLibrary.EventPayloads;
using RequestForQuoteInterfacesLibrary.Events;
using RequestForQuoteInterfacesLibrary.ServiceInterfaces;
using RequestForQuoteServicesModuleLibrary.BookMaintenanceService;
using RequestForQuoteServicesModuleLibrary.ServicesImplementation;

namespace RequestForQuoteServicesModuleLibrary.Test
{
    [TestFixture]
    public class BookManagerTest
    {
        private readonly Mock<BookControllerClient> bookController = new Mock<BookControllerClient>();
        private readonly Mock<IEventAggregator> eventAggregatorMock = new Mock<IEventAggregator>();
        private readonly Mock<IConfigurationManager> configManagerMock = new Mock<IConfigurationManager>();
        private readonly Mock<NewBookEvent> newBookEventMock = new Mock<NewBookEvent>();

        [TestFixtureSetUp]
        public void TestFixtureSetUp()
        {            
            eventAggregatorMock.Setup(p => p.GetEvent<NewBookEvent>()).Returns(newBookEventMock.Object);    
        }

        [Test]
        public void Constructor_ValidParameters_BooksCollectionInstaniated()
        {
            // Act
            IBookManager bookManager = new BookManagerImpl(configManagerMock.Object, eventAggregatorMock.Object, bookController.Object);
            // Assert
            Assert.NotNull(bookManager.Books, "the constructor should instantiate it");            
        }        

        [Test]
        [ExpectedException("System.ArgumentNullException")]
        public void Constructor_NullConfigManager_ArgumentNullExceptionThrown()
        {
            // Act            
            var test = new BookManagerImpl(null, eventAggregatorMock.Object, bookController.Object);         
        }

        [Test]
        [ExpectedException("System.ArgumentNullException")]
        public void Constructor_NullEventAggregator_ArgumentNullExceptionThrown()
        {
            // Act
            var test = new BookManagerImpl(configManagerMock.Object, null, bookController.Object);
        }


        [Test]
        [ExpectedException("System.ArgumentNullException")]
        public void Constructor_NullBookControllerProxy_ArgumentNullExceptionThrown()
        {
            // Act
            var test = new BookManagerImpl(configManagerMock.Object, eventAggregatorMock.Object, null);            
        }

        [Test]
        public void Initialize_StandAloneMode_BooksCollectionPopulated()
        {
            // Arrange
            IBookManager bookManager = new BookManagerImpl(configManagerMock.Object, eventAggregatorMock.Object, bookController.Object);
            configManagerMock.Setup(cm => cm.IsStandAlone).Returns(true);
            // Act
            bookManager.Initialize();
            // Assert
            Assert.IsNotEmpty(bookManager.Books, "because it is populated in standalone mode.");
        }

        [TestCase(null)]
        [TestCase("")]
        [ExpectedException(typeof(ArgumentException), ExpectedMessage = "bookCode")]
        public void AddBook_InvalidBookCodeParameter_ArgumentExceptionThrown(string bookCode)
        {
            // Arrange
            IBookManager bookManager = new BookManagerImpl(configManagerMock.Object, eventAggregatorMock.Object, bookController.Object);
            // Act
            bookManager.AddBook(bookCode, "test entity", true);
        }

        [TestCase(null)]
        [TestCase("")]
        [ExpectedException(typeof(ArgumentException), ExpectedMessage = "entity")]
        public void AddBook_InvalidEntityParameter_ArgumentExceptionThrown(string entity)
        {
            // Arrange
            IBookManager bookManager = new BookManagerImpl(configManagerMock.Object, eventAggregatorMock.Object, bookController.Object);
            // Act
            bookManager.AddBook("test bookCode", entity, true);
        }

        [Test]
        public void AddBook_ValidParameters_BooksCollectionPopulated()
        {
            // Arrange
            IBookManager bookManager = new BookManagerImpl(configManagerMock.Object, eventAggregatorMock.Object, bookController.Object);
            // Act
            bookManager.AddBook("test bookCode", "test entity", true);
            // Assert
            Assert.IsNotEmpty(bookManager.Books, "because a new book is added to the collection by AddBook");            
        }

        [Test]
        public void AddBook_ValidParameters_NewBookEventShouldBePublished()
        {
            // Arrange
            IBookManager bookManager = new BookManagerImpl(configManagerMock.Object, eventAggregatorMock.Object, bookController.Object);
            var wasCalled = false;
            eventAggregatorMock.Setup(ea => ea.GetEvent<NewBookEvent>().Publish(It.IsAny<NewBookEventPayload>())).Callback(() => wasCalled = true);
            // Act
            bookManager.AddBook("test bookCode", "test entity", true);
            // Assert
            Assert.IsTrue(wasCalled, "because a new book is published to all listeners by the AddBook method.");           
        }

        [TestCase(null)]
        [TestCase("")]
        [ExpectedException(typeof(ArgumentException), ExpectedMessage = "bookCode")]
        public void SaveToDatabase_InvalidBookCodeParameter_ArgumentExceptionThrown(String bookCode)
        {
            // Arrange
            IBookManager bookManager = new BookManagerImpl(configManagerMock.Object, eventAggregatorMock.Object, bookController.Object);
            // Act
            bookManager.SaveToDatabase(bookCode, "test entity", true);
        }

        [TestCase(null)]
        [TestCase("")]
        [ExpectedException(typeof(ArgumentException), ExpectedMessage = "entity")]
        public void SaveToDatabase_InvalidEntityParameter_ArgumentExceptionThrown(String entity)
        {
            // Arrange
            IBookManager bookManager = new BookManagerImpl(configManagerMock.Object, eventAggregatorMock.Object, bookController.Object);
            // Act
            bookManager.SaveToDatabase("test bookCode", entity, true);
        }

        [TestCase(null)]
        [TestCase("")]
        [ExpectedException(typeof(ArgumentException), ExpectedMessage = "bookCode")]
        public void UpdateValidity_InvalidBookCodeParameter_ArgumentExceptionThrown(String bookCode)
        {
            // Arrange
            IBookManager bookManager = new BookManagerImpl(configManagerMock.Object, eventAggregatorMock.Object, bookController.Object);
            // Act
            bookManager.UpdateValidity(bookCode, true);            
        }

        [TestCase(null)]
        [TestCase("")]
        [ExpectedException(typeof(ArgumentException), ExpectedMessage = "bookCode")]
        public void RemoveBook_InvalidBookCodeParameter_ArgumentExceptionThrown(String bookCode)
        {
            // Arrange
            IBookManager bookManager = new BookManagerImpl(configManagerMock.Object, eventAggregatorMock.Object, bookController.Object);
            // Act
            bookManager.RemoveBook(bookCode);
        }
    }
}
