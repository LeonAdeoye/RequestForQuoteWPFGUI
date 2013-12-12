using System;
using System.Collections.Generic;

namespace RequestForQuoteInterfacesLibrary.ServiceInterfaces
{
    public interface IReportDataManager
    {
        void CompileRequestCountByCategoryReport(string reportType, string categoryType, DateTime fromDate, int minimumCount);
        void CompileGreeksByCategoryReport(string reportType, string categoryType, ISet<string> greeks, DateTime maturityDateFrom, DateTime maturityDateTo, double minimumGreek);
        void CompileGreeksByInputReport(string reportType, string categoryType, ISet<string> greeks, int requestId, double minimumInput, double maximumInput);
    }
}