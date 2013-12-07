using System;
using System.Collections.Generic;
using System.Windows;

namespace RequestForQuoteReportsModuleLibrary
{
    public class GeneratedReportViewModel :  DependencyObject
    {
        public List<KeyValuePair<string, decimal>> ReportData { get; set; }
        public string ReportType { get; set; }
        public string ReportTitle { get; set; }

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="reportType"> to be used for setting visibility of appropriate chart.</param>
        /// <param name="reportTitle"> to be displayed in the popup window.</param>
        /// <param name="reportData"> to be used for the charting.</param>
        /// <exception cref="ArgumentException"> thrown if reportType or reporTitle parameters are null or empty.</exception>
        /// /// <exception cref="ArgumentNullException"> thrown if reportData is null.</exception>
        public GeneratedReportViewModel(string reportType, string reportTitle,  List<KeyValuePair<string, decimal>> reportData)
        {
            if (String.IsNullOrEmpty(reportType))
                throw new ArgumentException("reportType");

            if (String.IsNullOrEmpty(reportTitle))
                throw new ArgumentException("reportTitle");

            if (reportData == null)
                throw new ArgumentNullException("reportData");

            ReportData = reportData;
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
