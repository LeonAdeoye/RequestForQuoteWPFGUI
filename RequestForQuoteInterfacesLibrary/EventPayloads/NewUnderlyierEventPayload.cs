using System.Text;
using RequestForQuoteInterfacesLibrary.ModelInterfaces;

namespace RequestForQuoteInterfacesLibrary.EventPayloads
{
    public class NewUnderlyierEventPayload 
    {
        public IUnderlyier NewUnderlyier { get; set; }

        public override string ToString()
        {
            var builder = new StringBuilder("RIC = ");
            builder.Append(NewUnderlyier.RIC);
            builder.Append(", BBG = ");
            builder.Append(NewUnderlyier.BBG);
            builder.Append(", Description = ");
            builder.Append(NewUnderlyier.Description);
            builder.Append(", IsValid = ");
            builder.Append(NewUnderlyier.IsValid ? "TRUE" : "FALSE");
            return builder.ToString();
        }
    }
}
