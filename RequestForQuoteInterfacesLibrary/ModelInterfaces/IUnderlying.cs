
namespace RequestForQuoteInterfacesLibrary.ModelInterfaces
{
    public interface IUnderlying
    {
        string RIC { get; set; }
        string Description { get; set; }
        bool IsValid { get; set; }
    }
}
