using System;
using System.Text;
using RequestForQuoteInterfacesLibrary.Enums;
using RequestForQuoteInterfacesLibrary.ModelInterfaces;

namespace RequestForQuoteInterfacesLibrary.ModelImplementations
{
    public sealed class OptionDetailImpl : IOptionDetail
    {
        public static double DAY_COUNT_CONVENTION_255 = 255;
        public static double DAY_COUNT_CONVENTION_250 = 250;
        public static double DAY_COUNT_CONVENTION_265 = 265;
        private int quantity;

        public int LegId { get; set; }
        public double Strike { get; set; }
        public double StrikePercentage 
        {
            get { return 100*Strike/UnderlyingPrice; }
        }
        public int Quantity
        {
            get { return (Side == SideEnum.SELL && quantity > 0) ? -1 * quantity : quantity; }
            set { quantity = value; }
        }
        public string RIC { get; set; }
        public string BBG { get; set; }
        //public string Underlying { get; set; }
        public double UnderlyingPrice { get; set; }
        public SideEnum Side { get; set; }
        public bool IsCall { get; set; }
        public bool IsEuropean { get; set; }
        public double Delta { get; set; }
        public double Gamma { get; set; }
        public double Vega { get; set; }
        public double Theta { get; set; }
        public double Rho { get; set; }
        public double InterestRate { get; set; }
        public double DaysToExpiry { get; set; }
        public double YearsToExpiry { get; set; }
        public double DayCountConvention { get; set; }
        public DateTime MaturityDate { get; set; }
        public DateTime TradeDate { get; set; }
        public DateTime FinalPaymentDate { get; set; }
        public String Description { get; set; }
        public ProductTypeEnum ProductType { get;set; }

        public double PremiumAmount { get; set; }
        public double PremiumPercentage
        {
            get { return 100*PremiumAmount/UnderlyingPrice; }
        }
        public double Volatility { get; set; }
        public double ImpliedVol { get; set; }

        public IRequestForQuote ParentRequest { get; set; }
        public double ForwardPrice { get; set; }

        public IOptionDetail CloneOptionDetails()
        {
            var clone = new OptionDetailImpl
                {
                    LegId = this.LegId,
                    Quantity = this.Quantity,
                    Strike = this.Strike,
                    RIC = this.RIC,
                    BBG = this.BBG,
                    UnderlyingPrice = this.UnderlyingPrice,
                    Side = this.Side,
                    IsCall = this.IsCall,
                    IsEuropean = this.IsEuropean,
                    Delta = this.Delta,
                    Gamma = this.Gamma,
                    Theta = this.Theta,
                    Rho = this.Rho,
                    Vega = this.Vega,
                    DaysToExpiry = this.DaysToExpiry,
                    YearsToExpiry = this.YearsToExpiry,
                    InterestRate = this.InterestRate,
                    DayCountConvention = this.DayCountConvention,
                    MaturityDate = this.MaturityDate,
                    TradeDate = this.TradeDate,
                    FinalPaymentDate = this.FinalPaymentDate,
                    Description = this.Description,
                    ProductType = this.ProductType,
                    PremiumAmount = this.PremiumAmount,
                    Volatility = this.Volatility,
                    ImpliedVol = this.ImpliedVol,
                    ForwardPrice = this.ForwardPrice,
                    ParentRequest = this.ParentRequest                    
                };

            return clone;
        }
        
        public override string ToString()
        {
            var builder = new StringBuilder("RIC: ");
            builder.Append(this.RIC);
            builder.Append(", BBG");
            builder.Append(this.BBG);
            builder.Append(", Description");
            builder.Append(this.Description);
            builder.Append(", Leg");
            builder.Append(this.LegId);
            builder.Append(", Strike: ");
            builder.Append(this.Strike);
            builder.Append(", Strike %: ");
            builder.Append(this.StrikePercentage);
            builder.Append(", Quantity: ");
            builder.Append(this.Quantity);
            builder.Append(", Underlying price: ");
            builder.Append(this.UnderlyingPrice);
            builder.Append(", Side: ");
            builder.Append(this.Side);
            builder.Append(", Premium Amount: ");
            builder.Append(this.PremiumAmount);
            builder.Append(", Premium %: ");
            builder.Append(this.PremiumPercentage);
            builder.Append(", Delta: ");
            builder.Append(this.Delta);
            builder.Append(", Gamma: ");
            builder.Append(this.Gamma);
            builder.Append(", Theta: ");
            builder.Append(this.Theta);
            builder.Append(", Vega: ");
            builder.Append(this.Vega);
            builder.Append(", Rho: ");
            builder.Append(this.Rho);
            builder.Append(", Trade Date: ");
            builder.Append(this.TradeDate);
            builder.Append(", Maturity Date: ");
            builder.Append(this.MaturityDate);
            builder.Append(", Final Payment Date: ");
            builder.Append(this.FinalPaymentDate);
            builder.Append(", Years To Expiry: ");
            builder.Append(this.YearsToExpiry);
            builder.Append(", Days To Expiry: ");
            builder.Append(this.DaysToExpiry);
            builder.Append(", Final Payment Date: ");
            builder.Append(this.FinalPaymentDate);
            builder.Append(", Implied Vol: ");
            builder.Append(this.ImpliedVol);
            builder.Append(", Interest Rate: ");
            builder.Append(this.InterestRate);
            builder.Append(", Volatility: ");
            builder.Append(this.Volatility);
            builder.Append(", Day Count Convention: ");
            builder.Append(this.DayCountConvention);
            builder.Append(", Is Call: ");
            builder.Append(this.IsCall);
            builder.Append(", Is European: ");
            builder.Append(this.IsEuropean);
            return builder.ToString();
        }
    }
}
