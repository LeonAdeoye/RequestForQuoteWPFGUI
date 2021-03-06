﻿using System;
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
        private readonly IOptionRequestPersistanceManager optionRequestPersistanceManager;

        public IRequestForQuote SelectedRequestForQuote { get; set; }
        internal IRequestForQuote backupOfRequestForQuote;
        
        private readonly IClientManager clientManager;
        private readonly IBookManager bookManager;
        private readonly IUserManager userManager;
        private readonly IUnderlyingManager underlyingManager;
        private readonly IChatServiceManager chatServiceManager;
        private readonly IConfigurationManager configManager;

        public IClient SelectedSearchClient { get; set; }
        public ICommand SaveRequestCommand { get; private set; }
        public ICommand SendChatMessageCommand { get; private set; }

        public ObservableCollection<IClient> Clients { get; set; }
        public ObservableCollection<IBook> Books { get; set; }
        public ObservableCollection<IUser> Users { get; set; }
        public ObservableCollection<IUnderlying> Underlyiers { get; set; }
        public ObservableCollection<ChatMessageImpl> ChatMessages { get; set; }
        public List<double> DayCountConventions { get; set; }

        public RequestForQuoteDetailsViewModel(IOptionRequestPricer optionRequestPricer, IRequestForQuote requestForQuote,
                                                IClientManager clientManager, IBookManager bookManager, IEventAggregator eventAggregator,
                                                IUnderlyingManager underlyingManager, IChatServiceManager chatServiceManager,
                                                IOptionRequestPersistanceManager optionRequestPersistanceManager, IConfigurationManager configManager,
                                                IUserManager userManager)
        {
            if (optionRequestPricer == null)
                throw new ArgumentNullException("optionRequestPricer");

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

            if (requestForQuote == null)
                throw new ArgumentNullException("requestForQuote");

            this.optionRequestPricer = optionRequestPricer;
            this.clientManager = clientManager;
            this.bookManager = bookManager;
            this.userManager = userManager;
            this.underlyingManager = underlyingManager;
            this.chatServiceManager = chatServiceManager;
            this.eventAggregator = eventAggregator;
            this.optionRequestPersistanceManager = optionRequestPersistanceManager;
            this.configManager = configManager;

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
            DayCountConventions = new List<double>() { 250.0, 255.0, 365.0, 366.0};
            Clients = new ObservableCollection<IClient>(clientManager.Clients);
            Books = new ObservableCollection<IBook>(bookManager.Books);
            Users = new ObservableCollection<IUser>(userManager.Users);
            Underlyiers = new ObservableCollection<IUnderlying>(underlyingManager.Underlyings);

            if(SelectedRequestForQuote.Identifier == -1)
                ChatMessages = new ObservableCollection<ChatMessageImpl>();
            else
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

            eventAggregator.GetEvent<NewUserEvent>()
                           .Subscribe(HandleNewUserEvent, ThreadOption.UIThread, RequestForQuoteConstants.MAINTAIN_STRONG_REFERENCE);
        }

        public string MessageToBeSent
        {
            get { return (string)GetValue(MessageToBeSentProperty); }
            set { SetValue(MessageToBeSentProperty, value); }
        }

        // Using a DependencyProperty as the backing store for MessageToBeSent.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty MessageToBeSentProperty =
            DependencyProperty.Register("MessageToBeSent", typeof(string), typeof(RequestForQuoteDetailsViewModel), new UIPropertyMetadata(String.Empty));

        public IUser SelectedInitiator
        {
            get { return (IUser)GetValue(SelectedInitiatorProperty); }
            set { SetValue(SelectedInitiatorProperty, value); }
        }

        // Using a DependencyProperty as the backing store for SelectedInitiator.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty SelectedInitiatorProperty =
            DependencyProperty.Register("SelectedInitiator", typeof(IUser), typeof(RequestForQuoteDetailsViewModel), new UIPropertyMetadata(null));


        public IUser SelectedTarget
        {
            get { return (IUser)GetValue(SelectedTargetProperty); }
            set { SetValue(SelectedTargetProperty, value); }
        }

        // Using a DependencyProperty as the backing store for SelectedTarget.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty SelectedTargetProperty =
            DependencyProperty.Register("SelectedTarget", typeof(IUser), typeof(RequestForQuoteDetailsViewModel), new UIPropertyMetadata(null));
        
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

        public void HandleNewClientEvent(NewClientEventPayload eventPayload)
        {
            if (eventPayload == null)
                throw new ArgumentNullException("eventPayload");

            if (log.IsDebugEnabled)
                log.Debug("Received new client: " + eventPayload);

            Clients.Add(eventPayload.NewClient);
        }

        public void HandleNewUnderlyierEvent(NewUnderlyierEventPayload eventPayload)
        {
            if (eventPayload == null)
                throw new ArgumentNullException("eventPayload");

            if (log.IsDebugEnabled)
                log.Debug("Received new underlyier: " + eventPayload);

            Underlyiers.Add(eventPayload.NewUnderlying);
        }

        public void HandleNewChatMessageEvent(NewChatMessageEventPayload eventPayload)
        {
            if (eventPayload == null)
                throw new ArgumentNullException("eventPayload");

            if (eventPayload.NewChatMessage.RequestForQuoteId == SelectedRequestForQuote.Identifier)
            {
                if (log.IsDebugEnabled)
                    log.Debug("Received new chat message: " + eventPayload);
                
                ChatMessages.Add(eventPayload.NewChatMessage);
            }
        }

        public void SendChatMessage()
        {
            if (!String.IsNullOrEmpty(MessageToBeSent))
            {
                if (log.IsDebugEnabled)
                    log.Debug(String.Format("Sending chat message to server for storage [{0}]", MessageToBeSent));

                chatServiceManager.SendChatMessage(SelectedRequestForQuote.Identifier, configManager.CurrentUser, MessageToBeSent);
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
            var success = false;
            if (SelectedRequestForQuote.Identifier == -1)
            {
                var newIdentifer = optionRequestPersistanceManager.SaveRequest(SelectedRequestForQuote);
                if (newIdentifer != -1)
                {
                    SelectedRequestForQuote.Identifier = newIdentifer;
                    chatServiceManager.RegisterParticipant(newIdentifer);
                    success = true;
                }
            }                
            else
                success = optionRequestPersistanceManager.UpdateRequest(SelectedRequestForQuote);

            if (!success)
            {
                MessageBox.Show("Failed to save/update request: " + SelectedRequestForQuote.Request,
                    "Request For Quote Persistance Error", MessageBoxButton.OK, MessageBoxImage.Exclamation);

                if(log.IsErrorEnabled)
                    log.Error("Request for quote was not saved/update => " + SelectedRequestForQuote);
            }
            else
                SelectedRequestForQuote.CalculatePricing(optionRequestPricer);
        }

        public void CancelEdit()
        {
            if(log.IsDebugEnabled)
                log.Debug("User cancelled edit, restoring from backup");

            SelectedRequestForQuote.CopyMembers(backupOfRequestForQuote);
        }
    }
}
