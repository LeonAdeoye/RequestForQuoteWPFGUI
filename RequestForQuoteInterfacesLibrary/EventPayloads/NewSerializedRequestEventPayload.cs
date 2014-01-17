using System.Text;
using RequestForQuoteInterfacesLibrary.ModelInterfaces;

namespace RequestForQuoteInterfacesLibrary.EventPayloads
{
    public class NewSerializedRequestEventPayload 
    {
        public IRequestForQuote NewSerializedRequest { get; set; }

        public override string ToString()
        {
            var builder = new StringBuilder("Request = ");
            builder.Append(NewSerializedRequest);
            return builder.ToString();
        }
    }
}
