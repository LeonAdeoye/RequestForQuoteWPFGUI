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
        
        decimal Delta { get; set; }
        decimal Gamma { get; set; }
        decimal Theta { get; set; }
        decimal Vega { get; set; }
        decimal Rho { get; set; }

        decimal DeltaNotional { get; }
        decimal GammaNotional { get; }
        decimal ThetaNotional { get; }
        decimal VegaNotional { get; }
        decimal RhoNotional { get; }

        decimal DeltaShares { get; }
        decimal GammaShares { get; }
        decimal ThetaShares { get; }
        decimal VegaShares { get; }
        decimal RhoShares { get; }
        
        IWindowPopup Popup { get; set; }
        DateTime TradeDate { get; set; }
        DateTime ExpiryDate { get; set; }
        string BookCode { get; set; }

        CurrencyEnum PremiumSettlementCurrency { get; set; }
        DateTime PremiumSettlementDate { get; set; }
        int PremiumSettlementDaysOverride { get; set; }
        decimal PremiumSettlementFXRate { get; set; }
        
        decimal SalesCreditAmount { get; set; }
        decimal SalesCreditPercentage { get; set; }
        decimal SalesCreditFXRate { get; set; }
        CurrencyEnum SalesCreditCurrency { get; set; }
                
        int Multiplier { get; set; }
        int LotSize { get; set; }
        decimal Contracts { get; set; }
        decimal Quantity { get; }

        decimal NotionalMillions { get; set; }
        CurrencyEnum NotionalCurrency { get; set; }
        decimal NotionalFXRate { get; set; }

        decimal BidImpliedVol { get; set; }
        decimal BidPremiumPercentage { get; set; }
        decimal BidPremiumAmount { get; set; }
        decimal BidFinalAmount { get; set; }
        decimal BidFinalPercentage { get; set; }

        decimal ImpliedVol { get; set; }
        decimal PremiumAmount { get; set; }
        decimal PremiumPercentage { get; set; }

        decimal AskImpliedVol { get; set; }
        decimal AskPremiumPercentage { get; set; }
        decimal AskPremiumAmount { get; set; }
        decimal AskFinalAmount { get; set; }
        decimal AskFinalPercentage { get; set; }

        string SalesComment { get; set; }
        string TraderComment { get; set; }
        string ClientComment { get; set; }
        string PickedUpBy { get; set; }

        HedgeTypeEnum HedgeType { get; set; }
        decimal HedgePrice { get; set; }
        decimal TotalPremium { get; }

        List<IOptionDetail> Legs { get; set; }
        List<ChatMessageImpl> Messages { get; set; }
        IEditableObject EditableViewModel { get; set; }

        IRequestForQuote Clone(int nextIdentifier);
        bool CalculatePricing(IOptionRequestPricer optionPricer);
        void ClearChatMessages();
        void AddChatMessage(ChatMessageImpl message);

        void CopyMembers(IRequestForQuote fromSourceRequest);
        void Save();
    }
}
