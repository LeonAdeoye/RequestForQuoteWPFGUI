using System;
using System.Collections.Generic;
using Microsoft.Practices.Prism.Events;
using Moq;
using NUnit.Framework;
using RequestForQuoteInterfacesLibrary.EventPayloads;
using RequestForQuoteInterfacesLibrary.Events;
using RequestForQuoteInterfacesLibrary.ModelImplementations;
using RequestForQuoteInterfacesLibrary.ModelInterfaces;
using RequestForQuoteInterfacesLibrary.ServiceInterfaces;

namespace RequestForQuoteFunctionsModuleLibrary.Test
{
    [TestFixture]
    class RequestForQuoteFunctionsViewModelTests
    {
        private readonly Mock<IBookManager> bookManagerMock = new Mock<IBookManager>();
        private readonly Mock<IClientManager> clientManagerMock = new Mock<IClientManager>();
        private readonly Mock<ISearchManager> searchManagerMock = new Mock<ISearchManager>();
        private readonly Mock<IUnderlyingManager> underlyingManagerMock = new Mock<IUnderlyingManager>();
        private readonly Mock<IEventAggregator> eventAggregaterMock = new Mock<IEventAggregator>();
        
        private readonly Mock<NewBookEvent> newBookEventMock = new Mock<NewBookEvent>();
        private readonly Mock<NewClientEvent> newClientEventMock = new Mock<NewClientEvent>();
        private readonly Mock<NewSearchEvent> newSearchEventMock = new Mock<NewSearchEvent>();
        private readonly Mock<NewUnderlyierEvent> newUnderlyierEventMock = new Mock<NewUnderlyierEvent>();
        private readonly Mock<SearchRequestForQuoteEvent> searchRequestForQuoteEventMock = new Mock<SearchRequestForQuoteEvent>();

        private readonly IBook testBook = new BookImpl() {BookCode = "test book"};
        private readonly IClient testClient = new ClientImpl() {Identifier = 1, IsValid = true, Name ="test client", Tier = 1};
        private readonly IUnderlyier testUnderlyier = new UnderlyierImpl() {BBG = "test BBG", Description = "test description", IsValid = true, RIC = "test RIC"};
        private readonly ISearch testSearch = new SearchImpl() { Criteria = new Dictionary<string, string>(), DescriptionKey = "test key", IsFilter = false, IsPrivate = false, Owner = "test owner" };

        private RequestForQuoteFunctionsViewModel viewModel;
        private bool wasCalled;

        [TestFixtureSetUp]
        public void TestFixtureSetUp()
        {
            eventAggregaterMock.Setup(p => p.GetEvent<NewBookEvent>()).Returns(newBookEventMock.Object);
            eventAggregaterMock.Setup(p => p.GetEvent<NewClientEvent>()).Returns(newClientEventMock.Object);
            eventAggregaterMock.Setup(p => p.GetEvent<NewSearchEvent>()).Returns(newSearchEventMock.Object);
            eventAggregaterMock.Setup(p => p.GetEvent<NewUnderlyierEvent>()).Returns(newUnderlyierEventMock.Object);
            eventAggregaterMock.Setup(p => p.GetEvent<SearchRequestForQuoteEvent>()).Returns(searchRequestForQuoteEventMock.Object);

            bookManagerMock.Setup(bm => bm.Books).Returns(new List<IBook>() { testBook });
            clientManagerMock.Setup(cm => cm.Clients).Returns(new List<IClient>() { testClient });
            underlyingManagerMock.Setup(um => um.Underlyiers).Returns(new List<IUnderlyier>() { testUnderlyier});
            searchManagerMock.Setup(sm => sm.Searches).Returns(new List<ISearch>() { testSearch });

            viewModel = new RequestForQuoteFunctionsViewModel(eventAggregaterMock.Object, clientManagerMock.Object, 
                underlyingManagerMock.Object, bookManagerMock.Object, searchManagerMock.Object);
        }

        [SetUp]
        public void SetUpBeforeEachAndEveryTest()
        {
            wasCalled = false;
        }

        [TearDown]
        public void TearDown()
        {
            viewModel.ClearCriteria();
        }

        [TestFixtureTearDown]
        public void TestFixtureTearDown()
        {
            viewModel = null;
        }

        [Test]
        public void SelectedBook_PropertySet_BookCriteriaAdded()
        {
            viewModel.SelectedBook = testBook;
            Assert.IsNotEmpty(viewModel.Criteria, "After setting SelectedBook property the criteria collection should not be empty!");
        }

        [Test]
        public void ClearCriteria_SearchRFQEventShouldBePublished()
        {
            viewModel.SelectedBook = testBook;
            searchRequestForQuoteEventMock.Setup(s => s.Publish(It.IsAny<CriteriaUsageEventPayload>())).Callback(() => wasCalled = true);
            viewModel.ClearCriteria();
            Assert.IsTrue(wasCalled, "After ClearCriteria method is called, SearchRequestForQuoteEvent event is NOT published!");
        }

        [Test]
        public void ClearCriteria_CriteriaCollectionShouldEmptied()
        {
            viewModel.SelectedBook = testBook;            
            viewModel.ClearCriteria();
            Assert.IsEmpty(viewModel.Criteria, "After ClearCriteria method is called, Criteria is NOT empty!");            
        }

        [Test]
        public void ClearCriteria_SelectedBooksShouldBeNull()
        {
            viewModel.SelectedBook = testBook;
            viewModel.ClearCriteria();
            Assert.IsNull(viewModel.SelectedBook, "After ClearCriteria method is called, SelectedBook is NOT Null!");
        }

        [Test]
        public void ctor_BooksCollectionShouldBePopulated()
        {
            Assert.IsNotEmpty(viewModel.Books, "view model ctor did NOT populate Books property with IBookManager instances' books!");
        }

        [Test]
        public void ctor_ClientsCollectionShouldBePopulated()
        {
            Assert.IsNotEmpty(viewModel.Clients, "view model ctor did NOT populate Clients property with IClientManager instances' clients!");
        }

        [Test]
        public void ctor_UnderlyiersCollectionShouldBePopulated()
        {
            Assert.IsNotEmpty(viewModel.Underlyiers, "view model ctor did NOT populate Underlyiers property with IUnderlyierManager instances' underlyiers!");
        }

        [Test]
        public void ctor_SearchesCollectionShouldBePopulated()
        {
            Assert.IsNotEmpty(viewModel.Searches, "view model ctor did NOT populate Seacrhes property with ISearchManager instances' searches!");
        }

        [Test]
        public void ctor_NewBookEventShouldBeSubscribedTo()
        {
            newBookEventMock.Verify(bm => bm.Subscribe(It.IsAny<Action<NewBookEventPayload>>(), It.IsAny<ThreadOption>(), 
                It.IsAny<bool>(), It.IsAny<Predicate<NewBookEventPayload>>()), Times.Once(), "view model ctor did not subscribe to new book event!");
        }

        [Test]
        public void ctor_NewClientEventShouldBeSubscribedTo()
        {
            newClientEventMock.Verify(cm => cm.Subscribe(It.IsAny<Action<NewClientEventPayload>>(), It.IsAny<ThreadOption>(),
                It.IsAny<bool>(), It.IsAny<Predicate<NewClientEventPayload>>()), Times.Once(), "view model ctor did not subscribe to new client event!");
        }

        [Test]
        public void ctor_NewUnderlyierEventShouldBeSubscribedTo()
        {
            newUnderlyierEventMock.Verify(um => um.Subscribe(It.IsAny<Action<NewUnderlyierEventPayload>>(), It.IsAny<ThreadOption>(),
                It.IsAny<bool>(), It.IsAny<Predicate<NewUnderlyierEventPayload>>()), Times.Once(), "view model ctor did not subscribe to new underlyier event!");
        }

        [Test]
        public void ctor_NewSearchEventShouldBeSubscribedTo()
        {
            newSearchEventMock.Verify(um => um.Subscribe(It.IsAny<Action<NewSearchEventPayload>>(), It.IsAny<ThreadOption>(),
                It.IsAny<bool>(), It.IsAny<Predicate<NewSearchEventPayload>>()), Times.Once(), "view model ctor did not subscribe to new search event!");
        }

        [Test]
        public void SaveSearch_WithNonEmptyCriteriaAndCriteriaDescriptionKeyPropertySet_SaveSearchMethodOfISearchManagerInstanceShouldBeCalled()
        {
            searchManagerMock.Setup(sm => sm.SaveSearch(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<bool>(), 
                It.IsAny<bool>(), It.IsAny<IDictionary<string, string>>())).Callback(() => wasCalled = true);

            viewModel.SelectedBook = testBook;
            viewModel.CriteriaDescriptionKey = "test Criteria";
            viewModel.SaveSearch();

            Assert.IsTrue(wasCalled, "Saving newly added book criteria does NOT call SaveSearch method of ISearchManager instance!");
        }

        [Test]
        public void SaveSearch_CriteriaDescriptionKeyPropertyNotSet_SaveSearchMethodOfISearchManagerInstanceShouldNeverBeCalled()
        {
            searchManagerMock.Setup(sm => sm.SaveSearch(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<bool>(),
                It.IsAny<bool>(), It.IsAny<IDictionary<string, string>>())).Callback(() => wasCalled = false);

            viewModel.SelectedBook = testBook;
            viewModel.SaveSearch();

            Assert.IsFalse(wasCalled, "Saving newly added book criteria without a criteria description key incorrectly calls SaveSearch method of ISearchManager instance!");
        }

        [Test]
        public void SaveSearch_WithEmptyCriteria_SaveSearchMethodOfISearchManagerInstanceShouldNeverBeCalled()
        {
            searchManagerMock.Setup(sm => sm.SaveSearch(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<bool>(),
                It.IsAny<bool>(), It.IsAny<IDictionary<string, string>>())).Callback(() => wasCalled = false);

            viewModel.CriteriaDescriptionKey = "test Criteria";
            viewModel.SaveSearch();

            Assert.IsFalse(wasCalled, "Saving a search with an empty criteria collection incorrectly calls SaveSearch method of ISearchManager instance!");
        }
    }
}
