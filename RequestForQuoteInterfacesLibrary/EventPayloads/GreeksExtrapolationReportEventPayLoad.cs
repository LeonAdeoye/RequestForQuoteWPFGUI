using System;
using System.Collections.Generic;
using System.Text;
using RequestForQuoteInterfacesLibrary.Enums;

namespace RequestForQuoteInterfacesLibrary.EventPayloads
{
    public class GreeksExtrapolationReportEventPayLoad 
    {
        public string ReportType { get; set; }
        public string RangeVariable { get; set; }
        public double RangeMinimum { get; set; }
        public double RangeMaximum { get; set; }
        public double RangeIncrement { get; set; }
        public int RequestId { get; set; }
        public IDictionary<String, IDictionary<string, decimal>> OutputExtrapolation { get; set; }
        public String ReportDescription { get; set; }
        public ISet<string> GreeksToBeIncluded { get; set; }

        /// <summary>
        /// Default constructor
        /// </summary>
        public GreeksExtrapolationReportEventPayLoad() 
        {
            OutputExtrapolation = new Dictionary<string, IDictionary<string, decimal>>();
            GreeksToBeIncluded = new HashSet<string>();
        }

        public override string ToString()
        {
            var builder = new StringBuilder("Range variable = ");
            builder.Append(RangeVariable);
            builder.Append(", Report description = ");
            builder.Append(ReportDescription);
            builder.Append(", Report type = ");
            builder.Append(ReportType);
            builder.Append(", Request id = ");
            builder.Append(RequestId);
            builder.Append(", Range minimum = ");
            builder.Append(RangeMinimum);
            builder.Append(", Range maximum = ");
            builder.Append(RangeMaximum);
            builder.Append(", Range increment = ");
            builder.Append(RangeIncrement);
            builder.Append(", Output extrapolation = ");
            foreach (var output in OutputExtrapolation)
            {
                builder.Append("{");
                builder.Append(output.Key);   
                builder.Append(" = ");
                foreach (var valuePair in output.Value)
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
        /// <param name="rangeValue"> the category value used in the aggregation.</param>
        /// <param name="typeOfGreek"> the greek type like delta, gamma, vega, theta.</param>
        /// <param name="greekValue"> the aggregated total greek value for the specified category value.</param>
        /// <exception cref="ArgumentException"> if the category value is null or empty.</exception>
        public void AddOutputExtrapolation(string rangeValue, GreeksEnum typeOfGreek, double greekValue)
        {
            if (String.IsNullOrEmpty(rangeValue))
                throw new ArgumentException("rangeValue");

            // Cannot do conversion from Double to Decimal if NAN.
            if (double.IsNaN(greekValue))
                return;

            if (OutputExtrapolation.ContainsKey(typeOfGreek.ToString()))
            {
                var greekEntry = OutputExtrapolation[typeOfGreek.ToString()];
                greekEntry[rangeValue] = Convert.ToDecimal(greekValue);
            }
            else
                OutputExtrapolation.Add(typeOfGreek.ToString(), new Dictionary<string, decimal>() { { rangeValue, Convert.ToDecimal(greekValue) } });
        }
    }
}
