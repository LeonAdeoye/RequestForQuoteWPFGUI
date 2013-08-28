using System.Collections.Generic;
using Microsoft.Practices.Prism.Events;
using Microsoft.Practices.ServiceLocation;
using RequestForQuoteInterfacesLibrary.Constants;
using RequestForQuoteInterfacesLibrary.EventPayloads;
using RequestForQuoteInterfacesLibrary.Events;
using RequestForQuoteInterfacesLibrary.ModelImplementations;
using RequestForQuoteInterfacesLibrary.ModelInterfaces;
using RequestForQuoteInterfacesLibrary.ServiceInterfaces;
using RequestForQuoteServicesModuleLibrary.SearchCriteriaService;

namespace RequestForQuoteServicesModuleLibrary.ServicesImplementation
{
    class SearchManagerImpl : ISearchManager
    {
        public List<ISearch> Searches { get; set; }
        private readonly IEventAggregator eventAggregator = ServiceLocator.Current.GetInstance<IEventAggregator>();
        private readonly SearchControllerClient searchContollerProxy = new SearchControllerClient();

        public SearchManagerImpl()
        {
            Searches = new List<ISearch>();
            Initialize();
        }
       
        private void Initialize()
        {
            var searches = searchContollerProxy.getAll();
            if (searches != null)
            {
                foreach (var search in searches)
                {
                    var searchToBeUpdated = Searches.Find((existingSearch) => (existingSearch.Owner == search.owner) && (existingSearch.DescriptionKey == search.key));
                    if (searchToBeUpdated == null)
                    {
                        Searches.Add(new SearchImpl
                            {
                                Owner = search.owner,
                                DescriptionKey = search.key,
                                IsFilter = search.isFilter,
                                IsPrivate = search.isPrivate,
                                Criteria = new Dictionary<string, string>() {{search.controlName, search.controlValue}}
                            });
                    }
                    else
                        searchToBeUpdated.Criteria[search.controlName] = search.controlValue;
                }                
            }
        }
        
        public void SaveSearch(string owner, string descriptionKey, bool isPrivate, bool isFilter, IDictionary<string, string> criteria)
        {
            var newCriteria = new Dictionary<string, string>(criteria);
            var newSearch = new SearchImpl
                {
                    Owner = owner,
                    DescriptionKey = descriptionKey,
                    IsPrivate = isPrivate,
                    IsFilter = isFilter,
                    Criteria = newCriteria
                };
            
            Searches.Add(newSearch);

            foreach (var criterion in criteria)
                searchContollerProxy.save(RequestForQuoteConstants.MY_USER_NAME, descriptionKey, criterion.Key, criterion.Value, isPrivate, isFilter);
                                    
            eventAggregator.GetEvent<NewSearchEvent>().Publish(new NewSearchEventPayload()
            {
                NewSearch = newSearch,
            });
        }

        public void DeleteSearch(string owner, string descriptionKey)
        {
            // Remove from Searches and delete from database.
            searchContollerProxy.delete(owner, descriptionKey);
            Searches.RemoveAll((existingSearch) => existingSearch.Owner == owner && existingSearch.DescriptionKey == descriptionKey);
        }

        public void UpdatePrivacy(string owner, string descriptionKey, bool isPrivate)
        {
            searchContollerProxy.updatePrivacy(owner, descriptionKey, isPrivate);   
        }
    }
}
