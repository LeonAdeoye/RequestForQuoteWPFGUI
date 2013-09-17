using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;
using System.Windows.Input;
using System.ComponentModel;
using System.Windows.Data;
using Microsoft.Practices.Prism.Commands;
using Microsoft.Practices.Prism.Events;
using Microsoft.Practices.ServiceLocation;
using RequestForQuoteGridModuleLibrary.Commands;
using RequestForQuoteInterfacesLibrary.Constants;
using RequestForQuoteInterfacesLibrary.Enums;
using RequestForQuoteInterfacesLibrary.EventPayloads;
using RequestForQuoteInterfacesLibrary.Events;
using RequestForQuoteInterfacesLibrary.ModelImplementations;
using RequestForQuoteInterfacesLibrary.ModelInterfaces;
using RequestForQuoteInterfacesLibrary.ServiceInterfaces;
using RequestForQuoteInterfacesLibrary.WindowInterfaces;
using log4net;

namespace RequestForQuoteGridModuleLibrary
{
    public sealed class RequestForQuoteGridViewModel : DependencyObject, INotifyPropertyChanged
    {
        private static readonly ILog log = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        private readonly IOptionRequestParser optionRequestParser;
        private readonly IOptionRequestPricer optionRequestPricer;
        private readonly IClientManager clientManager;
        private readonly IBookManager bookManager;
        private readonly IChatServiceManager chatServiceManager;
        private readonly IUnderlyingManager underlyingManager;
        private readonly IEventAggregator eventAggregator;
  
        public ObservableCollection<IRequestForQuote> Requests { get; set; }
        public ObservableCollection<IRequestForQuote> SearchedRequests { get; set; }
        public ObservableCollection<IRequestForQuote> TodaysRequests { get; set; }
        
        public ObservableCollection<IClient> Clients { get; set; }
        public ObservableCollection<IBook> Books { get; set; }
        public List<string> Status { get; set; }

        public ICommand CloneRequestCommand { get; set; }
        public ICommand DeleteRequestCommand { get; set; }
        public ICommand InvalidateRequestCommand { get; set; }
        public ICommand ChangeStatusOfRequestCommand { get; set; }
        public ICommand GroupByCommand { get; set; }
        public ICommand CalculateRequestCommand { get; set; }
        public ICommand ShowRequestDetailsWindowCommand { get; set; }
        public ICommand PickUpRequestCommand { get; set; }

        public IClient NewRequestClient { get; set; }
        public IRequestForQuote SelectedRequest { get; set; }

        private int identifier = 9;

        public RequestForQuoteGridViewModel(IBookManager bookManager, IClientManager clientManager, IOptionRequestParser optionRequestParser,
            IOptionRequestPricer optionRequestPricer, IChatServiceManager chatServiceManager, IUnderlyingManager underlyingManager, IEventAggregator eventAggregator)
        {
            Requests = new ObservableCollection<IRequestForQuote>();
            SearchedRequests = new ObservableCollection<IRequestForQuote>();
            TodaysRequests = new ObservableCollection<IRequestForQuote>();

            CloneRequestCommand = new CloneRequestCommand(this);
            DeleteRequestCommand = new DeleteRequestCommand(this);
            InvalidateRequestCommand = new InvalidateRequestCommand(this);
            ChangeStatusOfRequestCommand = new ChangeStatusOfRequestCommand(this);
            CalculateRequestCommand = new CalculateRequestCommand(this);
            GroupByCommand = new GroupByCommand(this);
            ShowRequestDetailsWindowCommand = new ShowRequestDetailsWindowCommand(this);
            PickUpRequestCommand = new DelegateCommand(PickUpRequest);

            this.optionRequestParser = optionRequestParser;
            this.optionRequestPricer = optionRequestPricer;
            this.clientManager = clientManager;
            this.bookManager = bookManager;
            this.chatServiceManager = chatServiceManager;
            this.underlyingManager = underlyingManager;
            this.eventAggregator = eventAggregator;

            InitializeCollections();
            InitializeEventSubscriptions();
        }

        private void InitializeCollections()
        {
            Clients = new ObservableCollection<IClient>(clientManager.Clients);
            Books = new ObservableCollection<IBook>(bookManager.Books);
            
            // TODO change Java BE to use text.
            Status = new List<string>();
            foreach (var status in Enum.GetNames(typeof(StatusEnum)))
                Status.Add(status);

            TodaysRequests.Add(new RequestForQuoteImpl() { Request = "C 100 23Dec2013 0001.HK", Status = StatusEnum.PENDING, Identifier = 1, Client = "Nomura Securities", TradeDate = DateTime.Today, NotionalCurrency = CurrencyEnum.EUR, BookCode = "AB01" , HedgeType = HedgeTypeEnum.SHARES});
            TodaysRequests.Add(new RequestForQuoteImpl() { Request = "P 110 23Dec2013 0002.HK", Status = StatusEnum.FILLED, Identifier = 2, Client = "Morgan Stanley", TradeDate = DateTime.Today, NotionalCurrency = CurrencyEnum.EUR, BookCode = "AB02", HedgeType = HedgeTypeEnum.SHARES });
            TodaysRequests.Add(new RequestForQuoteImpl() { Request = "C+P 90 23Dec2013 0003.HK", Status = StatusEnum.PICKEDUP, Identifier = 3, Client = "Goldman Sachs", TradeDate = DateTime.Today, NotionalCurrency = CurrencyEnum.HKD, BookCode = "AB01", HedgeType = HedgeTypeEnum.SHARES });
            TodaysRequests.Add(new RequestForQuoteImpl() { Request = "C-P 100 23Dec2013 0004.HK", Status = StatusEnum.TRADEDAWAY, Identifier = 4, Client = "JP Morgan", TradeDate = DateTime.Today, NotionalCurrency = CurrencyEnum.USD, BookCode = "AB02", HedgeType = HedgeTypeEnum.SHARES });
            TodaysRequests.Add(new RequestForQuoteImpl() { Request = "2C 120 23Dec2013 0005.HK", Status = StatusEnum.PENDING, Identifier = 5, Client = "Nomura Securities", TradeDate = new DateTime(2013, 6, 21), NotionalCurrency = CurrencyEnum.GBP, BookCode = "AB03", HedgeType = HedgeTypeEnum.SHARES });
            TodaysRequests.Add(new RequestForQuoteImpl() { Request = "-2C 100 23Dec2013 0006.HK", Status = StatusEnum.TRADEDAWAY, Identifier = 6, Client = "Goldman Sachs", TradeDate = new DateTime(2013, 6, 23), NotionalCurrency = CurrencyEnum.SGD, BookCode = "AB03", HedgeType = HedgeTypeEnum.SHARES });
            TodaysRequests.Add(new RequestForQuoteImpl() { Request = "-2C+P 100 23Dec2013 0007.HK", Status = StatusEnum.PENDING, Identifier = 7, Client = "Morgan Stanley", TradeDate = DateTime.Today, NotionalCurrency = CurrencyEnum.GBP, BookCode = "AB01", HedgeType = HedgeTypeEnum.SHARES });
            TodaysRequests.Add(new RequestForQuoteImpl() { Request = "2P 100 23Dec2013 0008.HK", Status = StatusEnum.FILLED, Identifier = 8, Client = "Goldman Sachs", TradeDate = DateTime.Today, NotionalCurrency = CurrencyEnum.JPY, BookCode = "AB02", HedgeType = HedgeTypeEnum.SHARES });
            TodaysRequests.Add(new RequestForQuoteImpl() { Request = "2P-C 110 23Dec2013 0009.HK", Status = StatusEnum.PENDING, Identifier = 9, Client = "Goldman Sachs", TradeDate = DateTime.Today, NotionalCurrency = CurrencyEnum.USD, BookCode = "AB03", HedgeType = HedgeTypeEnum.SHARES });
            
            Requests = TodaysRequests;
            NotifyPropertyChanged("Requests");
        }

        private void InitializeEventSubscriptions()
        {

            eventAggregator.GetEvent<NewRequestForQuoteEvent>()
                           .Subscribe(HandlePublishedNewRequestEvent, ThreadOption.UIThread, RequestForQuoteConstants.MAINTAIN_STRONG_REFERENCE);

            eventAggregator.GetEvent<ClosedRequestForQuoteDetailsEvent>()
                           .Subscribe(HandlePublishedClosedRequestEvent, ThreadOption.UIThread, RequestForQuoteConstants.MAINTAIN_STRONG_REFERENCE);

            eventAggregator.GetEvent<SearchRequestForQuoteEvent>()
                           .Subscribe(HandleBothFilterAndSearchRequests, ThreadOption.UIThread, RequestForQuoteConstants.MAINTAIN_STRONG_REFERENCE);

            eventAggregator.GetEvent<GetTodaysRequestsEvent>()
                           .Subscribe(HandleGetTodaysRequestsEvent, ThreadOption.UIThread, RequestForQuoteConstants.MAINTAIN_STRONG_REFERENCE);

            eventAggregator.GetEvent<NewClientEvent>()
                           .Subscribe(HandleNewClientEvent, ThreadOption.UIThread, RequestForQuoteConstants.MAINTAIN_STRONG_REFERENCE);

            eventAggregator.GetEvent<NewBookEvent>()
               .Subscribe(HandleNewBookEvent, ThreadOption.UIThread, RequestForQuoteConstants.MAINTAIN_STRONG_REFERENCE);
        }

        public bool CanShowDetailsWindow()
        {
            return SelectedRequest != null;
        }

        public void ShowDetailsWindow()
        {
            if (SelectedRequest.Popup == null)
            {
                IRequestForQuoteDetailsPopupWindow requestForQuotePopup = ServiceLocator.Current.GetInstance<IRequestForQuoteDetailsPopupWindow>();
                // TODO make this a ctor param:

                SelectedRequest.Popup = requestForQuotePopup;
                requestForQuotePopup.ShowWindow(new RequestForQuoteDetailsViewModel(optionRequestPricer, SelectedRequest, clientManager, 
                    bookManager, eventAggregator, underlyingManager, chatServiceManager));
            }
            else
                SelectedRequest.Popup.ActivateWindow();
        }

        public bool CanAddNewRequest()
        {
            var canDo = false;
            if (!string.IsNullOrEmpty(NewRequest) && optionRequestParser.IsValidOptionRequest(NewRequest))
            {
                IClient newRequestClient = NewRequestClient as IClient;
                if (newRequestClient != null)
                    canDo = !string.IsNullOrEmpty(newRequestClient.Name);
            }
            return canDo;             
        }

        public bool CanClearNewRequest()
        {
            var canClear = false;
            if (string.IsNullOrEmpty(this.NewRequest))
            {
                var newRequestClient = this.NewRequestClient;
                if (newRequestClient != null)
                    canClear = !string.IsNullOrEmpty(newRequestClient.Name);
            }
            else
                canClear = true;

            return canClear;
        }

        public bool CanInvalidateRequest()
        {
            return (SelectedRequest != null && SelectedRequest.Status != StatusEnum.INVALID);
        }

        public bool IsSelectRequestNull()
        {
            return (SelectedRequest == null);
        }

        public void HandleNewClientEvent(NewClientEventPayload eventPayLoad)
        {
            if (log.IsDebugEnabled)
                log.Debug("Received new client: " + eventPayLoad);

            Clients.Add(eventPayLoad.NewClient);
        }

        public void HandleNewBookEvent(NewBookEventPayload eventPayLoad)
        {
            if (log.IsDebugEnabled)
                log.Debug("Received new book: " + eventPayLoad);

            Books.Add(eventPayLoad.NewBook);
        }

        public void HandlePublishedNewRequestEvent(NewRequestForQuoteEventPayload eventPayload)
        {
            IRequestForQuote request = new RequestForQuoteImpl();
            request.Legs = optionRequestParser.ParseRequest(eventPayload.NewRequestText.ToUpper(), request);
            request.Request = eventPayload.NewRequestText;
            request.Status = StatusEnum.PENDING;
            request.Identifier = ++identifier;
            request.Client = eventPayload.NewRequestClient;
            request.TradeDate = DateTime.Today;
            request.CalculatePricing(optionRequestPricer);
            Requests.Add(request);
            // TODO

            if (log.IsDebugEnabled)
                log.Debug("Received published new request for quote => " + request);
        }

        public void HandleGetTodaysRequestsEvent(EmptyEventPayload emptyPayload)
        {
            if (log.IsDebugEnabled)
                log.Debug("Received request to reset grid to today's requests.");

            var view = CollectionViewSource.GetDefaultView(Requests);
            view.Filter = null;

            Requests = TodaysRequests;
            NotifyPropertyChanged("Requests");            
        }

        public void HandlePublishedClosedRequestEvent(ClosedRequestForQuoteDetailsEventPayload eventPayload)
        {
            if (log.IsDebugEnabled)
                log.Debug("Received published closed request for quote => " + eventPayload.RequestForQuoteIdentifer);
        }

        public void HandleBothFilterAndSearchRequests(CriteriaUsageEventPayload usageEventPayload)
        {
            if (usageEventPayload != null && usageEventPayload.IsFilter)
                HandlePublishedFilterRequestsEvent(usageEventPayload);
            else
                HandlePublishedSearchRequestsEvent(usageEventPayload);
        }

        public void HandlePublishedFilterRequestsEvent(CriteriaUsageEventPayload usageEventPayload)
        {
            if (log.IsDebugEnabled)
                log.Debug("Received published filter requests event => " + usageEventPayload);

            if (usageEventPayload.Criteria != null && usageEventPayload.Criteria.Count > 0)
            {
                var view = CollectionViewSource.GetDefaultView(Requests);
                view.Filter = (request) => DoesRequestMatchFilter(request as IRequestForQuote, usageEventPayload.Criteria);
            }
            else
            {
                Requests = TodaysRequests;
                NotifyPropertyChanged("Requests");
                var view = CollectionViewSource.GetDefaultView(Requests);
                view.Filter = null;    
            }                
        }

        private bool DoesRequestMatchFilter(IRequestForQuote request, Dictionary<string, string> criteria)
        {
            if (request != null)
            {
                foreach (var criterion in criteria)
                {
                    switch (criterion.Key)
                    {
                        case RequestForQuoteConstants.CLIENT_CRITERION:
                            if (request.Client != criterion.Value)
                                return false;
                            break;
                        case RequestForQuoteConstants.BOOK_CRITERION:
                            if (request.BookCode != criterion.Value)
                                return false;
                            break;
                        // TODO
                        // case RequestForQuoteConstants.UNDERLYIER_CRITERION:
                        //    if (request.RIC != criterion.Value)
                        //        return false;
                        //    break;
                        case RequestForQuoteConstants.STATUS_CRITERION:
                            if (request.Status != (StatusEnum)Enum.Parse(typeof(StatusEnum), criterion.Value))
                                return false;
                            break;
                        case RequestForQuoteConstants.TRADE_DATE_CRITERION:
                            if (!IsWithinDateRange(request.TradeDate, criterion.Value))
                                return false;
                            break;
                        case RequestForQuoteConstants.EXPIRY_DATE_CRITERION:
                            // TODO Add Expiry date and change...
                            if (!IsWithinDateRange(request.TradeDate, criterion.Value))
                                return false;
                            break;
                        default:
                            return false;
                    }
                }
            }
            return true;
        }

        private bool IsWithinDateRange(DateTime dateToCheck, string criterionValue)
        {
            try
            {
                var dates = criterionValue.Split('-').ToArray();
                dates = dates.Where(x => !string.IsNullOrEmpty(x)).ToArray();
                DateTime startDate, endDate;                    
                if (dates.Count() == 2)
                {
                    startDate = Convert.ToDateTime(dates[0]);
                    endDate = Convert.ToDateTime(dates[1]);
                    return dateToCheck >= startDate && dateToCheck <= endDate;
                }
                if (dates.Count() == 1 && criterionValue[0] != '-')
                {
                    startDate = Convert.ToDateTime(dates[0]);
                    return dateToCheck >= startDate;
                }
                if (dates.Count() == 1 && criterionValue[0] == '-')
                {
                    endDate = Convert.ToDateTime(dates[0]);
                    return dateToCheck <= endDate;                    
                }
            }
            catch (Exception raisedException)
            {
                log.Error(string.Format("Could not convert criterion dates [{0}] into two date ranges.", criterionValue), raisedException);
            }
            return false;
        }

        public void HandlePublishedSearchRequestsEvent(CriteriaUsageEventPayload eventPayload)
        {
            if (log.IsDebugEnabled)
                log.Debug("Received published search requests event => " + eventPayload);

            SearchedRequests.Clear();

            // TODO
            // Load from the database...
            SearchedRequests.Add(new RequestForQuoteImpl() { Request = "C+3P 100 23Dec2013 1001.T", Status = StatusEnum.PENDING, Identifier = 1, Client = "Nomura Securities", TradeDate = Convert.ToDateTime("1 Jan 2013"), NotionalCurrency = CurrencyEnum.EUR, BookCode = "AB01" });

            Requests = SearchedRequests;
            NotifyPropertyChanged("Requests");
        }

        public string NewRequest
        {
            get { return (string)GetValue(NewRequestProperty); }
            set { SetValue(NewRequestProperty, value); }
        }

        public static readonly DependencyProperty NewRequestProperty =
            DependencyProperty.Register("NewRequest", typeof(string), typeof(RequestForQuoteGridViewModel), new UIPropertyMetadata(String.Empty));

        public void GroupRequests(string groupBy)
        {
            var view = CollectionViewSource.GetDefaultView(Requests);
            view.GroupDescriptions.Clear();
            if (groupBy != "Undo Grouping")
                view.GroupDescriptions.Add(new PropertyGroupDescription(groupBy));
        }

        public void CloneRequest()
        {
            Requests.Add(SelectedRequest.Clone(++identifier));
        }

        public void DeleteRequest()
        {
            Requests.Remove(SelectedRequest as IRequestForQuote);
        }

        public void InvalidateRequest()
        {
            SelectedRequest.Status = StatusEnum.INVALID;
        }

        public void SaveRequest()
        {
            throw new NotImplementedException();
        }

        public void CalculateRequest(bool isFromContextMenu)
        {
            if (SelectedRequest != null &&  !SelectedRequest.CalculatePricing(optionRequestPricer))
            {
                if (isFromContextMenu)
                    MessageBox.Show("Failed to calculate pricing for request: " + SelectedRequest.Request, "Calculation Error", MessageBoxButton.OK, MessageBoxImage.Exclamation);

                log.Error("Failed to calculate pricing for request: " + SelectedRequest.Request);
            }
        }

        public bool CanCalculateRequest()
        {
            return !IsSelectRequestNull();
        }

        public void ClearNewRequest()
        {
            NewRequest = String.Empty;
        }

        public void ChangeStatusOfRequest(StatusEnum status)
        {
            if (!IsSelectRequestNull())
                SelectedRequest.Status = status;
        }

        public void PickUpRequest()
        {
            if (!IsSelectRequestNull())
            {
                SelectedRequest.PickedUpBy = RequestForQuoteConstants.MY_USER_NAME;
                SelectedRequest.Status = StatusEnum.PICKEDUP;

                if (log.IsDebugEnabled)
                    log.Debug("Request => " + SelectedRequest.Request + " picked up by => " + SelectedRequest.PickedUpBy);
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        private void NotifyPropertyChanged(string propertyName)
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
