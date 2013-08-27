using System.Text;
using RequestForQuoteInterfacesLibrary.ModelInterfaces;

namespace RequestForQuoteInterfacesLibrary.EventPayloads
{
    public class NewClientEventPayload 
    {
        public IClient NewClient { get; set; }

        public override string ToString()
        {
            var builder = new StringBuilder("Name = ");
            builder.Append(NewClient.Name);
            builder.Append(", Tier = ");
            builder.Append(NewClient.Tier);
            builder.Append(", Identifier = ");
            builder.Append(NewClient.Identifier.ToString());
            builder.Append(", IsValid = ");
            builder.Append(NewClient.IsValid ? "TRUE" : "FALSE");
            return builder.ToString();
        }
    }
}
