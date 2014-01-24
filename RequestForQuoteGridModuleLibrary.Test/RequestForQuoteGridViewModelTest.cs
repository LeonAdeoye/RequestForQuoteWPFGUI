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
    }
}
