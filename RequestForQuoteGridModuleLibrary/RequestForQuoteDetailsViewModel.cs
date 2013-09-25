using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
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
    internal sealed class SequenceIdComparer : IComparer
    {
        // Faster sort than using ICollectionView.SortDescriptions - sort first by sequnece id and then by time (as a precaution).
        public int Compare(object firstParam, object secondParam)
        {
            var result = 0;
            var firstMessage = firstParam as ChatMessageImpl;
            var secondMessage = secondParam as ChatMessageImpl;
            if (firstMessage != null && secondMessage != null)
            {
                result = firstMessage.SequenceId.CompareTo(secondMessage.SequenceId);
                if (result == 0)
                    return firstMessage.TimeStamp.CompareTo(secondMessage.TimeStamp);
            }
            return result;
        }
    }

    public sealed class RequestForQuoteDetailsViewModel : DependencyObject, IEditableObject
    {
        private static readonly ILog log = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private readonly IEventAggregator eventAggregator;
        
        private readonly IOptionRequestPricer optionRequestPricer;
        public IRequestForQuote SelectedRequestForQuote { get; set; }
        private IRequestForQuote backupOfRequestForQuote;
        
        private readonly IClientManager clientManager;
        private readonly IBookManager bookManager;
        private readonly IUnderlyingManager underlyingManager;
        private readonly IChatServiceManager chatServiceManager;

        public IClient SelectedSearchClient { get; set; }
        public ICommand SaveRequestCommand { get; private set; }
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
            DependencyProperty.Register("MessageToBeSent", typeof(string), typeof(RequestForQuoteDetailsViewModel), new UIPropertyMetadata(String.Empty));

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

            SelectedRequestForQuote = requestForQuote;

            InitializeCommands();
            InitializeCollections();
            InitializeEventSubscriptions();
        }

        private void InitializeCommands()
        {
            SaveRequestCommand = new DelegateCommand<string>(Save, CanSave);
            SendChatMessageCommand = new DelegateCommand(SendChatMessage, () => true);            
        }

        private void InitializeCollections()
        {
            Clients = new ObservableCollection<IClient>(clientManager.Clients);
            Books = new ObservableCollection<IBook>(bookManager.Books);
            Underlyiers = new ObservableCollection<IUnderlyier>(underlyingManager.Underlyiers);
            ChatMessages = new ObservableCollection<ChatMessageImpl>(chatServiceManager.RegisterParticipant(SelectedRequestForQuote.Identifier));

            var messagesCollectionView = CollectionViewSource.GetDefaultView(ChatMessages) as ListCollectionView;
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
            if (eventPayLoad.NewChatMessage.RequestForQuoteId == SelectedRequestForQuote.Identifier)
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

                chatServiceManager.SendChatMessage(SelectedRequestForQuote.Identifier, RequestForQuoteConstants.MY_USER_NAME, MessageToBeSent);
                MessageToBeSent = String.Empty;
            }
        }

        public void CalculateRequest()
        {
            if (!SelectedRequestForQuote.CalculatePricing(optionRequestPricer))
            {
                MessageBox.Show("Failed to calculate pricing", "Calculation Error", MessageBoxButton.OK, MessageBoxImage.Exclamation);
                log.Error("Failed to calculate pricing for request: " + SelectedRequestForQuote.Request);
            }
        }

        public bool CanCalculateRequest()
        {
            return SelectedRequestForQuote != null;
        }

        public void Save(string saveChanges)
        {
            if (saveChanges == "true")
                EndEdit();
            else
                CancelEdit();

            SelectedRequestForQuote.Popup.HideWindow();
        }

        public bool CanSave(string saveChanges)
        {
            return SelectedRequestForQuote != null;
        }

        public void BeginEdit()
        {
            if (log.IsDebugEnabled)
                log.Debug("Beginning edit, creating from backup of currently selected request.");

            backupOfRequestForQuote = SelectedRequestForQuote.Clone(SelectedRequestForQuote.Identifier);
        }

        public void EndEdit()
        {
            if (log.IsDebugEnabled)
                log.Debug("Ending edit, saving currently selected request as is.");

            // Save to the database
        }

        public void CancelEdit()
        {
            if(log.IsDebugEnabled)
                log.Debug("User cancelled edit, restoring from backup");

            SelectedRequestForQuote.CopyMembers(backupOfRequestForQuote);
        }
    }
}
