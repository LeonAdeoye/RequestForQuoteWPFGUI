using System;
using System.Collections.Generic;
using System.ComponentModel;
using RequestForQuoteInterfacesLibrary.Enums;
using RequestForQuoteInterfacesLibrary.ModelImplementations;
using RequestForQuoteInterfacesLibrary.ServiceInterfaces;
using RequestForQuoteInterfacesLibrary.WindowInterfaces;

namespace RequestForQuoteInterfacesLibrary.ModelInterfaces
{
    public interface IRequestForQuote
    {
        string Request { get; set; }
        bool IsOTC { get; set; }
        int Identifier { get; set; }
        StatusEnum Status { get; set; }
        IClient Client { get; set; }       
        
        double Delta { get; set; }
        double Gamma { get; set; }
        double Theta { get; set; }
        double Vega { get; set; }
        double Rho { get; set; }

        double DeltaNotional { get; }
        double GammaNotional { get; }
        double ThetaNotional { get; }
        double VegaNotional { get; }
        double RhoNotional { get; }

        double DeltaShares { get; }
        double GammaShares { get; }
        double ThetaShares { get; }
        double VegaShares { get; }
        double RhoShares { get; }
        
        IWindowPopup Popup { get; set; }
        DateTime TradeDate { get; set; }
        DateTime ExpiryDate { get; set; }
        double DayCountConvention { get; set; }
        string BookCode { get; set; }

        CurrencyEnum PremiumSettlementCurrency { get; set; }
        DateTime PremiumSettlementDate { get; set; }
        int PremiumSettlementDaysOverride { get; set; }
        double PremiumSettlementFXRate { get; set; }
        
        double SalesCreditAmount { get; set; }
        double SalesCreditPercentage { get; set; }
        double SalesCreditFXRate { get; set; }
        CurrencyEnum SalesCreditCurrency { get; set; }
                
        int Multiplier { get; set; }
        int LotSize { get; set; }
        int Contracts { get; set; }
        int Quantity { get; }

        double NotionalMillions { get; set; }
        CurrencyEnum NotionalCurrency { get; set; }
        double NotionalFXRate { get; set; }

        double BidImpliedVol { get; set; }
        double BidPremiumPercentage { get; set; }
        double BidPremiumAmount { get; set; }
        double BidFinalAmount { get; set; }
        double BidFinalPercentage { get; set; }

        double ImpliedVol { get; set; }
        double PremiumAmount { get; set; }
        double PremiumPercentage { get; set; }

        double AskImpliedVol { get; set; }
        double AskPremiumPercentage { get; set; }
        double AskPremiumAmount { get; set; }
        double AskFinalAmount { get; set; }
        double AskFinalPercentage { get; set; }

        string SalesComment { get; set; }
        string TraderComment { get; set; }
        string ClientComment { get; set; }
        string PickedUpBy { get; set; }

        HedgeTypeEnum HedgeType { get; set; }
        double HedgePrice { get; set; }
        double TotalPremium { get; }

        List<IOptionDetail> Legs { get; set; }
        List<ChatMessageImpl> Messages { get; set; }
        IEditableObject EditableViewModel { get; set; }

        IRequestForQuote Clone(int nextIdentifier);
        bool CalculatePricing(IOptionRequestPricer optionPricer);
        void ClearChatMessages();
        void AddChatMessage(ChatMessageImpl message);

        void CopyMembers(IRequestForQuote fromSourceRequest);
    }
}
