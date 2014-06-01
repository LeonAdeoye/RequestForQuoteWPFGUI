using System;
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
            Assert.IsNotNull(groupManager.Groups, "because the constructor instantiates it.");
        }

        [Test]
        [ExpectedException("System.ArgumentNullException")]
        public void Constructor_NullConfigManager_ArgumentNullExceptionThrown()
        {
            // Act
            var test = new GroupManagerImpl(null, eventAggregatorMock.Object, groupController.Object);
        }

        [Test]
        [ExpectedException("System.ArgumentNullException")]
        public void Constructor_NullEventAggregator_ArgumentNullExceptionThrown()
        {
            // Act
            var test = new GroupManagerImpl(configManagerMock.Object, null, groupController.Object);
        }

        [Test]
        [ExpectedException("System.ArgumentNullException")]
        public void Constructor_NullGroupControllerProxy_ArgumentNullExceptionThrown()
        {
            // Act
            var test = new GroupManagerImpl(configManagerMock.Object, eventAggregatorMock.Object, null);
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
            Assert.IsNotEmpty(groupManager.Groups, "because it is populated in standalone mode.");
        }

        [TestCase(null)]
        [TestCase("")]
        [ExpectedException(typeof(ArgumentException), ExpectedMessage = "groupName")]
        public void AddGroup_InvalidEmailAddressParameter_ArgumentExceptionThrown(string groupName)
        {
            // Arrange
            IGroupManager groupManager = new GroupManagerImpl(configManagerMock.Object, eventAggregatorMock.Object, groupController.Object);
            // Act
            groupManager.AddGroup(1, groupName, true);
        }

        [Test]
        public void AddGroup_ValidParameters_GroupsCollectionPopulated()
        {
            // Arrange
            IGroupManager groupManager = new GroupManagerImpl(configManagerMock.Object, eventAggregatorMock.Object, groupController.Object);
            // Act
            groupManager.AddGroup(1, "groupName", true);
            // Assert           
           Assert.IsNotEmpty(groupManager.Groups, "because a new group is added to the collection by AddGroup");
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
            Assert.IsTrue(wasCalled, "because a new group is published to all listeners by the AddGroup method.");
        }

        [TestCase(null)]
        [TestCase("")]
        [ExpectedException(typeof(ArgumentException), ExpectedMessage = "groupName")]
        public void SaveToDatabase_InvalidGroupNameParameter_ArgumentExceptionThrown(String groupName)
        {
            // Arrange
            IGroupManager groupManager = new GroupManagerImpl(configManagerMock.Object, eventAggregatorMock.Object, groupController.Object);
            // Act
            groupManager.SaveToDatabase(groupName);
        }
    }
}
