using System.Collections.Generic;
using RequestForQuoteInterfacesLibrary.ModelInterfaces;

namespace RequestForQuoteInterfacesLibrary.ServiceInterfaces
{
    public interface ISearchManager
    {
        List<ISearch> Searches { get; set; }

        void SaveSearch(string owner, string descriptionKey, bool isPrivate, bool isFilter, IDictionary<string, string> criteria);
        void DeleteSearch(string owner, string descriptionKey);
        void UpdatePrivacy(string owner, string descriptionKey, bool isPrivate);
    }
}