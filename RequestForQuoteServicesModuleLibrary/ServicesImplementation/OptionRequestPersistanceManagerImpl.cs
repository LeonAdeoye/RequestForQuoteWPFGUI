using System;
using System.Collections.Generic;
using System.IO;
using System.ServiceModel;
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
		private readonly IConfigurationManager configManager;

		/// <summary>
		/// Constructor
		/// </summary>
		/// <param name="clientManager"> used to get client details for RFQs retrieved from the web service.</param>
		/// <param name="configManager"> used to obtain the current user in save/update operations.</param>
		/// <exception cref="ArgumentNullException"> thrown if either clientManager or configManager is null.</exception>
		public OptionRequestPersistanceManagerImpl(IClientManager clientManager, IConfigurationManager configManager)
		{
			if (configManager == null)
				throw new ArgumentNullException("configManager");

			if (clientManager == null)
				throw new ArgumentNullException("clientManager");

			this.clientManager = clientManager;
			this.configManager = configManager;
		}

		/// <summary>
		/// Saves an RFQ via the web service.
		/// </summary>
		/// <param name="requestToSave"> the rfq to save.</param>
		/// <returns> the request for quote ID if the RFQ is saved successfully, -1 otherwise.</returns>
		/// <exception cref="ArgumentNullException"> thrown if requestToSave is null.</exception>
		public int SaveRequest(IRequestForQuote requestToSave)
		{
			if (requestToSave == null)
				throw new ArgumentNullException("requestToSave");

			try
			{
				return requestControllerProxy.save(CreateServiceRequestFromRequestForQuote(requestToSave),
					configManager.CurrentUser);
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

		/// <summary>
		/// Updates an RFQ via the web service.
		/// </summary>
		/// <param name="requestToUpdate"> the rfq to update.</param>
		/// <returns> true if the RFQ is updated successfully, false otherwise.</returns>
		/// <exception cref="ArgumentNullException"> thrown if requestToSave is null.</exception>
		public bool UpdateRequest(IRequestForQuote requestToUpdate)
		{
			if (requestToUpdate == null)
				throw new ArgumentNullException("requestToUpdate");

			try
			{
				return requestControllerProxy.update(CreateServiceRequestFromRequestForQuote(requestToUpdate),
					configManager.CurrentUser);
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

		/// <summary>
		/// Gets today's RFQs via the web service.
		/// </summary>
		/// <param name="rePrice"> flag used to indicate whether the pricing should be recalculated based on current market prices.</param>
		/// <returns> the list of RFQs.</returns>
		public List<IRequestForQuote> GetRequestsForToday(bool rePrice)
		{
			var listOfRequests = new List<IRequestForQuote>();
			try
			{
				var requests = requestControllerProxy.getRequestsForToday(rePrice).requestDetailList;
				if (requests != null)
				{
					foreach (var request in requests)
						listOfRequests.Add(CreateRequestForQuoteFromServiceRequest(request));
				}
			}
			catch (EndpointNotFoundException enfe)
			{
				log.Error("Failed to get requests for today. Exception thrown: ", enfe);                
			}
			return listOfRequests;
		}

		/// <summary>
		/// Gets the RFQ matching the identifier.
		/// </summary>
		/// <param name="identifier"> the identifier of the RFQ.</param>
		/// <param name="rePrice"> flag used to indicate whether the pricing should be recalculated based on current market prices. </param>
		/// <returns> the RFQ returned by the web service call.</returns>
		public IRequestForQuote GetRequest(int identifier, bool rePrice)
		{
			return CreateRequestForQuoteFromServiceRequest(requestControllerProxy.getRequest(identifier, rePrice));
		}

		/// <summary>
		/// Gets the RFQs matching the adhoc search criteria via a web service call.
		/// </summary>
		/// <param name="search"> the search criteria</param>
		/// <param name="rePrice"> flag indicating whether the RFQs should be repriced using the latest market data.</param>
		/// <returns> the list of RFQs matching the criteria.</returns>
		public List<IRequestForQuote> GetRequestMatchingAdhocCriteria(ISearch search, bool rePrice)
		{
			var listOfRequests = new List<IRequestForQuote>();

			try
			{
				searchCriteriaImpl serviceCriteria = new searchCriteriaImpl
					{
						criteria = new searchCriterionImpl[search.Criteria.Count]
					};

				var index = 0;
				foreach (var criterion in search.Criteria)
				{
					var serviceCriterion = new RequestMaintenanceService.searchCriterionImpl
					{
						controlName = criterion.ControlName,
						controlValue = criterion.ControlValue
					};

					serviceCriteria.criteria[index++] = serviceCriterion;
				}

				var requests = requestControllerProxy.getRequestsMatchingAdhocCriteria(serviceCriteria, false);

				if (requests != null && requests.requestDetailList != null)
					foreach (var request in requests.requestDetailList)
						listOfRequests.Add(CreateRequestForQuoteFromServiceRequest(request));
			}
			catch (FaultException fe)
			{
				log.Error("Failed to complete adhoc search request. Exception thrown: ", fe);
			}
			catch (EndpointNotFoundException enfe)
			{
				log.Error("Failed to complete adhoc search request. Exception thrown: ", enfe);
			}

			return listOfRequests;
		}

		/// <summary>
		/// Retrieves the RFQs matching the previously saved criteria with the specified description key and belonging to the specified owner.
		/// Retrieves the RFQs from the database.
		/// </summary>
		/// <param name="criteriaOwner"> the owner of the previously saved set of criteria.</param>
		/// <param name="criteriaDescriptionKey"> the description key of the previously saved set of criteria.</param>
		/// <param name="rePrice"> used to indicate if the RFQs should be repriced with new market data.</param>
		/// <returns> returns the list of RFQs matching the previously saved criteria.</returns>
		/// <exception cref="ArgumentException"> thrown if either criteriaOwner or criteriaDescriptionKey is null or empty.</exception>
		public List<IRequestForQuote> GetRequestMatchingExistingCriteria(string criteriaOwner, string criteriaDescriptionKey, bool rePrice)
		{
			if(String.IsNullOrEmpty(criteriaOwner))
				throw new ArgumentException("criteriaOwner");

			if (String.IsNullOrEmpty(criteriaDescriptionKey))
				throw new ArgumentException("criteriaDescriptionKey");

			var listOfRequests = new List<IRequestForQuote>();
			try
			{
				var requests = requestControllerProxy.getRequestsMatchingExistingCriteria(criteriaOwner, criteriaDescriptionKey, rePrice);

				if (requests != null && requests.requestDetailList != null)
					foreach (var request in requests.requestDetailList)
						listOfRequests.Add(CreateRequestForQuoteFromServiceRequest(request));
			}
			catch (FaultException fe)
			{
				log.Error("Failed to complete existing search request. Exception thrown: ", fe);
			}
			catch (EndpointNotFoundException enfe)
			{
				log.Error("Failed to complete existing search request. Exception thrown: ", enfe);
			}
			return listOfRequests;
		}

		/// <summary>
		/// Converts web service format of the option leg of the RFQ into the GUI format.
		/// </summary>
		/// <param name="serviceOptionLeg"> the web service formatted RFQ's option leg to be converted.</param>
		/// <returns> the GUI formatted of the RFQ's option leg.</returns>
		/// <exception cref="ArgumentNullException"> thrown if the web service formatted RFQ's option leg is null.</exception>
		private IOptionDetail CreateRequestForQuoteLegFromServiceOptionLeg(optionDetailImpl serviceOptionLeg)
		{
			if (serviceOptionLeg == null)
				throw new ArgumentNullException("serviceOptionLeg");

		    DateTime maturityDateResult;
		    if(!DateTime.TryParse(serviceOptionLeg.maturityDate, out maturityDateResult))
                throw new InvalidDataException("maturityDate");

		    SideEnum sideResult;
            if(!Enum.TryParse(serviceOptionLeg.side, true, out sideResult))
                throw new InvalidDataException("side");

			return new OptionDetailImpl()
				{
					IsCall = serviceOptionLeg.isCall,
					IsEuropean = serviceOptionLeg.isEuropean,
					LegId = serviceOptionLeg.legId,
					Delta = serviceOptionLeg.delta,
					Gamma = serviceOptionLeg.gamma,
					Vega = serviceOptionLeg.vega,
					Theta = serviceOptionLeg.theta,
					Rho = serviceOptionLeg.rho,
					PremiumAmount = serviceOptionLeg.premium,

                    MaturityDate = maturityDateResult,
                    YearsToExpiry = serviceOptionLeg.yearsToExpiry,
                    Description = serviceOptionLeg.description,
                    Quantity = serviceOptionLeg.quantity,

					DayCountConvention = serviceOptionLeg.dayCountConvention,
					DaysToExpiry = serviceOptionLeg.daysToExpiry,
					UnderlyingPrice = serviceOptionLeg.underlyingPrice,
					Volatility = serviceOptionLeg.volatility,
					InterestRate = serviceOptionLeg.interestRate,
					RIC = serviceOptionLeg.underlyingRIC,
					Strike = serviceOptionLeg.strike,
					Side = sideResult
				};
		}

		/// <summary>
		/// Converts GUI formatted option leg of the RFQ into the web service format.
		/// </summary>
		/// <param name="requestForQuoteOptionLeg"> the GUI formatted RFQ's option leg to be converted.</param>
		/// <returns> the web service formatted RFQ's option leg.</returns>
		/// <exception cref="ArgumentNullException"> thrown if the GUI formatted RFQ's option leg is null.</exception>
		private optionDetailImpl CreateServiceOptionLegFromRequestForQuoteLeg(IOptionDetail requestForQuoteOptionLeg)
		{
			if (requestForQuoteOptionLeg == null)
				throw new ArgumentNullException("requestForQuoteOptionLeg");

			return new optionDetailImpl()
				{
					isCall = requestForQuoteOptionLeg.IsCall,
					isEuropean = requestForQuoteOptionLeg.IsEuropean,                    
					legId = requestForQuoteOptionLeg.LegId,
                    quantity = requestForQuoteOptionLeg.Quantity,
                    description = requestForQuoteOptionLeg.Description,
                    strikePercentage = requestForQuoteOptionLeg.StrikePercentage,
                    maturityDate = requestForQuoteOptionLeg.MaturityDate.ToShortDateString(),
                    yearsToExpiry = requestForQuoteOptionLeg.YearsToExpiry,
                    premiumPercentage = requestForQuoteOptionLeg.PremiumPercentage,
					delta = requestForQuoteOptionLeg.Delta,
					gamma = requestForQuoteOptionLeg.Gamma,
					vega = requestForQuoteOptionLeg.Vega,
					theta = requestForQuoteOptionLeg.Theta,
					rho = requestForQuoteOptionLeg.Rho,
					interestRate = requestForQuoteOptionLeg.InterestRate,
					volatility = requestForQuoteOptionLeg.Volatility,
					underlyingPrice = requestForQuoteOptionLeg.UnderlyingPrice,
					dayCountConvention = requestForQuoteOptionLeg.DayCountConvention,
					daysToExpiry = requestForQuoteOptionLeg.DaysToExpiry,
					premium = requestForQuoteOptionLeg.PremiumAmount,
					underlyingRIC = requestForQuoteOptionLeg.RIC,
					strike = requestForQuoteOptionLeg.Strike,
					side = requestForQuoteOptionLeg.Side.ToString(),                    
				};
		}

		/// <summary>
		/// Converts web service formatted RFQ into the GUI format.
		/// </summary>
		/// <param name="serviceRequest"> the web service formatted RFQ to be converted.</param>
		/// <returns> the GUI formatted RFQ.</returns>
		/// <exception cref="ArgumentNullException"> thrown if the web service formatted RFQ is null.</exception>
		private IRequestForQuote CreateRequestForQuoteFromServiceRequest(requestDetailImpl serviceRequest)
		{
			if (serviceRequest == null)
				throw new ArgumentNullException("serviceRequest");

			var requestForQuoteToCreate = new RequestForQuoteImpl();

			if (serviceRequest.legs.optionDetailList != null)
			{
				foreach (var leg in serviceRequest.legs.optionDetailList)
					requestForQuoteToCreate.Legs.Add(CreateRequestForQuoteLegFromServiceOptionLeg(leg));                
			}

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
			requestForQuoteToCreate.PremiumSettlementDate = Convert.ToDateTime(serviceRequest.premiumSettlementDate);
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

		/// <summary>
		/// Converts GUI formatted RFQ into the web service format.
		/// </summary>
		/// <param name="sourceRequestForQuote"> the GUI formatted RFQ to be converted.</param>
		/// <returns> the web service formatted RFQ.</returns>
		/// <exception cref="ArgumentNullException"> thrown if the GUI formatted RFQ is null.</exception>
		private requestDetailImpl CreateServiceRequestFromRequestForQuote(IRequestForQuote sourceRequestForQuote)
		{
			if(sourceRequestForQuote == null)
				throw new ArgumentNullException("sourceRequestForQuote");

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
