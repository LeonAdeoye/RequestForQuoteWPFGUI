using System.Collections.Generic;
using RequestForQuoteInterfacesLibrary.ModelImplementations;
using RequestForQuoteInterfacesLibrary.ModelInterfaces;

namespace RequestForQuoteInterfacesLibrary.ServiceInterfaces
{
    public interface IOptionRequestParser
    {
        bool IsValidOptionRequest(string request);
        void ParseOptionStrikes(string delimitedStrikes, List<OptionDetailImpl> optionLegs);
        void ParseOptionMaturityDates(string delimitedDates, List<OptionDetailImpl> optionLegs);
        void ParseOptionUnderlyings(string delimitedUnderlyings, List<OptionDetailImpl> optionLegs);
        List<OptionDetailImpl> ParseRequest(string request, IRequestForQuote parent);
        List<OptionDetailImpl> ParseOptionTypes(string request, IRequestForQuote parent);
    }
}