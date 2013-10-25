using System;
using System.Diagnostics;
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
            var requestCategoryCounts =  reportingContollerProxy.getRequestsByCategory(categoryType, fromDate, minimumCount);

            foreach (var categoryCount in requestCategoryCounts.RequestCountReportDataListImpl)
                Debug.WriteLine(categoryCount.categoryValue + " = " + categoryCount.requestCount);                
        }
    }
}
