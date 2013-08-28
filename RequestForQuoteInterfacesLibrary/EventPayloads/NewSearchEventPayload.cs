using System.Text;
using RequestForQuoteInterfacesLibrary.ModelInterfaces;

namespace RequestForQuoteInterfacesLibrary.EventPayloads
{
    public class NewSearchEventPayload 
    {
        public ISearch NewSearch { get; set; }

        public override string ToString()
        {
            var builder = new StringBuilder("New Search = [DescriptionKey =");
            builder.Append(NewSearch.DescriptionKey);
            builder.Append(", Is Private = ");
            builder.Append(NewSearch.IsPrivate);
            builder.Append(", Owner = ");
                builder.Append(NewSearch.Owner);
            builder.Append(", Criteria = {");
            foreach (var pair in NewSearch.Criteria)
            {
                builder.Append("{");
                builder.Append(pair.Key);
                builder.Append(" => ");
                builder.Append(pair.Value);
                builder.Append("}");
            }
            builder.Append("}");
            return builder.ToString();
        }
    }
}
