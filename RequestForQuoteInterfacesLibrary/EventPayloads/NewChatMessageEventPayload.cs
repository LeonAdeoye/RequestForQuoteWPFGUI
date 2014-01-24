using System.Text;
using RequestForQuoteInterfacesLibrary.ModelImplementations;

namespace RequestForQuoteInterfacesLibrary.EventPayloads
{
    public class NewChatMessageEventPayload 
    {
        public ChatMessageImpl NewChatMessage { get; set; }

        public override string ToString()
        {
            var builder = new StringBuilder("Owner = ");
            builder.Append(NewChatMessage.Owner);
            builder.Append(", Content = ");
            builder.Append(NewChatMessage.Content);
            builder.Append(", Request For Quote ID = ");
            builder.Append(NewChatMessage.RequestForQuoteId);
            builder.Append(", Sequence ID = ");
            builder.Append(NewChatMessage.SequenceId);
            builder.Append(", Timestamp = ");
            builder.Append(NewChatMessage.TimeStamp);
            return builder.ToString();
        }
    }
}
