using System.Collections.Generic;
using RequestForQuoteInterfacesLibrary.ModelInterfaces;

namespace RequestForQuoteInterfacesLibrary.ModelImplementations
{
    public class SearchImpl : ISearch
    {
        public List<ISearchCriterion> Criteria { get; set; }
        public string Owner { get; set; }
        public string DescriptionKey { get; set; }
        public bool IsFilter { get; set; }
        public bool IsPrivate { get; set; }

        public SearchImpl()
        {
            Criteria = new List<ISearchCriterion>();    
        }

        public void AddCriteria(ISearchCriterion criterion)
        {
            Criteria.Add(criterion);
        }

        public bool IsValidCriterionForCurrentSearch(ISearchCriterion criterion)
        {
            return this.Owner == criterion.Owner && this.DescriptionKey == criterion.DescriptionKey;
        }
    }
}
