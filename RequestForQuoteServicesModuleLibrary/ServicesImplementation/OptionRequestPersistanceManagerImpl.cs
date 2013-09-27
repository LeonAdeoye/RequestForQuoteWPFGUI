using System.Collections.Generic;
using RequestForQuoteInterfacesLibrary.ModelInterfaces;
using RequestForQuoteInterfacesLibrary.ServiceInterfaces;
using RequestForQuoteServicesModuleLibrary.RequestMaintenanceService;

namespace RequestForQuoteServicesModuleLibrary.ServicesImplementation
{
    public class OptionRequestPersistanceManagerImpl : IOptionRequestPersistanceManager
    {
        private readonly RequestControllerClient requestControllerProxy  = new RequestControllerClient();

        public int SaveRequest(IRequestForQuote requestToSave)
        {
            var requestDetail = new requestDetailImpl();
            if(requestToSave.Legs != null && requestToSave.Legs.Count > 0)
            {
                var legsArray = new optionDetailImpl[requestToSave.Legs.Count];
                var legCount = 0;
                foreach(var leg in requestToSave.Legs)
                {
                    legsArray[legCount++] = new optionDetailImpl()
                        {
                            isCall = leg.IsCall,
                            legId = leg.LegId,
                            isEuropean = leg.IsEuropean
                            // TODO add more properties
                        };
                }
                requestDetail.legs = new optionDetailListImpl() {optionDetailList = legsArray};
            }

            requestDetail.bookCode = requestToSave.BookCode;
            requestDetail.request = requestToSave.Request;

            return requestControllerProxy.save(requestDetail);
        }

        public bool UpdateRequest(IRequestForQuote requestToUpdate)
        {
            var requestDetail = new requestDetailImpl();
            if (requestToUpdate.Legs != null && requestToUpdate.Legs.Count > 0)
            {
                var legsArray = new optionDetailImpl[requestToUpdate.Legs.Count];
                var legCount = 0;
                foreach (var leg in requestToUpdate.Legs)
                {
                    legsArray[legCount++] = new optionDetailImpl()
                    {
                        isCall = leg.IsCall,
                        legId = leg.LegId,
                        isEuropean = leg.IsEuropean
                        // TODO add more properties
                    };
                }
                requestDetail.legs = new optionDetailListImpl() { optionDetailList = legsArray };
            }

            requestDetail.bookCode = requestToUpdate.BookCode;
            requestDetail.request = requestToUpdate.Request;

            return requestControllerProxy.update(requestDetail);
        }

        public IRequestForQuote GetRequest(int identifier, bool rePrice)
        {
            throw new System.NotImplementedException();
        }

        public List<IRequestForQuote> GetRequestMatchingAdhocCriteria(ISearch search, bool rePrice)
        {
            throw new System.NotImplementedException();
        }

        public List<IRequestForQuote> GetRequestMatchingExistingCriteria(string criteriaOwner, string criteriaDescriptionKey, bool rePrice)
        {
            throw new System.NotImplementedException();
        }
    }
}
