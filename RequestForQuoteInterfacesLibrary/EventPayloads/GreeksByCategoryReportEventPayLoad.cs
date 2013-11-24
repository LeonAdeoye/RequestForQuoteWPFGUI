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
        public double MinimumGreek { get; set; }
        public IDictionary<String, IDictionary<string, double>> GreeksByCategory { get; set; }
        public String ReportDescription { get; set; }
        public string CategoryDescription { get; set; }
        public ISet<string> GreeksToBeIncluded { get; set; } 

        public GreeksByCategoryReportEventPayLoad() 
        {
            GreeksByCategory = new Dictionary<string, IDictionary<string, double>>();
            GreeksToBeIncluded = new HashSet<string>();
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
            builder.Append(", GreeksByCategory = ");
            builder.Append(GreeksByCategory);
            builder.Append(", Greeks to be included in report = ");
            builder.Append(GreeksToBeIncluded);
            return builder.ToString();
        }
    }
}
