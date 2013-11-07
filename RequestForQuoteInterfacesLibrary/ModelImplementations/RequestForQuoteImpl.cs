using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.ComponentModel;
using RequestForQuoteInterfacesLibrary.Enums;
using RequestForQuoteInterfacesLibrary.ModelInterfaces;
using RequestForQuoteInterfacesLibrary.ServiceInterfaces;
using RequestForQuoteInterfacesLibrary.WindowInterfaces;

namespace RequestForQuoteInterfacesLibrary.ModelImplementations
{
    [DataContract]
    public sealed class RequestForQuoteImpl : IRequestForQuote, INotifyPropertyChanged
    {
        [DataMember] private bool isOTC;
        [DataMember] private int multiplier;
        [DataMember] private int contracts;
        [DataMember] private int lotSize;
        [DataMember] private double notionalMillions;
        [DataMember] private string notionalCurrency;
        [DataMember] private double notionalFXRate;
        [DataMember] private string premiumSettlementCurrency;
        [DataMember] private int premiumSettlementDaysOverride;
        [DataMember] private string premiumSettlementDate;
        [DataMember] private double premiumSettlementFXRate;
        
        [DataMember] private double salesCreditAmount;
        [DataMember] private double salesCreditPercentage;
        [DataMember] private string salesCreditCurrency;
        [DataMember] private double salesCreditFXRate;

        [DataMember] private double impliedVol;
        [DataMember] private double premiumAmount;
        [DataMember] private double premiumPercentage;

        [DataMember] private string request;
        [DataMember] private int identifier;

        [DataMember] private double gamma;
        [DataMember] private double delta;
        [DataMember] private double vega;
        [DataMember] private double theta;
        [DataMember] private double rho;

        [DataMember] private double hedgePrice;
        [DataMember] private string hedgeType;
        
        [DataMember] private double bidFinalAmount;
        [DataMember] private double bidFinalPercentage;
        [DataMember] private double askFinalAmount;
        [DataMember] private double askFinalPercentage;

        [DataMember] private string tradeDate;
        [DataMember] private string expiryDate;
        [DataMember] private IClient client;  //TODO
        [DataMember] private string status;
        [DataMember] private string bookCode;

        [DataMember] private double bidImpliedVol;
        [DataMember] private double bidPremiumPercentage;
        [DataMember] private double bidPremiumAmount;
        
        [DataMember] private double askImpliedVol;
        [DataMember] private double askPremiumPercentage;
        [DataMember] private double askPremiumAmount;

        [DataMember] private string salesComment;
        [DataMember] private string traderComment;
        [DataMember] private string clientComment;

        [DataMember] private string pickedUpBy;

        public List<IOptionDetail> Legs { get; set; }
        public List<ChatMessageImpl> Messages { get; set; }
        public IEditableObject EditableViewModel { get; set; }

        public void CopyMembers(IRequestForQuote fromSourceRequest)
        {
            Status = fromSourceRequest.Status;
            Client = fromSourceRequest.Client;
            TradeDate = fromSourceRequest.TradeDate;
            ExpiryDate = fromSourceRequest.ExpiryDate;
            Request = fromSourceRequest.Request;
            IsOTC = fromSourceRequest.IsOTC;

            Delta = fromSourceRequest.Delta;
            Gamma = fromSourceRequest.Gamma;
            Vega = fromSourceRequest.Vega;
            Theta = fromSourceRequest.Theta;
            Rho = fromSourceRequest.Rho;

            ImpliedVol = fromSourceRequest.ImpliedVol;
            PremiumAmount = fromSourceRequest.PremiumAmount;
            PremiumPercentage = fromSourceRequest.PremiumPercentage;

            Identifier = fromSourceRequest.Identifier;
            PickedUpBy = fromSourceRequest.PickedUpBy;
            BookCode = fromSourceRequest.BookCode;

            PremiumSettlementCurrency = fromSourceRequest.PremiumSettlementCurrency;
            PremiumSettlementDate = fromSourceRequest.PremiumSettlementDate;
            PremiumSettlementDaysOverride = fromSourceRequest.PremiumSettlementDaysOverride;
            PremiumSettlementFXRate = fromSourceRequest.PremiumSettlementFXRate;

            SalesCreditAmount = fromSourceRequest.SalesCreditAmount;
            SalesCreditPercentage = fromSourceRequest.SalesCreditPercentage;
            SalesCreditCurrency = fromSourceRequest.SalesCreditCurrency;
            SalesCreditFXRate = fromSourceRequest.SalesCreditFXRate;

            Multiplier = fromSourceRequest.Multiplier;
            Contracts = fromSourceRequest.Contracts;
            LotSize = fromSourceRequest.LotSize;

            NotionalMillions = fromSourceRequest.NotionalMillions;
            NotionalCurrency = fromSourceRequest.NotionalCurrency;
            NotionalFXRate = fromSourceRequest.NotionalFXRate;

            BidImpliedVol = fromSourceRequest.BidImpliedVol;
            BidPremiumPercentage = fromSourceRequest.BidPremiumPercentage;
            BidPremiumAmount = fromSourceRequest.BidPremiumAmount;
            BidFinalPercentage = fromSourceRequest.BidFinalPercentage;
            BidFinalAmount = fromSourceRequest.BidFinalAmount;

            AskImpliedVol = fromSourceRequest.AskImpliedVol;
            AskPremiumPercentage = fromSourceRequest.AskPremiumPercentage;
            AskPremiumAmount = fromSourceRequest.AskPremiumAmount;
            AskFinalPercentage = fromSourceRequest.AskFinalPercentage;
            AskFinalAmount = fromSourceRequest.AskFinalAmount;

            SalesComment = fromSourceRequest.SalesComment;
            TraderComment = fromSourceRequest.TraderComment;
            ClientComment = fromSourceRequest.ClientComment;

            HedgePrice = fromSourceRequest.HedgePrice;
            HedgeType = fromSourceRequest.HedgeType;

            if (fromSourceRequest.Legs != null)
            {
                Legs = new List<IOptionDetail>();
                foreach (var leg in fromSourceRequest.Legs)
                    Legs.Add(leg.CloneOptionDetails());
            }

            // TODO verify
            if (Messages != null)
                Messages = new List<ChatMessageImpl>(fromSourceRequest.Messages);
        }

        public IRequestForQuote Clone(int nextIdentifier)
        {
            IRequestForQuote clone = new RequestForQuoteImpl();
            clone.Status = (StatusEnum)Enum.Parse(typeof(StatusEnum), status);
            clone.Client = client;
            clone.TradeDate = DateTime.Parse(tradeDate);
            clone.ExpiryDate = DateTime.Parse(expiryDate);
            clone.Request = request;
            clone.IsOTC = isOTC;

            clone.Delta = delta;
            clone.Gamma = gamma;
            clone.Vega = vega;
            clone.Theta = theta;
            clone.Rho = rho;

            clone.ImpliedVol = impliedVol;
            clone.PremiumAmount = premiumAmount;
            clone.PremiumPercentage = PremiumPercentage;

            clone.Identifier = nextIdentifier;
            clone.PickedUpBy = pickedUpBy;
            clone.BookCode = bookCode;

            if(premiumSettlementCurrency != null)
                clone.PremiumSettlementCurrency = (CurrencyEnum)Enum.Parse(typeof(CurrencyEnum), premiumSettlementCurrency);
            if (premiumSettlementDate != null)
                clone.PremiumSettlementDate = DateTime.Parse(premiumSettlementDate);
            clone.PremiumSettlementDaysOverride = premiumSettlementDaysOverride;
            clone.PremiumSettlementFXRate = premiumSettlementFXRate;

            clone.SalesCreditAmount = salesCreditAmount;
            clone.SalesCreditPercentage = salesCreditPercentage;
            if (salesCreditCurrency != null)
                clone.SalesCreditCurrency = (CurrencyEnum)Enum.Parse(typeof(CurrencyEnum), salesCreditCurrency);
            clone.SalesCreditFXRate = salesCreditFXRate;
            
            clone.Multiplier = multiplier;
            clone.Contracts = contracts;
            clone.LotSize = lotSize;
            
            clone.NotionalMillions = notionalMillions;
            if (notionalCurrency != null)
            clone.NotionalCurrency = (CurrencyEnum)Enum.Parse(typeof(CurrencyEnum), notionalCurrency);
            clone.NotionalFXRate = notionalFXRate;
            
            clone.BidImpliedVol = bidImpliedVol;
            clone.BidPremiumPercentage = bidPremiumPercentage;
            clone.BidPremiumAmount = bidPremiumAmount;
            clone.BidFinalPercentage = bidFinalPercentage;
            clone.BidFinalAmount = bidFinalAmount;

            clone.AskImpliedVol = askImpliedVol;
            clone.AskPremiumPercentage = askPremiumPercentage;
            clone.AskPremiumAmount = askPremiumAmount;
            clone.AskFinalPercentage = askFinalPercentage;
            clone.AskFinalAmount = askFinalAmount;

            clone.SalesComment = salesComment;
            clone.TraderComment = traderComment;
            clone.ClientComment = clientComment;

            clone.HedgePrice = hedgePrice;
            if (hedgeType != null)
                clone.HedgeType = (HedgeTypeEnum)Enum.Parse(typeof(HedgeTypeEnum), hedgeType);

            if (Legs != null)
            {
                clone.Legs = new List<IOptionDetail>();
                foreach (var leg in Legs)
                    clone.Legs.Add(leg.CloneOptionDetails());
            }

            // TODO clone
            if (Messages != null)
                clone.Messages = new List<ChatMessageImpl>(Messages);
                            
            return clone;
        }

        private void ResetPricing()
        {
            Delta = 0;
            Gamma = 0;
            Vega = 0;
            Theta = 0;
            Rho = 0;
            PremiumAmount = 0;
            premiumPercentage = 0;
        }

        public bool CalculatePricing(IOptionRequestPricer optionPricer)
        {
            ResetPricing();
            return Legs != null && Legs.All(optionPricer.CalculatePricing);
        }

        public override string ToString()
        {
            var builder = new StringBuilder("identifier: ");
            builder.Append(identifier);
            builder.Append(", request: ");
            builder.Append(request);
            builder.Append(", is OTC: ");
            builder.Append(isOTC);

            builder.Append(", contracts: ");
            builder.Append(contracts);
            builder.Append(", multiplier: ");
            builder.Append(multiplier);
            builder.Append(", lot size: ");
            builder.Append(lotSize);
            builder.Append(", quantity: ");
            builder.Append(Quantity);

            builder.Append(", implied vol: ");
            builder.Append(impliedVol);
            builder.Append(", premium absolute: ");
            builder.Append(premiumAmount);
            builder.Append(", premium percentage: ");
            builder.Append(premiumPercentage);

            builder.Append(", ask premium absolute: ");
            builder.Append(askPremiumAmount);
            builder.Append(", ask premium percentage: ");
            builder.Append(askPremiumPercentage);
            builder.Append(", ask implied Vol: ");
            builder.Append(askImpliedVol);
            builder.Append(", ask final absolute: ");
            builder.Append(askFinalAmount);
            builder.Append(", ask final percentage: ");
            builder.Append(askFinalPercentage);

            builder.Append(", bid premium absolute: ");
            builder.Append(bidPremiumAmount);
            builder.Append(", bid premium percentage: ");
            builder.Append(bidPremiumPercentage);
            builder.Append(", bid implied Vol: ");
            builder.Append(bidImpliedVol);
            builder.Append(", bid final absolute: ");
            builder.Append(bidFinalAmount);
            builder.Append(", bid final percentage: ");
            builder.Append(bidFinalPercentage);

            builder.Append(", gamma: ");
            builder.Append(gamma);
            builder.Append(", delta: ");
            builder.Append(delta);
            builder.Append(", vega: ");
            builder.Append(vega);
            builder.Append(", trade date: ");
            builder.Append(tradeDate);
            builder.Append(", expiry date: ");
            builder.Append(expiryDate);
            builder.Append(", theta: ");
            builder.Append(theta);
            builder.Append(", rho: ");
            builder.Append(rho);

            builder.Append(", client: ");
            builder.Append(client);
            builder.Append(", status: ");
            builder.Append(status);
            builder.Append(", book code: ");
            builder.Append(bookCode);

            builder.Append(", sales comment: ");
            builder.Append(salesComment);
            builder.Append(", trader comment: ");
            builder.Append(traderComment);
            builder.Append(", client comment: ");
            builder.Append(clientComment);            

            builder.Append(", premium settlement date: ");
            builder.Append(premiumSettlementDate);
            builder.Append(", premium settlement days override: ");
            builder.Append(premiumSettlementDaysOverride);
            builder.Append(", premium settlement fx rate: ");
            builder.Append(premiumSettlementFXRate);
            builder.Append(", premium settlement currency: ");
            builder.Append(premiumSettlementCurrency);

            builder.Append(", picked up by: ");
            builder.Append(pickedUpBy);
            
            builder.Append(", sales credit amount: ");
            builder.Append(salesCreditAmount);
            builder.Append(", sales credit percentage: ");
            builder.Append(salesCreditPercentage);
            builder.Append(", sales credit currency: ");
            builder.Append(salesCreditCurrency);
            builder.Append(", sales credit FX rate: ");
            builder.Append(salesCreditFXRate);

            builder.Append(", notional millions: ");
            builder.Append(notionalMillions);
            builder.Append(", notional currency: ");
            builder.Append(notionalCurrency);
            builder.Append(", notional FX rate: ");
            builder.Append(notionalFXRate);

            builder.Append(", delta notional: ");
            builder.Append(DeltaNotional);
            builder.Append(", gamma notional: ");
            builder.Append(GammaNotional);
            builder.Append(", vega notional: ");
            builder.Append(VegaNotional);
            builder.Append(", theta notional: ");
            builder.Append(ThetaNotional);
            builder.Append(", rho notional: ");
            builder.Append(RhoNotional);

            builder.Append(", delta shares: ");
            builder.Append(DeltaShares);
            builder.Append(", gamma shares: ");
            builder.Append(GammaShares);
            builder.Append(", vega shares: ");
            builder.Append(VegaShares);
            builder.Append(", theta shares: ");
            builder.Append(ThetaShares);
            builder.Append(", rho shares: ");
            builder.Append(RhoShares);

            builder.Append(", hedge price: ");
            builder.Append(hedgePrice);
            builder.Append(", hedge type: ");
            builder.Append(hedgeType);

            // TODO ToString() for MessageImpl then add this
            return builder.ToString();
        }

        #region ALL_PROPERTIES

        public IWindowPopup Popup { get; set; }

        public string Request
        {
            get
            {
                return request;
            }
            set
            {
                if (request != value)
                {
                    request = value;
                    NotifyPropertyChanged("Request");                    
                }
            }
        }

        public bool IsOTC
        {
            get
            {
                return isOTC;
            }
            set
            {
                if (isOTC != value)
                {
                    isOTC = value;
                    NotifyPropertyChanged("IsOTC");
                }
            }
        }

        public int Identifier
        {
            get
            {
                return identifier;
            }
            set
            {
                if (identifier != value)
                {
                    identifier = value;
                    NotifyPropertyChanged("Identifier");
                }
            }
        }

        public StatusEnum Status
        {
            get { return (StatusEnum)Enum.Parse(typeof(StatusEnum), status); }
            set
            {
                if (status != value.ToString())
                {
                    status = value.ToString();
                    NotifyPropertyChanged("Status");
                }
            }
        }

        // TODO convert to integer
        public IClient Client
        {
            get
            {
                return client;
            }
            set
            {
                if (client != value)
                {
                    client = value;
                    NotifyPropertyChanged("Client");
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
                    NotifyPropertyChanged("TradeDate");
                }
            }
        }

        public DateTime ExpiryDate
        {
            get
            {
                return DateTime.Parse(expiryDate);
            }
            set
            {
                if (expiryDate != value.ToShortDateString())
                {
                    expiryDate = value.ToShortDateString();
                    NotifyPropertyChanged("ExpiryDate");
                }
            }
        }

        public string BookCode
        {
            get
            {
                return bookCode;
            }
            set
            {
                if (bookCode != value)
                {
                    bookCode = value;
                    NotifyPropertyChanged("BookCode");                    
                }
            }
        }

        public CurrencyEnum PremiumSettlementCurrency
        {
            get
            {
                return premiumSettlementCurrency != null
                    ? (CurrencyEnum)Enum.Parse(typeof(CurrencyEnum), premiumSettlementCurrency)
                    : CurrencyEnum.USD;
            }
            set
            {
                if (premiumSettlementCurrency != value.ToString())
                {
                    premiumSettlementCurrency = value.ToString();
                    NotifyPropertyChanged("PremiumSettlementCurrency");                    
                }
            }
        }

        public DateTime PremiumSettlementDate
        {
            get 
            {
                return premiumSettlementDate != null 
                ? DateTime.Parse(premiumSettlementDate) 
                : new DateTime(); 
            }
            set
            {
                if (premiumSettlementDate != value.ToShortDateString())
                {
                    premiumSettlementDate = value.ToShortDateString();
                    NotifyPropertyChanged("PremiumSettlementDate");                    
                }
            }
        }

        public int PremiumSettlementDaysOverride
        {
            get
            {
                return premiumSettlementDaysOverride;
            }
            set
            {
                if(premiumSettlementDaysOverride != value)
                {
                    premiumSettlementDaysOverride = value; 
                    NotifyPropertyChanged("PremiumSettlementDaysOverride");
                }
            }
        }

        public double PremiumSettlementFXRate
        {
            get
            {
                return premiumSettlementFXRate;
            }
            set
            {
                if (premiumSettlementFXRate != value)
                {
                    premiumSettlementFXRate = value;
                    NotifyPropertyChanged("PremiumSettlementFXRate");
                }
            }
        }

        public double SalesCreditAmount
        {
            get
            {
                return salesCreditAmount;
            }
            set
            {
                if (salesCreditAmount != value)
                {
                    salesCreditAmount = value;
                    NotifyPropertyChanged("SalesCreditAmount");                    
                }
            }
        }

        public double SalesCreditPercentage
        {
            get
            {
                return salesCreditPercentage;
            }
            set
            {
                if (salesCreditPercentage != value)
                {
                    salesCreditPercentage = value;
                    NotifyPropertyChanged("SalesCreditPercentage");                    
                }
            }
        }

        public CurrencyEnum SalesCreditCurrency
        {
            get
            {
                return salesCreditCurrency != null 
                    ? (CurrencyEnum)Enum.Parse(typeof(CurrencyEnum), salesCreditCurrency) 
                    : CurrencyEnum.USD;
            }
            set
            {
                if (salesCreditCurrency != value.ToString())
                {
                    salesCreditCurrency = value.ToString(); 
                    NotifyPropertyChanged("SalesCreditCurrency");                    
                }
            }
        }

        public double SalesCreditFXRate
        {
            get
            {
                return salesCreditFXRate;
            }
            set
            {
                if (salesCreditFXRate != value)
                {
                    salesCreditFXRate = value;
                    NotifyPropertyChanged("SalesCreditFXRate");
                }
            }
        }

        public int Multiplier
        {
            get
            {
                return multiplier;
            }
            set
            {
                if (multiplier != value)
                {
                    multiplier = value;
                    NotifyPropertyChanged("Multiplier");
                    NotifyPropertyChanged("Quantity");
                    NotifyPropertyChanged("TotalPremium");
                    NotifyPropertyChanged("DeltaShares");
                    NotifyPropertyChanged("GammaShares");
                    NotifyPropertyChanged("VegaShares");
                    NotifyPropertyChanged("ThetaShares");
                    NotifyPropertyChanged("RhoShares");
                }
            }
        }

        public int LotSize
        {
            get
            {
                return lotSize;
            }
            set
            {
                if (lotSize != value)
                {
                    lotSize = value;
                    NotifyPropertyChanged("LotSize");
                    NotifyPropertyChanged("DeltaShares");
                    NotifyPropertyChanged("GammaShares");
                    NotifyPropertyChanged("VegaShares");
                    NotifyPropertyChanged("ThetaShares");
                    NotifyPropertyChanged("RhoShares");
                }
            }
        }

        public int Contracts
        {
            get
            {
                return contracts;
            }
            set
            {
                if (contracts != value)
                {
                    contracts = value; 
                    NotifyPropertyChanged("Contracts");
                    NotifyPropertyChanged("Quantity");
                    NotifyPropertyChanged("TotalPremium");
                    NotifyPropertyChanged("DeltaShares");
                    NotifyPropertyChanged("GammaShares");
                    NotifyPropertyChanged("VegaShares");
                    NotifyPropertyChanged("ThetaShares");
                    NotifyPropertyChanged("RhoShares");                    
                }
            }
        }

        public int Quantity
        {
            get
            {
                return contracts * multiplier;
            }
        }

        public double NotionalMillions
        {
            get
            {
                return notionalMillions;
            }
            set
            {
                if (notionalMillions != value)
                {
                    notionalMillions = value;
                    NotifyPropertyChanged("NotionalMillions");
                    
                    NotifyPropertyChanged("DeltaNotional");
                    NotifyPropertyChanged("GammaNotional");
                    NotifyPropertyChanged("ThetaNotional");
                    NotifyPropertyChanged("VegaNotional");
                    NotifyPropertyChanged("RhoNotional");

                    NotifyPropertyChanged("DeltaShares");
                    NotifyPropertyChanged("GammaShares");
                    NotifyPropertyChanged("VegaShares");
                    NotifyPropertyChanged("ThetaShares");
                    NotifyPropertyChanged("RhoShares");
                }
            }
        }

        public double NotionalFXRate
        {
            get
            {
                return notionalFXRate;
            }
            set
            {
                if (notionalFXRate != value)
                {
                    notionalFXRate = value;
                    NotifyPropertyChanged("NotionalFXRate");                    
                }
            }
        }

        public CurrencyEnum NotionalCurrency
        {
            get
            {
                return notionalCurrency != null
                           ? (CurrencyEnum) Enum.Parse(typeof (CurrencyEnum), notionalCurrency)
                           : CurrencyEnum.USD;
            }
            set
            {
                if (notionalCurrency != value.ToString())
                {
                    notionalCurrency = value.ToString();
                    NotifyPropertyChanged("NotionalCurrency");
                }
            }
        }

        public double BidImpliedVol
        {
            get
            {
                return bidImpliedVol;
            }
            set
            {
                if (bidImpliedVol != value)
                {
                    bidImpliedVol = value;
                    NotifyPropertyChanged("BidImpliedVol");                    
                }
            }
        }

        public double BidPremiumAmount
        {
            get
            {
                return bidPremiumAmount;
            }
            set
            {
                if (bidPremiumAmount != value)
                {
                    bidPremiumAmount = value; 
                    NotifyPropertyChanged("BidPremiumAbsolute");    
                }                
            }
        }

        public double BidPremiumPercentage
        {
            get
            {
                return bidPremiumPercentage;
            }
            set
            {
                if (bidPremiumPercentage != value)
                {
                    bidPremiumPercentage = value; 
                    NotifyPropertyChanged("BidPremiumPercentage");    
                }                
            }
        }

        public double BidFinalPercentage
        {
            get
            {
                return bidFinalPercentage;
            }
            set
            {
                if (bidFinalPercentage != value)
                {
                    bidFinalPercentage = value;
                    NotifyPropertyChanged("BidFinalPercentage");
                }
            }
        }

        public double BidFinalAmount
        {
            get
            {
                return bidFinalAmount;
            }
            set
            {
                if (bidFinalAmount != value)
                {
                    bidFinalAmount = value;
                    NotifyPropertyChanged("BidFinalAbsolute");
                }
            }
        }

        public double AskImpliedVol
        {
            get
            {
                return askImpliedVol;
            }
            set
            {
                if (askImpliedVol != value)
                {
                    askImpliedVol = value; 
                    NotifyPropertyChanged("AskImpliedVol");    
                }                
            }
        }

        public double AskPremiumAmount
        {
            get
            {
                return askPremiumAmount;
            }
            set
            {
                if (askPremiumAmount != value)
                {
                    askPremiumAmount = value; 
                    NotifyPropertyChanged("AskPremiumAbsolute");    
                }                
            }
        }

        public double AskPremiumPercentage
        {
            get
            {
                return askPremiumPercentage;
            }
            set
            {
                if (askPremiumPercentage != value)
                {
                    askPremiumPercentage = value; 
                    NotifyPropertyChanged("AskPremiumPercentage");    
                }                
            }
        }


        public double AskFinalPercentage
        {
            get
            {
                return askFinalPercentage;
            }
            set
            {
                if (askFinalPercentage != value)
                {
                    askFinalPercentage = value;
                    NotifyPropertyChanged("AskFinalPercentage");
                }
            }
        }

        public double AskFinalAmount
        {
            get
            {
                return askFinalAmount;
            }
            set
            {
                if (askFinalAmount != value)
                {
                    askFinalAmount = value;
                    NotifyPropertyChanged("AskFinalAbsolute");
                }
            }
        }

        public string SalesComment
        {
            get
            {
                return salesComment;
            }
            set
            {
                if (salesComment != value)
                {
                    salesComment = value;
                    NotifyPropertyChanged("SalesComment");                    
                }
            }
        }

        public string TraderComment
        {
            get
            {
                return traderComment;
            }
            set
            {
                if (traderComment != value)
                {
                    traderComment = value;
                    NotifyPropertyChanged("TraderComment");                    
                }
            }
        }

        public string ClientComment
        {
            get
            {
                return clientComment;
            }
            set
            {
                if (clientComment != value)
                {
                    clientComment = value;
                    NotifyPropertyChanged("ClientComment");
                }
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
                if (premiumAmount != value)
                {
                    premiumAmount = value;
                    NotifyPropertyChanged("PremiumAbsolute");
                    NotifyPropertyChanged("TotalPremium");
                }
            }
        }

        public double PremiumPercentage
        {
            get
            {
                return premiumPercentage;
            }
            set
            {
                if (premiumPercentage != value)
                {
                    premiumPercentage = value;
                    NotifyPropertyChanged("PremiumPercentage");
                }
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
                if (impliedVol != value)
                {
                    impliedVol = value;
                    NotifyPropertyChanged("ImpliedVol");
                }
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
                if (delta != value)
                {
                    delta = value;
                    NotifyPropertyChanged("Delta");
                    NotifyPropertyChanged("DeltaNotional");
                    NotifyPropertyChanged("DeltaShares");
                }
            }
        }

        public double DeltaNotional
        {
            get
            {
                return delta * notionalMillions * 1000000;
            }
        }

        public double DeltaShares
        {
            get
            {
                if (lotSize == 0)
                    return 0;

                return (Math.Floor(delta * Quantity) / lotSize) * lotSize;
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
                if (gamma != value)
                {
                    gamma = value;
                    NotifyPropertyChanged("Gamma");
                    NotifyPropertyChanged("GammaNotional");
                    NotifyPropertyChanged("GammaShares");  
                }
            }
        }

        public double GammaNotional
        {
            get
            {
                return gamma * notionalMillions * 1000000;
            }
        }

        public double GammaShares
        {
            get
            {
                // TODO verify
                if (lotSize == 0)
                    return 0;
                   
                return (Math.Floor(gamma * Quantity) / lotSize) * lotSize;
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
                if (theta != value)
                {
                    theta = value;
                    NotifyPropertyChanged("Theta");
                    NotifyPropertyChanged("ThetaNotional");
                    NotifyPropertyChanged("ThetaShares"); 
                }
            }
        }

        public double ThetaNotional
        {
            get
            {
                return theta * notionalMillions * 1000000;
            }
        }

        public double ThetaShares
        {
            get
            {
                // TODO verify
                if (lotSize == 0)
                    return 0;

                return (Math.Floor(theta * Quantity) / lotSize) * lotSize;
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
                if (vega != value)
                {
                    vega = value;
                    NotifyPropertyChanged("Vega");
                    NotifyPropertyChanged("VegaNotional");
                    NotifyPropertyChanged("VegaShares");
                }
            }
        }

        public double VegaNotional
        {
            get
            {
                return vega * notionalMillions * 1000000;
            }
        }

        public double VegaShares
        {
            get
            {
                // TODO verify
                if (lotSize == 0)
                {
                    return 0;
                }

                return (Math.Floor(vega * Quantity) / lotSize) * lotSize;
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
                if (rho != value)
                {
                    rho = value;
                    NotifyPropertyChanged("Rho");
                    NotifyPropertyChanged("RhoNotional");
                    NotifyPropertyChanged("RhoShares");
                }
            }
        }

        public double RhoNotional
        {
            get
            {
                return rho * notionalMillions * 1000000;
            }
        }

        public double RhoShares
        {
            get
            {
                // TODO verify
                if (lotSize == 0)
                    return 0;

                return (Math.Floor(rho * Quantity) / lotSize) * lotSize;
            }
        }

        public double HedgePrice
        {
            get
            {
                return hedgePrice;
            }
            set
            {
                if (hedgePrice != value)
                {
                    hedgePrice = value;
                    NotifyPropertyChanged("HedgePrice");
                }
            }
        }

        public HedgeTypeEnum HedgeType
        {
            get
            {
                return hedgeType != null
                           ? (HedgeTypeEnum) Enum.Parse(typeof (HedgeTypeEnum), hedgeType)
                           : HedgeTypeEnum.FUTURES;
            }
            set
            {
                if (hedgeType != value.ToString())
                {
                    hedgeType = value.ToString();
                    NotifyPropertyChanged("HedgeType");
                }
            }
        }

        public double TotalPremium
        {
            get
            {
                return PremiumAmount * Quantity;
            }
        }

        public string PickedUpBy
        {
            get
            {
                return pickedUpBy;
            }
            set
            {
                if (pickedUpBy != value)
                {
                    pickedUpBy = value;
                    NotifyPropertyChanged("PickedUpBy");
                }
            }
        }

        #endregion

        public event PropertyChangedEventHandler PropertyChanged;

        private void NotifyPropertyChanged(string propertyName)
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
        }

        public void ClearChatMessages()
        {
            Messages.Clear();
        }

        public void AddChatMessage(ChatMessageImpl message)
        {
            Messages.Add(message);
        }
    }
}
