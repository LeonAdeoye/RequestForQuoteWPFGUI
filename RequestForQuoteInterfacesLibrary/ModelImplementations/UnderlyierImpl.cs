using RequestForQuoteInterfacesLibrary.ModelInterfaces;

namespace RequestForQuoteInterfacesLibrary.ModelImplementations
{
    public sealed class UnderlyierImpl : IUnderlyier
    {
        public string RIC { get; set; }
        public string Description { get; set; }
        public bool IsValid { get; set; }
    }
}
