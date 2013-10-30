
namespace RequestForQuoteInterfacesLibrary.ModelInterfaces
{
    public interface IUnderlyier
    {
        string RIC { get; set; }
        string Description { get; set; }
        bool IsValid { get; set; }
    }
}
