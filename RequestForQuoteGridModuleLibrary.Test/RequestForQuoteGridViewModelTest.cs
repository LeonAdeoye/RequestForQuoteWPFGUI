using System;
using System.Collections.Generic;
using FluentAssertions;
using Microsoft.Practices.Prism.Events;
using Moq;
using NUnit.Framework;
using RequestForQuoteInterfacesLibrary.Enums;
using RequestForQuoteInterfacesLibrary.EventPayloads;
using RequestForQuoteInterfacesLibrary.Events;
using RequestForQuoteInterfacesLibrary.ModelImplementations;
using RequestForQuoteInterfacesLibrary.ModelInterfaces;
using RequestForQuoteInterfacesLibrary.ServiceInterfaces;

namespace RequestForQuoteGridModuleLibrary.Test
{
    internal class RequestForQuoteGridViewModelTest
    {
        private readonly Mock<IBookManager> bookManagerMock = new Mock<IBookManager>();
        private readonly Mock<IConfigurationManager> configManagerMock = new Mock<IConfigurationManager>();
        private readonly Mock<IClientManager> clientManagerMock = new Mock<IClientManager>();
        private readonly Mock<IUnderlyingManager> underlyingManagerMock = new Mock<IUnderlyingManager>();
        private readonly Mock<IEventAggregator> eventAggregatorMock = new Mock<IEventAggregator>();
        private readonly Mock<IChatServiceManager> chatServiceManagerMock = new Mock<IChatServiceManager>();
        private readonly Mock<IOptionRequestPricer> optionRequestPricerMock = new Mock<IOptionRequestPricer>();
        private readonly Mock<IOptionRequestPersistanceManager> optionRequestPersistanceManagerMock = new Mock<IOptionRequestPersistanceManager>();
        private readonly Mock<IOptionRequestParser> optionRequestParserMock = new Mock<IOptionRequestParser>();

        private readonly Mock<NewBookEvent> newBookEventMock = new Mock<NewBookEvent>();
        private readonly Mock<NewClientEvent> newClientEventMock = new Mock<NewClientEvent>();
        private readonly Mock<NewRequestForQuoteEvent> newRequestForQuoteEventMock = new Mock<NewRequestForQuoteEvent>();
        private readonly Mock<ClosedRequestForQuoteDetailsEvent> closedRequestForQuoteDetailsEventMock = new Mock<ClosedRequestForQuoteDetailsEvent>();
        private readonly Mock<SearchRequestForQuoteEvent> searchRequestForQuoteEventMock = new Mock<SearchRequestForQuoteEvent>();    
        private readonly Mock<GetTodaysRequestsEvent> getTodaysRequestsEventMock = new Mock<GetTodaysRequestsEvent>();
        private readonly Mock<NewSerializedRequestEvent> newSerializedRequestEventMock = new Mock<NewSerializedRequestEvent>();

        private RequestForQuoteGridViewModel viewModel;

        private readonly IBook testBook = new BookImpl() { BookCode = "test book" };

        private readonly IClient testClient = new ClientImpl()
        {
            Identifier = 1,
            IsValid = true,
            Name = "test client",
            Tier = TierEnum.Top.ToString()
        };

        [TestFixtureSetUp]
        public void TestFixtureSetUp()
        {
            eventAggregatorMock.Setup(p => p.GetEvent<NewBookEvent>()).Returns(newBookEventMock.Object);
            eventAggregatorMock.Setup(p => p.GetEvent<NewClientEvent>()).Returns(newClientEventMock.Object);
            eventAggregatorMock.Setup(p => p.GetEvent<NewRequestForQuoteEvent>()).Returns(newRequestForQuoteEventMock.Object);
            eventAggregatorMock.Setup(p => p.GetEvent<ClosedRequestForQuoteDetailsEvent>()).Returns(closedRequestForQuoteDetailsEventMock.Object);
            eventAggregatorMock.Setup(p => p.GetEvent<SearchRequestForQuoteEvent>()).Returns(searchRequestForQuoteEventMock.Object);
            eventAggregatorMock.Setup(p => p.GetEvent<GetTodaysRequestsEvent>()).Returns(getTodaysRequestsEventMock.Object);
            eventAggregatorMock.Setup(p => p.GetEvent<NewSerializedRequestEvent>()).Returns(newSerializedRequestEventMock.Object);

            bookManagerMock.Setup(bm => bm.Books).Returns(new List<IBook>() { testBook });
            clientManagerMock.Setup(cm => cm.Clients).Returns(new List<IClient>() { testClient });
            optionRequestPersistanceManagerMock.Setup(orpm => orpm.GetRequestsForToday(true)).Returns(new List<IRequestForQuote>());

            viewModel = new RequestForQuoteGridViewModel(bookManagerMock.Object, clientManagerMock.Object,
                            optionRequestParserMock.Object, optionRequestPricerMock.Object, chatServiceManagerMock.Object, underlyingManagerMock.Object,
                            optionRequestPersistanceManagerMock.Object, eventAggregatorMock.Object, configManagerMock.Object);
        }

        [TearDown]
        public void TearDown()
        {

        }

        [TestFixtureTearDown]
        public void TestFixtureTearDown()
        {
            viewModel = null;
        }

        [Test]
        public void Constructor_BookManager_ArgumentNullExceptionThrown()
        {
            // Act
            Action act = () => viewModel = new RequestForQuoteGridViewModel(null, clientManagerMock.Object,
                optionRequestParserMock.Object, optionRequestPricerMock.Object, chatServiceManagerMock.Object, underlyingManagerMock.Object,
                optionRequestPersistanceManagerMock.Object, eventAggregatorMock.Object, configManagerMock.Object);
            // Assert
            act.ShouldThrow<ArgumentNullException>("because book manager parameter cannot be null.").WithMessage("bookManager", ComparisonMode.Substring);
        }

        [Test]
        public void Constructor_ClientManager_ArgumentNullExceptionThrown()
        {
            // Act
            Action act = () => viewModel = new RequestForQuoteGridViewModel(bookManagerMock.Object, null,
                optionRequestParserMock.Object, optionRequestPricerMock.Object, chatServiceManagerMock.Object, underlyingManagerMock.Object,
                optionRequestPersistanceManagerMock.Object, eventAggregatorMock.Object, configManagerMock.Object);
            // Assert
            act.ShouldThrow<ArgumentNullException>("because client manager parameter cannot be null.").WithMessage("clientManager", ComparisonMode.Substring);
        }

        [Test]
        public void Constructor_OptionRequestParser_ArgumentNullExceptionThrown()
        {
            // Act
            Action act = () => viewModel = new RequestForQuoteGridViewModel(bookManagerMock.Object, clientManagerMock.Object,
                null, optionRequestPricerMock.Object, chatServiceManagerMock.Object, underlyingManagerMock.Object,
                optionRequestPersistanceManagerMock.Object, eventAggregatorMock.Object, configManagerMock.Object);
            // Assert
            act.ShouldThrow<ArgumentNullException>("because option request parser parameter cannot be null.").WithMessage("optionRequestParser", ComparisonMode.Substring);
        }

        [Test]
        public void Constructor_OptionRequestPricer_ArgumentNullExceptionThrown()
        {
            // Act
            Action act = () => viewModel = new RequestForQuoteGridViewModel(bookManagerMock.Object, clientManagerMock.Object,
                optionRequestParserMock.Object, null, chatServiceManagerMock.Object, underlyingManagerMock.Object,
                optionRequestPersistanceManagerMock.Object, eventAggregatorMock.Object, configManagerMock.Object);
            // Assert
            act.ShouldThrow<ArgumentNullException>("because option request pricer parameter cannot be null.").WithMessage("optionRequestPricer", ComparisonMode.Substring);
        }

        [Test]
        public void Constructor_ChatServiceManager_ArgumentNullExceptionThrown()
        {
            // Act
            Action act = () => viewModel = new RequestForQuoteGridViewModel(bookManagerMock.Object, clientManagerMock.Object,
                optionRequestParserMock.Object, optionRequestPricerMock.Object, null, underlyingManagerMock.Object,
                optionRequestPersistanceManagerMock.Object, eventAggregatorMock.Object, configManagerMock.Object);
            // Assert
            act.ShouldThrow<ArgumentNullException>("because chat service manager parameter cannot be null.").WithMessage("chatServiceManager", ComparisonMode.Substring);
        }

        [Test]
        public void Constructor_UnderlyingManager_ArgumentNullExceptionThrown()
        {
            // Act
            Action act = () => viewModel = new RequestForQuoteGridViewModel(bookManagerMock.Object, clientManagerMock.Object,
                optionRequestParserMock.Object, optionRequestPricerMock.Object, chatServiceManagerMock.Object, null,
                optionRequestPersistanceManagerMock.Object, eventAggregatorMock.Object, configManagerMock.Object);
            // Assert
            act.ShouldThrow<ArgumentNullException>("because underlying manager parameter cannot be null.").WithMessage("underlyingManager", ComparisonMode.Substring);
        }

        [Test]
        public void Constructor_RequestPersistanceManager_ArgumentNullExceptionThrown()
        {
            // Act
            Action act = () => viewModel = new RequestForQuoteGridViewModel(bookManagerMock.Object, clientManagerMock.Object,
                optionRequestParserMock.Object, optionRequestPricerMock.Object, chatServiceManagerMock.Object, underlyingManagerMock.Object,
                null, eventAggregatorMock.Object, configManagerMock.Object);
            // Assert
            act.ShouldThrow<ArgumentNullException>("because option request persistance manager parameter cannot be null.").WithMessage("optionRequestPersistanceManager", ComparisonMode.Substring);
        }

        [Test]
        public void Constructor_NullEventAggregator_ArgumentNullExceptionThrown()
        {
            // Act
            Action act = () => viewModel = new RequestForQuoteGridViewModel(bookManagerMock.Object, clientManagerMock.Object,
                optionRequestParserMock.Object, optionRequestPricerMock.Object, chatServiceManagerMock.Object, underlyingManagerMock.Object,
                optionRequestPersistanceManagerMock.Object, null, configManagerMock.Object);
            // Assert
            act.ShouldThrow<ArgumentNullException>("because event aggregator parameter cannot be null.").WithMessage("eventAggregator", ComparisonMode.Substring);
        }

        [Test]
        public void Constructor_NullConfigManager_ArgumentNullExceptionThrown()
        {
            // Act
            Action act = () => viewModel = new RequestForQuoteGridViewModel(bookManagerMock.Object, clientManagerMock.Object,
                optionRequestParserMock.Object, optionRequestPricerMock.Object, chatServiceManagerMock.Object, underlyingManagerMock.Object,
                optionRequestPersistanceManagerMock.Object, eventAggregatorMock.Object, null);
            // Assert
            act.ShouldThrow<ArgumentNullException>("because config manager parameter cannot be null.").WithMessage("configManager", ComparisonMode.Substring);
        }

        [Test]
        public void HandleNewClientEvent_NullParameter_ArgumentNullExceptionThrown()
        {
            Action act = () => viewModel.HandleNewClientEvent(null);

            act.ShouldThrow<ArgumentNullException>("because eventPayload parameter cannot be null").WithMessage("eventPayload", ComparisonMode.Substring);
        }

        [Test]
        public void HandleGetTodaysRequestsEvent_NullParameter_ArgumentNullExceptionThrown()
        {
            Action act = () => viewModel.HandleGetTodaysRequestsEvent(null);

            act.ShouldThrow<ArgumentNullException>("because emptyPayload parameter cannot be null").WithMessage("emptyPayload", ComparisonMode.Substring);
        }

        [Test]
        public void HandleNewBookEvent_NullParameter_ArgumentNullExceptionThrown()
        {
            Action act = () => viewModel.HandleNewBookEvent(null);

            act.ShouldThrow<ArgumentNullException>("because eventPayload parameter cannot be null").WithMessage("eventPayload", ComparisonMode.Substring);
        }

        [Test]
        public void HandlePublishedClosedRequestEvent_NullParameter_ArgumentNullExceptionThrown()
        {
            Action act = () => viewModel.HandlePublishedClosedRequestEvent(null);

            act.ShouldThrow<ArgumentNullException>("because eventPayload parameter cannot be null").WithMessage("eventPayload", ComparisonMode.Substring);
        }

        [Test]
        public void HandleBothFilterAndSearchRequests_NullParameter_ArgumentNullExceptionThrown()
        {
            Action act = () => viewModel.HandleBothFilterAndSearchRequests(null);

            act.ShouldThrow<ArgumentNullException>("because eventPayload parameter cannot be null").WithMessage("eventPayload", ComparisonMode.Substring);
        }

        [Test]
        public void HandlePublishedFilterRequestsEvent_NullParameter_ArgumentNullExceptionThrown()
        {
            Action act = () => viewModel.HandlePublishedFilterRequestsEvent(null);

            act.ShouldThrow<ArgumentNullException>("because eventPayload parameter cannot be null").WithMessage("eventPayload", ComparisonMode.Substring);
        }

        [Test]
        public void HandlePublishedSearchRequestsEvent_NullRequestParameter_ArgumentNullExceptionThrown()
        {
            Action act = () => viewModel.HandlePublishedSearchRequestsEvent(null);

            act.ShouldThrow<ArgumentNullException>("because eventPayload parameter cannot be null").WithMessage("eventPayload", ComparisonMode.Substring);
        }

        [Test]
        public void Constructor_ClientsCollectionShouldBePopulated()
        {
            viewModel.Clients.Should().NotBeEmpty("because the constructor populates it");
        }

        [Test]
        public void Constructor_BooksCollectionShouldBePopulated()
        {
            viewModel.Books.Should().NotBeEmpty("because the constructor populates it");
        }

        [Test]
        public void Constructor_StatusCollectionShouldBePopulated()
        {
            viewModel.Status.Should().NotBeEmpty("because the constructor populates it");
        }

        [Test]
        public void Constructor_NewBookEventShouldBeSubscribedTo()
        {
            newBookEventMock.Verify(
                bm => bm.Subscribe(It.IsAny<Action<NewBookEventPayload>>(), It.IsAny<ThreadOption>(),
                                   It.IsAny<bool>(), It.IsAny<Predicate<NewBookEventPayload>>()), Times.Once(),
                "view model constructor did not subscribe to new book event!");
        }

        [Test]
        public void Constructor_NewClientEventShouldBeSubscribedTo()
        {
            newClientEventMock.Verify(
                bm => bm.Subscribe(It.IsAny<Action<NewClientEventPayload>>(), It.IsAny<ThreadOption>(),
                                   It.IsAny<bool>(), It.IsAny<Predicate<NewClientEventPayload>>()), Times.Once(),
                "view model constructor did not subscribe to new client event!");
        }

        [Test]
        public void Constructor_NewRequestForQuoteEventShouldBeSubscribedTo()
        {
            newRequestForQuoteEventMock.Verify(
                bm => bm.Subscribe(It.IsAny<Action<NewRequestForQuoteEventPayload>>(), It.IsAny<ThreadOption>(),
                                   It.IsAny<bool>(), It.IsAny<Predicate<NewRequestForQuoteEventPayload>>()), Times.Once(),
                "view model constructor did not subscribe to new request for quote event!");
        }

        [Test]
        public void Constructor_ClosedRequestForQuoteDetailsEventShouldBeSubscribedTo()
        {
            closedRequestForQuoteDetailsEventMock.Verify(
                bm => bm.Subscribe(It.IsAny<Action<ClosedRequestForQuoteDetailsEventPayload>>(), It.IsAny<ThreadOption>(),
                                   It.IsAny<bool>(), It.IsAny<Predicate<ClosedRequestForQuoteDetailsEventPayload>>()), Times.Once(),
                "view model constructor did not subscribe to new closed request for quote details event!");
        }

        [Test]
        public void Constructor_SearchRequestForQuoteEventShouldBeSubscribedTo()
        {
            searchRequestForQuoteEventMock.Verify(
                bm => bm.Subscribe(It.IsAny<Action<CriteriaUsageEventPayload>>(), It.IsAny<ThreadOption>(),
                                   It.IsAny<bool>(), It.IsAny<Predicate<CriteriaUsageEventPayload>>()), Times.Once(),
                "view model constructor did not subscribe to new search request for quote details event!");
        }

        [Test]
        public void Constructor_GetTodaysRequestsEventShouldBeSubscribedTo()
        {
            getTodaysRequestsEventMock.Verify(
                bm => bm.Subscribe(It.IsAny<Action<EmptyEventPayload>>(), It.IsAny<ThreadOption>(),
                                   It.IsAny<bool>(), It.IsAny<Predicate<EmptyEventPayload>>()), Times.Once(),
                "view model constructor did not subscribe to get todays requests event!");
        }

        [Test]
        public void Constructor_NewSerializedRequestEventShouldBeSubscribedTo()
        {
            newSerializedRequestEventMock.Verify(
                bm => bm.Subscribe(It.IsAny<Action<NewSerializedRequestEventPayload>>(), It.IsAny<ThreadOption>(),
                                   It.IsAny<bool>(), It.IsAny<Predicate<NewSerializedRequestEventPayload>>()), Times.Once(),
                "view model constructor did not subscribe to new serialized request event!");
        }
    }
}
