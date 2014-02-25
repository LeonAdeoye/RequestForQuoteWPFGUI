using System;
using FluentAssertions;
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
            userManager.Users.Should().NotBeNull("because the constructor instantiates it.");
        }

        [Test]
        public void Constructor_NullConfigManager_ArgumentNullExceptionThrown()
        {
            // Act
            Action act = () => new UserManagerImpl(null, eventAggregatorMock.Object, userController.Object);
            // Assert
            act.ShouldThrow<ArgumentNullException>("because configManager parameter cannot be null").WithMessage("configManager", ComparisonMode.Substring);
        }

        [Test]
        public void Constructor_NullEventAggregator_ArgumentNullExceptionThrown()
        {
            // Act
            Action act = () => new UserManagerImpl(configManagerMock.Object, null, userController.Object);
            // Assert
            act.ShouldThrow<ArgumentNullException>("because event aggregator parameter cannot be null.").WithMessage("eventAggregator", ComparisonMode.Substring);
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
            userManager.Users.Should().NotBeEmpty("because it is populated in standalone mode.");
        }

        [TestCase(null)]
        [TestCase("")]
        public void AddUser_InvalidUserCodeParameter_ArgumentExceptionThrown(string userId)
        {
            // Arrange
            IUserManager userManager = new UserManagerImpl(configManagerMock.Object, eventAggregatorMock.Object, userController.Object);
            // Act
            Action act = () => userManager.AddUser(userId, "firstName", "lastName", "emailAddress", "locationName", 1, true);
            // Assert
            act.ShouldThrow<ArgumentException>("because user code parameter cannot be empty or null.").WithMessage("userId", ComparisonMode.Substring);
        }

        [TestCase(null)]
        [TestCase("")]
        public void AddUser_InvalidFirstNameParameter_ArgumentExceptionThrown(string firstName)
        {
            // Arrange
            IUserManager userManager = new UserManagerImpl(configManagerMock.Object, eventAggregatorMock.Object, userController.Object);
            // Act
            Action act = () => userManager.AddUser("testUserId", firstName, "lastName", "emailAddress", "locationName", 1, true);
            // Assert
            act.ShouldThrow<ArgumentException>("because firstName parameter cannot be empty or null.").WithMessage("firstName", ComparisonMode.Substring);
        }

        [TestCase(null)]
        [TestCase("")]
        public void AddUser_InvalidLastNameParameter_ArgumentExceptionThrown(string lastName)
        {
            // Arrange
            IUserManager userManager = new UserManagerImpl(configManagerMock.Object, eventAggregatorMock.Object, userController.Object);
            // Act
            Action act = () => userManager.AddUser("testUserId", "firstName", lastName, "emailAddress", "locationName", 1, true);
            // Assert
            act.ShouldThrow<ArgumentException>("because lastName parameter cannot be empty or null.").WithMessage("lastName", ComparisonMode.Substring);
        }

        [TestCase(null)]
        [TestCase("")]
        public void AddUser_InvalidEmailAddressParameter_ArgumentExceptionThrown(string emailAddress)
        {
            // Arrange
            IUserManager userManager = new UserManagerImpl(configManagerMock.Object, eventAggregatorMock.Object, userController.Object);
            // Act
            Action act = () => userManager.AddUser("testUserId", "firstName", "lastName", emailAddress, "locationName", 1, true);
            // Assert
            act.ShouldThrow<ArgumentException>("because email Address parameter cannot be empty or null.").WithMessage("emailAddress", ComparisonMode.Substring);
        }

        [TestCase(null)]
        [TestCase("")]
        public void AddUser_InvalidLocationNameParameter_ArgumentExceptionThrown(string locationName)
        {
            // Arrange
            IUserManager userManager = new UserManagerImpl(configManagerMock.Object, eventAggregatorMock.Object, userController.Object);
            // Act
            Action act = () => userManager.AddUser("testUserId", "firstName", "lastName", "emailAddress", locationName, 1, true);
            // Assert
            act.ShouldThrow<ArgumentException>("because location name parameter cannot be empty or null.").WithMessage("locationName", ComparisonMode.Substring);
        }

        [Test]
        public void AddUser_ValidParameters_UsersCollectionPopulated()
        {
            // Arrange
            IUserManager userManager = new UserManagerImpl(configManagerMock.Object, eventAggregatorMock.Object, userController.Object);
            // Act
            userManager.AddUser("testUserId", "firstName", "lastName", "emailAddress", "locationName", 1, true);
            // Assert
            userManager.Users.Should().NotBeEmpty("because a new user is added to the collection by AddUser");
        }

        [Test]
        public void AddUser_ValidParameters_NewUserEventShouldBePublished()
        {
            // Arrange
            IUserManager userManager = new UserManagerImpl(configManagerMock.Object, eventAggregatorMock.Object, userController.Object);
            var wasCalled = false;
            eventAggregatorMock.Setup(ea => ea.GetEvent<NewUserEvent>().Publish(It.IsAny<NewUserEventPayload>())).Callback(() => wasCalled = true);
            // Act
            userManager.AddUser("testUserId", "firstName", "lastName", "emailAddress", "locationName", 1, true);
            // Assert
            wasCalled.Should().BeTrue("because a new user is published to all listeners by the AddUser method.");
        }

        [TestCase(null)]
        [TestCase("")]
        public void SaveToDatabase_InvalidUserCodeParameter_ArgumentExceptionThrown(String userId)
        {
            // Arrange
            IUserManager userManager = new UserManagerImpl(configManagerMock.Object, eventAggregatorMock.Object, userController.Object);
            // Act
            Action act = () => userManager.SaveToDatabase(userId, "firstName", "lastName", "emailAddress", "locationName", 1);
            // Assert
            act.ShouldThrow<ArgumentException>("because user id parameter cannot be empty or null.").WithMessage("userId", ComparisonMode.Substring);
        }

        [TestCase(null)]
        [TestCase("")]
        public void SaveToDatabase_InvalidFirstNameParameter_ArgumentExceptionThrown(String firstName)
        {
            // Arrange
            IUserManager userManager = new UserManagerImpl(configManagerMock.Object, eventAggregatorMock.Object, userController.Object);
            // Act
            Action act = () => userManager.SaveToDatabase("testUserId", firstName, "lastName", "emailAddress", "locationName", 1);
            // Assert
            act.ShouldThrow<ArgumentException>("because first name parameter cannot be empty or null.").WithMessage("firstName", ComparisonMode.Substring);
        }

        [TestCase(null)]
        [TestCase("")]
        public void SaveToDatabase_InvalidLastNameParameter_ArgumentExceptionThrown(String lastName)
        {
            // Arrange
            IUserManager userManager = new UserManagerImpl(configManagerMock.Object, eventAggregatorMock.Object, userController.Object);
            // Act
            Action act = () => userManager.SaveToDatabase("testUserId", "firstName", lastName, "emailAddress", "locationName", 1);
            // Assert
            act.ShouldThrow<ArgumentException>("because last name parameter cannot be empty or null.").WithMessage("lastName", ComparisonMode.Substring);
        }

        [TestCase(null)]
        [TestCase("")]
        public void SaveToDatabase_InvalidEmailAddressParameter_ArgumentExceptionThrown(String emailAddress)
        {
            // Arrange
            IUserManager userManager = new UserManagerImpl(configManagerMock.Object, eventAggregatorMock.Object, userController.Object);
            // Act
            Action act = () => userManager.SaveToDatabase("testUserId", "firstName", "lastName", emailAddress, "locationName", 1);
            // Assert
            act.ShouldThrow<ArgumentException>("because last email address cannot be empty or null.").WithMessage("emailAddress", ComparisonMode.Substring);
        }

        [TestCase(null)]
        [TestCase("")]
        public void SaveToDatabase_InvalidLocationNameParameter_ArgumentExceptionThrown(String locationName)
        {
            // Arrange
            IUserManager userManager = new UserManagerImpl(configManagerMock.Object, eventAggregatorMock.Object, userController.Object);
            // Act
            Action act = () => userManager.SaveToDatabase("testUserId", "firstName", "lastName", "emailAddress", locationName, 1);
            // Assert
            act.ShouldThrow<ArgumentException>("because location name cannot be empty or null.").WithMessage("locationName", ComparisonMode.Substring);
        }

        [TestCase(null)]
        [TestCase("")]
        public void UpdateValidity_InvalidUserCodeParameter_ArgumentExceptionThrown(String userId)
        {
            // Arrange
            IUserManager userManager = new UserManagerImpl(configManagerMock.Object, eventAggregatorMock.Object, userController.Object);
            // Act
            Action act = () => userManager.UpdateValidity(userId, true);
            // Assert
            act.ShouldThrow<ArgumentException>("because user id parameter cannot be empty or null.").WithMessage("userId", ComparisonMode.Substring);
        }
    }
}
