using System.Collections.Generic;

namespace RequestForQuoteInterfacesLibrary.ModelInterfaces
{
    public interface ISearch
    {
        string DescriptionKey { get; set; }
        bool IsPrivate { get; set; }
        bool IsFilter { get; set; }
        string Owner { get; set; }
        IDictionary<string, string> Criteria { get; set; }
    }
}
