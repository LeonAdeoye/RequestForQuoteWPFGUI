using System.Text;

namespace RequestForQuoteInterfacesLibrary.EventPayloads
{
    public class RequestSelectionEventPayload 
    {
        public int RequestId { get; set; }

        public override string ToString()
        {
            var builder = new StringBuilder("Request Id = ");
            builder.Append(RequestId);
            return builder.ToString();
        }
    }
}
