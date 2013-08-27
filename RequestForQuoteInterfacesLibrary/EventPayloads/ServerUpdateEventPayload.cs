namespace RequestForQuoteInterfacesLibrary.EventPayloads
{
    public class ServerUpdateEventPayload 
    {
        public string Content { get; set; }

        public override string ToString()
        {
            return Content;
        }
    }
}
