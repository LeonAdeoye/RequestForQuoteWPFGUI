using System;
using System.Collections.Generic;
using System.Text;

namespace RequestForQuoteInterfacesLibrary.EventPayloads
{
    public class RequestsCountByCategoryReportEventPayLoad 
    {
        public string ReportType { get; set; }
        public string Category { get; set; }
        public DateTime FromDate { get; set; }
        public int MinimumCount { get; set; }
        public IDictionary<String, decimal> CountByCategory { get; set; }
        public String ReportDescription { get; set; }
        public string CategoryDescription { get; set; }

        public RequestsCountByCategoryReportEventPayLoad() 
        {
            CountByCategory = new Dictionary<string, decimal>();    
        }

        public override string ToString()
        {
            var builder = new StringBuilder("Category = ");
            builder.Append(Category);
            builder.Append(", Category description = ");
            builder.Append(CategoryDescription);
            builder.Append(", Report description = ");
            builder.Append(ReportDescription);
            builder.Append(", Report type = ");
            builder.Append(ReportType);
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
