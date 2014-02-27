using System;
using FluentAssertions;
using Microsoft.Practices.Prism.Events;
using Moq;
using NUnit.Framework;
using RequestForQuoteInterfacesLibrary.Events;
using RequestForQuoteInterfacesLibrary.ServiceInterfaces;
using RequestForQuoteServicesModuleLibrary.GroupMaintenanceService;
using RequestForQuoteServicesModuleLibrary.ServicesImplementation;

namespace RequestForQuoteServicesModuleLibrary.Test
{
    public class GroupManagerTest
    {
        private readonly Mock<GroupControllerClient> groupController = new Mock<GroupControllerClient>();
        private readonly Mock<IEventAggregator> eventAggregatorMock = new Mock<IEventAggregator>();
        private readonly Mock<IConfigurationManager> configManagerMock = new Mock<IConfigurationManager>();
        private readonly Mock<NewGroupEvent> newGroupEventMock = new Mock<NewGroupEvent>();

        [TestFixtureSetUp]
        public void TestFixtureSetUp()
        {
            eventAggregatorMock.Setup(p => p.GetEvent<NewGroupEvent>()).Returns(newGroupEventMock.Object);
        }

        [Test]
        public void Constructor_ValidParameters_GroupsCollectionInstaniated()
        {
            // Act
            IGroupManager groupManager = new GroupManagerImpl(configManagerMock.Object, eventAggregatorMock.Object, groupController.Object);
            // Assert
            groupManager.Groups.Should().NotBeNull("because the constructor instantiates it.");
        }

        [Test]
        public void Constructor_NullConfigManager_ArgumentNullExceptionThrown()
        {
            // Act
            Action act = () => new GroupManagerImpl(null, eventAggregatorMock.Object, groupController.Object);
            // Assert
            act.ShouldThrow<ArgumentNullException>("because configManager parameter cannot be null").WithMessage("configManager", ComparisonMode.Substring);
        }

        [Test]
        public void Constructor_NullEventAggregator_ArgumentNullExceptionThrown()
        {
            // Act
            Action act = () => new GroupManagerImpl(configManagerMock.Object, null, groupController.Object);
            // Assert
            act.ShouldThrow<ArgumentNullException>("because event aggregator parameter cannot be null.").WithMessage("eventAggregator", ComparisonMode.Substring);
        }

        [Test]
        public void Initialize_StandAloneMode_GroupsCollectionPopulated()
        {
            // Arrange
            IGroupManager groupManager = new GroupManagerImpl(configManagerMock.Object, eventAggregatorMock.Object, groupController.Object);
            configManagerMock.Setup(cm => cm.IsStandAlone).Returns(true);
            // Act
            groupManager.Initialize();
            // Assert
            groupManager.Groups.Should().NotBeEmpty("because it is populated in standalone mode.");
        }

        [TestCase(null)]
        [TestCase("")]
        public void AddGroup_InvalidGroupCodeParameter_ArgumentExceptionThrown(string groupId)
        {
            // Arrange
            IGroupManager groupManager = new GroupManagerImpl(configManagerMock.Object, eventAggregatorMock.Object, groupController.Object);
            // Act
            Action act = () => groupManager.AddGroup(groupId, "firstName", "lastName", "emailAddress", "locationName", 1, true);
            // Assert
            act.ShouldThrow<ArgumentException>("because group code parameter cannot be empty or null.").WithMessage("groupId", ComparisonMode.Substring);
        }

        [TestCase(null)]
        [TestCase("")]
        public void AddGroup_InvalidFirstNameParameter_ArgumentExceptionThrown(string firstName)
        {
            // Arrange
            IGroupManager groupManager = new GroupManagerImpl(configManagerMock.Object, eventAggregatorMock.Object, groupController.Object);
            // Act
            Action act = () => groupManager.AddGroup("testGroupId", firstName, "lastName", "emailAddress", "locationName", 1, true);
            // Assert
            act.ShouldThrow<ArgumentException>("because firstName parameter cannot be empty or null.").WithMessage("firstName", ComparisonMode.Substring);
        }

        [TestCase(null)]
        [TestCase("")]
        public void AddGroup_InvalidLastNameParameter_ArgumentExceptionThrown(string lastName)
        {
            // Arrange
            IGroupManager groupManager = new GroupManagerImpl(configManagerMock.Object, eventAggregatorMock.Object, groupController.Object);
            // Act
            Action act = () => groupManager.AddGroup("testGroupId", "firstName", lastName, "emailAddress", "locationName", 1, true);
            // Assert
            act.ShouldThrow<ArgumentException>("because lastName parameter cannot be empty or null.").WithMessage("lastName", ComparisonMode.Substring);
        }

        [TestCase(null)]
        [TestCase("")]
        public void AddGroup_InvalidEmailAddressParameter_ArgumentExceptionThrown(string emailAddress)
        {
            // Arrange
            IGroupManager groupManager = new GroupManagerImpl(configManagerMock.Object, eventAggregatorMock.Object, groupController.Object);
            // Act
            Action act = () => groupManager.AddGroup("testGroupId", "firstName", "lastName", emailAddress, "locationName", 1, true);
            // Assert
            act.ShouldThrow<ArgumentException>("because email Address parameter cannot be empty or null.").WithMessage("emailAddress", ComparisonMode.Substring);
        }

        [TestCase(null)]
        [TestCase("")]
        public void AddGroup_InvalidLocationNameParameter_ArgumentExceptionThrown(string locationName)
        {
            // Arrange
            IGroupManager groupManager = new GroupManagerImpl(configManagerMock.Object, eventAggregatorMock.Object, groupController.Object);
            // Act
            Action act = () => groupManager.AddGroup("testGroupId", "firstName", "lastName", "emailAddress", locationName, 1, true);
            // Assert
            act.ShouldThrow<ArgumentException>("because location name parameter cannot be empty or null.").WithMessage("locationName", ComparisonMode.Substring);
        }

        [Test]
        public void AddGroup_ValidParameters_GroupsCollectionPopulated()
        {
            // Arrange
            IGroupManager groupManager = new GroupManagerImpl(configManagerMock.Object, eventAggregatorMock.Object, groupController.Object);
            // Act
            groupManager.AddGroup("testGroupId", "firstName", "lastName", "emailAddress", "locationName", 1, true);
            // Assert
            groupManager.Groups.Should().NotBeEmpty("because a new group is added to the collection by AddGroup");
        }

        [Test]
        public void AddGroup_ValidParameters_NewGroupEventShouldBePublished()
        {
            // Arrange
            IGroupManager groupManager = new GroupManagerImpl(configManagerMock.Object, eventAggregatorMock.Object, groupController.Object);
            var wasCalled = false;
            eventAggregatorMock.Setup(ea => ea.GetEvent<NewGroupEvent>().Publish(It.IsAny<NewGroupEventPayload>())).Callback(() => wasCalled = true);
            // Act
            groupManager.AddGroup("testGroupId", "firstName", "lastName", "emailAddress", "locationName", 1, true);
            // Assert
            wasCalled.Should().BeTrue("because a new group is published to all listeners by the AddGroup method.");
        }

        [TestCase(null)]
        [TestCase("")]
        public void SaveToDatabase_InvalidGroupCodeParameter_ArgumentExceptionThrown(String groupId)
        {
            // Arrange
            IGroupManager groupManager = new GroupManagerImpl(configManagerMock.Object, eventAggregatorMock.Object, groupController.Object);
            // Act
            Action act = () => groupManager.SaveToDatabase(groupId, "firstName", "lastName", "emailAddress", "locationName", 1);
            // Assert
            act.ShouldThrow<ArgumentException>("because group id parameter cannot be empty or null.").WithMessage("groupId", ComparisonMode.Substring);
        }

        [TestCase(null)]
        [TestCase("")]
        public void SaveToDatabase_InvalidFirstNameParameter_ArgumentExceptionThrown(String firstName)
        {
            // Arrange
            IGroupManager groupManager = new GroupManagerImpl(configManagerMock.Object, eventAggregatorMock.Object, groupController.Object);
            // Act
            Action act = () => groupManager.SaveToDatabase("testGroupId", firstName, "lastName", "emailAddress", "locationName", 1);
            // Assert
            act.ShouldThrow<ArgumentException>("because first name parameter cannot be empty or null.").WithMessage("firstName", ComparisonMode.Substring);
        }

        [TestCase(null)]
        [TestCase("")]
        public void SaveToDatabase_InvalidLastNameParameter_ArgumentExceptionThrown(String lastName)
        {
            // Arrange
            IGroupManager groupManager = new GroupManagerImpl(configManagerMock.Object, eventAggregatorMock.Object, groupController.Object);
            // Act
            Action act = () => groupManager.SaveToDatabase("testGroupId", "firstName", lastName, "emailAddress", "locationName", 1);
            // Assert
            act.ShouldThrow<ArgumentException>("because last name parameter cannot be empty or null.").WithMessage("lastName", ComparisonMode.Substring);
        }

        [TestCase(null)]
        [TestCase("")]
        public void SaveToDatabase_InvalidEmailAddressParameter_ArgumentExceptionThrown(String emailAddress)
        {
            // Arrange
            IGroupManager groupManager = new GroupManagerImpl(configManagerMock.Object, eventAggregatorMock.Object, groupController.Object);
            // Act
            Action act = () => groupManager.SaveToDatabase("testGroupId", "firstName", "lastName", emailAddress, "locationName", 1);
            // Assert
            act.ShouldThrow<ArgumentException>("because last email address cannot be empty or null.").WithMessage("emailAddress", ComparisonMode.Substring);
        }

        [TestCase(null)]
        [TestCase("")]
        public void SaveToDatabase_InvalidLocationNameParameter_ArgumentExceptionThrown(String locationName)
        {
            // Arrange
            IGroupManager groupManager = new GroupManagerImpl(configManagerMock.Object, eventAggregatorMock.Object, groupController.Object);
            // Act
            Action act = () => groupManager.SaveToDatabase("testGroupId", "firstName", "lastName", "emailAddress", locationName, 1);
            // Assert
            act.ShouldThrow<ArgumentException>("because location name cannot be empty or null.").WithMessage("locationName", ComparisonMode.Substring);
        }

        [TestCase(null)]
        [TestCase("")]
        public void UpdateValidity_InvalidGroupCodeParameter_ArgumentExceptionThrown(String groupId)
        {
            // Arrange
            IGroupManager groupManager = new GroupManagerImpl(configManagerMock.Object, eventAggregatorMock.Object, groupController.Object);
            // Act
            Action act = () => groupManager.UpdateValidity(groupId, true);
            // Assert
            act.ShouldThrow<ArgumentException>("because group id parameter cannot be empty or null.").WithMessage("groupId", ComparisonMode.Substring);
        }
    }
}
