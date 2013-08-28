using System;
using System.Collections.Generic;
using RequestForQuoteInterfacesLibrary.Enums;
using RequestForQuoteInterfacesLibrary.ModelInterfaces;

namespace RequestForQuoteInterfacesLibrary.ServiceInterfaces
{
    public interface IBankHolidayManager
    {
        Dictionary<LocationEnum, SortedDictionary<DateTime, DateTime>> BankHolidays { get; set; }
        int CalculateBusinessDaysToExpiry(DateTime startDate, DateTime endDate, LocationEnum location);
        int CalculateAllDaysToExpiry(DateTime startDate, DateTime endDate);
        int CalculateAllDaysToExpiryFromToday(DateTime endDate);
        int CalculateBusinessDaysToExpiryFromToday(DateTime endDate, LocationEnum location);
        bool IsHoliday(DateTime dateToValidate, LocationEnum location);
        bool IsValidBusinessDay(DateTime dateToValidate, LocationEnum location);
        void AddHoliday(DateTime holidayDate, LocationEnum location, bool canSaveToDatabase);
        List<IBankHoliday> GetHolidaysInLocation(LocationEnum location);
        void Initialize();
    }
}