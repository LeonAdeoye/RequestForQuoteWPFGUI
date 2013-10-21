using System.Collections.Generic;

namespace RequestForQuoteInterfacesLibrary.ModelInterfaces
{
    public interface ISearch
    {
        List<ISearchCriterion> Criteria { get; set; }
        string Owner { get; set; }
        string DescriptionKey { get; set; }
        bool IsFilter { get; set; }
        bool IsPrivate { get; set; }
    }
}
