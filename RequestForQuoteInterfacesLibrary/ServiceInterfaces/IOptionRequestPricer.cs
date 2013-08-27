using RequestForQuoteInterfacesLibrary.ModelInterfaces;

namespace RequestForQuoteInterfacesLibrary.ServiceInterfaces
{
    public interface IOptionRequestPricer
    {
        bool CalculatePricing(IOptionDetail optionToPrice);
    }
}
