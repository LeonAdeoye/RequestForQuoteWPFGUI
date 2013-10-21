using System;
using System.Collections.Generic;
using System.Linq;
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
    sealed class SearchManagerImpl : ISearchManager
    {
        private static readonly ILog log = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);        
        private readonly IEventAggregator eventAggregator = ServiceLocator.Current.GetInstance<IEventAggregator>();
        private readonly SearchControllerClient searchContollerProxy = new SearchControllerClient();
        public List<ISearch> Searches { get; set; }

        public SearchManagerImpl()
        {
            Searches = new List<ISearch>();
        }

        public ISearch AddCriterionToRelatedSearch(string owner, string descriptionKey, string controlName, string controlValue, bool isPrivate, bool isFilter)
        {
            var criterion = new SearchCriterionImpl
            {
                ControlName = controlName,
                ControlValue = controlValue,
                Owner = owner,
                DescriptionKey = descriptionKey,
                IsFilter = isFilter,
                IsPrivate = isPrivate
            };

            var searchToBeUpdated = Searches.FirstOrDefault((existingSearch) => (existingSearch.Owner == owner) && (existingSearch.DescriptionKey == descriptionKey));
            if (searchToBeUpdated == null)
            {
                if(log.IsDebugEnabled)
                    log.Debug("Adding JSON search criterion to NON-EXISTENT search with owner: " + owner + " and key: " + descriptionKey);

                var newSearch = new SearchImpl
                    {
                        Owner = criterion.Owner,
                        DescriptionKey = criterion.DescriptionKey,
                        IsFilter = criterion.IsFilter,
                        IsPrivate = criterion.IsPrivate,
                    };
                newSearch.Criteria.Add(criterion);
                Searches.Add(newSearch);
                return newSearch;
            }

            if (log.IsDebugEnabled)
                log.Debug("Adding JSON search criterion to EXISTING search with owner: " + owner + " and key: " + descriptionKey);

            searchToBeUpdated.Criteria.Add(criterion);
            return searchToBeUpdated;
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
                        AddCriterionToRelatedSearch(search.owner, search.key, search.controlName, search.controlValue, search.isPrivate, search.isFilter);                   
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
        
        public void AddSearch(string owner, string descriptionKey, bool isPrivate, bool isFilter, string controlName, string controlValue)
        {                                   
            eventAggregator.GetEvent<NewSearchEvent>().Publish(new NewSearchEventPayload()
            {
                NewSearch = AddCriterionToRelatedSearch(owner, descriptionKey, controlName, controlValue, isPrivate, isFilter)
            });
        }

        public bool SaveSearchToDatabase(string owner, string descriptionKey, bool isPrivate, bool isFilter, string controlName, string controlValue)
        {
            return searchContollerProxy.save(RequestForQuoteConstants.MY_USER_NAME, descriptionKey, controlName, controlValue, isPrivate, isFilter);    
        }

        public bool DeleteSearch(string owner, string descriptionKey)
        {
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
