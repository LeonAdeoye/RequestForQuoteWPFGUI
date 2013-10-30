using System.Collections.Generic;
using System.Collections.ObjectModel;
using RequestForQuoteInterfacesLibrary.ModelInterfaces;

namespace RequestForQuoteInterfacesLibrary.ServiceInterfaces
{
    public interface IUnderlyingManager
    {
        List<IUnderlyier> Underlyiers { get; set; }
        bool AddUnderlyier(string RIC, string description, bool isValid, bool saveToDatabase);
        bool RemoveUnderlyier(string RIC);
        void Initialize();
    }
}