using System.Collections.Generic;
using RequestForQuoteInterfacesLibrary.ModelInterfaces;

namespace RequestForQuoteInterfacesLibrary.ServiceInterfaces
{
    public interface IOptionRequestParser
    {
        bool IsValidOptionRequest(string request);
        void ParseOptionStrikes(string delimitedStrikes, List<IOptionDetail> optionLegs);
        void ParseOptionMaturityDates(string delimitedDates, List<IOptionDetail> optionLegs);
        void ParseOptionUnderlyings(string delimitedUnderlyings, List<IOptionDetail> optionLegs);
        List<IOptionDetail> ParseRequest(string request, IRequestForQuote parent);
        List<IOptionDetail> ParseOptionTypes(string request, IRequestForQuote parent);
    }
}