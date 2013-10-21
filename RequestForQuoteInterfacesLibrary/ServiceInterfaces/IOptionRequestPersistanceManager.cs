using System.Collections.Generic;
using RequestForQuoteInterfacesLibrary.ModelInterfaces;

namespace RequestForQuoteInterfacesLibrary.ServiceInterfaces
{
    public interface IOptionRequestPersistanceManager
    {
        int SaveRequest(IRequestForQuote requestToSave);
        bool UpdateRequest(IRequestForQuote requestToUpdate);
        IRequestForQuote GetRequest(int identifier, bool rePrice);
        List<IRequestForQuote> GetRequestsForToday(bool rePrice);
        List<IRequestForQuote> GetRequestMatchingAdhocCriteria(ISearchCriterion search, bool rePrice);
        List<IRequestForQuote> GetRequestMatchingExistingCriteria(string criteriaOwner, string criteriaDescriptionKey, bool rePrice);
    }
}
