using System;
using System.ComponentModel;
using System.Windows;
using System.Windows.Input;
using Microsoft.Practices.Prism.Events;
using RequestForQuoteFunctionsModuleLibrary.Commands;
using RequestForQuoteInterfacesLibrary.Constants;
using RequestForQuoteServicesModuleLibrary.ServicesImplementation;
using log4net;

namespace RequestForQuoteFunctionsModuleLibrary
{
    public class RequestForQuoteReportsViewModel : DependencyObject, INotifyPropertyChanged
    {
        private static readonly ILog log = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        private readonly IEventAggregator eventAggregator;
        private readonly IReportDataManager reportingManager;

        public event PropertyChangedEventHandler PropertyChanged;
        public ICommand CompileReportCommand { get; set; }
        public ICommand ClearReportInputCommand { get; set; }
        public ICommand SaveReportInputCommand { get; set; }

        public RequestForQuoteReportsViewModel(IEventAggregator eventAggregator, IReportDataManager reportingManager)
        {
            this.eventAggregator = eventAggregator;
            this.reportingManager = reportingManager;

            CompileReportCommand = new CompileReportCommand(this);
            ClearReportInputCommand = new ClearReportInputCommand(this);
            SaveReportInputCommand = new SaveReportInputCommand(this);
        }

        private void NotifyPropertyChanged(string propertyName)
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
        }

        public String ReportType
        {
            get { return (String)GetValue(ReportTypeProperty); }
            set { SetValue(ReportTypeProperty, value); }
        }

        // Using a DependencyProperty as the backing store for ReportType.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty ReportTypeProperty =
            DependencyProperty.Register("ReportType", typeof(String), typeof(RequestForQuoteReportsViewModel), new UIPropertyMetadata(""));
        
        public String RequestsCountCategory
        {
            get { return (String)GetValue(RequestsCountCategoryProperty); }
            set { SetValue(RequestsCountCategoryProperty, value); }
        }

        // Using a DependencyProperty as the backing store for RequestsCountCategory.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty RequestsCountCategoryProperty =
            DependencyProperty.Register("RequestsCountCategory", typeof(String), typeof(RequestForQuoteReportsViewModel), new UIPropertyMetadata(""));

        public int? MinimumCount
        {
            get { return (int?)GetValue(MinimumCountProperty); }
            set { SetValue(MinimumCountProperty, value); }
        }

        // Using a DependencyProperty as the backing store for MinimumCount.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty MinimumCountProperty =
            DependencyProperty.Register("MinimumCount", typeof(int?), typeof(RequestForQuoteReportsViewModel), new UIPropertyMetadata(null));

        public bool CanCompileResport()
        {
            return !String.IsNullOrEmpty(ReportType) && !String.IsNullOrEmpty(RequestsCountCategory);
        }

        public void CompileReport()
        {
            reportingManager.GetRequestCountPerCategory(ReportType, RequestForQuoteConstants.REQUEST_COUNT_BY_BOOKCODE, new DateTime(2013, 10, 1), 0);
            ClearRequestsPerCategoryInputs();
        }

        private void ClearRequestsPerCategoryInputs()
        {
            ReportType = "";
            RequestsCountCategory = "";
            MinimumCount = null;
        }

        public bool CanClearReportInput()
        {
            return !String.IsNullOrEmpty(ReportType) || !String.IsNullOrEmpty(RequestsCountCategory) || MinimumCount.HasValue;
        }

        public void ClearReportInput()
        {
            ClearRequestsPerCategoryInputs();
        }

        public void SaveReportInput()
        {
            throw new NotImplementedException();
        }

        public bool CanSaveReportInput()
        {
            return !String.IsNullOrEmpty(ReportType) && !String.IsNullOrEmpty(RequestsCountCategory);
        }
    }
}
