using System;

namespace RequestForQuoteServicesModuleLibrary.ServicesImplementation
{
    public interface IReportDataManager
    {
        void GetRequestCountPerCategory(string reportType, string categoryType, DateTime fromDate, int minimumCount);
    }
}