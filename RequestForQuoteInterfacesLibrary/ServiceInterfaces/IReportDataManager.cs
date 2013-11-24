using System;
using System.Collections.Generic;

namespace RequestForQuoteInterfacesLibrary.ServiceInterfaces
{
    public interface IReportDataManager
    {
        void CompileRequestCountPerCategoryReport(string reportType, string categoryType, DateTime fromDate, int minimumCount);
        void CompileGreeksPerCategoryReport(string reportType, string categoryType, ISet<string> greeks, DateTime maturityDateFrom, DateTime maturityDateTo, decimal minimumGreek);
    }
}