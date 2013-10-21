using System.Text;
using RequestForQuoteInterfacesLibrary.ModelInterfaces;

namespace RequestForQuoteInterfacesLibrary.EventPayloads
{
    public class NewSearchEventPayload 
    {
        public ISearch NewSearch { get; set; }

        public override string ToString()
        {
            var builder = new StringBuilder("New Search [ Description key: ");
            builder.Append(NewSearch.DescriptionKey);
            builder.Append(", Is private: ");
            builder.Append(NewSearch.IsPrivate);
            builder.Append(", Is filter: ");
            builder.Append(NewSearch.IsFilter);
            builder.Append(", Owner: ");
            builder.Append(NewSearch.Owner);
            builder.Append(", Criteria: [");
            // TODO
            builder.Append(" ]");
            return builder.ToString();
        }
    }
}
