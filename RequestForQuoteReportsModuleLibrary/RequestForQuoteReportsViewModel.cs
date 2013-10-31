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
using RequestForQuoteInterfacesLibrary.ServiceInterfaces;
using RequestForQuoteInterfacesLibrary.WindowInterfaces;
using RequestForQuoteReportsModuleLibrary.Commands;
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

        /// <summary>
        /// Constructor of the view model that initializes various private services, creates instances of command properties, 
        /// and calls collection and event initialization methods.
        /// </summary>
        /// <param name="eventAggregator"> the PRISM event aggregator used to publish and subscribe to report related events.</param>
        /// <param name="reportingManager"> the report manager used to initiate report requests to the back-end.</param>
        /// <param name="regionManager"> the PRISM region manager used to navigate to the various charting regions.</param>
        /// <exception cref="ArgumentNullException"> thrown if the eventAggregator/reportingManager/regionManager parameters are null.</exception>
        public RequestForQuoteReportsViewModel(IEventAggregator eventAggregator, IReportDataManager reportingManager, IRegionManager regionManager)
        {
            if (eventAggregator == null)
                throw new ArgumentNullException("eventAggregator");

            if (reportingManager == null)
                throw new ArgumentNullException("reportingManager");

            if (regionManager == null)
                throw new ArgumentNullException("regionManager");

            this.eventAggregator = eventAggregator;
            this.reportingManager = reportingManager;
            this.regionManager = regionManager;

            CompileReportCommand = new CompileReportCommand(this);
            ClearReportInputCommand = new ClearReportInputCommand(this);
            SaveReportInputCommand = new SaveReportInputCommand(this);

            InitializeEventSubscriptions();
            InitializeReportCollections();
        }

        /// <summary>
        /// Subscribes to report events using the PRISM event aggregator.
        /// </summary>
        private void InitializeEventSubscriptions()
        {
            eventAggregator.GetEvent<RequestsCountByCategoryReportEvent>()
                           .Subscribe(HandleRequestsCountByCategoryReportEvent, ThreadOption.UIThread, RequestForQuoteConstants.MAINTAIN_STRONG_REFERENCE);            
        }

        /// <summary>
        /// Initializes the lists of categories that RFQs will be grouped by, 
        /// and chart report types will be display the reported data.
        /// These lists are properties accessible to the XAML comboboxes.
        /// </summary>
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

        /// <summary>
        /// Prcoess the incoming report data event message, and creates and shows an instance of the report popup window with the appropriate chart.
        /// It uses the service locator to get an instance of the report popup window.
        /// </summary>
        /// <param name="eventPayLoad"> the RequestsCountByCategoryReportEventPayLoad event sent by the ReportDataManagerImpl.</param>
        /// <exception cref="ArgumentNullException"> thrown if the eventpayload paramter is null.</exception>
        private void HandleRequestsCountByCategoryReportEvent(RequestsCountByCategoryReportEventPayLoad eventPayLoad)
        {
            if (eventPayLoad == null)
                throw new ArgumentNullException("eventPayLoad");

            ReportData.Clear();
            ReportData.AddRange(eventPayLoad.CountByCategory);
            ReportTitle = "Request Count By " + eventPayLoad.Category + ":";

            var reportWindow = ServiceLocator.Current.GetInstance<IWindowPopup>(WindowPopupNames.REPORT_WINDOW_POPUP);
            var reportUri = new Uri(eventPayLoad.ReportType, UriKind.Relative);
            regionManager.RequestNavigate(RegionNames.GENERATED_REPORT_USER_CONTROL_REGION, reportUri, NavigationCallback);

            reportWindow.ShowWindow(this);
        }

        /// <summary>
        /// Logs any errors that occur during navigation to the appropriate chart report.
        /// </summary>
        /// <param name="navigationResult"></param>
        private void NavigationCallback(NavigationResult navigationResult)
        {
            if (navigationResult.Result == false)
            {
                // TODO: Make sure error is ENABLED. It is not right now.            
                if(log.IsErrorEnabled)
                    log.Error("Could not navigate to another region");
            }
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

        /// <summary>
        /// Returns true if the report input controls have valid contents needed to generate a report.
        /// </summary>
        /// <returns> true if ReportType property is not null or empty, and if the RequestsCountCategory is not null or empty </returns>
        public bool CanCompileResport()
        {
            return !String.IsNullOrEmpty(ReportType) && !String.IsNullOrEmpty(RequestsCountCategory);
        }

        /// <summary>
        /// Sends a request for report data to the ReportDataManager.
        /// Defaults the trade date of the RFQs partcipating in the reporting to Jan 1 2013.
        /// Defaults the minimum count of RFQs that will be excluded to zero. All RFQ counts greater than zero will be returned.
        /// </summary>
        public void CompileReport()
        {
            reportingManager.GetRequestCountPerCategory(ReportType, RequestsCountCategory, 
                FromDate.GetValueOrDefault(new DateTime(2013, 1, 1)), MinimumCount.GetValueOrDefault(0));           
        }

        /// <summary>
        /// Clears the report input controls of their content.
        /// </summary>
        private void ClearRequestsPerCategoryInputs()
        {
            ReportType = "";
            RequestsCountCategory = "";
            MinimumCount = null;
        }

        /// <summary>
        /// Returns true if the report input controls can be cleared of their contents.
        /// </summary>
        /// <returns>true if ReportType property is null or empty or if the RequestsCountCategory is null, 
        /// or empty or if the MinimumCount has a non-null value.</returns>
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

        /// <summary>
        /// Returns true if the report input controls have valid contents needed to save a report.
        /// </summary>
        /// <returns> true if ReportType property is not null or empty, and if the RequestsCountCategory is not null or empty.</returns>
        public bool CanSaveReportInput()
        {
            return !String.IsNullOrEmpty(ReportType) && !String.IsNullOrEmpty(RequestsCountCategory);
        }
    }
}
