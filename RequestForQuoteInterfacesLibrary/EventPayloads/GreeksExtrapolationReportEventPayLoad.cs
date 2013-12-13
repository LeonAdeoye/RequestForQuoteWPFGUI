using System;
using System.Collections.Generic;
using System.Text;

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
        public IDictionary<String, IDictionary<decimal, decimal>> OutputExtrapolation { get; set; }
        public String ReportDescription { get; set; }
        public ISet<string> GreeksToBeIncluded { get; set; }

        /// <summary>
        /// Default constructor
        /// </summary>
        public GreeksExtrapolationReportEventPayLoad() 
        {
            OutputExtrapolation = new Dictionary<string, IDictionary<decimal, decimal>>();
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
        /// Adds the input and output values to the extrapolation map.
        /// </summary>
        /// <param name="outputType"> the output type of the extrapolation.</param>
        /// <param name="inputValue"> the input type like underlying price, interest rate, volatility.</param>
        /// <param name="outputValue"> the output value extrapolated from the input value.</param>
        /// <exception cref="ArgumentException"> if the output type value is null or empty.</exception>
        public void AddOutputExtrapolation(string outputType, double inputValue, double outputValue)
        {
            if (String.IsNullOrEmpty(outputType))
                throw new ArgumentException("outputType");

            if (OutputExtrapolation.ContainsKey(outputType))
            {
                var outputDict = OutputExtrapolation[outputType];
                outputDict[Convert.ToDecimal(inputValue)] = Convert.ToDecimal(outputValue);
            }
            else
                OutputExtrapolation.Add(outputType, new Dictionary<decimal, decimal>() { { Convert.ToDecimal(inputValue), Convert.ToDecimal(outputValue) } });
        }
    }
}
