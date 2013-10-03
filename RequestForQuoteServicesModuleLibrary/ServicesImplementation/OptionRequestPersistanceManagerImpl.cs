using System;
using System.Collections.Generic;
using System.ServiceModel;
using RequestForQuoteInterfacesLibrary.Constants;
using RequestForQuoteInterfacesLibrary.ModelImplementations;
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
            if (requestToSave == null)
                throw new ArgumentNullException("requestToSave");

			try
			{
				return requestControllerProxy.save(CreateServiceRequestFromRequestForQuote(requestToSave), 
                    RequestForQuoteConstants.MY_USER_NAME);
			}
			catch (FaultException fe)
			{
				log.Error("Failed to save request. Exception thrown: " , fe);
			}
            catch (EndpointNotFoundException enfe)
            {
                log.Error("Failed to save request. Exception thrown: ", enfe);
            }
            return -1;  
		}

		public bool UpdateRequest(IRequestForQuote requestToUpdate)
		{
            if (requestToUpdate == null)
                throw new ArgumentNullException("requestToUpdate");

			try
			{
                return requestControllerProxy.update(CreateServiceRequestFromRequestForQuote(requestToUpdate),
                    RequestForQuoteConstants.MY_USER_NAME);
			}
            catch (FaultException fe)
            {
                log.Error("Failed to update request. Exception thrown: ", fe);
            }
            catch (EndpointNotFoundException enfe)
            {
                log.Error("Failed to update request. Exception thrown: ", enfe);
            }
		    return false;
		}

		public IRequestForQuote GetRequest(int identifier, bool rePrice)
		{
            return CreateRequestForQuoteFromServiceRequest(requestControllerProxy.getRequest(identifier, rePrice));
		}

		public List<IRequestForQuote> GetRequestMatchingAdhocCriteria(ISearch search, bool rePrice)
		{
			throw new System.NotImplementedException();
		}

		public List<IRequestForQuote> GetRequestMatchingExistingCriteria(string criteriaOwner, string criteriaDescriptionKey, bool rePrice)
		{
			throw new System.NotImplementedException();
		}

        private IOptionDetail CreateRequestForQuoteLegFromServiceOptionLeg(optionDetailImpl serviceOptionLeg)
        {
            if (serviceOptionLeg == null)
                throw new ArgumentNullException("serviceOptionLeg");

            return new OptionDetailImpl()
                {
                    IsCall = serviceOptionLeg.isCall,
                    IsEuropean = serviceOptionLeg.isEuropean,
                    LegId = serviceOptionLeg.legId
                    // TODO add all properties
                };
        }

        private optionDetailImpl CreateServiceOptionLegFromRequestForQuoteLeg(IOptionDetail requestForQuoteOptionLeg)
        {
            if (requestForQuoteOptionLeg == null)
                throw new ArgumentNullException("requestForQuoteOptionLeg");

            return new optionDetailImpl()
                {
                    isCall = requestForQuoteOptionLeg.IsCall,
                    isEuropean = requestForQuoteOptionLeg.IsEuropean,
                    legId = requestForQuoteOptionLeg.LegId
                    // TODO add all properties
                };
        }

        private IRequestForQuote CreateRequestForQuoteFromServiceRequest(requestDetailImpl serviceRequest)
        {
            throw new System.NotImplementedException();
        }

        private requestDetailImpl CreateServiceRequestFromRequestForQuote(IRequestForQuote sourceRequestForQuote)
        {
            var serviceRequestToBeCreated = new requestDetailImpl();

            if (sourceRequestForQuote.Legs != null && sourceRequestForQuote.Legs.Count > 0)
            {
                var legsArray = new optionDetailImpl[sourceRequestForQuote.Legs.Count];
                var legCount = 0;
                foreach (var leg in sourceRequestForQuote.Legs)
                {
                    legsArray[legCount++] = CreateServiceOptionLegFromRequestForQuoteLeg(leg);
                }
                serviceRequestToBeCreated.legs = new optionDetailListImpl() { optionDetailList = legsArray };
            }

            serviceRequestToBeCreated.bookCode = sourceRequestForQuote.BookCode;
            serviceRequestToBeCreated.request = sourceRequestForQuote.Request;
            serviceRequestToBeCreated.identifier = sourceRequestForQuote.Identifier;
            serviceRequestToBeCreated.clientId = sourceRequestForQuote.Client.Identifier;
            serviceRequestToBeCreated.isOTC = sourceRequestForQuote.IsOTC;
            serviceRequestToBeCreated.status = sourceRequestForQuote.Status.ToString(); //6

            serviceRequestToBeCreated.tradeDate = sourceRequestForQuote.TradeDate;
            serviceRequestToBeCreated.expiryDate = sourceRequestForQuote.ExpiryDate; //8

            serviceRequestToBeCreated.lotSize = sourceRequestForQuote.LotSize;
            serviceRequestToBeCreated.multiplier = sourceRequestForQuote.Multiplier;
            serviceRequestToBeCreated.contracts = sourceRequestForQuote.Contracts;
            serviceRequestToBeCreated.quantity = sourceRequestForQuote.Quantity; //12

            serviceRequestToBeCreated.notionalMillions = sourceRequestForQuote.NotionalMillions;
            serviceRequestToBeCreated.notionalFXRate = sourceRequestForQuote.NotionalFXRate;
            serviceRequestToBeCreated.notionalCurrency = sourceRequestForQuote.NotionalCurrency.ToString(); //15

            serviceRequestToBeCreated.delta = sourceRequestForQuote.Delta;
            serviceRequestToBeCreated.gamma = sourceRequestForQuote.Gamma;
            serviceRequestToBeCreated.vega = sourceRequestForQuote.Vega;
            serviceRequestToBeCreated.theta = sourceRequestForQuote.Theta;
            serviceRequestToBeCreated.rho = sourceRequestForQuote.Rho; //20

            serviceRequestToBeCreated.deltaNotional = sourceRequestForQuote.DeltaNotional;
            serviceRequestToBeCreated.gammaNotional = sourceRequestForQuote.GammaNotional;
            serviceRequestToBeCreated.vegaNotional = sourceRequestForQuote.VegaNotional;
            serviceRequestToBeCreated.thetaNotional = sourceRequestForQuote.ThetaNotional;
            serviceRequestToBeCreated.rhoNotional = sourceRequestForQuote.RhoNotional; //25

            serviceRequestToBeCreated.deltaShares = sourceRequestForQuote.DeltaShares;
            serviceRequestToBeCreated.gammaShares = sourceRequestForQuote.GammaShares;
            serviceRequestToBeCreated.vegaShares = sourceRequestForQuote.VegaShares;
            serviceRequestToBeCreated.thetaShares = sourceRequestForQuote.ThetaShares;
            serviceRequestToBeCreated.rhoShares = sourceRequestForQuote.RhoShares; //30

            serviceRequestToBeCreated.askFinalAmount = sourceRequestForQuote.AskFinalAmount;
            serviceRequestToBeCreated.askFinalPercentage = sourceRequestForQuote.AskFinalPercentage;
            serviceRequestToBeCreated.askImpliedVol = sourceRequestForQuote.AskImpliedVol;
            serviceRequestToBeCreated.askPremiumAmount = sourceRequestForQuote.AskPremiumAmount;
            serviceRequestToBeCreated.askPremiumPercentage = sourceRequestForQuote.AskPremiumPercentage; //35

            serviceRequestToBeCreated.bidFinalAmount = sourceRequestForQuote.BidFinalAmount;
            serviceRequestToBeCreated.bidFinalPercentage = sourceRequestForQuote.BidFinalPercentage;
            serviceRequestToBeCreated.bidImpliedVol = sourceRequestForQuote.BidImpliedVol;
            serviceRequestToBeCreated.bidPremiumAmount = sourceRequestForQuote.BidPremiumAmount;
            serviceRequestToBeCreated.bidPremiumPercentage = sourceRequestForQuote.BidPremiumPercentage; //40

            serviceRequestToBeCreated.premiumAmount = sourceRequestForQuote.PremiumAmount;
            serviceRequestToBeCreated.premiumPercentage = sourceRequestForQuote.PremiumPercentage;
            serviceRequestToBeCreated.impliedVol = sourceRequestForQuote.ImpliedVol; //43

            serviceRequestToBeCreated.salesCreditAmount = sourceRequestForQuote.SalesCreditAmount;
            serviceRequestToBeCreated.salesCreditPercentage = sourceRequestForQuote.SalesCreditPercentage;
            serviceRequestToBeCreated.salesCreditCurrency = sourceRequestForQuote.SalesCreditCurrency.ToString();
            serviceRequestToBeCreated.salesCreditFXRate = sourceRequestForQuote.SalesCreditFXRate; //47

            serviceRequestToBeCreated.premiumSettlementCurrency = sourceRequestForQuote.PremiumSettlementCurrency.ToString();
            serviceRequestToBeCreated.premiumSettlementDate = sourceRequestForQuote.PremiumSettlementDate;
            serviceRequestToBeCreated.premiumSettlementDaysOverride = sourceRequestForQuote.PremiumSettlementDaysOverride;
            serviceRequestToBeCreated.premiumSettlementFXRate = sourceRequestForQuote.PremiumSettlementFXRate; //51

            serviceRequestToBeCreated.salesComment = sourceRequestForQuote.SalesComment;
            serviceRequestToBeCreated.traderComment = sourceRequestForQuote.TraderComment;
            serviceRequestToBeCreated.clientComment = sourceRequestForQuote.ClientComment; //54

            serviceRequestToBeCreated.hedgePrice = sourceRequestForQuote.HedgePrice;
            serviceRequestToBeCreated.hedgeType = sourceRequestForQuote.HedgeType.ToString();
            serviceRequestToBeCreated.totalPremium = sourceRequestForQuote.TotalPremium;
            serviceRequestToBeCreated.pickedUpBy = sourceRequestForQuote.PickedUpBy; //58

            return serviceRequestToBeCreated;
        }
	}
}
