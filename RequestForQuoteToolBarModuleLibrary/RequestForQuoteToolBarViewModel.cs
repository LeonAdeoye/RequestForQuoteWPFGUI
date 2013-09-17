using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Input;
using Microsoft.Practices.Prism.Events;
using Microsoft.Practices.ServiceLocation;
using RequestForQuoteInterfacesLibrary.Constants;
using RequestForQuoteInterfacesLibrary.EventPayloads;
using RequestForQuoteInterfacesLibrary.Events;
using RequestForQuoteInterfacesLibrary.ModelInterfaces;
using RequestForQuoteInterfacesLibrary.ServiceInterfaces;
using RequestForQuoteToolBarModuleLibrary.Commands;
using log4net;

namespace RequestForQuoteToolBarModuleLibrary
{
    public sealed class RequestForQuoteToolBarViewModel : DependencyObject 
    {
        private static readonly ILog log = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private readonly IEventAggregator eventAggregator = ServiceLocator.Current.GetInstance<IEventAggregator>();

        private readonly IOptionRequestParser optionRequestParser;        
        private readonly IClientManager clientManager;
        private readonly IBookManager bookManager;

        public ObservableCollection<IClient> Clients { get; set; }
        public ObservableCollection<IBook> Books { get; set; }

        public IClient NewRequestClient { get; set; }
        public IBook NewRequestBook { get; set; }
        public IRequestForQuote SelectedRequest { get; set; }

        public ICommand AddNewRequestCommand { get; set; }
        public ICommand ClearNewRequestCommand { get; set; }
        public ICommand GetTodaysRequestsCommand { get; set; }

        public RequestForQuoteToolBarViewModel(IOptionRequestParser optionRequestParser, IClientManager clientManager, IBookManager bookManager)
        {
            AddNewRequestCommand = new AddRequestCommand(this);
            ClearNewRequestCommand = new ClearNewRequestCommand(this);
            GetTodaysRequestsCommand = new GetTodaysRequestsCommand(this);

            this.optionRequestParser = optionRequestParser;
            this.clientManager = clientManager;
            this.bookManager = bookManager;

            InitializeCollections();
            InitializeEventSubscriptions();
        }

        private void InitializeCollections()
        {
            Clients = new ObservableCollection<IClient>(clientManager.Clients);
            Books = new ObservableCollection<IBook>(bookManager.Books);
        }

        private void InitializeEventSubscriptions()
        {
            eventAggregator.GetEvent<NewBookEvent>()
                           .Subscribe(HandleNewBookEvent, ThreadOption.UIThread, RequestForQuoteConstants.MAINTAIN_STRONG_REFERENCE);

            eventAggregator.GetEvent<NewClientEvent>()
                           .Subscribe(HandleNewClientEvent, ThreadOption.UIThread, RequestForQuoteConstants.MAINTAIN_STRONG_REFERENCE);
        }

        public void HandleNewBookEvent(NewBookEventPayload eventPayLoad)
        {
            if (log.IsDebugEnabled)
                log.Debug("Received new book: " + eventPayLoad);

            Books.Add(eventPayLoad.NewBook);
        }

        public void HandleNewClientEvent(NewClientEventPayload eventPayLoad)
        {
            if (log.IsDebugEnabled)
                log.Debug("Received new client: " + eventPayLoad);

            Clients.Add(eventPayLoad.NewClient);
        }
      
        public bool CanAddNewRequest()
        {
            //TODO
            bool canDo = false;
            if (!string.IsNullOrEmpty(NewRequest) && optionRequestParser.IsValidOptionRequest(NewRequest))
            {
                IClient newRequestClient = this.NewRequestClient as IClient;
                if (newRequestClient != null)
                    canDo = !string.IsNullOrEmpty(newRequestClient.Name);
            }
            return canDo;             
        }

        public bool CanGetTodaysRequests()
        {
            return true;
        }

        public void GetTodaysRequests()
        {
            eventAggregator.GetEvent<GetTodaysRequestsEvent>().Publish(new EmptyEventPayload());       
        }

        public bool CanClearNewRequest()
        {
            bool canClear = false;
            if (string.IsNullOrEmpty(this.NewRequest))
            {
                IClient newRequestClient = this.NewRequestClient;
                if (newRequestClient != null)
                    canClear = !string.IsNullOrEmpty(newRequestClient.Name);
            }
            else
                canClear = true;

            return canClear;
        }

        public bool IsSelectRequestNull()
        {
            return (this.SelectedRequest == null);
        }

        public void AddRequest()
        {
            if (optionRequestParser.IsValidOptionRequest(NewRequest))
            {
                eventAggregator.GetEvent<NewRequestForQuoteEvent>().Publish(new NewRequestForQuoteEventPayload()
                    {
                        NewRequestText = this.NewRequest,
                        NewRequestClient = this.NewRequestClient.Name
                    });
    
                if(log.IsDebugEnabled)
                    log.Debug("Published new request for quote => " + this.NewRequest);
            }
        }

        public string NewRequest
        {
            get { return (string)GetValue(NewRequestProperty); }
            set { SetValue(NewRequestProperty, value); }
        }

        public static readonly DependencyProperty NewRequestProperty =
            DependencyProperty.Register("NewRequest", typeof(string), typeof(RequestForQuoteToolBarViewModel), new UIPropertyMetadata(""));

        public void ClearNewRequest()
        {
            this.NewRequest = "";
        }
    }
}
