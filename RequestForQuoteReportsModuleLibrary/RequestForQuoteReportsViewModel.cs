using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Windows;
using System.Windows.Input;
using Microsoft.Practices.Prism.Events;
using Microsoft.Practices.Prism.Regions;
using Microsoft.Practices.ServiceLocation;
using RequestForQuoteInterfacesLibrary.Constants;
using RequestForQuoteInterfacesLibrary.Enums;
using RequestForQuoteInterfacesLibrary.EventPayloads;
using RequestForQuoteInterfacesLibrary.Events;
using RequestForQuoteInterfacesLibrary.ServiceInterfaces;
using RequestForQuoteInterfacesLibrary.WindowInterfaces;
using RequestForQuoteReportsModuleLibrary.Commands;
using log4net;

namespace RequestForQuoteReportsModuleLibrary
{
    public sealed class RequestForQuoteReportsViewModel : DependencyObject, INotifyPropertyChanged
    {
        internal sealed class StringComparer : IComparer<KeyValuePair<String, decimal>> 
        {
            public int Compare(KeyValuePair<String, decimal> firstParam, KeyValuePair<String, decimal> secondParam)
            {
                return firstParam.Key.CompareTo(secondParam.Key);
            }            
        }

        internal sealed class NumericComparer : IComparer<KeyValuePair<String, decimal>>
        {
            public int Compare(KeyValuePair<String, decimal> firstParam, KeyValuePair<String, decimal> secondParam)
            {
                var first = Convert.ToDecimal(firstParam.Key);
                var second = Convert.ToDecimal(secondParam.Key);
                return first.CompareTo(second);
            }
        }

        private static readonly ILog log = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        private readonly IEventAggregator eventAggregator;
        private readonly IReportDataManager reportingManager;
        
        private string requestsCountCategory;
        private string rangeVariable;

        public ICommand CompileReportCommand { get; set; }
        public ICommand ClearReportInputCommand { get; set; }
        public ICommand SaveReportInputCommand { get; set; }

        public List<KeyValuePair<string, string>> ListOfReportTypes { get; set; }
        public List<KeyValuePair<string, string>> ListOfCategoryTypes { get; set; }
        public List<KeyValuePair<string, string>> ListOfRangeVariables { get; set; }

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

            CompileReportCommand = new CompileReportCommand(this);
            ClearReportInputCommand = new ClearReportInputCommand(this);
            SaveReportInputCommand = new SaveReportInputCommand(this);

            InitializeEventSubscriptions();
            InitializeReportCollections();

            FromDateType = RequestCountFromDateEnum.FROM_DATE;
            RangeIncrement = 1;
        }

        /// <summary>
        /// Subscribes to report events using the PRISM event aggregator.
        /// </summary>
        private void InitializeEventSubscriptions()
        {
            eventAggregator.GetEvent<GreeksExtrapolationReportEvent>()
                           .Subscribe(HandleGreeksExtrapolationyReportEvent, ThreadOption.UIThread, RequestForQuoteConstants.MAINTAIN_STRONG_REFERENCE);

            eventAggregator.GetEvent<RequestsCountByCategoryReportEvent>()
                           .Subscribe(HandleRequestsCountByCategoryReportEvent, ThreadOption.UIThread, RequestForQuoteConstants.MAINTAIN_STRONG_REFERENCE);

            eventAggregator.GetEvent<GreeksByCategoryReportEvent>()
                           .Subscribe(HandleGreeksByCategoryReportEvent, ThreadOption.UIThread, RequestForQuoteConstants.MAINTAIN_STRONG_REFERENCE);

            eventAggregator.GetEvent<RequestSelectionEvent>()
                           .Subscribe(HandleRequestSelectionEvent, ThreadOption.UIThread, RequestForQuoteConstants.MAINTAIN_STRONG_REFERENCE);
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
                new KeyValuePair<string, string>(RegionNames.AREA_SERIES_USER_CONTROL_REGION, "Area Series"),
                new KeyValuePair<string, string>(RegionNames.GREEK_SERIES_USER_CONTROL_REGION, "Greek Series"),
            };
            ListOfCategoryTypes = new List<KeyValuePair<string, string>>()
            {
                new KeyValuePair<string, string>("Client", "By Client"),
                new KeyValuePair<string, string>("BookCode", "By Book Code"),
                new KeyValuePair<string, string>("Underlying", "By Underlying"),
                new KeyValuePair<string, string>("Initiator", "By Initiator"),
                new KeyValuePair<string, string>("TradeDate", "By Trade Date"),
                new KeyValuePair<string, string>("MaturityDate", "By Maturity Date"),
                new KeyValuePair<string, string>("Status", "By Status"),
                new KeyValuePair<string, string>("Picker", "By Picker"),
            };
            ListOfRangeVariables = new List<KeyValuePair<string, string>>()
            {
                new KeyValuePair<string, string>("UNDERLYING_PRICE", "By underlying price"),
                new KeyValuePair<string, string>("VOLATILITY", "By volatility"),
                new KeyValuePair<string, string>("INTEREST_RATE", "By interest rate"),
                new KeyValuePair<string, string>("TIME_TO_EXPIRY", "By time to expiry"),
                new KeyValuePair<string, string>("STRIKE", "By strike"),
            };
        }

        private void HandleRequestSelectionEvent(RequestSelectionEventPayload eventPayload)
        {
            RequestId = eventPayload.RequestId;
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

            if (eventPayLoad.CountByCategory.Count == 0)
            {
                MessageBox.Show("No RFQ data returned for the selected criteria!", "No Report Data To Display", 
                    MessageBoxButton.OK, MessageBoxImage.Information);

                return;
            }
            
            var reportViewModel = new GeneratedReportViewModel()
                {
                    ReportTitle = "Request Count By " + eventPayLoad.Category + ":",
                    ReportType = eventPayLoad.ReportType
                };
            reportViewModel.AddSeries(GeneratedReportViewModel.ONLY_ONE_SERIES, eventPayLoad.CountByCategory.ToList());

            var reportWindow = ServiceLocator.Current.GetInstance<IWindowPopup>(WindowPopupNames.REPORT_WINDOW_POPUP);
            reportWindow.ShowWindow(reportViewModel);
        }

        /// <summary>
        /// Prcoess the incoming extrapolation data event message, and creates and shows an instance of the report popup window with the appropriate chart.
        /// It uses the service locator to get an instance of the report popup window.
        /// </summary>
        /// <param name="eventPayLoad"> the GreeksExtrapolationReportEventPayLoad event sent by the ReportDataManagerImpl.</param>
        /// <exception cref="ArgumentNullException"> thrown if the eventpayload parameter is null.</exception>
        private void HandleGreeksExtrapolationyReportEvent(GreeksExtrapolationReportEventPayLoad eventPayLoad)
        {
            if (eventPayLoad == null)
                throw new ArgumentNullException("eventPayLoad");

            if (eventPayLoad.OutputExtrapolation.Count == 0)
            {
                MessageBox.Show("No extrapolation data returned for the selected criteria!", "No Report Data To Display",
                    MessageBoxButton.OK, MessageBoxImage.Information);

                return;
            }

            var reportViewModel = new GeneratedReportViewModel
            {
                ReportTitle = "Extrapolation of " + eventPayLoad.RangeVariable + ":",
                ReportType = eventPayLoad.ReportType,
            };

            foreach (var extrapolationPoint in eventPayLoad.OutputExtrapolation)
            {
                var listOfExtrapolations = extrapolationPoint.Value.ToList();
                listOfExtrapolations.Sort(new NumericComparer());
                reportViewModel.AddSeries(extrapolationPoint.Key, listOfExtrapolations);
            }
                

            var reportWindow = ServiceLocator.Current.GetInstance<IWindowPopup>(WindowPopupNames.REPORT_WINDOW_POPUP);
            reportWindow.ShowWindow(reportViewModel);
        }

        /// <summary>
        /// Prcoess the incoming report data event message, and creates and shows an instance of the report popup window with the appropriate chart.
        /// It uses the service locator to get an instance of the report popup window.
        /// </summary>
        /// <param name="eventPayLoad"> the GreeksByCategoryReportEventPayLoad event sent by the ReportDataManagerImpl.</param>
        /// <exception cref="ArgumentNullException"> thrown if the eventpayload parameter is null.</exception>
        private void HandleGreeksByCategoryReportEvent(GreeksByCategoryReportEventPayLoad eventPayLoad)
        {
            if (eventPayLoad == null)
                throw new ArgumentNullException("eventPayLoad");

            if (eventPayLoad.GreeksByCategory.Count == 0)
            {
                MessageBox.Show("No greek data returned for the selected criteria!", "No Report Data To Display", 
                    MessageBoxButton.OK, MessageBoxImage.Information);
                
                return;
            }
            
            var reportViewModel = new GeneratedReportViewModel
                {
                    ReportTitle = "Greeks By " + eventPayLoad.Category + ":",
                    ReportType = eventPayLoad.ReportType,
                };

            foreach (var greeksBelongingToACatgeory in eventPayLoad.GreeksByCategory)
                reportViewModel.AddSeries(greeksBelongingToACatgeory.Key, greeksBelongingToACatgeory.Value.ToList());

            var reportWindow = ServiceLocator.Current.GetInstance<IWindowPopup>(WindowPopupNames.REPORT_WINDOW_POPUP);
            reportWindow.ShowWindow(reportViewModel);
        }

        public bool CanReportOnlyFromSpecificDate
        {
            get { return RequestsCountCategory != "TradeDate"; }
        }

        public string RangeVariable
        {
            get { return rangeVariable; }
            set 
            {
                rangeVariable = value;
                NotifyPropertyChanged("RangeVariable");
            }
        }

        public string RequestsCountCategory
        {
            get { return requestsCountCategory; }
            set
            {
                requestsCountCategory = value;
                NotifyPropertyChanged("RequestsCountCategory");
                NotifyPropertyChanged("CanReportOnlyFromSpecificDate");
            }
        }

        public string SelectedReportTab
        {
            get;
            set;
        }

        public bool ShowDelta
        {
            get { return (bool)GetValue(ShowDeltaProperty); }
            set { SetValue(ShowDeltaProperty, value); }
        }

        // Using a DependencyProperty as the backing store for ShowDelta.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty ShowDeltaProperty =
            DependencyProperty.Register("ShowDelta", typeof(bool), typeof(RequestForQuoteReportsViewModel), new UIPropertyMetadata(true));

        public int RequestId
        {
            get { return (int)GetValue(RequestIdProperty); }
            set { SetValue(RequestIdProperty, value); }
        }

        // Using a DependencyProperty as the backing store for RequestId.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty RequestIdProperty =
            DependencyProperty.Register("RequestId", typeof(int), typeof(RequestForQuoteReportsViewModel), new UIPropertyMetadata(0));
      
        public bool ShowGamma
        {
            get { return (bool)GetValue(ShowGammaProperty); }
            set { SetValue(ShowGammaProperty, value); }
        }

        // Using a DependencyProperty as the backing store for ShowGamma.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty ShowGammaProperty =
            DependencyProperty.Register("ShowGamma", typeof(bool), typeof(RequestForQuoteReportsViewModel), new UIPropertyMetadata(false));

        public bool ShowTheta
        {
            get { return (bool)GetValue(ShowThetaProperty); }
            set { SetValue(ShowThetaProperty, value); }
        }

        // Using a DependencyProperty as the backing store for ShowTheta.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty ShowThetaProperty =
            DependencyProperty.Register("ShowTheta", typeof(bool), typeof(RequestForQuoteReportsViewModel), new UIPropertyMetadata(false));

        public bool ShowPremium
        {
            get { return (bool)GetValue(ShowPremiumProperty); }
            set { SetValue(ShowPremiumProperty, value); }
        }

        // Using a DependencyProperty as the backing store for ShowPremium.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty ShowPremiumProperty =
            DependencyProperty.Register("ShowPremium", typeof(bool), typeof(RequestForQuoteReportsViewModel), new UIPropertyMetadata(false));

        public bool ShowVega
        {
            get { return (bool)GetValue(ShowVegaProperty); }
            set { SetValue(ShowVegaProperty, value); }
        }

        // Using a DependencyProperty as the backing store for ShowVega.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty ShowVegaProperty =
            DependencyProperty.Register("ShowVega", typeof(bool), typeof(RequestForQuoteReportsViewModel), new UIPropertyMetadata(false));

        public bool ShowRho
        {
            get { return (bool)GetValue(ShowRhoProperty); }
            set { SetValue(ShowRhoProperty, value); }
        }

        // Using a DependencyProperty as the backing store for ShowRho.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty ShowRhoProperty =
            DependencyProperty.Register("ShowRho", typeof(bool), typeof(RequestForQuoteReportsViewModel), new UIPropertyMetadata(false));
       
        public DateTime? FromDate
        {
            get { return (DateTime?)GetValue(FromDateProperty); }
            set { SetValue(FromDateProperty, value); }
        }

        // Using a DependencyProperty as the backing store for FromDate.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty FromDateProperty =
            DependencyProperty.Register("FromDate", typeof(DateTime?), typeof(RequestForQuoteReportsViewModel), new UIPropertyMetadata(null));

        public DateTime? MaturityDateFrom
        {
            get { return (DateTime?)GetValue(MaturityDateFromProperty); }
            set { SetValue(MaturityDateFromProperty, value); }
        }

        // Using a DependencyProperty as the backing store for MaturityDateFrom.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty MaturityDateFromProperty =
            DependencyProperty.Register("MaturityDateFrom", typeof(DateTime?), typeof(RequestForQuoteReportsViewModel), new UIPropertyMetadata(null));


        public DateTime? MaturityDateTo
        {
            get { return (DateTime?)GetValue(MaturityDateToProperty); }
            set { SetValue(MaturityDateToProperty, value); }
        }

        // Using a DependencyProperty as the backing store for MaturityDateTo.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty MaturityDateToProperty =
            DependencyProperty.Register("MaturityDateTo", typeof(DateTime?), typeof(RequestForQuoteReportsViewModel), new UIPropertyMetadata(null));
                
        public String ReportType
        {
            get { return (String)GetValue(ReportTypeProperty); }
            set { SetValue(ReportTypeProperty, value); }
        }

        // Using a DependencyProperty as the backing store for ReportType.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty ReportTypeProperty =
            DependencyProperty.Register("ReportType", typeof(String), typeof(RequestForQuoteReportsViewModel), new UIPropertyMetadata(""));

        public double MinimumGreek
        {
            get { return (double)GetValue(MinimumGreekProperty); }
            set { SetValue(MinimumGreekProperty, value); }
        }

        // Using a DependencyProperty as the backing store for MinimumGreek.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty MinimumGreekProperty =
            DependencyProperty.Register("MinimumGreek", typeof(double), typeof(RequestForQuoteReportsViewModel), new UIPropertyMetadata(0.0));

        public double RangeMinimum
        {
            get { return (double)GetValue(RangeMinimumProperty); }
            set { SetValue(RangeMinimumProperty, value); }
        }

        // Using a DependencyProperty as the backing store for RangeMinimum.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty RangeMinimumProperty =
            DependencyProperty.Register("RangeMinimum", typeof(double), typeof(RequestForQuoteReportsViewModel), new UIPropertyMetadata(0.0));

        public double RangeMaximum
        {
            get { return (double)GetValue(RangeMaximumProperty); }
            set { SetValue(RangeMaximumProperty, value); }
        }

        // Using a DependencyProperty as the backing store for RangeMaximum.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty RangeMaximumProperty =
            DependencyProperty.Register("RangeMaximum", typeof(double), typeof(RequestForQuoteReportsViewModel), new UIPropertyMetadata(0.0));



        public double RangeIncrement
        {
            get { return (double)GetValue(RangeIncrementProperty); }
            set { SetValue(RangeIncrementProperty, value); }
        }

        // Using a DependencyProperty as the backing store for RangeIncrement.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty RangeIncrementProperty =
            DependencyProperty.Register("RangeIncrement", typeof(double), typeof(RequestForQuoteReportsViewModel), new UIPropertyMetadata(0.0));

        
        public int MinimumCount
        {
            get { return (int)GetValue(MinimumCountProperty); }
            set { SetValue(MinimumCountProperty, value); }
        }

        // Using a DependencyProperty as the backing store for MinimumCount.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty MinimumCountProperty =
            DependencyProperty.Register("MinimumCount", typeof(int), typeof(RequestForQuoteReportsViewModel), new UIPropertyMetadata(0));

        public RequestCountFromDateEnum FromDateType
        {
            get { return (RequestCountFromDateEnum)GetValue(FromDateTypeProperty); }
            set { SetValue(FromDateTypeProperty, value); }
        }

        // Using a DependencyProperty as the backing store for FromDateType.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty FromDateTypeProperty =
            DependencyProperty.Register("FromDateType", typeof(RequestCountFromDateEnum), typeof(RequestForQuoteReportsViewModel), new UIPropertyMetadata(RequestCountFromDateEnum.ALL));
        
        /// <summary>
        /// Returns true if the report input controls have valid contents needed to generate a report.
        /// </summary>
        /// <returns> true if ReportType property is not null or empty, and if the RequestsCountCategory is not null or empty, 
        /// and if MinimumCount is greater than or equal zero </returns>
        public bool CanCompileResport()
        {
            return !String.IsNullOrEmpty(ReportType) && MinimumCount >= 0;
        }

        /// <summary>
        /// Sends a request for report data to the ReportDataManager.
        /// </summary>
        public void CompileReport()
        {
            switch (SelectedReportTab)
            {
                case "RFQs/Category":
                    CompileRequestCountByCategoryReport();
                    break;
                case "Greek/Category":
                    CompileGreeksByCategoryReport();
                    break;
                case "Extrapolation":
                    CompileGreeksExtrapolation();
                    break;
            }          
        }

        /// <summary>
        /// Compiles the set of greeks that need to be displayed in a report.
        /// </summary>
        /// <returns> the set of greeks</returns>
        ISet<string> GetSetofGreeksToDisplayInReport()
        {
            ISet<string> setOfGreeks = new HashSet<string>();
            
            if (ShowDelta)
                setOfGreeks.Add(GreeksEnum.DELTA.ToString());

            if (ShowGamma)
                setOfGreeks.Add(GreeksEnum.GAMMA.ToString());

            if (ShowVega)
                setOfGreeks.Add(GreeksEnum.VEGA.ToString());

            if (ShowTheta)
                setOfGreeks.Add(GreeksEnum.THETA.ToString());

            if (ShowRho)
                setOfGreeks.Add(GreeksEnum.RHO.ToString());

            if (ShowPremium)
                setOfGreeks.Add(GreeksEnum.PREMIUM.ToString());

            return setOfGreeks;
        }

        /// <summary>
        /// Compiles the greeks by catgeory by delegating this request to the webservice
        /// </summary>
        private void CompileGreeksByCategoryReport()
        {
            if (FromDateType == RequestCountFromDateEnum.TODAY_ONLY)
                MaturityDateFrom = DateTime.Parse(DateTime.Now.ToShortDateString());
            
            reportingManager.CompileGreeksByCategoryReport(ReportType, 
                                                            RequestsCountCategory,
                                                            GetSetofGreeksToDisplayInReport(),
                                                            MaturityDateFrom.GetValueOrDefault(new DateTime(2013, 1, 1)),
                                                            MaturityDateTo.GetValueOrDefault(DateTime.Parse(DateTime.Now.ToShortDateString())),
                                                            MinimumGreek);
        }


        /// <summary>
        /// Compiles the greeks extrapolation report by delegating this request to the web service.
        /// The method returns without generating a report if the RequestId value is not greater than zero.
        /// </summary>
        private void CompileGreeksExtrapolation()
        {
            if (RequestId <= 0)
            {
                MessageBox.Show("An RFQ (with a valid identifier) has not been selected. Pls select one...");
                return;
            }                

            if (FromDateType == RequestCountFromDateEnum.TODAY_ONLY)
                MaturityDateFrom = DateTime.Parse(DateTime.Now.ToShortDateString());

            reportingManager.CompileGreeksExtrapolationReport(ReportType,
                                                            RangeVariable,
                                                            GetSetofGreeksToDisplayInReport(),
                                                            RequestId,
                                                            RangeMinimum,
                                                            RangeMaximum, 
                                                            RangeIncrement);
        }

        /// <summary>
        /// Compiles the RFQ count by category report by delegating this request to the web service.
        /// </summary>
        private void CompileRequestCountByCategoryReport()
        {
            if (FromDateType == RequestCountFromDateEnum.TODAY_ONLY)
                FromDate = DateTime.Parse(DateTime.Now.ToShortDateString());
            else if (FromDateType == RequestCountFromDateEnum.ALL)
                FromDate = new DateTime(2013, 1, 1);

            reportingManager.CompileRequestCountByCategoryReport(ReportType, RequestsCountCategory,
                FromDate.GetValueOrDefault(new DateTime(2013, 1, 1)), MinimumCount);              
        }

        /// <summary>
        /// Returns true if the report input controls can be cleared of their contents.
        /// </summary>
        /// <returns>true if ReportType property is null or empty or if the RequestsCountCategory is null 
        /// or empty, or if the MinimumCount has a value greater than zero.</returns>
        public bool CanClearReportInput()
        {
            return !String.IsNullOrEmpty(ReportType) || !String.IsNullOrEmpty(RequestsCountCategory) || MinimumCount > 0 || FromDate != null;
        }

        /// <summary>
        /// Clears the report input controls of their content.
        /// </summary>
        public void ClearReportInput()
        {
            ReportType = "";
            RequestsCountCategory = "";
            
            MinimumCount = 0;
            MinimumGreek = 0.0;
            RangeIncrement = 1;
            RangeMaximum = 0;
            RangeMinimum = 0;
            RangeVariable = "";
            
            FromDate = null;
            FromDateType = RequestCountFromDateEnum.FROM_DATE;
            
            ShowDelta = true;
            ShowGamma = false;
            ShowVega = false;
            ShowTheta = false;
            ShowRho = false;
            ShowPremium = false;
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
            return !String.IsNullOrEmpty(ReportType);
        }

        public event PropertyChangedEventHandler PropertyChanged;

        private void NotifyPropertyChanged(string propertyName)
        {
            var handler = PropertyChanged;
            if (handler != null) handler(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
