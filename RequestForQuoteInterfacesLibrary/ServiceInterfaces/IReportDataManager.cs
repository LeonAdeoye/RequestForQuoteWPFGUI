using System;

namespace RequestForQuoteInterfacesLibrary.ServiceInterfaces
{
    public interface IReportDataManager
    {
        void GetRequestCountPerCategory(string reportType, string categoryType, DateTime fromDate, int minimumCount);
    }
}