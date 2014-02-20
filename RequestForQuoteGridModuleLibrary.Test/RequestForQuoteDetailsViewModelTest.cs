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
    [TestFixture]
    internal class RequestForQuoteDetailsViewModelTest
    {
        private readonly Mock<IBookManager> bookManagerMock = new Mock<IBookManager>();
        private readonly Mock<IConfigurationManager> configManagerMock = new Mock<IConfigurationManager>();
        private readonly Mock<IClientManager> clientManagerMock = new Mock<IClientManager>();
        private readonly Mock<ISearchManager> searchManagerMock = new Mock<ISearchManager>();
        private readonly Mock<IUserManager> userManagerMock = new Mock<IUserManager>();
        private readonly Mock<IUnderlyingManager> underlyingManagerMock = new Mock<IUnderlyingManager>();
        private readonly Mock<IEventAggregator> eventAggregatorMock = new Mock<IEventAggregator>();
        private readonly Mock<IChatServiceManager> chatServiceManagerMock = new Mock<IChatServiceManager>();
        private readonly Mock<IOptionRequestPricer> optionRequestPricerMock = new Mock<IOptionRequestPricer>();
        private readonly Mock<IOptionRequestPersistanceManager> optionRequestPersistanceManagerMock = new Mock<IOptionRequestPersistanceManager>();
        private readonly IRequestForQuote requestMock = new RequestForQuoteImpl() {Identifier = -1};

        private readonly Mock<NewBookEvent> newBookEventMock = new Mock<NewBookEvent>();
        private readonly Mock<NewClientEvent> newClientEventMock = new Mock<NewClientEvent>();
        private readonly Mock<NewSearchEvent> newSearchEventMock = new Mock<NewSearchEvent>();
        private readonly Mock<NewUnderlyierEvent> newUnderlyierEventMock = new Mock<NewUnderlyierEvent>();        
        private readonly Mock<SearchRequestForQuoteEvent> searchRequestForQuoteEventMock = new Mock<SearchRequestForQuoteEvent>();
        private readonly Mock<NewChatMessageEvent> newChatMessageEventMock = new Mock<NewChatMessageEvent>();
        private readonly Mock<NewUserEvent> newUserMessageEventMock = new Mock<NewUserEvent>();

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

        private readonly IUser testUser = new UserImpl()
        {
            UserId = "leon.adeoye",
            EmailAddress = "leon.o.adeoye@jpmchase.com",
            FirstName = "Leon",
            LastName = "Adeoye",
            IsValid = true,
            GroupId = 1,
            LocationName = LocationEnum.HONG_KONG
        };

        [TestFixtureSetUp]
        public void TestFixtureSetUp()
        {
            eventAggregatorMock.Setup(p => p.GetEvent<NewBookEvent>()).Returns(newBookEventMock.Object);
            eventAggregatorMock.Setup(p => p.GetEvent<NewClientEvent>()).Returns(newClientEventMock.Object);
            eventAggregatorMock.Setup(p => p.GetEvent<NewSearchEvent>()).Returns(newSearchEventMock.Object);
            eventAggregatorMock.Setup(p => p.GetEvent<NewUnderlyierEvent>()).Returns(newUnderlyierEventMock.Object);
            eventAggregatorMock.Setup(p => p.GetEvent<SearchRequestForQuoteEvent>()).Returns(searchRequestForQuoteEventMock.Object);
            eventAggregatorMock.Setup(p => p.GetEvent<NewChatMessageEvent>()).Returns(newChatMessageEventMock.Object);
            eventAggregatorMock.Setup(p => p.GetEvent<NewUserEvent>()).Returns(newUserMessageEventMock.Object);

            bookManagerMock.Setup(bm => bm.Books).Returns(new List<IBook>() {testBook});
            clientManagerMock.Setup(cm => cm.Clients).Returns(new List<IClient>() {testClient});
            underlyingManagerMock.Setup(um => um.Underlyings).Returns(new List<IUnderlying>() { testUnderlying });
            searchManagerMock.Setup(sm => sm.Searches).Returns(new List<ISearch>() {testSearch});
            userManagerMock.Setup(um => um.Users).Returns(new List<IUser>() { testUser });

            viewModel = new RequestForQuoteDetailsViewModel(optionRequestPricerMock.Object, requestMock, 
                clientManagerMock.Object, bookManagerMock.Object, eventAggregatorMock.Object, underlyingManagerMock.Object, 
                chatServiceManagerMock.Object, optionRequestPersistanceManagerMock.Object,configManagerMock.Object, userManagerMock.Object);
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
            Action act = () => viewModel = new RequestForQuoteDetailsViewModel(optionRequestPricerMock.Object, requestMock, clientManagerMock.Object,
                bookManagerMock.Object, null, underlyingManagerMock.Object, chatServiceManagerMock.Object,
                optionRequestPersistanceManagerMock.Object, configManagerMock.Object, userManagerMock.Object);
            // Assert
            act.ShouldThrow<ArgumentNullException>("because event aggregator parameter cannot be null.").WithMessage("eventAggregator", ComparisonMode.Substring);
        }

        [Test]
        public void Constructor_NullConfigManager_ArgumentNullExceptionThrown()
        {
            // Act
            Action act = () => viewModel = new RequestForQuoteDetailsViewModel(optionRequestPricerMock.Object, requestMock, clientManagerMock.Object,
                bookManagerMock.Object, eventAggregatorMock.Object, underlyingManagerMock.Object, chatServiceManagerMock.Object,
                optionRequestPersistanceManagerMock.Object, null, userManagerMock.Object);
            // Assert
            act.ShouldThrow<ArgumentNullException>("because configManager parameter cannot be null").WithMessage("configManager", ComparisonMode.Substring);
        }

        [Test]
        public void Constructor_NullOptionRequestPricer_ArgumentNullExceptionThrown()
        {
            // Act
            Action act = () => viewModel = new RequestForQuoteDetailsViewModel(null, requestMock, clientManagerMock.Object,
                bookManagerMock.Object, eventAggregatorMock.Object, underlyingManagerMock.Object, chatServiceManagerMock.Object,
                optionRequestPersistanceManagerMock.Object, configManagerMock.Object, userManagerMock.Object);
            // Assert
            act.ShouldThrow<ArgumentNullException>("because option request pricer parameter cannot be null").WithMessage("optionRequestPricer", ComparisonMode.Substring);
        }

        [Test]
        public void Constructor_NullRequestForQuote_ArgumentNullExceptionThrown()
        {
            // Act
            Action act = () => viewModel = new RequestForQuoteDetailsViewModel(optionRequestPricerMock.Object, null, clientManagerMock.Object,
                bookManagerMock.Object, eventAggregatorMock.Object, underlyingManagerMock.Object, chatServiceManagerMock.Object,
                optionRequestPersistanceManagerMock.Object, configManagerMock.Object, userManagerMock.Object);
            // Assert
            act.ShouldThrow<ArgumentNullException>("because request for quote parameter cannot be null").WithMessage("requestForQuote", ComparisonMode.Substring);
        }

        [Test]
        public void Constructor_NullClientManager_ArgumentNullExceptionThrown()
        {
            // Act
            Action act = () => viewModel = new RequestForQuoteDetailsViewModel(optionRequestPricerMock.Object, requestMock, null,
                bookManagerMock.Object, eventAggregatorMock.Object, underlyingManagerMock.Object, chatServiceManagerMock.Object,
                optionRequestPersistanceManagerMock.Object, configManagerMock.Object, userManagerMock.Object);
            // Assert
            act.ShouldThrow<ArgumentNullException>("because clientManager parameter cannot be null").WithMessage("clientManager", ComparisonMode.Substring);
        }

        [Test]
        public void Constructor_NullBookManager_ArgumentNullExceptionThrown()
        {
            // Act
            Action act = () => viewModel = new RequestForQuoteDetailsViewModel(optionRequestPricerMock.Object, requestMock, clientManagerMock.Object,
                null, eventAggregatorMock.Object, underlyingManagerMock.Object, chatServiceManagerMock.Object,
                optionRequestPersistanceManagerMock.Object, configManagerMock.Object, userManagerMock.Object);
            // Assert
            act.ShouldThrow<ArgumentNullException>("because book manager parameter cannot be null").WithMessage("bookManager", ComparisonMode.Substring);
        }

        [Test]
        public void Constructor_NullChatServiceManager_ArgumentNullExceptionThrown()
        {
            // Act
            Action act = () => viewModel = new RequestForQuoteDetailsViewModel(optionRequestPricerMock.Object, requestMock, clientManagerMock.Object,
                bookManagerMock.Object, eventAggregatorMock.Object, underlyingManagerMock.Object, null,
                optionRequestPersistanceManagerMock.Object, configManagerMock.Object, userManagerMock.Object);
            // Assert
            act.ShouldThrow<ArgumentNullException>("because chat service manager parameter cannot be null").WithMessage("chatServiceManager", ComparisonMode.Substring);
        }

        [Test]
        public void Constructor_NullUnderlyingManager_ArgumentNullExceptionThrown()
        {
            // Act
            Action act = () => viewModel = new RequestForQuoteDetailsViewModel(optionRequestPricerMock.Object, requestMock, clientManagerMock.Object,
                bookManagerMock.Object, eventAggregatorMock.Object, null, chatServiceManagerMock.Object,
                optionRequestPersistanceManagerMock.Object, configManagerMock.Object, userManagerMock.Object);
            // Assert
            act.ShouldThrow<ArgumentNullException>("because underlying manager parameter cannot be null").WithMessage("underlyingManager", ComparisonMode.Substring);
        }

        [Test]
        public void Constructor_NullOptionRequestPersistanceManager_ArgumentNullExceptionThrown()
        {
            // Act
            Action act = () => viewModel = new RequestForQuoteDetailsViewModel(optionRequestPricerMock.Object, requestMock, clientManagerMock.Object,
                bookManagerMock.Object, eventAggregatorMock.Object, underlyingManagerMock.Object, chatServiceManagerMock.Object,
                null, configManagerMock.Object, userManagerMock.Object);
            // Assert
            act.ShouldThrow<ArgumentNullException>("because option request persistance manager parameter cannot be null").WithMessage("optionRequestPersistanceManager", ComparisonMode.Substring);
        }

        [Test]
        public void HandleNewBookEvent_NullNewBookEventPayload_ArgumentNullExceptionThrown()
        {
            // Act
            Action act = () => viewModel.HandleNewBookEvent(null);
            // Assert
            act.ShouldThrow<ArgumentNullException>("because eventPayload parameter cannot be null").WithMessage("eventPayload", ComparisonMode.Substring);
        }

        [Test]
        public void HandleNewClientEvent_NullNewClientEventPayload_ArgumentNullExceptionThrown()
        {
            // Act
            Action act = () => viewModel.HandleNewClientEvent(null);
            // Assert
            act.ShouldThrow<ArgumentNullException>("because eventPayload parameter cannot be null").WithMessage("eventPayload", ComparisonMode.Substring);
        }

        [Test]
        public void HandleNewUnderlyierEvent_NullNewUnderlyierEventPayload_ArgumentNullExceptionThrown()
        {
            // Act
            Action act = () => viewModel.HandleNewUnderlyierEvent(null);
            // Assert
            act.ShouldThrow<ArgumentNullException>("because eventPayload parameter cannot be null").WithMessage("eventPayload", ComparisonMode.Substring);
        }

        [Test]
        public void HandleNewChatMessageEvent_NullNewChatMessageEventPayload_ArgumentNullExceptionThrown()
        {
            // Act
            Action act = () => viewModel.HandleNewChatMessageEvent(null);
            // Assert
            act.ShouldThrow<ArgumentNullException>("because eventPayload parameter cannot be null").WithMessage("eventPayload", ComparisonMode.Substring);
        }

        [Test]
        public void HandleNewChatMessageEvent_NewChatMessageWithMatchingIdentifier_ChatMessagesAddToCollection()
        {
            // Arrange
            viewModel.ChatMessages.Clear();
            viewModel.SelectedRequestForQuote = new RequestForQuoteImpl() {Identifier = 5};
            // Act
            viewModel.HandleNewChatMessageEvent(new NewChatMessageEventPayload() {NewChatMessage = new ChatMessageImpl() {RequestForQuoteId = 5}});
            // Assert
            viewModel.ChatMessages.Count.Should()
                     .Be(1, "because the  identifier in the chat messages matches the selected RFQ.");
        }

        [Test]
        public void HandleNewChatMessageEvent_NewChatMessageWithMismatchingIdentifier_ChatMessagesNotAddToCollection()
        {
            // Arrange
            viewModel.ChatMessages.Clear();
            viewModel.SelectedRequestForQuote = new RequestForQuoteImpl() { Identifier = 5 };
            // Act
            viewModel.HandleNewChatMessageEvent(new NewChatMessageEventPayload() { NewChatMessage = new ChatMessageImpl() { RequestForQuoteId = 6 } });
            // Assert
            viewModel.ChatMessages.Count.Should()
                     .Be(0, "because the  identifier in the chat messages does not match the selected RFQ.");
        }

        [Test]
        public void SendChatMessage_EmptyChatMessage_ChatServiceSendChatMessageNeverCalled()
        {
            // Arrange
            var wasCalled = false;
            viewModel.MessageToBeSent = String.Empty;
            chatServiceManagerMock.Setup(orp => orp.SendChatMessage(It.IsAny<Int32>(), It.IsAny<String>(), It.IsAny<String>()))
                .Callback(() => wasCalled = true);
            // Act
            viewModel.SendChatMessage();
            // Assert
            wasCalled.Should().BeFalse("because the message to be sent is empty");
        }

        [Test]
        public void SendChatMessage_NullChatMessage_ChatServiceSendChatMessageNeverCalled()
        {
            // Arrange
            var wasCalled = false;
            viewModel.MessageToBeSent = null;
            chatServiceManagerMock.Setup(orp => orp.SendChatMessage(It.IsAny<Int32>(), It.IsAny<String>(), It.IsAny<String>()))
                .Callback(() => wasCalled = true);
            // Act
            viewModel.SendChatMessage();
            // Assert
            wasCalled.Should().BeFalse("because the message to be sent is null");
        }

        [Test]
        public void SendChatMessage_ValidChatMessage_ChatServiceSendChatMessageCalled()
        {
            // Arrange
            var wasCalled = false;
            viewModel.MessageToBeSent = "test message";
            chatServiceManagerMock.Setup(csm => csm.SendChatMessage(It.IsAny<Int32>(), It.IsAny<String>(), It.IsAny<String>()))
                .Callback(() => wasCalled = true);
            // Act
            viewModel.SendChatMessage();
            // Assert
            wasCalled.Should().BeTrue("because the message to be sent is valid");
        }

        [Test]
        public void SendChatMessage_ValidChatMessage_MessageToBeSentCleared()
        {
            // Arrange
            viewModel.MessageToBeSent = "test message";
            // Act
            viewModel.SendChatMessage();
            // Assert
            viewModel.MessageToBeSent.Should().BeNullOrEmpty("because it is cleared after message is sent");
        }

        [Test]
        public void CanCalculateRequest_ValidSelectedRequestForQuote_ReturnsTrue()
        {
            // Arrange
            viewModel.SelectedRequestForQuote = new RequestForQuoteImpl();
            // Assert
            viewModel.CanCalculateRequest().Should().BeTrue("because the selected RFQ is not null");
        }

        [Test]
        public void CanCalculateRequest_NullSelectedRequestForQuote_ReturnsFalse()
        {
            // Arrange
            viewModel.SelectedRequestForQuote = null;
            // Assert
            viewModel.CanCalculateRequest().Should().BeFalse("because the selected RFQ is null");
        }

        [Test]
        public void CanSave_ValidSelectedRequestForQuote_ReturnsTrue()
        {
            // Arrange
            viewModel.SelectedRequestForQuote = new RequestForQuoteImpl();
            // Assert
            viewModel.CanSave(string.Empty).Should().BeTrue("because the selected RFQ is not null");
        }

        [Test]
        public void CanSave_NullSelectedRequestForQuote_ReturnsFalse()
        {
            // Arrange
            viewModel.SelectedRequestForQuote = null;
            // Assert
            viewModel.CanSave(string.Empty).Should().BeFalse("because the selected RFQ is null");
        }

        [Test]
        public void BeginEdit_ValidSelectedRequestForQuote_ClonedToBackupOfRequestForQuote()
        {
            // Arrange
            viewModel.backupOfRequestForQuote = null;
            viewModel.SelectedRequestForQuote = new RequestForQuoteImpl() {Identifier = 1000};
            // Act
            viewModel.BeginEdit();
            // Assert
            viewModel.backupOfRequestForQuote.Identifier.Should().Be(1000, "because it is cloned by BeginEdit method");
        }

        [Test]
        public void EndEdit_ValidSelectedRequestForQuoteWithNegativeIdentifier_CallsSaveRequest()
        {
            // Arrange
            var wasCalled = false;
            viewModel.SelectedRequestForQuote = new RequestForQuoteImpl() { Identifier = -1};
            optionRequestPersistanceManagerMock.Setup(orpm => orpm.SaveRequest(It.IsAny<IRequestForQuote>())).Callback(() => wasCalled = true);
            // Act
            viewModel.EndEdit();
            // Assert
            wasCalled.Should().BeTrue("because SaveRequest was called by EndEdit");
        }

        [Test]
        public void EndEdit_ValidSelectedRequestForQuoteWithNegativeIdentifier_SelectedRequestForQuoteIdentifierUpdated()
        {
            // Arrange
            viewModel.SelectedRequestForQuote = new RequestForQuoteImpl() { Identifier = -1 };
            optionRequestPersistanceManagerMock.Setup(orpm => orpm.SaveRequest(It.IsAny<IRequestForQuote>())).Returns(999);
            // Act
            viewModel.EndEdit();
            // Assert
            viewModel.SelectedRequestForQuote.Identifier.Should().Be(999,"because SaveRequest returns the new identifier and sets SelectedRequestForQuote with it");
        }

        [Test]
        public void EndEdit_ValidSelectedRequestForQuoteWithNegativeIdentifier_RegisterParticipantShouldBeCalled()
        {
            // Arrange
            var wasCalled = false;
            viewModel.SelectedRequestForQuote = new RequestForQuoteImpl() { Identifier = -1 };
            optionRequestPersistanceManagerMock.Setup(orpm => orpm.SaveRequest(It.IsAny<IRequestForQuote>())).Returns(999);
            chatServiceManagerMock.Setup(csm => csm.RegisterParticipant(It.IsAny<Int32>())).Callback(() => wasCalled = true);
            // Act
            viewModel.EndEdit();
            // Assert
            wasCalled.Should().BeTrue("because if SaveRequest returns a new identifier then RegisterParticipant should be called");
        }

        [Test]
        public void EndEdit_ValidSelectedRequestForQuoteWithNonNegativeIdentifier_RegisterParticipantShouldBeCalled()
        {
            // Arrange
            var wasCalled = false;
            viewModel.SelectedRequestForQuote = new RequestForQuoteImpl() { Identifier = 999 };
            optionRequestPersistanceManagerMock.Setup(orpm => orpm.UpdateRequest(It.IsAny<IRequestForQuote>())).Returns(true).Callback(() => wasCalled = true);
            // Act
            viewModel.EndEdit();
            // Assert
            wasCalled.Should().BeTrue("because Identifier is non-negative then UpdateRequest should be called");
        }

        [Test]
        public void CancelEdit_ValidSelectedRequestForQuote_ClonedToBackupOfRequestForQuote()
        {
            // Arrange
            viewModel.backupOfRequestForQuote = new RequestForQuoteImpl()
            {
                Identifier = 1111,
                Status = StatusEnum.TRADEDAWAY,
                TradeDate = DateTime.Today,
                ExpiryDate = DateTime.Today,
                HedgeType = HedgeTypeEnum.SHARES
            };
            viewModel.SelectedRequestForQuote = new RequestForQuoteImpl() { Identifier = 1000 }; ;
            // Act
            viewModel.CancelEdit();
            // Assert
            viewModel.SelectedRequestForQuote.Identifier.Should().Be(1111, "because of the memberwise copy by CancelEdit method");
        }
    }
}
