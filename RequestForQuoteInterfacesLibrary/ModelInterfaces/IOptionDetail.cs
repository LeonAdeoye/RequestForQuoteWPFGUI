using System;
using RequestForQuoteInterfacesLibrary.Enums;
using RequestForQuoteInterfacesLibrary.ServiceInterfaces;

namespace RequestForQuoteInterfacesLibrary.ModelInterfaces
{
    public interface IOptionDetail
    {
        int LegId { get; set; }
        decimal Strike { get; set; }
        decimal StrikePercentage { get; set; }
        int Quantity { get; set; }
        string RIC { get; set; }
        string BBG { get; set; }
        decimal UnderlyingPrice { get; set; }
        decimal ForwardPrice { get; set; }
        decimal DividendPercentageOfSpot { get; set; }
        SideEnum Side { get; set; }
        bool IsCall { get; set; }
        bool IsEuropean { get; set; }
        decimal Delta { get; set; }
        decimal Gamma { get; set; }
        decimal Vega { get; set; }
        decimal Theta { get; set; }
        decimal Rho { get; set; }
        decimal InterestRate { get; set; }
        decimal DaysToExpiry { get; set; }
        decimal MonthsToExpiry { get; set; }
        decimal YearsToExpiry { get; set; }
        decimal DayCountConvention { get; set; }
        DateTime MaturityDate { get; set; }
        DateTime TradeDate { get; set; }
        decimal Bid { get; set; }
        decimal Ask { get; set; }
        decimal BidPercentage { get; set; }
        decimal AskPercentage { get; set; }
        decimal Price { get; set; }
        decimal AskVolatility { get; set; }
        decimal BidVolatility { get; set; }
        decimal Volatility { get; set; }
        decimal AskImpliedVol { get; set; }
        decimal BidImpliedVol { get; set; }
        decimal AskRepoBump { get; set; }
        decimal BidRepoBump { get; set; }
        decimal AskDividendBump { get; set; }
        decimal BidDividendBump { get; set; }
        DateTime AskVolatilityUpdated { get; set; }
        DateTime BidVolatilityUpdated { get; set; }

        IRequestForQuote ParentRequest { get; set; }
    }
}
