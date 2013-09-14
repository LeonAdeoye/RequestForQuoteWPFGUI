using System.Collections.Generic;
using RequestForQuoteInterfacesLibrary.ModelInterfaces;

namespace RequestForQuoteInterfacesLibrary.ServiceInterfaces
{
    public interface ISearchManager
    {
        List<ISearch> Searches { get; set; }

        bool SaveSearch(string owner, string descriptionKey, bool isPrivate, bool isFilter, IDictionary<string, string> criteria);
        bool DeleteSearch(string owner, string descriptionKey);
        bool UpdatePrivacy(string owner, string descriptionKey, bool isPrivate);
        void Initialize(bool isStandAlone);
    }
}