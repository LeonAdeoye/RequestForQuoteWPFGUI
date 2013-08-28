namespace RequestForQuoteInterfacesLibrary.ModelInterfaces
{
    public interface IProduct
    {
        int Identifier { get; set; }
        string Name { get; set; }
        string RIC { get; set; }
    }
}
