using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Input;
using Microsoft.Practices.Prism.Events;
using Microsoft.Practices.ServiceLocation;
using RequestForQuoteInterfacesLibrary.Constants;
using RequestForQuoteInterfacesLibrary.Enums;
using RequestForQuoteInterfacesLibrary.EventPayloads;
using RequestForQuoteInterfacesLibrary.Events;
using RequestForQuoteInterfacesLibrary.ModelInterfaces;
using RequestForQuoteInterfacesLibrary.ServiceInterfaces;
using RequestForQuoteInterfacesLibrary.ViewModelInterfaces;
using RequestForQuoteMaintenanceModuleLibrary.Commands;
using log4net;

namespace RequestForQuoteMaintenanceModuleLibrary
{
    public sealed class ClientMaintenanceViewModel : DependencyObject, IUpdateValidityViewModel, IClearInputViewModel, IAddNewItemViewModel
    {
        private static readonly ILog log = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private readonly IEventAggregator eventAggregator = ServiceLocator.Current.GetInstance<IEventAggregator>();
        private readonly IClientManager clientManager = ServiceLocator.Current.GetInstance<IClientManager>();

        public ObservableCollection<IClient> Clients { get; set; }
        public List<string> Tiers { get; set; }
        public ICommand AddNewItemCommand { get; set; }
        public ICommand ClearInputCommand { get; set; }
        public ICommand UpdateValidityCommand { get; set; }

        public ClientMaintenanceViewModel()
        {
            AddNewItemCommand = new AddNewItemCommand(this);
            ClearInputCommand = new ClearInputCommand(this);
            UpdateValidityCommand = new UpdateValidityCommand(this);

            InitializeCollections();
            InitializeEventSubscriptions();
        }

        private void InitializeCollections()
        {
            Clients = new ObservableCollection<IClient>(clientManager.Clients);
            Tiers = new List<string>();
            foreach (var tier in Enum.GetNames(typeof(TierEnum)))
                Tiers.Add(tier);
        }

        private void InitializeEventSubscriptions()
        {
            eventAggregator.GetEvent<NewClientEvent>().Subscribe(HandleNewClientEvent, ThreadOption.UIThread, RequestForQuoteConstants.MAINTAIN_STRONG_REFERENCE);
        }

        public IClient SelectedClient
        {
            get { return (IClient)GetValue(SelectedClientProperty); }
            set { SetValue(SelectedClientProperty, value); }
        }

        // Using a DependencyProperty as the backing store for SelectedClient.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty SelectedClientProperty =
            DependencyProperty.Register("SelectedClient", typeof(IClient), typeof(ClientMaintenanceViewModel), new UIPropertyMetadata(null));
        
        public string NewClientName
        {
            get { return (string)GetValue(NewClientNameProperty); }
            set { SetValue(NewClientNameProperty, value); }
        }

        // Using a DependencyProperty as the backing store for NewClientName.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty NewClientNameProperty =
            DependencyProperty.Register("NewClientName", typeof(string), typeof(ClientMaintenanceViewModel), new UIPropertyMetadata(""));

        public string NewClientTier
        {
            get { return (string)GetValue(NewClientTierProperty); }
            set { SetValue(NewClientTierProperty, value); }
        }

        // Using a DependencyProperty as the backing store for NewClientTier.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty NewClientTierProperty =
            DependencyProperty.Register("NewClientTier", typeof(string), typeof(ClientMaintenanceViewModel), new UIPropertyMetadata(TierEnum.Top.ToString()));
              
        public void HandleNewClientEvent(NewClientEventPayload eventPayLoad)
        {
            if (log.IsDebugEnabled)
                log.Debug("Received new client event from ClientManager: " + eventPayLoad);

            Clients.Add(eventPayLoad.NewClient);
        }
     
        public void ClearInput()
        {
            NewClientName = "";
            NewClientTier = TierEnum.Top.ToString();
        }

        public bool CanClearInput()
        {
            return !string.IsNullOrEmpty(NewClientName);
        }

        public void AddNewItem()
        {
            if (!clientManager.Clients.Exists((client) => client.Name == NewClientName))
            {
                if (!String.IsNullOrEmpty(NewClientTier))
                {
                    if (clientManager.SaveToDatabase(NewClientName, NewClientTier))
                        ClearInput();
                    else
                        MessageBox.Show("Failed to add new client " + NewClientName +
                                        "perhaps due to a web service error! Pls consult log files.");
                }
                else
                    MessageBox.Show("Cannot add the client: " + NewClientName + " because client tier is invalid!",
                                    "Error adding client...", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            else
                MessageBox.Show("Cannot add the client: " + NewClientName + " because it already exists!",
                                "Error adding client...", MessageBoxButton.OK, MessageBoxImage.Error);
        }

        public bool CanAddNewItem()
        {
            // TODO the NewClientTier.HasValue part does not work when you select from the tier combobox.
            return !string.IsNullOrEmpty(NewClientName) && !String.IsNullOrEmpty(NewClientTier);    
        }

        public bool CanUpdateValidity(bool isRequestToMakeValid)
        {
            if (SelectedClient == null)
                return false;
            return (SelectedClient.IsValid != isRequestToMakeValid);
        }

        public void UpdateValidity()
        {
            if (clientManager.UpdateValidity(SelectedClient.Identifier, !SelectedClient.IsValid))
                SelectedClient.IsValid = !SelectedClient.IsValid;
            else
                MessageBox.Show("Failed to update validity of client " + SelectedClient.Name, "Client Maintenance Error",
                                MessageBoxButton.OK, MessageBoxImage.Error);
        }
    }
}
