﻿using System;
using System.Collections;
using RequestForQuoteInterfacesLibrary.Enums;
using RequestForQuoteInterfacesLibrary.ModelInterfaces;
using RequestForQuoteInterfacesLibrary.ServiceInterfaces;
using RequestForQuoteServicesModuleLibrary.OptionPricerService;
using log4net;

namespace RequestForQuoteServicesModuleLibrary.ServicesImplementation
{
    sealed class OptionRequestPricerImpl : IOptionRequestPricer
    {
        private readonly OptionPricingControllerClient pricerProxy;
        private static readonly ILog log = LogManager.GetLogger( System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);


        public OptionRequestPricerImpl()
        {
            pricerProxy = new OptionPricingControllerClient();
        }

        private bool CanCalculatePricing(double strike, double volatility, double underlyingPrice, double interestRate, double daysToExpiry, double dayCountConvention)
        {
            return (strike > 0.0 && volatility > 0.0 && underlyingPrice > 0.0 && interestRate > 0.0 && daysToExpiry > 0.0 && dayCountConvention > 0.0);
        }

        public bool CalculatePricing(IOptionDetail optionToPrice)
        {
            if (optionToPrice == null)
                throw new ArgumentNullException("optionToPrice");

            try
            {
                if (CanCalculatePricing(optionToPrice.Strike, optionToPrice.Volatility, optionToPrice.UnderlyingPrice, optionToPrice.InterestRate, optionToPrice.DaysToExpiry, optionToPrice.DayCountConvention))
                {
                    if (log.IsDebugEnabled)
                    {
                        log.Debug("strike: " + optionToPrice.Strike + ", volatility: " + optionToPrice.Volatility + ", underlying price: " + optionToPrice.UnderlyingPrice
                            + ", interest rate: " + optionToPrice.InterestRate + ", days to expiry: " + optionToPrice.DaysToExpiry + ", DayCountConvention: "
                            + optionToPrice.DayCountConvention + ", Type: " + (optionToPrice.IsEuropean ? "European " : "American ") + (optionToPrice.IsCall ? "Call" : "Put"));
                    }
                    ProcessCalculatedResult(pricerProxy.calculate(optionToPrice.Strike, optionToPrice.Volatility, optionToPrice.UnderlyingPrice, optionToPrice.DaysToExpiry,
                        optionToPrice.InterestRate, optionToPrice.IsCall, optionToPrice.IsEuropean, optionToPrice.DayCountConvention), optionToPrice);
                }

                return true;
            }
            catch(Exception asyncException)
            {
                log.Error(asyncException.Message, asyncException);
                return false;
            }
        }

        public IEnumerable CalculatePricingRange(int requestId, string rangeVariable, double rangeMinimum, double rangeMaximum, double rangeIncrement)
        {
            throw new NotImplementedException();
        }

        private void ProcessCalculatedResult(optionPriceResult pricings, IOptionDetail optionToPrice)
        {
            try
            {              
                if (pricings != null)
                {
                    if (optionToPrice != null)
                    {
                        if (log.IsDebugEnabled)
                        {
                            log.Debug("delta: " + pricings.delta + ", gamma: " + pricings.gamma + ", theta: " + pricings.theta
                                + ", vega: " + pricings.vega + ", price: " + pricings.price + ", Quantity: " + optionToPrice.Quantity
                                + ", Type: " + (optionToPrice.IsCall ? "Call" : "Put"));
                        }
                        optionToPrice.PremiumAmount = pricings.price * optionToPrice.Quantity * (optionToPrice.Side == SideEnum.BUY ? -1 : 1);
                        optionToPrice.Delta = pricings.delta * optionToPrice.Quantity * (optionToPrice.Side == SideEnum.BUY ? 1 : -1);
                        optionToPrice.Gamma = optionToPrice.Quantity * pricings.gamma * (optionToPrice.Side == SideEnum.BUY ? 1 : -1); // long options => long gamma
                        optionToPrice.Theta = optionToPrice.Quantity * pricings.theta * (optionToPrice.Side == SideEnum.BUY ? 1 : -1); // long options => short theta
                        optionToPrice.Vega = optionToPrice.Quantity * pricings.vega * (optionToPrice.Side == SideEnum.BUY ? 1 : -1); // long options => long vega
                        optionToPrice.Rho = optionToPrice.Quantity * pricings.rho;

                        optionToPrice.ParentRequest.Delta += optionToPrice.Delta;
                        optionToPrice.ParentRequest.Gamma += optionToPrice.Gamma;
                        optionToPrice.ParentRequest.Vega += optionToPrice.Vega;
                        optionToPrice.ParentRequest.Theta += optionToPrice.Theta;
                        optionToPrice.ParentRequest.Rho += optionToPrice.Rho;
                        optionToPrice.ParentRequest.PremiumAmount += optionToPrice.PremiumAmount;
                    }
                }
            }
            catch (Exception asyncException)
            {
                log.Error(asyncException.Message, asyncException);
            }
        }
    }
}
