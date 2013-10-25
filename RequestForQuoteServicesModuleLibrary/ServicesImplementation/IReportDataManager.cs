using System;

namespace RequestForQuoteServicesModuleLibrary.ServicesImplementation
{
    public interface IReportDataManager
    {
        void GetRequestCountPerCategory(string categoryType, DateTime fromDate, int minimumCount);
    }
}