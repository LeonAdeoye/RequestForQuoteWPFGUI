using System;
using RequestForQuoteInterfacesLibrary.Enums;
using RequestForQuoteInterfacesLibrary.ModelInterfaces;

namespace RequestForQuoteInterfacesLibrary.ModelImplementations
{
    public sealed class BankHolidayImpl : IBankHoliday
    {
        public LocationEnum Location { get; set; }
        public DateTime BankHoliday { get; set; }
    }
}
