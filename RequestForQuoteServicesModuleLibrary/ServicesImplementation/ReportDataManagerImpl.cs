using System;
using System.Diagnostics;
using System.ServiceModel;
using RequestForQuoteServicesModuleLibrary.ReportingService;
using log4net;

namespace RequestForQuoteServicesModuleLibrary.ServicesImplementation
{
    public class ReportDataManagerImpl : IReportDataManager
    {
        private static readonly ILog log = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private readonly ReportingControllerClient reportingContollerProxy = new ReportingControllerClient();

        public void GetRequestCountPerCategory(string categoryType, DateTime fromDate, int minimumCount)
        {
            try
            {
                var requestCategoryCounts = reportingContollerProxy.getRequestsByCategory(categoryType, fromDate,
                                                                                          minimumCount);

                foreach (var categoryCount in requestCategoryCounts.RequestCountReportDataListImpl)
                    Debug.WriteLine(categoryCount.categoryValue + " = " + categoryCount.requestCount);
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
