using RequestForQuoteInterfacesLibrary.ModelInterfaces;

namespace RequestForQuoteInterfacesLibrary.ModelImplementations
{
    public sealed class ProductImpl : IProduct
    {
        public int Identifier { get; set; }
        public string Name { get; set; }
        public string RIC { get; set; }
    }
}
