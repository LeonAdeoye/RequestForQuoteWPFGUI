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
        private readonly Mock<SearchRequestForQuoteEvent> searchRequestForQuoteEventMock = new Mock<SearchRequestForQuoteEvent>();    
        private readonly Mock<GetTodaysRequestsEvent> getTodaysRequestsEventMock = new Mock<GetTodaysRequestsEvent>();
        private readonly Mock<NewSerializedRequestEvent> newSerializedRequestEventMock = new Mock<NewSerializedRequestEvent>();

        private RequestForQuoteGridViewModel viewModel;
        private bool wasCalled;

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

        [SetUp]
        public void SetUpBeforeEachAndEveryTest()
        {
            wasCalled = false;            
        }

        [TestFixtureTearDown]
        public void TestFixtureTearDown()
        {
            viewModel = null;
        }

        [Test]
        public void Constructor_NullBookManager_ArgumentNullExceptionThrown()
        {
            // Act
            Action act = () => viewModel = new RequestForQuoteGridViewModel(null, clientManagerMock.Object,
                optionRequestParserMock.Object, optionRequestPricerMock.Object, chatServiceManagerMock.Object, underlyingManagerMock.Object,
                optionRequestPersistanceManagerMock.Object, eventAggregatorMock.Object, configManagerMock.Object);
            // Assert
            act.ShouldThrow<ArgumentNullException>("because book manager parameter cannot be null.").WithMessage("bookManager", ComparisonMode.Substring);
        }

        [Test]
        public void Constructor_NullClientManager_ArgumentNullExceptionThrown()
        {
            // Act
            Action act = () => viewModel = new RequestForQuoteGridViewModel(bookManagerMock.Object, null,
                optionRequestParserMock.Object, optionRequestPricerMock.Object, chatServiceManagerMock.Object, underlyingManagerMock.Object,
                optionRequestPersistanceManagerMock.Object, eventAggregatorMock.Object, configManagerMock.Object);
            // Assert
            act.ShouldThrow<ArgumentNullException>("because client manager parameter cannot be null.").WithMessage("clientManager", ComparisonMode.Substring);
        }

        [Test]
        public void Constructor_NullOptionRequestParser_ArgumentNullExceptionThrown()
        {
            // Act
            Action act = () => viewModel = new RequestForQuoteGridViewModel(bookManagerMock.Object, clientManagerMock.Object,
                null, optionRequestPricerMock.Object, chatServiceManagerMock.Object, underlyingManagerMock.Object,
                optionRequestPersistanceManagerMock.Object, eventAggregatorMock.Object, configManagerMock.Object);
            // Assert
            act.ShouldThrow<ArgumentNullException>("because option request parser parameter cannot be null.").WithMessage("optionRequestParser", ComparisonMode.Substring);
        }

        [Test]
        public void Constructor_NullOptionRequestPricer_ArgumentNullExceptionThrown()
        {
            // Act
            Action act = () => viewModel = new RequestForQuoteGridViewModel(bookManagerMock.Object, clientManagerMock.Object,
                optionRequestParserMock.Object, null, chatServiceManagerMock.Object, underlyingManagerMock.Object,
                optionRequestPersistanceManagerMock.Object, eventAggregatorMock.Object, configManagerMock.Object);
            // Assert
            act.ShouldThrow<ArgumentNullException>("because option request pricer parameter cannot be null.").WithMessage("optionRequestPricer", ComparisonMode.Substring);
        }

        [Test]
        public void Constructor_NullChatServiceManager_ArgumentNullExceptionThrown()
        {
            // Act
            Action act = () => viewModel = new RequestForQuoteGridViewModel(bookManagerMock.Object, clientManagerMock.Object,
                optionRequestParserMock.Object, optionRequestPricerMock.Object, null, underlyingManagerMock.Object,
                optionRequestPersistanceManagerMock.Object, eventAggregatorMock.Object, configManagerMock.Object);
            // Assert
            act.ShouldThrow<ArgumentNullException>("because chat service manager parameter cannot be null.").WithMessage("chatServiceManager", ComparisonMode.Substring);
        }

        [Test]
        public void Constructor_NullUnderlyingManager_ArgumentNullExceptionThrown()
        {
            // Act
            Action act = () => viewModel = new RequestForQuoteGridViewModel(bookManagerMock.Object, clientManagerMock.Object,
                optionRequestParserMock.Object, optionRequestPricerMock.Object, chatServiceManagerMock.Object, null,
                optionRequestPersistanceManagerMock.Object, eventAggregatorMock.Object, configManagerMock.Object);
            // Assert
            act.ShouldThrow<ArgumentNullException>("because underlying manager parameter cannot be null.").WithMessage("underlyingManager", ComparisonMode.Substring);
        }

        [Test]
        public void Constructor_NullRequestPersistanceManager_ArgumentNullExceptionThrown()
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
            // Act
            Action act = () => viewModel.HandleNewClientEvent(null);
            // Assert
            act.ShouldThrow<ArgumentNullException>("because eventPayload parameter cannot be null").WithMessage("eventPayload", ComparisonMode.Substring);
        }

        [Test]
        public void HandleGetTodaysRequestsEvent_NullParameter_ArgumentNullExceptionThrown()
        {
            // Act
            Action act = () => viewModel.HandleGetTodaysRequestsEvent(null);
            // Assert
            act.ShouldThrow<ArgumentNullException>("because emptyPayload parameter cannot be null").WithMessage("emptyPayload", ComparisonMode.Substring);
        }

        [Test]
        public void HandleNewBookEvent_NullParameter_ArgumentNullExceptionThrown()
        {
            // Act
            Action act = () => viewModel.HandleNewBookEvent(null);
            // Assert
            act.ShouldThrow<ArgumentNullException>("because eventPayload parameter cannot be null").WithMessage("eventPayload", ComparisonMode.Substring);
        }

        [Test]
        public void HandleBothFilterAndSearchRequests_NullParameter_ArgumentNullExceptionThrown()
        {
            // Act
            Action act = () => viewModel.HandleBothFilterAndSearchRequests(null);
            // Assert
            act.ShouldThrow<ArgumentNullException>("because eventPayload parameter cannot be null").WithMessage("eventPayload", ComparisonMode.Substring);
        }

        [Test]
        public void HandlePublishedFilterRequestsEvent_NullParameter_ArgumentNullExceptionThrown()
        {
            // Act
            Action act = () => viewModel.HandlePublishedFilterRequestsEvent(null);
            // Assert
            act.ShouldThrow<ArgumentNullException>("because eventPayload parameter cannot be null").WithMessage("eventPayload", ComparisonMode.Substring);
        }

        [Test]
        public void HandlePublishedSearchRequestsEvent_NullRequestParameter_ArgumentNullExceptionThrown()
        {
            // Act
            Action act = () => viewModel.HandlePublishedSearchRequestsEvent(null);
            // Assert
            act.ShouldThrow<ArgumentNullException>("because eventPayload parameter cannot be null").WithMessage("eventPayload", ComparisonMode.Substring);
        }

        [Test]
        public void Constructor_ClientsCollectionShouldBePopulated()
        {
            // Assert
            viewModel.Clients.Should().NotBeEmpty("because the constructor populates it");
        }

        [Test]
        public void Constructor_BooksCollectionShouldBePopulated()
        {
            // Assert
            viewModel.Books.Should().NotBeEmpty("because the constructor populates it");
        }

        [Test]
        public void Constructor_StatusCollectionShouldBePopulated()
        {
            // Assert
            viewModel.Status.Should().NotBeEmpty("because the constructor populates it");
        }

        [Test]
        public void Constructor_NewBookEventShouldBeSubscribedTo()
        {
            // Assert
            newBookEventMock.Verify(
                bm => bm.Subscribe(It.IsAny<Action<NewBookEventPayload>>(), It.IsAny<ThreadOption>(),
                                   It.IsAny<bool>(), It.IsAny<Predicate<NewBookEventPayload>>()), Times.Once(),
                "view model constructor did not subscribe to new book event!");
        }

        [Test]
        public void Constructor_NewClientEventShouldBeSubscribedTo()
        {
            // Assert
            newClientEventMock.Verify(
                bm => bm.Subscribe(It.IsAny<Action<NewClientEventPayload>>(), It.IsAny<ThreadOption>(),
                                   It.IsAny<bool>(), It.IsAny<Predicate<NewClientEventPayload>>()), Times.Once(),
                "view model constructor did not subscribe to new client event!");
        }

        [Test]
        public void Constructor_NewRequestForQuoteEventShouldBeSubscribedTo()
        {
            // Assert
            newRequestForQuoteEventMock.Verify(
                bm => bm.Subscribe(It.IsAny<Action<NewRequestForQuoteEventPayload>>(), It.IsAny<ThreadOption>(),
                                   It.IsAny<bool>(), It.IsAny<Predicate<NewRequestForQuoteEventPayload>>()), Times.Once(),
                "view model constructor did not subscribe to new request for quote event!");
        }

        [Test]
        public void Constructor_SearchRequestForQuoteEventShouldBeSubscribedTo()
        {
            // Assert
            searchRequestForQuoteEventMock.Verify(
                bm => bm.Subscribe(It.IsAny<Action<CriteriaUsageEventPayload>>(), It.IsAny<ThreadOption>(),
                                   It.IsAny<bool>(), It.IsAny<Predicate<CriteriaUsageEventPayload>>()), Times.Once(),
                "view model constructor did not subscribe to new search request for quote details event!");
        }

        [Test]
        public void Constructor_GetTodaysRequestsEventShouldBeSubscribedTo()
        {
            // Assert
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

        [Test]
        public void CanShowDetailsWindow_NonNullSelectedRequest_ReturnsTrue()
        {
            // Arrange
            Mock<IRequestForQuote> requestMock = new Mock<IRequestForQuote>();
            viewModel.SelectedRequest = requestMock.Object;
            // Assert
            viewModel.CanShowDetailsWindow().Should().BeTrue("because SelectedRequest != null");
        }

        [Test]
        public void CanAddNewRequest_EmptyNewRequest_ReturnsFalse()
        {
            // Arrange
            viewModel.NewRequest = String.Empty;
            // Assert
            viewModel.CanAddNewRequest().Should().BeFalse("because NewRequest is empty");
        }

        [Test]
        public void CanAddNewRequest_NullNewRequest_ReturnsFalse()
        {
            // Arrange
            viewModel.NewRequest = null;
            // Assert
            viewModel.CanAddNewRequest().Should().BeFalse("because NewRequest is null");
        }

        [Test]
        public void CanAddNewRequest_ValidNewRequestAndClient_ReturnsTrue()
        {
            // Arrange
            viewModel.NewRequest = "C 100 23Dec2020 1111.T";
            optionRequestParserMock.Setup(orp => orp.IsValidOptionRequest(It.IsAny<String>())).Returns(true);
            viewModel.NewRequestClient = testClient;
            // Assert
            viewModel.CanAddNewRequest().Should().BeTrue("because NewRequest and NewRequestClient are valid");
        }

        [Test]
        public void CanAddNewRequest_NullClient_ReturnsFalse()
        {
            // Arrange
            viewModel.NewRequest = "C 100 23Dec2020 1111.T";
            optionRequestParserMock.Setup(orp => orp.IsValidOptionRequest(It.IsAny<String>())).Returns(true);
            viewModel.NewRequestClient = null;
            // Assert
            viewModel.CanAddNewRequest().Should().BeFalse("because NewRequestClient is null");
        }

        [Test]
        public void CanAddNewRequest_InvalidNewRequest_ReturnsFalse()
        {
            // Arrange
            viewModel.NewRequest = "C 100 23Dec2020 1111.T";
            optionRequestParserMock.Setup(orp => orp.IsValidOptionRequest(It.IsAny<String>())).Returns(false);
            // Assert
            viewModel.CanAddNewRequest().Should().BeFalse("because NewRequest parsing returns false");
        }

        [Test]
        public void CanClearNewRequest_ValidNewRequestAndValidClient_ReturnsTrue()
        {
            // Arrange
            viewModel.NewRequest = "C 100 23Dec2020 1111.T";          
            // Assert
            viewModel.CanClearNewRequest().Should().BeTrue("because the new request string is valid");
        }

        [Test]
        public void CanClearNewRequest_NullNewRequest_ReturnsTrue()
        {
            // Arrange
            viewModel.NewRequest = null;
            viewModel.NewRequestClient = testClient; 
            // Assert
            viewModel.CanClearNewRequest().Should().BeTrue("because the new request string is null and the client is not null");
        }

        [Test]
        public void CanClearNewRequest_EmptyNewRequest_ReturnsTrue()
        {
            // Arrange
            viewModel.NewRequest = String.Empty;
            viewModel.NewRequestClient = testClient; 
            // Assert
            viewModel.CanClearNewRequest().Should().BeTrue("because the new request string is empty and new client is not null");
        }

        [Test]
        public void CanClearNewRequest_NullClient_ReturnsFalse()
        {
            // Arrange
            viewModel.NewRequest = String.Empty;
            viewModel.NewRequestClient = null;
            // Assert
            viewModel.CanClearNewRequest().Should().BeFalse("because NewRequestClient is null");
        }

        [Test]
        public void CanInvalidateRequest_NullSelectedRequest_ReturnsFalse()
        {
            // Arrange
            viewModel.SelectedRequest = null;            
            // Assert
            viewModel.CanInvalidateRequest().Should().BeFalse("because SelectedRequest is null");
        }

        [Test]
        public void CanInvalidateRequest_SelectedRequestStatusIsInvalid_ReturnsFalse()
        {
            // Arrange
            Mock<IRequestForQuote> requestMock = new Mock<IRequestForQuote>();
            viewModel.SelectedRequest = requestMock.Object;
            requestMock.Setup(sr => sr.Status).Returns(StatusEnum.INVALID);
            // Assert
            viewModel.CanInvalidateRequest().Should().BeFalse("because SelectedRequest's status is INVALID");
        }

        [Test]
        public void CanInvalidateRequest_SelectedRequestStatusIsNotInvalid_ReturnsTrue()
        {
            // Arrange
            Mock<IRequestForQuote> requestMock = new Mock<IRequestForQuote>();
            viewModel.SelectedRequest = requestMock.Object;
            requestMock.Setup(sr => sr.Status).Returns(StatusEnum.FILLED);
            // Assert
            viewModel.CanInvalidateRequest().Should().BeTrue("because SelectedRequest's status is NOT INVALID");
        }

        [Test]
        public void IsSelectedRequestNull_SelectedRequestIsNull_ReturnsTrue()
        {
            // Arrange            
            viewModel.SelectedRequest = null;
            // Assert
            viewModel.IsSelectedRequestNull().Should().BeTrue("because SelectedRequest is Null");
        }

        [Test]
        public void IsSelectedRequestNull_SelectedRequestIsNotNull_ReturnsFalse()
        {
            // Arrange            
            Mock<IRequestForQuote> requestMock = new Mock<IRequestForQuote>();
            viewModel.SelectedRequest = requestMock.Object;
            // Assert
            viewModel.IsSelectedRequestNull().Should().BeFalse("because SelectedRequest is NOT Null");
        }

        [Test]
        public void HandleNewClientEvent_ValidClientEventPayload_NotAddedToCollection()
        {
            // Arrange
            var currentCount = viewModel.Clients.Count;
            // Act
           viewModel.HandleNewClientEvent(new NewClientEventPayload() {NewClient = testClient});            
            // Assert
            viewModel.Clients.Count.ShouldBeEquivalentTo(currentCount + 1);
        }

        [Test]
        public void HandleNewBookEvent_ValidBookEventPayload_NotAddedToCollection()
        {
            // Arrange
            var currentCount = viewModel.Books.Count;
            // Act
            viewModel.HandleNewBookEvent(new NewBookEventPayload() { NewBook = testBook });
            // Assert
            viewModel.Books.Count.ShouldBeEquivalentTo(currentCount + 1);
        }

        [Test]
        public void HandleNewSerializedRequestEvent_SameTradeDate_ShouldBeAddedToCollection()
        {            
            // Arrange
            viewModel.TodaysRequests.Clear();
            viewModel.Requests.Clear();
            viewModel.TodaysRequests.Add(new RequestForQuoteImpl() {Identifier = 1});
            // Act
            viewModel.HandleNewSerializedRequestEvent(new NewSerializedRequestEventPayload() { NewSerializedRequest = new RequestForQuoteImpl() { Identifier = 2, TradeDate = DateTime.Today } });
            // Assert
            viewModel.Requests.Count.ShouldBeEquivalentTo(1);            
        }

        [Test]
        public void HandleNewSerializedRequestEvent_DifferentTradeDate_ShouldNotBeAddedToCollection()
        {
            // Arrange
            viewModel.TodaysRequests.Clear();
            viewModel.Requests.Clear();
            viewModel.TodaysRequests.Add(new RequestForQuoteImpl() { Identifier = 1 });
            // Act
            viewModel.HandleNewSerializedRequestEvent(new NewSerializedRequestEventPayload() { NewSerializedRequest = new RequestForQuoteImpl() { Identifier = 2, TradeDate = DateTime.Today.AddDays(-1.0)
            } });
            // Assert
            viewModel.Requests.Count.ShouldBeEquivalentTo(0);
        }

        [Test]
        public void HandleNewSerializedRequestEvent_SameIdentifier_ShouldNotBeAddedToCollection()
        {
            // Arrange
            viewModel.TodaysRequests.Clear();
            viewModel.Requests.Clear();
            viewModel.TodaysRequests.Add(new RequestForQuoteImpl() { Identifier = 1 });
            // Act
            viewModel.HandleNewSerializedRequestEvent(new NewSerializedRequestEventPayload() { NewSerializedRequest = new RequestForQuoteImpl() { Identifier = 1, TradeDate = DateTime.Today } });
            // Assert
            viewModel.Requests.Count.ShouldBeEquivalentTo(0);
        }

        [Test]
        public void HandlePublishedNewRequestEvent_ValidRequest_ShouldBeAddedToTodaysRequestsCollection()
        {
            // Arrange
            viewModel.TodaysRequests.Clear();
            // Act
            viewModel.HandlePublishedNewRequestEvent(new NewRequestForQuoteEventPayload()
                {
                    NewRequestBookCode = "AB01",
                    NewRequestClient = testClient,
                    NewRequestText = "C+P 100 23Dec2020 1111.T"
                });
            // Assert
            viewModel.TodaysRequests.Count.ShouldBeEquivalentTo(1);
        }

        [Test]
        public void HandlePublishedNewRequestEvent_ValidRequest_ShouldBeAddedRequestsCollection()
        {
            // Arrange
            viewModel.Requests.Clear();
            viewModel.TodaysRequests.Clear();
            // Act
            viewModel.HandlePublishedNewRequestEvent(new NewRequestForQuoteEventPayload()
            {
                NewRequestBookCode = "AB01",
                NewRequestClient = testClient,
                NewRequestText = "C+P 100 23Dec2020 1111.T"
            });
            // Assert
            viewModel.Requests.Count.ShouldBeEquivalentTo(1, "because it should be added to the collection");
        }

        [Test]
        public void HandlePublishedNewRequestEvent_ValidRequest_DefaultAttributesShouldBeSet()
        {
            // Arrange
            viewModel.Requests.Clear();
            viewModel.TodaysRequests.Clear();
            // Act
            viewModel.HandlePublishedNewRequestEvent(new NewRequestForQuoteEventPayload()
            {
                NewRequestBookCode = "AB01",
                NewRequestClient = testClient,
                NewRequestText = "C+P 100 23Dec2020 1111.T"
            });

            // Assert
            viewModel.Requests[0].Request.Should().Be("C+P 100 23Dec2020 1111.T");
            viewModel.Requests[0].Status.Should().Be(StatusEnum.PENDING);
            viewModel.Requests[0].Identifier.Should().Be(-1);
            viewModel.Requests[0].Client.Should().Be(testClient);
            viewModel.Requests[0].TradeDate.Should().Be(DateTime.Today);
            viewModel.Requests[0].LotSize.Should().Be(100);
            viewModel.Requests[0].Multiplier.Should().Be(10);
            viewModel.Requests[0].Contracts.Should().Be(100);
            viewModel.Requests[0].NotionalFXRate.Should().Be(1);
            viewModel.Requests[0].NotionalMillions.Should().Be(1);
            viewModel.Requests[0].DayCountConvention.Should().Be(250);
            viewModel.Requests[0].PremiumSettlementFXRate.Should().Be(1);
            viewModel.Requests[0].SalesCreditFXRate.Should().Be(1);
            viewModel.Requests[0].IsOTC.Should().BeTrue();
            viewModel.Requests[0].SalesCreditPercentage.Should().Be(2);
            viewModel.Requests[0].PremiumSettlementDaysOverride.Should().Be(1);
            viewModel.Requests[0].PremiumSettlementDate.Should().Be(DateTime.Today.AddDays(viewModel.Requests[0].PremiumSettlementDaysOverride));
            viewModel.Requests[0].BookCode.Should().Be("AB01");
        }

        [Test]
        public void HandlePublishedNewRequestEvent_ValidRequest_OptionRequestParserCalled()
        {
            // Arrange
            optionRequestParserMock.Setup(orp => orp.ParseRequest(It.IsAny<String>(), It.IsAny<IRequestForQuote>()))
                .Returns(new List<OptionDetailImpl>() { new OptionDetailImpl() { MaturityDate = DateTime.Today}}).Callback(() => wasCalled = true);
            // Act
            viewModel.HandlePublishedNewRequestEvent(new NewRequestForQuoteEventPayload()
            {
                NewRequestBookCode = "AB01",
                NewRequestClient = testClient,
                NewRequestText = "C+P 100 23Dec2020 1111.T"
            });
            // Assert
            wasCalled.Should().BeTrue("because valid properties were used");
        }

        [Test]
        public void HandleGetTodaysRequestsEvent_RequestsCollectionShouldBeRepopulated()
        {
            // Arrange
            viewModel.Requests.Add(new RequestForQuoteImpl() { Identifier = 1 });
            viewModel.TodaysRequests.Add(new RequestForQuoteImpl() { Identifier = 2 });
            // Act
            viewModel.HandleGetTodaysRequestsEvent(new EmptyEventPayload());
            // Assert
            viewModel.Requests.Count.Should().Be(1, "because it was repopulated");
            viewModel.Requests[0].Identifier.Should().Be(2, "because it was repopulated");
        }

        [Test]
        public void HandlePublishedFilterRequestsEvent_NullCriteriaEventPayload_RequestsCollectionShouldBeRepopulated()
        {
            // Arrange
            viewModel.Requests.Clear();
            viewModel.TodaysRequests.Clear();
            viewModel.Requests.Add(new RequestForQuoteImpl() { Identifier = 1 });
            viewModel.TodaysRequests.Add(new RequestForQuoteImpl() { Identifier = 2 });
            // Act
            viewModel.HandlePublishedFilterRequestsEvent(new CriteriaUsageEventPayload() {Criteria = null});
            // Assert
            viewModel.Requests.Count.Should().Be(1, "because it was repopulated");
            viewModel.Requests[0].Identifier.Should().Be(2, "because it was repopulated");
        }

        [Test]
        public void HandlePublishedFilterRequestsEvent_EmptyCriteriaEventPayload_RequestsCollectionShouldBeRepopulated()
        {
            // Arrange
            viewModel.Requests.Clear();
            viewModel.TodaysRequests.Clear();
            viewModel.Requests.Add(new RequestForQuoteImpl() { Identifier = 1 });
            viewModel.TodaysRequests.Add(new RequestForQuoteImpl() { Identifier = 2 });
            // Act
            viewModel.HandlePublishedFilterRequestsEvent(new CriteriaUsageEventPayload() { Criteria = new Dictionary<string, string>() });
            // Assert
            viewModel.Requests.Count.Should().Be(1, "because it was repopulated");
            viewModel.Requests[0].Identifier.Should().Be(2, "because it was repopulated");            
        }

        [Test]
        public void HandlePublishedSearchRequestsEvent_NullCriteriaEventPayload_RequestsCollectionShouldBeRepopulated()
        {
            // Arrange
            viewModel.Requests.Clear();
            viewModel.TodaysRequests.Clear();
            viewModel.Requests.Add(new RequestForQuoteImpl() { Identifier = 1 });

            optionRequestPersistanceManagerMock.Setup(orp => orp.GetRequestMatchingAdhocCriteria(It.IsAny<ISearch>(), It.IsAny<bool>()))
            .Returns(new List<IRequestForQuote>()
                {
                    new RequestForQuoteImpl() {Identifier = 2}
                });
            
            // Act
            viewModel.HandlePublishedSearchRequestsEvent(new CriteriaUsageEventPayload() { Criteria = null });
            // Assert
            viewModel.Requests.Count.Should().Be(1, "because it was repopulated");
            viewModel.Requests[0].Identifier.Should().Be(2, "because it was repopulated");
        }

        [Test]
        public void GroupRequests_NullGroupByParameter_ArgumentExceptionThrown()
        {
            // Act
            Action act = () => viewModel.GroupRequests(null);
            // Assert
            act.ShouldThrow<ArgumentException>("because groupBy parameter cannot be null").WithMessage("groupBy", ComparisonMode.Substring);
        }

        [Test]
        public void GroupRequests_EmptyGroupByParameter_ArgumentExceptionThrown()
        {
            // Act
            Action act = () => viewModel.GroupRequests(string.Empty);
            // Assert
            act.ShouldThrow<ArgumentException>("because groupBy parameter cannot be empty").WithMessage("groupBy", ComparisonMode.Substring);
        }

        [Test]
        public void DeleteRequest_SelectedRequestShouldBeDeletedFromCollection()
        {
            // Arrange
            viewModel.Requests.Clear();            
            viewModel.SelectedRequest = new RequestForQuoteImpl() { Identifier = 3 };
            viewModel.Requests.Add(viewModel.SelectedRequest);
            // Act
            viewModel.DeleteRequest();
            // Assert
            viewModel.Requests.Should().BeEmpty("because it was deleted by the DeleteRequest method");
        }

        [Test]
        public void InvalidateRequest_SelectedRequestStatusShouldBeInavlid()
        {
            // Arrange
            viewModel.SelectedRequest = new RequestForQuoteImpl() { Identifier = 3 , Status = StatusEnum.TRADEDAWAY};
            // Act
            viewModel.InvalidateRequest();
            // Assert
            viewModel.SelectedRequest.Status.Should().Be(StatusEnum.INVALID, "because it was updated by the InvalidateRequest method");
        }

        [Test]
        public void CloneRequest_ClonedSelectedRequestAddedToRequestsCollection()
        {
            // Arrange
            viewModel.Requests.Clear();            
            viewModel.SelectedRequest = new RequestForQuoteImpl() { Identifier = 3 };
            // Act
            viewModel.CloneRequest();
            // Assert
            viewModel.Requests.Count.Should().Be(1, "because it was repopulated");
        }

        [Test]
        public void CanCalculateRequest_NotNullSelectedRequest_ShouldReturnTrue()
        {
            // Arrange
            viewModel.SelectedRequest = new RequestForQuoteImpl() { Identifier = 3 };
            // Assert
            viewModel.CanCalculateRequest().Should().BeTrue("bacause SelectedRequest is not null");
        }

        [Test]
        public void CanCalculateRequest_NotNullSelectedRequest_ShouldReturnFalse()
        {
            // Arrange
            viewModel.SelectedRequest = null;
            // Assert
            viewModel.CanCalculateRequest().Should().BeFalse("bacause SelectedRequest is null");
        }

        [Test] 
        public void ClearNewRequest_ShouldClearNewRequest()
        {
            // Arrange
            viewModel.NewRequest = "C 100 23Dec2014 1111.T";
            // Act
            viewModel.ClearNewRequest();
            // Assert
            viewModel.NewRequest.Should().BeEmpty("because it was emptied by the method ClearNewRequest");
        }

        [Test]
        public void ChangeStatusOfRequest_ShouldChangeStatus()
        {
            // Arrange
            viewModel.SelectedRequest = new RequestForQuoteImpl() { Identifier = 3, Status = StatusEnum.TRADEDAWAY};
            // Act
            viewModel.ChangeStatusOfRequest(StatusEnum.PICKEDUP);
            // Assert
            viewModel.SelectedRequest.Status.Should().Be(StatusEnum.PICKEDUP);
        }

        [Test]
        public void PickUpRequest_ShouldChangeStatusToPickedUp()
        {
            // Arrange
            viewModel.SelectedRequest = new RequestForQuoteImpl() { Identifier = 3, Status = StatusEnum.TRADEDAWAY };
            // Act
            viewModel.PickUpRequest();
            // Assert
            viewModel.SelectedRequest.Status.Should().Be(StatusEnum.PICKEDUP);
        }
    }
}
