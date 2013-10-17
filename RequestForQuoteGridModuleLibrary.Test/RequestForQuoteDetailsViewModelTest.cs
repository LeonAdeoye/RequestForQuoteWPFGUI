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

        private readonly Mock<SearchRequestForQuoteEvent> searchRequestForQuoteEventMock =
            new Mock<SearchRequestForQuoteEvent>();

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

        private readonly IUnderlyier testUnderlyier = new UnderlyierImpl()
        {
            BBG = "test BBG",
            Description = "test description",
            IsValid = true,
            RIC = "test RIC"
        };

        private readonly ISearch testSearch = new SearchImpl()
        {
            Criteria = new Dictionary<string, string>(),
            DescriptionKey = "test key",
            IsFilter = false,
            IsPrivate = false,
            Owner = "test owner"
        };

        [TestFixtureSetUp]
        public void TestFixtureSetUp()
        {
            eventAggregatorMock.Setup(p => p.GetEvent<NewBookEvent>()).Returns(newBookEventMock.Object);
            eventAggregatorMock.Setup(p => p.GetEvent<NewClientEvent>()).Returns(newClientEventMock.Object);
            eventAggregatorMock.Setup(p => p.GetEvent<NewSearchEvent>()).Returns(newSearchEventMock.Object);
            eventAggregatorMock.Setup(p => p.GetEvent<NewUnderlyierEvent>()).Returns(newUnderlyierEventMock.Object);
            eventAggregatorMock.Setup(p => p.GetEvent<SearchRequestForQuoteEvent>())
                               .Returns(searchRequestForQuoteEventMock.Object);

            bookManagerMock.Setup(bm => bm.Books).Returns(new List<IBook>() {testBook});
            clientManagerMock.Setup(cm => cm.Clients).Returns(new List<IClient>() {testClient});
            underlyingManagerMock.Setup(um => um.Underlyiers).Returns(new List<IUnderlyier>() {testUnderlyier});
            searchManagerMock.Setup(sm => sm.Searches).Returns(new List<ISearch>() {testSearch});

            viewModel = new RequestForQuoteDetailsViewModel(optionRequestPricer.Object, request.Object, clientManagerMock.Object,
                bookManagerMock.Object, eventAggregatorMock.Object, underlyingManagerMock.Object, chatServiceManager.Object, optionRequestPersistanceManager.Object);
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
        public void ctor__CommandsShouldNotBeNull()
        {
            // Arrange

            // Act

            // Assert
            viewModel.SaveRequestCommand.Should().NotBeNull("because it was instantiated by the constructor");
            viewModel.SendChatMessageCommand.Should().NotBeNull("because it was instantiated by the constructor");
        }
    }
}
