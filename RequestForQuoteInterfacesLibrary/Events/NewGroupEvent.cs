using Microsoft.Practices.Prism.Events;
using RequestForQuoteInterfacesLibrary.EventPayloads;

namespace RequestForQuoteInterfacesLibrary.Events
{
    public class NewGroupEvent : CompositePresentationEvent<NewGroupEventPayload> {}
}
