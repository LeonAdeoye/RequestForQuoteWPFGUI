using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Input;
using Microsoft.Practices.Prism;
using Microsoft.Practices.Prism.Events;
using Microsoft.Practices.ServiceLocation;
using RequestForQuoteInterfacesLibrary.Constants;
using RequestForQuoteInterfacesLibrary.Enums;
using RequestForQuoteInterfacesLibrary.EventPayloads;
using RequestForQuoteInterfacesLibrary.Events;
using RequestForQuoteInterfacesLibrary.ModelImplementations;
using RequestForQuoteInterfacesLibrary.ModelInterfaces;
using RequestForQuoteInterfacesLibrary.ServiceInterfaces;
using RequestForQuoteInterfacesLibrary.ViewModelInterfaces;
using RequestForQuoteMaintenanceModuleLibrary.Commands;
using log4net;

namespace RequestForQuoteMaintenanceModuleLibrary
{
    public sealed class BankHolidayMaintenanceViewModel : DependencyObject, IClearInputViewModel, IAddNewItemViewModel
    {
        private static readonly ILog log = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private readonly IEventAggregator eventAggregator = ServiceLocator.Current.GetInstance<IEventAggregator>();
        private static readonly IBankHolidayManager bankHolidayManager = ServiceLocator.Current.GetInstance<IBankHolidayManager>();

        public static ObservableCollection<IBankHoliday> BankHolidays { get; set; }
        public List<string> Locations { get; set; }

        public ICommand AddNewItemCommand { get; set; }
        public ICommand ClearInputCommand { get; set; }

        private static void LocationPropertyChangedCallback(DependencyObject dependencyObject, DependencyPropertyChangedEventArgs dependencyPropertyChangedEventArgs)
        {
            string location = dependencyPropertyChangedEventArgs.NewValue as string;
            LocationEnum locationEnumValue;
            if (location != null && Enum.TryParse(location, true, out locationEnumValue))
            {
                BankHolidays.Clear();
                BankHolidays.AddRange(bankHolidayManager.GetHolidaysInLocation(locationEnumValue));                
            }
        }

        public string SelectedLocation
        {
            get { return (string)GetValue(SelectedLocationProperty); }
            set { SetValue(SelectedLocationProperty, value); }
        }

        // Using a DependencyProperty as the backing store for SelectedLocation.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty SelectedLocationProperty =
            DependencyProperty.Register("SelectedLocation", typeof(string), typeof(BankHolidayMaintenanceViewModel), new UIPropertyMetadata(null, LocationPropertyChangedCallback));

        public BankHolidayMaintenanceViewModel()
        {
            AddNewItemCommand = new AddNewItemCommand(this);
            ClearInputCommand = new ClearInputCommand(this);

            InitializeCollections();

            eventAggregator.GetEvent<NewBankHolidayEvent>().Subscribe(HandleNewBankHolidayEvent, ThreadOption.UIThread, RequestForQuoteConstants.MAINTAIN_STRONG_REFERENCE);
        }

        public void HandleNewBankHolidayEvent(NewBankHolidayEventPayload eventPayLoad)
        {
            if (log.IsDebugEnabled)
                log.Debug("Received new bank holiday event from BankHolidayManager: " + eventPayLoad);

            if (SelectedLocation != null && SelectedLocation == eventPayLoad.Location.ToString())
                BankHolidays.Add(new BankHolidayImpl()
                    {
                        HolidayDate = eventPayLoad.NewBankHolidayDate,
                        Location = eventPayLoad.Location
                    });
        }

        private void InitializeCollections()
        {
            Locations = new List<string>();
            foreach (var location in Enum.GetNames(typeof(LocationEnum)))
                Locations.Add(location);

            BankHolidays = new ObservableCollection<IBankHoliday>();
        }
      
        public void ClearInput()
        {
            SelectedBankHoliday = null;
        }

        public bool CanClearInput()
        {
            return !string.IsNullOrEmpty(SelectedBankHoliday.ToString()) && !String.IsNullOrEmpty(SelectedLocation);
        }

        public void AddNewItem()
        {
            LocationEnum locationEnumValue;
            if (SelectedBankHoliday.HasValue && Enum.TryParse(SelectedLocation, true, out locationEnumValue))
            {
                if (bankHolidayManager.SaveToDatabase(SelectedBankHoliday.Value, locationEnumValue))
                    SelectedBankHoliday = null;
                else
                    MessageBox.Show("Failed to add new bank holiday " + SelectedBankHoliday.Value + " to location " +
                                    locationEnumValue, "Bank Holiday Maintenance Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }                
        }

        public bool CanAddNewItem()
        {
            return !string.IsNullOrEmpty(SelectedBankHoliday.ToString()) && !String.IsNullOrEmpty(SelectedLocation);
        }

        public DateTime? SelectedBankHoliday
        {
            get { return (DateTime?)GetValue(SelectedBankHolidayProperty); }
            set { SetValue(SelectedBankHolidayProperty, value); }
        }

        // Using a DependencyProperty as the backing store for SelectedBankHoliday.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty SelectedBankHolidayProperty =
            DependencyProperty.Register("SelectedBankHoliday", typeof(DateTime?), typeof(BankHolidayMaintenanceViewModel), new UIPropertyMetadata(null));
    }
}
