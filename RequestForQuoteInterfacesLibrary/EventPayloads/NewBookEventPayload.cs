using System.Text;
using RequestForQuoteInterfacesLibrary.ModelInterfaces;

namespace RequestForQuoteInterfacesLibrary.EventPayloads
{
    public class NewBookEventPayload 
    {
        public IBook NewBook { get; set; }

        public override string ToString()
        {
            var builder = new StringBuilder("BookCode = ");
            builder.Append(NewBook.BookCode);
            builder.Append(", Entity = ");
            builder.Append(NewBook.Entity);
            builder.Append(", IsValid = ");
            builder.Append(NewBook.IsValid ? "TRUE" : "FALSE");
            return builder.ToString();
        }
    }
}
