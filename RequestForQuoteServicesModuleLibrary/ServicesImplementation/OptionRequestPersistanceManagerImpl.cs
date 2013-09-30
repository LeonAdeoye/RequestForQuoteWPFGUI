using System;
using System.Collections.Generic;
using System.ServiceModel;
using RequestForQuoteInterfacesLibrary.Constants;
using RequestForQuoteInterfacesLibrary.ModelInterfaces;
using RequestForQuoteInterfacesLibrary.ServiceInterfaces;
using RequestForQuoteServicesModuleLibrary.RequestMaintenanceService;
using log4net;

namespace RequestForQuoteServicesModuleLibrary.ServicesImplementation
{
    public class OptionRequestPersistanceManagerImpl : IOptionRequestPersistanceManager
    {
        private static readonly ILog log = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);        
        private readonly RequestControllerClient requestControllerProxy  = new RequestControllerClient();

        public int SaveRequest(IRequestForQuote requestToSave)
        {
            var requestDetail = new requestDetailImpl();
            if(requestToSave.Legs != null && requestToSave.Legs.Count > 0)
            {
                var legsArray = new optionDetailImpl[requestToSave.Legs.Count];
                var legCount = 0;
                foreach(var leg in requestToSave.Legs)
                {
                    legsArray[legCount++] = new optionDetailImpl()
                        {
                            isCall = leg.IsCall,
                            legId = leg.LegId,
                            isEuropean = leg.IsEuropean
                            // TODO add more properties
                        };
                }
                requestDetail.legs = new optionDetailListImpl() {optionDetailList = legsArray};
            }

            requestDetail.bookCode = requestToSave.BookCode;
            requestDetail.request = requestToSave.Request;
			requestDetail.identifier =	requestToSave.Identifier; 			
			requestDetail.clientId = requestToSave.Client.Identifier;  
			requestDetail.isOTC =requestToSave.IsOTC; 
		    requestDetail.status = requestToSave.Status.ToString(); //6

			requestDetail.tradeDate = requestToSave.TradeDate; 
			requestDetail.expiryDate = requestToSave.ExpiryDate; //8
								
			requestDetail.lotSize =	requestToSave.LotSize;
			requestDetail.multiplier = requestToSave.Multiplier; 
			requestDetail.contracts = requestToSave.Contracts; 
			requestDetail.quantity = requestToSave.Quantity; //12
				
			requestDetail.notionalMillions = requestToSave.NotionalMillions;
			requestDetail.notionalFXRate = requestToSave.NotionalFXRate; 
			requestDetail.notionalCurrency = requestToSave.NotionalCurrency.ToString(); //15
				
			requestDetail.delta = requestToSave.Delta; 
			requestDetail.gamma = requestToSave.Gamma;
			requestDetail.vega = requestToSave.Vega; 
			requestDetail.theta = requestToSave.Theta; 
			requestDetail.rho =	requestToSave.Rho; //20
				
			requestDetail.deltaNotional = requestToSave.DeltaNotional; 
			requestDetail.gammaNotional = requestToSave.GammaNotional;
			requestDetail.vegaNotional = requestToSave.VegaNotional; 
			requestDetail.thetaNotional = requestToSave.ThetaNotional; 
			requestDetail.rhoNotional =	requestToSave.RhoNotional; //25
				
			requestDetail.deltaShares =	requestToSave.DeltaShares;
			requestDetail.gammaShares =	requestToSave.GammaShares; 
			requestDetail.vegaShares =	requestToSave.VegaShares; 
			requestDetail.thetaShares =	requestToSave.ThetaShares; 
			requestDetail.rhoShares	= requestToSave.RhoShares; //30
				
			requestDetail.askFinalAmount =	requestToSave.AskFinalAmount; 
			requestDetail.askFinalPercentage =	requestToSave.AskFinalPercentage; 
			requestDetail.askImpliedVol = requestToSave.AskImpliedVol; 
			requestDetail.askPremiumAmount = requestToSave.AskPremiumAmount;
			requestDetail.askPremiumPercentage = requestToSave.AskPremiumPercentage; //35
				
			requestDetail.bidFinalAmount =	requestToSave.BidFinalAmount; 
			requestDetail.bidFinalPercentage = requestToSave.BidFinalPercentage; 
			requestDetail.bidImpliedVol = requestToSave.BidImpliedVol;
			requestDetail.bidPremiumAmount = requestToSave.BidPremiumAmount; 
			requestDetail.bidPremiumPercentage = requestToSave.BidPremiumPercentage; //40
				
			requestDetail.premiumAmount = requestToSave.PremiumAmount; 
			requestDetail.premiumPercentage = requestToSave.PremiumPercentage;
			requestDetail.impliedVol = requestToSave.ImpliedVol; //43
				
			requestDetail.salesCreditAmount = requestToSave.SalesCreditAmount;
			requestDetail.salesCreditPercentage = requestToSave.SalesCreditPercentage;
			requestDetail.salesCreditCurrency =	requestToSave.SalesCreditCurrency.ToString();
			requestDetail.salesCreditFXRate = requestToSave.SalesCreditFXRate; //47
				
			requestDetail.premiumSettlementCurrency = requestToSave.PremiumSettlementCurrency.ToString();
			requestDetail.premiumSettlementDate = requestToSave.PremiumSettlementDate;
			requestDetail.premiumSettlementDaysOverride = requestToSave.PremiumSettlementDaysOverride;
			requestDetail.premiumSettlementFXRate =	requestToSave.PremiumSettlementFXRate; //51
				
			requestDetail.salesComment = requestToSave.SalesComment;
			requestDetail.traderComment = requestToSave.TraderComment;
			requestDetail.clientComment = requestToSave.ClientComment; //54
				
			requestDetail.hedgePrice =	requestToSave.HedgePrice;
			requestDetail.hedgeType = requestToSave.HedgeType.ToString();
			requestDetail.totalPremium = requestToSave.TotalPremium;
			requestDetail.pickedUpBy =	requestToSave.PickedUpBy; //58

            try
            {
                return requestControllerProxy.save(requestDetail, RequestForQuoteConstants.MY_USER_NAME);
            }
            catch (FaultException exception)
            {
                log.Error("Failed to save request. exception thrown: " , exception);
                return -1;
            }            
        }

        public bool UpdateRequest(IRequestForQuote requestToUpdate)
        {
            var requestDetail = new requestDetailImpl();
            if (requestToUpdate.Legs != null && requestToUpdate.Legs.Count > 0)
            {
                var legsArray = new optionDetailImpl[requestToUpdate.Legs.Count];
                var legCount = 0;
                foreach (var leg in requestToUpdate.Legs)
                {
                    legsArray[legCount++] = new optionDetailImpl()
                    {
                        isCall = leg.IsCall,
                        legId = leg.LegId,
                        isEuropean = leg.IsEuropean
                        // TODO add more properties
                    };
                }
                requestDetail.legs = new optionDetailListImpl() { optionDetailList = legsArray };
            }

            requestDetail.bookCode = requestToUpdate.BookCode;
            requestDetail.request = requestToUpdate.Request;
            requestDetail.identifier = requestToUpdate.Identifier;
            requestDetail.clientId = requestToUpdate.Client.Identifier;
            requestDetail.isOTC = requestToUpdate.IsOTC;
            requestDetail.status = requestToUpdate.Status.ToString(); //6

            requestDetail.tradeDate = requestToUpdate.TradeDate;
            requestDetail.expiryDate = requestToUpdate.ExpiryDate; //8

            requestDetail.lotSize = requestToUpdate.LotSize;
            requestDetail.multiplier = requestToUpdate.Multiplier;
            requestDetail.contracts = requestToUpdate.Contracts;
            requestDetail.quantity = requestToUpdate.Quantity; //12

            requestDetail.notionalMillions = requestToUpdate.NotionalMillions;
            requestDetail.notionalFXRate = requestToUpdate.NotionalFXRate;
            requestDetail.notionalCurrency = requestToUpdate.NotionalCurrency.ToString(); //15

            requestDetail.delta = requestToUpdate.Delta;
            requestDetail.gamma = requestToUpdate.Gamma;
            requestDetail.vega = requestToUpdate.Vega;
            requestDetail.theta = requestToUpdate.Theta;
            requestDetail.rho = requestToUpdate.Rho; //20

            requestDetail.deltaNotional = requestToUpdate.DeltaNotional;
            requestDetail.gammaNotional = requestToUpdate.GammaNotional;
            requestDetail.vegaNotional = requestToUpdate.VegaNotional;
            requestDetail.thetaNotional = requestToUpdate.ThetaNotional;
            requestDetail.rhoNotional = requestToUpdate.RhoNotional; //25

            requestDetail.deltaShares = requestToUpdate.DeltaShares;
            requestDetail.gammaShares = requestToUpdate.GammaShares;
            requestDetail.vegaShares = requestToUpdate.VegaShares;
            requestDetail.thetaShares = requestToUpdate.ThetaShares;
            requestDetail.rhoShares = requestToUpdate.RhoShares; //30

            requestDetail.askFinalAmount = requestToUpdate.AskFinalAmount;
            requestDetail.askFinalPercentage = requestToUpdate.AskFinalPercentage;
            requestDetail.askImpliedVol = requestToUpdate.AskImpliedVol;
            requestDetail.askPremiumAmount = requestToUpdate.AskPremiumAmount;
            requestDetail.askPremiumPercentage = requestToUpdate.AskPremiumPercentage; //35

            requestDetail.bidFinalAmount = requestToUpdate.BidFinalAmount;
            requestDetail.bidFinalPercentage = requestToUpdate.BidFinalPercentage;
            requestDetail.bidImpliedVol = requestToUpdate.BidImpliedVol;
            requestDetail.bidPremiumAmount = requestToUpdate.BidPremiumAmount;
            requestDetail.bidPremiumPercentage = requestToUpdate.BidPremiumPercentage; //40

            requestDetail.premiumAmount = requestToUpdate.PremiumAmount;
            requestDetail.premiumPercentage = requestToUpdate.PremiumPercentage;
            requestDetail.impliedVol = requestToUpdate.ImpliedVol; //43

            requestDetail.salesCreditAmount = requestToUpdate.SalesCreditAmount;
            requestDetail.salesCreditPercentage = requestToUpdate.SalesCreditPercentage;
            requestDetail.salesCreditCurrency = requestToUpdate.SalesCreditCurrency.ToString();
            requestDetail.salesCreditFXRate = requestToUpdate.SalesCreditFXRate; //47

            requestDetail.premiumSettlementCurrency = requestToUpdate.PremiumSettlementCurrency.ToString();
            requestDetail.premiumSettlementDate = requestToUpdate.PremiumSettlementDate;
            requestDetail.premiumSettlementDaysOverride = requestToUpdate.PremiumSettlementDaysOverride;
            requestDetail.premiumSettlementFXRate = requestToUpdate.PremiumSettlementFXRate; //51

            requestDetail.salesComment = requestToUpdate.SalesComment;
            requestDetail.traderComment = requestToUpdate.TraderComment;
            requestDetail.clientComment = requestToUpdate.ClientComment; //54

            requestDetail.hedgePrice = requestToUpdate.HedgePrice;
            requestDetail.hedgeType = requestToUpdate.HedgeType.ToString();
            requestDetail.totalPremium = requestToUpdate.TotalPremium;
            requestDetail.pickedUpBy = requestToUpdate.PickedUpBy; //58

            try
            {
                return requestControllerProxy.update(requestDetail, RequestForQuoteConstants.MY_USER_NAME);
            }
            catch (FaultException exception)
            {
                log.Error("Failed to update request. exception thrown: ", exception);
                return false;
            }
        }

        public IRequestForQuote GetRequest(int identifier, bool rePrice)
        {
            throw new System.NotImplementedException();
        }

        public List<IRequestForQuote> GetRequestMatchingAdhocCriteria(ISearch search, bool rePrice)
        {
            throw new System.NotImplementedException();
        }

        public List<IRequestForQuote> GetRequestMatchingExistingCriteria(string criteriaOwner, string criteriaDescriptionKey, bool rePrice)
        {
            throw new System.NotImplementedException();
        }
    }
}
