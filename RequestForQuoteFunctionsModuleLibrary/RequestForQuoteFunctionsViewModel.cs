﻿using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Windows;
using System.Linq;
using System.Windows.Data;
using System.Windows.Input;
using Microsoft.Practices.Prism.Events;
using Microsoft.Practices.ServiceLocation;
using RequestForQuoteFunctionsModuleLibrary.Commands;
using RequestForQuoteInterfacesLibrary.Constants;
using RequestForQuoteInterfacesLibrary.Enums;
using RequestForQuoteInterfacesLibrary.EventPayloads;
using RequestForQuoteInterfacesLibrary.Events;
using RequestForQuoteInterfacesLibrary.ModelInterfaces;
using RequestForQuoteInterfacesLibrary.ServiceInterfaces;
using RequestForQuoteInterfacesLibrary.WindowInterfaces;
using log4net;

namespace RequestForQuoteFunctionsModuleLibrary
{
    public sealed class RequestForQuoteFunctionsViewModel : DependencyObject, INotifyPropertyChanged
    {     
        private static readonly ILog log = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private readonly IEventAggregator eventAggregator;
        private static Dictionary<string, string> criteria = new Dictionary<string, string>();
        private readonly IClientManager clientManager;
        private readonly IUnderlyingManager underlyingManager;
        private readonly IBookManager bookManager;
        private readonly ISearchManager searchManager;
        private readonly IConfigurationManager configManager;
        private readonly IUserManager userManager;
        private readonly IGroupManager groupManager;

        private readonly Object thisLock = new Object();

        public ObservableCollection<IClient> Clients { get; set; }
        public ObservableCollection<IUnderlying> Underlyiers { get; set; }
        public ObservableCollection<IBook> Books { get; set; }
        public ObservableCollection<ISearch> Searches { get; set; }        

        public CollectionViewSource MySavedItems { get; set; }
        public CollectionViewSource PublicSearches { get; set; }
        public CollectionViewSource PrivateSearches { get; set; }
        public CollectionViewSource PublicFilters { get; set; }
        public CollectionViewSource PrivateFilters { get; set; }

        public List<string> Status { get; set; }
        public ObservableCollection<IUser> Users { get; set; }
        public ObservableCollection<IGroup> Groups { get; set; }
        
        internal Dictionary<string, string> Criteria
        {
            get { return criteria;  }
            set { criteria = value; }
        }

        public ICommand SearchRequestsCommand { get; set; }
        public ICommand FilterRequestsCommand { get; set; }
        public ICommand ClearCriteriaCommand { get; set; }
        public ICommand SaveSearchCommand { get; set; }
        public ICommand DeleteSearchCommand { get; set; }
        public ICommand UpdatePrivacyCommand { get; set; }

        public RequestForQuoteFunctionsViewModel(IEventAggregator eventAggregator, IClientManager clientManager,
            IUnderlyingManager underlyingManager, IBookManager bookManager, ISearchManager searchManager, 
            IConfigurationManager configManager, IUserManager userManager, IGroupManager groupManager)
        {
            if (eventAggregator == null)
                throw new ArgumentNullException("eventAggregator");

            if (clientManager == null)
                throw new ArgumentNullException("clientManager");

            if (underlyingManager == null)
                throw new ArgumentNullException("underlyingManager");

            if (bookManager == null)
                throw new ArgumentNullException("bookManager");

            if (searchManager == null)
                throw new ArgumentNullException("searchManager");

            if (configManager == null)
                throw new ArgumentNullException("configManager");

            if (userManager == null)
                throw new ArgumentNullException("userManager");

            if (groupManager == null)
                throw new ArgumentNullException("groupManager");

            SearchRequestsCommand = new SearchRequestsCommand(this);
            FilterRequestsCommand = new FilterRequestsCommand(this);
            ClearCriteriaCommand = new ClearCriteriaCommand(this);
            SaveSearchCommand = new SaveSearchCommand(this);
            DeleteSearchCommand = new DeleteSearchCommand(this);
            UpdatePrivacyCommand = new UpdatePrivacyCommand(this);

            this.clientManager = clientManager;
            this.underlyingManager = underlyingManager;
            this.bookManager = bookManager;
            this.searchManager = searchManager;
            this.eventAggregator = eventAggregator;
            this.configManager = configManager;
            this.userManager = userManager;
            this.groupManager = groupManager;

            InitializeCollections();
            InitializeEventSubscriptions();
        }

        private void InitializeCollections()
        {
            Criteria = new Dictionary<string, string>();

            Status = new List<string>();
            foreach (var status in Enum.GetNames(typeof(StatusEnum)))
                Status.Add(status);
            
            Clients = new ObservableCollection<IClient>(clientManager.Clients);
            Underlyiers = new ObservableCollection<IUnderlying>(underlyingManager.Underlyings);
            Books = new ObservableCollection<IBook>(bookManager.Books);
            Searches = new ObservableCollection<ISearch>(searchManager.Searches);
            Users = new ObservableCollection<IUser>(userManager.Users);
            Groups = new ObservableCollection<IGroup>(groupManager.Groups);

            MySavedItems = new CollectionViewSource {Source = Searches};
            MySavedItems.Filter += delegate(object sender, FilterEventArgs e)
                {
                    var search = e.Item as ISearch;
                    if (search != null)
                        e.Accepted = search.Owner == configManager.CurrentUser;
                };

            PublicSearches = new CollectionViewSource { Source = Searches };
            PublicSearches.Filter += delegate(object sender, FilterEventArgs e)
                {
                    var search = e.Item as ISearch;
                    if (search != null)
                        e.Accepted = !search.IsFilter && !search.IsPrivate;
                };

            PrivateSearches = new CollectionViewSource { Source = Searches };
            PrivateSearches.Filter += delegate(object sender, FilterEventArgs e)
                {
                    var search = e.Item as ISearch;
                    if (search != null)
                        e.Accepted = !search.IsFilter && search.IsPrivate;
                };

            PublicFilters = new CollectionViewSource { Source = Searches };
            PublicFilters.Filter += delegate(object sender, FilterEventArgs e)
                {
                    var search = e.Item as ISearch;
                    if (search != null)
                        e.Accepted = search.IsFilter && !search.IsPrivate;
                };

            PrivateFilters = new CollectionViewSource { Source = Searches };
            PrivateFilters.Filter += delegate(object sender, FilterEventArgs e)
                {
                    var search = e.Item as ISearch;
                    if (search != null) 
                        e.Accepted = search.IsFilter && search.IsPrivate;
                };
        }

        private void InitializeEventSubscriptions()
        {
            eventAggregator.GetEvent<NewBookEvent>()
                           .Subscribe(HandleNewBookEvent, ThreadOption.UIThread, RequestForQuoteConstants.MAINTAIN_STRONG_REFERENCE);

            eventAggregator.GetEvent<NewClientEvent>()
                           .Subscribe(HandleNewClientEvent, ThreadOption.UIThread, RequestForQuoteConstants.MAINTAIN_STRONG_REFERENCE);

            eventAggregator.GetEvent<NewUnderlyierEvent>()
                           .Subscribe(HandleNewUnderlyierEvent, ThreadOption.UIThread, RequestForQuoteConstants.MAINTAIN_STRONG_REFERENCE);

            eventAggregator.GetEvent<NewSearchEvent>()
                           .Subscribe(HandleNewSearchEvent, ThreadOption.UIThread, RequestForQuoteConstants.MAINTAIN_STRONG_REFERENCE);

            eventAggregator.GetEvent<NewUserEvent>()
                            .Subscribe(HandleNewUserEvent, ThreadOption.UIThread, RequestForQuoteConstants.MAINTAIN_STRONG_REFERENCE);

            eventAggregator.GetEvent<NewGroupEvent>()
                            .Subscribe(HandleNewGroupEvent, ThreadOption.UIThread, RequestForQuoteConstants.MAINTAIN_STRONG_REFERENCE);
        }

        public void HandleNewUserEvent(NewUserEventPayload eventPayLoad)
        {
            if (eventPayLoad == null)
                throw new ArgumentNullException("eventPayLoad");

            if (log.IsDebugEnabled)
                log.Debug("Received new user event from UserManager: " + eventPayLoad);

            Users.Add(eventPayLoad.NewUser);
        }

        public void HandleNewGroupEvent(NewGroupEventPayload eventPayLoad)
        {
            if (eventPayLoad == null)
                throw new ArgumentNullException("eventPayLoad");

            if (log.IsDebugEnabled)
                log.Debug("Received new group event from GroupManager: " + eventPayLoad);

            Groups.Add(eventPayLoad.NewGroup);
        }

        public void HandleNewBookEvent(NewBookEventPayload eventPayLoad)
        {
            if (eventPayLoad == null)
                throw new ArgumentNullException("eventPayLoad");

            if (log.IsDebugEnabled)
                log.Debug("Received new book: " + eventPayLoad);

            Books.Add(eventPayLoad.NewBook);
        }

        public void HandleNewClientEvent(NewClientEventPayload eventPayLoad)
        {
            if (eventPayLoad == null)
                throw new ArgumentNullException("eventPayLoad");

            if (log.IsDebugEnabled)
                log.Debug("Received new client: " + eventPayLoad);

            Clients.Add(eventPayLoad.NewClient);
        }

        public void HandleNewSearchEvent(NewSearchEventPayload eventPayLoad)
        {
            if (eventPayLoad == null)
                throw new ArgumentNullException("eventPayLoad");

            if (log.IsDebugEnabled)
                log.Debug("Received new search: " + eventPayLoad);

            AddToExistingSearchesOrCreateNew(eventPayLoad.NewSearch);
        }

        private void AddToExistingSearchesOrCreateNew(ISearch searchToBeAdded)
        {
            if (searchToBeAdded == null)
                throw new ArgumentNullException("searchToBeAdded");

            lock (thisLock)
            {
                var matchingSearch = Searches.FirstOrDefault((existingSearch) => existingSearch.Owner == searchToBeAdded.Owner
                    && existingSearch.DescriptionKey == searchToBeAdded.DescriptionKey);

                if (matchingSearch != null)
                    matchingSearch.Criteria.AddRange(searchToBeAdded.Criteria);
                else
                    Searches.Add(searchToBeAdded);                
            }
        }

        public void HandleNewUnderlyierEvent(NewUnderlyierEventPayload eventPayLoad)
        {
            if (eventPayLoad == null)
                throw new ArgumentNullException("eventPayLoad");

            if (log.IsDebugEnabled)
                log.Debug("Received new underlyier: " + eventPayLoad);

            Underlyiers.Add(eventPayLoad.NewUnderlying);
        }

        public ISearch SelectedSearch
        {
            get { return (ISearch)GetValue(SelectedSearchProperty); }
            set { SetValue(SelectedSearchProperty, value); }
        }

        // Using a DependencyProperty as the backing store for SelectedSearch.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty SelectedSearchProperty =
            DependencyProperty.Register("SelectedSearch", typeof(ISearch), typeof(RequestForQuoteFunctionsViewModel), new UIPropertyMetadata(null));

        public string CriteriaDescriptionKey { get; set; }

        public PrivacyEnum PrivacyOfCriteria
        {
            get { return (PrivacyEnum)GetValue(PrivacyOfCriteriaProperty); }
            set { SetValue(PrivacyOfCriteriaProperty, value); }
        }

        // Using a DependencyProperty as the backing store for PrivacyOfCriteria.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty PrivacyOfCriteriaProperty =
            DependencyProperty.Register("PrivacyOfCriteria", typeof(PrivacyEnum), typeof(RequestForQuoteFunctionsViewModel), new UIPropertyMetadata(PrivacyEnum.PRIVATE));

        public CriteriaTypeEnum TypeOfCriteria
        {
            get { return (CriteriaTypeEnum)GetValue(TypeOfCriteriaProperty); }
            set { SetValue(TypeOfCriteriaProperty, value); }
        }

        // Using a DependencyProperty as the backing store for TypeOfCriteria.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty TypeOfCriteriaProperty =
            DependencyProperty.Register("TypeOfCriteria", typeof(CriteriaTypeEnum), typeof(RequestForQuoteFunctionsViewModel), new UIPropertyMetadata(CriteriaTypeEnum.FILTER));

        public IUser SelectedInitiator
        {
            get { return (IUser)GetValue(SelectedInitiatorProperty); }
            set { SetValue(SelectedInitiatorProperty, value); }
        }

        // Using a DependencyProperty as the backing store for SelectedInitiator.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty SelectedInitiatorProperty =
            DependencyProperty.Register("SelectedInitiator", typeof(IUser), typeof(RequestForQuoteFunctionsViewModel), new UIPropertyMetadata(null, InitiatorPropertyChangedCallback));


        private static void InitiatorPropertyChangedCallback(DependencyObject dependencyObject, DependencyPropertyChangedEventArgs dependencyPropertyChangedEventArgs)
        {
            var user = dependencyPropertyChangedEventArgs.NewValue as IUser;
            if (user != null)
                criteria[RequestForQuoteConstants.INITIATOR_CRITERION] = user.UserId;
        }

        public IUnderlying SelectedUnderlying
        {
            get { return (IUnderlying)GetValue(SelectedUnderlyierProperty); }
            set { SetValue(SelectedUnderlyierProperty, value); }
        }

        // Using a DependencyProperty as the backing store for SelectedUnderlyier.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty SelectedUnderlyierProperty =
            DependencyProperty.Register("SelectedUnderlyier", typeof(IUnderlying), typeof(RequestForQuoteFunctionsViewModel), new UIPropertyMetadata(null, UnderlyierPropertyChangedCallback));

        private static void UnderlyierPropertyChangedCallback(DependencyObject dependencyObject, DependencyPropertyChangedEventArgs dependencyPropertyChangedEventArgs)
        {
            var underlyier = dependencyPropertyChangedEventArgs.NewValue as IUnderlying;
            if (underlyier != null)
                criteria[RequestForQuoteConstants.UNDERLYIER_CRITERION] = underlyier.RIC; 
        }

        public IClient SelectedClient
        {
            get { return (IClient)GetValue(SelectedClientProperty); }
            set { SetValue(SelectedClientProperty, value); }
        }

        // Using a DependencyProperty as the backing store for SelectedClient.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty SelectedClientProperty =
            DependencyProperty.Register("SelectedClient", typeof(IClient), typeof(RequestForQuoteFunctionsViewModel), new UIPropertyMetadata(null, ClientPropertyChangedCallback));

        private static void ClientPropertyChangedCallback(DependencyObject dependencyObject, DependencyPropertyChangedEventArgs dependencyPropertyChangedEventArgs)
        {
            var client = dependencyPropertyChangedEventArgs.NewValue as IClient;
            if(client != null)
                criteria[RequestForQuoteConstants.CLIENT_CRITERION] = client.Identifier.ToString();        
        }

        public IBook SelectedBook
        {
            get { return (IBook)GetValue(SelectedBookProperty); }
            set { SetValue(SelectedBookProperty, value); }
        }

        // Using a DependencyProperty as the backing store for SelectedBook.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty SelectedBookProperty =
            DependencyProperty.Register("SelectedBook", typeof(IBook), typeof(RequestForQuoteFunctionsViewModel), new UIPropertyMetadata(null, BookPropertyChangedCallback));

        private static void BookPropertyChangedCallback(DependencyObject dependencyObject, DependencyPropertyChangedEventArgs dependencyPropertyChangedEventArgs)
        {
            var book = dependencyPropertyChangedEventArgs.NewValue as IBook;

            if (book != null)
                criteria[RequestForQuoteConstants.BOOK_CRITERION] = book.BookCode;
        }
       
        public string SelectedStatus
        {
            get { return (string)GetValue(SelectedStatusProperty); }
            set { SetValue(SelectedStatusProperty, value); }
        }

        // Using a DependencyProperty as the backing store for SelectedStatus.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty SelectedStatusProperty =
            DependencyProperty.Register("SelectedStatus", typeof(string), typeof(RequestForQuoteFunctionsViewModel), new UIPropertyMetadata(null, StatusPropertyChangedCallback));

        private static void StatusPropertyChangedCallback(DependencyObject dependencyObject, DependencyPropertyChangedEventArgs dependencyPropertyChangedEventArgs)
        {
            var status = dependencyPropertyChangedEventArgs.NewValue as string;

            if (status != null)
                criteria[RequestForQuoteConstants.STATUS_CRITERION] = status; 
        }

        private static void AddFirstDateToCriteria(string criteriaKey, string firstDate)
        {
            if (String.IsNullOrEmpty(criteriaKey))
                throw new ArgumentException("criteriaKey");

            if (String.IsNullOrEmpty(firstDate))
                throw new ArgumentException("firstDate");

            // if the first date does not exist or the first date exists and has a hyphen suffix as the last char then write.
            if (!criteria.ContainsKey(criteriaKey) || (criteria[criteriaKey].IndexOf('-') == criteria[criteriaKey].Length - 1))
                criteria[criteriaKey] = firstDate + "-";
            // else if the first date does not exist and the first char is a hyphen then add the first date as a prefix to what already exists.
            else if (criteria[criteriaKey].IndexOf('-') == 0)
                criteria[criteriaKey] = firstDate + criteria[criteriaKey];
            // else the first date exists so replace it.
            else
                criteria[criteriaKey] = firstDate + criteria[criteriaKey].Substring(criteria[criteriaKey].IndexOf('-'));
        }

        private static void AddSecondDateToCriteria(string criteriaKey, string secondDate)
        {
            if (String.IsNullOrEmpty(criteriaKey))
                throw new ArgumentException("criteriaKey");

            if (String.IsNullOrEmpty(secondDate))
                throw new ArgumentException("secondDate");

            // if the second date does not exist or the second date exists and a hyphen prefix then write
            if (!criteria.ContainsKey(criteriaKey) || (criteria[criteriaKey].IndexOf('-') == 0))
                criteria[criteriaKey] = "-" + secondDate;
            // else if first date exists and second date does not then append the second date
            else if (criteria[criteriaKey].IndexOf('-') == criteria[criteriaKey].Length - 1)
                criteria[criteriaKey] += secondDate;
            // else the first overwrite the second date
            else
                criteria[criteriaKey] = criteria[criteriaKey].Substring(0, criteria[criteriaKey].IndexOf('-') + 1) + secondDate;
        }

        public DateTime? StartTradeDate
        {
            get { return (DateTime?)GetValue(StartTradeDateProperty); }
            set { SetValue(StartTradeDateProperty, value); }
        }

        // Using a DependencyProperty as the backing store for StartTradeDate.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty StartTradeDateProperty =
            DependencyProperty.Register("StartTradeDate", typeof(DateTime?), typeof(RequestForQuoteFunctionsViewModel), new UIPropertyMetadata(null, StartTradeDatePropertyChangedCallback));

        private static void StartTradeDatePropertyChangedCallback(DependencyObject dependencyObject, DependencyPropertyChangedEventArgs eventArgs)
        {
            if (eventArgs.NewValue != null)
                AddFirstDateToCriteria(RequestForQuoteConstants.TRADE_DATE_CRITERION, eventArgs.NewValue.ToString());
        }

        public DateTime? EndTradeDate
        {
            get { return (DateTime?)GetValue(EndTradeDateProperty); }
            set { SetValue(EndTradeDateProperty, value); }
        }

        // Using a DependencyProperty as the backing store for EndTradeDate.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty EndTradeDateProperty =
            DependencyProperty.Register("EndTradeDate", typeof(DateTime?), typeof(RequestForQuoteFunctionsViewModel), new UIPropertyMetadata(null, EndTradeDatePropertyChangedCallback));

        private static void EndTradeDatePropertyChangedCallback(DependencyObject dependencyObject, DependencyPropertyChangedEventArgs eventArgs)
        {
            if (eventArgs.NewValue != null)
                AddSecondDateToCriteria(RequestForQuoteConstants.TRADE_DATE_CRITERION, eventArgs.NewValue.ToString());
        }

        public bool CanSearchRequests(bool isExistingCriteria)
        {
            return criteria.Count > 0  || (isExistingCriteria && SelectedSearch != null);
        }

        public bool CanFilterRequests()
        {
            return criteria.Count > 0;
        }

        public bool CanSaveSearch()
        {
            return criteria.Count > 0;
        }

        public bool CanDeleteSearch()
        {
            return (SelectedSearch != null && !string.IsNullOrEmpty(SelectedSearch.Owner) &&
                    !string.IsNullOrEmpty(SelectedSearch.DescriptionKey));
        }

        public bool CanClearCriteria()
        {
            return criteria.Count > 0;
        }

        public bool CanUpdatePrivacy(bool isRequestToMakePrivate)
        {
            if (SelectedSearch == null)
                return false;

            if (string.IsNullOrEmpty(SelectedSearch.Owner) || string.IsNullOrEmpty(SelectedSearch.DescriptionKey))
                return false;

            return (SelectedSearch.IsPrivate != isRequestToMakePrivate);
        }

        public void UpdatePrivacy()
        {
            if (searchManager.UpdatePrivacy(SelectedSearch.Owner, SelectedSearch.DescriptionKey, !SelectedSearch.IsPrivate))
                SelectedSearch.IsPrivate = !SelectedSearch.IsPrivate;
            else
                MessageBox.Show("Failed to update privacy of search " + SelectedSearch.DescriptionKey,
                                "Search Management Error", MessageBoxButton.OK, MessageBoxImage.Error);

        }

        public void DeleteSearch()
        {
            if (searchManager.DeleteSearch(SelectedSearch.Owner, SelectedSearch.DescriptionKey))
            {
                for (var index = 0; index < Searches.Count; index++)
                {
                    var search = Searches[index];
                    if (search.Owner == SelectedSearch.Owner && search.DescriptionKey == SelectedSearch.DescriptionKey)
                    {
                        Searches.RemoveAt(index);
                        break;
                    }
                }
            }
            else
                MessageBox.Show("Failed to delete search " + SelectedSearch.DescriptionKey, 
                                "Search Management Error", MessageBoxButton.OK, MessageBoxImage.Error);
        }

        public void LaunchSaveSearchWindow()
        {
            if (criteria.Count > 0)
            {
                CriteriaDescriptionKey = "";
                PrivacyOfCriteria = PrivacyEnum.PRIVATE;
                TypeOfCriteria = CriteriaTypeEnum.FILTER;

                // Cannot use constructor injection because ISaveSearchPopupWindow is not registered within the unity container the constructor is called.
                IWindowPopup searchPopupWindow = ServiceLocator.Current.GetInstance<IWindowPopup>(WindowPopupNames.SAVE_SEARCH_WINDOW_POPUP);
                searchPopupWindow.ShowModalWindow(this);
                // TODO verify search owner+key does not alreday exist in universe.
            }                
        }

        public void SearchRequests(bool isExistingCriteria)
        {
            InitializeAndPublishCriteria(isExistingCriteria, false);
        }

        public void FilterRequests(bool isExistingCriteria)
        {
            InitializeAndPublishCriteria(isExistingCriteria, true);
        }

        private void InitializeControlsWithCriteria(IEnumerable<ISearchCriterion> controlCriteria)
        {
            if (controlCriteria == null)
                return;
            
            foreach (var controlCriterion in controlCriteria)
            {
                switch (controlCriterion.ControlName)
                {
                    case RequestForQuoteConstants.CLIENT_CRITERION:
                        int clientIdentifier;
                        if(int.TryParse(controlCriterion.ControlValue, out clientIdentifier))
                            SelectedClient = Clients.FirstOrDefault((client) => client.Identifier == clientIdentifier);
                        break;
                    case RequestForQuoteConstants.TRADE_DATE_CRITERION:
                        var dates = controlCriterion.ControlValue.Split('-').ToArray();
                        StartTradeDate = Convert.ToDateTime(dates[0]);
                        EndTradeDate = String.IsNullOrEmpty(dates[1]) ? DateTime.Now : Convert.ToDateTime(dates[1]);
                        break;
                    case RequestForQuoteConstants.BOOK_CRITERION:
                        SelectedBook = Books.FirstOrDefault((book) => book.BookCode == controlCriterion.ControlValue);
                        break;
                    case RequestForQuoteConstants.STATUS_CRITERION:
                        SelectedStatus = Status.FirstOrDefault((status) => status == controlCriterion.ControlValue);
                        break;
                    case RequestForQuoteConstants.UNDERLYIER_CRITERION:
                        SelectedUnderlying = Underlyiers.FirstOrDefault((underlyier) => underlyier.RIC == controlCriterion.ControlValue);
                        break;
                    case RequestForQuoteConstants.INITIATOR_CRITERION:
                        SelectedInitiator = Users.FirstOrDefault((user) => user.UserId == controlCriterion.ControlValue);
                        break;
                }
            }                
        }

        private void InitializeAndPublishCriteria(bool isExistingCriteria, bool isFilter)    
        {
            if (isExistingCriteria && SelectedSearch != null)
            {
                ClearCriteria();
                InitializeControlsWithCriteria(SelectedSearch.Criteria);
                isFilter = SelectedSearch.IsFilter;
            }

            if (Criteria.Count > 0)
                eventAggregator.GetEvent<SearchRequestForQuoteEvent>().Publish(new CriteriaUsageEventPayload()
                    {
                        Criteria = criteria,
                        IsFilter = isFilter
                    });            
        }

        public void ClearCriteria()
        {
            criteria.Clear();

            SelectedClient = null;
            SelectedUnderlying = null;
            SelectedStatus = null;
            StartTradeDate = null;
            SelectedBook = null;
            EndTradeDate = null;
            SelectedInitiator = null;
            CriteriaDescriptionKey = "";

            eventAggregator.GetEvent<SearchRequestForQuoteEvent>().Publish(new CriteriaUsageEventPayload()
            {
                Criteria = {},
                IsFilter = true
            });
        }

        public void SaveSearch()
        {
            if (criteria.Count > 0 && !string.IsNullOrEmpty(CriteriaDescriptionKey))
            {
                foreach (var criterion in criteria)
                {
                    if (!searchManager.SaveSearchToDatabase(configManager.CurrentUser, CriteriaDescriptionKey,
                        PrivacyOfCriteria == PrivacyEnum.PRIVATE, TypeOfCriteria == CriteriaTypeEnum.FILTER, criterion.Key, criterion.Value))
                    {
                        MessageBox.Show("Failed to save search " + CriteriaDescriptionKey, "Search Management Error",
                                        MessageBoxButton.OK, MessageBoxImage.Error);   
                        break;
                    }
                }
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
