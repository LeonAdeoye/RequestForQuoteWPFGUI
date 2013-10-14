using System;
using System.Runtime.Serialization;
using RequestForQuoteInterfacesLibrary.Enums;
using RequestForQuoteInterfacesLibrary.ModelInterfaces;

namespace RequestForQuoteInterfacesLibrary.ModelImplementations
{
    [DataContract]
    public sealed class BankHolidayImpl : IBankHoliday
    {
        [DataMember] public LocationEnum Location { get; set; }
        [DataMember] public DateTime BankHoliday { get; set; } //TODO - cannot convert to datetime from json - need epoch time
    }
}
