using System.Collections.Generic;
using RequestForQuoteInterfacesLibrary.ModelInterfaces;

namespace RequestForQuoteInterfacesLibrary.ServiceInterfaces
{
    public interface ISearchManager
    {
        List<ISearch> Searches { get; set; }
        bool SaveSearchToDatabase(string owner, string descriptionKey, bool isPrivate, bool isFilter, string controlName, string controlValue);
        void AddSearch(string owner, string descriptionKey, bool isPrivate, bool isFilter, string controlName, string controlValue);
        bool DeleteSearch(string owner, string descriptionKey);
        bool UpdatePrivacy(string owner, string descriptionKey, bool isPrivate);
        void Initialize(bool isStandAlone);
    }
}