using System.Text;
using RequestForQuoteInterfacesLibrary.ModelInterfaces;

namespace RequestForQuoteInterfacesLibrary.EventPayloads
{
    public class NewUnderlyierEventPayload 
    {
        public IUnderlying NewUnderlying { get; set; }

        public override string ToString()
        {
            var builder = new StringBuilder("RIC = ");
            builder.Append(NewUnderlying.RIC);
            builder.Append(", Description = ");
            builder.Append(NewUnderlying.Description);
            builder.Append(", IsValid = ");
            builder.Append(NewUnderlying.IsValid);
            return builder.ToString();
        }
    }
}
