using System;
using System.Collections.Generic;
using System.ServiceModel;
using RequestForQuoteInterfacesLibrary.ModelImplementations;
using RequestForQuoteInterfacesLibrary.ServiceInterfaces;
using RequestForQuoteServicesModuleLibrary.ChatService;
using log4net;

namespace RequestForQuoteServicesModuleLibrary.ServicesImplementation
{
    public sealed class ChatServiceManagerImpl : IChatServiceManager
    {
        private static readonly ILog log = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);        
        private readonly ChatMediatorClient chatMediatorProxy = new ChatMediatorClient();
        private readonly IConfigurationManager configManager;

        public ChatServiceManagerImpl(IConfigurationManager configManager)
        {
            if (configManager == null)
                throw new ArgumentNullException("configManager");

            this.configManager = configManager;
        }

        public void SendChatMessage(int requestForQuoteId, string sender, string message)
        {
            if (String.IsNullOrEmpty(sender))
                throw new ArgumentException("sender");

            if (String.IsNullOrEmpty(message))
                throw new ArgumentException("message");

            try
            {
                chatMediatorProxy.sendMessage(requestForQuoteId, sender, message);
            }
            catch (EndpointNotFoundException exception)
            {
                if(log.IsErrorEnabled)
                    log.Error("Failed to send chat message via web service. Exception thrown: ", exception);
            }            
        }

        public List<ChatMessageImpl> GetChatMessages(int requestForQuoteId, int fromThisSequenceId)
        {
            var listOfMessages = new List<ChatMessageImpl>();
            try
            {
                var messages = chatMediatorProxy.getChatMessages(requestForQuoteId, fromThisSequenceId);
                foreach (var message in messages.ChatMessageListImpl)
                {
                    listOfMessages.Add(new ChatMessageImpl(message.owner, message.content, message.requestForQuoteId, message.sequenceId, Convert.ToDateTime(message.timeStamp)));
                }
            }
            catch (EndpointNotFoundException exception)
            {
                if (log.IsErrorEnabled)
                    log.Error("Failed to get chat messages via web service. Exception thrown: ", exception);
            }
            

            return listOfMessages;
        }

        public List<ChatMessageImpl> GetAllPreviousChatMessages(int requestForQuoteId)
        {
            const int fromThisSequenceId = 0;
            return GetChatMessages(requestForQuoteId, fromThisSequenceId);
        }

        public List<ChatMessageImpl> RegisterParticipant(int requestForQuoteId)
        {
            var result = new List<ChatMessageImpl>();
            try
            {
                var messages = chatMediatorProxy.registerParticipant(requestForQuoteId,
                                                                     configManager.CurrentUser);

                if (messages.chatMessageList != null)
                    foreach (var message in messages.chatMessageList)
                        result.Add(new ChatMessageImpl(message.owner, message.content, message.requestForQuoteId,
                                                       message.sequenceId, Convert.ToDateTime(message.timeStamp)));
            }
            catch (FormatException feException)
            {
                if (log.IsErrorEnabled)
                    log.Error("Failed to convert chat message timeStamp to datetime. Exception thrown: ", feException);
            }
            catch (EndpointNotFoundException exception)
            {
                if (log.IsErrorEnabled)
                    log.Error("Failed to register participant via web service. Exception thrown: ", exception);
            }
            return result;
        }

        public void UnregisterParticpant(int requestForQuoteId)
        {
            try
            {
                chatMediatorProxy.unregisterParticipant(requestForQuoteId, configManager.CurrentUser);
            }
            catch (EndpointNotFoundException exception)
            {
                if (log.IsErrorEnabled)
                    log.Error("Failed to unregister participant via web service. Exception thrown: ", exception);
            }
        }
    }
}
