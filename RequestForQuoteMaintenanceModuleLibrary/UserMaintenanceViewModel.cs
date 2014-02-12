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
    public sealed class UserMaintenanceViewModel : DependencyObject, IUpdateValidityViewModel, IClearInputViewModel, IAddNewItemViewModel
    {
        private static readonly ILog log = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private readonly IEventAggregator eventAggregator;
        private readonly IUserManager userManager;

        public ObservableCollection<IUser> Users { get; set; }
        public ICommand AddNewItemCommand { get; set; }
        public ICommand ClearInputCommand { get; set; }
        public ICommand UpdateValidityCommand { get; set; }

        public UserMaintenanceViewModel(IUserManager userManager, IEventAggregator eventAggregator)
        {
            if (userManager == null)
                throw new ArgumentNullException("userManager");

            if (eventAggregator == null)
                throw new ArgumentNullException("eventAggregator");

            this.userManager = userManager;
            this.eventAggregator = eventAggregator;

            AddNewItemCommand = new AddNewItemCommand(this);
            ClearInputCommand = new ClearInputCommand(this);
            UpdateValidityCommand = new UpdateValidityCommand(this);

            InitializeCollections();
            InitializeEventSubscriptions();
        }

        private void InitializeCollections()
        {
            Users = new ObservableCollection<IUser>(userManager.Users);
        }

        private void InitializeEventSubscriptions()
        {
            eventAggregator.GetEvent<NewUserEvent>().Subscribe(HandleNewUserEvent, ThreadOption.UIThread, RequestForQuoteConstants.MAINTAIN_STRONG_REFERENCE);
        }

        public IUser SelectedUser
        {
            get { return (IUser)GetValue(SelectedUserProperty); }
            set { SetValue(SelectedUserProperty, value); }
        }

        // Using a DependencyProperty as the backing store for SelectedUnderlyier.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty SelectedUserProperty =
            DependencyProperty.Register("SelectedUser", typeof(IUser), typeof(UserMaintenanceViewModel), new UIPropertyMetadata(null));

        public string NewUserId
        {
            get { return (string)GetValue(NewUserIdProperty); }
            set { SetValue(NewUserIdProperty, value); }
        }

        // Using a DependencyProperty as the backing store for NewUserRIC.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty NewUserIdProperty =
            DependencyProperty.Register("NewUserId", typeof(string), typeof(UserMaintenanceViewModel), new UIPropertyMetadata(String.Empty));

        public string FirstName
        {
            get { return (string)GetValue(FirstNameProperty); }
            set { SetValue(FirstNameProperty, value); }
        }

        // Using a DependencyProperty as the backing store for FirstName.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty FirstNameProperty =
            DependencyProperty.Register("FirstName", typeof(string), typeof(UserMaintenanceViewModel), new UIPropertyMetadata(String.Empty));

        public string LastName
        {
            get { return (string)GetValue(LastNameProperty); }
            set { SetValue(LastNameProperty, value); }
        }

        // Using a DependencyProperty as the backing store for LastName.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty LastNameProperty =
            DependencyProperty.Register("LastName", typeof(string), typeof(UserMaintenanceViewModel), new UIPropertyMetadata(String.Empty));

        public string EmailAddress
        {
            get { return (string)GetValue(EmailAddressProperty); }
            set { SetValue(EmailAddressProperty, value); }
        }

        // Using a DependencyProperty as the backing store for EmailAddress.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty EmailAddressProperty =
            DependencyProperty.Register("EmailAddress", typeof(string), typeof(UserMaintenanceViewModel), new UIPropertyMetadata(String.Empty));


        public int GroupId
        {
            get { return (int)GetValue(GroupIdProperty); }
            set { SetValue(GroupIdProperty, value); }
        }

        // Using a DependencyProperty as the backing store for GroupId.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty GroupIdProperty =
            DependencyProperty.Register("GroupId", typeof(int), typeof(UserMaintenanceViewModel), new UIPropertyMetadata(-1));

        public string LocationName
        {
            get { return (string)GetValue(LocationNameProperty); }
            set { SetValue(LocationNameProperty, value); }
        }

        // Using a DependencyProperty as the backing store for LocationName.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty LocationNameProperty =
            DependencyProperty.Register("LocationName", typeof(string), typeof(UserMaintenanceViewModel), new UIPropertyMetadata(String.Empty));
                        
        public void HandleNewUserEvent(NewUserEventPayload eventPayLoad)
        {
            if (log.IsDebugEnabled)
                log.Debug("Received new user event from UserManager: " + eventPayLoad);

            Users.Add(eventPayLoad.NewUser);
        }

        public void ClearInput()
        {
            NewUserId = String.Empty;
            FirstName = String.Empty;
            LastName = String.Empty;
            EmailAddress = String.Empty;
            GroupId = -1;
            LocationName = String.Empty;
        }

        public bool CanClearInput()
        {
            return !string.IsNullOrEmpty(NewUserId) || !string.IsNullOrEmpty(FirstName) || !string.IsNullOrEmpty(LastName)
            || !string.IsNullOrEmpty(EmailAddress) || !string.IsNullOrEmpty(LocationName);
        }

        public void AddNewItem()
        {
            if (!userManager.Users.Exists((user) => user.UserId == NewUserId))
            {
                if (userManager.SaveToDatabase(NewUserId, FirstName, LastName, EmailAddress, LocationName, GroupId))
                {
                    MessageBox.Show("Successfully saved new user: " + NewUserId, "User Maintenance",
                                    MessageBoxButton.OK, MessageBoxImage.Information);
                    ClearInput();
                }
                else
                    MessageBox.Show("Failed to save user: " + NewUserId, "User Maintenance Error",
                                    MessageBoxButton.OK, MessageBoxImage.Error);
            }
            else
                MessageBox.Show("Cannot Save! User: " + NewUserId + " already exists!", "User Maintenance Error",
                                MessageBoxButton.OK, MessageBoxImage.Error);
        }

        public bool CanAddNewItem()
        {
            return !String.IsNullOrEmpty(NewUserId) && !String.IsNullOrEmpty(FirstName) && !String.IsNullOrEmpty(LastName)
                && !String.IsNullOrEmpty(EmailAddress) && !String.IsNullOrEmpty(LocationName);
        }

        public bool CanUpdateValidity(bool isRequestToMakeValid)
        {
            if (SelectedUser == null)
                return false;

            return (SelectedUser.IsValid != isRequestToMakeValid);
        }

        public void UpdateValidity()
        {
            if (userManager.UpdateValidity(SelectedUser.UserId, !SelectedUser.IsValid))
                SelectedUser.IsValid = !SelectedUser.IsValid;
            else
                MessageBox.Show("Failed to update validity of User " + SelectedUser.UserId, "User Maintenance Error",
                                MessageBoxButton.OK, MessageBoxImage.Error);
        }
    }
}
