using System;
using System.Collections.Generic;
using System.Text;
using RequestForQuoteInterfacesLibrary.Enums;

namespace RequestForQuoteInterfacesLibrary.EventPayloads
{
    public class GreeksByCategoryReportEventPayLoad 
    {
        public string ReportType { get; set; }
        public string Category { get; set; }
        public DateTime MaturityDateFrom { get; set; }
        public DateTime MaturityDateTo { get; set; }
        public double MinimumGreek { get; set; }
        public IDictionary<String, IDictionary<string, decimal>> GreeksByCategory { get; set; }
        public String ReportDescription { get; set; }
        public string CategoryDescription { get; set; }
        public ISet<string> GreeksToBeIncluded { get; set; } 

        /// <summary>
        /// Default constructor
        /// </summary>
        public GreeksByCategoryReportEventPayLoad() 
        {
            GreeksByCategory = new Dictionary<string, IDictionary<string, decimal>>();
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
            foreach (var categoryValue in GreeksByCategory)
            {
                builder.Append("{");
                builder.Append(categoryValue.Key);   
                builder.Append(" = ");
                foreach (var valuePair in categoryValue.Value)
                {
                    builder.Append("{");
                    builder.Append(valuePair.Key);
                    builder.Append(" = ");
                    builder.Append(valuePair.Value);
                    builder.Append("} ");
                }
                builder.Append("} ");
            }
            builder.Append(", Greeks to be included in report = { ");
            foreach (var greekToBeIncluded in GreeksToBeIncluded)
            {
                builder.Append(greekToBeIncluded);
                builder.Append(" ");
            }
            builder.Append("}");

            return builder.ToString();
        }

        /// <summary>
        /// Adds the greek value to the map of greek values by category value.
        /// </summary>
        /// <param name="categoryValue"> the category value used in the aggregation.</param>
        /// <param name="typeOfGreek"> the greek type like delta, gamma, vega, theta.</param>
        /// <param name="greekValue"> the aggregated total greek value for the specified category value.</param>
        /// <exception cref="ArgumentException"> if the category value is null or empty.</exception>
        public void AddGreek(string categoryValue, GreeksEnum typeOfGreek, double greekValue)
        {
            if (String.IsNullOrEmpty(categoryValue))
                throw new ArgumentException("categoryValue");

            if (GreeksByCategory.ContainsKey(typeOfGreek.ToString()))
            {
                var greekEntry = GreeksByCategory[typeOfGreek.ToString()];
                greekEntry[categoryValue] = Convert.ToDecimal(greekValue);
            }
            else
                GreeksByCategory.Add(typeOfGreek.ToString(), new Dictionary<string, decimal>() { { categoryValue, Convert.ToDecimal(greekValue) } });
        }
    }
}
