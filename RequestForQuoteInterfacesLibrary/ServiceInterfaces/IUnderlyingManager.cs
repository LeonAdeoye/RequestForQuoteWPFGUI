using System.Collections.Generic;
using RequestForQuoteInterfacesLibrary.ModelInterfaces;

namespace RequestForQuoteInterfacesLibrary.ServiceInterfaces
{
    public interface IUnderlyingManager
    {
        List<IUnderlyier> Underlyings { get; set; }
        void AddUnderlying(string ric, string description, bool isValid);
        bool SaveToDatabase(string ric, string description);
        bool UpdateValidity(string ric, bool isValid);
        void Initialize();
    }
}