using System.Collections.Generic;
using RequestForQuoteInterfacesLibrary.ModelImplementations;
using RequestForQuoteInterfacesLibrary.ModelInterfaces;

namespace RequestForQuoteInterfacesLibrary.ServiceInterfaces
{
    public interface IChatServiceManager
    {
        void SendChatMessage(int requestForQuoteId, string sender, string message);
        List<ChatMessageImpl> GetChatMessages(int requestForQuoteId, int fromThisSequenceId);
        List<ChatMessageImpl> GetAllPreviousChatMessages(int requestForQuoteId);
        List<ChatMessageImpl> RegisterParticipant(int requestForQuoteId);
    }
}