using RequestForQuoteInterfacesLibrary.ModelInterfaces;

namespace RequestForQuoteInterfacesLibrary.EventPayloads
{
    public class NewRequestForQuoteEventPayload 
    {
        public string NewRequestText { get; set; }
        public IClient NewRequestClient { get; set; }
        public string NewRequestBookCode { get; set; }

        public override string ToString()
        {
            return "Text=" + NewRequestText + ", Client=" + NewRequestClient + ", BookCode=" + NewRequestBookCode;
        }
    }
}
