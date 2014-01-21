using System;
using System.Runtime.Serialization;
using System.Text;
using RequestForQuoteInterfacesLibrary.Constants;
using RequestForQuoteInterfacesLibrary.Enums;
using RequestForQuoteInterfacesLibrary.ModelInterfaces;

namespace RequestForQuoteInterfacesLibrary.ModelImplementations
{
    [DataContract]
    public sealed class OptionDetailImpl : IOptionDetail
    {
        [DataMember] private string ric;
        [DataMember] private double underlyingPrice;
        [DataMember] private double strike;
        [DataMember] private int quantity;
        [DataMember] private double delta;
        [DataMember] private double gamma;
        [DataMember] private double vega;
        [DataMember] private double theta;
        [DataMember] private double rho;
        [DataMember] private double interestRate;
        [DataMember] private double daysToExpiry;
        [DataMember] private double dayCountConvention;
        [DataMember] private double volatility;
        [DataMember] private double impliedVol;
        [DataMember] private double premiumAmount;
        [DataMember] private string description;
        [DataMember] private string tradeDate;
        [DataMember] private string maturityDate;
        [DataMember] private string side;
        [DataMember] private string productType;
        [DataMember] private int legId;
        [DataMember] private bool isCall;
        [DataMember] private bool isEuropean;

        public static double DAY_COUNT_CONVENTION_255 = 255;
        public static double DAY_COUNT_CONVENTION_250 = 250;
        public static double DAY_COUNT_CONVENTION_265 = 265;
        
        public IRequestForQuote ParentRequest { get; set; }

        public ProductTypeEnum ProductType
        {
            get
            {
                return productType != null
                           ? (ProductTypeEnum)Enum.Parse(typeof(ProductTypeEnum), productType)
                           : ProductTypeEnum.STOCK;
            }
            set
            {
                if (productType != value.ToString())
                {
                    productType = value.ToString();
                }
            }
        }

        public SideEnum Side
        {
            get
            {
                return side != null
                           ? (SideEnum)Enum.Parse(typeof(SideEnum), side)
                           : SideEnum.BUY;
            }
            set
            {
                if (side != value.ToString())
                {
                    side = value.ToString();
                }
            }
        }

        public DateTime MaturityDate
        {
            get
            {
                return DateTime.Parse(maturityDate);
            }
            set
            {
                if (maturityDate != value.ToShortDateString())
                {
                    maturityDate = value.ToShortDateString();
                }
            }
        }

        public DateTime TradeDate
        {
            get
            {
                return DateTime.Parse(tradeDate);
            }
            set
            {
                if (tradeDate != value.ToShortDateString())
                {
                    tradeDate = value.ToShortDateString();
                }
            }
        }

        public bool IsCall
        {
            get
            {
                return isCall;
            }
            set
            {
                isCall = value;
            }
        }

        public bool IsEuropean
        {
            get
            {
                return isEuropean;
            }
            set
            {
                isEuropean = value;
            }
        }

        public string Description
        {
            get
            {
                return description;
            }
            set
            {                
                description = value;
            }
        }

        public int LegId
        {
            get
            {
                return legId;
            }
            set
            {
                legId = value;
            }
        }

        public double PremiumPercentage
        {
            get { return 100 * PremiumAmount / UnderlyingPrice; }
        }

        public double StrikePercentage 
        {
            get { return 100*Strike/UnderlyingPrice; }
        }

        public double InterestRate
        {
            get
            {
                return interestRate;
            }
            set
            {
                if (Math.Abs(interestRate - value) > RequestForQuoteConstants.EPSILON)
                    interestRate = value;
            }
        }

        public double PremiumAmount
        {
            get
            {
                return premiumAmount;
            }
            set
            {
                if (Math.Abs(premiumAmount - value) > RequestForQuoteConstants.EPSILON)
                    premiumAmount = value;
            }
        }

        public double Volatility
        {
            get
            {
                return volatility;
            }
            set
            {
                if (Math.Abs(volatility - value) > RequestForQuoteConstants.EPSILON)
                    volatility = value;
            }
        }

        public double ImpliedVol
        {
            get
            {
                return impliedVol;
            }
            set
            {
                if (Math.Abs(impliedVol - value) > RequestForQuoteConstants.EPSILON)
                    impliedVol = value;
            }
        }

        public double DaysToExpiry
        {
            get
            {
                return daysToExpiry;
            }
            set
            {
                if (Math.Abs(daysToExpiry - value) > RequestForQuoteConstants.EPSILON)
                    daysToExpiry = value;
            }
        }

        public double YearsToExpiry
        {
            get
            {
                // if day count convention is less than actual days then default to ONE.
                if (daysToExpiry > dayCountConvention)
                    return 1.0;
                return daysToExpiry/dayCountConvention;
            }
        }

        public double DayCountConvention
        {
            get
            {
                return dayCountConvention;
            }
            set
            {
                if (Math.Abs(dayCountConvention - value) > RequestForQuoteConstants.EPSILON)
                    dayCountConvention = value;
            }
        }
        
        public int Quantity
        {
            get
            {
                return (Side == SideEnum.SELL && quantity > 0) ? -1 * quantity : quantity;
            }
            set
            {
                quantity = value;
            }
        }
        
        public string RIC { 
            get
            {
                return ric;
            }
            set
            {
                if (ric != value)
                    ric = value;
            }
        }

        public double UnderlyingPrice
        {
            get
            {
                return underlyingPrice;
            }
            set
            {                
                if (Math.Abs(underlyingPrice - value) > RequestForQuoteConstants.EPSILON)
                underlyingPrice = value;
            } 
        }

        public double Strike
        {
            get
            {
                return strike;
            }
            set
            {
                if (Math.Abs(strike - value) > RequestForQuoteConstants.EPSILON)
                    strike = value;
            }
        }

        public double Delta
        {
            get
            {
                return delta;
            }
            set
            {
                if (Math.Abs(delta - value) > RequestForQuoteConstants.EPSILON)
                    delta = value;
            }
        }

        public double Gamma
        {
            get
            {
                return gamma;
            }
            set
            {
                if (Math.Abs(gamma - value) > RequestForQuoteConstants.EPSILON)
                    gamma = value;
            }
        }

        public double Vega
        {
            get
            {
                return vega;
            }
            set
            {
                if (Math.Abs(vega - value) > RequestForQuoteConstants.EPSILON)
                    vega = value;
            }
        }

        public double Theta
        {
            get
            {
                return theta;
            }
            set
            {
                if (Math.Abs(theta - value) > RequestForQuoteConstants.EPSILON)
                    theta = value;
            }
        }

        public double Rho
        {
            get
            {
                return rho;
            }
            set
            {
                if (Math.Abs(rho - value) > RequestForQuoteConstants.EPSILON)
                    rho = value;
            }
        }

        public OptionDetailImpl CloneOptionDetails()
        {
            var clone = new OptionDetailImpl
                {
                    LegId = this.LegId,
                    Quantity = this.Quantity,
                    Strike = this.Strike,
                    RIC = this.RIC,
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
                    InterestRate = this.InterestRate,
                    DayCountConvention = this.DayCountConvention,
                    MaturityDate = this.MaturityDate,
                    TradeDate = this.TradeDate,
                    Description = this.Description,
                    ProductType = this.ProductType,
                    PremiumAmount = this.PremiumAmount,
                    Volatility = this.Volatility,
                    ImpliedVol = this.ImpliedVol,
                    ParentRequest = this.ParentRequest                    
                };

            return clone;
        }
        
        public override string ToString()
        {
            var builder = new StringBuilder("RIC: ");
            builder.Append(this.RIC);
            builder.Append(", Description: ");
            builder.Append(this.Description);
            builder.Append(", Leg: ");
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
            builder.Append(", Years To Expiry: ");
            builder.Append(this.YearsToExpiry);
            builder.Append(", Days To Expiry: ");
            builder.Append(this.DaysToExpiry);
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
