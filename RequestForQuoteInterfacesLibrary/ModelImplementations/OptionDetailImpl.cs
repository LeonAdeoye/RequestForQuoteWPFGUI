﻿using System;
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
        public decimal StrikePercentage 
        {
            get { return Strike/UnderlyingPrice; }
        }
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
        public decimal YearsToExpiry { get; set; }
        public decimal DayCountConvention { get; set; }
        public DateTime MaturityDate { get; set; }
        public DateTime TradeDate { get; set; }
        public DateTime FinalPaymentDate { get; set; }
        public String Description { get; set; }
        public ProductTypeEnum ProductType { get;set; }

        public decimal PremiumAmount { get; set; }
        public decimal PremiumPercentage
        {
            get { return PremiumAmount/UnderlyingPrice; }
        }
        public decimal Weight { get; set; }
        public decimal Volatility { get; set; }
        public decimal ImpliedVol { get; set; }

        public IRequestForQuote ParentRequest { get; set; }
        public decimal ForwardPrice { get; set; }
        
        public override string ToString()
        {
            StringBuilder builder = new StringBuilder("RIC: ");
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
            builder.Append(", Premium Percentage: ");
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
            builder.Append(", Weight: ");
            builder.Append(this.Weight);
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
