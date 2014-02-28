using System;
using System.Collections.Generic;
using System.ServiceModel;
using Microsoft.Practices.Prism.Events;
using RequestForQuoteInterfacesLibrary.EventPayloads;
using RequestForQuoteInterfacesLibrary.Events;
using RequestForQuoteInterfacesLibrary.ModelImplementations;
using RequestForQuoteInterfacesLibrary.ModelInterfaces;
using RequestForQuoteInterfacesLibrary.ServiceInterfaces;
using RequestForQuoteServicesModuleLibrary.GroupMaintenanceService;
using log4net;

namespace RequestForQuoteServicesModuleLibrary.ServicesImplementation
{
    public class GroupManagerImpl : IGroupManager
    {
        private static readonly ILog log = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private readonly IEventAggregator eventAggregator;
        private readonly IConfigurationManager configManager;
        private readonly GroupControllerClient groupControllerProxy;
        public List<IGroup> Groups { get; set; }

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="configManager"> to get the standalone flag and current user</param>
        /// <param name="eventAggregator"> for publishing new group to other listeners</param>
        /// <param name="groupControllerProxy"> for group controller proxy</param>
        /// <exception cref="ArgumentNullException"> if configManager/eventAggregator is null</exception>
        public GroupManagerImpl(IConfigurationManager configManager, IEventAggregator eventAggregator, GroupControllerClient groupControllerProxy)
        {
            if (configManager == null)
                throw new ArgumentNullException("configManager");

            if (eventAggregator == null)
                throw new ArgumentNullException("eventAggregator");

            if (groupControllerProxy == null)
                throw new ArgumentNullException("groupControllerProxy");

            this.configManager = configManager;
            this.eventAggregator = eventAggregator;
            this.groupControllerProxy = groupControllerProxy;

            Groups = new List<IGroup>();
        }


        /// <summary>
        /// Adds the group to the collection of groups maintained by the group manager.
        /// Publishes details of this group to all listeners.
        /// </summary>
        /// <param name="groupId"> the identifier of the group that will be added.</param>
        /// <param name="groupName"> the name of the group that will be added.</param>
        /// <param name="isValid"> the validity of the group that will be added.</param>
        /// <exception cref="ArgumentException"> thrown if any of the string parameters are null or empty.</exception>
        public void AddGroup(int groupId, string groupName, bool isValid)
        {
            if (String.IsNullOrEmpty(groupName))
                throw new ArgumentException("groupName");

            var newGroup = new GroupImpl()
            {
                GroupId = groupId,
                GroupName = groupName,
                IsValid = isValid
            };

            Groups.Add(newGroup);

            eventAggregator.GetEvent<NewGroupEvent>().Publish(new NewGroupEventPayload()
            {
                NewGroup = newGroup
            });
        }

        /// <summary>
        /// Saves the new group to the database.
        /// </summary>
        /// <param name="groupName"> the name of the group that will be added.</param>
        /// <returns> true if the save was successful; false otherwise.</returns>
        /// <exception cref="ArgumentException"> thrown if groupName string parameter is null or empty.</exception>
        public bool SaveToDatabase(string groupName)
        {
            if (String.IsNullOrEmpty(groupName))
                throw new ArgumentException("groupName");

            return groupControllerProxy.save(groupName, configManager.CurrentUser);
        }

        /// <summary>
        /// Updates the validity of the group.
        /// </summary>
        /// <param name="groupId"> the group id of the group that will be changed.</param>
        /// <param name="isValid"> the validity of the group.</param>
        /// <returns> true if the update was successful; false otherwise.</returns>
        public bool UpdateValidity(int groupId, bool isValid)
        {
            return groupControllerProxy.updateValidity(groupId, isValid, configManager.CurrentUser);
        }

        /// <summary>
        /// Initializes the groups collection.
        /// </summary>
        public void Initialize()
        {
            if (configManager.IsStandAlone)
            {
                Groups.Add(new GroupImpl()
                {
                    GroupId = 1,
                    GroupName = "Hong Kong Flow Derivatives",
                    IsValid = true
                });
            }
            else
            {
                try
                {
                    var previouslySavedGroups = groupControllerProxy.getAll();
                    if (previouslySavedGroups == null)
                        return;

                    foreach (var group in previouslySavedGroups)
                    {
                        Groups.Add(new GroupImpl()
                        {
                            GroupId = group.groupId,
                            GroupName = group.groupName,
                            IsValid = group.isValid
                        });
                    }
                }
                catch (EndpointNotFoundException exception)
                {
                    if (log.IsErrorEnabled)
                        log.Error(String.Format("Failed to connect to proxy for remote search controller webservice. Exception thrown {0}", exception));
                    throw;
                }
                catch (FaultException fe)
                {
                    if (log.IsErrorEnabled)
                        log.Error("Exception thrown while initializing groups collection" + ": " + fe);
                    throw;
                }
            }
        }
    }
}
