using System;
using System.Collections.Generic;
using System.ServiceModel;
using Microsoft.Practices.Prism.Events;
using Microsoft.Practices.ServiceLocation;
using RequestForQuoteInterfacesLibrary.Constants;
using RequestForQuoteInterfacesLibrary.Enums;
using RequestForQuoteInterfacesLibrary.EventPayloads;
using RequestForQuoteInterfacesLibrary.Events;
using RequestForQuoteInterfacesLibrary.ModelImplementations;
using RequestForQuoteInterfacesLibrary.ModelInterfaces;
using RequestForQuoteInterfacesLibrary.ServiceInterfaces;
using log4net;
using RequestForQuoteServicesModuleLibrary.HolidayMaintenanceService;

namespace RequestForQuoteServicesModuleLibrary.ServicesImplementation
{
    public class BankHolidayManagerImpl : IBankHolidayManager
    {
        private static readonly ILog log = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private readonly IEventAggregator eventAggregator = ServiceLocator.Current.GetInstance<IEventAggregator>();
        private readonly HolidayControllerClient holidayControllerProxy = new HolidayControllerClient();
        public Dictionary<LocationEnum, SortedDictionary<DateTime, DateTime>> BankHolidays { get; set; }

        public BankHolidayManagerImpl()
        {
            BankHolidays = new Dictionary<LocationEnum, SortedDictionary<DateTime, DateTime>>();
        }

        public int CalculateBusinessDaysToExpiry(DateTime startDate, DateTime endDate, LocationEnum location)
        {
            DateTime currentDate = startDate;
            int count = 0;
            do
            {
                currentDate = currentDate.AddDays(1);
                if (IsValidBusinessDay(currentDate, location))
                    ++count;
            }
            while (currentDate != endDate);
            return count;
        }

        public int CalculateAllDaysToExpiry(DateTime startDate, DateTime endDate)
        {
            TimeSpan difference = endDate - startDate;
            return difference.Days;
        }

        public int CalculateAllDaysToExpiryFromToday(DateTime endDate)
        {
            return CalculateAllDaysToExpiry(DateTime.Today, endDate);
        }

        public int CalculateBusinessDaysToExpiryFromToday(DateTime endDate, LocationEnum location)
        {
            return CalculateBusinessDaysToExpiry(DateTime.Today, endDate, location);
        }

        public bool IsHoliday(DateTime dateToValidate, LocationEnum location)
        {
            SortedDictionary<DateTime, DateTime> holidaysInLocation;
            if (BankHolidays.TryGetValue(location, out holidaysInLocation))
                return holidaysInLocation.ContainsKey(dateToValidate);
            return false;
        }

        public bool IsValidBusinessDay(DateTime dateToValidate, LocationEnum location)
        {
            return !IsHoliday(dateToValidate, location) && dateToValidate.DayOfWeek != DayOfWeek.Saturday && dateToValidate.DayOfWeek != DayOfWeek.Sunday;
        }

        public bool AddHoliday(DateTime holidayDate, LocationEnum location, bool canSaveToDatabase)
        {
            var wasSavedInDatabase = false;

            SortedDictionary<DateTime, DateTime> holidaysInLocation;
            if (BankHolidays.TryGetValue(location, out holidaysInLocation))
            {
                if (!holidaysInLocation.ContainsKey(holidayDate))
                    holidaysInLocation[holidayDate] = holidayDate;
            }
            else
                BankHolidays.Add(location, new SortedDictionary<DateTime, DateTime>() { { holidayDate, holidayDate } });
            
            // TODO - probably makes more sense to move below into two separate methods
            if (canSaveToDatabase)
                wasSavedInDatabase = holidayControllerProxy.save(location.ToString(), holidayDate, RequestForQuoteConstants.MY_USER_NAME);

            // TODO verify that this needs to be called even if canSaveToDatabase == false
            // Publish event for other observer view models...
            eventAggregator.GetEvent<NewBankHolidayEvent>().Publish(new NewBankHolidayEventPayload()
            {
                NewBankHolidayDate = holidayDate,
                Location = location
            });                

            return !canSaveToDatabase || wasSavedInDatabase;
        }

        public List<IBankHoliday> GetHolidaysInLocation(LocationEnum location)
        {
            var holidaysInLocation = new List<IBankHoliday>();

            if (BankHolidays.ContainsKey(location))
                foreach (var holiday in BankHolidays[location])
                    holidaysInLocation.Add(new BankHolidayImpl() {BankHoliday = holiday.Key, Location = location});

            return holidaysInLocation;
        }

        public void Initialize()
        {
            try
            {
                var allHolidays = holidayControllerProxy.getAll();
                foreach (var bankHoliday in holidayControllerProxy.getAll())
                {
                    LocationEnum locationEnumValue;
                    if (!string.IsNullOrEmpty(bankHoliday.location) && Enum.TryParse(bankHoliday.location, true, out locationEnumValue)
                            && AddHoliday(bankHoliday.holidayDate, locationEnumValue, RequestForQuoteConstants.DO_NOT_SAVE_TO_DATABASE))
                        log.Error(string.Format("Failed to add bank holiday with location: {0} and holidayDate: {1}.", bankHoliday.location, bankHoliday.holidayDate));
                }

                if (log.IsDebugEnabled)
                    log.Debug(string.Format("Loaded {0} holidays from the database.", allHolidays.Length));
            }
            catch (EndpointNotFoundException exception)
            {
                log.Error(String.Format("Failed to connect to proxy for remote holiday controller webservice. Exception thrown {0}", exception));
                throw;
            }

        }
    }
}