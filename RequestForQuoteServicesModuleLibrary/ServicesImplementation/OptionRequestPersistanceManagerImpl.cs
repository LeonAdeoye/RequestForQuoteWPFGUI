using System;
using System.Collections.Generic;
using System.ServiceModel;
using RequestForQuoteInterfacesLibrary.Constants;
using RequestForQuoteInterfacesLibrary.Enums;
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
	    private readonly IClientManager clientManager;

        public OptionRequestPersistanceManagerImpl(IClientManager clientManager)
        {
            this.clientManager = clientManager;
        }

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
            if (serviceRequest == null)
                throw new ArgumentNullException("serviceRequest");

            var requestForQuoteToCreate = new RequestForQuoteImpl();

            foreach(var leg in serviceRequest.legs.optionDetailList)
                requestForQuoteToCreate.Legs.Add(CreateRequestForQuoteLegFromServiceOptionLeg(leg));

            requestForQuoteToCreate.BookCode = serviceRequest.bookCode;
            requestForQuoteToCreate.Request = serviceRequest.request;
            requestForQuoteToCreate.Identifier = serviceRequest.identifier;
            requestForQuoteToCreate.Client = clientManager.GetClientWithMatchingIdentifier(serviceRequest.clientId);
            requestForQuoteToCreate.IsOTC = serviceRequest.isOTC;
            requestForQuoteToCreate.Status = (StatusEnum)Enum.Parse(typeof(StatusEnum), serviceRequest.status); //6

            requestForQuoteToCreate.TradeDate = System.Convert.ToDateTime(serviceRequest.tradeDate);
            requestForQuoteToCreate.ExpiryDate  = System.Convert.ToDateTime(serviceRequest.expiryDate); //8

            requestForQuoteToCreate.LotSize = serviceRequest.lotSize;
            requestForQuoteToCreate.Multiplier = serviceRequest.multiplier;
            requestForQuoteToCreate.Contracts = serviceRequest.contracts;

            requestForQuoteToCreate.NotionalMillions = serviceRequest.notionalMillions;
            requestForQuoteToCreate.NotionalFXRate = serviceRequest.notionalFXRate;
            requestForQuoteToCreate.NotionalCurrency = (CurrencyEnum)Enum.Parse(typeof(CurrencyEnum), serviceRequest.notionalCurrency); //15

            requestForQuoteToCreate.Delta = serviceRequest.delta;
            requestForQuoteToCreate.Gamma = serviceRequest.gamma;
            requestForQuoteToCreate.Vega = serviceRequest.vega;
            requestForQuoteToCreate.Theta = serviceRequest.theta;
            requestForQuoteToCreate.Rho = serviceRequest.rho; //20

            requestForQuoteToCreate.AskFinalAmount = serviceRequest.askFinalAmount;
            requestForQuoteToCreate.AskFinalPercentage = serviceRequest.askFinalPercentage;
            requestForQuoteToCreate.AskImpliedVol = serviceRequest.askImpliedVol;
            requestForQuoteToCreate.AskPremiumAmount = serviceRequest.askPremiumAmount;
            requestForQuoteToCreate.AskPremiumPercentage = serviceRequest.askPremiumPercentage; //35

            requestForQuoteToCreate.BidFinalAmount = serviceRequest.bidFinalAmount;
            requestForQuoteToCreate.BidFinalPercentage = serviceRequest.bidFinalPercentage;
            requestForQuoteToCreate.BidImpliedVol = serviceRequest.bidImpliedVol;
            requestForQuoteToCreate.BidPremiumAmount = serviceRequest.bidPremiumAmount;
            requestForQuoteToCreate.BidPremiumPercentage = serviceRequest.bidPremiumPercentage; //40

            requestForQuoteToCreate.PremiumAmount = serviceRequest.premiumAmount;
            requestForQuoteToCreate.PremiumPercentage = serviceRequest.premiumPercentage;
            requestForQuoteToCreate.ImpliedVol = serviceRequest.impliedVol; //43

            requestForQuoteToCreate.SalesCreditAmount = serviceRequest.salesCreditAmount;
            requestForQuoteToCreate.SalesCreditPercentage = serviceRequest.salesCreditPercentage;
            requestForQuoteToCreate.SalesCreditCurrency = (CurrencyEnum)Enum.Parse(typeof(CurrencyEnum), serviceRequest.salesCreditCurrency);
            requestForQuoteToCreate.SalesCreditFXRate = serviceRequest.salesCreditFXRate; //47

            requestForQuoteToCreate.PremiumSettlementCurrency = (CurrencyEnum)Enum.Parse(typeof(CurrencyEnum), serviceRequest.premiumSettlementCurrency);
            requestForQuoteToCreate.PremiumSettlementDate = System.Convert.ToDateTime(serviceRequest.premiumSettlementDate);
            requestForQuoteToCreate.PremiumSettlementDaysOverride = serviceRequest.premiumSettlementDaysOverride;
            requestForQuoteToCreate.PremiumSettlementFXRate = serviceRequest.premiumSettlementFXRate; //51

            requestForQuoteToCreate.SalesComment = serviceRequest.salesComment;
            requestForQuoteToCreate.TraderComment = serviceRequest.traderComment;
            requestForQuoteToCreate.ClientComment = serviceRequest.clientComment; //54

            requestForQuoteToCreate.HedgePrice = serviceRequest.hedgePrice;
            requestForQuoteToCreate.HedgeType = (HedgeTypeEnum) Enum.Parse(typeof(HedgeTypeEnum), serviceRequest.hedgeType);
            requestForQuoteToCreate.PickedUpBy = serviceRequest.pickedUpBy; //58

            return requestForQuoteToCreate;
        }

        private requestDetailImpl CreateServiceRequestFromRequestForQuote(IRequestForQuote sourceRequestForQuote)
        {
            var serviceRequestToCreate = new requestDetailImpl();

            if (sourceRequestForQuote.Legs != null && sourceRequestForQuote.Legs.Count > 0)
            {
                var legsArray = new optionDetailImpl[sourceRequestForQuote.Legs.Count];
                var legCount = 0;
                foreach (var leg in sourceRequestForQuote.Legs)
                {
                    legsArray[legCount++] = CreateServiceOptionLegFromRequestForQuoteLeg(leg);
                }
                serviceRequestToCreate.legs = new optionDetailListImpl() { optionDetailList = legsArray };
            }

            serviceRequestToCreate.bookCode = sourceRequestForQuote.BookCode;
            serviceRequestToCreate.request = sourceRequestForQuote.Request;
            serviceRequestToCreate.identifier = sourceRequestForQuote.Identifier;
            serviceRequestToCreate.clientId = sourceRequestForQuote.Client.Identifier;
            serviceRequestToCreate.isOTC = sourceRequestForQuote.IsOTC;
            serviceRequestToCreate.status = sourceRequestForQuote.Status.ToString(); //6

            serviceRequestToCreate.tradeDate = sourceRequestForQuote.TradeDate.ToShortDateString();
            serviceRequestToCreate.expiryDate = sourceRequestForQuote.ExpiryDate.ToShortDateString(); //8

            serviceRequestToCreate.lotSize = sourceRequestForQuote.LotSize;
            serviceRequestToCreate.multiplier = sourceRequestForQuote.Multiplier;
            serviceRequestToCreate.contracts = sourceRequestForQuote.Contracts;
            serviceRequestToCreate.quantity = sourceRequestForQuote.Quantity; //12

            serviceRequestToCreate.notionalMillions = sourceRequestForQuote.NotionalMillions;
            serviceRequestToCreate.notionalFXRate = sourceRequestForQuote.NotionalFXRate;
            serviceRequestToCreate.notionalCurrency = sourceRequestForQuote.NotionalCurrency.ToString(); //15

            serviceRequestToCreate.delta = sourceRequestForQuote.Delta;
            serviceRequestToCreate.gamma = sourceRequestForQuote.Gamma;
            serviceRequestToCreate.vega = sourceRequestForQuote.Vega;
            serviceRequestToCreate.theta = sourceRequestForQuote.Theta;
            serviceRequestToCreate.rho = sourceRequestForQuote.Rho; //20

            serviceRequestToCreate.deltaNotional = sourceRequestForQuote.DeltaNotional;
            serviceRequestToCreate.gammaNotional = sourceRequestForQuote.GammaNotional;
            serviceRequestToCreate.vegaNotional = sourceRequestForQuote.VegaNotional;
            serviceRequestToCreate.thetaNotional = sourceRequestForQuote.ThetaNotional;
            serviceRequestToCreate.rhoNotional = sourceRequestForQuote.RhoNotional; //25

            serviceRequestToCreate.deltaShares = sourceRequestForQuote.DeltaShares;
            serviceRequestToCreate.gammaShares = sourceRequestForQuote.GammaShares;
            serviceRequestToCreate.vegaShares = sourceRequestForQuote.VegaShares;
            serviceRequestToCreate.thetaShares = sourceRequestForQuote.ThetaShares;
            serviceRequestToCreate.rhoShares = sourceRequestForQuote.RhoShares; //30

            serviceRequestToCreate.askFinalAmount = sourceRequestForQuote.AskFinalAmount;
            serviceRequestToCreate.askFinalPercentage = sourceRequestForQuote.AskFinalPercentage;
            serviceRequestToCreate.askImpliedVol = sourceRequestForQuote.AskImpliedVol;
            serviceRequestToCreate.askPremiumAmount = sourceRequestForQuote.AskPremiumAmount;
            serviceRequestToCreate.askPremiumPercentage = sourceRequestForQuote.AskPremiumPercentage; //35

            serviceRequestToCreate.bidFinalAmount = sourceRequestForQuote.BidFinalAmount;
            serviceRequestToCreate.bidFinalPercentage = sourceRequestForQuote.BidFinalPercentage;
            serviceRequestToCreate.bidImpliedVol = sourceRequestForQuote.BidImpliedVol;
            serviceRequestToCreate.bidPremiumAmount = sourceRequestForQuote.BidPremiumAmount;
            serviceRequestToCreate.bidPremiumPercentage = sourceRequestForQuote.BidPremiumPercentage; //40

            serviceRequestToCreate.premiumAmount = sourceRequestForQuote.PremiumAmount;
            serviceRequestToCreate.premiumPercentage = sourceRequestForQuote.PremiumPercentage;
            serviceRequestToCreate.impliedVol = sourceRequestForQuote.ImpliedVol; //43

            serviceRequestToCreate.salesCreditAmount = sourceRequestForQuote.SalesCreditAmount;
            serviceRequestToCreate.salesCreditPercentage = sourceRequestForQuote.SalesCreditPercentage;
            serviceRequestToCreate.salesCreditCurrency = sourceRequestForQuote.SalesCreditCurrency.ToString();
            serviceRequestToCreate.salesCreditFXRate = sourceRequestForQuote.SalesCreditFXRate; //47

            serviceRequestToCreate.premiumSettlementCurrency = sourceRequestForQuote.PremiumSettlementCurrency.ToString();
            serviceRequestToCreate.premiumSettlementDate = sourceRequestForQuote.PremiumSettlementDate.ToShortDateString();
            serviceRequestToCreate.premiumSettlementDaysOverride = sourceRequestForQuote.PremiumSettlementDaysOverride;
            serviceRequestToCreate.premiumSettlementFXRate = sourceRequestForQuote.PremiumSettlementFXRate; //51

            serviceRequestToCreate.salesComment = sourceRequestForQuote.SalesComment;
            serviceRequestToCreate.traderComment = sourceRequestForQuote.TraderComment;
            serviceRequestToCreate.clientComment = sourceRequestForQuote.ClientComment; //54

            serviceRequestToCreate.hedgePrice = sourceRequestForQuote.HedgePrice;
            serviceRequestToCreate.hedgeType = sourceRequestForQuote.HedgeType.ToString();
            serviceRequestToCreate.totalPremium = sourceRequestForQuote.TotalPremium;
            serviceRequestToCreate.pickedUpBy = sourceRequestForQuote.PickedUpBy; //58

            return serviceRequestToCreate;
        }
	}
}
