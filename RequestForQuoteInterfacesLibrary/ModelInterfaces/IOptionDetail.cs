using System;
using RequestForQuoteInterfacesLibrary.Enums;
using RequestForQuoteInterfacesLibrary.ServiceInterfaces;

namespace RequestForQuoteInterfacesLibrary.ModelInterfaces
{
    public interface IOptionDetail
    {
        int LegId { get; set; }
        string RIC { get; set; }
        String Description { get; set; }
        double UnderlyingPrice { get; set; }
        double Strike { get; set; }
        double StrikePercentage { get; }
        ProductTypeEnum ProductType { get; set; }
        DateTime MaturityDate { get; set; }
        DateTime TradeDate { get; set; }
        double DaysToExpiry { get; set; }
        double YearsToExpiry { get; set; }
        double PremiumAmount { get; set; }
        double PremiumPercentage { get; }
        double Delta { get; set; }
        double Gamma { get; set; }
        double Vega { get; set; }
        double Theta { get; set; }
        double Rho { get; set; }
        double ImpliedVol { get; set; }
        int Quantity { get; set; }
        SideEnum Side { get; set; }
        bool IsCall { get; set; }
        bool IsEuropean { get; set; }
        double InterestRate { get; set; }              
        double DayCountConvention { get; set; }
        double Volatility { get; set; }

        IRequestForQuote ParentRequest { get; set; }
        IOptionDetail CloneOptionDetails();
    }
}
