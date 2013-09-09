using System;
using System.Collections;
using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Data;
using System.Windows.Input;
using Microsoft.Practices.Prism.Commands;
using Microsoft.Practices.Prism.Events;
using RequestForQuoteInterfacesLibrary.Constants;
using RequestForQuoteInterfacesLibrary.EventPayloads;
using RequestForQuoteInterfacesLibrary.Events;
using RequestForQuoteInterfacesLibrary.ModelImplementations;
using RequestForQuoteInterfacesLibrary.ModelInterfaces;
using RequestForQuoteInterfacesLibrary.ServiceInterfaces;
using log4net;

namespace RequestForQuoteGridModuleLibrary
{
    internal class SequenceIdComparer : IComparer
    {
        // Faster sort than using ICollectionView.SortDescriptions - sort first by sequnece id and then by time (as a precaution).
        public int Compare(object firstParam, object secondParam)
        {
            int result = 0;
            ChatMessageImpl firstMessage = firstParam as ChatMessageImpl;
            ChatMessageImpl secondMessage = secondParam as ChatMessageImpl;
            if (firstMessage != null && secondMessage != null)
            {
                result = firstMessage.SequenceId.CompareTo(secondMessage.SequenceId);
                if(result == 0)
                    return firstMessage.TimeStamp.CompareTo(secondMessage.TimeStamp);
            }
            return result;
        }
    }

    public class RequestForQuoteDetailsViewModel : DependencyObject 
    {
        private static readonly ILog log = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private readonly IEventAggregator eventAggregator;
        
        private readonly IRequestForQuote originalRequestForQuote;
        private readonly IOptionRequestPricer optionRequestPricer;
        
        private readonly IClientManager clientManager;
        private readonly IBookManager bookManager;
        private readonly IUnderlyingManager underlyingManager;
        private readonly IChatServiceManager chatServiceManager;

        public IClient SelectedSearchClient { get; set; }
        public IRequestForQuote ClonedRequest { get; set; }
        public ICommand SaveRequestCommand { get; private set; }
        public ICommand ClosePopupCommand { get; private set; }
        public ICommand SendChatMessageCommand { get; private set; }

        public ObservableCollection<IClient> Clients { get; set; }
        public ObservableCollection<IBook> Books { get; set; }
        public ObservableCollection<IUnderlyier> Underlyiers { get; set; }
        public ObservableCollection<ChatMessageImpl> ChatMessages { get; set; }

        public string MessageToBeSent
        {
            get { return (string)GetValue(MessageToBeSentProperty); }
            set { SetValue(MessageToBeSentProperty, value); }
        }

        // Using a DependencyProperty as the backing store for MessageToBeSent.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty MessageToBeSentProperty =
            DependencyProperty.Register("MessageToBeSent", typeof(string), typeof(RequestForQuoteDetailsViewModel), new UIPropertyMetadata(""));

        public RequestForQuoteDetailsViewModel(IOptionRequestPricer optionRequestPricer, IRequestForQuote requestForQuote,
                                                IClientManager clientManager, IBookManager bookManager, IEventAggregator eventAggregator,
                                                IUnderlyingManager underlyingManager, IChatServiceManager chatServiceManager)
        {
            this.optionRequestPricer = optionRequestPricer;
            this.clientManager = clientManager;
            this.bookManager = bookManager;
            this.underlyingManager = underlyingManager;
            this.chatServiceManager = chatServiceManager;
            this.eventAggregator = eventAggregator;

            originalRequestForQuote = requestForQuote;
            ClonedRequest = requestForQuote.Clone(requestForQuote.Identifier);

            InitializeCommands();
            InitializeCollections();
            InitializeEventSubscriptions();
        }

        private void InitializeCommands()
        {
            SaveRequestCommand = new DelegateCommand<string>(Save, CanSave);
            ClosePopupCommand = new DelegateCommand<string>(Save, CanSave);
            SendChatMessageCommand = new DelegateCommand(SendChatMessage, (() => true));            
        }

        private void InitializeCollections()
        {
            Clients = new ObservableCollection<IClient>(clientManager.Clients);
            Books = new ObservableCollection<IBook>(bookManager.Books);
            Underlyiers = new ObservableCollection<IUnderlyier>(underlyingManager.Underlyiers);
            ChatMessages = new ObservableCollection<ChatMessageImpl>(chatServiceManager.RegisterParticipant(originalRequestForQuote.Identifier));

            ListCollectionView messagesCollectionView = CollectionViewSource.GetDefaultView(ChatMessages) as ListCollectionView;
            if (messagesCollectionView != null)
                messagesCollectionView.CustomSort = new SequenceIdComparer(); 
        }

        private void InitializeEventSubscriptions()
        {
            eventAggregator.GetEvent<NewBookEvent>()
                           .Subscribe(HandleNewBookEvent, ThreadOption.UIThread, RequestForQuoteConstants.MAINTAIN_STRONG_REFERENCE);

            eventAggregator.GetEvent<NewClientEvent>()
                           .Subscribe(HandleNewClientEvent, ThreadOption.UIThread, RequestForQuoteConstants.MAINTAIN_STRONG_REFERENCE);

            eventAggregator.GetEvent<NewUnderlyierEvent>()
                           .Subscribe(HandleNewUnderlyierEvent, ThreadOption.UIThread, RequestForQuoteConstants.MAINTAIN_STRONG_REFERENCE);

            eventAggregator.GetEvent<NewChatMessageEvent>()
                           .Subscribe(HandleNewChatMessageEvent, ThreadOption.UIThread, RequestForQuoteConstants.MAINTAIN_STRONG_REFERENCE);
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

        public void HandleNewUnderlyierEvent(NewUnderlyierEventPayload eventPayLoad)
        {
            if (log.IsDebugEnabled)
                log.Debug("Received new underlyier: " + eventPayLoad);

            Underlyiers.Add(eventPayLoad.NewUnderlyier);
        }

        public void HandleNewChatMessageEvent(NewChatMessageEventPayload eventPayLoad)
        {
            if (eventPayLoad.NewChatMessage.RequestForQuoteId == originalRequestForQuote.Identifier)
            {
                if (log.IsDebugEnabled)
                    log.Debug("Received new chat message: " + eventPayLoad);
                
                ChatMessages.Add(eventPayLoad.NewChatMessage);
            }
        }

        public void SendChatMessage()
        {
            if (!String.IsNullOrEmpty(MessageToBeSent))
            {
                if (log.IsDebugEnabled)
                    log.Debug(String.Format("Sending chat message to server for storage [{0}]", MessageToBeSent));

                chatServiceManager.SendChatMessage(originalRequestForQuote.Identifier, RequestForQuoteConstants.MY_USER_NAME, MessageToBeSent);
                MessageToBeSent = String.Empty;
            }
        }

        public void CalculateRequest()
        {
            if (!ClonedRequest.CalculatePricing(optionRequestPricer))
            {
                MessageBox.Show("Failed to calculate pricing", "Calculation Error", MessageBoxButton.OK, MessageBoxImage.Exclamation);
                log.Error("Failed to calculate pricing for request: " + ClonedRequest.Request);
            }
        }

        public bool CanCalculateRequest()
        {
            return ClonedRequest != null;
        }

        public void Save(string saveChanges)
        {
            if (saveChanges == "true")
            {
                originalRequestForQuote.Request = ClonedRequest.Request;
                originalRequestForQuote.Client = ClonedRequest.Client;
                originalRequestForQuote.Status = ClonedRequest.Status;
                originalRequestForQuote.BookCode = ClonedRequest.BookCode;
                originalRequestForQuote.TradeDate = ClonedRequest.TradeDate;

                originalRequestForQuote.LotSize = ClonedRequest.LotSize;
                originalRequestForQuote.IsOTC = ClonedRequest.IsOTC;
                originalRequestForQuote.Multiplier = ClonedRequest.Multiplier;
                originalRequestForQuote.Contracts = ClonedRequest.Contracts;
                
                originalRequestForQuote.PremiumAmount = ClonedRequest.PremiumAmount;
                originalRequestForQuote.PremiumPercentage = ClonedRequest.PremiumPercentage;
                originalRequestForQuote.ImpliedVol = ClonedRequest.ImpliedVol;
                
                originalRequestForQuote.Delta = ClonedRequest.Delta;
                originalRequestForQuote.Gamma = ClonedRequest.Gamma;
                originalRequestForQuote.Theta = ClonedRequest.Theta;
                originalRequestForQuote.Rho = ClonedRequest.Rho;
                originalRequestForQuote.Vega = ClonedRequest.Vega;
                                               
                originalRequestForQuote.PremiumSettlementCurrency = ClonedRequest.PremiumSettlementCurrency;
                originalRequestForQuote.PremiumSettlementDate = ClonedRequest.PremiumSettlementDate;
                originalRequestForQuote.PremiumSettlementDaysOverride = ClonedRequest.PremiumSettlementDaysOverride;
                originalRequestForQuote.PremiumSettlementFXRate = ClonedRequest.PremiumSettlementFXRate;
                
                originalRequestForQuote.NotionalCurrency = ClonedRequest.NotionalCurrency;
                originalRequestForQuote.NotionalMillions = ClonedRequest.NotionalMillions;
                originalRequestForQuote.NotionalFXRate = ClonedRequest.NotionalFXRate;               

                originalRequestForQuote.SalesCreditAmount = ClonedRequest.SalesCreditAmount;
                originalRequestForQuote.SalesCreditPercentage = ClonedRequest.SalesCreditPercentage;
                originalRequestForQuote.SalesCreditFXRate = ClonedRequest.SalesCreditFXRate;
                originalRequestForQuote.SalesCreditCurrency = ClonedRequest.SalesCreditCurrency;

                originalRequestForQuote.HedgePrice = ClonedRequest.HedgePrice;
                originalRequestForQuote.HedgeType = ClonedRequest.HedgeType;

                originalRequestForQuote.SalesComment = ClonedRequest.SalesComment;
                originalRequestForQuote.TraderComment = ClonedRequest.TraderComment;
                originalRequestForQuote.ClientComment = ClonedRequest.ClientComment;
                originalRequestForQuote.PickedUpBy = ClonedRequest.PickedUpBy;

                originalRequestForQuote.AskImpliedVol = ClonedRequest.AskImpliedVol;
                originalRequestForQuote.AskPremiumPercentage = ClonedRequest.AskPremiumPercentage;
                originalRequestForQuote.AskPremiumAmount = ClonedRequest.AskPremiumAmount;
                originalRequestForQuote.AskFinalAmount = ClonedRequest.AskFinalAmount;
                originalRequestForQuote.AskFinalPercentage = ClonedRequest.AskFinalPercentage;

                originalRequestForQuote.BidImpliedVol = ClonedRequest.BidImpliedVol;
                originalRequestForQuote.BidPremiumPercentage = ClonedRequest.BidPremiumPercentage;
                originalRequestForQuote.BidPremiumAmount = ClonedRequest.BidPremiumAmount;
                originalRequestForQuote.BidFinalAmount = ClonedRequest.BidFinalAmount;
                originalRequestForQuote.BidFinalPercentage = ClonedRequest.BidFinalPercentage;
                // TODO chat mesages?
            }
            originalRequestForQuote.Popup.CloseWindow();
            originalRequestForQuote.Popup = null;
        }

        public bool CanSave(string saveChanges)
        {
            return ClonedRequest != null;
        }
    }
}
