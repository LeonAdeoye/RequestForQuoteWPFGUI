using System;
using Microsoft.Practices.Prism.Events;
using Moq;
using NUnit.Framework;
using RequestForQuoteInterfacesLibrary.EventPayloads;
using RequestForQuoteInterfacesLibrary.Events;
using RequestForQuoteInterfacesLibrary.ServiceInterfaces;
using RequestForQuoteServicesModuleLibrary.ServicesImplementation;
using RequestForQuoteServicesModuleLibrary.UserMaintenanceService;

namespace RequestForQuoteServicesModuleLibrary.Test
{
    public class UserManagerTest
    {
        private readonly Mock<UserControllerClient> userController = new Mock<UserControllerClient>();
        private readonly Mock<IEventAggregator> eventAggregatorMock = new Mock<IEventAggregator>();
        private readonly Mock<IConfigurationManager> configManagerMock = new Mock<IConfigurationManager>();
        private readonly Mock<NewUserEvent> newUserEventMock = new Mock<NewUserEvent>();

        [TestFixtureSetUp]
        public void TestFixtureSetUp()
        {
            eventAggregatorMock.Setup(p => p.GetEvent<NewUserEvent>()).Returns(newUserEventMock.Object);
        }

        [Test]
        public void Constructor_ValidParameters_UsersCollectionInstaniated()
        {
            // Act
            IUserManager userManager = new UserManagerImpl(configManagerMock.Object, eventAggregatorMock.Object, userController.Object);
            // Assert
            Assert.IsNotNull(userManager.Users, "because the constructor instantiates it.");
        }

        [Test]
        [ExpectedException("System.ArgumentNullException")]
        public void Constructor_NullConfigManager_ArgumentNullExceptionThrown()
        {
            // Act
            var test = new UserManagerImpl(null, eventAggregatorMock.Object, userController.Object);
        }

        [Test]
        [ExpectedException("System.ArgumentNullException")]
        public void Constructor_NullEventAggregator_ArgumentNullExceptionThrown()
        {
            // Act
            var test = new UserManagerImpl(configManagerMock.Object, null, userController.Object);
        }

        [Test]
        public void Initialize_StandAloneMode_UsersCollectionPopulated()
        {
            // Arrange
            IUserManager userManager = new UserManagerImpl(configManagerMock.Object, eventAggregatorMock.Object, userController.Object);
            configManagerMock.Setup(cm => cm.IsStandAlone).Returns(true);
            // Act
            userManager.Initialize();
            // Assert            
            Assert.IsNotEmpty(userManager.Users, "because it is populated in standalone mode.");
        }

        [TestCase(null)]
        [TestCase("")]
        [ExpectedException(typeof(ArgumentException), ExpectedMessage = "userId")]
        public void AddUser_InvalidUserCodeParameter_ArgumentExceptionThrown(string userId)
        {
            // Arrange
            IUserManager userManager = new UserManagerImpl(configManagerMock.Object, eventAggregatorMock.Object, userController.Object);
            // Act
            userManager.AddUser(userId, "firstName", "lastName", "emailAddress", "locationName", 1, true);
        }

        [TestCase(null)]
        [TestCase("")]
        [ExpectedException(typeof(ArgumentException), ExpectedMessage = "firstName")]
        public void AddUser_InvalidFirstNameParameter_ArgumentExceptionThrown(string firstName)
        {
            // Arrange
            IUserManager userManager = new UserManagerImpl(configManagerMock.Object, eventAggregatorMock.Object, userController.Object);
            // Act
            userManager.AddUser("testUserId", firstName, "lastName", "emailAddress", "locationName", 1, true);
        }

        [TestCase(null)]
        [TestCase("")]
        [ExpectedException(typeof(ArgumentException), ExpectedMessage = "lastName")]
        public void AddUser_InvalidLastNameParameter_ArgumentExceptionThrown(string lastName)
        {
            // Arrange
            IUserManager userManager = new UserManagerImpl(configManagerMock.Object, eventAggregatorMock.Object, userController.Object);
            // Act
            userManager.AddUser("testUserId", "firstName", lastName, "emailAddress", "locationName", 1, true);
        }

        [TestCase(null)]
        [TestCase("")]
        [ExpectedException(typeof(ArgumentException), ExpectedMessage = "emailAddress")]
        public void AddUser_InvalidEmailAddressParameter_ArgumentExceptionThrown(string emailAddress)
        {
            // Arrange
            IUserManager userManager = new UserManagerImpl(configManagerMock.Object, eventAggregatorMock.Object, userController.Object);
            // Act
            userManager.AddUser("testUserId", "firstName", "lastName", emailAddress, "Tokyo", 1, true);
        }

        [TestCase(null)]
        [TestCase("")]
        [ExpectedException(typeof(ArgumentException), ExpectedMessage = "locationName")]
        public void AddUser_InvalidLocationNameParameter_ArgumentExceptionThrown(string locationName)
        {
            // Arrange
            IUserManager userManager = new UserManagerImpl(configManagerMock.Object, eventAggregatorMock.Object, userController.Object);
            // Act
            userManager.AddUser("testUserId", "firstName", "lastName", "emailAddress", locationName, 1, true);
        }

        [Test]
        public void AddUser_ValidParameters_UsersCollectionPopulated()
        {
            // Arrange
            IUserManager userManager = new UserManagerImpl(configManagerMock.Object, eventAggregatorMock.Object, userController.Object);
            // Act
            userManager.AddUser("testUserId", "firstName", "lastName", "emailAddress", "Tokyo", 1, true);
            // Assert
            Assert.IsNotEmpty(userManager.Users, "because a new user is added to the collection by AddUser");
        }

        [Test]
        public void AddUser_ValidParameters_NewUserEventShouldBePublished()
        {
            // Arrange
            IUserManager userManager = new UserManagerImpl(configManagerMock.Object, eventAggregatorMock.Object, userController.Object);
            var wasCalled = false;
            eventAggregatorMock.Setup(ea => ea.GetEvent<NewUserEvent>().Publish(It.IsAny<NewUserEventPayload>())).Callback(() => wasCalled = true);
            // Act
            userManager.AddUser("testUserId", "firstName", "lastName", "emailAddress", "Tokyo", 1, true);
            // Assert
            Assert.IsTrue(wasCalled, "because a new user is published to all listeners by the AddUser method.");
            
        }

        [TestCase(null)]
        [TestCase("")]
        [ExpectedException(typeof(ArgumentException), ExpectedMessage = "userId")]
        public void SaveToDatabase_InvalidUserCodeParameter_ArgumentExceptionThrown(String userId)
        {
            // Arrange
            IUserManager userManager = new UserManagerImpl(configManagerMock.Object, eventAggregatorMock.Object, userController.Object);
            // Act
            userManager.SaveToDatabase(userId, "firstName", "lastName", "emailAddress", "Tokyo", 1);
        }

        [TestCase(null)]
        [TestCase("")]
        [ExpectedException(typeof(ArgumentException), ExpectedMessage = "firstName")]
        public void SaveToDatabase_InvalidFirstNameParameter_ArgumentExceptionThrown(String firstName)
        {
            // Arrange
            IUserManager userManager = new UserManagerImpl(configManagerMock.Object, eventAggregatorMock.Object, userController.Object);
            // Act
            userManager.SaveToDatabase("testUserId", firstName, "lastName", "emailAddress", "Tokyo", 1);
        }

        [TestCase(null)]
        [TestCase("")]
        [ExpectedException(typeof(ArgumentException), ExpectedMessage = "lastName")]
        public void SaveToDatabase_InvalidLastNameParameter_ArgumentExceptionThrown(String lastName)
        {
            // Arrange
            IUserManager userManager = new UserManagerImpl(configManagerMock.Object, eventAggregatorMock.Object, userController.Object);
            // Act
            userManager.SaveToDatabase("testUserId", "firstName", lastName, "emailAddress", "Tokyo", 1);
        }

        [TestCase(null)]
        [TestCase("")]
        [ExpectedException(typeof(ArgumentException), ExpectedMessage = "emailAddress")]
        public void SaveToDatabase_InvalidEmailAddressParameter_ArgumentExceptionThrown(String emailAddress)
        {
            // Arrange
            IUserManager userManager = new UserManagerImpl(configManagerMock.Object, eventAggregatorMock.Object, userController.Object);
            // Act
            userManager.SaveToDatabase("testUserId", "firstName", "lastName", emailAddress, "Tokyo", 1);
        }

        [TestCase(null)]
        [TestCase("")]
        [ExpectedException(typeof(ArgumentException), ExpectedMessage = "locationName")]
        public void SaveToDatabase_InvalidLocationNameParameter_ArgumentExceptionThrown(String locationName)
        {
            // Arrange
            IUserManager userManager = new UserManagerImpl(configManagerMock.Object, eventAggregatorMock.Object, userController.Object);
            // Act
            userManager.SaveToDatabase("testUserId", "firstName", "lastName", "emailAddress", locationName, 1);
        }

        [TestCase(null)]
        [TestCase("")]
        [ExpectedException(typeof(ArgumentException), ExpectedMessage = "userId")]
        public void UpdateValidity_InvalidUserCodeParameter_ArgumentExceptionThrown(String userId)
        {
            // Arrange
            IUserManager userManager = new UserManagerImpl(configManagerMock.Object, eventAggregatorMock.Object, userController.Object);
            // Act
            userManager.UpdateValidity(userId, true);
        }
    }
}
