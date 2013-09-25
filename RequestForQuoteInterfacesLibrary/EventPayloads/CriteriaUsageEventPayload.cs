using System.Collections.Generic;
using System.Text;

namespace RequestForQuoteInterfacesLibrary.EventPayloads
{
    public class CriteriaUsageEventPayload
    {
        public Dictionary<string, string> Criteria { get; set; }
        public string Owner { get; set; }
        public bool IsPrivate { get; set; }
        public bool IsFilter { get; set; }
        public string DescriptionKey { get; set; }
        
        public override string ToString()
        {
            var builder = new StringBuilder("[ Owner = ");
            builder.Append(Owner);
            builder.Append(", Description Key = ");
            builder.Append(DescriptionKey);
            builder.Append(", Is Private = ");
            builder.Append(IsPrivate);
            builder.Append(", Is Filter = ");
            builder.Append(IsFilter);
            if (Criteria != null)
            {
                builder.Append(", with SOME criteria: ");
                foreach (var criterion in Criteria)
                {
                    builder.Append("{");
                    builder.Append(criterion.Key);
                    builder.Append(" => ");
                    builder.Append(criterion.Value);
                    builder.Append("}");
                }
            }
            else
                builder.Append(", with NO criteria.");  
            builder.Append("]");
            return builder.ToString();
        }
    }
}
