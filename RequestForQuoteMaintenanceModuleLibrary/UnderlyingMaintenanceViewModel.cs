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
    public sealed class UnderlyingMaintenanceViewModel : DependencyObject, IUpdateValidityViewModel, IClearInputViewModel, IAddNewItemViewModel
    {
        private static readonly ILog log = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private readonly IEventAggregator eventAggregator;
        private readonly IUnderlyingManager underlyingManager;

        public ObservableCollection<IUnderlying> Underlyings { get; set; }
        public ICommand AddNewItemCommand { get; set; }
        public ICommand ClearInputCommand { get; set; }
        public ICommand UpdateValidityCommand { get; set; }

        public UnderlyingMaintenanceViewModel(IUnderlyingManager underlyingManager, IEventAggregator eventAggregator)
        {
            if (underlyingManager == null)
                throw new ArgumentNullException("underlyingManager");

            if (eventAggregator == null)
                throw new ArgumentNullException("eventAggregator");

            this.underlyingManager = underlyingManager;
            this.eventAggregator = eventAggregator;

            AddNewItemCommand = new AddNewItemCommand(this);
            ClearInputCommand = new ClearInputCommand(this);
            UpdateValidityCommand = new UpdateValidityCommand(this);

            InitializeCollections();
            InitializeEventSubscriptions();
        }

        private void InitializeCollections()
        {
            Underlyings = new ObservableCollection<IUnderlying>(underlyingManager.Underlyings);
        }

        private void InitializeEventSubscriptions()
        {
            eventAggregator.GetEvent<NewUnderlyierEvent>().Subscribe(HandleNewUnderlyierEvent, ThreadOption.UIThread, RequestForQuoteConstants.MAINTAIN_STRONG_REFERENCE);
        }

        public IUnderlying SelectedUnderlying
        {
            get { return (IUnderlying)GetValue(SelectedUnderlyingProperty); }
            set { SetValue(SelectedUnderlyingProperty, value); }
        }

        // Using a DependencyProperty as the backing store for SelectedUnderlyier.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty SelectedUnderlyingProperty =
            DependencyProperty.Register("SelectedUnderlying", typeof(IUnderlying), typeof(UnderlyingMaintenanceViewModel), new UIPropertyMetadata(null));

        public string NewUnderlyingRIC
        {
            get { return (string)GetValue(NewUnderlyingRICProperty); }
            set { SetValue(NewUnderlyingRICProperty, value); }
        }

        // Using a DependencyProperty as the backing store for NewUnderlyingRIC.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty NewUnderlyingRICProperty =
            DependencyProperty.Register("NewUnderlyingRIC", typeof(string), typeof(UnderlyingMaintenanceViewModel), new UIPropertyMetadata(String.Empty));

        public string NewUnderlyingDescription
        {
            get { return (string)GetValue(NewUnderlyingDescriptionProperty); }
            set { SetValue(NewUnderlyingDescriptionProperty, value); }
        }

        // Using a DependencyProperty as the backing store for NewUnderlyingDescription.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty NewUnderlyingDescriptionProperty =
            DependencyProperty.Register("NewUnderlyingDescription", typeof(string), typeof(UnderlyingMaintenanceViewModel), new UIPropertyMetadata(String.Empty));
            
        public void HandleNewUnderlyierEvent(NewUnderlyierEventPayload eventPayLoad)
        {
            if (log.IsDebugEnabled)
                log.Debug("Received new underlyier event from UnderlyingManager: " + eventPayLoad);

            Underlyings.Add(eventPayLoad.NewUnderlying);
        }
     
        public void ClearInput()
        {
            NewUnderlyingRIC = "";
            NewUnderlyingDescription = "";
        }

        public bool CanClearInput()
        {
            return !string.IsNullOrEmpty(NewUnderlyingRIC) || !string.IsNullOrEmpty(NewUnderlyingDescription);
        }

        public void AddNewItem()
        {
            if (!underlyingManager.Underlyings.Exists((underlyier) => underlyier.RIC == NewUnderlyingRIC))
            {
                if (underlyingManager.SaveToDatabase(NewUnderlyingRIC, NewUnderlyingDescription))
                {
                    MessageBox.Show("Successfully saved new underlying " + NewUnderlyingRIC, "Underlying Maintenance",
                                    MessageBoxButton.OK, MessageBoxImage.Information);
                    ClearInput();
                }
                else
                    MessageBox.Show("Failed to save underlying " + NewUnderlyingRIC, "Underlying Maintenance Error",
                                    MessageBoxButton.OK, MessageBoxImage.Error);
            }
            else
                MessageBox.Show("Cannot Save! Underlying " + NewUnderlyingRIC + " already exists!", "Underlying Maintenance Error",
                                MessageBoxButton.OK, MessageBoxImage.Error);
        }

        public bool CanAddNewItem()
        {
            return !String.IsNullOrEmpty(NewUnderlyingRIC) && !String.IsNullOrEmpty(NewUnderlyingDescription);
        }

        public bool CanUpdateValidity(bool isRequestToMakeValid)
        {
            if (SelectedUnderlying == null)
                return false;

            return (SelectedUnderlying.IsValid != isRequestToMakeValid);
        }

        public void UpdateValidity()
        {
            if (underlyingManager.UpdateValidity(SelectedUnderlying.RIC, !SelectedUnderlying.IsValid))
                SelectedUnderlying.IsValid = !SelectedUnderlying.IsValid;
            else
                MessageBox.Show("Failed to update validity of underlying " + SelectedUnderlying.RIC, "Underlying Maintenance Error",
                                MessageBoxButton.OK, MessageBoxImage.Error);
        }
    }
}
