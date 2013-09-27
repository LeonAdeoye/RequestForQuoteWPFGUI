﻿using System;
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
    public sealed class UnderlyingMaintenanceViewModel : DependencyObject, IUpdateValidityViewModel, IClearInputViewModel, IAddNewItemViewModel
    {
        private static readonly ILog log = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private readonly IEventAggregator eventAggregator = ServiceLocator.Current.GetInstance<IEventAggregator>();
        private readonly IUnderlyingManager underlyingManager = ServiceLocator.Current.GetInstance<IUnderlyingManager>();

        public ObservableCollection<IUnderlyier> Underlyiers { get; set; }
        public List<string> Tiers { get; set; }
        public ICommand AddNewItemCommand { get; set; }
        public ICommand ClearInputCommand { get; set; }
        public ICommand UpdateValidityCommand { get; set; }

        public UnderlyingMaintenanceViewModel()
        {
            AddNewItemCommand = new AddNewItemCommand(this);
            ClearInputCommand = new ClearInputCommand(this);
            UpdateValidityCommand = new UpdateValidityCommand(this);

            InitializeCollections();
            InitializeEventSubscriptions();
        }

        private void InitializeCollections()
        {
            Underlyiers = new ObservableCollection<IUnderlyier>(underlyingManager.Underlyiers);
            Tiers = new List<string>();
            foreach (var tier in Enum.GetNames(typeof(TierEnum)))
                Tiers.Add(tier);
        }

        private void InitializeEventSubscriptions()
        {
            eventAggregator.GetEvent<NewUnderlyierEvent>().Subscribe(HandleNewUnderlyierEvent, ThreadOption.UIThread, RequestForQuoteConstants.MAINTAIN_STRONG_REFERENCE);
        }

        public IUnderlyier SelectedUnderlyier
        {
            get { return (IUnderlyier)GetValue(SelectedUnderlyierProperty); }
            set { SetValue(SelectedUnderlyierProperty, value); }
        }

        // Using a DependencyProperty as the backing store for SelectedUnderlyier.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty SelectedUnderlyierProperty =
            DependencyProperty.Register("SelectedUnderlyier", typeof(IUnderlyier), typeof(UnderlyingMaintenanceViewModel), new UIPropertyMetadata(null));
                      
        public void HandleNewUnderlyierEvent(NewUnderlyierEventPayload eventPayLoad)
        {
            if (log.IsDebugEnabled)
                log.Debug("Received new underlyier event from UnderlyingManager: " + eventPayLoad);

            Underlyiers.Add(eventPayLoad.NewUnderlyier);
        }
     
        public void ClearInput()
        {
        }

        public bool CanClearInput()
        {
            return !string.IsNullOrEmpty(String.Empty);
        }

        public void AddNewItem()
        {
            // TODO
            if (!underlyingManager.Underlyiers.Exists((underlyier) => underlyier.RIC == "TODO"))
            {
            }
        }

        public bool CanAddNewItem()
        {
            // TODO the NewClientTier.HasValue part does not work when you select from the tier combobox.
            return false;
        }

        public bool CanUpdateValidity(bool isRequestToMakeValid)
        {
            if (SelectedUnderlyier == null)
                return false;
            return (SelectedUnderlyier.IsValid != isRequestToMakeValid);
        }

        public void UpdateValidity()
        {
            // TODO
            //if (underlyingManager.UpdateValidity(SelectedUnderlyier.Identifier, !SelectedUnderlyier.IsValid))
            //    SelectedUnderlyier.IsValid = !SelectedUnderlyier.IsValid;
            //else
            //    MessageBox.Show("Failed to update validity of underlyier " + SelectedUnderlyier.RIC, "Underlyier Maintenance Error",
            //                    MessageBoxButton.OK, MessageBoxImage.Error);
        }
    }
}