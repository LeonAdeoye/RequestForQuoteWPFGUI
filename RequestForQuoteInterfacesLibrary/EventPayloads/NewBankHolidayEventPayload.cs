using System;
using System.Text;
using RequestForQuoteInterfacesLibrary.Enums;

namespace RequestForQuoteInterfacesLibrary.EventPayloads
{
    public class NewBankHolidayEventPayload 
    {
        public LocationEnum Location { get; set; }
        public DateTime NewBankHolidayDate { get; set; }

        public override string ToString()
        {
            var builder = new StringBuilder("Location = ");
            builder.Append(Location.ToString());
            builder.Append(", New Bank Holiday Date = ");
            builder.Append(NewBankHolidayDate);
            return builder.ToString();
        }
    }
}
