using System;
using FluentAssertions;
using Microsoft.Practices.Prism.Events;
using Moq;
using NUnit.Framework;
using RequestForQuoteInterfacesLibrary.EventPayloads;
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
        public void Constructor_NullGroupControllerProxy_ArgumentNullExceptionThrown()
        {
            // Act
            Action act = () => new GroupManagerImpl(configManagerMock.Object, eventAggregatorMock.Object, null);
            // Assert
            act.ShouldThrow<ArgumentNullException>("because event group controller proxy parameter cannot be null.").WithMessage("groupControllerProxy", ComparisonMode.Substring);
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
        public void AddGroup_InvalidEmailAddressParameter_ArgumentExceptionThrown(string groupName)
        {
            // Arrange
            IGroupManager groupManager = new GroupManagerImpl(configManagerMock.Object, eventAggregatorMock.Object, groupController.Object);
            // Act
            Action act = () => groupManager.AddGroup(1, groupName, true);
            // Assert
            act.ShouldThrow<ArgumentException>("because email Address parameter cannot be empty or null.").WithMessage("groupName", ComparisonMode.Substring);
        }

        [Test]
        public void AddGroup_ValidParameters_GroupsCollectionPopulated()
        {
            // Arrange
            IGroupManager groupManager = new GroupManagerImpl(configManagerMock.Object, eventAggregatorMock.Object, groupController.Object);
            // Act
            groupManager.AddGroup(1, "groupName", true);
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
            groupManager.AddGroup(1, "groupName", true);
            // Assert
            wasCalled.Should().BeTrue("because a new group is published to all listeners by the AddGroup method.");
        }

        [TestCase(null)]
        [TestCase("")]
        public void SaveToDatabase_InvalidGroupNameParameter_ArgumentExceptionThrown(String groupName)
        {
            // Arrange
            IGroupManager groupManager = new GroupManagerImpl(configManagerMock.Object, eventAggregatorMock.Object, groupController.Object);
            // Act
            Action act = () => groupManager.SaveToDatabase(groupName);
            // Assert
            act.ShouldThrow<ArgumentException>("because group name parameter cannot be empty or null.").WithMessage("groupName", ComparisonMode.Substring);
        }
    }
}
