using System;
using System.Collections.Generic;
using System.Linq;
using RequestForQuoteInterfacesLibrary.Constants;
using RequestForQuoteInterfacesLibrary.ModelImplementations;
using RequestForQuoteInterfacesLibrary.ServiceInterfaces;
using RequestForQuoteServicesModuleLibrary.ChatService;

namespace RequestForQuoteServicesModuleLibrary.ServicesImplementation
{
    public class ChatServiceManagerImpl : IChatServiceManager
    {
        private readonly ChatMediatorClient chatMediatorProxy = new ChatMediatorClient();

        public void SendChatMessage(int requestForQuoteId, string sender, string message)
        {
            chatMediatorProxy.sendMessage(requestForQuoteId, sender, message);
        }

        public List<ChatMessageImpl> GetChatMessages(int requestForQuoteId, int fromThisSequenceId)
        {
            var listOfMessages = new List<ChatMessageImpl>();
            var messages = chatMediatorProxy.getChatMessages(requestForQuoteId, fromThisSequenceId);
            foreach (var message in messages.ChatMessageListImpl)
            {
                listOfMessages.Add(new ChatMessageImpl(message.owner, message.content, message.requestForQuoteId, message.sequenceId, Convert.ToDateTime(message.timeStamp)));
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
            var messages = chatMediatorProxy.registerParticipant(requestForQuoteId, RequestForQuoteConstants.MY_USER_NAME);
            var result = new List<ChatMessageImpl>();
            if (messages.chatMessageList != null)
                foreach (var message in messages.chatMessageList)
                    result.Add(new ChatMessageImpl(message.owner, message.content, message.requestForQuoteId, message.sequenceId, Convert.ToDateTime(message.timeStamp)));
            return result;
        }

        public void UnregisterParticpant(int requestForQuoteId)
        {
            chatMediatorProxy.unregisterParticipant(requestForQuoteId, RequestForQuoteConstants.MY_USER_NAME);
        }
    }
}
