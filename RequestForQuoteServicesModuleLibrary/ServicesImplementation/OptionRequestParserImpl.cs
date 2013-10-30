using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using Microsoft.Practices.ServiceLocation;
using RequestForQuoteInterfacesLibrary.Enums;
using RequestForQuoteInterfacesLibrary.ModelImplementations;
using RequestForQuoteInterfacesLibrary.ModelInterfaces;
using RequestForQuoteInterfacesLibrary.ServiceInterfaces;
using log4net;

namespace RequestForQuoteServicesModuleLibrary.ServicesImplementation
{
    public sealed class OptionRequestParserImpl : IOptionRequestParser
    {
        private static readonly ILog log = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private readonly IBankHolidayManager bankHolidayManager = ServiceLocator.Current.GetInstance<IBankHolidayManager>();

        public bool IsValidOptionRequest(string request)
        {
            var optionReg = new Regex(@"^([+-]?[\d]*[CP]{1}){1}([-+]{1}[\d]*[CP]{1})* ([\d]+){1}(,{1}[\d]+)* [\d]{1,2}(JAN|FEB|MAR|APR|MAY|JUN|JUL|AUG|SEP|OCT|NOV|DEC)20[\d]{2}(,{1}[\d]{1,2}(JAN|FEB|MAR|APR|MAY|JUN|JUL|AUG|SEP|OCT|NOV|DEC)20[\d]{2})* (\w){4,7}\.[A-Z]{1,2}(,{1}(\w){4,7}\.[A-Z]{1,2})*$", RegexOptions.IgnoreCase);
            return optionReg.Match(request).Success;
        }

        public void ParseOptionStrikes(string delimitedStrikes, List<IOptionDetail> optionLegs)
        {
            if (String.IsNullOrEmpty(delimitedStrikes))
                throw new ArgumentException("delimitedStrikes");

            if (optionLegs == null)
                throw new ArgumentNullException("optionLegs");

            var strikes = delimitedStrikes.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries);            
            if (strikes.Length == 1)
            {
                foreach (var optionLeg in optionLegs)                
                    optionLeg.Strike = Convert.ToDouble(strikes[0]);
            }
            else
            {
                var count = strikes.Length - 1;
                foreach (var optionLeg in optionLegs)
                    optionLeg.Strike = Convert.ToDouble(strikes[count--]);
            }
        }

        public void ParseOptionMaturityDates(string delimitedDates, List<IOptionDetail> optionLegs)
        {
            if (String.IsNullOrEmpty(delimitedDates))
                throw new ArgumentException("delimitedDates");

            if (optionLegs == null)
                throw new ArgumentNullException("optionLegs");

            var dates = delimitedDates.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries);
            if (dates.Length == 1)
            {
                foreach (var optionLeg in optionLegs)
                {
                    optionLeg.MaturityDate = Convert.ToDateTime(dates[0]);
                    optionLeg.TradeDate = DateTime.Today;
                    optionLeg.DaysToExpiry =  bankHolidayManager.CalculateBusinessDaysToExpiry(optionLeg.TradeDate, optionLeg.MaturityDate, LocationEnum.TOKYO);
                }
            }
            else
            {
                var count = 0;
                foreach (var optionLeg in optionLegs)
                {
                    optionLeg.MaturityDate = Convert.ToDateTime(dates[count++]);
                    optionLeg.TradeDate = DateTime.Today;
                    optionLeg.DaysToExpiry = bankHolidayManager.CalculateBusinessDaysToExpiry(optionLeg.TradeDate, optionLeg.MaturityDate, LocationEnum.TOKYO);
                }
            }
        }

        public void ParseOptionUnderlyings(string delimitedUnderlyings, List<IOptionDetail> optionLegs)
        {
            if (String.IsNullOrEmpty(delimitedUnderlyings))
                throw new ArgumentException("delimitedUnderlyings");

            if (optionLegs == null)
                throw new ArgumentNullException("optionLegs");

            var count = 0;
            var underlyings = delimitedUnderlyings.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries);
            if (underlyings.Length == 1)
            {
                foreach (var optionLeg in optionLegs)
                {
                    optionLeg.RIC = underlyings[0];
                    // TO-DO Add underlying manager
                    optionLeg.Volatility = 0.2;
                    optionLeg.UnderlyingPrice = 90;
                    optionLeg.IsEuropean = true;
                    optionLeg.InterestRate = 0.1;
                    optionLeg.DayCountConvention = OptionDetailImpl.DAY_COUNT_CONVENTION_250;
                }
            }
            else
            {
                count = 0;
                foreach (var optionLeg in optionLegs)
                {
                    optionLeg.RIC = underlyings[count++];
                    // TO-DO Add underlying manager
                    optionLeg.Volatility = 0.2;
                    optionLeg.UnderlyingPrice = 90;
                    optionLeg.IsEuropean = true;
                    optionLeg.InterestRate = 0.1;
                    optionLeg.DayCountConvention = OptionDetailImpl.DAY_COUNT_CONVENTION_250;
                }
            }
        }

        public List<IOptionDetail> ParseRequest(string request, IRequestForQuote parent)
        {
            if (String.IsNullOrEmpty(request))
                throw new ArgumentException("request");

            if (parent == null)
                throw new ArgumentNullException("parent");

            var partsOfTheRequest  = request.Split(new char[] {' '}, StringSplitOptions.RemoveEmptyEntries);
            var optionLegs = ParseOptionTypes(partsOfTheRequest[0], parent);
            ParseOptionStrikes(partsOfTheRequest[1], optionLegs);
            ParseOptionMaturityDates(partsOfTheRequest[2], optionLegs);
            ParseOptionUnderlyings(partsOfTheRequest[3], optionLegs);
            return optionLegs;
        }

        public List<IOptionDetail> ParseOptionTypes(string request, IRequestForQuote parent)
        {
            if (String.IsNullOrEmpty(request))
                throw new ArgumentException("request");

            if (parent == null)
                throw new ArgumentNullException("parent");

            var optionTypes = new List<IOptionDetail>();
            var optionDetailReg = new Regex(@"^(?<side>[+-])?(?<quantity>[1-9])?(?<type>[CP]{1})+");
            var optionLegReg = new Regex(@"^(?<leg>[+-]?[1-9]?[CP]{1})+");
            var matchedLegs = optionLegReg.Match(request);
            var legCount = 0;

            while (matchedLegs.Success)
            {
                var leg = matchedLegs.Groups["leg"].ToString();
                var matchedDetails = optionDetailReg.Match(leg);

                var side = matchedDetails.Groups["side"].Value == "-" ? SideEnum.SELL : SideEnum.BUY;
                var quantity = matchedDetails.Groups["quantity"].Value == "" ? 1 : Convert.ToInt32(matchedDetails.Groups["quantity"].Value);
                var isCall = matchedDetails.Groups["type"].Value == "C";

                optionTypes.Add(new OptionDetailImpl()
                {
                    Side = side,
                    Quantity = quantity,
                    IsCall = isCall,                    
                    LegId = ++legCount,
                    ParentRequest = parent
                });

                if (log.IsDebugEnabled)
                    log.Debug("leg #" + optionTypes.Count + ": " + leg + ", leg side: " + side + ", quantity: " + quantity + ", Type: " + (isCall ? "CALL" : "PUT"));

                request = request.Replace(leg, "");
                matchedLegs = optionLegReg.Match(request);
            }
            return optionTypes;
        }
    }
}
