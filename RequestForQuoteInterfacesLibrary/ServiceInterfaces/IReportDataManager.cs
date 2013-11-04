using System;

namespace RequestForQuoteInterfacesLibrary.ServiceInterfaces
{
    public interface IReportDataManager
    {
        void CompileRequestCountPerCategoryReport(string reportType, string categoryType, DateTime fromDate, int minimumCount);
    }
}