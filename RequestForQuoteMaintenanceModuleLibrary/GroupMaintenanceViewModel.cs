using System;
using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Input;
using Microsoft.Practices.Prism.Events;
using RequestForQuoteInterfacesLibrary.Constants;
using RequestForQuoteInterfacesLibrary.EventPayloads;
using RequestForQuoteInterfacesLibrary.Events;
using RequestForQuoteInterfacesLibrary.ModelInterfaces;
using RequestForQuoteInterfacesLibrary.ServiceInterfaces;
using RequestForQuoteInterfacesLibrary.ViewModelInterfaces;
using RequestForQuoteMaintenanceModuleLibrary.Commands;
using log4net;

namespace RequestForQuoteMaintenanceModuleLibrary
{
    public sealed class GroupMaintenanceViewModel : DependencyObject, IUpdateValidityViewModel, IClearInputViewModel, IAddNewItemViewModel
    {
        private static readonly ILog log = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private readonly IEventAggregator eventAggregator;
        private readonly IGroupManager groupManager;

        public ObservableCollection<IGroup> Groups { get; set; }
        public ICommand AddNewItemCommand { get; set; }
        public ICommand ClearInputCommand { get; set; }
        public ICommand UpdateValidityCommand { get; set; }

        public GroupMaintenanceViewModel(IGroupManager groupManager, IEventAggregator eventAggregator)
        {
            if (groupManager == null)
                throw new ArgumentNullException("groupManager");

            if (eventAggregator == null)
                throw new ArgumentNullException("eventAggregator");

            this.groupManager = groupManager;
            this.eventAggregator = eventAggregator;

            AddNewItemCommand = new AddNewItemCommand(this);
            ClearInputCommand = new ClearInputCommand(this);
            UpdateValidityCommand = new UpdateValidityCommand(this);

            InitializeCollections();
            InitializeEventSubscriptions();
        }

        private void InitializeCollections()
        {
            Groups = new ObservableCollection<IGroup>(groupManager.Groups);
        }

        private void InitializeEventSubscriptions()
        {
            eventAggregator.GetEvent<NewGroupEvent>().Subscribe(HandleNewGroupEvent, ThreadOption.UIThread, RequestForQuoteConstants.MAINTAIN_STRONG_REFERENCE);
        }

        public IGroup SelectedGroup
        {
            get { return (IGroup)GetValue(SelectedGroupProperty); }
            set { SetValue(SelectedGroupProperty, value); }
        }

        // Using a DependencyProperty as the backing store for SelectedUnderlyier.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty SelectedGroupProperty =
            DependencyProperty.Register("SelectedGroup", typeof(IGroup), typeof(GroupMaintenanceViewModel), new UIPropertyMetadata(null));

        public string NewGroupName
        {
            get { return (string)GetValue(NewGroupNameProperty); }
            set { SetValue(NewGroupNameProperty, value); }
        }

        // Using a DependencyProperty as the backing store for NewGroupRIC.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty NewGroupNameProperty =
            DependencyProperty.Register("NewGroupName", typeof(string), typeof(GroupMaintenanceViewModel), new UIPropertyMetadata(String.Empty));

        public void HandleNewGroupEvent(NewGroupEventPayload eventPayLoad)
        {
            if (log.IsDebugEnabled)
                log.Debug("Received new group event from GroupManager: " + eventPayLoad);

            Groups.Add(eventPayLoad.NewGroup);
        }

        public void ClearInput()
        {
            NewGroupName = "";
        }

        public bool CanClearInput()
        {
            return !string.IsNullOrEmpty(NewGroupName);
        }

        public void AddNewItem()
        {
            if (!groupManager.Groups.Exists((group) => group.GroupName == NewGroupName))
            {
                if (groupManager.SaveToDatabase(NewGroupName))
                {
                    MessageBox.Show("Successfully saved new Group: " + NewGroupName, "Group Maintenance",
                                    MessageBoxButton.OK, MessageBoxImage.Information);
                    ClearInput();
                }
                else
                    MessageBox.Show("Failed to save Group: " + NewGroupName, "Group Maintenance Error",
                                    MessageBoxButton.OK, MessageBoxImage.Error);
            }
            else
                MessageBox.Show("Cannot Save! Group: " + NewGroupName + " already exists!", "Group Maintenance Error",
                                MessageBoxButton.OK, MessageBoxImage.Error);
        }

        public bool CanAddNewItem()
        {
            return !String.IsNullOrEmpty(NewGroupName);
        }

        public bool CanUpdateValidity(bool isRequestToMakeValid)
        {
            if (SelectedGroup == null)
                return false;

            return (SelectedGroup.IsValid != isRequestToMakeValid);
        }

        public void UpdateValidity()
        {
            if (groupManager.UpdateValidity(SelectedGroup.GroupId, !SelectedGroup.IsValid))
                SelectedGroup.IsValid = !SelectedGroup.IsValid;
            else
                MessageBox.Show("Failed to update validity of Group " + SelectedGroup.GroupId, "Group Maintenance Error",
                                MessageBoxButton.OK, MessageBoxImage.Error);
        }
    }
}
