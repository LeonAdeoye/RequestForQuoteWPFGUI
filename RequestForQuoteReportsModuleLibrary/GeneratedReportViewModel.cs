using System;
using System.Collections.Generic;
using System.Windows;

namespace RequestForQuoteReportsModuleLibrary
{
    public class GeneratedReportViewModel :  DependencyObject
    {
        private readonly Dictionary<string, List<KeyValuePair<string, decimal>>> seriesData = new Dictionary<string, List<KeyValuePair<string, decimal>>>();
        public string ReportType { get; set; }
        public string ReportTitle { get; set; }
        public string SeriesKey { get; set; }
        public static string ONLY_ONE_SERIES = "OnlyOneSeries";

        public List<KeyValuePair<string, decimal>> SingleSeriesData
        {
            get
            {
                return seriesData[ONLY_ONE_SERIES];
            }
        }

        public Dictionary<string, List<KeyValuePair<string, decimal>>> MultipleSeriesData 
        { 
            get { return seriesData; }
        }

        /// <summary>
        /// Adds a series to the dictionary of series data using the seriesKey as the key
        /// </summary>
        /// <param name="seriesKey"> the key of the series dictionary</param>
        /// <param name="series"> the list of category-value pairs that will be added to the series dictionary</param>
        public void AddSeries(string seriesKey, List<KeyValuePair<string, decimal>> series)
        {
            if (String.IsNullOrEmpty(seriesKey))
                throw new ArgumentException("seriesKey");

            if (series == null)
                throw new ArgumentNullException("series");

            seriesData[seriesKey] = series;
        }

        /// <summary>
        /// Clears the series.
        /// </summary>
        public void ClearSeries()
        {
            seriesData.Clear();
        }

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="reportType"> to be used for setting visibility of appropriate chart.</param>
        /// <param name="reportTitle"> to be displayed in the popup window.</param>
        /// <param name="seriesData"> dictionary of series data to be used for the charting.</param>
        /// <exception cref="ArgumentException"> thrown if reportType or reporTitle parameters are null or empty.</exception>
        /// /// <exception cref="ArgumentNullException"> thrown if seriesData is null.</exception>
        public GeneratedReportViewModel(string reportType, string reportTitle,  Dictionary<string, List<KeyValuePair<string, decimal>>> seriesData)
        {
            if (String.IsNullOrEmpty(reportType))
                throw new ArgumentException("reportType");

            if (String.IsNullOrEmpty(reportTitle))
                throw new ArgumentException("reportTitle");

            if (seriesData == null)
                throw new ArgumentNullException("seriesData");

            foreach (var series in seriesData)
                AddSeries(series.Key, series.Value);    

            ReportType = reportType;
            ReportTitle = reportTitle;        
        }
        /// <summary>
        /// Default constructor
        /// </summary>
        public GeneratedReportViewModel()
        {            
        }
    }
}
