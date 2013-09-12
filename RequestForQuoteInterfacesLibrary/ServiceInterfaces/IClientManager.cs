using System.Collections.Generic;
using System.Collections.ObjectModel;
using RequestForQuoteInterfacesLibrary.ModelInterfaces;

namespace RequestForQuoteInterfacesLibrary.ServiceInterfaces
{
    public interface IClientManager
    {
        List<IClient> Clients { get; set; }
        bool AddClient(string name, int tier, bool isValid, bool canSaveToDatabase);
        bool RemoveClient(int identifier);
        bool UpdateTier(int identifier, int tier);
        bool UpdateValidity(int identifier, bool isValid);
    }
}