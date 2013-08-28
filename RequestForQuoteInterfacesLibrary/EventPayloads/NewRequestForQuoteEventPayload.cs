namespace RequestForQuoteInterfacesLibrary.EventPayloads
{
    public class NewRequestForQuoteEventPayload 
    {
        public string NewRequestText { get; set; }
        public string NewRequestClient { get; set; }
        public override string ToString()
        {
            return "Text=" + NewRequestText + "Client=" + NewRequestClient;
        }
    }
}
