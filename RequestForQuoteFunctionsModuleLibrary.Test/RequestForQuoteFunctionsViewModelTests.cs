using System;
using System.Collections.Generic;
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
        private readonly Mock<IEventAggregator> eventAggregatorMock = new Mock<IEventAggregator>();
        private readonly Mock<IConfigurationManager> configManagerMock = new Mock<IConfigurationManager>();
        private readonly Mock<IGroupManager> groupManagerMock = new Mock<IGroupManager>();
        private readonly Mock<IUserManager> userManagerMock = new Mock<IUserManager>();

        private readonly Mock<NewBookEvent> newBookEventMock = new Mock<NewBookEvent>();
        private readonly Mock<NewClientEvent> newClientEventMock = new Mock<NewClientEvent>();
        private readonly Mock<NewSearchEvent> newSearchEventMock = new Mock<NewSearchEvent>();
        private readonly Mock<NewUnderlyierEvent> newUnderlyierEventMock = new Mock<NewUnderlyierEvent>();
        private readonly Mock<NewUserEvent> newUserEventMock = new Mock<NewUserEvent>();
        private readonly Mock<NewGroupEvent> newGroupEventMock = new Mock<NewGroupEvent>();

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

        private readonly IGroup testGroup = new GroupImpl()
            {
                GroupId = 1,
                GroupName = "test Group",
                IsValid = true
            };

        private readonly IUnderlying testUnderlying = new UnderlyingImpl()
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
            eventAggregatorMock.Setup(p => p.GetEvent<NewBookEvent>()).Returns(newBookEventMock.Object);
            eventAggregatorMock.Setup(p => p.GetEvent<NewClientEvent>()).Returns(newClientEventMock.Object);
            eventAggregatorMock.Setup(p => p.GetEvent<NewSearchEvent>()).Returns(newSearchEventMock.Object);
            eventAggregatorMock.Setup(p => p.GetEvent<NewUnderlyierEvent>()).Returns(newUnderlyierEventMock.Object);
            eventAggregatorMock.Setup(p => p.GetEvent<NewUserEvent>()).Returns(newUserEventMock.Object);
            eventAggregatorMock.Setup(p => p.GetEvent<NewGroupEvent>()).Returns(newGroupEventMock.Object);
            eventAggregatorMock.Setup(p => p.GetEvent<SearchRequestForQuoteEvent>())
                               .Returns(searchRequestForQuoteEventMock.Object);

            bookManagerMock.Setup(bm => bm.Books).Returns(new List<IBook>() {testBook});
            userManagerMock.Setup(gm => gm.Users).Returns(new List<IUser>() { testUser });
            groupManagerMock.Setup(bm => bm.Groups).Returns(new List<IGroup>() { testGroup });
            clientManagerMock.Setup(cm => cm.Clients).Returns(new List<IClient>() {testClient});
            underlyingManagerMock.Setup(um => um.Underlyings).Returns(new List<IUnderlying>() {testUnderlying});
            searchManagerMock.Setup(sm => sm.Searches).Returns(new List<ISearch>() {testSearch});

            viewModel = new RequestForQuoteFunctionsViewModel(eventAggregatorMock.Object, clientManagerMock.Object,
                                                              underlyingManagerMock.Object, bookManagerMock.Object,
                                                              searchManagerMock.Object, configManagerMock.Object, 
                                                              userManagerMock.Object, groupManagerMock.Object);
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
        [ExpectedException("System.ArgumentNullException")]
        public void HandleNewUserEvent_NullParameter_ArgumentNullExceptionThrown()
        {
            viewModel.HandleNewUserEvent(null);
        }

        [Test]
        [ExpectedException("System.ArgumentNullException")]
        public void HandleNewGroupEvent_NullParameter_ArgumentNullExceptionThrown()
        {
            viewModel.HandleNewGroupEvent(null);
        }

        [Test]
        [ExpectedException("System.ArgumentNullException")]
        public void HandleNewClientEvent_NullParameter_ArgumentNullExceptionThrown()
        {
            viewModel.HandleNewClientEvent(null);
        }

        [Test]
        [ExpectedException("System.ArgumentNullException")]
        public void HandleNewBookEvent_NullParameter_ArgumentNullExceptionThrown()
        {
            viewModel.HandleNewBookEvent(null);
        }

        [Test]
        [ExpectedException("System.ArgumentNullException")]
        public void HandleNewSearchEvent_NullParameter_ArgumentNullExceptionThrown()
        {
            viewModel.HandleNewSearchEvent(null);
        }

        [Test]
        [ExpectedException("System.ArgumentNullException")]
        public void HandleNewUnderlyierEvent_NullParameter_ArgumentNullExceptionThrown()
        {
            viewModel.HandleNewUnderlyierEvent(null);
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
            searchRequestForQuoteEventMock.Setup(s => s.Publish(It.IsAny<CriteriaUsageEventPayload>()))
                                          .Callback(() => wasCalled = true);
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
        public void ctor_UsersCollectionShouldBePopulated()
        {
            Assert.IsNotEmpty(viewModel.Users,
                              "view model ctor did NOT populate Users property with IUserManager instances' users!");
        }

        [Test]
        public void ctor_GroupsCollectionShouldBePopulated()
        {
            Assert.IsNotEmpty(viewModel.Groups,
                              "view model ctor did NOT populate Groups property with IGroupManager instances' groups!");
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
        public void ctor_NewUserEventShouldBeSubscribedTo()
        {
            newUserEventMock.Verify(
                cm => cm.Subscribe(It.IsAny<Action<NewUserEventPayload>>(), It.IsAny<ThreadOption>(),
                                   It.IsAny<bool>(), It.IsAny<Predicate<NewUserEventPayload>>()), Times.Once(),
                "view model ctor did not subscribe to new user event!");
        }

        [Test]
        public void ctor_NewGroupShouldBeSubscribedTo()
        {
            newGroupEventMock.Verify(
                cm => cm.Subscribe(It.IsAny<Action<NewGroupEventPayload>>(), It.IsAny<ThreadOption>(),
                                   It.IsAny<bool>(), It.IsAny<Predicate<NewGroupEventPayload>>()), Times.Once(),
                "view model ctor did not subscribe to new group event!");
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
            // Act
            viewModel.SelectedBook = null;
            // Assert
            Assert.IsFalse(viewModel.Criteria.ContainsKey(RequestForQuoteConstants.BOOK_CRITERION), "because the selected book value is invalid");
        }

        [Test]
        public void SelectedBook_ValidValue_ValueShouldBeAddedToCriteriaCollection()
        {
            // Act
            viewModel.SelectedBook = testBook;
            // Assert
            Assert.IsTrue(viewModel.Criteria.ContainsValue(testBook.BookCode), "because the selected book value is valid");
        }

        [Test]
        public void SelectedClient_InvalidValue_ValueShouldNotBeAddedToCriteriaCollection()
        {
            // Act
            viewModel.SelectedClient = null;
            // Assert            
            Assert.IsFalse(viewModel.Criteria.ContainsKey(RequestForQuoteConstants.CLIENT_CRITERION), "because the selected client property value is invalid");
        }

        [Test]
        public void Selectedclient_ValidValue_ValueShouldBeAddedToCriteriaCollection()
        {
            // Act
            viewModel.SelectedClient = testClient;
            // Assert
            Assert.IsTrue(viewModel.Criteria.ContainsValue(testClient.Identifier.ToString()), "because the selected client property value is valid");
        }

        [Test]
        public void SelectedUnderlyier_InvalidValue_ValueShouldNotBeAddedToCriteriaCollection()
        {
            // Act
            viewModel.SelectedUnderlying = null;
            // Assert
            Assert.IsFalse(viewModel.Criteria.ContainsKey(RequestForQuoteConstants.UNDERLYIER_CRITERION),"because the selected underlyier property value is invalid");
        }

        [Test]
        public void SelectedUnderlyier_ValidValue_ValueShouldBeAddedToCriteriaCollection()
        {
            // Act
            viewModel.SelectedUnderlying = testUnderlying;
            // Assert
            Assert.IsTrue(viewModel.Criteria.ContainsValue(testUnderlying.RIC), "because the selected underlyier property value is valid");
        }

        [Test]
        public void SelectedStatus_InvalidValue_ValueShouldNotBeAddedToCriteriaCollection()
        {
            // Act
            viewModel.SelectedStatus = null;
            // Assert
            Assert.IsFalse(viewModel.Criteria.ContainsKey(RequestForQuoteConstants.STATUS_CRITERION),"because the selected status property value is invalid");
        }

        [Test]
        public void SelectedStatus_ValidValue_ValueShouldBeAddedToCriteriaCollection()
        {
            // Act
            viewModel.SelectedStatus = "test status";
            // Assert
            Assert.IsTrue(viewModel.Criteria.ContainsValue("test status"), "because the selected status property value is valid");
        }

        [Test]
        public void StartTradeDate_SetValidStartTradeDateFirstThenEndDate_DateRangeShouldBeAddedToCriteriaCollection()
        {
            // Act
            viewModel.StartTradeDate = startDate;
            viewModel.EndTradeDate = endDate;
            // Assert
            Assert.IsTrue(viewModel.Criteria.ContainsValue(startDate + "-" + endDate),"because the start trade date can be added before the end trade date");
        }

        [Test]
        public void StartTradeDate_SetValidEndTradeDateFirstThenStartDate_DateRangeShouldBeAddedToCriteriaCollection()
        {
            // Act
            viewModel.EndTradeDate = endDate;
            viewModel.StartTradeDate = startDate;
            // Assert
            Assert.IsTrue(viewModel.Criteria.ContainsValue(startDate + "-" + endDate),"because the start trade date can be added before the end trade date");
        }

        [Test]
        public void StartTradeDate_NoEndTradeDateSet_StartTradeDateWithHyphenSuffixShouldBeAddedToCriteriaCollection()
        {
            viewModel.StartTradeDate = startDate;
            // Assert
            Assert.IsTrue(viewModel.Criteria.ContainsValue(startDate + "-"), "because the start trade date can be added without the end trade date");
        }

        [Test]
        public void EndTradeDate_NoStartTradeDateSet_EndTradeDateWithHyphenPrefixShouldBeAddedToCriteriaCollection()
        {
            // Arrange

            // Act
            viewModel.EndTradeDate = endDate;
            // Assert
            Assert.IsTrue(viewModel.Criteria.ContainsValue("-" + endDate), "because the end trade date can be added before the start trade date");
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
            Assert.IsTrue(viewModel.Criteria.ContainsValue(updatedDate + "-"),"because the start trade date can be overwritten without the end trade date");
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
            Assert.IsTrue(viewModel.Criteria.ContainsValue("-" + updatedDate), "because the end trade date can be overwritten without the start trade date");
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
            Assert.IsTrue(viewModel.Criteria.ContainsValue(startDate + "-" + updatedDate),"because the end trade date can be overwritten even with the start trade date set");
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
            Assert.IsTrue(viewModel.Criteria.ContainsValue(updatedDate + "-" + endDate), "because the start trade date can be overwritten even with the end trade date set");
        }

        [Test]
        public void CanFilterRequests_EmptyCriteriaCollection_ShouldReturnFalse()
        {
            // Arrange
            // ClearCriteria() method called in SetUp method.
            // Assert
            Assert.IsFalse(viewModel.CanFilterRequests(), "because the criteria collection is empty");
        }

        [Test]
        public void CanDeleteSearch_EmptyCriteriaCollection_ShouldReturnFalse()
        {
            // Arrange
            // ClearCriteria() method called in SetUp method.
            

            // Act

            // Assert
            Assert.IsFalse(viewModel.CanDeleteSearch(), "because the criteria collection is empty");
        }

        [Test]
        public void CanClearCriteria_EmptyCriteriaCollection_ShouldReturnFalse()
        {
            // Arrange
            // ClearCriteria() method called in SetUp method.

            // Act

            // Assert
            Assert.IsFalse(viewModel.CanClearCriteria(),"because the criteria collection is empty");
        }

        [Test]
        public void CanSaveSearch_EmptyCriteriaCollection_ShouldReturnFalse()
        {
            // Arrange
            // ClearCriteria() method called in SetUp method.

            // Act

            // Assert
            Assert.IsFalse(viewModel.CanSaveSearch(), "because the criteria collection is empty");
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
            Assert.IsFalse(viewModel.CanDeleteSearch(), "because an invalid search owner is set");
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
            Assert.IsFalse(viewModel.CanDeleteSearch(), "because an invalid search description key is set");
        }

        [Test]
        public void CanSearchRequests_NonEmptyCriteriaAndSetExistingSearchParamSetToFalse_ShouldReturnTrue()
        {
            // Arrange
            viewModel.SelectedBook = testBook;
            const bool isExistingSearch = false;
            // Act

            // Assert
            Assert.IsTrue(viewModel.CanSearchRequests(isExistingSearch), "because criteria exists");
        }

        [Test]
        public void CanSearchRequests_NoCriteriaAndSetExistingSearchParamSetToFalse_ShouldReturnFalse()
        {
            // Arrange
            // ClearCriteria() method called in SetUp method
            const bool isExistingSearch = false;
            // Act

            // Assert
            Assert.IsFalse(viewModel.CanSearchRequests(isExistingSearch),"because there are no criteria and existing search param set to false");
        }

        [Test]
        public void CanSearchRequests_EmptyCriteriaAndSetExistingSearchParamSetToTrue_ShouldReturnFalse()
        {
            // Arrange
            const bool isExistingSearch = true;
            viewModel.SelectedSearch = null;
            // Act

            // Assert
            Assert.IsFalse(viewModel.CanSearchRequests(isExistingSearch), "because there are no criteria and selected search property is not set");            
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
            Assert.IsTrue(viewModel.CanSearchRequests(isExistingSearch), "because existing search param set to true");
        }

        [Test]
        public void CanUpdatePrivacy_NoSearchSelected_ShouldReturnFalse()
        {
            // Arrange
            const bool isRequestToMakePrivate = true;
            // Act

            // Assert
            Assert.IsFalse(viewModel.CanUpdatePrivacy(isRequestToMakePrivate),"because selected search property is not set");
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
            Assert.IsFalse(viewModel.CanUpdatePrivacy(isRequestToMakePrivate), "because the selected search owner is invalid");
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
            Assert.IsFalse(viewModel.CanUpdatePrivacy(isRequestToMakePrivate), "because the selected search description key is invalid");
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
            Assert.IsFalse(viewModel.CanUpdatePrivacy(isRequestToMakePrivate), "because there is not change to the privacy flag");
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
            Assert.IsTrue(viewModel.CanUpdatePrivacy(isRequestToMakePrivate), "because the privacy flag has changed");
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
            Assert.IsTrue(wasCalled, "because valid properties were used");
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
            Assert.IsTrue(wasCalled, "because valid properties were used");
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
            Assert.IsTrue(viewModel.Searches.Count == 0, "because valid properties were used");
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
            Assert.IsTrue(viewModel.SelectedBook.BookCode.Equals("test book"), "because a valid book criterion was used");
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
            Assert.IsTrue(viewModel.SelectedBook.BookCode.Equals("test book"), "because a valid book criterion was used");
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
            Assert.IsTrue(viewModel.Criteria.ContainsValue("test book"), "because a valid book criterion was used");
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
            Assert.IsTrue(viewModel.Criteria.ContainsValue("test book"), "because a valid book criterion was used");
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
            Assert.IsTrue(wasCalled,"the event should be raised if criteria are initialized");
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
            Assert.IsTrue(wasCalled, "the event should be raised if criteria are initialized");
        }
    }
}