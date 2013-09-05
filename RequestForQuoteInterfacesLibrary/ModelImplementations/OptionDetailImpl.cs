using System;
using System.Text;
using RequestForQuoteInterfacesLibrary.Enums;
using RequestForQuoteInterfacesLibrary.ModelInterfaces;

namespace RequestForQuoteInterfacesLibrary.ModelImplementations
{
    public class OptionDetailImpl : IOptionDetail
    {
        public static decimal DAY_COUNT_CONVENTION_255 = 255;
        public static decimal DAY_COUNT_CONVENTION_250 = 250;
        public static decimal DAY_COUNT_CONVENTION_265 = 265;

        public int LegId { get; set; }
        public decimal Strike { get; set; }
        public decimal StrikePercentage { get; set; }
        public int Quantity { get; set; }
        public string RIC { get; set; }
        public string BBG { get; set; }
        public string Underlying { get; set; }
        public decimal UnderlyingPrice { get; set; }
        public SideEnum Side { get; set; }
        public bool IsCall { get; set; }
        public bool IsEuropean { get; set; }
        public decimal Delta { get; set; }
        public decimal Gamma { get; set; }
        public decimal Vega { get; set; }
        public decimal Theta { get; set; }
        public decimal Rho { get; set; }
        public decimal InterestRate { get; set; }
        public decimal DaysToExpiry { get; set; }
        public decimal MonthsToExpiry { get; set; }
        public decimal YearsToExpiry { get; set; }
        public decimal DayCountConvention { get; set; }
        public DateTime MaturityDate { get; set; }
        public DateTime TradeDate { get; set; }
        public decimal Bid { get; set; }
        public decimal Ask { get; set; }
        public decimal BidPercentage { get; set; }
        public decimal AskPercentage { get; set; }
        public decimal Price { get; set; }
        public decimal AskVolatility { get; set; }
        public decimal BidVolatility { get; set; }
        public decimal Volatility { get; set; }
        public decimal AskImpliedVol { get; set; }
        public decimal BidImpliedVol { get; set; }
        public decimal AskRepoBump { get; set; }
        public decimal BidRepoBump { get; set; }
        public decimal AskDividendBump { get; set; }
        public decimal BidDividendBump { get; set; }
        public DateTime AskVolatilityUpdated { get; set; }
        public DateTime BidVolatilityUpdated { get; set; }
        public IRequestForQuote ParentRequest { get; set; }
        public decimal ForwardPrice { get; set; }
        public decimal DividendPercentageOfSpot { get; set; }
        
        public override string ToString()
        {
            // TODO add the others
            StringBuilder builder = new StringBuilder("Strike: ");
            builder.Append(this.Strike);
            builder.Append(", Quantity: ");
            builder.Append(this.Quantity);
            builder.Append(", Side: ");
            builder.Append(this.Side);
            return builder.ToString();
        }
    }
}
