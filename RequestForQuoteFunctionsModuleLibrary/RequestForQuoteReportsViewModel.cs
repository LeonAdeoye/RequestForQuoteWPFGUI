using System;
using System.ComponentModel;
using System.Windows;
using System.Windows.Input;
using Microsoft.Practices.Prism.Events;
using RequestForQuoteFunctionsModuleLibrary.Commands;
using log4net;

namespace RequestForQuoteFunctionsModuleLibrary
{
    public class RequestForQuoteReportsViewModel : DependencyObject, INotifyPropertyChanged
    {
        private static readonly ILog log = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private readonly IEventAggregator eventAggregator;
        public event PropertyChangedEventHandler PropertyChanged;
        public ICommand CompileReportCommand { get; set; }

        public RequestForQuoteReportsViewModel(IEventAggregator eventAggregator)
        {
            this.eventAggregator = eventAggregator;
            CompileReportCommand = new CompileReportCommand(this);
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

        public bool CanCompileResport()
        {
            return !String.IsNullOrEmpty(ReportType) && !String.IsNullOrEmpty(RequestsCountCategory);
        }

        public void CompileReport()
        {
            ClearRequestsPerCategoryInputs();
        }

        public void ClearRequestsPerCategoryInputs()
        {
            ReportType = "";
            RequestsCountCategory = "";
        }
    }
}
