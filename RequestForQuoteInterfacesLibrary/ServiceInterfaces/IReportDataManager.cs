using System;
using System.Collections.Generic;

namespace RequestForQuoteInterfacesLibrary.ServiceInterfaces
{
    public interface IReportDataManager
    {
        void CompileRequestCountByCategoryReport(string reportType, string categoryType, DateTime fromDate, int minimumCount);
        void CompileGreeksByCategoryReport(string reportType, string categoryType, ISet<string> greeks, DateTime maturityDateFrom, DateTime maturityDateTo, double minimumGreek);
        void CompileGreeksExtrapolationReport(string reportType, string rangeVariable, ISet<string> greeks, int requestId, double rangeMinimum, double rangeMaximum, double rangeIncrement);
    }
}