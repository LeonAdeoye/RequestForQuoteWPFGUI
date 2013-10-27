using System;
using System.Diagnostics;
using System.ServiceModel;
using Microsoft.Practices.Prism.Events;
using Microsoft.Practices.ServiceLocation;
using RequestForQuoteInterfacesLibrary.EventPayloads;
using RequestForQuoteInterfacesLibrary.Events;
using RequestForQuoteServicesModuleLibrary.ReportingService;
using log4net;

namespace RequestForQuoteServicesModuleLibrary.ServicesImplementation
{
    public class ReportDataManagerImpl : IReportDataManager
    {
        private static readonly ILog log = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private static readonly IEventAggregator eventAggregator     = ServiceLocator.Current.GetInstance<IEventAggregator>();
        private readonly ReportingControllerClient reportingContollerProxy = new ReportingControllerClient();

        public void GetRequestCountPerCategory(string reportType, string categoryType, DateTime fromDate, int minimumCount)
        {
            try
            {
                var requestCategoryCounts = reportingContollerProxy.getRequestsByCategory(categoryType, fromDate, minimumCount);

                if (requestCategoryCounts.RequestCountReportDataListImpl.Length > 0)
                {
                    var eventPayLoad = new RequestsCountByCategoryReportEventPayLoad
                        {
                            ReportType = reportType,
                            Category = categoryType,
                            FromDate = fromDate,
                            MinimumCount = minimumCount
                        };

                    foreach (var categoryCount in requestCategoryCounts.RequestCountReportDataListImpl)
                        eventPayLoad.CountByCategory.Add(categoryCount.categoryValue, categoryCount.requestCount);

                    eventAggregator.GetEvent<RequestsCountByCategoryReportEvent>().Publish(eventPayLoad);
                };
            }
            catch (FaultException fe)
            {
                if (log.IsErrorEnabled)
                    log.Error("Exception thrown while compile report data for requests by category: " + categoryType + ": " + fe);
            }
            catch (EndpointNotFoundException epnfe)
            {
                if (log.IsErrorEnabled)
                    log.Error("Exception thrown while compile report data for requests by category: " + categoryType + ": " + epnfe);
            }
        }
    }
}
