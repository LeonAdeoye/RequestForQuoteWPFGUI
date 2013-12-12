using System.Collections;
using RequestForQuoteInterfacesLibrary.ModelInterfaces;

namespace RequestForQuoteInterfacesLibrary.ServiceInterfaces
{
    public interface IOptionRequestPricer
    {
        bool CalculatePricing(IOptionDetail optionToPrice);
        IEnumerable CalculatePricingRange(int requestId, string inputType, double minimumInput, double maximumInput);
    }
}
