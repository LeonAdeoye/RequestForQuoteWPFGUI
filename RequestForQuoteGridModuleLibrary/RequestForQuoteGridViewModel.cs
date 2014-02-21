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
using RequestForQuoteInterfacesLibrary.Utilities;
using RequestForQuoteInterfacesLibrary.WindowInterfaces;
using log4net;

namespace RequestForQuoteGridModuleLibrary
{
    public sealed class RequestForQuoteGridViewModel : DependencyObject, INotifyPropertyChanged
    {
        private static readonly ILog log = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        private readonly IOptionRequestParser optionRequestParser;
        private readonly IOptionRequestPricer optionRequestPricer;
        private readonly IOptionRequestPersistanceManager optionRequestPersistanceManager;
        private readonly IClientManager clientManager;
        private readonly IBookManager bookManager;
        private readonly IChatServiceManager chatServiceManager;
        private readonly IUnderlyingManager underlyingManager;
        private readonly IEventAggregator eventAggregator;
        private readonly IConfigurationManager configManager;
        private readonly IUserManager userManager;

        public RangeObservableCollection<IRequestForQuote> Requests { get; set; }
        public RangeObservableCollection<IRequestForQuote> SearchedRequests { get; set; }
        public RangeObservableCollection<IRequestForQuote> TodaysRequests { get; set; }
        
        public ObservableCollection<IClient> Clients { get; set; }
        public ObservableCollection<IBook> Books { get; set; }
        public ObservableCollection<IUser> Users { get; set; }
        public List<string> Status { get; set; }

        public ICommand CloneRequestCommand { get; set; }
        public ICommand DeleteRequestCommand { get; set; }
        public ICommand InvalidateRequestCommand { get; set; }
        public ICommand ChangeStatusOfRequestCommand { get; set; }
        public ICommand GroupByCommand { get; set; }
        public ICommand CalculateRequestCommand { get; set; }
        public ICommand ShowRequestDetailsWindowCommand { get; set; }
        public ICommand RequestSelectionCommand { get; set; }
        public ICommand PickUpRequestCommand { get; set; }

        public IClient NewRequestClient { get; set; }
        public IRequestForQuote SelectedRequest { get; set; }

        private int identifier = 9;

        public RequestForQuoteGridViewModel(IBookManager bookManager, IClientManager clientManager, IOptionRequestParser optionRequestParser,
            IOptionRequestPricer optionRequestPricer, IChatServiceManager chatServiceManager, IUnderlyingManager underlyingManager,
            IOptionRequestPersistanceManager optionRequestPersistanceManager, IEventAggregator eventAggregator, 
            IConfigurationManager configManager, IUserManager userManager)
        {
            if (optionRequestPricer == null)
                throw new ArgumentNullException("optionRequestPricer");

            if (optionRequestParser == null)
                throw new ArgumentNullException("optionRequestParser");

            if (clientManager == null)
                throw new ArgumentNullException("clientManager");

            if (bookManager == null)
                throw new ArgumentNullException("bookManager");

            if (userManager == null)
                throw new ArgumentNullException("userManager");

            if (underlyingManager == null)
                throw new ArgumentNullException("underlyingManager");

            if (chatServiceManager == null)
                throw new ArgumentNullException("chatServiceManager");

            if (eventAggregator == null)
                throw new ArgumentNullException("eventAggregator");

            if (optionRequestPersistanceManager == null)
                throw new ArgumentNullException("optionRequestPersistanceManager");

            if (configManager == null)
                throw new ArgumentNullException("configManager");

            this.optionRequestParser = optionRequestParser;
            this.optionRequestPricer = optionRequestPricer;
            this.clientManager = clientManager;
            this.bookManager = bookManager;
            this.userManager = userManager;
            this.chatServiceManager = chatServiceManager;
            this.underlyingManager = underlyingManager;
            this.eventAggregator = eventAggregator;
            this.optionRequestPersistanceManager = optionRequestPersistanceManager;
            this.configManager = configManager;

            InitializeCommands();
            InitializeCollections();
            InitializeEventSubscriptions();
        }

        private void InitializeCommands()
        {
            CloneRequestCommand = new CloneRequestCommand(this);
            DeleteRequestCommand = new DeleteRequestCommand(this);
            InvalidateRequestCommand = new InvalidateRequestCommand(this);
            ChangeStatusOfRequestCommand = new ChangeStatusOfRequestCommand(this);
            CalculateRequestCommand = new CalculateRequestCommand(this);
            GroupByCommand = new GroupByCommand(this);
            ShowRequestDetailsWindowCommand = new ShowRequestDetailsWindowCommand(this);
            PickUpRequestCommand = new DelegateCommand(PickUpRequest);
            RequestSelectionCommand = new DelegateCommand(RequestSelected);            
        }

        private void InitializeCollections()
        {
            Clients = new ObservableCollection<IClient>(clientManager.Clients);
            Books = new ObservableCollection<IBook>(bookManager.Books);
            Users = new ObservableCollection<IUser>(userManager.Users);
            SearchedRequests = new RangeObservableCollection<IRequestForQuote>();
            TodaysRequests = new RangeObservableCollection<IRequestForQuote>();
            Requests = new RangeObservableCollection<IRequestForQuote>();
            Requests = new RangeObservableCollection<IRequestForQuote>();
            Users = new ObservableCollection<IUser>();
            
            // TODO change Java BE to use text.
            Status = new List<string>();
            foreach (var status in Enum.GetNames(typeof(StatusEnum)))
                Status.Add(status);

            if(configManager.IsStandAlone)
            {
                TodaysRequests.Add(new RequestForQuoteImpl() { Request = "C 100 23Dec2013 0001.HK", Status = StatusEnum.PENDING, Identifier = 1, Client = Clients[0], TradeDate = DateTime.Today, NotionalCurrency = CurrencyEnum.EUR, BookCode = "AB01", HedgeType = HedgeTypeEnum.SHARES });
                TodaysRequests.Add(new RequestForQuoteImpl() { Request = "P 110 23Dec2013 0002.HK", Status = StatusEnum.FILLED, Identifier = 2, Client = Clients[1], TradeDate = DateTime.Today, NotionalCurrency = CurrencyEnum.EUR, BookCode = "AB02", HedgeType = HedgeTypeEnum.SHARES });
                TodaysRequests.Add(new RequestForQuoteImpl() { Request = "C+P 90 23Dec2013 0003.HK", Status = StatusEnum.PICKEDUP, Identifier = 3, Client = Clients[2], TradeDate = DateTime.Today, NotionalCurrency = CurrencyEnum.HKD, BookCode = "AB01", HedgeType = HedgeTypeEnum.SHARES });
                TodaysRequests.Add(new RequestForQuoteImpl() { Request = "C-P 100 23Dec2013 0004.HK", Status = StatusEnum.TRADEDAWAY, Identifier = 4, Client = Clients[0], TradeDate = DateTime.Today, NotionalCurrency = CurrencyEnum.USD, BookCode = "AB02", HedgeType = HedgeTypeEnum.SHARES });
                TodaysRequests.Add(new RequestForQuoteImpl() { Request = "2C 120 23Dec2013 0005.HK", Status = StatusEnum.PENDING, Identifier = 5, Client = Clients[1], TradeDate = new DateTime(2013, 6, 21), NotionalCurrency = CurrencyEnum.GBP, BookCode = "AB03", HedgeType = HedgeTypeEnum.SHARES });
                TodaysRequests.Add(new RequestForQuoteImpl() { Request = "-2C 100 23Dec2013 0006.HK", Status = StatusEnum.TRADEDAWAY, Identifier = 6, Client = Clients[2], TradeDate = new DateTime(2013, 6, 23), NotionalCurrency = CurrencyEnum.SGD, BookCode = "AB03", HedgeType = HedgeTypeEnum.SHARES });                
            }
            else       
                TodaysRequests.AddRange(optionRequestPersistanceManager.GetRequestsForToday(true));                            

            Requests.AddRange(TodaysRequests);
            NotifyPropertyChanged("Requests");
        }

        private void InitializeEventSubscriptions()
        {
            eventAggregator.GetEvent<NewRequestForQuoteEvent>()
                           .Subscribe(HandlePublishedNewRequestEvent, ThreadOption.UIThread, RequestForQuoteConstants.MAINTAIN_STRONG_REFERENCE);

            eventAggregator.GetEvent<SearchRequestForQuoteEvent>()
                           .Subscribe(HandleBothFilterAndSearchRequests, ThreadOption.UIThread, RequestForQuoteConstants.MAINTAIN_STRONG_REFERENCE);

            eventAggregator.GetEvent<GetTodaysRequestsEvent>()
                           .Subscribe(HandleGetTodaysRequestsEvent, ThreadOption.UIThread, RequestForQuoteConstants.MAINTAIN_STRONG_REFERENCE);

            eventAggregator.GetEvent<NewClientEvent>()
                           .Subscribe(HandleNewClientEvent, ThreadOption.UIThread, RequestForQuoteConstants.MAINTAIN_STRONG_REFERENCE);

            eventAggregator.GetEvent<NewSerializedRequestEvent>()
                           .Subscribe(HandleNewSerializedRequestEvent, ThreadOption.UIThread, RequestForQuoteConstants.MAINTAIN_STRONG_REFERENCE);

            eventAggregator.GetEvent<NewBookEvent>()
               .Subscribe(HandleNewBookEvent, ThreadOption.UIThread, RequestForQuoteConstants.MAINTAIN_STRONG_REFERENCE);

            eventAggregator.GetEvent<NewUserEvent>()
               .Subscribe(HandleNewUserEvent, ThreadOption.UIThread, RequestForQuoteConstants.MAINTAIN_STRONG_REFERENCE);
        }

        public bool CanShowDetailsWindow()
        {
            return !IsSelectedRequestNull();
        }

        // Unable to test
        public void ShowDetailsWindow()
        {
            if (SelectedRequest.Popup == null)
            {
                // Need to use service locator because of dependency module initialization issues.
                SelectedRequest.Popup = ServiceLocator.Current.GetInstance<IWindowPopup>(WindowPopupNames.REQUEST_DETAIL_WINDOW_POPUP);

                var viewModel = new RequestForQuoteDetailsViewModel(optionRequestPricer, SelectedRequest,
                                                                    clientManager, bookManager, eventAggregator,
                                                                    underlyingManager, chatServiceManager,
                                                                    optionRequestPersistanceManager, configManager, userManager);

                SelectedRequest.EditableViewModel = viewModel;
                SelectedRequest.EditableViewModel.BeginEdit();
                SelectedRequest.Popup.ShowWindow(viewModel);
            }
            else
            {
                SelectedRequest.EditableViewModel.BeginEdit();
                SelectedRequest.Popup.ShowWindow();    
            }                        
        }

        public bool CanAddNewRequest()
        {
            var canDo = false;
            if (!string.IsNullOrEmpty(NewRequest) && optionRequestParser.IsValidOptionRequest(NewRequest))
            {
                var newRequestClient = NewRequestClient;
                if (newRequestClient != null)
                    canDo = !string.IsNullOrEmpty(newRequestClient.Name);
            }
            return canDo;             
        }

        public bool CanClearNewRequest()
        {
            var canClear = false;
            if (string.IsNullOrEmpty(NewRequest))
            {
                var newRequestClient = NewRequestClient;
                if (newRequestClient != null)
                    canClear = !string.IsNullOrEmpty(newRequestClient.Name);
            }
            else
                canClear = true;

            return canClear;
        }

        public bool CanInvalidateRequest()
        {
            return (!IsSelectedRequestNull() && SelectedRequest.Status != StatusEnum.INVALID);
        }

        public bool IsSelectedRequestNull()
        {
            return (SelectedRequest == null);
        }

        public void HandleNewClientEvent(NewClientEventPayload eventPayload)
        {
            if (eventPayload == null)
                throw new ArgumentNullException("eventPayload");

            if (log.IsDebugEnabled)
                log.Debug("Received new client: " + eventPayload);

            Clients.Add(eventPayload.NewClient);
        }

        public void HandleNewBookEvent(NewBookEventPayload eventPayload)
        {
            if (eventPayload == null)
                throw new ArgumentNullException("eventPayload");

            if (log.IsDebugEnabled)
                log.Debug("Received new book: " + eventPayload);

            Books.Add(eventPayload.NewBook);
        }

        public void HandleNewUserEvent(NewUserEventPayload eventPayload)
        {
            if (eventPayload == null)
                throw new ArgumentNullException("eventPayload");

            if (log.IsDebugEnabled)
                log.Debug("Received new user: " + eventPayload);

            Users.Add(eventPayload.NewUser);
        }

        public void HandleNewSerializedRequestEvent(NewSerializedRequestEventPayload eventPayload)
        {
            if (eventPayload == null)
                throw new ArgumentNullException("eventPayload");

            if (log.IsDebugEnabled)
                log.Debug("Received new serialized request: " + eventPayload);

            if(TodaysRequests.All(request => request.Identifier != eventPayload.NewSerializedRequest.Identifier) &&
                eventPayload.NewSerializedRequest.TradeDate == DateTime.Today)
                Requests.Add(eventPayload.NewSerializedRequest);
        }

        public void HandlePublishedNewRequestEvent(NewRequestForQuoteEventPayload eventPayload)
        {
            if (eventPayload == null)
                throw new ArgumentNullException("eventPayload");

            IRequestForQuote request = new RequestForQuoteImpl();
            request.Legs = optionRequestParser.ParseRequest(eventPayload.NewRequestText.ToUpper(), request);
            request.Request = eventPayload.NewRequestText;
            request.Status = StatusEnum.PENDING;
            request.Identifier = -1;
            request.Client = eventPayload.NewRequestClient;
            request.TradeDate = DateTime.Today;
            if(request.Legs !=null)
                request.ExpiryDate = request.Legs[0].MaturityDate;            
            request.LotSize = 100;
            request.Multiplier = 10;
            request.Contracts = 100;
            request.NotionalFXRate = 1;
            request.NotionalMillions = 1;
            request.BookCode = eventPayload.NewRequestBookCode;
            request.DayCountConvention = 250.0;            
            request.PremiumSettlementFXRate = 1;
            request.SalesCreditFXRate = 1;
            request.IsOTC = true;
            request.SalesCreditPercentage = 2;
            request.PremiumSettlementDaysOverride = 1;
            request.PremiumSettlementDate = DateTime.Today.AddDays(request.PremiumSettlementDaysOverride);
            request.CalculatePricing(optionRequestPricer);

            TodaysRequests.Add(request);
            Requests.Clear();
            Requests.AddRange(TodaysRequests);
            NotifyPropertyChanged("Requests"); 

            // TODO - add all fields with their defaults.

            if (log.IsDebugEnabled)
                log.Debug("Received and prcoessed published new request for quote => " + request);
        }

        public void HandleGetTodaysRequestsEvent(EmptyEventPayload emptyPayload)
        {
            if (emptyPayload == null)
                throw new ArgumentNullException("emptyPayload");

            if (log.IsDebugEnabled)
                log.Debug("Received request to reset grid to today's requests.");

            var view = CollectionViewSource.GetDefaultView(Requests);
            view.Filter = null;

            Requests.Clear();
            Requests.AddRange(TodaysRequests);
            NotifyPropertyChanged("Requests");            
        }

        public void HandleBothFilterAndSearchRequests(CriteriaUsageEventPayload eventPayload)
        {
            if (eventPayload == null)
                throw new ArgumentNullException("eventPayload");

            if (eventPayload.IsFilter)
                HandlePublishedFilterRequestsEvent(eventPayload);
            else
                HandlePublishedSearchRequestsEvent(eventPayload);
        }

        public void HandlePublishedFilterRequestsEvent(CriteriaUsageEventPayload eventPayload)
        {
            if (eventPayload == null)
                throw new ArgumentNullException("eventPayload");

            if (log.IsDebugEnabled)
                log.Debug("Received published filter requests event => " + eventPayload);

            if (eventPayload.Criteria != null && eventPayload.Criteria.Count > 0)
            {
                var view = CollectionViewSource.GetDefaultView(Requests);
                view.Filter = (request) => ((IRequestForQuote) request).DoesRequestMatchFilter(eventPayload.Criteria);
            }
            else
            {
                Requests.Clear();
                Requests.AddRange(TodaysRequests);
                NotifyPropertyChanged("Requests");
                var view = CollectionViewSource.GetDefaultView(Requests);
                view.Filter = null;    
            }                
        }

        public void HandlePublishedSearchRequestsEvent(CriteriaUsageEventPayload eventPayload)
        {
            if (eventPayload == null)
                throw new ArgumentNullException("eventPayload");

            if (log.IsDebugEnabled)
                log.Debug("Received published search requests event => " + eventPayload);

            ISearch search = new SearchImpl();

            if (eventPayload.Criteria != null)
                foreach (var criterion in eventPayload.Criteria)                
                    search.Criteria.Add(new SearchCriterionImpl {ControlName = criterion.Key, ControlValue = criterion.Value});

            Requests.Clear();
            Requests.AddRange(optionRequestPersistanceManager.GetRequestMatchingAdhocCriteria(search, false));
            NotifyPropertyChanged("Requests");

            if (!Requests.Any())
                MessageBox.Show("No RFQs matching the search criteria were found!\nPlease broaden your search criteria.",
                                "No Search Results Returned.", MessageBoxButton.OK, MessageBoxImage.Exclamation);
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
            if (String.IsNullOrEmpty(groupBy))
                throw new ArgumentException("groupBy");

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
            Requests.Remove(SelectedRequest);
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
            if (!IsSelectedRequestNull() && !SelectedRequest.CalculatePricing(optionRequestPricer))
            {
                if (isFromContextMenu)
                    MessageBox.Show("Failed to calculate pricing for request: " + SelectedRequest.Request, "Calculation Error", MessageBoxButton.OK, MessageBoxImage.Exclamation);

                log.Error("Failed to calculate pricing for request: " + SelectedRequest.Request);
            }
        }

        public bool CanCalculateRequest()
        {
            return !IsSelectedRequestNull();
        }

        public void ClearNewRequest()
        {
            NewRequest = String.Empty;
        }

        public void ChangeStatusOfRequest(StatusEnum status)
        {
            if (!IsSelectedRequestNull())
                SelectedRequest.Status = status;
        }

        public void PickUpRequest()
        {
            if (!IsSelectedRequestNull())
            {
                SelectedRequest.PickedUpBy = configManager.CurrentUser;
                SelectedRequest.Status = StatusEnum.PICKEDUP;

                if (log.IsDebugEnabled)
                    log.Debug("Request => " + SelectedRequest.Request + " picked up by => " + SelectedRequest.PickedUpBy);
            }
        }

        private void RequestSelected()
        {
            if (!IsSelectedRequestNull())
                eventAggregator.GetEvent<RequestSelectionEvent>().Publish(new RequestSelectionEventPayload() { RequestId = SelectedRequest.Identifier });
        }

        public event PropertyChangedEventHandler PropertyChanged;

        private void NotifyPropertyChanged(string propertyName)
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
