using System;
using System.Collections.Generic;
using FluentAssertions;
using Microsoft.Practices.Prism.Events;
using Moq;
using NUnit.Framework;
using RequestForQuoteInterfacesLibrary.Enums;
using RequestForQuoteInterfacesLibrary.Events;
using RequestForQuoteInterfacesLibrary.ModelImplementations;
using RequestForQuoteInterfacesLibrary.ModelInterfaces;
using RequestForQuoteInterfacesLibrary.ServiceInterfaces;

namespace RequestForQuoteGridModuleLibrary.Test
{
    [TestFixture]
    internal class RequestForQuoteDetailsViewModelTest
    {
        private readonly Mock<IBookManager> bookManagerMock = new Mock<IBookManager>();
        private readonly Mock<IConfigurationManager> configManagerMock = new Mock<IConfigurationManager>();
        private readonly Mock<IClientManager> clientManagerMock = new Mock<IClientManager>();
        private readonly Mock<ISearchManager> searchManagerMock = new Mock<ISearchManager>();
        private readonly Mock<IUnderlyingManager> underlyingManagerMock = new Mock<IUnderlyingManager>();
        private readonly Mock<IEventAggregator> eventAggregatorMock = new Mock<IEventAggregator>();
        private readonly Mock<IChatServiceManager> chatServiceManager = new Mock<IChatServiceManager>();
        private readonly Mock<IOptionRequestPricer> optionRequestPricer = new Mock<IOptionRequestPricer>();
        private readonly Mock<IOptionRequestPersistanceManager> optionRequestPersistanceManager = new Mock<IOptionRequestPersistanceManager>();
        private readonly Mock<IRequestForQuote> request = new Mock<IRequestForQuote>();

        private readonly Mock<NewBookEvent> newBookEventMock = new Mock<NewBookEvent>();
        private readonly Mock<NewClientEvent> newClientEventMock = new Mock<NewClientEvent>();
        private readonly Mock<NewSearchEvent> newSearchEventMock = new Mock<NewSearchEvent>();
        private readonly Mock<NewUnderlyierEvent> newUnderlyierEventMock = new Mock<NewUnderlyierEvent>();        
        private readonly Mock<SearchRequestForQuoteEvent> searchRequestForQuoteEventMock = new Mock<SearchRequestForQuoteEvent>();

        private RequestForQuoteDetailsViewModel viewModel;
        //private bool wasCalled;

        private readonly IBook testBook = new BookImpl() { BookCode = "test book" };

        private readonly IClient testClient = new ClientImpl()
        {
            Identifier = 1,
            IsValid = true,
            Name = "test client",
            Tier = TierEnum.Top.ToString()
        };

        private readonly IUnderlying testUnderlying = new UnderlyingImpl()
        {
            Description = "test description",
            IsValid = true,
            RIC = "test RIC"
        };

        private readonly ISearch testSearch = new SearchImpl()
        {
            DescriptionKey = "test key",
            IsFilter = false,
            IsPrivate = false,
            Owner = "test owner",
            Criteria = new List<ISearchCriterion>(){ new SearchCriterionImpl() {ControlName = "test control name", ControlValue = "test control value"}} //TODO
        };

        [TestFixtureSetUp]
        public void TestFixtureSetUp()
        {
            eventAggregatorMock.Setup(p => p.GetEvent<NewBookEvent>()).Returns(newBookEventMock.Object);
            eventAggregatorMock.Setup(p => p.GetEvent<NewClientEvent>()).Returns(newClientEventMock.Object);
            eventAggregatorMock.Setup(p => p.GetEvent<NewSearchEvent>()).Returns(newSearchEventMock.Object);
            eventAggregatorMock.Setup(p => p.GetEvent<NewUnderlyierEvent>()).Returns(newUnderlyierEventMock.Object);
            eventAggregatorMock.Setup(p => p.GetEvent<SearchRequestForQuoteEvent>()).Returns(searchRequestForQuoteEventMock.Object);

            bookManagerMock.Setup(bm => bm.Books).Returns(new List<IBook>() {testBook});
            clientManagerMock.Setup(cm => cm.Clients).Returns(new List<IClient>() {testClient});
            underlyingManagerMock.Setup(um => um.Underlyings).Returns(new List<IUnderlying>() { testUnderlying });
            searchManagerMock.Setup(sm => sm.Searches).Returns(new List<ISearch>() {testSearch});
        }

        [SetUp]
        public void SetUpBeforeEachAndEveryTest()
        {
            //wasCalled = false;
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
        public void Constructor_NullEventAggregator_ArgumentNullExceptionThrown()
        {
            // Act
            Action act = () => viewModel = new RequestForQuoteDetailsViewModel(optionRequestPricer.Object, request.Object, clientManagerMock.Object,
                bookManagerMock.Object, null, underlyingManagerMock.Object, chatServiceManager.Object,
                optionRequestPersistanceManager.Object, configManagerMock.Object);
            // Assert
            act.ShouldThrow<ArgumentNullException>("because event aggregator parameter cannot be null.").WithMessage("eventAggregator", ComparisonMode.Substring);
        }

        [Test]
        public void Constructor_NullConfigManager_ArgumentNullExceptionThrown()
        {
            // Act
            Action act = () => viewModel = new RequestForQuoteDetailsViewModel(optionRequestPricer.Object, request.Object, clientManagerMock.Object,
                bookManagerMock.Object, eventAggregatorMock.Object, underlyingManagerMock.Object, chatServiceManager.Object,
                optionRequestPersistanceManager.Object, null);
            // Assert
            act.ShouldThrow<ArgumentNullException>("because configManager parameter cannot be null").WithMessage("configManager", ComparisonMode.Substring);
        }

        [Test]
        public void Constructor_NullOptionRequestPricer_ArgumentNullExceptionThrown()
        {
            // Act
            Action act = () => viewModel = new RequestForQuoteDetailsViewModel(null, request.Object, clientManagerMock.Object,
                bookManagerMock.Object, eventAggregatorMock.Object, underlyingManagerMock.Object, chatServiceManager.Object,
                optionRequestPersistanceManager.Object, configManagerMock.Object);
            // Assert
            act.ShouldThrow<ArgumentNullException>("because option request pricer parameter cannot be null").WithMessage("optionRequestPricer", ComparisonMode.Substring);
        }

        [Test]
        public void Constructor_NullRequestForQuote_ArgumentNullExceptionThrown()
        {
            // Act
            Action act = () => viewModel = new RequestForQuoteDetailsViewModel(optionRequestPricer.Object, null, clientManagerMock.Object,
                bookManagerMock.Object, eventAggregatorMock.Object, underlyingManagerMock.Object, chatServiceManager.Object,
                optionRequestPersistanceManager.Object, configManagerMock.Object);
            // Assert
            act.ShouldThrow<ArgumentNullException>("because request for quote parameter cannot be null").WithMessage("requestForQuote", ComparisonMode.Substring);
        }

        [Test]
        public void Constructor_NullClientManager_ArgumentNullExceptionThrown()
        {
            // Act
            Action act = () => viewModel = new RequestForQuoteDetailsViewModel(optionRequestPricer.Object, request.Object, null,
                bookManagerMock.Object, eventAggregatorMock.Object, underlyingManagerMock.Object, chatServiceManager.Object,
                optionRequestPersistanceManager.Object, configManagerMock.Object);
            // Assert
            act.ShouldThrow<ArgumentNullException>("because clientManager parameter cannot be null").WithMessage("clientManager", ComparisonMode.Substring);
        }

        [Test]
        public void Constructor_NullBookManager_ArgumentNullExceptionThrown()
        {
            // Act
            Action act = () => viewModel = new RequestForQuoteDetailsViewModel(optionRequestPricer.Object, request.Object, clientManagerMock.Object,
                null, eventAggregatorMock.Object, underlyingManagerMock.Object, chatServiceManager.Object,
                optionRequestPersistanceManager.Object, configManagerMock.Object);
            // Assert
            act.ShouldThrow<ArgumentNullException>("because book manager parameter cannot be null").WithMessage("bookManager", ComparisonMode.Substring);
        }

        [Test]
        public void Constructor_NullChatServiceManager_ArgumentNullExceptionThrown()
        {
            // Act
            Action act = () => viewModel = new RequestForQuoteDetailsViewModel(optionRequestPricer.Object, request.Object, clientManagerMock.Object,
                bookManagerMock.Object, eventAggregatorMock.Object, underlyingManagerMock.Object, null,
                optionRequestPersistanceManager.Object, configManagerMock.Object);
            // Assert
            act.ShouldThrow<ArgumentNullException>("because chat service manager parameter cannot be null").WithMessage("chatServiceManager", ComparisonMode.Substring);
        }

        [Test]
        public void Constructor_NullUnderlyingManager_ArgumentNullExceptionThrown()
        {
            // Act
            Action act = () => viewModel = new RequestForQuoteDetailsViewModel(optionRequestPricer.Object, request.Object, clientManagerMock.Object,
                bookManagerMock.Object, eventAggregatorMock.Object, null, chatServiceManager.Object,
                optionRequestPersistanceManager.Object, configManagerMock.Object);
            // Assert
            act.ShouldThrow<ArgumentNullException>("because underlying manager parameter cannot be null").WithMessage("underlyingManager", ComparisonMode.Substring);
        }

        [Test]
        public void Constructor_NullOptionRequestPersistanceManager_ArgumentNullExceptionThrown()
        {
            // Act
            Action act = () => viewModel = new RequestForQuoteDetailsViewModel(optionRequestPricer.Object, request.Object, clientManagerMock.Object,
                bookManagerMock.Object, eventAggregatorMock.Object, underlyingManagerMock.Object, chatServiceManager.Object,
                null, configManagerMock.Object);
            // Assert
            act.ShouldThrow<ArgumentNullException>("because option request persistance manager parameter cannot be null").WithMessage("optionRequestPersistanceManager", ComparisonMode.Substring);
        }

        [Test]
        public void HandleNewBookEvent_NullNewBookEventPayload_ArgumentNullExceptionThrown()
        {
            // TODO
        }
    }
}
