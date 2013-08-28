using System;
using RequestForQuoteInterfacesLibrary.Enums;

namespace RequestForQuoteInterfacesLibrary.ModelInterfaces
{
    public interface IBankHoliday
    {
        LocationEnum Location { get; set; }
        DateTime BankHoliday { get; set; }
    }
}
