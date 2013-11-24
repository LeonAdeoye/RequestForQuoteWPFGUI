using System;
using System.Collections.Generic;
using System.Text;

namespace RequestForQuoteInterfacesLibrary.EventPayloads
{
    public class GreeksByCategoryReportEventPayLoad 
    {
        public string ReportType { get; set; }
        public string Category { get; set; }
        public DateTime MaturityDateFrom { get; set; }
        public DateTime MaturityDateTo { get; set; }
        public decimal MinimumGreek { get; set; }
        public IDictionary<String, int> CountByCategory { get; set; }
        public String ReportDescription { get; set; }
        public string CategoryDescription { get; set; }

        public GreeksByCategoryReportEventPayLoad() 
        {
            CountByCategory = new Dictionary<string, int>();    
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
            builder.Append(", Maturity date from = ");
            builder.Append(MaturityDateFrom);
            builder.Append(", Maturity date to = ");
            builder.Append(MaturityDateTo);
            builder.Append(", Minimum greek = ");
            builder.Append(MinimumGreek);
            builder.Append(", CountByCategory = ");
            builder.Append(CountByCategory);
            return builder.ToString();
        }
    }
}
