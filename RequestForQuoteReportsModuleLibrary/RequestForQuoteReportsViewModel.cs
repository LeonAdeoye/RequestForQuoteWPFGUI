using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Windows;
using System.Windows.Input;
using Microsoft.Practices.Prism.Events;
using Microsoft.Practices.Prism.Regions;
using Microsoft.Practices.ServiceLocation;
using RequestForQuoteInterfacesLibrary.Constants;
using RequestForQuoteInterfacesLibrary.EventPayloads;
using RequestForQuoteInterfacesLibrary.Events;
using RequestForQuoteInterfacesLibrary.WindowInterfaces;
using RequestForQuoteReportsModuleLibrary.Commands;
using RequestForQuoteServicesModuleLibrary.ServicesImplementation;
using log4net;

namespace RequestForQuoteReportsModuleLibrary
{
    public class RequestForQuoteReportsViewModel : DependencyObject, INotifyPropertyChanged
    {
        private static readonly ILog log = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        private readonly IEventAggregator eventAggregator;
        private readonly IReportDataManager reportingManager;
        private readonly IRegionManager regionManager;

        public event PropertyChangedEventHandler PropertyChanged;
        public ICommand CompileReportCommand { get; set; }
        public ICommand ClearReportInputCommand { get; set; }
        public ICommand SaveReportInputCommand { get; set; }

        public List<KeyValuePair<string, int>> ReportData { get; set; }
        public List<KeyValuePair<string, string>> ListOfReportTypes { get; set; }
        public List<KeyValuePair<string, string>> ListOfCategoryTypes { get; set; }
        public string ReportTitle { get; set; }

        public RequestForQuoteReportsViewModel(IEventAggregator eventAggregator, IReportDataManager reportingManager, IRegionManager regionManager)
        {
            this.eventAggregator = eventAggregator;
            this.reportingManager = reportingManager;
            this.regionManager = regionManager;

            CompileReportCommand = new CompileReportCommand(this);
            ClearReportInputCommand = new ClearReportInputCommand(this);
            SaveReportInputCommand = new SaveReportInputCommand(this);

            InitializeEventSubscriptions();
            InitializeReportCollections();
            ReportData = new List<KeyValuePair<string, int>>();
        }

        private void InitializeEventSubscriptions()
        {
            eventAggregator.GetEvent<RequestsCountByCategoryReportEvent>()
                           .Subscribe(HandleRequestsCountByCategoryReportEvent, ThreadOption.UIThread, RequestForQuoteConstants.MAINTAIN_STRONG_REFERENCE);
            
        }

        private void InitializeReportCollections()
        {
            ListOfReportTypes = new List<KeyValuePair<string, string>>()
                {
                    new KeyValuePair<string, string>(RegionNames.PIE_CHART_USER_CONTROL_REGION, "Pie Chart"),
                    new KeyValuePair<string, string>(RegionNames.BAR_CHART_USER_CONTROL_REGION, "Bar Chart"),
                    new KeyValuePair<string, string>(RegionNames.LINE_GRAPH_USER_CONTROL_REGION, "Line Graph"),
                    new KeyValuePair<string, string>(RegionNames.TABULATION_USER_CONTROL_REGION, "Tabulated Data"),
                };
            ListOfCategoryTypes = new List<KeyValuePair<string, string>>()
                {
                    new KeyValuePair<string, string>("Client", "By Client"),
                    new KeyValuePair<string, string>("BookCode", "By Book Code"),
                    new KeyValuePair<string, string>("Underlying", "By Underlying"),
                    new KeyValuePair<string, string>("Initiator", "By Initiator"),
                };
            ReportData = new List<KeyValuePair<string, int>>();
        }

        private void HandleRequestsCountByCategoryReportEvent(RequestsCountByCategoryReportEventPayLoad eventPayLoad)
        {
            ReportData.Clear();
            ReportData.AddRange(eventPayLoad.CountByCategory);
            ReportTitle = eventPayLoad.ReportDescription + " Showing Request Count " + eventPayLoad.CategoryDescription + ":";

            regionManager.RequestNavigate(RegionNames.GENERATED_REPORT_USER_CONTROL_REGION, new Uri(eventPayLoad.ReportType, UriKind.Relative));

            IWindowPopup reportWindow = ServiceLocator.Current.GetInstance<IWindowPopup>(WindowPopupNames.REPORT_WINDOW_POPUP);
            reportWindow.ShowWindow(this);
        }

        private void NotifyPropertyChanged(string propertyName)
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
        }

        public DateTime? FromDate
        {
            get { return (DateTime?)GetValue(FromDateProperty); }
            set { SetValue(FromDateProperty, value); }
        }

        // Using a DependencyProperty as the backing store for FromDate.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty FromDateProperty =
            DependencyProperty.Register("FromDate", typeof(DateTime?), typeof(RequestForQuoteReportsViewModel), new UIPropertyMetadata(null));

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
            reportingManager.GetRequestCountPerCategory(ReportType, RequestsCountCategory, 
                FromDate.GetValueOrDefault(new DateTime(2013, 1, 1)), MinimumCount.GetValueOrDefault(0));
            
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
