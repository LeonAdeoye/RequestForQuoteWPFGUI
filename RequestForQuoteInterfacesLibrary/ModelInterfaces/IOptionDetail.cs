using System;
using RequestForQuoteInterfacesLibrary.Enums;
using RequestForQuoteInterfacesLibrary.ServiceInterfaces;

namespace RequestForQuoteInterfacesLibrary.ModelInterfaces
{
    public interface IOptionDetail
    {
        int LegId { get; set; }
        string RIC { get; set; }
        string BBG { get; set; }
        String Description { get; set; }
        decimal UnderlyingPrice { get; set; }
        decimal Strike { get; set; }
        decimal StrikePercentage { get; }
        decimal ForwardPrice { get; set; }
        ProductTypeEnum ProductType { get; set; }
        decimal Weight { get; set; }
        DateTime MaturityDate { get; set; }
        DateTime TradeDate { get; set; }
        decimal DaysToExpiry { get; set; }
        decimal YearsToExpiry { get; set; }
        DateTime FinalPaymentDate { get; set; }
        decimal PremiumAmount { get; set; }
        decimal PremiumPercentage { get; }
        decimal Delta { get; set; }
        decimal Gamma { get; set; }
        decimal Vega { get; set; }
        decimal Theta { get; set; }
        decimal Rho { get; set; }
        decimal ImpliedVol { get; set; }
        int Quantity { get; set; }
        SideEnum Side { get; set; }
        bool IsCall { get; set; }
        bool IsEuropean { get; set; }
        decimal InterestRate { get; set; }              
        decimal DayCountConvention { get; set; }
        decimal Volatility { get; set; }

        IRequestForQuote ParentRequest { get; set; }
    }
}
