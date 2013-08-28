using System.Collections.Generic;
using System.Collections.ObjectModel;
using RequestForQuoteInterfacesLibrary.ModelInterfaces;

namespace RequestForQuoteInterfacesLibrary.ServiceInterfaces
{
    public interface IClientManager
    {
        List<IClient> Clients { get; set; }
        void AddClient(string name, int tier, bool isValid, bool canSaveToDatabase);
        void RemoveClient(int identifier);
        void UpdateTier(int identifier, int tier);
        void UpdateValidity(int identifier, bool isValid);
    }
}