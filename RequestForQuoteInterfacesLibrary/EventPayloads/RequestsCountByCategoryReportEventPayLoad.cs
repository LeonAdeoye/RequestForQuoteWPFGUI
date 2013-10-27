using System;
using System.Collections.Generic;
using System.Text;
using RequestForQuoteInterfacesLibrary.ModelInterfaces;

namespace RequestForQuoteInterfacesLibrary.EventPayloads
{
    public class RequestsCountByCategoryReportEventPayLoad 
    {
        public string Category { get; set; }
        public DateTime FromDate { get; set; }
        public int MinimumCount { get; set; }
        public IDictionary<String, int> CountByCategory { get; set; }

        public RequestsCountByCategoryReportEventPayLoad() 
        {
            CountByCategory = new Dictionary<string, int>();    
        }

        public override string ToString()
        {
            var builder = new StringBuilder("Category = ");
            builder.Append(Category);
            builder.Append(", FromDate = ");
            builder.Append(FromDate);
            builder.Append(", MinimumCount = ");
            builder.Append(MinimumCount);
            builder.Append(", CountByCategory = ");
            builder.Append(CountByCategory);
            return builder.ToString();
        }
    }
}
