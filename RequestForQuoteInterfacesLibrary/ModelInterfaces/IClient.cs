namespace RequestForQuoteInterfacesLibrary.ModelInterfaces
{
    public interface IClient
    {
        int Identifier { get; set; }
        string Name { get; set; }
        bool IsValid { get; set; }
        string Tier { get; set; }
    }
}
