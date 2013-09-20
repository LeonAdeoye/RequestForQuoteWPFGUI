using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;
using RequestForQuoteInterfacesLibrary.Enums;
using RequestForQuoteInterfacesLibrary.ModelInterfaces;
using RequestForQuoteInterfacesLibrary.ServiceInterfaces;
using RequestForQuoteInterfacesLibrary.WindowInterfaces;

namespace RequestForQuoteInterfacesLibrary.ModelImplementations
{
    public sealed class RequestForQuoteImpl : IRequestForQuote, INotifyPropertyChanged
    {
        private bool isOTC;
        
        private int multiplier;
        private decimal contracts;
        private int lotSize;

        private decimal notionalMillions;        
        private CurrencyEnum notionalCurrency;
        // TODO need an FX manager - which mocks FX rates using random numbers.
        private decimal notionalFXRate;

        private CurrencyEnum premiumSettlementCurrency; 
        private int premiumSettlementDaysOverride; 
        private DateTime premiumSettlementDate; 
        private decimal premiumSettlementFXRate;
        
        private decimal salesCreditAmount;
        private decimal salesCreditPercentage;
        private CurrencyEnum salesCreditCurrency;
        private decimal salesCreditFXRate;

        private decimal impliedVol;
        private decimal premiumAmount;
        private decimal premiumPercentage;

        private string request;
        private int identifier;

        private decimal gamma;
        private decimal delta;
        private decimal vega;
        private decimal theta;
        private decimal rho;

        private decimal hedgePrice;
        private HedgeTypeEnum hedgeType;
        
        private decimal bidFinalAmount;
        private decimal bidFinalPercentage;
        private decimal askFinalAmount;
        private decimal askFinalPercentage;

        private DateTime tradeDate;
        private DateTime expiryDate;
        private IClient client;
        private StatusEnum status;
        private string bookCode;

        private decimal bidImpliedVol;
        private decimal bidPremiumPercentage;
        private decimal bidPremiumAmount;
        
        private decimal askImpliedVol;
        private decimal askPremiumPercentage;
        private decimal askPremiumAmount;

        private string salesComment;
        private string traderComment;
        private string clientComment;

        private string pickedUpBy;

        public List<IOptionDetail> Legs { get; set; }
        public List<ChatMessageImpl> Messages { get; set; }

        public IRequestForQuote Clone(int nextIdentifier)
        {
            IRequestForQuote clone = new RequestForQuoteImpl();
            clone.Status = status;
            clone.Client = client;
            clone.TradeDate = tradeDate;
            clone.ExpiryDate = expiryDate;
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
                       
            clone.PremiumSettlementCurrency = premiumSettlementCurrency;
            clone.PremiumSettlementDate = premiumSettlementDate;
            clone.PremiumSettlementDaysOverride = premiumSettlementDaysOverride;
            clone.PremiumSettlementFXRate = premiumSettlementFXRate;

            clone.SalesCreditAmount = salesCreditAmount;
            clone.SalesCreditPercentage = salesCreditPercentage;
            clone.SalesCreditCurrency = salesCreditCurrency;
            clone.SalesCreditFXRate = salesCreditFXRate;
            
            clone.Multiplier = multiplier;
            clone.Contracts = contracts;
            clone.LotSize = lotSize;
            
            clone.NotionalMillions = notionalMillions;
            clone.NotionalCurrency = notionalCurrency;
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
            clone.HedgeType = hedgeType;

            if (this.Legs != null)
            {
                clone.Legs = new List<IOptionDetail>();
                foreach (var leg in this.Legs)
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
            StringBuilder builder = new StringBuilder("identifier: ");
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
            get
            {
                return status;
            }
            set
            {
                if (status != value)
                {
                    status = value;
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
                return tradeDate;
            }
            set
            {
                if (tradeDate != value)
                {
                    tradeDate = value;
                    NotifyPropertyChanged("TradeDate");
                }
            }
        }

        public DateTime ExpiryDate
        {
            get
            {
                return expiryDate;
            }
            set
            {
                if (expiryDate != value)
                {
                    expiryDate = value;
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
                return premiumSettlementCurrency;
            }
            set
            {
                if (premiumSettlementCurrency != value)
                {
                    premiumSettlementCurrency = value;
                    NotifyPropertyChanged("PremiumSettlementCurrency");                    
                }
            }
        }

        public DateTime PremiumSettlementDate
        {
            get
            {
                return premiumSettlementDate;
            }
            set
            {
                if (premiumSettlementDate != value)
                {
                    premiumSettlementDate = value;
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

        public decimal PremiumSettlementFXRate
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

        public decimal SalesCreditAmount
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

        public decimal SalesCreditPercentage
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
                return salesCreditCurrency;
            }
            set
            {
                if (salesCreditCurrency != value)
                {
                    salesCreditCurrency = value; 
                    NotifyPropertyChanged("SalesCreditCurrency");                    
                }
            }
        }

        public decimal SalesCreditFXRate
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

        public decimal Contracts
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

        public decimal Quantity
        {
            get
            {
                return contracts * multiplier;
            }
        }

        public decimal NotionalMillions
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

        public decimal NotionalFXRate
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
                return notionalCurrency;
            }
            set
            {
                if (notionalCurrency != value)
                {
                    notionalCurrency = value;
                    NotifyPropertyChanged("NotionalCurrency");
                }
            }
        }

        public decimal BidImpliedVol
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

        public decimal BidPremiumAmount
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

        public decimal BidPremiumPercentage
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

        public decimal BidFinalPercentage
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

        public decimal BidFinalAmount
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

        public decimal AskImpliedVol
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

        public decimal AskPremiumAmount
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

        public decimal AskPremiumPercentage
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


        public decimal AskFinalPercentage
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

        public decimal AskFinalAmount
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

        public decimal PremiumAmount
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

        public decimal PremiumPercentage
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

        public decimal ImpliedVol
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

        public decimal Delta
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

        public decimal DeltaNotional
        {
            get
            {
                return delta * notionalMillions * 1000000;
            }
        }

        public decimal DeltaShares
        {
            get
            {
                if (lotSize == 0)
                    return 0;

                return (Math.Floor(delta * Quantity) / lotSize) * lotSize;
            }
        }

        public decimal Gamma
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

        public decimal GammaNotional
        {
            get
            {
                return gamma * notionalMillions * 1000000;
            }
        }

        public decimal GammaShares
        {
            get
            {
                // TODO verify
                if (lotSize == 0)
                    return 0;
                   
                return (Math.Floor(gamma * Quantity) / lotSize) * lotSize;
            }
        }

        public decimal Theta
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

        public decimal ThetaNotional
        {
            get
            {
                return theta * notionalMillions * 1000000;
            }
        }

        public decimal ThetaShares
        {
            get
            {
                // TODO verify
                if (lotSize == 0)
                    return 0;

                return (Math.Floor(theta * Quantity) / lotSize) * lotSize;
            }
        }

        public decimal Vega
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

        public decimal VegaNotional
        {
            get
            {
                return vega * notionalMillions * 1000000;
            }
        }

        public decimal VegaShares
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

        public decimal Rho
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

        public decimal RhoNotional
        {
            get
            {
                return rho * notionalMillions * 1000000;
            }
        }

        public decimal RhoShares
        {
            get
            {
                // TODO verify
                if (lotSize == 0)
                    return 0;

                return (Math.Floor(rho * Quantity) / lotSize) * lotSize;
            }
        }

        public decimal HedgePrice
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
                return hedgeType;
            }
            set
            {
                if (hedgeType != value)
                {
                    hedgeType = value;
                    NotifyPropertyChanged("HedgeType");
                }
            }
        }

        public decimal TotalPremium
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
