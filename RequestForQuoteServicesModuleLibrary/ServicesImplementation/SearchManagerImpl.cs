using System;
using System.Collections.Generic;
using System.ServiceModel;
using Microsoft.Practices.Prism.Events;
using Microsoft.Practices.ServiceLocation;
using RequestForQuoteInterfacesLibrary.Constants;
using RequestForQuoteInterfacesLibrary.EventPayloads;
using RequestForQuoteInterfacesLibrary.Events;
using RequestForQuoteInterfacesLibrary.ModelImplementations;
using RequestForQuoteInterfacesLibrary.ModelInterfaces;
using RequestForQuoteInterfacesLibrary.ServiceInterfaces;
using RequestForQuoteServicesModuleLibrary.SearchCriteriaService;
using log4net;

namespace RequestForQuoteServicesModuleLibrary.ServicesImplementation
{
    class SearchManagerImpl : ISearchManager
    {
        private static readonly ILog log = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);        
        private readonly IEventAggregator eventAggregator = ServiceLocator.Current.GetInstance<IEventAggregator>();
        private readonly SearchControllerClient searchContollerProxy = new SearchControllerClient();
        public List<ISearch> Searches { get; set; }

        public SearchManagerImpl()
        {
            Searches = new List<ISearch>();
        }
       
        public void Initialize(bool isStandAlone)
        {
            if (isStandAlone)
                return;

            try
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
                                Criteria = new Dictionary<string, string>() { { search.controlName, search.controlValue } }
                            });
                        }
                        else
                            searchToBeUpdated.Criteria[search.controlName] = search.controlValue;
                    }
                }
            }
            catch (EndpointNotFoundException exception)
            {
                log.Error(String.Format("Failed to connect to proxy for remote search controller webservice. Exception thrown {0}", exception));
                throw;
            }
            catch (TimeoutException timeoutException)
            {
                log.Error(String.Format("Timeout: failed to connect to proxy for remote search controller webservice. Exception thrown {0}", timeoutException));
                throw;
            }
        }
        
        public bool SaveSearch(string owner, string descriptionKey, bool isPrivate, bool isFilter, IDictionary<string, string> criteria)
        {
            var wasSavedToDatabase = false;
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
            {
                wasSavedToDatabase = searchContollerProxy.save(RequestForQuoteConstants.MY_USER_NAME, descriptionKey, criterion.Key, criterion.Value, isPrivate, isFilter);
                if (!wasSavedToDatabase)
                    break;
            }
                                    
            eventAggregator.GetEvent<NewSearchEvent>().Publish(new NewSearchEventPayload()
            {
                NewSearch = newSearch,
            });

            // if no save is required then this should return true
            // otherwise if saved required the save through web service proxy must succeed.
            return wasSavedToDatabase;
        }

        public bool DeleteSearch(string owner, string descriptionKey)
        {
            // Remove from Searches and delete from database.
            if (searchContollerProxy.delete(owner, descriptionKey))
            {
                Searches.RemoveAll((existingSearch) => existingSearch.Owner == owner && existingSearch.DescriptionKey == descriptionKey);
                return true;
            }
            return false;
        }

        public bool UpdatePrivacy(string owner, string descriptionKey, bool isPrivate)
        {
            return searchContollerProxy.updatePrivacy(owner, descriptionKey, isPrivate);   
        }
    }
}
