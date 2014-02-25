using System;
using System.Collections.Generic;
using System.ServiceModel;
using Microsoft.Practices.Prism.Events;
using RequestForQuoteInterfacesLibrary.Enums;
using RequestForQuoteInterfacesLibrary.EventPayloads;
using RequestForQuoteInterfacesLibrary.Events;
using RequestForQuoteInterfacesLibrary.ModelImplementations;
using RequestForQuoteInterfacesLibrary.ModelInterfaces;
using RequestForQuoteInterfacesLibrary.ServiceInterfaces;
using RequestForQuoteServicesModuleLibrary.UserMaintenanceService;
using log4net;

namespace RequestForQuoteServicesModuleLibrary.ServicesImplementation
{
    public sealed class UserManagerImpl : IUserManager
    {
        private static readonly ILog log = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private readonly IEventAggregator eventAggregator;
        private readonly IConfigurationManager configManager;
        private readonly UserControllerClient userControllerProxy;
        public List<IUser> Users { get; set; }

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="configManager"> to get standalone flag and current user</param>
        /// <param name="eventAggregator"> for publishing new user to other listeners</param>
        /// <param name="userControllerProxy"> proxy for user web service operations</param>
        /// <exception cref="ArgumentNullException"> if configManager/eventAggregator is null</exception>
        public UserManagerImpl(IConfigurationManager configManager, IEventAggregator eventAggregator, UserControllerClient userControllerProxy)
        {
            if (configManager == null)
                throw new ArgumentNullException("configManager");

            if (eventAggregator == null)
                throw new ArgumentNullException("eventAggregator");

            if(userControllerProxy == null)
                throw new ArgumentNullException("userControllerProxy");

            this.configManager = configManager;
            this.eventAggregator = eventAggregator;
            this.userControllerProxy = userControllerProxy;

            Users = new List<IUser>();
        }

        /// <summary>
        /// Initializes the users collection.
        /// </summary>
        public void Initialize()
        {
            if (configManager.IsStandAlone)
            {
                Users.Add(new UserImpl() { UserId = "leon.adeoye", FirstName = "Leon" , LastName = "Adeoye", 
                    EmailAddress = "leon.adeoye@gmail.com", GroupId = 1, LocationName = LocationEnum.HONG_KONG, IsValid = true});               
            }
            else
            {
                try
                {
                    var previouslySavedUsers = userControllerProxy.getAll();
                    if (previouslySavedUsers == null)
                        return;

                    foreach (var user in previouslySavedUsers)
                    {
                        Users.Add(new UserImpl()
                            {
                                UserId = user.userId,
                                FirstName = user.firstName,
                                LastName = user.lastName,
                                EmailAddress = user.emailAddress,
                                LocationName = (LocationEnum) Enum.Parse(typeof(LocationEnum), user.locationName),
                                GroupId = user.groupId,
                                IsValid = user.isValid
                            });    
                    }
                }
                catch (EndpointNotFoundException exception)
                {
                    log.Error(String.Format("Failed to connect to proxy for remote search controller webservice. Exception thrown {0}", exception));
                    throw;
                }                
            }            
        }

        /// <summary>
        /// Updates the validity of the user.
        /// </summary>
        /// <param name="userId"> the user id of the user that will be changed.</param>
        /// <param name="isValid"> the validity of the user.</param>
        /// <returns> true if the update was successful; false otherwise.</returns>
        /// <exception cref="ArgumentException"> thrown if the userId parameter is null or empty.</exception>
        public bool UpdateValidity(string userId, bool isValid)
        {
            if (String.IsNullOrEmpty(userId))
                throw new ArgumentException("userId");

            return userControllerProxy.updateValidity(userId, isValid, configManager.CurrentUser);
        }

        /// <summary>
        /// Adds the user to the collection of users maintained by the user manager.
        /// Publishes details of this user to all listeners.
        /// </summary>
        /// <param name="userId"> the userId of the user that will be added.</param>
        /// <param name="firstName"> the first name of the user that will be added.</param>
        /// <param name="lastName"> the last name of the user that will be added.</param>
        /// <param name="emailAddress"> the email address of the user that will be added.</param>
        /// <param name="locationName"> the location of the user that will be added.</param>
        /// <param name="groupId"> the group of the user that will be added.</param>
        /// <param name="isValid"> the validity of the user that will be added.</param>
        /// <exception cref="ArgumentException"> thrown if any of the string parameters are null or empty.</exception>
        public void AddUser(string userId, string firstName, string lastName, string emailAddress, string locationName, int groupId, bool isValid)
        {
            if (String.IsNullOrEmpty(userId))
                throw new ArgumentException("userId");

            if (String.IsNullOrEmpty(firstName))
                throw new ArgumentException("firstName");

            if (String.IsNullOrEmpty(lastName))
                throw new ArgumentException("lastName");

            if (String.IsNullOrEmpty(emailAddress))
                throw new ArgumentException("emailAddress");

            LocationEnum parsedLocation = LocationEnum.HONG_KONG;
            if (String.IsNullOrEmpty(locationName) || !Enum.TryParse(locationName, true, out parsedLocation))
                throw new ArgumentException("locationName");

            var newUser = new UserImpl() 
            { 
                UserId = userId, 
                FirstName = firstName, 
                LastName = lastName, 
                EmailAddress = emailAddress, 
                LocationName = parsedLocation, 
                GroupId = groupId, 
                IsValid = isValid
            };

            Users.Add(newUser);

            eventAggregator.GetEvent<NewUserEvent>().Publish(new NewUserEventPayload()
            {
                NewUser = newUser
            });
        }

        /// <summary>
        /// Saves the new user to the database.
        /// </summary>
        /// <param name="userId"> the userId of the user that will be added.</param>
        /// <param name="firstName"> the first name of the user that will be added.</param>
        /// <param name="lastName"> the last name of the user that will be added.</param>
        /// <param name="emailAddress"> the email address of the user that will be added.</param>
        /// <param name="locationName"> the location of the user that will be added.</param>
        /// <param name="groupId"> the group of the user that will be added.</param>
        /// <returns> true if the save was successful; false otherwise.</returns>
        /// <exception cref="ArgumentException"> thrown if any of the string params are null or empty.</exception>
        public bool SaveToDatabase(string userId, string firstName, string lastName, string emailAddress, string locationName, int groupId)
        {
            if (String.IsNullOrEmpty(userId))
                throw new ArgumentException("userId");

            if (String.IsNullOrEmpty(firstName))
                throw new ArgumentException("firstName");

            if (String.IsNullOrEmpty(lastName))
                throw new ArgumentException("lastName");

            if (String.IsNullOrEmpty(emailAddress))
                throw new ArgumentException("emailAddress");

            if (String.IsNullOrEmpty(locationName))
                throw new ArgumentException("locationName");

            return userControllerProxy.save(userId, firstName, lastName, emailAddress, locationName, groupId,  configManager.CurrentUser);
        }
    }
}
