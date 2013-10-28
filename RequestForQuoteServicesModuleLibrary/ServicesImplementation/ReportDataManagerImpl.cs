using System;
using System.Diagnostics;
using System.ServiceModel;
using Microsoft.Practices.Prism.Events;
using Microsoft.Practices.ServiceLocation;
using RequestForQuoteInterfacesLibrary.EventPayloads;
using RequestForQuoteInterfacesLibrary.Events;
using RequestForQuoteInterfacesLibrary.ServiceInterfaces;
using RequestForQuoteServicesModuleLibrary.ReportingService;
using log4net;

namespace RequestForQuoteServicesModuleLibrary.ServicesImplementation
{
    public class ReportDataManagerImpl : IReportDataManager
    {
        private static readonly ILog log = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private static readonly IEventAggregator eventAggregator     = ServiceLocator.Current.GetInstance<IEventAggregator>();
        private static readonly IConfigurationManager configManager = ServiceLocator.Current.GetInstance<IConfigurationManager>();
        private readonly ReportingControllerClient reportingContollerProxy = new ReportingControllerClient();

        public void GetRequestCountPerCategory(string reportType, string categoryType, DateTime fromDate, int minimumCount)
        {
            if (String.IsNullOrEmpty(reportType))
                throw new ArgumentException("reportType");

            if (String.IsNullOrEmpty(categoryType))
                throw new ArgumentException("categoryType");

            try
            {
                var eventPayLoad = new RequestsCountByCategoryReportEventPayLoad
                    {
                        ReportType = reportType,
                        Category = categoryType,
                        FromDate = fromDate,
                        MinimumCount = minimumCount,                       
                    };

                if (!configManager.IsStandAlone())
                {
                    foreach (var categoryCount in reportingContollerProxy.getRequestsByCategory(categoryType, fromDate, minimumCount))
                        eventPayLoad.CountByCategory.Add(categoryCount.categoryValue, categoryCount.requestCount);
                }

                eventAggregator.GetEvent<RequestsCountByCategoryReportEvent>().Publish(eventPayLoad);

            }
            catch (FaultException fe)
            {
                if (log.IsErrorEnabled)
                    log.Error("Exception thrown while compile report data for requests by category: " + categoryType +
                              ": " + fe);
            }
            catch (EndpointNotFoundException epnfe)
            {
                if (log.IsErrorEnabled)
                    log.Error("Exception thrown while compile report data for requests by category: " + categoryType +
                              ": " + epnfe);
            }
            catch (NullReferenceException nre)
            {
                if (log.IsErrorEnabled)
                    log.Error("Exception thrown while compile report data for requests by category: " + categoryType +
                              ": " + nre);                
            }
        }
    }
}
