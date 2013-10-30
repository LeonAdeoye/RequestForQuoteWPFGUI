using System;
using System.Collections.Generic;
using FluentAssertions;
using Microsoft.Practices.Prism.Events;
using Moq;
using NUnit.Framework;
using RequestForQuoteInterfacesLibrary.Constants;
using RequestForQuoteInterfacesLibrary.Enums;
using RequestForQuoteInterfacesLibrary.EventPayloads;
using RequestForQuoteInterfacesLibrary.Events;
using RequestForQuoteInterfacesLibrary.ModelImplementations;
using RequestForQuoteInterfacesLibrary.ModelInterfaces;
using RequestForQuoteInterfacesLibrary.ServiceInterfaces;

namespace RequestForQuoteFunctionsModuleLibrary.Test
{
    [TestFixture]
    internal class RequestForQuoteFunctionsViewModelTests
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

        private readonly Mock<SearchRequestForQuoteEvent> searchRequestForQuoteEventMock =
            new Mock<SearchRequestForQuoteEvent>();

        private readonly IBook testBook = new BookImpl() {BookCode = "test book"};

        private readonly IClient testClient = new ClientImpl()
            {
                Identifier = 1,
                IsValid = true,
                Name = "test client",
                Tier = TierEnum.Top.ToString()
            };

        private readonly IUnderlyier testUnderlyier = new UnderlyierImpl()
            {
                Description = "test description",
                IsValid = true,
                RIC = "test RIC"
            };

        private readonly ISearch testSearch = new SearchImpl()
            {
                Criteria = new List<ISearchCriterion>(),
                DescriptionKey = "test key",
                IsFilter = false,
                IsPrivate = false,
                Owner = "test owner"
            };

        private RequestForQuoteFunctionsViewModel viewModel;
        private bool wasCalled;

        private readonly DateTime startDate = new DateTime(2012, 12, 23);
        private readonly DateTime endDate = new DateTime(2013, 12, 23);

        [TestFixtureSetUp]
        public void TestFixtureSetUp()
        {
            eventAggregaterMock.Setup(p => p.GetEvent<NewBookEvent>()).Returns(newBookEventMock.Object);
            eventAggregaterMock.Setup(p => p.GetEvent<NewClientEvent>()).Returns(newClientEventMock.Object);
            eventAggregaterMock.Setup(p => p.GetEvent<NewSearchEvent>()).Returns(newSearchEventMock.Object);
            eventAggregaterMock.Setup(p => p.GetEvent<NewUnderlyierEvent>()).Returns(newUnderlyierEventMock.Object);
            eventAggregaterMock.Setup(p => p.GetEvent<SearchRequestForQuoteEvent>())
                               .Returns(searchRequestForQuoteEventMock.Object);

            bookManagerMock.Setup(bm => bm.Books).Returns(new List<IBook>() {testBook});
            clientManagerMock.Setup(cm => cm.Clients).Returns(new List<IClient>() {testClient});
            underlyingManagerMock.Setup(um => um.Underlyiers).Returns(new List<IUnderlyier>() {testUnderlyier});
            searchManagerMock.Setup(sm => sm.Searches).Returns(new List<ISearch>() {testSearch});

            viewModel = new RequestForQuoteFunctionsViewModel(eventAggregaterMock.Object, clientManagerMock.Object,
                                                              underlyingManagerMock.Object, bookManagerMock.Object,
                                                              searchManagerMock.Object);
        }

        [SetUp]
        public void SetUpBeforeEachAndEveryTest()
        {
            viewModel.ClearCriteria();
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
        [ExpectedException(typeof(ArgumentNullException))]
        public void HandleNewClientEvent_NullParamter_ArgumentNullExceptionThrown()
        {
            viewModel.HandleNewClientEvent(null);
        }

        [Test]
        public void SelectedBook_PropertySet_BookCriteriaAdded()
        {
            viewModel.SelectedBook = testBook;
            Assert.IsNotEmpty(viewModel.Criteria,
                              "After setting SelectedBook property the criteria collection should not be empty!");
        }

        [Test]
        public void ClearCriteria_SearchRFQEventShouldBePublished()
        {
            viewModel.SelectedBook = testBook;
            searchRequestForQuoteEventMock.Setup(s => s.Publish(It.IsAny<CriteriaUsageEventPayload>()))
                                          .Callback(() => wasCalled = true);
            viewModel.ClearCriteria();
            Assert.IsTrue(wasCalled,
                          "After ClearCriteria method is called, SearchRequestForQuoteEvent event is NOT published!");
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
            Assert.IsNotEmpty(viewModel.Books,
                              "view model ctor did NOT populate Books property with IBookManager instances' books!");
        }

        [Test]
        public void ctor_ClientsCollectionShouldBePopulated()
        {
            Assert.IsNotEmpty(viewModel.Clients,
                              "view model ctor did NOT populate Clients property with IClientManager instances' clients!");
        }

        [Test]
        public void ctor_UnderlyiersCollectionShouldBePopulated()
        {
            Assert.IsNotEmpty(viewModel.Underlyiers,
                              "view model ctor did NOT populate Underlyiers property with IUnderlyierManager instances' underlyiers!");
        }

        [Test]
        public void ctor_SearchesCollectionShouldBePopulated()
        {
            Assert.IsNotEmpty(viewModel.Searches,
                              "view model ctor did NOT populate Seacrhes property with ISearchManager instances' searches!");
        }

        [Test]
        public void ctor_NewBookEventShouldBeSubscribedTo()
        {
            newBookEventMock.Verify(
                bm => bm.Subscribe(It.IsAny<Action<NewBookEventPayload>>(), It.IsAny<ThreadOption>(),
                                   It.IsAny<bool>(), It.IsAny<Predicate<NewBookEventPayload>>()), Times.Once(),
                "view model ctor did not subscribe to new book event!");
        }

        [Test]
        public void ctor_NewClientEventShouldBeSubscribedTo()
        {
            newClientEventMock.Verify(
                cm => cm.Subscribe(It.IsAny<Action<NewClientEventPayload>>(), It.IsAny<ThreadOption>(),
                                   It.IsAny<bool>(), It.IsAny<Predicate<NewClientEventPayload>>()), Times.Once(),
                "view model ctor did not subscribe to new client event!");
        }

        [Test]
        public void ctor_NewUnderlyierEventShouldBeSubscribedTo()
        {
            newUnderlyierEventMock.Verify(
                um => um.Subscribe(It.IsAny<Action<NewUnderlyierEventPayload>>(), It.IsAny<ThreadOption>(),
                                   It.IsAny<bool>(), It.IsAny<Predicate<NewUnderlyierEventPayload>>()), Times.Once(),
                "view model ctor did not subscribe to new underlyier event!");
        }

        [Test]
        public void ctor_NewSearchEventShouldBeSubscribedTo()
        {
            newSearchEventMock.Verify(
                um => um.Subscribe(It.IsAny<Action<NewSearchEventPayload>>(), It.IsAny<ThreadOption>(),
                                   It.IsAny<bool>(), It.IsAny<Predicate<NewSearchEventPayload>>()), Times.Once(),
                "view model ctor did not subscribe to new search event!");
        }

        [Test]
        public void SaveSearch_WithNonEmptyCriteriaAndCriteriaDescriptionKeyPropertySet_SaveSearchMethodOfISearchManagerInstanceShouldBeCalled()
        {
            searchManagerMock.Setup(sm => sm.SaveSearchToDatabase(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<bool>(),
                             It.IsAny<bool>(), It.IsAny<string>(), It.IsAny<string>())).Returns(true).Callback(() => wasCalled = true);

            viewModel.SelectedBook = testBook;
            viewModel.CriteriaDescriptionKey = "test Criteria";
            viewModel.SaveSearch();

            Assert.IsTrue(wasCalled, "Saving newly added book criteria does NOT call SaveSearch method of ISearchManager instance!");
        }

        [Test]
        public void
            SaveSearch_CriteriaDescriptionKeyPropertyNotSet_SaveSearchMethodOfISearchManagerInstanceShouldNeverBeCalled()
        {
            searchManagerMock.Setup(sm => sm.SaveSearchToDatabase(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<bool>(),
                             It.IsAny<bool>(), It.IsAny<string>(), It.IsAny<string>())).Callback(() => wasCalled = false);

            viewModel.SelectedBook = testBook;
            viewModel.SaveSearch();

            Assert.IsFalse(wasCalled,
                           "Saving newly added book criteria without a criteria description key incorrectly calls SaveSearch method of ISearchManager instance!");
        }

        [Test]
        public void SaveSearch_WithEmptyCriteria_SaveSearchMethodOfISearchManagerInstanceShouldNeverBeCalled()
        {
            // Arrange
            searchManagerMock.Setup(sm => sm.SaveSearchToDatabase(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<bool>(),
                            It.IsAny<bool>(), It.IsAny<string>(), It.IsAny<string>())).Callback(() => wasCalled = false);

            viewModel.CriteriaDescriptionKey = "test Criteria";

            // Act
            viewModel.SaveSearch();
            // Assert
            Assert.IsFalse(wasCalled,
                           "Saving a search with an empty criteria collection incorrectly calls SaveSearch method of ISearchManager instance!");
        }

        [Test]
        public void SelectedBook_InvalidValue_ValueShouldNotBeAddedToCriteriaCollection()
        {
            // Arrange

            // Act
            viewModel.SelectedBook = null;
            // Assert
            viewModel.Criteria.Should().NotContainKey(RequestForQuoteConstants.BOOK_CRITERION,"because the selected book value is invalid");
        }

        [Test]
        public void SelectedBook_ValidValue_ValueShouldBeAddedToCriteriaCollection()
        {
            // Arrange

            // Act
            viewModel.SelectedBook = testBook;
            // Assert
            viewModel.Criteria.Should().ContainValue(testBook.BookCode, "because the selected book value is valid");
        }

        [Test]
        public void SelectedClient_InvalidValue_ValueShouldNotBeAddedToCriteriaCollection()
        {
            // Arrange

            // Act
            viewModel.SelectedClient = null;
            // Assert
            viewModel.Criteria.Should().NotContainKey(RequestForQuoteConstants.CLIENT_CRITERION,"because the selected client property value is invalid");
        }

        [Test]
        public void Selectedclient_ValidValue_ValueShouldBeAddedToCriteriaCollection()
        {
            // Arrange

            // Act
            viewModel.SelectedClient = testClient;
            // Assert
            viewModel.Criteria.Should().ContainValue(testClient.Identifier.ToString(), "because the selected client property value is valid");
        }

        [Test]
        public void SelectedUnderlyier_InvalidValue_ValueShouldNotBeAddedToCriteriaCollection()
        {
            // Arrange

            // Act
            viewModel.SelectedUnderlyier = null;
            // Assert
            viewModel.Criteria.Should().NotContainKey(RequestForQuoteConstants.UNDERLYIER_CRITERION,"because the selected underlyier property value is invalid");
        }

        [Test]
        public void SelectedUnderlyier_ValidValue_ValueShouldBeAddedToCriteriaCollection()
        {
            // Arrange

            // Act
            viewModel.SelectedUnderlyier = testUnderlyier;
            // Assert
            viewModel.Criteria.Should().ContainValue(testUnderlyier.RIC, "because the selected underlyier property value is valid");
        }

        [Test]
        public void SelectedStatus_InvalidValue_ValueShouldNotBeAddedToCriteriaCollection()
        {
            // Arrange

            // Act
            viewModel.SelectedStatus = null;
            // Assert
            viewModel.Criteria.Should().NotContainKey(RequestForQuoteConstants.STATUS_CRITERION,"because the selected status property value is invalid");
        }

        [Test]
        public void SelectedStatus_ValidValue_ValueShouldBeAddedToCriteriaCollection()
        {
            // Arrange

            // Act
            viewModel.SelectedStatus = "test status";
            // Assert
            viewModel.Criteria.Should().ContainValue("test status", "because the selected status property value is valid");
        }

        [Test]
        public void StartTradeDate_SetValidStartTradeDateFirstThenEndDate_DateRangeShouldBeAddedToCriteriaCollection()
        {
            // Arrange

            // Act
            viewModel.StartTradeDate = startDate;
            viewModel.EndTradeDate = endDate;
            // Assert
            viewModel.Criteria.Should().ContainValue(startDate + "-" + endDate,"because the start trade date can be added before the end trade date");
        }

        [Test]
        public void StartTradeDate_SetValidEndTradeDateFirstThenStartDate_DateRangeShouldBeAddedToCriteriaCollection()
        {
            // Arrange

            // Act
            viewModel.EndTradeDate = endDate;
            viewModel.StartTradeDate = startDate;
            // Assert
            viewModel.Criteria.Should().ContainValue(startDate + "-" + endDate,"because the start trade date can be added before the end trade date");
        }

        [Test]
        public void StartTradeDate_NoEndTradeDateSet_StartTradeDateWithHyphenSuffixShouldBeAddedToCriteriaCollection()
        {
            // Arrange

            viewModel.StartTradeDate = startDate;
            // Assert
            viewModel.Criteria.Should().ContainValue(startDate + "-","because the start trade date can be added without the end trade date");
        }

        [Test]
        public void EndTradeDate_NoStartTradeDateSet_EndTradeDateWithHyphenPrefixShouldBeAddedToCriteriaCollection()
        {
            // Arrange

            // Act
            viewModel.EndTradeDate = endDate;
            // Assert
            viewModel.Criteria.Should().ContainValue("-" + endDate, "because the end trade date can be added before the start trade date");
        }

        [Test]
        public void StartTradeDate_StartTradeDateSetTwice_StartTradeDateWithHyphenSuffixShouldBeReplacedInTheCriteriaCollection()
        {
            // Arrange
            DateTime updatedDate = new DateTime(2014, 12, 23);
            // Act
            viewModel.StartTradeDate = startDate;
            viewModel.StartTradeDate = updatedDate;
            // Assert
            viewModel.Criteria.Should().ContainValue(updatedDate + "-","because the start trade date can be overwritten without the end trade date");
        }

        [Test]
        public void EndTradeDate_EndTradeDateSetTwice_EndTradeDateWithHyphenPrefixShouldBeReplacedInTheCriteriaCollection()
        {
            // Arrange
            DateTime updatedDate = new DateTime(2014, 12, 23);
            // Act
            viewModel.EndTradeDate = endDate;
            viewModel.EndTradeDate = updatedDate;
            // Assert
            viewModel.Criteria.Should().ContainValue("-" + updatedDate,"because the end trade date can be overwritten without the start trade date");
        }

        [Test]
        public void EndTradeDate_StartTradeDateSetAndEndTradeDateSetTwice_JusTheEndTradeDateShouldBeReplacedInTheCriteriaCollection()
        {
            // Arrange
            DateTime updatedDate = new DateTime(2014, 12, 23);
            viewModel.StartTradeDate = startDate;
            viewModel.EndTradeDate = endDate;
            // Act            
            viewModel.EndTradeDate = updatedDate;
            // Assert
            viewModel.Criteria.Should().ContainValue(startDate + "-" + updatedDate,"because the end trade date can be overwritten even with the start trade date set");
        }

        [Test]
        public void tartTradeDate_EndTradeDateSetAndStartTradeDateSetTwice_JusTheStartTradeDateShouldBeReplacedInTheCriteriaCollection()
        {
            // Arrange
            DateTime updatedDate = new DateTime(2014, 12, 23);
            viewModel.StartTradeDate = startDate;
            viewModel.EndTradeDate = endDate;
            // Act            
            viewModel.StartTradeDate = updatedDate;
            // Assert
            viewModel.Criteria.Should().ContainValue(updatedDate + "-" + endDate,"because the start trade date can be overwritten even with the end trade date set");
        }

        [Test]
        public void CanFilterRequests_EmptyCriteriaCollection_ShouldReturnFalse()
        {
            // Arrange
            // ClearCriteria() method called in SetUp method.

            // Act

            // Assert
            viewModel.CanFilterRequests().Should().BeFalse("because the criteria collection is empty");
        }

        [Test]
        public void CanDeleteSearch_EmptyCriteriaCollection_ShouldReturnFalse()
        {
            // Arrange
            // ClearCriteria() method called in SetUp method.
            

            // Act

            // Assert
            viewModel.CanDeleteSearch().Should().BeFalse("because the criteria collection is empty");
        }

        [Test]
        public void CanClearCriteria_EmptyCriteriaCollection_ShouldReturnFalse()
        {
            // Arrange
            // ClearCriteria() method called in SetUp method.

            // Act

            // Assert
            viewModel.CanClearCriteria().Should().BeFalse("because the criteria collection is empty");
        }

        [Test]
        public void CanSaveSearch_EmptyCriteriaCollection_ShouldReturnFalse()
        {
            // Arrange
            // ClearCriteria() method called in SetUp method.

            // Act

            // Assert
            viewModel.CanSaveSearch().Should().BeFalse("because the criteria collection is empty");
        }

        [Test]
        public void CanDeleteSearch_InvalidSelectedSearchOwner_ShouldReturnFalse()
        {
            // Arrange
            var searchMock = new Mock<ISearch>();
            searchMock.Setup(s => s.DescriptionKey).Returns("test description key");
            searchMock.Setup(s => s.Owner).Returns(String.Empty);
            // Act
            viewModel.SelectedSearch = searchMock.Object;
            // Assert
            viewModel.CanDeleteSearch().Should().BeFalse("because an invalid search owner is set");
        }

        [Test]
        public void CanDeleteSearch_InvalidSelectedSearchDescriptionKey_ShouldReturnFalse()
        {
            // Arrange
            var searchMock = new Mock<ISearch>();
            searchMock.Setup(s => s.DescriptionKey).Returns(string.Empty);
            searchMock.Setup(s => s.Owner).Returns("test owner");
            // Act
            viewModel.SelectedSearch = searchMock.Object;
            // Assert
            viewModel.CanDeleteSearch().Should().BeFalse("because an invalid search description key is set");
        }

        [Test]
        public void CanSearchRequests_NonEmptyCriteriaAndSetExistingSearchParamSetToFalse_ShouldReturnTrue()
        {
            // Arrange
            viewModel.SelectedBook = testBook;
            const bool isExistingSearch = false;
            // Act

            // Assert
            viewModel.CanSearchRequests(isExistingSearch).Should().BeTrue("because criteria exists");
        }

        [Test]
        public void CanSearchRequests_NoCriteriaAndSetExistingSearchParamSetToFalse_ShouldReturnFalse()
        {
            // Arrange
            // ClearCriteria() method called in SetUp method
            const bool isExistingSearch = false;
            // Act

            // Assert
            viewModel.CanSearchRequests(isExistingSearch).Should().BeFalse("because there are no criteria and existing search param set to false");
        }

        [Test]
        public void CanSearchRequests_EmptyCriteriaAndSetExistingSearchParamSetToTrue_ShouldReturnFalse()
        {
            // Arrange
            const bool isExistingSearch = true;
            viewModel.SelectedSearch = null;
            // Act

            // Assert
            viewModel.CanSearchRequests(isExistingSearch).Should().BeFalse("because there are no criteria and selected search property is not set");            
        }

        [Test]
        public void CanSearchRequests_EmptyCriteriaAndSetExistingSearchParamSetToTrueAndSelectedSearchSet_ShouldReturnTrue()
        {
            // Arrange 
            // ClearCriteria() method called in SetUp method
            const bool isExistingSearch = true;
            var searchMock = new Mock<ISearch>();
            viewModel.SelectedSearch = searchMock.Object;
            // Act
            // Assert
            viewModel.CanSearchRequests(isExistingSearch).Should().BeTrue("because existing search param set to true");
        }

        [Test]
        public void CanUpdatePrivacy_NoSearchSelected_ShouldReturnFalse()
        {
            // Arrange
            const bool isRequestToMakePrivate = true;
            // Act

            // Assert
            viewModel.CanUpdatePrivacy(isRequestToMakePrivate).Should().BeFalse("because selected search property is not set");
        }

        [Test]
        public void CanUpdatePrivacy_InvalidSelectedSearchOwner_ShouldReturnFalse()
        {
            // Arrange
            const bool isRequestToMakePrivate = true;
            var searchMock = new Mock<ISearch>();
            viewModel.SelectedSearch = searchMock.Object;
            searchMock.Setup(s => s.DescriptionKey).Returns("test description key");
            searchMock.Setup(s => s.Owner).Returns(String.Empty);
            // Act

            // Assert
            viewModel.CanUpdatePrivacy(isRequestToMakePrivate).Should().BeFalse("because the selected search owner is invalid");
        }

        [Test]
        public void CanUpdatePrivacy_InvalidSelectedSearchDescriptionKey_ShouldReturnFalse()
        {
            // Arrange
            const bool isRequestToMakePrivate = true;
            var searchMock = new Mock<ISearch>();
            viewModel.SelectedSearch = searchMock.Object;
            searchMock.Setup(s => s.DescriptionKey).Returns(string.Empty);
            searchMock.Setup(s => s.Owner).Returns("test owner");
            // Act

            // Assert
            viewModel.CanUpdatePrivacy(isRequestToMakePrivate).Should().BeFalse("because the selected search description key is invalid");
        }

        [Test]
        public void CanUpdatePrivacy_SelectedSearchPrivacyTheSame_ShouldReturnFalse()
        {
            // Arrange
            const bool isRequestToMakePrivate = true;
            var searchMock = new Mock<ISearch>();
            viewModel.SelectedSearch = searchMock.Object;
            searchMock.Setup(s => s.IsPrivate).Returns(isRequestToMakePrivate);
            searchMock.Setup(s => s.DescriptionKey).Returns("test description key");
            searchMock.Setup(s => s.Owner).Returns("test owner");
            // Act

            // Assert
            viewModel.CanUpdatePrivacy(isRequestToMakePrivate).Should().BeFalse("because there is not change to the privacy flag");
        }

        [Test]
        public void CanUpdatePrivacy_SelectedSearchPrivacyIsDifferent_ShouldReturnTrue()
        {
            // Arrange
            const bool isRequestToMakePrivate = true;
            var searchMock = new Mock<ISearch>();
            viewModel.SelectedSearch = searchMock.Object;
            searchMock.Setup(s => s.IsPrivate).Returns(!isRequestToMakePrivate);
            searchMock.Setup(s => s.DescriptionKey).Returns("test description key");
            searchMock.Setup(s => s.Owner).Returns("test owner");
            // Act

            // Assert
            viewModel.CanUpdatePrivacy(isRequestToMakePrivate).Should().BeTrue("because the privacy flag has changed");
        }

        [Test]
        public void UpdatePrivacy_ValidOwnerAndDescriptionKeyPropertiesSet_UpdatePrivacyMethodShouldBeCalled()
        {
            // Arrange
            searchManagerMock.Setup(sm => sm.UpdatePrivacy(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<bool>())).Returns(true).Callback(() => wasCalled = true);

            var searchMock = new Mock<ISearch>();
            viewModel.SelectedSearch = searchMock.Object;
            searchMock.Setup(s => s.IsPrivate).Returns(true);
            searchMock.Setup(s => s.DescriptionKey).Returns("test description key");
            searchMock.Setup(s => s.Owner).Returns("test owner");
            // Act
            viewModel.UpdatePrivacy();
            // Assert
            wasCalled.Should().BeTrue("because valid properties were used");
        }

        [Test]
        public void DeleteSearch_WithAllValidPropertiesSet_DeleteSearchMethodShouldBeCalled()
        {
            // Arrange
            searchManagerMock.Setup(sm => sm.DeleteSearch(It.IsAny<string>(), It.IsAny<string>())).Returns(true).Callback(() => wasCalled = true);
            var searchMock = new Mock<ISearch>();
            viewModel.SelectedSearch = searchMock.Object;
            searchMock.Setup(s => s.DescriptionKey).Returns("test description key");
            searchMock.Setup(s => s.Owner).Returns("test owner");
            // Act
            viewModel.DeleteSearch();
            // Assert
            wasCalled.Should().BeTrue("because valid properties were used");
        }

        [Test]
        public void DeleteSearch_WithAllValidPropertiesSet_DeletedSearchShouldBeRemovedFromTheSearchesCollection()
        {
            // Arrange
            viewModel.Searches.Clear();
            var searchMock = new Mock<ISearch>();
            viewModel.SelectedSearch = searchMock.Object;
            searchManagerMock.Setup(sm => sm.DeleteSearch(It.IsAny<string>(), It.IsAny<string>())).Returns(true);
            searchMock.Setup(s => s.DescriptionKey).Returns("test description key");
            searchMock.Setup(s => s.Owner).Returns("test owner");
            viewModel.Searches.Add(searchMock.Object);
            // Act
            viewModel.DeleteSearch();
            // Assert
            viewModel.Searches.Should().BeEmpty("because valid properties were used");
        }

        [Test]
        public void FilterRequests_SelectedSearchForExistingCriteria_BookCodeShouldBeSet()
        {
            // Arrange
            var searchMock = new Mock<ISearch>();
            viewModel.SelectedSearch = searchMock.Object;
            const bool isExistingSearch = true;
            searchMock.Setup(s => s.Criteria)
                .Returns(new List<ISearchCriterion>() { new SearchCriterionImpl() { ControlName = RequestForQuoteConstants.BOOK_CRITERION, ControlValue = "test book"} });
            // Act
            viewModel.FilterRequests(isExistingSearch);
            // Assert
            viewModel.SelectedBook.BookCode.Should().BeEquivalentTo("test book", "because a valid book criterion was used");
        }

        [Test]
        public void SearchRequests_SelectedSearchForExistingCriteria_BookCodeShouldBeSet()
        {
            // Arrange
            var searchMock = new Mock<ISearch>();
            viewModel.SelectedSearch = searchMock.Object;
            const bool isExistingSearch = true;
            searchMock.Setup(s => s.Criteria)
                      .Returns(new List<ISearchCriterion>() { new SearchCriterionImpl() { ControlName = RequestForQuoteConstants.BOOK_CRITERION, ControlValue = "test book" } });
            // Act
            viewModel.SearchRequests(isExistingSearch);
            // Assert
            viewModel.SelectedBook.BookCode.Should().BeEquivalentTo("test book", "because a valid book criterion was used");
        }

        [Test]
        public void SearchRequests_SelectedSearchForExistingCriteria_BookCriterionShouldBeAddedToViewModelCriteria()
        {
            // Arrange
            var searchMock = new Mock<ISearch>();
            viewModel.SelectedSearch = searchMock.Object;
            const bool isExistingSearch = true;
            searchMock.Setup(s => s.Criteria)
                      .Returns(new List<ISearchCriterion>() { new SearchCriterionImpl() { ControlName = RequestForQuoteConstants.BOOK_CRITERION, ControlValue = "test book" } });
            // Act
            viewModel.SearchRequests(isExistingSearch);
            // Assert
            viewModel.Criteria.Should().ContainValue("test book", "because a valid book criterion was used");
        }

        [Test]
        public void FilterRequests_SelectedSearchForExistingCriteria_BookCriterionShouldBeAddedToViewModelCriteria()
        {
            // Arrange
            var searchMock = new Mock<ISearch>();
            viewModel.SelectedSearch = searchMock.Object;
            const bool isExistingSearch = true;
            searchMock.Setup(s => s.Criteria)
                      .Returns(new List<ISearchCriterion>() { new SearchCriterionImpl() { ControlName = RequestForQuoteConstants.BOOK_CRITERION, ControlValue = "test book" } });
            // Act
            viewModel.FilterRequests(isExistingSearch);
            // Assert
            viewModel.Criteria.Should().ContainValue("test book", "because a valid book criterion was used");
        }

        [Test]
        public void SearchRequests_SelectedSearchForNonExistingCriteria_BookCriterionShouldBeAddedToViewModelCriteria()
        {
            // Arrange
            viewModel.SelectedBook = testBook;
            const bool isExistingSearch = false;
            searchRequestForQuoteEventMock.Setup(s => s.Publish(It.IsAny<CriteriaUsageEventPayload>())).Callback(() => wasCalled = true);
            // Act
            viewModel.SearchRequests(isExistingSearch);
            // Assert
            wasCalled.Should().BeTrue("the event should be raised if criteria are initialized");
        }

        [Test]
        public void FilterRequests_SelectedSearchForNonExistingCriteria_BookCriterionShouldBeAddedToViewModelCriteria()
        {
            // Arrange
            viewModel.SelectedBook = testBook;
            const bool isExistingSearch = false;
            searchRequestForQuoteEventMock.Setup(s => s.Publish(It.IsAny<CriteriaUsageEventPayload>())).Callback(() => wasCalled = true);
            // Act
            viewModel.FilterRequests(isExistingSearch);
            // Assert
            wasCalled.Should().BeTrue("the event should be raised if criteria are initialized");
        }
    }
}