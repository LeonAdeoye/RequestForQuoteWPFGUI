using System;
using FluentAssertions;
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
            bookManager.Books.Should().NotBeNull("because the constructor instantiates it.");
        }        

        [Test]
        public void Constructor_NullConfigManager_ArgumentNullExceptionThrown()
        {
            // Act
            Action act = () => new BookManagerImpl(null, eventAggregatorMock.Object, bookController.Object);
            // Assert
            act.ShouldThrow<ArgumentNullException>("because configManager parameter cannot be null").WithMessage("configManager", ComparisonMode.Substring);
        }

        [Test]
        public void Constructor_NullEventAggregator_ArgumentNullExceptionThrown()
        {
            // Act
            Action act = () => new BookManagerImpl(configManagerMock.Object, null, bookController.Object);
            // Assert
            act.ShouldThrow<ArgumentNullException>("because event aggregator parameter cannot be null.").WithMessage("eventAggregator", ComparisonMode.Substring);
        }

        [Test]
        public void Constructor_NullBookControllerProxy_ArgumentNullExceptionThrown()
        {
            // Act
            Action act = () => new BookManagerImpl(configManagerMock.Object, eventAggregatorMock.Object, null);
            // Assert
            act.ShouldThrow<ArgumentNullException>("because book controller proxy parameter cannot be null.").WithMessage("bookControllerProxy", ComparisonMode.Substring);
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
            bookManager.Books.Should().NotBeEmpty("because it is populated in standalone mode.");
        }

        [TestCase(null)]
        [TestCase("")]
        public void AddBook_InvalidBookCodeParameter_ArgumentExceptionThrown(string bookCode)
        {
            // Arrange
            IBookManager bookManager = new BookManagerImpl(configManagerMock.Object, eventAggregatorMock.Object, bookController.Object);
            // Act
            Action act = () => bookManager.AddBook(bookCode, "test entity", true);
            // Assert
            act.ShouldThrow<ArgumentException>("because book code parameter cannot be empty or null.").WithMessage("bookCode", ComparisonMode.Substring);
        }

        [TestCase(null)]
        [TestCase("")]
        public void AddBook_InvalidEntityParameter_ArgumentExceptionThrown(string entity)
        {
            // Arrange
            IBookManager bookManager = new BookManagerImpl(configManagerMock.Object, eventAggregatorMock.Object, bookController.Object);
            // Act
            Action act = () => bookManager.AddBook("test bookCode", entity, true);
            // Assert
            act.ShouldThrow<ArgumentException>("because entity parameter cannot be empty or null.").WithMessage("entity", ComparisonMode.Substring);
        }

        [Test]
        public void AddBook_ValidParameters_BooksCollectionPopulated()
        {
            // Arrange
            IBookManager bookManager = new BookManagerImpl(configManagerMock.Object, eventAggregatorMock.Object, bookController.Object);
            // Act
            bookManager.AddBook("test bookCode", "test entity", true);
            // Assert
            bookManager.Books.Should().NotBeEmpty("because a new book is added to the collection by AddBook");
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
            wasCalled.Should().BeTrue("because a new book is published to all listeners by the AddBook method.");
        }

        [TestCase(null)]
        [TestCase("")]
        public void SaveToDatabase_InvalidBookCodeParameter_ArgumentExceptionThrown(String bookCode)
        {
            // Arrange
            IBookManager bookManager = new BookManagerImpl(configManagerMock.Object, eventAggregatorMock.Object, bookController.Object);
            // Act
            Action act = () => bookManager.SaveToDatabase(bookCode, "test entity", true);
            // Assert
            act.ShouldThrow<ArgumentException>("because book code parameter cannot be empty or null.").WithMessage("bookCode", ComparisonMode.Substring);
        }

        [TestCase(null)]
        [TestCase("")]
        public void SaveToDatabase_InvalidEntityParameter_ArgumentExceptionThrown(String entity)
        {
            // Arrange
            IBookManager bookManager = new BookManagerImpl(configManagerMock.Object, eventAggregatorMock.Object, bookController.Object);
            // Act
            Action act = () => bookManager.SaveToDatabase("test bookCode", entity, true);
            // Assert
            act.ShouldThrow<ArgumentException>("because entity parameter cannot be empty or null.").WithMessage("entity", ComparisonMode.Substring);
        }

        [TestCase(null)]
        [TestCase("")]
        public void UpdateValidity_InvalidBookCodeParameter_ArgumentExceptionThrown(String bookCode)
        {
            // Arrange
            IBookManager bookManager = new BookManagerImpl(configManagerMock.Object, eventAggregatorMock.Object, bookController.Object);
            // Act
            Action act = () => bookManager.UpdateValidity(bookCode, true);
            // Assert
            act.ShouldThrow<ArgumentException>("because book code parameter cannot be empty or null.").WithMessage("bookCode", ComparisonMode.Substring);
        }

        [TestCase(null)]
        [TestCase("")]
        public void RemoveBook_InvalidBookCodeParameter_ArgumentExceptionThrown(String bookCode)
        {
            // Arrange
            IBookManager bookManager = new BookManagerImpl(configManagerMock.Object, eventAggregatorMock.Object, bookController.Object);
            // Act
            Action act = () => bookManager.RemoveBook(bookCode);
            // Assert
            act.ShouldThrow<ArgumentException>("because book code parameter cannot be empty or null.").WithMessage("bookCode", ComparisonMode.Substring);
        }
    }
}
